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
这是已记录的响应性整改项，不是已实现的异步导入能力；后续需保持校验失败回滚与 History 原子性。
详见 [提交前第四轮复核](../architecture/PRECOMMIT_BGCS_AUDIT_2026_10_02.md)。

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
using Inno.Editor.Panel.FileBrowser;
using InnoEditor.ImGui;

[AssetEditor(typeof(AnimationClipAsset), useForChildren: true, priority: 100)]
public sealed class AnimationClipEditor : AssetEditor
{
    public override bool CanOpen(AssetEditorContext context) => true;

    public override void Open(AssetEditorContext context)
    {
        // Open the animation feature without changing File Browser identity.
    }

    public override AssetOperationValidation ValidateDelete(
        AssetEditorContext context)
        => context.info.status == AssetImportStatus.Ready
            ? AssetOperationValidation.valid
            : AssetOperationValidation.Invalid("The clip is not ready.");
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
internal static class AnimationInteractionIds
{
    internal const string C_REIMPORT = "animation/reimport";
    internal const string C_FILE_BROWSER_AREA = "panel/asset.file-browser";
}

[EditorAction(AnimationInteractionIds.C_REIMPORT, AnimationInteractionIds.C_FILE_BROWSER_AREA)]
[EditorMenu(AnimationInteractionIds.C_FILE_BROWSER_AREA, "Animation/Reimport", order: 400)]
public sealed class ReimportAnimationAction : EditorAction<AssetFileEntry>
{
    protected override EditorActionState Query(
        EditorActionContext<AssetFileEntry> context)
        => context.target.extension == ".anim"
            ? EditorActionState.enabled
            : EditorActionState.hidden;

