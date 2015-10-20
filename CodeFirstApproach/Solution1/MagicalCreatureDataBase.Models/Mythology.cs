namespace MagicalCreatureDataBase.Models
{
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

        public string Discription { get; set; }

        public int LocationId { get; set; }

        public virtual Location Location { get; set; }

        public virtual ICollection<Species> Species
        {
            get { return this.species; }
            set { this.species = value; }
        }
    }
}
