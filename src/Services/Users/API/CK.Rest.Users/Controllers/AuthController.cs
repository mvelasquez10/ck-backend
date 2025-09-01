using CK.Rest.Common.Shared;
using CK.Rest.Common.Shared.Forms;
using CK.Rest.Users.Helpers;
using CK.Rest.Users.Shared.Forms;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CK.Rest.Users.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AuthController : Controller, IAuthController
    {
        #region Private Fields

        private readonly IAuthenticationHelper _authenticationHelper;

        #endregion Private Fields

        #region Public Constructors

        public AuthController(IAuthenticationHelper authenticationHelper)
        {
            _authenticationHelper = authenticationHelper;
        }

        #endregion Public Constructors

        #region Public Methods

        [HttpGet]
        public IActionResult IsAuthenticate()
        {
            return User.Identity.IsAuthenticated ? Ok() : (IActionResult)Unauthorized("Invalid user or credentials");
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Post([FromBody] UserCredentialsForm user)
        {
            var userResult = _authenticationHelper.Authenticate(user.Email.ToUnescapeDataString(), user.Password.ToUnescapeDataString());
            if (userResult is null)
                return Ok(new ErrorResult { Error = "Invalid user or credentials" });
            return Ok(userResult);
        }

        #endregion Public Methods
    }
}