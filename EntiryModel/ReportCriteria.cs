using System;
using System.Collections.Generic;
using System.Text;

namespace EntiryModel
{
    public class ReportCriteria
    {
        public string ReportID { get; set; }
        public string ParmID { get; set; }
        public int ParmOrder { get; set; }
        public int ParmMaxAllowed { get; set; }
        public string ParmDefaultAll { get; set; }
    }
}
