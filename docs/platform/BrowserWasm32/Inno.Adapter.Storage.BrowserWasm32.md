# Inno.Adapter.Storage.BrowserWasm32

[分类索引](README.md) · [Wiki 首页](../../README.md) · [Storage 契约](../../storage/Inno.Storage.md) · [Browser Player](Inno.Player.BrowserWasm32.md)

## 职责与边界

浏览器的 `IApplicationStorage` 实现。使用同源 `localStorage` 保存应用数据，并以应用持久数据目录隔离 key；它不保存 Content Pack、脚本或引擎缓存。浏览器源站点的存储策略与配额由浏览器决定。

## 依赖与初始化

浏览器入口向 `DefaultAdapterCatalog` 注入 `BrowserWasm32StorageBackendProvider`；Runtime 使用当前 manifest 的 `persistentDataPath` 选择 sandbox。`BrowserWasm32ApplicationStorage` 通过 JS interop 访问 origin storage，数据先编码为 Base64。应用脚本继续依赖 [Inno.Storage](../../storage/Inno.Storage.md)，无需知道浏览器 API。

## Public API

| API | 语义 |
| --- | --- |
| `BrowserWasm32ApplicationStorage(string rootDirectory)` | 为当前 origin 创建应用目录隔离的存储实例。 |
| `ExistsAsync(StorageKey, CancellationToken)` | 检查 key 是否存在。 |
| `ReadAsync(StorageKey, CancellationToken)` | 读取 bytes；不存在返回 `null`。 |
| `WriteAsync(StorageKey, ReadOnlyMemory<byte>, CancellationToken)` | 原子替换一个 key 的 bytes；浏览器配额失败会抛出异常。 |
| `DeleteAsync(StorageKey, CancellationToken)` | 删除 key，并返回是否原先存在。 |
| `ListAsync(StorageKey?, CancellationToken)` | 以 ordinal 顺序列出本应用的 key，可按路径前缀过滤。 |
| `BrowserWasm32StorageBackendProvider()`；`id`；`CreateStorage(string)` | 显式注册 `StorageBackendId.browser` 并创建 caller-owned 浏览器存储。 |

没有 `protected` 扩展点。取消在访问浏览器 storage 之前检查，`StorageKey` 无效时明确失败；不添加文件系统兼容旁路。

```csharp
using Inno.Adapter.Storage;
using Inno.Adapter.Storage.BrowserWasm32;
using Inno.Storage;

static IApplicationStorage CreateOwnedBrowserStorage(StorageScope scope)
{
    IStorageBackendFactory factory = new StorageBackendCatalog([new BrowserWasm32StorageBackendProvider()]);
    return factory.CreateStorage(StorageBackendId.browser, scope);
}
```

相邻项目：[FileSystem adapter](../../backends/FileSystem/Inno.Adapter.Storage.FileSystem.md) · [Browser Player](Inno.Player.BrowserWasm32.md)。






## 本轮边界与所有权

将同一 StorageScope namespace 映射到浏览器 origin 存储。生命周期回调由 Browser 宿主注入，领域服务不判断浏览器平台。

## 源码归属

