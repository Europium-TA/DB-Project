namespace MagicalCreatureDataBase.ConsoleClient
{
    using System;
    using System.Linq;
    using MagicalCreatureDataBase.Data;
    using Models;

    public class Program
    {
        public static void Main()
        {
            var db = new MagicalCreatureDbContext();

            var location = new Location
            {
                Name="Sofia"
            };

            db.Locations.Add(location);
            db.SaveChanges();

            Console.WriteLine(db.Locations.Count());
        }
    }
}
