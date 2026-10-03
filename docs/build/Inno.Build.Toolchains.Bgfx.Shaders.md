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
