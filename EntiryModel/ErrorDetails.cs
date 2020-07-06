using System;
using System.Collections.Generic;
using System.Text;

namespace EntiryModel
{
    public class ErrorDetails
    {
        public int ErrorCode { get; set; }
        public string ErrorTitle{ get; set; }
        public string ErrorDescription { get; set; }
        public bool IsException { get; set; }
        public string Exception { get; set; }
    }

    
}
