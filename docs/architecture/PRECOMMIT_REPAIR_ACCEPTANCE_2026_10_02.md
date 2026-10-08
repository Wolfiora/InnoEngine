# 提交前 1–8 项修复验收（2026-10-02）

[架构索引](README.md) · [Wiki 首页](../README.md) · [共享宿主验收](WEB_HOST_REFACTOR_ACCEPTANCE_2026_10_02.md) · [当前问题入口](CURRENT_ISSUES.md)

## 范围与结论

后续核查与当前行为见[再次核查与补充修复](PRECOMMIT_RECHECK_ACCEPTANCE_2026_10_02.md)。本页保留八项修复当时的测试与导出记录。

本报告对应本轮架构审查的八项发现。实现、调用方、公开文档、项目引用和回归测试已经一起修正。
修改涉及 **InnoEngine 与相邻 BindGen-CS 两个仓库**，没有提交 Git commit。
本轮没有使用 computer use，没有打开可见 Editor/Player，没有切换用户游戏窗口。
构建验证使用低优先级和单任务编译；这些验证资源限制没有硬编码进通用构建 API。

本轮没有修改渲染公式、光照方向或色彩转换算法。Event、Logging、Graph 与宿主构建生命周期的修正属于共享功能；
Browser 工具链只负责选择它实际使用的 SDK/compiler。BGCS 的配套 builtin headers 同时服务桌面、Linux/Arm64 与 WASM 解析。

## 八项修复

| 项 | 原因 | 当前实现 | 验收证据 |
| --- | --- | --- | --- |
| 1. Inline 日志并发与重入 | 多个 producer 可以同时排空；callback 再次写日志会递归交付，后面的 sink 先看到 nested entry；Flush 缺少等待正在执行的 Receive 的保证。 | 同一个 Router 只有一个交付执行者；callback 日志保留在原队列中，当前 entry 交付完成后再处理。Inline 与 Background 使用相同 queue barrier；Dispose 等待交付完成再释放 sink。sink 快照只在注册、移除和隔离时创建。 | Logging 16 项通过，包含 10,000 层日志链、并发 producer、Flush/Dispose 等待和 callback 自等待拒绝。 |
| 2. 输入坐标转换丢失消费状态 | Game View 创建新的 MouseMovedEvent，源事件与局部坐标表示拥有不同的全局消费状态。 | `Event(source)` 共享原事件的消费 owner；`WithPosition` 只改变坐标。Game View 在改变 capture 状态前拒绝已经全局消费的事件。Hub 内消费仍然只作用于本次表示的 dispatch scope。 | Core Events 23 项、Editor PlayMode 39 项通过；覆盖源到路由表示、路由表示到源、连续转换、已消费输入及 detached viewport 的窗口/focus 策略。 |
| 3. Debug 构建依赖旧 Release Native 缓存 | Debug engine 构建准备 Debug 运行库，但 Support Pack 发布要求 Release；独立 Pack 流程也没有准备这些输入。 | 桌面 Support Pack source 自己准备显式 Release runtime closure，再进行原子发布。统一 engine 流程准备 Editor 配置后，Pack 仍自行满足其 Release 输入。 | 实际 Debug engine 工作流成功；临时移走五个 Release DLL 后，独立 Windows Pack 成功重新生成五个输入并发布。 |
| 4. `--engine-root` 未贯穿 Native 构建 | 部分组件根据正在执行的 assembly/AppContext 找源码和中间目录，可能写入另一个 checkout。 | 一个不可变 `NativeBuildContext` 贯穿组件；源码、overlay、`obj/native` 与 `.lib` 全部由选中的 checkout 解析。默认发现只在调用方未指定 root 时执行。 | 不同 checkout 的公开契约测试通过；缺失源码诊断指向选中 root，而非工具 assembly 所在仓库。 |
| 5. Native 子进程无法取消 | 同步 runner 等待进程退出，不传递取消；Windows 工具发现和 Native verification 又各自实现进程生命周期。 | 所有组件使用共同异步 runner。取消终止完整 process tree，并等待退出与两条输出流完成；后续阶段不继续。Windows 工具发现与 Native verification 复用相同 runner。 | raw/structured 两种入口均验证隐藏 parent 与 child 被终止；失败和预取消路径通过。Build suite 共 53 项通过。 |
| 6. 默认 BGCS 解析环境不可靠 | libclang 20 从环境中的 LLVM 23 导入另一版本的 builtin headers；临时指定 clang 17 只掩盖不一致。 | BGCS.CppAst 内嵌未修改的 LLVM 20 builtin headers，按内容 hash 原子解包；host SDK 发现排除 compiler 自己的 resource headers。Browser 子进程使用 MSBuild workload 实际选择的 compiler，父环境不变。 | 不指定旧 clang 的默认 LLVM 23 环境：CppAst 118、Cpp2C 71、Generation 95 项全部通过。无 host SDK 的 Windows x64、Linux Arm64、WASM 布局测试通过。 |
| 7. Shader minimap 热路径重复查找 | 每条 edge 线性查找节点，并重复计算 node bounds；Graph 集合 getter 每次创建 wrapper。 | GraphDocument 维护中立 record 的节点索引与稳定只读 views；minimap 每帧为每个节点计算一次 geometry，edge 直接按 ID 查 geometry。拖拽预览仍每帧更新。 | Graph suite 8 项通过；2,000 个节点下，10,000 次 lookup/collection getter 操作测得 0 bytes 分配；minimap 遍历由 O(V×E) 降至 O(V+E)。 |
| 8. 可读性、重复入口及旧测试假设 | EditorHost 嵌套诊断类排版混乱，Clear 无实际语义；Browser Pack 重复检查相同 runtime DLL；旧同步 runner、重复 process runner 和无用引用仍存在。 | 诊断收口到 source 的 Replace/Clear；修正声明组织和 XML；删除重复 DLL 条目、无用引用、旧同步 API、重复进程实现与会删除 extern 输出的旧 verification 清理。 | 完整 solution 与 Release CLI 无编译警告/错误；架构验证通过。基线四项失败测试全部恢复通过。 |

