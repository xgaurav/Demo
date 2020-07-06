using System;
using System.Collections.Generic;
using System.Text;

namespace EntiryModel
{
    public class Location
    {
        public string LocationCode { get; set; }
        public string Description { get; set; }
        public string DisplayOnScreenInd { get; set; }
        public string StatementProducedInd { get; set; }
        public string SourceCdInput { get; set; }
        public string ScheduleFdatabaseName { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreationLogonId { get; set; }
        public DateTime ModificationDate { get; set; }
        public string ModificationLogonId { get; set; } 
    }
}
