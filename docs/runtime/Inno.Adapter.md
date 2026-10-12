# Inno.Adapter

[Runtime 索引](README.md) · [Shell](Inno.Shell.md) · [默认 Runtime catalog](Inno.Adapter.Default.md)

`Inno.Adapter` 是所有 runtime backend family 的统一组合契约，本身不引用任何具体 implementation 或 authoring pipeline。

## 公开 API

- `AdapterSelection`：Platform、Input、Storage、Rendering、Audio、Text、UI 的开放稳定 ID 选择；没有隐式默认 backend。
- `IAdapterCatalog`：公开上述七个领域的中立 factory。

```csharp
using Inno.Adapter;

static void Validate(
    AdapterSelection selection,
    IAdapterCatalog catalog
) => selection.Validate(catalog);
```

`AdapterSelection` 只保存选择，不保存 native handle、实例或可热重载对象。Presentation 和 compiler 不属于 runtime catalog；它们由 [authoring catalog](Inno.Adapter.Authoring.Default.md) 独立扩展。

## Composition 预检

`AdapterSelection.Validate(IAdapterCatalog)` 在创建任何领域服务前校验 platform/input/storage/rendering/audio/text/ui 的完整选择；无效或缺失注册抛出 `NotSupportedException`。所有选择为开放的领域 backend ID，默认值未赋值，内置静态 ID 只表示默认实现。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Adapter.AdapterSelection`

<!-- api: T:Inno.Adapter.AdapterSelection 1c34fa201f3258cdcae25b9b6568c0fc4583f7a2122914c2e1908ca31b9d0517 -->
<!-- api: P:Inno.Adapter.AdapterSelection.platform 1dd2b788d5c5fd1d3f497767554f8a6b0ab2bfdd2bd3f173804d527f9d6f98b0 -->
<!-- api: P:Inno.Adapter.AdapterSelection.input 32d353b65fb4671e929222b2ac46fb035c2e0db49bc78c16e13bb11c480c9022 -->
<!-- api: P:Inno.Adapter.AdapterSelection.storage f6395f8c643eb5e2bcb14b545620ecd20a9b39ec055da0f7cf397dcfdb4fe1f6 -->
<!-- api: P:Inno.Adapter.AdapterSelection.rendering 72c76531b78062509770cb4576171c9ce5999571514e32a9963f395c0a5887b7 -->
<!-- api: P:Inno.Adapter.AdapterSelection.audio 3f76ea8d4313ecc25f626d3f978d8f2fe14915f5db8b084e2b3224fa36411bd5 -->
<!-- api: P:Inno.Adapter.AdapterSelection.text 5c8b7eda3f95a2c5ee086ffccacad0feba8516689a905cd6d3e0ba3caa2cc2c9 -->
<!-- api: P:Inno.Adapter.AdapterSelection.ui 7f5cda8cdd628d580a438656a208bbd126e91868d1fb2ba3063c26266852d563 -->
<!-- api: M:Inno.Adapter.AdapterSelection.Validate(Inno.Adapter.IAdapterCatalog) cb1520625393b719cc8a58193a47a01dab3ddb530289a87802f4480d6cc07f53 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-5a9caa2bed6f62d6"></a>[`public void Inno.Adapter.AdapterSelection.Validate(Inno.Adapter.IAdapterCatalog catalog)`](../../src/adapters/common/Inno.Adapter/AdapterSelection.cs#L66) | Validates every selected registration before the composition creates any service.<br>`catalog`：The complete, composition-owned factory snapshot. | — | `System.ArgumentNullException`：The catalog is null.<br>`System.NotSupportedException`：A selected implementation is unassigned or absent from its domain catalog. |
| <a id="api-0422a7737c486a4d"></a>[`public Inno.Adapter.Audio.AudioBackendId Inno.Adapter.AdapterSelection.audio { get; init; }`](../../src/adapters/common/Inno.Adapter/AdapterSelection.cs#L42) | Gets the selected audio backend. | — | — |
| <a id="api-479fd33d7c45f4a0"></a>[`public Inno.Adapter.Input.InputBackendId Inno.Adapter.AdapterSelection.input { get; init; }`](../../src/adapters/common/Inno.Adapter/AdapterSelection.cs#L27) | Gets the selected input backend. | — | — |
| <a id="api-3f782e568d6d125e"></a>[`public Inno.Adapter.Platform.PlatformBackendId Inno.Adapter.AdapterSelection.platform { get; init; }`](../../src/adapters/common/Inno.Adapter/AdapterSelection.cs#L22) | Gets the selected platform backend. | — | — |
| <a id="api-aaaa7b541cf7297e"></a>[`public Inno.Adapter.Rendering.RenderingBackendId Inno.Adapter.AdapterSelection.rendering { get; init; }`](../../src/adapters/common/Inno.Adapter/AdapterSelection.cs#L37) | Gets the selected rendering backend. | — | — |
| <a id="api-ab78ba3c2e0bb4bd"></a>[`public Inno.Adapter.Storage.StorageBackendId Inno.Adapter.AdapterSelection.storage { get; init; }`](../../src/adapters/common/Inno.Adapter/AdapterSelection.cs#L32) | Gets the selected storage backend. | — | — |
| <a id="api-c1902c578a2f2480"></a>[`public Inno.Adapter.Text.TextBackendId Inno.Adapter.AdapterSelection.text { get; init; }`](../../src/adapters/common/Inno.Adapter/AdapterSelection.cs#L47) | Gets the selected Unicode text backend. | — | — |
| <a id="api-909496a71ef5ec5a"></a>[`public Inno.Adapter.UI.UiBackendId Inno.Adapter.AdapterSelection.ui { get; init; }`](../../src/adapters/common/Inno.Adapter/AdapterSelection.cs#L52) | Gets the selected retained-mode UI backend. | — | — |
| <a id="api-70fd3f41598e06cf"></a>[`public readonly struct Inno.Adapter.AdapterSelection`](../../src/adapters/common/Inno.Adapter/AdapterSelection.cs#L17) | Selects one implementation for every replaceable host backend family. | — | — |

### `Inno.Adapter.IAdapterCatalog`

<!-- api: T:Inno.Adapter.IAdapterCatalog 960399d1adcba03826b41fb4f5930cd8e760c8b43551539ac4bb14f9080dded8 -->
<!-- api: P:Inno.Adapter.IAdapterCatalog.platform 0f2bbf30d1c7e7f992409c1d2d4d772a118da26a54239f1c1755d4cbb5d923b8 -->
<!-- api: P:Inno.Adapter.IAdapterCatalog.input 3ed59e5d5a35f299ebcdec357da0262ef9a5c4bffe15915a68ec00f4eeb58c15 -->
<!-- api: P:Inno.Adapter.IAdapterCatalog.storage 17a75ce300df9abbcb77454877bd0b8c5d0b2e913a736baec1d5d00db7e7fb90 -->
<!-- api: P:Inno.Adapter.IAdapterCatalog.rendering e2585ee5baf3eaf5a36d29952cd5662bb6d21b279fa8bd6f47d2beebab78ce83 -->
<!-- api: P:Inno.Adapter.IAdapterCatalog.audio a12418b5e2649d0e147227a88d3353ea09301f8d25ec988b394963bf65277054 -->
<!-- api: P:Inno.Adapter.IAdapterCatalog.text 182c0ddcc5152576048e2caf965f8fcca00b38f4d650419fa9e8ee648bb97b7f -->
<!-- api: P:Inno.Adapter.IAdapterCatalog.ui 1d281cb5578824e4855d6a8a02dae5a3d2b25e618b3efd23e069a0fa97361bae -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-5a4be95e78f04841"></a>[`Inno.Adapter.Audio.IAudioBackendFactory Inno.Adapter.IAdapterCatalog.audio { get; }`](../../src/adapters/common/Inno.Adapter/IAdapterCatalog.cs#L39) | Gets the audio backend factory. | — | — |
| <a id="api-1c749bae1eab2125"></a>[`Inno.Adapter.Input.IInputBackendFactory Inno.Adapter.IAdapterCatalog.input { get; }`](../../src/adapters/common/Inno.Adapter/IAdapterCatalog.cs#L24) | Gets the input backend factory. | — | — |
| <a id="api-2d913070f72468e7"></a>[`Inno.Adapter.Platform.IPlatformBackendFactory Inno.Adapter.IAdapterCatalog.platform { get; }`](../../src/adapters/common/Inno.Adapter/IAdapterCatalog.cs#L19) | Gets the platform backend factory. | — | — |
| <a id="api-20ade1ebcf5d89cd"></a>[`Inno.Adapter.Rendering.IRenderingBackendFactory Inno.Adapter.IAdapterCatalog.rendering { get; }`](../../src/adapters/common/Inno.Adapter/IAdapterCatalog.cs#L34) | Gets the rendering backend factory. | — | — |
| <a id="api-cc8d6208375b74a6"></a>[`Inno.Adapter.Storage.IStorageBackendFactory Inno.Adapter.IAdapterCatalog.storage { get; }`](../../src/adapters/common/Inno.Adapter/IAdapterCatalog.cs#L29) | Gets the application-storage backend factory. | — | — |
| <a id="api-531e40761315677e"></a>[`Inno.Adapter.Text.ITextBackendFactory Inno.Adapter.IAdapterCatalog.text { get; }`](../../src/adapters/common/Inno.Adapter/IAdapterCatalog.cs#L44) | Gets the Unicode text backend factory. | — | — |
| <a id="api-6af03297371105c2"></a>[`Inno.Adapter.UI.IUiBackendFactory Inno.Adapter.IAdapterCatalog.ui { get; }`](../../src/adapters/common/Inno.Adapter/IAdapterCatalog.cs#L49) | Gets the retained-mode UI backend factory. | — | — |
| <a id="api-b740dcad45f86782"></a>[`public interface Inno.Adapter.IAdapterCatalog`](../../src/adapters/common/Inno.Adapter/IAdapterCatalog.cs#L14) | Exposes factories for every replaceable host backend family without exposing implementations. | — | — |

## 项目依赖

- [Inno.Adapter.Audio](../audio/Inno.Adapter.Audio.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Input](../input/Inno.Adapter.Input.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Platform](../platform/Inno.Adapter.Platform.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Rendering](../rendering/Inno.Adapter.Rendering.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Storage](../storage/Inno.Adapter.Storage.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Text](../text/Inno.Adapter.Text.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.UI](../ui/Inno.Adapter.UI.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
