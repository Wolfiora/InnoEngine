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
using Inno.Adapter.Storage;
using Inno.Adapter.Storage.Browser;
using Inno.Storage;

static IApplicationStorage CreateOwnedBrowserStorage(StorageScope scope)
{
    IStorageBackendFactory factory = new StorageBackendCatalog([new BrowserStorageBackendProvider()]);
    return factory.CreateStorage(StorageBackendId.browser, scope);
}
```

相邻项目：[FileSystem adapter](../../backends/FileSystem/Inno.Adapter.Storage.FileSystem.md) · [Browser Player](Inno.Player.Browser.md)。






## 本轮边界与所有权

将同一 StorageScope namespace 映射到浏览器 origin 存储。生命周期回调由 Browser 宿主注入，领域服务不判断浏览器平台。

## 源码归属

当前唯一源码 owner：`platforms/Browser/runtime/Inno.Adapter.Storage.Browser/Inno.Adapter.Storage.Browser.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Adapter.Storage.Browser.BrowserApplicationStorage`

<!-- api: T:Inno.Adapter.Storage.Browser.BrowserApplicationStorage a0767c248a6449d416f29abe42d96de01966ccb36bca94c9d374c15dfc5bbbff -->
<!-- api: M:Inno.Adapter.Storage.Browser.BrowserApplicationStorage.#ctor(Inno.Storage.StorageScope) e7efaa1b160e0c3836cbdd1d3768c11eed6bda8c8219f44c40517ce715a8b5dd -->
<!-- api: M:Inno.Adapter.Storage.Browser.BrowserApplicationStorage.ExistsAsync(Inno.Storage.StorageKey,System.Threading.CancellationToken) e2c595ea21f1bd0527e9f6253292033fe162cb6436ed195a5108bfbb6f16b626 -->
<!-- api: M:Inno.Adapter.Storage.Browser.BrowserApplicationStorage.ReadAsync(Inno.Storage.StorageKey,System.Threading.CancellationToken) e8fa01fda0236e26a9e8bf84616ebe4a6bc5cbe95e4965e9ee25dcadebfca9e5 -->
<!-- api: M:Inno.Adapter.Storage.Browser.BrowserApplicationStorage.WriteAsync(Inno.Storage.StorageKey,System.ReadOnlyMemory{System.Byte},System.Threading.CancellationToken) 1324193c9e73d05f839bb47f9020307ff6e343fe16e5d0184dc17fd50392ab04 -->
<!-- api: M:Inno.Adapter.Storage.Browser.BrowserApplicationStorage.DeleteAsync(Inno.Storage.StorageKey,System.Threading.CancellationToken) 02c635682beb00181147e14f87837f0361a255cfa0046c7af5ada3df19016fb2 -->
<!-- api: M:Inno.Adapter.Storage.Browser.BrowserApplicationStorage.ListAsync(System.Nullable{Inno.Storage.StorageKey},System.Threading.CancellationToken) c5ee4e8ea6fbe0410a71b9ae5dccd3b1ff652331d748fae59243ed036d12da7c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a9d65c20c6c2db3f"></a>[`public Inno.Adapter.Storage.Browser.BrowserApplicationStorage.BrowserApplicationStorage(Inno.Storage.StorageScope scope)`](../../../platforms/Browser/runtime/Inno.Adapter.Storage.Browser/BrowserApplicationStorage.cs#L27) | Creates a namespace sandbox within the current browser origin.<br>`scope`：The application namespace used to isolate keys within the browser origin. | — | `System.ArgumentException`：The namespace is unassigned. |
| <a id="api-62016b0fa22e3a1a"></a>[`public System.Threading.Tasks.ValueTask<bool> Inno.Adapter.Storage.Browser.BrowserApplicationStorage.DeleteAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../platforms/Browser/runtime/Inno.Adapter.Storage.Browser/BrowserApplicationStorage.cs#L112) | Removes a value from this browser origin.<br>返回：True when the key existed and was removed.<br>`key`：The application key to remove.<br>`cancellationToken`：Cancels the operation before accessing storage. | — | — |
| <a id="api-4be58103740d5d5c"></a>[`public System.Threading.Tasks.ValueTask<bool> Inno.Adapter.Storage.Browser.BrowserApplicationStorage.ExistsAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../platforms/Browser/runtime/Inno.Adapter.Storage.Browser/BrowserApplicationStorage.cs#L46) | Checks whether a key has a value in this browser origin.<br>返回：True when a value exists for the key.<br>`key`：The application key to check.<br>`cancellationToken`：Cancels the operation before accessing storage. | — | — |
| <a id="api-c8d7f390ef00bb62"></a>[`public System.Threading.Tasks.ValueTask<System.Collections.Generic.IReadOnlyList<Inno.Storage.StorageKey>> Inno.Adapter.Storage.Browser.BrowserApplicationStorage.ListAsync(Inno.Storage.StorageKey? prefix = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../platforms/Browser/runtime/Inno.Adapter.Storage.Browser/BrowserApplicationStorage.cs#L136) | Lists keys in this application sandbox, optionally under a prefix.<br>返回：The matching keys in ordinal order.<br>`prefix`：The optional key prefix to filter.<br>`cancellationToken`：Cancels the operation before accessing storage. | — | — |
| <a id="api-db4cbc32700ef302"></a>[`public System.Threading.Tasks.ValueTask<byte[]?> Inno.Adapter.Storage.Browser.BrowserApplicationStorage.ReadAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../platforms/Browser/runtime/Inno.Adapter.Storage.Browser/BrowserApplicationStorage.cs#L66) | Reads a stored value from this browser origin.<br>返回：The stored bytes, or null when the key is absent.<br>`key`：The application key to read.<br>`cancellationToken`：Cancels the operation before accessing storage. | — | — |
| <a id="api-14fd6c7dc772468b"></a>[`public System.Threading.Tasks.ValueTask Inno.Adapter.Storage.Browser.BrowserApplicationStorage.WriteAsync(Inno.Storage.StorageKey key, System.ReadOnlyMemory<byte> value, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../platforms/Browser/runtime/Inno.Adapter.Storage.Browser/BrowserApplicationStorage.cs#L90) | Replaces a value in this browser origin.<br>返回：An operation that completes when the browser has stored the value.<br>`key`：The application key to write.<br>`value`：The bytes to store.<br>`cancellationToken`：Cancels the operation before accessing storage. | — | — |
| <a id="api-60ec91939d109ae4"></a>[`public sealed class Inno.Adapter.Storage.Browser.BrowserApplicationStorage : Inno.Storage.IApplicationStorage`](../../../platforms/Browser/runtime/Inno.Adapter.Storage.Browser/BrowserApplicationStorage.cs#L14) | Stores application values atomically in the browser origin's durable key-value store. | — | — |

### `Inno.Adapter.Storage.Browser.BrowserStorageBackendProvider`

<!-- api: T:Inno.Adapter.Storage.Browser.BrowserStorageBackendProvider 505cffb82da13b7a6f3d1e1bdf63079830447497a2d9f8e9f8b22c3ebe242cf5 -->
<!-- api: M:Inno.Adapter.Storage.Browser.BrowserStorageBackendProvider.#ctor c4454426741115d11f47790502db79babc0181a0c4862838d18f3ee2ad7fa874 -->
<!-- api: M:Inno.Adapter.Storage.Browser.BrowserStorageBackendProvider.CreateStorage(Inno.Storage.StorageScope) ed84807a88ef21979f3a0cf8f1fe8b6a66c6a44086db95124ed4bf977d3f90f1 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-0ed5cc75d9d9b67c"></a>[`public Inno.Adapter.Storage.Browser.BrowserStorageBackendProvider.BrowserStorageBackendProvider()`](../../../platforms/Browser/runtime/Inno.Adapter.Storage.Browser/BrowserStorageBackendProvider.cs#L14) | Creates an explicitly composed registration for the bundled implementation. | — | — |
| <a id="api-3a281aa9da61b07c"></a>[`public override Inno.Storage.IApplicationStorage Inno.Adapter.Storage.Browser.BrowserStorageBackendProvider.CreateStorage(Inno.Storage.StorageScope scope)`](../../../platforms/Browser/runtime/Inno.Adapter.Storage.Browser/BrowserStorageBackendProvider.cs#L17) | Creates a caller-owned storage service using this implementation.<br>返回：A new service owned by the caller; providers must not return null.<br>`scope`：The application namespace to isolate within this provider's host configuration. | — | — |
| <a id="api-29be02fa9d2099f3"></a>[`public sealed class Inno.Adapter.Storage.Browser.BrowserStorageBackendProvider : Inno.Adapter.Storage.StorageBackendProvider`](../../../platforms/Browser/runtime/Inno.Adapter.Storage.Browser/BrowserStorageBackendProvider.cs#L9) | Supplies the Browser implementation through the neutral storage creation boundary. | — | — |

## 项目依赖

- [Inno.Adapter.Storage](../../storage/Inno.Adapter.Storage.md)：公开引用边界由实际签名核对。
- [Inno.Storage](../../storage/Inno.Storage.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
