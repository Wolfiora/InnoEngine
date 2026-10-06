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
internal sealed class GameHost : Shell
{
    internal GameHost(IAdapterCatalog adapters, ShellOptions options)
        : base(adapters, options)
    {
        InitializeAdapterResources();
    }

    protected override void OnFrame(ShellFrame frame)
    {
        // Advance the product runtime session.
    }
}
```

一个 Shell 只能运行一次。初始化中途失败会回滚已创建的 render/input/window/application；Dispose 先调用产品释放，再按 render → input → window → platform 逆序清理，并聚合 cleanup exception。`GamePlayerHost` 与 `EditorHost` 都必须继承 Shell，Architecture Tool 会拒绝直接引用具体 adapter 的 Host。

`RunAsync(driver, smokeFrameLimit)` 只有实际完成的帧数达到所请求的上限，才调用 `OnSmokeCompleted`。

唯一循环入口为 `RunAsync(driver, smokeFrameLimit, cancellationToken)`。`IShellFrameDriver` 只提供 `allowsBlockingPacing` 和 `RunAsync(advanceFrame, cancellationToken)`，回调必须在窗口/渲染 owner thread 执行。`PollingShellFrameDriver` 同步循环并允许软件帧率等待；`ScheduledShellFrameDriver(nextFrame)` 等待外部绘制机会、不阻塞宿主。两者共享 Shell 的同一帧实现，停止、取消和 callback 失败都经过同一 stopping 通知与资源退休。

```csharp
using Inno.Shell;

await shell.RunAsync(new PollingShellFrameDriver(), smokeFrameLimit: 60);
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

以下仅列出当前程序集自己声明的 public/protected 契约；继承成员遵循所属基类页面。internal/private 实现不作为稳定公开 API。签名依据当前源码语义模型生成，行为、参数、异常与所有权说明同时以对应英文 XML 为准。

### `Inno.Shell.IShellFrameDriver`

