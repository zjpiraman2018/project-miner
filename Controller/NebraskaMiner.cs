using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;
using System.Globalization;
using ProjMiner.Model;
using System.Text.RegularExpressions;


namespace ProjMiner.Controller 
{
    public  class NebraskaMiner
    {

        private CookieContainer _CookieContainer = new CookieContainer();
        private string _Html;


        public string OutputPath { get; set; }
        public string Status { get; set; }
        public bool isComplete { get; set; }
        private string County;
        private string[] StateList;
        private string[] StateListWithSpace;

        private KeyValuePair<string,string> UserCredential;

        public class PartyRaw
        {
            public int Id { get; set; }
            public string Type { get; set; }
            public string PartyDesc { get; set; }
        }

        public bool isValid(string type, string text)
        {

            string zipStatePattern = @"(AL|AK|AS|AZ|AR|CA|CO|CT|DE|DC|FM|FL|GA|GU|HI|ID|IL|IN|IA|KS|KY|LA|ME|MH|MD|MA|MI|MN|MS|MO|MT|NE|NV|NH|NJ|NM|NY|NC|ND|MP|OH|OK|OR|PW|PA|PR|RI|SC|SD|TN|TX|UT|VT|VI|VA|WA|WV|WI|WY)+([ ]{1,})+([0-9]{5,})";
            //string addressPattern = @"\d+[ ](?:[A-Za-z0-9.-]+[ ]?)+(?:Avenue|Lane|Road|Boulevard|Drive|Street|Ave|Dr|Rd|Blvd|Ln|St)?";
            string addressPattern = @"(\d+[ ](?:[A-Za-z0-9.-]+[ ]?)+(?:Avenue|Lane|Road|Boulevard|Drive|Street|Ave|Dr|Rd|Blvd|Ln|St)?)|(?:Apt+[ ]+\d+)";

            if (text.Trim() == "") return false;

            if (type.ToUpper() != "STATE" && 
                (text.ToUpper().IndexOf("PO BOX") > -1 || text.ToUpper().IndexOf("P.O. BOX") > -1)) return true;

            string pattern = (type.ToUpper() != "STATE" ? addressPattern : zipStatePattern);
            // Here we call Regex.Match.
            Match match = Regex.Match(text, pattern,
                RegexOptions.IgnoreCase);

            // Here we check the Match instance.
            if (match.Success)
            {
                //// Finally, we get the Group value and display it.
                //string key = match.Groups[1].Value;
                //Console.WriteLine(key);
                return true;
            }

            return false;

        }

