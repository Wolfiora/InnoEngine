# Inno.Extensibility.Types

[上一页：Modules](Inno.Extensibility.Modules.md) · [Extensibility 索引](README.md) · [Serialization](../core/Inno.Core.Serialization.md)

## 组合资源退休

TypeRegistry 新增 protected `ReportRetirementFailure(Exception)`：供派生 registry 报告由该代际创建、但在领域组合中持有的资源退休失败。它将共享 gate 置为 Faulted，并调用诊断 hook；不是吞异常的 fallback。调用方仍须向上传播清理失败。

Catalog participant 在同一个 publication 中重入 Rebuild 时合并为后续对账；不允许借此越过其他事务、读租约或 GC barrier。

Registry 现在通过 Core `LifetimeScope` / `RetirementBarrier` 逆序退休实例。普通错误聚合；Pending 先排空，
不能跳到下一项释放其依赖。达到 deadline 时保留失败 batch/snapshot/transaction，Fault 共享 gate；
重复 Clear/Dispose/Refresh 仍抛同一未退休失败，不清空字段伪装成功。这些强引用仅属于 Faulted Host 的
未完成退休所有权，不是可持久化状态或下一 generation 的缓存。
Pending/timeout 在 Aggregate 或普通 InnerException 中也相同处理，统一使用 Core 分类。
Registry、TypeCatalog、ModuleHost 保留原始外层异常与 owner，而非仅保存抽出的 Pending 子异常。

`Inno.Extensibility.Types` 把当前活动 Assembly catalog 转换成不可变的类型快照，并提供统一查询、持久/运行时类型身份和可事务刷新的 `TypeRegistry<TSnapshot>`。所有面向使用者的类型查询都位于公开的 `TypeCatalog`。

## 初始化关系

```csharp
using System;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;

static void UseTypeCatalog(
    IAssemblyCatalogSource ownedAssemblySource,
    ITypeCatalogSource borrowedTypeSource,
    Action<TypeCatalog> consume
) {
    using var modules = new ModuleHost(new ModuleHostOptions
    {
        catalogSource = ownedAssemblySource
    });
    using var types = new TypeCatalog(modules, borrowedTypeSource);
    consume(types);
}
```

Types 引用中立 Modules 并注册一个 catalog participant。类型事实由组合入口注入的元数据来源提供；Modules 不反向引用 Types 或 Adapter，也不存在 `InternalsVisibleTo` 耦合。

## TypeCatalog

| 成员 | 说明 |
| --- | --- |
| `bool isInitialized` | TypeCache participant 已注册且 ModuleHost 仍有效。 |
| `TypeCacheSnapshot current` | 当前不可变快照；读取前会先处理 dirty Host catalog。 |
| `TypeCatalog(ModuleHost, ITypeCatalogSource)` | 借用已启动的模块宿主和明确的元数据来源，注册类型/Registry 统一事务参与者。 |
| `Rebuild()` | 通过 ModuleHost 强制重建 assembly、type 与 registry 快照。 |
| `AcquireOperation(string)` | 同步领域操作期间保护捕获的 generation，延后自动刷新；发布线程内部可借用，其他发布并发访问拒绝。 |
| `Dispose()` | 注销 participant 并释放 Registry 状态。 |
| `GetSubTypesOf<T>()` | 返回所有具体派生类型。 |
| `GetTypesImplementing<TInterface>()` | 返回所有具体接口实现。 |
| `GetTypesWithAttribute<TAttribute>()` | 返回所有带指定 attribute 的具体类型。 |
| `GetTypeRef(Type)` | 把当前 CLR Type 转换为不持有 ALC 的 `TypeRef`。 |
| `TryGetTypeRef(Type, out TypeRef)` | 安全尝试同一转换。 |
| `TryResolve(TypeRef, out Type?)` / `Resolve(TypeRef)` | 对当前 generation 解析；前者返回可用性，后者在缺失时抛异常。 |

查询结果只包含非抽象、非接口的匹配实现，并按照 catalog 的稳定顺序输出。

跨多次查询或修改捕获的领域 owner 时，应持有 `AcquireOperation` 到整个操作和补偿结束。
它直接复用 ModuleHost 的 GenerationCoordinator，不触发对账；需要最新 Catalog 时先读取 `current`，
再捕获 owner 和建立 scope。旧 ALC 等待回收时允许读取已发布数据，不因此放开 Play/Build/Export；
异步 Build/Export 仍使用严格的 `GenerationCoordinator.AcquireRead` 并持有到消费者退出。

```csharp
using System;
using System.Collections.Generic;
using Inno.Extensibility.Types;

static IReadOnlyList<TypeRef> FindImplementations<T>(TypeCatalog types)
{
    return types.GetSubTypesOf<T>();
}

static string GetCurrentTypeName(
    TypeCatalog types,
    TypeRef reference
) {
    return reference.Resolve(types).FullName ?? throw new InvalidOperationException("The type has no full name.");
}
```

## TypeRef

`TypeRef` 是公开 readonly value type，只含 `Guid stableId` 与 `int runtimeId`。公开构造器只接收 Stable ID；TypeCache 生成的值才带 runtime hint。`IsValid(TypeCatalog)` 针对指定 catalog，`Resolve(TypeCatalog)` 无法解析时抛 `InvalidOperationException`，`Resolve(snapshot)` 可在事务中分别解析 previous/candidate。

相等性和 HashCode 只看 `stableId`，所以同一逻辑类型跨 generation 仍相等；`runtimeId` 只是进程内不复用的快速查找 hint。解析会验证 runtime 命中的 Stable ID，hint 过期或命中不符时回退到 Stable ID。`default(TypeRef)` 与空 Guid 无效。统一 Serialization converter 只写 `stableId`，绝不持久化 `runtimeId`/`isValid`。

`TypeRef` 不保存 `Type`、`Assembly`、delegate 或 ALC，也没有进入 Scripting API export。长期集合可以安全保存它；外部保存 `Resolve()` 返回的 CLR `Type`/对象/委托仍会按 .NET 规则延迟旧 ALC 卸载。

## TypeCacheSnapshot

快照可用于一次多查询需要严格一致版本的场景：

| 成员 | 说明 |
| --- | --- |
| `long version` | 单调递增版本号。 |
| `IReadOnlyList<TypeRef> types` | 本 generation 的全部已发现类型身份。 |
| `GetSubTypesOf<T>()` | 快照内的具体派生类。 |
| `GetTypesImplementing<TInterface>()` | 快照内的具体接口实现。 |
| `GetTypesWithAttribute<TAttribute>()` | 快照内带 attribute 的类型。 |
| `GetTypeRef(Type)` / `TryGetTypeRef` | 把属于该快照的 CLR Type 转成 `TypeRef`。 |
| `CanCreateInstance(TypeRef)` | 判断当前部署是否提供该声明的无参构造工厂；不存在的类型返回 false。 |
| `CreateInstance(Type)` | 通过所属来源构造声明或其封闭泛型；返回的新实例由调用者拥有。 |
| `ConstructGenericType(Type, IReadOnlyList<Type>)` | 在本 generation 内解析封闭泛型；约束不成立返回 null，静态部署未链接该构造时明确抛异常。 |
| `GetMetadata(TypeRef)` | 读取本 generation 的不可变发现事实，缺失或过期引用在解析时失败。 |
| `GetAttribute<TAttribute>(TypeRef, bool inherit = true)` | 从已提供的元数据中取得单一 attribute；缺失返回 null，重复声明失败。 |

## ITypeCatalogSource

来源提供 `GetTypes(Assembly)`、`GetMetadata(Type)`、`ConstructGenericType(definition, arguments)`、
`CanCreateInstance(Type)` 和 `CreateInstance(Type)`。这些行为必须对应同一个模块闭包与 generation。
动态来源通过反射取得事实；静态来源消费程序集生成目录。Types 不根据操作系统选择来源，也不隐式补做动态反射。
基础元数据与生成 registrar 契约属于 [Inno.Extensibility.Catalogs](Inno.Extensibility.Catalogs.md)。

不要长期缓存旧 snapshot：其内部为了 generation 一致性强持有 `Type` 和反射发现 slice，即使公开查询只返回 `TypeRef`。Registry 在 `Complete/Rollback` 中及时释放旧快照；外部调用方若自行保留旧 snapshot 或 `Resolve` 的结果，则 ALC 延迟卸载属于该引用的预期结果。

