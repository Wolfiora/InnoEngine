# Inno.Adapter.Storage.Browser

[Storage 索引](README.md) · [Wiki 首页](../README.md) · [Storage 契约](Inno.Storage.md) · [Browser Player](../runtime/Inno.Player.Browser.md)

## 职责与边界

浏览器的 `IApplicationStorage` 实现。使用同源 `localStorage` 保存应用数据，并以应用持久数据目录隔离 key；它不保存 Content Pack、脚本或引擎缓存。浏览器源站点的存储策略与配额由浏览器决定。

## 依赖与初始化

浏览器入口向 `DefaultAdapterCatalog` 注入 `BrowserStorageBackendFactory`；Runtime 使用当前 manifest 的 `persistentDataPath` 选择 sandbox。`BrowserApplicationStorage` 通过 JS interop 访问 origin storage，数据先编码为 Base64。应用脚本继续依赖 [Inno.Storage](Inno.Storage.md)，无需知道浏览器 API。

## Public API

| API | 语义 |
| --- | --- |
| `BrowserApplicationStorage(string rootDirectory)` | 为当前 origin 创建应用目录隔离的存储实例。 |
| `ExistsAsync(StorageKey, CancellationToken)` | 检查 key 是否存在。 |
| `ReadAsync(StorageKey, CancellationToken)` | 读取 bytes；不存在返回 `null`。 |
| `WriteAsync(StorageKey, ReadOnlyMemory<byte>, CancellationToken)` | 原子替换一个 key 的 bytes；浏览器配额失败会抛出异常。 |
| `DeleteAsync(StorageKey, CancellationToken)` | 删除 key，并返回是否原先存在。 |
| `ListAsync(StorageKey?, CancellationToken)` | 以 ordinal 顺序列出本应用的 key，可按路径前缀过滤。 |
| `BrowserStorageBackendFactory.CreateStorage(StorageBackend, string)` | 为标准 backend 创建浏览器 storage；其他 backend 抛出 `NotSupportedException`。 |

没有 `protected` 扩展点。取消在访问浏览器 storage 之前检查，`StorageKey` 无效时明确失败；不添加文件系统兼容旁路。

```csharp
IStorageBackendFactory factory = new BrowserStorageBackendFactory();
IApplicationStorage storage = factory.CreateStorage(StorageBackend.FileSystem, "my-game");
```

相邻项目：[FileSystem adapter](Inno.Adapter.Storage.FileSystem.md) · [Browser Player](../runtime/Inno.Player.Browser.md)。
