using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicalCreatureDataBase.ConsoleClient.Pdf
{
    using System;
    using System.IO;
    using System.Linq;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using Models;

    public class PdfReportGenerator
    {
        private string workingDir = "../../../DataSystem/PDF";

        public PdfReportGenerator()
        {
            if (!Directory.Exists(workingDir))
            {
                Directory.CreateDirectory(workingDir);
            }
        }

        public void CreateUserReport(IQueryable<MagicalCreature> creatures, string fileName, DateTime date)
        {
            using (var fs = new FileStream(Path.Combine(workingDir, fileName), FileMode.Create, FileAccess.Write))
            {
                Console.WriteLine("Generating of UsersReport.pdf initialized.");

                var document = new Document(PageSize.A4, 2, 2, 30, 30);
                PdfWriter.GetInstance(document, fs);
                document.Open();

                int year = date.Year;
                int month = date.Month;
                int day = date.Day;

                var groups = creatures.Select(x => new
                {
                   Name = x.Name,
                   Location = x.Location.Name,
                   AggressionLevel = x.AggressionWhenSpotted,
                   DangerLevel = x.AssesedDangerLevel,
                   Date = x.DateSpotted,
                   Species = x.Species.Name
                }).GroupBy(y => y.DangerLevel).ToList();

                var allCreatures = groups;
                var count = allCreatures.Count;
                var creatureCount = 0;

                for (int i = 0; i < count; i++)
                {
                    var singleCreature = allCreatures[i];

                    var table = new PdfPTable(6);

                    var headerCell = new PdfPCell(new Phrase("SpottedCreatures: " + singleCreature.Key));
                    headerCell.Colspan = 6;
                    headerCell.BackgroundColor = new BaseColor(232, 232, 232);
                    headerCell.HorizontalAlignment = 1;
                    table.AddCell(headerCell);

                    table.AddCell("Name");
                    table.AddCell("Location");
                    table.AddCell("AggresionLevel");
                    table.AddCell("DangerLevel");
                    table.AddCell("Date");
                    table.AddCell("Species");

                    foreach (var creat in singleCreature)
                    {
                        table.AddCell(creat.Name);
                        table.AddCell(creat.Location);
                        table.AddCell(creat.AggressionLevel.ToString());
                        table.AddCell(creat.DangerLevel.ToString());
                        table.AddCell(creat.Date.ToString());
                        table.AddCell(creat.Species);
                        creatureCount = creatureCount + 1;
                    }

                    document.Add(table);
                    document.Add(new Paragraph(new Phrase("\n")));
                }

                document.Add(new Paragraph(new Phrase("\n" + "Total User count: " + creatureCount)));

                var footer = new Paragraph(new Phrase("User Report"));
                footer.Alignment = 1;

                document.Add(footer);
                document.Close();

                Console.WriteLine("Generating of pdf report completed!");
            }
        }
    }
}
