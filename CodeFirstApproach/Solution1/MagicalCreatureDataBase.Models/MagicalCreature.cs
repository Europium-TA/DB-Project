namespace MagicalCreatureDataBase.Models
{
    using System;
    using Enumerations;
    using System.Collections;
    using System.Collections.Generic;

    public class MagicalCreature
    {
        public MagicalCreature()
        {
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime DateSpotted { get; set; }

        public DangerLevel AssesedDangerLevel { get; set; }

        public AggressionLevel AggressionWhenSpotted { get; set; }

        // Link to Speicies table
        public int SpeciesId { get; set; }

        public virtual Species Species { get; set; }

        // Linkt to Locations Table
        public int LocationId { get; set; }

        public virtual Location Location { get; set; }

    }
}
