# Editor UI 闭合实施与验收记录

[执行台账](../EDITOR_UI_CLOSURE_PLAN.md) · [设计规范](../../../docs/architecture/EDITOR_UI_DESIGN_STANDARD.md) · [总 issue #27](https://github.com/Wolfiora/InnoEngine/issues/27) · [PR #26](https://github.com/Wolfiora/InnoEngine/pull/26)

## 结论与适用范围

本轮已经完成实质性源码实现和多项真实原生、SDK、插件及产品验证；**尚不能声明原计划全部闭合，也不能把“企业级、现代级、现象级”作为已经通过的测试等级**。PR 保持 Draft，未授权自动合并。各 issue 维持 Open，完整验收的条目才可由合并时的 `Fixes` 关闭。

主要实现提交为 `63d4f7dce8b21daa22a6f7e5a00825be3f46dd2d`；资产刷新修复为 `273c42ad32be511c21fa21b1e99626af708f29c8`。author、committer 均为 Aaron Liao，无 co-author。后续修复提交为 `8dcceb94`（引导 scope）、`e23f4ba5`（源内容完整性）、`14338d61`（SDK 架构 owner）、`081f41f9`（测试临时 owner）；均为同一作者及提交者，无 co-author。后续提交 `af6eac31` 修正 smoke 的产品预期；`e053be5b` 统一测试的 SDK 输出 owner；`a4d975ab` 修复 Settings 原子 Apply 与异常退休；`0974c317` 同步当前文档。

本机是 Windows；macOS 真实显示器与人工 Metal 操作未执行。模拟 DPI、Native Host 和远程 CI 分别报告，不互相替代。

## 最新候选复验状态

下表优先于后文按阶段记录的历史结果。原始失败、无效运行和未执行项仍保留，不由新结果改写。

| 范围 | 状态与证据 |
| --- | --- |
| 完整十二工程测试图 | 通过；`final-candidate-suites.log/binlog`，0 编译错误；保留 NU1900 漏洞服务网络警告。 |
| Core / Scene | 当前编码及并发改良后 59 + 53 项通过，`e46/core-scene-current.trx`；独立 Release Core 59 项通过。 |
| Workflows | 通过；305 项，`final-candidate/9409749fa03d4badb6f3b34c99a3dcdc`。 |
| Visual / Lifecycle | 通过；分别 136、11 项，`final-candidate` 中对应 profile 的 summary / TRX。套件有交叠，不能直接相加。 |
| Editor Debug / Release、Build 入口 | 通过；`final-candidate-editor-*.log/binlog`、`final-candidate-build-entry.log/binlog`，0 编译错误。 |
| FlappyBird 产品 | 通过；独立的当前 Samples 源码副本，Debug/Release × D3D11/D3D12/Vulkan 六组，每组 300 帧、正常退出；`products/final-candidate-*.log`。该 smoke 不替代完整人工旅程。 |
| Canvas / Rendering2D / FlappyBird | 正式脚本编译及 IDE 投影通过；`companion-projections-final/*/generation.log`。未编辑派生代码或插件包。 |
| 完整 Contracts | `e2e6ca32` 普通及隔离执行 721 项通过，另有 2 个 Windows 不适用的 Unix link 用例跳过；`closure-final/5c4cfef780fe4107bf0822dbff8a8432`。本轮后续 SDK 输出修复另有 9 项真实 SDK 专项通过。 |
| 最终 Wiki / 架构 | 通过；`wiki-final-candidate`，241/241 工程实际归属、API 清单与可编译示例门禁。网络 NU1900 警告原样保留。 |
| 最终成对性能 | 三类负载各五组交替原版/当前进程、共 30 次，以及菜单/统计五组对照均完成；输入与依赖 12,778 文件、583,137,380 bytes 前后完全一致。详见下方性能结果。完整 E11 产品/交互矩阵仍未闭合。 |
| CI | `f7dce89c` 的完整 Windows、macOS ARM64、两端 Managed 和 NuGet 检查全部通过，完整 job 含 tests、Support Pack 和 Wiki；[workflow 40653896090](https://github.com/Wolfiora/InnoEngine/actions/runs/40653896090)。后续本轮提交尚需其自身 CI，不能借用旧 head 的绿灯。 |

Core 内部现在只在实际需要时创建回调、循环检测和恢复去重集合，复用 BCL reference equality；上下文仅在全部 owner 身份一致时复用。正式输出的同一属性 bytes 保持一致，分配测量为 fresh 1,888 / prepared 1,168 bytes，原 prepared 为 2,096。新公开边界回归先失败再通过；没有修改 `SerializationContext.With` 的新实例语义。该观察不是整帧或全产品 CPU 收益证明。

原生 command-key 模式会在 ImGui 内交换 Control / Super。`EditorUiInput` 现在还原物理 Core 修饰键含义，列表选择复用该边界；原生文本控件保留自身配置。四个真实输入组合验证两种 primary modifier 与两种原生模式。共享 Editor 不增加 OS 检测或平台引用，fixtures 显式选择键盘配置。


## 本轮收尾复验与深层自查

- `6df06585`：二进制编码先计算精确长度，直接写入最终 caller-owned buffer；wire bytes、字符串边界、特殊数值与错误语义由独立 BinaryWriter oracle 验证。1 MB 值的旧编码分配测试先失败，改良后通过。Converter lookup 继续属于同一个 generation，热命中使用 ConcurrentDictionary，首次构造由原 owner 锁保证唯一，最后一个 lease 才能退休类型和实例；没有全局 converter cache。
- `e2e6ca32`：Shader settings 六处行 ID 和一处 capability tree 采用局部 finally；合并 30 个重复 alias、176 个语义符号引用，删除 3 段重复 XML summary。AST 防线仅覆盖明确模式，不伪称证明任意动态 scope。相关 source-contract 16 项通过。
- `closure-final`：Contracts 721、Workflows 305、Visual 136、Lifecycle 11 项通过；各 profile 有交叠。Core/Scene 的后续专项为 59/53 项，不与 profile 计数相加。
- 新增的真实 SDK 输出回归先以两项 ArtifactsPath 失败复现，普通 Debug/Release 两项通过；修复后初始四项全部通过。扩展后的 `e40/output-entry-final.trx` 共 9 项通过，覆盖 Debug/Release、普通/独立输出、真实 RID、工具中立、工具/产品/fingerprint 输入、等价 props 路径、IDE execution 属性、并行编译、编译器工具引用和入口 owned restore。
- 全双入口构建暴露了两个关联问题：中立/产品实例争写 DLL，以及入口直接引用先于产品还原。前者使用 SDK ArtifactsProjectName 的早期选择，后者复用 RestoreOwnedProjectTask 和 Core IO lease；没有串行构建、手拷 DLL、借用默认 obj、删除 RID 或增加平台名单。`e40/two-entry-restore-fixed.log/binlog` 已通过，0 编译错误，NU1900 原样保留。规范化路径修正后的全新输出复验与最终 CI 另行记录。
- 实际 FlappyBird D3D12 运行 15,000 帧，9 个实际 Panel 各关闭/打开 100 次，完整退出与 sRGB 检查通过。两份完成 TSV 必须存在，不能只依赖进程退出。`products/closure-panel-lifecycle-resources.tsv` 在 0/10/.../100 次 closed-state 进行 Full GC → finalizers → Full GC：Selection 始终 0、Scene 1、History resident/disk 0、Core pending events 0；managed 从 26,168,896 到 26,179,976 bytes，private 从 880,267,264 降至 864,219,136 bytes。该结果是公开状态和内存观察，不证明每一个私有 native owner。
- 最初短 smoke 的期望字符串写错，未产生 fixture 完成标记，因此没有计入通过。增强 fixture 通过公开 Workspace API 打开实际 FlappyBird 源场景，不让引擎为验收自动创建 Untitled。测试辅助方法曾有一次编译错误，已修正，后续构建 0 错误、专项 9 项通过；失败日志保留。
- 并发序列化对照 `e46/parallel/summary.json`：同输入、五组交替进程，8 workers / 80,000 次 decode 的中位 wall 55.46 → 42.63 ms，CPU 375 → 265.625 ms，分配基本相同；1 worker 的 10,000 次为 25.11 → 23.36 ms。Monitor contention 计数不代表所有锁种类，因此收益以独立 wall/CPU 结果说明。此负载不等同完整 UI 帧性能，完整 Scene dirty-scan 的 #46 仍 Open。

自查已深入执行链：平台策略仍由平台贡献；入口只声明选中的产品；Restore 与 compilation 保留隔离；共享层使用 SDK 求值路径；History、Identity、Core Events、Serialization 与 IO 没有平行实现；新的 converter 缓存仍由 generation/lease 拥有。BGCS、NuGet 包、第三方及生成绑定未修改。没有因本轮通过项而宣布 E08–E12 或整个引擎已经完美；下方未完成矩阵仍然有效。
## 最终受控性能结果

源码候选为 `f7dce89c0d91ecb0f2333b8ca3117fefc71db96a`；原生产基线为 `b303d9f10cbaf5a905f7cb529e52cf8293b0d8bd`，原版只移植验收 harness，不修改生产实现。两侧使用同一 .NET 10.0.402、Debug 配置、机器、冻结数据和 Native command-validation 方法；各进程预热五帧，再采样十帧，五组进程交替顺序运行，没有构建/测试/产品并发。原始各组数据保留在 [性能证据目录](editor-ui-performance-2026-10-28/README.md)。

| 负载 | 原版 median / P95 | 当前 median / P95 | 原版 → 当前分配/帧 |
| --- | --- | --- | --- |
| 10,000 对象层级 | 5117.21 / 5594.09 ms | 345.14 / 402.69 ms | 709.02 → 139.06 MB |
| 10,000 资源目录 | 4104.39 / 4320.28 ms | 18.73 / 27.58 ms | 249.23 → 0.98 MB |
| 2,000 Shader 节点 | 29.01 / 33.17 ms | 28.78 / 33.39 ms | 4.36 → 3.63 MB |

这里 MB 使用十进制字节。图负载的 P95 为 +0.65%，CPU 中位数相同，前次超过 10% 的回退已消除；三类负载均通过本次相对回退与分配门槛。菜单打开的每次分配 8544.29 → 4240.14 bytes，时间 10.61 → 6.18 µs；无变化统计读取 16792 → 400 bytes，时间 30.40 → 0.24 µs。后两项是公开菜单/统计契约负载，不能等同完整产品帧收益。

**绝对表现仍有严重限制：345 ms/帧、139 MB/帧的大层级负载不能称为优秀。** 当前 `IsDirty` 仍周期性序列化完整 Scene 再散列，检查超过既有 100 ms 周期时会持续占用后续帧。[#46](https://github.com/Wolfiora/InnoEngine/issues/46) 已记录真实调用链、职责、设计约束和验收要求；没有用延长周期、隐藏检查或不完整 mutation cache 掩盖。#45 的 Core 改良不代表 #46 或完整 E07/E11 已完成。

本次是 Native command-validation / headless public-contract 比较，不是 GPU 或完整 FlappyBird 产品帧性能认证。原版与当前 FlappyBird API 输入不相容，以及滚动、搜索、属性拖动、冷启动等完整性能矩阵仍按对应 issue 保持未闭合。

证据根目录：`D:/InnoEngine-Verification/editor-ui-closure-2026-10-27`。表中的路径均相对此目录。日志、TRX、binlog、截图和大规模输入不进入源码仓库；本记录与执行台账提供可追溯索引。

## 实现与职责自查

| 位置 | 实际改动与边界 |
| --- | --- |
| Interactions | 静态菜单及快捷键结构按 generation 建索引，动态可用性与请求数据继续即时解析；不建立全局缓存或第二套 Action 系统。 |
| 通用 Graph / Graph.ImGui | 文档与 History 保持中立；节点、端口几何、命中与 pointer intent 可供非 Shader 图使用。Shader controller 保留语义解释和领域修改。 |
| ImGui presentation | Session 拥有 transient state；原生帧准备与绘制分离，同一帧不能提交新的 Zoom/Density。真实 callback 失败结束所属帧并保留原异常。 |
| Domain presentation | Browser、Console、Hierarchy、Settings、Inspector Tag/Layer 的专属值归对应 owner；通用 palette/metrics 只保存基础与复用组件值，删除旧成员后直接更新调用方。 |
| 大视图 | Hierarchy、Browser 使用 revision/展开/过滤决定的中立可见序列与原生裁剪；Console 使用有界槽位与测量缓存；Graph 局部更新几何。 |
| Statistics | 无变化时复用不可变快照，Panel 有界保存 0.5 秒间隔、60 秒窗口、最多 120 个 timing 样本。 |
| Scripting Compiler | 只消费中立冻结输入，不引用 Build、MSBuild 或命名平台；实际 SDK 求值由 Host/Build provider 提供，取消与失败不发布半成品。 |
| Tooling / Build | 读取 SDK 选定的 assets、references、compile items、defines 与输出 owner，不用过期默认 bin/obj 代替当前请求。 |
| AssetFileSystem | 在既有 Core Identity/store 内准备完整候选，复用未变化 entry，失败保留原视图，只退休移除内容；没有 UI fallback 或平行 live-object authority。 |

已经沿实际消费链复核：Feature 不依赖 Native presentation，Graph 不依赖 Shader 世界观，平台事实没有移入通用执行器，History 继续使用既有中立 payload 协议。跨帧选择和延迟目标使用 Core Identity／弱引用；同步 `EditorInteraction` 对目标的强借用在 XML 和规范中明确，不能误当成长生命周期缓存。

新增公开表面仅用于实际消费者：通用 Graph 的呈现输入及交互结果、SDK 冻结脚本输入、真实 Host 调用的帧准备协议和 hierarchy revision。内部 Console 过滤、样式值与呈现缓存没有因测试而公开。

没有增加 Events、Identity、History、Serialization、插件发现或缓存管理的平行实现。BGCS 源码、NuGet 包、第三方源码和生成绑定不属于本轮修改；负向程序集内容仅在测试自有副本中构造。没有加入 global/implicit using、IVT、私有反射穿透、兼容 reader 或旧 API 转发。

## 已执行证据

以下列出当前候选对应的专项结果；它们存在交叠，**不能相加成互不重复的总测试数**。

| 验证 | 实际结果 | 证据 |
| --- | --- | --- |
| 完整 SourceDebug 源码编译 | 通过 | `source-frame-final.log`、对应 binlog |
| Windows Editor Debug 构建 | 通过，0 errors | `editor-frame-final.log`、对应 binlog |
| Native ImGui context/frame/metrics/controls | 72 通过 | `e03/all-native-frame-contracts.trx` |
| 统一 Workflows profile | 297 通过 | `unified-final/2a003d59a27a4c22b65ff5ac2b285170/summary.json` 及 TRX |
| 统一 Visual profile | 135 通过 | `unified-final/*/summary.json` 中 Visual 记录及 TRX |
| 统一 Lifecycle profile | 5 个循环用例通过 | `unified-final/*/summary.json` 中 Lifecycle 记录及 TRX |
| 实际 package / loaded assembly 身份 | 普通 3 通过；独立进程 1 通过 | `e16/loaded-package-shared.trx` 与 isolated TRX |
| SDK / C# 14 / IDE 冻结输入 | 11 通过 | `e19/sdk-contract-final.trx` |
| SDK-owned tooling owner | 完整 153 项通过，包含实际 SDK／当前输出 owner 的仓库级检查 | `corrections/Inno.Tooling.Architecture.Tests/formal-complete.trx` |
| Asset pipeline / source identity / writable content | 完整 123 项通过，包括保留时间戳的源与依赖变化、被锁定源的 last-good 诊断 | `corrections/Inno.Assets.Pipeline.Tests/complete.trx` |
| Console Clear / 真正 eviction | 2 通过 | `e07/console-retired-real-before.trx` |
| 实际九类 Panel 100 次循环 | 执行完成；不替代完整资源计数矩阵 | `e10/panel-lifecycle-final.tsv`、产品日志 |

真实 package/load 负例保持程序集名称及 MVID 相同，仅改变测试副本内容，实际加载后由内容身份拒绝；随后执行 Full GC → finalizers → Full GC 并确认 collectible ALC 不可达。没有通过删除默认 obj 或清理全局 NuGet 使测试变绿。

C# 14 用例覆盖 `field`、空条件赋值、Span 重载、泛型 `nameof`、lambda 修饰符、扩展成员与跨插件 IDE 引用。正式输入一致性已验证。额外公开 SDK 探针验证了两个真实 CLI 根、显式 10.0.402 选择、干扰 pack 目录和不兼容宿主，详见下文；其后完成真实 10.0.401／10.0.402 的不同 patch 验证，详见后续复验。

### 真实 FlappyBird 操作

独立副本 `products/FlappyBird-functional-final` 使用当前正式插件包和 D3D12 Editor：

1. 正常打开 FlappyBird Scene，Scene/Game、Hierarchy 与 Browser 内容正常。
2. 使用公开快捷键／可见 toolbar 进入 Play，并返回 Edit；观察实际 action tooltip 和场景恢复。
3. 返回 Edit 后 Browser 目录树及 `~Authoring` 文件列表保持可见；之前 #43 的空列表问题未再出现。
4. 从 Edit 菜单打开 Settings，修改 Density，确认 Apply/Cancel 状态；Cancel 后重新打开仍是保存的 Comfortable。
5. 正常关闭窗口，进程 exit code 0，日志明确记录 product resources released 和 Dispose end。

证据：`products/editor-functional-final.log`、`products/FlappyBird-functional-final/Logs/EditorBoot.log`、`e09/flappybird-final-play-stop-browser.png`、`e09/flappybird-final-settings-cancel-reopen.png`。

此前同一实现阶段的真实 Inspector 单轴编辑、Undo、Redo 及回到原值保留于 `e09/flappybird-inspector-*.png`。它们是实际功能证据，最终版本完整多功能／失败／取消矩阵仍须独立说明。

### Release 工具图回归

最终 Release 冷构建发现 `BuildingInsideVisualStudio=false` 与未声明该属性造成两个 generator build identity；binlog 显示两个 Csc 同时写同一输出。SDK 提到可能的防护进程不是本报告确认的根因。已在原有 SDK-negotiated compiler-tool boundary 规范化该宿主事实，没有停止用户进程、关闭安全设置或串行化整个构建。

扩展的真实 SDK fixture 在修复前 Debug/Release 均失败，修复后两项通过；每项包含冷构建、未变化热构建和修改 Tool 源码后的重建。证据为 `editor-release-final.binlog`、`e40/release-graph-failure.json`、`e40/ide-graph-before-selected-host.trx`、`e40/ide-graph-after.trx`。随后产品 Release 又复现普通 Catalogs 输出冲突；此失败不能算通过。进一步定位到 Foundation 引导剥除调用方属性却写同一源码输出。`8dcceb94` 把中立引导放到独立且固定的 SDK scope，保留发布参数隔离；27 个完整引导回归通过，包括 Debug/Release 并行图、热构建和 Browser/AOT/RID 请求。证据 `e40/stable-bootstrap.trx`。最终产品重跑另列。第一次直接 VSTest 没有传入选定 DOTNET_HOST_PATH，使用了机器另一个 muxer；该运行不作为图缺陷证据。

### 证据纠正与无效运行

- 第一轮完整 Contracts 操作缺少其自有 Native deployment，之后又发生与仍运行 testhost 重叠的测试输出构建。该操作已经中断，`contracts-interrupted.json` 明确标为无效，不能计作全部通过。
- 统一 builder 已传递正式 `InnoTestNativeTarget` 和 `InnoTestNativeProduct`。未放宽文件锁协议、生产校验或超时。
- Console eviction 初始用例没有达到实际最小容量 16，不能证明 eviction 缺陷。已纠正前提；真实 Clear/evict 测试通过，推测性的 production focus 改动撤回。
- 诊断 dump 在保留文本栈和原因后，仅从本轮明确 owner 删除；未清理用户文件或 SDK/NuGet 全局缓存。

### 后续完整性与编排修复

- [#44](https://github.com/Wolfiora/InnoEngine/issues/44)：Windows CI 暴露同长度快速改写返回旧内容，保留时间戳的用例确定性复现。可写源及 Source 依赖现验证实际 SHA-256；采用标准库流式读取，保持既有失败诊断／last-good 恢复，不修改 ID、sidecar、BGCS 或只读安装 generation 协议。123 项完整 Asset Pipeline 测试通过。增加必要内容读取的成本单独报告，不能称为免费优化。
- #41：图检查原本过早 ResolveReferences，给纯 graph fixture 创建输出并对无 Foundation 的 fixture 抛错；改为按语义用途惰性获取框架引用。完整仓库测试改用 SDK 及编译时记录的实际 ArtifactsPath。轻量 fixture 仍为两分钟；新增的完整 SDK 检查独立设置十五分钟预算，实际完整套件为 6 分 13 秒。SDK 单请求与 ALC unload barrier 的原超时没有放宽。
- 测试临时目录改为显式 `-TemporaryDirectory`；默认保留 OS 环境。本轮短 D: owner 下原失败的 9 个 SDK/CMake 用例通过。深结果目录和源码调整期间中断的旧运行均不计入通过。

## 当前关键文件树

```text
build/msbuild/Inno.Build.Tasks.targets       # 中立 SDK Foundation scope
build/pipeline/Inno.Build/Scripting/
  SdkScriptCompilationEnvironment.cs         # SDK -> 中立冻结输入
src/runtime/scripting/Inno.Scripting.Compiler/
  IScriptCompilationEnvironmentProvider.cs
  ScriptCompilationInputs.cs
src/content/assets/Inno.Assets.Pipeline/
  Sources/AssetFileSystem.cs                 # 候选索引与稳定身份
  Importing/AssetLoader.SourceIO.cs          # 内容验证与稳定读取
  Importing/AssetLoader.Dependencies.cs
src/composition/editor/
  framework/Inno.Editor.Interactions/        # Action/Menu/Selection/History
  presentation/Inno.Editor.Graph.ImGui/Canvas/
  presentation/Inno.Editor.ImGui/Runtime/EditorUiSession.cs
  presentation/Inno.Editor.ImGui/Styling/
  presentation/Inno.Editor.ImGui/Widgets/
  presentation/Inno.Editor.Assets.ImGui/Styling/
  panels/Inno.Editor.Panel.Logging/Console/
  panels/Inno.Editor.Panel.Logging/Styling/
  panels/Inno.Editor.Panel.Hierarchy/Presentation/
  panels/Inno.Editor.Panel.Hierarchy/Styling/
  panels/Inno.Editor.Panel.Inspector/Styling/
  panels/Inno.Editor.Panel.Settings/Styling/
  panels/Inno.Editor.Panel.Stats/
tools/Inno.Tooling.Architecture/
  DocumentationSourceModels.cs
  WikiProjectReferences.cs
tests/Invoke-EditorUiAcceptance.ps1
tests/Invoke-IsolatedTests.ps1
tests/fixtures/editor-ui/PanelLifecycle.editor.cs
```

该树列出职责 owner 与关键改动；其余已有功能位置保持，完整文件差异以对应提交为准。没有为目录对称新建空项目。

## 必须保留的阻塞与未完成项

| 项目 | 当前事实及重新验收条件 |
| --- | --- |
| E11 原版 FlappyBird 同输入比较 | 原基线 `b303d9f1` 的正式工具构建成功，但当前插件源码依赖原版不存在的 `EditorUiSession`，正式脚本编译失败。日志 `e11/original-flappy-projection.log`。不能替换输入或加兼容层后称相同输入。需要可在双方正式接口上运行的、经认可的共同产品输入／基线定义。 |
| 原始完整计划文本 | 桌面文件现为闭合计划，hash `cbb7d50a…`；旧原始 UI 文档只有复审记录的 hash，不能把二者说成同一文件。现按获批闭合计划和复审矩阵执行。 |
| 完整工作流 | 已执行多条真实旅程和 handler/contracts，但每类界面正常、失败、取消、关闭重开四条路径尚不能全部逐格记为通过；Settings 跨 owner 部分失败已经真实复现并修复，四项 Native 回归通过；其余完整旅程仍需逐项闭合。 |
| 完整视觉矩阵 | 真实 Native simulated metrics 与部分产品截图已执行；匹配原版／当前的完整截图、所有浮动停靠与物理显示器组合没有全部实测。 |
| 完整生命周期收敛 | 100 次 Panel、Scene/Shader document 和 reload 用例不等于每个 owner 的全部订阅、手势、引用计数已证明收敛。不能用单次 Host 正常退出替代。 |
| #19 多 SDK 环境 | 已验证真实官方 10.0.401／10.0.402、167 个实际 references、Runtime／IDE 一致性及 SDK 切换缓存身份；Mac 实机未执行。 |
| 最终完整 Contracts 与 CI | 分别记录最终运行结果。旧 head 的绿灯、已中断运行或单个测试不替代同候选完整结果。 |
| macOS 人工设备 | 本机无 Mac；远程 ARM64 CI 仅证明其实际自动步骤，不能证明人工 Metal、音频、显示器和操作体验。 |

因此总 issue #27、E08–E12 等不得被无条件标记完成。没有验证的项继续保持 Open；阻塞没有被改写成缺陷已解决，也没有归责 BGCS。

## 十一项质量评价

判断针对可复查工程标准，不提供虚构的行业认证或“完美”分数。

| 维度 | 判断与证据 | 仍需改进／证明 |
| --- | --- | --- |
| 架构 | 分层已有实质改善：领域、交互、呈现、Host/SDK 有真实消费者和依赖边界。 | 完整最终门禁及所有生命周期 owner 证明完成前，不宣布整个 Editor 达到全部原标准。 |
| 代码整洁度 | 文件按功能 owner 收拢，删除旧转发，API/引用/Wiki 同步。 | 持续审查复杂 Panel 的职责规模；不把更多拆文件当作质量本身。 |
| 代码干净程度 | 修复实际 source identity、frame failure、同帧尺度问题，未用 fallback 掩盖。 | 完整异常、取消、资源收敛矩阵仍需补齐。 |
| 抽象程度 | Graph 有非 Shader 消费者；SDK 输入中立；帧准备有真实 Host 调用。 | 第二种完整图产品旅程与所有扩展失败路径尚不等于全部验证。 |
| 可复用程度 | 复用 Core Identity/Events、既有 History/Serialization 和 Session controls。 | 继续以真实调用方证明复用，不建立通用却无消费者的设施。 |
| 样式统一 | 基础/组件/领域三层归属落实；同帧尺度契约已测。 | 完整匹配截图、窄布局与物理 DPI 体验仍有缺口。 |
| 可拓展程度 | generation-owned 索引、最小 Graph contract、SDK provider，无中心 Shader 白名单。 | 完整 reload/ALC 和第三方实际扩展旅程需要持续验证。 |
| 操作逻辑性 | 键盘、Console 搜索、真实 Play/stop、Settings Cancel 已测。 | 全界面的失败、取消、关闭重开逐格证据尚不完整。 |
| Undo/Redo | 继续单一 History，真实 Inspector edit/undo/redo 与原子 handler 回归有证据。 | 各结构化批量操作、Settings 多 owner 失败、重试、单次 Undo/Redo 已补齐；其余结构化批量修改及 Missing 恢复旅程仍需完整矩阵。 |
| 菜单清晰度 | 静态结构复用、动态状态即时，真实 Edit 菜单 disabled Undo/Redo 正确。 | 全部 area/动态贡献、冲突和可发现性的最终旅程与性能需闭合。 |
| 组件颗粒度 | 通用与领域 owner 更明确，内部缓存/样式未扩大 public 表面。 | 保持公开边界最小且完整；复杂 controller/session 的长期规模仍需定期复核。 |

总体判断：有实质性工程进展，仍未达到原计划的全部完成条件。不能将局部强项概括为所有十一项已经“超级无敌优秀”。

## 资源、提交与后续结果

大型输入、baseline、binlog、TRX 与截图集中在 D 盘。保留现行有效产物和活动 reader，维护通过既有 owner/lease 协议进行，不手删协调锁或全局依赖缓存。完成时核对实际磁盘空间、companion/BGCS 状态、最终提交和 CI，并在此追加精确结果。

最终采样后通过正式 cache clean 先 dry-run 再执行：逻辑 inventory 10,787,641,712 bytes，保护 174,384,757，退休 10,613,256,955，skipped 0。C 盘实际可用从 92.090 增至 93.501 GiB，D 盘约 97.782 GiB；hardlink 的逻辑回收量不能当作实际卷空间。日志为 `cache-after-final-preview.log` 与 `cache-after-final-maintenance.log`。没有清理 SDK/NuGet 全局缓存或用户内容。

本环境没有 `/System/Library/Sounds/Glass.aiff`，不能播放指定提示音。

## Settings 与测试输入 owner 的专项复验

### 实际缺陷、失败证据与修复

Release Editor 在独立样例副本中复现：Editor Density 已写入后，Build settings 的独占文件锁使 Apply 失败；原 Settings Modal 随后被隔离，留下部分成功的修改。证据为 `products/settings-partial-apply-before.log`、`e08-settings-partial-apply-before.json`、`e09/settings-partial-apply-before.jpg`。这是现行实现的真实失败，不是 BGCS 缺陷。

修复沿原有 `EditorInteractions.history` transaction 处理 Editor、Project、Build 的一次 Apply；成功后才接受 draft baseline。失败保留 staged values，局部显示状态并保留完整 Console 诊断。回滚失去完整性时保持 History Faulted，不能继续 Apply。Settings 窗口拥有当前 generation 的借用与原有 reload guard；异常 Drawer 经标准隔离流程退休 transient owner，重建 catalog 后再允许打开。没有增加平行 History、Recovery、Events 或 Identity。

`e08-settings/green-official.trx` 四项通过：

1. Editor/Project 中途失败后全部恢复，重试成功，形成一次 Undo/Redo。
2. 回滚本身失败后保持 Faulted，修复磁盘前提也不会偷偷恢复 Apply。
3. 100 次打开/关闭后，公开 Drawer 接收的值可被收集，reload admission 恢复。
4. Drawer 抛异常后释放 reload guard，菜单禁用直到真实 catalog rebuild，再正常打开与关闭。

第 4 项在修复前实际失败（`red-official.trx`）。第 3 项在旧实现也通过：它观察的是 Drawer 获得的副本，**不能证明旧 Session 泄漏，也不证明全部私有 Session 或 ALC 已回收**。移除 Modal 对旧 session 的不必要强引用是代码所有权修正，不能伪称已经复现了该泄漏。各运行使用 SDK 选定的 Editor product props；早期错误选择普通 ProductBuild.props 的诊断运行单列，不作为最终产品构建。

### 完整 Contracts 失败与实际 owner

`unified-stable/554c93c1e0234bb880a7ae94623fbf2f` 是一次完成的失败运行，不能改写成通过：Build.Tests 379 通过、2 失败、2 个平台条件跳过；其他六个普通/隔离文件分别为 26、2、130、153、13、1、1 通过。各 profile 有重叠，不累计成唯一测试总数。

两个失败分别为 Wiki 漏传独立 ArtifactsPath，以及 Task fixture 猜测已经退出的默认 Foundation 输出。Wiki 原先已经传了 SDK 入口；问题是输出 owner，不能说成它完全没有 SDK 输入。Task fixture 改为读取实际 MSBuild TargetOutputs，保留文件句柄释放、两次私有 publisher 身份和零 Copy 重放断言。

测试 source 项目新增 `TestRepository.GetArtifactsPath(Assembly)` 只读取 SDK 生成的公开 assembly metadata。统一 targets 在 SDK 算出输出之后记录该事实；没有引擎内部访问后门、生产 API 扩大或第二个路径 resolver。六项真实 SDK/metadata 用例通过，包括默认/显式 owner 和损坏 metadata 拒绝。目标注册测试正式复验通过；同轮 Wiki 产生新的 241 工程库存，并指出新增 Dispose 漏 XML 一项，现已补齐。首次直接 VSTest 漏了官方入口的 PATH 环境而用了 .NET 9 muxer，该运行保留但不归责实现。最终完整 Wiki 复验另列。

### 多 root 与 pack 干扰

`e19-root-probe/result.json` 使用生产公开 SDK capture：已安装根与测试自有 D 盘 CLI 根都明确选择 10.0.402，167 份 framework references 内容一致；D 根只消费自己的 pack 路径，`10.0.999` 干扰目录不被选择。不兼容 .NET 9-only 宿主明确失败，没有 fallback。临时求值工程正常退休。

D 根的 host/sdk/shared 使用测试自有 junction 指向现有安装；干扰 pack 是测试副本，不是另一个实际 SDK 安装。没有修改安装 SDK、NuGet 或消费者 global.json。因此这是根选择和干扰拒绝证据，不冒充多个不同 SDK patch 或 Mac 实机矩阵。

## 后续实际 SDK 与产品复验

以下结果对应 660962a5 及此前实现；后续 Core／Graph 改动需要自己的最终验证。

- SDK 10.0.401 选择 reference pack 10.0.12；10.0.402 选择 10.0.13。401 根下存在真实 13 pack 的受控 junction，也继续选择 12。相同 C# 14 源码的 Runtime 编译与实际生成 IDE 工程均通过；167 个 reference 路径及内容在各自操作中一致。官方 401 安装包经 SHA-512 验证，只安装在本次 D 盘 owner。
- 同一测试 Project 执行 401 → 402 → 402 → 401：新编译、新编译、缓存复用、缓存复用。不同 SDK 使用不同产物。证据 e19-dual-sdk/verified-current-result.json、switch-0.json 至 switch-3.json。引擎副本与当前正式 producer 内容一致。
- Canvas、Rendering2D、实际 Samples/FlappyBird 正式完成脚本及 IDE 投影；companion 仓库 Git clean。证据 companion-projections/<name>/generation.log。
- Editor Debug/Release 和 Build 入口构建 0 errors；editor-Debug-settings-final.log、editor-Release-settings-final.log、build-entry-settings-final.log。
- 七个 UI 工程正式图编译 0 warnings/errors，final-ui-suites-build-corrected.log。660962a5 修复统一入口遗漏 SDK 实际 ToolTarget 的问题，没有新增平台推断。
- Workflows 301、Visual 135、Lifecycle 6 个用例通过，unified-settings-final/ 的 summary/TRX；完整 Wiki 与 Task registration 两项通过，final-owner/final-owner.trx。各项结果存在交叠。
- Debug/Release × D3D11/D3D12/Vulkan 六个 FlappyBird smoke，各 300 帧且正常退出，products/settings-final-<configuration>-<api>.log。只证明有界启动、渲染和资源退休。
- 最终真实 Settings：Build 文件独占时 Apply 失败，Editor 设置文件 SHA-256 保持基线；释放故障后重试成功，一次 Undo 恢复两份基线，一次 Redo 恢复两份成功文件。证据 e08-settings/final-failure-atomicity.json、after-single-undo.json、after-single-redo.json 及 e09/settings-atomic-after-*.jpg。随后 Play/stop 和正常退出完成。

## 性能复审与修复追踪

首次有效比较 e11/pairs-57543dce399247dfa6551b6351621c39：三个负载各五组交替原版／当前进程，共 30 个成功进程。相同冻结输入、SDK、配置及原生命令校验，不代表真实 GPU 或整个产品性能。

| 负载 | 原版 median / P95 ms | 修复前当前 median / P95 ms | 修复前当前 bytes/frame |
| --- | --- | --- | --- |
| 10,000 Hierarchy 对象 | 4703.73 / 4959.41 | 470.16 / 511.64 | 655,611,454 |
| 10,000 Assets 目录 | 3678.02 / 4049.01 | 16.29 / 24.56 | 998,359 |
| 2,000 Shader 节点 | 25.00 / 27.55 | 30.21 / 36.83 | 4,171,160 |

Graph 回退不满足要求；Hierarchy 即使更快，绝对分配仍过高。输入及 2,150 个依赖文件采样前后保持一致，e11/before-correction-integrity.json。更早首轮有准备重叠风险，整轮无效，e11/invalid-first-release.json；未挑选较好数据。

真实 [#45](https://github.com/Wolfiora/InnoEngine/issues/45) 跟踪 Core 修复：小属性编码不再预分配 16 KiB；单属性捕获不执行无关 getter。沿既有 Serialization pipeline 实现，没有新 serializer、池、缓存或 schema。修复前 4 项失败，修复后 Core 全部 46 项通过，e45/red.trx、e45/complete.trx。输出 bytes、大小增长、异常及属性恢复有公开契约覆盖。

受控副本只替换该 Core producer 的诊断探针，Hierarchy 分配约 655.6 MB → 171.8 MB/frame。该次与 trace 重叠，耗时不能作最终结果；剩余 full Scene dirty 检查成本如实保留，不延长间隔或使用不完整 revision 隐藏直接属性修改。

Graph 修复归 [#34](https://github.com/Wolfiora/InnoEngine/issues/34)：复用当前窗口绘制 borrow、已测标签宽度及帧 mouse；每次真实绘制保留窗口、帧、线程及 Context 校验。同一修复候选仍需正式构建、回归与五组性能复验。

## 浮点边界与测试产品归属复查

- 第一轮 Graph 借用／测量修复后的正式七工程图编译成功，6 项 Graph/Settings Native 回归通过；e45/graph-settings.trx。单次诊断为 median 29.55 ms、P95 30.96 ms，尚不足以通过相对原版的性能门槛。
- 随后的十二工程图编译成功，final-correction-suites.log/binlog；但颜色原生 oracle 在 NaN 上失败：原生结果 2147483648，普通托管转换结果 0。e45/color-oracle.trx。该操作在此停止，未运行后面的 Core/Scene 或统一 profiles，不能记为整体验收通过。
- 当前修复使用标准库 Single.ConvertToIntegerNative，保留原有 Session／Context／线程准入、饱和与取整协议。原生 oracle 覆盖通道顺序、上下溢出、非有限值、全部半整数边界和 4,096 个确定性输入。既有 Core Color 的 byte 转换使用截断语义，不能直接替换原生 UI 的舍入协议；没有为此增加另一套通用数学系统。
- 统一测试脚本不再全局设置 InnoTestNativeProduct；它覆盖了各工程的声明，也使带有传递 native 引用的纯契约套件反复准备完整 Editor。实际 Native UI 工程保留自己的产品声明，Build.Tests 根据此前真实脚本引用验收所需依赖显式声明。目标、工具和产品参数仍来自正式 SDK 求值；生产准备协议与校验没有跳过。
- 原生准备日志中的 processes=0、bindingGenerations=0 证明没有实际 Native 重编译，却不证明输入验证便宜。重复读取约 4.5 GB 的既有构建成本仍保持独立问题，不以减少生产完整性检查来使本次 UI 验收变快。
- SDK API 清单准备完成 241/241 工程求值；该次未提供当前逻辑脚本 references，且文档当时尚未同步，exit 1 是保留的准备失败，不是最终 Wiki 通过。另有环境 NU1900 审计网络告警；没有关闭审计或忽略告警。
颜色数值 oracle 在 ConvertToIntegerNative 修复后全部通过。额外断言错误地把 Render 后仍存活的 Context 当成无效 owner；该纯值操作原有准入只验证 Session、Context 和线程。已纠正为 owner 释放后的拒绝断言，没有修改生产准入。DrawList 的窗口／帧校验仍由 RequireDrawingFrame 承担。

## 最新统一功能门禁

同一生产实现候选完成：Core 序列化 46、Scene 53、Workflows 302、Visual 136、Lifecycle 11 项通过。它们存在重叠，不能相加成独立总数。完整 Contracts、最终产品构建及受控性能仍单独报告，不能由这些结果推断全部通过。

真实 UI 扩展的进程隔离用例保留第一代 immutable menu model，连续十轮先编译失败再成功替换，核对 last-good、当前标签和 Action ID；在下一轮前通过现有严格 barrier 确认旧 collectible Action Type 不可达。普通套件还验证了十轮运行脚本成功／失败、十轮删除／恢复、十轮破损插件更新／恢复及十轮 Play/stop/reload。没有缩短 timeout、清除 pending monitor 或放宽回收检查。证据 e10/final-reload-cases.json 和 unified-correction-final 对应 TRX。

原生测试产品归属经真实 SDK 求值确认：12 个选定工程中 4 个明确声明产品，其余没有被脚本强制声明。native-test-ownership.json 保存实际结果。该改动消除不必要的准备请求，不用不同输入的一次总构建计时宣称性能收益。
