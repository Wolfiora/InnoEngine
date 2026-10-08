# Inno.Extensibility.Reload

[Extensibility 索引](README.md) · [Wiki 首页](../README.md) · [Identity、可恢复引用与热重载标准](../architecture/IDENTITY_REFERENCE_RELOAD_STANDARD.md)

当前新增 `GenerationCoordinator.TryAcquireChange(operation, out reservation)`：后台读租约仍存活时返回 false，
供自动 Catalog 刷新延后，不绕过读保护。持有发布 reservation 时不接受新的 Build/Export 读者。
`Fault(exception)` 是 owner 发现不可恢复清理错误时关闭整个 Host admission 的入口；没有 Reset 或继续运行分支。
`EnsureRetirementSafe()` 是依赖退出的统一前置校验：普通终结错误仍允许继续清理；曾报告未退休工作后，
即使更早已有另一条普通错误，也始终抛出未退休信号。Registry、TypeCatalog、ModuleHost、RuntimeSession、EngineHost 共用此判断。

## 职责与边界

`Inno.Extensibility.Reload` 定义所有 collectible generation 共用的 GC unload barrier。它不加载程序集、
不扫描类型，也不决定哪个 candidate 应当激活；这些职责仍分别属于 Module、Scripting 与 Plugin owner。

- `IAssemblyUnloadProbe` 只通过弱状态观察一个退休 generation。
- `AssemblyUnloadBarrier` 执行 Full GC、等待 finalizer、再次 Full GC，并在所有 probe 完成前保持
  `AwaitingCollection`。
- `AssemblyUnloadException` 是终止性 retention failure，记录仍存活的 generation、等待时长与 GC 次数。
- `AssemblyUnloadBarrierState.Faulted` 不会自动回到 Completed；owner 必须阻止 Play、Build、Export 与下一次 reload。

## 标准流程

`GenerationCoordinator` 是 ModuleHost 持有的统一 admission owner，状态为 Ready/Transitioning/AwaitingCollection/Faulted。`Execute(operation, publication, changes)` 使用 `IGenerationPublication<TProbe>` 与 `IGenerationChange` 完成 prepare、activate、apply、complete；失败时逆序撤销结构、恢复旧 publication 和旧状态。不可逆清理/回滚失败进入 Faulted。

`Configure` 复制 GC 策略；`EnsureReady` 检查新的变更/Play admission；`AcquireRead` 允许并行 Build/Export reader，并阻止期间开始 generation transaction；`TrackRetirement` 不丢弃已 pending monitor；`Advance` / `Wait` 驱动退休。GC/finalizer 等待不持有 admission lock。ScriptReloadHost 与 EditorReloadCoordinator 复用该 owner，不各维护一个 barrier。

`AcquireOperation(operation)` 是**同步领域操作**的最小保护边界：从捕获当前 owner 到修改、观察者通知和补偿
结束均持有 scope，期间自动 Catalog 刷新延后。发布事务只能在自己的控制线程借用该 scope，其他线程被拒绝；
借用 scope 的 Dispose 不会提前结束 publication。已经发布的数据在 AwaitingCollection 时仍可访问，但这不授权
Play、Build、Export 或新 reload；这些操作继续使用 `EnsureReady` / `AcquireRead`。Faulted 时所有访问均被拒绝。
该 API 不自动刷新 Catalog；需要接纳最新快照的 owner 应在捕获领域对象前先对账，再取得 scope。
`TypeCatalog.AcquireOperation` 是相同 gate 的转交入口，不是第二套生命周期协议。

Core `RetirementPendingException` 是与普通清理错误不同的所有权信号。Execute 在 prepare、commit cleanup
或 rollback 中收到它时，保留 publication 与领域 change 数组并进入 Faulted，原样抛出；不继续卸载下层，
不恢复可能仍被在途工作使用的旧结构，也不清空事务后回到 Ready。可重试排空应由领域 owner 在返回前
通过 `RetirementBarrier` 完成；到达跨 generation 事务边界仍未排空就不能再继续该进程的代际操作。

上述规则也适用于包装异常。GenerationCoordinator 不再维护自己的异常递归函数，使用 Core
`RetirementPendingException.Find`；Module、TypeRegistry、Reference Recovery、Host 和各领域复用同一分类。
事务 catch 保留原始异常与未完成 transaction，不清空 participant 数组、不落入普通补偿路径继续销毁依赖。

