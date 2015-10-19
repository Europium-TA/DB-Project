namespace MagicalCreatureDataBase.Data
{
    using Models;
    using System.Data.Entity;

    public class MagicalCreatureMySqlDbContext : DbContext
    {
        public MagicalCreatureMySqlDbContext() : this("MyDB") { }
        public MagicalCreatureMySqlDbContext(string connStringName) : base(connStringName) { }
        static MagicalCreatureMySqlDbContext()
        {
            // static constructors are guaranteed to only fire once per application.
            // I do this here instead of App_Start so I can avoid including EF
            // in my MVC project (I use UnitOfWork/Repository pattern instead)
            DbConfiguration.SetConfiguration(new MySql.Data.Entity.MySqlEFConfiguration());
        }


        public virtual IDbSet<MagicalCreature> MagicalCreatures { get; set; }

        public virtual IDbSet<Mythology> Mythologies { get; set; }

        public virtual IDbSet<Species> Species { get; set; }

        public virtual IDbSet<SuperNaturalAbility> SuperNaturalAbilities { get; set; }

        public virtual IDbSet<Location> Locations { get; set; }

    }
}
