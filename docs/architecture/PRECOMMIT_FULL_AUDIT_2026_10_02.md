# 提交前全范围复核与补充修复（2026-10-02）

[架构索引](README.md) · [Wiki 首页](../README.md) · [上轮复核](PRECOMMIT_RECHECK_ACCEPTANCE_2026_10_02.md) · [当前问题入口](CURRENT_ISSUES.md)

## 范围与当前结论

本轮对当前未提交改动继续检查分层、公开封装、Core 复用、生命周期、文件安全、输入消费、
平台构建、渲染热路径、代码可读性与文档一致性。重点读取共享 Core、Shell/Player、Editor 输入与导出、
Support Pack 和 Browser 工具链，并运行全部 50 个引擎测试项目与仓库级架构验证。
确认了下面八项补充问题，已修改实现、调用方、公开 XML 和 Wiki；没有增加新的 public 入口或兼容包装。

本报告的最终结果只覆盖实际执行的验证。macOS/Linux 实机、可见 Editor 窗口交互、硬件 GPU 性能和
历史 Rendering/Canvas 的独立验收仍有明确缺口，不能据此宣布整个引擎无保留验收。

未使用 computer use，也没有启动可见 Editor/Player、切换焦点或播放游戏音频。
后台验证采用低优先级与受限编译并行度。没有执行 Git commit；BindGen-CS 沿用前轮修改。

## 确认的问题与修复

