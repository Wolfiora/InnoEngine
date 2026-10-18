# Inno.Extensibility.Modules

[Extensibility 索引](README.md) · [Wiki 首页](../README.md) · [Types](Inno.Extensibility.Types.md) · [DotNet 来源](../backends/DotNet/Inno.Adapter.Modules.DotNet.md)

## 职责与依赖

Foundation 拥有模块身份、目录快照、依赖排序、候选事务和 generation admission。它通过 `IAssemblyCatalogSource` 与 `IModuleSource` 接收代码；ALC、文件探测、shadow copy 和动态反射属于 DotNet Adapter。本项目依赖 Core.Collections、Core.Execution 与 Extensibility.Reload，不引用具体加载器、Types 或业务领域。

`ModuleHost` 是实例 owner。组合入口必须同时选择程序集来源与对应的类型元数据来源。共享事务适用于静态链接代码与 Editor 动态代码；是否可卸载由来源及其 `IModuleLifetime` 声明。

## 公开契约

| 类型或入口 | 当前行为 |
| --- | --- |
| `ModuleHost(ModuleHostOptions)` | 接管 `catalogSource`、订阅目录变化并发布初始目录；启动失败注销和释放已取得的资源。 |
| `ModuleHostOptions.catalogSource` | 必填目录来源，创建 Host 时转移所有权。Foundation 不隐式创建 ALC 或扫描全进程程序集。 |
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
| `ModuleSourceContext` | 当前候选的 `generation / host / upstreamModules / activeModules / plannedAssemblies`；`TrackRetirement(probe)` 将失败获取的退休责任交给共享 gate。 |
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
using System;
using Inno.Extensibility.Modules;

static void UseModuleHost(
    IAssemblyCatalogSource ownedAssemblySource,
    Action<ModuleHost> consume
) {
    using var modules = new ModuleHost(new ModuleHostOptions
    {
        catalogSource = ownedAssemblySource
    });
    consume(modules);
}
```

类型目录由上层 [Types](Inno.Extensibility.Types.md) 基于该 Host 组合。静态 Player 使用 Runtime 的 `StaticAssemblyCatalogSource / StaticModuleSource / StaticTypeCatalogSource`。类型、序列化与业务 registry 先于 ModuleHost 退休；owner-thread safe point 进行 publication，不在来源里偷偷切换全局状态。

## 失败与退休

获取贡献失败时来源必须释放其资源，必要时用 `TrackRetirement` 登记未完成的释放。候选 Prepare/Activate 失败完整回滚后保留 last-good，只有新来源变化或显式重建才重试。包装在 Aggregate/InnerException 中的 pending retirement 仍属于 pending。

可卸载代码必须经过 Full GC → finalizers → Full GC 与弱 probe 验证，成功前拒绝新的 Reload/Play/Build/Export。超时置 Faulted，保留未退出的 owner 与事务，要求重新启动 Host。静态链接代码的进程生命周期不伪装为 collectible unload。

## 验证入口

`tests/extensibility/Inno.Extensibility.Modules.Tests` 覆盖真实来源、依赖、候选成功/失败、回滚和残留强引用。静态与动态元数据的共同目录行为还由 Types、Runtime 和 Scene 测试验证。






## 本轮边界与所有权

ModuleSourceContext 和 ModuleHostOptions 不携带动态加载磁盘缓存要求。Foundation 管理 catalog、身份、snapshot 与 generation transaction；DotNet source 自己拥有 shadow-copy 根。静态来源不需要动态缓存目录。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Extensibility.Modules.AssemblyCatalogSnapshot`