| 当前声明 | 行为 |
| --- | --- |
| [`System.Threading.Tasks.ValueTask Inno.Shell.IShellFrameDriver.RunAsync(System.Func<bool> advanceFrame, System.Threading.CancellationToken cancellationToken)`](../../src/composition/shell/Inno.Shell/IShellFrameDriver.cs#L29) | Schedules frames on the owning thread until a frame callback requests termination. |
| [`bool Inno.Shell.IShellFrameDriver.allowsBlockingPacing`](../../src/composition/shell/Inno.Shell/IShellFrameDriver.cs#L15) | Gets whether the shell may block the calling thread to enforce its frame rate. |
| [`Inno.Shell.IShellFrameDriver`](../../src/composition/shell/Inno.Shell/IShellFrameDriver.cs#L10) | Gives a host ownership of frame scheduling while the shell owns frame execution and retirement. |

### `Inno.Shell.PollingShellFrameDriver`

| 当前声明 | 行为 |
| --- | --- |
| [`System.Threading.Tasks.ValueTask Inno.Shell.PollingShellFrameDriver.RunAsync(System.Func<bool> advanceFrame, System.Threading.CancellationToken cancellationToken)`](../../src/composition/shell/Inno.Shell/PollingShellFrameDriver.cs#L35) | Advances frames until the callback stops the loop, cancellation is requested or a callback fails. |
| [`bool Inno.Shell.PollingShellFrameDriver.allowsBlockingPacing`](../../src/composition/shell/Inno.Shell/PollingShellFrameDriver.cs#L15) | Gets whether this driver permits blocking frame pacing (true). |
| [`Inno.Shell.PollingShellFrameDriver`](../../src/composition/shell/Inno.Shell/PollingShellFrameDriver.cs#L10) | Runs an application-owned frame loop synchronously on its calling thread. |

### `Inno.Shell.ScheduledShellFrameDriver`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Shell.ScheduledShellFrameDriver.ScheduledShellFrameDriver(System.Func<System.Threading.CancellationToken, System.Threading.Tasks.ValueTask> nextFrame)`](../../src/composition/shell/Inno.Shell/ScheduledShellFrameDriver.cs#L23) | Creates a driver whose scheduling callback resumes on the frame owner's thread. |
| [`System.Threading.Tasks.ValueTask Inno.Shell.ScheduledShellFrameDriver.RunAsync(System.Func<bool> advanceFrame, System.Threading.CancellationToken cancellationToken)`](../../src/composition/shell/Inno.Shell/ScheduledShellFrameDriver.cs#L51) | Advances frames until the callback stops the loop, cancellation is requested or a callback fails. |
| [`bool Inno.Shell.ScheduledShellFrameDriver.allowsBlockingPacing`](../../src/composition/shell/Inno.Shell/ScheduledShellFrameDriver.cs#L31) | Gets whether this driver permits blocking frame pacing (false). |
| [`Inno.Shell.ScheduledShellFrameDriver`](../../src/composition/shell/Inno.Shell/ScheduledShellFrameDriver.cs#L10) | Runs frames when an external host grants a nonblocking presentation opportunity. |

### `Inno.Shell.Shell`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Shell.Shell.Shell(Inno.Adapter.IAdapterCatalog adapterCatalog, Inno.Shell.ShellOptions options)`](../../src/composition/shell/Inno.Shell/Shell.cs#L59) | Creates the backend-neutral host resources shared by Player and Editor products. |
| [`void Inno.Shell.Shell.Dispose()`](../../src/composition/shell/Inno.Shell/Shell.cs#L157) | Releases product resources followed by rendering, input, window, and platform resources. |
| [`virtual void Inno.Shell.Shell.DisposeProductResources()`](../../src/composition/shell/Inno.Shell/Shell.cs#L387) | Releases resources owned by the derived product before common adapter resources are destroyed. |
| [`void Inno.Shell.Shell.InitializeAdapterResources()`](../../src/composition/shell/Inno.Shell/Shell.cs#L253) | Creates common adapter resources after a derived product has prepared its non-window bootstrap state. |
| [`virtual void Inno.Shell.Shell.OnEvent(Inno.Core.Events.Event evnt)`](../../src/composition/shell/Inno.Shell/Shell.cs#L324) | Receives one backend-neutral platform event after shared input routing and close evaluation. |
| [`abstract void Inno.Shell.Shell.OnFrame(Inno.Shell.ShellFrame frame)`](../../src/composition/shell/Inno.Shell/Shell.cs#L356) | Advances one product-specific frame after all pending platform events have been dispatched. |
| [`virtual void Inno.Shell.Shell.OnPresentation(Inno.Shell.ShellFrame frame)`](../../src/composition/shell/Inno.Shell/Shell.cs#L364) | Submits product UI requests while the host output pipeline is open. |
| [`virtual void Inno.Shell.Shell.OnSmokeCompleted(int frameCount)`](../../src/composition/shell/Inno.Shell/Shell.cs#L373) | Receives the final frame count only when a bounded smoke run reaches its requested frame limit. Closing a window or requesting an earlier exit does not report smoke completion. |
| [`virtual void Inno.Shell.Shell.OnStarting()`](../../src/composition/shell/Inno.Shell/Shell.cs#L314) | Runs once immediately before the common event and frame loop starts. |
| [`virtual void Inno.Shell.Shell.OnStopping()`](../../src/composition/shell/Inno.Shell/Shell.cs#L380) | Runs once when the main loop is stopping while all product and adapter resources remain alive. |
| [`virtual void Inno.Shell.Shell.OnSuspensionChanged(bool isSuspended)`](../../src/composition/shell/Inno.Shell/Shell.cs#L335) | Receives an effective application suspension change at an event-pump safety point. Repeated notifications and changes that leave another suspension reason active do not invoke this hook. |
| [`void Inno.Shell.Shell.RequestExit()`](../../src/composition/shell/Inno.Shell/Shell.cs#L309) | Requests an orderly exit after the current event or frame callback completes. |
| [`System.Threading.Tasks.Task<int> Inno.Shell.Shell.RunAsync(Inno.Shell.IShellFrameDriver driver, int? smokeFrameLimit = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/composition/shell/Inno.Shell/Shell.cs#L109) | Runs one shell lifecycle using an explicitly selected owner-thread frame driver. |
| [`virtual bool Inno.Shell.Shell.ShouldExit(Inno.Core.Events.Event evnt)`](../../src/composition/shell/Inno.Shell/Shell.cs#L346) | Determines whether one event requests orderly product shutdown. |
| [`void Inno.Shell.Shell.UseHostPipeline(Inno.Runtime.RuntimeSubsystemPipeline pipeline)`](../../src/composition/shell/Inno.Shell/Shell.cs#L239) | Borrows the EngineHost-owned pipeline used for application-wide output and services. |
| [`Inno.Adapter.AdapterSelection Inno.Shell.Shell.adapterSelection`](../../src/composition/shell/Inno.Shell/Shell.cs#L190) | Gets the immutable backend selection used by this product host. |
| [`Inno.Adapter.IAdapterCatalog Inno.Shell.Shell.adapters`](../../src/composition/shell/Inno.Shell/Shell.cs#L185) | Gets the implementation-neutral adapter catalog used by this product host. |
| [`Inno.Platform.FramePacingOptions Inno.Shell.Shell.framePacing`](../../src/composition/shell/Inno.Shell/Shell.cs#L75) | Gets the mutable presentation cadence applied at the next complete host frame. Zero maximum frame rate means no software frame limit. |
| [`bool Inno.Shell.Shell.hasCompletedFrame`](../../src/composition/shell/Inno.Shell/Shell.cs#L228) | Gets whether at least one complete product frame has run. |
| [`Inno.Adapter.Input.IInputEventSource Inno.Shell.Shell.inputSource`](../../src/composition/shell/Inno.Shell/Shell.cs#L209) | Gets the shared input event source used to create isolated runtime-session backends. |
| [`Inno.Shell.ShellOptions Inno.Shell.Shell.options`](../../src/composition/shell/Inno.Shell/Shell.cs#L223) | Gets the immutable shell creation options. |
| [`Inno.Platform.IPlatformApplication Inno.Shell.Shell.platformApplication`](../../src/composition/shell/Inno.Shell/Shell.cs#L195) | Gets the active backend-neutral platform application. |
| [`Inno.Platform.IPlatformWindow Inno.Shell.Shell.primaryWindow`](../../src/composition/shell/Inno.Shell/Shell.cs#L202) | Gets the active primary platform window. |
| [`Inno.Rendering.IRenderDevice Inno.Shell.Shell.renderDevice`](../../src/composition/shell/Inno.Shell/Shell.cs#L216) | Gets the active backend-neutral rendering device. |
| [`Inno.Shell.ShellState Inno.Shell.Shell.state`](../../src/composition/shell/Inno.Shell/Shell.cs#L80) | Gets the current owner-thread lifecycle state, including suspension and failed retirement. |
| [`Inno.Shell.Shell`](../../src/composition/shell/Inno.Shell/Shell.cs#L22) | Owns the backend-neutral window, event pump, input source, rendering device, and host frame lifecycle. |

### `Inno.Shell.ShellFrame`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Shell.ShellFrame.ShellFrame(int frameIndex, double totalTime, float deltaTime)`](../../src/composition/shell/Inno.Shell/ShellFrame.cs#L22) | Creates one validated shell frame. |
| [`float Inno.Shell.ShellFrame.deltaTime`](../../src/composition/shell/Inno.Shell/ShellFrame.cs#L48) | Gets non-negative elapsed time since the previous frame in seconds. |
| [`int Inno.Shell.ShellFrame.frameIndex`](../../src/composition/shell/Inno.Shell/ShellFrame.cs#L38) | Gets the zero-based shell frame index. |
| [`double Inno.Shell.ShellFrame.totalTime`](../../src/composition/shell/Inno.Shell/ShellFrame.cs#L43) | Gets monotonic elapsed host time in seconds. |
| [`Inno.Shell.ShellFrame`](../../src/composition/shell/Inno.Shell/ShellFrame.cs#L8) | Captures immutable timing and identity for one composition-shell frame. |

### `Inno.Shell.ShellOptions`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.AdapterSelection Inno.Shell.ShellOptions.adapters`](../../src/composition/shell/Inno.Shell/ShellOptions.cs#L15) | Gets or sets the complete backend selection used by the shell and its derived product host. |
| [`bool Inno.Shell.ShellOptions.forceSingleThreadedRendering`](../../src/composition/shell/Inno.Shell/ShellOptions.cs#L40) | Gets or sets whether rendering must execute on the calling thread. |
| [`Inno.Rendering.GraphicsApi? Inno.Shell.ShellOptions.preferredGraphicsApi`](../../src/composition/shell/Inno.Shell/ShellOptions.cs#L25) | Gets or sets the preferred graphics API, or for the rendering-backend default. |
| [`bool Inno.Shell.ShellOptions.sRgbBackbuffer`](../../src/composition/shell/Inno.Shell/ShellOptions.cs#L35) | Gets or sets whether the primary backbuffer performs sRGB encoding. |
| [`bool Inno.Shell.ShellOptions.suspendWhenHidden`](../../src/composition/shell/Inno.Shell/ShellOptions.cs#L46) | Gets or sets whether hiding or minimizing the primary window suspends product frames. Application suspension always stops frames independently of this window policy. |
| [`bool Inno.Shell.ShellOptions.verticalSync`](../../src/composition/shell/Inno.Shell/ShellOptions.cs#L30) | Gets or sets whether presentation waits for display synchronization. |
| [`Inno.Platform.PlatformWindowOptions Inno.Shell.ShellOptions.window`](../../src/composition/shell/Inno.Shell/ShellOptions.cs#L20) | Gets or sets primary-window creation options. |
| [`Inno.Shell.ShellOptions`](../../src/composition/shell/Inno.Shell/ShellOptions.cs#L10) | Configures backend selection and primary-window policy for a composition shell. |

### `Inno.Shell.ShellState`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Shell.ShellState.Created`](../../src/composition/shell/Inno.Shell/ShellState.cs#L11) | Adapter resources have not been initialized. |
| [`Inno.Shell.ShellState.Disposed`](../../src/composition/shell/Inno.Shell/ShellState.cs#L46) | All owned resources were released successfully. |
| [`Inno.Shell.ShellState.Faulted`](../../src/composition/shell/Inno.Shell/ShellState.cs#L41) | Initialization, execution, or retirement failed; another run is prohibited. |
| [`Inno.Shell.ShellState.Ready`](../../src/composition/shell/Inno.Shell/ShellState.cs#L16) | Adapter resources are ready for the single permitted run. |
| [`Inno.Shell.ShellState.Running`](../../src/composition/shell/Inno.Shell/ShellState.cs#L21) | Platform events and product frames are advancing. |
| [`Inno.Shell.ShellState.Stopped`](../../src/composition/shell/Inno.Shell/ShellState.cs#L36) | The run ended or was canceled and the stopping callback completed. |
| [`Inno.Shell.ShellState.Stopping`](../../src/composition/shell/Inno.Shell/ShellState.cs#L31) | The stopping callback is executing while resources remain alive. |
| [`Inno.Shell.ShellState.Suspended`](../../src/composition/shell/Inno.Shell/ShellState.cs#L26) | Platform events continue while product frames and their clock are suspended. |
| [`Inno.Shell.ShellState`](../../src/composition/shell/Inno.Shell/ShellState.cs#L6) | Describes the observable lifetime of one owner-thread composition shell. |

## 项目依赖

- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Runtime](Inno.Runtime.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Runtime.Contracts](Inno.Runtime.Contracts.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Adapter](Inno.Adapter.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Core.Events](../core/Inno.Core.Events.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Adapter.Input](../input/Inno.Adapter.Input.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Platform](../platform/Inno.Platform.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Adapter.Rendering](../rendering/Inno.Adapter.Rendering.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Rendering](../rendering/Inno.Rendering.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：项目引用；公开签名可见性由语义边界检查确认。

共同 MSBuild 注入的 analyzer 与编译规则属于构建依赖，完整有效项目图记录在本轮验收证据中。
