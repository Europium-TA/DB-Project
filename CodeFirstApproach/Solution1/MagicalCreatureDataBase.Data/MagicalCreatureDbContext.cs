namespace MagicalCreatureDataBase.Data
{
    using Models;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public class MagicalCreatureDbContext : DbContext
    {
        public MagicalCreatureDbContext()
            :base("MagicalCreaturesDb")
        {
        }

        public virtual IDbSet<MagicalCreature> MagicalCreatures { get; set; }

        public virtual IDbSet<Mythology> Mythologies { get; set; }

        public virtual IDbSet<Species> Species { get; set; }

        public virtual IDbSet<Location> Locations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}
