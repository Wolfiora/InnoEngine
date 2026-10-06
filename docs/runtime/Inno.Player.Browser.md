# Inno.Player.Browser

[Runtime 索引](README.md) · [Wiki 首页](../README.md) · [共享 Player](Inno.Player.Runtime.md) · [Web 打包](../build/Inno.Build.Platform.Browser.md)

## 职责与边界

Web 平台进程入口，只准备浏览器内容、JavaScript 帧调度和浏览器持久存储，并将宿主能力传给 PlayerApplication。游戏循环、模块校验、Scene、Settings、渲染和资源退休属于 Inno.Player.Runtime；没有源码链接，也没有独立 BrowserAdapterCatalog。

## API、初始化和工作流

没有 public/protected API，不是脚本参考程序集。`Program` 委托 `BrowserPlayerComposition`；`HttpPlayerContentSource` 下载部署 metadata 并交付 owned Pack stream，`BrowserBridge` 隔离 JavaScript 回调。composition 注入 storage catalog、生成的静态 metadata/activator 与 ScheduledShellFrameDriver，然后调用 PlayerApplication.RunAsync。

在托管发布阶段，游戏程序集与引擎 references、相同 Native facade 的目标静态 archive 一起链接为 .NET WASM。`MonoWasmDeploymentCompiler` 提供解释执行与 AOT 两种发布选择，平台 packager 只负责站点布局。wwwroot 的 JavaScript 管理浏览器帧机会、页面状态和持久文件系统。发行为普通静态站点，需通过 HTTP/HTTPS 托管。

## 生命周期和错误

启动失败向页面和 Console 报错；帧等待观察取消。浏览器宿主使用同一静态部署 activator，不影响 Editor 的卸载和 GC 校验。线程能力由 entry 显式选择 single-thread jobs、owner-thread render、inline logging。浏览器 Input 继续走 SDL Platform → Core Events → 同一 Input Runtime。






## 本轮边界与所有权

HttpPlayerContentSource 下载同一 catalog.inno 与 manifest，使用 catalog 中唯一 Pack 描述，交出 owned seekable Pack stream。ContentPackReader 验证后供共享 Player 直接读取；不先写 /Content 再解压第二份目录，不使用 content-pack.txt。BrowserBridge 仅承担 JS 生命周期/系统回调。

## 当前源码公开 API 清单

以下仅列出当前程序集自己声明的 public/protected 契约；继承成员遵循所属基类页面。internal/private 实现不作为稳定公开 API。签名依据当前源码语义模型生成，行为、参数、异常与所有权说明同时以对应英文 XML 为准。

## 项目依赖

- [Inno.Player.Runtime](Inno.Player.Runtime.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Adapter.Default](Inno.Adapter.Default.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Adapter.Storage.Browser](../storage/Inno.Adapter.Storage.Browser.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：项目引用；公开签名可见性由语义边界检查确认。

共同 MSBuild 注入的 analyzer 与编译规则属于构建依赖，完整有效项目图记录在本轮验收证据中。
