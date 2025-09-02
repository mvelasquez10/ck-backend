using System;
using System.IO;
using System.Text;

using CK.Rest.Common.Middleware;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CK.Rest.Common.Extensions
{
    public static class ServiceExtensions
    {
        #region Public Methods

        public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var key = Encoding.UTF8.GetBytes(configuration["Common:Secret"]);
            var keyId = configuration["Common:KeyId"];

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key) { KeyId = keyId },
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                };
            });
        }

        public static void AddLogging(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConfiguration(configuration);
                loggingBuilder.AddFile(o => o.RootPath = AppContext.BaseDirectory);
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });
        }

        public static IHostBuilder CKBuilder<T>(string[] args)
            where T : class =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<T>();
                webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    config.ConfigurationBuilder(env.ContentRootPath, env.EnvironmentName);
                });
            });

        public static void ConfigurationBuilder(this IConfigurationBuilder builder, string contentRootPath, string environmentName)
        {
            var commonFolder = Path.Combine(contentRootPath, @"..\CK.Rest.Common");

            builder
                .AddJsonFile(Path.Combine(commonFolder, "sharedsettings.json"), optional: true)
                .AddJsonFile(Path.Combine(commonFolder, $"sharedsettings.{environmentName}.json"), optional: true)
                .AddJsonFile("sharedsettings.json", optional: true)
                .AddJsonFile($"sharedsettings.{environmentName}.json", optional: true)
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
        }

        public static void UseExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }

        #endregion Public Methods
    }
}
