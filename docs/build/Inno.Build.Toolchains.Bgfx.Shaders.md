# Inno.Build.Toolchains.Bgfx.Shaders

[Build 索引](README.md) · [Wiki 首页](../README.md) · [MSBuild Task](Inno.Build.Tasks.md) · [统一 CLI](Inno.Build.Cli.md)

## 职责与公开 API

Shader 图离线编译库，没有 Program。唯一 public 入口 `ShaderArtifactBuilder.Compile(assetRoot, shaderPath, platform, backend, outputFile)` 通过现有 Asset Pipeline、Shader IR、后端前端与 Artifact Codec 编译一个图。公开参数的目标平台来自 BGFX 工具链，GraphicsApi 属于中立 Rendering。没有 protected 扩展点。

```csharp
using Inno.Build.Toolchains.Bgfx.Shaders;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Rendering;

ShaderArtifactBuilder.Compile(assetRoot, "ImGui.ishader",
    BgfxShaderTargetPlatform.WindowsX64, GraphicsApi.Direct3D11, outputFile);
```

变量由调用者提供。MSBuild 内置 ImGui、Composition 和 OutputTransfer 使用同一个 CompileShaderTask；命令行使用统一 CLI 的 shader 命令。

## 失败、生命周期和热重载

新源导入或编译失败明确抛出 InvalidDataException，保留既有目标文件；只有完整产物成功才通过 AtomicFile 替换。临时 Asset/Module/诊断 owner 使用隔离 scratch，退出释放并清理。没有第二套 Shader 源编译旁路；此库不能进入 Player closure。

## 当前源码公开 API 清单

以下仅列出当前程序集自己声明的 public/protected 契约；继承成员遵循所属基类页面。internal/private 实现不作为稳定公开 API。签名依据当前源码语义模型生成，行为、参数、异常与所有权说明同时以对应英文 XML 为准。

### `Inno.Build.Toolchains.Bgfx.Shaders.ShaderArtifactBuilder`

| 当前声明 | 行为 |
| --- | --- |
| [`static void Inno.Build.Toolchains.Bgfx.Shaders.ShaderArtifactBuilder.Compile(string assetRoot, string shaderPath, Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetPlatform platform, Inno.Rendering.GraphicsApi backend, string outputFile, Inno.Build.Toolchains.Bgfx.Tools.ToolRunner? tools = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Shaders/ShaderArtifactBuilder.cs#L58) | Imports and compiles one shader graph for an explicitly selected graphics target. |
| [`static void Inno.Build.Toolchains.Bgfx.Shaders.ShaderArtifactBuilder.CompileGraphicsProgram(string assetRoot, string shaderPath, Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetPlatform platform, Inno.Rendering.GraphicsApi backend, string outputFile, Inno.Build.Toolchains.Bgfx.Tools.ToolRunner? tools = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Shaders/ShaderArtifactBuilder.cs#L103) | Compiles one raster graph into device-only program facts for an embedded adapter distribution. |
| [`Inno.Build.Toolchains.Bgfx.Shaders.ShaderArtifactBuilder`](../../build/toolchains/Inno.Build.Toolchains.Bgfx.Shaders/ShaderArtifactBuilder.cs#L26) | Compiles authored shader graphs through the standard import, IR and artifact pipeline. |

## 项目依赖

- [Inno.Build.Toolchains.Bgfx.Tools](Inno.Build.Toolchains.Bgfx.Tools.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Assets.Pipeline](../assets/Inno.Assets.Pipeline.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Core.Logging](../core/Inno.Core.Logging.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Core.IO](../core/Inno.Core.IO.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Extensibility.Modules](../extensibility/Inno.Extensibility.Modules.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Adapter.Modules.DotNet](../platform/Inno.Adapter.Modules.DotNet.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Adapter.Serialization.DotNet](../platform/Inno.Adapter.Serialization.DotNet.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Rendering.Assets](../rendering/Inno.Rendering.Assets.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Rendering.Assets.Authoring](../rendering/Inno.Rendering.Assets.Authoring.md)：项目引用；公开签名可见性由语义边界检查确认。

共同 MSBuild 注入的 analyzer 与编译规则属于构建依赖，完整有效项目图记录在本轮验收证据中。
