# Inno.Adapter.Default

[Runtime 索引](README.md) · [Adapter 契约](Inno.Adapter.md) · [Shell](Inno.Shell.md) · [Wiki 首页](../README.md)

## 职责与依赖

标准发行版的 composition catalog，组合七个独立领域的开放 provider 快照。Platform、Rendering 和 Storage factory 由产品明确注入；其余内置 provider 为 Core Events Input、MiniAudio、FreeType/HarfBuzz 与 RmlUi。禁止依赖 Editor、Presentation、编译器、AssetPipeline 或 Build Toolchain。

## 全部公开 API

`DefaultAdapterCatalog(options, uiProviders, inputProviders, audioProviders, textProviders)`：options 必填 platform/rendering/storage factory；四个可选 provider 集合分别完整替换对应领域。只有 null 使用内置 provider；空集合表示当前领域没有实现。构造只验证注册，不创建窗口、GPU 或音频设备。

`platform`、`input`、`storage`、`rendering`、`audio`、`text`、`ui` 返回各领域的中立 factory interface。所有 factory 暴露只读 `supportedBackends`；无效、重复或空 provider 拒绝注册；未注册 ID 和 null 创建结果明确失败。

没有 protected 扩展点。新增实现派生对应领域 provider，在 composition 构造时传入；ID 是显式配置的不可变注册值，不由抽象属性重复声明。类型发现扩展的 stable attribute 规则由发现协议管理。

## 使用与生命周期

```csharp
using Inno.Adapter;
using Inno.Adapter.Default;
using Inno.Adapter.Platform;
using Inno.Adapter.Rendering;
using Inno.Adapter.Storage;

static IAdapterCatalog Compose(
    IPlatformBackendFactory platform,
    IRenderingBackendFactory rendering,
    IStorageBackendFactory storage
) {
    var catalog = new DefaultAdapterCatalog(new DefaultAdapterCatalogOptions
    {
        platform = platform,
        rendering = rendering,
        storage = storage
    });
    AdapterSelection selection = StandardAdapterSelection.Create(StorageBackendId.fileSystem);
    selection.Validate(catalog);
    return catalog;
}
```

`Validate` 在初始化原生服务前检查全部选择。Provider 属于 composition 生命周期；catalog 不负责释放 provider；每次创建返回的服务由调用方拥有和释放。Provider 不存入跨 generation 的全局缓存。

浏览器入口在 options 中注入包含 BrowserWasm32StorageBackendProvider 的 storage factory，同时选择 `StorageBackendId.browser`；桌面选择 `StorageBackendId.fileSystem`。不同实现保持明确 ID，同一领域服务 API、输入和 Player 生命周期保持共用。

## 显式运行实现选择