当前唯一源码 owner：`platforms/BrowserWasm32/runtime/Inno.Adapter.Storage.BrowserWasm32/Inno.Adapter.Storage.BrowserWasm32.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

签名来自当前 SDK 求值与语义符号；按类型列出 public/protected 契约。工作流与生命周期见本页前文。

### `Inno.Adapter.Storage.BrowserWasm32.BrowserWasm32ApplicationStorage`

<!-- api: T:Inno.Adapter.Storage.BrowserWasm32.BrowserWasm32ApplicationStorage 70bd662d09bca2fbc55bf0fac355afe552e14a8b286720bd968e44a24fdd6cfb -->
<!-- api: M:Inno.Adapter.Storage.BrowserWasm32.BrowserWasm32ApplicationStorage.#ctor(Inno.Storage.StorageScope) f08945127584a243f843a015ec942b3bb518db852762fefd4720c5c410c4da1c -->
<!-- api: M:Inno.Adapter.Storage.BrowserWasm32.BrowserWasm32ApplicationStorage.ExistsAsync(Inno.Storage.StorageKey,System.Threading.CancellationToken) 0c51d2f73432700435456bdc01c8483dd58e70c8d42b1ea9a2fade5c5f9b9801 -->
<!-- api: M:Inno.Adapter.Storage.BrowserWasm32.BrowserWasm32ApplicationStorage.ReadAsync(Inno.Storage.StorageKey,System.Threading.CancellationToken) 20fb01c065200aa6b10ba9cbafdcc254a111c5d769f8e430777cbeb99e8b66d1 -->
<!-- api: M:Inno.Adapter.Storage.BrowserWasm32.BrowserWasm32ApplicationStorage.WriteAsync(Inno.Storage.StorageKey,System.ReadOnlyMemory{System.Byte},System.Threading.CancellationToken) 0ed97dbe004a5690003966a74e20e5f84fd4a256488baef1d688f547f57423e3 -->
<!-- api: M:Inno.Adapter.Storage.BrowserWasm32.BrowserWasm32ApplicationStorage.DeleteAsync(Inno.Storage.StorageKey,System.Threading.CancellationToken) 2fafd0954954bf787c0a15d993258841ea108488a2979b55ee362ec3b09acd97 -->
<!-- api: M:Inno.Adapter.Storage.BrowserWasm32.BrowserWasm32ApplicationStorage.ListAsync(System.Nullable{Inno.Storage.StorageKey},System.Threading.CancellationToken) 01867939c15da7287eb7c94a560027b65313dfde7e1c45a4d26ec0f46160d771 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-02745bb724fe0f89"></a>[`public Inno.Adapter.Storage.BrowserWasm32.BrowserWasm32ApplicationStorage.BrowserWasm32ApplicationStorage(Inno.Storage.StorageScope scope)`](../../../platforms/BrowserWasm32/runtime/Inno.Adapter.Storage.BrowserWasm32/BrowserWasm32ApplicationStorage.cs#L27) | Creates a namespace sandbox within the current browser origin.<br>`scope`：The application namespace used to isolate keys within the browser origin. | 约束由所属类型与本页生命周期定义。 | `System.ArgumentException`：The namespace is unassigned. |
| <a id="api-fba4a6cb03751198"></a>[`public System.Threading.Tasks.ValueTask<bool> Inno.Adapter.Storage.BrowserWasm32.BrowserWasm32ApplicationStorage.DeleteAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../platforms/BrowserWasm32/runtime/Inno.Adapter.Storage.BrowserWasm32/BrowserWasm32ApplicationStorage.cs#L112) | Removes a value from this browser origin.<br>返回：True when the key existed and was removed.<br>`key`：The application key to remove.<br>`cancellationToken`：Cancels the operation before accessing storage. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-fd0c95f5776797d1"></a>[`public System.Threading.Tasks.ValueTask<bool> Inno.Adapter.Storage.BrowserWasm32.BrowserWasm32ApplicationStorage.ExistsAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../platforms/BrowserWasm32/runtime/Inno.Adapter.Storage.BrowserWasm32/BrowserWasm32ApplicationStorage.cs#L46) | Checks whether a key has a value in this browser origin.<br>返回：True when a value exists for the key.<br>`key`：The application key to check.<br>`cancellationToken`：Cancels the operation before accessing storage. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-e91ca09b8d499f3d"></a>[`public System.Threading.Tasks.ValueTask<System.Collections.Generic.IReadOnlyList<Inno.Storage.StorageKey>> Inno.Adapter.Storage.BrowserWasm32.BrowserWasm32ApplicationStorage.ListAsync(Inno.Storage.StorageKey? prefix = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../platforms/BrowserWasm32/runtime/Inno.Adapter.Storage.BrowserWasm32/BrowserWasm32ApplicationStorage.cs#L136) | Lists keys in this application sandbox, optionally under a prefix.<br>返回：The matching keys in ordinal order.<br>`prefix`：The optional key prefix to filter.<br>`cancellationToken`：Cancels the operation before accessing storage. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-d2009c0e58d50863"></a>[`public System.Threading.Tasks.ValueTask<byte[]?> Inno.Adapter.Storage.BrowserWasm32.BrowserWasm32ApplicationStorage.ReadAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../platforms/BrowserWasm32/runtime/Inno.Adapter.Storage.BrowserWasm32/BrowserWasm32ApplicationStorage.cs#L66) | Reads a stored value from this browser origin.<br>返回：The stored bytes, or null when the key is absent.<br>`key`：The application key to read.<br>`cancellationToken`：Cancels the operation before accessing storage. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-3ea9bb59861421cf"></a>[`public System.Threading.Tasks.ValueTask Inno.Adapter.Storage.BrowserWasm32.BrowserWasm32ApplicationStorage.WriteAsync(Inno.Storage.StorageKey key, System.ReadOnlyMemory<byte> value, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../platforms/BrowserWasm32/runtime/Inno.Adapter.Storage.BrowserWasm32/BrowserWasm32ApplicationStorage.cs#L90) | Replaces a value in this browser origin.<br>返回：An operation that completes when the browser has stored the value.<br>`key`：The application key to write.<br>`value`：The bytes to store.<br>`cancellationToken`：Cancels the operation before accessing storage. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-60d09eeba6c4e8b7"></a>[`public sealed class Inno.Adapter.Storage.BrowserWasm32.BrowserWasm32ApplicationStorage : Inno.Storage.IApplicationStorage`](../../../platforms/BrowserWasm32/runtime/Inno.Adapter.Storage.BrowserWasm32/BrowserWasm32ApplicationStorage.cs#L14) | Stores application values atomically in the browser origin's durable key-value store. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Adapter.Storage.BrowserWasm32.BrowserWasm32StorageBackendProvider`

