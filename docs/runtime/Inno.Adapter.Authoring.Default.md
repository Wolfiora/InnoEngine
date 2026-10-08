# Inno.Adapter.Authoring.Default

[Runtime 索引](README.md) · [Runtime catalog](Inno.Adapter.Default.md) · [Presentation contract](../platform/Inno.Adapter.Presentation.md)

该项目是默认 Editor 的 authoring implementation catalog。`DefaultAuthoringAdapterCatalog` 委托 `DefaultAdapterCatalog` 提供 runtime families，并额外实现：

- `IRenderingAuthoringBackendFactory`：BGFX shaderc/texturec toolchain。
- `IPresentationBackendFactory`：由内部 `PresentationBackendCatalog` 提供，内置 provider 组合 SDL3 ImGui platform bridge 与 BGFX ImGui renderer。

EditorHost 只依赖 `IAuthoringAdapterCatalog`，不会看到 `Sdl3PlatformApplication`、`BgfxDevice`、`MiniAudioDevice` 或 `PlatformImGuiContext`。该 catalog 属于 Editor/authoring 发布闭包，不得进入 Player closure。

```csharp
IAuthoringAdapterCatalog catalog = new DefaultAuthoringAdapterCatalog();
```

构造参数 `renderingProviders`、`authoringProviders` 与 `presentationProviders` 分别接收完整 runtime、authoring 和 presentation provider 集合，
null 使用对应内置集合，空集合明确表示不安装该领域 provider。`RenderingAuthoringBackendCatalog` 在创建 GPU 设备前要求两边的
`RenderingBackendId` 集合完全配对，拒绝重复或缺失。Shader/Texture 工厂按稳定 ID 查找，不再维护
中央 Rendering enum switch。实际设备能力与 Shader 绑定/产物兼容性仍由各自后续验证负责。

Presentation 创建是原子操作：shader、renderer 或 context 任一步失败都释放已创建资源并让 Editor 启动失败；不会发布半初始化 context。
Presentation 使用开放 `PresentationBackendId`，通过 `catalog.presentation.supportedBackends` 查看冻结支持集合，
`catalog.presentation.CreateContext(id, options)` 按稳定 ID 创建上下文。重复、未分配或空 provider、未知 ID 和空返回值明确失败。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Audio.IAudioBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.audio`](../../src/composition/adapters/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L87) | Gets the built-in audio backend factory. |
| [`Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog`](../../src/composition/adapters/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L20) | Combines explicitly supplied rendering toolchains and standard ImGui presentation to the built-in runtime adapter catalog. |
| [`Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.DefaultAuthoringAdapterCatalog(Inno.Adapter.Default.DefaultAdapterCatalogOptions options, System.Collections.Generic.IEnumerable<Inno.Adapter.Rendering.RenderingAuthoringBackendProvider> authoringProviders, System.Collections.Generic.IEnumerable<Inno.Adapter.Presentation.PresentationBackendProvider> presentationProviders, System.Collections.Generic.IEnumerable<Inno.Adapter.Rendering.RenderingBackendProvider>? renderingProviders = null)`](../../src/composition/adapters/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L46) | Creates paired runtime and authoring registrations before any native device is initialized. |
| [`Inno.Adapter.Input.IInputBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.input`](../../src/composition/adapters/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L72) | Gets the built-in input backend factory. |
| [`Inno.Adapter.Platform.IPlatformBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.platform`](../../src/composition/adapters/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L67) | Gets the built-in platform backend factory. |
| [`Inno.Adapter.Presentation.IPresentationBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.presentation`](../../src/composition/adapters/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L107) | Gets the built-in graphical host-presentation factory. |
| [`Inno.Adapter.Rendering.IRenderingAuthoringBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.renderingAuthoring`](../../src/composition/adapters/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L102) | Gets the built-in rendering authoring toolchain factory. |
| [`Inno.Adapter.Rendering.IRenderingBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.rendering`](../../src/composition/adapters/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L82) | Gets the built-in runtime rendering backend factory. |
| [`Inno.Adapter.Storage.IStorageBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.storage`](../../src/composition/adapters/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L77) | Gets the built-in application-storage backend factory. |
| [`Inno.Adapter.Text.ITextBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.text`](../../src/composition/adapters/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L92) | Gets the built-in Unicode text backend factory. |
| [`Inno.Adapter.UI.IUiBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.ui`](../../src/composition/adapters/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L97) | Gets the built-in retained-mode UI backend factory. |

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