DefaultAdapterCatalogOptions 必须提供 platform、rendering、storage factory。Catalog 不创建 BGFX provider 或 platform surface integration，项目不引用 BGFX runtime 实现。平台产品明确注入 matching SDL 与 BGFX runtime integration；自定义 rendering factory 可直接组合。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Adapter.Default.DefaultAdapterCatalog`

<!-- api: T:Inno.Adapter.Default.DefaultAdapterCatalog 4bb125c5dfb17cbeea103030a8c780e4192be4349437bd86c7949a44f74a3267 -->
<!-- api: M:Inno.Adapter.Default.DefaultAdapterCatalog.#ctor(Inno.Adapter.Default.DefaultAdapterCatalogOptions,System.Collections.Generic.IEnumerable{Inno.Adapter.UI.UiBackendProvider},System.Collections.Generic.IEnumerable{Inno.Adapter.Input.InputBackendProvider},System.Collections.Generic.IEnumerable{Inno.Adapter.Audio.AudioBackendProvider},System.Collections.Generic.IEnumerable{Inno.Adapter.Text.TextBackendProvider}) bec5c945967a014a6d565fa39be42a41905fbd3f1d0a80783cfcd2b508021049 -->
<!-- api: P:Inno.Adapter.Default.DefaultAdapterCatalog.platform f9ab20de440c8b1799f16356907a0990658ad19f8cec1971db26a07ea8210518 -->
<!-- api: P:Inno.Adapter.Default.DefaultAdapterCatalog.input fcfc70853c4a35efe1bea0c25a0c61adb167048920fa47073e62c07505b4e29e -->
<!-- api: P:Inno.Adapter.Default.DefaultAdapterCatalog.storage c4679ed64da36a8a9459f5702cbe962c08c176a52bb4501b6b9967ead6ea6411 -->
<!-- api: P:Inno.Adapter.Default.DefaultAdapterCatalog.rendering 6ce0abefe875cfae62deec9b95dbcc69f3a5d4583c5702ca66b28d7eb72cf949 -->
<!-- api: P:Inno.Adapter.Default.DefaultAdapterCatalog.audio 3f07dae7ab1f47c7e164175fb0d4c8743a56dbf75b270a9672681500a8b2c72c -->
<!-- api: P:Inno.Adapter.Default.DefaultAdapterCatalog.text ee3a2fa3ae5eb56691a7b693d3dbb910345b65af62cb95df85d7e5b7cf2697c1 -->
<!-- api: P:Inno.Adapter.Default.DefaultAdapterCatalog.ui 31667cfc098e9ad7f4047267d0987ec4e4660de0cc4a5087df2f92160f5527f0 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-62f14808d3ddb81f"></a>[`public Inno.Adapter.Default.DefaultAdapterCatalog.DefaultAdapterCatalog(Inno.Adapter.Default.DefaultAdapterCatalogOptions options, System.Collections.Generic.IEnumerable<Inno.Adapter.UI.UiBackendProvider>? uiProviders = null, System.Collections.Generic.IEnumerable<Inno.Adapter.Input.InputBackendProvider>? inputProviders = null, System.Collections.Generic.IEnumerable<Inno.Adapter.Audio.AudioBackendProvider>? audioProviders = null, System.Collections.Generic.IEnumerable<Inno.Adapter.Text.TextBackendProvider>? textProviders = null)`](../../src/composition/adapters/Inno.Adapter.Default/DefaultAdapterCatalog.cs#L52) | Captures each domain registration snapshot without initializing native services.<br>`options`：Explicit host configuration for location-dependent services.<br>`uiProviders`：Complete ui registrations, or null to use the bundled implementation.<br>`inputProviders`：Complete input registrations, or null to use the bundled implementation.<br>`audioProviders`：Complete audio registrations, or null to use the bundled implementation.<br>`textProviders`：Complete text registrations, or null to use the bundled implementation. | — | — |
| <a id="api-edfa8706a937ec14"></a>[`public Inno.Adapter.Audio.IAudioBackendFactory Inno.Adapter.Default.DefaultAdapterCatalog.audio { get; }`](../../src/composition/adapters/Inno.Adapter.Default/DefaultAdapterCatalog.cs#L92) | Gets the composition-owned audio factory snapshot. | — | — |
| <a id="api-734e1564342d3f06"></a>[`public Inno.Adapter.Input.IInputBackendFactory Inno.Adapter.Default.DefaultAdapterCatalog.input { get; }`](../../src/composition/adapters/Inno.Adapter.Default/DefaultAdapterCatalog.cs#L77) | Gets the composition-owned input factory snapshot. | — | — |
| <a id="api-6c71390f8d94e83d"></a>[`public Inno.Adapter.Platform.IPlatformBackendFactory Inno.Adapter.Default.DefaultAdapterCatalog.platform { get; }`](../../src/composition/adapters/Inno.Adapter.Default/DefaultAdapterCatalog.cs#L72) | Gets the composition-owned platform factory snapshot. | — | — |
| <a id="api-d26ea08f1a838f81"></a>[`public Inno.Adapter.Rendering.IRenderingBackendFactory Inno.Adapter.Default.DefaultAdapterCatalog.rendering { get; }`](../../src/composition/adapters/Inno.Adapter.Default/DefaultAdapterCatalog.cs#L87) | Gets the composition-owned rendering factory snapshot. | — | — |
| <a id="api-265d670beeb14995"></a>[`public Inno.Adapter.Storage.IStorageBackendFactory Inno.Adapter.Default.DefaultAdapterCatalog.storage { get; }`](../../src/composition/adapters/Inno.Adapter.Default/DefaultAdapterCatalog.cs#L82) | Gets the composition-owned storage factory snapshot. | — | — |
| <a id="api-40ad30925b8c6ca4"></a>[`public Inno.Adapter.Text.ITextBackendFactory Inno.Adapter.Default.DefaultAdapterCatalog.text { get; }`](../../src/composition/adapters/Inno.Adapter.Default/DefaultAdapterCatalog.cs#L97) | Gets the composition-owned text factory snapshot. | — | — |
| <a id="api-b461fc8b4131d9bf"></a>[`public Inno.Adapter.UI.IUiBackendFactory Inno.Adapter.Default.DefaultAdapterCatalog.ui { get; }`](../../src/composition/adapters/Inno.Adapter.Default/DefaultAdapterCatalog.cs#L102) | Gets the composition-owned ui factory snapshot. | — | — |
| <a id="api-074b9f3339c460a9"></a>[`public sealed class Inno.Adapter.Default.DefaultAdapterCatalog : Inno.Adapter.IAdapterCatalog`](../../src/composition/adapters/Inno.Adapter.Default/DefaultAdapterCatalog.cs#L24) | Composes independent, open provider catalogs for the standard engine distribution. | Replacement sequences define the complete registrations for their domain. The composition owns providers; service creation and disposal remain with each caller. | — |

### `Inno.Adapter.Default.DefaultAdapterCatalogOptions`

<!-- api: T:Inno.Adapter.Default.DefaultAdapterCatalogOptions e94de83350f7357fa32718f145dce40d225e59c82f38f65d4d91b8283db66dd9 -->
<!-- api: P:Inno.Adapter.Default.DefaultAdapterCatalogOptions.platform 3964aeeab94ed17d4bef4629ed82ba278a12f38e8f1dcf41d8f25b22f82afe81 -->
<!-- api: P:Inno.Adapter.Default.DefaultAdapterCatalogOptions.storage a6f9ec62afd8462ce8bd85cde84480129c6a82829100a6a35ac0535b65e0b6c0 -->
<!-- api: P:Inno.Adapter.Default.DefaultAdapterCatalogOptions.rendering 6f844e6b73cda218caeccb1fdfb6c043f24eebf15ea3536b515ec160a5301156 -->
<!-- api: M:Inno.Adapter.Default.DefaultAdapterCatalogOptions.#ctor 015be8ab004e9a9455c696ee7de7d38ea898161480ee509ab995716bdc1fef40 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e66bd6de6cc4a8dc"></a>[`public Inno.Adapter.Default.DefaultAdapterCatalogOptions.DefaultAdapterCatalogOptions()`](../../src/composition/adapters/Inno.Adapter.Default/DefaultAdapterCatalogOptions.cs#L10) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-9cc148de841de29e"></a>[`public required Inno.Adapter.Platform.IPlatformBackendFactory Inno.Adapter.Default.DefaultAdapterCatalogOptions.platform { get; init; }`](../../src/composition/adapters/Inno.Adapter.Default/DefaultAdapterCatalogOptions.cs#L15) | Gets or initializes the explicitly selected platform factory and host integration. | — | — |
| <a id="api-402e919d359a6e29"></a>[`public required Inno.Adapter.Rendering.IRenderingBackendFactory Inno.Adapter.Default.DefaultAdapterCatalogOptions.rendering { get; init; }`](../../src/composition/adapters/Inno.Adapter.Default/DefaultAdapterCatalogOptions.cs#L25) | Gets or initializes the explicitly selected rendering factory and its host surface integration. | — | — |
| <a id="api-44922d6de25b1cdd"></a>[`public required Inno.Adapter.Storage.IStorageBackendFactory Inno.Adapter.Default.DefaultAdapterCatalogOptions.storage { get; init; }`](../../src/composition/adapters/Inno.Adapter.Default/DefaultAdapterCatalogOptions.cs#L20) | Gets or initializes the immutable storage factory configured for this host. | — | — |
| <a id="api-05e7cb58d9bc5807"></a>[`public sealed class Inno.Adapter.Default.DefaultAdapterCatalogOptions`](../../src/composition/adapters/Inno.Adapter.Default/DefaultAdapterCatalogOptions.cs#L10) | Supplies host-owned services whose locations cannot be inferred by the engine distribution. | — | — |

### `Inno.Adapter.Default.StandardAdapterSelection`

<!-- api: T:Inno.Adapter.Default.StandardAdapterSelection 3750ba4667244be97c62a6b39e21851cec965960738838c595613337c7964825 -->
<!-- api: M:Inno.Adapter.Default.StandardAdapterSelection.Create(Inno.Adapter.Storage.StorageBackendId) e3afcde071676a1265810fc95f3c34a0831fccb5649c935e7cc2a4097f516348 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1a2a29232adef868"></a>[`public static Inno.Adapter.AdapterSelection Inno.Adapter.Default.StandardAdapterSelection.Create(Inno.Adapter.Storage.StorageBackendId storage)`](../../src/composition/adapters/Inno.Adapter.Default/StandardAdapterSelection.cs#L25) | Creates a complete selection while leaving the location-dependent storage choice to the product.<br>返回：A fully assigned standard selection; its catalog must still be validated before creating resources.<br>`storage`：The storage implementation explicitly registered by the product. | — | — |
| <a id="api-7295e25468fef3d1"></a>[`public static class Inno.Adapter.Default.StandardAdapterSelection`](../../src/composition/adapters/Inno.Adapter.Default/StandardAdapterSelection.cs#L14) | Explicitly selects the shared backends shipped by the standard runtime composition. | — | — |

## 项目依赖

- [Inno.Adapter](Inno.Adapter.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Audio.MiniAudio](../backends/MiniAudio/Inno.Adapter.Audio.MiniAudio.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Input](../input/Inno.Adapter.Input.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Text.FreeTypeHarfBuzz](../backends/Text/Inno.Adapter.Text.FreeTypeHarfBuzz.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.UI.RmlUi](../backends/RmlUi/Inno.Adapter.UI.RmlUi.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Text](../text/Inno.Adapter.Text.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.UI](../ui/Inno.Adapter.UI.md)：公开引用边界由实际签名核对。
- [Inno.Text](../text/Inno.Text.md)：公开引用边界由实际签名核对。
- [Inno.UI](../ui/Inno.UI.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
