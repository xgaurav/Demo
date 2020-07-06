using System;
using System.Collections.Generic;
using System.Text;

namespace EntiryModel
{
    public class CompanyCode
    {
        public string ComCode { get; set; }
        public string Name { get; set; }
        public float PoolingPercent { get; set; }
        public string LOCFlag { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public  DateTime CreationDate { get; set; }
        public string CreationLogonID { get; set; }
        public DateTime ModificationDate { get; set; }
        public string ModificationLogonID { get; set; }
        public string GripCode { get; set; }
        public string GroupNameShort { get; set; }
        public string NameShort { get; set; }
        public string RollUpCompanyCode { get; set; }
    }
}
