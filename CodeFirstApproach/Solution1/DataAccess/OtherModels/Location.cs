namespace DataAccess
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class Location
    {
        private ICollection<MagicalCreatureModel> magicalCreatures;

        public Location()
        {
            this.magicalCreatures = new HashSet<MagicalCreatureModel>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual IList<MagicalCreatureModel> MagicalCreatures { get; set; }
    }
}
