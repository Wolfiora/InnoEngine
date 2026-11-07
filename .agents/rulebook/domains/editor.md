# domains/editor

按完整规则单元加载；路由与来源见 [规则登记](../coverage.json)。

<!-- rule-unit: editor-projects -->
<a id="editor-projects"></a>
## 13. Editor 项目组织与引用边界
- `src/composition/editor` 中每个项目的业务源码统一使用与 `.csproj`/程序集名称完全相同的命名空间；功能目录只负责组织文件，不追加到命名空间。例如 `Inno.Editor.Inspection/PropertyDrawing` 中的类型仍使用 `namespace Inno.Editor.Inspection;`。
- 可复用的 InspectionDrawer、PropertyDrawer、Registry 与 serialized property renderer 统一属于 `Inno.Editor.Inspection`；业务 Panel 只在自身项目中实现具体 Drawer，不得为了扩展检查显示而引用 `Inno.Editor.Panel.Inspector`。
- 唯一命名空间例外是 `Inno.Editor.ImGui/Widgets`：其中所有类型使用 `namespace Inno.Editor.ImGui.ImGuiWidget;`。
- `Inno.Editor.ImGui/Widgets` 只允许 `ImGuiWidget.*.cs` 文件。Widget 的 presentation、options、result 与私有状态应收口到对应的 `ImGuiWidget.<Feature>.cs`，不得创建独立的 Widget helper 文件。
- Editor 项目内部按可独立理解的功能建立目录（如 `Interactions`、`Documents`、`Zoom`、`PropertyDrawing`）；同一功能内的 Action、Menu、DragDrop、Runtime 与 Presentation 不得仅按类型角色机械拆成多个细碎目录。禁止使用含义模糊的 `Internal` 目录，访问级别由 C# 声明表达，不由目录名表达。
- Editor `.csproj` 的 `ProjectReference` 必须按公开 API 边界分组：第一个 `ItemGroup` 只放未出现在任何 public/protected API 中的实现依赖，并逐项设置 `PrivateAssets="compile"`；第二个 `ItemGroup` 只放公开签名、公开基类或公开接口实际泄漏的依赖。没有使用的引用应直接删除。
- `ProjectReference` 是否公开必须根据真实 API 签名判断，不能因为运行时会使用某程序集就默认向下游传递。调整公开类型、基类、参数、返回值或属性后，应同步复核引用分组。
<!-- /rule-unit: editor-projects -->

