//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Web;

//namespace Journals.Web.Utils
//{
//    class APIResult
//    {
//        public int Status { get; set; }
//        public String Info { get; set; }
//        public String Details { get; set; }
//    }

//    public class EmailVerify
//    {
//        const String APIURL = "http://www.email-validator.net/api/verify";
//        HttpClient client = new HttpClient();
//        String APIKey = "[your API key]";

//        public bool CheckEmailVerify(string email)
//        {
//            var postData = new List<KeyValuePair<string, string>>();
//            postData.Add(new KeyValuePair<string, string>("EmailAddress", email));
//            postData.Add(new KeyValuePair<string, string>("APIKey", APIKey));

//            HttpContent content = new FormUrlEncodedContent(postData);

//            HttpResponseMessage result = client.PostAsync(APIURL, content).Result;
//            string resultContent = result.Content.ReadAsStringAsync().Result;

//            APIResult res = new System.Web.Script.Serialization.JavaScriptSerializer().
//                Deserialize<APIResult>(resultContent);

//            switch (res.Status)
//            {
//                    // valid addresses have a {200, 207, 215} result code
//                    // result codes 114 and 118 need a retry
//                case 200:
//                case 207:
//                case 215:
//                    // Address is valid
//                    break;
//                case 114:
//                case 118:
//                    // retry
//                    break;
//                default:
//                    // Address is invalid
//                    // res.info
//                    // res.details
//                    break;
//            }
//        }
//    }
//}