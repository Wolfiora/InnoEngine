# Inno.UI

[UI 索引](README.md) · [Text](../text/Inno.Text.md)

`IUiService` / `UI` 门面提供 Context 生命周期、语言标记的文档加载、纯文本/内容片段/attribute/class 变更、RGBA8 命名纹理注册、`Update`、`Render` 与事件提取。字体由导入文档声明并在 `LoadDocument` 前注册，脚本不再逐项覆盖字体。核心不知道 RML。`UiRenderFrame` 只发布 mesh/texture 增量、稳定 handle、释放请求与有序 draw commands，供渲染 Plugin 持久缓存。

每个 Context 的生命周期由调用方明确拥有。无活动 `UiExecutionContext` 时门面失败；持有服务的 Runtime/Provider 在其 owner 生命周期中关闭 Context。唯一 `Properties/ScriptingApi.cs` 限定脚本导出类型；`IUiBackend` 和 RmlUi ABI 不导出。基础层不依赖 Scene、Rendering 或 Editor。

## 公开类型与工作流

| 类型 | 语义 |
| --- | --- |
| `UiContextOptions`、`UiContextHandle`、`UiDocumentHandle` | 独立 surface 的尺寸/density 与不透明代际句柄；调用方负责关闭。 |
| `UiDocumentAsset`、`UiDocumentSource`、`UiDocumentFontFace` | 导入文档与字体资产依赖，或运行期生成带语言标记的内容。 |
| `UiTextureData`、`UiFrameTexture` | 只接受紧密排列 RGBA8；帧纹理含 ID、revision 和像素副本。 |
| `UiVertex`、`UiMeshUpdate`、`UiTextureUpdate`、`UiDrawCommand`、`UiRenderFrame` | 后端中立资源增量与绘制命令；不是图形设备 handle。 |
| `UiEventType`、`UiEvent` | 后端更新后提取的 DOM 事件；`UiEvent` 派生自 Core `Event`，包含文档 handle 与元素 ID。 |
| `UI`、`IUiService`、`UiExecutionContext` | 文档生命周期、DOM 更改、纹理注册、独立 Context 输入更新、Render/DrainEvents。 |
| `IUiBackend`、`UiInputSnapshot`、`UiFontRegistration` | Runtime/adapter 边界，不属于脚本导出。 |

```csharp
using InnoEngine.UI;

static UiDocumentHandle Show(UiContextHandle context, UiDocumentAsset source)
{
    UiDocumentHandle document = UI.LoadDocument(context, source);
    UI.ShowDocument(context, document);
    return document;
}
```

