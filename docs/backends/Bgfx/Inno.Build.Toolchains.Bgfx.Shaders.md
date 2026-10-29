# Inno.Build.Toolchains.Bgfx.Shaders

[分类索引](README.md) · [Wiki 首页](../../README.md) · [MSBuild Task](../../build/Inno.Build.Tasks.md) · [统一 CLI](../../build/Inno.Build.Cli.md)

## 职责与公开 API

Shader 图离线编译库，没有 Program。公开组合边界为 `ShaderBuildInputs` 捕获/校验及 `ShaderArtifactBuilder.Compile` / `CompileGraphicsProgram` 通过现有 Asset Pipeline、Shader IR、后端前端与 Artifact Codec 编译一个图。公开参数的目标平台来自 BGFX 工具链，GraphicsApi 属于中立 Rendering。没有 protected 扩展点。

```csharp
using System.Threading;
using Inno.Build.Toolchains.Bgfx.Shaders;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Rendering;
using Inno.Core.IO;

static void CompileGraph(
    string assetRoot,
    ProcessOwnedDirectory workspace,
    string sourceLocalShaderPath,
    BgfxShaderTargetProfile target,
    GraphicsApi graphicsApi,
    string outputFile,
    ToolRunner tools,
    CancellationToken cancellationToken
) {
    ShaderBuildInputs inputs = ShaderBuildInputs.Capture(assetRoot, cancellationToken);
    ShaderArtifactBuilder.Compile(inputs, workspace, sourceLocalShaderPath,
        target, graphicsApi, outputFile, tools, cancellationToken);
}
```

变量由调用者提供。MSBuild 内置 ImGui、Composition 和 OutputTransfer 使用同一个 CompileShaderTask；命令行使用统一 CLI 的 shader 命令。

`ShaderBuildInputs.Capture` 固定真实源文件和永久 `.imeta` 输入；临时 atomic publication、派生 `.abin` 和 source policy 噪声不参与。Task 的指纹和编译器消费同一个捕获对象。调用方通过 `StandardBuildCaches.CreateShaderWorkspaceAsync` 持有已登记的 Core 进程目录，编译前真实复制到该操作 owner，导入只修改副本；复制后和发布前重新核对完整清单及内容，新增、删除和保留时间戳的修改均会拒绝发布。输入声明复用中立 `NativeBuildInput` / `NativeBuildFingerprint`；不削弱通用 Native 输入校验。

## 失败、生命周期和热重载

新源导入或编译失败明确抛出 InvalidDataException，保留既有目标文件；只有完整产物成功才通过 AtomicFile 替换。临时 Asset/Module/诊断 owner 使用隔离 scratch，退出释放并清理。没有第二套 Shader 源编译旁路；此库不能进入 Player closure。

## 源码归属

