# 提交前再次核查与补充修复（2026-10-02）

[架构索引](README.md) · [Wiki 首页](../README.md) · [上一轮八项修复](PRECOMMIT_REPAIR_ACCEPTANCE_2026_10_02.md) · [当前问题入口](CURRENT_ISSUES.md)

后续的目录安全、导出退休、输入边界与封装检查见[最新全范围复核](PRECOMMIT_FULL_AUDIT_2026_10_02.md)。
本页统计保留为该轮验证证据。

## 核查范围

本次继续检查当前未提交改动的共享宿主、Core Events/Logging、Editor Game View 输入、
构建工具进程与 Support Pack 事务，并复核依赖方向、公开契约、脚本引用及回归测试。
确实发现了四类补充问题，已经修正源码、调用方和对应 Wiki。
本页保留本次证据；上一轮报告的历史统计不会被改写。

没有使用 computer use、打开可见 Editor/Player 或切换用户的游戏窗口。构建与测试在后台低优先级执行。
没有提交 Git commit。本轮源码修改只在 InnoEngine；BindGen-CS 沿用上一轮已经验证的修改。

## 补充发现与修复

| 问题 | 原因 | 修复与验证契约 |
| --- | --- | --- |
| 文件日志存在第二个交付线程、轮换碰撞及不可靠失败处理 | Router 之外又创建无界 queue/worker；毫秒文件名可能在快速轮换时重复；内部吞掉 IO 错误，但最终 flush 又可能在线程外抛异常。Router Flush 只能等到 entry 入第二个队列。 | 文件 sink 同步写入完整 entry 并 flush；调度、队列和背压只由 Router 决定。CreateNew + UTC 时间/GUID 保证不同 sink 和快速轮换不会混写；轮换在完整 entry 之间进行。IO 失败交给 Router quarantine。stderr 自身失效也不能终止健康 sink 或后续 failure observer。 |
| Game Input 接收时间早于 Editor 消费，且被消费的 release 会残留按住状态 | Host 先改变 Game backend，再把事件 enqueue 给 Editor；后来快捷键 HandleInGlobal 已来不及阻止游戏接收。仅在 Route 入口检查标记无法修复这个实际接线顺序。丢弃 release 又无法清除已交付的 held state。 | Host 只 enqueue 到 Edit Session 的现有 Dispatcher，在其有序派发完成通知中才 Route 给 Game Input。已消费输入不重放；来自当前交付窗口的已消费 release 返回独立 focus-reset event，在 Play simulation 前释放按住状态。其他窗口的 release 不改变 capture。 |
| Support Pack 完整 Validate 期间的取消仍可能提交 | 原先最后一次取消检查位于 Validate 之前。 | Validate 后、第一次移动 installed pack 前再次检查取消。取消保留 last-good pack 并清理 staging；原子替换一旦开始不在中途响应取消。 |
| 工具输出读取或转交失败时，构建可能永久等待 | output pump 出错停止读取，但 runner 只等待进程退出。工具可能堵在输出管道上而无法退出。 | 共同 runner 同时观察 process exit、stdout 和 stderr。任一失败立即清理完整进程树，等待退出、观察所有输出任务，再传播原始失败。raw/structured 与 SDK 查询继续共用该生命周期。 |

