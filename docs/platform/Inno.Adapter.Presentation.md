# Inno.Adapter.Presentation

[Platform 索引](README.md) · [Rendering](../rendering/README.md) · [Runtime adapters](../runtime/Inno.Adapter.md)

该项目定义 authoring product 的图形 Presentation family。它不暴露 SDL3 window、BGFX handle 或具体 ImGui context。

## 公开 API

- `PresentationBackendId`：开放、区分大小写的稳定实现 ID，默认值表示未分配；内置值为 `imGui`。
- `PresentationBackendProvider`、`PresentationBackendCatalog`：组合入口提供完整 provider 集合，catalog 验证 ID、拒绝重复和 null，并发布只读 `supportedBackends`。注册不创建原生上下文。
- `PresentationFeatures`：声明中立 capability。
- `PresentationBackendOptions`：组合中立 platform/window/render device、shader compiler 与 asset source。
- `PresentationTextureHandle`：presentation generation 内使用的 opaque texture token。
- `IPresentationContext`：layout、frame draw、texture registration、render-graph contribution，以及 `TryGetWindowId(viewportId, out windowId)` 查询活动 viewport 的平台窗口身份。
- `IPresentationBackendFactory`：创建 presentation context。
- `IAuthoringAdapterCatalog`：在 runtime `IAdapterCatalog` 上增加 rendering authoring 与 presentation factory。

EditorHost 只保存 `IPresentationContext`。SDL3 event bridge 与 BGFX ImGui renderer 在 `Inno.Adapter.Authoring.Default` 内组合，不能出现在 Host 的字段、参数、返回值或 using 中。

```csharp
using IPresentationContext presentation = authoringCatalog.presentation.CreateContext(
    PresentationBackendId.imGui,
    options);
```

Presentation 是 Application 生命周期资源，必须先于 shared render device 释放。

