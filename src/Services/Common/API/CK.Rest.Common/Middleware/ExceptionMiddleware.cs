using System;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CK.Rest.Common.Middleware
{
    public class ExceptionMiddleware
    {
        #region Private Fields

        private readonly ILogger _logger;

        private readonly RequestDelegate _next;

        #endregion Private Fields

        #region Public Constructors

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex).ConfigureAwait(true);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response
                .WriteAsync(new ErrorDetails($"Internal Server Error: ({exception.GetType().Name}) {exception.Message}")
                .ToString());
        }

        #endregion Private Methods
    }
}