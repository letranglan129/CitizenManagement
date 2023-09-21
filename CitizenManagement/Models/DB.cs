using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Core.Configuration;

namespace CitizenManagement.Models
{
    public class DB
    {
        public static readonly string CONNECT_URL = "mongodb://lelanv2:785571@mongo.letranglan.top:27018";
        //public static readonly string CONNECT_URL = "mongodb://localhost:27017/?directConnection=true&serverSelectionTimeoutMS=2000";
        public static IMongoCollection<T> get<T>(string colllection)
        {
            var client = new MongoClient(CONNECT_URL);
            var collection = client.GetDatabase("ManagerPeople").GetCollection<T>(colllection);
            return collection;
        }
        public static IMongoDatabase get()
        {
            var client = new MongoClient(CONNECT_URL);
            var database = client.GetDatabase("ManagerPeople");

            return database;
        }
    }
    
}
