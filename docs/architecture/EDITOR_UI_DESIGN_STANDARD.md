# Editor UI 设计与实现规范

[Wiki 首页](../README.md) · [Editor 架构](EDITOR_UI_ARCHITECTURE.md) · [Identity / Reference / Reload](IDENTITY_REFERENCE_RELOAD_STANDARD.md)

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

## 交付与验收

变更必须同步源码、调用方、项目引用、公开 XML、Wiki 和适用测试。使用真实公开契约与可替换边界，不使用 IVT、私有反射或测试后门。

`tests/Invoke-EditorUiAcceptance.ps1` 编排既有 xUnit/VSTest，按 Contracts、Workflows、Visual、Lifecycle、Performance、Products 分组。每次保留独立 SDK、源码身份、TRX 和结果摘要；空发现不能视为成功。Products 必须显式提供平台产品的 `-GraphicsApis` 和启动日志期望 `-BackendPattern`；共享入口不内嵌 backend 名称作为默认断言。`-ArtifactsPath` 与 `-ResultsDirectory` 指定输出 owner；需要把大量临时文件放到其他卷时，使用 `-TemporaryDirectory` 明确指定较短的临时根。默认保留操作系统的 TEMP/TMP，不把它们隐式嵌套到深层结果目录。该参数同样传递给隔离测试入口，普通测试与独立进程使用同一临时 owner。

Settings 的一次 Apply 复用单一 History transaction，所有 dirty owner 成功后才接受新基线。失败先尝试回滚并保留草稿；可恢复的文件/校验错误在窗口内反馈并记录完整诊断。History 完整性丢失后禁止继续 Apply，不能把错误吞掉后重试。打开的设置窗口借用当前 generation 的编辑对象，使用现有 admission guard 延迟 reload，关闭后恢复许可；不为此建立另一套状态恢复或 History 系统。

验收编排从正式 Editor 入口取得实际 Native target、tool target 和产品属性，不全局覆盖 `InnoTestNativeProduct`。需要原生部署的测试工程声明自己的产品及目标；纯契约测试不会因传递引用包含 Native 程序集而自动承担完整产品准备。普通执行、隔离发现和隔离执行使用相同的 SDK 输入。

原生借用验证与纯值转换必须按实际契约区分：`EditorDrawList` 每次操作验证原窗口、帧、Context 和线程；颜色转换保留既有 Session／Context／线程准入。颜色格式的饱和、取整及非有限值行为通过实际原生 oracle 验证，不用普通数值相等推断所有边界一致。

`EditorUiInput` 的 Control/Super 表示物理修饰键，不继承原生控件内部的快捷键归一化。呈现边界依据当前 Context 的键盘配置还原这些值，列表选择复用同一快照语义，平台仍通过 `EditorKeyboardPolicy` 选择 primary modifier。原生文本控件继续使用该 Context 的输入习惯；测试必须显式声明自己的键盘配置，并覆盖 Control/Super 与两种原生模式的组合，不能依赖 runner 的默认值。

最终报告分别评价架构、组织、状态与生命周期、抽象、复用、样式、扩展、交互、Undo/Redo、菜单和颗粒度；每项列出证据与限制。目录整齐、测试数量或正常启动不能代替视觉、输入、重复 reload、资源收敛和成对性能验收。

## 动态 Modal 的状态与尺寸

- 窗口尺寸、换行和绘制使用本帧同一快照，不依赖上一条消息的高度。
- 准备只读取和测量；实际动作由原领域 owner 校验 operation 后执行。禁止双次绘制回调测量。
- 统一 metrics 表达 padding、item spacing、按钮高度和字体尺度。业务 Modal 不通过额外空行、固定大高度或零散数字补偿布局。
- 消息与详情共用一个正文滚动 owner，操作行保持可达。Child 不重复承担父窗口已经提供的外层 padding。
- 明确区分声明尺寸、自适应尺寸及用户调整尺寸；部分指定尺寸也必须得到正确解释。
- 当前帧内容增长、缩短、空文本、动作变化及非整数尺度都属于必要回归。

### 按职责选择 CI 验收

Engine CI 根据完整 Git 变更集合选择检查，不能按 issue 编号或为了节省时间临时跳过失败项。仅涉及 Editor 创作层（包括脚本 reload owner）及对应测试、文档时使用 Editor 范围：Debug Editor、真实原生 smoke、Editor 契约与工作流、架构和 Wiki 检查。Release、NativeAOT 还原、十次热构建、其他领域完整测试及 Player Support Pack 留给完整范围。SDK、工程图、build、platform、backend、共享 Runtime 或未知 owner 的变化保守选择完整范围。手动工作流仍执行完整范围。

脚本 `tests/ci/Select-ValidationScope.ps1` 只编排验收，不改变生产构建或缓存校验。移动文件同时核对旧 owner 和新 owner；选择理由与省略项写入 GitHub job summary。Editor 范围不等于完整产品矩阵通过；完整源码和 Player contract 编译仍服务于当前架构/Wiki 的真实引用验证，不执行 Player 导出或 Support Pack 生产。

UI 范围的生产源码检查由当前 Solution 生成 169 个生产工程的 `.slnf`，引用闭包仍由 MSBuild 求值，不编译全部无关测试工程。只有通过完整 Git 差异证明为标准 SDK 工程中、相同无条件引用边的 `PrivateAssets=compile` 收紧时，才允许保留 Editor 范围；新增引用、包、TFM、SDK、条件或构建目标变化仍走完整范围。PR 使用 merge base，push 使用 before/head，避免把目标分支的无关改动混进 PR 检查。
