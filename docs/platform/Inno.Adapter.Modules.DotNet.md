# Inno.Adapter.Modules.DotNet

[Platform 索引](README.md) · [Wiki 首页](../README.md) · [Modules](../extensibility/Inno.Extensibility.Modules.md) · [Types](../extensibility/Inno.Extensibility.Types.md)

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
