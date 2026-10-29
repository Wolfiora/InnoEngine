# Inno.Core.Serialization

[上一页：Identity](Inno.Core.Identity.md) · [Core 索引](README.md) · [Extensibility](../extensibility/Inno.Extensibility.Types.md)

`Inno.Core.Serialization` 提供确定性的二进制对象图格式、基于 attribute 的属性持久化，以及通过 TypeRegistry 自动发现的 Converter。它既支持“序列化完整 `ISerializable` 根对象”，也支持用 Writer/Reader 显式定义 schema。

### 完整状态捕获与复用

Host 可以使用 `CaptureSnapshot(value, context, previous)` 捕获不可变 `SerializationSnapshot`。每次仍完整执行当前 generation 的 getter 和 Converter；未变化的中立节点以 copy-on-write 复用，不依赖编辑通知或调用方上报 revision。`ToArray()` 使用同一二进制编码器返回独立 bytes，修改返回数组不会修改捕获结果。快照只持有中立标量、字符串和复制的二进制数据，不持有源对象、`Type`、Converter、delegate 或 generation lease，可在原 Registry 退休后编码。

该入口不导出到脚本。快照是 owner 所属的瞬时观察值，不是第二种文件格式，也不进入 History、Workspace 或资源元数据。比较失败时不发布新快照，旧快照保持有效。快照捕获期间，Converter 借用的 Writer 只属于其同步回调；不要在回调返回后继续写入。

内部 Writer 对同序、同类型且 converter 选择不变的标量复用一次位置查询，不重复查找相同节点；重复 member 仍明确失败。对象 keys 的现有二进制规范顺序保持不变，因此 Converter 调整写入次序不会改变规范 bytes，数组顺序仍属于数据。所有优化共用当前编码器与 generation lease，不缓存业务对象或绕过自定义 Converter。

`SerializationWriter.WritePropertiesData(name, value, context)` 将同一 generation 的独立属性快照写为现有 binary member，供 Scene 等需要保留独立恢复边界的 Converter 使用。它与 `CapturePropertiesData` 的 bytes 一致，并在重复捕获时复用未变化的属性节点和编码结果；传入的 context 必须保留 owner 的引用解析能力。普通序列化、保存与 Missing 恢复继续使用同一 pipeline，不建立领域专用 serializer。

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

源生成器在类型所属程序集产生私有成员访问器和构造工厂，派生类型通过基类所属目录组合继承信息。集合构造使用具体泛型调用，Core 的对象/结构体/集合 value pipeline 不执行成员反射或动态泛型构造。动态 Editor 的解析机制位于 [Inno.Adapter.Serialization.DotNet](../backends/DotNet/Inno.Adapter.Serialization.DotNet.md)。

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

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Core.Serialization.Converters.SerializationConverter`

<!-- api: T:Inno.Core.Serialization.Converters.SerializationConverter 5c9ee79952f69d6e2283ea93d46230258c89f636bea0e835a6bfbcf7ffca6700 -->
<!-- api: M:Inno.Core.Serialization.Converters.SerializationConverter.#ctor 33e46a5e66d8b6b1f77744d9a73a51f42144861a01bd0a03c3867d2458380224 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-095bbbcbeffed04b"></a>[`protected Inno.Core.Serialization.Converters.SerializationConverter.SerializationConverter()`](../../src/foundation/core/Inno.Core.Serialization/Converters/SerializationConverter.cs#L8) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-6e467ac49f50091d"></a>[`public abstract class Inno.Core.Serialization.Converters.SerializationConverter`](../../src/foundation/core/Inno.Core.Serialization/Converters/SerializationConverter.cs#L8) | Identifies a concrete serialization converter for generation-scoped type discovery. | — | — |

### `Inno.Core.Serialization.Converters.SerializationConverter`1`

