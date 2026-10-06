# Inno.Core.Execution

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

[Core 索引](README.md) · [Wiki 首页](../README.md) · [收口实施](../architecture/ENGINE_CONSOLIDATION_IMPLEMENTATION.md)

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

以下仅列出当前程序集自己声明的 public/protected 契约；继承成员遵循所属基类页面。internal/private 实现不作为稳定公开 API。签名依据当前源码语义模型生成，行为、参数、异常与所有权说明同时以对应英文 XML 为准。

### `Inno.Core.Execution.ExecutionSlot<TValue>`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Execution.ExecutionSlot<TValue>.ExecutionSlot(string name)`](../../src/foundation/core/Inno.Core.Execution/ExecutionSlot.cs#L27) | Creates a slot without binding a process-global service. |
| [`System.IDisposable Inno.Core.Execution.ExecutionSlot<TValue>.Enter(TValue value, bool threadAffine = false)`](../../src/foundation/core/Inno.Core.Execution/ExecutionSlot.cs#L58) | Binds a value until the returned scope is retired in strict last-in-first-out order. |
| [`System.IDisposable Inno.Core.Execution.ExecutionSlot<TValue>.Suspend()`](../../src/foundation/core/Inno.Core.Execution/ExecutionSlot.cs#L74) | Temporarily masks this slot without exposing an enclosing binding. |
| [`bool Inno.Core.Execution.ExecutionSlot<TValue>.TryGet(out TValue value)`](../../src/foundation/core/Inno.Core.Execution/ExecutionSlot.cs#L90) | Tries to read the current binding without falling through an expired child scope. |
| [`TValue Inno.Core.Execution.ExecutionSlot<TValue>.current`](../../src/foundation/core/Inno.Core.Execution/ExecutionSlot.cs#L39) | Gets the current live value, rejecting expired or wrong-thread bindings. |
| [`Inno.Core.Execution.ExecutionSlot<TValue>`](../../src/foundation/core/Inno.Core.Execution/ExecutionSlot.cs#L13) | Provides an isolated, revocable execution binding whose values never survive scope retirement. |

### `Inno.Core.Execution.LifetimeScope`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Execution.LifetimeScope.LifetimeScope(int maxTrackedWork = 4096)`](../../src/foundation/core/Inno.Core.Execution/LifetimeScope.cs#L37) | Creates a lifetime with finite asynchronous admission capacity. |
| [`void Inno.Core.Execution.LifetimeScope.Cancel()`](../../src/foundation/core/Inno.Core.Execution/LifetimeScope.cs#L177) | Stops new registrations and invokes cancellation callbacks on the calling thread. |
| [`void Inno.Core.Execution.LifetimeScope.Dispose()`](../../src/foundation/core/Inno.Core.Execution/LifetimeScope.cs#L223) | Releases a quiescent lifetime, attempting every resource even when release fails. |
| [`System.Threading.Tasks.ValueTask Inno.Core.Execution.LifetimeScope.DisposeAsync()`](../../src/foundation/core/Inno.Core.Execution/LifetimeScope.cs#L268) | Cancels and asynchronously drains tracked work before reverse-order release. |
| [`TResource Inno.Core.Execution.LifetimeScope.Own<TResource>(TResource resource)`](../../src/foundation/core/Inno.Core.Execution/LifetimeScope.cs#L113) | Transfers disposal ownership of a resource to this lifetime. |
| [`System.Threading.Tasks.Task<TResult> Inno.Core.Execution.LifetimeScope.RunAsync<TResult>(System.Func<System.Threading.CancellationToken, System.Threading.Tasks.ValueTask<TResult>> operation, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/foundation/core/Inno.Core.Execution/LifetimeScope.cs#L156) | Atomically admits and tracks asynchronous work under this owner's cancellation boundary. |
| [`void Inno.Core.Execution.LifetimeScope.Track(System.Threading.Tasks.Task task)`](../../src/foundation/core/Inno.Core.Execution/LifetimeScope.cs#L136) | Tracks work that must complete before this lifetime's resources can be released. |
| [`System.Threading.CancellationToken Inno.Core.Execution.LifetimeScope.cancellationToken`](../../src/foundation/core/Inno.Core.Execution/LifetimeScope.cs#L80) | Gets cancellation shared by work owned by this lifetime. |
| [`bool Inno.Core.Execution.LifetimeScope.isQuiescent`](../../src/foundation/core/Inno.Core.Execution/LifetimeScope.cs#L85) | Gets whether all tracked work has completed and resources can retire synchronously. |
| [`int Inno.Core.Execution.LifetimeScope.peakTrackedWorkCount`](../../src/foundation/core/Inno.Core.Execution/LifetimeScope.cs#L57) | Gets the largest simultaneous retained work count. |
| [`long Inno.Core.Execution.LifetimeScope.rejectedWorkCount`](../../src/foundation/core/Inno.Core.Execution/LifetimeScope.cs#L68) | Gets work registrations rejected before ownership transfer by finite capacity. |
| [`int Inno.Core.Execution.LifetimeScope.trackedWorkCount`](../../src/foundation/core/Inno.Core.Execution/LifetimeScope.cs#L46) | Gets retained work records, including faults awaiting final retirement reporting. |
| [`Inno.Core.Execution.LifetimeScope`](../../src/foundation/core/Inno.Core.Execution/LifetimeScope.cs#L12) | Owns cancellation, tracked work and reverse-order resource retirement for one lifecycle. |

### `Inno.Core.Execution.OwnerThreadExecution`

| 当前声明 | 行为 |
| --- | --- |
| [`static TResult Inno.Core.Execution.OwnerThreadExecution.Run<TResult>(System.Func<System.Threading.Tasks.Task<TResult>> operation)`](../../src/foundation/core/Inno.Core.Execution/OwnerThreadExecution.cs#L37) | Pumps continuations until the owned operation completes, then restores the previous context. |
| [`Inno.Core.Execution.OwnerThreadExecution`](../../src/foundation/core/Inno.Core.Execution/OwnerThreadExecution.cs#L12) | Runs asynchronous application work while returning its continuations to the calling thread. |

### `Inno.Core.Execution.RetirementBarrier`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Execution.RetirementBarrier.RetirementBarrier(string owner, System.TimeSpan? timeout = null)`](../../src/foundation/core/Inno.Core.Execution/RetirementBarrier.cs#L33) | Creates a deadline that starts with the first retirement attempt. |
| [`bool Inno.Core.Execution.RetirementBarrier.TryComplete(System.Action retire)`](../../src/foundation/core/Inno.Core.Execution/RetirementBarrier.cs#L66) | Attempts retirement once at an owner-thread safe point. |
| [`void Inno.Core.Execution.RetirementBarrier.Wait(System.Action retire)`](../../src/foundation/core/Inno.Core.Execution/RetirementBarrier.cs#L120) | Drains a failed startup or final shutdown on the owner thread until completion or explicit failure. |
| [`Inno.Core.Execution.RetirementBarrier`](../../src/foundation/core/Inno.Core.Execution/RetirementBarrier.cs#L11) | Bounds owner-thread retirement attempts without taking resource ownership or releasing pending dependencies. |

### `Inno.Core.Execution.RetirementPendingException`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Execution.RetirementPendingException.RetirementPendingException(string message, System.Exception? innerException = null)`](../../src/foundation/core/Inno.Core.Execution/RetirementPendingException.cs#L20) | Creates a retryable retirement barrier for an owner with unfinished work. |
| [`static void Inno.Core.Execution.RetirementPendingException.CollectCompletedFailures(System.Exception exception, System.Collections.Generic.ICollection<System.Exception> failures)`](../../src/foundation/core/Inno.Core.Execution/RetirementPendingException.cs#L69) | Retains ordinary failure branches across pending retries without recording transient pending signals as completed work. |
| [`static Inno.Core.Execution.RetirementPendingException? Inno.Core.Execution.RetirementPendingException.Find(System.Exception exception)`](../../src/foundation/core/Inno.Core.Execution/RetirementPendingException.cs#L38) | Finds unfinished retirement in an exception tree without discarding its contextual or sibling failures. |
| [`Inno.Core.Execution.RetirementPendingException`](../../src/foundation/core/Inno.Core.Execution/RetirementPendingException.cs#L9) | Reports that cancellation has begun but the owner must retain its dependencies until work drains. |

### `Inno.Core.Execution.RetirementTimeoutException`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Execution.RetirementTimeoutException.RetirementTimeoutException(string message, System.Exception? innerException = null)`](../../src/foundation/core/Inno.Core.Execution/RetirementTimeoutException.cs#L19) | Creates a terminal retirement failure whose owner must remain retained until host shutdown. |
| [`Inno.Core.Execution.RetirementTimeoutException`](../../src/foundation/core/Inno.Core.Execution/RetirementTimeoutException.cs#L8) | Reports an expired retirement deadline without granting permission to release dependent resources. |

## 项目依赖

- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：项目引用；公开签名可见性由语义边界检查确认。

共同 MSBuild 注入的 analyzer 与编译规则属于构建依赖，完整有效项目图记录在本轮验收证据中。
