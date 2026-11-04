# Inno.Adapter.UI.RmlUi

布局捕获通过 RmlUi 公开的文档更新、元素边界与变换接口完成。累积变换在原生渲染准备时确定，因此捕获可准备同一帧的原生 draw data；后续 `Render` 消费这份准备结果，而不是再执行一次相同绘制。未传输资源增量保持待发布状态，后续内容变化会移除已经退休的未发布条目。此机制属于适配器实现，不把第三方对象、矩阵指针或 GPU handle 暴露给 UI 服务层。

当前固定第三方实现提供 Flex 与 Table 等布局，不提供 CSS Grid；批量布局查询不会补写另一套布局算法来模拟未支持的语言功能。返回尺寸遵守原生像素取整。裁剪结果是 viewport 与矩形 scissor 的包围范围；变换或圆角遮罩仍由后端渲染与命中测试负责，不能把该矩形视为精确遮罩。输入纵向滚轮遵守中立约定（正值向上），由适配器映射到后端滚动方向。

[分类索引](README.md) · [Native UI](Inno.Native.UI.md) · [Wiki 首页](../../README.md)

默认 UI backend 将中立 Context、显式 `inno.ui-language.rml` 文本、字体、输入与纹理映射到 RmlUi 6.0 原生桥。RML 识别和 `.rml` importer 位于独立 `Inno.Adapter.UI.RmlUi.Authoring`，通用 UI service/asset 层只处理语言 ID 与文本。

原生侧只手写窄 C++ 语义 facade 与 RmlUi 适配逻辑；BGCS Cpp2C 生成 C ABI，BGCS 再生成 C# function-table binding。RmlUi 类型、回调与渲染热路径都留在 native adapter 内，managed 边界只传递中立 DTO、稳定句柄和增量资源。进程级初始化、线程归属和字体内存集中在显式 `RmlUiProcessHost`。逻辑字体 family 在每个 backend 内映射到 content-addressed 物理 family，因此并存 Session 的同名不同内容不会冲突。`Render` 仅在资源变化时复制 mesh/texture；成功 `FinishFrame` 后确认 delta，失败可重试。

## 使用与生命周期

### 手柄导航

`ProcessInput` 只消费所属 viewport 已接纳的手柄事实，不自行读取设备或注册事件监听器。D-pad 和左摇杆进入 RmlUi 的方向导航；South 进入当前控件的确认行为，East 进入取消行为。空间导航范围、目标、禁用和隐藏规则由文档的 `nav`、`tab-index` 及原生焦点权威决定；例如可导航菜单项声明 `tab-index: auto; nav: auto`。适配只发生在本 context 内，不向 Core Events 发布合成键盘事件，也不改写脚本读取的物理输入。

左摇杆绝对值达到 0.55 才开始导航，回到 0.35 以内释放，以避免阈值附近抖动。最后一个明确的方向输入拥有本 context 的重复动作：首次延迟 0.4 秒，此后间隔 0.08 秒，使用 context 时钟；暂停不会重复，显式步进可以推进重复时间。每次 `ProcessInput` 最多补发一次重复，不积累长帧追赶队列。确认和取消不自动重复，进入一个新 context 时已有的 held state 也不会凭空产生按下动作。

释放按钮、摇杆回中、断开设备、输入 route 不再提供对应 held state、显式切换/清除焦点、文档显示/隐藏/关闭及内容替换都会退休相应的待重复动作。键盘、文本及指针操作会终止原手柄重复；普通鼠标移动不夺走键盘焦点。状态只保存连接 ID、方向和时间，不保留原生控件指针、文档实例或脚本回调。设备输入应由已有窗口/viewport 路由分配，调用方不应向不拥有焦点的 UI context 重放同一快照。

### 文本输入的资源前置条件

组合输入通过每个原生 context 的官方 `TextInputHandler` 接入，原生文本借用只在激活与销毁回调限定的生命周期内使用。`ProcessInput` 按原有顺序消费 Core 的开始、更新、提交和取消事实；预编辑替换当前选区，提交先还原原选区再走控件正常字符插入规则，字符数限制和 change 事件由控件处理。取消、失焦、文档退休或输入快照不再拥有该操作时撤回预编辑。显式值写入退休同一控件的组合操作，迟到提交不能覆盖新值；普通样式更新不取消组合或清除焦点。

