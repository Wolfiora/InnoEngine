# Inno.Input.Runtime

[Input 索引](README.md) · [Contract](Inno.Input.md) · [SDL3 Adapter](Inno.Adapter.Input.md)

`Inno.Input.Runtime` 把一个 `IInputBackend` 作为 Session-owned `RuntimeSubsystem`。`BeginFrame` 捕获一次状态并打开 `InputExecutionContext`；`EndFrame` 必定逆序关闭 scope；Detach/Dispose 会补偿未完整结束的帧并释放 backend。

## 公开 API

| API | 语义 |
| --- | --- |
| `InputRuntime` | 当前快照、frame scope 与 backend 所有者。 |
| `InputRuntimeFactory` | 为每个 Session 创建独立 backend/feature；可被重复用于 Edit、Play 与 Player。 |

Composition Root 只需把 factory 加入 `RuntimeSessionOptions.subsystemFactories`。测试可以注入 synthetic backend；Runtime 不依赖 SDL 或真实输入设备。创建失败会中止 Session 构造，释放已创建 Feature。

[下一页：Inno.Adapter.Input](Inno.Adapter.Input.md)

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Input.Runtime.InputRuntime`

<!-- api: T:Inno.Input.Runtime.InputRuntime 15535fbc6acbf1e4fb33f1818f313a8b3d7e3a0f7379a7604a768e3d7dbf7e72 -->
<!-- api: M:Inno.Input.Runtime.InputRuntime.#ctor(Inno.Input.IInputBackend) fb68433310b4b12ade342f0ad95cdb95b251dd7a419a11db0f351c2bbe17674c -->
<!-- api: P:Inno.Input.Runtime.InputRuntime.snapshot 76b85ba8e7b0e9c161f38e4dbae5e3736780fe3eb4d4aef875294e64ef8c28a1 -->
<!-- api: M:Inno.Input.Runtime.InputRuntime.OnBeginFrame(Inno.Runtime.Contracts.RuntimeFrame) a0038783b5123006445323c0a610c1abfc3af09d3fc3194744c7c3bace0408a6 -->
<!-- api: M:Inno.Input.Runtime.InputRuntime.OnStop 4a425e8d5d9875023cbfd929af7d877140a3ac2e49100250b994332f8babbaa4 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8e1f71c24790e175"></a>[`public Inno.Input.Runtime.InputRuntime.InputRuntime(Inno.Input.IInputBackend backend)`](../../src/services/input/Inno.Input.Runtime/InputRuntime.cs#L21) | Creates a feature whose backend ownership transfers to this instance.<br>`backend`：The platform backend captured at frame boundaries. | — | — |
| <a id="api-9898774946da5f12"></a>[`protected override void Inno.Input.Runtime.InputRuntime.OnBeginFrame(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/services/input/Inno.Input.Runtime/InputRuntime.cs#L37) | Captures pending backend input and binds the resulting snapshot to the runtime frame.<br>`frame`：The immutable frame state whose index labels the captured snapshot. | — | — |
| <a id="api-82156b8554e2eacb"></a>[`protected override void Inno.Input.Runtime.InputRuntime.OnStop()`](../../src/services/input/Inno.Input.Runtime/InputRuntime.cs#L45) | Releases the storage backend after owned work has quiesced. | — | — |
| <a id="api-02fa1b28832dad7b"></a>[`public Inno.Input.InputSnapshot Inno.Input.Runtime.InputRuntime.snapshot { get; private set; }`](../../src/services/input/Inno.Input.Runtime/InputRuntime.cs#L29) | Gets the immutable input state captured at the beginning of the current frame. | — | — |
| <a id="api-613e5fc74bd4ca61"></a>[`public sealed class Inno.Input.Runtime.InputRuntime : Inno.Runtime.Contracts.RuntimeSubsystem, Inno.Input.IInputService`](../../src/services/input/Inno.Input.Runtime/InputRuntime.cs#L11) | Captures one immutable input snapshot and binds it for the complete runtime frame. | — | — |

### `Inno.Input.Runtime.InputRuntimeFactory`

<!-- api: T:Inno.Input.Runtime.InputRuntimeFactory a958c35dbe4d139c068c96368875d82d7ac929af992f32fd6e62ca97a5a26c2c -->
<!-- api: M:Inno.Input.Runtime.InputRuntimeFactory.#ctor(System.Func{Inno.Runtime.Contracts.RuntimeSubsystemContext,Inno.Input.IInputBackend}) 25be1c57f404b6f22b1af6f22af455b78a04ad5bca0a785ecc1bc21a8010e098 -->
<!-- api: P:Inno.Input.Runtime.InputRuntimeFactory.descriptor 66cd6693390f9e7c07bd82278ed9704c3ad8d064d1bdda05b07d91c55ff214ac -->
<!-- api: M:Inno.Input.Runtime.InputRuntimeFactory.Create(Inno.Runtime.Contracts.RuntimeSubsystemContext) 1f8bd02f6179bfb712bbe4f08152e6662606ff090672d9f8b61ea91ea06bd00a -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-803d0f7c10f20dda"></a>[`public Inno.Input.Runtime.InputRuntimeFactory.InputRuntimeFactory(System.Func<Inno.Runtime.Contracts.RuntimeSubsystemContext, Inno.Input.IInputBackend> backendFactory)`](../../src/services/input/Inno.Input.Runtime/InputRuntimeFactory.cs#L21) | Creates a reusable factory that obtains an exclusively owned backend for each session.<br>`backendFactory`：The composition callback that creates a distinct backend for the supplied session context. | — | — |
| <a id="api-ff9bbcf391db084e"></a>[`public Inno.Runtime.Contracts.IRuntimeSubsystem Inno.Input.Runtime.InputRuntimeFactory.Create(Inno.Runtime.Contracts.RuntimeSubsystemContext context)`](../../src/services/input/Inno.Input.Runtime/InputRuntimeFactory.cs#L42) | Creates an input feature over a newly allocated session backend.<br>返回：A new input feature that exclusively owns its backend.<br>`context`：The isolated session context passed to the backend factory. | — | — |
| <a id="api-aef5fc1add4647ce"></a>[`public Inno.Runtime.Contracts.RuntimeSubsystemDescriptor Inno.Input.Runtime.InputRuntimeFactory.descriptor { get; }`](../../src/services/input/Inno.Input.Runtime/InputRuntimeFactory.cs#L29) | Gets stable ordering metadata for the frame-scoped input service. | — | — |
| <a id="api-15253179baf6ab18"></a>[`public sealed class Inno.Input.Runtime.InputRuntimeFactory : Inno.Runtime.Contracts.IRuntimeSubsystemFactory`](../../src/services/input/Inno.Input.Runtime/InputRuntimeFactory.cs#L11) | Creates one frame-scoped input service over a caller-selected platform backend. | — | — |

## 项目依赖

- [Inno.Input](Inno.Input.md)：公开引用边界由实际签名核对。
- [Inno.Runtime.Contracts](../runtime/Inno.Runtime.Contracts.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
