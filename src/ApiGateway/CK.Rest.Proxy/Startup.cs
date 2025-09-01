using System;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;

using CK.Rest.Proxy.Filter;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace CK.Rest.Proxy
{
    public class Startup
    {
        #region Public Constructors

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion Public Constructors

        #region Public Properties

        public IConfiguration Configuration { get; }

        #endregion Public Properties

        #region Public Methods

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CK API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseCors(options => options
                .WithOrigins(new[]
                {
                    "https://localhost",
                    "http://localhost",
                })
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressMapClientErrors = true;
            });
            services.AddControllers();

            services.AddControllersWithViews()
                .AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            services.AddScoped<Forward>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CK API", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                var openApiSecurityScheme = new OpenApiSecurityScheme
                {
                    Description =
                    "JWT Authorization header using the Bearer scheme. <br/> " +
                    "Enter your token in the text input below.",
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                };

                var securityScheme = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference()
                    {
                        Id = "JWT",
                        Type = ReferenceType.SecurityScheme,
                    },
                };

                var securityRequirements = new OpenApiSecurityRequirement()
                {
                    { securityScheme, Array.Empty<string>() },
                };

                c.AddSecurityDefinition("JWT", openApiSecurityScheme);
                c.AddSecurityRequirement(securityRequirements);
            });
        }

        #endregion Public Methods
    }
}

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member