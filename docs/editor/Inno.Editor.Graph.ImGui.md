# Inno.Editor.Graph.ImGui

[Editor 索引](README.md) · [Wiki 首页](../README.md) · [UI 架构](../architecture/EDITOR_UI_ARCHITECTURE.md)

## 职责与边界

通用 Graph 画布呈现。依赖中立 Graph/Graphs 与 ImGui；禁止引用 Rendering、Shader 或具体 Panel。

## 依赖与初始化

产品通过 `EditorProduct.props` 显式部署此程序集。由当前 TypeRegistry/Editor Registry 发现声明并在候选成功后切换；不扫描目录、不建立独立注册表或全局状态。

## API 与扩展点

GraphCanvasView 组合当前 GraphCanvasState、画布原点和尺寸，提供 ToScreen、ToGraph、DrawGrid、DrawConnection 和 HitConnection。它不修改 graph，也不拥有 History、选择或资源。

## 工作流

每次绘制按当前 viewport 创建值对象；同一连接曲线用于绘制与命中测试。屏幕坐标只在当前帧有效。

## 错误、生命周期与热重载

控件只在当前 EditorUiSession 的 owner thread、作用域与原生上下文中使用。跨帧只保留稳定 ID 和中立状态；Missing、只读和未赋值状态保持区别。手势失败保留 History 所有权和诊断；扩展在所属 generation 退休，不保留插件 Type、delegate 或实例。

所有公开签名的当前映射见本页 API 清单；内部实现不构成稳定扩展契约。

## 当前源码公开 API 清单

此清单对应当前 SDK 求值与源码符号；内部类型不构成公开 API。

<!-- api: T:Inno.Editor.Graph.ImGui.GraphCanvasView ebd0487b4f3149c633c09a885e4c07c254ee35be9c1f2a4a137058ba94bc4c13 -->
<!-- api: M:Inno.Editor.Graph.ImGui.GraphCanvasView.#ctor(Inno.Editor.Graph.GraphCanvasState,System.Numerics.Vector2,System.Numerics.Vector2) 4bf23f4c09bf6254ecacfde839c17fb3c99513644bc7debe372647724046ff20 -->
<!-- api: M:Inno.Editor.Graph.ImGui.GraphCanvasView.ToScreen(Inno.Core.Graphs.GraphPosition) 121f46987030c64cc310f3e755685f39cceebfebf136afeff3e9e4d4591e885c -->
<!-- api: M:Inno.Editor.Graph.ImGui.GraphCanvasView.ToGraph(System.Numerics.Vector2) 0d5f2469effa58b7edcc3ff25fdf0d3723790bbed814b04bac1218e7a1acf5a1 -->
<!-- api: M:Inno.Editor.Graph.ImGui.GraphCanvasView.DrawGrid(System.UInt32,System.Single) cde8a13f414b287dbaea6b0976ec586638cd1a9ca114ba81604f35d1606094c0 -->
<!-- api: M:Inno.Editor.Graph.ImGui.GraphCanvasView.DrawConnection(System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32,System.Single) eb900290aa0899feda99151ce10a04a501e21a071d99d159aa7b01d8ba221c7e -->
<!-- api: M:Inno.Editor.Graph.ImGui.GraphCanvasView.HitConnection(System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Single) 98f966a03ad3b5e451d282ed66aa1252e776595b9e538ca562740adf0ad2a222 -->

