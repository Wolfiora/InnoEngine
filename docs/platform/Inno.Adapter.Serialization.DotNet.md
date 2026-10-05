# Inno.Adapter.Serialization.DotNet

[Platform 索引](README.md) · [Wiki 首页](../README.md) · [Serialization](../core/Inno.Core.Serialization.md) · [Modules Adapter](Inno.Adapter.Modules.DotNet.md)

## 职责与边界

为 Editor 和构建宿主提供动态 .NET 序列化元数据。项目依赖中立的 `Inno.Core.Serialization`，负责反射成员、构造函数、恢复回调及闭合集合的构造策略。Core 不引用本项目；发行 Player 使用生成的静态来源。

## 全部公开 API

| API | 语义 |
| --- | --- |
| `ReflectionSerializationMetadataSource()` | 创建一个隔离的动态来源；没有进程全局注册表。 |
| `GetMetadata(Type)` | 返回指定闭合声明的不可变成员、构造、集合和恢复信息；不支持的声明或无效成员显式失败。 |

没有 protected 扩展点。调用方借用结果，不修改其成员列表；元数据中的访问委托属于相应类型的 generation。

## 初始化与所有权

```csharp
using Inno.Adapter.Serialization.DotNet;
using Inno.Core.Serialization;

using var serialization = new SerializationRegistry(types, new ReflectionSerializationMetadataSource());
byte[] data = serialization.Serialize(state, ownerContext);
```

`types` 是已经初始化的 `TypeCatalog`，`ownerContext` 由对象 owner 提供完整引用解析服务。Registry 在构造时建立 Converter generation；退出时先退休使用它的 Session，再释放 Registry 和类型目录。

来源按 `Type` 弱缓存元数据，不把 collectible 类型保存在强静态表中。成员顺序、重复键检查、属性可见性和恢复回调验证与静态来源遵循相同契约。恢复通知在完整操作成功后提交；构造或回调失败传播到调用方事务。

## 验证入口

`Inno.Core.Serialization.Tests` 验证动态来源的现有序列化行为；`Inno.Runtime.Generators.Tests` 验证生成访问器的编译、私有成员、结构体、闭合集合、跨程序集继承及无效声明诊断。
