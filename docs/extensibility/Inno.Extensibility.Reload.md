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
using System;
using System.Collections.Generic;
using Inno.Extensibility.Reload;

static AssemblyUnloadBarrier CreateOwnedBarrier(IEnumerable<IAssemblyUnloadProbe> probes)
{
    return new AssemblyUnloadBarrier(probes, new AssemblyUnloadBarrierOptions
    {
        collectionInterval = TimeSpan.FromMilliseconds(250),
        retentionTimeout = TimeSpan.FromSeconds(30)
    });
}

static bool AdvanceAtFrameSafePoint(AssemblyUnloadBarrier barrier)
{
    return barrier.Advance();
}
```

Shutdown 可以调用 `Wait()`；交互式 Editor 应逐帧调用 `Advance()`。达到阈值时异常必须成为 reload
subsystem 的持久 Fault，而不是清空 monitor 后继续运行。

## 依赖与相邻项目

本项目属于 Foundation，依赖 Core Execution 的退休信号，不依赖 Assets、Scene、Runtime 或 Editor。`Inno.Extensibility.Modules` 提供具体
`AssemblyUnloadMonitor` probe；`Inno.Scripting.Reload` 和 Plugin composition 使用 barrier 控制可见完成语义。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Extensibility.Reload.AssemblyUnloadBarrier`

