# Inno.Scripting.Reload

## 项目创作代际

同一项目的 GameScripts 与 EditorScripts 共同构成一个创作代际。即使只修改 Editor 源码，也一起经过候选、Scene/Settings 状态恢复、原子激活与严格卸载验证。原因是 Editor 中的泛型扩展闭合于项目 runtime 类型时，CLR loader allocator 可以形成双向依赖；单向程序集引用并不保证两个 collectible context 可独立卸载。编译仍可复用未修改的 GameScripts 产物，不要求重编全部源码。Plugin 按公开模块依赖图处理，领域扩展使用同一候选和退休协议。

[Scripting 索引](README.md) · [Compiler](Inno.Scripting.Compiler.md) · [Extensibility](../extensibility/Inno.Extensibility.Modules.md) · [目标 reload 强制标准](../architecture/IDENTITY_REFERENCE_RELOAD_STANDARD.md)

## 公开 API

- `ScriptReloadHost`：把成功编译结果准备成 Module/Type/feature candidates；Plugin availability 改变而无法产出替代编译时，准备显式的 module retirement candidate；两者都只在安全点原子激活。
- `ScriptReloadOptions`：reload boundary 和诊断策略。
- `IScriptReloadCoordinator`：Editor/Scene/Rendering 等生产 feature 的事务协调 contract。

`ScriptReloadHost` 构造时要求组合入口提供 `Func<ScriptModuleDeployment, IModuleSource>`。工厂只为不可变产物选择模块来源，不能自行激活或发布；共同 host 仍负责候选事务、依赖选择、状态迁移和退休检查。Editor/Build 的桌面组合使用 DotNet Adapter，Compiler 与 Reload 程序集不引用具体 Adapter。

模块激活事务不重新编译源文件：编译先产出冻结候选，再进入激活。普通 Project 编译失败没有 candidate，active generation 保持不变；但 active Plugin 被删除、结构失效或更新失败时，安装集合变化本身是有效事实，不能因为依赖脚本编译失败而保留旧 Plugin。Host 会以 active/candidate Plugin ID 与 content hash 计算 retired Plugin modules，再沿 `upstreamModuleNames` 取得完整反向依赖闭包，原子移除 Plugin、Runtime Scripts 和 Editor Scripts。Scene participant 在同一事务内把已退休类型变成保留 Stable ID 与状态的 Missing，并在类型返回时恢复。

Plugin 文件系统删除触发的第一次自动 reload 就必须完成上述切换，不要求用户再次执行 Reload Plugins。成功编译出的每个 replacement request 使用编译快照给出的显式依赖清单；空清单不会重新绑定仍处于 previous generation 的 Plugin。提交后 unload verification 按帧协作式等待 CLR 完成 collectible ALC 回收。只有超时后仍真实可达时才发布唯一的 `INNO-ALC-UNLOAD` Diagnostic；Console 不再额外写入一条内容重复的普通 Error log。超时后当前 Host 必须重启，不能在 Faulted 进程里开始新的 reload 来清除此诊断。

当前 verification 是 Success-or-Exception barrier：旧 ALC 未被 GC 确认回收前
operation 不成功，达到失败阈值后进入 Faulted，并阻止重叠 reload、Play、Build 与 Export。成功 commit、rollback
candidate、Plugin uninstall、显式 unload 和 shutdown 都受同一规则约束。完整要求见
[Identity、可恢复引用与热重载强制标准](../architecture/IDENTITY_REFERENCE_RELOAD_STANDARD.md)。

只有 participant Prepare/Activate、Scene 状态迁移或外部 generation 同步本身失败时，事务才 rollback 到仍完整存在的 immutable previous snapshot。Complete 清理失败不能撤销已经提交的 generation，必须 Fault 共享 gate；不能只记录诊断后继续。调用者负责 Dispose host，释放 catalog registrations 与 retiring generation lease。