下面的低层 barrier 示例适合实现 probe/测试；产品流程应复用 host.generations，而不是创建并行 gate。

```csharp
var barrier = new AssemblyUnloadBarrier(
    probes,
    new AssemblyUnloadBarrierOptions
    {
        collectionInterval = TimeSpan.FromMilliseconds(250),
        retentionTimeout = TimeSpan.FromSeconds(30)
    });

while (!barrier.Advance())
{
    // Return to the Editor frame loop without publishing reload success.
}
```

Shutdown 可以调用 `Wait()`；交互式 Editor 应逐帧调用 `Advance()`。达到阈值时异常必须成为 reload
subsystem 的持久 Fault，而不是清空 monitor 后继续运行。

## 依赖与相邻项目

本项目属于 Foundation，依赖 Core Execution 的退休信号，不依赖 Assets、Scene、Runtime 或 Editor。`Inno.Extensibility.Modules` 提供具体
`AssemblyUnloadMonitor` probe；`Inno.Scripting.Reload` 和 Plugin composition 使用 barrier 控制可见完成语义。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Extensibility.Reload.AssemblyUnloadBarrier`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Extensibility.Reload.AssemblyUnloadBarrier`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadBarrier.cs#L12) | Prevents a generation transition from completing until every retired collectible assembly is reclaimed. |
| [`Inno.Extensibility.Reload.AssemblyUnloadBarrier.AssemblyUnloadBarrier(System.Collections.Generic.IEnumerable<Inno.Extensibility.Reload.IAssemblyUnloadProbe> probes, Inno.Extensibility.Reload.AssemblyUnloadBarrierOptions? options = null)`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadBarrier.cs#L39) | Creates a barrier over a complete retirement set. |
| [`Inno.Extensibility.Reload.AssemblyUnloadBarrierState Inno.Extensibility.Reload.AssemblyUnloadBarrier.state`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadBarrier.cs#L66) | Gets the current barrier state. |
| [`Inno.Extensibility.Reload.AssemblyUnloadException? Inno.Extensibility.Reload.AssemblyUnloadBarrier.failure`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadBarrier.cs#L90) | Gets the terminal failure after the barrier enters . |
| [`bool Inno.Extensibility.Reload.AssemblyUnloadBarrier.Advance()`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadBarrier.cs#L108) | Performs a due full collection cycle and reevaluates every retired generation. |
| [`int Inno.Extensibility.Reload.AssemblyUnloadBarrier.collectionAttempts`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadBarrier.cs#L78) | Gets the number of full collection cycles performed by this barrier. |
| [`void Inno.Extensibility.Reload.AssemblyUnloadBarrier.Wait(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadBarrier.cs#L164) | Blocks the caller until all retired generations are reclaimed, cancellation is requested, or the barrier faults. |

### `Inno.Extensibility.Reload.AssemblyUnloadBarrierOptions`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Extensibility.Reload.AssemblyUnloadBarrierOptions`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadBarrierOptions.cs#L8) | Configures forced-collection cadence and the retention failure threshold. |
| [`System.TimeSpan Inno.Extensibility.Reload.AssemblyUnloadBarrierOptions.collectionInterval`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadBarrierOptions.cs#L13) | Gets the minimum interval between full collection attempts. |
| [`System.TimeSpan Inno.Extensibility.Reload.AssemblyUnloadBarrierOptions.retentionTimeout`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadBarrierOptions.cs#L18) | Gets the duration after which a still-reachable generation faults the reload subsystem. |

### `Inno.Extensibility.Reload.AssemblyUnloadBarrierState`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Extensibility.Reload.AssemblyUnloadBarrierState`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadBarrierState.cs#L6) | Describes the terminal or waiting state of an assembly unload barrier. |
| [`Inno.Extensibility.Reload.AssemblyUnloadBarrierState.AwaitingCollection`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadBarrierState.cs#L11) | At least one retired collectible generation remains reachable. |
| [`Inno.Extensibility.Reload.AssemblyUnloadBarrierState.Completed`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadBarrierState.cs#L16) | Every retired generation is unreachable and cleaned up. |
| [`Inno.Extensibility.Reload.AssemblyUnloadBarrierState.Faulted`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadBarrierState.cs#L21) | The retention threshold was reached and the owning reload subsystem must stop accepting work. |

### `Inno.Extensibility.Reload.AssemblyUnloadException`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Extensibility.Reload.AssemblyUnloadException`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadException.cs#L10) | Reports retired collectible generations that remained reachable after the unload barrier threshold. |
| [`Inno.Extensibility.Reload.AssemblyUnloadException.AssemblyUnloadException(System.Collections.Generic.IReadOnlyList<string> retainedGenerations, System.TimeSpan elapsed, int collectionAttempts)`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadException.cs#L30) | Creates a terminal unload failure with stable generation diagnostics. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Extensibility.Reload.AssemblyUnloadException.retainedGenerations`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadException.cs#L48) | Gets stable descriptions of every generation that prevented completion. |
| [`System.TimeSpan Inno.Extensibility.Reload.AssemblyUnloadException.elapsed`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadException.cs#L53) | Gets the elapsed duration before the barrier faulted. |
| [`int Inno.Extensibility.Reload.AssemblyUnloadException.collectionAttempts`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadException.cs#L58) | Gets the number of full collection cycles performed before failure. |

### `Inno.Extensibility.Reload.GenerationCoordinator`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Extensibility.Reload.GenerationCoordinator`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationCoordinator.cs#L13) | Owns generation admission, atomic domain changes, rollback, and non-bypassable weak unload verification. |
| [`Inno.Extensibility.Reload.GenerationState Inno.Extensibility.Reload.GenerationCoordinator.state`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationCoordinator.cs#L30) | Gets the gate state shared by reload, Play, Build and Export owners. |
| [`System.Exception? Inno.Extensibility.Reload.GenerationCoordinator.failure`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationCoordinator.cs#L42) | Gets the terminal failure, retained until the entire host is discarded. |
| [`System.IDisposable Inno.Extensibility.Reload.GenerationCoordinator.AcquireOperation(string operation)`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationCoordinator.cs#L143) | Keeps one synchronous operation on the published generation and defers automatic catalog replacement. |
| [`System.IDisposable Inno.Extensibility.Reload.GenerationCoordinator.AcquireRead(string operation)`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationCoordinator.cs#L110) | Pins generation admission for a Build or Export operation, including asynchronous snapshot consumers. |
| [`TProbe Inno.Extensibility.Reload.GenerationCoordinator.Execute<TProbe>(string operation, Inno.Extensibility.Reload.IGenerationPublication<TProbe> publication, System.Collections.Generic.IReadOnlyList<Inno.Extensibility.Reload.IGenerationChange> changes)`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationCoordinator.cs#L239) | Activates one publication and its captured domain changes, preserving all rollback and cleanup failures. |
| [`bool Inno.Extensibility.Reload.GenerationCoordinator.Advance()`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationCoordinator.cs#L341) | Runs a due Full GC/finalizer/Full GC cycle only after candidate stack frames have unwound. |
| [`bool Inno.Extensibility.Reload.GenerationCoordinator.TryAcquireChange(string operation, out System.IDisposable? reservation)`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationCoordinator.cs#L177) | Tries to reserve exclusive owner-controlled publication without disturbing active read leases. |
| [`void Inno.Extensibility.Reload.GenerationCoordinator.Configure(Inno.Extensibility.Reload.AssemblyUnloadBarrierOptions options)`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationCoordinator.cs#L60) | Configures collection cadence before beginning a generation or retirement. |
| [`void Inno.Extensibility.Reload.GenerationCoordinator.EnsureReady(string operation)`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationCoordinator.cs#L82) | Rejects work while a candidate, retained context or terminal failure is pending. |
| [`void Inno.Extensibility.Reload.GenerationCoordinator.EnsureRetirementSafe()`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationCoordinator.cs#L431) | Rejects dependency destruction after a generation owner reported unfinished retirement. |
| [`void Inno.Extensibility.Reload.GenerationCoordinator.Fault(System.Exception exception)`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationCoordinator.cs#L410) | Permanently closes admission when an owner cannot safely retire or restore generation state. |
| [`void Inno.Extensibility.Reload.GenerationCoordinator.TrackRetirement(Inno.Extensibility.Reload.IAssemblyUnloadProbe probe)`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationCoordinator.cs#L202) | Registers every retired or discarded context without dropping already pending monitors. |
| [`void Inno.Extensibility.Reload.GenerationCoordinator.Wait(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationCoordinator.cs#L395) | Waits for all retirements while preserving pending monitors on cancellation or failure. |

### `Inno.Extensibility.Reload.GenerationState`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Extensibility.Reload.GenerationState`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationState.cs#L6) | Describes the shared admission gate for owner-thread generation transactions. |
| [`Inno.Extensibility.Reload.GenerationState.AwaitingCollection`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationState.cs#L19) | Publication finished but retired collectible contexts have not been verified unreachable. |
| [`Inno.Extensibility.Reload.GenerationState.Faulted`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationState.cs#L23) | A rollback, cleanup or unload failure requires a complete host restart. |
| [`Inno.Extensibility.Reload.GenerationState.Ready`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationState.cs#L11) | No candidate or unverified retirement is pending. |
| [`Inno.Extensibility.Reload.GenerationState.Transitioning`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationState.cs#L15) | A candidate is being activated or rolled back. |

### `Inno.Extensibility.Reload.IAssemblyUnloadProbe`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Extensibility.Reload.IAssemblyUnloadProbe`](../../src/foundation/extensibility/Inno.Extensibility.Reload/IAssemblyUnloadProbe.cs#L6) | Observes one retired collectible generation without retaining its load context. |
| [`bool Inno.Extensibility.Reload.IAssemblyUnloadProbe.isCompleted`](../../src/foundation/extensibility/Inno.Extensibility.Reload/IAssemblyUnloadProbe.cs#L16) | Gets whether the retired generation is unreachable and its generation storage is released. |
| [`string Inno.Extensibility.Reload.IAssemblyUnloadProbe.description`](../../src/foundation/extensibility/Inno.Extensibility.Reload/IAssemblyUnloadProbe.cs#L11) | Gets a stable diagnostic description of the retired generation. |

### `Inno.Extensibility.Reload.IGenerationChange`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Extensibility.Reload.IGenerationChange`](../../src/foundation/extensibility/Inno.Extensibility.Reload/IGenerationChange.cs#L6) | Stages domain-owned state around an atomic generation publication without retaining it after completion. |
| [`void Inno.Extensibility.Reload.IGenerationChange.Apply()`](../../src/foundation/extensibility/Inno.Extensibility.Reload/IGenerationChange.cs#L15) | Applies captured state after candidate publication; partial changes must be rollback-safe. |
| [`void Inno.Extensibility.Reload.IGenerationChange.Complete()`](../../src/foundation/extensibility/Inno.Extensibility.Reload/IGenerationChange.cs#L19) | Releases old instances after irreversible publication; cleanup failures fault the owner. |
| [`void Inno.Extensibility.Reload.IGenerationChange.PrepareForActivation()`](../../src/foundation/extensibility/Inno.Extensibility.Reload/IGenerationChange.cs#L11) | Quiesces old instances while the old publication remains active. |
| [`void Inno.Extensibility.Reload.IGenerationChange.RestorePreviousState()`](../../src/foundation/extensibility/Inno.Extensibility.Reload/IGenerationChange.cs#L27) | Restores old values and lifecycle after the previous publication has been restored. |
| [`void Inno.Extensibility.Reload.IGenerationChange.RollbackStructure()`](../../src/foundation/extensibility/Inno.Extensibility.Reload/IGenerationChange.cs#L23) | Removes provisional structures before restoring the old publication. |

### `Inno.Extensibility.Reload.IGenerationPublication<TProbe>`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Extensibility.Reload.IGenerationPublication<TProbe>`](../../src/foundation/extensibility/Inno.Extensibility.Reload/IGenerationPublication.cs#L9) | Publishes a candidate and supplies a weak retirement monitor at the irreversible commit boundary. |
| [`TProbe Inno.Extensibility.Reload.IGenerationPublication<TProbe>.Complete()`](../../src/foundation/extensibility/Inno.Extensibility.Reload/IGenerationPublication.cs#L25) | Commits publication after all dependent changes apply successfully. |
| [`void Inno.Extensibility.Reload.IGenerationPublication<TProbe>.Activate()`](../../src/foundation/extensibility/Inno.Extensibility.Reload/IGenerationPublication.cs#L14) | Makes the fully prepared candidate provisionally visible at an owner-thread safe point. |
| [`void Inno.Extensibility.Reload.IGenerationPublication<TProbe>.Rollback()`](../../src/foundation/extensibility/Inno.Extensibility.Reload/IGenerationPublication.cs#L18) | Restores the previous publication and retires discarded candidates. |

## 项目依赖

- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
