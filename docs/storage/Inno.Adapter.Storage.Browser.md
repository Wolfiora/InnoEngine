# Inno.Adapter.Storage.Browser

[Storage 索引](README.md) · [Wiki 首页](../README.md) · [Storage 契约](Inno.Storage.md) · [Browser Player](../runtime/Inno.Player.Browser.md)

## 职责与边界

浏览器的 `IApplicationStorage` 实现。使用同源 `localStorage` 保存应用数据，并以应用持久数据目录隔离 key；它不保存 Content Pack、脚本或引擎缓存。浏览器源站点的存储策略与配额由浏览器决定。

## 依赖与初始化

浏览器入口向 `DefaultAdapterCatalog` 注入 `BrowserStorageBackendProvider`；Runtime 使用当前 manifest 的 `persistentDataPath` 选择 sandbox。`BrowserApplicationStorage` 通过 JS interop 访问 origin storage，数据先编码为 Base64。应用脚本继续依赖 [Inno.Storage](Inno.Storage.md)，无需知道浏览器 API。

## Public API

| API | 语义 |
| --- | --- |
| `BrowserApplicationStorage(string rootDirectory)` | 为当前 origin 创建应用目录隔离的存储实例。 |
| `ExistsAsync(StorageKey, CancellationToken)` | 检查 key 是否存在。 |
| `ReadAsync(StorageKey, CancellationToken)` | 读取 bytes；不存在返回 `null`。 |
| `WriteAsync(StorageKey, ReadOnlyMemory<byte>, CancellationToken)` | 原子替换一个 key 的 bytes；浏览器配额失败会抛出异常。 |
| `DeleteAsync(StorageKey, CancellationToken)` | 删除 key，并返回是否原先存在。 |
| `ListAsync(StorageKey?, CancellationToken)` | 以 ordinal 顺序列出本应用的 key，可按路径前缀过滤。 |
| `BrowserStorageBackendProvider()`；`id`；`CreateStorage(string)` | 显式注册 `StorageBackendId.browser` 并创建 caller-owned 浏览器存储。 |

没有 `protected` 扩展点。取消在访问浏览器 storage 之前检查，`StorageKey` 无效时明确失败；不添加文件系统兼容旁路。

```csharp
IStorageBackendFactory factory = new StorageBackendCatalog([new BrowserStorageBackendProvider()]);
IApplicationStorage storage = factory.CreateStorage(StorageBackendId.browser, "my-game");
```

相邻项目：[FileSystem adapter](Inno.Adapter.Storage.FileSystem.md) · [Browser Player](../runtime/Inno.Player.Browser.md)。






## 本轮边界与所有权

将同一 StorageScope namespace 映射到浏览器 origin 存储。生命周期回调由 Browser 宿主注入，领域服务不判断浏览器平台。

## 当前源码公开 API 清单

以下仅列出当前程序集自己声明的 public/protected 契约；继承成员遵循所属基类页面。internal/private 实现不作为稳定公开 API。签名依据当前源码语义模型生成，行为、参数、异常与所有权说明同时以对应英文 XML 为准。

### `Inno.Adapter.Storage.Browser.BrowserApplicationStorage`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Storage.Browser.BrowserApplicationStorage.BrowserApplicationStorage(Inno.Storage.StorageScope scope)`](../../src/adapters/storage/Inno.Adapter.Storage.Browser/BrowserApplicationStorage.cs#L27) | Creates a namespace sandbox within the current browser origin. |
| [`System.Threading.Tasks.ValueTask<bool> Inno.Adapter.Storage.Browser.BrowserApplicationStorage.DeleteAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/adapters/storage/Inno.Adapter.Storage.Browser/BrowserApplicationStorage.cs#L112) | Removes a value from this browser origin. |
| [`System.Threading.Tasks.ValueTask<bool> Inno.Adapter.Storage.Browser.BrowserApplicationStorage.ExistsAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/adapters/storage/Inno.Adapter.Storage.Browser/BrowserApplicationStorage.cs#L46) | Checks whether a key has a value in this browser origin. |
| [`System.Threading.Tasks.ValueTask<System.Collections.Generic.IReadOnlyList<Inno.Storage.StorageKey>> Inno.Adapter.Storage.Browser.BrowserApplicationStorage.ListAsync(Inno.Storage.StorageKey? prefix = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/adapters/storage/Inno.Adapter.Storage.Browser/BrowserApplicationStorage.cs#L136) | Lists keys in this application sandbox, optionally under a prefix. |
| [`System.Threading.Tasks.ValueTask<byte[]?> Inno.Adapter.Storage.Browser.BrowserApplicationStorage.ReadAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/adapters/storage/Inno.Adapter.Storage.Browser/BrowserApplicationStorage.cs#L66) | Reads a stored value from this browser origin. |
| [`System.Threading.Tasks.ValueTask Inno.Adapter.Storage.Browser.BrowserApplicationStorage.WriteAsync(Inno.Storage.StorageKey key, System.ReadOnlyMemory<byte> value, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/adapters/storage/Inno.Adapter.Storage.Browser/BrowserApplicationStorage.cs#L90) | Replaces a value in this browser origin. |
| [`Inno.Adapter.Storage.Browser.BrowserApplicationStorage`](../../src/adapters/storage/Inno.Adapter.Storage.Browser/BrowserApplicationStorage.cs#L14) | Stores application values atomically in the browser origin's durable key-value store. |

### `Inno.Adapter.Storage.Browser.BrowserStorageBackendProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Storage.Browser.BrowserStorageBackendProvider.BrowserStorageBackendProvider()`](../../src/adapters/storage/Inno.Adapter.Storage.Browser/BrowserStorageBackendProvider.cs#L14) | Creates an explicitly composed registration for the bundled implementation. |
| [`override Inno.Storage.IApplicationStorage Inno.Adapter.Storage.Browser.BrowserStorageBackendProvider.CreateStorage(Inno.Storage.StorageScope scope)`](../../src/adapters/storage/Inno.Adapter.Storage.Browser/BrowserStorageBackendProvider.cs#L17) | See the implemented contract. |
| [`Inno.Adapter.Storage.Browser.BrowserStorageBackendProvider`](../../src/adapters/storage/Inno.Adapter.Storage.Browser/BrowserStorageBackendProvider.cs#L9) | Supplies the Browser implementation through the neutral storage creation boundary. |

## 项目依赖

- [Inno.Adapter.Storage](Inno.Adapter.Storage.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Storage](Inno.Storage.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：项目引用；公开签名可见性由语义边界检查确认。

共同 MSBuild 注入的 analyzer 与编译规则属于构建依赖，完整有效项目图记录在本轮验收证据中。
