# Inno.Runtime.Contracts

## Pending 与终态清理错误

`RuntimeSubsystem.Dispose` 先取消 tracked work；未静止时抛出 Foundation 的 `RetirementPendingException`，不执行 OnStop。Pipeline/Session/Host 必须保留该 owner 与依赖，完成后重试。不能将 pending 包装成普通 AggregateException 后继续销毁依赖。真正执行 OnStop 后发生的清理错误才聚合报告。

这一协议不承诺能强制终止任意插件线程，也不允许在后台线程释放控制线程资源。

[Runtime 索引](README.md) · [Wiki 首页](../README.md) · [Runtime owner](Inno.Runtime.md) · [默认装配](Inno.Engine.Default.md)

## 职责与依赖

这是引擎子系统的中立生命周期协议，只依赖 Foundation 的 Events、Diagnostics、Execution、Identity 与 TypeCatalog。它不引用 Scene、Assets、Rendering、Audio、Editor 或 backend。领域 Runtime 引用本项目，不引用包含 Scene/部署逻辑的 `Inno.Runtime`。

## 公开 API

| 契约 | 稳定语义 |
| --- | --- |
| `IRuntimeSubsystem` | Attach、BeginFrame、FixedUpdate、Update、LateUpdate、BeforeRender、Render、AfterRender、EndFrame、Detach、Dispose 的有限生命周期 |
| `RuntimeSubsystem` | 上述非虚公开入口的模板实现；统一 owner thread、frame scope、异常退出、取消与逆序释放 |
| `IRuntimeSubsystemFactory.descriptor` / `Create(context)` | 声明稳定身份、依赖和 owner 范围，构造独立实例 |
| `RuntimeSubsystemId` | 开放 semantic ID；不是 Object Identity 或 CLR 类型名 |
| `RuntimeSubsystemDescriptor` | 冻结 id、order、dependencies、lifetime、requirement、requiredCapabilities；拒绝非法 enum、空 ID、自依赖和重复声明 |
| `RuntimeSubsystemRequirement.Required/Optional` | Required 失败阻止 owner 启动；Optional 只有完成补偿后才能进入 Unavailable，不能吞清理失败 |
| `RuntimeCapabilityId` | Composition 验证后提供的开放、区分大小写能力 ID，不是 backend enum 或 live object Identity |
| `RuntimeSubsystemLifetime` | Host 与 Session 是不同资源所有权，不能把 Host GPU runtime 塞进 Session pipeline |
| `RuntimeFrame` / `RuntimeFixedFrame` | 中立时钟值；不携带 Session、Scene 或设备 |
| `RuntimeSubsystemContext` | 显式 events、diagnostics、identities、types、resources、lifetime、isEditMode、冻结 capabilities；不是任意服务查询容器，不携带物理持久目录 |
| `RuntimeSubsystemRegistrationAttribute(id)` | 在发行程序集声明一个强类型 factory 方法 |
| `RuntimeSubsystemCatalogAttribute` | 要求构建期生成同一 composition 参数类型的 catalog |

## 派生扩展点

`OnStart` / `OnStop` 处理领域资源；`OnBeginFrame`、`OnFixedUpdate`、`OnUpdate`、`OnLateUpdate`、`OnPrepareOutput`、`OnProduceOutput`、`OnCompleteOutput`、`OnEndFrame` 处理有限帧阶段。`OwnFrameScope` 登记必须退出的 façade binding；`lifetime` 跟踪该子系统自己的资源、取消与工作。

```csharp
using Inno.Runtime.Contracts;

public sealed class CounterRuntime : RuntimeSubsystem
{
    public double elapsed { get; private set; }

    protected override void OnUpdate(RuntimeFrame frame)
    {
        elapsed += frame.unscaledDeltaTime;
    }
}
```

示例省略 XML 注释；实际公开实现必须补齐。`AudioRuntime : RuntimeSubsystem, IAudioService` 组合 `IAudioDevice`；不能为了句柄编码继承 AudioDevice。Layer/LayerStack 留在 Core，负责局部 UI/应用层顺序；Pipeline/Mixer Feature 只贡献领域行为。

## 失败与生命周期

重复 Attach、错误线程、无 BeginFrame 的阶段调用会失败。帧入口失败会关闭已进入的作用域；Dispose 尝试逆序资源清理并聚合错误。尚未完成的 tracked work 不允许被当作已释放，必须先取消并排空；模板不是线程强杀器。

