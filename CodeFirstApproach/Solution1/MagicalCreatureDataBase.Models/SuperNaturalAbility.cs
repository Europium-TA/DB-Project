namespace MagicalCreatureDataBase.Models
{
    using System;
    using Enumerations;

    public class SuperNaturalAbility
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int RangeInMeters { get; set; }

        public DangerLevel MyProperty { get; set; }
    }
}
