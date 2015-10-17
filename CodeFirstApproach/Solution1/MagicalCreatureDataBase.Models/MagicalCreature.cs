namespace MagicalCreatureDataBase.Models
{
    using System;
    using Enumerations;
    using System.Collections;
    using System.Collections.Generic;

    public class MagicalCreature
    {
        private ICollection<SuperNaturalAbility> superNaturalAbilities;

        public MagicalCreature()
        {
            this.superNaturalAbilities = new HashSet<SuperNaturalAbility>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime DateSpotted { get; set; }

        public DangerLevel AssesedDangerLevel { get; set; }

        public AggressionLevel AggressionWhenSpotted { get; set; }

        public int SpeciesId { get; set; }

        public Species Species { get; set; }

        public int LocationId { get; set; }

        public Location Location { get; set; }

        public virtual ICollection<SuperNaturalAbility> SuperNaturalAbilities { get; set; }
    }
}
