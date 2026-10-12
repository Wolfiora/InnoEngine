# Inno.Adapter.Input

[分类索引](README.md) · [Wiki 首页](../README.md)

## 职责与边界

提供开放 Input backend catalog 与基于 Core Events 的通用事件输入实现。EventInputSource、EventInputBackend 和 EventInputBackendProvider 不引用 SDL 或 Native；平台 Adapter 将系统事件转成同一 Core Events 协议。

## 生命周期与事件消费

内置 ID 为 `inno.input.events`。source 使用 `Inno.Core.Events` 订阅/分发，为每个 Session 创建独立 backend。Dispose 释放所属订阅；其他 Session 保持可用。已消费事件不能再次影响游戏快照。

Press、Release、Pointer、Wheel 和 Text 使用同一入口。窗口身份、焦点与失焦释放保持明确；Editor GameView 在 presentation 边界筛选事件，Modal、Popup 或前景窗口阻止底层游戏输入。不建立第二个事件总线。

第三方 provider 使用开放稳定 ID 注册。catalog 捕获不可变集合，重复/null provider 在构造失败，未注册 ID 明确报错。provider 由 composition owner 管理，创建出的 source/backend 由调用方释放。

## 验证

InputRuntime、ShellLifecycle 和 EditorGameInputCapture 测试共同覆盖消费、焦点、浮动 GameView、窗口和 Session 隔离。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Adapter.Input.EventInputBackend`

<!-- api: T:Inno.Adapter.Input.EventInputBackend 3de6603b43fb741660968ab84f83d2e343dac37f49276817d13fd67537b9fbb7 -->
<!-- api: M:Inno.Adapter.Input.EventInputBackend.#ctor(System.UInt32) 3f338962d1bf2b9896aa66f18639372755c287ba6b2763378ec2b11599d9e6c1 -->
<!-- api: M:Inno.Adapter.Input.EventInputBackend.ProcessEvent(Inno.Core.Events.Event) c970b9313abfd3ec6bfac0f093123035267d980335e750515d95843fb5b1e264 -->
<!-- api: M:Inno.Adapter.Input.EventInputBackend.Capture(System.Int64) 81313ec77f5493c99dfda0c4a76eb432c0cacdec5020abf444592b47f3974e20 -->
<!-- api: M:Inno.Adapter.Input.EventInputBackend.Dispose c7c6726ad9361c63654e3b610c5bf2301f93bde2b3428862d55d1dc566c09b24 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4b19d2a5deabca2e"></a>[`public Inno.Adapter.Input.EventInputBackend.EventInputBackend(uint windowId)`](../../src/adapters/input/Inno.Adapter.Input/EventInputBackend.cs#L41) | Creates an event-backed input adapter for one platform window.<br>`windowId`：The translated event window identifier, or zero to accept input from every window. | — | — |
| <a id="api-3cebe24955bbc544"></a>[`public Inno.Input.InputSnapshot Inno.Adapter.Input.EventInputBackend.Capture(long frameIndex)`](../../src/adapters/input/Inno.Adapter.Input/EventInputBackend.cs#L137) | Captures an immutable snapshot of the current observable state.<br>返回：The validated input snapshot that represents the completed operation.<br>`frameIndex`：The monotonic frame identity associated with this operation. | — | — |
| <a id="api-eda9bae81ff7fce6"></a>[`public void Inno.Adapter.Input.EventInputBackend.Dispose()`](../../src/adapters/input/Inno.Adapter.Input/EventInputBackend.cs#L169) | Releases the resources owned by this implementation. | — | — |
| <a id="api-c592985070d15f8e"></a>[`public void Inno.Adapter.Input.EventInputBackend.ProcessEvent(Inno.Core.Events.Event evnt)`](../../src/adapters/input/Inno.Adapter.Input/EventInputBackend.cs#L66) | Applies one backend-neutral event translated by the owning platform application.<br>`evnt`：The event observed before runtime dispatch. | — | `System.ArgumentNullException`：Thrown when evnt is null.<br>`System.ObjectDisposedException`：Thrown after this backend is disposed. |
| <a id="api-0c90a7186f7c398e"></a>[`public sealed class Inno.Adapter.Input.EventInputBackend : Inno.Input.IInputBackend`](../../src/adapters/input/Inno.Adapter.Input/EventInputBackend.cs#L15) | Accumulates backend-neutral platform events into complete runtime input snapshots. | — | — |

### `Inno.Adapter.Input.EventInputBackendProvider`

<!-- api: T:Inno.Adapter.Input.EventInputBackendProvider 782a330b5dd86a1d719e98c5998f91e4f042ff1eeddb46fa2dc299feaedd1029 -->
<!-- api: M:Inno.Adapter.Input.EventInputBackendProvider.#ctor 4aaec93cd6e320f1a18a5ce8850007f89fe5bbefa9da61700705c23b94363616 -->
<!-- api: M:Inno.Adapter.Input.EventInputBackendProvider.CreateEventSource(Inno.Platform.IPlatformWindow,System.Boolean) a9948837586a2cdb287b1dd96f6ba6622f219857f39142788d24582c7ea0a733 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-59910159d954f15e"></a>[`public Inno.Adapter.Input.EventInputBackendProvider.EventInputBackendProvider()`](../../src/adapters/input/Inno.Adapter.Input/EventInputBackendProvider.cs#L14) | Creates an explicitly composed registration for the bundled implementation. | — | — |
| <a id="api-dc961e2bfcb9ba17"></a>[`public override Inno.Adapter.Input.IInputEventSource Inno.Adapter.Input.EventInputBackendProvider.CreateEventSource(Inno.Platform.IPlatformWindow window, bool acceptAllWindows)`](../../src/adapters/input/Inno.Adapter.Input/EventInputBackendProvider.cs#L17) | Creates a caller-owned input service using this implementation.<br>返回：A new service owned by the caller; providers must not return null.<br>`window`：The window supplying accepted input events.<br>`acceptAllWindows`：Whether events from every application window are accepted. | — | — |
| <a id="api-a3cb80d3e37d6f8a"></a>[`public sealed class Inno.Adapter.Input.EventInputBackendProvider : Inno.Adapter.Input.InputBackendProvider`](../../src/adapters/input/Inno.Adapter.Input/EventInputBackendProvider.cs#L9) | Supplies the common event implementation through the neutral input creation boundary. | — | — |

### `Inno.Adapter.Input.EventInputSource`

<!-- api: T:Inno.Adapter.Input.EventInputSource 6cb95109eaa092daf00988d9454f043f86e3878a627803f70645bd08df52fff5 -->
<!-- api: M:Inno.Adapter.Input.EventInputSource.#ctor(System.UInt32) 1c9d84397792c36f71df282214744f5a1f53019aca7764359bc86b7777590d10 -->
<!-- api: M:Inno.Adapter.Input.EventInputSource.CreateBackend 6dae84a09f3001dbaad4782d003c70d5b857fe61e61e1bc986ad45be53409afe -->
<!-- api: M:Inno.Adapter.Input.EventInputSource.ProcessEvent(Inno.Core.Events.Event) b2b260e4197c1f24550886f52b09a93d6759de4564cdf6d0c9cf95080b15de11 -->
<!-- api: M:Inno.Adapter.Input.EventInputSource.Dispose 0eee4584fe8e2e72537a73920b69c77299ca270a391a445b6767c1cab1e22a3b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3794fd70fc6b3202"></a>[`public Inno.Adapter.Input.EventInputSource.EventInputSource(uint windowId)`](../../src/adapters/input/Inno.Adapter.Input/EventInputSource.cs#L26) | Creates an input source restricted to one window, or to all windows when the identifier is zero.<br>`windowId`：The platform window identifier accepted by this source; zero accepts events from every window. | — | — |
| <a id="api-a3641553405f8c16"></a>[`public Inno.Input.IInputBackend Inno.Adapter.Input.EventInputSource.CreateBackend()`](../../src/adapters/input/Inno.Adapter.Input/EventInputSource.cs#L41) | Creates one isolated backend that receives subsequent events from this source.<br>返回：A caller-owned backend whose disposal unregisters it from this source. | — | `System.ObjectDisposedException`：Thrown after this source has been disposed. |
| <a id="api-20275aefae97bc1b"></a>[`public void Inno.Adapter.Input.EventInputSource.Dispose()`](../../src/adapters/input/Inno.Adapter.Input/EventInputSource.cs#L72) | Disconnects all session backends and rejects subsequent event delivery. | — | — |
| <a id="api-20d8895c5612a448"></a>[`public void Inno.Adapter.Input.EventInputSource.ProcessEvent(Inno.Core.Events.Event evnt)`](../../src/adapters/input/Inno.Adapter.Input/EventInputSource.cs#L62) | Dispatches one translated platform event to active session backends, honoring event consumption.<br>`evnt`：The backend-neutral event produced by a platform adapter. | — | `System.ArgumentNullException`：Thrown when evnt is null.<br>`System.ObjectDisposedException`：Thrown after this source has been disposed. |
| <a id="api-07d8bc8b1e347d4c"></a>[`public sealed class Inno.Adapter.Input.EventInputSource : Inno.Adapter.Input.IInputEventSource`](../../src/adapters/input/Inno.Adapter.Input/EventInputSource.cs#L12) | Routes backend-neutral platform events through Core Events to isolated runtime-session input backends. | — | — |

### `Inno.Adapter.Input.IInputBackendFactory`

<!-- api: T:Inno.Adapter.Input.IInputBackendFactory b2430a4737b20511f14884fcdffbac34a3e9b643ac5f34f91fe40fe0a448e127 -->
<!-- api: P:Inno.Adapter.Input.IInputBackendFactory.supportedBackends 5521af61ca122c4dd656354a19471e6fb5d7faff7628720f81777b09efc7c731 -->
<!-- api: M:Inno.Adapter.Input.IInputBackendFactory.CreateEventSource(Inno.Adapter.Input.InputBackendId,Inno.Platform.IPlatformWindow,System.Boolean) 36ffeaab49810357ea6216af5c7cd2ee366cbda432516d276baded3aa4ffccb6 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f86fe93a4b4c703e"></a>[`Inno.Adapter.Input.IInputEventSource Inno.Adapter.Input.IInputBackendFactory.CreateEventSource(Inno.Adapter.Input.InputBackendId backend, Inno.Platform.IPlatformWindow window, bool acceptAllWindows)`](../../src/adapters/input/Inno.Adapter.Input/IInputBackendFactory.cs#L34) | Creates an input event source for one window or for the entire application.<br>返回：A caller-owned backend-neutral input event source.<br>`backend`：input implementation selected by the composition root.<br>`window`：Primary platform window whose input is accepted by the source.<br>`acceptAllWindows`：Whether the source accepts events from every window owned by the application. | — | `System.NotSupportedException`：Thrown when the catalog does not contain the selected backend or the platform is incompatible. |
| <a id="api-f2bf96b91358164a"></a>[`System.Collections.Generic.IReadOnlyList<Inno.Adapter.Input.InputBackendId> Inno.Adapter.Input.IInputBackendFactory.supportedBackends { get; }`](../../src/adapters/input/Inno.Adapter.Input/IInputBackendFactory.cs#L14) | Gets the exact registrations available in this composition snapshot. | — | — |
| <a id="api-384906b1779db00e"></a>[`public interface Inno.Adapter.Input.IInputBackendFactory`](../../src/adapters/input/Inno.Adapter.Input/IInputBackendFactory.cs#L9) | Creates host-level input event sources from explicit backend selections. | — | — |

### `Inno.Adapter.Input.IInputEventSource`

<!-- api: T:Inno.Adapter.Input.IInputEventSource 90c1dc2b045223e262ff30a708bfc62c1e85ada0a82210a96860d352100f3d61 -->
<!-- api: M:Inno.Adapter.Input.IInputEventSource.CreateBackend bc1933e35f179670907cb9b5197ce8a872f188eb225e229e8a27bd3cf3556ef8 -->
<!-- api: M:Inno.Adapter.Input.IInputEventSource.ProcessEvent(Inno.Core.Events.Event) 99dd9fde6de94727f8cfe5516f4190d79be5281ceb3ed4f11aaae9bdf048c3ba -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4cadcaa4705c910e"></a>[`Inno.Input.IInputBackend Inno.Adapter.Input.IInputEventSource.CreateBackend()`](../../src/adapters/input/Inno.Adapter.Input/IInputEventSource.cs#L18) | Creates an isolated input backend owned by one runtime session.<br>返回：A caller-owned input backend connected to this event source. | — | — |
| <a id="api-dee328138c75b10b"></a>[`void Inno.Adapter.Input.IInputEventSource.ProcessEvent(Inno.Core.Events.Event evnt)`](../../src/adapters/input/Inno.Adapter.Input/IInputEventSource.cs#L26) | Routes one backend-neutral platform event to active input backends.<br>`evnt`：Event produced by the active platform application. | — | — |
| <a id="api-2f040db5b109a7f3"></a>[`public interface Inno.Adapter.Input.IInputEventSource : System.IDisposable`](../../src/adapters/input/Inno.Adapter.Input/IInputEventSource.cs#L10) | Routes backend-neutral platform events into isolated runtime input backends. | — | — |

### `Inno.Adapter.Input.InputBackendCatalog`

<!-- api: T:Inno.Adapter.Input.InputBackendCatalog 6f8e70e129b763b4b986055160e902a8fba83a45730332b68a101bc9712e23fd -->
<!-- api: M:Inno.Adapter.Input.InputBackendCatalog.#ctor(System.Collections.Generic.IEnumerable{Inno.Adapter.Input.InputBackendProvider}) 67213f7446bdc7a6577d9649fd9fab9361b5c83f28a51e74dc6f1dfe7eb8f6b1 -->
<!-- api: P:Inno.Adapter.Input.InputBackendCatalog.supportedBackends 58e91bc5680acfeba8970242ed706ced0759cad68df2dbdfa5154aae44fdea5e -->
<!-- api: M:Inno.Adapter.Input.InputBackendCatalog.CreateEventSource(Inno.Adapter.Input.InputBackendId,Inno.Platform.IPlatformWindow,System.Boolean) 1d1c7749184d99984235513317883d170cfc289ccf13a0361805f51b00883669 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-28124222b3896e78"></a>[`public Inno.Adapter.Input.InputBackendCatalog.InputBackendCatalog(System.Collections.Generic.IEnumerable<Inno.Adapter.Input.InputBackendProvider> providers)`](../../src/adapters/input/Inno.Adapter.Input/InputBackendCatalog.cs#L27) | Validates and captures a complete provider set without creating any service.<br>`providers`：Providers whose lifetime remains with the composition owner. | — | `System.ArgumentNullException`：The provider sequence is null.<br>`System.ArgumentException`：A provider is null, unassigned, or duplicates a registration identity. |
| <a id="api-edb6793dd0618c6b"></a>[`public Inno.Adapter.Input.IInputEventSource Inno.Adapter.Input.InputBackendCatalog.CreateEventSource(Inno.Adapter.Input.InputBackendId backend, Inno.Platform.IPlatformWindow window, bool acceptAllWindows)`](../../src/adapters/input/Inno.Adapter.Input/InputBackendCatalog.cs#L45) | Creates an input event source for one window or for the entire application.<br>返回：A caller-owned backend-neutral input event source.<br>`backend`：input implementation selected by the composition root.<br>`window`：Primary platform window whose input is accepted by the source.<br>`acceptAllWindows`：Whether the source accepts events from every window owned by the application. | — | `System.NotSupportedException`：Thrown when the catalog does not contain the selected backend or the platform is incompatible.<br>`System.InvalidOperationException`：The selected provider returned no service. |
| <a id="api-3cf870c5ce461a9d"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Adapter.Input.InputBackendId> Inno.Adapter.Input.InputBackendCatalog.supportedBackends { get; }`](../../src/adapters/input/Inno.Adapter.Input/InputBackendCatalog.cs#L39) | Gets the exact registrations available in this composition snapshot. | — | — |
| <a id="api-70d6c5d220114422"></a>[`public sealed class Inno.Adapter.Input.InputBackendCatalog : Inno.Adapter.Input.IInputBackendFactory`](../../src/adapters/input/Inno.Adapter.Input/InputBackendCatalog.cs#L11) | Resolves input providers from one immutable, composition-owned registration snapshot. | — | — |

### `Inno.Adapter.Input.InputBackendId`

<!-- api: T:Inno.Adapter.Input.InputBackendId 00cb420788d6aa65e614e61453f14b980d178b41ecd3a3b34c10113aa5a341ce -->
<!-- api: M:Inno.Adapter.Input.InputBackendId.#ctor(System.String) 0a428bd6c492ff07bc1f2e12494b3e6237eab44d7b80d51839c29ba347d0e56e -->
<!-- api: P:Inno.Adapter.Input.InputBackendId.events d2b7e72a62838029fd5277c05d29d3ef6b14ea4a517556f3dc35dcfcc53c0a60 -->
<!-- api: P:Inno.Adapter.Input.InputBackendId.value f2cde5b2a2ee3cdb062b5e3313fada7bc1f9aace7ae49f4b03162e96858ca9cb -->
<!-- api: P:Inno.Adapter.Input.InputBackendId.isValid 628dbab458e1d9545c858eac0a102515d5bd1f01257105aeb7e4c3fc0e45f1fa -->
<!-- api: M:Inno.Adapter.Input.InputBackendId.ToString 6007a7f55144d35833479e6f9fa266e23fab8b72fc5b0b2ff863ac3b1f04564a -->
<!-- api: M:Inno.Adapter.Input.InputBackendId.op_Inequality(Inno.Adapter.Input.InputBackendId,Inno.Adapter.Input.InputBackendId) 2f0fbd1d3422939e12fd9174912ba7cb63a9194750cdccc829f0c55a40075ad6 -->
<!-- api: M:Inno.Adapter.Input.InputBackendId.op_Equality(Inno.Adapter.Input.InputBackendId,Inno.Adapter.Input.InputBackendId) 908404e669f146d0937122a4bfec8eec13577c12ddd82cd814b9698982f667bc -->
<!-- api: M:Inno.Adapter.Input.InputBackendId.GetHashCode 6efbcb5c5011020b91590d638e23e3b97a16d40a390c802322cfb8904f3f0f8b -->
<!-- api: M:Inno.Adapter.Input.InputBackendId.Equals(System.Object) 794171c6b34d01a2917648082d834ee2a879fb9535f4a76c441cbec9e1bda29d -->
<!-- api: M:Inno.Adapter.Input.InputBackendId.Equals(Inno.Adapter.Input.InputBackendId) 6564dea394c2919d4c407a39f2aa4deb18d7ce06b3192aa4793e15705050e9e9 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-41d6c7a01ac1e6bc"></a>[`public Inno.Adapter.Input.InputBackendId.InputBackendId(string value)`](../../src/adapters/input/Inno.Adapter.Input/InputBackendId.cs#L19) | Creates an ordinal, case-sensitive implementation identifier.<br>`value`：A stable nonempty identifier containing no whitespace. | — | `System.ArgumentException`：The identifier is empty or contains whitespace. |
| <a id="api-756060ee7a24c1d2"></a>[`public bool Inno.Adapter.Input.InputBackendId.Equals(Inno.Adapter.Input.InputBackendId other)`](../../src/adapters/input/Inno.Adapter.Input/InputBackendId.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-0a15a5b172928e8e"></a>[`public override bool Inno.Adapter.Input.InputBackendId.Equals(object obj)`](../../src/adapters/input/Inno.Adapter.Input/InputBackendId.cs#L8) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-756736c1bd43cf69"></a>[`public override int Inno.Adapter.Input.InputBackendId.GetHashCode()`](../../src/adapters/input/Inno.Adapter.Input/InputBackendId.cs#L8) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-6ddc8350e78faca1"></a>[`public static bool Inno.Adapter.Input.InputBackendId.operator ==(Inno.Adapter.Input.InputBackendId left, Inno.Adapter.Input.InputBackendId right)`](../../src/adapters/input/Inno.Adapter.Input/InputBackendId.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-8f93d42dee98d565"></a>[`public static bool Inno.Adapter.Input.InputBackendId.operator !=(Inno.Adapter.Input.InputBackendId left, Inno.Adapter.Input.InputBackendId right)`](../../src/adapters/input/Inno.Adapter.Input/InputBackendId.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-c935b7b9adfc259d"></a>[`public override string Inno.Adapter.Input.InputBackendId.ToString()`](../../src/adapters/input/Inno.Adapter.Input/InputBackendId.cs#L49) | Returns the identifier without resolving a provider.<br>返回：The stable identifier, or an empty string when unassigned. | — | — |
| <a id="api-e0a21f47327d89c4"></a>[`public static Inno.Adapter.Input.InputBackendId Inno.Adapter.Input.InputBackendId.events { get; }`](../../src/adapters/input/Inno.Adapter.Input/InputBackendId.cs#L31) | Gets the identifier of the backend-neutral event accumulator. | — | — |
| <a id="api-32d363f053688f96"></a>[`public bool Inno.Adapter.Input.InputBackendId.isValid { get; }`](../../src/adapters/input/Inno.Adapter.Input/InputBackendId.cs#L41) | Gets whether this value identifies an implementation. | — | — |
| <a id="api-91f17209689df77d"></a>[`public string Inno.Adapter.Input.InputBackendId.value { get; }`](../../src/adapters/input/Inno.Adapter.Input/InputBackendId.cs#L36) | Gets the stable identifier; a default value is unassigned. | — | — |
| <a id="api-f540cf6609f88e4f"></a>[`public readonly record struct Inno.Adapter.Input.InputBackendId : System.IEquatable<Inno.Adapter.Input.InputBackendId>`](../../src/adapters/input/Inno.Adapter.Input/InputBackendId.cs#L8) | Identifies a input implementation without closing the set of supported backends. | — | — |

### `Inno.Adapter.Input.InputBackendProvider`

<!-- api: T:Inno.Adapter.Input.InputBackendProvider bad6dc024f11e790a45c9885cf69ff10f1bd35ccf9923753abaeeecf0df3394d -->
<!-- api: M:Inno.Adapter.Input.InputBackendProvider.#ctor(Inno.Adapter.Input.InputBackendId) ce76e0f4498ddb6d69b0dc058a770660ffbaa662bab85cb6635b78ef12016ffc -->
<!-- api: P:Inno.Adapter.Input.InputBackendProvider.id 644f4d1c3f105d1a2844038b194061c4bda4f29418970dca4002b7b59b78f4d6 -->
<!-- api: M:Inno.Adapter.Input.InputBackendProvider.CreateEventSource(Inno.Platform.IPlatformWindow,System.Boolean) 1f5d1a7063f39d1cf34fbd7b8e1f2c999fe50593b0fbe270f942dc1c73e0f9ff -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-bd1e45ab14816eb5"></a>[`protected Inno.Adapter.Input.InputBackendProvider.InputBackendProvider(Inno.Adapter.Input.InputBackendId id)`](../../src/adapters/input/Inno.Adapter.Input/InputBackendProvider.cs#L24) | Captures the identity assigned by the composition owner.<br>`id`：The assigned implementation identity. | — | `System.ArgumentException`：The identity is unassigned. |
| <a id="api-8a020f2176c2ce9d"></a>[`public abstract Inno.Adapter.Input.IInputEventSource Inno.Adapter.Input.InputBackendProvider.CreateEventSource(Inno.Platform.IPlatformWindow window, bool acceptAllWindows)`](../../src/adapters/input/Inno.Adapter.Input/InputBackendProvider.cs#L48) | Creates a caller-owned input service using this implementation.<br>返回：A new service owned by the caller; providers must not return null.<br>`window`：The window supplying accepted input events.<br>`acceptAllWindows`：Whether events from every application window are accepted. | — | — |
| <a id="api-8db64cb12891fd50"></a>[`public Inno.Adapter.Input.InputBackendId Inno.Adapter.Input.InputBackendProvider.id { get; }`](../../src/adapters/input/Inno.Adapter.Input/InputBackendProvider.cs#L34) | Gets this registration's immutable implementation identity. | — | — |
| <a id="api-d585604caa2a1412"></a>[`public abstract class Inno.Adapter.Input.InputBackendProvider`](../../src/adapters/input/Inno.Adapter.Input/InputBackendProvider.cs#L13) | Describes one explicitly composed input implementation and its creation boundary. | The composition owns this provider and its immutable registration identity. Products are owned by their callers. This registration does not perform type discovery. | — |

## 项目依赖

- [Inno.Input](Inno.Input.md)：公开引用边界由实际签名核对。
- [Inno.Core.Events](../core/Inno.Core.Events.md)：公开引用边界由实际签名核对。
- [Inno.Core.Input](../core/Inno.Core.Input.md)：公开引用边界由实际签名核对。
- [Inno.Core.Mathematics](../core/Inno.Core.Mathematics.md)：公开引用边界由实际签名核对。
- [Inno.Platform](../platform/Inno.Platform.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
