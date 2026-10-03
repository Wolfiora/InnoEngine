# 提交前 Sample 导入修复与 BGCS 独立性复核（2026-10-03）

[架构索引](README.md) · [Wiki 首页](../README.md) · [上轮审查](PRECOMMIT_BGCS_AUDIT_2026_10_02.md) · [当前问题入口](CURRENT_ISSUES.md)

## 范围

本轮修复 Editor Import Sample 阻塞主线程的 P2，继续复核当前未提交的宿主、输入、导出、Native 消费、绑定生成、退休、公开边界和可读性。额外按用户要求清理独立 BGCS 仓库的引擎专属文档及测试示例。两仓库的证据分别归属自身；引擎集成结果只在引擎报告中记录。

未提交代码；保留其他既有修改。用户后续允许 computer use；本轮必要验证仍通过 CLI、公开契约测试或静音 headless 浏览器进行，没有打开可见应用。

## Sample 导入修复

| 阶段 | 当前职责与执行边界 |
| --- | --- |
| 请求 | File Browser Action 只请求内部 Sample Module；Pipeline 在 owner thread 校验目标、捕获 Serialization generation 和 source rewriter，并取得共同 generation read lease。 |
| 私有准备 | 后台分块复制、哈希、身份与路径重写；完整保留目录名及所有前导 `~`。根 sidecar 与 Importer settings 中的引用也重写。 |
| 候选导入 | 后台复用现有 isolated Catalog、AssetLoader 和 FileSystem；扫描及 Importer 接收取消 token。活动索引与 Identity 在成功前不发布。 |
| 脚本验证 | owner 捕获只读候选源；API reference、Roslyn、缓存与产物写入明确调度到后台。串行 reference 准入与 emit 也可取消。 |
| History | 后台生成中立 archive/payload；owner safe point 采用共同 SourceMount/ReferenceRecovery 事务并记录现有 History。 |
| 完成 | owner 发布一次 Changed，选择导入目录并关闭共用阻塞 modal；自动脚本编译在读租约释放后处理保留的请求。 |
| 取消 / 失败 / 退出 | Core LifetimeScope / RetirementBarrier 排空任务，再 rollback 与后台清理。Sample Module 先于 Scripting 停止，未结束的任务不能提前释放依赖。 |

没有保留旧同步 ImportSample API、第二套 Undo 栈、resolver、事件队列或资产数据库。没有按 Sample 或 Web 类型选择第二套编译器。

原代码不仅在 Action 中同步等待 preflight；冷缓存时，async 编译器在第一次真正异步等待前也会同步生成 reference。只删除 GetResult 不足以修复该问题，因此统一编译入口改为 owner capture → background compilation。

进一步移除了候选索引前的 watcher 静默等待和旧 Catalog 同步导入：候选后台完整扫描当前源，结束后强制对账暂停期间的外部变化。真实 watcher 回归在修复前测得单次 owner Advance 等待 6.017 秒；修复后的初次回归整项测试耗时 993 ms。它不是模拟的未完成编译任务；最终矩阵还覆盖 commit/rollback 后对账暂停期间的外部新文件。
该回归还发现强制全量对账只刷新 Loader、没有同步 FileSystem 索引。已在通用完整对账与恢复 rescan 路径中补齐索引刷新，避免 observer 或 File Browser 看到过时目录。

公开行为变更及必要性：

- `AssetPipeline.PrepareSampleImport` / `AssetSampleImportTransaction`：明确准备、验证、提交、取消及退休的所有权。
- `IAssetSourceSnapshot`：活动与候选源共用最小只读编译入口，不提供发布方法，不向脚本导出。
- `CompileAuthoringGenerationAsync(..., sourceSnapshot)`：验证尚未发布的输入；runtime deployment 共用后台计算边界。
- `AssetLoader.Rescan(cancellationToken)` / `AssetSampleTransformContext.cancellationToken`：让扫描、Importer 和 source rewriter 响应同一事务取消。

普通失败可以 rollback；已经完成但报告 Pending ownership 的任务必须保持 Core lifetime 的失败状态并 Fault Host。退休 timeout 不放宽，不能把未释放的 collectible generation 当作成功。

