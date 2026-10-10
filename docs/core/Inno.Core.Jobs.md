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

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Core.Jobs.JobExecutionMode`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Jobs.JobExecutionMode`](../../src/foundation/core/Inno.Core.Jobs/JobExecutionMode.cs#L6) | Selects the execution strategy owned by a job scheduler. |
| [`Inno.Core.Jobs.JobExecutionMode.SingleThread`](../../src/foundation/core/Inno.Core.Jobs/JobExecutionMode.cs#L11) | Executes jobs deterministically on the scheduler owner thread. |
| [`Inno.Core.Jobs.JobExecutionMode.WorkerPool`](../../src/foundation/core/Inno.Core.Jobs/JobExecutionMode.cs#L16) | Executes ready jobs on a bounded work-stealing worker pool. |

### `Inno.Core.Jobs.JobHandle`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Jobs.JobHandle`](../../src/foundation/core/Inno.Core.Jobs/JobHandle.cs#L6) | Opaque handle representing a scheduled job. |
| [`bool Inno.Core.Jobs.JobHandle.isValid`](../../src/foundation/core/Inno.Core.Jobs/JobHandle.cs#L22) | Gets whether this handle contains a valid identifier. |

### `Inno.Core.Jobs.JobScheduler`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Jobs.JobHandle Inno.Core.Jobs.JobScheduler.CombineDependencies(System.ReadOnlySpan<Inno.Core.Jobs.JobHandle> dependencies)`](../../src/foundation/core/Inno.Core.Jobs/JobScheduler.cs#L98) | Creates one handle that completes after every supplied dependency. |
| [`Inno.Core.Jobs.JobHandle Inno.Core.Jobs.JobScheduler.ParallelFor(int length, int batchSize, System.Action<int, int> body)`](../../src/foundation/core/Inno.Core.Jobs/JobScheduler.cs#L115) | Schedules a range as independent contiguous batches. |
| [`Inno.Core.Jobs.JobHandle Inno.Core.Jobs.JobScheduler.Schedule(System.Action job)`](../../src/foundation/core/Inno.Core.Jobs/JobScheduler.cs#L65) | Schedules one parameterless job in the current frame. |
| [`Inno.Core.Jobs.JobHandle Inno.Core.Jobs.JobScheduler.Schedule(System.Action<object?> job, object? state, System.ReadOnlySpan<Inno.Core.Jobs.JobHandle> dependencies)`](../../src/foundation/core/Inno.Core.Jobs/JobScheduler.cs#L82) | Schedules one stateful job after all supplied dependencies complete. |
| [`Inno.Core.Jobs.JobScheduler`](../../src/foundation/core/Inno.Core.Jobs/JobScheduler.cs#L8) | Owns one isolated frame-scoped job scheduler and hides its concrete execution strategy. |
| [`Inno.Core.Jobs.JobScheduler.JobScheduler(Inno.Core.Jobs.JobExecutionMode mode, Inno.Core.Jobs.JobSchedulerOptions options = default(Inno.Core.Jobs.JobSchedulerOptions))`](../../src/foundation/core/Inno.Core.Jobs/JobScheduler.cs#L24) | Creates a scheduler using the requested execution strategy. |
| [`Inno.Core.Jobs.JobSchedulerStatistics Inno.Core.Jobs.JobScheduler.statistics`](../../src/foundation/core/Inno.Core.Jobs/JobScheduler.cs#L44) | Gets detached admission and owner-thread queue measurements. |
| [`int Inno.Core.Jobs.JobScheduler.workerCount`](../../src/foundation/core/Inno.Core.Jobs/JobScheduler.cs#L39) | Gets the number of background workers owned by this scheduler. |
| [`void Inno.Core.Jobs.JobScheduler.BeginFrame()`](../../src/foundation/core/Inno.Core.Jobs/JobScheduler.cs#L49) | Opens a frame scheduling scope on the owner thread. |
| [`void Inno.Core.Jobs.JobScheduler.Complete(Inno.Core.Jobs.JobHandle handle)`](../../src/foundation/core/Inno.Core.Jobs/JobScheduler.cs#L127) | Blocks the calling thread until one scheduled operation completes. |
| [`void Inno.Core.Jobs.JobScheduler.CompleteAll(System.ReadOnlySpan<Inno.Core.Jobs.JobHandle> handles)`](../../src/foundation/core/Inno.Core.Jobs/JobScheduler.cs#L135) | Blocks the calling thread until every supplied operation completes. |
| [`void Inno.Core.Jobs.JobScheduler.Dispose()`](../../src/foundation/core/Inno.Core.Jobs/JobScheduler.cs#L153) | Stops worker threads and releases every pending scheduling resource. |
| [`void Inno.Core.Jobs.JobScheduler.DrainMainThreadQueue()`](../../src/foundation/core/Inno.Core.Jobs/JobScheduler.cs#L148) | Attempts at most the configured drain budget from the current queue snapshot; reentrant posts wait for a later drain. |
| [`void Inno.Core.Jobs.JobScheduler.EndFrame()`](../../src/foundation/core/Inno.Core.Jobs/JobScheduler.cs#L54) | Completes every frame job and closes the current frame scheduling scope. |
| [`void Inno.Core.Jobs.JobScheduler.EnqueueMainThread(System.Action action)`](../../src/foundation/core/Inno.Core.Jobs/JobScheduler.cs#L143) | Enqueues an operation that must run on the scheduler owner thread. |

### `Inno.Core.Jobs.JobSchedulerOptions`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Jobs.JobSchedulerOptions`](../../src/foundation/core/Inno.Core.Jobs/JobSchedulerOptions.cs#L8) | Defines worker-pool options for one isolated . |
| [`int Inno.Core.Jobs.JobSchedulerOptions.mainThreadCapacity`](../../src/foundation/core/Inno.Core.Jobs/JobSchedulerOptions.cs#L26) | Gets main-thread callback capacity; zero selects 65536. |
| [`int Inno.Core.Jobs.JobSchedulerOptions.mainThreadDrainBudget`](../../src/foundation/core/Inno.Core.Jobs/JobSchedulerOptions.cs#L31) | Gets the maximum callbacks attempted per drain; zero selects 4096. |
| [`int Inno.Core.Jobs.JobSchedulerOptions.maxFrameJobs`](../../src/foundation/core/Inno.Core.Jobs/JobSchedulerOptions.cs#L21) | Gets the maximum accepted jobs per frame; zero selects 65536. |
| [`int Inno.Core.Jobs.JobSchedulerOptions.workerCount`](../../src/foundation/core/Inno.Core.Jobs/JobSchedulerOptions.cs#L16) | Number of worker threads. Set to 0 to use max(1, Environment.ProcessorCount - 1). |

### `Inno.Core.Jobs.JobSchedulerStatistics`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Jobs.JobSchedulerStatistics`](../../src/foundation/core/Inno.Core.Jobs/JobSchedulerStatistics.cs#L6) | Reports lifetime admission and bounded owner-thread queue measurements without retaining job delegates. |
| [`int Inno.Core.Jobs.JobSchedulerStatistics.frameJobs`](../../src/foundation/core/Inno.Core.Jobs/JobSchedulerStatistics.cs#L11) | Gets jobs admitted in the current frame, including completed jobs awaiting EndFrame. |
| [`int Inno.Core.Jobs.JobSchedulerStatistics.mainThreadPeak`](../../src/foundation/core/Inno.Core.Jobs/JobSchedulerStatistics.cs#L27) | Gets the largest simultaneous owner-thread queue occupancy. |
| [`int Inno.Core.Jobs.JobSchedulerStatistics.mainThreadPending`](../../src/foundation/core/Inno.Core.Jobs/JobSchedulerStatistics.cs#L23) | Gets queued or retirement-pending owner-thread callbacks. |
| [`int Inno.Core.Jobs.JobSchedulerStatistics.peakFrameJobs`](../../src/foundation/core/Inno.Core.Jobs/JobSchedulerStatistics.cs#L15) | Gets the largest admitted frame since scheduler creation. |
| [`long Inno.Core.Jobs.JobSchedulerStatistics.mainThreadCanceled`](../../src/foundation/core/Inno.Core.Jobs/JobSchedulerStatistics.cs#L35) | Gets accepted callbacks canceled before execution during shutdown. |
| [`long Inno.Core.Jobs.JobSchedulerStatistics.mainThreadRejected`](../../src/foundation/core/Inno.Core.Jobs/JobSchedulerStatistics.cs#L31) | Gets callbacks rejected because the queue was full or closed. |
| [`long Inno.Core.Jobs.JobSchedulerStatistics.rejectedJobs`](../../src/foundation/core/Inno.Core.Jobs/JobSchedulerStatistics.cs#L19) | Gets rejected job admissions caused by the finite frame capacity. |

## 项目依赖

- [Inno.Core.Execution](Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
