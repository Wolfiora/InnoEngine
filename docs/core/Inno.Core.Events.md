# Inno.Core.Events

`EventDispatcher(queueCapacity = 65536, flushBudget = 4096)` 提供显式背压。`TryEnqueue` 仅在成功时接受事件，容量不足返回 false；`Enqueue` 则抛出。`pendingCount` 可观测积压；`Flush` 只排空调用开始时的有限前缀，递归 enqueue 留到后续帧。`DiscardPending` 是显式丢弃操作，返回数量，不能用作静默处理关键事件的策略。

[上一页：Diagnostics](Inno.Core.Diagnostics.md) · [Core 索引](README.md) · [下一页：Coroutines](Inno.Core.Coroutines.md)

Events 系统由一个 `EventDispatcher` 和多个有序 `EventHub` 构成。Dispatcher 决定 hub 顺序；Hub 决定监听器优先级，并支持只终止当前 hub 或终止全局链。

脚本使用 `InnoEngine.Events` 逻辑命名空间中的 `Event`、`EventDispatcher` 和 `EventHub`。Canvas 通过该系统管理 UI 监听与即时分发；订阅 token 应随脚本生命周期释放。

## 分发模型

- Hub `order` 越大越先执行；相同 order 按 hub 创建顺序。
- 同一 Hub 内 listener `priority` 越大越先执行；相同 priority 按注册顺序。
- 监听具体事件类型时，分发还会沿事件基类向上匹配。
- `HandleInHub()` 停止当前 Hub 剩余 listener，但后续 Hub 仍收到事件。
- `HandleInGlobal()` 停止当前 Hub 和后续全部 Hub。

### 路由与消费生命周期

`Event.isGlobalHandled` 可供路由边界检查全局消费状态。`Event()` 创建独立事件；
派生事件可用 protected `Event(source)` 构造同一个事件的路由表示，共享全局消费状态。
`MouseEvent(source)` 同时保留源窗口 ID，`MouseMovedEvent.WithPosition(x, y)` 改变坐标而不重置消费状态。
原事件或任一表示调用 `HandleInGlobal()` 后，其他表示均被消费，跨 Dispatcher 也不能再次投递。
`HandleInHub()` 仍只作用于当前表示的当前 Hub 调用栈，不扩散到其他表示。

Editor Game View 使用这条公共契约转换窗口坐标；已消费事件在路由入口被拒绝，不能启动键盘或鼠标捕获。
事件只存活于当前输入生命周期，不作为跨 generation 的持久对象定位协议。

## EventDispatcher

| 方法 | 说明 |
| --- | --- |
| `CreateHub(int order = 0)` | 创建并附加一个 Hub。 |
| `Enqueue(Event)` | 线程安全地排队，等待 `Flush()`。 |
| `Flush()` | 排空调用时可见的队列并逐个 `Emit`。 |
| `Emit(Event)` | 立即按 order 分发到全部有效 Hub。 |
| `dispatched` | 有序 Hub 派发完成后的同步观察通知，包括全局已消费事件；不参与消费，owner 退场前必须退订。 |

`dispatched` 在 `Emit` 的正常派发结束后调用，`Flush` 通过同一个入口触发。
它允许边界适配器在消费顺序已经确定后处理未消费输入或释放 capture；不会重新调用 Hub，也不建立第二个队列。
预先全局消费的事件跳过所有 Hub，仍通知 observer。Hub 派发抛异常时不报告完成；observer 异常直接传播。
observer 不能修改消费状态或调用 `HandleInHub`，业务消费仍必须通过有序 Hub 完成。
该公共能力随 `EventDispatcher` 的既有脚本导出进入 `InnoEngine.Events`，没有新增导出清单或平台分支。

## EventHub