日志保留清理通过 exclusive open + DeleteOnClose 持有删除期间的 ownership；
活动 writer 关闭后，再由后续轮换清理。保留预算对锁定、权限受限或不支持共享锁的文件系统属于尽力执行。
这是基础 IO 的实际限制：.NET 9 的 Unix 实现使用建议锁，Browser 虚拟文件系统禁用文件锁。
[.NET Runtime 实现](https://github.com/dotnet/runtime/blob/v9.0.0/src/libraries/System.Private.CoreLib/src/Microsoft/Win32/SafeHandles/SafeFileHandle.Unix.cs)
浏览器 Player 的每个虚拟文件系统只使用自身 Session 的 sink；没有为此在基础库增加 Browser 判断或另建全局 owner 表。

## 公开契约与架构

| 变化 | 必要性与稳定语义 |
| --- | --- |
| `EventDispatcher.dispatched` | 同步观察有序 Hub 派发的完成状态，包括预先或当次全局消费的事件。用于在消费已经确定后进行边界适配和 capture 释放；不参与消费、不创建队列。Hub 抛错不通知完成；observer 抛错传播。owner 在退休前必须退订。 |
| `FileLogSink(directory, maxFileSizeBytes, maxFiles)` | 去掉独立 deliveryMode 参数。Host 只配置 Router 的一个交付策略，sink 不拥有 worker。构造时拒绝空目录和非正预算；Receive 返回前完整写入；Dispose 等待活动写入并报告关闭失败。 |
| `EditorGameInputCapture.Route` | 除正常 routed event 或 null 外，消费的 terminal event 可以返回中立 focus reset；原 consumed event 不会再次进入游戏。 |
| `ToolchainEnvironment.RunAsync` / `CaptureOutputAsync` | 输出失败与取消都完成进程树清理后才返回；输出 IO 失败明确传播，不继续下一构建阶段。 |

`dispatched` 随 EventDispatcher 的既有显式 scripting export 进入 `InnoEngine.Events`。
Runtime 编译和 IDE 引用验证使用同一裁剪 reference 链；没有增加中央清单或实现 namespace 引用。
Host observer 通过自身 LifetimeScope 退订，不保存插件 Type、实例或跨 generation callback。
Identity/Reference/Reload 强制标准已经核对；GC 退休与 Faulted barrier 继续保留。

这些是共享事件、日志和 Build 生命周期修正，没有新增 Native Browser 项目、第二套输入系统、第二套事件总线或 Program。
当前生产入口仍为 Editor、Desktop Player、Browser Player、统一 Build CLI 四个。
Foundation、Shell、Player Runtime 仍不通过 `OperatingSystem.IsBrowser` 决定能力。
本次没有修改渲染公式、光照方向、Shader IR 或颜色转换算法。

## 自动验证

完整 50 项目回归通过后，对最后的 Editor 焦点边界重新构建并补测；下表汇总各项目最终结果。

| 门禁 | 结果 |
| --- | --- |
| 全部 50 个 InnoEngine 测试项目 | **1424 total / 1408 passed / 0 failed / 16 skipped** |
| Core Events / Logging / Build / Editor PlayMode | **28 / 36 / 56 / 51 项通过**；包含真实 Input backend 的最终构建与测试 |
| Runtime 与 IDE scripting reference | **125 项通过**；新增 API 的 Runtime 编译与 Game/Editor IDE reference metadata 均通过 |
| Debug 完整解决方案 | 0 warnings / 0 errors |
| 最终 Editor Application 构建 | 0 warnings / 0 errors |
| Release Build CLI、架构验证 | 0 warnings / 0 errors；架构验证通过 |
| 两仓库 diff check、Markdown 链接 | 通过；缺失链接为 0 |

本轮相较上一轮新增 **41 个回归用例**。最后四个用例覆盖尚未交付首个输入就失焦/关闭的情况，
分别验证 consumed 与 unconsumed 路径都立即停止接受游戏输入；没有等待下一次 Game View presentation 才失效。
所有测试通过不等于下文未实测平台或 GUI 门禁已经完成。

相较上一轮新增的用例覆盖：两种日志交付策略、Unicode 字节预算、快速轮换、过大 entry、
同目录多 writer、并发写入、参数失败、Dispose、次级错误渠道失效，
派发顺序、全局/Hub 消费、预消费、递归 enqueue、observer/consumer 失败、退订，
真实 Game Input held state、其他窗口隔离、Validate 期间取消及 stdout/stderr 失败后的 parent/child 清理。
测试没有使用反射穿透、InternalsVisibleTo 或生产测试后门。

## FlappyBird 导出与运行

使用真实 `C:/Dev/GameEngineDev/InnoEngine.Samples/FlappyBird`，startup scene 为 `FlappyBird/FlappyBird.iscene`。

| 流程 | 结果 |
| --- | --- |
| Windows Support Pack 与 FlappyBird 导出 | 最终源码成功，exit 0；原子提交 |
| Browser Support Pack 与 FlappyBird 导出 | 最终源码成功，exit 0；原子提交 |
| 静音 headless Edge / SwiftShader | 成功；runtime/page/HTTP/console error 为 0 |
| 昼夜切换与夜间星光 | 真实 N 键切换；天空均值 155.33 → 9，当前星光检测区域 3009 个发光像素；截图检查通过 |
| 玩法、重开、持久化 | 真实 pointer/Space 输入，3 次 flap 得到 Score/Best 1；Space 重开，刷新后 best 仍为 1 |
| 退出清理 | headless browser 与临时 HTTP server 均已关闭 |

产物位于 `%TEMP%/FlappyBirdRecheckExports/windows-x64/FlappyBird-Windows-x64` 与
`%TEMP%/FlappyBirdRecheckExports/browser-wasm/FlappyBird-Web`。
最终 headless 测试沿用上轮的当前 Sample 检测与控制脚本，玩法阶段逐次推进外部帧调度机会；
没有替换 Time、物理、Input 或 Storage，昼夜与刷新检查使用正常调度。
软件渲染记录了 316 条重复 warning（310 条 WebGL capability 查询、2 条 ScriptProcessorNode 弃用、4 条 driver 性能提示），
它们继续属于下方已知限制，没有将 error 为 0 描述成 warning 为 0。

截图：[白天](images/flappybird-recheck-day-2026-10-02.png)、[夜间](images/flappybird-recheck-night-2026-10-02.png)、
[Score/Best 1](images/flappybird-recheck-game-over-2026-10-02.png)。

首次 Support Pack 验证被 Windows PowerShell 5 的 native stderr/Stop 处理打断：
HarfBuzz 上游 CMake 警告被测试脚本当作终止错误。改用 PowerShell 7、按进程 exit code 判定后继续成功。
没有修改第三方源码或把该警告伪装成引擎编译错误。
首次 headless 命令还使用了不存在的 Node 路径；已发现实际 Node executable 并修正验证脚本。
这两次属于验收脚本问题，保留日志，不计为成功验证。

## 已知限制与证据

- Windows 上的 16 项 macOS/Metal shader 测试按其平台限定跳过，未计入 passed。
- 没有操作可见 GUI、拖拽窗口或启动 Desktop Player；Editor 事件策略有自动测试，实机窗口验收仍未执行。
- 没有在 macOS/Linux 主机执行原生构建或运行。公共实现和边界已统一，不将 Windows 结果写成这些主机的实测结果。
- 浏览器验证使用静音软件渲染；不代表可听音频、硬件 GPU 性能或所有浏览器通过。
- 前轮已记录的 WebGL capability、ScriptProcessorNode、SwiftShader warning 没有在本次修改中治理。
- 前轮一次 Artifact staging access denied 的外部来源仍未确定；其历史失败和成功重试继续保留在上一轮报告。
- 本报告不覆盖历史台账中其他 Rendering/Canvas 收口的独立验收门禁。

日志位于当前机器 `%TEMP%`：

- `InnoRecheckAcceptanceTests/*.trx`、`summary.json`：各项目逐项结果。
- `inno-recheck-solution-sequential.log`、`inno-recheck-cli-release.log`、`inno-recheck-architecture-final.log`：最后一轮门禁。
- `inno-recheck-editor-last.log`：最后焦点边界后的 Editor 构建；最新 PlayMode TRX 为 51 项。
- `inno-recheck-support-windows-first-attempt.log`：PowerShell 5 首次中断前的输出。
- `inno-recheck-support-windows-x64.log`、`inno-recheck-support-browser-wasm.log`：平台 Support Pack。
- `inno-recheck-flappy-windows-x64-export.log`、`inno-recheck-flappy-browser-wasm-export.log`：实际 Samples 导出。
- `inno-recheck-web-headless.log`、`FlappyBirdRecheckHeadless/flappybird-repair-headless.json`：浏览器运行记录。

Windows 无法访问规范指定的 `/System/Library/Sounds/Glass.aiff`，完成提示音未播放。
