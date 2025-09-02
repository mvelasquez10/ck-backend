using CK.Entities;
using CK.Repository;
using CK.Rest.Common.Factories;
using CK.Rest.Common.Setup;
using CK.Rest.Users.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CK.Rest.Users
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
            services.AddScoped<IAuthenticationHelper, AuthenticationHelper>();
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
            });

            base.Configure(app, env);
        }

        #endregion Public Methods

        #region Private Methods

        private EntityRepository<User, uint> GetRepository()
        {
            return RepositoryFactory.GetRepository<User, uint>(
                                Configuration["UserRepository:Assembly"],
                                Configuration["UserRepository:ConnectionString"],
                                EntitiesFactory.GetDefaultUsers());
        }

        #endregion Private Methods
    }
}
