using System.IO;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CK.Rest.Proxy.Controllers
{
    /// <summary>
    /// ResetController Proxy
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class ResetController : ControllerBase
    {
        #region Private Fields

        private readonly IConfiguration _configuration;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">Injected configuration</param>
        public ResetController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Reset the repository
        /// </summary>
        /// <remarks>
        /// Can be execute by anonymous users on debug enviroments
        /// </remarks>
        /// <returns>The operation result</returns>
        /// <response code="200">The operation succeded</response>
        /// <response code="500">There was a problem operation</response>
        [HttpDelete]
        public IActionResult Get()
        {
#if DEBUG
            var repo = new FileInfo(_configuration["Repo"]);
            if (repo.Exists)
            {
                repo.Delete();
            }

            return Ok(_configuration["Repo"]);
#else
            return Ok("This is not allowed on production, no real action was done");
#endif
        }

#endregion Public Methods
    }
}