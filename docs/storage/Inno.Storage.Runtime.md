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

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Storage.Runtime.StorageRuntime`

<!-- api: T:Inno.Storage.Runtime.StorageRuntime 6e43075d7a9ce2651e665adce1a121a816a961807498b9cb2fbcb759a7348f53 -->
<!-- api: M:Inno.Storage.Runtime.StorageRuntime.#ctor(Inno.Storage.IApplicationStorage,System.Int32,System.Int32) 15d05c7f4042ce62b17c68f4c61ebceb43813dfb835a99d5ee146c6fa2202211 -->
<!-- api: P:Inno.Storage.Runtime.StorageRuntime.storage 6fb662360d40fc606ae6c08d0fdb51f8e45a181d1df2ccbfe76ee41d7fb4c679 -->
<!-- api: P:Inno.Storage.Runtime.StorageRuntime.pendingOperations 8ec479343007be57ba49a61e23e5ce3e87a5e20636b26062caac976206138e7a -->
<!-- api: P:Inno.Storage.Runtime.StorageRuntime.rejectedOperations 29535023777f5cdcdc0c22a813afe08aaabc662b76dd4d2d1d808d6660475152 -->
<!-- api: M:Inno.Storage.Runtime.StorageRuntime.ExistsAsync(Inno.Storage.StorageKey,System.Threading.CancellationToken) 02fc3444e7d31d5bd7ad9783fc26f2386a8bbc11c6e21300de2edc3fc3d2a86a -->
<!-- api: M:Inno.Storage.Runtime.StorageRuntime.ReadAsync(Inno.Storage.StorageKey,System.Threading.CancellationToken) 0ab065f8f1090dcc1abeebc351660bdcbbdd44e830738c760c749c8dcea1c835 -->
<!-- api: M:Inno.Storage.Runtime.StorageRuntime.WriteAsync(Inno.Storage.StorageKey,System.ReadOnlyMemory{System.Byte},System.Threading.CancellationToken) c28decc838fd4adb2e1fa114a3185d89d8185cbafecdfdca968f933b716f1c22 -->
<!-- api: M:Inno.Storage.Runtime.StorageRuntime.DeleteAsync(Inno.Storage.StorageKey,System.Threading.CancellationToken) 2443256a43cbebe08f44e9b4e042532951670becfd5b863d5f289269c4f233fe -->
<!-- api: M:Inno.Storage.Runtime.StorageRuntime.ListAsync(System.Nullable{Inno.Storage.StorageKey},System.Threading.CancellationToken) cfb0ccc61830a3eeb2752e90de0799532a541d57b7f8d97a18140c156afa3fb8 -->
<!-- api: M:Inno.Storage.Runtime.StorageRuntime.OnBeginFrame(Inno.Runtime.Contracts.RuntimeFrame) d814a06c4c66d74de1ab6652b18bc5a397bc28223f3702ee7c8f86566cd41cca -->
<!-- api: M:Inno.Storage.Runtime.StorageRuntime.OnStop f2a19f61b0b98843f2b225ab4c90d9385e2648e008dc6d312127ed9923985a66 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-73e19306e1c5dc01"></a>[`public Inno.Storage.Runtime.StorageRuntime.StorageRuntime(Inno.Storage.IApplicationStorage storage, int maxPendingOperations = 128, int maxWriteBytes = 67108864)`](../../src/services/storage/Inno.Storage.Runtime/StorageRuntime.cs#L33) | Creates a feature whose optional disposable storage ownership transfers to this instance.<br>`storage`：The application-specific persistent storage sandbox.<br>`maxPendingOperations`：Positive simultaneous asynchronous operation capacity.<br>`maxWriteBytes`：Positive maximum value size accepted by one write. | — | — |
| <a id="api-c7da3361bfa8301a"></a>[`public System.Threading.Tasks.ValueTask<bool> Inno.Storage.Runtime.StorageRuntime.DeleteAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/storage/Inno.Storage.Runtime/StorageRuntime.cs#L132) | Deletes one value without failing when it is already absent.<br>返回：true when an existing value was deleted.<br>`key`：The sandbox-relative storage key.<br>`cancellationToken`：Cancellation requested by the caller. | — | — |
| <a id="api-5e7e6a31388f0288"></a>[`public System.Threading.Tasks.ValueTask<bool> Inno.Storage.Runtime.StorageRuntime.ExistsAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/storage/Inno.Storage.Runtime/StorageRuntime.cs#L70) | Determines whether a value exists.<br>返回：true when the value exists.<br>`key`：The sandbox-relative storage key.<br>`cancellationToken`：Cancellation requested by the caller. | — | — |
| <a id="api-ec0d96ab8bc36d5d"></a>[`public System.Threading.Tasks.ValueTask<System.Collections.Generic.IReadOnlyList<Inno.Storage.StorageKey>> Inno.Storage.Runtime.StorageRuntime.ListAsync(Inno.Storage.StorageKey? prefix = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/storage/Inno.Storage.Runtime/StorageRuntime.cs#L149) | Lists immutable keys below an optional logical prefix.<br>返回：Keys in deterministic ordinal order.<br>`prefix`：An optional normalized directory prefix.<br>`cancellationToken`：Cancellation requested by the caller. | — | — |
| <a id="api-ac69721c3cb8fcb1"></a>[`protected override void Inno.Storage.Runtime.StorageRuntime.OnBeginFrame(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/services/storage/Inno.Storage.Runtime/StorageRuntime.cs#L161) | Begins a frame-scoped operation and makes queued work visible.<br>`frame`：The frame consumed by begin frame; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-10db344123697667"></a>[`protected override void Inno.Storage.Runtime.StorageRuntime.OnStop()`](../../src/services/storage/Inno.Storage.Runtime/StorageRuntime.cs#L168) | Releases the storage backend after owned work has quiesced. | — | — |
| <a id="api-ee91c472d8ee6130"></a>[`public System.Threading.Tasks.ValueTask<byte[]?> Inno.Storage.Runtime.StorageRuntime.ReadAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/storage/Inno.Storage.Runtime/StorageRuntime.cs#L87) | Reads a complete immutable value.<br>返回：The stored bytes, or null when the key does not exist.<br>`key`：The sandbox-relative storage key.<br>`cancellationToken`：Cancellation requested by the caller. | — | — |
| <a id="api-38f448978f95d725"></a>[`public System.Threading.Tasks.ValueTask Inno.Storage.Runtime.StorageRuntime.WriteAsync(Inno.Storage.StorageKey key, System.ReadOnlyMemory<byte> value, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/storage/Inno.Storage.Runtime/StorageRuntime.cs#L107) | Atomically replaces a complete value.<br>返回：A task that completes after the value is durably replaced.<br>`key`：The sandbox-relative storage key.<br>`value`：The complete value to commit.<br>`cancellationToken`：Cancellation requested by the caller. | — | — |
| <a id="api-7f2026b183e48ecc"></a>[`public int Inno.Storage.Runtime.StorageRuntime.pendingOperations { get; }`](../../src/services/storage/Inno.Storage.Runtime/StorageRuntime.cs#L53) | Gets asynchronous operations retained until completion or retirement reporting. | — | — |
| <a id="api-1f613c37887b061e"></a>[`public long Inno.Storage.Runtime.StorageRuntime.rejectedOperations { get; }`](../../src/services/storage/Inno.Storage.Runtime/StorageRuntime.cs#L57) | Gets operations rejected by finite asynchronous admission capacity. | — | — |
| <a id="api-86522c9ad5674494"></a>[`public Inno.Storage.IApplicationStorage Inno.Storage.Runtime.StorageRuntime.storage { get; }`](../../src/services/storage/Inno.Storage.Runtime/StorageRuntime.cs#L49) | Gets the storage service owned by this runtime feature. | — | — |
| <a id="api-e3b2203bf4cb1021"></a>[`public sealed class Inno.Storage.Runtime.StorageRuntime : Inno.Runtime.Contracts.RuntimeSubsystem, Inno.Storage.IApplicationStorage`](../../src/services/storage/Inno.Storage.Runtime/StorageRuntime.cs#L15) | Binds one application storage sandbox throughout every runtime frame. | — | — |

### `Inno.Storage.Runtime.StorageRuntimeFactory`

<!-- api: T:Inno.Storage.Runtime.StorageRuntimeFactory 79c25abce7079ccc3a943b99c4bca5043e832bb06daba87c92aa808557264ce0 -->
<!-- api: M:Inno.Storage.Runtime.StorageRuntimeFactory.#ctor(System.Func{Inno.Runtime.Contracts.RuntimeSubsystemContext,Inno.Storage.IApplicationStorage}) 0e3b118e72a3bd1a3359db96df51f64fae619489eb64d506c9e5913afe488af0 -->
<!-- api: P:Inno.Storage.Runtime.StorageRuntimeFactory.descriptor 401533f45b2b3150bfdb555c04ced6a6be1de937d4a4feea41052d84c05d6a57 -->
<!-- api: M:Inno.Storage.Runtime.StorageRuntimeFactory.Create(Inno.Runtime.Contracts.RuntimeSubsystemContext) d11b1e7587c90dc7e2aba7c2f4b8c8ed6e831925852782a899a809d33418c5f2 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-51339ba0ca7e7b1c"></a>[`public Inno.Storage.Runtime.StorageRuntimeFactory.StorageRuntimeFactory(System.Func<Inno.Runtime.Contracts.RuntimeSubsystemContext, Inno.Storage.IApplicationStorage> storageFactory)`](../../src/services/storage/Inno.Storage.Runtime/StorageRuntimeFactory.cs#L21) | Creates a reusable factory that obtains an exclusively owned storage sandbox for each session.<br>`storageFactory`：The composition callback that creates a distinct storage service for the supplied session context. | — | — |
| <a id="api-39a04905f1d35c09"></a>[`public Inno.Runtime.Contracts.IRuntimeSubsystem Inno.Storage.Runtime.StorageRuntimeFactory.Create(Inno.Runtime.Contracts.RuntimeSubsystemContext context)`](../../src/services/storage/Inno.Storage.Runtime/StorageRuntimeFactory.cs#L42) | Creates a storage feature over a newly allocated application sandbox.<br>返回：A new storage feature that exclusively owns any disposable storage adapter.<br>`context`：The isolated session context passed to the storage factory. | — | — |
| <a id="api-d4ffbbada6985f6c"></a>[`public Inno.Runtime.Contracts.RuntimeSubsystemDescriptor Inno.Storage.Runtime.StorageRuntimeFactory.descriptor { get; }`](../../src/services/storage/Inno.Storage.Runtime/StorageRuntimeFactory.cs#L29) | Gets stable ordering metadata for the frame-scoped storage service. | — | — |
| <a id="api-c62857eae2db764a"></a>[`public sealed class Inno.Storage.Runtime.StorageRuntimeFactory : Inno.Runtime.Contracts.IRuntimeSubsystemFactory`](../../src/services/storage/Inno.Storage.Runtime/StorageRuntimeFactory.cs#L11) | Creates one frame-scoped storage service for a runtime session. | — | — |

## 项目依赖

- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Storage](Inno.Storage.md)：公开引用边界由实际签名核对。
- [Inno.Runtime.Contracts](../runtime/Inno.Runtime.Contracts.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
