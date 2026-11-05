# Inno.Core.Serialization

[上一页：Identity](Inno.Core.Identity.md) · [Core 索引](README.md) · [Extensibility](../extensibility/Inno.Extensibility.Types.md)

`Inno.Core.Serialization` 提供确定性的二进制对象图格式、基于 attribute 的属性持久化，以及通过 TypeRegistry 自动发现的 Converter。它既支持“序列化完整 `ISerializable` 根对象”，也支持用 Writer/Reader 显式定义 schema。

### 完整状态捕获与复用

Host 可以使用 `CaptureSnapshot(value, context, previous)` 捕获不可变 `SerializationSnapshot`。默认完整执行当前 generation 的 getter 和 Converter；未变化的中立节点以 copy-on-write 复用。Converter 可显式声明完整捕获 key，证明未变化时复用已有节点；普通 `Serialize` 从不查询 key。`ToArray()` 使用同一二进制编码器返回独立 bytes，修改返回数组不会修改捕获结果。快照只持有中立标量、字符串和复制的二进制数据，不持有源对象、`Type`、Converter、delegate 或 generation lease，可在原 Registry 退休后编码。

该入口不导出到脚本。快照是 owner 所属的瞬时观察值，不是第二种文件格式，也不进入 History、Workspace 或资源元数据。比较失败时不发布新快照，旧快照保持有效。快照捕获期间，Converter 借用的 Writer 只属于其同步回调；不要在回调返回后继续写入。

内部 Writer 对同序、同类型且 converter 选择不变的标量复用一次位置查询，不重复查找相同节点；重复 member 仍明确失败。对象 keys 的现有二进制规范顺序保持不变，因此 Converter 调整写入次序不会改变规范 bytes，数组顺序仍属于数据。所有优化共用当前编码器与 generation lease，不缓存业务对象或绕过自定义 Converter。

`SerializationWriter.WritePropertiesData(name, value, context)` 将同一 generation 的独立属性快照写为现有 binary member，供 Scene 等需要保留独立恢复边界的 Converter 使用。它与 `CapturePropertiesData` 的 bytes 一致，并在重复捕获时复用未变化的属性节点和编码结果；传入的 context 必须保留 owner 的引用解析能力。普通序列化、保存与 Missing 恢复继续使用同一 pipeline，不建立领域专用 serializer。

### 显式完整捕获 key

`SerializationConverter<T>.TryCaptureKey(value, key)` 是可选 protected 扩展点，默认返回 false。只有当前注册的 Core Identity 根对象参与。回调每次捕获都会执行，必须纯读取并覆盖全部输出、上下文依赖与顺序；不能只报告 UI 编辑通知。Core 自动限定当前 runtime identity/domain、metadata 声明和 Converter generation。回调返回 true 且 key 完全相同时才省去 `Write`；变化时执行原 writer，随后再次验证 key 与所有借用 identity，失败不发布新快照。

| API | 责任与限制 |
| --- | --- |
| `SerializationCaptureKeyBuilder.context` | 当前同步回调的完整 owner context；回调结束后失效。 |
| `EnsureCapacity(int)` | 按已冻结输入的实际条目预留容量，不改变 key。 |
| `TryAddOwner(IdentityObject, ulong?)` | 通过现有 Core Identity 记录 owner 与完整 revision；null、失效绑定或任何失败使整个 key 不可复用。 |
| `TryAddProperties(ISerializable)` | 仅接受 metadata 显式证明的完整属性 revision，以及权威 Converter resolver 已确认没有自定义 Converter 的纯值成员；不在查询时构造 Converter。 |
| `AddValue(ulong)` / `AddValue(Guid)` / `AddText(string?)` | 追加精确值、完整标识符和有序文本，区分 null 与空串；不使用有碰撞风险的标识符 hash。 |
| `SerializationRevisionAttribute(string memberName)` / `memberName` | 选择 sealed serializable identity 对象所属的非持久化 ulong/nullable ulong 字段或属性；每次相关写入前递增，不能循环复用计数。 |
| `SerializationTypeMetadata.WithCaptureRevision(Func<object, ulong?>)` | Metadata provider 显式声明上述完整 revision 读取器；null 结果拒绝复用。 |
| `SerializationTypeMetadata.WithPureFieldAccess()` | Provider 证明值类型的持久成员仅为直接读取的 primitive 字段；任意 getter 不具备该资格。 |