当前唯一源码 owner：`backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Shaders/Inno.Build.Toolchains.Bgfx.Shaders.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

此清单对应当前 SDK 求值与源码符号；内部类型不构成公开 API。

<!-- api: T:Inno.Build.Toolchains.Bgfx.Shaders.ShaderArtifactBuilder a5d8b19a7c506ad4ab9ad6bf346c6015e26169f35209da55cd23c68795e705de -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.Shaders.ShaderArtifactBuilder.Compile(Inno.Build.Toolchains.Bgfx.Shaders.ShaderBuildInputs,Inno.Core.IO.ProcessOwnedDirectory,System.String,Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile,Inno.Rendering.GraphicsApi,System.String,Inno.Build.Toolchains.Bgfx.Tools.ToolRunner,System.Threading.CancellationToken) 3787bb36a2eb2002beff7ef87ce91292c4f4e45754d2acf573210a5a647e559c -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.Shaders.ShaderArtifactBuilder.CompileGraphicsProgram(Inno.Build.Toolchains.Bgfx.Shaders.ShaderBuildInputs,Inno.Core.IO.ProcessOwnedDirectory,System.String,Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile,Inno.Rendering.GraphicsApi,System.String,Inno.Build.Toolchains.Bgfx.Tools.ToolRunner,System.Threading.CancellationToken) 71bc0cd4e5bc90846804f2f2a39b44b1d318c671e3ad23c61ee5fd0dbb06c0af -->
<!-- api: T:Inno.Build.Toolchains.Bgfx.Shaders.ShaderBuildInputs 7cfc7e0ec13e67527cda64dd77058c4e5a036c45191f693c7d9751e902516444 -->
<!-- api: P:Inno.Build.Toolchains.Bgfx.Shaders.ShaderBuildInputs.fingerprint 3fc62b97490b39c1882cd3955cecb80627b2b6caa49cc0d4ce110e97bad184a6 -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.Shaders.ShaderBuildInputs.Capture(System.String,System.Threading.CancellationToken) eb33c778ba3c875f959291def070da8836807583ab1bafd2bfe06863c8842d15 -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.Shaders.ShaderBuildInputs.VerifyUnchanged(System.Threading.CancellationToken) b67dcce96bacd39df5d109d165b40eea8eb4cd32654a9cedd05f60359630aabb -->

| API 与源码 | 契约与参数 | 返回值 | 重要异常 |
| --- | --- | --- | --- |
| <a id="api-3deca2c9bb611ecf"></a>[`public static class Inno.Build.Toolchains.Bgfx.Shaders.ShaderArtifactBuilder`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Shaders/ShaderArtifactBuilder.cs#L26) | Compiles authored shader graphs through the standard import, IR and artifact pipeline. | — | — |
| <a id="api-cfe60052dff10b65"></a>[`public static void Inno.Build.Toolchains.Bgfx.Shaders.ShaderArtifactBuilder.Compile(Inno.Build.Toolchains.Bgfx.Shaders.ShaderBuildInputs inputs, Inno.Core.IO.ProcessOwnedDirectory workspace, string shaderPath, Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile platform, Inno.Rendering.GraphicsApi backend, string outputFile, Inno.Build.Toolchains.Bgfx.Tools.ToolRunner? tools = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Shaders/ShaderArtifactBuilder.cs#L70) | Imports and compiles one shader graph for an explicitly selected graphics target.<br>`inputs`: The operation's fixed source and permanent import-setting identity, copied before import.<br>`workspace`: A fresh caller-owned operation directory retained until compilation returns. The caller releases it after all consumers finish.<br>`shaderPath`: The source-local shader graph path.<br>`platform`: The native compiler's target platform.<br>`backend`: The graphics API for the compiled artifact.<br>`outputFile`: The artifact destination, replaced atomically after successful compilation.<br>`tools`: The host-selected executable distribution, or null for the application's deployed tools.<br>`cancellationToken`: Cancels import and native compilation before replacing the destination artifact. | — | `System.IO.InvalidDataException`: Import or shader compilation fails.<br>`System.ArgumentException`: A required shader path or output path is blank.<br>`System.ArgumentNullException`: The fixed input identity or workspace is null.<br>`System.InvalidOperationException`: The borrowed workspace has been released or is not empty.<br>`System.OperationCanceledException`: Import, copying, verification or compilation was canceled before publication. |
| <a id="api-e8abbeb67847dfa7"></a>[`public static void Inno.Build.Toolchains.Bgfx.Shaders.ShaderArtifactBuilder.CompileGraphicsProgram(Inno.Build.Toolchains.Bgfx.Shaders.ShaderBuildInputs inputs, Inno.Core.IO.ProcessOwnedDirectory workspace, string shaderPath, Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile platform, Inno.Rendering.GraphicsApi backend, string outputFile, Inno.Build.Toolchains.Bgfx.Tools.ToolRunner? tools = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Shaders/ShaderArtifactBuilder.cs#L129) | Compiles one raster graph into device-only program facts for an embedded adapter distribution.<br>`inputs`: The operation's fixed source and permanent import-setting identity, copied before import.<br>`workspace`: A fresh caller-owned operation directory retained until compilation returns. The caller releases it after all consumers finish.<br>`shaderPath`: The source-local graph whose sole raster pass becomes the device program.<br>`platform`: The compiler's native target platform.<br>`backend`: The graphics API for the program binaries.<br>`outputFile`: The device artifact destination, atomically replaced after successful compilation.<br>`tools`: The host-selected executable distribution, or null for the application's deployed tools.<br>`cancellationToken`: Cancels import and native compilation before replacing the destination artifact. | — | `System.ArgumentException`: A required shader path or output path is blank.<br>`System.IO.InvalidDataException`: Compilation fails or the graph does not define exactly one raster program.<br>`System.ArgumentNullException`: The fixed input identity or workspace is null.<br>`System.InvalidOperationException`: The borrowed workspace has been released or is not empty.<br>`System.OperationCanceledException`: Import, copying, verification or compilation was canceled before publication. |
| <a id="api-4cb33f02a1a0a13d"></a>[`public sealed class Inno.Build.Toolchains.Bgfx.Shaders.ShaderBuildInputs`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Shaders/ShaderBuildInputs.cs#L20) | Fixes the complete authored shader file inventory and content identity for one compilation operation. | — | — |
| <a id="api-07aa760ef7bef009"></a>[`public string Inno.Build.Toolchains.Bgfx.Shaders.ShaderBuildInputs.fingerprint { get; }`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Shaders/ShaderBuildInputs.cs#L40) | Gets the complete logical directory, file and content identity used by the enclosing build. | — | — |
| <a id="api-6eb57044f0a83792"></a>[`public static Inno.Build.Toolchains.Bgfx.Shaders.ShaderBuildInputs Inno.Build.Toolchains.Bgfx.Shaders.ShaderBuildInputs.Capture(string assetRoot, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Shaders/ShaderBuildInputs.cs#L63) | Captures permanent authoring files and their import metadata without consuming temporary publications.<br>`assetRoot`: The existing physical authoring root, containing no source links.<br>`cancellationToken`: Cancels discovery and complete content hashing. | A fixed input inventory and content identity; physical source files remain owned by their author. | `System.ArgumentException`: The source root is blank.<br>`System.IO.IOException`: A source cannot be read, does not exist, or crosses a link.<br>`System.OperationCanceledException`: Capture was canceled. |
| <a id="api-943fa848a68bbb21"></a>[`public void Inno.Build.Toolchains.Bgfx.Shaders.ShaderBuildInputs.VerifyUnchanged(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Shaders/ShaderBuildInputs.cs#L87) | Rechecks the complete current inventory and bytes against this operation's captured identity.<br>`cancellationToken`: Cancels discovery and complete content verification. | — | `System.IO.InvalidDataException`: A source or permanent metadata file was added, removed or changed.<br>`System.IO.IOException`: The source tree cannot be safely inspected or read.<br>`System.OperationCanceledException`: Verification was canceled. |

## 项目依赖

- [Inno.Build.Toolchains](../../build/Inno.Build.Toolchains.md)：实现依赖；复用内容指纹机制，输入文件清单不越过编译库边界。

- [Inno.Build.Toolchains.Bgfx.Tools](Inno.Build.Toolchains.Bgfx.Tools.md)：公开引用边界由实际签名核对。
- [Inno.Assets.Pipeline](../../assets/Inno.Assets.Pipeline.md)：公开引用边界由实际签名核对。
- [Inno.Core.Diagnostics](../../core/Inno.Core.Diagnostics.md)：公开引用边界由实际签名核对。
- [Inno.Core.Logging](../../core/Inno.Core.Logging.md)：公开引用边界由实际签名核对。
- [Inno.Core.IO](../../core/Inno.Core.IO.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Modules](../../extensibility/Inno.Extensibility.Modules.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Modules.DotNet](../DotNet/Inno.Adapter.Modules.DotNet.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Serialization.DotNet](../DotNet/Inno.Adapter.Serialization.DotNet.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
- [Inno.Rendering.Assets](../../rendering/Inno.Rendering.Assets.md)：公开引用边界由实际签名核对。
- [Inno.Rendering.Assets.Authoring](../../rendering/Inno.Rendering.Assets.Authoring.md)：公开引用边界由实际签名核对。
