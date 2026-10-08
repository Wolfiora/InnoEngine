# Inno.Player.Runtime

[分类索引](README.md) · [Wiki 首页](../README.md) · [本轮整改计划](../architecture/ARCHITECTURE_CLEANUP_PLAN_2026_10_06.md)

## 职责与边界

桌面和浏览器共用的 Player 应用层。平台 composition 注入内容来源、元数据、Adapter、存储、日志与帧驱动；共享流程不读取部署文件，不创建缓存目录，不引用 Build 或具体内容 Adapter。内部 GamePlayerHost 不是公开扩展协议。

## 启动数据流

1. `IPlayerContentSource.ReadMetadataAsync` 返回 owned manifest/catalog bytes。
2. 共享 Player 验证 envelope、应用 ID、代码闭包和 Pack 描述；activator 核对静态链接代码身份。
3. `PrepareAsync` 返回 verified read-only content store。Desktop/HTTP 来源使用同一 Pack reader。
4. Shell 创建 Adapter；Session 从 store 读取 Asset catalog 和只读 Project Settings；宿主 factory 创建存储和日志。
5. 启动 Scene 并运行同一帧驱动。退出先退休 Session，再 Settings、Content、Engine 和诊断 owner。

内容来源本身由宿主借用；它交出的 store 由 Player 释放。元数据 bytes 的所有权独立于外部下载缓冲。取消和启动失败执行完整补偿，不制造默认 Scene，不隐藏异常。

## 组合示例

```csharp
using System.Threading;
using System.Threading.Tasks;
using Inno.Player.Runtime;

static Task<int> StartPlayer(
    PlayerLaunchOptions options,
    CancellationToken cancellationToken
) {
    return PlayerApplication.RunAsync(options, cancellationToken);
}
```

options 的必要内容与生命周期见下方完整 API。桌面阻塞入口使用 `OwnerThreadExecution.Run`；异步宿主须保留 owner-thread synchronization context。`windowVisible=false` 用于不夺焦点的有界渲染验收，真实渲染生命周期仍执行。

## 扩展和热重载

