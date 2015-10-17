namespace MagicalCreatureDataBase.Models
{
    using System.Collections;
    using System.Collections.Generic;

    public class Mythology
    {
        private ICollection<Species> species;

        public Mythology()
        {
            this.species = new HashSet<Species>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string AreaOfOrigin { get; set; }

        public ICollection<Species> Species { get; set; }
    }
}
