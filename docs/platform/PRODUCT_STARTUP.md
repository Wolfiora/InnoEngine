# 启动 Editor、构建与发布游戏

[平台索引](README.md) · [Wiki 首页](../README.md) · [开发入口](../entry/README.md)

## 从零开始

准备 Git、仓库 global.json 声明的 .NET SDK 和当前宿主 Native SDK。Windows x64 使用 Visual Studio C++ 与 Windows SDK；macOS ARM64 使用 Xcode 工具链。

```sh
git clone --recurse-submodules https://github.com/Wolfiora/InnoEngine.git
cd InnoEngine
```

打开 InnoEngine.sln，日常启动项目只有：

| 项目 | 操作 |
| --- | --- |
| **InnoEngine.Build** | 增量准备当前宿主 Editor，成功后记录实际部署。 |
| **InnoEngine.Editor** | 验证并启动已准备 Editor；无 Project 参数时选择项目目录。 |

Rider 可使用仓库的两个共享 .run 配置，VS 可选择同名启动项目。用户以前保存的个人启动配置需要自行整理，源码不会删除 IDE 私有配置。

BGCS 位于 dependencies/BindGen-CS，使用固定 revision。普通 clone 后首次准备会初始化缺失子模块，因此需要网络；已初始化的用户修改保留。入口不安装系统 SDK。

## 日常操作

```sh
dotnet run --project entry/InnoEngine.Build/InnoEngine.Build.csproj
dotnet run --project entry/InnoEngine.Editor/InnoEngine.Editor.csproj
dotnet run --project entry/InnoEngine.Editor/InnoEngine.Editor.csproj -- /absolute/MyGame
```

引擎源码修改后运行 Build。游戏脚本和资产修改由 Editor 导入、编译与 reload 处理。没有传项目目录时弹出目录选择窗口，取消正常退出。

Debug/Release 分别准备并启动：

```sh
dotnet run --configuration Release --project entry/InnoEngine.Build/InnoEngine.Build.csproj
dotnet run --configuration Release --project entry/InnoEngine.Editor/InnoEngine.Editor.csproj
```

Build 的 `--configuration Release` 可以覆盖所构建产品的配置；Editor 跟随其入口编译配置。两者配置一致才能复用相同准备结果。

## 显式清理

```sh
dotnet run --project entry/InnoEngine.Build/InnoEngine.Build.csproj -- --clean
dotnet run --project entry/InnoEngine.Build/InnoEngine.Build.csproj -- clean --scope product --dry-run
```

第一条清理当前选定 Editor 工作状态后构建。第二条只预览。默认 Build 不清理。

产品清理先检查目标与 SDK，再取得 owner 保护；活动 Editor 阻止对应产品被删除。其他目标、Native/Binding 缓存、有效 Support Pack、游戏输出、日志和全局 SDK 缓存保留。Ctrl+C 取消后不会提交半成品。

缓存预算清理仍在实现，当前状态见[问题台账](../architecture/CURRENT_ISSUES.md)。

## 支持状态与 not built

| 宿主 | Editor / 默认 Build |
| --- | --- |
| Windows x64 | 路由至 Windows Editor 产品。 |
| macOS ARM64 | 路由至 macOS Editor 产品；本轮实机验证另行记录。 |
| Linux x64 / ARM64 | 没有完整 Editor；已有 Native 工具能力不能视为 Editor 支持。 |
| 其他 CPU/平台 | 未注册的产品明确拒绝。 |

默认 Solution Debug/Release 只构建两个入口及轻量依赖。其他项目显示 **not built** 表示本次配置未选中它，不代表废弃或编译失败。`SourceDebug`/`SourceRelease` 面向共享源码和测试。

平台 Editor 与 CLI 是应用库；Player 项目是发布源码输入。最终 Player Exe 由平台模板生成，包含游戏代码、内容和匹配的 Native 产物，不作为日常 IDE 启动项目。

## Support Pack 与游戏发布

```sh
dotnet run --project entry/InnoEngine.Build/InnoEngine.Build.csproj -- support-pack --target windows-x64
dotnet run --project entry/InnoEngine.Build/InnoEngine.Build.csproj -- game --tools-target windows-x64 --project /absolute/MyGame --target windows-x64 --deployment coreclr --startup-scene Scenes/Main.iscene --output artifacts/games
```

Pack 是目标编译、链接与运行输入，不包含游戏内容。可单独准备，也可由游戏发布获取。准备前缓存优化尚未完整验收；NativeAOT 和 Web AOT 的最终游戏编译与链接仍需要执行。

macOS 产品目标为 `macos-arm64`。Browser 目标为 `browser-wasm`，部署选 `mono-wasm` 或 `mono-wasm-aot`，实际发布需要匹配的 workload 和 Emscripten。网页通过 HTTP 服务运行，不能把 Browser 源码库当作独立网页游戏启动。

## Native 文件在哪里

| 内容 | 位置与 owner |
| --- | --- |
| 第三方源码 | extern，构建不修改它。 |
| 可复用工作区 | backend Toolchain 的 `obj/native/<target>/<workspaceIdentity>`。 |
| 校验后产物 | `artifacts/native/<component>/<target>/<artifactFingerprint>`。 |
| 生成绑定 | 当前位于 `artifacts/bindings/<component>/<target>/<generationFingerprint>/`；HostSource 位于同项目的 `Generated/`。 |
| Pack | `artifacts/support-packs/<target>/<contentFingerprint>`。 |
| Editor 部署 | 平台项目 SDK 输出；ready.json 指向并校验实际部署。 |
| Windows 短路径 | Temp/InnoTools 下由工具 owner 建立的工作区别名。 |

短路径 junction 不代表另一份完整 Native 缓存。正常结束删除本次别名，实际工作区继续复用；不能递归删除 junction 来“腾出”对应工作区空间。源码、工作区和成品缓存各有独立职责，不使用混合 .libs 目录。

最终用户运行发布后的 Player 时不执行开发入口、MSBuild、BGCS 或 C++ 编译。发布方式决定是否需要外部 .NET 运行时。

相邻页面：[构建命令](../build/Inno.Build.Cli.md) · [平台扩展](../architecture/PLATFORM_EXTENSION_GUIDE.md) · [当前验收](../architecture/CURRENT_ACCEPTANCE.md)
