# Inno.Adapter.Modules.DotNet

[分类索引](README.md) · [Wiki 首页](../../README.md) · [Modules](../../extensibility/Inno.Extensibility.Modules.md) · [Types](../../extensibility/Inno.Extensibility.Types.md)

## 职责与允许依赖

本库实现 .NET 动态代码的具体来源，依赖中立 Modules、Types 与 Reload。collectible ALC、shadow copy、文件依赖解析、宿主 .deps.json 和反射扫描全部属于这里。Foundation 不引用本库。Editor 和作者工具在组合入口注入这些实现，发行 Player 使用编译生成的静态目录。

## 所有公开入口

| API | 语义 |
| --- | --- |
| `DotNetAssemblyCatalogSource(params Assembly[] roots)` | 只从显式 roots 的引用与 owning deps 闭包发现宿主 Inno 代码，不扫描整个 AppDomain。 |
| `changed / GetAssemblies / GetSharedAssemblies` | 宿主目录变化通知与当前 explicit closure；通知只在所属 Inno 目录变化时触发。 |
| `IsFrameworkReference / IsFrameworkAssembly / Dispose` | 共享框架归属查询及事件注销。 |
| `DotNetModuleSource` | 必填 moduleName、mainAssemblyPath；可选 preloadAssemblyPaths、upstreamModuleNames、collectible、domain、scope、assemblyScopes。 |
| `GetAssemblyNames()` | 读取并验证 owned code 的 simple names，供事务预检。 |
| `Prepare(ModuleSourceContext)` | 获取 shadow generation 与 ALC，返回冻结 contribution 和来源 lifetime；失败不发布代码。 |
| `ReflectionTypeCatalogSource.GetTypes(assembly)` | 在候选构建时扫描声明；不在帧热路径扫描。 |
| `GetMetadata(type)` | 提供基类、接口、直接/继承 Attribute 与生命周期虚方法 override 事实。 |
| `ConstructGenericType(definition, arguments)` | 使用当前 .NET 运行时构造封闭泛型；参数无效明确失败，泛型约束不成立返回 null。 |
| `CanCreateInstance(type) / CreateInstance(type)` | 通过该来源的构造规则创建实例；构造失败保留原始用户异常。 |

没有公开 ALC、加载 context 或弱 monitor 类型。它们是实现细节，不是应用或插件协议。

## 工作流与所有权

```csharp
using Inno.Adapter.Modules.DotNet;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Reload;

var source = new DotNetModuleSource
{
    moduleName = "RuntimeScripts",
    mainAssemblyPath = compiledPath,
    domain = AssemblyDomain.InnoScripting,
    scope = AssemblyScope.Runtime,
    collectible = true
};
using AssemblyReloadSession candidate = modules.BeginReload([source]);
candidate.Activate();
IAssemblyUnloadProbe retirement = candidate.Complete();
```

调用方仍必须通过共享 generation retirement gate 验证 probe 完成。shadow cache 位于 context.artifactDirectory 下的来源 generation；代码不可达后才能删目录。失败、取消和 Host 停止均注销回调并逆序退休来源资源；timeout 不清空 probe。

## 扩展与验收

新增运行时来源实现 Foundation 接口，不能修改本库为跨平台 switch。当前动态来源用于支持 ALC 的作者宿主；实际测试通过 Modules/Types/Scripting 的公开边界执行，无测试后门。






## 本轮边界与所有权

DotNetModuleSource 自己持有 artifact root，负责 ALC、shadow copy 与动态依赖解析。Foundation 不替它选择路径；退休继续执行 Full GC → finalizers → Full GC 与弱监测，Pending 不得当作 Success。

## 源码归属

