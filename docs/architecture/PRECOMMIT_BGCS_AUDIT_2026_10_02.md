# 提交前第四轮复核与 BGCS 修改说明（2026-10-02）

[架构索引](README.md) · [Wiki 首页](../README.md) · [上轮全范围复核](PRECOMMIT_FULL_AUDIT_2026_10_02.md) · [当前问题入口](CURRENT_ISSUES.md)

## 范围和结论

本轮继续检查当前未提交源码的领域边界、公开封装、事件消费、生命周期、生成器 ABI、构建工具进程、
渲染与输入共享路径、代码可读性和文档证据。读取 InnoEngine、BindGen-CS 及 Rendering2D 当前差异，
重新生成全部宿主绑定，运行完整引擎与 BGCS 测试矩阵，并使用 Samples/FlappyBird 验证实际导出。
这不是对全部源码逐行阅读或所有硬件性能的认证。

确认并修复两处 BGCS 问题；补齐五个落后的配置测试预期；纠正文档把历史验证缺口描述为功能未实现的错误。
另保留一个已确认的 P2：Editor Import Sample 在主线程同步等待预编译，尚未改造成可绘制进度和取消的流程。
本轮没有新增 public API、兼容包装、Native Browser 副本、渲染算法或输入入口。

没有执行 Git commit。验证采用 CLI、低优先级、受限编译预算和静音 headless 浏览器；没有使用 computer use、
启动可见 Editor/Player 或切换前台焦点。当前 Windows 环境不存在指定的 Glass.aiff，未播放提示音。

## 确认的问题与处理

| 优先级 / 项目 | 证据 | 处理与结果 |
| --- | --- | --- |
| P1：FunctionTable 漏用 opaque handle 原生载体转换 | 扩展原测试至三个目标 × 三种 import mode，修复前仅 FunctionTable 的三个用例失败。其 `delegate*` 直接携带 managed handle struct，与另外两条导入路径不一致。 | 三种 mode 统一使用 `nint` 原生载体与同一 typed adapter；不增加平台分支。九个用例通过，DllImport/FunctionTable 做 C# 语义编译，LibraryImport 使用真实 SDK build 执行 source generator。 |
| P1：编译器 resource 查询可能被 stderr 堵塞 | 实际 C 编译器查询 fixture 输出约 560 KB stderr；修复前 stdout 同步读取无法结束，15 秒边界测试失败。 | resource、include 和 fingerprint 查询共用同时读取两条流的进程实现。超时终止进程树、等待退出并观察输出任务。阻塞输出与 timeout 退休测试均通过。 |
| 配置测试仍要求旧 handle 导入签名 | 全部 11 个 BGCS 测试项目首次运行发现 Configuration 的五项失败；之前三个主要项目的矩阵未覆盖这些测试。 | 同步五份 expected bindings，检查 `nint` 原生导入、类型化 Native 包装和 `.Handle` 转换，同时保留 extension/naming 的原断言。最终 Configuration 147 项通过。 |
| 历史 Rendering/Canvas 门禁描述错误 | 2026-09-26 报告已有 route/input 实现、CLI Import Sample 和 TestProject Player 构建通过记录。 | Current Issues 与第三轮报告改为缺少真实双模型图像、交互/Transform/高 DPI 矩阵及 Editor 菜单留证，不再称上述功能未实现或构建失败。 |

生产修改位于 BindGen-CS 的 `CSharpEmitter.cs` 与 `CppToolchainDiscovery.cs`；对应英文 XML、
中英文架构文档及公开边界测试同步更新。没有手改生成物，Inno 的 binding 通过既有 MSBuild 入口重新生成。
原生载体参数继续使用现有 metadata emitter，保留 NativeName 等参数元数据，不再在 handle 分支手写一个会遗漏 metadata 的参数循环。

## 为什么 BGCS 仍有修改

上轮第三轮审查没有修改 BGCS 源码；Git 显示的是 Web 实施与第一轮修复尚未提交的累积变更。
本轮又补充上述两处通用缺陷修复及完整测试预期。这些必须与引擎消费端一起提交，不能只提交引擎后继续依赖本机脏工具源码。

