# Inno.Core.Serialization

[上一页：Identity](Inno.Core.Identity.md) · [Core 索引](README.md) · [Extensibility](../extensibility/Inno.Extensibility.Types.md)

`Inno.Core.Serialization` 提供确定性的二进制对象图格式、基于 attribute 的属性持久化，以及通过 TypeRegistry 自动发现的 Converter。它既支持“序列化完整 `ISerializable` 根对象”，也支持用 Writer/Reader 显式定义 schema。

## 初始化

Shader 等 Editor 创作回调可以借用宿主的 `SerializationRegistry`，脚本侧只导出数据编解码及属性读取。
构造 Registry、捕获 generation 和底层属性恢复快照仍由宿主管理，不进入脚本 API。运行时脚本不导出 Registry。

`SerializationRegistry` 是实例服务，必须在 `ModuleHost` 与 `TypeCatalog` 之后创建。构造时显式注入 `ISerializationMetadataSource`：Editor 使用独立 DotNet Adapter，Player 使用源生成器产生的静态目录。Converter Registry 跟随 TypeCache 的候选事务刷新。

```csharp
using var serialization = new SerializationRegistry(types, serializationMetadataSource);
```

| SerializationRegistry 成员 | 说明 |
| --- | --- |
| `SerializationRegistry(types, metadata)` / `Dispose()` | 创建并释放本实例的 Converter generation；借用类型目录和元数据来源。 |
| `CaptureGeneration()` | 捕获当前 Converter generation，调用方负责释放，适合跨异步阶段的确定性操作。 |
| `GetMetadata(Type)` | 从当前来源取得声明访问器，供宿主领域流程复用；不能跨 generation 退休保留，不导出到脚本。 |
| `GetProperties(ISerializable)` | 返回稳定排序且允许运行时读取的 `SerializedProperty`。 |
| `CaptureProperties(ISerializable, context?)` | 把每个持久成员独立编码为带声明类型的当前代际快照。 |
| `RestoreProperties(target, snapshots, mode, context?)` | 使用 Strict 或 CollectFailures 恢复独立成员快照；后者收集当前恢复失败，不读取旧格式或转换旧 schema。 |
| `CapturePropertyData(value, name, context?)` | 把一个 persistent property 编码为独立的中立 bytes。 |
| `CapturePropertiesData(value, context?)` | 把全部 persistent property snapshots 编码为独立的中立 bytes。 |
| `RestorePropertiesData(target, data, mode, context?)` | 从 property-data bytes 恢复既有对象，不序列化 owning graph。 |
| `Serialize<T>(T, context?)` | 把 class `ISerializable` 根对象编码为确定性二进制数据。 |
| `Deserialize<T>(ReadOnlySpan<byte>, context?)` | 创建并恢复一个新根对象。 |
| `Restore<T>(T target, ReadOnlySpan<byte>, context?)` | 将数据恢复到既有实例，适合身份必须保留的对象。 |
| `Encode(Action<SerializationWriter>, context?)` | 用手写 structured schema 编码。 |
| `Decode<TResult>(bytes, Func<SerializationReader,TResult>, context?)` | 用手写 schema 解码并返回结果。 |
| `SerializedIdentityRemapper.Rewrite(source, identities, paths?)` | 在已识别的二进制对象或属性快照中精确重写 Guid 与完整路径字符串；嵌套 payload 递归处理，未知格式原样复制。用于样例克隆等需要保持序列化结构的事务。 |

## 元数据来源与静态部署

| API | 稳定语义 |
| --- | --- |
| `ISerializationMetadataSource.GetMetadata(Type)` | 提供指定闭合声明的访问与构造信息；没有隐式反射 fallback。 |
| `SerializationMemberMetadata(name, type, visibility, getter, setter)` | 验证访问能力并冻结一个成员；`GetValue` / `SetValue` 执行选定访问器。 |
| `SerializationCollectionMetadata(elementType, keyType, buildSequence, buildMap, enumerateMap)` | 描述完整序列或映射的类型化构造与枚举；`keyType` 为 null 时表示序列。 |
| `SerializationTypeMetadata(type, members, factory, restored, collection, requiresConverter, inherited)` | 复制有序成员，验证唯一键，组合基类恢复回调；`CreateInstance` 调用选定构造器，缺失或失败显式抛异常。 |
| `StaticSerializationMetadataSource(catalogs)` | 一次性执行程序集注册入口并释放注册委托；拒绝互相矛盾的闭合声明。 |

这些 API 由 composition 和生成器使用，不导出到 gameplay 脚本。元数据来源与类型目录必须属于同一个 Host；操作 context 自动包含该来源，引用解析服务仍由领域 owner 提供。

源生成器在类型所属程序集产生私有成员访问器和构造工厂，派生类型通过基类所属目录组合继承信息。集合构造使用具体泛型调用，Core 的对象/结构体/集合 value pipeline 不执行成员反射或动态泛型构造。动态 Editor 的解析机制位于 [Inno.Adapter.Serialization.DotNet](../platform/Inno.Adapter.Serialization.DotNet.md)。

