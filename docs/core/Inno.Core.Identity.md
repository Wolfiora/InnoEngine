# Inno.Core.Identity

[上一页：Job](Inno.Core.Jobs.md) · [Core 索引](README.md) · [下一页：Input](Inno.Core.Input.md) · [跨域引用与热重载标准](../architecture/IDENTITY_REFERENCE_RELOAD_STANDARD.md)

Identity 模块为引擎对象提供两个层次的身份：跨保存/重载保持的 `persistentId`，以及只在当前 Registry 注册期间有效的 `runtimeId`。需要身份的对象继承 `IdentityObject`，并由明确生命周期 owner 的 `IdentityAllocator` 注册。

## Identity

`Identity` 是可复制 struct：

| 成员 | 说明 |
| --- | --- |
| `Identity(Guid persistentId)` | 创建未注册的 identity。 |
| `persistentId` | 持久 Guid；注册时为空会生成新值。 |
| `int? runtimeId` | Registry 仍存活且绑定有效时返回 packed runtime ID，否则 null。 |

复制 Identity 只是复制快照；runtime 有效性仍由对 Registry 的弱引用验证。

## IdentityObject

抽象基类提供实例 identity 存储：

- `identity`：返回当前 persistent/runtime identity snapshot。
- 新实例立即具有非空 persistent ID，但在 allocator 注册前没有 runtime ID。
- identity 绑定和替换只由同程序集的 Identity 基础设施执行。

```csharp
using System;
using Inno.Core.Identity;

RuntimeResource resource = new();
Guid persistentId = resource.identity.persistentId;
Console.WriteLine(persistentId);

internal sealed class RuntimeResource : IdentityObject;
```

Registry 使用弱 object entry 和 `ConditionalWeakTable` slot 映射，不会仅因索引本身阻止失去 owner 的对象 GC。

## IdentityAllocator

| 成员 | 说明 |
| --- | --- |
| `ObjectUnregistered` | 对象从 Registry 永久移除后触发；所有 handler 都执行，失败聚合。 |
| `EnterScope()` | 将当前 Session 的 Allocator 绑定到异步执行上下文。 |
| `Register(obj, Guid? override = null)` | 绑定 runtime ID；已注册返回 `false`。 |
| `InitializePersistentIdentity(obj, Guid)` | 给未注册对象指定非空 persistent ID。 |
| `Unregister(obj)` | 移除 runtime 映射并保留 persistent ID。 |
| `Get<TIdentity>(int runtimeId)` | 按 runtime ID 查找，类型不匹配/陈旧时 null。 |
| `Get<TIdentity>(Guid persistentId)` | 按 persistent ID 查找。 |

```csharp
using System;
using Inno.Core.Identity;

static Guid RegisterAndRelease(Guid savedId)
{
    var identities = new IdentityAllocator();
    RuntimeResource resource = new();
    identities.InitializePersistentIdentity(resource, savedId);
    identities.Register(resource);
    try
    {
        int runtimeId = resource.identity.runtimeId!.Value;
        RuntimeResource? same = identities.Get<RuntimeResource>(runtimeId);
        return same!.identity.persistentId;
    }
    finally
    {
        identities.Unregister(resource);
    }
}

internal sealed class RuntimeResource : IdentityObject;
```

## 冲突与陈旧 ID

- 两个不同存活对象不能注册相同 persistent ID；冲突抛 `InvalidOperationException`。
- runtime ID 由 slot + generation 编码。对象移除后 slot 可复用，但旧 ID 的 generation 不匹配，因此不会解析到新对象。
- `InitializePersistentIdentity` 只允许未注册对象；空 Guid、null 或已注册状态会失败。
- Unregister event 在 Registry 已更新后触发，即使 handler 抛错也不会恢复对象。
- 每个 `RuntimeSession` 拥有独立 `IdentityAllocator`；不存在进程级可变 Identity Manager。

## 热重载用法

