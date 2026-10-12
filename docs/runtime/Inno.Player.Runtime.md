# Inno.Player.Runtime

[分类索引](README.md) · [Wiki 首页](../README.md)

## 职责与边界

桌面和浏览器共用的 Player 应用层。平台 composition 注入内容来源、元数据、Adapter、存储、日志与帧驱动；共享流程不读取部署文件，不创建缓存目录，不引用 Build 或具体内容 Adapter。内部 GamePlayerHost 不是公开扩展协议。

## 启动数据流

1. `IPlayerContentSource.ReadMetadataAsync` 返回 owned manifest/catalog bytes。
2. 共享 Player 验证 envelope、应用 ID、代码闭包和 Pack 描述；activator 核对静态链接代码身份。
3. `PrepareAsync` 返回 verified read-only content store。文件或 HTTP 来源通过同一内容协议验证 Pack。
4. Shell 创建 Adapter；Session 从 store 读取 Asset catalog 和只读 Project Settings；宿主 factory 创建存储和日志。
5. 启动 Scene 并运行同一帧驱动。退出先退休 Session，再 Settings、Content、Engine 和诊断 owner。

内容来源本身由宿主借用；它交出的 store 由 Player 释放。元数据 bytes 的所有权独立于外部下载缓冲。取消和启动失败执行清理，不制造默认 Scene，不隐藏异常。退出顺序先退休 Session，再释放 Settings、Content、Engine 与诊断，最后由 Shell 释放设备和平台应用。普通清理失败会汇总；出现 `RetirementPendingException` 时停止释放下游依赖，保留仍被使用的 owner，不能把未完成退休当作正常退出。

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

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Player.Runtime.IPlayerContentSource`

<!-- api: T:Inno.Player.Runtime.IPlayerContentSource af0b31fc472b9fa253a8809ea9fe144ce31baa14ff540c6cbadf35ef2a31dfd6 -->
<!-- api: M:Inno.Player.Runtime.IPlayerContentSource.ReadMetadataAsync(System.Threading.CancellationToken) 437463fd909e8843b2ab4ca99319b56ec3939715f689ac22035987f0b3f8b4af -->
<!-- api: M:Inno.Player.Runtime.IPlayerContentSource.PrepareAsync(Inno.Content.ContentPackDescriptor,Inno.Storage.StorageScope,Inno.Core.Serialization.SerializationGeneration,System.Threading.CancellationToken) 822747a7ae795c163685b1dbb6e97602493791be39d10760afc437d579b93108 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-6107eea71d3ffff4"></a>[`System.Threading.Tasks.ValueTask<Inno.Content.IRuntimeContentStore> Inno.Player.Runtime.IPlayerContentSource.PrepareAsync(Inno.Content.ContentPackDescriptor pack, Inno.Storage.StorageScope scope, Inno.Core.Serialization.SerializationGeneration serialization, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/composition/player/Inno.Player.Runtime/Deployment/IPlayerContentSource.cs#L47) | Prepares the exact verified pack selected by the decoded catalog.<br>返回：A caller-owned immutable store whose leases pin independent readers.<br>`pack`：The validated pack identity; other packs must not be substituted.<br>`scope`：The application namespace used to isolate any preparation cache.<br>`serialization`：The active serialization generation borrowed until this operation completes.<br>`cancellationToken`：Cancels acquisition and preparation before ownership transfers to the caller. | — | `System.IO.InvalidDataException`：Pack bytes, inventory, lengths, or payload hashes are invalid. |
| <a id="api-eb04f3bbb638fb5a"></a>[`System.Threading.Tasks.ValueTask<Inno.Player.Runtime.PlayerContentMetadata> Inno.Player.Runtime.IPlayerContentSource.ReadMetadataAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/composition/player/Inno.Player.Runtime/Deployment/IPlayerContentSource.cs#L24) | Reads the bounded deployment manifest and content catalog before serialization starts.<br>返回：Independently owned metadata bytes; missing or unreadable metadata fails the operation.<br>`cancellationToken`：Cancels metadata acquisition before content preparation starts. | — | — |
| <a id="api-57d300520a527e31"></a>[`public interface Inno.Player.Runtime.IPlayerContentSource`](../../src/composition/player/Inno.Player.Runtime/Deployment/IPlayerContentSource.cs#L13) | Supplies deployment metadata and verified content without prescribing a physical layout. | — | — |

### `Inno.Player.Runtime.IPlayerModuleActivator`

<!-- api: T:Inno.Player.Runtime.IPlayerModuleActivator f73caabb9d53418972de7bcca8e7a0b305a7646820cc4bb08e675124de744a84 -->
<!-- api: M:Inno.Player.Runtime.IPlayerModuleActivator.Activate(Inno.Extensibility.Modules.ModuleHost,Inno.Runtime.GameCodeDeployment) 5c91418fecb3ad4e4089a34328c46498453ebbaad478ea8997e92637b18631a4 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f2b66e1810d8ee4e"></a>[`void Inno.Player.Runtime.IPlayerModuleActivator.Activate(Inno.Extensibility.Modules.ModuleHost modules, Inno.Runtime.GameCodeDeployment deployment)`](../../src/composition/player/Inno.Player.Runtime/Deployment/IPlayerModuleActivator.cs#L23) | Verifies the code closure and publishes its module contributions atomically.<br>`modules`：The engine-owned module catalog and generation coordinator.<br>`deployment`：The frozen logical deployment, independent of DLL directories or managed runtime technology. | — | `System.IO.InvalidDataException`：The runtime manifest does not match the linked code closure. |
| <a id="api-f9711962e51dbf5a"></a>[`public interface Inno.Player.Runtime.IPlayerModuleActivator`](../../src/composition/player/Inno.Player.Runtime/Deployment/IPlayerModuleActivator.cs#L9) | Activates a validated logical code deployment through the common module transaction boundary. | — | — |

### `Inno.Player.Runtime.PlayerApplication`

<!-- api: T:Inno.Player.Runtime.PlayerApplication b187fd756127fcfb508dc9d17212347a34c9082129399e358c40eb0e187345a8 -->
<!-- api: M:Inno.Player.Runtime.PlayerApplication.RunAsync(Inno.Player.Runtime.PlayerLaunchOptions,System.Threading.CancellationToken) 3b42c74784cfe926a95bebcec8433766c86cf7854fcc8f0b3f3e2f6580b7998f -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-ec999cde975575ce"></a>[`public static System.Threading.Tasks.Task<int> Inno.Player.Runtime.PlayerApplication.RunAsync(Inno.Player.Runtime.PlayerLaunchOptions options, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/composition/player/Inno.Player.Runtime/PlayerApplication.cs#L40) | Runs a frozen game deployment using explicitly supplied host services.<br>返回：Zero after orderly shutdown; startup, execution and retirement failures propagate.<br>`options`：The prepared content and platform service composition.<br>`cancellationToken`：Requests termination of frame scheduling. | Hosts must preserve their owner thread across asynchronous startup and frame callbacks using their synchronization context. A blocking host can use OwnerThreadExecution.Run. | `System.ArgumentNullException`：Options or a required host service is null.<br>`System.OperationCanceledException`：Frame scheduling was canceled.<br>`System.IO.InvalidDataException`：Deployment metadata, linked code, or the prepared content identity is inconsistent.<br>`System.InvalidOperationException`：A host factory does not provide its required product, or runtime startup or retirement fails. |
| <a id="api-90cdaf3284e7a878"></a>[`public static class Inno.Player.Runtime.PlayerApplication`](../../src/composition/player/Inno.Player.Runtime/PlayerApplication.cs#L10) | Owns the common Player startup, frame execution and verified resource retirement. | — | — |

### `Inno.Player.Runtime.PlayerCommandLineOptions`

<!-- api: T:Inno.Player.Runtime.PlayerCommandLineOptions a051d6ae82bd724f9c57840ab3e5bd445bd82b6c325ef2f3ca5c17dcb606adc5 -->
<!-- api: P:Inno.Player.Runtime.PlayerCommandLineOptions.smokeFrameLimit 1f05c879dfc4e879e4439b10a1bdb7916121d0d23413babca0b2c9909994cd3e -->
<!-- api: P:Inno.Player.Runtime.PlayerCommandLineOptions.graphicsApi ab822a130bd8c59b489cc01d0bf08ec8bd01bbc3deda7365716512dae415f232 -->
<!-- api: P:Inno.Player.Runtime.PlayerCommandLineOptions.windowVisible 16df601b0bdc89b38bfbfbe20daebb5440955cb5ae982bbffbbe0b7b147f5b34 -->
<!-- api: M:Inno.Player.Runtime.PlayerCommandLineOptions.Parse(System.String[]) 32c5423989460bee8eb21027abe2e8731d2df43030b0f4957ac3239570ac725d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-61ce663530e5dfe3"></a>[`public static Inno.Player.Runtime.PlayerCommandLineOptions Inno.Player.Runtime.PlayerCommandLineOptions.Parse(string[] arguments)`](../../src/composition/player/Inno.Player.Runtime/PlayerCommandLineOptions.cs#L40) | Parses the shared Player command line before creating any native resource.<br>返回：A complete immutable command configuration.<br>`arguments`：Pairs of supported option names and values. | — | `System.ArgumentException`：An option is missing, repeated or invalid. |
| <a id="api-289638a8741c4ace"></a>[`public Inno.Rendering.GraphicsApi? Inno.Player.Runtime.PlayerCommandLineOptions.graphicsApi { get; private init; }`](../../src/composition/player/Inno.Player.Runtime/PlayerCommandLineOptions.cs#L21) | Gets an explicit renderer preference, or null to use the selected backend's policy. | — | — |
| <a id="api-4446705412562b73"></a>[`public int? Inno.Player.Runtime.PlayerCommandLineOptions.smokeFrameLimit { get; private init; }`](../../src/composition/player/Inno.Player.Runtime/PlayerCommandLineOptions.cs#L16) | Gets an optional positive bounded run length. | — | — |
| <a id="api-6b4c10cc3d953020"></a>[`public bool Inno.Player.Runtime.PlayerCommandLineOptions.windowVisible { get; private init; }`](../../src/composition/player/Inno.Player.Runtime/PlayerCommandLineOptions.cs#L26) | Gets whether the primary window should initially be shown. | — | — |
| <a id="api-521b75d9522f5ea3"></a>[`public sealed class Inno.Player.Runtime.PlayerCommandLineOptions`](../../src/composition/player/Inno.Player.Runtime/PlayerCommandLineOptions.cs#L9) | Parses the shared Player verification and window preferences independently of publication platform. | — | — |

### `Inno.Player.Runtime.PlayerContentMetadata`

<!-- api: T:Inno.Player.Runtime.PlayerContentMetadata 7e20d5f13befe21e0e9cf62a0fc7a9b84dd32f05cfebee2467b7f9574cc20af7 -->
<!-- api: M:Inno.Player.Runtime.PlayerContentMetadata.#ctor(System.ReadOnlySpan{System.Byte},System.ReadOnlySpan{System.Byte}) 150e038c32dd554a4ed33dec1db1100081ea55c3ed76f9d759aa969d2f6c9af1 -->
<!-- api: P:Inno.Player.Runtime.PlayerContentMetadata.manifest f5ccf11e27ae6709871c4f58e5901821fe8a64d330171d613a0d1c0ed1a4a4e5 -->
<!-- api: P:Inno.Player.Runtime.PlayerContentMetadata.catalog 1b425d4c8f36879f49d50442b020961101f9bdc7dea94760dd3acd1883df39bc -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b7738e400daf5301"></a>[`public Inno.Player.Runtime.PlayerContentMetadata.PlayerContentMetadata(System.ReadOnlySpan<byte> manifest, System.ReadOnlySpan<byte> catalog)`](../../src/composition/player/Inno.Player.Runtime/Deployment/PlayerContentMetadata.cs#L22) | Copies deployment metadata so the source can retire its own buffers immediately.<br>`manifest`：The complete strict runtime manifest envelope.<br>`catalog`：The serialized runtime content catalog. | — | `System.ArgumentException`：Either document is empty. |
| <a id="api-197da010be209fb3"></a>[`public System.ReadOnlyMemory<byte> Inno.Player.Runtime.PlayerContentMetadata.catalog { get; }`](../../src/composition/player/Inno.Player.Runtime/Deployment/PlayerContentMetadata.cs#L40) | Gets the owned serialized content catalog, valid for this metadata object's lifetime. | — | — |
| <a id="api-1d01229669acb6ab"></a>[`public System.ReadOnlyMemory<byte> Inno.Player.Runtime.PlayerContentMetadata.manifest { get; }`](../../src/composition/player/Inno.Player.Runtime/Deployment/PlayerContentMetadata.cs#L35) | Gets the owned manifest envelope, valid for this metadata object's lifetime. | — | — |
| <a id="api-078be1bab6aa4ebb"></a>[`public sealed class Inno.Player.Runtime.PlayerContentMetadata`](../../src/composition/player/Inno.Player.Runtime/Deployment/PlayerContentMetadata.cs#L8) | Owns the two metadata documents required to identify and verify a Player deployment. | — | — |

### `Inno.Player.Runtime.PlayerLaunchOptions`

<!-- api: T:Inno.Player.Runtime.PlayerLaunchOptions bf3423b1f5efa84fe8cedd10f04dd4c10b302dca506767d5c2cf6fc819faa926 -->
<!-- api: P:Inno.Player.Runtime.PlayerLaunchOptions.modules 0aed2ff38ffe727d29a43142fbb9fb1d811370128a04b6fd171fb51368039d02 -->
<!-- api: P:Inno.Player.Runtime.PlayerLaunchOptions.types e1174132b946769e3cefe2f676c17bdabe9d3b3b429f4fe29ecac21ee76fbcf8 -->
<!-- api: P:Inno.Player.Runtime.PlayerLaunchOptions.serializationMetadata 6801689cc47e29c8d05d581a43c81d79e98e362f77eb6f49ff75c83fd70dabe6 -->
<!-- api: P:Inno.Player.Runtime.PlayerLaunchOptions.adapters 16f60f2d19b31b3373571177ce576b985a997b74166fc4adb4c69e9a3d13e8cc -->
<!-- api: P:Inno.Player.Runtime.PlayerLaunchOptions.contentSource 7b82375bf5b187e78bb9ce2be089a8cd1a868859ee39389320d4b3b2ad12072d -->
<!-- api: P:Inno.Player.Runtime.PlayerLaunchOptions.createStorage 851f699840c8ba59fe051e858e64ed94718512702e4041bd4938661fbbdc06b9 -->
<!-- api: P:Inno.Player.Runtime.PlayerLaunchOptions.createLogSink 1c3209533945ab3df2a3eb09dde5e946f041f98e702a23e28ccf73b66ad1cefb -->
<!-- api: P:Inno.Player.Runtime.PlayerLaunchOptions.moduleActivator 7d5ea6c87b04e8aeb1bef73bbe9c5de6b77a96ee6562cf7894b097b3fac76f95 -->
<!-- api: P:Inno.Player.Runtime.PlayerLaunchOptions.frameDriver 112bfa4956f6840f0e0670efdcdd1b2c7c29979cd5fd574b45c2d8ca0c37cdbe -->
<!-- api: P:Inno.Player.Runtime.PlayerLaunchOptions.adapterSelection c2dca5801c93398894e9c7940406586a87f556d3805efbcb6e95263b9f359fc0 -->
<!-- api: P:Inno.Player.Runtime.PlayerLaunchOptions.jobExecutionMode aa82c86878d54175c142ee3f757dbe4323f61c095e4d2a050f5b5d80797b5e65 -->
<!-- api: P:Inno.Player.Runtime.PlayerLaunchOptions.renderOnCallingThread 132f04da574d222bae4ac98b3a5503979bc9616bee5607cc577c4e8a839495af -->
<!-- api: P:Inno.Player.Runtime.PlayerLaunchOptions.logDeliveryMode 63db3a640540ea92ce2518eb0ef99bb178018d91a22fc7e5118803537630fa08 -->
<!-- api: P:Inno.Player.Runtime.PlayerLaunchOptions.consoleColors 0798ea1cd977139f58048ede26a83e49c18cf1045ce871d5a5391680f3a02c94 -->
<!-- api: P:Inno.Player.Runtime.PlayerLaunchOptions.graphicsApi d0080254898c6a110ca070b19dc1c105003c66384d98b480fa2691c95c390ee1 -->
<!-- api: P:Inno.Player.Runtime.PlayerLaunchOptions.windowVisible bb685645b31c654dc4c039c805f0f72685e0d3fcff55cf16c0a33a92dba3fc48 -->
<!-- api: P:Inno.Player.Runtime.PlayerLaunchOptions.smokeFrameLimit 0db9b29889ad449695ecf39dc6f8de5691b05948ffb77cb4d8af7c2c47ced7b9 -->
<!-- api: M:Inno.Player.Runtime.PlayerLaunchOptions.#ctor 0a6b58cc22ca9b0b31bb4c40e10fda4929bc3331bc6a777be34566707a674b8c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c7b6f84e5c3d0047"></a>[`public Inno.Player.Runtime.PlayerLaunchOptions.PlayerLaunchOptions()`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L17) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-9e3678020633557a"></a>[`public required Inno.Adapter.IAdapterCatalog Inno.Player.Runtime.PlayerLaunchOptions.adapters { get; init; }`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L37) | Gets the host-owned factories used to create isolated runtime adapters. | — | — |
| <a id="api-52c08a34b854425b"></a>[`public required Inno.Adapter.AdapterSelection Inno.Player.Runtime.PlayerLaunchOptions.adapterSelection { get; init; }`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L67) | Gets the coherent runtime backend selection. | — | — |
| <a id="api-001b132dfd9a55dc"></a>[`public bool Inno.Player.Runtime.PlayerLaunchOptions.consoleColors { get; init; }`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L87) | Gets whether the host console supports changing terminal colors. | — | — |
| <a id="api-8e53cf9124055248"></a>[`public required Inno.Player.Runtime.IPlayerContentSource Inno.Player.Runtime.PlayerLaunchOptions.contentSource { get; init; }`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L42) | Gets the borrowed source of deployment metadata and verified immutable content. | — | — |
| <a id="api-6880f855d2f7c409"></a>[`public System.Func<Inno.Runtime.GameRuntimeManifest, Inno.Core.Logging.LogSessionId, Inno.Core.Logging.ILogSink>? Inno.Player.Runtime.PlayerLaunchOptions.createLogSink { get; init; }`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L52) | Gets the optional host factory transferring a session log sink to the game session. | — | — |
| <a id="api-8bf9fcb90f8b6695"></a>[`public required System.Func<Inno.Runtime.GameRuntimeManifest, Inno.Storage.IApplicationStorage> Inno.Player.Runtime.PlayerLaunchOptions.createStorage { get; init; }`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L47) | Gets the host factory transferring application storage ownership to the game session. | — | — |
| <a id="api-8f672d536774e941"></a>[`public required Inno.Shell.IShellFrameDriver Inno.Player.Runtime.PlayerLaunchOptions.frameDriver { get; init; }`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L62) | Gets the owner-thread driver that schedules the common shell frames. | — | — |
| <a id="api-537cb85a8306ad82"></a>[`public Inno.Rendering.GraphicsApi? Inno.Player.Runtime.PlayerLaunchOptions.graphicsApi { get; init; }`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L92) | Gets an optional rendering API preference; null selects the adapter default. | — | — |
| <a id="api-2a620b7309ef99de"></a>[`public Inno.Runtime.RuntimeJobExecutionMode Inno.Player.Runtime.PlayerLaunchOptions.jobExecutionMode { get; init; }`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L72) | Gets the execution policy available to the game's job scheduler. | — | — |
| <a id="api-dc6b4f35026cd8fb"></a>[`public Inno.Core.Logging.LogDeliveryMode Inno.Player.Runtime.PlayerLaunchOptions.logDeliveryMode { get; init; }`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L82) | Gets the host's logging delivery policy. | — | — |
| <a id="api-901abe679b9b186b"></a>[`public required Inno.Player.Runtime.IPlayerModuleActivator Inno.Player.Runtime.PlayerLaunchOptions.moduleActivator { get; init; }`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L57) | Gets the strategy that activates the frozen deployment's managed modules. | — | — |
| <a id="api-d77cd71dcb08f421"></a>[`public required Inno.Extensibility.Modules.IAssemblyCatalogSource Inno.Player.Runtime.PlayerLaunchOptions.modules { get; init; }`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L22) | Gets the code catalog selected by the platform composition; ownership transfers to the application. | — | — |
| <a id="api-e74fae437c15d02e"></a>[`public bool Inno.Player.Runtime.PlayerLaunchOptions.renderOnCallingThread { get; init; }`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L77) | Gets whether graphics commands must execute on the frame owner's thread. | — | — |
| <a id="api-0b3c97c953cab6c7"></a>[`public required Inno.Core.Serialization.ISerializationMetadataSource Inno.Player.Runtime.PlayerLaunchOptions.serializationMetadata { get; init; }`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L32) | Gets generated declaration access and collection construction for the linked code closure. | — | — |
| <a id="api-46689e594c98031b"></a>[`public int? Inno.Player.Runtime.PlayerLaunchOptions.smokeFrameLimit { get; init; }`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L102) | Gets an optional positive frame count for a bounded verification run. | — | — |
| <a id="api-2def09a9fd531e11"></a>[`public required Inno.Extensibility.Types.ITypeCatalogSource Inno.Player.Runtime.PlayerLaunchOptions.types { get; init; }`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L27) | Gets the metadata implementation corresponding to the selected code deployment. | — | — |
| <a id="api-cd376da0b972ba64"></a>[`public bool Inno.Player.Runtime.PlayerLaunchOptions.windowVisible { get; init; }`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L97) | Gets whether the primary window is initially shown; false preserves rendering without taking focus. | — | — |
| <a id="api-f7777778712605ce"></a>[`public sealed class Inno.Player.Runtime.PlayerLaunchOptions`](../../src/composition/player/Inno.Player.Runtime/PlayerLaunchOptions.cs#L17) | Supplies resolved host services and content to the common Player lifecycle. | — | — |

### `Inno.Player.Runtime.StaticPlayerModuleActivator`

<!-- api: T:Inno.Player.Runtime.StaticPlayerModuleActivator a551eea9c2525ca02583e7c4f540ff8bf65902da020a428e6c9249cc25f650d2 -->
<!-- api: M:Inno.Player.Runtime.StaticPlayerModuleActivator.#ctor(Inno.Runtime.GameCodeDeployment,System.Collections.Generic.IReadOnlyDictionary{System.String,System.Collections.Generic.IReadOnlyList{System.Reflection.Assembly}}) ce9693295b70681c3b1f3b32e8dcef797a0c13c71c6a10d4af00eee54fca1c9b -->
<!-- api: M:Inno.Player.Runtime.StaticPlayerModuleActivator.Activate(Inno.Extensibility.Modules.ModuleHost,Inno.Runtime.GameCodeDeployment) 6681578e2e8731b78a7fbb9eacd39e18b34503bc8f1dd264072ed587bbb1bce5 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b53e56d1ee8471e4"></a>[`public Inno.Player.Runtime.StaticPlayerModuleActivator.StaticPlayerModuleActivator(Inno.Runtime.GameCodeDeployment deployment, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IReadOnlyList<System.Reflection.Assembly>> assemblies)`](../../src/composition/player/Inno.Player.Runtime/Deployment/StaticPlayerModuleActivator.cs#L31) | Validates and freezes linked assembly ownership before the Player opens its runtime session.<br>`deployment`：The build-generated code identity closure.<br>`assemblies`：Explicit linked assembly instances grouped by logical module; values are copied. | — | `System.IO.InvalidDataException`：A linked assembly is absent, duplicated, or owned by an undeclared module. |
| <a id="api-d8e316ef3c987383"></a>[`public void Inno.Player.Runtime.StaticPlayerModuleActivator.Activate(Inno.Extensibility.Modules.ModuleHost modules, Inno.Runtime.GameCodeDeployment deployment)`](../../src/composition/player/Inno.Player.Runtime/Deployment/StaticPlayerModuleActivator.cs#L55) | Verifies the code closure and publishes its module contributions atomically.<br>`modules`：The engine-owned module catalog and generation coordinator.<br>`deployment`：The frozen logical deployment, independent of DLL directories or managed runtime technology. | — | `System.IO.InvalidDataException`：The runtime manifest does not match the linked code closure. |
| <a id="api-631d50e30cc8cb9e"></a>[`public sealed class Inno.Player.Runtime.StaticPlayerModuleActivator : Inno.Player.Runtime.IPlayerModuleActivator`](../../src/composition/player/Inno.Player.Runtime/Deployment/StaticPlayerModuleActivator.cs#L14) | Activates explicit linked code without probing assemblies or opening runtime code files. | — | — |

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
