# Inno.Adapter.Audio

[Audio 索引](README.md) · [中立 Audio](Inno.Audio.md) · [MiniAudio implementation](../backends/MiniAudio/Inno.Adapter.Audio.MiniAudio.md)

该项目定义 Audio Adapter family，不包含 MiniAudio binding 或 `Ma*` 类型。

## 公开 API

- `AudioBackendId`：Composition 启动时使用的设备 implementation 选择。
- `AudioBackendOptions`：只描述后端中立的设备启动策略；`noDevice` 用于无硬件运行和测试。
- `IAudioBackendFactory.CreateDevice`：返回 caller-owned `IAudioDevice`。

```csharp
using Inno.Adapter.Audio;
using Inno.Audio;

static IAudioDevice CreateOwnedDevice(
    IAudioBackendFactory factory,
    AudioBackendId selection
) {
    return factory.CreateDevice(selection, new AudioBackendOptions { noDevice = true });
}
```

设备丢失、muted state、voice/mixer 语义仍由 `Inno.Audio` / `Inno.Audio.Runtime` 处理。Factory 不包装 gameplay AudioSource、BGM 或 Dialogue 模型。

## 开放注册与生命周期

后端 ID 是开放的 ordinal 字符串，不能包含空白；默认 struct 未赋值。内置 ID `miniAudio` 只提供默认组合，不限制第三方实现。

| API | 当前语义 |
| --- | --- |
| `AudioBackendId(string)`；`value`、`isValid`、`ToString()` | 创建、检查并显示稳定 ID；无效构造抛出 `ArgumentException`。 |
| `AudioBackendProvider(AudioBackendId)`（protected）；`id` | composition 显式配置的不可变注册描述；provider 不执行类型发现。 |
| `AudioBackendProvider.CreateDevice` | 实现者的创建扩展点；返回 caller-owned `IAudioDevice`，不允许 null。 |
| `AudioBackendCatalog(IEnumerable<AudioBackendProvider>)` | 捕获完整注册快照；重复或 null provider 在构造时失败；不创建设备。 |
| `AudioBackendCatalog.supportedBackends / CreateDevice` | 只解析当前快照中的 exact ID；未注册抛出 `NotSupportedException`，null 产品抛出 `InvalidOperationException`。 |
| `IAudioBackendFactory.supportedBackends` | 启动前能力预检使用的只读注册列表。 |

provider 及其 delegate/资源由 composition owner 释放；catalog 不接管 provider。创建出的服务由调用方释放。源码扩展若通过 TypeRegistry 发现，其 ID 仍由发现协议的 Attribute 声明；此处是宿主明确传入的 provider 集合，不额外扫描程序集。

