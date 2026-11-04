## 24. Editor 表单与滚动约束

- 每个可视区域只能指定一个滚动 owner。父 Panel/Modal、布局 Child 与内容 Child 不得同时为同一方向生成滚动条；承载填满区域的可滚动 Child 时，父容器必须禁用对应滚动。
- 所有左侧 label、右侧 input 的 Editor 字段复用 `ImGuiWidget.SetupPropertyColumns()` 与 `PropertyLabel()`，按 2:3 分配可用宽度；有操作列时先扣除操作列。label 必须在自身列换行，行高随内容增长。
- 禁止以固定 label 像素宽度、无条件 scrollbar flag、虚假的 content size 或额外嵌套滚动 Child 修补布局。修改表单、Panel、Popup 后要在小窗口、长文本与不同 zoom 下验证 `ScrollMaxX/ScrollMaxY` 只在内容确实溢出时为正。
- Editor 下拉选择器统一使用 `BeginBoundedCombo` / `EndBoundedCombo` 或 `BeginMenuSelector` / `EndMenuSelector`；弹层从触发控件下边缘向下展开，宽度受所属窗口限制，高度同时受所属窗口剩余空间和窗口高度比例限制。不要直接用原生 Combo 的自动翻转定位。
- Popup 尺寸约束统一在 Widget 边界做像素对齐：上限向下取整，内容所需尺寸向上取整，防止 ImGui 截断小数 padding 后产生假溢出。回归必须覆盖 85%、90%、110% 等非整像素缩放；不能只测试整数或半整数 padding，也不能用 `NoScrollbar` 隐藏问题。
- 自绘命中区域除了矩形包含关系，还必须确认所属 ImGui window 是当前可交互的前景窗口；被浮动窗口、Popup 或 Modal 遮挡时不得触发底层 Panel 的关闭、画布手势或游戏输入。平台事件仍使用 `Inno.Core.Events`，ImGui 只决定可见 UI 的命中与焦点；Play Session 使用独立 Input backend 接收经 Game View 焦点策略筛选的事件，不建立第二个事件总线。
- 树行的 disclosure、正文与叠加按钮必须各自拥有独立命中区域；原生 TreeNode 不得以整行 SpanFullWidth/AllowOverlap 覆盖自绘正文。回归同时覆盖鼠标先悬停与移动/按下发生在同一帧、正文双击、箭头展开及叠加按钮，不能依赖额外一帧悬停才能点击。
