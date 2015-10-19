namespace DataAccess
{
    using MagicalCreatureDataBase.Models;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class Species
    {
       

        public Species()
        {
            this.MagicalCreatures = new List<MagicalCreatureModel>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int MythologyId { get; set; }

        public Mythology Mythology { get; set; }

        public IList<MagicalCreatureModel> MagicalCreatures { get; set; }

    }
}
