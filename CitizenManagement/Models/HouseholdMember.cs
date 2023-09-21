using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CitizenManagement.Models
{
    public class HouseholdMember
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string MemberId { get; set; }
        public string Relationship { get; set; }
    }
}