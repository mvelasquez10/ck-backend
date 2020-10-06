using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace CK.Rest.Common.Filters
{
    public class AdminOrSelf : TypeFilterAttribute
    {
        #region Public Constructors

        public AdminOrSelf()
            : base(typeof(AdminOrSelfImp))
        {
        }

        #endregion Public Constructors

        #region Private Classes

        private class AdminOrSelfImp : ActionFilterAttribute
        {
            #region Private Fields

            private readonly ILogger _logger;

            #endregion Private Fields

            #region Public Constructors

            public AdminOrSelfImp(ILoggerFactory loggerFactory)
            {
                _logger = loggerFactory.CreateLogger<AdminOrSelf>();
            }

            #endregion Public Constructors

            #region Public Methods

            public override void OnActionExecuting(ActionExecutingContext context)
            {
                var requestId = (uint)context.ActionArguments["id"];

                if (!context.HttpContext.User.IsInRole(Role.Admin) && context.HttpContext.User.GetId() != requestId)
                {
                    context.Result = new ForbidResult();
                    _logger.LogWarning($"The User id: '{requestId}' was rejected by filter");
                }
            }

            #endregion Public Methods
        }

        #endregion Private Classes
    }
}