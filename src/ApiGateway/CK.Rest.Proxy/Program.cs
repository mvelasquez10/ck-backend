using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace CK.Rest.Proxy
{
    public static class Program
    {
        #region Public Methods

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        #endregion Public Methods
    }
}

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member