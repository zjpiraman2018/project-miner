using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using HtmlAgilityPack;
using ProjMiner.Model;

namespace ProjMiner.Controller
{
    public  class FresnoMiner
    {


        private CookieContainer _CookieContainer = new CookieContainer();
        private string _Html;
        private string cookieHeader = "";
        public string Status { get; set; }
        public bool isComplete { get; set; }
        public bool isProxyEnable { get; set; }

        private List<KeyValuePair<string,string>> CourtDunsList { get; set; }
        
        
        
        public FresnoBaseField ParseHtml(string html, int recordNo)
        {
           // File.WriteAllText(Path.Combine (Environment.GetFolderPath(Environment.SpecialFolder.Desktop),recordNo.ToString()+".html"),html);
        	FresnoBaseField field = new FresnoBaseField();
        	HtmlAgilityPack.HtmlDocument htmldoc = new HtmlAgilityPack.HtmlDocument();
            htmldoc.LoadHtml(html);
        


            field.Title = HtmlEntity.DeEntitize((from a  in htmldoc.DocumentNode.SelectNodes("//h3").Cast<HtmlNode>() select a).ToList().FirstOrDefault().InnerText);
            
            field.FilingNo = (from a  in htmldoc.DocumentNode.SelectNodes("//div[@id='divCaseInformation_body']/div[1]/p[contains(.,'Case Number')]")
                              .Cast<HtmlNode>() select a).ToList().FirstOrDefault().InnerText.Replace("Case Number","").Replace(":","").Trim();
            
            field.Status =  (from a  in htmldoc.DocumentNode.SelectNodes("//div[@id='divCaseInformation_body']/div[1]/p[contains(.,'Case Status')]")
                             .Cast<HtmlNode>() select a).ToList().FirstOrDefault().InnerText.Replace("Case Status","").Replace(":","").Trim();

            field.Court = (from a in htmldoc.DocumentNode.SelectNodes("//div[@id='divCaseInformation_body']/div[1]/p[contains(.,'Court')]")
                 .Cast<HtmlNode>() select a).ToList().FirstOrDefault().InnerText.Replace("Court", "").Replace(":", "").Trim();



            field.FilingDate =  (from a  in htmldoc.DocumentNode.SelectNodes("//div[@id='divCaseInformation_body']/div[1]/p[contains(.,'File Date')]")
                                 .Cast<HtmlNode>() select a).ToList().FirstOrDefault().InnerText.Replace("File Date","").Replace(":","").Trim();
            
            field.CauseOfActionDescription =  (from a  in htmldoc.DocumentNode.SelectNodes("//div[@id='divCaseInformation_body']/div[1]/p[contains(.,'Case Type')]")
                                 .Cast<HtmlNode>() select a).ToList().FirstOrDefault().InnerText.Replace("Case Type","").Replace(":","").Trim();

            field.CauseOfActionDescription = HtmlEntity.DeEntitize(field.CauseOfActionDescription);

            if(field.Status.Trim().ToUpper() == "OPEN"){
            	field.StatusDate =  field.FilingDate.Replace("/","");
            	field.Status = "PENDING";
            }
            
            
            field.CourtDuns =  GetCourtDuns(field.FilingNo);
            field.DescCode = "CASE NUMBER";
            field.FilingType = "S";
            field.CollectedDate = DateTime.Now.ToString("MMddyyyy");
            field.CauseOfAction1 = "999";
            field.RecordNo = (recordNo + 1).ToString();


            if (htmldoc.DocumentNode.SelectNodes("//div[@id='divPartyInformation_body']//div[@class='tyler-layout-fluid tyler-cols-3']") == null)
            {
                field.partyRawHTML = "";
            }
            else
            {
                var partyQuery = (from a in htmldoc.DocumentNode
                                 .SelectNodes("//div[@id='divPartyInformation_body']//div[@class='tyler-layout-fluid tyler-cols-3']")
                                  .Cast<HtmlNode>()
                                  select a).ToList();


                string cPartyRawHtml = (from a in htmldoc.DocumentNode.SelectNodes("//div[@id='divPartyInformation_body']")
                                   .Cast<HtmlNode>()
                                        select a).ToList().FirstOrDefault().OuterHtml;


                HtmlAgilityPack.HtmlDocument htmldoc2 = new HtmlAgilityPack.HtmlDocument();
                htmldoc2.LoadHtml(cPartyRawHtml);

                var queryParty = (from a in htmldoc2.DocumentNode.SelectNodes("/div/div")
                                    .Cast<HtmlNode>()
                                  select a).ToList();

                foreach (var div in queryParty)
                {
                    HtmlAgilityPack.HtmlDocument htmldoc3 = new HtmlAgilityPack.HtmlDocument();
                    htmldoc3.LoadHtml(div.OuterHtml);

                    var queryParty2 = (from a in htmldoc3.DocumentNode.SelectNodes("/div/div")
                                    .Cast<HtmlNode>()
                                       select a).ToList();

                    string tmpPartyName = "";
                    string tmpPartyType = "";

                    if (queryParty2.Count == 2)
                    {
                        if (queryParty2[0].InnerText.Trim() == "")
                        {
                            tmpPartyName = queryParty2[1].InnerHtml.Replace("<hr>", "");
                        }
                        else
                        {
                            tmpPartyName = queryParty2[0].InnerHtml.Replace("<hr>", "");
                            tmpPartyType = queryParty2[1].InnerHtml.Replace("<hr>", "");
                        }
                    }
                    if (queryParty2.Count == 3)
                    {
                        tmpPartyName = queryParty2[1].InnerHtml.Replace("<hr>", "");
                        tmpPartyType = queryParty2[2].InnerHtml.Replace("<hr>", "");
                    }

                    field.partyRawHTML += string.Format("<tr><td style='width:50%;vertical-align: top;'>{0}</td><td style='width:50%;vertical-align: top;'>{1}</td></tr>", tmpPartyName, tmpPartyType);


                }

                field.partyRawHTML = string.Format(Settings1.Default.tableTemplate3, field.partyRawHTML);

                // Add parties
                int plainTiffCount = 0;
                int defendantCount = 0;

                foreach (var item in partyQuery)
                {
                    FresnoPartyField party = new FresnoPartyField();

                    party.CourtDuns = field.CourtDuns;
                    party.FileNumber = field.FilingNo;
                    party.FilingType = field.FilingType;
                    party.FilingDate = field.FilingDate;
                    party.RecordNo = field.RecordNo;

                    var partyArr = item.InnerText.Replace("\r", "").Replace(":", "").Replace("&nbsp;", "").Split('\n').ToList().Where(x => x.Trim() != "").Select(a => a.Trim()).ToList();

                    if (partyArr.ToList().Count > 1)
                    {

                        bool isActiveAttorney = false;
                        foreach (var arr in partyArr)
                        {
                            if (arr.Trim() == "Active Attorneys")
                            {
                                isActiveAttorney = true;
                            }

                            if (isActiveAttorney)
                            {
                                party.ActiveAttorney += (party.ActiveAttorney.Trim() == "" ? "" : "\n") + arr;
                            }
                        }

                    }

                    if (item.InnerText.ToUpper().IndexOf("PLAINTIFF") > -1)
                    {
                        party.PartyType = "P";
                        party.PartyName = partyArr[1].ToString();
                        plainTiffCount += 1;
                    }
                    else if (item.InnerText.ToUpper().IndexOf("DEFENDANT") > -1)
                    {
                        party.PartyType = "D";
                        party.PartyName = HtmlEntity.DeEntitize(partyArr[1].ToString());
                        defendantCount += 1;
                    }
                    else
                    {
                        //Other types
                    }


                    field.Parties.Add(party);
                }
                field.NoOfPlaintiffs = plainTiffCount.ToString();
                field.NoOfDefendants = defendantCount.ToString();

            }


             if (htmldoc.DocumentNode.SelectNodes("//span/a[@class='document-download']") != null)
             {
                 var queryEventList = (from a in htmldoc.DocumentNode.SelectNodes("//span/a[@class='document-download']")
                                       .Cast<HtmlNode>()
                                       select a);

                 foreach (var evt in queryEventList)
                 {
                     evt.ParentNode.Remove();
                 }
             }
     
          
        
            //field.EventInformation = (from a  in htmldoc.DocumentNode.SelectNodes("//div[@id='divEventsInformation_body']")
              //                   .Cast<HtmlNode>() select a).ToList().FirstOrDefault().OuterHtml;



            //pang test
             //if (htmldoc.DocumentNode.SelectNodes("//div[@id='divdispositionInformation_body']//table") != null)
             //{
             //    var cnt=htmldoc.DocumentNode.SelectNodes("//div[@id='divdispositionInformation_body']//table").ToList();
             //    if (cnt.Count > 1)
             //    {
             //        Console.WriteLine("pasok");
             //    }
             //}
            
            if (htmldoc.DocumentNode.SelectNodes("//div[@id='divdispositionInformation_body']/div") != null)
             {
                 
                 var dispoList = htmldoc.DocumentNode.SelectNodes("//div[@id='divdispositionInformation_body']/div").ToList();
                 foreach (var divDispoInfoItem in dispoList)
                 {
                     HtmlAgilityPack.HtmlDocument htmldocDispo = new HtmlAgilityPack.HtmlDocument();
                     htmldocDispo.LoadHtml(divDispoInfoItem.OuterHtml);


                     if (htmldocDispo.DocumentNode.SelectNodes("//table") == null)
                     {
                         field.DispositionEventInfo = string.Format(Settings1.Default.tableTemplate, "<tr><td style='font-size:12px;'>" + htmldocDispo.DocumentNode.InnerHtml.Replace("<hr>", "") + "</td></tr>");
                         continue;
                     }

       
                     var dTable = (from a in htmldocDispo.DocumentNode.SelectNodes("//table").Cast<HtmlNode>()
                                   select a).ToList();

                     foreach (var tbl in dTable.Take(1))
                     {

                         var dispositionTemp = tbl.InnerText.Split('\n');

                         string tmpRes = "";
                         foreach (var dpArr in dispositionTemp)
                         {
                             string dpDesc = dpArr.Replace("\r", "").Trim();
                             if (dpDesc != "")
                                 if (dpDesc == "Party")
                                 {
                                     tmpRes += "<b>Party</b>";
                                 }
                                 else if (dpDesc == "Names:")
                                 {
                                     tmpRes += "<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Names:";
                                 }
                                 else
                                 {
                                     tmpRes += (tmpRes != "" ? "<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" : "") + dpDesc;
                                 }
                         }

                         //var dispositionNode = (from a in tbl.SelectNodes("//table")
                         //                                    .Cast<HtmlNode>()
                         //                       select a);

                         //var nodex = HtmlNode.CreateNode("<p>" + tmpRes + "</p>");
                         //foreach (var dpNode in dispositionNode)
                         //{
                         //    dpNode.ParentNode.ReplaceChild(nodex, dpNode);
                         //}

                         string dispoBody = htmldocDispo.DocumentNode.InnerHtml.Replace("class=\"ssMenuText ssSmallText\"", "style=\"font-size:12px;font-family:arial;\"").Replace("<hr>", "<br/>");

                         field.DispositionEventInfo += dispoBody;

                     }



                 }

                 field.DispositionEventInfo = string.Format(Settings1.Default.tableTemplate, field.DispositionEventInfo);
             }

            string cEventInfoRawHtml = (from a in htmldoc.DocumentNode.SelectNodes("//div[@id='divEventsInformation_body']")
                                   .Cast<HtmlNode>()
                                    select a).ToList().FirstOrDefault().OuterHtml;


            HtmlAgilityPack.HtmlDocument htmldocInfo = new HtmlAgilityPack.HtmlDocument();
            htmldocInfo.LoadHtml(cEventInfoRawHtml);

            var queryEventInfo = (from a in htmldocInfo.DocumentNode.SelectNodes("/div/div")
                                .Cast<HtmlNode>()
                              select a).ToList();

            foreach (var eventInfo in queryEventInfo)
            {
                HtmlAgilityPack.HtmlDocument htmldocInfoInner = new HtmlAgilityPack.HtmlDocument();
                htmldocInfoInner.LoadHtml(eventInfo.InnerHtml);

                var queryEventInfoHeader = (from a in htmldocInfoInner.DocumentNode.SelectNodes("/div")
                                    .Cast<HtmlNode>()
                                      select a).FirstOrDefault();

                string eventHeader = queryEventInfoHeader.InnerText;
                field.EventInformation += string.Format("<tr><td style='width:100%;font-size:14px;font-weight:bold'>{0}</td></tr>", eventHeader);




                var nodeQuery = htmldocInfoInner.DocumentNode.SelectNodes("//p");
                if (nodeQuery != null)
                {
                    var queryEventInfoBody = (from a in htmldocInfoInner.DocumentNode.SelectNodes("//p")
                                     .Cast<HtmlNode>()
                                              select a).ToList();
                    foreach (var p in queryEventInfoBody)
                    {
                        if (p.InnerText.Trim() != "")
                            field.EventInformation += string.Format("<tr><td style='width:100%;font-size:12px;padding-left:20px'>{0}</td></tr>", p.InnerText);
                    }
                }
            }

            if (htmldoc.DocumentNode.SelectNodes("//div[@id='divDocumentsInformation_body']") != null)
            {
                var docInfoArr = (from a in htmldoc.DocumentNode.SelectNodes("//div[@id='divDocumentsInformation_body']")
                                                      .Cast<HtmlNode>()
                                  select a).FirstOrDefault().InnerText.Split('\n');

                foreach (var info in docInfoArr)
                {
                    if (info.Trim() != "")
                        field.DocumentsInfo += "<tr><td>" + info + "</td></tr>";
                }

                if (field.DocumentsInfo.Trim() != "")
                {
                    field.DocumentsInfo = string.Format(Settings1.Default.tableTemplate3, field.DocumentsInfo); 
                }
            }


            if (htmldoc.DocumentNode.SelectNodes("//div[@id='divCauseInformation_body']//table/thead") != null)
            {
                var coaHeader = (from a in htmldoc.DocumentNode.SelectNodes("//div[@id='divCauseInformation_body']//table/thead")
                                                       .Cast<HtmlNode>()
                                 select a).FirstOrDefault().OuterHtml;

                var coaBody = (from a in htmldoc.DocumentNode.SelectNodes("//div[@id='divCauseInformation_body']//table/tbody")
                                                       .Cast<HtmlNode>()
                               select a).FirstOrDefault().OuterHtml;

                field.CauseOfActionInfo = string.Format(Settings1.Default.tableTemplate2,coaHeader + coaBody);
            }


     
            
        	return field;
	
        }
        
        
        private string GetCaseId(string Html)
        {
            HtmlAgilityPack.HtmlDocument htmldoc = new HtmlAgilityPack.HtmlDocument();
            htmldoc.LoadHtml(Html);

            var query = (from span in htmldoc.DocumentNode.SelectNodes("//div[@id='case-subscribe-container']").Cast<HtmlNode>()
                         select span).ToList();
            return query[0].Attributes[3].Value;
        }


