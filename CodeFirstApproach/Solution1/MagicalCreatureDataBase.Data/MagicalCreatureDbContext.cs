namespace MagicalCreatureDataBase.Data
{
    using Models;
    using System.Data.Entity;

    public class MagicalCreatureDbContext : DbContext
    {
        public virtual IDbSet<MagicalCreature> MagicalCreatures { get; set; }

        public virtual IDbSet<Mythology> Mythologies { get; set; }

        public virtual IDbSet<Species> Species { get; set; }

        public virtual IDbSet<SuperNaturalAbility> SuperNaturalAbilities { get; set; }

        public virtual IDbSet<Location> Locations { get; set; }
    }
}
