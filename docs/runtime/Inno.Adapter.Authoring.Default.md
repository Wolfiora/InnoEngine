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

以下仅列出当前程序集自己声明的 public/protected 契约；继承成员遵循所属基类页面。internal/private 实现不作为稳定公开 API。签名依据当前源码语义模型生成，行为、参数、异常与所有权说明同时以对应英文 XML 为准。

### `Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.DefaultAuthoringAdapterCatalog(Inno.Adapter.Default.DefaultAdapterCatalogOptions options, System.Collections.Generic.IEnumerable<Inno.Adapter.Rendering.RenderingBackendProvider>? renderingProviders = null, System.Collections.Generic.IEnumerable<Inno.Adapter.Rendering.RenderingAuthoringBackendProvider>? authoringProviders = null, System.Collections.Generic.IEnumerable<Inno.Adapter.Presentation.PresentationBackendProvider>? presentationProviders = null)`](../../src/adapters/default/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L48) | Creates paired runtime and authoring registrations before any native device is initialized. |
| [`Inno.Adapter.Audio.IAudioBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.audio`](../../src/adapters/default/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L87) | Gets the built-in audio backend factory. |
| [`Inno.Adapter.Input.IInputBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.input`](../../src/adapters/default/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L72) | Gets the built-in input backend factory. |
| [`Inno.Adapter.Platform.IPlatformBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.platform`](../../src/adapters/default/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L67) | Gets the built-in platform backend factory. |
| [`Inno.Adapter.Presentation.IPresentationBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.presentation`](../../src/adapters/default/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L107) | Gets the built-in graphical host-presentation factory. |
| [`Inno.Adapter.Rendering.IRenderingBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.rendering`](../../src/adapters/default/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L82) | Gets the built-in runtime rendering backend factory. |
| [`Inno.Adapter.Rendering.IRenderingAuthoringBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.renderingAuthoring`](../../src/adapters/default/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L102) | Gets the built-in rendering authoring toolchain factory. |
| [`Inno.Adapter.Storage.IStorageBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.storage`](../../src/adapters/default/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L77) | Gets the built-in application-storage backend factory. |
| [`Inno.Adapter.Text.ITextBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.text`](../../src/adapters/default/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L92) | Gets the built-in Unicode text backend factory. |
| [`Inno.Adapter.UI.IUiBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.ui`](../../src/adapters/default/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L97) | Gets the built-in retained-mode UI backend factory. |
| [`Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog`](../../src/adapters/default/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L22) | Adds the standard rendering toolchain and ImGui presentation to the built-in runtime adapter catalog. |

## 项目依赖

- [Inno.Adapter](Inno.Adapter.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Adapter.Default](Inno.Adapter.Default.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Adapter.Text](../text/Inno.Adapter.Text.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Adapter.UI](../ui/Inno.Adapter.UI.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Adapter.UI.RmlUi.Authoring](../ui/Inno.Adapter.UI.RmlUi.Authoring.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Adapter.Platform.Sdl3](../platform/Inno.Adapter.Platform.Sdl3.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Adapter.Presentation.ImGui.Sdl3](../platform/Inno.Adapter.Presentation.ImGui.Sdl3.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Adapter.Presentation.ImGui.Bgfx](../rendering/Inno.Adapter.Presentation.ImGui.Bgfx.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Adapter.Rendering.Authoring](../rendering/Inno.Adapter.Rendering.Authoring.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Rendering](../rendering/Inno.Rendering.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Rendering.Assets](../rendering/Inno.Rendering.Assets.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Build.Toolchains.Bgfx.Tools](../build/Inno.Build.Toolchains.Bgfx.Tools.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Rendering.Assets.Authoring](../rendering/Inno.Rendering.Assets.Authoring.md)：项目引用；公开签名可见性由语义边界检查确认。

共同 MSBuild 注入的 analyzer 与编译规则属于构建依赖，完整有效项目图记录在本轮验收证据中。
