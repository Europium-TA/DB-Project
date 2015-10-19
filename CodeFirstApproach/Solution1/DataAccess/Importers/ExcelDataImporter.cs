namespace DataAccess.Importers
{
    using MagicalCreatureDataBase.Models.Enumerations;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OleDb;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class ExcelDataImporter
    {
        private const string WorksheetFileExtensionPattern = @".xls[x]?\b";
        // TODO: FileNames - MagicalCreatures-19-oct-2015.xls
        private const string CreaturesWorksheetFilePattern = @"\MagicalCreatures-\d{2}-\w{3}-\d{4}.xls[x]?\b";
        private const string InvalidFileNameMessage = @"Provided file name is either invalid or does not match 
                                                    the naming convention for an xls/xlsx [{0}] data file.";

        public ICollection<MagicalCreatureModel> ImportMagCreaturesDataFromDirectory(string directoryPath)
        {
            IEnumerable<string> filePaths = Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories)
                                     .Where(p => Regex.IsMatch(p, CreaturesWorksheetFilePattern));

            ICollection<MagicalCreatureModel> importedMagCreatures = new HashSet<MagicalCreatureModel>();

            foreach (var path in filePaths)
            {
                foreach (var creature in this.ImportMagCreaturesDataFromFile(path))
                {
                    importedMagCreatures.Add(creature);
                }
            }

            return importedMagCreatures;
        }

        public ICollection<MagicalCreatureModel> ImportMagCreaturesDataFromFile(string filePath)
        {
            if (!Regex.IsMatch(filePath, CreaturesWorksheetFilePattern))
            {
                throw new ArgumentException("Invalid file name!");
            }

            OleDbConnection connection = new OleDbConnection();

            connection.ConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=yes'", filePath);

            connection.Open();

            using (connection)
            {
                var schema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                var sheetName = schema.Rows[0]["TABLE_NAME"].ToString();

                OleDbCommand selectAllRowsCommand = new OleDbCommand("SELECT * FROM [" + sheetName + "]", connection);

                ICollection<MagicalCreatureModel> importedCreatures = new HashSet<MagicalCreatureModel>();

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
                                DateTime date = DateTime.Parse(reader["DateTime"].ToString(), CultureInfo.InvariantCulture);
                                DangerLevel dangerLevel = (DangerLevel)int.Parse(reader["AssesedDangerLevel"].ToString());
                                AggressionLevel aggressionLevel = (AggressionLevel)int.Parse(reader["AggressionWhenSpotted"].ToString());

                                var creature = new MagicalCreatureModel()
                                {
                                    Name = name,
                                    DateSpotted = date,
                                    AssesedDangerLevel = dangerLevel,
                                    AggressionWhenSpotted=aggressionLevel
                                };

                                importedCreatures.Add(creature);
                            }
                            catch (FormatException)
                            { }
                        }
                    }
                }

                return importedCreatures;
            }
        }
    }
}