<!-- rule-unit: editor-state -->
<a id="editor-state"></a>
## 14. Editor History 与 Workspace 状态
- 可逆 Editor 数据修改统一进入 `EditorInteractions.history`；不要在 Panel 中维护第二套 Undo 栈，也不要把简单 `inverse Action` 当作通用模型。
- Feature Module 先完成领域修改，再用 `RecordApplied(name, EditorHistoryChange)` 记录；History payload 只能保存 stable protocol kind、persistent ID、Stable Type ID、路径、索引、标量和中立序列化 bytes，禁止捕获 runtime 对象、插件 `Type`、extension 实例或来自 collectible ALC 的委托。
- 每个 reload-safe 协议必须声明 `[EditorHistoryHandler(kind)]`。Handler 的 `Query` 只检查当前 generation 可用性；`Apply` 必须在失败时回滚本次部分修改。Handler Registry 与其他 Editor Registry 一起候选构建和原子切换。
- `RecordValue`、委托式 `Execute` 与派生 `EditorHistoryOperation` 只允许 Host-only 兼容流程；这些 runtime-bound entry 会在 extension generation 改变时截断，不得用于 EditorScripts 或 Scene/Asset 等长期记录。
- 稳定 `mergeKey` 只用于同一个逻辑值的连续输入；布尔开关、创建、删除和排序不得合并。多步骤修改使用 `BeginTransaction`，但每个 child 仍必须独立原子化。
- Undo/Redo 失败时必须保持操作位于原栈，禁止移动指针或覆盖新状态。新操作必须释放 Redo 分支；被淘汰或清除的 operation 必须释放其文件、对象或插件代际引用。
- 大 payload 使用 `EditorHistoryOptions` 自动溢出到 `<Project>/Library/Editor/History`；History 受 entry、resident bytes 与 disk bytes 三重预算限制，缓存不进入 `editor.ini`、Asset metadata 或 Scene 序列化。
- 保存、打开、选择等纯工作流操作默认不进入数据 Undo；它们只有在确实修改项目数据时才记录对应的数据部分。
- 跨启动的项目语义状态直接属于 Module/Panel；它们使用 Attribute 中必填、稳定且全局唯一的 ID，并通过 protected `Capture(EditorState)` / `Restore(EditorState)` hooks 参与持久化。扩展只调用参数对象的 `Get` / `Set`，不得在公开或 protected API 中暴露 JSON 实现。未 override Capture 的类型不进入状态 IO；不得恢复独立 Workspace interface、reader/writer 或第二个状态 ID。持久值只允许可重新解析的中立数据。
- `editor.ini` 是统一且可读的项目级 Editor settings 文档：标准 ImGui section 保存 layout；每个 Module/Panel 分别使用 `[InnoEditor][Module.<id>]` / `[InnoEditor][Panel.<id>]`；Panel 开关使用 `[InnoEditor][Panels]`。禁止用 Base64 或单一 opaque payload 包装全部 Workspace。Undo 栈、dirty Scene 内容、runtime 引用和编译中间态不得持久化。
- 新项目的默认 layout 由标准 Host 的嵌入资源提供，Core 只接收中立初始文档并复用 Core IO 原子写入。仅文件不存在时初始化，不能覆盖已有或刻意留空的 `editor.ini`；模板不得带入示例场景、Asset ID、插件状态、Selection、History 或临时弹窗。
- Editor Selection 是当前 session 的瞬时交互状态，不得写入 `editor.ini`。Workspace 可以保存可独立解释的导航位置、已打开文档和 active document，但启动后不得自动恢复 Asset、Scene、GameObject、Component 或 System selection。
- Editor 正常退出时必须先捕获全部有状态的 Module/Panel，再捕获最新 ImGui layout，最后在 Module 停止和 Scene 卸载前强制原子写入一次完整 `editor.ini`。运行期间仍可节流保存，但不能把它当作退出保存的替代品。
- Editor Scene 修改统一进入 `Inno.Editor.Scene.SceneEdits`。普通属性只保存单 property bytes；Component/System 保存 element identity/type/index/state；GameObject 删除保存最小 subtree；层级只保存受影响 placement；禁止为小修改序列化或恢复完整 Scene。
- Module/Panel 状态恢复必须容忍缺失 Asset、损坏 payload 和脚本类型尚未进入 TypeCache。候选未完整准备好前不得破坏当前可编辑状态。
- 有状态的 Module/Panel 必须先成功执行一次 protected `Restore`，之后才允许 `Capture` 覆盖磁盘 section。扩展 Registry 在启动或脚本激活期间可能重入刷新；恢复协调器必须按 Module/Panel 实例弱跟踪 `restoring/restored` 状态，禁止重入回调，也不能因为实例被新 snapshot 保留就误判其已经恢复。
- Scene Workspace 恢复时必须区分“源文件确实缺失”和“Asset Source Index 尚未完成首轮对账”。物理源仍存在时应保留 pending scene setup 并重试，不能用暂时为空的运行时 Scene 集合覆盖项目设置。Editor 允许没有任何已加载 Scene，不得为恢复、启动或删除最后一个 Scene 隐式创建 Untitled Scene。
<!-- /rule-unit: editor-state -->

<!-- rule-unit: editor-layout -->
<a id="editor-layout"></a>
## 24. Editor 表单与滚动约束

