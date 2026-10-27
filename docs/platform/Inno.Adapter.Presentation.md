# Inno.Adapter.Presentation

[Platform 索引](README.md) · [Rendering](../rendering/README.md) · [Runtime adapters](../runtime/Inno.Adapter.md)

该项目定义 authoring product 的图形 Presentation family。它不暴露 SDL3 window、BGFX handle 或具体 ImGui context。

## 公开 API

- `PresentationBackendId`：开放、区分大小写的稳定实现 ID，默认值表示未分配；内置值为 `imGui`。
- `PresentationBackendProvider`、`PresentationBackendCatalog`：组合入口提供完整 provider 集合，catalog 验证 ID、拒绝重复和 null，并发布只读 `supportedBackends`。注册不创建原生上下文。
- `PresentationFeatures`：声明中立 capability。
- `PresentationBackendOptions`：组合中立 platform/window/render device、shader compiler 与 asset source。
- `PresentationTextureHandle`：presentation generation 内使用的 opaque texture token。
- `IPresentationContext`：layout、frame draw、texture registration、render-graph contribution，以及 `TryGetWindowId(viewportId, out windowId)` 查询活动 viewport 的平台窗口身份。
- `IPresentationBackendFactory`：创建 presentation context。
- `IAuthoringAdapterCatalog`：在 runtime `IAdapterCatalog` 上增加 rendering authoring 与 presentation factory。

EditorHost 只保存 `IPresentationContext`。SDL3 event bridge 与 BGFX ImGui renderer 在 `Inno.Adapter.Authoring.Default` 内组合，不能出现在 Host 的字段、参数、返回值或 using 中。

```csharp
using Inno.Adapter.Presentation;

static IPresentationContext CreateOwnedPresentation(
    IPresentationBackendFactory factory,
    PresentationBackendId selection,
    PresentationBackendOptions options
) {
    return factory.CreateContext(selection, options);
}
```

Presentation 是 Application 生命周期资源，必须先于 shared render device 释放。

新增实现继承 `PresentationBackendProvider`，由组合入口赋予稳定 ID 并注册到 catalog。
`CreateContext` 的调用方拥有返回的 context；provider 不接管 options 中的平台与渲染资源。
未知 ID 抛出 `NotSupportedException`，provider 返回 null 则抛出 `InvalidOperationException`。
`DefaultAuthoringAdapterCatalog` 的 `presentationProviders` 为 null 时注册内置 ImGui，空序列明确注册零个实现。

## 当前源码公开 API 清单

此清单对应当前 SDK 求值与源码符号；内部类型不构成公开 API。