owner safe point 仍需捕获恢复状态、采用候选和发布，任意用户扩展也可能耗时；这里不承诺所有项目的固定单帧预算。事务是 owner-safe publication 和文件系统补偿，不是跨多文件 crash-atomic 写入。

## BGCS 的独立边界

BGCS 文档中两处使用 InnoEngine Native / FlappyBird 证明 Wasm 支持的描述已删除，两处以 Inno.Native 风格定义 FunctionTable 的描述改为调用方持有 context 的通用契约；单文件生成测试使用中立示例名。唯一保留的 InnoEngine 字符串属于 library-agnostic 架构测试的禁止名单。

BGCS 的 capability matrix 只采用自身测试：target triple、32 位 pointer/record layout、三种 import mode 的 opaque handle。独立 Wasm invocation、打包及其他作者主机/浏览器验收仍明确待完成，不能借引擎集成结果称为 BGCS 全平台认证。

| BGCS 累积修改 | 所属能力与判断 |
| --- | --- |
| Wasm32 / Emscripten target、ABI、preset | 通用编译目标模型，与 Windows/MSVC、Darwin 等目标共用解析链；每个库不新增生成器。 |
| 环境变量与 config-relative 路径 | 通用工具配置。本轮修复 compiler 变量展开前误判 bare command 的顺序问题；绝对与相对编译器路径均有独立 fixture。 |
| Opaque handle 与 callback carrier | C ABI 使用 pointer-sized carrier，managed API 保留类型化 wrapper；所有目标和 import mode 复用。 |
| Clang builtin resource bundle | 解析器与自身主版本的 builtin headers 配套；SDK/sysroot/标准库仍来自显式目标输入。bundle 20.1.8 与 native libclang 20.1.2 同主版本但补丁不同，不声称完全同版本。 |
| Compiler probe 输出 / timeout | 通用子进程排空与退出处理，不涉及引擎、图形或浏览器输入。 |

源码复核没有 BGCS → InnoEngine 的引用、库名分支或应用运行时逻辑。BGCS 内部仍使用原有 target/configuration/IR/emitter；consumer 的 SDK、native facade、link graph 和部署属于 consumer。BGCS 修改不改变 Shader 渲染公式。

本轮 compiler-path regression 曾在恢复原实现时确认失败，在修复后通过；不是仅检查修改后的代码文本。BGCS 本身的成功证据见其独立测试输出；本报告只记录消费关系，不能反向成为 BGCS 的验收依赖。

## Emscripten wasm32 是什么

