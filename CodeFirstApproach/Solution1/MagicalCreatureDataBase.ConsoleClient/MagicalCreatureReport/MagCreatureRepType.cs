namespace MagicalCreatureDataBase.ConsoleClient.MagicalCreatureReport
{
    using MagicalCreatureDataBase.Models.Enumerations;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MagCreatureRepType
    {
        public string Name { get; set; }

        public string Location { get; set; }

        public DateTime Date { get; set; }

        public string Species { get; set; }
        
        public AggressionLevel Aggression { get; set; }
    }
}
