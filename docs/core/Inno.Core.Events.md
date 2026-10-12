# Inno.Core.Events

`EventDispatcher(queueCapacity = 65536, flushBudget = 4096)` 提供显式背压。`TryEnqueue` 仅在成功时接受事件，容量不足返回 false；`Enqueue` 则抛出。`pendingCount` 可观测积压；`Flush` 只排空调用开始时的有限前缀，递归 enqueue 留到后续帧。`DiscardPending` 是显式丢弃操作，返回数量，不能用作静默处理关键事件的策略。

[上一页：Diagnostics](Inno.Core.Diagnostics.md) · [Core 索引](README.md) · [下一页：Coroutines](Inno.Core.Coroutines.md)

Events 系统由一个 `EventDispatcher` 和多个有序 `EventHub` 构成。Dispatcher 决定 hub 顺序；Hub 决定监听器优先级，并支持只终止当前 hub 或终止全局链。

脚本使用 `InnoEngine.Events` 逻辑命名空间中的 `Event`、`EventDispatcher` 和 `EventHub`。领域调用方通过该系统管理监听与即时分发；订阅 token 应随脚本生命周期释放。

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

### 输入事件组合

示例借用 dispatcher，在作用域中拥有 hub 和两个订阅。离开作用域时先释放订阅，再释放 hub；长期监听者应由相应 Session 或模块持有。

```csharp
using System;
using Inno.Core.Events;
using Inno.Core.Input;

static void DispatchExample(
    EventDispatcher dispatcher,
    Action beginSelection,
    Action startGame
) {
    using EventHub hub = dispatcher.CreateHub();
    using IDisposable mouse = hub.Listen<MouseButtonPressedEvent>(e =>
    {
        if (e.button == MouseButton.Left)
            beginSelection();
    });
    using IDisposable keys = hub.Listen<KeyPressedEvent>(e =>
    {
        if (e.key == KeyCode.F5 && !e.repeat)
            startGame();
    });
    dispatcher.Enqueue(new KeyPressedEvent(1, KeyCode.F5));
    dispatcher.Flush();
}
```

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
using System;
using Inno.Core.Events;
using Inno.Core.Input;

