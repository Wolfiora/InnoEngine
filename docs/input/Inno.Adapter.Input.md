# Inno.Adapter.Input

[分类索引](README.md) · [Wiki 首页](../README.md) · [本轮整改计划](../architecture/ARCHITECTURE_CLEANUP_PLAN_2026_10_06.md)

## 职责与边界

提供开放 Input backend catalog 与基于 Core Events 的通用事件输入实现。EventInputSource、EventInputBackend 和 EventInputBackendProvider 不引用 SDL 或 Native；平台 Adapter 将系统事件转成同一 Core Events 协议。

## 生命周期与事件消费

内置 ID 为 `inno.input.events`。source 使用 `Inno.Core.Events` 订阅/分发，为每个 Session 创建独立 backend。Dispose 释放所属订阅；其他 Session 保持可用。已消费事件不能再次影响游戏快照。

Press、Release、Pointer、Wheel 和 Text 使用同一入口。窗口身份、焦点与失焦释放保持明确；Editor GameView 在 presentation 边界筛选事件，Modal、Popup 或前景窗口阻止底层游戏输入。不建立第二个事件总线。

第三方 provider 使用开放稳定 ID 注册。catalog 捕获不可变集合，重复/null provider 在构造失败，未注册 ID 明确报错。provider 由 composition owner 管理，创建出的 source/backend 由调用方释放。

## 验证

