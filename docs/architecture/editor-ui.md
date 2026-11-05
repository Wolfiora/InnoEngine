# Editor UI 架构与执行边界

[架构索引](overview.md) · [Editor Wiki](../modules/editor/README.md)

## 当前目录

```text
src/composition/editor/
├── contracts/Inno.Editor.Annotations/        # Stable discovery metadata
├── framework/
│   ├── Inno.Editor.Core/                    # Context, Module, Panel, state
│   ├── Inno.Editor.Interactions/
│   │   ├── Actions/                         # Availability and fresh dispatch
│   │   ├── Selection/                       # Ordered Core Identity references
│   │   ├── Editing/                         # One shared History gesture
│   │   ├── History/                         # Neutral records and atomic transactions
│   │   ├── Documents/                       # Save/Revert/Apply/Close ownership
│   │   ├── Menus/                           # Action-derived menu models
│   │   └── Viewport/                        # Generic interaction tools
│   ├── Inno.Editor.Inspection/              # Registries and serialized controls
│   ├── Inno.Editor.Graph/                   # Graph document/state/history
│   └── Inno.Editor.Settings/                # Structured settings and Core Events
├── features/
│   ├── Inno.Editor.Assets/
│   │   ├── Editing/                         # Reusable source mutations
│   │   ├── History/                         # Source archives and neutral records
│   │   ├── Documents/                       # Immediate draft history/recovery
│   │   └── Samples/                         # Formal Sample import
│   ├── Inno.Editor.Scene/Documents/         # Granular SceneEdits and batch operations
│   ├── Inno.Editor.Shaders/Materials/       # Material drafts and source presentation
│   ├── Inno.Editor.Rendering/Pipelines/     # Pipeline source drafts
│   ├── Inno.Editor.Scripting/Runtime/       # Compilation and retirement; no UI
│   └── Inno.Editor.Exporting/Runtime/       # Export ownership; no UI
├── presentation/
│   ├── Inno.Editor.ImGui/
│   │   ├── Runtime/                        # Session, frame and owner lifetime
│   │   │   ├── EditorUiSession.cs
│   │   │   └── EditorMenuRenderer.cs        # Unified models to ImGui
│   │   ├── Controls/                       # Pointer-free facade and borrows
│   │   ├── Widgets/                        # Session-owned controls
│   │   └── Styling/                        # Palette and density metrics
│   ├── Inno.Editor.Assets.ImGui/            # Browser state, icons, basic Inspector
│   ├── Inno.Editor.Shaders.ImGui/           # Material and node presentation
│   ├── Inno.Editor.Graph.ImGui/             # Generic canvas geometry and hit tests
│   └── Inno.Editor.Workbench/               # Global commands, settings, export UI
├── panels/                                 # Concrete views, no Panel references
└── hosting/Inno.Editor.Hosting/             # Runtime composition and lifetime
```

## 真正的依赖与执行链

领域操作和 Undo payload 不包含原生 UI 类型。Panel 读取当前 owner/context，调用 feature API；feature 先验证并执行修改，然后向同一个 History 发布中立记录。ImGui adapter 负责 native context、ID stack、绘制借用和 transient 状态。宿主负责初始化顺序与候选切换。

普通菜单、快捷键、工具栏与 command search 消费同一 Action 的 Query/Execute；presentation 不能维护另一份 enabled/checked 业务规则。Action 索引按 active generation 失效。所有跨帧目标用 Core Identity 重新解析，不能保留 Component wrapper 或 collectible Type。

静态菜单路径和结构在当前 generation 内冻结；快捷键按按键、规范化修饰键及 area 索引候选。缓存只保存声明事实，菜单状态和动态 source 仍按请求查询。动态菜单采用局部不可变结构合并，不把请求参数保存到静态模板。候选完成和 shutdown 明确清空索引；此处是内部执行机制，不增加项目作者配置。

非 Identity target 只有明确的中立标量可以强持有，其余对象使用弱引用，包括 boxed struct。宿主程序集的值类型仍可能包含插件对象，不能根据 `IsValueType` 或程序集不可回收就假定其内容中立。Selection、focus 与 snapshot 复用同一规则。

连续输入在首个有效 sample 打开 EditorEditGesture，release 提交，Escape 回滚。显式 Selection 切换先提交当前输入，准备失败则保持原选择；关闭 owner 或未完成的控件消失时回滚。命令和 reload 边界同样使用已有 Core Events 准备通知。材质/Pipeline 不再自己维护 deferred gesture；SceneEdits 和 asset draft service 都立即记录。

