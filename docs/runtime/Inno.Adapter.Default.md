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