这些公开入口是 Converter 与 metadata provider 组合所需的最小能力。Builder 和 attribute 导出为逻辑脚本契约；metadata provider 的声明能力仍只由 Host/编译器 owner 使用。不得把 builder、metadata、访问委托或类型长期存入插件状态。Builder 只属于同步 owner 线程，跨线程、退休后访问明确失败；即使扩展错误地保留 builder，其退休也会清空 context、identity 检查与临时存储。

不完整 revision、计数耗尽、可变引用、Missing、任意 getter、stateful Converter 和未证明的 metadata 继续执行原 capture。新返回的等价 metadata 可以正常捕获，但不会获得跨声明复用。Key 不进入二进制数据、History、Workspace 或 Asset metadata，不是第二套序列化或对象管理系统。静态 metadata provider 可以声明同样能力；未声明时保持完整捕获，不要求 Player 使用动态反射来源。

`SerializationWriter.WriteObjectArray(name, values, writeElement, captureKey)` 将同一证明用于结构化数组元素。可选回调必须声明当前 Core identity owner，以及完整数据、顺序、布局和上下文输入。普通序列化逐次执行原 `writeElement`；Snapshot 只复用已经完整证明的相同元素，变化元素仍使用相同 writer。回调抛错、identity 失效或写入中输入变化均拒绝发布。未证明的元素继续完整捕获；初次获得 hint 不能单独改变数据未变化的既有节点。

Core 跟踪当前同步写入范围中实际执行的 Converter，并从独立属性 payload 向外传播中立标记。嵌套 Converter 的状态不能由父元素的局部 revision 隐式证明，因此其执行会阻止父节点发布可复用 key；既有 Converter 本身仍遵守自己的捕获契约。标记不编码、不持有类型或回调，也不是另一个依赖解析或缓存服务。

已证明的不可变对象节点可按需保存最多 64 KiB 的规范编码块。编码和流式写入复用同一编码器；只保留中立 bytes，并以并发安全方式发布完整块。更大的节点仍走原流式编码，不分配完整输出缓冲。未变化的对象可重复写出已有块，变化对象重新编码；没有第二套编码格式或全局块缓存。`ToArray()` 仍返回独立 caller-owned bytes，目标 Stream 失败不会破坏快照。Guid 写入直接使用借用 Span，规范字节顺序保持一致。

Scene 的实际消费者在 Scene owner 中声明每个对象的元数据计数、精确 source/parent ID、sibling 位置及全部 Component 属性证明。结构、对象顺序、根属性、别名和 System 仍由现有 writer 处理；单个对象改变不强迫其他已证明对象重新编码。普通插件字段、自定义 Converter、Missing 和 Prefab 不会因这一优化失去变化检测。源状态必须在 owner 同步操作内稳定；此能力不是并发读写锁。

## 初始化

Shader 等 Editor 创作回调可以借用宿主的 `SerializationRegistry`，脚本侧只导出数据编解码及属性读取。
构造 Registry、捕获 generation 和底层属性恢复快照仍由宿主管理，不进入脚本 API。运行时脚本不导出 Registry。

`SerializationRegistry` 是实例服务，必须在 `ModuleHost` 与 `TypeCatalog` 之后创建。构造时显式注入 `ISerializationMetadataSource`：Editor 使用独立 DotNet Adapter，Player 使用源生成器产生的静态目录。Converter Registry 跟随 TypeCache 的候选事务刷新。

