using CK.Rest.Users.Shared.Forms;

using Microsoft.AspNetCore.Mvc;

namespace CK.Rest.Users.Controllers
{
    public interface IAuthController
    {
        #region Public Methods

        IActionResult IsAuthenticate();

        IActionResult Post(UserCredentialsForm user);

        #endregion Public Methods
    }
}