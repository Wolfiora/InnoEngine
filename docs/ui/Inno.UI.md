# Inno.UI

[UI 索引](README.md) · [Text](../text/Inno.Text.md)

`IUiService` / `UI` 门面提供 Context 生命周期、语言标记的文档加载、纯文本/内容片段/attribute/class 变更、RGBA8 命名纹理注册、`Update`、`Render` 与事件提取。字体由导入文档声明并在 `LoadDocument` 前注册，脚本不再逐项覆盖字体。语言选择由显式标识与已注册前端决定。`UiRenderFrame` 只发布 mesh/texture 增量、稳定 handle、释放请求与有序 draw commands，供渲染 Plugin 持久缓存。

每个 Context 的生命周期由调用方明确拥有。无活动 `UiExecutionContext` 时门面失败；持有服务的 Runtime/Provider 在其 owner 生命周期中关闭 Context。唯一 `Properties/ScriptingApi.cs` 限定脚本导出类型；后端 SPI 与原生 ABI 不导出。UI 使用后端中立的 Rendering Assets 纹理产物引用，不依赖 Scene、Editor 或具体图形实现。

## 公开类型与工作流

| 类型 | 语义 |
| --- | --- |
| `UiContextOptions`、`UiContextHandle`、`UiDocumentHandle` | 独立 surface 的尺寸/density 与不透明代际句柄；调用方负责关闭。 |
| `UiDocumentAsset`、`UiDocumentSource`、`UiDocumentFontFace` | 导入文档与字体资产依赖，或运行期生成带语言标记的内容。 |
| `UiTextureData`、`UiTextureUpdate` | 接受紧密排列 RGBA8；纹理增量含 handle、revision 和像素副本。 |
| `UiVertex`、`UiMeshUpdate`、`UiTextureUpdate`、`UiDrawCommand`、`UiRenderFrame` | 后端中立资源增量与绘制命令；不是图形设备 handle。 |
| `UiEventType`、`UiEvent` | 后端更新后提取的 DOM 事件；`UiEvent` 派生自 Core `Event`，包含文档 handle 与元素 ID。 |
| `UI`、`IUiService`、`UiExecutionContext` | 文档生命周期、DOM 更改、纹理注册、独立 Context 输入更新、Render/DrainEvents。 |
| `IUiBackend`、`UiFontRegistration` | Runtime/adapter 边界，不属于脚本导出。 |
| `UiInputSnapshot`、`UiContextTime`、`UiBackendCapabilities` | 显式路由输入、context 时间快照与开放能力查询；可供脚本组合。 |

```csharp
using InnoEngine.UI;

static UiDocumentHandle Show(UiContextHandle context, UiDocumentAsset source)
{
    UiDocumentHandle document = UI.LoadDocument(context, source);
    UI.ShowDocument(context, document);
    return document;
}
```

上述调用需位于 UI Session scope；先 `Update` 再 `DrainEvents`/`Render`。`DrainEvents` 是后端到调用方的传输边界；调用方使用 Core `EventDispatcher` / `EventHub` 组织订阅，并在所属会话退出前释放订阅。多个 UI 区域分别将命中的局部输入传给 `Update(context, input)`。Context 关闭后句柄失效，跨 Session/Plugin reload 不持久化；持久状态应保存 Asset ID 与稳定业务 ID。文档通过内容与资产契约读取，命名纹理由调用方显式注册。具体语言支持范围见所属后端页面。导入字体在文档关闭或 Context 销毁后释放资产 lease；具体后端决定原生字体缓存何时清空。

## 布局快照与坐标

`CaptureLayout(context, document, elementIds)` 在 owner thread 上刷新一次当前布局，并按请求顺序返回不可变的 `UiLayoutSnapshot`。请求列表至少包含一个非空元素 ID；Missing 和 Hidden 是明确的结果，失效或属于另一 context 的文档则失败。一次批量查询共享同一 revision，不会为每个元素重新刷新布局。

`UiElementLayout` 的内容框、边框和裁剪包围框使用已经应用 density、滚动和变换的 context 像素。`TryLocalToContext` 与 `TryContextToLocal` 使用捕获时的平面变换，支持透视除法；不可用或奇异几何返回 false。`UiViewportMapping` 只负责 context 像素与明确 viewport 矩形之间的映射，不能再次乘 DPI。裁剪框不是圆角或复杂遮罩的精确形状；输入仍由后端命中测试决定。

快照是历史值，不保留文档或原生对象。修改布局、替换或关闭文档后，不得把旧快照当作当前事实；执行依赖布局的动作前重新捕获。捕获不推进 context 时间、不消费输入事件，也不替代正常的帧资源消费与关闭协议。

## 控件值与焦点

`GetValue` 返回中立 `UiControlValue`：Text、Boolean、Number、Selection 或 None。None 表示当前元素没有可读取的控件值；失效文档不是 None，而是明确失败。`SetValue` 只接受匹配类型；数值必须有限，后端正常范围约束仍生效。程序设置不产生 Change/Submit 回声，业务 owner 自行校验与写回。

`UiEvent.value` 在事件产生时捕获；按顺序消费时不会读到后来的控件状态。`phase` 区分 Editing、Committed 和非编辑事件，`origin` 区分明确输入、程序调用和后端更新。表单提交不把任意字段字典塞入事件；调用方按自己的表单契约读取字段。关闭文档移除尚未提取的旧文档事件；已经提取的快照仍须核对当前 document handle，不能用于替换后的文档。

`FocusElement` 返回 Focused、Unchanged、NotFound 或 NotFocusable。`GetFocusedElement` 返回文档和 authored element ID；null 表示没有控件焦点，空 ID 表示匿名控件。`ClearFocus` 返回到文档焦点，遵守当前后端的 Modal 约束。鼠标、Tab 和显式 API 使用同一后端焦点事实，没有额外焦点管理器。

这些调用均属于 context owner thread。焦点快照和事件是中立值，不是文档生命周期 lease，不应跨关闭或 generation 切换使用。不同 context 的文档句柄不可混用。

## 时间与布局的独立操作

`Update(context, input)` 保留正常的组合更新。需要隔离预览时，分别使用 `ProcessInput`、`AdvanceTime` 和 `RefreshLayout`：前者只处理明确传入的输入；时间步进只修改时钟；布局刷新在当前时间更新样式和布局，不推进游戏或消费新的输入。`SetTimePaused` 只控制正常 Update 的自动时间推进，显式非负步进仍可用。

`GetTime` 返回不可变的秒数与暂停状态。不同 context 拥有独立时间，不能通过改写进程级全局时钟控制预览。调用 `RefreshLayout` 后可直接 `Render`，不要求先发送一次空输入。

`UiBackendCapability` 是开放的稳定能力 ID；后端贡献不可变集合，公共层不维护实现白名单。`TrySeekTime` 必须明确返回是否能够保持文档语义：不支持时返回 false 并保留原状态，不能通过重新加载文档伪装成功。具体动画离散步进规则见所选后端页面。

## 控制器输入快照

`UiInputSnapshot.gamepads` 是由输入 owner 授权的不可变控制器状态，和 `sequence` 一起交付。UI context 不自行枚举平台设备，也不能把其他 viewport 的状态并入当前输入。构造时验证连接身份与状态一致，输入传递不会修改原始设备样本或生成 Core 键盘事件。

## 当前源码公开 API 清单

此清单对应当前 SDK 求值与源码符号；内部类型不构成公开 API。

### `Inno.UI.IUiBackend`