        public NebraskaCaseField parseHtml(string html)
        {
            NebraskaCaseField casefield = new NebraskaCaseField();

            HtmlAgilityPack.HtmlDocument htmldoc = new HtmlAgilityPack.HtmlDocument();
            htmldoc.LoadHtml(html);

            casefield.CourtType = "County Court of " + this.County + " County";
            // GET CASE DETAILS

            if (htmldoc.DocumentNode.SelectNodes("//h3[contains(.,'Case Summary')]/../parent::*[@class='panel panel-default']//pre[@class='notranslate']") == null) return null;

            var queryCaseDetail = (from action in htmldoc.DocumentNode.SelectNodes("//h3[contains(.,'Case Summary')]/../parent::*[@class='panel panel-default']//pre[@class='notranslate']").Cast<HtmlNode>()
                                   select action).FirstOrDefault();

            var CaseDetails = HtmlEntity.DeEntitize(queryCaseDetail.InnerText).Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            foreach (var item in CaseDetails)
            {

                if (item.Trim().StartsWith("This case is "))
                {
                    casefield.Status = item.Replace("This case is ", "").Trim().Split(' ')[0].ToString();
                }
                else if (item.Trim().StartsWith("Filed on "))
                {
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    casefield.FilingDate = DateTime.ParseExact(item.Replace("Filed on ", "").Trim(), "MM/dd/yyyy", provider).ToString("MMddyyyy");
                }
                else if (item.Trim().StartsWith("Classification: "))
                {
                    casefield.SuitCOA = item.Trim().Replace("Classification: ", "").Trim();
                }
                else if (item.Trim().StartsWith("In the") && item.Trim().EndsWith("of Douglas County"))
                {
                    //casefield.CourtType = item.Trim().Substring(6, item.Trim().Length - 6).Replace("of Douglas County", "").Trim();
                }
                else if (item.Trim().StartsWith("The Case ID is"))
                {
                    casefield.CaseNumber = item.Trim().Replace("The Case ID is", "").Trim();
                }
            }



            // GET CASE PARTIES
            int partyWidth = 0;
            //List<string> attorneyParties = new List<string>();
            List<string> parties = new List<string>();

            int x = 0;
            var queryParties = (from action in htmldoc.DocumentNode.SelectNodes("//h3[contains(.,'Parties/Attorneys to the Case')]/../parent::*[@class='panel panel-default']//pre[@class='notranslate']").Cast<HtmlNode>()
                                select action).FirstOrDefault();

            var CaseParties = HtmlEntity.DeEntitize(queryParties.InnerText).Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

            foreach (var item in CaseParties)
            {
                x += 1;
                if (item.Trim().StartsWith("Party") && item.Trim().EndsWith("Attorney") && (partyWidth == 0))
                {
                    partyWidth = item.Substring(0, item.IndexOf("Attorney")).Length;
                }
                else
                {
                    if (item.Trim() != "")
                    {
                        parties.Add(item.Substring(0, partyWidth));
                        //attorneyParties.Add(item.Substring(partyWidth, item.Length - partyWidth));
                    }
                }


            }

            bool isPlainTiff = false;
            bool isDefendant = false;
            bool isOther = false;

       

            List<PartyRaw> rawList = new List<PartyRaw>();


            int index = 0;
            foreach (var item in parties)
            {
                if (item.Trim() == "") continue;

                PartyRaw raw = new PartyRaw();

                if (item.ToUpper().IndexOf("PLAINTIFF") > -1) { isPlainTiff = true; isDefendant = false; isOther = false; index += 1; }
                else if (item.ToUpper().IndexOf("DEFENDANT") > -1) { isPlainTiff = false; isDefendant = true; isOther = false; index += 1; }
                else if (item.Trim().EndsWith("ACTIVE") || item.Trim() == "Witness DISMISSED") {
                    isPlainTiff = false; isDefendant = false; isOther = true; index += 1;
                }


                raw.Id = index;
                if (isDefendant)
                {
                    raw.Type = "DEFENDANT"; 
                    raw.PartyDesc = item.Trim();
                    rawList.Add(raw);
                }
                else if (isPlainTiff)
                {
                    raw.Type = "PLAINTIFF";
                    raw.PartyDesc = item.Trim();
                    rawList.Add(raw);
                }
                else if (isOther)
                {
                    //raw.Type = "ASSIGNEE";
                    //raw.PartyDesc = item.Trim();
                    //rawList.Add(raw);
                }

            }


            for (int i = 1; i <= index; i++)
            {
                NebraskaPartyField party = new NebraskaPartyField();
                var query = rawList.Where(a => a.Id == i).ToList();

                if (query.Count < 2) continue;

  
                party.Type = query[0].Type;
                party.Name = query[1].PartyDesc;

                int line = 0;
                int addressLine = 0;
                if(query.Count>2)
                    foreach (var item in query.Skip(2))
                    {
                        line += 1;
                        if (isValid("state", item.PartyDesc))
                        {
                            if(party.StateCityZip.Trim() == "")
                                party.StateCityZip = item.PartyDesc;

                            if(addressLine > 0 && (line - addressLine) == 2) {
                                party.Address = party.Address + " " + query[addressLine + 2].PartyDesc;
                            }

                        }
                        else if (isValid("address", item.PartyDesc))
                        {
                            addressLine = line;

                            if (party.Address.Trim() == "")
                            {
                                
                                party.Address = item.PartyDesc;
                            }
                            else
                            {
                                party.Address += " " + item.PartyDesc;
                            }
                                
                        }
                        else
                        {
                            if ((item.PartyDesc.IndexOf("owes") > -1 && item.PartyDesc.IndexOf("$") > -1) || item.PartyDesc.IndexOf("Alias is") > -1 || item.PartyDesc.Trim().StartsWith("Attn:")) continue;

                            if (item.PartyDesc.StartsWith("c/o") && party.Address == "") {
                                party.Name = party.Name + " " + item.PartyDesc;
                            }
                            else
                            {
                                continue;
                            }
                            
                        }
                    }

        
                casefield.NebraskaPartyField.Add(party);

            }


            //FOR PDF


            NebraskaPDFField pdfField = new NebraskaPDFField();

            var caseSummary = (from a in htmldoc.DocumentNode.SelectNodes("//h3[contains(.,'Case Summary')]/../..//div[@class='panel-body']").Cast<HtmlNode>()
                                   select a).FirstOrDefault();

            pdfField.CaseSummary = caseSummary.InnerText;

            var partyAttorney = (from a in htmldoc.DocumentNode.SelectNodes("//h3[contains(.,'Parties/Attorneys to the Case')]/../..//div[@class='panel-body']").Cast<HtmlNode>()
                                   select a).FirstOrDefault();

            pdfField.PartyAttorney = partyAttorney.InnerText;


            if (htmldoc.DocumentNode.SelectNodes("//h3[contains(.,'Judgment Information')]/../..//div[@class='panel-body']") != null)
            {
                var judgementInformation = (from a in htmldoc.DocumentNode.SelectNodes("//h3[contains(.,'Judgment Information')]/../..//div[@class='panel-body']").Cast<HtmlNode>()
                                            select a).FirstOrDefault();

                pdfField.JudgementInformation = judgementInformation.InnerText;
            }


            if (htmldoc.DocumentNode.SelectNodes("//h3[contains(.,'Court Costs Information')]/../..//table") != null)
            {
                var courtCostsInfoRows = (from a in htmldoc.DocumentNode.SelectNodes("//h3[contains(.,'Court Costs Information')]/../..//table//tr").Cast<HtmlNode>()
                                            select a).ToList();

                string tmpCostInfo = "";
                string tmpRow="<tr>{0}</tr>";
                foreach (var item in courtCostsInfoRows)
                {
                    tmpCostInfo += string.Format(tmpRow, item.InnerHtml.Replace("\t", "")
                        .Replace("\n", "").Replace("</th>", "</td>")
                        .Replace("<td>", "<td  style=\"border-bottom:1px solid #e0e0e0; font-size:11px;padding:8px\">")
                        .Replace("<th>", "<td  style=\"font-weight: bold;border-bottom:1px solid #e0e0e0; font-size:12px;padding:8px\">")
                        .Replace("<th class=\"text-right\">", "<td  style=\"text-align: right;font-weight: bold;border-bottom:1px solid #e0e0e0;border-right:1px solid #e0e0e0; font-size:12px;padding:8px\">")
                        .Replace("<td class=\"text-right\">", "<td  style=\"text-align: right;border-bottom:1px solid #e0e0e0;border-right:1px solid #e0e0e0; font-size:11px;padding:8px\">")
                        );
                   tmpCostInfo= tmpCostInfo.Replace("<tr><td  style=\"", "<tr><td  style=\"border-left:1px solid #e0e0e0;");
                }

                pdfField.CostInformation = tmpCostInfo;
            }


            if (htmldoc.DocumentNode.SelectNodes("//h3[contains(.,'Financial Activity')]/../..//div[@class='panel-body']") != null)
            {
                var financialActivity = (from a in htmldoc.DocumentNode.SelectNodes("//h3[contains(.,'Financial Activity')]/../..//div[@class='panel-body']").Cast<HtmlNode>()
                                         select a).FirstOrDefault();

                pdfField.FinancialActivity = financialActivity.InnerText;
            }
            else
            {
                throw new Exception();
            }




            if (htmldoc.DocumentNode.SelectNodes("//h3[contains(.,'Payments Made to the Court')]/../..//table") != null)
            {
                var paymentMadeToCourtRows = (from a in htmldoc.DocumentNode.SelectNodes("//h3[contains(.,'Payments Made to the Court')]/../..//table//tr").Cast<HtmlNode>()
                                          select a).ToList();

                string tmpPaymentMadeToCourtInfo = "";
                string tmpRow = "<tr>{0}</tr>";
                foreach (var item in paymentMadeToCourtRows)
                {
                    tmpPaymentMadeToCourtInfo += string.Format(tmpRow, item.InnerHtml.Replace("\t", "")
                        .Replace("\n", "").Replace("</th>", "</td>")
                        .Replace("<td>", "<td  style=\"border-bottom:1px solid #e0e0e0; font-size:11px;padding:8px\">")
                        .Replace("<th>", "<td  style=\"font-weight: bold;border-bottom:1px solid #e0e0e0; font-size:12px;padding:8px\">")
                        .Replace("<th class=\"text-right\">", "<td  style=\"text-align: right;font-weight: bold;border-bottom:1px solid #e0e0e0;border-right:1px solid #e0e0e0; font-size:12px;padding:8px\">")
                        .Replace("<td class=\"text-right\">", "<td  style=\"text-align: right;border-bottom:1px solid #e0e0e0;border-right:1px solid #e0e0e0; font-size:11px;padding:8px\">")
                        );
                    tmpPaymentMadeToCourtInfo = tmpPaymentMadeToCourtInfo.Replace("<tr><td  style=\"", "<tr><td  style=\"border-left:1px solid #e0e0e0;");
                }

                pdfField.PaymentsMadeToCourt = tmpPaymentMadeToCourtInfo;
            }


            if (htmldoc.DocumentNode.SelectNodes("//h3[contains(.,'Register of Actions')]/../..//div[@class='panel-body']") != null)
            {
                var registerOfAction = (from a in htmldoc.DocumentNode.SelectNodes("//h3[contains(.,'Register of Actions')]/../..//div[@class='panel-body']").Cast<HtmlNode>()
                                            select a).FirstOrDefault();

                pdfField.RegisterOfAction = registerOfAction.InnerText;
            }

            

            casefield.NebraskaPDFField = pdfField;
            
            return casefield;


        }

