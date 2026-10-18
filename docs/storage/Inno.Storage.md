# Inno.Storage

[Storage 索引](README.md) · [Runtime](Inno.Storage.Runtime.md) · [Wiki 首页](../README.md)

`Inno.Storage` 公开 `IApplicationStorage`，只接受规范化 `StorageKey`，不向脚本暴露绝对路径。全部操作支持 cancellation；写入语义是完整值原子替换。

## 公开 API

| API | 语义 |
| --- | --- |
| `StorageKey` | 不允许 rooted、空段、`.`、`..` 或 drive-qualified 的 slash key。 |
| `IApplicationStorage` | `ExistsAsync`、`ReadAsync`、`WriteAsync`、`DeleteAsync`、`ListAsync`。 |
| `StorageExecutionContext` | `AsyncLocal` 隔离且严格 LIFO。 |
| `Storage` | 脚本友好的无状态异步 façade。 |

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;
using InnoEngine.Storage;

static async ValueTask<byte[]?> SaveAndRead(
    ReadOnlyMemory<byte> bytes,
    CancellationToken cancellationToken
) {
    StorageKey key = new("saves/slot-1.bin");
    await Storage.WriteAsync(key, bytes, cancellationToken);
    return await Storage.ReadAsync(key, cancellationToken);
}
```

无活动 scope 时 façade 抛出明确异常。应用存档格式由项目或 Plugin 定义，不进入本体 Storage。

[下一页：Inno.Storage.Runtime](Inno.Storage.Runtime.md)






## 本轮边界与所有权

StorageScope 表达中立应用 namespace；不携带物理根。application ID 或显式配置决定 namespace，FileSystem provider 的宿主配置决定实际位置，Browser provider 映射到 origin 存储。Editor Edit/Play/Project 保持隔离，不添加 InnoEngine 产品路径前缀。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Storage.IApplicationStorage`

