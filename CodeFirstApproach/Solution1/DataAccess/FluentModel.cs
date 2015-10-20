namespace DataAccess
{
    using System.Linq;
    using Telerik.OpenAccess;
    using Telerik.OpenAccess.Metadata;

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

        public static BackendConfiguration GetBackendConfiguration()
        {
            BackendConfiguration backend = new BackendConfiguration();
            backend.Backend = "mysql";
            backend.ProviderName = "MySql.Data.MySqlClient";

            return backend;
        }
    }
}
