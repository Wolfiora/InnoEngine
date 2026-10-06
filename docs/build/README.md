# Build API

[Wiki 首页](../README.md) · [Runtime](../runtime/README.md) · [Native](../native/README.md)

| 项目 | 职责 |
| --- | --- |
| [Inno.Build.Managed](Inno.Build.Managed.md) | 独立托管部署契约与开放 publisher catalog |
| [Inno.Build.Managed.DotNet](Inno.Build.Managed.DotNet.md) | CoreCLR、Mono Wasm/ AOT、NativeAOT 发布 |
| [Inno.Build](Inno.Build.md) | Profile、request/result、Plugin/Game pipeline 与内部 staging stages |
| [Inno.Build.Platform.Browser](Inno.Build.Platform.Browser.md) | WebGL 2 内容编译与静态站点布局 |
| [Inno.Build.Platform.MacOS](Inno.Build.Platform.MacOS.md) | macOS ARM64 target artifact 与 app bundle |
| [Inno.Build.Platform.Windows](Inno.Build.Platform.Windows.md) | Windows x64 target artifact 与 portable application directory |
| [Inno.Build.Cli](Inno.Build.Cli.md) | 唯一工具进程入口，组合构建、清理、生成、发布与验证 |
| [Inno.Build.Tasks](Inno.Build.Tasks.md) | MSBuild 编译和发布的薄适配层 |
| [Native binding generation](../native/BindingGeneration.md) | 各 Native 项目自己的单文件 binding 生成与检查入口 |
| [Inno.Build.SupportPacks](Inno.Build.SupportPacks.md) | 生产 source-independent Player Support Pack |
| [Inno.Build.SupportPacks.Core](Inno.Build.SupportPacks.Core.md) | 可嵌入 Export 的缺包供给与原子发布实现 |
| [Inno.Build.Toolchains](Inno.Build.Toolchains.md) | toolchain layout、process environment 与 artifact copy |
| [Inno.Build.Toolchains.Host](Inno.Build.Toolchains.Host.md) | 显式 checkout 下的桌面运行时与 Editor Native 构建组合 |
| [Inno.Build.Toolchains.Bgfx](Inno.Build.Toolchains.Bgfx.md) | BGFX 原生构建库 |
| [Inno.Build.Toolchains.Browser](Inno.Build.Toolchains.Browser.md) | WebAssembly 目标生成、SDK 解析与原生构建库 |
| [Inno.Build.Toolchains.Bgfx.Tools](Inno.Build.Toolchains.Bgfx.Tools.md) | shaderc/texturec 与目标内容编译 |
| [Inno.Build.Toolchains.Bgfx.Shaders](Inno.Build.Toolchains.Bgfx.Shaders.md) | 离线图编译及内置 ImGui 预编译产物 |
| [Inno.Build.Toolchains.Sdl3](Inno.Build.Toolchains.Sdl3.md) | SDL3 原生构建库 |
| [Inno.Build.Toolchains.MiniAudio](Inno.Build.Toolchains.MiniAudio.md) | miniaudio 原生构建库 |
| [Inno.Build.Toolchains.ImGui](Inno.Build.Toolchains.ImGui.md) | cimgui 原生构建库 |
| [Inno.Build.Toolchains.ImGuizmo](Inno.Build.Toolchains.ImGuizmo.md) | cimguizmo 原生构建库 |
| [Inno.Build.Toolchains.Text](Inno.Build.Toolchains.Text.md) | FreeType/HarfBuzz Text bridge 原生构建库 |
| [Inno.Build.Toolchains.UI](Inno.Build.Toolchains.UI.md) | RmlUi bridge 原生构建库 |

Game Build 固定执行 Validate → Support Pack 供给与校验 → Combined Snapshot → Scripts/Target Artifacts → Content Pack → Managed Deployment → Platform Package → Atomic Commit。Editor Exporting 只调用该 API，不拥有构建机制。

[Inno.Build.Composition](Inno.Build.Composition.md)：Editor/CLI/MSBuild 共用的内置构建组合。