Snapshot 构建会按 `Assembly` 引用身份复用上一代的内部 Type slice：未变化的 host/default/upstream assembly 不再重复调用 `GetTypes()`，新加载或新 ALC 中的 assembly 才重新发现。该优化不会跨 ALC 复用脚本 `Type`；即使程序集字节来自增量缓存，被替换的 Plugin/Runtime/Editor ALC 中 `Assembly` 引用也不同，因此旧 slice 会随 previous snapshot 一起释放。Snapshot 本身必须保持强引用才能提供 generation 内一致性；把其中的 `Type` 改成弱引用会让一次查询中类型集合随 GC 变化，不能解决外部强引用，反而破坏事务语义。

## Stable Type ID 与 Runtime Type ID

普通 Host/Plugin 类型在没有 attribute 时，Stable ID 由 `程序集简单名 + 完整类型名` 生成确定性 UUIDv5。脚本编译器可以通过 assembly metadata 提供 source-based canonical ID；TypeCache 只验证并消费当前映射，不依赖 Asset/Scripting 项目。类型级 `[StableTypeId]` 始终优先于编译器映射。

需要让语义身份独立于类型名称时显式固定：

```csharp
using Inno.Extensibility.Types;

[StableTypeId("c5db9123-9768-4e34-a346-22981ee4b4da")]
public sealed class ToolConfiguration
{
}
```

`StableTypeIdAttribute.id` 是 Guid 字符串。无效字符串或重复 ID 会让候选快照验证失败，从而保留旧 generation。

`GetTypeRef` 返回唯一 canonical ID；`TypeRef.Resolve` 只接受当前明确注册的 ID，不维护 former alias 或旧持久数据兼容表。

Runtime Type ID 是只适用于某个 CLR `Type` 实例的整数。新 ALC 里的替代 Type 会获得新的 runtime ID；失败/回滚候选已分配的 ID 也不会复用。它在 `TypeIdentityRegistry`、TypeCache query index，以及明确绑定当前 TypeCache generation 的 Scene Component/System 内存索引与查询缓存中使用。`TypeRef.Resolve` 也把它作为可选快速 hint。Asset、History、Workspace、Editor action、Missing、序列化和 reload 边界继续保存 `TypeRef`，不得让裸 runtime ID 跨 generation 或持久化。

## TypeCacheReloadContext

程序集 Reload session 可取得这个上下文，用来迁移旧实例：

| 成员 | 说明 |
| --- | --- |
| `previous` | 旧类型快照。 |
| `candidate` | 已验证的候选类型快照。 |
| `IsRetired(TypeRef)` | 逻辑类型是否在候选中被替换或移除。 |
| `TryResolveReplacement(TypeRef, out TypeRef)` | 按 Stable ID 查找不同的候选 generation identity。 |

上下文只在 reload transaction 存活；`Complete()` 或 `Rollback()` 后访问会抛 `InvalidOperationException`。

## TypeRegistry&lt;TSnapshot&gt;

清理失败不再只是日志。Complete 会尝试全部旧 snapshot 清理，然后抛出聚合异常并把所属
GenerationCoordinator 置为 Faulted；已提交 candidate 不伪回滚，Host 必须重启。
Rollback 同样尝试全部已准备项，任何恢复/释放失败都不得报告成功。Build 过程中创建的资源在验证失败时统一逆序释放；成功后由 snapshot 接管，不能在派生 Build catch 中重复释放。
OnCleanupFailed 仅用于 override 观测，
不能吞掉真实失败或批准继续 generation 操作。

这是 Importer、Serialization Converter、Inspector Drawer 等扩展点的通用基类。它在候选 TypeCache 上构建完整不可变索引，与 TypeCache 一起原子 activate/rollback。

| 成员 | 可见性 | 说明 |
| --- | --- | --- |
| `TypeRegistry(TypeCatalog, TimeSpan? retirementTimeout = null)` | `protected` | 自动弱注册；退休 deadline 为正值，默认 30 秒，在首次退出尝试时绑定 owner thread。 |
| `isInitialized` | `public` | 是否已有活动快照。 |
| `Refresh()` | `public` | 从当前 TypeCache 主动刷新；相同版本不会重复构建。若 TypeCache 在激活回调期间变化，完成当前 transaction 后再以独立 transaction 追平。 |
| `Clear()` | `public` | 释放快照但保留 Registry，可在下次访问重建。 |
| `Dispose()` | `public` | 注销并释放，之后不能再用。 |
| `current` | `protected` | 懒取得当前快照；版本过期时自动刷新。 |
| `Build(TypeCacheSnapshot)` | `protected abstract` | 旁路验证并构建完整候选快照。 |
| `OnActivating(previous, candidate)` | `protected virtual` | candidate 已临时发布后的可失败激活；实现必须准备对应回滚。 |
| `OnActivationRolledBack(previous, candidate)` | `protected virtual` | 激活失败后逆转已完成的生命周期工作。 |
| `OnActivationCompleted(previous, current)` | `protected virtual` | 全部 Registry 激活成功后的 cleanup-only 阶段；失败必须报告 Fault，不能再进行发布。 |
| `DisposeSnapshot(snapshot)` | `protected virtual` | 默认对实现 `IDisposable` 的 snapshot 调用 `Dispose()`。 |
| `OnCleanupFailed(phase, exception)` | `protected virtual` | 报告 rollback/complete/snapshot release 清理异常；不会重新进入回滚。 |
| `CreateExtension<TExtension>(Type)` | `protected` | 验证并创建实例，自动登记 candidate rollback ownership。 |
| `OwnCandidateExtension<T>(T)` | `protected` | 将自定义工厂创建的新实例纳入 Build 失败补偿；不能传入借用的旧实例。 |
| `DisposeExtensions(IEnumerable<object>)` | `protected` | 按引用去重后交由 LifetimeScope 逆序退休；Pending 保留本项及以下依赖，普通失败仍尝试后续项。 |
| `ReportRetirementFailure(Exception)` | `protected` | 组合资源无法退休时封锁共享 generation，并通知诊断。 |
| `RetireResource(string owner, Action retire)` | `protected` | 在 control-thread safe point 有界排空单个可重试 Stop/Detach；正常完成不保存 delegate，超时保留未完成 owner 并 Fault。不是实时 callback API。 |

### 自定义 Registry 示例

```csharp
using System;
using System.Collections.Generic;
using System.Collections.Frozen;
using Inno.Extensibility.Types;

internal abstract class ExampleExtension : IDisposable
{
    public abstract string id { get; }
    public abstract void Dispose();
}

internal sealed class ExampleRegistry(TypeCatalog types)
    : TypeRegistry<FrozenDictionary<string, ExampleExtension>>(types)
{
    protected override FrozenDictionary<string, ExampleExtension> Build(TypeCacheSnapshot snapshot)
    {
        var entries = new Dictionary<string, ExampleExtension>(StringComparer.Ordinal);
        foreach (TypeRef typeRef in snapshot.GetSubTypesOf<ExampleExtension>())
        {
            ExampleExtension entry = CreateExtension<ExampleExtension>(typeRef.Resolve(snapshot));
            if (!entries.TryAdd(entry.id, entry))
                throw new InvalidOperationException($"Duplicate extension ID: {entry.id}");
        }
        return entries.ToFrozenDictionary(StringComparer.Ordinal);
    }

    protected override void DisposeSnapshot(FrozenDictionary<string, ExampleExtension> snapshot)
        => DisposeExtensions(snapshot.Values);
}
```

新增这种 Registry 不需要修改 `ModuleHost` 或添加全局 Hook。协调器执行 Build candidate → reversible Activate → global Complete；后一个 Registry 激活失败时，已激活项按逆序恢复 `m_current` 与 TypeCache version，candidate 被释放，previous snapshot 保留。Complete 只做 previous release 等清理，异常会逐项报告并继续，不会制造“已发布后伪回滚”。完成后不会再执行 pending Registry refresh；重入排队的 rebuild 必须在外层全局 transaction 完成后作为独立 transaction 运行，失败只回滚自己的 candidate。候选构造/冲突验证失败同样保留当前 Registry。

## 错误模型

`TypeCacheBuildException` 表示枚举程序集类型时发生一个或多个 loader error：

- `loaderExceptions`：每个底层异常。
- `InnerException`：第一个 loader exception（若存在）。

TypeCache 不再静默吞掉 `ReflectionTypeLoadException`。这对热重载很重要：缺依赖的候选不能部分进入全局查询。

## 热重载注意事项

