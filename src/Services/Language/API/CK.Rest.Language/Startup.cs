using CK.Entities;
using CK.Repository;
using CK.Rest.Common.Factories;
using CK.Rest.Common.Setup;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CK.Rest.Languages
{
    public class Startup : CKStartup
    {
        #region Public Constructors

        public Startup(IConfiguration configuration)
            : base(configuration)
        {
            _ = GetRepository();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddScoped(f => GetRepository());
        }

        #endregion Public Methods

        #region Private Methods

        private EntityRepository<Language, uint> GetRepository()
        {
            return RepositoryFactory.GetRepository<Language, uint>(
                                Configuration["LanguageRepository:Assembly"],
                                Configuration["LanguageRepository:ConnectionString"],
                                EntitiesFactory.GetDefaultLanguages());
        }

        #endregion Private Methods
    }
}