新增实现继承 `PresentationBackendProvider`，由组合入口赋予稳定 ID 并注册到 catalog。
`CreateContext` 的调用方拥有返回的 context；provider 不接管 options 中的平台与渲染资源。
未知 ID 抛出 `NotSupportedException`，provider 返回 null 则抛出 `InvalidOperationException`。
`DefaultAuthoringAdapterCatalog` 的 `presentationProviders` 为 null 时注册内置 ImGui，空序列明确注册零个实现。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Adapter.Presentation.IAuthoringAdapterCatalog`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Presentation.IAuthoringAdapterCatalog`](../../src/adapters/presentation/Inno.Adapter.Presentation/IAuthoringAdapterCatalog.cs#L8) | Extends the runtime adapter catalog with tooling and presentation factories required by authoring products. |
| [`Inno.Adapter.Presentation.IPresentationBackendFactory Inno.Adapter.Presentation.IAuthoringAdapterCatalog.presentation`](../../src/adapters/presentation/Inno.Adapter.Presentation/IAuthoringAdapterCatalog.cs#L18) | Gets the graphical host-presentation factory used by authoring products. |
| [`Inno.Adapter.Rendering.IRenderingAuthoringBackendFactory Inno.Adapter.Presentation.IAuthoringAdapterCatalog.renderingAuthoring`](../../src/adapters/presentation/Inno.Adapter.Presentation/IAuthoringAdapterCatalog.cs#L13) | Gets the rendering toolchain factory used to compile authoring artifacts. |

### `Inno.Adapter.Presentation.IPresentationBackendFactory`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Presentation.IPresentationBackendFactory`](../../src/adapters/presentation/Inno.Adapter.Presentation/IPresentationBackendFactory.cs#L8) | Creates host presentation contexts from explicit backend selections. |
| [`Inno.Adapter.Presentation.IPresentationContext Inno.Adapter.Presentation.IPresentationBackendFactory.CreateContext(Inno.Adapter.Presentation.PresentationBackendId backend, Inno.Adapter.Presentation.PresentationBackendOptions options)`](../../src/adapters/presentation/Inno.Adapter.Presentation/IPresentationBackendFactory.cs#L30) | Creates a presentation context over compatible platform and rendering adapters. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Adapter.Presentation.PresentationBackendId> Inno.Adapter.Presentation.IPresentationBackendFactory.supportedBackends`](../../src/adapters/presentation/Inno.Adapter.Presentation/IPresentationBackendFactory.cs#L13) | Gets the implementation identities available in this immutable factory snapshot. |

### `Inno.Adapter.Presentation.IPresentationContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Presentation.IPresentationContext`](../../src/adapters/presentation/Inno.Adapter.Presentation/IPresentationContext.cs#L10) | Defines the backend-neutral lifecycle used by a graphical composition host. |
| [`Inno.Adapter.Presentation.PresentationTextureHandle Inno.Adapter.Presentation.IPresentationContext.RegisterTexture(Inno.Rendering.PersistentTextureHandle texture)`](../../src/adapters/presentation/Inno.Adapter.Presentation/IPresentationContext.cs#L79) | Registers a persistent render texture for presentation drawing. |
| [`bool Inno.Adapter.Presentation.IPresentationContext.TryCaptureLayout(out string settings, bool force = false)`](../../src/adapters/presentation/Inno.Adapter.Presentation/IPresentationContext.cs#L57) | Captures complete layout text when persistence is required. |
| [`bool Inno.Adapter.Presentation.IPresentationContext.TryGetWindowId(uint viewportId, out uint windowId)`](../../src/adapters/presentation/Inno.Adapter.Presentation/IPresentationContext.cs#L24) | Resolves the platform window that owns a presentation viewport. |
| [`bool Inno.Adapter.Presentation.IPresentationContext.UnregisterTexture(Inno.Adapter.Presentation.PresentationTextureHandle texture)`](../../src/adapters/presentation/Inno.Adapter.Presentation/IPresentationContext.cs#L90) | Releases a presentation token without taking ownership of the source texture. |
| [`void Inno.Adapter.Presentation.IPresentationContext.DrawImage(Inno.Adapter.Presentation.PresentationTextureHandle texture, System.Numerics.Vector2 size)`](../../src/adapters/presentation/Inno.Adapter.Presentation/IPresentationContext.cs#L101) | Draws a registered texture in the active presentation surface. |
| [`void Inno.Adapter.Presentation.IPresentationContext.LoadLayout(string? settings)`](../../src/adapters/presentation/Inno.Adapter.Presentation/IPresentationContext.cs#L43) | Loads complete layout text captured by the host. |
| [`void Inno.Adapter.Presentation.IPresentationContext.RenderFrame(System.Action drawFrame)`](../../src/adapters/presentation/Inno.Adapter.Presentation/IPresentationContext.cs#L68) | Builds one presentation frame by invoking the host draw callback. |
| [`void Inno.Adapter.Presentation.IPresentationContext.SetLayoutFile(string? filePath)`](../../src/adapters/presentation/Inno.Adapter.Presentation/IPresentationContext.cs#L35) | Disables or redirects backend-owned layout-file persistence. |

### `Inno.Adapter.Presentation.PresentationBackendCatalog`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Presentation.IPresentationContext Inno.Adapter.Presentation.PresentationBackendCatalog.CreateContext(Inno.Adapter.Presentation.PresentationBackendId backend, Inno.Adapter.Presentation.PresentationBackendOptions options)`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendCatalog.cs#L44) | See the implemented contract. |
| [`Inno.Adapter.Presentation.PresentationBackendCatalog`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendCatalog.cs#L10) | Resolves presentation providers from one immutable, composition-owned registration snapshot. |
| [`Inno.Adapter.Presentation.PresentationBackendCatalog.PresentationBackendCatalog(System.Collections.Generic.IEnumerable<Inno.Adapter.Presentation.PresentationBackendProvider> providers)`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendCatalog.cs#L26) | Validates and captures a complete provider set without creating any service. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Adapter.Presentation.PresentationBackendId> Inno.Adapter.Presentation.PresentationBackendCatalog.supportedBackends`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendCatalog.cs#L38) | See the implemented contract. |

### `Inno.Adapter.Presentation.PresentationBackendId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Presentation.PresentationBackendId`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendId.cs#L8) | Identifies a presentation implementation without closing the set of supported backends. |
| [`Inno.Adapter.Presentation.PresentationBackendId.PresentationBackendId(string value)`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendId.cs#L19) | Creates an ordinal, case-sensitive implementation identifier. |
| [`bool Inno.Adapter.Presentation.PresentationBackendId.isValid`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendId.cs#L41) | Gets whether this value identifies an implementation. |
| [`override string Inno.Adapter.Presentation.PresentationBackendId.ToString()`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendId.cs#L49) | Returns the identifier without resolving a provider. |
| [`static Inno.Adapter.Presentation.PresentationBackendId Inno.Adapter.Presentation.PresentationBackendId.imGui`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendId.cs#L31) | Gets the identifier of the bundled ImGui implementation. |
| [`string Inno.Adapter.Presentation.PresentationBackendId.value`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendId.cs#L36) | Gets the stable identifier; a default value is unassigned. |

### `Inno.Adapter.Presentation.PresentationBackendOptions`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Presentation.PresentationBackendOptions`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendOptions.cs#L9) | Collects the validated presentation backend options values that configure one owned operation. |
| [`Inno.Adapter.Presentation.PresentationFeatures Inno.Adapter.Presentation.PresentationBackendOptions.features`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendOptions.cs#L29) | Gets or sets optional presentation features requested by the host. |
| [`required Inno.Platform.IPlatformApplication Inno.Adapter.Presentation.PresentationBackendOptions.platformApplication`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendOptions.cs#L14) | Gets or sets the platform application that owns presentation windows and events. |
| [`required Inno.Platform.IPlatformWindow Inno.Adapter.Presentation.PresentationBackendOptions.window`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendOptions.cs#L19) | Gets or sets the primary presentation window. |
| [`required Inno.Rendering.IRenderDevice Inno.Adapter.Presentation.PresentationBackendOptions.renderDevice`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendOptions.cs#L24) | Gets or sets the rendering device used to present host draw data. |

### `Inno.Adapter.Presentation.PresentationBackendProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Presentation.PresentationBackendId Inno.Adapter.Presentation.PresentationBackendProvider.id`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendProvider.cs#L29) | Gets this registration's immutable implementation identity. |
| [`Inno.Adapter.Presentation.PresentationBackendProvider`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendProvider.cs#L8) | Describes an explicitly composed host presentation implementation and its creation boundary. |
| [`Inno.Adapter.Presentation.PresentationBackendProvider.PresentationBackendProvider(Inno.Adapter.Presentation.PresentationBackendId id)`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendProvider.cs#L19) | Captures the implementation identity assigned by the composition owner. |
| [`abstract Inno.Adapter.Presentation.IPresentationContext Inno.Adapter.Presentation.PresentationBackendProvider.CreateContext(Inno.Adapter.Presentation.PresentationBackendOptions options)`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendProvider.cs#L44) | Creates a new caller-owned presentation context over the supplied host resources. |

### `Inno.Adapter.Presentation.PresentationFeatures`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Presentation.PresentationFeatures`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationFeatures.cs#L8) | Describes optional capabilities requested from a host presentation backend. |
| [`Inno.Adapter.Presentation.PresentationFeatures.Docking`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationFeatures.cs#L24) | Enables dockable host surfaces when supported. |
| [`Inno.Adapter.Presentation.PresentationFeatures.MultipleWindows`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationFeatures.cs#L19) | Allows the presentation to create detached native windows. |
| [`Inno.Adapter.Presentation.PresentationFeatures.None`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationFeatures.cs#L14) | Requests no optional presentation features. |
| [`Inno.Adapter.Presentation.PresentationFeatures.SmoothResize`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationFeatures.cs#L29) | Enables smooth rendering while native windows are being resized. |

### `Inno.Adapter.Presentation.PresentationTextureHandle`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Presentation.PresentationTextureHandle`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationTextureHandle.cs#L8) | Identifies a renderer-owned texture through an opaque host-presentation token. |
| [`Inno.Adapter.Presentation.PresentationTextureHandle.PresentationTextureHandle(ulong value)`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationTextureHandle.cs#L16) | Creates an opaque presentation texture token. |
| [`bool Inno.Adapter.Presentation.PresentationTextureHandle.isValid`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationTextureHandle.cs#L31) | Gets whether this token identifies a registered presentation texture. |
| [`ulong Inno.Adapter.Presentation.PresentationTextureHandle.value`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationTextureHandle.cs#L26) | Gets the opaque backend token. |

## 项目依赖

- [Inno.Adapter](../runtime/Inno.Adapter.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Rendering.Authoring](../rendering/Inno.Adapter.Rendering.Authoring.md)：公开引用边界由实际签名核对。
- [Inno.Platform](Inno.Platform.md)：公开引用边界由实际签名核对。
- [Inno.Rendering](../rendering/Inno.Rendering.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
