# Inno.Rendering.Assets.Authoring

[分类索引](README.md) · [Wiki 首页](../README.md) · [本轮整改计划](../architecture/ARCHITECTURE_CLEANUP_PLAN_2026_10_06.md)

## 职责与边界

承载 Texture、Shader、Material、Geometry 和 Pipeline 的导入、源码冻结、图编辑、目标编译与 last-good 保存。依赖运行资产、Shaders 与 Assets Pipeline；不进入发行 Player。语言解析与原生编译继续由具体 provider 和工具链实现。

## 工作流

Importer 通过既有 Asset Pipeline 取得完整序列化与引用上下文，冻结源码、依赖及图定义。目标 compiler 接收不可变请求，返回结构化诊断及完整候选。候选验证成功后发布；失败保留相同逻辑资产的 last-good。

`ShaderSourceBundle`、`ShaderGraphArtifact` 将依赖内容冻结为 bytes，运行消费者不重新读取创作目录。SDK 必须使用文件时，由具体工具链创建短生命周期输入文件；临时路径不进入运行资产协议。

`ShaderGraphSourceStore` 管理创作源保存。`ShaderLastGoodStore` 管理可重建创作缓存；两者均不创建 GPU owner。`TextureTargetCompiler` 与 Shader compiler 是可替换边界。

## 使用示例

```csharp
using System.Collections.Generic;
using Inno.Core.Graphs;
using Inno.Core.Serialization;
using Inno.Rendering.Assets.Authoring;

static byte[] FreezeGraph(
    GraphDocument graph,
    IReadOnlyDictionary<GraphNodeId, byte[]> functions,
    SerializationRegistry serialization
) {
    return ShaderGraphArtifact.Encode(graph, functions, serialization);
}
```

注册归属本项目的 importer/provider 由 [默认创作 Adapter](../runtime/Inno.Adapter.Authoring.Default.md) 完成。项目各自维护唯一 `Properties/ScriptingApi.cs`；本层仅导出 Editor API。

## 验证与失败

`tests/rendering/Inno.Rendering.Assets.Authoring.Tests` 验证导入、图冻结、编译候选与 last-good。扩展 generation、Missing 与发布事务沿现有 Asset/TypeRegistry 协调器处理；不能为了目标编译绕过该事务。

## 当前源码公开 API 清单

以下仅列出当前程序集自己声明的 public/protected 契约；继承成员遵循所属基类页面。internal/private 实现不作为稳定公开 API。签名依据当前源码语义模型生成，行为、参数、异常与所有权说明同时以对应英文 XML 为准。

