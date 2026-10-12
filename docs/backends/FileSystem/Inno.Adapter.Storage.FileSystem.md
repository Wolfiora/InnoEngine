# Inno.Adapter.Storage.FileSystem

## IO 与关闭竞态

直接 Dispose 与文件 IO 在同一个 gate 串行化，不能在 Write 的 finally Release 之前销毁 SemaphoreSlim。已有 waiter 可以观察 disposed 状态并退出；未请求 OS wait handle 的托管 semaphore 随最后的 waiter 回收。写入在原子替换前再次检查取消。Runtime owner 通常会先取消并排空操作，再调用 adapter Dispose。

[分类索引](README.md) · [Runtime](../../storage/Inno.Storage.Runtime.md) · [Wiki 首页](../../README.md)

`FileSystemApplicationStorage` 是默认本地 adapter。构造时固定绝对 sandbox root；每次操作重新验证现有路径组件，拒绝 symlink/reparse point 越界。

键的路径解析复用 `Inno.Core.IO.PathBoundary`，也支持宿主明确指定卷根目录；存储层不再维护另一套字符串前缀边界算法。

写入先在目标目录创建唯一临时文件，flush 后执行同目录 replace；失败或取消会清理临时文件。查询与修改通过实例 gate 串行化，列表使用稳定 ordinal key 顺序。Dispose 后全部 API 明确失败。

机器路径只存在于 Composition Root；脚本和 Mechanism 始终只依赖 `StorageKey` 与 `IApplicationStorage`。

## Composition provider

`FileSystemStorageBackendProvider()` 只创建注册描述，不初始化原生服务。`CreateStorage(string)` 是继承的 provider 创建扩展点，返回调用方拥有的服务。`id` 来自所属领域的内置稳定 ID；同一 provider 可在 composition 生命周期内创建独立服务，具体线程及进程 owner 约束仍由该实现执行。






## 本轮边界与所有权

宿主向 provider 注入物理根；StorageScope 只负责 namespace。生成路径受 sandbox 边界约束，不改变游戏逻辑 API。

## 源码归属

当前唯一源码 owner：`backends/FileSystem/runtime/Inno.Adapter.Storage.FileSystem/Inno.Adapter.Storage.FileSystem.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Adapter.Storage.FileSystem.FileSystemApplicationStorage`

