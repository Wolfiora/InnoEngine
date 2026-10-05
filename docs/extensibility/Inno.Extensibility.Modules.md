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
