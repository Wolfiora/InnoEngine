# Inno.Plugins.Authoring

[Plugins 索引](README.md) · [Assets](../assets/Inno.Assets.Pipeline.md) · [Build](../build/Inno.Build.md)

## 职责与依赖

该 project 只发现 `Plugins/*.iplugin`，执行 archive 安全校验、依赖拓扑、只读 Asset Source Mount 候选和原子 Plugin generation 激活。Folder Plugin、`.zip` 文件和其他扩展名会产生安装诊断，不进入候选。该 project 属于 Editor/Build authoring closure，不进入 Player。

## 公开 API

内部依赖 Core Execution 的统一退休异常分类；不会向游戏脚本暴露底层退休调度器或原生 callback。

- `PluginEnvironment`：active/discovery generation、pending code activation、owner-thread Update/Refresh。
- `PluginSourceService`：一次隔离 Scan 与 `.iplugin` validation。
- `PluginSourceKind`, `PluginSourceLimits`：安装源种类和资源上限。
- `PluginCandidate`, `PluginDiagnostic`, `PluginScanResult`：不可变候选与诊断模型。

## 工作流

Application 先 Scan，再以相同结果构造 Asset source mounts 和 `PluginEnvironment`。`.iplugin` 会先物化到 `<Project>/Library/Plugins/<pluginId>/<contentHash>` 的不可变、内容寻址 generation snapshot；active 与 candidate Source Mount 只读取各自 snapshot。因此安装包被删除或原子替换时，不会令正在读取的 generation、事务 rollback 或 Editor shutdown 重新访问失效路径。

物化复用 Core.IO 的 `AtomicDirectory.Publish`，包括 Windows 短期文件读者的有界 rename 重试。
并发扫描可以准备独立 staging，但只能接受已经完整提交的同一内容目录。复用缓存时核对确切文件集合、
必需目录、长度与 SHA-256；缓存损坏产生隔离诊断，不能把该目录直接交给 Asset Mount。
已发布 snapshot 不原位覆盖；删除损坏的可重建缓存后重新扫描，可从当前安装包恢复同一内容身份。

文件系统 watcher 只合并变化信号；候选激活回到 Asset owner thread。不可变 old snapshot 的用途是保证原子事务在真正的 participant/迁移异常时能够完整 rollback，它不用于否认安装集合已经发生的删除或不可用更新。若 active Plugin 被物理删除、结构校验失败、Asset metadata 无法构成候选，或更新后的代码无法完成全量脚本编译，Scripting 会在帧安全点提交 unavailable generation：退休该 Plugin module 及其完整反向依赖脚本闭包，并提交与当前安装内容一致的只读 Mount/Catalog/Settings。普通、与 Plugin availability 无关的 Project C# 编译失败仍不会改变 active generation。

Unavailable generation 中的 Scene Component/System 会原位转换为 `MissingGameComponent` / `MissingGameSystem`，保留 Stable Type ID、persistent ID、序列化状态和顺序；Project Scripts 因缺失 Plugin API 而无法重建时，其所在脚本 module 同样退休并进入 Missing。相同 Stable ID 的有效 Plugin 和 Project Scripts 再次编译成功后，下一次原子 reload 自动恢复真实类型和保留状态。结构或 Asset 候选失败会发布 discovery diagnostic，不会从 `Refresh`、File Browser、reload rollback 或 shutdown 抛出安装目录失效异常。

Archive 路径、entry 数、压缩比、大小、case collision、symbolic link、重复 ID、缺失依赖和循环均在 candidate 阶段拒绝。内嵌依赖也必须是 `Dependencies/<id>.iplugin`。安装源永远只读，修改必须通过外部替换完整 `.iplugin` 文件。

## 退出与异常边界

普通输入校验失败可以进入 unavailable 候选，但直接或嵌套在 `AggregateException` / `InnerException` 中的
`RetirementPendingException` 不属于可隔离的输入错误。初始激活、候选准备、激活和观察者通知都必须立即传播该信号；
不能再调用后续观察者，不能自动回滚仍在使用的候选，也不能以空 Plugin 集合继续启动。普通错误先发生、之后退休阻塞时，
两者保留在同一异常树中，不丢失最初的激活/回滚错误。