```csharp
using Inno.Core.Serialization;
using Inno.Extensibility.Types;

static SerializationRegistry CreateOwnedRegistry(
    TypeCatalog types,
    ISerializationMetadataSource serializationMetadataSource
) {
    return new SerializationRegistry(types, serializationMetadataSource);
}
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

源生成器在类型所属程序集产生私有成员访问器和构造工厂，派生类型通过基类所属目录组合继承信息。集合构造使用具体泛型调用，Core 的对象/结构体/集合 value pipeline 不执行成员反射或动态泛型构造。动态 Editor 的解析机制位于 [Inno.Adapter.Serialization.DotNet](../../backends/DotNet/Inno.Adapter.Serialization.DotNet.md)。

## 属性序列化

类型实现空标记接口 `ISerializable`，需要持久化的 field/property 标注 `SerializablePropertyAttribute`：

```csharp
using Inno.Core.Serialization;

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
using Inno.Core.Serialization;

static void ReassignWritableProperties(
    SerializationRegistry serialization,
    ISerializable state
) {
    foreach (SerializedProperty property in serialization.GetProperties(state))
    {
        object? value = property.GetValue();
        if (property.canWrite)
            property.SetValue(value);
    }
}
```

### 成员顺序

CLR 将 field 与 property 存放在不同 metadata table 中，因此 `MetadataToken` 不能表达两者在 C# 源码中的交错顺序。Inno 脚本编译器会在生成运行时代码时自动把源码声明顺序写入 `SerializablePropertyAttribute.order`，所以 GameScripts/EditorScripts 中混合声明的 field 和 property 会按脚本顺序出现在 Inspector 和序列化管线中。

普通预编译程序集可以在需要跨 field/property 固定顺序时显式声明：

```csharp
using Inno.Core.Serialization;

public sealed class OrderedState : ISerializable
{
    [SerializableProperty(order = 0)]
    public int firstProperty { get; set; }

    [SerializableProperty(order = 1)]
    public int secondField;
}
```

继承层级仍保持 base type 在前、derived type 在后；`order` 只比较同一个 declaring type 内的成员。

### 跨 generation 的独立成员恢复

逐成员快照用于当前会话中的扩展重载、成员暂时 Missing 及恢复；普通 `Serialize` / `Deserialize` / `Restore` 仍保持严格的当前格式：

```csharp
using System.Collections.Generic;
using Inno.Core.Serialization;

static SerializationPropertyRestoreResult RestoreMatchingProperties(
    SerializationRegistry serialization,
    ISerializable previous,
    ISerializable current,
    SerializationContext ownerContext
) {
    IReadOnlyList<SerializationPropertySnapshot> snapshots =
        serialization.CaptureProperties(previous, ownerContext);
    return serialization.RestoreProperties(current, snapshots,
        SerializationPropertyRestoreMode.CollectFailures, ownerContext);
}
```

`SerializationPropertyRestoreMode.Strict` 在第一个匹配但不可解码的成员处抛异常。`Compatible` 使用独立的 operation checkpoint 跳过该成员，撤销它注册的 completion callback，保留目标对象构造后的默认值，并在 `failures` 中记录旧类型、新类型和错误信息。

Editor History 等需要跨 assembly generation 保留数据的系统应使用 `CapturePropertyData` / `RestorePropertiesData`。该格式保存 property key 与独立编码的值节点，不保存 CLR Type 或 owning object；恢复时由当前目标的 metadata 决定声明类型。内存中的 `SerializationPropertySnapshot.propertyType` 只属于其创建代际，不能据此长期持有 collectible 类型。调用方必须用 persistent ID 在当前 generation 解析目标。格式严格检查当前 magic、长度与 trailing bytes，损坏数据不会被部分接受。

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
using Inno.Core.Serialization;

static byte[] SerializeState<T>(
    SerializationRegistry serialization,
    T state,
    SerializationContext ownerContext
) where T : class, ISerializable {
    return serialization.Serialize(state, ownerContext);
}
```

