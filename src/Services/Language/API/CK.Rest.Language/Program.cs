using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using static CK.Rest.Common.Extensions.ServiceExtensions;

namespace CK.Rest.Languages
{
    public static class Program
    {
        #region Public Methods

        public static IHostBuilder CreateHostBuilder(string[] args) => CKBuilder<Startup>(args);

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        #endregion Public Methods
    }
}