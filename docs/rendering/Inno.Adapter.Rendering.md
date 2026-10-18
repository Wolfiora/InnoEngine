# Inno.Adapter.Rendering

[Rendering 索引](README.md) · [中立 Rendering](Inno.Rendering.md) · [BGFX implementation](../backends/Bgfx/Inno.Adapter.Rendering.Bgfx.md)

该项目定义 Rendering Adapter family，不引用 BGFX Native。

## 公开 API

- `RenderingBackendId`：开放、区分大小写的稳定实现标识；`bgfx` 是默认值，不是支持名单。默认 struct 值未赋值，创建设备前拒绝。
- `RenderingBackendProvider`：一个实现的 `id`、`CreateDevice(options)` 与 `CreateCompositionProgramProvider()`，由当前 Composition 持有。
- `RenderingBackendCatalog`：捕获完整 provider 集合，拒绝重复/空 ID；只解析明确注册的后端，不做静默替换。
- `RenderingBackendOptions`：中立 window、graphics API preference、VSync、sRGB 与 threading policy。
- `IRenderingBackendFactory.supportedBackends/CreateDevice/CreateCompositionProgramProvider`：Player 与 Shell 使用的 runtime-only factory。Shell 在初始化窗口前检查所选 ID；Editor 与 Player 通过同一注册后端取得图层合成程序供给器。
Authoring compiler factory 被隔离在 [Inno.Adapter.Rendering.Authoring](Inno.Adapter.Rendering.Authoring.md)。
`Inno.Adapter.Default` 只依赖本项目，防止 Player 闭包间接带入 `Inno.Rendering.Assets`、AssetPipeline 与 Build Toolchain。

```csharp
using Inno.Adapter.Rendering;
using Inno.Platform;
using Inno.Rendering;

static IRenderDevice CreateOwnedDevice(
    IRenderingBackendFactory factory,
    RenderingBackendId selection,
    IPlatformWindow window
) {
    return factory.CreateDevice(selection, new RenderingBackendOptions { window = window });
}
```

BGFX handle、view ID、native enum 和 compiler executable path 不得进入这些公开契约。

自定义后端通过 `new RenderingBackendId("studio.rendering.custom")` 与派生 provider 注册；不修改公共枚举或中央分支。
`DefaultAdapterCatalogOptions.rendering` 必須接收完整 `IRenderingBackendFactory`，不隐式选择 BGFX。Provider 属于 Composition generation，不在跨代静态缓存中保留。

## 注册身份