<!-- api: T:Inno.Extensibility.Modules.AssemblyCatalogSnapshot 3ca61f1e26b89f3ddee5e98c39fca026ba0d34d18772e67328c3f4d1b0d060e4 -->
<!-- api: P:Inno.Extensibility.Modules.AssemblyCatalogSnapshot.version 4de7a87fb557e3f6215fcb8c01ada62b8773c6d2aa53cfee380553d68a05ccfb -->
<!-- api: P:Inno.Extensibility.Modules.AssemblyCatalogSnapshot.assemblies 2b290befc1326e11d7c893b712d53daa20d07532ae21c59c2010a5aa5e946104 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-bc02dc69b9841dea"></a>[`public System.Collections.Generic.IReadOnlyList<System.Reflection.Assembly> Inno.Extensibility.Modules.AssemblyCatalogSnapshot.assemblies { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/AssemblyCatalogSnapshot.cs#L34) | Gets the host and active module assemblies in this generation. | — | — |
| <a id="api-5324f0361f9fd724"></a>[`public long Inno.Extensibility.Modules.AssemblyCatalogSnapshot.version { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/AssemblyCatalogSnapshot.cs#L29) | Gets the monotonically increasing catalog version. | — | — |
| <a id="api-8e422304490c709d"></a>[`public sealed class Inno.Extensibility.Modules.AssemblyCatalogSnapshot`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/AssemblyCatalogSnapshot.cs#L14) | Represents an immutable view of the assemblies participating in one catalog generation. | A snapshot retains its assemblies. Consumers must not keep obsolete snapshots after a reload, because doing so can delay unloading a collectible assembly context. | — |

### `Inno.Extensibility.Modules.AssemblyDomain`

<!-- api: T:Inno.Extensibility.Modules.AssemblyDomain 51a06f4d21c0e734a4024627bb8b25e0785cbb4d62b066fc5e30f19f96eb2211 -->
<!-- api: F:Inno.Extensibility.Modules.AssemblyDomain.InnoInternal 3c0580b32a5c058e5e52c8c556bca4846defcc628a4581447b7c3a332f8020b7 -->
<!-- api: F:Inno.Extensibility.Modules.AssemblyDomain.InnoScripting 8e4ee8a7c729b3e7ba79b7478c57fae0abf12981bcfe7ea801bf5b0fcc99c3a2 -->
<!-- api: F:Inno.Extensibility.Modules.AssemblyDomain.InnoPlugin 6d20de002e841bc8f4580d0c303db8223525c5c20ef32a22d968e84aa1732dd2 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f09d3674d55d4118"></a>[`Inno.Extensibility.Modules.AssemblyDomain.InnoInternal = 0`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Metadata/AssemblyDomain.cs#L11) | The assembly is engine-owned and remains in the default load context. | — | — |
| <a id="api-8bd8cb6f5c2e3e54"></a>[`Inno.Extensibility.Modules.AssemblyDomain.InnoPlugin = 2`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Metadata/AssemblyDomain.cs#L21) | The assembly belongs to the project's unified collectible plugin generation. | — | — |
| <a id="api-4f70614c7e5882cb"></a>[`Inno.Extensibility.Modules.AssemblyDomain.InnoScripting = 1`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Metadata/AssemblyDomain.cs#L16) | The assembly contains project scripting code in a collectible load context. | — | — |
| <a id="api-aae566a2aaa32efd"></a>[`public enum Inno.Extensibility.Modules.AssemblyDomain : int`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Metadata/AssemblyDomain.cs#L6) | Identifies the ownership and reload boundary of a managed assembly. | — | — |

### `Inno.Extensibility.Modules.AssemblyExtensions`

<!-- api: T:Inno.Extensibility.Modules.AssemblyExtensions 0661ccb989a068d2d587d9eae4319631c7d4932a3f249c662a418b6fb6781019 -->
<!-- api: M:Inno.Extensibility.Modules.AssemblyExtensions.GetInnoAssemblyDomain(System.Reflection.Assembly) e0f468e0c8ea50ddba64b7c0921f3208ecea91eec6dd908a85aeadc273d20612 -->
<!-- api: M:Inno.Extensibility.Modules.AssemblyExtensions.GetInnoAssemblyScope(System.Reflection.Assembly) a2b27b27560b623cc952c24982b3f0fe88d92b993766db7abb1c564bfdb8a182 -->
<!-- api: M:Inno.Extensibility.Modules.AssemblyExtensions.TryGetInnoAssemblyClassification(System.Reflection.Assembly,Inno.Extensibility.Modules.AssemblyDomain@,Inno.Extensibility.Modules.AssemblyScope@) 76bad3ea1605966e686275548a782fa3558ae44a3553421370b2b97f0960d88b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-ab49ae17a3f9115c"></a>[`public static Inno.Extensibility.Modules.AssemblyDomain Inno.Extensibility.Modules.AssemblyExtensions.GetInnoAssemblyDomain(System.Reflection.Assembly assembly)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Metadata/AssemblyExtensions.cs#L32) | Resolves the reload domain declared by an assembly.<br>返回：The declared assembly domain.<br>`assembly`：The assembly whose metadata should be inspected. | — | `System.ArgumentNullException`：Thrown when assembly is null.<br>`System.InvalidOperationException`：Thrown when the metadata is absent or invalid. |
| <a id="api-4b39f060e523426e"></a>[`public static Inno.Extensibility.Modules.AssemblyScope Inno.Extensibility.Modules.AssemblyExtensions.GetInnoAssemblyScope(System.Reflection.Assembly assembly)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Metadata/AssemblyExtensions.cs#L55) | Resolves the dependency scope declared by an assembly.<br>返回：The declared assembly scope.<br>`assembly`：The assembly whose metadata should be inspected. | — | `System.ArgumentNullException`：Thrown when assembly is null.<br>`System.InvalidOperationException`：Thrown when the metadata is absent or invalid. |
| <a id="api-f84479558a1b8992"></a>[`public static bool Inno.Extensibility.Modules.AssemblyExtensions.TryGetInnoAssemblyClassification(System.Reflection.Assembly assembly, out Inno.Extensibility.Modules.AssemblyDomain domain, out Inno.Extensibility.Modules.AssemblyScope scope)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Metadata/AssemblyExtensions.cs#L78) | Reads ownership metadata without treating an unrelated assembly as an extension.<br>返回：Whether both classifications are present and valid; outputs are default values on failure.<br>`assembly`：The assembly whose declared classification is inspected.<br>`domain`：Receives the declared domain when metadata is complete.<br>`scope`：Receives the declared dependency scope when metadata is complete. | — | — |
| <a id="api-29bb37f556f472c6"></a>[`public static class Inno.Extensibility.Modules.AssemblyExtensions`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Metadata/AssemblyExtensions.cs#L10) | Provides ownership and scope metadata helpers for assemblies participating in the Inno runtime. | — | — |

### `Inno.Extensibility.Modules.AssemblyModuleHandle`

<!-- api: T:Inno.Extensibility.Modules.AssemblyModuleHandle acfb37792268d134a8c5a8eaa9ae4ff5ad518c17a599459020ae945de32823e9 -->
<!-- api: M:Inno.Extensibility.Modules.AssemblyModuleHandle.#ctor(System.Guid) 56ba3351881bfa3f7ac3d94ff1d7161f85f39381e30aa6e83655b2f0c0c94b01 -->
<!-- api: P:Inno.Extensibility.Modules.AssemblyModuleHandle.id c714428aec4372bc79a7b8573b20308aab89a222443168b1fe8f6620ae6c8aad -->
<!-- api: M:Inno.Extensibility.Modules.AssemblyModuleHandle.ToString ffb0b6056e3a98301bf37bf20a5cf62e5b036906ba68d917433b22d8c16d2852 -->
<!-- api: M:Inno.Extensibility.Modules.AssemblyModuleHandle.op_Inequality(Inno.Extensibility.Modules.AssemblyModuleHandle,Inno.Extensibility.Modules.AssemblyModuleHandle) e9eed3b1d97bf24021b3e880ef3e9b983eaf8b41482b53f9336a678f629d0849 -->
<!-- api: M:Inno.Extensibility.Modules.AssemblyModuleHandle.op_Equality(Inno.Extensibility.Modules.AssemblyModuleHandle,Inno.Extensibility.Modules.AssemblyModuleHandle) 1a6986ee3a0bfa7632c6fc40b6f7526326878405878d0b860311976a5a342239 -->
<!-- api: M:Inno.Extensibility.Modules.AssemblyModuleHandle.GetHashCode 3e2d12eeed2fbdea632657395921744c01d63c1aac238c376813898976257154 -->
<!-- api: M:Inno.Extensibility.Modules.AssemblyModuleHandle.Equals(System.Object) 0f04f3bcd9e7324ef0d7816d8f88bdf0643adc1cfb481ca1708fb70bd91818b8 -->
<!-- api: M:Inno.Extensibility.Modules.AssemblyModuleHandle.Equals(Inno.Extensibility.Modules.AssemblyModuleHandle) 38c02df329fd1cc1015a98ad0e72703fe4accb9383c30e4724a8654b9a75a783 -->
<!-- api: M:Inno.Extensibility.Modules.AssemblyModuleHandle.Deconstruct(System.Guid@) 62daa80c3a359d216f80191e3fafdc02155cbd0015e83c53acbe83b39b4c6c98 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2d852448b29920f4"></a>[`public Inno.Extensibility.Modules.AssemblyModuleHandle.AssemblyModuleHandle(System.Guid id)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleHandle.cs#L11) | Identifies a logical assembly module without retaining its runtime assemblies.<br>`id`：The stable identity used to locate the requested value. | — | — |
| <a id="api-84f8b307d5849c80"></a>[`public void Inno.Extensibility.Modules.AssemblyModuleHandle.Deconstruct(out System.Guid id)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleHandle.cs#L11) | C# record 生成的解构入口，按主构造声明顺序返回位置成员。 | — | — |
| <a id="api-48b2f64c9c5ab86b"></a>[`public bool Inno.Extensibility.Modules.AssemblyModuleHandle.Equals(Inno.Extensibility.Modules.AssemblyModuleHandle other)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleHandle.cs#L11) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-a5a2d18770b3be3a"></a>[`public override bool Inno.Extensibility.Modules.AssemblyModuleHandle.Equals(object obj)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleHandle.cs#L11) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-7b0a5a3b3f7fd67b"></a>[`public override int Inno.Extensibility.Modules.AssemblyModuleHandle.GetHashCode()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleHandle.cs#L11) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-67156e33df66a48c"></a>[`public static bool Inno.Extensibility.Modules.AssemblyModuleHandle.operator ==(Inno.Extensibility.Modules.AssemblyModuleHandle left, Inno.Extensibility.Modules.AssemblyModuleHandle right)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleHandle.cs#L11) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-e90e96e9c12c3a04"></a>[`public static bool Inno.Extensibility.Modules.AssemblyModuleHandle.operator !=(Inno.Extensibility.Modules.AssemblyModuleHandle left, Inno.Extensibility.Modules.AssemblyModuleHandle right)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleHandle.cs#L11) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-c820546c73e6bc2c"></a>[`public override string Inno.Extensibility.Modules.AssemblyModuleHandle.ToString()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleHandle.cs#L11) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-58e0534f919df2e9"></a>[`public System.Guid Inno.Extensibility.Modules.AssemblyModuleHandle.id { get; init; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleHandle.cs#L11) | The stable identity used to locate the requested value. | — | — |
| <a id="api-f76db355c5f2f3b8"></a>[`public readonly record struct Inno.Extensibility.Modules.AssemblyModuleHandle : System.IEquatable<Inno.Extensibility.Modules.AssemblyModuleHandle>`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleHandle.cs#L11) | Identifies a logical assembly module without retaining its runtime assemblies.<br>`id`：The stable identity used to locate the requested value. | — | — |

### `Inno.Extensibility.Modules.AssemblyModuleInfo`

<!-- api: T:Inno.Extensibility.Modules.AssemblyModuleInfo 792fc44422937b76a1711466b2abf3974bf160ce2897116950e968dc49abe4c1 -->
<!-- api: M:Inno.Extensibility.Modules.AssemblyModuleInfo.#ctor(Inno.Extensibility.Modules.AssemblyModuleHandle,System.String,System.Int32,System.Boolean,System.Boolean,Inno.Extensibility.Modules.AssemblyDomain,Inno.Extensibility.Modules.AssemblyScope,Inno.Extensibility.Modules.AssemblyModuleStatus,System.Collections.Generic.IReadOnlyList{System.String}) 8b296cf174ac33b160092d3e08371d6d956543939d0ab220ea29fa1864e92130 -->
<!-- api: P:Inno.Extensibility.Modules.AssemblyModuleInfo.handle 69146e3f2d18eab406c50caecc7bbcc8aa8e981e01a823213a82012e1e44a5b9 -->
<!-- api: P:Inno.Extensibility.Modules.AssemblyModuleInfo.moduleName 6941b13a989f44dac4191f8ffcfef47ea50b406c03e03950da3e83a405ec96d0 -->
<!-- api: P:Inno.Extensibility.Modules.AssemblyModuleInfo.generation 566dfe95739f78ac4cfce17870f596cca4bf7c46b8a2e3f8232575662bc50933 -->
<!-- api: P:Inno.Extensibility.Modules.AssemblyModuleInfo.collectible e392ddbc6bcd8f1e82f73b473ed7131be84947b80a17fb7e5e7c9452312e26bd -->
<!-- api: P:Inno.Extensibility.Modules.AssemblyModuleInfo.externallyOwned 9da3068d5dca52cd1d64a8d23a06164c9d5d5cf536269505b9ca4911106592e7 -->
<!-- api: P:Inno.Extensibility.Modules.AssemblyModuleInfo.domain 79c73dad7535f6b35b329dd81a77d957bc5168a9e1848e1fc479a5734c68596d -->
<!-- api: P:Inno.Extensibility.Modules.AssemblyModuleInfo.scope 4abdcb39c0fc07176ff1d5d3b44ce8ea735e31f92c9db595d1047919b904d3c4 -->
<!-- api: P:Inno.Extensibility.Modules.AssemblyModuleInfo.status c326dbe05493dfc83aaca0caf4803e60161fa2ce625d05c3e661b606d15200e4 -->
<!-- api: P:Inno.Extensibility.Modules.AssemblyModuleInfo.assemblyNames ac43f4c7eb4613d4faea856ddee15c41c686f4f3ba9068c9275355e9cacd6ece -->
<!-- api: P:Inno.Extensibility.Modules.AssemblyModuleInfo.upstreamModuleNames 3f83cf5607e17557e1cb2edfff46fc95239428a271be5ae7570e1fcc7b5b4ff7 -->
<!-- api: M:Inno.Extensibility.Modules.AssemblyModuleInfo.ToString d64ebbd35d5b557d9d6f7705ea71143b5bbda053f8a9270bec7fbf61063ab7c7 -->
<!-- api: M:Inno.Extensibility.Modules.AssemblyModuleInfo.op_Inequality(Inno.Extensibility.Modules.AssemblyModuleInfo,Inno.Extensibility.Modules.AssemblyModuleInfo) 213aafaad57ccdba7c6d0d6235ef8dbc7dcede39c36df1fd69e460e7da55e4e6 -->
<!-- api: M:Inno.Extensibility.Modules.AssemblyModuleInfo.op_Equality(Inno.Extensibility.Modules.AssemblyModuleInfo,Inno.Extensibility.Modules.AssemblyModuleInfo) 80bea7fa5b85275aa9b63a7e8ecb7446b0b02969818f83cdf5660db8e80b2143 -->
<!-- api: M:Inno.Extensibility.Modules.AssemblyModuleInfo.GetHashCode 12704d9d618eb3d2bf19055042aca7f14e54df1aef5c4e44b3ebb2b59ee47bb1 -->
<!-- api: M:Inno.Extensibility.Modules.AssemblyModuleInfo.Equals(System.Object) 046784b76b23446366c7c3388c486102dde3e9742cb07066cf1f0f328be65bca -->
<!-- api: M:Inno.Extensibility.Modules.AssemblyModuleInfo.Equals(Inno.Extensibility.Modules.AssemblyModuleInfo) f66c5d3bb160872c7c401a2696f0da0983dcd64aa59349379bc9ec0edc8ac8e5 -->
<!-- api: M:Inno.Extensibility.Modules.AssemblyModuleInfo.Deconstruct(Inno.Extensibility.Modules.AssemblyModuleHandle@,System.String@,System.Int32@,System.Boolean@,System.Boolean@,Inno.Extensibility.Modules.AssemblyDomain@,Inno.Extensibility.Modules.AssemblyScope@,Inno.Extensibility.Modules.AssemblyModuleStatus@,System.Collections.Generic.IReadOnlyList{System.String}@) 5c528f566713bbd358c5730c563816a49b9c76d8e840c18e444fd0066df649f1 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-bf7dddd6689a9398"></a>[`public Inno.Extensibility.Modules.AssemblyModuleInfo.AssemblyModuleInfo(Inno.Extensibility.Modules.AssemblyModuleHandle handle, string moduleName, int generation, bool collectible, bool externallyOwned, Inno.Extensibility.Modules.AssemblyDomain domain, Inno.Extensibility.Modules.AssemblyScope scope, Inno.Extensibility.Modules.AssemblyModuleStatus status, System.Collections.Generic.IReadOnlyList<string> assemblyNames)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleInfo.cs#L47) | Provides non-owning diagnostic information about an active assembly module.<br>`handle`：The opaque handle validated by this operation.<br>`moduleName`：The string value used to initialize this instance.<br>`generation`：The owner generation used to reject stale handles or snapshots.<br>`collectible`：The bool value used to initialize this instance.<br>`externallyOwned`：The bool value used to initialize this instance.<br>`domain`：The assembly domain value used to initialize this instance.<br>`scope`：The assembly scope value used to initialize this instance.<br>`status`：The assembly module status value used to initialize this instance.<br>`assemblyNames`：The immutable assembly names exposed for this active module generation. | — | — |
| <a id="api-7f27f4f4350ef92d"></a>[`public void Inno.Extensibility.Modules.AssemblyModuleInfo.Deconstruct(out Inno.Extensibility.Modules.AssemblyModuleHandle handle, out string moduleName, out int generation, out bool collectible, out bool externallyOwned, out Inno.Extensibility.Modules.AssemblyDomain domain, out Inno.Extensibility.Modules.AssemblyScope scope, out Inno.Extensibility.Modules.AssemblyModuleStatus status, out System.Collections.Generic.IReadOnlyList<string> assemblyNames)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleInfo.cs#L47) | C# record 生成的解构入口，按主构造声明顺序返回位置成员。 | — | — |
| <a id="api-e6640c7fd31d00c5"></a>[`public bool Inno.Extensibility.Modules.AssemblyModuleInfo.Equals(Inno.Extensibility.Modules.AssemblyModuleInfo? other)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleInfo.cs#L47) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-10b659eeda9874f2"></a>[`public override bool Inno.Extensibility.Modules.AssemblyModuleInfo.Equals(object? obj)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleInfo.cs#L47) | Determines whether the specified object is equal to the current object.<br>返回：true if the specified object is equal to the current object; otherwise, false.<br>`obj`：The object to compare with the current object. | — | — |
| <a id="api-6f52c0622da93f8f"></a>[`public override int Inno.Extensibility.Modules.AssemblyModuleInfo.GetHashCode()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleInfo.cs#L47) | Serves as the default hash function.<br>返回：A hash code for the current object. | — | — |
| <a id="api-e511089a0f57b1b7"></a>[`public static bool Inno.Extensibility.Modules.AssemblyModuleInfo.operator ==(Inno.Extensibility.Modules.AssemblyModuleInfo? left, Inno.Extensibility.Modules.AssemblyModuleInfo? right)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleInfo.cs#L47) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-af29b8add2c2ca0e"></a>[`public static bool Inno.Extensibility.Modules.AssemblyModuleInfo.operator !=(Inno.Extensibility.Modules.AssemblyModuleInfo? left, Inno.Extensibility.Modules.AssemblyModuleInfo? right)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleInfo.cs#L47) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-c306bcc43d077240"></a>[`public override string Inno.Extensibility.Modules.AssemblyModuleInfo.ToString()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleInfo.cs#L47) | Returns a string that represents the current object.<br>返回：A string that represents the current object. | — | — |
| <a id="api-7773fb87ccf32eef"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Extensibility.Modules.AssemblyModuleInfo.assemblyNames { get; init; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleInfo.cs#L56) | The immutable assembly names exposed for this active module generation. | — | — |
| <a id="api-93b6e2aae2e6caba"></a>[`public bool Inno.Extensibility.Modules.AssemblyModuleInfo.collectible { get; init; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleInfo.cs#L51) | The bool value used to initialize this instance. | — | — |
| <a id="api-90c5d71d3511f83f"></a>[`public Inno.Extensibility.Modules.AssemblyDomain Inno.Extensibility.Modules.AssemblyModuleInfo.domain { get; init; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleInfo.cs#L53) | The assembly domain value used to initialize this instance. | — | — |
| <a id="api-4a3f8ca0b1ae887e"></a>[`public bool Inno.Extensibility.Modules.AssemblyModuleInfo.externallyOwned { get; init; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleInfo.cs#L52) | The bool value used to initialize this instance. | — | — |
| <a id="api-517fd422d6b53a24"></a>[`public int Inno.Extensibility.Modules.AssemblyModuleInfo.generation { get; init; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleInfo.cs#L50) | The owner generation used to reject stale handles or snapshots. | — | — |
| <a id="api-c3c00b378e943886"></a>[`public Inno.Extensibility.Modules.AssemblyModuleHandle Inno.Extensibility.Modules.AssemblyModuleInfo.handle { get; init; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleInfo.cs#L48) | The opaque handle validated by this operation. | — | — |
| <a id="api-ec74cb25871591b6"></a>[`public string Inno.Extensibility.Modules.AssemblyModuleInfo.moduleName { get; init; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleInfo.cs#L49) | The string value used to initialize this instance. | — | — |
| <a id="api-b382459227b522b1"></a>[`public Inno.Extensibility.Modules.AssemblyScope Inno.Extensibility.Modules.AssemblyModuleInfo.scope { get; init; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleInfo.cs#L54) | The assembly scope value used to initialize this instance. | — | — |
| <a id="api-11d6f38f5763ad1e"></a>[`public Inno.Extensibility.Modules.AssemblyModuleStatus Inno.Extensibility.Modules.AssemblyModuleInfo.status { get; init; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleInfo.cs#L55) | The assembly module status value used to initialize this instance. | — | — |
| <a id="api-ccafc51b3117b11b"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Extensibility.Modules.AssemblyModuleInfo.upstreamModuleNames { get; init; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleInfo.cs#L61) | Gets stable module dependencies used by this active generation. | — | — |
| <a id="api-19cf03aa2fb1cee4"></a>[`public sealed record Inno.Extensibility.Modules.AssemblyModuleInfo : System.IEquatable<Inno.Extensibility.Modules.AssemblyModuleInfo>`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleInfo.cs#L47) | Provides non-owning diagnostic information about an active assembly module.<br>`handle`：The opaque handle validated by this operation.<br>`moduleName`：The string value used to initialize this instance.<br>`generation`：The owner generation used to reject stale handles or snapshots.<br>`collectible`：The bool value used to initialize this instance.<br>`externallyOwned`：The bool value used to initialize this instance.<br>`domain`：The assembly domain value used to initialize this instance.<br>`scope`：The assembly scope value used to initialize this instance.<br>`status`：The assembly module status value used to initialize this instance.<br>`assemblyNames`：The immutable assembly names exposed for this active module generation. | — | — |

### `Inno.Extensibility.Modules.AssemblyModuleStatus`

<!-- api: T:Inno.Extensibility.Modules.AssemblyModuleStatus 540053300ae6a78ebce21a4c4439f9f076a893880f775b81b488fd194d8f8d83 -->
<!-- api: F:Inno.Extensibility.Modules.AssemblyModuleStatus.Active 3b8d78c3d4132a01245d3fe32e1a20b995e2ca0c24ce9b06499d05baeb17c4e5 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9b6437e2807a77fd"></a>[`Inno.Extensibility.Modules.AssemblyModuleStatus.Active = 0`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleInfo.cs#L14) | The module generation is visible to assembly catalog participants. | — | — |
| <a id="api-48d205f051a732b2"></a>[`public enum Inno.Extensibility.Modules.AssemblyModuleStatus : int`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Modules/AssemblyModuleInfo.cs#L9) | Describes the catalog state of a published assembly module generation. | — | — |

### `Inno.Extensibility.Modules.AssemblyReloadContext`

<!-- api: T:Inno.Extensibility.Modules.AssemblyReloadContext 0df6b3498b7f7736df148f364b41102fd61cacacb14a29555f3f8c952f5f8a59 -->
<!-- api: P:Inno.Extensibility.Modules.AssemblyReloadContext.previousCatalog 74ab3ab819b3a51bbfb155d4793494c72076c802915067eddd1fed8e334216c5 -->
<!-- api: P:Inno.Extensibility.Modules.AssemblyReloadContext.candidateCatalog ca398aaef1643423f5df6bb4f51508c8ffa25469d26b9c85fadf53c51b08366a -->
<!-- api: P:Inno.Extensibility.Modules.AssemblyReloadContext.module b929be4a508d6f0a8ef09002f323e6a0ffc3a67063af4d62d58def278714bc6a -->
<!-- api: P:Inno.Extensibility.Modules.AssemblyReloadContext.modules 6eac2fabfc45f6b86b580ecd6c9163b51bbb720b254033b4d840ee194fc0a9ac -->
<!-- api: M:Inno.Extensibility.Modules.AssemblyReloadContext.GetContext``1 bf0d663e82338b769d78df59ed1554ad3af25ff1da3f636886699339678ab156 -->
<!-- api: M:Inno.Extensibility.Modules.AssemblyReloadContext.TryGetContext``1(``0@) 3ab6d61eea7eecc17a9ea4cdc6c8dd8a84bf53d029938127955429228e4b3351 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4495630931d29997"></a>[`public TContext Inno.Extensibility.Modules.AssemblyReloadContext.GetContext<TContext>() where TContext : class`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Reloading/AssemblyReloadContext.cs#L59) | Gets a context contributed by a registered catalog participant.<br>返回：The matching context. | — | `System.InvalidOperationException`：Thrown when no participant supplied the requested context or the reload has completed. |
| <a id="api-689239c1a139b139"></a>[`public bool Inno.Extensibility.Modules.AssemblyReloadContext.TryGetContext<TContext>(out TContext? context) where TContext : class`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Reloading/AssemblyReloadContext.cs#L79) | Tries to get a context contributed by a registered catalog participant.<br>返回：true when exactly one matching context exists.<br>`context`：Receives the matching context when found. | — | — |
| <a id="api-3082e6d823f7aae4"></a>[`public Inno.Extensibility.Modules.AssemblyCatalogSnapshot Inno.Extensibility.Modules.AssemblyReloadContext.candidateCatalog { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Reloading/AssemblyReloadContext.cs#L35) | Gets the validated candidate assembly catalog. | — | — |
| <a id="api-aeb0a73c29f1df0d"></a>[`public Inno.Extensibility.Modules.AssemblyModuleHandle Inno.Extensibility.Modules.AssemblyReloadContext.module { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Reloading/AssemblyReloadContext.cs#L40) | Gets the first logical module in dependency staging order. | — | — |
| <a id="api-9dd4793a84c29138"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Extensibility.Modules.AssemblyModuleHandle> Inno.Extensibility.Modules.AssemblyReloadContext.modules { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Reloading/AssemblyReloadContext.cs#L45) | Gets every logical module staged by this atomic reload transaction. | — | — |
| <a id="api-86261a4195723cd5"></a>[`public Inno.Extensibility.Modules.AssemblyCatalogSnapshot Inno.Extensibility.Modules.AssemblyReloadContext.previousCatalog { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Reloading/AssemblyReloadContext.cs#L30) | Gets the assembly catalog from before activation. | — | — |
| <a id="api-a85ecb9ac4362bb0"></a>[`public sealed class Inno.Extensibility.Modules.AssemblyReloadContext`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Reloading/AssemblyReloadContext.cs#L9) | Provides assembly catalogs and participant-specific state during a reload transaction. | — | — |

### `Inno.Extensibility.Modules.AssemblyReloadSession`

<!-- api: T:Inno.Extensibility.Modules.AssemblyReloadSession 9e95336c23491c1d91bcb843d18904548399341a3f705f935ff938c1b0934cde -->
<!-- api: P:Inno.Extensibility.Modules.AssemblyReloadSession.context 819f5a9cedfdae86e62f9c4cb58a5a3d13ef1abf48cf8fba9cdb666c4b521efd -->
<!-- api: P:Inno.Extensibility.Modules.AssemblyReloadSession.generations efa3c301276e4438951f2e76b0c73e9e9e7de8d0937bc78a3ac52cb3c54a7a0a -->
<!-- api: M:Inno.Extensibility.Modules.AssemblyReloadSession.Activate 1021c59a002f8281817cdfe785fa3573de3ff81f2b7bb6c6ea0ae71fdb53f836 -->
<!-- api: M:Inno.Extensibility.Modules.AssemblyReloadSession.Complete ff0e0d6bd6d64f0b9c87f2cfb64e0f05ea51c8c79ec62dbd88e129b29579b05a -->
<!-- api: M:Inno.Extensibility.Modules.AssemblyReloadSession.Rollback 006eed67196f4ae11158027664bce2ac3b089c372395631d0601870a35ca70ad -->
<!-- api: M:Inno.Extensibility.Modules.AssemblyReloadSession.Dispose 818f0f0cefee417a6aa7069b39b06453816712fbdb6b7dd87cad4c1c7d005e04 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4072590cb7fde4ef"></a>[`public void Inno.Extensibility.Modules.AssemblyReloadSession.Activate()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Reloading/AssemblyReloadSession.cs#L48) | Atomically publishes the candidate module and all participant snapshots. | — | `Inno.Core.Execution.RetirementPendingException`：A participant could not quiesce. Both generations remain owned and the host must restart. |
| <a id="api-8bfc29e629dcb14d"></a>[`public Inno.Extensibility.Reload.IAssemblyUnloadProbe Inno.Extensibility.Modules.AssemblyReloadSession.Complete()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Reloading/AssemblyReloadSession.cs#L64) | Commits an activated reload and begins cooperative unload of the old generation.<br>返回：The validated assembly unload monitor that represents the completed operation. | — | `Inno.Core.Execution.RetirementPendingException`：A participant still uses its dependencies. Contexts remain owned and no unload monitor is issued. |
| <a id="api-d9beac1a9e53fdd7"></a>[`public void Inno.Extensibility.Modules.AssemblyReloadSession.Dispose()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Reloading/AssemblyReloadSession.cs#L125) | Rolls back an incomplete session. | — | `Inno.Core.Execution.RetirementPendingException`：The session still owns unfinished retirement and cannot be marked disposed. |
| <a id="api-1baa62fd19253ab9"></a>[`public void Inno.Extensibility.Modules.AssemblyReloadSession.Rollback()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Reloading/AssemblyReloadSession.cs#L94) | Restores the previous generation and unloads the candidate generation. | — | `Inno.Core.Execution.RetirementPendingException`：Rollback cannot safely release the candidate. Contexts remain owned and the shared gate is faulted. |
| <a id="api-0826b071f9debe46"></a>[`public Inno.Extensibility.Modules.AssemblyReloadContext Inno.Extensibility.Modules.AssemblyReloadSession.context { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Reloading/AssemblyReloadSession.cs#L35) | Gets the old and candidate assembly catalogs and participant migration contexts. | — | — |
| <a id="api-715a2c7ae3f81f7b"></a>[`public Inno.Extensibility.Reload.GenerationCoordinator Inno.Extensibility.Modules.AssemblyReloadSession.generations { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Reloading/AssemblyReloadSession.cs#L40) | Gets the owning host's shared generation gate, including discarded-candidate retirements. | — | — |
| <a id="api-928c369d945233ee"></a>[`public sealed class Inno.Extensibility.Modules.AssemblyReloadSession : System.IDisposable, Inno.Extensibility.Reload.IGenerationPublication<Inno.Extensibility.Reload.IAssemblyUnloadProbe>`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Reloading/AssemblyReloadSession.cs#L12) | Controls activation, completion, and rollback of one prepared module generation. | — | — |

### `Inno.Extensibility.Modules.AssemblyScope`

<!-- api: T:Inno.Extensibility.Modules.AssemblyScope 828ae2bb37b57b76101316cf60e67f143ce7fb6018e58c75e0835de10295fcc9 -->
<!-- api: F:Inno.Extensibility.Modules.AssemblyScope.Runtime a0a8948833416a3c770b1ec52e214352a5520d1caa14196865f23062f13dfc7a -->
<!-- api: F:Inno.Extensibility.Modules.AssemblyScope.Editor b4cc2e4d49927be15516650095e24644592c4d78719c8a7dea30957ad34359e8 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7a128b45a9fb9368"></a>[`Inno.Extensibility.Modules.AssemblyScope.Editor = 1`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Metadata/AssemblyScope.cs#L16) | The assembly is available only to editor consumers. | — | — |
| <a id="api-f0935c41651407db"></a>[`Inno.Extensibility.Modules.AssemblyScope.Runtime = 0`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Metadata/AssemblyScope.cs#L11) | The assembly is available to runtime and editor consumers. | — | — |
| <a id="api-c3c4b0de77ae21c4"></a>[`public enum Inno.Extensibility.Modules.AssemblyScope : int`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Metadata/AssemblyScope.cs#L6) | Identifies whether an assembly can participate in runtime or editor-only dependency graphs. | — | — |

### `Inno.Extensibility.Modules.IAssemblyCatalogParticipant`

<!-- api: T:Inno.Extensibility.Modules.IAssemblyCatalogParticipant 65283a948134574abba62494268e28e5954240d464937098a62cfe46fdf53957 -->
<!-- api: M:Inno.Extensibility.Modules.IAssemblyCatalogParticipant.Prepare(Inno.Extensibility.Modules.AssemblyCatalogSnapshot) 7fc42b1200fe4894a7116c4a3a2f9ef4770465b9c13b45cbf2940e6254fdd455 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d6b69ca08d091a4d"></a>[`Inno.Extensibility.Modules.IAssemblyCatalogTransaction Inno.Extensibility.Modules.IAssemblyCatalogParticipant.Prepare(Inno.Extensibility.Modules.AssemblyCatalogSnapshot catalog)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IAssemblyCatalogParticipant.cs#L17) | Validates a candidate catalog and prepares state without publishing it.<br>返回：A transaction controlling publication, completion, and rollback.<br>`catalog`：The complete candidate assembly catalog. | — | — |
| <a id="api-11bf60feb42a33f6"></a>[`public interface Inno.Extensibility.Modules.IAssemblyCatalogParticipant`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IAssemblyCatalogParticipant.cs#L6) | Builds transactional derived state for an assembly catalog generation. | — | — |

### `Inno.Extensibility.Modules.IAssemblyCatalogSource`

<!-- api: T:Inno.Extensibility.Modules.IAssemblyCatalogSource 40731b14c0615f33b250fbbee7832b1e6cba34ed1bf32f4627df4ccfd0868efc -->
<!-- api: E:Inno.Extensibility.Modules.IAssemblyCatalogSource.changed fc823798152036efd8602b678bf9f0819a0d8020531fd298b8c72f94c96a00bc -->
<!-- api: M:Inno.Extensibility.Modules.IAssemblyCatalogSource.GetAssemblies feadbead5da99338bd82c99769a526520f80790304a7ce0319b1673923cde575 -->
<!-- api: M:Inno.Extensibility.Modules.IAssemblyCatalogSource.GetSharedAssemblies d52c028f528bd3481a953b6b3c4f0c2bf51d8eeed1a23d0ee34fc952c45a4448 -->
<!-- api: M:Inno.Extensibility.Modules.IAssemblyCatalogSource.IsFrameworkReference(System.String) 2879fe8caf86af55a64fa08e11be2bccae4fe0840e415110b5d3e6cdc88109f5 -->
<!-- api: M:Inno.Extensibility.Modules.IAssemblyCatalogSource.IsFrameworkAssembly(System.Reflection.Assembly) 1d47cb4349e17acad257cb5b607e821e362870ad5f79aff2b31a8691a9b0fe74 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1c6f10e3c32c0d9d"></a>[`System.Action? Inno.Extensibility.Modules.IAssemblyCatalogSource.changed`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IAssemblyCatalogSource.cs#L18) | Notifies the owner that the host assembly snapshot should be refreshed at a safe point. | — | — |
| <a id="api-d1468838cbc1885b"></a>[`System.Collections.Generic.IReadOnlyList<System.Reflection.Assembly> Inno.Extensibility.Modules.IAssemblyCatalogSource.GetAssemblies()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IAssemblyCatalogSource.cs#L25) | Gets the complete host assemblies that participate in extension discovery.<br>返回：The current host snapshot; the caller must not retain it beyond its catalog generation. | — | — |
| <a id="api-99c239d44b54fb9c"></a>[`System.Collections.Generic.IReadOnlyList<System.Reflection.Assembly> Inno.Extensibility.Modules.IAssemblyCatalogSource.GetSharedAssemblies()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IAssemblyCatalogSource.cs#L32) | Gets host contracts and framework assemblies available to candidate modules.<br>返回：A snapshot of shareable assemblies owned by the host implementation. | — | — |
| <a id="api-9dd1d70417ea84a4"></a>[`bool Inno.Extensibility.Modules.IAssemblyCatalogSource.IsFrameworkAssembly(System.Reflection.Assembly assembly)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IAssemblyCatalogSource.cs#L52) | Determines whether an assembly is a framework contract rather than a contributed module.<br>返回：Whether the assembly is owned by the selected managed framework.<br>`assembly`：The candidate shared assembly. | — | — |
| <a id="api-f89f48b9c5187baa"></a>[`bool Inno.Extensibility.Modules.IAssemblyCatalogSource.IsFrameworkReference(string assemblyName)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IAssemblyCatalogSource.cs#L42) | Determines whether a referenced simple name belongs to the selected managed framework.<br>返回：Whether the framework owns this dependency.<br>`assemblyName`：The referenced managed assembly simple name. | — | — |
| <a id="api-05db74c12661c579"></a>[`public interface Inno.Extensibility.Modules.IAssemblyCatalogSource : System.IDisposable`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IAssemblyCatalogSource.cs#L13) | Owns the host assembly source independently of any module loading implementation. | — | — |

### `Inno.Extensibility.Modules.IAssemblyCatalogTransaction`

<!-- api: T:Inno.Extensibility.Modules.IAssemblyCatalogTransaction 8600287a6d5c182cdb4fd12c9d096e4ff9ac153748a8e534b869e335fe793ee3 -->
<!-- api: P:Inno.Extensibility.Modules.IAssemblyCatalogTransaction.context 9952729a6dfb2aad85ac16e4b63f903cc42851efd263c5e489eb54b80398ade3 -->
<!-- api: M:Inno.Extensibility.Modules.IAssemblyCatalogTransaction.Activate ef97abdf2e057e583fbba9aeb9f73c68e8c6dc58328910206b0196b5fbafc404 -->
<!-- api: M:Inno.Extensibility.Modules.IAssemblyCatalogTransaction.Complete 02daa23240fbac71671b25f9a70d44f1492d8b481a1d0fc43191f9e2cb86c16c -->
<!-- api: M:Inno.Extensibility.Modules.IAssemblyCatalogTransaction.Rollback ad6252e9aa3db4fdda8ebd926be0cd894ac4368e4e23e62b1071652602390391 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-49639abf2eb336f5"></a>[`void Inno.Extensibility.Modules.IAssemblyCatalogTransaction.Activate()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IAssemblyCatalogTransaction.cs#L16) | Publishes the prepared candidate state. | — | — |
| <a id="api-51e8c1f9ba0fe65d"></a>[`void Inno.Extensibility.Modules.IAssemblyCatalogTransaction.Complete()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IAssemblyCatalogTransaction.cs#L26) | Finalizes an activated state and releases the previous state without performing further publication work. | Ordinary cleanup failures must remain observable after all independent owners have been attempted. The coordinator faults further generation admission instead of ignoring the failure or pretending publication can roll back. Unfinished retirement must retain its owners and propagate the retirement barrier before releasing their dependencies. | — |
| <a id="api-234e48ee211e82ba"></a>[`void Inno.Extensibility.Modules.IAssemblyCatalogTransaction.Rollback()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IAssemblyCatalogTransaction.cs#L34) | Restores the previous state and releases the candidate state. | Implementations should isolate individual cleanup failures so every candidate resource can be released. | — |
| <a id="api-db5181af9cb60f79"></a>[`object? Inno.Extensibility.Modules.IAssemblyCatalogTransaction.context { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IAssemblyCatalogTransaction.cs#L11) | Gets an optional short-lived context exposed through Inno.Extensibility.Modules.AssemblyReloadContext. | — | — |
| <a id="api-eb6f05b08f38182f"></a>[`public interface Inno.Extensibility.Modules.IAssemblyCatalogTransaction`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IAssemblyCatalogTransaction.cs#L6) | Controls one participant's prepared state during an assembly catalog transaction. | — | — |

### `Inno.Extensibility.Modules.IModuleLifetime`

<!-- api: T:Inno.Extensibility.Modules.IModuleLifetime cf19fd21f6275883520ca1fd2f4db44934d5c1147e39b124cc30a882be90c4fb -->
<!-- api: M:Inno.Extensibility.Modules.IModuleLifetime.BeginRetirement b12a064828a7985b0d62a2f208ca94015952d1bd5f2afe7936800f25e8c2c047 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-019cc51017bce752"></a>[`Inno.Extensibility.Reload.IAssemblyUnloadProbe Inno.Extensibility.Modules.IModuleLifetime.BeginRetirement()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IModuleLifetime.cs#L21) | Begins retirement once all catalog participants have released the generation.<br>返回：A non-owning probe that completes only after code and generation storage are released. | — | — |
| <a id="api-3594af410d394861"></a>[`public interface Inno.Extensibility.Modules.IModuleLifetime`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IModuleLifetime.cs#L13) | Owns implementation-specific resources acquired for one unpublished or active module generation. | — | — |

### `Inno.Extensibility.Modules.IModuleSource`

<!-- api: T:Inno.Extensibility.Modules.IModuleSource 789591045b610f8aed9f8ddf822efca7cd0c60720719adfd7fcdf7857f48a8b1 -->
<!-- api: P:Inno.Extensibility.Modules.IModuleSource.moduleName 709d91a9d1d6033a839127d2900f52a0b83a74c8bc299012021979842ca5444a -->
<!-- api: P:Inno.Extensibility.Modules.IModuleSource.domain 9b468f677bd78df08080796a1a330e0b9855e0b57bcd13dcbf25f2cd7eb599cd -->
<!-- api: P:Inno.Extensibility.Modules.IModuleSource.scope 8974ad3f5e15ce5315f37c57d89f13b00bf93399d77d0af1e724a31e8c2a595c -->
<!-- api: P:Inno.Extensibility.Modules.IModuleSource.collectible ab2467b17b6bbb6a5548c88be8a4772ee2e1fc0196d197cd3ce48868bf8ecc66 -->
<!-- api: P:Inno.Extensibility.Modules.IModuleSource.upstreamModuleNames fe1dfcd3fc128b7622a41b39b136959e588f427f9436839050b44e7db4e2bc0b -->
<!-- api: P:Inno.Extensibility.Modules.IModuleSource.assemblyScopes bf41f12b54e8574d18dde589ca3a70863e0c8f15d7741831702de6d2d63e78d6 -->
<!-- api: M:Inno.Extensibility.Modules.IModuleSource.GetAssemblyNames 6b6968c55ad5b753866fbfc56cd4d813b46627398ef592fe908d4c9d662d5071 -->
<!-- api: M:Inno.Extensibility.Modules.IModuleSource.Prepare(Inno.Extensibility.Modules.ModuleSourceContext) ef4a35f9cea3b867fb972d1206140cd97c6b8e03a7b2e3be789ad05f3ce91326 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f3cf8bff91d4c102"></a>[`System.Collections.Generic.IReadOnlyList<string> Inno.Extensibility.Modules.IModuleSource.GetAssemblyNames()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IModuleSource.cs#L45) | Reads the complete owned assembly identities before any generation is acquired.<br>返回：Distinct, nonempty simple names; invalid or unavailable source data causes an exception. | — | — |
| <a id="api-74f937d662853ef8"></a>[`Inno.Extensibility.Modules.ModuleCatalogContribution Inno.Extensibility.Modules.IModuleSource.Prepare(Inno.Extensibility.Modules.ModuleSourceContext context)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IModuleSource.cs#L55) | Acquires a validated candidate and transfers its lifetime to the caller on success.<br>返回：A complete unpublished contribution; failure retires every acquired resource through the context.<br>`context`：The immutable candidate dependency closure and retirement owner. | — | — |
| <a id="api-90626979d81babce"></a>[`System.Collections.Generic.IReadOnlyDictionary<string, Inno.Extensibility.Modules.AssemblyScope> Inno.Extensibility.Modules.IModuleSource.assemblyScopes { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IModuleSource.cs#L38) | Gets explicit scope overrides keyed by contributed assembly simple name. | — | — |
| <a id="api-94d66b2f9dd0d61f"></a>[`bool Inno.Extensibility.Modules.IModuleSource.collectible { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IModuleSource.cs#L30) | Gets whether retirement can release this source's code generation. | — | — |
| <a id="api-af6341f3bdc5f8c6"></a>[`Inno.Extensibility.Modules.AssemblyDomain Inno.Extensibility.Modules.IModuleSource.domain { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IModuleSource.cs#L22) | Gets the ownership domain of the contribution. | — | — |
| <a id="api-a4efa3d214f65f07"></a>[`string Inno.Extensibility.Modules.IModuleSource.moduleName { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IModuleSource.cs#L18) | Gets the stable logical name used by dependency ordering and replacement transactions. | — | — |
| <a id="api-081ae2c9a58b59b1"></a>[`Inno.Extensibility.Modules.AssemblyScope Inno.Extensibility.Modules.IModuleSource.scope { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IModuleSource.cs#L26) | Gets the default dependency scope of contributed assemblies. | — | — |
| <a id="api-8537f73b9775a921"></a>[`System.Collections.Generic.IReadOnlyList<string> Inno.Extensibility.Modules.IModuleSource.upstreamModuleNames { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IModuleSource.cs#L34) | Gets the explicit logical upstream module names. | — | — |
| <a id="api-015e4ec8cc593d69"></a>[`public interface Inno.Extensibility.Modules.IModuleSource`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/IModuleSource.cs#L13) | Prepares one module generation without publishing it to the owning catalog. | — | — |

### `Inno.Extensibility.Modules.ModuleAssemblyDescriptor`

<!-- api: T:Inno.Extensibility.Modules.ModuleAssemblyDescriptor 4bbf43adaacb6118ebfba20c4e93dc1a6261cf5a8405298573c46243463b787d -->
<!-- api: M:Inno.Extensibility.Modules.ModuleAssemblyDescriptor.#ctor(Inno.Extensibility.Modules.AssemblyDomain,Inno.Extensibility.Modules.AssemblyScope) 0498d398489d066512dd44246114f49745b1a3d29c89952a5f006567bcecf4a4 -->
<!-- api: P:Inno.Extensibility.Modules.ModuleAssemblyDescriptor.domain 76f3ca042a8c8a2ca06dbd46745b624f82d397d716631d9302a86049590d0461 -->
<!-- api: P:Inno.Extensibility.Modules.ModuleAssemblyDescriptor.scope 9588e027aed3b2d7b5d24311bd70414c33d637725e794b28e604a7c9ee7f5898 -->
<!-- api: M:Inno.Extensibility.Modules.ModuleAssemblyDescriptor.ToString 67c194b6d531ed8edb1b181195c7ccb06db43d58590263d53dc0681b42362dc9 -->
<!-- api: M:Inno.Extensibility.Modules.ModuleAssemblyDescriptor.op_Inequality(Inno.Extensibility.Modules.ModuleAssemblyDescriptor,Inno.Extensibility.Modules.ModuleAssemblyDescriptor) 6e2df4cf01473fc39ad814b5bcd2432312e7204761d8ba33f3c6288790d9443b -->
<!-- api: M:Inno.Extensibility.Modules.ModuleAssemblyDescriptor.op_Equality(Inno.Extensibility.Modules.ModuleAssemblyDescriptor,Inno.Extensibility.Modules.ModuleAssemblyDescriptor) 788a4a6cdfef70786b84e4b322f17428ed505b099232e458fa467eb7d1306794 -->
<!-- api: M:Inno.Extensibility.Modules.ModuleAssemblyDescriptor.GetHashCode 185fe6328bc665642eb429882e72f96fd965d6492c602178ea82893c47425a8b -->
<!-- api: M:Inno.Extensibility.Modules.ModuleAssemblyDescriptor.Equals(System.Object) a7e5fd5a595a7570ffa4911855698c1e3cbe2c2a8288ae3b8179b8254671def5 -->
<!-- api: M:Inno.Extensibility.Modules.ModuleAssemblyDescriptor.Equals(Inno.Extensibility.Modules.ModuleAssemblyDescriptor) 3d666d4b5535a0aec2ca9d72f2fa6b77491dd6510c78fb2ef71311683dc8de96 -->
<!-- api: M:Inno.Extensibility.Modules.ModuleAssemblyDescriptor.Deconstruct(Inno.Extensibility.Modules.AssemblyDomain@,Inno.Extensibility.Modules.AssemblyScope@) 85bdde5ebc20c8e015ce82d5f6e0c07b26567cdf1c66f49e61c8f458600bead5 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e0b05397ab17bef4"></a>[`public Inno.Extensibility.Modules.ModuleAssemblyDescriptor.ModuleAssemblyDescriptor(Inno.Extensibility.Modules.AssemblyDomain domain, Inno.Extensibility.Modules.AssemblyScope scope)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleAssemblyDescriptor.cs#L19) | Describes the ownership classification of one planned assembly without loading its code.<br>`domain`：The owning extension domain.<br>`scope`：The dependency scope declared by the module source. | — | — |
| <a id="api-14ffe313aca11a45"></a>[`public void Inno.Extensibility.Modules.ModuleAssemblyDescriptor.Deconstruct(out Inno.Extensibility.Modules.AssemblyDomain domain, out Inno.Extensibility.Modules.AssemblyScope scope)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleAssemblyDescriptor.cs#L19) | C# record 生成的解构入口，按主构造声明顺序返回位置成员。 | — | — |
| <a id="api-a2ca6250c8e1af85"></a>[`public bool Inno.Extensibility.Modules.ModuleAssemblyDescriptor.Equals(Inno.Extensibility.Modules.ModuleAssemblyDescriptor other)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleAssemblyDescriptor.cs#L19) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-aaf872ed27e5bf43"></a>[`public override bool Inno.Extensibility.Modules.ModuleAssemblyDescriptor.Equals(object obj)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleAssemblyDescriptor.cs#L19) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-c4f384a4ae6e7d97"></a>[`public override int Inno.Extensibility.Modules.ModuleAssemblyDescriptor.GetHashCode()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleAssemblyDescriptor.cs#L19) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-112752f4641932b1"></a>[`public static bool Inno.Extensibility.Modules.ModuleAssemblyDescriptor.operator ==(Inno.Extensibility.Modules.ModuleAssemblyDescriptor left, Inno.Extensibility.Modules.ModuleAssemblyDescriptor right)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleAssemblyDescriptor.cs#L19) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-90d35466b7df3ffa"></a>[`public static bool Inno.Extensibility.Modules.ModuleAssemblyDescriptor.operator !=(Inno.Extensibility.Modules.ModuleAssemblyDescriptor left, Inno.Extensibility.Modules.ModuleAssemblyDescriptor right)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleAssemblyDescriptor.cs#L19) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-5365b0c41c5f5773"></a>[`public override string Inno.Extensibility.Modules.ModuleAssemblyDescriptor.ToString()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleAssemblyDescriptor.cs#L19) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-ea28211537288e22"></a>[`public Inno.Extensibility.Modules.AssemblyDomain Inno.Extensibility.Modules.ModuleAssemblyDescriptor.domain { get; init; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleAssemblyDescriptor.cs#L20) | The owning extension domain. | — | — |
| <a id="api-82c09fb4490a3073"></a>[`public Inno.Extensibility.Modules.AssemblyScope Inno.Extensibility.Modules.ModuleAssemblyDescriptor.scope { get; init; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleAssemblyDescriptor.cs#L21) | The dependency scope declared by the module source. | — | — |
| <a id="api-bfd7aa5a816450e1"></a>[`public readonly record struct Inno.Extensibility.Modules.ModuleAssemblyDescriptor : System.IEquatable<Inno.Extensibility.Modules.ModuleAssemblyDescriptor>`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleAssemblyDescriptor.cs#L19) | Describes the ownership classification of one planned assembly without loading its code.<br>`domain`：The owning extension domain.<br>`scope`：The dependency scope declared by the module source. | — | — |

### `Inno.Extensibility.Modules.ModuleCatalogContribution`

<!-- api: T:Inno.Extensibility.Modules.ModuleCatalogContribution 89f1ddba06451a3e497bdf328fcee0d0ec802ffa5272b66c3d5e1177b6c009af -->
<!-- api: M:Inno.Extensibility.Modules.ModuleCatalogContribution.#ctor(System.String,Inno.Extensibility.Modules.AssemblyDomain,Inno.Extensibility.Modules.AssemblyScope,System.Collections.Generic.IReadOnlyList{System.Reflection.Assembly},System.Collections.Generic.IReadOnlyDictionary{System.Reflection.Assembly,Inno.Extensibility.Modules.AssemblyScope},Inno.Extensibility.Modules.IModuleLifetime) 428ab49eb78ff6e1535ad755f3bc04ad7ed85dd9fc191a065a34d9dc31467f2f -->
<!-- api: P:Inno.Extensibility.Modules.ModuleCatalogContribution.moduleName 858aae5058f587ce9935a9e7dcdefd3dbd877244d10ffb353c6f31d3e2b343d1 -->
<!-- api: P:Inno.Extensibility.Modules.ModuleCatalogContribution.domain c0ec173594c358e9beb19b15c26ae0b35fc1eb7ec8faf0479958ef74316474c9 -->
<!-- api: P:Inno.Extensibility.Modules.ModuleCatalogContribution.scope 3027ccb727e3c4c9ccfce9a20aeb72b322cbfc6e7dc2accd72b4ca8d462d035b -->
<!-- api: P:Inno.Extensibility.Modules.ModuleCatalogContribution.assemblies f9c5433f8af33a476a20482bf01122a344c9dc90664144e66a3472845e5fe950 -->
<!-- api: P:Inno.Extensibility.Modules.ModuleCatalogContribution.assemblyScopes 4a50bc20d368db688d4268d480b13d995589b6c885ca9ea98ae919b1b459468f -->
<!-- api: P:Inno.Extensibility.Modules.ModuleCatalogContribution.lifetime 17c0215b3a7bfb11762843b5406ad9c5fd09086b8d841b23648b8eb5568378af -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e769e1b71ba06622"></a>[`public Inno.Extensibility.Modules.ModuleCatalogContribution.ModuleCatalogContribution(string moduleName, Inno.Extensibility.Modules.AssemblyDomain domain, Inno.Extensibility.Modules.AssemblyScope scope, System.Collections.Generic.IReadOnlyList<System.Reflection.Assembly> assemblies, System.Collections.Generic.IReadOnlyDictionary<System.Reflection.Assembly, Inno.Extensibility.Modules.AssemblyScope> assemblyScopes, Inno.Extensibility.Modules.IModuleLifetime? lifetime = null)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleCatalogContribution.cs#L39) | Acquires immutable views while transferring the generation lifetime to the contribution.<br>`moduleName`：The stable module name.<br>`domain`：The ownership domain.<br>`scope`：The module's default dependency scope.<br>`assemblies`：The complete acquired assembly set.<br>`assemblyScopes`：The actual dependency scope of every acquired assembly.<br>`lifetime`：The transferred generation owner; null denotes externally owned static code. | — | `System.ArgumentException`：The name is empty, the assembly set is empty or contains duplicates, or scope metadata is incomplete. |
| <a id="api-7aa08496eea66227"></a>[`public System.Collections.Generic.IReadOnlyList<System.Reflection.Assembly> Inno.Extensibility.Modules.ModuleCatalogContribution.assemblies { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleCatalogContribution.cs#L78) | Gets the immutable owned assembly set. | — | — |
| <a id="api-3d04d9d3b4ea0c03"></a>[`public System.Collections.Generic.IReadOnlyDictionary<System.Reflection.Assembly, Inno.Extensibility.Modules.AssemblyScope> Inno.Extensibility.Modules.ModuleCatalogContribution.assemblyScopes { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleCatalogContribution.cs#L82) | Gets the complete per-assembly scope map. | — | — |
| <a id="api-9e3a3567b5024e2c"></a>[`public Inno.Extensibility.Modules.AssemblyDomain Inno.Extensibility.Modules.ModuleCatalogContribution.domain { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleCatalogContribution.cs#L70) | Gets the ownership domain. | — | — |
| <a id="api-b5908828cbc9fd32"></a>[`public Inno.Extensibility.Modules.IModuleLifetime? Inno.Extensibility.Modules.ModuleCatalogContribution.lifetime { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleCatalogContribution.cs#L86) | Gets the owned generation lifetime, or null for externally owned code. | — | — |
| <a id="api-43596464bf75dbd4"></a>[`public string Inno.Extensibility.Modules.ModuleCatalogContribution.moduleName { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleCatalogContribution.cs#L66) | Gets the stable module name. | — | — |
| <a id="api-20754bf13a3eb73b"></a>[`public Inno.Extensibility.Modules.AssemblyScope Inno.Extensibility.Modules.ModuleCatalogContribution.scope { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleCatalogContribution.cs#L74) | Gets the default dependency scope. | — | — |
| <a id="api-5cc4bb19163a4efb"></a>[`public sealed class Inno.Extensibility.Modules.ModuleCatalogContribution`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleCatalogContribution.cs#L13) | Freezes the assemblies, classification and lifetime of one acquired module generation. | — | — |

### `Inno.Extensibility.Modules.ModuleHost`

<!-- api: T:Inno.Extensibility.Modules.ModuleHost 854a392aa50532fa5a3977904ad092334686786a33ec9d01345a017421f18ce9 -->
<!-- api: P:Inno.Extensibility.Modules.ModuleHost.isInitialized c1ab31f3873accb7a09dff3d00125fbafbdcf3c1cd236e18a25cf5dba924d4f7 -->
<!-- api: P:Inno.Extensibility.Modules.ModuleHost.generations 5eecbf227e76a7eeb86b4a11ef1c811f7743ddbae8e902a622a1c2dc6be5df70 -->
<!-- api: P:Inno.Extensibility.Modules.ModuleHost.modules eb64af83696c58427fb7efe13855a28193caddabe0249799bdb9a3233391f1c3 -->
<!-- api: M:Inno.Extensibility.Modules.ModuleHost.#ctor(Inno.Extensibility.Modules.ModuleHostOptions) 4cc65fbe1d9f7e45354a7dc3fcf0cd99ad59538614821a7af18f28480e11d3a9 -->
<!-- api: M:Inno.Extensibility.Modules.ModuleHost.RegisterCatalogParticipant(Inno.Extensibility.Modules.IAssemblyCatalogParticipant) 9cd0653bd42a8bbb79f3b558129a1deec352710cc17b557b1120863800cf8617 -->
<!-- api: M:Inno.Extensibility.Modules.ModuleHost.Load(Inno.Extensibility.Modules.IModuleSource) f1406ff4671696756e3eac057aadcafeef371eb34be70b7ba2ed523b8420ca7c -->
<!-- api: M:Inno.Extensibility.Modules.ModuleHost.Register(System.String,System.Collections.Generic.IReadOnlyList{System.Reflection.Assembly}) 85574ec8d8eea8a37cb612362a4c736566753f6127f60a79fe7a80f097f57bbe -->
<!-- api: M:Inno.Extensibility.Modules.ModuleHost.BeginReload(Inno.Extensibility.Modules.AssemblyModuleHandle,Inno.Extensibility.Modules.IModuleSource) d647a39d71f2d2929aeda2423a8430638ad8b9ad972dcd7b138fa1274ac3d5c6 -->
<!-- api: M:Inno.Extensibility.Modules.ModuleHost.BeginReload(System.Collections.Generic.IReadOnlyList{Inno.Extensibility.Modules.IModuleSource}) c5dd1240f673eaea48fe6b1524659fc85551a5f515d1ba2eeaee674faa40f993 -->
<!-- api: M:Inno.Extensibility.Modules.ModuleHost.BeginReload(System.Collections.Generic.IReadOnlyList{Inno.Extensibility.Modules.IModuleSource},System.Collections.Generic.IReadOnlyList{System.String}) e5b83c14bac978ffe6ec12fee382f2912b414a4eaf9ebaf2e03d751d0e2800f8 -->
<!-- api: M:Inno.Extensibility.Modules.ModuleHost.Unload(Inno.Extensibility.Modules.AssemblyModuleHandle) 101645b681eab128c9a01794e32f4261f6690d39c33c2790519cb309ea895a5a -->
<!-- api: M:Inno.Extensibility.Modules.ModuleHost.Unload(System.Collections.Generic.IReadOnlyList{Inno.Extensibility.Modules.AssemblyModuleHandle}) d3466f53061b6991a81cd5ed583b8782cce33cac1dd0fe9152d39655cc555907 -->
<!-- api: M:Inno.Extensibility.Modules.ModuleHost.Refresh de790b9592d0027a83c54e1dc3fe12c0ae6798ec4e8ce3d9658b93daffdf4770 -->
<!-- api: M:Inno.Extensibility.Modules.ModuleHost.Rebuild b5a862a230668943712fec5ac86454ae9774451767ed70a8c28a627f5059fdee -->
<!-- api: M:Inno.Extensibility.Modules.ModuleHost.Dispose afa828f92ecf0a3564c758a5e100a486d676dd8d23446cfc160a0192dd5bcf3d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-0418814a4fef4cdd"></a>[`public Inno.Extensibility.Modules.ModuleHost.ModuleHost(Inno.Extensibility.Modules.ModuleHostOptions options)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHost.cs#L67) | Creates a module host, discovers assemblies in its owning load context, and publishes the first catalog.<br>`options`：The validated configuration that controls this operation. | — | — |
| <a id="api-dfdfda494025dbd6"></a>[`public Inno.Extensibility.Modules.AssemblyReloadSession Inno.Extensibility.Modules.ModuleHost.BeginReload(Inno.Extensibility.Modules.AssemblyModuleHandle module, Inno.Extensibility.Modules.IModuleSource request)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHost.cs#L293) | Stages and validates a replacement generation without publishing it.<br>返回：The validated assembly reload session that represents the completed operation.<br>`module`：The module consumed by begin reload; ownership remains with the caller unless explicitly stated otherwise.<br>`request`：The validated immutable request that defines this operation. | — | — |
| <a id="api-2f1870ba7941348a"></a>[`public Inno.Extensibility.Modules.AssemblyReloadSession Inno.Extensibility.Modules.ModuleHost.BeginReload(System.Collections.Generic.IReadOnlyList<Inno.Extensibility.Modules.IModuleSource> requests, System.Collections.Generic.IReadOnlyList<string> removedModuleNames)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHost.cs#L364) | Stages additions, replacements, and removals as one atomic dependency-ordered transaction.<br>返回：A session that publishes or rolls back every candidate and removal together.<br>`requests`：Complete candidate module additions and replacements.<br>`removedModuleNames`：Active stable module names omitted from the candidate generation. | — | `System.ArgumentNullException`：Thrown when either argument is null.<br>`System.ArgumentException`：Thrown when no change is requested, a name is duplicated, or one module is both replaced and removed. |
| <a id="api-7e8517e9329ee5e9"></a>[`public Inno.Extensibility.Modules.AssemblyReloadSession Inno.Extensibility.Modules.ModuleHost.BeginReload(System.Collections.Generic.IReadOnlyList<Inno.Extensibility.Modules.IModuleSource> requests)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHost.cs#L335) | Stages a dependency-ordered set of module additions or replacements as one atomic transaction. Existing modules are matched by their stable module names; an unknown name creates a new module.<br>返回：A session that publishes or rolls back every candidate together.<br>`requests`：The complete reverse-dependency reload closure. | — | `System.ArgumentNullException`：Thrown when requests is null.<br>`System.ArgumentException`：Thrown when the request set is empty or contains duplicate module names. |
| <a id="api-28e6e98e8538fc4d"></a>[`public void Inno.Extensibility.Modules.ModuleHost.Dispose()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHost.cs#L509) | Unsubscribes assembly discovery and begins unload of every module owned by this host. | — | `Inno.Core.Execution.RetirementPendingException`：A generation still owns live work. Discovery stops, but modules are not unloaded or forgotten. |
| <a id="api-a61e6a735d31982c"></a>[`public Inno.Extensibility.Modules.AssemblyModuleHandle Inno.Extensibility.Modules.ModuleHost.Load(Inno.Extensibility.Modules.IModuleSource request)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHost.cs#L171) | Loads and activates a new shadow-copied assembly module.<br>返回：The validated assembly module handle that represents the completed operation.<br>`request`：The validated immutable request that defines this operation. | — | — |
| <a id="api-6800116a8668bed9"></a>[`public void Inno.Extensibility.Modules.ModuleHost.Rebuild()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHost.cs#L485) | Rebuilds the active assembly catalog and every registered derived-state participant. | — | — |
| <a id="api-988885c22467d620"></a>[`public void Inno.Extensibility.Modules.ModuleHost.Refresh()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHost.cs#L470) | Applies pending host assembly changes without rebuilding an unchanged catalog. | — | — |
| <a id="api-78e9f40a7bc553e4"></a>[`public Inno.Extensibility.Modules.AssemblyModuleHandle Inno.Extensibility.Modules.ModuleHost.Register(string moduleName, System.Collections.Generic.IReadOnlyList<System.Reflection.Assembly> assemblies)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHost.cs#L218) | Registers assemblies owned by an external load context.<br>返回：The validated assembly module handle that represents the completed operation.<br>`moduleName`：The module name text validated by the register operation.<br>`assemblies`：The assemblies consumed by register; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-b31023d29454b909"></a>[`public System.IDisposable Inno.Extensibility.Modules.ModuleHost.RegisterCatalogParticipant(Inno.Extensibility.Modules.IAssemblyCatalogParticipant participant)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHost.cs#L104) | Registers a transactional consumer of the assembly catalog and initializes it from the currently active generation.<br>返回：A registration that removes the participant when disposed.<br>`participant`：The participant that derives state from catalog snapshots. | — | — |
| <a id="api-a57b187f2806c47f"></a>[`public Inno.Extensibility.Reload.IAssemblyUnloadProbe Inno.Extensibility.Modules.ModuleHost.Unload(Inno.Extensibility.Modules.AssemblyModuleHandle module)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHost.cs#L387) | Removes an active module and starts cooperative unload when it is manager-owned.<br>返回：The validated assembly unload monitor that represents the completed operation.<br>`module`：The module consumed by unload; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-cb01dcd25f559c45"></a>[`public Inno.Extensibility.Reload.IAssemblyUnloadProbe Inno.Extensibility.Modules.ModuleHost.Unload(System.Collections.Generic.IReadOnlyList<Inno.Extensibility.Modules.AssemblyModuleHandle> modules)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHost.cs#L425) | Removes several active modules in one catalog transaction, then requests unload in reverse dependency order.<br>返回：A monitor that completes after every collectible context becomes unreachable.<br>`modules`：The distinct active module handles to remove. | — | `System.ArgumentNullException`：Thrown when modules is null.<br>`System.ArgumentException`：Thrown when a handle is duplicated or is not active. |
| <a id="api-44a2e4ac5a253efe"></a>[`public Inno.Extensibility.Reload.GenerationCoordinator Inno.Extensibility.Modules.ModuleHost.generations { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHost.cs#L42) | Gets the admission gate and retirement owner shared by all module and host generation changes. | — | — |
| <a id="api-41de57bdf74bc14e"></a>[`public bool Inno.Extensibility.Modules.ModuleHost.isInitialized { get; private set; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHost.cs#L37) | Gets whether this module host can accept catalog operations. | — | — |
| <a id="api-a869ae3051ce398f"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Extensibility.Modules.AssemblyModuleInfo> Inno.Extensibility.Modules.ModuleHost.modules { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHost.cs#L47) | Gets non-owning information about active managed and external modules. | — | — |
| <a id="api-8d63a92bc53048c9"></a>[`public sealed class Inno.Extensibility.Modules.ModuleHost : System.IDisposable`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHost.cs#L17) | Owns the active managed assembly catalog and transactional module generations. | — | — |

### `Inno.Extensibility.Modules.ModuleHostOptions`

<!-- api: T:Inno.Extensibility.Modules.ModuleHostOptions 70ca9695cb3a1451c7498fd17c963d1862f5be5f1b30a1e354b707cb10da44cc -->
<!-- api: P:Inno.Extensibility.Modules.ModuleHostOptions.catalogSource 95235980d0d82b92417511e8ef93fc0127d259080874c1462a5de91c699019cf -->
<!-- api: M:Inno.Extensibility.Modules.ModuleHostOptions.#ctor bff520c9dd27f525819697380695c5325de206c68f3fe5659e40c70059837a94 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-69ff6e80a6f9ce48"></a>[`public Inno.Extensibility.Modules.ModuleHostOptions.ModuleHostOptions()`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHostOptions.cs#L6) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-e2745f07963cad33"></a>[`public required Inno.Extensibility.Modules.IAssemblyCatalogSource Inno.Extensibility.Modules.ModuleHostOptions.catalogSource { get; init; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHostOptions.cs#L11) | Gets the host source whose ownership transfers to the module host during construction. | — | — |
| <a id="api-df6a9bca8d06016b"></a>[`public sealed class Inno.Extensibility.Modules.ModuleHostOptions`](../../src/foundation/extensibility/Inno.Extensibility.Modules/ModuleHostOptions.cs#L6) | Selects the isolated host catalog without prescribing storage for contributed module sources. | — | — |

### `Inno.Extensibility.Modules.ModuleSourceContext`

<!-- api: T:Inno.Extensibility.Modules.ModuleSourceContext ccf659e65e2af1a6f3cef2f37b818f2f83a2e0fdbeb647b363f90c99ff228540 -->
<!-- api: P:Inno.Extensibility.Modules.ModuleSourceContext.generation f34937c1ac35bd48f43e68502ab4000f0ee4ecb3a3aa373d7e56fb58a92ebcac -->
<!-- api: P:Inno.Extensibility.Modules.ModuleSourceContext.host 42d77063369a452fedc7ee172cb4d339ba004dff961c89ddadf3d17714352db1 -->
<!-- api: P:Inno.Extensibility.Modules.ModuleSourceContext.upstreamModules 0fe25b1bb907c400d13382fe68172434af96620ad188fc56157c10d04da31a42 -->
<!-- api: P:Inno.Extensibility.Modules.ModuleSourceContext.activeModules 173fac802114d57ac11750c84a631bfd8c00cbe409a9b5969841ed9a1d151270 -->
<!-- api: P:Inno.Extensibility.Modules.ModuleSourceContext.plannedAssemblies 0235be6c59bf70c65e4010db9f9de4719a5b43e95010788c4cc461b86c71feb7 -->
<!-- api: M:Inno.Extensibility.Modules.ModuleSourceContext.TrackRetirement(Inno.Extensibility.Reload.IAssemblyUnloadProbe) d3d15173f706b672b2ce96629952d4cfe3adc27f1208f0822cfeba7f608bb4a4 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-996b35870283569d"></a>[`public void Inno.Extensibility.Modules.ModuleSourceContext.TrackRetirement(Inno.Extensibility.Reload.IAssemblyUnloadProbe probe)`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleSourceContext.cs#L60) | Transfers a failed acquisition's non-owning retirement probe to the shared generation gate.<br>`probe`：The failed candidate's code and storage release probe. | — | — |
| <a id="api-f35ae9b433892f2d"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Extensibility.Modules.ModuleCatalogContribution> Inno.Extensibility.Modules.ModuleSourceContext.activeModules { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleSourceContext.cs#L49) | Gets active contributions used to reject forbidden downstream references. | — | — |
| <a id="api-d9ddd63dd606ce6b"></a>[`public int Inno.Extensibility.Modules.ModuleSourceContext.generation { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleSourceContext.cs#L37) | Gets the candidate generation number. | — | — |
| <a id="api-ba999a6f232535b1"></a>[`public Inno.Extensibility.Modules.IAssemblyCatalogSource Inno.Extensibility.Modules.ModuleSourceContext.host { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleSourceContext.cs#L41) | Gets the borrowed host contract source. | — | — |
| <a id="api-8e0f0d4dc8de2ff2"></a>[`public System.Collections.Generic.IReadOnlyDictionary<string, Inno.Extensibility.Modules.ModuleAssemblyDescriptor> Inno.Extensibility.Modules.ModuleSourceContext.plannedAssemblies { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleSourceContext.cs#L53) | Gets identities and classifications for the complete candidate closure. | — | — |
| <a id="api-979bb18532d1a7c0"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Extensibility.Modules.ModuleCatalogContribution> Inno.Extensibility.Modules.ModuleSourceContext.upstreamModules { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleSourceContext.cs#L45) | Gets the validated direct upstream contributions for this candidate. | — | — |
| <a id="api-75b97d0ea070d173"></a>[`public sealed class Inno.Extensibility.Modules.ModuleSourceContext`](../../src/foundation/extensibility/Inno.Extensibility.Modules/Catalog/ModuleSourceContext.cs#L13) | Supplies a source with the frozen dependencies and retirement boundary of one candidate generation. | — | — |

## 项目依赖

- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Collections](../core/Inno.Core.Collections.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Reload](Inno.Extensibility.Reload.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