新增内容来源只实现 `IPlayerContentSource` 和内容 store；不修改 Session 或 AssetDatabase。新增宿主只选择帧驱动和平台 Adapter。静态 Player 不携带 collectible loader 或运行时 compiler；Editor 的 Full GC/finalizer/弱监测/Faulted gate 不因静态发布改变。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Player.Runtime.IPlayerContentSource`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Player.Runtime.IPlayerContentSource`](../../src/composition/player/Inno.Player.Runtime/Deployment/IPlayerContentSource.cs#L13) | Supplies deployment metadata and verified content without prescribing a physical layout. |
| [`System.Threading.Tasks.ValueTask<Inno.Content.IRuntimeContentStore> Inno.Player.Runtime.IPlayerContentSource.PrepareAsync(Inno.Content.ContentPackDescriptor pack, Inno.Storage.StorageScope scope, Inno.Core.Serialization.SerializationGeneration serialization, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/composition/player/Inno.Player.Runtime/Deployment/IPlayerContentSource.cs#L47) | Prepares the exact verified pack selected by the decoded catalog. |
| [`System.Threading.Tasks.ValueTask<Inno.Player.Runtime.PlayerContentMetadata> Inno.Player.Runtime.IPlayerContentSource.ReadMetadataAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/composition/player/Inno.Player.Runtime/Deployment/IPlayerContentSource.cs#L24) | Reads the bounded deployment manifest and content catalog before serialization starts. |

### `Inno.Player.Runtime.IPlayerModuleActivator`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Player.Runtime.IPlayerModuleActivator`](../../src/composition/player/Inno.Player.Runtime/Deployment/IPlayerModuleActivator.cs#L9) | Activates a validated logical code deployment through the common module transaction boundary. |
| [`void Inno.Player.Runtime.IPlayerModuleActivator.Activate(Inno.Extensibility.Modules.ModuleHost modules, Inno.Runtime.GameCodeDeployment deployment)`](../../src/composition/player/Inno.Player.Runtime/Deployment/IPlayerModuleActivator.cs#L23) | Verifies the code closure and publishes its module contributions atomically. |

### `Inno.Player.Runtime.PlayerApplication`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Player.Runtime.PlayerApplication`](../../src/composition/player/Inno.Player.Runtime/PlayerApplication.cs#L10) | Owns the common Player startup, frame execution and verified resource retirement. |
| [`static System.Threading.Tasks.Task<int> Inno.Player.Runtime.PlayerApplication.RunAsync(Inno.Player.Runtime.PlayerLaunchOptions options, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/composition/player/Inno.Player.Runtime/PlayerApplication.cs#L40) | Runs a frozen game deployment using explicitly supplied host services. |

### `Inno.Player.Runtime.PlayerCommandLineOptions`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Player.Runtime.PlayerCommandLineOptions`](../../src/composition/player/Inno.Player.Runtime/PlayerCommandLineOptions.cs#L9) | Parses the shared Player verification and window preferences independently of publication platform. |
| [`Inno.Rendering.GraphicsApi? Inno.Player.Runtime.PlayerCommandLineOptions.graphicsApi`](../../src/composition/player/Inno.Player.Runtime/PlayerCommandLineOptions.cs#L21) | Gets an explicit renderer preference, or null to use the selected backend's policy. |
| [`bool Inno.Player.Runtime.PlayerCommandLineOptions.windowVisible`](../../src/composition/player/Inno.Player.Runtime/PlayerCommandLineOptions.cs#L26) | Gets whether the primary window should initially be shown. |
| [`int? Inno.Player.Runtime.PlayerCommandLineOptions.smokeFrameLimit`](../../src/composition/player/Inno.Player.Runtime/PlayerCommandLineOptions.cs#L16) | Gets an optional positive bounded run length. |
| [`static Inno.Player.Runtime.PlayerCommandLineOptions Inno.Player.Runtime.PlayerCommandLineOptions.Parse(string[] arguments)`](../../src/composition/player/Inno.Player.Runtime/PlayerCommandLineOptions.cs#L40) | Parses the shared Player command line before creating any native resource. |

### `Inno.Player.Runtime.PlayerContentMetadata`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Player.Runtime.PlayerContentMetadata`](../../src/composition/player/Inno.Player.Runtime/Deployment/PlayerContentMetadata.cs#L8) | Owns the two metadata documents required to identify and verify a Player deployment. |
| [`Inno.Player.Runtime.PlayerContentMetadata.PlayerContentMetadata(System.ReadOnlySpan<byte> manifest, System.ReadOnlySpan<byte> catalog)`](../../src/composition/player/Inno.Player.Runtime/Deployment/PlayerContentMetadata.cs#L22) | Copies deployment metadata so the source can retire its own buffers immediately. |
| [`System.ReadOnlyMemory<byte> Inno.Player.Runtime.PlayerContentMetadata.catalog`](../../src/composition/player/Inno.Player.Runtime/Deployment/PlayerContentMetadata.cs#L40) | Gets the owned serialized content catalog, valid for this metadata object's lifetime. |
| [`System.ReadOnlyMemory<byte> Inno.Player.Runtime.PlayerContentMetadata.manifest`](../../src/composition/player/Inno.Player.Runtime/Deployment/PlayerContentMetadata.cs#L35) | Gets the owned manifest envelope, valid for this metadata object's lifetime. |

### `Inno.Player.Runtime.PlayerLaunchOptions`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Logging.LogDeliveryMode Inno.Player.Runtime.PlayerLaunchOptions.logDeliveryMode`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L82) | Gets the host's logging delivery policy. |
| [`Inno.Player.Runtime.PlayerLaunchOptions`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L17) | Supplies resolved host services and content to the common Player lifecycle. |
| [`Inno.Rendering.GraphicsApi? Inno.Player.Runtime.PlayerLaunchOptions.graphicsApi`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L92) | Gets an optional rendering API preference; null selects the adapter default. |
| [`Inno.Runtime.RuntimeJobExecutionMode Inno.Player.Runtime.PlayerLaunchOptions.jobExecutionMode`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L72) | Gets the execution policy available to the game's job scheduler. |
| [`System.Func<Inno.Runtime.GameRuntimeManifest, Inno.Core.Logging.LogSessionId, Inno.Core.Logging.ILogSink>? Inno.Player.Runtime.PlayerLaunchOptions.createLogSink`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L52) | Gets the optional host factory transferring a session log sink to the game session. |
| [`bool Inno.Player.Runtime.PlayerLaunchOptions.consoleColors`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L87) | Gets whether the host console supports changing terminal colors. |
| [`bool Inno.Player.Runtime.PlayerLaunchOptions.renderOnCallingThread`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L77) | Gets whether graphics commands must execute on the frame owner's thread. |
| [`bool Inno.Player.Runtime.PlayerLaunchOptions.windowVisible`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L97) | Gets whether the primary window is initially shown; false preserves rendering without taking focus. |
| [`int? Inno.Player.Runtime.PlayerLaunchOptions.smokeFrameLimit`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L102) | Gets an optional positive frame count for a bounded verification run. |
| [`required Inno.Adapter.AdapterSelection Inno.Player.Runtime.PlayerLaunchOptions.adapterSelection`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L67) | Gets the coherent runtime backend selection. |
| [`required Inno.Adapter.IAdapterCatalog Inno.Player.Runtime.PlayerLaunchOptions.adapters`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L37) | Gets the host-owned factories used to create isolated runtime adapters. |
| [`required Inno.Core.Serialization.ISerializationMetadataSource Inno.Player.Runtime.PlayerLaunchOptions.serializationMetadata`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L32) | Gets generated declaration access and collection construction for the linked code closure. |
| [`required Inno.Extensibility.Modules.IAssemblyCatalogSource Inno.Player.Runtime.PlayerLaunchOptions.modules`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L22) | Gets the code catalog selected by the platform composition; ownership transfers to the application. |
| [`required Inno.Extensibility.Types.ITypeCatalogSource Inno.Player.Runtime.PlayerLaunchOptions.types`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L27) | Gets the metadata implementation corresponding to the selected code deployment. |
| [`required Inno.Player.Runtime.IPlayerContentSource Inno.Player.Runtime.PlayerLaunchOptions.contentSource`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L42) | Gets the borrowed source of deployment metadata and verified immutable content. |
| [`required Inno.Player.Runtime.IPlayerModuleActivator Inno.Player.Runtime.PlayerLaunchOptions.moduleActivator`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L57) | Gets the strategy that activates the frozen deployment's managed modules. |
| [`required Inno.Shell.IShellFrameDriver Inno.Player.Runtime.PlayerLaunchOptions.frameDriver`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L62) | Gets the owner-thread driver that schedules the common shell frames. |
| [`required System.Func<Inno.Runtime.GameRuntimeManifest, Inno.Storage.IApplicationStorage> Inno.Player.Runtime.PlayerLaunchOptions.createStorage`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L47) | Gets the host factory transferring application storage ownership to the game session. |

### `Inno.Player.Runtime.StaticPlayerModuleActivator`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Player.Runtime.StaticPlayerModuleActivator`](../../src/composition/player/Inno.Player.Runtime/Deployment/StaticPlayerModuleActivator.cs#L14) | Activates explicit linked code without probing assemblies or opening runtime code files. |
| [`Inno.Player.Runtime.StaticPlayerModuleActivator.StaticPlayerModuleActivator(Inno.Runtime.GameCodeDeployment deployment, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IReadOnlyList<System.Reflection.Assembly>> assemblies)`](../../src/composition/player/Inno.Player.Runtime/Deployment/StaticPlayerModuleActivator.cs#L31) | Validates and freezes linked assembly ownership before the Player opens its runtime session. |
| [`void Inno.Player.Runtime.StaticPlayerModuleActivator.Activate(Inno.Extensibility.Modules.ModuleHost modules, Inno.Runtime.GameCodeDeployment deployment)`](../../src/composition/player/Inno.Player.Runtime/Deployment/StaticPlayerModuleActivator.cs#L55) | See the implemented contract. |

## 项目依赖

- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Engine.Default](Inno.Engine.Default.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Animation.Runtime](../animation/Inno.Animation.Runtime.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Animation](../animation/Inno.Animation.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Audio.Runtime](../audio/Inno.Audio.Runtime.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Audio](../audio/Inno.Audio.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Assets](../assets/Inno.Assets.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Events](../core/Inno.Core.Events.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.IO](../core/Inno.Core.IO.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Settings](../core/Inno.Core.Settings.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Input.Runtime](../input/Inno.Input.Runtime.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Platform](../platform/Inno.Platform.md)：实现依赖，PrivateAssets="compile"。
- [Inno.References](../references/Inno.References.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Rendering.Runtime](../rendering/Inno.Rendering.Runtime.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Text](../text/Inno.Text.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Text.Runtime](../text/Inno.Text.Runtime.md)：实现依赖，PrivateAssets="compile"。
- [Inno.UI](../ui/Inno.UI.md)：实现依赖，PrivateAssets="compile"。
- [Inno.UI.Runtime](../ui/Inno.UI.Runtime.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scene](../scene/Inno.Scene.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Storage.Runtime](../storage/Inno.Storage.Runtime.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter](Inno.Adapter.md)：公开引用边界由实际签名核对。
- [Inno.Shell](Inno.Shell.md)：公开引用边界由实际签名核对。
- [Inno.Core.Logging](../core/Inno.Core.Logging.md)：公开引用边界由实际签名核对。
- [Inno.Rendering](../rendering/Inno.Rendering.md)：公开引用边界由实际签名核对。
- [Inno.Runtime](Inno.Runtime.md)：公开引用边界由实际签名核对。
- [Inno.Content](../assets/Inno.Content.md)：公开引用边界由实际签名核对。
- [Inno.Storage](../storage/Inno.Storage.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Modules](../extensibility/Inno.Extensibility.Modules.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
