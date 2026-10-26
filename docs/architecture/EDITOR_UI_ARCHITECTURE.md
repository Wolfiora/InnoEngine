# Editor UI 架构与执行边界

[架构索引](ENGINE_ARCHITECTURE_OVERVIEW.md) · [Editor Wiki](../editor/README.md)

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
│   │   ├── Controls/                       # Pointer-free facade and borrows
│   │   ├── Widgets/                        # Session-owned controls
│   │   ├── Menus/                          # Unified models to ImGui
│   │   └── Styling/                          # Palette and density metrics
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

连续输入在首个有效 sample 打开 EditorEditGesture，release 提交，Escape 回滚。显式 Selection 切换先提交当前输入，准备失败则保持原选择；关闭 owner 或未完成的控件消失时回滚。命令和 reload 边界同样使用已有 Core Events 准备通知。材质/Pipeline 不再自己维护 deferred gesture；SceneEdits 和 asset draft service 都立即记录。

多选包含有序身份和 anchor；visible range 来自同一过滤/排序结果。批量源操作归一化最小 roots，先验证所有 owner，再由既有 History transaction 执行；失败不接受半批结果。共同属性使用 owner serialization context，保持各对象独立的 before 值。

## 复用与颗粒度

复用 Core Events、Identity、Serialization、AtomicFile 与现有 Graph/History；未新增平行事件总线、Undo 栈、identity registry 或依赖注入框架。Graph.ImGui 只提取实际共用的坐标、网格、曲线及命中机制；Shader 的 ports、参数与诊断仍由 Shader owner 解释。

ShaderEditor 是最大的领域专用 view，其 canvas、预览、菜单和文档协调按功能文件组织。该部分仍有降低复杂度的空间；不通过新增空 interface/project 或把全部方法变 public 来伪装解耦。架构评价和实际验收见本轮交付报告。

## 自动约束

EditorUiBoundaryValidator 校验 feature 无 UI/native/Panel 引用、Panel 无相互引用、native 访问集中在 adapter、Graph 无 Rendering/Shader 依赖。公开 API 依赖分组、XML、逻辑脚本导出、Solution 和 Wiki 继续由仓库统一验证器验证。


## 精确多选分量与 History 查询

属性编辑传递 root member、当前手势 key 和可选 scalar component。向量、整数向量及矩形控件明确传递被编辑的公共字段；Euler 控件声明具体轴。SceneEdits 使用当前公开值字段或 Core Quaternion 运算组合目标值，未读取私有字段，也不缓存插件 metadata。持久 History 继续保存既有 root property bytes，未增加第二种序列化格式。

History Query 只判断当前 owner/handler 是否可用。草稿的 expected bytes 在实际 Apply 时检查，避免同一事务较早 sample 因当前值已前进而使整个 Undo 永久禁用。Apply 冲突仍明确失败并保留栈位置。


## 临时运行世界的 Identity 路由

启动时登记 Asset 与 Edit allocator；Play 的独立 allocator 由 Scene workspace 在准备候选前取得 `RegisterIdentityDomain` 路由 lease。Selection、focus、queued Action 与 native DragDrop 共同读取这一域路由表，实际对象始终由 Core Identity 解析。返回 Edit 并恢复选择成功后再释放 lease，失败保留可重试的 owner；不登记全局 allocator，也不保存第二张 live object 表。

`IEditorSelectionCoordinator` 是 Host 组合协议，承担临时 presentation 的域所有权，不再导出给项目脚本。脚本继续通过 `EditorInteractions` 使用选择和交互，不负责创建或登记 allocator。
