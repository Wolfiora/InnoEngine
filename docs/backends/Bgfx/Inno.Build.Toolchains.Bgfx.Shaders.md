# Inno.Build.Toolchains.Bgfx.Shaders

[分类索引](README.md) · [Wiki 首页](../../README.md) · [MSBuild Task](../../build/Inno.Build.Tasks.md) · [统一 CLI](../../build/Inno.Build.Cli.md)

## 职责与公开 API

Shader 图离线编译库，没有 Program。唯一 public 入口 `ShaderArtifactBuilder.Compile(assetRoot, shaderPath, platform, backend, outputFile)` 通过现有 Asset Pipeline、Shader IR、后端前端与 Artifact Codec 编译一个图。公开参数的目标平台来自 BGFX 工具链，GraphicsApi 属于中立 Rendering。没有 protected 扩展点。

```csharp
using System.Threading;
using Inno.Build.Toolchains.Bgfx.Shaders;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Rendering;

static void CompileGraph(
    string assetRoot,
    string sourceLocalShaderPath,
    BgfxShaderTargetProfile target,
    GraphicsApi graphicsApi,
    string outputFile,
    ToolRunner tools,
    CancellationToken cancellationToken
) {
    ShaderArtifactBuilder.Compile(assetRoot, sourceLocalShaderPath,
        target, graphicsApi, outputFile, tools, cancellationToken);
}
```

变量由调用者提供。MSBuild 内置 ImGui、Composition 和 OutputTransfer 使用同一个 CompileShaderTask；命令行使用统一 CLI 的 shader 命令。

## 失败、生命周期和热重载

新源导入或编译失败明确抛出 InvalidDataException，保留既有目标文件；只有完整产物成功才通过 AtomicFile 替换。临时 Asset/Module/诊断 owner 使用隔离 scratch，退出释放并清理。没有第二套 Shader 源编译旁路；此库不能进入 Player closure。

## 源码归属

当前唯一源码 owner：`backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Shaders/Inno.Build.Toolchains.Bgfx.Shaders.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Build.Toolchains.Bgfx.Shaders.ShaderArtifactBuilder`

<!-- api: T:Inno.Build.Toolchains.Bgfx.Shaders.ShaderArtifactBuilder a5d8b19a7c506ad4ab9ad6bf346c6015e26169f35209da55cd23c68795e705de -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.Shaders.ShaderArtifactBuilder.Compile(System.String,System.String,Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile,Inno.Rendering.GraphicsApi,System.String,Inno.Build.Toolchains.Bgfx.Tools.ToolRunner,System.Threading.CancellationToken) 11bf1f8b6c606817cfc27259cbc77a0c727af73eb2c94a21fadd142a89e16b30 -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.Shaders.ShaderArtifactBuilder.CompileGraphicsProgram(System.String,System.String,Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile,Inno.Rendering.GraphicsApi,System.String,Inno.Build.Toolchains.Bgfx.Tools.ToolRunner,System.Threading.CancellationToken) 6d9d3f3bafd0e0957317e4c573176bba5db831f4d56f4f361fa105f23f5d34a4 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2a75972b6e07efdb"></a>[`public static void Inno.Build.Toolchains.Bgfx.Shaders.ShaderArtifactBuilder.Compile(string assetRoot, string shaderPath, Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile platform, Inno.Rendering.GraphicsApi backend, string outputFile, Inno.Build.Toolchains.Bgfx.Tools.ToolRunner? tools = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Shaders/ShaderArtifactBuilder.cs#L58) | Imports and compiles one shader graph for an explicitly selected graphics target.<br>`assetRoot`：The directory containing shader authoring sources.<br>`shaderPath`：The source-local shader graph path.<br>`platform`：The native compiler's target platform.<br>`backend`：The graphics API for the compiled artifact.<br>`outputFile`：The artifact destination, replaced atomically after successful compilation.<br>`tools`：The host-selected executable distribution, or null for the application's deployed tools.<br>`cancellationToken`：Cancels import and native compilation before replacing the destination artifact. | — | `System.IO.InvalidDataException`：Import or shader compilation fails.<br>`System.ArgumentException`：A required authoring path or output path is blank. |
| <a id="api-3ec9e20a7298d587"></a>[`public static void Inno.Build.Toolchains.Bgfx.Shaders.ShaderArtifactBuilder.CompileGraphicsProgram(string assetRoot, string shaderPath, Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile platform, Inno.Rendering.GraphicsApi backend, string outputFile, Inno.Build.Toolchains.Bgfx.Tools.ToolRunner? tools = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Shaders/ShaderArtifactBuilder.cs#L103) | Compiles one raster graph into device-only program facts for an embedded adapter distribution.<br>`assetRoot`：The directory containing the graph and its source dependencies.<br>`shaderPath`：The source-local graph whose sole raster pass becomes the device program.<br>`platform`：The compiler's native target platform.<br>`backend`：The graphics API for the program binaries.<br>`outputFile`：The device artifact destination, atomically replaced after successful compilation.<br>`tools`：The host-selected executable distribution, or null for the application's deployed tools.<br>`cancellationToken`：Cancels import and native compilation before replacing the destination artifact. | — | `System.ArgumentException`：A required authoring or output path is blank.<br>`System.IO.InvalidDataException`：Compilation fails or the graph does not define exactly one raster program. |
| <a id="api-3deca2c9bb611ecf"></a>[`public static class Inno.Build.Toolchains.Bgfx.Shaders.ShaderArtifactBuilder`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Shaders/ShaderArtifactBuilder.cs#L26) | Compiles authored shader graphs through the standard import, IR and artifact pipeline. | — | — |

## 项目依赖

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
