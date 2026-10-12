# Inno.Editor.Panel.FileBrowser

[Editor 索引](README.md) · [Assets](../assets/README.md) · [Hierarchy](Inno.Editor.Panel.Hierarchy.md)

该项目完整拥有 File Browser feature：Tree/List/Grid 表现、导航与过滤、Asset selection、AssetEditor 扩展、文件操作 Action、菜单、Asset drag source，以及 `AssetFileEntry` 的 `AssetSelectionInspectionDrawer`。它只引用共享的 `Inno.Editor.Inspection`，不引用 Hierarchy 或 Inspector Panel。

Scene、Prefab、Folder 和普通文件 icon declaration 可以直接保存完整 `Editor/...` Settings path；`AssetIconRegistry` 用 `EditorSettings.Get(path).GetAsString("value")` 读取当前 Inno Serialization 值对象中的 glyph。脚本声明仍可填写 literal glyph。

## Assets / Plugins 双根与 `.iplugin`

Tree 底部使用带水平分隔线、占满 pane 宽度的 `Switch to Assets` / `Switch to Plugins` 按钮，在可写 `Assets` 创作根与只读 `Plugins` 安装根之间切换。两边分别保存上次访问目录；`Plugins` overview 会在 Tree、List、Grid 中一致列出每个已激活 Plugin ID，进入后继续使用相同的导航、过滤、搜索、selection、打开和 drag source 逻辑。虚拟 `Assets` 与 `Plugins` 根是稳定容器节点，即使当前没有任何子项也始终保留 disclosure，并允许展开或收起；它们只用 icon/text 表示当前目录，不进入普通 Asset selection，也不绘制 selected row highlight。Plugin mount 与其后代才是可选择条目。单击普通目录只选择，双击才进入；Tree 展开只响应 disclosure 或明确的目录导航，不因 content selection 持续强制展开。所有真实条目使用完整 `AssetPath(source, localPath)`，因此不同 mount 的同名文件不会碰撞。

Plugin ID 条目对应 active Plugin catalog 明确拥有的 Source Mount 根，而不是普通 Directory 或 Catalog Asset；它可以导航、选择和作为只读目录 drag source，但没有 `AssetInfo` 或 runtime asset type。FileBrowser 不再用“非 Project mount”猜测 Plugin 身份，因此以后增加其他 source kind 也不会误显示为 Plugin。List 将它显示为 `IPLUGIN` 且 Source 为当前层级 `~`，Inspector 也显示 `IPlugin`，安装容器内部的目录才继续显示 Directory/FOLDER。这里的 `IPlugin` 是 `.iplugin` package 激活后的 source mount 语义，不创建 companion asset。FileBrowser 核心布局与 entry 绘制的 Child、Table、Tree、ID 和 Style scope 会在异常路径中完整 unwind，因此条目交互失败不会再把 ImGui window stack 留在半开启状态。

- `Assets` 是唯一官方创作源；Plugin 源码、Shader、设置和资产先在这里开发，再由 File 菜单的 `Export as Plugin...` 直接导出完整 Project。
- 只有放在 `Plugins/` 根下的 `.iplugin` 文件是安装形态；Folder、`.zip` 和其他扩展名不会激活。Plugin 根和条目显示只读状态；Create、Rename、Move、Delete、Save 与 drop target 会隐藏或明确拒绝，Open、查看、导航、搜索与只读 drag source 保持可用。
- File Browser 不提供 Plugin 管理或 trust 按钮；`.iplugin` 文件系统变化由 `PluginEnvironment` 自动监听、轮询并进入统一候选事务，错误通过 Diagnostics/Console 报告。
- 放入带代码的 Plugin 即表示允许其以项目脚本相同的本机权限执行；File Browser 只展示 active Source Mount，不承担安全确认职责。
- File Browser 不创建导出定义，也不拥有打包 UI。`.iplugin` 与 Game Player 的统一入口属于 [Inno.Editor.Exporting](Inno.Editor.Exporting.md)；导出不会自动安装或刷新当前项目。

Project `Assets` 中名称以 `~` 开头的目录显示为 `ISAMPLE`，但仍按普通可写 authoring Folder 运行：正常导入、编译并参与 Editor 与 Play Mode，只在 Player deployment 中排除。相同目录导出并安装为 `.iplugin` 后，在只读 Plugin 根下才切换为待导入 Sample：它仍可展开、搜索、选择和浏览，但自身及后代不会直接导入或编译。右键该目录的 `Import Sample` 会把一个稳定快照直接导入到 `Assets/<原始~目录名>`，完整保留所有前导 `~`；目标已存在时命令禁用。导入结果是普通可编辑 Assets，保留 Sample 内部 `.imeta` 引用，并作为一个完整目录操作进入共享 Undo/Redo。

Import Sample 的 Action 只启动资产层事务，`Samples/` 中的 feature Module 按帧推进；复制、身份重写、隔离资产预导入/索引、脚本 reference/Roslyn preflight 与 History archive/payload 编码在后台完成。进度复用同一 `EditorModal` 与共享 Widget，居中、不可拖拽、阻断底层交互，提供 Cancel；成功、普通失败或取消完成后自动关闭，详细结果进入共同 LogRouter。

脚本校验读取 `IAssetSourceSnapshot` 隔离候选；成功前当前 File Browser / Catalog / Identity 不变。成功在发布前记录一个共享 History 项，History 失败回滚候选；没有第二套 Undo 栈、事件队列或资产数据库。Feature Module 的 stop 顺序先于 Scripting，取消仍 Pending 时保留任务、快照和所有依赖，退休超时明确 Fault。Source rewriter 的线程契约见 [Asset Pipeline](../assets/Inno.Assets.Pipeline.md)。

项目引用先列仅实现依赖并设置 `PrivateAssets="compile"`，再列真实 public/protected 签名依赖；Compiler、Scripting readiness 和 presentation 不作为 File Browser 公开 API 传递。
异步流程的代码入口位于本项目 Samples 目录；产品响应性、取消和 History 的本轮实测状态见[当前验收](../architecture/CURRENT_ACCEPTANCE.md)。

## 公共扩展 API

| API | 作用 |
| --- | --- |
| `AssetBrowserRoot` / `AssetBrowserState` | 区分 Assets/Plugins 根，分别保存导航位置，并始终保留最近的可写 Project 目录。 |
| `AssetEditor` / `AssetEditorAttribute` | 为特定 Asset 类型声明 Open/Rename/Delete/Drag 行为。 |
| `AssetEditorContext` | 当前 `EditorContext`、interactions、路径、Asset 信息和实例。 |
| `AssetFileEntry` | EditorScript 可检查的源文件身份、路径、扩展名和只读状态。 |
| `AssetIconAttribute` | 按 imported Asset 类型或 source extension 配置 Tree/List/Grid 共用图标；glyph 使用 `InnoEditor.ImGui.ImGuiIcon`。 |
| `AssetEditorModule.GetIcon` | 为其他 Editor presentation 解析完全相同的 Asset 图标。 |
| `AssetEditorModule.CreateSource` / `BeginCreatedSourceRename` | 让其他 Editor feature 复用原子 source 创建，并在用户发起创建时接入统一选择与 inline Rename。 |

## 为新 Asset 添加双击与右键行为

```csharp
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Editor.Interactions;
using Inno.Editor.Panel.FileBrowser;

[AssetEditor(typeof(TextAsset), useForChildren: true, priority: 100)]
public sealed class TextAssetEditor : AssetEditor
{
    public override AssetOperationValidation ValidateDelete(AssetEditorContext context)
        => context.info.status == AssetImportStatus.Imported
            ? AssetOperationValidation.valid
            : AssetOperationValidation.Invalid("The source is not ready.");
}
```

