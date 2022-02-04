using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjMiner.Model
{
    public class NebraskaPDFField
    {
        public string CaseSummary { get; set; }
        public string PartyAttorney { get; set; }
        public string CostInformation { get; set; }
        public string FinancialActivity { get; set; }
        public string PaymentsMadeToCourt { get; set; }
        public string RegisterOfAction { get; set; }
        public string JudgementInformation { get; set; }
        public NebraskaPDFField()
        {
            CaseSummary = "";
            PartyAttorney = "";
            CostInformation = "";
            FinancialActivity = "";
            PaymentsMadeToCourt = "";
            RegisterOfAction = "";
            JudgementInformation = "";
        }
    }
}
