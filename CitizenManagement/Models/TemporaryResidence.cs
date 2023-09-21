using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CitizenManagement.Models
{
    public class TemporaryResidence
    {
        public string HouseholdId { get; set; }
        public string RelationshipHeadHouse { get; set; }
        public DateTime? DateCreate { get; set; }
        public string Note { get; set; }

        public TemporaryResidence() {
            DateCreate = DateTime.Now;
        }

    }
}