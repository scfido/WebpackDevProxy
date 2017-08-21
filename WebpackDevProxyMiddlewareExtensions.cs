
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Yalla.WebServer.Middleware
{
    public static partial class WebpackDevProxyMiddlewareExtensions
    {

        public static IApplicationBuilder UseWebpackDevProxy(this IApplicationBuilder builder, WebpackDevProxyMiddlewareOptions options)
        {
            return builder
                .UseMiddleware<WebpackDevHtmlProxyMiddleware>(options)
                .UseMiddleware<WebpackDevHotReplaceProxyMiddleware>(options);
        }
    }
}