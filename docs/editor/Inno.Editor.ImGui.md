# Inno.Editor.ImGui

## 共享 Inspector 与窗口呈现

`ImGuiWidget.SectionLayout(Action drawContent)` 为一段内容建立可复用的 fieldset scope；scope 内连续调用 `bool SectionHeader(string title, string? description = null, Action? drawLeadingControl = null)` 时，标题嵌入上边框，直到下一个标题或 scope 结束的内容都被同一边框完整包裹。展开的内部 content fieldset 使用轻微圆角；窗口、Panel、顶层 Header 和其他大容器保持直角。返回值决定当前 section 正文是否应绘制；标题文字本身通过共享的 `ClickableText` widget 切换展开状态，不显示额外的加减号或整行 hover 背景。标题与 Stats 的共享 `CollectionSectionHeader` 使用同一 `sectionHeaderPadding`，因此左侧缩进不会由各 Panel 自行估算。折叠态保留中断式横线，并在整条线的左右端各绘制一根以横线为中心的短竖帽，形成 `⊢ … ⊣` 轮廓；竖帽与横线共享颜色、粗细和交点，不形成向下的残留边框。折叠状态由 ImGui 按当前 Inspector scope 的稳定顺序保存，标题或 Local/World 文案变化不会重置状态。`EnsureSection` 为没有 `[Header]` 的默认属性自动建立 `Properties` fieldset，`isSectionContentVisible` 则让 serialized-property pipeline 在任意 section 折叠后持续跳过内容，直到下一个 Header。可选 leading control 用于 Local/World 这类属于分组语义本身的开关；scope 外的 `SectionHeader` 仍保持普通分隔标题并返回 `true`。`HeaderSurface(id, drawContent, spanWindowPadding)` 是 Inspector target 与 Shader Editor 顶部共用的直角 header 容器；`CollectionSectionHeader` 是 Stats 等 collection surface 的直角分组条。`HelpBox(string text, string icon, Vector4 color)` 绘制有边框、状态图标与侧边语义色的内部提示卡片。`DrawItemTooltip` 使用父 viewport work area、真实内宽和缩放 padding 测量，靠边自动翻向并约束位置。File Browser、Hierarchy 与 Stats 共用暗色 collectionRow/collectionRowAlternate，交替条纹、hover、选择仍保持区分。

浮动 Panel 在标题右侧显示统一关闭按钮；docked Panel 保持每个 dock node 一个关闭入口。Docked close glyph 以 ImGui 实际 `TabBar.BarRect` 的几何中心定位，不用字体高度或 DockNode 顶点推测，因此不同缩放与顶栏布局下仍严格垂直居中。窗口标题不继承输入控件的 FrameBorderSize，避免额外分隔线；这与透明接缝是两个不同的问题。窗口、Child、Tab 和 component header 的外轮廓使用直角；按钮、输入框、菜单、tooltip、HelpBox 与 Inspector content fieldset 等内部交互/内容表面仍使用语义圆角。所有按钮仍走现有 Panel close/dirty 确认生命周期。

[Editor 索引](README.md) · [Platform ImGui](../backends/ImGui/Inno.Adapter.Presentation.ImGui.Sdl3.md) · [Wiki 首页](../README.md)

## 退出所有权

`EditorGameInputCapture` 是 Game View 对 Play Session 的中立输入策略。Host 在每帧前调用 `BeginFrame`，Game View 用 `Report` 更新当前 viewport 的窗口、画面边界、焦点和前景悬停，帧后用 `CompleteFrame` 识别需要释放按键的失焦窗口。Host 将 `Route(Event)` 返回的事件送进独立 Play Input source；`null` 表示该事件属于其他 Editor 窗口或被 UI 遮挡。它只筛选现有 `Inno.Core.Events` 事件，不注册另一套监听器。Dock close 等自绘命中区同样必须核对 ImGui 前景窗口，不能单凭屏幕坐标触发。

平台事件先进入 Edit Session 的统一 Dispatcher；Host 在 `dispatched` 观察阶段调用 Game View 路由，保证 Editor 快捷键与 Hub 消费已经完成。全局消费的输入不会再次送入游戏。已经向游戏交付输入的窗口，如果其 Key/Mouse release、失焦或关闭事件被上层消费，`Route` 返回新的 focus-reset event；Host 通过统一 Input source 在 Play simulation 前释放按住状态，不重放原 release。其他窗口被消费的事件不改变当前 capture。隐藏、关闭或移动 Game View 的 presentation 失焦仍由 `CompleteFrame` 补充 reset。

同一窗口的鼠标移动即使被 Editor 消费，也更新 capture 的位置边界观察；它不会送入游戏，也不会释放已捕获的拖拽。后续点击和滚轮因此使用最新位置判断，不沿用已过时的“鼠标仍在图像内”状态。

`TreeNode` 的 disclosure、正文与叠加按钮分别拥有命中区域。原生树项仅管理展开箭头及其保留状态，不创建覆盖正文的整行可重叠控件；正文使用自己的按钮，叠加按钮优先消费自身点击。鼠标移动和按下可以在同一帧发生，不要求先悬停一帧；正文双击只切换一次父节点展开，箭头或叠加按钮不触发正文选择。该行为由真实 ImGui context 的 `WidgetLayoutTests` 覆盖，Hierarchy 与 FileBrowser 共同复用，没有各 Panel 的补丁。

`ImGuiEditorRuntime.HandleKeyPressed()` 在原生文本控件要求文本输入时，不向底层 Panel 分发删除、复制、撤销等快捷键；Command/Ctrl+S 仍可执行当前文档的显式保存。焦点与语义动作继续由共享 Interactions 管理，不在控件中直接写盘。

`ImGuiEditorRuntime.Dispose()` 只有在 interaction runtime 完整退场后才标记完成。
Core `RetirementPendingException` 原样上抛并保留内部 runtime；普通已终结错误仍传播，但不会重复已完成的 Dispose。
因此表现层不会把未退休的扩展误当作已经销毁，也不授权上层提前释放 ImGui/native context。

`Inno.Editor.ImGui` 提供编辑器统一控件、菜单/拖放渲染桥和视觉配置。它只包装可复用的 UI 原语，不持有 Scene、Selection 或 Panel 业务状态。

EditorScripts 使用逻辑 namespace `InnoEditor.ImGui`。该项目的唯一 `Properties/ScriptingApi.cs` 导出 Editor widgets、常用 Dear ImGui flags 与 pointer-free `ImGui` facade；`Inno.Adapter.Presentation.ImGui` 不声明脚本 API。Facade 不暴露 native pointer、callback userdata 或 backend texture ID，只能在 Panel/Modal/Drawer 绘制回调期间调用。

`ImGui.ColorEdit4` 沿用 Dear ImGui 的显示 RGB 数值语义。编辑渲染用的线性 RGB 时使用 `ImGui.ColorEditLinear4(label, ref Vector4 value, flags)`：显示前以标准 sRGB 传递函数编码，编辑后解码回线性 RGB，alpha 不转换；输入若显式声明 `InputHsv` 则拒绝，避免把 HSV 数值当作线性 RGB。Inspector 的 `Color` 属性与 Scene/Game 背景设置都使用后一入口，所以选择器的色块与实际画面基于同一线性值。

```text
Inno.Editor.ImGui/
├─ Styling/
│  ├─ EditorPalette.cs
│  └─ EditorStyleMetrics.cs
├─ Runtime/
│  ├─ ImGuiEditorRuntime.cs
│  ├─ EditorModalHost.cs
│  ├─ EditorModalRenderer.cs
│  ├─ EditorMenuRenderer.cs
│  └─ EditorDragDropRenderer.cs
└─ Widgets/
   ├─ ImGuiWidget.Style.cs
   ├─ ImGuiWidget.Search.cs
   ├─ ImGuiWidget.ContextMenu.cs
   ├─ ImGuiWidget.InlineRename.cs
   ├─ ImGuiWidget.Controls.cs
   ├─ ImGuiWidget.Card.cs
   ├─ ImGuiWidget.Tree.cs
   └─ ...
```

Palette 与 Style Metrics 并列位于 `Styling`，runtime host 与三个表现桥统一位于 `Runtime`，不再人为拆分只有三个文件的 Renderers 层。它们仍使用项目 namespace `Inno.Editor.ImGui`。所有 `ImGuiWidget.*.cs` 位于 `Widgets`，namespace 统一为 `Inno.Editor.ImGui.ImGuiWidget`；实现统一组成 `static partial ImGuiWidget`，可复用入口全部是 static 方法。Options、Result、presentation 与私有状态收口在对应的 `ImGuiWidget.<Feature>.cs` 中，不创建独立 Widget helper 文件。

## Palette 与 Style

所有主题颜色集中在 `EditorPalette`：原生 ImGui col、Inspector、Hierarchy、Asset Browser、Logging、轴颜色与 drag target 都不在 Panel 中声明。换主题只需替换这一个 palette surface。

所有跨 Panel 的像素布局、padding、spacing、rounding、列比例和最小尺寸集中在 `ImGuiWidget.style`（`EditorStyleMetrics`）。Panel 可以读取语义名，例如 `panelTabFramePadding`、`sectionHeaderPadding`、`inspectorSectionPadding`、`inspectorSectionRounding`、`inspectorCollapsedSectionCapLength`、`propertyMetadataSpacing`、`assetListNameSeparatorPosition`、`inspectorCardSpacing`、`hierarchyItemSpacing`、`hierarchyRenameMinimumWidth` 与 `settingsFieldPadding`，不应新增散落的固定像素。Inspector fieldset 的 outline 同样来自 `EditorPalette.inspectorSectionBorder`。

`ImGuiWidget.SetupStyle()` 把 layout metrics 和 `EditorPalette` 应用到原生 ImGui style；运行期间 zoom 改变时，runtime 只在倍率发生变化后重新应用一次 native style。普通窗口绘制时，`ResizeGrip`、`ResizeGripHovered` 与 `ResizeGripActive` 使用透明色，因此可缩放窗口仍保留边缘/角落命中能力，但不会显示右下角三角形。Dear ImGui 在更新 Dock tree splitter 时会把 separator hover/active 临时映射到 resize-grip hover/active；`ImGuiEditorRuntime` 只在 `DockSpaceOverViewport` 调用范围内恢复这两个 accent color，保证 Panel 间连接线的 hover/drag feedback 可见，同时不恢复窗口三角形。

主 dockspace 在窗口尺寸变化时按 ImGui 当前节点数据逐层更新子节点的 `SizeRef`，中央 node 不再独占新增加的空间。只有两个子节点的实际尺寸之和等于父节点的可用尺寸时，才用实际尺寸推导比例；刚载入 layout 时的子节点 `Size` 可能为零或错误地等于父尺寸，这时改用 ImGui 保存的 `SizeRef`，避免比例在启动首帧逐次漂移。宿主按当前 ImGui viewport ID 推导 dockspace ID，在第一次提交 dockspace 前读取已加载的节点并等比更新；拖动 splitter 的实际结果成为下一次 resize 的比例，最小尺寸仍由原生 `WindowMinSize` 和 separator 限制。比例不另存一份，布局的唯一持久来源仍是 `editor.ini` 的 `[Docking][Data]`。

`PanelWindow(..., useWindowPadding)` 在 native `Begin` 阶段锁定当前 Panel 的窗口内边距。关闭 padding 只影响该 Panel window 本身，不污染随后打开的菜单、selector 或 popup；它与 `EditorPanel.useWindowPadding` 组成表现无关的布局契约。

`ConstrainedContent(id, drawContent, useWindowPadding)` 是统一的 Panel 正文容器。它按当前可用宽度创建纵向 auto-size child，默认准确应用一层标准 `WindowPadding`，并把显式 content width 设置为扣除左右 padding 后的宽度；child 自身禁止 scrollbar 与 scroll input。外层 Panel 因此可以让纵向 scrollbar 贴紧 Dock 边缘，同时所有 Drawer 自动获得一致的正文间距，长文本或自定义控件也不能制造横向滚动范围。

## 全局缩放

`EditorStyleMetrics.zoom` 是整个 Editor 的统一 UI 倍率。字体、窗口与 frame padding、item spacing、rounding、border、scrollbar、最小尺寸及各 Panel 的语义像素指标都从同一基准乘以 zoom；归一化列比例、图标相对倍率、透明度和时间值保持不变，所以布局比例不会发生二次缩放。

