# Inno.Adapter.Text

[Text 索引](README.md) · [Runtime adapter catalog](../runtime/Inno.Adapter.md)

`ITextBackendFactory` 与 `TextBackendId` 是 Composition 使用的中立选择协议。Host 依 `AdapterSelection` 选实现并为 Session 创建后端；游戏脚本只见 `Inno.Text` 契约。新增排版后端不改变 Text API。

当前 `TextBackendId.freeTypeHarfBuzz` 由默认 catalog 提供。工厂的 `CreateBackend` 每次返回调用方独占的 `ITextBackend`；`LoadFont` 返回 generation-local `TextFontHandle`，`ReleaseFont`/`Dispose` 逆序清理。`Shape` 和 `Rasterize` 不能接受上一后端代际的 handle。实现者不得把原生指针放入持久资产或脚本导出。

## 公开边界

| API | 用途 |
| --- | --- |
| `TextBackendId` | 选择内置 FreeType/HarfBuzz 实现；不是脚本导出。 |
| `ITextBackendFactory.CreateBackend` | Composition 在创建 Session 时取得独占 backend。 |

扩展后端仍须实现 [Text Service](Inno.Text.md) 的 `ITextBackend`，返回不可持久化的 face handle。Host 应在 Session 停止时调用 `Dispose`；原生依赖不向上层传播。无效 face 和后端故障必须在调用点报告。

## 开放注册与生命周期

后端 ID 是开放的 ordinal 字符串，不能包含空白；默认 struct 未赋值。内置 ID `freeTypeHarfBuzz` 只提供默认组合，不限制第三方实现。

| API | 当前语义 |
| --- | --- |
| `TextBackendId(string)`；`value`、`isValid`、`ToString()` | 创建、检查并显示稳定 ID；无效构造抛出 `ArgumentException`。 |
| `TextBackendProvider(TextBackendId)`（protected）；`id` | composition 显式配置的不可变注册描述；provider 不执行类型发现。 |
| `TextBackendProvider.CreateBackend` | 实现者的创建扩展点；返回 caller-owned `ITextBackend`，不允许 null。 |
| `TextBackendCatalog(IEnumerable<TextBackendProvider>)` | 捕获完整注册快照；重复或 null provider 在构造时失败；不创建设备。 |
| `TextBackendCatalog.supportedBackends / CreateBackend` | 只解析当前快照中的 exact ID；未注册抛出 `NotSupportedException`，null 产品抛出 `InvalidOperationException`。 |
| `ITextBackendFactory.supportedBackends` | 启动前能力预检使用的只读注册列表。 |

provider 及其 delegate/资源由 composition owner 释放；catalog 不接管 provider。创建出的服务由调用方释放。源码扩展若通过 TypeRegistry 发现，其 ID 仍由发现协议的 Attribute 声明；此处是宿主明确传入的 provider 集合，不额外扫描程序集。

