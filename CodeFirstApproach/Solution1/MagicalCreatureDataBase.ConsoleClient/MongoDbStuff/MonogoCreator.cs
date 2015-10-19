namespace MagicalCreatureDataBase.ConsoleClient.MongoDbStuff
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MongoDB.Driver;
    using MongoDB.Bson;

    using MongoDB.Bson.Serialization.Attributes;
    using MagicalCreatureDataBase.Models.Enumerations;
    using DataAccess;

    public class MongoCreator
    {

        public const string DatabaseHost = "mongodb://127.0.0.1";
        public const string DatabaseName = "MagicalCreatures";

        public void GenerateSampleData()
        {
            var data = new MongDbDataStruct[]
            {
                new MongDbDataStruct { Name = "Norse1", LocationOfOrigin = "DMCity1" },
                new MongDbDataStruct { Name = "Norse2", LocationOfOrigin = "DMCity2" },
                new MongDbDataStruct { Name = "Norse3", LocationOfOrigin = "DMCity3" },
                new MongDbDataStruct { Name = "Norse4", LocationOfOrigin = "DMCity4" },
                new MongDbDataStruct { Name = "Norse5", LocationOfOrigin = "DMCity5" },
                new MongDbDataStruct { Name = "Norse6", LocationOfOrigin = "DMCity6" },
                new MongDbDataStruct { Name = "Norse7", LocationOfOrigin = "DMCity7" },
                new MongDbDataStruct { Name = "Norse8", LocationOfOrigin = "DMCity8" },
                new MongDbDataStruct { Name = "Norse9", LocationOfOrigin = "DMCity9" },
                new MongDbDataStruct { Name = "Norse10", LocationOfOrigin = "DMCity10" },

            };

            foreach (var item in data)
            {
                this.InsertData(item);
            }
        }

        public MongoDatabase GetDatabase(string name, string fromHost)
        {
            var mongoClient = new MongoClient(fromHost);
            var server = mongoClient.GetServer();
            return server.GetDatabase(name);
        }

        public void InsertData(MongDbDataStruct data)
        {
            var db = this.GetDatabase(DatabaseName, DatabaseHost);

            var transports = db.GetCollection<BsonDocument>("MagicalCreatureMythologyData");
            transports.Insert(new BsonDocument
            {
                { "Id", ObjectId.GenerateNewId().ToString() },
                {"Name", data.Name },
                {"LocationOfOrigin", data.LocationOfOrigin },
                {"Description", "" }             
            });
        }
    }
}