    protected override void Execute(EditorActionContext<AssetFileEntry> context)
    {
        AssetPipeline.Import(context.target.relativePath);
    }
}
```

同一个菜单 Attribute 自动出现在 Tree/List/Grid，因为三种视图都提交相同 area 和共享 `AssetFileEntry` target。

文件条目的右键菜单由两个同 area 的 interaction 组合：包含目录提供 `Create` 命令，条目本身提供 Rename、Delete 等对象命令；两组之间只绘制一个语义分隔线。因此右键文件也能在其父目录创建资产，右键目录则在该目录创建。统一 Create 菜单包含 Folder、插件贡献的 Shader 图模板，以及所有从 `AssetCreationTemplate` 派生并带源元数据的资产模板。Registry 根据 `menuPath` 自动建立多层分类，只在 top-level domain 边界应用声明的 separator；不会给每个叶子画横线。

Material 与 Render Pipeline 已分别由自己的 feature 贡献模板；Rendering2D 插件同样贡献 Sprite Atlas、Sprite Animation、Tile Set、Tilemap、Particle Effect、Post Process Profile 与已配置的 2D Pipeline。新 `AssetObject` 不进入 File Browser switch，只需在所属 Editor feature 中添加创建模板。原生结构化资产直接使用泛型模板；自定义文本或二进制源 override `Encode`。详细协议见 [Inno.Editor.Assets](Inno.Editor.Assets.md)。用户通过 Create 菜单建立资产后统一完成原子文件创建、导入和选择，并立即进入共享 inline Rename；Folder 和 Shader 模板遵循同一交互。只读 mount 的创建命令保持禁用。

## 为 Asset 类型声明图标

图标扩展不要求 runtime Asset 程序集引用 Editor。在 Editor extension 项目中选择任意容器类型，并把任意数量的声明并排放在该类型上：

```csharp
using Inno.Editor.Panel.FileBrowser;

[AssetIcon(
    typeof(AnimationClipAsset),
    ImGuiIcon.FileAudio,
    useForChildren: true,
    priority: 100)]
[AssetIcon(
    typeof(AnimationControllerAsset),
    ImGuiIcon.DiagramProject)]
[AssetIcon(".animationclip", ImGuiIcon.FileAudio)]
internal static class AnimationAssetIcons
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

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Editor.Panel.FileBrowser.AssetBrowserRoot`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Panel.FileBrowser.AssetBrowserRoot`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Browser/AssetBrowserState.cs#L16) | Identifies the authoring or installed-content root displayed by the Asset Browser. |
| [`Inno.Editor.Panel.FileBrowser.AssetBrowserRoot.Assets`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Browser/AssetBrowserState.cs#L21) | The writable project Assets authoring root. |
| [`Inno.Editor.Panel.FileBrowser.AssetBrowserRoot.Plugins`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Browser/AssetBrowserState.cs#L26) | The read-only Plugins installation root. |

### `Inno.Editor.Panel.FileBrowser.AssetBrowserState`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Panel.FileBrowser.AssetBrowserRoot Inno.Editor.Panel.FileBrowser.AssetBrowserState.root`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Browser/AssetBrowserState.cs#L62) | Gets the root currently displayed by the Asset Browser. |
| [`Inno.Editor.Panel.FileBrowser.AssetBrowserState`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Browser/AssetBrowserState.cs#L32) | Stores asset browser navigation independently from global object selection. |
| [`string Inno.Editor.Panel.FileBrowser.AssetBrowserState.currentDirectory`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Browser/AssetBrowserState.cs#L67) | Gets the current directory inside . An empty value identifies that root's overview. |
| [`string Inno.Editor.Panel.FileBrowser.AssetBrowserState.projectDirectory`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Browser/AssetBrowserState.cs#L74) | Gets the most recently visited writable project directory, independently of the displayed root. |
| [`string? Inno.Editor.Panel.FileBrowser.AssetBrowserState.GetSelectedPath(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Browser/AssetBrowserState.cs#L88) | Gets the selected asset path when the editor-wide target belongs to the Asset Browser. |
| [`void Inno.Editor.Panel.FileBrowser.AssetBrowserState.Select(Inno.Editor.Core.EditorContext context, string? relativePath)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Browser/AssetBrowserState.cs#L171) | Selects an asset path through the editor-wide selection state, or clears Asset selection. |
| [`void Inno.Editor.Panel.FileBrowser.AssetBrowserState.SetCurrentDirectory(string relativePath)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Browser/AssetBrowserState.cs#L142) | Sets the current Asset Browser directory and infers its root from the isolated source identity. |
| [`void Inno.Editor.Panel.FileBrowser.AssetBrowserState.SetRoot(Inno.Editor.Panel.FileBrowser.AssetBrowserRoot value)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Browser/AssetBrowserState.cs#L134) | Switches the displayed root while preserving the last directory visited in each root. |

### `Inno.Editor.Panel.FileBrowser.AssetEditor`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Panel.FileBrowser.AssetEditor`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditor.cs#L8) | Customizes editor interactions for one imported asset type. |
| [`virtual Inno.Editor.Interactions.EditorDragData Inno.Editor.Panel.FileBrowser.AssetEditor.CreateDragData(Inno.Editor.Panel.FileBrowser.AssetEditorContext context)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditor.cs#L108) | Creates the managed source, preview label, and validity predicate for an asset drag. |
| [`virtual Inno.Editor.Panel.FileBrowser.AssetOperationValidation Inno.Editor.Panel.FileBrowser.AssetEditor.ValidateDelete(Inno.Editor.Panel.FileBrowser.AssetEditorContext context)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditor.cs#L76) | Validates a requested asset deletion before the AssetPipeline transaction begins. |
| [`virtual Inno.Editor.Panel.FileBrowser.AssetOperationValidation Inno.Editor.Panel.FileBrowser.AssetEditor.ValidateRename(Inno.Editor.Panel.FileBrowser.AssetEditorContext context, string targetPath)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditor.cs#L43) | Validates a requested asset move before the AssetPipeline transaction begins. |
| [`virtual bool Inno.Editor.Panel.FileBrowser.AssetEditor.CanOpen(Inno.Editor.Panel.FileBrowser.AssetEditorContext context)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditor.cs#L19) | Gets whether the fallback asset editor can open the supplied source entry. |
| [`virtual bool Inno.Editor.Panel.FileBrowser.AssetEditor.CanStartDrag(Inno.Editor.Panel.FileBrowser.AssetEditorContext context)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditor.cs#L97) | Gets whether the supplied entry can begin a managed editor drag operation. |
| [`virtual void Inno.Editor.Panel.FileBrowser.AssetEditor.OnDeleted(Inno.Editor.Panel.FileBrowser.AssetEditorContext context)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditor.cs#L84) | Runs after an asset deletion transaction commits successfully. |
| [`virtual void Inno.Editor.Panel.FileBrowser.AssetEditor.OnRenamed(Inno.Editor.Panel.FileBrowser.AssetEditorContext context, string oldPath, string newPath)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditor.cs#L60) | Runs after an asset move transaction commits successfully. |
| [`virtual void Inno.Editor.Panel.FileBrowser.AssetEditor.Open(Inno.Editor.Panel.FileBrowser.AssetEditorContext context)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditor.cs#L27) | Opens the supplied asset entry when no more specific typed open action handled it. |

### `Inno.Editor.Panel.FileBrowser.AssetEditorAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Panel.FileBrowser.AssetEditorAttribute`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorAttribute.cs#L8) | Associates an asset editor with an imported asset type. |
| [`Inno.Editor.Panel.FileBrowser.AssetEditorAttribute.AssetEditorAttribute(System.Type assetType, bool useForChildren = false, int priority = 0)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorAttribute.cs#L26) | Creates an asset-editor registration for an imported runtime asset type. |
| [`System.Type Inno.Editor.Panel.FileBrowser.AssetEditorAttribute.assetType`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorAttribute.cs#L39) | Gets the imported asset type handled by the editor. |
| [`bool Inno.Editor.Panel.FileBrowser.AssetEditorAttribute.useForChildren`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorAttribute.cs#L44) | Gets whether derived asset types are accepted. |
| [`int Inno.Editor.Panel.FileBrowser.AssetEditorAttribute.priority`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorAttribute.cs#L49) | Gets the tie-breaking priority. |

### `Inno.Editor.Panel.FileBrowser.AssetEditorContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.AssetInfo? Inno.Editor.Panel.FileBrowser.AssetEditorContext.info`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorContext.cs#L95) | Gets the cataloged asset information when available. |
| [`Inno.Editor.Core.EditorContext Inno.Editor.Panel.FileBrowser.AssetEditorContext.editorContext`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorContext.cs#L70) | Gets the shared editor context. |
| [`Inno.Editor.Interactions.EditorInteractions Inno.Editor.Panel.FileBrowser.AssetEditorContext.interactions`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorContext.cs#L75) | Gets the active editor interaction entry point. |
| [`Inno.Editor.Panel.FileBrowser.AssetEditorContext`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorContext.cs#L12) | Provides an immutable snapshot for an asset editor operation. |
| [`System.Type? Inno.Editor.Panel.FileBrowser.AssetEditorContext.assetType`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorContext.cs#L100) | Gets the resolved imported asset type when available. |
| [`bool Inno.Editor.Panel.FileBrowser.AssetEditorContext.isDirectory`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorContext.cs#L90) | Gets whether the source represents a directory. |
| [`string Inno.Editor.Panel.FileBrowser.AssetEditorContext.name`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorContext.cs#L85) | Gets the final source path segment. |
| [`string Inno.Editor.Panel.FileBrowser.AssetEditorContext.relativePath`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorContext.cs#L80) | Gets the source-relative path. |

### `Inno.Editor.Panel.FileBrowser.AssetEditorModule`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.Pipeline.AssetFileEntry Inno.Editor.Panel.FileBrowser.AssetEditorModule.CreateSource(Inno.Assets.AssetPath path, System.ReadOnlySpan<byte> bytes)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorModule.Creation.cs#L48) | Creates a native or ordinary-text asset source as one recoverable shared-history operation. |
| [`Inno.Editor.Panel.FileBrowser.AssetBrowserState Inno.Editor.Panel.FileBrowser.AssetEditorModule.browser`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorModule.cs#L85) | Gets shared Asset Browser navigation and selection state. |
| [`Inno.Editor.Panel.FileBrowser.AssetEditorModule`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorModule.Creation.cs#L14) | Handles asset creation actions in the Editor file browser. |
| [`Inno.Editor.Panel.FileBrowser.AssetEditorModule.AssetEditorModule(Inno.Editor.Interactions.EditorInteractions interactions, Inno.Editor.Settings.EditorSettings settings, Inno.Assets.Pipeline.AssetPipeline pipeline, Inno.Plugins.Authoring.PluginEnvironment plugins, Inno.Extensibility.Types.TypeCatalog types, Inno.Core.Logging.LogRouter logs)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorModule.cs#L61) | Creates the Asset Browser feature module. |
| [`override void Inno.Editor.Panel.FileBrowser.AssetEditorModule.Capture(Inno.Editor.Core.EditorState state)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorModule.cs#L93) | Captures an immutable snapshot of the current observable state. |
| [`override void Inno.Editor.Panel.FileBrowser.AssetEditorModule.OnDispose()`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorModule.cs#L582) | Releases resources retained by this feature after it has stopped. |
| [`override void Inno.Editor.Panel.FileBrowser.AssetEditorModule.OnStart(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorModule.cs#L563) | Initializes this feature when its owning runtime becomes active. |
| [`override void Inno.Editor.Panel.FileBrowser.AssetEditorModule.OnStop(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorModule.cs#L574) | Stops this feature before its owning runtime releases the active generation. |
| [`override void Inno.Editor.Panel.FileBrowser.AssetEditorModule.Restore(Inno.Editor.Core.EditorState state)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorModule.cs#L106) | Restores the supplied snapshot while preserving current invariants. |
| [`string Inno.Editor.Panel.FileBrowser.AssetEditorModule.GetIcon(Inno.Assets.Pipeline.AssetFileEntry entry)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorModule.cs#L542) | Resolves the presentation icon registered for an asset type or source extension. |
| [`void Inno.Editor.Panel.FileBrowser.AssetEditorModule.BeginCreatedSourceRename(Inno.Assets.Pipeline.AssetFileEntry entry)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetEditorModule.Creation.cs#L22) | Selects a newly created source and starts its shared inline rename interaction. |

### `Inno.Editor.Panel.FileBrowser.AssetIconAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Panel.FileBrowser.AssetIconAttribute`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Icons/AssetIconAttribute.cs#L12) | Declares the icon used by the Asset Browser for an imported asset type or source extension. |
| [`Inno.Editor.Panel.FileBrowser.AssetIconAttribute.AssetIconAttribute(System.Type assetType, string icon, bool useForChildren = false, int priority = 0)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Icons/AssetIconAttribute.cs#L43) | Creates an icon declaration using a glyph from the Editor icon catalog. |
| [`Inno.Editor.Panel.FileBrowser.AssetIconAttribute.AssetIconAttribute(string extension, string icon, int priority = 0)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Icons/AssetIconAttribute.cs#L71) | Creates an icon declaration for files ending with a source extension. |
| [`System.Type? Inno.Editor.Panel.FileBrowser.AssetIconAttribute.assetType`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Icons/AssetIconAttribute.cs#L85) | Gets the imported asset type represented by this declaration, or for an extension declaration. |
| [`bool Inno.Editor.Panel.FileBrowser.AssetIconAttribute.useForChildren`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Icons/AssetIconAttribute.cs#L102) | Gets whether a type declaration may also represent derived asset types. Extension declarations always return . |
| [`int Inno.Editor.Panel.FileBrowser.AssetIconAttribute.priority`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Icons/AssetIconAttribute.cs#L107) | Gets the tie-breaking priority after target specificity. |
| [`string Inno.Editor.Panel.FileBrowser.AssetIconAttribute.icon`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Icons/AssetIconAttribute.cs#L96) | Gets the Editor icon glyph to render. |
| [`string? Inno.Editor.Panel.FileBrowser.AssetIconAttribute.extension`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Icons/AssetIconAttribute.cs#L91) | Gets the normalized source extension represented by this declaration, or for a type declaration. |

### `Inno.Editor.Panel.FileBrowser.AssetImportSettingsEdits`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Panel.FileBrowser.AssetImportSettingsEdits`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetImportSettingsEdits.cs#L15) | Applies importer configuration through the common sidecar pipeline and stable-identity Editor history. |
| [`Inno.Editor.Panel.FileBrowser.AssetImportSettingsEdits.AssetImportSettingsEdits(Inno.Assets.Pipeline.AssetPipeline assets, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Extensibility.Types.TypeCatalog types, Inno.Editor.Interactions.EditorInteractions interactions)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetImportSettingsEdits.cs#L38) | Uses the authoring owners responsible for source identity, converter generations and shared undo. |
| [`bool Inno.Editor.Panel.FileBrowser.AssetImportSettingsEdits.Apply(Inno.Assets.AssetPath path, Inno.Core.Serialization.ISerializable settings, string expectedFingerprint)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetImportSettingsEdits.cs#L66) | Saves one settings gesture, recording neutral before/after properties even when reimport reports an error. |

### `Inno.Editor.Panel.FileBrowser.AssetOperationValidation`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Panel.FileBrowser.AssetOperationValidation`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetOperationValidation.cs#L6) | Describes whether an editor asset operation may proceed. |
| [`Inno.Editor.Panel.FileBrowser.AssetOperationValidation.AssetOperationValidation(bool isValid, string message)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetOperationValidation.cs#L17) | Creates the validation result returned before an asset transaction begins. |
| [`bool Inno.Editor.Panel.FileBrowser.AssetOperationValidation.isValid`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetOperationValidation.cs#L28) | Gets whether the operation may proceed. |
| [`static Inno.Editor.Panel.FileBrowser.AssetOperationValidation Inno.Editor.Panel.FileBrowser.AssetOperationValidation.Invalid(string message)`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetOperationValidation.cs#L49) | Creates a failed asset-operation validation result. |
| [`static Inno.Editor.Panel.FileBrowser.AssetOperationValidation Inno.Editor.Panel.FileBrowser.AssetOperationValidation.valid`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetOperationValidation.cs#L38) | Gets a successful validation result. |
| [`string Inno.Editor.Panel.FileBrowser.AssetOperationValidation.message`](../../src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/AssetEditors/AssetOperationValidation.cs#L33) | Gets the validation diagnostic. |

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
