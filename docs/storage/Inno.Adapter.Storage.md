# Inno.Adapter.Storage

[Storage 索引](README.md) · [中立 Storage](Inno.Storage.md) · [FileSystem implementation](Inno.Adapter.Storage.FileSystem.md)

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