| 成员 | 说明 |
| --- | --- |
| `order` | 可动态修改；Dispatcher 会重新排序。 |
| `isValid` | 尚未 Dispose、仍连接活 dispatcher。 |
| `Listen<TEvent>(Action<TEvent>, priority)` | 添加长期监听，返回 dispose 即退订的 token。 |
| `ListenOnce<TEvent>(...)` | 首次调用后自动退订，也可提前 dispose。 |
| `Announce(Event)` | 仅在该 Hub 内立即广播，不走 Dispatcher 链。 |
| `Dispose()` | 清空监听并从 Dispatcher 移除。 |

```csharp
EventDispatcher dispatcher = new();
using EventHub ui = dispatcher.CreateHub(order: 100);
using EventHub game = dispatcher.CreateHub(order: 0);

ui.Listen<KeyPressedEvent>(e =>
{
    if (e.key == KeyCode.Escape)
        e.HandleInHub();
});

game.Listen<KeyEvent>(e => Console.WriteLine(e.key));
dispatcher.Enqueue(new KeyPressedEvent(1, KeyCode.Space));
dispatcher.Flush();
```

`HandleInHub()` 只能在该事件当前正在 Hub dispatch 的调用栈中使用，其他位置调用会抛 `InvalidOperationException`。

## 内置事件类型

所有事件派生自 `Event`，构造参数也会作为只读属性公开。

| 分类 | 类型 | 数据 |
| --- | --- | --- |
| Application | `ApplicationEvent` | 抽象基类 |
| Application | `ApplicationQuitEvent` | 无附加数据 |
| Application | `ApplicationSuspensionChangedEvent` | `isSuspended`，平台或有效宿主暂停通知 |
| Keyboard | `KeyEvent` | `windowId`、`key`、`modifiers` |
| Keyboard | `KeyPressedEvent` | 另有 `repeat` |
| Keyboard | `KeyReleasedEvent` | 基类数据 |
| Mouse | `MouseEvent` | `windowId` |
| Mouse | `MouseMovedEvent` | `x`、`y` |
| Mouse | `MouseScrolledEvent` | `offsetX`、`offsetY` |
| Mouse | `MouseButtonEvent` | `button` |
| Mouse | `MouseButtonPressedEvent` | 基类数据 |
| Mouse | `MouseButtonReleasedEvent` | 基类数据 |
| Window | `WindowEvent` | `windowId` |
| Window | `WindowResizeEvent` | `width`、`height` |
| Window | `WindowCloseEvent` | 基类数据 |
| Window | `WindowFocusChangedEvent` | `isFocused` |
| Window | `WindowVisibilityChangedEvent` | `isVisible`，显示与最小化共同决定 |

Keyboard/Mouse 枚举详见 [Inno.Core.Input](Inno.Core.Input.md)。

## 生命周期与热重载

