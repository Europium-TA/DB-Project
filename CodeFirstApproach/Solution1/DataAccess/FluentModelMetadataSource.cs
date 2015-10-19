using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.OpenAccess.Metadata.Fluent;

namespace DataAccess
{
    public partial class FluentModelMetadataSource : FluentMetadataSource
    {
        protected override IList<MappingConfiguration> PrepareMapping()
        {
            List<MappingConfiguration> configurations =
                new List<MappingConfiguration>();

            var creatureMapping = new MappingConfiguration<MagicalCreatureModel>();
            creatureMapping.MapType(cr => new
            {
                Id = cr.Id,
                Name = cr.Name,
                Date = cr.DateSpotted,
                Location = cr.Location,
                Agression = cr.AggressionWhenSpotted,
                Danger = cr.AssesedDangerLevel,
                Species = cr.Species,
            }).ToTable("MagicalCreatures");
            creatureMapping.HasProperty(c => c.Id).IsIdentity();

            configurations.Add(creatureMapping);           

            return configurations;
        }
    }
}
