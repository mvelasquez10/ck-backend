using CK.Rest.Common.Filters;

using Microsoft.AspNetCore.Mvc;

namespace CK.Rest.Common.Extensions
{
    public static class ControllerExtensions
    {
        #region Public Methods

        public static bool IsAdmin(this ControllerBase controller)
        {
            return controller.User != null && controller.User.IsInRole(Role.Admin);
        }

        #endregion Public Methods
    }
}