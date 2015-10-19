namespace MagicalCreatureDataBase.Models
{
    using System;
    using Enumerations;
    using System.Collections;
    using System.Collections.Generic;

    public class SuperNaturalAbility
    {
        private ICollection<MagicalCreature> magicalCreatures;

        public SuperNaturalAbility()
        {
            this.magicalCreatures = new HashSet<MagicalCreature>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public int? RangeInMeters { get; set; }

        public DangerLevel? DangerLevel { get; set; }

        public virtual ICollection<MagicalCreature> MagicalCreatures
        {
            get { return this.magicalCreatures; }
            set { this.magicalCreatures = value; }
        }

    }
}
