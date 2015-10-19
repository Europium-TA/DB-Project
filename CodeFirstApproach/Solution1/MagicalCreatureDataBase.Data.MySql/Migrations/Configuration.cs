namespace MagicalCreatureDataBase.Data.MySql.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<MagicalCreatureMySqlDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MagicalCreatureMySqlDbContext context)
        {
            
        }
    }
}
