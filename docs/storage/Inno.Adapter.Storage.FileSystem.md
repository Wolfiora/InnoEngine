# Inno.Adapter.Storage.FileSystem

## IO 与关闭竞态

直接 Dispose 与文件 IO 在同一个 gate 串行化，不能在 Write 的 finally Release 之前销毁 SemaphoreSlim。已有 waiter 可以观察 disposed 状态并退出；未请求 OS wait handle 的托管 semaphore 随最后的 waiter 回收。写入在原子替换前再次检查取消。Runtime owner 通常会先取消并排空操作，再调用 adapter Dispose。

[Storage 索引](README.md) · [Runtime](Inno.Storage.Runtime.md) · [Wiki 首页](../README.md)

`FileSystemApplicationStorage` 是默认本地 adapter。构造时固定绝对 sandbox root；每次操作重新验证现有路径组件，拒绝 symlink/reparse point 越界。

键的路径解析复用 `Inno.Core.IO.PathBoundary`，也支持宿主明确指定卷根目录；存储层不再维护另一套字符串前缀边界算法。

写入先在目标目录创建唯一临时文件，flush 后执行同目录 replace；失败或取消会清理临时文件。查询与修改通过实例 gate 串行化，列表使用稳定 ordinal key 顺序。Dispose 后全部 API 明确失败。

机器路径只存在于 Composition Root；脚本和 Mechanism 始终只依赖 `StorageKey` 与 `IApplicationStorage`。

## Composition provider

`FileSystemStorageBackendProvider()` 只创建注册描述，不初始化原生服务。`CreateStorage(string)` 是继承的 provider 创建扩展点，返回调用方拥有的服务。`id` 来自所属领域的内置稳定 ID；同一 provider 可在 composition 生命周期内创建独立服务，具体线程及进程 owner 约束仍由该实现执行。






## 本轮边界与所有权

宿主向 provider 注入物理根；StorageScope 只负责 namespace。生成路径受 sandbox 边界约束，不改变游戏逻辑 API。

## 当前源码公开 API 清单

以下仅列出当前程序集自己声明的 public/protected 契约；继承成员遵循所属基类页面。internal/private 实现不作为稳定公开 API。签名依据当前源码语义模型生成，行为、参数、异常与所有权说明同时以对应英文 XML 为准。

### `Inno.Adapter.Storage.FileSystem.FileSystemApplicationStorage`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Storage.FileSystem.FileSystemApplicationStorage.FileSystemApplicationStorage(string rootDirectory)`](../../src/adapters/storage/Inno.Adapter.Storage.FileSystem/FileSystemApplicationStorage.cs#L34) | Creates a storage sandbox rooted at the supplied directory. |
| [`System.Threading.Tasks.ValueTask<bool> Inno.Adapter.Storage.FileSystem.FileSystemApplicationStorage.DeleteAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/adapters/storage/Inno.Adapter.Storage.FileSystem/FileSystemApplicationStorage.cs#L184) | Deletes one regular file without failing when it is absent. |
| [`void Inno.Adapter.Storage.FileSystem.FileSystemApplicationStorage.Dispose()`](../../src/adapters/storage/Inno.Adapter.Storage.FileSystem/FileSystemApplicationStorage.cs#L248) | Releases synchronization resources retained by this adapter. |
| [`System.Threading.Tasks.ValueTask<bool> Inno.Adapter.Storage.FileSystem.FileSystemApplicationStorage.ExistsAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/adapters/storage/Inno.Adapter.Storage.FileSystem/FileSystemApplicationStorage.cs#L59) | Determines whether a regular file exists for the supplied sandbox key. |
| [`System.Threading.Tasks.ValueTask<System.Collections.Generic.IReadOnlyList<Inno.Storage.StorageKey>> Inno.Adapter.Storage.FileSystem.FileSystemApplicationStorage.ListAsync(Inno.Storage.StorageKey? prefix = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/adapters/storage/Inno.Adapter.Storage.FileSystem/FileSystemApplicationStorage.cs#L217) | Enumerates regular files below an optional logical prefix in deterministic order. |
| [`System.Threading.Tasks.ValueTask<byte[]?> Inno.Adapter.Storage.FileSystem.FileSystemApplicationStorage.ReadAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/adapters/storage/Inno.Adapter.Storage.FileSystem/FileSystemApplicationStorage.cs#L89) | Reads one complete immutable value after validating every existing path component. |
| [`System.Threading.Tasks.ValueTask Inno.Adapter.Storage.FileSystem.FileSystemApplicationStorage.WriteAsync(Inno.Storage.StorageKey key, System.ReadOnlyMemory<byte> value, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/adapters/storage/Inno.Adapter.Storage.FileSystem/FileSystemApplicationStorage.cs#L124) | Atomically replaces one complete value through a temporary file in the target directory. |
| [`string Inno.Adapter.Storage.FileSystem.FileSystemApplicationStorage.rootDirectory`](../../src/adapters/storage/Inno.Adapter.Storage.FileSystem/FileSystemApplicationStorage.cs#L45) | Gets the absolute host directory that owns the sandbox. |
| [`Inno.Adapter.Storage.FileSystem.FileSystemApplicationStorage`](../../src/adapters/storage/Inno.Adapter.Storage.FileSystem/FileSystemApplicationStorage.cs#L16) | Implements atomic application storage inside one isolated filesystem directory. |

### `Inno.Adapter.Storage.FileSystem.FileSystemStorageBackendProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Storage.FileSystem.FileSystemStorageBackendProvider.FileSystemStorageBackendProvider(string rootDirectory)`](../../src/adapters/storage/Inno.Adapter.Storage.FileSystem/FileSystemStorageBackendProvider.cs#L23) | Captures the host-selected root beneath which application namespaces are isolated. |
| [`override Inno.Storage.IApplicationStorage Inno.Adapter.Storage.FileSystem.FileSystemStorageBackendProvider.CreateStorage(Inno.Storage.StorageScope scope)`](../../src/adapters/storage/Inno.Adapter.Storage.FileSystem/FileSystemStorageBackendProvider.cs#L32) | See the implemented contract. |
| [`Inno.Adapter.Storage.FileSystem.FileSystemStorageBackendProvider`](../../src/adapters/storage/Inno.Adapter.Storage.FileSystem/FileSystemStorageBackendProvider.cs#L10) | Supplies the FileSystem implementation through the neutral storage creation boundary. |

## 项目依赖

- [Inno.Core.IO](../core/Inno.Core.IO.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Adapter.Storage](Inno.Adapter.Storage.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Storage](Inno.Storage.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：项目引用；公开签名可见性由语义边界检查确认。

共同 MSBuild 注入的 analyzer 与编译规则属于构建依赖，完整有效项目图记录在本轮验收证据中。
