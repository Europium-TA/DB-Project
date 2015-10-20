namespace MagicalCreatureDataBase.Models
{
    using System.Collections.Generic;

    public class Species
    {
        private ICollection<MagicalCreature> magicalCreatures;

        public Species()
        {
            this.magicalCreatures = new HashSet<MagicalCreature>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int MythologyId { get; set; }

        public virtual Mythology Mythology { get; set; }

        public virtual ICollection<MagicalCreature> MagicalCreatures
        {
            get { return this.magicalCreatures; }
            set { this.magicalCreatures = value; }
        }

    }
}