<!-- api: T:Inno.Extensibility.Reload.AssemblyUnloadBarrier 2c547be59e981a5bb26a03090f9fda201f0d2978dbd0943632af8cfce493617a -->
<!-- api: M:Inno.Extensibility.Reload.AssemblyUnloadBarrier.#ctor(System.Collections.Generic.IEnumerable{Inno.Extensibility.Reload.IAssemblyUnloadProbe},Inno.Extensibility.Reload.AssemblyUnloadBarrierOptions) f33007f29e1e91bf5a79d8b336d886ac992022d4b02b4108945189b3b36a6791 -->
<!-- api: P:Inno.Extensibility.Reload.AssemblyUnloadBarrier.state 1c65ddf9236136d06680769895d2e3b876927baa9a71e7066ff93dc42c155958 -->
<!-- api: P:Inno.Extensibility.Reload.AssemblyUnloadBarrier.collectionAttempts fc702c5b2894695c86a3396f644d857c04eee0ac68760cc9aa8647b874b5ae3d -->
<!-- api: P:Inno.Extensibility.Reload.AssemblyUnloadBarrier.failure 5378288366bd598a45d2bdf5172a875eb8d9602bf576f4f437c7c6370b4e65e0 -->
<!-- api: M:Inno.Extensibility.Reload.AssemblyUnloadBarrier.Advance 9096c6af39310e437cc08858ab34ebcd2c603ac0fecaae731582affbf5479d9f -->
<!-- api: M:Inno.Extensibility.Reload.AssemblyUnloadBarrier.Wait(System.Threading.CancellationToken) c5358ef83909fbd95c4d3a9a40029a84f9397c661a6d1c156fc95f47f2d4e744 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-de3b67c2dc466fed"></a>[`public Inno.Extensibility.Reload.AssemblyUnloadBarrier.AssemblyUnloadBarrier(System.Collections.Generic.IEnumerable<Inno.Extensibility.Reload.IAssemblyUnloadProbe> probes, Inno.Extensibility.Reload.AssemblyUnloadBarrierOptions? options = null)`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadBarrier.cs#L39) | Creates a barrier over a complete retirement set.<br>`probes`：Weak unload probes for every retired collectible generation in the transaction.<br>`options`：Optional collection cadence and retention threshold. | — | `System.ArgumentNullException`：Thrown when probes or one of its values is null.<br>`System.ArgumentOutOfRangeException`：Thrown when an option duration is negative or the retention threshold is zero. |
| <a id="api-04630c847f89f91e"></a>[`public bool Inno.Extensibility.Reload.AssemblyUnloadBarrier.Advance()`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadBarrier.cs#L108) | Performs a due full collection cycle and reevaluates every retired generation.<br>返回：true only when every probe has completed; otherwise, false. | — | `Inno.Extensibility.Reload.AssemblyUnloadException`：Thrown on every call after the retention threshold is reached while a generation remains reachable. |
| <a id="api-f92221354de70ac0"></a>[`public void Inno.Extensibility.Reload.AssemblyUnloadBarrier.Wait(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadBarrier.cs#L164) | Blocks the caller until all retired generations are reclaimed, cancellation is requested, or the barrier faults.<br>`cancellationToken`：A token that can cancel the wait without changing the barrier state. | — | `System.OperationCanceledException`：Thrown when cancellationToken is canceled before completion.<br>`Inno.Extensibility.Reload.AssemblyUnloadException`：Thrown when the retention threshold is reached. |
| <a id="api-3d851dc0fc921be8"></a>[`public int Inno.Extensibility.Reload.AssemblyUnloadBarrier.collectionAttempts { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadBarrier.cs#L78) | Gets the number of full collection cycles performed by this barrier. | — | — |
| <a id="api-8ff6fec1fcd867aa"></a>[`public Inno.Extensibility.Reload.AssemblyUnloadException? Inno.Extensibility.Reload.AssemblyUnloadBarrier.failure { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadBarrier.cs#L90) | Gets the terminal failure after the barrier enters Inno.Extensibility.Reload.AssemblyUnloadBarrierState.Faulted. | — | — |
| <a id="api-90dc8c5d17d1727f"></a>[`public Inno.Extensibility.Reload.AssemblyUnloadBarrierState Inno.Extensibility.Reload.AssemblyUnloadBarrier.state { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadBarrier.cs#L66) | Gets the current barrier state. | — | — |
| <a id="api-f2a4cad4a58221e5"></a>[`public sealed class Inno.Extensibility.Reload.AssemblyUnloadBarrier`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadBarrier.cs#L12) | Prevents a generation transition from completing until every retired collectible assembly is reclaimed. | — | — |

### `Inno.Extensibility.Reload.AssemblyUnloadBarrierOptions`

<!-- api: T:Inno.Extensibility.Reload.AssemblyUnloadBarrierOptions 26689dfcc6e91f323096e99c21da094854580f3a54f85afdd83660737bfa935c -->
<!-- api: P:Inno.Extensibility.Reload.AssemblyUnloadBarrierOptions.collectionInterval 469da742edb7cddfb310df9efea45aea34757ef7007015d95828cb727d4463f2 -->
<!-- api: P:Inno.Extensibility.Reload.AssemblyUnloadBarrierOptions.retentionTimeout 2eeedfacb7587d8747c78f975161b435a87bbfb55655a60ee9d9ce47a0e22cbf -->
<!-- api: M:Inno.Extensibility.Reload.AssemblyUnloadBarrierOptions.#ctor 8af4813db4c37e9afa6dd9f98576a35f780e9a416d3793f6a857fe4af87b800b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2cff3683276be3dc"></a>[`public Inno.Extensibility.Reload.AssemblyUnloadBarrierOptions.AssemblyUnloadBarrierOptions()`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadBarrierOptions.cs#L8) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-2df08c4a705a8b32"></a>[`public System.TimeSpan Inno.Extensibility.Reload.AssemblyUnloadBarrierOptions.collectionInterval { get; init; }`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadBarrierOptions.cs#L13) | Gets the minimum interval between full collection attempts. | — | — |
| <a id="api-cf686e60505f0a13"></a>[`public System.TimeSpan Inno.Extensibility.Reload.AssemblyUnloadBarrierOptions.retentionTimeout { get; init; }`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadBarrierOptions.cs#L18) | Gets the duration after which a still-reachable generation faults the reload subsystem. | — | — |
| <a id="api-0a144a712f7cbd95"></a>[`public sealed class Inno.Extensibility.Reload.AssemblyUnloadBarrierOptions`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadBarrierOptions.cs#L8) | Configures forced-collection cadence and the retention failure threshold. | — | — |

### `Inno.Extensibility.Reload.AssemblyUnloadBarrierState`

<!-- api: T:Inno.Extensibility.Reload.AssemblyUnloadBarrierState f95aaa89d663a30d790b928ff7ed8384f082ebfba717c449ec4f453e5cfe017a -->
<!-- api: F:Inno.Extensibility.Reload.AssemblyUnloadBarrierState.AwaitingCollection 9e2fbc4c32e77e05469b08ca2aeda5f021690641a97a1f62a0e913454f161107 -->
<!-- api: F:Inno.Extensibility.Reload.AssemblyUnloadBarrierState.Completed d3108ed424d996eb8e9c4e6ef4f292d87cb99f51f165269cb2bdef619f8ab722 -->
<!-- api: F:Inno.Extensibility.Reload.AssemblyUnloadBarrierState.Faulted 0f7902e1a7731e470b86befcbedef1712d0af591e1d8c262a8d1fd32aff0a250 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-05167f014c35fbe7"></a>[`Inno.Extensibility.Reload.AssemblyUnloadBarrierState.AwaitingCollection = 0`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadBarrierState.cs#L11) | At least one retired collectible generation remains reachable. | — | — |
| <a id="api-e659c729d9c051f5"></a>[`Inno.Extensibility.Reload.AssemblyUnloadBarrierState.Completed = 1`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadBarrierState.cs#L16) | Every retired generation is unreachable and cleaned up. | — | — |
| <a id="api-503a0cf36106f2f7"></a>[`Inno.Extensibility.Reload.AssemblyUnloadBarrierState.Faulted = 2`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadBarrierState.cs#L21) | The retention threshold was reached and the owning reload subsystem must stop accepting work. | — | — |
| <a id="api-aaa5f4ef222db37f"></a>[`public enum Inno.Extensibility.Reload.AssemblyUnloadBarrierState : int`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadBarrierState.cs#L6) | Describes the terminal or waiting state of an assembly unload barrier. | — | — |

### `Inno.Extensibility.Reload.AssemblyUnloadException`

<!-- api: T:Inno.Extensibility.Reload.AssemblyUnloadException ef701db1d847c0a3d0c98fc6ff229a0900eb2f0155085a4f413e8fa02e7985d2 -->
<!-- api: M:Inno.Extensibility.Reload.AssemblyUnloadException.#ctor(System.Collections.Generic.IReadOnlyList{System.String},System.TimeSpan,System.Int32) 964abf72d13c9467a354dfffa6af841d77f3140595fe695f5f6b82905fb4952d -->
<!-- api: P:Inno.Extensibility.Reload.AssemblyUnloadException.retainedGenerations 1da4f8cebe24ea6e42028b5d38a5e19e33cac01449d4b412543ec562e3436251 -->
<!-- api: P:Inno.Extensibility.Reload.AssemblyUnloadException.elapsed 9c1c0e9050ad01b364bae81fba8fb9f400b71be233d47d0bae7828f190068dad -->
<!-- api: P:Inno.Extensibility.Reload.AssemblyUnloadException.collectionAttempts 7187cad135e6fc470ecd34569483f05a71f4e9357a5d142bb79c288622702a19 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-861505e94d606d16"></a>[`public Inno.Extensibility.Reload.AssemblyUnloadException.AssemblyUnloadException(System.Collections.Generic.IReadOnlyList<string> retainedGenerations, System.TimeSpan elapsed, int collectionAttempts)`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadException.cs#L30) | Creates a terminal unload failure with stable generation diagnostics.<br>`retainedGenerations`：Descriptions of every retired generation that remains reachable.<br>`elapsed`：The elapsed barrier duration.<br>`collectionAttempts`：The number of full collection cycles already performed. | — | `System.ArgumentNullException`：Thrown when retainedGenerations is null.<br>`System.ArgumentException`：Thrown when no retained generation is supplied. |
| <a id="api-4edfe7769be9c55c"></a>[`public int Inno.Extensibility.Reload.AssemblyUnloadException.collectionAttempts { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadException.cs#L58) | Gets the number of full collection cycles performed before failure. | — | — |
| <a id="api-fadaf0f33956c86a"></a>[`public System.TimeSpan Inno.Extensibility.Reload.AssemblyUnloadException.elapsed { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadException.cs#L53) | Gets the elapsed duration before the barrier faulted. | — | — |
| <a id="api-56def0329cefd45d"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Extensibility.Reload.AssemblyUnloadException.retainedGenerations { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadException.cs#L48) | Gets stable descriptions of every generation that prevented completion. | — | — |
| <a id="api-b5baecaf4258325d"></a>[`public sealed class Inno.Extensibility.Reload.AssemblyUnloadException : System.InvalidOperationException`](../../src/foundation/extensibility/Inno.Extensibility.Reload/AssemblyUnloadException.cs#L10) | Reports retired collectible generations that remained reachable after the unload barrier threshold. | — | — |

### `Inno.Extensibility.Reload.GenerationCoordinator`

<!-- api: T:Inno.Extensibility.Reload.GenerationCoordinator 63838bf75a84203aaa32e2b1dfd863ed6cfdb38f4e8d998ddcf009bdd088f261 -->
<!-- api: P:Inno.Extensibility.Reload.GenerationCoordinator.state ef9d3c19ddb150253f1a3ed6e35f051de5327d2bcc257ba369921ed2ee2bb5be -->
<!-- api: P:Inno.Extensibility.Reload.GenerationCoordinator.failure 5c3ff66b7353b250300f634c2038a627b73fe63eb1628f5bcb8fc275bb86beab -->
<!-- api: M:Inno.Extensibility.Reload.GenerationCoordinator.Configure(Inno.Extensibility.Reload.AssemblyUnloadBarrierOptions) 12f73ae39ce45a2232d9e2d5d5ba3cac6f3aa5b65998369c0a4cc400f8592cce -->
<!-- api: M:Inno.Extensibility.Reload.GenerationCoordinator.EnsureReady(System.String) f5868a6f365ade513fc2ecd60cd3cbf46a0a6b7aaf18ef4855825af14d41d6d5 -->
<!-- api: M:Inno.Extensibility.Reload.GenerationCoordinator.AcquireRead(System.String) 01db83a55243d9b33f4bda5c536755b8d34f771309a617f8289fc9b028338f4f -->
<!-- api: M:Inno.Extensibility.Reload.GenerationCoordinator.AcquireOperation(System.String) cf2b5abc863e823ff2442cb4017172d404b5280d7ced594067ab06322a9689bc -->
<!-- api: M:Inno.Extensibility.Reload.GenerationCoordinator.TryAcquireChange(System.String,System.IDisposable@) 4e09a5aaec08660a2804c4949e8496f6198278835eee9d7a2e8e339616445c49 -->
<!-- api: M:Inno.Extensibility.Reload.GenerationCoordinator.TrackRetirement(Inno.Extensibility.Reload.IAssemblyUnloadProbe) f3bd8037d45da640ef8f3ccdab28b03bdc62cd7af8b0cd944e2137d38ac94d5e -->
<!-- api: M:Inno.Extensibility.Reload.GenerationCoordinator.Execute``1(System.String,Inno.Extensibility.Reload.IGenerationPublication{``0},System.Collections.Generic.IReadOnlyList{Inno.Extensibility.Reload.IGenerationChange}) 3e7e665ecdee47a9ceff86820321b5b969f26770e6e1f3b3990da3f2fb486ce3 -->
<!-- api: M:Inno.Extensibility.Reload.GenerationCoordinator.Advance 5a2bea7abe29b545b153327626787a708eb9fe930614a5c08c7879cb7b3903df -->
<!-- api: M:Inno.Extensibility.Reload.GenerationCoordinator.Wait(System.Threading.CancellationToken) 48c50e87f720161b2b17eea9b5f860a82c638899c73c3acd3a42478bdc44f452 -->
<!-- api: M:Inno.Extensibility.Reload.GenerationCoordinator.Fault(System.Exception) 265388b9727b7b5dcde2019c9646d7a7b155a736a15cdd0256dbaa63059c638a -->
<!-- api: M:Inno.Extensibility.Reload.GenerationCoordinator.EnsureRetirementSafe 19836f5581e5ec4ebd3e1f1fe0eda8b17c2d751a313d53b0173bd7cf58f3d656 -->
<!-- api: M:Inno.Extensibility.Reload.GenerationCoordinator.#ctor 1cdab85c0b19fd0be224bc2ae1f80e43c52fad71c33bc6150288193ec3bd05d1 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c67f1a6086d8a5d0"></a>[`public Inno.Extensibility.Reload.GenerationCoordinator.GenerationCoordinator()`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationCoordinator.cs#L13) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-7d69a2b465f7f97b"></a>[`public System.IDisposable Inno.Extensibility.Reload.GenerationCoordinator.AcquireOperation(string operation)`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationCoordinator.cs#L143) | Keeps one synchronous operation on the published generation and defers automatic catalog replacement.<br>返回：A scope for the entire operation, or a borrowed scope inside publication on its own control thread.<br>`operation`：The operation name included in admission failures. | Published data remains readable while old contexts await collection. This scope does not authorize Play, Build, Export or a new generation transaction; those owners must still use their strict admission boundary. | `System.InvalidOperationException`：Another thread is publishing or the owner is Faulted.<br>`Inno.Core.Execution.RetirementPendingException`：Retirement has failed without releasing its dependent owners. |
| <a id="api-f9eb1d3fa3f25ef5"></a>[`public System.IDisposable Inno.Extensibility.Reload.GenerationCoordinator.AcquireRead(string operation)`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationCoordinator.cs#L110) | Pins generation admission for a Build or Export operation, including asynchronous snapshot consumers.<br>返回：A lease that must be disposed after every snapshot and asynchronous consumer has retired.<br>`operation`：The diagnostic operation name. | — | `System.InvalidOperationException`：Admission is blocked by another operation or a retirement. |
| <a id="api-e7b4be4d75c839ab"></a>[`public bool Inno.Extensibility.Reload.GenerationCoordinator.Advance()`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationCoordinator.cs#L341) | Runs a due Full GC/finalizer/Full GC cycle only after candidate stack frames have unwound.<br>返回：True only when every tracked context is unreachable and admission is ready. | — | `System.InvalidOperationException`：The gate is terminally Faulted.<br>`Inno.Extensibility.Reload.AssemblyUnloadException`：A retained context exceeded the configured deadline. |
| <a id="api-871e140e2395e87a"></a>[`public void Inno.Extensibility.Reload.GenerationCoordinator.Configure(Inno.Extensibility.Reload.AssemblyUnloadBarrierOptions options)`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationCoordinator.cs#L60) | Configures collection cadence before beginning a generation or retirement.<br>`options`：The collection and retention policy, copied by the coordinator. | — | `System.InvalidOperationException`：The gate is not ready. |
| <a id="api-d3dc56bd93ca57b8"></a>[`public void Inno.Extensibility.Reload.GenerationCoordinator.EnsureReady(string operation)`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationCoordinator.cs#L82) | Rejects work while a candidate, retained context or terminal failure is pending.<br>`operation`：The caller's operation name included in rejection diagnostics. | — | `System.InvalidOperationException`：Admission is blocked or the host requires restart. |
| <a id="api-57e05dfa78bacf43"></a>[`public void Inno.Extensibility.Reload.GenerationCoordinator.EnsureRetirementSafe()`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationCoordinator.cs#L431) | Rejects dependency destruction after a generation owner reported unfinished retirement. | Ordinary terminal cleanup failures do not prevent remaining cleanup. Unfinished retirement remains blocking even if an earlier ordinary error is the primary diagnostic failure. | `Inno.Core.Execution.RetirementPendingException`：A failed generation still owns live work; its dependencies must remain owned until host restart. |
| <a id="api-dd8e02ceec182a3f"></a>[`public TProbe Inno.Extensibility.Reload.GenerationCoordinator.Execute<TProbe>(string operation, Inno.Extensibility.Reload.IGenerationPublication<TProbe> publication, System.Collections.Generic.IReadOnlyList<Inno.Extensibility.Reload.IGenerationChange> changes) where TProbe : Inno.Extensibility.Reload.IAssemblyUnloadProbe`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationCoordinator.cs#L239) | Activates one publication and its captured domain changes, preserving all rollback and cleanup failures.<br>返回：The retirement monitor; admission remains blocked until all monitors pass verification.<br>`operation`：A stable diagnostic name for the generation operation.<br>`publication`：The prepared candidate publication.<br>`changes`：Captured domain changes in deterministic dependency order. | — | `System.AggregateException`：An irreversible cleanup or rollback stage failed and the gate is Faulted.<br>`System.InvalidOperationException`：Another generation or retirement is pending. |
| <a id="api-b5c0bb41b0d76a4d"></a>[`public void Inno.Extensibility.Reload.GenerationCoordinator.Fault(System.Exception exception)`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationCoordinator.cs#L410) | Permanently closes admission when an owner cannot safely retire or restore generation state.<br>`exception`：The cleanup or rollback failure retained for diagnostics until the host restarts. | — | — |
| <a id="api-2ac042bb045e344d"></a>[`public void Inno.Extensibility.Reload.GenerationCoordinator.TrackRetirement(Inno.Extensibility.Reload.IAssemblyUnloadProbe probe)`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationCoordinator.cs#L202) | Registers every retired or discarded context without dropping already pending monitors.<br>`probe`：A stable engine-owned weak monitor that must not retain the collectible context. | — | — |
| <a id="api-4d4523ad98b3a4cc"></a>[`public bool Inno.Extensibility.Reload.GenerationCoordinator.TryAcquireChange(string operation, out System.IDisposable? reservation)`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationCoordinator.cs#L177) | Tries to reserve exclusive owner-controlled publication without disturbing active read leases.<br>返回：True only when no reader, generation change or retirement is active.<br>`operation`：The operation name used when a terminal failure rejects admission.<br>`reservation`：Receives the exclusive reservation, or null when the caller must defer mutation. | — | `System.InvalidOperationException`：The generation owner is Faulted and requires a host restart. |
| <a id="api-6d7734fc5cc219bc"></a>[`public void Inno.Extensibility.Reload.GenerationCoordinator.Wait(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationCoordinator.cs#L395) | Waits for all retirements while preserving pending monitors on cancellation or failure.<br>`cancellationToken`：Cancellation for the wait, not permission to resume generation changes. | — | `System.OperationCanceledException`：The caller canceled before retirement completed. |
| <a id="api-efc9e6984c661eee"></a>[`public System.Exception? Inno.Extensibility.Reload.GenerationCoordinator.failure { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationCoordinator.cs#L42) | Gets the terminal failure, retained until the entire host is discarded. | — | — |
| <a id="api-97c2110b10aa02cf"></a>[`public Inno.Extensibility.Reload.GenerationState Inno.Extensibility.Reload.GenerationCoordinator.state { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationCoordinator.cs#L30) | Gets the gate state shared by reload, Play, Build and Export owners. | — | — |
| <a id="api-7dea96ba0ecafdeb"></a>[`public sealed class Inno.Extensibility.Reload.GenerationCoordinator`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationCoordinator.cs#L13) | Owns generation admission, atomic domain changes, rollback, and non-bypassable weak unload verification. | — | — |

### `Inno.Extensibility.Reload.GenerationState`

<!-- api: T:Inno.Extensibility.Reload.GenerationState b3a2e73fa5393eddf83bd29c248d87964f784987debdbfe7fa1dc12a7a6954f7 -->
<!-- api: F:Inno.Extensibility.Reload.GenerationState.Ready 7d38222c56fdfaaf9cd6e56f59954af7ada9446c683d2e6fa0571301a157db76 -->
<!-- api: F:Inno.Extensibility.Reload.GenerationState.Transitioning 8b1a744a65f2b313b2bce21d32a79f7f6bb1fbd6d600966c460f18abbbcdb21f -->
<!-- api: F:Inno.Extensibility.Reload.GenerationState.AwaitingCollection 185034f216dc2762a188a914994fb85fea86c311e34914294b889fda722d9ae6 -->
<!-- api: F:Inno.Extensibility.Reload.GenerationState.Faulted 37949367c44653160e46546d37ef03b60d0be5fb652ba042fbd79a0000f3debf -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c316cde119880a47"></a>[`Inno.Extensibility.Reload.GenerationState.AwaitingCollection = 2`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationState.cs#L19) | Publication finished but retired collectible contexts have not been verified unreachable. | — | — |
| <a id="api-387e7b2d911fcfa8"></a>[`Inno.Extensibility.Reload.GenerationState.Faulted = 3`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationState.cs#L23) | A rollback, cleanup or unload failure requires a complete host restart. | — | — |
| <a id="api-26659c4276a50431"></a>[`Inno.Extensibility.Reload.GenerationState.Ready = 0`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationState.cs#L11) | No candidate or unverified retirement is pending. | — | — |
| <a id="api-c4498296b0ef8fe9"></a>[`Inno.Extensibility.Reload.GenerationState.Transitioning = 1`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationState.cs#L15) | A candidate is being activated or rolled back. | — | — |
| <a id="api-c8773da1e3bbf4ec"></a>[`public enum Inno.Extensibility.Reload.GenerationState : int`](../../src/foundation/extensibility/Inno.Extensibility.Reload/GenerationState.cs#L6) | Describes the shared admission gate for owner-thread generation transactions. | — | — |

### `Inno.Extensibility.Reload.IAssemblyUnloadProbe`

<!-- api: T:Inno.Extensibility.Reload.IAssemblyUnloadProbe 76bc61c54543d56b85dfcb8429aad0f8671ba842cd2888195e9d51f0f8a87dbf -->
<!-- api: P:Inno.Extensibility.Reload.IAssemblyUnloadProbe.description 5aa030e641ccf5ffe6e960d160cd4d7eed25648fdd1cc4202084faa3b94217a3 -->
<!-- api: P:Inno.Extensibility.Reload.IAssemblyUnloadProbe.isCompleted 46fb77022c2dc47fd4546a86360c83b68e3096f73ff7e3584840b5f3d6d171ab -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f25c59f0d1414f1a"></a>[`string Inno.Extensibility.Reload.IAssemblyUnloadProbe.description { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Reload/IAssemblyUnloadProbe.cs#L11) | Gets a stable diagnostic description of the retired generation. | — | — |
| <a id="api-c57958a059ab790b"></a>[`bool Inno.Extensibility.Reload.IAssemblyUnloadProbe.isCompleted { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Reload/IAssemblyUnloadProbe.cs#L16) | Gets whether the retired generation is unreachable and its generation storage is released. | — | — |
| <a id="api-989ee0a696a1dde7"></a>[`public interface Inno.Extensibility.Reload.IAssemblyUnloadProbe`](../../src/foundation/extensibility/Inno.Extensibility.Reload/IAssemblyUnloadProbe.cs#L6) | Observes one retired collectible generation without retaining its load context. | — | — |

### `Inno.Extensibility.Reload.IGenerationChange`

<!-- api: T:Inno.Extensibility.Reload.IGenerationChange 788ba5bb9d9f49118a1764ca5e830fb1d9282c9718550a8459fda50f5bde6b9c -->
<!-- api: M:Inno.Extensibility.Reload.IGenerationChange.PrepareForActivation 500eab8993ac8310f523c922acf56ef85770549f786944ac426ee3210bf70caa -->
<!-- api: M:Inno.Extensibility.Reload.IGenerationChange.Apply 08e9587792fdf1a4e3ec48d8c9ea3cf4f0f711491198c755d5c4a620eb799d6a -->
<!-- api: M:Inno.Extensibility.Reload.IGenerationChange.Complete 4c72692b296091e35c8199c2b55faf70d479d282e3c9a361fb8b0beb8a2f030d -->
<!-- api: M:Inno.Extensibility.Reload.IGenerationChange.RollbackStructure 85815898126eca2766cc7e318d984e3cb85dc05490e256c4f35ffcf60f1e6203 -->
<!-- api: M:Inno.Extensibility.Reload.IGenerationChange.RestorePreviousState 74ee2847df8aab0131d1034016f16919e0f780218e282c9bbf4ed79bc7affb4d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e8d14225d4e3cbc0"></a>[`void Inno.Extensibility.Reload.IGenerationChange.Apply()`](../../src/foundation/extensibility/Inno.Extensibility.Reload/IGenerationChange.cs#L15) | Applies captured state after candidate publication; partial changes must be rollback-safe. | — | — |
| <a id="api-b61ea336afae53ef"></a>[`void Inno.Extensibility.Reload.IGenerationChange.Complete()`](../../src/foundation/extensibility/Inno.Extensibility.Reload/IGenerationChange.cs#L19) | Releases old instances after irreversible publication; cleanup failures fault the owner. | — | — |
| <a id="api-da4a04a90ec170b5"></a>[`void Inno.Extensibility.Reload.IGenerationChange.PrepareForActivation()`](../../src/foundation/extensibility/Inno.Extensibility.Reload/IGenerationChange.cs#L11) | Quiesces old instances while the old publication remains active. | — | — |
| <a id="api-11c77f8d2f5f9141"></a>[`void Inno.Extensibility.Reload.IGenerationChange.RestorePreviousState()`](../../src/foundation/extensibility/Inno.Extensibility.Reload/IGenerationChange.cs#L27) | Restores old values and lifecycle after the previous publication has been restored. | — | — |
| <a id="api-19e4ac64b5e519ca"></a>[`void Inno.Extensibility.Reload.IGenerationChange.RollbackStructure()`](../../src/foundation/extensibility/Inno.Extensibility.Reload/IGenerationChange.cs#L23) | Removes provisional structures before restoring the old publication. | — | — |
| <a id="api-c57b148c97d895eb"></a>[`public interface Inno.Extensibility.Reload.IGenerationChange`](../../src/foundation/extensibility/Inno.Extensibility.Reload/IGenerationChange.cs#L6) | Stages domain-owned state around an atomic generation publication without retaining it after completion. | — | — |

### `Inno.Extensibility.Reload.IGenerationPublication`1`

<!-- api: T:Inno.Extensibility.Reload.IGenerationPublication`1 e8cfe0d41fb8e2cc465b9311416c9f36c9138318f804146c7156b1b8265d912e -->
<!-- api: M:Inno.Extensibility.Reload.IGenerationPublication`1.Activate fedf0399b958f0a36234104bc55ddc3fcc91d29dca6b8870ecf381b803d1c056 -->
<!-- api: M:Inno.Extensibility.Reload.IGenerationPublication`1.Rollback 45adaa3068b3f7fe3e321a9d4d04b03dac8a18f1a2608c73efdea31ce1551071 -->
<!-- api: M:Inno.Extensibility.Reload.IGenerationPublication`1.Complete 36bff90b9975228e5cb0b19e5689ca483af6524aac3d7b82c63c2910a75fe078 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-52766f426905fd33"></a>[`void Inno.Extensibility.Reload.IGenerationPublication<TProbe>.Activate()`](../../src/foundation/extensibility/Inno.Extensibility.Reload/IGenerationPublication.cs#L14) | Makes the fully prepared candidate provisionally visible at an owner-thread safe point. | — | — |
| <a id="api-5fa4f46f07a3ca09"></a>[`TProbe Inno.Extensibility.Reload.IGenerationPublication<TProbe>.Complete()`](../../src/foundation/extensibility/Inno.Extensibility.Reload/IGenerationPublication.cs#L25) | Commits publication after all dependent changes apply successfully.<br>返回：A weak probe observing retired contexts; success is not reported until it completes. | — | — |
| <a id="api-ad517575fe950f43"></a>[`void Inno.Extensibility.Reload.IGenerationPublication<TProbe>.Rollback()`](../../src/foundation/extensibility/Inno.Extensibility.Reload/IGenerationPublication.cs#L18) | Restores the previous publication and retires discarded candidates. | — | — |
| <a id="api-ba3107770141df84"></a>[`public interface Inno.Extensibility.Reload.IGenerationPublication<TProbe> where TProbe : Inno.Extensibility.Reload.IAssemblyUnloadProbe`](../../src/foundation/extensibility/Inno.Extensibility.Reload/IGenerationPublication.cs#L9) | Publishes a candidate and supplies a weak retirement monitor at the irreversible commit boundary. | — | — |

## 项目依赖

- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