Editor ImGui context 默认启用 Inno overlay scrollbar 扩展。纵横滚动条不参与 `InnerRect`/content width 分配，直接覆盖在内容边缘；滚轮、程序化滚动或正在拖动时立即显示，停止交互 `0.60` 秒后在 `0.30` 秒内淡出。完全淡出后滚动条不参与 hit test，因此不会拦截底层元素。Scrollbar thumb 使用 `EditorPalette.scrollbarGrab*` 的浅紫语义色。交互在 Window Begin 阶段处理，视觉则延迟到 Window End 并提交到所属 viewport 的 foreground draw list，以当前 Window rect 作为精确 clip；因此它与 DropTarget highlight 处于同一顶层表现层，不会被 Parent、Child、Table、Image、Canvas 或自定义 Drawer 遮挡。横纵滚动条同时存在时各自避开右下角交叉区域。该行为在 context 层统一覆盖 Panel、Child、Table host 与 Popup；明确声明 `allowScrolling = false` 的 Scene/Game 等 viewport 则完全不创建滚动条。SDL 轻量 Demo 使用自己的 context，不被 Editor 策略修改。

| 操作 | 快捷键 | 结果 |
| --- | --- | --- |
| `View/Zoom In` | Command/Ctrl + `+` | 在 actual size 基础上增加一个 `0.10` 倍率步长。 |
| `View/Zoom Out` | Command/Ctrl + `-` | 在 actual size 基础上减少一个 `0.10` 倍率步长。 |
| `View/Actual Size` | Command/Ctrl + `0` | 恢复 Settings 中配置的 actual size。 |

有效范围固定为 `0.75..1.50`；Actual Size 使用完整路径 `Editor/Appearance/Accessibility/Actual Size`，只由 Settings Apply 写入 `<ProjectRoot>/Settings.Editor.inno`。Zoom In/Out 的相对步数通过 `EditorZoomModule` 的项目状态保存到 `<ProjectRoot>/editor.ini`，不改 Settings，也不制造 History。

## Modal renderer

`EditorModalRenderer` 根据 `EditorModal.canMove/canResize/initialSize/minimumSize` 选择固定 auto-size 或可拉伸窗口策略。可拉伸 Modal 只在首次出现时居中和应用初始尺寸，随后保留用户移动与缩放；最小尺寸和初始尺寸随当前 zoom 变换。`allowScrolling = false` 会同时禁用 Modal 自身的滚动条与鼠标滚动，由正文 Child 承担溢出滚动。所有 Popup Modal 固定 `NoDocking | NoCollapse`，因此可调整尺寸但不能 Dock 或缩成标题栏。`EditorModalHost` 继续统一管理 fade transition、背景阻塞与 popup 关闭。

所有 runtime-owned `BeginDisabled/EndDisabled`、`PushStyleVar/PopStyleVar`、`BeginPopupModal/EndPopup` 与 PanelWindow Begin/End 都使用 `try/finally` 保证栈平衡。Panel/Modal 的扩展回调在独立边界捕获异常并交给当前 generation quarantine；单个扩展失败不会逃出完整 Editor Draw，也不会污染下一帧的 ImGui stack。

## Menu renderer

`EditorMenuRenderer` 是唯一调用原生 `BeginMenu/MenuItem` 的业务渲染桥。它递归绘制任意层级的 `EditorMenuModel`，从 Action Attribute 自动读取快捷键标签，并把点击排入 Action queue。Panel 只提供 `EditorMenuContext(surface, target)`。

主菜单由同一模型生成，并包含 `File`、`Edit`、`View`、`Panel` 等顶层节点。全局缩放属于 `View`；当前 `EditorPanelRegistry` 中的窗口开关统一生成到 `Panel`，显示 checked 状态并调用内建 Toggle Panel Action。脚本代际新增或移除 Panel 时不需要修改菜单代码。标准 Panel window 不向原生 ImGui 提交 `p_open`，因此普通 Tab 完全不包含关闭按钮。`PanelWindow` 只在原生 `Begin` 建立窗口装饰时应用 `panelTabFramePadding`，避免紧凑输入控件的 padding 把紧贴 Main Menu 的第一行 Dock Tab 压扁。当前可见 Panel 根据所属 Dock Node 的实际位置和尺寸，在 Dock Header 最右侧的原生 close slot 位置绘制一个独立关闭控件；其纵向中心来自当前 `ImGuiTabBar.BarRect`，命中必须属于前景 dock window。控件会补偿图标在字体 slot 中的水平居中 inset，使 X 的可见右边缘与第一个 Tab 的可见左边缘使用相同的 `WindowBorderSize + FramePadding.X` 外边距。它不参与 Tab 排列、不绘制 Tab 背景，并使用共享 palette 的文本颜色及 hover 颜色。点击只关闭当前选中的 Panel，不会关闭同一 Dock Node 内的其他 Tab。该实现不修改 cimgui 或 Dear ImGui 源码。

同一个 MainMenu pass 还读取 `EditorToolbarModel`，按 MenuBar window 的实际宽度把紧凑 icon 组放到几何中心。Renderer 只负责把 `EditorToolbarIcon` 映射到 `ImGuiIcon`、绘制 checked/hover/disabled 状态、tooltip 与快捷键，然后把点击排回 Action queue；Play Mode ID、状态机与命令语义不进入 ImGui 项目。左侧菜单宽度异常接近中心时，toolbar 会向右避让而不覆盖菜单 item。

`ContextMenu` 绑定最近提交的 ImGui item；`WindowContextMenu` 只响应当前 window 中没有 item 占用的背景区域。两者都会先构建菜单模型，模型没有可见条目时不会打开原生 popup，因此不会显示空的黑色菜单框。

所有 context menu 在 `BeginContextMenu` / `EndContextMenu` 范围内应用同一组 `EditorPalette.menu*` 颜色和 `EditorStyleMetrics.menu*` padding、spacing、rounding 与 border。显式点击 Popup 使用 `BeginMenuPopup` / `EndMenuPopup`；hover tooltip 使用 `BeginMenuTooltip` / `EndMenuTooltip`，因此三种浮层共享同一个 presentation contract。`DrawItemTooltip` 是 Settings、Inspector 和其他 property surface 的统一入口：它从最近提交的 item 取得 hover 状态，使用随 Editor zoom 缩放的 300 px 最小宽度和 440 px 换行上限，并复用完全相同的 menu tooltip 外观。短生命周期 Popup 显式继承调用窗口的 viewport，不会因为靠近平台窗口边缘而被提升为独立 OS viewport。Popup 先按内容 auto-size，达到 viewport work area 或调用方约束后转为纵向滚动，且不保存临时窗口尺寸；长菜单不会继续扩大 native window。Panel 的局部 Table/Tree style 不会再改变浮层外观。Popup 打开时，Tree、disclosure 等自绘控件会暂停其底层 hover feedback；原生 popup 本身接收鼠标事件，避免 hover 或点击继续影响菜单后面的 entry。

## CollapsingCard

```csharp
bool open = ImGuiWidget.CollapsingCard(
    id,
    title,
    drawLeadingControl: DrawEnabledToggle,
    drawTrailingControl: DrawRemoveButton,
    defaultOpen: true,
    dimmed: !enabled,
    trailingControlWidth: ImGuiWidget.GetIconButtonSize().X,
    drawContextMenu: DrawHeaderContextMenu);
```

`dimmed` 为 `true` 时，leading control、标题和 trailing control 使用与 Hierarchy inactive GameObject 相同的灰色文本色。GameBehavior 与 GameSystem Inspector 已把该参数绑定到各自的 `enabled` 状态。

Header 的 disclosure triangle 由 `DrawDisclosureIndicator` 统一绘制：保留 `▶ / ▼` glyph，并根据实际 header bounds 居中。卡片、disabled text 与 disclosure hover 颜色都来自 `EditorPalette`，便于主题统一替换。底层 TreeNode 仍负责 open state 和点击命中，因此没有第二套折叠状态。

`trailingControlWidth` 可以为多个右侧按钮预留固定宽度。Component 与 System Inspector 使用它放置 Reset 与 Remove；Transform 不可移除，因此只显示 Reset。

`drawContextMenu` 在完整 Header TreeNode 仍是当前 ImGui item 时执行，因此右键命中覆盖整个 Header，而不会错误绑定到 enabled checkbox、标题或末尾按钮。Component、Transform 与 System 都使用相同入口。

Component（包含 Transform）与 GameSystem 的排序拖拽也绑定在这个完整 Header item 上。拖动时 tooltip 使用相同 header surface、drag grip、标题和 dimmed text，明确表现被提起的是整张卡片。Drop target 使用 header 到展开 body 底部的完整矩形；目标展开时，“插入到后面”的黄色 insertion line 位于整个 body 下方，而不是 header 下方。Inspector 调用 `DragDropSource(..., allowHoldToOpenOthers: false)`，由 ImGui 实现内部映射为 `SourceNoHoldToOpenOthers`，禁止 TreeNode 在 drag-hover 超时后自动展开，也不向上层暴露 native flag。payload 只携带 generation-safe `RuntimeIdentity`，preview 和 delivery 都重新解析 live object，最后仍调用 `SceneEdits.SetComponentIndex` / `SetSystemIndex`，所以排序继续进入同一 Undo/Redo 历史。

`DragDropTarget<TPayload>(string payloadType, Vector2 minimum, Vector2 maximum, uint targetId, out TPayload payload, out bool isPreviewing, bool drawDefaultHighlight = true)` 是可复用的显式矩形 drop target：它允许调用方把 header 与展开 body 合并成一个命中区，并在关闭默认 highlight 后绘制统一 insertion line。

`CenteredWrappedText` 在调用方提供的完整区域内按水平/垂直 padding 计算换行宽度，将整个文本块居中，并以同一 padded rectangle 裁剪。Scene View 与 Game View 的 Provider 缺失、隔离失败和 GPU target 准备提示统一使用该 primitive，因此长诊断不会贴边、越界或只停留在左上角。

展开内容使用配套的 `CardBody` 绘制：

```csharp
if (open)
{
    ImGuiWidget.CardBody(
        id,
        drawContent: DrawSerializedProperties,
        dimmed: !enabled);
    ImGui.TreePop();
}
```

`CardBody` 提供统一的背景、边框与内边距；`dimmed` 为 `true` 时，正文整体灰化且不可编辑，但 header 中的 enabled checkbox 仍可用于重新启用对象。`CollapsingCard` 的 header 自身也使用同色语义边框；展开时 header 与 body 使用同一左右边界并在同一 Y 坐标衔接，body 原有上边框继续作为明确的 header/content 分隔线。折叠时 header 保持完整独立外框。Card 的 full-width bounds 使用当前 window 的实际 padding，而不是全局默认值，因此零 padding Panel 不会被误判为横向溢出。Header title 在 leading/trailing 控件之间裁剪并提交固定可用宽度，长 GameBehavior/GameSystem 类型名不会扩大 window content size。相邻卡片之间的外部间距由调用方控制。

`PropertyRow` 使用 `SetupPropertyColumns()` 建立 2:3 的 label/input 列，`PropertyLabel()` 在当前列宽内自动换行；Settings 和 Export 的字段表复用同一列配置与标签绘制。自定义 label callback 仍可在同一列组合名称、类型 badge 或多行说明。向量属性的每个 axis field 同样按实际列宽收缩，不用固定 label 宽度反向撑大 Inspector。这些控件在宽窗口保持 2:3 比例，在窄窗口换行并增加行高，不创建人工 `ScrollMaxX`。`CompactDragFloat` 与 `CompactSliderFloat` 默认只呈现一位小数，并启用 `NoRoundToFormat` 保留底层精确值；双击（或 Ctrl+单击）使用同一 ImGui ID 进入九位有效数字文本编辑，所以 Vector、Rect、Quaternion、Transform 与普通 float 共享相同语义。

## 表单与滚动容器约定

一个视觉区域只能有一个滚动 owner。Modal 若直接绘制完整表单，由 Modal window 滚动；若使用固定工具栏和正文 Child，则外层窗口与承载 Child 的布局容器必须禁用滚动，由正文 Child 独占滚动。Panel 若用填满正文的 Child，也应设置 `allowScrolling = false`，避免 Panel 与 Child 同时出现滚动条。不要用固定内容高度、负 cursor 偏移或 `Always*Scrollbar` 掩盖测量错误；只有内容实际超出可用区域时才允许对应方向的滚动条。新增或改动布局时，在小窗口、长标签、长输入、不同 zoom 下检查纵向与横向 `ScrollMax`，确认没有由 padding、表格最小列宽或父子容器重复造成的虚假滚动。

