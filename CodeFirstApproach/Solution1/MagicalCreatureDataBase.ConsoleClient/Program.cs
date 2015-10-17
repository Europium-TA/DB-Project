namespace MagicalCreatureDataBase.ConsoleClient
{
    using System;
    using System.Linq;
    using MagicalCreatureDataBase.Data;
    using Models;
    using System.Data.Entity;
    using Data.Migrations;

    public class Program
    {
        public static void Main()
        {
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<MagicalCreatureDbContext>());

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MagicalCreatureDbContext, Configuration>());

            var db = new MagicalCreatureDbContext();

            

            Console.WriteLine(db.Locations.Count());
        }
    }
}
