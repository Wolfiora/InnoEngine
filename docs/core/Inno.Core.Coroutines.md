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
using System;
using System.Collections;
using System.Threading.Tasks;
using Inno.Core.Coroutines;

static IEnumerator Fade(
    Func<bool> resourceReady,
    IEnumerator load,
    Task task
) {
    yield return new WaitForFrames(1);
    yield return new WaitForSeconds(0.25f);
    yield return new WaitUntil(resourceReady);
    yield return load;
    yield return new WaitForTask(task);
}

static CoroutineHandle StartFade(
    CoroutineScheduler scheduler,
    object owner,
    Func<bool> resourceReady,
    IEnumerator load,
    Task task
) {
    return scheduler.StartCoroutine(owner, Fade(resourceReady, load, task));
}
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

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Core.Coroutines.CoroutineHandle`

<!-- api: T:Inno.Core.Coroutines.CoroutineHandle 075c1cead38fad98e070e57d2bd6f9c1a2aec62bbf815698865f38f771e1dfa5 -->
<!-- api: P:Inno.Core.Coroutines.CoroutineHandle.isValid 3770c2b952c6ed20f2e31111a7bf9b7f908132e32371ac60bc883c1a30ff56c4 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-462163c64e2891a6"></a>[`public bool Inno.Core.Coroutines.CoroutineHandle.isValid { get; }`](../../src/foundation/core/Inno.Core.Coroutines/CoroutineHandle.cs#L29) | Gets whether this handle currently points to a live coroutine in a still-alive scheduler. | — | — |
| <a id="api-9c47ff402c5c8640"></a>[`public readonly struct Inno.Core.Coroutines.CoroutineHandle`](../../src/foundation/core/Inno.Core.Coroutines/CoroutineHandle.cs#L10) | Opaque handle returned by Inno.Core.Coroutines.CoroutineScheduler.StartCoroutine(System.Collections.IEnumerator) or Inno.Core.Coroutines.CoroutineScheduler.StartCoroutine(System.Object,System.Collections.IEnumerator). | — | — |

### `Inno.Core.Coroutines.CoroutineScheduler`

<!-- api: T:Inno.Core.Coroutines.CoroutineScheduler 22dbfed8add0586b2823c9267edb2f152a276850caa0db6e197d3449e7c4d1a3 -->
<!-- api: M:Inno.Core.Coroutines.CoroutineScheduler.#ctor 1a553522eefa6ce95f6e4ff77ed4abed3a4ceed9637163d2354baab4a657cafe -->
<!-- api: M:Inno.Core.Coroutines.CoroutineScheduler.StartCoroutine(System.Collections.IEnumerator) 9ba3ba04c19dc2b45deaf2af0f86e2a9d685bc61f692adbff88dff187f749c8e -->
<!-- api: M:Inno.Core.Coroutines.CoroutineScheduler.StartCoroutine(System.Object,System.Collections.IEnumerator) 042eecf8f8ed52a907f795e61f64c0c1bf3bf6487c0f90067bb42fd53a82dcc6 -->
<!-- api: M:Inno.Core.Coroutines.CoroutineScheduler.StopCoroutine(Inno.Core.Coroutines.CoroutineHandle) c4e6757749a096db87c80d719d2769b9f561bd8dce975d58b4b4dd24228eb4b3 -->
<!-- api: M:Inno.Core.Coroutines.CoroutineScheduler.StopAllCoroutines(System.Object) edee117a0651204bbefbe5337276f85e537c1542df08985e898610a85fe65e5e -->
<!-- api: M:Inno.Core.Coroutines.CoroutineScheduler.StopAllCoroutines 1cffd6c7a89ecb50c3528b8f51af80994c4d2bf91ae8254423acd07e79b1f594 -->
<!-- api: M:Inno.Core.Coroutines.CoroutineScheduler.Dispose b863d71ff70b118c5ea13e58db9b01750c1ad267cc263f5d0536489215d8fc9b -->
<!-- api: M:Inno.Core.Coroutines.CoroutineScheduler.Tick(System.Single) a6bcd3a0cbc095ec9f059cfc5508caad23d2963f985ddd243849ba1ee457dc21 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9153c4e9ba02858d"></a>[`public Inno.Core.Coroutines.CoroutineScheduler.CoroutineScheduler()`](../../src/foundation/core/Inno.Core.Coroutines/CoroutineScheduler.cs#L29) | Creates an empty scheduler whose lifetime is owned by the caller. | — | — |
| <a id="api-d6149e4c9e5598b9"></a>[`public void Inno.Core.Coroutines.CoroutineScheduler.Dispose()`](../../src/foundation/core/Inno.Core.Coroutines/CoroutineScheduler.cs#L131) | Disposes the scheduler and clears all active/pending coroutines. | — | — |
| <a id="api-40d0b77365ef11bd"></a>[`public Inno.Core.Coroutines.CoroutineHandle Inno.Core.Coroutines.CoroutineScheduler.StartCoroutine(System.Collections.IEnumerator routine)`](../../src/foundation/core/Inno.Core.Coroutines/CoroutineScheduler.cs#L43) | Starts a coroutine.<br>返回：Coroutine handle.<br>`routine`：Coroutine enumerator. | — | — |
| <a id="api-6be63970b2fcdd84"></a>[`public Inno.Core.Coroutines.CoroutineHandle Inno.Core.Coroutines.CoroutineScheduler.StartCoroutine(object? owner, System.Collections.IEnumerator routine)`](../../src/foundation/core/Inno.Core.Coroutines/CoroutineScheduler.cs#L60) | Starts a coroutine with an owner token.<br>返回：Coroutine handle.<br>`owner`：Owner key used by Inno.Core.Coroutines.CoroutineScheduler.StopAllCoroutines(System.Object).<br>`routine`：Coroutine enumerator. | — | — |
| <a id="api-5a1ef857eb528486"></a>[`public void Inno.Core.Coroutines.CoroutineScheduler.StopAllCoroutines()`](../../src/foundation/core/Inno.Core.Coroutines/CoroutineScheduler.cs#L118) | Requests stopping all active coroutines. | — | — |
| <a id="api-e3ae0402198b5654"></a>[`public void Inno.Core.Coroutines.CoroutineScheduler.StopAllCoroutines(object owner)`](../../src/foundation/core/Inno.Core.Coroutines/CoroutineScheduler.cs#L103) | Stops all coroutines owned by the specified owner token before this method returns.<br>`owner`：Owner key used when starting coroutines. | — | — |
| <a id="api-c0115c95f7a3d703"></a>[`public bool Inno.Core.Coroutines.CoroutineScheduler.StopCoroutine(Inno.Core.Coroutines.CoroutineHandle handle)`](../../src/foundation/core/Inno.Core.Coroutines/CoroutineScheduler.cs#L86) | Requests stopping a coroutine by handle.<br>返回：true if the handle is valid.<br>`handle`：Target coroutine handle. | — | — |
| <a id="api-6949bb654af677e7"></a>[`public void Inno.Core.Coroutines.CoroutineScheduler.Tick(float deltaTime)`](../../src/foundation/core/Inno.Core.Coroutines/CoroutineScheduler.cs#L159) | Advances coroutine execution by one frame.<br>`deltaTime`：Frame delta time in seconds. | — | — |
| <a id="api-44f36c50a6bd55b2"></a>[`public sealed class Inno.Core.Coroutines.CoroutineScheduler : System.IDisposable`](../../src/foundation/core/Inno.Core.Coroutines/CoroutineScheduler.cs#L13) | Thread-safe coroutine scheduler with start/stop APIs. | — | — |

### `Inno.Core.Coroutines.WaitForFrames`

<!-- api: T:Inno.Core.Coroutines.WaitForFrames 502ba5cbfa62beae5f091f9204c9c746d2ee01a3ddc2bbeded6697aec38cc272 -->
<!-- api: M:Inno.Core.Coroutines.WaitForFrames.#ctor(System.Int32) 1f7c3fcfcbf9838d999bcb866a5f7e01290e414b52ea5c9e2d7ea86d0f78e406 -->
<!-- api: P:Inno.Core.Coroutines.WaitForFrames.frames d6764184eabaf45a3bfab59f9469bc1b53340f993f2e0296027c330a44bbba34 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-ef2209b8b2d28bc9"></a>[`public Inno.Core.Coroutines.WaitForFrames.WaitForFrames(int frames)`](../../src/foundation/core/Inno.Core.Coroutines/YieldInstructions/WaitForFrames.cs#L9) | Waits for a fixed number of frames.<br>`frames`：The frames used to initialize this instance. | — | — |
| <a id="api-3eecbdde92bcc2e1"></a>[`public int Inno.Core.Coroutines.WaitForFrames.frames { get; }`](../../src/foundation/core/Inno.Core.Coroutines/YieldInstructions/WaitForFrames.cs#L14) | Gets the configured frame count. | — | — |
| <a id="api-d29db1a074e6f9ee"></a>[`public sealed class Inno.Core.Coroutines.WaitForFrames : Inno.Core.Coroutines.YieldInstruction`](../../src/foundation/core/Inno.Core.Coroutines/YieldInstructions/WaitForFrames.cs#L9) | Waits for a fixed number of frames.<br>`frames`：The frames used to initialize this instance. | — | — |

### `Inno.Core.Coroutines.WaitForSeconds`

<!-- api: T:Inno.Core.Coroutines.WaitForSeconds 4bbaaa68fc69280642c9bc465ac84db6ff7940da1ff9d03368c03f67fabb8d75 -->
<!-- api: M:Inno.Core.Coroutines.WaitForSeconds.#ctor(System.Single) 6d17e5a1c0f3da38e0117b5e09be7b1fd6847908c629a888aa83752ab231a07f -->
<!-- api: P:Inno.Core.Coroutines.WaitForSeconds.seconds 0aa07e1e70f57bd4e7af7256b923453687bd723f6425343d1cf33e95d304a841 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-5c970c58bf80efa1"></a>[`public Inno.Core.Coroutines.WaitForSeconds.WaitForSeconds(float seconds)`](../../src/foundation/core/Inno.Core.Coroutines/YieldInstructions/WaitForSeconds.cs#L9) | Waits for the specified number of scaled seconds.<br>`seconds`：The seconds used to initialize this instance. | — | — |
| <a id="api-9d91d6f9365553a1"></a>[`public float Inno.Core.Coroutines.WaitForSeconds.seconds { get; }`](../../src/foundation/core/Inno.Core.Coroutines/YieldInstructions/WaitForSeconds.cs#L14) | Gets the configured wait time in seconds. | — | — |
| <a id="api-7b1f6d5740602968"></a>[`public sealed class Inno.Core.Coroutines.WaitForSeconds : Inno.Core.Coroutines.YieldInstruction`](../../src/foundation/core/Inno.Core.Coroutines/YieldInstructions/WaitForSeconds.cs#L9) | Waits for the specified number of scaled seconds.<br>`seconds`：The seconds used to initialize this instance. | — | — |

### `Inno.Core.Coroutines.WaitForTask`

<!-- api: T:Inno.Core.Coroutines.WaitForTask 1006b820c8c1e4af7cf0f5627d2abe3490e199b928738cc7a12f7797ecf97325 -->
<!-- api: M:Inno.Core.Coroutines.WaitForTask.#ctor(System.Threading.Tasks.Task) 1ed1ad00fd4523b2d5ee9d2f4d72bc5efede6dba7ad423f406f51ed0c34b8e0c -->
<!-- api: P:Inno.Core.Coroutines.WaitForTask.task 789c06112731b402f9304f977212951dc6b060924afe7de880b6f3eaa0a6ad0b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-72486a65d8548948"></a>[`public Inno.Core.Coroutines.WaitForTask.WaitForTask(System.Threading.Tasks.Task task)`](../../src/foundation/core/Inno.Core.Coroutines/YieldInstructions/WaitForTask.cs#L12) | Waits until a task is completed.<br>`task`：The task used to initialize this instance. | — | — |
| <a id="api-be17e3c47851e637"></a>[`public System.Threading.Tasks.Task Inno.Core.Coroutines.WaitForTask.task { get; }`](../../src/foundation/core/Inno.Core.Coroutines/YieldInstructions/WaitForTask.cs#L17) | Gets the awaited task. | — | — |
| <a id="api-2abdec6e7ff2921c"></a>[`public sealed class Inno.Core.Coroutines.WaitForTask : Inno.Core.Coroutines.YieldInstruction`](../../src/foundation/core/Inno.Core.Coroutines/YieldInstructions/WaitForTask.cs#L12) | Waits until a task is completed.<br>`task`：The task used to initialize this instance. | — | — |

### `Inno.Core.Coroutines.WaitUntil`

<!-- api: T:Inno.Core.Coroutines.WaitUntil 2387c4e36d3296fddb1bd5ec112d85bf5c8563881b4ddf404b9cc7a57afd700b -->
<!-- api: M:Inno.Core.Coroutines.WaitUntil.#ctor(System.Func{System.Boolean}) cdc4c1bfa65fdd3eb4d59e7b77551600cc9ee79434d05a7af164175132112837 -->
<!-- api: P:Inno.Core.Coroutines.WaitUntil.predicate 4cc1e18a4a21c8c33f2d89d5d0aab438b87e1d3a0ace0b7350d708e48171cab1 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b279b81ef962dbda"></a>[`public Inno.Core.Coroutines.WaitUntil.WaitUntil(System.Func<bool> predicate)`](../../src/foundation/core/Inno.Core.Coroutines/YieldInstructions/WaitUntil.cs#L11) | Waits until the predicate returns true.<br>`predicate`：The predicate used to initialize this instance. | — | — |
| <a id="api-a41f284a45bc68de"></a>[`public System.Func<bool> Inno.Core.Coroutines.WaitUntil.predicate { get; }`](../../src/foundation/core/Inno.Core.Coroutines/YieldInstructions/WaitUntil.cs#L16) | Gets the predicate evaluated each tick. | — | — |
| <a id="api-cfdeeefa424095c4"></a>[`public sealed class Inno.Core.Coroutines.WaitUntil : Inno.Core.Coroutines.YieldInstruction`](../../src/foundation/core/Inno.Core.Coroutines/YieldInstructions/WaitUntil.cs#L11) | Waits until the predicate returns true.<br>`predicate`：The predicate used to initialize this instance. | — | — |

### `Inno.Core.Coroutines.WaitWhile`

<!-- api: T:Inno.Core.Coroutines.WaitWhile a6e2cb03589d417c355de3659515e263f95b5d652245211f47212ffb204bde4c -->
<!-- api: M:Inno.Core.Coroutines.WaitWhile.#ctor(System.Func{System.Boolean}) 4dd6ec914dd482c792239b58f89cea64198b276a43149b042f1c2c6ad9e1700e -->
<!-- api: P:Inno.Core.Coroutines.WaitWhile.predicate ffc85d708cf1ddf4089c8da33eeafdd9dee571ddb46a977cbe106a73817e8bdc -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-696d105567d30470"></a>[`public Inno.Core.Coroutines.WaitWhile.WaitWhile(System.Func<bool> predicate)`](../../src/foundation/core/Inno.Core.Coroutines/YieldInstructions/WaitWhile.cs#L11) | Waits while the predicate returns true.<br>`predicate`：The predicate used to initialize this instance. | — | — |
| <a id="api-f8d60a9b993094a0"></a>[`public System.Func<bool> Inno.Core.Coroutines.WaitWhile.predicate { get; }`](../../src/foundation/core/Inno.Core.Coroutines/YieldInstructions/WaitWhile.cs#L16) | Gets the predicate evaluated each tick. | — | — |
| <a id="api-56c93aa091fd6687"></a>[`public sealed class Inno.Core.Coroutines.WaitWhile : Inno.Core.Coroutines.YieldInstruction`](../../src/foundation/core/Inno.Core.Coroutines/YieldInstructions/WaitWhile.cs#L11) | Waits while the predicate returns true.<br>`predicate`：The predicate used to initialize this instance. | — | — |

### `Inno.Core.Coroutines.YieldInstruction`

<!-- api: T:Inno.Core.Coroutines.YieldInstruction 0a41008cc97d1c1076aec0f2f1706653c468225442b1de418cbbb1db66424dde -->
<!-- api: M:Inno.Core.Coroutines.YieldInstruction.#ctor da94412f5a4434c862d42a35b52cfe5fd967468f9e3c9e8ee66f3aa5c4897410 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a141ea787b961743"></a>[`protected Inno.Core.Coroutines.YieldInstruction.YieldInstruction()`](../../src/foundation/core/Inno.Core.Coroutines/YieldInstructions/YieldInstruction.cs#L8) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-a46d7a711104ae23"></a>[`public abstract class Inno.Core.Coroutines.YieldInstruction`](../../src/foundation/core/Inno.Core.Coroutines/YieldInstructions/YieldInstruction.cs#L8) | Base type for custom coroutine wait instructions. | — | — |

## 项目依赖

- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