所有左侧标签、右侧输入的字段统一复用 `SetupPropertyColumns()` 和 `PropertyLabel()`，剩余宽度以 2:3 分配给标签与输入；有固定操作列时，先扣除操作列再按 2:3 分配。标签必须在本列换行，行高由真实内容确定，不能截断到邻列或用固定像素宽度替代比例。

其余常用控件包括 `SearchInput`、`BeginSearchPopup`/`EndSearchPopup`、`BeginMenuPopup`/`EndMenuPopup`、`BeginMenuTooltip`/`EndMenuTooltip`、`BeginBoundedCombo`/`EndBoundedCombo`、`BeginMenuSelector`/`EndMenuSelector`、`InlineRename`、`IconButton`、`Checkbox`、`CompactCheckbox`、`MetadataValue`、`LabelChip`、`TypeBadge`、`CenteredButton`、`CenteredProgressBar` 和 `WrappedText`。`Checkbox` 与 `CompactCheckbox` 都在控件自身的 hover item 上接受可选 tooltip，调用方不需要在每个 header 或 PropertyDrawer 中重复手写 hover 检测。`MetadataValue` 把类型、来源等短 metadata 保留在右侧 value column，可选择后接文字或交互控件，避免把类型 badge 污染到属性 label。`BeginBoundedCombo` / `EndBoundedCombo` 为普通 Enum、索引、Asset、Scene Object 与节点选择器提供统一的下方弹层：宽度由触发控件确定，并限制在所属窗口可用宽度内；高度不超过所属窗口下方剩余空间与该窗口高度的 45%。内部 `SearchInput(-1)` 填满固定内宽，仅在实际内容超过上限时显示纵向 scrollbar。`LabelChip` 与 `GetLabelChipSize` 共用全局 padding/rounding，给紧凑的非交互标签提供柔和彩色背景；`TypeBadge` 与 `GetTypeBadgeSize` 仍可用于确实需要独立语义色图例的界面，但普通属性类型应优先使用 `MetadataValue`。调用方无需分别估算背景、边框与文字宽度。`BeginMenuSelector(id, preview, width, minimumPopupWidth)` 使用独立箭头按钮区并遵循相同的下方定位与窗口边界；宽度取 `width` 与 `minimumPopupWidth` 的较大值，再按窗口剩余宽度裁剪。超长文字由 clip/tooltip 处理，不会反向撑宽 Popup。`WrappedText` 通过 wrap scope 与 `TextUnformatted` 绘制 literal text，不经过 native variadic formatting ABI，适合诊断、说明文字和来自 Asset 的内容。`Hint` 同样使用当前内容宽度换行，不再要求 Panel 预估单行长度。`CenteredProgressBar` 使用原生进度填充，但把 overlay 独立绘制在完整 bar 的几何中心，因此百分比不会跟随填充边缘移动。每个组件位于对应的 `ImGuiWidget.<Component>.cs`，避免继续形成一个混合所有控件的 EditorControls 文件。`GetGlyphVisualBounds` 与 `AddGlyphCentered` 使用 baked font 的 glyph bearing，而不是字符串 advance rectangle，适合把不对称 icon glyph 按实际可见轮廓居中；`ClickableIcon` 同样按 glyph 可见边界居中，而不是按 advance rectangle 估算。`InlineRename` 不缩放字体；它直接在调用方当前内容层绘制原生输入框，使用统一的紧凑 frame padding、rounding 与 border，并在 `rowHeight` 内垂直居中。该控件只对自身隐藏原生向外扩展 4px 的 nav cursor，并沿实际输入框外扩 1px 重画焦点线框。焦点线框与 DropTarget/InsertionLine 共用 `interactionOverlayThickness`，并绘制到 foreground draw list，因此始终覆盖 Table、Tree、Grid 的 highlight、分隔线和后续普通内容。首次请求焦点时控件会显式全选当前值；其结果明确区分 Enter `Commit`、`FocusLost` 与 Escape `Cancel`，因此 feature 可以为校验失败定义一致的收尾规则。所有需要 identity 的控件都应传入稳定且在当前 ImGui scope 内唯一的 `id`。

## Tree 与拖拽反馈

`TreeNode` 统一负责整行 hit area、hover/selection 背景和树连接线。内容回调会收到 `TreeNodeDrawContext`，其中的 `rowHeight` 来自当前原生 TreeNode 的实际几何，可用于在任意 zoom 下精确对齐行内控件。有子节点的行既可单击 disclosure arrow 展开，也可双击文字/图标所在的 content hit area 切换展开状态；双击内容不会同时命中箭头，leaf 也不会产生虚假展开状态。状态在下一帧应用，以保持当前帧 native TreePush/TreePop 完整对称。`TreeNodeResult.min/max` 表示整行几何，`contentMin` 表示排除层级缩进与箭头后的真实内容起点；行右边界使用当前 window 的 `WorkRect.Max.X`，不会因为外部 padding 人为制造 `ScrollMaxX`。Tree 内容 offset 从未滚动的 window/group 坐标计算，ImGui 只会应用一次 `ScrollX`，因此文字、图标和交互区始终保持同一坐标系。整行 invisible hit area 不参与 `CursorMaxPos/IdealMaxPos`；固定到可视区域右侧的控件应通过 `TreeNodeOptions.drawViewportOverlay` 绘制，它同样不扩大内容边界。Panel 从宽变窄后，水平范围因此会重新收敛到名称和层级缩进的真实最小容纳宽度，不会保留旧 viewport 宽度。Tree guide 使用 ImGui window 的真实 `TreeDepth` 建立 parent stack，不通过缩放后的 X 坐标猜测层级；guide 在原生 TreeNode 提交前按当前 frame 的行起点绘制，不读取上一帧缓存。父子纵线从父行底部与当前子行顶部之间开始，使 disclosure triangle 下方保留明确空隙，不再穿过 triangle 或与同层横线重叠；末端在当前行中心或底部结束，不增加 entry 高度。每个 Tree row 使用两个 draw-list channel：guide、文字、图标和交互控件进入前景 channel，等本行 hover/selection/自定义背景状态确定后，背景矩形以同一帧的最终几何进入后景 channel；合并后仍保持背景在内容下面。Tree 因此不保存上一帧的 guide 或背景矩形，drag/drop、scroll、zoom、窗口移动或尺寸变化都不会产生被丢弃几何造成的空白帧和闪烁。Hierarchy 的 child-target 框使用 `contentMin..max`，因此不会覆盖左侧 Tree guide/indent 区域。拖拽期间普通 hover 背景会暂停，调用方可用 `InsertionLine` 表示同级插入，或用 `DropTargetHighlight` 表示成为目标的 child。

Tree 行高采用紧凑的原生 `TreeNode` 内容高度；Hierarchy 通过可缩放的 `hierarchyItemSpacing` 控制 Scene/GameObject 行距，不以额外 frame padding 增高栏目。`DropTargetHighlight` 绘制到当前 viewport 的 foreground draw list，因此目标框不会被发起它的 Panel clip rect 截断。

Scripting facade 同时导出 `TreeNodeDrawContext`、`TreeNodeOptions`、`TreeNodeResult` 与可调用的 `TreeNode` 入口；content callback 必须接收 draw context。native 指针/内部布局 helper 通过显式 member-level ignore 留在 host，不会因为 signature closure 被误导出。

`IconText` 使用 baked glyph 的可见边界把 icon 轮廓放在 slot 中心。字体注册与自定义方式见 [Platform ImGui](../backends/ImGui/Inno.Adapter.Presentation.ImGui.Sdl3.md)。

`DragDropTarget(..., drawDefaultHighlight: false)` 可关闭 ImGui 默认目标框，适合需要按鼠标在行内位置绘制互斥反馈的复合目标。

`EditorDragDropRenderer` 向 native payload 写入源对象的 runtime identity。Preview 与 Delivery 从所属 `IdentityAllocator` 重新解析并检查 generation，落盘及 History 使用 persistent identity。Panel 与 Widget 的公开调用不要求 `unsafe`，FileBrowser 的 Grid 文本和搜索框也只使用安全的 Widget/ImGui API。

## 原生布局回归测试