`AdapterSelection.Validate(catalog)` 在初始化任何窗口或设备前检查全部领域。可在 composition 为 provider 传入任意分配的 `AudioBackendId`，无需新增枚举或修改中央分支。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Adapter.Audio.AudioBackendCatalog`

<!-- api: T:Inno.Adapter.Audio.AudioBackendCatalog d90d6e3a3e313affb81d33aa98965ab019b52da0e45e15a61fcabc206e36740e -->
<!-- api: M:Inno.Adapter.Audio.AudioBackendCatalog.#ctor(System.Collections.Generic.IEnumerable{Inno.Adapter.Audio.AudioBackendProvider}) 459055d2f1565b98055d2269914ebb26c1adf7bb9bdb345a6dc6306acece5e26 -->
<!-- api: P:Inno.Adapter.Audio.AudioBackendCatalog.supportedBackends d09cdb7cd59dc862499b886664923abff6944efa509e8ca0911618c177c92cce -->
<!-- api: M:Inno.Adapter.Audio.AudioBackendCatalog.CreateDevice(Inno.Adapter.Audio.AudioBackendId,Inno.Adapter.Audio.AudioBackendOptions) 39e5fe760dbbed05f555a46029e0d65364c546ac5a3fcfce90bda9c088bc774b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d9878eee32e3e048"></a>[`public Inno.Adapter.Audio.AudioBackendCatalog.AudioBackendCatalog(System.Collections.Generic.IEnumerable<Inno.Adapter.Audio.AudioBackendProvider> providers)`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendCatalog.cs#L27) | Validates and captures a complete provider set without creating any service.<br>`providers`：Providers whose lifetime remains with the composition owner. | — | `System.ArgumentNullException`：The provider sequence is null.<br>`System.ArgumentException`：A provider is null, unassigned, or duplicates a registration identity. |
| <a id="api-1252df96a1df144a"></a>[`public Inno.Audio.IAudioDevice Inno.Adapter.Audio.AudioBackendCatalog.CreateDevice(Inno.Adapter.Audio.AudioBackendId backend, Inno.Adapter.Audio.AudioBackendOptions options)`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendCatalog.cs#L45) | Creates a new audio device for one runtime audio generation.<br>返回：A caller-owned backend-neutral audio device.<br>`backend`：audio implementation selected by the composition root.<br>`options`：Backend-neutral device options. | — | `System.NotSupportedException`：Thrown when the catalog does not contain the selected backend.<br>`System.InvalidOperationException`：The selected provider returned no service. |
| <a id="api-a7b93dab60af3159"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Adapter.Audio.AudioBackendId> Inno.Adapter.Audio.AudioBackendCatalog.supportedBackends { get; }`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendCatalog.cs#L39) | Gets the exact registrations available in this composition snapshot. | — | — |
| <a id="api-42de89e2649e3339"></a>[`public sealed class Inno.Adapter.Audio.AudioBackendCatalog : Inno.Adapter.Audio.IAudioBackendFactory`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendCatalog.cs#L11) | Resolves audio providers from one immutable, composition-owned registration snapshot. | — | — |

### `Inno.Adapter.Audio.AudioBackendId`

<!-- api: T:Inno.Adapter.Audio.AudioBackendId 4e12fd4df8d44c391ba9d763e91ffdffbceeedddf00f1b4d082ac7365f68d46e -->
<!-- api: M:Inno.Adapter.Audio.AudioBackendId.#ctor(System.String) 1b7d04803907ff14de316da0a08340684a324e58fef813344d340e1f6b57ef55 -->
<!-- api: P:Inno.Adapter.Audio.AudioBackendId.miniAudio c86c86a14a19b7766d94e8ecca31777ca6ad61f64f92e58e91619cb2eea7c106 -->
<!-- api: P:Inno.Adapter.Audio.AudioBackendId.value e812f1aa6988efa7298a1cb6bdb53765f5ce889a10bee3746fdce52f24a1e910 -->
<!-- api: P:Inno.Adapter.Audio.AudioBackendId.isValid 938d641964d5f3c9bf0d2b06240b4cef49ddbe7c9cd8806f5d059e758563cf0f -->
<!-- api: M:Inno.Adapter.Audio.AudioBackendId.ToString 8ecf668309c2c317a40b8aa10f17eacf893da2a0a0de08e266be96c84a7d5ab0 -->
<!-- api: M:Inno.Adapter.Audio.AudioBackendId.op_Inequality(Inno.Adapter.Audio.AudioBackendId,Inno.Adapter.Audio.AudioBackendId) 2c0592e30053f61fb7ae5d90f062e705f2181fb44a0d9e4c2133707f1cee7dc2 -->
<!-- api: M:Inno.Adapter.Audio.AudioBackendId.op_Equality(Inno.Adapter.Audio.AudioBackendId,Inno.Adapter.Audio.AudioBackendId) 5630f0ba187eec3011fac7fa3a919111fdf7a4ac111b348479ae52cf22d069e2 -->
<!-- api: M:Inno.Adapter.Audio.AudioBackendId.GetHashCode e5a68896ce033d5fcba079c23fce739072994274ecf466c4d78f539d04b3be0e -->
<!-- api: M:Inno.Adapter.Audio.AudioBackendId.Equals(System.Object) 56ef4ad3e13b2e080c08dd216b665d813e27c9c7d8de1ed3923352ffe7cda37e -->
<!-- api: M:Inno.Adapter.Audio.AudioBackendId.Equals(Inno.Adapter.Audio.AudioBackendId) 650161d0b30af8c5a2ba1a4e9c77cf707e55ee93417923c99d89ff6c3bf908a6 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3da8d215e7abab34"></a>[`public Inno.Adapter.Audio.AudioBackendId.AudioBackendId(string value)`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendId.cs#L19) | Creates an ordinal, case-sensitive implementation identifier.<br>`value`：A stable nonempty identifier containing no whitespace. | — | `System.ArgumentException`：The identifier is empty or contains whitespace. |
| <a id="api-7b8c2d9e8a336cb3"></a>[`public bool Inno.Adapter.Audio.AudioBackendId.Equals(Inno.Adapter.Audio.AudioBackendId other)`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendId.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-5fbe77a8a4c2190e"></a>[`public override bool Inno.Adapter.Audio.AudioBackendId.Equals(object obj)`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendId.cs#L8) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-0d7a324289c9f854"></a>[`public override int Inno.Adapter.Audio.AudioBackendId.GetHashCode()`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendId.cs#L8) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-1b10b83326f4a578"></a>[`public static bool Inno.Adapter.Audio.AudioBackendId.operator ==(Inno.Adapter.Audio.AudioBackendId left, Inno.Adapter.Audio.AudioBackendId right)`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendId.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-e348feadac154bb8"></a>[`public static bool Inno.Adapter.Audio.AudioBackendId.operator !=(Inno.Adapter.Audio.AudioBackendId left, Inno.Adapter.Audio.AudioBackendId right)`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendId.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-e17b508006d7ad74"></a>[`public override string Inno.Adapter.Audio.AudioBackendId.ToString()`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendId.cs#L49) | Returns the identifier without resolving a provider.<br>返回：The stable identifier, or an empty string when unassigned. | — | — |
| <a id="api-aff1371cd126ad77"></a>[`public bool Inno.Adapter.Audio.AudioBackendId.isValid { get; }`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendId.cs#L41) | Gets whether this value identifies an implementation. | — | — |
| <a id="api-e05b0259e28438c9"></a>[`public static Inno.Adapter.Audio.AudioBackendId Inno.Adapter.Audio.AudioBackendId.miniAudio { get; }`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendId.cs#L31) | Gets the identifier of the bundled miniAudio implementation. | — | — |
| <a id="api-7dd8bfcf11e78707"></a>[`public string Inno.Adapter.Audio.AudioBackendId.value { get; }`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendId.cs#L36) | Gets the stable identifier; a default value is unassigned. | — | — |
| <a id="api-6537abbdfdfd4d0c"></a>[`public readonly record struct Inno.Adapter.Audio.AudioBackendId : System.IEquatable<Inno.Adapter.Audio.AudioBackendId>`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendId.cs#L8) | Identifies a audio implementation without closing the set of supported backends. | — | — |

### `Inno.Adapter.Audio.AudioBackendOptions`

<!-- api: T:Inno.Adapter.Audio.AudioBackendOptions ce7985f85c5b5ee7286c39010f1065f145857079b965197822a3ef7ffc31767e -->
<!-- api: M:Inno.Adapter.Audio.AudioBackendOptions.#ctor 786e76f00b8d585af47212829fb7f2fc36f8ec8aad91efd05c22add96c6639ac -->
<!-- api: P:Inno.Adapter.Audio.AudioBackendOptions.noDevice 862d1a9af6c8ecb8167b6d4343178daa7a3e61c46f0a1ad76e47516d40944b5e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-303c03f16d6c5308"></a>[`public Inno.Adapter.Audio.AudioBackendOptions.AudioBackendOptions()`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendOptions.cs#L6) | Configures backend-neutral audio device creation. | — | — |
| <a id="api-e08905d1e099a10a"></a>[`public bool Inno.Adapter.Audio.AudioBackendOptions.noDevice { get; init; }`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendOptions.cs#L11) | Gets whether the device must run without opening an operating-system playback endpoint. | — | — |
| <a id="api-965e243c5ba236e0"></a>[`public readonly struct Inno.Adapter.Audio.AudioBackendOptions`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendOptions.cs#L6) | Configures backend-neutral audio device creation. | — | — |

### `Inno.Adapter.Audio.AudioBackendProvider`

<!-- api: T:Inno.Adapter.Audio.AudioBackendProvider 66e5a8442bc7c4b735efd9384fe266778331ff98e6a9a3fa064240c303f86c4d -->
<!-- api: M:Inno.Adapter.Audio.AudioBackendProvider.#ctor(Inno.Adapter.Audio.AudioBackendId) 1bbd342743be381ca72365fe631c69489bd665c4f22451e1f6ea1d6a1986380d -->
<!-- api: P:Inno.Adapter.Audio.AudioBackendProvider.id 414da21633cd63748d1ed9784b7073e1ced5c31f1ee4845c594019911a8546aa -->
<!-- api: M:Inno.Adapter.Audio.AudioBackendProvider.CreateDevice(Inno.Adapter.Audio.AudioBackendOptions) f0c3dd0823025b8c28ed8e4675b95174a68916cf9013d3041100047278abaa57 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-401fa4323e94e8f6"></a>[`protected Inno.Adapter.Audio.AudioBackendProvider.AudioBackendProvider(Inno.Adapter.Audio.AudioBackendId id)`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendProvider.cs#L24) | Captures the identity assigned by the composition owner.<br>`id`：The assigned implementation identity. | — | `System.ArgumentException`：The identity is unassigned. |
| <a id="api-b836a7a3162a232b"></a>[`public abstract Inno.Audio.IAudioDevice Inno.Adapter.Audio.AudioBackendProvider.CreateDevice(Inno.Adapter.Audio.AudioBackendOptions options)`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendProvider.cs#L45) | Creates a caller-owned audio service using this implementation.<br>返回：A new service owned by the caller; providers must not return null.<br>`options`：Backend-neutral device creation options. | — | — |
| <a id="api-08a4488ef639fe95"></a>[`public Inno.Adapter.Audio.AudioBackendId Inno.Adapter.Audio.AudioBackendProvider.id { get; }`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendProvider.cs#L34) | Gets this registration's immutable implementation identity. | — | — |
| <a id="api-ab7b1a41e14a0c0b"></a>[`public abstract class Inno.Adapter.Audio.AudioBackendProvider`](../../src/adapters/audio/Inno.Adapter.Audio/AudioBackendProvider.cs#L13) | Describes one explicitly composed audio implementation and its creation boundary. | The composition owns this provider and its immutable registration identity. Products are owned by their callers. This registration does not perform type discovery. | — |

### `Inno.Adapter.Audio.IAudioBackendFactory`

<!-- api: T:Inno.Adapter.Audio.IAudioBackendFactory 1e697396c1f00afa2a5caaf7a7b5bb73d0fb823831ddb414d5576a1f12a8cac4 -->
<!-- api: P:Inno.Adapter.Audio.IAudioBackendFactory.supportedBackends f90e99b5f952de80c413ae2c75ca54577d11e27f55e9cdc891b7b137c7103182 -->
<!-- api: M:Inno.Adapter.Audio.IAudioBackendFactory.CreateDevice(Inno.Adapter.Audio.AudioBackendId,Inno.Adapter.Audio.AudioBackendOptions) 9e60ebf5c626291c5a67ce4baab801139bfda6eacc643874123a067594003f55 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-058b0612a6ec8216"></a>[`Inno.Audio.IAudioDevice Inno.Adapter.Audio.IAudioBackendFactory.CreateDevice(Inno.Adapter.Audio.AudioBackendId backend, Inno.Adapter.Audio.AudioBackendOptions options = default(Inno.Adapter.Audio.AudioBackendOptions))`](../../src/adapters/audio/Inno.Adapter.Audio/IAudioBackendFactory.cs#L31) | Creates a new audio device for one runtime audio generation.<br>返回：A caller-owned backend-neutral audio device.<br>`backend`：audio implementation selected by the composition root.<br>`options`：Backend-neutral device options. | — | `System.NotSupportedException`：Thrown when the catalog does not contain the selected backend. |
| <a id="api-cb64f4a8ed996326"></a>[`System.Collections.Generic.IReadOnlyList<Inno.Adapter.Audio.AudioBackendId> Inno.Adapter.Audio.IAudioBackendFactory.supportedBackends { get; }`](../../src/adapters/audio/Inno.Adapter.Audio/IAudioBackendFactory.cs#L14) | Gets the exact registrations available in this composition snapshot. | — | — |
| <a id="api-f43e796a715bd678"></a>[`public interface Inno.Adapter.Audio.IAudioBackendFactory`](../../src/adapters/audio/Inno.Adapter.Audio/IAudioBackendFactory.cs#L9) | Creates backend-neutral audio devices from explicit backend selections. | — | — |

## 项目依赖

- [Inno.Audio](Inno.Audio.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
