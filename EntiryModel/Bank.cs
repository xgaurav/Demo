using System;
using System.Collections.Generic;
using System.Text;

namespace EntiryModel
{
    public class Bank
    {

        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string ABANumber { get; set; }
        public string Location { get; set; }
         public int CCNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreationLogonID { get; set; }
        public DateTime ModificationDate { get; set; }
        public string ModificationLogonID { get; set; }

    }
}
