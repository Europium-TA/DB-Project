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
            //this.AutomaticMigrationDataLossAllowed = true;
            this.ContextKey = "MagicalCreatureDataBase.Data.MagicalCreatureDbContext";
        }

        protected override void Seed(MagicalCreatureDbContext context)
        {
            var norseMythology = new Mythology
            {
                Name = "Norse",
                AreaOfOrigin = "Denmark"
            };

            var elfSpecies = new Species
            {
                Name = "Elf",
                Mythology = norseMythology,
                Description = "Tall,Fast and pretty"
            };

            var plovdivLocation = new Location { Name = "Plovdiv" };

            //context.Species.AddOrUpdate(elfSpecies);

            context.Locations.AddOrUpdate(location => location.Name, plovdivLocation);

            context.SuperNaturalAbilities.AddOrUpdate(
                ability => ability.Name,
                new SuperNaturalAbility
                {
                    Name = "Magic Missle",
                    RangeInMeters = 10,
                    DangerLevel = DangerLevel.Medium
                });

            context.MagicalCreatures.AddOrUpdate(
                creature => creature.Name,
                new MagicalCreature
                {
                    Name = "Bob",
                    DateSpotted = DateTime.Now,
                    AggressionWhenSpotted = AggressionLevel.Aggitated,
                    AssesedDangerLevel = DangerLevel.Medium,
                    Species = elfSpecies,
                    Location = plovdivLocation,
                });

            context.SaveChanges();
        }
    }
}
