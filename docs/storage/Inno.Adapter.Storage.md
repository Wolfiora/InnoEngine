# Inno.Adapter.Storage

[Storage 索引](README.md) · [中立 Storage](Inno.Storage.md) · [FileSystem implementation](../backends/FileSystem/Inno.Adapter.Storage.FileSystem.md)

该项目定义应用存储 Adapter family，不包含文件系统路径实现。

## 公开 API

- `StorageBackendId`：Composition 启动时使用的存储后端选择。
- `IStorageBackendFactory.CreateStorage`：从 backend 与宿主批准的 root 创建 caller-owned `IApplicationStorage`。

Factory 负责 implementation 选择；`IApplicationStorage` 继续负责 key sandbox、异步读取与原子提交。游戏脚本永远不接触绝对路径或 `FileSystemApplicationStorage`。

```csharp
using IApplicationStorage storage = catalog.storage.CreateStorage(selection.storage, persistentRoot);
```

未知 backend、无效 root 或初始化失败必须在 Composition/Session 启动边界明确报告。

## 开放注册与生命周期

后端 ID 是开放的 ordinal 字符串，不能包含空白；默认 struct 未赋值。内置 ID `fileSystem / browser` 只提供默认组合，不限制第三方实现。

| API | 当前语义 |
| --- | --- |
| `StorageBackendId(string)`；`value`、`isValid`、`ToString()` | 创建、检查并显示稳定 ID；无效构造抛出 `ArgumentException`。 |
| `StorageBackendProvider(StorageBackendId)`（protected）；`id` | composition 显式配置的不可变注册描述；provider 不执行类型发现。 |
| `StorageBackendProvider.CreateStorage` | 实现者的创建扩展点；返回 caller-owned `IApplicationStorage`，不允许 null。 |
| `StorageBackendCatalog(IEnumerable<StorageBackendProvider>)` | 捕获完整注册快照；重复或 null provider 在构造时失败；不创建设备。 |
| `StorageBackendCatalog.supportedBackends / CreateStorage` | 只解析当前快照中的 exact ID；未注册抛出 `NotSupportedException`，null 产品抛出 `InvalidOperationException`。 |
| `IStorageBackendFactory.supportedBackends` | 启动前能力预检使用的只读注册列表。 |

provider 及其 delegate/资源由 composition owner 释放；catalog 不接管 provider。创建出的服务由调用方释放。源码扩展若通过 TypeRegistry 发现，其 ID 仍由发现协议的 Attribute 声明；此处是宿主明确传入的 provider 集合，不额外扫描程序集。

`AdapterSelection.Validate(catalog)` 在初始化任何窗口或设备前检查全部领域。可在 composition 为 provider 传入任意分配的 `StorageBackendId`，无需新增枚举或修改中央分支。






## 本轮边界与所有权

provider/catalog 接收 StorageScope。逻辑 namespace 与物理布局分开；composition 明确提供 factory。未注册 backend 或缺少能力明确失败。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Adapter.Storage.IStorageBackendFactory`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Storage.IStorageBackendFactory`](../../src/adapters/storage/Inno.Adapter.Storage/IStorageBackendFactory.cs#L9) | Creates isolated application-storage instances from explicit backend selections. |
| [`Inno.Storage.IApplicationStorage Inno.Adapter.Storage.IStorageBackendFactory.CreateStorage(Inno.Adapter.Storage.StorageBackendId backend, Inno.Storage.StorageScope scope)`](../../src/adapters/storage/Inno.Adapter.Storage/IStorageBackendFactory.cs#L31) | Creates isolated storage for a logical application namespace. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Adapter.Storage.StorageBackendId> Inno.Adapter.Storage.IStorageBackendFactory.supportedBackends`](../../src/adapters/storage/Inno.Adapter.Storage/IStorageBackendFactory.cs#L14) | Gets the exact registrations available in this composition snapshot. |

### `Inno.Adapter.Storage.StorageBackendCatalog`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Storage.StorageBackendCatalog`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendCatalog.cs#L11) | Resolves storage providers from one immutable, composition-owned registration snapshot. |
| [`Inno.Adapter.Storage.StorageBackendCatalog.StorageBackendCatalog(System.Collections.Generic.IEnumerable<Inno.Adapter.Storage.StorageBackendProvider> providers)`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendCatalog.cs#L27) | Validates and captures a complete provider set without creating any service. |
| [`Inno.Storage.IApplicationStorage Inno.Adapter.Storage.StorageBackendCatalog.CreateStorage(Inno.Adapter.Storage.StorageBackendId backend, Inno.Storage.StorageScope scope)`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendCatalog.cs#L45) | See the implemented contract. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Adapter.Storage.StorageBackendId> Inno.Adapter.Storage.StorageBackendCatalog.supportedBackends`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendCatalog.cs#L39) | See the implemented contract. |

### `Inno.Adapter.Storage.StorageBackendId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Storage.StorageBackendId`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendId.cs#L8) | Identifies a storage implementation without closing the set of supported backends. |
| [`Inno.Adapter.Storage.StorageBackendId.StorageBackendId(string value)`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendId.cs#L19) | Creates an ordinal, case-sensitive implementation identifier. |
| [`bool Inno.Adapter.Storage.StorageBackendId.isValid`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendId.cs#L46) | Gets whether this value identifies an implementation. |
| [`override string Inno.Adapter.Storage.StorageBackendId.ToString()`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendId.cs#L54) | Returns the identifier without resolving a provider. |
| [`static Inno.Adapter.Storage.StorageBackendId Inno.Adapter.Storage.StorageBackendId.browser`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendId.cs#L36) | Gets the identifier of the bundled browser implementation. |
| [`static Inno.Adapter.Storage.StorageBackendId Inno.Adapter.Storage.StorageBackendId.fileSystem`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendId.cs#L31) | Gets the identifier of the bundled fileSystem implementation. |
| [`string Inno.Adapter.Storage.StorageBackendId.value`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendId.cs#L41) | Gets the stable identifier; a default value is unassigned. |

### `Inno.Adapter.Storage.StorageBackendProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Storage.StorageBackendId Inno.Adapter.Storage.StorageBackendProvider.id`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendProvider.cs#L34) | Gets this registration's immutable implementation identity. |
| [`Inno.Adapter.Storage.StorageBackendProvider`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendProvider.cs#L13) | Describes one explicitly composed storage implementation and its creation boundary. |
| [`Inno.Adapter.Storage.StorageBackendProvider.StorageBackendProvider(Inno.Adapter.Storage.StorageBackendId id)`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendProvider.cs#L24) | Captures the identity assigned by the composition owner. |
| [`abstract Inno.Storage.IApplicationStorage Inno.Adapter.Storage.StorageBackendProvider.CreateStorage(Inno.Storage.StorageScope scope)`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendProvider.cs#L45) | Creates a caller-owned storage service using this implementation. |

## 项目依赖

- [Inno.Storage](Inno.Storage.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
