# Inno.Core.Layers

[Core 索引](README.md) · [Wiki 首页](../README.md) · [Runtime](../runtime/README.md) · [架构 Overview](../architecture/ENGINE_ARCHITECTURE_OVERVIEW.md)

## 职责与边界

`Inno.Core.Layers` 提供不依赖 Editor、Player、Scene、Rendering 或任何 native backend 的有序生命周期原语。
它解决三件事：Layer/Overlay 顺序、每个 Layer 独立的 `EventHub` 作用域，以及 render prepare/submit/unwind
的确定性执行。它不创建窗口、设备、RuntimeSession 或业务服务，也不是 Composition Shell。

旧 `Inno.Core.Framework` 同时拥有 Shell、Assets、Plugins 与 Settings，依赖方向错误，因此不能恢复。
其中真正通用的 `Layer`/`LayerStack` 被提取为本项目；`Inno.Shell` 和产品 Host 仍留在 Composition。

项目依赖 `Inno.Core.Events` 与 `Inno.Core.Execution`，不导出 Scripting API。
Execution 只提供统一 Pending 退休信号；Stack 不创建自己的 generation gate 或设备。

## 公开 API

| API | 稳定语义 |
| --- | --- |
| `Layer` | 可派生的生命周期参与者；订阅在每次 attachment 内自动释放 |
| `Layer.name` | 诊断名称，不作为持久 ID 或类型 ID |
| `OnAttach` / `OnDetach` | 进入和离开一个 LayerStack attachment |
| `OnFixedUpdate` / `OnUpdate` / `OnLateUpdate` | 固定步、普通帧和迟更新阶段 |
| `OnBeforeRender` / `OnRender` / `OnAfterRender` | 准备、提交和逆序释放 render frame ownership |
| `Listen<T>` / `ListenOnce<T>` | 在当前 Layer 的独立 EventHub 中订阅，Detach 时自动释放 |
| `Announce` | 只向当前 Layer 自身 EventHub 同步分发 |
| `LayerStack` | 拥有 Base Layer 与 Overlay 的顺序、事件 scope 和 attachment lifetime |
| `PushLayer` / `PopLayer` | 管理 Overlay 下方的 base region |
| `PushOverlay` / `PopOverlay` | 管理 stack 顶部的 overlay region |
| `Clear` / `Dispose` | 逆序 Detach；`Clear` 后可复用，`Dispose` 后永久关闭 |

## 顺序模型

- Base Layer 总在 Overlay 之前；后加入 Base Layer 会插在现有 Overlay 下方。
- Update、LateUpdate、Render preparation 和 Render submission 从底到顶执行。
- `OnAfterRender` 只对成功完成 `OnBeforeRender` 的 Layer 执行，并从顶到底 unwind。
- 每个 Layer 使用独立 `EventHub`；顶层 Layer 的 hub order 更高，因此跨 Hub event 优先到达 Overlay。
- 同一个 Layer 实例不能重复加入一个 Stack；Layer 也不能同时持有两个 attachment。

## 常见工作流

```csharp
using System;

using Inno.Core.Events;
using Inno.Core.Layers;

var dispatcher = new EventDispatcher();
using var layers = new LayerStack(() => dispatcher.CreateHub());
layers.PushOverlay(new ConsoleOverlay());
layers.OnUpdate(1f / 60f);
layers.RenderFrame(1f / 60f);

public sealed class ConsoleOverlay : Layer
{
    public ConsoleOverlay()
        : base("Console")
    {
    }

    public override void OnAttach()
    {
        _ = Listen<ApplicationQuitEvent>(OnQuitRequested);
    }

    public override void OnUpdate(float deltaTime)
    {
        // Update backend-neutral overlay state.
    }

    private static void OnQuitRequested(ApplicationQuitEvent evnt)
    {
        evnt.HandleInHub();
    }
}
```

## 错误与生命周期