原位替换脚本组件时，应把 persistent ID 迁移到新实例，再注册新实例。外部引用优先保存 persistent ID，不应保存裸 runtime ID 或旧实例引用。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Core.Identity.Identity`

<!-- api: T:Inno.Core.Identity.Identity ae23d98bdf9addebb36a5cdec1884ce3e8a5cbfff1648a0a5fb7a638d86432d3 -->
<!-- api: P:Inno.Core.Identity.Identity.persistentId a4ac3f1ea5bd679ec19b677170829f55a8500fe5aa9e9fa4dbb5e6a98fb4bd6b -->
<!-- api: P:Inno.Core.Identity.Identity.runtimeId 1f45203ef6fc65bba21cbbb7b7d67d5ab3fea24eeace44b360dec6c2224c7b4d -->
<!-- api: P:Inno.Core.Identity.Identity.runtimeIdentity c4997aeab9543bfc113c0d259077ed0cb6e820747651a007a14c1f7534a8cf9e -->
<!-- api: M:Inno.Core.Identity.Identity.#ctor(System.Guid) 3f6f58cc730d5183f117bd8f35bf9af6d0fc7f8313548d6fbf5d54d4714bc735 -->
<!-- api: M:Inno.Core.Identity.Identity.Resolve``1 186569f2c8abd3318142e3fb438df276f7d0a3cb164c0eb1e7d64b7d74d3008a -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-ceccb7f74f56ec26"></a>[`public Inno.Core.Identity.Identity.Identity(System.Guid persistentId)`](../../src/foundation/core/Inno.Core.Identity/Identity.cs#L59) | Creates an unbound identity with the supplied persistent identifier.<br>`persistentId`：The stable identifier to preserve across serialization boundaries. | — | — |
| <a id="api-18eb14fb3da25d77"></a>[`public TObject? Inno.Core.Identity.Identity.Resolve<TObject>() where TObject : Inno.Core.Identity.IdentityObject`](../../src/foundation/core/Inno.Core.Identity/Identity.cs#L99) | Resolves this transient snapshot through its original weak identity registry.<br>返回：The current object, or null if the registry retired, the slot changed generation, or the type differs. | — | — |
| <a id="api-7a0513ed5e0290ca"></a>[`public System.Guid Inno.Core.Identity.Identity.persistentId { get; private set; }`](../../src/foundation/core/Inno.Core.Identity/Identity.cs#L13) | Gets the stable identifier preserved by serialization and runtime reconstruction. | — | — |
| <a id="api-4ce65b0bd5c32a01"></a>[`public int? Inno.Core.Identity.Identity.runtimeId { get; }`](../../src/foundation/core/Inno.Core.Identity/Identity.cs#L21) | Gets the identifier assigned by the currently bound runtime registry. | The value is null while this identity is not bound to a live registry. | — |
| <a id="api-670eede97a80627e"></a>[`public Inno.Core.Identity.RuntimeIdentity? Inno.Core.Identity.Identity.runtimeIdentity { get; }`](../../src/foundation/core/Inno.Core.Identity/Identity.cs#L38) | Gets the domain-qualified runtime identity while this object is registered. | The value is null when this identity is detached or its registry is no longer reachable. | — |
| <a id="api-9347eae8dc0c5be0"></a>[`public struct Inno.Core.Identity.Identity`](../../src/foundation/core/Inno.Core.Identity/Identity.cs#L8) | Identity payload for an object, carrying persistent id and runtime id. | — | — |

### `Inno.Core.Identity.IdentityAllocator`

<!-- api: T:Inno.Core.Identity.IdentityAllocator 71eac4e4b19542242c806a9417adabf4f50c3ddd07584cf3b315a39136443e6c -->
<!-- api: M:Inno.Core.Identity.IdentityAllocator.#ctor ba67a1be3b834d56934b6a4262f7b6a8967e36126a0830d0a6208fa56f3f6bd5 -->
<!-- api: P:Inno.Core.Identity.IdentityAllocator.hasCurrent 5556b93f4568c5a362440b07d900be1550f9eca1664277a4d8210ce333a5fa7e -->
<!-- api: P:Inno.Core.Identity.IdentityAllocator.current b58f0bc971a5390e04ad99eea9076166161fbdc23b9707ec9d563d229cd1be39 -->
<!-- api: P:Inno.Core.Identity.IdentityAllocator.count 0530fe54d82648977af79947067ef72b698cb34eb2a70e789da5acebe96f2baf -->
<!-- api: P:Inno.Core.Identity.IdentityAllocator.domainId 35c9fbc45a2cc2cb0c7a32f34ccf1ae050385c68be62716d4ee85e92c1729735 -->
<!-- api: E:Inno.Core.Identity.IdentityAllocator.ObjectUnregistered 4d1da146e134d923faa1829c9592eb944bde2ce5cbc31f4b42a89a0b9aba1b54 -->
<!-- api: M:Inno.Core.Identity.IdentityAllocator.EnterScope 58ab0cd9fba870b0119a9802e0be854b38052b25e11104869ba9f58baea89a39 -->
<!-- api: M:Inno.Core.Identity.IdentityAllocator.Register(Inno.Core.Identity.IdentityObject,System.Nullable{System.Guid}) eda085c2ab82320e1e7b63e9c717656d1f24828de8da489b0ab3826e4bce54d7 -->
<!-- api: M:Inno.Core.Identity.IdentityAllocator.InitializePersistentIdentity(Inno.Core.Identity.IdentityObject,System.Guid) 1cff89322d4a7d5a7d0f0dd728cf7899ccb05357127b5701c5b91edc0f625b73 -->
<!-- api: M:Inno.Core.Identity.IdentityAllocator.Unregister(Inno.Core.Identity.IdentityObject) ca348d607b0388fbaa0feea1025465265e11dcaab8cdbb383e1fb7a7a4226622 -->
<!-- api: M:Inno.Core.Identity.IdentityAllocator.Get``1(System.Int32) b8d359ab7db5c4b32da7d4a22f128c7dfac97b6381b8b70e6de0584ad07bd6f0 -->
<!-- api: M:Inno.Core.Identity.IdentityAllocator.Get``1(Inno.Core.Identity.RuntimeIdentity) b5350b9c92820a6bcf89571e274bea0e2ee4512b6c545695a10c7279396800d2 -->
<!-- api: M:Inno.Core.Identity.IdentityAllocator.Get``1(System.Guid) 4c859c635a73b32d88a1d7813be59c370b206864b6be25f9e0f1d3a38291b684 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-832408f50987af75"></a>[`public System.Action<Inno.Core.Identity.IdentityObject>? Inno.Core.Identity.IdentityAllocator.ObjectUnregistered`](../../src/foundation/core/Inno.Core.Identity/IdentityAllocator.cs#L63) | Occurs after an object has been permanently removed from this allocator. | Every observer is invoked. Multiple observer failures are reported as one System.AggregateException after notification completes. | — |
| <a id="api-fb1a4cbd6bf3937e"></a>[`public Inno.Core.Identity.IdentityAllocator.IdentityAllocator()`](../../src/foundation/core/Inno.Core.Identity/IdentityAllocator.cs#L24) | Creates an isolated runtime identity domain. | — | `System.InvalidOperationException`：Thrown if the process exhausts the available identity-domain values. |
| <a id="api-6f5a0a8b6a323668"></a>[`public System.IDisposable Inno.Core.Identity.IdentityAllocator.EnterScope()`](../../src/foundation/core/Inno.Core.Identity/IdentityAllocator.cs#L74) | Binds this allocator to the current asynchronous execution context until the returned scope is disposed.<br>返回：A strict last-in-first-out scope owned by the caller. | — | `System.InvalidOperationException`：Thrown when scopes are disposed out of order. |
| <a id="api-9fb0cfdc2ff030dc"></a>[`public TIdentity? Inno.Core.Identity.IdentityAllocator.Get<TIdentity>(Inno.Core.Identity.RuntimeIdentity identity) where TIdentity : Inno.Core.Identity.IdentityObject`](../../src/foundation/core/Inno.Core.Identity/IdentityAllocator.cs#L192) | Resolves a domain-qualified runtime identity without allowing cross-session aliasing.<br>返回：The live compatible object, or null when the domain, generation, or type does not match.<br>`identity`：The transient identity containing both its owner domain and runtime identifier. | — | — |
| <a id="api-d4f47120ddfd836e"></a>[`public TIdentity? Inno.Core.Identity.IdentityAllocator.Get<TIdentity>(System.Guid persistentId) where TIdentity : Inno.Core.Identity.IdentityObject`](../../src/foundation/core/Inno.Core.Identity/IdentityAllocator.cs#L210) | Resolves a registered object by its persistent identity.<br>返回：The live compatible object, or null when no matching object exists.<br>`persistentId`：The non-empty persistent identity to resolve. | — | — |
| <a id="api-05ce7a7720d9674e"></a>[`public TIdentity? Inno.Core.Identity.IdentityAllocator.Get<TIdentity>(int runtimeId) where TIdentity : Inno.Core.Identity.IdentityObject`](../../src/foundation/core/Inno.Core.Identity/IdentityAllocator.cs#L176) | Resolves a registered object by its session-local runtime identity.<br>返回：The live compatible object, or null when no matching object exists.<br>`runtimeId`：The packed runtime identity allocated by this instance. | — | — |
| <a id="api-d528fbadbd0d8873"></a>[`public void Inno.Core.Identity.IdentityAllocator.InitializePersistentIdentity(Inno.Core.Identity.IdentityObject obj, System.Guid persistentId)`](../../src/foundation/core/Inno.Core.Identity/IdentityAllocator.cs#L114) | Assigns a persistent identity to a detached object without allocating a runtime identity.<br>`obj`：The detached identity object to initialize.<br>`persistentId`：The non-empty persistent identity to assign. | — | `System.ArgumentNullException`：Thrown when obj is null.<br>`System.ArgumentException`：Thrown when persistentId is empty.<br>`System.InvalidOperationException`：Thrown when the object already has a live runtime identity. |
| <a id="api-9bfe47e598e6ac94"></a>[`public bool Inno.Core.Identity.IdentityAllocator.Register(Inno.Core.Identity.IdentityObject obj, System.Guid? persistentId = null)`](../../src/foundation/core/Inno.Core.Identity/IdentityAllocator.cs#L91) | Registers an object and assigns its session-local runtime identity.<br>返回：true when the object was newly registered; otherwise, false.<br>`obj`：The identity object owned by this runtime session.<br>`persistentId`：An optional explicit persistent identity; an identity is generated when omitted or empty. | — | — |
| <a id="api-9ac6b3624b129f7c"></a>[`public bool Inno.Core.Identity.IdentityAllocator.Unregister(Inno.Core.Identity.IdentityObject obj)`](../../src/foundation/core/Inno.Core.Identity/IdentityAllocator.cs#L138) | Removes an object from this session and notifies every unregistration observer.<br>返回：true when the object was registered; otherwise, false.<br>`obj`：The identity object to remove. | — | `System.AggregateException`：Thrown after removal when one or more observers fail. |
| <a id="api-17b69b81370f7a3e"></a>[`public int Inno.Core.Identity.IdentityAllocator.count { get; }`](../../src/foundation/core/Inno.Core.Identity/IdentityAllocator.cs#L49) | Gets the number of currently registered live identity objects. | — | — |
| <a id="api-1ce87a44000ebb99"></a>[`public static Inno.Core.Identity.IdentityAllocator Inno.Core.Identity.IdentityAllocator.current { get; }`](../../src/foundation/core/Inno.Core.Identity/IdentityAllocator.cs#L44) | Gets the allocator bound to the current asynchronous execution context. | — | `System.InvalidOperationException`：Thrown when the caller is outside a runtime session execution scope. |
| <a id="api-bd2f6bcd3e638afc"></a>[`public Inno.Core.Identity.IdentityDomainId Inno.Core.Identity.IdentityAllocator.domainId { get; }`](../../src/foundation/core/Inno.Core.Identity/IdentityAllocator.cs#L54) | Gets the process-unique domain that qualifies every runtime identity allocated here. | — | — |
| <a id="api-d8de76ab90605d01"></a>[`public static bool Inno.Core.Identity.IdentityAllocator.hasCurrent { get; }`](../../src/foundation/core/Inno.Core.Identity/IdentityAllocator.cs#L36) | Gets whether the current asynchronous execution context is bound to an allocator. | — | — |
| <a id="api-f78dd27950048801"></a>[`public sealed class Inno.Core.Identity.IdentityAllocator`](../../src/foundation/core/Inno.Core.Identity/IdentityAllocator.cs#L11) | Owns the persistent-to-runtime identity map for one isolated runtime session. | — | — |

### `Inno.Core.Identity.IdentityDomainId`

<!-- api: T:Inno.Core.Identity.IdentityDomainId 4738d0fcb2e5c60e48c6f96a0e7c9d8e5e4b971031e6d53dbcbdc38941ac1cfb -->
<!-- api: P:Inno.Core.Identity.IdentityDomainId.value 58f1eca0053a6cc6ec05e6655eda22acf6717098da679650a81627ab60882343 -->
<!-- api: P:Inno.Core.Identity.IdentityDomainId.isValid 51c4c701dec73a93a35b3891f2c5a61176b72671b74d2159f9a0a73417776c1a -->
<!-- api: M:Inno.Core.Identity.IdentityDomainId.Equals(Inno.Core.Identity.IdentityDomainId) 5309e9d4edbcdb5044599bec042ad6078051f9c689a39251887fbc245b342cb9 -->
<!-- api: M:Inno.Core.Identity.IdentityDomainId.Equals(System.Object) 3c236f30444a6e1141e2bd83e90df6e1f240fb08be053481412549571fa9c529 -->
<!-- api: M:Inno.Core.Identity.IdentityDomainId.GetHashCode 1c6baaf1ae2f1921a6886e23a07f772546552e99efc3c415530c975318b5b8ee -->
<!-- api: M:Inno.Core.Identity.IdentityDomainId.ToString 68d8fc75f0b31c787ffdc06e49f215c3da098e9d5e329a3edd356bbd00f3dc7f -->
<!-- api: M:Inno.Core.Identity.IdentityDomainId.op_Equality(Inno.Core.Identity.IdentityDomainId,Inno.Core.Identity.IdentityDomainId) d09a76d22ed4a4fd4f4853895164398a54c1edfed63e31a62d3c28c53dd88f00 -->
<!-- api: M:Inno.Core.Identity.IdentityDomainId.op_Inequality(Inno.Core.Identity.IdentityDomainId,Inno.Core.Identity.IdentityDomainId) dd3c1afd51fcfd5b4e48236c2045d5e1328a59c3ca93ad4f25f45ebfb44b1ce0 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-75f50dbdb74fd521"></a>[`public bool Inno.Core.Identity.IdentityDomainId.Equals(Inno.Core.Identity.IdentityDomainId other)`](../../src/foundation/core/Inno.Core.Identity/IdentityDomainId.cs#L37) | Determines whether this value and another value identify the same runtime domain.<br>返回：true when both values identify the same domain; otherwise, false.<br>`other`：The domain identifier to compare with this value. | — | — |
| <a id="api-f8d6a57fad5b14b8"></a>[`public override bool Inno.Core.Identity.IdentityDomainId.Equals(object? obj)`](../../src/foundation/core/Inno.Core.Identity/IdentityDomainId.cs#L48) | Determines whether an object represents the same runtime domain.<br>返回：true when the object is an equal domain identifier; otherwise, false.<br>`obj`：The object to compare with this value. | — | — |
| <a id="api-d9e47fe8899ec2da"></a>[`public override int Inno.Core.Identity.IdentityDomainId.GetHashCode()`](../../src/foundation/core/Inno.Core.Identity/IdentityDomainId.cs#L56) | Computes a hash code for this runtime domain identifier.<br>返回：A hash code derived from the process-local domain value. | — | — |
| <a id="api-0eae18545ba9250e"></a>[`public static bool Inno.Core.Identity.IdentityDomainId.operator ==(Inno.Core.Identity.IdentityDomainId left, Inno.Core.Identity.IdentityDomainId right)`](../../src/foundation/core/Inno.Core.Identity/IdentityDomainId.cs#L78) | Determines whether two values identify the same runtime domain.<br>返回：true when the values are equal; otherwise, false.<br>`left`：The first domain identifier.<br>`right`：The second domain identifier. | — | — |
| <a id="api-e8cceb1521dfdda4"></a>[`public static bool Inno.Core.Identity.IdentityDomainId.operator !=(Inno.Core.Identity.IdentityDomainId left, Inno.Core.Identity.IdentityDomainId right)`](../../src/foundation/core/Inno.Core.Identity/IdentityDomainId.cs#L95) | Determines whether two values identify different runtime domains.<br>返回：true when the values are different; otherwise, false.<br>`left`：The first domain identifier.<br>`right`：The second domain identifier. | — | — |
| <a id="api-57f28063eb53de01"></a>[`public override string Inno.Core.Identity.IdentityDomainId.ToString()`](../../src/foundation/core/Inno.Core.Identity/IdentityDomainId.cs#L64) | Formats this runtime domain identifier for diagnostics.<br>返回：The numeric domain value, or <invalid> when the value is not allocated. | — | — |
| <a id="api-29337279b9a3f675"></a>[`public bool Inno.Core.Identity.IdentityDomainId.isValid { get; }`](../../src/foundation/core/Inno.Core.Identity/IdentityDomainId.cs#L26) | Gets whether this value identifies an allocated runtime domain. | — | — |
| <a id="api-ba9317d5579ebe3f"></a>[`public int Inno.Core.Identity.IdentityDomainId.value { get; }`](../../src/foundation/core/Inno.Core.Identity/IdentityDomainId.cs#L21) | Gets the process-local numeric value assigned to this domain. | — | — |
| <a id="api-23a3e219915d3dd0"></a>[`public readonly struct Inno.Core.Identity.IdentityDomainId : System.IEquatable<Inno.Core.Identity.IdentityDomainId>`](../../src/foundation/core/Inno.Core.Identity/IdentityDomainId.cs#L11) | Identifies one isolated runtime identity namespace within the current process. | Domain identifiers are runtime-only values. They must never be serialized or used as persistent object identity. | — |

### `Inno.Core.Identity.IdentityObject`

<!-- api: T:Inno.Core.Identity.IdentityObject c5d8af87d1f5f3bb9a6a1c02fd63b09dc6ece3194b918261e5f77d0f7289a904 -->
<!-- api: P:Inno.Core.Identity.IdentityObject.identity 0ab2c53f62d845ef715ee9a0f93e61dd1961f6fa9220bad6a81ee6f8c7fc8bfd -->
<!-- api: M:Inno.Core.Identity.IdentityObject.#ctor 174baefe3fe31b119268ac1b70040dd10d22d4d34459e575fcd641628df6c78c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-aec5948761a52394"></a>[`protected Inno.Core.Identity.IdentityObject.IdentityObject()`](../../src/foundation/core/Inno.Core.Identity/IdentityObject.cs#L6) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-af4176cd424cf819"></a>[`public Inno.Core.Identity.Identity Inno.Core.Identity.IdentityObject.identity { get; }`](../../src/foundation/core/Inno.Core.Identity/IdentityObject.cs#L13) | Gets this object's current persistent and optional session-local identity snapshot. | — | — |
| <a id="api-f1cabb67f91f0492"></a>[`public abstract class Inno.Core.Identity.IdentityObject`](../../src/foundation/core/Inno.Core.Identity/IdentityObject.cs#L6) | Provides instance-owned persistent and session-local identity state managed by an Inno.Core.Identity.IdentityAllocator. | — | — |

### `Inno.Core.Identity.RuntimeIdentity`

<!-- api: T:Inno.Core.Identity.RuntimeIdentity 63e876f3bd538ebd55e3b86a7a2d212c44d0525f7b0799ca62e8d8fa499fd3f9 -->
<!-- api: M:Inno.Core.Identity.RuntimeIdentity.#ctor(Inno.Core.Identity.IdentityDomainId,System.Int32) f08aba03a70dd294e65a1c19eb14059a3795a9b7b4f2ced81a72d54f341a887c -->
<!-- api: P:Inno.Core.Identity.RuntimeIdentity.domainId 689d9d1687ed047bf7662ea67e7a6dd951c726fce89915599bfae01f796cb9d1 -->
<!-- api: P:Inno.Core.Identity.RuntimeIdentity.runtimeId d7b22f3e084fa579118499c448b3fbe7ed2665c3d1b61bdcd847b05810e710e0 -->
<!-- api: M:Inno.Core.Identity.RuntimeIdentity.Equals(Inno.Core.Identity.RuntimeIdentity) b8cc60e6667e01c84e0e997c273e689e8c4d7983727e857dd85e111e145cff7c -->
<!-- api: M:Inno.Core.Identity.RuntimeIdentity.Equals(System.Object) b96157da4e295220e8020b76192cfb0be4134a4352275308102e12b78b9a9d4a -->
<!-- api: M:Inno.Core.Identity.RuntimeIdentity.GetHashCode ff0bebb6bf1e02b975a0dcf0830f6f713589d3074e4511fbbed89de829e3bce5 -->
<!-- api: M:Inno.Core.Identity.RuntimeIdentity.ToString d5caa62854f8c64aa712f1a75b9789b1b8a4c79d2dc76f5b3f24ccd9bc9d5866 -->
<!-- api: M:Inno.Core.Identity.RuntimeIdentity.op_Equality(Inno.Core.Identity.RuntimeIdentity,Inno.Core.Identity.RuntimeIdentity) c1b66a8567006108e35f0c514efa1fb44d029e829c5cc6c98abd19bf0c330b06 -->
<!-- api: M:Inno.Core.Identity.RuntimeIdentity.op_Inequality(Inno.Core.Identity.RuntimeIdentity,Inno.Core.Identity.RuntimeIdentity) cc161dcfec706d58b6b37f5944e4a535921a53c644e9fe2551c00e9fdedc775e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-25fd8c87a6da20ac"></a>[`public Inno.Core.Identity.RuntimeIdentity.RuntimeIdentity(Inno.Core.Identity.IdentityDomainId domainId, int runtimeId)`](../../src/foundation/core/Inno.Core.Identity/RuntimeIdentity.cs#L25) | Creates a runtime identity from its domain and generation-safe object identifier.<br>`domainId`：The identity domain that owns the runtime identifier.<br>`runtimeId`：The generation-safe runtime identifier allocated inside the domain. | — | `System.ArgumentException`：Thrown when either value is invalid. |
| <a id="api-7b4b9343fe650354"></a>[`public bool Inno.Core.Identity.RuntimeIdentity.Equals(Inno.Core.Identity.RuntimeIdentity other)`](../../src/foundation/core/Inno.Core.Identity/RuntimeIdentity.cs#L56) | Determines whether this value and another value address the same live identity slot.<br>返回：true when both the domain and runtime identifier are equal; otherwise, false.<br>`other`：The runtime identity to compare with this value. | — | — |
| <a id="api-bdf09237329ab979"></a>[`public override bool Inno.Core.Identity.RuntimeIdentity.Equals(object? obj)`](../../src/foundation/core/Inno.Core.Identity/RuntimeIdentity.cs#L67) | Determines whether an object addresses the same live identity slot.<br>返回：true when the object is an equal runtime identity; otherwise, false.<br>`obj`：The object to compare with this value. | — | — |
| <a id="api-80a83f7b83912c26"></a>[`public override int Inno.Core.Identity.RuntimeIdentity.GetHashCode()`](../../src/foundation/core/Inno.Core.Identity/RuntimeIdentity.cs#L75) | Computes a hash code from the domain and runtime identifier.<br>返回：A hash code consistent with runtime identity equality. | — | — |
| <a id="api-f289518236c81b16"></a>[`public static bool Inno.Core.Identity.RuntimeIdentity.operator ==(Inno.Core.Identity.RuntimeIdentity left, Inno.Core.Identity.RuntimeIdentity right)`](../../src/foundation/core/Inno.Core.Identity/RuntimeIdentity.cs#L97) | Determines whether two runtime identities are equal.<br>返回：true when both values are equal; otherwise, false.<br>`left`：The first runtime identity.<br>`right`：The second runtime identity. | — | — |
| <a id="api-7d58d9620b059f8f"></a>[`public static bool Inno.Core.Identity.RuntimeIdentity.operator !=(Inno.Core.Identity.RuntimeIdentity left, Inno.Core.Identity.RuntimeIdentity right)`](../../src/foundation/core/Inno.Core.Identity/RuntimeIdentity.cs#L114) | Determines whether two runtime identities are different.<br>返回：true when the values are different; otherwise, false.<br>`left`：The first runtime identity.<br>`right`：The second runtime identity. | — | — |
| <a id="api-7dfbf326bc5c8b7b"></a>[`public override string Inno.Core.Identity.RuntimeIdentity.ToString()`](../../src/foundation/core/Inno.Core.Identity/RuntimeIdentity.cs#L83) | Formats this transient identity for diagnostics.<br>返回：A domain-qualified runtime identifier. | — | — |
| <a id="api-ccc79c3482e5fc01"></a>[`public Inno.Core.Identity.IdentityDomainId Inno.Core.Identity.RuntimeIdentity.domainId { get; }`](../../src/foundation/core/Inno.Core.Identity/RuntimeIdentity.cs#L40) | Gets the identity domain that owns this runtime identifier. | — | — |
| <a id="api-292ff9b836cea786"></a>[`public int Inno.Core.Identity.RuntimeIdentity.runtimeId { get; }`](../../src/foundation/core/Inno.Core.Identity/RuntimeIdentity.cs#L45) | Gets the generation-safe identifier interpreted inside Inno.Core.Identity.RuntimeIdentity.domainId. | — | — |
| <a id="api-d0a8ebd12dfc6ab9"></a>[`public readonly struct Inno.Core.Identity.RuntimeIdentity : System.IEquatable<Inno.Core.Identity.RuntimeIdentity>`](../../src/foundation/core/Inno.Core.Identity/RuntimeIdentity.cs#L11) | Addresses a live identity object within one explicitly identified runtime domain. | This value is suitable for transient interaction payloads only and must never be persisted. | — |

## 项目依赖

- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Execution](Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
