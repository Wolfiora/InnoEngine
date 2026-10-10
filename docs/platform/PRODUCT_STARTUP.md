# 产品启动与 Solution 的 “not built”

[平台索引](README.md) · [Wiki 首页](../README.md) · [Build CLI](../build/Inno.Build.Cli.md) · [当前计划复核](../architecture/PLATFORM_OWNERSHIP_PLAN_AUDIT_2026_10_08.md)

## 1. 哪些项目可以启动

| 项目 | 产品/环境 | 实际启动方式 |
| --- | --- | --- |
| `Inno.Editor.Windows` | Windows x64 Editor | 明确构建本项目；第一个程序参数传 Project 目录。 |
| `Inno.Editor.MacOS` | Apple Silicon macOS Editor | 在 macOS 上明确构建本项目；第一个程序参数传 Project 目录。 |
| `Inno.Player.Windows` | Windows 游戏宿主 | `game` 导出游戏后，运行输出中的游戏名 `.exe`。 |
| `Inno.Player.MacOS` | macOS 游戏宿主 | 在 macOS 上导出游戏，打开输出的 `.app`。 |
| `Inno.Player.Browser` | Browser Wasm32 游戏宿主 | 导出 Web 游戏，通过 HTTP 服务打开网页。 |
| `Inno.Build.Cli` | 构建命令 | 运行 `engine`、`game`、`support-pack`、`verify` 等明确命令。 |

这六个项目是生产可执行入口。`Inno.Editor.Hosting`、`Inno.Player.Runtime`、平台 Build 模块与 backend 项目都是库。
Linux 当前只有 Native 工具链贡献；没有完整 Linux Editor/Player。WindowsX86/Arm64、MacOSX64、iOS、NS 没有已实现的产品入口。

## 2. “not built” 的准确含义

当前 `InnoEngine.sln` 的 Debug/Release 配置给五个平台产品项目声明了 `ActiveCfg`，没有声明 `Build.0`。
它们保留在 Solution 中，但默认整 Solution Build 跳过这些项目。其余共享库、工具、测试按各自配置构建。
这是一项**批量构建选择**，不表示项目废弃、目标不支持、代码错误或项目不能启动。

当前实现连本机 Windows 两个产品也采用明确产品构建。这比计划只排除外国宿主/Browser 的最低要求更严格，
避免同一批构建混合中立库、产品 Native 目标及发布属性。没有为改善 IDE 显示而恢复隐式目标。

在 IDE 中选择本机 Editor 的启动配置，设置程序参数，并把启动前构建设为该 Editor 项目及依赖。
如果 IDE 配置只执行默认 Solution Build，应该先显式执行该产品的 Build；不要因为旧 `.exe` 仍存在就跳过构建。
仅设置 Startup Project 不应被当成产品已经成功构建的证据。

## 3. 当前 Windows Editor：FlappyBird

在 InnoEngine 仓库根目录执行以下 PowerShell。当前电脑的验收 SDK 路径如下；其他电脑选择其工程要求的 .NET 9 SDK。

```powershell
$dotnet = 'C:/Users/23842/AppData/Local/InnoWebDotnet/dotnet.exe'
& $dotnet run --project platforms/Windows/editor/Inno.Editor.Windows/Inno.Editor.Windows.csproj -c Debug -- 'C:/Dev/GameEngineDev/InnoEngine.Samples/FlappyBird'
```

等效 IDE 配置：启动项目 `Inno.Editor.Windows`；程序参数为 `C:/Dev/GameEngineDev/InnoEngine.Samples/FlappyBird`。
**Editor 的目录是位置参数**。`--project` 是 Build CLI 的参数，不能传给 Editor。

普通产品 Build/Publish 自动准备匹配的 Native、绑定与 Editor Shader。首次构建需要原生 SDK/依赖，可能耗时较长。
已有构建时可运行对应 `bin/windows-x64/Debug/net9.0/Inno.Editor.Windows.exe` 并传入同一 Project 参数。

可选验证参数：`--graphics-api d3d11`、`--smoke-frames 120`。普通开发不需要设置 frame limit。

## 4. 导出与运行 Windows Player

Player 项目只定义平台宿主；游戏脚本/Plugin 闭包、静态注册与内容由导出流程组合。
源码项目直接 Run 不会导入创作 Project，也没有自动寻找 Project 的逻辑。缺少内容 metadata 会明确失败。
无需向源码 Player 的 bin 手工复制游戏资产；统一导出得到完整、经过校验的发布布局。