`RollbackPending()` 只有在 Asset、Catalog 和 Settings contributor 回滚全部返回后才清空候选所有权。
回滚中断时 `hasPendingActivation` 和 `compilationAssets` 仍指向原事务；`Dispose()` 也不能越过该事务提前拆卸其他依赖。
外层 generation owner 仍负责统一 Fault / GC unload barrier；这不是允许在 Faulted Host 内重新发起 generation 的入口。
构造器在初始激活成功后才创建 watcher，避免激活失败时遗留不可达的文件系统订阅。

## 共享 Recovery 与后台工作

`PluginEnvironment` 构造注入 Host 唯一的 `GenerationCoordinator`。`CreateReloadChange()` 返回真实的 ReferenceRecoveryTransaction：Prepare 捕获 Settings neutral effective snapshot，Apply 发布 mount/catalog/contributor 并更新 Assets/Settings，Validate 检查 source 身份与 owner 恢复结果，Complete 提交，失败先恢复旧类型发布再恢复旧内容。

有代码变化时由 IScriptReloadCoordinator.Execute(reload, externalChange) 把同一个 change 与 Editor participant 合并；无代码变化的安装内容使用 ApplyPendingChange() 经过同一 gate。低层 ActivatePending/CommitPending/RollbackPending 只用于组装该事务，Activate 失败不自行抢先补偿。没有两段 Action callback 或按领域中央 switch。

Plugin/Setting ID 是稳定语义，不能伪造 object Identity；mount 内实际 Asset slots 仍走共享 ReferenceRecoveryTransaction。Settings、Graph 的中立结构 participant 可以没有 object slots，但必须真实捕获/验证/回滚其领域状态。

后台 reconciliation 属于 LifetimeScope.RunAsync，预期 IO 变化返回中立重试结果；停止时先取消/排空，确认 Task 退休后才拆 watcher、sources、catalog。不得遗留未拥有的 Task.Run 或复制 AsyncLocal generation 根。PluginCandidate.manifest 返回深复制，Scan/Catalog 列表冻结；修改外部记录不会修改已发布 generation。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Plugins.Authoring.PluginCandidate`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.Pipeline.AssetSourceMount Inno.Plugins.Authoring.PluginCandidate.sourceMount`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L112) | Gets the isolated read-only asset source mount. |
| [`Inno.Plugins.Authoring.PluginCandidate`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L70) | Describes one validated installed Plugin source candidate. |
| [`Inno.Plugins.Authoring.PluginSourceKind Inno.Plugins.Authoring.PluginCandidate.sourceKind`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L97) | Gets the physical source container kind. |
| [`Inno.Plugins.PluginManifest Inno.Plugins.Authoring.PluginCandidate.manifest`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L107) | Gets a detached copy of the validated native manifest, including its nested contribution payloads. |
| [`bool Inno.Plugins.Authoring.PluginCandidate.containsCode`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L117) | Gets whether source code exists in this Plugin. |
| [`string Inno.Plugins.Authoring.PluginCandidate.contentHash`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L102) | Gets the deterministic complete source-content hash. |
| [`string Inno.Plugins.Authoring.PluginCandidate.sourcePath`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L92) | Gets the installed .iplugin package path. |

### `Inno.Plugins.Authoring.PluginDiagnostic`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Plugins.Authoring.PluginDiagnostic`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L133) | Reports one Plugin discovery, validation, or dependency problem. |
| [`Inno.Plugins.Authoring.PluginDiagnostic.PluginDiagnostic(string sourcePath, string message)`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L144) | Creates a Plugin source diagnostic. |
| [`string Inno.Plugins.Authoring.PluginDiagnostic.message`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L160) | Gets the actionable problem description. |
| [`string Inno.Plugins.Authoring.PluginDiagnostic.sourcePath`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L155) | Gets the related .iplugin package path or invalid installation entry. |

