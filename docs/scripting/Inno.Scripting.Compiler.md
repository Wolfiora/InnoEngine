# Inno.Scripting.Compiler

## Authoring 标注与 Player 边界

编译器根据 ScriptingApiScope.Authoring 的导出清单，在语义绑定后、目标 Player references 绑定前移除纯创作标注使用、自定义派生标注类及相关 using。规则由清单与继承符号推导，不引用 Editor.Annotations/Serialization/Rendering2D，也没有具体属性类型白名单。创作编译与 IDE 保留完整标注 API，Player IL 不保留 Editor 程序集引用。普通 SerializableProperty 不在此 scope，继续持久化。

[Scripting 索引](README.md) · [API](Inno.Scripting.Api.md) · [Reload](Inno.Scripting.Reload.md)

## 职责与边界

Compiler 拥有 Roslyn、裁剪 reference assemblies、logical namespace analyzer、source fingerprint、runtime/editor artifact 和编译进度。它不拥有 active assembly generation，也不进入 Player。

用户源码先对与 IDE 相同的裁剪契约完成编译检查。只有通过后，生成注册代码的 emission 阶段才读取所属运行时程序集的完整元数据；这些实现引用不扩展用户 API，也不加入 IDE project。该顺序允许生成序列化访问和模块目录，同时继续拒绝标有 `ScriptingApiIgnore` 的成员。发行代码绑定所选 Support Pack，Editor 代码绑定当前 authoring Host。
框架参考程序集优先从宿主输出目录的 `packs/Microsoft.NETCore.App.Ref` 读取；源码开发环境继续从 `DOTNET_ROOT` 或当前 .NET 安装目录读取。Editor Publish 会随发行目录复制与目标框架匹配的 Ref Pack，因此脱离引擎源码和本机 SDK 安装后仍可编译脚本、生成 IDE 工程。
生成实现侧 reference assembly 时以 Ref Pack 的程序集身份排除框架实现，不能按 DLL 所在目录判断：自包含 Editor 将引擎和 .NET DLL 放在同一目录，按目录过滤会在首次生成脚本 API 缓存时误删引擎依赖。

## 公开 API

- `ScriptCompiler`, `ScriptCompilerOptions`：一次 fresh compilation 的入口与 project context；`CompileAuthoringGenerationAsync(progress, cancellationToken, sourceSnapshot)` 生成 Runtime + Editor 的可激活候选；可选 `IAssetSourceSnapshot` 是 owner 捕获编译输入的只读候选视图，省略时读取当前 Plugin compilation view，`CompileRuntimeDeploymentAsync(targetRuntimeDirectory, ...)` 只生成 Player 所需 Runtime closure，并将生成结果绑定到指定 Support Pack 的真实运行时程序集。
- `ScriptCompilationResult`, `ScriptCompilationProgress`, `ScriptCompilationStageTiming`：确定性结果、阶段和 timing。
- `ScriptModuleDeployment`：编译结果中的不可变模块产物描述，包含程序集路径、所属 domain/scope、程序集 scope 和显式模块依赖；集合复制后只读。`ScriptCompilationResult.moduleDeployments` 不携带加载器、ALC 或可执行回调。组合入口决定如何将这些产物激活到 ModuleHost。
- `ScriptDiagnostic`, `ScriptDiagnosticSeverity`：源码定位诊断。
- `ScriptSourceAsset`, `ScriptAssemblyDefinitionAsset`, `ScriptAssemblyScope`：由 common Asset Pipeline 导入的脚本模型。
- `LogicalScriptingApiAnalyzer`：拒绝实现 namespace、global using 和未导出 API。

同一裁剪 reference 规则用于 authoring generation、runtime deployment 与 IDE project。Runtime deployment 先用当前裁剪 API 和 analyzer 验证逻辑 namespace、可见性与脚本规则，再把改写后的源码直接针对目标 Support Pack 的 `Inno.*` 实现程序集编译；目标程序集内容指纹属于增量缓存键，因此更换或修改 Pack 不会复用不兼容脚本产物。IDE 只为 Project 的 Runtime、Editor 和显式 `.iasmdef` assembly 生成源码工程；Project 工程之间通过 `ProjectReference` 连接。安装 Plugin 的源码按同一逻辑 API reference 图编译成 `Library/IDE/PluginReferences` 中的 metadata reference，供 Project 工程引用，不在 Project 根目录生成 Plugin 工程。这样 Plugin 类型的基类与用户源码中的 `InnoEngine.*` 类型属于同一引用图，不会把运行时代的 `Inno.*` 实现程序集泄漏到 IDE。Game Build 只调用 runtime deployment 入口，不解析 Editor API reference、不编译 `.editor.cs`，也不生成 `Inno.EditorScripts.dll`。取消或失败结果没有 activation artifact；缓存命中仍重放诊断。