<!-- api: T:Inno.Adapter.Presentation.IAuthoringAdapterCatalog f8a256584ddcba75456025440be40263fb9e44a523604eef54d5120a19714d9e -->
<!-- api: P:Inno.Adapter.Presentation.IAuthoringAdapterCatalog.renderingAuthoring a706e2dacf5d714fb65d0f590845b671b015744f8fb578e8679cd346ed4ca230 -->
<!-- api: P:Inno.Adapter.Presentation.IAuthoringAdapterCatalog.presentation ed48266f2f7906c4cb6bbc84ad552e339d9e91110a264eba43b15cd5c1493ef0 -->
<!-- api: T:Inno.Adapter.Presentation.IPresentationBackendFactory c87c9e6657058dcb15b5dc4be5e92310d414d791b47ebb5c5fd6facc1fbaadc7 -->
<!-- api: P:Inno.Adapter.Presentation.IPresentationBackendFactory.supportedBackends 1598260780e79215da8640053175bce0883e19c6c3275206ec8930b24c900ba0 -->
<!-- api: M:Inno.Adapter.Presentation.IPresentationBackendFactory.CreateContext(Inno.Adapter.Presentation.PresentationBackendId,Inno.Adapter.Presentation.PresentationBackendOptions) 2ecffc70c3042396b76f6073ec0df941a35f97b60fca31233954e01de364ba8f -->
<!-- api: T:Inno.Adapter.Presentation.IPresentationContext c02062b9be1a8e513c7d1a28e3f194f13ceb151bf718091e04ad4ffd4e9a9937 -->
<!-- api: M:Inno.Adapter.Presentation.IPresentationContext.TryGetWindowId(System.UInt32,System.UInt32@) 0cfbcfee0f20fe7f702c91f95b848d3f2c79e69bddcc728d270653586bf5232f -->
<!-- api: M:Inno.Adapter.Presentation.IPresentationContext.SetLayoutFile(System.String) 55629daab68df078349c37a23c6358355f1dc29935f0b16bc556170688cbe463 -->
<!-- api: M:Inno.Adapter.Presentation.IPresentationContext.LoadLayout(System.String) 61105756d748a01d3dcc7e87cb6444e8005665604071524e8a12f94f31abf034 -->
<!-- api: M:Inno.Adapter.Presentation.IPresentationContext.TryCaptureLayout(System.String@,System.Boolean) a3e53722867d112ea6016d37b3f446500d02aacee1eea5f1362580272e87ab52 -->
<!-- api: M:Inno.Adapter.Presentation.IPresentationContext.RenderFrame(System.Action,System.Action) da10b0baa577bdd304514f883125991fa9fac5493619717cd3d03a1da53df21b -->
<!-- api: M:Inno.Adapter.Presentation.IPresentationContext.RegisterTexture(Inno.Rendering.PersistentTextureHandle) b1705ef7dce9bee8f0ad128456172035e4f6f73472bd57d4e846c5d443a77f9d -->
<!-- api: M:Inno.Adapter.Presentation.IPresentationContext.UnregisterTexture(Inno.Adapter.Presentation.PresentationTextureHandle) a54698fe16def23b163400cbb62c2b39886d374f5792ae5fee97a6673de8bd1f -->
<!-- api: M:Inno.Adapter.Presentation.IPresentationContext.DrawImage(Inno.Adapter.Presentation.PresentationTextureHandle,System.Numerics.Vector2) f29eadfc05c39366f95abb71a588d23241753e727249a7725a1b6a69fd25a252 -->
<!-- api: T:Inno.Adapter.Presentation.PresentationBackendCatalog f6ae41599ee4eefbb657eecff19c6ebccf866e407eeefab642287d4b18233fb1 -->
<!-- api: M:Inno.Adapter.Presentation.PresentationBackendCatalog.#ctor(System.Collections.Generic.IEnumerable{Inno.Adapter.Presentation.PresentationBackendProvider}) bbcd6337e4339c02f570ba5957e17298bcbceea67beb96e8851797792a88d7b5 -->
<!-- api: P:Inno.Adapter.Presentation.PresentationBackendCatalog.supportedBackends 2e6bb70afb6c6b6cdaa8acbf64802ca6d7411d1f6e213046b5a56f1bc8a0308e -->
<!-- api: M:Inno.Adapter.Presentation.PresentationBackendCatalog.CreateContext(Inno.Adapter.Presentation.PresentationBackendId,Inno.Adapter.Presentation.PresentationBackendOptions) d799f920f5c272421262668816ab36b54bf5256dd5fd4a407a6c5d6550861422 -->
<!-- api: T:Inno.Adapter.Presentation.PresentationBackendId fefdcab5e77e1708c26cf94bc7ef334300c1ad7f053a42a7304a18578b8df099 -->
<!-- api: M:Inno.Adapter.Presentation.PresentationBackendId.#ctor(System.String) 9fd28775c4142a9baa1abec923ae61607c023bd1b8bda0c251bbfb592da7b111 -->
<!-- api: P:Inno.Adapter.Presentation.PresentationBackendId.imGui f2a2679c79ff5e8ce291ad5f927634f22e838a72a6c4e471ee7f130384561d11 -->
<!-- api: P:Inno.Adapter.Presentation.PresentationBackendId.value 19aa8fb1629a9b3e08ca762e66d3bb5f955261cb6ccd637fbe8134a87b78b3ff -->
<!-- api: P:Inno.Adapter.Presentation.PresentationBackendId.isValid c2b47b9d400ec2a671f0340714613be689935f802c7b608d47ad44d40a931791 -->
<!-- api: M:Inno.Adapter.Presentation.PresentationBackendId.ToString 91b4d91941a28cf8059c515217d35e651242889f8b98473d96f08f19d720def0 -->
<!-- api: M:Inno.Adapter.Presentation.PresentationBackendId.op_Inequality(Inno.Adapter.Presentation.PresentationBackendId,Inno.Adapter.Presentation.PresentationBackendId) ecedb88686f774c7ab0ae3cfc4f0f5013c1d4e2210e1b20e24d20300a7422f69 -->
<!-- api: M:Inno.Adapter.Presentation.PresentationBackendId.op_Equality(Inno.Adapter.Presentation.PresentationBackendId,Inno.Adapter.Presentation.PresentationBackendId) 40ae458ac8f1867c44db2fa9201aad53a5e5cf8989664f2b8251fb477a276ad3 -->
<!-- api: M:Inno.Adapter.Presentation.PresentationBackendId.GetHashCode afde939f66d077eabbb19fc4eff8c520a4523b2cd5db6a647d7b92d0445f8d6b -->
<!-- api: M:Inno.Adapter.Presentation.PresentationBackendId.Equals(System.Object) 198266edfcdcb7e31ed9aa64a012a8be9b6a7ddf736c608d1c6a564ce7e79f9f -->
<!-- api: M:Inno.Adapter.Presentation.PresentationBackendId.Equals(Inno.Adapter.Presentation.PresentationBackendId) 1923798d66fa84899b03d34d14fee039c17ac4f5c4e50fb2de3778bc89662c97 -->
<!-- api: T:Inno.Adapter.Presentation.PresentationBackendOptions 5f85ad6b1c55963549798b65ab58674c2171965b73e896acfcf06ca64d7482be -->
<!-- api: P:Inno.Adapter.Presentation.PresentationBackendOptions.platformApplication 1bc0eef1cddffec114cdf111c5fde3ff3b4e79c6ae29475e79b78b1648e710ff -->
<!-- api: P:Inno.Adapter.Presentation.PresentationBackendOptions.window c661b5212461ce3a3cb9a7473b8b345d8e89dda343669c0b4fdc52ca60bf8f3d -->
<!-- api: P:Inno.Adapter.Presentation.PresentationBackendOptions.renderDevice 632c92a329ee97b8deaba19bf0e1865e5597ba15e4101532417d9a13ed410d4f -->
<!-- api: P:Inno.Adapter.Presentation.PresentationBackendOptions.features 0fca5155df83967e4e61ddf366cb43d173ca1045d0ed8ddd228f5c9931560c31 -->
<!-- api: M:Inno.Adapter.Presentation.PresentationBackendOptions.#ctor 04e78fa2d115837ddf06965df43f4d152189bb4206b7d4a0caa3bc60d0aa5ce3 -->
<!-- api: T:Inno.Adapter.Presentation.PresentationBackendProvider 87413423192bdf205434979a8657c118a6b63c1b7997d6695ae483be4d033923 -->
<!-- api: M:Inno.Adapter.Presentation.PresentationBackendProvider.#ctor(Inno.Adapter.Presentation.PresentationBackendId) b9da8c9f3d10e213df4205606a1d1f895b5435c3bf8d39b46d93b65cd70551c6 -->
<!-- api: P:Inno.Adapter.Presentation.PresentationBackendProvider.id f6b3f6476be26a60f43c5c463e138726c7e94dd66a6f9adfdf6e582304addd16 -->
<!-- api: M:Inno.Adapter.Presentation.PresentationBackendProvider.CreateContext(Inno.Adapter.Presentation.PresentationBackendOptions) b03c953d76932d618d7ba68dd0034445973bb27682ca01d3c047f51f06656318 -->
<!-- api: T:Inno.Adapter.Presentation.PresentationFeatures 6f9fe823d4e2115a065dd116ba253dabbcdf6d40f3cd250055e8893aa77cd92e -->
<!-- api: F:Inno.Adapter.Presentation.PresentationFeatures.None 38d8827dd5fe329625b4cbd8e9e95a5ea71e11a4125d6d9f8892696e0b496ff1 -->
<!-- api: F:Inno.Adapter.Presentation.PresentationFeatures.MultipleWindows 11b7c9f814f9484b353b77e32e89970ec841a8c22c0fd7a742b006bcf5999d54 -->
<!-- api: F:Inno.Adapter.Presentation.PresentationFeatures.Docking c31a09f67010d5aed820dcffbd20a893bfc29cea65c26adbcee8767d3d4d5b85 -->
<!-- api: F:Inno.Adapter.Presentation.PresentationFeatures.SmoothResize 5bce5d193390c3bbc6ebe2a567e1dd15ca1ae496b5c3ee932c0d9dcb52d7db2d -->
<!-- api: T:Inno.Adapter.Presentation.PresentationTextureHandle 1524a8ea2e9ab15e054b1eb4e5359f12be9d6ad2498f85aa7f3f389acd1454f3 -->
<!-- api: M:Inno.Adapter.Presentation.PresentationTextureHandle.#ctor(System.UInt64) 06039695d6ebc1c44b5fe20ae598bffeab5b63cd93b33d8094594f784c7f9d3b -->
<!-- api: P:Inno.Adapter.Presentation.PresentationTextureHandle.value 3d46761dca9afc561ea191cdbf1c1ab07fe06d442320c6490f50b583e9276f57 -->
<!-- api: P:Inno.Adapter.Presentation.PresentationTextureHandle.isValid d4d12508434bb2d59bb150c994e729bc84a2a9205cd952274716848c5c3a5255 -->
<!-- api: M:Inno.Adapter.Presentation.PresentationTextureHandle.ToString dc547489f7102986476243bbf70a9ae4c43bb92bb68b60533fd8491f2b816a50 -->
<!-- api: M:Inno.Adapter.Presentation.PresentationTextureHandle.op_Inequality(Inno.Adapter.Presentation.PresentationTextureHandle,Inno.Adapter.Presentation.PresentationTextureHandle) 59d93503e89079c6a98daef8cb4dfa542150d84d366cdaf051ef35d8a2fd9df9 -->
<!-- api: M:Inno.Adapter.Presentation.PresentationTextureHandle.op_Equality(Inno.Adapter.Presentation.PresentationTextureHandle,Inno.Adapter.Presentation.PresentationTextureHandle) 52468509c601905478c8c8c31953a94f9f3474af3e66a147aa7dd733a0efe581 -->
<!-- api: M:Inno.Adapter.Presentation.PresentationTextureHandle.GetHashCode 5cca8c5f46ad3f8f124ef6b9444f4e8a9f9b3c7ebf86101eae53e14fba42930d -->
<!-- api: M:Inno.Adapter.Presentation.PresentationTextureHandle.Equals(System.Object) c8bdc602780385c7fb6bf24f8a34c052a43e29d289b75f5bcb52bd3c1e8a7860 -->
<!-- api: M:Inno.Adapter.Presentation.PresentationTextureHandle.Equals(Inno.Adapter.Presentation.PresentationTextureHandle) 97c37004ba16ac53f738203eb97516d01172fc58b313bce440114d6acc85da7a -->

