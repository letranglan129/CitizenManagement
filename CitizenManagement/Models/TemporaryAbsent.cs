using System;

namespace CitizenManagement.Models
{
    public class TemporaryAbsent
    {
      public DateTime AbsenceDay { get; set; }
      public DateTime ReturnDay { get; set; }
      public string Reason { get; set; }
      public string Destination { get; set; }
      public string Note { get; set; }
      public DateTime DateCreate { get; set; }
      public string Confirmer { get; set; }
        public TemporaryAbsent()
        {
            DateCreate = DateTime.Now;
        }
    }
}
