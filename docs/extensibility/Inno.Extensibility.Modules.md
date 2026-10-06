# Inno.Extensibility.Modules

[Extensibility 索引](README.md) · [Wiki 首页](../README.md) · [Types](Inno.Extensibility.Types.md) · [DotNet 来源](../platform/Inno.Adapter.Modules.DotNet.md)

## 职责与依赖

Foundation 拥有模块身份、目录快照、依赖排序、候选事务和 generation admission。它通过 `IAssemblyCatalogSource` 与 `IModuleSource` 接收代码；ALC、文件探测、shadow copy 和动态反射属于 DotNet Adapter。本项目依赖 Core.Collections、Core.Execution 与 Extensibility.Reload，不引用具体加载器、Types 或业务领域。

`ModuleHost` 是实例 owner。组合入口必须同时选择程序集来源与对应的类型元数据来源。共享事务适用于静态链接代码与 Editor 动态代码；是否可卸载由来源及其 `IModuleLifetime` 声明。

## 公开契约

| 类型或入口 | 当前行为 |
| --- | --- |
| `ModuleHost(ModuleHostOptions)` | 接管 `catalogSource`、订阅目录变化并发布初始目录；启动失败注销和释放已取得的资源。 |
| `ModuleHostOptions.cacheDirectory / catalogSource` | 来源产物根与必填目录来源。Foundation 不隐式创建 ALC 或扫描全进程程序集。 |
| `ModuleHost.isInitialized / generations / modules` | Host 状态、统一 generation gate 与非 owning 模块信息。 |
| `RegisterCatalogParticipant(participant)` | 准备、激活当前目录的派生状态，返回可释放的注册。 |
| `Load(IModuleSource)` | 准备并发布一个来源贡献，返回 `AssemblyModuleHandle`。 |
| `Register(moduleName, assemblies)` | 登记调用方拥有的外部代码；调用方负责其最终生命周期。 |
| `BeginReload(handle, source)` | 替换一个指定模块的候选。 |
| `BeginReload(sources)` | 对完整来源集合构建同一 publication。 |
| `BeginReload(sources, removedModuleNames)` | 将替换、新增与移除纳入同一事务。 |
| `Unload(handle)` / `Unload(handles)` | 移除目录贡献并返回退休 probe；返回 probe 不等于退休完成。 |
| `Refresh / Rebuild / Dispose` | 对账来源变化、明确重建或退休 Host；Faulted 不可复位。 |
| `IAssemblyCatalogSource` | `changed`、`GetAssemblies`、`GetSharedAssemblies`、`IsFrameworkReference`、`IsFrameworkAssembly` 和 `Dispose`。 |
| `IModuleSource` | `moduleName / domain / scope / collectible / upstreamModuleNames / assemblyScopes`、预检 `GetAssemblyNames()`、候选获取 `Prepare(context)`。 |
| `ModuleCatalogContribution` | 构造后冻结模块名、domain、scope、assemblies、逐程序集 scopes 和可选 lifetime。 |
| `ModuleSourceContext` | 当前候选的 `generation / artifactDirectory / host / upstreamModules / activeModules / plannedAssemblies`；`TrackRetirement(probe)` 将失败获取的退休责任交给共享 gate。 |
| `ModuleAssemblyDescriptor` | 冻结 planned assembly 的 domain 与 scope。 |
| `IModuleLifetime.BeginRetirement()` | 来源交出不再强持有代码的释放 probe；Foundation 不知道 ALC。 |
| `AssemblyCatalogSnapshot(version, assemblies)` | generation revision 与独立只读程序集列表。快照仍强持有程序集，须在退休前释放。 |
| `IAssemblyCatalogParticipant.Prepare(candidate)` | 构建完整 unpublished 派生状态；失败保留 last-good。 |
| `IAssemblyCatalogTransaction.Activate / Complete / Rollback` | 原子切换、释放前代或回滚候选。 |
| `AssemblyReloadSession.context / generations` | 同一候选的中立上下文与 gate；`Activate / Complete / Rollback / Dispose` 管理 publication。 |
| `AssemblyReloadContext` | `previousCatalog / candidateCatalog / module / modules`；`GetContext<T>() / TryGetContext<T>()` 查询候选 participant 上下文，完成后目录访问失效。 |
| `AssemblyModuleHandle` | 运行时模块事务身份，不是 Object persistent ID。 |
| `AssemblyModuleInfo` | handle、moduleName、generation、collectible、externallyOwned、domain、scope、status、assemblyNames 与 upstreamModuleNames。 |
| `AssemblyDomain` | InnoInternal、InnoScripting、InnoPlugin 的代码所有权域。 |
| `AssemblyScope` | Runtime 与 Editor 的依赖作用域。 |
| `AssemblyModuleStatus` | 活动或退休过程的模块诊断状态。 |
| `AssemblyExtensions` | `GetInnoAssemblyDomain`、`GetInnoAssemblyScope` 与 `TryGetInnoAssemblyClassification` 读取当前明确的程序集 metadata。 |