        public string createHTML(NebraskaPDFField field) {
            string htmlTemp = Settings1.Default.NebraskaHtmlTemplate;

            string tmpTable = "<table style='font-size:11px;'><tbody>{0}</tbody></table>";
            string tmpCaseSummaryContainer = "";
            var aCaseSummary = field.CaseSummary.Replace("\r", "").Split('\n');
            foreach (var item in aCaseSummary)
            {
                tmpCaseSummaryContainer += string.Format("<tr><td>{0}</td></tr>", item.Replace(" ","&nbsp;"));
                
            }

            string tmpPartyAttornelContainer = "";
            var aPartyAttorney = field.PartyAttorney.Replace("\r", "").Split('\n');
            foreach (var item in aPartyAttorney)
            {
                tmpPartyAttornelContainer += string.Format("<tr><td>{0}</td></tr>", item.Replace(" ", "&nbsp;&nbsp;"));

            }


            string tmpJudgementInfoContainer = "";
            var aJudgementInfo = field.JudgementInformation.Replace("\r", "").Split('\n');
            foreach (var item in aJudgementInfo)
            {
                
                tmpJudgementInfoContainer += string.Format("<tr><td>{0}</td></tr>", item.Replace(" ", "&nbsp;&nbsp;"));

            }


            string tmpFinancialActivityContainer = "";
            var aFinancialActivity = field.FinancialActivity.Replace("\r", "").Split('\n');
            foreach (var item in aFinancialActivity)
            {
                if(item.Trim() != "")
                tmpFinancialActivityContainer += string.Format("<tr><td>{0}</td></tr>", item.Trim().Replace("\t", "&nbsp;"));

            }

            string tmpRegisterOfAction = "";
            var aRegisterOfAction = field.RegisterOfAction.Replace("\r", "").Split('\n');
            foreach (var item in aRegisterOfAction)
            {
                //if (item.Trim().StartsWith("Image ID")) continue;
                tmpRegisterOfAction += string.Format("<tr><td>{0}</td></tr>", item.Replace(" ", "&nbsp;&nbsp;"));

            }

            return string.Format(htmlTemp, 
                string.Format(tmpTable, tmpCaseSummaryContainer),
                string.Format(tmpTable, tmpPartyAttornelContainer),
                string.Format(Settings1.Default.NebraskTableTemplate, "Judgment Information", string.Format(tmpTable,tmpJudgementInfoContainer)),
                string.Format(Settings1.Default.NebraskTableTemplate2, "Court Costs Information", field.CostInformation),
                string.Format(Settings1.Default.NebraskTableTemplate, "Financial Activity",string.Format(tmpTable, tmpFinancialActivityContainer)),
                string.Format(Settings1.Default.NebraskTableTemplate3, "Payments Made to the Court", field.PaymentsMadeToCourt),
                string.Format(Settings1.Default.NebraskTableTemplate, "Register of Actions", string.Format(tmpTable, tmpRegisterOfAction)));
        }