- 每个可视区域只能指定一个滚动 owner。父 Panel/Modal、布局 Child 与内容 Child 不得同时为同一方向生成滚动条；承载填满区域的可滚动 Child 时，父容器必须禁用对应滚动。
- 所有左侧 label、右侧 input 的 Editor 字段复用 `ImGuiWidget.SetupPropertyColumns()` 与 `PropertyLabel()`，按 2:3 分配可用宽度；有操作列时先扣除操作列。label 必须在自身列换行，行高随内容增长。
- 禁止以固定 label 像素宽度、无条件 scrollbar flag、虚假的 content size 或额外嵌套滚动 Child 修补布局。修改表单、Panel、Popup 后要在小窗口、长文本与不同 zoom 下验证 `ScrollMaxX/ScrollMaxY` 只在内容确实溢出时为正。
- Editor 下拉选择器统一使用 `BeginBoundedCombo` / `EndBoundedCombo` 或 `BeginMenuSelector` / `EndMenuSelector`；弹层从触发控件下边缘向下展开，宽度受所属窗口限制，高度同时受所属窗口剩余空间和窗口高度比例限制。不要直接用原生 Combo 的自动翻转定位。
- Popup 尺寸约束统一在 Widget 边界做像素对齐：上限向下取整，内容所需尺寸向上取整，防止 ImGui 截断小数 padding 后产生假溢出。回归必须覆盖 85%、90%、110% 等非整像素缩放；不能只测试整数或半整数 padding，也不能用 `NoScrollbar` 隐藏问题。
- 自绘命中区域除了矩形包含关系，还必须确认所属 ImGui window 是当前可交互的前景窗口；被浮动窗口、Popup 或 Modal 遮挡时不得触发底层 Panel 的关闭、画布手势或游戏输入。平台事件仍使用 `Inno.Core.Events`，ImGui 只决定可见 UI 的命中与焦点；Play Session 使用独立 Input backend 接收经 Game View 焦点策略筛选的事件，不建立第二个事件总线。
- 树行的 disclosure、正文与叠加按钮必须各自拥有独立命中区域；原生 TreeNode 不得以整行 SpanFullWidth/AllowOverlap 覆盖自绘正文。回归同时覆盖鼠标先悬停与移动/按下发生在同一帧、正文双击、箭头展开及叠加按钮，不能依赖额外一帧悬停才能点击。
<!-- /rule-unit: editor-layout -->

<!-- rule-unit: editor-acceptance -->
<a id="editor-acceptance"></a>
## 32. Editor UI ownership and acceptance

- Domain editing in `features` must remain independent from native UI, Panels and Workbench. Panels compose feature APIs; they do not reference other Panels. Reusable controls belong to the session-owned ImGui adapter, and generic Graph presentation cannot depend on Shader semantics.
- Every interactive host owns one `EditorUiSession`. Transient widget/menu state belongs to its session and stable presentation owner. Native drawing borrows cannot survive their frame, window, context or owner thread. `current` is only a synchronous scoped accessor, never a global state owner.
- Continuous Inspector and viewport edits use one `EditorEditGesture` over `EditorInteractions.history`. Each sample records immediately; Escape rolls back the same transaction. Save, commands and generation changes finish the gesture before their own operation. No feature may delay History publication behind a separate UI-active flag.
- Selection, deferred commands and cross-frame tools resolve identities through Core Identity. Batch operations validate the complete selection, normalize overlapping roots and roll back atomically. Mixed, Missing, read-only and unassigned states remain distinct.
- Final delivery must evaluate architecture, organization, state/lifetime cleanliness, abstraction, reuse, visual consistency, extensibility, interaction, Undo/Redo, menu clarity and component granularity individually. Each evaluation must identify actual evidence, limitations and remaining improvements; directory changes and interface counts alone are insufficient.
- Reassess completion against the original approved plan, including every functional and quantitative acceptance requirement. Separate missing implementation from missing evidence; green CI and focused benchmarks cannot establish untested scale, visual, reload or lifecycle guarantees. Correct stale completion statements and issue ledgers when later review finds a gap.
- UI-neutral name formatting and Action/Menu queries must work without a native context or drawing session. Keep pure label formatting in Editor Core; only drawing and native borrows require an active UI scope.
- Temporary presentation worlds register their borrowed Core allocator through the existing selection coordinator's routing lease. Restore Edit selection before releasing that lease; failed retirement must preserve its owner. The routing table never becomes a second live-object authority.
- Boxed value types are not automatically neutral: a host struct may contain extension objects. Cross-callback targets retain only known neutral scalars strongly; all other non-identity objects use weak references, including boxed structs. Selection, focus and snapshots must not prolong their referenced objects' lifetime.
- The formal script SDK host references the actual exported API owners directly, including reusable presentation libraries when required. It must not use concrete Panels as transitive dependency carriers for asset, shader or graph authoring APIs.
- Session-owned zoom and density requests commit through the presentation preparation phase after selecting the owned context and display metrics, before native frame creation. Repeated preparation during the same drawing frame cannot publish a new metric snapshot. Standalone metrics remain usable without native UI.
- Presentation preparation, drawing and publication form one owned operation. Reentry and retirement during that operation must fail before releasing resources. A callback failure ends only its owned native frame, preserves the original failure and never publishes partial draw data; it must not clear another context's stacks or lower native assertions.
- Native presentation fixtures must create and enter an `EditorUiSession`, explicitly compose their feature/presentation owners, and resolve the exact service under test. Asynchronous shutdown fixtures must retry the public Pending retirement protocol until owned cleanup completes; they cannot rely on continuation timing or remove resource-release assertions.
- Presentation input snapshots expose physical Core modifier semantics. Reverse native shortcut normalization at the presentation boundary using the owned context configuration; do not interpret normalized Control/Super flags as physical keys or add shared-layer OS detection. Native input fixtures explicitly select their conventions and cover both modifier policies and native modes.

