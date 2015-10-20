namespace DataAccess
{
    using MagicalCreatureDataBase.Models.Enumerations;
    using System;

    public class MagicalCreatureModel
    {
        public MagicalCreatureModel()
        {
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime DateSpotted { get; set; }

        public DangerLevel AssesedDangerLevel { get; set; }

        public AggressionLevel AggressionWhenSpotted { get; set; }

        // Link to Speicies table
        public string Species { get; set; }

        // Linkt to Locations Table
        public string Location { get; set; }

    }
}
