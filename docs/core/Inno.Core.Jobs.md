# Inno.Core.Jobs

## 有界调度

`JobSchedulerOptions.maxFrameJobs`、`mainThreadCapacity`、`mainThreadDrainBudget` 同时适用于单线程和 worker pool；0 分别采用 65536、65536、4096。负值拒绝。

Frame job 超限或主线程回调队列满时明确抛 InvalidOperationException，调用者不得把异常当作已接受。Drain 最多执行入口时已有的 budget 数量，递归 enqueue 留到后续 drain，不无限吞掉一帧。

Dispose 会先完成活动 frame 的 job，再关闭队列并清除剩余通知/任务记录中的托管引用。它不能中断任意不返回的用户回调。

[Core 索引](README.md) · [Wiki 首页](../README.md) · [Runtime](../runtime/Inno.Runtime.md)

## 公开 API

- `JobScheduler`：session-owned scheduler，支持单线程与工作窃取执行。
- `JobSchedulerOptions`, `JobExecutionMode`：线程和执行策略。
- `JobSchedulerStatistics` / `JobScheduler.statistics`：frameJobs、peakFrameJobs、rejectedJobs、mainThreadPending、mainThreadPeak、mainThreadRejected、mainThreadCanceled 的中立计数快照。
- `JobHandle`：带 generation 的依赖/完成句柄。

一个 scheduler 只能由其 owner 按 BeginFrame/EndFrame contract 驱动。单线程模式拒绝跨线程 mutation；工作窃取模式允许并发 schedule，但 main-thread queue 只在 owner thread drain。stale handle、重复 frame 和 job exception 明确失败。

## 所有权、接纳与失败

Schedule 先验证全部依赖，再分配 record；ParallelFor 在同一个 admission 临界区预留所有分块及 combine record，超限不会留下部分 job。失败 Task/Job 不伪装成成功完成。

Main-thread queue 的 capacity 限制等待项；正在执行的一个 callback 另计，因此 pending/peak 最多是 capacity + 1。Drain 只处理入口快照，不递归耗尽新入队工作。callback 报 Pending 时保留同一个 callback 和 Core RetirementBarrier，后续安全点重试；Close 取消未开始项，但必须退休已经开始的步骤。普通失败跨 Pending 保留，最终一起报告。完成清理后清掉异常引用。

