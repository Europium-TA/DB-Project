namespace MagicalCreatureDataBase.ConsoleClient
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Data;
    using Models;
    using System.Data.Entity;
    using Data.Migrations;
    using System.Data.Entity.Migrations;
    using Models.Enumerations;
    using MagicalCreatureReport;
    using PdfSharp.Pdf;
    using PdfSharp.Drawing;
    using PdfSharp.Charting;
    using System.Text;
    using System.Xml.Serialization;
    using System.Xml.Linq;
    using Newtonsoft.Json;
    using MongoDbStuff;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Bson.Serialization.Attributes;
    using MongoDB.Driver.Builders;

    public class Program
    {

        const string DatabaseHost = "mongodb://127.0.0.1";
        const string DatabaseName = "Logger";


        class Log
        {
            [BsonRepresentation(BsonType.ObjectId)]
            public string Id { get; set; }

            public string Text { get; set; }

            public DateTime LogDate { get; set; }
        }

        static MongoDatabase GetDatabase(string name, string fromHost)
        {
            var mongoClient = new MongoClient(fromHost);
            var server = mongoClient.GetServer();
            return server.GetDatabase(name);
        }

        static void Main()
        {
            var db = GetDatabase(DatabaseName, DatabaseHost);

            var logs = db.GetCollection<Log>("Logs");

            logs.Insert(new Log()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                LogDate = DateTime.Now,
                Text = "Something important happened"
            });

            var update = Update.Set("Text", "Changed Text at " + DateTime.Now);


            var query = Query.And(
                Query.LT("LogDate", DateTime.Now.AddSeconds(-1))
                );

            logs.Update(query, update);

            var s= logs.FindAll()
                .Select(l => l.Text)
                .ToList();

            foreach (var item in s)
            {
                Console.WriteLine(item);
            }
        }

        /*public static void Main()
        {
            ////Database.SetInitializer(new DropCreateDatabaseIfModelChanges<MagicalCreatureDbContext>());

            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<MagicalCreatureDbContext, Configuration>());

            //var db = new MagicalCreatureDbContext();

            ////var intput = Console.ReadLine();

            //var list = ExtractMagicalCreaturesByMythologyName("Norse");

            //XmlReport(list);
            //PdfReportFromList(list);
            //JsonReport(list);

           
        }*/

        private static void JsonReport(ICollection<MagCreatureRepType> list)
        {
            var jsonObjs = new StringBuilder();

            foreach (var item in list)
            {
                var jsonObj = JsonConvert.SerializeObject(list.First(), Formatting.Indented);
                jsonObjs.Append(jsonObj);
            }

            System.IO.File.WriteAllText("jsonReport.json", jsonObjs.ToString());
        }


        private static void XmlReport(ICollection<MagCreatureRepType> creatures)
        {
            var report = new XElement(XName.Get("MagicalCreatureReport"));
            foreach (var c in creatures)
            {
                var personXml = new XElement("Creature",
                    new XElement("name", c.Name),
                    new XElement("location", c.Location),
                    new XElement("dateSpotted", c.Date),
                    new XElement("species", c.Species),
                    new XElement("aggression", c.Aggression));

                report.Add(personXml);
            }
           
            report.Save("report.xml");
        }

        private static void PdfReportFromList(ICollection<MagCreatureRepType> list)
        {
            PdfDocument pdf = new PdfDocument();
            PdfPage pdfPage = pdf.AddPage();
            XGraphics graph = XGraphics.FromPdfPage(pdfPage);
            XFont font = new XFont("Verdana", 10, XFontStyle.Bold);

            int yPoint = 0;
            foreach (var item in list)
            {
                var text = item.Name + " at " + item.Location + " and it is: " + item.Species;
                graph.DrawString(text, font, XBrushes.Black, new XRect(40, yPoint, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                yPoint = yPoint + 40;
            }


            pdf.Save("firstpage.pdf");
        }

        private static void AddElf()
        {
            var db = new MagicalCreatureDbContext();

            var mythology = new Mythology
            {
                Name = "Norse",
                AreaOfOrigin = "Denmakr"
            };

            var goblins = new Species
            {
                Name = "Elf",
                Mythology = mythology,
                Description = "Tall,Fast and pretty",
            };

            db.Species.Add(goblins);
            //context.SaveChanges();

            var loc = new Location { Name = "Plovdiv" };
            var supAb = new SuperNaturalAbility
            {
                Name = "Magic Missle",
                RangeInMeters = 10,
                DangerLevel = DangerLevel.Medium
            };

            db.Locations.Add(loc);
            //context.SaveChanges();

            db.SuperNaturalAbilities.Add(supAb);

            var creature = new MagicalCreature
            {
                Name = "Bob1",
                DateSpotted = DateTime.Now,
                AggressionWhenSpotted = AggressionLevel.Aggitated,
                AssesedDangerLevel = DangerLevel.Medium,
                Species = goblins,
                Location = loc,
            };

            creature.SuperNaturalAbilities.Add(supAb);

            db.MagicalCreatures.AddOrUpdate(creature);

            db.SaveChanges();

            Console.WriteLine(db.Mythologies.Count());
        }

        private static ICollection<MagCreatureRepType> ExtractMagicalCreaturesByMythologyName(string mythology) 
        {
            var db = new MagicalCreatureDbContext();

            var list = db.MagicalCreatures
                .Where(c => c.Species.Mythology.Name == "Norse")
                .Select(c => new MagCreatureRepType
                {
                    Name = c.Name,
                    Location = c.Location.Name,
                    Date = c.DateSpotted,
                    Species = c.Species.Name,
                    Aggression = c.AggressionWhenSpotted
                })
                .ToList();

            return list;
        }
    }
}
