namespace DataAccess
{

        using System.Collections;
        using System.Collections.Generic;

        public class Mythology
        {
            public Mythology()
            {
                this.Species = new List<Species>();
            }

            public int Id { get; set; }

            public string Name { get; set; }

            public string AreaOfOrigin { get; set; }

            public virtual IList<Species> Species { get; set; }
        }

}
