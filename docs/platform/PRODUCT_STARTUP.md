# 启动 Editor、构建与发布游戏

[平台索引](README.md) · [Wiki 首页](../README.md) · [开发入口](../entry/README.md)

<a id="from-zero"></a>

## 从零开始

开发入口支持 WindowsX64 和 MacOSArm64。先安装下表工具；入口不会自动安装系统 SDK。`global.json` 固定 .NET SDK 10.0.402，并允许同一 feature band 的稳定补丁。引擎与项目脚本统一使用 C# 14。

| 宿主 | 首次准备 |
| --- | --- |
| WindowsX64 | Git、x64 .NET 10 SDK、支持 .NET 10 的 Rider 或 Visual Studio 2026。Visual Studio Installer 安装“使用 C++ 的桌面开发”，包含 MSVC v143 x64/x86 工具、Windows SDK 和 C++ CMake 工具。仅安装 Rider 或 .NET SDK 不包含这些原生工具。 |
| MacOSArm64 | Git、原生 ARM64 .NET 10 SDK、原生 Apple Silicon Rider、Xcode Command Line Tools 或完整 Xcode，以及 PATH 中的 CMake。使用 `xcode-select --install` 安装命令行工具，首次使用接受相应许可；工具链通过 `xcrun` 选择 clang 和 macOS SDK，通过 PATH 查找 make、cmake。 |

### WindowsX64

在 PowerShell 中：

```powershell
git clone --branch kernel --recurse-submodules https://github.com/Wolfiora/InnoEngine.git
cd InnoEngine
dotnet --version
```

Rider 打开 `InnoEngine.sln`，选择 **Debug / Any CPU**，选择 `entry/InnoEngine.Editor` 的 **InnoEngine.Editor** 项目并 Run。Rider 自动 Restore、构建入口和 WindowsX64 Editor 依赖；没有传游戏 Project 时出现目录选择窗口。

### MacOSArm64

在终端中：

```sh
git clone --branch kernel --recurse-submodules https://github.com/Wolfiora/InnoEngine.git
cd InnoEngine
dotnet --info
xcrun --find clang
xcrun --sdk macosx --show-sdk-path
cmake --version
```

确认 `dotnet --info` 的 SDK 宿主 RID 为 `osx-arm64`，不要从 Rosetta/x64 .NET SDK 启动。Rider 打开同一个 `InnoEngine.sln`，选择 **Debug / Any CPU**，运行 **InnoEngine.Editor**，入口只引用 MacOSArm64 Editor。macOS 的这些步骤来自当前工程与工具链配置；本轮未进行 macOS 实机启动验收。

两平台使用同一个启动命令，也可完全通过终端构建并启动：

```sh
dotnet run --project entry/InnoEngine.Editor/InnoEngine.Editor.csproj
```

以上 `--branch kernel` 用于取得当前整改代码；合并后使用实际包含这些改动的分支。已经 clone 但未初始化子模块时，在仓库根目录执行 `git submodule update --init --recursive`。

### 两个日常入口

日常启动项目只有：

| 项目 | 操作 |
| --- | --- |
| **InnoEngine.Build** | 默认准备当前宿主 Editor 支持产物；可显式构建或发布。 |
| **InnoEngine.Editor** | 正常构建当前宿主依赖并启动；无 Project 参数时选择项目目录。 |

Rider 或 VS 直接选择 entry 中的同名可执行项目。项目使用正常 ProjectReference；不需要共享 .run 或外部脚本，也不需要先运行 Build。个人 IDE 配置不由源码修改。

BGCS 通过固定版本 NuGet 包恢复；Native 第三方源码位于 extern，clone 时包含递归子模块。入口不安装系统 SDK。

首次运行需要网络恢复 NuGet，编译本机 Native 支持及生成必要绑定，然后编译 C# 依赖并部署。等待构建成功后才会打开窗口；这不是已经准备好的发行包。以后正常运行复用有效支持缓存，并由 SDK 增量编译修改过的代码。启动 Windows/macOS Editor 不需要安装 Browser 的 wasm-tools workload，也不需要先运行 InnoEngine.Build。

## 日常操作

```sh
dotnet run --project entry/InnoEngine.Build/InnoEngine.Build.csproj
dotnet run --project entry/InnoEngine.Editor/InnoEngine.Editor.csproj
dotnet run --project entry/InnoEngine.Editor/InnoEngine.Editor.csproj -- /absolute/MyGame
```

引擎 C# 源码修改后直接再次构建或运行 Editor，SDK 增量编译受影响项目。游戏脚本和资产修改由 Editor 导入、编译与 reload 处理。没有传项目目录时弹出目录选择窗口，取消正常退出。

Debug/Release 分别增量构建并启动：

```sh
dotnet run --configuration Release --project entry/InnoEngine.Build/InnoEngine.Build.csproj
dotnet run --configuration Release --project entry/InnoEngine.Editor/InnoEngine.Editor.csproj
```

