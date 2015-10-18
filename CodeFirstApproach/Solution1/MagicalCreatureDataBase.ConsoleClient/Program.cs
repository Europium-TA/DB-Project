﻿namespace MagicalCreatureDataBase.ConsoleClient
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
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

    public class Program
    {
        public static void Main()
        {
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<MagicalCreatureDbContext>());

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MagicalCreatureDbContext, Configuration>());

            var db = new MagicalCreatureDbContext();

            //var intput = Console.ReadLine();

            var list = ExtractMagicalCreaturesByMythologyName("Norse");

            //PdfReportFromList(list);

            
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