当前唯一源码 owner：`backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/Inno.Adapter.Modules.DotNet.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Adapter.Modules.DotNet.DotNetAssemblyCatalogSource`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Modules.DotNet.DotNetAssemblyCatalogSource`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetAssemblyCatalogSource.cs#L14) | Owns discovery of an explicit desktop authoring closure and its shareable framework contracts. |
| [`Inno.Adapter.Modules.DotNet.DotNetAssemblyCatalogSource.DotNetAssemblyCatalogSource(params System.Reflection.Assembly[] roots)`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetAssemblyCatalogSource.cs#L35) | Resolves the referenced Inno closure in the roots' existing load context. |
| [`System.Action? Inno.Adapter.Modules.DotNet.DotNetAssemblyCatalogSource.changed`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetAssemblyCatalogSource.cs#L50) | See the implemented contract. |
| [`System.Collections.Generic.IReadOnlyList<System.Reflection.Assembly> Inno.Adapter.Modules.DotNet.DotNetAssemblyCatalogSource.GetAssemblies()`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetAssemblyCatalogSource.cs#L53) | See the implemented contract. |
| [`System.Collections.Generic.IReadOnlyList<System.Reflection.Assembly> Inno.Adapter.Modules.DotNet.DotNetAssemblyCatalogSource.GetSharedAssemblies()`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetAssemblyCatalogSource.cs#L63) | See the implemented contract. |
| [`bool Inno.Adapter.Modules.DotNet.DotNetAssemblyCatalogSource.IsFrameworkAssembly(System.Reflection.Assembly assembly)`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetAssemblyCatalogSource.cs#L75) | See the implemented contract. |
| [`bool Inno.Adapter.Modules.DotNet.DotNetAssemblyCatalogSource.IsFrameworkReference(string assemblyName)`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetAssemblyCatalogSource.cs#L72) | See the implemented contract. |
| [`void Inno.Adapter.Modules.DotNet.DotNetAssemblyCatalogSource.Dispose()`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetAssemblyCatalogSource.cs#L78) | See the implemented contract. |

### `Inno.Adapter.Modules.DotNet.DotNetModuleSource`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Modules.DotNet.DotNetModuleSource`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetModuleSource.cs#L15) | Describes one independently reloadable managed assembly module. |
| [`Inno.Extensibility.Modules.AssemblyDomain Inno.Adapter.Modules.DotNet.DotNetModuleSource.domain`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetModuleSource.cs#L54) | Gets or sets the ownership domain for every assembly in this module. |
| [`Inno.Extensibility.Modules.AssemblyScope Inno.Adapter.Modules.DotNet.DotNetModuleSource.scope`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetModuleSource.cs#L59) | Gets or sets the dependency scope for assemblies without a more specific internal descriptor. |
| [`Inno.Extensibility.Modules.ModuleCatalogContribution Inno.Adapter.Modules.DotNet.DotNetModuleSource.Prepare(Inno.Extensibility.Modules.ModuleSourceContext context)`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetModuleSource.cs#L93) | See the implemented contract. |
| [`System.Collections.Generic.IReadOnlyDictionary<string, Inno.Extensibility.Modules.AssemblyScope> Inno.Adapter.Modules.DotNet.DotNetModuleSource.assemblyScopes`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetModuleSource.cs#L68) | Gets per-assembly scope overrides keyed by managed assembly simple name. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Adapter.Modules.DotNet.DotNetModuleSource.GetAssemblyNames()`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetModuleSource.cs#L72) | See the implemented contract. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Adapter.Modules.DotNet.DotNetModuleSource.preloadAssemblyPaths`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetModuleSource.cs#L35) | Gets or sets additional managed assemblies loaded into the same context. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Adapter.Modules.DotNet.DotNetModuleSource.upstreamModuleNames`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetModuleSource.cs#L44) | Gets stable module names whose exported assemblies may satisfy this module's managed dependencies. |
| [`bool Inno.Adapter.Modules.DotNet.DotNetModuleSource.collectible`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetModuleSource.cs#L49) | Gets or sets whether the module load context supports cooperative unloading. |
| [`required string Inno.Adapter.Modules.DotNet.DotNetModuleSource.artifactRootDirectory`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetModuleSource.cs#L20) | Gets the absolute host-selected root owned by this dynamic source for shadow-copy generations. |
| [`required string Inno.Adapter.Modules.DotNet.DotNetModuleSource.mainAssemblyPath`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetModuleSource.cs#L30) | Gets or sets the path of the module's primary managed assembly. |
| [`required string Inno.Adapter.Modules.DotNet.DotNetModuleSource.moduleName`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetModuleSource.cs#L25) | Gets or sets the stable logical module name. |

### `Inno.Adapter.Modules.DotNet.ReflectionTypeCatalogSource`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Modules.DotNet.ReflectionTypeCatalogSource`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/Discovery/ReflectionTypeCatalogSource.cs#L15) | Discovers desktop authoring types through the selected managed runtime's reflection service. |
| [`Inno.Extensibility.Catalogs.TypeCatalogMetadata Inno.Adapter.Modules.DotNet.ReflectionTypeCatalogSource.GetMetadata(System.Type type)`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/Discovery/ReflectionTypeCatalogSource.cs#L25) | See the implemented contract. |
| [`System.Collections.Generic.IReadOnlyList<System.Type> Inno.Adapter.Modules.DotNet.ReflectionTypeCatalogSource.GetTypes(System.Reflection.Assembly assembly)`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/Discovery/ReflectionTypeCatalogSource.cs#L18) | See the implemented contract. |
| [`System.Type? Inno.Adapter.Modules.DotNet.ReflectionTypeCatalogSource.ConstructGenericType(System.Type definition, System.Collections.Generic.IReadOnlyList<System.Type> arguments)`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/Discovery/ReflectionTypeCatalogSource.cs#L40) | See the implemented contract. |
| [`bool Inno.Adapter.Modules.DotNet.ReflectionTypeCatalogSource.CanCreateInstance(System.Type type)`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/Discovery/ReflectionTypeCatalogSource.cs#L62) | See the implemented contract. |
| [`object Inno.Adapter.Modules.DotNet.ReflectionTypeCatalogSource.CreateInstance(System.Type type)`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/Discovery/ReflectionTypeCatalogSource.cs#L71) | See the implemented contract. |

## 项目依赖

- [Inno.Core.Collections](../../core/Inno.Core.Collections.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Modules](../../extensibility/Inno.Extensibility.Modules.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Types](../../extensibility/Inno.Extensibility.Types.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
