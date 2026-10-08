# Inno.Storage.Runtime

## 异步所有权

所有五种异步操作通过 LifetimeScope.RunAsync 接受并跟踪，使用调用者和 Runtime 组合取消信号。停止后拒绝新操作；有已接受操作仍运行时保留 backend 并抛 RetirementPendingException，完成后在 owner-thread 重试释放。

[Storage 索引](README.md) · [Contract](Inno.Storage.md) · [FileSystem Adapter](../backends/FileSystem/Inno.Adapter.Storage.FileSystem.md)

`Inno.Storage.Runtime` 把一个 `IApplicationStorage` 绑定到完整 Session 生命周期。RuntimeSubsystem 在 BeginFrame 打开 scope，EndFrame 恢复父 scope；Stop 等全部异步工作退休后释放实现了 `IDisposable` 的 adapter。

`StorageRuntimeFactory` 接受 Composition Root 的 factory callback，并为每个 Edit、Play 或 Player Session 创建独立存储实例。创建 null、重复 Subsystem ID 或依赖环会在 Session 启动时失败，不会进入半初始化状态。

[下一页：Inno.Adapter.Storage.FileSystem](../backends/FileSystem/Inno.Adapter.Storage.FileSystem.md)

## 接纳与公开状态

`StorageRuntime(storage, maxPendingOperations = 128, maxWriteBytes = 67108864)` 显式组合服务和预算。五种 operation 共用有界 LifetimeScope；容量不足在调用 backend 前拒绝。Write 在已接纳 operation 的同步前缀复制完整 bytes，调用者之后修改输入不会修改进行中的写入。预算不是磁盘配额，文件系统边界另由 adapter 负责。

`pendingOperations`、`rejectedOperations` 提供中立统计。公开存储能力为 ExistsAsync、ReadAsync、WriteAsync、DeleteAsync、ListAsync；`storage` 返回同一个受限服务，不能绕过接纳直接拿 backend。没有额外 protected 扩展点。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Storage.Runtime.StorageRuntime`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Storage.IApplicationStorage Inno.Storage.Runtime.StorageRuntime.storage`](../../src/services/storage/Inno.Storage.Runtime/StorageRuntime.cs#L49) | Gets the storage service owned by this runtime feature. |
| [`Inno.Storage.Runtime.StorageRuntime`](../../src/services/storage/Inno.Storage.Runtime/StorageRuntime.cs#L15) | Binds one application storage sandbox throughout every runtime frame. |
| [`Inno.Storage.Runtime.StorageRuntime.StorageRuntime(Inno.Storage.IApplicationStorage storage, int maxPendingOperations = 128, int maxWriteBytes = 67108864)`](../../src/services/storage/Inno.Storage.Runtime/StorageRuntime.cs#L33) | Creates a feature whose optional disposable storage ownership transfers to this instance. |
| [`System.Threading.Tasks.ValueTask Inno.Storage.Runtime.StorageRuntime.WriteAsync(Inno.Storage.StorageKey key, System.ReadOnlyMemory<byte> value, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/storage/Inno.Storage.Runtime/StorageRuntime.cs#L107) | Atomically replaces a complete value. |
| [`System.Threading.Tasks.ValueTask<System.Collections.Generic.IReadOnlyList<Inno.Storage.StorageKey>> Inno.Storage.Runtime.StorageRuntime.ListAsync(Inno.Storage.StorageKey? prefix = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/storage/Inno.Storage.Runtime/StorageRuntime.cs#L149) | Lists immutable keys below an optional logical prefix. |
| [`System.Threading.Tasks.ValueTask<bool> Inno.Storage.Runtime.StorageRuntime.DeleteAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/storage/Inno.Storage.Runtime/StorageRuntime.cs#L132) | Deletes one value without failing when it is already absent. |
| [`System.Threading.Tasks.ValueTask<bool> Inno.Storage.Runtime.StorageRuntime.ExistsAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/storage/Inno.Storage.Runtime/StorageRuntime.cs#L70) | Determines whether a value exists. |
| [`System.Threading.Tasks.ValueTask<byte[]?> Inno.Storage.Runtime.StorageRuntime.ReadAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/storage/Inno.Storage.Runtime/StorageRuntime.cs#L87) | Reads a complete immutable value. |
| [`int Inno.Storage.Runtime.StorageRuntime.pendingOperations`](../../src/services/storage/Inno.Storage.Runtime/StorageRuntime.cs#L53) | Gets asynchronous operations retained until completion or retirement reporting. |
| [`long Inno.Storage.Runtime.StorageRuntime.rejectedOperations`](../../src/services/storage/Inno.Storage.Runtime/StorageRuntime.cs#L57) | Gets operations rejected by finite asynchronous admission capacity. |
| [`override void Inno.Storage.Runtime.StorageRuntime.OnBeginFrame(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/services/storage/Inno.Storage.Runtime/StorageRuntime.cs#L161) | Begins a frame-scoped operation and makes queued work visible. |
| [`override void Inno.Storage.Runtime.StorageRuntime.OnStop()`](../../src/services/storage/Inno.Storage.Runtime/StorageRuntime.cs#L168) | Releases the storage backend after owned work has quiesced. |

### `Inno.Storage.Runtime.StorageRuntimeFactory`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Runtime.Contracts.IRuntimeSubsystem Inno.Storage.Runtime.StorageRuntimeFactory.Create(Inno.Runtime.Contracts.RuntimeSubsystemContext context)`](../../src/services/storage/Inno.Storage.Runtime/StorageRuntimeFactory.cs#L42) | Creates a storage feature over a newly allocated application sandbox. |
| [`Inno.Runtime.Contracts.RuntimeSubsystemDescriptor Inno.Storage.Runtime.StorageRuntimeFactory.descriptor`](../../src/services/storage/Inno.Storage.Runtime/StorageRuntimeFactory.cs#L29) | Gets stable ordering metadata for the frame-scoped storage service. |
| [`Inno.Storage.Runtime.StorageRuntimeFactory`](../../src/services/storage/Inno.Storage.Runtime/StorageRuntimeFactory.cs#L11) | Creates one frame-scoped storage service for a runtime session. |
| [`Inno.Storage.Runtime.StorageRuntimeFactory.StorageRuntimeFactory(System.Func<Inno.Runtime.Contracts.RuntimeSubsystemContext, Inno.Storage.IApplicationStorage> storageFactory)`](../../src/services/storage/Inno.Storage.Runtime/StorageRuntimeFactory.cs#L21) | Creates a reusable factory that obtains an exclusively owned storage sandbox for each session. |

## 项目依赖

- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Storage](Inno.Storage.md)：公开引用边界由实际签名核对。
- [Inno.Runtime.Contracts](../runtime/Inno.Runtime.Contracts.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
