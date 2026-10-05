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
