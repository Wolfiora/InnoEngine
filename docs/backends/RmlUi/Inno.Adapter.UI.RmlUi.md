# Inno.Adapter.UI.RmlUi

[分类索引](README.md) · [Native UI](Inno.Native.UI.md) · [Wiki 首页](../../README.md)

默认 UI backend 将中立 Context、显式 `inno.ui-language.rml` 文本、字体、输入与纹理映射到 RmlUi 6.0 原生桥。RML 识别和 `.rml` importer 位于独立 `Inno.Adapter.UI.RmlUi.Authoring`，通用 UI service/asset 层只处理语言 ID 与文本。

原生侧只手写窄 C++ 语义 facade 与 RmlUi 适配逻辑；BGCS Cpp2C 生成 C ABI，BGCS 再生成 C# function-table binding。RmlUi 类型、回调与渲染热路径都留在 native adapter 内，managed 边界只传递中立 DTO、稳定句柄和增量资源。进程级初始化、线程归属和字体内存集中在显式 `RmlUiProcessHost`。逻辑字体 family 在每个 backend 内映射到 content-addressed 物理 family，因此并存 Session 的同名不同内容不会冲突。`Render` 仅在资源变化时复制 mesh/texture；成功 `FinishFrame` 后确认 delta，失败可重试。

