# Inno.Scripting.Reload

## 项目创作代际

同一项目的 GameScripts 与 EditorScripts 共同构成一个创作代际。即使只修改 Editor 源码，也一起经过候选、Scene/Settings 状态恢复、原子激活与严格卸载验证。原因是 Editor 中的泛型扩展闭合于项目 runtime 类型时，CLR loader allocator 可以形成双向依赖；单向程序集引用并不保证两个 collectible context 可独立卸载。编译仍可复用未修改的 GameScripts 产物，不要求重编全部源码。Plugin 仍按公开模块依赖图处理，没有任何 Rendering2D 特例。

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

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Scripting.Reload.IScriptReloadCoordinator`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Extensibility.Reload.IAssemblyUnloadProbe Inno.Scripting.Reload.IScriptReloadCoordinator.Execute(Inno.Extensibility.Modules.AssemblyReloadSession reload, Inno.Extensibility.Reload.IGenerationChange? externalChange = null)`](../../src/runtime/scripting/Inno.Scripting.Reload/IScriptReloadCoordinator.cs#L24) | Commits a prepared assembly reload together with dependent host state. |
| [`Inno.Scripting.Reload.IScriptReloadCoordinator`](../../src/runtime/scripting/Inno.Scripting.Reload/IScriptReloadCoordinator.cs#L10) | Coordinates host-owned state with an atomic script assembly generation transition. |
| [`void Inno.Scripting.Reload.IScriptReloadCoordinator.RefreshDiagnostics()`](../../src/runtime/scripting/Inno.Scripting.Reload/IScriptReloadCoordinator.cs#L32) | Requests diagnostics derived from the active host generation to be republished. |

### `Inno.Scripting.Reload.ScriptReloadHost`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Scripting.Compiler.ScriptCompilationResult? Inno.Scripting.Reload.ScriptReloadHost.lastCompilation`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L221) | Gets the most recently completed compilation. |
| [`Inno.Scripting.Reload.ScriptReloadHost`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L24) | Watches, compiles, and atomically activates one project's C# script assemblies. |
| [`Inno.Scripting.Reload.ScriptReloadHost.ScriptReloadHost(Inno.Scripting.Reload.ScriptReloadOptions options, Inno.Scripting.Compiler.ScriptCompiler compiler, Inno.Assets.Pipeline.AssetPipeline assets, Inno.Plugins.Authoring.PluginEnvironment plugins, Inno.Extensibility.Modules.ModuleHost modules, Inno.Core.Settings.ProjectSettingsStore settings, Inno.Scripting.Reload.IScriptReloadCoordinator reloads, System.Func<Inno.Scripting.Compiler.ScriptModuleDeployment, Inno.Extensibility.Modules.IModuleSource> moduleSourceFactory)`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L101) | Creates a reload host for one compiler and runtime module owner. |
| [`System.TimeSpan Inno.Scripting.Reload.ScriptReloadHost.compilationElapsed`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L199) | Gets the elapsed duration of the active or most recently completed compilation. |
| [`bool Inno.Scripting.Reload.ScriptReloadHost.AdvanceUnloadVerification(out System.Exception? failure)`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L663) | Advances cooperative unload verification for retired script generations. |
| [`bool Inno.Scripting.Reload.ScriptReloadHost.ApplyPendingReload()`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L505) | Applies the latest compiled generation, or commits a Plugin-unavailability generation when changed Plugin code cannot produce a replacement, at a caller-controlled main-thread safe point. |
| [`bool Inno.Scripting.Reload.ScriptReloadHost.CancelCompilation()`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L296) | Requests cancellation of the active compilation without changing the active script generation. |
| [`bool Inno.Scripting.Reload.ScriptReloadHost.TryCompilePending(out System.Threading.Tasks.Task<Inno.Scripting.Compiler.ScriptCompilationResult>? compilation)`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L357) | Starts a pending compilation after the configured quiet period has elapsed. |
| [`bool Inno.Scripting.Reload.ScriptReloadHost.isCompilationPending`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L168) | Gets whether source or plugin changes are waiting to be compiled. |
| [`bool Inno.Scripting.Reload.ScriptReloadHost.isCompilationTakingLong`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L213) | Gets whether the active compilation exceeded the configured warning duration. |
| [`bool Inno.Scripting.Reload.ScriptReloadHost.isCompiling`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L163) | Gets whether a compilation currently owns the compiler gate. |
| [`bool Inno.Scripting.Reload.ScriptReloadHost.isFaulted`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L245) | Gets whether a retired generation exceeded the retention threshold and permanently stopped this host. |
| [`bool Inno.Scripting.Reload.ScriptReloadHost.isUnloadVerificationPending`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L233) | Gets whether retired collectible assembly generations are still being verified for unload. |
| [`float Inno.Scripting.Reload.ScriptReloadHost.compilationProgress`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L180) | Gets the current compilation progress in the inclusive range from zero to one. |
| [`string Inno.Scripting.Reload.ScriptReloadHost.compilationStatus`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L185) | Gets a short description of the current compilation stage. |
| [`void Inno.Scripting.Reload.ScriptReloadHost.Dispose()`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L586) | Cancels and waits for active compilation work, stops Asset Database observation, and unloads the active script module. |
| [`void Inno.Scripting.Reload.ScriptReloadHost.GenerateProjectFiles()`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L571) | Generates standard SDK-style game/editor projects and a solution for IDE tooling. |
| [`void Inno.Scripting.Reload.ScriptReloadHost.RecompileScripting()`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L322) | Queues incremental recompilation of changed script assemblies. |
| [`void Inno.Scripting.Reload.ScriptReloadHost.ReloadPlugins()`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L343) | Queues replacement of the unified plugin generation and both dependent scripting generations. Valid script artifacts are reused when plugin reference fingerprints are unchanged. |
| [`void Inno.Scripting.Reload.ScriptReloadHost.ReloadScripting()`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L334) | Queues a complete rebuild of both scripting load contexts while retaining the plugin generation. Valid cached artifacts are reused. |
| [`void Inno.Scripting.Reload.ScriptReloadHost.Start()`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs#L257) | Subscribes to committed asset changes and queues a cache-aware initial compilation request. |

### `Inno.Scripting.Reload.ScriptReloadOptions`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Scripting.Reload.ScriptReloadOptions`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadOptions.cs#L7) | Configures automatic compilation requests and retired-generation verification. |
| [`System.TimeSpan Inno.Scripting.Reload.ScriptReloadOptions.compilationWarningTimeout`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadOptions.cs#L30) | Gets the elapsed duration after which an active compilation is reported as long-running. |
| [`System.TimeSpan Inno.Scripting.Reload.ScriptReloadOptions.unloadCollectionInterval`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadOptions.cs#L35) | Gets the minimum interval between full garbage-collection attempts for retired assembly generations. |
| [`System.TimeSpan Inno.Scripting.Reload.ScriptReloadOptions.unloadRetentionTimeout`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadOptions.cs#L40) | Gets the duration after which a retained collectible generation permanently faults this reload host. |
| [`bool Inno.Scripting.Reload.ScriptReloadOptions.autoCompile`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadOptions.cs#L16) | Gets whether startup and subsequent source changes request automatic compilation. |
| [`int Inno.Scripting.Reload.ScriptReloadOptions.debounceMilliseconds`](../../src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadOptions.cs#L21) | Gets the quiet period applied to source changes before a compilation starts, in milliseconds. |

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
