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

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Input.Runtime.InputRuntime`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Input.InputSnapshot Inno.Input.Runtime.InputRuntime.snapshot`](../../src/services/input/Inno.Input.Runtime/InputRuntime.cs#L29) | Gets the immutable input state captured at the beginning of the current frame. |
| [`Inno.Input.Runtime.InputRuntime`](../../src/services/input/Inno.Input.Runtime/InputRuntime.cs#L11) | Captures one immutable input snapshot and binds it for the complete runtime frame. |
| [`Inno.Input.Runtime.InputRuntime.InputRuntime(Inno.Input.IInputBackend backend)`](../../src/services/input/Inno.Input.Runtime/InputRuntime.cs#L21) | Creates a feature whose backend ownership transfers to this instance. |
| [`override void Inno.Input.Runtime.InputRuntime.OnBeginFrame(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/services/input/Inno.Input.Runtime/InputRuntime.cs#L37) | Captures pending backend input and binds the resulting snapshot to the runtime frame. |
| [`override void Inno.Input.Runtime.InputRuntime.OnStop()`](../../src/services/input/Inno.Input.Runtime/InputRuntime.cs#L45) | Releases the storage backend after owned work has quiesced. |

### `Inno.Input.Runtime.InputRuntimeFactory`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Input.Runtime.InputRuntimeFactory`](../../src/services/input/Inno.Input.Runtime/InputRuntimeFactory.cs#L11) | Creates one frame-scoped input service over a caller-selected platform backend. |
| [`Inno.Input.Runtime.InputRuntimeFactory.InputRuntimeFactory(System.Func<Inno.Runtime.Contracts.RuntimeSubsystemContext, Inno.Input.IInputBackend> backendFactory)`](../../src/services/input/Inno.Input.Runtime/InputRuntimeFactory.cs#L21) | Creates a reusable factory that obtains an exclusively owned backend for each session. |
| [`Inno.Runtime.Contracts.IRuntimeSubsystem Inno.Input.Runtime.InputRuntimeFactory.Create(Inno.Runtime.Contracts.RuntimeSubsystemContext context)`](../../src/services/input/Inno.Input.Runtime/InputRuntimeFactory.cs#L42) | Creates an input feature over a newly allocated session backend. |
| [`Inno.Runtime.Contracts.RuntimeSubsystemDescriptor Inno.Input.Runtime.InputRuntimeFactory.descriptor`](../../src/services/input/Inno.Input.Runtime/InputRuntimeFactory.cs#L29) | Gets stable ordering metadata for the frame-scoped input service. |

## 项目依赖

- [Inno.Input](Inno.Input.md)：公开引用边界由实际签名核对。
- [Inno.Runtime.Contracts](../runtime/Inno.Runtime.Contracts.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