        private string GetElementValueById(string Html, string Type, string Id)
        {
        	if(Html.IndexOf("No Results Found") > -1) return "";
        	
            HtmlAgilityPack.HtmlDocument htmldoc = new HtmlAgilityPack.HtmlDocument();
            htmldoc.LoadHtml(Html);

            var query = (from span in htmldoc.DocumentNode.SelectNodes("//" + Type + "[@data-caseid]").Cast<HtmlNode>()
                         select span).ToList();
            return Uri.EscapeDataString(query[0].Attributes[1].Value);
        	
        	
        	
        }

        private string GetCourtDuns(string filingNo){
        	
        	foreach (var item in CourtDunsList) {
        		if(item.Key ==  filingNo.ToUpper().Trim().Substring(4,2)){
        			return item.Value;
        		}
        	}
        	return "";
        }
        
        public FresnoMiner(bool EnableProxy = false)
        {
        	  //Initialize Miner
            CourtDunsList = new List<KeyValuePair<string, string>>();
            CourtDunsList.Add(new KeyValuePair<string,string>("CG","602752263"));
            CourtDunsList.Add(new KeyValuePair<string,string>("CL","362272866"));
            CourtDunsList.Add(new KeyValuePair<string,string>("SC","962372124"));


            this.isProxyEnable =EnableProxy;
           // _Html = HttpRequestGet(@"https://publicportal.fresno.courts.ca.gov/FRESNOPORTAL/");
        
        	
        }