<!-- api: T:Inno.UI.IUiBackend 6f62df37d8b1b67642a58e9e4abc367705cb9a4d19709fdf95625a8feb3c7020 -->
<!-- api: M:Inno.UI.IUiBackend.CaptureLayout(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.Collections.Generic.IReadOnlyList{System.String}) 04622f6b674294c3fdd3b0be9f77be8a02e81341a07a357c773ac1816352b6f3 -->
<!-- api: M:Inno.UI.IUiBackend.GetTime(Inno.UI.UiContextHandle) 0af914c91b9725815ac91d93a318113001fb863c41d0e70c3d0ad136d8f95b2b -->
<!-- api: M:Inno.UI.IUiBackend.SetTimePaused(Inno.UI.UiContextHandle,System.Boolean) 27b1dc5ee52c298f718b10c206d4017476fe7df1635ce77ff15bfc2c205684a0 -->
<!-- api: M:Inno.UI.IUiBackend.AdvanceTime(Inno.UI.UiContextHandle,System.Double) d7bdaef2226527bb3b8739c8f2684c09a758abf596137ba0e755c612c46f8229 -->
<!-- api: M:Inno.UI.IUiBackend.TrySeekTime(Inno.UI.UiContextHandle,System.Double) 114ed4dc7a737d96dcf19bf7e91a73f4b769d0b57d248aeda13acd26cffe39fb -->
<!-- api: M:Inno.UI.IUiBackend.RefreshLayout(Inno.UI.UiContextHandle) 7e285468868fc1092feadea7d18d0cc49e25661fcc2987d8f6d354a61d15216d -->
<!-- api: M:Inno.UI.IUiBackend.ProcessInput(Inno.UI.UiContextHandle,Inno.UI.UiInputSnapshot) 3b9e2e163e8270126f17342dbd9b38aa8a964b6f87d97954181015ad1cd131b8 -->
<!-- api: M:Inno.UI.IUiBackend.GetValue(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.String) ba09634508d9bceb009bd6de8496187756a4044bffb7ab010d23ea6521e238b1 -->
<!-- api: M:Inno.UI.IUiBackend.SetValue(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.String,Inno.UI.UiControlValue) 2a1f62adef1fe366a956b8bf4a26b371db07701df2d83b55dc9ba73689a8e679 -->
<!-- api: M:Inno.UI.IUiBackend.FocusElement(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.String) 32dc6edd019a30736e8412596b331ccdd8f0938d5878778ef694e1b1c2636d04 -->
<!-- api: M:Inno.UI.IUiBackend.GetFocusedElement(Inno.UI.UiContextHandle) d70ad19e4d1fc4adcefd0cef5c7d7138a55e37bc7abe27eacaab5c2446c22411 -->
<!-- api: M:Inno.UI.IUiBackend.ClearFocus(Inno.UI.UiContextHandle) 8a8a4aead37c5c8545d7428175f0418b87b40030531b97ae48b909c7a22aee63 -->
<!-- api: P:Inno.UI.IUiBackend.implementationId c6d6ff8336b6b5a542386a4fc1e61fddcfd715f9d47fada1085416009ad3bb7e -->
<!-- api: P:Inno.UI.IUiBackend.capabilities ec6c2f6ce09b7392afa2d32a3d7e4af8b2dff483f0060e0adba342d360067f85 -->
<!-- api: M:Inno.UI.IUiBackend.CreateContext(Inno.UI.UiContextOptions) e44daa1fd61067d5b7a4ae0cdd6ae21648270e926e292ed98c2565b1ec94ff79 -->
<!-- api: M:Inno.UI.IUiBackend.DestroyContext(Inno.UI.UiContextHandle) 5c5472a58e187bbd5ef635a3450eb5beadd92de358ce4e1b4f49543969651221 -->
<!-- api: M:Inno.UI.IUiBackend.SetViewport(Inno.UI.UiContextHandle,System.Int32,System.Int32,System.Single) c877f1ced36ca3a4183ab02ff5cf8a7883acb8e42ab7d8bbb5ed1bd85063af13 -->
<!-- api: M:Inno.UI.IUiBackend.LoadDocument(Inno.UI.UiContextHandle,Inno.UI.UiDocumentSource) 5afc2db7a5e56daa60796cd98fd0bb2736f10eea730c17b4ff0daa035dc49392 -->
<!-- api: M:Inno.UI.IUiBackend.ShowDocument(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle) 3c252be5b372ea194272c2afef65ad659973d3def1a4eb9b176967b74ad9799a -->
<!-- api: M:Inno.UI.IUiBackend.HideDocument(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle) 57c0472344ba2efa873ed2c4e995ecc3a9f9504634dc3d195747e83f1f2f4afe -->
<!-- api: M:Inno.UI.IUiBackend.CloseDocument(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle) 1eab1158cb4b65de04e0788f0a5d478c8859928a999e852ad309ea06ef92d4d9 -->
<!-- api: M:Inno.UI.IUiBackend.SetText(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.String,System.String) 874c04632ba94461ea1f07c118b62a06418e671ccddb8ee6057859b4cdd965d7 -->
<!-- api: M:Inno.UI.IUiBackend.SetContent(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.String,Inno.UI.UiDocumentFragment) 0c8ed96156fce9187f31ce69b51457a923f1311215b0a8bacf508ac5ea0d00c1 -->
<!-- api: M:Inno.UI.IUiBackend.SetAttribute(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.String,System.String,System.String) 9e121c86defc79aba8926cc79cea18667495a06ac48b42860699f39644725d74 -->
<!-- api: M:Inno.UI.IUiBackend.SetClass(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.String,System.String,System.Boolean) 5968555d83671b99498368801ad5434bbfaebc1669d428c43df8d9e9b9e767d1 -->
<!-- api: M:Inno.UI.IUiBackend.RegisterFont(Inno.UI.UiFontRegistration) 0e49ad43560ea462f42deb35b72dd48abbafa8dbd3f099bc44071a4821201610 -->
<!-- api: M:Inno.UI.IUiBackend.RegisterImage(Inno.UI.UiContextHandle,Inno.UI.UiImageSource) a803a68a68960ee5e129726a9897e2a5c2552974359a65431b146e2400cf9241 -->
<!-- api: M:Inno.UI.IUiBackend.RegisterTexture(Inno.UI.UiContextHandle,System.String,Inno.UI.UiTextureData) 64646af8e542ccd8f8ef1e8318e5709e4ba1942c693a61c14443a43f3e622af9 -->
<!-- api: M:Inno.UI.IUiBackend.HasElementAtPoint(Inno.UI.UiContextHandle,Inno.Core.Mathematics.Vector2) b8c9c58bc57c6b4dbe11d6b8477ca3e1042a9c6607ccad88ca49acf47da23e86 -->
<!-- api: M:Inno.UI.IUiBackend.Update(Inno.UI.UiContextHandle,Inno.UI.UiInputSnapshot) 00e03e55b978dd26f33b0a6f12d2b6b19ae8a8a02aa3ae166a35f27c712c19d8 -->
<!-- api: M:Inno.UI.IUiBackend.Render(Inno.UI.UiContextHandle) 6cf025c074e21847cc9f668bb99d9e93f0188f161f4c0182eb8b4aeef35d0bc0 -->
<!-- api: M:Inno.UI.IUiBackend.DrainEvents(Inno.UI.UiContextHandle) 978f9ac1ee044477af9f835a1f459f2da9d08fc9d974bb4a891821ae23f348eb -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-3c3a34e932be8f46"></a>[`public interface Inno.UI.IUiBackend : System.IDisposable`](../../src/services/ui/Inno.UI/IUiBackend.cs#L15) | Defines the replaceable document, layout, input, and draw-geometry backend boundary. | — |
| <a id="api-647fcf95ec6e0cbb"></a>[`Inno.UI.UiLayoutSnapshot Inno.UI.IUiBackend.CaptureLayout(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, System.Collections.Generic.IReadOnlyList<string> elementIds)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L38) | Refreshes visual layout once and captures a batch of element geometry without input or time advancement.<br>`context`：The live context owned by the calling thread.<br>`document`：The live document belonging to the context.<br>`elementIds`：A non-empty list of non-empty authored IDs in result order; missing and hidden elements remain explicit results.<br>返回：An immutable snapshot in context pixels. It does not retain a document lease or native memory. | `System.ArgumentException`：A requested ID is empty or an owner handle is invalid.<br>`System.ArgumentNullException`：The element list is null. |
| <a id="api-740e81f8da9f2cfd"></a>[`Inno.UI.UiContextTime Inno.UI.IUiBackend.GetTime(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L53) | Reads the context clock without advancing it.<br>`context`：The live context on its owner thread.<br>返回：The current elapsed seconds and pause state. | — |
| <a id="api-f6cdcae0bac590f3"></a>[`void Inno.UI.IUiBackend.SetTimePaused(Inno.UI.UiContextHandle context, bool paused)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L64) | Controls automatic time advancement; input remains independently routable.<br>`context`：The live context on its owner thread.<br>`paused`：True to freeze automatic time; explicit steps remain available. | — |
| <a id="api-d0845f1c25544d1f"></a>[`void Inno.UI.IUiBackend.AdvanceTime(Inno.UI.UiContextHandle context, double seconds)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L81) | Advances only the context clock without processing input or refreshing layout.<br>`context`：The live context on its owner thread.<br>`seconds`：Finite nonnegative time to advance, including while paused. | `System.ArgumentOutOfRangeException`：The step is negative, nonfinite or overflows the clock. |
| <a id="api-082b7606b8d965fb"></a>[`bool Inno.UI.IUiBackend.TrySeekTime(Inno.UI.UiContextHandle context, double seconds)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L101) | Requests an absolute time seek only when the backend can preserve document semantics.<br>`context`：The live context on its owner thread.<br>`seconds`：The finite nonnegative requested time.<br>返回：True if applied; false if unsupported, with all state unchanged. | `System.ArgumentOutOfRangeException`：The requested time is negative or nonfinite. |
| <a id="api-8eb16d5e08c0e9e2"></a>[`void Inno.UI.IUiBackend.RefreshLayout(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L112) | Refreshes document style and layout at the current context time without processing input.<br>`context`：The live context on its owner thread. | — |
| <a id="api-7d8ac14269557038"></a>[`void Inno.UI.IUiBackend.ProcessInput(Inno.UI.UiContextHandle context, Inno.UI.UiInputSnapshot input)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L123) | Processes the explicitly routed input without advancing time or updating the whole context.<br>`context`：The live context on its owner thread.<br>`input`：The current immutable input sample. | — |
| <a id="api-a867c9605b884aad"></a>[`Inno.UI.UiControlValue Inno.UI.IUiBackend.GetValue(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L147) | Reads a control value on the context owner thread without changing layout or input.<br>`context`：The live context owning the document.<br>`document`：The live document containing the control.<br>`elementId`：The nonempty authored element ID.<br>返回：The current value, or None for a missing element or unsupported control. | `System.InvalidOperationException`：A handle is stale or the caller violates ownership. |
| <a id="api-0b1a2ef872c023ee"></a>[`bool Inno.UI.IUiBackend.SetValue(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, Inno.UI.UiControlValue value)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L177) | Writes a matching control value without generating value-change or submit events.<br>`context`：The live context owning the document.<br>`document`：The live document containing the control.<br>`elementId`：The nonempty authored element ID.<br>`value`：The typed value; normal control clamping still applies.<br>返回：True when the control accepts the value; false when absent, unsupported or no matching option exists. | `System.ArgumentException`：The value kind does not match the control.<br>`System.InvalidOperationException`：A handle is stale or the caller violates ownership. |
| <a id="api-16ca219721420d53"></a>[`Inno.UI.UiFocusResult Inno.UI.IUiBackend.FocusElement(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L206) | Requests focus through the context's existing focus and modal policy on its owner thread.<br>`context`：The context owning the document.<br>`document`：The live document containing the target.<br>`elementId`：The nonempty authored element ID.<br>返回：The applied, unchanged, missing or ineligible request outcome. | `System.ArgumentException`：The element ID is empty.<br>`System.InvalidOperationException`：A handle is stale, belongs to another context or the caller violates ownership. |
| <a id="api-a43f040c544ae5ca"></a>[`Inno.UI.UiFocusTarget? Inno.UI.IUiBackend.GetFocusedElement(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L224) | Reads the current focused control without changing layout or focus.<br>`context`：The live context to query on its owner thread.<br>返回：A neutral focus snapshot, or null when no document control has focus. | `System.InvalidOperationException`：The context is stale or the caller violates ownership. |
| <a id="api-9d3cd17fa531d929"></a>[`bool Inno.UI.IUiBackend.ClearFocus(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L238) | Returns focus to the owning document, preserving modal ownership and emitting normal blur events.<br>`context`：The live context to modify on its owner thread.<br>返回：True when no document control retains focus; false when focus policy refuses the change. | `System.InvalidOperationException`：The context is stale or the caller violates ownership. |
| <a id="api-da467b8a51c5f1d9"></a>[`string Inno.UI.IUiBackend.implementationId { get; }`](../../src/services/ui/Inno.UI/IUiBackend.cs#L243) | Gets the stable implementation identity used by imported document artifacts. | — |
| <a id="api-03e29f1cbf38ac5c"></a>[`Inno.UI.UiBackendCapabilities Inno.UI.IUiBackend.capabilities { get; }`](../../src/services/ui/Inno.UI/IUiBackend.cs#L247) | Gets optional behavior and accepted document languages for this generation. | — |
| <a id="api-7f236147ed2fbe5a"></a>[`Inno.UI.UiContextHandle Inno.UI.IUiBackend.CreateContext(Inno.UI.UiContextOptions options)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L257) | Creates one independent UI context.<br>`options`：Initial layout and density for the new context.<br>返回：A handle owned by the caller until destruction. | — |
| <a id="api-a81fa4e343fc9464"></a>[`void Inno.UI.IUiBackend.DestroyContext(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L264) | Destroys one UI context and all of its documents.<br>`context`：The context that owns the requested document or state. | — |
| <a id="api-9f20c724fe53c984"></a>[`void Inno.UI.IUiBackend.SetViewport(Inno.UI.UiContextHandle context, int width, int height, float density)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L280) | Changes one UI context's pixel dimensions and density.<br>`context`：The context that owns the requested document or state.<br>`width`：New viewport width in physical pixels.<br>`height`：New viewport height in physical pixels.<br>`density`：Physical pixels per logical layout pixel. | — |
| <a id="api-49843a8ae59a9d08"></a>[`Inno.UI.UiDocumentHandle Inno.UI.IUiBackend.LoadDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentSource source)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L298) | Loads one explicitly tagged in-memory document.<br>`context`：The context that owns the requested document or state.<br>`source`：Tagged source or named texture source for this operation.<br>返回：A document handle owned by the supplied context. | — |
| <a id="api-b78135d454bd57ec"></a>[`void Inno.UI.IUiBackend.ShowDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L311) | Shows one document.<br>`context`：The context that owns the requested document or state.<br>`document`：The document owned by the supplied context. | — |
| <a id="api-3d7db7e80d8d59c7"></a>[`void Inno.UI.IUiBackend.HideDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L324) | Hides one document.<br>`context`：The context that owns the requested document or state.<br>`document`：The document owned by the supplied context. | — |
| <a id="api-c9564f5b91bf66e4"></a>[`void Inno.UI.IUiBackend.CloseDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L337) | Closes one document.<br>`context`：The context that owns the requested document or state.<br>`document`：The document owned by the supplied context. | — |
| <a id="api-81499af7b12968e4"></a>[`bool Inno.UI.IUiBackend.SetText(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, string text)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L359) | Replaces an element's children with plain Unicode text.<br>`context`：The context that owns the requested document or state.<br>`document`：The document owned by the supplied context.<br>`elementId`：The target document element ID.<br>`text`：Escaped Unicode replacement text.<br>返回：True when the element exists and its text was changed. | — |
| <a id="api-d53304a7e7cf472b"></a>[`bool Inno.UI.IUiBackend.SetContent(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, Inno.UI.UiDocumentFragment fragment)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L383) | Replaces an element's children with an explicitly tagged source-language fragment.<br>`context`：The context that owns the requested document or state.<br>`document`：The document owned by the supplied context.<br>`elementId`：The target document element ID.<br>`fragment`：Explicitly tagged replacement document fragment.<br>返回：True when the element exists and its content was changed. | — |
| <a id="api-3002e60e88e23f52"></a>[`bool Inno.UI.IUiBackend.SetAttribute(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, string name, string value)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L410) | Sets one element attribute.<br>`context`：The context that owns the requested document or state.<br>`document`：The document owned by the supplied context.<br>`elementId`：The target document element ID.<br>`name`：Attribute name to change.<br>`value`：New attribute value.<br>返回：True when the element exists and its attribute was changed. | — |
| <a id="api-f44a7bf425e22779"></a>[`bool Inno.UI.IUiBackend.SetClass(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, string className, bool active)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L438) | Activates or deactivates one element class.<br>`context`：The context that owns the requested document or state.<br>`document`：The document owned by the supplied context.<br>`elementId`：The target document element ID.<br>`className`：CSS class name to toggle.<br>`active`：Whether the CSS class is enabled.<br>返回：True when the element exists and its class was changed. | — |
| <a id="api-ddda6b9f478471b6"></a>[`void Inno.UI.IUiBackend.RegisterFont(Inno.UI.UiFontRegistration registration)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L451) | Registers an encoded font face in the document engine.<br>`registration`：Font bytes and family metadata to register. | — |
| <a id="api-5dd275f3464c27db"></a>[`Inno.UI.IUiImageRegistration Inno.UI.IUiBackend.RegisterImage(Inno.UI.UiContextHandle context, Inno.UI.UiImageSource image)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L470) | Registers an imported image without decoding pixels or acquiring a second GPU resource.<br>`context`：Owning context on the current UI thread.<br>`image`：Frozen artifact identity and imported dimensions.<br>返回：A context-local source registration requiring deterministic disposal. | `System.ArgumentNullException`：The image is null.<br>`System.InvalidOperationException`：The context is unavailable or the caller is not its owner. |
| <a id="api-6a721a8531954866"></a>[`void Inno.UI.IUiBackend.RegisterTexture(Inno.UI.UiContextHandle context, string source, Inno.UI.UiTextureData texture)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L487) | Registers an owned pixel source for a UI context.<br>`context`：Owning UI context.<br>`source`：Document image source.<br>`texture`：Pixel data copied by the backend. | — |
| <a id="api-91408311f0710b7b"></a>[`bool Inno.UI.IUiBackend.HasElementAtPoint(Inno.UI.UiContextHandle context, Inno.Core.Mathematics.Vector2 position)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L504) | Tests whether a visible document element receives pointer events at a pixel location.<br>`context`：The context that owns the requested document or state.<br>`position`：Pointer position in context pixels.<br>返回：True when an interactive visible element occupies the location. | — |
| <a id="api-c1567b886ff461ae"></a>[`void Inno.UI.IUiBackend.Update(Inno.UI.UiContextHandle context, Inno.UI.UiInputSnapshot input)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L517) | Processes input and advances document state.<br>`context`：The context that owns the requested document or state.<br>`input`：The input snapshot routed to this context. | — |
| <a id="api-aa98157bebb89b4b"></a>[`Inno.UI.UiRenderFrame Inno.UI.IUiBackend.Render(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L530) | Builds an immutable frame of incremental resources and ordered draws.<br>`context`：The context that owns the requested document or state.<br>返回：An immutable frame of ordered UI draws and resources. | — |
| <a id="api-a2eea65dbd3a8379"></a>[`System.Collections.Generic.IReadOnlyList<Inno.UI.UiEvent> Inno.UI.IUiBackend.DrainEvents(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L540) | Drains document events queued by the preceding updates.<br>`context`：The context that owns the requested document or state.<br>返回：Events emitted since the preceding drain, in dispatch order. | — |

### `Inno.UI.IUiImageRegistration`

<!-- api: T:Inno.UI.IUiImageRegistration fc49dd0c6d629e206f155c91e8f294466689b890827f0d9b18727d1f1ab1c52e -->
<!-- api: P:Inno.UI.IUiImageRegistration.source 61374ceca86e91ef01bfaa32b28a8b91a6a1cbcc1d49a2db8d008fa2f0b701a1 -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-272f0fe1a29d5d21"></a>[`public interface Inno.UI.IUiImageRegistration : System.IDisposable`](../../src/services/ui/Inno.UI/IUiImageRegistration.cs#L14) | Owns one context-local image source. Dispose on the UI owner thread to invalidate that source and retire its UI texture handles; shared Rendering assets remain owned by Rendering. | — |
| <a id="api-1028842906a40c8d"></a>[`string Inno.UI.IUiImageRegistration.source { get; }`](../../src/services/ui/Inno.UI/IUiImageRegistration.cs#L19) | Gets the opaque source URI accepted by document image elements and styles. | — |

### `Inno.UI.IUiService`

<!-- api: T:Inno.UI.IUiService 431e83960c3dbb4c1d5f13e4c03b8e2d1d27d24317ffe79ab2c70c6569c2e87f -->
<!-- api: M:Inno.UI.IUiService.CaptureLayout(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.Collections.Generic.IReadOnlyList{System.String}) acafbd661b12140a0cfc4cd038595ca3c3022a531d00ea36723aa5abff92a026 -->
<!-- api: P:Inno.UI.IUiService.capabilities fa790bfd7392da37fa43e91908b3115033660d663a068bd0ca47bd7867598ad8 -->
<!-- api: M:Inno.UI.IUiService.GetTime(Inno.UI.UiContextHandle) 1a8f69698d2e58ab3fe909a607b78ac6f57be14e1322fc6c16b79b34fa533547 -->
<!-- api: M:Inno.UI.IUiService.SetTimePaused(Inno.UI.UiContextHandle,System.Boolean) 0b2822ab4dff0fe59c09f3605607779d40321efe2bfceebbda38ecf782f8f04e -->
<!-- api: M:Inno.UI.IUiService.AdvanceTime(Inno.UI.UiContextHandle,System.Double) f3414d0da70813216dcdefc610f5f94a799b250306c751e24c3033c83003c208 -->
<!-- api: M:Inno.UI.IUiService.TrySeekTime(Inno.UI.UiContextHandle,System.Double) 6f715451db537b8be683731cb45f31aebe520a220cac54480e71d1db0056d11f -->
<!-- api: M:Inno.UI.IUiService.RefreshLayout(Inno.UI.UiContextHandle) 76492c476b5c27cb3fc45c11150a2896f3a2e492ed70011db1c3330d03dc5aac -->
<!-- api: M:Inno.UI.IUiService.ProcessInput(Inno.UI.UiContextHandle,Inno.UI.UiInputSnapshot) 6e8a7b7c942e9b07af6d738e9642dc2afc7cec0d02e1bc9001af75aa70afe34a -->
<!-- api: M:Inno.UI.IUiService.GetValue(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.String) 331612d3d84c23fd596a9e0b4bf0faca1eb1e071567fd5966b561aa71a4be081 -->
<!-- api: M:Inno.UI.IUiService.SetValue(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.String,Inno.UI.UiControlValue) f804d14a82ebf9f56a9a39522accd6492dfda8961fc14a676e75b19124fc3f69 -->
<!-- api: M:Inno.UI.IUiService.FocusElement(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.String) 3fe65dc93a2f58ce85229a9a5373f3ed155261398b88b94043e71212425eb8b6 -->
<!-- api: M:Inno.UI.IUiService.GetFocusedElement(Inno.UI.UiContextHandle) 980723ca38592ad1f7ea64c067942609cdad40d6d6184556c019439ff329204d -->
<!-- api: M:Inno.UI.IUiService.ClearFocus(Inno.UI.UiContextHandle) 52063e91908561c1eaae0fcb63a23f1885222be5664f3d212a8841fb5a6f3229 -->
<!-- api: M:Inno.UI.IUiService.CreateContext(Inno.UI.UiContextOptions) fdece6e174d013def21a92d8f091450f452862b2726a59475c617f485aa9afcb -->
<!-- api: M:Inno.UI.IUiService.DestroyContext(Inno.UI.UiContextHandle) ce2b35fd844cc1237fdb842336347349853cc127d720ccc0a32d710d4a78efc4 -->
<!-- api: M:Inno.UI.IUiService.SetViewport(Inno.UI.UiContextHandle,System.Int32,System.Int32,System.Single) 8ffd6965a850cab600aa0f74f6f23cfe0f24239ff810dfb811e629f1cbb92bc9 -->
<!-- api: M:Inno.UI.IUiService.LoadDocument(Inno.UI.UiContextHandle,Inno.UI.UiDocumentSource) 559480d9ed280e8f8571a255e62fae9303b9deef1c62e0ae9a7fdff5d16370b2 -->
<!-- api: M:Inno.UI.IUiService.LoadDocument(Inno.UI.UiContextHandle,Inno.UI.UiDocumentAsset) 74e2c294361c10e56b8905bab80d20f0fba058ab0ede8a5374ecbae9761a7597 -->
<!-- api: M:Inno.UI.IUiService.ShowDocument(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle) 65586e48736fdf712bcb3f892c53e7e975794dd11b826a626cfd369ffeb2cd29 -->
<!-- api: M:Inno.UI.IUiService.HideDocument(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle) b1da5630d484325018cd8dfbfcf463317a563e4f4dd6c1a424c8cb988a3e2ff1 -->
<!-- api: M:Inno.UI.IUiService.CloseDocument(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle) 7e1647f15187ce822ac674cd4a521358e8944a77c2af00822c3e5994e6d0c90d -->
<!-- api: M:Inno.UI.IUiService.SetText(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.String,System.String) 1f71716a84d6e1e6d675b2e27697dbf791a666b67e479374107d7235618f231f -->
<!-- api: M:Inno.UI.IUiService.SetContent(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.String,Inno.UI.UiDocumentFragment) 91a040a47e344e4214c2353c37dd37eea282e3c09c51d677bca86ee9abf338a1 -->
<!-- api: M:Inno.UI.IUiService.SetAttribute(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.String,System.String,System.String) f20c9bec107666b5f6ffa20e169427262c6b1564ccce662903c56e55adb59c1e -->
<!-- api: M:Inno.UI.IUiService.SetClass(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.String,System.String,System.Boolean) 03b8bf9016326a242296d19d4302a7e5763fad2f9bbad38cf48d9fc3684db617 -->
<!-- api: M:Inno.UI.IUiService.RegisterImage(Inno.UI.UiContextHandle,Inno.UI.UiImageSource) 8ad83403f7bdb5942288f5b2057d5b6bf296b7c3f44efb13d0ff78f994d21283 -->
<!-- api: M:Inno.UI.IUiService.RegisterTexture(Inno.UI.UiContextHandle,System.String,Inno.UI.UiTextureData) 589387c603b0d18ca76695f6ef47942a5a9893954c9a1b1df0dd281f94c90544 -->
<!-- api: M:Inno.UI.IUiService.HasElementAtPoint(Inno.UI.UiContextHandle,Inno.Core.Mathematics.Vector2) bcaf94a0720c596d70b60154ec42e3797e2a3787d707a8c55236e94e15f5798b -->
<!-- api: M:Inno.UI.IUiService.Update(Inno.UI.UiContextHandle) 518fc634187eeac806d44deacaafe59e3c8c17c8e4e9d3ad8a97dd2593e5ce69 -->
<!-- api: M:Inno.UI.IUiService.Update(Inno.UI.UiContextHandle,Inno.UI.UiInputSnapshot) 589ff5660be38370d1a429f332c2a0b9070eedcb268b7be7d6913f8bba61777b -->
<!-- api: M:Inno.UI.IUiService.Render(Inno.UI.UiContextHandle) b809e6d5e31e08cf632d263266018e1eeb31c9f2cc123dc2c73ff75396712e04 -->
<!-- api: M:Inno.UI.IUiService.DrainEvents(Inno.UI.UiContextHandle) e689f9f74c92f5b3ac586895ebce5d43aff1c29156368cdb49e88c44cd352cba -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-bbd7db1ad0963475"></a>[`public interface Inno.UI.IUiService`](../../src/services/ui/Inno.UI/IUiService.cs#L11) | Provides retained-mode documents and backend-neutral UI frames to scripts and rendering plugins. | — |
| <a id="api-24f2baebe9c8f709"></a>[`Inno.UI.UiLayoutSnapshot Inno.UI.IUiService.CaptureLayout(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, System.Collections.Generic.IReadOnlyList<string> elementIds)`](../../src/services/ui/Inno.UI/IUiService.cs#L34) | Refreshes visual layout once and captures a batch of element geometry without input or time advancement.<br>`context`：The live context owned by the calling thread.<br>`document`：The live document belonging to the context.<br>`elementIds`：A non-empty list of non-empty authored IDs in result order; missing and hidden elements remain explicit results.<br>返回：An immutable snapshot in context pixels. It does not retain a document lease or native memory. | `System.ArgumentException`：A requested ID is empty or an owner handle is invalid.<br>`System.ArgumentNullException`：The element list is null. |
| <a id="api-839f7d4a5c38bd9f"></a>[`Inno.UI.UiBackendCapabilities Inno.UI.IUiService.capabilities { get; }`](../../src/services/ui/Inno.UI/IUiService.cs#L43) | Gets immutable optional capabilities of this service generation. | — |
| <a id="api-be9fd46e96f7cb25"></a>[`Inno.UI.UiContextTime Inno.UI.IUiService.GetTime(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI/IUiService.cs#L54) | Reads the context clock without advancing it.<br>`context`：The live context on its owner thread.<br>返回：The current elapsed seconds and pause state. | — |
| <a id="api-97c59e923131368d"></a>[`void Inno.UI.IUiService.SetTimePaused(Inno.UI.UiContextHandle context, bool paused)`](../../src/services/ui/Inno.UI/IUiService.cs#L65) | Controls automatic time advancement; input remains independently routable.<br>`context`：The live context on its owner thread.<br>`paused`：True to freeze automatic time; explicit steps remain available. | — |
| <a id="api-ab7fd12fc368e63d"></a>[`void Inno.UI.IUiService.AdvanceTime(Inno.UI.UiContextHandle context, double seconds)`](../../src/services/ui/Inno.UI/IUiService.cs#L82) | Advances only the context clock without processing input or refreshing layout.<br>`context`：The live context on its owner thread.<br>`seconds`：Finite nonnegative time to advance, including while paused. | `System.ArgumentOutOfRangeException`：The step is negative, nonfinite or overflows the clock. |
| <a id="api-7f704086ced78933"></a>[`bool Inno.UI.IUiService.TrySeekTime(Inno.UI.UiContextHandle context, double seconds)`](../../src/services/ui/Inno.UI/IUiService.cs#L102) | Requests an absolute time seek only when the backend can preserve document semantics.<br>`context`：The live context on its owner thread.<br>`seconds`：The finite nonnegative requested time.<br>返回：True if applied; false if unsupported, with all state unchanged. | `System.ArgumentOutOfRangeException`：The requested time is negative or nonfinite. |
| <a id="api-5c2d798fd60c0ee9"></a>[`void Inno.UI.IUiService.RefreshLayout(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI/IUiService.cs#L113) | Refreshes document style and layout at the current context time without processing input.<br>`context`：The live context on its owner thread. | — |
| <a id="api-01ae4a77fc0b9cde"></a>[`void Inno.UI.IUiService.ProcessInput(Inno.UI.UiContextHandle context, Inno.UI.UiInputSnapshot input)`](../../src/services/ui/Inno.UI/IUiService.cs#L124) | Processes the explicitly routed input without advancing time or updating the whole context.<br>`context`：The live context on its owner thread.<br>`input`：The current immutable input sample. | — |
| <a id="api-41b62f6fb233a880"></a>[`Inno.UI.UiControlValue Inno.UI.IUiService.GetValue(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId)`](../../src/services/ui/Inno.UI/IUiService.cs#L148) | Reads a control value on the context owner thread without changing layout or input.<br>`context`：The live context owning the document.<br>`document`：The live document containing the control.<br>`elementId`：The nonempty authored element ID.<br>返回：The current value, or None for a missing element or unsupported control. | `System.InvalidOperationException`：A handle is stale or the caller violates ownership. |
| <a id="api-968b5781c3b2cf0e"></a>[`bool Inno.UI.IUiService.SetValue(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, Inno.UI.UiControlValue value)`](../../src/services/ui/Inno.UI/IUiService.cs#L178) | Writes a matching control value without generating value-change or submit events.<br>`context`：The live context owning the document.<br>`document`：The live document containing the control.<br>`elementId`：The nonempty authored element ID.<br>`value`：The typed value; normal control clamping still applies.<br>返回：True when the control accepts the value; false when absent, unsupported or no matching option exists. | `System.ArgumentException`：The value kind does not match the control.<br>`System.InvalidOperationException`：A handle is stale or the caller violates ownership. |
| <a id="api-599737008160ef0c"></a>[`Inno.UI.UiFocusResult Inno.UI.IUiService.FocusElement(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId)`](../../src/services/ui/Inno.UI/IUiService.cs#L207) | Requests focus through the context's existing focus and modal policy on its owner thread.<br>`context`：The context owning the document.<br>`document`：The live document containing the target.<br>`elementId`：The nonempty authored element ID.<br>返回：The applied, unchanged, missing or ineligible request outcome. | `System.ArgumentException`：The element ID is empty.<br>`System.InvalidOperationException`：A handle is stale, belongs to another context or the caller violates ownership. |
| <a id="api-8e94df78bc46f159"></a>[`Inno.UI.UiFocusTarget? Inno.UI.IUiService.GetFocusedElement(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI/IUiService.cs#L225) | Reads the current focused control without changing layout or focus.<br>`context`：The live context to query on its owner thread.<br>返回：A neutral focus snapshot, or null when no document control has focus. | `System.InvalidOperationException`：The context is stale or the caller violates ownership. |
| <a id="api-c7f375247de73a48"></a>[`bool Inno.UI.IUiService.ClearFocus(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI/IUiService.cs#L239) | Returns focus to the owning document, preserving modal ownership and emitting normal blur events.<br>`context`：The live context to modify on its owner thread.<br>返回：True when no document control retains focus; false when focus policy refuses the change. | `System.InvalidOperationException`：The context is stale or the caller violates ownership. |
| <a id="api-41c4ba7ae7a4314e"></a>[`Inno.UI.UiContextHandle Inno.UI.IUiService.CreateContext(Inno.UI.UiContextOptions options)`](../../src/services/ui/Inno.UI/IUiService.cs#L250) | Creates one independent UI context.<br>`options`：Initial layout and density for the new context.<br>返回：A handle owned by the caller until destruction. | — |
| <a id="api-aed90191b0b97f1f"></a>[`void Inno.UI.IUiService.DestroyContext(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI/IUiService.cs#L257) | Destroys one UI context and all owned documents.<br>`context`：The context that owns the requested document or state. | — |
| <a id="api-d46000fc3c6b4498"></a>[`void Inno.UI.IUiService.SetViewport(Inno.UI.UiContextHandle context, int width, int height, float density = 1)`](../../src/services/ui/Inno.UI/IUiService.cs#L273) | Changes one context's pixel dimensions and density.<br>`context`：The context that owns the requested document or state.<br>`width`：New viewport width in physical pixels.<br>`height`：New viewport height in physical pixels.<br>`density`：Physical pixels per logical layout pixel. | — |
| <a id="api-5868adcb758759db"></a>[`Inno.UI.UiDocumentHandle Inno.UI.IUiService.LoadDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentSource source)`](../../src/services/ui/Inno.UI/IUiService.cs#L291) | Loads one explicitly tagged in-memory document.<br>`context`：The context that owns the requested document or state.<br>`source`：Tagged source or named texture source for this operation.<br>返回：A document handle owned by the supplied context. | — |
| <a id="api-ed1533f655339b49"></a>[`Inno.UI.UiDocumentHandle Inno.UI.IUiService.LoadDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentAsset document)`](../../src/services/ui/Inno.UI/IUiService.cs#L307) | Loads one imported document compatible with the selected backend.<br>`context`：The context that owns the requested document or state.<br>`document`：The document owned by the supplied context.<br>返回：A document handle owned by the supplied context. | — |
| <a id="api-3be1aeaf62c417b6"></a>[`void Inno.UI.IUiService.ShowDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document)`](../../src/services/ui/Inno.UI/IUiService.cs#L320) | Shows one document.<br>`context`：The context that owns the requested document or state.<br>`document`：The document owned by the supplied context. | — |
| <a id="api-c4680ef755a9a6ec"></a>[`void Inno.UI.IUiService.HideDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document)`](../../src/services/ui/Inno.UI/IUiService.cs#L333) | Hides one document.<br>`context`：The context that owns the requested document or state.<br>`document`：The document owned by the supplied context. | — |
| <a id="api-6c2637da0455babb"></a>[`void Inno.UI.IUiService.CloseDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document)`](../../src/services/ui/Inno.UI/IUiService.cs#L346) | Closes one document.<br>`context`：The context that owns the requested document or state.<br>`document`：The document owned by the supplied context. | — |
| <a id="api-ffaa051b6e6882af"></a>[`bool Inno.UI.IUiService.SetText(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, string text)`](../../src/services/ui/Inno.UI/IUiService.cs#L368) | Replaces an element's children with plain Unicode text.<br>`context`：The context that owns the requested document or state.<br>`document`：The document owned by the supplied context.<br>`elementId`：The target document element ID.<br>`text`：Escaped Unicode replacement text.<br>返回：True when the element exists and its text was changed. | — |
| <a id="api-588b99472472bcfc"></a>[`bool Inno.UI.IUiService.SetContent(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, Inno.UI.UiDocumentFragment fragment)`](../../src/services/ui/Inno.UI/IUiService.cs#L392) | Replaces an element's children with an explicitly tagged source-language fragment.<br>`context`：The context that owns the requested document or state.<br>`document`：The document owned by the supplied context.<br>`elementId`：The target document element ID.<br>`fragment`：Explicitly tagged replacement document fragment.<br>返回：True when the element exists and its content was changed. | — |
| <a id="api-70a6a480fcb7d2a3"></a>[`bool Inno.UI.IUiService.SetAttribute(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, string name, string value)`](../../src/services/ui/Inno.UI/IUiService.cs#L419) | Sets one element attribute.<br>`context`：The context that owns the requested document or state.<br>`document`：The document owned by the supplied context.<br>`elementId`：The target document element ID.<br>`name`：Attribute name to change.<br>`value`：New attribute value.<br>返回：True when the element exists and its attribute was changed. | — |
| <a id="api-42ee72d6e3aa3aff"></a>[`bool Inno.UI.IUiService.SetClass(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, string className, bool active)`](../../src/services/ui/Inno.UI/IUiService.cs#L447) | Activates or deactivates one element class.<br>`context`：The context that owns the requested document or state.<br>`document`：The document owned by the supplied context.<br>`elementId`：The target document element ID.<br>`className`：CSS class name to toggle.<br>`active`：Whether the CSS class is enabled.<br>返回：True when the element exists and its class was changed. | — |
| <a id="api-ca8beaac434ca487"></a>[`Inno.UI.IUiImageRegistration Inno.UI.IUiService.RegisterImage(Inno.UI.UiContextHandle context, Inno.UI.UiImageSource image)`](../../src/services/ui/Inno.UI/IUiService.cs#L472) | Registers an imported image without decoding pixels or acquiring a second GPU resource.<br>`context`：Owning context on the current UI thread.<br>`image`：Frozen artifact identity and imported dimensions.<br>返回：A context-local source registration requiring deterministic disposal. | `System.ArgumentNullException`：The image is null.<br>`System.InvalidOperationException`：The context is unavailable or the caller is not its owner. |
| <a id="api-c0330459bbab41a5"></a>[`void Inno.UI.IUiService.RegisterTexture(Inno.UI.UiContextHandle context, string source, Inno.UI.UiTextureData texture)`](../../src/services/ui/Inno.UI/IUiService.cs#L489) | Registers an owned pixel source for a UI context.<br>`context`：Owning UI context.<br>`source`：Document image source.<br>`texture`：Pixel data copied by the backend. | — |
| <a id="api-a9d2769ff278b946"></a>[`bool Inno.UI.IUiService.HasElementAtPoint(Inno.UI.UiContextHandle context, Inno.Core.Mathematics.Vector2 position)`](../../src/services/ui/Inno.UI/IUiService.cs#L506) | Tests whether a visible document element receives pointer events at a pixel location.<br>`context`：The context that owns the requested document or state.<br>`position`：Pointer position in context pixels.<br>返回：True when an interactive visible element occupies the location. | — |
| <a id="api-fb5491fe9aa4f914"></a>[`void Inno.UI.IUiService.Update(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI/IUiService.cs#L516) | Processes current-frame input and advances one context.<br>`context`：The context that owns the requested document or state. | — |
| <a id="api-9f6207df3765650f"></a>[`void Inno.UI.IUiService.Update(Inno.UI.UiContextHandle context, Inno.UI.UiInputSnapshot input)`](../../src/services/ui/Inno.UI/IUiService.cs#L526) | Advances one context with explicitly routed viewport-local input.<br>`context`：The context that owns the requested document or state.<br>`input`：The input snapshot routed to this context. | — |
| <a id="api-6c83953ae39baf14"></a>[`Inno.UI.UiRenderFrame Inno.UI.IUiService.Render(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI/IUiService.cs#L539) | Builds an immutable frame of incremental resources and ordered draws.<br>`context`：The context that owns the requested document or state.<br>返回：An immutable frame of ordered UI draws and resources. | — |
| <a id="api-ad68ccb596ac3714"></a>[`System.Collections.Generic.IReadOnlyList<Inno.UI.UiEvent> Inno.UI.IUiService.DrainEvents(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI/IUiService.cs#L549) | Drains document events queued by preceding updates.<br>`context`：The context that owns the requested document or state.<br>返回：Events emitted since the preceding drain, in dispatch order. | — |

### `Inno.UI.UiExecutionContext`

<!-- api: T:Inno.UI.UiExecutionContext 449827572ba077d73b957c5ceaabb5558aa531ad1e37ffa59cfd35a3a6110339 -->
<!-- api: P:Inno.UI.UiExecutionContext.current f7ec1095423827966a965d5860f623fc110b5b558be80be8a1db45169f847d99 -->
<!-- api: M:Inno.UI.UiExecutionContext.EnterScope(Inno.UI.IUiService) 5a4143c2e801f7e836a577bfc76e4dc6a65c9af249c51bb151b9eefab8963baa -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-76f09d8d79a995a1"></a>[`public static class Inno.UI.UiExecutionContext`](../../src/services/ui/Inno.UI/UI.cs#L11) | Binds one UI service to the current asynchronous execution context. | — |
| <a id="api-56fb25775bca139d"></a>[`public static Inno.UI.IUiService Inno.UI.UiExecutionContext.current { get; }`](../../src/services/ui/Inno.UI/UI.cs#L18) | Gets the UI service bound to the current execution context. | — |
| <a id="api-796005657aa6ffca"></a>[`public static System.IDisposable Inno.UI.UiExecutionContext.EnterScope(Inno.UI.IUiService ui)`](../../src/services/ui/Inno.UI/UI.cs#L29) | Binds a UI service until the returned strict last-in-first-out scope is disposed.<br>`ui`：The host-owned UI service.<br>返回：The caller-owned binding scope. | — |

### `Inno.UI.UI`

<!-- api: T:Inno.UI.UI ddf125ee7c5b9b90f60d0d291ac787a7d5e7ee6a88a582cd40465ebca5250d76 -->
<!-- api: M:Inno.UI.UI.CaptureLayout(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.Collections.Generic.IReadOnlyList{System.String}) 40e4374a423d99e709e8bd670877b97441938a5b939b3b360a23c1204dbe68e9 -->
<!-- api: P:Inno.UI.UI.capabilities 2f84c932db5a8c9363943a7ee3c92d959525fbb280555e88a3d4d229efcf4358 -->
<!-- api: M:Inno.UI.UI.GetTime(Inno.UI.UiContextHandle) ed62c21fc299b05b326c65862e4ebc5b67ad1c71cfed9a616b2c4a6ef8663bba -->
<!-- api: M:Inno.UI.UI.SetTimePaused(Inno.UI.UiContextHandle,System.Boolean) 2159e17dd454026821e364402c5d5c0fbe6b80967c17b3a176043dad81b40eb9 -->
<!-- api: M:Inno.UI.UI.AdvanceTime(Inno.UI.UiContextHandle,System.Double) 7ad69d6f453f653e37806d8168a821fb56be2073ded08a50e66e29e3e20eca58 -->
<!-- api: M:Inno.UI.UI.TrySeekTime(Inno.UI.UiContextHandle,System.Double) 1cd23f48c200341fa0ad4d9289fafb68e00c143ca6efb1e21d0a0e63f6b78dce -->
<!-- api: M:Inno.UI.UI.RefreshLayout(Inno.UI.UiContextHandle) ff54ed1f54446101b54e69c7936985bb28486b6a1d2eea16e0d983fd0236bde3 -->
<!-- api: M:Inno.UI.UI.ProcessInput(Inno.UI.UiContextHandle,Inno.UI.UiInputSnapshot) 7541f3eec68f3a88b271a3938ac39728f9d68e1dd0d4937a9bb3a13abfa4605e -->
<!-- api: M:Inno.UI.UI.GetValue(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.String) 9c3f6ce851d4347cfdd2b9ac730a682ebfc8bc744d21f8867dd9050ae166c6a2 -->
<!-- api: M:Inno.UI.UI.SetValue(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.String,Inno.UI.UiControlValue) 357104788b1e8502cb8b18a3abd2cd38d45a753db3bcdce62b139cc7a8377236 -->
<!-- api: M:Inno.UI.UI.FocusElement(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.String) f75dbe84b8da2cb72861181047238c2611b9a9f781f0368d26c0f239eeea46b2 -->
<!-- api: M:Inno.UI.UI.GetFocusedElement(Inno.UI.UiContextHandle) 52fd850a1e8aa3003f5145d6053745a4673b3d4c2f91c559e7e27d2935379fa6 -->
<!-- api: M:Inno.UI.UI.ClearFocus(Inno.UI.UiContextHandle) cceb0641ffe52cf23557edbee6d45d4bf66695e1fdcbc3388c1779309f5afe90 -->
<!-- api: M:Inno.UI.UI.CreateContext(Inno.UI.UiContextOptions) e87075d88a12e63009e4ee843f808a3424ae4167590b9423dbb487d357cc90f2 -->
<!-- api: M:Inno.UI.UI.DestroyContext(Inno.UI.UiContextHandle) 548162742f8461a0f9a884a9815b6f6bc362f7d9e62d2a73882bc69fd6a10ec6 -->
<!-- api: M:Inno.UI.UI.SetViewport(Inno.UI.UiContextHandle,System.Int32,System.Int32,System.Single) 3a7e76120b1a9cd4a12c00666ee523c1a9350a005ccf097de49d4c406f8fcc74 -->
<!-- api: M:Inno.UI.UI.LoadDocument(Inno.UI.UiContextHandle,Inno.UI.UiDocumentSource) ceb97e851b172e3143da7244917f6cb015e1bd6b8fa6a2cde78c61a6e0455f3e -->
<!-- api: M:Inno.UI.UI.LoadDocument(Inno.UI.UiContextHandle,Inno.UI.UiDocumentAsset) d42730a4a8b9b7f10dc35c3b526f35d81f70a6447583bd1e6cf84cd005caea72 -->
<!-- api: M:Inno.UI.UI.ShowDocument(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle) a1ce373dcd6b9670aab28f26e37e55eb4a73492da41c61bebc210a220ee97a64 -->
<!-- api: M:Inno.UI.UI.HideDocument(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle) 669dbcdaae231af3cb7c88decdc05664f73360f87a0453c83a59b33b86cefdb7 -->
<!-- api: M:Inno.UI.UI.CloseDocument(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle) 2db0598569dc4549bb692aeac0180662a46bbbe03bac2fc8a64fe766e0cad206 -->
<!-- api: M:Inno.UI.UI.SetText(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.String,System.String) fe5739dc644a3ecbf4f03e8e2ebfd5f44fecb66492cfd2fde495392c1c297930 -->
<!-- api: M:Inno.UI.UI.SetContent(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.String,Inno.UI.UiDocumentFragment) ea1c9389c73cdacb547d99f0751c091935e55745c6ad7a73f24d488534b6c9c1 -->
<!-- api: M:Inno.UI.UI.SetAttribute(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.String,System.String,System.String) 54c13d3989ab10be717eecf31c10701eafc18d2a2539ffb98468c7bb6a87b1c9 -->
<!-- api: M:Inno.UI.UI.SetClass(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.String,System.String,System.Boolean) 1d1bdbb029e2b0b1eef592b2bdd23b493c5a3d80c2f6a66c3fa1a708052b63f5 -->
<!-- api: M:Inno.UI.UI.RegisterTexture(Inno.UI.UiContextHandle,System.String,Inno.UI.UiTextureData) 98a3e4a1413e61d006b3376bd7d494f55d970d0c9eeef202e471ef1432f69727 -->
<!-- api: M:Inno.UI.UI.Update(Inno.UI.UiContextHandle) 20e39b79b33355c58a144cafa68f2b945d22d4afc8ad60d8833489c15cd6a6ef -->
<!-- api: M:Inno.UI.UI.Update(Inno.UI.UiContextHandle,Inno.UI.UiInputSnapshot) 907f5c7fe5e43d018adfa95f843dde5184355f613694d0249533e25094f2985e -->
<!-- api: M:Inno.UI.UI.Render(Inno.UI.UiContextHandle) d46500e9aac8b1e86875c6c154d904cb8878a316929f1bb531d35d90b853b05e -->
<!-- api: M:Inno.UI.UI.DrainEvents(Inno.UI.UiContextHandle) d6e1e1d53526d236f95a3efbaddd4dd5280501ed934ba84c68cc18b4311ddc2d -->
<!-- api: M:Inno.UI.UI.RegisterImage(Inno.UI.UiContextHandle,Inno.UI.UiImageSource) 69eaab38527834a562a8ef4dc508597dab61b31ef57125cece1beb87a31f2229 -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-415278d026b1bc06"></a>[`public static class Inno.UI.UI`](../../src/services/ui/Inno.UI/UI.cs#L39) | Provides script-friendly retained-mode UI operations. | — |
| <a id="api-600757731d576fe6"></a>[`public static Inno.UI.UiLayoutSnapshot Inno.UI.UI.CaptureLayout(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, System.Collections.Generic.IReadOnlyList<string> elementIds)`](../../src/services/ui/Inno.UI/UI.cs#L62) | Refreshes visual layout once and captures a batch of element geometry without input or time advancement.<br>`context`：The live context owned by the calling thread.<br>`document`：The live document belonging to the context.<br>`elementIds`：A non-empty list of non-empty authored IDs in result order; missing and hidden elements remain explicit results.<br>返回：An immutable snapshot in context pixels. It does not retain a document lease or native memory. | `System.ArgumentException`：A requested ID is empty or an owner handle is invalid.<br>`System.ArgumentNullException`：The element list is null. |
| <a id="api-cd88e1d5d646d424"></a>[`public static Inno.UI.UiBackendCapabilities Inno.UI.UI.capabilities { get; }`](../../src/services/ui/Inno.UI/UI.cs#L71) | Gets immutable optional capabilities of this service generation. | — |
| <a id="api-53b46b770d1371c0"></a>[`public static Inno.UI.UiContextTime Inno.UI.UI.GetTime(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI/UI.cs#L82) | Reads the context clock without advancing it.<br>`context`：The live context on its owner thread.<br>返回：The current elapsed seconds and pause state. | — |
| <a id="api-0331d5cc407621e0"></a>[`public static void Inno.UI.UI.SetTimePaused(Inno.UI.UiContextHandle context, bool paused)`](../../src/services/ui/Inno.UI/UI.cs#L93) | Controls automatic time advancement; input remains independently routable.<br>`context`：The live context on its owner thread.<br>`paused`：True to freeze automatic time; explicit steps remain available. | — |
| <a id="api-c98149333a1de51d"></a>[`public static void Inno.UI.UI.AdvanceTime(Inno.UI.UiContextHandle context, double seconds)`](../../src/services/ui/Inno.UI/UI.cs#L110) | Advances only the context clock without processing input or refreshing layout.<br>`context`：The live context on its owner thread.<br>`seconds`：Finite nonnegative time to advance, including while paused. | `System.ArgumentOutOfRangeException`：The step is negative, nonfinite or overflows the clock. |
| <a id="api-598b5f97ee32ab54"></a>[`public static bool Inno.UI.UI.TrySeekTime(Inno.UI.UiContextHandle context, double seconds)`](../../src/services/ui/Inno.UI/UI.cs#L130) | Requests an absolute time seek only when the backend can preserve document semantics.<br>`context`：The live context on its owner thread.<br>`seconds`：The finite nonnegative requested time.<br>返回：True if applied; false if unsupported, with all state unchanged. | `System.ArgumentOutOfRangeException`：The requested time is negative or nonfinite. |
| <a id="api-2075e69a488b326b"></a>[`public static void Inno.UI.UI.RefreshLayout(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI/UI.cs#L141) | Refreshes document style and layout at the current context time without processing input.<br>`context`：The live context on its owner thread. | — |
| <a id="api-f707af4ad24718de"></a>[`public static void Inno.UI.UI.ProcessInput(Inno.UI.UiContextHandle context, Inno.UI.UiInputSnapshot input)`](../../src/services/ui/Inno.UI/UI.cs#L152) | Processes the explicitly routed input without advancing time or updating the whole context.<br>`context`：The live context on its owner thread.<br>`input`：The current immutable input sample. | — |
| <a id="api-e726cdb67f440f86"></a>[`public static Inno.UI.UiControlValue Inno.UI.UI.GetValue(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId)`](../../src/services/ui/Inno.UI/UI.cs#L175) | Reads a control value on the context owner thread without changing layout or input.<br>`context`：The live context owning the document.<br>`document`：The live document containing the control.<br>`elementId`：The nonempty authored element ID.<br>返回：The current value, or None for a missing element or unsupported control. | `System.InvalidOperationException`：A handle is stale or the caller violates ownership. |
| <a id="api-012431a1c8db2b95"></a>[`public static bool Inno.UI.UI.SetValue(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, Inno.UI.UiControlValue value)`](../../src/services/ui/Inno.UI/UI.cs#L205) | Writes a matching control value without generating value-change or submit events.<br>`context`：The live context owning the document.<br>`document`：The live document containing the control.<br>`elementId`：The nonempty authored element ID.<br>`value`：The typed value; normal control clamping still applies.<br>返回：True when the control accepts the value; false when absent, unsupported or no matching option exists. | `System.ArgumentException`：The value kind does not match the control.<br>`System.InvalidOperationException`：A handle is stale or the caller violates ownership. |
| <a id="api-a49736497fd3632b"></a>[`public static Inno.UI.UiFocusResult Inno.UI.UI.FocusElement(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId)`](../../src/services/ui/Inno.UI/UI.cs#L233) | Requests focus through the context's existing focus and modal policy on its owner thread.<br>`context`：The context owning the document.<br>`document`：The live document containing the target.<br>`elementId`：The nonempty authored element ID.<br>返回：The applied, unchanged, missing or ineligible request outcome. | `System.ArgumentException`：The element ID is empty.<br>`System.InvalidOperationException`：A handle is stale, belongs to another context or the caller violates ownership. |
| <a id="api-a0b7b65dd4753ece"></a>[`public static Inno.UI.UiFocusTarget? Inno.UI.UI.GetFocusedElement(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI/UI.cs#L251) | Reads the current focused control without changing layout or focus.<br>`context`：The live context to query on its owner thread.<br>返回：A neutral focus snapshot, or null when no document control has focus. | `System.InvalidOperationException`：The context is stale or the caller violates ownership. |
| <a id="api-6e884f317bd97bbc"></a>[`public static bool Inno.UI.UI.ClearFocus(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI/UI.cs#L266) | Returns focus to the owning document, preserving modal ownership and emitting normal blur events.<br>`context`：The live context to modify on its owner thread.<br>返回：True when no document control retains focus; false when focus policy refuses the change. | `System.InvalidOperationException`：The context is stale or the caller violates ownership. |
| <a id="api-534c98b625b6ed3d"></a>[`public static Inno.UI.UiContextHandle Inno.UI.UI.CreateContext(Inno.UI.UiContextOptions options)`](../../src/services/ui/Inno.UI/UI.cs#L277) | Creates one independent UI context.<br>`options`：The validated configuration that controls this operation.<br>返回：The validated ui context handle that represents the completed operation. | — |
| <a id="api-76f18a47e12111a9"></a>[`public static void Inno.UI.UI.DestroyContext(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI/UI.cs#L284) | Destroys one UI context and all owned documents.<br>`context`：The context that supplies state and services for this operation. | — |
| <a id="api-918e0e123b9e0352"></a>[`public static void Inno.UI.UI.SetViewport(Inno.UI.UiContextHandle context, int width, int height, float density = 1)`](../../src/services/ui/Inno.UI/UI.cs#L300) | Changes one context's pixel dimensions and density.<br>`context`：The context that supplies state and services for this operation.<br>`width`：The width in logical units or pixels required by this operation.<br>`height`：The height in logical units or pixels required by this operation.<br>`density`：The density consumed by set viewport; ownership remains with the caller unless explicitly stated otherwise. | — |
| <a id="api-40f3ae3ddd7a1a67"></a>[`public static Inno.UI.UiDocumentHandle Inno.UI.UI.LoadDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentSource source)`](../../src/services/ui/Inno.UI/UI.cs#L319) | Loads one explicitly tagged in-memory document.<br>`context`：The context that supplies state and services for this operation.<br>`source`：The source value or location read by this operation.<br>返回：The validated ui document handle that represents the completed operation. | — |
| <a id="api-dc12c7275da39ba8"></a>[`public static Inno.UI.UiDocumentHandle Inno.UI.UI.LoadDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentAsset document)`](../../src/services/ui/Inno.UI/UI.cs#L336) | Loads one imported document compatible with the selected backend.<br>`context`：The context that supplies state and services for this operation.<br>`document`：The document consumed by load document; ownership remains with the caller unless explicitly stated otherwise.<br>返回：The validated ui document handle that represents the completed operation. | — |
| <a id="api-041cb234d473b8f0"></a>[`public static void Inno.UI.UI.ShowDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document)`](../../src/services/ui/Inno.UI/UI.cs#L350) | Shows one document.<br>`context`：The context that supplies state and services for this operation.<br>`document`：The document consumed by show document; ownership remains with the caller unless explicitly stated otherwise. | — |
| <a id="api-eccf65f1fd30c82d"></a>[`public static void Inno.UI.UI.HideDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document)`](../../src/services/ui/Inno.UI/UI.cs#L364) | Hides one document.<br>`context`：The context that supplies state and services for this operation.<br>`document`：The document consumed by hide document; ownership remains with the caller unless explicitly stated otherwise. | — |
| <a id="api-c417c2ed80e005c4"></a>[`public static void Inno.UI.UI.CloseDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document)`](../../src/services/ui/Inno.UI/UI.cs#L378) | Closes one document.<br>`context`：The context that supplies state and services for this operation.<br>`document`：The document consumed by close document; ownership remains with the caller unless explicitly stated otherwise. | — |
| <a id="api-716e3868fb0ae5bd"></a>[`public static bool Inno.UI.UI.SetText(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, string text)`](../../src/services/ui/Inno.UI/UI.cs#L401) | Replaces an element's children with plain Unicode text.<br>`context`：The context that supplies state and services for this operation.<br>`document`：The document consumed by set text; ownership remains with the caller unless explicitly stated otherwise.<br>`elementId`：The element id text validated by the set text operation.<br>`text`：The text text validated by the set text operation.<br>返回：`true` when the operation succeeds or its condition is satisfied; otherwise, `false`. | — |
| <a id="api-cd313432ab3f341c"></a>[`public static bool Inno.UI.UI.SetContent(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, Inno.UI.UiDocumentFragment fragment)`](../../src/services/ui/Inno.UI/UI.cs#L426) | Replaces an element's children with an explicitly tagged source-language fragment.<br>`context`：The context that supplies state and services for this operation.<br>`document`：The document consumed by set content; ownership remains with the caller unless explicitly stated otherwise.<br>`elementId`：The element id text validated by the set content operation.<br>`fragment`：The fragment consumed by set content; ownership remains with the caller unless explicitly stated otherwise.<br>返回：`true` when the operation succeeds or its condition is satisfied; otherwise, `false`. | — |
| <a id="api-a35051587eb9d6cf"></a>[`public static bool Inno.UI.UI.SetAttribute(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, string name, string value)`](../../src/services/ui/Inno.UI/UI.cs#L454) | Sets one element attribute.<br>`context`：The context that supplies state and services for this operation.<br>`document`：The document consumed by set attribute; ownership remains with the caller unless explicitly stated otherwise.<br>`elementId`：The element id text validated by the set attribute operation.<br>`name`：The human-readable name used for presentation and diagnostics.<br>`value`：The concrete value read or transformed by this operation.<br>返回：`true` when the operation succeeds or its condition is satisfied; otherwise, `false`. | — |
| <a id="api-5316f4c2542866f2"></a>[`public static bool Inno.UI.UI.SetClass(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, string className, bool active)`](../../src/services/ui/Inno.UI/UI.cs#L483) | Activates or deactivates one element class.<br>`context`：The context that supplies state and services for this operation.<br>`document`：The document consumed by set class; ownership remains with the caller unless explicitly stated otherwise.<br>`elementId`：The element id text validated by the set class operation.<br>`className`：The class name text validated by the set class operation.<br>`active`：Whether active behavior is enabled while set class executes.<br>返回：`true` when the operation succeeds or its condition is satisfied; otherwise, `false`. | — |
| <a id="api-efd4d7199962fc14"></a>[`public static void Inno.UI.UI.RegisterTexture(Inno.UI.UiContextHandle context, string source, Inno.UI.UiTextureData texture)`](../../src/services/ui/Inno.UI/UI.cs#L503) | Registers one named RGBA8 texture source.<br>`context`：The context that supplies state and services for this operation.<br>`source`：The source value or location read by this operation.<br>`texture`：The texture consumed by register texture; ownership remains with the caller unless explicitly stated otherwise. | — |
| <a id="api-185400bde0552e3e"></a>[`public static void Inno.UI.UI.Update(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI/UI.cs#L515) | Processes current-frame input and advances one context.<br>`context`：The context that supplies state and services for this operation. | — |
| <a id="api-eda728afbae27c38"></a>[`public static void Inno.UI.UI.Update(Inno.UI.UiContextHandle context, Inno.UI.UiInputSnapshot input)`](../../src/services/ui/Inno.UI/UI.cs#L525) | Advances one context with explicitly routed viewport-local input.<br>`context`：The context that supplies state and services for this operation.<br>`input`：The input consumed by update; ownership remains with the caller unless explicitly stated otherwise. | — |
| <a id="api-c3f57658d738885c"></a>[`public static Inno.UI.UiRenderFrame Inno.UI.UI.Render(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI/UI.cs#L538) | Builds an immutable frame of incremental resources and ordered draws.<br>`context`：The context that supplies state and services for this operation.<br>返回：The validated ui render frame that represents the completed operation. | — |
| <a id="api-3f5d64dae61e5f60"></a>[`public static System.Collections.Generic.IReadOnlyList<Inno.UI.UiEvent> Inno.UI.UI.DrainEvents(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI/UI.cs#L548) | Drains document events queued by preceding updates.<br>`context`：The context that supplies state and services for this operation.<br>返回：An immutable snapshot of the values selected by the operation. | — |
| <a id="api-4fba8a96d91a3277"></a>[`public static Inno.UI.IUiImageRegistration Inno.UI.UI.RegisterImage(Inno.UI.UiContextHandle context, Inno.UI.UiImageSource image)`](../../src/services/ui/Inno.UI/UI.cs#L561) | Registers an imported image in the current UI execution scope.<br>`context`：Owning UI context.<br>`image`：Frozen imported texture source.<br>返回：A disposable context-local source; Rendering retains GPU ownership. | — |

### `Inno.UI.UiBackendCapabilities`

<!-- api: T:Inno.UI.UiBackendCapabilities c954dd9e015696bf3806095fa6228b6b15bf435719a000727d829ad54d1951ee -->
<!-- api: M:Inno.UI.UiBackendCapabilities.#ctor(System.Collections.Generic.IEnumerable{Inno.UI.UiDocumentLanguageId},System.Collections.Generic.IEnumerable{Inno.UI.UiBackendCapability}) 27dbe00a258ca566238d1a02bb51ab3ac07d4b3322ff799011999451966c78f7 -->
<!-- api: P:Inno.UI.UiBackendCapabilities.documentLanguages 3684b53da0fde915b293cd835873de38220b4f3298e2e59c9d54f9ced35674b9 -->
<!-- api: P:Inno.UI.UiBackendCapabilities.capabilities 70477ae94819f28d1c1b5ae1862c4fabce39f0219c97c3de8b59cf304cc6e0df -->
<!-- api: M:Inno.UI.UiBackendCapabilities.Supports(Inno.UI.UiBackendCapability) 4612a68f5e363a9620616cf7b4cc8079fc8879aa5bbe64b75c75a72359ffb388 -->
<!-- api: M:Inno.UI.UiBackendCapabilities.Supports(Inno.UI.UiDocumentLanguageId) 94547ecabb8876b077309d82d807d78c4c16e1086f173f553dc4afbe57c0200d -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-9b4b0a0a92c590ee"></a>[`public sealed class Inno.UI.UiBackendCapabilities`](../../src/services/ui/Inno.UI/UiBackendCapabilities.cs#L12) | Describes optional behavior implemented by one UI backend generation. | — |
| <a id="api-8f266b93e2834ccb"></a>[`public Inno.UI.UiBackendCapabilities.UiBackendCapabilities(System.Collections.Generic.IEnumerable<Inno.UI.UiDocumentLanguageId> documentLanguages, System.Collections.Generic.IEnumerable<Inno.UI.UiBackendCapability> capabilities)`](../../src/services/ui/Inno.UI/UiBackendCapabilities.cs#L26) | Creates an immutable capability description.<br>`documentLanguages`：Explicit source languages accepted by the backend.<br>`capabilities`：Open stable identities of optional behaviors supported by this backend. | — |
| <a id="api-3a8867905a639cfe"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.UI.UiDocumentLanguageId> Inno.UI.UiBackendCapabilities.documentLanguages { get; }`](../../src/services/ui/Inno.UI/UiBackendCapabilities.cs#L44) | Gets source languages accepted by the backend. | — |
| <a id="api-6e41fe3412e8120e"></a>[`public System.Collections.Generic.IReadOnlySet<Inno.UI.UiBackendCapability> Inno.UI.UiBackendCapabilities.capabilities { get; }`](../../src/services/ui/Inno.UI/UiBackendCapabilities.cs#L49) | Gets the immutable optional feature identities. | — |
| <a id="api-78316308c24171fe"></a>[`public bool Inno.UI.UiBackendCapabilities.Supports(Inno.UI.UiBackendCapability capability)`](../../src/services/ui/Inno.UI/UiBackendCapabilities.cs#L60) | Checks an exact optional capability identity.<br>`capability`：The requested capability.<br>返回：True when this backend explicitly supports it. | — |
| <a id="api-285969b36662cfc5"></a>[`public bool Inno.UI.UiBackendCapabilities.Supports(Inno.UI.UiDocumentLanguageId language)`](../../src/services/ui/Inno.UI/UiBackendCapabilities.cs#L71) | Determines whether an exact source language is supported.<br>`language`：Language to test.<br>返回：True when the language is registered by this backend. | — |

### `Inno.UI.UiCapabilityUnavailableException`

<!-- api: T:Inno.UI.UiCapabilityUnavailableException 05158e859c19db0bd75fcf09f722afd46fcce1be2200c7af7e089a0035d28c97 -->
<!-- api: M:Inno.UI.UiCapabilityUnavailableException.#ctor(Inno.UI.UiBackendCapability) 17441e50fd2cbd624c50e6e0edd6feaeac8083488badfd040c6ce67d87feb40f -->
<!-- api: P:Inno.UI.UiCapabilityUnavailableException.capability 7f03094539b2b8563fc6295e2a0085b764554a5b8443bc3d99f0ac3117543c10 -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-0e259825bed2d713"></a>[`public sealed class Inno.UI.UiCapabilityUnavailableException : System.NotSupportedException`](../../src/services/ui/Inno.UI/UiBackendCapabilities.cs#L77) | Reports that the selected backend cannot perform an explicitly requested optional operation. | — |
| <a id="api-70a4dc8ba3a203df"></a>[`public Inno.UI.UiCapabilityUnavailableException.UiCapabilityUnavailableException(Inno.UI.UiBackendCapability capability)`](../../src/services/ui/Inno.UI/UiBackendCapabilities.cs#L85) | Creates a capability failure without naming a concrete implementation.<br>`capability`：Unavailable neutral capability. | — |
| <a id="api-5862a27a90c87331"></a>[`public Inno.UI.UiBackendCapability Inno.UI.UiCapabilityUnavailableException.capability { get; }`](../../src/services/ui/Inno.UI/UiBackendCapabilities.cs#L92) | Gets the unavailable capability. | — |

### `Inno.UI.UiBackendCapability`

<!-- api: T:Inno.UI.UiBackendCapability 4df218743302d04d09d16646fd377b9843b71ab03b72b47752e45a2a5853029b -->
<!-- api: F:Inno.UI.UiBackendCapability.FontCollectionFaceSelection da2c1db2f649dd75df113200ebb9bc09a14e628daaa8ff664e83e15fff24cc6b -->
<!-- api: F:Inno.UI.UiBackendCapability.TimeSeek b7daeea9340f0931a8d7b4aaf26f29121173e4b593ab8cf98c434e5dc6ac87f7 -->
<!-- api: M:Inno.UI.UiBackendCapability.#ctor(System.String) d6d4ddd4e6a51fa05eb5e377f48b7dde1578fb415ea2afc885e0c2ab2dda7761 -->
<!-- api: P:Inno.UI.UiBackendCapability.value e7e12aaabee4d965e0ef725cf3a9f251084c6eb70defb928d56d941f857c28b1 -->
<!-- api: P:Inno.UI.UiBackendCapability.isValid a35fd69e03c6060ddace41edc7a47b375d9d145a0e4ed9663bbc0e3cac2f2359 -->
<!-- api: M:Inno.UI.UiBackendCapability.ToString 05b2ea648d5420f77a5dd2e51993d4a6f1137ba74eebbc93f3a3e26015c4a13b -->
<!-- api: M:Inno.UI.UiBackendCapability.op_Inequality(Inno.UI.UiBackendCapability,Inno.UI.UiBackendCapability) 9aa8b279ea35eae6d7f78f397d727a6c0c9cd3836f9c9f612116a69fb862550b -->
<!-- api: M:Inno.UI.UiBackendCapability.op_Equality(Inno.UI.UiBackendCapability,Inno.UI.UiBackendCapability) b0467cbde36e4ebc641eb43a285de5b3d13f9fd2fd8b17e338638797903be02e -->
<!-- api: M:Inno.UI.UiBackendCapability.GetHashCode 76256170b258558ecebeec2c705bdd5417480cd5f1d92d6d4aac20ce442892f3 -->
<!-- api: M:Inno.UI.UiBackendCapability.Equals(System.Object) 0d662d1e56d68726c42e9192ca92aa313d3d5b922d8818b553f05297ed8f845d -->
<!-- api: M:Inno.UI.UiBackendCapability.Equals(Inno.UI.UiBackendCapability) 0be368becef02eea110d6267f109fa3ea8b3373b00afa0fe6d0f109f2473c5dc -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-183d6257c3a3ee38"></a>[`public readonly record struct Inno.UI.UiBackendCapability : System.IEquatable<Inno.UI.UiBackendCapability>`](../../src/services/ui/Inno.UI/UiBackendCapability.cs#L8) | Identifies an optional backend behavior using an open, provider-qualified stable value. | — |
| <a id="api-99b56aa279c0a45e"></a>[`public static readonly Inno.UI.UiBackendCapability Inno.UI.UiBackendCapability.FontCollectionFaceSelection`](../../src/services/ui/Inno.UI/UiBackendCapability.cs#L13) | Selecting an explicit nonzero face from a font collection. | — |
| <a id="api-a84d337aa8cc688d"></a>[`public static readonly Inno.UI.UiBackendCapability Inno.UI.UiBackendCapability.TimeSeek`](../../src/services/ui/Inno.UI/UiBackendCapability.cs#L17) | Seeking context time without replacing the document or losing interaction state. | — |
| <a id="api-302fb55ef5438fe5"></a>[`public Inno.UI.UiBackendCapability.UiBackendCapability(string value)`](../../src/services/ui/Inno.UI/UiBackendCapability.cs#L28) | Creates an optional behavior identity.<br>`value`：A nonempty provider-qualified value without whitespace. | `System.ArgumentException`：The value is empty or contains whitespace. |
| <a id="api-a9d486c18eff5c18"></a>[`public string Inno.UI.UiBackendCapability.value { get; }`](../../src/services/ui/Inno.UI/UiBackendCapability.cs#L40) | Gets the stable capability value. | — |
| <a id="api-1debb768897d86d8"></a>[`public bool Inno.UI.UiBackendCapability.isValid { get; }`](../../src/services/ui/Inno.UI/UiBackendCapability.cs#L44) | Gets whether this identity is assigned. | — |
| <a id="api-bae151db1b82600d"></a>[`public override string Inno.UI.UiBackendCapability.ToString()`](../../src/services/ui/Inno.UI/UiBackendCapability.cs#L51) | Returns the assigned identity, or an empty string for the default value.<br>返回：The stable textual identity. | — |
| <a id="api-1a8f1260629abd15"></a>[`public static bool Inno.UI.UiBackendCapability.operator !=(Inno.UI.UiBackendCapability left, Inno.UI.UiBackendCapability right)`](../../src/services/ui/Inno.UI/UiBackendCapability.cs#L8) | 编译器生成的值不等比较。 | — |
| <a id="api-09ae88ca5b47c9ff"></a>[`public static bool Inno.UI.UiBackendCapability.operator ==(Inno.UI.UiBackendCapability left, Inno.UI.UiBackendCapability right)`](../../src/services/ui/Inno.UI/UiBackendCapability.cs#L8) | 编译器生成的记录值相等比较。 | — |
| <a id="api-d01bec5fec3bcbdd"></a>[`public override int Inno.UI.UiBackendCapability.GetHashCode()`](../../src/services/ui/Inno.UI/UiBackendCapability.cs#L8) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-dd72522c1f3cc4d6"></a>[`public override bool Inno.UI.UiBackendCapability.Equals(object obj)`](../../src/services/ui/Inno.UI/UiBackendCapability.cs#L8) | Indicates whether this instance and a specified object are equal.<br>`obj`：The object to compare with the current instance.<br>返回：`true` if `obj` and this instance are the same type and represent the same value; otherwise, `false`. | — |
| <a id="api-6c3b2066df725256"></a>[`public bool Inno.UI.UiBackendCapability.Equals(Inno.UI.UiBackendCapability other)`](../../src/services/ui/Inno.UI/UiBackendCapability.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |

### `Inno.UI.UiContextTime`

<!-- api: T:Inno.UI.UiContextTime f96052a49b2616a87a71dac995b133b184eaa30e55b45348e17456e82ed645b5 -->
<!-- api: M:Inno.UI.UiContextTime.#ctor(System.Double,System.Boolean) 3b3828a798b32305f877ba30b0ea528ccb5ace9265ec5df7760f4fad4a101ad2 -->
<!-- api: P:Inno.UI.UiContextTime.seconds 14fefc6fb4411d7c2d0eba1a6e96fce0f936d3ca10784a17e6153bd2020099d5 -->
<!-- api: P:Inno.UI.UiContextTime.paused cb0c26b3feba3171317600a617c2e8596315e9dfc2dbc9dac8ebcf8673d4423a -->
<!-- api: M:Inno.UI.UiContextTime.ToString 5ad76963b5ce51a19c9c12c95d51c9eb05a9fd3e5960c96ca0a7007e2e76fd20 -->
<!-- api: M:Inno.UI.UiContextTime.op_Inequality(Inno.UI.UiContextTime,Inno.UI.UiContextTime) 5fc48bb1aaaecdd84b1fd14afdea69c55db9d9ec3b1e5c455991c9e76f4a2ba3 -->
<!-- api: M:Inno.UI.UiContextTime.op_Equality(Inno.UI.UiContextTime,Inno.UI.UiContextTime) 26306ffc226a4a2c9f2322aec0818b9b4fab141f1652df985ecf947a0d6817aa -->
<!-- api: M:Inno.UI.UiContextTime.GetHashCode d6c16b3ed2f917252a65bf29fa50988aeff803afd3067be4b6fdbc360e96927e -->
<!-- api: M:Inno.UI.UiContextTime.Equals(System.Object) 276bfc52bd0b1ccc28575706b77de241412a9a72ef9c894f075cc476e8bc0512 -->
<!-- api: M:Inno.UI.UiContextTime.Equals(Inno.UI.UiContextTime) 563ebb68c287064d3aba4ebf0f0e3bf55813546cf895af88d824bc11763c53c5 -->
<!-- api: M:Inno.UI.UiContextTime.Deconstruct(System.Double@,System.Boolean@) c9b039bd071a2b585adeaa73d3a8a752269376760c396f19996226112b86dc11 -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-aebd2785a5b3a83a"></a>[`public readonly record struct Inno.UI.UiContextTime : System.IEquatable<Inno.UI.UiContextTime>`](../../src/services/ui/Inno.UI/UiContextTime.cs#L12) | Captures a context clock without advancing it or retaining backend state.<br>`seconds`：The elapsed nonnegative context time in seconds.<br>`paused`：Whether normal Update leaves this clock unchanged. | — |
| <a id="api-96ad8f7eae952f89"></a>[`public Inno.UI.UiContextTime.UiContextTime(double seconds, bool paused)`](../../src/services/ui/Inno.UI/UiContextTime.cs#L12) | Captures a context clock without advancing it or retaining backend state.<br>`seconds`：The elapsed nonnegative context time in seconds.<br>`paused`：Whether normal Update leaves this clock unchanged. | — |
| <a id="api-726afcaec969714c"></a>[`public double Inno.UI.UiContextTime.seconds { get; init; }`](../../src/services/ui/Inno.UI/UiContextTime.cs#L13) | The elapsed nonnegative context time in seconds. | — |
| <a id="api-069f8bfba6eb9657"></a>[`public bool Inno.UI.UiContextTime.paused { get; init; }`](../../src/services/ui/Inno.UI/UiContextTime.cs#L14) | Whether normal Update leaves this clock unchanged. | — |
| <a id="api-4c2cd616c5734136"></a>[`public override string Inno.UI.UiContextTime.ToString()`](../../src/services/ui/Inno.UI/UiContextTime.cs#L12) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — |
| <a id="api-ec9aaca258dc558f"></a>[`public static bool Inno.UI.UiContextTime.operator !=(Inno.UI.UiContextTime left, Inno.UI.UiContextTime right)`](../../src/services/ui/Inno.UI/UiContextTime.cs#L12) | 编译器生成的值不等比较。 | — |
| <a id="api-953df2b38b709e60"></a>[`public static bool Inno.UI.UiContextTime.operator ==(Inno.UI.UiContextTime left, Inno.UI.UiContextTime right)`](../../src/services/ui/Inno.UI/UiContextTime.cs#L12) | 编译器生成的记录值相等比较。 | — |
| <a id="api-c6862ea753f56e0f"></a>[`public override int Inno.UI.UiContextTime.GetHashCode()`](../../src/services/ui/Inno.UI/UiContextTime.cs#L12) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-8847676aed993ec0"></a>[`public override bool Inno.UI.UiContextTime.Equals(object obj)`](../../src/services/ui/Inno.UI/UiContextTime.cs#L12) | Indicates whether this instance and a specified object are equal.<br>`obj`：The object to compare with the current instance.<br>返回：`true` if `obj` and this instance are the same type and represent the same value; otherwise, `false`. | — |
| <a id="api-29469f43d004a323"></a>[`public bool Inno.UI.UiContextTime.Equals(Inno.UI.UiContextTime other)`](../../src/services/ui/Inno.UI/UiContextTime.cs#L12) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |
| <a id="api-ee2a4f32265f4a70"></a>[`public void Inno.UI.UiContextTime.Deconstruct(out double seconds, out bool paused)`](../../src/services/ui/Inno.UI/UiContextTime.cs#L12) | 按声明顺序读取记录成员。 | — |

### `Inno.UI.UiControlValue`

<!-- api: T:Inno.UI.UiControlValue 7683758eb771d3c59a0875c5f3a6eebc6a728d18e08fc91f996cb8824ea2b611 -->
<!-- api: P:Inno.UI.UiControlValue.kind 455ba17399c4f0eb7b0bfb295e6193e7fca65b7c0cf5ae7d25f8320515226d80 -->
<!-- api: P:Inno.UI.UiControlValue.text b754083b464a51ef151bbbc25a2232eca682e2718356cd5588d1bb7e07283eae -->
<!-- api: P:Inno.UI.UiControlValue.boolean fa75004dfac935f6c3403182aff7ac53d1b33f3d7d0883db2797d7baee1d74b8 -->
<!-- api: P:Inno.UI.UiControlValue.number 865fa7658f5825a34c6a0419823303ae6069332532dabaef8daa94dd6ea45443 -->
<!-- api: M:Inno.UI.UiControlValue.Text(System.String) e34db99b407ef0309fef602067ed964c2ca9196d0ce44af1c10660b863f887e8 -->
<!-- api: M:Inno.UI.UiControlValue.Boolean(System.Boolean) 66f4076213ecde5a6592a592f1a1e83d658d11631823a79a48ea8ed61881fdde -->
<!-- api: M:Inno.UI.UiControlValue.Number(System.Double) 7dee10441f2424e724b818acff442677816a730fd0b52454f5e232af3fa3e49a -->
<!-- api: M:Inno.UI.UiControlValue.Selection(System.String) 90aed4b629016c81396dc6d6f549746dddc2db4757ba4087a2ddae501c0231d0 -->
<!-- api: M:Inno.UI.UiControlValue.ToString 51adf91cedc2084b88a0f0065e321ab6783730bc24028b6df32d9a7c8bd517bf -->
<!-- api: M:Inno.UI.UiControlValue.op_Inequality(Inno.UI.UiControlValue,Inno.UI.UiControlValue) d2967e62872cfb80c61accece55f693204e796fca75054b0ee415bb3b9c2fc08 -->
<!-- api: M:Inno.UI.UiControlValue.op_Equality(Inno.UI.UiControlValue,Inno.UI.UiControlValue) 42e1abba9eaabb388f1057044a7b3ac830b43560b21fa3f61cc04a79627a41d9 -->
<!-- api: M:Inno.UI.UiControlValue.GetHashCode bf57e443903f5a2f317e020b70e4a44c486dae90061a09683f4b947e69de3599 -->
<!-- api: M:Inno.UI.UiControlValue.Equals(System.Object) ad087e34aa101815030601e3f56d5f546a3b0f478aef5153a64090ce21e53d1e -->
<!-- api: M:Inno.UI.UiControlValue.Equals(Inno.UI.UiControlValue) f15c083b9a63f9fd5fee47be6184867af086f0d1b37671fc560a52e77291e1c0 -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-6559b11ddd49c386"></a>[`public readonly record struct Inno.UI.UiControlValue : System.IEquatable<Inno.UI.UiControlValue>`](../../src/services/ui/Inno.UI/UiControlValue.cs#L8) | Stores an immutable control value without backend objects or document ownership. | — |
| <a id="api-e680e07018bc0d08"></a>[`public Inno.UI.UiControlValueKind Inno.UI.UiControlValue.kind { get; }`](../../src/services/ui/Inno.UI/UiControlValue.cs#L25) | Gets the active value kind; the default value represents no control. | — |
| <a id="api-810d81c56bffccd8"></a>[`public string? Inno.UI.UiControlValue.text { get; }`](../../src/services/ui/Inno.UI/UiControlValue.cs#L29) | Gets text or the selected option value; null for all other kinds. | — |
| <a id="api-a3e2f2a26d83fcab"></a>[`public bool Inno.UI.UiControlValue.boolean { get; }`](../../src/services/ui/Inno.UI/UiControlValue.cs#L33) | Gets the checked value when kind is Boolean; otherwise false. | — |
| <a id="api-3380e3ad961c640a"></a>[`public double Inno.UI.UiControlValue.number { get; }`](../../src/services/ui/Inno.UI/UiControlValue.cs#L37) | Gets the finite value when kind is Number; otherwise zero. | — |
| <a id="api-596b8440edf3d619"></a>[`public static Inno.UI.UiControlValue Inno.UI.UiControlValue.Text(string text)`](../../src/services/ui/Inno.UI/UiControlValue.cs#L51) | Captures an immutable text value.<br>`text`：The non-null text, which may be empty.<br>返回：A text control value. | `System.ArgumentNullException`：Text is null. |
| <a id="api-72c87f25318774a4"></a>[`public static Inno.UI.UiControlValue Inno.UI.UiControlValue.Boolean(bool value)`](../../src/services/ui/Inno.UI/UiControlValue.cs#L63) | Captures a checked state.<br>`value`：Whether the control is checked.<br>返回：A boolean control value. | — |
| <a id="api-3fcfed9c70c583cc"></a>[`public static Inno.UI.UiControlValue Inno.UI.UiControlValue.Number(double value)`](../../src/services/ui/Inno.UI/UiControlValue.cs#L77) | Captures a finite numeric value.<br>`value`：The requested finite value.<br>返回：A numeric control value. | `System.ArgumentOutOfRangeException`：The value is not finite. |
| <a id="api-ff4bcc60b77196fb"></a>[`public static Inno.UI.UiControlValue Inno.UI.UiControlValue.Selection(string value)`](../../src/services/ui/Inno.UI/UiControlValue.cs#L96) | Captures the authored value of a selected option.<br>`value`：The non-null option value, which may be empty.<br>返回：A selection control value. | `System.ArgumentNullException`：The option value is null. |
| <a id="api-42092646b002bd35"></a>[`public override string Inno.UI.UiControlValue.ToString()`](../../src/services/ui/Inno.UI/UiControlValue.cs#L8) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — |
| <a id="api-526bb3ed47c983f9"></a>[`public static bool Inno.UI.UiControlValue.operator !=(Inno.UI.UiControlValue left, Inno.UI.UiControlValue right)`](../../src/services/ui/Inno.UI/UiControlValue.cs#L8) | 编译器生成的值不等比较。 | — |
| <a id="api-39406b39b7f04b61"></a>[`public static bool Inno.UI.UiControlValue.operator ==(Inno.UI.UiControlValue left, Inno.UI.UiControlValue right)`](../../src/services/ui/Inno.UI/UiControlValue.cs#L8) | 编译器生成的记录值相等比较。 | — |
| <a id="api-27a47763ae04822b"></a>[`public override int Inno.UI.UiControlValue.GetHashCode()`](../../src/services/ui/Inno.UI/UiControlValue.cs#L8) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-b5aa4865f3ef7091"></a>[`public override bool Inno.UI.UiControlValue.Equals(object obj)`](../../src/services/ui/Inno.UI/UiControlValue.cs#L8) | Indicates whether this instance and a specified object are equal.<br>`obj`：The object to compare with the current instance.<br>返回：`true` if `obj` and this instance are the same type and represent the same value; otherwise, `false`. | — |
| <a id="api-7ba3ad96834d9296"></a>[`public bool Inno.UI.UiControlValue.Equals(Inno.UI.UiControlValue other)`](../../src/services/ui/Inno.UI/UiControlValue.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |

### `Inno.UI.UiControlValueKind`

<!-- api: T:Inno.UI.UiControlValueKind c382cbf10f3581fef7a91fa0280d59c8eadea23b4b90795b28910eec50e61303 -->
<!-- api: F:Inno.UI.UiControlValueKind.None b73328a3223c62bc3d8938247f925c34897418dbd8e79bb03600aba7ed6d64a4 -->
<!-- api: F:Inno.UI.UiControlValueKind.Text 4c0e74374737cf5bf92902b68e78c5c9f903397511419ffc72a24e1873f4bd8f -->
<!-- api: F:Inno.UI.UiControlValueKind.Boolean 4e4cb086baf68a8b33b2e05de2b5779424ce440daaeb13c2acf4747abf5bafae -->
<!-- api: F:Inno.UI.UiControlValueKind.Number 8c43108c4150a68a49375848d108700bc7ba2663dab5cf4f645267f29f3bac57 -->
<!-- api: F:Inno.UI.UiControlValueKind.Selection ccabc2e5f5ca9267d578c69333c2f333877a17ccb1073658fa463b3135627423 -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-9240d1f128943f39"></a>[`public enum Inno.UI.UiControlValueKind : int`](../../src/services/ui/Inno.UI/UiControlValueKind.cs#L6) | Identifies the neutral value carried by a UI control or event. | — |
| <a id="api-1c1b7b06b863ea7a"></a>[`Inno.UI.UiControlValueKind.None = 0`](../../src/services/ui/Inno.UI/UiControlValueKind.cs#L11) | The element has no supported control value. | — |
| <a id="api-3068fb539cbd71c4"></a>[`Inno.UI.UiControlValueKind.Text = 1`](../../src/services/ui/Inno.UI/UiControlValueKind.cs#L15) | A Unicode text value. | — |
| <a id="api-1632bcb9792f8ea4"></a>[`Inno.UI.UiControlValueKind.Boolean = 2`](../../src/services/ui/Inno.UI/UiControlValueKind.cs#L19) | A checked or unchecked value. | — |
| <a id="api-7ae32e9e11e94818"></a>[`Inno.UI.UiControlValueKind.Number = 3`](../../src/services/ui/Inno.UI/UiControlValueKind.cs#L23) | A finite numeric value. | — |
| <a id="api-5b2e20484b75d645"></a>[`Inno.UI.UiControlValueKind.Selection = 4`](../../src/services/ui/Inno.UI/UiControlValueKind.cs#L27) | The authored value of a selected option. | — |

### `Inno.UI.UiDocumentAsset`

<!-- api: T:Inno.UI.UiDocumentAsset ef88f54bc5bf7c02c986811a9a89b4f760a8d1006277c44f4db3f1cd6258bdbb -->
<!-- api: P:Inno.UI.UiDocumentAsset.source 0647be106353fe2567a17c3a378f4586393fac31ebcd86cf2e9c743d9e5ee6fb -->
<!-- api: P:Inno.UI.UiDocumentAsset.fonts 110eeb4167482d61f116b835dbd58d9aa93e97d3f0a887c055aa93c6db1f7db1 -->
<!-- api: P:Inno.UI.UiDocumentAsset.images 695a97590e8683ef93a15846daebcd0da8873c99623dfa06b5c86241e64cd09b -->
<!-- api: P:Inno.UI.UiDocumentAsset.implementationId 1ab85b214ab9a6f636b228c61152cc867b35029115ba89728097bdef4335df58 -->
<!-- api: M:Inno.UI.UiDocumentAsset.CreateRuntimePayload(System.String,Inno.UI.UiDocumentSource,System.Collections.Generic.IReadOnlyList{Inno.UI.UiDocumentFontFace},System.Collections.Generic.IReadOnlyList{Inno.UI.UiDocumentImage}) c74d1d1116b8a10f2eb33a1d61ffb983472824e60de0e33ea613944b909ef318 -->
<!-- api: M:Inno.UI.UiDocumentAsset.OnRuntimePayloadChanged(System.ReadOnlyMemory{System.Byte},System.ReadOnlyMemory{System.Byte}) f99effc614b8bd3c8dfbd78562faf76318037e1244f87ed7e7066d1f38f0131f -->
<!-- api: M:Inno.UI.UiDocumentAsset.#ctor 5df6a0bd779a7365aaabba972c29c0666420ec4165b874f332e2748b4bc0ef3e -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-0a6d478fc3d7dc43"></a>[`public sealed class Inno.UI.UiDocumentAsset : Inno.Assets.AssetObject`](../../src/services/ui/Inno.UI/UiDocumentAsset.cs#L18) | Represents one imported, language-tagged UI document source. | — |
| <a id="api-5b58dd2159033e9d"></a>[`public Inno.UI.UiDocumentSource? Inno.UI.UiDocumentAsset.source { get; }`](../../src/services/ui/Inno.UI/UiDocumentAsset.cs#L29) | Gets the frozen source, or null before runtime content is loaded. | — |
| <a id="api-dcfb6fc1cd17c96b"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.UI.UiDocumentFontFace> Inno.UI.UiDocumentAsset.fonts { get; }`](../../src/services/ui/Inno.UI/UiDocumentAsset.cs#L33) | Gets font dependencies declared by this document. | — |
| <a id="api-3eeca67e69cd6900"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.UI.UiDocumentImage> Inno.UI.UiDocumentAsset.images { get; }`](../../src/services/ui/Inno.UI/UiDocumentAsset.cs#L38) | Gets imported images declared by the canonical document. | — |
| <a id="api-4dae5531559a0815"></a>[`public string Inno.UI.UiDocumentAsset.implementationId { get; }`](../../src/services/ui/Inno.UI/UiDocumentAsset.cs#L43) | Gets the exact backend implementation selected when the source was imported. | — |
| <a id="api-eb72aa59f41661c5"></a>[`public static byte[] Inno.UI.UiDocumentAsset.CreateRuntimePayload(string implementationId, Inno.UI.UiDocumentSource source, System.Collections.Generic.IReadOnlyList<Inno.UI.UiDocumentFontFace>? fonts = null, System.Collections.Generic.IReadOnlyList<Inno.UI.UiDocumentImage>? images = null)`](../../src/services/ui/Inno.UI/UiDocumentAsset.cs#L63) | Encodes an importer-owned runtime payload without exposing an implementation-specific format.<br>`implementationId`：Stable backend implementation identity.<br>`source`：Validated frozen document source.<br>`fonts`：Imported font asset dependencies.<br>`images`：Frozen image dependencies recorded by the asset import pipeline.<br>返回：Complete deterministic runtime payload bytes. | — |
| <a id="api-0b311bd490bd89be"></a>[`protected override void Inno.UI.UiDocumentAsset.OnRuntimePayloadChanged(System.ReadOnlyMemory<byte> previousPayload, System.ReadOnlyMemory<byte> currentPayload)`](../../src/services/ui/Inno.UI/UiDocumentAsset.cs#L119) | Rebuilds runtime-derived state after the serialized asset payload changes.<br>`previousPayload`：The previous payload consumed by on runtime payload changed; ownership remains with the caller unless explicitly stated otherwise.<br>`currentPayload`：The current payload consumed by on runtime payload changed; ownership remains with the caller unless explicitly stated otherwise. | — |
| <a id="api-4c19f6f7c0258afd"></a>[`public Inno.UI.UiDocumentAsset.UiDocumentAsset()`](../../src/services/ui/Inno.UI/UiDocumentAsset.cs#L18) | 编译器生成的记录构造入口，参数对应左列声明的成员。 | — |

### `Inno.UI.UiDocumentFontFace`

<!-- api: T:Inno.UI.UiDocumentFontFace a81126aef8ddf7038664642e4789bebfe681e6e51fc4536b206de55bab43bc0d -->
<!-- api: M:Inno.UI.UiDocumentFontFace.#ctor(System.Guid,System.String,Inno.Text.TextFontStyle,System.Int32) 3b6ceaee2e8bd377bfaeada929bd6c09ae20ec1bba3d91bad5a8a0b16f50648b -->
<!-- api: P:Inno.UI.UiDocumentFontFace.assetId d2434c793993187206b9be777458366f0353de39f2cd501d56e54af655b65026 -->
<!-- api: P:Inno.UI.UiDocumentFontFace.family ff05343f949d9f0b469d1340a19f42d0cc52b00206370d40914e286b6159e71c -->
<!-- api: P:Inno.UI.UiDocumentFontFace.style 1688ae646f8a79a568542add1e4ca499f2fa9d4ff75d4eec5bba15438077fc5e -->
<!-- api: P:Inno.UI.UiDocumentFontFace.weight 2d393ad09b1b7355381fba02aa9385aaae991ca7fd9941d8a25f014e0e82cead -->
<!-- api: M:Inno.UI.UiDocumentFontFace.ToString 5d41804eeddd22ef0c454ddf59fee32e6e54a3a2cc0320628d723cfe764a6258 -->
<!-- api: M:Inno.UI.UiDocumentFontFace.op_Inequality(Inno.UI.UiDocumentFontFace,Inno.UI.UiDocumentFontFace) 9b57296732e958d1ad103b1015c78af07b734e98a56a971601179c93c10110be -->
<!-- api: M:Inno.UI.UiDocumentFontFace.op_Equality(Inno.UI.UiDocumentFontFace,Inno.UI.UiDocumentFontFace) bd43f39f99d9500fbcbc7bb6fcb03bbb9d28782cf4a008279ed1cb8edfe70ac8 -->
<!-- api: M:Inno.UI.UiDocumentFontFace.GetHashCode f01b3691fa081af7131a1fc12706ef2afd0cf103df538510a0c13a6fd793f8bd -->
<!-- api: M:Inno.UI.UiDocumentFontFace.Equals(System.Object) 330f78955f3685ebb9847bf7d0f8d2223e35563267149c4de1faa28ad83edc6b -->
<!-- api: M:Inno.UI.UiDocumentFontFace.Equals(Inno.UI.UiDocumentFontFace) f2fec808bd5c802c469f3e255ca7d94bacdb63ce8e67db977db8aa8aef52d6d6 -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-d45614ff9380b66b"></a>[`public readonly record struct Inno.UI.UiDocumentFontFace : System.IEquatable<Inno.UI.UiDocumentFontFace>`](../../src/services/ui/Inno.UI/UiDocumentFontFace.cs#L9) | Identifies one imported font face owned by a single UI document. | — |
| <a id="api-32efc424bb84f778"></a>[`public Inno.UI.UiDocumentFontFace.UiDocumentFontFace(System.Guid assetId, string family, Inno.Text.TextFontStyle style, int weight)`](../../src/services/ui/Inno.UI/UiDocumentFontFace.cs#L26) | Creates a font dependency with its document-private family.<br>`assetId`：Persistent font asset identity.<br>`family`：Private family referenced by canonical document text.<br>`style`：Declared face style.<br>`weight`：Declared face weight. | — |
| <a id="api-7998a102a04d78c7"></a>[`public System.Guid Inno.UI.UiDocumentFontFace.assetId { get; }`](../../src/services/ui/Inno.UI/UiDocumentFontFace.cs#L46) | Gets the imported font asset identity. | — |
| <a id="api-f9d093d814b02738"></a>[`public string Inno.UI.UiDocumentFontFace.family { get; }`](../../src/services/ui/Inno.UI/UiDocumentFontFace.cs#L50) | Gets the family referenced by the document. | — |
| <a id="api-beb361130bf7d4f4"></a>[`public Inno.Text.TextFontStyle Inno.UI.UiDocumentFontFace.style { get; }`](../../src/services/ui/Inno.UI/UiDocumentFontFace.cs#L54) | Gets the declared style. | — |
| <a id="api-1c807062641ec23a"></a>[`public int Inno.UI.UiDocumentFontFace.weight { get; }`](../../src/services/ui/Inno.UI/UiDocumentFontFace.cs#L58) | Gets the declared weight. | — |
| <a id="api-6fd3592f75a955ae"></a>[`public override string Inno.UI.UiDocumentFontFace.ToString()`](../../src/services/ui/Inno.UI/UiDocumentFontFace.cs#L9) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — |
| <a id="api-bbb624089d83a4df"></a>[`public static bool Inno.UI.UiDocumentFontFace.operator !=(Inno.UI.UiDocumentFontFace left, Inno.UI.UiDocumentFontFace right)`](../../src/services/ui/Inno.UI/UiDocumentFontFace.cs#L9) | 编译器生成的值不等比较。 | — |
| <a id="api-c610cbb88d5a004b"></a>[`public static bool Inno.UI.UiDocumentFontFace.operator ==(Inno.UI.UiDocumentFontFace left, Inno.UI.UiDocumentFontFace right)`](../../src/services/ui/Inno.UI/UiDocumentFontFace.cs#L9) | 编译器生成的记录值相等比较。 | — |
| <a id="api-88c7357c77853861"></a>[`public override int Inno.UI.UiDocumentFontFace.GetHashCode()`](../../src/services/ui/Inno.UI/UiDocumentFontFace.cs#L9) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-a053f5d6b16c4562"></a>[`public override bool Inno.UI.UiDocumentFontFace.Equals(object obj)`](../../src/services/ui/Inno.UI/UiDocumentFontFace.cs#L9) | Indicates whether this instance and a specified object are equal.<br>`obj`：The object to compare with the current instance.<br>返回：`true` if `obj` and this instance are the same type and represent the same value; otherwise, `false`. | — |
| <a id="api-9568975fd0273710"></a>[`public bool Inno.UI.UiDocumentFontFace.Equals(Inno.UI.UiDocumentFontFace other)`](../../src/services/ui/Inno.UI/UiDocumentFontFace.cs#L9) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |

### `Inno.UI.UiDocumentImage`

<!-- api: T:Inno.UI.UiDocumentImage 1244dfa5038ad6fdc76cf12919922d64be4621480b3adda278691f6434db5cb4 -->
<!-- api: M:Inno.UI.UiDocumentImage.#ctor(System.String,Inno.UI.UiImageSource) 3e17e20824579b255736d6b283d6814cf6f5f573a4130e546771bdd6ef2becd2 -->
<!-- api: P:Inno.UI.UiDocumentImage.source 4c27246ff5cd7a023469efbaf39152ce888875791712773134ba62cc18cb49de -->
<!-- api: P:Inno.UI.UiDocumentImage.image ae0217958c9e0bd98fd51dcf74ec2264c9359c4d0d31a9276a63ea9c61e4f504 -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-8492be050d85e2fc"></a>[`public sealed class Inno.UI.UiDocumentImage`](../../src/services/ui/Inno.UI/UiDocumentImage.cs#L8) | Associates a frontend-owned source name with one frozen imported image dependency. | — |
| <a id="api-580dfc5776200c6b"></a>[`public Inno.UI.UiDocumentImage.UiDocumentImage(string source, Inno.UI.UiImageSource image)`](../../src/services/ui/Inno.UI/UiDocumentImage.cs#L25) | Declares an image used by canonical document source.<br>`source`：Opaque resource name emitted by the language frontend.<br>`image`：Imported artifact identity and dimensions. | `System.ArgumentException`：The source is empty.<br>`System.ArgumentNullException`：The image is null. |
| <a id="api-b0d4c9592daa72c5"></a>[`public string Inno.UI.UiDocumentImage.source { get; }`](../../src/services/ui/Inno.UI/UiDocumentImage.cs#L37) | Gets the frontend-owned source name. | — |
| <a id="api-be354af935456a53"></a>[`public Inno.UI.UiImageSource Inno.UI.UiDocumentImage.image { get; }`](../../src/services/ui/Inno.UI/UiDocumentImage.cs#L42) | Gets the frozen imported resource. | — |

### `Inno.UI.UiElementLayout`

<!-- api: T:Inno.UI.UiElementLayout 170ad789d2d0f1b13a4aa48129ee027ba2855dbeb766c3393415bfdff99e7927 -->
<!-- api: M:Inno.UI.UiElementLayout.#ctor(System.String,Inno.UI.UiElementLayoutStatus,Inno.Core.Mathematics.Rect,Inno.Core.Mathematics.Rect,Inno.Core.Mathematics.Rect,Inno.Core.Mathematics.Matrix) f03a84a580259f7f488c23c2c48f1090ba5b0e453c32f11f8963cd2e4c291d48 -->
<!-- api: P:Inno.UI.UiElementLayout.elementId eaa358a7b68aa178550ca8ac2e4cf4e92526b161c62c0ee66db57b75e2fe5bae -->
<!-- api: P:Inno.UI.UiElementLayout.status f73d273fe6a5d10715870fff2f4d6b84563c3d86fb34a3ebdc65609b32763524 -->
<!-- api: P:Inno.UI.UiElementLayout.contentBounds 94cc1cac99b1c2873d509004123f219ec7ff913f8c6e655d2fb9030f18321609 -->
<!-- api: P:Inno.UI.UiElementLayout.borderBounds c82ae8e33e8571834061d9885d82f1414cd3153f4f35cf530bfa73807d446539 -->
<!-- api: P:Inno.UI.UiElementLayout.clipBounds ede960b3c91f6cd858317f3937eff7ea935c3da37b3e0b8fc6a9ee2c1db21006 -->
<!-- api: P:Inno.UI.UiElementLayout.localToContext b39fe099afbacf335f4c185a1ff0eeeb7e1df370ecfaa10b0495c57f105a6ee3 -->
<!-- api: M:Inno.UI.UiElementLayout.TryLocalToContext(Inno.Core.Mathematics.Vector2,Inno.Core.Mathematics.Vector2@) 6754160190e2fecb5ba6927ea3fe3ed48b5f9c900e7be4adee8166dcc03d2da5 -->
<!-- api: M:Inno.UI.UiElementLayout.TryContextToLocal(Inno.Core.Mathematics.Vector2,Inno.Core.Mathematics.Vector2@) 69b85257b36a02ae167fc71f473935a8290e74a35e698fe2a2da1592208d33bd -->
<!-- api: M:Inno.UI.UiElementLayout.ToString d63977c06d0ddb885c77f7fc1486a303ba4935b3ea8179aa897a83253c85342e -->
<!-- api: M:Inno.UI.UiElementLayout.op_Inequality(Inno.UI.UiElementLayout,Inno.UI.UiElementLayout) 089ff4850e0c41ef1faf219ef3ced54e8ff6cf49c00b502b73d22a96302667fe -->
<!-- api: M:Inno.UI.UiElementLayout.op_Equality(Inno.UI.UiElementLayout,Inno.UI.UiElementLayout) ecd14e1fbe18789fe64ad376c9c1f6a3028c9f27b930bedde73691c793242bcb -->
<!-- api: M:Inno.UI.UiElementLayout.GetHashCode 72561d61a63dce404343b0b7a56f289353720033e489093ef1f58e0b0b749d7c -->
<!-- api: M:Inno.UI.UiElementLayout.Equals(System.Object) c231f720401768156513e7749d82e25b047004a11e94f111938d0162dbc1f13b -->
<!-- api: M:Inno.UI.UiElementLayout.Equals(Inno.UI.UiElementLayout) 6f368a9f38c2d871e76d17245955c23a0f2eed1f7c591484b9553a1657ff2856 -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-1be5f7014d29bfc4"></a>[`public readonly record struct Inno.UI.UiElementLayout : System.IEquatable<Inno.UI.UiElementLayout>`](../../src/services/ui/Inno.UI/UiElementLayout.cs#L15) | Captures one element's visual geometry without retaining a backend element or borrowing native memory. | — |
| <a id="api-ff45dae227feefff"></a>[`public Inno.UI.UiElementLayout.UiElementLayout(string elementId, Inno.UI.UiElementLayoutStatus status, Inno.Core.Mathematics.Rect contentBounds, Inno.Core.Mathematics.Rect borderBounds, Inno.Core.Mathematics.Rect clipBounds, Inno.Core.Mathematics.Matrix localToContext)`](../../src/services/ui/Inno.UI/UiElementLayout.cs#L42) | Creates a neutral element measurement belonging to a containing layout snapshot.<br>`elementId`：The authored element ID used by the request.<br>`status`：Whether the element is missing, hidden, or measurable.<br>`contentBounds`：The transformed content box in context pixels; meaningful only when available.<br>`borderBounds`：The transformed border box in context pixels; meaningful only when available.<br>`clipBounds`：The effective rectangular clip intersected with the context viewport.<br>`localToContext`：The column-vector transform from border-local pixels to homogeneous context pixels. Divide transformed X and Y by W; singular transforms do not grant an inverse mapping. | `System.ArgumentException`：The element ID is empty or the status is undefined. |
| <a id="api-0a9821fb6ce40938"></a>[`public string Inno.UI.UiElementLayout.elementId { get; }`](../../src/services/ui/Inno.UI/UiElementLayout.cs#L64) | Gets the requested authored element identity within its document. | — |
| <a id="api-62e86e19e22d31b9"></a>[`public Inno.UI.UiElementLayoutStatus Inno.UI.UiElementLayout.status { get; }`](../../src/services/ui/Inno.UI/UiElementLayout.cs#L68) | Gets whether the requested element has measurable geometry. | — |
| <a id="api-bdef138bab4986c6"></a>[`public Inno.Core.Mathematics.Rect Inno.UI.UiElementLayout.contentBounds { get; }`](../../src/services/ui/Inno.UI/UiElementLayout.cs#L72) | Gets the transformed content box in context pixels. | — |
| <a id="api-45d3d465dc0f595a"></a>[`public Inno.Core.Mathematics.Rect Inno.UI.UiElementLayout.borderBounds { get; }`](../../src/services/ui/Inno.UI/UiElementLayout.cs#L76) | Gets the transformed border box in context pixels. | — |
| <a id="api-1a98a2430ca9a13c"></a>[`public Inno.Core.Mathematics.Rect Inno.UI.UiElementLayout.clipBounds { get; }`](../../src/services/ui/Inno.UI/UiElementLayout.cs#L80) | Gets the enclosing rectangular clip in context pixels. | — |
| <a id="api-9892d25fc7d30e64"></a>[`public Inno.Core.Mathematics.Matrix Inno.UI.UiElementLayout.localToContext { get; }`](../../src/services/ui/Inno.UI/UiElementLayout.cs#L84) | Gets the transform from border-local pixels to homogeneous context pixels. | — |
| <a id="api-540069f413b3b117"></a>[`public bool Inno.UI.UiElementLayout.TryLocalToContext(Inno.Core.Mathematics.Vector2 point, out Inno.Core.Mathematics.Vector2 result)`](../../src/services/ui/Inno.UI/UiElementLayout.cs#L97) | Projects border-local pixels into context pixels using this captured geometry.<br>`point`：A finite point relative to the element's untransformed border origin.<br>`result`：Receives context pixels, or zero when the element or projection is unavailable.<br>返回：Whether the captured element and homogeneous projection are usable. | — |
| <a id="api-2eb5fe2f0a8bf4a7"></a>[`public bool Inno.UI.UiElementLayout.TryContextToLocal(Inno.Core.Mathematics.Vector2 point, out Inno.Core.Mathematics.Vector2 result)`](../../src/services/ui/Inno.UI/UiElementLayout.cs#L114) | Maps context pixels back onto the element's local plane, including perspective transforms.<br>`point`：A finite context-pixel point; density is already accounted for.<br>`result`：Receives border-local pixels, or zero for hidden, missing, or singular geometry.<br>返回：Whether the captured planar transform is invertible and the point can be projected. | — |
| <a id="api-80b7bc172b5e8676"></a>[`public override string Inno.UI.UiElementLayout.ToString()`](../../src/services/ui/Inno.UI/UiElementLayout.cs#L15) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — |
| <a id="api-16ee86762f9b2a53"></a>[`public static bool Inno.UI.UiElementLayout.operator !=(Inno.UI.UiElementLayout left, Inno.UI.UiElementLayout right)`](../../src/services/ui/Inno.UI/UiElementLayout.cs#L15) | 编译器生成的值不等比较。 | — |
| <a id="api-eebff4a2282e2c44"></a>[`public static bool Inno.UI.UiElementLayout.operator ==(Inno.UI.UiElementLayout left, Inno.UI.UiElementLayout right)`](../../src/services/ui/Inno.UI/UiElementLayout.cs#L15) | 编译器生成的记录值相等比较。 | — |
| <a id="api-311b3adc6472b86d"></a>[`public override int Inno.UI.UiElementLayout.GetHashCode()`](../../src/services/ui/Inno.UI/UiElementLayout.cs#L15) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-c1ed0d9cd5af996e"></a>[`public override bool Inno.UI.UiElementLayout.Equals(object obj)`](../../src/services/ui/Inno.UI/UiElementLayout.cs#L15) | Indicates whether this instance and a specified object are equal.<br>`obj`：The object to compare with the current instance.<br>返回：`true` if `obj` and this instance are the same type and represent the same value; otherwise, `false`. | — |
| <a id="api-beb426ed0377cb55"></a>[`public bool Inno.UI.UiElementLayout.Equals(Inno.UI.UiElementLayout other)`](../../src/services/ui/Inno.UI/UiElementLayout.cs#L15) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |

### `Inno.UI.UiElementLayoutStatus`

<!-- api: T:Inno.UI.UiElementLayoutStatus 1ed97d7c6a1a803acd376de98029eb69db7ebe0e04d79c5fc43940ae050ca51a -->
<!-- api: F:Inno.UI.UiElementLayoutStatus.Missing 9162dd40be9b5a78a051a7d7b5c21afc41db1a4a526a4739483c4929de4b9f6b -->
<!-- api: F:Inno.UI.UiElementLayoutStatus.Hidden 3b121779beaf64615fb85d3094f4dfd7fc65752baeaa6267ee7659ae84b0c593 -->
<!-- api: F:Inno.UI.UiElementLayoutStatus.Available e0c6507bdd7870e4d0f6a23ac817934fadcbe2984182f7bc3f94af1e01fc68c0 -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-ad82fb71c757c1bc"></a>[`public enum Inno.UI.UiElementLayoutStatus : int`](../../src/services/ui/Inno.UI/UiElementLayoutStatus.cs#L6) | Distinguishes a missing declaration from a hidden element and measurable visual content. | — |
| <a id="api-a5890bf3950036d0"></a>[`Inno.UI.UiElementLayoutStatus.Missing = 0`](../../src/services/ui/Inno.UI/UiElementLayoutStatus.cs#L11) | The document does not contain the requested authored element ID. | — |
| <a id="api-cbfa82d24dafb709"></a>[`Inno.UI.UiElementLayoutStatus.Hidden = 1`](../../src/services/ui/Inno.UI/UiElementLayoutStatus.cs#L15) | The element exists but does not currently produce visible layout geometry. | — |
| <a id="api-6fd263b3378a11b1"></a>[`Inno.UI.UiElementLayoutStatus.Available = 2`](../../src/services/ui/Inno.UI/UiElementLayoutStatus.cs#L19) | The snapshot contains the element's current visual geometry. | — |

### `Inno.UI.UiEventOrigin`

<!-- api: T:Inno.UI.UiEventOrigin 2e4faed7cd5642ff4643fc5222ec8ac2287cf9e7a104dac19b68c6b39af2abbb -->
<!-- api: F:Inno.UI.UiEventOrigin.Backend 58e9a9132fbfad2e62c3ef9c3b5d75946edbcce4d51f972f60250c48b4a63358 -->
<!-- api: F:Inno.UI.UiEventOrigin.UserInput 3c78709ca439d4c7d84b882921fcc09b06fa768017e80fd985caaf1e18b2ce86 -->
<!-- api: F:Inno.UI.UiEventOrigin.Programmatic 45a994c34ed88faccc1c18b95358f82dafd26be28da78d6b0e37943c7b22d3c5 -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-6f0a356bfef5d31e"></a>[`public enum Inno.UI.UiEventOrigin : int`](../../src/services/ui/Inno.UI/UiEventOrigin.cs#L6) | Identifies the operation that caused a document event. | — |
| <a id="api-5112919603b1589f"></a>[`Inno.UI.UiEventOrigin.Backend = 0`](../../src/services/ui/Inno.UI/UiEventOrigin.cs#L11) | The backend updated document state independently of input. | — |
| <a id="api-45ae6e0a40b050c4"></a>[`Inno.UI.UiEventOrigin.UserInput = 1`](../../src/services/ui/Inno.UI/UiEventOrigin.cs#L15) | An explicitly routed user input sample caused the event. | — |
| <a id="api-cd798ba1f2a6db4f"></a>[`Inno.UI.UiEventOrigin.Programmatic = 2`](../../src/services/ui/Inno.UI/UiEventOrigin.cs#L19) | An application API call caused the event. | — |

### `Inno.UI.UiEventPhase`

<!-- api: T:Inno.UI.UiEventPhase ba3254723939891e60f52495158524f9af5711c30432184dcbcecae635252935 -->
<!-- api: F:Inno.UI.UiEventPhase.None 45773fa0e6973c996748d68756869677edab25f42ee48051ba33b8e7ca1ffdec -->
<!-- api: F:Inno.UI.UiEventPhase.Editing d299f3ef8b5d7fc0cfdcdd4371974c695b4b0039a844f619d97192b3e2ae2bac -->
<!-- api: F:Inno.UI.UiEventPhase.Committed b8843e4b88ef01f4cfb71d8edf838bc9e87a0a255d0040c28d2251857da4ae14 -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-8f5377a87f85763b"></a>[`public enum Inno.UI.UiEventPhase : int`](../../src/services/ui/Inno.UI/UiEventPhase.cs#L6) | Distinguishes ongoing value editing from a completed control interaction. | — |
| <a id="api-7c669bd38b784770"></a>[`Inno.UI.UiEventPhase.None = 0`](../../src/services/ui/Inno.UI/UiEventPhase.cs#L11) | The event does not describe a value edit. | — |
| <a id="api-65693b03e1c6825c"></a>[`Inno.UI.UiEventPhase.Editing = 1`](../../src/services/ui/Inno.UI/UiEventPhase.cs#L15) | The user is editing a value and may continue changing it. | — |
| <a id="api-adc3a7e6e85e0786"></a>[`Inno.UI.UiEventPhase.Committed = 2`](../../src/services/ui/Inno.UI/UiEventPhase.cs#L19) | The control interaction submitted or finished its current value. | — |

### `Inno.UI.UiFocusResult`

<!-- api: T:Inno.UI.UiFocusResult e7d4ebaace9e753d5f11c974d6f4d3cb98a3e3a44df62e5922f6650aedadbd6e -->
<!-- api: F:Inno.UI.UiFocusResult.Focused f0c930a085c908d59398d99304ea420d9d2cb7df3a2fde757cdffe5d951979a4 -->
<!-- api: F:Inno.UI.UiFocusResult.Unchanged 3eadc5cbd2ba5e26c0778aaedb4348192c4d10619e2d5187848dcd1174f2ec86 -->
<!-- api: F:Inno.UI.UiFocusResult.NotFound 51ae5a97a162185dbd07f8da8eef69a1c602525791101aa43763840bb998c1d2 -->
<!-- api: F:Inno.UI.UiFocusResult.NotFocusable 261dbd0d14574580c00708a3065197efdfe6f405218d2923b62897366035110d -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-dd6314eec44b3eba"></a>[`public enum Inno.UI.UiFocusResult : int`](../../src/services/ui/Inno.UI/UiFocusResult.cs#L7) | Describes an explicit focus request without treating an ineligible element as a backend failure. | — |
| <a id="api-d5adbb41f72d1d29"></a>[`Inno.UI.UiFocusResult.Focused = 0`](../../src/services/ui/Inno.UI/UiFocusResult.cs#L12) | The requested element acquired focus. | — |
| <a id="api-29842964aa3a57ab"></a>[`Inno.UI.UiFocusResult.Unchanged = 1`](../../src/services/ui/Inno.UI/UiFocusResult.cs#L16) | The requested focus state was already active. | — |
| <a id="api-8fae3333e33a9215"></a>[`Inno.UI.UiFocusResult.NotFound = 2`](../../src/services/ui/Inno.UI/UiFocusResult.cs#L20) | The live document contains no element with the requested ID. | — |
| <a id="api-7feb03a0388aca0d"></a>[`Inno.UI.UiFocusResult.NotFocusable = 3`](../../src/services/ui/Inno.UI/UiFocusResult.cs#L24) | Visibility, enabled state, focus policy or a modal prevents the request. | — |

### `Inno.UI.UiFocusTarget`

<!-- api: T:Inno.UI.UiFocusTarget de4d7300931e1da845312a3a8ff6e7c77cab3a82491ec1420e6567508cf60108 -->
<!-- api: M:Inno.UI.UiFocusTarget.#ctor(Inno.UI.UiDocumentHandle,System.String) 3d13b3ab00a163b0f2f4fbfd94eb327227ed25b4a4dccd19164080c849ff6093 -->
<!-- api: P:Inno.UI.UiFocusTarget.document c0b6b5cd239f2e4d69cf3c34d4b5c7ffe20866da9e43044d171ce50fce900a46 -->
<!-- api: P:Inno.UI.UiFocusTarget.elementId bfcc39cd9d012bd938f624c12528bcd24c617e30eb0fedcc7b6b05f018e79923 -->
<!-- api: M:Inno.UI.UiFocusTarget.ToString 6fe507bea75acb34dba2805799364ef2cdaa6444745434661ef30241c93c65c5 -->
<!-- api: M:Inno.UI.UiFocusTarget.op_Inequality(Inno.UI.UiFocusTarget,Inno.UI.UiFocusTarget) d44cf5732e3d14f63e1d0acba50b38384e3ff1ed2c273c925302a8e61feff8f3 -->
<!-- api: M:Inno.UI.UiFocusTarget.op_Equality(Inno.UI.UiFocusTarget,Inno.UI.UiFocusTarget) 400f10b39ffdf9ce785a0e0734ddb18f1059d78463e0b371929b18583e3c77bd -->
<!-- api: M:Inno.UI.UiFocusTarget.GetHashCode eb776e6c0034a09ffb4a832b74680ddbfa830c664fc697f2458c8d1c4e10d504 -->
<!-- api: M:Inno.UI.UiFocusTarget.Equals(System.Object) 7b4f998e46c00cfeec39da540e5fcca379b6eccc290e13946aa3810f9aa07a22 -->
<!-- api: M:Inno.UI.UiFocusTarget.Equals(Inno.UI.UiFocusTarget) 71e2e6843d88d74782161da857d28ca6c7ced49a29dbcd5f448023af153741cb -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-c4b7363867000c60"></a>[`public readonly record struct Inno.UI.UiFocusTarget : System.IEquatable<Inno.UI.UiFocusTarget>`](../../src/services/ui/Inno.UI/UiFocusTarget.cs#L11) | Identifies the focused element without borrowing a backend object. | — |
| <a id="api-24ff73f456b0e26a"></a>[`public Inno.UI.UiFocusTarget.UiFocusTarget(Inno.UI.UiDocumentHandle document, string elementId)`](../../src/services/ui/Inno.UI/UiFocusTarget.cs#L28) | Creates a document-scoped focus snapshot.<br>`document`：The live owning document.<br>`elementId`：The element's authored ID, or empty for an anonymous element. | `System.ArgumentException`：The document handle is unassigned.<br>`System.ArgumentNullException`：The element ID is null. |
| <a id="api-774f605828b1c0d6"></a>[`public Inno.UI.UiDocumentHandle Inno.UI.UiFocusTarget.document { get; }`](../../src/services/ui/Inno.UI/UiFocusTarget.cs#L42) | Gets the owning document identity; it must be revalidated before future operations. | — |
| <a id="api-f2af9c5679a5978a"></a>[`public string Inno.UI.UiFocusTarget.elementId { get; }`](../../src/services/ui/Inno.UI/UiFocusTarget.cs#L47) | Gets the authored element ID, or empty for an anonymous element. | — |
| <a id="api-2a87148bf1b28a76"></a>[`public override string Inno.UI.UiFocusTarget.ToString()`](../../src/services/ui/Inno.UI/UiFocusTarget.cs#L11) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — |
| <a id="api-c24b345f31245bcc"></a>[`public static bool Inno.UI.UiFocusTarget.operator !=(Inno.UI.UiFocusTarget left, Inno.UI.UiFocusTarget right)`](../../src/services/ui/Inno.UI/UiFocusTarget.cs#L11) | 编译器生成的值不等比较。 | — |
| <a id="api-b076f3cd7daf110a"></a>[`public static bool Inno.UI.UiFocusTarget.operator ==(Inno.UI.UiFocusTarget left, Inno.UI.UiFocusTarget right)`](../../src/services/ui/Inno.UI/UiFocusTarget.cs#L11) | 编译器生成的记录值相等比较。 | — |
| <a id="api-830e7ca4ee3f8a3a"></a>[`public override int Inno.UI.UiFocusTarget.GetHashCode()`](../../src/services/ui/Inno.UI/UiFocusTarget.cs#L11) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-f6a09fb1e511ff15"></a>[`public override bool Inno.UI.UiFocusTarget.Equals(object obj)`](../../src/services/ui/Inno.UI/UiFocusTarget.cs#L11) | Indicates whether this instance and a specified object are equal.<br>`obj`：The object to compare with the current instance.<br>返回：`true` if `obj` and this instance are the same type and represent the same value; otherwise, `false`. | — |
| <a id="api-1975fa2bf42fcb01"></a>[`public bool Inno.UI.UiFocusTarget.Equals(Inno.UI.UiFocusTarget other)`](../../src/services/ui/Inno.UI/UiFocusTarget.cs#L11) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |

### `Inno.UI.UiFontRegistration`

<!-- api: T:Inno.UI.UiFontRegistration 41a19c51a22ffc55176236be31a1d2bdb3e860d7839ab66bc0e33c8b470317d3 -->
<!-- api: M:Inno.UI.UiFontRegistration.#ctor(System.ReadOnlyMemory{System.Byte},System.Int32,System.String,Inno.Text.TextFontStyle,System.Int32) a91493bce53e78c9ebb1ecf6c27a7ff3f5c8aec3b96c3ef91735ff0bcacc1605 -->
<!-- api: P:Inno.UI.UiFontRegistration.data fe34b1be507c24a5204c40949090204ec92ac0170311706a5fc31424721a7e2e -->
<!-- api: P:Inno.UI.UiFontRegistration.faceIndex fddff65e748b4fd198e391df6a29602de4bd36a2f335237dfc8112fe40093277 -->
<!-- api: P:Inno.UI.UiFontRegistration.family 6d2806938ff181530e16ed4f0f56b2356079acbc87cd818ee04910cd2a9b9c84 -->
<!-- api: P:Inno.UI.UiFontRegistration.style 5bf0c157f65379a741f9611519c5a94731d15a5770c96ce868ec92c027a5f1fd -->
<!-- api: P:Inno.UI.UiFontRegistration.weight 013d4d0499cce253627e1ceaaaa9d347673f41cda0f35c032b3d9c041ed01a38 -->
<!-- api: M:Inno.UI.UiFontRegistration.ToString 1f2ba75ca3840c1ba91aee347f38a8e1bd82f04c3c070898eb9987578d88436f -->
<!-- api: M:Inno.UI.UiFontRegistration.op_Inequality(Inno.UI.UiFontRegistration,Inno.UI.UiFontRegistration) 371b8cce0d2a977653d7fffdbe07444c3d442a2ff577c41357b550e53a34b942 -->
<!-- api: M:Inno.UI.UiFontRegistration.op_Equality(Inno.UI.UiFontRegistration,Inno.UI.UiFontRegistration) b80806fddaf170c2e1b37fe88edbabc5f11bbd8d539b8f906a26ff57136233d1 -->
<!-- api: M:Inno.UI.UiFontRegistration.GetHashCode 0483642640ac8eb5dff3f2fa05fe7142dcf8897c1fb36a327ae764195b5285b9 -->
<!-- api: M:Inno.UI.UiFontRegistration.Equals(System.Object) fa7eb9cc63711876f2396e6ea0826c4f24001e561549401010f2f0f4e77b3a8a -->
<!-- api: M:Inno.UI.UiFontRegistration.Equals(Inno.UI.UiFontRegistration) a90f53bb072a655b4ae5b2b0e40e69745ea5b3afa581badb25d41ca5dda0eef7 -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-e196be1e59e1d740"></a>[`public readonly record struct Inno.UI.UiFontRegistration : System.IEquatable<Inno.UI.UiFontRegistration>`](../../src/services/ui/Inno.UI/UiFontRegistration.cs#L10) | Describes one encoded font face registration across the neutral backend boundary. | — |
| <a id="api-86b01fdbb32d83bd"></a>[`public Inno.UI.UiFontRegistration.UiFontRegistration(System.ReadOnlyMemory<byte> data, int faceIndex, string family, Inno.Text.TextFontStyle style, int weight)`](../../src/services/ui/Inno.UI/UiFontRegistration.cs#L30) | Creates a validated synchronous font registration request.<br>`data`：Encoded font bytes, valid for the duration of the backend call.<br>`faceIndex`：Zero-based face index in a font collection.<br>`family`：Logical family visible to document authors.<br>`style`：Requested logical font style.<br>`weight`：CSS-compatible weight from 100 through 1000. | — |
| <a id="api-523399e450825ae5"></a>[`public System.ReadOnlyMemory<byte> Inno.UI.UiFontRegistration.data { get; }`](../../src/services/ui/Inno.UI/UiFontRegistration.cs#L53) | Gets encoded font bytes valid for the synchronous backend call. | — |
| <a id="api-b686e2fa10566d02"></a>[`public int Inno.UI.UiFontRegistration.faceIndex { get; }`](../../src/services/ui/Inno.UI/UiFontRegistration.cs#L57) | Gets the zero-based collection face index. | — |
| <a id="api-17a57ed65ece4c5d"></a>[`public string Inno.UI.UiFontRegistration.family { get; }`](../../src/services/ui/Inno.UI/UiFontRegistration.cs#L61) | Gets the logical family. | — |
| <a id="api-d9c957e032c236e8"></a>[`public Inno.Text.TextFontStyle Inno.UI.UiFontRegistration.style { get; }`](../../src/services/ui/Inno.UI/UiFontRegistration.cs#L65) | Gets the logical style. | — |
| <a id="api-6c0a8dfbb24e4a71"></a>[`public int Inno.UI.UiFontRegistration.weight { get; }`](../../src/services/ui/Inno.UI/UiFontRegistration.cs#L69) | Gets the logical weight. | — |
| <a id="api-99d28ef726f6304d"></a>[`public override string Inno.UI.UiFontRegistration.ToString()`](../../src/services/ui/Inno.UI/UiFontRegistration.cs#L10) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — |
| <a id="api-689ccc8cfe7e56f2"></a>[`public static bool Inno.UI.UiFontRegistration.operator !=(Inno.UI.UiFontRegistration left, Inno.UI.UiFontRegistration right)`](../../src/services/ui/Inno.UI/UiFontRegistration.cs#L10) | 编译器生成的值不等比较。 | — |
| <a id="api-a6f6374898413e55"></a>[`public static bool Inno.UI.UiFontRegistration.operator ==(Inno.UI.UiFontRegistration left, Inno.UI.UiFontRegistration right)`](../../src/services/ui/Inno.UI/UiFontRegistration.cs#L10) | 编译器生成的记录值相等比较。 | — |
| <a id="api-1ca598f0a05fed79"></a>[`public override int Inno.UI.UiFontRegistration.GetHashCode()`](../../src/services/ui/Inno.UI/UiFontRegistration.cs#L10) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-cf8d1ae46b0992cf"></a>[`public override bool Inno.UI.UiFontRegistration.Equals(object obj)`](../../src/services/ui/Inno.UI/UiFontRegistration.cs#L10) | Indicates whether this instance and a specified object are equal.<br>`obj`：The object to compare with the current instance.<br>返回：`true` if `obj` and this instance are the same type and represent the same value; otherwise, `false`. | — |
| <a id="api-bca13c3db39a4fc2"></a>[`public bool Inno.UI.UiFontRegistration.Equals(Inno.UI.UiFontRegistration other)`](../../src/services/ui/Inno.UI/UiFontRegistration.cs#L10) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |

### `Inno.UI.UiContextHandle`

<!-- api: T:Inno.UI.UiContextHandle a581b69f367dadc5ddb963af950d968fd82156529b613f304f21b04014b97956 -->
<!-- api: M:Inno.UI.UiContextHandle.#ctor(System.UInt64) 22aa387dcadf50f0fb3c2a856e1d363ef6c0260efdcd07a8a015d64c14ba1cce -->
<!-- api: P:Inno.UI.UiContextHandle.value 5871c120ba6c47e73297068ed37bcbb29896bdf11b07cd69100692c10d535078 -->
<!-- api: P:Inno.UI.UiContextHandle.isValid 63761975eb4f1dbf4ae91ecbadd10f48fad3e128d35fc46e193fcabfa0d211ef -->
<!-- api: M:Inno.UI.UiContextHandle.ToString de372c952a68001e70105efe4ba268672567323cb14f0cab43971cc8506ee0e2 -->
<!-- api: M:Inno.UI.UiContextHandle.op_Inequality(Inno.UI.UiContextHandle,Inno.UI.UiContextHandle) d92bd78898db56b6e5fd8b73fc8a40f8c08f6f957b80c8932041f77c231d60ce -->
<!-- api: M:Inno.UI.UiContextHandle.op_Equality(Inno.UI.UiContextHandle,Inno.UI.UiContextHandle) bc603460fe95adb8173700c73b2529bab5bc778b499f7858bcdf81f0d9441444 -->
<!-- api: M:Inno.UI.UiContextHandle.GetHashCode d995e3bb565dd6358c9fcec920110e74b17073884898af6ad5b56503455cf374 -->
<!-- api: M:Inno.UI.UiContextHandle.Equals(System.Object) 99e258f321a20f1a6926356d244d4df2c85cb6bf48d9b67783750dc2330081c2 -->
<!-- api: M:Inno.UI.UiContextHandle.Equals(Inno.UI.UiContextHandle) aea48b6b19879c9b331612ef41b226e5010fcd486e2cd0fff4e642f8b456394b -->
<!-- api: M:Inno.UI.UiContextHandle.Deconstruct(System.UInt64@) bcf6136a05e1bf1a902fec61e46dd8e2b2875ae3c974e05a5d6c462c1365f74f -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-2c2a20f36cb6f8f5"></a>[`public readonly record struct Inno.UI.UiContextHandle : System.IEquatable<Inno.UI.UiContextHandle>`](../../src/services/ui/Inno.UI/UiHandles.cs#L9) | Identifies one UI context owned by the active UI service generation.<br>`value`：The concrete value read or transformed by this operation. | — |
| <a id="api-906415154e29c936"></a>[`public Inno.UI.UiContextHandle.UiContextHandle(ulong value)`](../../src/services/ui/Inno.UI/UiHandles.cs#L9) | Identifies one UI context owned by the active UI service generation.<br>`value`：The concrete value read or transformed by this operation. | — |
| <a id="api-978c841eb195c63a"></a>[`public ulong Inno.UI.UiContextHandle.value { get; init; }`](../../src/services/ui/Inno.UI/UiHandles.cs#L9) | The concrete value read or transformed by this operation. | — |
| <a id="api-cfaadefe9f041856"></a>[`public bool Inno.UI.UiContextHandle.isValid { get; }`](../../src/services/ui/Inno.UI/UiHandles.cs#L14) | Gets whether this handle identifies a live context candidate. | — |
| <a id="api-8a0b0b342de652ec"></a>[`public override string Inno.UI.UiContextHandle.ToString()`](../../src/services/ui/Inno.UI/UiHandles.cs#L9) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — |
| <a id="api-34567ae1c1191531"></a>[`public static bool Inno.UI.UiContextHandle.operator !=(Inno.UI.UiContextHandle left, Inno.UI.UiContextHandle right)`](../../src/services/ui/Inno.UI/UiHandles.cs#L9) | 编译器生成的值不等比较。 | — |
| <a id="api-2f0940335e1e0861"></a>[`public static bool Inno.UI.UiContextHandle.operator ==(Inno.UI.UiContextHandle left, Inno.UI.UiContextHandle right)`](../../src/services/ui/Inno.UI/UiHandles.cs#L9) | 编译器生成的记录值相等比较。 | — |
| <a id="api-4b5477b07cd7560e"></a>[`public override int Inno.UI.UiContextHandle.GetHashCode()`](../../src/services/ui/Inno.UI/UiHandles.cs#L9) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-9cf51e08e9cfc2f2"></a>[`public override bool Inno.UI.UiContextHandle.Equals(object obj)`](../../src/services/ui/Inno.UI/UiHandles.cs#L9) | Indicates whether this instance and a specified object are equal.<br>`obj`：The object to compare with the current instance.<br>返回：`true` if `obj` and this instance are the same type and represent the same value; otherwise, `false`. | — |
| <a id="api-73f495ade56cfcc6"></a>[`public bool Inno.UI.UiContextHandle.Equals(Inno.UI.UiContextHandle other)`](../../src/services/ui/Inno.UI/UiHandles.cs#L9) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |
| <a id="api-f3b8f55f0f93bcaf"></a>[`public void Inno.UI.UiContextHandle.Deconstruct(out ulong value)`](../../src/services/ui/Inno.UI/UiHandles.cs#L9) | 按声明顺序读取记录成员。 | — |

### `Inno.UI.UiDocumentHandle`

<!-- api: T:Inno.UI.UiDocumentHandle 01df9214392752c40509af939633954c497c2e6c24361d3d6f9783d891ed24ce -->
<!-- api: M:Inno.UI.UiDocumentHandle.#ctor(System.UInt64) d8b48f182e0d7aeb9c7d541b080ba8bed9047b687f4926094b66d7a5bab81e89 -->
<!-- api: P:Inno.UI.UiDocumentHandle.value 9cd9e0ed29bc2331a97008bb71a57ff9ef37e87e8a1a1664337d56b2af678f11 -->
<!-- api: P:Inno.UI.UiDocumentHandle.isValid 6ede06fd6d17bc39ea10a8083a7dfba1c04b755895952150b0710314bca46627 -->
<!-- api: M:Inno.UI.UiDocumentHandle.ToString ac04a91059f82991e70f39466e9cd5b6df5eb8a9b4b854c9a8a2f384baff804b -->
<!-- api: M:Inno.UI.UiDocumentHandle.op_Inequality(Inno.UI.UiDocumentHandle,Inno.UI.UiDocumentHandle) be5d24abd84433c8e8295b8e61768b44885609eca335d549b73da8af93956e0c -->
<!-- api: M:Inno.UI.UiDocumentHandle.op_Equality(Inno.UI.UiDocumentHandle,Inno.UI.UiDocumentHandle) bc1058469234514d90192e63741e91454adaeba41593ee42448d53d8b83f4de7 -->
<!-- api: M:Inno.UI.UiDocumentHandle.GetHashCode fe427785e35f29b675346340bf84c021e0c29055cafb907612539aaa34911a24 -->
<!-- api: M:Inno.UI.UiDocumentHandle.Equals(System.Object) 5a67ad65384a10f3b8908e3e584a0cf7e4ed01e9a477895ca2f110f00e97c4cb -->
<!-- api: M:Inno.UI.UiDocumentHandle.Equals(Inno.UI.UiDocumentHandle) 1b3dfe4c0ebf52d150c5e41de478b16c3b01d33df3b76411c708558dbe82faef -->
<!-- api: M:Inno.UI.UiDocumentHandle.Deconstruct(System.UInt64@) 1e39a5d6673e92542092b3a8e71d9541f54cb82ee76a42c149d5f0dcb31c44d6 -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-28d865ee3c467859"></a>[`public readonly record struct Inno.UI.UiDocumentHandle : System.IEquatable<Inno.UI.UiDocumentHandle>`](../../src/services/ui/Inno.UI/UiHandles.cs#L23) | Identifies one document owned by a UI context.<br>`value`：The concrete value read or transformed by this operation. | — |
| <a id="api-43707c37302ded9d"></a>[`public Inno.UI.UiDocumentHandle.UiDocumentHandle(ulong value)`](../../src/services/ui/Inno.UI/UiHandles.cs#L23) | Identifies one document owned by a UI context.<br>`value`：The concrete value read or transformed by this operation. | — |
| <a id="api-1397cc1d7ba80bb6"></a>[`public ulong Inno.UI.UiDocumentHandle.value { get; init; }`](../../src/services/ui/Inno.UI/UiHandles.cs#L23) | The concrete value read or transformed by this operation. | — |
| <a id="api-43f3d1d1476d3de2"></a>[`public bool Inno.UI.UiDocumentHandle.isValid { get; }`](../../src/services/ui/Inno.UI/UiHandles.cs#L28) | Gets whether this handle identifies a live document candidate. | — |
| <a id="api-39f4e9a86d98ef81"></a>[`public override string Inno.UI.UiDocumentHandle.ToString()`](../../src/services/ui/Inno.UI/UiHandles.cs#L23) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — |
| <a id="api-1f6dedb6f3483ea6"></a>[`public static bool Inno.UI.UiDocumentHandle.operator !=(Inno.UI.UiDocumentHandle left, Inno.UI.UiDocumentHandle right)`](../../src/services/ui/Inno.UI/UiHandles.cs#L23) | 编译器生成的值不等比较。 | — |
| <a id="api-9949e4955ed2fb2a"></a>[`public static bool Inno.UI.UiDocumentHandle.operator ==(Inno.UI.UiDocumentHandle left, Inno.UI.UiDocumentHandle right)`](../../src/services/ui/Inno.UI/UiHandles.cs#L23) | 编译器生成的记录值相等比较。 | — |
| <a id="api-1f8ddb34f13771b8"></a>[`public override int Inno.UI.UiDocumentHandle.GetHashCode()`](../../src/services/ui/Inno.UI/UiHandles.cs#L23) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-a3368b294ebef5dc"></a>[`public override bool Inno.UI.UiDocumentHandle.Equals(object obj)`](../../src/services/ui/Inno.UI/UiHandles.cs#L23) | Indicates whether this instance and a specified object are equal.<br>`obj`：The object to compare with the current instance.<br>返回：`true` if `obj` and this instance are the same type and represent the same value; otherwise, `false`. | — |
| <a id="api-405e4b35076bfd73"></a>[`public bool Inno.UI.UiDocumentHandle.Equals(Inno.UI.UiDocumentHandle other)`](../../src/services/ui/Inno.UI/UiHandles.cs#L23) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |
| <a id="api-f50eaf31e73fd7bf"></a>[`public void Inno.UI.UiDocumentHandle.Deconstruct(out ulong value)`](../../src/services/ui/Inno.UI/UiHandles.cs#L23) | 按声明顺序读取记录成员。 | — |

### `Inno.UI.UiDocumentLanguageId`

<!-- api: T:Inno.UI.UiDocumentLanguageId fcfbcf7b3095c1f84b6a307af36db246ca17ab9e641f5f2fc5d93b133d95b10f -->
<!-- api: M:Inno.UI.UiDocumentLanguageId.#ctor(System.String) d84e8fc958512cb9678e3800cb60d230748b4a15011b0835b01aa3bc1a555ac1 -->
<!-- api: P:Inno.UI.UiDocumentLanguageId.value 2334a0e8f541660626ea848230a428bb8e8c12aeb415832dc6a1fa1e6a1e9f77 -->
<!-- api: P:Inno.UI.UiDocumentLanguageId.isValid 4c71808f8ab8536d85e5c04d79d74b0ae61841ee5e895f386de96b66dc511686 -->
<!-- api: M:Inno.UI.UiDocumentLanguageId.ToString 54e7f6468f202b183a5483b2f0bcdea991996adce6724b1bad4b356f7d38110a -->
<!-- api: M:Inno.UI.UiDocumentLanguageId.op_Inequality(Inno.UI.UiDocumentLanguageId,Inno.UI.UiDocumentLanguageId) e29af3d01fd95d89f6b7c3c42e64779c83d3adc65409503a4b3cc9d351a2d20a -->
<!-- api: M:Inno.UI.UiDocumentLanguageId.op_Equality(Inno.UI.UiDocumentLanguageId,Inno.UI.UiDocumentLanguageId) f528ac3ef2c5ff78e694559f012feb90bcb51e61228323608cf4d49e3c56b490 -->
<!-- api: M:Inno.UI.UiDocumentLanguageId.GetHashCode d6b0216409590bb88688e5ffbdbb0074d1d08b883ddfedba5a0d11aa85df9381 -->
<!-- api: M:Inno.UI.UiDocumentLanguageId.Equals(System.Object) d197ac7bb4c04da537487ac580d9a826bc8a3bdfe0d37d1e0bcffd5e58fb5eef -->
<!-- api: M:Inno.UI.UiDocumentLanguageId.Equals(Inno.UI.UiDocumentLanguageId) f775690ee5c497a721699aa1832ab672c1871b5ef558e46857a9a49e03680c3f -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-c6c7334fcce1811d"></a>[`public readonly record struct Inno.UI.UiDocumentLanguageId : System.IEquatable<Inno.UI.UiDocumentLanguageId>`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L8) | Identifies a document source language independently from any UI backend implementation. | — |
| <a id="api-d502e9991c748c78"></a>[`public Inno.UI.UiDocumentLanguageId.UiDocumentLanguageId(string value)`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L16) | Creates a stable, ordinal language identifier.<br>`value`：Provider-qualified nonempty identifier without whitespace. | — |
| <a id="api-82a278fd0c5e9afe"></a>[`public string Inno.UI.UiDocumentLanguageId.value { get; }`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L28) | Gets the stable language identifier. | — |
| <a id="api-8d7009482a84f53c"></a>[`public bool Inno.UI.UiDocumentLanguageId.isValid { get; }`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L33) | Gets whether the identifier is assigned. | — |
| <a id="api-4c9e1ab2dcf575b4"></a>[`public override string Inno.UI.UiDocumentLanguageId.ToString()`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L41) | Returns the stable identifier.<br>返回：The identifier, or an empty string for an unassigned value. | — |
| <a id="api-865adde901788408"></a>[`public static bool Inno.UI.UiDocumentLanguageId.operator !=(Inno.UI.UiDocumentLanguageId left, Inno.UI.UiDocumentLanguageId right)`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L8) | 编译器生成的值不等比较。 | — |
| <a id="api-e9347ed6a949b91b"></a>[`public static bool Inno.UI.UiDocumentLanguageId.operator ==(Inno.UI.UiDocumentLanguageId left, Inno.UI.UiDocumentLanguageId right)`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L8) | 编译器生成的记录值相等比较。 | — |
| <a id="api-30e1809a576198da"></a>[`public override int Inno.UI.UiDocumentLanguageId.GetHashCode()`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L8) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-d0c29954072d5143"></a>[`public override bool Inno.UI.UiDocumentLanguageId.Equals(object obj)`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L8) | Indicates whether this instance and a specified object are equal.<br>`obj`：The object to compare with the current instance.<br>返回：`true` if `obj` and this instance are the same type and represent the same value; otherwise, `false`. | — |
| <a id="api-83a343f4739d0500"></a>[`public bool Inno.UI.UiDocumentLanguageId.Equals(Inno.UI.UiDocumentLanguageId other)`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |

### `Inno.UI.UiMeshHandle`

<!-- api: T:Inno.UI.UiMeshHandle 08a65a7afcc04563cf375edba0e722563e7d895ab255135c1d1257ff45fcd3f6 -->
<!-- api: M:Inno.UI.UiMeshHandle.#ctor(System.UInt64) 6d9b4593602dcac090724dfed6ab28badf0164dcbe612929ae7d20993c3c5e1e -->
<!-- api: P:Inno.UI.UiMeshHandle.value 142971a6de133d32c80bf04ac6c2ee08bf2ea58132a6c6f585b2bf845ad658a8 -->
<!-- api: P:Inno.UI.UiMeshHandle.isValid 1def3081f09c7f58a6a4717f4f6bcffefb36c70179f777ead24c6db2a3b6b17f -->
<!-- api: M:Inno.UI.UiMeshHandle.ToString 2ecf38201f605f3da0b4d0e32119e725a6a6db57eb04d74d5fb2d305e4e55f3f -->
<!-- api: M:Inno.UI.UiMeshHandle.op_Inequality(Inno.UI.UiMeshHandle,Inno.UI.UiMeshHandle) e8ae75c593386acca523b1bb1648fddc1ea5f25002a9987bd211664b6d1ea17b -->
<!-- api: M:Inno.UI.UiMeshHandle.op_Equality(Inno.UI.UiMeshHandle,Inno.UI.UiMeshHandle) 50fc888ebf4895283ab31a5b923a43865de8165eb7cfd57e11f0c320201e7a08 -->
<!-- api: M:Inno.UI.UiMeshHandle.GetHashCode e247f22edc1303c449ec13d96d23ce027929e41b6a9bf04f7afd3e86ae143d1e -->
<!-- api: M:Inno.UI.UiMeshHandle.Equals(System.Object) feda83241dd3f1be9f223bd9fd2f9599d2a851ac382aac3e2bc97a2b1f4bbb1c -->
<!-- api: M:Inno.UI.UiMeshHandle.Equals(Inno.UI.UiMeshHandle) fbb94ae3651766f6048b5e3aafb3cba2af172bcdc272626e1f08455f131bbf73 -->
<!-- api: M:Inno.UI.UiMeshHandle.Deconstruct(System.UInt64@) b40c41f57236b9e8a3438f73879d77b04b7f94993c1947af147e284c3025deab -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-015e55fcad5b2368"></a>[`public readonly record struct Inno.UI.UiMeshHandle : System.IEquatable<Inno.UI.UiMeshHandle>`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L50) | Identifies one backend-owned immutable mesh generation.<br>`value`：The concrete value read or transformed by this operation. | — |
| <a id="api-f0dfc167befd83a7"></a>[`public Inno.UI.UiMeshHandle.UiMeshHandle(ulong value)`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L50) | Identifies one backend-owned immutable mesh generation.<br>`value`：The concrete value read or transformed by this operation. | — |
| <a id="api-a33933d5bd4a04f3"></a>[`public ulong Inno.UI.UiMeshHandle.value { get; init; }`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L50) | The concrete value read or transformed by this operation. | — |
| <a id="api-d6f5db618581b154"></a>[`public bool Inno.UI.UiMeshHandle.isValid { get; }`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L55) | Gets whether the handle identifies a mesh. | — |
| <a id="api-858b8bd9815ac75c"></a>[`public override string Inno.UI.UiMeshHandle.ToString()`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L50) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — |
| <a id="api-2f4b9cbffb36d86f"></a>[`public static bool Inno.UI.UiMeshHandle.operator !=(Inno.UI.UiMeshHandle left, Inno.UI.UiMeshHandle right)`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L50) | 编译器生成的值不等比较。 | — |
| <a id="api-c1e81f1a998afee6"></a>[`public static bool Inno.UI.UiMeshHandle.operator ==(Inno.UI.UiMeshHandle left, Inno.UI.UiMeshHandle right)`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L50) | 编译器生成的记录值相等比较。 | — |
| <a id="api-2a74305d11d8a817"></a>[`public override int Inno.UI.UiMeshHandle.GetHashCode()`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L50) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-e25001db7f781a29"></a>[`public override bool Inno.UI.UiMeshHandle.Equals(object obj)`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L50) | Indicates whether this instance and a specified object are equal.<br>`obj`：The object to compare with the current instance.<br>返回：`true` if `obj` and this instance are the same type and represent the same value; otherwise, `false`. | — |
| <a id="api-dbe7fbecb0f3c059"></a>[`public bool Inno.UI.UiMeshHandle.Equals(Inno.UI.UiMeshHandle other)`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L50) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |
| <a id="api-e6a63ee936a4e08b"></a>[`public void Inno.UI.UiMeshHandle.Deconstruct(out ulong value)`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L50) | 按声明顺序读取记录成员。 | — |

### `Inno.UI.UiTextureHandle`

<!-- api: T:Inno.UI.UiTextureHandle ba39df2d42a237f7ecf38f0c86d3e314216e1e7136e6cde7a65a39068e6b0082 -->
<!-- api: M:Inno.UI.UiTextureHandle.#ctor(System.UInt64) aaf1046f3827fea1474d4cfc16f003c94788e89067be02f010ace6c808439999 -->
<!-- api: P:Inno.UI.UiTextureHandle.value eeb8d337994362baac052835539d344a92a35273e9bef89043321f1b69343d33 -->
<!-- api: P:Inno.UI.UiTextureHandle.isValid 57be8082717c44ebd605b941e3f6c778d00544ea9f5ed372d2791cb3f72953e0 -->
<!-- api: M:Inno.UI.UiTextureHandle.ToString d78085625396c676c93ea3ab42d2ae13d086a453d41bbde710cef45fbdff28f5 -->
<!-- api: M:Inno.UI.UiTextureHandle.op_Inequality(Inno.UI.UiTextureHandle,Inno.UI.UiTextureHandle) c2c33243e2eb313676d36baeff26d77ebb660ecdba7b835d120d0654622e8267 -->
<!-- api: M:Inno.UI.UiTextureHandle.op_Equality(Inno.UI.UiTextureHandle,Inno.UI.UiTextureHandle) f82f2ca8124bda07a244e1c91328014333fd65a791604ec4af38eb8c28b543ac -->
<!-- api: M:Inno.UI.UiTextureHandle.GetHashCode 83c600d8b3ad6392a953fd03d249fa25e1477856baf3fbecc2a514358124de3a -->
<!-- api: M:Inno.UI.UiTextureHandle.Equals(System.Object) c7806b3653aa408830a1eb71dc1e65336b08cfb68f159ea510899e69aaa97b0d -->
<!-- api: M:Inno.UI.UiTextureHandle.Equals(Inno.UI.UiTextureHandle) e923dc1eb151ef4b3284b895ec31098cfe4224fbf3eb4f429722a99932c41d0b -->
<!-- api: M:Inno.UI.UiTextureHandle.Deconstruct(System.UInt64@) c2adc21340249aab4d25b9f051ea44c373fff7865323102bb46be387d01b51bb -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-27e920bedf7c3db2"></a>[`public readonly record struct Inno.UI.UiTextureHandle : System.IEquatable<Inno.UI.UiTextureHandle>`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L64) | Identifies one backend-owned immutable texture generation.<br>`value`：The concrete value read or transformed by this operation. | — |
| <a id="api-e58fe30ab1f33c8a"></a>[`public Inno.UI.UiTextureHandle.UiTextureHandle(ulong value)`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L64) | Identifies one backend-owned immutable texture generation.<br>`value`：The concrete value read or transformed by this operation. | — |
| <a id="api-5ed32912f337f2fc"></a>[`public ulong Inno.UI.UiTextureHandle.value { get; init; }`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L64) | The concrete value read or transformed by this operation. | — |
| <a id="api-b8b87672e61291e1"></a>[`public bool Inno.UI.UiTextureHandle.isValid { get; }`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L69) | Gets whether the handle identifies a texture; an unassigned handle selects the renderer's white texture. | — |
| <a id="api-a580419a6e0e3733"></a>[`public override string Inno.UI.UiTextureHandle.ToString()`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L64) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — |
| <a id="api-a0216e0253f164a3"></a>[`public static bool Inno.UI.UiTextureHandle.operator !=(Inno.UI.UiTextureHandle left, Inno.UI.UiTextureHandle right)`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L64) | 编译器生成的值不等比较。 | — |
| <a id="api-4c9ce2273bfc9155"></a>[`public static bool Inno.UI.UiTextureHandle.operator ==(Inno.UI.UiTextureHandle left, Inno.UI.UiTextureHandle right)`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L64) | 编译器生成的记录值相等比较。 | — |
| <a id="api-470199c909f394d0"></a>[`public override int Inno.UI.UiTextureHandle.GetHashCode()`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L64) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-d760dbe9808cd89c"></a>[`public override bool Inno.UI.UiTextureHandle.Equals(object obj)`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L64) | Indicates whether this instance and a specified object are equal.<br>`obj`：The object to compare with the current instance.<br>返回：`true` if `obj` and this instance are the same type and represent the same value; otherwise, `false`. | — |
| <a id="api-5ed14de0527b2bfd"></a>[`public bool Inno.UI.UiTextureHandle.Equals(Inno.UI.UiTextureHandle other)`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L64) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |
| <a id="api-925f9b19f1ad78ce"></a>[`public void Inno.UI.UiTextureHandle.Deconstruct(out ulong value)`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L64) | 按声明顺序读取记录成员。 | — |

### `Inno.UI.UiImageSource`

<!-- api: T:Inno.UI.UiImageSource 178893c6fde9667002405bda76a2fe1da608346dcbb8a5af39012ac6089daef8 -->
<!-- api: M:Inno.UI.UiImageSource.#ctor(Inno.Rendering.Assets.RenderTextureArtifactReference,System.Int32,System.Int32) 6a06e869638686d369e0b6063164811384103cc6c6157f283737853a386d99d7 -->
<!-- api: P:Inno.UI.UiImageSource.artifact 84cd3d5f07e93182c2816c52401bfe55e564dea843d063548507b686f9c15fae -->
<!-- api: P:Inno.UI.UiImageSource.width e81c5264ce1aa7a3e985039cd5582bba5a2215201b9da581993a59f940a64e4a -->
<!-- api: P:Inno.UI.UiImageSource.height 0964bc69f975df64eed9f720e93f3ee83c47719e7d78d5e64a39b33d379d0d1e -->
<!-- api: M:Inno.UI.UiImageSource.FromTexture(Inno.Rendering.Assets.TextureAsset) d5708084d27a450549b2c2d724451a741dd8697ebbe6c72243a03710e8545cbc -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-9d540c8707d604da"></a>[`public sealed class Inno.UI.UiImageSource`](../../src/services/ui/Inno.UI/UiImageSource.cs#L9) | Describes an imported image without decoding or owning its GPU texture. | — |
| <a id="api-f276cbcc767209cb"></a>[`public Inno.UI.UiImageSource.UiImageSource(Inno.Rendering.Assets.RenderTextureArtifactReference artifact, int width, int height)`](../../src/services/ui/Inno.UI/UiImageSource.cs#L29) | Captures an immutable texture artifact and its imported pixel dimensions.<br>`artifact`：Validated imported texture slot and content identity.<br>`width`：Positive imported pixel width.<br>`height`：Positive imported pixel height. | `System.ArgumentException`：The artifact is unassigned.<br>`System.ArgumentOutOfRangeException`：A dimension is not positive. |
| <a id="api-6f95c8b4d1967ef8"></a>[`public Inno.Rendering.Assets.RenderTextureArtifactReference Inno.UI.UiImageSource.artifact { get; }`](../../src/services/ui/Inno.UI/UiImageSource.cs#L47) | Gets the frozen imported texture identity; Rendering owns resolution and residency. | — |
| <a id="api-5b24fbffedd374f6"></a>[`public int Inno.UI.UiImageSource.width { get; }`](../../src/services/ui/Inno.UI/UiImageSource.cs#L52) | Gets the imported pixel width used by layout. | — |
| <a id="api-ba73f6474e9d63ce"></a>[`public int Inno.UI.UiImageSource.height { get; }`](../../src/services/ui/Inno.UI/UiImageSource.cs#L57) | Gets the imported pixel height used by layout. | — |
| <a id="api-13354662d304c8f7"></a>[`public static Inno.UI.UiImageSource Inno.UI.UiImageSource.FromTexture(Inno.Rendering.Assets.TextureAsset texture)`](../../src/services/ui/Inno.UI/UiImageSource.cs#L74) | Captures a loaded texture without retaining the asset object.<br>`texture`：Loaded texture in the caller's current asset generation.<br>返回：An immutable source that remains independent of subsequent asset mutation. | `System.ArgumentNullException`：The asset is null.<br>`System.InvalidOperationException`：The asset is Missing or has no imported dimensions. |

### `Inno.UI.UiLayoutSnapshot`

<!-- api: T:Inno.UI.UiLayoutSnapshot 307ed0ff0b711b8dc07bf10c2dab478094b21a93bf236d290ea8f63fbc94b7a5 -->
<!-- api: M:Inno.UI.UiLayoutSnapshot.#ctor(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.UInt64,System.Collections.Generic.IEnumerable{Inno.UI.UiElementLayout}) fb41aff184bb7f872b72e68008c6f2819b5aa12599c0de3c751b58e41b9db4ed -->
<!-- api: P:Inno.UI.UiLayoutSnapshot.context 4372a25dfd0aee52baa9d41e8099ef65c8be24b51a0bc6f34497b3916b949e3e -->
<!-- api: P:Inno.UI.UiLayoutSnapshot.document b6d6a862356288644d757ae20f9a434b5352d3d68c1f092c041de9b1b331fb3a -->
<!-- api: P:Inno.UI.UiLayoutSnapshot.revision 14df0a87349f46fefe7f0a357c2c2a261711702d47667b560e6b195651869675 -->
<!-- api: P:Inno.UI.UiLayoutSnapshot.elements 980ca1e4435aeffa8eb5a0c8306bd7731ef775db4098c1144c53553ac6631800 -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-a71f9de089e44506"></a>[`public sealed class Inno.UI.UiLayoutSnapshot`](../../src/services/ui/Inno.UI/UiLayoutSnapshot.cs#L14) | Holds an immutable batch of element measurements from one document and layout revision. | — |
| <a id="api-d0fe1bb9261cfada"></a>[`public Inno.UI.UiLayoutSnapshot.UiLayoutSnapshot(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, ulong revision, System.Collections.Generic.IEnumerable<Inno.UI.UiElementLayout> elements)`](../../src/services/ui/Inno.UI/UiLayoutSnapshot.cs#L37) | Freezes the measurements produced by one owner-thread layout capture.<br>`context`：The context that produced the measurement.<br>`document`：The owning document, validated by the backend during capture.<br>`revision`：The nonzero context-local layout revision; it is not a persistent format version.<br>`elements`：Ordered measurements corresponding to the request, copied into this snapshot. | `System.ArgumentException`：A handle is unassigned or the revision is zero.<br>`System.ArgumentNullException`：The measurements are null. |
| <a id="api-d4ae13628ac0f894"></a>[`public Inno.UI.UiContextHandle Inno.UI.UiLayoutSnapshot.context { get; }`](../../src/services/ui/Inno.UI/UiLayoutSnapshot.cs#L55) | Gets the context identity that produced these measurements. | — |
| <a id="api-63ed80c671372c77"></a>[`public Inno.UI.UiDocumentHandle Inno.UI.UiLayoutSnapshot.document { get; }`](../../src/services/ui/Inno.UI/UiLayoutSnapshot.cs#L59) | Gets the document identity that produced these measurements. | — |
| <a id="api-15b0a99b58effee3"></a>[`public ulong Inno.UI.UiLayoutSnapshot.revision { get; }`](../../src/services/ui/Inno.UI/UiLayoutSnapshot.cs#L63) | Gets the context-local revision shared by all measurements in this batch. | — |
| <a id="api-9988781a3968c6eb"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.UI.UiElementLayout> Inno.UI.UiLayoutSnapshot.elements { get; }`](../../src/services/ui/Inno.UI/UiLayoutSnapshot.cs#L67) | Gets the immutable measurements in request order, including missing and hidden entries. | — |

### `Inno.UI.UiVertex`

<!-- api: T:Inno.UI.UiVertex 97d056f784ddc120d7287765b21ffe5aaaee215b34af5f8a37033f08dec7d5ec -->
<!-- api: M:Inno.UI.UiVertex.#ctor(System.Single,System.Single,System.Single,System.Single,System.UInt32) ee3e7c34038a4d56e5019e78451fe22ace3e7afdc6175fa36cd10c40d3bab64b -->
<!-- api: P:Inno.UI.UiVertex.x d155caa8fca38a485da29045a1fa02cbee26c69f8820834198134ea87ff2801f -->
<!-- api: P:Inno.UI.UiVertex.y 4fc9ad4a3839fbca5b941f77e931151dd2b331ee8ad9cc93863d8714b3dd9350 -->
<!-- api: P:Inno.UI.UiVertex.u dbcc1937cf7665342ee0ac13a674df323665b3d050ce021d0d63a80607b4391c -->
<!-- api: P:Inno.UI.UiVertex.v bf246e0940de06436a604c6948fe79566cf17626f4a8089e9496d4fe903e937e -->
<!-- api: P:Inno.UI.UiVertex.color a0c96c3b3d75efa732e49d8805ebf03203644b3f20f46c690e8acdd36699db29 -->
<!-- api: M:Inno.UI.UiVertex.ToString 5834924a78ab345741887954cd675105a8f0cc524392bc112d6c87dd9f9eef09 -->
<!-- api: M:Inno.UI.UiVertex.op_Inequality(Inno.UI.UiVertex,Inno.UI.UiVertex) 16d45080166eff210bdb376395626557d294204fa864c8e476bc49e7b8464838 -->
<!-- api: M:Inno.UI.UiVertex.op_Equality(Inno.UI.UiVertex,Inno.UI.UiVertex) 40d08563d631d7f9ca400508ff15186f8e88952ad68e6e7ccbda7f269caa8ce9 -->
<!-- api: M:Inno.UI.UiVertex.GetHashCode b27db223f4ecf11d380dd3d86389d4a3f7e395fdb88cb0940cb05c24ced7ea77 -->
<!-- api: M:Inno.UI.UiVertex.Equals(System.Object) 52db8541664b268a003ece884b2a105c496063aba0fd7933baecdb111ee27ebc -->
<!-- api: M:Inno.UI.UiVertex.Equals(Inno.UI.UiVertex) e01562f0dec9c75d28b86b69acd32f10510b49ef3cd91fa24142655e34e4f0e3 -->
<!-- api: M:Inno.UI.UiVertex.Deconstruct(System.Single@,System.Single@,System.Single@,System.Single@,System.UInt32@) 06e3e3159c76885bab7eb15ea0379356d31fdaca91d40526d896409b0ba3763e -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-53981e37451785a0"></a>[`public readonly record struct Inno.UI.UiVertex : System.IEquatable<Inno.UI.UiVertex>`](../../src/services/ui/Inno.UI/UiRendering.cs#L25) | Describes one vertex in backend-neutral UI draw geometry.<br>`x`：The horizontal or first component.<br>`y`：The vertical or second component.<br>`u`：The float value used to initialize this instance.<br>`v`：The concrete value read or transformed by this operation.<br>`color`：The uint value used to initialize this instance. | — |
| <a id="api-70686e202b32a857"></a>[`public Inno.UI.UiVertex.UiVertex(float x, float y, float u, float v, uint color)`](../../src/services/ui/Inno.UI/UiRendering.cs#L25) | Describes one vertex in backend-neutral UI draw geometry.<br>`x`：The horizontal or first component.<br>`y`：The vertical or second component.<br>`u`：The float value used to initialize this instance.<br>`v`：The concrete value read or transformed by this operation.<br>`color`：The uint value used to initialize this instance. | — |
| <a id="api-3330e11b9d664a79"></a>[`public float Inno.UI.UiVertex.x { get; init; }`](../../src/services/ui/Inno.UI/UiRendering.cs#L26) | The horizontal or first component. | — |
| <a id="api-b75879f6ffefeaec"></a>[`public float Inno.UI.UiVertex.y { get; init; }`](../../src/services/ui/Inno.UI/UiRendering.cs#L27) | The vertical or second component. | — |
| <a id="api-9f78aab721c6b417"></a>[`public float Inno.UI.UiVertex.u { get; init; }`](../../src/services/ui/Inno.UI/UiRendering.cs#L28) | The float value used to initialize this instance. | — |
| <a id="api-3286a7009c559745"></a>[`public float Inno.UI.UiVertex.v { get; init; }`](../../src/services/ui/Inno.UI/UiRendering.cs#L29) | The concrete value read or transformed by this operation. | — |
| <a id="api-197818aeeb1859d0"></a>[`public uint Inno.UI.UiVertex.color { get; init; }`](../../src/services/ui/Inno.UI/UiRendering.cs#L30) | The uint value used to initialize this instance. | — |
| <a id="api-7718521e226d4fd1"></a>[`public override string Inno.UI.UiVertex.ToString()`](../../src/services/ui/Inno.UI/UiRendering.cs#L25) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — |
| <a id="api-ffff143e9c2d3f2d"></a>[`public static bool Inno.UI.UiVertex.operator !=(Inno.UI.UiVertex left, Inno.UI.UiVertex right)`](../../src/services/ui/Inno.UI/UiRendering.cs#L25) | 编译器生成的值不等比较。 | — |
| <a id="api-c2aa824a24e6ab86"></a>[`public static bool Inno.UI.UiVertex.operator ==(Inno.UI.UiVertex left, Inno.UI.UiVertex right)`](../../src/services/ui/Inno.UI/UiRendering.cs#L25) | 编译器生成的记录值相等比较。 | — |
| <a id="api-20978900bdaa2a9c"></a>[`public override int Inno.UI.UiVertex.GetHashCode()`](../../src/services/ui/Inno.UI/UiRendering.cs#L25) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-90cccaa0ee72fa94"></a>[`public override bool Inno.UI.UiVertex.Equals(object obj)`](../../src/services/ui/Inno.UI/UiRendering.cs#L25) | Indicates whether this instance and a specified object are equal.<br>`obj`：The object to compare with the current instance.<br>返回：`true` if `obj` and this instance are the same type and represent the same value; otherwise, `false`. | — |
| <a id="api-a6b769e150c881b9"></a>[`public bool Inno.UI.UiVertex.Equals(Inno.UI.UiVertex other)`](../../src/services/ui/Inno.UI/UiRendering.cs#L25) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |
| <a id="api-1b2ce7a50066ed01"></a>[`public void Inno.UI.UiVertex.Deconstruct(out float x, out float y, out float u, out float v, out uint color)`](../../src/services/ui/Inno.UI/UiRendering.cs#L25) | 按声明顺序读取记录成员。 | — |

### `Inno.UI.UiClipRectangle`

<!-- api: T:Inno.UI.UiClipRectangle 1c0edeb5f5d83b556bc94220fd0a61fc57bc7faacb5d5d3237b36b6e64f5a8ed -->
<!-- api: M:Inno.UI.UiClipRectangle.#ctor(System.Int32,System.Int32,System.Int32,System.Int32) a0138c53109a0057672a36aab513d37b8290ebc8b7331f74676322c1e809ccdb -->
<!-- api: P:Inno.UI.UiClipRectangle.x 654f06bb5f39d663a9dfac0eeb5cfba641372cd6ab5a40a8ab82ecbe3bc098d9 -->
<!-- api: P:Inno.UI.UiClipRectangle.y 21ed6733937130eee46b3807163ca2cb4e8e8cef3c8c21e2aa201fab96faac24 -->
<!-- api: P:Inno.UI.UiClipRectangle.width 527cda6d5fa1d73296bffb3781ccb62828c406ee3454362cf34854e194cf9294 -->
<!-- api: P:Inno.UI.UiClipRectangle.height 23de653a7db93e546c7ddd6aabe80263c2b65cc5b313b8e0d1f2e799fbdfa7af -->
<!-- api: M:Inno.UI.UiClipRectangle.ToString 8ebe34291238409989073b144bd62b9fc2a026c68dc535c292158b04e77506e1 -->
<!-- api: M:Inno.UI.UiClipRectangle.op_Inequality(Inno.UI.UiClipRectangle,Inno.UI.UiClipRectangle) b310e4c8755d3323d1c3ff80655c1fbafe1e65b82bc0f19bf28c0b2eb06d6e15 -->
<!-- api: M:Inno.UI.UiClipRectangle.op_Equality(Inno.UI.UiClipRectangle,Inno.UI.UiClipRectangle) ef4fa77455fce09ec221dbb41ba9b7ecc94334667042e3b1de9a057784db9650 -->
<!-- api: M:Inno.UI.UiClipRectangle.GetHashCode fc248735ba7bc5fd2b0765de85218c5e58b3ecaceb481210bb6a1ecd48ce77bc -->
<!-- api: M:Inno.UI.UiClipRectangle.Equals(System.Object) e3d43976c93034a598e6e2e2d9f9f13ff423b2b1359cfd5b27003838442af3ad -->
<!-- api: M:Inno.UI.UiClipRectangle.Equals(Inno.UI.UiClipRectangle) 49307b025f69d697845733465657e86976ee5930b9b3cc4340ad1fa7d9f128d9 -->
<!-- api: M:Inno.UI.UiClipRectangle.Deconstruct(System.Int32@,System.Int32@,System.Int32@,System.Int32@) 761e25947f5f4f77c348f653d217ff0d0aa9c47ade1fa5591d8eb784b4a15f9c -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-027621c659e0930a"></a>[`public readonly record struct Inno.UI.UiClipRectangle : System.IEquatable<Inno.UI.UiClipRectangle>`](../../src/services/ui/Inno.UI/UiRendering.cs#L48) | Defines an integer clipping rectangle in UI surface coordinates.<br>`x`：The horizontal or first component.<br>`y`：The vertical or second component.<br>`width`：The width in logical units or pixels required by this operation.<br>`height`：The height in logical units or pixels required by this operation. | — |
| <a id="api-96ddf7b3c5853ef5"></a>[`public Inno.UI.UiClipRectangle.UiClipRectangle(int x, int y, int width, int height)`](../../src/services/ui/Inno.UI/UiRendering.cs#L48) | Defines an integer clipping rectangle in UI surface coordinates.<br>`x`：The horizontal or first component.<br>`y`：The vertical or second component.<br>`width`：The width in logical units or pixels required by this operation.<br>`height`：The height in logical units or pixels required by this operation. | — |
| <a id="api-24ba5265d15341c2"></a>[`public int Inno.UI.UiClipRectangle.x { get; init; }`](../../src/services/ui/Inno.UI/UiRendering.cs#L49) | The horizontal or first component. | — |
| <a id="api-ca76796e15c710d2"></a>[`public int Inno.UI.UiClipRectangle.y { get; init; }`](../../src/services/ui/Inno.UI/UiRendering.cs#L50) | The vertical or second component. | — |
| <a id="api-3ccdc8137a1c195a"></a>[`public int Inno.UI.UiClipRectangle.width { get; init; }`](../../src/services/ui/Inno.UI/UiRendering.cs#L51) | The width in logical units or pixels required by this operation. | — |
| <a id="api-562146061d4aeab3"></a>[`public int Inno.UI.UiClipRectangle.height { get; init; }`](../../src/services/ui/Inno.UI/UiRendering.cs#L52) | The height in logical units or pixels required by this operation. | — |
| <a id="api-0f0ae2b24c10ead0"></a>[`public override string Inno.UI.UiClipRectangle.ToString()`](../../src/services/ui/Inno.UI/UiRendering.cs#L48) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — |
| <a id="api-e696ce0667c02c57"></a>[`public static bool Inno.UI.UiClipRectangle.operator !=(Inno.UI.UiClipRectangle left, Inno.UI.UiClipRectangle right)`](../../src/services/ui/Inno.UI/UiRendering.cs#L48) | 编译器生成的值不等比较。 | — |
| <a id="api-59497ce7cd3a17d3"></a>[`public static bool Inno.UI.UiClipRectangle.operator ==(Inno.UI.UiClipRectangle left, Inno.UI.UiClipRectangle right)`](../../src/services/ui/Inno.UI/UiRendering.cs#L48) | 编译器生成的记录值相等比较。 | — |
| <a id="api-b8797630c1f49d24"></a>[`public override int Inno.UI.UiClipRectangle.GetHashCode()`](../../src/services/ui/Inno.UI/UiRendering.cs#L48) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-5fd4284e7a42eddd"></a>[`public override bool Inno.UI.UiClipRectangle.Equals(object obj)`](../../src/services/ui/Inno.UI/UiRendering.cs#L48) | Indicates whether this instance and a specified object are equal.<br>`obj`：The object to compare with the current instance.<br>返回：`true` if `obj` and this instance are the same type and represent the same value; otherwise, `false`. | — |
| <a id="api-cc10695fc6f2b0bb"></a>[`public bool Inno.UI.UiClipRectangle.Equals(Inno.UI.UiClipRectangle other)`](../../src/services/ui/Inno.UI/UiRendering.cs#L48) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |
| <a id="api-4e346cdceb679edb"></a>[`public void Inno.UI.UiClipRectangle.Deconstruct(out int x, out int y, out int width, out int height)`](../../src/services/ui/Inno.UI/UiRendering.cs#L48) | 按声明顺序读取记录成员。 | — |

### `Inno.UI.UiMeshUpdate`

<!-- api: T:Inno.UI.UiMeshUpdate 08340e9b7e7204dea5fd303ba8a9b84aac0013e85743bf2d5f0aa904d4e17a10 -->
<!-- api: P:Inno.UI.UiMeshUpdate.mesh ef7ef207c5bcd2278eb1ef1e41b03ad484794c2fe8e3716d3e353a9bf52c1adb -->
<!-- api: P:Inno.UI.UiMeshUpdate.revision 83f18216b40c4dafb7f5b3499d16f58a255dfd01ab2f92cda02ffbc3bf749131 -->
<!-- api: P:Inno.UI.UiMeshUpdate.vertices f921a3438fd12434f4cee42e62a2f5be97ac7294a48802eeec5f1a2c94f9febf -->
<!-- api: P:Inno.UI.UiMeshUpdate.indices 41dd8bc9045d00fba738457d8faa8f5eaa7765da56c2bd97725a56b089a7dd68 -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-88856152b18fc320"></a>[`public sealed class Inno.UI.UiMeshUpdate`](../../src/services/ui/Inno.UI/UiRendering.cs#L58) | Publishes one immutable mesh generation to a rendering plugin. | — |
| <a id="api-28cfbbfa223bc23b"></a>[`public Inno.UI.UiMeshHandle Inno.UI.UiMeshUpdate.mesh { get; }`](../../src/services/ui/Inno.UI/UiRendering.cs#L78) | Gets the generation-scoped mesh handle. | — |
| <a id="api-07bb41eb0d7ae767"></a>[`public ulong Inno.UI.UiMeshUpdate.revision { get; }`](../../src/services/ui/Inno.UI/UiRendering.cs#L82) | Gets the monotonically increasing mesh content revision. | — |
| <a id="api-afe65db2abdf85ae"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.UI.UiVertex> Inno.UI.UiMeshUpdate.vertices { get; }`](../../src/services/ui/Inno.UI/UiRendering.cs#L86) | Gets immutable local-space vertices. | — |
| <a id="api-3410827e0af95452"></a>[`public System.Collections.Generic.IReadOnlyList<uint> Inno.UI.UiMeshUpdate.indices { get; }`](../../src/services/ui/Inno.UI/UiRendering.cs#L90) | Gets immutable triangle indices. | — |

### `Inno.UI.UiTextureUpdate`

<!-- api: T:Inno.UI.UiTextureUpdate 1f5abdc77181b34fa5f82f0fc16756d392c423f71df52769fb897425baf5f2ab -->
<!-- api: P:Inno.UI.UiTextureUpdate.image 170fba81a48d9ddbcacc9cb59c04c1310e626db1ab3c747080df0008e9e7f2c7 -->
<!-- api: P:Inno.UI.UiTextureUpdate.texture 77b8b5ca285deee38c3e9c4d731d10f69db3cebac39ab2fc898830a45cac74c3 -->
<!-- api: P:Inno.UI.UiTextureUpdate.revision 45a0e8ddc515f54b7912791d8f16df9e7f32d891f0523cdb9646ede1f00eacdc -->
<!-- api: P:Inno.UI.UiTextureUpdate.width 185d133b5b50c25a1e5abc4b97848ec3e9fd3b82dd4c3a0a310a240ae78c681d -->
<!-- api: P:Inno.UI.UiTextureUpdate.height c5bbe7c79aaf97bb4aca6856c8e141508f704b31a4de33375fd8dd20f1793977 -->
<!-- api: P:Inno.UI.UiTextureUpdate.pixels ea993536fedfc27aa7a36146e751c8c98966e909b943dcea2ba95758d1f09c4b -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-1022bf3ae548f8a5"></a>[`public sealed class Inno.UI.UiTextureUpdate`](../../src/services/ui/Inno.UI/UiRendering.cs#L96) | Publishes one immutable pixel texture or imported image reference. | — |
| <a id="api-de27cf241c91698c"></a>[`public Inno.UI.UiImageSource? Inno.UI.UiTextureUpdate.image { get; }`](../../src/services/ui/Inno.UI/UiRendering.cs#L130) | Gets the imported image reference, or null for backend-generated premultiplied pixels. | — |
| <a id="api-02bf8ec55b8f0499"></a>[`public Inno.UI.UiTextureHandle Inno.UI.UiTextureUpdate.texture { get; }`](../../src/services/ui/Inno.UI/UiRendering.cs#L135) | Gets the generation-scoped texture handle. | — |
| <a id="api-36e2e1bb95fe61f8"></a>[`public ulong Inno.UI.UiTextureUpdate.revision { get; }`](../../src/services/ui/Inno.UI/UiRendering.cs#L139) | Gets the monotonically increasing content revision. | — |
| <a id="api-47ca80bf92f8e7f0"></a>[`public int Inno.UI.UiTextureUpdate.width { get; }`](../../src/services/ui/Inno.UI/UiRendering.cs#L143) | Gets the texture width. | — |
| <a id="api-b6a3ff734d3db9c3"></a>[`public int Inno.UI.UiTextureUpdate.height { get; }`](../../src/services/ui/Inno.UI/UiRendering.cs#L147) | Gets the texture height. | — |
| <a id="api-e9f3004a8c2e7265"></a>[`public System.ReadOnlyMemory<byte> Inno.UI.UiTextureUpdate.pixels { get; }`](../../src/services/ui/Inno.UI/UiRendering.cs#L151) | Gets tightly packed premultiplied RGBA8 pixels, or empty memory when image is assigned. | — |

### `Inno.UI.UiDrawCommand`

<!-- api: T:Inno.UI.UiDrawCommand 44e0917dd86963fc0780fe72a9aaa908e1a1fca9a5c7baaae048becc1d75afc6 -->
<!-- api: M:Inno.UI.UiDrawCommand.#ctor(Inno.UI.UiMeshHandle,Inno.UI.UiTextureHandle,System.Boolean,Inno.UI.UiClipRectangle) 143ad1fffaf2f369fedd27aa538910744ae11f15c08dba030f01c54bf02fe3ad -->
<!-- api: P:Inno.UI.UiDrawCommand.mesh a1bbe2a284b7b9c02f214976aa77b962df90ac2f3b0ceab5bc704f16840f0efe -->
<!-- api: P:Inno.UI.UiDrawCommand.texture 5b55a63000876fbfe6b66ca842176ecacb987c60253a3b10dfc30b20c532fc8c -->
<!-- api: P:Inno.UI.UiDrawCommand.scissorEnabled 0c44dd960287368e28fd348edaaa7018b6bb9c07ea0ef9e36e246b929dacb249 -->
<!-- api: P:Inno.UI.UiDrawCommand.scissor 9b548f2c50691b537e6e8d0260bb58860b0c2b64b5027580fc09f580f7bf4533 -->
<!-- api: M:Inno.UI.UiDrawCommand.ToString 472fe5a5b713505b8dee713f2d83c4d355c04dbf144df8c9d371891df708d4bf -->
<!-- api: M:Inno.UI.UiDrawCommand.op_Inequality(Inno.UI.UiDrawCommand,Inno.UI.UiDrawCommand) 285354e9418fffec6c8a25c916e7d59646c29cd2d50348b60d98d97f5db2af04 -->
<!-- api: M:Inno.UI.UiDrawCommand.op_Equality(Inno.UI.UiDrawCommand,Inno.UI.UiDrawCommand) 76a722ddbc8b287932bd9b65f61affcb98d5418382808dec4ab4a18fc3738fb2 -->
<!-- api: M:Inno.UI.UiDrawCommand.GetHashCode ea9c25c89d2dc5d9f33d56785370361a28a971fe9fedc5b4ff2a8cfc02439642 -->
<!-- api: M:Inno.UI.UiDrawCommand.Equals(System.Object) 4152fc29f4f11646dbb9db120a336630bf70d73fe1f9bb29e2734567700cdc0e -->
<!-- api: M:Inno.UI.UiDrawCommand.Equals(Inno.UI.UiDrawCommand) 3a98392cb1be2138b1b067f178a5464bc5668805bc9318b242062145295ae7b7 -->
<!-- api: M:Inno.UI.UiDrawCommand.Deconstruct(Inno.UI.UiMeshHandle@,Inno.UI.UiTextureHandle@,System.Boolean@,Inno.UI.UiClipRectangle@) 27ac9f02f7c03805833eb936ff98476d4b6064cca250a3e3d50ca8f57e8947de -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-0d3a9bb5d82163bb"></a>[`public readonly record struct Inno.UI.UiDrawCommand : System.IEquatable<Inno.UI.UiDrawCommand>`](../../src/services/ui/Inno.UI/UiRendering.cs#L169) | Describes one draw of a previously published mesh.<br>`mesh`：The ui mesh handle value used to initialize this instance.<br>`texture`：The ui texture handle value used to initialize this instance.<br>`scissorEnabled`：The bool value used to initialize this instance.<br>`scissor`：The ui clip rectangle value used to initialize this instance. | — |
| <a id="api-b7b21ff97e1d875a"></a>[`public Inno.UI.UiDrawCommand.UiDrawCommand(Inno.UI.UiMeshHandle mesh, Inno.UI.UiTextureHandle texture, bool scissorEnabled, Inno.UI.UiClipRectangle scissor)`](../../src/services/ui/Inno.UI/UiRendering.cs#L169) | Describes one draw of a previously published mesh.<br>`mesh`：The ui mesh handle value used to initialize this instance.<br>`texture`：The ui texture handle value used to initialize this instance.<br>`scissorEnabled`：The bool value used to initialize this instance.<br>`scissor`：The ui clip rectangle value used to initialize this instance. | — |
| <a id="api-2d59e30749fe50a5"></a>[`public Inno.UI.UiMeshHandle Inno.UI.UiDrawCommand.mesh { get; init; }`](../../src/services/ui/Inno.UI/UiRendering.cs#L170) | The ui mesh handle value used to initialize this instance. | — |
| <a id="api-8fbb69b2bc55ec4d"></a>[`public Inno.UI.UiTextureHandle Inno.UI.UiDrawCommand.texture { get; init; }`](../../src/services/ui/Inno.UI/UiRendering.cs#L171) | The ui texture handle value used to initialize this instance. | — |
| <a id="api-09f8dd1108e2a08c"></a>[`public bool Inno.UI.UiDrawCommand.scissorEnabled { get; init; }`](../../src/services/ui/Inno.UI/UiRendering.cs#L172) | The bool value used to initialize this instance. | — |
| <a id="api-be5ce9457dddb532"></a>[`public Inno.UI.UiClipRectangle Inno.UI.UiDrawCommand.scissor { get; init; }`](../../src/services/ui/Inno.UI/UiRendering.cs#L173) | The ui clip rectangle value used to initialize this instance. | — |
| <a id="api-32b71d92dd517ac7"></a>[`public override string Inno.UI.UiDrawCommand.ToString()`](../../src/services/ui/Inno.UI/UiRendering.cs#L169) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — |
| <a id="api-de0998c4b0a91966"></a>[`public static bool Inno.UI.UiDrawCommand.operator !=(Inno.UI.UiDrawCommand left, Inno.UI.UiDrawCommand right)`](../../src/services/ui/Inno.UI/UiRendering.cs#L169) | 编译器生成的值不等比较。 | — |
| <a id="api-dfaeba2919b3f991"></a>[`public static bool Inno.UI.UiDrawCommand.operator ==(Inno.UI.UiDrawCommand left, Inno.UI.UiDrawCommand right)`](../../src/services/ui/Inno.UI/UiRendering.cs#L169) | 编译器生成的记录值相等比较。 | — |
| <a id="api-7d5607d6ac8b78bd"></a>[`public override int Inno.UI.UiDrawCommand.GetHashCode()`](../../src/services/ui/Inno.UI/UiRendering.cs#L169) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-703326f0a9734422"></a>[`public override bool Inno.UI.UiDrawCommand.Equals(object obj)`](../../src/services/ui/Inno.UI/UiRendering.cs#L169) | Indicates whether this instance and a specified object are equal.<br>`obj`：The object to compare with the current instance.<br>返回：`true` if `obj` and this instance are the same type and represent the same value; otherwise, `false`. | — |
| <a id="api-c6c9197212bd8c45"></a>[`public bool Inno.UI.UiDrawCommand.Equals(Inno.UI.UiDrawCommand other)`](../../src/services/ui/Inno.UI/UiRendering.cs#L169) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |
| <a id="api-4fbb8a6035a8fd50"></a>[`public void Inno.UI.UiDrawCommand.Deconstruct(out Inno.UI.UiMeshHandle mesh, out Inno.UI.UiTextureHandle texture, out bool scissorEnabled, out Inno.UI.UiClipRectangle scissor)`](../../src/services/ui/Inno.UI/UiRendering.cs#L169) | 按声明顺序读取记录成员。 | — |

### `Inno.UI.UiRenderFrame`

<!-- api: T:Inno.UI.UiRenderFrame b046744cf205329cbbad7f8b7400717b46a62cd73d1af1400a82051549a22241 -->
<!-- api: P:Inno.UI.UiRenderFrame.empty 1368e526633cb437f48772be925dbb7c5229992c64ca07bbf8e031a4c085de1c -->
<!-- api: M:Inno.UI.UiRenderFrame.CreateRetirement(Inno.UI.UiMeshHandle[],Inno.UI.UiTextureHandle[]) a71eda0505b5d9f6ba724cbc7235a74aee19a5fad80b670a1ee969efb7106379 -->
<!-- api: P:Inno.UI.UiRenderFrame.meshUpdates 9a16b2e389602a8c1dd82e5adeaa5310447b5f5c931754f9e32baa4a6adcc2a1 -->
<!-- api: P:Inno.UI.UiRenderFrame.releasedMeshes 6bdabd9b1b2cfb469e58bf0b01f1ad849a189661a6e084390690aaddb8fea05a -->
<!-- api: P:Inno.UI.UiRenderFrame.textureUpdates b5ec382c5d6d536fcbfd5aa431804dada9b16d4b52968023fd1a43f294e64074 -->
<!-- api: P:Inno.UI.UiRenderFrame.releasedTextures bfb1655d236df639ab19004a4f9ad075710c01279814339d022a9b5fd5670a73 -->
<!-- api: P:Inno.UI.UiRenderFrame.commands ee00c512ca79b608101edff3944d03f31920521442d1e6bc25e669be04eafeed -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-c04560e6d35bf7f6"></a>[`public sealed class Inno.UI.UiRenderFrame`](../../src/services/ui/Inno.UI/UiRendering.cs#L179) | Contains incremental resource changes and ordered draw commands for one UI context. | — |
| <a id="api-2d9545f40b39bebf"></a>[`public static Inno.UI.UiRenderFrame Inno.UI.UiRenderFrame.empty { get; }`](../../src/services/ui/Inno.UI/UiRendering.cs#L204) | Gets an empty resource-delta frame. | — |
| <a id="api-4741d9f94f7056fd"></a>[`public static Inno.UI.UiRenderFrame Inno.UI.UiRenderFrame.CreateRetirement(Inno.UI.UiMeshHandle[] meshes, Inno.UI.UiTextureHandle[] textures)`](../../src/services/ui/Inno.UI/UiRendering.cs#L218) | Creates a command-free frame that deterministically retires plugin-owned resources.<br>`meshes`：Assigned mesh handles to retire.<br>`textures`：Assigned texture handles to retire.<br>返回：An immutable retirement frame. | — |
| <a id="api-b483c7cd06a74303"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.UI.UiMeshUpdate> Inno.UI.UiRenderFrame.meshUpdates { get; }`](../../src/services/ui/Inno.UI/UiRendering.cs#L233) | Gets created or replaced mesh generations. | — |
| <a id="api-54c6768b2b3e5bbd"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.UI.UiMeshHandle> Inno.UI.UiRenderFrame.releasedMeshes { get; }`](../../src/services/ui/Inno.UI/UiRendering.cs#L237) | Gets mesh generations that must be retired. | — |
| <a id="api-9d5e0b8b7d15e249"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.UI.UiTextureUpdate> Inno.UI.UiRenderFrame.textureUpdates { get; }`](../../src/services/ui/Inno.UI/UiRendering.cs#L241) | Gets created or replaced texture generations. | — |
| <a id="api-dabf10668ec5a9d3"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.UI.UiTextureHandle> Inno.UI.UiRenderFrame.releasedTextures { get; }`](../../src/services/ui/Inno.UI/UiRendering.cs#L245) | Gets texture generations that must be retired. | — |
| <a id="api-a762df9fd3356d7d"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.UI.UiDrawCommand> Inno.UI.UiRenderFrame.commands { get; }`](../../src/services/ui/Inno.UI/UiRendering.cs#L249) | Gets ordered draws referencing published resources. | — |

### `Inno.UI.UiRenderFrameBuilder`

<!-- api: T:Inno.UI.UiRenderFrameBuilder b165510a9ce6d6a4db7d5a89f09d961a44370bbea4f6440f71f101a29decc841 -->
<!-- api: M:Inno.UI.UiRenderFrameBuilder.AddMeshUpdate(Inno.UI.UiMeshHandle,System.UInt64,Inno.UI.UiVertex[],System.UInt32[]) 7899b3af9877a5c1085b7ec0d6859755ed8df52e3326029d7561545b734f99b1 -->
<!-- api: M:Inno.UI.UiRenderFrameBuilder.AddReleasedMesh(Inno.UI.UiMeshHandle) 1bcfabb5acd536bfd91c7660d0ff4a8156ae3ea41b4cb0dca1e48493a68bceb7 -->
<!-- api: M:Inno.UI.UiRenderFrameBuilder.AddTextureUpdate(Inno.UI.UiTextureHandle,System.UInt64,System.Int32,System.Int32,System.Byte[]) c2875621f6f9acda426b793b55b1e40044afa02a12e81031a44a0f8d89b3a9db -->
<!-- api: M:Inno.UI.UiRenderFrameBuilder.AddReleasedTexture(Inno.UI.UiTextureHandle) 70b47da5e68c98f1438c98f1f1b0493a048df5727c01d767de28a72bd10d6de8 -->
<!-- api: M:Inno.UI.UiRenderFrameBuilder.AddCommand(Inno.UI.UiDrawCommand) 96e9299ea4ab0395b988cfd844777d3c27563a3d96c4ac0aa2f87f012ce046aa -->
<!-- api: M:Inno.UI.UiRenderFrameBuilder.Build cf39a7ca24a3e7925279bf469143ce728d67b367d43befce36e7bf6a4022002d -->
<!-- api: M:Inno.UI.UiRenderFrameBuilder.AddImageUpdate(Inno.UI.UiTextureHandle,System.UInt64,Inno.UI.UiImageSource) 81976b45cb91cd1964aea01d849f5cfdfcd8900ebd77fa9c1fe7643e2696ad0d -->
<!-- api: M:Inno.UI.UiRenderFrameBuilder.#ctor 3df4d1a786af259c14a477beba3f230ddf122befa2a09ad2fad38f3d3618dd3d -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-f4ac5b2e2b699df2"></a>[`public sealed class Inno.UI.UiRenderFrameBuilder`](../../src/services/ui/Inno.UI/UiRendering.cs#L258) | Builds one immutable backend-neutral frame while transferring ownership of update arrays. | — |
| <a id="api-544518aedea2597b"></a>[`public void Inno.UI.UiRenderFrameBuilder.AddMeshUpdate(Inno.UI.UiMeshHandle mesh, ulong revision, Inno.UI.UiVertex[] vertices, uint[] indices)`](../../src/services/ui/Inno.UI/UiRendering.cs#L282) | Adds one mesh update and takes exclusive ownership of both arrays.<br>`mesh`：Assigned mesh handle.<br>`revision`：Positive content revision.<br>`vertices`：Owned vertex array; the caller must not mutate it afterward.<br>`indices`：Owned index array; the caller must not mutate it afterward. | — |
| <a id="api-19cd0d5424d810b2"></a>[`public void Inno.UI.UiRenderFrameBuilder.AddReleasedMesh(Inno.UI.UiMeshHandle mesh)`](../../src/services/ui/Inno.UI/UiRendering.cs#L309) | Adds one mesh retirement.<br>`mesh`：Assigned mesh handle. | — |
| <a id="api-234f406318729212"></a>[`public void Inno.UI.UiRenderFrameBuilder.AddTextureUpdate(Inno.UI.UiTextureHandle texture, ulong revision, int width, int height, byte[] pixels)`](../../src/services/ui/Inno.UI/UiRendering.cs#L335) | Adds one texture update and takes exclusive ownership of its pixel array.<br>`texture`：Assigned texture handle.<br>`revision`：Positive content revision.<br>`width`：Positive pixel width.<br>`height`：Positive pixel height.<br>`pixels`：Owned tightly packed premultiplied RGBA8 pixels. | — |
| <a id="api-08245c74b32339a0"></a>[`public void Inno.UI.UiRenderFrameBuilder.AddReleasedTexture(Inno.UI.UiTextureHandle texture)`](../../src/services/ui/Inno.UI/UiRendering.cs#L361) | Adds one texture retirement.<br>`texture`：Assigned texture handle. | — |
| <a id="api-17fd6de911683c90"></a>[`public void Inno.UI.UiRenderFrameBuilder.AddCommand(Inno.UI.UiDrawCommand command)`](../../src/services/ui/Inno.UI/UiRendering.cs#L375) | Adds one ordered mesh draw.<br>`command`：Draw command referencing an assigned mesh. | — |
| <a id="api-887ed96f5417d5e6"></a>[`public Inno.UI.UiRenderFrame Inno.UI.UiRenderFrameBuilder.Build()`](../../src/services/ui/Inno.UI/UiRendering.cs#L389) | Freezes the accumulated frame. The builder cannot be reused.<br>返回：An immutable frame owning every supplied array. | — |
| <a id="api-bbf8de65028d71e6"></a>[`public void Inno.UI.UiRenderFrameBuilder.AddImageUpdate(Inno.UI.UiTextureHandle texture, ulong revision, Inno.UI.UiImageSource image)`](../../src/services/ui/Inno.UI/UiRendering.cs#L422) | Adds an imported image reference without copying pixels or creating GPU resources.<br>`texture`：Assigned UI texture handle.<br>`revision`：Positive UI publication revision.<br>`image`：Frozen imported artifact and dimensions. | `System.ArgumentException`：The texture handle is unassigned.<br>`System.ArgumentOutOfRangeException`：The revision is zero.<br>`System.ArgumentNullException`：The image is null. |
| <a id="api-30dc5b27e71ac14b"></a>[`public Inno.UI.UiRenderFrameBuilder.UiRenderFrameBuilder()`](../../src/services/ui/Inno.UI/UiRendering.cs#L258) | 编译器生成的记录构造入口，参数对应左列声明的成员。 | — |

### `Inno.UI.UiContextOptions`

<!-- api: T:Inno.UI.UiContextOptions 5f3bd5c15024a2503e142b192b19bec9c01154434d2a9ff40b14d92f5b755d8f -->
<!-- api: M:Inno.UI.UiContextOptions.#ctor(System.String,System.Int32,System.Int32,System.Single) c8e2692b6477f486598075654b1df52031e5e408da7641023671705eeb0f5671 -->
<!-- api: P:Inno.UI.UiContextOptions.name 545602e1631d931e4e9b1bcd239384278a3ba7449d0390af8eecb013dcf00b16 -->
<!-- api: P:Inno.UI.UiContextOptions.width b712e4b21d864580c514ecafb011a8f1203afa0d56495d706ac28ad96ae6a419 -->
<!-- api: P:Inno.UI.UiContextOptions.height 9acbe1dabce94613241c1bc1bedaed7bd985ddda4a6641f5fc6c19e4ad195926 -->
<!-- api: P:Inno.UI.UiContextOptions.density fcca87f6f895c569ef5662ee125b15f438dc7b6089e9b1efb7b31a7527322a8b -->
<!-- api: M:Inno.UI.UiContextOptions.ToString 94c73f79067cc659cb0834028e0d83a8290557989d7d6391a66540079550fd3c -->
<!-- api: M:Inno.UI.UiContextOptions.op_Inequality(Inno.UI.UiContextOptions,Inno.UI.UiContextOptions) 723449ec7768abfa5b01ab00604410c9695b00adde7c0d813da752815a952f97 -->
<!-- api: M:Inno.UI.UiContextOptions.op_Equality(Inno.UI.UiContextOptions,Inno.UI.UiContextOptions) cf6c6f71e177cd6f70e8a379182d0bc276481b5ad7bad35f0362db24336ba537 -->
<!-- api: M:Inno.UI.UiContextOptions.GetHashCode 55a02d39855ae2ad1bf2b879cf6dbffa100cb24aa8e6848143761d38bf0ab645 -->
<!-- api: M:Inno.UI.UiContextOptions.Equals(System.Object) 86deeb805f3862b983d3007c3a42d3fd4d47adbb00b242c17e0fde065f2edf6a -->
<!-- api: M:Inno.UI.UiContextOptions.Equals(Inno.UI.UiContextOptions) a2df9fff7e1a76c5a73873258eda1e44b15d1698533418b0fedc5ece47794391 -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-78676f2e0ae8a192"></a>[`public readonly record struct Inno.UI.UiContextOptions : System.IEquatable<Inno.UI.UiContextOptions>`](../../src/services/ui/Inno.UI/UiTypes.cs#L13) | Defines one independent UI layout and interaction surface. | — |
| <a id="api-a57a79673c9a577e"></a>[`public Inno.UI.UiContextOptions.UiContextOptions(string name, int width, int height, float density = 1)`](../../src/services/ui/Inno.UI/UiTypes.cs#L30) | Creates validated context options.<br>`name`：A non-empty diagnostic context name.<br>`width`：The positive initial width in pixels.<br>`height`：The positive initial height in pixels.<br>`density`：The positive density-independent pixel ratio. | — |
| <a id="api-c144d7424625b040"></a>[`public string Inno.UI.UiContextOptions.name { get; }`](../../src/services/ui/Inno.UI/UiTypes.cs#L50) | Gets the diagnostic context name. | — |
| <a id="api-5d26a320a645d3fa"></a>[`public int Inno.UI.UiContextOptions.width { get; }`](../../src/services/ui/Inno.UI/UiTypes.cs#L54) | Gets the initial width in pixels. | — |
| <a id="api-00d6f0e48a46c6ec"></a>[`public int Inno.UI.UiContextOptions.height { get; }`](../../src/services/ui/Inno.UI/UiTypes.cs#L58) | Gets the initial height in pixels. | — |
| <a id="api-88f0b58ca8ab55a8"></a>[`public float Inno.UI.UiContextOptions.density { get; }`](../../src/services/ui/Inno.UI/UiTypes.cs#L62) | Gets the density-independent pixel ratio. | — |
| <a id="api-a462013183daf651"></a>[`public override string Inno.UI.UiContextOptions.ToString()`](../../src/services/ui/Inno.UI/UiTypes.cs#L13) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — |
| <a id="api-f989c2beba7a4f0e"></a>[`public static bool Inno.UI.UiContextOptions.operator !=(Inno.UI.UiContextOptions left, Inno.UI.UiContextOptions right)`](../../src/services/ui/Inno.UI/UiTypes.cs#L13) | 编译器生成的值不等比较。 | — |
| <a id="api-305fe364cdf3bb5a"></a>[`public static bool Inno.UI.UiContextOptions.operator ==(Inno.UI.UiContextOptions left, Inno.UI.UiContextOptions right)`](../../src/services/ui/Inno.UI/UiTypes.cs#L13) | 编译器生成的记录值相等比较。 | — |
| <a id="api-029334aad3fc532f"></a>[`public override int Inno.UI.UiContextOptions.GetHashCode()`](../../src/services/ui/Inno.UI/UiTypes.cs#L13) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-82fb5916aefcc0d6"></a>[`public override bool Inno.UI.UiContextOptions.Equals(object obj)`](../../src/services/ui/Inno.UI/UiTypes.cs#L13) | Indicates whether this instance and a specified object are equal.<br>`obj`：The object to compare with the current instance.<br>返回：`true` if `obj` and this instance are the same type and represent the same value; otherwise, `false`. | — |
| <a id="api-7c19405cb3973a56"></a>[`public bool Inno.UI.UiContextOptions.Equals(Inno.UI.UiContextOptions other)`](../../src/services/ui/Inno.UI/UiTypes.cs#L13) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |

### `Inno.UI.UiDocumentSource`

<!-- api: T:Inno.UI.UiDocumentSource 99d39866a60ce4bfe3326790aa7fcbc47814269569e7e6ecc30af4f831109a81 -->
<!-- api: M:Inno.UI.UiDocumentSource.#ctor(Inno.UI.UiDocumentLanguageId,System.String,System.String,System.Collections.Generic.IReadOnlyDictionary{System.String,System.String}) c097915597dbdf9602069057575970c6a561154b35cd0d084035d674688a3c36 -->
<!-- api: P:Inno.UI.UiDocumentSource.language 0ff995fd77f080588f3456954bb33b652adb52601b36637c579c1ec38d134143 -->
<!-- api: P:Inno.UI.UiDocumentSource.text 3a398d811f40dd40d2abef852d243cb57f624570cb5cb834423175ecc57e9ba6 -->
<!-- api: P:Inno.UI.UiDocumentSource.sourceUri 1e8a8a76bdca9b654a08f8616feb3a29cca481bf4d4b7c33099e0f13bf2c93f4 -->
<!-- api: P:Inno.UI.UiDocumentSource.resourceBindings 4196b48865443962d5adfc01a0fddf4e5f1dcb53548cfdd26915be2378a9c048 -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-13cd221296c74a30"></a>[`public sealed class Inno.UI.UiDocumentSource`](../../src/services/ui/Inno.UI/UiTypes.cs#L68) | Contains immutable text in one explicitly selected UI document language. | — |
| <a id="api-e1c7d8675b4524b4"></a>[`public Inno.UI.UiDocumentSource.UiDocumentSource(Inno.UI.UiDocumentLanguageId language, string text, string sourceUri = "memory://ui-document", System.Collections.Generic.IReadOnlyDictionary<string, string>? resourceBindings = null)`](../../src/services/ui/Inno.UI/UiTypes.cs#L85) | Creates a validated in-memory document source.<br>`language`：Explicit source language understood by a registered backend.<br>`text`：Complete immutable document text.<br>`sourceUri`：Virtual source address used only for diagnostics and relative dependency identity.<br>`resourceBindings`：Optional frontend resource names mapped to registered runtime resource names; captured defensively. | — |
| <a id="api-22713592c992652d"></a>[`public Inno.UI.UiDocumentLanguageId Inno.UI.UiDocumentSource.language { get; }`](../../src/services/ui/Inno.UI/UiTypes.cs#L112) | Gets the explicitly selected source language. | — |
| <a id="api-4f8a6146ee5dc7a5"></a>[`public string Inno.UI.UiDocumentSource.text { get; }`](../../src/services/ui/Inno.UI/UiTypes.cs#L116) | Gets the complete immutable source text. | — |
| <a id="api-1ac3b66f349e00d4"></a>[`public string Inno.UI.UiDocumentSource.sourceUri { get; }`](../../src/services/ui/Inno.UI/UiTypes.cs#L120) | Gets the virtual source address. | — |
| <a id="api-eadb31dd80714b65"></a>[`public System.Collections.Generic.IReadOnlyDictionary<string, string> Inno.UI.UiDocumentSource.resourceBindings { get; }`](../../src/services/ui/Inno.UI/UiTypes.cs#L125) | Gets immutable runtime-only source bindings interpreted by the selected language backend. | — |

### `Inno.UI.UiDocumentFragment`

<!-- api: T:Inno.UI.UiDocumentFragment c22ce03bc1dbf915cab29f8cce46c70ec068455e9d3b1fc384cda607803794d2 -->
<!-- api: M:Inno.UI.UiDocumentFragment.#ctor(Inno.UI.UiDocumentLanguageId,System.String) 5448e528367bbb8a18f6c4e403fae39052c6c58e1b8fdfe694a0270d2b9f0fb0 -->
<!-- api: P:Inno.UI.UiDocumentFragment.language af86d1d233707cdfef5f21dffbc025ddb0d884831c394276f38c6e4ee0a0e863 -->
<!-- api: P:Inno.UI.UiDocumentFragment.text 901a6f214e2c27ae7bc59658c90e4601429c9f645e51bf50c9838bee537969f0 -->
<!-- api: M:Inno.UI.UiDocumentFragment.ToString 158183ec683d04b6448ab7338690ba8fbc35e4828faf8a6068dd013d9201e0ec -->
<!-- api: M:Inno.UI.UiDocumentFragment.op_Inequality(Inno.UI.UiDocumentFragment,Inno.UI.UiDocumentFragment) ca0c67aef1ddb5e7ea8b6e2467fbc39b3c7ffb8d11b78db6dc638d8f0549e759 -->
<!-- api: M:Inno.UI.UiDocumentFragment.op_Equality(Inno.UI.UiDocumentFragment,Inno.UI.UiDocumentFragment) 0af0bdde7dad18b7bf06c7fe229def39448f42301bc73cddcfbb24a4bdc9fad1 -->
<!-- api: M:Inno.UI.UiDocumentFragment.GetHashCode 8d004a48f08c35da5935b1f82827f83b4a20e99d71b36d401c7742b4ccd23105 -->
<!-- api: M:Inno.UI.UiDocumentFragment.Equals(System.Object) ebe1f543b3d9d5731873563dffa1d8009b61a4a31ba3ee4290c1a889033cde85 -->
<!-- api: M:Inno.UI.UiDocumentFragment.Equals(Inno.UI.UiDocumentFragment) 4f17a5495c6718d7e130a5707c0895100a187d3ea16ef3816775a97eb853d2bb -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-b0b1504a7118f5fc"></a>[`public readonly record struct Inno.UI.UiDocumentFragment : System.IEquatable<Inno.UI.UiDocumentFragment>`](../../src/services/ui/Inno.UI/UiTypes.cs#L131) | Contains a language-tagged fragment for a backend-specific document mutation. | — |
| <a id="api-5f35b3b1872eb444"></a>[`public Inno.UI.UiDocumentFragment.UiDocumentFragment(Inno.UI.UiDocumentLanguageId language, string text)`](../../src/services/ui/Inno.UI/UiTypes.cs#L142) | Creates a validated source fragment.<br>`language`：Explicit fragment language.<br>`text`：Complete fragment text. | — |
| <a id="api-14e4cd760b9002e8"></a>[`public Inno.UI.UiDocumentLanguageId Inno.UI.UiDocumentFragment.language { get; }`](../../src/services/ui/Inno.UI/UiTypes.cs#L156) | Gets the explicitly selected fragment language. | — |
| <a id="api-22f4706ec5fda69e"></a>[`public string Inno.UI.UiDocumentFragment.text { get; }`](../../src/services/ui/Inno.UI/UiTypes.cs#L160) | Gets the complete fragment text. | — |
| <a id="api-98ea156f7b4c504e"></a>[`public override string Inno.UI.UiDocumentFragment.ToString()`](../../src/services/ui/Inno.UI/UiTypes.cs#L131) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — |
| <a id="api-7c64095745762ab6"></a>[`public static bool Inno.UI.UiDocumentFragment.operator !=(Inno.UI.UiDocumentFragment left, Inno.UI.UiDocumentFragment right)`](../../src/services/ui/Inno.UI/UiTypes.cs#L131) | 编译器生成的值不等比较。 | — |
| <a id="api-7463dd86466aadae"></a>[`public static bool Inno.UI.UiDocumentFragment.operator ==(Inno.UI.UiDocumentFragment left, Inno.UI.UiDocumentFragment right)`](../../src/services/ui/Inno.UI/UiTypes.cs#L131) | 编译器生成的记录值相等比较。 | — |
| <a id="api-27f204c19a6c0e2a"></a>[`public override int Inno.UI.UiDocumentFragment.GetHashCode()`](../../src/services/ui/Inno.UI/UiTypes.cs#L131) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-0fa2ea401044294c"></a>[`public override bool Inno.UI.UiDocumentFragment.Equals(object obj)`](../../src/services/ui/Inno.UI/UiTypes.cs#L131) | Indicates whether this instance and a specified object are equal.<br>`obj`：The object to compare with the current instance.<br>返回：`true` if `obj` and this instance are the same type and represent the same value; otherwise, `false`. | — |
| <a id="api-40bac34594693a56"></a>[`public bool Inno.UI.UiDocumentFragment.Equals(Inno.UI.UiDocumentFragment other)`](../../src/services/ui/Inno.UI/UiTypes.cs#L131) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |

### `Inno.UI.UiTextureData`

<!-- api: T:Inno.UI.UiTextureData 52d0ecff47c81233773b05d441251d386b95fc64ced3e309ef740fa7d6dd998e -->
<!-- api: M:Inno.UI.UiTextureData.#ctor(System.Int32,System.Int32,System.ReadOnlySpan{System.Byte}) 79d6f8e81f6275c6f79c321fd8e191aba96039ac1c5d2ba55ba42ae78ba3f8b6 -->
<!-- api: P:Inno.UI.UiTextureData.width 721d10bb326053288d3a282fd3c136f7d31c6bf86fb44a9c28f8aea09800525a -->
<!-- api: P:Inno.UI.UiTextureData.height 8d41989e55c1347084f815ee9fa69a288645ba43f2f3afc3c940c551d6ce7f8a -->
<!-- api: P:Inno.UI.UiTextureData.pixels 238a636af1f5f780da1d16730be5867ea49c82a63e13fdd622f399d040416cd8 -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-cd4a6e52056aa20d"></a>[`public sealed class Inno.UI.UiTextureData`](../../src/services/ui/Inno.UI/UiTypes.cs#L166) | Contains immutable RGBA8 texture pixels addressable by a document source name. | — |
| <a id="api-bd7d8e3d245c609f"></a>[`public Inno.UI.UiTextureData.UiTextureData(int width, int height, System.ReadOnlySpan<byte> pixels)`](../../src/services/ui/Inno.UI/UiTypes.cs#L182) | Creates a validated immutable texture.<br>`width`：The positive texture width.<br>`height`：The positive texture height.<br>`pixels`：Tightly packed RGBA8 pixels. | — |
| <a id="api-33376922350a4b8f"></a>[`public int Inno.UI.UiTextureData.width { get; }`](../../src/services/ui/Inno.UI/UiTypes.cs#L199) | Gets the texture width. | — |
| <a id="api-a84e06ffc5c6bcb9"></a>[`public int Inno.UI.UiTextureData.height { get; }`](../../src/services/ui/Inno.UI/UiTypes.cs#L203) | Gets the texture height. | — |
| <a id="api-119c37489f380b01"></a>[`public System.ReadOnlyMemory<byte> Inno.UI.UiTextureData.pixels { get; }`](../../src/services/ui/Inno.UI/UiTypes.cs#L207) | Gets the immutable RGBA8 pixels. | — |

### `Inno.UI.UiEventType`

<!-- api: T:Inno.UI.UiEventType 6f3a6aaf239d98eb5027bf581aff9957fd624e80c39441c3ce09322c2bad3a52 -->
<!-- api: F:Inno.UI.UiEventType.Click 0f7391d03dba500b73c0694621bd0a16be5e99aff0972f2d707740b741ecb209 -->
<!-- api: F:Inno.UI.UiEventType.Change ca8a520aa80c34f0cb33d8af4144d3a3ff29d4e59323baabb20795b597a75bf0 -->
<!-- api: F:Inno.UI.UiEventType.Submit 932348136cd738a4b9497d88d8c2d08e601ef33560ca9ae8ccb76dbb5ad3fb0c -->
<!-- api: F:Inno.UI.UiEventType.Focus b0a18ec42475d4adf1eaec8afebd0a2bec27d9757a2b719faa1369c7726d016c -->
<!-- api: F:Inno.UI.UiEventType.Blur 5396e25fd1f00c8188f03ead86c95cf28d1d6226511575939f4c60e03336d8a5 -->
<!-- api: F:Inno.UI.UiEventType.MouseEnter b449a3ebb12af1d528e40b54915c109b16bce442338fdf2e3d41cdbc7700faa6 -->
<!-- api: F:Inno.UI.UiEventType.MouseLeave dbe80ab17c25b932fec2ee44f9507906f8527fc0e77a3be3a0a1ec600f5310c7 -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-391eb50fba323892"></a>[`public enum Inno.UI.UiEventType : int`](../../src/services/ui/Inno.UI/UiTypes.cs#L213) | Identifies one document event made visible to game code. | — |
| <a id="api-a98c7ebba74517eb"></a>[`Inno.UI.UiEventType.Click = 0`](../../src/services/ui/Inno.UI/UiTypes.cs#L218) | The target was clicked. | — |
| <a id="api-5549862ef8487b8a"></a>[`Inno.UI.UiEventType.Change = 1`](../../src/services/ui/Inno.UI/UiTypes.cs#L222) | The target value changed. | — |
| <a id="api-2eb514b87d1cae85"></a>[`Inno.UI.UiEventType.Submit = 2`](../../src/services/ui/Inno.UI/UiTypes.cs#L226) | A form was submitted. | — |
| <a id="api-6925a564d8f9b970"></a>[`Inno.UI.UiEventType.Focus = 3`](../../src/services/ui/Inno.UI/UiTypes.cs#L230) | The target gained focus. | — |
| <a id="api-c5646c6633f147c6"></a>[`Inno.UI.UiEventType.Blur = 4`](../../src/services/ui/Inno.UI/UiTypes.cs#L234) | The target lost focus. | — |
| <a id="api-d1493985ac7b9fef"></a>[`Inno.UI.UiEventType.MouseEnter = 5`](../../src/services/ui/Inno.UI/UiTypes.cs#L238) | A pointer entered the target. | — |
| <a id="api-5548ba9095b485ed"></a>[`Inno.UI.UiEventType.MouseLeave = 6`](../../src/services/ui/Inno.UI/UiTypes.cs#L242) | A pointer left the target. | — |

### `Inno.UI.UiEvent`

<!-- api: T:Inno.UI.UiEvent 0829b459c57246567cce4905861bc0564a53b899ab655782a9b065ef3c6aec0e -->
<!-- api: M:Inno.UI.UiEvent.#ctor(Inno.UI.UiEventType,Inno.UI.UiDocumentHandle,System.String,Inno.UI.UiControlValue,Inno.UI.UiEventPhase,Inno.UI.UiEventOrigin) 857e2b3724bca2f6a7b6c1ae85da39776e3cf9c6640a1f23c4e150844b3f7d2d -->
<!-- api: P:Inno.UI.UiEvent.type 81eeb008559ce6b61a177e7865c4a201850c334dafef7386d73e07f2e4063d71 -->
<!-- api: P:Inno.UI.UiEvent.value ca4ae0adcc2504cd9c590f6b9cf3862c3cc9e64d6c04ef5f80e972468d249227 -->
<!-- api: P:Inno.UI.UiEvent.phase 920e2ea2dc09135396d6e2d18c7adf3804fd25dde29b567744f6739ef173d3b2 -->
<!-- api: P:Inno.UI.UiEvent.origin bc775fe7b3872d19ed22108fa16a40de16ee566134ec928a63d8b8b362bc2b09 -->
<!-- api: P:Inno.UI.UiEvent.document 827d982baec2ec60d27122256cab4f94daf4fc3f70bbf0b3cc231d2a127ce9eb -->
<!-- api: P:Inno.UI.UiEvent.targetId c621927bc28ff1144f7203f7e657ec04203d28ddc2109c18f3f33d4825e4814f -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-ab2f4ff6598192b6"></a>[`public sealed class Inno.UI.UiEvent : Inno.Core.Events.Event`](../../src/services/ui/Inno.UI/UiTypes.cs#L248) | Describes one queued document event after a context update. | — |
| <a id="api-1073e04e847a8c7f"></a>[`public Inno.UI.UiEvent.UiEvent(Inno.UI.UiEventType type, Inno.UI.UiDocumentHandle document, string targetId, Inno.UI.UiControlValue value, Inno.UI.UiEventPhase phase, Inno.UI.UiEventOrigin origin)`](../../src/services/ui/Inno.UI/UiTypes.cs#L271) | Creates a document event with its source document and target element.<br>`type`：The kind of document interaction.<br>`document`：The document that produced the event.<br>`targetId`：The target element identifier, which can be empty.<br>`value`：The control value captured when this event occurred.<br>`phase`：The editing or commit phase of this event.<br>`origin`：The operation that caused the event. | — |
| <a id="api-a6012dc4a0c5d268"></a>[`public Inno.UI.UiEventType Inno.UI.UiEvent.type { get; }`](../../src/services/ui/Inno.UI/UiTypes.cs#L290) | Gets the kind of document interaction. | — |
| <a id="api-b037b66904ad98bd"></a>[`public Inno.UI.UiControlValue Inno.UI.UiEvent.value { get; }`](../../src/services/ui/Inno.UI/UiTypes.cs#L295) | Gets the immutable value at event time, not the current control value. | — |
| <a id="api-abad1e6cf9f62302"></a>[`public Inno.UI.UiEventPhase Inno.UI.UiEvent.phase { get; }`](../../src/services/ui/Inno.UI/UiTypes.cs#L300) | Gets the edit phase of this event. | — |
| <a id="api-ffdaa6b4abc6eb60"></a>[`public Inno.UI.UiEventOrigin Inno.UI.UiEvent.origin { get; }`](../../src/services/ui/Inno.UI/UiTypes.cs#L305) | Gets the cause of this event. | — |
| <a id="api-ce1c0ce79c3345e1"></a>[`public Inno.UI.UiDocumentHandle Inno.UI.UiEvent.document { get; }`](../../src/services/ui/Inno.UI/UiTypes.cs#L310) | Gets the source document. | — |
| <a id="api-3fa1238af92f36be"></a>[`public string Inno.UI.UiEvent.targetId { get; }`](../../src/services/ui/Inno.UI/UiTypes.cs#L315) | Gets the target element identifier. | — |

### `Inno.UI.UiInputSnapshot`

<!-- api: T:Inno.UI.UiInputSnapshot 7195c9ec42ec185b068c14efa4f7b50b6893a2671658c0bd75c4f27b5c413156 -->
<!-- api: M:Inno.UI.UiInputSnapshot.#ctor(Inno.Core.Mathematics.Vector2,Inno.Core.Input.KeyModifier,Inno.Core.Input.InputSequence,System.Collections.Generic.IReadOnlyDictionary{System.UInt64,Inno.Core.Input.GamepadState},Inno.Core.Input.TextCompositionState) 21c0b79d94b0f15ddcd6441f5e33658687378075789c38f638c8ea369ee1d533 -->
<!-- api: P:Inno.UI.UiInputSnapshot.sequence 13d376d87c6f50bfbe23fa337acaacb8bc57e161dc09f11d8f64a38b8e3a34bc -->
<!-- api: P:Inno.UI.UiInputSnapshot.textComposition 6caa65e83a7040e88afa2a8486eb3fbd1cadf1bf8b378772efbe7010587c5bf7 -->
<!-- api: P:Inno.UI.UiInputSnapshot.hasTextCompositionInput 48c80674c793839857edecd97d4644712e087c964a69dc8833743722b8386577 -->
<!-- api: P:Inno.UI.UiInputSnapshot.gamepads 64986416dd90b3965e7b675aa5765bdf3e16cf6863829fb2256d9eeda0de6259 -->
<!-- api: P:Inno.UI.UiInputSnapshot.mousePosition 2e65ad038347687c8b667d7227d5635858329919eef87e5f417badac3e9c166d -->
<!-- api: P:Inno.UI.UiInputSnapshot.scrollDelta 3e96b3409ae56c4b734f87beb70f0ba32802ff7d952208fa59f254fd89a6ac32 -->
<!-- api: P:Inno.UI.UiInputSnapshot.modifiers bfd54e4ff816cf43dee28c5b072f3ed4ed0de3999f87694da4c4b5bbc79f6328 -->
<!-- api: P:Inno.UI.UiInputSnapshot.keysPressed 392f5110de7f87b560074fb00ee72bf3600490b34b493eda522092bdd4dad604 -->
<!-- api: P:Inno.UI.UiInputSnapshot.keysReleased 152018b52a38f47d321d892392b810e03c839a61135a937978243c5513546837 -->
<!-- api: P:Inno.UI.UiInputSnapshot.buttonsPressed 745402ce416167cd0d171357e48f94be8f41044be9c4ab654e59a8e333b9b9f4 -->
<!-- api: P:Inno.UI.UiInputSnapshot.buttonsReleased ab7c1cc0834bcf7dd47d4fc8435168996313f2e163a26ac7daa5f58a6c98bf5c -->
<!-- api: P:Inno.UI.UiInputSnapshot.textInput 9f0cdb75af1db0db7cac2a9ca4eb4e6b2491ff4b873907cec5c67dd629afd436 -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-1b3c4f22855e3a64"></a>[`public sealed class Inno.UI.UiInputSnapshot`](../../src/services/ui/Inno.UI/UiTypes.cs#L322) | Carries one immutable input snapshot across the UI backend boundary. | — |
| <a id="api-5496fb56738b24d7"></a>[`public Inno.UI.UiInputSnapshot.UiInputSnapshot(Inno.Core.Mathematics.Vector2 mousePosition, Inno.Core.Input.KeyModifier modifiers, Inno.Core.Input.InputSequence sequence, System.Collections.Generic.IReadOnlyDictionary<ulong, Inno.Core.Input.GamepadState>? gamepads = null, Inno.Core.Input.TextCompositionState? textComposition = null)`](../../src/services/ui/Inno.UI/UiTypes.cs#L348) | Creates a context-local view of one immutable ordered input sequence.<br>`mousePosition`：Final pointer position in context pixels.<br>`modifiers`：Physical modifiers at the frame boundary.<br>`sequence`：Input changes already mapped into context pixels, in acquisition order.<br>`gamepads`：Immutable controller states admitted by the owning input route; null means no admitted devices.<br>`textComposition`：Active composition admitted by the same input route, or null when none remains active. | `System.ArgumentNullException`：The sequence is null.<br>`System.ArgumentException`：A controller state is null or does not match its dictionary key. |
| <a id="api-1e94a4243f4c835c"></a>[`public Inno.Core.Input.InputSequence Inno.UI.UiInputSnapshot.sequence { get; }`](../../src/services/ui/Inno.UI/UiTypes.cs#L370) | Gets the ordered immutable changes without retaining a platform event or callback. | — |
| <a id="api-24dd8a510b14d901"></a>[`public Inno.Core.Input.TextCompositionState? Inno.UI.UiInputSnapshot.textComposition { get; }`](../../src/services/ui/Inno.UI/UiTypes.cs#L375) | Gets active provisional text belonging to this context's input route. | — |
| <a id="api-cb4718bcf6e8bd7e"></a>[`public bool Inno.UI.UiInputSnapshot.hasTextCompositionInput { get; }`](../../src/services/ui/Inno.UI/UiTypes.cs#L380) | Gets whether composition owns this sample, including a commit or cancellation. | — |
| <a id="api-1e8d24bd0d397576"></a>[`public System.Collections.Generic.IReadOnlyDictionary<ulong, Inno.Core.Input.GamepadState> Inno.UI.UiInputSnapshot.gamepads { get; }`](../../src/services/ui/Inno.UI/UiTypes.cs#L385) | Gets controller held state for this input owner without retaining backend or native objects. | — |
| <a id="api-0e3855a029a741e9"></a>[`public Inno.Core.Mathematics.Vector2 Inno.UI.UiInputSnapshot.mousePosition { get; }`](../../src/services/ui/Inno.UI/UiTypes.cs#L390) | Gets the pointer position. | — |
| <a id="api-34e72cbae4aa1cf6"></a>[`public Inno.Core.Mathematics.Vector2 Inno.UI.UiInputSnapshot.scrollDelta { get; }`](../../src/services/ui/Inno.UI/UiTypes.cs#L394) | Gets pointer wheel movement. | — |
| <a id="api-27c961a6932e83b1"></a>[`public Inno.Core.Input.KeyModifier Inno.UI.UiInputSnapshot.modifiers { get; }`](../../src/services/ui/Inno.UI/UiTypes.cs#L398) | Gets active keyboard modifiers. | — |
| <a id="api-e9c1aed81ed4c5fb"></a>[`public System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.KeyCode> Inno.UI.UiInputSnapshot.keysPressed { get; }`](../../src/services/ui/Inno.UI/UiTypes.cs#L402) | Gets physical keys pressed this frame. | — |
| <a id="api-e981be0be5c52876"></a>[`public System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.KeyCode> Inno.UI.UiInputSnapshot.keysReleased { get; }`](../../src/services/ui/Inno.UI/UiTypes.cs#L406) | Gets physical keys released this frame. | — |
| <a id="api-8d9f7181d4817d03"></a>[`public System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.MouseButton> Inno.UI.UiInputSnapshot.buttonsPressed { get; }`](../../src/services/ui/Inno.UI/UiTypes.cs#L410) | Gets pointer buttons pressed this frame. | — |
| <a id="api-38cafe4c79ded47a"></a>[`public System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.MouseButton> Inno.UI.UiInputSnapshot.buttonsReleased { get; }`](../../src/services/ui/Inno.UI/UiTypes.cs#L414) | Gets pointer buttons released this frame. | — |
| <a id="api-2e5f8bd4432260ac"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.UI.UiInputSnapshot.textInput { get; }`](../../src/services/ui/Inno.UI/UiTypes.cs#L418) | Gets ordered Unicode text commits. | — |

### `Inno.UI.UiViewportMapping`

<!-- api: T:Inno.UI.UiViewportMapping 0d7626ae73873bcd478f7446dea650e4e1c9374ff140d40f6d3dfda01b0d25ec -->
<!-- api: M:Inno.UI.UiViewportMapping.#ctor(Inno.Core.Mathematics.Vector2,Inno.Core.Mathematics.Rect) 80bc9ab128a4d1bb1b8c5206d354760777e0c4a3c91350d491b089dd14717690 -->
<!-- api: P:Inno.UI.UiViewportMapping.contextSize ec089a6486a05369536196de70cb7b7985eb874fc17a2f4987c4a26b8f69d66e -->
<!-- api: P:Inno.UI.UiViewportMapping.viewport cf67d0a16f90e928672a75ab5eecba74890e3183219565ef847bc82b56d617c9 -->
<!-- api: M:Inno.UI.UiViewportMapping.ToViewport(Inno.Core.Mathematics.Vector2) a6436b1d7f39d61c2634a4f9c8b07b2eb8f3d1729e96b30c63485257f262651f -->
<!-- api: M:Inno.UI.UiViewportMapping.ToContext(Inno.Core.Mathematics.Vector2) 4974f90d31ce06abfff1c2cb32d32f866c7280c13f50f63f79607eafe5353fb9 -->
<!-- api: M:Inno.UI.UiViewportMapping.ToString 76dae495c456403cdc2b1dfec4ae6c811613abd103fc983761803fbdeed7e7af -->
<!-- api: M:Inno.UI.UiViewportMapping.op_Inequality(Inno.UI.UiViewportMapping,Inno.UI.UiViewportMapping) 76411bcf5eba39ed3070fdaa372b8f51c961c9e59a2f455f4816fe559c8c604b -->
<!-- api: M:Inno.UI.UiViewportMapping.op_Equality(Inno.UI.UiViewportMapping,Inno.UI.UiViewportMapping) 9cfdaa62893554fae31b1b92d79428b3d615e7eba32e96816e19b8e8493aa270 -->
<!-- api: M:Inno.UI.UiViewportMapping.GetHashCode da3bd930b72f920ce2964fa6eddd05323844457cc6f9c9a626728177f0640391 -->
<!-- api: M:Inno.UI.UiViewportMapping.Equals(System.Object) 7df860796c79883d642fbe02a9bfd20d687905e66a1dcecf07885f40790850ba -->
<!-- api: M:Inno.UI.UiViewportMapping.Equals(Inno.UI.UiViewportMapping) ba9a273a69af7245ec51b2aa6de29e3996a7de12ac51ff4e112953fce5834d5d -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-7a07bfb6c6d58112"></a>[`public readonly record struct Inno.UI.UiViewportMapping : System.IEquatable<Inno.UI.UiViewportMapping>`](../../src/services/ui/Inno.UI/UiViewportMapping.cs#L10) | Maps an already density-resolved UI context into the rectangle where its frame is displayed. | — |
| <a id="api-2f2eb25f0d3e861d"></a>[`public Inno.UI.UiViewportMapping.UiViewportMapping(Inno.Core.Mathematics.Vector2 contextSize, Inno.Core.Mathematics.Rect viewport)`](../../src/services/ui/Inno.UI/UiViewportMapping.cs#L24) | Creates the mapping selected by the viewport owner, including letterboxing or editor zoom.<br>`contextSize`：The positive context width and height in pixels.<br>`viewport`：The finite, positive destination rectangle in the owner's viewport coordinate space. | `System.ArgumentOutOfRangeException`：A coordinate is not finite or a size is not positive. |
| <a id="api-6a6471cc4657b522"></a>[`public Inno.Core.Mathematics.Vector2 Inno.UI.UiViewportMapping.contextSize { get; }`](../../src/services/ui/Inno.UI/UiViewportMapping.cs#L40) | Gets the density-resolved UI context dimensions. | — |
| <a id="api-034d5620f41257e6"></a>[`public Inno.Core.Mathematics.Rect Inno.UI.UiViewportMapping.viewport { get; }`](../../src/services/ui/Inno.UI/UiViewportMapping.cs#L44) | Gets the destination rectangle chosen by the viewport owner. | — |
| <a id="api-b1caceb45b04d345"></a>[`public Inno.Core.Mathematics.Vector2 Inno.UI.UiViewportMapping.ToViewport(Inno.Core.Mathematics.Vector2 point)`](../../src/services/ui/Inno.UI/UiViewportMapping.cs#L61) | Converts context pixels to the destination coordinate space without applying density again.<br>`point`：A finite point in UI context pixels, including points outside the context.<br>返回：The corresponding viewport point without clamping. | `System.InvalidOperationException`：This value is uninitialized.<br>`System.ArgumentOutOfRangeException`：The point or result is not finite. |
| <a id="api-0f92868d4b8c9932"></a>[`public Inno.Core.Mathematics.Vector2 Inno.UI.UiViewportMapping.ToContext(Inno.Core.Mathematics.Vector2 point)`](../../src/services/ui/Inno.UI/UiViewportMapping.cs#L85) | Converts a destination point into context pixels without applying density again.<br>`point`：A finite viewport point, including points outside the display rectangle.<br>返回：The corresponding context-pixel point without clamping. | `System.InvalidOperationException`：This value is uninitialized.<br>`System.ArgumentOutOfRangeException`：The point or result is not finite. |
| <a id="api-50e31e0d2656f05f"></a>[`public override string Inno.UI.UiViewportMapping.ToString()`](../../src/services/ui/Inno.UI/UiViewportMapping.cs#L10) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — |
| <a id="api-6e00be2cbffbd755"></a>[`public static bool Inno.UI.UiViewportMapping.operator !=(Inno.UI.UiViewportMapping left, Inno.UI.UiViewportMapping right)`](../../src/services/ui/Inno.UI/UiViewportMapping.cs#L10) | 编译器生成的值不等比较。 | — |
| <a id="api-b2acac0b814658cf"></a>[`public static bool Inno.UI.UiViewportMapping.operator ==(Inno.UI.UiViewportMapping left, Inno.UI.UiViewportMapping right)`](../../src/services/ui/Inno.UI/UiViewportMapping.cs#L10) | 编译器生成的记录值相等比较。 | — |
| <a id="api-0fef33821f3349ad"></a>[`public override int Inno.UI.UiViewportMapping.GetHashCode()`](../../src/services/ui/Inno.UI/UiViewportMapping.cs#L10) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-86cb6740d4b13d16"></a>[`public override bool Inno.UI.UiViewportMapping.Equals(object obj)`](../../src/services/ui/Inno.UI/UiViewportMapping.cs#L10) | Indicates whether this instance and a specified object are equal.<br>`obj`：The object to compare with the current instance.<br>返回：`true` if `obj` and this instance are the same type and represent the same value; otherwise, `false`. | — |
| <a id="api-721d27ec0d544b3e"></a>[`public bool Inno.UI.UiViewportMapping.Equals(Inno.UI.UiViewportMapping other)`](../../src/services/ui/Inno.UI/UiViewportMapping.cs#L10) | Indicates whether the current object is equal to another object of the same type.<br>`other`：An object to compare with this object.<br>返回：`true` if the current object is equal to the `other` parameter; otherwise, `false`. | — |

## 导入纹理与图片注册

UI 不解码已导入图片，也不拥有第二份 GPU 纹理。UiImageSource 保存 Rendering 的纹理产物引用和布局所需尺寸；FromTexture 从已经加载的 TextureAsset 捕获这些值，不保留资产对象。IUiService.RegisterImage 返回 context 内的 IUiImageRegistration；将其 source 赋给文档图片后，由调用方明确释放。替换时先准备新注册，再更新文档引用，最后释放旧注册。释放使该 context 的旧图片源失效，不销毁 Rendering 管理的共享纹理。

UiTextureUpdate.image 非空时表示导入纹理引用，pixels 为空；image 为空时保留后端生成的预乘 RGBA8 像素协议。渲染消费者通过既有纹理产物解析服务采样导入图片，不能将空 pixels 当作缺失图片上传。正常导入纹理保留原颜色空间，采样时由 Rendering 解码。UI 像素与导入图片的 alpha 处理必须由消费者明确区分。

UiDocumentAsset.images 记录导入时确定的依赖。文档加载自动注册这些资源，关闭后解除对应借用。UiDocumentSource.resourceBindings 仅是运行时、不可变的语言资源名映射；其 URI 不能写回资产产物。当前 UiImageSource 表达静态导入纹理，不提供媒体解码、音视频同步或实时视频流能力。