<!-- api: T:Inno.Core.Serialization.Converters.SerializationConverter`1 1e82987af3daf6297ffc6e4111cadadc128846f946a7378e031689a350391016 -->
<!-- api: M:Inno.Core.Serialization.Converters.SerializationConverter`1.Write(Inno.Core.Serialization.SerializationWriter,`0) 55f7426415e872ee1322ecb26d05a6cc4e3ad25fcfd01ca8abd6fb1da1d94fc6 -->
<!-- api: M:Inno.Core.Serialization.Converters.SerializationConverter`1.Read(Inno.Core.Serialization.SerializationReader) 48607f9850741206e4e0f19d1c8955f72364f3bd34a5e3856f6bfd65bac707a0 -->
<!-- api: M:Inno.Core.Serialization.Converters.SerializationConverter`1.Restore(Inno.Core.Serialization.SerializationReader,`0) cba1bf0836cf98f2a62e08614137940a37a78f8040156f8ff6943226794b77fe -->
<!-- api: M:Inno.Core.Serialization.Converters.SerializationConverter`1.#ctor 654b06431a2fac0fa38fe554ad8094f0270b7793c80a135af8cecb64757e725b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-949b7721dbc9ff96"></a>[`protected Inno.Core.Serialization.Converters.SerializationConverter<T>.SerializationConverter()`](../../src/foundation/core/Inno.Core.Serialization/Converters/SerializationConverter.cs#L19) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-7f150363e1234329"></a>[`public abstract T Inno.Core.Serialization.Converters.SerializationConverter<T>.Read(Inno.Core.Serialization.SerializationReader reader)`](../../src/foundation/core/Inno.Core.Serialization/Converters/SerializationConverter.cs#L44) | Reads and creates a value from the current structured object.<br>返回：The restored value.<br>`reader`：The operation-scoped reader. | — | — |
| <a id="api-33ca386c4b08e5fd"></a>[`public virtual void Inno.Core.Serialization.Converters.SerializationConverter<T>.Restore(Inno.Core.Serialization.SerializationReader reader, T target)`](../../src/foundation/core/Inno.Core.Serialization/Converters/SerializationConverter.cs#L58) | Restores data into an existing value.<br>`reader`：The operation-scoped reader.<br>`target`：The existing target value. | — | `System.NotSupportedException`：Thrown when the converter does not support restoring existing values. |
| <a id="api-ec99400cd4570887"></a>[`public abstract void Inno.Core.Serialization.Converters.SerializationConverter<T>.Write(Inno.Core.Serialization.SerializationWriter writer, T value)`](../../src/foundation/core/Inno.Core.Serialization/Converters/SerializationConverter.cs#L30) | Writes a value into the current structured object.<br>`writer`：The operation-scoped writer.<br>`value`：The value to write. | — | — |
| <a id="api-a77a03185f231f88"></a>[`public abstract class Inno.Core.Serialization.Converters.SerializationConverter<T> : Inno.Core.Serialization.Converters.SerializationConverter`](../../src/foundation/core/Inno.Core.Serialization/Converters/SerializationConverter.cs#L19) | Defines the single advanced extension contract for serializing a specific value type. | — | — |

### `Inno.Core.Serialization.GenerateSerializationConverterAttribute`

<!-- api: T:Inno.Core.Serialization.GenerateSerializationConverterAttribute 5b1a14fb318307eba57276af799f0bd62d6636e10cea9f1d1c573b0b0d656757 -->
<!-- api: M:Inno.Core.Serialization.GenerateSerializationConverterAttribute.#ctor 73722471ee5599bc3ae8dfce99247c2a55417984cbec0ab37354006899e49426 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2cea33f612b7cac5"></a>[`public Inno.Core.Serialization.GenerateSerializationConverterAttribute.GenerateSerializationConverterAttribute()`](../../src/foundation/core/Inno.Core.Serialization/Attributes/GenerateSerializationConverterAttribute.cs#L15) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-11812311c3a25ad5"></a>[`public sealed class Inno.Core.Serialization.GenerateSerializationConverterAttribute : System.Attribute`](../../src/foundation/core/Inno.Core.Serialization/Attributes/GenerateSerializationConverterAttribute.cs#L15) | Requests a compile-time serialization converter for a closed data-transfer type. | The annotated type must implement Inno.Core.Serialization.ISerializable, expose a constructor that generated code can invoke, and use Inno.Core.Serialization.SerializablePropertyAttribute on every persisted member. Types with polymorphic identity, reference graphs, or custom restoration invariants must use an explicit converter instead. | — |

### `Inno.Core.Serialization.ISerializable`

<!-- api: T:Inno.Core.Serialization.ISerializable adf7c19424d7c870b1059a9903548e8df1e50d45374eb52484626eb1e3434c94 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b1a3ede6855f6cb8"></a>[`public interface Inno.Core.Serialization.ISerializable`](../../src/foundation/core/Inno.Core.Serialization/Contracts/ISerializable.cs#L6) | Marks a reference type whose annotated properties can participate in property-based serialization. | — | — |

### `Inno.Core.Serialization.ISerializationMetadataSource`

<!-- api: T:Inno.Core.Serialization.ISerializationMetadataSource 8ba6cdf5ba31d1f2ee3ccfcea4dbcdcfdb4117dd96f491fc22f01c465a176e4c -->
<!-- api: M:Inno.Core.Serialization.ISerializationMetadataSource.GetMetadata(System.Type) 55876bc6f85adb2fbf968adaa7be19d4fbafb8144f7291d4fbbd81c790d8a7be -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d9c4551a4752e1ba"></a>[`Inno.Core.Serialization.SerializationTypeMetadata Inno.Core.Serialization.ISerializationMetadataSource.GetMetadata(System.Type type)`](../../src/foundation/core/Inno.Core.Serialization/Metadata/ISerializationMetadataSource.cs#L19) | Resolves a complete serialization shape for the current declaration.<br>返回：Immutable access metadata; an unsupported declaration throws instead of probing another provider.<br>`type`：The exact closed declaration, including its generic arguments. | — | — |
| <a id="api-ce5500d26cc2e970"></a>[`public interface Inno.Core.Serialization.ISerializationMetadataSource`](../../src/foundation/core/Inno.Core.Serialization/Metadata/ISerializationMetadataSource.cs#L8) | Supplies declaration access and construction without choosing a runtime reflection implementation. | — | — |

### `Inno.Core.Serialization.OnSerializableRestored`

<!-- api: T:Inno.Core.Serialization.OnSerializableRestored ab12df828b83d61d7dfcc5e4e6146fc40a87da529e8f47bbeed605a5ae0ffbab -->
<!-- api: M:Inno.Core.Serialization.OnSerializableRestored.#ctor 21a62774cd642749f14b08c9a42ba12adbb1498977f67e029903905dbc36fcab -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2430dbbbdc08844b"></a>[`public Inno.Core.Serialization.OnSerializableRestored.OnSerializableRestored()`](../../src/foundation/core/Inno.Core.Serialization/Attributes/OnSerializableRestored.cs#L9) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-92cbb79273dcda20"></a>[`public sealed class Inno.Core.Serialization.OnSerializableRestored : System.Attribute`](../../src/foundation/core/Inno.Core.Serialization/Attributes/OnSerializableRestored.cs#L9) | Marks one nonvirtual instance method to run after restoration succeeds, optionally receiving its SerializationContext. | — | — |

### `Inno.Core.Serialization.PropertyVisibility`

<!-- api: T:Inno.Core.Serialization.PropertyVisibility 84c2172ec524d0df0c616a6eb2eace6ebc87c381c441a7413ad4dca202b3751c -->
<!-- api: F:Inno.Core.Serialization.PropertyVisibility.None fa994c3b8abc935832e1cf90e7dc53d6d28d0aa6cd2edfa375937c0ff5c92532 -->
<!-- api: F:Inno.Core.Serialization.PropertyVisibility.Serialize c5a1807286179a973565b4de82c4c71b0eed82a322717203dfbed25ec30dac79 -->
<!-- api: F:Inno.Core.Serialization.PropertyVisibility.Deserialize 77d07731d8c90799a73c6828ee58ef213336269652a66d90a0e35f6b23cdaa4f -->
<!-- api: F:Inno.Core.Serialization.PropertyVisibility.RuntimeGet de702d525993187d5abdbf40c56abfc7ef2ec60097d804f795a79bcd608312b9 -->
<!-- api: F:Inno.Core.Serialization.PropertyVisibility.RuntimeSet a725699684f6dd0fe5659f32380bd2d5d37d3588f42e7e02940e4e839f7f9412 -->
<!-- api: F:Inno.Core.Serialization.PropertyVisibility.Show b51615088e1980cb3d6045d9a51265f1841729935653d789e04313b9f38fa51c -->
<!-- api: F:Inno.Core.Serialization.PropertyVisibility.Hide 2e8a5143c006221418cb894fbcb2340ff2e0b09a13b2485d508f7b25d00006e3 -->
<!-- api: F:Inno.Core.Serialization.PropertyVisibility.Readonly cf7ae80eeb167ce7232e3fa0b33238ae6060bfac2f830be4d317b128e58b5a59 -->
<!-- api: F:Inno.Core.Serialization.PropertyVisibility.Transient 2f0b611d29819770c53d7e03980367f5c6ed09de679ccfa3b3a0a0acfb43ec40 -->
<!-- api: F:Inno.Core.Serialization.PropertyVisibility.SerializeOnly 3d8634535169d906923dbc319aa5ad5431fc72a9702654a8cbc4d5c429e35855 -->
<!-- api: F:Inno.Core.Serialization.PropertyVisibility.DeserializeOnly 1022e9ad15fa93472d74e9aaed9b93735958f820f865eed14ff8cff85efddecf -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7723ebed8c34fe6d"></a>[`Inno.Core.Serialization.PropertyVisibility.Deserialize = 2`](../../src/foundation/core/Inno.Core.Serialization/Metadata/PropertyVisibility.cs#L28) | Participates in deserialization (restore/read). | — | — |
| <a id="api-1c16044e68c1cfda"></a>[`Inno.Core.Serialization.PropertyVisibility.DeserializeOnly = Inno.Core.Serialization.PropertyVisibility.Deserialize &#124; Inno.Core.Serialization.PropertyVisibility.Transient`](../../src/foundation/core/Inno.Core.Serialization/Metadata/PropertyVisibility.cs#L69) | Participates in deserialization only (no serialization), runtime can get/set. | — | — |
| <a id="api-e80a773076a47fc6"></a>[`Inno.Core.Serialization.PropertyVisibility.Hide = Inno.Core.Serialization.PropertyVisibility.Serialize &#124; Inno.Core.Serialization.PropertyVisibility.Deserialize`](../../src/foundation/core/Inno.Core.Serialization/Metadata/PropertyVisibility.cs#L48) | Can serialize and deserialize, but runtime code cannot get or set through Inno.Core.Serialization.SerializedProperty. | — | — |
| <a id="api-9d8577197199382c"></a>[`Inno.Core.Serialization.PropertyVisibility.None = 0`](../../src/foundation/core/Inno.Core.Serialization/Metadata/PropertyVisibility.cs#L18) | No participation in serialization, deserialization, or runtime access. | — | — |
| <a id="api-647d6a5a0d983848"></a>[`Inno.Core.Serialization.PropertyVisibility.Readonly = Inno.Core.Serialization.PropertyVisibility.Hide &#124; Inno.Core.Serialization.PropertyVisibility.RuntimeGet`](../../src/foundation/core/Inno.Core.Serialization/Metadata/PropertyVisibility.cs#L54) | Can serialize and deserialize, runtime can get but cannot set. Runtime set attempts should be rejected (see Inno.Core.Serialization.SerializedProperty.SetValue(System.Object) behavior). | — | — |
| <a id="api-bad46a449fef09a3"></a>[`Inno.Core.Serialization.PropertyVisibility.RuntimeGet = 4`](../../src/foundation/core/Inno.Core.Serialization/Metadata/PropertyVisibility.cs#L33) | Allows runtime retrieval through the Inno.Core.Serialization.SerializedProperty API. | — | — |
| <a id="api-4f980081668617b5"></a>[`Inno.Core.Serialization.PropertyVisibility.RuntimeSet = 8`](../../src/foundation/core/Inno.Core.Serialization/Metadata/PropertyVisibility.cs#L38) | Allows runtime assignment through the Inno.Core.Serialization.SerializedProperty API. | — | — |
| <a id="api-98b7a5d314f417fe"></a>[`Inno.Core.Serialization.PropertyVisibility.Serialize = 1`](../../src/foundation/core/Inno.Core.Serialization/Metadata/PropertyVisibility.cs#L23) | Participates in serialization output. | — | — |
| <a id="api-4903d78a6db8e14f"></a>[`Inno.Core.Serialization.PropertyVisibility.SerializeOnly = Inno.Core.Serialization.PropertyVisibility.Serialize &#124; Inno.Core.Serialization.PropertyVisibility.Transient`](../../src/foundation/core/Inno.Core.Serialization/Metadata/PropertyVisibility.cs#L64) | Participates in serialization only (no deserialization), runtime can get/set. | — | — |
| <a id="api-6fc8c291836f188f"></a>[`Inno.Core.Serialization.PropertyVisibility.Show = Inno.Core.Serialization.PropertyVisibility.Serialize &#124; Inno.Core.Serialization.PropertyVisibility.DeserializeOnly`](../../src/foundation/core/Inno.Core.Serialization/Metadata/PropertyVisibility.cs#L43) | Can serialize, deserialize, and allow runtime get/set. | — | — |
| <a id="api-b9fc50382d707d72"></a>[`Inno.Core.Serialization.PropertyVisibility.Transient = Inno.Core.Serialization.PropertyVisibility.RuntimeGet &#124; Inno.Core.Serialization.PropertyVisibility.RuntimeSet`](../../src/foundation/core/Inno.Core.Serialization/Metadata/PropertyVisibility.cs#L59) | Does not participate in serialization or deserialization, but runtime can get/set. | — | — |
| <a id="api-3c81aa3af425f240"></a>[`public enum Inno.Core.Serialization.PropertyVisibility : int`](../../src/foundation/core/Inno.Core.Serialization/Metadata/PropertyVisibility.cs#L13) | Defines member participation rules for persistence and runtime access. | This is a flag enum. Prefer the named presets (Inno.Core.Serialization.PropertyVisibility.Show, Inno.Core.Serialization.PropertyVisibility.Hide, Inno.Core.Serialization.PropertyVisibility.Readonly, Inno.Core.Serialization.PropertyVisibility.Transient, Inno.Core.Serialization.PropertyVisibility.SerializeOnly, Inno.Core.Serialization.PropertyVisibility.DeserializeOnly) for common configurations. | — |

### `Inno.Core.Serialization.RequiresSerializationConverterAttribute`

<!-- api: T:Inno.Core.Serialization.RequiresSerializationConverterAttribute 8d9a3b00e48561bf67f0585b651be5f8f4045296417319c5a402f099c8abc76e -->
<!-- api: M:Inno.Core.Serialization.RequiresSerializationConverterAttribute.#ctor 7676b8ecb9e715058800695adeb60fca758b15bdfc9d5c27ea8a6b72589f413c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-38107d8d01abb4e4"></a>[`public Inno.Core.Serialization.RequiresSerializationConverterAttribute.RequiresSerializationConverterAttribute()`](../../src/foundation/core/Inno.Core.Serialization/Attributes/RequiresSerializationConverterAttribute.cs#L9) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-448155a382c2ef00"></a>[`public sealed class Inno.Core.Serialization.RequiresSerializationConverterAttribute : System.Attribute`](../../src/foundation/core/Inno.Core.Serialization/Attributes/RequiresSerializationConverterAttribute.cs#L9) | Requires a serializable type to use an explicitly discovered converter. | — | — |

### `Inno.Core.Serialization.SerializablePropertyAttribute`

<!-- api: T:Inno.Core.Serialization.SerializablePropertyAttribute 17bffd6bda214f0026e9876650f829cf2c6548f041d368cf55c19f828f533c92 -->
<!-- api: P:Inno.Core.Serialization.SerializablePropertyAttribute.order bc590684627a939d4d98641dd33d55c121c6807c1bc9b7228cc0aab6eb5e447c -->
<!-- api: P:Inno.Core.Serialization.SerializablePropertyAttribute.propertyVisibility 29e8b9262b7326b5a306de62c149e5b3c7849ef12c7b307ab3ce1ef51dd18ee1 -->
<!-- api: M:Inno.Core.Serialization.SerializablePropertyAttribute.#ctor(Inno.Core.Serialization.PropertyVisibility) 110475887e6c6df37f93fd03c9ef3cfce4ce7f90923dbdc45a6bb06d8ab07cb3 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-94755b7f39a633fc"></a>[`public Inno.Core.Serialization.SerializablePropertyAttribute.SerializablePropertyAttribute(Inno.Core.Serialization.PropertyVisibility visibility = Inno.Core.Serialization.PropertyVisibility.Show)`](../../src/foundation/core/Inno.Core.Serialization/Attributes/SerializablePropertyAttribute.cs#L33) | Initializes a new instance of the Inno.Core.Serialization.SerializablePropertyAttribute class.<br>`visibility`：The member visibility in the serialization graph. | — | — |
| <a id="api-9ccd3ebafac31f22"></a>[`public int Inno.Core.Serialization.SerializablePropertyAttribute.order { get; set; }`](../../src/foundation/core/Inno.Core.Serialization/Attributes/SerializablePropertyAttribute.cs#L20) | Gets or sets the member order within its declaring type. | Lower values are processed first. The script compiler fills this value from source declaration order. | — |
| <a id="api-a08df422923edfa2"></a>[`public Inno.Core.Serialization.PropertyVisibility Inno.Core.Serialization.SerializablePropertyAttribute.propertyVisibility { get; }`](../../src/foundation/core/Inno.Core.Serialization/Attributes/SerializablePropertyAttribute.cs#L25) | Gets the visibility of the annotated member. | — | — |
| <a id="api-69b10187f4da906c"></a>[`public sealed class Inno.Core.Serialization.SerializablePropertyAttribute : System.Attribute`](../../src/foundation/core/Inno.Core.Serialization/Attributes/SerializablePropertyAttribute.cs#L12) | Declares a field or property as participating in Inno.Core.Serialization.ISerializable property persistence. | The default visibility is Inno.Core.Serialization.PropertyVisibility.Show. | — |

### `Inno.Core.Serialization.SerializationCollectionMetadata`

<!-- api: T:Inno.Core.Serialization.SerializationCollectionMetadata f6317e7d1d94853d92e5cb51516b00ecd2e3701bccf152796ea8bb9578c2926f -->
<!-- api: M:Inno.Core.Serialization.SerializationCollectionMetadata.#ctor(System.Type,System.Type,System.Func{System.Collections.Generic.IReadOnlyList{System.Object},System.Object},System.Func{System.Collections.Generic.IReadOnlyList{System.Collections.Generic.KeyValuePair{System.Object,System.Object}},System.Object},System.Func{System.Object,System.Collections.Generic.IReadOnlyList{System.Collections.Generic.KeyValuePair{System.Object,System.Object}}}) 342a657e5dbb940d2a9bbb4463b05c77cda6fbda159d1b9e9975d02f87edfed3 -->
<!-- api: P:Inno.Core.Serialization.SerializationCollectionMetadata.elementType 19cc39e70aa108fe907b3749cd83b4364c2fc126a67f98c999414c37237bb4ab -->
<!-- api: P:Inno.Core.Serialization.SerializationCollectionMetadata.keyType 4174864a2d226e6fe30c0ea0fbdb303745721f6a589af61aa7ef354ea429ba2f -->
<!-- api: P:Inno.Core.Serialization.SerializationCollectionMetadata.buildSequence d40ef4c4da4b369b1207773a8ddb65a8564c30c750af4596ab37d7b827911a3c -->
<!-- api: P:Inno.Core.Serialization.SerializationCollectionMetadata.buildMap 8896ed958cf71f7c6d670ac9e7724da729a0f6381908bf9ef8c8edb640362c46 -->
<!-- api: P:Inno.Core.Serialization.SerializationCollectionMetadata.enumerateMap dd722e6587623eb0e2358eb63e0852d29c61eb9853fdcfe1d8a53a0f0b57e517 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4308240581e0ca6d"></a>[`public Inno.Core.Serialization.SerializationCollectionMetadata.SerializationCollectionMetadata(System.Type elementType, System.Type? keyType, System.Func<System.Collections.Generic.IReadOnlyList<object?>, object>? buildSequence, System.Func<System.Collections.Generic.IReadOnlyList<System.Collections.Generic.KeyValuePair<object?, object?>>, object>? buildMap, System.Func<object, System.Collections.Generic.IReadOnlyList<System.Collections.Generic.KeyValuePair<object?, object?>>>? enumerateMap)`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationCollectionMetadata.cs#L32) | Defines the shape and typed operations of a sequence or map.<br>`elementType`：The sequence element or map value type.<br>`keyType`：The map key type, or null for a sequence.<br>`buildSequence`：A sequence factory, or null for a map.<br>`buildMap`：A map factory, or null for a sequence.<br>`enumerateMap`：Typed map enumeration, or null for a sequence. | — | `System.ArgumentException`：The operations do not describe exactly one complete collection shape. |
| <a id="api-b20d049e93ee3212"></a>[`public System.Func<System.Collections.Generic.IReadOnlyList<System.Collections.Generic.KeyValuePair<object?, object?>>, object>? Inno.Core.Serialization.SerializationCollectionMetadata.buildMap { get; }`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationCollectionMetadata.cs#L68) | Gets the typed map constructor, or null for a sequence. | — | — |
| <a id="api-7a8e82bf4eccaaca"></a>[`public System.Func<System.Collections.Generic.IReadOnlyList<object?>, object>? Inno.Core.Serialization.SerializationCollectionMetadata.buildSequence { get; }`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationCollectionMetadata.cs#L63) | Gets the typed sequence constructor, or null for a map. | — | — |
| <a id="api-de3e179936eb5bed"></a>[`public System.Type Inno.Core.Serialization.SerializationCollectionMetadata.elementType { get; }`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationCollectionMetadata.cs#L53) | Gets the sequence element or map value type. | — | — |
| <a id="api-034c53bff0c53f48"></a>[`public System.Func<object, System.Collections.Generic.IReadOnlyList<System.Collections.Generic.KeyValuePair<object?, object?>>>? Inno.Core.Serialization.SerializationCollectionMetadata.enumerateMap { get; }`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationCollectionMetadata.cs#L73) | Gets typed map enumeration, or null for a sequence. | — | — |
| <a id="api-a69c1e52938095bf"></a>[`public System.Type? Inno.Core.Serialization.SerializationCollectionMetadata.keyType { get; }`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationCollectionMetadata.cs#L58) | Gets the map key type, or null for a sequence. | — | — |
| <a id="api-6db23bce20aeb15c"></a>[`public sealed class Inno.Core.Serialization.SerializationCollectionMetadata`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationCollectionMetadata.cs#L9) | Supplies typed collection construction and map enumeration without runtime generic compilation. | — | — |

### `Inno.Core.Serialization.SerializationContext`

<!-- api: T:Inno.Core.Serialization.SerializationContext 538004cb3ada7cd8c123767790c3f5d3f3089f7c7d8ea477559401b7378f7e33 -->
<!-- api: P:Inno.Core.Serialization.SerializationContext.empty 5bce9384d0472f991121822e7507dde4feadf69cd2b9cea329897d51ce1fbd96 -->
<!-- api: M:Inno.Core.Serialization.SerializationContext.With``1(``0) a7418fd86a90f7c415ae7d3705246bf8f6ba2476c6ed6f9c4764ee9165699d6a -->
<!-- api: M:Inno.Core.Serialization.SerializationContext.TryGet``1(``0@) c79eeb53db91d441296a75468aa7deacf7f94f952c8a179000bc31632af3447f -->
<!-- api: M:Inno.Core.Serialization.SerializationContext.GetRequired``1 c61d0c7f27493e0c65ea8a0b93d3a6f93e2a6301160ee16a66cd40ee95de6253 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-61674d926170341e"></a>[`public TContext Inno.Core.Serialization.SerializationContext.GetRequired<TContext>() where TContext : class`](../../src/foundation/core/Inno.Core.Serialization/Context/SerializationContext.cs#L84) | Resolves a value registered under the exact context contract type.<br>返回：The registered context value. | — | `System.InvalidOperationException`：Thrown when the requested context was not registered. |
| <a id="api-5a888eef07e65506"></a>[`public bool Inno.Core.Serialization.SerializationContext.TryGet<TContext>(out TContext? value) where TContext : class`](../../src/foundation/core/Inno.Core.Serialization/Context/SerializationContext.cs#L60) | Attempts to resolve a value registered under the exact context contract type.<br>返回：true when a value is registered; otherwise, false.<br>`value`：The registered value when available. | — | — |
| <a id="api-aec399826cfbdc2d"></a>[`public Inno.Core.Serialization.SerializationContext Inno.Core.Serialization.SerializationContext.With<TContext>(TContext value) where TContext : class`](../../src/foundation/core/Inno.Core.Serialization/Context/SerializationContext.cs#L38) | Returns a new context containing the supplied value under its declared context type.<br>返回：A new context containing the registered value.<br>`value`：The context value to register. | — | `System.ArgumentNullException`：Thrown when value is null. |
| <a id="api-f03ceb31e37cbd5f"></a>[`public static Inno.Core.Serialization.SerializationContext Inno.Core.Serialization.SerializationContext.empty { get; }`](../../src/foundation/core/Inno.Core.Serialization/Context/SerializationContext.cs#L21) | Gets an empty serialization context. | — | — |
| <a id="api-3827a2d220cf4954"></a>[`public sealed class Inno.Core.Serialization.SerializationContext`](../../src/foundation/core/Inno.Core.Serialization/Context/SerializationContext.cs#L9) | Provides immutable, operation-independent context to serialization converters. | — | — |

### `Inno.Core.Serialization.SerializationGeneration`

<!-- api: T:Inno.Core.Serialization.SerializationGeneration 17f9bf32c7924a338f280550b69a79c5daa0b550c6436c3e9f6e85b4f1baa766 -->
<!-- api: M:Inno.Core.Serialization.SerializationGeneration.Serialize``1(``0,Inno.Core.Serialization.SerializationContext) 25ba1604b162cfb7271399a3b644be530b679d22a71aa80d07476904c5c67bb8 -->
<!-- api: M:Inno.Core.Serialization.SerializationGeneration.Deserialize``1(System.ReadOnlySpan{System.Byte},Inno.Core.Serialization.SerializationContext) 83431ecb5b6d6b07679fd019e157eb5f371405b6cd564655a520e8bc22e67960 -->
<!-- api: M:Inno.Core.Serialization.SerializationGeneration.Encode(System.Action{Inno.Core.Serialization.SerializationWriter},Inno.Core.Serialization.SerializationContext) dd640c58a09d286b82833d1b3550d6f5303b20f668334595dab93585b4293196 -->
<!-- api: M:Inno.Core.Serialization.SerializationGeneration.Decode``1(System.ReadOnlySpan{System.Byte},System.Func{Inno.Core.Serialization.SerializationReader,``0},Inno.Core.Serialization.SerializationContext) 70c36f02daa49e8953ba14d5052a7bf9cb98e39bf102f3e4a3aaf77e2dc3c45f -->
<!-- api: M:Inno.Core.Serialization.SerializationGeneration.Dispose 0af3c06f1fcf11f0f97f8d0a53dcdf3c3ac8455d8139ab1502c0c43745f21d18 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c8faf9c4019d2d9c"></a>[`public TResult Inno.Core.Serialization.SerializationGeneration.Decode<TResult>(System.ReadOnlySpan<byte> bytes, System.Func<Inno.Core.Serialization.SerializationReader, TResult> read, Inno.Core.Serialization.SerializationContext? context = null)`](../../src/foundation/core/Inno.Core.Serialization/SerializationGeneration.cs#L128) | Decodes a manually defined structured root using the converter generation captured by this instance.<br>返回：The callback result after all restoration callbacks complete.<br>`bytes`：Deterministic binary data containing the structured root.<br>`read`：The callback that consumes the complete root object.<br>`context`：Optional immutable converter context for this operation. | — | `System.ArgumentNullException`：Thrown when read is null.<br>`System.ObjectDisposedException`：Thrown when this generation has already been disposed. |
| <a id="api-fe23d17205f7ed9e"></a>[`public T Inno.Core.Serialization.SerializationGeneration.Deserialize<T>(System.ReadOnlySpan<byte> bytes, Inno.Core.Serialization.SerializationContext? context = null) where T : class, Inno.Core.Serialization.ISerializable`](../../src/foundation/core/Inno.Core.Serialization/SerializationGeneration.cs#L73) | Deserializes a complete root object using the converter generation captured by this instance.<br>返回：The restored root object.<br>`bytes`：Deterministic binary data containing the root object.<br>`context`：Optional immutable converter context for this operation. | — | `System.ObjectDisposedException`：Thrown when this generation has already been disposed. |
| <a id="api-bb64b7bcbf5907dd"></a>[`public void Inno.Core.Serialization.SerializationGeneration.Dispose()`](../../src/foundation/core/Inno.Core.Serialization/SerializationGeneration.cs#L138) | Releases the pinned converter generation and every collectible type reference owned by this lease. | — | — |
| <a id="api-f3672a35d8da0d26"></a>[`public byte[] Inno.Core.Serialization.SerializationGeneration.Encode(System.Action<Inno.Core.Serialization.SerializationWriter> write, Inno.Core.Serialization.SerializationContext? context = null)`](../../src/foundation/core/Inno.Core.Serialization/SerializationGeneration.cs#L98) | Encodes a manually defined structured root using the converter generation captured by this instance.<br>返回：Deterministic binary data containing the structured root.<br>`write`：The callback that writes the complete root object.<br>`context`：Optional immutable converter context for this operation. | — | `System.ArgumentNullException`：Thrown when write is null.<br>`System.ObjectDisposedException`：Thrown when this generation has already been disposed. |
| <a id="api-a32124b1bf098381"></a>[`public byte[] Inno.Core.Serialization.SerializationGeneration.Serialize<T>(T value, Inno.Core.Serialization.SerializationContext? context = null) where T : class, Inno.Core.Serialization.ISerializable`](../../src/foundation/core/Inno.Core.Serialization/SerializationGeneration.cs#L48) | Serializes a complete root object using the converter generation captured by this instance.<br>返回：Deterministic binary data encoded by the captured converter generation.<br>`value`：The non-null root object to encode.<br>`context`：Optional immutable converter context for this operation. | — | `System.ArgumentNullException`：Thrown when value is null.<br>`System.ObjectDisposedException`：Thrown when this generation has already been disposed. |
| <a id="api-629826818c1ca289"></a>[`public sealed class Inno.Core.Serialization.SerializationGeneration : System.IDisposable`](../../src/foundation/core/Inno.Core.Serialization/SerializationGeneration.cs#L14) | Pins one immutable converter generation so serialization remains deterministic across asynchronous continuations and extension reloads. | A generation may execute operations from any thread after it has been captured. The owner must dispose it when the surrounding transaction completes so collectible extension generations can unload. | — |

### `Inno.Core.Serialization.SerializationMemberMetadata`

<!-- api: T:Inno.Core.Serialization.SerializationMemberMetadata 21186c272875f49b67c873451713c7989fba21bbbe6a60bd6af9980789f2f769 -->
<!-- api: M:Inno.Core.Serialization.SerializationMemberMetadata.#ctor(System.String,System.Type,Inno.Core.Serialization.PropertyVisibility,System.Func{System.Object,System.Object},System.Action{System.Object,System.Object}) 661d5a7742047c7516aeb8bf033bcc6a56535d7419529fc40ee7e22824d80393 -->
<!-- api: P:Inno.Core.Serialization.SerializationMemberMetadata.name 5f0c01bad08401ffecd2492e64f0b661f65f777a5cc103a3483b2c5b7993546d -->
<!-- api: P:Inno.Core.Serialization.SerializationMemberMetadata.type 5aa74a595d72a556174b10e9e57d8cb8b2ee4356bef2320a66b9fb0ccf86690d -->
<!-- api: P:Inno.Core.Serialization.SerializationMemberMetadata.visibility 76539a272651f5eaeba105a167c9c04f5959cce7da675df2d019cc17250b2481 -->
<!-- api: M:Inno.Core.Serialization.SerializationMemberMetadata.GetValue(System.Object) 9ee1ccd97a3af860adb19b17e89d339ae0d34b3b2325751b0506795da23905b0 -->
<!-- api: M:Inno.Core.Serialization.SerializationMemberMetadata.SetValue(System.Object,System.Object) 3d9bf5d2606cdd37a371733ed172a8a7e34e8f3154a9597d1f7a9dca37bfe6f7 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e7f577d3d17d0752"></a>[`public Inno.Core.Serialization.SerializationMemberMetadata.SerializationMemberMetadata(string name, System.Type type, Inno.Core.Serialization.PropertyVisibility visibility, System.Func<object, object?>? getter, System.Action<object, object?>? setter)`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationMemberMetadata.cs#L34) | Defines a member whose access is implemented by the selected metadata provider.<br>`name`：The exact persistent key.<br>`type`：The declared value type.<br>`visibility`：Persistent and runtime access permissions.<br>`getter`：A read accessor, or null when reading is not permitted.<br>`setter`：A write accessor, or null when writing is not permitted. | — | `System.ArgumentException`：The name is empty or an accessor required by the visibility is absent. |
| <a id="api-a70fa1db4097d8dd"></a>[`public object? Inno.Core.Serialization.SerializationMemberMetadata.GetValue(object target)`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationMemberMetadata.cs#L81) | Reads the current value through the selected declaration accessor.<br>返回：The current value, including null when allowed by the declaration.<br>`target`：The declaring object; ownership remains with the caller. | — | `System.InvalidOperationException`：Reading is not permitted by this metadata. |
| <a id="api-9acafe9d7bbd26e3"></a>[`public void Inno.Core.Serialization.SerializationMemberMetadata.SetValue(object target, object? value)`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationMemberMetadata.cs#L97) | Writes a value through the selected declaration accessor.<br>`target`：The object receiving the value.<br>`value`：A value compatible with the declared member type. | — | `System.InvalidOperationException`：Writing is not permitted by this metadata. |
| <a id="api-ad6da8a02d383381"></a>[`public string Inno.Core.Serialization.SerializationMemberMetadata.name { get; }`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationMemberMetadata.cs#L57) | Gets the exact persistent key. | — | — |
| <a id="api-cfddd46aea18038c"></a>[`public System.Type Inno.Core.Serialization.SerializationMemberMetadata.type { get; }`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationMemberMetadata.cs#L62) | Gets the declared value type. | — | — |
| <a id="api-70f13cbe9fa5a8e3"></a>[`public Inno.Core.Serialization.PropertyVisibility Inno.Core.Serialization.SerializationMemberMetadata.visibility { get; }`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationMemberMetadata.cs#L67) | Gets the member's persistent and runtime access permissions. | — | — |
| <a id="api-9aef015422855559"></a>[`public sealed class Inno.Core.Serialization.SerializationMemberMetadata`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationMemberMetadata.cs#L8) | Captures one ordered serialization key and its generation-owned accessors. | — | — |

### `Inno.Core.Serialization.SerializationPropertyRestoreFailure`

<!-- api: T:Inno.Core.Serialization.SerializationPropertyRestoreFailure fd35050b15865eb97a29868ed562031737341cabed36976609f670adde3f8a30 -->
<!-- api: P:Inno.Core.Serialization.SerializationPropertyRestoreFailure.name 4681bb3f6b97c9c83980718ad1b75048e83fa80b3a95d54a1383223d710ebcf4 -->
<!-- api: P:Inno.Core.Serialization.SerializationPropertyRestoreFailure.previousPropertyType 0f39fd84c27087fce43ef6fb920402289c13f7e1507c1a3b19c6a7d6d9371d52 -->
<!-- api: P:Inno.Core.Serialization.SerializationPropertyRestoreFailure.currentPropertyType 37cf00b9a2dc7a81c187d5ecd07a15ec0f7c6284bd94a36941cf0747f469d168 -->
<!-- api: P:Inno.Core.Serialization.SerializationPropertyRestoreFailure.message 0ed4827c186ee126e561540c4dae76595ac9f0e45253f97efdb60d811ac4708c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b871a4144ca1898c"></a>[`public System.Type Inno.Core.Serialization.SerializationPropertyRestoreFailure.currentPropertyType { get; }`](../../src/foundation/core/Inno.Core.Serialization/PropertyRestoration/SerializationPropertyRestoreFailure.cs#L35) | Gets the current target member type. | — | — |
| <a id="api-9a448ae3888e0dda"></a>[`public string Inno.Core.Serialization.SerializationPropertyRestoreFailure.message { get; }`](../../src/foundation/core/Inno.Core.Serialization/PropertyRestoration/SerializationPropertyRestoreFailure.cs#L40) | Gets the underlying compatibility failure message. | — | — |
| <a id="api-fef7f6d48804f5b9"></a>[`public string Inno.Core.Serialization.SerializationPropertyRestoreFailure.name { get; }`](../../src/foundation/core/Inno.Core.Serialization/PropertyRestoration/SerializationPropertyRestoreFailure.cs#L25) | Gets the serialized member key. | — | — |
| <a id="api-1225a0cad0e85ae8"></a>[`public System.Type Inno.Core.Serialization.SerializationPropertyRestoreFailure.previousPropertyType { get; }`](../../src/foundation/core/Inno.Core.Serialization/PropertyRestoration/SerializationPropertyRestoreFailure.cs#L30) | Gets the type used to capture the previous value. | — | — |
| <a id="api-f791e086bc6228d5"></a>[`public sealed class Inno.Core.Serialization.SerializationPropertyRestoreFailure`](../../src/foundation/core/Inno.Core.Serialization/PropertyRestoration/SerializationPropertyRestoreFailure.cs#L8) | Describes one property skipped during a compatible property restore. | — | — |

### `Inno.Core.Serialization.SerializationPropertyRestoreMode`

<!-- api: T:Inno.Core.Serialization.SerializationPropertyRestoreMode 95329d1e0f3a73ef04318bd453bed834b5ef0dfa248a6220bf3c50312c18b2ae -->
<!-- api: F:Inno.Core.Serialization.SerializationPropertyRestoreMode.Strict f595cd2c2ccbc004ceec54594ebdcd29a615532245af2b007c6797cb4d4c77c7 -->
<!-- api: F:Inno.Core.Serialization.SerializationPropertyRestoreMode.CollectFailures 96bb7b8a5e23f0767729ae4d3d87758ad18f1724c1fcbe520d9615c50af49c4e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-55dbb29725179fc0"></a>[`Inno.Core.Serialization.SerializationPropertyRestoreMode.CollectFailures = 1`](../../src/foundation/core/Inno.Core.Serialization/PropertyRestoration/SerializationPropertyRestoreMode.cs#L16) | Continues restoring independent properties and reports every rejected value. | — | — |
| <a id="api-68c62e4ea7f49767"></a>[`Inno.Core.Serialization.SerializationPropertyRestoreMode.Strict = 0`](../../src/foundation/core/Inno.Core.Serialization/PropertyRestoration/SerializationPropertyRestoreMode.cs#L11) | Stops at the first property that cannot be restored into the current object. | — | — |
| <a id="api-b2943ce188a12723"></a>[`public enum Inno.Core.Serialization.SerializationPropertyRestoreMode : int`](../../src/foundation/core/Inno.Core.Serialization/PropertyRestoration/SerializationPropertyRestoreMode.cs#L6) | Defines how independently captured serialized properties handle restore failures. | — | — |

### `Inno.Core.Serialization.SerializationPropertyRestoreResult`

<!-- api: T:Inno.Core.Serialization.SerializationPropertyRestoreResult f6a99c470fdfb6d71947fc0567d00401e349f9bd433e1968b7dcff66b680fb12 -->
<!-- api: P:Inno.Core.Serialization.SerializationPropertyRestoreResult.restoredCount afa7fd007114250d17a3365e172a0b34750052c8a3d3bef761dc8e188072e781 -->
<!-- api: P:Inno.Core.Serialization.SerializationPropertyRestoreResult.ignoredCount 433340c05d31eb4f58d3668748cbf4010c256f03868909eca3def4c9dcf837cd -->
<!-- api: P:Inno.Core.Serialization.SerializationPropertyRestoreResult.failures 5ca0b1b409cdd876df19b8b1b655e62cb6714fa4e174d55f79c50509912c5d50 -->
<!-- api: P:Inno.Core.Serialization.SerializationPropertyRestoreResult.success 880ae61d7667a2b844b189126e2abac8dc0391cf1d6e0e526923ee050a92d427 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4e72807249a46bb8"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Core.Serialization.SerializationPropertyRestoreFailure> Inno.Core.Serialization.SerializationPropertyRestoreResult.failures { get; }`](../../src/foundation/core/Inno.Core.Serialization/PropertyRestoration/SerializationPropertyRestoreResult.cs#L33) | Gets properties skipped because their previous data was incompatible. | — | — |
| <a id="api-b218eb1f93da0741"></a>[`public int Inno.Core.Serialization.SerializationPropertyRestoreResult.ignoredCount { get; }`](../../src/foundation/core/Inno.Core.Serialization/PropertyRestoration/SerializationPropertyRestoreResult.cs#L28) | Gets the number of removed or non-deserializable properties ignored by schema matching. | — | — |
| <a id="api-2701b40425df87e9"></a>[`public int Inno.Core.Serialization.SerializationPropertyRestoreResult.restoredCount { get; }`](../../src/foundation/core/Inno.Core.Serialization/PropertyRestoration/SerializationPropertyRestoreResult.cs#L23) | Gets the number of properties successfully restored. | — | — |
| <a id="api-b09d8c69cb6475bd"></a>[`public bool Inno.Core.Serialization.SerializationPropertyRestoreResult.success { get; }`](../../src/foundation/core/Inno.Core.Serialization/PropertyRestoration/SerializationPropertyRestoreResult.cs#L38) | Gets whether every matching property was restored successfully. | — | — |
| <a id="api-5af8a2ab5b6699b0"></a>[`public sealed class Inno.Core.Serialization.SerializationPropertyRestoreResult`](../../src/foundation/core/Inno.Core.Serialization/PropertyRestoration/SerializationPropertyRestoreResult.cs#L8) | Summarizes an independently captured property restore operation. | — | — |

### `Inno.Core.Serialization.SerializationPropertySnapshot`

<!-- api: T:Inno.Core.Serialization.SerializationPropertySnapshot 343f4d701fcffae6282b1af105d094929feb6220ce611577c93e0218aa83af60 -->
<!-- api: P:Inno.Core.Serialization.SerializationPropertySnapshot.name f249a9f256a9352dc7c714ef1550b48c97bf8a381137f597944bb496c6d2118f -->
<!-- api: P:Inno.Core.Serialization.SerializationPropertySnapshot.propertyType b1139504acfa6f60763ae8bc736166163cde3e7740ddbcdc8d98c130b02f10ba -->
<!-- api: P:Inno.Core.Serialization.SerializationPropertySnapshot.data bfc765dcc156759d69d5d41ef1c7d970bca52b89cd4c2860325651ae504c17c3 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-94a07b344cd52a84"></a>[`public System.ReadOnlyMemory<byte> Inno.Core.Serialization.SerializationPropertySnapshot.data { get; }`](../../src/foundation/core/Inno.Core.Serialization/PropertyRestoration/SerializationPropertySnapshot.cs#L35) | Gets the independently encoded property data. | — | — |
| <a id="api-1aea42f283eade2d"></a>[`public string Inno.Core.Serialization.SerializationPropertySnapshot.name { get; }`](../../src/foundation/core/Inno.Core.Serialization/PropertyRestoration/SerializationPropertySnapshot.cs#L25) | Gets the serialized member key. | — | — |
| <a id="api-b48bf524e5c776e5"></a>[`public System.Type Inno.Core.Serialization.SerializationPropertySnapshot.propertyType { get; }`](../../src/foundation/core/Inno.Core.Serialization/PropertyRestoration/SerializationPropertySnapshot.cs#L30) | Gets the declared property type used when the value was captured. | — | — |
| <a id="api-60310733297dccbd"></a>[`public sealed class Inno.Core.Serialization.SerializationPropertySnapshot`](../../src/foundation/core/Inno.Core.Serialization/PropertyRestoration/SerializationPropertySnapshot.cs#L8) | Stores one independently encoded serializable property and its original declared type. | — | — |

### `Inno.Core.Serialization.SerializationReader`

<!-- api: T:Inno.Core.Serialization.SerializationReader 361bc5302de36298fb93292040e719890473a0301cf6ee7db3684d9ef8e91317 -->
<!-- api: P:Inno.Core.Serialization.SerializationReader.context 1812af27543bc7f8b74a8616ca16ccc960dec60038b1a678f3f4d6227ba3e521 -->
<!-- api: P:Inno.Core.Serialization.SerializationReader.path 0511ba6d9d35e69be1f49b61fcd2c554872fe40c416de4e04948ff93d757901b -->
<!-- api: P:Inno.Core.Serialization.SerializationReader.valueType 59d91737c47c8128dabdcce67714229d7c14ff615abef053b0ea486ece21d5b7 -->
<!-- api: M:Inno.Core.Serialization.SerializationReader.Contains(System.String) 67143f1b1f67f6d7893434f90ad7ecd4edf301df3595ed2d859c5ab7e776f0d4 -->
<!-- api: M:Inno.Core.Serialization.SerializationReader.Read``1(System.String) 575a584fa4221bad178513877cbac17a0c60ce00c56135e84a739b51e10a4630 -->
<!-- api: M:Inno.Core.Serialization.SerializationReader.Read(System.String,System.Type) a1578d6b0bab32f4655490af5c328db46c2dd572942d2dfe11e13832a3b5d4ed -->
<!-- api: M:Inno.Core.Serialization.SerializationReader.TryRead``1(System.String,``0@) 9770746d31a0576714f8ec4bb208a7b9a2c05fccb6a85e8e3d0975c734cac62f -->
<!-- api: M:Inno.Core.Serialization.SerializationReader.ReadObject(System.String) 3241b791999c5503dd52d63d8af205ae140af706dc135489d4e32621d11ad99d -->
<!-- api: M:Inno.Core.Serialization.SerializationReader.ReadObjectArray(System.String) 1a927d2c6ea4579e42620077498855ce43bdeb3038083fbf8e307b0d77e7f775 -->
<!-- api: M:Inno.Core.Serialization.SerializationReader.RestoreProperties(Inno.Core.Serialization.ISerializable) f56a599810a50bba26c9d6f8d0e237ffd0fc7b913cbc3df964b0eb4e684733c2 -->
<!-- api: M:Inno.Core.Serialization.SerializationReader.OnCompleted(System.Action) ffc89cfbd89afe704f261fc6d117638109c238ea21ec410372f4f7a1543da15c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-44f5673d73b4a7c0"></a>[`public bool Inno.Core.Serialization.SerializationReader.Contains(string name)`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationReader.cs#L50) | Determines whether the current object contains a named member.<br>返回：true when the member exists; otherwise, false.<br>`name`：The member name. | — | — |
| <a id="api-cae79f2fe1378289"></a>[`public void Inno.Core.Serialization.SerializationReader.OnCompleted(System.Action callback)`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationReader.cs#L211) | Schedules a callback to run after the complete decode operation succeeds.<br>`callback`：The callback to schedule. | — | `System.ArgumentNullException`：Thrown when callback is null. |
| <a id="api-a4ed28034f0a50b4"></a>[`public object? Inno.Core.Serialization.SerializationReader.Read(string name, System.Type declaredType)`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationReader.cs#L94) | Reads a value using its declared metadata type without constructing a generic method at runtime.<br>返回：The restored value, including null when the declaration permits it; invalid or missing data throws.<br>`name`：The required non-empty member name.<br>`declaredType`：The exact declaration whose converter and value semantics apply. | — | `System.ArgumentNullException`：The declared type is null.<br>`System.ArgumentException`：The member name is empty.<br>`System.InvalidOperationException`：The operation has ended or the member is missing, incompatible or unsupported. |
| <a id="api-ae40a53d619aacc0"></a>[`public TValue Inno.Core.Serialization.SerializationReader.Read<TValue>(string name)`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationReader.cs#L71) | Reads a required named value through the unified value pipeline.<br>返回：The restored value.<br>`name`：The required member name. | — | `System.InvalidOperationException`：Thrown when the member is missing or invalid. |
| <a id="api-953de61426352dbb"></a>[`public Inno.Core.Serialization.SerializationReader Inno.Core.Serialization.SerializationReader.ReadObject(string name)`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationReader.cs#L152) | Reads a required named structured object.<br>返回：A reader for the child object.<br>`name`：The required member name. | — | — |
| <a id="api-8db22c5bd8977821"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Core.Serialization.SerializationReader> Inno.Core.Serialization.SerializationReader.ReadObjectArray(string name)`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationReader.cs#L169) | Reads a required ordered array of structured objects.<br>返回：Operation-scoped readers for the array elements.<br>`name`：The required member name. | — | — |
| <a id="api-049a7b82eaa7e923"></a>[`public void Inno.Core.Serialization.SerializationReader.RestoreProperties(Inno.Core.Serialization.ISerializable target)`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationReader.cs#L195) | Restores annotated properties from the current structured object into an existing object.<br>`target`：The target whose annotated properties should be restored. | — | `System.ArgumentNullException`：Thrown when target is null. |
| <a id="api-c2c450749c4a4d17"></a>[`public bool Inno.Core.Serialization.SerializationReader.TryRead<TValue>(string name, out TValue value)`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationReader.cs#L123) | Attempts to read a named value through the unified value pipeline.<br>返回：true when the member exists; otherwise, false.<br>`name`：The optional member name.<br>`value`：The restored value when the member exists. | — | — |
| <a id="api-5f46152c48d0d1c0"></a>[`public Inno.Core.Serialization.SerializationContext Inno.Core.Serialization.SerializationReader.context { get; }`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationReader.cs#L29) | Gets the immutable context supplied to the current operation. | — | — |
| <a id="api-8d9cc0f084a61c27"></a>[`public string Inno.Core.Serialization.SerializationReader.path { get; }`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationReader.cs#L34) | Gets the current diagnostic path. | — | — |
| <a id="api-0896435346b6f9ff"></a>[`public System.Type Inno.Core.Serialization.SerializationReader.valueType { get; }`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationReader.cs#L39) | Gets the declared value type represented by this reader. | — | — |
| <a id="api-71ec1b20deb103ef"></a>[`public sealed class Inno.Core.Serialization.SerializationReader`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationReader.cs#L9) | Reads a structured object during the active deserialization operation. | — | — |

### `Inno.Core.Serialization.SerializationRegistry`

<!-- api: T:Inno.Core.Serialization.SerializationRegistry 433a2d4ba10f94273f8208ce28534d14d1c73ae79fdab2330dc353d58cd3caa1 -->
<!-- api: M:Inno.Core.Serialization.SerializationRegistry.#ctor(Inno.Extensibility.Types.TypeCatalog,Inno.Core.Serialization.ISerializationMetadataSource) 34433cc82f6e9db0275b2907eb03004a9af9e0bcef87c5abb00c8bc163698b62 -->
<!-- api: M:Inno.Core.Serialization.SerializationRegistry.Dispose 82be1d5e66db58d177bca2c9e09ce02447dbbc35e80875d0be649b5f7c763841 -->
<!-- api: M:Inno.Core.Serialization.SerializationRegistry.CaptureGeneration 6c0e04353d711a0aaa5f0a26a7a8df262cd4bc60df68ce815916c95b040b7983 -->
<!-- api: M:Inno.Core.Serialization.SerializationRegistry.GetMetadata(System.Type) fbc2aebf4e6758e592610cd36ad0dbd32a2e77b22d0296785985f51f4b6e62cd -->
<!-- api: M:Inno.Core.Serialization.SerializationRegistry.GetProperties(Inno.Core.Serialization.ISerializable) a49d244777434eab95a479b023888899da5620f660a7b2e248bbfb420cc742ec -->
<!-- api: M:Inno.Core.Serialization.SerializationRegistry.CaptureProperties(Inno.Core.Serialization.ISerializable,Inno.Core.Serialization.SerializationContext) 723dc51881ca80211f61da62b78b20c259d0ac2337588cd0f7302aa49a64fb2a -->
<!-- api: M:Inno.Core.Serialization.SerializationRegistry.CapturePropertyData(Inno.Core.Serialization.ISerializable,System.String,Inno.Core.Serialization.SerializationContext) d5fcce53c7bc45eb1f56700ac7828d1c7695bd558710c2b58be85f874675a54e -->
<!-- api: M:Inno.Core.Serialization.SerializationRegistry.CapturePropertiesData(Inno.Core.Serialization.ISerializable,Inno.Core.Serialization.SerializationContext) d7f310aeba37dd09c7e47ce9745a39f2a964bf5c429cd254b88a90173ad03ad3 -->
<!-- api: M:Inno.Core.Serialization.SerializationRegistry.EncodePropertySnapshots(System.Collections.Generic.IReadOnlyList{Inno.Core.Serialization.SerializationPropertySnapshot}) af825ab10236befea0dedec4e1f73a34cb1939ad283687d19ebf9237f7ab0ded -->
<!-- api: M:Inno.Core.Serialization.SerializationRegistry.RestoreProperties(Inno.Core.Serialization.ISerializable,System.Collections.Generic.IReadOnlyList{Inno.Core.Serialization.SerializationPropertySnapshot},Inno.Core.Serialization.SerializationPropertyRestoreMode,Inno.Core.Serialization.SerializationContext) 7ace592b1430be9288e9c13f24b14e9e9a7d150db60d9fe18ff63decc7fb7e11 -->
<!-- api: M:Inno.Core.Serialization.SerializationRegistry.RestorePropertiesData(Inno.Core.Serialization.ISerializable,System.ReadOnlySpan{System.Byte},Inno.Core.Serialization.SerializationPropertyRestoreMode,Inno.Core.Serialization.SerializationContext) eaeb5320fac6b89a00748238307696991a39900d73201c22a22a75f0e74e6519 -->
<!-- api: M:Inno.Core.Serialization.SerializationRegistry.Serialize``1(``0,Inno.Core.Serialization.SerializationContext) 923f0d6202085e211df1c88722a514ed2bb7dc0aad5efcc2e8fdcead11b4b3ab -->
<!-- api: M:Inno.Core.Serialization.SerializationRegistry.Deserialize``1(System.ReadOnlySpan{System.Byte},Inno.Core.Serialization.SerializationContext) fff0417555696161363fa13692667258eb91d5f94f82c23418112813eb7ef205 -->
<!-- api: M:Inno.Core.Serialization.SerializationRegistry.Restore``1(``0,System.ReadOnlySpan{System.Byte},Inno.Core.Serialization.SerializationContext) 1d482e7f8cb6eb6b0eed6c5d2baaf3c34abe625768667e27f8dd86759e300a54 -->
<!-- api: M:Inno.Core.Serialization.SerializationRegistry.Encode(System.Action{Inno.Core.Serialization.SerializationWriter},Inno.Core.Serialization.SerializationContext) 09c46545af6383da77d5cf4770bbdbe82b0c2707485e24df37a0ba0c6d850a85 -->
<!-- api: M:Inno.Core.Serialization.SerializationRegistry.Decode``1(System.ReadOnlySpan{System.Byte},System.Func{Inno.Core.Serialization.SerializationReader,``0},Inno.Core.Serialization.SerializationContext) 7553cb1fe3aaaf07e8eae5a7b805664435cd986065e8da5d1b1ed496ae437e04 -->

<!-- api: M:Inno.Core.Serialization.SerializationRegistry.CaptureSnapshot``1(``0,Inno.Core.Serialization.SerializationContext,Inno.Core.Serialization.SerializationSnapshot) 3ce45c702c1e6199badf94793717a299a4e6970f7b7b6dbd691f53f226506eab -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-41da8fccf82a6cc0"></a>[`public Inno.Core.Serialization.SerializationRegistry.SerializationRegistry(Inno.Extensibility.Types.TypeCatalog types, Inno.Core.Serialization.ISerializationMetadataSource metadata)`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L32) | Creates a serialization registry derived from one isolated type catalog.<br>`types`：The type catalog that owns converter extension generations.<br>`metadata`：The declaration access provider selected by the same composition root. | — | `System.ArgumentNullException`：Thrown when types is null. |
| <a id="api-b3d26af74d8341e3"></a>[`public Inno.Core.Serialization.SerializationGeneration Inno.Core.Serialization.SerializationRegistry.CaptureGeneration()`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L65) | Captures the currently active converter generation for deterministic work that may cross asynchronous continuations.<br>返回：An immutable serialization generation owned by the caller. | — | `System.InvalidOperationException`：Thrown when serialization services have not been initialized. |
| <a id="api-7c907daa79a2dcd7"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Core.Serialization.SerializationPropertySnapshot> Inno.Core.Serialization.SerializationRegistry.CaptureProperties(Inno.Core.Serialization.ISerializable value, Inno.Core.Serialization.SerializationContext? context = null)`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L146) | Captures each persistent property independently for reload-safe state restoration.<br>返回：Ordered independent property snapshots.<br>`value`：The object whose serializable properties should be captured.<br>`context`：Optional immutable converter context. | — | `System.ArgumentNullException`：Thrown when value is null.<br>`System.InvalidOperationException`：Thrown when the manager is not initialized. |
| <a id="api-0e2037e47f8483d3"></a>[`public byte[] Inno.Core.Serialization.SerializationRegistry.CapturePropertiesData(Inno.Core.Serialization.ISerializable value, Inno.Core.Serialization.SerializationContext? context = null)`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L214) | Captures every persistent property as neutral bytes without serializing the owning object graph.<br>返回：Strictly validated bytes containing independently encoded property values.<br>`value`：The serializable object whose persistent properties should be captured.<br>`context`：Optional immutable converter context. | — | `System.ArgumentNullException`：Thrown when value is null.<br>`System.InvalidOperationException`：Thrown when the manager is not initialized. |
| <a id="api-40358d50872e0291"></a>[`public byte[] Inno.Core.Serialization.SerializationRegistry.CapturePropertyData(Inno.Core.Serialization.ISerializable value, string propertyName, Inno.Core.Serialization.SerializationContext? context = null)`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L180) | Captures one persistent property as independently restorable neutral bytes without reading unrelated members.<br>返回：Neutral bytes containing the property name and encoded value.<br>`value`：The serializable object containing the requested property.<br>`propertyName`：The exact serialized member key to capture.<br>`context`：Optional immutable converter context. | — | `System.ArgumentNullException`：Thrown when value is null.<br>`System.ArgumentException`：Thrown when propertyName is empty or does not identify a persistent property.<br>`System.InvalidOperationException`：Thrown when the manager is not initialized. |
| <a id="api-2116243245150f14"></a>[`public TResult Inno.Core.Serialization.SerializationRegistry.Decode<TResult>(System.ReadOnlySpan<byte> bytes, System.Func<Inno.Core.Serialization.SerializationReader, TResult> read, Inno.Core.Serialization.SerializationContext? context = null)`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L566) | Decodes a manually defined structured schema from deterministic binary data.<br>返回：The callback result after all completion callbacks succeed.<br>`bytes`：The encoded bytes.<br>`read`：The callback that reads the root object.<br>`context`：Optional immutable converter context. | — | `System.ArgumentNullException`：Thrown when read is null.<br>`System.InvalidOperationException`：Thrown when the manager is not initialized. |
| <a id="api-83d3ade5b12e0225"></a>[`public T Inno.Core.Serialization.SerializationRegistry.Deserialize<T>(System.ReadOnlySpan<byte> bytes, Inno.Core.Serialization.SerializationContext? context = null) where T : class, Inno.Core.Serialization.ISerializable`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L415) | Deserializes a new serializable root object from deterministic binary data.<br>返回：The restored object.<br>`bytes`：The encoded bytes.<br>`context`：Optional immutable converter context. | — | `System.InvalidOperationException`：Thrown when the manager is not initialized. |
| <a id="api-27023162af6ea6fe"></a>[`public void Inno.Core.Serialization.SerializationRegistry.Dispose()`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L45) | Releases every converter generation owned by this registry. | — | — |
| <a id="api-cc40affbc6e53c00"></a>[`public byte[] Inno.Core.Serialization.SerializationRegistry.Encode(System.Action<Inno.Core.Serialization.SerializationWriter> write, Inno.Core.Serialization.SerializationContext? context = null)`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L512) | Encodes a manually defined structured schema into deterministic binary data.<br>返回：The encoded bytes.<br>`write`：The callback that writes the root object.<br>`context`：Optional immutable converter context. | — | `System.ArgumentNullException`：Thrown when write is null.<br>`System.InvalidOperationException`：Thrown when the manager is not initialized. |
| <a id="api-e83961d064bb3214"></a>[`public byte[] Inno.Core.Serialization.SerializationRegistry.EncodePropertySnapshots(System.Collections.Generic.IReadOnlyList<Inno.Core.Serialization.SerializationPropertySnapshot> snapshots)`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L238) | Encodes an existing ordered set of independent property snapshots as neutral restoration bytes.<br>返回：Strictly validated bytes accepted by Inno.Core.Serialization.SerializationRegistry.RestorePropertiesData(Inno.Core.Serialization.ISerializable,System.ReadOnlySpan{System.Byte},Inno.Core.Serialization.SerializationPropertyRestoreMode,Inno.Core.Serialization.SerializationContext).<br>`snapshots`：Snapshots previously produced by Inno.Core.Serialization.SerializationRegistry.CaptureProperties(Inno.Core.Serialization.ISerializable,Inno.Core.Serialization.SerializationContext). | — | `System.ArgumentNullException`：Thrown when snapshots is null. |
| <a id="api-33601a9f8291cfe1"></a>[`public Inno.Core.Serialization.SerializationTypeMetadata Inno.Core.Serialization.SerializationRegistry.GetMetadata(System.Type type)`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L88) | Reads the current provider's declaration metadata for domain operations such as prefab overrides.<br>返回：Immutable declaration accessors owned by this generation; unsupported declarations throw. The caller must not retain the result across generation retirement.<br>`type`：The exact declaration owned by the current serialization generation. | — | `System.ArgumentNullException`：The declaration is null.<br>`System.ObjectDisposedException`：The registry has been disposed. |
| <a id="api-8f66ff9d7e0251f6"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Core.Serialization.SerializedProperty> Inno.Core.Serialization.SerializationRegistry.GetProperties(Inno.Core.Serialization.ISerializable value)`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L110) | Gets the stable ordered runtime-visible properties for a serializable object.<br>返回：The properties that permit runtime reads.<br>`value`：The object whose property metadata should be created. | — | `System.ArgumentNullException`：Thrown when value is null.<br>`System.InvalidOperationException`：Thrown when the manager is not initialized. |
| <a id="api-054a392c95c990f7"></a>[`public void Inno.Core.Serialization.SerializationRegistry.Restore<T>(T target, System.ReadOnlySpan<byte> bytes, Inno.Core.Serialization.SerializationContext? context = null) where T : class, Inno.Core.Serialization.ISerializable`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L470) | Restores deterministic binary data into an existing serializable root object.<br>`target`：The existing target object.<br>`bytes`：The encoded bytes.<br>`context`：Optional immutable converter context. | — | `System.ArgumentNullException`：Thrown when target is null.<br>`System.InvalidOperationException`：Thrown when the manager is not initialized. |
| <a id="api-17b43fa918914afc"></a>[`public Inno.Core.Serialization.SerializationPropertyRestoreResult Inno.Core.Serialization.SerializationRegistry.RestoreProperties(Inno.Core.Serialization.ISerializable target, System.Collections.Generic.IReadOnlyList<Inno.Core.Serialization.SerializationPropertySnapshot> snapshots, Inno.Core.Serialization.SerializationPropertyRestoreMode mode = Inno.Core.Serialization.SerializationPropertyRestoreMode.Strict, Inno.Core.Serialization.SerializationContext? context = null)`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L273) | Restores independently captured properties into an existing object.<br>返回：A summary containing restored, ignored, and skipped properties.<br>`target`：The object receiving compatible property values.<br>`snapshots`：The previously captured property snapshots.<br>`mode`：The failure policy for matching but incompatible properties.<br>`context`：Optional immutable converter context. | — | `System.ArgumentNullException`：Thrown when an argument is null.<br>`System.ArgumentOutOfRangeException`：Thrown when mode is unknown.<br>`System.InvalidOperationException`：Thrown when strict restoration or an object-level callback fails. |
| <a id="api-e8b1ff3827c59576"></a>[`public Inno.Core.Serialization.SerializationPropertyRestoreResult Inno.Core.Serialization.SerializationRegistry.RestorePropertiesData(Inno.Core.Serialization.ISerializable target, System.ReadOnlySpan<byte> data, Inno.Core.Serialization.SerializationPropertyRestoreMode mode = Inno.Core.Serialization.SerializationPropertyRestoreMode.Strict, Inno.Core.Serialization.SerializationContext? context = null)`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L324) | Restores one or more independently encoded persistent properties from neutral bytes.<br>返回：A summary containing restored, ignored, and skipped properties.<br>`target`：The existing object receiving the captured values.<br>`data`：Strict property snapshot bytes created by Inno.Core.Serialization.SerializationRegistry.CapturePropertyData(Inno.Core.Serialization.ISerializable,System.String,Inno.Core.Serialization.SerializationContext) or Inno.Core.Serialization.SerializationRegistry.CapturePropertiesData(Inno.Core.Serialization.ISerializable,Inno.Core.Serialization.SerializationContext).<br>`mode`：The failure policy for matching but incompatible current properties.<br>`context`：Optional immutable converter context. | — | `System.ArgumentNullException`：Thrown when target is null.<br>`System.ArgumentOutOfRangeException`：Thrown when mode is unknown.<br>`System.IO.InvalidDataException`：Thrown when data is malformed or unsupported.<br>`System.InvalidOperationException`：Thrown when strict restoration or an object-level callback fails. |
| <a id="api-9d0f261cec2f5d3f"></a>[`public byte[] Inno.Core.Serialization.SerializationRegistry.Serialize<T>(T value, Inno.Core.Serialization.SerializationContext? context = null) where T : class, Inno.Core.Serialization.ISerializable`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L364) | Serializes a complete serializable root object into deterministic binary data.<br>返回：The encoded bytes.<br>`value`：The root object to serialize.<br>`context`：Optional immutable converter context. | — | `System.ArgumentNullException`：Thrown when value is null.<br>`System.InvalidOperationException`：Thrown when the manager is not initialized. |
| <a id="api-532d59aed512a0ec"></a>[`public sealed class Inno.Core.Serialization.SerializationRegistry : System.IDisposable`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L12) | Owns one generation-aware converter registry and provides deterministic root serialization operations. | — | — |
| <a id="api-a5c1e5869b56e173"></a>[`public Inno.Core.Serialization.SerializationSnapshot Inno.Core.Serialization.SerializationRegistry.CaptureSnapshot<T>(T value, Inno.Core.Serialization.SerializationContext? context = null, Inno.Core.Serialization.SerializationSnapshot? previous = null) where T : class, Inno.Core.Serialization.ISerializable`](../../src/foundation/core/Inno.Core.Serialization/SerializationRegistry.cs#L395) | Captures complete serialized state while sharing unchanged neutral values with a previous capture.<br>The exact concrete root type, interpreted by the current converter generation.<br>`value`：The non-null root whose getters and converters are evaluated in full.<br>`context`：The complete owner context, including required reference resolvers.<br>`previous`：An optional immutable capture to reuse; it is never changed or retained by this registry.<br>返回：The previous instance when all captured values can be reused, otherwise a new immutable snapshot. Reference equality proves unchanged bytes; different instances may still encode identically. Ordinary serialization and snapshot encoding produce identical bytes. Capture failure publishes no snapshot.<br>Writers borrowed by a snapshot converter belong to its synchronous callback and must not be used after it returns. Every capture still evaluates getters and converters; this is not a mutation-notification cache.<br>System.ArgumentNullException：The source value is null.<br>System.ObjectDisposedException：The registry has been disposed.<br>System.InvalidOperationException：A converter, unsupported value or reference context prevents capture. | 操作内借用；快照仅含中立值。 | 失败不发布部分捕获。 |

### `Inno.Core.Serialization.SerializationSnapshot`

<!-- api: T:Inno.Core.Serialization.SerializationSnapshot 883dcbcf6107904ff950202670cec521ec54015a55c5c575d0b13d37f76d0b1c -->
<!-- api: M:Inno.Core.Serialization.SerializationSnapshot.ToArray a00c841730036584c0d4932feb4fa74d9d4ba0bd81b8ba3a094413eebbfc5ec1 -->
<!-- api: M:Inno.Core.Serialization.SerializationSnapshot.WriteTo(System.IO.Stream) 2ad80e7b3b82905c0947b3072c2fde0b65008063c5393f1f983063160b61cb52 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-839fb78ae8c26a3e"></a>[`public sealed class Inno.Core.Serialization.SerializationSnapshot`](../../src/foundation/core/Inno.Core.Serialization/SerializationSnapshot.cs#L14) | Holds an immutable, neutral capture of serialized values without retaining the source object or its generation.<br>Capture again through to reuse unchanged values. Readers may share a snapshot across threads. It owns only scalar values, strings and copied binary data, never source instances, metadata, converters or delegates. It is transient and is not a persistence format. | 操作内借用；快照仅含中立值。 | 失败不发布部分捕获。 |
| <a id="api-34c92f5b1e07bfd5"></a>[`public byte[] Inno.Core.Serialization.SerializationSnapshot.ToArray()`](../../src/foundation/core/Inno.Core.Serialization/SerializationSnapshot.cs#L24) | Encodes this capture using the same deterministic binary format as ordinary serialization.<br>返回：A new caller-owned byte array; changing it cannot modify this snapshot. | 操作内借用；快照仅含中立值。 | 失败不发布部分捕获。 |
| <a id="api-23e71b4b24a157b0"></a>[`public void Inno.Core.Serialization.SerializationSnapshot.WriteTo(System.IO.Stream destination)`](../../src/foundation/core/Inno.Core.Serialization/SerializationSnapshot.cs#L33) | Streams this capture in the ordinary deterministic binary format without allocating the complete output payload.<br>`destination`：The writable caller-owned stream, which remains open.<br>System.ArgumentNullException：The destination is null.<br>System.ArgumentException：The destination cannot be written.<br>System.IO.IOException：The destination rejects a write; its partial bytes remain the caller's responsibility. | 操作内借用；快照仅含中立值。 | 失败不发布部分捕获。 |

### `Inno.Core.Serialization.SerializationTypeMetadata`

<!-- api: T:Inno.Core.Serialization.SerializationTypeMetadata cf0a2ad5ab2e7a767949dadb442e3b2ede3d55225536f6e18cf2d782a5693cac -->
<!-- api: M:Inno.Core.Serialization.SerializationTypeMetadata.#ctor(System.Type,System.Collections.Generic.IReadOnlyList{Inno.Core.Serialization.SerializationMemberMetadata},System.Func{System.Object},System.Action{System.Object,Inno.Core.Serialization.SerializationContext},Inno.Core.Serialization.SerializationCollectionMetadata,System.Boolean,Inno.Core.Serialization.SerializationTypeMetadata) 790a9f2e14f0921f88db6eef1c618af8bbf9144c84152dff3fb39896346058e9 -->
<!-- api: P:Inno.Core.Serialization.SerializationTypeMetadata.type 9af26a12a9db4f9ae35845ad8e66bc8144a850c3c6fbf9b3e1a4ee13775a080b -->
<!-- api: P:Inno.Core.Serialization.SerializationTypeMetadata.members aa4bb144e3e068bed35d19dd6da8c65ebccbdc1995b374ccb75f455150290c98 -->
<!-- api: P:Inno.Core.Serialization.SerializationTypeMetadata.restored 7c1542d881f25587817d8f59eff17b54b2ee1fb00f15043311068fdb57a501f2 -->
<!-- api: P:Inno.Core.Serialization.SerializationTypeMetadata.collection 176332da5e6f8b8e26e5bc12641e2df27b8e6a33f4050154ec31f63cef227f4c -->
<!-- api: P:Inno.Core.Serialization.SerializationTypeMetadata.requiresConverter b0933ec2d98dd4a4b6245e599acc14de4455e744adab6aeda88d58405cba3185 -->
<!-- api: M:Inno.Core.Serialization.SerializationTypeMetadata.CreateInstance 86862e15977bc7f84d317f062f3d5153bb45d95d0c029c102e95143e16ab492b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7a427659b5f98683"></a>[`public Inno.Core.Serialization.SerializationTypeMetadata.SerializationTypeMetadata(System.Type type, System.Collections.Generic.IReadOnlyList<Inno.Core.Serialization.SerializationMemberMetadata> members, System.Func<object>? factory, System.Action<object, Inno.Core.Serialization.SerializationContext>? restored = null, Inno.Core.Serialization.SerializationCollectionMetadata? collection = null, bool requiresConverter = false, Inno.Core.Serialization.SerializationTypeMetadata? inherited = null)`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationTypeMetadata.cs#L41) | Copies complete declaration metadata before it participates in a serialization generation.<br>`type`：The exact closed declaration.<br>`members`：Ordered persistent and runtime members, without duplicate keys.<br>`factory`：A parameterless constructor, or null when an explicit converter owns construction.<br>`restored`：An ordered restoration callback, or null when the type declares no hooks.<br>`collection`：Optional sequence or map construction and enumeration metadata.<br>`requiresConverter`：Whether default object serialization must be rejected.<br>`inherited`：Optional metadata contributed by the owner of a base declaration. | — | `System.ArgumentException`：A member is null or a persistent key is duplicated. |
| <a id="api-18a4bb440a35330c"></a>[`public object Inno.Core.Serialization.SerializationTypeMetadata.CreateInstance()`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationTypeMetadata.cs#L107) | Creates a new instance through the declaration's selected constructor.<br>返回：A new caller-owned instance; a missing factory or failed constructor throws. | — | `System.InvalidOperationException`：No parameterless constructor is supplied. |
| <a id="api-a942d7ce44c4bbea"></a>[`public Inno.Core.Serialization.SerializationCollectionMetadata? Inno.Core.Serialization.SerializationTypeMetadata.collection { get; }`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationTypeMetadata.cs#L91) | Gets collection access metadata, or null for a scalar or object declaration. | — | — |
| <a id="api-e216f1b9fd5a8856"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Core.Serialization.SerializationMemberMetadata> Inno.Core.Serialization.SerializationTypeMetadata.members { get; }`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationTypeMetadata.cs#L81) | Gets ordered immutable member declarations. | — | — |
| <a id="api-09a82af69190b388"></a>[`public bool Inno.Core.Serialization.SerializationTypeMetadata.requiresConverter { get; }`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationTypeMetadata.cs#L96) | Gets whether default object serialization requires an explicit converter. | — | — |
| <a id="api-656ec679b4a72c5e"></a>[`public System.Action<object, Inno.Core.Serialization.SerializationContext>? Inno.Core.Serialization.SerializationTypeMetadata.restored { get; }`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationTypeMetadata.cs#L86) | Gets the generation-owned restoration callback, or null when no hooks are declared. | — | — |
| <a id="api-88e89d6ba6df602c"></a>[`public System.Type Inno.Core.Serialization.SerializationTypeMetadata.type { get; }`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationTypeMetadata.cs#L76) | Gets the exact closed declaration. | — | — |
| <a id="api-54f9b83dc96f194a"></a>[`public sealed class Inno.Core.Serialization.SerializationTypeMetadata`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializationTypeMetadata.cs#L10) | Freezes member access, construction and restoration callbacks for an exact closed declaration. | — | — |

### `Inno.Core.Serialization.SerializationWriter`

<!-- api: T:Inno.Core.Serialization.SerializationWriter 8ffe0a7db4d99f8cfbbcbc0c594613b6d5f82d9610e1c25339b7af1e9d97a76e -->
<!-- api: P:Inno.Core.Serialization.SerializationWriter.context b8f1929507dc3f2139b9e9538fa66912413e66f7a7de20e77a33cf5a9c8b9ee3 -->
<!-- api: P:Inno.Core.Serialization.SerializationWriter.path 32a51c93993f4afee3a9628898e7a194e5a887143bf4469463d34f7e7a5c1e29 -->
<!-- api: P:Inno.Core.Serialization.SerializationWriter.valueType 92d8e93c948abd7cca91c2d37bdb83f8202d54459a6aa18720899e6afffb5cfb -->
<!-- api: M:Inno.Core.Serialization.SerializationWriter.Write``1(System.String,``0) 0c61a07bec280ab01e7bba3e6fabd27fa81dead420b2055538776d0113d89145 -->
<!-- api: M:Inno.Core.Serialization.SerializationWriter.Write(System.String,System.Object,System.Type) 681857d4266171ff3409d70aa6d221f5302ef8cad1cfcb9c2a93fa12bc08c989 -->
<!-- api: M:Inno.Core.Serialization.SerializationWriter.WriteObject(System.String,System.Action{Inno.Core.Serialization.SerializationWriter}) 32a7ca695c18e8760348ad49c94c92a4804f060caf485451bc8cd0931fe19201 -->
<!-- api: M:Inno.Core.Serialization.SerializationWriter.WriteObjectArray``1(System.String,System.Collections.Generic.IEnumerable{``0},System.Action{Inno.Core.Serialization.SerializationWriter,``0}) adbe6fc9813f3d82fa2aa9275105e012efa1d10f687e5c259e1acfbd5b6261a6 -->
<!-- api: M:Inno.Core.Serialization.SerializationWriter.WriteProperties(Inno.Core.Serialization.ISerializable) 72541ebb0931fbb270821e0b936892fd218b76546b82769419e664926f620da9 -->

<!-- api: P:Inno.Core.Serialization.SerializationWriter.typeSnapshot 8b26d9032ad76550871d85a3a7dddb640a332fcb743e4c974622c4f98b7a28c6 -->
<!-- api: M:Inno.Core.Serialization.SerializationWriter.WritePropertiesData(System.String,Inno.Core.Serialization.ISerializable,Inno.Core.Serialization.SerializationContext) dad3986fa87b4a62b6d9b501d7bc0d862d92fd57064e401975dc13d54447eb0b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-aed44d097eab215e"></a>[`public void Inno.Core.Serialization.SerializationWriter.Write(string name, object? value, System.Type declaredType)`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationWriter.cs#L85) | Writes a value using its declared metadata type without constructing a generic method at runtime.<br>`name`：The unique non-empty member name.<br>`value`：The value to encode, including null when permitted by the declared type.<br>`declaredType`：The exact declaration whose converter and value semantics apply. | — | `System.ArgumentNullException`：The declared type is null.<br>`System.ArgumentException`：The member name is empty.<br>`System.InvalidOperationException`：The operation has ended, the name is duplicated, or the value is unsupported. |
| <a id="api-6324d9176a45e173"></a>[`public void Inno.Core.Serialization.SerializationWriter.Write<TValue>(string name, TValue value)`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationWriter.cs#L59) | Writes a named value through the unified value pipeline.<br>`name`：The unique non-empty member name.<br>`value`：The value to write. | — | `System.ArgumentException`：Thrown when name is empty.<br>`System.InvalidOperationException`：Thrown when the name is duplicated or the value is unsupported. |
| <a id="api-5cde4545c5255e8d"></a>[`public void Inno.Core.Serialization.SerializationWriter.WriteObject(string name, System.Action<Inno.Core.Serialization.SerializationWriter> write)`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationWriter.cs#L107) | Writes a named structured object.<br>`name`：The unique non-empty member name.<br>`write`：The callback that fills the child object. | — | `System.ArgumentNullException`：Thrown when write is null. |
| <a id="api-1b4d21b6f2d5b529"></a>[`public void Inno.Core.Serialization.SerializationWriter.WriteObjectArray<TValue>(string name, System.Collections.Generic.IEnumerable<TValue> values, System.Action<Inno.Core.Serialization.SerializationWriter, TValue> writeElement)`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationWriter.cs#L136) | Writes an ordered array of structured objects.<br>`name`：The unique non-empty member name.<br>`values`：The elements to write.<br>`writeElement`：The callback that fills each element object. | — | `System.ArgumentNullException`：Thrown when an argument is null. |
| <a id="api-7b8a863cc76b6a81"></a>[`public void Inno.Core.Serialization.SerializationWriter.WriteProperties(Inno.Core.Serialization.ISerializable value)`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationWriter.cs#L168) | Writes annotated properties from a serializable object into the current structured object.<br>`value`：The object whose annotated properties should be written. | — | `System.ArgumentNullException`：Thrown when value is null. |
| <a id="api-300839d749d9405c"></a>[`public Inno.Core.Serialization.SerializationContext Inno.Core.Serialization.SerializationWriter.context { get; }`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationWriter.cs#L29) | Gets the immutable context supplied to the current operation. | — | — |
| <a id="api-cc80ec5f8d58fc03"></a>[`public string Inno.Core.Serialization.SerializationWriter.path { get; }`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationWriter.cs#L34) | Gets the current diagnostic path. | — | — |
| <a id="api-afe33d543cdcbbc9"></a>[`public System.Type Inno.Core.Serialization.SerializationWriter.valueType { get; }`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationWriter.cs#L39) | Gets the declared value type represented by this writer. | — | — |
| <a id="api-9e8e30016feba2fd"></a>[`public sealed class Inno.Core.Serialization.SerializationWriter`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationWriter.cs#L9) | Writes a structured object during the active serialization operation. | — | — |
| <a id="api-fd613717a762d4ca"></a>[`public Inno.Extensibility.Types.TypeCacheSnapshot Inno.Core.Serialization.SerializationWriter.typeSnapshot { get; }`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationWriter.cs#L53) | Borrows the immutable type snapshot pinned by this operation's converter generation.<br>Use this snapshot for consistent type identities during a complete capture. It must not be retained beyond the writer's operation; keeping it would retain its collectible types after the converter lease is released.<br>System.InvalidOperationException：The writer no longer belongs to an active capture. | 操作内借用；快照仅含中立值。 | 失败不发布部分捕获。 |
| <a id="api-50a81629de922ddd"></a>[`public void Inno.Core.Serialization.SerializationWriter.WritePropertiesData(string name, Inno.Core.Serialization.ISerializable value, Inno.Core.Serialization.SerializationContext? context = null)`](../../src/foundation/core/Inno.Core.Serialization/IO/SerializationWriter.cs#L241) | Writes independently restorable property bytes through the current converter generation.<br>`name`：The unique member receiving the binary property snapshot.<br>`value`：The object whose persistent properties are captured once.<br>`context`：The complete owner context, or null to use this writer's context.<br>System.ArgumentNullException：The source object is null.<br>System.InvalidOperationException：The writer has ended, a key is duplicated, or capture fails.<br>System.ArgumentException：The member name is empty. | 操作内借用；快照仅含中立值。 | 失败不发布部分捕获。 |

### `Inno.Core.Serialization.SerializedIdentityRemapper`

<!-- api: T:Inno.Core.Serialization.SerializedIdentityRemapper a705d9e1cf6acde439d4ed1ae60d47596f6c12bcf1889cd0b4fd23e393f42833 -->
<!-- api: M:Inno.Core.Serialization.SerializedIdentityRemapper.Rewrite(System.ReadOnlySpan{System.Byte},System.Collections.Generic.IReadOnlyDictionary{System.Guid,System.Guid},System.Collections.Generic.IReadOnlyDictionary{System.String,System.String}) ab3ab2224970e740ae93f1d0e66523f5d1a69550c16ede7955c89bef617ad5b6 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-bfdfadfe0cd92669"></a>[`public static byte[] Inno.Core.Serialization.SerializedIdentityRemapper.Rewrite(System.ReadOnlySpan<byte> source, System.Collections.Generic.IReadOnlyDictionary<System.Guid, System.Guid> identities, System.Collections.Generic.IReadOnlyDictionary<string, string>? paths = null)`](../../src/foundation/core/Inno.Core.Serialization/SerializedIdentityRemapper.cs#L34) | Replaces exact Guid and string values while retaining the binary serialization structure.<br>返回：A detached rewritten payload; unknown binary subvalues remain unchanged.<br>`source`：A complete serialized value or property snapshot.<br>`identities`：Old to new Guid values.<br>`paths`：Optional exact old to new string values. | — | `System.IO.InvalidDataException`：The recognized serialized format is malformed. |
| <a id="api-ab5f74e47b8b9dbb"></a>[`public static class Inno.Core.Serialization.SerializedIdentityRemapper`](../../src/foundation/core/Inno.Core.Serialization/SerializedIdentityRemapper.cs#L11) | Rewrites selected scalar identities in a serialized value and its nested serialized payloads. | — | — |

### `Inno.Core.Serialization.SerializedProperty`

<!-- api: T:Inno.Core.Serialization.SerializedProperty 3d78d478beb981dd02e4f8ad9ea8a4d6c43b1b773cc001c96634d8ed5092f3db -->
<!-- api: P:Inno.Core.Serialization.SerializedProperty.name dd656a5f293afdd05d6df638aefd01643322db3eae1b2f79602ad22b604358a5 -->
<!-- api: P:Inno.Core.Serialization.SerializedProperty.propertyType 5bdf72940c6b84fbfeb94e1b37c927a18d89c489faef88036da8c5ad11924ff9 -->
<!-- api: P:Inno.Core.Serialization.SerializedProperty.visibility a1318b8cacdbc6e505080a6ea4cf7ce347d40e28932e2891679adb94de17a95d -->
<!-- api: P:Inno.Core.Serialization.SerializedProperty.canRead b02fa51e6e7252e20988e2020ad815cb0ba0f757b111986fe5f55e917ab16ef0 -->
<!-- api: P:Inno.Core.Serialization.SerializedProperty.canWrite 4bd64498031b934ecef95632a5cad628b18b446cbd8bed63ea103d1ffbe53e6a -->
<!-- api: M:Inno.Core.Serialization.SerializedProperty.GetValue b5d145a0b678ba11efa6f5c05abdb5af59863972fb24f924997f8fbc504e0fff -->
<!-- api: M:Inno.Core.Serialization.SerializedProperty.SetValue(System.Object) 2cbc3cbb370f0c07bf4451ead6e03a5d741d17724161b178c22b52604c3b208d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4d339a2966882cb4"></a>[`public object? Inno.Core.Serialization.SerializedProperty.GetValue()`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializedProperty.cs#L79) | Gets the current value.<br>返回：The value produced by the getter delegate. | — | `System.InvalidOperationException`：Thrown when runtime reads are not permitted. |
| <a id="api-76feabdb37c33904"></a>[`public void Inno.Core.Serialization.SerializedProperty.SetValue(object? value)`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializedProperty.cs#L95) | Sets the current value.<br>`value`：The value to assign. | — | `System.InvalidOperationException`：Thrown when runtime writes are not permitted. |
| <a id="api-5abd0081ffdcdbe5"></a>[`public bool Inno.Core.Serialization.SerializedProperty.canRead { get; }`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializedProperty.cs#L37) | Gets whether runtime callers may read this property. | — | — |
| <a id="api-ec0db583776c8fae"></a>[`public bool Inno.Core.Serialization.SerializedProperty.canWrite { get; }`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializedProperty.cs#L42) | Gets whether runtime callers may write this property. | — | — |
| <a id="api-12a07181f28482cf"></a>[`public string Inno.Core.Serialization.SerializedProperty.name { get; }`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializedProperty.cs#L22) | Gets the display and serialization key name. | — | — |
| <a id="api-b5983f7cb8e9be8b"></a>[`public System.Type Inno.Core.Serialization.SerializedProperty.propertyType { get; }`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializedProperty.cs#L27) | Gets the declared CLR type of this property. | — | — |
| <a id="api-2680032fc99015bb"></a>[`public Inno.Core.Serialization.PropertyVisibility Inno.Core.Serialization.SerializedProperty.visibility { get; }`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializedProperty.cs#L32) | Gets the visibility of this property. | — | — |
| <a id="api-a1acfec87581e0fc"></a>[`public sealed class Inno.Core.Serialization.SerializedProperty`](../../src/foundation/core/Inno.Core.Serialization/Metadata/SerializedProperty.cs#L8) | Represents a discoverable property in editor and persistence workflows. | — | — |

### `Inno.Core.Serialization.StaticSerializationMetadataSource`

<!-- api: T:Inno.Core.Serialization.StaticSerializationMetadataSource ba01c6ec0b159a1581970cc26c5f75f196b22835cfdb36b46deb65ca2c3c7242 -->
<!-- api: M:Inno.Core.Serialization.StaticSerializationMetadataSource.#ctor(System.Collections.Generic.IReadOnlyList{System.Action{System.Action{Inno.Core.Serialization.SerializationTypeMetadata}}}) 0d6f8350da8a220813466e8877cebc8cbab25d9ddad016d3399c4848b954e38c -->
<!-- api: M:Inno.Core.Serialization.StaticSerializationMetadataSource.GetMetadata(System.Type) 0ff57ea88adb6b36c761066ddd69bb1cb21c9da04b45164733ee4e0fcedcb92c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-67d98027decd9f44"></a>[`public Inno.Core.Serialization.StaticSerializationMetadataSource.StaticSerializationMetadataSource(System.Collections.Generic.IReadOnlyList<System.Action<System.Action<Inno.Core.Serialization.SerializationTypeMetadata>>> catalogs)`](../../src/foundation/core/Inno.Core.Serialization/Metadata/StaticSerializationMetadataSource.cs#L22) | Freezes generated declaration contributions supplied by the Player composition.<br>`catalogs`：Assembly-local registration methods; delegates are released after construction. | — | `System.ArgumentException`：A registration is null or two assemblies disagree about a closed shape. |
| <a id="api-a49866863c36222e"></a>[`public Inno.Core.Serialization.SerializationTypeMetadata Inno.Core.Serialization.StaticSerializationMetadataSource.GetMetadata(System.Type type)`](../../src/foundation/core/Inno.Core.Serialization/Metadata/StaticSerializationMetadataSource.cs#L33) | Resolves a complete serialization shape for the current declaration.<br>返回：Immutable access metadata; an unsupported declaration throws instead of probing another provider.<br>`type`：The exact closed declaration, including its generic arguments. | — | — |
| <a id="api-1fcf78be948e81d4"></a>[`public sealed class Inno.Core.Serialization.StaticSerializationMetadataSource : Inno.Core.Serialization.ISerializationMetadataSource`](../../src/foundation/core/Inno.Core.Serialization/Metadata/StaticSerializationMetadataSource.cs#L9) | Resolves exact serialization shapes from explicit generated contributions without reflection fallback. | — | — |

## 项目依赖

- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