`IScriptReloadCoordinator.Execute(AssemblyReloadSession, IGenerationChange? externalChange = null)` 用统一五阶段 owner 替代两段无所有权 callback。Prepare 在 gate 内执行；参与者 Capture 失败也会回滚已尝试的 prepare，不对未准备的 participant 执行恢复。外部内容 Apply 在 Editor domain 恢复前完成，rollback 先恢复旧 assembly/catalog 与外部内容，再恢复 Editor 属性。ScriptReloadHost 使用 PluginEnvironment.CreateReloadChange，不在末尾另做非原子的 CommitPending。

关闭 ScriptReloadHost 时先撤销 observation、取消并等待编译，然后完成共享 GenerationCoordinator 的上一批 GC barrier，才提交活动模块卸载。退出不是绕过 AwaitingCollection 的特殊入口；Faulted/timeout 仍会阻止新的卸载事务。

自动编译只排除本 ScriptReloadHost 同步发布候选时产生的 SourceMountsChanged 通知回声。Assembly Catalog 会在每次脚本发布时重新发布隔离 Asset Catalog，这不是一次新的源文件编辑，不能由此排队下一轮 reload。真实文件变化、显式编译请求和外部 source-mount 发布仍然正常入队；不关闭 watcher、不取消全局通知，也不跳过 GC barrier。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Scripting.Reload.IScriptReloadCoordinator`

<!-- api: T:Inno.Scripting.Reload.IScriptReloadCoordinator 62df8dadeb67a0243ab07966dd023f072d1a3aa4de96b2471b3b66fa50ca9081 -->
<!-- api: M:Inno.Scripting.Reload.IScriptReloadCoordinator.Execute(Inno.Extensibility.Modules.AssemblyReloadSession,Inno.Extensibility.Reload.IGenerationChange) 9f0acebffa5543c959ecc39b64cc72869fef94b27559bd1b3c4c9e7f363b9f8d -->
<!-- api: M:Inno.Scripting.Reload.IScriptReloadCoordinator.RefreshDiagnostics 6188f45a72e9dcd8c7640b31630ecd7636febf4fe549e2e3ba330a6159fcbad6 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8a4e6337911d127a"></a>[`Inno.Extensibility.Reload.IAssemblyUnloadProbe Inno.Scripting.Reload.IScriptReloadCoordinator.Execute(Inno.Extensibility.Modules.AssemblyReloadSession reload, Inno.Extensibility.Reload.IGenerationChange? externalChange = null)`](../../src/runtime/scripting/Inno.Scripting.Reload/IScriptReloadCoordinator.cs#L24) | Commits a prepared assembly reload together with dependent host state.<br>返回：A monitor observing cooperative unload of assemblies retired by the committed generation.<br>`reload`：The prepared assembly reload session to activate and complete.<br>`externalChange`：Optional five-phase content change; captured before publication and restored before domain property recovery. | — | — |
| <a id="api-9b4acdb72dc1fb4c"></a>[`void Inno.Scripting.Reload.IScriptReloadCoordinator.RefreshDiagnostics()`](../../src/runtime/scripting/Inno.Scripting.Reload/IScriptReloadCoordinator.cs#L32) | Requests diagnostics derived from the active host generation to be republished. | — | — |
| <a id="api-19c466a403b1e0eb"></a>[`public interface Inno.Scripting.Reload.IScriptReloadCoordinator`](../../src/runtime/scripting/Inno.Scripting.Reload/IScriptReloadCoordinator.cs#L10) | Coordinates host-owned state with an atomic script assembly generation transition. | — | — |

### `Inno.Scripting.Reload.ScriptReloadHost`

<!-- api: T:Inno.Scripting.Reload.ScriptReloadHost f7f9e6f9a8b72d9b87ced665ffffa03729bc6b4d611124e67cc95d0e1482e1d9 -->
<!-- api: M:Inno.Scripting.Reload.ScriptReloadHost.#ctor(Inno.Scripting.Reload.ScriptReloadOptions,Inno.Scripting.Compiler.ScriptCompiler,Inno.Assets.Pipeline.AssetPipeline,Inno.Plugins.Authoring.PluginEnvironment,Inno.Extensibility.Modules.ModuleHost,Inno.Core.Settings.ProjectSettingsStore,Inno.Scripting.Reload.IScriptReloadCoordinator,System.Func{Inno.Scripting.Compiler.ScriptModuleDeployment,Inno.Extensibility.Modules.IModuleSource}) ace01430c6f226c6e14477095fb5cfdb56a932b6ac81720154b6473359de9bf6 -->
<!-- api: P:Inno.Scripting.Reload.ScriptReloadHost.isCompiling 7b77f9acd70f6174f0d75832135a253d30f85f43cec0177735cf41efb78dc8f0 -->
<!-- api: P:Inno.Scripting.Reload.ScriptReloadHost.isCompilationPending a83aa7eaabbe69ef81fbd95e2b3451b154f4594171e164c1fb3ab6e7f54a7163 -->
<!-- api: P:Inno.Scripting.Reload.ScriptReloadHost.compilationProgress de1a0b65de030e2c5f87056e994d2526e8a7884df826735e979563a8a1e3fca0 -->
<!-- api: P:Inno.Scripting.Reload.ScriptReloadHost.compilationStatus ddead7f0a6f299bd1c85bba9e609da346fc5d594221c9140c9494cc82bbacedd -->
<!-- api: P:Inno.Scripting.Reload.ScriptReloadHost.compilationElapsed ac141cb10c477e4f2c99a2df08d195bb4891912cb0b0e24f744147a794861e2b -->
<!-- api: P:Inno.Scripting.Reload.ScriptReloadHost.isCompilationTakingLong 7047ddd0e9147dcb27ba019b98fff8bdd61ab33e335fb358bd27db3987fef89f -->
<!-- api: P:Inno.Scripting.Reload.ScriptReloadHost.lastCompilation d232189383ddea05046c1c8d9a9702e879e74ae4e31e00521ceeb6fc7c463acb -->
<!-- api: P:Inno.Scripting.Reload.ScriptReloadHost.isUnloadVerificationPending eadfe071683871ccc6e3238b50955fff749b32e42890b0142f738912d439e2a0 -->
<!-- api: P:Inno.Scripting.Reload.ScriptReloadHost.isFaulted eb47d6ac19582a159a1a020a25f5abed3cddaf1ae944985d204fabd63805b3a3 -->
<!-- api: M:Inno.Scripting.Reload.ScriptReloadHost.Start e3610960bd7d5ad3fabdb50906c5a64e1ec3475663b3c88a21b797667808d804 -->
<!-- api: M:Inno.Scripting.Reload.ScriptReloadHost.CancelCompilation 7f315d5db6aa9d7e6de4defc7767be9b1c6b78b3a4b5e5ab0b45e010db95cb6b -->
<!-- api: M:Inno.Scripting.Reload.ScriptReloadHost.RecompileScripting 5254d4cc2dcf2750dfd2dfe6d3b95be50ec16051ef781d9e735186f6648ede9b -->
<!-- api: M:Inno.Scripting.Reload.ScriptReloadHost.ReloadScripting dad84f2718a0f27df3652b1ef2246b862571522f51593002420493ac1cf34df8 -->
<!-- api: M:Inno.Scripting.Reload.ScriptReloadHost.ReloadPlugins b74ab53e73f3fe141431bb56a287649b5c440cc90b3deabbe5a5b20589016094 -->
<!-- api: M:Inno.Scripting.Reload.ScriptReloadHost.TryCompilePending(System.Threading.Tasks.Task{Inno.Scripting.Compiler.ScriptCompilationResult}@) 2034b6958e3b2fe20f067ef054978ecbb9be13cc62cd5f8ac1f2914afaabccd9 -->
<!-- api: M:Inno.Scripting.Reload.ScriptReloadHost.ApplyPendingReload f97703983ea27376b5c4fafd6f072d14af15933324673f1078d77cd29c32524e -->
<!-- api: M:Inno.Scripting.Reload.ScriptReloadHost.GenerateProjectFiles 515fa12c27e9ffe3656b42e284878dd89bb181f069f0af9142a64fdf7e859922 -->
<!-- api: M:Inno.Scripting.Reload.ScriptReloadHost.Dispose 7d04652880792814591fa3cf60e49919ec901dec0a93e46dfc1bff24d405a862 -->
<!-- api: M:Inno.Scripting.Reload.ScriptReloadHost.AdvanceUnloadVerification(System.Exception@) 99d8c5abda7a53296c72735c570bcda98d71ef9d4426ec3c849c7e7030e54327 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-565f8678e79922b3"></a>[`public Inno.Scripting.Reload.ScriptReloadHost.ScriptReloadHost(Inno.Scripting.Reload.ScriptReloadOptions options, Inno.Scripting.Compiler.ScriptCompiler compiler, Inno.Assets.Pipeline.AssetPipeline assets, Inno.Plugins.Authoring.PluginEnvironment plugins, Inno.Extensibility.Modules.ModuleHost modules, Inno.Core.Settings.ProjectSettingsStore settings, Inno.Scripting.Reload.IScriptReloadCoordinator reloads, System.Func<Inno.Scripting.Compiler.ScriptModuleDeployment, Inno.Extensibility.Modules.IModuleSource> moduleSourceFactory)`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L101) | Creates a reload host for one compiler and runtime module owner.<br>`options`：The automatic compilation, debounce, and long-running warning policy.<br>`compiler`：The compiler that produces immutable candidate script generations.<br>`assets`：The authoring asset pipeline that supplies committed source artifacts.<br>`plugins`：The Plugin environment that owns active and candidate source generations.<br>`modules`：The module host that atomically activates compiled assemblies.<br>`settings`：The project settings store rebuilt with each committed Plugin generation.<br>`reloads`：The host-owned coordinator that commits dependent state together with script generations.<br>`moduleSourceFactory`：The composition-owned factory that chooses the loading policy for each immutable script module. It must return a fresh source without activating or publishing the module. | — | `System.ArgumentNullException`：Thrown when any required dependency is null.<br>`System.ArgumentOutOfRangeException`：Thrown when the configured debounce duration is negative. |
| <a id="api-d99fd3e5cc175627"></a>[`public bool Inno.Scripting.Reload.ScriptReloadHost.AdvanceUnloadVerification(out System.Exception? failure)`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L663) | Advances cooperative unload verification for retired script generations.<br>返回：true when no additional verification frame is required.<br>`failure`：Receives the retention failure after all verification attempts, otherwise null. | — | — |
| <a id="api-3359fe2269df35c8"></a>[`public bool Inno.Scripting.Reload.ScriptReloadHost.ApplyPendingReload()`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L505) | Applies the latest compiled generation, or commits a Plugin-unavailability generation when changed Plugin code cannot produce a replacement, at a caller-controlled main-thread safe point.<br>返回：true when modules were replaced or retired; otherwise, false. | — | `System.ObjectDisposedException`：Thrown after the manager has been disposed. |
| <a id="api-56b75b5b554b3278"></a>[`public bool Inno.Scripting.Reload.ScriptReloadHost.CancelCompilation()`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L296) | Requests cancellation of the active compilation without changing the active script generation.<br>返回：true when an active compilation received the request. | — | `System.ObjectDisposedException`：Thrown after the manager has been disposed. |
| <a id="api-b43176b6c42c3187"></a>[`public void Inno.Scripting.Reload.ScriptReloadHost.Dispose()`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L586) | Cancels and waits for active compilation work, stops Asset Database observation, and unloads the active script module. | — | — |
| <a id="api-6b6265f58517adaa"></a>[`public void Inno.Scripting.Reload.ScriptReloadHost.GenerateProjectFiles()`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L571) | Generates standard SDK-style game/editor projects and a solution for IDE tooling. | — | `System.ObjectDisposedException`：Thrown after the manager has been disposed. |
| <a id="api-166a113fbf040051"></a>[`public void Inno.Scripting.Reload.ScriptReloadHost.RecompileScripting()`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L322) | Queues incremental recompilation of changed script assemblies. | — | `System.ObjectDisposedException`：Thrown after the manager has been disposed. |
| <a id="api-fa69c054093a1013"></a>[`public void Inno.Scripting.Reload.ScriptReloadHost.ReloadPlugins()`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L343) | Queues replacement of the unified plugin generation and both dependent scripting generations. Valid script artifacts are reused when plugin reference fingerprints are unchanged. | — | `System.ObjectDisposedException`：Thrown after the manager has been disposed. |
| <a id="api-6cfe661c1443c5a5"></a>[`public void Inno.Scripting.Reload.ScriptReloadHost.ReloadScripting()`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L334) | Queues a complete rebuild of both scripting load contexts while retaining the plugin generation. Valid cached artifacts are reused. | — | `System.ObjectDisposedException`：Thrown after the manager has been disposed. |
| <a id="api-386a67b3386a3eb2"></a>[`public void Inno.Scripting.Reload.ScriptReloadHost.Start()`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L257) | Subscribes to committed asset changes and queues a cache-aware initial compilation request. | — | — |
| <a id="api-c6ca072769826943"></a>[`public bool Inno.Scripting.Reload.ScriptReloadHost.TryCompilePending(out System.Threading.Tasks.Task<Inno.Scripting.Compiler.ScriptCompilationResult>? compilation)`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L357) | Starts a pending compilation after the configured quiet period has elapsed.<br>返回：true when a pending compilation was started.<br>`compilation`：The started compilation task, or null when none was ready. | The initial automatic request is immediately ready and does not observe the debounce duration. | — |
| <a id="api-320960c1cbc9cff3"></a>[`public System.TimeSpan Inno.Scripting.Reload.ScriptReloadHost.compilationElapsed { get; }`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L199) | Gets the elapsed duration of the active or most recently completed compilation. | — | — |
| <a id="api-ca27730b8c6f1ae5"></a>[`public float Inno.Scripting.Reload.ScriptReloadHost.compilationProgress { get; }`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L180) | Gets the current compilation progress in the inclusive range from zero to one. | — | — |
| <a id="api-cf8ccf11837a63b5"></a>[`public string Inno.Scripting.Reload.ScriptReloadHost.compilationStatus { get; }`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L185) | Gets a short description of the current compilation stage. | — | — |
| <a id="api-477b93b244c46342"></a>[`public bool Inno.Scripting.Reload.ScriptReloadHost.isCompilationPending { get; }`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L168) | Gets whether source or plugin changes are waiting to be compiled. | — | — |
| <a id="api-019f5b07b6fcb214"></a>[`public bool Inno.Scripting.Reload.ScriptReloadHost.isCompilationTakingLong { get; }`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L213) | Gets whether the active compilation exceeded the configured warning duration. | — | — |
| <a id="api-478d31b91243bdfa"></a>[`public bool Inno.Scripting.Reload.ScriptReloadHost.isCompiling { get; }`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L163) | Gets whether a compilation currently owns the compiler gate. | — | — |
| <a id="api-8f3fe58c8e57bbe5"></a>[`public bool Inno.Scripting.Reload.ScriptReloadHost.isFaulted { get; }`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L245) | Gets whether a retired generation exceeded the retention threshold and permanently stopped this host. | — | — |
| <a id="api-c3b1aa504ce412a8"></a>[`public bool Inno.Scripting.Reload.ScriptReloadHost.isUnloadVerificationPending { get; }`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L233) | Gets whether retired collectible assembly generations are still being verified for unload. | — | — |
| <a id="api-842e9368e922a5f9"></a>[`public Inno.Scripting.Compiler.ScriptCompilationResult? Inno.Scripting.Reload.ScriptReloadHost.lastCompilation { get; }`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L221) | Gets the most recently completed compilation. | — | — |
| <a id="api-d272da59695cbec1"></a>[`public sealed class Inno.Scripting.Reload.ScriptReloadHost : System.IDisposable`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L24) | Watches, compiles, and atomically activates one project's C# script assemblies. | — | — |

### `Inno.Scripting.Reload.ScriptReloadOptions`

<!-- api: T:Inno.Scripting.Reload.ScriptReloadOptions 9e53f24253de0cdc4333a50a3d1bfa01ffbb07437a815a961a4c7f2a58426921 -->
<!-- api: P:Inno.Scripting.Reload.ScriptReloadOptions.autoCompile 48a7db07c8dc6ef3264e5e28f1332e1a266874e0ed1117a76e6546ff846e4fc2 -->
<!-- api: P:Inno.Scripting.Reload.ScriptReloadOptions.debounceMilliseconds 1438359436a12a01de3cea85a80dfec0788bc9902a1b246aceadc95d8b7409f1 -->
<!-- api: P:Inno.Scripting.Reload.ScriptReloadOptions.compilationWarningTimeout f138ef44d4957d80715ff3ec77778989047eaa6e42f43cd51a8974ed27aa9085 -->
<!-- api: P:Inno.Scripting.Reload.ScriptReloadOptions.unloadCollectionInterval 91b60cbebcdf4b79390202f64bc76b6eb93d2a7f724e0d72252ffad4b6bc5a85 -->
<!-- api: P:Inno.Scripting.Reload.ScriptReloadOptions.unloadRetentionTimeout d196ab90dd556553ae139b5b02fbdf98cca55f26c98c16c8c1c1fa473a2697c3 -->
<!-- api: M:Inno.Scripting.Reload.ScriptReloadOptions.#ctor 841f2d85d2e5a4d1f033caa3ae8dffb5d38cd3dd47c5d39d0b03dbfc4d44e0f9 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2995c03f0fc9c532"></a>[`public Inno.Scripting.Reload.ScriptReloadOptions.ScriptReloadOptions()`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadOptions.cs#L7) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-c979a8724228d153"></a>[`public bool Inno.Scripting.Reload.ScriptReloadOptions.autoCompile { get; init; }`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadOptions.cs#L16) | Gets whether startup and subsequent source changes request automatic compilation. | The initial request starts immediately. Later change requests are consumed only at a focused Editor safe point after the configured debounce duration. | — |
| <a id="api-c38c4f6cbdedce43"></a>[`public System.TimeSpan Inno.Scripting.Reload.ScriptReloadOptions.compilationWarningTimeout { get; init; }`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadOptions.cs#L30) | Gets the elapsed duration after which an active compilation is reported as long-running. | Use System.Threading.Timeout.InfiniteTimeSpan to disable the warning. The warning never cancels compilation automatically. | — |
| <a id="api-ee6fba86871ed254"></a>[`public int Inno.Scripting.Reload.ScriptReloadOptions.debounceMilliseconds { get; init; }`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadOptions.cs#L21) | Gets the quiet period applied to source changes before a compilation starts, in milliseconds. | — | — |
| <a id="api-f208b10bb0a775e5"></a>[`public System.TimeSpan Inno.Scripting.Reload.ScriptReloadOptions.unloadCollectionInterval { get; init; }`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadOptions.cs#L35) | Gets the minimum interval between full garbage-collection attempts for retired assembly generations. | — | — |
| <a id="api-88f5e587d7b44a7e"></a>[`public System.TimeSpan Inno.Scripting.Reload.ScriptReloadOptions.unloadRetentionTimeout { get; init; }`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadOptions.cs#L40) | Gets the duration after which a retained collectible generation permanently faults this reload host. | — | — |
| <a id="api-5e58b031a84cc1de"></a>[`public sealed class Inno.Scripting.Reload.ScriptReloadOptions`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadOptions.cs#L7) | Configures automatic compilation requests and retired-generation verification. | — | — |

## 项目依赖

- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Reload](../extensibility/Inno.Extensibility.Reload.md)：公开引用边界由实际签名核对。
- [Inno.Plugins.Authoring](../plugins/Inno.Plugins.Authoring.md)：公开引用边界由实际签名核对。
- [Inno.Core.Settings](../core/Inno.Core.Settings.md)：公开引用边界由实际签名核对。
- [Inno.Assets](../assets/Inno.Assets.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Modules](../extensibility/Inno.Extensibility.Modules.md)：公开引用边界由实际签名核对。
- [Inno.Scripting.Compiler](Inno.Scripting.Compiler.md)：公开引用边界由实际签名核对。
- [Inno.Assets.Pipeline](../assets/Inno.Assets.Pipeline.md)：公开引用边界由实际签名核对。
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
