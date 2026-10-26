# Inno.Core.Execution

`OwnerThreadExecution.Run` 的完成通知在 owner 回调队列中消费后才结束 pump，并继续排空已入队工作及其派发的回调。内部不会先唤醒空队列、再依赖另一线程的 continuation Task 状态改变，避免任务已完成而 owner 永久等待下一条消息；同步完成、失败、取消和原同步上下文恢复语义保持一致。

## 取消、异步工作与退休屏障

`RunAsync<TResult>(operation, cancellationToken)` 先在锁内校验接收状态并登记占位 Task，再在锁外组合取消并调用 operation 的同步前缀，避免“开始 IO 后才发现已经停止”的窗口，也不持 owner 锁执行调用者代码。返回原操作结果；调用方仍须观察失败。

`RetirementPendingException` 表示依赖仍在使用，**不是已经释放之后的清理异常**。同步 Dispose 保留未完成 owner 和后续依赖，允许 owner-thread 重试；普通释放异常仍聚合并继续。嵌套 LifetimeScope 也保留 pending 子 owner。

该信号可嵌套在 `AggregateException` 或普通 `InnerException` 中，清理链统一调用
`RetirementPendingException.Find(exception)` 分类；返回嵌套 timeout 优先，否则返回 Pending 或 null。
分类结果不替代原异常：owner 必须原样保留和传播整个异常树，不能丢掉上下文和其他失败。
`CollectCompletedFailures(exception, failures)` 收集普通错误分支并按异常实例去重，供可重试 owner 在最终完成后
继续报告以前发生的错误；不能把暂时 Pending 当成一个已完成错误。

LifetimeScope、RetirementBarrier 与 Asset residency 共用此逻辑。包装 Pending 时不释放下层依赖；普通错误在重试
成功后仍报告；包装 timeout 保持终态，重复推进不调用 cleanup。两个异常构造器均可用 `innerException` 保留原始原因。
如果 cancellation callback 或已完成 Task 的异常仍报告 Pending，原 work 已无法通过重新调用来完成退休，
LifetimeScope 会持续保留异常和依赖，要求上层 Fault/重启，不能因 Task.IsCompleted 就放行释放。
直接调用 `Cancel()` 同样保留该终止失败，后续 Cancel/Dispose 不能丢掉它。取消回调仍在另一线程执行时，
`isQuiescent` 为 false，Dispose 返回 Pending 并保留资源；回调正常结束后才可继续。
普通取消错误也在 Cancel 入口保存，随后 Dispose 完成资源释放时仍会汇总报告。并发或重入 Dispose
不能重复调用同一资源：后进入者返回 Pending，由现有 owner 完成这次释放；不持锁执行资源 callback。
`RunAsync` 在调用 operation 的同步前缀之前就登记受拥有的任务，operation 可以重入取消，但不能让
自己的 Dispose 越过仍在执行的工作。链接取消注册释放之后才发布任务完成，不另建领域专属 Task 所有权表。
取消回调在调用线程同步执行，必须及时返回；该机制不声称可以安全抢占一个无限阻塞的托管 callback。

`DisposeAsync` 只适用于线程中立资源：它不负责回到 UI/control thread。RuntimeSubsystem 使用同步 Dispose，在自己线程的安全点完成释放。

[Core 索引](README.md) · [Wiki 首页](../README.md)

## 职责与依赖

只依赖 BCL，提供可撤销的执行绑定和生命周期资源所有权；不引用 Scene、Runtime、Assets 或 backend。

## 公开 API