        public NebraskaCaseField Mine(string CountyNum, string CourtType, string CaseType, int CaseYear, string UserAccount, int CaseId, string outputPath = "")
        {
            this.OutputPath = outputPath;
            this.County = (CountyNum == "01" ? "Douglas" : "Lan Caster");

            UserCredential = NebraskaController.GetCredential(UserAccount);

            //_Html = HttpRequestGet(@"  https://www.nebraska.gov/justice/case.cgi", true);


            StringBuilder sb = new StringBuilder();


            sb.Clear();
            //string PostData = String.Format(@"search=1&from_case_search=1&court_type={0}&county_num=01&case_type={1}&case_year={2}&case_id={3}&client_data=&", CourtType, CaseType, CaseYear, ("0000000" + i.ToString()).Substring(("0000000" + i.ToString()).Length - 7, 7));
            sb.Append("search=1");
            sb.Append("&from_case_search=1");
            sb.Append("&court_type=" + CourtType);
            sb.Append("&county_num=" + CountyNum);
            sb.Append("&case_type=" + CaseType);
            sb.Append("&case_year=" + CaseYear.ToString());
            sb.Append("&case_id=" + ("0000000" + CaseId.ToString()).Substring(CaseId.ToString().Length, 7));
            sb.Append("&client_data=");
            sb.Append("&search=Search+Now");


         
            _Html = HttpRequestPost(@"https://www.nebraska.gov/justice/case.cgi", sb.ToString(), @"https://www.nebraska.gov/justice/case.cgi", true, false);
            if (_Html.IndexOf("No case information was available for the case you were trying to view") > -1)
            {
                return null;
            }

            if (_Html.IndexOf("Connection to the county server has timed out") > -1)
                return null;

            //File.WriteAllText(@"D:\Offsite\test nebraska\" + i.ToString() + ".html", _Html);
            var field = this.parseHtml(_Html);


            return field;

        }

     