## 自定义 Converter

```csharp
using System;
using Inno.Core.Serialization;
using Inno.Core.Serialization.Converters;

public sealed class RangeConverter : SerializationConverter<Range>
{
    public override void Write(
        SerializationWriter writer,
        Range value
    ) {
        writer.Write("start", value.Start.Value);
        writer.Write("startFromEnd", value.Start.IsFromEnd);
        writer.Write("end", value.End.Value);
        writer.Write("endFromEnd", value.End.IsFromEnd);
    }

    public override Range Read(SerializationReader reader)
    {
        Index start = new(reader.Read<int>("start"), reader.Read<bool>("startFromEnd"));
        Index end = new(reader.Read<int>("end"), reader.Read<bool>("endFromEnd"));
        return new Range(start, end);
    }
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
| `WriteObjectArray<T>(name, values, writeElement, captureKey = null)` | 写有序结构化对象数组。 |
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
using System.Linq;
using Inno.Core.Serialization;

static (float x, float y)[] RoundTripPoints(
    SerializationRegistry serialization,
    (float x, float y)[] points
) {
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
    return serialization.Decode(bytes, reader => reader.ReadObjectArray("points")
        .Select(item => (item.Read<float>("x"), item.Read<float>("y")))
        .ToArray());
}
```

## 约束

- Writer/Reader 都是 operation-scoped；在操作外使用会失败。
- 循环引用和外部对象身份通常需要 Converter + context + `OnCompleted` 协作解决。
- Restore callback 只在整个操作成功后提交完成通知；异常时不会执行 completion callbacks。
- Compatible property restore 只跳过单个成员的数据/类型不兼容；对象结构或 restore hook 错误仍由上层事务处理。
- 序列化层只实现当前二进制契约，不包含旧 schema 的兼容读取或迁移分支。不兼容数据会直接失败。

单属性快照 CapturePropertyData 只读取指定的持久属性；其他 getter 不会因捕获该属性而执行。二进制节点编码先计算确定大小，再用标准库 BinaryWriter 写入独立的结果数组，避免增长缓冲区及最终复制。当前 magic、字段顺序、UTF-8 长度编码和调用方拥有的 bytes 语义保持一致；这是非公开实现，不增加格式或缓存协议。

已解析 Converter 的查找复用当前 generation 自有的并发缓存；首次解析及实例创建仍受同一个 owner 协调，最后一个 lease 释放后才清空缓存并退休实例。冻结 generation 的并行调用仍要求 Converter 和被读写数据自身满足相应线程约束，调用方应在所有操作完成后释放 lease。这些内部查找改良不改变 Registry、Context 或业务对象的所有权。

内部序列化操作仅在需要时创建回调、恢复去重与循环检测集合；不使用跨操作对象池。已有上下文只有在 TypeCatalog、Registry 和 metadata source 均属于当前 owner 时才复用，否则重新绑定这些依赖并保留调用方服务。`SerializationContext.With` 仍返回新的不可变上下文，确定性键排序和当前二进制格式保持不变。

## API 参考

[完整声明、行为与调用约束](Inno.Core.Serialization/api.md)

<!-- api-include: Inno.Core.Serialization/api.md -->

## 项目依赖

- [Inno.Core.Identity](Inno.Core.Identity.md)：公开 key builder 使用现有 identity owner；不创建第二个对象路由表。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。

## 捕获 key 源码 API 清单

本节对应当前源码声明；完整线程、所有权、失败和脚本边界见前文。签名仍由正式 Wiki 门禁复核。

