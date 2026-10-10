# Inno.Build.Toolchains.Bgfx.Shaders

[分类索引](README.md) · [Wiki 首页](../../README.md) · [MSBuild Task](../../build/Inno.Build.Tasks.md) · [统一 CLI](../../build/Inno.Build.Cli.md)

## 职责与公开 API

Shader 图离线编译库，没有 Program。唯一 public 入口 `ShaderArtifactBuilder.Compile(assetRoot, shaderPath, platform, backend, outputFile)` 通过现有 Asset Pipeline、Shader IR、后端前端与 Artifact Codec 编译一个图。公开参数的目标平台来自 BGFX 工具链，GraphicsApi 属于中立 Rendering。没有 protected 扩展点。

```csharp
using Inno.Build.Toolchains.Bgfx.Shaders;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Rendering;

ShaderArtifactBuilder.Compile(assetRoot, "ImGui.ishader",
    BgfxShaderTargetProfile.WindowsX64, GraphicsApi.Direct3D11, outputFile);
```

变量由调用者提供。MSBuild 内置 ImGui、Composition 和 OutputTransfer 使用同一个 CompileShaderTask；命令行使用统一 CLI 的 shader 命令。

## 失败、生命周期和热重载

新源导入或编译失败明确抛出 InvalidDataException，保留既有目标文件；只有完整产物成功才通过 AtomicFile 替换。临时 Asset/Module/诊断 owner 使用隔离 scratch，退出释放并清理。没有第二套 Shader 源编译旁路；此库不能进入 Player closure。

## 源码归属

当前唯一源码 owner：`backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Shaders/Inno.Build.Toolchains.Bgfx.Shaders.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Build.Toolchains.Bgfx.Shaders.ShaderArtifactBuilder`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.Bgfx.Shaders.ShaderArtifactBuilder`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Shaders/ShaderArtifactBuilder.cs#L26) | Compiles authored shader graphs through the standard import, IR and artifact pipeline. |
| [`static void Inno.Build.Toolchains.Bgfx.Shaders.ShaderArtifactBuilder.Compile(string assetRoot, string shaderPath, Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile platform, Inno.Rendering.GraphicsApi backend, string outputFile, Inno.Build.Toolchains.Bgfx.Tools.ToolRunner? tools = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Shaders/ShaderArtifactBuilder.cs#L58) | Imports and compiles one shader graph for an explicitly selected graphics target. |
| [`static void Inno.Build.Toolchains.Bgfx.Shaders.ShaderArtifactBuilder.CompileGraphicsProgram(string assetRoot, string shaderPath, Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile platform, Inno.Rendering.GraphicsApi backend, string outputFile, Inno.Build.Toolchains.Bgfx.Tools.ToolRunner? tools = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx.Shaders/ShaderArtifactBuilder.cs#L103) | Compiles one raster graph into device-only program facts for an embedded adapter distribution. |

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
