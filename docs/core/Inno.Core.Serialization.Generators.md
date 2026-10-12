# Inno.Core.Serialization.Generators

[Core 索引](README.md) · [Serialization](Inno.Core.Serialization.md)

该 Analyzer project 提供两个公开 `IIncrementalGenerator` 实现。`Initialize(context)` 是它们唯一的公开方法，由 Roslyn 调用；不作为 gameplay API 使用。

| 生成器 | 输入与产物 |
| --- | --- |
| `SerializationConverterGenerator` | 为标注 `GenerateSerializationConverterAttribute` 的闭合 DTO 生成 Converter 和程序集注册入口。 |
| `SerializationMetadataGenerator` | 为当前程序集的序列化声明和可达值形状生成成员访问器、构造、恢复、集合构造以及 `RuntimeSerializationMetadataCatalog`。 |

生成器验证可用构造器、property key、支持的 scalar/collection/map 类型和重复声明。生成代码与目标 DTO 同 compilation，因此可以访问 internal 类型；它带明确 generated marker，由手写 XML 检查豁免。

多态、对象身份、引用图、自定义恢复不变量和外部类型不使用自动生成路径，继续实现显式 `SerializationConverter<T>`。

上述限制适用于自动 DTO Converter。元数据生成仍支持显式 Converter 对应类型的声明访问、私有成员和恢复回调；引用语义由既有 Converter 与 owner context 负责。

继承使用基类所属目录的访问器；每个声明分别拥有自己的成员和恢复回调。结构体通过类型化访问器修改 boxed value。数组、序列、映射、可赋值的集合接口、单参数构造、公开 `Create` / `CreateRange` 及 `Add` 构造在编译阶段降低为具体泛型调用；没有平台类型分支。

`INNOSER001`–`INNOSER004` 检查自动 DTO Converter 的类型、构造、访问和键；`INNOSER010` 拒绝不一致的成员访问，`INNOSER011` 拒绝错误或重复的恢复回调，`INNOSER012` 要求私有形状的词法 owner 可以被生成器扩展。生成物由所属 compilation 管理，不保存为创作源码。

生成器测试实际编译产物，运行静态来源的往返序列化，并覆盖私有构造、字段、setter、继承 hook、结构体、数组、Immutable / ReadOnly 集合及自定义集合。

## 私有形状与封装

私有嵌套 DTO、私有结构体集合元素以及公开类型内的私有成员类型都保持原访问级别。生成器在所属 `partial` 类型中声明内部元数据目录，程序集目录汇集这些贡献。多层私有 owner 逐层传递贡献；程序集目录不直接命名它无法访问的私有类型。

所有需要承载内部目录的 containing owner 必须是 nongeneric `partial` 类型。开放泛型不作为闭合部署形状生成；实际可达的闭合泛型必须能在编译阶段表达，否则报告构建诊断。这里不引入反射 fallback，也不为了生成器扩大数据类型的访问级别。

候选序列化调用先根据当前公共/内部泛型方法名作语法筛选，再由 Roslyn 绑定实际方法，保留类型推导调用。元数据、typed accessors 与生成目录受同一程序集 generation 管理。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Core.Serialization.Generators.SerializationConverterGenerator`

<!-- api: T:Inno.Core.Serialization.Generators.SerializationConverterGenerator 87e78fe78cc35ac29846acf4d831654974053ef2e2ddb0286c04502bdabb01f5 -->
<!-- api: M:Inno.Core.Serialization.Generators.SerializationConverterGenerator.Initialize(Microsoft.CodeAnalysis.IncrementalGeneratorInitializationContext) 35df1119527fc624fca9f2e122b27e003dc4d9fd0091a185a8361be0ffaa5b64 -->
<!-- api: M:Inno.Core.Serialization.Generators.SerializationConverterGenerator.#ctor c9f3e1388a52741487744d1d3f592c5d58f9bec65069fdb10847cb625a5502a6 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-5f1664c66d003541"></a>[`public Inno.Core.Serialization.Generators.SerializationConverterGenerator.SerializationConverterGenerator()`](../../src/foundation/core/Inno.Core.Serialization.Generators/SerializationConverterGenerator.cs#L17) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-84626d9ea4c8c8b1"></a>[`public void Inno.Core.Serialization.Generators.SerializationConverterGenerator.Initialize(Microsoft.CodeAnalysis.IncrementalGeneratorInitializationContext context)`](../../src/foundation/core/Inno.Core.Serialization.Generators/SerializationConverterGenerator.cs#L64) | Registers the incremental syntax and source-production pipeline.<br>`context`：The Roslyn initialization context that receives generator registrations. | — | — |
| <a id="api-a08888a2bac71405"></a>[`public sealed class Inno.Core.Serialization.Generators.SerializationConverterGenerator : Microsoft.CodeAnalysis.IIncrementalGenerator`](../../src/foundation/core/Inno.Core.Serialization.Generators/SerializationConverterGenerator.cs#L17) | Generates stateless serialization converters for explicitly annotated closed data-transfer types. | — | — |

### `Inno.Core.Serialization.Generators.SerializationMetadataGenerator`

<!-- api: T:Inno.Core.Serialization.Generators.SerializationMetadataGenerator a702c8a6f48e176dc8b6bdc191fee8a67bb472de9e8b382b799066e6598d3cc5 -->
<!-- api: M:Inno.Core.Serialization.Generators.SerializationMetadataGenerator.Initialize(Microsoft.CodeAnalysis.IncrementalGeneratorInitializationContext) 476e7c294f5f915f9f9d1b99c11e0dce40b9df293bdb68529b8e32cff6520c9a -->
<!-- api: M:Inno.Core.Serialization.Generators.SerializationMetadataGenerator.#ctor d35b528500a7460f521f8ce28cbaaffaa0b4ab357f884e38a1d657f74f79a418 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-0b253547777ee5fe"></a>[`public Inno.Core.Serialization.Generators.SerializationMetadataGenerator.SerializationMetadataGenerator()`](../../src/foundation/core/Inno.Core.Serialization.Generators/SerializationMetadataGenerator.cs#L15) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-4cd8656080cc0509"></a>[`public void Inno.Core.Serialization.Generators.SerializationMetadataGenerator.Initialize(Microsoft.CodeAnalysis.IncrementalGeneratorInitializationContext context)`](../../src/foundation/core/Inno.Core.Serialization.Generators/SerializationMetadataGenerator.cs#L33) | Called to initialize the generator and register generation steps via callbacks on the context<br>`context`：The Microsoft.CodeAnalysis.IncrementalGeneratorInitializationContext to register callbacks on | — | — |
| <a id="api-0ba919101b60c047"></a>[`public sealed class Inno.Core.Serialization.Generators.SerializationMetadataGenerator : Microsoft.CodeAnalysis.IIncrementalGenerator`](../../src/foundation/core/Inno.Core.Serialization.Generators/SerializationMetadataGenerator.cs#L15) | Generates closed member access, restoration hooks and collection construction for static deployments. | — | — |

## 项目依赖