```powershell
$dotnet = 'C:/Users/23842/AppData/Local/InnoWebDotnet/dotnet.exe'
$env:DOTNET_HOST_PATH = $dotnet
& $dotnet run --project build/cli/Inno.Build.Cli/Inno.Build.Cli.csproj -- game --tools-target windows-x64 --project 'C:/Dev/GameEngineDev/InnoEngine.Samples/FlappyBird' --support-packs artifacts/support-packs --target windows-x64 --deployment coreclr --startup-scene FlappyBird/FlappyBird.iscene --output artifacts/games
& './artifacts/games/FlappyBird-Windows-x64/FlappyBird.exe'
```

需要 NativeAOT 时把 `--deployment coreclr` 改为 `--deployment nativeaot`；仍使用同一个 Windows Player 源码入口。
CLI 的 Project 命令从 `DOTNET_HOST_PATH` 或 PATH 取得 host；`--dotnet` 当前属于 `engine/support-pack` 等引擎命令。
本指南显式设置 host，避免将一个未被 Project 命令读取的参数当成有效选择。

## 5. 导出与打开 Web Player

Windows 作者主机使用 `--tools-target windows-x64`，游戏目标独立为 `browser-wasm`。
所选 .NET 9 SDK 需要 wasm-tools，构建宿主需要可用 CMake/Ninja；目标 Native/绑定由导出流程冻结并链接。

```powershell
$dotnet = 'C:/Users/23842/AppData/Local/InnoWebDotnet/dotnet.exe'
$env:DOTNET_HOST_PATH = $dotnet
& $dotnet run --project build/cli/Inno.Build.Cli/Inno.Build.Cli.csproj -- game --tools-target windows-x64 --project 'C:/Dev/GameEngineDev/InnoEngine.Samples/FlappyBird' --support-packs artifacts/support-packs --target browser-wasm --deployment mono-wasm --startup-scene FlappyBird/FlappyBird.iscene --output artifacts/games
python -m http.server 8080 --bind 127.0.0.1 --directory artifacts/games/FlappyBird-Web
```

然后打开 `http://127.0.0.1:8080/`。`python` 示例要求可用 Python 3，也可以使用现有静态 HTTP 服务；不自动打开或控制用户浏览器。
Web AOT 使用 `--deployment mono-wasm-aot`。HTTP 服务是本地查看发布结果的工具，不是引擎启动层。
在服务终端按 Ctrl+C 停止。当前 Web 解释执行/AOT 证据来自 owned headless 浏览器，不替代用户 GPU/声学听音验收。

## 6. macOS：在对应机器上执行

Apple Silicon macOS、.NET 9、Xcode command line tools 与所需 CMake 等工具准备好后，在仓库根目录执行：

```sh
dotnet run --project platforms/MacOS/editor/Inno.Editor.MacOS/Inno.Editor.MacOS.csproj -c Debug -- /absolute/InnoEngine.Samples/FlappyBird
dotnet run --project build/cli/Inno.Build.Cli/Inno.Build.Cli.csproj -- game --tools-target macos-arm64 --project /absolute/InnoEngine.Samples/FlappyBird --support-packs artifacts/support-packs --target macos-arm64 --deployment coreclr --startup-scene FlappyBird/FlappyBird.iscene --output artifacts/games
open artifacts/games/FlappyBird.app
```

这是根据当前入口和打包源码列出的操作方式。本轮 Windows 环境未执行 macOS Native/产品运行，不能据此宣称实机通过。
在 Windows 上运行 macOS 入口会被系统服务/SDK 边界拒绝；勾选 Solution 的 Build 不会获得 macOS SDK。

## 7. 构建、运行和发布的区别

```text
Editor 项目 Build → Native/绑定/Shader 与共享 Editor 功能 → 传 Project 路径 → 编辑与 Play
Player 宿主源码 + 游戏代码闭包 + 内容 Pack → 目标/部署发布 → 运行发布产品
Solution 默认 Build → 共享库、工具、测试 → 不隐式选择或启动平台产品
```

新的产品/平台必须明确声明目标、SDK、后端、启动和包装；不同 CPU 可以复用同一产品源码。
完整接入边界及当前剩余问题见[扩展指南](../architecture/PLATFORM_EXTENSION_GUIDE.md)和[计划复核](../architecture/PLATFORM_OWNERSHIP_PLAN_AUDIT_2026_10_08.md)。
