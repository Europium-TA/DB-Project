namespace MagicalCreatureDataBase.ConsoleClient.MongoDbStuff
{
    using System;
    using MagicalCreatureDataBase.Models.Enumerations;

    public class MongDbDataStruct
    {
        public string Name { get; set; }

        public DateTime DateSpoted { get; set; }

        public DangerLevel DLevel { get; set; }

        public AggressionLevel AggressionWhenSpotted { get; set; }

        public string Spices { get; set; }

        public string Mythology { get; set; }

        public string[] Abilities { get; set; }

        public string Location { get; set; }
    }
}
