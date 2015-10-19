using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.OpenAccess;
using Telerik.OpenAccess.Metadata;

namespace DataAccess
{
    public partial class FluentModel : OpenAccessContext
    {
        private static string connectionStringName = @"connectionId1";

        private static BackendConfiguration backend =
            GetBackendConfiguration();

        private static MetadataSource metadataSource =
            new FluentModelMetadataSource();

        public FluentModel()
            : base(connectionStringName, backend, metadataSource)
        { }

        public IQueryable<MagicalCreatureModel> MagicalCreatures
        {
            get
            {
                return this.GetAll<MagicalCreatureModel>();
            }
        }

        public IQueryable<Location> Locations
        {
            get
            {
                return this.GetAll<Location>();
            }
        }

        public IQueryable<Mythology> Mythologies
        {
            get
            {
                return this.GetAll<Mythology>();
            }
        }

        public IQueryable<Species> Species
        {
            get
            {
                return this.GetAll<Species>();
            }
        }

        public IQueryable<SuperNaturalAbility> SuperNaturalAbilities
        {
            get
            {
                return this.GetAll<SuperNaturalAbility>();
            }
        }

        public static BackendConfiguration GetBackendConfiguration()
        {
            BackendConfiguration backend = new BackendConfiguration();
            backend.Backend = "mysql";
            backend.ProviderName = "MySql.Data.MySqlClient";

            return backend;
        }
    }
}