<!-- api: T:Inno.Storage.IApplicationStorage eacae9b666303a81748fb6331fab8959bd899ce0b4099556aa060d7195b97214 -->
<!-- api: M:Inno.Storage.IApplicationStorage.ExistsAsync(Inno.Storage.StorageKey,System.Threading.CancellationToken) ba233d0da76709978f2fc2772a6a330fd155bd7190e3e55dea79e7263f333200 -->
<!-- api: M:Inno.Storage.IApplicationStorage.ReadAsync(Inno.Storage.StorageKey,System.Threading.CancellationToken) f49c09953d2c005c6ebd2e82bb48b441ab18c74939801f64cdc9b18170b8f9f4 -->
<!-- api: M:Inno.Storage.IApplicationStorage.WriteAsync(Inno.Storage.StorageKey,System.ReadOnlyMemory{System.Byte},System.Threading.CancellationToken) 43247e0129b8b02b6f10b715ad4049464d9b50e3a4d053dc95e67af6dc93bd0a -->
<!-- api: M:Inno.Storage.IApplicationStorage.DeleteAsync(Inno.Storage.StorageKey,System.Threading.CancellationToken) 599bd0064f561ca5a84fbd716c5906f8119baf03812a01e711bcc7f4e2cfa1c1 -->
<!-- api: M:Inno.Storage.IApplicationStorage.ListAsync(System.Nullable{Inno.Storage.StorageKey},System.Threading.CancellationToken) 9197418366de3ffaaec57a19c2f6269c761885bdd20b2309c911ce4bdfb26f36 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-cda5d4ec036816c0"></a>[`System.Threading.Tasks.ValueTask<bool> Inno.Storage.IApplicationStorage.DeleteAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/storage/Inno.Storage/IApplicationStorage.cs#L80) | Deletes one value without failing when it is already absent.<br>返回：true when an existing value was deleted.<br>`key`：The sandbox-relative storage key.<br>`cancellationToken`：Cancellation requested by the caller. | — | — |
| <a id="api-ad3020a9dff73b3f"></a>[`System.Threading.Tasks.ValueTask<bool> Inno.Storage.IApplicationStorage.ExistsAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/storage/Inno.Storage/IApplicationStorage.cs#L25) | Determines whether a value exists.<br>返回：true when the value exists.<br>`key`：The sandbox-relative storage key.<br>`cancellationToken`：Cancellation requested by the caller. | — | — |
| <a id="api-ff7b552161e58bd1"></a>[`System.Threading.Tasks.ValueTask<System.Collections.Generic.IReadOnlyList<Inno.Storage.StorageKey>> Inno.Storage.IApplicationStorage.ListAsync(Inno.Storage.StorageKey? prefix = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/storage/Inno.Storage/IApplicationStorage.cs#L97) | Lists immutable keys below an optional logical prefix.<br>返回：Keys in deterministic ordinal order.<br>`prefix`：An optional normalized directory prefix.<br>`cancellationToken`：Cancellation requested by the caller. | — | — |
| <a id="api-9e26f73c2b41c170"></a>[`System.Threading.Tasks.ValueTask<byte[]?> Inno.Storage.IApplicationStorage.ReadAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/storage/Inno.Storage/IApplicationStorage.cs#L42) | Reads a complete immutable value.<br>返回：The stored bytes, or null when the key does not exist.<br>`key`：The sandbox-relative storage key.<br>`cancellationToken`：Cancellation requested by the caller. | — | — |
| <a id="api-ad639962a25cd115"></a>[`System.Threading.Tasks.ValueTask Inno.Storage.IApplicationStorage.WriteAsync(Inno.Storage.StorageKey key, System.ReadOnlyMemory<byte> value, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/storage/Inno.Storage/IApplicationStorage.cs#L62) | Atomically replaces a complete value.<br>返回：A task that completes after the value is durably replaced.<br>`key`：The sandbox-relative storage key.<br>`value`：The complete value to commit.<br>`cancellationToken`：Cancellation requested by the caller. | — | — |
| <a id="api-a2689b2828c5bca4"></a>[`public interface Inno.Storage.IApplicationStorage`](../../src/services/storage/Inno.Storage/IApplicationStorage.cs#L11) | Provides asynchronous atomic access to one application-specific persistent sandbox. | — | — |

### `Inno.Storage.Storage`

<!-- api: T:Inno.Storage.Storage 927386b6b71ca5a211114ac148be649b0df2e25878f033ed6da05f9b4eed3db4 -->
<!-- api: M:Inno.Storage.Storage.ExistsAsync(Inno.Storage.StorageKey,System.Threading.CancellationToken) 37b97323684c60cfcb397dfc6132779df0833f40a05771414c407dc8f555ee5a -->
<!-- api: M:Inno.Storage.Storage.ReadAsync(Inno.Storage.StorageKey,System.Threading.CancellationToken) b44f9c2d86d63a1fed5db8c24b2cecba37ca2459055aa285d14f4b76ced6c038 -->
<!-- api: M:Inno.Storage.Storage.WriteAsync(Inno.Storage.StorageKey,System.ReadOnlyMemory{System.Byte},System.Threading.CancellationToken) b9ef7d69fe21ebe7e81764a040e0a398d407e2886bc98370f784e9bbe56a989f -->
<!-- api: M:Inno.Storage.Storage.DeleteAsync(Inno.Storage.StorageKey,System.Threading.CancellationToken) f7b57e1368ac0fbd118330ca4cbc6b12d0dfae70a07a0dca80e10cbb1282d7ee -->
<!-- api: M:Inno.Storage.Storage.ListAsync(System.Nullable{Inno.Storage.StorageKey},System.Threading.CancellationToken) c8bae4cb09a0bfce8047131132957f6c40724219e5878d2b7898a8bc0023cd2c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-eccbaf8c9bb3aefb"></a>[`public static System.Threading.Tasks.ValueTask<bool> Inno.Storage.Storage.DeleteAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/storage/Inno.Storage/Storage.cs#L116) | Deletes one value.<br>返回：true when a value was deleted.<br>`key`：The sandbox-relative storage key.<br>`cancellationToken`：Cancellation requested by the caller. | — | — |
| <a id="api-1268f36a35b9729a"></a>[`public static System.Threading.Tasks.ValueTask<bool> Inno.Storage.Storage.ExistsAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/storage/Inno.Storage/Storage.cs#L58) | Determines whether a value exists.<br>返回：true when the value exists.<br>`key`：The sandbox-relative storage key.<br>`cancellationToken`：Cancellation requested by the caller. | — | — |
| <a id="api-7fb549b7841f7b6f"></a>[`public static System.Threading.Tasks.ValueTask<System.Collections.Generic.IReadOnlyList<Inno.Storage.StorageKey>> Inno.Storage.Storage.ListAsync(Inno.Storage.StorageKey? prefix = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/storage/Inno.Storage/Storage.cs#L134) | Lists keys below an optional prefix.<br>返回：Keys in deterministic ordinal order.<br>`prefix`：An optional normalized directory prefix.<br>`cancellationToken`：Cancellation requested by the caller. | — | — |
| <a id="api-1f6a48d1958a5f51"></a>[`public static System.Threading.Tasks.ValueTask<byte[]?> Inno.Storage.Storage.ReadAsync(Inno.Storage.StorageKey key, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/storage/Inno.Storage/Storage.cs#L76) | Reads a complete value.<br>返回：The stored bytes, or null when absent.<br>`key`：The sandbox-relative storage key.<br>`cancellationToken`：Cancellation requested by the caller. | — | — |
| <a id="api-7d0a6ec983185fcb"></a>[`public static System.Threading.Tasks.ValueTask Inno.Storage.Storage.WriteAsync(Inno.Storage.StorageKey key, System.ReadOnlyMemory<byte> value, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/storage/Inno.Storage/Storage.cs#L97) | Atomically replaces a complete value.<br>返回：A task that completes after commit.<br>`key`：The sandbox-relative storage key.<br>`value`：The complete value to commit.<br>`cancellationToken`：Cancellation requested by the caller. | — | — |
| <a id="api-69793dfc1818dda4"></a>[`public static class Inno.Storage.Storage`](../../src/services/storage/Inno.Storage/Storage.cs#L44) | Provides script-friendly access to the current application storage sandbox. | — | — |

### `Inno.Storage.StorageExecutionContext`

<!-- api: T:Inno.Storage.StorageExecutionContext 92117b110b8a1ce30e02c2e1d9048d50d98e98572f299a3c46f229c7a1685ef5 -->
<!-- api: P:Inno.Storage.StorageExecutionContext.current a8372efe95a48d133bea90a9c0c47b2f940986757fe7e41e9bf4f3b769b77f70 -->
<!-- api: M:Inno.Storage.StorageExecutionContext.EnterScope(Inno.Storage.IApplicationStorage) 752ca46f4dbfa16283a12bb349ac1f9ff44bd7e37f4b97153fc54680061e3a74 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-5b50cb65bd16704b"></a>[`public static System.IDisposable Inno.Storage.StorageExecutionContext.EnterScope(Inno.Storage.IApplicationStorage storage)`](../../src/services/storage/Inno.Storage/Storage.cs#L33) | Binds a storage service until the returned strict last-in-first-out scope is disposed.<br>返回：A strict last-in-first-out execution scope.<br>`storage`：The host-owned application storage service. | — | — |
| <a id="api-85d0aef315a2062f"></a>[`public static Inno.Storage.IApplicationStorage Inno.Storage.StorageExecutionContext.current { get; }`](../../src/services/storage/Inno.Storage/Storage.cs#L22) | Gets the storage service bound to the current execution context. | — | `System.InvalidOperationException`：Thrown when no storage scope is active. |
| <a id="api-9103bb0746c8cd96"></a>[`public static class Inno.Storage.StorageExecutionContext`](../../src/services/storage/Inno.Storage/Storage.cs#L12) | Binds application storage to the current asynchronous execution context. | — | — |

### `Inno.Storage.StorageKey`

<!-- api: T:Inno.Storage.StorageKey 49ff730009d36feefa4c8a0c67e10fa1d8cbcc83dbfba6e63a7a2bcdadf3a757 -->
<!-- api: M:Inno.Storage.StorageKey.#ctor(System.String) 56e5979e9aad07bc68fc8bf2e8e7562b96277b7250dc7d5f1f965b946730c134 -->
<!-- api: P:Inno.Storage.StorageKey.value 484bd2beceafd2766debebb34461115cb679ec0626b4bdebcc2beaab4aa18509 -->
<!-- api: P:Inno.Storage.StorageKey.isValid 4c088147096af0055a38ef775ed913ba871439f4d3d072d68dd11e78080d32cd -->
<!-- api: M:Inno.Storage.StorageKey.ToString e5735dd8e0a547372828a5b634c6077753b1158d02ba6d9db12a0619a66e4a39 -->
<!-- api: M:Inno.Storage.StorageKey.op_Inequality(Inno.Storage.StorageKey,Inno.Storage.StorageKey) 9cef0884bf8e9919b3482615242284fb48a0beb53037979defa6e81dabf4e680 -->
<!-- api: M:Inno.Storage.StorageKey.op_Equality(Inno.Storage.StorageKey,Inno.Storage.StorageKey) 5a3fcfb5118f212843ec1ab66112e9a258d42b97cfc6d0402dadc752b3ea92b7 -->
<!-- api: M:Inno.Storage.StorageKey.GetHashCode 07535f9ec121b5a3d02b1b394cd6bb80fe33df36cfa1ddffa634fea1cd2c4335 -->
<!-- api: M:Inno.Storage.StorageKey.Equals(System.Object) c4e313ff2e8761c076e68d81f18558c0400ee357e9ad43093424857240c31798 -->
<!-- api: M:Inno.Storage.StorageKey.Equals(Inno.Storage.StorageKey) b62739e0bd9e13561f550284521907287676f045f78628c125266ab5bdd7f951 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-989d7d06c37f8487"></a>[`public Inno.Storage.StorageKey.StorageKey(string value)`](../../src/services/storage/Inno.Storage/StorageKey.cs#L19) | Creates a normalized storage key from slash-separated path segments.<br>`value`：The non-rooted key with no empty, current, or parent segments. | — | `System.ArgumentException`：Thrown when value can escape or ambiguously address the storage root. |
| <a id="api-e02cc2207fc36efe"></a>[`public bool Inno.Storage.StorageKey.Equals(Inno.Storage.StorageKey other)`](../../src/services/storage/Inno.Storage/StorageKey.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-b432c0960ad8d329"></a>[`public override bool Inno.Storage.StorageKey.Equals(object obj)`](../../src/services/storage/Inno.Storage/StorageKey.cs#L8) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-1290073616002314"></a>[`public override int Inno.Storage.StorageKey.GetHashCode()`](../../src/services/storage/Inno.Storage/StorageKey.cs#L8) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-68fb30f4c1647932"></a>[`public static bool Inno.Storage.StorageKey.operator ==(Inno.Storage.StorageKey left, Inno.Storage.StorageKey right)`](../../src/services/storage/Inno.Storage/StorageKey.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-a9b28faaecfd72e8"></a>[`public static bool Inno.Storage.StorageKey.operator !=(Inno.Storage.StorageKey left, Inno.Storage.StorageKey right)`](../../src/services/storage/Inno.Storage/StorageKey.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-4622091129bbcb80"></a>[`public override string Inno.Storage.StorageKey.ToString()`](../../src/services/storage/Inno.Storage/StorageKey.cs#L52) | Formats this key for diagnostics and persistence.<br>返回：The normalized key value. | — | — |
| <a id="api-147cc5d9bd2113e7"></a>[`public bool Inno.Storage.StorageKey.isValid { get; }`](../../src/services/storage/Inno.Storage/StorageKey.cs#L44) | Gets whether this value contains a valid storage key. | — | — |
| <a id="api-afa390f0f162793c"></a>[`public string Inno.Storage.StorageKey.value { get; }`](../../src/services/storage/Inno.Storage/StorageKey.cs#L39) | Gets the normalized slash-separated key value. | — | — |
| <a id="api-a8365331f405021e"></a>[`public readonly record struct Inno.Storage.StorageKey : System.IEquatable<Inno.Storage.StorageKey>`](../../src/services/storage/Inno.Storage/StorageKey.cs#L8) | Identifies one sandbox-relative application storage value. | — | — |

### `Inno.Storage.StorageScope`

<!-- api: T:Inno.Storage.StorageScope a9927897495afa32e3e72cad82afd487b5aa0b1c1b033e8b558b480356b14f21 -->
<!-- api: M:Inno.Storage.StorageScope.#ctor(System.String) a3549b64005ebda3570a8ce2be8591ff470bf422b032e38fbb66eaeb2a8a858a -->
<!-- api: P:Inno.Storage.StorageScope.value 891b0f871ed3db5ea40460f76daaec1efc63eb6d5b4cb861ac35881aa33a423f -->
<!-- api: P:Inno.Storage.StorageScope.isValid 73465defe06295603420452d69f4c6e7aa915a127c400f07400c53de3efb57a9 -->
<!-- api: M:Inno.Storage.StorageScope.ToString 3ed027d342bc1e12e4931a1c266cb681aa8fb094e908c2d579ff358f39b51cba -->
<!-- api: M:Inno.Storage.StorageScope.op_Inequality(Inno.Storage.StorageScope,Inno.Storage.StorageScope) f7c0f7efd47c7f3ac254e3c6ff2ac078eda14b4424d697c42ab33c1c768163dd -->
<!-- api: M:Inno.Storage.StorageScope.op_Equality(Inno.Storage.StorageScope,Inno.Storage.StorageScope) ff37db1a8dbc03ef1f651ed136c0ba8d46c5165d150d22536e37381137838dfd -->
<!-- api: M:Inno.Storage.StorageScope.GetHashCode 5d4648e49a7f4c7a0d0af41b23dd70d8288b3d3e14f0e7d7780b8a78f06cd9f4 -->
<!-- api: M:Inno.Storage.StorageScope.Equals(System.Object) 5a4d669961396b28cc8f0c33ca2d908254b9277009f1a4f3cd21de73a2f64fba -->
<!-- api: M:Inno.Storage.StorageScope.Equals(Inno.Storage.StorageScope) 880d538c5d61aca134dd9d2b0fa17be5427622c8e6fa4224641250937564f00a -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-07fe1ece1e7749e1"></a>[`public Inno.Storage.StorageScope.StorageScope(string value)`](../../src/services/storage/Inno.Storage/StorageScope.cs#L20) | Captures a portable, case-sensitive application namespace.<br>`value`：A nonempty namespace containing ASCII letters, digits, dots, underscores, or hyphens. | — | `System.ArgumentException`：The namespace is empty, contains path syntax, has a leading or trailing dot, or contains a reserved portable device name. |
| <a id="api-8485d60e38473455"></a>[`public bool Inno.Storage.StorageScope.Equals(Inno.Storage.StorageScope other)`](../../src/services/storage/Inno.Storage/StorageScope.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-7e166fd7e0005fe5"></a>[`public override bool Inno.Storage.StorageScope.Equals(object obj)`](../../src/services/storage/Inno.Storage/StorageScope.cs#L8) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-7a7c5e46f25a8ffd"></a>[`public override int Inno.Storage.StorageScope.GetHashCode()`](../../src/services/storage/Inno.Storage/StorageScope.cs#L8) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-8bb799f07c097972"></a>[`public static bool Inno.Storage.StorageScope.operator ==(Inno.Storage.StorageScope left, Inno.Storage.StorageScope right)`](../../src/services/storage/Inno.Storage/StorageScope.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-ba55c2c4fc6d1765"></a>[`public static bool Inno.Storage.StorageScope.operator !=(Inno.Storage.StorageScope left, Inno.Storage.StorageScope right)`](../../src/services/storage/Inno.Storage/StorageScope.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-68c1101320f7aa9a"></a>[`public override string Inno.Storage.StorageScope.ToString()`](../../src/services/storage/Inno.Storage/StorageScope.cs#L58) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-740813bb8a3c5c42"></a>[`public bool Inno.Storage.StorageScope.isValid { get; }`](../../src/services/storage/Inno.Storage/StorageScope.cs#L55) | Gets whether this value identifies a storage namespace. | — | — |
| <a id="api-f0b5ce218829466e"></a>[`public string? Inno.Storage.StorageScope.value { get; }`](../../src/services/storage/Inno.Storage/StorageScope.cs#L50) | Gets the logical namespace, or null for an unassigned value. | — | — |
| <a id="api-2692c5b144332fb4"></a>[`public readonly record struct Inno.Storage.StorageScope : System.IEquatable<Inno.Storage.StorageScope>`](../../src/services/storage/Inno.Storage/StorageScope.cs#L8) | Identifies an application data namespace independently of its physical storage location. | — | — |

## 项目依赖

- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
