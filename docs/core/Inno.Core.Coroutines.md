# Inno.Core.Coroutines

[上一页：Events](Inno.Core.Events.md) · [Core 索引](README.md) · [下一页：Job](Inno.Core.Jobs.md)

`CoroutineScheduler` 执行 `IEnumerator` 协程，支持嵌套 enumerator、帧/时间/Task/条件等待以及 owner 级批量停止。普通 Start/Stop 请求通过线程安全命令队列进入 Scheduler，实际状态变化在 `Tick` 安全点应用；owner 级停止会取得 Tick gate，并在返回前立即移除匹配状态。

## CoroutineScheduler

| API | 说明 |
| --- | --- |
| `StartCoroutine(IEnumerator)` | 启动无 owner 协程，返回 handle。 |
| `StartCoroutine(object? owner, IEnumerator)` | 用引用身份保存 owner，便于批量停止。 |
| `StopCoroutine(CoroutineHandle)` | 请求停止；handle 不属于当前 scheduler 或已失效时返回 `false`。 |
| `StopAllCoroutines(object owner)` | 同步停止与该 owner `ReferenceEquals` 的全部协程；返回时 handle 已失效，Scheduler 不再持有 owner/enumerator。 |
| `StopAllCoroutines()` | 请求停止全部协程。 |
| `Tick(float deltaTime)` | 推进一帧；负 delta 当作 0。 |
| `Dispose()` | 清空全部 active/pending 状态并永久关闭。 |

`CoroutineHandle.isValid` 表示 handle 仍指向一个存活 scheduler 中的 live coroutine。Handle 是 opaque readonly struct，不公开内部 ID。

```csharp
IEnumerator Fade()
{
    yield return new WaitForFrames(1);
    yield return new WaitForSeconds(0.25f);
    yield return new WaitUntil(() => resourceReady);
    yield return LoadAsync();       // Nested IEnumerator is supported.
    yield return new WaitForTask(task);
}

CoroutineHandle handle = scheduler.StartCoroutine(owner: this, Fade());
scheduler.Tick(deltaTime);
```

## YieldInstruction

`YieldInstruction` 是公开抽象基类，但 waiter 创建由 Scheduler 内部控制。内置指令：

| 类型 | 公开属性 | 恢复条件 |
| --- | --- | --- |
| `WaitForFrames(int)` | `frames` | 经过指定帧数；`<= 0` 仍至少等到下一帧。 |
| `WaitForSeconds(float)` | `seconds` | Scheduler 累计时间达到目标；`<= 0` 等下一帧。 |
| `WaitForTask(Task)` | `task` | `Task.IsCompleted`；构造时不接受 null。 |
| `WaitUntil(Func<bool>)` | `predicate` | predicate 返回 `true`。 |
| `WaitWhile(Func<bool>)` | `predicate` | predicate 返回 `false`。 |

协程可 `yield return null` 表示等待下一次 Tick，也可 yield 另一个 `IEnumerator` 形成嵌套栈。

## Owner 与热重载

Owner 和 IEnumerator 都会强引用脚本实例及其程序集。脚本代际退出时，应在迁移前调用 `StopAllCoroutines(oldInstance)`；该调用返回时 Scheduler 已释放匹配的 owner/enumerator，因此不依赖下一帧 Tick 才允许旧 collectible ALC 回收。不要用值相等但引用不同的对象去停止 owner 协程。

## 线程与异常

