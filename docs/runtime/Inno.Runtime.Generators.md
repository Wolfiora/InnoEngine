# Inno.Runtime.Generators

[Runtime 索引](README.md) · [Wiki 首页](../README.md) · [Contracts](Inno.Runtime.Contracts.md) · [Default](Inno.Engine.Default.md)

本项目是 Roslyn incremental generator，在业务项目作为 Analyzer，在作者编译器作为生成库使用；不进入 Player 执行闭包。公开实现入口是各生成器的 `Initialize`，由编译器调用，不是游戏 API。

同一发行程序集中的 `[RuntimeSubsystemRegistration("stable.id")]` 标记静态 factory 方法，参数是明确的 composition 类型，返回 `IRuntimeSubsystemFactory`。带 `[RuntimeSubsystemCatalog]` 的静态 partial 方法返回 `IReadOnlyList<IRuntimeSubsystemFactory>`，生成器按参数类型收集声明，按 ID 排序并输出直接 C# 调用与冻结列表。返回 factory 的 descriptor ID 还会被再次核对。

`INNORUN001` 拒绝空 catalog、重复 ID、错误 factory 返回边界和不支持的 catalog 形状。没有运行时反射，没有扫描任意未引用程序集，也没有 Plugin 类型中央名单。

新增引擎能力时需要：领域 contract/runtime、发行项目引用、发行项目中该领域的一个声明文件。无需修改 Shell、EditorHost、GamePlayerHost 或 generator 中的领域 switch。生成器测试覆盖新增声明自动进入 catalog、重复 ID 和无效声明。

## 编译代码目录

`RuntimeModuleCatalogGenerator` 输出程序集自己的 `Generated.RuntimeModuleCatalog`，包含 assembly、显式 Register 与 ResolveType。注册事实包括直接/继承 Attribute、基类、接口、override、私有无参数构造 factory；required 类型不伪造默认 factory。外部 internal 声明通过其 owning catalog 的明确 ResolveType 取得，没有运行时探测。

`RuntimeFactoryGenerator` 由 entry project 的 `InnoGeneratePlayerComposition` 启用，组合已引用的本地 catalog，生成共享静态模块、类型、序列化来源与 activator。每游戏发布必须提供由构建生成的 PlayerDeploymentDefinition；engine-only build 的空 code closure 不能启动非空游戏 manifest。

SerializationConverterGenerator 另外输出本程序集的 RuntimeSerializationCatalog，确保另一个 generator 无法看到本轮生成符号时，转换器仍进入同一 static type source。脚本编译也运行本地注册与序列化生成器，生成器 MVID 参与编译缓存身份。

同一 Analyzer 程序集中的 `SerializationMetadataGenerator` 输出 RuntimeSerializationMetadataCatalog，通过公开 `Register` / `GetMetadata` 提供闭合声明的构造、访问与恢复信息。私有成员由所属程序集的生成访问器读取，跨程序集继承通过所属目录组合；不在 Player 中使用反射探测 private setter。无效访问声明产生 `INNOSER010`，错误或重复恢复回调产生 `INNOSER011`。这两个入口服务于生成 composition，不属于 gameplay API。

生成器测试除检查源码外，还编译生成代码并调用公开目录，验证 metadata 与 factory 能实际运行。AOT 泛型和具体平台启动仍须经过独立发行验收。

