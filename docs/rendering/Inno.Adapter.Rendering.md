# Inno.Adapter.Rendering

[Rendering 索引](README.md) · [中立 Rendering](Inno.Rendering.md) · [BGFX implementation](../backends/Bgfx/Inno.Adapter.Rendering.Bgfx.md)

该项目定义 Rendering Adapter family，不引用 BGFX Native。

## 公开 API

- `RenderingBackendId`：开放、区分大小写的稳定实现标识；`bgfx` 是默认值，不是支持名单。默认 struct 值未赋值，创建设备前拒绝。
- `RenderingBackendProvider`：一个实现的 `id`、`CreateDevice(options)` 与 `CreateCompositionProgramProvider()`，由当前 Composition 持有。
- `RenderingBackendCatalog`：捕获完整 provider 集合，拒绝重复/空 ID；只解析明确注册的后端，不做静默替换。
- `RenderingBackendOptions`：中立 window、graphics API preference、VSync、sRGB 与 threading policy。
- `IRenderingBackendFactory.supportedBackends/CreateDevice/CreateCompositionProgramProvider`：Player 与 Shell 使用的 runtime-only factory。Shell 在初始化窗口前检查所选 ID；Editor 与 Player 通过同一注册后端取得图层合成程序供给器。
Authoring compiler factory 被隔离在 [Inno.Adapter.Rendering.Authoring](Inno.Adapter.Rendering.Authoring.md)。
`Inno.Adapter.Default` 只依赖本项目，防止 Player 闭包间接带入 `Inno.Rendering.Assets`、AssetPipeline 与 Build Toolchain。

```csharp
IRenderDevice device = catalog.rendering.CreateDevice(
    selection.rendering,
    new RenderingBackendOptions { window = window });
```

BGFX handle、view ID、native enum 和 compiler executable path 不得进入这些公开契约。

自定义后端通过 `new RenderingBackendId("studio.rendering.custom")` 与派生 provider 注册；不修改公共枚举或中央分支。
`DefaultAdapterCatalogOptions.rendering` 必須接收完整 `IRenderingBackendFactory`，不隐式选择 BGFX。Provider 属于 Composition generation，不在跨代静态缓存中保留。

## 注册身份

