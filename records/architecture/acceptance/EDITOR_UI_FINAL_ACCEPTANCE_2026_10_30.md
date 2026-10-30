# Editor UI 完整计划验收与交付记录

生产实现：`dd5979c613b0fe8ebd9f7026f25001ceee9f8cee`，分支 `editor-ui`。本报告之后的交付提交只允许包含文档及验收证据；其完整 SHA、差异证明、最终 CI 和 Ready 状态以 [PR #26 的精确提交附件](https://github.com/Wolfiora/InnoEngine/pull/26#issuecomment-6516741338)为准。**本地通过不自动等于最终 CI 通过。** 附件确认全部适用检查后才转 Ready，不自动合并。

本地适用的功能、原生输入、六组 Windows 产品、生命周期、正式脚本/IDE、文档及受控性能检查通过。真实显示器、OS IME、人工 Mac 操作未执行。没有“现象级”认证，也不把本轮 UI 验收说成整个引擎无缺陷。

## 范围、身份与追踪

- 原计划：桌面批准计划的内容哈希及逐条来源见[计划映射](editor-ui-2026-10-30/original-plan-requirements.csv)和[证据清单](editor-ui-2026-10-30/manifest.json)。共 307 行，含十个界面工作流；其中 7 行是状态词汇、2 行是未来合并后核对规则，不能算作九项功能测试。
- 在报告写入时，283 行本地要求通过，15 行交付/精确 CI 条件待外部附件确认，9 行明确不适用。最终附件逐行补齐交付条件，不能从本地数字推断 CI 已通过。
- 实际 issue：已有 #13、#16、#19；总项 #27；E01–E12 对应 #28–#39；实现和验收中新发现的必要前置项 #40–#55。共 32 项，必须经分支验证后保持 Open，PR 逐行 `Fixes`，合并 master 后关闭。#18 仅关联本轮语义文档，用 `Refs #18`。
- SDK 10.0.402、.NET 10、C# 14、BGCS 4.1.2。设备、驱动和系统原始数据见[环境](editor-ui-2026-10-30/environment.json)：Ryzen 9 6900HX、RTX 3070 Ti Laptop、Windows 11；同机比较，不用共享 CI runner 推断性能。
- 配套仓库：Canvas `eed5fd9b`、Rendering2D `067d4eb0`、Samples `3e331fd0`；本次收尾没有改它们的源码。正式 Canvas、Rendering2D、FlappyBird 脚本与 IDE 流程均通过，安装态调用由现有插件回归覆盖。
- BGCS 仓库 `28945389` clean，Agent 文件为零；四份 4.1.2 包归档 SHA 与实施前一致，126 个被核对的安装条目与归档一致。见[包完整性](editor-ui-2026-10-30/bgcs-package-integrity.json)。这与真实 Restore/load 内容身份测试是不同证据，不能互相替代。
- 原始日志 owner：`D:/InnoEngine-Verification/editor-ui-closure-2026-10-27`。完整 TRX、binlog、失败尝试与运行文件清单保留在该 owner；可审查的原始采样及结果索引约 2 MiB，纳入本报告同目录。

## 实现闭合与职责

| 项目 | 实际实现与验收边界 |
|---|---|
| E01 / #28 | 作用域按实际 Begin/End、Push/Pop 规则配对；可扩展绘制失败只退休自身帧/owner；下一帧、另一 Session 与错误释放顺序有公开边界回归。 |
| E02 / #29 | 菜单路径、顺序、Action ID、gesture 候选按 extension generation 建索引；可用性、目标、参数和动态贡献每次请求重新求值；失败保留 last-good，旧 generation 不保留实例。 |
| E03 / #30 | 基础、通用组件与领域样式归属分开；Assets/Shader 规则不进入通用 metrics；2:3 表单、单滚动 owner、边界下拉和同帧尺度一致。 |
| E04 / #31 | Hierarchy、Browser、Console 使用过滤后可见顺序导航、anchor/range 和焦点策略；Console 搜索既有 occurrence/group，Collapse、详情和复制指向同一匹配结果。 |
| E05 / #32 | Graph 文档/controller/History 保持中立；Graph.ImGui 负责中立几何、命中和交互意图；Shader 自己解释类型与编译。第二种非 Shader 图真实使用相同公开呈现契约。 |
| E06 / #33 | EditorStatistics 是权威；未变快照与分组复用；Panel 自有 0.5 秒/60 秒/120 点数字趋势缓冲，暂停与关闭重置。 |
| E07 / #34 | 列表/网格按可见范围提交；树的显示序列按实际 revision/展开/过滤/排序更新；Console 高度缓存和 Graph 相关节点几何失效不成为第二份领域数据。 |
| E08 / #35 | 实际 Inspector、Hierarchy、Browser、Scene/Game、Shader、Settings、Export 与 Workbench 的正常、失败、取消和关闭边界有对应工作流；数据修改统一进入 feature 和同一 History。 |
| #13 / #16 / #19 | History 拒绝非 LIFO 完成时保持栈；SDK 实际 assets 与真实已加载内容一致、错配被拒绝；实际选定 SDK 向运行编译和 IDE 提供相同 references/符号/语言事实。 |
| E09 / #36 | 真实 ImGui Context + 公开输入覆盖 7 Zoom × 4 DPI × 2 尺寸，共 56 个尺度/尺寸组合；额外窄窗、遮挡、滚动、长文本、Unicode、各种状态单独覆盖。模拟 metrics 不称为物理显示器实测。 |
| E10 / #37 | 实际九类 Panel 各 100 次开关；Scene/Shader 文档各 100 次；连续十次成功/候选失败/Missing/恢复 reload；严格 GC→finalizer→GC 与弱 monitor 不放宽。 |
| E11 / #38 | 可运行原始 producer `b303d9f1` 与当前实现同 SDK、配置、输入成对交替，150 条初始记录；图尾部复查另加十个进程、每进程 50 帧，保留两批全部数据。 |
| E12 / #39 | 当前 XML、API Wiki、脚本导出、项目引用、Solution、文件树、十一维评价与逐条计划映射同步；移除确认无调用的直接 Hierarchy 删除路径。 |
| #40–#44、#52 | 输出锁问题修在 SDK 项目/输出 owner，而非杀 IDE 或全局串行化；工具验证使用本次 SDK 输入；Asset owner 保留身份并验证实际内容；Shader 输入冻结排除的仅是 Core 原子发布保留候选。 |
| #45–#46、#53–#54 | Core 既有 Writer、member capture、不可变规范快照和可选完整输入证明减少重复工作；Scene 对单次属性操作借用准确 Core allocator，完整 graph/subtree/Prefab 仍保留各自图引用语义。 |
| #47–#51 | 标准 Host 提供默认布局，缺失才原子创建；已有空/自定义文档不覆盖。Core reader 允许原子替换共享。官方未改 Noto CJK 字体按实际 Context/atlas 生命周期补字；成功退休后精确解除登记。Missing Scene 源分配和编辑 bytes 保留直到恢复。 |
| #55 | Compiler 完整 staging/校验仍归 Compiler；发布/替换复用 Core AtomicDirectory。取消前后、失败回滚与提交后清理语义明确，不增加脚本重试或平台分支。 |

完整实际文件树见[源码 owner 树](editor-ui-2026-10-30/file-tree.md)，实际 241 个工程、30 个 Editor 工程；不是未来目录图或空项目清单。执行链深审见[所有权复查](editor-ui-2026-10-30/ownership-review.md)。

## 最终生产实现的本地结果

| 检查 | 结果 |
|---|---|
| 源码、工具、两开发入口、Editor Debug/Release、选定 Player 与测试输出准备 | 11/11 阶段通过 |
| Contracts | 822 次通过、2 次 Unix 专用文件链接测试在 Windows 未执行 |
| Workflows | 704 次通过 |
| Visual | 524 次通过 |
| Lifecycle | 15 次通过 |
| Core Serialization / Scene / Core IO | 97/97、67/67、41/41 通过 |
| 正式配套项目脚本与 IDE | 三个项目全部通过 |
| 源码分层、工程图、公开依赖、XML、Wiki | 241/241 工程 owner 求值；34,582 API inventory；197/197 实际编译示例；零错误、零未求值示例 |
| Windows 产品 | Debug/Release × D3D11/D3D12/Vulkan，6/6 通过 |
| 完整输入冻结 | 16,406 个文件、3,207,829,122 bytes，测量前后完全相同 |
| 受控性能 | 初始 150 条记录、68 项比较逐项审阅；图尾部预先声明的额外十进程复验后，无未解决的可重复 >10% CPU/P95 或热分配回退 |

各 profile 有重叠，不将这些次数相加当作独立测试数量。原始名称、outcome 和 TRX 路径见[用例索引](editor-ui-2026-10-30/test-cases.csv)；全部产品循环资源样本、阶段状态与分配量见[本地结果](editor-ui-2026-10-30/local-acceptance.json)。不能把两次 Windows skip 记为 Unix 通过。

六组产品各完成 300 帧隐藏 smoke、八项真实 feature 工作流及实际 Panel 100 次开关。工作流会绘制实际 Panel，并通过公开 feature 操作；它与独立的原生指针/键盘测试共同提供证据，不能称为每一步物理鼠标端到端操作。循环资源采样 0–100 每十次记录，所有 pending events/selection 为零；最终 Full-GC managed 增量介于 -649,584 与 823,016 bytes，没有按已完成次数线性增长，均低于 4 MiB 预算。进程 working set 不要求回到一模一样。

GPU 观察记录了十二个确切归属的 workflow/lifecycle 进程与 NVIDIA C+G context 关联，不推断独占 GPU 或物理屏幕输出；WDDM memory 为 N/A。见[原始观察](editor-ui-2026-10-30/gpu-context-observations.json)。

## 性能：收益、回退审查与限制

同一 .NET 10 SDK 下比较源码实现，不把结果归因为运行时升级。以下是五个独立进程结果的中位数；不同负载不可互相比 FPS。

| 范围 | 原实现 | 当前 | 解释 |
|---|---:|---:|---|
| 10,000 Hierarchy 静态帧 median | 4,387.32 ms | 2.02 ms | Scene root 索引、可见提交与重复捕获改良；原始二次复杂度负载，不代表普通场景几千倍提速 |
| 10,000 资源静态帧 median | 3,431.32 ms | 15.39 ms | 相同输入与窗口下的可见工作；分配约 249.23 MB → 0.92 MB/操作 |
| 10,000 资源搜索 median | 10,467.93 ms | 73.16 ms | 显式搜索操作，仍有可观察延迟，不等同稳定帧时间 |
| 真实 History 连续属性编辑 median | 4,060.34 ms | 1.55 ms | 正常 SceneEdits/History 路径，未用直接标量赋值替换 |
| 菜单分配 | 8,544 B | 4,240 B | generation 静态结构复用，动态 Query 保持 |
| 未匹配 shortcut 分配 | 1,424 B | 48 B | 候选索引避免全量扫描 |
| 未变统计读取分配 | 16,792 B | 400 B | 复用权威快照；不是任意字符串解析缓存 |
| 产品 idle median / P95 | 9.88 / 30.86 ms | 9.63 / 25.52 ms | 实际同布局产品 CPU 帧范围，非 GPU fence/显示完成 |
| 产品 idle 分配 | 4.88 MB | 1.94 MB/帧 | 同输入测得约减少 60%；不是全产品所有场景保证 |
| 产品 Play median / P95 | 11.94 / 17.15 ms | 11.85 / 15.43 ms | Play 分配约减少 1.2%，不夸大为数量级改进 |
| 第一个应用帧 wall | 8.935 s | 8.929 s | 大致持平；源码激活 CPU 帧间隔反而 52.72 s → 56.65 s，约 +7.46%，仍在本轮门槛内，不能说所有启动阶段变快 |

**原始图尾部标记保留。** 初始每进程十帧，nearest-rank P95 等于 max；P95 中位数 29.95 → 35.93 ms，约 +19.98%。按 #38 预先记录的复验规则，使用同一未修改 fixture 的正常五轮、每进程 50 帧，额外五组成对进程得到 P95 32.84 → 32.07 ms、CPU 40.00 → 36.875 ms/操作，分配 4.47 → 3.30 MB；没有一对 P95 超过 +10%。未挑最好一轮，所有帧和 outlier 保留。这支持“未重现持续回退”，不支持稳定 60 FPS、尾部完全无波动或已经找到噪声的具体外部来源。

十次独立 10,000 对象完整 dirty inspection（idle/edit）在不改变 100 ms 策略下均满足 median <16 ms、P95 <25 ms、分配 <8 MiB。最差 P95 **24.5695 ms**，余量较小，后续负载增加需要继续测量。不是只检查可见对象或跳过 getter/converter/Missing 验证。

[完整比较](editor-ui-2026-10-30/performance-summary.json)、[逐项审查](editor-ui-2026-10-30/performance-review.json)、[图尾部复验](editor-ui-2026-10-30/graph-tail-summary.json)及 `raw-*-measurements.json` 保留所有原始记录。短命令 CPU 的零值是计时量化，不宣称 CPU 开销为零。

## 视觉、输入和当前数据

[原始外观](editor-ui-2026-10-30/original-ui.png)与[当前外观](editor-ui-2026-10-30/current-ui.png)是同 1600×900 布局、相同数据的 native draw replay，包含真实 GPU viewport texture readback。Scene/Game 中游戏内容正常显示；主要停靠分区保持，菜单规整为统一动作入口，Stats 增加有界趋势。人工图像检查由本次助手完成，不冒称用户人工验收或物理屏幕截图。

当前 replay：1,827 triangles，零 unavailable GPU commands、零 unexecuted callbacks。未选择对象时 Inspector 为空是合法状态。两张图中已有的文件名裁切不当作新增回归。图上的瞬时 FPS 不作为性能采样。56 个 native metrics 组合的几何/交互断言与这一匹配图像对是两类证据，不声称拍了 56 组物理显示器截图。

默认布局在项目 `editor.ini` 缺失时由标准 Host 初始化；已有自定义或空文件保留。Selection、History、dirty Scene、runtime 引用不进入 INI。正常退出先捕获 Module/Panel，再取最新 layout，最后原子写入；恢复未成功不能覆盖已有状态。

## 深层自查与必要 API

已自问并核对实际调用链：**职责是否真实清楚，是否为了性能破坏正确性，是否重复造基础设施？** 本轮未发现必须用妥协路径才能完成的剩余实现。具体依据：

1. Core Events 输入进入一个 Session；Action generation 索引不是事件总线，Host 逆序释放栈也不是监听器注册表。
2. 跨帧目标经 Core Identity；Scene 的 source-ID 投影只用于当前序列化输入，不成为 persistent-ID→live-object 的第二权威。移除的 System 不被结构缓存长期持有。
3. Snapshot 和规范编码仍属于 Core Writer；可选 capture proof 必须完整，含副作用/上下文/不确定性的 Converter 继续使用权威捕获。同步借用 builder 退出回调即退休；中立缓存不保存 source、Type、delegate 或 ALC。
4. 单属性修改使用当前 Core allocator 的同步 scope，逐引用校验 domain、注册、存活和 Scene。Graph local ID、Prefab 和 subtree 的完整映射仍由原 owner 处理。
5. Graph 呈现不解释 Shader；Panel 不拥有领域事务；History payload 中立，失败不移动栈或提前释放回滚 owner。
6. SDK 决策来自实际选定的 MSBuild 求值，Compiler 不依赖 Build/MSBuild/平台；没有最高 reference pack 猜测或配置 fallback。
7. Compiler 发布使用现有 Core 原子协议，旧树回滚使用未取消 token，提交后清理失败不谎报未提交。原始外部 handle owner 未确认，不归责 BGCS/Roslyn/杀毒软件。

Core IO 的通用宿主文件系统实现也必须如实说明：create-only 发布在 Windows 使用 SDK 的不覆盖 move，在 Unix 使用公开 POSIX `link`，不支持相应原子能力的宿主/文件系统明确失败。这里包含 Windows/POSIX 实现区别，归既有 Core IO 原语，不等同于把产品平台 SDK、ABI、图形 backend 或部署策略放入 Core。Editor/Scripting 只消费中立契约；不能把“没有泄漏产品平台策略”写成“Core 没有 OS 分支”。

必要公开边界：中立 Graph node/port/layout/input/result；Host 注入不可变 ScriptCompilationInputs；Core member/snapshot/capture proof；EditorContext 可选默认文档；既有 AtomicDirectory.Install 的可选 CancellationToken；AtomicFile.IsCandidatePath 让外部构建输入 owner 复用准确的原子候选命名协议，识别结果不授予删除权限。它们有真实跨程序集消费者，且同步 XML、Wiki、项目引用和失败/生命周期语义；没有为了测试扩大 internal，没有保留旧 overload/facade。菜单、Console、Stats、source-ID 投影和编码缓存保持内部。

新增 Action/Drawer/Panel 使用现有发现协议；新增图领域只提供自己的 controller/语义和中立呈现；新平台仍实现其 provider/integration 并验证真实 SDK/ABI，UI 不增加平台 switch。没有为了基准降低 validation、reload barrier、Missing、History 原子性或输出完整性。

## 十一项评价

“满足”限定为本计划可执行的工程标准，不是没有后续改善空间。

| 维度 | 判断与证据 | 限制与合理改进点 |
|---|---|---|
| 架构 | 满足：feature/presentation/Host 方向明确；241 工程、公开依赖图与真实执行链一致 | 不替代全引擎其他开放 issue 的审计 |
| 代码整洁度 | 满足：按功能 owner 组织，删除无调用的旧 mutation 路径，项目/namespace/XML/引用分组有门禁 | 较大的领域协调器继续按实际独立职责演化，不追求机械拆文件 |
| 代码与状态干净程度 | 满足：Session/frame/document/generation 所有权、异常、取消、100 次退休与十次 reload 有证据 | 不能以 working set 不归零断言泄漏；新扩展需同样验证 |
| 抽象程度 | 满足：第二种非 Shader Graph、SDK→中立输入、可替换 Converter 有真实消费者 | 不声称未来任何能力都无需扩展契约 |
| 可复用程度 | 满足：Core IO/Events/Identity/Serialization、统一 History 实际复用 | 派生显示投影和有界采样属于领域状态，不应强塞入 Core |
| 样式统一 | 满足自动规则与已审图像范围：三层样式、表单、滚动、下拉、56 组合 | 物理屏幕、IME、可访问性和更广用户研究未执行，不能称视觉完美 |
| 可扩展程度 | 满足当前插件/图/菜单/Drawer 扩展标准，无中央领域 switch | 长期第三方采用和 API 演进仍需真实经验 |
| 操作逻辑性 | 满足已列工作流：可见选择顺序、焦点/遮挡、只读/Missing、失败/取消 | feature 驱动产品旅程不替代所有物理手势和主观易用性研究 |
| Undo/Redo | 满足：单一 History、即时手势、离散操作不错误合并、批量原子回滚、Missing barrier | 新领域操作需继续提供稳定 handler 与失败用例 |
| 菜单清晰度 | 满足结构标准：同一 Action 协议、fresh 状态、稳定分组、冲突诊断与索引复用 | 用语可发现性仍可根据实际使用改进 |
| 组件颗粒度 | 满足：Core/feature/presentation/Panel 以真实职责与资源边界划分 | 不为目录对称建空项目，不为行数再造 helper 层 |

## 失败、未执行与资源说明

- 保留原始无法启动的基线尝试、旧候选失败、#40 输出竞争、#48 读写竞争、#55 raw Directory.Move 失败与红/绿回归。最终通过没有抹掉这些记录。
- 第一次冷发布观察脚本误将首轮导入的 metadata 时间戳写入当作缓存失效：19 个 `.imeta` bytes/hash/ID 未变。修正的是外部观测分类；五个独立 cold/warm 请求确认 warm 源/metadata/cache 的集合、内容及时间戳均不变。没有放宽生产校验。
- 三个非当前 Windows 目标的产品 API 编译没有在本地冒充执行；精确交付的 Mac 自动构建/测试/产品 smoke 由最终 CI 附件说明。人工 Mac、不同物理显示器、OS IME、Rider Run 点击体验、音频听感和长期运行未执行。
- Browser Editor、新 Timeline、渲染模型和其他全引擎独立 issue 不属于本轮。此前 Browser Player/BGCS 专项证据保留原 revision，不并入本次 UI 数量。
- 验收大输出优先在 D 盘；正式维护先 dry-run，再以默认预算、600 秒显式预算执行 `clean --scope cache`，exit 0。逻辑库存 90,213,428,162 bytes，退休 90,039,337,805 bytes，保护 174,090,357 bytes；C 盘空闲实测 77.79 → 90.32 GiB，增加约 12.53 GiB，D 盘仍约 5.78 GiB。见[维护日志](editor-ui-2026-10-30/cache-maintenance.log)与卷空间 JSON。只回收明确 owner、无活动读取者的可退休内容，不把 hardlink 逻辑字节当实际释放空间。
- **自动审批曾拒绝**删除过期诊断 `e09/visual-capture/current/Runtime` 及 `e46/snapshot` 下四个旧诊断 runtime 目录，只返回 `blocked by policy`，未提供更具体理由。这五处保留，没有换工具绕过，也不宣称已释放其空间。
- Windows 无指定 `/System/Library/Sounds/Glass.aiff`，并遵守静默操作要求，未播放提示音。

## 执行与复验入口

正式入口为 `tests/Invoke-EditorUiAcceptance.ps1`，提供 Contracts、Workflows、Visual、Lifecycle、Performance、Products、All，继续编排 xUnit/VSTest 及现有隔离测试；零发现、工具错误不算成功。产品需显式给独立项目副本、GraphicsApis 与实际初始化预期。

本次完整命令清单和阶段原始输出在 `candidate-dd5979c6/preparation.json`、`matrices.json`、各 profile summary/TRX；受控采样由同 owner 下 `e54/Invoke-FinalQuantitative.ps1`、预先声明的 `Invoke-GraphTailPairs.ps1` 编排正式公开 fixture。Wiki 使用正式 `tests/Invoke-WikiChecks.ps1`、Release、`snapshot-ordered` 输出及本次正式生成的逻辑 references。原始路径只作为证据 owner，不进入生产代码。

最终应再次核对：精确交付 diff 仅文档/证据，Git author/committer 均为 Aaron Liao，无 co-author；适用 CI 全绿；32 个 issue 的实际条件和英文结论齐全；PR 逐行 Fixes、#18 Refs；仍 Open 等待 merge。其最终签名、命令和检查链接统一记录在[精确提交附件](https://github.com/Wolfiora/InnoEngine/pull/26#issuecomment-6516741338)。
