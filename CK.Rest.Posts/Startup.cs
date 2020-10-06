using CK.Entities;
using CK.Repository;
using CK.Rest.Common.Factories;
using CK.Rest.Common.Setup;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CK.Rest.Posts
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

        private EntityRepository<Post, uint> GetRepository()
        {
            return RepositoryFactory.GetRepository<Post, uint>(
                                Configuration["PostRepository:Assembly"],
                                Configuration["PostRepository:ConnectionString"],
                                EntitiesFactory.GetDefaultPosts());
        }

        #endregion Private Methods
    }
}