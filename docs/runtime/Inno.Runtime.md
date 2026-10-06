# Inno.Runtime

[分类索引](README.md) · [Wiki 首页](../README.md) · [本轮整改计划](../architecture/ARCHITECTURE_CLEANUP_PLAN_2026_10_06.md)

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

Asset、Settings 和 Rendering 的内容读取分别由其公开契约完成；共享 Session 不读取部署目录，也不创建日志文件。完整 Player 工作流见 [Player Runtime](Inno.Player.Runtime.md)，代际规则见 [Identity 与 Reload](../architecture/IDENTITY_REFERENCE_RELOAD_STANDARD.md)。

## 当前源码公开 API 清单

以下仅列出当前程序集自己声明的 public/protected 契约；继承成员遵循所属基类页面。internal/private 实现不作为稳定公开 API。签名依据当前源码语义模型生成，行为、参数、异常与所有权说明同时以对应英文 XML 为准。

### `Inno.Runtime.EngineHost`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Runtime.RuntimeSubsystemPipeline Inno.Runtime.EngineHost.CreateHostPipeline(System.Collections.Generic.IReadOnlyList<Inno.Runtime.Contracts.IRuntimeSubsystemFactory> factories, System.Collections.Generic.IReadOnlyList<Inno.Runtime.Contracts.RuntimeCapabilityId>? capabilities = null)`](../../src/runtime/engine/Inno.Runtime/Hosting/EngineHost.cs#L143) | Creates host-owned subsystems that outlive individual Edit, Play or Player sessions. |
| [`Inno.Runtime.RuntimeSession Inno.Runtime.EngineHost.CreateSession(Inno.Runtime.RuntimeSessionOptions options)`](../../src/runtime/engine/Inno.Runtime/Hosting/EngineHost.cs#L104) | Creates an isolated Edit, Play, or Player runtime session. |
| [`void Inno.Runtime.EngineHost.Dispose()`](../../src/runtime/engine/Inno.Runtime/Hosting/EngineHost.cs#L178) | Disposes every owned session before releasing application metadata services. |
| [`Inno.Core.Diagnostics.DiagnosticHub Inno.Runtime.EngineHost.diagnostics`](../../src/runtime/engine/Inno.Runtime/Hosting/EngineHost.cs#L67) | Gets the isolated diagnostic state hub owned by this host. |
| [`Inno.Extensibility.Reload.GenerationCoordinator Inno.Runtime.EngineHost.generations`](../../src/runtime/engine/Inno.Runtime/Hosting/EngineHost.cs#L72) | Gets the shared admission gate for reload, recovery, Play, Build and Export. |
| [`Inno.Core.Logging.LogRouter Inno.Runtime.EngineHost.logs`](../../src/runtime/engine/Inno.Runtime/Hosting/EngineHost.cs#L62) | Gets the isolated logging router using this host's configured delivery policy. |
| [`Inno.Extensibility.Modules.ModuleHost Inno.Runtime.EngineHost.modules`](../../src/runtime/engine/Inno.Runtime/Hosting/EngineHost.cs#L77) | Gets the isolated managed module host that owns this engine host's reload generations. |
| [`Inno.Core.Serialization.SerializationRegistry Inno.Runtime.EngineHost.serialization`](../../src/runtime/engine/Inno.Runtime/Hosting/EngineHost.cs#L87) | Gets the isolated serialization registry derived from this host's active type generation. |
| [`Inno.Extensibility.Types.TypeCatalog Inno.Runtime.EngineHost.types`](../../src/runtime/engine/Inno.Runtime/Hosting/EngineHost.cs#L82) | Gets the isolated immutable type catalog derived from this host's active modules. |
| [`Inno.Runtime.EngineHost`](../../src/runtime/engine/Inno.Runtime/Hosting/EngineHost.cs#L20) | Owns application-level engine services and creates isolated runtime sessions. |

### `Inno.Runtime.EngineHostBuilder`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Runtime.EngineHost Inno.Runtime.EngineHostBuilder.Build()`](../../src/runtime/engine/Inno.Runtime/Hosting/EngineHostBuilder.cs#L46) | Creates an application host and acquires its immutable metadata services. |
| [`Inno.Runtime.EngineHostBuilder Inno.Runtime.EngineHostBuilder.UseLogDelivery(Inno.Core.Logging.LogDeliveryMode deliveryMode)`](../../src/runtime/engine/Inno.Runtime/Hosting/EngineHostBuilder.cs#L101) | Selects the host router's delivery policy for host and session sinks. |
| [`Inno.Runtime.EngineHostBuilder Inno.Runtime.EngineHostBuilder.UseMetadataSources(Inno.Extensibility.Modules.IAssemblyCatalogSource modules, Inno.Extensibility.Types.ITypeCatalogSource types, Inno.Core.Serialization.ISerializationMetadataSource serialization)`](../../src/runtime/engine/Inno.Runtime/Hosting/EngineHostBuilder.cs#L73) | Selects the code and metadata implementation before creating the application host. |
| [`Inno.Runtime.EngineHostBuilder Inno.Runtime.EngineHostBuilder.UseRetirementTimeout(System.TimeSpan timeout)`](../../src/runtime/engine/Inno.Runtime/Hosting/EngineHostBuilder.cs#L32) | Sets the maximum owner-thread drain duration before retirement faults the host. |
| [`Inno.Runtime.EngineHostBuilder`](../../src/runtime/engine/Inno.Runtime/Hosting/EngineHostBuilder.cs#L12) | Collects application-level runtime services before creating an . |

### `Inno.Runtime.GameCodeAssembly`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Runtime.GameCodeAssembly.GameCodeAssembly(string name, string contentFingerprint)`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeAssembly.cs#L22) | Validates and freezes one logical assembly identity. |
| [`string Inno.Runtime.GameCodeAssembly.contentFingerprint`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeAssembly.cs#L39) | Gets the build input's content fingerprint. |
| [`string Inno.Runtime.GameCodeAssembly.name`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeAssembly.cs#L34) | Gets the exact simple assembly name. |
| [`Inno.Runtime.GameCodeAssembly`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeAssembly.cs#L8) | Identifies one immutable code input independently of its physical deployment representation. |

### `Inno.Runtime.GameCodeDeployment`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Runtime.GameCodeDeployment.GameCodeDeployment(System.Collections.Generic.IReadOnlyList<Inno.Runtime.GameCodeModule> modules)`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeDeployment.cs#L22) | Copies an explicit code closure and verifies unique ownership and dependency ordering. |
| [`static Inno.Runtime.GameCodeDeployment Inno.Runtime.GameCodeDeployment.FromManifest(System.Collections.Generic.IReadOnlyList<Inno.Runtime.GameRuntimeModule> modules)`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeDeployment.cs#L58) | Freezes validated manifest declarations without retaining their mutable arrays or entries. |
| [`void Inno.Runtime.GameCodeDeployment.ValidateMatches(Inno.Runtime.GameCodeDeployment expected)`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeDeployment.cs#L81) | Rejects a manifest that differs from the code closure linked by the build. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Runtime.GameCodeModule> Inno.Runtime.GameCodeDeployment.modules`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeDeployment.cs#L44) | Gets the immutable dependency-ordered code closure. |
| [`Inno.Runtime.GameCodeDeployment`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeDeployment.cs#L11) | Owns a validated, immutable and dependency-ordered logical game code closure. |

### `Inno.Runtime.GameCodeModule`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Runtime.GameCodeModule.GameCodeModule(string name, Inno.Extensibility.Modules.AssemblyDomain domain, System.Collections.Generic.IReadOnlyList<Inno.Runtime.GameCodeAssembly> assemblies, System.Collections.Generic.IReadOnlyList<string> dependencies)`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeModule.cs#L31) | Copies and validates a module contribution without retaining mutable manifest objects. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Runtime.GameCodeAssembly> Inno.Runtime.GameCodeModule.assemblies`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeModule.cs#L72) | Gets the exact ordered code identities. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Runtime.GameCodeModule.dependencies`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeModule.cs#L77) | Gets the logical modules required before activation. |
| [`Inno.Extensibility.Modules.AssemblyDomain Inno.Runtime.GameCodeModule.domain`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeModule.cs#L67) | Gets the ownership domain of this contribution. |
| [`string Inno.Runtime.GameCodeModule.name`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeModule.cs#L62) | Gets the stable logical module identity. |
| [`Inno.Runtime.GameCodeModule`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeModule.cs#L11) | Freezes the code ownership, ordered assembly identities and dependencies of one logical module. |

### `Inno.Runtime.GamePresentationSettings`

| 当前声明 | 行为 |
| --- | --- |
| [`const string Inno.Runtime.GamePresentationSettings.settingProtocolId`](../../src/runtime/engine/Inno.Runtime/Presentation/GamePresentationSettings.cs#L20) | Gets the immutable project-setting protocol value used by discovery metadata. |
| [`Inno.Runtime.GamePresentationViewport Inno.Runtime.GamePresentationSettings.CalculateViewport(int availableWidth, int availableHeight)`](../../src/runtime/engine/Inno.Runtime/Presentation/GamePresentationSettings.cs#L60) | Calculates the centered content region for an available presentation surface. |
| [`bool Inno.Runtime.GamePresentationSettings.preserveAspectRatio`](../../src/runtime/engine/Inno.Runtime/Presentation/GamePresentationSettings.cs#L30) | Gets or sets whether the complete reference frame is fitted inside the available surface. |
| [`int Inno.Runtime.GamePresentationSettings.referenceHeight`](../../src/runtime/engine/Inno.Runtime/Presentation/GamePresentationSettings.cs#L42) | Gets or sets the positive reference-frame height used to derive the presentation aspect ratio. |
| [`int Inno.Runtime.GamePresentationSettings.referenceWidth`](../../src/runtime/engine/Inno.Runtime/Presentation/GamePresentationSettings.cs#L36) | Gets or sets the positive reference-frame width used to derive the presentation aspect ratio. |
| [`static Inno.Core.Settings.ProjectSettingId Inno.Runtime.GamePresentationSettings.settingId`](../../src/runtime/engine/Inno.Runtime/Presentation/GamePresentationSettings.cs#L25) | Gets the stable project-setting identity for game presentation. |
| [`Inno.Runtime.GamePresentationSettings`](../../src/runtime/engine/Inno.Runtime/Presentation/GamePresentationSettings.cs#L12) | Defines the project-wide game presentation area shared by Editor previews and deployed Players. |

### `Inno.Runtime.GamePresentationViewport`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Runtime.GamePresentationViewport.GamePresentationViewport(int x, int y, int width, int height)`](../../src/runtime/engine/Inno.Runtime/Presentation/GamePresentationSettings.cs#L108) | Creates a validated game presentation viewport. |
| [`int Inno.Runtime.GamePresentationViewport.height`](../../src/runtime/engine/Inno.Runtime/Presentation/GamePresentationSettings.cs#L142) | Gets the content height. |
| [`int Inno.Runtime.GamePresentationViewport.width`](../../src/runtime/engine/Inno.Runtime/Presentation/GamePresentationSettings.cs#L137) | Gets the content width. |
| [`int Inno.Runtime.GamePresentationViewport.x`](../../src/runtime/engine/Inno.Runtime/Presentation/GamePresentationSettings.cs#L127) | Gets the horizontal content offset. |
| [`int Inno.Runtime.GamePresentationViewport.y`](../../src/runtime/engine/Inno.Runtime/Presentation/GamePresentationSettings.cs#L132) | Gets the vertical content offset. |
| [`Inno.Runtime.GamePresentationViewport`](../../src/runtime/engine/Inno.Runtime/Presentation/GamePresentationSettings.cs#L91) | Stores one centered game-content region within a presentation surface. |

### `Inno.Runtime.GameRuntimeAssembly`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Runtime.GameRuntimeAssembly.Validate()`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeAssembly.cs#L32) | Rejects identities or fingerprints that cannot describe a frozen code input. |
| [`string Inno.Runtime.GameRuntimeAssembly.contentFingerprint`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeAssembly.cs#L23) | Gets or sets the lowercase SHA-256 fingerprint of the frozen compiler output. |
| [`string Inno.Runtime.GameRuntimeAssembly.name`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeAssembly.cs#L17) | Gets or sets the exact simple assembly name, independent of deployment file layout. |
| [`Inno.Runtime.GameRuntimeAssembly`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeAssembly.cs#L11) | Records a logical assembly identity and the content fingerprint of its build input. |

### `Inno.Runtime.GameRuntimeManifest`

| 当前声明 | 行为 |
| --- | --- |
| [`System.Collections.Generic.IReadOnlyList<Inno.Core.Settings.ProjectSettingsContributor> Inno.Runtime.GameRuntimeManifest.CreateSettingContributors()`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeManifest.cs#L132) | Creates current-generation setting contributors from neutral manifest data. |
| [`void Inno.Runtime.GameRuntimeManifest.Validate()`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeManifest.cs#L69) | Validates startup identity, dimensions, scene, and Plugin ordering. |
| [`string Inno.Runtime.GameRuntimeManifest.applicationId`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeManifest.cs#L18) | Gets or sets the stable lowercase application identifier. |
| [`Inno.Runtime.GameRuntimeModule[] Inno.Runtime.GameRuntimeManifest.modules`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeManifest.cs#L60) | Gets or sets the dependency-ordered managed modules in the frozen Player generation. |
| [`string Inno.Runtime.GameRuntimeManifest.persistentDataPath`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeManifest.cs#L30) | Gets or sets the writable data folder below local application data; empty uses the application ID. |
| [`Inno.Runtime.GameRuntimePlugin[] Inno.Runtime.GameRuntimeManifest.plugins`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeManifest.cs#L54) | Gets or sets dependency-ordered runtime Plugin setting contributions. |
| [`string Inno.Runtime.GameRuntimeManifest.productName`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeManifest.cs#L24) | Gets or sets the player-facing product name. |
| [`string Inno.Runtime.GameRuntimeManifest.startupScene`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeManifest.cs#L36) | Gets or sets the mount-qualified startup scene path. |
| [`int Inno.Runtime.GameRuntimeManifest.windowHeight`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeManifest.cs#L48) | Gets or sets the initial logical window height. |
| [`int Inno.Runtime.GameRuntimeManifest.windowWidth`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeManifest.cs#L42) | Gets or sets the initial logical window width. |
| [`Inno.Runtime.GameRuntimeManifest`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeManifest.cs#L13) | Stores the immutable startup contract consumed by a deployed game Player. |

### `Inno.Runtime.GameRuntimeModule`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Runtime.GameRuntimeModule.Validate()`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeModule.cs#L45) | Validates module identity, ownership, file names, and dependency declarations. |
| [`Inno.Runtime.GameRuntimeAssembly[] Inno.Runtime.GameRuntimeModule.assemblies`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeModule.cs#L30) | Gets or sets the exact ordered code identities contributed by this module. |
| [`string[] Inno.Runtime.GameRuntimeModule.dependencies`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeModule.cs#L36) | Gets or sets stable module names that must be active before this module. |
| [`Inno.Extensibility.Modules.AssemblyDomain Inno.Runtime.GameRuntimeModule.domain`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeModule.cs#L24) | Gets or sets the ownership domain declared by the deployed module. |
| [`string Inno.Runtime.GameRuntimeModule.name`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeModule.cs#L18) | Gets or sets the stable module name used by the managed module host. |
| [`Inno.Runtime.GameRuntimeModule`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeModule.cs#L12) | Describes one dependency-ordered managed module in a frozen Player generation. |

### `Inno.Runtime.GameRuntimePlugin`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Runtime.GameRuntimePlugin.Validate()`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimePlugin.cs#L45) | Validates identity and ownership declarations. |
| [`string[] Inno.Runtime.GameRuntimePlugin.dependencies`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimePlugin.cs#L24) | Gets or sets Plugin IDs that must precede this Plugin. |
| [`string Inno.Runtime.GameRuntimePlugin.id`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimePlugin.cs#L18) | Gets or sets the stable Plugin identifier. |
| [`string[] Inno.Runtime.GameRuntimePlugin.overrides`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimePlugin.cs#L30) | Gets or sets dependencies whose setting defaults may be replaced. |
| [`Inno.Core.Settings.ProjectSettingRecord[] Inno.Runtime.GameRuntimePlugin.settings`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimePlugin.cs#L36) | Gets or sets neutral setting contribution records. |
| [`Inno.Runtime.GameRuntimePlugin`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimePlugin.cs#L12) | Stores one Plugin's runtime-only project setting contribution. |

### `Inno.Runtime.RuntimeContentCatalog`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Runtime.RuntimeContentCatalog.Validate()`](../../src/runtime/engine/Inno.Runtime/Deployment/RuntimeContentCatalog.cs#L55) | Validates content identity, file naming, snapshot identity, and counts. |
| [`int Inno.Runtime.RuntimeContentCatalog.artifactBundleCount`](../../src/runtime/engine/Inno.Runtime/Deployment/RuntimeContentCatalog.cs#L40) | Gets or sets the number of content-addressed artifact bundles in the pack. |
| [`int Inno.Runtime.RuntimeContentCatalog.assetCount`](../../src/runtime/engine/Inno.Runtime/Deployment/RuntimeContentCatalog.cs#L34) | Gets or sets the number of runtime assets in the enclosed asset catalog. |
| [`string Inno.Runtime.RuntimeContentCatalog.contentHash`](../../src/runtime/engine/Inno.Runtime/Deployment/RuntimeContentCatalog.cs#L16) | Gets or sets the SHA-256 identity of the complete content pack bytes. |
| [`string Inno.Runtime.RuntimeContentCatalog.packFileName`](../../src/runtime/engine/Inno.Runtime/Deployment/RuntimeContentCatalog.cs#L22) | Gets or sets the content pack file name relative to the packaged Content directory. |
| [`int Inno.Runtime.RuntimeContentCatalog.runtimeAssemblyCount`](../../src/runtime/engine/Inno.Runtime/Deployment/RuntimeContentCatalog.cs#L46) | Gets or sets the number of runtime assemblies in the pack. |
| [`string Inno.Runtime.RuntimeContentCatalog.snapshotFingerprint`](../../src/runtime/engine/Inno.Runtime/Deployment/RuntimeContentCatalog.cs#L28) | Gets or sets the combined build input snapshot fingerprint. |
| [`Inno.Runtime.RuntimeContentCatalog`](../../src/runtime/engine/Inno.Runtime/Deployment/RuntimeContentCatalog.cs#L11) | Describes the single immutable content pack deployed with a Player build. |

### `Inno.Runtime.RuntimeJobExecutionMode`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Runtime.RuntimeJobExecutionMode.SingleThread`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeJobExecutionMode.cs#L11) | Executes jobs deterministically on the session owner thread. |
| [`Inno.Runtime.RuntimeJobExecutionMode.WorkerPool`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeJobExecutionMode.cs#L16) | Executes ready jobs on a bounded worker pool owned by the session. |
| [`Inno.Runtime.RuntimeJobExecutionMode`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeJobExecutionMode.cs#L6) | Selects how one runtime session executes its frame jobs. |

### `Inno.Runtime.RuntimeManifestEnvelope`

| 当前声明 | 行为 |
| --- | --- |
| [`static Inno.Runtime.GameRuntimeManifest Inno.Runtime.RuntimeManifestEnvelope.Decode(System.ReadOnlySpan<byte> data, Inno.Core.Serialization.SerializationGeneration serialization)`](../../src/runtime/engine/Inno.Runtime/Deployment/RuntimeManifestEnvelope.cs#L118) | Deserializes and validates the complete runtime manifest after engine serialization is available. |
| [`static byte[] Inno.Runtime.RuntimeManifestEnvelope.Encode(Inno.Runtime.GameRuntimeManifest manifest, Inno.Core.Serialization.SerializationGeneration serialization)`](../../src/runtime/engine/Inno.Runtime/Deployment/RuntimeManifestEnvelope.cs#L34) | Encodes a validated runtime manifest into the strict deployment envelope. |
| [`static string Inno.Runtime.RuntimeManifestEnvelope.ReadApplicationId(System.ReadOnlySpan<byte> data)`](../../src/runtime/engine/Inno.Runtime/Deployment/RuntimeManifestEnvelope.cs#L76) | Reads and validates the application identity without requiring serialization services to be initialized. |
| [`static string Inno.Runtime.RuntimeManifestEnvelope.ReadPersistentDataPath(System.ReadOnlySpan<byte> data)`](../../src/runtime/engine/Inno.Runtime/Deployment/RuntimeManifestEnvelope.cs#L95) | Reads the validated writable data folder before engine serialization is initialized. |
| [`Inno.Runtime.RuntimeManifestEnvelope`](../../src/runtime/engine/Inno.Runtime/Deployment/RuntimeManifestEnvelope.cs#L12) | Frames the serialized runtime manifest with the application identity required before engine startup. |

### `Inno.Runtime.RuntimeSession`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Runtime.RuntimeSession.Dispose()`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSession.cs#L299) | Releases scene, asset, scheduling, serialization, and logging ownership for this session. |
| [`System.IDisposable Inno.Runtime.RuntimeSession.EnterExecutionScope()`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSession.cs#L181) | Binds this session's script façades to the current asynchronous execution context. |
| [`void Inno.Runtime.RuntimeSession.StopCoroutines(object owner)`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSession.cs#L286) | Stops every coroutine owned by a runtime object before that object is retired by an atomic reload. |
| [`void Inno.Runtime.RuntimeSession.Tick(float deltaTime)`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSession.cs#L216) | Advances session events, jobs, coroutines, and scene lifecycle by one frame. |
| [`Inno.Assets.AssetDatabase Inno.Runtime.RuntimeSession.assets`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSession.cs#L168) | Gets the source-free runtime asset database configured for this session. |
| [`Inno.Core.Events.EventDispatcher Inno.Runtime.RuntimeSession.events`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSession.cs#L118) | Gets the event dispatcher owned by this session. |
| [`Inno.Core.Identity.IdentityAllocator Inno.Runtime.RuntimeSession.identities`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSession.cs#L123) | Gets the identity domain that owns every live object in this isolated session. |
| [`bool Inno.Runtime.RuntimeSession.isPaused`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSession.cs#L151) | Gets or sets whether scaled simulation is paused while unscaled subsystems continue to receive frames. |
| [`Inno.Runtime.RuntimeSessionOptions Inno.Runtime.RuntimeSession.options`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSession.cs#L108) | Gets the validated immutable options used to create this session. |
| [`Inno.References.ReferenceCatalog Inno.Runtime.RuntimeSession.references`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSession.cs#L128) | Gets the immutable cross-domain reference resolver generation owned by this session. |
| [`Inno.Scene.SceneWorld Inno.Runtime.RuntimeSession.scenes`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSession.cs#L160) | Gets the isolated scene world owned by this session. |
| [`Inno.Core.Logging.LogSessionId Inno.Runtime.RuntimeSession.sessionId`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSession.cs#L113) | Gets the unique logging identity assigned to this session. |
| [`Inno.Runtime.RuntimeSubsystemPipeline Inno.Runtime.RuntimeSession.subsystems`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSession.cs#L133) | Gets the dependency-ordered subsystem generation owned by this session. |
| [`float Inno.Runtime.RuntimeSession.timeScale`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSession.cs#L142) | Gets or sets the finite non-negative simulation time multiplier. |
| [`Inno.Runtime.RuntimeSession`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSession.cs#L22) | Owns all mutable simulation, identity, asset, scheduling, and logging state for one isolated execution session. |

### `Inno.Runtime.RuntimeSessionKind`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Runtime.RuntimeSessionKind.Edit`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionKind.cs#L11) | An authoring session whose scene world remains editable. |
| [`Inno.Runtime.RuntimeSessionKind.Play`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionKind.cs#L16) | A disposable Editor play-test session created from an immutable start snapshot. |
| [`Inno.Runtime.RuntimeSessionKind.Player`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionKind.cs#L21) | A deployed standalone game session backed only by runtime artifacts. |
| [`Inno.Runtime.RuntimeSessionKind`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionKind.cs#L6) | Identifies the ownership and lifecycle semantics of an isolated runtime session. |

### `Inno.Runtime.RuntimeSessionOptions`

| 当前声明 | 行为 |
| --- | --- |
| [`string Inno.Runtime.RuntimeSessionOptions.applicationId`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionOptions.cs#L24) | Gets or initializes the stable application identifier used to isolate persistent data. |
| [`long Inno.Runtime.RuntimeSessionOptions.assetPreparationBudgetBytes`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionOptions.cs#L53) | Gets or initializes the maximum simultaneous cold asset payload preparation bytes before admission is rejected. |
| [`long Inno.Runtime.RuntimeSessionOptions.assetResidencyBudgetBytes`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionOptions.cs#L48) | Gets or initializes the runtime asset payload residency budget in bytes. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Runtime.Contracts.RuntimeCapabilityId> Inno.Runtime.RuntimeSessionOptions.capabilities`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionOptions.cs#L89) | Gets or initializes capabilities verified by composition for this session's selected services. |
| [`Inno.Content.IRuntimeContentStore? Inno.Runtime.RuntimeSessionOptions.contentStore`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionOptions.cs#L33) | Gets or initializes the immutable content store borrowed for the session lifetime. |
| [`System.Func<Inno.Core.Logging.LogSessionId, Inno.Core.Logging.ILogSink>? Inno.Runtime.RuntimeSessionOptions.createLogSink`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionOptions.cs#L43) | Gets or initializes the optional factory that transfers one session log sink to this session. |
| [`System.Func<Inno.Runtime.RuntimeSession, System.Collections.Generic.IReadOnlyList<Inno.Runtime.Contracts.IRuntimeSubsystemFactory>> Inno.Runtime.RuntimeSessionOptions.createSubsystems`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionOptions.cs#L83) | Gets or initializes the backend-neutral subsystem factories composed into each session. |
| [`float Inno.Runtime.RuntimeSessionOptions.fixedDeltaTime`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionOptions.cs#L58) | Gets or initializes the fixed simulation interval in seconds. |
| [`Inno.Runtime.RuntimeJobExecutionMode Inno.Runtime.RuntimeSessionOptions.jobExecutionMode`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionOptions.cs#L73) | Gets or initializes the job execution strategy owned by this session. |
| [`int Inno.Runtime.RuntimeSessionOptions.jobWorkerCount`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionOptions.cs#L78) | Gets or initializes the worker count used by the work-stealing scheduler; zero selects the default. |
| [`Inno.Runtime.RuntimeSessionKind Inno.Runtime.RuntimeSessionOptions.kind`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionOptions.cs#L19) | Gets or initializes the session role. |
| [`int Inno.Runtime.RuntimeSessionOptions.maxFixedStepsPerFrame`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionOptions.cs#L68) | Gets or initializes the maximum number of fixed updates performed by one frame tick. |
| [`float Inno.Runtime.RuntimeSessionOptions.maxFrameDeltaTime`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionOptions.cs#L63) | Gets or initializes the maximum accepted variable frame interval in seconds. |
| [`System.Collections.Generic.IReadOnlyList<Inno.References.IReferenceResolver> Inno.Runtime.RuntimeSessionOptions.referenceResolvers`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionOptions.cs#L97) | Gets or initializes owner-provided resolvers included in this session's immutable reference catalog. |
| [`Inno.Runtime.RuntimeSessionOptions`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionOptions.cs#L14) | Defines storage, scheduling, and timing policy for one runtime session. |

### `Inno.Runtime.RuntimeSubsystemPipeline`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Runtime.RuntimeSubsystemPipeline.BeginFrame(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/engine/Inno.Runtime/Subsystems/RuntimeSubsystemPipeline.cs#L164) | Opens foundation scopes and immutable snapshots for one owner frame. |
| [`void Inno.Runtime.RuntimeSubsystemPipeline.Dispose()`](../../src/runtime/engine/Inno.Runtime/Subsystems/RuntimeSubsystemPipeline.cs#L309) | Detaches and disposes every subsystem in reverse dependency order. |
| [`void Inno.Runtime.RuntimeSubsystemPipeline.EndFrame(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/engine/Inno.Runtime/Subsystems/RuntimeSubsystemPipeline.cs#L282) | Closes every successfully opened frame scope in reverse dependency order. |
| [`void Inno.Runtime.RuntimeSubsystemPipeline.FixedUpdate(Inno.Runtime.Contracts.RuntimeFixedFrame frame)`](../../src/runtime/engine/Inno.Runtime/Subsystems/RuntimeSubsystemPipeline.cs#L184) | Advances every subsystem by one deterministic fixed step. |
| [`TFeature Inno.Runtime.RuntimeSubsystemPipeline.GetRequiredSubsystem<TFeature>()`](../../src/runtime/engine/Inno.Runtime/Subsystems/RuntimeSubsystemPipeline.cs#L140) | Resolves the unique active subsystem that implements the requested contract. |
| [`void Inno.Runtime.RuntimeSubsystemPipeline.LateUpdate(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/engine/Inno.Runtime/Subsystems/RuntimeSubsystemPipeline.cs#L214) | Advances state that depends on completed simulation. |
| [`void Inno.Runtime.RuntimeSubsystemPipeline.RenderFrame(Inno.Runtime.Contracts.RuntimeFrame frame, System.Action? submit = null)`](../../src/runtime/engine/Inno.Runtime/Subsystems/RuntimeSubsystemPipeline.cs#L232) | Opens output resources, accepts product requests and submits each owner's output once. |
| [`void Inno.Runtime.RuntimeSubsystemPipeline.Update(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/engine/Inno.Runtime/Subsystems/RuntimeSubsystemPipeline.cs#L199) | Advances variable-clock domain state. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Runtime.Contracts.RuntimeSubsystemDescriptor> Inno.Runtime.RuntimeSubsystemPipeline.descriptors`](../../src/runtime/engine/Inno.Runtime/Subsystems/RuntimeSubsystemPipeline.cs#L118) | Gets the active immutable subsystem descriptors in execution order. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Core.Diagnostics.Diagnostic> Inno.Runtime.RuntimeSubsystemPipeline.startupDiagnostics`](../../src/runtime/engine/Inno.Runtime/Subsystems/RuntimeSubsystemPipeline.cs#L123) | Gets immutable diagnostics for unavailable optional subsystems; no exception or extension instance is retained. |
| [`Inno.Runtime.RuntimeSubsystemPipeline`](../../src/runtime/engine/Inno.Runtime/Subsystems/RuntimeSubsystemPipeline.cs#L14) | Owns the dependency-ordered runtime subsystem generation for one Host or Session scope. |

### `Inno.Runtime.StaticAssemblyCatalogSource`

| 当前声明 | 行为 |
| --- | --- |
| [`System.Action? Inno.Runtime.StaticAssemblyCatalogSource.changed`](../../src/runtime/engine/Inno.Runtime/Registration/StaticAssemblyCatalogSource.cs#L46) | See the implemented contract. |
| [`Inno.Runtime.StaticAssemblyCatalogSource.StaticAssemblyCatalogSource(System.Collections.Generic.IReadOnlyList<System.Reflection.Assembly> assemblies, System.Collections.Generic.IReadOnlyList<System.Reflection.Assembly> frameworkAssemblies)`](../../src/runtime/engine/Inno.Runtime/Registration/StaticAssemblyCatalogSource.cs#L29) | Copies the host and framework identities provided by the generated platform composition. |
| [`void Inno.Runtime.StaticAssemblyCatalogSource.Dispose()`](../../src/runtime/engine/Inno.Runtime/Registration/StaticAssemblyCatalogSource.cs#L81) | See the implemented contract. |
| [`System.Collections.Generic.IReadOnlyList<System.Reflection.Assembly> Inno.Runtime.StaticAssemblyCatalogSource.GetAssemblies()`](../../src/runtime/engine/Inno.Runtime/Registration/StaticAssemblyCatalogSource.cs#L53) | See the implemented contract. |
| [`System.Collections.Generic.IReadOnlyList<System.Reflection.Assembly> Inno.Runtime.StaticAssemblyCatalogSource.GetSharedAssemblies()`](../../src/runtime/engine/Inno.Runtime/Registration/StaticAssemblyCatalogSource.cs#L60) | See the implemented contract. |
| [`bool Inno.Runtime.StaticAssemblyCatalogSource.IsFrameworkAssembly(System.Reflection.Assembly assembly)`](../../src/runtime/engine/Inno.Runtime/Registration/StaticAssemblyCatalogSource.cs#L74) | See the implemented contract. |
| [`bool Inno.Runtime.StaticAssemblyCatalogSource.IsFrameworkReference(string assemblyName)`](../../src/runtime/engine/Inno.Runtime/Registration/StaticAssemblyCatalogSource.cs#L67) | See the implemented contract. |
| [`Inno.Runtime.StaticAssemblyCatalogSource`](../../src/runtime/engine/Inno.Runtime/Registration/StaticAssemblyCatalogSource.cs#L12) | Supplies an explicit linked host closure without runtime assembly probing or dynamic loading. |

### `Inno.Runtime.StaticModuleSource`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Runtime.StaticModuleSource.StaticModuleSource(string moduleName, Inno.Extensibility.Modules.AssemblyDomain domain, Inno.Extensibility.Modules.AssemblyScope scope, System.Collections.Generic.IReadOnlyList<System.Reflection.Assembly> assemblies, System.Collections.Generic.IReadOnlyList<string> dependencies)`](../../src/runtime/engine/Inno.Runtime/Registration/StaticModuleSource.cs#L35) | Freezes a linked module's explicit assemblies and logical dependencies. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Runtime.StaticModuleSource.GetAssemblyNames()`](../../src/runtime/engine/Inno.Runtime/Registration/StaticModuleSource.cs#L66) | See the implemented contract. |
| [`Inno.Extensibility.Modules.ModuleCatalogContribution Inno.Runtime.StaticModuleSource.Prepare(Inno.Extensibility.Modules.ModuleSourceContext context)`](../../src/runtime/engine/Inno.Runtime/Registration/StaticModuleSource.cs#L69) | See the implemented contract. |
| [`System.Collections.Generic.IReadOnlyDictionary<string, Inno.Extensibility.Modules.AssemblyScope> Inno.Runtime.StaticModuleSource.assemblyScopes`](../../src/runtime/engine/Inno.Runtime/Registration/StaticModuleSource.cs#L64) | See the implemented contract. |
| [`bool Inno.Runtime.StaticModuleSource.collectible`](../../src/runtime/engine/Inno.Runtime/Registration/StaticModuleSource.cs#L60) | See the implemented contract. |
| [`Inno.Extensibility.Modules.AssemblyDomain Inno.Runtime.StaticModuleSource.domain`](../../src/runtime/engine/Inno.Runtime/Registration/StaticModuleSource.cs#L56) | See the implemented contract. |
| [`string Inno.Runtime.StaticModuleSource.moduleName`](../../src/runtime/engine/Inno.Runtime/Registration/StaticModuleSource.cs#L54) | See the implemented contract. |
| [`Inno.Extensibility.Modules.AssemblyScope Inno.Runtime.StaticModuleSource.scope`](../../src/runtime/engine/Inno.Runtime/Registration/StaticModuleSource.cs#L58) | See the implemented contract. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Runtime.StaticModuleSource.upstreamModuleNames`](../../src/runtime/engine/Inno.Runtime/Registration/StaticModuleSource.cs#L62) | See the implemented contract. |
| [`Inno.Runtime.StaticModuleSource`](../../src/runtime/engine/Inno.Runtime/Registration/StaticModuleSource.cs#L13) | Contributes statically linked module code to the same catalog transactions used by authoring hosts. |

### `Inno.Runtime.StaticTypeCatalogSource`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Runtime.StaticTypeCatalogSource.StaticTypeCatalogSource(System.Collections.Generic.IReadOnlyList<System.Action<Inno.Extensibility.Catalogs.ITypeCatalogRegistrar>> catalogs)`](../../src/runtime/engine/Inno.Runtime/Registration/StaticTypeCatalogSource.cs#L31) | Executes generated contributions once and freezes the resulting metadata and factory closure. |
| [`bool Inno.Runtime.StaticTypeCatalogSource.CanCreateInstance(System.Type type)`](../../src/runtime/engine/Inno.Runtime/Registration/StaticTypeCatalogSource.cs#L110) | See the implemented contract. |
| [`System.Type? Inno.Runtime.StaticTypeCatalogSource.ConstructGenericType(System.Type definition, System.Collections.Generic.IReadOnlyList<System.Type> arguments)`](../../src/runtime/engine/Inno.Runtime/Registration/StaticTypeCatalogSource.cs#L87) | See the implemented contract. |
| [`object Inno.Runtime.StaticTypeCatalogSource.CreateInstance(System.Type type)`](../../src/runtime/engine/Inno.Runtime/Registration/StaticTypeCatalogSource.cs#L114) | See the implemented contract. |
| [`Inno.Extensibility.Catalogs.TypeCatalogMetadata Inno.Runtime.StaticTypeCatalogSource.GetMetadata(System.Type type)`](../../src/runtime/engine/Inno.Runtime/Registration/StaticTypeCatalogSource.cs#L82) | See the implemented contract. |
| [`System.Collections.Generic.IReadOnlyList<System.Type> Inno.Runtime.StaticTypeCatalogSource.GetTypes(System.Reflection.Assembly assembly)`](../../src/runtime/engine/Inno.Runtime/Registration/StaticTypeCatalogSource.cs#L77) | See the implemented contract. |
| [`Inno.Runtime.StaticTypeCatalogSource`](../../src/runtime/engine/Inno.Runtime/Registration/StaticTypeCatalogSource.cs#L14) | Resolves type metadata and factories from linked registrations without runtime type discovery. |

### `Inno.Runtime.Time`

| 当前声明 | 行为 |
| --- | --- |
| [`static float Inno.Runtime.Time.deltaTime`](../../src/runtime/engine/Inno.Runtime/Execution/Time.cs#L31) | Gets the current variable frame interval in seconds. |
| [`static float Inno.Runtime.Time.fixedDeltaTime`](../../src/runtime/engine/Inno.Runtime/Execution/Time.cs#L63) | Gets the interval of the active fixed simulation step in seconds. |
| [`static float Inno.Runtime.Time.fixedTime`](../../src/runtime/engine/Inno.Runtime/Execution/Time.cs#L55) | Gets the accumulated fixed simulation time in seconds. |
| [`static long Inno.Runtime.Time.frameCount`](../../src/runtime/engine/Inno.Runtime/Execution/Time.cs#L95) | Gets the number of variable frames begun by this session. |
| [`static bool Inno.Runtime.Time.isPaused`](../../src/runtime/engine/Inno.Runtime/Execution/Time.cs#L83) | Gets whether scaled simulation is paused. |
| [`static float Inno.Runtime.Time.time`](../../src/runtime/engine/Inno.Runtime/Execution/Time.cs#L23) | Gets the total elapsed session time in seconds. |
| [`static float Inno.Runtime.Time.timeScale`](../../src/runtime/engine/Inno.Runtime/Execution/Time.cs#L71) | Gets the current simulation time multiplier. |
| [`static float Inno.Runtime.Time.unscaledDeltaTime`](../../src/runtime/engine/Inno.Runtime/Execution/Time.cs#L47) | Gets the current frame interval unaffected by pause or time scaling. |
| [`static float Inno.Runtime.Time.unscaledTime`](../../src/runtime/engine/Inno.Runtime/Execution/Time.cs#L39) | Gets total elapsed session time unaffected by pause or time scaling. |
| [`Inno.Runtime.Time`](../../src/runtime/engine/Inno.Runtime/Execution/Time.cs#L15) | Provides Unity-style timing values for the runtime session bound to the current execution context. |

## 项目依赖

- [Inno.Extensibility.Modules](../extensibility/Inno.Extensibility.Modules.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Core.Coroutines](../core/Inno.Core.Coroutines.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Core.Jobs](../core/Inno.Core.Jobs.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Assets](../assets/Inno.Assets.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Content](../assets/Inno.Content.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Core.Events](../core/Inno.Core.Events.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Core.Identity](../core/Inno.Core.Identity.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Core.Logging](../core/Inno.Core.Logging.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Core.Settings](../core/Inno.Core.Settings.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.References](../references/Inno.References.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Scene](../scene/Inno.Scene.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Runtime.Contracts](Inno.Runtime.Contracts.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Extensibility.Reload](../extensibility/Inno.Extensibility.Reload.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：项目引用；公开签名可见性由语义边界检查确认。

共同 MSBuild 注入的 analyzer 与编译规则属于构建依赖，完整有效项目图记录在本轮验收证据中。