| 项目 | 原因与风险 | 当前实现与验收 |
| --- | --- | --- |
| 1. 目录提交后的备份清理失败会破坏新输出 | 原协议把旧备份递归删除放在安装回滚的 try 内。清理已删除部分旧内容后出错，可能删除完整新目录，再恢复残缺备份。 | 候选安装是提交点；旧备份清理位于提交之后。清理失败保留完整新目录及剩余备份，IO 异常明确说明已安装和备份位置。Windows 只读文件故障测试通过。 |
| 2. 目录重叠与安装失败的回滚边界不完整 | 候选与目标相同或相互包含时先移动目录，之后才能发现失败；恢复时强行删除目标还会损害另一 writer 的数据。 | 首次 mutation 前拒绝同一路径和两种包含关系。候选无法移动时只尝试恢复旧目录；恢复也失败时保留树并聚合两次失败。成功、重叠和真实 Windows 文件占用回滚测试通过。 |
| 3. Volume root 的路径边界错误且 Storage 重复实现 | `C:\` 或 `/` 本身已有 separator，再拼一次导致合法子路径被拒绝；Storage 又维护第二套 prefix/comparison 判断。 | Core `PathBoundary` 正确处理已有 separator；FileSystem Storage 直接复用 Core 边界，保留 key 和 symlink 校验。根目录解析与真实 Storage 访问测试通过，不在系统根写入数据。 |
| 4. 已消费的鼠标移动留下过期命中状态 | 全局消费标记使 Route 提前返回，鼠标已离开 Game View 时，后续 press/wheel 仍可能使用上次 inside 状态。 | 当前目标窗口的 move 先更新几何状态，再按消费策略决定是否交付。消费事件不进入 Game backend；既有 drag capture 与其他窗口隔离保留。新增三个边界用例通过。 |
| 5. 异步 Build API 在 snapshot 阶段阻塞异步 Pack 供给 | `BuildGameAsync` 内部同步等待 provisioner，导致调用方被阻塞，并给依赖 UI continuation 的 provider 带来死锁风险。 | Pack 准备显式使用现有 `EnsurePlayerSupportPackAsync`；Build snapshot 只验证已准备 Pack，缺失立即失败。Editor 在后续 owner-thread update 启动 build；CLI 在 composition root 协调准备。挂起准备、取消与后续成功导出测试通过。 |
| 6. Editor 停止时没有等待导出清理，两个导出可共享取消源 | Stop 只取消并立即释放 CTS/services；仍运行的准备、构建或清理持有旧服务。另一个 workflow 还可能替换 CTS。 | 所有导出共享互斥 admission。Stop/Dispose 先取消，复用现有 `RetirementPendingException` 和 Editor 退休屏障，清理完成后再观察结果并释放资源。实际 File Action、原生 ImGui 点击和 public Module.Stop/Runtime.Dispose 集成测试通过。退出中的准备不会再启动新 build。 |
| 7. Web 编译被硬编码为单 worker | EMCC、Binaryen、CMake 与绑定生成强制串行，把运行时线程策略混入构建策略。 | 工具链不覆盖 EMCC/Binaryen 的 worker 环境，也不固定 CMake 或绑定生成的单 worker。继承工具默认值或宿主预算。环境契约测试与真实 Web 构建验证；没有改动运行时线程能力。 |
| 8. 公开 Build 目标清单可被外部修改 | `IReadOnlyList` 实际返回裸数组，调用者可 cast 后修改，导致 UI 清单与内部注册表不一致。 | 返回缓存的 `Array.AsReadOnly` 集合视图；通过可写接口赋值会明确失败。没有复制每次读取或增加第二套 target 注册。封装测试通过。 |

Support Pack Publisher 已删除自身的重复目录移动协议，直接复用 `Inno.Core.IO.AtomicDirectory`。
Directory install 使用两次移动；Host 必须协调同一目标的读写，移动间可能短暂没有目标目录。
源码、XML 与 Wiki 已明确这一约束，不把它描述成文件式的一次无间隙替换。

## 架构与公开行为

| 边界 | 复核结果 |
| --- | --- |
| Foundation 与平台能力 | Core、Shell、Player Runtime 未新增 Browser 分支；输入继续经过 Core Events。 |
| Editor 与 Build | 通用 Build 不引用 Editor；两阶段编排分别位于 Editor feature 与 CLI composition root。 |
| 基础能力复用 | Storage 路径校验与 Pack 目录提交回归 Core.IO；退出等待使用已有 Core.Execution/Editor 生命周期契约。 |
| 平台 target | 仍通过开放 target ID、packager、source 和 validator 组合，无新增中央平台 switch。 |
| Native 与第三方 | 无新 Native Browser 副本，无手写替代 binding，无第三方源码改动。 |
| Rendering | 本轮没有修改光照、颜色、Shader IR 或输出公式；检查共享 compositor 的 GPU buffer 生命周期与 capability 分支，未发现需要新增 Web 特判的问题。 |
| Entry point | 生产入口仍是 Editor、Desktop Player、Browser Player、统一 Build CLI；build 只有一个 Program。 |
| Reload/Identity | 核对统一标准；没有新增旧代 delegate/object 持久化、测试后门、反射穿透或 `InternalsVisibleTo`。 |

公开行为变化使用既有 API：

- `AtomicDirectory.Install` 在任何修改前拒绝重叠树；提交后清理失败明确报告，保留完整当前输出。
- `PathBoundary` 和 FileSystem Storage 接受 volume root 的合法子路径，继续拒绝越界。
- `EditorGameInputCapture.Route` 观察 consumed move 的位置，不向游戏重放此事件。
- `BuildPipeline.BuildGameAsync` 要求目标 Pack 预先准备，调用方必须在 authoring owner thread 启动 snapshot。
- `BuildPipeline.availableGameTargets` 是不可写的集合视图。
- Browser toolchain 的编译资源预算由宿主环境或工具默认策略决定。

没有移除 GC unload verification、generation admission、Faulted barrier 或 last-good 资源保护。
本轮没有进行硬件性能基准，不能把移除 worker 限制写成已经测得某个倍数的加速。

## 回归结果

| 门禁 | 最终结果 |
| --- | --- |
| 全部 50 个引擎测试项目 | **1437 total / 1421 passed / 0 failed / 16 skipped** |
| Core.IO / Storage / Editor PlayMode / Build | **9 / 8 / 54 / 61 项通过** |
| Scripting / Architecture 测试 | **125 / 50 项通过** |
| Debug 完整解决方案 | 0 warnings / 0 errors |
| 最终 Release Build CLI / Debug Exporting | 均为 0 warnings / 0 errors；包含最后的 XML 与完成状态文案修正 |
| 仓库架构验证 | 通过 |
| 两仓库 diff check | 通过 |
| Markdown 相对文件链接 | 缺失目标为 0 |

相较上轮增加 13 个回归用例。新增测试通过真实公开契约执行，不扩大 production API 可见性。
Export 集成测试只创建内存中的 ImGui context，没有创建 OS window、GPU device 或音频输出。
最后一次 Build suite 已重新编译并完整运行，包含确定性的 Stop barrier 验证。

验证过程曾发现测试布置错误：Windows 开放的旧目录文件句柄先阻止了 backup move，未进入预期的
post-commit cleanup 阶段。调整为只读属性后验证清理失败；另用候选文件占用独立验证安装失败的回滚。
该次失败不计入最终通过结果，也不把它描述成完成了目标清理场景的复现。

## FlappyBird 实际导出

使用 `C:/Dev/GameEngineDev/InnoEngine.Samples/FlappyBird`，startup scene 为 `FlappyBird/FlappyBird.iscene`。

| 流程 | 实际结果 |
| --- | --- |
| Windows Support Pack 与 FlappyBird 导出 | 成功，exit 0；完整输出安装 |
| Browser Support Pack 与 FlappyBird 导出 | 成功，exit 0；完整输出安装 |
| 静音 headless Edge / SwiftShader | 成功；runtime/page/HTTP/console error 为 0 |
| 昼夜切换与星光 | 真实 N 键切换，天空均值 155.33 → 9；当前检测区域 3191 个发光像素，截图中星光可见 |
| 玩法、重开与持久化 | 真实 pointer/Space 输入，3 次 flap 得到 Score/Best 1；Space 重开，刷新后 best 仍为 1 |
| 退出清理 | 浏览器与临时 HTTP server 均已关闭，没有可见桌面窗口 |

产物位于 `%TEMP%/FlappyBirdThirdAuditExports/windows-x64/FlappyBird-Windows-x64` 与
`%TEMP%/FlappyBirdThirdAuditExports/browser-wasm/FlappyBird-Web`。
最高分键为 `inno-storage:/persistent/flappybird/Storage/flappy-bird/best-score.txt`，本次值为 `1`。
7 张实际截图和 JSON 位于 `%TEMP%/FlappyBirdThirdAuditHeadless`；夜间与 Game Over 截图已目视核对。

玩法阶段沿用前轮验证脚本逐次推进外部帧调度机会，没有替换 Time、Physics、Input 或 Storage；
启动、昼夜切换和刷新采用正常调度。该控制用于稳定验证玩法，不是性能基准。
本次保留 315 条 warning：310 条 WebGL capability 查询、2 条 ScriptProcessorNode 弃用、
3 条 SwiftShader driver 性能提示。没有把 error 为 0 写成 warning 为 0。
本轮移除的 worker 硬编码已在真实 Web 构建中验证；为了不影响前台游戏，验证进程显式设置
EMCC、Binaryen 和 CMake 的 worker 预算为 1，没有改回产品默认值。

## 提交清洁度与保留边界

`extern/cimguizmo` 曾显示未跟踪编译垃圾，确认是 2026-09-26 的 obj/pdb/lib/exp/dll/build 产物。
已逐项确认未跟踪、核对路径边界并可逆移动到 `%TEMP%/InnoThirdAuditQuarantine-1b1a6806806449a18e4e1ffc9058fbe2`，
附带 `manifest.json`；没有删除或修改第三方源码。当前该 submodule 工作树干净。
当前 native builder 的中间产物使用 toolchain `obj/native`，不再依赖这些旧产物。

提交时需要一并考虑前轮的 BindGen-CS 修改：目标 ABI、LLVM resource headers 与 binding 生成契约是
当前引擎构建的依赖。该仓库前轮 284 项测试通过；本轮未修改其源码，不将历史结果计入上述 1421 项。

仍保留的验证边界：

- Windows 上 16 项 macOS/Metal shader 测试按平台限定跳过；没有 macOS/Linux 实机 native 构建与运行。
- 没有执行可见 GUI 的窗口拖拽、重叠与交互验收；自动输入和内存 ImGui 测试不能替代它。
- Headless Web 使用静音软件渲染，不能代表可听音频、硬件 GPU 性能或所有浏览器。
- 既有 WebGL capability、ScriptProcessorNode 和 SwiftShader warning 没有在本轮治理。
- 前轮一次 Artifact staging access denied 的外部来源尚未确定；其历史失败和成功重试继续保留。
- [历史 Rendering / Canvas / Plugin Samples 门禁](RENDER_CANVAS_ACCEPTANCE_2026_09_26.md)仍未无保留验收，
  缺真实双模型图像、跨 View 交互与 Transform/高 DPI 视觉矩阵、Editor Import Sample 菜单留证。
  实现、自动测试、CLI 导入和 TestProject Player 构建已有历史通过证据；FlappyBird 验证不替代这些独立场景。

## 证据位置

当前机器 `%TEMP%` 下：

- `InnoThirdAuditAcceptanceTests/summary.json` 与各项目 `.trx`：最终 50 项目结果。
- `inno-third-audit-solution-final.log`、`inno-third-audit-cli-release-final.log`、`inno-third-audit-exporting-final.log`、`inno-third-audit-architecture-final.log`：构建与架构门禁。
- `inno-third-audit-build-tests-latest.log`：最后一次 Build suite 的完整重新编译和 61 项测试。
- `InnoThirdAuditTests/pointer-before.trx`、`io-before.trx`：修复前输入及路径边界检查；IO 中清理 fixture 的阶段错误见上文。
- `inno-third-audit-diff-check.log`、`inno-third-audit-bgcs-diff-check.log`：仓库差异检查。
- `inno-third-audit-support-windows-x64.log`、`inno-third-audit-support-browser-wasm.log`：真实 Support Pack。
- `inno-third-audit-flappy-windows-x64-export.log`、`inno-third-audit-flappy-browser-wasm-export.log`：实际 Samples 导出。
- `inno-third-audit-web-headless.log`、`FlappyBirdThirdAuditHeadless/flappybird-repair-headless.json`：运行、图像采样、warning 和持久化证据。

Windows 无法访问规范指定的 `/System/Library/Sounds/Glass.aiff`，完成提示音未播放。
