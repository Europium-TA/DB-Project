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
            var creature1 = new MagicalCreatureModel
            {
                Name = "Jhon1",
                DateSpotted = DateTime.Now.AddDays(-10),
                AssesedDangerLevel =DangerLevel.High,
                AggressionWhenSpotted = AggressionLevel.Aggitated,
                Species = "Dragon",
                Location = "New York"
            };

            var creature2 = new MagicalCreatureModel
            {
                Name = "Jhon2",
                DateSpotted = DateTime.Now.AddDays(-10),
                AssesedDangerLevel = DangerLevel.High,
                AggressionWhenSpotted = AggressionLevel.Enraged,
                Species = "Gian",
                Location = "Sofia",



            };

            var creature3 = new MagicalCreatureModel
            {
                Name = "Locki",
                DateSpotted = DateTime.Now.AddDays(-10),
                AssesedDangerLevel = DangerLevel.High,
                AggressionWhenSpotted = AggressionLevel.Docile,
                Species = "God",
                Location = "Sofia",
            };



            this.InsertData(creature1);
            this.InsertData(creature2);
            this.InsertData(creature3);
         


        }

        public MongoDatabase GetDatabase(string name, string fromHost)
        {
            var mongoClient = new MongoClient(fromHost);
            var server = mongoClient.GetServer();
            return server.GetDatabase(name);
        }

        public void InsertData(MagicalCreatureModel data)
        {
            var db = this.GetDatabase(DatabaseName, DatabaseHost);

            var transports = db.GetCollection<BsonDocument>("MagicalCreatureDocuments");
            transports.Insert(new BsonDocument
            {
                { "Id", ObjectId.GenerateNewId().ToString() },
                {"Name", data.Name },
                {"Date", data.DateSpotted.ToShortDateString() },
                {"DangerLeve", data.AssesedDangerLevel },
                {"Aggression", data.AggressionWhenSpotted },
                {"Species", data.Species },
                { "Location", data.Location },
            });
        }
    }
}
