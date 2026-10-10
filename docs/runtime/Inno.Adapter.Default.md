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

浏览器入口在 options 中注入包含 BrowserStorageBackendProvider 的 storage factory，同时选择 `StorageBackendId.browser`；桌面选择 `StorageBackendId.fileSystem`。不同实现保持明确 ID，同一领域服务 API、输入和 Player 生命周期保持共用。

## 显式运行实现选择

DefaultAdapterCatalogOptions 必须提供 platform、rendering、storage factory。Catalog 不创建 BGFX provider 或 platform surface integration，项目不引用 BGFX runtime 实现。平台产品明确注入 matching SDL 与 BGFX runtime integration；自定义 rendering factory 可直接组合。

## 当前源码公开 API 清单

只列当前源码 public/protected 表面；内部机制不是稳定 API，参数、返回、失败及所有权以英文 XML 为准。

### `Inno.Adapter.Default.DefaultAdapterCatalog`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Audio.IAudioBackendFactory Inno.Adapter.Default.DefaultAdapterCatalog.audio`](../../src/composition/adapters/Inno.Adapter.Default/DefaultAdapterCatalog.cs#L92) | Gets the composition-owned audio factory snapshot. |
| [`Inno.Adapter.Default.DefaultAdapterCatalog`](../../src/composition/adapters/Inno.Adapter.Default/DefaultAdapterCatalog.cs#L24) | Composes independent, open provider catalogs for the standard engine distribution. |
| [`Inno.Adapter.Default.DefaultAdapterCatalog.DefaultAdapterCatalog(Inno.Adapter.Default.DefaultAdapterCatalogOptions options, System.Collections.Generic.IEnumerable<Inno.Adapter.UI.UiBackendProvider>? uiProviders = null, System.Collections.Generic.IEnumerable<Inno.Adapter.Input.InputBackendProvider>? inputProviders = null, System.Collections.Generic.IEnumerable<Inno.Adapter.Audio.AudioBackendProvider>? audioProviders = null, System.Collections.Generic.IEnumerable<Inno.Adapter.Text.TextBackendProvider>? textProviders = null)`](../../src/composition/adapters/Inno.Adapter.Default/DefaultAdapterCatalog.cs#L52) | Captures each domain registration snapshot without initializing native services. |
| [`Inno.Adapter.Input.IInputBackendFactory Inno.Adapter.Default.DefaultAdapterCatalog.input`](../../src/composition/adapters/Inno.Adapter.Default/DefaultAdapterCatalog.cs#L77) | Gets the composition-owned input factory snapshot. |
| [`Inno.Adapter.Platform.IPlatformBackendFactory Inno.Adapter.Default.DefaultAdapterCatalog.platform`](../../src/composition/adapters/Inno.Adapter.Default/DefaultAdapterCatalog.cs#L72) | Gets the composition-owned platform factory snapshot. |
| [`Inno.Adapter.Rendering.IRenderingBackendFactory Inno.Adapter.Default.DefaultAdapterCatalog.rendering`](../../src/composition/adapters/Inno.Adapter.Default/DefaultAdapterCatalog.cs#L87) | Gets the composition-owned rendering factory snapshot. |
| [`Inno.Adapter.Storage.IStorageBackendFactory Inno.Adapter.Default.DefaultAdapterCatalog.storage`](../../src/composition/adapters/Inno.Adapter.Default/DefaultAdapterCatalog.cs#L82) | Gets the composition-owned storage factory snapshot. |
| [`Inno.Adapter.Text.ITextBackendFactory Inno.Adapter.Default.DefaultAdapterCatalog.text`](../../src/composition/adapters/Inno.Adapter.Default/DefaultAdapterCatalog.cs#L97) | Gets the composition-owned text factory snapshot. |
| [`Inno.Adapter.UI.IUiBackendFactory Inno.Adapter.Default.DefaultAdapterCatalog.ui`](../../src/composition/adapters/Inno.Adapter.Default/DefaultAdapterCatalog.cs#L102) | Gets the composition-owned ui factory snapshot. |

### `Inno.Adapter.Default.DefaultAdapterCatalogOptions`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Default.DefaultAdapterCatalogOptions`](../../src/composition/adapters/Inno.Adapter.Default/DefaultAdapterCatalogOptions.cs#L10) | Supplies host-owned services whose locations cannot be inferred by the engine distribution. |
| [`required Inno.Adapter.Platform.IPlatformBackendFactory Inno.Adapter.Default.DefaultAdapterCatalogOptions.platform`](../../src/composition/adapters/Inno.Adapter.Default/DefaultAdapterCatalogOptions.cs#L15) | Gets or initializes the explicitly selected platform factory and host integration. |
| [`required Inno.Adapter.Rendering.IRenderingBackendFactory Inno.Adapter.Default.DefaultAdapterCatalogOptions.rendering`](../../src/composition/adapters/Inno.Adapter.Default/DefaultAdapterCatalogOptions.cs#L25) | Gets or initializes the explicitly selected rendering factory and its host surface integration. |
| [`required Inno.Adapter.Storage.IStorageBackendFactory Inno.Adapter.Default.DefaultAdapterCatalogOptions.storage`](../../src/composition/adapters/Inno.Adapter.Default/DefaultAdapterCatalogOptions.cs#L20) | Gets or initializes the immutable storage factory configured for this host. |

### `Inno.Adapter.Default.StandardAdapterSelection`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Default.StandardAdapterSelection`](../../src/composition/adapters/Inno.Adapter.Default/StandardAdapterSelection.cs#L14) | Explicitly selects the shared backends shipped by the standard runtime composition. |
| [`static Inno.Adapter.AdapterSelection Inno.Adapter.Default.StandardAdapterSelection.Create(Inno.Adapter.Storage.StorageBackendId storage)`](../../src/composition/adapters/Inno.Adapter.Default/StandardAdapterSelection.cs#L25) | Creates a complete selection while leaving the location-dependent storage choice to the product. |

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
