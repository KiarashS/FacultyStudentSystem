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
using System.Configuration;

namespace ContentManagementSystem.Web.Infrastructure
{
    public class ExternalResearchRecordsFetcher
    {
        public ExternalResearchRecordsFetcher() { }
        public async Task<ScopusHIndexViewModel> FetchScopusHindexAsync(string scopusLink, int professorId)
        {
            try
            {
                var responseStream = await GetHttpWebResponseAsync(scopusLink, "GET", true);

                if (responseStream.IsNull())
                    throw new ArgumentNullException("scopus responseStream is null in line of 25.");

                var SCSessionID = responseStream.Headers["setcookie"];

                var doc = GetHtmlAgilityDoc(responseStream);

                var ScopusInfo = new ScopusHIndexViewModel
                {
                    ScopusCitations = GetScopusTotalCitaions(doc),
                    ScopusDocuments = GetScopusDocuments(doc),
                    ScopusHIndex = GetScopusHindex(doc),
                    OtherNamesFormat = GetScopusOtherNames(doc),
                    ScopusTotalDocumentsCited = GetScopusTotalDocumentsCited(doc),
                    DocumentsCitation = await GetScopusDocumentsCitation(scopusLink, professorId, SCSessionID)
                };

                return ScopusInfo;
            }
            catch
            {
                throw;
            }
        }

        public async Task<GoogleHIndexViewModel> FetchGoogleHindex(string googleLink, int professorId)
        {
            try
            {

                var responseStream = await GetHttpWebResponseAsync(googleLink, "GET", true);

                if (responseStream.IsNull())
                    return null;

                var doc = GetHtmlAgilityDoc(responseStream);

                var googleInfo = new GoogleHIndexViewModel
                {
                    GoogleCitations = GetGoogleTotalCitaions(doc),
                    GoogleHIndex = GetGoogleHindex(doc),
                    DocumentsCitation = await GetGoogleDocumentsCitation(googleLink, professorId)
                };

                return googleInfo;
            }
            catch {
                return null;
            }
        }

        public async Task<HttpWebResponse> GetHttpWebResponseAsync(string link, string method,
            bool autoRedirect, CookieCollection sugar = null, int downloadTimeOut = 0)
        {
            //SCSessionID,AWSELB
            var request = (HttpWebRequest)WebRequest.Create(link);
            var uri = new Uri(link);
            request.CookieContainer = new CookieContainer();

            if (sugar.IsNotNull())
            {
                foreach (var cook in sugar)
                request.CookieContainer.Add((Cookie)cook);
            }

            request.UserAgent = ConstantsUtil.UserAgent;
            request.AllowAutoRedirect = autoRedirect;
            request.Method = method;
            request.KeepAlive = false;
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

            var timeOut = ConstantsUtil.ResponseStreamTimeout;
            if (downloadTimeOut != 0)
                timeOut = downloadTimeOut;

            var response = (HttpWebResponse)await request.GetResponseAsync().WithTimeout(timeOut);

            return response;
        }
        private bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
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

        private string GetScopusOtherNames(HtmlDocument doc)
        {
            var scopusOtherNames = string.Empty;
            var namesElements = doc.DocumentNode.SelectNodes("//div[@id='otherNameFormatBadges']/span");

            foreach (var name in namesElements)
            {
                scopusOtherNames += name.InnerText.Trim() + " | ";
            }

            return scopusOtherNames.Remove(scopusOtherNames.Length - 3, 3);
        }

        private async Task<IList<DocumentCitation>> GetScopusDocumentsCitation(string scopusLink, int professorId, string SCSessionID)
        {
            var authorId = scopusLink.Remove(0, scopusLink.IndexOf("Id=") + 3);

            var responseStreamBase = await GetHttpWebResponseAsync("https://www.scopus.com/author/highchart.uri?authorId=" + authorId, "GET", false);

            var responseStream = await GetHttpWebResponseAsync("https://www.scopus.com/author/highchart.uri?authorId=" + authorId, "GET", true, responseStreamBase.Cookies);
            if (responseStream.IsNull())
                return null;

            if(!responseStream.ContentType.Contains("json"))
                responseStream = await GetHttpWebResponseAsync("https://www.scopus.com/author/highchart.uri?authorId=" + authorId, "GET", true, responseStreamBase.Cookies);

            if (responseStream.IsNull())
                return null;
            var reader = new StreamReader(responseStream.GetResponseStream());
            var js = new JavaScriptSerializer();
            var objText = reader.ReadToEnd();
            
            var highChartJson = (HighchartObj)js.Deserialize(objText, typeof(HighchartObj));

            //var docMaxCount = (highChartJson.citeObj.Count > highChartJson.docObj.Count ? highChartJson.citeObj.Count : highChartJson.docObj.Count);
            var documentsCitation = new List<DocumentCitation>();
            for (int i = 0; i < highChartJson.citeObj.Count; i++)
            {
                var documentCitation = new DocumentCitation()
                {
                    Year = highChartJson.citeObj[i].x,
                    Citation = highChartJson.citeObj[i].y,
                    Source = DocSource.Scopus,
                    ProfessorId = professorId
                };
                documentsCitation.Add(documentCitation);
            };
            for (int i = 0; i < highChartJson.docObj.Count; i++)
            {
                var added = false;
                for (int j = 0; j < documentsCitation.Count && !added; j++)
                {
                    if (highChartJson.docObj[i].x == documentsCitation[j].Year)
                    {
                        documentsCitation[j].Document = highChartJson.docObj[i].y;
                        added = true;
                    }
                }
                if (!added)
                {
                    var documentCitation = new DocumentCitation()
                    {
                        Year = highChartJson.docObj[i].x,
                        Document = highChartJson.docObj[i].y,
                        Source = DocSource.Scopus,
                        ProfessorId = professorId
                    };
                    documentsCitation.Add(documentCitation);
                }
            };
            return documentsCitation;
        }