- Start/Stop API 可从其他线程提交；`Tick` 自身由 gate 串行化。
- Scheduler 设计为由主循环每帧推进，不会自行创建更新线程。
- Enumerator/predicate 抛出的异常会从 `Tick` 传播；调用层应决定日志与故障策略。
- Dispose 后 `StartCoroutine`、owner stop 和 `Tick` 会抛 `ObjectDisposedException`；无参 stop-all 安全返回。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Core.Coroutines.CoroutineHandle`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Coroutines.CoroutineHandle`](../../src/foundation/core/Inno.Core.Coroutines/CoroutineHandle.cs#L10) | Opaque handle returned by or . |
| [`bool Inno.Core.Coroutines.CoroutineHandle.isValid`](../../src/foundation/core/Inno.Core.Coroutines/CoroutineHandle.cs#L29) | Gets whether this handle currently points to a live coroutine in a still-alive scheduler. |

### `Inno.Core.Coroutines.CoroutineScheduler`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Coroutines.CoroutineHandle Inno.Core.Coroutines.CoroutineScheduler.StartCoroutine(System.Collections.IEnumerator routine)`](../../src/foundation/core/Inno.Core.Coroutines/CoroutineScheduler.cs#L43) | Starts a coroutine. |
| [`Inno.Core.Coroutines.CoroutineHandle Inno.Core.Coroutines.CoroutineScheduler.StartCoroutine(object? owner, System.Collections.IEnumerator routine)`](../../src/foundation/core/Inno.Core.Coroutines/CoroutineScheduler.cs#L60) | Starts a coroutine with an owner token. |
| [`Inno.Core.Coroutines.CoroutineScheduler`](../../src/foundation/core/Inno.Core.Coroutines/CoroutineScheduler.cs#L13) | Thread-safe coroutine scheduler with start/stop APIs. |
| [`Inno.Core.Coroutines.CoroutineScheduler.CoroutineScheduler()`](../../src/foundation/core/Inno.Core.Coroutines/CoroutineScheduler.cs#L29) | Creates an empty scheduler whose lifetime is owned by the caller. |
| [`bool Inno.Core.Coroutines.CoroutineScheduler.StopCoroutine(Inno.Core.Coroutines.CoroutineHandle handle)`](../../src/foundation/core/Inno.Core.Coroutines/CoroutineScheduler.cs#L86) | Requests stopping a coroutine by handle. |
| [`void Inno.Core.Coroutines.CoroutineScheduler.Dispose()`](../../src/foundation/core/Inno.Core.Coroutines/CoroutineScheduler.cs#L131) | Disposes the scheduler and clears all active/pending coroutines. |
| [`void Inno.Core.Coroutines.CoroutineScheduler.StopAllCoroutines()`](../../src/foundation/core/Inno.Core.Coroutines/CoroutineScheduler.cs#L118) | Requests stopping all active coroutines. |
| [`void Inno.Core.Coroutines.CoroutineScheduler.StopAllCoroutines(object owner)`](../../src/foundation/core/Inno.Core.Coroutines/CoroutineScheduler.cs#L103) | Stops all coroutines owned by the specified owner token before this method returns. |
| [`void Inno.Core.Coroutines.CoroutineScheduler.Tick(float deltaTime)`](../../src/foundation/core/Inno.Core.Coroutines/CoroutineScheduler.cs#L159) | Advances coroutine execution by one frame. |

### `Inno.Core.Coroutines.WaitForFrames`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Coroutines.WaitForFrames`](../../src/foundation/core/Inno.Core.Coroutines/YieldInstructions/WaitForFrames.cs#L9) | Waits for a fixed number of frames. |
| [`int Inno.Core.Coroutines.WaitForFrames.frames`](../../src/foundation/core/Inno.Core.Coroutines/YieldInstructions/WaitForFrames.cs#L14) | Gets the configured frame count. |

### `Inno.Core.Coroutines.WaitForSeconds`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Coroutines.WaitForSeconds`](../../src/foundation/core/Inno.Core.Coroutines/YieldInstructions/WaitForSeconds.cs#L9) | Waits for the specified number of scaled seconds. |
| [`float Inno.Core.Coroutines.WaitForSeconds.seconds`](../../src/foundation/core/Inno.Core.Coroutines/YieldInstructions/WaitForSeconds.cs#L14) | Gets the configured wait time in seconds. |

### `Inno.Core.Coroutines.WaitForTask`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Coroutines.WaitForTask`](../../src/foundation/core/Inno.Core.Coroutines/YieldInstructions/WaitForTask.cs#L12) | Waits until a task is completed. |
| [`System.Threading.Tasks.Task Inno.Core.Coroutines.WaitForTask.task`](../../src/foundation/core/Inno.Core.Coroutines/YieldInstructions/WaitForTask.cs#L17) | Gets the awaited task. |

### `Inno.Core.Coroutines.WaitUntil`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Coroutines.WaitUntil`](../../src/foundation/core/Inno.Core.Coroutines/YieldInstructions/WaitUntil.cs#L11) | Waits until the predicate returns . |
| [`System.Func<bool> Inno.Core.Coroutines.WaitUntil.predicate`](../../src/foundation/core/Inno.Core.Coroutines/YieldInstructions/WaitUntil.cs#L16) | Gets the predicate evaluated each tick. |

### `Inno.Core.Coroutines.WaitWhile`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Coroutines.WaitWhile`](../../src/foundation/core/Inno.Core.Coroutines/YieldInstructions/WaitWhile.cs#L11) | Waits while the predicate returns . |
| [`System.Func<bool> Inno.Core.Coroutines.WaitWhile.predicate`](../../src/foundation/core/Inno.Core.Coroutines/YieldInstructions/WaitWhile.cs#L16) | Gets the predicate evaluated each tick. |

### `Inno.Core.Coroutines.YieldInstruction`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Coroutines.YieldInstruction`](../../src/foundation/core/Inno.Core.Coroutines/YieldInstructions/YieldInstruction.cs#L8) | Base type for custom coroutine wait instructions. |

## 项目依赖

- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