完整 DAG 排序、缺失依赖/环检测与逆序阶段展开由 `Inno.Runtime` 的 `RuntimeSubsystemPipeline` 实现。GenerationCoordinator 管 collectible 扩展代际，Subsystem 不建立第二套 GC barrier。

Pipeline/Session **先被 Host 持有，再运行 factory/Attach**。每个 factory 的 `context.resources` 是独立 lifetime，
可选系统失败不会释放其他系统的资源。Factory 必须将尚未交给返回实例的资源与 Task 登记到该 lifetime；
引擎无法自动接管用户在 factory 内部偷偷保存到静态字段的资源。

`Attach` 失败不自行进行一次不完整的 Dispose；外层 owner 负责完整排空。`OnStop` 抛 Pending 时允许重试，
其完成后即使后续 lifetime Pending，也不会再次执行已完成的 `OnStop`。

## 能力与失败政策

```csharp
using Inno.Runtime.Contracts;

var descriptor = new RuntimeSubsystemDescriptor(
    new RuntimeSubsystemId("sample.capture"),
    lifetime: RuntimeSubsystemLifetime.Host,
    requirement: RuntimeSubsystemRequirement.Optional,
    requiredCapabilities: [new RuntimeCapabilityId("sample.frame-capture")]);
```

能力集合由已经检查设备/服务的 Composition 传入，Runtime 不猜测当前 OS 或 backend。
缺失能力/依赖不会调用该 factory。Optional 依赖不可用时，它的 Optional consumer 同样不可用，
Required consumer 则拒绝 owner。重复 ID、cycle、生命周期错误始终是配置错误，Optional 不豁免。
运行帧异常和退休异常不属于 Optional 的降级范围；当前政策明确传播失败，不提供自动重启/静默跳帧。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Runtime.Contracts.IRuntimeSubsystem`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Runtime.Contracts.IRuntimeSubsystem`](../../src/runtime/contracts/Inno.Runtime.Contracts/IRuntimeSubsystem.cs#L8) | Participates in the ordered lifecycle of one isolated runtime session. |
| [`void Inno.Runtime.Contracts.IRuntimeSubsystem.AfterRender(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/IRuntimeSubsystem.cs#L69) | Finalizes frame output even when rendering fails. |
| [`void Inno.Runtime.Contracts.IRuntimeSubsystem.Attach()`](../../src/runtime/contracts/Inno.Runtime.Contracts/IRuntimeSubsystem.cs#L13) | Acquires session resources after every dependency has attached. |
| [`void Inno.Runtime.Contracts.IRuntimeSubsystem.BeforeRender(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/IRuntimeSubsystem.cs#L53) | Prepares frame output after simulation has completed. |
| [`void Inno.Runtime.Contracts.IRuntimeSubsystem.BeginFrame(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/IRuntimeSubsystem.cs#L21) | Begins one frame before event dispatch and simulation. |
| [`void Inno.Runtime.Contracts.IRuntimeSubsystem.Detach()`](../../src/runtime/contracts/Inno.Runtime.Contracts/IRuntimeSubsystem.cs#L82) | Releases attached session resources before dependencies detach. |
| [`void Inno.Runtime.Contracts.IRuntimeSubsystem.EndFrame(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/IRuntimeSubsystem.cs#L77) | Ends one frame and releases frame-scoped state. |
| [`void Inno.Runtime.Contracts.IRuntimeSubsystem.FixedUpdate(Inno.Runtime.Contracts.RuntimeFixedFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/IRuntimeSubsystem.cs#L29) | Advances one deterministic simulation step. |
| [`void Inno.Runtime.Contracts.IRuntimeSubsystem.LateUpdate(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/IRuntimeSubsystem.cs#L45) | Advances state that depends on completed variable simulation. |
| [`void Inno.Runtime.Contracts.IRuntimeSubsystem.Render(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/IRuntimeSubsystem.cs#L61) | Produces frame output owned by this subsystem. |
| [`void Inno.Runtime.Contracts.IRuntimeSubsystem.Update(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/IRuntimeSubsystem.cs#L37) | Advances variable simulation state. |

### `Inno.Runtime.Contracts.IRuntimeSubsystemFactory`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Runtime.Contracts.IRuntimeSubsystem Inno.Runtime.Contracts.IRuntimeSubsystemFactory.Create(Inno.Runtime.Contracts.RuntimeSubsystemContext context)`](../../src/runtime/contracts/Inno.Runtime.Contracts/IRuntimeSubsystemFactory.cs#L22) | Creates a subsystem owned exclusively by the supplied construction scope. |
| [`Inno.Runtime.Contracts.IRuntimeSubsystemFactory`](../../src/runtime/contracts/Inno.Runtime.Contracts/IRuntimeSubsystemFactory.cs#L6) | Creates one runtime subsystem instance for each declared Host or Session owner. |
| [`Inno.Runtime.Contracts.RuntimeSubsystemDescriptor Inno.Runtime.Contracts.IRuntimeSubsystemFactory.descriptor`](../../src/runtime/contracts/Inno.Runtime.Contracts/IRuntimeSubsystemFactory.cs#L11) | Gets stable ordering and dependency metadata without creating runtime state. |

### `Inno.Runtime.Contracts.RuntimeCapabilityId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Runtime.Contracts.RuntimeCapabilityId`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeCapabilityId.cs#L8) | Identifies a backend-neutral capability supplied by composition, not an object or native handle. |
| [`Inno.Runtime.Contracts.RuntimeCapabilityId.RuntimeCapabilityId(string value)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeCapabilityId.cs#L19) | Creates an open capability protocol identifier. |
| [`bool Inno.Runtime.Contracts.RuntimeCapabilityId.isValid`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeCapabilityId.cs#L33) | Gets whether this value contains a usable capability name. |
| [`override string Inno.Runtime.Contracts.RuntimeCapabilityId.ToString()`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeCapabilityId.cs#L41) | Formats the capability for diagnostics. |
| [`string Inno.Runtime.Contracts.RuntimeCapabilityId.value`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeCapabilityId.cs#L28) | Gets the stable capability name, without interpreting it as a backend name. |

### `Inno.Runtime.Contracts.RuntimeFixedFrame`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Runtime.Contracts.RuntimeFixedFrame`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFixedFrame.cs#L6) | Provides immutable timing state to one deterministic fixed simulation step. |
| [`Inno.Runtime.Contracts.RuntimeFixedFrame.RuntimeFixedFrame(long stepIndex, float time, float deltaTime)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFixedFrame.cs#L20) | Creates one deterministic fixed-step timing value. |
| [`float Inno.Runtime.Contracts.RuntimeFixedFrame.deltaTime`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFixedFrame.cs#L43) | Gets the configured fixed simulation interval in seconds. |
| [`float Inno.Runtime.Contracts.RuntimeFixedFrame.time`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFixedFrame.cs#L38) | Gets accumulated fixed simulation time in seconds. |
| [`long Inno.Runtime.Contracts.RuntimeFixedFrame.stepIndex`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFixedFrame.cs#L33) | Gets the zero-based fixed-step index. |

### `Inno.Runtime.Contracts.RuntimeFrame`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Runtime.Contracts.RuntimeFrame`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFrame.cs#L8) | Provides immutable timing state to one variable runtime frame. |
| [`Inno.Runtime.Contracts.RuntimeFrame.RuntimeFrame(long frameIndex, float time, float unscaledTime, float deltaTime, float unscaledDeltaTime, float timeScale, bool isPaused)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFrame.cs#L34) | Creates the immutable timing snapshot for one variable frame. |
| [`bool Inno.Runtime.Contracts.RuntimeFrame.isPaused`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFrame.cs#L85) | Gets whether scaled simulation is paused for this frame. |
| [`float Inno.Runtime.Contracts.RuntimeFrame.deltaTime`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFrame.cs#L70) | Gets the scaled variable frame interval in seconds. |
| [`float Inno.Runtime.Contracts.RuntimeFrame.time`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFrame.cs#L60) | Gets accumulated scaled session time in seconds. |
| [`float Inno.Runtime.Contracts.RuntimeFrame.timeScale`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFrame.cs#L80) | Gets the simulation time multiplier used for this frame. |
| [`float Inno.Runtime.Contracts.RuntimeFrame.unscaledDeltaTime`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFrame.cs#L75) | Gets the unscaled variable frame interval in seconds. |
| [`float Inno.Runtime.Contracts.RuntimeFrame.unscaledTime`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFrame.cs#L65) | Gets accumulated unscaled session time in seconds. |
| [`long Inno.Runtime.Contracts.RuntimeFrame.frameIndex`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFrame.cs#L55) | Gets the zero-based session frame index. |

### `Inno.Runtime.Contracts.RuntimeSubsystem`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Execution.LifetimeScope Inno.Runtime.Contracts.RuntimeSubsystem.lifetime`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L30) | Gets resources and cancellation owned exclusively by this subsystem. |
| [`Inno.Runtime.Contracts.RuntimeSubsystem`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L10) | Enforces one control-thread lifecycle while derived runtimes compose their own domain services. |
| [`bool Inno.Runtime.Contracts.RuntimeSubsystem.isStarted`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L25) | Gets whether this instance is attached to an owner and has not retired. |
| [`virtual void Inno.Runtime.Contracts.RuntimeSubsystem.OnBeginFrame(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L235) | Captures snapshots and binds service façades. |
| [`virtual void Inno.Runtime.Contracts.RuntimeSubsystem.OnCompleteOutput(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L277) | Submits output and closes output-specific temporary resources. |
| [`virtual void Inno.Runtime.Contracts.RuntimeSubsystem.OnEndFrame(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L284) | Finalizes control-thread frame state before execution bindings are revoked. |
| [`virtual void Inno.Runtime.Contracts.RuntimeSubsystem.OnFixedUpdate(Inno.Runtime.Contracts.RuntimeFixedFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L242) | Advances one deterministic fixed step. |
| [`virtual void Inno.Runtime.Contracts.RuntimeSubsystem.OnLateUpdate(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L256) | Updates state that depends on completed simulation. |
| [`virtual void Inno.Runtime.Contracts.RuntimeSubsystem.OnPrepareOutput(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L263) | Opens resources required for this frame's output. |
| [`virtual void Inno.Runtime.Contracts.RuntimeSubsystem.OnProduceOutput(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L270) | Collects output commands without executing managed code on a native realtime callback. |
| [`virtual void Inno.Runtime.Contracts.RuntimeSubsystem.OnStart()`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L228) | Acquires resources after dependencies have started. |
| [`virtual void Inno.Runtime.Contracts.RuntimeSubsystem.OnStop()`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L288) | Releases domain resources, including allocations made before attachment. |
| [`virtual void Inno.Runtime.Contracts.RuntimeSubsystem.OnUpdate(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L249) | Advances domain state on the variable clock. |
| [`void Inno.Runtime.Contracts.RuntimeSubsystem.AfterRender(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L149) | Finalizes frame output even when rendering fails. |
| [`void Inno.Runtime.Contracts.RuntimeSubsystem.Attach()`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L41) | Acquires session resources after every dependency has attached. |
| [`void Inno.Runtime.Contracts.RuntimeSubsystem.BeforeRender(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L127) | Prepares frame output after simulation has completed. |
| [`void Inno.Runtime.Contracts.RuntimeSubsystem.BeginFrame(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L65) | Begins one frame before event dispatch and simulation. |
| [`void Inno.Runtime.Contracts.RuntimeSubsystem.Detach()`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L176) | Releases attached session resources before dependencies detach. |
| [`void Inno.Runtime.Contracts.RuntimeSubsystem.Dispose()`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L186) | Releases this owner's registrations and resources exactly once. |
| [`void Inno.Runtime.Contracts.RuntimeSubsystem.EndFrame(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L160) | Ends one frame and releases frame-scoped state. |
| [`void Inno.Runtime.Contracts.RuntimeSubsystem.FixedUpdate(Inno.Runtime.Contracts.RuntimeFixedFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L94) | Advances one deterministic simulation step. |
| [`void Inno.Runtime.Contracts.RuntimeSubsystem.LateUpdate(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L116) | Advances state that depends on completed variable simulation. |
| [`void Inno.Runtime.Contracts.RuntimeSubsystem.OwnFrameScope(System.IDisposable scope)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L218) | Registers a binding that must close even when a frame hook fails. |
| [`void Inno.Runtime.Contracts.RuntimeSubsystem.Render(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L138) | Produces frame output owned by this subsystem. |
| [`void Inno.Runtime.Contracts.RuntimeSubsystem.Update(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L105) | Advances variable simulation state. |

### `Inno.Runtime.Contracts.RuntimeSubsystemCatalogAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Runtime.Contracts.RuntimeSubsystemCatalogAttribute`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemCatalogAttribute.cs#L8) | Requests a generated partial method that gathers subsystem declarations with the same typed composition parameter. |

### `Inno.Runtime.Contracts.RuntimeSubsystemContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Diagnostics.DiagnosticHub Inno.Runtime.Contracts.RuntimeSubsystemContext.diagnostics`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemContext.cs#L77) | Gets the shared diagnostic state owner. |
| [`Inno.Core.Events.EventDispatcher Inno.Runtime.Contracts.RuntimeSubsystemContext.events`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemContext.cs#L73) | Gets the owner-thread event dispatcher. |
| [`Inno.Core.Execution.LifetimeScope Inno.Runtime.Contracts.RuntimeSubsystemContext.resources`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemContext.cs#L89) | Gets resources owned by this pipeline, not by a global container. |
| [`Inno.Core.Identity.IdentityAllocator Inno.Runtime.Contracts.RuntimeSubsystemContext.identities`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemContext.cs#L81) | Gets the isolated live-object identity domain. |
| [`Inno.Extensibility.Types.TypeCatalog Inno.Runtime.Contracts.RuntimeSubsystemContext.types`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemContext.cs#L85) | Gets the current foundation type catalog. |
| [`Inno.Runtime.Contracts.RuntimeSubsystemContext`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemContext.cs#L16) | Exposes only foundation services and owner metadata, never a complete RuntimeSession or domain service locator. |
| [`Inno.Runtime.Contracts.RuntimeSubsystemContext.RuntimeSubsystemContext(Inno.Core.Events.EventDispatcher events, Inno.Core.Diagnostics.DiagnosticHub diagnostics, Inno.Core.Identity.IdentityAllocator identities, Inno.Extensibility.Types.TypeCatalog types, Inno.Core.Execution.LifetimeScope resources, Inno.Runtime.Contracts.RuntimeSubsystemLifetime lifetime, bool isEditMode = false, System.Collections.Generic.IEnumerable<Inno.Runtime.Contracts.RuntimeCapabilityId>? capabilities = null)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemContext.cs#L48) | Creates the explicit construction boundary for a host or session pipeline. |
| [`Inno.Runtime.Contracts.RuntimeSubsystemLifetime Inno.Runtime.Contracts.RuntimeSubsystemContext.lifetime`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemContext.cs#L93) | Gets the owner scope used to validate factory lifetimes. |
| [`System.Collections.Generic.IReadOnlySet<Inno.Runtime.Contracts.RuntimeCapabilityId> Inno.Runtime.Contracts.RuntimeSubsystemContext.capabilities`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemContext.cs#L102) | Gets the immutable capabilities verified by the owner, never a mutable service container. |
| [`bool Inno.Runtime.Contracts.RuntimeSubsystemContext.isEditMode`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemContext.cs#L97) | Gets whether scaled simulation should be disabled for authoring. |

### `Inno.Runtime.Contracts.RuntimeSubsystemDescriptor`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Runtime.Contracts.RuntimeSubsystemDescriptor`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemDescriptor.cs#L10) | Describes stable ordering and dependency requirements for one runtime subsystem. |
| [`Inno.Runtime.Contracts.RuntimeSubsystemDescriptor.RuntimeSubsystemDescriptor(Inno.Runtime.Contracts.RuntimeSubsystemId id, int order = 0, System.Collections.Generic.IReadOnlyList<Inno.Runtime.Contracts.RuntimeSubsystemId>? dependencies = null, Inno.Runtime.Contracts.RuntimeSubsystemLifetime lifetime = Inno.Runtime.Contracts.RuntimeSubsystemLifetime.Session, Inno.Runtime.Contracts.RuntimeSubsystemRequirement requirement = Inno.Runtime.Contracts.RuntimeSubsystemRequirement.Required, System.Collections.Generic.IReadOnlyList<Inno.Runtime.Contracts.RuntimeCapabilityId>? requiredCapabilities = null)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemDescriptor.cs#L36) | Creates an immutable runtime subsystem descriptor. |
| [`Inno.Runtime.Contracts.RuntimeSubsystemId Inno.Runtime.Contracts.RuntimeSubsystemDescriptor.id`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemDescriptor.cs#L71) | Gets the unique subsystem protocol identifier. |
| [`Inno.Runtime.Contracts.RuntimeSubsystemLifetime Inno.Runtime.Contracts.RuntimeSubsystemDescriptor.lifetime`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemDescriptor.cs#L76) | Gets the exclusive owner scope required by this subsystem. |
| [`Inno.Runtime.Contracts.RuntimeSubsystemRequirement Inno.Runtime.Contracts.RuntimeSubsystemDescriptor.requirement`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemDescriptor.cs#L91) | Gets whether this subsystem must start for its owner to become available. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Runtime.Contracts.RuntimeCapabilityId> Inno.Runtime.Contracts.RuntimeSubsystemDescriptor.requiredCapabilities`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemDescriptor.cs#L96) | Gets the immutable capability prerequisites checked before allocating subsystem resources. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Runtime.Contracts.RuntimeSubsystemId> Inno.Runtime.Contracts.RuntimeSubsystemDescriptor.dependencies`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemDescriptor.cs#L86) | Gets the immutable required-subsystem identifiers. |
| [`int Inno.Runtime.Contracts.RuntimeSubsystemDescriptor.order`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemDescriptor.cs#L81) | Gets the deterministic order used after dependency constraints. |

### `Inno.Runtime.Contracts.RuntimeSubsystemId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Runtime.Contracts.RuntimeSubsystemId`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemId.cs#L8) | Identifies one backend-neutral runtime subsystem protocol. |
| [`Inno.Runtime.Contracts.RuntimeSubsystemId.RuntimeSubsystemId(string value)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemId.cs#L19) | Creates a stable runtime subsystem identifier. |
| [`bool Inno.Runtime.Contracts.RuntimeSubsystemId.isValid`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemId.cs#L33) | Gets whether this identifier contains a usable protocol value. |
| [`override string Inno.Runtime.Contracts.RuntimeSubsystemId.ToString()`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemId.cs#L41) | Formats the subsystem identifier for diagnostics. |
| [`string Inno.Runtime.Contracts.RuntimeSubsystemId.value`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemId.cs#L28) | Gets the stable protocol value. |

### `Inno.Runtime.Contracts.RuntimeSubsystemLifetime`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Runtime.Contracts.RuntimeSubsystemLifetime`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemLifetime.cs#L6) | Identifies the exclusive owner that creates and retires a stable engine subsystem. |
| [`Inno.Runtime.Contracts.RuntimeSubsystemLifetime.Host`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemLifetime.cs#L11) | The subsystem is shared by sessions and retires with the application host. |
| [`Inno.Runtime.Contracts.RuntimeSubsystemLifetime.Session`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemLifetime.cs#L15) | The subsystem belongs to one isolated Edit, Play or Player session. |

### `Inno.Runtime.Contracts.RuntimeSubsystemRegistrationAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Runtime.Contracts.RuntimeSubsystemRegistrationAttribute`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemRegistrationAttribute.cs#L8) | Declares a strongly typed composition method for build-time subsystem catalog generation. |
| [`Inno.Runtime.Contracts.RuntimeSubsystemRegistrationAttribute.RuntimeSubsystemRegistrationAttribute(string id)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemRegistrationAttribute.cs#L17) | Declares the stable ID returned by this method's subsystem factory. |
| [`string Inno.Runtime.Contracts.RuntimeSubsystemRegistrationAttribute.id`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemRegistrationAttribute.cs#L26) | Gets the stable descriptor ID checked by the generated catalog. |

### `Inno.Runtime.Contracts.RuntimeSubsystemRequirement`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Runtime.Contracts.RuntimeSubsystemRequirement`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemRequirement.cs#L6) | Selects startup failure behavior without changing runtime-frame or retirement error semantics. |
| [`Inno.Runtime.Contracts.RuntimeSubsystemRequirement.Optional`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemRequirement.cs#L16) | A fully compensated startup may remain unavailable with diagnostics; cleanup failures still reject the owner. |
| [`Inno.Runtime.Contracts.RuntimeSubsystemRequirement.Required`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemRequirement.cs#L11) | Missing capabilities, dependencies or failed startup reject the entire owner. |

## 项目依赖

- [Inno.Core.Events](../core/Inno.Core.Events.md)：公开引用边界由实际签名核对。
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：公开引用边界由实际签名核对。
- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：公开引用边界由实际签名核对。
- [Inno.Core.Identity](../core/Inno.Core.Identity.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
