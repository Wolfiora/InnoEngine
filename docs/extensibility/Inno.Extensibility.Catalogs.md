# Inno.Extensibility.Catalogs

[Extensibility 索引](README.md) · [Wiki 首页](../README.md) · [类型目录](Inno.Extensibility.Types.md) · [Runtime](../runtime/Inno.Runtime.md)

## 职责与依赖

这是不依赖其他引擎程序集的 Foundation 契约项目。它保存显式类型目录的发现事实和注册入口，
不扫描程序集、不加载 DLL、不执行构造函数、不管理代际事务，也不决定平台或托管运行时。

静态目录由每个程序集生成。基础程序集也能直接引用这个叶节点，而不反向引用带有
ModuleHost、Registry 和 execution 协调职责的 `Inno.Extensibility.Types`。
这是实施 Plan 时为维持依赖方向而增加的契约边界；BGCS 不依赖该项目。

## 公开 API

| 类型或成员 | 行为 |
| --- | --- |
| `TypeCatalogMetadata(...)` | 防御性复制一个声明的发现事实；所有集合必填 |
| `type` | 事实所属的 CLR 声明 |
| `baseTypes` | 从直接父类型开始、排除 System.Object 的继承链 |
| `interfaces` | 完整接口集合 |
| `declaredAttributes` | 直接声明的 Attribute 实例 |
| `inheritedAttributes` | 按继承和多实例规则合并后的有效 Attribute 实例 |
| `parameterlessOverrides` | 无参数实例重写名称及其虚方法槽的所属基类 |
| `ITypeCatalogRegistrar.Register(metadata, factory)` | 贡献一个声明和可选无参数工厂；重复声明失败 |
| `ITypeCatalogRegistrar.RegisterFactory(type, factory)` | 贡献完全封闭的泛型工厂；同一构造重复贡献时保留第一份 |
| `ITypeCatalogRegistrar.RejectGenericConstruction(definition, arguments)` | 贡献编译期确认不满足约束的参数组合；复制参数并幂等记录，与正向工厂冲突时失败 |

该项目没有 protected 扩展点。普通功能通过生成器贡献目录，只有自定义组合入口需要实现或调用 registrar。

## 组合示例

```csharp
using Inno.Extensibility.Catalogs;

public sealed class Example;

public static class ExampleCatalog
{
    public static void Register(ITypeCatalogRegistrar registrar)
    {
        registrar.Register(new TypeCatalogMetadata(typeof(Example), [], [], [], [], []),
            static () => new Example());
    }
}
```

生成目录使用同一协议，由 `StaticTypeCatalogSource` 在组合期间接收。
封闭泛型工厂不作为另一种声明进入扩展发现，不重复生成 Stable Type ID。

## 生命周期与错误

事实中的 Type、Attribute 和工厂属于当前 generation；不能持久化或跨代长期缓存。
目录创建结束或贡献失败后 registrar 封闭，保留它继续写入会抛出 `InvalidOperationException`。
元数据对象只冻结集合，Attribute 实例仍是本代对象，不提供可变设置存储。
构造函数按需执行；工厂异常向调用者传播，缺失静态泛型实例明确失败。
编译期确认违反泛型约束的组合返回 null，让统一 Converter Registry 继续选择其他候选；
这与一个合法实例未被链接的部署错误严格区分。生成器为带约束的私有构造函数生成对应的泛型类型入口，
保留类型参数位置和完整约束，不依赖运行时反射构造。

## 验证