### `Inno.Plugins.Authoring.PluginEnvironment`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.Pipeline.AssetSourceMountTransaction? Inno.Plugins.Authoring.PluginEnvironment.compilationAssets`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L322) | Gets the isolated Asset candidate used by the next Plugin script compilation. |
| [`Inno.Extensibility.Reload.IGenerationChange Inno.Plugins.Authoring.PluginEnvironment.CreateReloadChange()`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L292) | Captures Plugin availability, Asset mounts and effective settings as one shared recovery participant. |
| [`Inno.Plugins.Authoring.PluginEnvironment`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L23) | Owns automatic installed Plugin source discovery and atomically publishes Asset mounts, settings contributors, and the active Plugin catalog on the Asset Database owner thread. |
| [`Inno.Plugins.Authoring.PluginEnvironment.PluginEnvironment(Inno.Assets.Pipeline.AssetPipeline assets, Inno.Core.Settings.ProjectSettingsStore settings, Inno.Core.Serialization.SerializationRegistry serialization, string pluginRoot, string libraryRoot, Inno.Plugins.Authoring.PluginScanResult initialScan, Inno.Extensibility.Reload.GenerationCoordinator generations)`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L114) | Creates automatic Plugin discovery around one asset pipeline and project settings store. |
| [`Inno.Plugins.Authoring.PluginScanResult Inno.Plugins.Authoring.PluginEnvironment.discovery`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L79) | Gets the latest installed Plugin discovery and isolated diagnostics. |
| [`System.Action? Inno.Plugins.Authoring.PluginEnvironment.ActivationCandidateChanged`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L51) | Occurs after a validated Plugin generation is staged for script compilation but before it becomes active. |
| [`System.Action? Inno.Plugins.Authoring.PluginEnvironment.Changed`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L84) | Occurs after discovery diagnostics or the active Plugin generation changes. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Plugins.Authoring.PluginCandidate> Inno.Plugins.Authoring.PluginEnvironment.activePlugins`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L69) | Gets the dependency-ordered active Plugin generation. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Plugins.Authoring.PluginCandidate> Inno.Plugins.Authoring.PluginEnvironment.compilationPlugins`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L335) | Gets the Plugin candidates visible to the next script compilation. |
| [`bool Inno.Plugins.Authoring.PluginEnvironment.Refresh()`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L182) | Forces validation and prepares or commits one atomic Plugin availability generation. |
| [`bool Inno.Plugins.Authoring.PluginEnvironment.TryGet(Inno.Assets.AssetSourceId source, out Inno.Plugins.Authoring.PluginCandidate? plugin)`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L382) | Tries to resolve the active Plugin that owns an asset source. |
| [`bool Inno.Plugins.Authoring.PluginEnvironment.TryGetCompilationPlugin(Inno.Assets.AssetSourceId source, out Inno.Plugins.Authoring.PluginCandidate? plugin)`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L361) | Resolves a Plugin owned by the active or pending script-compilation generation. |
| [`bool Inno.Plugins.Authoring.PluginEnvironment.hasPendingActivation`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L309) | Gets whether a source-mount candidate is waiting for successful script generation activation. |
| [`bool Inno.Plugins.Authoring.PluginEnvironment.isInitialized`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L56) | Gets whether project Plugin management is initialized. |
| [`long Inno.Plugins.Authoring.PluginEnvironment.revision`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L74) | Gets the monotonic identity of the active Plugin generation. |
| [`void Inno.Plugins.Authoring.PluginEnvironment.ActivatePending()`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L390) | Provisionally publishes at the shared generation safety point; the owning change must roll back failures. |
| [`void Inno.Plugins.Authoring.PluginEnvironment.ApplyPendingChange()`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L303) | Commits a non-code availability change using the same generation gate and recovery phases as script reload. |
| [`void Inno.Plugins.Authoring.PluginEnvironment.CommitPending()`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L409) | Commits the pending mount generation after scripts, assets, settings, and registries activate. |
| [`void Inno.Plugins.Authoring.PluginEnvironment.Dispose()`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L461) | Stops automatic discovery without deleting Plugin sources or rebuildable extraction caches. |
| [`void Inno.Plugins.Authoring.PluginEnvironment.RollbackPending()`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L434) | Restores the complete last-good mount, catalog, and settings contributor generation. |
| [`void Inno.Plugins.Authoring.PluginEnvironment.Update()`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L154) | Processes debounced Plugin source notifications and completed background reconciliation. |

