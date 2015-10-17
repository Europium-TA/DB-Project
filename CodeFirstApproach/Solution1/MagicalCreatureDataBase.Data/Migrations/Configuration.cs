namespace MagicalCreatureDataBase.Data.Migrations
{
    using Models.Enumerations;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Collections.Generic;

    public sealed class Configuration : DbMigrationsConfiguration<MagicalCreatureDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
            this.ContextKey = "MagicalCreatureDataBase.Data.MagicalCreatureDbContext";
        }

        protected override void Seed(MagicalCreatureDbContext context)
        {
            var mythilog = new Mythology
            {
                Name = "Galic",
                AreaOfOrigin = "Germany"
            };

            context.Mythologies.AddOrUpdate(
                m => m.Name,
                mythilog
                );
            context.SaveChanges();

            var goblins = new Species
            {
                Name = "Goblins",
                Mythology = mythilog,
                Description = "Small and Fast",
            };
            context.Species.AddOrUpdate(
                s => s.Name,
                goblins
                );
            context.SaveChanges();

            var loc = new Location { Name = "Sofia" };
            var loc2 = new Location { Name = "Varna" };
            var supAb = new SuperNaturalAbility
            {
                Name = "Scratch",
                RangeInMeters = 1,
                DangerLevel = DangerLevel.Low
            };

            context.Locations.AddOrUpdate(
                l => l.Name,
                loc,
                loc2
                );
            context.SaveChanges();

            context.SuperNaturalAbilities.AddOrUpdate(
                sa => sa.Name,
                supAb
                );

            var creature = new MagicalCreature
            {
                Name = "Bob0",
                DateSpotted = DateTime.Now,
                AggressionWhenSpotted = AggressionLevel.Aggitated,
                AssesedDangerLevel = DangerLevel.Medium,
                Species = goblins,
                Location =  loc2,
                SuperNaturalAbilities = new List<SuperNaturalAbility> { supAb }
            };

            context.MagicalCreatures.AddOrUpdate(
                mc => mc.Name,
                creature
                );



            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
