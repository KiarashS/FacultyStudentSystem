using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using ContentManagementSystem.Web.Utils;
using HtmlAgilityPack;
using System.Text;
using ContentManagementSystem.DomainClasses;
using System.IO;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace ContentManagementSystem.Web.Infrastructure
{
    public class ArticlesFetcher
    {
        public ArticlesFetcher() { }
        public async Task<IList<ExternalResearchRecord>> SetProfessorOrcidArticles(string OrcidLink)
        {
            //OrcidLink = "http://orcid.org/0000-0003-1043-3814";
            //var response = await GetHttpWebResponseAsync(OrcidLink, "get", true);

            //if (response.IsNull())
            //    return null;

            //var doc = GetHtmlAgilityDoc(response);

            //var workIds = doc.DocumentNode.InnerHtml.Remove(0, doc.DocumentNode.InnerHtml.IndexOf("orcidVar.workIds"));
            //workIds = workIds.Remove(0, workIds.IndexOf("[") + 1);
            //workIds = workIds.Remove(workIds.IndexOf("]"));

            return await GetOrcidArticles(OrcidLink);
        }

        private async Task<IList<ExternalResearchRecord>> GetOrcidArticles(string OrcidLink)
        {
            var Articles = new List<ExternalResearchRecord>();
            var ArticleCount = 0;
            var offSet = 0;
            var link = OrcidLink + "/worksPage.json?offset=" + offSet + "&sort=date&sortAsc=false&_=1516551989572";
            do
            {
                offSet += 50;
                ArticleCount++;
                var JsonResponse = await GetHttpWebResponseAsync(link, "GET", true);
                if (JsonResponse.IsNull())
                    break;

                link = OrcidLink + "/worksPage.json?offset=" + offSet + "&sort=date&sortAsc=false&_=1516551989572";
                var articleResults = await AddOrcidArticles(JsonResponse);
                if (articleResults.IsNull() || articleResults.Count == 0)
                    break;

                Articles.AddRange(articleResults);
                continue;
            }
            while (true);
            return Articles;
        }

        private async Task<IList<ExternalResearchRecord>> AddOrcidArticles(HttpWebResponse jsonResponse)
        {
            using (var reader = new StreamReader(jsonResponse.GetResponseStream()))
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                var objText = reader.ReadToEnd();
                var OrcidObj = (OrcIdPublication)js.Deserialize(objText, typeof(OrcIdPublication));
                var Articles = new List<ExternalResearchRecord>();
                foreach (var item in OrcidObj.workGroups)
                {
                    var Article = new ExternalResearchRecord();
                    Article.Year = !string.IsNullOrEmpty(item.defaultWork?.publicationDate?.year) ? int.Parse(item.works[0]?.publicationDate.year) : (int?)null;
                    Article.Link = (string)item.defaultWork?.url;
                    Article.Doi = item.workExternalIdentifiers.Count == 0 ? null : item.workExternalIdentifiers[0].workExternalIdentifierId.value.Trim();
                    var identifierType = item.workExternalIdentifiers.Count == 0 ? null : item.workExternalIdentifiers[0].workExternalIdentifierType.value.Trim();
                    Article.Title = item.defaultWork.title.value.Trim();
                    if (identifierType.IsNotNullOrEmpty() && identifierType.Contains("doi", StringComparison.OrdinalIgnoreCase))
                    {
                        var crossrefResult = await SearchInCrossref(Article.Doi);
                        if (crossrefResult.IsNotNull())
                        {
                            Article.Authors = crossrefResult.Authors;
                            Article.Volume = crossrefResult.Volume;
                            Article.Journal = crossrefResult.Journal;
                            Article.Pages = crossrefResult.Pages;
                            Article.Issue = crossrefResult.Issue;
                        }
                    }
                    Articles.Add(Article);
                }
                return Articles;
            }
        }

        public async Task<IList<ExternalResearchRecord>> SetProfessorResIdArticles(string ResIdLink)
        {
            //OrcidLink = "http://orcid.org/0000-0003-1043-3814";
            var response = await GetHttpWebResponseAsync(ResIdLink, "GET", true);

            if (response.IsNull())
                return null;

            var doc = GetHtmlAgilityDoc(response);

            var privacySettings = doc.DocumentNode.SelectSingleNode("//td[@class='profileInnerBlue']").SelectSingleNode("input[@id='privacySettings']").Attributes["value"].Value.Trim();
            var researcherPk = doc.GetElementbyId("researcherPk").Attributes["value"].Value.Trim();
            var totalNumberOfResults = doc.GetElementbyId("totalNumberOfResults").Attributes["value"].Value.Trim();

            var postData = string.Format("researcherId={0}&listName={1}&privacySettings={2}&mode={3}&sortBy={4}&current.metadata.total={5}&current.metadata.size={6}&current.metadata.number=", researcherPk, "LIST1", privacySettings, "preview", "pubYear", totalNumberOfResults, "50");

            return await GetResIdArticles(ResIdLink, postData, totalNumberOfResults);
        }

        private async Task<IList<ExternalResearchRecord>> GetResIdArticles(string ResIdLink, string postData, string totalNumberOfResults)
        {
            var Articles = new List<ExternalResearchRecord>();
            for (int i = 1; i <= Math.Ceiling(double.Parse(totalNumberOfResults) / 50); i++)
            {
                var posData = postData + i.ToString();
                var response = await GetHttpWebResponseAsync("http://www.researcherid.com/WorkspacePaging.action", "POST", true, null, posData);

                if (response.IsNull())
                    return null;

                var doc = GetHtmlAgilityDoc(response);
                var articleElm = doc.DocumentNode.SelectNodes("//td[@class='summary_data']");
                var articleElm_2 = doc.DocumentNode.SelectNodes("//td[@class='summary_data_2']");

                var j = 0;
                foreach (var item in articleElm)
                {
                    try
                    {
                        var Article = new ExternalResearchRecord();
                        Article.Year = GetResIdYear(item, j);
                        //Article.Link = item.SelectSingleNode("input[@id='privacySettings']").Attributes["value"].Value.Trim();
                        Article.Doi = GetResIdDoi(item, j);
                        Article.Title = GetResIdTitle(item, j);
                        Article.Authors = GetResIdAuthors(item, j);
                        Article.Volume = GetResIdVolume(item, j);
                        Article.Journal = GetResIdJournal(item, j);
                        Article.Pages = GetResIdPages(item, j);
                        Article.Issue = GetResIdIssue(item, j);
                        Article.TimesCited = GetResIdTimesCited(item, j);
                        Articles.Add(Article);
                        j += 2;
                    }
                    catch
                    {
                        j += 2;
                        continue;
                    }
                }
                j = 1;
                foreach (var item in articleElm_2)
                {
                    try
                    {
                        var Article = new ExternalResearchRecord();
                        Article.Year = GetResIdYear(item, j);
                        //Article.Link = item.SelectSingleNode("input[@id='privacySettings']").Attributes["value"].Value.Trim();
                        Article.Doi = GetResIdDoi(item, j);
                        Article.Title = GetResIdTitle(item, j);
                        Article.Authors = GetResIdAuthors(item, j);
                        Article.Volume = GetResIdVolume(item, j);
                        Article.Journal = GetResIdJournal(item, j);
                        Article.Pages = GetResIdPages(item, j);
                        Article.Issue = GetResIdIssue(item, j);
                        Article.TimesCited = GetResIdTimesCited(item, j);
                        Articles.Add(Article);
                        j += 2;
                    }
                    catch
                    {
                        j += 2;
                        continue;
                    }
                }
            }
            return Articles;
        }

        private int? GetResIdTimesCited(HtmlNode item, int j)
        {
            try
            {
                return int.Parse(item.SelectSingleNode("input[@name='artifacts[" + j.ToString() + "].timesCited']").Attributes["value"].Value.Trim());
            }
            catch
            {
                return null;
            }
        }

        private string GetResIdIssue(HtmlNode item, int j)
        {
            try
            {
                return item.SelectSingleNode("input[@name='artifacts[" + j.ToString() + "].issue']").Attributes["value"].Value.Trim();
            }
            catch
            {
                return null;
            }
        }

        private string GetResIdPages(HtmlNode item, int j)
        {
            try
            {
                return item.SelectSingleNode("input[@name='artifacts[" + j.ToString() + "].bibPages']").Attributes["value"].Value.Trim();
            }
            catch
            {
                return null;
            }
        }

        private string GetResIdJournal(HtmlNode item, int j)
        {
            try
            {
                return item.SelectSingleNode("input[@name='artifacts[" + j.ToString() + "].sourceTitle']").Attributes["value"].Value.Trim();
            }
            catch
            {
                return null;
            }
        }

        private string GetResIdVolume(HtmlNode item, int j)
        {
            try
            {
                return item.SelectSingleNode("input[@name='artifacts[" + j.ToString() + "].volume']").Attributes["value"].Value.Trim();
            }
            catch
            {
                return null;
            }
        }

        private string GetResIdAuthors(HtmlNode item, int j)
        {
            try
            {
                return item.SelectSingleNode("input[@name='artifacts[" + j.ToString() + "].listOfAuthorsAsDescription']").Attributes["value"].Value.Trim();
            }
            catch
            {
                return null;
            }
        }

        private string GetResIdTitle(HtmlNode item, int j)
        {
            try
            {
                return item.SelectSingleNode("input[@name='artifacts[" + j.ToString() + "].itemTitle']").Attributes["value"].Value.Trim();
            }
            catch
            {
                return null;
            }
        }

        private string GetResIdDoi(HtmlNode item, int j)
        {
            try
            {
                return item.SelectSingleNode("input[@name='artifacts[" + j.ToString() + "].doi']").Attributes["value"].Value.Trim();
            }
            catch
            {
                return null;
            }
        }

        private int? GetResIdYear(HtmlNode item, int j)
        {
            try
            {
                var year = item.SelectSingleNode("input[@name='artifacts[" + j.ToString() + "].date']").Attributes["value"].Value.Trim();

                if (year.Length > 4)
                    return int.Parse(year.Remove(0, year.Length - 4));

                return int.Parse(year);
            }
            catch
            {
                return null;
            }
        }

        public async Task<HttpWebResponse> GetHttpWebResponseAsync(string link, string method,
            bool autoRedirect, string sugar = null, string postData = null, int downloadTimeOut = 0)
        {
            //SCSessionID,AWSELB
            var request = (HttpWebRequest)WebRequest.Create(link);
            var cook = new Cookie();
            var uri = new Uri(link);
            request.CookieContainer = new CookieContainer();
            request.UserAgent = ConstantsUtil.UserAgent;
            request.AllowAutoRedirect = autoRedirect;
            request.Method = method;
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9,fa;q=0.8");
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");

            var timeOut = ConstantsUtil.ResponseStreamTimeout;
            if (downloadTimeOut != 0)
                timeOut = downloadTimeOut;

            if (sugar.IsNotNullOrEmpty())
            {
                var name = sugar.Remove(0, sugar.IndexOf(":", StringComparison.OrdinalIgnoreCase) + 1);
                name = name.Remove(name.IndexOf("=", StringComparison.OrdinalIgnoreCase));
                cook.Name = name;
                cook.Value = sugar.Remove(0, sugar.IndexOf("=", StringComparison.OrdinalIgnoreCase) + 1);
                cook.Domain = uri.Host;
                request.CookieContainer.Add(cook);
            }

            if (postData.IsNotNullOrEmpty() && method == "POST")
            {
                request.ContentType = "application/x-www-form-urlencoded";
                var data = Encoding.ASCII.GetBytes(postData);
                request.ContentLength = data.Length;
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }

            var response = (HttpWebResponse)await request.GetResponseAsync().WithTimeout(timeOut);

            return response;
        }

        public HtmlDocument GetHtmlAgilityDoc(HttpWebResponse pageStream)
        {
            var doc = new HtmlDocument()
            {
                OptionCheckSyntax = true,
                OptionFixNestedTags = true,
                OptionAutoCloseOnEnd = true,
                OptionDefaultStreamEncoding = Encoding.UTF8
            };
            doc.Load(pageStream.GetResponseStream(), Encoding.UTF8);
            return doc;
        }

        public async Task<ExternalResearchRecord> SearchInCrossref(string doi)
        {
            try
            {
                var articleInfo = new ExternalResearchRecord();
                var response = await GetHttpWebResponseAsync("https://search.crossref.org/dois?q=" + doi + "&rows=3", "get", false);

                if (response.IsNull())
                    return null;

                var reader = response.GetResponseStream();

                if (reader != null)
                {
                    var bodyreader = new StreamReader(reader);

                    var data = bodyreader.ReadToEnd();
                    var result = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(data);

                    foreach (var value in result)
                    {
                        articleInfo.Doi = value["doi"];
                        articleInfo.Title = value["title"];

                        var fullCitation = value["fullCitation"];
                        var year = !string.IsNullOrWhiteSpace(value["year"]) ? value["year"] : "0";
                        articleInfo.Year = year.ToInt32();

                        if (fullCitation.Contains("<i>"))
                        {
                            var journal = fullCitation.Split("<i>");

                            if (journal.IsNotNullOrEmpty())
                            {
                                if (journal[1].LastIndexOf('<') > 0)
                                    articleInfo.Journal = journal[1].Remove(journal[1].LastIndexOf('<'));
                            }
                        }

                        if (fullCitation.Contains("<i>"))
                        {
                            var volume = fullCitation.Split("</i>");

                            if (volume.IsNotNullOrEmpty() && volume[1].IndexOf(',') >= 0)
                            {
                                var vol = volume[1].Remove(0, 1).Split(",");
                                foreach (var part in vol)
                                {
                                    if (part.Contains("vol", StringComparison.OrdinalIgnoreCase))
                                        articleInfo.Volume = part.Split(".")[1].Trim();

                                    if (part.Contains("no", StringComparison.OrdinalIgnoreCase))
                                        articleInfo.Issue = part.Split(".")[1].Trim();

                                    if (part.Contains("pp", StringComparison.OrdinalIgnoreCase))
                                        articleInfo.Pages = part.Split(".")[1].Trim();
                                }
                            }
                        }

                        if (year != "0")
                        {
                            var authors = fullCitation.Split(year);
                            if (authors[0].LastIndexOf(',') > 0)
                                articleInfo.Authors = authors[0].Remove(authors[0].LastIndexOf(',')).Trim();
                        }
                        else
                        {
                            var authors = fullCitation.Split("'");
                            if (authors[0].LastIndexOf(',') > 0)
                                articleInfo.Authors = authors[0].Remove(authors[0].LastIndexOf(',')).Trim();
                        }

                        if (articleInfo.Doi.Contains(doi, StringComparison.OrdinalIgnoreCase) ||
                            doi.Contains(articleInfo.Doi, StringComparison.OrdinalIgnoreCase))
                        {
                            return articleInfo;
                        }
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }


    public class Visibility
    {
        public List<object> errors { get; set; }
        public bool required { get; set; }
        public object getRequiredMessage { get; set; }
        public string visibility { get; set; }
    }

    public class PublicationDate
    {
        public List<object> errors { get; set; }
        public object month { get; set; }
        public object day { get; set; }
        public string year { get; set; }
        public bool required { get; set; }
        public object getRequiredMessage { get; set; }
    }

    public class PutCode
    {
        public List<object> errors { get; set; }
        public string value { get; set; }
        public bool required { get; set; }
        public object getRequiredMessage { get; set; }
    }

    public class JournalTitle
    {
        public List<object> errors { get; set; }
        public string value { get; set; }
        public bool required { get; set; }
        public object getRequiredMessage { get; set; }
    }

    public class WorkExternalIdentifierId
    {
        public List<object> errors { get; set; }
        public string value { get; set; }
        public bool required { get; set; }
        public object getRequiredMessage { get; set; }
    }

    public class WorkExternalIdentifierType
    {
        public List<object> errors { get; set; }
        public string value { get; set; }
        public bool required { get; set; }
        public object getRequiredMessage { get; set; }
    }

    public class Relationship
    {
        public List<object> errors { get; set; }
        public string value { get; set; }
        public bool required { get; set; }
        public object getRequiredMessage { get; set; }
    }

    public class WorkExternalIdentifier
    {
        public List<object> errors { get; set; }
        public WorkExternalIdentifierId workExternalIdentifierId { get; set; }
        public WorkExternalIdentifierType workExternalIdentifierType { get; set; }
        public object url { get; set; }
        public Relationship relationship { get; set; }
    }

    public class Title
    {
        public List<object> errors { get; set; }
        public string value { get; set; }
        public bool required { get; set; }
        public object getRequiredMessage { get; set; }
    }

    public class WorkType
    {
        public List<object> errors { get; set; }
        public string value { get; set; }
        public bool required { get; set; }
        public object getRequiredMessage { get; set; }
    }

    public class Work
    {
        public Visibility visibility { get; set; }
        public List<object> errors { get; set; }
        public PublicationDate publicationDate { get; set; }
        public PutCode putCode { get; set; }
        public object shortDescription { get; set; }
        public object url { get; set; }
        public JournalTitle journalTitle { get; set; }
        public object languageCode { get; set; }
        public object languageName { get; set; }
        public object citation { get; set; }
        public object countryCode { get; set; }
        public object countryName { get; set; }
        public object contributors { get; set; }
        public List<WorkExternalIdentifier> workExternalIdentifiers { get; set; }
        public string source { get; set; }
        public string sourceName { get; set; }
        public Title title { get; set; }
        public object subtitle { get; set; }
        public object translatedTitle { get; set; }
        public object workCategory { get; set; }
        public WorkType workType { get; set; }
        public object dateSortString { get; set; }
        public object createdDate { get; set; }
        public object lastModified { get; set; }
    }

    public class Visibility2
    {
        public List<object> errors { get; set; }
        public bool required { get; set; }
        public object getRequiredMessage { get; set; }
        public string visibility { get; set; }
    }

    public class PublicationDate2
    {
        public List<object> errors { get; set; }
        public object month { get; set; }
        public object day { get; set; }
        public string year { get; set; }
        public bool required { get; set; }
        public object getRequiredMessage { get; set; }
    }

    public class PutCode2
    {
        public List<object> errors { get; set; }
        public string value { get; set; }
        public bool required { get; set; }
        public object getRequiredMessage { get; set; }
    }

    public class JournalTitle2
    {
        public List<object> errors { get; set; }
        public string value { get; set; }
        public bool required { get; set; }
        public object getRequiredMessage { get; set; }
    }

    public class WorkExternalIdentifierId2
    {
        public List<object> errors { get; set; }
        public string value { get; set; }
        public bool required { get; set; }
        public object getRequiredMessage { get; set; }
    }

    public class WorkExternalIdentifierType2
    {
        public List<object> errors { get; set; }
        public string value { get; set; }
        public bool required { get; set; }
        public object getRequiredMessage { get; set; }
    }

    public class Relationship2
    {
        public List<object> errors { get; set; }
        public string value { get; set; }
        public bool required { get; set; }
        public object getRequiredMessage { get; set; }
    }

    public class WorkExternalIdentifier2
    {
        public List<object> errors { get; set; }
        public WorkExternalIdentifierId2 workExternalIdentifierId { get; set; }
        public WorkExternalIdentifierType2 workExternalIdentifierType { get; set; }
        public object url { get; set; }
        public Relationship2 relationship { get; set; }
    }

    public class Title2
    {
        public List<object> errors { get; set; }
        public string value { get; set; }
        public bool required { get; set; }
        public object getRequiredMessage { get; set; }
    }

    public class WorkType2
    {
        public List<object> errors { get; set; }
        public string value { get; set; }
        public bool required { get; set; }
        public object getRequiredMessage { get; set; }
    }

    public class DefaultWork
    {
        public Visibility2 visibility { get; set; }
        public List<object> errors { get; set; }
        public PublicationDate2 publicationDate { get; set; }
        public PutCode2 putCode { get; set; }
        public object shortDescription { get; set; }
        public object url { get; set; }
        public JournalTitle2 journalTitle { get; set; }
        public object languageCode { get; set; }
        public object languageName { get; set; }
        public object citation { get; set; }
        public object countryCode { get; set; }
        public object countryName { get; set; }
        public object contributors { get; set; }
        public List<WorkExternalIdentifier2> workExternalIdentifiers { get; set; }
        public string source { get; set; }
        public string sourceName { get; set; }
        public Title2 title { get; set; }
        public object subtitle { get; set; }
        public object translatedTitle { get; set; }
        public object workCategory { get; set; }
        public WorkType2 workType { get; set; }
        public object dateSortString { get; set; }
        public object createdDate { get; set; }
        public object lastModified { get; set; }
    }

    public class WorkExternalIdentifierId3
    {
        public List<object> errors { get; set; }
        public string value { get; set; }
        public bool required { get; set; }
        public object getRequiredMessage { get; set; }
    }

    public class WorkExternalIdentifierType3
    {
        public List<object> errors { get; set; }
        public string value { get; set; }
        public bool required { get; set; }
        public object getRequiredMessage { get; set; }
    }

    public class Relationship3
    {
        public List<object> errors { get; set; }
        public string value { get; set; }
        public bool required { get; set; }
        public object getRequiredMessage { get; set; }
    }

    public class WorkExternalIdentifier3
    {
        public List<object> errors { get; set; }
        public WorkExternalIdentifierId3 workExternalIdentifierId { get; set; }
        public WorkExternalIdentifierType3 workExternalIdentifierType { get; set; }
        public object url { get; set; }
        public Relationship3 relationship { get; set; }
    }

    public class WorkGroup
    {
        public List<Work> works { get; set; }
        public int activePutCode { get; set; }
        public DefaultWork defaultWork { get; set; }
        public int groupId { get; set; }
        public string activeVisibility { get; set; }
        public bool userVersionPresent { get; set; }
        public List<WorkExternalIdentifier3> workExternalIdentifiers { get; set; }
    }

    public class OrcIdPublication
    {
        public int nextOffset { get; set; }
        public int totalGroups { get; set; }
        public List<WorkGroup> workGroups { get; set; }
    }
}