## 公开契约与分层

| 契约 | 必要性与稳定语义 | 所属层 |
| --- | --- | --- |
| `Event.isGlobalHandled`、protected `Event(source)` / `MouseEvent(source)`、`MouseMovedEvent.WithPosition` | 路由边界检查消费状态；坐标表示共享一个原始事件的全局消费生命周期，不创建第二套事件系统。 | `Inno.Core.Events` |
| `NativeBuildContext(engineRoot, configuration)`、`GetNativeBuildRoot(assembly)` | 一个构建操作明确拥有哪个 checkout、配置和组件中间目录；不能由工具的加载位置替调用方决定。 | `Inno.Build.Toolchains` |
| `HostNativeBuild.BuildRuntimeAsync` / `BuildEditorAsync` | 组合内置桌面 runtime/editor native closure；组件具体实现仍在各工具链库中。该项目没有 Program，不进入 Runtime。 | `Inno.Build.Toolchains.Host` |
| 组件 `BuildAsync(context, token)`、BGFX `BuildToolsAsync(context, token)` | 取代旧同步入口，显式传递 owner 与取消生命周期；所有调用方已更新，不保留旧 API 转发层。 | 各组件 Toolchain |
| `ToolchainEnvironment.RunAsync` / `CaptureOutputAsync` | 统一 structured/raw 参数入口、输出、取消及退出处理。raw 参数不经过 shell；调用方仍负责目标程序所需的引用规则。 | Build 共同基础库 |

Graph 的索引保存中立 `GraphNodeRecord`，不建立跨 generation live object 的第二权威表。
Minimap 缓存只保存 ID 和数值 geometry，不捕获插件 Type、实例或委托。
Native 组合库的 public reference 只有共同构建契约；具体工具链引用属于实现依赖。
各项目 Wiki、Build 索引、solution 归类和架构规则已经同步。

相关说明：[Core Events](../core/Inno.Core.Events.md)、[Logging](../core/Inno.Core.Logging.md)、
[Graph](../core/Inno.Core.Graphs.md)、[Toolchains](../build/Inno.Build.Toolchains.md)、
[Host Native 组合](../build/Inno.Build.Toolchains.md)、[Browser Toolchain](../platform/Browser/Inno.Build.Browser.md)。