<!-- api: T:Inno.Core.Serialization.SerializationCaptureKeyBuilder 6c97de0261870eb0b2ca7b43da673bbc954ac0cdb3d5651765f2c36f31bb68b9 -->
<!-- api: P:Inno.Core.Serialization.SerializationCaptureKeyBuilder.context 2ef0aecc865385075552b213d81f74edbb099fd65ea180fa70bceb058a352085 -->
<!-- api: M:Inno.Core.Serialization.SerializationCaptureKeyBuilder.EnsureCapacity(System.Int32) 85368cff5690182ff05680562168d382346aa6c09b171649a5c70802a2464da2 -->
<!-- api: M:Inno.Core.Serialization.SerializationCaptureKeyBuilder.TryAddOwner(Inno.Core.Identity.IdentityObject,System.Nullable{System.UInt64}) ff96320ab89162c6486f565a3bd1a9174177bda8a104e44d0150b17877845efd -->
<!-- api: M:Inno.Core.Serialization.SerializationCaptureKeyBuilder.TryAddProperties(Inno.Core.Serialization.ISerializable) dda539c1be3d7d72c4aa0a2c3b338840af0f399dad581cd3b0c7e32bc59a62cd -->
<!-- api: M:Inno.Core.Serialization.SerializationCaptureKeyBuilder.AddValue(System.UInt64) da6b1c5af5b7a4064330c4c6454b600fe1d0153826f7563c9ba01439e91a8315 -->
<!-- api: M:Inno.Core.Serialization.SerializationCaptureKeyBuilder.AddValue(System.Guid) d7df60a5ecaa3629190e2a1e1fd3e16a727574633203c1154c650771b12776a0 -->
<!-- api: M:Inno.Core.Serialization.SerializationCaptureKeyBuilder.AddText(System.String) 6c9c299d231276f170056bd9e2df9e3d2138e9ddf81229fa2790493636c2b821 -->
<!-- api: T:Inno.Core.Serialization.SerializationRevisionAttribute e49647c08c1f418466ab48adab7ddd73a70b19cc71fbe736c64f29102c42d02b -->
<!-- api: M:Inno.Core.Serialization.SerializationRevisionAttribute.#ctor(System.String) 47d42e2a55ba785652056081a29a9de665d5352efe06e9156fd770ad238a9fa3 -->
<!-- api: P:Inno.Core.Serialization.SerializationRevisionAttribute.memberName b7eb7e11d44a6f0466b3bfb3cb1afd2002595cbcaf75bef3c031735a4bb47772 -->
<!-- api: M:Inno.Core.Serialization.SerializationTypeMetadata.WithPureFieldAccess 0da51207ebb97cea53f9f8d209c0fca3d2c68ada4293bc6836c205943df23b76 -->
<!-- api: M:Inno.Core.Serialization.SerializationTypeMetadata.WithCaptureRevision(System.Func{System.Object,System.Nullable{System.UInt64}}) 6e24281b721a57687b72a2c39297a77d70c5d00b2342bb3135f227a741e1173a -->
<!-- api: M:Inno.Core.Serialization.Converters.SerializationConverter`1.TryCaptureKey(`0,Inno.Core.Serialization.SerializationCaptureKeyBuilder) 242ab82d7388793f935010754f931138b8f1aed9eb9217dafffcfc85ab47121d -->

