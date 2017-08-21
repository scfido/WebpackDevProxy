# 简介
项目使用React作为客户端框架时，时常搭配Webpack打包JS脚本，开发阶段Webpack将JS脚本打包到内存中，通过Webpack Dev Server访问，并具有文件更改热更新特性。当软件开发完毕正式发布时，Webpack将JS脚本打包成文件，复制到应用服务目录中，用过应用服务的Http Server访问。

这样就造成了开发阶段和发布阶段两个JS来源不一致，导致经常要手动修改HTML引用JS脚本路径。

使用WebpackDevProxyMiddleware后，应用Http Server会将开发阶段的JS请求自动代理到Webpack Dev Server，这样HTML引用JS脚本路径就可以不需要修改。

# 使用方法

以下代码在ASP.NET Core 2.0中测试通过。

将本项目的四个cs文件复制到你的项目中，并在`Startup.cs`修改以下内容。

添加`using Yalla.WebServer.Middleware`namespace。
在`Configure`方法中加入`UseWebpackDevProxy()`

```cs
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseWebpackDevProxy(new WebpackDevProxyMiddlewareOptions
        {
            Host = "localhost",
            Port = "8000",
            PublicPath = "/dist"
        });
    }
}
```

## 参数说明

|字段|说明|
|--------|--------|
|WebpackDevProxyMiddlewareOptions.Host|Webpack Dev Server的主机地址，例如192.168.0.1|
|WebpackDevProxyMiddlewareOptions.Port|Webpack Dev Server的主机地址，例如192.168.0.1|
|WebpackDevProxyMiddlewareOptions.PublicPath|Webpack.config文件中设置的JS文件publicPath,例如/dist，注意结尾不要添加"/"|