<!-- api: T:Inno.Adapter.Storage.BrowserWasm32.BrowserWasm32StorageBackendProvider 43915d0ccbad2560a84e55fc3352c097f10c12f50b1f43aec18902bb1150431b -->
<!-- api: M:Inno.Adapter.Storage.BrowserWasm32.BrowserWasm32StorageBackendProvider.#ctor ed9e5877406f1007612056a9da8c4f380ce2e2f5b9ab0bcba5517fe4b4077167 -->
<!-- api: M:Inno.Adapter.Storage.BrowserWasm32.BrowserWasm32StorageBackendProvider.CreateStorage(Inno.Storage.StorageScope) 8ec71dada6bf6bae1a144ba76e493ffb96cff9cfbc418d9a79e3112bb355e50e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4ad0b2c58cf0f817"></a>[`public Inno.Adapter.Storage.BrowserWasm32.BrowserWasm32StorageBackendProvider.BrowserWasm32StorageBackendProvider()`](../../../platforms/BrowserWasm32/runtime/Inno.Adapter.Storage.BrowserWasm32/BrowserWasm32StorageBackendProvider.cs#L14) | Creates an explicitly composed registration for the bundled implementation. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-4416a3a52820ff78"></a>[`public override Inno.Storage.IApplicationStorage Inno.Adapter.Storage.BrowserWasm32.BrowserWasm32StorageBackendProvider.CreateStorage(Inno.Storage.StorageScope scope)`](../../../platforms/BrowserWasm32/runtime/Inno.Adapter.Storage.BrowserWasm32/BrowserWasm32StorageBackendProvider.cs#L17) | Creates a caller-owned storage service using this implementation.<br>返回：A new service owned by the caller; providers must not return null.<br>`scope`：The application namespace to isolate within this provider's host configuration. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-a80e3b2a0cd06f3d"></a>[`public sealed class Inno.Adapter.Storage.BrowserWasm32.BrowserWasm32StorageBackendProvider : Inno.Adapter.Storage.StorageBackendProvider`](../../../platforms/BrowserWasm32/runtime/Inno.Adapter.Storage.BrowserWasm32/BrowserWasm32StorageBackendProvider.cs#L9) | Supplies the Browser implementation through the neutral storage creation boundary. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

## 项目依赖

- [Inno.Adapter.Storage](../../storage/Inno.Adapter.Storage.md)：公开引用边界由实际签名核对。
- [Inno.Storage](../../storage/Inno.Storage.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