- Content-sized modals prepare one coherent frame snapshot and measure it before opening. Explicitly sized resizable windows prepare after current native input establishes their content bounds. Never invoke arbitrary drawing callbacks twice for measurement or rely on the previous message height.
- Shared dialog widgets own text wrapping, progress, bounded body scrolling and action spacing. Domain callers consume those controls and shared metrics; they must not compensate with empty lines, arbitrary padding or a second scroll owner. Escape action behavior is explicit.
- Workflow progress and cancellation are bound to the original owner and operation identity. Late callbacks cannot overwrite a newer request or a cancellation state, and progress presentation cannot weaken publication or generation retirement.

- CI and local acceptance must follow the actual changed owners. Use scoped Editor verification for bounded UI changes; retain full verification for build, platform, backend, SDK, shared runtime or unknown owners. Record omitted checks explicitly and never bypass production validation to shorten CI.
<!-- /rule-unit: editor-acceptance -->

<!-- rule-unit: editor-standard-9 -->
<a id="editor-standard-9"></a>
## 交付与验收

变更必须同步源码、调用方、项目引用、公开 XML、Wiki 和适用测试。使用真实公开契约与可替换边界，不使用 IVT、私有反射或测试后门。

Build 入口的统一 `validate` 计划编排既有 xUnit/VSTest 和真实产品旅程，分别记录领域、公共契约、集成、Native、产品与性能证据。契约、工作流、视觉和生命周期断言随实际依赖选择，不按多个 profile 重复构建同一工程。每次保留 SDK、源码身份、TRX 和结果摘要；空发现或隔离进程零执行不能视为成功。产品验收使用实际平台声明的 renderer 与启动期望；共享入口不内嵌 backend 名称作为默认断言。稳定的 `--artifacts-path` 与独立 `--output` 分别指定构建 owner 和运行日志。隔离入口的 `-TemporaryDirectory` 可显式指定较短临时根；默认保留操作系统 TEMP/TMP，不隐式嵌套到深层结果目录，普通与隔离进程使用同一临时 owner。

Settings 的一次 Apply 复用单一 History transaction，所有 dirty owner 成功后才接受新基线。失败先尝试回滚并保留草稿；可恢复的文件/校验错误在窗口内反馈并记录完整诊断。History 完整性丢失后禁止继续 Apply，不能把错误吞掉后重试。打开的设置窗口借用当前 generation 的编辑对象，使用现有 admission guard 延迟 reload，关闭后恢复许可；不为此建立另一套状态恢复或 History 系统。