| 累积修改 | 必要性 | 通用性 / 平台差异归属 |
| --- | --- | --- |
| Wasm32 / Emscripten target、ABI 和 preset | 解析器必须知道目标指针宽度、布局与预处理环境，并拒绝错误的目标组合。 | 这是编译器正常的目标支持；与 Windows/macOS target 一样留在工具层，对每个 native 库共用。 |
| C/C++ 配置路径的环境变量展开 | 由消费工具链传入真实 SDK、sysroot 和编译器路径。 | 所有目标可用；BGCS 不发现 .NET workload，不写入开发者机器路径。 |
| Callback 的 incomplete record/alias 指针载体 | C callback 参数是原生指针，不能被误当成 typed managed handle 的值传递。 | 通用 C ABI 修正，不依赖 SDL、BGFX 或 Inno 名称。 |
| Opaque handle 的 `nint` import + typed adapter | 保持 managed 类型安全，同时让实际 ABI 使用原生指针。 | DllImport、LibraryImport、FunctionTable 统一，桌面与 wasm 共用规则。 |
| Clang builtin resource headers 随解析器提供 | 避免系统 LLVM 升级后把较新内置语法交给包内较旧主版本解析器。 | 宿主和交叉目标共用；SDK/标准库仍由显式目标输入决定。 |
| 本轮查询进程的输出与 timeout 处理 | 防止诊断输出导致探测卡住，并避免遗留 owned compiler process。 | 对全部平台的编译器探测适用。 |

BGCS 源码中没有 Inno、BGFX、SDL、MiniAudio、RmlUi、EM_CACHE、browser-wasm 或 `OperatingSystem.IsBrowser` 的引擎专用判断。
环境变量名字和目标 ID 在消费配置与 Browser 工具链中声明，库特有事实继续留在各 Native 项目的 BGCS 配置。

解析器 native libclang 为 20.1.2，内置 header bundle 为同主版本 20.1.8；它们不是相同补丁版本。
新增 bundle 含原样第三方 headers、来源校验和和 Apache-2.0 WITH LLVM-exception 许可，位于 BGCS 的
`extern/clang-resource/`。没有修改 InnoEngine 的 extern 源码。升级解析器主版本需要同步更新 bundle 并复验。

## Web 专用部分的真实边界

Browser target、静态链接、帧调度和 localStorage 仍需平台实现。共享 Core、Shell、Player Runtime
没有浏览器能力推断，游戏和输入系统没有复制。Rendering2D 的当前差异删除了重复 light UV 翻转及其未使用 uniform，
没有加入 Browser 分支；本轮不改变其渲染公式。

`wasm_sjlj_shim.c` 是明确的 SDK lowering 补足，不能称为“完全没有平台专用实现”。它属于 Browser 工具链，
不是 BGCS 生成器改动。实际 MSBuild workload 9.0.20 选择 Emscripten pack 3.1.56，其 clang 报告 19.1.0。
同一测试程序无 shim 时因 `__wasm_setjmp` / `__wasm_setjmp_test` 未定义失败；加入后实际 wasm 运行通过：
嵌套 jump buffer、跨中间调用帧、零值归一化及 20 次重复 invocation。
详细职责和 SDK 升级后的删除/复验要求见 [Browser 工具链](../platform/Browser/Inno.Build.Browser.md)。

这些 BGCS 修复不改变颜色空间、光照方向或 Shader 输出；重新生成和实际导出用于检查消费链是否回退。
当前分层把平台差异集中在目标和 adapter/toolchain 中，但工具链与第三方升级仍需要维护，不能承诺永久无需适配。

宿主生成文件此次记录 Windows ABI；与仓库基线的 macOS 生成结果比较时，可能看到 enum underlying type 等解析结果差异。
这属于按目标头文件与编译器生成的产物，不能把一次 Windows CheckBindings 推断为 macOS ABI 认证。
统一 `engine` 构建会先重新生成当前宿主 binding，Browser 则生成隔离的 wasm32 输出；不同目标共用生成规则，但仍需各自的目标布局。

## 验证证据