## 使用与生命周期

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

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

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
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.Render(Inno.UI.UiContextHandle) b851a0a6cfb98c19e425243fdfb3b837273443e7a5b0a6a7dbd13c4130904a1c -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.DrainEvents(Inno.UI.UiContextHandle) ad7bf73a099b9f8513749a84c1b46feea3a7e49aafd171263038fb5bdf081112 -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackend.Dispose ac2a90c00dac47936860aef06afbca731a320a6742b27cc1f199a0f70b4c7b76 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2afae8bc6561b325"></a>[`public Inno.Adapter.UI.RmlUi.RmlUiBackend.RmlUiBackend()`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L45) | Creates and validates one isolated native UI engine. | — | — |
| <a id="api-c05da6c9296f5e1c"></a>[`public void Inno.Adapter.UI.RmlUi.RmlUiBackend.CloseDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L179) | Closes the selected document and releases its retained state.<br>`context`：The context that supplies state and services for this operation.<br>`document`：The document consumed by close document; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-ceafa5abaae0610e"></a>[`public Inno.UI.UiContextHandle Inno.Adapter.UI.RmlUi.RmlUiBackend.CreateContext(Inno.UI.UiContextOptions options)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L61) | Creates a context using this implementation's validated inputs.<br>返回：The validated ui context handle that represents the completed operation.<br>`options`：The validated configuration that controls this operation. | — | — |
| <a id="api-87917827e81a1912"></a>[`public void Inno.Adapter.UI.RmlUi.RmlUiBackend.DestroyContext(Inno.UI.UiContextHandle context)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L76) | Destroys the context after its in-flight references retire.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-6bae8dfa208a2a62"></a>[`public void Inno.Adapter.UI.RmlUi.RmlUiBackend.Dispose()`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L593) | Releases every native context and document owned by this backend. | — | — |
| <a id="api-a7c75eac8a74345f"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.UI.UiEvent> Inno.Adapter.UI.RmlUi.RmlUiBackend.DrainEvents(Inno.UI.UiContextHandle context)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L558) | Returns and clears events emitted by this UI context.<br>返回：An immutable snapshot of the values selected by the operation.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-efcb2c0d78241c2f"></a>[`public bool Inno.Adapter.UI.RmlUi.RmlUiBackend.HasElementAtPoint(Inno.UI.UiContextHandle context, Inno.Core.Mathematics.Vector2 position)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L424) | Tests whether an interactive document element occupies the supplied point.<br>返回：true when the operation succeeds or its condition is satisfied; otherwise, false.<br>`context`：The context that supplies state and services for this operation.<br>`position`：The position consumed by has element at point; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-2f86fb87fe6414ff"></a>[`public void Inno.Adapter.UI.RmlUi.RmlUiBackend.HideDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L162) | Hides the selected document while retaining its state.<br>`context`：The context that supplies state and services for this operation.<br>`document`：The document consumed by hide document; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-9e8a9d48fdf7670c"></a>[`public Inno.UI.UiDocumentHandle Inno.Adapter.UI.RmlUi.RmlUiBackend.LoadDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentSource source)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L123) | Loads a document into the specified independent UI context.<br>返回：The validated ui document handle that represents the completed operation.<br>`context`：The context that supplies state and services for this operation.<br>`source`：The source value or location read by this operation. | — | — |
| <a id="api-1da21c61104f5b9f"></a>[`public void Inno.Adapter.UI.RmlUi.RmlUiBackend.RegisterFont(Inno.UI.UiFontRegistration registration)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L351) | Registers a font face for document text rendering.<br>`registration`：The registration consumed by register font; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-7faf9695b3b3bf1c"></a>[`public void Inno.Adapter.UI.RmlUi.RmlUiBackend.RegisterTexture(Inno.UI.UiContextHandle context, string source, Inno.UI.UiTextureData texture)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L389) | Registers a named RGBA texture source for UI document drawing.<br>`context`：The context that supplies state and services for this operation.<br>`source`：The source value or location read by this operation.<br>`texture`：The texture consumed by register texture; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-2803f0ee7fa7c720"></a>[`public Inno.UI.UiRenderFrame Inno.Adapter.UI.RmlUi.RmlUiBackend.Render(Inno.UI.UiContextHandle context)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L484) | Records value rendering for the current frame.<br>返回：The validated ui render frame that represents the completed operation.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-4fc50982f9be30e9"></a>[`public bool Inno.Adapter.UI.RmlUi.RmlUiBackend.SetAttribute(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, string name, string value)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L279) | Updates the attribute state and applies the resulting invariants.<br>返回：true when the operation succeeds or its condition is satisfied; otherwise, false.<br>`context`：The context that supplies state and services for this operation.<br>`document`：The document consumed by set attribute; ownership remains with the caller unless explicitly stated otherwise.<br>`elementId`：The element id text validated by the set attribute operation.<br>`name`：The human-readable name used for presentation and diagnostics.<br>`value`：The concrete value read or transformed by this operation. | — | — |
| <a id="api-9c54894f2dd7efa7"></a>[`public bool Inno.Adapter.UI.RmlUi.RmlUiBackend.SetClass(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, string className, bool active)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L323) | Updates the class state and applies the resulting invariants.<br>返回：true when the operation succeeds or its condition is satisfied; otherwise, false.<br>`context`：The context that supplies state and services for this operation.<br>`document`：The document consumed by set class; ownership remains with the caller unless explicitly stated otherwise.<br>`elementId`：The element id text validated by the set class operation.<br>`className`：The class name text validated by the set class operation.<br>`active`：Whether active behavior is enabled while set class executes. | — | — |
| <a id="api-12ab29b986d8f426"></a>[`public bool Inno.Adapter.UI.RmlUi.RmlUiBackend.SetContent(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, Inno.UI.UiDocumentFragment content)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L238) | Updates the content state and applies the resulting invariants.<br>返回：true when the operation succeeds or its condition is satisfied; otherwise, false.<br>`context`：The context that supplies state and services for this operation.<br>`document`：The document consumed by set content; ownership remains with the caller unless explicitly stated otherwise.<br>`elementId`：The element id text validated by the set content operation.<br>`content`：The content consumed by set content; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-8fdafb1ac40c9da5"></a>[`public bool Inno.Adapter.UI.RmlUi.RmlUiBackend.SetText(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, string text)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L205) | Updates the text state and applies the resulting invariants.<br>返回：true when the operation succeeds or its condition is satisfied; otherwise, false.<br>`context`：The context that supplies state and services for this operation.<br>`document`：The document consumed by set text; ownership remains with the caller unless explicitly stated otherwise.<br>`elementId`：The element id text validated by the set text operation.<br>`text`：The text text validated by the set text operation. | — | — |
| <a id="api-2fbea93a9f0604c0"></a>[`public void Inno.Adapter.UI.RmlUi.RmlUiBackend.SetViewport(Inno.UI.UiContextHandle context, int width, int height, float density)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L97) | Updates the viewport state and applies the resulting invariants.<br>`context`：The context that supplies state and services for this operation.<br>`width`：The width in logical units or pixels required by this operation.<br>`height`：The height in logical units or pixels required by this operation.<br>`density`：The density consumed by set viewport; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-77aca56c50a2ab58"></a>[`public void Inno.Adapter.UI.RmlUi.RmlUiBackend.ShowDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L145) | Makes the selected document visible in its owning UI context.<br>`context`：The context that supplies state and services for this operation.<br>`document`：The document consumed by show document; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-7690fa2e3b8d843e"></a>[`public void Inno.Adapter.UI.RmlUi.RmlUiBackend.Update(Inno.UI.UiContextHandle context, Inno.UI.UiInputSnapshot input)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L444) | Recomputes owned state from the current validated inputs.<br>`context`：The context that supplies state and services for this operation.<br>`input`：The input consumed by update; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-0c97b92dccb9d373"></a>[`public Inno.UI.UiBackendCapabilities Inno.Adapter.UI.RmlUi.RmlUiBackend.capabilities { get; }`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L40) | Gets the immutable feature and limit set reported by the active graphics backend. | — | — |
| <a id="api-90d47899cecb71c0"></a>[`public string Inno.Adapter.UI.RmlUi.RmlUiBackend.implementationId { get; }`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L35) | Gets the implementation id text used by the current instance. | — | — |
| <a id="api-58b4adde7d8289b6"></a>[`public sealed class Inno.Adapter.UI.RmlUi.RmlUiBackend : Inno.UI.IUiBackend`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L20) | Implements retained-mode UI document processing through the pinned RmlUi native bridge. | — | — |

