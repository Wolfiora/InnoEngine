# Backend 运行接入与构建成本整改验收

[架构索引](README.md) · [执行计划](BACKEND_RUNTIME_BOUNDARY_OPTIMIZATION_PLAN.md) · [平台扩展](PLATFORM_EXTENSION_GUIDE.md) · [Wiki 首页](../README.md)

## 当前状态

2026-10-09–10-10：源码整改、消费者迁移、四个新增库及本机可自动执行的最终门禁已完成。
R01、R02、R02-L、R03 的实现和成功/失败/取消/边界回归已闭合；当前 GUI reload、多屏 DPI、声学与其他机器实测仍有明确证据缺口，整体无保留实机验收不标记完成。
本页按实际结果更新，不将历史报告、编译成功或后台运行当作 GUI/实机通过。
证据根：`artifacts/acceptance/backend-runtime-boundary`。未自动提交。
用户在续跑时允许 Computer Use；2026-10-10 01:19 起追加真实 Windows GUI 验收，早期后台证据仍保留原口径。

## 用户问题与整改结果

| 问题 | 当前源码结果 | 所有权与扩展位置 |
| --- | --- | --- |
| Windows/macOS SDL 窗口创建和焦点逻辑重复 | `Sdl3WindowOperations.CreateWindow/ReadFocus` 为唯一共同实现；Browser 复用 OpenGL 创建，保持初始焦点策略 | 应用登记/释放窗口；平台 SDL integration 只处理系统差异 |
| BGFX Device 固定 surface/Browser 判断 | `IBgfxSurfaceIntegration` 注入；三个 runtime integration 验证自己的 ABI/角色；Default 必须接收 rendering factory | 平台接入解析借用 handle；Device 拥有渲染资源，不识别命名平台 |
| ImGui 附加窗口先销毁而 framebuffer 尚未退休 | `RetireWindowSurface/RetireViewport` Task 与 owner-thread drain；所有 resize framebuffer 纳入退休组 | 先 detach、注销输入和 callback，renderer 确认销毁命令后，原 owner 才销毁 SDL 窗口；故障保留依赖 |
| 生成子 MSBuild、阶段读取、Task 准备重复 | `Inno.Build.Bindings` 直接调用 BGCS；7 份唯一 bindings.props；fresh phase 去重；Build 生命周期复用成功 runtime | Toolchains 只借用中立 generator；Task registry 只存 BCL metadata；独立读锁/编译/输出验证仍保留 |

普通产品 Build 在托管引用编译前准备完整 Native 闭包，并传递同一 binding selection。
Selection 同时包含预期指纹及实际源码目录，不能编译 host Generated 代替 target generation。
普通 Publish 的本轮运行发现 selection 在嵌套 Build 后被过早删除，完整失败日志已保留。
选择文件现归属私有 Task reader，存活至 Native 发布校验完成；普通 Build 由该 reader 的既有退休流程回收。
不再依靠 `IsPublishing` 猜测 SDK 的目标调用方式，也没有删除或放宽严格 generation 校验。
真实 SDK 的 2 项 selection 生命周期/缺失输入回归通过：Build 后仍存活、Publish 后释放；
无 selection 的产品发布在引导 Task runtime 前失败。`--no-build` 不能隐式使用另一份当前 Native generation。
生成 extension 的逻辑身份不包含随机私有 Task load 路径，仍覆盖实际内容和 SDK。
基线并发出现的内部 restore 写入碰撞由 TaskHosting 的 Core.IO 写 lease 协调；等待 Yield/Reacquire。
最终完整回归发现并修复外部消费者的还原作用域问题：`CallTarget` 未传递本次收集的引用，任务收到空列表。
改用同一项目作用域的正常目标顺序，并拒绝空列表；独立外部工程真实构建和 172 项完整 Build 回归均通过。
外部临时工程按所在目录解析到了安装的 SDK 10.0.401；引擎仍按工程声明使用 SDK 9.0.318，未升级目标框架。