| 类型/成员 | 稳定语义 |
| --- | --- |
| `ExecutionSlot<TValue>(name)` | 创建独立 AsyncLocal slot；不是全局 service locator |
| `current` / `TryGet(out value)` | 仅访问当前有效绑定；退休或线程不匹配时分别抛异常/返回 false |
| `Enter(value, threadAffine)` | 创建严格 LIFO scope；Dispose 清除共享节点中的强引用，使异步后代也失去访问 |
| `Suspend()` | 临时屏蔽当前值，不回退到父值 |
| `LifetimeScope(maxTrackedWork = 4096)` | 正容量的异步工作 owner；容量耗尽在执行 operation 前拒绝 |
| `trackedWorkCount` / `peakTrackedWorkCount` / `rejectedWorkCount` | 当前保留、峰值和接纳拒绝计数；故障任务保留到退休报告 |
| `LifetimeScope.cancellationToken` | 当前生命周期的取消信号 |
| `Own<T>(resource)` | 转移 IDisposable 所有权，返回原资源用于显式依赖组合 |
| `Track(task)` / `isQuiescent` | 跟踪必须完成的工作；不偷偷放弃仍运行任务 |
| `Cancel()` | 禁止新注册并在调用线程执行取消回调；回调执行期间保留依赖，包装 Pending 持续封锁释放 |
| `RunAsync<TResult>(operation, cancellationToken)` | 原子登记异步工作，合并 owner/caller cancellation，停止后拒绝 |
| `RetirementPendingException` | 仍有工作或子 owner 尚未退休；当前及更低依赖保持存活，必须重试 |
| `RetirementPendingException.Find(exception)` | 查询完整异常树；timeout 优先，缺失返回 null，不改变原错误 |
| `RetirementPendingException.CollectCompletedFailures(exception, failures)` | 可重试 owner 保存普通失败分支；按实例去重，不把 Pending 计作完成错误 |
| `RetirementBarrier(owner, timeout)` | 不拥有资源、不保存 callback；在创建线程推进一个有截止时间的退休操作，默认 30 秒 |
| `RetirementBarrier.TryComplete(retire)` / `Wait(retire)` | 前者每个安全点尝试一次并以 false 表示 Pending；后者用于启动失败/最终退出的有界排空 |
| `RetirementTimeoutException` | 派生自 Pending，表示依赖仍然不能释放，但 deadline 已终态失败；上层必须 Fault，不能继续启动或重载 |
| `Dispose()` | 仅在工作完成后逆序释放；普通故障聚合；遇到 Pending 停止释放依赖 |
| `DisposeAsync()` | 异步取消并等待工作完成，再释放资源 |

没有 protected 扩展点。`LifetimeScope` 是 ownership component，不是可继承的全局基类。

## 工作流

```csharp
using System;
using Inno.Core.Execution;

var slot = new ExecutionSlot<string>("request");
using (slot.Enter("current request"))
{
    Console.WriteLine(slot.current);
}
```

 façade 在自己的项目中拥有固定 contract 的 slot；服务本身由 Host/Session 显式注入。scope 不负责 Dispose 服务；服务由更长的 lifetime owner 释放。任何已经自行取出并保存的裸 service 引用仍由调用者负责，不存在强制让任意托管引用失效的机制。

退休工作必须响应取消；同步排空委托必须包含所需的主线程完成队列推进，不能等待一个没人推进的 owner continuation。
同一个 `RetirementBarrier` 不会因为再次调用重置 deadline，超时后也不会调用传入的新 cleanup。
它不替代 GC unload barrier：前者确保对象仍在运行的工作已经结束，后者证明 collectible ALC 的所有根都消失。

`LifetimeScope` 在“某个资源释放失败 → 后续资源 Pending → 重试”期间保留此前失败；最终聚合报告，已完成的资源不会重复释放。

RunAsync 自有 Task 成功/取消时在完成路径内直接从 owner 移除，不等下次 Track/Dispose，也不另外排队清理 continuation；否则多余回调本身会短暂保留 completed Task/TResult。Track 外部任务只登记一次完成观察，回调不捕获 ExecutionContext，避免复制 AsyncLocal 代际根。失败 Task 仍保留至退休报告，调用者也必须观察失败。直接 Track 已经启动的任务被拒绝时，任务仍归调用者；优先用 RunAsync 在启动前接纳。

## 验证

`Inno.Core.Execution.Tests` 覆盖 LIFO、异步后代撤销、隔离、屏蔽、线程限制、取消和异常逆序释放。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Core.Execution.ExecutionSlot`1`

