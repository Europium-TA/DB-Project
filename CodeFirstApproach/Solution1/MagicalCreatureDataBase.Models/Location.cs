namespace MagicalCreatureDataBase.Models
{
    using System.Collections.Generic;

    public class Location
    {
        private ICollection<MagicalCreature> magicalCreatures;

        private ICollection<Mythology> mythologies;

        public Location()
        {
            this.magicalCreatures = new HashSet<MagicalCreature>();

            this.mythologies = new HashSet<Mythology>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<MagicalCreature> MagicalCreatures
        {
            get { return this.magicalCreatures; }
            set { this.magicalCreatures = value; }
        }

        public virtual ICollection<Mythology> Mythologies
        {
            get { return this.mythologies; }
            set { this.mythologies = value; }
        }
    }
}
