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

        // Link to Speicies table
        public int SpeciesId { get; set; }

        public Species Species { get; set; }

        // Linkt to Locations Table
        public int LocationId { get; set; }

        public Location Location { get; set; }

        //Link to SuperNaturalAbilities Talbe
        public virtual ICollection<SuperNaturalAbility> SuperNaturalAbilities
        {
            get { return this.superNaturalAbilities; }
            set { this.superNaturalAbilities = value; }
        }
    }
}