<!-- api: T:Inno.Core.Execution.ExecutionSlot`1 a27c1b941d710605d737071d4e59a22b9350fd4bdb7e8114b89f10a6d0e38163 -->
<!-- api: M:Inno.Core.Execution.ExecutionSlot`1.#ctor(System.String) dbedf2f80848ff6946528429b79958b5ca7e8df195c92c628a14442fe9504add -->
<!-- api: P:Inno.Core.Execution.ExecutionSlot`1.current 23bfedd3617e32b28d6112259bad427979af606c6514f2e3f7482822bd4dfbc8 -->
<!-- api: M:Inno.Core.Execution.ExecutionSlot`1.Enter(`0,System.Boolean) b916c0638db43d7303327be57393c6c9c00650d2fe695bc1f742dad6bcbfc447 -->
<!-- api: M:Inno.Core.Execution.ExecutionSlot`1.Suspend 6f1978c91133b66c04ed52f60cce0492a66c6dbce21c93a6e7c61a0bc5c9a17a -->
<!-- api: M:Inno.Core.Execution.ExecutionSlot`1.TryGet(`0@) ed4ee14b821b36485a63296f3912a5c6cc661947ea2058326caf866e2ca18fd9 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-cf0d13ed845c3ce9"></a>[`public Inno.Core.Execution.ExecutionSlot<TValue>.ExecutionSlot(string name)`](../../src/foundation/core/Inno.Core.Execution/ExecutionSlot.cs#L27) | Creates a slot without binding a process-global service.<br>`name`：The diagnostic name used when access is invalid. | — | `System.ArgumentException`：The name is empty. |
| <a id="api-1f74885f7c3ad6f1"></a>[`public System.IDisposable Inno.Core.Execution.ExecutionSlot<TValue>.Enter(TValue value, bool threadAffine = false)`](../../src/foundation/core/Inno.Core.Execution/ExecutionSlot.cs#L58) | Binds a value until the returned scope is retired in strict last-in-first-out order.<br>返回：A scope that revokes inherited bindings and clears their strong references on retirement.<br>`value`：The service or value owned by the enclosing lifetime.<br>`threadAffine`：Whether access and retirement require the entering thread. | — | `System.ArgumentNullException`：The value is null. |
| <a id="api-474923f5273e06b1"></a>[`public System.IDisposable Inno.Core.Execution.ExecutionSlot<TValue>.Suspend()`](../../src/foundation/core/Inno.Core.Execution/ExecutionSlot.cs#L74) | Temporarily masks this slot without exposing an enclosing binding.<br>返回：A strict scope that restores the enclosing binding when retired. | — | — |
| <a id="api-4454e5b751fd2305"></a>[`public bool Inno.Core.Execution.ExecutionSlot<TValue>.TryGet(out TValue value)`](../../src/foundation/core/Inno.Core.Execution/ExecutionSlot.cs#L90) | Tries to read the current binding without falling through an expired child scope.<br>返回：Whether a live binding is available on this thread.<br>`value`：Receives the live value, or the default value when access is unavailable. | — | — |
| <a id="api-45176de172a01fc0"></a>[`public TValue Inno.Core.Execution.ExecutionSlot<TValue>.current { get; }`](../../src/foundation/core/Inno.Core.Execution/ExecutionSlot.cs#L39) | Gets the current live value, rejecting expired or wrong-thread bindings. | — | `System.InvalidOperationException`：No live binding exists on the permitted thread. |
| <a id="api-15e1daf6ef15ff68"></a>[`public sealed class Inno.Core.Execution.ExecutionSlot<TValue>`](../../src/foundation/core/Inno.Core.Execution/ExecutionSlot.cs#L13) | Provides an isolated, revocable execution binding whose values never survive scope retirement. | — | — |

### `Inno.Core.Execution.LifetimeScope`

<!-- api: T:Inno.Core.Execution.LifetimeScope 4b8ade4123b82823d0d6f70a9d3ababde337f5e8461846b951fb6135d7e07161 -->
<!-- api: M:Inno.Core.Execution.LifetimeScope.#ctor(System.Int32) 2b1d9a4a43a2131b477b72d00100b292389a9901a596147337e992083cf4be38 -->
<!-- api: P:Inno.Core.Execution.LifetimeScope.trackedWorkCount 2e2058240440acb4e5118623da298ba67716a9377f2b2d6120d24a7d384486d0 -->
<!-- api: P:Inno.Core.Execution.LifetimeScope.peakTrackedWorkCount 0c3c59a789adf09f8d4c638d4a9b4d62f7ae2f9d5857316090267774e8bcc0ab -->
<!-- api: P:Inno.Core.Execution.LifetimeScope.rejectedWorkCount ded7aa82d5dd3a6636728ecd2c361b293135d4fc78964625e586af79cc03ad3c -->
<!-- api: P:Inno.Core.Execution.LifetimeScope.cancellationToken d00738fb32a1e9c8a44043d08ca5410ae13e1d8161d024ab639b22857d54c416 -->
<!-- api: P:Inno.Core.Execution.LifetimeScope.isQuiescent d9a1ebcccc21dd9de79d720bcaf1d9f953a7c8895d76567ea89c3a81930ca830 -->
<!-- api: M:Inno.Core.Execution.LifetimeScope.Own``1(``0) 46bdfcdb263136fef59dd8f4f894ec5158c1ca3bb51379fb39d9fa7d6a83a223 -->
<!-- api: M:Inno.Core.Execution.LifetimeScope.Track(System.Threading.Tasks.Task) e562ab0badf95355313965c23500dc63bbc29f01535ff74668036422a10e9627 -->
<!-- api: M:Inno.Core.Execution.LifetimeScope.RunAsync``1(System.Func{System.Threading.CancellationToken,System.Threading.Tasks.ValueTask{``0}},System.Threading.CancellationToken) 99da0b9104c149ac4ec52a6a89f4fd1e35285eae0303a2e89a7caed18fe17329 -->
<!-- api: M:Inno.Core.Execution.LifetimeScope.Cancel 1fb5329ef93ddb572728c8f209ecac572283eb0232bc619b0f1a38d2586fdcd3 -->
<!-- api: M:Inno.Core.Execution.LifetimeScope.Dispose ef523f7dbce77cf1823de2bd53215b53378cfcc00a74118ad2546c58150ef3a1 -->
<!-- api: M:Inno.Core.Execution.LifetimeScope.DisposeAsync 93b7110e0809d52750561e00bcee8b74a5eee91f84a66fede3954d84f493dabb -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-eaca8524f48eedb4"></a>[`public Inno.Core.Execution.LifetimeScope.LifetimeScope(int maxTrackedWork = 4096)`](../../src/foundation/core/Inno.Core.Execution/LifetimeScope.cs#L37) | Creates a lifetime with finite asynchronous admission capacity.<br>`maxTrackedWork`：Maximum retained work records; completed successful work releases capacity immediately. | — | `System.ArgumentOutOfRangeException`：The capacity is not positive. |
| <a id="api-d775bc923fc4df9a"></a>[`public void Inno.Core.Execution.LifetimeScope.Cancel()`](../../src/foundation/core/Inno.Core.Execution/LifetimeScope.cs#L177) | Stops new registrations and invokes cancellation callbacks on the calling thread. | Callbacks must return promptly. Dependent resources remain owned while cancellation callbacks execute, including when another thread attempts disposal. A callback reporting pending work permanently blocks release. | `System.AggregateException`：One or more cancellation callbacks fail. |
| <a id="api-3b81ef2d6cbca02b"></a>[`public void Inno.Core.Execution.LifetimeScope.Dispose()`](../../src/foundation/core/Inno.Core.Execution/LifetimeScope.cs#L223) | Releases a quiescent lifetime, attempting every resource even when release fails. | Concurrent or reentrant retirement reports pending work without invoking a resource twice. | `System.InvalidOperationException`：Tracked work remains active; drain it before retrying.<br>`System.AggregateException`：Cancellation, work or resource retirement fails. |
| <a id="api-31c436fa7828060f"></a>[`public System.Threading.Tasks.ValueTask Inno.Core.Execution.LifetimeScope.DisposeAsync()`](../../src/foundation/core/Inno.Core.Execution/LifetimeScope.cs#L268) | Cancels and asynchronously drains tracked work before reverse-order release.<br>返回：A task that completes only after work and resource retirement finish. | Only use asynchronous disposal for thread-neutral resources. Control-thread owners must await their work and call synchronous disposal at their own safe point; this method does not marshal to an owner thread. | `System.AggregateException`：Cancellation, work or resource retirement fails. |
| <a id="api-8d3db4fc7c320527"></a>[`public TResource Inno.Core.Execution.LifetimeScope.Own<TResource>(TResource resource) where TResource : System.IDisposable`](../../src/foundation/core/Inno.Core.Execution/LifetimeScope.cs#L113) | Transfers disposal ownership of a resource to this lifetime.<br>返回：The same resource for explicit dependency wiring.<br>`resource`：The resource to release in reverse registration order. | — | `System.ArgumentNullException`：The resource is null.<br>`System.InvalidOperationException`：Retirement has already started. |
| <a id="api-10c04802468bfb60"></a>[`public System.Threading.Tasks.Task<TResult> Inno.Core.Execution.LifetimeScope.RunAsync<TResult>(System.Func<System.Threading.CancellationToken, System.Threading.Tasks.ValueTask<TResult>> operation, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/foundation/core/Inno.Core.Execution/LifetimeScope.cs#L156) | Atomically admits and tracks asynchronous work under this owner's cancellation boundary.<br>返回：The tracked result, cancellation or operation failure.<br>`operation`：The operation, which must observe the supplied token and must not require owner-thread completion.<br>`cancellationToken`：Additional cancellation requested by the caller. | — | `System.InvalidOperationException`：Retirement has begun or work capacity is exhausted; the operation is not invoked. |
| <a id="api-e19a98aab2f3f0be"></a>[`public void Inno.Core.Execution.LifetimeScope.Track(System.Threading.Tasks.Task task)`](../../src/foundation/core/Inno.Core.Execution/LifetimeScope.cs#L136) | Tracks work that must complete before this lifetime's resources can be released.<br>`task`：Work that observes this lifetime's cancellation token. | — | `System.ArgumentNullException`：The task is null.<br>`System.InvalidOperationException`：Retirement has started or work capacity is exhausted; ownership remains with the caller. |
| <a id="api-e5ce378276960228"></a>[`public System.Threading.CancellationToken Inno.Core.Execution.LifetimeScope.cancellationToken { get; }`](../../src/foundation/core/Inno.Core.Execution/LifetimeScope.cs#L80) | Gets cancellation shared by work owned by this lifetime. | — | — |
| <a id="api-8db5ba4a2211bc85"></a>[`public bool Inno.Core.Execution.LifetimeScope.isQuiescent { get; }`](../../src/foundation/core/Inno.Core.Execution/LifetimeScope.cs#L85) | Gets whether all tracked work has completed and resources can retire synchronously. | — | — |
| <a id="api-94ed7eb0caf5b427"></a>[`public int Inno.Core.Execution.LifetimeScope.peakTrackedWorkCount { get; }`](../../src/foundation/core/Inno.Core.Execution/LifetimeScope.cs#L57) | Gets the largest simultaneous retained work count. | — | — |
| <a id="api-173cea11f9bdc644"></a>[`public long Inno.Core.Execution.LifetimeScope.rejectedWorkCount { get; }`](../../src/foundation/core/Inno.Core.Execution/LifetimeScope.cs#L68) | Gets work registrations rejected before ownership transfer by finite capacity. | — | — |
| <a id="api-5849620ced2a4d62"></a>[`public int Inno.Core.Execution.LifetimeScope.trackedWorkCount { get; }`](../../src/foundation/core/Inno.Core.Execution/LifetimeScope.cs#L46) | Gets retained work records, including faults awaiting final retirement reporting. | — | — |
| <a id="api-96ec7efa1e86c67f"></a>[`public sealed class Inno.Core.Execution.LifetimeScope : System.IDisposable, System.IAsyncDisposable`](../../src/foundation/core/Inno.Core.Execution/LifetimeScope.cs#L12) | Owns cancellation, tracked work and reverse-order resource retirement for one lifecycle. | — | — |

### `Inno.Core.Execution.OwnerThreadExecution`

<!-- api: T:Inno.Core.Execution.OwnerThreadExecution 4c81dad5f8e475b9ab6d958c1724b4d2ad8b3bc50a26116e9452f8adbcff2082 -->
<!-- api: M:Inno.Core.Execution.OwnerThreadExecution.Run``1(System.Func{System.Threading.Tasks.Task{``0}}) a1fa27ab96dbe19c0b063a7a20c44ea9671dfbd64339235d2f8d5ba356634c5b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-bd7c3d37d004be09"></a>[`public static TResult Inno.Core.Execution.OwnerThreadExecution.Run<TResult>(System.Func<System.Threading.Tasks.Task<TResult>> operation)`](../../src/foundation/core/Inno.Core.Execution/OwnerThreadExecution.cs#L37) | Pumps continuations until the owned operation completes, then restores the previous context.<br>返回：The completed operation result. Operation failures and cancellation propagate unchanged.<br>`operation`：Work that owns and retires every task it starts before returning. Async void work is not supported. | This is a blocking host boundary. Callback-driven hosts use their platform synchronization context instead. Work that explicitly suppresses context capture is responsible for returning to the owner before accessing thread-affine resources. | `System.ArgumentNullException`：The operation is null.<br>`System.InvalidOperationException`：The operation returned a null task. |
| <a id="api-99520b876abfaf1b"></a>[`public static class Inno.Core.Execution.OwnerThreadExecution`](../../src/foundation/core/Inno.Core.Execution/OwnerThreadExecution.cs#L12) | Runs asynchronous application work while returning its continuations to the calling thread. | — | — |

### `Inno.Core.Execution.RetirementBarrier`

<!-- api: T:Inno.Core.Execution.RetirementBarrier 0ef1d318563bfcd1a925f0c2d067b3387b274eed29c66ec826f03ad639e5bb49 -->
<!-- api: M:Inno.Core.Execution.RetirementBarrier.#ctor(System.String,System.Nullable{System.TimeSpan}) b88c89e4c9ac80c052f8863385c1faa5749374072980f5065e64a95c01c70628 -->
<!-- api: M:Inno.Core.Execution.RetirementBarrier.TryComplete(System.Action) 371f1c7a7b62ca3d01c30d61f8340d85e62a7149af832a418e4f8997d34632cf -->
<!-- api: M:Inno.Core.Execution.RetirementBarrier.Wait(System.Action) 23709e74055c913ab483e64ae4dfe9bdf67cee2f098526883b6f6eec7bae039d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-30ce5bfcdaceebc6"></a>[`public Inno.Core.Execution.RetirementBarrier.RetirementBarrier(string owner, System.TimeSpan? timeout = null)`](../../src/foundation/core/Inno.Core.Execution/RetirementBarrier.cs#L33) | Creates a deadline that starts with the first retirement attempt.<br>`owner`：A diagnostic name identifying the retained resource owner.<br>`timeout`：The positive maximum drain duration; omitted values use thirty seconds. | — | `System.ArgumentException`：The owner name is empty or the timeout is not positive. |
| <a id="api-3b1b5872ae87a5d1"></a>[`public bool Inno.Core.Execution.RetirementBarrier.TryComplete(System.Action retire)`](../../src/foundation/core/Inno.Core.Execution/RetirementBarrier.cs#L66) | Attempts retirement once at an owner-thread safe point.<br>返回：True after retirement completes, or false while work remains within its deadline.<br>`retire`：The owner's retryable retirement operation; the barrier never retains this delegate. | Wrapped pending failures retain their ownership semantics. A nested timeout is rethrown in its original exception tree. Ordinary sibling failures survive retries and are reported once retirement completes. | `System.AggregateException`：Retirement completed after earlier ordinary failures, or the owner reported a wrapped terminal timeout.<br>`Inno.Core.Execution.RetirementTimeoutException`：Work exceeded its deadline. The barrier remains terminally faulted and dependencies must be retained.<br>`System.InvalidOperationException`：The caller is not the thread that created this barrier. |
| <a id="api-abecacea82410063"></a>[`public void Inno.Core.Execution.RetirementBarrier.Wait(System.Action retire)`](../../src/foundation/core/Inno.Core.Execution/RetirementBarrier.cs#L120) | Drains a failed startup or final shutdown on the owner thread until completion or explicit failure.<br>`retire`：The retryable operation, including any necessary owner-thread queue draining. | — | `Inno.Core.Execution.RetirementTimeoutException`：Work exceeded its deadline; callers must fault admission and retain pending owners. |
| <a id="api-20bb54901ece4074"></a>[`public sealed class Inno.Core.Execution.RetirementBarrier`](../../src/foundation/core/Inno.Core.Execution/RetirementBarrier.cs#L11) | Bounds owner-thread retirement attempts without taking resource ownership or releasing pending dependencies. | — | — |

### `Inno.Core.Execution.RetirementPendingException`

<!-- api: T:Inno.Core.Execution.RetirementPendingException d8f8bbef8568b2918ac661c8c8adc37cd21ffdaddd14aafe4c938d7fab6aeb06 -->
<!-- api: M:Inno.Core.Execution.RetirementPendingException.#ctor(System.String,System.Exception) b8945dbc63c41f163521dd37438749a9b1541226c9508561fab068fe58d8921b -->
<!-- api: M:Inno.Core.Execution.RetirementPendingException.Find(System.Exception) 12ec8febe8d7cbfa36c9820a2ea88f27f290b648277827c0e9e672e488adde51 -->
<!-- api: M:Inno.Core.Execution.RetirementPendingException.CollectCompletedFailures(System.Exception,System.Collections.Generic.ICollection{System.Exception}) 5a2adc805b34f92785fff52f0be5f53f35b0c4fed5b4567fe493c93f86b767af -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3bcb75c6ae3ddb32"></a>[`public Inno.Core.Execution.RetirementPendingException.RetirementPendingException(string message, System.Exception? innerException = null)`](../../src/foundation/core/Inno.Core.Execution/RetirementPendingException.cs#L20) | Creates a retryable retirement barrier for an owner with unfinished work.<br>`message`：The owner and work preventing synchronous release.<br>`innerException`：The original retirement failure, including any contextual wrappers, or null when there is no cause. | — | — |
| <a id="api-825d0444888d4d0d"></a>[`public static void Inno.Core.Execution.RetirementPendingException.CollectCompletedFailures(System.Exception exception, System.Collections.Generic.ICollection<System.Exception> failures)`](../../src/foundation/core/Inno.Core.Execution/RetirementPendingException.cs#L69) | Retains ordinary failure branches across pending retries without recording transient pending signals as completed work.<br>`exception`：The original exception tree reported by a retirement attempt.<br>`failures`：The owner's failure collection; existing exception instances are not added twice. | — | `System.ArgumentNullException`：The exception or collection is null. |
| <a id="api-3ecd427a3209f82d"></a>[`public static Inno.Core.Execution.RetirementPendingException? Inno.Core.Execution.RetirementPendingException.Find(System.Exception exception)`](../../src/foundation/core/Inno.Core.Execution/RetirementPendingException.cs#L38) | Finds unfinished retirement in an exception tree without discarding its contextual or sibling failures.<br>返回：A nested timeout when present, otherwise the first pending signal, or null when retirement is not pending. Callers must propagate and retain the original exception rather than replacing it with this classification.<br>`exception`：The exception to classify, including aggregate and ordinary inner-exception wrappers. | — | `System.ArgumentNullException`：The exception is null. |
| <a id="api-4aa08895e340a7e1"></a>[`public class Inno.Core.Execution.RetirementPendingException : System.InvalidOperationException`](../../src/foundation/core/Inno.Core.Execution/RetirementPendingException.cs#L9) | Reports that cancellation has begun but the owner must retain its dependencies until work drains. | — | — |

### `Inno.Core.Execution.RetirementTimeoutException`

<!-- api: T:Inno.Core.Execution.RetirementTimeoutException 4cf7021e8beba7e961e45f6eeffac13cafe1034625b13463f1849a339e706c30 -->
<!-- api: M:Inno.Core.Execution.RetirementTimeoutException.#ctor(System.String,System.Exception) 7744bd1a237ba6bc75c5273e5798d0b1dd76427fe0dfdc0030422550c40b9d6a -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-88b0277c041bf6a5"></a>[`public Inno.Core.Execution.RetirementTimeoutException.RetirementTimeoutException(string message, System.Exception? innerException = null)`](../../src/foundation/core/Inno.Core.Execution/RetirementTimeoutException.cs#L19) | Creates a terminal retirement failure whose owner must remain retained until host shutdown.<br>`message`：The owner and unfinished work that exceeded the deadline.<br>`innerException`：The complete failure that prevented retirement, or null when no underlying exception is available. | — | — |
| <a id="api-f0cc9e9e1c1c07d0"></a>[`public sealed class Inno.Core.Execution.RetirementTimeoutException : Inno.Core.Execution.RetirementPendingException`](../../src/foundation/core/Inno.Core.Execution/RetirementTimeoutException.cs#L8) | Reports an expired retirement deadline without granting permission to release dependent resources. | — | — |

## 项目依赖

- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
