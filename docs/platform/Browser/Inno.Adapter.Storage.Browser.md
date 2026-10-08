# Inno.Adapter.Storage.Browser

[分类索引](README.md) · [Wiki 首页](../../README.md) · [Storage 契约](../../storage/Inno.Storage.md) · [Browser Player](Inno.Player.Browser.md)

## 职责与边界

浏览器的 `IApplicationStorage` 实现。使用同源 `localStorage` 保存应用数据，并以应用持久数据目录隔离 key；它不保存 Content Pack、脚本或引擎缓存。浏览器源站点的存储策略与配额由浏览器决定。

## 依赖与初始化

浏览器入口向 `DefaultAdapterCatalog` 注入 `BrowserStorageBackendProvider`；Runtime 使用当前 manifest 的 `persistentDataPath` 选择 sandbox。`BrowserApplicationStorage` 通过 JS interop 访问 origin storage，数据先编码为 Base64。应用脚本继续依赖 [Inno.Storage](../../storage/Inno.Storage.md)，无需知道浏览器 API。

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

相邻项目：[FileSystem adapter](../../backends/FileSystem/Inno.Adapter.Storage.FileSystem.md) · [Browser Player](Inno.Player.Browser.md)。






## 本轮边界与所有权

将同一 StorageScope namespace 映射到浏览器 origin 存储。生命周期回调由 Browser 宿主注入，领域服务不判断浏览器平台。

## 源码归属

当前唯一源码 owner：`platforms/Browser/runtime/Inno.Adapter.Storage.Browser/Inno.Adapter.Storage.Browser.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Adapter.Storage.Browser.BrowserApplicationStorage`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Storage.Browser.BrowserApplicationStorage`](../../../platforms/Browser/runtime/Inno.Adapter.Storage.Browser/BrowserApplicationStorage.cs#L14) | Stores application values atomically in the browser origin's durable key-value store. |
| [`Inno.Adapter.Storage.Browser.BrowserApplicationStorage.BrowserApplicationStorage(Inno.Storage.StorageScope scope)`](../../../platforms/Browser/runtime/Inno.Adapter.Storage.Browser/BrowserApplicationStorage.cs#L27) | Creates a namespace sandbox within the current browser origin. |
| [`System.Threading.Tasks.ValueTask Inno.Adapter.Storage.Browser.BrowserApplicationStorage.WriteAsync(Inno.Storage.StorageKey key, System.ReadOnlyMemory<byte> value, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../platforms/Browser/runtime/Inno.Adapter.Storage.Browser/BrowserApplicationStorage.cs#L90) | Replaces a value in this browser origin. |
| [`System.Threading.Tasks.ValueTask<System.Collections.Generic.IReadOnlyList<Inno.Storage.StorageKey>> Inno.Adapter.Storage.Browser.BrowserApplicationStorage.ListAsync(Inno.Storage.StorageKey? prefix = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../platforms/Browser/runtime/Inno.Adapter.Storage.Browser/BrowserApplicationStorage.cs#L136) | Lists keys in this application sandbox, optionally under a prefix. |
| [`System.Threading.Tasks.ValueTask<bool> Inno.Adapter.Storage.Browser.BrowserApplicationStorage.DeleteAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../platforms/Browser/runtime/Inno.Adapter.Storage.Browser/BrowserApplicationStorage.cs#L112) | Removes a value from this browser origin. |
| [`System.Threading.Tasks.ValueTask<bool> Inno.Adapter.Storage.Browser.BrowserApplicationStorage.ExistsAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../platforms/Browser/runtime/Inno.Adapter.Storage.Browser/BrowserApplicationStorage.cs#L46) | Checks whether a key has a value in this browser origin. |
| [`System.Threading.Tasks.ValueTask<byte[]?> Inno.Adapter.Storage.Browser.BrowserApplicationStorage.ReadAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../platforms/Browser/runtime/Inno.Adapter.Storage.Browser/BrowserApplicationStorage.cs#L66) | Reads a stored value from this browser origin. |

### `Inno.Adapter.Storage.Browser.BrowserStorageBackendProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Storage.Browser.BrowserStorageBackendProvider`](../../../platforms/Browser/runtime/Inno.Adapter.Storage.Browser/BrowserStorageBackendProvider.cs#L9) | Supplies the Browser implementation through the neutral storage creation boundary. |
| [`Inno.Adapter.Storage.Browser.BrowserStorageBackendProvider.BrowserStorageBackendProvider()`](../../../platforms/Browser/runtime/Inno.Adapter.Storage.Browser/BrowserStorageBackendProvider.cs#L14) | Creates an explicitly composed registration for the bundled implementation. |
| [`override Inno.Storage.IApplicationStorage Inno.Adapter.Storage.Browser.BrowserStorageBackendProvider.CreateStorage(Inno.Storage.StorageScope scope)`](../../../platforms/Browser/runtime/Inno.Adapter.Storage.Browser/BrowserStorageBackendProvider.cs#L17) | See the implemented contract. |

## 项目依赖

- [Inno.Adapter.Storage](../../storage/Inno.Adapter.Storage.md)：公开引用边界由实际签名核对。
- [Inno.Storage](../../storage/Inno.Storage.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