验收编排从正式 Editor 入口取得实际 Native target、tool target 和产品属性，不全局覆盖 `InnoTestNativeProduct`。需要原生部署的测试工程声明自己的产品及目标；纯契约测试不会因传递引用包含 Native 程序集而自动承担完整产品准备。普通执行、隔离发现和隔离执行使用相同的 SDK 输入。

原生借用验证与纯值转换必须按实际契约区分：`EditorDrawList` 每次操作验证原窗口、帧、Context 和线程；颜色转换保留既有 Session／Context／线程准入。颜色格式的饱和、取整及非有限值行为通过实际原生 oracle 验证，不用普通数值相等推断所有边界一致。

`EditorUiInput` 的 Control/Super 表示物理修饰键，不继承原生控件内部的快捷键归一化。呈现边界依据当前 Context 的键盘配置还原这些值，列表选择复用同一快照语义，平台仍通过 `EditorKeyboardPolicy` 选择 primary modifier。原生文本控件继续使用该 Context 的输入习惯；测试必须显式声明自己的键盘配置，并覆盖 Control/Super 与两种原生模式的组合，不能依赖 runner 的默认值。

最终报告分别评价架构、组织、状态与生命周期、抽象、复用、样式、扩展、交互、Undo/Redo、菜单和颗粒度；每项列出证据与限制。目录整齐、测试数量或正常启动不能代替视觉、输入、重复 reload、资源收敛和成对性能验收。
<!-- /rule-unit: editor-standard-9 -->

<!-- rule-unit: editor-standard-10 -->
<a id="editor-standard-10"></a>
## 动态 Modal 的状态与尺寸

- 窗口尺寸、换行和绘制使用本帧同一快照，不依赖上一条消息的高度。
- 准备只读取和测量；实际动作由原领域 owner 校验 operation 后执行。禁止双次绘制回调测量。
- 统一 metrics 表达 padding、item spacing、按钮高度和字体尺度。业务 Modal 不通过额外空行、固定大高度或零散数字补偿布局。
- 消息与详情共用一个正文滚动 owner，操作行保持可达。Child 不重复承担父窗口已经提供的外层 padding。
- 明确区分声明尺寸、自适应尺寸及用户调整尺寸；部分指定尺寸也必须得到正确解释。
- 当前帧内容增长、缩短、空文本、动作变化及非整数尺度都属于必要回归。

### 按职责选择 CI 验收

Engine CI 根据完整 Git 变更集合及实际 SDK 依赖选择检查，不能按 issue 编号或为了节省时间临时跳过失败项。Editor 领域、呈现、工作流与生命周期按执行前提隔离；所选产品保留真实原生 smoke 和产品旅程。Release、NativeAOT 还原、Player Support Pack 与完整功能矩阵属于显式完整验收；build、native、reload 等相关变更提前选择相应深层检查，周验收增加冷构建、十轮热缓存和长期运行。未知 owner 或无法推导的共同输入必须产生可见诊断并保守扩大范围，手动工作流支持完整验收。

`Inno.Tooling.Validation` 是唯一计划 owner，不改变生产构建的完整性要求。移动文件同时覆盖旧、新 owner，PR 使用实际合并候选与 merge base，本地包含 staged、unstaged、untracked；选择理由与排除项写入计划。有限范围通过不等于完整产品或设备矩阵通过；必要环境不可用、缺少结果或必要测试为空必须失败或阻塞。

廉价架构规则保持全仓执行；编译元数据和 Wiki 示例按真实依赖准备，批量图由架构、测试和文档共同消费。项目引用、imports、源码、资源和生成输入变化均进入影响分析，SDK、TFM 与共同构建机制不能按文本相似度判为无影响。日志目录、报告、commit/push 和会话恢复不是重复验收的依据；仅输入、工具、环境与产物一致的确定性证据可以复用。
<!-- /rule-unit: editor-standard-10 -->
