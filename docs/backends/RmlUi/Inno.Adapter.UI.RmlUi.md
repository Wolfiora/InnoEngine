# Inno.Adapter.UI.RmlUi

[分类索引](README.md) · [Native UI](Inno.Native.UI.md) · [Wiki 首页](../../README.md)

默认 UI backend 将中立 Context、显式 `inno.ui-language.rml` 文本、字体、输入与纹理映射到 RmlUi 6.0 原生桥。RML 识别和 `.rml` importer 位于独立 `Inno.Adapter.UI.RmlUi.Authoring`，通用 UI service/asset 层只处理语言 ID 与文本。

原生侧只手写窄 C++ 语义 facade 与 RmlUi 适配逻辑；BGCS Cpp2C 生成 C ABI，BGCS 再生成 C# function-table binding。RmlUi 类型、回调与渲染热路径都留在 native adapter 内，managed 边界只传递中立 DTO、稳定句柄和增量资源。进程级初始化、线程归属和字体内存集中在显式 `RmlUiProcessHost`。逻辑字体 family 在每个 backend 内映射到 content-addressed 物理 family，因此并存 Session 的同名不同内容不会冲突。`Render` 仅在资源变化时复制 mesh/texture；成功 `FinishFrame` 后确认 delta，失败可重试。

## 使用与生命周期

```csharp
using Inno.Adapter.UI.RmlUi;
using Inno.UI;

using var backend = new RmlUiBackend();
UiContextHandle context = backend.CreateContext(new UiContextOptions("HUD", 800, 600));
UiDocumentHandle document = backend.LoadDocument(context, new UiDocumentSource(RmlUiIdentifiers.documentLanguage, markup));
backend.ShowDocument(context, document);
backend.Update(context, input);
UiRenderFrame frame = backend.Render(context);
backend.DestroyContext(context);
```

`markup` 与 `input` 由调用方提供；该代码只适用于 Host/adapter 层，脚本使用 `InnoEngine.UI.UI`。Context/Document handle 在进程内不复用；无效或已退休 handle 抛错。Context 销毁时清理 DOM 和像素，因 RmlUi 的全局字体缓存可能在 `Rml::Shutdown` 再调用渲染接口，极小的接口壳保留至最后一个 UI backend 停止。实现不允许外部文件/网络读取隐式绕过 Asset Pipeline。参见 [UI Runtime](../../ui/Inno.UI.Runtime.md)。

未指定文档根 `width`、`height` 时，RML 文档在其所属 Context 内默认占满整个 viewport；文档明确通过 CSS/RCSS 设置宽高时保留作者值。因此绝对定位的 `right`、`bottom` 和百分比以当前 Canvas/Context 的范围为基准，而不是以零尺寸的自动收缩文档为基准。遵循 CSS 的盒模型，`left: 50%; top: 50%` 定位的是元素左上角；要让元素本身居中，再写 `transform: translate(-50%, -50%)`，也可以使用 flex 布局。作者端支持 RML `<style>`、指向 `.rcss` 的 `<link>` 和样式表 `@import`，它们由 RML frontend 在 Asset Pipeline 中展开并追踪依赖；这是 RmlUi 支持的 CSS/RCSS 子集，并不宣称实现完整浏览器 CSS。

## Composition provider

`RmlUiBackendProvider()` 只创建注册描述，不初始化原生服务。`CreateBackend()` 是继承的 provider 创建扩展点，返回调用方拥有的服务。`id` 来自所属领域的内置稳定 ID；同一 provider 可在 composition 生命周期内创建独立服务，具体线程及进程 owner 约束仍由该实现执行。

## 源码归属