subscription token、Hub listener delegate 都会强引用处理对象。插件或脚本实例卸载前必须 dispose 订阅；`Layer` 帮助自动处理其通过 protected API 建立的订阅。外部直接注册的 listener 则由调用者负责清理。
`dispatched` 的 event delegate 同样受 owner 生命周期约束；退订后 Dispatcher 不再保留该 observer。通知在当前派发线程执行，不自动切换线程。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Core.Events.ApplicationEvent`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Events.ApplicationEvent`](../../src/foundation/core/Inno.Core.Events/Events/ApplicationEvents.cs#L6) | Base class for application lifecycle events. |

### `Inno.Core.Events.ApplicationQuitEvent`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Events.ApplicationQuitEvent`](../../src/foundation/core/Inno.Core.Events/Events/ApplicationEvents.cs#L13) | Raised when the application requests shutdown. |

### `Inno.Core.Events.ApplicationSuspensionChangedEvent`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Events.ApplicationSuspensionChangedEvent`](../../src/foundation/core/Inno.Core.Events/Events/ApplicationSuspensionChangedEvent.cs#L9) | Reports whether the application must stop advancing frames until the platform resumes it. |
| [`bool Inno.Core.Events.ApplicationSuspensionChangedEvent.isSuspended`](../../src/foundation/core/Inno.Core.Events/Events/ApplicationSuspensionChangedEvent.cs#L14) | Gets whether application execution is suspended after this notification. |

### `Inno.Core.Events.Event`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Events.Event`](../../src/foundation/core/Inno.Core.Events/Events/Event.cs#L9) | Base type for all engine events. |
| [`Inno.Core.Events.Event.Event()`](../../src/foundation/core/Inno.Core.Events/Events/Event.cs#L19) | Creates an event with an independent global consumption lifetime. |
| [`Inno.Core.Events.Event.Event(Inno.Core.Events.Event source)`](../../src/foundation/core/Inno.Core.Events/Events/Event.cs#L30) | Creates a routed representation that shares global consumption with its source event. |
| [`bool Inno.Core.Events.Event.isGlobalHandled`](../../src/foundation/core/Inno.Core.Events/Events/Event.cs#L39) | Gets whether this event or any routed representation has been globally consumed. |
| [`void Inno.Core.Events.Event.HandleInGlobal()`](../../src/foundation/core/Inno.Core.Events/Events/Event.cs#L47) | Marks this event as globally handled. |
| [`void Inno.Core.Events.Event.HandleInHub()`](../../src/foundation/core/Inno.Core.Events/Events/Event.cs#L62) | Marks this event as handled in the current hub only. |

### `Inno.Core.Events.EventDispatcher`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Events.EventDispatcher`](../../src/foundation/core/Inno.Core.Events/EventDispatcher.cs#L12) | Thread-safe event dispatcher that owns a set of instances and routes events to hubs in descending . |
| [`Inno.Core.Events.EventDispatcher.EventDispatcher(int queueCapacity = 65536, int flushBudget = 4096)`](../../src/foundation/core/Inno.Core.Events/EventDispatcher.cs#L44) | Creates a dispatcher with bounded pending work and a finite per-flush budget. |
| [`Inno.Core.Events.EventHub Inno.Core.Events.EventDispatcher.CreateHub(int order = 0)`](../../src/foundation/core/Inno.Core.Events/EventDispatcher.cs#L70) | Creates a new hub attached to this dispatcher. |
| [`System.Action<Inno.Core.Events.Event>? Inno.Core.Events.EventDispatcher.dispatched`](../../src/foundation/core/Inno.Core.Events/EventDispatcher.cs#L30) | Observes an event after its ordered hub dispatch has completed, including globally consumed events. |
| [`bool Inno.Core.Events.EventDispatcher.TryEnqueue(Inno.Core.Events.Event e)`](../../src/foundation/core/Inno.Core.Events/EventDispatcher.cs#L109) | Attempts to enqueue without silently dropping a critical event on overload. |
| [`int Inno.Core.Events.EventDispatcher.DiscardPending()`](../../src/foundation/core/Inno.Core.Events/EventDispatcher.cs#L140) | Releases queued references at a quiescent owner safe point without invoking retired handlers. |
| [`int Inno.Core.Events.EventDispatcher.pendingCount`](../../src/foundation/core/Inno.Core.Events/EventDispatcher.cs#L59) | Gets the number of pending events, including producer reservations. |
| [`void Inno.Core.Events.EventDispatcher.Emit(Inno.Core.Events.Event e)`](../../src/foundation/core/Inno.Core.Events/EventDispatcher.cs#L161) | Immediately dispatches an event to all valid hubs in priority order. |
| [`void Inno.Core.Events.EventDispatcher.Enqueue(Inno.Core.Events.Event e)`](../../src/foundation/core/Inno.Core.Events/EventDispatcher.cs#L91) | Enqueues an event for later processing via . |
| [`void Inno.Core.Events.EventDispatcher.Flush()`](../../src/foundation/core/Inno.Core.Events/EventDispatcher.cs#L124) | Dispatches a bounded prefix of pending events; recursively enqueued work waits for a later flush. |

### `Inno.Core.Events.EventHub`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Events.EventHub`](../../src/foundation/core/Inno.Core.Events/EventHub.cs#L13) | A disposable subscription hub bound to a single . |
| [`System.IDisposable Inno.Core.Events.EventHub.Listen<TEvent>(System.Action<TEvent> handler, int priority = 0)`](../../src/foundation/core/Inno.Core.Events/EventHub.cs#L88) | Subscribes a listener for the specified event type. |
| [`System.IDisposable Inno.Core.Events.EventHub.ListenOnce<TEvent>(System.Action<TEvent> handler, int priority = 0)`](../../src/foundation/core/Inno.Core.Events/EventHub.cs#L132) | Subscribes a one-shot listener for the specified event type. |
| [`bool Inno.Core.Events.EventHub.isValid`](../../src/foundation/core/Inno.Core.Events/EventHub.cs#L57) | Gets whether this hub is still attached to a live dispatcher and not disposed. |
| [`int Inno.Core.Events.EventHub.order`](../../src/foundation/core/Inno.Core.Events/EventHub.cs#L36) | Gets or sets the hub dispatch order. Higher values run earlier. |
| [`void Inno.Core.Events.EventHub.Announce(Inno.Core.Events.Event e)`](../../src/foundation/core/Inno.Core.Events/EventHub.cs#L157) | Immediately dispatches an event inside this hub only. |
| [`void Inno.Core.Events.EventHub.Dispose()`](../../src/foundation/core/Inno.Core.Events/EventHub.cs#L167) | Disposes this hub and removes all listeners in this layer. |

### `Inno.Core.Events.KeyEvent`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Events.KeyEvent`](../../src/foundation/core/Inno.Core.Events/Events/KeyEvents.cs#L19) | Base class for keyboard events. |
| [`Inno.Core.Input.KeyCode Inno.Core.Events.KeyEvent.key`](../../src/foundation/core/Inno.Core.Events/Events/KeyEvents.cs#L34) | Gets the key code for this keyboard event. |
| [`Inno.Core.Input.KeyModifier Inno.Core.Events.KeyEvent.modifiers`](../../src/foundation/core/Inno.Core.Events/Events/KeyEvents.cs#L39) | Gets active key modifiers for this keyboard event. |
| [`uint Inno.Core.Events.KeyEvent.windowId`](../../src/foundation/core/Inno.Core.Events/Events/KeyEvents.cs#L29) | Gets the source window id for this event. |

### `Inno.Core.Events.KeyPressedEvent`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Events.KeyPressedEvent`](../../src/foundation/core/Inno.Core.Events/Events/KeyEvents.cs#L57) | Raised when a key is pressed. |
| [`bool Inno.Core.Events.KeyPressedEvent.repeat`](../../src/foundation/core/Inno.Core.Events/Events/KeyEvents.cs#L68) | Gets whether this key press is an auto-repeat event. |

### `Inno.Core.Events.KeyReleasedEvent`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Events.KeyReleasedEvent`](../../src/foundation/core/Inno.Core.Events/Events/KeyEvents.cs#L83) | Raised when a key is released. |

### `Inno.Core.Events.MouseButtonEvent`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Events.MouseButtonEvent`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L137) | Base class for mouse button events. |
| [`Inno.Core.Input.MouseButton Inno.Core.Events.MouseButtonEvent.button`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L145) | Gets the mouse button for this event. |

### `Inno.Core.Events.MouseButtonPressedEvent`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Events.MouseButtonPressedEvent`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L157) | Raised when a mouse button is pressed. |

### `Inno.Core.Events.MouseButtonReleasedEvent`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Events.MouseButtonReleasedEvent`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L173) | Raised when a mouse button is released. |

### `Inno.Core.Events.MouseEvent`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Events.MouseEvent`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L8) | Base class for mouse events. |
| [`Inno.Core.Events.MouseEvent.MouseEvent(Inno.Core.Events.MouseEvent source)`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L27) | Creates a routed mouse event sharing its source's window and global consumption. |
| [`Inno.Core.Events.MouseEvent.MouseEvent(uint windowId)`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L16) | Creates an independent mouse event belonging to one platform window. |
| [`uint Inno.Core.Events.MouseEvent.windowId`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L32) | Gets the source window id for this event. |

### `Inno.Core.Events.MouseMovedEvent`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Events.MouseMovedEvent`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L39) | Raised when the cursor moves. |
| [`Inno.Core.Events.MouseMovedEvent Inno.Core.Events.MouseMovedEvent.WithPosition(float x, float y)`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L84) | Repositions this movement without starting a new global consumption lifetime. |
| [`Inno.Core.Events.MouseMovedEvent.MouseMovedEvent(uint windowId, float x, float y)`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L53) | Creates a pointer movement in platform-window coordinates. |
| [`float Inno.Core.Events.MouseMovedEvent.x`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L65) | Gets cursor X coordinate. |
| [`float Inno.Core.Events.MouseMovedEvent.y`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L70) | Gets cursor Y coordinate. |

### `Inno.Core.Events.MouseScrolledEvent`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Events.MouseScrolledEvent`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L111) | Raised when mouse wheel scrolls. |
| [`float Inno.Core.Events.MouseScrolledEvent.offsetX`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L120) | Gets horizontal scroll offset. |
| [`float Inno.Core.Events.MouseScrolledEvent.offsetY`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L125) | Gets vertical scroll offset. |

### `Inno.Core.Events.TextInputEvent`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Events.TextInputEvent`](../../src/foundation/core/Inno.Core.Events/Events/KeyEvents.cs#L101) | Raised when the platform commits UTF-8 text through keyboard, IME, or virtual-keyboard input. |
| [`string Inno.Core.Events.TextInputEvent.text`](../../src/foundation/core/Inno.Core.Events/Events/KeyEvents.cs#L114) | Gets the committed Unicode text. |
| [`uint Inno.Core.Events.TextInputEvent.windowId`](../../src/foundation/core/Inno.Core.Events/Events/KeyEvents.cs#L109) | Gets the source window identifier. |

### `Inno.Core.Events.WindowCloseEvent`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Events.WindowCloseEvent`](../../src/foundation/core/Inno.Core.Events/Events/WindowEvents.cs#L53) | Raised when the window requests close. |

### `Inno.Core.Events.WindowEvent`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Events.WindowEvent`](../../src/foundation/core/Inno.Core.Events/Events/WindowEvents.cs#L9) | Base class for window lifecycle events. |
| [`uint Inno.Core.Events.WindowEvent.windowId`](../../src/foundation/core/Inno.Core.Events/Events/WindowEvents.cs#L14) | Gets the source window id for this event. |

### `Inno.Core.Events.WindowFocusChangedEvent`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Events.WindowFocusChangedEvent`](../../src/foundation/core/Inno.Core.Events/Events/WindowEvents.cs#L66) | Raised when a window gains or loses input focus. |
| [`bool Inno.Core.Events.WindowFocusChangedEvent.isFocused`](../../src/foundation/core/Inno.Core.Events/Events/WindowEvents.cs#L74) | Gets whether the window has input focus after the change. |

### `Inno.Core.Events.WindowResizeEvent`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Events.WindowResizeEvent`](../../src/foundation/core/Inno.Core.Events/Events/WindowEvents.cs#L30) | Raised when the window size changes. |
| [`int Inno.Core.Events.WindowResizeEvent.height`](../../src/foundation/core/Inno.Core.Events/Events/WindowEvents.cs#L44) | Gets the new window height. |
| [`int Inno.Core.Events.WindowResizeEvent.width`](../../src/foundation/core/Inno.Core.Events/Events/WindowEvents.cs#L39) | Gets the new window width. |

### `Inno.Core.Events.WindowVisibilityChangedEvent`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Events.WindowVisibilityChangedEvent`](../../src/foundation/core/Inno.Core.Events/Events/WindowVisibilityChangedEvent.cs#L12) | Reports whether a platform window can present visible content. |
| [`bool Inno.Core.Events.WindowVisibilityChangedEvent.isVisible`](../../src/foundation/core/Inno.Core.Events/Events/WindowVisibilityChangedEvent.cs#L20) | Gets whether the window can present visible content after this notification. |

## 项目依赖

- [Inno.Core.Input](Inno.Core.Input.md)：公开引用边界由实际签名核对。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
