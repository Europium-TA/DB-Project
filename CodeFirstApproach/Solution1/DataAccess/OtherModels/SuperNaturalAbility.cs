namespace DataAccess
{
    using System;
    using MagicalCreatureDataBase.Models.Enumerations;
    using System.Collections;
    using System.Collections.Generic;

    public class SuperNaturalAbility
    {
        public SuperNaturalAbility()
        {
            this.MagicalCreatures = new List<MagicalCreatureModel>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public int RangeInMeters { get; set; }

        public DangerLevel DangerLevel { get; set; }

        public virtual IList<MagicalCreatureModel> MagicalCreatures { get; set; }

    }
}
