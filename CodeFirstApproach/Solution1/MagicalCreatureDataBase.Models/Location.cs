namespace MagicalCreatureDataBase.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class Location
    {
        private ICollection<MagicalCreature> magicalCreatures;

        public Location()
        {
            this.magicalCreatures = new HashSet<MagicalCreature>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<MagicalCreature> MagicalCreatures
        {
            get { return this.magicalCreatures; }
            set { this.magicalCreatures = value; }
        }
    }
}