包含活动组合或终止事实的样本不再派发编辑按键及手柄导航，避免同次候选确认再次触发提交。原始 Core 输入事实保持不变。预编辑不发布业务值提交，失焦事件在撤回预编辑后捕获值。这里定义的是文档适配行为；操作系统候选窗口仍需要 Host 的文本输入租约及实际窗口坐标接入，不能把直接后端测试视为实体 IME 验收。

文本框与多行文本编辑需要已解析的字体 face。适配器在编辑按键、字符提交和文本控件按下操作进入第三方之前检查该资源；缺失时抛出带控件 ID 的明确异常，保留原值。Tab 导航与纯焦点查询不要求已经进行文本编辑。调用方通过正常字体注册、文档样式更新和布局刷新修正资源后，可以继续使用同一文档。不会偷偷注册默认字体，也不会关闭原生断言。

### Context 时钟和布局

每个 context 保存独立的 elapsed time、暂停状态和单调时钟采样点。进程宿主只安装一次官方 `SystemInterface`；每次原生调用在 owner thread 的局部作用域内绑定该 context 的时间，离开时恢复。创建 context 时初始化尚无文档和输入的根节点；后续 `RefreshLayout` 使用官方 `ElementDocument.UpdateDocument`，不调用私有布局方法，也不触发 Context 的输入/hover 更新。

RmlUi 的 CSS 动画有自身的离散更新规则：当前上游单次动画增量最大为 0.1 秒。需要可重复动画预览时，应按小步进推进并刷新，而不是把一次很大的时钟增量解释成精确 seek。`AdvanceTime` 只推进时间，不偷偷增加布局次数。当前适配器不声明 `TimeSeek`，`TrySeekTime` 返回 false 且不改变文档、焦点或时间。引擎不修改第三方动画算法。

```csharp
using System;
using Inno.Adapter.UI.RmlUi;
using Inno.UI;

static void RenderMarkup(
    RmlUiBackend backend,
    string markup,
    UiInputSnapshot input,
    Action<UiRenderFrame> consumeWhileActive
) {
    UiContextHandle context = backend.CreateContext(new UiContextOptions("HUD", 800, 600));
    try
    {
        UiDocumentHandle document = backend.LoadDocument(context,
            new UiDocumentSource(RmlUiIdentifiers.documentLanguage, markup));
        backend.ShowDocument(context, document);
        backend.Update(context, input);
        consumeWhileActive(backend.Render(context));
    }
    finally
    {
        backend.DestroyContext(context);
    }
}
```

`markup` 与 `input` 由调用方提供；该代码只适用于 Host/adapter 层，脚本使用 `InnoEngine.UI.UI`。Context/Document handle 在进程内不复用；无效或已退休 handle 抛错。Context 销毁时清理 DOM 和像素，因 RmlUi 的全局字体缓存可能在 `Rml::Shutdown` 再调用渲染接口，极小的接口壳保留至最后一个 UI backend 停止。实现不允许外部文件/网络读取隐式绕过 Asset Pipeline。参见 [UI Runtime](../../ui/Inno.UI.Runtime.md)。

未指定文档根 `width`、`height` 时，RML 文档在其所属 Context 内默认占满整个 viewport；文档明确通过 CSS/RCSS 设置宽高时保留作者值。因此绝对定位的 `right`、`bottom` 和百分比以当前 UI Context 的范围为基准，而不是以零尺寸的自动收缩文档为基准。遵循 CSS 的盒模型，`left: 50%; top: 50%` 定位的是元素左上角；要让元素本身居中，再写 `transform: translate(-50%, -50%)`，也可以使用 flex 布局。作者端支持 RML `<style>`、指向 `.rcss` 的 `<link>` 和样式表 `@import`，它们由 RML frontend 在 Asset Pipeline 中展开并追踪依赖；这是 RmlUi 支持的 CSS/RCSS 子集，并不宣称实现完整浏览器 CSS。

## Composition provider

`RmlUiBackendProvider()` 只创建注册描述，不初始化原生服务。`CreateBackend()` 是继承的 provider 创建扩展点，返回调用方拥有的服务。`id` 来自所属领域的内置稳定 ID；同一 provider 可在 composition 生命周期内创建独立服务，具体线程及进程 owner 约束仍由该实现执行。

## 源码归属