`RuntimeModuleCatalogGeneratorTests` 编译并执行生成目录；`StaticGenericFactoryTests` 覆盖
发现隔离、重复工厂、缺失定义、非法结果、成功和失败后的 registrar 退休。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Extensibility.Catalogs.ITypeCatalogRegistrar`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Extensibility.Catalogs.ITypeCatalogRegistrar`](../../src/foundation/extensibility/Inno.Extensibility.Catalogs/ITypeCatalogRegistrar.cs#L13) | Accepts explicit declarations and linked generic factories while a static catalog is being composed. |
| [`void Inno.Extensibility.Catalogs.ITypeCatalogRegistrar.Register(Inno.Extensibility.Catalogs.TypeCatalogMetadata metadata, System.Func<object>? factory)`](../../src/foundation/extensibility/Inno.Extensibility.Catalogs/ITypeCatalogRegistrar.cs#L30) | Registers immutable discovery metadata and an optional parameterless construction function. |
| [`void Inno.Extensibility.Catalogs.ITypeCatalogRegistrar.RegisterFactory(System.Type type, System.Func<object> factory)`](../../src/foundation/extensibility/Inno.Extensibility.Catalogs/ITypeCatalogRegistrar.cs#L53) | Registers a closed generic construction linked by the contributing assembly. |
| [`void Inno.Extensibility.Catalogs.ITypeCatalogRegistrar.RejectGenericConstruction(System.Type definition, System.Collections.Generic.IReadOnlyList<System.Type> arguments)`](../../src/foundation/extensibility/Inno.Extensibility.Catalogs/ITypeCatalogRegistrar.cs#L80) | Records a construction whose argument set was rejected by the declaration's compile-time constraints. |

### `Inno.Extensibility.Catalogs.TypeCatalogMetadata`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Extensibility.Catalogs.TypeCatalogMetadata`](../../src/foundation/extensibility/Inno.Extensibility.Catalogs/TypeCatalogMetadata.cs#L10) | Freezes a declaration's discovery facts independently of its runtime's reflection implementation. |
| [`Inno.Extensibility.Catalogs.TypeCatalogMetadata.TypeCatalogMetadata(System.Type type, System.Collections.Generic.IReadOnlyList<System.Type> baseTypes, System.Collections.Generic.IReadOnlyList<System.Type> interfaces, System.Collections.Generic.IReadOnlyList<System.Attribute> declaredAttributes, System.Collections.Generic.IReadOnlyList<System.Attribute> inheritedAttributes, System.Collections.Generic.IReadOnlyList<(string name, System.Type declaringBase)> parameterlessOverrides)`](../../src/foundation/extensibility/Inno.Extensibility.Catalogs/TypeCatalogMetadata.cs#L36) | Captures type relationships and attributes for one candidate generation. |
| [`System.Collections.Generic.IReadOnlyList<(string name, System.Type declaringBase)> Inno.Extensibility.Catalogs.TypeCatalogMetadata.parameterlessOverrides`](../../src/foundation/extensibility/Inno.Extensibility.Catalogs/TypeCatalogMetadata.cs#L86) | Gets effective parameterless overrides without retaining runtime MethodInfo objects. |
| [`System.Collections.Generic.IReadOnlyList<System.Attribute> Inno.Extensibility.Catalogs.TypeCatalogMetadata.declaredAttributes`](../../src/foundation/extensibility/Inno.Extensibility.Catalogs/TypeCatalogMetadata.cs#L76) | Gets directly declared attribute instances owned by this generation. |
| [`System.Collections.Generic.IReadOnlyList<System.Attribute> Inno.Extensibility.Catalogs.TypeCatalogMetadata.inheritedAttributes`](../../src/foundation/extensibility/Inno.Extensibility.Catalogs/TypeCatalogMetadata.cs#L81) | Gets the effective attribute set used by inherited extension discovery. |
| [`System.Collections.Generic.IReadOnlyList<System.Type> Inno.Extensibility.Catalogs.TypeCatalogMetadata.baseTypes`](../../src/foundation/extensibility/Inno.Extensibility.Catalogs/TypeCatalogMetadata.cs#L66) | Gets the ordered inheritance chain, excluding System.Object. |
| [`System.Collections.Generic.IReadOnlyList<System.Type> Inno.Extensibility.Catalogs.TypeCatalogMetadata.interfaces`](../../src/foundation/extensibility/Inno.Extensibility.Catalogs/TypeCatalogMetadata.cs#L71) | Gets the complete implemented interface set. |
| [`System.Type Inno.Extensibility.Catalogs.TypeCatalogMetadata.type`](../../src/foundation/extensibility/Inno.Extensibility.Catalogs/TypeCatalogMetadata.cs#L61) | Gets the declaration whose metadata is owned by this generation. |

## 项目依赖

