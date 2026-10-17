# Inno.Scripting.Compiler

## Authoring 标注与 Player 边界

编译器根据 `ScriptingApiScope.Authoring` 的导出清单，在语义绑定后、目标 Player references 绑定前移除纯创作标注使用、自定义派生标注类及相关 using。规则由清单与继承符号推导，不依赖标注的具体实现，也没有属性类型白名单。创作编译与 IDE 保留完整标注 API；Player IL 不保留纯创作标注引用。普通 `SerializableProperty` 不在此 scope，继续表达持久化契约。

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

## Sample 候选编译

Frame owner 在源准备完成后启动编译验证，成功后才提交源事务。调用方持有 transaction 和 compiler 到验证、提交或取消清理全部完成；禁止在 callback 中激活候选 generation。

```csharp
using System;
using Inno.Assets.Pipeline;
using Inno.Scripting.Compiler;

static bool AdvanceSampleImport(
    AssetSampleImportTransaction import,
    ScriptCompiler compiler,
    ref bool validationStarted
) {
    if (!validationStarted)
    {
        if (!import.Advance())
            return false;
        import.BeginValidation(async (
            sources,
            cancellationToken
        ) =>
        {
            ScriptCompilationResult result = await compiler.CompileAuthoringGenerationAsync(
                cancellationToken: cancellationToken, sourceSnapshot: sources).ConfigureAwait(false);
            if (!result.success)
                throw new InvalidOperationException("Sample validation failed.");
        });
        validationStarted = true;
    }
    if (!import.isValidationComplete)
        return false;
    import.Commit();
    return true;
}
```

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Scripting.Compiler.CSharpSampleSourceRewriter`

<!-- api: T:Inno.Scripting.Compiler.CSharpSampleSourceRewriter 78e71d0ddfa26e747f81462b8714fc6ed74b4201d9c3a85c4d9013d05196abab -->
<!-- api: M:Inno.Scripting.Compiler.CSharpSampleSourceRewriter.Transform(Inno.Assets.Pipeline.AssetSampleTransformContext) 1e24adba6bb90a90b67ec19be140bf6db88bc0316a478215b9eff92234ea8454 -->
<!-- api: M:Inno.Scripting.Compiler.CSharpSampleSourceRewriter.#ctor b0b48f6f368f673494776f6cff7115052f7a4fce935a9468c155d6a822e7536d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-32781397bedf2f7b"></a>[`public Inno.Scripting.Compiler.CSharpSampleSourceRewriter.CSharpSampleSourceRewriter()`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/CSharpSampleSourceRewriter.cs#L17) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-e7ef04d98553113d"></a>[`public void Inno.Scripting.Compiler.CSharpSampleSourceRewriter.Transform(Inno.Assets.Pipeline.AssetSampleTransformContext context)`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/CSharpSampleSourceRewriter.cs#L25) | Rewrites explicit stable type identities in cloned C# scripts and registers their asset mappings.<br>`context`：The private sample import stage and its identity map. | — | — |
| <a id="api-814f1b60b90c377e"></a>[`public sealed class Inno.Scripting.Compiler.CSharpSampleSourceRewriter : Inno.Assets.Pipeline.IAssetSampleSourceRewriter`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/CSharpSampleSourceRewriter.cs#L17) | Gives imported C# sample scripts distinct type identities before their scene assets are remapped. | — | — |

### `Inno.Scripting.Compiler.LogicalScriptingApiAnalyzer`

<!-- api: T:Inno.Scripting.Compiler.LogicalScriptingApiAnalyzer 755e4677e7b645f4cba7d131b4c5cedc894e9d0256eae2fbafe0c636c8f0d479 -->
<!-- api: F:Inno.Scripting.Compiler.LogicalScriptingApiAnalyzer.directImplementationNamespaceDiagnosticId e5eed1f01f5a2d84f17558e89fe71bf29744737e496b8b6a06bc5740034e6712 -->
<!-- api: F:Inno.Scripting.Compiler.LogicalScriptingApiAnalyzer.missingLogicalNamespaceDiagnosticId 71786a38139472a58c0a782b5180c823d4b4c16ed5567f9d62c051aa0723c674 -->
<!-- api: F:Inno.Scripting.Compiler.LogicalScriptingApiAnalyzer.compilationWideUsingDiagnosticId 341c5e3c383683a09f60119707fa5be4e4f44582edfd8d33cc51101fabc1ff03 -->
<!-- api: P:Inno.Scripting.Compiler.LogicalScriptingApiAnalyzer.SupportedDiagnostics 589afde8d7091e70f9dbfc4b239d11612b4d3a3424cf1bcfb987ad92dfe3c73a -->
<!-- api: M:Inno.Scripting.Compiler.LogicalScriptingApiAnalyzer.Initialize(Microsoft.CodeAnalysis.Diagnostics.AnalysisContext) ea1bcb4a5eb55466494a7f4ce949683c88fe9afe07fb877302a54050af962c6d -->
<!-- api: M:Inno.Scripting.Compiler.LogicalScriptingApiAnalyzer.#ctor 0fac946baf51d905b93415db4be8154840f69e9056d892b0a36972daaa7391c6 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d987b349f8ec966b"></a>[`public const string Inno.Scripting.Compiler.LogicalScriptingApiAnalyzer.compilationWideUsingDiagnosticId = "INNO2003"`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/LogicalScriptingApiAnalyzer.cs#L32) | Diagnostic identifier for a forbidden compilation-wide namespace import. | — | — |
| <a id="api-4d630d8b3febdbd9"></a>[`public const string Inno.Scripting.Compiler.LogicalScriptingApiAnalyzer.directImplementationNamespaceDiagnosticId = "INNO2001"`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/LogicalScriptingApiAnalyzer.cs#L22) | Diagnostic identifier for direct implementation namespace access. | — | — |
| <a id="api-86b02ec2472e01e0"></a>[`public const string Inno.Scripting.Compiler.LogicalScriptingApiAnalyzer.missingLogicalNamespaceDiagnosticId = "INNO2002"`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/LogicalScriptingApiAnalyzer.cs#L27) | Diagnostic identifier for a missing logical namespace import. | — | — |
| <a id="api-838b3a541be8adc7"></a>[`public Inno.Scripting.Compiler.LogicalScriptingApiAnalyzer.LogicalScriptingApiAnalyzer()`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/LogicalScriptingApiAnalyzer.cs#L17) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-ce5d3008681fca61"></a>[`public override void Inno.Scripting.Compiler.LogicalScriptingApiAnalyzer.Initialize(Microsoft.CodeAnalysis.Diagnostics.AnalysisContext context)`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/LogicalScriptingApiAnalyzer.cs#L73) | Initializes owned resources and establishes the instance's active lifetime.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-9b22eb913825f606"></a>[`public override System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.DiagnosticDescriptor> Inno.Scripting.Compiler.LogicalScriptingApiAnalyzer.SupportedDiagnostics { get; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/LogicalScriptingApiAnalyzer.cs#L61) | Gets the complete compiler diagnostic set enforced by this analyzer. | — | — |
| <a id="api-951c55fbe067f0bc"></a>[`public sealed class Inno.Scripting.Compiler.LogicalScriptingApiAnalyzer : Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/LogicalScriptingApiAnalyzer.cs#L17) | Enforces logical scripting namespaces and rejects direct implementation namespace access. | — | — |

### `Inno.Scripting.Compiler.ScriptAssemblyDefinitionAsset`

<!-- api: T:Inno.Scripting.Compiler.ScriptAssemblyDefinitionAsset 066c6b73f217d691525db115d667cc86fd98b607d770ea019965fbab60d1cb06 -->
<!-- api: P:Inno.Scripting.Compiler.ScriptAssemblyDefinitionAsset.assemblyName 60ab5b9f0a3300164f45e89b29c23d303d71fae4f29c21cdaec429eeaa1a0d61 -->
<!-- api: P:Inno.Scripting.Compiler.ScriptAssemblyDefinitionAsset.scope ed6da8b02b16e52dd0470dca48f6a78d84b0a13f5376ccb37c4e4f154091f023 -->
<!-- api: P:Inno.Scripting.Compiler.ScriptAssemblyDefinitionAsset.references 6cf990b5d7534a64c4d05ef5cf5f7c5bbaf737d22f2042089447734179cb83a9 -->
<!-- api: P:Inno.Scripting.Compiler.ScriptAssemblyDefinitionAsset.defines efc5fc0d5d8216da7a976896cb826c58188e1e4cfaadd473835de72332102cff -->
<!-- api: P:Inno.Scripting.Compiler.ScriptAssemblyDefinitionAsset.nullable 3906576fb1b1d3ff6d2dda84fff188d21a033b260619c201750d4b85621b575c -->
<!-- api: P:Inno.Scripting.Compiler.ScriptAssemblyDefinitionAsset.allowUnsafe 1b405fb8650f1d8d853c0cb5267bd53d3bb1f39f056717e53c43c58fc9a7e5b1 -->
<!-- api: M:Inno.Scripting.Compiler.ScriptAssemblyDefinitionAsset.#ctor e14ede228e1f3e3cf1cb961681d239353f57450a40c3c6a17bb4f5d88fb68296 -->
<!-- api: M:Inno.Scripting.Compiler.ScriptAssemblyDefinitionAsset.#ctor(System.String,Inno.Scripting.Compiler.ScriptAssemblyScope,System.String[],System.String[],System.Boolean,System.Boolean) 5edba28ace99e4ae5f845f1d789478ef3f6ac5bb5727e37798fb0565dd3dfa07 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-5a8428011eab51f6"></a>[`public Inno.Scripting.Compiler.ScriptAssemblyDefinitionAsset.ScriptAssemblyDefinitionAsset()`](../../src/runtime/scripting/Inno.Scripting.Compiler/Assets/ScriptAssemblyDefinitionAsset.cs#L54) | Creates an empty definition asset for deserialization. | — | — |
| <a id="api-bf643f72db87f202"></a>[`public Inno.Scripting.Compiler.ScriptAssemblyDefinitionAsset.ScriptAssemblyDefinitionAsset(string assemblyName, Inno.Scripting.Compiler.ScriptAssemblyScope scope, string[]? references = null, string[]? defines = null, bool nullable = true, bool allowUnsafe = false)`](../../src/runtime/scripting/Inno.Scripting.Compiler/Assets/ScriptAssemblyDefinitionAsset.cs#L82) | Creates a configured script assembly definition for native asset export.<br>`assemblyName`：Stable managed assembly name.<br>`scope`：Runtime or Editor API scope.<br>`references`：Referenced script assembly names.<br>`defines`：Preprocessor symbols.<br>`nullable`：Whether nullable reference analysis is enabled.<br>`allowUnsafe`：Whether unsafe source is permitted. | — | `System.ArgumentException`：Thrown when assemblyName is empty. |
| <a id="api-cae87c53fcc69f8e"></a>[`public bool Inno.Scripting.Compiler.ScriptAssemblyDefinitionAsset.allowUnsafe { get; private set; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/Assets/ScriptAssemblyDefinitionAsset.cs#L49) | Gets whether unsafe source is permitted. | — | — |
| <a id="api-7870d3280379871b"></a>[`public string Inno.Scripting.Compiler.ScriptAssemblyDefinitionAsset.assemblyName { get; private set; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/Assets/ScriptAssemblyDefinitionAsset.cs#L19) | Gets the stable assembly name. | — | — |
| <a id="api-25571d63529bd8b3"></a>[`public string[] Inno.Scripting.Compiler.ScriptAssemblyDefinitionAsset.defines { get; private set; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/Assets/ScriptAssemblyDefinitionAsset.cs#L37) | Gets preprocessor symbols applied to this assembly. | — | — |
| <a id="api-27adb35d854a53ea"></a>[`public bool Inno.Scripting.Compiler.ScriptAssemblyDefinitionAsset.nullable { get; private set; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/Assets/ScriptAssemblyDefinitionAsset.cs#L43) | Gets whether nullable reference analysis is enabled. | — | — |
| <a id="api-08f5b00738a807be"></a>[`public string[] Inno.Scripting.Compiler.ScriptAssemblyDefinitionAsset.references { get; private set; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/Assets/ScriptAssemblyDefinitionAsset.cs#L31) | Gets referenced script assembly names. | — | — |
| <a id="api-4acc36db008dba0b"></a>[`public Inno.Scripting.Compiler.ScriptAssemblyScope Inno.Scripting.Compiler.ScriptAssemblyDefinitionAsset.scope { get; private set; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/Assets/ScriptAssemblyDefinitionAsset.cs#L25) | Gets the assembly API scope. | — | — |
| <a id="api-e2451df648b6b38b"></a>[`public sealed class Inno.Scripting.Compiler.ScriptAssemblyDefinitionAsset : Inno.Assets.AssetObject`](../../src/runtime/scripting/Inno.Scripting.Compiler/Assets/ScriptAssemblyDefinitionAsset.cs#L13) | Defines one project script assembly and its compilation policy. | — | — |

### `Inno.Scripting.Compiler.ScriptAssemblyScope`

<!-- api: T:Inno.Scripting.Compiler.ScriptAssemblyScope a011596928e34fd35c6a89566e349598d5d4cb463cc1594f433968534ae3f8a5 -->
<!-- api: F:Inno.Scripting.Compiler.ScriptAssemblyScope.Runtime 813caad331415f7505fa10db01a004fa767576299672064aa613654a926fb77e -->
<!-- api: F:Inno.Scripting.Compiler.ScriptAssemblyScope.Editor 78b766161fdf2029566aa34afa8a6f06049e870466c3cb5080f7a016e358d36d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-108a849413d89584"></a>[`Inno.Scripting.Compiler.ScriptAssemblyScope.Editor = 1`](../../src/runtime/scripting/Inno.Scripting.Compiler/Assets/ScriptAssemblyScope.cs#L16) | The assembly can use runtime and editor scripting APIs. | — | — |
| <a id="api-70f379ec67a9e3f1"></a>[`Inno.Scripting.Compiler.ScriptAssemblyScope.Runtime = 0`](../../src/runtime/scripting/Inno.Scripting.Compiler/Assets/ScriptAssemblyScope.cs#L11) | The assembly can use runtime scripting APIs only. | — | — |
| <a id="api-1044e5f0b1f28b08"></a>[`public enum Inno.Scripting.Compiler.ScriptAssemblyScope : int`](../../src/runtime/scripting/Inno.Scripting.Compiler/Assets/ScriptAssemblyScope.cs#L6) | Identifies whether a project script assembly can use editor-only APIs. | — | — |

### `Inno.Scripting.Compiler.ScriptCompilationProgress`

<!-- api: T:Inno.Scripting.Compiler.ScriptCompilationProgress 12522211b4c10f31065fa889c6e70738eb30d8ac5ec276dab55353e15e88033e -->
<!-- api: M:Inno.Scripting.Compiler.ScriptCompilationProgress.#ctor(System.Single,System.String) 936250b5c45b32ffc3598e8b420059d90c0449a9659b6e9937c1538a36bec78b -->
<!-- api: P:Inno.Scripting.Compiler.ScriptCompilationProgress.fraction 3b8541505d70b1d8bc98d9a6f74ea8e6e8a2cfa77051b3d11a605d8f0cd21c69 -->
<!-- api: P:Inno.Scripting.Compiler.ScriptCompilationProgress.stage 19fec976ff5fcc9c361d730d1187c459f0de6fae8f2e65a5be03b595a460ab27 -->
<!-- api: M:Inno.Scripting.Compiler.ScriptCompilationProgress.ToString 7add20dc40ddf83f6e9c590ed2c61052270b3cfeae09160165e0d9afce6dfbc4 -->
<!-- api: M:Inno.Scripting.Compiler.ScriptCompilationProgress.op_Inequality(Inno.Scripting.Compiler.ScriptCompilationProgress,Inno.Scripting.Compiler.ScriptCompilationProgress) 4bdc24c549d47768609f9aacc7d866a2500b7dc322ad4358ea5914e469c05a00 -->
<!-- api: M:Inno.Scripting.Compiler.ScriptCompilationProgress.op_Equality(Inno.Scripting.Compiler.ScriptCompilationProgress,Inno.Scripting.Compiler.ScriptCompilationProgress) 58e167567d14d6d009f0a0aad5d7e9779b95e955d201e1d0b9c0e25f07eea42e -->
<!-- api: M:Inno.Scripting.Compiler.ScriptCompilationProgress.GetHashCode 4502c66aad058c7da1555513e75fdedc05f5424fd7d72a26027434a21b05ba3c -->
<!-- api: M:Inno.Scripting.Compiler.ScriptCompilationProgress.Equals(System.Object) 089b2663810e9ca6da304af60affe91e00dabd6337822afb14b61ba4c12280a3 -->
<!-- api: M:Inno.Scripting.Compiler.ScriptCompilationProgress.Equals(Inno.Scripting.Compiler.ScriptCompilationProgress) 1a7e0470bc80a369b2e9583064cdaee3565ffa4468fb4a3e5a4b4ae79a372ae0 -->
<!-- api: M:Inno.Scripting.Compiler.ScriptCompilationProgress.Deconstruct(System.Single@,System.String@) 38ad1db28bb52d7b21835aa09eee116d541c794c389b007354e9686e8e1f024b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-cac710a0afe66b79"></a>[`public Inno.Scripting.Compiler.ScriptCompilationProgress.ScriptCompilationProgress(float fraction, string stage)`](../../src/runtime/scripting/Inno.Scripting.Compiler/ScriptCompilationProgress.cs#L12) | Describes monotonic progress through one script compilation.<br>`fraction`：The completed fraction in the inclusive range from zero to one.<br>`stage`：The current human-readable compiler stage. | — | — |
| <a id="api-755c5b365353a10d"></a>[`public void Inno.Scripting.Compiler.ScriptCompilationProgress.Deconstruct(out float fraction, out string stage)`](../../src/runtime/scripting/Inno.Scripting.Compiler/ScriptCompilationProgress.cs#L12) | C# record 生成的解构入口，按主构造声明顺序返回位置成员。 | — | — |
| <a id="api-3e7563b4b651d49e"></a>[`public bool Inno.Scripting.Compiler.ScriptCompilationProgress.Equals(Inno.Scripting.Compiler.ScriptCompilationProgress other)`](../../src/runtime/scripting/Inno.Scripting.Compiler/ScriptCompilationProgress.cs#L12) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-5983e54e3e920be9"></a>[`public override bool Inno.Scripting.Compiler.ScriptCompilationProgress.Equals(object obj)`](../../src/runtime/scripting/Inno.Scripting.Compiler/ScriptCompilationProgress.cs#L12) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-d7e18b35b7ffcd48"></a>[`public override int Inno.Scripting.Compiler.ScriptCompilationProgress.GetHashCode()`](../../src/runtime/scripting/Inno.Scripting.Compiler/ScriptCompilationProgress.cs#L12) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-b718b9a882872789"></a>[`public static bool Inno.Scripting.Compiler.ScriptCompilationProgress.operator ==(Inno.Scripting.Compiler.ScriptCompilationProgress left, Inno.Scripting.Compiler.ScriptCompilationProgress right)`](../../src/runtime/scripting/Inno.Scripting.Compiler/ScriptCompilationProgress.cs#L12) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-16f74a8095ad8717"></a>[`public static bool Inno.Scripting.Compiler.ScriptCompilationProgress.operator !=(Inno.Scripting.Compiler.ScriptCompilationProgress left, Inno.Scripting.Compiler.ScriptCompilationProgress right)`](../../src/runtime/scripting/Inno.Scripting.Compiler/ScriptCompilationProgress.cs#L12) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-d0b960aa536e7976"></a>[`public override string Inno.Scripting.Compiler.ScriptCompilationProgress.ToString()`](../../src/runtime/scripting/Inno.Scripting.Compiler/ScriptCompilationProgress.cs#L12) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-8b413f6ec8dd9679"></a>[`public float Inno.Scripting.Compiler.ScriptCompilationProgress.fraction { get; init; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/ScriptCompilationProgress.cs#L13) | The completed fraction in the inclusive range from zero to one. | — | — |
| <a id="api-218bd414bf3ebc34"></a>[`public string Inno.Scripting.Compiler.ScriptCompilationProgress.stage { get; init; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/ScriptCompilationProgress.cs#L14) | The current human-readable compiler stage. | — | — |
| <a id="api-934f87c1d7658981"></a>[`public readonly record struct Inno.Scripting.Compiler.ScriptCompilationProgress : System.IEquatable<Inno.Scripting.Compiler.ScriptCompilationProgress>`](../../src/runtime/scripting/Inno.Scripting.Compiler/ScriptCompilationProgress.cs#L12) | Describes monotonic progress through one script compilation.<br>`fraction`：The completed fraction in the inclusive range from zero to one.<br>`stage`：The current human-readable compiler stage. | — | — |

### `Inno.Scripting.Compiler.ScriptCompilationResult`

<!-- api: T:Inno.Scripting.Compiler.ScriptCompilationResult 8fc595bf485eefa9ac0a6347e91d5f3cc7a59c256baa9acd67a6758663e95258 -->
<!-- api: P:Inno.Scripting.Compiler.ScriptCompilationResult.success ff787a337f24110997a708c7c16e1fb59b070476a0a248a9306e83358ef75771 -->
<!-- api: P:Inno.Scripting.Compiler.ScriptCompilationResult.diagnostics 8efa3254b59c541932fb3c7a5e8795c474cc767e89bf3f6a218a6f60e07eb065 -->
<!-- api: P:Inno.Scripting.Compiler.ScriptCompilationResult.outputDirectory 3e7de2e2d08a0063771d3373e4169fb189cf6af020926789a2a70311d62ed7a2 -->
<!-- api: P:Inno.Scripting.Compiler.ScriptCompilationResult.stageTimings e914a5e7bb11610df41703ab573c712e333a03a56a7c3e276407eca64b7e9547 -->
<!-- api: P:Inno.Scripting.Compiler.ScriptCompilationResult.runtimeAssemblyPaths 6507d816e744ce426affeb276b7a912edd159b24013e69aa896f501cddfc2b87 -->
<!-- api: P:Inno.Scripting.Compiler.ScriptCompilationResult.moduleDeployments d2a825cc8ab4ae19a2a749a3d36e531e5fcbf861b7e9545fbd893618c192786f -->
<!-- api: P:Inno.Scripting.Compiler.ScriptCompilationResult.compiledAssemblyNames b6ccbdcffa7f0b3e3b73b8a3df084a6fea241ff7602873b9afd2a9683673aadc -->
<!-- api: P:Inno.Scripting.Compiler.ScriptCompilationResult.reusedAssemblyNames 15139c5c7ba3128c9bb2c406c0155d65ef93899c71239f77c6e70ffc05de5230 -->
<!-- api: M:Inno.Scripting.Compiler.ScriptCompilationResult.Failure(Inno.Scripting.Compiler.ScriptDiagnostic) 81cb46f48d2f072e622f38901575c2981679e230ca2b91f8f8c134968dca8302 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-650cac3fce83c264"></a>[`public static Inno.Scripting.Compiler.ScriptCompilationResult Inno.Scripting.Compiler.ScriptCompilationResult.Failure(Inno.Scripting.Compiler.ScriptDiagnostic diagnostic)`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptCompilationResult.cs#L93) | Creates a failed result for an exception intercepted at an orchestration boundary.<br>返回：A failed immutable result with no output directory or module deployments.<br>`diagnostic`：The diagnostic describing why no candidate generation was produced. | — | — |
| <a id="api-696c3e062d0c319c"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Scripting.Compiler.ScriptCompilationResult.compiledAssemblyNames { get; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptCompilationResult.cs#L77) | Gets assembly names compiled during this request instead of reused from the artifact cache. | — | — |
| <a id="api-92d264b1545744fa"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Scripting.Compiler.ScriptDiagnostic> Inno.Scripting.Compiler.ScriptCompilationResult.diagnostics { get; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptCompilationResult.cs#L52) | Gets all diagnostics produced by the compilation. | — | — |
| <a id="api-600fe9de8cdfe464"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Scripting.Compiler.ScriptModuleDeployment> Inno.Scripting.Compiler.ScriptCompilationResult.moduleDeployments { get; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptCompilationResult.cs#L72) | Gets the immutable module artifacts and dependency topology produced by this compilation. | — | — |
| <a id="api-ac1ea0ed7c4e078d"></a>[`public string? Inno.Scripting.Compiler.ScriptCompilationResult.outputDirectory { get; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptCompilationResult.cs#L57) | Gets the generation output directory when one was created. | — | — |
| <a id="api-73018b9a1d16c616"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Scripting.Compiler.ScriptCompilationResult.reusedAssemblyNames { get; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptCompilationResult.cs#L82) | Gets assembly names reused from the deterministic artifact cache. | — | — |
| <a id="api-c39e387875388336"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Scripting.Compiler.ScriptCompilationResult.runtimeAssemblyPaths { get; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptCompilationResult.cs#L67) | Gets exact runtime-scope managed assemblies suitable for a deployed Player. | — | — |
| <a id="api-062ccd5d52593abd"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Scripting.Compiler.ScriptCompilationStageTiming> Inno.Scripting.Compiler.ScriptCompilationResult.stageTimings { get; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptCompilationResult.cs#L62) | Gets completed compiler stage timings in execution order. | — | — |
| <a id="api-a4dbbaa4c99cc9c1"></a>[`public bool Inno.Scripting.Compiler.ScriptCompilationResult.success { get; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptCompilationResult.cs#L47) | Gets whether every discovered script assembly compiled or reused successfully. | — | — |
| <a id="api-a0ec28dacb809935"></a>[`public sealed class Inno.Scripting.Compiler.ScriptCompilationResult`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptCompilationResult.cs#L13) | Reports the outputs and diagnostics of one authoring-generation or runtime-deployment script compilation. | — | — |

### `Inno.Scripting.Compiler.ScriptCompilationStageTiming`

<!-- api: T:Inno.Scripting.Compiler.ScriptCompilationStageTiming 2ef7c37178f1cb154ded343d287fda46f81bfaf07cadec942e95c05d9c1e9566 -->
<!-- api: M:Inno.Scripting.Compiler.ScriptCompilationStageTiming.#ctor(System.String,System.TimeSpan) e910d63e001bf4e500dba39ceb70c07e5d024e6034843b71371948ad379243e8 -->
<!-- api: P:Inno.Scripting.Compiler.ScriptCompilationStageTiming.stage 2deb2c4266be4ac72dcd09c1267f260628db6b5cf12138cd846b7961c3189cad -->
<!-- api: P:Inno.Scripting.Compiler.ScriptCompilationStageTiming.elapsed e5a2968817265e582aa7cb3213b4ca379c95b6ffbe7d25d16da3ebc05923902d -->
<!-- api: M:Inno.Scripting.Compiler.ScriptCompilationStageTiming.ToString 7a2a0d25849dfb7053b6b1bd122d6a4768491c1ee499db38264cce9f1b51e438 -->
<!-- api: M:Inno.Scripting.Compiler.ScriptCompilationStageTiming.op_Inequality(Inno.Scripting.Compiler.ScriptCompilationStageTiming,Inno.Scripting.Compiler.ScriptCompilationStageTiming) b3ed7f415aff3ba045b774694def4b568db76337cfdc15ad28feb3f69bfa8fc0 -->
<!-- api: M:Inno.Scripting.Compiler.ScriptCompilationStageTiming.op_Equality(Inno.Scripting.Compiler.ScriptCompilationStageTiming,Inno.Scripting.Compiler.ScriptCompilationStageTiming) 0cf308b7bb1036e8701e84c226d31c6560e4e0147a79834cab4765752f0f88e7 -->
<!-- api: M:Inno.Scripting.Compiler.ScriptCompilationStageTiming.GetHashCode b43160ccccff5ca235733f1453c9000fb11436f1929b9d7eecab72e66ff4a331 -->
<!-- api: M:Inno.Scripting.Compiler.ScriptCompilationStageTiming.Equals(System.Object) cb95104ea80dc7892b5afe4ba7c57e54ea836f2657ca8f5a7c9c8af70fc050c9 -->
<!-- api: M:Inno.Scripting.Compiler.ScriptCompilationStageTiming.Equals(Inno.Scripting.Compiler.ScriptCompilationStageTiming) efa68b31362ab885a77c3282a161a66105321b407b37ae54ca90c6425c71c28c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-6f1602ed8781df3d"></a>[`public Inno.Scripting.Compiler.ScriptCompilationStageTiming.ScriptCompilationStageTiming(string stage, System.TimeSpan elapsed)`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptCompilationStageTiming.cs#L25) | Creates one immutable stage timing sample.<br>`stage`：The human-readable stage description.<br>`elapsed`：The wall time spent in the stage. | — | `System.ArgumentException`：Thrown when stage is empty or contains only whitespace.<br>`System.ArgumentOutOfRangeException`：Thrown when elapsed is negative. |
| <a id="api-df6556c62b7803a2"></a>[`public bool Inno.Scripting.Compiler.ScriptCompilationStageTiming.Equals(Inno.Scripting.Compiler.ScriptCompilationStageTiming other)`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptCompilationStageTiming.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-37d30d2c765c6674"></a>[`public override bool Inno.Scripting.Compiler.ScriptCompilationStageTiming.Equals(object obj)`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptCompilationStageTiming.cs#L8) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-51108aff3e8235a3"></a>[`public override int Inno.Scripting.Compiler.ScriptCompilationStageTiming.GetHashCode()`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptCompilationStageTiming.cs#L8) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-0308bc93b1fef6a4"></a>[`public static bool Inno.Scripting.Compiler.ScriptCompilationStageTiming.operator ==(Inno.Scripting.Compiler.ScriptCompilationStageTiming left, Inno.Scripting.Compiler.ScriptCompilationStageTiming right)`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptCompilationStageTiming.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-afdae13afef2bdbc"></a>[`public static bool Inno.Scripting.Compiler.ScriptCompilationStageTiming.operator !=(Inno.Scripting.Compiler.ScriptCompilationStageTiming left, Inno.Scripting.Compiler.ScriptCompilationStageTiming right)`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptCompilationStageTiming.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-059f5b9a3c53a6ca"></a>[`public override string Inno.Scripting.Compiler.ScriptCompilationStageTiming.ToString()`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptCompilationStageTiming.cs#L8) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-9b337878009a461e"></a>[`public System.TimeSpan Inno.Scripting.Compiler.ScriptCompilationStageTiming.elapsed { get; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptCompilationStageTiming.cs#L45) | Gets the wall time spent in the stage. | — | — |
| <a id="api-456580e90e8e4fd7"></a>[`public string Inno.Scripting.Compiler.ScriptCompilationStageTiming.stage { get; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptCompilationStageTiming.cs#L40) | Gets the human-readable stage description. | — | — |
| <a id="api-d7249863de783d6b"></a>[`public readonly record struct Inno.Scripting.Compiler.ScriptCompilationStageTiming : System.IEquatable<Inno.Scripting.Compiler.ScriptCompilationStageTiming>`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptCompilationStageTiming.cs#L8) | Describes the elapsed wall time of one completed script compilation stage. | — | — |

### `Inno.Scripting.Compiler.ScriptCompiler`

<!-- api: T:Inno.Scripting.Compiler.ScriptCompiler e45ca3f759396bc9b3ede5b955212a63f6a24b1c99d0d1a3d41e3a1dbd05c594 -->
<!-- api: M:Inno.Scripting.Compiler.ScriptCompiler.#ctor(Inno.Scripting.Compiler.ScriptCompilerOptions,Inno.Assets.Pipeline.AssetPipeline,Inno.Plugins.Authoring.PluginEnvironment) 3aa4991f8cbdafb35d8fb38271b1da1b081550b8cac87e7cb67fe5c462bff9a3 -->
<!-- api: M:Inno.Scripting.Compiler.ScriptCompiler.GenerateProjectFiles c868a7dbad380ffff94536c4072a81062ba507cdd89f4eb3d67848ee12fc20ca -->
<!-- api: M:Inno.Scripting.Compiler.ScriptCompiler.CompileAuthoringGenerationAsync(System.IProgress{Inno.Scripting.Compiler.ScriptCompilationProgress},System.Threading.CancellationToken,Inno.Assets.Pipeline.IAssetSourceSnapshot) 46bcba0cea8ef0e2627c79e05299d77829f5b8aa217bfdec7cd959e5bb01f717 -->
<!-- api: M:Inno.Scripting.Compiler.ScriptCompiler.CompileRuntimeDeploymentAsync(System.String,System.IProgress{Inno.Scripting.Compiler.ScriptCompilationProgress},System.Threading.CancellationToken) 07093e9dd3515291d1c8ebbd6bb3d275b24cb93c263257314c394641742f4e94 -->
<!-- api: M:Inno.Scripting.Compiler.ScriptCompiler.CollectArtifacts(System.Collections.Generic.IEnumerable{System.String}) 51955c25138858591e10844aac9c25d40944e4a403102e985c056cd3f072ac80 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a8433c37a7df04de"></a>[`public Inno.Scripting.Compiler.ScriptCompiler.ScriptCompiler(Inno.Scripting.Compiler.ScriptCompilerOptions options, Inno.Assets.Pipeline.AssetPipeline assets, Inno.Plugins.Authoring.PluginEnvironment plugins)`](../../src/runtime/scripting/Inno.Scripting.Compiler/ScriptCompiler.cs#L40) | Creates a compiler over explicit authoring services owned by one host.<br>`options`：The project and derived-cache locations used by compilation.<br>`assets`：The authoring asset pipeline supplying committed source artifacts.<br>`plugins`：The Plugin environment supplying the candidate Plugin source generation. | — | `System.ArgumentNullException`：Thrown when an owner argument is null.<br>`System.ArgumentException`：Thrown when the project root is empty. |
| <a id="api-25139372f5a70322"></a>[`public int Inno.Scripting.Compiler.ScriptCompiler.CollectArtifacts(System.Collections.Generic.IEnumerable<string?> retainedDirectories)`](../../src/runtime/scripting/Inno.Scripting.Compiler/ScriptCompiler.cs#L173) | Removes unreferenced compiler generations while retaining the supplied active directories.<br>返回：The number of obsolete generation directories removed from the compiler cache.<br>`retainedDirectories`：The generation directories that remain reachable from active runtime sessions. | — | — |
| <a id="api-41efe62a5f7fac88"></a>[`public System.Threading.Tasks.ValueTask<Inno.Scripting.Compiler.ScriptCompilationResult> Inno.Scripting.Compiler.ScriptCompiler.CompileAuthoringGenerationAsync(System.IProgress<Inno.Scripting.Compiler.ScriptCompilationProgress>? progress = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken), Inno.Assets.Pipeline.IAssetSourceSnapshot? sourceSnapshot = null)`](../../src/runtime/scripting/Inno.Scripting.Compiler/ScriptCompiler.cs#L96) | Compiles a complete runtime and editor candidate generation without activating it.<br>返回：The immutable diagnostics and artifacts produced by the compilation attempt.<br>`progress`：Optional observer for monotonic compiler progress.<br>`cancellationToken`：The token that cancels compilation before an artifact generation is committed.<br>`sourceSnapshot`：Optional isolated authoring sources. Capture on its owner thread and keep its transaction alive until compilation drains; null selects the current Plugin candidate or active sources. | Source discovery captures live inputs on the owner thread. Reference generation, Roslyn and artifact writes run in the background; progress observers must accept callbacks from either thread. Retain the source generation until the returned operation drains, including after cancellation. | `System.OperationCanceledException`：Thrown when cancellationToken is canceled. |
| <a id="api-aa1ed1280f65dc28"></a>[`public System.Threading.Tasks.ValueTask<Inno.Scripting.Compiler.ScriptCompilationResult> Inno.Scripting.Compiler.ScriptCompiler.CompileRuntimeDeploymentAsync(string targetRuntimeDirectory, System.IProgress<Inno.Scripting.Compiler.ScriptCompilationProgress>? progress = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/runtime/scripting/Inno.Scripting.Compiler/ScriptCompiler.cs#L143) | Compiles only the runtime assembly closure required by a deployed Player and binds it to that Player runtime.<br>返回：The immutable diagnostics and runtime-only artifacts produced by the compilation attempt.<br>`targetRuntimeDirectory`：The verified Player Support Pack directory whose runtime assemblies will execute the emitted scripts.<br>`progress`：Optional observer for monotonic compiler progress.<br>`cancellationToken`：The token that cancels compilation before an artifact generation is committed. | — | `System.OperationCanceledException`：Thrown when cancellationToken is canceled.<br>`System.ArgumentException`：Thrown when targetRuntimeDirectory is empty.<br>`System.IO.DirectoryNotFoundException`：Thrown when targetRuntimeDirectory does not exist.<br>`System.IO.InvalidDataException`：Thrown when the target runtime has no valid Inno runtime assembly closure. |
| <a id="api-44713677a7cd1fa2"></a>[`public void Inno.Scripting.Compiler.ScriptCompiler.GenerateProjectFiles()`](../../src/runtime/scripting/Inno.Scripting.Compiler/ScriptCompiler.cs#L64) | Regenerates IDE project files from the current source graph and logical API references. | — | `System.InvalidOperationException`：Thrown when the authoring asset pipeline is not initialized. |
| <a id="api-81e89c199c4d737e"></a>[`public sealed class Inno.Scripting.Compiler.ScriptCompiler`](../../src/runtime/scripting/Inno.Scripting.Compiler/ScriptCompiler.cs#L16) | Produces deterministic runtime and editor script artifacts from one authoring snapshot. | — | — |

### `Inno.Scripting.Compiler.ScriptCompilerOptions`

<!-- api: T:Inno.Scripting.Compiler.ScriptCompilerOptions bfb8dca5b12ed4938c888a651a9fb91dcf4f851fbb5ea27abd57df74e558cebd -->
<!-- api: P:Inno.Scripting.Compiler.ScriptCompilerOptions.projectRootDirectory 28735ace6bd3bd78c1d4374e916b360b15a29e06a8e869bf1ea14ffae49e4246 -->
<!-- api: M:Inno.Scripting.Compiler.ScriptCompilerOptions.#ctor ec4de17d05cb245e285292978f8397f6b892d4a9b8630d03e8c6214419315a94 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-adfcd154e8e65ade"></a>[`public Inno.Scripting.Compiler.ScriptCompilerOptions.ScriptCompilerOptions()`](../../src/runtime/scripting/Inno.Scripting.Compiler/ScriptCompilerOptions.cs#L8) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-cf48dc360711e071"></a>[`public required string Inno.Scripting.Compiler.ScriptCompilerOptions.projectRootDirectory { get; init; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/ScriptCompilerOptions.cs#L13) | Gets the project root containing Assets and Library. | — | — |
| <a id="api-1a806827f50e4f72"></a>[`public sealed class Inno.Scripting.Compiler.ScriptCompilerOptions`](../../src/runtime/scripting/Inno.Scripting.Compiler/ScriptCompilerOptions.cs#L8) | Identifies the project and derived-cache locations used by one script compiler. | — | — |

### `Inno.Scripting.Compiler.ScriptDiagnostic`

<!-- api: T:Inno.Scripting.Compiler.ScriptDiagnostic a79953be6af696862091eb88787ff319ff4ebd1adaf145e24fadda3f660eb7f4 -->
<!-- api: M:Inno.Scripting.Compiler.ScriptDiagnostic.#ctor(System.String,Inno.Core.Diagnostics.DiagnosticSeverity,System.String,System.String,System.Int32,System.Int32) 2fc8f2832db7290830557893883d87a1f622b75c197eae812b47ee24c77dfd3a -->
<!-- api: P:Inno.Scripting.Compiler.ScriptDiagnostic.id a53893d17f0beef956da3f4c32710794f2f18b293d139a5a3fed43c51adfdd38 -->
<!-- api: P:Inno.Scripting.Compiler.ScriptDiagnostic.severity 4cab89c1c73f7718c9f374cc53e00eceed82a3c14b1973343425904754d54286 -->
<!-- api: P:Inno.Scripting.Compiler.ScriptDiagnostic.message bf4a3c132fec2f4e45c53f72628fe09796a433e349810f05323166089bc5f87e -->
<!-- api: P:Inno.Scripting.Compiler.ScriptDiagnostic.filePath 5a013c5a516854a1043fa7d54d1c04a3d8fc922f02c74f1334915ccae67d38e0 -->
<!-- api: P:Inno.Scripting.Compiler.ScriptDiagnostic.line be1e797b924a9d6a9c1f6b42e57803cb4460e93e9e76e415602f6df35df34586 -->
<!-- api: P:Inno.Scripting.Compiler.ScriptDiagnostic.column 38befff7533ac9ca98273e6be8e227a200a8ff61aec80ed7376db6a7ca0261c9 -->
<!-- api: M:Inno.Scripting.Compiler.ScriptDiagnostic.ToString 3e7444b8ad287f709ee8309a2ab6c48b8870135af9b42610553e656b3f55c0a2 -->
<!-- api: M:Inno.Scripting.Compiler.ScriptDiagnostic.op_Inequality(Inno.Scripting.Compiler.ScriptDiagnostic,Inno.Scripting.Compiler.ScriptDiagnostic) 0a449822ecedcb813afff1d99d9b6ef8d1d5dc606b4f329c37da31bddb317d23 -->
<!-- api: M:Inno.Scripting.Compiler.ScriptDiagnostic.op_Equality(Inno.Scripting.Compiler.ScriptDiagnostic,Inno.Scripting.Compiler.ScriptDiagnostic) f7941b5b068c6af9f6d2c1bbe4cec5e089b3148da13df0ca659c38e9740bae1a -->
<!-- api: M:Inno.Scripting.Compiler.ScriptDiagnostic.GetHashCode fe739cc33decd413773fb3c86f02ef1d0a6349d8bd6c1c3b13690f3af8bb45a6 -->
<!-- api: M:Inno.Scripting.Compiler.ScriptDiagnostic.Equals(System.Object) 6e66143f6b89bd7cba055eac7f557aee6152503c3e24cbc9cab412e35bbe047c -->
<!-- api: M:Inno.Scripting.Compiler.ScriptDiagnostic.Equals(Inno.Scripting.Compiler.ScriptDiagnostic) 30716c9e833e5d135fb11026da5ec500069ee60360f9f279d04972034c04f596 -->
<!-- api: M:Inno.Scripting.Compiler.ScriptDiagnostic.Deconstruct(System.String@,Inno.Core.Diagnostics.DiagnosticSeverity@,System.String@,System.String@,System.Int32@,System.Int32@) c7c14465ff62b6705d5d572565774140eca3693353ab22748e599768d2bb7e9b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-eacff6b7102629d7"></a>[`public Inno.Scripting.Compiler.ScriptDiagnostic.ScriptDiagnostic(string id, Inno.Core.Diagnostics.DiagnosticSeverity severity, string message, string? filePath, int line, int column)`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptDiagnostic.cs#L25) | Represents one source-oriented script compilation diagnostic.<br>`id`：The stable compiler, analyzer, or reload diagnostic identifier.<br>`severity`：The presentation severity of the diagnostic.<br>`message`：The human-readable diagnostic message.<br>`filePath`：The source path associated with the diagnostic, or null for a project-level diagnostic.<br>`line`：The one-based source line, or zero when no source location is available.<br>`column`：The one-based source column, or zero when no source location is available. | — | — |
| <a id="api-4ed9f348c1f8bb03"></a>[`public void Inno.Scripting.Compiler.ScriptDiagnostic.Deconstruct(out string id, out Inno.Core.Diagnostics.DiagnosticSeverity severity, out string message, out string? filePath, out int line, out int column)`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptDiagnostic.cs#L25) | C# record 生成的解构入口，按主构造声明顺序返回位置成员。 | — | — |
| <a id="api-6e18ea3432addcf8"></a>[`public bool Inno.Scripting.Compiler.ScriptDiagnostic.Equals(Inno.Scripting.Compiler.ScriptDiagnostic? other)`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptDiagnostic.cs#L25) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-c64061944438ff80"></a>[`public override bool Inno.Scripting.Compiler.ScriptDiagnostic.Equals(object? obj)`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptDiagnostic.cs#L25) | Determines whether the specified object is equal to the current object.<br>返回：true if the specified object is equal to the current object; otherwise, false.<br>`obj`：The object to compare with the current object. | — | — |
| <a id="api-00bfbc4d57a1ce56"></a>[`public override int Inno.Scripting.Compiler.ScriptDiagnostic.GetHashCode()`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptDiagnostic.cs#L25) | Serves as the default hash function.<br>返回：A hash code for the current object. | — | — |
| <a id="api-6eb71e4dd0d47621"></a>[`public static bool Inno.Scripting.Compiler.ScriptDiagnostic.operator ==(Inno.Scripting.Compiler.ScriptDiagnostic? left, Inno.Scripting.Compiler.ScriptDiagnostic? right)`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptDiagnostic.cs#L25) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-257cbaa796ad4625"></a>[`public static bool Inno.Scripting.Compiler.ScriptDiagnostic.operator !=(Inno.Scripting.Compiler.ScriptDiagnostic? left, Inno.Scripting.Compiler.ScriptDiagnostic? right)`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptDiagnostic.cs#L25) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-17d3fd9555bb5e7b"></a>[`public override string Inno.Scripting.Compiler.ScriptDiagnostic.ToString()`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptDiagnostic.cs#L25) | Returns a string that represents the current object.<br>返回：A string that represents the current object. | — | — |
| <a id="api-87fbc59e7a15b3d4"></a>[`public int Inno.Scripting.Compiler.ScriptDiagnostic.column { get; init; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptDiagnostic.cs#L31) | The one-based source column, or zero when no source location is available. | — | — |
| <a id="api-174032a308de3968"></a>[`public string? Inno.Scripting.Compiler.ScriptDiagnostic.filePath { get; init; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptDiagnostic.cs#L29) | The source path associated with the diagnostic, or null for a project-level diagnostic. | — | — |
| <a id="api-1618dc6e4168ae7b"></a>[`public string Inno.Scripting.Compiler.ScriptDiagnostic.id { get; init; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptDiagnostic.cs#L26) | The stable compiler, analyzer, or reload diagnostic identifier. | — | — |
| <a id="api-e35ead435ebaefb7"></a>[`public int Inno.Scripting.Compiler.ScriptDiagnostic.line { get; init; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptDiagnostic.cs#L30) | The one-based source line, or zero when no source location is available. | — | — |
| <a id="api-73509cbf49834390"></a>[`public string Inno.Scripting.Compiler.ScriptDiagnostic.message { get; init; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptDiagnostic.cs#L28) | The human-readable diagnostic message. | — | — |
| <a id="api-05897a610543d92c"></a>[`public Inno.Core.Diagnostics.DiagnosticSeverity Inno.Scripting.Compiler.ScriptDiagnostic.severity { get; init; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptDiagnostic.cs#L27) | The presentation severity of the diagnostic. | — | — |
| <a id="api-15ef34a77546d5bf"></a>[`public sealed record Inno.Scripting.Compiler.ScriptDiagnostic : System.IEquatable<Inno.Scripting.Compiler.ScriptDiagnostic>`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptDiagnostic.cs#L25) | Represents one source-oriented script compilation diagnostic.<br>`id`：The stable compiler, analyzer, or reload diagnostic identifier.<br>`severity`：The presentation severity of the diagnostic.<br>`message`：The human-readable diagnostic message.<br>`filePath`：The source path associated with the diagnostic, or null for a project-level diagnostic.<br>`line`：The one-based source line, or zero when no source location is available.<br>`column`：The one-based source column, or zero when no source location is available. | — | — |

### `Inno.Scripting.Compiler.ScriptModuleDeployment`

<!-- api: T:Inno.Scripting.Compiler.ScriptModuleDeployment bb136cf45c5b752abb846a5d3d1d9641343f8cd241d3c35d46aba90e41c31d73 -->
<!-- api: P:Inno.Scripting.Compiler.ScriptModuleDeployment.moduleName 71780ed9c8d5f5221f88442b1777d6edc287ce6893275a272192c72c3f7cf054 -->
<!-- api: P:Inno.Scripting.Compiler.ScriptModuleDeployment.mainAssemblyPath 99038c794c38da2e23d913b81c9ce45edb605ff629c4b677d7dd99779dc96829 -->
<!-- api: P:Inno.Scripting.Compiler.ScriptModuleDeployment.domain 5e81888edf6a7926ce812b0ac682f31f7511c4ef275506b5519c8241319c6f70 -->
<!-- api: P:Inno.Scripting.Compiler.ScriptModuleDeployment.scope e5748e97e74f5eed04c7b5425542775b672c6af95abafb2e0f075a3c2e6821c5 -->
<!-- api: P:Inno.Scripting.Compiler.ScriptModuleDeployment.preloadAssemblyPaths 6f8c693ae7026f2b6ecfa98cfd5f38658a2fbee9d403e7f2c73694376cab0a0e -->
<!-- api: P:Inno.Scripting.Compiler.ScriptModuleDeployment.upstreamModuleNames 1a113ac04c239af959dae94cc1ecc2c0e9415ae28b8ae06a32a890665cb9c91d -->
<!-- api: P:Inno.Scripting.Compiler.ScriptModuleDeployment.assemblyScopes cd1126ce46565a2dd868a48e00ec2f7b69317deaea1dcd96e5b4fc8c2b7ca949 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-35733fd687079254"></a>[`public System.Collections.Generic.IReadOnlyDictionary<string, Inno.Extensibility.Modules.AssemblyScope> Inno.Scripting.Compiler.ScriptModuleDeployment.assemblyScopes { get; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptModuleDeployment.cs#L70) | Gets explicit scopes keyed by assembly simple name, with case-insensitive lookup. | — | — |
| <a id="api-1af7a6b360d108dd"></a>[`public Inno.Extensibility.Modules.AssemblyDomain Inno.Scripting.Compiler.ScriptModuleDeployment.domain { get; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptModuleDeployment.cs#L50) | Gets the ownership domain of this module's assemblies. | — | — |
| <a id="api-a6d3dea512c3cf61"></a>[`public string Inno.Scripting.Compiler.ScriptModuleDeployment.mainAssemblyPath { get; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptModuleDeployment.cs#L45) | Gets the absolute path of the module's primary assembly artifact. | — | — |
| <a id="api-810476c68e010df5"></a>[`public string Inno.Scripting.Compiler.ScriptModuleDeployment.moduleName { get; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptModuleDeployment.cs#L40) | Gets the logical module name used to resolve dependencies within this compiled generation. | — | — |
| <a id="api-77cc1a64788af9f6"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Scripting.Compiler.ScriptModuleDeployment.preloadAssemblyPaths { get; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptModuleDeployment.cs#L60) | Gets the additional assembly artifacts that belong to this same module generation. | — | — |
| <a id="api-95c4cd7cf6b154fe"></a>[`public Inno.Extensibility.Modules.AssemblyScope Inno.Scripting.Compiler.ScriptModuleDeployment.scope { get; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptModuleDeployment.cs#L55) | Gets the dependency scope used for assemblies without an explicit scope override. | — | — |
| <a id="api-b785e889d07278d1"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Scripting.Compiler.ScriptModuleDeployment.upstreamModuleNames { get; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptModuleDeployment.cs#L65) | Gets the logical names of modules that supply managed dependencies to this module. | — | — |
| <a id="api-30f0357e0014dd52"></a>[`public sealed class Inno.Scripting.Compiler.ScriptModuleDeployment`](../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptModuleDeployment.cs#L14) | Describes the immutable assembly artifacts and dependency topology of one compiled script module. The receiving host chooses how these artifacts enter its module catalog. | — | — |

### `Inno.Scripting.Compiler.ScriptSourceAsset`

<!-- api: T:Inno.Scripting.Compiler.ScriptSourceAsset 6d67dd205add0968316a6268a79a3e5f2832f9c4d293123bc65c416acb412ac1 -->
<!-- api: P:Inno.Scripting.Compiler.ScriptSourceAsset.scope a0b7c44e2488943cc0070047eafd84eca4e6867411f0ef8f190d4efdffc55e93 -->
<!-- api: P:Inno.Scripting.Compiler.ScriptSourceAsset.declaredTypeNames dd89427f64c0430e169148cf0abc5ebdb35daae67e8742c77f99cf50a8f1539e -->
<!-- api: P:Inno.Scripting.Compiler.ScriptSourceAsset.parseDiagnostics 7e440e9ed3791aabe6c3d77bddb4c45f86102d61c4f91412ede27c5e760add7d -->
<!-- api: M:Inno.Scripting.Compiler.ScriptSourceAsset.#ctor 27697db22ec259e3ce564f7fbbd7c89fd9211f8377ce1c7d7ef723db10ccf61e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-47a645edb7818274"></a>[`public Inno.Scripting.Compiler.ScriptSourceAsset.ScriptSourceAsset()`](../../src/runtime/scripting/Inno.Scripting.Compiler/Assets/ScriptSourceAsset.cs#L34) | Creates an empty script source asset for deserialization. | — | — |
| <a id="api-71d31112bbd8b7b1"></a>[`public string[] Inno.Scripting.Compiler.ScriptSourceAsset.declaredTypeNames { get; private set; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/Assets/ScriptSourceAsset.cs#L23) | Gets syntax-level type names declared by the source. | — | — |
| <a id="api-05a9644d8eb65756"></a>[`public string[] Inno.Scripting.Compiler.ScriptSourceAsset.parseDiagnostics { get; private set; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/Assets/ScriptSourceAsset.cs#L29) | Gets parse diagnostics associated with the source snapshot. | — | — |
| <a id="api-510620ef9ea4afee"></a>[`public Inno.Scripting.Compiler.ScriptAssemblyScope Inno.Scripting.Compiler.ScriptSourceAsset.scope { get; private set; }`](../../src/runtime/scripting/Inno.Scripting.Compiler/Assets/ScriptSourceAsset.cs#L17) | Gets the default assembly scope inferred for the source. | — | — |
| <a id="api-d7e928e85abd6845"></a>[`public sealed class Inno.Scripting.Compiler.ScriptSourceAsset : Inno.Assets.AssetObject`](../../src/runtime/scripting/Inno.Scripting.Compiler/Assets/ScriptSourceAsset.cs#L11) | Describes an imported C# source snapshot and its parse diagnostics. | — | — |

## 项目依赖

- [Inno.Core.Collections](../core/Inno.Core.Collections.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Modules](../extensibility/Inno.Extensibility.Modules.md)：公开引用边界由实际签名核对。
- [Inno.Scripting.Api](Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Assets](../assets/Inno.Assets.md)：公开引用边界由实际签名核对。
- [Inno.Assets.Pipeline](../assets/Inno.Assets.Pipeline.md)：公开引用边界由实际签名核对。
- [Inno.Plugins.Authoring](../plugins/Inno.Plugins.Authoring.md)：公开引用边界由实际签名核对。
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：公开引用边界由实际签名核对。
- [Inno.Runtime.Generators](../runtime/Inno.Runtime.Generators.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Serialization.Generators](../core/Inno.Core.Serialization.Generators.md)：实现依赖，PrivateAssets="compile"。