多选包含有序身份和 anchor；visible range 来自同一过滤/排序结果。批量源操作归一化最小 roots，先验证所有 owner，再由既有 History transaction 执行；失败不接受半批结果。共同属性使用 owner serialization context，保持各对象独立的 before 值。

## 复用与颗粒度

复用 Core Events、Identity、Serialization、AtomicFile 与现有 Graph/History；未新增平行事件总线、Undo 栈、identity registry 或依赖注入框架。Graph.ImGui 只提取实际共用的坐标、网格、曲线及命中机制；Shader 的 ports、参数与诊断仍由 Shader owner 解释。

ShaderEditor 是最大的领域专用 view，其 canvas、预览、菜单和文档协调按功能文件组织。该部分仍有降低复杂度的空间；不通过新增空 interface/project 或把全部方法变 public 来伪装解耦。当前证据范围见 [验收记录](../../.agents/records/acceptance/current.md)。

## 自动约束

EditorUiBoundaryValidator 校验 feature 无 UI/native/Panel 引用、Panel 无相互引用、native 访问集中在 adapter、Graph 无 Rendering/Shader 依赖。公开 API 依赖分组、XML、逻辑脚本导出、Solution 和 Wiki 继续由仓库统一验证器验证。

正式脚本 SDK 的 Build CLI 直接引用 Assets、Shaders、Graph 等导出 API 的实际 owner，不通过 FileBrowser、ShaderEditor、Hierarchy 或 Inspector Panel 间接携带编译 API。CLI 的这项引用边界同样受正反例约束。

Hierarchy 与 Inspector 自己公开的 drop target 协议仍由它们各自声明并被 SDK 直接引用；这些是明确的 UI 扩展契约，不是其他模块 API 的传递载体。校验器解析真实的 assembly export 声明，空清单或注释中的声明都不能充当 owner 证明。


## 精确多选分量与 History 查询

属性编辑传递 root member、当前手势 key 和可选 scalar component。向量、整数向量及矩形控件明确传递被编辑的公共字段；Euler 控件声明具体轴。SceneEdits 使用当前公开值字段或 Core Quaternion 运算组合目标值，未读取私有字段，也不缓存插件 metadata。持久 History 继续保存既有 root property bytes，未增加第二种序列化格式。

History Query 只判断当前 owner/handler 是否可用。草稿的 expected bytes 在实际 Apply 时检查，避免同一事务较早 sample 因当前值已前进而使整个 Undo 永久禁用。Apply 冲突仍明确失败并保留栈位置。


## 临时运行世界的 Identity 路由

启动时登记 Asset 与 Edit allocator；Play 的独立 allocator 由 Scene workspace 在准备候选前取得 `RegisterIdentityDomain` 路由 lease。Selection、focus、queued Action 与 native DragDrop 共同读取这一域路由表，实际对象始终由 Core Identity 解析。返回 Edit 并恢复选择成功后再释放 lease，失败保留可重试的 owner；不登记全局 allocator，也不保存第二张 live object 表。

`IEditorSelectionCoordinator` 是 Host 组合协议，承担临时 presentation 的域所有权，不再导出给项目脚本。脚本继续通过 `EditorInteractions` 使用选择和交互，不负责创建或登记 allocator。

# Editor UI 设计与实现规范

[Wiki 首页](../README.md) · [Editor 架构](editor-ui.md) · [Identity / Reference / Reload](identity-and-reload.md)

本页定义当前 Editor 的交互和实现契约。测试结果及未完成项属于验收报告；本页不以规范代替验收证据。

## 首次打开项目

标准 Host 为缺少 `editor.ini` 的项目提供通用停靠布局，并在加载 presentation 前原子创建文档。布局资源由 Host 组合层拥有；Core 只处理传入的中立文档与现有状态协议。已有布局（包括刻意留空的文件）不能被默认值覆盖。默认资源不得带入示例场景、Asset ID、插件状态、Selection、History 或临时弹窗；后续保存复用统一文档生命周期。

## 职责与依赖

