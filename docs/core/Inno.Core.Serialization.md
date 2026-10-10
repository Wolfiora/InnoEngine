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

源生成器在类型所属程序集产生私有成员访问器和构造工厂，派生类型通过基类所属目录组合继承信息。集合构造使用具体泛型调用，Core 的对象/结构体/集合 value pipeline 不执行成员反射或动态泛型构造。动态 Editor 的解析机制位于 [Inno.Adapter.Serialization.DotNet](../backends/DotNet/Inno.Adapter.Serialization.DotNet.md)。

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

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Core.Serialization.Converters.SerializationConverter`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Serialization.Converters.SerializationConverter`](../../src/foundation/core/Inno.Core.Serialization/Converters/SerializationConverter.cs#L8) | Identifies a concrete serialization converter for generation-scoped type discovery. |

### `Inno.Core.Serialization.Converters.SerializationConverter<T>`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Serialization.Converters.SerializationConverter<T>`](../../src/foundation/core/Inno.Core.Serialization/Converters/SerializationConverter.cs#L19) | Defines the single advanced extension contract for serializing a specific value type. |
| [`abstract T Inno.Core.Serialization.Converters.SerializationConverter<T>.Read(Inno.Core.Serialization.SerializationReader reader)`](../../src/foundation/core/Inno.Core.Serialization/Converters/SerializationConverter.cs#L44) | Reads and creates a value from the current structured object. |
| [`abstract void Inno.Core.Serialization.Converters.SerializationConverter<T>.Write(Inno.Core.Serialization.SerializationWriter writer, T value)`](../../src/foundation/core/Inno.Core.Serialization/Converters/SerializationConverter.cs#L30) | Writes a value into the current structured object. |
| [`virtual void Inno.Core.Serialization.Converters.SerializationConverter<T>.Restore(Inno.Core.Serialization.SerializationReader reader, T target)`](../../src/foundation/core/Inno.Core.Serialization/Converters/SerializationConverter.cs#L58) | Restores data into an existing value. |

### `Inno.Core.Serialization.GenerateSerializationConverterAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Serialization.GenerateSerializationConverterAttribute`](../../src/foundation/core/Inno.Core.Serialization/Attributes/GenerateSerializationConverterAttribute.cs#L14) | Requests a compile-time serialization converter for a closed data-transfer type. |

### `Inno.Core.Serialization.ISerializable`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Serialization.ISerializable`](../../src/foundation/core/Inno.Core.Serialization/Contracts/ISerializable.cs#L6) | Marks a reference type whose annotated properties can participate in property-based serialization. |

### `Inno.Core.Serialization.ISerializationMetadataSource`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Serialization.ISerializationMetadataSource`](../../src/foundation/core/Inno.Core.Serialization/Metadata/ISerializationMetadataSource.cs#L8) | Supplies declaration access and construction without choosing a runtime reflection implementation. |
| [`Inno.Core.Serialization.SerializationTypeMetadata Inno.Core.Serialization.ISerializationMetadataSource.GetMetadata(System.Type type)`](../../src/foundation/core/Inno.Core.Serialization/Metadata/ISerializationMetadataSource.cs#L19) | Resolves a complete serialization shape for the current declaration. |

### `Inno.Core.Serialization.OnSerializableRestored`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Serialization.OnSerializableRestored`](../../src/foundation/core/Inno.Core.Serialization/Attributes/OnSerializableRestored.cs#L8) | Marks one nonvirtual instance method to run after restoration succeeds, optionally receiving its SerializationContext. |

### `Inno.Core.Serialization.PropertyVisibility`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Serialization.PropertyVisibility`](../../src/foundation/core/Inno.Core.Serialization/Metadata/PropertyVisibility.cs#L12) | Defines member participation rules for persistence and runtime access. |
| [`Inno.Core.Serialization.PropertyVisibility.Deserialize`](../../src/foundation/core/Inno.Core.Serialization/Metadata/PropertyVisibility.cs#L28) | Participates in deserialization (restore/read). |
| [`Inno.Core.Serialization.PropertyVisibility.DeserializeOnly`](../../src/foundation/core/Inno.Core.Serialization/Metadata/PropertyVisibility.cs#L69) | Participates in deserialization only (no serialization), runtime can get/set. |
| [`Inno.Core.Serialization.PropertyVisibility.Hide`](../../src/foundation/core/Inno.Core.Serialization/Metadata/PropertyVisibility.cs#L48) | Can serialize and deserialize, but runtime code cannot get or set through . |
| [`Inno.Core.Serialization.PropertyVisibility.None`](../../src/foundation/core/Inno.Core.Serialization/Metadata/PropertyVisibility.cs#L18) | No participation in serialization, deserialization, or runtime access. |
| [`Inno.Core.Serialization.PropertyVisibility.Readonly`](../../src/foundation/core/Inno.Core.Serialization/Metadata/PropertyVisibility.cs#L54) | Can serialize and deserialize, runtime can get but cannot set. Runtime set attempts should be rejected (see behavior). |
| [`Inno.Core.Serialization.PropertyVisibility.RuntimeGet`](../../src/foundation/core/Inno.Core.Serialization/Metadata/PropertyVisibility.cs#L33) | Allows runtime retrieval through the API. |
| [`Inno.Core.Serialization.PropertyVisibility.RuntimeSet`](../../src/foundation/core/Inno.Core.Serialization/Metadata/PropertyVisibility.cs#L38) | Allows runtime assignment through the API. |
| [`Inno.Core.Serialization.PropertyVisibility.Serialize`](../../src/foundation/core/Inno.Core.Serialization/Metadata/PropertyVisibility.cs#L23) | Participates in serialization output. |
| [`Inno.Core.Serialization.PropertyVisibility.SerializeOnly`](../../src/foundation/core/Inno.Core.Serialization/Metadata/PropertyVisibility.cs#L64) | Participates in serialization only (no deserialization), runtime can get/set. |
| [`Inno.Core.Serialization.PropertyVisibility.Show`](../../src/foundation/core/Inno.Core.Serialization/Metadata/PropertyVisibility.cs#L43) | Can serialize, deserialize, and allow runtime get/set. |
| [`Inno.Core.Serialization.PropertyVisibility.Transient`](../../src/foundation/core/Inno.Core.Serialization/Metadata/PropertyVisibility.cs#L59) | Does not participate in serialization or deserialization, but runtime can get/set. |

### `Inno.Core.Serialization.RequiresSerializationConverterAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Serialization.RequiresSerializationConverterAttribute`](../../src/foundation/core/Inno.Core.Serialization/Attributes/RequiresSerializationConverterAttribute.cs#L8) | Requires a serializable type to use an explicitly discovered converter. |

### `Inno.Core.Serialization.SerializablePropertyAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Serialization.PropertyVisibility Inno.Core.Serialization.SerializablePropertyAttribute.propertyVisibility`](../../src/foundation/core/Inno.Core.Serialization/Attributes/SerializablePropertyAttribute.cs#L25) | Gets the visibility of the annotated member. |
| [`Inno.Core.Serialization.SerializablePropertyAttribute`](../../src/foundation/core/Inno.Core.Serialization/Attributes/SerializablePropertyAttribute.cs#L11) | Declares a field or property as participating in property persistence. |
| [`Inno.Core.Serialization.SerializablePropertyAttribute.SerializablePropertyAttribute(Inno.Core.Serialization.PropertyVisibility visibility = Inno.Core.Serialization.PropertyVisibility.Show)`](../../src/foundation/core/Inno.Core.Serialization/Attributes/SerializablePropertyAttribute.cs#L33) | Initializes a new instance of the class. |
| [`int Inno.Core.Serialization.SerializablePropertyAttribute.order`](../../src/foundation/core/Inno.Core.Serialization/Attributes/SerializablePropertyAttribute.cs#L20) | Gets or sets the member order within its declaring type. |

### `Inno.Core.Serialization.SerializationCollectionMetadata`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Serialization.SerializationCollectionMetadata`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationCollectionMetadata.cs#L9) | Supplies typed collection construction and map enumeration without runtime generic compilation. |
| [`Inno.Core.Serialization.SerializationCollectionMetadata.SerializationCollectionMetadata(System.Type elementType, System.Type? keyType, System.Func<System.Collections.Generic.IReadOnlyList<object?>, object>? buildSequence, System.Func<System.Collections.Generic.IReadOnlyList<System.Collections.Generic.KeyValuePair<object?, object?>>, object>? buildMap, System.Func<object, System.Collections.Generic.IReadOnlyList<System.Collections.Generic.KeyValuePair<object?, object?>>>? enumerateMap)`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationCollectionMetadata.cs#L32) | Defines the shape and typed operations of a sequence or map. |
| [`System.Func<System.Collections.Generic.IReadOnlyList<System.Collections.Generic.KeyValuePair<object?, object?>>, object>? Inno.Core.Serialization.SerializationCollectionMetadata.buildMap`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationCollectionMetadata.cs#L68) | Gets the typed map constructor, or null for a sequence. |
| [`System.Func<System.Collections.Generic.IReadOnlyList<object?>, object>? Inno.Core.Serialization.SerializationCollectionMetadata.buildSequence`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationCollectionMetadata.cs#L63) | Gets the typed sequence constructor, or null for a map. |
| [`System.Func<object, System.Collections.Generic.IReadOnlyList<System.Collections.Generic.KeyValuePair<object?, object?>>>? Inno.Core.Serialization.SerializationCollectionMetadata.enumerateMap`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationCollectionMetadata.cs#L73) | Gets typed map enumeration, or null for a sequence. |
| [`System.Type Inno.Core.Serialization.SerializationCollectionMetadata.elementType`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationCollectionMetadata.cs#L53) | Gets the sequence element or map value type. |
| [`System.Type? Inno.Core.Serialization.SerializationCollectionMetadata.keyType`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationCollectionMetadata.cs#L58) | Gets the map key type, or null for a sequence. |

### `Inno.Core.Serialization.SerializationContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Serialization.SerializationContext`](../../src/foundation/core/Inno.Core.Serialization/Context/SerializationContext.cs#L9) | Provides immutable, operation-independent context to serialization converters. |
| [`Inno.Core.Serialization.SerializationContext Inno.Core.Serialization.SerializationContext.With<TContext>(TContext value)`](../../src/foundation/core/Inno.Core.Serialization/Context/SerializationContext.cs#L38) | Returns a new context containing the supplied value under its declared context type. |
| [`TContext Inno.Core.Serialization.SerializationContext.GetRequired<TContext>()`](../../src/foundation/core/Inno.Core.Serialization/Context/SerializationContext.cs#L84) | Resolves a value registered under the exact context contract type. |
| [`bool Inno.Core.Serialization.SerializationContext.TryGet<TContext>(out TContext? value)`](../../src/foundation/core/Inno.Core.Serialization/Context/SerializationContext.cs#L60) | Attempts to resolve a value registered under the exact context contract type. |
| [`static Inno.Core.Serialization.SerializationContext Inno.Core.Serialization.SerializationContext.empty`](../../src/foundation/core/Inno.Core.Serialization/Context/SerializationContext.cs#L21) | Gets an empty serialization context. |

### `Inno.Core.Serialization.SerializationGeneration`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Serialization.SerializationGeneration`](../../src/foundation/core/Inno.Core.Serialization/SerializationGeneration.cs#L14) | Pins one immutable converter generation so serialization remains deterministic across asynchronous continuations and extension reloads. |
| [`T Inno.Core.Serialization.SerializationGeneration.Deserialize<T>(System.ReadOnlySpan<byte> bytes, Inno.Core.Serialization.SerializationContext? context = null)`](../../src/foundation/core/Inno.Core.Serialization/SerializationGeneration.cs#L73) | Deserializes a complete root object using the converter generation captured by this instance. |
| [`TResult Inno.Core.Serialization.SerializationGeneration.Decode<TResult>(System.ReadOnlySpan<byte> bytes, System.Func<Inno.Core.Serialization.SerializationReader, TResult> read, Inno.Core.Serialization.SerializationContext? context = null)`](../../src/foundation/core/Inno.Core.Serialization/SerializationGeneration.cs#L128) | Decodes a manually defined structured root using the converter generation captured by this instance. |
| [`byte[] Inno.Core.Serialization.SerializationGeneration.Encode(System.Action<Inno.Core.Serialization.SerializationWriter> write, Inno.Core.Serialization.SerializationContext? context = null)`](../../src/foundation/core/Inno.Core.Serialization/SerializationGeneration.cs#L98) | Encodes a manually defined structured root using the converter generation captured by this instance. |
| [`byte[] Inno.Core.Serialization.SerializationGeneration.Serialize<T>(T value, Inno.Core.Serialization.SerializationContext? context = null)`](../../src/foundation/core/Inno.Core.Serialization/SerializationGeneration.cs#L48) | Serializes a complete root object using the converter generation captured by this instance. |
| [`void Inno.Core.Serialization.SerializationGeneration.Dispose()`](../../src/foundation/core/Inno.Core.Serialization/SerializationGeneration.cs#L138) | Releases the pinned converter generation and every collectible type reference owned by this lease. |

### `Inno.Core.Serialization.SerializationMemberMetadata`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Serialization.PropertyVisibility Inno.Core.Serialization.SerializationMemberMetadata.visibility`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationMemberMetadata.cs#L67) | Gets the member's persistent and runtime access permissions. |
| [`Inno.Core.Serialization.SerializationMemberMetadata`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationMemberMetadata.cs#L8) | Captures one ordered serialization key and its generation-owned accessors. |
| [`Inno.Core.Serialization.SerializationMemberMetadata.SerializationMemberMetadata(string name, System.Type type, Inno.Core.Serialization.PropertyVisibility visibility, System.Func<object, object?>? getter, System.Action<object, object?>? setter)`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationMemberMetadata.cs#L34) | Defines a member whose access is implemented by the selected metadata provider. |
| [`System.Type Inno.Core.Serialization.SerializationMemberMetadata.type`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationMemberMetadata.cs#L62) | Gets the declared value type. |
| [`object? Inno.Core.Serialization.SerializationMemberMetadata.GetValue(object target)`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationMemberMetadata.cs#L81) | Reads the current value through the selected declaration accessor. |
| [`string Inno.Core.Serialization.SerializationMemberMetadata.name`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationMemberMetadata.cs#L57) | Gets the exact persistent key. |
| [`void Inno.Core.Serialization.SerializationMemberMetadata.SetValue(object target, object? value)`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationMemberMetadata.cs#L97) | Writes a value through the selected declaration accessor. |

### `Inno.Core.Serialization.SerializationPropertyRestoreFailure`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Serialization.SerializationPropertyRestoreFailure`](../../src/foundation/core/Inno.Core.Serialization/PropertyRestoration/SerializationPropertyRestoreFailure.cs#L8) | Describes one property skipped during a compatible property restore. |
| [`System.Type Inno.Core.Serialization.SerializationPropertyRestoreFailure.currentPropertyType`](../../src/foundation/core/Inno.Core.Serialization/PropertyRestoration/SerializationPropertyRestoreFailure.cs#L35) | Gets the current target member type. |
| [`System.Type Inno.Core.Serialization.SerializationPropertyRestoreFailure.previousPropertyType`](../../src/foundation/core/Inno.Core.Serialization/PropertyRestoration/SerializationPropertyRestoreFailure.cs#L30) | Gets the type used to capture the previous value. |
| [`string Inno.Core.Serialization.SerializationPropertyRestoreFailure.message`](../../src/foundation/core/Inno.Core.Serialization/PropertyRestoration/SerializationPropertyRestoreFailure.cs#L40) | Gets the underlying compatibility failure message. |
| [`string Inno.Core.Serialization.SerializationPropertyRestoreFailure.name`](../../src/foundation/core/Inno.Core.Serialization/PropertyRestoration/SerializationPropertyRestoreFailure.cs#L25) | Gets the serialized member key. |

### `Inno.Core.Serialization.SerializationPropertyRestoreMode`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Serialization.SerializationPropertyRestoreMode`](../../src/foundation/core/Inno.Core.Serialization/PropertyRestoration/SerializationPropertyRestoreMode.cs#L6) | Defines how independently captured serialized properties handle restore failures. |
| [`Inno.Core.Serialization.SerializationPropertyRestoreMode.CollectFailures`](../../src/foundation/core/Inno.Core.Serialization/PropertyRestoration/SerializationPropertyRestoreMode.cs#L16) | Continues restoring independent properties and reports every rejected value. |
| [`Inno.Core.Serialization.SerializationPropertyRestoreMode.Strict`](../../src/foundation/core/Inno.Core.Serialization/PropertyRestoration/SerializationPropertyRestoreMode.cs#L11) | Stops at the first property that cannot be restored into the current object. |

### `Inno.Core.Serialization.SerializationPropertyRestoreResult`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Serialization.SerializationPropertyRestoreResult`](../../src/foundation/core/Inno.Core.Serialization/PropertyRestoration/SerializationPropertyRestoreResult.cs#L8) | Summarizes an independently captured property restore operation. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Core.Serialization.SerializationPropertyRestoreFailure> Inno.Core.Serialization.SerializationPropertyRestoreResult.failures`](../../src/foundation/core/Inno.Core.Serialization/PropertyRestoration/SerializationPropertyRestoreResult.cs#L33) | Gets properties skipped because their previous data was incompatible. |
| [`bool Inno.Core.Serialization.SerializationPropertyRestoreResult.success`](../../src/foundation/core/Inno.Core.Serialization/PropertyRestoration/SerializationPropertyRestoreResult.cs#L38) | Gets whether every matching property was restored successfully. |
| [`int Inno.Core.Serialization.SerializationPropertyRestoreResult.ignoredCount`](../../src/foundation/core/Inno.Core.Serialization/PropertyRestoration/SerializationPropertyRestoreResult.cs#L28) | Gets the number of removed or non-deserializable properties ignored by schema matching. |
| [`int Inno.Core.Serialization.SerializationPropertyRestoreResult.restoredCount`](../../src/foundation/core/Inno.Core.Serialization/PropertyRestoration/SerializationPropertyRestoreResult.cs#L23) | Gets the number of properties successfully restored. |

### `Inno.Core.Serialization.SerializationPropertySnapshot`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Serialization.SerializationPropertySnapshot`](../../src/foundation/core/Inno.Core.Serialization/PropertyRestoration/SerializationPropertySnapshot.cs#L8) | Stores one independently encoded serializable property and its original declared type. |
| [`System.ReadOnlyMemory<byte> Inno.Core.Serialization.SerializationPropertySnapshot.data`](../../src/foundation/core/Inno.Core.Serialization/PropertyRestoration/SerializationPropertySnapshot.cs#L35) | Gets the independently encoded property data. |
| [`System.Type Inno.Core.Serialization.SerializationPropertySnapshot.propertyType`](../../src/foundation/core/Inno.Core.Serialization/PropertyRestoration/SerializationPropertySnapshot.cs#L30) | Gets the declared property type used when the value was captured. |
| [`string Inno.Core.Serialization.SerializationPropertySnapshot.name`](../../src/foundation/core/Inno.Core.Serialization/PropertyRestoration/SerializationPropertySnapshot.cs#L25) | Gets the serialized member key. |

### `Inno.Core.Serialization.SerializationReader`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Serialization.SerializationContext Inno.Core.Serialization.SerializationReader.context`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationReader.cs#L29) | Gets the immutable context supplied to the current operation. |
| [`Inno.Core.Serialization.SerializationReader`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationReader.cs#L9) | Reads a structured object during the active deserialization operation. |
| [`Inno.Core.Serialization.SerializationReader Inno.Core.Serialization.SerializationReader.ReadObject(string name)`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationReader.cs#L152) | Reads a required named structured object. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Core.Serialization.SerializationReader> Inno.Core.Serialization.SerializationReader.ReadObjectArray(string name)`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationReader.cs#L169) | Reads a required ordered array of structured objects. |
| [`System.Type Inno.Core.Serialization.SerializationReader.valueType`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationReader.cs#L39) | Gets the declared value type represented by this reader. |
| [`TValue Inno.Core.Serialization.SerializationReader.Read<TValue>(string name)`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationReader.cs#L71) | Reads a required named value through the unified value pipeline. |
| [`bool Inno.Core.Serialization.SerializationReader.Contains(string name)`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationReader.cs#L50) | Determines whether the current object contains a named member. |
| [`bool Inno.Core.Serialization.SerializationReader.TryRead<TValue>(string name, out TValue value)`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationReader.cs#L123) | Attempts to read a named value through the unified value pipeline. |
| [`object? Inno.Core.Serialization.SerializationReader.Read(string name, System.Type declaredType)`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationReader.cs#L94) | Reads a value using its declared metadata type without constructing a generic method at runtime. |
| [`string Inno.Core.Serialization.SerializationReader.path`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationReader.cs#L34) | Gets the current diagnostic path. |
| [`void Inno.Core.Serialization.SerializationReader.OnCompleted(System.Action callback)`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationReader.cs#L211) | Schedules a callback to run after the complete decode operation succeeds. |
| [`void Inno.Core.Serialization.SerializationReader.RestoreProperties(Inno.Core.Serialization.ISerializable target)`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationReader.cs#L195) | Restores annotated properties from the current structured object into an existing object. |

### `Inno.Core.Serialization.SerializationRegistry`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Serialization.SerializationGeneration Inno.Core.Serialization.SerializationRegistry.CaptureGeneration()`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L64) | Captures the currently active converter generation for deterministic work that may cross asynchronous continuations. |
| [`Inno.Core.Serialization.SerializationPropertyRestoreResult Inno.Core.Serialization.SerializationRegistry.RestoreProperties(Inno.Core.Serialization.ISerializable target, System.Collections.Generic.IReadOnlyList<Inno.Core.Serialization.SerializationPropertySnapshot> snapshots, Inno.Core.Serialization.SerializationPropertyRestoreMode mode = Inno.Core.Serialization.SerializationPropertyRestoreMode.Strict, Inno.Core.Serialization.SerializationContext? context = null)`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L272) | Restores independently captured properties into an existing object. |
| [`Inno.Core.Serialization.SerializationPropertyRestoreResult Inno.Core.Serialization.SerializationRegistry.RestorePropertiesData(Inno.Core.Serialization.ISerializable target, System.ReadOnlySpan<byte> data, Inno.Core.Serialization.SerializationPropertyRestoreMode mode = Inno.Core.Serialization.SerializationPropertyRestoreMode.Strict, Inno.Core.Serialization.SerializationContext? context = null)`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L323) | Restores one or more independently encoded persistent properties from neutral bytes. |
| [`Inno.Core.Serialization.SerializationRegistry`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L12) | Owns one generation-aware converter registry and provides deterministic root serialization operations. |
| [`Inno.Core.Serialization.SerializationRegistry.SerializationRegistry(Inno.Extensibility.Types.TypeCatalog types, Inno.Core.Serialization.ISerializationMetadataSource metadata)`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L31) | Creates a serialization registry derived from one isolated type catalog. |
| [`Inno.Core.Serialization.SerializationTypeMetadata Inno.Core.Serialization.SerializationRegistry.GetMetadata(System.Type type)`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L87) | Reads the current provider's declaration metadata for domain operations such as prefab overrides. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Core.Serialization.SerializationPropertySnapshot> Inno.Core.Serialization.SerializationRegistry.CaptureProperties(Inno.Core.Serialization.ISerializable value, Inno.Core.Serialization.SerializationContext? context = null)`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L145) | Captures each persistent property independently for reload-safe state restoration. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Core.Serialization.SerializedProperty> Inno.Core.Serialization.SerializationRegistry.GetProperties(Inno.Core.Serialization.ISerializable value)`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L110) | Gets the stable ordered runtime-visible properties for a serializable object. |
| [`T Inno.Core.Serialization.SerializationRegistry.Deserialize<T>(System.ReadOnlySpan<byte> bytes, Inno.Core.Serialization.SerializationContext? context = null)`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L415) | Deserializes a new serializable root object from deterministic binary data. |
| [`TResult Inno.Core.Serialization.SerializationRegistry.Decode<TResult>(System.ReadOnlySpan<byte> bytes, System.Func<Inno.Core.Serialization.SerializationReader, TResult> read, Inno.Core.Serialization.SerializationContext? context = null)`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L566) | Decodes a manually defined structured schema from deterministic binary data. |
| [`byte[] Inno.Core.Serialization.SerializationRegistry.CapturePropertiesData(Inno.Core.Serialization.ISerializable value, Inno.Core.Serialization.SerializationContext? context = null)`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L214) | Captures every persistent property as neutral bytes without serializing the owning object graph. |
| [`byte[] Inno.Core.Serialization.SerializationRegistry.CapturePropertyData(Inno.Core.Serialization.ISerializable value, string propertyName, Inno.Core.Serialization.SerializationContext? context = null)`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L180) | Captures one persistent property as independently restorable neutral bytes. |
| [`byte[] Inno.Core.Serialization.SerializationRegistry.Encode(System.Action<Inno.Core.Serialization.SerializationWriter> write, Inno.Core.Serialization.SerializationContext? context = null)`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L512) | Encodes a manually defined structured schema into deterministic binary data. |
| [`byte[] Inno.Core.Serialization.SerializationRegistry.EncodePropertySnapshots(System.Collections.Generic.IReadOnlyList<Inno.Core.Serialization.SerializationPropertySnapshot> snapshots)`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L237) | Encodes an existing ordered set of independent property snapshots as neutral restoration bytes. |
| [`byte[] Inno.Core.Serialization.SerializationRegistry.Serialize<T>(T value, Inno.Core.Serialization.SerializationContext? context = null)`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L364) | Serializes a complete serializable root object into deterministic binary data. |
| [`void Inno.Core.Serialization.SerializationRegistry.Dispose()`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L45) | Releases every converter generation owned by this registry. |
| [`void Inno.Core.Serialization.SerializationRegistry.Restore<T>(T target, System.ReadOnlySpan<byte> bytes, Inno.Core.Serialization.SerializationContext? context = null)`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L470) | Restores deterministic binary data into an existing serializable root object. |

### `Inno.Core.Serialization.SerializationTypeMetadata`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Serialization.SerializationCollectionMetadata? Inno.Core.Serialization.SerializationTypeMetadata.collection`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationTypeMetadata.cs#L91) | Gets collection access metadata, or null for a scalar or object declaration. |
| [`Inno.Core.Serialization.SerializationTypeMetadata`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationTypeMetadata.cs#L10) | Freezes member access, construction and restoration callbacks for an exact closed declaration. |
| [`Inno.Core.Serialization.SerializationTypeMetadata.SerializationTypeMetadata(System.Type type, System.Collections.Generic.IReadOnlyList<Inno.Core.Serialization.SerializationMemberMetadata> members, System.Func<object>? factory, System.Action<object, Inno.Core.Serialization.SerializationContext>? restored = null, Inno.Core.Serialization.SerializationCollectionMetadata? collection = null, bool requiresConverter = false, Inno.Core.Serialization.SerializationTypeMetadata? inherited = null)`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationTypeMetadata.cs#L41) | Copies complete declaration metadata before it participates in a serialization generation. |
| [`System.Action<object, Inno.Core.Serialization.SerializationContext>? Inno.Core.Serialization.SerializationTypeMetadata.restored`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationTypeMetadata.cs#L86) | Gets the generation-owned restoration callback, or null when no hooks are declared. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Core.Serialization.SerializationMemberMetadata> Inno.Core.Serialization.SerializationTypeMetadata.members`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationTypeMetadata.cs#L81) | Gets ordered immutable member declarations. |
| [`System.Type Inno.Core.Serialization.SerializationTypeMetadata.type`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationTypeMetadata.cs#L76) | Gets the exact closed declaration. |
| [`bool Inno.Core.Serialization.SerializationTypeMetadata.requiresConverter`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationTypeMetadata.cs#L96) | Gets whether default object serialization requires an explicit converter. |
| [`object Inno.Core.Serialization.SerializationTypeMetadata.CreateInstance()`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationTypeMetadata.cs#L107) | Creates a new instance through the declaration's selected constructor. |

### `Inno.Core.Serialization.SerializationWriter`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Serialization.SerializationContext Inno.Core.Serialization.SerializationWriter.context`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationWriter.cs#L29) | Gets the immutable context supplied to the current operation. |
| [`Inno.Core.Serialization.SerializationWriter`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationWriter.cs#L9) | Writes a structured object during the active serialization operation. |
| [`System.Type Inno.Core.Serialization.SerializationWriter.valueType`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationWriter.cs#L39) | Gets the declared value type represented by this writer. |
| [`string Inno.Core.Serialization.SerializationWriter.path`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationWriter.cs#L34) | Gets the current diagnostic path. |
| [`void Inno.Core.Serialization.SerializationWriter.Write(string name, object? value, System.Type declaredType)`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationWriter.cs#L85) | Writes a value using its declared metadata type without constructing a generic method at runtime. |
| [`void Inno.Core.Serialization.SerializationWriter.Write<TValue>(string name, TValue value)`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationWriter.cs#L59) | Writes a named value through the unified value pipeline. |
| [`void Inno.Core.Serialization.SerializationWriter.WriteObject(string name, System.Action<Inno.Core.Serialization.SerializationWriter> write)`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationWriter.cs#L107) | Writes a named structured object. |
| [`void Inno.Core.Serialization.SerializationWriter.WriteObjectArray<TValue>(string name, System.Collections.Generic.IEnumerable<TValue> values, System.Action<Inno.Core.Serialization.SerializationWriter, TValue> writeElement)`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationWriter.cs#L136) | Writes an ordered array of structured objects. |
| [`void Inno.Core.Serialization.SerializationWriter.WriteProperties(Inno.Core.Serialization.ISerializable value)`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationWriter.cs#L168) | Writes annotated properties from a serializable object into the current structured object. |

### `Inno.Core.Serialization.SerializedIdentityRemapper`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Serialization.SerializedIdentityRemapper`](../../src/foundation/core/Inno.Core.Serialization/SerializedIdentityRemapper.cs#L11) | Rewrites selected scalar identities in a serialized value and its nested serialized payloads. |
| [`static byte[] Inno.Core.Serialization.SerializedIdentityRemapper.Rewrite(System.ReadOnlySpan<byte> source, System.Collections.Generic.IReadOnlyDictionary<System.Guid, System.Guid> identities, System.Collections.Generic.IReadOnlyDictionary<string, string>? paths = null)`](../../src/foundation/core/Inno.Core.Serialization/SerializedIdentityRemapper.cs#L34) | Replaces exact Guid and string values while retaining the binary serialization structure. |

### `Inno.Core.Serialization.SerializedProperty`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Serialization.PropertyVisibility Inno.Core.Serialization.SerializedProperty.visibility`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializedProperty.cs#L32) | Gets the visibility of this property. |
| [`Inno.Core.Serialization.SerializedProperty`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializedProperty.cs#L8) | Represents a discoverable property in editor and persistence workflows. |
| [`System.Type Inno.Core.Serialization.SerializedProperty.propertyType`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializedProperty.cs#L27) | Gets the declared CLR type of this property. |
| [`bool Inno.Core.Serialization.SerializedProperty.canRead`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializedProperty.cs#L37) | Gets whether runtime callers may read this property. |
| [`bool Inno.Core.Serialization.SerializedProperty.canWrite`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializedProperty.cs#L42) | Gets whether runtime callers may write this property. |
| [`object? Inno.Core.Serialization.SerializedProperty.GetValue()`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializedProperty.cs#L79) | Gets the current value. |
| [`string Inno.Core.Serialization.SerializedProperty.name`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializedProperty.cs#L22) | Gets the display and serialization key name. |
| [`void Inno.Core.Serialization.SerializedProperty.SetValue(object? value)`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializedProperty.cs#L95) | Sets the current value. |

### `Inno.Core.Serialization.StaticSerializationMetadataSource`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Serialization.SerializationTypeMetadata Inno.Core.Serialization.StaticSerializationMetadataSource.GetMetadata(System.Type type)`](../../src/foundation/core/Inno.Core.Serialization/Metadata/StaticSerializationMetadataSource.cs#L33) | See the implemented contract. |
| [`Inno.Core.Serialization.StaticSerializationMetadataSource`](../../src/foundation/core/Inno.Core.Serialization/Metadata/StaticSerializationMetadataSource.cs#L9) | Resolves exact serialization shapes from explicit generated contributions without reflection fallback. |
| [`Inno.Core.Serialization.StaticSerializationMetadataSource.StaticSerializationMetadataSource(System.Collections.Generic.IReadOnlyList<System.Action<System.Action<Inno.Core.Serialization.SerializationTypeMetadata>>> catalogs)`](../../src/foundation/core/Inno.Core.Serialization/Metadata/StaticSerializationMetadataSource.cs#L22) | Freezes generated declaration contributions supplied by the Player composition. |

## 项目依赖

- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
