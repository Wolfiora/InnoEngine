# Inno.Adapter.UI

[UI 索引](README.md) · [Runtime adapter catalog](../runtime/Inno.Adapter.md)

`IUiBackendFactory`、`UiBackendId`、`UiBackendProvider` 与 `UiBackendCatalog` 是 Composition 层的开放式后端选择协议。默认 catalog 注册 RmlUi provider；第三方可以注册自己的稳定 ID/provider，而不修改引擎枚举，也不要求游戏脚本引用原生库。

`UiBackendId.rmlUi` 只是内置 provider 的 ID，不是白名单。工厂的 `CreateBackend` 每次返回 Session 独占 `IUiBackend`；backend 必须声明 `implementationId` 与 `UiBackendCapabilities`，并在 `DestroyContext`/`Dispose` 完成所属资源退休。

## 公开边界

| API | 用途 |
| --- | --- |
| `UiBackendId`、`UiBackendProvider`、`UiBackendCatalog` | 开放式实现注册与不可变 composition snapshot；不是脚本导出。 |
| `IUiBackendFactory.CreateBackend` | Composition 在创建 Session 时取得独占 backend。 |

扩展实现只需符合 [IUiService](Inno.UI.md) 下方的后端中立帧与事件协议。它不可将 native context、RmlUi 元素或 GPU handle 写入资产或 Scene。Host 在 Session 停止时关闭 backend；后端创建失败应阻止 Session 启动，而非回退到空 UI。

## 注册身份

