using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace CitizenManagement.Models
{
    public class People
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Avatar { get; set; }
        public string Name { get; set; }
        public string OrtherName { get; set; }
        public DateTime Birthday { get; set; }
        public string Gender { get; set; }
        public string Ethnic { get; set; }
        public string Religion { get; set; }
        public string Nationality { get; set; }
        public Identity Identity { get; set; }
        public Int32 Level { get; set; }
        public Place Domicile { get; set; }
        public Place Resident { get; set; }
        public Place Staying { get; set; }
        public List<Job> Jobs { get; set; }
        public List<TemporaryAbsent> TemporaryAbsent { get; set; }
        public Die Die { get; set; }
        public List<TemporaryResidence> TemporaryResidence { get; set; }

        public People()
        {
            TemporaryResidence = new List<TemporaryResidence>();
            TemporaryAbsent = new List<TemporaryAbsent>();
        }
    }
}