[`Inno.Editor.ImGui.Tests`](../../tests/editor/Inno.Editor.ImGui.Tests/WidgetLayoutTests.cs) 使用真实 ImGui context 和公开 Widget API，通过鼠标事件打开弹层并测量稳定后的布局。覆盖三档缩放下的 2:3 列、长标签换行、短列表无滚动、长列表高度边界、菜单搜索填满内容宽度，以及重叠窗口仅前景控件接收点击。测试宿主必须部署与当前绑定一致的 native 目录；测试不依赖桌面解锁或截图推测内部布局。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Editor.ImGui.EditorDragDropRenderer`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.ImGui.EditorDragDropRenderer`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Runtime/EditorDragDropRenderer.cs#L14) | Bridges managed editor drag sessions to the native ImGui payload API. |
| [`static Inno.Editor.ImGui.EditorDropWidgetResult Inno.Editor.ImGui.EditorDragDropRenderer.Target(Inno.Editor.Interactions.EditorInteraction interaction, Inno.Editor.Interactions.EditorDropPlacement placement = Inno.Editor.Interactions.EditorDropPlacement.None)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Runtime/EditorDragDropRenderer.cs#L57) | Evaluates and, on delivery, accepts a managed drag on the most recently submitted ImGui item. |
| [`static bool Inno.Editor.ImGui.EditorDragDropRenderer.Source(Inno.Editor.Interactions.EditorInteraction interaction, Inno.Editor.Interactions.EditorDragData data, System.Action? drawPreview = null)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Runtime/EditorDragDropRenderer.cs#L33) | Publishes managed drag data for the most recently submitted ImGui item. |

### `Inno.Editor.ImGui.EditorDropWidgetResult`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.ImGui.EditorDropWidgetResult`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Runtime/EditorDragDropRenderer.cs#L87) | Reports the preview and delivery state of an ImGui editor drop target. |
| [`Inno.Editor.ImGui.EditorDropWidgetResult.EditorDropWidgetResult(bool isPreviewing, Inno.Editor.Interactions.EditorDropStatus status, Inno.Editor.Interactions.EditorDropResult result)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Runtime/EditorDragDropRenderer.cs#L101) | Creates a combined native-preview and managed-drop result. |
| [`Inno.Editor.Interactions.EditorDropResult Inno.Editor.ImGui.EditorDropWidgetResult.result`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Runtime/EditorDragDropRenderer.cs#L124) | Gets the delivered drop result. |
| [`Inno.Editor.Interactions.EditorDropStatus Inno.Editor.ImGui.EditorDropWidgetResult.status`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Runtime/EditorDragDropRenderer.cs#L119) | Gets the managed drop compatibility status. |
| [`bool Inno.Editor.ImGui.EditorDropWidgetResult.isPreviewing`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Runtime/EditorDragDropRenderer.cs#L114) | Gets whether a compatible native payload is hovering the target. |
| [`static Inno.Editor.ImGui.EditorDropWidgetResult Inno.Editor.ImGui.EditorDropWidgetResult.none`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Runtime/EditorDragDropRenderer.cs#L129) | Gets an inactive drop target result. |

### `Inno.Editor.ImGui.EditorGameInputCapture`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Events.Event? Inno.Editor.ImGui.EditorGameInputCapture.Route(Inno.Core.Events.Event evnt)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Runtime/EditorGameInputCapture.cs#L126) | Filters an engine event through the current Game View focus and pointer bounds. |
| [`Inno.Editor.ImGui.EditorGameInputCapture`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Runtime/EditorGameInputCapture.cs#L13) | Routes platform input to the Play session only while the visible Game View owns focus. |
| [`Inno.Editor.ImGui.EditorGameInputCapture.EditorGameInputCapture(System.Func<uint, uint?> resolveWindow)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Runtime/EditorGameInputCapture.cs#L35) | Creates a capture policy using a presentation viewport to platform window resolver. |
| [`bool Inno.Editor.ImGui.EditorGameInputCapture.Report(uint viewportId, System.Numerics.Vector2 origin, System.Numerics.Vector2 size, bool focused, bool hovered)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Runtime/EditorGameInputCapture.cs#L66) | Reports the Game View image in coordinates local to its platform window. |
| [`uint Inno.Editor.ImGui.EditorGameInputCapture.CompleteFrame()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Runtime/EditorGameInputCapture.cs#L89) | Ends a presentation pass and identifies input that must be released after focus loss. |
| [`void Inno.Editor.ImGui.EditorGameInputCapture.BeginFrame()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Runtime/EditorGameInputCapture.cs#L43) | Starts a presentation pass in which the visible Game View must report its current bounds. |

### `Inno.Editor.ImGui.EditorMenuRenderer`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.ImGui.EditorMenuRenderer`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Runtime/EditorMenuRenderer.cs#L17) | Renders immutable editor menu models through ImGui. |
| [`static bool Inno.Editor.ImGui.EditorMenuRenderer.ContextMenu(string id, Inno.Editor.Interactions.EditorInteraction interaction)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Runtime/EditorMenuRenderer.cs#L38) | Draws a resolved right-click menu for the most recently submitted ImGui item. |
| [`static bool Inno.Editor.ImGui.EditorMenuRenderer.ContextMenu(string id, Inno.Editor.Interactions.EditorInteraction scopeInteraction, Inno.Editor.Interactions.EditorInteraction itemInteraction)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Runtime/EditorMenuRenderer.cs#L80) | Draws one context popup composed from an item interaction and its containing-scope interaction. |
| [`static bool Inno.Editor.ImGui.EditorMenuRenderer.DrawSearchItems(Inno.Editor.Interactions.EditorInteraction interaction, System.Collections.Generic.IReadOnlyList<Inno.Editor.Interactions.EditorMenuItem> items, string search)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Runtime/EditorMenuRenderer.cs#L379) | Draws matching leaf commands as a flat searchable list. |
| [`static bool Inno.Editor.ImGui.EditorMenuRenderer.WindowContextMenu(string id, Inno.Editor.Interactions.EditorInteraction interaction)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Runtime/EditorMenuRenderer.cs#L126) | Draws a resolved right-click menu when the current ImGui window's unoccupied background is clicked. |
| [`static void Inno.Editor.ImGui.EditorMenuRenderer.DrawItems(Inno.Editor.Interactions.EditorInteraction interaction, System.Collections.Generic.IReadOnlyList<Inno.Editor.Interactions.EditorMenuItem> items)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Runtime/EditorMenuRenderer.cs#L322) | Recursively draws resolved menu nodes into the currently open popup or menu. |
| [`static void Inno.Editor.ImGui.EditorMenuRenderer.MainMenu(Inno.Editor.Interactions.EditorInteraction interaction)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Runtime/EditorMenuRenderer.cs#L216) | Draws the complete editor main menu bar for the supplied menu context. |

### `Inno.Editor.ImGui.EditorPalette`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.ImGui.EditorPalette`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L11) | Defines the complete editor color palette in one theme surface. |
| [`const float Inno.Editor.ImGui.EditorPalette.opacityEmphasized`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L61) | Gets emphasized opacity for borders and interactive grips. |
| [`const float Inno.Editor.ImGui.EditorPalette.opacityFaint`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L26) | Gets faint opacity for quiet guides and separators. |
| [`const float Inno.Editor.ImGui.EditorPalette.opacityMedium`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L46) | Gets medium opacity for secondary content and overlays. |
| [`const float Inno.Editor.ImGui.EditorPalette.opacityMuted`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L41) | Gets muted opacity for shadows and subdued outlines. |
| [`const float Inno.Editor.ImGui.EditorPalette.opacityNearOpaque`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L66) | Gets near-opaque opacity for active overlays. |
| [`const float Inno.Editor.ImGui.EditorPalette.opacityNone`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L16) | Gets zero opacity for invisible surfaces. |
| [`const float Inno.Editor.ImGui.EditorPalette.opacityOpaque`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L76) | Gets full opacity. |
| [`const float Inno.Editor.ImGui.EditorPalette.opacityPopup`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L71) | Gets popup opacity, retaining only enough transparency for depth. |
| [`const float Inno.Editor.ImGui.EditorPalette.opacityProminent`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L56) | Gets prominent opacity for navigation emphasis. |
| [`const float Inno.Editor.ImGui.EditorPalette.opacitySoft`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L36) | Gets soft opacity for tinted container surfaces. |
| [`const float Inno.Editor.ImGui.EditorPalette.opacityStrong`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L51) | Gets strong opacity for accents that remain translucent. |
| [`const float Inno.Editor.ImGui.EditorPalette.opacitySubtle`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L31) | Gets subtle opacity for selection washes and low-emphasis overlays. |
| [`const float Inno.Editor.ImGui.EditorPalette.opacityTrace`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L21) | Gets trace opacity for barely perceptible alternating surfaces. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.GetActive(System.Numerics.Vector4 color)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L458) | Gets an active treatment derived from a base theme color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.GetHovered(System.Numerics.Vector4 color)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L447) | Gets a hover treatment derived from a base theme color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.GetLogExpandedBorder(System.Numerics.Vector4 severityColor)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L575) | Gets the expanded log card border derived from a severity color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.GetLogExpandedCard(System.Numerics.Vector4 severityColor)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L564) | Gets the expanded log card background derived from a severity color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.GetLogSeparator(System.Numerics.Vector4 cardColor)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L586) | Gets the separator color used inside an expanded log card. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.Lerp(System.Numerics.Vector4 from, System.Numerics.Vector4 to, float amount)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L603) | Linearly interpolates two palette colors using a clamped amount. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.WithOpacity(System.Numerics.Vector4 color, float opacity)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L686) | Returns a theme color with a caller-selected opacity, normally one of the shared opacity levels. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.accent`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L216) | Gets the standard accent color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.accentActive`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L226) | Gets the active accent color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.accentHovered`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L221) | Gets the hovered accent color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.assetAccent`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L436) | Gets the opaque asset browser accent. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.assetBorder`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L411) | Gets the asset browser border. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.assetBorderSoft`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L416) | Gets the soft asset browser border. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.assetBreadcrumbText`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L431) | Gets the subdued text color used by asset browser breadcrumb paths. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.assetField`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L406) | Gets the asset browser field background. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.assetText`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L421) | Gets the asset browser text color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.assetTextMuted`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L426) | Gets the muted asset browser text color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.axisW`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L493) | Gets W axis color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.axisX`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L478) | Gets X axis color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.axisY`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L483) | Gets Y axis color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.axisZ`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L488) | Gets Z axis color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.border`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L161) | Gets the standard border color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.borderShadow`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L166) | Gets the standard border shadow color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.collectionHeader`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L381) | Gets the deepest collection background. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.collectionRow`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L386) | Gets the primary collection row background. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.collectionRowAlternate`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L391) | Gets the alternate collection row background. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.compactControlHovered`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L376) | Gets compact control hover color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.dragDropTarget`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L281) | Gets the standard drag target color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.error`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L96) | Gets the standard error color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.frame`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L171) | Gets the standard frame background. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.frameActive`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L181) | Gets the active frame background. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.frameHovered`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L176) | Gets the hovered frame background. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.hierarchyInactiveText`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L468) | Gets inactive hierarchy text. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.hierarchySceneRow`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L463) | Gets scene row background. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.inspectorCardBody`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L326) | Gets inspector card body background. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.inspectorCardBodyBorder`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L331) | Gets inspector card body border. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.inspectorCardDisabledText`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L366) | Gets disabled inspector card text. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.inspectorCardDisclosureHovered`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L371) | Gets inspector disclosure hover background. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.inspectorCardHeader`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L301) | Gets inspector card header background. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.inspectorLayerLabel`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L321) | Gets the subdued background of Layer labels in Inspector target headers. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.inspectorSectionBorder`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L336) | Gets the outline color of framed Inspector sections. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.inspectorTagLabel`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L316) | Gets the subdued background of Tag labels in Inspector target headers. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.inspectorTargetHeader`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L306) | Gets the persistent Inspector target header background. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.inspectorTargetHeaderBorder`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L311) | Gets the persistent Inspector target header border. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.logCollapsedBorder`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L528) | Gets collapsed log card border. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.logCollapsedCard`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L523) | Gets collapsed log card background. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.logDebug`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L498) | Gets debug log color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.logError`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L513) | Gets error log color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.logExpandedBase`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L533) | Gets the base expanded log card background. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.logExpandedBorderBase`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L538) | Gets the base expanded log card border. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.logFatal`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L518) | Gets fatal log color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.logInfo`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L503) | Gets informational log color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.logToggle`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L543) | Gets log header button background. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.logToggleActive`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L553) | Gets active log header button background. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.logToggleHovered`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L548) | Gets hovered log header button background. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.logWarning`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L508) | Gets warning log color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.menuBackground`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L136) | Gets the background used by editor context menus. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.menuItem`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L141) | Gets the resting background of an editor context-menu item. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.menuItemActive`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L151) | Gets the active background of an editor context-menu item. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.menuItemHovered`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L146) | Gets the hovered background of an editor context-menu item. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.menuSeparator`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L156) | Gets the separator color used by editor context menus. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.menuText`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L131) | Gets text color used by editor context menus. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.modalDim`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L296) | Gets modal dim background. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.navigationDim`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L291) | Gets navigation dim background. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.navigationHighlight`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L286) | Gets navigation highlight color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.popupBackground`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L126) | Gets the editor popup background. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.scrollbarGrab`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L201) | Gets the standard scrollbar thumb. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.scrollbarGrabActive`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L211) | Gets the active scrollbar thumb. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.scrollbarGrabHovered`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L206) | Gets the hovered scrollbar thumb. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.settingsField`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L396) | Gets the translucent primary background of a complete Settings field. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.settingsFieldAlternate`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L401) | Gets the translucent alternate background of a complete Settings field. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.shaderInputNodeHeader`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L111) | Gets the orange heading surface shared by Shader input nodes. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.shaderNodeHeader`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L106) | Gets the neutral Shader node heading surface. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.shaderOutputNodeHeader`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L116) | Gets the purple heading surface shared by Shader output nodes. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.shaderTypeBoolean`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L351) | Gets the accent used by Boolean Shader type badges. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.shaderTypeInteger`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L346) | Gets the accent used by integer Shader type badges. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.shaderTypeNumeric`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L341) | Gets the accent used by numeric Shader type badges. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.shaderTypeOther`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L361) | Gets the accent used by unclassified Shader type badges. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.shaderTypeResource`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L356) | Gets the accent used by Shader resource type badges. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.tab`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L231) | Gets the standard tab color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.tabDimmed`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L246) | Gets the dimmed tab color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.tabDimmedSelected`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L251) | Gets the selected dimmed tab color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.tabHovered`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L236) | Gets the hovered tab color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.tabSelected`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L241) | Gets the selected tab color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.tabSelectedOverline`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L256) | Gets the selected tab overline color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.tableBorderLight`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L271) | Gets the light table border. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.tableBorderStrong`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L266) | Gets the strong table border. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.tableHeader`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L261) | Gets the table header background. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.tableRowAlternate`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L276) | Gets the alternate table row background. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.text`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L86) | Gets the primary text color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.textDisabled`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L91) | Gets disabled text color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.title`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L186) | Gets the inactive title background. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.titleActive`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L191) | Gets the active title background. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.titleCollapsed`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L196) | Gets the collapsed title background. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.transparent`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L81) | Gets fully transparent color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.treeGuide`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L473) | Gets hierarchy tree guide color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.warning`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L101) | Gets the standard warning color. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.EditorPalette.windowBackground`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorPalette.cs#L121) | Gets the editor window background. |

### `Inno.Editor.ImGui.EditorStyleMetrics`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.ImGui.EditorStyleMetrics`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L9) | Defines named editor layout metrics used by widgets and feature panels. |
| [`System.Numerics.Vector2 Inno.Editor.ImGui.EditorStyleMetrics.assetCellPadding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L237) | Gets asset browser cell padding. |
| [`System.Numerics.Vector2 Inno.Editor.ImGui.EditorStyleMetrics.assetItemSpacing`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L242) | Gets asset browser item spacing. |
| [`System.Numerics.Vector2 Inno.Editor.ImGui.EditorStyleMetrics.assetWindowPadding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L232) | Gets asset browser window padding. |
| [`System.Numerics.Vector2 Inno.Editor.ImGui.EditorStyleMetrics.breadcrumbFramePadding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L192) | Gets breadcrumb frame padding. |
| [`System.Numerics.Vector2 Inno.Editor.ImGui.EditorStyleMetrics.cellPadding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L222) | Gets standard table cell padding. |
| [`System.Numerics.Vector2 Inno.Editor.ImGui.EditorStyleMetrics.compactFramePadding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L177) | Gets compact frame padding. |
| [`System.Numerics.Vector2 Inno.Editor.ImGui.EditorStyleMetrics.compactItemSpacing`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L207) | Gets compact vertical item spacing. |
| [`System.Numerics.Vector2 Inno.Editor.ImGui.EditorStyleMetrics.framePadding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L136) | Gets standard frame padding. |
| [`System.Numerics.Vector2 Inno.Editor.ImGui.EditorStyleMetrics.hierarchyItemSpacing`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L212) | Gets hierarchy item spacing. |
| [`System.Numerics.Vector2 Inno.Editor.ImGui.EditorStyleMetrics.inlineRenameFramePadding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L182) | Gets the compact inner padding of inline rename fields. |
| [`System.Numerics.Vector2 Inno.Editor.ImGui.EditorStyleMetrics.inspectorCardBodyPadding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L342) | Gets inspector card body padding. |
| [`System.Numerics.Vector2 Inno.Editor.ImGui.EditorStyleMetrics.inspectorCardHeaderPadding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L347) | Gets inspector card header padding. |
| [`System.Numerics.Vector2 Inno.Editor.ImGui.EditorStyleMetrics.inspectorSectionPadding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L357) | Gets the inner padding of a framed Inspector section. |
| [`System.Numerics.Vector2 Inno.Editor.ImGui.EditorStyleMetrics.inspectorTargetHeaderPadding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L327) | Gets the inner padding of the persistent Inspector target header. |
| [`System.Numerics.Vector2 Inno.Editor.ImGui.EditorStyleMetrics.itemInnerSpacing`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L217) | Gets standard inner item spacing. |
| [`System.Numerics.Vector2 Inno.Editor.ImGui.EditorStyleMetrics.itemSpacing`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L202) | Gets standard item spacing. |
| [`System.Numerics.Vector2 Inno.Editor.ImGui.EditorStyleMetrics.labelChipPadding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L427) | Gets the inner padding of compact colored label chips. |
| [`System.Numerics.Vector2 Inno.Editor.ImGui.EditorStyleMetrics.logDisclosurePadding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L402) | Gets log disclosure padding. |
| [`System.Numerics.Vector2 Inno.Editor.ImGui.EditorStyleMetrics.menuFramePadding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L151) | Gets the uniform item padding of editor context menus. |
| [`System.Numerics.Vector2 Inno.Editor.ImGui.EditorStyleMetrics.menuItemSpacing`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L162) | Gets the uniform spacing between editor context-menu items. |
| [`System.Numerics.Vector2 Inno.Editor.ImGui.EditorStyleMetrics.menuSearchFramePadding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L157) | Gets the compact padding used by the search field at the top of editor context menus. Its vertical padding matches the half-spacing that menu entries add above and below a line. |
| [`System.Numerics.Vector2 Inno.Editor.ImGui.EditorStyleMetrics.menuWindowPadding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L146) | Gets the uniform content padding of editor context menus. |
| [`System.Numerics.Vector2 Inno.Editor.ImGui.EditorStyleMetrics.panelTabFramePadding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L141) | Gets the frame padding used while docked panel tabs are laid out. |
| [`System.Numerics.Vector2 Inno.Editor.ImGui.EditorStyleMetrics.sectionHeaderPadding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L352) | Gets the shared padding around separator-style section titles. |
| [`System.Numerics.Vector2 Inno.Editor.ImGui.EditorStyleMetrics.settingsFieldPadding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L227) | Gets the inner padding applied to one complete Settings field. |
| [`System.Numerics.Vector2 Inno.Editor.ImGui.EditorStyleMetrics.toolbarFramePadding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L187) | Gets toolbar frame padding. |
| [`System.Numerics.Vector2 Inno.Editor.ImGui.EditorStyleMetrics.typeBadgePadding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L437) | Gets the compact inner padding of outlined type badges. |
| [`System.Numerics.Vector2 Inno.Editor.ImGui.EditorStyleMetrics.windowMinimumSize`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L131) | Gets minimum window size. |
| [`System.Numerics.Vector2 Inno.Editor.ImGui.EditorStyleMetrics.windowPadding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L116) | Gets standard window padding. |
| [`bool Inno.Editor.ImGui.EditorStyleMetrics.ResetZoom()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L101) | Restores the baseline editor UI zoom. |
| [`bool Inno.Editor.ImGui.EditorStyleMetrics.SetCompactMode(bool value)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L48) | Switches between comfortable and compact editor density. |
| [`bool Inno.Editor.ImGui.EditorStyleMetrics.SetZoom(float value)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L68) | Sets the editor UI zoom after clamping it to the supported range. |
| [`bool Inno.Editor.ImGui.EditorStyleMetrics.ZoomIn()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L85) | Increases editor UI zoom by one bounded increment. |
| [`bool Inno.Editor.ImGui.EditorStyleMetrics.ZoomOut()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L93) | Decreases editor UI zoom by one bounded increment. |
| [`bool Inno.Editor.ImGui.EditorStyleMetrics.isCompact`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L37) | Gets whether compact editor density is active. |
| [`const float Inno.Editor.ImGui.EditorStyleMetrics.C_MAX_ZOOM`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L19) | Defines the largest supported editor UI zoom. |
| [`const float Inno.Editor.ImGui.EditorStyleMetrics.C_MIN_ZOOM`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L14) | Defines the smallest supported editor UI zoom. |
| [`const float Inno.Editor.ImGui.EditorStyleMetrics.C_ZOOM_STEP`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L24) | Defines one keyboard or menu zoom increment. |
| [`double Inno.Editor.ImGui.EditorStyleMetrics.modalFadeInSeconds`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L597) | Gets the modal fade-in duration in seconds. |
| [`double Inno.Editor.ImGui.EditorStyleMetrics.modalFadeOutSeconds`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L607) | Gets the modal fade-out duration in seconds. |
| [`double Inno.Editor.ImGui.EditorStyleMetrics.modalMinimumVisibleSeconds`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L602) | Gets the minimum modal visibility duration in seconds. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.assetBreadcrumbHeight`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L287) | Gets asset breadcrumb bar height. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.assetBreadcrumbSpacing`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L562) | Gets spacing around breadcrumb separators. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.assetFrameRounding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L247) | Gets asset browser frame rounding. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.assetGridCellPadding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L297) | Gets asset grid cell padding. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.assetGridDefaultScale`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L302) | Gets default asset grid scale. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.assetGridFixedCellPadding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L537) | Gets fixed padding added to calculated asset grid cells. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.assetGridIconHorizontalPadding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L552) | Gets the horizontal inset that constrains an asset grid icon inside its card. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.assetGridIconLabelSpacing`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L557) | Gets the vertical spacing between an asset grid icon and its label. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.assetGridIconTopPadding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L547) | Gets the top inset reserved above an asset grid icon. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.assetGridLabelBottomPadding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L527) | Gets bottom padding for asset grid labels. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.assetGridLabelHorizontalPadding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L522) | Gets horizontal padding for asset grid labels. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.assetGridLabelLineSpacing`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L532) | Gets the additional vertical spacing between asset grid label lines. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.assetGridMaximumScale`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L312) | Gets maximum asset grid scale. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.assetGridMinimumCellSize`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L292) | Gets minimum asset grid cell size. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.assetGridMinimumScale`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L307) | Gets minimum asset grid scale. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.assetGridScaleBias`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L542) | Gets the scale bias added to asset grid icons. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.assetListContentHorizontalPadding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L517) | Gets the horizontal inset between an asset list separator and column content. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.assetListMinimumColumnRatio`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L507) | Gets the minimum normalized width reserved for each asset list column. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.assetListNameSeparatorPosition`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L497) | Gets the default normalized position of the asset list name/type separator. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.assetListRowSpacing`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L317) | Gets spacing between asset list rows. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.assetListSeparatorHitWidth`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L512) | Gets the horizontal hit width of an asset list column separator. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.assetListTypeSeparatorPosition`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L502) | Gets the default normalized position of the asset list type/source separator. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.assetPaneMinimumVisibleWidth`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L277) | Gets the minimum visible width retained for either asset browser pane while its splitter is dragged. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.assetSplitterMinimumWidth`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L282) | Gets the minimum draggable asset browser splitter width. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.assetToolbarSectionSpacing`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L492) | Gets spacing between asset browser toolbar sections. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.assetToolbarSpacing`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L487) | Gets the regular asset browser toolbar spacing. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.assetToolbarTightSpacing`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L482) | Gets the tight asset browser toolbar spacing. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.axisPrefixMinimumWidth`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L572) | Gets the minimum width of an axis prefix. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.axisPrefixWidthRatio`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L577) | Gets the axis prefix share of the complete control. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.axisValueMinimumWidth`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L567) | Gets the minimum width of one axis value field. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.borderSize`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L126) | Gets standard border thickness. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.columnMinimumSpacing`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L257) | Gets minimum column spacing. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.compactCheckboxSize`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L417) | Gets the compact checkbox size used by card headers. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.disabledAlpha`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L111) | Gets disabled content opacity. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.disclosureButtonInset`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L387) | Gets inspector disclosure inset. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.fontScale`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L106) | Gets global content scale. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.frameRounding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L197) | Gets standard frame rounding. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.grabMinimumSize`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L272) | Gets minimum grab size. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.hierarchyBlankMinimumHeight`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L477) | Gets the minimum height of the hierarchy blank drop area. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.hierarchyRenameMinimumWidth`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L467) | Gets the minimum inline hierarchy rename width. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.hierarchyRenameTrailingGap`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L472) | Gets the hierarchy rename trailing-control gap. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.iconLabelSpacing`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L412) | Gets spacing between a leading icon and its text. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.indentSpacing`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L252) | Gets tree indentation. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.inspectorAddButtonTopPadding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L462) | Gets top padding above inspector add buttons. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.inspectorCardSpacing`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L322) | Gets spacing between inspector cards. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.inspectorCollapsedSectionCapLength`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L367) | Gets the length of each centered end cap on a collapsed Inspector section. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.inspectorHeaderControlSpacing`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L422) | Gets spacing between leading controls inside card headers. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.inspectorHeaderSectionSpacing`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L457) | Gets spacing between distinct control groups in an Inspector target header row. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.inspectorSectionLegendGap`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L362) | Gets the gap between an Inspector section legend and its interrupted top border. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.inspectorSectionRounding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L382) | Gets the subtle corner rounding used by framed content sections inside editor containers. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.inspectorSectionSpacing`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L377) | Gets the vertical distance after a framed Inspector section. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.inspectorTargetHeaderRowSpacing`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L332) | Gets spacing between the two rows of the persistent Inspector target header. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.inspectorTargetIconScale`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L337) | Gets the font-size multiplier used by the large Inspector target icon. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.interactionOverlayThickness`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L582) | Gets the standard foreground interaction-overlay thickness. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.labelChipRounding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L432) | Gets the corner rounding of compact colored label chips. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.logAutoScrollTolerance`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L397) | Gets the distance from the bottom treated as auto-scroll. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.menuBorderSize`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L172) | Gets editor context-menu border thickness. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.menuRounding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L167) | Gets editor context-menu corner rounding. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.modalWidth`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L592) | Gets the standard centered modal width. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.propertyLabelRatio`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L392) | Gets property label width ratio. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.propertyMetadataSpacing`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L372) | Gets the spacing around the separator between inline metadata and its value. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.scriptCompilationWidth`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L587) | Gets script compilation modal width. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.scrollbarPadding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L267) | Gets the inset that keeps an overlay scrollbar grab visually lightweight. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.scrollbarSize`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L262) | Gets scrollbar width. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.searchPopupWidth`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L407) | Gets the default width of searchable editor popups. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.statisticRowPadding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L452) | Gets the vertical padding inside one statistics row. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.textDecorationOffset`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L627) | Gets the vertical offset of text decorations from the baseline. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.treeFolderConnectorPadding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L622) | Gets additional connector padding for expandable tree nodes. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.treeGuideLeftOffset`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L612) | Gets tree guide left offset. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.treeGuideLineOverlap`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L617) | Gets the overlap used to join adjacent tree guide segments. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.typeBadgeRounding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L442) | Gets the corner rounding of outlined type badges. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.typeBadgeSpacing`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L447) | Gets the horizontal gap between a property name and its type badge. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.windowRounding`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L121) | Gets standard window rounding. |
| [`float Inno.Editor.ImGui.EditorStyleMetrics.zoom`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Styling/EditorStyleMetrics.cs#L32) | Gets the current editor UI zoom multiplier. |

### `Inno.Editor.ImGui.ImGui`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.ImGui.ImGui`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L13) | Exposes a script-safe, pointer-free subset of Dear ImGui for custom Editor tools. Every call is valid only while an Editor drawing callback is active. |
| [`static System.Numerics.Vector2 Inno.Editor.ImGui.ImGui.GetContentRegionAvailable()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L740) | Gets the remaining content size in the current region. |
| [`static System.Numerics.Vector2 Inno.Editor.ImGui.ImGui.GetCursorPosition()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L748) | Gets the current local cursor position. |
| [`static System.Numerics.Vector2 Inno.Editor.ImGui.ImGui.GetCursorScreenPosition()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L764) | Gets the current screen-space cursor position. |
| [`static System.Numerics.Vector2 Inno.Editor.ImGui.ImGui.GetItemMaximum()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L780) | Gets the maximum screen-space corner of the previous item. |
| [`static System.Numerics.Vector2 Inno.Editor.ImGui.ImGui.GetItemMinimum()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L772) | Gets the minimum screen-space corner of the previous item. |
| [`static System.Numerics.Vector2 Inno.Editor.ImGui.ImGui.MeasureText(string text)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L791) | Measures text using the current font. |
| [`static bool Inno.Editor.ImGui.ImGui.BeginChild(string id, System.Numerics.Vector2 size = default(System.Numerics.Vector2), Inno.Native.ImGui.ImGuiChildFlags flags = Inno.Native.ImGui.ImGuiChildFlags.None)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L304) | Begins a child region. |
| [`static bool Inno.Editor.ImGui.ImGui.BeginCombo(string label, string preview, Inno.Native.ImGui.ImGuiComboFlags flags = Inno.Native.ImGui.ImGuiComboFlags.None)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L399) | Begins a combo popup. |
| [`static bool Inno.Editor.ImGui.ImGui.BeginPopup(string id, Inno.Native.ImGui.ImGuiWindowFlags flags = Inno.Native.ImGui.ImGuiWindowFlags.None)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L569) | Begins a named popup. |
| [`static bool Inno.Editor.ImGui.ImGui.BeginTabBar(string id, Inno.Native.ImGui.ImGuiTabBarFlags flags = Inno.Native.ImGui.ImGuiTabBarFlags.None)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L485) | Begins a tab bar. |
| [`static bool Inno.Editor.ImGui.ImGui.BeginTabItem(string label, Inno.Native.ImGui.ImGuiTabItemFlags flags = Inno.Native.ImGui.ImGuiTabItemFlags.None)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L507) | Begins one tab item. |
| [`static bool Inno.Editor.ImGui.ImGui.BeginTabItem(string label, ref bool isOpen, Inno.Native.ImGui.ImGuiTabItemFlags flags = Inno.Native.ImGui.ImGuiTabItemFlags.None)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L527) | Begins one closeable tab item. |
| [`static bool Inno.Editor.ImGui.ImGui.BeginTable(string id, int columns, Inno.Native.ImGui.ImGuiTableFlags flags = Inno.Native.ImGui.ImGuiTableFlags.None, System.Numerics.Vector2 size = default(System.Numerics.Vector2))`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L334) | Begins a table. |
| [`static bool Inno.Editor.ImGui.ImGui.Button(string label, System.Numerics.Vector2 size = default(System.Numerics.Vector2))`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L35) | Draws a standard button. |
| [`static bool Inno.Editor.ImGui.ImGui.Checkbox(string label, ref bool value)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L63) | Draws and edits a Boolean value. |
| [`static bool Inno.Editor.ImGui.ImGui.CollapsingHeader(string label, Inno.Native.ImGui.ImGuiTreeNodeFlags flags = Inno.Native.ImGui.ImGuiTreeNodeFlags.None)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L445) | Draws a collapsible section header. |
| [`static bool Inno.Editor.ImGui.ImGui.ColorEdit4(string label, ref System.Numerics.Vector4 value, Inno.Native.ImGui.ImGuiColorEditFlags flags = Inno.Native.ImGui.ImGuiColorEditFlags.None)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L243) | Draws and edits a four-component color. |
| [`static bool Inno.Editor.ImGui.ImGui.ColorEditLinear4(string label, ref System.Numerics.Vector4 value, Inno.Native.ImGui.ImGuiColorEditFlags flags = Inno.Native.ImGui.ImGuiColorEditFlags.None)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L270) | Displays a linear RGBA color through a display-sRGB picker and returns edits in linear space. |
| [`static bool Inno.Editor.ImGui.ImGui.DragFloat(string label, ref float value, float speed = 1, float minimum = 0, float maximum = 0)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L189) | Draws a floating-point drag control. |
| [`static bool Inno.Editor.ImGui.ImGui.InputFloat(string label, ref float value)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L163) | Draws and edits a floating-point value. |
| [`static bool Inno.Editor.ImGui.ImGui.InputInt(string label, ref int value)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L146) | Draws and edits an integer value. |
| [`static bool Inno.Editor.ImGui.ImGui.InputText(string label, ref string value, int capacity = 1024, Inno.Native.ImGui.ImGuiInputTextFlags flags = Inno.Native.ImGui.ImGuiInputTextFlags.None)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L89) | Draws and edits a bounded UTF-8 text value. |
| [`static bool Inno.Editor.ImGui.ImGui.InputTextWithHint(string label, string hint, ref string value, int capacity = 1024, Inno.Native.ImGui.ImGuiInputTextFlags flags = Inno.Native.ImGui.ImGuiInputTextFlags.None)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L123) | Draws and edits a bounded UTF-8 text value with placeholder text. |
| [`static bool Inno.Editor.ImGui.ImGui.IsItemActive()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L810) | Gets whether the previous item is active. |
| [`static bool Inno.Editor.ImGui.ImGui.IsItemClicked(Inno.Native.ImGui.ImGuiMouseButton button = Inno.Native.ImGui.ImGuiMouseButton.Left)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L821) | Gets whether the previous item was clicked with a mouse button. |
| [`static bool Inno.Editor.ImGui.ImGui.IsItemHovered(Inno.Native.ImGui.ImGuiHoveredFlags flags = Inno.Native.ImGui.ImGuiHoveredFlags.None)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L802) | Gets whether the previous item is hovered. |
| [`static bool Inno.Editor.ImGui.ImGui.Selectable(string label, bool selected = false, Inno.Native.ImGui.ImGuiSelectableFlags flags = Inno.Native.ImGui.ImGuiSelectableFlags.None)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L426) | Draws one selectable item. |
| [`static bool Inno.Editor.ImGui.ImGui.SliderFloat(string label, ref float value, float minimum, float maximum, Inno.Native.ImGui.ImGuiSliderFlags flags = Inno.Native.ImGui.ImGuiSliderFlags.None)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L219) | Draws a bounded floating-point slider. |
| [`static bool Inno.Editor.ImGui.ImGui.SmallButton(string label)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L49) | Draws a compact button. |
| [`static bool Inno.Editor.ImGui.ImGui.TableNextColumn()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L382) | Advances to the next table column. |
| [`static bool Inno.Editor.ImGui.ImGui.TreeNode(string label, Inno.Native.ImGui.ImGuiTreeNodeFlags flags = Inno.Native.ImGui.ImGuiTreeNodeFlags.None)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L463) | Draws a tree node and pushes its tree scope when it is open. |
| [`static uint Inno.Editor.ImGui.ImGui.ToPackedColor(System.Numerics.Vector4 color)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L832) | Converts a floating-point RGBA color to Dear ImGui packed color order. |
| [`static void Inno.Editor.ImGui.ImGui.BeginDisabled(bool disabled = true)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L590) | Begins a disabled block. |
| [`static void Inno.Editor.ImGui.ImGui.BeginGroup()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L600) | Begins an item group. |
| [`static void Inno.Editor.ImGui.ImGui.CloseCurrentPopup()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L582) | Closes the current popup. |
| [`static void Inno.Editor.ImGui.ImGui.DrawFilledRectangle(System.Numerics.Vector2 minimum, System.Numerics.Vector2 maximum, uint color, float rounding = 0)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L899) | Adds a filled rectangle to the current window draw list. |
| [`static void Inno.Editor.ImGui.ImGui.DrawLine(System.Numerics.Vector2 start, System.Numerics.Vector2 end, uint color, float thickness = 1)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L849) | Adds a line to the current window draw list. |
| [`static void Inno.Editor.ImGui.ImGui.DrawRectangle(System.Numerics.Vector2 minimum, System.Numerics.Vector2 maximum, uint color, float rounding = 0, float thickness = 1)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L875) | Adds a rectangle outline to the current window draw list. |
| [`static void Inno.Editor.ImGui.ImGui.DrawText(System.Numerics.Vector2 position, uint color, string text)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L919) | Adds text to the current window draw list. |
| [`static void Inno.Editor.ImGui.ImGui.Dummy(System.Numerics.Vector2 size)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L724) | Advances layout by an invisible size. |
| [`static void Inno.Editor.ImGui.ImGui.EndChild()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L314) | Ends the current child region. |
| [`static void Inno.Editor.ImGui.ImGui.EndCombo()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L409) | Ends the current combo popup. |
| [`static void Inno.Editor.ImGui.ImGui.EndDisabled()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L595) | Ends the current disabled block. |
| [`static void Inno.Editor.ImGui.ImGui.EndGroup()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L605) | Ends the current item group. |
| [`static void Inno.Editor.ImGui.ImGui.EndPopup()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L577) | Ends the current popup. |
| [`static void Inno.Editor.ImGui.ImGui.EndTabBar()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L493) | Ends the current tab bar. |
| [`static void Inno.Editor.ImGui.ImGui.EndTabItem()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L541) | Ends the current tab item. |
| [`static void Inno.Editor.ImGui.ImGui.EndTable()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L345) | Ends the current table. |
| [`static void Inno.Editor.ImGui.ImGui.OpenPopup(string id, Inno.Native.ImGui.ImGuiPopupFlags flags = Inno.Native.ImGui.ImGuiPopupFlags.None)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L552) | Opens a named popup. |
| [`static void Inno.Editor.ImGui.ImGui.PopId()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L626) | Pops one identity from the ImGui ID stack. |
| [`static void Inno.Editor.ImGui.ImGui.PopStyleColor(int count = 1)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L648) | Pops color overrides. |
| [`static void Inno.Editor.ImGui.ImGui.PopStyleVar(int count = 1)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L684) | Pops style-variable overrides. |
| [`static void Inno.Editor.ImGui.ImGui.PushId(int id)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L621) | Pushes an integer identity onto the ImGui ID stack. |
| [`static void Inno.Editor.ImGui.ImGui.PushId(string id)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L613) | Pushes a string identity onto the ImGui ID stack. |
| [`static void Inno.Editor.ImGui.ImGui.PushStyleColor(Inno.Native.ImGui.ImGuiCol color, System.Numerics.Vector4 value)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L637) | Pushes one color override. |
| [`static void Inno.Editor.ImGui.ImGui.PushStyleVar(Inno.Native.ImGui.ImGuiStyleVar style, System.Numerics.Vector2 value)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L673) | Pushes one two-component style override. |
| [`static void Inno.Editor.ImGui.ImGui.PushStyleVar(Inno.Native.ImGui.ImGuiStyleVar style, float value)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L659) | Pushes one scalar style override. |
| [`static void Inno.Editor.ImGui.ImGui.SameLine(float offset = 0, float spacing = -1)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L695) | Places the next item on the same line. |
| [`static void Inno.Editor.ImGui.ImGui.Separator()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L703) | Draws a horizontal separator. |
| [`static void Inno.Editor.ImGui.ImGui.SeparatorText(string label)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L711) | Draws a labeled horizontal separator. |
| [`static void Inno.Editor.ImGui.ImGui.SetCursorPosition(System.Numerics.Vector2 position)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L756) | Sets the current local cursor position. |
| [`static void Inno.Editor.ImGui.ImGui.SetNextItemWidth(float width)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L732) | Sets the width of the next item. |
| [`static void Inno.Editor.ImGui.ImGui.Spacing()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L716) | Adds one standard vertical spacing unit. |
| [`static void Inno.Editor.ImGui.ImGui.TableHeadersRow()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L369) | Draws the table header row from configured column labels. |
| [`static void Inno.Editor.ImGui.ImGui.TableNextRow()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L374) | Advances to the next table row. |
| [`static void Inno.Editor.ImGui.ImGui.TableSetupColumn(string label, Inno.Native.ImGui.ImGuiTableColumnFlags flags = Inno.Native.ImGui.ImGuiTableColumnFlags.None, float widthOrWeight = 0)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L359) | Declares one table column. |
| [`static void Inno.Editor.ImGui.ImGui.Text(string text)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L21) | Draws unformatted text. |
| [`static void Inno.Editor.ImGui.ImGui.TreePop()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/ImGui.cs#L471) | Ends the current open tree-node scope. |

### `Inno.Editor.ImGui.ImGuiEditorRuntime`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.ImGui.ImGuiEditorRuntime`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Runtime/ImGuiEditorRuntime.cs#L23) | Presents the backend-independent editor interaction runtime through ImGui. |
| [`Inno.Editor.ImGui.ImGuiEditorRuntime.ImGuiEditorRuntime(Inno.Editor.Core.EditorContext context, Inno.Extensibility.Types.TypeCatalog types, Inno.Core.Logging.LogRouter logs, System.Collections.Generic.IEnumerable<object> hostServices)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Runtime/ImGuiEditorRuntime.cs#L51) | Creates an ImGui editor runtime with stable host-owned extension services. |
| [`Inno.Editor.Interactions.EditorInteractions Inno.Editor.ImGui.ImGuiEditorRuntime.interactions`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Runtime/ImGuiEditorRuntime.cs#L68) | Gets the active presentation-independent interaction entry point. |
| [`int Inno.Editor.ImGui.ImGuiEditorRuntime.panelCount`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Runtime/ImGuiEditorRuntime.cs#L73) | Gets the number of active dockable panels. |
| [`override void Inno.Editor.ImGui.ImGuiEditorRuntime.Dispose()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Runtime/ImGuiEditorRuntime.cs#L267) | Releases the resources owned by this implementation. |
| [`override void Inno.Editor.ImGui.ImGuiEditorRuntime.Start()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Runtime/ImGuiEditorRuntime.cs#L78) | Starts value processing after validating the current state. |
| [`override void Inno.Editor.ImGui.ImGuiEditorRuntime.Update(Inno.Editor.Core.EditorFrame frame)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Runtime/ImGuiEditorRuntime.cs#L86) | Recomputes owned state from the current validated inputs. |
| [`void Inno.Editor.ImGui.ImGuiEditorRuntime.Draw()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Runtime/ImGuiEditorRuntime.cs#L120) | Draws the complete editor frame through ImGui. |
| [`void Inno.Editor.ImGui.ImGuiEditorRuntime.HandleKeyPressed(Inno.Core.Events.KeyPressedEvent keyEvent)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Runtime/ImGuiEditorRuntime.cs#L248) | Dispatches contextual shortcuts, leaving editing keys with active text widgets while permitting explicit save. |
| [`void Inno.Editor.ImGui.ImGuiEditorRuntime.PrepareShutdown()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Runtime/ImGuiEditorRuntime.cs#L111) | Freezes automatic extension-state persistence and writes the final project state before editor modules begin shutting down. |
| [`void Inno.Editor.ImGui.ImGuiEditorRuntime.SaveState()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Runtime/ImGuiEditorRuntime.cs#L94) | Captures all stateful active modules and panels and flushes their project state to disk. |

### `Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Card.cs#L12) | Provides reusable editor controls and rendering helpers built on the native ImGui API. |
| [`static Inno.Editor.ImGui.EditorStyleMetrics Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.style`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Style.cs#L20) | Gets the centralized editor layout metrics shared by every widget and feature panel. |
| [`static Inno.Editor.ImGui.ImGuiWidget.InlineRenameResult Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.InlineRename(string id, ref string text, ref bool requestFocus, float rowHeight, nuint capacity = 512, float width = -1)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.InlineRename.cs#L47) | Draws a compact single-line rename editor inside an existing row. |
| [`static Inno.Editor.ImGui.ImGuiWidget.TreeNodeResult Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.TreeNode(string id, System.Action<Inno.Editor.ImGui.ImGuiWidget.TreeNodeDrawContext> onDraw, in Inno.Editor.ImGui.ImGuiWidget.TreeNodeOptions options)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Tree.cs#L47) | Draws a full-width interactive tree row whose non-leaf content toggles expansion when double-clicked, while preserving single-click disclosure-arrow behavior. |
| [`static System.Numerics.Vector2 Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.GetClickableTextSize(string text, System.Numerics.Vector2 padding)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Text.cs#L475) | Calculates a clickable text area from visible text and requested inner padding. |
| [`static System.Numerics.Vector2 Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.GetCompactClickableTextSize()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Text.cs#L498) | Gets the compact fixed interaction size used by icon-style clickable text. |
| [`static System.Numerics.Vector2 Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.GetCompactIconSize()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Text.cs#L510) | Gets the square icon interaction size shared by dock-header close controls and compact editor icons. |
| [`static System.Numerics.Vector2 Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.GetLabelChipSize(string label)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Controls.cs#L26) | Gets the layout size of a compact colored label chip using the centralized editor style. |
| [`static System.Numerics.Vector2 Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.GetTypeBadgeSize(string label)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Controls.cs#L82) | Gets the layout size of a compact outlined type badge. |
| [`static System.Numerics.Vector4 Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.GetGlyphVisualBounds(Inno.Native.ImGui.ImFontPtr font, float fontSize, string text)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Text.cs#L35) | Gets the visible bounds of the first glyph in a string at a requested font size. |
| [`static bool Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.AxisDragFloat(string id, string axis, ref float value, float width, float speed = 0.1)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Property.cs#L225) | Draws a float drag field with a compact colored axis prefix. |
| [`static bool Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.AxisDragInt(string id, string axis, ref int value, float width, float speed = 1)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Property.cs#L351) | Draws an integer drag field with a compact colored axis prefix. |
| [`static bool Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.BeginBoundedCombo(string id, string preview, Inno.Native.ImGui.ImGuiComboFlags flags = Inno.Native.ImGui.ImGuiComboFlags.None)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Selector.cs#L40) | Begins a combo whose popup opens below the control and stays within its containing window. |
| [`static bool Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.BeginContextMenu(string id)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.ContextMenu.cs#L163) | Begins a parent-viewport styled right-click context menu for the most recently submitted item. |
| [`static bool Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.BeginMenuPopup(string id)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.ContextMenu.cs#L34) | Begins an explicitly opened popup using the editor context-menu presentation contract. The popup stays in its parent viewport, sizes itself to submitted content, and scrolls when its work-area bound is reached. |
| [`static bool Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.BeginMenuSelector(string id, string preview, float width, float minimumPopupWidth)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Selector.cs#L90) | Draws a compact selector control and begins a work-area-bounded menu popup. |
| [`static bool Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.BeginMenuTooltip()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.ContextMenu.cs#L63) | Begins a parent-viewport tooltip using the same padding, colors, border, and spacing as editor menus. |
| [`static bool Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.BeginSearchPopup(string id, ref string query, string hint, nuint capacity = 256, float width = -1)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Search.cs#L67) | Begins a popup and draws its focused search field. |
| [`static bool Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.BeginWindowContextMenu(string id)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.ContextMenu.cs#L184) | Begins a parent-viewport styled right-click context menu for the current window's unoccupied background. |
| [`static bool Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.CenteredButton(string label, float topPadding = 0)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Controls.cs#L262) | Draws a horizontally centered button with optional space above it. |
| [`static bool Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.Checkbox(string label, ref bool value, string? tooltip = null)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Controls.cs#L239) | Draws a standard checkbox with the shared editor tooltip behavior. |
| [`static bool Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.ClickableIcon(string id, string icon, string? tooltip = null)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Text.cs#L199) | Draws a clickable icon inside the same square interaction slot used by editor close controls. The icon has no resting background and changes only its glyph color while hovered or active. |
| [`static bool Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.ClickableText(string id, string text, System.Numerics.Vector2 controlSize, string? tooltip = null)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Text.cs#L257) | Draws clickable text centered inside an explicitly sized transparent interaction area. |
| [`static bool Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.ClickableText(string id, string text, string? tooltip = null)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Text.cs#L122) | Draws clickable text without a persistent or hovered background. |
| [`static bool Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.CollapsingCard(string id, string title, System.Action? drawLeadingControl = null, System.Action? drawTrailingControl = null, bool defaultOpen = true, bool dimmed = false, float trailingControlWidth = 0, System.Action? drawContextMenu = null)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Card.cs#L44) | Draws a component-style collapsible card header. |
| [`static bool Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.CompactCheckbox(string id, ref bool value, float size = -1, string? tooltip = null)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Controls.cs#L156) | Draws a compact checkbox whose checked fill uses the current text color. |
| [`static bool Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.CompactDragFloat(string label, ref float value, float speed = 0.1, float? minimum = null, float? maximum = null)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Property.cs#L257) | Draws a floating-point drag field with a compact one-decimal presentation and precise text editing. |
| [`static bool Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.CompactSliderFloat(string label, ref float value, float minimum, float maximum)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Property.cs#L300) | Draws a bounded slider with compact presentation and a double-click precise input mode. |
| [`static bool Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.DragDropSource<TPayload>(string payloadType, System.Func<TPayload> payloadFactory, System.Action? drawPreview = null, bool allowHoldToOpenOthers = true)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.DragDrop.cs#L88) | Publishes a lazily created unmanaged drag payload for the most recently submitted item. |
| [`static bool Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.DragDropSource<TPayload>(string payloadType, in TPayload payload, System.Action? drawPreview = null, bool allowHoldToOpenOthers = true)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.DragDrop.cs#L38) | Publishes an unmanaged drag payload for the most recently submitted item. |
| [`static bool Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.DragDropTarget<TPayload>(string payloadType, System.Numerics.Vector2 minimum, System.Numerics.Vector2 maximum, uint targetId, out TPayload payload, out bool isPreviewing, bool drawDefaultHighlight = true)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.DragDrop.cs#L248) | Accepts an unmanaged payload on an explicit screen-space rectangle. |
| [`static bool Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.DragDropTarget<TPayload>(string payloadType, out TPayload payload)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.DragDrop.cs#L128) | Accepts an unmanaged payload on the most recently submitted item. |
| [`static bool Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.DragDropTarget<TPayload>(string payloadType, out TPayload payload, out bool isPreviewing)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.DragDrop.cs#L155) | Accepts an unmanaged payload and reports its preview state on the most recently submitted item. |
| [`static bool Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.DragDropTarget<TPayload>(string payloadType, out TPayload payload, out bool isPreviewing, bool drawDefaultHighlight)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.DragDrop.cs#L186) | Accepts an unmanaged payload and controls whether ImGui draws its default target rectangle. |
| [`static bool Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.EnsureSection(string title = "Properties", string? description = null)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.HelpBox.cs#L148) | Ensures that subsequently drawn default properties belong to a framed section. |
| [`static bool Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.HoverText(string id, string text)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Text.cs#L147) | Draws a text link with no button background and highlights it only through text presentation. |
| [`static bool Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.SearchInput(string id, string hint, ref string query, nuint capacity = 256, float width = -1)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Search.cs#L34) | Draws a single-line search field with a stable identifier. |
| [`static bool Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.SectionHeader(string title, string? description = null, System.Action? drawLeadingControl = null)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.HelpBox.cs#L180) | Draws a section heading with hover-only description using the shared Inspector presentation. |
| [`static bool Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.isSectionContentVisible`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.HelpBox.cs#L130) | Gets whether content belonging to the current Inspector section should be drawn. |
| [`static string Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.NicifyName(string name)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Property.cs#L444) | Converts an identifier into a readable editor label. |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.AddGlyphCentered(Inno.Native.ImGui.ImDrawListPtr drawList, Inno.Native.ImGui.ImFontPtr font, float fontSize, string text, System.Numerics.Vector2 center, uint color)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Text.cs#L79) | Draws one glyph so the center of its visible bounds matches a requested point. |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.CardBody(string id, System.Action drawContent, bool dimmed = false)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Card.cs#L253) | Draws expanded collapsible-card content inside a framed body. |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.CenteredProgressBar(float fraction, System.Numerics.Vector2 size, string overlay)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Controls.cs#L298) | Draws a progress bar whose overlay remains centered over the complete bar. |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.CenteredText(string text, System.Numerics.Vector2 areaSize)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Text.cs#L311) | Draws non-interactive text centered inside a reserved layout area. |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.CenteredWrappedText(string text, System.Numerics.Vector2 areaSize, System.Numerics.Vector2 padding)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Text.cs#L351) | Draws wrapped non-interactive text as a centered block inside a padded layout area. |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.CollectionSectionHeader(string title)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.HelpBox.cs#L76) | Draws a square collection-section band whose background, separator, and padding are shared with hierarchy and Inspector presentation. |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.ColoredText(System.Numerics.Vector4 color, string text)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Text.cs#L416) | Draws unformatted text with a temporary foreground color. |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.ConstrainedContent(string id, System.Action drawContent, bool useWindowPadding = true)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Panel.cs#L112) | Draws a vertically auto-sized content region that is constrained to the current available width and cannot create an independent scroll range. |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.Disabled(bool disabled, System.Action draw)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Property.cs#L371) | Draws content inside an ImGui disabled scope when requested. |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.DrawDisclosureIndicator(System.Numerics.Vector2 min, System.Numerics.Vector2 max, bool open, bool dimmed = false)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Card.cs#L208) | Draws a vertically centered disclosure triangle with button-style hover feedback. |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.DrawItemTooltip(string? text)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.ContextMenu.cs#L88) | Draws a consistently sized, wrapped editor tooltip for the most recently submitted item. |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.DrawTooltip(string? text)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.ContextMenu.cs#L105) | Draws the standard viewport-clamped tooltip when a custom-drawn canvas element is hovered. |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.DropTargetHighlight(System.Numerics.Vector2 min, System.Numerics.Vector2 max)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Property.cs#L422) | Draws the standard yellow rectangular drag-and-drop target highlight above all normal window content in screen coordinates. |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.EndBoundedCombo()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Selector.cs#L60) | Ends a combo opened by . |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.EndContextMenu()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.ContextMenu.cs#L203) | Ends a context menu opened by or . |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.EndMenuPopup()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.ContextMenu.cs#L50) | Ends a popup opened by and restores the previous style. |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.EndMenuSelector()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Selector.cs#L134) | Ends a selector popup opened by . |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.EndMenuTooltip()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.ContextMenu.cs#L76) | Ends a tooltip opened by and restores the previous style. |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.EndSearchPopup()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Search.cs#L89) | Ends a popup opened by . |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.HeaderSurface(string id, System.Action drawContent, bool spanWindowPadding = false)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Panel.cs#L164) | Draws a square-cornered editor header surface using the shared target-header palette, border, and padding. |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.HelpBox(string text, string icon, System.Numerics.Vector4 color)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.HelpBox.cs#L28) | Draws a wrapped contextual message with a semantic icon and a subdued status surface. |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.Hint(string text)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Panel.cs#L352) | Draws disabled hint text that wraps to the current content width. |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.IconText(string icon, string text, bool highlight)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Text.cs#L544) | Draws icon and text with the icon's visible glyph bounds centered in a slot that expands when the glyph is wider than the normal editor icon slot. |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.InsertionLine(float fromX, float toX, float y)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Property.cs#L399) | Draws a horizontal insertion marker above normal window content in screen coordinates. |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.LabelChip(string label, System.Numerics.Vector4 background)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Controls.cs#L49) | Draws non-interactive text centered on a compact, softly rounded colored background. |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.MetadataValue(string metadata, System.Action drawValue, string? tooltip = null)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Property.cs#L152) | Draws a subdued metadata prefix followed by an interactive value on the same line. |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.MetadataValue(string metadata, string value, string? tooltip = null)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Property.cs#L185) | Draws one wrapped, subdued value whose metadata and content remain in the value column. |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.PanelWindow(string title, ref bool isOpen, System.Action drawBody, Inno.Native.ImGui.ImGuiWindowFlags flags = Inno.Native.ImGui.ImGuiWindowFlags.NoCollapse, bool useWindowPadding = true)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Panel.cs#L33) | Opens a standard panel window and executes panel body. |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.PropertyLabel(string label, string? tooltip = null)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Property.cs#L126) | Draws a label within its current table cell, wrapping when the column is narrow. |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.PropertyRow(string id, System.Action drawLabel, System.Action drawValue)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Property.cs#L70) | Draws a two-column property row whose label is supplied by a custom presentation callback. |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.PropertyRow(string id, string label, System.Action drawValue, string? tooltip = null)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Property.cs#L39) | Draws a two-column property row with a stable internal identifier. |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.SectionLayout(System.Action drawContent)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.HelpBox.cs#L106) | Draws content in a scope where consecutive section headers become framed fieldsets. |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.SetNextTreeNodeOpen(bool open)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Tree.cs#L176) | Overrides the retained expansion state of the next submitted non-leaf tree row. |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.SetupPropertyColumns()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Property.cs#L108) | Configures the label and input columns of an active table with a shared two-to-three ratio. A following fixed-width action column may be added by the caller. |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.SetupStyle()`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Style.cs#L25) | Applies the centralized editor layout metrics and palette to the current ImGui context. |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.TypeBadge(string label, System.Numerics.Vector4 accent, string? tooltip = null)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Controls.cs#L104) | Draws a compact, non-interactive type badge with a subdued fill and semantic outline. |
| [`static void Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget.WrappedText(string text)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Text.cs#L442) | Draws literal text wrapped to the remaining content width without using a native variadic formatting entry point. |

### `Inno.Editor.ImGui.ImGuiWidget.InlineRenamePresentation`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.ImGui.ImGuiWidget.InlineRenamePresentation`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.InlineRename.cs#L152) | Describes the view-owned geometry used to present an active inline rename action. |
| [`Inno.Editor.ImGui.ImGuiWidget.InlineRenamePresentation.InlineRenamePresentation(string id, float width, float rowHeight, nuint bufferSize = 512)`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.InlineRename.cs#L176) | Creates inline rename presentation data. |
| [`float Inno.Editor.ImGui.ImGuiWidget.InlineRenamePresentation.rowHeight`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.InlineRename.cs#L224) | Gets the height of the row area in which the input field is centered. |
| [`float Inno.Editor.ImGui.ImGuiWidget.InlineRenamePresentation.width`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.InlineRename.cs#L219) | Gets the requested input width in logical pixels. |
| [`nuint Inno.Editor.ImGui.ImGuiWidget.InlineRenamePresentation.bufferSize`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.InlineRename.cs#L229) | Gets the maximum UTF-8 buffer size accepted by the input. |
| [`string Inno.Editor.ImGui.ImGuiWidget.InlineRenamePresentation.id`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.InlineRename.cs#L214) | Gets the stable ImGui identifier used by the input field. |

### `Inno.Editor.ImGui.ImGuiWidget.InlineRenameResult`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.ImGui.ImGuiWidget.InlineRenameResult`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.InlineRename.cs#L235) | Describes the outcome of an inline rename control. |
| [`Inno.Editor.ImGui.ImGuiWidget.InlineRenameResult.Cancel`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.InlineRename.cs#L255) | Indicates that the edited text should be discarded. |
| [`Inno.Editor.ImGui.ImGuiWidget.InlineRenameResult.Commit`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.InlineRename.cs#L245) | Indicates that the edited text should be committed. |
| [`Inno.Editor.ImGui.ImGuiWidget.InlineRenameResult.FocusLost`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.InlineRename.cs#L250) | Indicates that the input lost focus and its valid value should be committed before closing. |
| [`Inno.Editor.ImGui.ImGuiWidget.InlineRenameResult.None`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.InlineRename.cs#L240) | Indicates that the interaction remains active. |

### `Inno.Editor.ImGui.ImGuiWidget.TreeNodeDrawContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.ImGui.ImGuiWidget.TreeNodeDrawContext`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Tree.cs#L393) | Provides the native geometry of a tree row while its custom content is being drawn. |
| [`float Inno.Editor.ImGui.ImGuiWidget.TreeNodeDrawContext.rowHeight`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Tree.cs#L403) | Gets the actual native row height established for the current tree node. |

### `Inno.Editor.ImGui.ImGuiWidget.TreeNodeOptions`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.ImGui.ImGuiWidget.TreeNodeOptions`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Tree.cs#L409) | Configures an interactive tree row. |
| [`System.Action? Inno.Editor.ImGui.ImGuiWidget.TreeNodeOptions.drawViewportOverlay`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Tree.cs#L445) | Gets the callback that draws controls fixed to the current viewport without extending the tree's horizontal content boundary. |
| [`System.Numerics.Vector4 Inno.Editor.ImGui.ImGuiWidget.TreeNodeOptions.backgroundColor`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Tree.cs#L429) | Gets the custom background color used when is enabled. |
| [`bool Inno.Editor.ImGui.ImGuiWidget.TreeNodeOptions.hideGuideLines`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Tree.cs#L439) | Gets whether ancestor and branch guide lines are omitted for this row. |
| [`bool Inno.Editor.ImGui.ImGuiWidget.TreeNodeOptions.isLeaf`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Tree.cs#L419) | Gets whether the row has no expandable children. |
| [`bool Inno.Editor.ImGui.ImGuiWidget.TreeNodeOptions.selected`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Tree.cs#L414) | Gets whether the row is selected. |
| [`bool Inno.Editor.ImGui.ImGuiWidget.TreeNodeOptions.showBackground`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Tree.cs#L424) | Gets whether a custom background is drawn behind an unselected row. |
| [`bool Inno.Editor.ImGui.ImGuiWidget.TreeNodeOptions.suppressHoverHighlight`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Tree.cs#L434) | Gets whether the row keeps its configured background while hovered. |

### `Inno.Editor.ImGui.ImGuiWidget.TreeNodeResult`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.ImGui.ImGuiWidget.TreeNodeResult`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Tree.cs#L451) | Describes interaction state produced by a tree row. |
| [`System.Numerics.Vector2 Inno.Editor.ImGui.ImGuiWidget.TreeNodeResult.contentMin`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Tree.cs#L505) | Gets the minimum screen coordinate of the row's interactive content, excluding tree indentation. |
| [`System.Numerics.Vector2 Inno.Editor.ImGui.ImGuiWidget.TreeNodeResult.max`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Tree.cs#L500) | Gets the row maximum screen coordinate. |
| [`System.Numerics.Vector2 Inno.Editor.ImGui.ImGuiWidget.TreeNodeResult.min`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Tree.cs#L495) | Gets the row minimum screen coordinate. |
| [`bool Inno.Editor.ImGui.ImGuiWidget.TreeNodeResult.isClicked`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Tree.cs#L479) | Gets whether the content row was clicked. |
| [`bool Inno.Editor.ImGui.ImGuiWidget.TreeNodeResult.isDoubleClicked`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Tree.cs#L485) | Gets whether the content row was double-clicked. Double-clicking a non-leaf content row also toggles its retained expansion state for the next frame. |
| [`bool Inno.Editor.ImGui.ImGuiWidget.TreeNodeResult.isHovered`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Tree.cs#L490) | Gets whether the full row is hovered. |
| [`bool Inno.Editor.ImGui.ImGuiWidget.TreeNodeResult.isOpen`](../../src/composition/editor/presentation/Inno.Editor.ImGui/Widgets/ImGuiWidget.Tree.cs#L474) | Gets whether child content should be rendered. |

## 项目依赖

- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Presentation.ImGui.Sdl3](../backends/ImGui/Inno.Adapter.Presentation.ImGui.Sdl3.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Identity](../core/Inno.Core.Identity.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：公开引用边界由实际签名核对。
- [Inno.Editor.Core](Inno.Editor.Core.md)：公开引用边界由实际签名核对。
- [Inno.Editor.Interactions](Inno.Editor.Interactions.md)：公开引用边界由实际签名核对。
- [Inno.Native.ImGui](../backends/ImGui/Inno.Native.ImGui.md)：公开引用边界由实际签名核对。
- [Inno.Core.Events](../core/Inno.Core.Events.md)：公开引用边界由实际签名核对。
- [Inno.Core.Logging](../core/Inno.Core.Logging.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