InputRuntime、ShellLifecycle 和 EditorGameInputCapture 测试共同覆盖消费、焦点、浮动 GameView、窗口和 Session 隔离。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Adapter.Input.EventInputBackend`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Input.EventInputBackend`](../../src/adapters/input/Inno.Adapter.Input/EventInputBackend.cs#L15) | Accumulates backend-neutral platform events into complete runtime input snapshots. |
| [`Inno.Adapter.Input.EventInputBackend.EventInputBackend(uint windowId)`](../../src/adapters/input/Inno.Adapter.Input/EventInputBackend.cs#L41) | Creates an event-backed input adapter for one platform window. |
| [`Inno.Input.InputSnapshot Inno.Adapter.Input.EventInputBackend.Capture(long frameIndex)`](../../src/adapters/input/Inno.Adapter.Input/EventInputBackend.cs#L137) | Captures an immutable snapshot of the current observable state. |
| [`void Inno.Adapter.Input.EventInputBackend.Dispose()`](../../src/adapters/input/Inno.Adapter.Input/EventInputBackend.cs#L169) | Releases the resources owned by this implementation. |
| [`void Inno.Adapter.Input.EventInputBackend.ProcessEvent(Inno.Core.Events.Event evnt)`](../../src/adapters/input/Inno.Adapter.Input/EventInputBackend.cs#L66) | Applies one backend-neutral event translated by the owning platform application. |

### `Inno.Adapter.Input.EventInputBackendProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Input.EventInputBackendProvider`](../../src/adapters/input/Inno.Adapter.Input/EventInputBackendProvider.cs#L9) | Supplies the common event implementation through the neutral input creation boundary. |
| [`Inno.Adapter.Input.EventInputBackendProvider.EventInputBackendProvider()`](../../src/adapters/input/Inno.Adapter.Input/EventInputBackendProvider.cs#L14) | Creates an explicitly composed registration for the bundled implementation. |
| [`override Inno.Adapter.Input.IInputEventSource Inno.Adapter.Input.EventInputBackendProvider.CreateEventSource(Inno.Platform.IPlatformWindow window, bool acceptAllWindows)`](../../src/adapters/input/Inno.Adapter.Input/EventInputBackendProvider.cs#L17) | See the implemented contract. |

### `Inno.Adapter.Input.EventInputSource`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Input.EventInputSource`](../../src/adapters/input/Inno.Adapter.Input/EventInputSource.cs#L12) | Routes backend-neutral platform events through Core Events to isolated runtime-session input backends. |
| [`Inno.Adapter.Input.EventInputSource.EventInputSource(uint windowId)`](../../src/adapters/input/Inno.Adapter.Input/EventInputSource.cs#L26) | Creates an input source restricted to one window, or to all windows when the identifier is zero. |
| [`Inno.Input.IInputBackend Inno.Adapter.Input.EventInputSource.CreateBackend()`](../../src/adapters/input/Inno.Adapter.Input/EventInputSource.cs#L41) | Creates one isolated backend that receives subsequent events from this source. |
| [`void Inno.Adapter.Input.EventInputSource.Dispose()`](../../src/adapters/input/Inno.Adapter.Input/EventInputSource.cs#L72) | Disconnects all session backends and rejects subsequent event delivery. |
| [`void Inno.Adapter.Input.EventInputSource.ProcessEvent(Inno.Core.Events.Event evnt)`](../../src/adapters/input/Inno.Adapter.Input/EventInputSource.cs#L62) | Dispatches one translated platform event to active session backends, honoring event consumption. |

### `Inno.Adapter.Input.IInputBackendFactory`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Input.IInputBackendFactory`](../../src/adapters/input/Inno.Adapter.Input/IInputBackendFactory.cs#L9) | Creates host-level input event sources from explicit backend selections. |
| [`Inno.Adapter.Input.IInputEventSource Inno.Adapter.Input.IInputBackendFactory.CreateEventSource(Inno.Adapter.Input.InputBackendId backend, Inno.Platform.IPlatformWindow window, bool acceptAllWindows)`](../../src/adapters/input/Inno.Adapter.Input/IInputBackendFactory.cs#L34) | Creates an input event source for one window or for the entire application. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Adapter.Input.InputBackendId> Inno.Adapter.Input.IInputBackendFactory.supportedBackends`](../../src/adapters/input/Inno.Adapter.Input/IInputBackendFactory.cs#L14) | Gets the exact registrations available in this composition snapshot. |

### `Inno.Adapter.Input.IInputEventSource`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Input.IInputEventSource`](../../src/adapters/input/Inno.Adapter.Input/IInputEventSource.cs#L10) | Routes backend-neutral platform events into isolated runtime input backends. |
| [`Inno.Input.IInputBackend Inno.Adapter.Input.IInputEventSource.CreateBackend()`](../../src/adapters/input/Inno.Adapter.Input/IInputEventSource.cs#L18) | Creates an isolated input backend owned by one runtime session. |
| [`void Inno.Adapter.Input.IInputEventSource.ProcessEvent(Inno.Core.Events.Event evnt)`](../../src/adapters/input/Inno.Adapter.Input/IInputEventSource.cs#L26) | Routes one backend-neutral platform event to active input backends. |

### `Inno.Adapter.Input.InputBackendCatalog`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Input.IInputEventSource Inno.Adapter.Input.InputBackendCatalog.CreateEventSource(Inno.Adapter.Input.InputBackendId backend, Inno.Platform.IPlatformWindow window, bool acceptAllWindows)`](../../src/adapters/input/Inno.Adapter.Input/InputBackendCatalog.cs#L45) | See the implemented contract. |
| [`Inno.Adapter.Input.InputBackendCatalog`](../../src/adapters/input/Inno.Adapter.Input/InputBackendCatalog.cs#L11) | Resolves input providers from one immutable, composition-owned registration snapshot. |
| [`Inno.Adapter.Input.InputBackendCatalog.InputBackendCatalog(System.Collections.Generic.IEnumerable<Inno.Adapter.Input.InputBackendProvider> providers)`](../../src/adapters/input/Inno.Adapter.Input/InputBackendCatalog.cs#L27) | Validates and captures a complete provider set without creating any service. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Adapter.Input.InputBackendId> Inno.Adapter.Input.InputBackendCatalog.supportedBackends`](../../src/adapters/input/Inno.Adapter.Input/InputBackendCatalog.cs#L39) | See the implemented contract. |

### `Inno.Adapter.Input.InputBackendId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Input.InputBackendId`](../../src/adapters/input/Inno.Adapter.Input/InputBackendId.cs#L8) | Identifies a input implementation without closing the set of supported backends. |
| [`Inno.Adapter.Input.InputBackendId.InputBackendId(string value)`](../../src/adapters/input/Inno.Adapter.Input/InputBackendId.cs#L19) | Creates an ordinal, case-sensitive implementation identifier. |
| [`bool Inno.Adapter.Input.InputBackendId.isValid`](../../src/adapters/input/Inno.Adapter.Input/InputBackendId.cs#L41) | Gets whether this value identifies an implementation. |
| [`override string Inno.Adapter.Input.InputBackendId.ToString()`](../../src/adapters/input/Inno.Adapter.Input/InputBackendId.cs#L49) | Returns the identifier without resolving a provider. |
| [`static Inno.Adapter.Input.InputBackendId Inno.Adapter.Input.InputBackendId.events`](../../src/adapters/input/Inno.Adapter.Input/InputBackendId.cs#L31) | Gets the identifier of the backend-neutral event accumulator. |
| [`string Inno.Adapter.Input.InputBackendId.value`](../../src/adapters/input/Inno.Adapter.Input/InputBackendId.cs#L36) | Gets the stable identifier; a default value is unassigned. |

### `Inno.Adapter.Input.InputBackendProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Input.InputBackendId Inno.Adapter.Input.InputBackendProvider.id`](../../src/adapters/input/Inno.Adapter.Input/InputBackendProvider.cs#L34) | Gets this registration's immutable implementation identity. |
| [`Inno.Adapter.Input.InputBackendProvider`](../../src/adapters/input/Inno.Adapter.Input/InputBackendProvider.cs#L13) | Describes one explicitly composed input implementation and its creation boundary. |
| [`Inno.Adapter.Input.InputBackendProvider.InputBackendProvider(Inno.Adapter.Input.InputBackendId id)`](../../src/adapters/input/Inno.Adapter.Input/InputBackendProvider.cs#L24) | Captures the identity assigned by the composition owner. |
| [`abstract Inno.Adapter.Input.IInputEventSource Inno.Adapter.Input.InputBackendProvider.CreateEventSource(Inno.Platform.IPlatformWindow window, bool acceptAllWindows)`](../../src/adapters/input/Inno.Adapter.Input/InputBackendProvider.cs#L48) | Creates a caller-owned input service using this implementation. |

## 项目依赖

- [Inno.Input](Inno.Input.md)：公开引用边界由实际签名核对。
- [Inno.Core.Events](../core/Inno.Core.Events.md)：公开引用边界由实际签名核对。
- [Inno.Core.Input](../core/Inno.Core.Input.md)：公开引用边界由实际签名核对。
- [Inno.Core.Mathematics](../core/Inno.Core.Mathematics.md)：公开引用边界由实际签名核对。
- [Inno.Platform](../platform/Inno.Platform.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
