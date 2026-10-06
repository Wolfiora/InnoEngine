# Inno.Adapter.Default

[Runtime 索引](README.md) · [Adapter 契约](Inno.Adapter.md) · [Shell](Inno.Shell.md) · [Wiki 首页](../README.md)

## 职责与依赖

标准发行版的 composition catalog，组合七个独立领域的开放 provider 快照。默认实现为 SDL3 platform/input、FileSystem storage、BGFX rendering、MiniAudio audio、FreeType/HarfBuzz text 与 RmlUi UI。禁止依赖 Editor、Presentation、编译器、AssetPipeline 或 Build Toolchain。

## 全部公开 API

`DefaultAdapterCatalog(renderingProviders, uiProviders, storageProviders, platformProviders, inputProviders, audioProviders, textProviders)`：七个可选集合分别完整替换该领域的注册。只有 null 使用内置默认；空集合表示当前领域没有实现。构造只验证注册，不创建窗口、GPU 或音频设备。

`platform`、`input`、`storage`、`rendering`、`audio`、`text`、`ui` 返回各领域的中立 factory interface。所有 factory 暴露只读 `supportedBackends`；无效、重复或空 provider 拒绝注册；未注册 ID 和 null 创建结果明确失败。

没有 protected 扩展点。新增实现派生对应领域 provider，在 composition 构造时传入；ID 是显式配置的不可变注册值，不由抽象属性重复声明。类型发现扩展的 stable attribute 规则由发现协议管理。

## 使用与生命周期

```csharp
using Inno.Adapter;
using Inno.Adapter.Default;

IAdapterCatalog catalog = new DefaultAdapterCatalog();
AdapterSelection selection = AdapterSelection.defaultValue;
selection.Validate(catalog);
```

`Validate` 在初始化原生服务前检查全部选择。Provider 属于 composition 生命周期；catalog 不负责释放 provider；每次创建返回的服务由调用方拥有和释放。Provider 不存入跨 generation 的全局缓存。

浏览器入口传入 `storageProviders: [new BrowserStorageBackendProvider()]`，同时选择 `StorageBackendId.browser`；桌面选择 `StorageBackendId.fileSystem`。不同实现保持明确 ID，同一领域服务 API、输入和 Player 生命周期保持共用。

## 当前源码公开 API 清单

以下仅列出当前程序集自己声明的 public/protected 契约；继承成员遵循所属基类页面。internal/private 实现不作为稳定公开 API。签名依据当前源码语义模型生成，行为、参数、异常与所有权说明同时以对应英文 XML 为准。

### `Inno.Adapter.Default.DefaultAdapterCatalog`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Default.DefaultAdapterCatalog.DefaultAdapterCatalog(Inno.Adapter.Default.DefaultAdapterCatalogOptions options, System.Collections.Generic.IEnumerable<Inno.Adapter.Rendering.RenderingBackendProvider>? renderingProviders = null, System.Collections.Generic.IEnumerable<Inno.Adapter.UI.UiBackendProvider>? uiProviders = null, System.Collections.Generic.IEnumerable<Inno.Adapter.Platform.PlatformBackendProvider>? platformProviders = null, System.Collections.Generic.IEnumerable<Inno.Adapter.Input.InputBackendProvider>? inputProviders = null, System.Collections.Generic.IEnumerable<Inno.Adapter.Audio.AudioBackendProvider>? audioProviders = null, System.Collections.Generic.IEnumerable<Inno.Adapter.Text.TextBackendProvider>? textProviders = null)`](../../src/adapters/default/Inno.Adapter.Default/DefaultAdapterCatalog.cs#L60) | Captures each domain registration snapshot without initializing native services. |
| [`Inno.Adapter.Audio.IAudioBackendFactory Inno.Adapter.Default.DefaultAdapterCatalog.audio`](../../src/adapters/default/Inno.Adapter.Default/DefaultAdapterCatalog.cs#L102) | Gets the composition-owned audio factory snapshot. |
| [`Inno.Adapter.Input.IInputBackendFactory Inno.Adapter.Default.DefaultAdapterCatalog.input`](../../src/adapters/default/Inno.Adapter.Default/DefaultAdapterCatalog.cs#L87) | Gets the composition-owned input factory snapshot. |
| [`Inno.Adapter.Platform.IPlatformBackendFactory Inno.Adapter.Default.DefaultAdapterCatalog.platform`](../../src/adapters/default/Inno.Adapter.Default/DefaultAdapterCatalog.cs#L82) | Gets the composition-owned platform factory snapshot. |
| [`Inno.Adapter.Rendering.IRenderingBackendFactory Inno.Adapter.Default.DefaultAdapterCatalog.rendering`](../../src/adapters/default/Inno.Adapter.Default/DefaultAdapterCatalog.cs#L97) | Gets the composition-owned rendering factory snapshot. |
| [`Inno.Adapter.Storage.IStorageBackendFactory Inno.Adapter.Default.DefaultAdapterCatalog.storage`](../../src/adapters/default/Inno.Adapter.Default/DefaultAdapterCatalog.cs#L92) | Gets the composition-owned storage factory snapshot. |
| [`Inno.Adapter.Text.ITextBackendFactory Inno.Adapter.Default.DefaultAdapterCatalog.text`](../../src/adapters/default/Inno.Adapter.Default/DefaultAdapterCatalog.cs#L107) | Gets the composition-owned text factory snapshot. |
| [`Inno.Adapter.UI.IUiBackendFactory Inno.Adapter.Default.DefaultAdapterCatalog.ui`](../../src/adapters/default/Inno.Adapter.Default/DefaultAdapterCatalog.cs#L112) | Gets the composition-owned ui factory snapshot. |
| [`Inno.Adapter.Default.DefaultAdapterCatalog`](../../src/adapters/default/Inno.Adapter.Default/DefaultAdapterCatalog.cs#L26) | Composes independent, open provider catalogs for the standard engine distribution. |

### `Inno.Adapter.Default.DefaultAdapterCatalogOptions`

| 当前声明 | 行为 |
| --- | --- |
| [`required Inno.Adapter.Storage.IStorageBackendFactory Inno.Adapter.Default.DefaultAdapterCatalogOptions.storage`](../../src/adapters/default/Inno.Adapter.Default/DefaultAdapterCatalogOptions.cs#L13) | Gets or initializes the immutable storage factory configured for this host. |
| [`Inno.Adapter.Default.DefaultAdapterCatalogOptions`](../../src/adapters/default/Inno.Adapter.Default/DefaultAdapterCatalogOptions.cs#L8) | Supplies host-owned services whose locations cannot be inferred by the engine distribution. |

## 项目依赖

- [Inno.Adapter](Inno.Adapter.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Adapter.Audio.MiniAudio](../audio/Inno.Adapter.Audio.MiniAudio.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Adapter.Input](../input/Inno.Adapter.Input.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Adapter.Platform.Sdl3](../platform/Inno.Adapter.Platform.Sdl3.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Adapter.Rendering.Bgfx](../rendering/Inno.Adapter.Rendering.Bgfx.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Adapter.Text.FreeTypeHarfBuzz](../text/Inno.Adapter.Text.FreeTypeHarfBuzz.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Adapter.UI.RmlUi](../ui/Inno.Adapter.UI.RmlUi.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Adapter.Text](../text/Inno.Adapter.Text.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Adapter.UI](../ui/Inno.Adapter.UI.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Text](../text/Inno.Text.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.UI](../ui/Inno.UI.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：项目引用；公开签名可见性由语义边界检查确认。

共同 MSBuild 注入的 analyzer 与编译规则属于构建依赖，完整有效项目图记录在本轮验收证据中。