| Owner | 职责 | 禁止承担的职责 |
| --- | --- | --- |
| Core | Identity、Events、IO、序列化等基础机制 | 具体 Panel、ImGui 或领域编辑 |
| Editor Interactions | Action、菜单模型、选择、手势、History、文档协议 | Native 绘制、具体领域数据模型 |
| Feature | 原子领域修改、验证、恢复与 History payload | 停靠布局、Native 控件、另一个 Panel 的状态 |
| ImGui presentation | Session、作用域、控件、输入路由和通用样式 | 领域持久化、第二套事件总线或 Undo 栈 |
| Domain presentation | Browser、Shader 等领域视觉与组合 | 修改底层机制以维护领域类型白名单 |
| Panel | 组合已有 Feature 和 presentation | 独立资产数据库、编译流水线或通用状态管理器 |

新增扩展先确定程序集、所有权和失败边界，再实现控件。每个项目采用项目级命名空间；Widgets 只在对应 `ImGuiWidget.<Feature>.cs` 中组织紧密相关的类型。公开 API 必须有实际外部消费者，内部缓存不得因测试方便而公开。

## 样式、尺寸与布局

样式分为基础语义值、通用组件值和领域呈现值。通用层表达文本、边框、焦点、禁用和内容强调，不认识 Shader 节点类别或 Asset 类型。Browser 布局属于 Assets presentation；Shader 端口和节点类别属于 Shader presentation。

Console、Hierarchy 和 Settings 的专属颜色与尺寸由各自 Panel 的内部 `Styling` 类型维护，Tag／Layer 颜色属于 Inspector 的内部呈现。公共 `EditorPalette` 与 `EditorStyleMetrics` 只保留基础值和可复用的卡片、标题、分区、表单等组件值；这些内部领域样式不进入脚本 API，也不保存 Session、Type 或扩展实例。

- DPI 表达显示设备密度，Zoom 表达用户的逻辑尺寸偏好；不能把同一尺寸重复缩放。
- 后端先选择所属 Context 并更新显示 metrics，再执行 preparation callback，最后开始 Native frame。Session-owned Zoom／Density 在准备阶段统一生效；绘制中请求修改只影响后续帧。单独创建的 metrics 仍可作为无 Native 的即时值模型使用。
- 表单先扣除操作列，再按标签与输入约 2:3 分配空间。长标签和窄 Inspector 使用裁剪、换行或 Tooltip，不能遮挡输入。
- 同类按钮、搜索、树行、菜单、表格、卡片和 Modal 使用同一套通用 metrics。
- 每个内容区只有一个主要滚动 owner。下拉框和菜单在当前 viewport 工作区内有界滚动。
- 纯图标操作必须提供明确 Tooltip；错误、Missing、只读与禁用状态不能只依赖颜色。
- 空内容说明下一步操作；忙碌状态说明当前工作；失败保留具体诊断和可执行的恢复路径。

尺度契约测试与物理显示器测试分别记录。模拟 framebuffer scale 不能代表已在所有真实 DPI 设备验收。

## Action、菜单、工具栏与快捷键

动作的唯一业务入口是现有 Action 协议。主菜单、上下文菜单、命令搜索、工具栏和快捷键查询相同的可用性、checked 状态和禁用原因。

带目标的 `EditorInteraction` 只用于当前同步请求，并在请求内强持有目标；不能把它缓存到后续帧或 generation。长期焦点与延迟路由另由既有 Interactions 使用 Core Identity 或弱引用管理。Hierarchy／Inspector 的公开 drop target 也是请求级借用，不属于 History payload 或持久状态。

静态声明的 Action ID、area、路径、顺序和快捷键按 extension generation 建索引。Selection、Play、Missing、只读状态及动态菜单参数必须来自当前请求，不能进入 generation 长寿命缓存。候选完整成功后原子替换；失败保留 last-good。

菜单路径表达用户任务，显示名称不承担持久身份。排序稳定、分组清晰，没有空组、重复动作或多余分隔线。上下文菜单的目标必须与当前选中或右击的记录一致；目标消失时关闭菜单，不能继续操作旧对象。

文本框、Popup 和 Modal 占有输入时，不向底层列表派发编辑键。保存等明确工作流仍遵守现有输入策略，不能以每个 Panel 自行判断快捷键代替统一路由。

## 选择、焦点与键盘导航

焦点、Selection、激活和数据修改是四种不同操作。跨帧定位对象必须重新经过 Core Identity；列表缓存只保留中立显示数据和 ID。

| 输入 | 行为 |
| --- | --- |
| Up / Down | 在当前可见序列移动 |
| Home / End | 移到首尾 |
| PageUp / PageDown | 按当前可见页移动 |
| Shift | 从 anchor 扩展范围 |
| 平台主修饰键 | 遵守产品注入的键盘策略 |
| Enter | 激活当前条目 |
| Escape | 退出搜索或取消当前交互 |