<!-- api: T:Inno.Adapter.Storage.FileSystem.FileSystemApplicationStorage 2c9dc48583345e09b527f82e4c3f6dc563f3b9832df6091cc198ce21b23902df -->
<!-- api: M:Inno.Adapter.Storage.FileSystem.FileSystemApplicationStorage.#ctor(System.String) 1c42bd9bb90d33a6cdcd1e1f441fc8ae3e5757e5149b75524d0eeaf5cefbd29d -->
<!-- api: P:Inno.Adapter.Storage.FileSystem.FileSystemApplicationStorage.rootDirectory 9d01417a74b07af59808785247b262fb1cd0056eed9e9e633138e68130f15685 -->
<!-- api: M:Inno.Adapter.Storage.FileSystem.FileSystemApplicationStorage.ExistsAsync(Inno.Storage.StorageKey,System.Threading.CancellationToken) b5c45a02029ddd7967df7dab7b085dfee86bf184a460a217390b5de0d4b55e58 -->
<!-- api: M:Inno.Adapter.Storage.FileSystem.FileSystemApplicationStorage.ReadAsync(Inno.Storage.StorageKey,System.Threading.CancellationToken) 66af48a2fd70ce2ea57c6df1528ec331b198ecab147a51e0734431dfe7128645 -->
<!-- api: M:Inno.Adapter.Storage.FileSystem.FileSystemApplicationStorage.WriteAsync(Inno.Storage.StorageKey,System.ReadOnlyMemory{System.Byte},System.Threading.CancellationToken) bcf60d9a5b9f2148cfca7157b4e17f566089f3f69b8723f787126f72b90acea8 -->
<!-- api: M:Inno.Adapter.Storage.FileSystem.FileSystemApplicationStorage.DeleteAsync(Inno.Storage.StorageKey,System.Threading.CancellationToken) 32b344ddb6d8c31f2567afc185ff327e845f4af4c49b547713f302167a169450 -->
<!-- api: M:Inno.Adapter.Storage.FileSystem.FileSystemApplicationStorage.ListAsync(System.Nullable{Inno.Storage.StorageKey},System.Threading.CancellationToken) 4fb54c912692a602b93edba7f3b95387473635a8e120055638233c5d8997516c -->
<!-- api: M:Inno.Adapter.Storage.FileSystem.FileSystemApplicationStorage.Dispose 1d154dd3816f0c3568faaca86913093061d462ff95dd3e3fc41cb20758e8f053 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2777544c591d5dba"></a>[`public Inno.Adapter.Storage.FileSystem.FileSystemApplicationStorage.FileSystemApplicationStorage(string rootDirectory)`](../../../backends/FileSystem/runtime/Inno.Adapter.Storage.FileSystem/FileSystemApplicationStorage.cs#L34) | Creates a storage sandbox rooted at the supplied directory.<br>`rootDirectory`：The directory exclusively exposed through storage keys. | — | `System.ArgumentException`：Thrown when the root is blank.<br>`System.IO.IOException`：Thrown when the root itself is a symbolic link or reparse point. |
| <a id="api-39c2c6f6e0a333a0"></a>[`public System.Threading.Tasks.ValueTask<bool> Inno.Adapter.Storage.FileSystem.FileSystemApplicationStorage.DeleteAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/FileSystem/runtime/Inno.Adapter.Storage.FileSystem/FileSystemApplicationStorage.cs#L184) | Deletes one regular file without failing when it is absent.<br>返回：true when an existing value was deleted.<br>`key`：The sandbox-relative storage key.<br>`cancellationToken`：The token that cancels the operation before it commits. | — | — |
| <a id="api-8e94dda1ae3dad1b"></a>[`public void Inno.Adapter.Storage.FileSystem.FileSystemApplicationStorage.Dispose()`](../../../backends/FileSystem/runtime/Inno.Adapter.Storage.FileSystem/FileSystemApplicationStorage.cs#L248) | Releases synchronization resources retained by this adapter. | — | — |
| <a id="api-eb1a05dc556b9279"></a>[`public System.Threading.Tasks.ValueTask<bool> Inno.Adapter.Storage.FileSystem.FileSystemApplicationStorage.ExistsAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/FileSystem/runtime/Inno.Adapter.Storage.FileSystem/FileSystemApplicationStorage.cs#L59) | Determines whether a regular file exists for the supplied sandbox key.<br>返回：true when the value exists.<br>`key`：The sandbox-relative storage key.<br>`cancellationToken`：The token that cancels the operation before it commits. | — | — |
| <a id="api-223917850faa0ee7"></a>[`public System.Threading.Tasks.ValueTask<System.Collections.Generic.IReadOnlyList<Inno.Storage.StorageKey>> Inno.Adapter.Storage.FileSystem.FileSystemApplicationStorage.ListAsync(Inno.Storage.StorageKey? prefix = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/FileSystem/runtime/Inno.Adapter.Storage.FileSystem/FileSystemApplicationStorage.cs#L217) | Enumerates regular files below an optional logical prefix in deterministic order.<br>返回：An immutable key snapshot ordered by ordinal key value.<br>`prefix`：An optional normalized key prefix.<br>`cancellationToken`：The token that cancels the operation before it commits. | — | — |
| <a id="api-bbf862ff4dbfd074"></a>[`public System.Threading.Tasks.ValueTask<byte[]?> Inno.Adapter.Storage.FileSystem.FileSystemApplicationStorage.ReadAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/FileSystem/runtime/Inno.Adapter.Storage.FileSystem/FileSystemApplicationStorage.cs#L89) | Reads one complete immutable value after validating every existing path component.<br>返回：The stored bytes, or null when the key does not exist.<br>`key`：The sandbox-relative storage key.<br>`cancellationToken`：The token that cancels the operation before it commits. | — | — |
| <a id="api-a0eb37b1927b521e"></a>[`public System.Threading.Tasks.ValueTask Inno.Adapter.Storage.FileSystem.FileSystemApplicationStorage.WriteAsync(Inno.Storage.StorageKey key, System.ReadOnlyMemory<byte> value, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/FileSystem/runtime/Inno.Adapter.Storage.FileSystem/FileSystemApplicationStorage.cs#L124) | Atomically replaces one complete value through a temporary file in the target directory.<br>返回：A task that completes after the replacement has been flushed and committed.<br>`key`：The sandbox-relative storage key.<br>`value`：The complete value to commit.<br>`cancellationToken`：The token that cancels the operation before it commits. | — | — |
| <a id="api-384e39a2a5ea318b"></a>[`public string Inno.Adapter.Storage.FileSystem.FileSystemApplicationStorage.rootDirectory { get; }`](../../../backends/FileSystem/runtime/Inno.Adapter.Storage.FileSystem/FileSystemApplicationStorage.cs#L45) | Gets the absolute host directory that owns the sandbox. | — | — |
| <a id="api-79b273fd0ee44e5b"></a>[`public sealed class Inno.Adapter.Storage.FileSystem.FileSystemApplicationStorage : Inno.Storage.IApplicationStorage, System.IDisposable`](../../../backends/FileSystem/runtime/Inno.Adapter.Storage.FileSystem/FileSystemApplicationStorage.cs#L16) | Implements atomic application storage inside one isolated filesystem directory. | — | — |

### `Inno.Adapter.Storage.FileSystem.FileSystemStorageBackendProvider`

<!-- api: T:Inno.Adapter.Storage.FileSystem.FileSystemStorageBackendProvider 762db495943f7be99de93f65d8bcaaf91b3c0b339a75e2a04389cc84044c57c6 -->
<!-- api: M:Inno.Adapter.Storage.FileSystem.FileSystemStorageBackendProvider.#ctor(System.String) 64376eb1fa27d9bb1e74416b53d0864ced5a3a0eb2545a7cef4c3d5710ac2313 -->
<!-- api: M:Inno.Adapter.Storage.FileSystem.FileSystemStorageBackendProvider.CreateStorage(Inno.Storage.StorageScope) bb1611a683e04873502530e546f2e59abbdda96a1efb1de7012d90c9f68f1e31 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b1932ca3b82ebb6f"></a>[`public Inno.Adapter.Storage.FileSystem.FileSystemStorageBackendProvider.FileSystemStorageBackendProvider(string rootDirectory)`](../../../backends/FileSystem/runtime/Inno.Adapter.Storage.FileSystem/FileSystemStorageBackendProvider.cs#L23) | Captures the host-selected root beneath which application namespaces are isolated.<br>`rootDirectory`：The absolute writable data root owned by the host. | — | `System.ArgumentException`：The root is absent or not fully qualified. |
| <a id="api-75a2bda41b99082d"></a>[`public override Inno.Storage.IApplicationStorage Inno.Adapter.Storage.FileSystem.FileSystemStorageBackendProvider.CreateStorage(Inno.Storage.StorageScope scope)`](../../../backends/FileSystem/runtime/Inno.Adapter.Storage.FileSystem/FileSystemStorageBackendProvider.cs#L32) | Creates a caller-owned storage service using this implementation.<br>返回：A new service owned by the caller; providers must not return null.<br>`scope`：The application namespace to isolate within this provider's host configuration. | — | — |
| <a id="api-e934561af6fd34af"></a>[`public sealed class Inno.Adapter.Storage.FileSystem.FileSystemStorageBackendProvider : Inno.Adapter.Storage.StorageBackendProvider`](../../../backends/FileSystem/runtime/Inno.Adapter.Storage.FileSystem/FileSystemStorageBackendProvider.cs#L10) | Supplies the FileSystem implementation through the neutral storage creation boundary. | — | — |

## 项目依赖

- [Inno.Core.IO](../../core/Inno.Core.IO.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Storage](../../storage/Inno.Adapter.Storage.md)：公开引用边界由实际签名核对。
- [Inno.Storage](../../storage/Inno.Storage.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
