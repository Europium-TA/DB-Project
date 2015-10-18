namespace MagicalCreatureDataBase.ConsoleClient
{
    using System;
    using System.Linq;
    using MagicalCreatureDataBase.Data;
    using Models;
    using System.Data.Entity;
    using Data.Migrations;
    using System.Data.Entity.Migrations;
    using Models.Enumerations;

    public class Program
    {
        public static void Main()
        {
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<MagicalCreatureDbContext>());

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MagicalCreatureDbContext, Configuration>());

            var db = new MagicalCreatureDbContext();

            var mythology = new Mythology
            {
                Name = "Norse",
                AreaOfOrigin = "Denmakr"
            };

            db.Mythologies.Add(mythology);


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

            Console.WriteLine(db.MagicalCreatures.Count());
        }
    }
}
