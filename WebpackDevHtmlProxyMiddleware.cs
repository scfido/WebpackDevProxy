
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Yalla.WebServer.Middleware
{
    public class WebpackDevHtmlProxyMiddleware
    {
        // Fields
        private readonly RequestDelegate _next;
        private readonly WebpackDevProxyMiddlewareOptions options;
        //private readonly string path;

        // Constructors
        public WebpackDevHtmlProxyMiddleware(RequestDelegate next, WebpackDevProxyMiddlewareOptions options)
        {
            this._next = next;
            this.options = options;
        }

        // Methods
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments(options.PublicPath)) //处理Webpack dev server请求
            {
                string url = $"http://{options.Host}:{options.Port}{context.Request.Path}";
                await Proxy(url, context);
                return;
            }

            // Next
            await _next.Invoke(context);
        }


        private async Task Proxy(string url, HttpContext context)
        {
            var client = new HttpClient();
            try
            {
                //Console.WriteLine(url);
                foreach (var header in context.Request.Headers)
                {
                    //Console.WriteLine($"  {header.Key}:{header.Value.ToString()}");
                    client.DefaultRequestHeaders.Add(header.Key, header.Value.ToList());
                }

                var response = await client.GetAsync(url);

                var content = await response.Content.ReadAsStringAsync();
                var contentType = response.Content.Headers.ContentType;
                context.Response.Headers.Add("Content-Type", contentType.MediaType);
                await context.Response.WriteAsync(content, Encoding.GetEncoding(contentType.CharSet), CancellationToken.None);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }
    }
}