Asset Rename/Delete 的物理事务始终由 `AssetPipeline` 执行。AssetEditor 只能验证以及接收提交后的通知，不能自行移动 source/meta/artifact，因此外部文件变化与 Editor 操作拥有同一身份规则。

Create Asset、Create Folder、Import Sample、Rename、Move 与 Delete 都接入共享中立 Undo/Redo。每个 Asset 创建模板以及 Shader Editor 的创建、复制、折叠为 Subgraph 流程都必须通过 `AssetEditorModule.CreateSource`，并记录一个 `Create Asset` 历史项。该历史项保存完整 source 与 `.imeta`；Undo 删除已创建资产，Redo 恢复同一 persistent ID，而不是创建新的对象身份。Rename/Move 只记录 source/target path；Delete 与 Sample Import 同样把 source、目录结构和 `.imeta` 编码进 History payload。大 payload 自动落到 `<Project>/Library/Editor/History`，Undo 先在临时目录完整验证 archive，再提交回 Asset root 并 `Rescan`，因此恢复失败不会留下半个目录。目标发生外部冲突时操作失败并留在原栈，绝不覆盖新文件。Asset Browser selection 仅在文件系统事务成功后 best-effort 更新，通知异常不改变 History 结果。

## 文件与目录移动

Tree、List 和 Grid 使用同一个 `AssetFileEntry` 目录目标及 `panel/asset.file-browser` drop area。文件仍以共享 `AssetInfo` 作为 payload，目录以 `AssetFileEntry` 作为 payload；两者都可以拖到任意视图中的目录，因此可以从 Grid 拖到 Tree，也可以从 Tree 拖到 List/Grid。Tree 的 `Assets` 根节点和 Tree pane 未占用背景都明确以 Assets 根目录为目标；List/Grid 的未占用背景才以当前打开目录为目标。目标路径必须由每个 drop site 显式提供，不会隐式回退到当前目录。

FileBrowser 根 Panel 和填满正文的布局 Child 均不滚动；Tree、内容列表与底部面包屑各自只在自己的内容实际溢出时滚动。Tree pane 只在名称或层级缩进真实超出 viewport 时产生横向范围，并显示原生水平 scrollbar；短内容没有 scrollbar。Tree 的 label/icon/hit area 只应用一次 `ScrollX`，不会出现内容比 disclosure 或 guide 多移动一份滚动距离的情况。

底部面包屑栏仅在路径实际超出可用宽度时设置显式内容宽度并启用横向滚动；可容纳的路径交给 ImGui 按真实 item 宽度布局，避免 Windows 缩放和像素取整使等宽的空白滚动范围常驻。

提交前统一检查目标目录存在、同名冲突、目录拖入自身或 descendant，以及 AssetEditor 对 move 的验证。拖到当前 parent 属于 no-op，不产生 History；成功移动后保留 source/meta identity、选择新路径，并以单个 `Move Asset` 操作进入 Undo/Redo。目录移动由 `AssetPipeline.Move` 原子处理，目录内子项不单独复制或逐项重建。SceneAsset 的 Rename、Move、拖放及其 Undo/Redo 只改变 Asset source metadata；已加载的 clean Scene 会在同一 UI frame 更新 document 路径和显示名，不产生 Hierarchy `*`。

所有 Tree/List/Grid 目录目标统一调用 `ImGuiWidget.DropTargetHighlight`。目标框使用全局 `DragDropTarget` 黄色、统一 rounding/thickness，并绘制在 viewport foreground draw list，因此不会被 Table column、Grid cell 或 child window 的 clip rect 截断。

为某类 Asset 添加额外右键菜单只需普通 Action：

```csharp
using Inno.Assets.Pipeline;
using Inno.Editor.Assets;
using Inno.Editor.Interactions;

[EditorAction("example.source.reimport", "panel/asset.file-browser")]
[EditorMenu("panel/asset.file-browser", "Source/Reimport", order: 400)]
public sealed class ReimportSourceAction(AssetPipeline assets) : EditorAction<AssetFileEntry>
{
    protected override void Execute(EditorActionContext<AssetFileEntry> context)
    {
        assets.Import(context.target.assetPath);
    }
}
```

同一个菜单 Attribute 自动出现在 Tree/List/Grid，因为三种视图都提交相同 area 和共享 `AssetFileEntry` target。

文件条目的右键菜单由两个同 area 的 interaction 组合：包含目录提供 `Create` 命令，条目本身提供 Rename、Delete 等对象命令；两组之间只绘制一个语义分隔线。因此右键文件也能在其父目录创建资产，右键目录则在该目录创建。统一 Create 菜单包含 Folder、插件贡献的 Shader 图模板，以及所有从 `AssetCreationTemplate` 派生并带源元数据的资产模板。Registry 根据 `menuPath` 自动建立多层分类，只在 top-level domain 边界应用声明的 separator；不会给每个叶子画横线。

Material 与 Render Pipeline 已分别由自己的 feature 贡献模板。其他资产类型使用相同模板协议贡献创建入口。新 `AssetObject` 不进入 File Browser switch，只需在所属 Editor feature 中添加创建模板。原生结构化资产直接使用泛型模板；自定义文本或二进制源 override `Encode`。详细协议见 [Inno.Editor.Assets](Inno.Editor.Assets.md)。用户通过 Create 菜单建立资产后统一完成原子文件创建、导入和选择，并立即进入共享 inline Rename；Folder 和 Shader 模板遵循同一交互。只读 mount 的创建命令保持禁用。

## 为 Asset 类型声明图标

图标扩展不要求 runtime Asset 程序集引用 Editor。在 Editor extension 项目中选择任意容器类型，并把任意数量的声明并排放在该类型上：

```csharp
using Inno.Assets;
using Inno.Editor.Panel.FileBrowser;

[AssetIcon(typeof(TextAsset), "T", useForChildren: true, priority: 100)]
[AssetIcon(".txt", "T")]
internal static class TextAssetIcons
{
}
```

容器类没有实例和运行时职责，只是 TypeCache 可以发现的声明位置。它可以是任意 class、struct、interface、enum 或 delegate；同一个类型允许多个 `AssetIcon`。内建声明位于 `Icons/BuiltInAssetIcons.cs`，不需要放在 `Properties`。

类型声明适合需要按照继承体系选择图标的 Editor extension；extension 声明适合引擎内建文件格式，并且不要求 FileBrowser 项目引用定义 Asset 类型的程序集。extension 可以省略开头的 `.`，匹配时忽略大小写，也支持 `.editor.cs` 这样的复合后缀。解析时先选择类型声明；没有类型声明时选择最长的匹配后缀，再用 priority 打破同等 specificity。

`ImGuiIcon` 与 pointer-free `NativeImGui` 统一由 `Inno.Editor.ImGui/Properties/ScriptingApi.cs` 导出到 `InnoEditor.ImGui`。FileBrowser 的脚本清单只拥有 Asset feature API，不重复导出图标；`Inno.Adapter.Presentation.ImGui` 不声明脚本 API。

内建 Text、Binary、Scene、Prefab 和 Scripting 图标全部在 `BuiltInAssetIcons` 上使用 extension overload 声明，没有基于具体 Asset CLR 类型的引用。图标发现不依赖具体 Asset 类型项目；FileBrowser 的公开资产 API 仍声明 `Inno.Assets` 依赖，Sample module 通过实现依赖 `Inno.Editor.Scripting` 查询编译状态。内部 `AssetIconRegistry` 扫描当前 TypeCache snapshot 中的声明类型。EditorScripts 热重载时，新增或修改声明会随候选代际原子生效；移除声明或整个容器类型后，Registry 会释放旧映射并恢复优先级较低的内建声明，没有匹配时则使用通用 File icon。