- 查询永远只看当前活动 generation；旧 ALC 即使尚未被 GC，也不会重新出现在 TypeCache。
- 同一程序集名与完整类型名通常保持 Stable ID，但运行时 `Type` 对象不是同一个。
- Registry snapshot 不应跨代际缓存旧 `Type`、delegate 或 extension instance。
- `TypeCatalog.Rebuild()` 适合“活动程序集集合未重新载入，但需要重算类型/Registry”的场景；读取新 DLL 仍由 Assemblies 的 Reload API 完成。

## 显式类型元数据来源

`ITypeCatalogSource.GetTypes(Assembly)`、`GetMetadata(Type)`、`ConstructGenericType(Type, IReadOnlyList<Type>)`、`CanCreateInstance(Type)` 和 `CreateInstance(Type)` 是动态反射与编译目录共用的边界。`TypeCatalog(ModuleHost, ITypeCatalogSource)` 接收组合入口选择的来源；Types 不调用 Assembly.GetTypes、MakeGenericType 或负责 ALC。

[`TypeCatalogMetadata`](Inno.Extensibility.Catalogs.md) 属于独立的 Catalogs 契约项目，构造时防御性复制 `type / baseTypes / interfaces / declaredAttributes / inheritedAttributes / parameterlessOverrides`。TypeCacheSnapshot 的 `GetMetadata(TypeRef)`、`GetAttribute<TAttribute>(TypeRef, inherit=true)`、`CanCreateInstance(TypeRef)`、`ConstructGenericType(Type, IReadOnlyList<Type>)` 与 `CreateInstance(Type)` 将 registry 查询和构造统一到当前 generation。过期 TypeRef 或缺失 factory 明确失败。无返回语义的 marker 不作为发现条件；Attribute 只携带真实元数据。