当前唯一源码 owner：`backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/Inno.Adapter.UI.RmlUi.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Adapter.UI.RmlUi.RmlUiBackend`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.UI.RmlUi.RmlUiBackend`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L20) | Implements retained-mode UI document processing through the pinned RmlUi native bridge. |
| [`Inno.Adapter.UI.RmlUi.RmlUiBackend.RmlUiBackend()`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L45) | Creates and validates one isolated native UI engine. |
| [`Inno.UI.UiBackendCapabilities Inno.Adapter.UI.RmlUi.RmlUiBackend.capabilities`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L40) | Gets the immutable feature and limit set reported by the active graphics backend. |
| [`Inno.UI.UiContextHandle Inno.Adapter.UI.RmlUi.RmlUiBackend.CreateContext(Inno.UI.UiContextOptions options)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L61) | Creates a context using this implementation's validated inputs. |
| [`Inno.UI.UiDocumentHandle Inno.Adapter.UI.RmlUi.RmlUiBackend.LoadDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentSource source)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L123) | Loads a document into the specified independent UI context. |
| [`Inno.UI.UiRenderFrame Inno.Adapter.UI.RmlUi.RmlUiBackend.Render(Inno.UI.UiContextHandle context)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L484) | Records value rendering for the current frame. |
| [`System.Collections.Generic.IReadOnlyList<Inno.UI.UiEvent> Inno.Adapter.UI.RmlUi.RmlUiBackend.DrainEvents(Inno.UI.UiContextHandle context)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L558) | Returns and clears events emitted by this UI context. |
| [`bool Inno.Adapter.UI.RmlUi.RmlUiBackend.HasElementAtPoint(Inno.UI.UiContextHandle context, Inno.Core.Mathematics.Vector2 position)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L424) | Tests whether an interactive document element occupies the supplied point. |
| [`bool Inno.Adapter.UI.RmlUi.RmlUiBackend.SetAttribute(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, string name, string value)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L279) | Updates the attribute state and applies the resulting invariants. |
| [`bool Inno.Adapter.UI.RmlUi.RmlUiBackend.SetClass(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, string className, bool active)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L323) | Updates the class state and applies the resulting invariants. |
| [`bool Inno.Adapter.UI.RmlUi.RmlUiBackend.SetContent(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, Inno.UI.UiDocumentFragment content)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L238) | Updates the content state and applies the resulting invariants. |
| [`bool Inno.Adapter.UI.RmlUi.RmlUiBackend.SetText(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, string text)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L205) | Updates the text state and applies the resulting invariants. |
| [`string Inno.Adapter.UI.RmlUi.RmlUiBackend.implementationId`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L35) | Gets the implementation id text used by the current instance. |
| [`void Inno.Adapter.UI.RmlUi.RmlUiBackend.CloseDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L179) | Closes the selected document and releases its retained state. |
| [`void Inno.Adapter.UI.RmlUi.RmlUiBackend.DestroyContext(Inno.UI.UiContextHandle context)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L76) | Destroys the context after its in-flight references retire. |
| [`void Inno.Adapter.UI.RmlUi.RmlUiBackend.Dispose()`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L593) | Releases every native context and document owned by this backend. |
| [`void Inno.Adapter.UI.RmlUi.RmlUiBackend.HideDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L162) | Hides the selected document while retaining its state. |
| [`void Inno.Adapter.UI.RmlUi.RmlUiBackend.RegisterFont(Inno.UI.UiFontRegistration registration)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L351) | Registers a font face for document text rendering. |
| [`void Inno.Adapter.UI.RmlUi.RmlUiBackend.RegisterTexture(Inno.UI.UiContextHandle context, string source, Inno.UI.UiTextureData texture)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L389) | Registers a named RGBA texture source for UI document drawing. |
| [`void Inno.Adapter.UI.RmlUi.RmlUiBackend.SetViewport(Inno.UI.UiContextHandle context, int width, int height, float density)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L97) | Updates the viewport state and applies the resulting invariants. |
| [`void Inno.Adapter.UI.RmlUi.RmlUiBackend.ShowDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L145) | Makes the selected document visible in its owning UI context. |
| [`void Inno.Adapter.UI.RmlUi.RmlUiBackend.Update(Inno.UI.UiContextHandle context, Inno.UI.UiInputSnapshot input)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackend.cs#L444) | Recomputes owned state from the current validated inputs. |

### `Inno.Adapter.UI.RmlUi.RmlUiBackendProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.UI.RmlUi.RmlUiBackendProvider`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackendProvider.cs#L8) | Supplies independently owned retained UI backend generations. |
| [`Inno.Adapter.UI.RmlUi.RmlUiBackendProvider.RmlUiBackendProvider()`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackendProvider.cs#L13) | Creates a composition-owned registration for the bundled implementation. |
| [`override Inno.UI.IUiBackend Inno.Adapter.UI.RmlUi.RmlUiBackendProvider.CreateBackend()`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiBackendProvider.cs#L16) | See the implemented contract. |

### `Inno.Adapter.UI.RmlUi.RmlUiIdentifiers`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.UI.RmlUi.RmlUiIdentifiers`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiIdentifiers.cs#L9) | Publishes stable identities owned by the bundled RmlUi adapter. |
| [`static Inno.Adapter.UI.UiBackendId Inno.Adapter.UI.RmlUi.RmlUiIdentifiers.backend`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiIdentifiers.cs#L14) | Gets the runtime implementation identity. |
| [`static Inno.UI.UiDocumentLanguageId Inno.Adapter.UI.RmlUi.RmlUiIdentifiers.documentLanguage`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi/RmlUiIdentifiers.cs#L19) | Gets the RML source-language identity. |

## 项目依赖

- [Inno.Adapter.UI](../../ui/Inno.Adapter.UI.md)：公开引用边界由实际签名核对。
- [Inno.UI](../../ui/Inno.UI.md)：公开引用边界由实际签名核对。
- [Inno.Core.Input](../../core/Inno.Core.Input.md)：公开引用边界由实际签名核对。
- [Inno.Core.Mathematics](../../core/Inno.Core.Mathematics.md)：公开引用边界由实际签名核对。
- [Inno.Native.UI](Inno.Native.UI.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
