using System;

namespace CitizenManagement.Models
{
    public class Job
    {
        public string Name { get; set; }
        public Place Place { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
}