## 属性序列化

类型实现空标记接口 `ISerializable`，需要持久化的 field/property 标注 `SerializablePropertyAttribute`：

```csharp
public sealed class PlayerState : ISerializable
{
    [SerializableProperty]
    public string playerName { get; set; } = string.Empty;

    [SerializableProperty(PropertyVisibility.Readonly)]
    public int score { get; private set; }

    [SerializableProperty(PropertyVisibility.Transient)]
    public bool isSelected { get; set; }

    [OnSerializableRestored]
    private void OnRestored()
    {
        // Rebuild non-serialized derived state after the whole operation succeeds.
    }
}
```

### Attributes 与接口

| API | 说明 |
| --- | --- |
| `ISerializable` | 声明引用类型参与属性序列化。 |
| `[SerializableProperty(visibility)]` | 标注 field/property；默认 `Show`。`propertyVisibility` 暴露规则，`order` 控制同一声明类型内的处理顺序。 |
| `[OnSerializableRestored]` | 标记返回 void 的非虚实例方法，允许无参或一个 `SerializationContext` 参数，在完整 restore 成功后调用。 |
| `[RequiresSerializationConverter]` | 强制该 class 必须由显式 Converter 处理。 |
| `[GenerateSerializationConverter]` | 为闭合数据传输类型生成直接 Converter；不适用于身份图或复杂恢复不变量。 |
| `SerializationConverter` | 非泛型发现基类；具体 Converter 通过继承关系自动进入当前 generation 的 Registry。 |
| `SerializationConverter<T>` | 强类型读写与 Restore 扩展契约；不再要求重复 marker Attribute。 |

Inspector 展示标注不属于 Serialization。`Header`、`Text`、`Tooltip`、`Range`、`ShowIf` 等均由独立的 [Inno.Editor.Annotations](../editor/Inno.Editor.Annotations.md) 声明；本程序集不声明、引用或转发这些类型。`SerializableProperty` 继续只负责持久数据契约。

### PropertyVisibility

这是 `[Flags]` enum：

| 值 | Serialize | Deserialize | Runtime Get | Runtime Set |
| --- | --- | --- | --- | --- |
| `None` | 否 | 否 | 否 | 否 |
| `Show` | 是 | 是 | 是 | 是 |
| `Hide` | 是 | 是 | 否 | 否 |
| `Readonly` | 是 | 是 | 是 | 否 |
| `Transient` | 否 | 否 | 是 | 是 |
| `SerializeOnly` | 是 | 否 | 是 | 是 |
| `DeserializeOnly` | 否 | 是 | 是 | 是 |

也可直接组合底层 flags：`Serialize`、`Deserialize`、`RuntimeGet`、`RuntimeSet`。

### SerializedProperty

`name`、`propertyType`、`visibility`、`canRead`、`canWrite` 描述成员；`GetValue()` / `SetValue(object?)` 执行运行时访问，不符合 visibility 时抛 `InvalidOperationException`。

```csharp
foreach (SerializedProperty property in serialization.GetProperties(state))
{
    object? value = property.GetValue();
    if (property.canWrite)
        property.SetValue(value);
}
```

### 成员顺序

CLR 将 field 与 property 存放在不同 metadata table 中，因此 `MetadataToken` 不能表达两者在 C# 源码中的交错顺序。Inno 脚本编译器会在生成运行时代码时自动把源码声明顺序写入 `SerializablePropertyAttribute.order`，所以 GameScripts/EditorScripts 中混合声明的 field 和 property 会按脚本顺序出现在 Inspector 和序列化管线中。

普通预编译程序集可以在需要跨 field/property 固定顺序时显式声明：

```csharp
[SerializableProperty(order = 0)]
public int firstProperty { get; set; }

[SerializableProperty(order = 1)]
public int secondField;
```

继承层级仍保持 base type 在前、derived type 在后；`order` 只比较同一个 declaring type 内的成员。

### 跨 generation 的独立成员恢复

逐成员快照用于热重载和其他需要容忍 schema 演进的流程，不会改变普通 `Serialize` / `Deserialize` / `Restore` 的严格行为：

```csharp
IReadOnlyList<SerializationPropertySnapshot> snapshots =
    serialization.CaptureProperties(previous);

SerializationPropertyRestoreResult result = serialization.RestoreProperties(
    current,
    snapshots,
    SerializationPropertyRestoreMode.Compatible);
```

`SerializationPropertyRestoreMode.Strict` 在第一个匹配但不可解码的成员处抛异常。`Compatible` 使用独立的 operation checkpoint 跳过该成员，撤销它注册的 completion callback，保留目标对象构造后的默认值，并在 `failures` 中记录旧类型、新类型和错误信息。