### `Inno.Plugins.Authoring.PluginScanResult`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Plugins.Authoring.PluginScanResult`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L166) | Contains a complete immutable Plugin discovery result. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Plugins.Authoring.PluginCandidate> Inno.Plugins.Authoring.PluginScanResult.candidates`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L179) | Gets dependency-ordered valid candidates. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Plugins.Authoring.PluginDiagnostic> Inno.Plugins.Authoring.PluginScanResult.diagnostics`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L184) | Gets isolated diagnostics for rejected candidates. |

### `Inno.Plugins.Authoring.PluginSourceKind`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Plugins.Authoring.PluginSourceKind`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L13) | Identifies how an .iplugin package participates in one installation. |
| [`Inno.Plugins.Authoring.PluginSourceKind.EmbeddedPackage`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L23) | The Plugin is embedded as one complete .iplugin dependency package. |
| [`Inno.Plugins.Authoring.PluginSourceKind.Package`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L18) | The Plugin is installed as one top-level .iplugin package. |

### `Inno.Plugins.Authoring.PluginSourceLimits`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Plugins.Authoring.PluginSourceLimits`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L29) | Controls bounded validation and archive extraction for installed Plugin packages. |
| [`double Inno.Plugins.Authoring.PluginSourceLimits.maximumCompressionRatio`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L54) | Gets or initializes the maximum accepted archive uncompressed-to-compressed ratio. |
| [`int Inno.Plugins.Authoring.PluginSourceLimits.maximumEmbeddedDepth`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L64) | Gets or initializes the maximum nested embedded dependency depth. |
| [`int Inno.Plugins.Authoring.PluginSourceLimits.maximumEmbeddedPluginCount`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L59) | Gets or initializes the maximum number of embedded dependency packages in one installation. |
| [`int Inno.Plugins.Authoring.PluginSourceLimits.maximumEntryCount`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L39) | Gets or initializes the maximum number of source entries. |
| [`long Inno.Plugins.Authoring.PluginSourceLimits.maximumFileBytes`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L44) | Gets or initializes the maximum size of one source file. |
| [`long Inno.Plugins.Authoring.PluginSourceLimits.maximumTotalBytes`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L49) | Gets or initializes the maximum total source size. |
| [`static Inno.Plugins.Authoring.PluginSourceLimits Inno.Plugins.Authoring.PluginSourceLimits.defaults`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L34) | Gets default conservative local Plugin limits. |

### `Inno.Plugins.Authoring.PluginSourceService`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Plugins.Authoring.PluginScanResult Inno.Plugins.Authoring.PluginSourceService.Scan()`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceService.cs#L75) | Scans every installed .iplugin package into one isolated candidate snapshot. |
| [`Inno.Plugins.Authoring.PluginSourceService`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceService.cs#L21) | Discovers, validates, mounts, and dependency-orders local .iplugin packages. |
| [`Inno.Plugins.Authoring.PluginSourceService.PluginSourceService(Inno.Core.Serialization.SerializationRegistry serialization, string pluginRoot, string libraryRoot, Inno.Plugins.Authoring.PluginSourceLimits? limits = null)`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceService.cs#L52) | Creates a local Plugin source service. |
| [`static System.Collections.Generic.IReadOnlyList<Inno.Assets.Pipeline.AssetSourceMount> Inno.Plugins.Authoring.PluginSourceService.GetActivatableMounts(Inno.Plugins.Authoring.PluginScanResult result)`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceService.cs#L111) | Gets read-only mounts that passed validation and dependency ordering. |
| [`static System.Collections.Generic.IReadOnlyList<Inno.Plugins.Authoring.PluginCandidate> Inno.Plugins.Authoring.PluginSourceService.GetActivatableCandidates(Inno.Plugins.Authoring.PluginScanResult result)`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceService.cs#L123) | Gets validated candidates in complete dependency order. |

## 项目依赖

- [Inno.References](../references/Inno.References.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Reload](../extensibility/Inno.Extensibility.Reload.md)：公开引用边界由实际签名核对。
- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.IO](../core/Inno.Core.IO.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Collections](../core/Inno.Core.Collections.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Settings](../core/Inno.Core.Settings.md)：公开引用边界由实际签名核对。
- [Inno.Assets](../assets/Inno.Assets.md)：公开引用边界由实际签名核对。
- [Inno.Assets.Pipeline](../assets/Inno.Assets.Pipeline.md)：公开引用边界由实际签名核对。
- [Inno.Plugins](Inno.Plugins.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