BGCS 新增的是配套工具链数据，不是手写修改第三方实现：
`BindGen-CS/extern/clang-resource/Headers.zip` 内含 275 个未经修改的 LLVM 20.1.8 builtin headers，附许可和来源。
native parser 为 Clang 20.1.2，headers 为同一 major 的 20.1.8；本报告没有宣称二者 patch 完全相同。
Bundle SHA-256：`16927BE0948F2E287D2F20E34E8F8CE0EE287B833EE04CB7CD671FDF4A7C2B84`。

## 自动测试与构建

| 门禁 | 结果 |
| --- | --- |
| 全部 50 个 InnoEngine 测试项目 | **1383 total / 1367 passed / 0 failed / 16 skipped** |
| BGCS.CppAst.Tests，默认 compiler 环境 | **118 passed / 0 failed** |
| BGCS.Cpp2C.Tests，Release，默认 compiler 环境 | **71 passed / 0 failed** |
| BGCS.Generation.Tests | **95 passed / 0 failed** |
| InnoEngine.sln Debug，最终源码 | **0 warnings / 0 errors** |
| Inno.Build.Cli Release，最终源码 | **0 warnings / 0 errors** |
| `Inno.Build.Cli verify .` | **通过** |
| 两仓库 `git diff --check` | **通过** |

16 项 skip 来自 Windows 无法运行的 macOS/Metal shader 测试，没有把它们计入 passed。
两仓库合计 **1651 项通过、0 项失败、16 项跳过**。

本轮增加 22 个引擎回归测试用例与 4 个 BGCS 用例。
四个基线失败中，三个 ImGui 测试仍依赖旧 native combo popup 或自动缩到极小的父窗口；
它们现在通过正式 bounded selector API，并使用具有实际可用空间的父窗口检查位置和滚动量。
另一个 Windows 文件访问测试现在接受真实的 IOException/UnauthorizedAccessException 边界，仍完整断言候选提交失败后的补偿状态。
没有通过跳过测试、反射穿透、InternalsVisibleTo 或测试专用接口取得这些结果。

最终 Build 53 项另行重新运行，确认工具环境仅作用于 child，且最后的通用 runner 调整没有引入回归。

## FlappyBird 导出

使用用户指定的真实项目：`C:/Dev/GameEngineDev/InnoEngine.Samples/FlappyBird`。
startup scene：`FlappyBird/FlappyBird.iscene`。

| 流程 | 结果 | 产物 |
| --- | --- | --- |
| Debug engine + Windows Support Pack | 成功，exit 0 | `%TEMP%/InnoRepairSupportPacks/windows-x64` |
| 五个 Release runtime DLL 缺失时，独立 Windows Support Pack | 成功，五个输入重新生成，exit 0 | `%TEMP%/InnoRepairMissingReleaseSupportPacks/windows-x64` |
| FlappyBird Windows 导出 | 原子提交成功，exit 0 | `%TEMP%/FlappyBirdRepairExports/Windows/FlappyBird-Windows-x64` |
| Browser Support Pack | 0 warnings / 0 errors，exit 0 | `%TEMP%/InnoRepairMissingReleaseSupportPacks/browser-wasm` |
| FlappyBird Web 导出 | 原子提交成功，exit 0 | `%TEMP%/FlappyBirdRepairExports/Web/FlappyBird-Web` |

缺失输入测试只移走 `.lib` 中五个 Release DLL，保留原生编译中间缓存；因此它证明不再依赖已安装的 Release 输入，
不等同于从没有任何 SDK、extern 或编译缓存的空机器构建。
验证机使用已有的 CMake/Ninja、Visual Studio C++ tools 和 .NET wasm-tools；没有修改系统环境或替用户设置全局 compiler override。

第一次 Windows 导出遇到四个 Artifact staging 目录的 IOException/access denied。
检查时目录已被正常清理，没有活动 Editor/Player；没有修改 ACL、删除项目资源或吞掉异常。
再次运行相同公开导出流程成功，随后 Web 导出也成功。
现有证据无法确定外部占用来源，因此保留首次失败日志，不把“重试成功”描述成已定位并修复该环境事件。

