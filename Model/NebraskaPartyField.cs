using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjMiner.Model
{

    public class NebraskaPartyField
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string StateCityZip { get; set; }
        public string Type { get; set; }

        public NebraskaPartyField()
        {
            this.Name = "";
            this.Address = "";
            this.StateCityZip = "";
            this.Type = "";
        }
    }
}