| API 与源码 | 契约与参数 | 返回值 | 重要异常 |
| --- | --- | --- | --- |
| <a id="api-0a05c4ee5b94a76e"></a>[`public readonly struct Inno.Editor.Graph.ImGui.GraphCanvasView`](../../src/composition/editor/presentation/Inno.Editor.Graph.ImGui/Canvas/GraphCanvasView.cs#L11) | Projects and presents an arbitrary graph viewport without owning document mutations or domain semantics. | — | — |
| <a id="api-6c3db7b4cd80a1fd"></a>[`public Inno.Editor.Graph.ImGui.GraphCanvasView.GraphCanvasView(Inno.Editor.Graph.GraphCanvasState state, System.Numerics.Vector2 origin, System.Numerics.Vector2 size)`](../../src/composition/editor/presentation/Inno.Editor.Graph.ImGui/Canvas/GraphCanvasView.cs#L31) | Creates a frame-local graph presenter over navigation owned by its document.<br>`state`: The existing neutral navigation and selection state.<br>`origin`: The screen-space canvas origin.<br>`size`: The finite nonnegative canvas extent. | — | `System.ArgumentOutOfRangeException`: The canvas geometry is invalid. |
| <a id="api-be8793c43f2dca40"></a>[`public System.Numerics.Vector2 Inno.Editor.Graph.ImGui.GraphCanvasView.ToScreen(Inno.Core.Graphs.GraphPosition point)`](../../src/composition/editor/presentation/Inno.Editor.Graph.ImGui/Canvas/GraphCanvasView.cs#L55) | Projects a graph-space location into the current screen rectangle.<br>`point`: The domain-independent graph coordinate. | The corresponding screen coordinate. | — |
| <a id="api-4f59cd260d6560d9"></a>[`public Inno.Core.Graphs.GraphPosition Inno.Editor.Graph.ImGui.GraphCanvasView.ToGraph(System.Numerics.Vector2 point)`](../../src/composition/editor/presentation/Inno.Editor.Graph.ImGui/Canvas/GraphCanvasView.cs#L65) | Converts a screen location back into graph coordinates.<br>`point`: The screen coordinate to project. | The graph coordinate under the pointer. | — |
| <a id="api-3d76ebaab6e16eca"></a>[`public void Inno.Editor.Graph.ImGui.GraphCanvasView.DrawGrid(uint color, float spacing = 32)`](../../src/composition/editor/presentation/Inno.Editor.Graph.ImGui/Canvas/GraphCanvasView.cs#L78) | Draws a bounded graph-space grid through the current UI drawing context.<br>`color`: The packed presentation color.<br>`spacing`: Positive graph-space grid spacing. | — | `System.ArgumentOutOfRangeException`: Spacing is nonpositive or nonfinite. |
| <a id="api-be8d556e70f484fe"></a>[`public void Inno.Editor.Graph.ImGui.GraphCanvasView.DrawConnection(System.Numerics.Vector2 output, System.Numerics.Vector2 input, uint color, float thickness = 2)`](../../src/composition/editor/presentation/Inno.Editor.Graph.ImGui/Canvas/GraphCanvasView.cs#L106) | Draws a horizontal-tangent connection using the same curve used for hit testing.<br>`output`: The output endpoint in screen space.<br>`input`: The input endpoint in screen space.<br>`color`: The packed connection color.<br>`thickness`: The positive screen-space stroke width. | — | — |
| <a id="api-80b9ddf9d925d3f0"></a>[`public static bool Inno.Editor.Graph.ImGui.GraphCanvasView.HitConnection(System.Numerics.Vector2 point, System.Numerics.Vector2 output, System.Numerics.Vector2 input, float radius = 6)`](../../src/composition/editor/presentation/Inno.Editor.Graph.ImGui/Canvas/GraphCanvasView.cs#L137) | Checks pointer distance against the displayed cubic connection without any UI or document access.<br>`point`: The screen-space pointer location.<br>`output`: The displayed output endpoint.<br>`input`: The displayed input endpoint.<br>`radius`: Nonnegative screen-space acceptance radius. | True when the pointer is within the curve's hit area, including its endpoints. | `System.ArgumentOutOfRangeException`: Radius is negative or nonfinite. |

## 项目依赖

| 工程 | 边界 |
| --- | --- |
| `Inno.Scripting.Api` | 实现依赖；不传递 compile surface |
| `Inno.Editor.ImGui` | 实现依赖；不传递 compile surface |
| `Inno.Core.Graphs` | 公开签名依赖 |
| `Inno.Core.Serialization` | 公开签名依赖 |
| `Inno.Extensibility.Catalogs` | 公开签名依赖 |
| `Inno.Editor.Graph` | 公开签名依赖 |