        private bool IsNoRecord(string html)
        {
            if (html.IndexOf("No Results Found") > -1) return true;
            return false;
        }


        public  FresnoBaseField GetDetails(string CaseNumber, int recordNo){
        
            retry:
            _Html = HttpRequestPost(@"https://publicportal.fresno.courts.ca.gov/FRESNOPORTAL/SmartSearch/SmartSearch/SmartSearch", CreateSearchPostData(CaseNumber), @"https://publicportal.fresno.courts.ca.gov/FRESNOPORTAL/");



            if (IsNoRecord(_Html)) return null;
            if (_Html == "") return null; 


            //_Html = HttpRequestGet(@"https://publicportal.fresno.courts.ca.gov/FRESNOPORTAL/Home/WorkspaceMode?p=0", @"https://publicportal.fresno.courts.ca.gov/FRESNOPORTAL/");
                _Html = HttpRequestGet(@"https://publicportal.fresno.courts.ca.gov/FRESNOPORTAL/SmartSearch/SmartSearchResults", @"https://publicportal.fresno.courts.ca.gov/FRESNOPORTAL/Home/WorkspaceMode?p=0");

            if (_Html.Trim() == "") goto retry;

            if (IsNoRecord(_Html)) return null;
            
            string eid = GetElementValueById(_Html,"a","");
            _Html = HttpRequestGet(@"https://publicportal.fresno.courts.ca.gov/FRESNOPORTAL/Case/CaseDetail?eid=" + eid + "&tabIndex=3", @"https://publicportal.fresno.courts.ca.gov/FRESNOPORTAL/Home/WorkspaceMode?p=0");

            if (_Html.Trim() == "") goto retry;



			var baseField = ParseHtml( _Html, recordNo); 
            string cid = GetCaseId(_Html);
            _Html = HttpRequestGet(@"https://publicportal.fresno.courts.ca.gov/FRESNOPORTAL/Case/CaseDetail/LoadFinancialInformation?caseId=" + cid ,@"https://publicportal.fresno.courts.ca.gov/FRESNOPORTAL/Home/WorkspaceMode?p=0");

            if (_Html.Trim() == "") goto retry;

          

            baseField.FinancialInformation = ParseFinancialInfo(_Html);
             //File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), recordNo.ToString() + "_FinancialInfo.html"), baseField.FinancialInformation);
             return baseField;
        }