| 源码声明 | 语义与约束 |
| --- | --- |
| [`public sealed class Inno.Core.Serialization.SerializationCaptureKeyBuilder`](../../../src/foundation/core/Inno.Core.Serialization/Capture/SerializationCaptureKeyBuilder.cs#L16) | 同步借用的完整 key builder；没有公开构造与释放权限。 |
| [`public Inno.Core.Serialization.SerializationContext Inno.Core.Serialization.SerializationCaptureKeyBuilder.context { get; }`](../../../src/foundation/core/Inno.Core.Serialization/Capture/SerializationCaptureKeyBuilder.cs#L64) | 借用 owner context；退休或跨线程读取抛 InvalidOperationException。 |
| [`public void Inno.Core.Serialization.SerializationCaptureKeyBuilder.EnsureCapacity(int capacity)`](../../../src/foundation/core/Inno.Core.Serialization/Capture/SerializationCaptureKeyBuilder.cs#L78) | capacity 是实际输入容量，负数拒绝；不改变 key。 |
| [`public bool Inno.Core.Serialization.SerializationCaptureKeyBuilder.TryAddOwner(Inno.Core.Identity.IdentityObject source, ulong? revision)`](../../../src/foundation/core/Inno.Core.Serialization/Capture/SerializationCaptureKeyBuilder.cs#L105) | source 为当前 Core owner；revision 覆盖全部所属状态，null/无效 identity 返回 false 并拒绝整个 key。 |
| [`public bool Inno.Core.Serialization.SerializationCaptureKeyBuilder.TryAddProperties(Inno.Core.Serialization.ISerializable source)`](../../../src/foundation/core/Inno.Core.Serialization/Capture/SerializationCaptureKeyBuilder.cs#L134) | source 为当前属性 owner；只有完整 revision、纯值与无自定义 Converter 证明时返回 true。 |
| [`public void Inno.Core.Serialization.SerializationCaptureKeyBuilder.AddValue(ulong value)`](../../../src/foundation/core/Inno.Core.Serialization/Capture/SerializationCaptureKeyBuilder.cs#L170) | value 作为有序精确中立输入；不解析或保存 live object。 |
| [`public void Inno.Core.Serialization.SerializationCaptureKeyBuilder.AddValue(System.Guid value)`](../../../src/foundation/core/Inno.Core.Serialization/Capture/SerializationCaptureKeyBuilder.cs#L181) | value 作为完整中立标识符比较，不编码为有碰撞风险的 hash。 |
| [`public void Inno.Core.Serialization.SerializationCaptureKeyBuilder.AddText(string? value)`](../../../src/foundation/core/Inno.Core.Serialization/Capture/SerializationCaptureKeyBuilder.cs#L192) | value 使用 ordinal 比较，null 与空串不同；不编码到持久数据。 |
| [`public sealed class Inno.Core.Serialization.SerializationRevisionAttribute : System.Attribute`](../../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationRevisionAttribute.cs#L13) | 声明 sealed identity source 的完整非持久化 revision 成员。 |
| [`public Inno.Core.Serialization.SerializationRevisionAttribute.SerializationRevisionAttribute(string memberName)`](../../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationRevisionAttribute.cs#L24) | memberName 选择可读 ulong/nullable ulong 成员；空名抛 ArgumentException。 |
| [`public string Inno.Core.Serialization.SerializationRevisionAttribute.memberName { get; }`](../../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationRevisionAttribute.cs#L33) | 取得唯一声明的 revision 成员名称。 |
| [`public Inno.Core.Serialization.SerializationTypeMetadata Inno.Core.Serialization.SerializationTypeMetadata.WithPureFieldAccess()`](../../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationTypeMetadata.cs#L108) | 返回声明纯 primitive 字段读取的不可变 metadata；非默认值类型抛 InvalidOperationException。 |
| [`public Inno.Core.Serialization.SerializationTypeMetadata Inno.Core.Serialization.SerializationTypeMetadata.WithCaptureRevision(System.Func<object, ulong?> readRevision)`](../../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationTypeMetadata.cs#L134) | readRevision 是 provider 所属的完整计数读取器；null 读取器拒绝，返回新 metadata，不改变原声明。 |
| [`protected virtual bool Inno.Core.Serialization.Converters.SerializationConverter<T>.TryCaptureKey(T value, Inno.Core.Serialization.SerializationCaptureKeyBuilder key)`](../../../src/foundation/core/Inno.Core.Serialization/Converters/SerializationConverter.cs#L81) | value 是当前根对象，key 只借用于本次同步回调；仅全部输出与上下文输入已覆盖时返回 true，默认 false。 |