Worker pool 构造失败会停止、唤醒并 join 已启动的线程，再释放同步对象。构造完成前不接纳任务。同步 job 和 callback 必须有限返回；不支持安全抢占任意无限循环代码，也不能把 shutdown deadline 当作 Thread.Abort。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Core.Jobs.JobExecutionMode`

<!-- api: T:Inno.Core.Jobs.JobExecutionMode 0b5125a013f2945ed8cb765b42535f50ff2c56639f296c1be5ce26ba57ca51e7 -->
<!-- api: F:Inno.Core.Jobs.JobExecutionMode.SingleThread f7ee5f62a71c779d2b0868454e948b2376f8f116876f5c857618b3930846c3cb -->
<!-- api: F:Inno.Core.Jobs.JobExecutionMode.WorkerPool f31a3e5c2822a90aef6d9a7aa1991e552816863616cc54e5b25db26ba9fc9e06 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-da0968767d302a51"></a>[`Inno.Core.Jobs.JobExecutionMode.SingleThread = 0`](../../src/foundation/core/Inno.Core.Jobs/JobExecutionMode.cs#L11) | Executes jobs deterministically on the scheduler owner thread. | — | — |
| <a id="api-404d45fee772384b"></a>[`Inno.Core.Jobs.JobExecutionMode.WorkerPool = 1`](../../src/foundation/core/Inno.Core.Jobs/JobExecutionMode.cs#L16) | Executes ready jobs on a bounded work-stealing worker pool. | — | — |
| <a id="api-a45e92a8c3592802"></a>[`public enum Inno.Core.Jobs.JobExecutionMode : int`](../../src/foundation/core/Inno.Core.Jobs/JobExecutionMode.cs#L6) | Selects the execution strategy owned by a job scheduler. | — | — |

### `Inno.Core.Jobs.JobHandle`

<!-- api: T:Inno.Core.Jobs.JobHandle d9fd384e8b10e6c6d22f023f69aa685a8abf629d977237b87a1123fd9d2544f9 -->
<!-- api: P:Inno.Core.Jobs.JobHandle.isValid c083f6e48731e0e6740fcc895378dbb7bb6ecc0a8b64a39710852776a00998c1 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-08df93504e3ce771"></a>[`public bool Inno.Core.Jobs.JobHandle.isValid { get; }`](../../src/foundation/core/Inno.Core.Jobs/JobHandle.cs#L22) | Gets whether this handle contains a valid identifier. | — | — |
| <a id="api-43b1819a53c1f814"></a>[`public readonly struct Inno.Core.Jobs.JobHandle`](../../src/foundation/core/Inno.Core.Jobs/JobHandle.cs#L6) | Opaque handle representing a scheduled job. | — | — |

### `Inno.Core.Jobs.JobScheduler`

<!-- api: T:Inno.Core.Jobs.JobScheduler c23a346ba0e26d073f1f0007a61b94203e46ad4e3e5817f6171d1b7418ce6933 -->
<!-- api: M:Inno.Core.Jobs.JobScheduler.#ctor(Inno.Core.Jobs.JobExecutionMode,Inno.Core.Jobs.JobSchedulerOptions) f57c07dcc588313d9dee110b38475a8decf91d93a25559ac98337d6681519be9 -->
<!-- api: P:Inno.Core.Jobs.JobScheduler.workerCount 53a3cb61f738904611ba370fa53c54f3016eb491053e1b81df7645a942c92e2c -->
<!-- api: P:Inno.Core.Jobs.JobScheduler.statistics 0eadb3ef9c63e7c988b667d742e2f5d98210df3d75726f3a8a39bfddaa03fac4 -->
<!-- api: M:Inno.Core.Jobs.JobScheduler.BeginFrame 53a73da098e3a5d25aedc3d0d2d59ef184139c2dd4b3bda91fc40ed3e5055bdf -->
<!-- api: M:Inno.Core.Jobs.JobScheduler.EndFrame d85ee21a312013da82984249e708ee6aabffc00c30fd9ece140acc5ad61997c9 -->
<!-- api: M:Inno.Core.Jobs.JobScheduler.Schedule(System.Action) 7902eb35ae33a1e6472cc849a754263d236f1a658e6c9851efe48273c6870cee -->
<!-- api: M:Inno.Core.Jobs.JobScheduler.Schedule(System.Action{System.Object},System.Object,System.ReadOnlySpan{Inno.Core.Jobs.JobHandle}) 568d191b496d3ec6ac46ee340a674e2b46f86fe5d7b19c3613877072faf0cc72 -->
<!-- api: M:Inno.Core.Jobs.JobScheduler.CombineDependencies(System.ReadOnlySpan{Inno.Core.Jobs.JobHandle}) 353b782df2ac7ce4661c2cbe8a120786656e5bfac6ba757c49d9521aecd16b6f -->
<!-- api: M:Inno.Core.Jobs.JobScheduler.ParallelFor(System.Int32,System.Int32,System.Action{System.Int32,System.Int32}) 3356526411276c9f5739881fdac7dd5e0c19aeec7d2d4905bd48ec1757d3c432 -->
<!-- api: M:Inno.Core.Jobs.JobScheduler.Complete(Inno.Core.Jobs.JobHandle) e71c87536d369da4a566a578b2e58345789134d6028bfd11de3880beb6c446dc -->
<!-- api: M:Inno.Core.Jobs.JobScheduler.CompleteAll(System.ReadOnlySpan{Inno.Core.Jobs.JobHandle}) 30bfa95558a55b88e21354cb38b7e9bac817f17dc5d8ca7700c3bcce26f631a2 -->
<!-- api: M:Inno.Core.Jobs.JobScheduler.EnqueueMainThread(System.Action) 8c8082cefd0e3ad53d4f51083e4fddca83eaddc7c838207011eb0130ff5d5062 -->
<!-- api: M:Inno.Core.Jobs.JobScheduler.DrainMainThreadQueue ca1c73ea7a8b406ed255c71e7273f4971ddfaea814ba300f4912a15555bec460 -->
<!-- api: M:Inno.Core.Jobs.JobScheduler.Dispose 2d50e688dc220b1741bc14be20fe313be6c5f3e3110c5baeb105d6e12648be87 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d2af170293d5c007"></a>[`public Inno.Core.Jobs.JobScheduler.JobScheduler(Inno.Core.Jobs.JobExecutionMode mode, Inno.Core.Jobs.JobSchedulerOptions options = default(Inno.Core.Jobs.JobSchedulerOptions))`](../../src/foundation/core/Inno.Core.Jobs/JobScheduler.cs#L24) | Creates a scheduler using the requested execution strategy.<br>`mode`：The execution strategy owned by the scheduler.<br>`options`：Worker-pool settings used when mode is Inno.Core.Jobs.JobExecutionMode.WorkerPool. | — | `System.ArgumentOutOfRangeException`：Thrown when mode is not a defined execution strategy. |
| <a id="api-a768bc928077c5fc"></a>[`public void Inno.Core.Jobs.JobScheduler.BeginFrame()`](../../src/foundation/core/Inno.Core.Jobs/JobScheduler.cs#L49) | Opens a frame scheduling scope on the owner thread. | — | — |
| <a id="api-c6651ebef849a497"></a>[`public Inno.Core.Jobs.JobHandle Inno.Core.Jobs.JobScheduler.CombineDependencies(System.ReadOnlySpan<Inno.Core.Jobs.JobHandle> dependencies)`](../../src/foundation/core/Inno.Core.Jobs/JobScheduler.cs#L98) | Creates one handle that completes after every supplied dependency.<br>返回：A generation-safe combined handle.<br>`dependencies`：Handles combined by the returned synchronization point. | — | — |
| <a id="api-e3f1d38c683a7e04"></a>[`public void Inno.Core.Jobs.JobScheduler.Complete(Inno.Core.Jobs.JobHandle handle)`](../../src/foundation/core/Inno.Core.Jobs/JobScheduler.cs#L127) | Blocks the calling thread until one scheduled operation completes.<br>`handle`：The generation-safe operation handle to complete. | — | — |
| <a id="api-0c674e7e78f59f3f"></a>[`public void Inno.Core.Jobs.JobScheduler.CompleteAll(System.ReadOnlySpan<Inno.Core.Jobs.JobHandle> handles)`](../../src/foundation/core/Inno.Core.Jobs/JobScheduler.cs#L135) | Blocks the calling thread until every supplied operation completes.<br>`handles`：The generation-safe operation handles to complete. | — | — |
| <a id="api-54b89a02d7f222b2"></a>[`public void Inno.Core.Jobs.JobScheduler.Dispose()`](../../src/foundation/core/Inno.Core.Jobs/JobScheduler.cs#L153) | Stops worker threads and releases every pending scheduling resource. | — | — |
| <a id="api-879fa28224576fba"></a>[`public void Inno.Core.Jobs.JobScheduler.DrainMainThreadQueue()`](../../src/foundation/core/Inno.Core.Jobs/JobScheduler.cs#L148) | Attempts at most the configured drain budget from the current queue snapshot; reentrant posts wait for a later drain. | — | — |
| <a id="api-659cac4054b95feb"></a>[`public void Inno.Core.Jobs.JobScheduler.EndFrame()`](../../src/foundation/core/Inno.Core.Jobs/JobScheduler.cs#L54) | Completes every frame job and closes the current frame scheduling scope. | — | — |
| <a id="api-6ee25fa94f2a6e43"></a>[`public void Inno.Core.Jobs.JobScheduler.EnqueueMainThread(System.Action action)`](../../src/foundation/core/Inno.Core.Jobs/JobScheduler.cs#L143) | Enqueues an operation that must run on the scheduler owner thread.<br>`action`：The owner-thread operation. | — | — |
| <a id="api-b89348f47f74d661"></a>[`public Inno.Core.Jobs.JobHandle Inno.Core.Jobs.JobScheduler.ParallelFor(int length, int batchSize, System.Action<int, int> body)`](../../src/foundation/core/Inno.Core.Jobs/JobScheduler.cs#L115) | Schedules a range as independent contiguous batches.<br>返回：A handle that completes after every batch.<br>`length`：The exclusive upper bound of the complete range.<br>`batchSize`：The maximum number of indices in one batch.<br>`body`：The callback receiving each batch's inclusive start and exclusive end. | — | — |
| <a id="api-41a7be3f0941282a"></a>[`public Inno.Core.Jobs.JobHandle Inno.Core.Jobs.JobScheduler.Schedule(System.Action job)`](../../src/foundation/core/Inno.Core.Jobs/JobScheduler.cs#L65) | Schedules one parameterless job in the current frame.<br>返回：A generation-safe handle representing the scheduled operation.<br>`job`：The operation to execute once its implicit empty dependency set is ready. | — | — |
| <a id="api-4282136504070f2d"></a>[`public Inno.Core.Jobs.JobHandle Inno.Core.Jobs.JobScheduler.Schedule(System.Action<object?> job, object? state, System.ReadOnlySpan<Inno.Core.Jobs.JobHandle> dependencies)`](../../src/foundation/core/Inno.Core.Jobs/JobScheduler.cs#L82) | Schedules one stateful job after all supplied dependencies complete.<br>返回：A generation-safe handle representing the scheduled operation.<br>`job`：The operation receiving state.<br>`state`：Optional caller-owned state passed to the operation.<br>`dependencies`：Handles that must complete before the operation becomes runnable. | — | — |
| <a id="api-01dbe9402f6d21ac"></a>[`public Inno.Core.Jobs.JobSchedulerStatistics Inno.Core.Jobs.JobScheduler.statistics { get; }`](../../src/foundation/core/Inno.Core.Jobs/JobScheduler.cs#L44) | Gets detached admission and owner-thread queue measurements. | — | — |
| <a id="api-db839d608e66cdb9"></a>[`public int Inno.Core.Jobs.JobScheduler.workerCount { get; }`](../../src/foundation/core/Inno.Core.Jobs/JobScheduler.cs#L39) | Gets the number of background workers owned by this scheduler. | — | — |
| <a id="api-b6c95e46fd6c6bbe"></a>[`public sealed class Inno.Core.Jobs.JobScheduler : System.IDisposable`](../../src/foundation/core/Inno.Core.Jobs/JobScheduler.cs#L8) | Owns one isolated frame-scoped job scheduler and hides its concrete execution strategy. | — | — |

### `Inno.Core.Jobs.JobSchedulerOptions`

<!-- api: T:Inno.Core.Jobs.JobSchedulerOptions e0e12ad9e42889797a7b04bc9daba8cba1cccd3e5040206e2fdafcdad08076b6 -->
<!-- api: P:Inno.Core.Jobs.JobSchedulerOptions.workerCount e630e55ad3ebe79ab828987d92d738cbcde0a740a0253558df97a8f4f3dd339c -->
<!-- api: P:Inno.Core.Jobs.JobSchedulerOptions.maxFrameJobs 690c3e6087a24b53eef83bd4452f0459a72efc99957354684ea80f5e8cc4553a -->
<!-- api: P:Inno.Core.Jobs.JobSchedulerOptions.mainThreadCapacity ebb84a6b5c9c316227e34673f812178c186cbcbe022dc897a4751f24678cd7c6 -->
<!-- api: P:Inno.Core.Jobs.JobSchedulerOptions.mainThreadDrainBudget a6b7928e93c13b9b8cc775010218f25e5b84b1f5f2a46a74a0360788cf22d772 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8c9e8fcdd0478d17"></a>[`public int Inno.Core.Jobs.JobSchedulerOptions.mainThreadCapacity { get; init; }`](../../src/foundation/core/Inno.Core.Jobs/JobSchedulerOptions.cs#L26) | Gets main-thread callback capacity; zero selects 65536. | — | — |
| <a id="api-d58eaabda83e4804"></a>[`public int Inno.Core.Jobs.JobSchedulerOptions.mainThreadDrainBudget { get; init; }`](../../src/foundation/core/Inno.Core.Jobs/JobSchedulerOptions.cs#L31) | Gets the maximum callbacks attempted per drain; zero selects 4096. | — | — |
| <a id="api-05a79a3c11978049"></a>[`public int Inno.Core.Jobs.JobSchedulerOptions.maxFrameJobs { get; init; }`](../../src/foundation/core/Inno.Core.Jobs/JobSchedulerOptions.cs#L21) | Gets the maximum accepted jobs per frame; zero selects 65536. | — | — |
| <a id="api-6438f9d4033164af"></a>[`public int Inno.Core.Jobs.JobSchedulerOptions.workerCount { get; init; }`](../../src/foundation/core/Inno.Core.Jobs/JobSchedulerOptions.cs#L16) | Number of worker threads. Set to 0 to use max(1, Environment.ProcessorCount - 1). | — | — |
| <a id="api-bd4a1985aeaeb405"></a>[`public readonly struct Inno.Core.Jobs.JobSchedulerOptions`](../../src/foundation/core/Inno.Core.Jobs/JobSchedulerOptions.cs#L8) | Defines worker-pool options for one isolated Inno.Core.Jobs.JobScheduler. | — | — |

### `Inno.Core.Jobs.JobSchedulerStatistics`

<!-- api: T:Inno.Core.Jobs.JobSchedulerStatistics 21fb32821a63f03755168c0702342f20eaf66ea8452f88208d3b2579e4a98003 -->
<!-- api: P:Inno.Core.Jobs.JobSchedulerStatistics.frameJobs 89bdd0093825af975628cf07d6470042bf87618b089c011b21729f1ca38d41e0 -->
<!-- api: P:Inno.Core.Jobs.JobSchedulerStatistics.peakFrameJobs d703abab38daa97869bb06465a7e7ed2ca1bf7087df99363265d8aa7d487828d -->
<!-- api: P:Inno.Core.Jobs.JobSchedulerStatistics.rejectedJobs 50f9e2bff536e1bd72c67a76f07ae4509df2faf1769fe1fae4c9b3e52a1eff78 -->
<!-- api: P:Inno.Core.Jobs.JobSchedulerStatistics.mainThreadPending 53adca8dcafc8484d942ba03492aadf31def1262ca5bc9c32f4cf713231d3007 -->
<!-- api: P:Inno.Core.Jobs.JobSchedulerStatistics.mainThreadPeak 86e9fa93a22d82975b5986d7ab176e96e168f75a45a635da381652ada1bc3330 -->
<!-- api: P:Inno.Core.Jobs.JobSchedulerStatistics.mainThreadRejected 09803b8d9e2cd0359a124d4518d394320e656f4bc971ce4aeea8450d55ee7317 -->
<!-- api: P:Inno.Core.Jobs.JobSchedulerStatistics.mainThreadCanceled ec78a95b0f1a41003940658e893d2008bcc4f0069bc14c4193060afe84abb0ab -->
<!-- api: M:Inno.Core.Jobs.JobSchedulerStatistics.ToString 999a918237ab8e1dcb9d7aef2c078235a76289d2f43c2ad42b4d8cb12a77c7a9 -->
<!-- api: M:Inno.Core.Jobs.JobSchedulerStatistics.op_Inequality(Inno.Core.Jobs.JobSchedulerStatistics,Inno.Core.Jobs.JobSchedulerStatistics) 2f08fc8fe45aca826b1ce23aa1b6e0a4e400f3786b5799769c84a362620a0320 -->
<!-- api: M:Inno.Core.Jobs.JobSchedulerStatistics.op_Equality(Inno.Core.Jobs.JobSchedulerStatistics,Inno.Core.Jobs.JobSchedulerStatistics) 358eee3d3704375c3762c9b9534c4c279edb60ad551571c11f156c3f861fb2c8 -->
<!-- api: M:Inno.Core.Jobs.JobSchedulerStatistics.GetHashCode 0b5caf5746e0d6126fb7574deaa627171415f6d0009fb39397f3285c1bd61949 -->
<!-- api: M:Inno.Core.Jobs.JobSchedulerStatistics.Equals(System.Object) 6090832aee142e138f2708fca7c6a2af073d5ddc032e8a907567bfd300606cd5 -->
<!-- api: M:Inno.Core.Jobs.JobSchedulerStatistics.Equals(Inno.Core.Jobs.JobSchedulerStatistics) a0dda35f948e32fd419856e8c618d4c9900823535cad39ce8a4a4af2effc03d9 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-188e29fe98969383"></a>[`public bool Inno.Core.Jobs.JobSchedulerStatistics.Equals(Inno.Core.Jobs.JobSchedulerStatistics other)`](../../src/foundation/core/Inno.Core.Jobs/JobSchedulerStatistics.cs#L6) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-dc76fc85a2346763"></a>[`public override bool Inno.Core.Jobs.JobSchedulerStatistics.Equals(object obj)`](../../src/foundation/core/Inno.Core.Jobs/JobSchedulerStatistics.cs#L6) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-6f179ab2824699c4"></a>[`public override int Inno.Core.Jobs.JobSchedulerStatistics.GetHashCode()`](../../src/foundation/core/Inno.Core.Jobs/JobSchedulerStatistics.cs#L6) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-88ba43022b6d21a2"></a>[`public static bool Inno.Core.Jobs.JobSchedulerStatistics.operator ==(Inno.Core.Jobs.JobSchedulerStatistics left, Inno.Core.Jobs.JobSchedulerStatistics right)`](../../src/foundation/core/Inno.Core.Jobs/JobSchedulerStatistics.cs#L6) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-95009abe889562ee"></a>[`public static bool Inno.Core.Jobs.JobSchedulerStatistics.operator !=(Inno.Core.Jobs.JobSchedulerStatistics left, Inno.Core.Jobs.JobSchedulerStatistics right)`](../../src/foundation/core/Inno.Core.Jobs/JobSchedulerStatistics.cs#L6) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-c611ae2261378ddb"></a>[`public override string Inno.Core.Jobs.JobSchedulerStatistics.ToString()`](../../src/foundation/core/Inno.Core.Jobs/JobSchedulerStatistics.cs#L6) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-aff7e986b10f86f4"></a>[`public int Inno.Core.Jobs.JobSchedulerStatistics.frameJobs { get; init; }`](../../src/foundation/core/Inno.Core.Jobs/JobSchedulerStatistics.cs#L11) | Gets jobs admitted in the current frame, including completed jobs awaiting EndFrame. | — | — |
| <a id="api-7539b72d77f7c58c"></a>[`public long Inno.Core.Jobs.JobSchedulerStatistics.mainThreadCanceled { get; init; }`](../../src/foundation/core/Inno.Core.Jobs/JobSchedulerStatistics.cs#L35) | Gets accepted callbacks canceled before execution during shutdown. | — | — |
| <a id="api-4f3159036b903564"></a>[`public int Inno.Core.Jobs.JobSchedulerStatistics.mainThreadPeak { get; init; }`](../../src/foundation/core/Inno.Core.Jobs/JobSchedulerStatistics.cs#L27) | Gets the largest simultaneous owner-thread queue occupancy. | — | — |
| <a id="api-41dccec09176c673"></a>[`public int Inno.Core.Jobs.JobSchedulerStatistics.mainThreadPending { get; init; }`](../../src/foundation/core/Inno.Core.Jobs/JobSchedulerStatistics.cs#L23) | Gets queued or retirement-pending owner-thread callbacks. | — | — |
| <a id="api-224abe4984a0ded1"></a>[`public long Inno.Core.Jobs.JobSchedulerStatistics.mainThreadRejected { get; init; }`](../../src/foundation/core/Inno.Core.Jobs/JobSchedulerStatistics.cs#L31) | Gets callbacks rejected because the queue was full or closed. | — | — |
| <a id="api-333bb346b5814c87"></a>[`public int Inno.Core.Jobs.JobSchedulerStatistics.peakFrameJobs { get; init; }`](../../src/foundation/core/Inno.Core.Jobs/JobSchedulerStatistics.cs#L15) | Gets the largest admitted frame since scheduler creation. | — | — |
| <a id="api-fa6b7be801699372"></a>[`public long Inno.Core.Jobs.JobSchedulerStatistics.rejectedJobs { get; init; }`](../../src/foundation/core/Inno.Core.Jobs/JobSchedulerStatistics.cs#L19) | Gets rejected job admissions caused by the finite frame capacity. | — | — |
| <a id="api-d9b1f213435f4b15"></a>[`public readonly record struct Inno.Core.Jobs.JobSchedulerStatistics : System.IEquatable<Inno.Core.Jobs.JobSchedulerStatistics>`](../../src/foundation/core/Inno.Core.Jobs/JobSchedulerStatistics.cs#L6) | Reports lifetime admission and bounded owner-thread queue measurements without retaining job delegates. | — | — |

## 项目依赖

- [Inno.Core.Execution](Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
