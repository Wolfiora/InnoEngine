# Editor UI 原计划逐项复审 — 2026-10-27

[当前验收](../../../docs/architecture/CURRENT_ACCEPTANCE.md) · [当前问题](../../../docs/architecture/CURRENT_ISSUES.md) · [实现记录](../EDITOR_UI_IMPLEMENTATION_PLAN.md) · [前次验收](EDITOR_UI_ACCEPTANCE_2026_10_27.md) · [PR #26](https://github.com/Wolfiora/InnoEngine/pull/26)

## 结论与审查范围

**尚未完整达到原计划的完成标准。** 当前已经形成较好的工程基础，尤其是领域与呈现分层、Session 所有权、Core Identity 路由、统一 History 和回归测试。但仍有明确的功能、抽象和性能实现缺口，也缺少原计划要求的完整规模、生命周期和视觉验收。不能把这些统一解释成“只剩人工看看”，也不能把 CI 全绿解释成整个计划完成。

前次报告中的 “The implementation and local correctness checks are complete” 范围过大，本次予以纠正。已有通过结果继续有效；改变的是对它们能够证明什么的判断。本次没有发现足以否定全部重构的证据，也没有据此认定存在新的 BGCS 缺陷。

- 审查源码：`2dc37114b7c9b5137e866efc057a66e0dfd72046`，分支 `editor-ui`。
- 远端 master：`cf799c8280f74f6faa19916a89f216778391fcd9`；PR #26 已存在，base 为 master，状态为 Draft。
- 原始计划：用户提供的 `InnoEngine_Editor_UI_Plan.md`；SHA-256 为 `a55f25b76db09befd6cdd2db06a607d0e2a70d660dca6ca5429c64f3d5623646`。
- Editor 范围：30 个项目、507 个 C# 源文件；PR 相对 master 共 373 个变更文件。此次沿关键路径深查，并不等于对每个分支作形式化正确性证明。
- 方法：对照原计划条目、项目依赖、执行链和测试源码，重新解析最终 Windows/macOS CI 的 TRX，并重新构建运行 Interactions 测试。没有使用 computer use，没有重新执行全产品矩阵。
- 本次修改范围是审查记录、当前文档与规范；没有修改生产代码、测试实现、BGCS、平台或生成绑定。

## 十一个维度的逐项评价

| 维度 | 已经成立的工程性质 | 尚未达到的要求 / 改进点 | 判断 |
| --- | --- | --- | --- |
| 架构 | Features 不依赖原生 UI 或 Panels；Panels 不互相引用；Assets 操作、Export 生命周期与绘制分开；相关边界有架构测试 | Graph 通用呈现只抽出了坐标、网格、曲线和连线命中，节点呈现、节点命中、框选等仍在 Shader Canvas；不能仅凭新增程序集判定抽象完成 | 主干达标，原计划未全闭合 |
| 代码整洁度 | 功能目录与 owner 更明确，公开依赖分组、Solution、脚本导出及调用方同步 | Shader Canvas 主文件约 1,110 行，多个 partial 共享 draft/controller 状态；部分源码同时使用多个指向同一 ImGui facade 的别名，阅读仍有噪声 | 明显改善，尚非全面清晰 |
| 代码干净程度 | Session/owner 退休、native borrow 校验、弱目标、Core Events 订阅所有权均有实际实现；没有靠全局 UI 状态表或额外 Undo 栈解决问题 | 全量异常退出与长期资源回收证据不足；Console、Shader 绘制局部仍有手动 Push/Pop 缺少局部 finally 的路径，需要逐个证明异常安全 | 核心机制较强，不能认证无泄漏/无残留 |
| 抽象程度 | Action、History、Inspection、Graph 文档和领域 Feature 边界实际有消费者；没有单靠空接口实现“解耦” | Graph presentation 抽取不足；通用样式中仍保留具体 Browser 布局策略；应沿真实复用点提取，避免再造万能 UI 框架 | 部分达标 |
| 可复用程度 | Core Events、Identity、Serialization、IO、History 和 Graph 被复用；资产操作不再只能经 File Browser 使用；脚本 SDK 直接引用真实 API owner | 通用节点呈现/命中/框选尚不能独立复用于非 Shader 图；需要第二种真实图呈现验证抽象边界 | 主要基础能力达标，图呈现仍有限 |
| 样式统一 | Session 统一 metrics/palette，表单、树、菜单和有界下拉有共享控件，已有缩放和布局回归 | 三层样式尚未完全落实；Browser 专属列比例、图标尺寸策略等仍在通用 metrics；没有完整 Zoom/DPI 与同条件前后截图矩阵 | 部分达标，未完成视觉验收 |
| 可拓展程度 | Attribute/Registry、Action 查询与执行、Drawer 发现、脚本导出按现有协议接入；有候选切换、Missing 和恢复测试 | 新 Graph 呈现不改 Shader/核心的完整接入证明不足；连续 10 次 UI/plugin reload 与旧 ALC 回收证据缺失 | 扩展基础达标，完整承诺未验收 |
| 操作逻辑性 | 有序选择、anchor/range/toggle、多对象修改、Play identity 路由和编辑手势具备共同契约 | Console 搜索未实现；统一键盘选择链没有完成证明；Stats 趋势未实现；真实完整编辑→Undo→保存→重开旅程不能由帧数代替 | 部分达标 |
| UndoRedo | 非 LIFO 拒绝先校验再弹栈；同一 History 处理手势、批量操作和中立 payload；失败保留操作，Missing 可恢复；公开边界回归较强 | 全 Panel 生命周期、键盘/鼠标混合输入、连续 reload 的端到端矩阵仍未完成；不可将单独 handler 测试当成全部界面旅程 | 本轮最扎实的部分，核心契约达标 |
| 菜单清晰度 | 菜单/快捷键/工具栏/搜索共享 Action Query/Execute；已有冲突诊断和禁用原因契约 | 静态菜单结构及 shortcut 候选尚未按 generation 建索引；目录顺序/无重复/禁用原因覆盖和完整键盘旅程仍需验收 | 功能基础达标，结构与成本未完全达标 |
| 组件颗粒度 | 领域操作、呈现和 Panel 的项目粒度合理；大部分功能文件有明确 owner | Shader Canvas 仍集中多类交互，Console 手动布局与通用样式仍需收拢；应按可独立验证的职责拆分，不以文件数为目标 | 主干合理，局部仍偏粗 |

因此，不能将当前整体认证为“企业级、现代级、现象级全部达成”。其中现代化的生命周期与分层基础已经存在；“现象级”还包含主观使用体验，自动测试不能给出这样的结论。

## 真实调用链与边界自查

### 命令、选择与修改

实际链路是输入 → Session/Widget → Action 或 Feature → owner 的序列化上下文 → 统一 History → 当前 generation 的 handler。Panel 不应自行建立第二套业务可用性、序列化、对象索引或 Undo 协议。

`EditorActionRouter` 的 Action ID 索引确实按 discovery 失效；执行时仍重新查询上下文。`EditorExtensionCatalog.ValidateActions/ValidateShortcuts` 会报告冲突，不能因为运行时排序存在 tie-break 就认定没有冲突检测。跨帧目标通过 Core Identity 或受约束的弱引用解析；Play allocator 的 routing lease 只路由，不是另一张 live-object 权威表。

### Session 与基础能力复用

`EditorUiSession.current` 是 ThreadStatic 同步作用域入口；状态由实例的 widgets/menus 持有，不能简单把出现 static 视为全局状态泄漏。Enter/Dispose 检查线程、嵌套和上下文，owner 退休会清理自己的控件/菜单状态。非 Identity target 只有明确中立标量强持有，包含引用的 boxed struct 不被错误视作纯值。

本轮关键链复用了 Core Events、Identity、Serialization 与现有 History；没有发现本次 UI 重构新增平行事件总线、Undo 栈或对象身份系统。这个判断限定于已检查的 UI 改动，不能替代全引擎所有事件/缓存实现的审计。

### 平台和第三方边界

这些 UI 缺口属于 Editor owner，不要求修改 BGCS、Native generated、第三方源码或平台层。前次 BGCS 4.1.2 包完整性与平台无改动证据继续有效，本次没有重新把 122 个文件的历史校验写成新执行。Mac CI 已通过真实 ARM64 自动运行；本地没有 Mac 交互设备，人工 Metal/视觉检查仍未执行。

## 确认的实现缺口与待验证风险

以下“确认”表示从源码可直接定位的现状；没有测量的数据不标为性能回退，没有复现的异常不标为已发生故障。

### U01 — 静态菜单与快捷键索引未落实

- 原计划：第三节 Action/菜单与性能条目，静态结构按 extension generation 索引，可用性 fresh 查询。
- 位置：`framework/Inno.Editor.Interactions/Menus/EditorMenuCatalog.cs:14–112`；`Actions/EditorActionRouter.cs:179–220,350`（均相对 `src/composition/editor`）。
- 当前 `Build` 每次遍历 registrations、收集 placement、规范化并拆分路径、排序和构造树；shortcut dispatch 每次收集全部 Action ID 和候选。
- 已有 `m_byId` 不等于静态菜单或 shortcut 索引。动态菜单也不能被整体缓存成过期 enabled/checked 状态。
- 完成方式：generation 内固定静态 metadata/候选索引；上下文、动态 contribution 和 Query 继续 fresh；候选切换时替换并释放旧 generation 引用。复用现有 catalog，不另建全局 cache service。
- 验收：重复打开不重建静态结构；新增/卸载扩展正确更新；上下文变化立刻生效；冲突仍明确失败；对菜单打开至少五次采样 CPU/分配，包含旧 ALC 退休。

### U02 — 大数据视图与 Stats 尚未满足成本要求

- `FileBrowserPanel.List.cs:73` 遍历全部 entry，Grid 同样逐项提交；Hierarchy 每次绘制重新构造 visible-selection 列表。Native clipping 不等于避免 managed 每项处理。
- `ShaderEditorCanvas.Node` **已有离屏矩形提前返回**（约 622 行），不能声称 Graph 完全不裁剪。但 `PortPositions()` 仍为全图创建 Dictionary 并计算端口，绘制/命中仍遍历图。是否构成严重瓶颈必须通过 2,000 节点实测判断。
- `EditorStatistics.GetSnapshot():230` 每次合并 Dictionary、排序和 ToArray；`StatsPanel.OnDraw():35` 每次 GroupBy/Select，并重新构建字符串。它有 0.5 秒帧时长采样器，但不能据此宣称展示只读取复用采样快照；趋势显示尚不存在。
- 完成方式：列表按可见范围提交，保留编辑/拖动/展开状态；显示数据按实际 revision、过滤、排序和尺度更新。Stats 的采样与不可变展示快照由明确 owner 管理；Graph 先测全图成本，再决定必要的增量几何复用。不能用复杂池掩盖重复工作。
- 验收：10,000 项列表/树、2,000 节点图，空闲/滚动/搜索/拖动至少五次重复，CPU、分配、GC、median/P95；不得丢失多选或拖动状态，不得降低刷新正确性。

### U03 — 通用 Graph 呈现与领域样式边界仅部分完成

- `presentation/Inno.Editor.Graph.ImGui/Canvas/GraphCanvasView.cs` 提供坐标转换、网格、连线绘制和连线命中。它保持无 Shader 依赖，是实际有效的抽取。
- 原计划还要求通用节点/命中/框选；这些行为仍与 Shader 的端口、默认参数、文档 draft 交织在 `ShaderEditorCanvas`。partial 文件分开并不自动解除共享状态依赖。
- `EditorStyleMetrics.cs:230–562` 集中 Browser pane、breadcrumb、列分隔比例及图标布局等策略。它们不是平台耦合，也不等同于“包含资产类型白名单”，但三层样式中领域 owner 的责任仍不够清楚。
- 完成方式：由真实第二种图消费者验证最小可复用节点、命中和框选输入/结果；Shader 只贡献自身端口/参数/颜色语义。Browser 专属布局值由 Assets presentation owner 组合通用尺度，通用层不反向认识 Browser。
- 验收：非 Shader 测试图可使用同一呈现机制；领域专属类型不泄漏进通用层；节点选择/框选/缩放/连线与 History 语义保持；样式同帧一致、API/脚本/Wiki 同步。

### U04 — Console 搜索与键盘操作验收未闭合

- `ConsolePanel.DrawToolbar():102` 只有 Collapse、级别 Filter、Clear；DrawEntries 只有级别筛选。原计划明确要求的文本搜索没有实现。
- Console 没有统一的条目键盘选择模型。Hierarchy 的共享 Tree 返回鼠标 clicked/doubleClicked；原生 Tree 可以处理部分导航，但这不足以证明它与引擎 ordered selection、范围选择、焦点和批量命令一致。
- File Browser 使用原生 Selectable，不能因此断言完全没有键盘激活；缺少的是原计划统一键盘选择/焦点行为的完整证明。
- 完成方式：Console 搜索使用现有 Diagnostics snapshot 与 revision；键盘动作接入既有 Action/Selection，不能新造列表专用事件系统。明确焦点、上下移动、范围扩展、激活、取消和只读行为。
- 验收：真实公开输入驱动 Console/Hierarchy/File Browser，覆盖过滤后导航、Shift 范围、Ctrl 切换、输入框与 Popup 遮挡；结果与鼠标操作一致，批量修改仍原子化。

### U05 — 异常栈平衡与长期退休仍有证据缺口

- Shader 节点端口标签局部 `PushClip`/`Text`/`PopClip` 没有局部 finally；Console 也有部分 Child、style 和 ID push/pop 直接顺序配对。
- 这是源码审查得到的异常安全风险，不是本次已复现的崩溃。正常产品跑通不能证明中途异常一定平衡，也不能在没有可触发路径时武断判为 P0。
- 完成方式：在已有 owner/borrow 边界补齐成对资源的异常安全；通过实际可替换 public boundary 注入失败，确认原生栈和后续绘制可继续。没有必要引入一个覆盖所有 UI 的新异常框架。
- 再执行 100 次 Panel/Document 开关、10 次脚本/plugin reload、Host 退出资源检查；记录 UI owner、订阅、gesture、旧 ALC weak monitor 和 GPU 资源。保持严格 unload barrier，不放宽超时。

## 原计划验收对照

| 原计划条目 | 当前有效证据 | 本次判定 |
| --- | --- | --- |
| 无 Panel→Panel、feature→UI/native 依赖 | 架构验证与当前项目引用 | 已通过该边界检查；不代替全部内部职责审查 |
| History LIFO、手势、失败/Missing、批量 | 当前源码、121 项 Interactions 本次复跑；Windows/Mac CI 对应回归 | 核心契约通过 |
| 无 native borrow 越界、Session owner 退休 | UiSession/native widget tests，关闭 fixture | 有针对性证据；不等于全 UI 长时认证 |
| Static menu/shortcut generation index | 仅 Action ID 索引存在 | 实现未完成，见 U01 |
| 通用节点/命中/框选 presentation | 通用坐标/网格/曲线/连线命中存在 | 部分完成，见 U03 |
| Console 搜索、键盘选择；Stats 趋势 | 搜索/趋势未实现，键盘矩阵不足 | 未完成，见 U02/U04 |
| 7 档 Zoom × 可执行 DPI 场景、长标签/遮挡/焦点/中文输入 | 有若干真实 native layout/输入测试 | 不能推断全矩阵已通过 |
| 同布局、数据、尺度和尺寸的前后截图 | 有 FlappyBird 截图 | 没有完整成对视觉证据 |
| 当前 UI 操作、菜单与三层样式规范 | 有 `EDITOR_UI_ARCHITECTURE.md` 架构说明；原计划的 `EDITOR_UI_DESIGN_STANDARD.md` 未落地 | 架构说明不能直接代替操作/样式设计规范；应补足实质内容，文件名本身不是完成标准 |
| 10,000 项树/资源列表、2,000 节点图；至少五次 UI CPU/alloc/GC/median/P95 | 只有独立 UTF-8 编码 microbenchmark 和产品帧运行记录 | 未执行完整规模矩阵，不能宣称整体提速 |
| 100 次 Panel/Document 开关 | 有单次/定向生命周期测试 | 没有计划要求的 100-cycle 结果 |
| 连续 10 次脚本/plugin reload、严格旧 ALC 回收 | 有 reload/Missing/GC 定向测试 | 没有整个 UI 场景的 10-cycle 结果 |
| Windows Debug/Release、D3D11/12/Vulkan、Play→Edit | 本地 FlappyBird 和远端产品 smoke | 有界产品路径通过；不是完整人工操作旅程 |
| Canvas/Rendering2D 当前脚本及 SDK/API 投影 | 新 SDK probe、Rendering2D 42 项、正式包导出 | 已有证据按前次记录范围成立；不能扩大为所有插件操作已验收 |
| Mac CI | 最终 ARM64 job 通过 | 自动验收通过；本地/人工 Mac 交互未执行 |

原计划明确允许通过真实 ImGui Context 和公开输入完成测试，**不依赖 computer use**。因此此前窗口激活失败只能说明人工操作没执行，不能解释或豁免完整自动几何/输入矩阵的缺失。

## Issue 核对

| Issue | 源码与证据结论 | 处理 |
| --- | --- | --- |
| [#13](https://github.com/Wolfiora/InnoEngine/issues/13) History 非 LIFO 拒绝破坏栈 | `EditorHistory.PopTransaction` 先 Peek/校验，再 Pop；Commit/Rollback/Dispose 三种拒绝后仍可正常回滚/Undo。最终 Windows/Mac TRX 各三项通过；本次 Interactions 复跑通过 | 已在本分支修复，PR 添加 `Fixes #13`；合并前不把 master 标为已修复 |
| [#16](https://github.com/Wolfiora/InnoEngine/issues/16) 固定默认 assets 路径 | 当前及 master 测试都由 SDK Restore 返回实际 `ProjectAssetsFile`，使用独立 ArtifactsPath；两平台最终 TRX 通过。旧描述的固定路径已不存在 | 根因已修正；原 issue 还要求真实 package/load mismatch 拒绝等完整矩阵。本次没有新的 mismatch 反例证据，不直接宣布全部关闭 |
| [#18](https://github.com/Wolfiora/InnoEngine/issues/18) API XML/Wiki 语义 | UI 新边界已补文档，但 Stats 等仍有泛化描述，全仓语义审查不因 XML/Wiki gate 通过而结束 | 保持开放；与本次文档质量有关 |
| [#19](https://github.com/Wolfiora/InnoEngine/issues/19) 多 SDK reference 一致性 | SDK 直接引用 API owner 修复了另一种依赖问题；没有证明多 pack/root 输入一致 | 保持开放，不混淆两类问题 |
| #14 / #15 | 热 Native 准备成本、自动维护预算收敛 | 0 native processes、显式清理成功都不是成本/自动预算验收；保持开放 |
| #17 / #20 / #21 / #22 | 文件系统语义、原生编码、Runtime coroutine、Browser 诊断 | UI 改动和本次审查没有关闭这些问题的证据 |
| #23 | NuGet 锁文件治理 | 已关闭且被标准 PackageReference 流程接替；不重新引入锁清单 |

没有因为 issue 标题看起来相近就直接关闭，也没有将旧问题重新包装成此次 UI 回归。U01–U05 在本报告给出 owner 和验收条件，作为 PR 尚未完成的计划清单；本次没有批量新建重复 issue。

## 测试与证据

本次重新运行：

```powershell
dotnet test tests/editor/Inno.Editor.Interactions.Tests/Inno.Editor.Interactions.Tests.csproj --no-restore -c SourceDebug --logger "trx;LogFileName=interactions.trx" --results-directory D:/InnoEngine-Verification/editor-ui-2026-10-27/plan-review-tests -v quiet
```

结果：**121 passed、0 failed、0 skipped**。此命令重新构建当前源代码，不以 `--no-build` 的旧程序集替代。日志 `plan-review-interactions.log`；TRX `plan-review-tests/interactions.trx`。

本次重新解析最终 CI 原始归档（不是重新运行 CI）：

| 来源 | 结果 | 适用限制 |
| --- | --- | --- |
| [Engine CI 40444183857](https://github.com/Wolfiora/InnoEngine/actions/runs/40444183857)，head `2dc37114` | Windows 2,230 pass / 18 skip；macOS ARM64 2,247 pass / 1 skip；均 0 fail | 环境相关 skip 保留；自动产品 smoke 不替代完整人工旅程 |
| 两个 managed job | 各 1,185 pass | 与完整 suite 重叠，不相加 |
| [NuGet graph 40444183878](https://github.com/Wolfiora/InnoEngine/actions/runs/40444183878) 与 submission | 通过；该 head 共 6/6 checks success | 不代表原计划缺失条目已经被 CI 覆盖 |
| 本次从两平台 TRX 定向抽取 History/SceneHistory/UiSession/GraphCanvas/包身份 | 每平台 54 条通过，包含 #13 的三种拒绝和 #16 还原身份 | 子集证据，不与上列累加 |

证据 owner 为 `D:/InnoEngine-Verification/editor-ui-2026-10-27`，新增 `plan-review-evidence.json` 保存原计划哈希、归档哈希、具体 test ID/名称/结果及 TRX 路径。既有 `remote-ci-final.json`、两平台 `remote-ci-*-contract-analysis.json` 和实际下载归档保留。

前次微基准只证明编码缓冲实现减少分配，不能推出整个 Editor 更快。Mac Metal 的八条 BGFX refcount warning 在 master 基线也存在，device count 有差异、根因未知；本次不将其当作新 UI 缺陷，也不宣称已经无 GPU 泄漏。GitHub Action runtime 弃用告警同样不是此次 UI 完成标准的替代项。

## 后续闭合顺序与 PR 决定

1. **先修正文档和状态**：撤回“整个实现完成”的表述，保持原通过日志，区分缺功能、缺抽象、缺测量。当前问题页修正仍把 #13 描述为未修代码的旧结论。
2. **完成 U01/U04 的用户可见闭环**：generation 索引、Console 搜索、统一键盘选择；保留 fresh Query、既有 Core Events/Selection。
3. **完成 U03 的职责收拢**：用实际非 Shader 消费者验证通用 Graph 机制；领域布局回到领域 presentation，公开 API 和 SDK 同步。
4. **完成 U02 的测量与针对性实现**：先固定相同基线与输入，记录 10k/2k 成本，再实现必要可见范围/增量快照；不凭猜测建池。
5. **闭合 U05 和完整验收矩阵**：异常路径、100-cycle/10-reload、Zoom/DPI、同条件截图、完整 FlappyBird 编辑旅程及插件安装态。真实 Mac 人工项单列。
6. **最终同一源码再判定**：适用实现与验收全部通过后，将 PR 从 Draft 转 Ready。同步 Rendering2D/Samples 的调用方交付状态；它们此前仅本地提交，不把引擎 PR 当作四仓库都已远端发布。

本次不为给出漂亮评价而临时大改未测的菜单缓存或图形算法。已存在的 **PR #26 继续面向 master 并保持 Draft**，无需再创建一个同分支重复 PR。标题和正文继续英文、`type(scope): description`；`Fixes #13` 只在合并时关闭已修复 issue。

自查答案：**基础分层和 Core 复用有真实代码支撑，但“超干净、全部达标”的说法目前不成立。** 上述缺口属于明确的后续工程工作，不能用改名、拆文件、增加接口或更多正常路径 smoke 掩盖。Windows 无 `/System/Library/Sounds/Glass.aiff`，本次未播放指定提示音。