`AssetEditorModule.GetIcon(entry)` 是唯一对外 presentation resolver，同时通过 `IInspectionIconProvider<AssetFileEntry>` 向 Inspection 基础设施提供同一个规则。File Browser 的三种视图与 Asset Inspection Header 都调用该入口，不复制 extension switch，也不各自持有 Registry snapshot。Registry 先按类型/extension 选中声明；若 declaration 字符串是已注册 Settings path，就直接读取其中的 `value`，否则把它当作 literal glyph。Settings 基础项目不提供 icon resolver。

## Rename 与打开

- 快速双击调用 AssetEditor 的 Open。
- Rename 只能从 entry 的右键菜单、F2 快捷键或 Create Folder 的创建完成流程启动；单击、延迟单击和双击都不会进入重命名。
- F2 会使用当前正在操作的 Tree、List 或 Grid 展示位置绘制输入框。
- Create Folder 完成后会选中新目录并自动进入重命名。
- Rename Action 自己持有输入/验证状态；Tree/List/Grid 只调用 `Present` 绘制 inline editor。
- 文件重命名只编辑最后一个扩展名前的真实名称，并在提交时无条件保留原文件的最后扩展名；目录名则完整可编辑。例如 `Player.iscene` 的输入值是 `Player`，`Tool.editor.cs` 的输入值是 `Tool.editor`。这里不为 `.editor.cs` 建立复合扩展特例，规则始终只是标准的最后扩展名 `.cs`。
- Tree/List/Grid 与 Hierarchy 共用 `ImGuiWidget.InlineRename` 的紧凑输入框；输入框采用相同 frame metrics，在 row 内垂直居中，首次获得焦点时全选内容，并绘制在 selection/hover highlight 之上。蓝色焦点线框以实际输入框为基准只向外扩展 1px，使用与 DropTarget 相同的统一 overlay 粗细并绘制到 foreground。List 不读取隐藏标签 Selectable 的临时 item 高度，而是以 Table `RowPosY1/RowPosY2` 的实际屏幕边界为居中基准。
- 输入框失去焦点或 selection 切换到其他 target 时，Rename Action 会提交当前有效名称并结束；无效名称保留原值并结束。
- Tree/List/Grid 的未占用背景收到左键点击时会清除当前 Asset selection。
- SceneAsset 打开 Action 由 Hierarchy feature 实现，但使用全局 Open 语义和共享路径参数，不形成 Panel project 引用。
- 全局 Save 保存尚无 source path 的 Scene 时，始终使用 File Browser 最近访问的 `Assets` 目录作为 fallback；即使当前正在查看只读 Plugin，也不会尝试向 Plugin Mount 写入。已有 source 的 Scene 仍保存回自身路径。

ID 为 `asset-browser` 的 Asset Browser Module 保存 active root，以及 Assets/Plugins 各自的完整当前 `AssetPath`；Asset selection 属于当前 Editor session，不写入 `editor.ini`。Plugin mount 消失时 Plugins 视图退回只读 overview 并清除无效 selection，不会破坏 Assets 创作位置。ID 为 `asset.file-browser` 的 Panel 保存 List/Grid 模式、搜索过滤、scope/type filter、Tree/Content 分隔比例、grid scale，以及 List 分隔位置。

List 的三个 column 使用同一个内容 inset，手动 splitter 只占用从 header 到最后一行的真实 table 高度，因此 header 与每一条内容 row 都能接收 resize 拖动，而下方空白区域不会继续接收 hover 或拖动。row Selectable 明确允许 splitter overlay 重叠，separator 不会吞掉 Name、Type 或 Source 的正常点击区域。Grid 图标和文件名使用 draw-list overlay 绘制，不通过 `SetCursorScreenPos` 移动布局 cursor；图标先从卡片中扣除顶部、水平和 label 间距，再按剩余区域等比缩小。最终位置使用 baked glyph 的 `X0/Y0/X1/Y1` 可见边界计算，所以 Font Awesome 中左右 bearing 不对称的 Cube、Folder 等图标也会把真实轮廓中心放在卡片水平中心线上，并且不会越过卡片上沿。Selectable 仍是唯一负责 cell 尺寸与输入的 ImGui item。Inline Rename 必须临时移动 cursor 时，会在恢复布局位置后提交零尺寸 item，避免扩展 parent boundary 的 ImGui assertion。

## Scripting API