        private string GetElementValueById(string Html, string Type, string Id)
        {
            HtmlAgilityPack.HtmlDocument htmldoc = new HtmlAgilityPack.HtmlDocument();
            htmldoc.LoadHtml(Html);

            var query = (from span in htmldoc.DocumentNode.SelectNodes("//" + Type + "[@id='" + Id + "']").Cast<HtmlNode>()
                         select span).ToList();
            return Uri.EscapeDataString(query[0].Attributes[3].Value);
        }


        private string HttpRequestPost(string Url, string PostData, string Referer = "", bool UseAuthenticaton = false,bool UseCookie = false)
        {
            int ReTryCount = 0;

        Retry:
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                string result = string.Empty;
                UTF8Encoding encoding = new UTF8Encoding();
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);

                byte[] bytedata = encoding.GetBytes(PostData);
                req.Method = "POST";
                req.Proxy = null;
                //req.KeepAlive = false;
                req.Proxy = null; // null;
                req.Accept = "text/html, application/xhtml+xml, */*";
                req.ContentType = "application/x-www-form-urlencoded";
                req.Headers.Add("Accept-Language: en-PH");
                //req.ServicePoint.UseNagleAlgorithm = false;
                //req.ServicePoint.ConnectionLimit = 100000;
                //req.ServicePoint.Expect100Continue = false;

                req.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:30.0) Gecko/20100101 Firefox/30.0";
                req.ContentLength = bytedata.Length;

      
                req.CookieContainer = _CookieContainer;
                

                req.Referer = Referer;
                req.Host = "www.nebraska.gov";

                if (UseAuthenticaton)
                {
                    //string authInfo = "cybers01:cfRfHRfR";
                    string authInfo = UserCredential.Key + ":" + UserCredential.Value;
                    authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                    req.Headers["Authorization"] = "Basic " + authInfo;
                }

                using (Stream stream = req.GetRequestStream())
                {
                    stream.Write(bytedata, 0, bytedata.Length);
                    using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        result = reader.ReadToEnd();
                        _CookieContainer.Add(response.Cookies); 
                    }
                }


                return result;
            }
            catch (Exception ex)
            {
                ReTryCount += 1;
                if (ReTryCount > 3) { return ""; }

                goto Retry;
            }

        }




        private string HttpRequestGet(string url, bool UseAuthenticaton = false)
        {
            int ReTryCount = 0;
        Retry:

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                string result = "";
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "Get";
                req.Proxy = null ; // null;
                req.Host = "www.nebraska.gov";
                req.CookieContainer = _CookieContainer;
                req.ServicePoint.ConnectionLimit = 100000;

                if (UseAuthenticaton)
                {

                    //string authInfo = "cybers01:cfRfHRfR";
                    string authInfo = UserCredential.Key + ":" + UserCredential.Value;
                    authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                    req.Headers["Authorization"] = "Basic " + authInfo;
                }

                using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader sr = new StreamReader(stream))
                {
                    result = sr.ReadToEnd();
                    //_CookieContainer.Add(response.Cookies);
                }
                    return result;
                
            }
            catch (Exception ex)
            {
                ReTryCount += 1;
                if (ReTryCount > 3) { throw new Exception(); }
                goto Retry;
            }
        }




    }
}
