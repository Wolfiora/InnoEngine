# Web 能力、共享宿主与构建边界

[架构索引](README.md) · [Wiki 首页](../README.md) · [实施计划](WEB_HOST_REFACTOR_PLAN.md) · [Build 索引](../build/README.md)

## 当前架构

游戏使用同一份 Scene、脚本、Plugin、内容包与服务契约。桌面与 Web 的差异由组合入口、Adapter 和工具链承担。共享 Foundation、Shell、Player Runtime 不判断浏览器平台。

```text
Editor / Inno.Build.Cli / MSBuild Tasks
  → Inno.Build（脚本、内容闭包、诊断、取消与原子提交）
  → Windows / macOS / Browser target（目标验证、部署与链接）

Desktop Program / Browser Program
  → Inno.Player.Runtime.PlayerApplication
  → 同一个 GamePlayerHost 与 Shell.RunAsync
  → EngineHost / Scene / Assets / 各领域服务
  → 同一个 DefaultAdapterCatalog（可注入存储工厂）
  → SDL3、BGFX、MiniAudio、Text、UI Adapter
```

`Inno.Player.Browser` 是 .NET 浏览器 SDK 的启动入口，负责 HTTP 内容下载、JS 帧调度及存储组合。它没有第二套游戏 Host、输入系统或场景逻辑，也不链接桌面 Host 源码。共享 Player 是真正的程序集项目 `Inno.Player.Runtime`。

## 必要的平台策略

| 能力 | 桌面入口 | Web 入口 | 共同执行路径 |
| --- | --- | --- | --- |
| 帧调度 | `PollingShellFrameDriver` | `ScheduledShellFrameDriver` 接收 requestAnimationFrame | `Shell.RunAsync` |
| 线程策略 | WorkerPool、可用渲染线程 | SingleThread、调用线程渲染 | EngineHost 与 Rendering Runtime |
| 日志 | Background delivery、终端颜色 | Inline delivery | Core LogRouter 与同一 sink 契约 |
| 模块激活 | Collectible ALC | 静态链接程序集 | 同一 ModuleHost、TypeCatalog、Runtime manifest |
| 持久化 | 文件系统 | 浏览器 localStorage Adapter | 同一 Storage Runtime API |
| 输入 | SDL3 Native | SDL3 Emscripten | Platform Event → Core Events → Input Runtime |

这些参数表达宿主能力，不在基础库中使用 `OperatingSystem.IsBrowser()` 推断策略。浏览器原生 Canvas 句柄等实现细节留在平台和图形 Adapter。

模块发现使用 ModuleHost 所在的加载上下文；普通进程、隔离 MSBuild Task 与浏览器都遵循这一规则。collectible generation 仍必须经过 Full GC、finalizers、Full GC 和弱 monitor 验证；此次重构没有降低退休门槛。

## Native 与 BGCS

每个组件只有一个 `Inno.Native.XXX` 项目。五个 `.Browser` Native 副本已删除。

- `Bindings/common.json` 保存共同声明和映射。
- `bindgen.json` 与 `bindgen.browser-wasm.json` 只描述目标 ABI、链接方式与生成策略。
- Host managed 绑定位于 `Generated/Bindings.cs`；目标绑定位于该项目 `obj/<target>/Generated/Bindings.cs`。
- managed 的 `bin/obj` 同样按目标隔离，桌面编译不会使用 wasm32 布局。
- Browser 工具链从选定 .NET workload 解析完整、版本匹配的 Emscripten SDK；环境只传给子进程。
- CMake 输出归 `Inno.Build.Toolchains.Browser/obj/native/browser-wasm`，安装的静态 archive 归 `.lib/<component>/browser-wasm`。
- UI 的同一 C++ facade 通过目标 Cpp2C profile 和 BGCS 生成绑定；不手写一套浏览器 P/Invoke。
- UI 目标 C++ 桥位于所属项目 `obj/browser-wasm/Native`，通过 CMake 参数选择；宿主 `Native/Generated` 不被目标生成覆盖。

Emscripten、wasm32、静态链接和浏览器存储需要平台实现。共享程序集不因此增加副本。当前 .NET 9 / Emscripten 3.1.56 的 SjLj ABI lowering 位于 Browser 工具链与链接模板中，原因及作用范围有代码记录，不进入游戏、Shell 或渲染服务。

## 统一构建

仓库生产代码只保留四个程序入口：Editor、桌面 Player、Web Player、`Inno.Build.Cli`。原生组件构建、Shader 编译、Support Pack 与架构验证都是库，MSBuild 使用薄 Task 调用相同实现。

Support Pack 核心通过 `IPlayerSupportPackSource` 注册准备流程，通过 `IPlayerSupportPackValidator` 验证目标闭包，不维护平台 switch。`BuiltInPlayerSupportPacks` 是内置目标组合点。准备失败或取消不替换已有 pack；游戏发布在隔离 staging 中完成后再提交输出。

```powershell
 dotnet build build/cli/Inno.Build.Cli/Inno.Build.Cli.csproj -c Release -m:1
 dotnet build/cli/Inno.Build.Cli/bin/Release/net9.0/Inno.Build.Cli.dll support-pack `
   --target browser-wasm --dotnet <wasm-tools-dotnet> --output <support-packs>
 dotnet build/cli/Inno.Build.Cli/bin/Release/net9.0/Inno.Build.Cli.dll game `
   --project <FlappyBird> --support-packs <support-packs> --target browser-wasm `
   --startup-scene FlappyBird/FlappyBird.iscene --output <export>
```

目标游戏输出是 HTTP(S) 可静态托管的站点。浏览器要求 WebGL 2。运行代码、内容格式和输入契约在不同主机上相同；具体 SDK、原生 ABI、发布产物及浏览器能力由所属边界处理。

## 渲染与验收边界

此轮宿主重构复用现有 BGFX Adapter、Shader IR、能力与颜色空间契约，不增加第二套 Web 渲染管线。此前颜色偏暗、光照坐标与夜晚星星的问题已有独立的渲染修复；此轮重新导出 FlappyBird 验证这些结果没有回退。

构建成功不等于全部平台实际运行通过。Windows 构建、后台 Edge/WebGL 运行、macOS 主机和 Safari 的实测结果必须分别记录。历史实测见 [前轮 Web 验收](WEB_PLAYER_ACCEPTANCE_2026_10_01.md)，本轮结果见 [共享宿主与构建重构验收](WEB_HOST_REFACTOR_ACCEPTANCE_2026_10_02.md)。
