namespace MagicalCreatureDataBase.ConsoleClient
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Xml;
    using Data;
    using Models;
    using System.Data.Entity;
    using Data.Migrations;
    using PdfSharp.Pdf;
    using PdfSharp.Drawing;
    using System.Xml.Linq;
    using Newtonsoft.Json;
    using MongoDbStuff;
    using MongoDB.Bson;
    using Telerik.OpenAccess;
    using DataAccess;
    using DataAccess.Importers;
    using Pdf;

    public class Program
    {


        public static void Main()
        {

            System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<MagicalCreatureDbContext, Configuration>());

            
            var dbContext = new MagicalCreatureDbContext();
            List<MagicalCreatureModel> list;
            string lastCommnad = "";
            while (true)
            {

                var input = Console.ReadLine();

                switch(input)
                {
                    case "init":
                        GenerateSqlDatabBaseIfNeeded();
                        CreteMongoDb();
                        break;
                    case "excel":
                        LoadInitialExcelDataFromZipFile();
                        break;
                    case "importMongo":
                        ImportMongoToSql();
                        break;
                    case "pdf":
                        GeneratePDfReport();
                        break;
                    case "xmlReport":
                        dbContext = new MagicalCreatureDbContext();
                        list = AllCreatures(dbContext);
                        XmlReport(list);
                        break;
                    case "json":
                        dbContext = new MagicalCreatureDbContext();
                        list = AllCreatures(dbContext);
                        JsonReport(list);
                        break;
                    case "mysql":
                        dbContext = new MagicalCreatureDbContext();
                        list = AllCreatures(dbContext);
                        addToMySql(list);
                        break;
                    case "update":
                        UpdateMythologies();
                        break;
                    case "exit":
                        return;
                    default:
                        break;
                }

            }
           
        }

        private static void UpdateMythologies()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("../../../DataSystem/AddData.xml");
            XmlNode rootNode = doc.DocumentElement;

            var dbsql = new MagicalCreatureDbContext();

            var mongoCreator = new MongoCreator();
            var db = mongoCreator.GetDatabase(MongoCreator.DatabaseName, MongoCreator.DatabaseHost);
           

            foreach (XmlNode node in rootNode.ChildNodes)
            {
                var name = node["name"].InnerText;
                var description = node["description"].InnerText;

                var myth = dbsql.Mythologies.First(x => x.Name == name);
                myth.Discription = description;

                var collection = db.GetCollection<BsonDocument>("MagicalCreatureMythologyData");
                var mythDb = collection.FindAll().First(x => x["Name"] == name);
                mythDb["Description"] = description;

                Console.WriteLine(mythDb["Description"].AsString);
                collection.Save(mythDb);
            }
            dbsql.SaveChanges();
        }

        private static List<MagicalCreatureModel> AllCreatures(MagicalCreatureDbContext dbContext)
        {
            return dbContext.MagicalCreatures
                .Where(c => c.LocationId != c.Species.Mythology.LocationId)
                .Select(c => new MagicalCreatureModel
                {
                    Name = c.Name,
                    AggressionWhenSpotted = c.AggressionWhenSpotted,
                    DateSpotted = c.DateSpotted,
                    AssesedDangerLevel = c.AssesedDangerLevel,
                    Location = c.Location.Name,
                    Species = c.Species.Name
                })
                .ToList();
        }

        private static void GeneratePDfReport()
        {
            /* 
            
             var species = new Species();
             species.Name = "Dragon";
             species.Mythology = db.Mythologies.FirstOrDefault();

             var creature = new MagicalCreature();
             creature.Name = "Bob";
             creature.AssesedDangerLevel = DangerLevel.High;
             creature.DateSpotted = DateTime.Now.AddDays(-10);
             creature.AggressionWhenSpotted = AggressionLevel.Aggitated;
             creature.Location = db.Locations.FirstOrDefault();
             creature.Species = species;

             db.MagicalCreatures.Add(creature);
             db.SaveChanges();*/
            var report = new PdfReportGenerator();
            var db = new MagicalCreatureDbContext();
            report.CreateUserReport(db.MagicalCreatures,"Report1.pdf",DateTime.Now);
        }
        private static void ImportMongoToSql()
        {
            var mongoCreator = new MongoCreator();
            var db = mongoCreator.GetDatabase(MongoCreator.DatabaseName, MongoCreator.DatabaseHost);
            var myths = db.GetCollection<BsonDocument>("MagicalCreatureMythologyData");
            var data = myths.FindAll();

            var dbsql = new MagicalCreatureDbContext();

            var locations = dbsql.Locations.Select(o => o.Name).ToString();

            foreach (var item in data)
            {
                var loctionData = item["LocationOfOrigin"].AsString;
                
                if(!locations.Contains(loctionData))
                {
                    var loc = new Location { Name = loctionData };
                    dbsql.Locations.Add(loc);

                    var mythDataNme = item["Name"].AsString;
                    var mythology = new Mythology { Name = mythDataNme, Location = loc };

                    dbsql.Mythologies.Add(mythology);
                }
                else
                {
                    var mythDataNme = item["Name"].AsString;
                    var mythology = new Mythology { Name = mythDataNme, LocationId = locations.IndexOf(loctionData) };
                    dbsql.Mythologies.Add(mythology);
                }

            }

            dbsql.SaveChanges();

        }

        private static void CreteMongoDb()
        {
            var mongoCreator =  new MongoCreator();
          
            var db1 = mongoCreator.GetDatabase(MongoCreator.DatabaseName, MongoCreator.DatabaseHost);
            var transports = db1.GetCollection<BsonDocument>("MagicalCreatureMythologyData");

            var count = transports.FindAll().Count();

            if(count==0)
            {
                mongoCreator.GenerateSampleData();
                transports = db1.GetCollection<BsonDocument>("MagicalCreatureMythologyData");
                count = transports.FindAll().Count();
                Console.WriteLine("MythologiesCreated" + count);
                return;
            }

            transports = db1.GetCollection<BsonDocument>("MagicalCreatureMythologyData");
            count = transports.FindAll().Count();

            Console.WriteLine("Mongo db Exists with " + count);           

        }

        private static void LoadInitialExcelDataFromZipFile()
        {
            ExtractFilesFromZip();

            var dataImporter = new ExcelDataImporter();
            dataImporter.ImportDataFromDirectories("../../../DataSystem/ExtractedFiles");
        }

        private static void ExtractFilesFromZip()
        {
            var zipExtractor = new ZipExtractor();
            zipExtractor.Extract("../../../DataSystem/LocationsData.zip", "../../../DataSystem/ExtractedFiles");
        }

        private static void GenerateSqlDatabBaseIfNeeded()
        {
            var db = new MagicalCreatureDbContext();

            var loc = new Location
            {
                Name = "Sofia"
            };
            if (db.Locations.Count() == 0)
            {
                db.Locations.Add(loc);
            }
                        
            db.SaveChanges();
        }

        private static void JsonReport(ICollection<MagicalCreatureModel> list)
        {
              
            var count = 0;
            foreach (var item in list)
            {
                count++;
               
                var jsonObj = JsonConvert.SerializeObject(item, Newtonsoft.Json.Formatting.Indented);
                System.IO.File.WriteAllText("../../../DataSystem/" + count + ".json", jsonObj.ToString());
            }
        }

        private static void addToMySql(ICollection<MagicalCreatureModel> list)
        {
            UpdateDatabaseMySql();
            var context = new FluentModel();

            var count = context.MagicalCreatures.Count();
            foreach (var item in list)
            {
                
                var obj = item;
                obj.Id = count;
                context.Add(item);
                count++;
            }
            context.SaveChanges();
        }

        private static void XmlReport(ICollection<MagicalCreatureModel> creatures)
        {
            Console.WriteLine("Xml Report Generating");
            var report = new XElement(XName.Get("MagicalCreatureReport"));
            foreach (var c in creatures)
            {
                var personXml = new XElement("Creature",
                    new XElement("name", c.Name),
                    new XElement("dateSpotted", c.DateSpotted),
                    new XElement("aggression", c.AggressionWhenSpotted),
                    new XElement("danger", c.AssesedDangerLevel),
                    new XElement("location", c.Location),
                    new XElement("species", c.Species));

                report.Add(personXml);
            }

            report.Save("../../../DataSystem/report.xml");
        }

        private static void PdfReportFromList(ICollection<MagicalCreatureModel> list)
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

       /* private static void AddElf()
        {
            var db = new MagicalCreatureDbContext();

            var mythology = new Models.Mythology
            {
                Name = "Norse",
                AreaOfOrigin = "Denmakr"
            };

            var goblins = new Models.Species
            {
                Name = "Elf",
                Mythology = mythology,
                Description = "Tall,Fast and pretty",
            };

            db.Species.Add(goblins);
            //context.SaveChanges();

            var loc = new Models.Location { Name = "Plovdiv" };
            var supAb = new Models.SuperNaturalAbility
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
        }*/

        private static ICollection<MagicalCreatureModel> ExtractMagicalCreaturesByMythologyName(string mythology)
        {
            var db = new MagicalCreatureDbContext();

            var list = db.MagicalCreatures
                .Where(c => c.Species.Mythology.Name == "Norse")
                .Select(c => new MagicalCreatureModel
                {
                    Name = c.Name,
                    DateSpotted = c.DateSpotted,
                    AggressionWhenSpotted = c.AggressionWhenSpotted,
                    AssesedDangerLevel = c.AssesedDangerLevel,
                    Location = c.Location.Name,
                    Species = c.Species.Name + " from" + c.Species.Mythology.Name + " mythology"
                })
                .ToList();

            return list;
        }

        private static void UpdateDatabaseMySql()
        {
            using (var context = new FluentModel())
            {
                var schemaHandler = context.GetSchemaHandler();
                EnsureDB(schemaHandler);
            }
        }

        private static void EnsureDB(ISchemaHandler schemaHandler)
        {
            string script = null;
            if (schemaHandler.DatabaseExists())
            {
                script = schemaHandler.CreateUpdateDDLScript(null);
            }
            else
            {
                schemaHandler.CreateDatabase();
                script = schemaHandler.CreateDDLScript();
            }

            if (!string.IsNullOrEmpty(script))
            {
                schemaHandler.ExecuteDDLScript(script);
            }
        }
    }
}
