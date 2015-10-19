

namespace FluentModelClient
{
    using Telerik.OpenAccess;
    using DataAccess;

    namespace FluentModelClient
    {
        class Program
        {
            static void Main(string[] args)
            {
                UpdateDatabase();
            }

            private static void UpdateDatabase()
            {
                using (var context = new FluentModel())
                {
                    var schemaHandler = context.GetSchemaHandler();
                    EnsureDB(schemaHandler);
                }
            }

            private static void EnsureDB(ISchemaHandler schemaHandler)
            {
                string script = null;
                if (schemaHandler.DatabaseExists())
                {
                    script = schemaHandler.CreateUpdateDDLScript(null);
                }
                else
                {
                    schemaHandler.CreateDatabase();
                    script = schemaHandler.CreateDDLScript();
                }

                if (!string.IsNullOrEmpty(script))
                {
                    schemaHandler.ExecuteDDLScript(script);
                }
            }
        }
    }
}