## 浏览器运行与验证边界

验收使用独立 headless Edge context、SwiftShader 和静音模式；
测试 HTTP server/browser 在结束时关闭，没有使用用户的浏览器 profile。
不会把软件渲染器结果推断为所有 GPU、所有浏览器或 macOS/Linux 宿主都已实测通过。

| 实测项 | 结果 |
| --- | --- |
| 入口、内容加载、运行时错误 | 正常启动；页面 error、HTTP error 与 console error 为 0。 |
| 共享输入的昼夜切换 | 真实 N 键切换；天空样本平均 RGB 从 155.33 降至 9。 |
| 当前 Sample 的夜间发光 | 当前场景星光区域中 7007 个像素满足蓝色亮度/色差检测，并人工检查生成截图。 |
| 玩法与输入 | 使用真实 pointer/Space 输入；自动控制发送 4 次 flap，得到 Score 1。 |
| Game Over 与重开 | 生成截图确认 Score/Best 1；真实 Space 重开后恢复飞行。 |
| 跨刷新存储 | best-score 为 1，页面 reload 后仍为 1。 |
| 退出清理 | 浏览器与临时 HTTP server 均已关闭。 |

Gameplay 自动控制在白天运行，以可见鸟 sprite 的颜色定位发出 Space；夜间显示单独检查。
软件渲染较慢，玩法阶段逐次推进外部 requestAnimationFrame 调度机会；仍执行真实 Shell/Runtime 帧，
没有替换游戏 Time、物理、Input 或 Storage API。启动、昼夜切换和页面刷新采用正常调度。

初次复用较早验收脚本时，夜间检测 crop 与当前 Sample 的星光位置不符；在该区域得到 96 个像素。
读取当前 Sample 并检查截图后，改为当前发光位置，同时保留天空亮度变化检查。
随后夜间 sprite 的颜色也不符合白天自动控制阈值，第一次玩法控制没有获得分数；
最终将显示检查与白天玩法控制分别执行，得到上表结果。没有因此修改游戏、光照或渲染算法。

浏览器还记录了 316 条重复 warning：WebGL capability 查询的 INVALID_ENUM、
上游 ScriptProcessorNode 弃用提示和 SwiftShader render-pass 性能提示。
这些没有进入本轮八项源码修正；报告没有把 runtime error 为 0 写成浏览器 warning 为 0，也没有声称可听音频或硬件 GPU 性能通过。

截图：[白天](images/flappybird-repair-day-2026-10-02.png)、[夜间](images/flappybird-repair-night-2026-10-02.png)、
[Score/Best 1](images/flappybird-repair-game-over-2026-10-02.png)。

## 证据位置

验证日志保留在当前机器 `%TEMP%`：

- `inno-repair-solution-last.log`、`inno-repair-cli-last.log`、`inno-repair-architecture-last.log`。
- `InnoRepairAcceptanceTests/*.trx` 与 `summary.json`：全部 50 个测试项目结果；最新 Build TRX 为最后一次 53 项运行。
- `inno-repair-bgcs-cppast-final.log`、`inno-repair-bgcs-cpp2c.log`、`inno-repair-bgcs-generation.log`。
- `inno-repair-debug-engine-with-tools.log`、`inno-repair-missing-release-pack.log`。
- `inno-repair-flappy-windows-export.log`（第一次 access denied）、`inno-repair-flappy-windows-export-retry.log`（成功）。
- `inno-repair-browser-support.log`、`inno-repair-flappy-web-export.log`。
- `inno-repair-web-headless-final.log`、`FlappyBirdRepairHeadlessFinal/flappybird-repair-headless.json`：最终玩法和存储记录。

本轮未进行可见 GUI 的面板拖拽与窗口重叠验收，也未启动桌面 Player。
没有在 macOS/Linux 主机进行原生编译或运行；相关源码与公开契约已统一调整，实机验证仍有这些平台边界。
Windows 无法访问规范指定的 `/System/Library/Sounds/Glass.aiff`，完成提示音未播放。
