
namespace Yalla.WebServer.Middleware
{
    /// <summary>
    /// WebpackDevProxy 参数
    /// </summary>
    public class WebpackDevProxyMiddlewareOptions
    {
        /// <summary>
        /// Webpack Dev Server的主机地址。
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Webpack Dev Server的主机端口。
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// Webpack.config文件中设置的publicPath，
        /// </summary>
        public string PublicPath { get; set; }

    }
}