using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CitizenManagement.Models
{
    public class Token
    {
        public string UserName { get; set; }
        public string Info { get; set; }
        public long exp { get; set; }
    }
}