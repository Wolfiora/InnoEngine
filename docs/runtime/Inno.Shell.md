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
