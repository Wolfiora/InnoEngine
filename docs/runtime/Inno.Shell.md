# Inno.Shell

## 原生实时缩放与帧率

Shell 在首次帧和 `framePacing.verticalSync` 发生变化时调用必需的 `IRenderDevice.SetVerticalSync`，
只有调用成功才更新已应用值；稳态帧不重复调用。普通帧与原生 resize 帧共享这一状态和设备帧边界。

`framePacing` 是每 Shell 的 `FramePacingOptions`：`verticalSync` 控制设备同步，`maximumFrameRate = 0` 不施加软件上限。Editor 默认关闭 VSync；该策略不改变 fixed-step simulation。

Shell 订阅平台的 `redrawRequested`。系统模态缩放阻塞普通 PollEvent 循环时，使用同一个时钟、帧号及 Begin/Update/LateUpdate/Render/End 生命周期渲染，不递归 PumpEvents。重入保护避免正在执行的帧再次进入。原生 callback 不允许托管异常穿越 ABI；SDL adapter 将失败留到受控事件循环中重新抛出。

Presentation adapter 只能同步尺寸并请求绘制，不能独自回放 Editor draw callback。这一约束确保 Game/Scene 请求在同一帧提交和消费，BGFX 帧推进及资源退休仍由正常 runtime owner 完成。

## 有界产品退休

Shell 使用 Core 的 `RetirementBarrier`，在 owner thread 排空产品资源后才释放 rendering/input/window/platform。
同一 barrier 的 deadline 不随 Dispose 重试重置；超时仍属于未退休状态，不能将其聚合成一般错误后继续释放 adapter。
Editor 产品先通过 Play Mode loop quiesce，随后才进入 Module/Layer 资源栈的拆卸。

## 退出时的异步工作

Shell 在控制线程重试 product retirement，最多等待 30 秒。收到 `RetirementPendingException` 时不会销毁窗口、Input 或 GPU 依赖；超过等待期限仍将原屏障异常向上传递，不宣称成功。正常错误清理继续聚合，且 diagnostic producer 必须活到它服务的 owner 退休之后。

[Runtime 索引](README.md) · [Adapter contract](Inno.Adapter.md) · [架构 Overview](../architecture/ENGINE_ARCHITECTURE_OVERVIEW.md)

`Inno.Shell` 是 Player 与 Editor 共用的 backend-neutral Composition Host 基类，不是全局 singleton 或 Service Locator。

## 公开与 protected API

- `ShellOptions`：Adapter selection、window 和中立 rendering policy。
- `ShellFrame`：frame index、total time 与 delta time 的不可变值。
- `Shell`：拥有 application、primary window、input event source、render device 与公共 run loop。
- `ShellState` / `Shell.state`：`Created → Ready → Running ↔ Suspended → Stopping → Stopped`；初始化、执行或退休失败进入 `Faulted`，成功释放进入 `Disposed`。
- `InitializeAdapterResources`：派生产品完成非窗口 bootstrap 后显式创建公共 Adapter 资源。
- `RunAsync(IShellFrameDriver, smokeFrameLimit, cancellationToken)` / `RequestExit`：执行一次事件与 frame 生命周期。
- `OnStarting`、`ShouldExit`、`OnEvent`、`OnSuspensionChanged`、`OnFrame`、`OnSmokeCompleted`、`OnStopping`：产品行为 hook。
- `DisposeProductResources`：在公共 Adapter 逆序销毁前释放产品资源。

```csharp
using Inno.Adapter;
using Inno.Shell;

internal sealed class GameHost : Shell
{
    internal GameHost(
        IAdapterCatalog adapters,
        ShellOptions options
    ) : base(adapters, options)
    {
        InitializeAdapterResources();
    }

    protected override void OnFrame(ShellFrame frame)
    {
        // Advance the product-owned runtime session here.
    }
}
```

一个 Shell 只能运行一次。初始化中途失败会回滚已创建的 render/input/window/application；Dispose 先调用产品释放，再按 render → input → window → platform 逆序清理，并聚合 cleanup exception。`GamePlayerHost` 与 `EditorHost` 都必须继承 Shell，Architecture Tool 会拒绝直接引用具体 adapter 的 Host。

`RunAsync(driver, smokeFrameLimit)` 只有实际完成的帧数达到所请求的上限，才调用 `OnSmokeCompleted`。

唯一循环入口为 `RunAsync(driver, smokeFrameLimit, cancellationToken)`。`IShellFrameDriver` 只提供 `allowsBlockingPacing` 和 `RunAsync(advanceFrame, cancellationToken)`，回调必须在窗口/渲染 owner thread 执行。`PollingShellFrameDriver` 同步循环并允许软件帧率等待；`ScheduledShellFrameDriver(nextFrame)` 等待外部绘制机会、不阻塞宿主。两者共享 Shell 的同一帧实现，停止、取消和 callback 失败都经过同一 stopping 通知与资源退休。

```csharp
using System.Threading;
using System.Threading.Tasks;
using Inno.Shell;

static Task RunPreparedShell(
    Shell shell,
    CancellationToken cancellationToken
) {
    return shell.RunAsync(new PollingShellFrameDriver(), smokeFrameLimit: 60, cancellationToken);
}
```

`shell` 是调用方已经初始化的派生 Shell。异步调度器构造函数拒绝 null；frame callback 的 false 结束调度，异常传播；取消检查在等待前与帧执行前，未达到 smoke 上限不能报告成功。