Web 发布发现直接生成库没有获得原先子进程中的 Emscripten 路径变量，解析 BGFX 时缺少 `stdlib.h`。
路径展开现属于生成库内部实现：managed/bridge 从 operation 冻结的 SDK 环境读取，不修改进程环境。
CLI 与 MSBuild 使用同一发行的目标 SDK 贡献；独立绑定定义不需要注册游戏发布平台，缺失 SDK 变量仍明确失败。
13 项定向回归通过，包含两份 SDK 选择、缺失变量、输出保护和 bridge；随后完整 Build 回归 178 项及两条 Web 最终发布/运行通过。
首次完整重跑的两项失败日志保留：过度要求 fixture 注册产品目标，以及阻塞 Task 测试的调度等待。
独立绑定能力已恢复为明确的声明配置边界；阻塞 MSBuild fixture 使用独立线程，失败路径等待 owned 工作退出。

## 新增项目、迁移和 API

新增四个生产库：

- `Inno.Integration.Windows.Bgfx.Runtime`
- `Inno.Integration.MacOS.Bgfx.Runtime`
- `Inno.Integration.Browser.Bgfx.Runtime`
- `Inno.Build.Bindings`

没有新增生产 Program；BGCS、extern 与手写生成结果未修改。
ImGui 七个 Internal 文件纯移动的内容摘要见 `results/imgui-pure-moves.json`；随后单独调整生命周期。
旧 DestroyWindowSurface/DestroyViewport、Default optional rendering provider、Tasks 中的生成 identity 与生成子 MSBuild 删除。
实际逐文件闭包以 `file-map.tsv` 为准。

| 公开契约 | 必要性与稳定语义 |
| --- | --- |
| `Sdl3WindowOperations` | 共同无状态 SDL 操作；成功窗口 handle 的销毁权交给应用 |
| `IBgfxSurfaceIntegration` / descriptor / role | backend 自身运行接入 SPI；平台验证实际 handle，域与脚本不依赖它 |
| rendering factory 必填 | 组合入口决定实现，共同 Catalog 不隐式创建 BGFX |
| surface/viewport retirement Task 与 drain | renderer 解除窗口使用后才释放原 owner；不是 GPU fence |
| `INativeBindingGenerator` / request | 中立冻结生成请求、完整结果、失败/取消，无 BGCS/MSBuild 类型泄漏 |
| `NativeBindingGenerator` | 唯一共享生成应用入口；配置/生成/身份/发布内部化 |
| phase statistics | 记录真实读取、复用、进程和物化成本；不服务功能选择 |
| exact binding selection | 连接已校验 Native generation 与后续 managed Compile source |
| `TryResolveNativeToolchain` | 仅查询同目标 SDK 贡献；独立绑定可以没有产品 publisher，缺失 SDK 能力不被替代 |
| `RestoreOwnedProjectTask` | SDK restore 共享写入的薄适配，显式属性和取消，不依赖 BGCS |
| `NativeCMakeSource` / `NativeCMakeExecutor.PrepareSourceAsync` | 冷 producer 固定 recipe 工作区输入，显式解析输入位置；SDK 和输出不重定向，热命中不复制 |

## 环境、基线与结构检查

- HEAD：`1518aa77e87cd208122292780c97c7c280aeb700`。
- 选定 SDK：9.0.318，`C:/Users/23842/AppData/Local/InnoWebDotnet/dotnet.exe`。
- 基线保留原有三处文档修改，原始 diff/revisions/environment/project-dependencies/API 位于 baseline。
- 当前结构检查：229 项目、143 Solution Folder、6 个生产 Exe；游离项目、无 owner 源码、空 Folder、失效显式引用均为 0，见 `results/structure.json`。
- 逐文件闭包当前 2,030 文件、160 项目，未分类变更为 0；最终分类数量以 `results/file-map-summary.json` 为准。
- 新运行集成依赖规则及实际公开架构 CLI 纳入 92 项架构回归，全部通过；最终 Release 实际 MSBuild 图覆盖 229/229 项目并通过。
- 语法 token 保持的多参数排版核对：116 个当前改动手写 C# 文件。