作者主机、编译目标和运行环境是不同概念。Windows/macOS/Linux 可以作为 Emscripten 作者主机，输出 WebAssembly；`wasm32` 是目标的 32 位指针地址模型。它不表示 Windows 浏览器，也不覆盖所有 WebAssembly 环境。WASI 等环境有不同 target 与系统契约。[Emscripten 安装](https://emscripten.org/docs/getting_started/downloads.html)、[WebAssembly 输出](https://emscripten.org/docs/compiling/WebAssembly.html)。

Binding 必须按目标解析 `long`、pointer、alignment、calling convention 和 target headers。否则 64 位作者主机上的解析结果可能被错误当成 32 位 Wasm ABI。Clang 明确区分 target triple、sysroot 和宿主工具链输入；BGCS 识别 Emscripten 属于必要的通用目标支持。[Clang 交叉编译](https://clang.llvm.org/docs/CrossCompilation.html)。

必要的是一份目标描述及显式 SDK 输入；不需要每个 native 库拥有另一套 BGCS 生成逻辑。引擎 Browser 工具链的静态链接、宿主帧调度和存储，以及 SDK SjLj lowering，继续属于引擎平台实现。参见 [Browser 工具链](../build/Inno.Build.Toolchains.Browser.md)及 [Web Player 边界](WEB_PLAYER_ARCHITECTURE.md)。

## 验证

以下为最终源码的实际结果。测试输出位于本机 `%TEMP%`；它们是本轮证据，不是引擎或 BGCS 的运行依赖。

| 验证 | 结果 | 证据 |
| --- | --- | --- |
| 引擎 Solution / Release Build CLI | 两者均 0 warning、0 error | `inno-fifth-accepted-solution.log` / `inno-fifth-accepted-cli.log` |
| 仓库架构验证 | passed | `inno-fifth-accepted-architecture.log` |
| 引擎 50 个测试项目 | 1434 passed、0 failed、16 skipped，共 1450 | `InnoFifthAcceptedAggregate/summary.json`，逐项目指向原始 TRX |
| BGCS 独立 11 个测试项目 | 710 passed、0 failed、0 skipped | `InnoFifthFinalBgcsTests/summary.json` 与各项目 TRX |
| 七个 Native 的 GenerateBindings / CheckBindings | Bgfx、ImGui、ImGuizmo、MiniAudio、Sdl3、Text、UI 全部通过 | `inno-fifth-final-Inno.Native.<Name>-GenerateBindings.log` / `CheckBindings.log` |
| Windows / Browser Support Pack | 两个目标均生成成功 | `inno-fifth-final-support-windows-x64.log` / `inno-fifth-final-support-browser-wasm.log` |
| FlappyBird 最终 Windows / Web 导出 | 两个目标均成功 | `inno-fifth-accepted-flappy-windows-export.log` / `inno-fifth-accepted-flappy-browser-export.log` |
| FlappyBird 静音 headless Web | 运行 error 0；日夜切换、星星 glow、3 次 flap、得分、重开及刷新后 best score=1 均通过 | `FlappyBirdFifthFinalHeadless/flappybird-repair-headless.json` 与七张截图 |
| 真实 CLI Sample 导入 | 冷缓存 FlappyBird 副本导入成功并保留 `~Samples`；重复导入 exit 1；四个子树文件及根 sidecar 的哈希保持不变 | `inno-fifth-accepted-cli-sample.log`、`inno-fifth-accepted-cli-sample-hashes.log`、`inno-fifth-accepted-cli-sample-root-hash.log` |
| 两仓库文档 / diff | Markdown 文件目标无缺失；`git diff --check` 通过 | 本轮链接审查及 diff 输出 |

引擎汇总采用最终矩阵中通过的 49 个项目，加上修正测试等待方式后的完整 Scripting 重跑（138 passed、0 failed，`InnoFifthAcceptedScripting/Inno.Editor.Scripting.Tests.trx`）。原矩阵和失败 TRX 保留，未覆盖为成功。该测试原先在 `EnsureReady` 推进一次协作式收集后就假定 GC 已完成；现改为先调用真实公开 `GenerationCoordinator.Wait()`，再验证可继续工作。Full GC、finalizer 和弱引用退休验证没有省略，生产 timeout 没有放宽。16 skipped 均要求本机没有的 macOS BGFX Metal compiler，不计为通过。

Sample 回归覆盖后台 source transform/Importer、真实 watcher 静默周期、候选索引不可提前发布、成功与取消后的外部文件对账、身份与 sidecar 引用重写、非法源码 rollback、重入拒绝、History Undo/Redo，以及 Editor 停止期间的任务/代际所有权。BGCS compiler-path 回归在旧实现下确认失败后，修复的绝对/相对环境变量路径均通过。

Web headless 仍记录 315 条既有 WebGL capability 查询 warning；没有隐藏它们，也不将 error=0 描述成零警告。夜间测得 6772 个符合阈值的 glow 像素，并已查看夜间截图；此证据只覆盖当前 FlappyBird 和该浏览器环境。

## 保留问题与提交判断

本轮两次 Windows FlappyBird 导出出现 `Library/Artifacts/.staging` 访问拒绝，各自重试成功。已检查 staging 生命周期、权限和同时运行的引擎进程；另外通过临时 CLR first-chance observer（公开 CLR 诊断入口，不改引擎）验证重试及独立冷缓存 FlappyBird 副本，两者成功且未捕获访问拒绝。尚未确定外部文件占用或其他根因；不能把它宣称为已修复，也没有加入未经证明的 IO retry 或平台特例。

macOS/Linux 实机、可见 Editor 的布局/高 DPI 矩阵和音频可听性没有在本轮验收。历史 Rendering/Canvas 视觉门禁继续保留；公开契约和 headless 验证不替代它们。

提交时需分别保存 BGCS 与引擎的关联修改及 BGCS 自有资源 bundle；不能只提交引擎并依赖本机未提交的生成器。修复范围和自动门禁通过不等于整个引擎已无保留验收。当前 Windows 环境不存在规范指定的 `/System/Library/Sounds/Glass.aiff`，提示音未能播放。