本项目没有面向外部继承者的 protected 扩展面；使用来源和 participant 接口组合。

## 初始化示例

```csharp
using Inno.Adapter.Modules.DotNet;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;

using var modules = new ModuleHost(new ModuleHostOptions
{
    cacheDirectory = cachePath,
    catalogSource = new DotNetAssemblyCatalogSource(typeof(Application).Assembly)
});
using var types = new TypeCatalog(modules, new ReflectionTypeCatalogSource());
```

静态 Player 使用 Runtime 的 `StaticAssemblyCatalogSource / StaticModuleSource / StaticTypeCatalogSource`。类型、序列化与业务 registry 先于 ModuleHost 退休；owner-thread safe point 进行 publication，不在来源里偷偷切换全局状态。

## 失败与退休

获取贡献失败时来源必须释放其资源，必要时用 `TrackRetirement` 登记未完成的释放。候选 Prepare/Activate 失败完整回滚后保留 last-good，只有新来源变化或显式重建才重试。包装在 Aggregate/InnerException 中的 pending retirement 仍属于 pending。

可卸载代码必须经过 Full GC → finalizers → Full GC 与弱 probe 验证，成功前拒绝新的 Reload/Play/Build/Export。超时置 Faulted，保留未退出的 owner 与事务，要求重新启动 Host。静态链接代码的进程生命周期不伪装为 collectible unload。

## 验证入口

`tests/extensibility/Inno.Extensibility.Modules.Tests` 覆盖真实来源、依赖、候选成功/失败、回滚和残留强引用。静态与动态元数据的共同目录行为还由 Types、Runtime 和 Scene 测试验证。






## 本轮边界与所有权

ModuleSourceContext 和 ModuleHostOptions 不携带动态加载磁盘缓存要求。Foundation 管理 catalog、身份、snapshot 与 generation transaction；DotNet source 自己拥有 shadow-copy 根。静态来源不需要动态缓存目录。

## 当前源码公开 API 清单

以下仅列出当前程序集自己声明的 public/protected 契约；继承成员遵循所属基类页面。internal/private 实现不作为稳定公开 API。签名依据当前源码语义模型生成，行为、参数、异常与所有权说明同时以对应英文 XML 为准。

### `Inno.Extensibility.Modules.AssemblyCatalogSnapshot`

