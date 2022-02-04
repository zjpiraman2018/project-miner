using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;

namespace ProjMiner.Controller
{
    public static class NebraskaController
    {

        public static string GetCourtType(string CourtType)
        {
            string ret = "";
            switch (CourtType.Trim())
            {
                case "[ C ] - Country":
                    ret = "C";
                    break;
                case "[ D ] - District":
                    ret = "D";
                    break;
            }
            return ret ;
        }

        public static string GetCaseType(string CaseType)
        {
            string ret = "";
            switch (CaseType.Trim())
            {
                case "[ CI ] - Civil":
                    ret =  "CI";
                    break;
                case "[ SC ] - Small Claims":
                    ret = "SC";
                    break;
            }

            return ret;
        }

        public static string[] GetUserAccounts()
        {
            List<string> accounts = new List<string>();

            StringReader sr = new StringReader(Settings1.Default.Users);

            // Loading from a file, you can also load from a stream
            XDocument xml = XDocument.Load(sr);

            foreach (XElement x in xml.Descendants("Users"))
            {
                foreach (XElement y in x.Descendants("Credential"))
                {
                    accounts.Add(y.Element("Account").Value);
                }

            }
            return accounts.ToArray();
        }

        public static KeyValuePair<string, string> GetCredential(string Account)
        {

            StringReader sr = new StringReader(Settings1.Default.Users);

            // Loading from a file, you can also load from a stream
            XDocument xml = XDocument.Load(sr);

            foreach (XElement x in xml.Descendants("Users"))
            {
                foreach (XElement y in x.Descendants("Credential"))
                {
                    if (y.Element("Account").Value == Account)
                    {
                        return new KeyValuePair<string, string>(
                            y.Element("Username").Value,
                            y.Element("Password").Value);
                    }

                }
            }

            return new KeyValuePair<string, string>("", "");
        }

    }
}
