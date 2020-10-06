using System.Globalization;
using System.Security.Claims;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CK.Rest.TestsBase
{
    public static class TestExtensions
    {
        #region Public Methods

        public static void SetClaimsPrincipal(this ControllerBase controller, uint id, string email, string role)
        {
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new Claim[]
                            {
                                new Claim(ClaimTypes.Name, email),
                                new Claim(ClaimTypes.NameIdentifier, id.ToString(CultureInfo.InvariantCulture)),
                                new Claim(ClaimTypes.Role, role),
                            }, "Mock")),
                },
            };
        }

        public static void SetEmptyContext(this ControllerBase controller)
        {
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext(),
            };
        }

        #endregion Public Methods
    }
}