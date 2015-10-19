﻿using System.IO.Compression;

namespace DataAccess.Importers
{
    public class ZipExtractor
    {
        public void Extract(string sourcePath, string destinationPath)
        {
            ZipFile.ExtractToDirectory(sourcePath, destinationPath);
        }
    }
}