| 当前声明 | 行为 |
| --- | --- |
| [`System.Collections.Generic.IReadOnlyList<System.Reflection.Assembly> Inno.Extensibility.Modules.AssemblyCatalogSnapshot.assemblies`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/AssemblyCatalogSnapshot.cs#L34) | Gets the host and active module assemblies in this generation. |
| [`long Inno.Extensibility.Modules.AssemblyCatalogSnapshot.version`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/AssemblyCatalogSnapshot.cs#L29) | Gets the monotonically increasing catalog version. |
| [`Inno.Extensibility.Modules.AssemblyCatalogSnapshot`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/AssemblyCatalogSnapshot.cs#L14) | Represents an immutable view of the assemblies participating in one catalog generation. |

### `Inno.Extensibility.Modules.AssemblyDomain`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Extensibility.Modules.AssemblyDomain.InnoInternal`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Metadata/AssemblyDomain.cs#L11) | The assembly is engine-owned and remains in the default load context. |
| [`Inno.Extensibility.Modules.AssemblyDomain.InnoPlugin`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Metadata/AssemblyDomain.cs#L21) | The assembly belongs to the project's unified collectible plugin generation. |
| [`Inno.Extensibility.Modules.AssemblyDomain.InnoScripting`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Metadata/AssemblyDomain.cs#L16) | The assembly contains project scripting code in a collectible load context. |
| [`Inno.Extensibility.Modules.AssemblyDomain`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Metadata/AssemblyDomain.cs#L6) | Identifies the ownership and reload boundary of a managed assembly. |

### `Inno.Extensibility.Modules.AssemblyExtensions`

| 当前声明 | 行为 |
| --- | --- |
| [`static Inno.Extensibility.Modules.AssemblyDomain Inno.Extensibility.Modules.AssemblyExtensions.GetInnoAssemblyDomain(System.Reflection.Assembly assembly)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Metadata/AssemblyExtensions.cs#L32) | Resolves the reload domain declared by an assembly. |
| [`static Inno.Extensibility.Modules.AssemblyScope Inno.Extensibility.Modules.AssemblyExtensions.GetInnoAssemblyScope(System.Reflection.Assembly assembly)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Metadata/AssemblyExtensions.cs#L55) | Resolves the dependency scope declared by an assembly. |
| [`static bool Inno.Extensibility.Modules.AssemblyExtensions.TryGetInnoAssemblyClassification(System.Reflection.Assembly assembly, out Inno.Extensibility.Modules.AssemblyDomain domain, out Inno.Extensibility.Modules.AssemblyScope scope)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Metadata/AssemblyExtensions.cs#L78) | Reads ownership metadata without treating an unrelated assembly as an extension. |
| [`Inno.Extensibility.Modules.AssemblyExtensions`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Metadata/AssemblyExtensions.cs#L10) | Provides ownership and scope metadata helpers for assemblies participating in the Inno runtime. |

### `Inno.Extensibility.Modules.AssemblyModuleHandle`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Extensibility.Modules.AssemblyModuleHandle`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleHandle.cs#L11) | Identifies a logical assembly module without retaining its runtime assemblies. |

### `Inno.Extensibility.Modules.AssemblyModuleInfo`

| 当前声明 | 行为 |
| --- | --- |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Extensibility.Modules.AssemblyModuleInfo.upstreamModuleNames`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleInfo.cs#L61) | Gets stable module dependencies used by this active generation. |
| [`Inno.Extensibility.Modules.AssemblyModuleInfo`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleInfo.cs#L47) | Provides non-owning diagnostic information about an active assembly module. |

### `Inno.Extensibility.Modules.AssemblyModuleStatus`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Extensibility.Modules.AssemblyModuleStatus.Active`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleInfo.cs#L14) | The module generation is visible to assembly catalog participants. |
| [`Inno.Extensibility.Modules.AssemblyModuleStatus`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleInfo.cs#L9) | Describes the catalog state of a published assembly module generation. |

### `Inno.Extensibility.Modules.AssemblyReloadContext`

| 当前声明 | 行为 |
| --- | --- |
| [`TContext Inno.Extensibility.Modules.AssemblyReloadContext.GetContext<TContext>()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Reloading/AssemblyReloadContext.cs#L59) | Gets a context contributed by a registered catalog participant. |
| [`bool Inno.Extensibility.Modules.AssemblyReloadContext.TryGetContext<TContext>(out TContext? context)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Reloading/AssemblyReloadContext.cs#L79) | Tries to get a context contributed by a registered catalog participant. |
| [`Inno.Extensibility.Modules.AssemblyCatalogSnapshot Inno.Extensibility.Modules.AssemblyReloadContext.candidateCatalog`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Reloading/AssemblyReloadContext.cs#L35) | Gets the validated candidate assembly catalog. |
| [`Inno.Extensibility.Modules.AssemblyModuleHandle Inno.Extensibility.Modules.AssemblyReloadContext.module`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Reloading/AssemblyReloadContext.cs#L40) | Gets the first logical module in dependency staging order. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Extensibility.Modules.AssemblyModuleHandle> Inno.Extensibility.Modules.AssemblyReloadContext.modules`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Reloading/AssemblyReloadContext.cs#L45) | Gets every logical module staged by this atomic reload transaction. |
| [`Inno.Extensibility.Modules.AssemblyCatalogSnapshot Inno.Extensibility.Modules.AssemblyReloadContext.previousCatalog`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Reloading/AssemblyReloadContext.cs#L30) | Gets the assembly catalog from before activation. |
| [`Inno.Extensibility.Modules.AssemblyReloadContext`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Reloading/AssemblyReloadContext.cs#L9) | Provides assembly catalogs and participant-specific state during a reload transaction. |

### `Inno.Extensibility.Modules.AssemblyReloadSession`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Extensibility.Modules.AssemblyReloadSession.Activate()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Reloading/AssemblyReloadSession.cs#L48) | Atomically publishes the candidate module and all participant snapshots. |
| [`Inno.Extensibility.Reload.IAssemblyUnloadProbe Inno.Extensibility.Modules.AssemblyReloadSession.Complete()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Reloading/AssemblyReloadSession.cs#L64) | Commits an activated reload and begins cooperative unload of the old generation. |
| [`void Inno.Extensibility.Modules.AssemblyReloadSession.Dispose()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Reloading/AssemblyReloadSession.cs#L125) | Rolls back an incomplete session. |
| [`void Inno.Extensibility.Modules.AssemblyReloadSession.Rollback()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Reloading/AssemblyReloadSession.cs#L94) | Restores the previous generation and unloads the candidate generation. |
| [`Inno.Extensibility.Modules.AssemblyReloadContext Inno.Extensibility.Modules.AssemblyReloadSession.context`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Reloading/AssemblyReloadSession.cs#L35) | Gets the old and candidate assembly catalogs and participant migration contexts. |
| [`Inno.Extensibility.Reload.GenerationCoordinator Inno.Extensibility.Modules.AssemblyReloadSession.generations`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Reloading/AssemblyReloadSession.cs#L40) | Gets the owning host's shared generation gate, including discarded-candidate retirements. |
| [`Inno.Extensibility.Modules.AssemblyReloadSession`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Reloading/AssemblyReloadSession.cs#L12) | Controls activation, completion, and rollback of one prepared module generation. |

### `Inno.Extensibility.Modules.AssemblyScope`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Extensibility.Modules.AssemblyScope.Editor`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Metadata/AssemblyScope.cs#L16) | The assembly is available only to editor consumers. |
| [`Inno.Extensibility.Modules.AssemblyScope.Runtime`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Metadata/AssemblyScope.cs#L11) | The assembly is available to runtime and editor consumers. |
| [`Inno.Extensibility.Modules.AssemblyScope`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Metadata/AssemblyScope.cs#L6) | Identifies whether an assembly can participate in runtime or editor-only dependency graphs. |

### `Inno.Extensibility.Modules.IAssemblyCatalogParticipant`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Extensibility.Modules.IAssemblyCatalogTransaction Inno.Extensibility.Modules.IAssemblyCatalogParticipant.Prepare(Inno.Extensibility.Modules.AssemblyCatalogSnapshot catalog)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IAssemblyCatalogParticipant.cs#L17) | Validates a candidate catalog and prepares state without publishing it. |
| [`Inno.Extensibility.Modules.IAssemblyCatalogParticipant`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IAssemblyCatalogParticipant.cs#L6) | Builds transactional derived state for an assembly catalog generation. |

### `Inno.Extensibility.Modules.IAssemblyCatalogSource`

| 当前声明 | 行为 |
| --- | --- |
| [`System.Action? Inno.Extensibility.Modules.IAssemblyCatalogSource.changed`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IAssemblyCatalogSource.cs#L18) | Notifies the owner that the host assembly snapshot should be refreshed at a safe point. |
| [`System.Collections.Generic.IReadOnlyList<System.Reflection.Assembly> Inno.Extensibility.Modules.IAssemblyCatalogSource.GetAssemblies()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IAssemblyCatalogSource.cs#L25) | Gets the complete host assemblies that participate in extension discovery. |
| [`System.Collections.Generic.IReadOnlyList<System.Reflection.Assembly> Inno.Extensibility.Modules.IAssemblyCatalogSource.GetSharedAssemblies()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IAssemblyCatalogSource.cs#L32) | Gets host contracts and framework assemblies available to candidate modules. |
| [`bool Inno.Extensibility.Modules.IAssemblyCatalogSource.IsFrameworkAssembly(System.Reflection.Assembly assembly)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IAssemblyCatalogSource.cs#L52) | Determines whether an assembly is a framework contract rather than a contributed module. |
| [`bool Inno.Extensibility.Modules.IAssemblyCatalogSource.IsFrameworkReference(string assemblyName)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IAssemblyCatalogSource.cs#L42) | Determines whether a referenced simple name belongs to the selected managed framework. |
| [`Inno.Extensibility.Modules.IAssemblyCatalogSource`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IAssemblyCatalogSource.cs#L13) | Owns the host assembly source independently of any module loading implementation. |

### `Inno.Extensibility.Modules.IAssemblyCatalogTransaction`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Extensibility.Modules.IAssemblyCatalogTransaction.Activate()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IAssemblyCatalogTransaction.cs#L16) | Publishes the prepared candidate state. |
| [`void Inno.Extensibility.Modules.IAssemblyCatalogTransaction.Complete()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IAssemblyCatalogTransaction.cs#L26) | Finalizes an activated state and releases the previous state without performing further publication work. |
| [`void Inno.Extensibility.Modules.IAssemblyCatalogTransaction.Rollback()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IAssemblyCatalogTransaction.cs#L34) | Restores the previous state and releases the candidate state. |
| [`object? Inno.Extensibility.Modules.IAssemblyCatalogTransaction.context`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IAssemblyCatalogTransaction.cs#L11) | Gets an optional short-lived context exposed through . |
| [`Inno.Extensibility.Modules.IAssemblyCatalogTransaction`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IAssemblyCatalogTransaction.cs#L6) | Controls one participant's prepared state during an assembly catalog transaction. |

### `Inno.Extensibility.Modules.IModuleLifetime`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Extensibility.Reload.IAssemblyUnloadProbe Inno.Extensibility.Modules.IModuleLifetime.BeginRetirement()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IModuleLifetime.cs#L21) | Begins retirement once all catalog participants have released the generation. |
| [`Inno.Extensibility.Modules.IModuleLifetime`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IModuleLifetime.cs#L13) | Owns implementation-specific resources acquired for one unpublished or active module generation. |

### `Inno.Extensibility.Modules.IModuleSource`

| 当前声明 | 行为 |
| --- | --- |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Extensibility.Modules.IModuleSource.GetAssemblyNames()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IModuleSource.cs#L45) | Reads the complete owned assembly identities before any generation is acquired. |
| [`Inno.Extensibility.Modules.ModuleCatalogContribution Inno.Extensibility.Modules.IModuleSource.Prepare(Inno.Extensibility.Modules.ModuleSourceContext context)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IModuleSource.cs#L55) | Acquires a validated candidate and transfers its lifetime to the caller on success. |
| [`System.Collections.Generic.IReadOnlyDictionary<string, Inno.Extensibility.Modules.AssemblyScope> Inno.Extensibility.Modules.IModuleSource.assemblyScopes`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IModuleSource.cs#L38) | Gets explicit scope overrides keyed by contributed assembly simple name. |
| [`bool Inno.Extensibility.Modules.IModuleSource.collectible`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IModuleSource.cs#L30) | Gets whether retirement can release this source's code generation. |
| [`Inno.Extensibility.Modules.AssemblyDomain Inno.Extensibility.Modules.IModuleSource.domain`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IModuleSource.cs#L22) | Gets the ownership domain of the contribution. |
| [`string Inno.Extensibility.Modules.IModuleSource.moduleName`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IModuleSource.cs#L18) | Gets the stable logical name used by dependency ordering and replacement transactions. |
| [`Inno.Extensibility.Modules.AssemblyScope Inno.Extensibility.Modules.IModuleSource.scope`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IModuleSource.cs#L26) | Gets the default dependency scope of contributed assemblies. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Extensibility.Modules.IModuleSource.upstreamModuleNames`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IModuleSource.cs#L34) | Gets the explicit logical upstream module names. |
| [`Inno.Extensibility.Modules.IModuleSource`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IModuleSource.cs#L13) | Prepares one module generation without publishing it to the owning catalog. |

### `Inno.Extensibility.Modules.ModuleAssemblyDescriptor`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Extensibility.Modules.ModuleAssemblyDescriptor`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleAssemblyDescriptor.cs#L19) | Describes the ownership classification of one planned assembly without loading its code. |

### `Inno.Extensibility.Modules.ModuleCatalogContribution`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Extensibility.Modules.ModuleCatalogContribution.ModuleCatalogContribution(string moduleName, Inno.Extensibility.Modules.AssemblyDomain domain, Inno.Extensibility.Modules.AssemblyScope scope, System.Collections.Generic.IReadOnlyList<System.Reflection.Assembly> assemblies, System.Collections.Generic.IReadOnlyDictionary<System.Reflection.Assembly, Inno.Extensibility.Modules.AssemblyScope> assemblyScopes, Inno.Extensibility.Modules.IModuleLifetime? lifetime = null)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleCatalogContribution.cs#L39) | Acquires immutable views while transferring the generation lifetime to the contribution. |
| [`System.Collections.Generic.IReadOnlyList<System.Reflection.Assembly> Inno.Extensibility.Modules.ModuleCatalogContribution.assemblies`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleCatalogContribution.cs#L78) | Gets the immutable owned assembly set. |
| [`System.Collections.Generic.IReadOnlyDictionary<System.Reflection.Assembly, Inno.Extensibility.Modules.AssemblyScope> Inno.Extensibility.Modules.ModuleCatalogContribution.assemblyScopes`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleCatalogContribution.cs#L82) | Gets the complete per-assembly scope map. |
| [`Inno.Extensibility.Modules.AssemblyDomain Inno.Extensibility.Modules.ModuleCatalogContribution.domain`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleCatalogContribution.cs#L70) | Gets the ownership domain. |
| [`Inno.Extensibility.Modules.IModuleLifetime? Inno.Extensibility.Modules.ModuleCatalogContribution.lifetime`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleCatalogContribution.cs#L86) | Gets the owned generation lifetime, or null for externally owned code. |
| [`string Inno.Extensibility.Modules.ModuleCatalogContribution.moduleName`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleCatalogContribution.cs#L66) | Gets the stable module name. |
| [`Inno.Extensibility.Modules.AssemblyScope Inno.Extensibility.Modules.ModuleCatalogContribution.scope`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleCatalogContribution.cs#L74) | Gets the default dependency scope. |
| [`Inno.Extensibility.Modules.ModuleCatalogContribution`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleCatalogContribution.cs#L13) | Freezes the assemblies, classification and lifetime of one acquired module generation. |

### `Inno.Extensibility.Modules.ModuleHost`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Extensibility.Modules.ModuleHost.ModuleHost(Inno.Extensibility.Modules.ModuleHostOptions options)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHost.cs#L67) | Creates a module host, discovers assemblies in its owning load context, and publishes the first catalog. |
| [`Inno.Extensibility.Modules.AssemblyReloadSession Inno.Extensibility.Modules.ModuleHost.BeginReload(Inno.Extensibility.Modules.AssemblyModuleHandle module, Inno.Extensibility.Modules.IModuleSource request)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHost.cs#L293) | Stages and validates a replacement generation without publishing it. |
| [`Inno.Extensibility.Modules.AssemblyReloadSession Inno.Extensibility.Modules.ModuleHost.BeginReload(System.Collections.Generic.IReadOnlyList<Inno.Extensibility.Modules.IModuleSource> requests)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHost.cs#L335) | Stages a dependency-ordered set of module additions or replacements as one atomic transaction. Existing modules are matched by their stable module names; an unknown name creates a new module. |
| [`Inno.Extensibility.Modules.AssemblyReloadSession Inno.Extensibility.Modules.ModuleHost.BeginReload(System.Collections.Generic.IReadOnlyList<Inno.Extensibility.Modules.IModuleSource> requests, System.Collections.Generic.IReadOnlyList<string> removedModuleNames)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHost.cs#L364) | Stages additions, replacements, and removals as one atomic dependency-ordered transaction. |
| [`void Inno.Extensibility.Modules.ModuleHost.Dispose()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHost.cs#L509) | Unsubscribes assembly discovery and begins unload of every module owned by this host. |
| [`Inno.Extensibility.Modules.AssemblyModuleHandle Inno.Extensibility.Modules.ModuleHost.Load(Inno.Extensibility.Modules.IModuleSource request)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHost.cs#L171) | Loads and activates a new shadow-copied assembly module. |
| [`void Inno.Extensibility.Modules.ModuleHost.Rebuild()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHost.cs#L485) | Rebuilds the active assembly catalog and every registered derived-state participant. |
| [`void Inno.Extensibility.Modules.ModuleHost.Refresh()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHost.cs#L470) | Applies pending host assembly changes without rebuilding an unchanged catalog. |
| [`Inno.Extensibility.Modules.AssemblyModuleHandle Inno.Extensibility.Modules.ModuleHost.Register(string moduleName, System.Collections.Generic.IReadOnlyList<System.Reflection.Assembly> assemblies)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHost.cs#L218) | Registers assemblies owned by an external load context. |
| [`System.IDisposable Inno.Extensibility.Modules.ModuleHost.RegisterCatalogParticipant(Inno.Extensibility.Modules.IAssemblyCatalogParticipant participant)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHost.cs#L104) | Registers a transactional consumer of the assembly catalog and initializes it from the currently active generation. |
| [`Inno.Extensibility.Reload.IAssemblyUnloadProbe Inno.Extensibility.Modules.ModuleHost.Unload(Inno.Extensibility.Modules.AssemblyModuleHandle module)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHost.cs#L387) | Removes an active module and starts cooperative unload when it is manager-owned. |
| [`Inno.Extensibility.Reload.IAssemblyUnloadProbe Inno.Extensibility.Modules.ModuleHost.Unload(System.Collections.Generic.IReadOnlyList<Inno.Extensibility.Modules.AssemblyModuleHandle> modules)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHost.cs#L425) | Removes several active modules in one catalog transaction, then requests unload in reverse dependency order. |
| [`Inno.Extensibility.Reload.GenerationCoordinator Inno.Extensibility.Modules.ModuleHost.generations`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHost.cs#L42) | Gets the admission gate and retirement owner shared by all module and host generation changes. |
| [`bool Inno.Extensibility.Modules.ModuleHost.isInitialized`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHost.cs#L37) | Gets whether this module host can accept catalog operations. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Extensibility.Modules.AssemblyModuleInfo> Inno.Extensibility.Modules.ModuleHost.modules`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHost.cs#L47) | Gets non-owning information about active managed and external modules. |
| [`Inno.Extensibility.Modules.ModuleHost`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHost.cs#L17) | Owns the active managed assembly catalog and transactional module generations. |

### `Inno.Extensibility.Modules.ModuleHostOptions`

| 当前声明 | 行为 |
| --- | --- |
| [`required Inno.Extensibility.Modules.IAssemblyCatalogSource Inno.Extensibility.Modules.ModuleHostOptions.catalogSource`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHostOptions.cs#L11) | Gets the host source whose ownership transfers to the module host during construction. |
| [`Inno.Extensibility.Modules.ModuleHostOptions`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHostOptions.cs#L6) | Selects the isolated host catalog without prescribing storage for contributed module sources. |

### `Inno.Extensibility.Modules.ModuleSourceContext`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Extensibility.Modules.ModuleSourceContext.TrackRetirement(Inno.Extensibility.Reload.IAssemblyUnloadProbe probe)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleSourceContext.cs#L60) | Transfers a failed acquisition's non-owning retirement probe to the shared generation gate. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Extensibility.Modules.ModuleCatalogContribution> Inno.Extensibility.Modules.ModuleSourceContext.activeModules`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleSourceContext.cs#L49) | Gets active contributions used to reject forbidden downstream references. |
| [`int Inno.Extensibility.Modules.ModuleSourceContext.generation`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleSourceContext.cs#L37) | Gets the candidate generation number. |
| [`Inno.Extensibility.Modules.IAssemblyCatalogSource Inno.Extensibility.Modules.ModuleSourceContext.host`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleSourceContext.cs#L41) | Gets the borrowed host contract source. |
| [`System.Collections.Generic.IReadOnlyDictionary<string, Inno.Extensibility.Modules.ModuleAssemblyDescriptor> Inno.Extensibility.Modules.ModuleSourceContext.plannedAssemblies`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleSourceContext.cs#L53) | Gets identities and classifications for the complete candidate closure. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Extensibility.Modules.ModuleCatalogContribution> Inno.Extensibility.Modules.ModuleSourceContext.upstreamModules`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleSourceContext.cs#L45) | Gets the validated direct upstream contributions for this candidate. |
| [`Inno.Extensibility.Modules.ModuleSourceContext`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleSourceContext.cs#L13) | Supplies a source with the frozen dependencies and retirement boundary of one candidate generation. |

## 项目依赖

- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Core.Collections](../core/Inno.Core.Collections.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Extensibility.Reload](Inno.Extensibility.Reload.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Extensibility.Catalogs](Inno.Extensibility.Catalogs.md)：项目引用；公开签名可见性由语义边界检查确认。

共同 MSBuild 注入的 analyzer 与编译规则属于构建依赖，完整有效项目图记录在本轮验收证据中。