| 验证 | 本轮实际结果 |
| --- | --- |
| InnoEngine Debug solution | 0 warnings / 0 errors |
| Release Build CLI | 0 warnings / 0 errors |
| 七个宿主 Native GenerateBindings / CheckBindings | 全部通过；重新生成与当前产物一致 |
| 仓库级架构验证 | `InnoEngine architecture validation passed` |
| 引擎完整矩阵 | 50 项目，1437 total，1421 passed / 0 failed / 16 平台限定 skipped |
| BGCS 完整矩阵 | 11 项目，708 passed / 0 failed / 0 skipped |
| Opaque ABI 回归 | 三目标 × 三 import mode 共九用例通过；目标标记测试不替代 macOS ABI 实机验证 |
| 编译器 process 边界 | 实际输出拥塞与 timeout 退休，两用例通过 |
| SDK SjLj 补足 | 无补足链接失败；加入后真实 wasm 控制流测试通过 |
| Windows Support Pack / FlappyBird | 从源码准备并导出成功 |
| Web Support Pack / FlappyBird | 实际 SDK 生成五套目标 binding、准备 pack 并链接导出成功 |
| 静音 headless Web 回归 | `succeeded=true`，运行 error 0；昼夜、星光、输入、得分、重开和刷新持久化通过 |

引擎 TRX 与汇总：`%TEMP%/InnoFourthAuditAcceptanceTests/`。
BGCS 初次矩阵与配置失败记录：`%TEMP%/InnoFourthAuditBgcsTests/`；配置最终复验和最终汇总：
`%TEMP%/InnoFourthAuditBgcsFinal/`。初次失败证据没有被最终通过结果覆盖。
Compiler/opaque 修复前后的定向 TRX 位于 `%TEMP%/InnoFourthAuditTests/`。
SDK 补足测试源码、链接日志与运行日志位于 `%TEMP%/InnoFourthAuditSjLj/`。

FlappyBird 导出位于 `%TEMP%/FlappyBirdFourthAuditExports/`，两个目标的 Support Pack 位于
`%TEMP%/InnoFourthAuditSupportPacks/`。headless 的 JSON 与七张截图位于 `%TEMP%/FlappyBirdFourthAuditHeadless/`。
白天天空均值 155.3333，夜晚 9，指定星光区域超过阈值的像素为 3201；三次 flap 后得分/最高分达到 1，
重开与刷新后仍读到最高分 1。运行测试的 browser 与 HTTP server 在 finally 中关闭。

本轮仍出现 315 条既有浏览器 warning：310 条 WebGL `getInternalformatParameter` 能力查询警告，
两条 ScriptProcessorNode deprecated，三条 SwiftShader driver/performance 提示。
因此运行 error 为 0 不能描述成浏览器 console 零 warning；本轮没有更改第三方实现来隐藏这些信息。

## 仍需整改或独立验收

### P2：Editor Import Sample 的同步预编译等待

`ImportAssetSampleCommand.Execute` 在 owner-thread `AssetPipeline.ImportSample` 的 validation callback 内调用
`CompileAuthoringGenerationAsync().GetAwaiter().GetResult()`。未命中缓存且编译任务未完成时，当前 Editor 帧必须等待，
无法继续绘制进度或接收取消。源码已经确认此阻塞；本轮没有测量特定项目的卡顿时长，也不把它推断为必然死锁。

现有导入需要维持候选校验、失败回滚、watcher/observer 隔离及成功后的 History 记录。
应先在资产领域定义 preparation/validation/commit 的 owner-thread 边界，再复用现有 Editor 进度与取消 presentation。
不能把整个 `ImportSample` 塞进 Task.Run，或在 Panel 另建事件队列、候选状态机和 Undo 栈。
本轮审查把它作为独立整改项保留，没有用未经验证的临时异步包装改变事务。

### 验证边界

macOS/Linux 实机、可见 Editor 的窗口交互与音频可听性、真实 GPU 性能、历史双模型 Rendering/Canvas
视觉矩阵仍缺独立验收。平台限定测试跳过不等于通过；headless 软件 WebGL 不代表所有浏览器和 GPU。
完整历史状态仍见 [当前问题入口](CURRENT_ISSUES.md)，本轮不能宣布整个引擎零问题或无保留验收。