- `OnAttach` 失败时，已登记 entry 仍由 Stack 拥有，并执行 `OnDetach` 补偿部分启动。补偿完成后才移除 entry 和 EventHub。
- `RetirementPendingException` 表示依赖仍在使用：保留当前 entry、EventHub 和所有下层资源，拒绝 Push/Pop/帧调用；通过 `Clear` 或 `Dispose` 继续退休。Stack 本身不阻塞等待，最外层 Host 使用统一 `RetirementBarrier` 提供 deadline。
- Detach 开始先注销事件订阅，避免退出中的 Layer 再接收事件。`OnDetach` 的已完成步骤须可重试，Pending 不清除 attachment owner。
- 普通清理异常表示本步骤已结束但失败，后续资源仍逆序尝试；此前的错误跨 Pending 保留，最终统一报告，不重复退休已移除的 entry。
- Render callback 失败后仍会逆序执行所有已完成 prepare 的 `OnAfterRender`。
- Dispose 完成后幂等；完成前 Pending 不能标为 disposed。Dispose 后调用 mutation 或 frame API 会抛出 `ObjectDisposedException`。Clear 完整退出后才恢复可复用状态。
- Layer 不跨 Plugin generation 保存 runtime object；需要跨代恢复的业务状态仍应使用 Stable ID 与中立 bytes。

## 与 Shell、Runtime Subsystem 的关系