macOS/Browser 产品在 Solution 可见，但默认批量 Build 为 opt-in；明确平台发布使用对应产品和发行入口。
这不是项目废弃或无法启动。普通 Windows Editor/Player 也按明确产品构建，不把全部产品混入中立 Solution 批量 Build。

## 本轮验证记录

### 最终源码门禁结果

当前冻结源码摘要：`98ad5f6def32d13bb14667d08f68fedb5584c749520f145b59887147a7aa5e30`。
摘要覆盖本轮引擎/消费者源代码及构建文件闭包；确切扩展名、目录与排除项见 `results/source-identity-scope.json`（2,341 文件）。
各 Native recipe 另行验证声明的 C/header/SDK/工具/链接输入与输出，总摘要不代替它们；日志逐项记录对应摘要。
这轮 CLI、178 项 Build 测试、92 项架构测试、6 项 Canvas 测试、Canvas Plugin 导出和普通 Windows Editor Debug Build 已通过。
最终 Windows CoreCLR 发布/运行也已通过：发布 3,253.453 秒，运行 4.891 秒，120 帧、2 views / 25 draws，exit code 0。
实际 CoreCLR exe 的 bundle/deps 闭包检查通过：93 个条目及依赖，无 Editor、Authoring、动态模块加载器、构建集成、BGCS 生成器或 MSBuild Task。
`BGCS.Runtime` 及 Windows SDL/BGFX runtime integration 存在；证据为 `results/coreclr-published-closure.json`。
检查只读当前产物，布局依据 [.NET 9 HostModel 的 bundle manifest](https://github.com/dotnet/runtime/blob/v9.0.0/src/installer/managed/Microsoft.NET.HostModel/Bundle/Manifest.cs)，没有载入或穿透业务代码。
最终 Windows NativeAOT 发布/运行通过：发布 1,401.437 秒，运行 2.641 秒，120 帧、2 views / 25 draws，exit code 0。
最终 Web 解释执行发布/运行通过：发布 2,390.844 秒，浏览器检查 35.250 秒，11 项行为断言、page error 0。
高分为 1，刷新后从 `flappybird:flappy-bird/best-score.txt` 读回；光照移动、昼夜/星光和数字音频采样均通过。
截图已复核，见 `captures/web-interpreted-attempt-15/{day,night,scored,keyboard-night}.png` 与 report.json。
浏览器使用独立 headless Edge/SwiftShader，不代表物理 GPU、焦点或扬声器；favicon 的两次 404 单独保留。
实际 browser boot 闭包共 87 个程序集、93 个资源哈希检查通过，无 Editor、Authoring、动态 loader、构建集成或生成器。
静态导入所不再需要的 `BGCS.Runtime` 被裁剪，Windows 动态导入仍保留；见 `results/web-interpreted-published-closure.json`。
闭包脚本最初把 BGCS.Runtime 错列为 Browser 必需库，修正验收条件后通过，原尝试独立保留；未修改生产源码。
最终 Web AOT 发布/运行通过：发布 1,200.141 秒，浏览器检查 31.156 秒，同样 11 项行为断言、page error 0，高分刷新读回成功。
AOT Native Wasm 为 23,108,481 bytes，解释执行为 6,935,590 bytes，哈希不同；compiler 显式启用 RunAOTCompilation。
AOT 的 87 个程序集/93 个资源哈希也检查通过，见 `results/web-aot-published-closure.json`、`results/browser-native-artifacts.json` 与 `captures/web-aot-attempt-17/report.json`。
两个 Browser 都保留 315 条第三方 capability warning 和两次 favicon 404；不描述成控制台无 warning 或全部 HTTP 请求成功。
普通 Windows Editor Release Build 已通过：4,172.797 秒，托管汇总 0 warnings / 0 errors。
最终源码隐藏 FlappyBird Debug 6,000 帧与真实源文件 reload 已通过；两个模块换代后 Ready，连续 5,231 帧无渲染错误，完整 6,000 帧并正常退出。
reload 使用公开 SDL integration 模拟隐藏窗口的初始焦点，严格日志检查拒绝过程中出现的任何渲染错误；不是物理焦点或 GUI 视觉验收。
Rendering2D D3D12 6,000 帧、它生成的 EditorScripts 编译，以及隐藏 FlappyBird Release 6,000 帧均已通过。
最终 runtime、产品、性能与 Release 实际项目图组均 exit code 0；10 个最终源码门禁全部通过，摘要一致。
最终 Release 图检查 132.656 秒，输出 `results/evaluated-projects-release-final.json`；不是只检查静态 ProjectReference 文本。
下方同轮较早结果保留为历史证据，不代替最终源码门禁。

普通 Editor 的 MSBuild 汇总为 0 warnings / 0 errors；Native 子工具的第三方 warning 单独记录。
本次 CLI 准备 BGFX 离线工具的冷编译汇总为 2,829 warnings / 0 errors，主要为 C4819、C4389 和临时目录 MSB8029。
未修改第三方源码或关闭其诊断，也不将托管汇总的零警告描述成整个 Native 构建没有 warning。

| 门禁 | 已有本轮结果 / 当前状态 |
| --- | --- |
| SDL 共同窗口、owner、surface | 最终整合重跑 10 项通过 |
| BGFX surface 预检/开放接入 | 最终完整回归 40 项通过；实际 D3D11 threaded/inline/失败后再初始化分别独立进程各 1 项通过 |
| ImGui 尺度、交互、窗口退休 | 38 项通过；Faulted owner 的隔离进程另 1 项通过 |
| binding/phase/Task/restore | SDK 路径修复后的最终完整 Build 回归 178 项通过；实际 MSBuild 精确源码消费通过 |
| Task publication/实际 SDK 编译 | 19 项通过；后续普通产品路径继续覆盖 |
| 整体 Solution Debug / Release | 均 0 warnings、0 errors，分别 831.031 / 850.609 秒 |
| 普通 Windows Editor Debug Publish | 修复后通过，350.359 秒；完整 Native selection 在最终安装前严格校验 |
| Windows Editor Debug / Release、隐藏启动/退出 | 最终源码普通 Build 均通过，0 warnings / 0 errors，分别 1,721.062 / 4,172.797 秒；两个隐藏 FlappyBird 各 6,000 帧并正常退出 |
| CoreCLR/NativeAOT | 最终 CoreCLR 与 NativeAOT 发布及各 120 帧隐藏运行通过。较早本轮 CoreCLR 真实窗口已确认输入、昼夜/星光、最大化、最小化恢复与退出，其源码阶段和 GUI 证据单独保留 |
| 浏览器解释执行/AOT | 最终解释执行/AOT 发布及两组实际浏览器各 11 项行为检查通过；早期 SDK/链接失败日志与 capability warning 独立保留 |
| Canvas | 追加修复后的真实消费者构建及 6 项测试通过，包含替换服务后相同 handle 的 owner 检查；Plugin 已原子导出并同步 FlappyBird |
| 实际绑定库生成及 CheckOnly | target 7 / host 7，通过；host Bindings.cs SHA-256 未改变 |
| Rendering2D/Plugin/脚本 | 最终源码 D3D12 隐藏 Editor 6,000 帧并正常退出，生成 EditorScripts 的真实编译通过；Canvas Plugin 已同步当前 FlappyBird |
| Input / Hosting / PlayMode | 18 / 6 / 54 项通过 |
| reload / History / Runtime / Scene / RetirementBarrier | 22 / 99 / 61 / 50 / 30 项通过，包含相关 Missing、Workspace 与退出回归 |
| 实际焦点、GUI 视觉与窗口退休 | 同轮较早源码阶段的真实浮动 GameView、失焦隔离、Play/Stop、附加视口关闭、Modal、向下 Selector 和 Shader 小地图已有证据；最终 Canvas 修复后的 GUI reload 访问被拒绝，未执行。Export 完成自动关闭等未覆盖交互不计作通过 |
| 多屏 DPI、live-resize 与声学 | 不同物理 DPI、浮动边角拖拽 resize 和物理扬声器尚未取得有效证据，不能以最大化或音频采样替代 |
| macOS Native/Editor/Player；Linux Native 实机 | 当前 Windows 无相应执行环境，未实测 |

当前回归矩阵共 701 项通过、0 失败；加上独立 Faulted owner 1 项与 Canvas 消费者 6 项，共 708 项通过。
selection 生命周期的 2 项已包含于完整 Build 回归，不再重复累计；早期定向重跑与失败尝试不重复累计。
真实项目图、公开 API、脚本导出和边界规则均纳入架构检查。
较早 Release 实际项目图 CLI 136.219 秒的结果独立保留；最终源码图以 132.656 秒的 `results/evaluated-projects-release-final.json` 为准。

首次准备构建因发现随机加载路径与选择源码目录的问题主动中止，保留失败/中止日志；没有计作成功。
所有后续通过项必须有本轮对应命令和日志，不能沿用旧 acceptance 的成功记录。

## 性能证据与口径

本轮基线三次热构建：253.750、229.875、225.390 秒；Native 86.321、84.636、83.786 秒。
各次 Native 输入读取 139,978 次、13,948,278,105 bytes、Native 进程 0、8 个产品。
两个基线并发：一项成功；另一项在共享内部 NuGet restore 发生写入碰撞，见 baseline-concurrent 日志。
这些不是历史 215–242 秒的数据。

新计数包含 binding batch/phase，而旧计数不包含生成子 MSBuild 的输入读取。
因此不能直接把两个总 bytes 数相减宣称同口径读取消减；最终同时报告阶段、进程、生成、Task 复用和部署 mtime。
本轮产品 runner 使用 Debug CLI，而普通 Release Publish 使用 Release Task runtime；它们的 BGCS 生成程序集身份不同。
例如 BGCS.dll 分别为 482,816 / 462,336 bytes，MVID 分别为 `6f8eba09-9b84-40ec-b2fa-3f8d2d270b38` / `b5755cc9-8496-4ef2-9d42-bc10341308b8`。
生成身份按真实生成实现隔离，因此不能把跨这两份工具身份的冷准备计成同一请求的错误缓存命中或强行合并；
读取 PE metadata 的证据为 `results/generator-configuration-identities.json`，没有载入或穿透业务程序集内部类型。
最终同环境测量通过，完整数据见 `results/performance-summary.json` 与 `results/performance-contracts.json`：

| 测量 | 整次 Build 秒数 | Native 阶段秒数 | Task 准备 / 复用 | Native 文件变化 |
| --- | ---: | ---: | --- | --- |
| 热构建 1 | 246.015 | 24.9580 | 1 / 2 | 0 |
| 热构建 2 | 246.015 | 23.4347 | 1 / 2 | 0 |
| 热构建 3 | 260.140 | 25.0317 | 1 / 2 | 0 |
| 两个并发请求 | 342.032 / 375.890，均 exit 0 | 详见各自 phase 记录 | 各 1 / 2 | 均 0 |

普通热构建均值从基线 236.338 秒变为 250.723 秒，约增加 6.1%；**整次构建没有变快**。
Native 阶段均值从 84.915 秒降为 24.475 秒；这是局部改善，不据此宣称整体构建加速。
三次热构建和两个并发请求均只有 1 个 binding batch、0 次实际绑定生成、0 个 Native/托管子工具进程及 0 bytes materialization。
各热构建仍完整读取 140,947 次、14,753,983,807 bytes，并验证 56 个输出、128,396,496 bytes；阶段独立校验没有被取消。
同一 Build 成功 Task runtime 准备 1 次、复用 2 次；普通详细日志确认 3 次 Task host 验证，新增 immutable publication 为 0。
每次保留 3 个实际载入边界的私有 load owner；不把不同边界合并为共享可写 obj。14 个已部署 Native 文件内容和 mtime 均未改变。
最低日志级别不显示 host publication 消息，因此这些计数来自独立 normal-verbosity 诊断运行，未混入三次热构建均值。
准备运行、两次并发、阶段读取与部署记录均基于最终源码摘要；性能契约检查无失败。
外部 restore 修复前的阶段测量保留在 `results/pre-restore-fix-performance.json`，
最终性能必须使用修复后的重跑结果，不将此前成功测量冒充最终源码结果。

## 生命周期、扩展与清理

### 本轮执行入口

完整命令、工作目录、退出码、耗时和源码摘要保存在 `results/post-sdk-gates.json`、
`results/product-executions.json`、`results/quiet-executions.json` 与 `results/performance.json`。
最终测试计数以 TRX 为准，产品运行还必须满足帧、绘制、错误和内容断言。

```powershell
$taskSdk = 'C:/Users/23842/AppData/Local/InnoWebDotnet/dotnet.exe'
& $taskSdk test tests/build/Inno.Build.Tests/Inno.Build.Tests.csproj -m:1 -nodeReuse:false
& $taskSdk test tests/tooling/Inno.Tooling.Architecture.Tests/Inno.Tooling.Architecture.Tests.csproj -m:1 -nodeReuse:false
& $taskSdk build build/cli/Inno.Build.Cli/Inno.Build.Cli.csproj -m:1 -nodeReuse:false
```

实际发布由当前 CLI 的 `game` 命令执行，四组 target/deployment 分别为
`windows-x64/coreclr`、`windows-x64/nativeaot`、`browser-wasm/mono-wasm`、
`browser-wasm/mono-wasm-aot`；Project 为 Samples/FlappyBird，启动场景为
`FlappyBird/FlappyBird.iscene`。输出、Support Pack 和浏览器 profile 使用本轮独立 owner。
Windows Player 使用 `--window-visible false --smoke-frames 120` 后台运行。
隐藏 Editor fixture 通过公开组合边界运行 6,000 帧，不改变生产入口或用户 Project。

热构建使用普通 Windows Editor Build；并发请求通过 SDK `--artifacts-path` 分别声明托管输出 owner。
诊断详细日志单独运行，不混入三次热构建平均耗时。

创建成功才转移 renderer owner；失败由原创建方清理。重复 context 创建拒绝。
关闭 callback 只登记退休，不 await、不推进 BGFX 帧、不抛向 Native。日常 managed 安全点确认 Task；
最终退出使用 RetirementBarrier，Pending/Faulted 不继续释放 Device/窗口/callback owner。
命令确认依据当前 BGFX 队列与[官方 frame 同步说明](https://bkaradzic.github.io/bgfx/bgfx.html#frame)。

新平台在实际平台目录实现 SDL 和 BGFX runtime integration，在产品中明确注入；共同领域不改平台判断。
新 renderer 使用自己的 backend/SPIs 与必要实际 integration，通过 rendering factory、content compiler、Native plan 组合。
无需 BGFX SPI 的 renderer 不依赖它；没有真实差异的组件不增加逐平台空项目。

当前有效产物、活跃 owner 和验收日志保留。清理只针对本轮证明失去 owner 的候选/中间/私有载入目录；
不删除锁文件解锁，不清理用户、SDK、NuGet 或浏览器全局缓存。全部构建结束后磁盘容量与 owner 状态写入 `results/final-closeout.json`。
收尾时 C 盘可用 24,508,002,304 bytes（约 22.825 GiB）；本轮 Editor/Player 运行进程已结束，当前有效产物与日志保留。
手动清理已结束的本轮 `InnoExternalDiagnostic` 临时目录被自动审批拒绝，原因返回 `blocked by policy`；
命令没有执行，未以其他工具绕过，见 `results/cleanup-policy-status.json`。此前被拒绝的 Native/staging 清理也未绕过。

指定 `/System/Library/Sounds/Glass.aiff` 在 Windows 不可访问，本轮提示音未播放。

最终结构与文档复核：229 项目、143 Solution Folder、1,604 显式引用，无游离项目、空目录或无 owner 源码；
258 个引擎文档页、54 个 BGCS 页和消费者入口的本地 Markdown 链接均有效。
引擎、Canvas、Samples 的 `git diff --check` 均 exit 0；BGCS 工作区干净，extern 和 checked-in Generated 无差异。
本轮逐文件映射 2,030 个文件、160 项目，57 新增、162 修改、8 迁移、1,803 保留，未分类变更为 0。

## 建议 commit title

```text
refactor(sdl3): centralize shared window operations
refactor(bgfx): inject platform surface integrations
fix(imgui): retire rendering surfaces before destroying windows
refactor(build): centralize binding generation and validation
perf(msbuild): reuse task runtime preparation within build scope
docs(architecture): document backend runtime boundaries and acceptance
```


## 实际验收追加发现（2026-10-10，修复与重验结果）

- Web 解释执行准备的真实编译发现 SDL 静态导入仍编译动态 InitApi。原因是生成闭包只传递绑定身份，未传递逐组件链接请求。已将实际 Static/Shared 请求写入同一选择文件，托管 Native 边界据此选择初始化，发布验证同时检查链接一致性；不新增平台判断。
- GUI 项目副本的源代码重编译后，Scene View 出现 Canvas unavailable mesh 1。原因是 UI 文档仍存活，新 ViewContentSource 的绘制器却重建了空的增量缓存。已把中立 CPU 网格/纹理数据归属 Canvas 文档 owner；渲染代际只拥有 GPU 缓存。失败截图为 artifacts/acceptance/backend-runtime-boundary/captures/gui/editor-reload-failure.png，原编辑器正常退出 code 0。最终源码的后台真实 reload 已通过；修复后的 GUI 视觉 reload 因访问拒绝未执行。
- Canvas 的公开替换边界还覆盖 UI 服务 owner：新服务复用相同数值 handle 时必须重建视图状态，不能继续调用旧服务。当前 6 项完整消费者测试通过；没有新增公开 API。
- 冷构建日志复核发现：通用 CMake executor 和 Browser 聚合仍直接向编译器传递工作区源码，阶段哈希只能检测阶段边界的变化，不能保障编译途中修改后恢复的输入。现已通过 `NativeCMakeSource` 固定 recipe 的工作区输入，Browser 的组件 include、bridge 和输入参数解析到同一复制树；外部 SDK 和输出保持原位置。真实 CMake 回归在原文件临时改为 17、mtime 保持不变时编译，实际加载 DLL 返回冻结值 42；最终源码完整 Build 回归 178 项通过，见 `cmake-source-snapshot.trx` 和 `build-sdk-corrected.trx`。
- 修复后的 GUI reload 尝试已创建真实 D3D11 Editor 并完成首帧，但 Computer Use 激活窗口失败，重新捕获后返回 `GetCursorPos` 访问拒绝（0x80070005）。已停止 GUI 操作，并取消本轮独立快照进程；这次没有验证正常退出、reload 或视觉结果，见 `results/gui-editor-reload-current.json`。公开 FrameDriver、Module catalog 和 Diagnostics 边界的最终后台真实源文件 reload 已通过；不替代这项 GUI 视觉缺口。
- 先行隔离 reload 已通过：当前 Canvas Plugin、已校验的 Editor runtime 快照、真实脚本修改、2 个模块换代、Ready 后连续 5,277 帧无渲染错误，完整 6,000 帧并完成退出。使用公开 SDL integration 模拟初始焦点，因为隐藏窗口按生产规则不自动编译；不是物理焦点或 GUI 视觉验收。前两次 fixture 失败（运行时间不足、无焦点）保留。结果为 `results/early-source-reload.json`，明确 `exactFinalRuntime=false`；随后最终源码主门禁也已完成真实 reload，2 个模块换代、连续 5,231 个 Ready 后帧无渲染错误、6,000 帧与正常退出。最终结果见 `results/quiet-executions.json`，先行结果不替代它。
