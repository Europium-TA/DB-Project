namespace MagicalCreatureDataBase.ConsoleClient
{
    using System;
    using System.Data;
    using System.Data.Entity.Migrations;
    using System.Data.OleDb;
    using System.IO;
    using Ionic.Zip;
    using MagicalCreatureDataBase.Data;
    using MagicalCreatureDataBase.Models;
    using MagicalCreatureDataBase.Models.Enumerations;

    public class ExcelImporter : IExcelImporter
    {
        public ExcelImporter(string filePath, MagicalCreatureDbContext dbContext)
        {
            this.FilePath = filePath;
            this.DbContext = dbContext;
        }

        public string FilePath { get; set; }

        public MagicalCreatureDbContext DbContext { get; set; }

        public void ImportFromExcel()
        {
            try
            {
                ExtractZip(this.FilePath);

                var list = Directory.GetFiles("../../temp/", "*", SearchOption.AllDirectories);

                foreach (var filePath in list)
                {
                    Console.WriteLine("Reading from file {0}", filePath);
                    ConnectToExcel(this.FilePath, this.DbContext);
                }
            }
            finally
            {
                Directory.Delete("../../temp", true);
            }   
        }

        private static void ExtractZip(string filePath)
        {
            using (ZipFile zipExtractor = ZipFile.Read(filePath))
            {
                foreach (var zippedFile in zipExtractor)
                {
                    Console.WriteLine("Extracting {0}", zippedFile.FileName);
                    zippedFile.Extract("../../temp/");
                    Console.WriteLine("Extracted {0}", zippedFile.FileName);
                }
            }
        }

        public static void ConnectToExcel(string filePath, MagicalCreatureDbContext db)
        {
            //string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=Excel 12.0;";
            string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=Yes'";

            using (OleDbConnection excelConnection = new OleDbConnection(connectionString))
            {
                excelConnection.Open();

                DataTable dtExcelSchema;
                dtExcelSchema = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                var sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();

                OleDbCommand excelCommand = new OleDbCommand(string.Format("SELECT * FROM [sheet1$]", sheetName), excelConnection);

                OleDbDataReader reader = excelCommand.ExecuteReader();

                while (reader.Read())
                {
                    MagicalCreature newMonster = new MagicalCreature();

                    newMonster.Name = reader.GetValue(0).ToString();
                    newMonster.DateSpotted = DateTime.Now;
                    newMonster.AssesedDangerLevel = (DangerLevel)int.Parse(reader.GetValue(2).ToString());
                    newMonster.AggressionWhenSpotted = (AggressionLevel)int.Parse(reader.GetValue(3).ToString());
                    newMonster.SpeciesId = 1;
                    newMonster.LocationId = 1;

                    Console.WriteLine("Adding... {0} {1} {2} {3} {4} {5}",
                        newMonster.Name,
                        newMonster.DateSpotted,
                        newMonster.AssesedDangerLevel,
                        newMonster.AggressionWhenSpotted,
                        newMonster.SpeciesId,
                        newMonster.LocationId);

                    db.MagicalCreatures.AddOrUpdate(m => m.DateSpotted, newMonster);
                    db.SaveChanges();
                }

            }
        }
    }
}