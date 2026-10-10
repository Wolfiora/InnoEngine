# Inno.Storage

[Storage 索引](README.md) · [Runtime](Inno.Storage.Runtime.md) · [Wiki 首页](../README.md)

`Inno.Storage` 公开 `IApplicationStorage`，只接受规范化 `StorageKey`，不向脚本暴露绝对路径。全部操作支持 cancellation；写入语义是完整值原子替换。

## 公开 API

| API | 语义 |
| --- | --- |
| `StorageKey` | 不允许 rooted、空段、`.`、`..` 或 drive-qualified 的 slash key。 |
| `IApplicationStorage` | `ExistsAsync`、`ReadAsync`、`WriteAsync`、`DeleteAsync`、`ListAsync`。 |
| `StorageExecutionContext` | `AsyncLocal` 隔离且严格 LIFO。 |
| `Storage` | 脚本友好的无状态异步 façade。 |

```csharp
using InnoEngine.Storage;

StorageKey key = new("saves/slot-1.bin");
await Storage.WriteAsync(key, bytes);
byte[]? restored = await Storage.ReadAsync(key);
```

无活动 scope 时 façade 抛出明确异常。应用存档格式由项目或 Plugin 定义，不进入本体 Storage。

[下一页：Inno.Storage.Runtime](Inno.Storage.Runtime.md)






## 本轮边界与所有权

StorageScope 表达中立应用 namespace；不携带物理根。application ID 或显式配置决定 namespace，FileSystem provider 的宿主配置决定实际位置，Browser provider 映射到 origin 存储。Editor Edit/Play/Project 保持隔离，不添加 InnoEngine 产品路径前缀。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Storage.IApplicationStorage`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Storage.IApplicationStorage`](../../src/services/storage/Inno.Storage/IApplicationStorage.cs#L11) | Provides asynchronous atomic access to one application-specific persistent sandbox. |
| [`System.Threading.Tasks.ValueTask Inno.Storage.IApplicationStorage.WriteAsync(Inno.Storage.StorageKey key, System.ReadOnlyMemory<byte> value, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/storage/Inno.Storage/IApplicationStorage.cs#L62) | Atomically replaces a complete value. |
| [`System.Threading.Tasks.ValueTask<System.Collections.Generic.IReadOnlyList<Inno.Storage.StorageKey>> Inno.Storage.IApplicationStorage.ListAsync(Inno.Storage.StorageKey? prefix = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/storage/Inno.Storage/IApplicationStorage.cs#L97) | Lists immutable keys below an optional logical prefix. |
| [`System.Threading.Tasks.ValueTask<bool> Inno.Storage.IApplicationStorage.DeleteAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/storage/Inno.Storage/IApplicationStorage.cs#L80) | Deletes one value without failing when it is already absent. |
| [`System.Threading.Tasks.ValueTask<bool> Inno.Storage.IApplicationStorage.ExistsAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/storage/Inno.Storage/IApplicationStorage.cs#L25) | Determines whether a value exists. |
| [`System.Threading.Tasks.ValueTask<byte[]?> Inno.Storage.IApplicationStorage.ReadAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/storage/Inno.Storage/IApplicationStorage.cs#L42) | Reads a complete immutable value. |

### `Inno.Storage.Storage`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Storage.Storage`](../../src/services/storage/Inno.Storage/Storage.cs#L44) | Provides script-friendly access to the current application storage sandbox. |
| [`static System.Threading.Tasks.ValueTask Inno.Storage.Storage.WriteAsync(Inno.Storage.StorageKey key, System.ReadOnlyMemory<byte> value, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/storage/Inno.Storage/Storage.cs#L97) | Atomically replaces a complete value. |
| [`static System.Threading.Tasks.ValueTask<System.Collections.Generic.IReadOnlyList<Inno.Storage.StorageKey>> Inno.Storage.Storage.ListAsync(Inno.Storage.StorageKey? prefix = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/storage/Inno.Storage/Storage.cs#L134) | Lists keys below an optional prefix. |
| [`static System.Threading.Tasks.ValueTask<bool> Inno.Storage.Storage.DeleteAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/storage/Inno.Storage/Storage.cs#L116) | Deletes one value. |
| [`static System.Threading.Tasks.ValueTask<bool> Inno.Storage.Storage.ExistsAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/storage/Inno.Storage/Storage.cs#L58) | Determines whether a value exists. |
| [`static System.Threading.Tasks.ValueTask<byte[]?> Inno.Storage.Storage.ReadAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/storage/Inno.Storage/Storage.cs#L76) | Reads a complete value. |

### `Inno.Storage.StorageExecutionContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Storage.StorageExecutionContext`](../../src/services/storage/Inno.Storage/Storage.cs#L12) | Binds application storage to the current asynchronous execution context. |
| [`static Inno.Storage.IApplicationStorage Inno.Storage.StorageExecutionContext.current`](../../src/services/storage/Inno.Storage/Storage.cs#L22) | Gets the storage service bound to the current execution context. |
| [`static System.IDisposable Inno.Storage.StorageExecutionContext.EnterScope(Inno.Storage.IApplicationStorage storage)`](../../src/services/storage/Inno.Storage/Storage.cs#L33) | Binds a storage service until the returned strict last-in-first-out scope is disposed. |

### `Inno.Storage.StorageKey`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Storage.StorageKey`](../../src/services/storage/Inno.Storage/StorageKey.cs#L8) | Identifies one sandbox-relative application storage value. |
| [`Inno.Storage.StorageKey.StorageKey(string value)`](../../src/services/storage/Inno.Storage/StorageKey.cs#L19) | Creates a normalized storage key from slash-separated path segments. |
| [`bool Inno.Storage.StorageKey.isValid`](../../src/services/storage/Inno.Storage/StorageKey.cs#L44) | Gets whether this value contains a valid storage key. |
| [`override string Inno.Storage.StorageKey.ToString()`](../../src/services/storage/Inno.Storage/StorageKey.cs#L52) | Formats this key for diagnostics and persistence. |
| [`string Inno.Storage.StorageKey.value`](../../src/services/storage/Inno.Storage/StorageKey.cs#L39) | Gets the normalized slash-separated key value. |

### `Inno.Storage.StorageScope`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Storage.StorageScope`](../../src/services/storage/Inno.Storage/StorageScope.cs#L8) | Identifies an application data namespace independently of its physical storage location. |
| [`Inno.Storage.StorageScope.StorageScope(string value)`](../../src/services/storage/Inno.Storage/StorageScope.cs#L20) | Captures a portable, case-sensitive application namespace. |
| [`bool Inno.Storage.StorageScope.isValid`](../../src/services/storage/Inno.Storage/StorageScope.cs#L55) | Gets whether this value identifies a storage namespace. |
| [`override string Inno.Storage.StorageScope.ToString()`](../../src/services/storage/Inno.Storage/StorageScope.cs#L58) | See the implemented contract. |
| [`string? Inno.Storage.StorageScope.value`](../../src/services/storage/Inno.Storage/StorageScope.cs#L50) | Gets the logical namespace, or null for an unassigned value. |

## 项目依赖

- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
