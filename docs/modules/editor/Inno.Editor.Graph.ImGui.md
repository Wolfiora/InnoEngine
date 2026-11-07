# Inno.Editor.Graph.ImGui

[Editor 索引](README.md) · [Wiki 首页](../../README.md) · [UI 架构](../../architecture/editor-ui.md)

## 职责与边界

通用 Graph 画布呈现。依赖中立 Graph/Graphs 与 ImGui；禁止引用 Rendering、Shader 或具体 Panel。

## 依赖与初始化

产品通过 `EditorProduct.props` 显式部署此程序集。由当前 TypeRegistry/Editor Registry 发现声明并在候选成功后切换；不扫描目录、不建立独立注册表或全局状态。

## API 与扩展点

`GraphCanvasView` 在构造时复制当前 pan、zoom、画布原点和尺寸；同一帧的投影不会因后续导航修改而漂移。它提供节点、端口、连线、命中与框选的通用呈现，不解释 Shader 或其他图领域语义。

`GraphCanvasLayout` 保留由稳定图 ID、位置、尺寸、文字与端口组成的中立快照。`SetNodes` 原子校验候选，按值复用未变化的节点；平移、缩放和拖动预览不要求重建端口几何。长期状态不能包含领域实例、插件 Type 或绘制 callback。

`GraphCanvasPointerController` 只管理选择与当前手势的中立预览。`Step` 接收已经完成焦点、遮挡和导航路由的输入，返回移动或连线意图；调用方负责校验、原子领域修改及既有 History。取消不修改文档，完成移动只有一个逻辑 Undo。Shader 消费方在保存、关闭、Play 或 reload 前通过现有 Core Events 操作准备通知完成尚未提交的预览；失败保留可重试输入。

## 工作流

每次绘制按当前 viewport 创建值对象；同一连接曲线用于绘制与命中测试。屏幕坐标只在当前帧有效。

`DrawNode` 和 `DrawConnection` 使用调用方当前窗口的帧内 `EditorDrawList` 借用；方法不保留或释放该借用，每次原生绘制仍验证窗口、帧、线程和 Context。`DrawNode` 的自定义内容只在节点与画布相交时借用执行，返回前恢复本次 clip；异常继续传播。调用方应在该次回调中配对自己的样式和其他作用域。没有原生 UI 帧时仍可调用纯几何、命中及指针契约；实际绘制要求有效的 owner thread、Session 和 ImGui Context。

## 错误、生命周期与热重载

`GraphCanvasView` 构造函数拒绝空的文档导航状态（`ArgumentNullException`）及非有限或负的画布几何（`ArgumentOutOfRangeException`）；它不会修正调用方的非法输入。

控件只在当前 EditorUiSession 的 owner thread、作用域与原生上下文中使用。跨帧只保留稳定 ID 和中立状态；Missing、只读和未赋值状态保持区别。手势失败保留 History 所有权和诊断；扩展在所属 generation 退休，不保留插件 Type、delegate 或实例。

所有公开签名的当前映射见本页 API 清单；内部实现不构成稳定扩展契约。

## API 参考

[完整声明、行为与调用约束](Inno.Editor.Graph.ImGui/api.md)

<!-- api-include: Inno.Editor.Graph.ImGui/api.md -->

## 项目依赖

| 工程 | 边界 |
| --- | --- |
| `Inno.Scripting.Api` | 实现依赖；不传递 compile surface |
| `Inno.Editor.ImGui` | 公开签名依赖 |
| `Inno.Core.Graphs` | 公开签名依赖 |
| `Inno.Core.Serialization` | 公开签名依赖 |
| `Inno.Extensibility.Catalogs` | 公开签名依赖 |
| `Inno.Editor.Graph` | 公开签名依赖 |
