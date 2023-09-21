using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CitizenManagement.Models
{
    public class Die
    {
        public DateTime DateOfDeath { get; set; }
        public DateTime DateCreate { get; set; }
        public string ReasonDeath { get; set; }
        public string Declarant { get; set; }
        public string Note { get; set; }
        public string Confirmer { get; set; }
        public Die()
        {
            DateCreate = DateTime.Now;
        }
    }
}