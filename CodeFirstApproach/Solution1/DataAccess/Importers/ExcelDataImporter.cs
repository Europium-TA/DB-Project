namespace DataAccess.Importers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OleDb;
    using System.IO;
    using MagicalCreatureDataBase.Data;
    using MagicalCreatureDataBase.Models;

    public class ExcelDataImporter
    {
        private const string WorksheetFileExtensionPattern = @".xls[x]?\b";
        // TODO: FileNames - MagicalCreatures-19-oct-2015.xls
        private const string CreaturesWorksheetFilePattern = @"\d{2}-\d{2}-\d{4}.xls[x]?\b";
        private const string InvalidFileNameMessage = @"Provided file name is either invalid or does not match 
                                                    the naming convention for an xls/xlsx [{0}] data file.";

        public void ImportDataFromDirectories(string directoryPath)
        {
            IEnumerable<string> filePaths = Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);
                                    // .Where(p => Regex.IsMatch(p, CreaturesWorksheetFilePattern));
            
            var db = new MagicalCreatureDbContext();

            foreach (var path in filePaths)
            {
                foreach (var location in ImportDataFromFile(path))
                {
                    db.Locations.Add(location);
                }
            }
            db.SaveChanges();
        }

        private ICollection<Location> ImportDataFromFile(string filePath)
        {
            OleDbConnection connection = new OleDbConnection();

            connection.ConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0 xml;HDR=yes'", filePath);

            connection.Open();
            Console.WriteLine("Connection open...");
            using (connection)
            {
                var schema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                var sheetName = schema.Rows[0]["TABLE_NAME"].ToString();

                OleDbCommand selectAllRowsCommand = new OleDbCommand("SELECT * FROM [" + sheetName + "]", connection);

                ICollection<Location> importedLocations = new HashSet<Location>();

                using (OleDbDataAdapter adapter = new OleDbDataAdapter(selectAllRowsCommand))
                {
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet);

                    using (DataTableReader reader = dataSet.CreateDataReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                string name = reader["Name"].ToString();

                                var location = new Location()
                                {
                                    Name = name,
                                   
                                };

                                importedLocations.Add(location);
                            }
                            catch (FormatException)
                            { }
                        }
                    }
                }

                return importedLocations;
            }
        }
    }
}
