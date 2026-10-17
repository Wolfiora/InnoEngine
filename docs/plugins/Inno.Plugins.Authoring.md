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

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Plugins.Authoring.PluginCandidate`

<!-- api: T:Inno.Plugins.Authoring.PluginCandidate 175e7396d23c1d5bef04c365c2e3dd421dae4aaa0900a3d9a64c3d331445a3bd -->
<!-- api: P:Inno.Plugins.Authoring.PluginCandidate.sourcePath 0f5956f44b8c1128cf8e938ae10497c8984ae9ae0568e8a6a478410b2d92bd33 -->
<!-- api: P:Inno.Plugins.Authoring.PluginCandidate.sourceKind 3fb545bfdb735ac12b7aa9a58df5ed2f20ca99712ba0c4c8099bb5342cefe790 -->
<!-- api: P:Inno.Plugins.Authoring.PluginCandidate.contentHash 92bee60ed3ed316cead2f786f27e5833053457fd2cbf5ed5cfbd17026c6e6afa -->
<!-- api: P:Inno.Plugins.Authoring.PluginCandidate.manifest d6becd63163d3924a0cfbbc0e273b77337fab5014f426d51453d8aa75e4ac132 -->
<!-- api: P:Inno.Plugins.Authoring.PluginCandidate.sourceMount 0f9bfe2c35d2b5f71c0b6ef551a181a95dd4413d10ec1463aa5367cfda0ec264 -->
<!-- api: P:Inno.Plugins.Authoring.PluginCandidate.containsCode 9133c81b7e683232ed5e683960d0a9e1c8de40085658a6c184ab879b48d8bc54 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-73a85958b135a7b1"></a>[`public bool Inno.Plugins.Authoring.PluginCandidate.containsCode { get; }`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L117) | Gets whether source code exists in this Plugin. | — | — |
| <a id="api-9c0c64209f4bf97c"></a>[`public string Inno.Plugins.Authoring.PluginCandidate.contentHash { get; }`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L102) | Gets the deterministic complete source-content hash. | — | — |
| <a id="api-f5b3a7316e7a84cd"></a>[`public Inno.Plugins.PluginManifest Inno.Plugins.Authoring.PluginCandidate.manifest { get; }`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L107) | Gets a detached copy of the validated native manifest, including its nested contribution payloads. | — | — |
| <a id="api-c91aa32cb283df24"></a>[`public Inno.Plugins.Authoring.PluginSourceKind Inno.Plugins.Authoring.PluginCandidate.sourceKind { get; }`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L97) | Gets the physical source container kind. | — | — |
| <a id="api-e8582b74e7e303e0"></a>[`public Inno.Assets.Pipeline.AssetSourceMount Inno.Plugins.Authoring.PluginCandidate.sourceMount { get; }`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L112) | Gets the isolated read-only asset source mount. | — | — |
| <a id="api-ddb5da65227d7f94"></a>[`public string Inno.Plugins.Authoring.PluginCandidate.sourcePath { get; }`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L92) | Gets the installed .iplugin package path. | — | — |
| <a id="api-f2418739f542aa1b"></a>[`public sealed class Inno.Plugins.Authoring.PluginCandidate`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L70) | Describes one validated installed Plugin source candidate. | — | — |

### `Inno.Plugins.Authoring.PluginDiagnostic`

<!-- api: T:Inno.Plugins.Authoring.PluginDiagnostic 92cd68cc96a6a071f40c92caf445f275780696250cf097d25410749baba4ee11 -->
<!-- api: M:Inno.Plugins.Authoring.PluginDiagnostic.#ctor(System.String,System.String) 32961cbdc057e5fdef583c3cecc621c2245487d34f39aafa63912f83762ae58f -->
<!-- api: P:Inno.Plugins.Authoring.PluginDiagnostic.sourcePath 45c1b348509ca10cc59f9c7b08434b0a61d80ccf5e8cb94135f302930968a512 -->
<!-- api: P:Inno.Plugins.Authoring.PluginDiagnostic.message 6b8ef951121d3d823802a0dd1583d9cf080808a68278536b406264195606f82f -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-00da7a5ba39278e1"></a>[`public Inno.Plugins.Authoring.PluginDiagnostic.PluginDiagnostic(string sourcePath, string message)`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L144) | Creates a Plugin source diagnostic.<br>`sourcePath`：Related .iplugin package path or invalid installation entry.<br>`message`：Actionable problem description. | — | — |
| <a id="api-6419e721241a44d1"></a>[`public string Inno.Plugins.Authoring.PluginDiagnostic.message { get; }`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L160) | Gets the actionable problem description. | — | — |
| <a id="api-6036d9760ea9acad"></a>[`public string Inno.Plugins.Authoring.PluginDiagnostic.sourcePath { get; }`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L155) | Gets the related .iplugin package path or invalid installation entry. | — | — |
| <a id="api-21b64646e1c7f132"></a>[`public sealed class Inno.Plugins.Authoring.PluginDiagnostic`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L133) | Reports one Plugin discovery, validation, or dependency problem. | — | — |

### `Inno.Plugins.Authoring.PluginEnvironment`

<!-- api: T:Inno.Plugins.Authoring.PluginEnvironment 44a2d428e5bb1bf7e72389edd5b2c0ffc8fbca80054a4e5b930b03ba1377252a -->
<!-- api: E:Inno.Plugins.Authoring.PluginEnvironment.ActivationCandidateChanged a20ccdf05eccc89df77bbc25488e424242bb8c65b340946412f5acc44a36539c -->
<!-- api: P:Inno.Plugins.Authoring.PluginEnvironment.isInitialized 80a02c33ab12d62a31a043562d165b123a12d7c9ea8ee250583b4f560c8ec738 -->
<!-- api: P:Inno.Plugins.Authoring.PluginEnvironment.activePlugins 53f7f93ae00a039289a84ad8a8aa613a8d1a364b2a4c28d9c3382d411cc9aa0a -->
<!-- api: P:Inno.Plugins.Authoring.PluginEnvironment.revision 8b1aa8f01ef6d5aa2e6366a6f1f1516ef4a33371dbf0bb2e1adba537d0156cb5 -->
<!-- api: P:Inno.Plugins.Authoring.PluginEnvironment.discovery 42c0628c4ec5cd0440b1d184c243760d5c5c2b367f012f7d1a3d9a6934515fad -->
<!-- api: E:Inno.Plugins.Authoring.PluginEnvironment.Changed e995a16989581bf8ec1c080ae107012fe4179436ac77bf280fee3aef87c66c58 -->
<!-- api: M:Inno.Plugins.Authoring.PluginEnvironment.#ctor(Inno.Assets.Pipeline.AssetPipeline,Inno.Core.Settings.ProjectSettingsStore,Inno.Core.Serialization.SerializationRegistry,System.String,System.String,Inno.Plugins.Authoring.PluginScanResult,Inno.Extensibility.Reload.GenerationCoordinator) f061843ad229378d52676049742db9acc9660e3c4ef94162dece2b1ab1f16aa1 -->
<!-- api: M:Inno.Plugins.Authoring.PluginEnvironment.Update 69d9131317071eec2554285c2b3fa694452e7aecb9243057191e7fcbd58b739e -->
<!-- api: M:Inno.Plugins.Authoring.PluginEnvironment.Refresh c0e2780991618362e24042061e83b1289db3bb66aa887fbe414b2a7f69d6bb38 -->
<!-- api: M:Inno.Plugins.Authoring.PluginEnvironment.CreateReloadChange 4b80a548a1b18e835681c5253ffdb1d8b7eacff63a20d0da24cf0da628d91f72 -->
<!-- api: M:Inno.Plugins.Authoring.PluginEnvironment.ApplyPendingChange f77ec995fc8aefb3c389165338a9982f61dfa85da46d9d3a8719041e91f715dc -->
<!-- api: P:Inno.Plugins.Authoring.PluginEnvironment.hasPendingActivation 155500c11219d4e01f3014f677886e886114c5794e560bc05dca82a08f728a57 -->
<!-- api: P:Inno.Plugins.Authoring.PluginEnvironment.compilationAssets 65171ca61f7691391922b01831f0a07c2e21cc56d0bc8a81c58c55c9cb80cefe -->
<!-- api: P:Inno.Plugins.Authoring.PluginEnvironment.compilationPlugins 8581ea8b4436233024223788746d35c9918a297e2d4bdf923216c5ca9f8f4666 -->
<!-- api: M:Inno.Plugins.Authoring.PluginEnvironment.TryGetCompilationPlugin(Inno.Assets.AssetSourceId,Inno.Plugins.Authoring.PluginCandidate@) d46ab73bea1f0d87eb22a9a5c641bf76eccb87a3998e7cc373301b3488a6f8dc -->
<!-- api: M:Inno.Plugins.Authoring.PluginEnvironment.TryGet(Inno.Assets.AssetSourceId,Inno.Plugins.Authoring.PluginCandidate@) 0d4b6ddbc06be794e8e76e7e7c55ad3015c2014a4c999eed9b93347698df36d9 -->
<!-- api: M:Inno.Plugins.Authoring.PluginEnvironment.ActivatePending ae3845a4f072dc721cc3ff18eefdf1f2589d5a75ce48d7d7f18b88a51821b0a7 -->
<!-- api: M:Inno.Plugins.Authoring.PluginEnvironment.CommitPending 5f0df23000edd614507c66403f6997bee7b86e70cb3e1ace0be7c8eeee9161d0 -->
<!-- api: M:Inno.Plugins.Authoring.PluginEnvironment.RollbackPending 91e6d6cd27efaa0f054051f01efb4f57ef67ce7aeb2e5612afc828e06093b440 -->
<!-- api: M:Inno.Plugins.Authoring.PluginEnvironment.Dispose 0a54f9adc3e397d4927ac1fa1d55bfe89f48f393d0e143a753c151f83ea6df3c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9749e86e209dc128"></a>[`public System.Action? Inno.Plugins.Authoring.PluginEnvironment.ActivationCandidateChanged`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L51) | Occurs after a validated Plugin generation is staged for script compilation but before it becomes active. | — | — |
| <a id="api-bdbbd7e7d3bc54d4"></a>[`public System.Action? Inno.Plugins.Authoring.PluginEnvironment.Changed`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L84) | Occurs after discovery diagnostics or the active Plugin generation changes. | — | — |
| <a id="api-eca3d43295a04e99"></a>[`public Inno.Plugins.Authoring.PluginEnvironment.PluginEnvironment(Inno.Assets.Pipeline.AssetPipeline assets, Inno.Core.Settings.ProjectSettingsStore settings, Inno.Core.Serialization.SerializationRegistry serialization, string pluginRoot, string libraryRoot, Inno.Plugins.Authoring.PluginScanResult initialScan, Inno.Extensibility.Reload.GenerationCoordinator generations)`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L115) | Creates automatic Plugin discovery around one asset pipeline and project settings store.<br>`assets`：The authoring asset pipeline that owns Plugin source mounts.<br>`settings`：The project settings store that composes active Plugin contributions.<br>`serialization`：The serialization registry used to validate installed Plugin manifests.<br>`pluginRoot`：Project Plugins directory.<br>`libraryRoot`：Project rebuildable Library directory.<br>`initialScan`：Scan already used to create the initial Asset mount generation.<br>`generations`：The shared Host gate for publication, Play, Build and unload verification. | — | — |
| <a id="api-ca20fd6a37dee8d9"></a>[`public void Inno.Plugins.Authoring.PluginEnvironment.ActivatePending()`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L391) | Provisionally publishes at the shared generation safety point; the owning change must roll back failures. | — | — |
| <a id="api-3a63e7b54fac61af"></a>[`public void Inno.Plugins.Authoring.PluginEnvironment.ApplyPendingChange()`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L304) | Commits a non-code availability change using the same generation gate and recovery phases as script reload. | — | `System.InvalidOperationException`：The Host is not ready for a new generation. |
| <a id="api-66e0d2eb2ed77d19"></a>[`public void Inno.Plugins.Authoring.PluginEnvironment.CommitPending()`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L410) | Commits the pending mount generation after scripts, assets, settings, and registries activate. | — | — |
| <a id="api-b91dfaccac90065e"></a>[`public Inno.Extensibility.Reload.IGenerationChange Inno.Plugins.Authoring.PluginEnvironment.CreateReloadChange()`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L293) | Captures Plugin availability, Asset mounts and effective settings as one shared recovery participant.<br>返回：A five-phase change for the Host assembly publication, preserving Missing semantic contributions. | — | — |
| <a id="api-dd565ef54bfe45b9"></a>[`public void Inno.Plugins.Authoring.PluginEnvironment.Dispose()`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L462) | Stops automatic discovery without deleting Plugin sources or rebuildable extraction caches. | — | — |
| <a id="api-335cb36e9026b48d"></a>[`public bool Inno.Plugins.Authoring.PluginEnvironment.Refresh()`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L183) | Forces validation and prepares or commits one atomic Plugin availability generation.<br>返回：true when a different mount generation was committed immediately or staged for code reload; otherwise, false. | — | — |
| <a id="api-c7371c41a4cfc7aa"></a>[`public void Inno.Plugins.Authoring.PluginEnvironment.RollbackPending()`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L435) | Restores the complete last-good mount, catalog, and settings contributor generation. | — | — |
| <a id="api-5c8d3cfa2d89f779"></a>[`public bool Inno.Plugins.Authoring.PluginEnvironment.TryGet(Inno.Assets.AssetSourceId source, out Inno.Plugins.Authoring.PluginCandidate? plugin)`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L382) | Tries to resolve the active Plugin that owns an asset source.<br>返回：true when an active Plugin owns the source.<br>`source`：The isolated asset source identity.<br>`plugin`：Receives the active Plugin candidate when the source is owned by one. | — | — |
| <a id="api-2b2d836a7ada13fa"></a>[`public bool Inno.Plugins.Authoring.PluginEnvironment.TryGetCompilationPlugin(Inno.Assets.AssetSourceId source, out Inno.Plugins.Authoring.PluginCandidate? plugin)`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L362) | Resolves a Plugin owned by the active or pending script-compilation generation.<br>返回：True when the compilation generation owns the source.<br>`source`：Plugin source mount ID.<br>`plugin`：Resolved Plugin candidate. | — | — |
| <a id="api-1e0ba017969a546d"></a>[`public void Inno.Plugins.Authoring.PluginEnvironment.Update()`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L155) | Processes debounced Plugin source notifications and completed background reconciliation. | — | — |
| <a id="api-170da364a2895a71"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Plugins.Authoring.PluginCandidate> Inno.Plugins.Authoring.PluginEnvironment.activePlugins { get; }`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L69) | Gets the dependency-ordered active Plugin generation. | — | — |
| <a id="api-b8ad6856a8caf048"></a>[`public Inno.Assets.Pipeline.AssetSourceMountTransaction? Inno.Plugins.Authoring.PluginEnvironment.compilationAssets { get; }`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L323) | Gets the isolated Asset candidate used by the next Plugin script compilation. | — | — |
| <a id="api-25303ca91bbb7b9a"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Plugins.Authoring.PluginCandidate> Inno.Plugins.Authoring.PluginEnvironment.compilationPlugins { get; }`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L336) | Gets the Plugin candidates visible to the next script compilation. | — | — |
| <a id="api-a54631d9425a3e9a"></a>[`public Inno.Plugins.Authoring.PluginScanResult Inno.Plugins.Authoring.PluginEnvironment.discovery { get; }`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L79) | Gets the latest installed Plugin discovery and isolated diagnostics. | — | — |
| <a id="api-96474e5b42b7c0bb"></a>[`public bool Inno.Plugins.Authoring.PluginEnvironment.hasPendingActivation { get; }`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L310) | Gets whether a source-mount candidate is waiting for successful script generation activation. | — | — |
| <a id="api-ca67c563bf219b2e"></a>[`public bool Inno.Plugins.Authoring.PluginEnvironment.isInitialized { get; }`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L57) | Gets whether project Plugin management is initialized. | — | — |
| <a id="api-635f142f92f1af98"></a>[`public long Inno.Plugins.Authoring.PluginEnvironment.revision { get; }`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L74) | Gets the monotonic identity of the active Plugin generation. | — | — |
| <a id="api-1a2897a392751b15"></a>[`public sealed class Inno.Plugins.Authoring.PluginEnvironment : System.IDisposable`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginEnvironment.cs#L23) | Owns automatic installed Plugin source discovery and atomically publishes Asset mounts, settings contributors, and the active Plugin catalog on the Asset Database owner thread. | — | — |

### `Inno.Plugins.Authoring.PluginScanResult`

<!-- api: T:Inno.Plugins.Authoring.PluginScanResult a555a4fcb67421a0d8bd36b3a4e46cd3cdde3bcc5eedefeb63991c7123815807 -->
<!-- api: P:Inno.Plugins.Authoring.PluginScanResult.candidates 1c84f82b2e80ac3c85d0fe118c9f7c7d88dd46275f48908a8a5278036fc46574 -->
<!-- api: P:Inno.Plugins.Authoring.PluginScanResult.diagnostics 8b1479e53012e1a7794f5dc6461b709c9f55c0ea48ed8a8e32d70a9ef524688a -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8e49e8533ace5142"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Plugins.Authoring.PluginCandidate> Inno.Plugins.Authoring.PluginScanResult.candidates { get; }`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L179) | Gets dependency-ordered valid candidates. | — | — |
| <a id="api-90f3af7bc8255981"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Plugins.Authoring.PluginDiagnostic> Inno.Plugins.Authoring.PluginScanResult.diagnostics { get; }`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L184) | Gets isolated diagnostics for rejected candidates. | — | — |
| <a id="api-2f8d5da8ac401f9a"></a>[`public sealed class Inno.Plugins.Authoring.PluginScanResult`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L166) | Contains a complete immutable Plugin discovery result. | — | — |

### `Inno.Plugins.Authoring.PluginSourceKind`

<!-- api: T:Inno.Plugins.Authoring.PluginSourceKind 113aea6206b51047e21193d7ea19efecf8c2f56ab069e76fd0f0869e340406ef -->
<!-- api: F:Inno.Plugins.Authoring.PluginSourceKind.Package 68e73b7493105b8bc83a381c5aecf333ff2091c764bcdc029281af3b59703c8c -->
<!-- api: F:Inno.Plugins.Authoring.PluginSourceKind.EmbeddedPackage ac3655b2eaca3a3cf47bd52d72c19dd5e67af1fa053ebddd6f1ac0ddeed8806f -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1b2412bd05fedfb1"></a>[`Inno.Plugins.Authoring.PluginSourceKind.EmbeddedPackage = 1`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L23) | The Plugin is embedded as one complete .iplugin dependency package. | — | — |
| <a id="api-c4dd4f471538574d"></a>[`Inno.Plugins.Authoring.PluginSourceKind.Package = 0`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L18) | The Plugin is installed as one top-level .iplugin package. | — | — |
| <a id="api-aec146e1aad86277"></a>[`public enum Inno.Plugins.Authoring.PluginSourceKind : int`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L13) | Identifies how an .iplugin package participates in one installation. | — | — |

### `Inno.Plugins.Authoring.PluginSourceLimits`

<!-- api: T:Inno.Plugins.Authoring.PluginSourceLimits 242056ecf3097027c622bdb123806cab94d8699861ec24ad09d1d3919ab202ca -->
<!-- api: P:Inno.Plugins.Authoring.PluginSourceLimits.defaults 0112e6a901950994821ae24f2306dbd7be843cc276aa3ba1206b2e84cd7ea0b8 -->
<!-- api: P:Inno.Plugins.Authoring.PluginSourceLimits.maximumEntryCount 43d36cfdf0422529ba7fb38b73f62cb05b2eb99b469a59f4f626a8d752d879a6 -->
<!-- api: P:Inno.Plugins.Authoring.PluginSourceLimits.maximumFileBytes 0908d40c4d5efd300c19347443856be8fbebf7f04a24f983bbc8603bcaa5b11f -->
<!-- api: P:Inno.Plugins.Authoring.PluginSourceLimits.maximumTotalBytes 9880e8fdf479b87837c7c22f05bd0149bbc0ebc35b91b1a42e080c1c871b9667 -->
<!-- api: P:Inno.Plugins.Authoring.PluginSourceLimits.maximumCompressionRatio e735a9f64dcc5f6d5c0267710e196e314fc5a7e7919a66774e7c9b2977f58bb0 -->
<!-- api: P:Inno.Plugins.Authoring.PluginSourceLimits.maximumEmbeddedPluginCount d346dea58af201ee9b3c20dc409a05656ebe36ff3a7eae244b3c4c59a7bd74d1 -->
<!-- api: P:Inno.Plugins.Authoring.PluginSourceLimits.maximumEmbeddedDepth 6aef304944c01ee27c073a2a2aea4aed5fbc2e4a6ad93b0ca3ac8d6912212b16 -->
<!-- api: M:Inno.Plugins.Authoring.PluginSourceLimits.#ctor bda257077690a1fb09608b1ea85eb514abb23cd68a6b0f90c866916dcec584f0 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-caa16182a9a64144"></a>[`public Inno.Plugins.Authoring.PluginSourceLimits.PluginSourceLimits()`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L29) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-d1cfadff9316d0f6"></a>[`public static Inno.Plugins.Authoring.PluginSourceLimits Inno.Plugins.Authoring.PluginSourceLimits.defaults { get; }`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L34) | Gets default conservative local Plugin limits. | — | — |
| <a id="api-6deb1fc90c923035"></a>[`public double Inno.Plugins.Authoring.PluginSourceLimits.maximumCompressionRatio { get; init; }`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L54) | Gets or initializes the maximum accepted archive uncompressed-to-compressed ratio. | — | — |
| <a id="api-628bb99be1d7ab22"></a>[`public int Inno.Plugins.Authoring.PluginSourceLimits.maximumEmbeddedDepth { get; init; }`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L64) | Gets or initializes the maximum nested embedded dependency depth. | — | — |
| <a id="api-d998899eea6facb4"></a>[`public int Inno.Plugins.Authoring.PluginSourceLimits.maximumEmbeddedPluginCount { get; init; }`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L59) | Gets or initializes the maximum number of embedded dependency packages in one installation. | — | — |
| <a id="api-527fc552c49a7861"></a>[`public int Inno.Plugins.Authoring.PluginSourceLimits.maximumEntryCount { get; init; }`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L39) | Gets or initializes the maximum number of source entries. | — | — |
| <a id="api-9e689d653fd92e5a"></a>[`public long Inno.Plugins.Authoring.PluginSourceLimits.maximumFileBytes { get; init; }`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L44) | Gets or initializes the maximum size of one source file. | — | — |
| <a id="api-0c7012a7ab61ae3f"></a>[`public long Inno.Plugins.Authoring.PluginSourceLimits.maximumTotalBytes { get; init; }`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L49) | Gets or initializes the maximum total source size. | — | — |
| <a id="api-8d4a4c72a0d86b5e"></a>[`public sealed class Inno.Plugins.Authoring.PluginSourceLimits`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceModels.cs#L29) | Controls bounded validation and archive extraction for installed Plugin packages. | — | — |

### `Inno.Plugins.Authoring.PluginSourceService`

<!-- api: T:Inno.Plugins.Authoring.PluginSourceService aa7b6a7c9ff4d929129533b74a8281504791357c84b10b81e33bdae005a3d4cd -->
<!-- api: M:Inno.Plugins.Authoring.PluginSourceService.#ctor(Inno.Core.Serialization.SerializationRegistry,System.String,System.String,Inno.Plugins.Authoring.PluginSourceLimits) afc1cdc1e1cad1ca1588d62bab18876aa8b62b574f868a2d273893ebe70d4ce7 -->
<!-- api: M:Inno.Plugins.Authoring.PluginSourceService.Scan 8edc5839f8d149883c99f0ce4f1652baf626c4870fc3c1accbaf05f621d9441f -->
<!-- api: M:Inno.Plugins.Authoring.PluginSourceService.GetActivatableMounts(Inno.Plugins.Authoring.PluginScanResult) 60f8356e69ef41785c0672dfce5eb5e956692665e1c0fb203adfe92d4eef474e -->
<!-- api: M:Inno.Plugins.Authoring.PluginSourceService.GetActivatableCandidates(Inno.Plugins.Authoring.PluginScanResult) 40524cd60a7a0aaaf3b20a77e72fe371e229b049f381b4de1083fe9cde9449eb -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1d9157caa001561a"></a>[`public Inno.Plugins.Authoring.PluginSourceService.PluginSourceService(Inno.Core.Serialization.SerializationRegistry serialization, string pluginRoot, string libraryRoot, Inno.Plugins.Authoring.PluginSourceLimits? limits = null)`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceService.cs#L52) | Creates a local Plugin source service.<br>`serialization`：The serialization registry used to read the current Plugin manifest contract.<br>`pluginRoot`：Project Plugins directory containing .iplugin packages.<br>`libraryRoot`：Project rebuildable Library directory.<br>`limits`：Optional bounded source limits. | — | — |
| <a id="api-41925903972daf90"></a>[`public static System.Collections.Generic.IReadOnlyList<Inno.Plugins.Authoring.PluginCandidate> Inno.Plugins.Authoring.PluginSourceService.GetActivatableCandidates(Inno.Plugins.Authoring.PluginScanResult result)`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceService.cs#L123) | Gets validated candidates in complete dependency order.<br>返回：Dependency-ordered candidates suitable for one activation transaction.<br>`result`：Complete scan result. | — | — |
| <a id="api-da8aa02b4f1f2bf6"></a>[`public static System.Collections.Generic.IReadOnlyList<Inno.Assets.Pipeline.AssetSourceMount> Inno.Plugins.Authoring.PluginSourceService.GetActivatableMounts(Inno.Plugins.Authoring.PluginScanResult result)`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceService.cs#L111) | Gets read-only mounts that passed validation and dependency ordering.<br>返回：Dependency-ordered mount snapshot suitable for the common AssetPipeline.<br>`result`：Complete scan result. | — | — |
| <a id="api-0a7a9b5c6a413f07"></a>[`public Inno.Plugins.Authoring.PluginScanResult Inno.Plugins.Authoring.PluginSourceService.Scan()`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceService.cs#L75) | Scans every installed .iplugin package into one isolated candidate snapshot.<br>返回：Dependency-ordered candidates plus isolated diagnostics. | — | — |
| <a id="api-4cb5a801deec190d"></a>[`public sealed class Inno.Plugins.Authoring.PluginSourceService`](../../src/runtime/plugins/Inno.Plugins.Authoring/PluginSourceService.cs#L21) | Discovers, validates, mounts, and dependency-orders local .iplugin packages. | — | — |

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