### `Inno.Rendering.Assets.Authoring.CompiledShaderArtifact`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.Authoring.CompiledShaderArtifact.CompiledShaderArtifact(string shaderName, string targetKey, Inno.Rendering.Assets.RenderShaderVariant variant, Inno.Rendering.ShaderInterface shaderInterface, System.Collections.Generic.IReadOnlyList<Inno.Rendering.Assets.Authoring.CompiledShaderPass> passes, System.ReadOnlySpan<byte> definitionData)`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L208) | Creates a compiled shader artifact. |
| [`Inno.Rendering.Assets.RenderShaderArtifact Inno.Rendering.Assets.Authoring.CompiledShaderArtifact.CreateRuntimeArtifact()`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L261) | Creates the source-free deployment artifact consumed by Editor preview sessions and Players. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Assets.Authoring.CompiledShaderPass> Inno.Rendering.Assets.Authoring.CompiledShaderArtifact.passes`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L253) | Gets compiled pass binaries. |
| [`Inno.Rendering.ShaderInterface Inno.Rendering.Assets.Authoring.CompiledShaderArtifact.shaderInterface`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L248) | Gets the manifest-derived binding contract. |
| [`string Inno.Rendering.Assets.Authoring.CompiledShaderArtifact.shaderName`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L233) | Gets the artist-facing shader name. |
| [`string Inno.Rendering.Assets.Authoring.CompiledShaderArtifact.targetKey`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L238) | Gets the stable target cache key. |
| [`Inno.Rendering.Assets.RenderShaderVariant Inno.Rendering.Assets.Authoring.CompiledShaderArtifact.variant`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L243) | Gets the static keyword variant. |
| [`Inno.Rendering.Assets.Authoring.CompiledShaderArtifact`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L183) | Contains an immutable target shader artifact and expected reflected interface. |

### `Inno.Rendering.Assets.Authoring.CompiledShaderPass`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.Authoring.CompiledShaderPass.CompiledShaderPass(Inno.Rendering.Assets.ShaderPassDefinition definition, System.Collections.Generic.IReadOnlyList<Inno.Rendering.Assets.Authoring.ShaderStageArtifact> stages, Inno.Rendering.ShaderInterface shaderInterface)`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L143) | Creates a compiled pass artifact. |
| [`Inno.Rendering.Assets.ShaderPassDefinition Inno.Rendering.Assets.Authoring.CompiledShaderPass.definition`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L159) | Gets the stable pass definition. |
| [`Inno.Rendering.ShaderInterface Inno.Rendering.Assets.Authoring.CompiledShaderPass.shaderInterface`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L177) | Gets the pass-local manifest binding contract. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Assets.Authoring.ShaderStageArtifact> Inno.Rendering.Assets.Authoring.CompiledShaderPass.stages`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L172) | Gets compiled target stages. |
| [`Inno.Rendering.Assets.Authoring.CompiledShaderPass`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L127) | Stores all compiled stages and state for one shader pass. |

### `Inno.Rendering.Assets.Authoring.IShaderCompilerToolchain`

| 当前声明 | 行为 |
| --- | --- |
| [`System.Threading.Tasks.ValueTask<Inno.Rendering.Assets.Authoring.ShaderStageToolResult> Inno.Rendering.Assets.Authoring.IShaderCompilerToolchain.CompileAsync(Inno.Rendering.Assets.Authoring.ShaderStageToolRequest request, System.Threading.CancellationToken cancellationToken)`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L377) | Generates and compiles one typed stage, including its frozen function modules and resource layout. |
| [`Inno.Rendering.Assets.Authoring.ShaderCompileTarget Inno.Rendering.Assets.Authoring.IShaderCompilerToolchain.CreateTarget(Inno.Rendering.GraphicsCapabilities capabilities, bool optimize = true, bool debugInformation = false)`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L397) | Creates a target supported by this toolchain and capability snapshot. |
| [`string Inno.Rendering.Assets.Authoring.IShaderCompilerToolchain.implementationId`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L358) | Gets the stable source implementation identity paired with this rendering adapter. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Rendering.Assets.Authoring.IShaderCompilerToolchain.supportedSourceLanguages`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L363) | Gets the explicit source-language identities accepted by this adapter's typed IR generator. |
| [`Inno.Rendering.Assets.Authoring.IShaderCompilerToolchain`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L352) | Defines a backend-owned target compiler used by the common Shader IR pipeline. |

### `Inno.Rendering.Assets.Authoring.ITextureTargetCompiler`

