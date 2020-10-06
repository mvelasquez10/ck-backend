using CK.Rest.Common.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CK.Rest.Common.Setup
{
    public abstract class CKStartup
    {
        #region Public Constructors

        public CKStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion Public Constructors

        #region Protected Properties

        protected IConfiguration Configuration { get; private set; }

        #endregion Protected Properties

        #region Public Methods

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseExceptionMiddleware();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionMiddleware();
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(Configuration);

            services.AddControllers();

            services.AddAuthentication(Configuration);
        }

        #endregion Public Methods
    }
}