        private int? GetScopusTotalDocumentsCited(HtmlDocument doc)
        {
            var citationCntLnk = doc.DocumentNode.SelectSingleNode("//button[@id='citationCntLnk']/span").InnerText.Trim();

            var result = 0;

            if (int.TryParse(citationCntLnk, out result))
                return result;

            return null;
        }

        private int? GetScopusHindex(HtmlDocument doc)
        {
            var hSingleIndex = doc.DocumentNode.SelectSingleNode("//input[@id='hSingleIndexHidden']").Attributes["value"].Value.Trim();

            var result = 0;

            if (int.TryParse(hSingleIndex, out result))
                return result;

            return null;
        }

        private int? GetScopusDocuments(HtmlDocument doc)
        {
            var documentCount = doc.DocumentNode.SelectSingleNode("//input[@id='documentCountHidden']").Attributes["value"].Value.Trim();

            var result = 0;

            if (int.TryParse(documentCount, out result))
                return result;

            return null;
        }

        private int? GetScopusTotalCitaions(HtmlDocument doc)
        {
            var totalCiteCount = doc.DocumentNode.SelectSingleNode("//span[@id='totalCiteCount']").InnerText.Trim();

            var result = 0;

            if (int.TryParse(totalCiteCount, out result))
                return result;

            return null;
        }

        private async Task<IList<DocumentCitation>> GetGoogleDocumentsCitation(string googleLink, int professorId)
        {
            var responseStream = await GetHttpWebResponseAsync(googleLink + "&view_op=citations_histogram", "GET", true);

            if (responseStream.IsNull())
                return null;

            var doc = GetHtmlAgilityDoc(responseStream);
            //var docMaxCount = (highChartJson.citeObj.Count > highChartJson.docObj.Count ? highChartJson.citeObj.Count : highChartJson.docObj.Count);
            var documentsCitation = new List<DocumentCitation>();
            var years = doc.DocumentNode.SelectNodes("//span[@class='gsc_g_t']");
            foreach (var element in years)
            {
                var documentCitation = new DocumentCitation()
                {
                    Year = int.Parse(element.InnerText.Trim()),
                    Source = DocSource.Google,
                    ProfessorId = professorId
                };
                documentsCitation.Add(documentCitation);
            };
            var citations = doc.DocumentNode.SelectNodes("//a[@class='gsc_g_a']");
            var i = 0;
            foreach (var element in citations)
            {
                documentsCitation[i++].Citation = int.Parse(element.InnerText.Trim());
            };
            return documentsCitation;
        }

        private int? GetGoogleHindex(HtmlDocument doc)
        {
            var hSingleIndex = doc.DocumentNode.SelectSingleNode("//table[@id='gsc_rsb_st']/tbody/tr[2]/td[2]").InnerText.Trim();

            var result = 0;

            if (int.TryParse(hSingleIndex, out result))
                return result;

            return null;
        }

        private int? GetGoogleTotalCitaions(HtmlDocument doc)
        {
            var TotalCitaions = doc.DocumentNode.SelectSingleNode("//table[@id='gsc_rsb_st']/tbody/tr[1]/td[2]").InnerText.Trim();

            var result = 0;

            if (int.TryParse(TotalCitaions, out result))
                return result;

            return null;
        }
    }

    public class CiteObj
{
    public int x { get; set; }
    public int y { get; set; }
    public string Tooltip { get; set; }
    public string url { get; set; }
}

public class DocObj
{
    public int x { get; set; }
    public int y { get; set; }
    public string Tooltip { get; set; }
    public string url { get; set; }
}

public class HighchartObj
{
    public List<CiteObj> citeObj { get; set; }
    public int yAxiscitationhigh { get; set; }
    public List<DocObj> docObj { get; set; }
    public int yAxisdocshigh { get; set; }
    public int xAxisLow { get; set; }
    public int xAxisHigh { get; set; }
}
}