`UiBackendProvider` 的 protected 构造函数接收对应领域的 backend ID，并公开只读 `id`。ID 由 composition 分配，不能 override 或从临时创建的设备推导；构造拒绝未赋值 ID。此 provider 是显式 composition 注册，不进行类型发现。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Adapter.UI.IUiBackendFactory`

<!-- api: T:Inno.Adapter.UI.IUiBackendFactory e88c4a503e86c68dcb52276af830d57bb5afde6e7d9e5b6754cf599c719d197c -->
<!-- api: P:Inno.Adapter.UI.IUiBackendFactory.supportedBackends b54d64c12769185fae3d2e449f2ef013a7193c391ade86f087c732891268dd11 -->
<!-- api: M:Inno.Adapter.UI.IUiBackendFactory.CreateBackend(Inno.Adapter.UI.UiBackendId) d18aa6c512041f14f1f3151cc49bd8889fe673744b0a3aea598821506c2a0db3 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-5f5acb29571ecff0"></a>[`Inno.UI.IUiBackend Inno.Adapter.UI.IUiBackendFactory.CreateBackend(Inno.Adapter.UI.UiBackendId backend)`](../../src/adapters/ui/Inno.Adapter.UI/IUiBackendFactory.cs#L25) | Creates one caller-owned UI backend.<br>返回：A newly allocated UI backend.<br>`backend`：The selected backend implementation. | — | — |
| <a id="api-eb33947380dc5a1f"></a>[`System.Collections.Generic.IReadOnlyList<Inno.Adapter.UI.UiBackendId> Inno.Adapter.UI.IUiBackendFactory.supportedBackends { get; }`](../../src/adapters/ui/Inno.Adapter.UI/IUiBackendFactory.cs#L14) | Gets exact backend identities available in this composition generation. | — | — |
| <a id="api-8e24396f5941ff74"></a>[`public interface Inno.Adapter.UI.IUiBackendFactory`](../../src/adapters/ui/Inno.Adapter.UI/IUiBackendFactory.cs#L9) | Creates isolated UI backends without exposing implementation assemblies to composition code. | — | — |

### `Inno.Adapter.UI.UiBackendCatalog`

<!-- api: T:Inno.Adapter.UI.UiBackendCatalog bbd80939a1019e71ff23c0b35f58826431ebd68c298ce3d6a50f745bd66a079e -->
<!-- api: M:Inno.Adapter.UI.UiBackendCatalog.#ctor(System.Collections.Generic.IEnumerable{Inno.Adapter.UI.UiBackendProvider}) 48244041d24ab377db6998df9aeccd1f60efb44a1d877942338d8bd67003e9fb -->
<!-- api: P:Inno.Adapter.UI.UiBackendCatalog.supportedBackends af9f85695eafc1e04a42a371112b7c89a5c4aa6bad06cabcbed7ec4bfb1836bd -->
<!-- api: M:Inno.Adapter.UI.UiBackendCatalog.CreateBackend(Inno.Adapter.UI.UiBackendId) 7df27e821aefa2b0d875137c9baf94ba550248d11fe99b87de11e7a231413e1e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1257173312ad73e0"></a>[`public Inno.Adapter.UI.UiBackendCatalog.UiBackendCatalog(System.Collections.Generic.IEnumerable<Inno.Adapter.UI.UiBackendProvider> providers)`](../../src/adapters/ui/Inno.Adapter.UI/UiBackendCatalog.cs#L22) | Validates and captures a complete provider set.<br>`providers`：Providers owned by this composition generation. | — | — |
| <a id="api-a99522c2e1a66596"></a>[`public Inno.UI.IUiBackend Inno.Adapter.UI.UiBackendCatalog.CreateBackend(Inno.Adapter.UI.UiBackendId backend)`](../../src/adapters/ui/Inno.Adapter.UI/UiBackendCatalog.cs#L45) | Creates a backend using this implementation's validated inputs.<br>返回：The validated iui backend that represents the completed operation.<br>`backend`：The backend consumed by create backend; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-8069ed0c49790e3f"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Adapter.UI.UiBackendId> Inno.Adapter.UI.UiBackendCatalog.supportedBackends { get; }`](../../src/adapters/ui/Inno.Adapter.UI/UiBackendCatalog.cs#L34) | Gets backend registrations available in this type generation. | — | — |
| <a id="api-3def526de2033208"></a>[`public sealed class Inno.Adapter.UI.UiBackendCatalog : Inno.Adapter.UI.IUiBackendFactory`](../../src/adapters/ui/Inno.Adapter.UI/UiBackendCatalog.cs#L12) | Resolves UI providers from one immutable composition-owned registration snapshot. | — | — |

### `Inno.Adapter.UI.UiBackendId`

<!-- api: T:Inno.Adapter.UI.UiBackendId d482c18d3a31c94b74ce76a662c30db42a491066b7ec5b7d30eaf4b30f76632f -->
<!-- api: M:Inno.Adapter.UI.UiBackendId.#ctor(System.String) 107d5865cd95ee3f4f10ea75a6bb6c57bdeb35545bae79042aa4628e4be262a6 -->
<!-- api: P:Inno.Adapter.UI.UiBackendId.rmlUi ef3f0df1a05d32ccb5974123297d4f8605205ee0b57d0691e95521e0e2bcfa3d -->
<!-- api: P:Inno.Adapter.UI.UiBackendId.value bff0c0cfe6c2849e88695d26e983391a17727f92880a7a95c699ec9534856f75 -->
<!-- api: P:Inno.Adapter.UI.UiBackendId.isValid ea03d49ce70aeebd0acbdbb79c86e9d037494793c4a4b3c7890ebe7a259aaf7b -->
<!-- api: M:Inno.Adapter.UI.UiBackendId.ToString c6783aec45532a8b6dfa7b65e928defd27d85376b78e52e76f07be01803f56fe -->
<!-- api: M:Inno.Adapter.UI.UiBackendId.op_Inequality(Inno.Adapter.UI.UiBackendId,Inno.Adapter.UI.UiBackendId) e5b95da9b25af7f3604eac320929ae8457cac4cf4bd00e5455e09133a35e6165 -->
<!-- api: M:Inno.Adapter.UI.UiBackendId.op_Equality(Inno.Adapter.UI.UiBackendId,Inno.Adapter.UI.UiBackendId) 87c0c6926dc3166ea90ecaaac7522248c801923ef9182ee194f6d85652c0bc21 -->
<!-- api: M:Inno.Adapter.UI.UiBackendId.GetHashCode e2f253cd4289c407b94c0c3c3dfd381a005ced5259f7fe78b4d84d0e4c1a1211 -->
<!-- api: M:Inno.Adapter.UI.UiBackendId.Equals(System.Object) 3c61fa3563bda6e57a600209e286f5f3fa6707b580f84a687f4f6a68a85a68c4 -->
<!-- api: M:Inno.Adapter.UI.UiBackendId.Equals(Inno.Adapter.UI.UiBackendId) 78832f73e7256d7f3beb4f5c24709a8d3514250565cf789de740eb50570145ed -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a72f8a4ae8640093"></a>[`public Inno.Adapter.UI.UiBackendId.UiBackendId(string value)`](../../src/adapters/ui/Inno.Adapter.UI/UiBackendId.cs#L16) | Creates an ordinal, case-sensitive backend identifier.<br>`value`：Stable nonempty implementation identifier. | — | — |
| <a id="api-5de2251e106acad7"></a>[`public bool Inno.Adapter.UI.UiBackendId.Equals(Inno.Adapter.UI.UiBackendId other)`](../../src/adapters/ui/Inno.Adapter.UI/UiBackendId.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-ccad7402f105fc27"></a>[`public override bool Inno.Adapter.UI.UiBackendId.Equals(object obj)`](../../src/adapters/ui/Inno.Adapter.UI/UiBackendId.cs#L8) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-ce8e7257bf6cf58f"></a>[`public override int Inno.Adapter.UI.UiBackendId.GetHashCode()`](../../src/adapters/ui/Inno.Adapter.UI/UiBackendId.cs#L8) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-ec3b35df55f8cd23"></a>[`public static bool Inno.Adapter.UI.UiBackendId.operator ==(Inno.Adapter.UI.UiBackendId left, Inno.Adapter.UI.UiBackendId right)`](../../src/adapters/ui/Inno.Adapter.UI/UiBackendId.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-c602ee901cb5fdf0"></a>[`public static bool Inno.Adapter.UI.UiBackendId.operator !=(Inno.Adapter.UI.UiBackendId left, Inno.Adapter.UI.UiBackendId right)`](../../src/adapters/ui/Inno.Adapter.UI/UiBackendId.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-bfda0188ec834796"></a>[`public override string Inno.Adapter.UI.UiBackendId.ToString()`](../../src/adapters/ui/Inno.Adapter.UI/UiBackendId.cs#L46) | Returns the stable identifier.<br>返回：The identifier, or an empty string when unassigned. | — | — |
| <a id="api-bcf25dfb2272f201"></a>[`public bool Inno.Adapter.UI.UiBackendId.isValid { get; }`](../../src/adapters/ui/Inno.Adapter.UI/UiBackendId.cs#L38) | Gets whether this value identifies an implementation. | — | — |
| <a id="api-e39eb8f08a26eb25"></a>[`public static Inno.Adapter.UI.UiBackendId Inno.Adapter.UI.UiBackendId.rmlUi { get; }`](../../src/adapters/ui/Inno.Adapter.UI/UiBackendId.cs#L28) | Gets the identifier of the bundled RmlUi implementation, not a closed backend list. | — | — |
| <a id="api-95c3b2c80431ada6"></a>[`public string Inno.Adapter.UI.UiBackendId.value { get; }`](../../src/adapters/ui/Inno.Adapter.UI/UiBackendId.cs#L33) | Gets the stable identifier; the default value is unassigned. | — | — |
| <a id="api-7127f6a85ca110de"></a>[`public readonly record struct Inno.Adapter.UI.UiBackendId : System.IEquatable<Inno.Adapter.UI.UiBackendId>`](../../src/adapters/ui/Inno.Adapter.UI/UiBackendId.cs#L8) | Identifies a UI implementation independently from its document language. | — | — |

### `Inno.Adapter.UI.UiBackendProvider`

<!-- api: T:Inno.Adapter.UI.UiBackendProvider 1b2c3c0c0ef676833be92aa09de6d2bdac841e9fa190532bc49b78e3a4e4afa2 -->
<!-- api: M:Inno.Adapter.UI.UiBackendProvider.#ctor(Inno.Adapter.UI.UiBackendId) a7e39ea4330dad990d316239eaa06b423d6a00daf1d9158b6e994a3e40655d0e -->
<!-- api: P:Inno.Adapter.UI.UiBackendProvider.id 584cd981b7f2043e7945740adf8a1280ca538caef1be2a08276525dbcb9c25f8 -->
<!-- api: M:Inno.Adapter.UI.UiBackendProvider.CreateBackend cf1a93e68086365e9297c44736564cd294ac581e01a62df753cf6e376ef5a47c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-5f6b6abb4b2b7a69"></a>[`protected Inno.Adapter.UI.UiBackendProvider.UiBackendProvider(Inno.Adapter.UI.UiBackendId id)`](../../src/adapters/ui/Inno.Adapter.UI/UiBackendProvider.cs#L20) | Captures the registration identity assigned by the composition owner.<br>`id`：The assigned implementation identity. | — | `System.ArgumentException`：The identity is unassigned. |
| <a id="api-30be8736bfac5a6e"></a>[`public abstract Inno.UI.IUiBackend Inno.Adapter.UI.UiBackendProvider.CreateBackend()`](../../src/adapters/ui/Inno.Adapter.UI/UiBackendProvider.cs#L38) | Creates one caller-owned backend generation.<br>返回：A new backend. | — | — |
| <a id="api-40319a37560893aa"></a>[`public Inno.Adapter.UI.UiBackendId Inno.Adapter.UI.UiBackendProvider.id { get; }`](../../src/adapters/ui/Inno.Adapter.UI/UiBackendProvider.cs#L30) | Gets this provider's stable implementation identity. | — | — |
| <a id="api-6293e0c692c4dfe5"></a>[`public abstract class Inno.Adapter.UI.UiBackendProvider`](../../src/adapters/ui/Inno.Adapter.UI/UiBackendProvider.cs#L9) | Creates one concrete UI backend registered by a composition root. | — | — |

## 项目依赖

- [Inno.UI](Inno.UI.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