EditorScripts 使用 `InnoEditor.Assets` 扩展 AssetEditor、声明 AssetIcon/AssetCreationTemplate，并可用 `AssetFileEntry` 为插件源类型贡献条件 Inspector Drawer。`IInspectionIconProvider<AssetFileEntry>` 也进入裁剪 API，使 Drawer Header 与 File Browser 使用同一个 Appearance 图标来源。Action/Menu/Drop Attribute 与运行时 API 共用 feature-owned `const string` ID；脚本必须显式写 `using InnoEditor.Assets;`。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Editor.Panel.FileBrowser.AssetBrowserRoot`

<!-- api: T:Inno.Editor.Panel.FileBrowser.AssetBrowserRoot b72d053229252f5639b5e3d348ee5ca7959c959101725508aa899bf39386278e -->
<!-- api: F:Inno.Editor.Panel.FileBrowser.AssetBrowserRoot.Assets 30c829ee735eb5a0d1828b2364331ae1b71da5a357ba26684f8e4c4406e70098 -->
<!-- api: F:Inno.Editor.Panel.FileBrowser.AssetBrowserRoot.Plugins 6f43b7c88926098231df5bd1106aad6b391162c3a754a09f6f261b6a5775eea1 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-126f2477e67e5b2d"></a>[`Inno.Editor.Panel.FileBrowser.AssetBrowserRoot.Assets = 0`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Browser/AssetBrowserState.cs#L21) | The writable project Assets authoring root. | — | — |
| <a id="api-369de8bafc803c02"></a>[`Inno.Editor.Panel.FileBrowser.AssetBrowserRoot.Plugins = 1`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Browser/AssetBrowserState.cs#L26) | The read-only Plugins installation root. | — | — |
| <a id="api-06b4379884c5cfaf"></a>[`public enum Inno.Editor.Panel.FileBrowser.AssetBrowserRoot : int`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Browser/AssetBrowserState.cs#L16) | Identifies the authoring or installed-content root displayed by the Asset Browser. | — | — |

### `Inno.Editor.Panel.FileBrowser.AssetBrowserState`

<!-- api: T:Inno.Editor.Panel.FileBrowser.AssetBrowserState 4381ad421f0011d3a92e257f015e3a707e314f76235600346711d97e5dfabdf6 -->
<!-- api: P:Inno.Editor.Panel.FileBrowser.AssetBrowserState.root f710de6de3cb478152dab7fae828db7990cad765a9ae80438efb08088e766185 -->
<!-- api: P:Inno.Editor.Panel.FileBrowser.AssetBrowserState.currentDirectory f04b7226d9266b541c9fa7b7410be6513324b235a8ae8fa0a7dbf932f91ad92c -->
<!-- api: P:Inno.Editor.Panel.FileBrowser.AssetBrowserState.projectDirectory 172d4a26ea72aaec181b81eeab9901e5d571dc9937f4811bb37cedad966c96b2 -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetBrowserState.GetSelectedPath(Inno.Editor.Core.EditorContext) 3f73935bd4fbd8833be532c8b0ed936e407908f36eb5c2d21604df929771b591 -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetBrowserState.SetRoot(Inno.Editor.Panel.FileBrowser.AssetBrowserRoot) f3219441efeb463b388e58b87b69f2f6da7f75bba7dba9d30c5046033621c0e1 -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetBrowserState.SetCurrentDirectory(System.String) 08147520e57636ded572abc74db8f0304619f2ecfefd33cdc60ede97006d2703 -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetBrowserState.Select(Inno.Editor.Core.EditorContext,System.String) 241c27d4eded3cc203052056c2a5e9cc135561b99f8806984a4762f889611ab7 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-41f56aede9050024"></a>[`public string? Inno.Editor.Panel.FileBrowser.AssetBrowserState.GetSelectedPath(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Browser/AssetBrowserState.cs#L88) | Gets the selected asset path when the editor-wide target belongs to the Asset Browser.<br>返回：The normalized source-relative path, or null when another target type is selected.<br>`context`：The shared editor context containing the global selection. | — | `System.ArgumentNullException`：Thrown when context is null. |
| <a id="api-e1d04e88bbdf6070"></a>[`public void Inno.Editor.Panel.FileBrowser.AssetBrowserState.Select(Inno.Editor.Core.EditorContext context, string? relativePath)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Browser/AssetBrowserState.cs#L171) | Selects an asset path through the editor-wide selection state, or clears Asset selection.<br>`context`：The shared editor context whose selection should be updated.<br>`relativePath`：The source-relative path to select, or an empty value to clear selection. | — | `System.ArgumentNullException`：Thrown when context is null. |
| <a id="api-58b9321ca38f8519"></a>[`public void Inno.Editor.Panel.FileBrowser.AssetBrowserState.SetCurrentDirectory(string relativePath)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Browser/AssetBrowserState.cs#L142) | Sets the current Asset Browser directory and infers its root from the isolated source identity.<br>`relativePath`：The isolated source-relative directory path, or an empty value for the currently displayed root. | — | — |
| <a id="api-915001c3c4afd6ad"></a>[`public void Inno.Editor.Panel.FileBrowser.AssetBrowserState.SetRoot(Inno.Editor.Panel.FileBrowser.AssetBrowserRoot value)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Browser/AssetBrowserState.cs#L134) | Switches the displayed root while preserving the last directory visited in each root.<br>`value`：The authoring or installed-content root to display. | — | — |
| <a id="api-bfa71b75db3fe588"></a>[`public string Inno.Editor.Panel.FileBrowser.AssetBrowserState.currentDirectory { get; }`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Browser/AssetBrowserState.cs#L67) | Gets the current directory inside Inno.Editor.Panel.FileBrowser.AssetBrowserState.root. An empty value identifies that root's overview. | — | — |
| <a id="api-d45ee5cd06404383"></a>[`public string Inno.Editor.Panel.FileBrowser.AssetBrowserState.projectDirectory { get; }`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Browser/AssetBrowserState.cs#L74) | Gets the most recently visited writable project directory, independently of the displayed root. | — | — |
| <a id="api-5ae058ec1c806968"></a>[`public Inno.Editor.Panel.FileBrowser.AssetBrowserRoot Inno.Editor.Panel.FileBrowser.AssetBrowserState.root { get; private set; }`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Browser/AssetBrowserState.cs#L62) | Gets the root currently displayed by the Asset Browser. | — | — |
| <a id="api-c0d6297bb465076c"></a>[`public sealed class Inno.Editor.Panel.FileBrowser.AssetBrowserState`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Browser/AssetBrowserState.cs#L32) | Stores asset browser navigation independently from global object selection. | — | — |

### `Inno.Editor.Panel.FileBrowser.AssetEditor`

<!-- api: T:Inno.Editor.Panel.FileBrowser.AssetEditor b971321f0f37e5b44bf4008044013407ecbc296b57c52baac26dfd45e12f3432 -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetEditor.CanOpen(Inno.Editor.Panel.FileBrowser.AssetEditorContext) 620094553be48965ce491ff22588ecca451c2703090e32044f29e7da04cf7572 -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetEditor.Open(Inno.Editor.Panel.FileBrowser.AssetEditorContext) 77cabd2eea0a0b302944a23d722a9655b37c7216e3834d7cad95bea0c84c0f30 -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetEditor.ValidateRename(Inno.Editor.Panel.FileBrowser.AssetEditorContext,System.String) 4fb40e72adae03bc17090583e017bd6cb6f8d31d7595207ac45e518e9d3dbc28 -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetEditor.OnRenamed(Inno.Editor.Panel.FileBrowser.AssetEditorContext,System.String,System.String) adbd8c1500ab9e5ed2048f7946954a33c6d5e6cfa57147aca9337d5c4b0fdc69 -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetEditor.ValidateDelete(Inno.Editor.Panel.FileBrowser.AssetEditorContext) 914dabcc11425690561260ebc9d2841a1887c6b119044c79d9513331b8a1afd8 -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetEditor.OnDeleted(Inno.Editor.Panel.FileBrowser.AssetEditorContext) 32e701335c109125b62cf7474418a666d1b028c295eda99802ea06051107eed8 -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetEditor.CanStartDrag(Inno.Editor.Panel.FileBrowser.AssetEditorContext) 8d71dfb92e2100c8136aad6faf00c646676b358490d6caaeb7f94691c37f7f6a -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetEditor.CreateDragData(Inno.Editor.Panel.FileBrowser.AssetEditorContext) d1d143ca5ba615a6ce9c238e30ce6a5dd0ff7faa1b748b1bfc472f6da5630ad9 -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetEditor.#ctor c230d1bd5fbedec408cf278802bf6a4654e7f4e1d86d8cfb33757fe30c514fd4 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b676a74f0f828e7f"></a>[`protected Inno.Editor.Panel.FileBrowser.AssetEditor.AssetEditor()`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditor.cs#L8) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-bd34da933c7c1c46"></a>[`public virtual bool Inno.Editor.Panel.FileBrowser.AssetEditor.CanOpen(Inno.Editor.Panel.FileBrowser.AssetEditorContext context)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditor.cs#L19) | Gets whether the fallback asset editor can open the supplied source entry.<br>返回：true when Inno.Editor.Panel.FileBrowser.AssetEditor.Open(Inno.Editor.Panel.FileBrowser.AssetEditorContext) can handle the entry; otherwise, false.<br>`context`：The immutable source and catalog snapshot for the operation. | — | — |
| <a id="api-397544a5daeaa40d"></a>[`public virtual bool Inno.Editor.Panel.FileBrowser.AssetEditor.CanStartDrag(Inno.Editor.Panel.FileBrowser.AssetEditorContext context)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditor.cs#L97) | Gets whether the supplied entry can begin a managed editor drag operation.<br>返回：true when drag data can be created; otherwise, false.<br>`context`：The immutable source and catalog snapshot for the entry. | — | — |
| <a id="api-4d11856dbe785f53"></a>[`public virtual Inno.Editor.Interactions.EditorDragData Inno.Editor.Panel.FileBrowser.AssetEditor.CreateDragData(Inno.Editor.Panel.FileBrowser.AssetEditorContext context)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditor.cs#L108) | Creates the managed source, preview label, and validity predicate for an asset drag.<br>返回：The managed drag data published by the Asset Browser.<br>`context`：The immutable source and catalog snapshot for the entry. | — | — |
| <a id="api-8e1c90b7eaa820c0"></a>[`public virtual void Inno.Editor.Panel.FileBrowser.AssetEditor.OnDeleted(Inno.Editor.Panel.FileBrowser.AssetEditorContext context)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditor.cs#L84) | Runs after an asset deletion transaction commits successfully.<br>`context`：The immutable snapshot captured before the entry was deleted. | — | — |
| <a id="api-9f742c55f1855661"></a>[`public virtual void Inno.Editor.Panel.FileBrowser.AssetEditor.OnRenamed(Inno.Editor.Panel.FileBrowser.AssetEditorContext context, string oldPath, string newPath)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditor.cs#L60) | Runs after an asset move transaction commits successfully.<br>`context`：The immutable snapshot captured before the move.<br>`oldPath`：The previous source-relative path.<br>`newPath`：The committed source-relative destination path. | — | — |
| <a id="api-339878da57ab25fe"></a>[`public virtual void Inno.Editor.Panel.FileBrowser.AssetEditor.Open(Inno.Editor.Panel.FileBrowser.AssetEditorContext context)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditor.cs#L27) | Opens the supplied asset entry when no more specific typed open action handled it.<br>`context`：The immutable source and catalog snapshot for the operation. | — | — |
| <a id="api-a198453cf88858fd"></a>[`public virtual Inno.Editor.Panel.FileBrowser.AssetOperationValidation Inno.Editor.Panel.FileBrowser.AssetEditor.ValidateDelete(Inno.Editor.Panel.FileBrowser.AssetEditorContext context)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditor.cs#L76) | Validates a requested asset deletion before the AssetPipeline transaction begins.<br>返回：A valid result when the transaction may proceed, or a diagnostic describing the rejection.<br>`context`：The immutable snapshot of the entry that would be deleted. | — | — |
| <a id="api-35ce422146b2e589"></a>[`public virtual Inno.Editor.Panel.FileBrowser.AssetOperationValidation Inno.Editor.Panel.FileBrowser.AssetEditor.ValidateRename(Inno.Editor.Panel.FileBrowser.AssetEditorContext context, string targetPath)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditor.cs#L43) | Validates a requested asset move before the AssetPipeline transaction begins.<br>返回：A valid result when the transaction may proceed, or a diagnostic describing the rejection.<br>`context`：The immutable snapshot of the source entry before the move.<br>`targetPath`：The normalized source-relative destination path. | — | — |
| <a id="api-9dd714815a7f16f2"></a>[`public abstract class Inno.Editor.Panel.FileBrowser.AssetEditor`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditor.cs#L8) | Customizes editor interactions for one imported asset type. | — | — |

### `Inno.Editor.Panel.FileBrowser.AssetEditorAttribute`

<!-- api: T:Inno.Editor.Panel.FileBrowser.AssetEditorAttribute 411bedecd652a1500961a0535dbc5474c998742e64f0f975293c0fd68b32b56e -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetEditorAttribute.#ctor(System.Type,System.Boolean,System.Int32) a78bad626143adf85f972c9d0b97b6d1203b8e5794a15f5dff410f7bbacc732f -->
<!-- api: P:Inno.Editor.Panel.FileBrowser.AssetEditorAttribute.assetType 0b846b3cefe30d035686b4e8031fef95031cd86a9e8cced83c5d60df36ef7f0e -->
<!-- api: P:Inno.Editor.Panel.FileBrowser.AssetEditorAttribute.useForChildren 9ddd70663ea295aa27b0584c23b6b92ae90649ce71575b1fac4786af6f771200 -->
<!-- api: P:Inno.Editor.Panel.FileBrowser.AssetEditorAttribute.priority ae4b0c7b796517def4cdf3f63aa50da1751c0b097211f12660d42902cf865e50 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c690a543fa069043"></a>[`public Inno.Editor.Panel.FileBrowser.AssetEditorAttribute.AssetEditorAttribute(System.Type assetType, bool useForChildren = false, int priority = 0)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorAttribute.cs#L26) | Creates an asset-editor registration for an imported runtime asset type.<br>`assetType`：The imported asset type handled by the editor.<br>`useForChildren`：Whether the editor may also handle assignable derived asset types.<br>`priority`：The tie-breaking priority after exactness and inheritance distance. | — | `System.ArgumentNullException`：Thrown when assetType is null. |
| <a id="api-78c8287cfb575349"></a>[`public System.Type Inno.Editor.Panel.FileBrowser.AssetEditorAttribute.assetType { get; }`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorAttribute.cs#L39) | Gets the imported asset type handled by the editor. | — | — |
| <a id="api-94f1fe8f5f759b54"></a>[`public int Inno.Editor.Panel.FileBrowser.AssetEditorAttribute.priority { get; }`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorAttribute.cs#L49) | Gets the tie-breaking priority. | — | — |
| <a id="api-4e1815e44a5afb40"></a>[`public bool Inno.Editor.Panel.FileBrowser.AssetEditorAttribute.useForChildren { get; }`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorAttribute.cs#L44) | Gets whether derived asset types are accepted. | — | — |
| <a id="api-f32000ab0b38acfb"></a>[`public sealed class Inno.Editor.Panel.FileBrowser.AssetEditorAttribute : System.Attribute`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorAttribute.cs#L9) | Associates an asset editor with an imported asset type. | — | — |

### `Inno.Editor.Panel.FileBrowser.AssetEditorContext`

<!-- api: T:Inno.Editor.Panel.FileBrowser.AssetEditorContext b898d27aac7abd1203267f49651bade39c262ad011149154777d31a5e34f423a -->
<!-- api: P:Inno.Editor.Panel.FileBrowser.AssetEditorContext.editorContext 5e5d124bf02d01b0d601213a8b6be7dee19320166b251fc61a2cf00d8cc9293d -->
<!-- api: P:Inno.Editor.Panel.FileBrowser.AssetEditorContext.interactions 31e6fd76d4e2e115e77678aa6b3a68d99f4758600a58b882c478f0c7516c11db -->
<!-- api: P:Inno.Editor.Panel.FileBrowser.AssetEditorContext.relativePath 5e99bdc338799a8d7efc45cd568cc63c2c20c2f2046df7f0a29a94c21890e475 -->
<!-- api: P:Inno.Editor.Panel.FileBrowser.AssetEditorContext.name 18d1ff2c68e43e9d0751f90f5e12c50701b19ab1b9a803f81915df87fc3c6757 -->
<!-- api: P:Inno.Editor.Panel.FileBrowser.AssetEditorContext.isDirectory de0083ad7401d71cd68e1c16ed784070e754a3e1258f7cce59b5615e4a70299c -->
<!-- api: P:Inno.Editor.Panel.FileBrowser.AssetEditorContext.info 7ebd89166be5b17d318afe72dcf68283e4b5f4e69a90544933432c5a0a2c5538 -->
<!-- api: P:Inno.Editor.Panel.FileBrowser.AssetEditorContext.assetType d9a11fdc79bb5c0a46401ed02a69ca9e579994fa6b03136d9eae361102cf58bb -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-39eb054b4ea82968"></a>[`public System.Type? Inno.Editor.Panel.FileBrowser.AssetEditorContext.assetType { get; }`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorContext.cs#L100) | Gets the resolved imported asset type when available. | — | — |
| <a id="api-3d22908503b4611b"></a>[`public Inno.Editor.Core.EditorContext Inno.Editor.Panel.FileBrowser.AssetEditorContext.editorContext { get; }`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorContext.cs#L70) | Gets the shared editor context. | — | — |
| <a id="api-e40fffa7c3990e45"></a>[`public Inno.Assets.AssetInfo? Inno.Editor.Panel.FileBrowser.AssetEditorContext.info { get; }`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorContext.cs#L95) | Gets the cataloged asset information when available. | — | — |
| <a id="api-8bc424d12ea02ccb"></a>[`public Inno.Editor.Interactions.EditorInteractions Inno.Editor.Panel.FileBrowser.AssetEditorContext.interactions { get; }`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorContext.cs#L75) | Gets the active editor interaction entry point. | — | — |
| <a id="api-ddd6a7f3893ace41"></a>[`public bool Inno.Editor.Panel.FileBrowser.AssetEditorContext.isDirectory { get; }`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorContext.cs#L90) | Gets whether the source represents a directory. | — | — |
| <a id="api-9b0c6c8f9d9ad105"></a>[`public string Inno.Editor.Panel.FileBrowser.AssetEditorContext.name { get; }`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorContext.cs#L85) | Gets the final source path segment. | — | — |
| <a id="api-ec4e81977c90fd2e"></a>[`public string Inno.Editor.Panel.FileBrowser.AssetEditorContext.relativePath { get; }`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorContext.cs#L80) | Gets the source-relative path. | — | — |
| <a id="api-84e9f4a4cc6a6db9"></a>[`public sealed class Inno.Editor.Panel.FileBrowser.AssetEditorContext`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorContext.cs#L12) | Provides an immutable snapshot for an asset editor operation. | — | — |

### `Inno.Editor.Panel.FileBrowser.AssetEditorModule`

<!-- api: T:Inno.Editor.Panel.FileBrowser.AssetEditorModule c2fb56936a6b0c50a4398814b00b51771bca3d5a2ae22aa7b857f66da0b53a74 -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetEditorModule.BeginCreatedSourceRename(Inno.Assets.Pipeline.AssetFileEntry) 3baa1d690519d6592f0bb28f9c7339038d014d63d4420d561117330baf39b896 -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetEditorModule.CreateSource(Inno.Assets.AssetPath,System.ReadOnlySpan{System.Byte}) 0712a2c9f9d33c438461b78fa377274533d1ce1649136f4b85fa378fc7bd6948 -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetEditorModule.#ctor(Inno.Editor.Interactions.EditorInteractions,Inno.Editor.Settings.EditorSettings,Inno.Assets.Pipeline.AssetPipeline,Inno.Plugins.Authoring.PluginEnvironment,Inno.Extensibility.Types.TypeCatalog,Inno.Core.Logging.LogRouter) 26c24d60e1b739ecee7b3b525366d2a595740fb1d1d78408511cdde5cdb9d2a0 -->
<!-- api: P:Inno.Editor.Panel.FileBrowser.AssetEditorModule.browser 3049a2db50655916813c62686ce31ff5a942aadf18693da4c5cc8b521532f533 -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetEditorModule.Capture(Inno.Editor.Core.EditorState) 580f4c1968114ae1769d5193e88f7c2e1cad3d2fd0511f903fdbad2902eec1f4 -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetEditorModule.Restore(Inno.Editor.Core.EditorState) 3d0a924fd4d9b695639cd75ba684282dc4e838d0a23b8b280d441c26a3e1791a -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetEditorModule.GetIcon(Inno.Assets.Pipeline.AssetFileEntry) 9628bee490b8277da545944ad49e0d318a3fd7f1cc6fed4d44f76618f3b9e863 -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetEditorModule.OnStart(Inno.Editor.Core.EditorContext) 8a21b3d5ce377f230ec998a0fe254efc86fe52ee952ca325854d440ffad8eb8c -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetEditorModule.OnStop(Inno.Editor.Core.EditorContext) aa96ff7f0696accca7ff66806a4a1eea5d5a5b46e5596cf328ed60031e24aac5 -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetEditorModule.OnDispose e33e02b1ad53e8a5fd948849d2ce347552d447146ea34a8fdd2a87f20b668f91 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-95a22260c93e94df"></a>[`public Inno.Editor.Panel.FileBrowser.AssetEditorModule.AssetEditorModule(Inno.Editor.Interactions.EditorInteractions interactions, Inno.Editor.Settings.EditorSettings settings, Inno.Assets.Pipeline.AssetPipeline pipeline, Inno.Plugins.Authoring.PluginEnvironment plugins, Inno.Extensibility.Types.TypeCatalog types, Inno.Core.Logging.LogRouter logs)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorModule.cs#L61) | Creates the Asset Browser feature module.<br>`interactions`：The active editor interaction entry point.<br>`settings`：The project Settings service that owns semantic icon values.<br>`pipeline`：The isolated authoring asset pipeline used by browser operations.<br>`plugins`：The active Plugin catalog that authoritatively identifies Plugin-owned source mounts.<br>`types`：The active type catalog used to build extension registry snapshots.<br>`logs`：The application log router used for Asset Browser operation diagnostics. | — | `System.ArgumentNullException`：Thrown when any required service is null. |
| <a id="api-ee1eda0f3ee7d8c9"></a>[`public void Inno.Editor.Panel.FileBrowser.AssetEditorModule.BeginCreatedSourceRename(Inno.Assets.Pipeline.AssetFileEntry entry)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorModule.Creation.cs#L22) | Selects a newly created source and starts its shared inline rename interaction.<br>`entry`：The newly indexed writable source. | — | — |
| <a id="api-d118904a275f94ba"></a>[`protected override void Inno.Editor.Panel.FileBrowser.AssetEditorModule.Capture(Inno.Editor.Core.EditorState state)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorModule.cs#L93) | Captures an immutable snapshot of the current observable state.<br>`state`：The lifecycle or domain state applied by this operation. | — | — |
| <a id="api-1deaf0986364abf6"></a>[`public Inno.Assets.Pipeline.AssetFileEntry Inno.Editor.Panel.FileBrowser.AssetEditorModule.CreateSource(Inno.Assets.AssetPath path, System.ReadOnlySpan<byte> bytes)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorModule.Creation.cs#L48) | Creates a native or ordinary-text asset source as one recoverable shared-history operation.<br>返回：The indexed source identity, even when its first import reports a source error.<br>`path`：A new file in an existing writable asset directory.<br>`bytes`：Complete source bytes supplied by the feature's native writer. | — | `System.IO.IOException`：The source already exists, has no existing parent, or cannot be installed.<br>`System.InvalidOperationException`：The source mount is unavailable/read-only or history cannot retain the change. |
| <a id="api-9b2bc2d1271b13d9"></a>[`public string Inno.Editor.Panel.FileBrowser.AssetEditorModule.GetIcon(Inno.Assets.Pipeline.AssetFileEntry entry)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorModule.cs#L542) | Resolves the presentation icon registered for an asset type or source extension.<br>返回：The most specific registered icon, or the built-in directory or file icon when no registration matches the entry.<br>`entry`：The source entry whose presentation icon should be resolved. | — | `System.ArgumentNullException`：Thrown when entry is null. |
| <a id="api-edcc0d2cd2d322d3"></a>[`protected override void Inno.Editor.Panel.FileBrowser.AssetEditorModule.OnDispose()`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorModule.cs#L582) | Releases resources retained by this feature after it has stopped. | — | — |
| <a id="api-ea7c09ff6efbb8ab"></a>[`protected override void Inno.Editor.Panel.FileBrowser.AssetEditorModule.OnStart(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorModule.cs#L563) | Initializes this feature when its owning runtime becomes active.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-f51506124167c139"></a>[`protected override void Inno.Editor.Panel.FileBrowser.AssetEditorModule.OnStop(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorModule.cs#L574) | Stops this feature before its owning runtime releases the active generation.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-a7d510f6f4665c62"></a>[`protected override void Inno.Editor.Panel.FileBrowser.AssetEditorModule.Restore(Inno.Editor.Core.EditorState state)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorModule.cs#L106) | Restores the supplied snapshot while preserving current invariants.<br>`state`：The lifecycle or domain state applied by this operation. | — | — |
| <a id="api-90488802edcab69b"></a>[`public Inno.Editor.Panel.FileBrowser.AssetBrowserState Inno.Editor.Panel.FileBrowser.AssetEditorModule.browser { get; }`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorModule.cs#L85) | Gets shared Asset Browser navigation and selection state. | — | — |
| <a id="api-d5ab6ce1376980a7"></a>[`public sealed class Inno.Editor.Panel.FileBrowser.AssetEditorModule : Inno.Editor.Core.EditorModule, Inno.Editor.Inspection.IInspectionIconProvider<Inno.Assets.Pipeline.AssetFileEntry>`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorModule.Creation.cs#L14) | Handles asset creation actions in the Editor file browser. | — | — |

### `Inno.Editor.Panel.FileBrowser.AssetIconAttribute`

<!-- api: T:Inno.Editor.Panel.FileBrowser.AssetIconAttribute 94e317149a3e2530b192b042b3d749123f4771e05b4b95b39becc3e04a9e51c8 -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetIconAttribute.#ctor(System.Type,System.String,System.Boolean,System.Int32) 0eb74f5fcd3bcadf97c84a30b070f7c29850f9a3a825e9612ea49e33f5597981 -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetIconAttribute.#ctor(System.String,System.String,System.Int32) c6a1a28d8918f3a08e19c67acffcaa075e065f73ed48a7af81f68623fd516b23 -->
<!-- api: P:Inno.Editor.Panel.FileBrowser.AssetIconAttribute.assetType 374fce6ed266aaaeb2a6f10810288ed15ae76e794803e971c2179bf3acd1a7f7 -->
<!-- api: P:Inno.Editor.Panel.FileBrowser.AssetIconAttribute.extension 7a6d20a0e92e8242ab916b68ad12a5287a6100893109c386c985073c1399a38c -->
<!-- api: P:Inno.Editor.Panel.FileBrowser.AssetIconAttribute.icon bfb9097555a7c9205a758677063b8d9cb4fcd09ef41aecd792b388c8791b1069 -->
<!-- api: P:Inno.Editor.Panel.FileBrowser.AssetIconAttribute.useForChildren 06f09bc744f935e6ea08b436c0755deb588fac0e8b90e433c13567b06bc82a8c -->
<!-- api: P:Inno.Editor.Panel.FileBrowser.AssetIconAttribute.priority 496532cade17115145a8b834a188fb69d947e9756d1792f49ba4ee6b0864376e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e967254982f1f39a"></a>[`public Inno.Editor.Panel.FileBrowser.AssetIconAttribute.AssetIconAttribute(string extension, string icon, int priority = 0)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Icons/AssetIconAttribute.cs#L71) | Creates an icon declaration for files ending with a source extension.<br>`extension`：The simple or compound file extension. A leading period is optional and matching is case-insensitive.<br>`icon`：The Editor icon glyph to render.<br>`priority`：The tie-breaking priority after extension specificity. | — | `System.ArgumentException`：Thrown when extension is empty or contains a path separator, or when icon is empty. |
| <a id="api-4cf3f605975e837b"></a>[`public Inno.Editor.Panel.FileBrowser.AssetIconAttribute.AssetIconAttribute(System.Type assetType, string icon, bool useForChildren = false, int priority = 0)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Icons/AssetIconAttribute.cs#L43) | Creates an icon declaration using a glyph from the Editor icon catalog.<br>`assetType`：The imported asset type represented by the icon.<br>`icon`：The Editor icon glyph to render.<br>`useForChildren`：Whether the declaration may also represent derived asset types.<br>`priority`：The tie-breaking priority after exactness and inheritance distance. | — | `System.ArgumentNullException`：Thrown when assetType is null.<br>`System.ArgumentException`：Thrown when icon is empty. |
| <a id="api-58940c8c8ea9f7b7"></a>[`public System.Type? Inno.Editor.Panel.FileBrowser.AssetIconAttribute.assetType { get; }`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Icons/AssetIconAttribute.cs#L85) | Gets the imported asset type represented by this declaration, or null for an extension declaration. | — | — |
| <a id="api-92d43582a4072703"></a>[`public string? Inno.Editor.Panel.FileBrowser.AssetIconAttribute.extension { get; }`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Icons/AssetIconAttribute.cs#L91) | Gets the normalized source extension represented by this declaration, or null for a type declaration. | — | — |
| <a id="api-fa2691d03f5e5825"></a>[`public string Inno.Editor.Panel.FileBrowser.AssetIconAttribute.icon { get; }`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Icons/AssetIconAttribute.cs#L96) | Gets the Editor icon glyph to render. | — | — |
| <a id="api-632207a626814644"></a>[`public int Inno.Editor.Panel.FileBrowser.AssetIconAttribute.priority { get; }`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Icons/AssetIconAttribute.cs#L107) | Gets the tie-breaking priority after target specificity. | — | — |
| <a id="api-fd9ef59113dd250c"></a>[`public bool Inno.Editor.Panel.FileBrowser.AssetIconAttribute.useForChildren { get; }`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Icons/AssetIconAttribute.cs#L102) | Gets whether a type declaration may also represent derived asset types. Extension declarations always return false. | — | — |
| <a id="api-ed9adb8d5d5be6e7"></a>[`public sealed class Inno.Editor.Panel.FileBrowser.AssetIconAttribute : System.Attribute`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Icons/AssetIconAttribute.cs#L20) | Declares the icon used by the Asset Browser for an imported asset type or source extension. | Apply one or more declarations to a container type. The container has no runtime responsibility; it only keeps related icon declarations together so the type catalog can discover them. | — |

### `Inno.Editor.Panel.FileBrowser.AssetImportSettingsEdits`

<!-- api: T:Inno.Editor.Panel.FileBrowser.AssetImportSettingsEdits 93fdc7b3ebfd634da8452ed62a560c43a176263603d4263b3fa1f5536a0b17b3 -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetImportSettingsEdits.#ctor(Inno.Assets.Pipeline.AssetPipeline,Inno.Core.Serialization.SerializationRegistry,Inno.Extensibility.Types.TypeCatalog,Inno.Editor.Interactions.EditorInteractions) 7c6cc80622970d540353a29d5e2afb8a48542405e15fac06c623b35b974422ed -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetImportSettingsEdits.Apply(Inno.Assets.AssetPath,Inno.Core.Serialization.ISerializable,System.String) a48805b16e7bd6665ffeb52e7587b2f502ec911e1a3e80faafc9d6e80fa2d3ab -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-577ef337cea73c25"></a>[`public Inno.Editor.Panel.FileBrowser.AssetImportSettingsEdits.AssetImportSettingsEdits(Inno.Assets.Pipeline.AssetPipeline assets, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Extensibility.Types.TypeCatalog types, Inno.Editor.Interactions.EditorInteractions interactions)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetImportSettingsEdits.cs#L39) | Uses the authoring owners responsible for source identity, converter generations and shared undo.<br>`assets`：Authoritative asset pipeline.<br>`serialization`：Current owner converter registry.<br>`types`：Current type generation owner.<br>`interactions`：Shared Editor history owner. | — | — |
| <a id="api-0bcb183d6134d5ca"></a>[`public bool Inno.Editor.Panel.FileBrowser.AssetImportSettingsEdits.Apply(Inno.Assets.AssetPath path, Inno.Core.Serialization.ISerializable settings, string expectedFingerprint)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetImportSettingsEdits.cs#L66) | Saves one settings gesture, recording neutral before/after properties even when reimport reports an error.<br>返回：Whether the saved configuration reimported successfully; false does not mean the settings were discarded.<br>`path`：Writable source asset path.<br>`settings`：Detached current-generation settings value.<br>`expectedFingerprint`：Sidecar fingerprint captured when editing began. | — | — |
| <a id="api-7c2c82fdb99ffabe"></a>[`public sealed class Inno.Editor.Panel.FileBrowser.AssetImportSettingsEdits : Inno.Editor.Core.EditorModule`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetImportSettingsEdits.cs#L16) | Applies importer configuration through the common sidecar pipeline and stable-identity Editor history. | — | — |

### `Inno.Editor.Panel.FileBrowser.AssetOperationValidation`

<!-- api: T:Inno.Editor.Panel.FileBrowser.AssetOperationValidation 5434ddc197466d210f9c1ff03755cf5207eaf9c5817287b3aa9e8ba6a95cad6b -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetOperationValidation.#ctor(System.Boolean,System.String) 00cda119eb32e92f366eb645eba318e20654a3e1c1962f32ec032c7c057181d8 -->
<!-- api: P:Inno.Editor.Panel.FileBrowser.AssetOperationValidation.isValid 79ffc568ef67ea45ea3e984ecb90bb04e287b4ea70bddf4eb2987a022848b0af -->
<!-- api: P:Inno.Editor.Panel.FileBrowser.AssetOperationValidation.message 97a0c59f0eec58011b0570e60f252b9aeec02c3f7232b3ea9b92aee516d518f7 -->
<!-- api: P:Inno.Editor.Panel.FileBrowser.AssetOperationValidation.valid 8beb1f378039ae55db31324257013a80ed0f2acb098326a4c3a8fe40efaf52e8 -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetOperationValidation.Invalid(System.String) f96ff760c308ddd3eabcc83235051513b527190b363897bb104b1b4fddbfb76d -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetOperationValidation.ToString 3826da074be5ddaa82559129322964bcf7d87d86d0673de10f4f0efb96aef43d -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetOperationValidation.op_Inequality(Inno.Editor.Panel.FileBrowser.AssetOperationValidation,Inno.Editor.Panel.FileBrowser.AssetOperationValidation) 0ad96e47841b163df9f9ff98c7d922ecdb01ebf8b37eb94de528f2ccc9dd2f7a -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetOperationValidation.op_Equality(Inno.Editor.Panel.FileBrowser.AssetOperationValidation,Inno.Editor.Panel.FileBrowser.AssetOperationValidation) a719051955c483731828b733743b388d1d7668cfc88bde618f18bd17e92695a3 -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetOperationValidation.GetHashCode 68b16a6870d5a21d316c33744785d9a391a4e77a28ebaa0a581a2585cff21d6f -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetOperationValidation.Equals(System.Object) a413ee2a25c7d08bfee463ed47385ad4fc28530d71f0a1c28dd0304a15bb3dcb -->
<!-- api: M:Inno.Editor.Panel.FileBrowser.AssetOperationValidation.Equals(Inno.Editor.Panel.FileBrowser.AssetOperationValidation) 6e8468169fdea0de630ddc844635ab2706ce2d4151a736b6f83cd75fa50d2abd -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-29a030289495916e"></a>[`public Inno.Editor.Panel.FileBrowser.AssetOperationValidation.AssetOperationValidation(bool isValid, string message)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetOperationValidation.cs#L17) | Creates the validation result returned before an asset transaction begins.<br>`isValid`：Whether the requested asset operation may proceed.<br>`message`：The user-facing rejection diagnostic, or an empty string for a valid result. | — | — |
| <a id="api-a8d716b150fae7e0"></a>[`public bool Inno.Editor.Panel.FileBrowser.AssetOperationValidation.Equals(Inno.Editor.Panel.FileBrowser.AssetOperationValidation other)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetOperationValidation.cs#L6) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-d91b85fd41b9a1b5"></a>[`public override bool Inno.Editor.Panel.FileBrowser.AssetOperationValidation.Equals(object obj)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetOperationValidation.cs#L6) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-a27663d333fe72b5"></a>[`public override int Inno.Editor.Panel.FileBrowser.AssetOperationValidation.GetHashCode()`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetOperationValidation.cs#L6) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-53adbb368375142e"></a>[`public static Inno.Editor.Panel.FileBrowser.AssetOperationValidation Inno.Editor.Panel.FileBrowser.AssetOperationValidation.Invalid(string message)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetOperationValidation.cs#L49) | Creates a failed asset-operation validation result.<br>返回：An invalid validation result containing the supplied diagnostic.<br>`message`：The user-facing diagnostic explaining why the operation was rejected. | — | — |
| <a id="api-990f762df2bf22da"></a>[`public static bool Inno.Editor.Panel.FileBrowser.AssetOperationValidation.operator ==(Inno.Editor.Panel.FileBrowser.AssetOperationValidation left, Inno.Editor.Panel.FileBrowser.AssetOperationValidation right)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetOperationValidation.cs#L6) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-b1682fbcc4449395"></a>[`public static bool Inno.Editor.Panel.FileBrowser.AssetOperationValidation.operator !=(Inno.Editor.Panel.FileBrowser.AssetOperationValidation left, Inno.Editor.Panel.FileBrowser.AssetOperationValidation right)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetOperationValidation.cs#L6) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-4f10282495bc0934"></a>[`public override string Inno.Editor.Panel.FileBrowser.AssetOperationValidation.ToString()`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetOperationValidation.cs#L6) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-ab0c785fb0199778"></a>[`public bool Inno.Editor.Panel.FileBrowser.AssetOperationValidation.isValid { get; }`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetOperationValidation.cs#L28) | Gets whether the operation may proceed. | — | — |
| <a id="api-3d7b1d52453362d6"></a>[`public string Inno.Editor.Panel.FileBrowser.AssetOperationValidation.message { get; }`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetOperationValidation.cs#L33) | Gets the validation diagnostic. | — | — |
| <a id="api-0978233b2f2f0158"></a>[`public static Inno.Editor.Panel.FileBrowser.AssetOperationValidation Inno.Editor.Panel.FileBrowser.AssetOperationValidation.valid { get; }`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetOperationValidation.cs#L38) | Gets a successful validation result. | — | — |
| <a id="api-17b94c96736f4cae"></a>[`public readonly record struct Inno.Editor.Panel.FileBrowser.AssetOperationValidation : System.IEquatable<Inno.Editor.Panel.FileBrowser.AssetOperationValidation>`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetOperationValidation.cs#L6) | Describes whether an editor asset operation may proceed. | — | — |

## 项目依赖

- [Inno.Scripting.Compiler](../scripting/Inno.Scripting.Compiler.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Editor.Assets](Inno.Editor.Assets.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.IO](../core/Inno.Core.IO.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Editor.ImGui](Inno.Editor.ImGui.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Presentation.ImGui.Sdl3](../backends/ImGui/Inno.Adapter.Presentation.ImGui.Sdl3.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Native.ImGui](../backends/ImGui/Inno.Native.ImGui.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Editor.Scripting](Inno.Editor.Scripting.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Editor.Core](Inno.Editor.Core.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
- [Inno.Editor.Inspection](Inno.Editor.Inspection.md)：公开引用边界由实际签名核对。
- [Inno.Editor.Interactions](Inno.Editor.Interactions.md)：公开引用边界由实际签名核对。
- [Inno.Editor.Settings](Inno.Editor.Settings.md)：公开引用边界由实际签名核对。
- [Inno.Assets](../assets/Inno.Assets.md)：公开引用边界由实际签名核对。
- [Inno.Assets.Pipeline](../assets/Inno.Assets.Pipeline.md)：公开引用边界由实际签名核对。
- [Inno.Plugins.Authoring](../plugins/Inno.Plugins.Authoring.md)：公开引用边界由实际签名核对。
- [Inno.Core.Logging](../core/Inno.Core.Logging.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
