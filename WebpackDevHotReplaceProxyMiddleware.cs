
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Yalla.WebServer.Middleware
{
    public class WebpackDevHotReplaceProxyMiddleware
    {
        // Fields
        private readonly RequestDelegate _next;
        private readonly WebpackDevProxyMiddlewareOptions options;

        // Constructors
        public WebpackDevHotReplaceProxyMiddleware(RequestDelegate next, WebpackDevProxyMiddlewareOptions options)
        {
            this._next = next;
            this.options = options;
        }

        // Methods
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/__webpack_hmr")) //处理Webpack热加载请求的请求
            {
                var response = context.Response;
                response.Headers.Add("Content-Type", "text/event-stream");
                string url = $"http://{options.Host}:{options.Port}{context.Request.Path}";

                WebClient client = new WebClient();
                using (Stream s = client.OpenRead(url))
                {
                    byte[] data = new byte[1024 * 4];
                    int count = await s.ReadAsync(data, 0, data.Length);
                    while (count > 0)
                    {
                        await response.Body.WriteAsync(data, 0, count);
                        response.Body.Flush();
                        count = await s.ReadAsync(data, 0, data.Length);
                    }
                }

                response.Body.Close();
            }

            // Next
            await _next.Invoke(context);
        }
    }
}