using System.Globalization;
using System.Security.Claims;

namespace CK.Rest.Common.Filters
{
    public static class Role
    {
        #region Public Fields

        public const string Admin = "Admin";

        public const string User = "User";

        #endregion Public Fields

        #region Public Methods

        public static uint GetId(this ClaimsPrincipal principal)
        {
            return uint.Parse(principal.FindFirst(ClaimTypes.NameIdentifier).Value, CultureInfo.InvariantCulture);
        }

        #endregion Public Methods
    }
}