using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CK.Rest.Proxy.Filter
{
    internal class Forward : TypeFilterAttribute
    {
        #region Public Constructors

        public Forward()
            : base(typeof(ForwardImp))
        {
        }

        #endregion Public Constructors

        #region Private Classes

        private class ForwardImp : ActionFilterAttribute
        {
            #region Private Fields

            private readonly IConfiguration _config;

            private readonly ILogger _logger;

            #endregion Private Fields

            #region Public Constructors

            public ForwardImp(IConfiguration config, ILoggerFactory loggerFactory)
            {
                _config = config;
                _logger = loggerFactory.CreateLogger<Forward>();
            }

            #endregion Public Constructors

            #region Public Methods

            public override void OnActionExecuting(ActionExecutingContext context)
            {
                base.OnActionExecuting(context);
                try
                {
                    var proxy = new UriBuilder(new Uri(_config[$"Proxies:{(string)context.RouteData.Values["Controller"]}"]))
                    {
                        Path = context.HttpContext.Request.Path,
                        Query = context.HttpContext.Request.QueryString.Value,
                    };

                    var isFromBody = context.ActionDescriptor.Parameters?.FirstOrDefault(x => x.BindingInfo.BindingSource.Id == "Body")?.Name;
                    var body = context.ActionArguments?.FirstOrDefault(x => x.Key == isFromBody);
                    object content = null;
                    if (body != null)
                    {
                        content = body.Value.Value;
                    }

                    using var client = new HttpClient(new HttpClientHandler()
                    {
                        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                    });
                    var result = client.SendAsync(CreateProxiedHttpRequest(context.HttpContext, proxy.Uri, content)).Result;
                    var response = new ObjectResult(result.Content?.ReadAsStringAsync().Result) { StatusCode = (int)result.StatusCode };
                    context.Result = response;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
            }

            #endregion Public Methods

            #region Private Methods

            private static HttpRequestMessage CreateProxiedHttpRequest(HttpContext context, Uri uri, object content)
            {
                var request = context.Request;

                var requestMessage = new HttpRequestMessage(new HttpMethod(request.Method), uri);

                if (content != null)
                {
                    requestMessage.Content = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, context.Request.ContentType);
                }

                foreach (var header in context.Request.Headers)
                {
                    if (!requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray()))
                        requestMessage.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                }

                requestMessage.Headers.Host = uri.Authority;

                return requestMessage;
            }

            #endregion Private Methods
        }

        #endregion Private Classes
    }
}