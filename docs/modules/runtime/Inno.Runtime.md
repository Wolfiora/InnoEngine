# Inno.Runtime

[分类索引](README.md) · [Wiki 首页](../../README.md)

## 职责与边界

Session 通过 `contentStore` 读取运行内容，通过 `createLogSink` 接管自己的日志 sink；不自行创建文件日志或部署目录。EngineHostBuilder 的 metadata 来源显式注入；动态 shadow-copy 根属于 DotNet module source。Settings 来自注入的文档来源，Player 使用只读文档。Session 退出先停止新工作、取消并完成任务，再退休子系统、资源与事件订阅；失败保留 Pending/Faulted 语义。

## 组合与生命周期

EngineHost 由模块、类型及序列化来源建立；Session 的内容 store 是借用边界，日志 factory 创建的 sink 由 Session 独占释放。Edit、Play 和 Player 使用独立 identity、event、diagnostic 与资源作用域。

```csharp
using Inno.Content;
using Inno.Runtime;

static RuntimeSession CreateSession(
    EngineHost host,
    IRuntimeContentStore content
) {
    return host.CreateSession(new RuntimeSessionOptions
    {
        kind = RuntimeSessionKind.Player,
        applicationId = "sample.game",
        contentStore = content,
        jobExecutionMode = RuntimeJobExecutionMode.SingleThread
    });
}
```

宿主先配置 metadata、完整引用解析上下文和 subsystem factories，再创建 Session。先退出 Session，再关闭 content；创建失败按反序退休已取得资源。Pending 退休保留 owner，Faulted generation 禁止继续 Play/Build/Export，需重启 Host。动态模块路径仅属于 DotNet source，静态模块不需要 shadow-copy cache。

Asset、Settings 和 Rendering 的内容读取分别由其公开契约完成；共享 Session 不读取部署目录，也不创建日志文件。完整 Player 工作流见 [Player Runtime](Inno.Player.Runtime.md)，代际规则见 [Identity 与 Reload](../../architecture/identity-and-reload.md)。

## API 参考

[完整声明、行为与调用约束](Inno.Runtime/api.md)

<!-- api-include: Inno.Runtime/api.md -->

## 项目依赖

- [Inno.Extensibility.Modules](../extensibility/Inno.Extensibility.Modules.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Coroutines](../core/Inno.Core.Coroutines.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：公开引用边界由实际签名核对。
- [Inno.Core.Jobs](../core/Inno.Core.Jobs.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Assets](../assets/Inno.Assets.md)：公开引用边界由实际签名核对。
- [Inno.Content](../assets/Inno.Content.md)：公开引用边界由实际签名核对。
- [Inno.Core.Events](../core/Inno.Core.Events.md)：公开引用边界由实际签名核对。
- [Inno.Core.Identity](../core/Inno.Core.Identity.md)：公开引用边界由实际签名核对。
- [Inno.Core.Logging](../core/Inno.Core.Logging.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
- [Inno.Core.Settings](../core/Inno.Core.Settings.md)：公开引用边界由实际签名核对。
- [Inno.References](../references/Inno.References.md)：公开引用边界由实际签名核对。
- [Inno.Scene](../scene/Inno.Scene.md)：公开引用边界由实际签名核对。
- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Runtime.Contracts](Inno.Runtime.Contracts.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Reload](../extensibility/Inno.Extensibility.Reload.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