用户提前关闭窗口或产品 `RequestExit` 仍按正常生命周期清理，但不能打印 smoke 成功标记；验收调用方必须同时检查完成标记和退出码。

## 暂停与恢复

平台通过 Core Events 的 `ApplicationSuspensionChangedEvent(isSuspended)` 通知应用暂停；
`ShellOptions.suspendWhenHidden` 可以额外选择主窗口的 `WindowVisibilityChangedEvent` 策略。
两个原因独立保存，次窗口隐藏不改变主窗口策略，重复通知不会重复调用 hook。
暂停期间继续处理平台事件及退出请求，但不执行产品、presentation 或 resize redraw 帧。
Shell 时钟停止，恢复时不补跑后台停留时间；允许阻塞的轮询驱动在暂停时有限等待，避免忙循环。

暂停入口释放 Input backend 的 held 与 transient 状态，并拒绝后台输入。
`OnSuspensionChanged(bool)` 在 owner-thread event-pump 安全点执行，派生产品可用同一 Core Events
通知 Session 的领域服务。取消仍传播 `OperationCanceledException`，正常 stopping 后状态为 `Stopped`；
执行和 stopping 同时失败时保留两个异常。所有 run 结果都会退订平台 redraw callback。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Shell.IShellFrameDriver`

<!-- api: T:Inno.Shell.IShellFrameDriver 2991c79d8ed45aae5857f47c31019cc543c35cbe01bc687b0e9433d4aa61f938 -->
<!-- api: P:Inno.Shell.IShellFrameDriver.allowsBlockingPacing 049e38e6fb539fc13b7ed0e8061c9f98da35b7c0f5371557dc56dd81bbad04d8 -->
<!-- api: M:Inno.Shell.IShellFrameDriver.RunAsync(System.Func{System.Boolean},System.Threading.CancellationToken) b5c25d4365b477be661108837878742b5a86ff772e58aed1e269f6941b2eabdf -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f37c1b4fcffdda23"></a>[`System.Threading.Tasks.ValueTask Inno.Shell.IShellFrameDriver.RunAsync(System.Func<bool> advanceFrame, System.Threading.CancellationToken cancellationToken)`](../../src/composition/shell/Inno.Shell/IShellFrameDriver.cs#L29) | Schedules frames on the owning thread until a frame callback requests termination.<br>返回：Completion after scheduling ends; callback failures must propagate to the shell.<br>`advanceFrame`：Executes one frame and returns true when the host should schedule another frame.<br>`cancellationToken`：Requests termination before the next frame. | — | — |
| <a id="api-02830b7d2f6f49c6"></a>[`bool Inno.Shell.IShellFrameDriver.allowsBlockingPacing { get; }`](../../src/composition/shell/Inno.Shell/IShellFrameDriver.cs#L15) | Gets whether the shell may block the calling thread to enforce its frame rate. | — | — |
| <a id="api-d948dbe8c99e61ba"></a>[`public interface Inno.Shell.IShellFrameDriver`](../../src/composition/shell/Inno.Shell/IShellFrameDriver.cs#L10) | Gives a host ownership of frame scheduling while the shell owns frame execution and retirement. | — | — |

### `Inno.Shell.PollingShellFrameDriver`

<!-- api: T:Inno.Shell.PollingShellFrameDriver 8e88ae0bd13f1b05d72e4c0898c5b3e1a082cbbfe3577ae642f2150d65634e1a -->
<!-- api: P:Inno.Shell.PollingShellFrameDriver.allowsBlockingPacing 480d7e37fe8cd7273998fd526d348ce5ac751fe5c665dcef99b9fbcc1630165a -->
<!-- api: M:Inno.Shell.PollingShellFrameDriver.RunAsync(System.Func{System.Boolean},System.Threading.CancellationToken) 081d25069e067663bb6fdac22a0ac55e42b8031082c48a729ebdf2e1731446cf -->
<!-- api: M:Inno.Shell.PollingShellFrameDriver.#ctor a2921d79ec2546ca82fe873004fa8258d7d13c407a5f9d5f9fe5c33bc1a8e6de -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-599a5a3679c40db7"></a>[`public Inno.Shell.PollingShellFrameDriver.PollingShellFrameDriver()`](../../src/composition/shell/Inno.Shell/PollingShellFrameDriver.cs#L10) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-c726fe206414e781"></a>[`public System.Threading.Tasks.ValueTask Inno.Shell.PollingShellFrameDriver.RunAsync(System.Func<bool> advanceFrame, System.Threading.CancellationToken cancellationToken)`](../../src/composition/shell/Inno.Shell/PollingShellFrameDriver.cs#L35) | Advances frames until the callback stops the loop, cancellation is requested or a callback fails.<br>返回：Completion when the callback stops; cancellation and callback failures propagate to the owner.<br>`advanceFrame`：The owner-thread frame callback; false terminates the loop.<br>`cancellationToken`：Cancels before the next frame can execute. | — | `System.ArgumentNullException`：The frame callback is null.<br>`System.OperationCanceledException`：Cancellation was requested before a frame. |
| <a id="api-a2c06ab51290bac9"></a>[`public bool Inno.Shell.PollingShellFrameDriver.allowsBlockingPacing { get; }`](../../src/composition/shell/Inno.Shell/PollingShellFrameDriver.cs#L15) | Gets whether this driver permits blocking frame pacing (true). | — | — |
| <a id="api-57edc7000c86a3bb"></a>[`public sealed class Inno.Shell.PollingShellFrameDriver : Inno.Shell.IShellFrameDriver`](../../src/composition/shell/Inno.Shell/PollingShellFrameDriver.cs#L10) | Runs an application-owned frame loop synchronously on its calling thread. | — | — |

### `Inno.Shell.ScheduledShellFrameDriver`

<!-- api: T:Inno.Shell.ScheduledShellFrameDriver b807de2b2927ba2cbcbd8e4f555c0d650994594992c57210c7fa5d909e99e597 -->
<!-- api: M:Inno.Shell.ScheduledShellFrameDriver.#ctor(System.Func{System.Threading.CancellationToken,System.Threading.Tasks.ValueTask}) 2889e3aac3edb22c445ab760ca6a7967e99dc1000981f819d657d414be13ab2d -->
<!-- api: P:Inno.Shell.ScheduledShellFrameDriver.allowsBlockingPacing 32ad9e0951dda110bdd65ca7d7699382a52c4a485bed4386402c17f10fd5e087 -->
<!-- api: M:Inno.Shell.ScheduledShellFrameDriver.RunAsync(System.Func{System.Boolean},System.Threading.CancellationToken) 7396972093fcd0948e999010916d786b244add17e938555b442d2d9a8227591e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-cbb92cf3ae88ac68"></a>[`public Inno.Shell.ScheduledShellFrameDriver.ScheduledShellFrameDriver(System.Func<System.Threading.CancellationToken, System.Threading.Tasks.ValueTask> nextFrame)`](../../src/composition/shell/Inno.Shell/ScheduledShellFrameDriver.cs#L23) | Creates a driver whose scheduling callback resumes on the frame owner's thread.<br>`nextFrame`：The callback that waits for a presentation opportunity and observes cancellation. | — | `System.ArgumentNullException`：The callback is null. |
| <a id="api-3701ad4c62376278"></a>[`public System.Threading.Tasks.ValueTask Inno.Shell.ScheduledShellFrameDriver.RunAsync(System.Func<bool> advanceFrame, System.Threading.CancellationToken cancellationToken)`](../../src/composition/shell/Inno.Shell/ScheduledShellFrameDriver.cs#L51) | Advances frames until the callback stops the loop, cancellation is requested or a callback fails.<br>返回：Completion when the callback stops; cancellation and callback failures propagate to the owner.<br>`advanceFrame`：The owner-thread frame callback; false terminates the loop.<br>`cancellationToken`：Cancels before the next frame can execute. | — | `System.ArgumentNullException`：The frame callback is null.<br>`System.OperationCanceledException`：Cancellation was requested before a frame. |
| <a id="api-ba81d8afcc5bac6d"></a>[`public bool Inno.Shell.ScheduledShellFrameDriver.allowsBlockingPacing { get; }`](../../src/composition/shell/Inno.Shell/ScheduledShellFrameDriver.cs#L31) | Gets whether this driver permits blocking frame pacing (false). | — | — |
| <a id="api-fa5fbaa941203d8c"></a>[`public sealed class Inno.Shell.ScheduledShellFrameDriver : Inno.Shell.IShellFrameDriver`](../../src/composition/shell/Inno.Shell/ScheduledShellFrameDriver.cs#L10) | Runs frames when an external host grants a nonblocking presentation opportunity. | — | — |

### `Inno.Shell.Shell`

<!-- api: T:Inno.Shell.Shell 2e1b299bffe18896a53708727d5cc4d681faf5e111c71c97b3745c8b9cd9f4f8 -->
<!-- api: M:Inno.Shell.Shell.#ctor(Inno.Adapter.IAdapterCatalog,Inno.Shell.ShellOptions) e3354783cad0e91c5c9dc5148fcd8f1f11fa87342510fe9e666bba8b2db66609 -->
<!-- api: P:Inno.Shell.Shell.framePacing aff75d5888f97216a7517202f9fc4276f5d2b6c49998e6987cd629e561dc61de -->
<!-- api: P:Inno.Shell.Shell.state 15d0b3613866def3f409f74f7eeceba31f7f55007d4694212e3a7d47047a98ca -->
<!-- api: M:Inno.Shell.Shell.RunAsync(Inno.Shell.IShellFrameDriver,System.Nullable{System.Int32},System.Threading.CancellationToken) ab4c50014e038b7102de4e7a2a06d0b9f36bfd196ad095b7979699a1c0752e55 -->
<!-- api: M:Inno.Shell.Shell.Dispose cc3e8c3b8038bef28f2290d3d0aebccf170f15c55c724da49989535e01771b44 -->
<!-- api: P:Inno.Shell.Shell.adapters 685c5f81dc4f4a2f67c317fb4b36f40f47dfe6012434e23bda04ead488477e39 -->
<!-- api: P:Inno.Shell.Shell.adapterSelection 149801246b4b1ec9bdb93f8a4470e99c1eb1af6f95ad07082a3a15ea1e11a0a3 -->
<!-- api: P:Inno.Shell.Shell.platformApplication 4b4837a49659e2be2d403f289ec1371e54a2bdd4776b05938f44be5589bcc267 -->
<!-- api: P:Inno.Shell.Shell.primaryWindow 526f3fd7cc4a0e4397842f868869128accf8f4daa57b63901a55230352013ff2 -->
<!-- api: P:Inno.Shell.Shell.inputSource 406a8f5c4809880043b802dd3b0efe7c4844be442f124d7b2f37007c8f5768af -->
<!-- api: P:Inno.Shell.Shell.renderDevice 770c9c453c3b715fe0b7b57ab964068207e00bea80bad998a262a03a5250f783 -->
<!-- api: P:Inno.Shell.Shell.options 8d5bc50800743bcf2dd71fcfcc662efc5266079913aa364bd3d3ad1595344ddc -->
<!-- api: P:Inno.Shell.Shell.hasCompletedFrame 61c64cc295ba6b45c1df882887bde86990b87b2a3b84a41dcca532d68971289d -->
<!-- api: M:Inno.Shell.Shell.UseHostPipeline(Inno.Runtime.RuntimeSubsystemPipeline) 2982d8e75a21f9e14613f67889a01404937ab29f02ca44c5b43a0632d8918e03 -->
<!-- api: M:Inno.Shell.Shell.InitializeAdapterResources b60c7c1a341bbf3ec76561ebaa977853504e654767d094479e5a5867228c6492 -->
<!-- api: M:Inno.Shell.Shell.RequestExit 7e6dade4f23ad93e3cd4941e15018552967b4bf705c6a95f2c1aef64818ad989 -->
<!-- api: M:Inno.Shell.Shell.OnStarting ba6389851a50feec75596e9e1e2bb0ef2a27bfe402485a8ec6223a1503571058 -->
<!-- api: M:Inno.Shell.Shell.OnEvent(Inno.Core.Events.Event) ce91124a8094654892366a16200e6e992d45b18f7066383531bdf7f7ca1f1bc1 -->
<!-- api: M:Inno.Shell.Shell.OnSuspensionChanged(System.Boolean) 6fcc5fdba928faf95201698852cfc07025a55ee2ccd14138bfa67b6791cb4fe9 -->
<!-- api: M:Inno.Shell.Shell.ShouldExit(Inno.Core.Events.Event) a26aaaf31d06b97a662345eaa8f6a170527d25d751c9a750723e1abf83a290fe -->
<!-- api: M:Inno.Shell.Shell.OnFrame(Inno.Shell.ShellFrame) 1e92c2106911564849b01baea78f8a5d2b69aca3666f76fbbf063f4a50fe4af0 -->
<!-- api: M:Inno.Shell.Shell.OnPresentation(Inno.Shell.ShellFrame) d6e2e3721fdc59730f785cdddce4832231c47b5b6171f9f30135e2f58329f81a -->
<!-- api: M:Inno.Shell.Shell.OnSmokeCompleted(System.Int32) 3729605297e0686e55765f673404bbb8f784475a28375bc9a4b2e83832eac288 -->
<!-- api: M:Inno.Shell.Shell.OnStopping a32ada468d2be018084ddb6f22e8f12feb61daba2d3c9393c875c0a5614e70a0 -->
<!-- api: M:Inno.Shell.Shell.DisposeProductResources 446ea18ecdc9f8a0969a1a1b85660fe25a0185c5050ac3861716929cfb37f8e8 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9dae1bb75d25e8ee"></a>[`protected Inno.Shell.Shell.Shell(Inno.Adapter.IAdapterCatalog adapterCatalog, Inno.Shell.ShellOptions options)`](../../src/composition/shell/Inno.Shell/Shell.cs#L59) | Creates the backend-neutral host resources shared by Player and Editor products.<br>`adapterCatalog`：Catalog that resolves explicit backend selections without leaking implementation types.<br>`options`：Primary-window and rendering policy. | — | `System.ArgumentNullException`：Thrown when adapterCatalog or options is null. |
| <a id="api-4f92ebd105a9dd8c"></a>[`public void Inno.Shell.Shell.Dispose()`](../../src/composition/shell/Inno.Shell/Shell.cs#L157) | Releases product resources followed by rendering, input, window, and platform resources. | — | — |
| <a id="api-0afcc3f35a149915"></a>[`protected virtual void Inno.Shell.Shell.DisposeProductResources()`](../../src/composition/shell/Inno.Shell/Shell.cs#L387) | Releases resources owned by the derived product before common adapter resources are destroyed. | — | — |
| <a id="api-ff5efce6c57e8263"></a>[`protected void Inno.Shell.Shell.InitializeAdapterResources()`](../../src/composition/shell/Inno.Shell/Shell.cs#L253) | Creates common adapter resources after a derived product has prepared its non-window bootstrap state. | — | `System.InvalidOperationException`：Thrown when common adapter resources are already initialized. |
| <a id="api-2848e5dfc35a8260"></a>[`protected virtual void Inno.Shell.Shell.OnEvent(Inno.Core.Events.Event evnt)`](../../src/composition/shell/Inno.Shell/Shell.cs#L324) | Receives one backend-neutral platform event after shared input routing and close evaluation.<br>`evnt`：Event produced by the active platform adapter. | — | — |
| <a id="api-3ea128c5f877ad9d"></a>[`protected abstract void Inno.Shell.Shell.OnFrame(Inno.Shell.ShellFrame frame)`](../../src/composition/shell/Inno.Shell/Shell.cs#L356) | Advances one product-specific frame after all pending platform events have been dispatched.<br>`frame`：Immutable timing and identity for the current shell frame. | — | — |
| <a id="api-f74d4066262e2ab4"></a>[`protected virtual void Inno.Shell.Shell.OnPresentation(Inno.Shell.ShellFrame frame)`](../../src/composition/shell/Inno.Shell/Shell.cs#L364) | Submits product UI requests while the host output pipeline is open.<br>`frame`：The current application frame. | — | — |
| <a id="api-c277b6de088bd144"></a>[`protected virtual void Inno.Shell.Shell.OnSmokeCompleted(int frameCount)`](../../src/composition/shell/Inno.Shell/Shell.cs#L373) | Receives the final frame count only when a bounded smoke run reaches its requested frame limit. Closing a window or requesting an earlier exit does not report smoke completion.<br>`frameCount`：Number of product frames completed by the smoke run. | — | — |
| <a id="api-1cdf456da8e889ce"></a>[`protected virtual void Inno.Shell.Shell.OnStarting()`](../../src/composition/shell/Inno.Shell/Shell.cs#L314) | Runs once immediately before the common event and frame loop starts. | — | — |
| <a id="api-c3d4da32b9e37f55"></a>[`protected virtual void Inno.Shell.Shell.OnStopping()`](../../src/composition/shell/Inno.Shell/Shell.cs#L380) | Runs once when the main loop is stopping while all product and adapter resources remain alive. | — | — |
| <a id="api-6b58d695e0c0fc63"></a>[`protected virtual void Inno.Shell.Shell.OnSuspensionChanged(bool isSuspended)`](../../src/composition/shell/Inno.Shell/Shell.cs#L335) | Receives an effective application suspension change at an event-pump safety point. Repeated notifications and changes that leave another suspension reason active do not invoke this hook.<br>`isSuspended`：Whether product frames and their clock are suspended after the change. | — | — |
| <a id="api-ad13774ad558bb6b"></a>[`protected void Inno.Shell.Shell.RequestExit()`](../../src/composition/shell/Inno.Shell/Shell.cs#L309) | Requests an orderly exit after the current event or frame callback completes. | — | — |
| <a id="api-0a9848122f60aecc"></a>[`public System.Threading.Tasks.Task<int> Inno.Shell.Shell.RunAsync(Inno.Shell.IShellFrameDriver driver, int? smokeFrameLimit = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/composition/shell/Inno.Shell/Shell.cs#L109) | Runs one shell lifecycle using an explicitly selected owner-thread frame driver.<br>返回：Zero after orderly shutdown; lifecycle and scheduling failures propagate.<br>`driver`：The host's scheduling policy; it must execute frames on their owning thread.<br>`smokeFrameLimit`：An optional positive frame count for bounded verification.<br>`cancellationToken`：Requests termination before the next frame. | — | `System.ArgumentNullException`：The frame driver is null.<br>`System.ArgumentOutOfRangeException`：The smoke frame limit is not positive.<br>`System.InvalidOperationException`：Adapter resources are missing or the shell has already run.<br>`System.OperationCanceledException`：Scheduling was canceled. |
| <a id="api-79ebc40b29c07515"></a>[`protected virtual bool Inno.Shell.Shell.ShouldExit(Inno.Core.Events.Event evnt)`](../../src/composition/shell/Inno.Shell/Shell.cs#L346) | Determines whether one event requests orderly product shutdown.<br>返回：true when the common loop should stop.<br>`evnt`：Event produced by the active platform adapter. | — | — |
| <a id="api-a4ff3089d98bce16"></a>[`protected void Inno.Shell.Shell.UseHostPipeline(Inno.Runtime.RuntimeSubsystemPipeline pipeline)`](../../src/composition/shell/Inno.Shell/Shell.cs#L239) | Borrows the EngineHost-owned pipeline used for application-wide output and services.<br>`pipeline`：The started pipeline; its EngineHost remains responsible for retirement. | — | `System.InvalidOperationException`：A host pipeline is already configured. |
| <a id="api-f46e011cc134a516"></a>[`protected Inno.Adapter.IAdapterCatalog Inno.Shell.Shell.adapters { get; }`](../../src/composition/shell/Inno.Shell/Shell.cs#L185) | Gets the implementation-neutral adapter catalog used by this product host. | — | — |
| <a id="api-ac46e218460f23d3"></a>[`protected Inno.Adapter.AdapterSelection Inno.Shell.Shell.adapterSelection { get; }`](../../src/composition/shell/Inno.Shell/Shell.cs#L190) | Gets the immutable backend selection used by this product host. | — | — |
| <a id="api-c3a75a1147bf23de"></a>[`public Inno.Platform.FramePacingOptions Inno.Shell.Shell.framePacing { get; }`](../../src/composition/shell/Inno.Shell/Shell.cs#L75) | Gets the mutable presentation cadence applied at the next complete host frame. Zero maximum frame rate means no software frame limit. | — | — |
| <a id="api-112f747ecee94841"></a>[`protected bool Inno.Shell.Shell.hasCompletedFrame { get; private set; }`](../../src/composition/shell/Inno.Shell/Shell.cs#L228) | Gets whether at least one complete product frame has run. | — | — |
| <a id="api-43a4f0a65cac7b9e"></a>[`protected Inno.Adapter.Input.IInputEventSource Inno.Shell.Shell.inputSource { get; }`](../../src/composition/shell/Inno.Shell/Shell.cs#L209) | Gets the shared input event source used to create isolated runtime-session backends. | — | — |
| <a id="api-8ec315d70c06ab16"></a>[`protected Inno.Shell.ShellOptions Inno.Shell.Shell.options { get; }`](../../src/composition/shell/Inno.Shell/Shell.cs#L223) | Gets the immutable shell creation options. | — | — |
| <a id="api-4d400ea77609041e"></a>[`protected Inno.Platform.IPlatformApplication Inno.Shell.Shell.platformApplication { get; }`](../../src/composition/shell/Inno.Shell/Shell.cs#L195) | Gets the active backend-neutral platform application. | — | — |
| <a id="api-da52a3c016be6dff"></a>[`protected Inno.Platform.IPlatformWindow Inno.Shell.Shell.primaryWindow { get; }`](../../src/composition/shell/Inno.Shell/Shell.cs#L202) | Gets the active primary platform window. | — | — |
| <a id="api-062d39baa9b23133"></a>[`protected Inno.Rendering.IRenderDevice Inno.Shell.Shell.renderDevice { get; }`](../../src/composition/shell/Inno.Shell/Shell.cs#L216) | Gets the active backend-neutral rendering device. | — | — |
| <a id="api-1720c626f52a78df"></a>[`public Inno.Shell.ShellState Inno.Shell.Shell.state { get; private set; }`](../../src/composition/shell/Inno.Shell/Shell.cs#L80) | Gets the current owner-thread lifecycle state, including suspension and failed retirement. | — | — |
| <a id="api-22558ac0338c8e35"></a>[`public abstract class Inno.Shell.Shell : System.IDisposable`](../../src/composition/shell/Inno.Shell/Shell.cs#L22) | Owns the backend-neutral window, event pump, input source, rendering device, and host frame lifecycle. | — | — |

### `Inno.Shell.ShellFrame`

<!-- api: T:Inno.Shell.ShellFrame 41c836564babf960c878f7f4f3b0cec0533210bfc03697b38ce9f7de6a65fee6 -->
<!-- api: M:Inno.Shell.ShellFrame.#ctor(System.Int32,System.Double,System.Single) fb38f5564752690a0cce13ba564803be2663a84398257c5c4b12a689cd7c4e64 -->
<!-- api: P:Inno.Shell.ShellFrame.frameIndex c83bc257f2ceafac94eb256cb0394c2e3cc134572ef941c19df14ab77b8f7a87 -->
<!-- api: P:Inno.Shell.ShellFrame.totalTime 2afd02881b3dfc19fa3a9aa0725048f3644f9b2a1c014a8ef3c92fa53dd14606 -->
<!-- api: P:Inno.Shell.ShellFrame.deltaTime b08669275323985c8fa1b85393fb520a6281f920811c3802a9242e91321dfbc8 -->
<!-- api: M:Inno.Shell.ShellFrame.ToString ccf65c7e44fa3cf0f7f2cbe3b9b19b6b460f7150b63319d8f490869a568c65d0 -->
<!-- api: M:Inno.Shell.ShellFrame.op_Inequality(Inno.Shell.ShellFrame,Inno.Shell.ShellFrame) 52a21c98817c1df7c1f12998e81c0c94d51a948bc012d23d147f03f5999f2534 -->
<!-- api: M:Inno.Shell.ShellFrame.op_Equality(Inno.Shell.ShellFrame,Inno.Shell.ShellFrame) fd8514b53f64623f0eac6b39730ae334ca543325615b74ff7dfb34181bf22775 -->
<!-- api: M:Inno.Shell.ShellFrame.GetHashCode 31076f0396744004e897611d7d266b63e543b0c47729fe2073dc4dde5c344ec8 -->
<!-- api: M:Inno.Shell.ShellFrame.Equals(System.Object) 73f04d41ae8c13b9589ee2c91ea967a538b63aa28b569be626867da63a71c080 -->
<!-- api: M:Inno.Shell.ShellFrame.Equals(Inno.Shell.ShellFrame) 85bbd27d6aa8b166c79ddfe75550e7e9ec8d03ae7b6abe60100d7cdbe3040ace -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-57d720cb127598cc"></a>[`public Inno.Shell.ShellFrame.ShellFrame(int frameIndex, double totalTime, float deltaTime)`](../../src/composition/shell/Inno.Shell/ShellFrame.cs#L22) | Creates one validated shell frame.<br>`frameIndex`：Zero-based shell frame index.<br>`totalTime`：Monotonic elapsed host time in seconds.<br>`deltaTime`：Non-negative elapsed time since the previous frame in seconds. | — | — |
| <a id="api-5ce09374267ceae4"></a>[`public bool Inno.Shell.ShellFrame.Equals(Inno.Shell.ShellFrame other)`](../../src/composition/shell/Inno.Shell/ShellFrame.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-fc4c53267116c131"></a>[`public override bool Inno.Shell.ShellFrame.Equals(object obj)`](../../src/composition/shell/Inno.Shell/ShellFrame.cs#L8) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-5b96c55547d963d8"></a>[`public override int Inno.Shell.ShellFrame.GetHashCode()`](../../src/composition/shell/Inno.Shell/ShellFrame.cs#L8) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-46488cb71d97c62a"></a>[`public static bool Inno.Shell.ShellFrame.operator ==(Inno.Shell.ShellFrame left, Inno.Shell.ShellFrame right)`](../../src/composition/shell/Inno.Shell/ShellFrame.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-ff3591600afbc5dc"></a>[`public static bool Inno.Shell.ShellFrame.operator !=(Inno.Shell.ShellFrame left, Inno.Shell.ShellFrame right)`](../../src/composition/shell/Inno.Shell/ShellFrame.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-f8cabff41b58b6ae"></a>[`public override string Inno.Shell.ShellFrame.ToString()`](../../src/composition/shell/Inno.Shell/ShellFrame.cs#L8) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-575b9c5db31e0c91"></a>[`public float Inno.Shell.ShellFrame.deltaTime { get; }`](../../src/composition/shell/Inno.Shell/ShellFrame.cs#L48) | Gets non-negative elapsed time since the previous frame in seconds. | — | — |
| <a id="api-54e2bd295e831201"></a>[`public int Inno.Shell.ShellFrame.frameIndex { get; }`](../../src/composition/shell/Inno.Shell/ShellFrame.cs#L38) | Gets the zero-based shell frame index. | — | — |
| <a id="api-68fd9fbaf98e5253"></a>[`public double Inno.Shell.ShellFrame.totalTime { get; }`](../../src/composition/shell/Inno.Shell/ShellFrame.cs#L43) | Gets monotonic elapsed host time in seconds. | — | — |
| <a id="api-5090426a4edb698c"></a>[`public readonly record struct Inno.Shell.ShellFrame : System.IEquatable<Inno.Shell.ShellFrame>`](../../src/composition/shell/Inno.Shell/ShellFrame.cs#L8) | Captures immutable timing and identity for one composition-shell frame. | — | — |

### `Inno.Shell.ShellOptions`

<!-- api: T:Inno.Shell.ShellOptions 7f09294229d2876f0310e64c1d12e8734b9f5f5b1100d03f614425e04eaff52a -->
<!-- api: P:Inno.Shell.ShellOptions.adapters 2f4e342ad6654d4b620ca4ccf344b068e3166634447ed16ca641777ddf93aba7 -->
<!-- api: P:Inno.Shell.ShellOptions.window 74217cb34bcd13d5be1c8dc2008e0e19e1d2e38bcd7d486393c627d4fbd1e7b1 -->
<!-- api: P:Inno.Shell.ShellOptions.preferredGraphicsApi 4fff705cd2406a94bf38c6420c187edae22349f4e868470cce826b92d74f1fb7 -->
<!-- api: P:Inno.Shell.ShellOptions.verticalSync 3c1caa5bdfd2b03e5b85c017e13d11c0df463cde98ca5d7924cb5af14ec0f7a1 -->
<!-- api: P:Inno.Shell.ShellOptions.sRgbBackbuffer 3a5b314b2c444d5b2175ab77b30e59683a632a7317c160d46f46fa1038f6794b -->
<!-- api: P:Inno.Shell.ShellOptions.forceSingleThreadedRendering c50342b57d0907562d7fa40ae3d7882f0dc63c72243516685bca4a38581351cc -->
<!-- api: P:Inno.Shell.ShellOptions.suspendWhenHidden 98635edc53be5a472b2f829567f5e3f0c8698bea47692bab898e334140e8c56e -->
<!-- api: M:Inno.Shell.ShellOptions.#ctor fc282c4b3a41d92c5e06e81103f038b918a21cf6a1d6285af2cd0cfb1f0f50fa -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-674523066434881e"></a>[`public Inno.Shell.ShellOptions.ShellOptions()`](../../src/composition/shell/Inno.Shell/ShellOptions.cs#L10) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-4305f4f63039fb0e"></a>[`public required Inno.Adapter.AdapterSelection Inno.Shell.ShellOptions.adapters { get; set; }`](../../src/composition/shell/Inno.Shell/ShellOptions.cs#L15) | Gets or sets the complete backend selection used by the shell and its derived product host. | — | — |
| <a id="api-71ac14ef7a417d2f"></a>[`public bool Inno.Shell.ShellOptions.forceSingleThreadedRendering { get; set; }`](../../src/composition/shell/Inno.Shell/ShellOptions.cs#L40) | Gets or sets whether rendering must execute on the calling thread. | — | — |
| <a id="api-4d870c93bfe078c8"></a>[`public Inno.Rendering.GraphicsApi? Inno.Shell.ShellOptions.preferredGraphicsApi { get; set; }`](../../src/composition/shell/Inno.Shell/ShellOptions.cs#L25) | Gets or sets the preferred graphics API, or null for the rendering-backend default. | — | — |
| <a id="api-cb1c1e820f243c05"></a>[`public bool Inno.Shell.ShellOptions.sRgbBackbuffer { get; set; }`](../../src/composition/shell/Inno.Shell/ShellOptions.cs#L35) | Gets or sets whether the primary backbuffer performs sRGB encoding. | — | — |
| <a id="api-ba51a7af8496c44b"></a>[`public bool Inno.Shell.ShellOptions.suspendWhenHidden { get; set; }`](../../src/composition/shell/Inno.Shell/ShellOptions.cs#L46) | Gets or sets whether hiding or minimizing the primary window suspends product frames. Application suspension always stops frames independently of this window policy. | — | — |
| <a id="api-420638a42aad258d"></a>[`public bool Inno.Shell.ShellOptions.verticalSync { get; set; }`](../../src/composition/shell/Inno.Shell/ShellOptions.cs#L30) | Gets or sets whether presentation waits for display synchronization. | — | — |
| <a id="api-37b58ba36bd94f41"></a>[`public Inno.Platform.PlatformWindowOptions Inno.Shell.ShellOptions.window { get; set; }`](../../src/composition/shell/Inno.Shell/ShellOptions.cs#L20) | Gets or sets primary-window creation options. | — | — |
| <a id="api-bed99a73cc5af969"></a>[`public sealed class Inno.Shell.ShellOptions`](../../src/composition/shell/Inno.Shell/ShellOptions.cs#L10) | Configures backend selection and primary-window policy for a composition shell. | — | — |

### `Inno.Shell.ShellState`

<!-- api: T:Inno.Shell.ShellState 15fbdece45d4b17ee01994736ddf8a85ea5068cd5d4de4824c1c5f50e4f001b5 -->
<!-- api: F:Inno.Shell.ShellState.Created acb509e99ef0cd62aeff553d1d82e2145ec5c6dd63d7f5d9e4a9854bf85fb45d -->
<!-- api: F:Inno.Shell.ShellState.Ready dc6f6392450b5236ac12c6066a972071568c3cf208effdf83c4bc098cb6c91fc -->
<!-- api: F:Inno.Shell.ShellState.Running 1d6b4fea915fbf26fda6fca4d1c1bb9954e463f41ba257851e0663b2c5c6d020 -->
<!-- api: F:Inno.Shell.ShellState.Suspended 554ab66630d007079940330a777311e06173ed2eb78c92ad2400f661c61d94de -->
<!-- api: F:Inno.Shell.ShellState.Stopping 4243eaa31e86c9b63d9ad2679c69a5a040fa8c7ee7d2a0062e949b016880c63a -->
<!-- api: F:Inno.Shell.ShellState.Stopped 7936124551c1c5868262f1286bf661be15b86438a91918085b9bb620e0cdd6a8 -->
<!-- api: F:Inno.Shell.ShellState.Faulted f0a4f68901d8af5e0a5bbef10fbfad28e7404f8e63fbfd3aa2227036876483fb -->
<!-- api: F:Inno.Shell.ShellState.Disposed 80b7d16959cb13cca06d7c3f95595d6015187e81179948643b0264e600eca7ce -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e01af4c9044f5bc1"></a>[`Inno.Shell.ShellState.Created = 0`](../../src/composition/shell/Inno.Shell/ShellState.cs#L11) | Adapter resources have not been initialized. | — | — |
| <a id="api-07d9404a6185f130"></a>[`Inno.Shell.ShellState.Disposed = 7`](../../src/composition/shell/Inno.Shell/ShellState.cs#L46) | All owned resources were released successfully. | — | — |
| <a id="api-3af9797c3727452c"></a>[`Inno.Shell.ShellState.Faulted = 6`](../../src/composition/shell/Inno.Shell/ShellState.cs#L41) | Initialization, execution, or retirement failed; another run is prohibited. | — | — |
| <a id="api-9d85225eaaacfe2c"></a>[`Inno.Shell.ShellState.Ready = 1`](../../src/composition/shell/Inno.Shell/ShellState.cs#L16) | Adapter resources are ready for the single permitted run. | — | — |
| <a id="api-879f9dedd7965927"></a>[`Inno.Shell.ShellState.Running = 2`](../../src/composition/shell/Inno.Shell/ShellState.cs#L21) | Platform events and product frames are advancing. | — | — |
| <a id="api-f69150a7df5342f4"></a>[`Inno.Shell.ShellState.Stopped = 5`](../../src/composition/shell/Inno.Shell/ShellState.cs#L36) | The run ended or was canceled and the stopping callback completed. | — | — |
| <a id="api-b7489c0c7e107c22"></a>[`Inno.Shell.ShellState.Stopping = 4`](../../src/composition/shell/Inno.Shell/ShellState.cs#L31) | The stopping callback is executing while resources remain alive. | — | — |
| <a id="api-ca50aef073c3b0bc"></a>[`Inno.Shell.ShellState.Suspended = 3`](../../src/composition/shell/Inno.Shell/ShellState.cs#L26) | Platform events continue while product frames and their clock are suspended. | — | — |
| <a id="api-a761c86bc837a685"></a>[`public enum Inno.Shell.ShellState : int`](../../src/composition/shell/Inno.Shell/ShellState.cs#L6) | Describes the observable lifetime of one owner-thread composition shell. | — | — |

## 项目依赖

- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Runtime](Inno.Runtime.md)：公开引用边界由实际签名核对。
- [Inno.Runtime.Contracts](Inno.Runtime.Contracts.md)：公开引用边界由实际签名核对。
- [Inno.Adapter](Inno.Adapter.md)：公开引用边界由实际签名核对。
- [Inno.Core.Events](../core/Inno.Core.Events.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Input](../input/Inno.Adapter.Input.md)：公开引用边界由实际签名核对。
- [Inno.Platform](../platform/Inno.Platform.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Rendering](../rendering/Inno.Adapter.Rendering.md)：公开引用边界由实际签名核对。
- [Inno.Rendering](../rendering/Inno.Rendering.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