`AdapterSelection.Validate(catalog)` 在初始化任何窗口或设备前检查全部领域。可在 composition 为 provider 传入任意分配的 `TextBackendId`，无需新增枚举或修改中央分支。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Adapter.Text.ITextBackendFactory`

<!-- api: T:Inno.Adapter.Text.ITextBackendFactory d42ee39d70ced51afa026bcfaa3fa6a558c0dfe66f20ff09b68135dbb3536f28 -->
<!-- api: P:Inno.Adapter.Text.ITextBackendFactory.supportedBackends 6bec23079fe48b18ee40ea9b5af2eff7e1f389ac3bcb07b14f3a57c64b320cf9 -->
<!-- api: M:Inno.Adapter.Text.ITextBackendFactory.CreateBackend(Inno.Adapter.Text.TextBackendId) 631e689271a22b1047cb23c270be8b06afc263a420a7388ba59030bf751bfa78 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8cd78b94d402fb4f"></a>[`Inno.Text.ITextBackend Inno.Adapter.Text.ITextBackendFactory.CreateBackend(Inno.Adapter.Text.TextBackendId backend)`](../../src/adapters/text/Inno.Adapter.Text/ITextBackendFactory.cs#L25) | Creates one caller-owned text backend.<br>返回：A newly allocated text backend.<br>`backend`：The selected backend implementation. | — | — |
| <a id="api-8739f77f77b4b957"></a>[`System.Collections.Generic.IReadOnlyList<Inno.Adapter.Text.TextBackendId> Inno.Adapter.Text.ITextBackendFactory.supportedBackends { get; }`](../../src/adapters/text/Inno.Adapter.Text/ITextBackendFactory.cs#L14) | Gets the exact registrations available in this composition snapshot. | — | — |
| <a id="api-c7b234b16fdc54e7"></a>[`public interface Inno.Adapter.Text.ITextBackendFactory`](../../src/adapters/text/Inno.Adapter.Text/ITextBackendFactory.cs#L9) | Creates isolated text backends without exposing implementation assemblies to composition code. | — | — |

### `Inno.Adapter.Text.TextBackendCatalog`

<!-- api: T:Inno.Adapter.Text.TextBackendCatalog 75a43e2825b11606af8bd49d92c72fccfa4aeb5213a1010efe45475592864248 -->
<!-- api: M:Inno.Adapter.Text.TextBackendCatalog.#ctor(System.Collections.Generic.IEnumerable{Inno.Adapter.Text.TextBackendProvider}) 104d55c4b8cc9247555aa03045583d413db2a789d72b895f8a1b66b887302a3c -->
<!-- api: P:Inno.Adapter.Text.TextBackendCatalog.supportedBackends 04d6c6e5be34a2df94f2aabec4999f6d9094bb504c84ea229df74c3c4db180b3 -->
<!-- api: M:Inno.Adapter.Text.TextBackendCatalog.CreateBackend(Inno.Adapter.Text.TextBackendId) 40e43f5ca80b68dfc68ab984454b54abff6d2a40c6f0e2465821ac1361db379d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-04623a0626133d72"></a>[`public Inno.Adapter.Text.TextBackendCatalog.TextBackendCatalog(System.Collections.Generic.IEnumerable<Inno.Adapter.Text.TextBackendProvider> providers)`](../../src/adapters/text/Inno.Adapter.Text/TextBackendCatalog.cs#L27) | Validates and captures a complete provider set without creating any service.<br>`providers`：Providers whose lifetime remains with the composition owner. | — | `System.ArgumentNullException`：The provider sequence is null.<br>`System.ArgumentException`：A provider is null, unassigned, or duplicates a registration identity. |
| <a id="api-57961aac1c9992dd"></a>[`public Inno.Text.ITextBackend Inno.Adapter.Text.TextBackendCatalog.CreateBackend(Inno.Adapter.Text.TextBackendId backend)`](../../src/adapters/text/Inno.Adapter.Text/TextBackendCatalog.cs#L45) | Creates one caller-owned text backend.<br>返回：A newly allocated text backend.<br>`backend`：The selected backend implementation. | — | `System.InvalidOperationException`：The selected provider returned no service. |
| <a id="api-26a9b4a523c29b3d"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Adapter.Text.TextBackendId> Inno.Adapter.Text.TextBackendCatalog.supportedBackends { get; }`](../../src/adapters/text/Inno.Adapter.Text/TextBackendCatalog.cs#L39) | Gets the exact registrations available in this composition snapshot. | — | — |
| <a id="api-9eb482e8087f6882"></a>[`public sealed class Inno.Adapter.Text.TextBackendCatalog : Inno.Adapter.Text.ITextBackendFactory`](../../src/adapters/text/Inno.Adapter.Text/TextBackendCatalog.cs#L11) | Resolves text providers from one immutable, composition-owned registration snapshot. | — | — |

### `Inno.Adapter.Text.TextBackendId`

<!-- api: T:Inno.Adapter.Text.TextBackendId 77359386f171eced6427d13bf77441bed38acb064d9354fccaf06960bbe927d5 -->
<!-- api: M:Inno.Adapter.Text.TextBackendId.#ctor(System.String) 8402f4d5bca28cdf953796adbc979d7fa11436d4ac1d5c4c48641e4ec60289b4 -->
<!-- api: P:Inno.Adapter.Text.TextBackendId.freeTypeHarfBuzz c5a8905408a4b70003e99c7833d4a294512c9f62443cf22002ebc1b313dcb73d -->
<!-- api: P:Inno.Adapter.Text.TextBackendId.value 2d934c7ee469d7112849628610b742190bd46b10a381e1c16644abd26549d2d8 -->
<!-- api: P:Inno.Adapter.Text.TextBackendId.isValid 4c2146e72329b7d46bfca546795a424c2023263849feee785b8793a17f42c816 -->
<!-- api: M:Inno.Adapter.Text.TextBackendId.ToString d97bd5dcd599b6768aaef97a4bb6978890b6211a90eca6c6b1ec2f248af15390 -->
<!-- api: M:Inno.Adapter.Text.TextBackendId.op_Inequality(Inno.Adapter.Text.TextBackendId,Inno.Adapter.Text.TextBackendId) 5047999931f079693a42afad7210ab835cd440320f25e719c97b4c1497e3003d -->
<!-- api: M:Inno.Adapter.Text.TextBackendId.op_Equality(Inno.Adapter.Text.TextBackendId,Inno.Adapter.Text.TextBackendId) 845ad3ac4855b3db07a7e6dfdc7b2ecfcfba57b8f5cf3bfb2759b7ecd3676c04 -->
<!-- api: M:Inno.Adapter.Text.TextBackendId.GetHashCode 833a2c8c5f33f68a3f2370aa410cc2d4e015f70883eac8f4dc31ba7d0b850ab8 -->
<!-- api: M:Inno.Adapter.Text.TextBackendId.Equals(System.Object) 4750617f2765cd4ff0fc8bda88185c71cda453506bf7d3d08c31fba338edb51c -->
<!-- api: M:Inno.Adapter.Text.TextBackendId.Equals(Inno.Adapter.Text.TextBackendId) de96d087fc9c880e3b1783903e2464712096aa4e4ee923081fec1d9ac5d8db5c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2313931419caad7c"></a>[`public Inno.Adapter.Text.TextBackendId.TextBackendId(string value)`](../../src/adapters/text/Inno.Adapter.Text/TextBackendId.cs#L19) | Creates an ordinal, case-sensitive implementation identifier.<br>`value`：A stable nonempty identifier containing no whitespace. | — | `System.ArgumentException`：The identifier is empty or contains whitespace. |
| <a id="api-81b7f2f9ce154c8e"></a>[`public bool Inno.Adapter.Text.TextBackendId.Equals(Inno.Adapter.Text.TextBackendId other)`](../../src/adapters/text/Inno.Adapter.Text/TextBackendId.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-e32a9de201403881"></a>[`public override bool Inno.Adapter.Text.TextBackendId.Equals(object obj)`](../../src/adapters/text/Inno.Adapter.Text/TextBackendId.cs#L8) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-2ccb59f2df66be4c"></a>[`public override int Inno.Adapter.Text.TextBackendId.GetHashCode()`](../../src/adapters/text/Inno.Adapter.Text/TextBackendId.cs#L8) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-2dda101471489fe8"></a>[`public static bool Inno.Adapter.Text.TextBackendId.operator ==(Inno.Adapter.Text.TextBackendId left, Inno.Adapter.Text.TextBackendId right)`](../../src/adapters/text/Inno.Adapter.Text/TextBackendId.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-91de8e4d3eff09cb"></a>[`public static bool Inno.Adapter.Text.TextBackendId.operator !=(Inno.Adapter.Text.TextBackendId left, Inno.Adapter.Text.TextBackendId right)`](../../src/adapters/text/Inno.Adapter.Text/TextBackendId.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-7c84442397142ed8"></a>[`public override string Inno.Adapter.Text.TextBackendId.ToString()`](../../src/adapters/text/Inno.Adapter.Text/TextBackendId.cs#L49) | Returns the identifier without resolving a provider.<br>返回：The stable identifier, or an empty string when unassigned. | — | — |
| <a id="api-784d5b6b30bd12a9"></a>[`public static Inno.Adapter.Text.TextBackendId Inno.Adapter.Text.TextBackendId.freeTypeHarfBuzz { get; }`](../../src/adapters/text/Inno.Adapter.Text/TextBackendId.cs#L31) | Gets the identifier of the bundled freeTypeHarfBuzz implementation. | — | — |
| <a id="api-4aab35e45c55daae"></a>[`public bool Inno.Adapter.Text.TextBackendId.isValid { get; }`](../../src/adapters/text/Inno.Adapter.Text/TextBackendId.cs#L41) | Gets whether this value identifies an implementation. | — | — |
| <a id="api-beed4526a4d2b89b"></a>[`public string Inno.Adapter.Text.TextBackendId.value { get; }`](../../src/adapters/text/Inno.Adapter.Text/TextBackendId.cs#L36) | Gets the stable identifier; a default value is unassigned. | — | — |
| <a id="api-5048bb92223daacc"></a>[`public readonly record struct Inno.Adapter.Text.TextBackendId : System.IEquatable<Inno.Adapter.Text.TextBackendId>`](../../src/adapters/text/Inno.Adapter.Text/TextBackendId.cs#L8) | Identifies a text implementation without closing the set of supported backends. | — | — |

### `Inno.Adapter.Text.TextBackendProvider`

<!-- api: T:Inno.Adapter.Text.TextBackendProvider 207d11feb79253075f0b53829e9ad35f7b9ad8cae4c2094c67c18d6c0dbfe273 -->
<!-- api: M:Inno.Adapter.Text.TextBackendProvider.#ctor(Inno.Adapter.Text.TextBackendId) 2a7c4795c11833525d5af6d121159d0e919e56ec27a509ba41de6ac9bbe635fc -->
<!-- api: P:Inno.Adapter.Text.TextBackendProvider.id 4c2a2e2e3d19f4b5cbc23a3217b50ef85e5fbd5391d5b629ca2db595a61989e8 -->
<!-- api: M:Inno.Adapter.Text.TextBackendProvider.CreateBackend 45903d23b7ae31e6ca761f112ea2de2916c83658d590d0c784cf174550d60205 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-44ba61f6876a0689"></a>[`protected Inno.Adapter.Text.TextBackendProvider.TextBackendProvider(Inno.Adapter.Text.TextBackendId id)`](../../src/adapters/text/Inno.Adapter.Text/TextBackendProvider.cs#L24) | Captures the identity assigned by the composition owner.<br>`id`：The assigned implementation identity. | — | `System.ArgumentException`：The identity is unassigned. |
| <a id="api-fc85ade3d61e04c1"></a>[`public abstract Inno.Text.ITextBackend Inno.Adapter.Text.TextBackendProvider.CreateBackend()`](../../src/adapters/text/Inno.Adapter.Text/TextBackendProvider.cs#L42) | Creates a caller-owned text service using this implementation.<br>返回：A new service owned by the caller; providers must not return null. | — | — |
| <a id="api-3c0f825e380dbe03"></a>[`public Inno.Adapter.Text.TextBackendId Inno.Adapter.Text.TextBackendProvider.id { get; }`](../../src/adapters/text/Inno.Adapter.Text/TextBackendProvider.cs#L34) | Gets this registration's immutable implementation identity. | — | — |
| <a id="api-12af744bfa08f922"></a>[`public abstract class Inno.Adapter.Text.TextBackendProvider`](../../src/adapters/text/Inno.Adapter.Text/TextBackendProvider.cs#L13) | Describes one explicitly composed text implementation and its creation boundary. | The composition owns this provider and its immutable registration identity. Products are owned by their callers. This registration does not perform type discovery. | — |

## 项目依赖

- [Inno.Text](Inno.Text.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
