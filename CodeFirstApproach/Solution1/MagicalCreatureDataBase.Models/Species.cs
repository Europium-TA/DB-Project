namespace MagicalCreatureDataBase.Models
{
    using System;

    public class Species
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual int MythologyId { get; set; }

        public virtual Mythology Mythology { get; set; }

    }
}
