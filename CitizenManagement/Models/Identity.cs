using System;

namespace CitizenManagement.Models
{
    public class Identity
    {
        public Place Place { get; set; }
        public DateTime Date { get; set; }
        public string Number { get; set; }
        public string Identification { get; set; }
        public string Confirmer { get; set; }

    }
}
