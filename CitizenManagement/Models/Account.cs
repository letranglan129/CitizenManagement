using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CitizenManagement.Models
{
    public class Account
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<Access> Permissions { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Info { get; set; }
        public List<People> InfoData { get; set; }

        public Account() { 
            Permissions = new List<Access>();
        }
    }
}