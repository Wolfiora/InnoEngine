# Inno.Editor.ImGui

## 共享 Inspector 与窗口呈现

`ImGuiWidget.SectionLayout(Action drawContent)` 为一段内容建立可复用的 fieldset scope；scope 内连续调用 `bool SectionHeader(string title, string? description = null, Action? drawLeadingControl = null)` 时，标题嵌入上边框，直到下一个标题或 scope 结束的内容都被同一边框完整包裹。展开的内部 content fieldset 使用轻微圆角；窗口、Panel、顶层 Header 和其他大容器保持直角。返回值决定当前 section 正文是否应绘制；标题文字本身通过共享的 `ClickableText` widget 切换展开状态，不显示额外的加减号或整行 hover 背景。标题与 Stats 的共享 `CollectionSectionHeader` 使用同一 `sectionHeaderPadding`，因此左侧缩进不会由各 Panel 自行估算。折叠态保留中断式横线，并在整条线的左右端各绘制一根以横线为中心的短竖帽，形成 `⊢ … ⊣` 轮廓；竖帽与横线共享颜色、粗细和交点，不形成向下的残留边框。折叠状态由 ImGui 按当前 Inspector scope 的稳定顺序保存，标题或 Local/World 文案变化不会重置状态。`EnsureSection` 为没有 `[Header]` 的默认属性自动建立 `Properties` fieldset，`isSectionContentVisible` 则让 serialized-property pipeline 在任意 section 折叠后持续跳过内容，直到下一个 Header。可选 leading control 用于 Local/World 这类属于分组语义本身的开关；scope 外的 `SectionHeader` 仍保持普通分隔标题并返回 `true`。`HeaderSurface(id, drawContent, spanWindowPadding)` 是 Inspector target 与 Shader Editor 顶部共用的直角 header 容器；`CollectionSectionHeader` 是 Stats 等 collection surface 的直角分组条。`HelpBox(string text, string icon, Vector4 color)` 绘制有边框、状态图标与侧边语义色的内部提示卡片。`DrawItemTooltip` 使用父 viewport work area、真实内宽和缩放 padding 测量，靠边自动翻向并约束位置。File Browser、Hierarchy 与 Stats 共用暗色 collectionRow/collectionRowAlternate，交替条纹、hover、选择仍保持区分。

浮动 Panel 在标题右侧显示统一关闭按钮；docked Panel 保持每个 dock node 一个关闭入口。Docked close glyph 以 ImGui 实际 `TabBar.BarRect` 的几何中心定位，不用字体高度或 DockNode 顶点推测，因此不同缩放与顶栏布局下仍严格垂直居中。窗口标题不继承输入控件的 FrameBorderSize，避免额外分隔线；这与透明接缝是两个不同的问题。窗口、Child、Tab 和 component header 的外轮廓使用直角；按钮、输入框、菜单、tooltip、HelpBox 与 Inspector content fieldset 等内部交互/内容表面仍使用语义圆角。所有按钮仍走现有 Panel close/dirty 确认生命周期。

[Editor 索引](README.md) · [Platform ImGui](../platform/Inno.Adapter.Presentation.ImGui.Sdl3.md) · [Wiki 首页](../README.md)

## 退出所有权

`EditorGameInputCapture` 是 Game View 对 Play Session 的中立输入策略。Host 在每帧前调用 `BeginFrame`，Game View 用 `Report` 更新当前 viewport 的窗口、画面边界、焦点和前景悬停，帧后用 `CompleteFrame` 识别需要释放按键的失焦窗口。Host 将 `Route(Event)` 返回的事件送进独立 Play Input source；`null` 表示该事件属于其他 Editor 窗口或被 UI 遮挡。它只筛选现有 `Inno.Core.Events` 事件，不注册另一套监听器。Dock close 等自绘命中区同样必须核对 ImGui 前景窗口，不能单凭屏幕坐标触发。

平台事件先进入 Edit Session 的统一 Dispatcher；Host 在 `dispatched` 观察阶段调用 Game View 路由，保证 Editor 快捷键与 Hub 消费已经完成。全局消费的输入不会再次送入游戏。已经向游戏交付输入的窗口，如果其 Key/Mouse release、失焦或关闭事件被上层消费，`Route` 返回新的 focus-reset event；Host 通过统一 Input source 在 Play simulation 前释放按住状态，不重放原 release。其他窗口被消费的事件不改变当前 capture。隐藏、关闭或移动 Game View 的 presentation 失焦仍由 `CompleteFrame` 补充 reset。

同一窗口的鼠标移动即使被 Editor 消费，也更新 capture 的位置边界观察；它不会送入游戏，也不会释放已捕获的拖拽。后续点击和滚轮因此使用最新位置判断，不沿用已过时的“鼠标仍在图像内”状态。

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

`IconText` 使用 baked glyph 的可见边界把 icon 轮廓放在 slot 中心。字体注册与自定义方式见 [Platform ImGui](../platform/Inno.Adapter.Presentation.ImGui.Sdl3.md)。

`DragDropTarget(..., drawDefaultHighlight: false)` 可关闭 ImGui 默认目标框，适合需要按鼠标在行内位置绘制互斥反馈的复合目标。

`EditorDragDropRenderer` 只向 native payload 写入固定 session token；业务对象保留在 Interactions 的 managed session 中。Panel 与 Widget 的公开调用不要求 `unsafe`，FileBrowser 的 Grid 文本和搜索框也只使用安全的 Widget/ImGui API。