### `Inno.Adapter.UI.RmlUi.RmlUiBackendProvider`

<!-- api: T:Inno.Adapter.UI.RmlUi.RmlUiBackendProvider 2ff6581915ea03a4bba23dffc722e3fed6a30f113078c55e47e70707488ccda9 -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackendProvider.#ctor 648a60394af9211a8027b14f024ccbf91a22652808c00aa192b621a37910a7d3 -->
<!-- api: M:Inno.Adapter.UI.RmlUi.RmlUiBackendProvider.CreateBackend e97665d2f777ada4047caeba6cd37e380979f1a6fa545c4083d5713751c62bef -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c568bfee6f020879"></a>[`public Inno.Adapter.UI.RmlUi.RmlUiBackendProvider.RmlUiBackendProvider()`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackendProvider.cs#L13) | Creates a composition-owned registration for the bundled implementation. | — | — |
| <a id="api-54d9d9c54436f309"></a>[`public override Inno.UI.IUiBackend Inno.Adapter.UI.RmlUi.RmlUiBackendProvider.CreateBackend()`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackendProvider.cs#L16) | Creates one caller-owned backend generation.<br>返回：A new backend. | — | — |
| <a id="api-44bfee6f76f83a3b"></a>[`public sealed class Inno.Adapter.UI.RmlUi.RmlUiBackendProvider : Inno.Adapter.UI.UiBackendProvider`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackendProvider.cs#L8) | Supplies independently owned retained UI backend generations. | — | — |

### `Inno.Adapter.UI.RmlUi.RmlUiIdentifiers`

<!-- api: T:Inno.Adapter.UI.RmlUi.RmlUiIdentifiers 3c4d2a5bc585b399afb3993ef8216e543b0400904b4acaf8b1c46ece54c1108f -->
<!-- api: P:Inno.Adapter.UI.RmlUi.RmlUiIdentifiers.backend 8ab638a69d8ddfee1726d0e6021c0d5d44a99d30ad36250dd110da4bf6b77039 -->
<!-- api: P:Inno.Adapter.UI.RmlUi.RmlUiIdentifiers.documentLanguage 455d9e6e7bfe1591b1189f77b68af09f0ee0f8c62a2f50e30f7d6a965f5150a5 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1a3a82d04ccdbe3f"></a>[`public static Inno.Adapter.UI.UiBackendId Inno.Adapter.UI.RmlUi.RmlUiIdentifiers.backend { get; }`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiIdentifiers.cs#L14) | Gets the runtime implementation identity. | — | — |
| <a id="api-2630f0d18c97fbe6"></a>[`public static Inno.UI.UiDocumentLanguageId Inno.Adapter.UI.RmlUi.RmlUiIdentifiers.documentLanguage { get; }`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiIdentifiers.cs#L19) | Gets the RML source-language identity. | — | — |
| <a id="api-1f52b1a212643012"></a>[`public static class Inno.Adapter.UI.RmlUi.RmlUiIdentifiers`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiIdentifiers.cs#L9) | Publishes stable identities owned by the bundled RmlUi adapter. | — | — |

## 项目依赖

- [Inno.Adapter.UI](../../ui/Inno.Adapter.UI.md)：公开引用边界由实际签名核对。
- [Inno.UI](../../ui/Inno.UI.md)：公开引用边界由实际签名核对。
- [Inno.Core.Input](../../core/Inno.Core.Input.md)：公开引用边界由实际签名核对。
- [Inno.Core.Mathematics](../../core/Inno.Core.Mathematics.md)：公开引用边界由实际签名核对。
- [Inno.Native.UI](Inno.Native.UI.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