Shader 创作扩展通过 [`InnoEditor.Rendering.Shaders`](../rendering/Inno.Rendering.Shaders.md) 的逐类型 Editor scope 清单进入同一规则，
不在 Compiler 中增加 Shader 类型或程序集白名单。新增集成用例实际编译节点扩展，并读取生成 IDE reference 的公开 metadata：
Editor 可见 `IShaderNodeCompiler`，Runtime 不可见；Runtime 脚本直接引用该命名空间必须编译失败。

Project `Assets` 的 `~` 目录是普通 authoring content，其中的 `.cs` 与 `.iasmdef` 进入 authoring generation 和 IDE project；runtime deployment 始终剔除该子树。只读 `.iplugin` Mount 中的 `~` 目录是 `.isample`，其脚本编译到独立的作者端临时程序集，供安装态样例场景的 Editor Play 使用，不进入插件运行程序集和 Player。`Import Sample` 完整保留原目录名和全部前导 `~`，可写副本进入 Project authoring/Play 编译，仍从 runtime deployment 排除；`CSharpSampleSourceRewriter` 在导入事务的后台 stage 内用 Roslyn 重写显式 `StableTypeId`，并登记新旧类型身份供场景引用重映射；逐文件响应共同取消 token，不访问 live Editor state。

Compiler 为每个产物写入 `Inno.AssetSource` assembly metadata。Project assembly 写入 `project`，Plugin assembly 写入 manifest Plugin ID。编译和 IDE project 共用源码路径映射，将 `CallerFilePath` 编译为 `<sourceId>::./<asset-local-path>`；`Assets.LocalPath` 通过调用点的该字面量解析同源资源，不依赖 JIT、调用栈或调用者程序集反射。路径中的逗号和等号按 Roslyn PathMap 规则转义，使业务源码在 Project 开发态、安装态、CoreCLR 和 AOT 中保持一致。

脚本编译、generation 激活和 IDE 投影是有顺序但不同的责任。只有完整 Runtime + Editor + Plugin 候选编译成功后才能激活；IDE project 在激活成功后生成。IDE 文件写入失败只发布 `INNO-IDE-PROJECTION` Warning，不允许回滚或阻止已经验证成功的运行 generation。

实现与逻辑 reference 均从公开成员的 nullable metadata 生成可空类型，包括参数、返回值、字段、属性、事件、数组元素与嵌套泛型实参。生成期间的 `NullabilityInfoContext` 在结束时释放，不进入跨代缓存；这些注解属于已有公共契约内容指纹，改变注解会产生新的 API 缓存身份。不能在调用方用 `null!` 掩盖投影遗失的可空契约。

Sample 等输入事务持有共同 generation read lease 时，`ScriptReloadHost.TryCompilePending` 使用 GenerationCoordinator 的非阻塞准入检查：保留请求并返回 false，事务释放后再编译。Compiler 本身不判断 Sample 或平台类型，候选快照与活动快照复用同一个 source discovery / reference / Roslyn 链。

编译入口只在 owner thread 捕获 Asset/Plugin 源快照，随后明确把 API reference 生成、Roslyn、缓存读写与产物写入调度到后台。该边界同时用于 authoring 与 runtime deployment，不依赖 File.ReadAllTextAsync 恰好产生未完成 await；进度 observer 必须接受 owner/worker 回调。取消后调用方仍须等待返回操作 drain，再释放 generation 与源事务。
Reference 生成的串行准入及 Roslyn parse/emit 也接收本次取消 token；取消或生成失败清理未发布的临时 reference 文件，已完成的不可变 reference cache 可以继续复用。

## 当前源码公开 API 清单

以下仅列出当前程序集自己声明的 public/protected 契约；继承成员遵循所属基类页面。internal/private 实现不作为稳定公开 API。签名依据当前源码语义模型生成，行为、参数、异常与所有权说明同时以对应英文 XML 为准。

