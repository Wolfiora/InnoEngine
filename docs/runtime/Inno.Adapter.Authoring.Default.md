# Inno.Adapter.Authoring.Default

[Runtime 索引](README.md) · [Runtime catalog](Inno.Adapter.Default.md) · [Presentation contract](../platform/Inno.Adapter.Presentation.md)

该项目是默认 Editor 的 authoring implementation catalog。`DefaultAuthoringAdapterCatalog` 委托 `DefaultAdapterCatalog` 提供 runtime families，并额外实现：

- `IRenderingAuthoringBackendFactory`：组合入口明确传入的 authoring providers。
- `IPresentationBackendFactory`：由内部 `PresentationBackendCatalog` 提供，组合入口明确传入 presentation providers；产品选择 SDL3 ImGui 与 BGFX 时也由产品注入交互配置和 renderer。

EditorHost 只依赖 `IAuthoringAdapterCatalog`，不会看到 `Sdl3PlatformApplication`、`BgfxDevice`、`MiniAudioDevice` 或 `PlatformImGuiContext`。该 catalog 属于 Editor/authoring 发布闭包，不得进入 Player closure。

```csharp
using System.Collections.Generic;
using Inno.Adapter;
using Inno.Adapter.Authoring.Default;
using Inno.Adapter.Default;
using Inno.Adapter.Presentation;
using Inno.Adapter.Rendering;

static IAuthoringAdapterCatalog Compose(
    DefaultAdapterCatalogOptions options,
    IEnumerable<RenderingAuthoringBackendProvider> authoring,
    IEnumerable<PresentationBackendProvider> presentation
) => new DefaultAuthoringAdapterCatalog(options, authoring, presentation);
```

options 必须明确提供 Platform、Rendering 和 Storage factory；authoring 和 presentation 集合均必填，空集合表示不安装该领域 provider。RenderingAuthoringBackendCatalog 在创建 GPU 设备前校验 runtime 与 authoring Backend ID 完全配对；重复、缺失或 null 明确失败。标准平台产品选择 BGFX 与 ImGui，自定义产品可以传入其他实现。

Presentation 创建是原子操作：shader、renderer 或 context 任一步失败都释放已创建资源并让 Editor 启动失败；不会发布半初始化 context。
Presentation 使用开放 `PresentationBackendId`，通过 `catalog.presentation.supportedBackends` 查看冻结支持集合，
`catalog.presentation.CreateContext(id, options)` 按稳定 ID 创建上下文。重复、未分配或空 provider、未知 ID 和空返回值明确失败。

## 构建和运行选择分开

Authoring catalog 借用显式运行选项，并接收实际 authoring/presentation provider。BGFX runtime integration 属于平台产品，Shader/Native 构建 integration 属于 distribution，不进入 Player 运行闭包。

## 当前源码公开 API 清单

只列当前源码 public/protected 表面；内部机制不是稳定 API，参数、返回、失败及所有权以英文 XML 为准。

### `Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Audio.IAudioBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.audio`](../../src/composition/adapters/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L83) | Gets the built-in audio backend factory. |
| [`Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog`](../../src/composition/adapters/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L20) | Combines explicitly supplied rendering toolchains and standard ImGui presentation to the built-in runtime adapter catalog. |
| [`Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.DefaultAuthoringAdapterCatalog(Inno.Adapter.Default.DefaultAdapterCatalogOptions options, System.Collections.Generic.IEnumerable<Inno.Adapter.Rendering.RenderingAuthoringBackendProvider> authoringProviders, System.Collections.Generic.IEnumerable<Inno.Adapter.Presentation.PresentationBackendProvider> presentationProviders)`](../../src/composition/adapters/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L43) | Creates paired runtime and authoring registrations before any native device is initialized. |
| [`Inno.Adapter.Input.IInputBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.input`](../../src/composition/adapters/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L68) | Gets the built-in input backend factory. |
| [`Inno.Adapter.Platform.IPlatformBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.platform`](../../src/composition/adapters/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L63) | Gets the built-in platform backend factory. |
| [`Inno.Adapter.Presentation.IPresentationBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.presentation`](../../src/composition/adapters/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L103) | Gets the built-in graphical host-presentation factory. |
| [`Inno.Adapter.Rendering.IRenderingAuthoringBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.renderingAuthoring`](../../src/composition/adapters/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L98) | Gets the built-in rendering authoring toolchain factory. |
| [`Inno.Adapter.Rendering.IRenderingBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.rendering`](../../src/composition/adapters/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L78) | Gets the built-in runtime rendering backend factory. |
| [`Inno.Adapter.Storage.IStorageBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.storage`](../../src/composition/adapters/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L73) | Gets the built-in application-storage backend factory. |
| [`Inno.Adapter.Text.ITextBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.text`](../../src/composition/adapters/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L88) | Gets the built-in Unicode text backend factory. |
| [`Inno.Adapter.UI.IUiBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.ui`](../../src/composition/adapters/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L93) | Gets the built-in retained-mode UI backend factory. |

## 项目依赖

- [Inno.Adapter](Inno.Adapter.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Default](Inno.Adapter.Default.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Text](../text/Inno.Adapter.Text.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.UI](../ui/Inno.Adapter.UI.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.UI.RmlUi.Authoring](../backends/RmlUi/Inno.Adapter.UI.RmlUi.Authoring.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Presentation.ImGui.Bgfx](../backends/ImGui/Inno.Adapter.Presentation.ImGui.Bgfx.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Rendering.Authoring](../rendering/Inno.Adapter.Rendering.Authoring.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
- [Inno.Rendering.Assets.Authoring](../rendering/Inno.Rendering.Assets.Authoring.md)：公开引用边界由实际签名核对。