当前唯一源码 owner：`backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/Inno.Adapter.UI.RmlUi.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

此清单对应当前 SDK 求值与源码符号；内部类型不构成公开 API。

### `Inno.Adapter.UI.RmlUi.RmlUiBackend`

<!-- api: T:Inno.Adapter.UI.RmlUi.RmlUiBackend 007f6201525f1b4fdf9d83d44c50fb813bb0f1879863e98d066a5665c5bb509e -->
<!-- api: P:Inno.Adapter.UI.RmlUi.RmlUiBackend.implementationId f8541b3a8c29ae4d9554bf9770a953c0235c34f6e35fd38cce5c9f238d8d5bdb -->
<!-- api: P:Inno.Adapter.UI.RmlUi.RmlUiBackend.capabilities 595b308a4cd9997c637cd17e43c8e3e21f7540af6a391f88c7aa9d748e79e4b5 -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.#ctor 46e5657a47dfc59f63c6aa6fd246db404ecbb0e7b92a2b09e90599f5e2ce040c -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.CreateContext(Inno.UI.UiContextOptions) 9b6d62336745bd2de5f7b9b7d490714193a096b080103b603c7c8be59bc18c5b -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.DestroyContext(Inno.UI.UiContextHandle) 4957b038b739d4c8b6c3a3ab9de84dc3390dc3e0dfad6609a05243b626119a25 -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.SetViewport(Inno.UI.UiContextHandle,System.Int32,System.Int32,System.Single) 8e317dff8c3ccc244f8839070f598ce89f79e8595819d0bbaaa6c2d528d2583c -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.LoadDocument(Inno.UI.UiContextHandle,Inno.UI.UiDocumentSource) 762f9ebd49656901cdf3519465bb1c07971ab5cd6a7b0ff92afe11e3455be8c7 -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.ShowDocument(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle) 9195cac9b689bca0c79c69581e7ed7efd6486b526095d7071fa6d5a6a0b7334a -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.HideDocument(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle) 24d3db18436c86a9aa773c22d0214900cfaf0ef02e243f05f4669fd400555cd9 -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.CloseDocument(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle) a1ee4a1f3b7507eb11fc09455050590308aaa2017d093c43a8871fea1c6e230d -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.SetText(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.String,System.String) 7cf9706959bf764644e81d0bfdcc55d0dfb5e533c6514a197a6b30ee768b6c3e -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.SetContent(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.String,Inno.UI.UiDocumentFragment) 65b6dc586c058f859896a0b6fe31c02afb8c070be8427dbbee7a9b5f811e052f -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.SetAttribute(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.String,System.String,System.String) 0206eba4b51639ae3c7b6d0a6eb1d3a980759a231efb18dca6cfbd127f900ecd -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.SetClass(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.String,System.String,System.Boolean) 849c111394867dee441c96121342d4eb59b38f954f67fad87cac364559ce1a1e -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.RegisterFont(Inno.UI.UiFontRegistration) 69dd1c553a7d5da1e24aa97a40ea928a1076ddad213251568111031acd13e34c -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.RegisterTexture(Inno.UI.UiContextHandle,System.String,Inno.UI.UiTextureData) 2d1a1e1b72e50ffc009534a3b47cc28a8159e2ba6c32ff15afec7bfc3fbddc29 -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.HasElementAtPoint(Inno.UI.UiContextHandle,Inno.Core.Mathematics.Vector2) 4889b2c8a8e506440cacd27d40820bb4508bf71070192d8eb60f0543f7cecaea -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.Update(Inno.UI.UiContextHandle,Inno.UI.UiInputSnapshot) 21e17e3caf1c5043c5e652067c8e77c4c47f4a98179361fb1b25d213750893a2 -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.ProcessInput(Inno.UI.UiContextHandle,Inno.UI.UiInputSnapshot) 37d896d7162a957981b385574643fda38ad74cbdf38110e7a28712bc8965b2a4 -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.Render(Inno.UI.UiContextHandle) b851a0a6cfb98c19e425243fdfb3b837273443e7a5b0a6a7dbd13c4130904a1c -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.DrainEvents(Inno.UI.UiContextHandle) ad7bf73a099b9f8513749a84c1b46feea3a7e49aafd171263038fb5bdf081112 -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.Dispose ac2a90c00dac47936860aef06afbca731a320a6742b27cc1f199a0f70b4c7b76 -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.FocusElement(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.String) 537745dba9bf3c8d5cf748274cdd3a08e89ad04a44c2eb272eed4df4ea288767 -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.GetFocusedElement(Inno.UI.UiContextHandle) 29465f161d270714da1d7fe208234964cf45a720248b91b8e66ddfd2929e1eff -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.ClearFocus(Inno.UI.UiContextHandle) d80ec4e76c6a442d20d958d1896de9de191baf2542341e6c98ec06c48a4051f7 -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.RegisterImage(Inno.UI.UiContextHandle,Inno.UI.UiImageSource) 6460fbcc7c045d08d169f14a7203f19b654e0dcb4e66a751bef408b339edcd74 -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.CaptureLayout(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.Collections.Generic.IReadOnlyList{System.String}) a4d58bad598dfe2e7712f94a223b57f11bdbf5aef75bbe6c1296f339eca6f69b -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.GetTime(Inno.UI.UiContextHandle) e54fd10f0e2802acb62b9bb1641f14ff28ef798e96905fdcc3ba4b5ebc5961a0 -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.SetTimePaused(Inno.UI.UiContextHandle,System.Boolean) b40dde5511e05380f70f102038b5cde265a06ba81f3acb29a52e338d68aa8556 -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.AdvanceTime(Inno.UI.UiContextHandle,System.Double) 72156ea31f634e32e846499fca27914e09097a4440ef9fc9b71844d54a16920c -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.TrySeekTime(Inno.UI.UiContextHandle,System.Double) 76c8d13d061a4be1f108ffb77286cd0756711f49f41b1f2e57d85f3917e25a6e -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.RefreshLayout(Inno.UI.UiContextHandle) ce109a8a772038a176126afaefbed5e60fc32b74fcc9e5f305d034a0e40d0c75 -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.GetValue(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.String) c9341642d9667214a36d924622148930d7d6d54b3a9dfa049dd21dac03694136 -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.SetValue(Inno.UI.UiContextHandle,Inno.UI.UiDocumentHandle,System.String,Inno.UI.UiControlValue) cd78404783f7323fe32305e803e69168a6b076304a69cc5912395a60e04078fa -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-58b4adde7d8289b6"></a>[`public sealed class Inno.Adapter.UI.RmlUi.RmlUiBackend : Inno.UI.IUiBackend`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L20) | Implements retained-mode UI document processing through the pinned RmlUi native bridge. | — |
| <a id="api-90d47899cecb71c0"></a>[`public string Inno.Adapter.UI.RmlUi.RmlUiBackend.implementationId { get; }`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L37) | Gets the implementation id text used by the current instance. | — |
| <a id="api-0c97b92dccb9d373"></a>[`public Inno.UI.UiBackendCapabilities Inno.Adapter.UI.RmlUi.RmlUiBackend.capabilities { get; }`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L42) | Gets the immutable feature and limit set reported by the active graphics backend. | — |
| <a id="api-2afae8bc6561b325"></a>[`public Inno.Adapter.UI.RmlUi.RmlUiBackend.RmlUiBackend()`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L47) | Creates and validates one isolated native UI engine. | — |
| <a id="api-ceafa5abaae0610e"></a>[`public Inno.UI.UiContextHandle Inno.Adapter.UI.RmlUi.RmlUiBackend.CreateContext(Inno.UI.UiContextOptions options)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L63) | Creates a context using this implementation's validated inputs.<br>`options`：The validated configuration that controls this operation.<br>返回：The validated ui context handle that represents the completed operation. | — |
| <a id="api-87917827e81a1912"></a>[`public void Inno.Adapter.UI.RmlUi.RmlUiBackend.DestroyContext(Inno.UI.UiContextHandle context)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L78) | Destroys the context after its in-flight references retire.<br>`context`：The context that supplies state and services for this operation. | — |
| <a id="api-2fbea93a9f0604c0"></a>[`public void Inno.Adapter.UI.RmlUi.RmlUiBackend.SetViewport(Inno.UI.UiContextHandle context, int width, int height, float density)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L106) | Updates the viewport state and applies the resulting invariants.<br>`context`：The context that supplies state and services for this operation.<br>`width`：The width in logical units or pixels required by this operation.<br>`height`：The height in logical units or pixels required by this operation.<br>`density`：The density consumed by set viewport; ownership remains with the caller unless explicitly stated otherwise. | — |
| <a id="api-9e8a9d48fdf7670c"></a>[`public Inno.UI.UiDocumentHandle Inno.Adapter.UI.RmlUi.RmlUiBackend.LoadDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentSource source)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L132) | Loads a document into the specified independent UI context.<br>`context`：The context that supplies state and services for this operation.<br>`source`：The source value or location read by this operation.<br>返回：The validated ui document handle that represents the completed operation. | — |
| <a id="api-77aca56c50a2ab58"></a>[`public void Inno.Adapter.UI.RmlUi.RmlUiBackend.ShowDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L158) | Makes the selected document visible in its owning UI context.<br>`context`：The context that supplies state and services for this operation.<br>`document`：The document consumed by show document; ownership remains with the caller unless explicitly stated otherwise. | — |
| <a id="api-2f86fb87fe6414ff"></a>[`public void Inno.Adapter.UI.RmlUi.RmlUiBackend.HideDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L175) | Hides the selected document while retaining its state.<br>`context`：The context that supplies state and services for this operation.<br>`document`：The document consumed by hide document; ownership remains with the caller unless explicitly stated otherwise. | — |
| <a id="api-c05da6c9296f5e1c"></a>[`public void Inno.Adapter.UI.RmlUi.RmlUiBackend.CloseDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L192) | Closes the selected document and releases its retained state.<br>`context`：The context that supplies state and services for this operation.<br>`document`：The document consumed by close document; ownership remains with the caller unless explicitly stated otherwise. | — |
| <a id="api-8fdafb1ac40c9da5"></a>[`public bool Inno.Adapter.UI.RmlUi.RmlUiBackend.SetText(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, string text)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L219) | Updates the text state and applies the resulting invariants.<br>`context`：The context that supplies state and services for this operation.<br>`document`：The document consumed by set text; ownership remains with the caller unless explicitly stated otherwise.<br>`elementId`：The element id text validated by the set text operation.<br>`text`：The text text validated by the set text operation.<br>返回：`true` when the operation succeeds or its condition is satisfied; otherwise, `false`. | — |
| <a id="api-12ab29b986d8f426"></a>[`public bool Inno.Adapter.UI.RmlUi.RmlUiBackend.SetContent(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, Inno.UI.UiDocumentFragment content)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L252) | Updates the content state and applies the resulting invariants.<br>`context`：The context that supplies state and services for this operation.<br>`document`：The document consumed by set content; ownership remains with the caller unless explicitly stated otherwise.<br>`elementId`：The element id text validated by the set content operation.<br>`content`：The content consumed by set content; ownership remains with the caller unless explicitly stated otherwise.<br>返回：`true` when the operation succeeds or its condition is satisfied; otherwise, `false`. | — |
| <a id="api-4fc50982f9be30e9"></a>[`public bool Inno.Adapter.UI.RmlUi.RmlUiBackend.SetAttribute(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, string name, string value)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L294) | Updates the attribute state and applies the resulting invariants.<br>`context`：The context that supplies state and services for this operation.<br>`document`：The document consumed by set attribute; ownership remains with the caller unless explicitly stated otherwise.<br>`elementId`：The element id text validated by the set attribute operation.<br>`name`：The human-readable name used for presentation and diagnostics.<br>`value`：The concrete value read or transformed by this operation.<br>返回：`true` when the operation succeeds or its condition is satisfied; otherwise, `false`. | — |
| <a id="api-9c54894f2dd7efa7"></a>[`public bool Inno.Adapter.UI.RmlUi.RmlUiBackend.SetClass(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, string className, bool active)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L342) | Updates the class state and applies the resulting invariants.<br>`context`：The context that supplies state and services for this operation.<br>`document`：The document consumed by set class; ownership remains with the caller unless explicitly stated otherwise.<br>`elementId`：The element id text validated by the set class operation.<br>`className`：The class name text validated by the set class operation.<br>`active`：Whether active behavior is enabled while set class executes.<br>返回：`true` when the operation succeeds or its condition is satisfied; otherwise, `false`. | — |
| <a id="api-1da21c61104f5b9f"></a>[`public void Inno.Adapter.UI.RmlUi.RmlUiBackend.RegisterFont(Inno.UI.UiFontRegistration registration)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L370) | Registers a font face for document text rendering.<br>`registration`：The registration consumed by register font; ownership remains with the caller unless explicitly stated otherwise. | — |
| <a id="api-7faf9695b3b3bf1c"></a>[`public void Inno.Adapter.UI.RmlUi.RmlUiBackend.RegisterTexture(Inno.UI.UiContextHandle context, string source, Inno.UI.UiTextureData texture)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L408) | Registers a named RGBA texture source for UI document drawing.<br>`context`：The context that supplies state and services for this operation.<br>`source`：The source value or location read by this operation.<br>`texture`：The texture consumed by register texture; ownership remains with the caller unless explicitly stated otherwise. | — |
| <a id="api-efcb2c0d78241c2f"></a>[`public bool Inno.Adapter.UI.RmlUi.RmlUiBackend.HasElementAtPoint(Inno.UI.UiContextHandle context, Inno.Core.Mathematics.Vector2 position)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L443) | Tests whether an interactive document element occupies the supplied point.<br>`context`：The context that supplies state and services for this operation.<br>`position`：The position consumed by has element at point; ownership remains with the caller unless explicitly stated otherwise.<br>返回：`true` when the operation succeeds or its condition is satisfied; otherwise, `false`. | — |
| <a id="api-7690fa2e3b8d843e"></a>[`public void Inno.Adapter.UI.RmlUi.RmlUiBackend.Update(Inno.UI.UiContextHandle context, Inno.UI.UiInputSnapshot input)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L463) | Recomputes owned state from the current validated inputs.<br>`context`：The context that supplies state and services for this operation.<br>`input`：The input consumed by update; ownership remains with the caller unless explicitly stated otherwise. | — |
| <a id="api-64cc62a1e1b6e4c4"></a>[`public void Inno.Adapter.UI.RmlUi.RmlUiBackend.ProcessInput(Inno.UI.UiContextHandle context, Inno.UI.UiInputSnapshot input)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L472) | Processes the explicitly routed input without advancing time or updating the whole context.<br>`context`：The live context on its owner thread.<br>`input`：The current immutable input sample. | — |
| <a id="api-2803f0ee7fa7c720"></a>[`public Inno.UI.UiRenderFrame Inno.Adapter.UI.RmlUi.RmlUiBackend.Render(Inno.UI.UiContextHandle context)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L523) | Records value rendering for the current frame.<br>`context`：The context that supplies state and services for this operation.<br>返回：The validated ui render frame that represents the completed operation. | — |
| <a id="api-a7c75eac8a74345f"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.UI.UiEvent> Inno.Adapter.UI.RmlUi.RmlUiBackend.DrainEvents(Inno.UI.UiContextHandle context)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L605) | Returns and clears events emitted by this UI context.<br>`context`：The context that supplies state and services for this operation.<br>返回：An immutable snapshot of the values selected by the operation. | — |
| <a id="api-6bae8dfa208a2a62"></a>[`public void Inno.Adapter.UI.RmlUi.RmlUiBackend.Dispose()`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L643) | Releases every native context and document owned by this backend. | — |
| <a id="api-125a051726917527"></a>[`public Inno.UI.UiFocusResult Inno.Adapter.UI.RmlUi.RmlUiBackend.FocusElement(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.Focus.cs#L15) | Requests focus through the context's existing focus and modal policy on its owner thread.<br>`context`：The context owning the document.<br>`document`：The live document containing the target.<br>`elementId`：The nonempty authored element ID.<br>返回：The applied, unchanged, missing or ineligible request outcome. | `System.ArgumentException`：The element ID is empty.<br>`System.InvalidOperationException`：A handle is stale, belongs to another context or the caller violates ownership. |
| <a id="api-c751e945755011fa"></a>[`public Inno.UI.UiFocusTarget? Inno.Adapter.UI.RmlUi.RmlUiBackend.GetFocusedElement(Inno.UI.UiContextHandle context)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.Focus.cs#L40) | Reads the current focused control without changing layout or focus.<br>`context`：The live context to query on its owner thread.<br>返回：A neutral focus snapshot, or null when no document control has focus. | `System.InvalidOperationException`：The context is stale or the caller violates ownership. |
| <a id="api-071921696d53a6cc"></a>[`public bool Inno.Adapter.UI.RmlUi.RmlUiBackend.ClearFocus(Inno.UI.UiContextHandle context)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.Focus.cs#L65) | Returns focus to the owning document, preserving modal ownership and emitting normal blur events.<br>`context`：The live context to modify on its owner thread.<br>返回：True when no document control retains focus; false when focus policy refuses the change. | `System.InvalidOperationException`：The context is stale or the caller violates ownership. |
| <a id="api-2cb17f970b209055"></a>[`public Inno.UI.IUiImageRegistration Inno.Adapter.UI.RmlUi.RmlUiBackend.RegisterImage(Inno.UI.UiContextHandle context, Inno.UI.UiImageSource image)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.Images.cs#L31) | Registers an imported image without decoding pixels or acquiring a second GPU resource.<br>`context`：Owning context on the current UI thread.<br>`image`：Frozen artifact identity and imported dimensions.<br>返回：A context-local source registration requiring deterministic disposal. | `System.ArgumentNullException`：The image is null.<br>`System.InvalidOperationException`：The context is unavailable or the caller is not its owner. |
| <a id="api-aa8ccc64bf75183b"></a>[`public Inno.UI.UiLayoutSnapshot Inno.Adapter.UI.RmlUi.RmlUiBackend.CaptureLayout(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, System.Collections.Generic.IReadOnlyList<string> elementIds)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.Layout.cs#L16) | Refreshes visual layout once and captures a batch of element geometry without input or time advancement.<br>`context`：The live context owned by the calling thread.<br>`document`：The live document belonging to the context.<br>`elementIds`：A non-empty list of non-empty authored IDs in result order; missing and hidden elements remain explicit results.<br>返回：An immutable snapshot in context pixels. It does not retain a document lease or native memory. | `System.ArgumentException`：A requested ID is empty or an owner handle is invalid.<br>`System.ArgumentNullException`：The element list is null. |
| <a id="api-ec0e34868a599ae7"></a>[`public Inno.UI.UiContextTime Inno.Adapter.UI.RmlUi.RmlUiBackend.GetTime(Inno.UI.UiContextHandle context)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.Time.cs#L14) | Reads the context clock without advancing it.<br>`context`：The live context on its owner thread.<br>返回：The current elapsed seconds and pause state. | — |
| <a id="api-bf0e91d6f52427af"></a>[`public void Inno.Adapter.UI.RmlUi.RmlUiBackend.SetTimePaused(Inno.UI.UiContextHandle context, bool paused)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.Time.cs#L28) | Controls automatic time advancement; input remains independently routable.<br>`context`：The live context on its owner thread.<br>`paused`：True to freeze automatic time; explicit steps remain available. | — |
| <a id="api-610d3d9088823b8f"></a>[`public void Inno.Adapter.UI.RmlUi.RmlUiBackend.AdvanceTime(Inno.UI.UiContextHandle context, double seconds)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.Time.cs#L41) | Advances only the context clock without processing input or refreshing layout.<br>`context`：The live context on its owner thread.<br>`seconds`：Finite nonnegative time to advance, including while paused. | `System.ArgumentOutOfRangeException`：The step is negative, nonfinite or overflows the clock. |
| <a id="api-869c28e56d1be985"></a>[`public bool Inno.Adapter.UI.RmlUi.RmlUiBackend.TrySeekTime(Inno.UI.UiContextHandle context, double seconds)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.Time.cs#L57) | Requests an absolute time seek only when the backend can preserve document semantics.<br>`context`：The live context on its owner thread.<br>`seconds`：The finite nonnegative requested time.<br>返回：True if applied; false if unsupported, with all state unchanged. | `System.ArgumentOutOfRangeException`：The requested time is negative or nonfinite. |
| <a id="api-a95976e7b4a33be5"></a>[`public void Inno.Adapter.UI.RmlUi.RmlUiBackend.RefreshLayout(Inno.UI.UiContextHandle context)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.Time.cs#L67) | Refreshes document style and layout at the current context time without processing input.<br>`context`：The live context on its owner thread. | — |
| <a id="api-acb6e2ecd2ab631d"></a>[`public Inno.UI.UiControlValue Inno.Adapter.UI.RmlUi.RmlUiBackend.GetValue(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.Values.cs#L15) | Reads a control value on the context owner thread without changing layout or input.<br>`context`：The live context owning the document.<br>`document`：The live document containing the control.<br>`elementId`：The nonempty authored element ID.<br>返回：The current value, or None for a missing element or unsupported control. | `System.InvalidOperationException`：A handle is stale or the caller violates ownership. |
| <a id="api-be6ac6ee4bed03f8"></a>[`public bool Inno.Adapter.UI.RmlUi.RmlUiBackend.SetValue(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, Inno.UI.UiControlValue value)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.Values.cs#L47) | Writes a matching control value without generating value-change or submit events.<br>`context`：The live context owning the document.<br>`document`：The live document containing the control.<br>`elementId`：The nonempty authored element ID.<br>`value`：The typed value; normal control clamping still applies.<br>返回：True when the control accepts the value; false when absent, unsupported or no matching option exists. | `System.ArgumentException`：The value kind does not match the control.<br>`System.InvalidOperationException`：A handle is stale or the caller violates ownership. |

### `Inno.Adapter.UI.RmlUi.RmlUiBackendProvider`

<!-- api: T:Inno.Adapter.UI.RmlUi.RmlUiBackendProvider 2ff6581915ea03a4bba23dffc722e3fed6a30f113078c55e47e70707488ccda9 -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackendProvider.#ctor 648a60394af9211a8027b14f024ccbf91a22652808c00aa192b621a37910a7d3 -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackendProvider.CreateBackend e97665d2f777ada4047caeba6cd37e380979f1a6fa545c4083d5713751c62bef -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-44bfee6f76f83a3b"></a>[`public sealed class Inno.Adapter.UI.RmlUi.RmlUiBackendProvider : Inno.Adapter.UI.UiBackendProvider`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackendProvider.cs#L8) | Supplies independently owned retained UI backend generations. | — |
| <a id="api-c568bfee6f020879"></a>[`public Inno.Adapter.UI.RmlUi.RmlUiBackendProvider.RmlUiBackendProvider()`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackendProvider.cs#L13) | Creates a composition-owned registration for the bundled implementation. | — |
| <a id="api-54d9d9c54436f309"></a>[`public override Inno.UI.IUiBackend Inno.Adapter.UI.RmlUi.RmlUiBackendProvider.CreateBackend()`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackendProvider.cs#L16) | Creates one caller-owned backend generation.<br>返回：A new backend. | — |

### `Inno.Adapter.UI.RmlUi.RmlUiIdentifiers`

<!-- api: T:Inno.Adapter.UI.RmlUi.RmlUiIdentifiers 3c4d2a5bc585b399afb3993ef8216e543b0400904b4acaf8b1c46ece54c1108f -->
<!-- api: P:Inno.Adapter.UI.RmlUi.RmlUiIdentifiers.backend 8ab638a69d8ddfee1726d0e6021c0d5d44a99d30ad36250dd110da4bf6b77039 -->
<!-- api: P:Inno.Adapter.UI.RmlUi.RmlUiIdentifiers.documentLanguage 455d9e6e7bfe1591b1189f77b68af09f0ee0f8c62a2f50e30f7d6a965f5150a5 -->

| 声明 | 行为与参数 | 异常 |
|---|---|---|
| <a id="api-1f52b1a212643012"></a>[`public static class Inno.Adapter.UI.RmlUi.RmlUiIdentifiers`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiIdentifiers.cs#L9) | Publishes stable identities owned by the bundled RmlUi adapter. | — |
| <a id="api-1a3a82d04ccdbe3f"></a>[`public static Inno.Adapter.UI.UiBackendId Inno.Adapter.UI.RmlUi.RmlUiIdentifiers.backend { get; }`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiIdentifiers.cs#L14) | Gets the runtime implementation identity. | — |
| <a id="api-2630f0d18c97fbe6"></a>[`public static Inno.UI.UiDocumentLanguageId Inno.Adapter.UI.RmlUi.RmlUiIdentifiers.documentLanguage { get; }`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiIdentifiers.cs#L19) | Gets the RML source-language identity. | — |

## 导入图片与原生纹理边界

适配器将 context 内图片 source URI 映射为不透明资源编号与尺寸。原生 RenderInterface 只处理布局和纹理引用，不读取资产数据库或取得图形后端句柄。注册退休通过 RmlUi 公开的 ReleaseTexture(source, renderInterface) 作用于当前 render interface，并失效已经准备的布局绘制帧；不能让已释放纹理留在下一次命令中。

当前前端解析 img 的 src 和 spritesheet 的 src，使用原有 AssetPath 解析 source-relative 路径并交给统一导入流程。外部样式中的图片相对该样式来源解析；未生成独立资源数据库。图片声明的私有名称仅在加载时由适配器解释，普通文档文字不参与 URI 替换。


## 文本输入公开边界

<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.GetTextInputArea(Inno.UI.UiContextHandle) 671d2d12b5045175aeda87ff7df2859be5ae48ecbc0b689d8d2f294a4da76642 -->
- `public Inno.Core.Mathematics.Rect? Inno.Adapter.UI.RmlUi.RmlUiBackend.GetTextInputArea(Inno.UI.UiContextHandle context)`：通过官方 `TextInputContext.GetBoundingBox` 读取当前文本控件的投影区域。查询保持只读，不调用私有布局入口，不保存跨帧原生借用；没有活动文本控件或有效可见区域时返回 null。
