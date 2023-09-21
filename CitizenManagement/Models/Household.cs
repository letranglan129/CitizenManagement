using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CitizenManagement.Models
{
    public class Household
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string HouseholdId { get; set; }
        public DateTime DateCreate { get; set; }
        public Place Place { get; set; }
        public List<HouseholdMember> Members { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string Owner { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<People> MembersLookup { get; set; }
        public List<People> OwnerInfo { get; set; }
        public Household()
        {
            DateCreate = DateTime.Now;
        }
    }
}