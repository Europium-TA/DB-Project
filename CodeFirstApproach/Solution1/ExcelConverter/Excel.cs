namespace MagicalCreatureDataBase.ConsoleClient.Excell
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OleDb;
    using System.IO;
    using System.IO.Compression;
    using Models;
    using Data;

    public class Excel
    {
        private const string extractPath = "../../../DataSystem";

        public void SelectExcelFilesFromZip(string path)
        {
            using (ZipArchive archive = ZipFile.Open(path, ZipArchiveMode.Update))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.FullName.EndsWith(".xls"))
                    {
                        entry.ExtractToFile(Path.Combine(extractPath, entry.Name));
                        string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Path.Combine(extractPath, entry.Name) + ";Extended Properties='Excel 12.0 xml;HDR=Yes';";

                        OleDbConnection connection = new OleDbConnection(connectionString);

                        using (connection)
                        {
                            connection.Open();
                            var excelSchema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            var sheetName = excelSchema.Rows[0]["TABLE_NAME"].ToString();
                            this.ReadExcelData(connection, sheetName);
                        }
                    }
                }
            }
        }

        private void ReadExcelData(OleDbConnection conn, string sheetName)
        {
            Console.WriteLine("Reading data...");
            var excelDbCommand = new OleDbCommand(@"SELECT * FROM [" + sheetName + "]", conn);
            using (var oleDbDataAdapter = new OleDbDataAdapter(excelDbCommand))
            {
                DataSet ds = new DataSet();
                oleDbDataAdapter.Fill(ds);
                var locations = new List<Location>();
                using (var reader = ds.CreateDataReader())
                {
                    while (reader.Read())
                    {
                        var loc = new Location();
                        loc.Name = reader["City"].ToString();
                        locations.Add(loc);
                    }
                }

                var db = new MagicalCreatureDbContext();
                foreach (var loc in locations)
                {
                    db.Locations.Add(loc);
                }

                db.SaveChanges();
            }
        }
    }
}