`RenderingBackendProvider` 的 protected 构造函数接收对应领域的 backend ID，并公开只读 `id`。ID 由 composition 分配，不能 override 或从临时创建的设备推导；构造拒绝未赋值 ID。此 provider 是显式 composition 注册，不进行类型发现。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Adapter.Rendering.IRenderingBackendFactory`

<!-- api: T:Inno.Adapter.Rendering.IRenderingBackendFactory 715922ad75e07f88ffbcf0a4f4cc0d6b46b30622af0b178a831bcd4d5f41ef15 -->
<!-- api: P:Inno.Adapter.Rendering.IRenderingBackendFactory.supportedBackends f9695ad97d5237dcf7f4be410854ebcc42368284a9dc9792f9e5cb9a07452f62 -->
<!-- api: M:Inno.Adapter.Rendering.IRenderingBackendFactory.CreateDevice(Inno.Adapter.Rendering.RenderingBackendId,Inno.Adapter.Rendering.RenderingBackendOptions) 5dd5b7d75051e9d03908f25d71be8116fd3d6e245182874d207068230f7af2e2 -->
<!-- api: M:Inno.Adapter.Rendering.IRenderingBackendFactory.CreateCompositionProgramProvider(Inno.Adapter.Rendering.RenderingBackendId) a39225c78bcb3d21c5283b8c206e940b06a30e6999afd72741d86b5ffe563890 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-afab209b07481d1b"></a>[`Inno.Rendering.IRenderLayerCompositionProgramProvider Inno.Adapter.Rendering.IRenderingBackendFactory.CreateCompositionProgramProvider(Inno.Adapter.Rendering.RenderingBackendId backend)`](../../src/adapters/rendering/Inno.Adapter.Rendering/IRenderingBackendFactory.cs#L46) | Creates the composition program provider owned by the selected rendering backend.<br>返回：A provider whose target programs are compatible with that backend.<br>`backend`：Stable identity of the selected rendering backend. | — | `System.NotSupportedException`：No provider is registered for the requested backend identity. |
| <a id="api-42e542903cbe0bd6"></a>[`Inno.Rendering.IRenderDevice Inno.Adapter.Rendering.IRenderingBackendFactory.CreateDevice(Inno.Adapter.Rendering.RenderingBackendId backend, Inno.Adapter.Rendering.RenderingBackendOptions options)`](../../src/adapters/rendering/Inno.Adapter.Rendering/IRenderingBackendFactory.cs#L29) | Creates a rendering device for the supplied primary presentation surface.<br>返回：A caller-owned backend-neutral rendering device.<br>`backend`：Stable rendering backend selected by the composition root.<br>`options`：Backend-neutral rendering options. | — | — |
| <a id="api-8a5156033efd487f"></a>[`System.Collections.Generic.IReadOnlyList<Inno.Adapter.Rendering.RenderingBackendId> Inno.Adapter.Rendering.IRenderingBackendFactory.supportedBackends { get; }`](../../src/adapters/rendering/Inno.Adapter.Rendering/IRenderingBackendFactory.cs#L15) | Gets the exact runtime backend identities available in this composition generation. | — | — |
| <a id="api-aac2c58b4a6be123"></a>[`public interface Inno.Adapter.Rendering.IRenderingBackendFactory`](../../src/adapters/rendering/Inno.Adapter.Rendering/IRenderingBackendFactory.cs#L10) | Creates runtime rendering devices from one backend selection. | — | — |

### `Inno.Adapter.Rendering.RenderingBackendCatalog`

<!-- api: T:Inno.Adapter.Rendering.RenderingBackendCatalog c62d8c8325b4fffa4e66e3674217901a71fa0634ba67da7f350a38ed541caa4f -->
<!-- api: M:Inno.Adapter.Rendering.RenderingBackendCatalog.#ctor(System.Collections.Generic.IEnumerable{Inno.Adapter.Rendering.RenderingBackendProvider}) 668fe224e648a153fed598887b4ae4c1ff18a8949b1bee5b77ef2bc016174e9b -->
<!-- api: P:Inno.Adapter.Rendering.RenderingBackendCatalog.supportedBackends ce93c6d8fcc3912883df760e86b0a42b5e90acf14b318f9736c69a9923e44495 -->
<!-- api: M:Inno.Adapter.Rendering.RenderingBackendCatalog.CreateDevice(Inno.Adapter.Rendering.RenderingBackendId,Inno.Adapter.Rendering.RenderingBackendOptions) 12fedce7140a1a9b0cb1761dc32953d819f156cf02ef34ccbcb61b4bfd5cb1b2 -->
<!-- api: M:Inno.Adapter.Rendering.RenderingBackendCatalog.CreateCompositionProgramProvider(Inno.Adapter.Rendering.RenderingBackendId) ca34599a2e7fb7b7fbae451504d093ce25e3fc69cfe7eb5964c64c19bb319d91 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-addcff754596e9b7"></a>[`public Inno.Adapter.Rendering.RenderingBackendCatalog.RenderingBackendCatalog(System.Collections.Generic.IEnumerable<Inno.Adapter.Rendering.RenderingBackendProvider> providers)`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendCatalog.cs#L24) | Validates and captures a complete set of runtime providers.<br>`providers`：Providers whose lifetime is owned by this composition generation. | — | `System.ArgumentException`：A provider is null, has no ID, or duplicates an ID. |
| <a id="api-ea0b910ee7de75df"></a>[`public Inno.Rendering.IRenderLayerCompositionProgramProvider Inno.Adapter.Rendering.RenderingBackendCatalog.CreateCompositionProgramProvider(Inno.Adapter.Rendering.RenderingBackendId backend)`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendCatalog.cs#L78) | Resolves a backend-owned program provider for ordered render layer composition.<br>返回：A provider compatible with devices created by the same backend registration.<br>`backend`：Stable identity of the selected rendering backend. | — | `System.NotSupportedException`：No provider is registered for this exact backend identity. |
| <a id="api-5884c41e906c795e"></a>[`public Inno.Rendering.IRenderDevice Inno.Adapter.Rendering.RenderingBackendCatalog.CreateDevice(Inno.Adapter.Rendering.RenderingBackendId backend, Inno.Adapter.Rendering.RenderingBackendOptions options)`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendCatalog.cs#L55) | Creates a device using this implementation's validated inputs.<br>返回：The validated irender device that represents the completed operation.<br>`backend`：The backend consumed by create device; ownership remains with the caller unless explicitly stated otherwise.<br>`options`：The validated configuration that controls this operation. | — | `System.NotSupportedException`：No provider is registered for this exact backend ID. |
| <a id="api-a935b65febdf7db0"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Adapter.Rendering.RenderingBackendId> Inno.Adapter.Rendering.RenderingBackendCatalog.supportedBackends { get; }`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendCatalog.cs#L38) | Gets backend registrations available in this type generation. | — | — |
| <a id="api-9f8daef0b4422088"></a>[`public sealed class Inno.Adapter.Rendering.RenderingBackendCatalog : Inno.Adapter.Rendering.IRenderingBackendFactory`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendCatalog.cs#L11) | Resolves runtime providers from one immutable composition-owned registration snapshot. | — | — |

### `Inno.Adapter.Rendering.RenderingBackendId`

<!-- api: T:Inno.Adapter.Rendering.RenderingBackendId fb36ca12fb600fc048ea2667f90ab8fff392cbcb0ff4fcef6943c645d76b90b5 -->
<!-- api: M:Inno.Adapter.Rendering.RenderingBackendId.#ctor(System.String) f9bcfe266b686251e208af1d4ae55029982850ac032ffd5981b4a9a7bfd9199b -->
<!-- api: P:Inno.Adapter.Rendering.RenderingBackendId.bgfx dacffb2ce760fff34ed1c617011f390e8737c55c67c9fcf42ef5d1706763afb4 -->
<!-- api: P:Inno.Adapter.Rendering.RenderingBackendId.value 1febb70c71648745494a3121301d8419043bb48da215aebd8f22ad1dc7a2a8d9 -->
<!-- api: P:Inno.Adapter.Rendering.RenderingBackendId.isValid afb20095321cd64b828ea2312f5ac415948686329551e8339159edc6e1c3f90c -->
<!-- api: M:Inno.Adapter.Rendering.RenderingBackendId.ToString 231eec5d452a3b05485c6e4188ae96a08f2ce9b84f40981cce124d6c15cf1106 -->
<!-- api: M:Inno.Adapter.Rendering.RenderingBackendId.op_Inequality(Inno.Adapter.Rendering.RenderingBackendId,Inno.Adapter.Rendering.RenderingBackendId) b562fcd857d993394aecd4e724484a841f3347a263e319b337a306ca404e470e -->
<!-- api: M:Inno.Adapter.Rendering.RenderingBackendId.op_Equality(Inno.Adapter.Rendering.RenderingBackendId,Inno.Adapter.Rendering.RenderingBackendId) 4079824a3dc48dcb974430279006199abd2b306e8da31e0bd2f6253a29e900cc -->
<!-- api: M:Inno.Adapter.Rendering.RenderingBackendId.GetHashCode d7967f7cdb0e85b2339c963f6ed8dc68fee7e4d2af971fdcb2dbdeb1d2cc6e1a -->
<!-- api: M:Inno.Adapter.Rendering.RenderingBackendId.Equals(System.Object) bee109a90ccfb64abe38696f2ee5f1c47ddf6b4489a06398ebf927ea10fc3d16 -->
<!-- api: M:Inno.Adapter.Rendering.RenderingBackendId.Equals(Inno.Adapter.Rendering.RenderingBackendId) c79d9ac7e9ad7462a21a5afc729b1b82538d8bc2e73940111e50bdef930c9510 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-98e85f6f737968c1"></a>[`public Inno.Adapter.Rendering.RenderingBackendId.RenderingBackendId(string value)`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendId.cs#L19) | Creates an ordinal, case-sensitive backend identifier.<br>`value`：Stable nonempty implementation identifier. | — | `System.ArgumentException`：The identifier is empty or contains whitespace. |
| <a id="api-44c3acdf6ee7d764"></a>[`public bool Inno.Adapter.Rendering.RenderingBackendId.Equals(Inno.Adapter.Rendering.RenderingBackendId other)`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendId.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-ff547f478a6d32bb"></a>[`public override bool Inno.Adapter.Rendering.RenderingBackendId.Equals(object obj)`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendId.cs#L8) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-d7037aca422a7956"></a>[`public override int Inno.Adapter.Rendering.RenderingBackendId.GetHashCode()`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendId.cs#L8) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-807edebc97bd5e46"></a>[`public static bool Inno.Adapter.Rendering.RenderingBackendId.operator ==(Inno.Adapter.Rendering.RenderingBackendId left, Inno.Adapter.Rendering.RenderingBackendId right)`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendId.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-811268bf1b1aca8e"></a>[`public static bool Inno.Adapter.Rendering.RenderingBackendId.operator !=(Inno.Adapter.Rendering.RenderingBackendId left, Inno.Adapter.Rendering.RenderingBackendId right)`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendId.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-880fa2ed786a8e3e"></a>[`public override string Inno.Adapter.Rendering.RenderingBackendId.ToString()`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendId.cs#L49) | Returns the identifier without resolving or creating a device.<br>返回：The stable identifier, or an empty string when unassigned. | — | — |
| <a id="api-f257f920e71781b5"></a>[`public static Inno.Adapter.Rendering.RenderingBackendId Inno.Adapter.Rendering.RenderingBackendId.bgfx { get; }`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendId.cs#L31) | Gets the identifier of the bundled BGFX implementation, not a whitelist of supported backends. | — | — |
| <a id="api-e9486481728c86b4"></a>[`public bool Inno.Adapter.Rendering.RenderingBackendId.isValid { get; }`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendId.cs#L41) | Gets whether this value identifies an implementation. | — | — |
| <a id="api-b73d789803951084"></a>[`public string Inno.Adapter.Rendering.RenderingBackendId.value { get; }`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendId.cs#L36) | Gets the stable implementation identifier; the default value is unassigned. | — | — |
| <a id="api-5bdd65f0b8886bb4"></a>[`public readonly record struct Inno.Adapter.Rendering.RenderingBackendId : System.IEquatable<Inno.Adapter.Rendering.RenderingBackendId>`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendId.cs#L8) | Identifies a rendering implementation independently from its graphics API and compiler language. | — | — |

### `Inno.Adapter.Rendering.RenderingBackendOptions`

<!-- api: T:Inno.Adapter.Rendering.RenderingBackendOptions 929ff86a78b2919bb810e4d7590c09441dbd1ddb679947d2f106ade5ef0a7b8a -->
<!-- api: P:Inno.Adapter.Rendering.RenderingBackendOptions.window d668a08f51bd03c8decca70cf4d4b0715d418005b0341577ecec30d85482ad31 -->
<!-- api: P:Inno.Adapter.Rendering.RenderingBackendOptions.preferredGraphicsApi 643dbfa6247cda774ca7d94813367bb06ba48d3c7623d7d5954b1f3785b8afee -->
<!-- api: P:Inno.Adapter.Rendering.RenderingBackendOptions.verticalSync b5cbf8836d767a0243709d26578c52bf338edef67db2b45398246479742f4a1b -->
<!-- api: P:Inno.Adapter.Rendering.RenderingBackendOptions.sRgbBackbuffer a5bd7dac8d9e9a8145ee9888dbb56570ad72fc379f3d28092f37a6254394e6fc -->
<!-- api: P:Inno.Adapter.Rendering.RenderingBackendOptions.forceSingleThreaded b388e643f3abcaf57e2189d4364baf33a38afc4f917c00b696f39cf898bd15ec -->
<!-- api: M:Inno.Adapter.Rendering.RenderingBackendOptions.#ctor a0a4db502a3d48727047d1e007de89dae8fdd39d1b4c7b8c018ac7fa52e77a1c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-ee8130116677f040"></a>[`public Inno.Adapter.Rendering.RenderingBackendOptions.RenderingBackendOptions()`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendOptions.cs#L9) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-16589524f47e2ac4"></a>[`public bool Inno.Adapter.Rendering.RenderingBackendOptions.forceSingleThreaded { get; set; }`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendOptions.cs#L34) | Gets or sets whether rendering must execute on the calling thread. | — | — |
| <a id="api-e86a0a152e642806"></a>[`public Inno.Rendering.GraphicsApi? Inno.Adapter.Rendering.RenderingBackendOptions.preferredGraphicsApi { get; set; }`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendOptions.cs#L19) | Gets or sets the preferred graphics API, or null for the backend default. | — | — |
| <a id="api-dab194b2668c2fe4"></a>[`public bool Inno.Adapter.Rendering.RenderingBackendOptions.sRgbBackbuffer { get; set; }`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendOptions.cs#L29) | Gets or sets whether the primary backbuffer performs sRGB encoding. | — | — |
| <a id="api-d23e048a4da5100c"></a>[`public bool Inno.Adapter.Rendering.RenderingBackendOptions.verticalSync { get; set; }`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendOptions.cs#L24) | Gets or sets whether presentation waits for display synchronization. | — | — |
| <a id="api-a50e194ed357ec71"></a>[`public Inno.Platform.IPlatformWindow? Inno.Adapter.Rendering.RenderingBackendOptions.window { get; set; }`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendOptions.cs#L14) | Gets or sets the primary platform window used as the presentation surface. | — | — |
| <a id="api-114363fb04012ddf"></a>[`public sealed class Inno.Adapter.Rendering.RenderingBackendOptions`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendOptions.cs#L9) | Configures backend-neutral rendering-device creation. | — | — |

### `Inno.Adapter.Rendering.RenderingBackendProvider`

<!-- api: T:Inno.Adapter.Rendering.RenderingBackendProvider 6e91c0670a808b6e3505e6c20143edbb91cf357480a8025999680ea406e3cc24 -->
<!-- api: M:Inno.Adapter.Rendering.RenderingBackendProvider.#ctor(Inno.Adapter.Rendering.RenderingBackendId) 58679e0f1e0915c2902bacffde177764f35085e0d093df359c08d90274658aef -->
<!-- api: P:Inno.Adapter.Rendering.RenderingBackendProvider.id 9df046be77944489ffddf8d03f3c2b22408b171a45e38dfc7256d9529ab32277 -->
<!-- api: M:Inno.Adapter.Rendering.RenderingBackendProvider.CreateDevice(Inno.Adapter.Rendering.RenderingBackendOptions) de5b78208f8488a0cf3f8273ad2a50658c6180b024aadf05d13e6bf24cf65862 -->
<!-- api: M:Inno.Adapter.Rendering.RenderingBackendProvider.CreateCompositionProgramProvider 0ddb6a7782ce28c07be18a9d18b20a849a733042a5404c7599448fef636f9567 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-ea6f036432e3746c"></a>[`protected Inno.Adapter.Rendering.RenderingBackendProvider.RenderingBackendProvider(Inno.Adapter.Rendering.RenderingBackendId id)`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendProvider.cs#L20) | Captures the registration identity assigned by the composition owner.<br>`id`：The assigned implementation identity. | — | `System.ArgumentException`：The identity is unassigned. |
| <a id="api-21bc8e9163f7a393"></a>[`public abstract Inno.Rendering.IRenderLayerCompositionProgramProvider Inno.Adapter.Rendering.RenderingBackendProvider.CreateCompositionProgramProvider()`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendProvider.cs#L49) | Creates the backend-owned program provider used when the host composites render layers.<br>返回：A provider compatible with devices created by this rendering implementation. | — | — |
| <a id="api-7242b3e1587aafba"></a>[`public abstract Inno.Rendering.IRenderDevice Inno.Adapter.Rendering.RenderingBackendProvider.CreateDevice(Inno.Adapter.Rendering.RenderingBackendOptions options)`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendProvider.cs#L41) | Creates a caller-owned device using backend-neutral surface options.<br>返回：A new device owned by the caller.<br>`options`：Validated surface and device creation options. | — | — |
| <a id="api-904d6b88b8ac6acf"></a>[`public Inno.Adapter.Rendering.RenderingBackendId Inno.Adapter.Rendering.RenderingBackendProvider.id { get; }`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendProvider.cs#L30) | Gets the stable implementation identity paired with its authoring tools. | — | — |
| <a id="api-f84b936ef5d5d204"></a>[`public abstract class Inno.Adapter.Rendering.RenderingBackendProvider`](../../src/adapters/rendering/Inno.Adapter.Rendering/RenderingBackendProvider.cs#L9) | Supplies one rendering implementation registered by a composition owner. | — | — |

## 项目依赖

- [Inno.Rendering](Inno.Rendering.md)：公开引用边界由实际签名核对。
- [Inno.Platform](../platform/Inno.Platform.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
