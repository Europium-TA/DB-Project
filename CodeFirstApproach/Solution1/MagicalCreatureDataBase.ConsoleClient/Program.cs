namespace MagicalCreatureDataBase.ConsoleClient
{
    using System;
    using System.Data;
    using System.Data.OleDb;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using Ionic.Zip;
    using MagicalCreatureDataBase.Data;
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

    public class Program
    {
        public static void Main()
        {
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<MagicalCreatureDbContext>());

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MagicalCreatureDbContext, Configuration>());

            var db = new MagicalCreatureDbContext();

            //var hoi = db.Mythologies.FirstOrDefault(m => m.Name == "Norse");
            //Console.WriteLine(hoi.ToString());

            try
            {
                ExtractZip("../../Sightings.zip");

                var list = Directory.GetFiles("../../temp/", "*", SearchOption.AllDirectories);

                foreach (var filePath in list)
                {
                    Console.WriteLine("Reading from file {0}", filePath);
                    Console.WriteLine("Database exist: {0}", db.Database.Exists());
                    ConnectToExcel(filePath, db);
                }
            }
            finally
            {
                Directory.Delete("../../temp", true);
            }   

            //var intput = Console.ReadLine();

            //var list = ExtractMagicalCreaturesByMythologyName("Norse");

            //XmlReport(list);
            //PdfReportFromList(list);
            //JsonReport(list);

        }

        public static void ConnectToExcel(string filePath, MagicalCreatureDbContext db)
        {
            string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=Yes'";

            using (OleDbConnection excelConnection = new OleDbConnection(connectionString))
            {
                excelConnection.Open();

                DataTable dtExcelSchema;
                dtExcelSchema = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                var sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();

                OleDbCommand excelCommand = new OleDbCommand(string.Format("SELECT * FROM [Sheet1$]", sheetName), excelConnection);

                OleDbDataReader reader = excelCommand.ExecuteReader();                
 
                while (reader.Read())
                {
                    MagicalCreature newMonster = new MagicalCreature();

                    newMonster.Name = reader.GetValue(0).ToString();
                    newMonster.AssesedDangerLevel = (DangerLevel)int.Parse(reader.GetValue(2).ToString());
                    newMonster.AggressionWhenSpotted = (AggressionLevel)int.Parse(reader.GetValue(3).ToString());

                    Console.WriteLine("Adding... {0} {1} {2} {3} {4} {5}",
                        reader.GetValue(0),
                        reader.GetValue(1),
                        reader.GetValue(2),
                        reader.GetValue(3),
                        reader.GetValue(4),
                        reader.GetValue(5));

                    db.MagicalCreatures.AddOrUpdate(newMonster); 
                    db.SaveChanges();
                }

            }       
        }

        public static void ExtractZip(string filePath)
        {
            using (ZipFile zipExtractor = ZipFile.Read(filePath))
            {
                foreach (var zippedFile in zipExtractor)
                {
                    zippedFile.Extract("../../temp/");
                }
            }
        }

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
