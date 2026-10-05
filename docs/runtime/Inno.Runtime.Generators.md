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
符合 [.NET 9 UnsafeAccessor 的签名匹配规则](https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/9.0/unsafeaccessor-generics)。
