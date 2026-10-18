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

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog`

<!-- api: T:Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog 19b95913d7f1233e7f18e18af8d9dbd20a2f46776418008e18a9cec03ca5212a -->
<!-- api: M:Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.#ctor(Inno.Adapter.Default.DefaultAdapterCatalogOptions,System.Collections.Generic.IEnumerable{Inno.Adapter.Rendering.RenderingAuthoringBackendProvider},System.Collections.Generic.IEnumerable{Inno.Adapter.Presentation.PresentationBackendProvider}) 41ac992fa3260d6bf3c1eecea9eb3868293fa50667c3dda4dee7d8880bd26d5b -->
<!-- api: P:Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.platform 0a3989845b6414939029d59ead0ba1ef8d3b2d128807058af834e36cc1754054 -->
<!-- api: P:Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.input 45f2a619ca9e6bceeb840d6fb7b6b82ae7c511d8706bca7cd2cae49e1f696268 -->
<!-- api: P:Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.storage fe6b2ca6c3b3fe0ca04c03a5df8d6f62f9b7a6f1d8ba5ca07e7cf2bede6d3132 -->
<!-- api: P:Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.rendering b77737085e5a14b1a59c28fdc7e20fec969d742aae10191b7c4312b127b66316 -->
<!-- api: P:Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.audio ffacc61f7b2f442e8b9bff91793835425b80d034898241eef07ebbe7e99c7424 -->
<!-- api: P:Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.text 690d8ccce71238b9f95f9a3bacadb5d55938af0bacdeb03130376a5d4d86c2c7 -->
<!-- api: P:Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.ui c6f05a6ea0c96771380289cf725f2fd2f468018eb96ce0e8ce919915bb256dc4 -->
<!-- api: P:Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.renderingAuthoring 66d0668e224e7c0977cf81b03e769b1336cd69559e2aff45c58f249eaf4f20a2 -->
<!-- api: P:Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.presentation 80d8819cf92bf24401f3179d21b1f12d3bd2c1a2ff33209c2618e7a11a8857f6 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b50342efa8d0abdc"></a>[`public Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.DefaultAuthoringAdapterCatalog(Inno.Adapter.Default.DefaultAdapterCatalogOptions options, System.Collections.Generic.IEnumerable<Inno.Adapter.Rendering.RenderingAuthoringBackendProvider> authoringProviders, System.Collections.Generic.IEnumerable<Inno.Adapter.Presentation.PresentationBackendProvider> presentationProviders)`](../../src/composition/adapters/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L43) | Creates paired runtime and authoring registrations before any native device is initialized.<br>`options`：The host's explicit runtime service configuration.<br>`authoringProviders`：Complete matching authoring registrations with explicit product compilation targets.<br>`presentationProviders`：Explicit complete presentation registrations. An empty sequence registers none. | — | `System.ArgumentException`：The runtime and authoring backend registrations do not match. |
| <a id="api-66f7be26e3759abd"></a>[`public Inno.Adapter.Audio.IAudioBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.audio { get; }`](../../src/composition/adapters/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L83) | Gets the built-in audio backend factory. | — | — |
| <a id="api-b536201a6e2f293a"></a>[`public Inno.Adapter.Input.IInputBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.input { get; }`](../../src/composition/adapters/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L68) | Gets the built-in input backend factory. | — | — |
| <a id="api-815c4e3b261c8ee6"></a>[`public Inno.Adapter.Platform.IPlatformBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.platform { get; }`](../../src/composition/adapters/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L63) | Gets the built-in platform backend factory. | — | — |
| <a id="api-5d8fcf24b0cbdc45"></a>[`public Inno.Adapter.Presentation.IPresentationBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.presentation { get; }`](../../src/composition/adapters/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L103) | Gets the built-in graphical host-presentation factory. | — | — |
| <a id="api-5181801a33b56c02"></a>[`public Inno.Adapter.Rendering.IRenderingBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.rendering { get; }`](../../src/composition/adapters/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L78) | Gets the built-in runtime rendering backend factory. | — | — |
| <a id="api-c51aeb97c8639452"></a>[`public Inno.Adapter.Rendering.IRenderingAuthoringBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.renderingAuthoring { get; }`](../../src/composition/adapters/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L98) | Gets the built-in rendering authoring toolchain factory. | — | — |
| <a id="api-2bb8f0ebc5a839fe"></a>[`public Inno.Adapter.Storage.IStorageBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.storage { get; }`](../../src/composition/adapters/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L73) | Gets the built-in application-storage backend factory. | — | — |
| <a id="api-9cecb188be78839c"></a>[`public Inno.Adapter.Text.ITextBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.text { get; }`](../../src/composition/adapters/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L88) | Gets the built-in Unicode text backend factory. | — | — |
| <a id="api-4ef1f157585b8660"></a>[`public Inno.Adapter.UI.IUiBackendFactory Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog.ui { get; }`](../../src/composition/adapters/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L93) | Gets the built-in retained-mode UI backend factory. | — | — |
| <a id="api-b36bacc5f420d055"></a>[`public sealed class Inno.Adapter.Authoring.Default.DefaultAuthoringAdapterCatalog : Inno.Adapter.Presentation.IAuthoringAdapterCatalog, Inno.Adapter.Rendering.IRenderingAuthoringBackendFactory`](../../src/composition/adapters/Inno.Adapter.Authoring.Default/DefaultAuthoringAdapterCatalog.cs#L20) | Combines explicitly supplied rendering toolchains and standard ImGui presentation to the built-in runtime adapter catalog. | — | — |

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