Editor History 等需要跨 assembly generation 保留数据的系统应使用 `CapturePropertyData` / `RestorePropertiesData`。该格式包含 property key 与原声明类型信息，但不包含 owning object 引用；调用方必须用 persistent ID 在当前 generation 解析目标。格式严格检查当前 magic、长度与 trailing bytes，损坏数据不会被部分接受。

已删除成员计入 `ignoredCount`，新增成员自然保持默认值。所有兼容成员完成后，目标的 `[OnSerializableRestored]` 仍只执行一次；对象级 restore hook 失败属于严重错误，不会被 compatible 模式吞掉。

## SerializationContext

Context 是不可变的、按“精确契约类型”索引的操作依赖容器：

| 成员 | 说明 |
| --- | --- |
| `SerializationContext.empty` | 空 context。 |
| `With<TContext>(value)` | 返回包含/替换该精确类型的新 context。 |
| `TryGet<TContext>(out value)` | 尝试按精确类型取值，不按派生关系搜索。 |
| `GetRequired<TContext>()` | 缺少时抛异常。 |

```csharp
SerializationContext context = ownerContext;
byte[] bytes = serialization.Serialize(state, context);
```

## 自定义 Converter

```csharp
public sealed class RangeConverter : SerializationConverter<Range>
{
    public override void Write(
        SerializationWriter writer,
        Range value
    ) {
        writer.Write("start", value.Start.Value);
        writer.Write("end", value.End.Value);
    }

    public override Range Read(SerializationReader reader)
        => new(reader.Read<int>("start"), reader.Read<int>("end"));
}
```

`SerializationConverter<T>` 的公开扩展点：

- `Write(SerializationWriter, T)`：写一个值。
- `Read(SerializationReader)`：创建一个值。
- `Restore(SerializationReader, T target)`：可选覆盖，原位恢复已有值；默认抛 `NotSupportedException`。

Converter 应为无状态 class，并提供无参构造函数。选择优先级为精确声明（0）、类继承距离或可赋值接口（1）。变型泛型接口也按接口匹配；同一最佳距离有多个候选时明确报歧义，不按扫描顺序选择。冲突、构造失败或候选 DLL 缺依赖时，新 Registry 不会激活。

## SerializationWriter

`context`、`path`、`valueType` 提供当前操作信息。Writer 在 callback 结束后失效，不应缓存。

| 方法 | 说明 |
| --- | --- |
| `Write<TValue>(name, value)` | 经统一 value pipeline 写命名值；名称必须非空且唯一。 |
| `Write(name, object?, Type declaredType)` | 按明确声明写值；和泛型入口共用格式、Converter 与空值规则，不构造运行时泛型方法。 |
| `WriteObject(name, Action<SerializationWriter>)` | 写一个结构化子对象。 |
| `WriteObjectArray<T>(name, values, writeElement)` | 写有序结构化对象数组。 |
| `WriteProperties(ISerializable)` | 把对象上标注的成员写入当前 object。 |

## SerializationReader

Reader 同样公开 `context`、`path`、`valueType`，仅在当前 decode/restore callback 内有效。

| 方法 | 说明 |
| --- | --- |
| `Contains(name)` | 是否存在成员。 |
| `Read<TValue>(name)` | 读取必需值；缺少或类型错误时抛异常。 |
| `Read(name, Type declaredType)` | 按声明元数据读取；与泛型入口共用规则，结果可能为 null。 |
| `TryRead<TValue>(name, out value)` | 缺少成员时返回 `false`。 |
| `ReadObject(name)` | 读取结构化子对象。 |
| `ReadObjectArray(name)` | 读取结构化对象数组。 |
| `RestoreProperties(ISerializable)` | 将当前对象的属性数据恢复到既有实例。 |
| `OnCompleted(Action)` | 整个 decode 成功后调用；用于解析图引用等延迟工作。 |

## 手写 Schema 示例

```csharp
byte[] bytes = serialization.Encode(writer =>
{
    writer.WriteObjectArray("points", points, (
        item,
        point
    ) => {
        item.Write("x", point.x);
        item.Write("y", point.y);
    });
});

Vector2[] decoded = serialization.Decode(bytes, reader =>
    reader.ReadObjectArray("points")
        .Select(item => new Vector2(item.Read<float>("x"), item.Read<float>("y")))
        .ToArray());
```

## 约束

- Writer/Reader 都是 operation-scoped；在操作外使用会失败。
- 循环引用和外部对象身份通常需要 Converter + context + `OnCompleted` 协作解决。
- Restore callback 只在整个操作成功后提交完成通知；异常时不会执行 completion callbacks。
- Compatible property restore 只跳过单个成员的数据/类型不兼容；对象结构或 restore hook 错误仍由上层事务处理。
- 序列化层只实现当前二进制契约，不包含旧 schema 的兼容读取或迁移分支。不兼容数据会直接失败。