过滤、排序、展开、鼠标范围选择与键盘范围选择必须使用同一序列。隐藏项不能被误选，裁剪不能丢失重命名、拖动、anchor 或焦点。安装 Plugin 内容保持只读，不能因为换了一种输入方式而绕过领域权限。

File Browser 的树、列表与网格根据当前键盘焦点选择重命名所在区域；不能要求先点击条目，也不能把输入框放到上次使用的另一种呈现中。它们复用同一个重命名 Action 与 History，领域修改保持不变。

Console 搜索对 message、source、file 及已有诊断文本执行不区分大小写的普通子串匹配，并与级别过滤组合。Collapse、详情、复制和选择必须对应相同的匹配 occurrences。Clear 清除派生选择及展开状态，不建立第二份日志存储。

## 编辑、History 与文档

可逆数据修改统一进入 `EditorInteractions.history`。Feature 先完成原子领域修改，再记录中立 payload。单次连续拖动产生一条逻辑 Undo；两次独立拖动产生两条。布尔、创建、删除和排序不能使用连续值 mergeKey 合并。

Escape 回滚当前手势；命令、保存、Play、reload 和关闭文档先完成或取消相应手势，再进入自己的操作。批量中途失败必须恢复所有已修改内容。回滚失败保留 payload、owner 和诊断，不能提前释放后宣称成功。

Missing 形成可恢复 barrier，不移动 Undo/Redo 栈，不丢原始 bytes；恢复后原操作重新可用。保存、打开、纯选择不属于数据 Undo。文档 dirty、保存、放弃、关闭与恢复通过统一文档协议完成；不能用 Panel 局部标志隐藏尚未发布的领域修改。

## Graph 与大视图

通用 Graph 文档不依赖 Rendering 或 ImGui。Graph ImGui 只负责节点几何、命中、框选、移动预览和连接意图；Shader owner 解释端口、节点、参数、编译与诊断。呈现层不能直接代替领域 controller 修改文档。

几何缓存只保存 ID 和中立值，其有效性由文档、revision、generation 和尺度决定。平移通过坐标变换处理；局部节点变化只失效相关几何。自定义内容回调只属于当前绘制作用域，不进入长期缓存。

固定行高列表按可见行提交；树按 revision、展开及过滤建立扁平可见序列。Console 对可变高度内容缓存测量结果，宽度、尺度、内容和展开变化时失效。原生子窗口及上下文菜单复用有限的呈现槽位，不能用无限增长的记录 ID 创建历史窗口。

优化先定义负载和采样方法。提交项数量、分配、CPU、GC、frame median/P95 与功能结果一起验收；不能只展示 FPS，不能用降低正确性换取指标。

## 生命周期与异常

每个交互 Host 拥有一个 `EditorUiSession`。`current` 是同步作用域访问器；状态仍属于具体 Session 和 presentation owner。Native 借用不能越过 frame、window、context 或 owner thread。

Begin/End、Push/Pop、Child、Table、Popup、Clip 和样式遵守各自真实配对规则，并用局部 `try/finally` 释放本次所有权。扩展异常保留原始诊断，后续合法帧必须能够继续绘制。不能清空其他 owner 的状态来恢复当前 Panel。

循环行的 ID 生命周期必须覆盖异常、提前退出和普通完成；移除条目后的 break 也通过同一 finally 释放，不在分支中手工重复 PopId。架构源码门禁检查这一已确认的回归模式，并检查重复呈现别名及失效 XML 说明；真实绘制、失效借用和跨 owner 异常仍由各自 Native 契约验收。

Presentation 的准备、绘制与发布属于同一次 owner 操作；期间拒绝重入和退休。准备失败不创建 Native frame；绘制失败只结束所属 Context 的失败帧，不发布部分 draw data。调用方仍须平衡自己打开的窗口和栈，后端不伪造修复其他 owner 的作用域。

关闭 Panel 退休其暂态控件；关闭文档退休文档状态。generation 切换不能保留旧 Type、delegate、扩展实例或 collectible 引用。旧 ALC 退休严格执行既有 GC/finalizer/weak-monitor barrier；timeout 进入 Faulted，不能清空 monitor 继续。

`editor.ini` 保存可读布局及中立项目导航，不保存 Selection、Undo、runtime 引用或编译中间态。正常退出先捕获 Module/Panel，再捕获最新布局，最后原子写入。