上述调用需位于 UI Session scope；先 `Update` 再 `DrainEvents`/`Render`。`DrainEvents` 是后端到调用方的传输边界；世界 Canvas 将提取的事件交给 Core EventDispatcher/EventHub 分发，业务脚本通过 `Canvas.Listen` 订阅。世界 UI 应把实际 View 命中的局部输入传给 `Update(context, input)`，避免不同 Canvas 共用窗口鼠标。Context 关闭后句柄失效，跨 Session/Plugin reload 不持久化；持久状态应保存 Asset ID 与稳定业务 ID。UI 不承诺任意外部文件路径加载：RML 适配器目前支持内联 RCSS 的字体声明，以及调用方注册的命名纹理。导入字体在文档关闭或 Context 销毁后释放资产 lease；具体后端决定原生字体缓存何时清空。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.UI.IUiBackend`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.IUiBackend`](../../src/services/ui/Inno.UI/IUiBackend.cs#L10) | Defines the replaceable document, layout, input, and draw-geometry backend boundary. |
| [`Inno.UI.UiBackendCapabilities Inno.UI.IUiBackend.capabilities`](../../src/services/ui/Inno.UI/IUiBackend.cs#L19) | Gets optional behavior and accepted document languages for this generation. |
| [`Inno.UI.UiContextHandle Inno.UI.IUiBackend.CreateContext(Inno.UI.UiContextOptions options)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L29) | Creates one independent UI context. |
| [`Inno.UI.UiDocumentHandle Inno.UI.IUiBackend.LoadDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentSource source)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L70) | Loads one explicitly tagged in-memory document. |
| [`Inno.UI.UiRenderFrame Inno.UI.IUiBackend.Render(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L279) | Builds an immutable frame of incremental resources and ordered draws. |
| [`System.Collections.Generic.IReadOnlyList<Inno.UI.UiEvent> Inno.UI.IUiBackend.DrainEvents(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L289) | Drains document events queued by the preceding updates. |
| [`bool Inno.UI.IUiBackend.HasElementAtPoint(Inno.UI.UiContextHandle context, Inno.Core.Mathematics.Vector2 position)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L253) | Tests whether a visible document element receives pointer events at a pixel location. |
| [`bool Inno.UI.IUiBackend.SetAttribute(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, string name, string value)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L182) | Sets one element attribute. |
| [`bool Inno.UI.IUiBackend.SetClass(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, string className, bool active)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L210) | Activates or deactivates one element class. |
| [`bool Inno.UI.IUiBackend.SetContent(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, Inno.UI.UiDocumentFragment fragment)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L155) | Replaces an element's children with an explicitly tagged source-language fragment. |
| [`bool Inno.UI.IUiBackend.SetText(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, string text)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L131) | Replaces an element's children with plain Unicode text. |
| [`string Inno.UI.IUiBackend.implementationId`](../../src/services/ui/Inno.UI/IUiBackend.cs#L15) | Gets the stable implementation identity used by imported document artifacts. |
| [`void Inno.UI.IUiBackend.CloseDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L109) | Closes one document. |
| [`void Inno.UI.IUiBackend.DestroyContext(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L36) | Destroys one UI context and all of its documents. |
| [`void Inno.UI.IUiBackend.HideDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L96) | Hides one document. |
| [`void Inno.UI.IUiBackend.RegisterFont(Inno.UI.UiFontRegistration registration)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L223) | Registers an encoded font face in the document engine. |
| [`void Inno.UI.IUiBackend.RegisterTexture(Inno.UI.UiContextHandle context, string source, Inno.UI.UiTextureData texture)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L236) | Registers a named RGBA8 texture source. |
| [`void Inno.UI.IUiBackend.SetViewport(Inno.UI.UiContextHandle context, int width, int height, float density)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L52) | Changes one UI context's pixel dimensions and density. |
| [`void Inno.UI.IUiBackend.ShowDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L83) | Shows one document. |
| [`void Inno.UI.IUiBackend.Update(Inno.UI.UiContextHandle context, Inno.UI.UiInputSnapshot input)`](../../src/services/ui/Inno.UI/IUiBackend.cs#L266) | Processes input and advances document state. |

### `Inno.UI.IUiService`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.IUiService`](../../src/services/ui/Inno.UI/IUiService.cs#L10) | Provides retained-mode documents and backend-neutral UI frames to scripts and rendering plugins. |
| [`Inno.UI.UiContextHandle Inno.UI.IUiService.CreateContext(Inno.UI.UiContextOptions options)`](../../src/services/ui/Inno.UI/IUiService.cs#L21) | Creates one independent UI context. |
| [`Inno.UI.UiDocumentHandle Inno.UI.IUiService.LoadDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentAsset document)`](../../src/services/ui/Inno.UI/IUiService.cs#L78) | Loads one imported document compatible with the selected backend. |
| [`Inno.UI.UiDocumentHandle Inno.UI.IUiService.LoadDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentSource source)`](../../src/services/ui/Inno.UI/IUiService.cs#L62) | Loads one explicitly tagged in-memory document. |
| [`Inno.UI.UiRenderFrame Inno.UI.IUiService.Render(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI/IUiService.cs#L287) | Builds an immutable frame of incremental resources and ordered draws. |
| [`System.Collections.Generic.IReadOnlyList<Inno.UI.UiEvent> Inno.UI.IUiService.DrainEvents(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI/IUiService.cs#L297) | Drains document events queued by preceding updates. |
| [`bool Inno.UI.IUiService.HasElementAtPoint(Inno.UI.UiContextHandle context, Inno.Core.Mathematics.Vector2 position)`](../../src/services/ui/Inno.UI/IUiService.cs#L254) | Tests whether a visible document element receives pointer events at a pixel location. |
| [`bool Inno.UI.IUiService.SetAttribute(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, string name, string value)`](../../src/services/ui/Inno.UI/IUiService.cs#L190) | Sets one element attribute. |
| [`bool Inno.UI.IUiService.SetClass(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, string className, bool active)`](../../src/services/ui/Inno.UI/IUiService.cs#L218) | Activates or deactivates one element class. |
| [`bool Inno.UI.IUiService.SetContent(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, Inno.UI.UiDocumentFragment fragment)`](../../src/services/ui/Inno.UI/IUiService.cs#L163) | Replaces an element's children with an explicitly tagged source-language fragment. |
| [`bool Inno.UI.IUiService.SetText(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, string text)`](../../src/services/ui/Inno.UI/IUiService.cs#L139) | Replaces an element's children with plain Unicode text. |
| [`void Inno.UI.IUiService.CloseDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document)`](../../src/services/ui/Inno.UI/IUiService.cs#L117) | Closes one document. |
| [`void Inno.UI.IUiService.DestroyContext(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI/IUiService.cs#L28) | Destroys one UI context and all owned documents. |
| [`void Inno.UI.IUiService.HideDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document)`](../../src/services/ui/Inno.UI/IUiService.cs#L104) | Hides one document. |
| [`void Inno.UI.IUiService.RegisterTexture(Inno.UI.UiContextHandle context, string source, Inno.UI.UiTextureData texture)`](../../src/services/ui/Inno.UI/IUiService.cs#L237) | Registers one named RGBA8 texture source. |
| [`void Inno.UI.IUiService.SetViewport(Inno.UI.UiContextHandle context, int width, int height, float density = 1)`](../../src/services/ui/Inno.UI/IUiService.cs#L44) | Changes one context's pixel dimensions and density. |
| [`void Inno.UI.IUiService.ShowDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document)`](../../src/services/ui/Inno.UI/IUiService.cs#L91) | Shows one document. |
| [`void Inno.UI.IUiService.Update(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI/IUiService.cs#L264) | Processes current-frame input and advances one context. |
| [`void Inno.UI.IUiService.Update(Inno.UI.UiContextHandle context, Inno.UI.UiInputSnapshot input)`](../../src/services/ui/Inno.UI/IUiService.cs#L274) | Advances one context with explicitly routed viewport-local input. |

### `Inno.UI.UI`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.UI`](../../src/services/ui/Inno.UI/UI.cs#L39) | Provides script-friendly retained-mode UI operations. |
| [`static Inno.UI.UiContextHandle Inno.UI.UI.CreateContext(Inno.UI.UiContextOptions options)`](../../src/services/ui/Inno.UI/UI.cs#L50) | Creates one independent UI context. |
| [`static Inno.UI.UiDocumentHandle Inno.UI.UI.LoadDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentAsset document)`](../../src/services/ui/Inno.UI/UI.cs#L109) | Loads one imported document compatible with the selected backend. |
| [`static Inno.UI.UiDocumentHandle Inno.UI.UI.LoadDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentSource source)`](../../src/services/ui/Inno.UI/UI.cs#L92) | Loads one explicitly tagged in-memory document. |
| [`static Inno.UI.UiRenderFrame Inno.UI.UI.Render(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI/UI.cs#L311) | Builds an immutable frame of incremental resources and ordered draws. |
| [`static System.Collections.Generic.IReadOnlyList<Inno.UI.UiEvent> Inno.UI.UI.DrainEvents(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI/UI.cs#L321) | Drains document events queued by preceding updates. |
| [`static bool Inno.UI.UI.SetAttribute(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, string name, string value)`](../../src/services/ui/Inno.UI/UI.cs#L227) | Sets one element attribute. |
| [`static bool Inno.UI.UI.SetClass(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, string className, bool active)`](../../src/services/ui/Inno.UI/UI.cs#L256) | Activates or deactivates one element class. |
| [`static bool Inno.UI.UI.SetContent(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, Inno.UI.UiDocumentFragment fragment)`](../../src/services/ui/Inno.UI/UI.cs#L199) | Replaces an element's children with an explicitly tagged source-language fragment. |
| [`static bool Inno.UI.UI.SetText(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document, string elementId, string text)`](../../src/services/ui/Inno.UI/UI.cs#L174) | Replaces an element's children with plain Unicode text. |
| [`static void Inno.UI.UI.CloseDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document)`](../../src/services/ui/Inno.UI/UI.cs#L151) | Closes one document. |
| [`static void Inno.UI.UI.DestroyContext(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI/UI.cs#L57) | Destroys one UI context and all owned documents. |
| [`static void Inno.UI.UI.HideDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document)`](../../src/services/ui/Inno.UI/UI.cs#L137) | Hides one document. |
| [`static void Inno.UI.UI.RegisterTexture(Inno.UI.UiContextHandle context, string source, Inno.UI.UiTextureData texture)`](../../src/services/ui/Inno.UI/UI.cs#L276) | Registers one named RGBA8 texture source. |
| [`static void Inno.UI.UI.SetViewport(Inno.UI.UiContextHandle context, int width, int height, float density = 1)`](../../src/services/ui/Inno.UI/UI.cs#L73) | Changes one context's pixel dimensions and density. |
| [`static void Inno.UI.UI.ShowDocument(Inno.UI.UiContextHandle context, Inno.UI.UiDocumentHandle document)`](../../src/services/ui/Inno.UI/UI.cs#L123) | Shows one document. |
| [`static void Inno.UI.UI.Update(Inno.UI.UiContextHandle context)`](../../src/services/ui/Inno.UI/UI.cs#L288) | Processes current-frame input and advances one context. |
| [`static void Inno.UI.UI.Update(Inno.UI.UiContextHandle context, Inno.UI.UiInputSnapshot input)`](../../src/services/ui/Inno.UI/UI.cs#L298) | Advances one context with explicitly routed viewport-local input. |

### `Inno.UI.UiBackendCapabilities`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.UiBackendCapabilities`](../../src/services/ui/Inno.UI/UiBackendCapabilities.cs#L11) | Describes optional behavior implemented by one UI backend generation. |
| [`Inno.UI.UiBackendCapabilities.UiBackendCapabilities(System.Collections.Generic.IEnumerable<Inno.UI.UiDocumentLanguageId> documentLanguages, bool supportsFontCollectionFaces)`](../../src/services/ui/Inno.UI/UiBackendCapabilities.cs#L24) | Creates an immutable capability description. |
| [`System.Collections.Generic.IReadOnlyList<Inno.UI.UiDocumentLanguageId> Inno.UI.UiBackendCapabilities.documentLanguages`](../../src/services/ui/Inno.UI/UiBackendCapabilities.cs#L39) | Gets source languages accepted by the backend. |
| [`bool Inno.UI.UiBackendCapabilities.Supports(Inno.UI.UiDocumentLanguageId language)`](../../src/services/ui/Inno.UI/UiBackendCapabilities.cs#L55) | Determines whether an exact source language is supported. |
| [`bool Inno.UI.UiBackendCapabilities.supportsFontCollectionFaces`](../../src/services/ui/Inno.UI/UiBackendCapabilities.cs#L44) | Gets whether the backend can select nonzero faces from a font collection. |

### `Inno.UI.UiBackendCapability`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.UiBackendCapability`](../../src/services/ui/Inno.UI/UiBackendCapabilities.cs#L61) | Identifies an optional backend behavior requested by a caller. |
| [`Inno.UI.UiBackendCapability.FontCollectionFaceSelection`](../../src/services/ui/Inno.UI/UiBackendCapabilities.cs#L66) | Selecting an explicit face from a font collection. |

### `Inno.UI.UiCapabilityUnavailableException`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.UiBackendCapability Inno.UI.UiCapabilityUnavailableException.capability`](../../src/services/ui/Inno.UI/UiBackendCapabilities.cs#L87) | Gets the unavailable capability. |
| [`Inno.UI.UiCapabilityUnavailableException`](../../src/services/ui/Inno.UI/UiBackendCapabilities.cs#L72) | Reports that the selected backend cannot perform an explicitly requested optional operation. |
| [`Inno.UI.UiCapabilityUnavailableException.UiCapabilityUnavailableException(Inno.UI.UiBackendCapability capability)`](../../src/services/ui/Inno.UI/UiBackendCapabilities.cs#L80) | Creates a capability failure without naming a concrete implementation. |

### `Inno.UI.UiClipRectangle`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.UiClipRectangle`](../../src/services/ui/Inno.UI/UiRendering.cs#L48) | Defines an integer clipping rectangle in UI surface coordinates. |

### `Inno.UI.UiContextHandle`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.UiContextHandle`](../../src/services/ui/Inno.UI/UiHandles.cs#L9) | Identifies one UI context owned by the active UI service generation. |
| [`bool Inno.UI.UiContextHandle.isValid`](../../src/services/ui/Inno.UI/UiHandles.cs#L14) | Gets whether this handle identifies a live context candidate. |

### `Inno.UI.UiContextOptions`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.UiContextOptions`](../../src/services/ui/Inno.UI/UiTypes.cs#L12) | Defines one independent UI layout and interaction surface. |
| [`Inno.UI.UiContextOptions.UiContextOptions(string name, int width, int height, float density = 1)`](../../src/services/ui/Inno.UI/UiTypes.cs#L29) | Creates validated context options. |
| [`float Inno.UI.UiContextOptions.density`](../../src/services/ui/Inno.UI/UiTypes.cs#L61) | Gets the density-independent pixel ratio. |
| [`int Inno.UI.UiContextOptions.height`](../../src/services/ui/Inno.UI/UiTypes.cs#L57) | Gets the initial height in pixels. |
| [`int Inno.UI.UiContextOptions.width`](../../src/services/ui/Inno.UI/UiTypes.cs#L53) | Gets the initial width in pixels. |
| [`string Inno.UI.UiContextOptions.name`](../../src/services/ui/Inno.UI/UiTypes.cs#L49) | Gets the diagnostic context name. |

### `Inno.UI.UiDocumentAsset`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.UiDocumentAsset`](../../src/services/ui/Inno.UI/UiDocumentAsset.cs#L16) | Represents one imported, language-tagged UI document source. |
| [`Inno.UI.UiDocumentSource? Inno.UI.UiDocumentAsset.source`](../../src/services/ui/Inno.UI/UiDocumentAsset.cs#L27) | Gets the frozen source, or null before runtime content is loaded. |
| [`System.Collections.Generic.IReadOnlyList<Inno.UI.UiDocumentFontFace> Inno.UI.UiDocumentAsset.fonts`](../../src/services/ui/Inno.UI/UiDocumentAsset.cs#L31) | Gets font dependencies declared by this document. |
| [`override void Inno.UI.UiDocumentAsset.OnRuntimePayloadChanged(System.ReadOnlyMemory<byte> previousPayload, System.ReadOnlyMemory<byte> currentPayload)`](../../src/services/ui/Inno.UI/UiDocumentAsset.cs#L90) | Rebuilds runtime-derived state after the serialized asset payload changes. |
| [`static byte[] Inno.UI.UiDocumentAsset.CreateRuntimePayload(string implementationId, Inno.UI.UiDocumentSource source, System.Collections.Generic.IReadOnlyList<Inno.UI.UiDocumentFontFace>? fonts = null)`](../../src/services/ui/Inno.UI/UiDocumentAsset.cs#L53) | Encodes an importer-owned runtime payload without exposing an implementation-specific format. |
| [`string Inno.UI.UiDocumentAsset.implementationId`](../../src/services/ui/Inno.UI/UiDocumentAsset.cs#L36) | Gets the exact backend implementation selected when the source was imported. |

### `Inno.UI.UiDocumentFontFace`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Text.TextFontStyle Inno.UI.UiDocumentFontFace.style`](../../src/services/ui/Inno.UI/UiDocumentFontFace.cs#L54) | Gets the declared style. |
| [`Inno.UI.UiDocumentFontFace`](../../src/services/ui/Inno.UI/UiDocumentFontFace.cs#L9) | Identifies one imported font face owned by a single UI document. |
| [`Inno.UI.UiDocumentFontFace.UiDocumentFontFace(System.Guid assetId, string family, Inno.Text.TextFontStyle style, int weight)`](../../src/services/ui/Inno.UI/UiDocumentFontFace.cs#L26) | Creates a font dependency with its document-private family. |
| [`System.Guid Inno.UI.UiDocumentFontFace.assetId`](../../src/services/ui/Inno.UI/UiDocumentFontFace.cs#L46) | Gets the imported font asset identity. |
| [`int Inno.UI.UiDocumentFontFace.weight`](../../src/services/ui/Inno.UI/UiDocumentFontFace.cs#L58) | Gets the declared weight. |
| [`string Inno.UI.UiDocumentFontFace.family`](../../src/services/ui/Inno.UI/UiDocumentFontFace.cs#L50) | Gets the family referenced by the document. |

### `Inno.UI.UiDocumentFragment`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.UiDocumentFragment`](../../src/services/ui/Inno.UI/UiTypes.cs#L112) | Contains a language-tagged fragment for a backend-specific document mutation. |
| [`Inno.UI.UiDocumentFragment.UiDocumentFragment(Inno.UI.UiDocumentLanguageId language, string text)`](../../src/services/ui/Inno.UI/UiTypes.cs#L123) | Creates a validated source fragment. |
| [`Inno.UI.UiDocumentLanguageId Inno.UI.UiDocumentFragment.language`](../../src/services/ui/Inno.UI/UiTypes.cs#L137) | Gets the explicitly selected fragment language. |
| [`string Inno.UI.UiDocumentFragment.text`](../../src/services/ui/Inno.UI/UiTypes.cs#L141) | Gets the complete fragment text. |

### `Inno.UI.UiDocumentHandle`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.UiDocumentHandle`](../../src/services/ui/Inno.UI/UiHandles.cs#L23) | Identifies one document owned by a UI context. |
| [`bool Inno.UI.UiDocumentHandle.isValid`](../../src/services/ui/Inno.UI/UiHandles.cs#L28) | Gets whether this handle identifies a live document candidate. |

### `Inno.UI.UiDocumentLanguageId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.UiDocumentLanguageId`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L8) | Identifies a document source language independently from any UI backend implementation. |
| [`Inno.UI.UiDocumentLanguageId.UiDocumentLanguageId(string value)`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L16) | Creates a stable, ordinal language identifier. |
| [`bool Inno.UI.UiDocumentLanguageId.isValid`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L33) | Gets whether the identifier is assigned. |
| [`override string Inno.UI.UiDocumentLanguageId.ToString()`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L41) | Returns the stable identifier. |
| [`string Inno.UI.UiDocumentLanguageId.value`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L28) | Gets the stable language identifier. |

### `Inno.UI.UiDocumentSource`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.UiDocumentLanguageId Inno.UI.UiDocumentSource.language`](../../src/services/ui/Inno.UI/UiTypes.cs#L98) | Gets the explicitly selected source language. |
| [`Inno.UI.UiDocumentSource`](../../src/services/ui/Inno.UI/UiTypes.cs#L67) | Contains immutable text in one explicitly selected UI document language. |
| [`Inno.UI.UiDocumentSource.UiDocumentSource(Inno.UI.UiDocumentLanguageId language, string text, string sourceUri = "memory://ui-document")`](../../src/services/ui/Inno.UI/UiTypes.cs#L81) | Creates a validated in-memory document source. |
| [`string Inno.UI.UiDocumentSource.sourceUri`](../../src/services/ui/Inno.UI/UiTypes.cs#L106) | Gets the virtual source address. |
| [`string Inno.UI.UiDocumentSource.text`](../../src/services/ui/Inno.UI/UiTypes.cs#L102) | Gets the complete immutable source text. |

### `Inno.UI.UiDrawCommand`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.UiDrawCommand`](../../src/services/ui/Inno.UI/UiRendering.cs#L151) | Describes one draw of a previously published mesh. |

### `Inno.UI.UiEvent`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.UiDocumentHandle Inno.UI.UiEvent.document`](../../src/services/ui/Inno.UI/UiTypes.cs#L261) | Gets the source document. |
| [`Inno.UI.UiEvent`](../../src/services/ui/Inno.UI/UiTypes.cs#L229) | Describes one queued document event after a context update. |
| [`Inno.UI.UiEvent.UiEvent(Inno.UI.UiEventType type, Inno.UI.UiDocumentHandle document, string targetId)`](../../src/services/ui/Inno.UI/UiTypes.cs#L243) | Creates a document event with its source document and target element. |
| [`Inno.UI.UiEventType Inno.UI.UiEvent.type`](../../src/services/ui/Inno.UI/UiTypes.cs#L256) | Gets the kind of document interaction. |
| [`string Inno.UI.UiEvent.targetId`](../../src/services/ui/Inno.UI/UiTypes.cs#L266) | Gets the target element identifier. |

### `Inno.UI.UiEventType`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.UiEventType`](../../src/services/ui/Inno.UI/UiTypes.cs#L194) | Identifies one document event made visible to game code. |
| [`Inno.UI.UiEventType.Blur`](../../src/services/ui/Inno.UI/UiTypes.cs#L215) | The target lost focus. |
| [`Inno.UI.UiEventType.Change`](../../src/services/ui/Inno.UI/UiTypes.cs#L203) | The target value changed. |
| [`Inno.UI.UiEventType.Click`](../../src/services/ui/Inno.UI/UiTypes.cs#L199) | The target was clicked. |
| [`Inno.UI.UiEventType.Focus`](../../src/services/ui/Inno.UI/UiTypes.cs#L211) | The target gained focus. |
| [`Inno.UI.UiEventType.MouseEnter`](../../src/services/ui/Inno.UI/UiTypes.cs#L219) | A pointer entered the target. |
| [`Inno.UI.UiEventType.MouseLeave`](../../src/services/ui/Inno.UI/UiTypes.cs#L223) | A pointer left the target. |
| [`Inno.UI.UiEventType.Submit`](../../src/services/ui/Inno.UI/UiTypes.cs#L207) | A form was submitted. |

### `Inno.UI.UiExecutionContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.UiExecutionContext`](../../src/services/ui/Inno.UI/UI.cs#L11) | Binds one UI service to the current asynchronous execution context. |
| [`static Inno.UI.IUiService Inno.UI.UiExecutionContext.current`](../../src/services/ui/Inno.UI/UI.cs#L18) | Gets the UI service bound to the current execution context. |
| [`static System.IDisposable Inno.UI.UiExecutionContext.EnterScope(Inno.UI.IUiService ui)`](../../src/services/ui/Inno.UI/UI.cs#L29) | Binds a UI service until the returned strict last-in-first-out scope is disposed. |

### `Inno.UI.UiFontRegistration`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Text.TextFontStyle Inno.UI.UiFontRegistration.style`](../../src/services/ui/Inno.UI/UiFontRegistration.cs#L65) | Gets the logical style. |
| [`Inno.UI.UiFontRegistration`](../../src/services/ui/Inno.UI/UiFontRegistration.cs#L10) | Describes one encoded font face registration across the neutral backend boundary. |
| [`Inno.UI.UiFontRegistration.UiFontRegistration(System.ReadOnlyMemory<byte> data, int faceIndex, string family, Inno.Text.TextFontStyle style, int weight)`](../../src/services/ui/Inno.UI/UiFontRegistration.cs#L30) | Creates a validated synchronous font registration request. |
| [`System.ReadOnlyMemory<byte> Inno.UI.UiFontRegistration.data`](../../src/services/ui/Inno.UI/UiFontRegistration.cs#L53) | Gets encoded font bytes valid for the synchronous backend call. |
| [`int Inno.UI.UiFontRegistration.faceIndex`](../../src/services/ui/Inno.UI/UiFontRegistration.cs#L57) | Gets the zero-based collection face index. |
| [`int Inno.UI.UiFontRegistration.weight`](../../src/services/ui/Inno.UI/UiFontRegistration.cs#L69) | Gets the logical weight. |
| [`string Inno.UI.UiFontRegistration.family`](../../src/services/ui/Inno.UI/UiFontRegistration.cs#L61) | Gets the logical family. |

### `Inno.UI.UiInputSnapshot`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Input.KeyModifier Inno.UI.UiInputSnapshot.modifiers`](../../src/services/ui/Inno.UI/UiTypes.cs#L332) | Gets active keyboard modifiers. |
| [`Inno.Core.Mathematics.Vector2 Inno.UI.UiInputSnapshot.mousePosition`](../../src/services/ui/Inno.UI/UiTypes.cs#L324) | Gets the pointer position. |
| [`Inno.Core.Mathematics.Vector2 Inno.UI.UiInputSnapshot.scrollDelta`](../../src/services/ui/Inno.UI/UiTypes.cs#L328) | Gets pointer wheel movement. |
| [`Inno.UI.UiInputSnapshot`](../../src/services/ui/Inno.UI/UiTypes.cs#L272) | Carries one immutable input snapshot across the UI backend boundary. |
| [`Inno.UI.UiInputSnapshot.UiInputSnapshot(Inno.Core.Mathematics.Vector2 mousePosition, Inno.Core.Mathematics.Vector2 scrollDelta, Inno.Core.Input.KeyModifier modifiers, System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.KeyCode> keysPressed, System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.KeyCode> keysReleased, System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.MouseButton> buttonsPressed, System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.MouseButton> buttonsReleased, System.Collections.Generic.IReadOnlyList<string> textInput)`](../../src/services/ui/Inno.UI/UiTypes.cs#L301) | Creates one backend-neutral UI input snapshot. |
| [`System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.KeyCode> Inno.UI.UiInputSnapshot.keysPressed`](../../src/services/ui/Inno.UI/UiTypes.cs#L336) | Gets physical keys pressed this frame. |
| [`System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.KeyCode> Inno.UI.UiInputSnapshot.keysReleased`](../../src/services/ui/Inno.UI/UiTypes.cs#L340) | Gets physical keys released this frame. |
| [`System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.MouseButton> Inno.UI.UiInputSnapshot.buttonsPressed`](../../src/services/ui/Inno.UI/UiTypes.cs#L344) | Gets pointer buttons pressed this frame. |
| [`System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.MouseButton> Inno.UI.UiInputSnapshot.buttonsReleased`](../../src/services/ui/Inno.UI/UiTypes.cs#L348) | Gets pointer buttons released this frame. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.UI.UiInputSnapshot.textInput`](../../src/services/ui/Inno.UI/UiTypes.cs#L352) | Gets ordered Unicode text commits. |

### `Inno.UI.UiMeshHandle`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.UiMeshHandle`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L50) | Identifies one backend-owned immutable mesh generation. |
| [`bool Inno.UI.UiMeshHandle.isValid`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L55) | Gets whether the handle identifies a mesh. |

### `Inno.UI.UiMeshUpdate`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.UiMeshHandle Inno.UI.UiMeshUpdate.mesh`](../../src/services/ui/Inno.UI/UiRendering.cs#L78) | Gets the generation-scoped mesh handle. |
| [`Inno.UI.UiMeshUpdate`](../../src/services/ui/Inno.UI/UiRendering.cs#L58) | Publishes one immutable mesh generation to a rendering plugin. |
| [`System.Collections.Generic.IReadOnlyList<Inno.UI.UiVertex> Inno.UI.UiMeshUpdate.vertices`](../../src/services/ui/Inno.UI/UiRendering.cs#L86) | Gets immutable local-space vertices. |
| [`System.Collections.Generic.IReadOnlyList<uint> Inno.UI.UiMeshUpdate.indices`](../../src/services/ui/Inno.UI/UiRendering.cs#L90) | Gets immutable triangle indices. |
| [`ulong Inno.UI.UiMeshUpdate.revision`](../../src/services/ui/Inno.UI/UiRendering.cs#L82) | Gets the monotonically increasing mesh content revision. |

### `Inno.UI.UiRenderFrame`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.UiRenderFrame`](../../src/services/ui/Inno.UI/UiRendering.cs#L161) | Contains incremental resource changes and ordered draw commands for one UI context. |
| [`System.Collections.Generic.IReadOnlyList<Inno.UI.UiDrawCommand> Inno.UI.UiRenderFrame.commands`](../../src/services/ui/Inno.UI/UiRendering.cs#L231) | Gets ordered draws referencing published resources. |
| [`System.Collections.Generic.IReadOnlyList<Inno.UI.UiMeshHandle> Inno.UI.UiRenderFrame.releasedMeshes`](../../src/services/ui/Inno.UI/UiRendering.cs#L219) | Gets mesh generations that must be retired. |
| [`System.Collections.Generic.IReadOnlyList<Inno.UI.UiMeshUpdate> Inno.UI.UiRenderFrame.meshUpdates`](../../src/services/ui/Inno.UI/UiRendering.cs#L215) | Gets created or replaced mesh generations. |
| [`System.Collections.Generic.IReadOnlyList<Inno.UI.UiTextureHandle> Inno.UI.UiRenderFrame.releasedTextures`](../../src/services/ui/Inno.UI/UiRendering.cs#L227) | Gets texture generations that must be retired. |
| [`System.Collections.Generic.IReadOnlyList<Inno.UI.UiTextureUpdate> Inno.UI.UiRenderFrame.textureUpdates`](../../src/services/ui/Inno.UI/UiRendering.cs#L223) | Gets created or replaced texture generations. |
| [`static Inno.UI.UiRenderFrame Inno.UI.UiRenderFrame.CreateRetirement(Inno.UI.UiMeshHandle[] meshes, Inno.UI.UiTextureHandle[] textures)`](../../src/services/ui/Inno.UI/UiRendering.cs#L200) | Creates a command-free frame that deterministically retires plugin-owned resources. |
| [`static Inno.UI.UiRenderFrame Inno.UI.UiRenderFrame.empty`](../../src/services/ui/Inno.UI/UiRendering.cs#L186) | Gets an empty resource-delta frame. |

### `Inno.UI.UiRenderFrameBuilder`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.UiRenderFrame Inno.UI.UiRenderFrameBuilder.Build()`](../../src/services/ui/Inno.UI/UiRendering.cs#L371) | Freezes the accumulated frame. The builder cannot be reused. |
| [`Inno.UI.UiRenderFrameBuilder`](../../src/services/ui/Inno.UI/UiRendering.cs#L240) | Builds one immutable backend-neutral frame while transferring ownership of update arrays. |
| [`void Inno.UI.UiRenderFrameBuilder.AddCommand(Inno.UI.UiDrawCommand command)`](../../src/services/ui/Inno.UI/UiRendering.cs#L357) | Adds one ordered mesh draw. |
| [`void Inno.UI.UiRenderFrameBuilder.AddMeshUpdate(Inno.UI.UiMeshHandle mesh, ulong revision, Inno.UI.UiVertex[] vertices, uint[] indices)`](../../src/services/ui/Inno.UI/UiRendering.cs#L264) | Adds one mesh update and takes exclusive ownership of both arrays. |
| [`void Inno.UI.UiRenderFrameBuilder.AddReleasedMesh(Inno.UI.UiMeshHandle mesh)`](../../src/services/ui/Inno.UI/UiRendering.cs#L291) | Adds one mesh retirement. |
| [`void Inno.UI.UiRenderFrameBuilder.AddReleasedTexture(Inno.UI.UiTextureHandle texture)`](../../src/services/ui/Inno.UI/UiRendering.cs#L343) | Adds one texture retirement. |
| [`void Inno.UI.UiRenderFrameBuilder.AddTextureUpdate(Inno.UI.UiTextureHandle texture, ulong revision, int width, int height, byte[] pixels)`](../../src/services/ui/Inno.UI/UiRendering.cs#L317) | Adds one texture update and takes exclusive ownership of its pixel array. |

### `Inno.UI.UiTextureData`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.UiTextureData`](../../src/services/ui/Inno.UI/UiTypes.cs#L147) | Contains immutable RGBA8 texture pixels addressable by a document source name. |
| [`Inno.UI.UiTextureData.UiTextureData(int width, int height, System.ReadOnlySpan<byte> pixels)`](../../src/services/ui/Inno.UI/UiTypes.cs#L163) | Creates a validated immutable texture. |
| [`System.ReadOnlyMemory<byte> Inno.UI.UiTextureData.pixels`](../../src/services/ui/Inno.UI/UiTypes.cs#L188) | Gets the immutable RGBA8 pixels. |
| [`int Inno.UI.UiTextureData.height`](../../src/services/ui/Inno.UI/UiTypes.cs#L184) | Gets the texture height. |
| [`int Inno.UI.UiTextureData.width`](../../src/services/ui/Inno.UI/UiTypes.cs#L180) | Gets the texture width. |

### `Inno.UI.UiTextureHandle`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.UiTextureHandle`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L64) | Identifies one backend-owned immutable texture generation. |
| [`bool Inno.UI.UiTextureHandle.isValid`](../../src/services/ui/Inno.UI/UiIdentifiers.cs#L69) | Gets whether the handle identifies a texture; an unassigned handle selects the renderer's white texture. |

### `Inno.UI.UiTextureUpdate`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.UiTextureHandle Inno.UI.UiTextureUpdate.texture`](../../src/services/ui/Inno.UI/UiRendering.cs#L117) | Gets the generation-scoped texture handle. |
| [`Inno.UI.UiTextureUpdate`](../../src/services/ui/Inno.UI/UiRendering.cs#L96) | Publishes one immutable premultiplied RGBA8 texture generation. |
| [`System.ReadOnlyMemory<byte> Inno.UI.UiTextureUpdate.pixels`](../../src/services/ui/Inno.UI/UiRendering.cs#L133) | Gets immutable tightly packed premultiplied RGBA8 pixels. |
| [`int Inno.UI.UiTextureUpdate.height`](../../src/services/ui/Inno.UI/UiRendering.cs#L129) | Gets the texture height. |
| [`int Inno.UI.UiTextureUpdate.width`](../../src/services/ui/Inno.UI/UiRendering.cs#L125) | Gets the texture width. |
| [`ulong Inno.UI.UiTextureUpdate.revision`](../../src/services/ui/Inno.UI/UiRendering.cs#L121) | Gets the monotonically increasing content revision. |

### `Inno.UI.UiVertex`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.UiVertex`](../../src/services/ui/Inno.UI/UiRendering.cs#L25) | Describes one vertex in backend-neutral UI draw geometry. |

## 项目依赖

- [Inno.Assets](../assets/Inno.Assets.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Text](../text/Inno.Text.md)：公开引用边界由实际签名核对。
- [Inno.Input](../input/Inno.Input.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Events](../core/Inno.Core.Events.md)：公开引用边界由实际签名核对。
- [Inno.Core.Input](../core/Inno.Core.Input.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Mathematics](../core/Inno.Core.Mathematics.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