封闭泛型工厂由显式类型使用、泛型调用参数及可序列化字段的值闭包推导。
分析阶段检查泛型约束，合法组合注册工厂，不满足约束的已知组合记录为拒绝事实。
`INNORUN002` 在构建阶段拒绝无法访问或缺少无参数构造入口的封闭 Converter。
私有泛型构造入口使用对应的泛型辅助类型，保留声明的类型参数顺序与约束，
符合 [UnsafeAccessor 的签名匹配规则](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.unsafeaccessorattribute)。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Runtime.Generators.RuntimeFactoryGenerator`

<!-- api: T:Inno.Runtime.Generators.RuntimeFactoryGenerator 570fef67bd1c5c0aa2e497e378d8e3f1965366a5a5739078f488ddb78bc92b2c -->
<!-- api: M:Inno.Runtime.Generators.RuntimeFactoryGenerator.Initialize(Microsoft.CodeAnalysis.IncrementalGeneratorInitializationContext) 7b820400eecf40ef89876d97793959d3e3f37b013483457aaa6d1587e5deb9b5 -->
<!-- api: M:Inno.Runtime.Generators.RuntimeFactoryGenerator.#ctor d949ff89e15bbe9c3a89bd8004124dfc3d2402767ca896edd650880006a859a4 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f8e2cda4cdc0768b"></a>[`public Inno.Runtime.Generators.RuntimeFactoryGenerator.RuntimeFactoryGenerator()`](../../src/runtime/generators/Inno.Runtime.Generators/Registration/RuntimeFactoryGenerator.cs#L13) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-d9bd2a7732188aa3"></a>[`public void Inno.Runtime.Generators.RuntimeFactoryGenerator.Initialize(Microsoft.CodeAnalysis.IncrementalGeneratorInitializationContext context)`](../../src/runtime/generators/Inno.Runtime.Generators/Registration/RuntimeFactoryGenerator.cs#L16) | Called to initialize the generator and register generation steps via callbacks on the context<br>`context`：The Microsoft.CodeAnalysis.IncrementalGeneratorInitializationContext to register callbacks on | — | — |
| <a id="api-5508a99212991841"></a>[`public sealed class Inno.Runtime.Generators.RuntimeFactoryGenerator : Microsoft.CodeAnalysis.IIncrementalGenerator`](../../src/runtime/generators/Inno.Runtime.Generators/Registration/RuntimeFactoryGenerator.cs#L13) | Composes explicit assembly-local metadata catalogs for a Player entry project selected by its build. | — | — |

### `Inno.Runtime.Generators.RuntimeModuleCatalogGenerator`

<!-- api: T:Inno.Runtime.Generators.RuntimeModuleCatalogGenerator b03246f77c5198204594dec7955771775ed32a8084cbc449ad1a2b85770146c1 -->
<!-- api: M:Inno.Runtime.Generators.RuntimeModuleCatalogGenerator.Initialize(Microsoft.CodeAnalysis.IncrementalGeneratorInitializationContext) a8a3562b05077d33f188a746a6bd79ff5d38af9e61b97ed3b1162596ff2445ec -->
<!-- api: M:Inno.Runtime.Generators.RuntimeModuleCatalogGenerator.#ctor 91680322f697eec06122858ae0cda74baf8f2dd93b6c39c921923137f3776baf -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-740008bc3f0e923b"></a>[`public Inno.Runtime.Generators.RuntimeModuleCatalogGenerator.RuntimeModuleCatalogGenerator()`](../../src/runtime/generators/Inno.Runtime.Generators/Registration/RuntimeModuleCatalogGenerator.cs#L14) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-289dd1ea975bce09"></a>[`public void Inno.Runtime.Generators.RuntimeModuleCatalogGenerator.Initialize(Microsoft.CodeAnalysis.IncrementalGeneratorInitializationContext context)`](../../src/runtime/generators/Inno.Runtime.Generators/Registration/RuntimeModuleCatalogGenerator.cs#L22) | Registers deterministic catalog generation from the current assembly's declared symbols.<br>`context`：The compiler's incremental registration boundary. | — | — |
| <a id="api-f2e81d19a9af8c96"></a>[`public sealed class Inno.Runtime.Generators.RuntimeModuleCatalogGenerator : Microsoft.CodeAnalysis.IIncrementalGenerator`](../../src/runtime/generators/Inno.Runtime.Generators/Registration/RuntimeModuleCatalogGenerator.cs#L14) | Emits a local, explicit type and construction catalog for statically deployed managed assemblies. | — | — |

### `Inno.Runtime.Generators.RuntimeSubsystemGenerator`

<!-- api: T:Inno.Runtime.Generators.RuntimeSubsystemGenerator d91c4ebedb8b81936f72b75cf19e39f6b95939620e5e4832edc376ce2eae24c5 -->
<!-- api: M:Inno.Runtime.Generators.RuntimeSubsystemGenerator.Initialize(Microsoft.CodeAnalysis.IncrementalGeneratorInitializationContext) 5fdb44bb3d5feb9501e294a04f509ed6118cd6080e79ffeea8790e0f4e00b506 -->
<!-- api: M:Inno.Runtime.Generators.RuntimeSubsystemGenerator.#ctor e07053cb1a4e18f6ecae9b6df7fa96f50de9a34b67b66e3b539dee5aefccbf0b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-37a09f7fa365d6fe"></a>[`public Inno.Runtime.Generators.RuntimeSubsystemGenerator.RuntimeSubsystemGenerator()`](../../src/runtime/generators/Inno.Runtime.Generators/RuntimeSubsystemGenerator.cs#L15) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-0e27f86c72cc1348"></a>[`public void Inno.Runtime.Generators.RuntimeSubsystemGenerator.Initialize(Microsoft.CodeAnalysis.IncrementalGeneratorInitializationContext context)`](../../src/runtime/generators/Inno.Runtime.Generators/RuntimeSubsystemGenerator.cs#L26) | Registers incremental discovery of composition methods and catalog declarations.<br>`context`：The compiler-owned incremental generation context. | — | — |
| <a id="api-320cc7c2d65360bb"></a>[`public sealed class Inno.Runtime.Generators.RuntimeSubsystemGenerator : Microsoft.CodeAnalysis.IIncrementalGenerator`](../../src/runtime/generators/Inno.Runtime.Generators/RuntimeSubsystemGenerator.cs#L15) | Generates deterministic, reflection-free catalogs from local strongly typed subsystem declarations. | — | — |

## 项目依赖

