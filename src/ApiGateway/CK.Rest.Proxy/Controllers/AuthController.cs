using CK.Rest.Proxy.Filter;
using CK.Rest.Users.Controllers;
using CK.Rest.Users.Shared.Forms;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CK.Rest.Proxy.Controllers
{
    /// <summary>
    /// AuthController Proxy
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Forward]
    public class AuthController : ControllerBase, IAuthController
    {
        #region Public Methods

        /// <summary>
        /// Test if the token is valid
        /// </summary>
        /// <remarks>
        /// Needs <b>Authorization</b> header:
        /// <p/>
        /// <i>Authorization : bearer &lt;token&gt;</i>
        /// </remarks>
        /// <returns>The operation result</returns>
        /// ///
        /// <response code="200">Returns the token is valid</response>
        /// <response code="401">If the token is invalid</response>
        [HttpGet]
        public IActionResult IsAuthenticate()
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Authenticate the user to adquire an autorization token
        /// </summary>
        /// <remarks>
        /// Can be execute by anonymous users
        /// </remarks>
        /// <param name="user">The user credentials</param>
        /// <returns>The operation result</returns>
        /// ///
        /// <response code="200">Returns the user or and error if the credantial fail</response>
        [HttpPost]
        public IActionResult Post([FromBody] UserCredentialsForm user)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        #endregion Public Methods
    }
}