`RenderingBackendProvider` 的 protected 构造函数接收对应领域的 backend ID，并公开只读 `id`。ID 由 composition 分配，不能 override 或从临时创建的设备推导；构造拒绝未赋值 ID。此 provider 是显式 composition 注册，不进行类型发现。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Adapter.Rendering.IRenderingBackendFactory`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Rendering.IRenderingBackendFactory`](../../src/adapters/rendering/Inno.Adapter.Rendering/IRenderingBackendFactory.cs#L10) | Creates runtime rendering devices from one backend selection. |
| [`Inno.Rendering.IRenderDevice Inno.Adapter.Rendering.IRenderingBackendFactory.CreateDevice(Inno.Adapter.Rendering.RenderingBackendId backend, Inno.Adapter.Rendering.RenderingBackendOptions options)`](../../src/adapters/rendering/Inno.Adapter.Rendering/IRenderingBackendFactory.cs#L29) | Creates a rendering device for the supplied primary presentation surface. |
| [`Inno.Rendering.IRenderLayerCompositionProgramProvider Inno.Adapter.Rendering.IRenderingBackendFactory.CreateCompositionProgramProvider(Inno.Adapter.Rendering.RenderingBackendId backend)`](../../src/adapters/rendering/Inno.Adapter.Rendering/IRenderingBackendFactory.cs#L46) | Creates the composition program provider owned by the selected rendering backend. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Adapter.Rendering.RenderingBackendId> Inno.Adapter.Rendering.IRenderingBackendFactory.supportedBackends`](../../src/adapters/rendering/Inno.Adapter.Rendering/IRenderingBackendFactory.cs#L15) | Gets the exact runtime backend identities available in this composition generation. |

### `Inno.Adapter.Rendering.RenderingBackendCatalog`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Rendering.RenderingBackendCatalog`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendCatalog.cs#L11) | Resolves runtime providers from one immutable composition-owned registration snapshot. |
| [`Inno.Adapter.Rendering.RenderingBackendCatalog.RenderingBackendCatalog(System.Collections.Generic.IEnumerable<Inno.Adapter.Rendering.RenderingBackendProvider> providers)`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendCatalog.cs#L24) | Validates and captures a complete set of runtime providers. |
| [`Inno.Rendering.IRenderDevice Inno.Adapter.Rendering.RenderingBackendCatalog.CreateDevice(Inno.Adapter.Rendering.RenderingBackendId backend, Inno.Adapter.Rendering.RenderingBackendOptions options)`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendCatalog.cs#L55) | Creates a device using this implementation's validated inputs. |
| [`Inno.Rendering.IRenderLayerCompositionProgramProvider Inno.Adapter.Rendering.RenderingBackendCatalog.CreateCompositionProgramProvider(Inno.Adapter.Rendering.RenderingBackendId backend)`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendCatalog.cs#L78) | Resolves a backend-owned program provider for ordered render layer composition. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Adapter.Rendering.RenderingBackendId> Inno.Adapter.Rendering.RenderingBackendCatalog.supportedBackends`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendCatalog.cs#L38) | Gets backend registrations available in this type generation. |

### `Inno.Adapter.Rendering.RenderingBackendId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Rendering.RenderingBackendId`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendId.cs#L8) | Identifies a rendering implementation independently from its graphics API and compiler language. |
| [`Inno.Adapter.Rendering.RenderingBackendId.RenderingBackendId(string value)`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendId.cs#L19) | Creates an ordinal, case-sensitive backend identifier. |
| [`bool Inno.Adapter.Rendering.RenderingBackendId.isValid`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendId.cs#L41) | Gets whether this value identifies an implementation. |
| [`override string Inno.Adapter.Rendering.RenderingBackendId.ToString()`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendId.cs#L49) | Returns the identifier without resolving or creating a device. |
| [`static Inno.Adapter.Rendering.RenderingBackendId Inno.Adapter.Rendering.RenderingBackendId.bgfx`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendId.cs#L31) | Gets the identifier of the bundled BGFX implementation, not a whitelist of supported backends. |
| [`string Inno.Adapter.Rendering.RenderingBackendId.value`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendId.cs#L36) | Gets the stable implementation identifier; the default value is unassigned. |

### `Inno.Adapter.Rendering.RenderingBackendOptions`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Rendering.RenderingBackendOptions`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendOptions.cs#L9) | Configures backend-neutral rendering-device creation. |
| [`Inno.Platform.IPlatformWindow? Inno.Adapter.Rendering.RenderingBackendOptions.window`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendOptions.cs#L14) | Gets or sets the primary platform window used as the presentation surface. |
| [`Inno.Rendering.GraphicsApi? Inno.Adapter.Rendering.RenderingBackendOptions.preferredGraphicsApi`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendOptions.cs#L19) | Gets or sets the preferred graphics API, or for the backend default. |
| [`bool Inno.Adapter.Rendering.RenderingBackendOptions.forceSingleThreaded`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendOptions.cs#L34) | Gets or sets whether rendering must execute on the calling thread. |
| [`bool Inno.Adapter.Rendering.RenderingBackendOptions.sRgbBackbuffer`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendOptions.cs#L29) | Gets or sets whether the primary backbuffer performs sRGB encoding. |
| [`bool Inno.Adapter.Rendering.RenderingBackendOptions.verticalSync`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendOptions.cs#L24) | Gets or sets whether presentation waits for display synchronization. |

### `Inno.Adapter.Rendering.RenderingBackendProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Rendering.RenderingBackendId Inno.Adapter.Rendering.RenderingBackendProvider.id`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendProvider.cs#L30) | Gets the stable implementation identity paired with its authoring tools. |
| [`Inno.Adapter.Rendering.RenderingBackendProvider`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendProvider.cs#L9) | Supplies one rendering implementation registered by a composition owner. |
| [`Inno.Adapter.Rendering.RenderingBackendProvider.RenderingBackendProvider(Inno.Adapter.Rendering.RenderingBackendId id)`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendProvider.cs#L20) | Captures the registration identity assigned by the composition owner. |
| [`abstract Inno.Rendering.IRenderDevice Inno.Adapter.Rendering.RenderingBackendProvider.CreateDevice(Inno.Adapter.Rendering.RenderingBackendOptions options)`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendProvider.cs#L41) | Creates a caller-owned device using backend-neutral surface options. |
| [`abstract Inno.Rendering.IRenderLayerCompositionProgramProvider Inno.Adapter.Rendering.RenderingBackendProvider.CreateCompositionProgramProvider()`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendProvider.cs#L49) | Creates the backend-owned program provider used when the host composites render layers. |

## 项目依赖

- [Inno.Rendering](Inno.Rendering.md)：公开引用边界由实际签名核对。
- [Inno.Platform](../platform/Inno.Platform.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