        private string ParseFinancialInfo(string html)
        {
            if (html.IndexOf("No financial information exists for this case") > -1) return "<tr><td colspan='5' class='tyler-bold'>No financial information exists for this case</td></tr>";
            
            string tmpRes = "";

            

            HtmlAgilityPack.HtmlDocument htmldocFinancial = new HtmlAgilityPack.HtmlDocument();
            htmldocFinancial.LoadHtml(html);

            var queryName = (from a in htmldocFinancial.DocumentNode.SelectNodes("/p[1]").Cast<HtmlNode>()
                                select a).FirstOrDefault();

            if (htmldocFinancial.DocumentNode.SelectNodes("/div[@class='k-widget k-grid']") == null) return "<tr><td colspan='5' style='font-size:14px;'><b>" + htmldocFinancial.DocumentNode.InnerText + "</b></td></tr>"; 
            int gridCount = (from a in htmldocFinancial.DocumentNode.SelectNodes("/div[@class='k-widget k-grid']").Cast<HtmlNode>()
                             select a).ToList().Count;

            int gridCurLine = 0;
            tmpRes = "<tr><td colspan='5' style='font-size:14px;'><b>" + queryName.InnerText + "</b></td></tr>";
            tmpRes += "<tr><td colspan='5'>&nbsp;</td></tr>";

            var nodes = htmldocFinancial.DocumentNode.ChildNodes;
            foreach (var elm in nodes)
            {
                if (elm.Attributes.Count == 0) continue;
                if (elm.Attributes[0].Value == "tyler-layout-fluid tyler-cols-2")
                {

                    HtmlAgilityPack.HtmlDocument htmldocTopInfo = new HtmlAgilityPack.HtmlDocument();
                    htmldocTopInfo.LoadHtml(elm.OuterHtml);

                    var queryTopInfoInner = (from a in htmldocTopInfo.DocumentNode.SelectNodes("//p").Cast<HtmlNode>()
                                             select a).ToList();


                    tmpRes += string.Format("<tr><td colspan='3' style='padding-left:20px;'><b>{0}</b></td><td colspan='2' style='text-align:right;padding-right:20px;'><b>{1}</b></td></tr>", queryTopInfoInner[0].InnerText, queryTopInfoInner[1].InnerText);
                }
                else if (elm.Attributes[0].Value == "k-widget k-grid")
                {
                    gridCurLine += 1;
                    HtmlAgilityPack.HtmlDocument htmldocTableRoot = new HtmlAgilityPack.HtmlDocument();
                    htmldocTableRoot.LoadHtml(elm.OuterHtml);

                    var queryTable= (from a in htmldocTableRoot.DocumentNode.SelectNodes("//table").Cast<HtmlNode>()
                                     select a).FirstOrDefault();

                 
                    HtmlAgilityPack.HtmlDocument htmldocTable = new HtmlAgilityPack.HtmlDocument();
                    htmldocTable.LoadHtml(queryTable.OuterHtml);

                    var queryRows = (from a in htmldocTable.DocumentNode.SelectNodes("//tr").Cast<HtmlNode>()
                                     select a).ToList();

                    tmpRes += "<tr><td colspan='5'>&nbsp;</td></tr>";


                    string rowTemplate = "<tr ><td class='borderx'>{0}</td><td class='borderx'>{1}</td><td class='borderx'>{2}</td><td class='borderx'>{3}</td><td style='text-align:right;' class='borderx'>{4}</td></tr>";

                    foreach (var row in queryRows)
                    {
                        HtmlAgilityPack.HtmlDocument htmldocTableRow = new HtmlAgilityPack.HtmlDocument();
                        htmldocTableRow.LoadHtml(row.OuterHtml);
                        if (htmldocTableRow.DocumentNode.SelectNodes("//td") == null) continue;
                        var queryData = (from a in htmldocTableRow.DocumentNode.SelectNodes("//td").Cast<HtmlNode>()
                                         select a).ToList();
                        tmpRes += string.Format(rowTemplate, queryData[0].InnerText, queryData[1].InnerText, queryData[2].InnerText, queryData[3].InnerText, queryData[4].InnerText);

                    }

                    if (gridCount != gridCurLine)
                    tmpRes += "<tr><td colspan='5'>&nbsp;</td></tr>";
                }
            }



            //var queryTopInfo = (from a in htmldocFinancial.DocumentNode.SelectNodes("/div[@class='tyler-layout-fluid tyler-cols-2']").Cast<HtmlNode>()
            //             select a).ToList();

            //foreach (HtmlNode divInfo in queryTopInfo)
            //{


            //    HtmlAgilityPack.HtmlDocument htmldocTopInfo = new HtmlAgilityPack.HtmlDocument();
            //    htmldocTopInfo.LoadHtml(divInfo.OuterHtml);

            //    var queryTopInfoInner = (from a in htmldocTopInfo.DocumentNode.SelectNodes("//p").Cast<HtmlNode>()
            //                             select a).ToList();


            //    tmpRes += string.Format("<tr><td colspan='3' style='padding-left:20px;'><b>{0}</b></td><td colspan='2' style='text-align:right;padding-right:20px;'><b>{1}</b></td></tr>", queryTopInfoInner[0].InnerText, queryTopInfoInner[1].InnerText);
            //}

            //var queryTable = (from a in htmldocFinancial.DocumentNode.SelectNodes("//table").Cast<HtmlNode>()
            //                    select a).FirstOrDefault();

            //HtmlAgilityPack.HtmlDocument htmldocTable = new HtmlAgilityPack.HtmlDocument();
            //htmldocTable.LoadHtml(queryTable.OuterHtml);

            //var queryRows = (from a in htmldocTable.DocumentNode.SelectNodes("//tr").Cast<HtmlNode>()
            //                  select a).ToList();

            //tmpRes += "<tr><td colspan='5'>&nbsp;</td></tr>";

            
            //string rowTemplate = "<tr ><td class='borderx'>{0}</td><td class='borderx'>{1}</td><td class='borderx'>{2}</td><td class='borderx'>{3}</td><td style='text-align:right;' class='borderx'>{4}</td></tr>";

            //foreach (var row in queryRows)
            //{
            //    HtmlAgilityPack.HtmlDocument htmldocTableRow = new HtmlAgilityPack.HtmlDocument();
            //    htmldocTableRow.LoadHtml(row.OuterHtml);
            //    if (htmldocTableRow.DocumentNode.SelectNodes("//td") == null) continue;
            //    var queryData = (from a in htmldocTableRow.DocumentNode.SelectNodes("//td").Cast<HtmlNode>()
            //                     select a).ToList();
            //    tmpRes += string.Format(rowTemplate, queryData[0].InnerText, queryData[1].InnerText, queryData[2].InnerText, queryData[3].InnerText, queryData[4].InnerText);

            //}

            return tmpRes;
        }
        
