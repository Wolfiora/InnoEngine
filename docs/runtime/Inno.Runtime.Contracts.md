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

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Runtime.Contracts.IRuntimeSubsystem`

<!-- api: T:Inno.Runtime.Contracts.IRuntimeSubsystem 93753e614ee597c8aa2f6d4eb3a3803f639aff99c0125aa802ccc9c02bbda61a -->
<!-- api: M:Inno.Runtime.Contracts.IRuntimeSubsystem.Attach 715a21cecd468bac039f2840e37191bcbf59f3194447a5e57f8bf14398d998c2 -->
<!-- api: M:Inno.Runtime.Contracts.IRuntimeSubsystem.BeginFrame(Inno.Runtime.Contracts.RuntimeFrame) 89ad63e504a249a8470b5ecaa77d348e628c309f450989a3cb2b9a6c9b000cca -->
<!-- api: M:Inno.Runtime.Contracts.IRuntimeSubsystem.FixedUpdate(Inno.Runtime.Contracts.RuntimeFixedFrame) 48e198d649b26b093b34ac71a1b6c5ccde4979cd2d5e3f46740486c79f43ff86 -->
<!-- api: M:Inno.Runtime.Contracts.IRuntimeSubsystem.Update(Inno.Runtime.Contracts.RuntimeFrame) 8d531af1600b5c72c21c9ab9314a050512d02b011e23638084902a3b18edb857 -->
<!-- api: M:Inno.Runtime.Contracts.IRuntimeSubsystem.LateUpdate(Inno.Runtime.Contracts.RuntimeFrame) 8947010d54249af687a392ddec3b2b1d32e334e46ec89cbd9b6254bb41c13733 -->
<!-- api: M:Inno.Runtime.Contracts.IRuntimeSubsystem.BeforeRender(Inno.Runtime.Contracts.RuntimeFrame) 32b517b6caf84ff7833b670472e9b3abe2a5134ce2a9f7ab03cde1c2d93a3ef4 -->
<!-- api: M:Inno.Runtime.Contracts.IRuntimeSubsystem.Render(Inno.Runtime.Contracts.RuntimeFrame) 0943e9bcbb198e9e6ab96bc12e606111bd114915c52b56ae7415949c34d624fd -->
<!-- api: M:Inno.Runtime.Contracts.IRuntimeSubsystem.AfterRender(Inno.Runtime.Contracts.RuntimeFrame) 04e6c087a97736d37600d96af8c83bf0028c271fb9c0a4f4d9c506777d827bf4 -->
<!-- api: M:Inno.Runtime.Contracts.IRuntimeSubsystem.EndFrame(Inno.Runtime.Contracts.RuntimeFrame) d6c17ab91017ea509b46e05c3a07bf189c11dde6020a4d29e322b3982b68dc9d -->
<!-- api: M:Inno.Runtime.Contracts.IRuntimeSubsystem.Detach b8728bf1c672a94b19a69fe5b174e68b621770faab2e66161553926fb0377be4 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-adc4365bffa7bf17"></a>[`void Inno.Runtime.Contracts.IRuntimeSubsystem.AfterRender(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/IRuntimeSubsystem.cs#L69) | Finalizes frame output even when rendering fails.<br>`frame`：The immutable variable-frame state. | — | — |
| <a id="api-2984536823dc7d73"></a>[`void Inno.Runtime.Contracts.IRuntimeSubsystem.Attach()`](../../src/runtime/contracts/Inno.Runtime.Contracts/IRuntimeSubsystem.cs#L13) | Acquires session resources after every dependency has attached. | — | — |
| <a id="api-ed91f0325b411a34"></a>[`void Inno.Runtime.Contracts.IRuntimeSubsystem.BeforeRender(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/IRuntimeSubsystem.cs#L53) | Prepares frame output after simulation has completed.<br>`frame`：The immutable variable-frame state. | — | — |
| <a id="api-bfcbd2e08961e975"></a>[`void Inno.Runtime.Contracts.IRuntimeSubsystem.BeginFrame(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/IRuntimeSubsystem.cs#L21) | Begins one frame before event dispatch and simulation.<br>`frame`：The immutable variable-frame state. | — | — |
| <a id="api-2528e70f5e706dcd"></a>[`void Inno.Runtime.Contracts.IRuntimeSubsystem.Detach()`](../../src/runtime/contracts/Inno.Runtime.Contracts/IRuntimeSubsystem.cs#L82) | Releases attached session resources before dependencies detach. | — | — |
| <a id="api-1ac22947b1bf5ad6"></a>[`void Inno.Runtime.Contracts.IRuntimeSubsystem.EndFrame(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/IRuntimeSubsystem.cs#L77) | Ends one frame and releases frame-scoped state.<br>`frame`：The immutable variable-frame state. | — | — |
| <a id="api-8114b56b5c6584b5"></a>[`void Inno.Runtime.Contracts.IRuntimeSubsystem.FixedUpdate(Inno.Runtime.Contracts.RuntimeFixedFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/IRuntimeSubsystem.cs#L29) | Advances one deterministic simulation step.<br>`frame`：The immutable fixed-step state. | — | — |
| <a id="api-856721e3449b61c1"></a>[`void Inno.Runtime.Contracts.IRuntimeSubsystem.LateUpdate(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/IRuntimeSubsystem.cs#L45) | Advances state that depends on completed variable simulation.<br>`frame`：The immutable variable-frame state. | — | — |
| <a id="api-a4d06f0d6cbe6a4d"></a>[`void Inno.Runtime.Contracts.IRuntimeSubsystem.Render(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/IRuntimeSubsystem.cs#L61) | Produces frame output owned by this subsystem.<br>`frame`：The immutable variable-frame state. | — | — |
| <a id="api-8f529bf1988012eb"></a>[`void Inno.Runtime.Contracts.IRuntimeSubsystem.Update(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/IRuntimeSubsystem.cs#L37) | Advances variable simulation state.<br>`frame`：The immutable variable-frame state. | — | — |
| <a id="api-65155321de3d06c0"></a>[`public interface Inno.Runtime.Contracts.IRuntimeSubsystem : System.IDisposable`](../../src/runtime/contracts/Inno.Runtime.Contracts/IRuntimeSubsystem.cs#L8) | Participates in the ordered lifecycle of one isolated runtime session. | — | — |

### `Inno.Runtime.Contracts.IRuntimeSubsystemFactory`

<!-- api: T:Inno.Runtime.Contracts.IRuntimeSubsystemFactory 3038f27e96ef0d55719eb58311d75720935bfa14bb5ae5c202bbf28532b1a27e -->
<!-- api: P:Inno.Runtime.Contracts.IRuntimeSubsystemFactory.descriptor 5a41b02a4c4d2f4ad8a9c7a0501d784b57eca91e94f47e81a60f9829008b00f4 -->
<!-- api: M:Inno.Runtime.Contracts.IRuntimeSubsystemFactory.Create(Inno.Runtime.Contracts.RuntimeSubsystemContext) 56a791da5259440fb676f90941810178609f693ac7926290dd256d2ea1fac82d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-aff16149a43fbf35"></a>[`Inno.Runtime.Contracts.IRuntimeSubsystem Inno.Runtime.Contracts.IRuntimeSubsystemFactory.Create(Inno.Runtime.Contracts.RuntimeSubsystemContext context)`](../../src/runtime/contracts/Inno.Runtime.Contracts/IRuntimeSubsystemFactory.cs#L22) | Creates a subsystem owned exclusively by the supplied construction scope.<br>返回：A new unattached runtime subsystem; after returning, its lifecycle belongs to the pipeline.<br>`context`：Stable foundation services and a private factory lifetime. Register partial allocations and work here before callbacks can fail. | — | — |
| <a id="api-882fcca705e56429"></a>[`Inno.Runtime.Contracts.RuntimeSubsystemDescriptor Inno.Runtime.Contracts.IRuntimeSubsystemFactory.descriptor { get; }`](../../src/runtime/contracts/Inno.Runtime.Contracts/IRuntimeSubsystemFactory.cs#L11) | Gets stable ordering and dependency metadata without creating runtime state. | — | — |
| <a id="api-3840281a4756c12a"></a>[`public interface Inno.Runtime.Contracts.IRuntimeSubsystemFactory`](../../src/runtime/contracts/Inno.Runtime.Contracts/IRuntimeSubsystemFactory.cs#L6) | Creates one runtime subsystem instance for each declared Host or Session owner. | — | — |

### `Inno.Runtime.Contracts.RuntimeCapabilityId`

<!-- api: T:Inno.Runtime.Contracts.RuntimeCapabilityId 524f3d15b490a5eeb622e7aa93d98fec0b5701b2cb1cccbeccbeb5b9ce5fc9e4 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeCapabilityId.#ctor(System.String) 9ca8517fdd7c09da66056e0e329e63132a68baaf64fb12d38e5c72c47f81b7bb -->
<!-- api: P:Inno.Runtime.Contracts.RuntimeCapabilityId.value a735d37ba6f28fa69d156d0cfceeeec7fce17a3f7eb85d3534b6c3f676e77f1b -->
<!-- api: P:Inno.Runtime.Contracts.RuntimeCapabilityId.isValid dc0c203b915dbc14fefda506c95d324ff815fadb092a725979fee78fe56449c3 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeCapabilityId.ToString 8971b09f69aab04460ffd162f57c83606383dcfc3ff37063a11b28ce9ee515bc -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeCapabilityId.op_Inequality(Inno.Runtime.Contracts.RuntimeCapabilityId,Inno.Runtime.Contracts.RuntimeCapabilityId) 80de9a4bd95bdda28ba03b88e132db8d7292803006266ff30242cac811d6ed62 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeCapabilityId.op_Equality(Inno.Runtime.Contracts.RuntimeCapabilityId,Inno.Runtime.Contracts.RuntimeCapabilityId) dc80c12e40f385434d81497a1e9034c4de57f772afe3243706853200988d1a18 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeCapabilityId.GetHashCode a23ad36917f865f0d6b3aef84337e68ceb772faaac5bdd3ffa15a63ff19f2231 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeCapabilityId.Equals(System.Object) a9ccf5ecf64472f95c355aef80bbdfd30c36a8d5926fbdeab06a132a8b3c5282 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeCapabilityId.Equals(Inno.Runtime.Contracts.RuntimeCapabilityId) 35954e93230cf889617d5e5c347e62579e73b23275690fe99aa765ffae65d910 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-334f3189d7aa1af9"></a>[`public Inno.Runtime.Contracts.RuntimeCapabilityId.RuntimeCapabilityId(string value)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeCapabilityId.cs#L19) | Creates an open capability protocol identifier.<br>`value`：The non-empty, case-sensitive capability name. | — | `System.ArgumentException`：The identifier is empty. |
| <a id="api-76246c946ce84c6b"></a>[`public bool Inno.Runtime.Contracts.RuntimeCapabilityId.Equals(Inno.Runtime.Contracts.RuntimeCapabilityId other)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeCapabilityId.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-c00734e20170e35b"></a>[`public override bool Inno.Runtime.Contracts.RuntimeCapabilityId.Equals(object obj)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeCapabilityId.cs#L8) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-c23e6c36aa5e93e3"></a>[`public override int Inno.Runtime.Contracts.RuntimeCapabilityId.GetHashCode()`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeCapabilityId.cs#L8) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-8276f12d5fd71fd3"></a>[`public static bool Inno.Runtime.Contracts.RuntimeCapabilityId.operator ==(Inno.Runtime.Contracts.RuntimeCapabilityId left, Inno.Runtime.Contracts.RuntimeCapabilityId right)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeCapabilityId.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-c049ae3e4cb3e46b"></a>[`public static bool Inno.Runtime.Contracts.RuntimeCapabilityId.operator !=(Inno.Runtime.Contracts.RuntimeCapabilityId left, Inno.Runtime.Contracts.RuntimeCapabilityId right)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeCapabilityId.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-249232c976d0933b"></a>[`public override string Inno.Runtime.Contracts.RuntimeCapabilityId.ToString()`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeCapabilityId.cs#L41) | Formats the capability for diagnostics.<br>返回：The protocol name, or an empty string for an uninitialized value. | — | — |
| <a id="api-1b4f4dccb8b13d5b"></a>[`public bool Inno.Runtime.Contracts.RuntimeCapabilityId.isValid { get; }`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeCapabilityId.cs#L33) | Gets whether this value contains a usable capability name. | — | — |
| <a id="api-20049a940d5c1afc"></a>[`public string Inno.Runtime.Contracts.RuntimeCapabilityId.value { get; }`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeCapabilityId.cs#L28) | Gets the stable capability name, without interpreting it as a backend name. | — | — |
| <a id="api-86f6eacc513c2e78"></a>[`public readonly record struct Inno.Runtime.Contracts.RuntimeCapabilityId : System.IEquatable<Inno.Runtime.Contracts.RuntimeCapabilityId>`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeCapabilityId.cs#L8) | Identifies a backend-neutral capability supplied by composition, not an object or native handle. | — | — |

### `Inno.Runtime.Contracts.RuntimeFixedFrame`

<!-- api: T:Inno.Runtime.Contracts.RuntimeFixedFrame af77e8f75c0c8b2024feef447dfe7cb5bdcd501ddc39acd16f7ce23b8c0ceaf6 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeFixedFrame.#ctor(System.Int64,System.Single,System.Single) e1de7a74642467d0625d0c7c32a5f2c815228c9b6fc836634b30688ed08b2f76 -->
<!-- api: P:Inno.Runtime.Contracts.RuntimeFixedFrame.stepIndex 77b47c313a311da76a776582172fd43a3c9e4b77fe41b547c00e9ba9b06b7ad0 -->
<!-- api: P:Inno.Runtime.Contracts.RuntimeFixedFrame.time a6052da95943563bd9012cf5a37f4967c4fc0c0c4c75f4b1928f8f639b0ec0de -->
<!-- api: P:Inno.Runtime.Contracts.RuntimeFixedFrame.deltaTime 28481c24410e8151f996a3c48e00128af64a44356ee99ff78bd7dc4c74bf4400 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeFixedFrame.ToString 69fd68d4aa38c0cb7d70ccd0e62b474aa336ee409ccab848f38d05cf9282f0d5 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeFixedFrame.op_Inequality(Inno.Runtime.Contracts.RuntimeFixedFrame,Inno.Runtime.Contracts.RuntimeFixedFrame) 8de943a5ae4ae4336ec63e950565e47caa6373b09f9a84f264bd4495771bf483 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeFixedFrame.op_Equality(Inno.Runtime.Contracts.RuntimeFixedFrame,Inno.Runtime.Contracts.RuntimeFixedFrame) 3587617606af689bb1e1600025e25e75347e0215f77005dc3ccb3536ee134e7d -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeFixedFrame.GetHashCode 535fdb6616be772758fb658997ffc56859ac40a813a93a56c8243058af049a9a -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeFixedFrame.Equals(System.Object) 6295465225b66d90fdbb3e08c83c16629b0abfd88ab59f7f473b707be08dc46f -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeFixedFrame.Equals(Inno.Runtime.Contracts.RuntimeFixedFrame) 91b3cd99300d9565ca47588e1b39ee3eb366252d729bd4c36c7fe6c140b6c910 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1821a92650cf5009"></a>[`public Inno.Runtime.Contracts.RuntimeFixedFrame.RuntimeFixedFrame(long stepIndex, float time, float deltaTime)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFixedFrame.cs#L20) | Creates one deterministic fixed-step timing value.<br>`stepIndex`：The monotonically increasing fixed-step index.<br>`time`：Accumulated scaled fixed time.<br>`deltaTime`：The fixed interval. | — | — |
| <a id="api-e8aa3cc09b258011"></a>[`public bool Inno.Runtime.Contracts.RuntimeFixedFrame.Equals(Inno.Runtime.Contracts.RuntimeFixedFrame other)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFixedFrame.cs#L6) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-5493e2239ded8518"></a>[`public override bool Inno.Runtime.Contracts.RuntimeFixedFrame.Equals(object obj)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFixedFrame.cs#L6) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-7ceae6c7a00c2afb"></a>[`public override int Inno.Runtime.Contracts.RuntimeFixedFrame.GetHashCode()`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFixedFrame.cs#L6) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-22b161b72e1576ad"></a>[`public static bool Inno.Runtime.Contracts.RuntimeFixedFrame.operator ==(Inno.Runtime.Contracts.RuntimeFixedFrame left, Inno.Runtime.Contracts.RuntimeFixedFrame right)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFixedFrame.cs#L6) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-0a2440762ecc5fe6"></a>[`public static bool Inno.Runtime.Contracts.RuntimeFixedFrame.operator !=(Inno.Runtime.Contracts.RuntimeFixedFrame left, Inno.Runtime.Contracts.RuntimeFixedFrame right)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFixedFrame.cs#L6) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-3913fb65fb25f7c4"></a>[`public override string Inno.Runtime.Contracts.RuntimeFixedFrame.ToString()`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFixedFrame.cs#L6) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-2a661453c30a4023"></a>[`public float Inno.Runtime.Contracts.RuntimeFixedFrame.deltaTime { get; }`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFixedFrame.cs#L43) | Gets the configured fixed simulation interval in seconds. | — | — |
| <a id="api-7bf72c07af75466b"></a>[`public long Inno.Runtime.Contracts.RuntimeFixedFrame.stepIndex { get; }`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFixedFrame.cs#L33) | Gets the zero-based fixed-step index. | — | — |
| <a id="api-7d21765c14e478f5"></a>[`public float Inno.Runtime.Contracts.RuntimeFixedFrame.time { get; }`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFixedFrame.cs#L38) | Gets accumulated fixed simulation time in seconds. | — | — |
| <a id="api-2957e8a7123139d0"></a>[`public readonly record struct Inno.Runtime.Contracts.RuntimeFixedFrame : System.IEquatable<Inno.Runtime.Contracts.RuntimeFixedFrame>`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFixedFrame.cs#L6) | Provides immutable timing state to one deterministic fixed simulation step. | — | — |

### `Inno.Runtime.Contracts.RuntimeFrame`

<!-- api: T:Inno.Runtime.Contracts.RuntimeFrame 0a726d099ad2fbbce0d6c6649cec4431ec4a8e6e7b84739062f7380fdd1c7037 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeFrame.#ctor(System.Int64,System.Single,System.Single,System.Single,System.Single,System.Single,System.Boolean) 45e531a5d50489a118ae72c2b52380890f966c664ee84e8d94a4f507e969c9a1 -->
<!-- api: P:Inno.Runtime.Contracts.RuntimeFrame.frameIndex d0fdd0b4a6b80139267ca70f3b06bea36c3d83d7afa15440a17d65f61822f421 -->
<!-- api: P:Inno.Runtime.Contracts.RuntimeFrame.time 9d7f6d6eda8a1923e653350602445b70ea7fddf3e40510512a178f30db85b47c -->
<!-- api: P:Inno.Runtime.Contracts.RuntimeFrame.unscaledTime 907ad6592da218044023f0aea1317d0a0e9d5024ab2c038368bb16448f2baedd -->
<!-- api: P:Inno.Runtime.Contracts.RuntimeFrame.deltaTime 2dd1f374362704a163855e8d2d834b7d29fb39917d076b998b4b84c143a53ca8 -->
<!-- api: P:Inno.Runtime.Contracts.RuntimeFrame.unscaledDeltaTime ccc00163fd6f39aba0a3d36756c9275c63234b9d147c9924bdde73c7e1b2f91a -->
<!-- api: P:Inno.Runtime.Contracts.RuntimeFrame.timeScale 1e45351d9ce8600aad30554ec0aefd26cfec427160ffc0abe22a64a5a2d68805 -->
<!-- api: P:Inno.Runtime.Contracts.RuntimeFrame.isPaused 667f39ce0431a670a32fb40fba7589566353645f13890244c9a5550077944fca -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeFrame.ToString 47a279899b7b1d60be2abc51506a5947259dd9672f9f9dcff8a3d27b8483a91c -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeFrame.op_Inequality(Inno.Runtime.Contracts.RuntimeFrame,Inno.Runtime.Contracts.RuntimeFrame) 2dc6eae85eba2e879101dc4ebd8527be31320d6cc0f53069cc7ed8819b900778 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeFrame.op_Equality(Inno.Runtime.Contracts.RuntimeFrame,Inno.Runtime.Contracts.RuntimeFrame) 1698d9bd4b2831ffed4930468a153b80f7c6f1b98503cad5c9e7d14aaee86a53 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeFrame.GetHashCode 1382325f0f010828ecb586f7b41328993df948330841d568db57bda43619bde8 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeFrame.Equals(System.Object) 825fd4627c9b8e09edcecf50d686279daa58daa2d44b30d2cce4d75907530d59 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeFrame.Equals(Inno.Runtime.Contracts.RuntimeFrame) 168f863b4b62050adf66d2ca60c20395c46d0dcb828adcf9ea2ed78c61882f54 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-47b48447b84c6c93"></a>[`public Inno.Runtime.Contracts.RuntimeFrame.RuntimeFrame(long frameIndex, float time, float unscaledTime, float deltaTime, float unscaledDeltaTime, float timeScale, bool isPaused)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFrame.cs#L34) | Creates the immutable timing snapshot for one variable frame.<br>`frameIndex`：The monotonically increasing owner frame index.<br>`time`：Accumulated scaled time.<br>`unscaledTime`：Accumulated unscaled time.<br>`deltaTime`：Scaled interval.<br>`unscaledDeltaTime`：Unscaled interval.<br>`timeScale`：Simulation multiplier.<br>`isPaused`：Whether scaled simulation is paused. | — | — |
| <a id="api-740e26f7c0374124"></a>[`public bool Inno.Runtime.Contracts.RuntimeFrame.Equals(Inno.Runtime.Contracts.RuntimeFrame other)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFrame.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-3452d8fde1e33cb6"></a>[`public override bool Inno.Runtime.Contracts.RuntimeFrame.Equals(object obj)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFrame.cs#L8) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-2e60f4619733e4e1"></a>[`public override int Inno.Runtime.Contracts.RuntimeFrame.GetHashCode()`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFrame.cs#L8) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-f0ae6c58d9b5a50c"></a>[`public static bool Inno.Runtime.Contracts.RuntimeFrame.operator ==(Inno.Runtime.Contracts.RuntimeFrame left, Inno.Runtime.Contracts.RuntimeFrame right)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFrame.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-87b13bbdfa1d8263"></a>[`public static bool Inno.Runtime.Contracts.RuntimeFrame.operator !=(Inno.Runtime.Contracts.RuntimeFrame left, Inno.Runtime.Contracts.RuntimeFrame right)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFrame.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-e25ef5d12f2659ab"></a>[`public override string Inno.Runtime.Contracts.RuntimeFrame.ToString()`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFrame.cs#L8) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-9743b949ba82533a"></a>[`public float Inno.Runtime.Contracts.RuntimeFrame.deltaTime { get; }`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFrame.cs#L70) | Gets the scaled variable frame interval in seconds. | — | — |
| <a id="api-ba5b6838765e9dce"></a>[`public long Inno.Runtime.Contracts.RuntimeFrame.frameIndex { get; }`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFrame.cs#L55) | Gets the zero-based session frame index. | — | — |
| <a id="api-a48d9b20a775c966"></a>[`public bool Inno.Runtime.Contracts.RuntimeFrame.isPaused { get; }`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFrame.cs#L85) | Gets whether scaled simulation is paused for this frame. | — | — |
| <a id="api-5f5c49743959ff0d"></a>[`public float Inno.Runtime.Contracts.RuntimeFrame.time { get; }`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFrame.cs#L60) | Gets accumulated scaled session time in seconds. | — | — |
| <a id="api-ccff96588d57a05e"></a>[`public float Inno.Runtime.Contracts.RuntimeFrame.timeScale { get; }`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFrame.cs#L80) | Gets the simulation time multiplier used for this frame. | — | — |
| <a id="api-f40c33fdf4a197b9"></a>[`public float Inno.Runtime.Contracts.RuntimeFrame.unscaledDeltaTime { get; }`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFrame.cs#L75) | Gets the unscaled variable frame interval in seconds. | — | — |
| <a id="api-7443d47cf0a22c31"></a>[`public float Inno.Runtime.Contracts.RuntimeFrame.unscaledTime { get; }`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFrame.cs#L65) | Gets accumulated unscaled session time in seconds. | — | — |
| <a id="api-167789a09a16b983"></a>[`public readonly record struct Inno.Runtime.Contracts.RuntimeFrame : System.IEquatable<Inno.Runtime.Contracts.RuntimeFrame>`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeFrame.cs#L8) | Provides immutable timing state to one variable runtime frame. | — | — |

### `Inno.Runtime.Contracts.RuntimeSubsystem`

<!-- api: T:Inno.Runtime.Contracts.RuntimeSubsystem 3fc08491479fc97b1a33c16ba884cd8dfd3a0f9014ad8ad85746fe6c4dc46a68 -->
<!-- api: P:Inno.Runtime.Contracts.RuntimeSubsystem.isStarted 68254979e5ede186b13ed884bf87c038aa4c4a5a4abd7d747a78b40800b46bbe -->
<!-- api: P:Inno.Runtime.Contracts.RuntimeSubsystem.lifetime 0ec7871bcfa6f0a09d2b49f5a0a84442026981ac20fea11ea9e91a59dbe4c6b0 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeSubsystem.Attach 9e605d91c939da49bf0564d86a33c1329d30d5bb6199b5e3be025601119309ba -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeSubsystem.BeginFrame(Inno.Runtime.Contracts.RuntimeFrame) ecb26f5d023f34202222170eb8ad84aeeded8f43740b5bce8da6496a90559b3f -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeSubsystem.FixedUpdate(Inno.Runtime.Contracts.RuntimeFixedFrame) c632f6d2cf2c66382225c39e3eeff40276826e25ddda3c05dfc67e7202df829f -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeSubsystem.Update(Inno.Runtime.Contracts.RuntimeFrame) bd66cff648e0f09db16cf1e67893772e413f178709390a2313a46e846f5e3f35 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeSubsystem.LateUpdate(Inno.Runtime.Contracts.RuntimeFrame) a018c4730846b9d221b053dbf6581c8dfac4cbbe079d69f1b7bb9e754373fd9e -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeSubsystem.BeforeRender(Inno.Runtime.Contracts.RuntimeFrame) d04e13a00aaaff5c98916abd1c71238647fac08ae422beacae91b9fd5751374b -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeSubsystem.Render(Inno.Runtime.Contracts.RuntimeFrame) cf713dae3b1765c3d294c1ba5cf453bce6cded8d148a910ed957a84f7f443376 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeSubsystem.AfterRender(Inno.Runtime.Contracts.RuntimeFrame) 62273b1f611b3fdb304657b065fbe0e12b4d8f2192934a8e0496f9b6421fb6b9 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeSubsystem.EndFrame(Inno.Runtime.Contracts.RuntimeFrame) ad67b904aad0acb5f6866e4d9416e7f17cd2f830b6011e2b9ecdeabafa6d5021 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeSubsystem.Detach ea7bdc294ba3ad1a1ba79724f6d7f1cea62d6001fb9b697f46daa91f4b581e48 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeSubsystem.Dispose 1f2db5ecf200eed5d9fcf8f6710b06e857a89812ec37d33a495479e2c18b3314 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeSubsystem.OwnFrameScope(System.IDisposable) cfb762903e576cdc4941432249c83a2a29bf05df6d4faa60d9a89d6603778980 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeSubsystem.OnStart 90590b22854dd6fe3d69fa03d5ecae6892d13060743bacedf81aef063caaf6b4 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeSubsystem.OnBeginFrame(Inno.Runtime.Contracts.RuntimeFrame) 83b84694ffd6c4dcc42611dda7eecfd884abbb9fe307de53f0e3b5cf03286509 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeSubsystem.OnFixedUpdate(Inno.Runtime.Contracts.RuntimeFixedFrame) db2f5ad17bb50f22cfbbaa473e502c61d1d3fddb54c88174765411d45e91e772 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeSubsystem.OnUpdate(Inno.Runtime.Contracts.RuntimeFrame) 4d14c16ae6cbcac1ebad4d27a360d7c7a0114fcd9be28b4deb85d2c34ea0b935 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeSubsystem.OnLateUpdate(Inno.Runtime.Contracts.RuntimeFrame) 9c9929944d26f5bfda819f1dc7e56cda4c1c5954aadffdc099c6cce6225876f3 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeSubsystem.OnPrepareOutput(Inno.Runtime.Contracts.RuntimeFrame) 155837155e34f1d26f234c8223e649b42a37a8b303c3ee552d3278e383cf0885 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeSubsystem.OnProduceOutput(Inno.Runtime.Contracts.RuntimeFrame) d90f42e057f281dede4bca884efc93f2380ac621f46fb753b9d575bb77205c00 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeSubsystem.OnCompleteOutput(Inno.Runtime.Contracts.RuntimeFrame) 2e5b3dfe6e5dd124832850ed0e7b157646fb2408a3edeeff8bd9c9dfc7aceb12 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeSubsystem.OnEndFrame(Inno.Runtime.Contracts.RuntimeFrame) 95519b26e0052cf8e28a65ece49fd4c0b82ab1fbab14f045a9143079fd4b3b00 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeSubsystem.OnStop 353b0a90648503171963c40f3553afba5cd8dfbf7d5cefdab72d1b624a51facc -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeSubsystem.#ctor 055f9c8e798f634ae435277777b7a7d76489500b3f775ebceb789fb1e97755a6 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4d56c6c4c2b425b1"></a>[`protected Inno.Runtime.Contracts.RuntimeSubsystem.RuntimeSubsystem()`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L10) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-093d46ae60e6ba82"></a>[`public void Inno.Runtime.Contracts.RuntimeSubsystem.AfterRender(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L149) | Finalizes frame output even when rendering fails.<br>`frame`：The immutable variable-frame state. | — | — |
| <a id="api-af217e8280ea137b"></a>[`public void Inno.Runtime.Contracts.RuntimeSubsystem.Attach()`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L41) | Acquires session resources after every dependency has attached. | The caller must own this instance before attachment. Failed attachment stops admission but leaves partial resources with that owner, which must drive Dispose through any pending retirement. | `System.InvalidOperationException`：The subsystem is already started. |
| <a id="api-be4722cacf0dfcb6"></a>[`public void Inno.Runtime.Contracts.RuntimeSubsystem.BeforeRender(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L127) | Prepares frame output after simulation has completed.<br>`frame`：The immutable variable-frame state. | — | — |
| <a id="api-4978295976a1beb6"></a>[`public void Inno.Runtime.Contracts.RuntimeSubsystem.BeginFrame(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L65) | Begins one frame before event dispatch and simulation.<br>`frame`：The immutable variable-frame state. | — | — |
| <a id="api-3984d3d5aa5a5d12"></a>[`public void Inno.Runtime.Contracts.RuntimeSubsystem.Detach()`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L176) | Releases attached session resources before dependencies detach. | — | — |
| <a id="api-1e3e4f8431da66dd"></a>[`public void Inno.Runtime.Contracts.RuntimeSubsystem.Dispose()`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L186) | Releases this owner's registrations and resources exactly once. | — | `Inno.Core.Execution.RetirementPendingException`：Tracked work, the stop hook or a child resource is still draining. Retry without releasing dependencies.<br>`System.AggregateException`：Retirement failed after every cleanup stage was attempted. |
| <a id="api-bec987ddad7ba01b"></a>[`public void Inno.Runtime.Contracts.RuntimeSubsystem.EndFrame(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L160) | Ends one frame and releases frame-scoped state.<br>`frame`：The immutable variable-frame state. | — | — |
| <a id="api-5dc8fa4cbb6d5c3c"></a>[`public void Inno.Runtime.Contracts.RuntimeSubsystem.FixedUpdate(Inno.Runtime.Contracts.RuntimeFixedFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L94) | Advances one deterministic simulation step.<br>`frame`：The immutable fixed-step state. | — | — |
| <a id="api-b2434b1c9a079b75"></a>[`public void Inno.Runtime.Contracts.RuntimeSubsystem.LateUpdate(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L116) | Advances state that depends on completed variable simulation.<br>`frame`：The immutable variable-frame state. | — | — |
| <a id="api-63a7f41dc7095a4d"></a>[`protected virtual void Inno.Runtime.Contracts.RuntimeSubsystem.OnBeginFrame(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L235) | Captures snapshots and binds service façades.<br>`frame`：The variable frame timing. | — | — |
| <a id="api-a76a3a8bd71686a1"></a>[`protected virtual void Inno.Runtime.Contracts.RuntimeSubsystem.OnCompleteOutput(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L277) | Submits output and closes output-specific temporary resources.<br>`frame`：The variable frame timing. | — | — |
| <a id="api-4dca8c06bc47cee2"></a>[`protected virtual void Inno.Runtime.Contracts.RuntimeSubsystem.OnEndFrame(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L284) | Finalizes control-thread frame state before execution bindings are revoked.<br>`frame`：The frame being closed. | — | — |
| <a id="api-09ab8e48315d9df2"></a>[`protected virtual void Inno.Runtime.Contracts.RuntimeSubsystem.OnFixedUpdate(Inno.Runtime.Contracts.RuntimeFixedFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L242) | Advances one deterministic fixed step.<br>`frame`：The fixed-step timing. | — | — |
| <a id="api-c6d860477bc3ccf6"></a>[`protected virtual void Inno.Runtime.Contracts.RuntimeSubsystem.OnLateUpdate(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L256) | Updates state that depends on completed simulation.<br>`frame`：The variable frame timing. | — | — |
| <a id="api-f82c1242a6320c2f"></a>[`protected virtual void Inno.Runtime.Contracts.RuntimeSubsystem.OnPrepareOutput(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L263) | Opens resources required for this frame's output.<br>`frame`：The variable frame timing. | — | — |
| <a id="api-b7f7a725eedc1675"></a>[`protected virtual void Inno.Runtime.Contracts.RuntimeSubsystem.OnProduceOutput(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L270) | Collects output commands without executing managed code on a native realtime callback.<br>`frame`：The variable frame timing. | — | — |
| <a id="api-a9fbda8fe104277c"></a>[`protected virtual void Inno.Runtime.Contracts.RuntimeSubsystem.OnStart()`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L228) | Acquires resources after dependencies have started. | — | — |
| <a id="api-5b6d1fa272929cb7"></a>[`protected virtual void Inno.Runtime.Contracts.RuntimeSubsystem.OnStop()`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L288) | Releases domain resources, including allocations made before attachment. | — | — |
| <a id="api-a6395ad0feac3b77"></a>[`protected virtual void Inno.Runtime.Contracts.RuntimeSubsystem.OnUpdate(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L249) | Advances domain state on the variable clock.<br>`frame`：The variable frame timing. | — | — |
| <a id="api-7413c02f443af663"></a>[`protected void Inno.Runtime.Contracts.RuntimeSubsystem.OwnFrameScope(System.IDisposable scope)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L218) | Registers a binding that must close even when a frame hook fails.<br>`scope`：The newly entered execution scope. | — | `System.InvalidOperationException`：No subsystem frame is open. |
| <a id="api-9f7fc0627335a31c"></a>[`public void Inno.Runtime.Contracts.RuntimeSubsystem.Render(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L138) | Produces frame output owned by this subsystem.<br>`frame`：The immutable variable-frame state. | — | — |
| <a id="api-d1e3e4fc2d4062fd"></a>[`public void Inno.Runtime.Contracts.RuntimeSubsystem.Update(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L105) | Advances variable simulation state.<br>`frame`：The immutable variable-frame state. | — | — |
| <a id="api-3487a7f1eef35995"></a>[`public bool Inno.Runtime.Contracts.RuntimeSubsystem.isStarted { get; }`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L25) | Gets whether this instance is attached to an owner and has not retired. | — | — |
| <a id="api-199befd627820a05"></a>[`protected Inno.Core.Execution.LifetimeScope Inno.Runtime.Contracts.RuntimeSubsystem.lifetime { get; }`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L30) | Gets resources and cancellation owned exclusively by this subsystem. | — | — |
| <a id="api-5f32ef0057834e35"></a>[`public abstract class Inno.Runtime.Contracts.RuntimeSubsystem : Inno.Runtime.Contracts.IRuntimeSubsystem`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystem.cs#L10) | Enforces one control-thread lifecycle while derived runtimes compose their own domain services. | — | — |

### `Inno.Runtime.Contracts.RuntimeSubsystemCatalogAttribute`

<!-- api: T:Inno.Runtime.Contracts.RuntimeSubsystemCatalogAttribute fe947f27ee889c3b1fda876f01d2273d005b569ea9b11e7b0caf63ef45bd0dd1 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeSubsystemCatalogAttribute.#ctor 8052f95f328caaeddb5b45610140c33072d7af17232eff46754b33e088582980 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-256ff02049605465"></a>[`public Inno.Runtime.Contracts.RuntimeSubsystemCatalogAttribute.RuntimeSubsystemCatalogAttribute()`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemCatalogAttribute.cs#L9) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-ba3d9b59c1762da1"></a>[`public sealed class Inno.Runtime.Contracts.RuntimeSubsystemCatalogAttribute : System.Attribute`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemCatalogAttribute.cs#L9) | Requests a generated partial method that gathers subsystem declarations with the same typed composition parameter. | — | — |

### `Inno.Runtime.Contracts.RuntimeSubsystemContext`

<!-- api: T:Inno.Runtime.Contracts.RuntimeSubsystemContext 090e5073e7b05d36c6d88e00a6d5193c892cf42f26c2c993c5194042c1e4c0a0 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeSubsystemContext.#ctor(Inno.Core.Events.EventDispatcher,Inno.Core.Diagnostics.DiagnosticHub,Inno.Core.Identity.IdentityAllocator,Inno.Extensibility.Types.TypeCatalog,Inno.Core.Execution.LifetimeScope,Inno.Runtime.Contracts.RuntimeSubsystemLifetime,System.Boolean,System.Collections.Generic.IEnumerable{Inno.Runtime.Contracts.RuntimeCapabilityId}) 55e6384919a7be53ffc9bc3f73c021c3ce0e70f147c35ef07b5f5c20d239622f -->
<!-- api: P:Inno.Runtime.Contracts.RuntimeSubsystemContext.events b50c3633394b512676d47dacd2a9217a6205af9c205177fcc3724b1a4e08df3e -->
<!-- api: P:Inno.Runtime.Contracts.RuntimeSubsystemContext.diagnostics 7b505fe98704d01e68388db4137510e6542097f7d7ce7e863604dc7438fc31a3 -->
<!-- api: P:Inno.Runtime.Contracts.RuntimeSubsystemContext.identities 465fdc1909c69f478b753bd4d133cbf87b8288829baefb9090f1aec2a5e2a261 -->
<!-- api: P:Inno.Runtime.Contracts.RuntimeSubsystemContext.types 4086a819bf823743f5b8fa0a42f2c5240b4b958ca70a42153305bce7ca6b78a1 -->
<!-- api: P:Inno.Runtime.Contracts.RuntimeSubsystemContext.resources eaec5be40f2997f3d0604db410f5a44d26b379a09958eb971f8a6f7a5b9032a9 -->
<!-- api: P:Inno.Runtime.Contracts.RuntimeSubsystemContext.lifetime 6b8f949286b7c46e8a8690cbfaa3d034c6c84545be2c8847e0f2ba4caedfa276 -->
<!-- api: P:Inno.Runtime.Contracts.RuntimeSubsystemContext.isEditMode 6b20961ff10a5f435ea698d1d8dd768fbd9a98319a2c57af2063bad44a8397ad -->
<!-- api: P:Inno.Runtime.Contracts.RuntimeSubsystemContext.capabilities d001a047b6b9085523c938e3e70a5fcacdcb4f34c9d1f10cfa3825be267700d5 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-cc1be25c2b660131"></a>[`public Inno.Runtime.Contracts.RuntimeSubsystemContext.RuntimeSubsystemContext(Inno.Core.Events.EventDispatcher events, Inno.Core.Diagnostics.DiagnosticHub diagnostics, Inno.Core.Identity.IdentityAllocator identities, Inno.Extensibility.Types.TypeCatalog types, Inno.Core.Execution.LifetimeScope resources, Inno.Runtime.Contracts.RuntimeSubsystemLifetime lifetime, bool isEditMode = false, System.Collections.Generic.IEnumerable<Inno.Runtime.Contracts.RuntimeCapabilityId>? capabilities = null)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemContext.cs#L48) | Creates the explicit construction boundary for a host or session pipeline.<br>`events`：The owner event dispatcher.<br>`diagnostics`：The shared diagnostic state owner.<br>`identities`：The isolated identity domain.<br>`types`：The active extension type catalog.<br>`resources`：The pipeline-owned construction lifetime.<br>`lifetime`：The scope in which created subsystems will live.<br>`isEditMode`：Whether simulation belongs to an authoring session.<br>`capabilities`：Verified backend-neutral capabilities supplied by composition; copied into an immutable set. | — | `System.ArgumentNullException`：A required foundation service is null. |
| <a id="api-97b09b899ad4ddd1"></a>[`public System.Collections.Generic.IReadOnlySet<Inno.Runtime.Contracts.RuntimeCapabilityId> Inno.Runtime.Contracts.RuntimeSubsystemContext.capabilities { get; }`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemContext.cs#L102) | Gets the immutable capabilities verified by the owner, never a mutable service container. | — | — |
| <a id="api-2bc71ecfee41d12d"></a>[`public Inno.Core.Diagnostics.DiagnosticHub Inno.Runtime.Contracts.RuntimeSubsystemContext.diagnostics { get; }`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemContext.cs#L77) | Gets the shared diagnostic state owner. | — | — |
| <a id="api-6e5a356b4f96beb3"></a>[`public Inno.Core.Events.EventDispatcher Inno.Runtime.Contracts.RuntimeSubsystemContext.events { get; }`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemContext.cs#L73) | Gets the owner-thread event dispatcher. | — | — |
| <a id="api-b34bbff6c9ec45e7"></a>[`public Inno.Core.Identity.IdentityAllocator Inno.Runtime.Contracts.RuntimeSubsystemContext.identities { get; }`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemContext.cs#L81) | Gets the isolated live-object identity domain. | — | — |
| <a id="api-470c0df61043393b"></a>[`public bool Inno.Runtime.Contracts.RuntimeSubsystemContext.isEditMode { get; }`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemContext.cs#L97) | Gets whether scaled simulation should be disabled for authoring. | — | — |
| <a id="api-e89ae40d80caf49d"></a>[`public Inno.Runtime.Contracts.RuntimeSubsystemLifetime Inno.Runtime.Contracts.RuntimeSubsystemContext.lifetime { get; }`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemContext.cs#L93) | Gets the owner scope used to validate factory lifetimes. | — | — |
| <a id="api-e4001d01912531a6"></a>[`public Inno.Core.Execution.LifetimeScope Inno.Runtime.Contracts.RuntimeSubsystemContext.resources { get; }`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemContext.cs#L89) | Gets resources owned by this pipeline, not by a global container. | — | — |
| <a id="api-f43b024c0c9bbe87"></a>[`public Inno.Extensibility.Types.TypeCatalog Inno.Runtime.Contracts.RuntimeSubsystemContext.types { get; }`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemContext.cs#L85) | Gets the current foundation type catalog. | — | — |
| <a id="api-45b8410ed4449130"></a>[`public sealed class Inno.Runtime.Contracts.RuntimeSubsystemContext`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemContext.cs#L16) | Exposes only foundation services and owner metadata, never a complete RuntimeSession or domain service locator. | — | — |

### `Inno.Runtime.Contracts.RuntimeSubsystemDescriptor`

<!-- api: T:Inno.Runtime.Contracts.RuntimeSubsystemDescriptor c3a59a32f01902729330a654e41c1feb467710733a30bd1121735f6d79506846 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeSubsystemDescriptor.#ctor(Inno.Runtime.Contracts.RuntimeSubsystemId,System.Int32,System.Collections.Generic.IReadOnlyList{Inno.Runtime.Contracts.RuntimeSubsystemId},Inno.Runtime.Contracts.RuntimeSubsystemLifetime,Inno.Runtime.Contracts.RuntimeSubsystemRequirement,System.Collections.Generic.IReadOnlyList{Inno.Runtime.Contracts.RuntimeCapabilityId}) d4ac07ea0551c08af175b409b666a39b83895bd9bae79ba8d56a4e8002f1cf03 -->
<!-- api: P:Inno.Runtime.Contracts.RuntimeSubsystemDescriptor.id eb8d2ca22fb432d64cfa24a869e7e47894df52d28a50803d13c184b05375b07f -->
<!-- api: P:Inno.Runtime.Contracts.RuntimeSubsystemDescriptor.lifetime 37236367b65dfdcb635b771831dd37a2cc4b5f3074685209a965e7ca7f07fad5 -->
<!-- api: P:Inno.Runtime.Contracts.RuntimeSubsystemDescriptor.order ab73e8a334e2352ef59e9dbfc817bf7e88a0a3cd579d429f817293f2532fca5b -->
<!-- api: P:Inno.Runtime.Contracts.RuntimeSubsystemDescriptor.dependencies 225e426a334c1e46a6dbe7511ded8b513d147f5af583edad5ea76bd57b11fe88 -->
<!-- api: P:Inno.Runtime.Contracts.RuntimeSubsystemDescriptor.requirement c3b318c6242cc94ec292e855d05db30237296c7ebe38684084f333398ef510da -->
<!-- api: P:Inno.Runtime.Contracts.RuntimeSubsystemDescriptor.requiredCapabilities 746d003d09ded782a4b50e136a83c22952ee1cdec8fbbdc830cb75752fd578d4 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8da5c5884d0f419f"></a>[`public Inno.Runtime.Contracts.RuntimeSubsystemDescriptor.RuntimeSubsystemDescriptor(Inno.Runtime.Contracts.RuntimeSubsystemId id, int order = 0, System.Collections.Generic.IReadOnlyList<Inno.Runtime.Contracts.RuntimeSubsystemId>? dependencies = null, Inno.Runtime.Contracts.RuntimeSubsystemLifetime lifetime = Inno.Runtime.Contracts.RuntimeSubsystemLifetime.Session, Inno.Runtime.Contracts.RuntimeSubsystemRequirement requirement = Inno.Runtime.Contracts.RuntimeSubsystemRequirement.Required, System.Collections.Generic.IReadOnlyList<Inno.Runtime.Contracts.RuntimeCapabilityId>? requiredCapabilities = null)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemDescriptor.cs#L36) | Creates an immutable runtime subsystem descriptor.<br>`id`：The unique subsystem protocol identifier.<br>`order`：The deterministic tie-break order among otherwise independent subsystems.<br>`dependencies`：Features that must attach and execute before this subsystem.<br>`lifetime`：The exclusive Host or Session lifetime required by this subsystem.<br>`requirement`：Whether startup must succeed or may remain explicitly unavailable after complete compensation.<br>`requiredCapabilities`：Backend-neutral capabilities that composition must supply before factory creation. | — | `System.ArgumentException`：Thrown when id is invalid or a dependency is invalid, duplicated, or self-referential. |
| <a id="api-8fe7f0b601c08043"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Runtime.Contracts.RuntimeSubsystemId> Inno.Runtime.Contracts.RuntimeSubsystemDescriptor.dependencies { get; }`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemDescriptor.cs#L86) | Gets the immutable required-subsystem identifiers. | — | — |
| <a id="api-496e28be2facf7f1"></a>[`public Inno.Runtime.Contracts.RuntimeSubsystemId Inno.Runtime.Contracts.RuntimeSubsystemDescriptor.id { get; }`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemDescriptor.cs#L71) | Gets the unique subsystem protocol identifier. | — | — |
| <a id="api-bb23280c8e444fce"></a>[`public Inno.Runtime.Contracts.RuntimeSubsystemLifetime Inno.Runtime.Contracts.RuntimeSubsystemDescriptor.lifetime { get; }`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemDescriptor.cs#L76) | Gets the exclusive owner scope required by this subsystem. | — | — |
| <a id="api-e13e89ffff735c9d"></a>[`public int Inno.Runtime.Contracts.RuntimeSubsystemDescriptor.order { get; }`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemDescriptor.cs#L81) | Gets the deterministic order used after dependency constraints. | — | — |
| <a id="api-a56522ec718ed792"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Runtime.Contracts.RuntimeCapabilityId> Inno.Runtime.Contracts.RuntimeSubsystemDescriptor.requiredCapabilities { get; }`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemDescriptor.cs#L96) | Gets the immutable capability prerequisites checked before allocating subsystem resources. | — | — |
| <a id="api-9cf4850ac125d4e2"></a>[`public Inno.Runtime.Contracts.RuntimeSubsystemRequirement Inno.Runtime.Contracts.RuntimeSubsystemDescriptor.requirement { get; }`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemDescriptor.cs#L91) | Gets whether this subsystem must start for its owner to become available. | — | — |
| <a id="api-3913c12b6e6e84b6"></a>[`public sealed class Inno.Runtime.Contracts.RuntimeSubsystemDescriptor`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemDescriptor.cs#L10) | Describes stable ordering and dependency requirements for one runtime subsystem. | — | — |

### `Inno.Runtime.Contracts.RuntimeSubsystemId`

<!-- api: T:Inno.Runtime.Contracts.RuntimeSubsystemId 773e865c835c08707e64e2ccafdeaebe06adc3c3f4e3e71068d487f772c08091 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeSubsystemId.#ctor(System.String) f912ebd81241536f8ecd666516c711ef71afdc838cadd45337658374a887851e -->
<!-- api: P:Inno.Runtime.Contracts.RuntimeSubsystemId.value b302d30e060e650dbc71d48dadb88e65f76bfc3391bc0805796200082db8e644 -->
<!-- api: P:Inno.Runtime.Contracts.RuntimeSubsystemId.isValid af971e6adb349ae47c2b16e47f6b64e555ee0f1e90dd7241cd4c2cd3e253652f -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeSubsystemId.ToString 6cf965ac1e07f423fc4bc6baa2f16b92a331bff7f8091ff4f6c150805f69a908 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeSubsystemId.op_Inequality(Inno.Runtime.Contracts.RuntimeSubsystemId,Inno.Runtime.Contracts.RuntimeSubsystemId) 940d10d06bf02b800516611ffc447747283e816af0d3ae274345690a32159072 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeSubsystemId.op_Equality(Inno.Runtime.Contracts.RuntimeSubsystemId,Inno.Runtime.Contracts.RuntimeSubsystemId) 3b377c13d2fafcfc748bd33f3f583295a00207fb88569ae381fd3fc4db0199ad -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeSubsystemId.GetHashCode c33238b9b63f371a95b390d3ef118c572a7bd83d3a6cadb6b745ad12d1c0ce50 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeSubsystemId.Equals(System.Object) 95ff227ceaee461e7ce4dad7feea51db82586124de4392a781e2561e2f413917 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeSubsystemId.Equals(Inno.Runtime.Contracts.RuntimeSubsystemId) 4528644589d5b9740314052ddb75cbfb4e6e18429db8aef6eef339d3912dc00c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-42193042c23e1b31"></a>[`public Inno.Runtime.Contracts.RuntimeSubsystemId.RuntimeSubsystemId(string value)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemId.cs#L19) | Creates a stable runtime subsystem identifier.<br>`value`：The non-empty protocol identifier. | — | `System.ArgumentException`：Thrown when value is empty. |
| <a id="api-00a80a73260cc6cc"></a>[`public bool Inno.Runtime.Contracts.RuntimeSubsystemId.Equals(Inno.Runtime.Contracts.RuntimeSubsystemId other)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemId.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-bdfa92be0924dd86"></a>[`public override bool Inno.Runtime.Contracts.RuntimeSubsystemId.Equals(object obj)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemId.cs#L8) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-f567d32a4b62d7a9"></a>[`public override int Inno.Runtime.Contracts.RuntimeSubsystemId.GetHashCode()`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemId.cs#L8) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-5f2d2da316ee7e43"></a>[`public static bool Inno.Runtime.Contracts.RuntimeSubsystemId.operator ==(Inno.Runtime.Contracts.RuntimeSubsystemId left, Inno.Runtime.Contracts.RuntimeSubsystemId right)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemId.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-f3d89b16dd864811"></a>[`public static bool Inno.Runtime.Contracts.RuntimeSubsystemId.operator !=(Inno.Runtime.Contracts.RuntimeSubsystemId left, Inno.Runtime.Contracts.RuntimeSubsystemId right)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemId.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-6fd18439ca5ef27a"></a>[`public override string Inno.Runtime.Contracts.RuntimeSubsystemId.ToString()`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemId.cs#L41) | Formats the subsystem identifier for diagnostics.<br>返回：The protocol value, or an empty string for an uninitialized value. | — | — |
| <a id="api-075778518df227e6"></a>[`public bool Inno.Runtime.Contracts.RuntimeSubsystemId.isValid { get; }`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemId.cs#L33) | Gets whether this identifier contains a usable protocol value. | — | — |
| <a id="api-d5a256d3c704af13"></a>[`public string Inno.Runtime.Contracts.RuntimeSubsystemId.value { get; }`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemId.cs#L28) | Gets the stable protocol value. | — | — |
| <a id="api-09c418cf26add3c2"></a>[`public readonly record struct Inno.Runtime.Contracts.RuntimeSubsystemId : System.IEquatable<Inno.Runtime.Contracts.RuntimeSubsystemId>`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemId.cs#L8) | Identifies one backend-neutral runtime subsystem protocol. | — | — |

### `Inno.Runtime.Contracts.RuntimeSubsystemLifetime`

<!-- api: T:Inno.Runtime.Contracts.RuntimeSubsystemLifetime 4f64d176b5860077d31a7e3d628e937b2e7b07cb81db9accba793d1d091e9f94 -->
<!-- api: F:Inno.Runtime.Contracts.RuntimeSubsystemLifetime.Host a53fc6f886e1fbd9168b19f1a6e2d1c9646c07ab12cd58a87ed545b79e9bedb4 -->
<!-- api: F:Inno.Runtime.Contracts.RuntimeSubsystemLifetime.Session 53fa016089a5b75dfe4a2a0c8861f138e204f1aee0e7e5c415eba1c1acb403d7 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e2d0fc0f5721bffb"></a>[`Inno.Runtime.Contracts.RuntimeSubsystemLifetime.Host = 0`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemLifetime.cs#L11) | The subsystem is shared by sessions and retires with the application host. | — | — |
| <a id="api-3cc1275637279e6a"></a>[`Inno.Runtime.Contracts.RuntimeSubsystemLifetime.Session = 1`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemLifetime.cs#L15) | The subsystem belongs to one isolated Edit, Play or Player session. | — | — |
| <a id="api-f0a68402e97dcfce"></a>[`public enum Inno.Runtime.Contracts.RuntimeSubsystemLifetime : int`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemLifetime.cs#L6) | Identifies the exclusive owner that creates and retires a stable engine subsystem. | — | — |

### `Inno.Runtime.Contracts.RuntimeSubsystemRegistrationAttribute`

<!-- api: T:Inno.Runtime.Contracts.RuntimeSubsystemRegistrationAttribute 4490c562f85f366207bd8306c20156e9e37ee485974ce84bc3ee4f5cdb4dbf68 -->
<!-- api: M:Inno.Runtime.Contracts.RuntimeSubsystemRegistrationAttribute.#ctor(System.String) b7c185daa334c2544e1777418c0cdc0b1dfa35b89306ae9336c7024aa0a055be -->
<!-- api: P:Inno.Runtime.Contracts.RuntimeSubsystemRegistrationAttribute.id 055cd364212f01d1a75f7a4a359e1f1a5bd0375c1e65c43a39831c8ae2e7c622 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3b60d3072253b975"></a>[`public Inno.Runtime.Contracts.RuntimeSubsystemRegistrationAttribute.RuntimeSubsystemRegistrationAttribute(string id)`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemRegistrationAttribute.cs#L17) | Declares the stable ID returned by this method's subsystem factory.<br>`id`：The exact non-empty subsystem descriptor ID. | — | — |
| <a id="api-351a4734522b7aaf"></a>[`public string Inno.Runtime.Contracts.RuntimeSubsystemRegistrationAttribute.id { get; }`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemRegistrationAttribute.cs#L26) | Gets the stable descriptor ID checked by the generated catalog. | — | — |
| <a id="api-dfaf525a51ce0913"></a>[`public sealed class Inno.Runtime.Contracts.RuntimeSubsystemRegistrationAttribute : System.Attribute`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemRegistrationAttribute.cs#L9) | Declares a strongly typed composition method for build-time subsystem catalog generation. | — | — |

### `Inno.Runtime.Contracts.RuntimeSubsystemRequirement`

<!-- api: T:Inno.Runtime.Contracts.RuntimeSubsystemRequirement 340f24ccc20663dcd2e858c605f84fde5916ea17571d06d47d15868adc2eb8d1 -->
<!-- api: F:Inno.Runtime.Contracts.RuntimeSubsystemRequirement.Required 33d6698627b9efba9c57290c0f3c5b1c39d481b574230b74e3d0c22e5bbab247 -->
<!-- api: F:Inno.Runtime.Contracts.RuntimeSubsystemRequirement.Optional 1b193806005424b010cf01af39634295cba9ba1953d91ee052bbdeb1b6a9ab88 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1fb401f0e53edd99"></a>[`Inno.Runtime.Contracts.RuntimeSubsystemRequirement.Optional = 1`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemRequirement.cs#L16) | A fully compensated startup may remain unavailable with diagnostics; cleanup failures still reject the owner. | — | — |
| <a id="api-a564618628602633"></a>[`Inno.Runtime.Contracts.RuntimeSubsystemRequirement.Required = 0`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemRequirement.cs#L11) | Missing capabilities, dependencies or failed startup reject the entire owner. | — | — |
| <a id="api-4560a9e9e695b12f"></a>[`public enum Inno.Runtime.Contracts.RuntimeSubsystemRequirement : int`](../../src/runtime/contracts/Inno.Runtime.Contracts/RuntimeSubsystemRequirement.cs#L6) | Selects startup failure behavior without changing runtime-frame or retirement error semantics. | — | — |

## 项目依赖

- [Inno.Core.Events](../core/Inno.Core.Events.md)：公开引用边界由实际签名核对。
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：公开引用边界由实际签名核对。
- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：公开引用边界由实际签名核对。
- [Inno.Core.Identity](../core/Inno.Core.Identity.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
