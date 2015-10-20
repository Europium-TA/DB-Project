namespace DataAccess.Importers
{
    using System.IO.Compression;

    public class ZipExtractor
    {
        public void Extract(string sourcePath, string destinationPath)
        {
            ZipFile.ExtractToDirectory(sourcePath, destinationPath);
        }
    }
}