        private string CreateSearchPostData(string CaseNumber)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("caseCriteria.SearchCriteria={0}&caseCriteria.JudicialOfficerSearchBy=&caseCriteria.NameMiddle=&caseCriteria.NameSuffix=&caseCriteria.AdvancedSearchOptionsOpen=false&caseCriteria.CourtLocation_input=All+Locations&caseCriteria.CourtLocation=All+Locations&caseCriteria.SearchBy_input=Smart+Search&caseCriteria.SearchBy=SmartSearch&caseCriteria.SearchByPartyName=true&caseCriteria.SearchByNickName=false&caseCriteria.SearchByBusinessName=false&caseCriteria.FBINumber=&caseCriteria.SONumber=&caseCriteria.BookingNumber=&caseCriteria.SearchCases=true&caseCriteria.SearchCases=false&caseCriteria.CaseType_input=&caseCriteria.CaseType=&caseCriteria.CaseStatus_input=&caseCriteria.CaseStatus=&caseCriteria.FileDateStart=&caseCriteria.FileDateEnd=&caseCriteria.JudicialOfficer_input=&caseCriteria.JudicialOfficer=&caseCriteria.SearchJudgments=true&caseCriteria.SearchJudgments=false&caseCriteria.JudgmentType_input=&caseCriteria.JudgmentType=&caseCriteria.JudgmentDateFrom=&caseCriteria.JudgmentDateTo=",CaseNumber));
            return sb.ToString();
        }


        private string HttpRequestPost(string Url, string PostData, string Referer = "")
        {
            int ReTryCount = 0;

        Retry:
            try
            {

                string result = string.Empty;
                UTF8Encoding encoding = new UTF8Encoding();
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);

                byte[] bytedata = encoding.GetBytes(PostData);
                req.Method = "POST";
                //req.KeepAlive = false;
                //req.Proxy = null; // null;
                req.ContentType = "application/x-www-form-urlencoded";
            
                //req.ServicePoint.UseNagleAlgorithm = false;
                req.ServicePoint.ConnectionLimit = 100000;
                //req.ServicePoint.Expect100Continue = false;

                req.Host = "publicportal.fresno.courts.ca.gov";
                req.Headers.Add("Cache-Control: max-age=0");
                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                req.Headers.Add("Upgrade-Insecure-Requests: 1");
                req.UserAgent = "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2497.0 Safari/537.36";

                req.Headers.Add("Accept-Language: en-US,en;q=0.8");
                req.Accept = "text/html, application/xhtml+xml, */*";
                req.ContentType = "application/x-www-form-urlencoded";
                req.KeepAlive = true;

      
                req.ContentLength = bytedata.Length;

                req.CookieContainer = _CookieContainer;
                

                req.Referer = Referer;
                //req.Headers.Add("Cookie: " + cookieHeader);

                if(isProxyEnable)
                req.Proxy = new WebProxy(Settings1.Default.ProxyAddress + ":" + Settings1.Default.ProxyPort ,false);

                using (Stream stream = req.GetRequestStream())
                {
                    stream.Write(bytedata, 0, bytedata.Length);
                    using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        result = reader.ReadToEnd();
                        _CookieContainer.Add(response.Cookies); 
                       // cookieHeader = response.GetResponseHeader("Set-Cookie"); 
                    }
                }


                return result;
            }
            catch (Exception ex)
            {
                ReTryCount += 1;
                if (ReTryCount > 3) {
                    return "";
                }
                goto Retry;
            }

        }




        private string HttpRequestGet(string url,string referer = "")
        {
            int ReTryCount = 0;
        Retry:

            try
            {
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                string result = "";
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "Get";
                req.Referer = referer;
                req.Host = "publicportal.fresno.courts.ca.gov";
                req.Headers.Add("Cache-Control: max-age=0");
                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                req.Headers.Add("Upgrade-Insecure-Requests: 1");
                req.UserAgent = "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2497.0 Safari/537.36";
               
                req.Headers.Add("Accept-Language: en-US,en;q=0.8");
                req.Accept = "text/html, application/xhtml+xml, */*";
                req.ContentType = "application/x-www-form-urlencoded";
            req.KeepAlive = true;
          
                req.CookieContainer = _CookieContainer;
                req.ServicePoint.ConnectionLimit = 100000;
                //                req.Headers.Add("Cookie: "+ cookieHeader);

                if (isProxyEnable)
                    req.Proxy = new WebProxy(Settings1.Default.ProxyAddress + ":" + Settings1.Default.ProxyPort  ,false);

                using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader sr = new StreamReader(stream))
                        result = sr.ReadToEnd();
                    _CookieContainer.Add(response.Cookies); 

         
                    //cookieHeader = response.GetResponseHeader("Set-Cookie"); 
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




    }
}