Build 的 `--configuration Release` 可以覆盖所构建产品的配置；Editor 跟随其入口编译配置。支持产物按目标与配置隔离。

## 显式清理

```sh
dotnet run --project entry/InnoEngine.Build/InnoEngine.Build.csproj -- editor --clean
dotnet run --project entry/InnoEngine.Build/InnoEngine.Build.csproj -- clean --scope product --dry-run
```

第一条清理当前选定 Editor 工作状态后构建。第二条只预览。默认 Build 不清理。

产品清理先检查目标与 SDK，再取得 owner 保护；活动 Editor 阻止对应产品被删除。其他目标、Native/Binding 缓存、有效 Support Pack、游戏输出、日志和全局 SDK 缓存保留。Ctrl+C 取消后不会提交半成品。

缓存清理仅处理已登记 owner 的可退休产物。`clean --scope cache --target windows-x64 --dry-run` 预览该目标；不传 target 检查所有 owner。共享宿主工具及未选目标的依赖仍受保护。实际验证状态见[问题台账](../architecture/CURRENT_ISSUES.md)。

## 支持状态与 not built

| 宿主 | Editor / 默认 Build |
| --- | --- |
| Windows x64 | 路由至 `Inno.Editor.WindowsX64` 产品库。 |
| macOS ARM64 | 路由至 `Inno.Editor.MacOSArm64` 产品库；实机验证另行记录。 |
| 其他 CPU/平台 | 未注册的产品明确拒绝。 |

默认 Solution Debug/Release 正常构建共同源码、工具和适用测试；当前宿主 Editor 通过 entry 的条件项目引用构建。其他宿主产品和未选择的 Player 不参与本次 Editor 构建。**not built** 表示未选入当前构建；没有修改而增量跳过表示已经最新。SourceDebug/SourceRelease 保留源码验证用途，不构建 Editor entry。可移植的其他目标构建规则按发行组合正常编译，不触发对应 Native SDK。

平台 Editor 与 CLI 是应用库；Player 项目是发布源码输入。最终 Player Exe 由平台模板生成，包含游戏代码、内容和匹配的 Native 产物，不作为日常 IDE 启动项目。

## Support Pack 与游戏发布

```sh
dotnet run --project entry/InnoEngine.Build/InnoEngine.Build.csproj -- support-pack --target windows-x64
dotnet run --project entry/InnoEngine.Build/InnoEngine.Build.csproj -- game --tools-target windows-x64 --project /absolute/MyGame --target windows-x64 --deployment coreclr --startup-scene Scenes/Main.iscene --output artifacts/games
```

Pack 是目标编译、链接与运行输入，不包含游戏内容。可单独准备，也可由游戏发布获取。Windows 的 CoreCLR / NativeAOT 发布已有实际通过记录；其他目标、完整构建成本及设备检查以[当前验收](../architecture/CURRENT_ACCEPTANCE.md)为准，不能由一个目标推断全部通过。

macOS 产品目标为 `macos-arm64`。Browser 目标为 `browser-wasm`，部署选 `mono-wasm` 或 `mono-wasm-aot`，实际发布需要匹配的 workload 和 Emscripten。网页通过 HTTP 服务运行，不能把 Browser 源码库当作独立网页游戏启动。

## Native 文件在哪里

| 内容 | 位置与 owner |
| --- | --- |
| 第三方源码 | extern，构建不修改它。 |
| 可复用工作区 | backend Toolchain 的 `obj/native/<target>/<workspaceIdentity>`。 |
| 校验后产物 | `artifacts/native/<component>/<target>/<artifactFingerprint>`。 |
| 生成绑定 | 当前位于 `artifacts/bindings/<component>/<target>/<generationFingerprint>/`；HostSource 位于同项目的 `Generated/`。 |
| Pack | `artifacts/support-packs/<target>/<contentFingerprint>`。 |
| Editor 部署 | 平台项目 SDK 输出，再由 ProjectReference 传递到 `entry/InnoEngine.Editor/bin/<Configuration>/net10.0/`；入口直接运行此目录中的程序集和 native 文件。 |
| Windows 短路径 | Temp/InnoTools 下由工具 owner 建立的工作区别名。 |

短路径 junction 不代表另一份完整 Native 缓存。正常结束删除本次别名，实际工作区继续复用；不能递归删除 junction 来“腾出”对应工作区空间。源码、工作区和成品缓存各有独立职责，不使用混合 .libs 目录。

最终用户运行发布后的 Player 时不执行开发入口、MSBuild、BGCS 或 C++ 编译。发布方式决定是否需要外部 .NET 运行时。

相邻页面：[构建命令](../build/Inno.Build.Cli.md) · [平台扩展](../architecture/PLATFORM_EXTENSION_GUIDE.md) · [当前验收](../architecture/CURRENT_ACCEPTANCE.md)