EventDispatcher dispatcher = new();
using EventHub ui = dispatcher.CreateHub(order: 100);
using EventHub game = dispatcher.CreateHub(order: 0);
using IDisposable uiKeys = ui.Listen<KeyPressedEvent>(e =>
{
    if (e.key == KeyCode.Escape)
        e.HandleInHub();
});
using IDisposable gameKeys = game.Listen<KeyEvent>(e => Console.WriteLine(e.key));
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

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Core.Events.ApplicationEvent`

<!-- api: T:Inno.Core.Events.ApplicationEvent 733356a886e170be2dd0e71fbd44b156141c3e33cd759bab687b9c64c9f5ce15 -->
<!-- api: M:Inno.Core.Events.ApplicationEvent.#ctor 6330633548ce1b8f6068b7c14c751962834fa6587196bca5358b4a67e5d41a4a -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-addb28d16cc44db6"></a>[`protected Inno.Core.Events.ApplicationEvent.ApplicationEvent()`](../../src/foundation/core/Inno.Core.Events/Events/ApplicationEvents.cs#L6) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-f47ed6ea5f5d5063"></a>[`public abstract class Inno.Core.Events.ApplicationEvent : Inno.Core.Events.Event`](../../src/foundation/core/Inno.Core.Events/Events/ApplicationEvents.cs#L6) | Base class for application lifecycle events. | — | — |

### `Inno.Core.Events.ApplicationQuitEvent`

<!-- api: T:Inno.Core.Events.ApplicationQuitEvent c5f967e14852d0e6695ceb59ce095b1d05d6e176d7d6c20556adcc46890fa7b9 -->
<!-- api: M:Inno.Core.Events.ApplicationQuitEvent.#ctor 16174ab6809d62db9a937e6ac7a4e2744e4f7d3fad31fec222693dff79754700 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e0973feb576d3ed8"></a>[`public Inno.Core.Events.ApplicationQuitEvent.ApplicationQuitEvent()`](../../src/foundation/core/Inno.Core.Events/Events/ApplicationEvents.cs#L13) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-ae50f5a2115cbdbe"></a>[`public class Inno.Core.Events.ApplicationQuitEvent : Inno.Core.Events.ApplicationEvent`](../../src/foundation/core/Inno.Core.Events/Events/ApplicationEvents.cs#L13) | Raised when the application requests shutdown. | — | — |

### `Inno.Core.Events.ApplicationSuspensionChangedEvent`

<!-- api: T:Inno.Core.Events.ApplicationSuspensionChangedEvent ca96365cafc4c0347145845b2163ca58ffbf379306cbc09074188c7794100b43 -->
<!-- api: M:Inno.Core.Events.ApplicationSuspensionChangedEvent.#ctor(System.Boolean) 6d81b065b18fa8b4c00add214bdfc6665b726212e39b03f26f2cdf72ee98909f -->
<!-- api: P:Inno.Core.Events.ApplicationSuspensionChangedEvent.isSuspended 7fd6d16fba81951a8780c00bfaf251cb32950ba74dbbf391fd978262d955b459 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-bff0ad61bb347584"></a>[`public Inno.Core.Events.ApplicationSuspensionChangedEvent.ApplicationSuspensionChangedEvent(bool isSuspended)`](../../src/foundation/core/Inno.Core.Events/Events/ApplicationSuspensionChangedEvent.cs#L9) | Reports whether the application must stop advancing frames until the platform resumes it.<br>`isSuspended`：Whether the platform has suspended application execution. | — | — |
| <a id="api-7ab9210301dde954"></a>[`public bool Inno.Core.Events.ApplicationSuspensionChangedEvent.isSuspended { get; }`](../../src/foundation/core/Inno.Core.Events/Events/ApplicationSuspensionChangedEvent.cs#L14) | Gets whether application execution is suspended after this notification. | — | — |
| <a id="api-2e381feb534c056b"></a>[`public sealed class Inno.Core.Events.ApplicationSuspensionChangedEvent : Inno.Core.Events.ApplicationEvent`](../../src/foundation/core/Inno.Core.Events/Events/ApplicationSuspensionChangedEvent.cs#L9) | Reports whether the application must stop advancing frames until the platform resumes it.<br>`isSuspended`：Whether the platform has suspended application execution. | — | — |

### `Inno.Core.Events.Event`

<!-- api: T:Inno.Core.Events.Event f5af5e24b63fc8066ea69ebaec7f4132d3fcd4bf3e84c17f572606963cc3db1b -->
<!-- api: M:Inno.Core.Events.Event.#ctor 7d2eec325af6fa57e0e561a0aece07d0f72acfa230ef6290518442b531829f52 -->
<!-- api: M:Inno.Core.Events.Event.#ctor(Inno.Core.Events.Event) fc36c6accfb0d67d697aa057b79656a4968f8ed5e68dac4694e102a1391484e2 -->
<!-- api: P:Inno.Core.Events.Event.isGlobalHandled 3e7e4b6409a77db0eff831fcf74047da5e07c63ac165c6f606bbf3e4f1915388 -->
<!-- api: M:Inno.Core.Events.Event.HandleInGlobal ad5c08f15445f18afb0d0b27aa8bceb86009dd02e17719477c35b4ccc3f604bb -->
<!-- api: M:Inno.Core.Events.Event.HandleInHub 8aad7f8557a8559a5b32f89166c5fe44b0efce520d9907a613fc6bc83bc0bbdb -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-993d8fc124e63193"></a>[`protected Inno.Core.Events.Event.Event()`](../../src/foundation/core/Inno.Core.Events/Events/Event.cs#L19) | Creates an event with an independent global consumption lifetime. | — | — |
| <a id="api-b32e040232458e2b"></a>[`protected Inno.Core.Events.Event.Event(Inno.Core.Events.Event source)`](../../src/foundation/core/Inno.Core.Events/Events/Event.cs#L30) | Creates a routed representation that shares global consumption with its source event.<br>`source`：The original event whose consumption lifetime remains authoritative. | — | `System.ArgumentNullException`：The source event is null. |
| <a id="api-e3501d217ee0dbf1"></a>[`public void Inno.Core.Events.Event.HandleInGlobal()`](../../src/foundation/core/Inno.Core.Events/Events/Event.cs#L47) | Marks this event as globally handled. | Global handling stops dispatch in the current hub and all following hubs. | — |
| <a id="api-5ad73b62bc2308e1"></a>[`public void Inno.Core.Events.Event.HandleInHub()`](../../src/foundation/core/Inno.Core.Events/Events/Event.cs#L62) | Marks this event as handled in the current hub only. | Hub handling stops remaining listeners in the current hub but does not affect following hubs. | `System.InvalidOperationException`：Thrown when called outside a valid hub dispatch context for this event. |
| <a id="api-ce8d88a8e0cbdb97"></a>[`public bool Inno.Core.Events.Event.isGlobalHandled { get; }`](../../src/foundation/core/Inno.Core.Events/Events/Event.cs#L39) | Gets whether this event or any routed representation has been globally consumed. | — | — |
| <a id="api-cf759d40b9fbb747"></a>[`public abstract class Inno.Core.Events.Event`](../../src/foundation/core/Inno.Core.Events/Events/Event.cs#L9) | Base type for all engine events. | — | — |

### `Inno.Core.Events.EventDispatcher`

<!-- api: T:Inno.Core.Events.EventDispatcher 2cb2ca8cfe0af09273766a6e012d35b3d4fe040b297faa8bf1b9a142c7026f56 -->
<!-- api: E:Inno.Core.Events.EventDispatcher.dispatched 3ae753e2a2d9df64771bb3bd7618a1274efda42e0a25a6333ca845006e7750b0 -->
<!-- api: M:Inno.Core.Events.EventDispatcher.#ctor(System.Int32,System.Int32) 43b7fd226ca417105fb289d9707042be0db2a01d856bfa7536e948410216525d -->
<!-- api: P:Inno.Core.Events.EventDispatcher.pendingCount ee94707544f7efd2f67b97282894245e3a78762f4469ce7dd1eb5439cda2bd31 -->
<!-- api: M:Inno.Core.Events.EventDispatcher.CreateHub(System.Int32) dd6c239cb67b4be6ed56125dc19d799eda27f96f8fc3f85e2535b9d4cf2e6648 -->
<!-- api: M:Inno.Core.Events.EventDispatcher.Enqueue(Inno.Core.Events.Event) 44089c5f20605f436a6f38dd7cae5ba82d5bbd3ac59672b6f904619c794ec6da -->
<!-- api: M:Inno.Core.Events.EventDispatcher.TryEnqueue(Inno.Core.Events.Event) ed5075a04b262d88a9d2891ee81f417930911268030339a5a6a3fa936088b182 -->
<!-- api: M:Inno.Core.Events.EventDispatcher.Flush 8ed53447a07ad78e440262bcf0e1f1a57f733801bde2c86f3cfabd4507cd203a -->
<!-- api: M:Inno.Core.Events.EventDispatcher.DiscardPending 2d9e75a7e26fc898deebdc79e1dae3a4a3ab99c053d622202083cc42eb49f05a -->
<!-- api: M:Inno.Core.Events.EventDispatcher.Emit(Inno.Core.Events.Event) ed6ceb4f46c190f2de8f6a2957496901e834dbb6bd61d0dcf761efb96589ba98 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-0aadbbdaf8be9547"></a>[`public System.Action<Inno.Core.Events.Event>? Inno.Core.Events.EventDispatcher.dispatched`](../../src/foundation/core/Inno.Core.Events/EventDispatcher.cs#L30) | Observes an event after its ordered hub dispatch has completed, including globally consumed events. | Observers run synchronously on the dispatching thread and do not participate in consumption. They must not change consumption or call HandleInHub. Observer failures propagate to the caller. Owners must unsubscribe before retiring their callbacks or extensions. | — |
| <a id="api-c5210898d89b2866"></a>[`public Inno.Core.Events.EventDispatcher.EventDispatcher(int queueCapacity = 65536, int flushBudget = 4096)`](../../src/foundation/core/Inno.Core.Events/EventDispatcher.cs#L44) | Creates a dispatcher with bounded pending work and a finite per-flush budget.<br>`queueCapacity`：Maximum pending events; producers must handle explicit rejection.<br>`flushBudget`：Maximum events dispatched by one flush, excluding events enqueued during that flush. | — | `System.ArgumentOutOfRangeException`：A budget is not positive. |
| <a id="api-f307c2f0e0e8beb1"></a>[`public Inno.Core.Events.EventHub Inno.Core.Events.EventDispatcher.CreateHub(int order = 0)`](../../src/foundation/core/Inno.Core.Events/EventDispatcher.cs#L70) | Creates a new hub attached to this dispatcher.<br>返回：The created valid hub.<br>`order`：Hub dispatch order. Higher values run earlier. | — | — |
| <a id="api-ea717acd015c8059"></a>[`public int Inno.Core.Events.EventDispatcher.DiscardPending()`](../../src/foundation/core/Inno.Core.Events/EventDispatcher.cs#L140) | Releases queued references at a quiescent owner safe point without invoking retired handlers.<br>返回：The number of pending events discarded by the owner. | — | — |
| <a id="api-5d9f11a0e08c82fd"></a>[`public void Inno.Core.Events.EventDispatcher.Emit(Inno.Core.Events.Event e)`](../../src/foundation/core/Inno.Core.Events/EventDispatcher.cs#L161) | Immediately dispatches an event to all valid hubs in priority order.<br>`e`：The event instance to dispatch. | Hub dispatch stops when the event is marked globally handled. Completion observers are still notified. A failed hub propagates its exception without reporting successful completion. | — |
| <a id="api-7803abe58ca24049"></a>[`public void Inno.Core.Events.EventDispatcher.Enqueue(Inno.Core.Events.Event e)`](../../src/foundation/core/Inno.Core.Events/EventDispatcher.cs#L91) | Enqueues an event for later processing via Inno.Core.Events.EventDispatcher.Flush.<br>`e`：The event instance to enqueue. | — | `System.InvalidOperationException`：The queue is full; the event was not accepted. |
| <a id="api-44935a267cdc9985"></a>[`public void Inno.Core.Events.EventDispatcher.Flush()`](../../src/foundation/core/Inno.Core.Events/EventDispatcher.cs#L124) | Dispatches a bounded prefix of pending events; recursively enqueued work waits for a later flush. | — | — |
| <a id="api-0479ca4a013df3c7"></a>[`public bool Inno.Core.Events.EventDispatcher.TryEnqueue(Inno.Core.Events.Event e)`](../../src/foundation/core/Inno.Core.Events/EventDispatcher.cs#L109) | Attempts to enqueue without silently dropping a critical event on overload.<br>返回：False when capacity is exhausted; the caller retains the event.<br>`e`：The event whose ownership transfers only on success. | — | `System.ArgumentNullException`：The event is null. |
| <a id="api-124e3d838099a9e3"></a>[`public int Inno.Core.Events.EventDispatcher.pendingCount { get; }`](../../src/foundation/core/Inno.Core.Events/EventDispatcher.cs#L59) | Gets the number of pending events, including producer reservations. | — | — |
| <a id="api-48b0cbce21ed23c6"></a>[`public sealed class Inno.Core.Events.EventDispatcher`](../../src/foundation/core/Inno.Core.Events/EventDispatcher.cs#L12) | Thread-safe event dispatcher that owns a set of Inno.Core.Events.EventHub instances and routes events to hubs in descending Inno.Core.Events.EventHub.order. | — | — |

### `Inno.Core.Events.EventHub`

<!-- api: T:Inno.Core.Events.EventHub 9f6f280c1899fa2b454c1ed1b3baaf319682a647f53c5a1e27a132fe2e9bf5ed -->
<!-- api: P:Inno.Core.Events.EventHub.order 48b0be461b1927c3dc1f99e8d90a4a48c400223a16ad407ab4dca6726aebfe37 -->
<!-- api: P:Inno.Core.Events.EventHub.isValid 1dd1e71f89c09f1385526898e8d36bf086e98c04045c60d6ce97e0fce50fa04c -->
<!-- api: M:Inno.Core.Events.EventHub.Listen``1(System.Action{``0},System.Int32) a350a08e489fccee0420d8a6d5a8f214e6e4f84a4a3843c0309be9ca6280d660 -->
<!-- api: M:Inno.Core.Events.EventHub.ListenOnce``1(System.Action{``0},System.Int32) 2c377f1fc4a515dab06d7334b69df0ebfd01664cb3bb1348e13f1c7b19f69793 -->
<!-- api: M:Inno.Core.Events.EventHub.Announce(Inno.Core.Events.Event) ee3964ab0d03b2fc261a1eb98d79b2058f86253d8e8c02796b982156f1ae7ca1 -->
<!-- api: M:Inno.Core.Events.EventHub.Dispose 3767e5018a15b19fa55b6e8e6f32082b5b25498f3c00fbd5e089c0d733f90507 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2f33adb1d30547ed"></a>[`public void Inno.Core.Events.EventHub.Announce(Inno.Core.Events.Event e)`](../../src/foundation/core/Inno.Core.Events/EventHub.cs#L157) | Immediately dispatches an event inside this hub only.<br>`e`：The event instance to dispatch locally. | This does not route through the dispatcher hub chain. Only listeners registered on this hub are invoked. | — |
| <a id="api-5608de70373d0185"></a>[`public void Inno.Core.Events.EventHub.Dispose()`](../../src/foundation/core/Inno.Core.Events/EventHub.cs#L167) | Disposes this hub and removes all listeners in this layer. | — | — |
| <a id="api-11fd3f619ed209fd"></a>[`public System.IDisposable Inno.Core.Events.EventHub.Listen<TEvent>(System.Action<TEvent> handler, int priority = 0) where TEvent : Inno.Core.Events.Event`](../../src/foundation/core/Inno.Core.Events/EventHub.cs#L88) | Subscribes a listener for the specified event type.<br>返回：A token that unsubscribes this listener when disposed.<br>`handler`：Listener callback.<br>`priority`：Listener priority within this hub. Higher values run earlier. | — | — |
| <a id="api-7264b8ade0d8c9aa"></a>[`public System.IDisposable Inno.Core.Events.EventHub.ListenOnce<TEvent>(System.Action<TEvent> handler, int priority = 0) where TEvent : Inno.Core.Events.Event`](../../src/foundation/core/Inno.Core.Events/EventHub.cs#L132) | Subscribes a one-shot listener for the specified event type.<br>返回：A token that can cancel the one-shot listener before it runs.<br>`handler`：Listener callback.<br>`priority`：Listener priority within this hub. Higher values run earlier. | — | — |
| <a id="api-a196f3245943e8e7"></a>[`public bool Inno.Core.Events.EventHub.isValid { get; }`](../../src/foundation/core/Inno.Core.Events/EventHub.cs#L57) | Gets whether this hub is still attached to a live dispatcher and not disposed. | — | — |
| <a id="api-943ddb34a09a19fa"></a>[`public int Inno.Core.Events.EventHub.order { get; set; }`](../../src/foundation/core/Inno.Core.Events/EventHub.cs#L36) | Gets or sets the hub dispatch order. Higher values run earlier. | — | — |
| <a id="api-cbb7274ff8371a01"></a>[`public sealed class Inno.Core.Events.EventHub : System.IDisposable`](../../src/foundation/core/Inno.Core.Events/EventHub.cs#L13) | A disposable subscription hub bound to a single Inno.Core.Events.EventDispatcher. | Listeners are ordered by priority (descending) and registration order (ascending). | — |

### `Inno.Core.Events.KeyEvent`

<!-- api: T:Inno.Core.Events.KeyEvent af758b230e1c0aaca790b17fb57f7dbc4c87efc87145ec69c4600a4341b1efb2 -->
<!-- api: M:Inno.Core.Events.KeyEvent.#ctor(System.UInt32,Inno.Core.Input.KeyCode,Inno.Core.Input.KeyModifier) 333a602d2fdddd898a0dcb4e123f3369da9cdfdb7552796604ad324ec47e6724 -->
<!-- api: P:Inno.Core.Events.KeyEvent.windowId 1511fc215d8f88bcd929622f64cfd9d6e586c55c33f2308b56b33bf3c076cb4e -->
<!-- api: P:Inno.Core.Events.KeyEvent.key 74dbb5ac93051c391bb99fe4093d7b77a19bf87a004be3e8168330c99aec62ad -->
<!-- api: P:Inno.Core.Events.KeyEvent.modifiers 109739343b2508f4d8af78a6f4b75dcb43d17d44323d16275a75708e3542687d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-29547869d1b17333"></a>[`protected Inno.Core.Events.KeyEvent.KeyEvent(uint windowId, Inno.Core.Input.KeyCode key, Inno.Core.Input.KeyModifier modifiers = Inno.Core.Input.KeyModifier.None)`](../../src/foundation/core/Inno.Core.Events/Events/KeyEvents.cs#L19) | Base class for keyboard events.<br>`windowId`：The window id used to initialize this instance.<br>`key`：The key used to initialize this instance.<br>`modifiers`：The modifiers used to initialize this instance. | — | — |
| <a id="api-401f01ef0e8b9363"></a>[`public Inno.Core.Input.KeyCode Inno.Core.Events.KeyEvent.key { get; }`](../../src/foundation/core/Inno.Core.Events/Events/KeyEvents.cs#L34) | Gets the key code for this keyboard event. | — | — |
| <a id="api-63a2c7927e0173b5"></a>[`public Inno.Core.Input.KeyModifier Inno.Core.Events.KeyEvent.modifiers { get; }`](../../src/foundation/core/Inno.Core.Events/Events/KeyEvents.cs#L39) | Gets active key modifiers for this keyboard event. | — | — |
| <a id="api-d6aa178df216e587"></a>[`public uint Inno.Core.Events.KeyEvent.windowId { get; }`](../../src/foundation/core/Inno.Core.Events/Events/KeyEvents.cs#L29) | Gets the source window id for this event. | — | — |
| <a id="api-8ab808d0fc044e31"></a>[`public abstract class Inno.Core.Events.KeyEvent : Inno.Core.Events.Event`](../../src/foundation/core/Inno.Core.Events/Events/KeyEvents.cs#L19) | Base class for keyboard events.<br>`windowId`：The window id used to initialize this instance.<br>`key`：The key used to initialize this instance.<br>`modifiers`：The modifiers used to initialize this instance. | — | — |

### `Inno.Core.Events.KeyPressedEvent`

<!-- api: T:Inno.Core.Events.KeyPressedEvent e26443acde278bd689a92c4e302c3cc3d3f976b03d039f49e9b871a3fb3b92ae -->
<!-- api: M:Inno.Core.Events.KeyPressedEvent.#ctor(System.UInt32,Inno.Core.Input.KeyCode,Inno.Core.Input.KeyModifier,System.Boolean) c48d666f0cac7b3c8ac09c8b1571eb799820a57ce235cf291aa49cedaa4c63da -->
<!-- api: P:Inno.Core.Events.KeyPressedEvent.repeat dd3cbffed12b4c273c2beb9d5795a1ccd3e32e6b6bf71022cab79a662a14b524 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f7ff84451fac05f9"></a>[`public Inno.Core.Events.KeyPressedEvent.KeyPressedEvent(uint windowId, Inno.Core.Input.KeyCode key, Inno.Core.Input.KeyModifier modifiers = Inno.Core.Input.KeyModifier.None, bool repeat = false)`](../../src/foundation/core/Inno.Core.Events/Events/KeyEvents.cs#L57) | Raised when a key is pressed.<br>`windowId`：The window id used to initialize this instance.<br>`key`：The key used to initialize this instance.<br>`modifiers`：The modifiers used to initialize this instance.<br>`repeat`：The repeat used to initialize this instance. | — | — |
| <a id="api-b553287c374a2676"></a>[`public bool Inno.Core.Events.KeyPressedEvent.repeat { get; }`](../../src/foundation/core/Inno.Core.Events/Events/KeyEvents.cs#L68) | Gets whether this key press is an auto-repeat event. | — | — |
| <a id="api-dec0fcde34bd616f"></a>[`public class Inno.Core.Events.KeyPressedEvent : Inno.Core.Events.KeyEvent`](../../src/foundation/core/Inno.Core.Events/Events/KeyEvents.cs#L57) | Raised when a key is pressed.<br>`windowId`：The window id used to initialize this instance.<br>`key`：The key used to initialize this instance.<br>`modifiers`：The modifiers used to initialize this instance.<br>`repeat`：The repeat used to initialize this instance. | — | — |

### `Inno.Core.Events.KeyReleasedEvent`

<!-- api: T:Inno.Core.Events.KeyReleasedEvent d4671a8ac838d4784c2dc32b4e9d525179797d34e2725ea5afb73c93c81297c2 -->
<!-- api: M:Inno.Core.Events.KeyReleasedEvent.#ctor(System.UInt32,Inno.Core.Input.KeyCode,Inno.Core.Input.KeyModifier) 4ce08ee7eec06a7e2b4bab70ae615d8636d307b756ed9bd1de29dfacd1d45efc -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f49c881e8849e0ff"></a>[`public Inno.Core.Events.KeyReleasedEvent.KeyReleasedEvent(uint windowId, Inno.Core.Input.KeyCode key, Inno.Core.Input.KeyModifier modifiers = Inno.Core.Input.KeyModifier.None)`](../../src/foundation/core/Inno.Core.Events/Events/KeyEvents.cs#L83) | Raised when a key is released.<br>`windowId`：The window id used to initialize this instance.<br>`key`：The key used to initialize this instance.<br>`modifiers`：The modifiers used to initialize this instance. | — | — |
| <a id="api-4d9b32847c5f8d30"></a>[`public class Inno.Core.Events.KeyReleasedEvent : Inno.Core.Events.KeyEvent`](../../src/foundation/core/Inno.Core.Events/Events/KeyEvents.cs#L83) | Raised when a key is released.<br>`windowId`：The window id used to initialize this instance.<br>`key`：The key used to initialize this instance.<br>`modifiers`：The modifiers used to initialize this instance. | — | — |

### `Inno.Core.Events.MouseButtonEvent`

<!-- api: T:Inno.Core.Events.MouseButtonEvent f1db0cd69de39f6aa49065e03231da3b7652ea023ef409acce0d8fced5403947 -->
<!-- api: M:Inno.Core.Events.MouseButtonEvent.#ctor(System.UInt32,Inno.Core.Input.MouseButton) 3218c938c846466a658ad2f8076b531acdef8e1e7d702220a28f697e1147f5b0 -->
<!-- api: P:Inno.Core.Events.MouseButtonEvent.button 2f5acde8a977e0e0176e0d3cde2fd8427bdc786bd2d711c1d63c7e632af6eb62 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8663232c9c743c3c"></a>[`protected Inno.Core.Events.MouseButtonEvent.MouseButtonEvent(uint windowId, Inno.Core.Input.MouseButton button)`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L137) | Base class for mouse button events.<br>`windowId`：The window id used to initialize this instance.<br>`button`：The button used to initialize this instance. | — | — |
| <a id="api-bf79f8c7d26d02bf"></a>[`public Inno.Core.Input.MouseButton Inno.Core.Events.MouseButtonEvent.button { get; }`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L145) | Gets the mouse button for this event. | — | — |
| <a id="api-a0e1cc49b8a3ff07"></a>[`public abstract class Inno.Core.Events.MouseButtonEvent : Inno.Core.Events.MouseEvent`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L137) | Base class for mouse button events.<br>`windowId`：The window id used to initialize this instance.<br>`button`：The button used to initialize this instance. | — | — |

### `Inno.Core.Events.MouseButtonPressedEvent`

<!-- api: T:Inno.Core.Events.MouseButtonPressedEvent fd229e1817aa22f222998fc0ff4f3fffe244174b739d8556ba8f03da5681aab6 -->
<!-- api: M:Inno.Core.Events.MouseButtonPressedEvent.#ctor(System.UInt32,Inno.Core.Input.MouseButton) 05ea38cc796955e050853114d597c2586b3df1654c67edda03f5c77efb4795f3 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d3b42d1cec32d458"></a>[`public Inno.Core.Events.MouseButtonPressedEvent.MouseButtonPressedEvent(uint windowId, Inno.Core.Input.MouseButton button)`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L157) | Raised when a mouse button is pressed.<br>`windowId`：The window id used to initialize this instance.<br>`button`：The button used to initialize this instance. | — | — |
| <a id="api-ff107d47057fc2d2"></a>[`public class Inno.Core.Events.MouseButtonPressedEvent : Inno.Core.Events.MouseButtonEvent`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L157) | Raised when a mouse button is pressed.<br>`windowId`：The window id used to initialize this instance.<br>`button`：The button used to initialize this instance. | — | — |

### `Inno.Core.Events.MouseButtonReleasedEvent`

<!-- api: T:Inno.Core.Events.MouseButtonReleasedEvent 09c311676a16bcd8dba53218b0d9106616ba49b7a0d271aafbec1e9c11a3096d -->
<!-- api: M:Inno.Core.Events.MouseButtonReleasedEvent.#ctor(System.UInt32,Inno.Core.Input.MouseButton) c29fdadd8007c4d144d3740d182306fb0b4e3c438e64a715d692711eddf8778a -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2f0a92d05a89cb93"></a>[`public Inno.Core.Events.MouseButtonReleasedEvent.MouseButtonReleasedEvent(uint windowId, Inno.Core.Input.MouseButton button)`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L173) | Raised when a mouse button is released.<br>`windowId`：The window id used to initialize this instance.<br>`button`：The button used to initialize this instance. | — | — |
| <a id="api-d1464bd0a3902d28"></a>[`public class Inno.Core.Events.MouseButtonReleasedEvent : Inno.Core.Events.MouseButtonEvent`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L173) | Raised when a mouse button is released.<br>`windowId`：The window id used to initialize this instance.<br>`button`：The button used to initialize this instance. | — | — |

### `Inno.Core.Events.MouseEvent`

<!-- api: T:Inno.Core.Events.MouseEvent 2b8964fc8f037fe61a40c996fcefa8e6c75bfc27589ebe2e2e094eb3386feace -->
<!-- api: M:Inno.Core.Events.MouseEvent.#ctor(System.UInt32) a66995fdc69146818a0bc425ee52a11b9a864b72aca5812db54dc81b72170432 -->
<!-- api: M:Inno.Core.Events.MouseEvent.#ctor(Inno.Core.Events.MouseEvent) 098223f0a8b32f6ef892e5d30ae486a37d801b5c492a704bc51cf1c546a7994f -->
<!-- api: P:Inno.Core.Events.MouseEvent.windowId a4a6f107d770f5a614bd31fefa9a9e4eb228a7975e6663ca2e32092da75c0280 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7702d63c3e765417"></a>[`protected Inno.Core.Events.MouseEvent.MouseEvent(Inno.Core.Events.MouseEvent source)`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L27) | Creates a routed mouse event sharing its source's window and global consumption.<br>`source`：The mouse event whose input lifetime remains authoritative. | — | `System.ArgumentNullException`：The source event is null. |
| <a id="api-407705706a657b5f"></a>[`protected Inno.Core.Events.MouseEvent.MouseEvent(uint windowId)`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L16) | Creates an independent mouse event belonging to one platform window.<br>`windowId`：The platform window that produced the event. | — | — |
| <a id="api-30e3370aa3618ae0"></a>[`public uint Inno.Core.Events.MouseEvent.windowId { get; }`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L32) | Gets the source window id for this event. | — | — |
| <a id="api-adb09c1c33cdbc84"></a>[`public abstract class Inno.Core.Events.MouseEvent : Inno.Core.Events.Event`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L8) | Base class for mouse events. | — | — |

### `Inno.Core.Events.MouseMovedEvent`

<!-- api: T:Inno.Core.Events.MouseMovedEvent df2bbeebb7086d3a88937fa01c464cc6466896e19e886829939839afba6dccfb -->
<!-- api: M:Inno.Core.Events.MouseMovedEvent.#ctor(System.UInt32,System.Single,System.Single) 3ec0083ab6017747f22ded4730b8fffc15591461a60514a38e025cac2c22b2b1 -->
<!-- api: P:Inno.Core.Events.MouseMovedEvent.x 7029595740764582f2de02047a9428d5261a7c97b9fc67d05fba570ff03a0c14 -->
<!-- api: P:Inno.Core.Events.MouseMovedEvent.y 0b59f1aa64da2c40cab58a82fb55a2562ae4e67d3123b9a74437343f6f4d9637 -->
<!-- api: M:Inno.Core.Events.MouseMovedEvent.WithPosition(System.Single,System.Single) 7d4075624596db9af331cd658e911d3e280c1ce865e9cd9d07418d60d9506d35 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2f29f5c5dd4c0ecf"></a>[`public Inno.Core.Events.MouseMovedEvent.MouseMovedEvent(uint windowId, float x, float y)`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L53) | Creates a pointer movement in platform-window coordinates.<br>`windowId`：The platform window that produced the movement.<br>`x`：The horizontal pointer coordinate in logical pixels.<br>`y`：The vertical pointer coordinate in logical pixels. | — | — |
| <a id="api-99909a545baee917"></a>[`public Inno.Core.Events.MouseMovedEvent Inno.Core.Events.MouseMovedEvent.WithPosition(float x, float y)`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L84) | Repositions this movement without starting a new global consumption lifetime.<br>返回：A new coordinate representation sharing global consumption with this movement.<br>`x`：The routed horizontal coordinate in logical pixels.<br>`y`：The routed vertical coordinate in logical pixels. | — | — |
| <a id="api-33cf198db5b0b06d"></a>[`public float Inno.Core.Events.MouseMovedEvent.x { get; }`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L65) | Gets cursor X coordinate. | — | — |
| <a id="api-2270cf58bd0d5acc"></a>[`public float Inno.Core.Events.MouseMovedEvent.y { get; }`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L70) | Gets cursor Y coordinate. | — | — |
| <a id="api-adfe26fd12347d51"></a>[`public class Inno.Core.Events.MouseMovedEvent : Inno.Core.Events.MouseEvent`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L39) | Raised when the cursor moves. | — | — |

### `Inno.Core.Events.MouseScrolledEvent`

<!-- api: T:Inno.Core.Events.MouseScrolledEvent 6cac60bd74197855d7e9c5784cffaa5d10e6c745f34387412cb46f70928150f1 -->
<!-- api: M:Inno.Core.Events.MouseScrolledEvent.#ctor(System.UInt32,System.Single,System.Single) 165047fab0d3b1bf82198969c5b0a24c51e288d2659be2dbaf35e7ce6ea14b03 -->
<!-- api: P:Inno.Core.Events.MouseScrolledEvent.offsetX 3568d6f7ffea0afa3dd279737a27a0852607e304db70c3ed2b8cc87e371ac6d0 -->
<!-- api: P:Inno.Core.Events.MouseScrolledEvent.offsetY 66e433480a03372493886b1ce95b18df01c8175c34641c52b781f971ecb3eebf -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-cc342e5e55cb5963"></a>[`public Inno.Core.Events.MouseScrolledEvent.MouseScrolledEvent(uint windowId, float offsetX, float offsetY)`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L111) | Raised when mouse wheel scrolls.<br>`windowId`：The window id used to initialize this instance.<br>`offsetX`：The offset x used to initialize this instance.<br>`offsetY`：The offset y used to initialize this instance. | — | — |
| <a id="api-9232a3e02693b3a2"></a>[`public float Inno.Core.Events.MouseScrolledEvent.offsetX { get; }`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L120) | Gets horizontal scroll offset. | — | — |
| <a id="api-e80a5d2fba274a6d"></a>[`public float Inno.Core.Events.MouseScrolledEvent.offsetY { get; }`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L125) | Gets vertical scroll offset. | — | — |
| <a id="api-91d900183403162a"></a>[`public class Inno.Core.Events.MouseScrolledEvent : Inno.Core.Events.MouseEvent`](../../src/foundation/core/Inno.Core.Events/Events/MouseEvents.cs#L111) | Raised when mouse wheel scrolls.<br>`windowId`：The window id used to initialize this instance.<br>`offsetX`：The offset x used to initialize this instance.<br>`offsetY`：The offset y used to initialize this instance. | — | — |

### `Inno.Core.Events.TextInputEvent`

<!-- api: T:Inno.Core.Events.TextInputEvent c30b50dd296358c0ed3ba1997a7b287405adb3b5598e04bfc69d3cb2ffeab6b1 -->
<!-- api: M:Inno.Core.Events.TextInputEvent.#ctor(System.UInt32,System.String) cf10d4237f3925ba0c0728714a7803e885052fd000b7c4ae5f7b316310ceca23 -->
<!-- api: P:Inno.Core.Events.TextInputEvent.windowId a9d2681ee0c8f553bd7ddea311d3ded0c03a15823d4c3b567971473693b26955 -->
<!-- api: P:Inno.Core.Events.TextInputEvent.text cf280ff3a13bb9efe2900d0d5ff278853b8730ddb96517f2717fcfbe5736db26 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1ac3b7b3b74920e2"></a>[`public Inno.Core.Events.TextInputEvent.TextInputEvent(uint windowId, string text)`](../../src/foundation/core/Inno.Core.Events/Events/KeyEvents.cs#L101) | Raised when the platform commits UTF-8 text through keyboard, IME, or virtual-keyboard input.<br>`windowId`：The source window identifier.<br>`text`：The committed Unicode text. | — | — |
| <a id="api-aabfb1d2aac92627"></a>[`public string Inno.Core.Events.TextInputEvent.text { get; }`](../../src/foundation/core/Inno.Core.Events/Events/KeyEvents.cs#L114) | Gets the committed Unicode text. | — | — |
| <a id="api-22c6e5012eed3fcb"></a>[`public uint Inno.Core.Events.TextInputEvent.windowId { get; }`](../../src/foundation/core/Inno.Core.Events/Events/KeyEvents.cs#L109) | Gets the source window identifier. | — | — |
| <a id="api-2736f51e923170ec"></a>[`public sealed class Inno.Core.Events.TextInputEvent : Inno.Core.Events.Event`](../../src/foundation/core/Inno.Core.Events/Events/KeyEvents.cs#L101) | Raised when the platform commits UTF-8 text through keyboard, IME, or virtual-keyboard input.<br>`windowId`：The source window identifier.<br>`text`：The committed Unicode text. | — | — |

### `Inno.Core.Events.WindowCloseEvent`

<!-- api: T:Inno.Core.Events.WindowCloseEvent 40c321fd8c1bb62dcb6f8e4761262a63679108a014df53d62e0262caa5ddf5b5 -->
<!-- api: M:Inno.Core.Events.WindowCloseEvent.#ctor(System.UInt32) e81807440f2e05be8385d817731652574a75cfcff4a31566c0f56ffdf0243c0f -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d9a0263bd114240c"></a>[`public Inno.Core.Events.WindowCloseEvent.WindowCloseEvent(uint windowId)`](../../src/foundation/core/Inno.Core.Events/Events/WindowEvents.cs#L53) | Raised when the window requests close.<br>`windowId`：The window id used to initialize this instance. | — | — |
| <a id="api-9f1500929a2ebafc"></a>[`public class Inno.Core.Events.WindowCloseEvent : Inno.Core.Events.WindowEvent`](../../src/foundation/core/Inno.Core.Events/Events/WindowEvents.cs#L53) | Raised when the window requests close.<br>`windowId`：The window id used to initialize this instance. | — | — |

### `Inno.Core.Events.WindowEvent`

<!-- api: T:Inno.Core.Events.WindowEvent eaf24aca3087e2fed521e7362b7d3f028a8fd801191ac7601c19f052d444e7e6 -->
<!-- api: M:Inno.Core.Events.WindowEvent.#ctor(System.UInt32) e6e18266729366d82411c31824cd2cc579053bb64bc91139217822f325cabdbe -->
<!-- api: P:Inno.Core.Events.WindowEvent.windowId 10b41dee261302de71dfba1c0100dd61b32af6c7c7221fe2d35576cf221c1f0f -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-301617a3029822f0"></a>[`protected Inno.Core.Events.WindowEvent.WindowEvent(uint windowId)`](../../src/foundation/core/Inno.Core.Events/Events/WindowEvents.cs#L9) | Base class for window lifecycle events.<br>`windowId`：The window id used to initialize this instance. | — | — |
| <a id="api-a401cdfcdb0cd936"></a>[`public uint Inno.Core.Events.WindowEvent.windowId { get; }`](../../src/foundation/core/Inno.Core.Events/Events/WindowEvents.cs#L14) | Gets the source window id for this event. | — | — |
| <a id="api-6ef5f9fae8fbf9d7"></a>[`public abstract class Inno.Core.Events.WindowEvent : Inno.Core.Events.Event`](../../src/foundation/core/Inno.Core.Events/Events/WindowEvents.cs#L9) | Base class for window lifecycle events.<br>`windowId`：The window id used to initialize this instance. | — | — |

### `Inno.Core.Events.WindowFocusChangedEvent`

<!-- api: T:Inno.Core.Events.WindowFocusChangedEvent 78cb171a184ab89cb3988e0b3393c8ca3f0071ddb2d160ac75c5c1625d252924 -->
<!-- api: M:Inno.Core.Events.WindowFocusChangedEvent.#ctor(System.UInt32,System.Boolean) 263a7e664260ebec3fbcf64063418bf1a2c701202224b7f0b74dc43da8ca61e4 -->
<!-- api: P:Inno.Core.Events.WindowFocusChangedEvent.isFocused fee31c3cb6e0fd7fc2e9dcbd6b2c3143ea9ba567d664971901e3cd9cbadf3063 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-6b78d61618b4c672"></a>[`public Inno.Core.Events.WindowFocusChangedEvent.WindowFocusChangedEvent(uint windowId, bool isFocused)`](../../src/foundation/core/Inno.Core.Events/Events/WindowEvents.cs#L66) | Raised when a window gains or loses input focus.<br>`windowId`：The window id used to initialize this instance.<br>`isFocused`：The is focused used to initialize this instance. | — | — |
| <a id="api-6e902ef54ab2e3b8"></a>[`public bool Inno.Core.Events.WindowFocusChangedEvent.isFocused { get; }`](../../src/foundation/core/Inno.Core.Events/Events/WindowEvents.cs#L74) | Gets whether the window has input focus after the change. | — | — |
| <a id="api-80846ba3b33ed42f"></a>[`public class Inno.Core.Events.WindowFocusChangedEvent : Inno.Core.Events.WindowEvent`](../../src/foundation/core/Inno.Core.Events/Events/WindowEvents.cs#L66) | Raised when a window gains or loses input focus.<br>`windowId`：The window id used to initialize this instance.<br>`isFocused`：The is focused used to initialize this instance. | — | — |

### `Inno.Core.Events.WindowResizeEvent`

<!-- api: T:Inno.Core.Events.WindowResizeEvent 8097715848d5a3ebe25c1616cc7ae4c2ede2c578e3fb520dffd4035787c32ab4 -->
<!-- api: M:Inno.Core.Events.WindowResizeEvent.#ctor(System.UInt32,System.Int32,System.Int32) b686cc0d1b15b43c3d7bdef6df23e5212c10bf5629e2f0e92b453b1158b132a2 -->
<!-- api: P:Inno.Core.Events.WindowResizeEvent.width 24c07a8870e865eacf175103e8677393631aec43ebdb43f1502a776a8bf35f2b -->
<!-- api: P:Inno.Core.Events.WindowResizeEvent.height ba5294ecc398c1e8d21d7f6ac13a4542782203056a1f2aa66948bed7a7107131 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-abe19d29747662fc"></a>[`public Inno.Core.Events.WindowResizeEvent.WindowResizeEvent(uint windowId, int width, int height)`](../../src/foundation/core/Inno.Core.Events/Events/WindowEvents.cs#L30) | Raised when the window size changes.<br>`windowId`：The window id used to initialize this instance.<br>`width`：The width in logical units or pixels required by this operation.<br>`height`：The height in logical units or pixels required by this operation. | — | — |
| <a id="api-4db9d46f31059f74"></a>[`public int Inno.Core.Events.WindowResizeEvent.height { get; }`](../../src/foundation/core/Inno.Core.Events/Events/WindowEvents.cs#L44) | Gets the new window height. | — | — |
| <a id="api-f2c18bf6f30cbaf5"></a>[`public int Inno.Core.Events.WindowResizeEvent.width { get; }`](../../src/foundation/core/Inno.Core.Events/Events/WindowEvents.cs#L39) | Gets the new window width. | — | — |
| <a id="api-1cee4d0bebb4c419"></a>[`public class Inno.Core.Events.WindowResizeEvent : Inno.Core.Events.WindowEvent`](../../src/foundation/core/Inno.Core.Events/Events/WindowEvents.cs#L30) | Raised when the window size changes.<br>`windowId`：The window id used to initialize this instance.<br>`width`：The width in logical units or pixels required by this operation.<br>`height`：The height in logical units or pixels required by this operation. | — | — |

### `Inno.Core.Events.WindowVisibilityChangedEvent`

<!-- api: T:Inno.Core.Events.WindowVisibilityChangedEvent 5e2cc2082c88d8ded78e5e1c1b0e54bccf859d9beee610d58a45a4234c9e4434 -->
<!-- api: M:Inno.Core.Events.WindowVisibilityChangedEvent.#ctor(System.UInt32,System.Boolean) 0d5f9905fe049831dcd288431b5447b3754fe9f7018708efeeeee2ff12c42686 -->
<!-- api: P:Inno.Core.Events.WindowVisibilityChangedEvent.isVisible f20dd73fb006390bb9b635c50a1457c4b2453f77026875a79bf03fb204aac21b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-06c41522fdae279c"></a>[`public Inno.Core.Events.WindowVisibilityChangedEvent.WindowVisibilityChangedEvent(uint windowId, bool isVisible)`](../../src/foundation/core/Inno.Core.Events/Events/WindowVisibilityChangedEvent.cs#L12) | Reports whether a platform window can present visible content.<br>`windowId`：The application-local identity of the affected window.<br>`isVisible`：Whether the window is shown and not minimized. | — | — |
| <a id="api-285dacb24e48adde"></a>[`public bool Inno.Core.Events.WindowVisibilityChangedEvent.isVisible { get; }`](../../src/foundation/core/Inno.Core.Events/Events/WindowVisibilityChangedEvent.cs#L20) | Gets whether the window can present visible content after this notification. | — | — |
| <a id="api-14f7c7ecccf57f7c"></a>[`public sealed class Inno.Core.Events.WindowVisibilityChangedEvent : Inno.Core.Events.WindowEvent`](../../src/foundation/core/Inno.Core.Events/Events/WindowVisibilityChangedEvent.cs#L12) | Reports whether a platform window can present visible content.<br>`windowId`：The application-local identity of the affected window.<br>`isVisible`：Whether the window is shown and not minimized. | — | — |

## 项目依赖

- [Inno.Core.Input](Inno.Core.Input.md)：公开引用边界由实际签名核对。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
