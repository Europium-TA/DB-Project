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
    using MySql.Data;
    using MySql.Data.Entity;
    using MongoDbStuff;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Bson.Serialization.Attributes;
    using MongoDB.Driver.Builders;
    using Telerik.OpenAccess;
    using DataAccess;
    using DataAccess.Importers;
    using System.IO;

    public class Program
    {


        public static void Main()
        {

            System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<MagicalCreatureDbContext, Configuration>());

            GenerateSqlDatabBaseIfNeeded();

            LoadInitialExcelDataFromZipFile();
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

        private static void MongoDb()
        {
            Console.WriteLine();
            var s = new MongoCreator();
            //s.GenerateSampleData();

            var db1 = s.GetDatabase(MongoCreator.DatabaseName, MongoCreator.DatabaseHost);
            var transports = db1.GetCollection<BsonDocument>("MagicalCreatureDocuments");
            var all = transports.FindAll();
            foreach (var item in all)
            {
                Console.WriteLine(item);
            }
           
        }

        private static void JsonReport(ICollection<MagicalCreatureModel> list)
        {
              
            var count = 0;
            foreach (var item in list)
            {
                count++;
               
                var jsonObj = JsonConvert.SerializeObject(item, Formatting.Indented);
                System.IO.File.WriteAllText("../../../DataSystem/" + count + ".json", jsonObj.ToString());
            }
        }

        private static void addToMySql(ICollection<MagicalCreatureModel> list)
        {

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

            report.Save("report.xml");
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
