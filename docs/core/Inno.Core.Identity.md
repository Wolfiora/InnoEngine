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
public sealed class RuntimeResource : IdentityObject
{
}

RuntimeResource resource = new();
Guid persistentId = resource.identity.persistentId;
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
var identities = new IdentityAllocator();

RuntimeResource resource = new();
identities.InitializePersistentIdentity(resource, savedId);
identities.Register(resource);

int runtimeId = resource.identity.runtimeId!.Value;
RuntimeResource? same = identities.Get<RuntimeResource>(runtimeId);

identities.Unregister(resource);
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

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Core.Identity.Identity`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Identity.Identity`](../../src/foundation/core/Inno.Core.Identity/Identity.cs#L8) | Identity payload for an object, carrying persistent id and runtime id. |
| [`Inno.Core.Identity.Identity.Identity(System.Guid persistentId)`](../../src/foundation/core/Inno.Core.Identity/Identity.cs#L59) | Creates an unbound identity with the supplied persistent identifier. |
| [`Inno.Core.Identity.RuntimeIdentity? Inno.Core.Identity.Identity.runtimeIdentity`](../../src/foundation/core/Inno.Core.Identity/Identity.cs#L38) | Gets the domain-qualified runtime identity while this object is registered. |
| [`System.Guid Inno.Core.Identity.Identity.persistentId`](../../src/foundation/core/Inno.Core.Identity/Identity.cs#L13) | Gets the stable identifier preserved by serialization and runtime reconstruction. |
| [`TObject? Inno.Core.Identity.Identity.Resolve<TObject>()`](../../src/foundation/core/Inno.Core.Identity/Identity.cs#L99) | Resolves this transient snapshot through its original weak identity registry. |
| [`int? Inno.Core.Identity.Identity.runtimeId`](../../src/foundation/core/Inno.Core.Identity/Identity.cs#L21) | Gets the identifier assigned by the currently bound runtime registry. |

### `Inno.Core.Identity.IdentityAllocator`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Identity.IdentityAllocator`](../../src/foundation/core/Inno.Core.Identity/IdentityAllocator.cs#L11) | Owns the persistent-to-runtime identity map for one isolated runtime session. |
| [`Inno.Core.Identity.IdentityAllocator.IdentityAllocator()`](../../src/foundation/core/Inno.Core.Identity/IdentityAllocator.cs#L24) | Creates an isolated runtime identity domain. |
| [`Inno.Core.Identity.IdentityDomainId Inno.Core.Identity.IdentityAllocator.domainId`](../../src/foundation/core/Inno.Core.Identity/IdentityAllocator.cs#L54) | Gets the process-unique domain that qualifies every runtime identity allocated here. |
| [`System.Action<Inno.Core.Identity.IdentityObject>? Inno.Core.Identity.IdentityAllocator.ObjectUnregistered`](../../src/foundation/core/Inno.Core.Identity/IdentityAllocator.cs#L63) | Occurs after an object has been permanently removed from this allocator. |
| [`System.IDisposable Inno.Core.Identity.IdentityAllocator.EnterScope()`](../../src/foundation/core/Inno.Core.Identity/IdentityAllocator.cs#L74) | Binds this allocator to the current asynchronous execution context until the returned scope is disposed. |
| [`TIdentity? Inno.Core.Identity.IdentityAllocator.Get<TIdentity>(Inno.Core.Identity.RuntimeIdentity identity)`](../../src/foundation/core/Inno.Core.Identity/IdentityAllocator.cs#L192) | Resolves a domain-qualified runtime identity without allowing cross-session aliasing. |
| [`TIdentity? Inno.Core.Identity.IdentityAllocator.Get<TIdentity>(System.Guid persistentId)`](../../src/foundation/core/Inno.Core.Identity/IdentityAllocator.cs#L210) | Resolves a registered object by its persistent identity. |
| [`TIdentity? Inno.Core.Identity.IdentityAllocator.Get<TIdentity>(int runtimeId)`](../../src/foundation/core/Inno.Core.Identity/IdentityAllocator.cs#L176) | Resolves a registered object by its session-local runtime identity. |
| [`bool Inno.Core.Identity.IdentityAllocator.Register(Inno.Core.Identity.IdentityObject obj, System.Guid? persistentId = null)`](../../src/foundation/core/Inno.Core.Identity/IdentityAllocator.cs#L91) | Registers an object and assigns its session-local runtime identity. |
| [`bool Inno.Core.Identity.IdentityAllocator.Unregister(Inno.Core.Identity.IdentityObject obj)`](../../src/foundation/core/Inno.Core.Identity/IdentityAllocator.cs#L138) | Removes an object from this session and notifies every unregistration observer. |
| [`int Inno.Core.Identity.IdentityAllocator.count`](../../src/foundation/core/Inno.Core.Identity/IdentityAllocator.cs#L49) | Gets the number of currently registered live identity objects. |
| [`static Inno.Core.Identity.IdentityAllocator Inno.Core.Identity.IdentityAllocator.current`](../../src/foundation/core/Inno.Core.Identity/IdentityAllocator.cs#L44) | Gets the allocator bound to the current asynchronous execution context. |
| [`static bool Inno.Core.Identity.IdentityAllocator.hasCurrent`](../../src/foundation/core/Inno.Core.Identity/IdentityAllocator.cs#L36) | Gets whether the current asynchronous execution context is bound to an allocator. |
| [`void Inno.Core.Identity.IdentityAllocator.InitializePersistentIdentity(Inno.Core.Identity.IdentityObject obj, System.Guid persistentId)`](../../src/foundation/core/Inno.Core.Identity/IdentityAllocator.cs#L114) | Assigns a persistent identity to a detached object without allocating a runtime identity. |

### `Inno.Core.Identity.IdentityDomainId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Identity.IdentityDomainId`](../../src/foundation/core/Inno.Core.Identity/IdentityDomainId.cs#L11) | Identifies one isolated runtime identity namespace within the current process. |
| [`bool Inno.Core.Identity.IdentityDomainId.Equals(Inno.Core.Identity.IdentityDomainId other)`](../../src/foundation/core/Inno.Core.Identity/IdentityDomainId.cs#L37) | Determines whether this value and another value identify the same runtime domain. |
| [`bool Inno.Core.Identity.IdentityDomainId.isValid`](../../src/foundation/core/Inno.Core.Identity/IdentityDomainId.cs#L26) | Gets whether this value identifies an allocated runtime domain. |
| [`int Inno.Core.Identity.IdentityDomainId.value`](../../src/foundation/core/Inno.Core.Identity/IdentityDomainId.cs#L21) | Gets the process-local numeric value assigned to this domain. |
| [`override bool Inno.Core.Identity.IdentityDomainId.Equals(object? obj)`](../../src/foundation/core/Inno.Core.Identity/IdentityDomainId.cs#L48) | Determines whether an object represents the same runtime domain. |
| [`override int Inno.Core.Identity.IdentityDomainId.GetHashCode()`](../../src/foundation/core/Inno.Core.Identity/IdentityDomainId.cs#L56) | Computes a hash code for this runtime domain identifier. |
| [`override string Inno.Core.Identity.IdentityDomainId.ToString()`](../../src/foundation/core/Inno.Core.Identity/IdentityDomainId.cs#L64) | Formats this runtime domain identifier for diagnostics. |
| [`static bool Inno.Core.Identity.IdentityDomainId.operator !=(Inno.Core.Identity.IdentityDomainId left, Inno.Core.Identity.IdentityDomainId right)`](../../src/foundation/core/Inno.Core.Identity/IdentityDomainId.cs#L95) | Determines whether two values identify different runtime domains. |
| [`static bool Inno.Core.Identity.IdentityDomainId.operator ==(Inno.Core.Identity.IdentityDomainId left, Inno.Core.Identity.IdentityDomainId right)`](../../src/foundation/core/Inno.Core.Identity/IdentityDomainId.cs#L78) | Determines whether two values identify the same runtime domain. |

### `Inno.Core.Identity.IdentityObject`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Identity.Identity Inno.Core.Identity.IdentityObject.identity`](../../src/foundation/core/Inno.Core.Identity/IdentityObject.cs#L13) | Gets this object's current persistent and optional session-local identity snapshot. |
| [`Inno.Core.Identity.IdentityObject`](../../src/foundation/core/Inno.Core.Identity/IdentityObject.cs#L6) | Provides instance-owned persistent and session-local identity state managed by an . |

### `Inno.Core.Identity.RuntimeIdentity`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Identity.IdentityDomainId Inno.Core.Identity.RuntimeIdentity.domainId`](../../src/foundation/core/Inno.Core.Identity/RuntimeIdentity.cs#L40) | Gets the identity domain that owns this runtime identifier. |
| [`Inno.Core.Identity.RuntimeIdentity`](../../src/foundation/core/Inno.Core.Identity/RuntimeIdentity.cs#L11) | Addresses a live identity object within one explicitly identified runtime domain. |
| [`Inno.Core.Identity.RuntimeIdentity.RuntimeIdentity(Inno.Core.Identity.IdentityDomainId domainId, int runtimeId)`](../../src/foundation/core/Inno.Core.Identity/RuntimeIdentity.cs#L25) | Creates a runtime identity from its domain and generation-safe object identifier. |
| [`bool Inno.Core.Identity.RuntimeIdentity.Equals(Inno.Core.Identity.RuntimeIdentity other)`](../../src/foundation/core/Inno.Core.Identity/RuntimeIdentity.cs#L56) | Determines whether this value and another value address the same live identity slot. |
| [`int Inno.Core.Identity.RuntimeIdentity.runtimeId`](../../src/foundation/core/Inno.Core.Identity/RuntimeIdentity.cs#L45) | Gets the generation-safe identifier interpreted inside . |
| [`override bool Inno.Core.Identity.RuntimeIdentity.Equals(object? obj)`](../../src/foundation/core/Inno.Core.Identity/RuntimeIdentity.cs#L67) | Determines whether an object addresses the same live identity slot. |
| [`override int Inno.Core.Identity.RuntimeIdentity.GetHashCode()`](../../src/foundation/core/Inno.Core.Identity/RuntimeIdentity.cs#L75) | Computes a hash code from the domain and runtime identifier. |
| [`override string Inno.Core.Identity.RuntimeIdentity.ToString()`](../../src/foundation/core/Inno.Core.Identity/RuntimeIdentity.cs#L83) | Formats this transient identity for diagnostics. |
| [`static bool Inno.Core.Identity.RuntimeIdentity.operator !=(Inno.Core.Identity.RuntimeIdentity left, Inno.Core.Identity.RuntimeIdentity right)`](../../src/foundation/core/Inno.Core.Identity/RuntimeIdentity.cs#L114) | Determines whether two runtime identities are different. |
| [`static bool Inno.Core.Identity.RuntimeIdentity.operator ==(Inno.Core.Identity.RuntimeIdentity left, Inno.Core.Identity.RuntimeIdentity right)`](../../src/foundation/core/Inno.Core.Identity/RuntimeIdentity.cs#L97) | Determines whether two runtime identities are equal. |

## 项目依赖

- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Execution](Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