| 当前声明 | 行为 |
| --- | --- |
| [`System.Threading.Tasks.ValueTask<byte[]> Inno.Rendering.Assets.Authoring.ITextureTargetCompiler.CompileKtxAsync(System.IO.Stream source, Inno.Rendering.Assets.TextureColorSpace colorSpace, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/TextureTargetCompiler.cs#L34) | Compiles one source texture into an uncompressed KTX artifact with a complete mip chain. |
| [`Inno.Rendering.Assets.Authoring.ITextureTargetCompiler`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/TextureTargetCompiler.cs#L13) | Converts supported artist texture sources into a validated portable runtime container. |

### `Inno.Rendering.Assets.Authoring.ShaderArtifactSelection`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.Authoring.ShaderArtifactSelection.ShaderArtifactSelection(Inno.Rendering.Assets.Authoring.CompiledShaderArtifact? artifact, bool candidateSucceeded, bool usingLastGood, System.Collections.Generic.IReadOnlyList<Inno.Rendering.Assets.ShaderDiagnostic> diagnostics)`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderLastGoodStore.cs#L30) | Creates an artifact selection result. |
| [`Inno.Rendering.Assets.Authoring.CompiledShaderArtifact? Inno.Rendering.Assets.Authoring.ShaderArtifactSelection.artifact`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderLastGoodStore.cs#L46) | Gets the selected candidate or last-good artifact. |
| [`bool Inno.Rendering.Assets.Authoring.ShaderArtifactSelection.candidateSucceeded`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderLastGoodStore.cs#L51) | Gets whether the candidate replaced active state. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Assets.ShaderDiagnostic> Inno.Rendering.Assets.Authoring.ShaderArtifactSelection.diagnostics`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderLastGoodStore.cs#L61) | Gets candidate diagnostics. |
| [`bool Inno.Rendering.Assets.Authoring.ShaderArtifactSelection.usingLastGood`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderLastGoodStore.cs#L56) | Gets whether a previous artifact was preserved. |
| [`Inno.Rendering.Assets.Authoring.ShaderArtifactSelection`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderLastGoodStore.cs#L12) | Describes the artifact selected after evaluating a candidate compilation. |

### `Inno.Rendering.Assets.Authoring.ShaderCompilationResult`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.Authoring.ShaderCompilationResult.ShaderCompilationResult(Inno.Rendering.Assets.Authoring.CompiledShaderArtifact? artifact, System.Collections.Generic.IReadOnlyList<Inno.Rendering.Assets.ShaderDiagnostic> diagnostics)`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L323) | Creates a shader compilation result. |
| [`Inno.Rendering.Assets.Authoring.CompiledShaderArtifact? Inno.Rendering.Assets.Authoring.ShaderCompilationResult.artifact`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L335) | Gets the candidate artifact, or after failure. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Assets.ShaderDiagnostic> Inno.Rendering.Assets.Authoring.ShaderCompilationResult.diagnostics`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L340) | Gets validation and compiler diagnostics. |
| [`bool Inno.Rendering.Assets.Authoring.ShaderCompilationResult.succeeded`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L345) | Gets whether a complete candidate artifact was produced. |
| [`Inno.Rendering.Assets.Authoring.ShaderCompilationResult`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L311) | Returns a candidate artifact and structured diagnostics without mutating active state. |

### `Inno.Rendering.Assets.Authoring.ShaderCompileTarget`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.Authoring.ShaderCompileTarget.ShaderCompileTarget(string profileKey, Inno.Rendering.GraphicsCapabilities capabilities, bool optimize = true, bool debugInformation = false)`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L33) | Creates a shader compilation target. |
| [`Inno.Rendering.GraphicsCapabilities Inno.Rendering.Assets.Authoring.ShaderCompileTarget.capabilities`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L55) | Gets target renderer capabilities. |
| [`bool Inno.Rendering.Assets.Authoring.ShaderCompileTarget.debugInformation`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L65) | Gets whether shader debug information is emitted. |
| [`string Inno.Rendering.Assets.Authoring.ShaderCompileTarget.key`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L70) | Gets a stable target cache-key fragment. |
| [`bool Inno.Rendering.Assets.Authoring.ShaderCompileTarget.optimize`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L60) | Gets whether release optimization is enabled. |
| [`string Inno.Rendering.Assets.Authoring.ShaderCompileTarget.profileKey`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L50) | Gets the backend compiler-owned stable profile key. |
| [`Inno.Rendering.Assets.Authoring.ShaderCompileTarget`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L15) | Selects one renderer profile and compilation policy. |

### `Inno.Rendering.Assets.Authoring.ShaderCompiler`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.Authoring.ShaderCompiler.ShaderCompiler(Inno.Rendering.Assets.Authoring.IShaderCompilerToolchain toolchain)`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L419) | Creates a common IR compiler with one backend-owned target toolchain. |
| [`System.Threading.Tasks.ValueTask<Inno.Rendering.Assets.Authoring.ShaderCompilationResult> Inno.Rendering.Assets.Authoring.ShaderCompiler.CompileAsync(Inno.Rendering.Assets.ShaderDefinition definition, Inno.Rendering.Shaders.ShaderGraphProgramResult program, Inno.Rendering.Assets.Authoring.ShaderCompileTarget target, Inno.Rendering.Assets.RenderShaderVariant variant, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderGraphCompilation.cs#L145) | Compiles every graph-lowered pass through the adapter's typed stage interface. |
| [`System.Threading.Tasks.ValueTask<Inno.Rendering.Assets.Authoring.ShaderStageToolResult> Inno.Rendering.Assets.Authoring.ShaderCompiler.CompileAsync(Inno.Rendering.Shaders.ShaderIrStage stage, Inno.Rendering.Assets.Authoring.ShaderCompileTarget target, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderStageCompilation.cs#L121) | Compiles a typed stage through the configured adapter without an intermediate complete-source authoring asset. |
| [`System.Threading.Tasks.ValueTask<Inno.Rendering.Assets.Authoring.ShaderCompilationResult> Inno.Rendering.Assets.Authoring.ShaderCompiler.CompileGraphAsync(Inno.Rendering.Assets.ShaderAsset shader, Inno.Rendering.Assets.Authoring.ShaderCompileTarget target, Inno.Rendering.Assets.RenderShaderVariant variant, Inno.Extensibility.Types.TypeCatalog types, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context, Inno.Assets.IAssetArtifactLookup artifacts, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderGraphCompilation.cs#L50) | Captures an imported graph in the current owner generation, then starts native compilation without retaining its asset. |
| [`System.Threading.Tasks.ValueTask<Inno.Rendering.Assets.Authoring.ShaderCompilationResult> Inno.Rendering.Assets.Authoring.ShaderCompiler.CompileGraphAsync(System.ReadOnlyMemory<byte> artifact, Inno.Rendering.Assets.Authoring.ShaderCompileTarget target, Inno.Rendering.Assets.RenderShaderVariant variant, Inno.Extensibility.Types.TypeCatalog types, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderGraphCompilation.cs#L91) | Compiles an immutable import or preview candidate without reading or mutating a canonical Shader asset. |
| [`Inno.Rendering.Assets.Authoring.ShaderCompileTarget Inno.Rendering.Assets.Authoring.ShaderCompiler.CreateTarget(Inno.Rendering.GraphicsCapabilities capabilities, bool optimize = true, bool debugInformation = false)`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L439) | Creates a target supported by the configured backend compiler. |
| [`Inno.Rendering.Assets.Authoring.ShaderCompiler`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L408) | Compiles validated Shader IR through one target toolchain. |

### `Inno.Rendering.Assets.Authoring.ShaderFunctionAsset`

| 当前声明 | 行为 |
| --- | --- |
| [`int Inno.Rendering.Assets.Authoring.ShaderFunctionAsset.catalogOrder`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Importing/ShaderFunctionAsset.cs#L33) | Gets the stable ordering value inside the authoring catalog. |
| [`string Inno.Rendering.Assets.Authoring.ShaderFunctionAsset.catalogPath`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Importing/ShaderFunctionAsset.cs#L29) | Gets the author-declared catalog path used by Shader creation tools. |
| [`string[] Inno.Rendering.Assets.Authoring.ShaderFunctionAsset.exports`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Importing/ShaderFunctionAsset.cs#L21) | Gets the explicitly exported functions available to graph source-function nodes. |
| [`string Inno.Rendering.Assets.Authoring.ShaderFunctionAsset.implementationId`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Importing/ShaderFunctionAsset.cs#L25) | Gets the adapter implementation identity. |
| [`string Inno.Rendering.Assets.Authoring.ShaderFunctionAsset.languageId`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Importing/ShaderFunctionAsset.cs#L17) | Gets the configured source language identity. |
| [`Inno.Rendering.Assets.Authoring.ShaderFunctionAsset`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Importing/ShaderFunctionAsset.cs#L11) | Represents an authoring-only source-function library; it is never a complete GPU stage or runtime dependency. |

### `Inno.Rendering.Assets.Authoring.ShaderGraphArtifact`

| 当前声明 | 行为 |
| --- | --- |
| [`const string Inno.Rendering.Assets.Authoring.ShaderGraphArtifact.outputName`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderGraphArtifact.cs#L24) | Identifies the authoring-only graph and frozen source output. |
| [`static byte[] Inno.Rendering.Assets.Authoring.ShaderGraphArtifact.Capture(Inno.Core.Graphs.GraphDocument graph, Inno.Extensibility.Types.TypeCatalog types, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context, System.Func<System.Guid, string, byte[]> readSource, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken), System.Func<System.Guid, string, Inno.Core.Graphs.GraphDocument>? readGraph = null)`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderGraphArtifact.cs#L53) | Freezes an authored graph and its target-expanded function dependencies without publishing an asset. |
| [`static byte[] Inno.Rendering.Assets.Authoring.ShaderGraphArtifact.Encode(Inno.Core.Graphs.GraphDocument graph, System.Collections.Generic.IReadOnlyDictionary<Inno.Core.Graphs.GraphNodeId, byte[]> sources, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Graphs.GraphDocument? program = null)`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderGraphArtifact.cs#L182) | Captures one native immutable graph import candidate. |
| [`static string Inno.Rendering.Assets.Authoring.ShaderGraphArtifact.GetSemanticHash(System.ReadOnlySpan<byte> bytes, Inno.Core.Serialization.SerializationRegistry serialization)`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderGraphArtifact.cs#L145) | Fingerprints semantic graph records and every frozen source dependency, excluding node positions and reserved Editor metadata. |
| [`static Inno.Rendering.Shaders.ShaderGraphProgramResult Inno.Rendering.Assets.Authoring.ShaderGraphArtifact.Lower(System.ReadOnlySpan<byte> bytes, string implementationId, Inno.Rendering.Shaders.ShaderNodeCompilerRegistry nodes, Inno.Rendering.Shaders.ShaderSourceFrontendRegistry frontends, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context, System.Collections.Generic.IReadOnlyDictionary<string, string>? defines = null)`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderGraphArtifact.cs#L241) | Analyzes frozen functions and lowers the graph in the owner's active generation. |
| [`static byte[] Inno.Rendering.Assets.Authoring.ShaderGraphArtifact.Read(Inno.Rendering.Assets.ShaderAsset shader, Inno.Assets.IAssetArtifactLookup artifacts)`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderGraphArtifact.cs#L123) | Reads a retained immutable authoring snapshot through its explicit artifact owner. |
| [`static Inno.Rendering.Assets.ShaderDefinition Inno.Rendering.Assets.Authoring.ShaderGraphArtifact.ReadDefinition(System.ReadOnlySpan<byte> bytes, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderGraphArtifact.cs#L104) | Reads the target-expanded runtime interface from the same frozen candidate as its computations. |
| [`static Inno.Core.Graphs.GraphDocument Inno.Rendering.Assets.Authoring.ShaderGraphArtifact.ReadDocument(System.ReadOnlySpan<byte> bytes, Inno.Core.Serialization.SerializationRegistry serialization)`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderGraphArtifact.cs#L208) | Restores the imported graph for authoring or source export. |
| [`Inno.Rendering.Assets.Authoring.ShaderGraphArtifact`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderGraphArtifact.cs#L18) | Persists graph and frozen function inputs as an authoring artifact; Player export removes this payload. |

### `Inno.Rendering.Assets.Authoring.ShaderGraphSourceSnapshot`

| 当前声明 | 行为 |
| --- | --- |
| [`string Inno.Rendering.Assets.Authoring.ShaderGraphSourceSnapshot.contentHash`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Editing/ShaderGraphSourceStore.cs#L116) | Gets the source fingerprint used to reject external-edit conflicts. |
| [`Inno.Core.Graphs.GraphDocument Inno.Rendering.Assets.Authoring.ShaderGraphSourceSnapshot.document`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Editing/ShaderGraphSourceStore.cs#L112) | Gets a detached copy of authored graph records. |
| [`bool Inno.Rendering.Assets.Authoring.ShaderGraphSourceSnapshot.isReadOnly`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Editing/ShaderGraphSourceStore.cs#L120) | Gets whether the source belongs to an immutable installation mount. |
| [`Inno.Rendering.Assets.Authoring.ShaderGraphSourceSnapshot`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Editing/ShaderGraphSourceStore.cs#L96) | Contains detached graph source state, not a compiled or live runtime asset. |

### `Inno.Rendering.Assets.Authoring.ShaderGraphSourceStore`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.Authoring.ShaderGraphSourceStore.ShaderGraphSourceStore(Inno.Assets.Pipeline.AssetPipeline assets, Inno.Core.Serialization.SerializationRegistry serialization)`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Editing/ShaderGraphSourceStore.cs#L29) | Uses the authoritative asset mounts and native graph serializer. |
| [`Inno.Rendering.Assets.Authoring.ShaderGraphSourceSnapshot Inno.Rendering.Assets.Authoring.ShaderGraphSourceStore.Read(Inno.Assets.AssetPath path)`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Editing/ShaderGraphSourceStore.cs#L47) | Reads source directly, so a broken graph remains editable even when import has no successful artifact. |
| [`string Inno.Rendering.Assets.Authoring.ShaderGraphSourceStore.Save(Inno.Assets.AssetPath path, Inno.Core.Graphs.GraphDocument graph, string? expectedHash)`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Editing/ShaderGraphSourceStore.cs#L75) | Saves serializable graph records independently of import or native compilation success. |
| [`Inno.Rendering.Assets.Authoring.ShaderGraphSourceStore`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Editing/ShaderGraphSourceStore.cs#L14) | Reads and atomically saves the sole shader graph source format without requiring successful compilation. |

### `Inno.Rendering.Assets.Authoring.ShaderLastGoodStore`

| 当前声明 | 行为 |
| --- | --- |
| [`int Inno.Rendering.Assets.Authoring.ShaderLastGoodStore.Remove(System.Guid shaderId)`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderLastGoodStore.cs#L135) | Removes all CPU artifacts associated with one persistent shader. |
| [`Inno.Rendering.Assets.Authoring.ShaderArtifactSelection Inno.Rendering.Assets.Authoring.ShaderLastGoodStore.Select(System.Guid shaderId, string targetKey, Inno.Rendering.Assets.RenderShaderVariant variant, Inno.Rendering.Assets.Authoring.ShaderCompilationResult candidate)`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderLastGoodStore.cs#L91) | Commits a complete candidate or returns the current last-good artifact after failure. |
| [`Inno.Rendering.Assets.Authoring.ShaderLastGoodStore`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderLastGoodStore.cs#L67) | Atomically preserves last-good CPU shader artifacts by asset, target and variant. |

### `Inno.Rendering.Assets.Authoring.ShaderSourceBundle`

| 当前声明 | 行为 |
| --- | --- |
| [`const string Inno.Rendering.Assets.Authoring.ShaderSourceBundle.outputName`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderSourceBundle.cs#L19) | Identifies the authoring-only frozen function module output. |
| [`static System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderSourceImplementationRequest> Inno.Rendering.Assets.Authoring.ShaderSourceBundle.Decode(System.ReadOnlySpan<byte> bytes, string function, Inno.Core.Serialization.SerializationRegistry serialization, System.Collections.Generic.IReadOnlyDictionary<string, string>? defines = null)`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderSourceBundle.cs#L100) | Restores requests whose resolvers can access only the captured files and edges. |
| [`static byte[] Inno.Rendering.Assets.Authoring.ShaderSourceBundle.Encode(System.Collections.Generic.IReadOnlyDictionary<string, Inno.Rendering.Shaders.ShaderSourceModuleAnalysis> functions, Inno.Core.Serialization.SerializationRegistry serialization)`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderSourceBundle.cs#L55) | Captures every exported function, implementation and include resolution from a successful source import. |
| [`static byte[] Inno.Rendering.Assets.Authoring.ShaderSourceBundle.Read(Inno.Rendering.Assets.Authoring.ShaderFunctionAsset function, Inno.Assets.IAssetArtifactLookup artifacts)`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderSourceBundle.cs#L33) | Reads an immutable function snapshot through its explicit artifact owner. |
| [`Inno.Rendering.Assets.Authoring.ShaderSourceBundle`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderSourceBundle.cs#L13) | Persists only frozen function compilation inputs, never a parser, live asset, resolver or generated main function. |

### `Inno.Rendering.Assets.Authoring.ShaderSourceImportSettings`

| 当前声明 | 行为 |
| --- | --- |
| [`int Inno.Rendering.Assets.Authoring.ShaderSourceImportSettings.catalogOrder`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Importing/ShaderSourceImportSettings.cs#L36) | Gets or sets the stable ordering value of this library inside its authoring catalog group. |
| [`string Inno.Rendering.Assets.Authoring.ShaderSourceImportSettings.catalogPath`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Importing/ShaderSourceImportSettings.cs#L32) | Gets or sets the slash-delimited authoring catalog path used to group this library in creation UIs. |
| [`string[] Inno.Rendering.Assets.Authoring.ShaderSourceImportSettings.exports`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Importing/ShaderSourceImportSettings.cs#L20) | Gets or sets the explicitly exported function names. Unlisted helpers remain private to the source library. |
| [`string Inno.Rendering.Assets.Authoring.ShaderSourceImportSettings.implementationId`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Importing/ShaderSourceImportSettings.cs#L24) | Gets or sets the exact implementation key used by the selected rendering backend. |
| [`Inno.Rendering.Assets.Authoring.ShaderFunctionAsset[] Inno.Rendering.Assets.Authoring.ShaderSourceImportSettings.implementations`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Importing/ShaderSourceImportSettings.cs#L28) | Gets or sets alternate source implementations; all must expose the same public interface with unique backend IDs. |
| [`string Inno.Rendering.Assets.Authoring.ShaderSourceImportSettings.languageId`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Importing/ShaderSourceImportSettings.cs#L16) | Gets or sets the registered language identity; an empty selection is an import error. |
| [`Inno.Rendering.Assets.Authoring.ShaderSourceImportSettings`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Importing/ShaderSourceImportSettings.cs#L10) | Configures one source-function library through the standard asset import-settings sidecar. |

### `Inno.Rendering.Assets.Authoring.ShaderStageArtifact`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.Authoring.ShaderStageArtifact.ShaderStageArtifact(Inno.Rendering.ShaderStage stage, System.ReadOnlySpan<byte> bytes, Inno.Rendering.Assets.ShaderSourceLocation sourceLocation)`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L93) | Creates a stage artifact. |
| [`System.ReadOnlyMemory<byte> Inno.Rendering.Assets.Authoring.ShaderStageArtifact.bytes`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L116) | Gets immutable target binary bytes. |
| [`Inno.Rendering.Assets.ShaderSourceLocation Inno.Rendering.Assets.Authoring.ShaderStageArtifact.sourceLocation`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L121) | Gets the original source mapping. |
| [`Inno.Rendering.ShaderStage Inno.Rendering.Assets.Authoring.ShaderStageArtifact.stage`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L111) | Gets the compiled shader stage. |
| [`Inno.Rendering.Assets.Authoring.ShaderStageArtifact`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderCompilation.cs#L76) | Stores one immutable target stage binary. |

### `Inno.Rendering.Assets.Authoring.ShaderStageBinding`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.Authoring.ShaderStageBinding`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderStageCompilation.cs#L39) | Maps one logical binding to the adapter-generated native name, without exposing a GPU handle. |

### `Inno.Rendering.Assets.Authoring.ShaderStageToolRequest`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.Authoring.ShaderStageToolRequest`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderStageCompilation.cs#L21) | Supplies typed stage semantics and frozen source modules to a target compiler. |

### `Inno.Rendering.Assets.Authoring.ShaderStageToolResult`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.Authoring.ShaderStageToolResult.ShaderStageToolResult(System.ReadOnlySpan<byte> bytes, System.Collections.Generic.IEnumerable<Inno.Rendering.Assets.Authoring.ShaderStageBinding> bindings, System.Collections.Generic.IEnumerable<Inno.Rendering.Shaders.ShaderSourceDiagnostic> diagnostics)`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderStageCompilation.cs#L66) | Creates a candidate result from adapter-owned generation, compilation and diagnostics. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Assets.Authoring.ShaderStageBinding> Inno.Rendering.Assets.Authoring.ShaderStageToolResult.bindings`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderStageCompilation.cs#L87) | Gets the exact generated logical-to-native resource layout. |
| [`System.ReadOnlyMemory<byte> Inno.Rendering.Assets.Authoring.ShaderStageToolResult.bytes`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderStageCompilation.cs#L83) | Gets immutable target binary data, empty on failure. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Shaders.ShaderSourceDiagnostic> Inno.Rendering.Assets.Authoring.ShaderStageToolResult.diagnostics`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderStageCompilation.cs#L91) | Gets generation and native compiler diagnostics. |
| [`bool Inno.Rendering.Assets.Authoring.ShaderStageToolResult.succeeded`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderStageCompilation.cs#L95) | Gets whether a nonempty binary was compiled without error diagnostics. |
| [`Inno.Rendering.Assets.Authoring.ShaderStageToolResult`](../../src/services/rendering/Inno.Rendering.Assets.Authoring/Compilation/ShaderStageCompilation.cs#L49) | Returns a frozen typed-stage compilation candidate; failure never carries a usable artifact. |

## 项目依赖

- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Core.IO](../core/Inno.Core.IO.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Rendering.Shaders](Inno.Rendering.Shaders.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Core.Mathematics](../core/Inno.Core.Mathematics.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Assets](../assets/Inno.Assets.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Assets.Pipeline](../assets/Inno.Assets.Pipeline.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Rendering](Inno.Rendering.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Rendering.Assets](Inno.Rendering.Assets.md)：项目引用；公开签名可见性由语义边界检查确认。

共同 MSBuild 注入的 analyzer 与编译规则属于构建依赖，完整有效项目图记录在本轮验收证据中。