| API 与源码 | 契约与参数 | 返回值 | 重要异常 |
| --- | --- | --- | --- |
| <a id="api-201f881f920dd4b4"></a>[`public interface Inno.Adapter.Presentation.IAuthoringAdapterCatalog : Inno.Adapter.IAdapterCatalog`](../../src/adapters/presentation/Inno.Adapter.Presentation/IAuthoringAdapterCatalog.cs#L8) | Extends the runtime adapter catalog with tooling and presentation factories required by authoring products. | — | — |
| <a id="api-9f08c3218d3829bd"></a>[`Inno.Adapter.Rendering.IRenderingAuthoringBackendFactory Inno.Adapter.Presentation.IAuthoringAdapterCatalog.renderingAuthoring { get; }`](../../src/adapters/presentation/Inno.Adapter.Presentation/IAuthoringAdapterCatalog.cs#L13) | Gets the rendering toolchain factory used to compile authoring artifacts. | — | — |
| <a id="api-8b2f73a6f11693dd"></a>[`Inno.Adapter.Presentation.IPresentationBackendFactory Inno.Adapter.Presentation.IAuthoringAdapterCatalog.presentation { get; }`](../../src/adapters/presentation/Inno.Adapter.Presentation/IAuthoringAdapterCatalog.cs#L18) | Gets the graphical host-presentation factory used by authoring products. | — | — |
| <a id="api-8ad264f443bb4f1d"></a>[`public interface Inno.Adapter.Presentation.IPresentationBackendFactory`](../../src/adapters/presentation/Inno.Adapter.Presentation/IPresentationBackendFactory.cs#L8) | Creates host presentation contexts from explicit backend selections. | — | — |
| <a id="api-f32691a1a5ad7695"></a>[`System.Collections.Generic.IReadOnlyList<Inno.Adapter.Presentation.PresentationBackendId> Inno.Adapter.Presentation.IPresentationBackendFactory.supportedBackends { get; }`](../../src/adapters/presentation/Inno.Adapter.Presentation/IPresentationBackendFactory.cs#L13) | Gets the implementation identities available in this immutable factory snapshot. | — | — |
| <a id="api-ec7921f4b19a3a37"></a>[`Inno.Adapter.Presentation.IPresentationContext Inno.Adapter.Presentation.IPresentationBackendFactory.CreateContext(Inno.Adapter.Presentation.PresentationBackendId backend, Inno.Adapter.Presentation.PresentationBackendOptions options)`](../../src/adapters/presentation/Inno.Adapter.Presentation/IPresentationBackendFactory.cs#L30) | Creates a presentation context over compatible platform and rendering adapters.<br>`backend`: Stable presentation implementation selected by the composition root.<br>`options`: Host-owned resources and presentation policy. | A caller-owned backend-neutral presentation context. | `System.NotSupportedException`: Thrown when the selected platform, rendering, and presentation backends are incompatible. |
| <a id="api-929c87336b7b3e6c"></a>[`public interface Inno.Adapter.Presentation.IPresentationContext : Inno.Rendering.IRenderFrameGraphContributor, System.IDisposable`](../../src/adapters/presentation/Inno.Adapter.Presentation/IPresentationContext.cs#L10) | Defines the backend-neutral lifecycle used by a graphical composition host. | — | — |
| <a id="api-7e96655fbd0d86de"></a>[`bool Inno.Adapter.Presentation.IPresentationContext.TryGetWindowId(uint viewportId, out uint windowId)`](../../src/adapters/presentation/Inno.Adapter.Presentation/IPresentationContext.cs#L24) | Resolves the platform window that owns a presentation viewport.<br>`viewportId`: The presentation viewport identity.<br>`windowId`: The owning platform window identity when available. | when the viewport has a live platform window. | — |
| <a id="api-50967e31ee7ec4cd"></a>[`void Inno.Adapter.Presentation.IPresentationContext.SetLayoutFile(string? filePath)`](../../src/adapters/presentation/Inno.Adapter.Presentation/IPresentationContext.cs#L35) | Disables or redirects backend-owned layout-file persistence.<br>`filePath`: Layout path, or when the host persists layout text itself. | — | — |
| <a id="api-d9ea213c2bfd0adc"></a>[`void Inno.Adapter.Presentation.IPresentationContext.LoadLayout(string? settings)`](../../src/adapters/presentation/Inno.Adapter.Presentation/IPresentationContext.cs#L43) | Loads complete layout text captured by the host.<br>`settings`: Layout text, or an empty value when no layout exists. | — | — |
| <a id="api-567cefa5b2db04b1"></a>[`bool Inno.Adapter.Presentation.IPresentationContext.TryCaptureLayout(out string settings, bool force = false)`](../../src/adapters/presentation/Inno.Adapter.Presentation/IPresentationContext.cs#L57) | Captures complete layout text when persistence is required.<br>`settings`: Complete layout text when capture succeeds.<br>`force`: Whether to capture even when the backend did not mark the layout dirty. | when layout text was captured. | — |
| <a id="api-29075e9e2effb367"></a>[`void Inno.Adapter.Presentation.IPresentationContext.RenderFrame(System.Action drawFrame, System.Action? prepareFrame = null)`](../../src/adapters/presentation/Inno.Adapter.Presentation/IPresentationContext.cs#L78) | Builds one presentation frame by invoking the host draw callback.<br>`drawFrame`: Callback that emits presentation commands for the current frame.<br>`prepareFrame`: Optional synchronous preparation after the owned context and display metrics are selected, before frame creation. Failure prevents drawing and no partial frame is published. | — | `System.ArgumentNullException`: The draw callback is null.<br>`System.InvalidOperationException`: Another frame operation is active on this context. |
| <a id="api-0536dd4854ec7a43"></a>[`Inno.Adapter.Presentation.PresentationTextureHandle Inno.Adapter.Presentation.IPresentationContext.RegisterTexture(Inno.Rendering.PersistentTextureHandle texture)`](../../src/adapters/presentation/Inno.Adapter.Presentation/IPresentationContext.cs#L92) | Registers a persistent render texture for presentation drawing.<br>`texture`: Texture owned by the active rendering-device generation. | An opaque presentation token. | — |
| <a id="api-4a1b75d0840f9e2c"></a>[`bool Inno.Adapter.Presentation.IPresentationContext.UnregisterTexture(Inno.Adapter.Presentation.PresentationTextureHandle texture)`](../../src/adapters/presentation/Inno.Adapter.Presentation/IPresentationContext.cs#L103) | Releases a presentation token without taking ownership of the source texture.<br>`texture`: Previously allocated presentation token. | when the token was registered and released. | — |
| <a id="api-95c1e356f4be8d97"></a>[`void Inno.Adapter.Presentation.IPresentationContext.DrawImage(Inno.Adapter.Presentation.PresentationTextureHandle texture, System.Numerics.Vector2 size)`](../../src/adapters/presentation/Inno.Adapter.Presentation/IPresentationContext.cs#L114) | Draws a registered texture in the active presentation surface.<br>`texture`: Opaque presentation texture token.<br>`size`: Destination size in logical presentation units. | — | — |
| <a id="api-52c7610b0dd5590c"></a>[`public sealed class Inno.Adapter.Presentation.PresentationBackendCatalog : Inno.Adapter.Presentation.IPresentationBackendFactory`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendCatalog.cs#L10) | Resolves presentation providers from one immutable, composition-owned registration snapshot. | — | — |
| <a id="api-8dee44613dce9ef2"></a>[`public Inno.Adapter.Presentation.PresentationBackendCatalog.PresentationBackendCatalog(System.Collections.Generic.IEnumerable<Inno.Adapter.Presentation.PresentationBackendProvider> providers)`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendCatalog.cs#L26) | Validates and captures a complete provider set without creating any service.<br>`providers`: Providers whose lifetime remains with the composition owner. | — | `System.ArgumentNullException`: The provider sequence is null.<br>`System.ArgumentException`: A provider is null, unassigned, or duplicates a registration identity. |
| <a id="api-a15f08031ff61f1f"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Adapter.Presentation.PresentationBackendId> Inno.Adapter.Presentation.PresentationBackendCatalog.supportedBackends { get; }`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendCatalog.cs#L38) | Gets the implementation identities available in this immutable factory snapshot. | — | — |
| <a id="api-d4284a1eec39ec1a"></a>[`public Inno.Adapter.Presentation.IPresentationContext Inno.Adapter.Presentation.PresentationBackendCatalog.CreateContext(Inno.Adapter.Presentation.PresentationBackendId backend, Inno.Adapter.Presentation.PresentationBackendOptions options)`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendCatalog.cs#L44) | Creates a presentation context over compatible platform and rendering adapters.<br>`backend`: Stable presentation implementation selected by the composition root.<br>`options`: Host-owned resources and presentation policy. | A caller-owned backend-neutral presentation context. | `System.NotSupportedException`: Thrown when the selected platform, rendering, and presentation backends are incompatible.<br>`System.InvalidOperationException`: The selected provider returned no service. |
| <a id="api-ffbbb64d9deecb2d"></a>[`public readonly record struct Inno.Adapter.Presentation.PresentationBackendId : System.IEquatable<Inno.Adapter.Presentation.PresentationBackendId>`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendId.cs#L8) | Identifies a presentation implementation without closing the set of supported backends. | — | — |
| <a id="api-f8f171a1ba0ce402"></a>[`public Inno.Adapter.Presentation.PresentationBackendId.PresentationBackendId(string value)`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendId.cs#L19) | Creates an ordinal, case-sensitive implementation identifier.<br>`value`: A stable nonempty identifier containing no whitespace. | — | `System.ArgumentException`: The identifier is empty or contains whitespace. |
| <a id="api-d9d2348903434a2f"></a>[`public static Inno.Adapter.Presentation.PresentationBackendId Inno.Adapter.Presentation.PresentationBackendId.imGui { get; }`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendId.cs#L31) | Gets the identifier of the bundled ImGui implementation. | — | — |
| <a id="api-eca2bbec79ad8d94"></a>[`public string Inno.Adapter.Presentation.PresentationBackendId.value { get; }`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendId.cs#L36) | Gets the stable identifier; a default value is unassigned. | — | — |
| <a id="api-8b137be7a4867491"></a>[`public bool Inno.Adapter.Presentation.PresentationBackendId.isValid { get; }`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendId.cs#L41) | Gets whether this value identifies an implementation. | — | — |
| <a id="api-700c882e4c8601ee"></a>[`public override string Inno.Adapter.Presentation.PresentationBackendId.ToString()`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendId.cs#L49) | Returns the identifier without resolving a provider. | The stable identifier, or an empty string when unassigned. | — |
| <a id="api-2da9335f59a17870"></a>[`public static bool Inno.Adapter.Presentation.PresentationBackendId.operator !=(Inno.Adapter.Presentation.PresentationBackendId left, Inno.Adapter.Presentation.PresentationBackendId right)`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendId.cs#L8) | Inherited or compiler-provided contract. | — | — |
| <a id="api-000be5fb3b6c3ad1"></a>[`public static bool Inno.Adapter.Presentation.PresentationBackendId.operator ==(Inno.Adapter.Presentation.PresentationBackendId left, Inno.Adapter.Presentation.PresentationBackendId right)`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendId.cs#L8) | Inherited or compiler-provided contract. | — | — |
| <a id="api-97c232c7c01afc84"></a>[`public override int Inno.Adapter.Presentation.PresentationBackendId.GetHashCode()`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendId.cs#L8) | Returns the hash code for this instance. | A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-18ef55525612a19d"></a>[`public override bool Inno.Adapter.Presentation.PresentationBackendId.Equals(object obj)`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendId.cs#L8) | Indicates whether this instance and a specified object are equal.<br>`obj`: The object to compare with the current instance. | if and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-67bfe20cbad30c56"></a>[`public bool Inno.Adapter.Presentation.PresentationBackendId.Equals(Inno.Adapter.Presentation.PresentationBackendId other)`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendId.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>`other`: An object to compare with this object. | if the current object is equal to the parameter; otherwise, . | — |
| <a id="api-54f2bb543073b793"></a>[`public sealed class Inno.Adapter.Presentation.PresentationBackendOptions`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendOptions.cs#L9) | Collects the validated presentation backend options values that configure one owned operation. | — | — |
| <a id="api-21ed94104a2bf286"></a>[`public required Inno.Platform.IPlatformApplication Inno.Adapter.Presentation.PresentationBackendOptions.platformApplication { get; set; }`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendOptions.cs#L14) | Gets or sets the platform application that owns presentation windows and events. | — | — |
| <a id="api-0de0dafdf9b8982d"></a>[`public required Inno.Platform.IPlatformWindow Inno.Adapter.Presentation.PresentationBackendOptions.window { get; set; }`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendOptions.cs#L19) | Gets or sets the primary presentation window. | — | — |
| <a id="api-4968686c40137ef4"></a>[`public required Inno.Rendering.IRenderDevice Inno.Adapter.Presentation.PresentationBackendOptions.renderDevice { get; set; }`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendOptions.cs#L24) | Gets or sets the rendering device used to present host draw data. | — | — |
| <a id="api-d8ed34adf783980c"></a>[`public Inno.Adapter.Presentation.PresentationFeatures Inno.Adapter.Presentation.PresentationBackendOptions.features { get; set; }`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendOptions.cs#L29) | Gets or sets optional presentation features requested by the host. | — | — |
| <a id="api-f22320527665b290"></a>[`public Inno.Adapter.Presentation.PresentationBackendOptions.PresentationBackendOptions()`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendOptions.cs#L9) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3c63f1ad0a666728"></a>[`public abstract class Inno.Adapter.Presentation.PresentationBackendProvider`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendProvider.cs#L8) | Describes an explicitly composed host presentation implementation and its creation boundary. | — | — |
| <a id="api-667ed04e887987d8"></a>[`protected Inno.Adapter.Presentation.PresentationBackendProvider.PresentationBackendProvider(Inno.Adapter.Presentation.PresentationBackendId id)`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendProvider.cs#L19) | Captures the implementation identity assigned by the composition owner.<br>`id`: The assigned, immutable implementation identifier. | — | `System.ArgumentException`: The identifier is unassigned. |
| <a id="api-69969514867e8593"></a>[`public Inno.Adapter.Presentation.PresentationBackendId Inno.Adapter.Presentation.PresentationBackendProvider.id { get; }`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendProvider.cs#L29) | Gets this registration's immutable implementation identity. | — | — |
| <a id="api-c88dbf1e67e33da2"></a>[`public abstract Inno.Adapter.Presentation.IPresentationContext Inno.Adapter.Presentation.PresentationBackendProvider.CreateContext(Inno.Adapter.Presentation.PresentationBackendOptions options)`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationBackendProvider.cs#L44) | Creates a new caller-owned presentation context over the supplied host resources.<br>`options`: Externally owned platform and rendering resources, together with the requested presentation policy. | A new context whose caller must dispose it before releasing the supplied resources. Providers must not return null. | `System.NotSupportedException`: The host resources or requested features are incompatible with this implementation. |
| <a id="api-67446136bde68444"></a>[`public enum Inno.Adapter.Presentation.PresentationFeatures : int`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationFeatures.cs#L9) | Describes optional capabilities requested from a host presentation backend. | — | — |
| <a id="api-696d1e047d85d6b0"></a>[`Inno.Adapter.Presentation.PresentationFeatures.None = 0`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationFeatures.cs#L14) | Requests no optional presentation features. | — | — |
| <a id="api-c1b74af8b4809a71"></a>[`Inno.Adapter.Presentation.PresentationFeatures.MultipleWindows = 1`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationFeatures.cs#L19) | Allows the presentation to create detached native windows. | — | — |
| <a id="api-d5c997cabd4af836"></a>[`Inno.Adapter.Presentation.PresentationFeatures.Docking = 2`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationFeatures.cs#L24) | Enables dockable host surfaces when supported. | — | — |
| <a id="api-48d22d9898702369"></a>[`Inno.Adapter.Presentation.PresentationFeatures.SmoothResize = 4`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationFeatures.cs#L29) | Enables smooth rendering while native windows are being resized. | — | — |
| <a id="api-948c5b8727cd8c73"></a>[`public readonly record struct Inno.Adapter.Presentation.PresentationTextureHandle : System.IEquatable<Inno.Adapter.Presentation.PresentationTextureHandle>`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationTextureHandle.cs#L8) | Identifies a renderer-owned texture through an opaque host-presentation token. | — | — |
| <a id="api-83b868525c478bd1"></a>[`public Inno.Adapter.Presentation.PresentationTextureHandle.PresentationTextureHandle(ulong value)`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationTextureHandle.cs#L16) | Creates an opaque presentation texture token.<br>`value`: Non-zero token allocated by the active presentation backend. | — | — |
| <a id="api-db02f1e362ad5f51"></a>[`public ulong Inno.Adapter.Presentation.PresentationTextureHandle.value { get; }`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationTextureHandle.cs#L26) | Gets the opaque backend token. | — | — |
| <a id="api-e101a49d4c41f8a3"></a>[`public bool Inno.Adapter.Presentation.PresentationTextureHandle.isValid { get; }`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationTextureHandle.cs#L31) | Gets whether this token identifies a registered presentation texture. | — | — |
| <a id="api-ed39d4fff8fa36ed"></a>[`public override string Inno.Adapter.Presentation.PresentationTextureHandle.ToString()`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationTextureHandle.cs#L8) | Returns the fully qualified type name of this instance. | The fully qualified type name. | — |
| <a id="api-2e3df573a4e03e07"></a>[`public static bool Inno.Adapter.Presentation.PresentationTextureHandle.operator !=(Inno.Adapter.Presentation.PresentationTextureHandle left, Inno.Adapter.Presentation.PresentationTextureHandle right)`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationTextureHandle.cs#L8) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b965811a1d224c57"></a>[`public static bool Inno.Adapter.Presentation.PresentationTextureHandle.operator ==(Inno.Adapter.Presentation.PresentationTextureHandle left, Inno.Adapter.Presentation.PresentationTextureHandle right)`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationTextureHandle.cs#L8) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f33bc83a7fa989d3"></a>[`public override int Inno.Adapter.Presentation.PresentationTextureHandle.GetHashCode()`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationTextureHandle.cs#L8) | Returns the hash code for this instance. | A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-1f44c8f6fe788360"></a>[`public override bool Inno.Adapter.Presentation.PresentationTextureHandle.Equals(object obj)`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationTextureHandle.cs#L8) | Indicates whether this instance and a specified object are equal.<br>`obj`: The object to compare with the current instance. | if and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-3ea832f30642ffbe"></a>[`public bool Inno.Adapter.Presentation.PresentationTextureHandle.Equals(Inno.Adapter.Presentation.PresentationTextureHandle other)`](../../src/adapters/presentation/Inno.Adapter.Presentation/PresentationTextureHandle.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>`other`: An object to compare with this object. | if the current object is equal to the parameter; otherwise, . | — |

## 项目依赖

| 工程 | 边界 |
| --- | --- |
| `Inno.Adapter` | 公开签名依赖 |
| `Inno.Adapter.Rendering.Authoring` | 公开签名依赖 |
| `Inno.Platform` | 公开签名依赖 |
| `Inno.Rendering` | 公开签名依赖 |
| `Inno.Extensibility.Catalogs` | 公开签名依赖 |
