# Inno.Player.Browser

[Runtime 索引](README.md) · [Wiki 首页](../README.md) · [共享 Player](Inno.Player.Runtime.md) · [Web 打包](../build/Inno.Build.Platform.Browser.md)

## 职责与边界

Web 平台进程入口，只准备浏览器内容、JavaScript 帧调度和浏览器持久存储，并将宿主能力传给 PlayerApplication。游戏循环、模块校验、Scene、Settings、渲染和资源退休属于 Inno.Player.Runtime；没有源码链接，也没有独立 BrowserAdapterCatalog。

## API、初始化和工作流

没有 public/protected API，不是脚本参考程序集。`Program` 委托 `BrowserPlayerComposition`；`BrowserContentLoader` 准备内容，`BrowserBridge` 隔离 JavaScript 回调。composition 配置 DefaultAdapterCatalog 的 storageProviders、生成的静态 metadata/activator 与 ScheduledShellFrameDriver，然后调用 PlayerApplication.RunAsync。

在托管发布阶段，游戏程序集与引擎 references、相同 Native facade 的目标静态 archive 一起链接为 .NET WASM。`MonoWasmDeploymentCompiler` 提供解释执行与 AOT 两种发布选择，平台 packager 只负责站点布局。wwwroot 的 JavaScript 管理浏览器帧机会、页面状态和持久文件系统。发行为普通静态站点，需通过 HTTP/HTTPS 托管。

## 生命周期和错误

启动失败向页面和 Console 报错；帧等待观察取消。浏览器宿主使用同一静态部署 activator，不影响 Editor 的卸载和 GC 校验。线程能力由 entry 显式选择 single-thread jobs、owner-thread render、inline logging。浏览器 Input 继续走 SDL Platform → Core Events → 同一 Input Runtime。