`LayerStack` 是通用的局部顺序容器。`Inno.Shell` 是 application-level Host 基类；`IRuntimeSubsystem` 是
RuntimeSession 的正式 feature pipeline。三者不能互相替代：Editor 可以在 Shell 的 `OnFrame` 中使用
LayerStack 排列 presentation layer，但 Runtime feature 的 attach/safe-point/detach 仍由 RuntimeSession 管理。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Core.Layers.Layer`

<!-- api: T:Inno.Core.Layers.Layer 4960b81f83d9528c244a7562a99e56660616f166842a8535411d68e07cb7e20e -->
<!-- api: M:Inno.Core.Layers.Layer.#ctor(System.String) 827c878facaf20829ae56e7896fb99c1a701d584fbc97f71c940a02da887c18d -->
<!-- api: P:Inno.Core.Layers.Layer.name d9f72e0e77f370f1d9c325e3ea77c3893085f3c1a6f58115e7dd591e7ae908e3 -->
<!-- api: M:Inno.Core.Layers.Layer.OnAttach ecfc48fd9604cdb386ec07f4754e04c92f4447731d5f75b4488ffda7f753e78b -->
<!-- api: M:Inno.Core.Layers.Layer.OnDetach 301243b410da67904df00c7f686ba83f589a9cc08f6c75e088cf019198376aba -->
<!-- api: M:Inno.Core.Layers.Layer.OnFixedUpdate(System.Single) 1705c922dc70526dee18a91893484ff088c0f57c860a99e4afd26ac258881027 -->
<!-- api: M:Inno.Core.Layers.Layer.OnUpdate(System.Single) ecfe97b150bcd63f294d96397d7e32da3916a97d66a86b8c0d131cc842778013 -->
<!-- api: M:Inno.Core.Layers.Layer.OnLateUpdate(System.Single) fa084b1776a87129aa957865dfd8090c7c2d7260fe93a9c779c14c567288e2c5 -->
<!-- api: M:Inno.Core.Layers.Layer.OnBeforeRender(System.Single) 4f7cb4bf29ddfc49bef5458852c8729ea3248c706e176422903d718fb12610f9 -->
<!-- api: M:Inno.Core.Layers.Layer.OnRender(System.Single) 050a802803b7d534c8ff732fb95789c2cdae2d034fa844b9b56fa3deb575fddf -->
<!-- api: M:Inno.Core.Layers.Layer.OnAfterRender(System.Single) 533b49f794e9bd088d37a76f2a9bdedc180767e214d3fae1a7a1c6a1b1ad4f91 -->
<!-- api: M:Inno.Core.Layers.Layer.Listen``1(System.Action{``0},System.Int32) 9843c2aaeb3834410a9875ab21cc48a55e848f7fca50f087fb317d457d0e0895 -->
<!-- api: M:Inno.Core.Layers.Layer.ListenOnce``1(System.Action{``0},System.Int32) bcf5661d3cc352eeb9cea971514e8a88ee890f4970fe4599b034926a561ce3c5 -->
<!-- api: M:Inno.Core.Layers.Layer.Announce(Inno.Core.Events.Event) 32749dbdb9a4d4eae45e3743f4efaf24fb44cf4e27ecc8a9405611adc14437d8 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d04245ff34a54c67"></a>[`protected Inno.Core.Layers.Layer.Layer(string name = "Layer")`](../../src/foundation/core/Inno.Core.Layers/Layer.cs#L29) | Creates a layer with the supplied diagnostic name.<br>`name`：The non-empty name used to identify the layer in diagnostics. | — | `System.ArgumentException`：Thrown when name is empty or consists only of whitespace. |
| <a id="api-c18fec37c96c4fa8"></a>[`protected void Inno.Core.Layers.Layer.Announce(Inno.Core.Events.Event evnt)`](../../src/foundation/core/Inno.Core.Layers/Layer.cs#L193) | Dispatches an event immediately inside this layer's event scope.<br>`evnt`：The event to dispatch to this layer's listeners. | — | `System.InvalidOperationException`：Thrown when the layer is not currently attached to a stack. |
| <a id="api-be06bae0a66ab863"></a>[`protected System.IDisposable Inno.Core.Layers.Layer.Listen<TEvent>(System.Action<TEvent> handler, int priority = 0) where TEvent : Inno.Core.Events.Event`](../../src/foundation/core/Inno.Core.Layers/Layer.cs#L142) | Subscribes to an event for the lifetime of this layer attachment.<br>返回：A token that can remove the subscription before the layer detaches.<br>`handler`：The callback invoked when the event reaches this layer.<br>`priority`：The listener priority inside this layer. Higher values run earlier. | — | `System.InvalidOperationException`：Thrown when the layer is not currently attached to a stack. |
| <a id="api-7be0af5e32efd78e"></a>[`protected System.IDisposable Inno.Core.Layers.Layer.ListenOnce<TEvent>(System.Action<TEvent> handler, int priority = 0) where TEvent : Inno.Core.Events.Event`](../../src/foundation/core/Inno.Core.Layers/Layer.cs#L172) | Subscribes once to an event for the lifetime of this layer attachment.<br>返回：A token that can cancel the one-shot subscription before it runs.<br>`handler`：The callback invoked at most once when the event reaches this layer.<br>`priority`：The listener priority inside this layer. Higher values run earlier. | — | `System.InvalidOperationException`：Thrown when the layer is not currently attached to a stack. |
| <a id="api-6cd1c785f8be2478"></a>[`public virtual void Inno.Core.Layers.Layer.OnAfterRender(float deltaTime)`](../../src/foundation/core/Inno.Core.Layers/Layer.cs#L120) | Completes frame-scoped rendering state after render submission.<br>`deltaTime`：The elapsed frame time in seconds. | A Inno.Core.Layers.LayerStack invokes this callback in reverse order for every layer whose Inno.Core.Layers.Layer.OnBeforeRender(System.Single) callback completed successfully, including when later work fails. | — |
| <a id="api-497693988c29136c"></a>[`public virtual void Inno.Core.Layers.Layer.OnAttach()`](../../src/foundation/core/Inno.Core.Layers/Layer.cs#L44) | Runs after the layer has received its event scope and joined a stack. | — | — |
| <a id="api-36c7d6df5ff5e787"></a>[`public virtual void Inno.Core.Layers.Layer.OnBeforeRender(float deltaTime)`](../../src/foundation/core/Inno.Core.Layers/Layer.cs#L96) | Prepares frame-scoped rendering state before render work is submitted.<br>`deltaTime`：The elapsed frame time in seconds. | — | — |
| <a id="api-7a30f41d1d037442"></a>[`public virtual void Inno.Core.Layers.Layer.OnDetach()`](../../src/foundation/core/Inno.Core.Layers/Layer.cs#L56) | Runs while the layer is leaving its stack and before its event scope is released. | This callback also compensates a partially failed attachment. Throwing Inno.Core.Execution.RetirementPendingException retains ownership and permits a later cleanup retry. Completed cleanup steps must not be repeated on that retry. | — |
| <a id="api-da04fda6fcc03afa"></a>[`public virtual void Inno.Core.Layers.Layer.OnFixedUpdate(float fixedDeltaTime)`](../../src/foundation/core/Inno.Core.Layers/Layer.cs#L66) | Advances fixed-step work for this layer.<br>`fixedDeltaTime`：The fixed simulation interval in seconds. | — | — |
| <a id="api-c49b4e3fb3e57adb"></a>[`public virtual void Inno.Core.Layers.Layer.OnLateUpdate(float deltaTime)`](../../src/foundation/core/Inno.Core.Layers/Layer.cs#L86) | Advances work that must run after the regular update phase.<br>`deltaTime`：The elapsed frame time in seconds. | — | — |
| <a id="api-beb80e26c5754f6c"></a>[`public virtual void Inno.Core.Layers.Layer.OnRender(float deltaTime)`](../../src/foundation/core/Inno.Core.Layers/Layer.cs#L106) | Submits rendering work without presenting a concrete graphics backend.<br>`deltaTime`：The elapsed frame time in seconds. | — | — |
| <a id="api-e335f3baa944f800"></a>[`public virtual void Inno.Core.Layers.Layer.OnUpdate(float deltaTime)`](../../src/foundation/core/Inno.Core.Layers/Layer.cs#L76) | Advances variable-step work for this layer.<br>`deltaTime`：The elapsed frame time in seconds. | — | — |
| <a id="api-86096bb75f31ceea"></a>[`public string Inno.Core.Layers.Layer.name { get; }`](../../src/foundation/core/Inno.Core.Layers/Layer.cs#L39) | Gets the diagnostic name of this layer. | — | — |
| <a id="api-8a83c4cbccb84691"></a>[`public abstract class Inno.Core.Layers.Layer`](../../src/foundation/core/Inno.Core.Layers/Layer.cs#L13) | Defines a backend-neutral lifecycle participant that can receive frame callbacks and own event subscriptions. | — | — |

### `Inno.Core.Layers.LayerStack`

<!-- api: T:Inno.Core.Layers.LayerStack d3cc022e7fcef800dc941922ec9f6fba7b3b71d1dbc0213d41fce8d66a1d0bed -->
<!-- api: M:Inno.Core.Layers.LayerStack.#ctor(System.Func{Inno.Core.Events.EventHub}) 8e62ab711ca93457d7dca0a7238e6dda2652097fd1c6edeb8c5b689f73873037 -->
<!-- api: P:Inno.Core.Layers.LayerStack.count 8b00e14362c7c7bebbf99689fff557c049715999b84403697eccebf85aa8eef4 -->
<!-- api: P:Inno.Core.Layers.LayerStack.Item(System.Int32) 39536263e82d5504dde5955c1280eb461b781ab1561142cc4a612180b7e65d88 -->
<!-- api: M:Inno.Core.Layers.LayerStack.PushLayer(Inno.Core.Layers.Layer) 2d6a458deef33434742fad0788d00346bc2bb9b1b3d3d870341d7171b8d8aa46 -->
<!-- api: M:Inno.Core.Layers.LayerStack.PushOverlay(Inno.Core.Layers.Layer) 593bf31a51f903daf33c60ad875994aab112546440f51a9be071baac5861f65d -->
<!-- api: M:Inno.Core.Layers.LayerStack.PopLayer(Inno.Core.Layers.Layer) ff79217749c290b6ea9d4233f8bde620788be9deaf640609a188df58c60ae94f -->
<!-- api: M:Inno.Core.Layers.LayerStack.PopOverlay(Inno.Core.Layers.Layer) ee8c0fc512083af98c2e320c75d6ee5cc970feec579fd972c3591b07426047b8 -->
<!-- api: M:Inno.Core.Layers.LayerStack.OnFixedUpdate(System.Single) 2732a4f343b1988295f38eb5b02e52c72ac6186f241e29c7b83ca74f34b8d075 -->
<!-- api: M:Inno.Core.Layers.LayerStack.OnUpdate(System.Single) 8a5311937be668c72575a652a5bdd639cb1bacb4e6f08462559ce36928fb0586 -->
<!-- api: M:Inno.Core.Layers.LayerStack.OnLateUpdate(System.Single) 3440634fa5bf60e32f1ab208728fe3093a409ac65e9af6f68a23839ecf3f93b7 -->
<!-- api: M:Inno.Core.Layers.LayerStack.RenderFrame(System.Single) d98fb3ba3feed73adaea42c7382bbf9b5e7782cbed63847bb5356963baa94761 -->
<!-- api: M:Inno.Core.Layers.LayerStack.Clear 8ea9b77b9244ec4c985133d4b8cc7477b87352f707b24c50fed75cc3725012f1 -->
<!-- api: M:Inno.Core.Layers.LayerStack.Dispose e24519f284c71960b03765282b78b65af4cca9c4abc352a57ea2539455a3b810 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1d6b75b4b9b8e34d"></a>[`public Inno.Core.Layers.LayerStack.LayerStack(System.Func<Inno.Core.Events.EventHub> eventHubFactory)`](../../src/foundation/core/Inno.Core.Layers/LayerStack.cs#L31) | Creates a layer stack that obtains one event scope for each attachment.<br>`eventHubFactory`：The factory used to create a distinct event hub for every attached layer. | — | `System.ArgumentNullException`：Thrown when eventHubFactory is null. |
| <a id="api-76190adff81bfab2"></a>[`public void Inno.Core.Layers.LayerStack.Clear()`](../../src/foundation/core/Inno.Core.Layers/LayerStack.cs#L251) | Detaches every layer in top-to-bottom order while keeping this stack reusable. | — | `Inno.Core.Execution.RetirementPendingException`：Cleanup is incomplete. The remaining layers stay owned; only Clear or Dispose may resume cleanup.<br>`System.AggregateException`：Thrown after cleanup completes when multiple layers fail to detach.<br>`System.ObjectDisposedException`：Thrown when this stack has been disposed. |
| <a id="api-6c896dfd09e1a44e"></a>[`public void Inno.Core.Layers.LayerStack.Dispose()`](../../src/foundation/core/Inno.Core.Layers/LayerStack.cs#L268) | Detaches every layer and permanently releases this stack. | — | `Inno.Core.Execution.RetirementPendingException`：A layer still owns live work. The current and lower layers remain owned until disposal is retried.<br>`System.AggregateException`：Thrown after cleanup completes when multiple layers fail to detach. |
| <a id="api-e3481f49e1875c5a"></a>[`public void Inno.Core.Layers.LayerStack.OnFixedUpdate(float fixedDeltaTime)`](../../src/foundation/core/Inno.Core.Layers/LayerStack.cs#L153) | Advances fixed-step callbacks in bottom-to-top stack order.<br>`fixedDeltaTime`：The fixed simulation interval in seconds. | — | `System.ObjectDisposedException`：Thrown when this stack has been disposed. |
| <a id="api-2e9c6bdcbc57b40c"></a>[`public void Inno.Core.Layers.LayerStack.OnLateUpdate(float deltaTime)`](../../src/foundation/core/Inno.Core.Layers/LayerStack.cs#L185) | Advances late-update callbacks in bottom-to-top stack order.<br>`deltaTime`：The elapsed frame time in seconds. | — | `System.ObjectDisposedException`：Thrown when this stack has been disposed. |
| <a id="api-c44463e4ac01bd74"></a>[`public void Inno.Core.Layers.LayerStack.OnUpdate(float deltaTime)`](../../src/foundation/core/Inno.Core.Layers/LayerStack.cs#L169) | Advances variable-step callbacks in bottom-to-top stack order.<br>`deltaTime`：The elapsed frame time in seconds. | — | `System.ObjectDisposedException`：Thrown when this stack has been disposed. |
| <a id="api-d4b2fa2c899e7acd"></a>[`public bool Inno.Core.Layers.LayerStack.PopLayer(Inno.Core.Layers.Layer layer)`](../../src/foundation/core/Inno.Core.Layers/LayerStack.cs#L107) | Detaches a base layer from this stack.<br>返回：true when the layer was attached in the base region; otherwise, false.<br>`layer`：The base layer to detach. | — | `System.ArgumentNullException`：Thrown when layer is null.<br>`System.ObjectDisposedException`：Thrown when this stack has been disposed. |
| <a id="api-2fdda75a0568f717"></a>[`public bool Inno.Core.Layers.LayerStack.PopOverlay(Inno.Core.Layers.Layer overlay)`](../../src/foundation/core/Inno.Core.Layers/LayerStack.cs#L133) | Detaches an overlay from this stack.<br>返回：true when the layer was attached in the overlay region; otherwise, false.<br>`overlay`：The overlay to detach. | — | `System.ArgumentNullException`：Thrown when overlay is null.<br>`System.ObjectDisposedException`：Thrown when this stack has been disposed. |
| <a id="api-ac53ab5b73fe5278"></a>[`public void Inno.Core.Layers.LayerStack.PushLayer(Inno.Core.Layers.Layer layer)`](../../src/foundation/core/Inno.Core.Layers/LayerStack.cs#L70) | Attaches a base layer below all overlays.<br>`layer`：The layer to attach. | — | `System.ArgumentNullException`：Thrown when layer is null.<br>`System.InvalidOperationException`：Thrown when the same instance is already attached or the event hub factory returns null.<br>`System.ObjectDisposedException`：Thrown when this stack has been disposed. |
| <a id="api-3164e299531ceb5e"></a>[`public void Inno.Core.Layers.LayerStack.PushOverlay(Inno.Core.Layers.Layer overlay)`](../../src/foundation/core/Inno.Core.Layers/LayerStack.cs#L90) | Attaches an overlay above all base layers and existing overlays.<br>`overlay`：The overlay to attach. | — | `System.ArgumentNullException`：Thrown when overlay is null.<br>`System.InvalidOperationException`：Thrown when the same instance is already attached or the event hub factory returns null.<br>`System.ObjectDisposedException`：Thrown when this stack has been disposed. |
| <a id="api-3094fff4f50a3b89"></a>[`public void Inno.Core.Layers.LayerStack.RenderFrame(float deltaTime)`](../../src/foundation/core/Inno.Core.Layers/LayerStack.cs#L204) | Executes preparation and submission in stack order, then completion in reverse order.<br>`deltaTime`：The elapsed frame time in seconds. | — | `System.AggregateException`：Thrown when multiple render or unwind callbacks fail.<br>`System.ObjectDisposedException`：Thrown when this stack has been disposed. |
| <a id="api-0c7537d94ad1eb9e"></a>[`public int Inno.Core.Layers.LayerStack.count { get; }`](../../src/foundation/core/Inno.Core.Layers/LayerStack.cs#L39) | Gets the number of attached base layers and overlays. | — | — |
| <a id="api-2a5f7a32be804c30"></a>[`public Inno.Core.Layers.Layer Inno.Core.Layers.LayerStack.this[int index] { get; }`](../../src/foundation/core/Inno.Core.Layers/LayerStack.cs#L53) | Gets the layer at the supplied stack position.<br>返回：The attached layer at the requested position.<br>`index`：The zero-based stack position. | — | `System.ArgumentOutOfRangeException`：Thrown when index is outside the current stack. |
| <a id="api-e301efda57467678"></a>[`public sealed class Inno.Core.Layers.LayerStack : System.IDisposable`](../../src/foundation/core/Inno.Core.Layers/LayerStack.cs#L13) | Owns an ordered collection of base layers and overlays with isolated event scopes. | — | — |

## 项目依赖

- [Inno.Core.Execution](Inno.Core.Execution.md)：公开引用边界由实际签名核对。
- [Inno.Core.Events](Inno.Core.Events.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
