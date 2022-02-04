using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjMiner.Model
{
    public class NebraskaCaseField
    {
        public string CourtType { get; set; }
        public string CaseNumber { get; set; }
        public string SuitCOA { get; set; }
        public string FilingDate  { get; set; }
        public string Status { get; set; }
        public List<NebraskaPartyField> NebraskaPartyField { get; set; }
        public NebraskaPDFField NebraskaPDFField { get; set; }

        public NebraskaCaseField() {
            this.CourtType = "";
            this.CaseNumber = "";
            this.SuitCOA = "";
            this.Status = "";
            this.FilingDate = "";
            NebraskaPartyField = new List<NebraskaPartyField>();
            NebraskaPDFField = new NebraskaPDFField();
        }
    }
}