### `Inno.Scripting.Compiler.CSharpSampleSourceRewriter`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Scripting.Compiler.CSharpSampleSourceRewriter.Transform(Inno.Assets.Pipeline.AssetSampleTransformContext context)`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/CSharpSampleSourceRewriter.cs#L25) | Rewrites explicit stable type identities in cloned C# scripts and registers their asset mappings. |
| [`Inno.Scripting.Compiler.CSharpSampleSourceRewriter`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/CSharpSampleSourceRewriter.cs#L16) | Gives imported C# sample scripts distinct type identities before their scene assets are remapped. |

### `Inno.Scripting.Compiler.LogicalScriptingApiAnalyzer`

| 当前声明 | 行为 |
| --- | --- |
| [`const string Inno.Scripting.Compiler.LogicalScriptingApiAnalyzer.compilationWideUsingDiagnosticId`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/LogicalScriptingApiAnalyzer.cs#L32) | Diagnostic identifier for a forbidden compilation-wide namespace import. |
| [`const string Inno.Scripting.Compiler.LogicalScriptingApiAnalyzer.directImplementationNamespaceDiagnosticId`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/LogicalScriptingApiAnalyzer.cs#L22) | Diagnostic identifier for direct implementation namespace access. |
| [`const string Inno.Scripting.Compiler.LogicalScriptingApiAnalyzer.missingLogicalNamespaceDiagnosticId`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/LogicalScriptingApiAnalyzer.cs#L27) | Diagnostic identifier for a missing logical namespace import. |
| [`override void Inno.Scripting.Compiler.LogicalScriptingApiAnalyzer.Initialize(Microsoft.CodeAnalysis.Diagnostics.AnalysisContext context)`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/LogicalScriptingApiAnalyzer.cs#L73) | Initializes owned resources and establishes the instance's active lifetime. |
| [`override System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.DiagnosticDescriptor> Inno.Scripting.Compiler.LogicalScriptingApiAnalyzer.SupportedDiagnostics`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/LogicalScriptingApiAnalyzer.cs#L61) | Gets the complete compiler diagnostic set enforced by this analyzer. |
| [`Inno.Scripting.Compiler.LogicalScriptingApiAnalyzer`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/LogicalScriptingApiAnalyzer.cs#L16) | Enforces logical scripting namespaces and rejects direct implementation namespace access. |

### `Inno.Scripting.Compiler.ScriptAssemblyDefinitionAsset`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Scripting.Compiler.ScriptAssemblyDefinitionAsset.ScriptAssemblyDefinitionAsset()`](../../src/runtime/scripting/Inno.Scripting.Compiler/Assets/ScriptAssemblyDefinitionAsset.cs#L54) | Creates an empty definition asset for deserialization. |
| [`Inno.Scripting.Compiler.ScriptAssemblyDefinitionAsset.ScriptAssemblyDefinitionAsset(string assemblyName, Inno.Scripting.Compiler.ScriptAssemblyScope scope, string[]? references = null, string[]? defines = null, bool nullable = true, bool allowUnsafe = false)`](../../src/runtime/scripting/Inno.Scripting.Compiler/Assets/ScriptAssemblyDefinitionAsset.cs#L82) | Creates a configured script assembly definition for native asset export. |
| [`bool Inno.Scripting.Compiler.ScriptAssemblyDefinitionAsset.allowUnsafe`](../../src/runtime/scripting/Inno.Scripting.Compiler/Assets/ScriptAssemblyDefinitionAsset.cs#L48) | Gets whether unsafe source is permitted. |
| [`string Inno.Scripting.Compiler.ScriptAssemblyDefinitionAsset.assemblyName`](../../src/runtime/scripting/Inno.Scripting.Compiler/Assets/ScriptAssemblyDefinitionAsset.cs#L18) | Gets the stable assembly name. |
| [`string[] Inno.Scripting.Compiler.ScriptAssemblyDefinitionAsset.defines`](../../src/runtime/scripting/Inno.Scripting.Compiler/Assets/ScriptAssemblyDefinitionAsset.cs#L36) | Gets preprocessor symbols applied to this assembly. |
| [`bool Inno.Scripting.Compiler.ScriptAssemblyDefinitionAsset.nullable`](../../src/runtime/scripting/Inno.Scripting.Compiler/Assets/ScriptAssemblyDefinitionAsset.cs#L42) | Gets whether nullable reference analysis is enabled. |
| [`string[] Inno.Scripting.Compiler.ScriptAssemblyDefinitionAsset.references`](../../src/runtime/scripting/Inno.Scripting.Compiler/Assets/ScriptAssemblyDefinitionAsset.cs#L30) | Gets referenced script assembly names. |
| [`Inno.Scripting.Compiler.ScriptAssemblyScope Inno.Scripting.Compiler.ScriptAssemblyDefinitionAsset.scope`](../../src/runtime/scripting/Inno.Scripting.Compiler/Assets/ScriptAssemblyDefinitionAsset.cs#L24) | Gets the assembly API scope. |
| [`Inno.Scripting.Compiler.ScriptAssemblyDefinitionAsset`](../../src/runtime/scripting/Inno.Scripting.Compiler/Assets/ScriptAssemblyDefinitionAsset.cs#L12) | Defines one project script assembly and its compilation policy. |

### `Inno.Scripting.Compiler.ScriptAssemblyScope`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Scripting.Compiler.ScriptAssemblyScope.Editor`](../../src/runtime/scripting/Inno.Scripting.Compiler/Assets/ScriptAssemblyScope.cs#L16) | The assembly can use runtime and editor scripting APIs. |
| [`Inno.Scripting.Compiler.ScriptAssemblyScope.Runtime`](../../src/runtime/scripting/Inno.Scripting.Compiler/Assets/ScriptAssemblyScope.cs#L11) | The assembly can use runtime scripting APIs only. |
| [`Inno.Scripting.Compiler.ScriptAssemblyScope`](../../src/runtime/scripting/Inno.Scripting.Compiler/Assets/ScriptAssemblyScope.cs#L6) | Identifies whether a project script assembly can use editor-only APIs. |

### `Inno.Scripting.Compiler.ScriptCompilationProgress`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Scripting.Compiler.ScriptCompilationProgress`](../../src/runtime/scripting/Inno.Scripting.Compiler/ScriptCompilationProgress.cs#L12) | Describes monotonic progress through one script compilation. |

### `Inno.Scripting.Compiler.ScriptCompilationResult`

| 当前声明 | 行为 |
| --- | --- |
| [`static Inno.Scripting.Compiler.ScriptCompilationResult Inno.Scripting.Compiler.ScriptCompilationResult.Failure(Inno.Scripting.Compiler.ScriptDiagnostic diagnostic)`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptCompilationResult.cs#L93) | Creates a failed result for an exception intercepted at an orchestration boundary. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Scripting.Compiler.ScriptCompilationResult.compiledAssemblyNames`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptCompilationResult.cs#L77) | Gets assembly names compiled during this request instead of reused from the artifact cache. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Scripting.Compiler.ScriptDiagnostic> Inno.Scripting.Compiler.ScriptCompilationResult.diagnostics`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptCompilationResult.cs#L52) | Gets all diagnostics produced by the compilation. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Scripting.Compiler.ScriptModuleDeployment> Inno.Scripting.Compiler.ScriptCompilationResult.moduleDeployments`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptCompilationResult.cs#L72) | Gets the immutable module artifacts and dependency topology produced by this compilation. |
| [`string? Inno.Scripting.Compiler.ScriptCompilationResult.outputDirectory`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptCompilationResult.cs#L57) | Gets the generation output directory when one was created. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Scripting.Compiler.ScriptCompilationResult.reusedAssemblyNames`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptCompilationResult.cs#L82) | Gets assembly names reused from the deterministic artifact cache. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Scripting.Compiler.ScriptCompilationResult.runtimeAssemblyPaths`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptCompilationResult.cs#L67) | Gets exact runtime-scope managed assemblies suitable for a deployed Player. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Scripting.Compiler.ScriptCompilationStageTiming> Inno.Scripting.Compiler.ScriptCompilationResult.stageTimings`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptCompilationResult.cs#L62) | Gets completed compiler stage timings in execution order. |
| [`bool Inno.Scripting.Compiler.ScriptCompilationResult.success`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptCompilationResult.cs#L47) | Gets whether every discovered script assembly compiled or reused successfully. |
| [`Inno.Scripting.Compiler.ScriptCompilationResult`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptCompilationResult.cs#L13) | Reports the outputs and diagnostics of one authoring-generation or runtime-deployment script compilation. |

### `Inno.Scripting.Compiler.ScriptCompilationStageTiming`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Scripting.Compiler.ScriptCompilationStageTiming.ScriptCompilationStageTiming(string stage, System.TimeSpan elapsed)`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptCompilationStageTiming.cs#L25) | Creates one immutable stage timing sample. |
| [`System.TimeSpan Inno.Scripting.Compiler.ScriptCompilationStageTiming.elapsed`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptCompilationStageTiming.cs#L45) | Gets the wall time spent in the stage. |
| [`string Inno.Scripting.Compiler.ScriptCompilationStageTiming.stage`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptCompilationStageTiming.cs#L40) | Gets the human-readable stage description. |
| [`Inno.Scripting.Compiler.ScriptCompilationStageTiming`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptCompilationStageTiming.cs#L8) | Describes the elapsed wall time of one completed script compilation stage. |

### `Inno.Scripting.Compiler.ScriptCompiler`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Scripting.Compiler.ScriptCompiler.ScriptCompiler(Inno.Scripting.Compiler.ScriptCompilerOptions options, Inno.Assets.Pipeline.AssetPipeline assets, Inno.Plugins.Authoring.PluginEnvironment plugins)`](../../src/runtime/scripting/Inno.Scripting.Compiler/ScriptCompiler.cs#L40) | Creates a compiler over explicit authoring services owned by one host. |
| [`int Inno.Scripting.Compiler.ScriptCompiler.CollectArtifacts(System.Collections.Generic.IEnumerable<string?> retainedDirectories)`](../../src/runtime/scripting/Inno.Scripting.Compiler/ScriptCompiler.cs#L173) | Removes unreferenced compiler generations while retaining the supplied active directories. |
| [`System.Threading.Tasks.ValueTask<Inno.Scripting.Compiler.ScriptCompilationResult> Inno.Scripting.Compiler.ScriptCompiler.CompileAuthoringGenerationAsync(System.IProgress<Inno.Scripting.Compiler.ScriptCompilationProgress>? progress = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken), Inno.Assets.Pipeline.IAssetSourceSnapshot? sourceSnapshot = null)`](../../src/runtime/scripting/Inno.Scripting.Compiler/ScriptCompiler.cs#L96) | Compiles a complete runtime and editor candidate generation without activating it. |
| [`System.Threading.Tasks.ValueTask<Inno.Scripting.Compiler.ScriptCompilationResult> Inno.Scripting.Compiler.ScriptCompiler.CompileRuntimeDeploymentAsync(string targetRuntimeDirectory, System.IProgress<Inno.Scripting.Compiler.ScriptCompilationProgress>? progress = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/runtime/scripting/Inno.Scripting.Compiler/ScriptCompiler.cs#L143) | Compiles only the runtime assembly closure required by a deployed Player and binds it to that Player runtime. |
| [`void Inno.Scripting.Compiler.ScriptCompiler.GenerateProjectFiles()`](../../src/runtime/scripting/Inno.Scripting.Compiler/ScriptCompiler.cs#L64) | Regenerates IDE project files from the current source graph and logical API references. |
| [`Inno.Scripting.Compiler.ScriptCompiler`](../../src/runtime/scripting/Inno.Scripting.Compiler/ScriptCompiler.cs#L16) | Produces deterministic runtime and editor script artifacts from one authoring snapshot. |

### `Inno.Scripting.Compiler.ScriptCompilerOptions`

| 当前声明 | 行为 |
| --- | --- |
| [`required string Inno.Scripting.Compiler.ScriptCompilerOptions.projectRootDirectory`](../../src/runtime/scripting/Inno.Scripting.Compiler/ScriptCompilerOptions.cs#L13) | Gets the project root containing Assets and Library. |
| [`Inno.Scripting.Compiler.ScriptCompilerOptions`](../../src/runtime/scripting/Inno.Scripting.Compiler/ScriptCompilerOptions.cs#L8) | Identifies the project and derived-cache locations used by one script compiler. |

### `Inno.Scripting.Compiler.ScriptDiagnostic`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Scripting.Compiler.ScriptDiagnostic`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptDiagnostic.cs#L25) | Represents one source-oriented script compilation diagnostic. |

### `Inno.Scripting.Compiler.ScriptModuleDeployment`

| 当前声明 | 行为 |
| --- | --- |
| [`System.Collections.Generic.IReadOnlyDictionary<string, Inno.Extensibility.Modules.AssemblyScope> Inno.Scripting.Compiler.ScriptModuleDeployment.assemblyScopes`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptModuleDeployment.cs#L70) | Gets explicit scopes keyed by assembly simple name, with case-insensitive lookup. |
| [`Inno.Extensibility.Modules.AssemblyDomain Inno.Scripting.Compiler.ScriptModuleDeployment.domain`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptModuleDeployment.cs#L50) | Gets the ownership domain of this module's assemblies. |
| [`string Inno.Scripting.Compiler.ScriptModuleDeployment.mainAssemblyPath`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptModuleDeployment.cs#L45) | Gets the absolute path of the module's primary assembly artifact. |
| [`string Inno.Scripting.Compiler.ScriptModuleDeployment.moduleName`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptModuleDeployment.cs#L40) | Gets the logical module name used to resolve dependencies within this compiled generation. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Scripting.Compiler.ScriptModuleDeployment.preloadAssemblyPaths`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptModuleDeployment.cs#L60) | Gets the additional assembly artifacts that belong to this same module generation. |
| [`Inno.Extensibility.Modules.AssemblyScope Inno.Scripting.Compiler.ScriptModuleDeployment.scope`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptModuleDeployment.cs#L55) | Gets the dependency scope used for assemblies without an explicit scope override. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Scripting.Compiler.ScriptModuleDeployment.upstreamModuleNames`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptModuleDeployment.cs#L65) | Gets the logical names of modules that supply managed dependencies to this module. |
| [`Inno.Scripting.Compiler.ScriptModuleDeployment`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptModuleDeployment.cs#L14) | Describes the immutable assembly artifacts and dependency topology of one compiled script module. The receiving host chooses how these artifacts enter its module catalog. |

### `Inno.Scripting.Compiler.ScriptSourceAsset`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Scripting.Compiler.ScriptSourceAsset.ScriptSourceAsset()`](../../src/runtime/scripting/Inno.Scripting.Compiler/Assets/ScriptSourceAsset.cs#L34) | Creates an empty script source asset for deserialization. |
| [`string[] Inno.Scripting.Compiler.ScriptSourceAsset.declaredTypeNames`](../../src/runtime/scripting/Inno.Scripting.Compiler/Assets/ScriptSourceAsset.cs#L22) | Gets syntax-level type names declared by the source. |
| [`string[] Inno.Scripting.Compiler.ScriptSourceAsset.parseDiagnostics`](../../src/runtime/scripting/Inno.Scripting.Compiler/Assets/ScriptSourceAsset.cs#L28) | Gets parse diagnostics associated with the source snapshot. |
| [`Inno.Scripting.Compiler.ScriptAssemblyScope Inno.Scripting.Compiler.ScriptSourceAsset.scope`](../../src/runtime/scripting/Inno.Scripting.Compiler/Assets/ScriptSourceAsset.cs#L16) | Gets the default assembly scope inferred for the source. |
| [`Inno.Scripting.Compiler.ScriptSourceAsset`](../../src/runtime/scripting/Inno.Scripting.Compiler/Assets/ScriptSourceAsset.cs#L10) | Describes an imported C# source snapshot and its parse diagnostics. |

## 项目依赖

- [Inno.Core.Collections](../core/Inno.Core.Collections.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Extensibility.Modules](../extensibility/Inno.Extensibility.Modules.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Scripting.Api](Inno.Scripting.Api.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Assets](../assets/Inno.Assets.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Assets.Pipeline](../assets/Inno.Assets.Pipeline.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Plugins.Authoring](../plugins/Inno.Plugins.Authoring.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Runtime.Generators](../runtime/Inno.Runtime.Generators.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Core.Serialization.Generators](../core/Inno.Core.Serialization.Generators.md)：实现依赖（`PrivateAssets="compile"`）。

共同 MSBuild 注入的 analyzer 与编译规则属于构建依赖，完整有效项目图记录在本轮验收证据中。
