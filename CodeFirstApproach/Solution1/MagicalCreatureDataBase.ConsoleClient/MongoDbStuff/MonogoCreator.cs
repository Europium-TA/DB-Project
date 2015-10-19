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

    public class InitializedMongoDb
    {

        public const string DatabaseName = "MagicalCreatureDocuments";

        public void GenerateSampleData()
        {
            var creature1 = new MongDbDataStruct
            {
                Name = "Jhon1",
                DateSpoted = DateTime.Now.AddDays(-10),
                DLevel = DangerLevel.High,
                AggressionWhenSpotted = AggressionLevel.Docile,
                Location = "Sofia",
                Mythology = "British",
                Spices = "Dragon",
                Abilities = new string[] { "fly", "fire breath", "Magic" }
            };

            var creature2 = new MongDbDataStruct
            {
                Name = "Jhon2",
                DateSpoted = DateTime.Now.AddDays(-10),
                DLevel = DangerLevel.High,
                AggressionWhenSpotted = AggressionLevel.Enraged,
                Location = "Sofia",
                Mythology = "Norse",
                Spices = "Gian",
                Abilities = new string[] { "strenght" }
            };

            var creature3 = new MongDbDataStruct
            {
                Name = "Locki",
                DateSpoted = DateTime.Now.AddDays(-10),
                DLevel = DangerLevel.High,
                AggressionWhenSpotted = AggressionLevel.Docile,
                Location = "Sofia",
                Mythology = "Norse",
                Spices = "God",
                Abilities = new string[] { "Illusiong", "Magic" }
            };



            this.InsertData(creature1);
            this.InsertData(creature2);
            this.InsertData(creature3);
            var creature4 = creature1;
            creature4.Name = "Jim";
            this.InsertData(creature4);


        }
        public IMongoDatabase GetDatabase(string name)
        {
            var mongoClient = new MongoClient();
            var dataBase = mongoClient.GetDatabase(name);

            return dataBase;
        }

        private void InsertData(MongDbDataStruct data)
        {
            var db = this.GetDatabase(DatabaseName);

            var transports = db.GetCollection<BsonDocument>("MagicalCreatureDocuments");
            transports.InsertOneAsync(new BsonDocument
            {
                { "Id", ObjectId.GenerateNewId().ToString() },
                {"Name", data.Name },
                {"Date", data.DateSpoted.ToShortDateString() },
                { "Location", data.Location },
                {"DangerLeve", data.DLevel },
                {"Species", data.Spices },
                {"Mythology", data.Mythology },
                {"Aggression", data.AggressionWhenSpotted },
                {"Abilities", string.Join(",",data.Abilities) }
            });
        }
    }
}
