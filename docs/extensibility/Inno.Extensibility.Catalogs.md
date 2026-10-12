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

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Extensibility.Catalogs.ITypeCatalogRegistrar`

<!-- api: T:Inno.Extensibility.Catalogs.ITypeCatalogRegistrar 4d156d218d3770bc3bfe49d50d760bfc14de006a5c27a487b7cf853aab90a3fe -->
<!-- api: M:Inno.Extensibility.Catalogs.ITypeCatalogRegistrar.Register(Inno.Extensibility.Catalogs.TypeCatalogMetadata,System.Func{System.Object}) cb36f9187c466801ecdab92ba8c7c4ba1ff9da2cc647eb15cd6a4d2e4dc0fec6 -->
<!-- api: M:Inno.Extensibility.Catalogs.ITypeCatalogRegistrar.RegisterFactory(System.Type,System.Func{System.Object}) 3747f7891a68f57870793b8f51dc84e912565eb0d2d29caba70d1a75a06c0375 -->
<!-- api: M:Inno.Extensibility.Catalogs.ITypeCatalogRegistrar.RejectGenericConstruction(System.Type,System.Collections.Generic.IReadOnlyList{System.Type}) 2a342742f74bf53d8ca9442947da2aba638b778ab744db40ef4d5011f2a9f42d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b7648c1df4753ed4"></a>[`void Inno.Extensibility.Catalogs.ITypeCatalogRegistrar.Register(Inno.Extensibility.Catalogs.TypeCatalogMetadata metadata, System.Func<object>? factory)`](../../src/foundation/extensibility/Inno.Extensibility.Catalogs/ITypeCatalogRegistrar.cs#L30) | Registers immutable discovery metadata and an optional parameterless construction function.<br>`metadata`：Complete discovery facts for one assembly-local declaration.<br>`factory`：Creates a new instance, or null when the declaration is not constructible. | — | `System.ArgumentException`：The declaration has already been registered.<br>`System.InvalidOperationException`：The caller runs on another thread or retained this registrar after catalog composition completed. |
| <a id="api-7618258ec059790b"></a>[`void Inno.Extensibility.Catalogs.ITypeCatalogRegistrar.RegisterFactory(System.Type type, System.Func<object> factory)`](../../src/foundation/extensibility/Inno.Extensibility.Catalogs/ITypeCatalogRegistrar.cs#L53) | Registers a closed generic construction linked by the contributing assembly.<br>`type`：A fully closed generic type whose definition is contributed by the completed catalog.<br>`factory`：Creates a new instance of the linked construction without dynamic code generation. | Repeated registrations of the same closed construction are idempotent; the first factory is retained. | `System.ArgumentException`：The type is not a fully closed generic construction.<br>`System.InvalidOperationException`：The caller runs on another thread or retained this registrar after catalog composition completed. |
| <a id="api-d08b97d5bfbcd615"></a>[`void Inno.Extensibility.Catalogs.ITypeCatalogRegistrar.RejectGenericConstruction(System.Type definition, System.Collections.Generic.IReadOnlyList<System.Type> arguments)`](../../src/foundation/extensibility/Inno.Extensibility.Catalogs/ITypeCatalogRegistrar.cs#L80) | Records a construction whose argument set was rejected by the declaration's compile-time constraints.<br>`definition`：The generic declaration contributed by the completed catalog.<br>`arguments`：The complete closed argument set. The registrar copies this collection during registration. | This fact lets static resolution skip an inapplicable converter without attempting dynamic generic creation. Unknown constructions remain explicit deployment errors. Repeated rejections are idempotent. | `System.ArgumentException`：The declaration or argument set is incomplete, open, or conflicts with a registered factory.<br>`System.ArgumentNullException`：The declaration or argument collection is null.<br>`System.InvalidOperationException`：The caller runs on another thread or retained this registrar after catalog composition completed. |
| <a id="api-3c11be680076b4b1"></a>[`public interface Inno.Extensibility.Catalogs.ITypeCatalogRegistrar`](../../src/foundation/extensibility/Inno.Extensibility.Catalogs/ITypeCatalogRegistrar.cs#L13) | Accepts explicit declarations and linked generic factories while a static catalog is being composed. | Generated contributors run synchronously on the composition thread and must not retain the registrar. Its registrations belong to the resulting generation. Constructors execute only when requested. | — |

### `Inno.Extensibility.Catalogs.TypeCatalogMetadata`

<!-- api: T:Inno.Extensibility.Catalogs.TypeCatalogMetadata e6b887f02cbc4d6276a16dfaa37cafac7005d1a635f2907aae91ec03699bab8b -->
<!-- api: M:Inno.Extensibility.Catalogs.TypeCatalogMetadata.#ctor(System.Type,System.Collections.Generic.IReadOnlyList{System.Type},System.Collections.Generic.IReadOnlyList{System.Type},System.Collections.Generic.IReadOnlyList{System.Attribute},System.Collections.Generic.IReadOnlyList{System.Attribute},System.Collections.Generic.IReadOnlyList{System.ValueTuple{System.String,System.Type}}) 5f24c3565575eff0108f353df59d8eb3f8b5bd231ac12218e8592baecb946516 -->
<!-- api: P:Inno.Extensibility.Catalogs.TypeCatalogMetadata.type d5a9c01629cfc9ca7bd660302880e6f5582ee11cb541c28ab5135da5754d39fe -->
<!-- api: P:Inno.Extensibility.Catalogs.TypeCatalogMetadata.baseTypes d5da597d50fd2afd345835e8fcc483dfcc78bccdf5818eec60ea7a86bd77873e -->
<!-- api: P:Inno.Extensibility.Catalogs.TypeCatalogMetadata.interfaces 72c93e4edadb1324d15d3c3e8f4fd8bf12409b7f40558467f8e4f42d8e59579c -->
<!-- api: P:Inno.Extensibility.Catalogs.TypeCatalogMetadata.declaredAttributes 1804a50ee2f2a5a2736c974c28fa0272c1d3f404f01e04418f39a3cec4ba058b -->
<!-- api: P:Inno.Extensibility.Catalogs.TypeCatalogMetadata.inheritedAttributes 2d2dd25e29661d46615adc2f01bf41e283a757a041a8c54968948f934fe80988 -->
<!-- api: P:Inno.Extensibility.Catalogs.TypeCatalogMetadata.parameterlessOverrides 6a42b14ac9e913772a840f7e83ec250cff4d3adb16849ebbde6a77e45e8e10b0 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-ad0229f445032706"></a>[`public Inno.Extensibility.Catalogs.TypeCatalogMetadata.TypeCatalogMetadata(System.Type type, System.Collections.Generic.IReadOnlyList<System.Type> baseTypes, System.Collections.Generic.IReadOnlyList<System.Type> interfaces, System.Collections.Generic.IReadOnlyList<System.Attribute> declaredAttributes, System.Collections.Generic.IReadOnlyList<System.Attribute> inheritedAttributes, System.Collections.Generic.IReadOnlyList<(string name, System.Type declaringBase)> parameterlessOverrides)`](../../src/foundation/extensibility/Inno.Extensibility.Catalogs/TypeCatalogMetadata.cs#L36) | Captures type relationships and attributes for one candidate generation.<br>`type`：The declared type belonging to the contributing assembly.<br>`baseTypes`：Ordered base types, starting with the immediate base and excluding System.Object.<br>`interfaces`：All implemented interfaces, including inherited interfaces.<br>`declaredAttributes`：Attributes declared directly on this type.<br>`inheritedAttributes`：The effective attributes after applying inheritance and multiplicity rules.<br>`parameterlessOverrides`：Effective parameterless instance overrides, paired with the declaration owning their virtual slot. | — | `System.ArgumentNullException`：A required declaration or collection is null. |
| <a id="api-05c82d23fce9c40c"></a>[`public System.Collections.Generic.IReadOnlyList<System.Type> Inno.Extensibility.Catalogs.TypeCatalogMetadata.baseTypes { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Catalogs/TypeCatalogMetadata.cs#L66) | Gets the ordered inheritance chain, excluding System.Object. | — | — |
| <a id="api-e5afe2d435b1099f"></a>[`public System.Collections.Generic.IReadOnlyList<System.Attribute> Inno.Extensibility.Catalogs.TypeCatalogMetadata.declaredAttributes { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Catalogs/TypeCatalogMetadata.cs#L76) | Gets directly declared attribute instances owned by this generation. | — | — |
| <a id="api-ebd79da2076efdcb"></a>[`public System.Collections.Generic.IReadOnlyList<System.Attribute> Inno.Extensibility.Catalogs.TypeCatalogMetadata.inheritedAttributes { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Catalogs/TypeCatalogMetadata.cs#L81) | Gets the effective attribute set used by inherited extension discovery. | — | — |
| <a id="api-8c5cad816b73eec6"></a>[`public System.Collections.Generic.IReadOnlyList<System.Type> Inno.Extensibility.Catalogs.TypeCatalogMetadata.interfaces { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Catalogs/TypeCatalogMetadata.cs#L71) | Gets the complete implemented interface set. | — | — |
| <a id="api-24292b889ef6d56a"></a>[`public System.Collections.Generic.IReadOnlyList<(string name, System.Type declaringBase)> Inno.Extensibility.Catalogs.TypeCatalogMetadata.parameterlessOverrides { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Catalogs/TypeCatalogMetadata.cs#L86) | Gets effective parameterless overrides without retaining runtime MethodInfo objects. | — | — |
| <a id="api-7411abb258f56958"></a>[`public System.Type Inno.Extensibility.Catalogs.TypeCatalogMetadata.type { get; }`](../../src/foundation/extensibility/Inno.Extensibility.Catalogs/TypeCatalogMetadata.cs#L61) | Gets the declaration whose metadata is owned by this generation. | — | — |
| <a id="api-43e36eec7b98f7f7"></a>[`public sealed class Inno.Extensibility.Catalogs.TypeCatalogMetadata`](../../src/foundation/extensibility/Inno.Extensibility.Catalogs/TypeCatalogMetadata.cs#L10) | Freezes a declaration's discovery facts independently of its runtime's reflection implementation. | — | — |

## 项目依赖

