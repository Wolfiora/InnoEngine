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
| `DotNetModuleSource` | 必填 moduleName、mainAssemblyPath、artifactRootDirectory；可选 preloadAssemblyPaths、upstreamModuleNames、collectible、domain、scope、assemblyScopes。 |
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

static DotNetModuleSource DescribeModule(
    string compiledPath,
    string ownedCacheRoot
) {
    return new DotNetModuleSource
    {
        moduleName = "RuntimeScripts",
        mainAssemblyPath = compiledPath,
        artifactRootDirectory = ownedCacheRoot,
        domain = AssemblyDomain.InnoScripting,
        scope = AssemblyScope.Runtime,
        collectible = true
    };
}
```

将该来源交给 `ModuleHost.BeginReload` 创建候选，按共同事务协调 Activate 和 Complete，并通过 generation retirement gate 验证返回的 probe 完成。shadow cache 位于来源明确持有的 `artifactRootDirectory` 下；代码不可达后才能删目录。失败、取消和 Host 停止均注销回调并逆序退休来源资源；timeout 不清空 probe。

## 扩展与验收

新增运行时来源实现 Foundation 接口，不能修改本库为跨平台 switch。当前动态来源用于支持 ALC 的作者宿主；实际测试通过 Modules/Types/Scripting 的公开边界执行，无测试后门。






## 本轮边界与所有权

DotNetModuleSource 自己持有 artifact root，负责 ALC、shadow copy 与动态依赖解析。Foundation 不替它选择路径；退休继续执行 Full GC → finalizers → Full GC 与弱监测，Pending 不得当作 Success。

## 源码归属

当前唯一源码 owner：`backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/Inno.Adapter.Modules.DotNet.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Adapter.Modules.DotNet.DotNetAssemblyCatalogSource`

<!-- api: T:Inno.Adapter.Modules.DotNet.DotNetAssemblyCatalogSource d438e2eb336277aa90b9713d439d9795a766e13916d22e52cb5338cc613d6ceb -->
<!-- api: M:Inno.Adapter.Modules.DotNet.DotNetAssemblyCatalogSource.#ctor(System.Reflection.Assembly[]) c96927a9f4c2b906d6ac18d0d5344449366007aa15b2370a5eb1f22989ea0e92 -->
<!-- api: E:Inno.Adapter.Modules.DotNet.DotNetAssemblyCatalogSource.changed 22f0fc121a3c9d75161137203f75264abad4ae57570763e34866cc81a00db27f -->
<!-- api: M:Inno.Adapter.Modules.DotNet.DotNetAssemblyCatalogSource.GetAssemblies 144251dbc521a2f3ff2f281459916aba03d08587f87b6709807e7ba1df542d85 -->
<!-- api: M:Inno.Adapter.Modules.DotNet.DotNetAssemblyCatalogSource.GetSharedAssemblies 5fc49394fa02fefc66dc57d65636c40cadd638c6f25718cc88b50ea1aa459cb4 -->
<!-- api: M:Inno.Adapter.Modules.DotNet.DotNetAssemblyCatalogSource.IsFrameworkReference(System.String) 6d3337cff6cf8a7e9838cedd7f464fc47c7b7a657d038ea0811019f9df7988e0 -->
<!-- api: M:Inno.Adapter.Modules.DotNet.DotNetAssemblyCatalogSource.IsFrameworkAssembly(System.Reflection.Assembly) a8eb9967c7818872e6df643f73531c09c0fb06679d061f0985a7896a4e92ccb9 -->
<!-- api: M:Inno.Adapter.Modules.DotNet.DotNetAssemblyCatalogSource.Dispose 35fcd296a2cc4087872f5eabc43a2a89b64b35db9d3be0e71af0e05aa1400c53 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-bbde0880bc990a1a"></a>[`public System.Action? Inno.Adapter.Modules.DotNet.DotNetAssemblyCatalogSource.changed`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetAssemblyCatalogSource.cs#L50) | Notifies the owner that the host assembly snapshot should be refreshed at a safe point. | — | — |
| <a id="api-ff50f71908f0e11a"></a>[`public Inno.Adapter.Modules.DotNet.DotNetAssemblyCatalogSource.DotNetAssemblyCatalogSource(params System.Reflection.Assembly[] roots)`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetAssemblyCatalogSource.cs#L35) | Resolves the referenced Inno closure in the roots' existing load context.<br>`roots`：Explicit composition roots, all belonging to the same externally owned load context. | — | `System.ArgumentException`：No roots are supplied or roots belong to different load contexts.<br>`System.IO.FileNotFoundException`：An explicitly declared engine dependency is absent from the host deployment. |
| <a id="api-ada1d6f131f6e8a2"></a>[`public void Inno.Adapter.Modules.DotNet.DotNetAssemblyCatalogSource.Dispose()`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetAssemblyCatalogSource.cs#L78) | Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. | — | — |
| <a id="api-0d8731c3dd19c832"></a>[`public System.Collections.Generic.IReadOnlyList<System.Reflection.Assembly> Inno.Adapter.Modules.DotNet.DotNetAssemblyCatalogSource.GetAssemblies()`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetAssemblyCatalogSource.cs#L53) | Gets the complete host assemblies that participate in extension discovery.<br>返回：The current host snapshot; the caller must not retain it beyond its catalog generation. | — | — |
| <a id="api-7bc35abc89b67305"></a>[`public System.Collections.Generic.IReadOnlyList<System.Reflection.Assembly> Inno.Adapter.Modules.DotNet.DotNetAssemblyCatalogSource.GetSharedAssemblies()`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetAssemblyCatalogSource.cs#L63) | Gets host contracts and framework assemblies available to candidate modules.<br>返回：A snapshot of shareable assemblies owned by the host implementation. | — | — |
| <a id="api-582a72f04596d8e0"></a>[`public bool Inno.Adapter.Modules.DotNet.DotNetAssemblyCatalogSource.IsFrameworkAssembly(System.Reflection.Assembly assembly)`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetAssemblyCatalogSource.cs#L75) | Determines whether an assembly is a framework contract rather than a contributed module.<br>返回：Whether the assembly is owned by the selected managed framework.<br>`assembly`：The candidate shared assembly. | — | — |
| <a id="api-1b82deb3a2cb01ae"></a>[`public bool Inno.Adapter.Modules.DotNet.DotNetAssemblyCatalogSource.IsFrameworkReference(string assemblyName)`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetAssemblyCatalogSource.cs#L72) | Determines whether a referenced simple name belongs to the selected managed framework.<br>返回：Whether the framework owns this dependency.<br>`assemblyName`：The referenced managed assembly simple name. | — | — |
| <a id="api-3d27e635d040ef9a"></a>[`public sealed class Inno.Adapter.Modules.DotNet.DotNetAssemblyCatalogSource : Inno.Extensibility.Modules.IAssemblyCatalogSource`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetAssemblyCatalogSource.cs#L14) | Owns discovery of an explicit desktop authoring closure and its shareable framework contracts. | — | — |

### `Inno.Adapter.Modules.DotNet.DotNetModuleSource`

<!-- api: T:Inno.Adapter.Modules.DotNet.DotNetModuleSource 8a38ad62953841af72be05e533d17f10f9eae6fbd2bf0d91ef12e72b665c1e09 -->
<!-- api: P:Inno.Adapter.Modules.DotNet.DotNetModuleSource.artifactRootDirectory ba39a900664e1d34242b4f9e0cf4c10c98bde2e684171ffe064b84fcc7294bef -->
<!-- api: P:Inno.Adapter.Modules.DotNet.DotNetModuleSource.moduleName 9ee8cf044b5bcadaf08d57ba02c97538b6a93532e1982afe05d9aa5ed22e8988 -->
<!-- api: P:Inno.Adapter.Modules.DotNet.DotNetModuleSource.mainAssemblyPath c94ce612d5989426da9f4bd7abfac62cb905eb730bdb434c861bd4df10d4c139 -->
<!-- api: P:Inno.Adapter.Modules.DotNet.DotNetModuleSource.preloadAssemblyPaths 3cff72d53312fd46566608cb617a3345b1e1db551272029bd69ff618aa2a6bcc -->
<!-- api: P:Inno.Adapter.Modules.DotNet.DotNetModuleSource.upstreamModuleNames 24778d8bb7a2283f494da31fba54d5faf910957248a5c2778adfdc06a30e6bac -->
<!-- api: P:Inno.Adapter.Modules.DotNet.DotNetModuleSource.collectible f5cdd96840e6ec84f44bb8af3aed1edf69b0fb19711b0367ae8f7cb30803b739 -->
<!-- api: P:Inno.Adapter.Modules.DotNet.DotNetModuleSource.domain e5e64797641e29a21f536cd7655daf429aadde1e4d9d05512dbb1c54c87fe697 -->
<!-- api: P:Inno.Adapter.Modules.DotNet.DotNetModuleSource.scope 603a98db0cad3a84e5bb54c74547653a1940476f422af3f2ba3360e0bffda774 -->
<!-- api: P:Inno.Adapter.Modules.DotNet.DotNetModuleSource.assemblyScopes c4af95cd360cb0078dd66b9da55bf84b2366bdd92a3b777f79ae36413d3e0cbd -->
<!-- api: M:Inno.Adapter.Modules.DotNet.DotNetModuleSource.GetAssemblyNames 46fb793c44d65ba5925d274e769c1c573d9c4c5c5dec73cca67edb45c17a5842 -->
<!-- api: M:Inno.Adapter.Modules.DotNet.DotNetModuleSource.Prepare(Inno.Extensibility.Modules.ModuleSourceContext) 1054e9c37a4a4eb023c669c27a621476afa7de00bc0ac82eeceb5f7e64f49734 -->
<!-- api: M:Inno.Adapter.Modules.DotNet.DotNetModuleSource.#ctor 26e74f90fc0aae324b83be96962436a9b0be349b9aa409c5b488f7978dbbd3e3 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e64268d2a9a507ee"></a>[`public Inno.Adapter.Modules.DotNet.DotNetModuleSource.DotNetModuleSource()`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetModuleSource.cs#L15) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-5e7072ce19f9620d"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Adapter.Modules.DotNet.DotNetModuleSource.GetAssemblyNames()`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetModuleSource.cs#L72) | Reads the complete owned assembly identities before any generation is acquired.<br>返回：Distinct, nonempty simple names; invalid or unavailable source data causes an exception. | — | — |
| <a id="api-d3d1397de54abcee"></a>[`public Inno.Extensibility.Modules.ModuleCatalogContribution Inno.Adapter.Modules.DotNet.DotNetModuleSource.Prepare(Inno.Extensibility.Modules.ModuleSourceContext context)`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetModuleSource.cs#L93) | Acquires a validated candidate and transfers its lifetime to the caller on success.<br>返回：A complete unpublished contribution; failure retires every acquired resource through the context.<br>`context`：The immutable candidate dependency closure and retirement owner. | — | — |
| <a id="api-1597c6adcde305d2"></a>[`public required string Inno.Adapter.Modules.DotNet.DotNetModuleSource.artifactRootDirectory { get; init; }`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetModuleSource.cs#L20) | Gets the absolute host-selected root owned by this dynamic source for shadow-copy generations. | — | — |
| <a id="api-ee15adf027a7c2d6"></a>[`public System.Collections.Generic.IReadOnlyDictionary<string, Inno.Extensibility.Modules.AssemblyScope> Inno.Adapter.Modules.DotNet.DotNetModuleSource.assemblyScopes { get; init; }`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetModuleSource.cs#L68) | Gets per-assembly scope overrides keyed by managed assembly simple name. | This is used when one plugin generation contains both runtime and editor-only assemblies. Names not present in the map use Inno.Adapter.Modules.DotNet.DotNetModuleSource.scope. | — |
| <a id="api-74764e33681a6163"></a>[`public bool Inno.Adapter.Modules.DotNet.DotNetModuleSource.collectible { get; init; }`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetModuleSource.cs#L49) | Gets or sets whether the module load context supports cooperative unloading. | — | — |
| <a id="api-7980fea9e6af0dc0"></a>[`public Inno.Extensibility.Modules.AssemblyDomain Inno.Adapter.Modules.DotNet.DotNetModuleSource.domain { get; init; }`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetModuleSource.cs#L54) | Gets or sets the ownership domain for every assembly in this module. | — | — |
| <a id="api-ea853347498ee75f"></a>[`public required string Inno.Adapter.Modules.DotNet.DotNetModuleSource.mainAssemblyPath { get; init; }`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetModuleSource.cs#L30) | Gets or sets the path of the module's primary managed assembly. | — | — |
| <a id="api-da5f8bd7d016a12a"></a>[`public required string Inno.Adapter.Modules.DotNet.DotNetModuleSource.moduleName { get; init; }`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetModuleSource.cs#L25) | Gets or sets the stable logical module name. | — | — |
| <a id="api-04e73a862fc54cb0"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Adapter.Modules.DotNet.DotNetModuleSource.preloadAssemblyPaths { get; init; }`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetModuleSource.cs#L35) | Gets or sets additional managed assemblies loaded into the same context. | — | — |
| <a id="api-d3c25713c1bb31e6"></a>[`public Inno.Extensibility.Modules.AssemblyScope Inno.Adapter.Modules.DotNet.DotNetModuleSource.scope { get; init; }`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetModuleSource.cs#L59) | Gets or sets the dependency scope for assemblies without a more specific internal descriptor. | — | — |
| <a id="api-d37175b2f3d87c1c"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Adapter.Modules.DotNet.DotNetModuleSource.upstreamModuleNames { get; init; }`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetModuleSource.cs#L44) | Gets stable module names whose exported assemblies may satisfy this module's managed dependencies. | Dependencies are explicit for every module. An empty collection declares that the module has no upstream module dependencies; the host never infers dependencies from process-wide active state. | — |
| <a id="api-2e056245149b587b"></a>[`public sealed class Inno.Adapter.Modules.DotNet.DotNetModuleSource : Inno.Extensibility.Modules.IModuleSource`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/DotNetModuleSource.cs#L15) | Describes one independently reloadable managed assembly module. | — | — |

### `Inno.Adapter.Modules.DotNet.ReflectionTypeCatalogSource`

<!-- api: T:Inno.Adapter.Modules.DotNet.ReflectionTypeCatalogSource c8f75709a9ac0f8ad2b80024a977a0e3b2ee609db12763fefb6c159b5ae999de -->
<!-- api: M:Inno.Adapter.Modules.DotNet.ReflectionTypeCatalogSource.GetTypes(System.Reflection.Assembly) 715bab55ccd81bbed8b4d43714c71e4c44a17112f494e2d26901cb8eb55eec83 -->
<!-- api: M:Inno.Adapter.Modules.DotNet.ReflectionTypeCatalogSource.GetMetadata(System.Type) 4ba30deb4b18e5696ebec2a3513628ec656975623b4d8a891663080eb8031819 -->
<!-- api: M:Inno.Adapter.Modules.DotNet.ReflectionTypeCatalogSource.ConstructGenericType(System.Type,System.Collections.Generic.IReadOnlyList{System.Type}) 5432f7c9e24615145d7b9a160f0126951dc5c671a6b2c14dd73a2c41223c2296 -->
<!-- api: M:Inno.Adapter.Modules.DotNet.ReflectionTypeCatalogSource.CanCreateInstance(System.Type) c3354ecd1e2fd60dabaaf36bdb05cfd65c604f77d5565c14c7c7a789ba1ab520 -->
<!-- api: M:Inno.Adapter.Modules.DotNet.ReflectionTypeCatalogSource.CreateInstance(System.Type) 90e771697235c90f48304b19c0a712fbe403b5fb2161bc9e5d1d627b81d1bc98 -->
<!-- api: M:Inno.Adapter.Modules.DotNet.ReflectionTypeCatalogSource.#ctor 5418b3c4801e37efa27c17350a3b8df9c8bb678d853504bbf6865801382d4947 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-abac8cbcd79a12d7"></a>[`public Inno.Adapter.Modules.DotNet.ReflectionTypeCatalogSource.ReflectionTypeCatalogSource()`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/Discovery/ReflectionTypeCatalogSource.cs#L15) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-c60b0a689bc7995d"></a>[`public bool Inno.Adapter.Modules.DotNet.ReflectionTypeCatalogSource.CanCreateInstance(System.Type type)`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/Discovery/ReflectionTypeCatalogSource.cs#L62) | Determines whether this deployment supplies a parameterless factory for a declared type.<br>返回：Whether instance construction is supported by the selected deployment.<br>`type`：The declared runtime type. | — | — |
| <a id="api-01688ef5890c1d32"></a>[`public System.Type? Inno.Adapter.Modules.DotNet.ReflectionTypeCatalogSource.ConstructGenericType(System.Type definition, System.Collections.Generic.IReadOnlyList<System.Type> arguments)`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/Discovery/ReflectionTypeCatalogSource.cs#L40) | Resolves a generic construction through the deployment's linked or dynamic type mechanism.<br>返回：The closed type, or null when its generic constraints reject these arguments.<br>`definition`：A generic declaration owned by the candidate generation.<br>`arguments`：The complete ordered set of closed type arguments. | — | `System.NotSupportedException`：A static deployment did not link the requested construction.<br>`System.ArgumentException`：The declaration is not generic, or the argument count or closedness is invalid. |
| <a id="api-bb14f6d93566778a"></a>[`public object Inno.Adapter.Modules.DotNet.ReflectionTypeCatalogSource.CreateInstance(System.Type type)`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/Discovery/ReflectionTypeCatalogSource.cs#L71) | Constructs a declared type through the selected deployment's factory mechanism.<br>返回：A new instance owned by the caller; missing factories or failed constructors throw.<br>`type`：The declared runtime type. | — | — |
| <a id="api-caf6f94f7c72224e"></a>[`public Inno.Extensibility.Catalogs.TypeCatalogMetadata Inno.Adapter.Modules.DotNet.ReflectionTypeCatalogSource.GetMetadata(System.Type type)`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/Discovery/ReflectionTypeCatalogSource.cs#L25) | Resolves immutable discovery facts for a declaration in this source.<br>返回：Complete metadata owned by the caller's candidate generation; unavailable declarations throw.<br>`type`：The declaration whose relationships and attributes are required. | — | — |
| <a id="api-cd97f776b3584c5d"></a>[`public System.Collections.Generic.IReadOnlyList<System.Type> Inno.Adapter.Modules.DotNet.ReflectionTypeCatalogSource.GetTypes(System.Reflection.Assembly assembly)`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/Discovery/ReflectionTypeCatalogSource.cs#L18) | Resolves the complete type set without publishing a partial generation.<br>返回：The complete type metadata for this assembly; unsupported or invalid metadata throws.<br>`assembly`：An assembly owned by the candidate module catalog. | — | — |
| <a id="api-73a53bb99a5acbe8"></a>[`public sealed class Inno.Adapter.Modules.DotNet.ReflectionTypeCatalogSource : Inno.Extensibility.Types.ITypeCatalogSource`](../../../backends/DotNet/runtime/Inno.Adapter.Modules.DotNet/Discovery/ReflectionTypeCatalogSource.cs#L15) | Discovers desktop authoring types through the selected managed runtime's reflection service. | — | — |

## 项目依赖

- [Inno.Core.Collections](../../core/Inno.Core.Collections.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Modules](../../extensibility/Inno.Extensibility.Modules.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Types](../../extensibility/Inno.Extensibility.Types.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