Reflection 来源属于 [DotNet Adapter](../backends/DotNet/Inno.Adapter.Modules.DotNet.md)。静态来源属于 [Runtime](../runtime/Inno.Runtime.md)，通过 generated catalogs 提供相同事实。旧 snapshot 清理时释放 source、metadata、Type 和 factory 的 generation 引用。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Extensibility.Types.ITypeCatalogSource`

<!-- api: T:Inno.Extensibility.Types.ITypeCatalogSource 3f9feed2499dc9e265d499208333b02246456241d75545c1ce15f2047c175626 -->
<!-- api: M:Inno.Extensibility.Types.ITypeCatalogSource.GetTypes(System.Reflection.Assembly) d311ccd3e8f6365908e08872e487492c0f62f53c92b476d1724fcb4049f32cd6 -->
<!-- api: M:Inno.Extensibility.Types.ITypeCatalogSource.GetMetadata(System.Type) 668a7d219fc296ec0acd092a5b003fd033ba7dadaa8745a6344faaee1e877d0c -->
<!-- api: M:Inno.Extensibility.Types.ITypeCatalogSource.ConstructGenericType(System.Type,System.Collections.Generic.IReadOnlyList{System.Type}) 7672376bd592a87421bc592b18d4f2a48111ae1b3982a83d94343df3e9691534 -->
<!-- api: M:Inno.Extensibility.Types.ITypeCatalogSource.CanCreateInstance(System.Type) d82fcc2a0878a52f43f15d5d29f05e909bab2e702202c26015ac1675ed4eaa95 -->
<!-- api: M:Inno.Extensibility.Types.ITypeCatalogSource.CreateInstance(System.Type) 2c02ac2f404a76872da660fd51ca19d24d5a0aeba056dd0b5f5145b2181faee7 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-0fb0b51a368e8c6d"></a>[`bool Inno.Extensibility.Types.ITypeCatalogSource.CanCreateInstance(System.Type type)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/ITypeCatalogSource.cs#L68) | Determines whether this deployment supplies a parameterless factory for a declared type.<br>返回：Whether instance construction is supported by the selected deployment.<br>`type`：The declared runtime type. | — | — |
| <a id="api-5bd48147c0f92bc1"></a>[`System.Type? Inno.Extensibility.Types.ITypeCatalogSource.ConstructGenericType(System.Type definition, System.Collections.Generic.IReadOnlyList<System.Type> arguments)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/ITypeCatalogSource.cs#L54) | Resolves a generic construction through the deployment's linked or dynamic type mechanism.<br>返回：The closed type, or null when its generic constraints reject these arguments.<br>`definition`：A generic declaration owned by the candidate generation.<br>`arguments`：The complete ordered set of closed type arguments. | — | `System.NotSupportedException`：A static deployment did not link the requested construction.<br>`System.ArgumentException`：The declaration is not generic, or the argument count or closedness is invalid. |
| <a id="api-74ef63507286056f"></a>[`object Inno.Extensibility.Types.ITypeCatalogSource.CreateInstance(System.Type type)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/ITypeCatalogSource.cs#L79) | Constructs a declared type through the selected deployment's factory mechanism.<br>返回：A new instance owned by the caller; missing factories or failed constructors throw.<br>`type`：The declared runtime type. | — | — |
| <a id="api-0be779d0335aa53d"></a>[`Inno.Extensibility.Catalogs.TypeCatalogMetadata Inno.Extensibility.Types.ITypeCatalogSource.GetMetadata(System.Type type)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/ITypeCatalogSource.cs#L34) | Resolves immutable discovery facts for a declaration in this source.<br>返回：Complete metadata owned by the caller's candidate generation; unavailable declarations throw.<br>`type`：The declaration whose relationships and attributes are required. | — | — |
| <a id="api-182aec19a607c355"></a>[`System.Collections.Generic.IReadOnlyList<System.Type> Inno.Extensibility.Types.ITypeCatalogSource.GetTypes(System.Reflection.Assembly assembly)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/ITypeCatalogSource.cs#L23) | Resolves the complete type set without publishing a partial generation.<br>返回：The complete type metadata for this assembly; unsupported or invalid metadata throws.<br>`assembly`：An assembly owned by the candidate module catalog. | — | — |
| <a id="api-12e9d79b81828ed4"></a>[`public interface Inno.Extensibility.Types.ITypeCatalogSource`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/ITypeCatalogSource.cs#L12) | Provides type metadata for an explicit assembly in a candidate module generation. | — | — |

### `Inno.Extensibility.Types.StableTypeIdAttribute`

<!-- api: T:Inno.Extensibility.Types.StableTypeIdAttribute 77024b1a955768e995143a84db264bacea022ddee27b6b88cbec67e923a9ecec -->
<!-- api: M:Inno.Extensibility.Types.StableTypeIdAttribute.#ctor(System.String) 02d449bd62d0b396b28aea78357cc6f1e626070bede8df7c5421793972796309 -->
<!-- api: P:Inno.Extensibility.Types.StableTypeIdAttribute.id 5f176dfcc77b6d7b0205ae7c32bf713aacc9b6e4df50446dca112a0de2823446 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d7e3d3effd552fc1"></a>[`public Inno.Extensibility.Types.StableTypeIdAttribute.StableTypeIdAttribute(string id)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/Attributes/StableTypeIdAttribute.cs#L12) | Defines a stable identity for a type, used by persistence and hot-reload remapping.<br>`id`：The stable identity used to locate the requested value. | — | — |
| <a id="api-19b85461c8770e0e"></a>[`public string Inno.Extensibility.Types.StableTypeIdAttribute.id { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/Attributes/StableTypeIdAttribute.cs#L17) | Stable type id as Guid string. | — | — |
| <a id="api-651172b63fe8fe28"></a>[`public sealed class Inno.Extensibility.Types.StableTypeIdAttribute : System.Attribute`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/Attributes/StableTypeIdAttribute.cs#L12) | Defines a stable identity for a type, used by persistence and hot-reload remapping.<br>`id`：The stable identity used to locate the requested value. | — | — |

### `Inno.Extensibility.Types.TypeCacheBuildException`

<!-- api: T:Inno.Extensibility.Types.TypeCacheBuildException 75dd7e924f577ceb4973fe1b0c9b3d4fff03529dfb193539d3e6f6e229a5a2cf -->
<!-- api: M:Inno.Extensibility.Types.TypeCacheBuildException.#ctor(System.String,System.Collections.Generic.IReadOnlyList{System.Exception}) bd220ae0d05034963dc14467573b2572a0416c47eaa96bc7226f9da8a2f6384a -->
<!-- api: P:Inno.Extensibility.Types.TypeCacheBuildException.loaderExceptions a2640e9510e8454654bd685840d45e43f12a992b86b2adb1f260a43dd70623da -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-345abf404d40e874"></a>[`public Inno.Extensibility.Types.TypeCacheBuildException.TypeCacheBuildException(string message, System.Collections.Generic.IReadOnlyList<System.Exception> loaderExceptions)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCacheBuildException.cs#L20) | Creates an exception for a failed type-cache snapshot build.<br>`message`：A summary of the build failure.<br>`loaderExceptions`：The underlying type-loader failures. | — | — |
| <a id="api-847b3aa4d26ef3af"></a>[`public System.Collections.Generic.IReadOnlyList<System.Exception> Inno.Extensibility.Types.TypeCacheBuildException.loaderExceptions { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCacheBuildException.cs#L32) | Gets the individual type-loader failures. | — | — |
| <a id="api-7e0166142ae9cf5e"></a>[`public sealed class Inno.Extensibility.Types.TypeCacheBuildException : System.Exception`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCacheBuildException.cs#L9) | Reports one or more failures encountered while building a type-cache snapshot. | — | — |

### `Inno.Extensibility.Types.TypeCacheReloadContext`

<!-- api: T:Inno.Extensibility.Types.TypeCacheReloadContext cb02f0fe43d87f722f2f489ea299e8103cba71f6de44a4968ab5dbb4628c516e -->
<!-- api: P:Inno.Extensibility.Types.TypeCacheReloadContext.previous 77c615ee54e8ecc4cca93b563fd5a5f54ac2a343f3ebcc542a60fffc7d307c00 -->
<!-- api: P:Inno.Extensibility.Types.TypeCacheReloadContext.candidate 2519e00502b2b559d109c830ba6d92a775836c57f8ff2ae939d85d582e0f4688 -->
<!-- api: M:Inno.Extensibility.Types.TypeCacheReloadContext.IsRetired(Inno.Extensibility.Types.TypeRef) 36e567dd8f52e2181495ff4cea22b0e5af69a778861a499d4892973fbdd70bd5 -->
<!-- api: M:Inno.Extensibility.Types.TypeCacheReloadContext.TryResolveReplacement(Inno.Extensibility.Types.TypeRef,Inno.Extensibility.Types.TypeRef@) 9dd59a2538dbb22dc08224f98f002dd7bb36b77d31f9456d4f1cc9bbb9527457 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-970d36570baaa10a"></a>[`public bool Inno.Extensibility.Types.TypeCacheReloadContext.IsRetired(Inno.Extensibility.Types.TypeRef typeRef)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCacheReloadContext.cs#L40) | Determines whether a logical type belongs to the retiring generation.<br>返回：true when the candidate replaces or removes the type.<br>`typeRef`：The logical type to inspect. | — | — |
| <a id="api-80a4e94c377477c2"></a>[`public bool Inno.Extensibility.Types.TypeCacheReloadContext.TryResolveReplacement(Inno.Extensibility.Types.TypeRef previousType, out Inno.Extensibility.Types.TypeRef replacement)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCacheReloadContext.cs#L59) | Tries to find the candidate type that preserves a retiring type's stable identity.<br>返回：true when a distinct replacement exists.<br>`previousType`：A type reference from the previous generation.<br>`replacement`：Receives its candidate replacement when found. | — | — |
| <a id="api-adfac9a55997b588"></a>[`public Inno.Extensibility.Types.TypeCacheSnapshot Inno.Extensibility.Types.TypeCacheReloadContext.candidate { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCacheReloadContext.cs#L29) | Gets the validated candidate type snapshot. | — | — |
| <a id="api-f7910fb00a5d5b26"></a>[`public Inno.Extensibility.Types.TypeCacheSnapshot Inno.Extensibility.Types.TypeCacheReloadContext.previous { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCacheReloadContext.cs#L24) | Gets the active type snapshot from before activation. | — | — |
| <a id="api-93f91626904eb114"></a>[`public sealed class Inno.Extensibility.Types.TypeCacheReloadContext`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCacheReloadContext.cs#L8) | Provides the previous and candidate type catalogs during an assembly reload transaction. | — | — |

### `Inno.Extensibility.Types.TypeCacheSnapshot`

<!-- api: T:Inno.Extensibility.Types.TypeCacheSnapshot f02e7aae991dd5ea895c9939c685eeaf975abfd50692980bca21f8dc44c8c6f0 -->
<!-- api: P:Inno.Extensibility.Types.TypeCacheSnapshot.version 0a306796190cbaf528960f78acd7a7834de066942b707dcefa994773f9fbfa32 -->
<!-- api: P:Inno.Extensibility.Types.TypeCacheSnapshot.types ddcffc4fa174c635f4696838066a999343e08bc3459e70b65cfe50dfd5c94de9 -->
<!-- api: M:Inno.Extensibility.Types.TypeCacheSnapshot.GetSubTypesOf``1 be4fe182950e563ed3ce2a36f303df23586ad9c568354006684f030dd382cd46 -->
<!-- api: M:Inno.Extensibility.Types.TypeCacheSnapshot.GetTypesImplementing``1 63436558a4ab20ebbc46f26d7cbf8e4bc7f065efb1190ee283b518b89a8aff85 -->
<!-- api: M:Inno.Extensibility.Types.TypeCacheSnapshot.GetTypesWithAttribute``1 eb8d7e01eede6f1c6a3825948732c07a98901fc3b41ae171099d7f9b260c51eb -->
<!-- api: M:Inno.Extensibility.Types.TypeCacheSnapshot.GetTypeRef(System.Type) 446eb9b327acfc2b0424eb93e55665727b23678c299d34d879216d3a73c78cf3 -->
<!-- api: M:Inno.Extensibility.Types.TypeCacheSnapshot.TryGetTypeRef(System.Type,Inno.Extensibility.Types.TypeRef@) 8968a323c7ae8d395c74b29aa2902ce5ffb1e4589eebd493c7f721f3cbf3b4e9 -->
<!-- api: M:Inno.Extensibility.Types.TypeCacheSnapshot.CanCreateInstance(Inno.Extensibility.Types.TypeRef) 2e53b2a64a2174457b0b7eb8502505ed4545a0cd4e8b7e310e5bbb211e38e515 -->
<!-- api: M:Inno.Extensibility.Types.TypeCacheSnapshot.CreateInstance(System.Type) 821968147286b764e2d94b1dae51dd65673bb1a6278a2216d0aa5fea452de6af -->
<!-- api: M:Inno.Extensibility.Types.TypeCacheSnapshot.ConstructGenericType(System.Type,System.Collections.Generic.IReadOnlyList{System.Type}) 68286140471acbde725ecea3c4d44ef0f0bea6344f6aec7db313e54be1ef29c3 -->
<!-- api: M:Inno.Extensibility.Types.TypeCacheSnapshot.GetMetadata(Inno.Extensibility.Types.TypeRef) e59abf2ed0c23495042a3f801ad8b5d29d11460140f9e0490510160145b18626 -->
<!-- api: M:Inno.Extensibility.Types.TypeCacheSnapshot.GetAttribute``1(Inno.Extensibility.Types.TypeRef,System.Boolean) 2c44d6840190f35f87c57c41ffd3a54c58e9e6fa03722353b53895e3e6f6dc84 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-bf928c9abc16fc9f"></a>[`public bool Inno.Extensibility.Types.TypeCacheSnapshot.CanCreateInstance(Inno.Extensibility.Types.TypeRef type)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCacheSnapshot.cs#L137) | Determines whether the current deployment can construct a type in this exact generation.<br>返回：Whether a declared type resolves and has an available parameterless factory.<br>`type`：The type reference to resolve against this snapshot. | — | — |
| <a id="api-e9106986cb5e5b1e"></a>[`public System.Type? Inno.Extensibility.Types.TypeCacheSnapshot.ConstructGenericType(System.Type definition, System.Collections.Generic.IReadOnlyList<System.Type> arguments)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCacheSnapshot.cs#L179) | Resolves a generic construction belonging to this exact generation.<br>返回：The resolved closed type, or null when generic constraints reject the arguments.<br>`definition`：A registered generic declaration.<br>`arguments`：Ordered closed type arguments. | — | `System.InvalidOperationException`：The declaration does not belong to this generation.<br>`System.NotSupportedException`：This deployment has not linked the requested construction. |
| <a id="api-df3ef8296f35d458"></a>[`public object Inno.Extensibility.Types.TypeCacheSnapshot.CreateInstance(System.Type type)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCacheSnapshot.cs#L152) | Constructs a declared type through this generation's selected metadata source.<br>返回：The new instance owned by the caller.<br>`type`：A declaration in this snapshot, or a closed construction of its registered generic declaration. | — | `System.InvalidOperationException`：The reference does not resolve or its deployment does not provide a factory. |
| <a id="api-f4b2597e325044f8"></a>[`public TAttribute? Inno.Extensibility.Types.TypeCacheSnapshot.GetAttribute<TAttribute>(Inno.Extensibility.Types.TypeRef type, bool inherit = true) where TAttribute : System.Attribute`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCacheSnapshot.cs#L216) | Reads a single extension attribute without requiring runtime reflection.<br>返回：The matching attribute, or null when absent; duplicate singleton attributes throw.<br>`type`：A declaration in this snapshot.<br>`inherit`：Whether effective inherited attributes participate. | — | — |
| <a id="api-9b0c77cdd6e4953f"></a>[`public Inno.Extensibility.Catalogs.TypeCatalogMetadata Inno.Extensibility.Types.TypeCacheSnapshot.GetMetadata(Inno.Extensibility.Types.TypeRef type)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCacheSnapshot.cs#L199) | Reads the immutable discovery facts for a declaration in this exact generation.<br>返回：Complete metadata; stale or unregistered declarations fail during resolution.<br>`type`：The logical declaration to resolve against this snapshot. | — | — |
| <a id="api-c48abd2a054c353b"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Extensibility.Types.TypeRef> Inno.Extensibility.Types.TypeCacheSnapshot.GetSubTypesOf<T>()`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCacheSnapshot.cs#L68) | Gets all concrete discovered types assignable to T.<br>返回：An immutable snapshot of the values selected by the operation. | — | — |
| <a id="api-8bc09aa425a384ad"></a>[`public Inno.Extensibility.Types.TypeRef Inno.Extensibility.Types.TypeCacheSnapshot.GetTypeRef(System.Type type)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCacheSnapshot.cs#L109) | Gets the reference for a CLR type in this snapshot.<br>返回：The logical and generation-local identity of type.<br>`type`：The type consumed by get type ref; ownership remains with the caller unless explicitly stated otherwise. | — | `System.ArgumentNullException`：Thrown when type is null.<br>`System.InvalidOperationException`：Thrown when the type does not belong to this snapshot. |
| <a id="api-a00961e056d4b08b"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Extensibility.Types.TypeRef> Inno.Extensibility.Types.TypeCacheSnapshot.GetTypesImplementing<TInterface>()`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCacheSnapshot.cs#L79) | Gets all concrete discovered types implementing TInterface.<br>返回：An immutable snapshot of the values selected by the operation. | — | — |
| <a id="api-8781d7308ae4e69d"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Extensibility.Types.TypeRef> Inno.Extensibility.Types.TypeCacheSnapshot.GetTypesWithAttribute<TAttribute>() where TAttribute : System.Attribute`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCacheSnapshot.cs#L91) | Gets all concrete discovered types marked with TAttribute.<br>返回：An immutable snapshot of the values selected by the operation. | — | — |
| <a id="api-bc9ec05cf65f4c3f"></a>[`public bool Inno.Extensibility.Types.TypeCacheSnapshot.TryGetTypeRef(System.Type type, out Inno.Extensibility.Types.TypeRef typeRef)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCacheSnapshot.cs#L123) | Tries to get the reference for a CLR type in this snapshot.<br>返回：true when the type belongs to this snapshot.<br>`type`：The CLR type to identify.<br>`typeRef`：Receives its logical and generation-local identity. | — | — |
| <a id="api-e7a3e0a326da5b9b"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Extensibility.Types.TypeRef> Inno.Extensibility.Types.TypeCacheSnapshot.types { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCacheSnapshot.cs#L57) | Gets every type included in this snapshot. | — | — |
| <a id="api-847c497084f31cc9"></a>[`public long Inno.Extensibility.Types.TypeCacheSnapshot.version { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCacheSnapshot.cs#L52) | Gets the monotonically increasing catalog version. | — | — |
| <a id="api-839037fe9c97a082"></a>[`public sealed class Inno.Extensibility.Types.TypeCacheSnapshot`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCacheSnapshot.cs#L18) | Represents an immutable, internally consistent view of discoverable runtime types. | A snapshot strongly retains its discovered System.Type instances. Callers must not cache an obsolete snapshot or one of its type lists beyond the operation that needs generation consistency, because doing so delays unloading the corresponding collectible assembly load context. | — |

### `Inno.Extensibility.Types.TypeCatalog`

<!-- api: T:Inno.Extensibility.Types.TypeCatalog 21f253f8d269368093e242ee50ecd9ce8f5eea60876e149d4db58cd14586a6b4 -->
<!-- api: M:Inno.Extensibility.Types.TypeCatalog.#ctor(Inno.Extensibility.Modules.ModuleHost,Inno.Extensibility.Types.ITypeCatalogSource) 579c682ea45fe907ff08b7b4c9e0a80568b1e3462f542b52ac5c904fed60ccfa -->
<!-- api: P:Inno.Extensibility.Types.TypeCatalog.isInitialized 9eae3ff57a8c33b371469258df830b26091699914ea01b273e2b4cfd6543646c -->
<!-- api: M:Inno.Extensibility.Types.TypeCatalog.AcquireOperation(System.String) 983c6e6e3f3f545319d63cec92c225b470df1dc5aeb35d2a092e8df9819e416f -->
<!-- api: P:Inno.Extensibility.Types.TypeCatalog.current 24051d8a2792f4c95377618605272e16c0c85c5f1a980bafc139acf0255d1c37 -->
<!-- api: M:Inno.Extensibility.Types.TypeCatalog.Rebuild 2bb4c57f7c9f55c246a3599b7d31254237311cb3afcaaa063641a871f05c599c -->
<!-- api: M:Inno.Extensibility.Types.TypeCatalog.Dispose ab978efc0a777aa0037a2028faebeb59b4c08bfd179b54f3951cd356ff29402b -->
<!-- api: M:Inno.Extensibility.Types.TypeCatalog.GetSubTypesOf``1 6c460f392cb7cc9cc52f4512e68058a1a468a6ec990ab85efa992a58aabf35f5 -->
<!-- api: M:Inno.Extensibility.Types.TypeCatalog.GetTypesImplementing``1 a285fc4e68c5a3bbd20aaa878bbdcd7c9054b2e20557a88270aa9effbeb35530 -->
<!-- api: M:Inno.Extensibility.Types.TypeCatalog.GetTypesWithAttribute``1 d8d05c537a0ede3364e2f3013e8f36fdc224584f2aa711129a815fcde8b9bb60 -->
<!-- api: M:Inno.Extensibility.Types.TypeCatalog.GetTypeRef(System.Type) cfe4264591993009636401692118e54328b1b379c8a6c3c4d81eac4b17c028de -->
<!-- api: M:Inno.Extensibility.Types.TypeCatalog.TryGetTypeRef(System.Type,Inno.Extensibility.Types.TypeRef@) d037246d94e81e001b98615984455c5ce5baceeccebb47610889920e9b32c520 -->
<!-- api: M:Inno.Extensibility.Types.TypeCatalog.TryResolve(Inno.Extensibility.Types.TypeRef,System.Type@) 01b1b6ee4ac5b206b95958792d5ead58803ec43d255cedb8fc6b776167fd0e6a -->
<!-- api: M:Inno.Extensibility.Types.TypeCatalog.Resolve(Inno.Extensibility.Types.TypeRef) 85f73f2579d1832971ee01a869c2f4a8829e6fbc1e2cff6036e8661b6044f7d1 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-743dc37d104eee92"></a>[`public Inno.Extensibility.Types.TypeCatalog.TypeCatalog(Inno.Extensibility.Modules.ModuleHost modules, Inno.Extensibility.Types.ITypeCatalogSource source)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCatalog.cs#L43) | Creates a type catalog derived transactionally from one module host.<br>`modules`：The module host that owns the assembly generations visible to this catalog.<br>`source`：The metadata provider selected by the composition root for every contributed assembly. | — | `System.ArgumentNullException`：Thrown when modules is null.<br>`System.InvalidOperationException`：Thrown when modules has already been disposed. |
| <a id="api-a0edb47e5e220cd0"></a>[`public System.IDisposable Inno.Extensibility.Types.TypeCatalog.AcquireOperation(string operation)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCatalog.cs#L77) | Defers automatic assembly publication until a synchronous typed operation releases its current owners.<br>返回：A scope held across every access to the captured generation; publication may borrow it on its own thread.<br>`operation`：The operation name used for admission diagnostics. | — | `System.InvalidOperationException`：The catalog is inactive, another thread is publishing, or the shared generation owner is Faulted.<br>`Inno.Core.Execution.RetirementPendingException`：Generation retirement has failed while dependent resources remain owned. |
| <a id="api-7fe13ed42b9d3490"></a>[`public void Inno.Extensibility.Types.TypeCatalog.Dispose()`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCatalog.cs#L112) | Unregisters type discovery and releases all active type-registry snapshots. | — | `Inno.Core.Execution.RetirementPendingException`：A generation still owns live work. Type discovery and unfinished snapshots remain owned until host restart. |
| <a id="api-798af98ed61c1221"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Extensibility.Types.TypeRef> Inno.Extensibility.Types.TypeCatalog.GetSubTypesOf<T>()`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCatalog.cs#L148) | Gets all non-abstract discovered types assignable to T.<br>返回：The matching concrete types in stable catalog order. | — | — |
| <a id="api-2f40d69da8247e90"></a>[`public Inno.Extensibility.Types.TypeRef Inno.Extensibility.Types.TypeCatalog.GetTypeRef(System.Type type)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCatalog.cs#L188) | Gets the reference for an active CLR type.<br>返回：Its logical and generation-local identity.<br>`type`：The type consumed by get type ref; ownership remains with the caller unless explicitly stated otherwise. | — | `System.ArgumentNullException`：Thrown when type is null.<br>`System.InvalidOperationException`：Thrown when the type does not belong to the active catalog. |
| <a id="api-961f7aac8f4cacd7"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Extensibility.Types.TypeRef> Inno.Extensibility.Types.TypeCatalog.GetTypesImplementing<TInterface>()`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCatalog.cs#L159) | Gets all non-abstract discovered types implementing TInterface.<br>返回：The matching concrete types in stable catalog order. | — | — |
| <a id="api-0acb10af2ca2d177"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Extensibility.Types.TypeRef> Inno.Extensibility.Types.TypeCatalog.GetTypesWithAttribute<TAttribute>() where TAttribute : System.Attribute`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCatalog.cs#L170) | Gets all non-abstract discovered types marked with TAttribute.<br>返回：The matching concrete types in stable catalog order. | — | — |
| <a id="api-7f52128bc0237850"></a>[`public void Inno.Extensibility.Types.TypeCatalog.Rebuild()`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCatalog.cs#L100) | Rebuilds the assembly catalog, type snapshot, and every registered type registry. | — | — |
| <a id="api-653c02d85ca9ed90"></a>[`public System.Type Inno.Extensibility.Types.TypeCatalog.Resolve(Inno.Extensibility.Types.TypeRef typeRef)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCatalog.cs#L243) | Resolves a logical type reference against the active immutable generation.<br>返回：The CLR type owned by the active generation.<br>`typeRef`：The stable type reference to resolve. | — | `System.InvalidOperationException`：Thrown when the reference is empty or unavailable in the active generation. |
| <a id="api-14e66f20a7755aca"></a>[`public bool Inno.Extensibility.Types.TypeCatalog.TryGetTypeRef(System.Type type, out Inno.Extensibility.Types.TypeRef typeRef)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCatalog.cs#L202) | Tries to get the reference for an active CLR type.<br>返回：true when the type belongs to the active catalog.<br>`type`：The active CLR type.<br>`typeRef`：Receives its logical and generation-local identity. | — | — |
| <a id="api-37a9c05e270c3057"></a>[`public bool Inno.Extensibility.Types.TypeCatalog.TryResolve(Inno.Extensibility.Types.TypeRef typeRef, out System.Type? type)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCatalog.cs#L219) | Attempts to resolve a logical type reference against the active immutable generation.<br>返回：true when the active generation contains the logical type.<br>`typeRef`：The stable type reference to resolve.<br>`type`：Receives the active CLR type when resolution succeeds. | — | — |
| <a id="api-f37e4c5fa46765c3"></a>[`public Inno.Extensibility.Types.TypeCacheSnapshot Inno.Extensibility.Types.TypeCatalog.current { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCatalog.cs#L86) | Gets the current immutable type snapshot after applying pending host assembly changes. | — | — |
| <a id="api-5c407aa03e80127d"></a>[`public bool Inno.Extensibility.Types.TypeCatalog.isInitialized { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCatalog.cs#L60) | Gets whether the type catalog is registered with an initialized Inno.Extensibility.Modules.ModuleHost. | — | — |
| <a id="api-4f0425a0e0d434d5"></a>[`public sealed class Inno.Extensibility.Types.TypeCatalog : System.IDisposable`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeCatalog.cs#L13) | Owns the active type catalog and provides all type discovery and identity queries. | — | — |

### `Inno.Extensibility.Types.TypeRef`

<!-- api: T:Inno.Extensibility.Types.TypeRef 6a5060e7381654d6108c9fc0cf8e25edce7980da63b9a24863ad516bd2a1b349 -->
<!-- api: M:Inno.Extensibility.Types.TypeRef.#ctor(System.Guid) f693555fbf120dee3ecc6d6e67a19f010f468e50111d70bcd4f702ffc9238910 -->
<!-- api: P:Inno.Extensibility.Types.TypeRef.stableId 63afb09f04cba9dc8ff7e035dce09317f258a9ed734af6f1a0f8e0b34685f2a0 -->
<!-- api: P:Inno.Extensibility.Types.TypeRef.runtimeId 9e289d5ea4ff40fe98eb098fbe114bf5cceaad9c81d55648f1c5bc495f72fbe6 -->
<!-- api: M:Inno.Extensibility.Types.TypeRef.IsValid(Inno.Extensibility.Types.TypeCatalog) 36a00c85acad266d0b97320a1bfa748bd11bfa165518ccf35de70c40914d749e -->
<!-- api: M:Inno.Extensibility.Types.TypeRef.Resolve(Inno.Extensibility.Types.TypeCatalog) e956277b7d3fe06f9f8831c6ef32408aef26eaf8e072ff18c338b4cb962e10f0 -->
<!-- api: M:Inno.Extensibility.Types.TypeRef.Resolve(Inno.Extensibility.Types.TypeCacheSnapshot) c58b7f829e300eef5ddfb87490401b4ddd279987948f6bca612cab5cca5555a5 -->
<!-- api: M:Inno.Extensibility.Types.TypeRef.Equals(Inno.Extensibility.Types.TypeRef) 366a1ea75e61c810bdefb1452c3e01c4d79d4cb76827f69cb2cded1c85e6e65b -->
<!-- api: M:Inno.Extensibility.Types.TypeRef.Equals(System.Object) f12155220933f0e3684a9a8490092416e63900c6862604715fb33791bb2c0dec -->
<!-- api: M:Inno.Extensibility.Types.TypeRef.GetHashCode d9f9dcebfde036fe350c61d3cbe10a1656b899129b5160fe8c9e6005966a571f -->
<!-- api: M:Inno.Extensibility.Types.TypeRef.ToString 8e837d999799dde53a02bdaaaa94932bc81f90dc3949bc9897ae1d160fd72a62 -->
<!-- api: M:Inno.Extensibility.Types.TypeRef.op_Equality(Inno.Extensibility.Types.TypeRef,Inno.Extensibility.Types.TypeRef) 5ac11be34a65468500b6e6fed97ac1fc9717f3b9774e843f6592b2640a6d87aa -->
<!-- api: M:Inno.Extensibility.Types.TypeRef.op_Inequality(Inno.Extensibility.Types.TypeRef,Inno.Extensibility.Types.TypeRef) d36997f61963be07dfeebc8a46dfaf6df6d56178694e8b9677296b82165e7d57 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3f384573974f6217"></a>[`public Inno.Extensibility.Types.TypeRef.TypeRef(System.Guid stableId)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeRef.cs#L20) | Creates an unresolved type reference from its persistent identity.<br>`stableId`：The persistent type identity, or an empty value for an invalid reference. | — | — |
| <a id="api-329835b5642aea94"></a>[`public bool Inno.Extensibility.Types.TypeRef.Equals(Inno.Extensibility.Types.TypeRef other)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeRef.cs#L117) | Determines whether this instance and the supplied value represent the same logical state.<br>返回：true when both values represent the same logical state; otherwise, false.<br>`other`：The value to compare with this instance. | — | — |
| <a id="api-83834b7ef29b552b"></a>[`public override bool Inno.Extensibility.Types.TypeRef.Equals(object? obj)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeRef.cs#L128) | Determines whether this instance and the supplied value represent the same logical state.<br>返回：true when both values represent the same logical state; otherwise, false.<br>`obj`：The object to compare with this instance. | — | — |
| <a id="api-cfd2afd9cf297e91"></a>[`public override int Inno.Extensibility.Types.TypeRef.GetHashCode()`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeRef.cs#L136) | Computes a hash code from the fields that participate in logical equality.<br>返回：A hash code consistent with the implemented equality contract. | — | — |
| <a id="api-053a49db130e2e5b"></a>[`public bool Inno.Extensibility.Types.TypeRef.IsValid(Inno.Extensibility.Types.TypeCatalog catalog)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeRef.cs#L58) | Determines whether one type catalog currently resolves this reference.<br>返回：true when the reference is non-empty and currently resolvable.<br>`catalog`：The catalog whose active immutable generation should be queried. | — | `System.ArgumentNullException`：Thrown when catalog is null. |
| <a id="api-0ed94e1a432d6672"></a>[`public static bool Inno.Extensibility.Types.TypeRef.operator ==(Inno.Extensibility.Types.TypeRef left, Inno.Extensibility.Types.TypeRef right)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeRef.cs#L158) | Determines whether two references identify the same logical type.<br>返回：true when both references have the same stable identity.<br>`left`：The first reference.<br>`right`：The second reference. | — | — |
| <a id="api-923164c9e9d833f9"></a>[`public static bool Inno.Extensibility.Types.TypeRef.operator !=(Inno.Extensibility.Types.TypeRef left, Inno.Extensibility.Types.TypeRef right)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeRef.cs#L175) | Determines whether two references identify different logical types.<br>返回：true when the references have different stable identities.<br>`left`：The first reference.<br>`right`：The second reference. | — | — |
| <a id="api-bf917aa075915655"></a>[`public System.Type Inno.Extensibility.Types.TypeRef.Resolve(Inno.Extensibility.Types.TypeCacheSnapshot snapshot)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeRef.cs#L100) | Resolves this reference against a specific immutable type-cache snapshot.<br>返回：The CLR type represented by this reference in snapshot.<br>`snapshot`：The snapshot that defines the required assembly generation. | — | `System.ArgumentNullException`：Thrown when snapshot is null.<br>`System.InvalidOperationException`：Thrown when the reference is empty or the logical type does not exist in the snapshot. |
| <a id="api-bbf30a5d68159544"></a>[`public System.Type Inno.Extensibility.Types.TypeRef.Resolve(Inno.Extensibility.Types.TypeCatalog catalog)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeRef.cs#L79) | Resolves this reference against one catalog's active immutable generation.<br>返回：The active CLR type represented by this reference.<br>`catalog`：The catalog that owns the required type generation. | — | `System.InvalidOperationException`：Thrown when the reference is empty, the type cache is unavailable, or the logical type is not loaded.<br>`System.ArgumentNullException`：Thrown when catalog is null. |
| <a id="api-1d5201967ee655bc"></a>[`public override string Inno.Extensibility.Types.TypeRef.ToString()`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeRef.cs#L144) | Formats this value as a human-readable representation.<br>返回：The human-readable representation of this value. | — | — |
| <a id="api-a33a2d2c7a746a61"></a>[`public int Inno.Extensibility.Types.TypeRef.runtimeId { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeRef.cs#L44) | Gets the generation-local lookup hint captured when this value was created. | This value must not be persisted or used as logical type identity. | — |
| <a id="api-795cad1418ebe6dc"></a>[`public System.Guid Inno.Extensibility.Types.TypeRef.stableId { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeRef.cs#L36) | Gets the persistent identity used across assembly generations and process launches. | — | — |
| <a id="api-9b8925da12eb170b"></a>[`public readonly struct Inno.Extensibility.Types.TypeRef : System.IEquatable<Inno.Extensibility.Types.TypeRef>`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeCache/TypeRef.cs#L12) | Identifies one logical runtime type without retaining its assembly or load context. | Equality and persistence use Inno.Extensibility.Types.TypeRef.stableId. The Inno.Extensibility.Types.TypeRef.runtimeId is only a generation-local resolution hint and can become stale after an assembly reload. | — |

### `Inno.Extensibility.Types.TypeRegistry`1`

<!-- api: T:Inno.Extensibility.Types.TypeRegistry`1 5827a0c8da88f00591676b5a5c7a1cd2932b6e86011bf665faebb6b0dbdf83cb -->
<!-- api: M:Inno.Extensibility.Types.TypeRegistry`1.#ctor(Inno.Extensibility.Types.TypeCatalog,System.Nullable{System.TimeSpan}) f3b1d6f4af36071ff74c0986dfbfce36b6202406695e783f787cdc0a1c4d9276 -->
<!-- api: P:Inno.Extensibility.Types.TypeRegistry`1.isInitialized 9e37c0908ba2efb4fb98303d610c6866efe1026ad2be0f5179ff2d1987328d44 -->
<!-- api: M:Inno.Extensibility.Types.TypeRegistry`1.Refresh 489139a516bb16969402998053b461597fabcec770b068a3dbd41bc570d06da8 -->
<!-- api: M:Inno.Extensibility.Types.TypeRegistry`1.Clear a1e7323855ec484b00cc518e620b7a5179e14ec7d3eab5e931e8a11be12ac6ad -->
<!-- api: M:Inno.Extensibility.Types.TypeRegistry`1.Dispose c83f949e862621176f62f38ca1cc414907a1c0f4bf8be1fac6475373d1fccb9b -->
<!-- api: P:Inno.Extensibility.Types.TypeRegistry`1.current 4ae0b890aed00669600dbda6f8054611d803d5811ce4266bbb7b6f86254a75a7 -->
<!-- api: M:Inno.Extensibility.Types.TypeRegistry`1.Build(Inno.Extensibility.Types.TypeCacheSnapshot) 9aa532cc3b8733c94ed36e9f8124f58fe488e365166c08217b93101bf021e2a3 -->
<!-- api: M:Inno.Extensibility.Types.TypeRegistry`1.OnActivating(`0,`0) 3d4282679a026d267f2faad104e47f06868701657b991875fdd44192f9cf723c -->
<!-- api: M:Inno.Extensibility.Types.TypeRegistry`1.OnActivationRolledBack(`0,`0) 95b0dfed83b7f63f997ea1c681426fc2f7aad41af0ce27723bc6e7d94595b59f -->
<!-- api: M:Inno.Extensibility.Types.TypeRegistry`1.OnActivationCompleted(`0,`0) 1842809605fca1eb2eb48402201072dc307b9ad0d4a8949695f231de5facd74e -->
<!-- api: M:Inno.Extensibility.Types.TypeRegistry`1.DisposeSnapshot(`0) f066143feb8264a71606a62a0043791baae8eaee8cc1e768f2f77399daf00e2f -->
<!-- api: M:Inno.Extensibility.Types.TypeRegistry`1.OnCleanupFailed(System.String,System.Exception) c545d0b6794fec6a38919e67232256511ac277e4a2fe79506b0d51cbeeb8c614 -->
<!-- api: M:Inno.Extensibility.Types.TypeRegistry`1.ReportRetirementFailure(System.Exception) 19a37c2f9ed21e454a39bb7879fd0443b36a534158f5161d446a399f614e6705 -->
<!-- api: M:Inno.Extensibility.Types.TypeRegistry`1.CreateExtension``1(System.Type) c6077305c96ba2c8d39b430c868d9a099af6f7ae6342a0931d65ddbbc843d58d -->
<!-- api: M:Inno.Extensibility.Types.TypeRegistry`1.OwnCandidateExtension``1(``0) 898de4422eca6c97fe998a635271c3983dbb1ca3971e5a46a9c3620fd01843a2 -->
<!-- api: M:Inno.Extensibility.Types.TypeRegistry`1.DisposeExtensions(System.Collections.Generic.IEnumerable{System.Object}) 790d779a2e9e4bdc02ad33bb62cde73abafcda370dd17a7290e83a711619dadc -->
<!-- api: M:Inno.Extensibility.Types.TypeRegistry`1.RetireResource(System.String,System.Action) 5e5862379a91b84ae1498665684dee18012f6c1bad676391d2610145becaa3d3 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9aa0793281fd1399"></a>[`protected Inno.Extensibility.Types.TypeRegistry<TSnapshot>.TypeRegistry(Inno.Extensibility.Types.TypeCatalog types, System.TimeSpan? retirementTimeout = null)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeRegistry.cs#L49) | Creates and registers a refreshable type registry.<br>`types`：The type catalog that coordinates this registry's candidate generations.<br>`retirementTimeout`：The positive owner-thread drain deadline; omitted values use thirty seconds. | — | `System.ArgumentNullException`：Thrown when types is null.<br>`System.ArgumentOutOfRangeException`：The retirement timeout is not positive. |
| <a id="api-33575575ba6083ff"></a>[`protected abstract TSnapshot Inno.Extensibility.Types.TypeRegistry<TSnapshot>.Build(Inno.Extensibility.Types.TypeCacheSnapshot types)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeRegistry.cs#L231) | Builds a complete candidate registry without changing the active snapshot.<br>返回：The complete immutable registry snapshot.<br>`types`：The candidate type-cache snapshot. | — | — |
| <a id="api-c90d6912d59861e3"></a>[`public void Inno.Extensibility.Types.TypeRegistry<TSnapshot>.Clear()`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeRegistry.cs#L130) | Releases the active snapshot while keeping the registry reusable. | — | `Inno.Core.Execution.RetirementPendingException`：Generation work could not retire safely. The snapshot remains owned and this registry cannot be reused. |
| <a id="api-86ad2ba4adf04653"></a>[`protected TExtension Inno.Extensibility.Types.TypeRegistry<TSnapshot>.CreateExtension<TExtension>(System.Type type)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeRegistry.cs#L349) | Creates a validated extension instance using a parameterless constructor.<br>返回：A newly created extension, retained for rollback until the complete candidate snapshot takes ownership.<br>`type`：The concrete implementation type. | — | — |
| <a id="api-7bd1676c54d2dae5"></a>[`public void Inno.Extensibility.Types.TypeRegistry<TSnapshot>.Dispose()`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeRegistry.cs#L169) | Releases the current registry snapshot. | — | `Inno.Core.Execution.RetirementPendingException`：Live generation work remains; repeated disposal cannot bypass its retained ownership barrier. |
| <a id="api-2137b172918345b0"></a>[`protected void Inno.Extensibility.Types.TypeRegistry<TSnapshot>.DisposeExtensions(System.Collections.Generic.IEnumerable<object> extensions)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeRegistry.cs#L408) | Retires distinct disposable extensions in reverse order, draining pending work before lower dependencies.<br>`extensions`：Instances whose ownership has ended; duplicate references are disposed once. | — | `System.AggregateException`：One or more instances failed retirement after all remaining instances were attempted.<br>`Inno.Core.Execution.RetirementTimeoutException`：An instance did not drain. The registry retains the batch and faults its shared generation gate. |
| <a id="api-9b52ceb59af66d67"></a>[`protected virtual void Inno.Extensibility.Types.TypeRegistry<TSnapshot>.DisposeSnapshot(TSnapshot snapshot)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeRegistry.cs#L296) | Releases resources owned by a registry snapshot.<br>`snapshot`：The snapshot that is no longer active. | — | — |
| <a id="api-31e3b00f8f21dbb5"></a>[`protected virtual void Inno.Extensibility.Types.TypeRegistry<TSnapshot>.OnActivating(TSnapshot? previous, TSnapshot candidate)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeRegistry.cs#L246) | Reversibly activates a complete candidate while the previous snapshot remains available.<br>`previous`：The previous active snapshot, or null during initial activation.<br>`candidate`：The candidate exposed through Inno.Extensibility.Types.TypeRegistry`1.current during this callback. | Implementations may perform fallible lifecycle work here. They must keep enough local state for Inno.Extensibility.Types.TypeRegistry`1.OnActivationRolledBack(`0,`0) to reverse every completed step. | — |
| <a id="api-2b3dcfb4ecbbafc6"></a>[`protected virtual void Inno.Extensibility.Types.TypeRegistry<TSnapshot>.OnActivationCompleted(TSnapshot? previous, TSnapshot currentSnapshot)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeRegistry.cs#L284) | Finalizes a successfully activated candidate after every coordinated registry has activated.<br>`previous`：The previous snapshot that is about to be released, or null.<br>`currentSnapshot`：The committed active snapshot. | This is a cleanup-only phase and must not perform fallible publication work. Exceptions are reported through Inno.Extensibility.Types.TypeRegistry`1.OnCleanupFailed(System.String,System.Exception) and cannot cause the completed activation to roll back. | — |
| <a id="api-6cad800e107503da"></a>[`protected virtual void Inno.Extensibility.Types.TypeRegistry<TSnapshot>.OnActivationRolledBack(TSnapshot? previous, TSnapshot candidate)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeRegistry.cs#L265) | Reverses lifecycle work performed while activating a candidate snapshot.<br>`previous`：The restored active snapshot, or null when none existed.<br>`candidate`：The rejected candidate snapshot. | Ordinary failures are reported through Inno.Extensibility.Types.TypeRegistry`1.OnCleanupFailed(System.String,System.Exception) and aggregated across registries. Unfinished retirement retains dependencies and stops further rollback until the host is restarted. | — |
| <a id="api-63a10a9df7d4a8c6"></a>[`protected virtual void Inno.Extensibility.Types.TypeRegistry<TSnapshot>.OnCleanupFailed(string phase, System.Exception exception)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeRegistry.cs#L315) | Reports an exception raised while rolling back activation, completing activation, or releasing a snapshot.<br>`phase`：The non-transactional phase that raised the exception.<br>`exception`：The cleanup exception. | This callback is diagnostic only. Exceptions raised by an override are ignored so cleanup can continue. | — |
| <a id="api-73a18f735b8b452c"></a>[`protected TExtension Inno.Extensibility.Types.TypeRegistry<TSnapshot>.OwnCandidateExtension<TExtension>(TExtension extension)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeRegistry.cs#L386) | Adds a newly constructed extension to the current build's rollback ownership before further validation.<br>返回：The same instance. After a successful build its containing snapshot must own its retirement.<br>`extension`：A new instance, never an instance borrowed from the active snapshot. | — | `System.InvalidOperationException`：This method is called outside synchronous candidate construction. |
| <a id="api-08a498c559d9871a"></a>[`public void Inno.Extensibility.Types.TypeRegistry<TSnapshot>.Refresh()`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeRegistry.cs#L81) | Refreshes this registry from the currently active type snapshot. | Each refresh iteration is a complete transaction. If the active type snapshot changes during activation, convergence runs as a separate transaction after the completed transaction returns. | — |
| <a id="api-1f4f75df6685fe62"></a>[`protected void Inno.Extensibility.Types.TypeRegistry<TSnapshot>.ReportRetirementFailure(System.Exception exception)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeRegistry.cs#L331) | Faults the shared generation gate when a resource composed from this registry cannot retire safely.<br>`exception`：The retirement or compensation failure; this notification never makes publication reversible. | — | — |
| <a id="api-a674743724d826f4"></a>[`protected void Inno.Extensibility.Types.TypeRegistry<TSnapshot>.RetireResource(string owner, System.Action retire)`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeRegistry.cs#L429) | Drains one retryable lifecycle operation at the control-thread safe point before releasing dependencies.<br>`owner`：The resource or extension identifier included in deadline diagnostics.<br>`retire`：The retirement operation, including owner-thread completion pumping when required. | — | `Inno.Core.Execution.RetirementTimeoutException`：The operation exceeded its deadline. Its closure stays owned and further retirement or refresh is blocked. |
| <a id="api-f5178efc052d31b5"></a>[`protected TSnapshot Inno.Extensibility.Types.TypeRegistry<TSnapshot>.current { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeRegistry.cs#L204) | Gets the current snapshot, refreshing it when the type catalog changed. | — | — |
| <a id="api-6e337e4c6e4f9d61"></a>[`public bool Inno.Extensibility.Types.TypeRegistry<TSnapshot>.isInitialized { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeRegistry.cs#L65) | Gets whether the registry has built its first snapshot. | — | — |
| <a id="api-cc29493ab20a2809"></a>[`public abstract class Inno.Extensibility.Types.TypeRegistry<TSnapshot> where TSnapshot : class : System.IDisposable`](../../src/foundation/extensibility/Inno.Extensibility.Types/TypeRegistry.cs#L16) | Builds immutable extension registries from versioned type-cache snapshots. | — | — |

## 项目依赖

- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Modules](Inno.Extensibility.Modules.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
