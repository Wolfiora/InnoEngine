# Inno.Adapter.Storage

[Storage 索引](README.md) · [中立 Storage](Inno.Storage.md) · [FileSystem implementation](../backends/FileSystem/Inno.Adapter.Storage.FileSystem.md)

该项目定义应用存储 Adapter family，不包含文件系统路径实现。

## 公开 API

- `StorageBackendId`：Composition 启动时使用的存储后端选择。
- `IStorageBackendFactory.CreateStorage`：从 backend 与逻辑 `StorageScope` 创建 caller-owned `IApplicationStorage`。物理位置由所选 provider 的宿主配置决定。

Factory 负责 implementation 选择；`IApplicationStorage` 继续负责 key sandbox、异步读取与原子提交。游戏脚本永远不接触绝对路径或 `FileSystemApplicationStorage`。

```csharp
using Inno.Adapter.Storage;
using Inno.Storage;

static IApplicationStorage CreateOwnedStorage(
    IStorageBackendFactory factory,
    StorageBackendId selection,
    StorageScope scope
) {
    return factory.CreateStorage(selection, scope);
}
```

未知 backend、无效 scope 或初始化失败必须在 Composition/Session 启动边界明确报告。

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

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Adapter.Storage.IStorageBackendFactory`

<!-- api: T:Inno.Adapter.Storage.IStorageBackendFactory ca140a914a3f1bf4ea2f48eaa1480b6202fb8b17e284c084cdbd1c76a7e0002e -->
<!-- api: P:Inno.Adapter.Storage.IStorageBackendFactory.supportedBackends ab5cdc7b8dd015226529e8ad6c81f1e1cdef66768eafce8cd8f2b47866fbcd67 -->
<!-- api: M:Inno.Adapter.Storage.IStorageBackendFactory.CreateStorage(Inno.Adapter.Storage.StorageBackendId,Inno.Storage.StorageScope) 7d6215b15b429e5baaf40bbbfd1ca52fde24a020b2763f67e6c9d2e6953ca7ed -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-07701109d1fc1542"></a>[`Inno.Storage.IApplicationStorage Inno.Adapter.Storage.IStorageBackendFactory.CreateStorage(Inno.Adapter.Storage.StorageBackendId backend, Inno.Storage.StorageScope scope)`](../../src/adapters/storage/Inno.Adapter.Storage/IStorageBackendFactory.cs#L31) | Creates isolated storage for a logical application namespace.<br>返回：A caller-owned backend-neutral application storage service.<br>`backend`：storage implementation selected by the composition root.<br>`scope`：The application namespace mapped to a location by the selected provider. | — | `System.NotSupportedException`：Thrown when the catalog does not contain the selected backend. |
| <a id="api-328e36f80e510b88"></a>[`System.Collections.Generic.IReadOnlyList<Inno.Adapter.Storage.StorageBackendId> Inno.Adapter.Storage.IStorageBackendFactory.supportedBackends { get; }`](../../src/adapters/storage/Inno.Adapter.Storage/IStorageBackendFactory.cs#L14) | Gets the exact registrations available in this composition snapshot. | — | — |
| <a id="api-a431ac47d391da82"></a>[`public interface Inno.Adapter.Storage.IStorageBackendFactory`](../../src/adapters/storage/Inno.Adapter.Storage/IStorageBackendFactory.cs#L9) | Creates isolated application-storage instances from explicit backend selections. | — | — |

### `Inno.Adapter.Storage.StorageBackendCatalog`

<!-- api: T:Inno.Adapter.Storage.StorageBackendCatalog 321664d193436a79dd05c20db3bd8e9a920ba69c1b0e97ef9ca97fb4144a81da -->
<!-- api: M:Inno.Adapter.Storage.StorageBackendCatalog.#ctor(System.Collections.Generic.IEnumerable{Inno.Adapter.Storage.StorageBackendProvider}) 85f1e7a52ff62bfd547a3e9102c3532a06e236917d6ce1077628437c4c9130df -->
<!-- api: P:Inno.Adapter.Storage.StorageBackendCatalog.supportedBackends 3d777b2682d7b257d7aae7a14b737fdbf83c2f115459581c77ba669646fe7574 -->
<!-- api: M:Inno.Adapter.Storage.StorageBackendCatalog.CreateStorage(Inno.Adapter.Storage.StorageBackendId,Inno.Storage.StorageScope) 952061aebb1b68b8ecfaea996bb7e43392045eba7d74d478b658487523370c60 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-64b96c32329a77b9"></a>[`public Inno.Adapter.Storage.StorageBackendCatalog.StorageBackendCatalog(System.Collections.Generic.IEnumerable<Inno.Adapter.Storage.StorageBackendProvider> providers)`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendCatalog.cs#L27) | Validates and captures a complete provider set without creating any service.<br>`providers`：Providers whose lifetime remains with the composition owner. | — | `System.ArgumentNullException`：The provider sequence is null.<br>`System.ArgumentException`：A provider is null, unassigned, or duplicates a registration identity. |
| <a id="api-dfa5c331828cee48"></a>[`public Inno.Storage.IApplicationStorage Inno.Adapter.Storage.StorageBackendCatalog.CreateStorage(Inno.Adapter.Storage.StorageBackendId backend, Inno.Storage.StorageScope scope)`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendCatalog.cs#L45) | Creates isolated storage for a logical application namespace.<br>返回：A caller-owned backend-neutral application storage service.<br>`backend`：storage implementation selected by the composition root.<br>`scope`：The application namespace mapped to a location by the selected provider. | — | `System.NotSupportedException`：Thrown when the catalog does not contain the selected backend.<br>`System.InvalidOperationException`：The selected provider returned no service. |
| <a id="api-2c32b04bf7b76df2"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Adapter.Storage.StorageBackendId> Inno.Adapter.Storage.StorageBackendCatalog.supportedBackends { get; }`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendCatalog.cs#L39) | Gets the exact registrations available in this composition snapshot. | — | — |
| <a id="api-95507b5eb761ae80"></a>[`public sealed class Inno.Adapter.Storage.StorageBackendCatalog : Inno.Adapter.Storage.IStorageBackendFactory`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendCatalog.cs#L11) | Resolves storage providers from one immutable, composition-owned registration snapshot. | — | — |

### `Inno.Adapter.Storage.StorageBackendId`

<!-- api: T:Inno.Adapter.Storage.StorageBackendId d699637c54de4debf7b12bce737696174b5dd24939a9cb825bc247660142f3e4 -->
<!-- api: M:Inno.Adapter.Storage.StorageBackendId.#ctor(System.String) 80006f7da26a8a263af21b82577367387bfae209591551d43efe349f37916169 -->
<!-- api: P:Inno.Adapter.Storage.StorageBackendId.fileSystem a4caff359204b48987df0ccce4e0a2ee085ce5e58ea9903ea715c33fea20a9ee -->
<!-- api: P:Inno.Adapter.Storage.StorageBackendId.browser 936b81ff688ecc649ee521a8d124fcee3a2a0dc16c8f2f840b0f880fb2ec0c55 -->
<!-- api: P:Inno.Adapter.Storage.StorageBackendId.value e6f379402eab9dbea446402d2edf4eaf190601787972d7a38115117e10fabdf9 -->
<!-- api: P:Inno.Adapter.Storage.StorageBackendId.isValid 2c8aeced0abd89dd62f0ce2dbe04ba54db0ad9d52b5fa592ea9f4ff95388f2f2 -->
<!-- api: M:Inno.Adapter.Storage.StorageBackendId.ToString f148fb54bdd88392cd4831759e75c99cd18038b9f215d07d719dd20d71c48a6d -->
<!-- api: M:Inno.Adapter.Storage.StorageBackendId.op_Inequality(Inno.Adapter.Storage.StorageBackendId,Inno.Adapter.Storage.StorageBackendId) fcecd73e651cdf0d86c9ce636798dcaf68c75bff73a0fa1b3b9c19ff38f72260 -->
<!-- api: M:Inno.Adapter.Storage.StorageBackendId.op_Equality(Inno.Adapter.Storage.StorageBackendId,Inno.Adapter.Storage.StorageBackendId) 49367758b6ae79e03ebd19968c35272a038896fdb9e4396d6d010b1c2ae585a5 -->
<!-- api: M:Inno.Adapter.Storage.StorageBackendId.GetHashCode a6c9ccb5b536b68079562683a09f43efedddfe9c480660c928fe64acc6862bb9 -->
<!-- api: M:Inno.Adapter.Storage.StorageBackendId.Equals(System.Object) c092f8040ffb458efd75836e925f7a1d0bdec97078a64f04e21969fc9527ac62 -->
<!-- api: M:Inno.Adapter.Storage.StorageBackendId.Equals(Inno.Adapter.Storage.StorageBackendId) 3b22cf11c3c380b6c850bbf8cb7b04d176636d604847f150ed4e2bafd8028974 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-278aa70e31f10670"></a>[`public Inno.Adapter.Storage.StorageBackendId.StorageBackendId(string value)`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendId.cs#L19) | Creates an ordinal, case-sensitive implementation identifier.<br>`value`：A stable nonempty identifier containing no whitespace. | — | `System.ArgumentException`：The identifier is empty or contains whitespace. |
| <a id="api-3265ff01094b65c6"></a>[`public bool Inno.Adapter.Storage.StorageBackendId.Equals(Inno.Adapter.Storage.StorageBackendId other)`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendId.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-019b69c1e43459bb"></a>[`public override bool Inno.Adapter.Storage.StorageBackendId.Equals(object obj)`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendId.cs#L8) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-5264725266744844"></a>[`public override int Inno.Adapter.Storage.StorageBackendId.GetHashCode()`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendId.cs#L8) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-8260b3cc5a22b9c3"></a>[`public static bool Inno.Adapter.Storage.StorageBackendId.operator ==(Inno.Adapter.Storage.StorageBackendId left, Inno.Adapter.Storage.StorageBackendId right)`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendId.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-3a955feca5e2da2e"></a>[`public static bool Inno.Adapter.Storage.StorageBackendId.operator !=(Inno.Adapter.Storage.StorageBackendId left, Inno.Adapter.Storage.StorageBackendId right)`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendId.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-f0b7879efabc5f68"></a>[`public override string Inno.Adapter.Storage.StorageBackendId.ToString()`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendId.cs#L54) | Returns the identifier without resolving a provider.<br>返回：The stable identifier, or an empty string when unassigned. | — | — |
| <a id="api-5946466ad8323b4e"></a>[`public static Inno.Adapter.Storage.StorageBackendId Inno.Adapter.Storage.StorageBackendId.browser { get; }`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendId.cs#L36) | Gets the identifier of the bundled browser implementation. | — | — |
| <a id="api-8bc75c0b0deabf17"></a>[`public static Inno.Adapter.Storage.StorageBackendId Inno.Adapter.Storage.StorageBackendId.fileSystem { get; }`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendId.cs#L31) | Gets the identifier of the bundled fileSystem implementation. | — | — |
| <a id="api-dee8d11fe7bcf816"></a>[`public bool Inno.Adapter.Storage.StorageBackendId.isValid { get; }`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendId.cs#L46) | Gets whether this value identifies an implementation. | — | — |
| <a id="api-e094e2ab8b1b553e"></a>[`public string Inno.Adapter.Storage.StorageBackendId.value { get; }`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendId.cs#L41) | Gets the stable identifier; a default value is unassigned. | — | — |
| <a id="api-b084ac3fc470954c"></a>[`public readonly record struct Inno.Adapter.Storage.StorageBackendId : System.IEquatable<Inno.Adapter.Storage.StorageBackendId>`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendId.cs#L8) | Identifies a storage implementation without closing the set of supported backends. | — | — |

### `Inno.Adapter.Storage.StorageBackendProvider`

<!-- api: T:Inno.Adapter.Storage.StorageBackendProvider e07ca4f204bbc1ddeb2a742dcacf2c60da9595a27b7fd46d8e987967acc5b99f -->
<!-- api: M:Inno.Adapter.Storage.StorageBackendProvider.#ctor(Inno.Adapter.Storage.StorageBackendId) 3f26baac666a501a184403384ddac47a3d673e97f79fd5d5ed68f27f5f070251 -->
<!-- api: P:Inno.Adapter.Storage.StorageBackendProvider.id c6b7e330cc9b61a8c4ef4782c986fadcf5de6f5621fd2309496c1d8d68c1e677 -->
<!-- api: M:Inno.Adapter.Storage.StorageBackendProvider.CreateStorage(Inno.Storage.StorageScope) 3bbce42782fac60982757252546fe69de57d5318619a65600d538df5ef6e888e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f7540284ac15e126"></a>[`protected Inno.Adapter.Storage.StorageBackendProvider.StorageBackendProvider(Inno.Adapter.Storage.StorageBackendId id)`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendProvider.cs#L24) | Captures the identity assigned by the composition owner.<br>`id`：The assigned implementation identity. | — | `System.ArgumentException`：The identity is unassigned. |
| <a id="api-cfcb7c8c2950e821"></a>[`public abstract Inno.Storage.IApplicationStorage Inno.Adapter.Storage.StorageBackendProvider.CreateStorage(Inno.Storage.StorageScope scope)`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendProvider.cs#L45) | Creates a caller-owned storage service using this implementation.<br>返回：A new service owned by the caller; providers must not return null.<br>`scope`：The application namespace to isolate within this provider's host configuration. | — | — |
| <a id="api-a02749644d9029d0"></a>[`public Inno.Adapter.Storage.StorageBackendId Inno.Adapter.Storage.StorageBackendProvider.id { get; }`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendProvider.cs#L34) | Gets this registration's immutable implementation identity. | — | — |
| <a id="api-3e873e1411576f97"></a>[`public abstract class Inno.Adapter.Storage.StorageBackendProvider`](../../src/adapters/storage/Inno.Adapter.Storage/StorageBackendProvider.cs#L13) | Describes one explicitly composed storage implementation and its creation boundary. | The composition owns this provider and its immutable registration identity. Products are owned by their callers. This registration does not perform type discovery. | — |

## 项目依赖

- [Inno.Storage](Inno.Storage.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
