namespace MagicalCreatureDataBase.Data.Migrations
{
    using Models.Enumerations;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Collections.Generic;

    public sealed class ConfigrationMySql : DbMigrationsConfiguration<MagicalCreatureMySqlDbContext>
    {
        public ConfigrationMySql()
        {
            this.AutomaticMigrationsEnabled = true;
            //this.AutomaticMigrationDataLossAllowed = true;
            this.ContextKey = "MagicalCreatureDataBase.Data.MagicalCreatyreDBMySql";
        }

        /*protected override void Seed(MagicalCreatureDbContext context)
        {
        }*/
    }
}
