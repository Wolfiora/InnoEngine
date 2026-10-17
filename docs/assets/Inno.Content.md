# Inno.Content

[Assets 索引](README.md) · [Wiki 首页](../README.md) · [文件缓存 Adapter](../backends/FileSystem/Inno.Adapter.Content.FileSystem.md)

## 职责与边界

提供部署内容的逻辑身份、完整索引、严格 Pack 验证及只读租约。仅依赖 Foundation 的序列化契约；不引用 Assets、Scene、Runtime、Build 或具体平台。文件系统、HTTP 与内存来源通过相同 store 进入运行服务。

## 公开 API

| 类型与入口 | 当前语义 |
| --- | --- |
| `ContentKey(string)`、`value`、`ToString()` | 精确大小写的可移植逻辑路径；默认值未赋值。拒绝逃逸、宿主路径语法与歧义路径段 |
| `ContentEntry(key, length, contentHash)` | 不可变条目，包含长度与规范化 SHA-256 |
| `ContentPackDescriptor(contentHash, fileName)` | 完整 Pack 身份；文件名必须与哈希一致 |
| `ContentPackIndex(entries)`、`entries`、`totalLength`、`TryGetEntry(key, out entry)` | 冻结、排序的唯一条目集合；拒绝折叠冲突及目录／文件冲突 |
| `ContentPackIndex.C_INDEX_KEY` | 唯一保留的索引条目 `Content.index`；索引不包含自身 |
| `ContentPackIndexCodec.Encode(index, serialization)`／`Decode(data, serialization)` | 使用当前序列化 generation 写入、读取相同协议，无旧格式分支 |
| `ContentReadLimits(...)` | Pack、单条目、展开总量、索引与条目数的有限预算 |
| `ContentPackReader.Open(pack, descriptor, serialization, limits, cancellationToken)` | 接管可读、可定位流；验证整个 Pack 哈希、索引、Archive 集合及所有 payload |
| `IRuntimeContentStore.descriptor`、`index`、`Acquire(key)`、`Dispose()` | 只读来源；不公开物理路径 |
| `ContentReadLease(entry, openRead, release)`、`entry`、`OpenRead()`、`Dispose()` | Provider 交出租约 pin；每条读取流拥有独立 pin |
| `PackContentStore` | Reader 返回的 owned Archive store；公开面与 store 契约一致 |

参数名称及完整异常说明以源码英文 XML 为准。未知 key 报 `FileNotFoundException`，损坏和预算错误报 `InvalidDataException`，取消报 `OperationCanceledException`；已经关闭的 owner 拒绝新租约。

## 创建、使用与退出

1. 宿主取得部署 metadata 并校验 Pack 身份。
2. 捕获一个 `SerializationGeneration`，将 owned Pack stream 交给 Reader。
3. Reader 完成全部验证后返回 store；失败也关闭原 stream。
4. 运行消费者取得租约，通过 `OpenRead()` 读取，不取得路径。
5. 先退休运行服务，再关闭 store。已取得的租约和读取流仍可完成；最后一个 pin 释放 Archive。

Archive 共享定位在内部同步，每个读取流保留自己的逻辑位置。它不假设 `ZipArchive` 可并发访问，也不为每次读取复制全部 Pack。

```csharp
using System.IO;
using Inno.Content;
using Inno.Core.Serialization;

static int ReadFirstByte(
    Stream ownedPack,
    ContentPackDescriptor descriptor,
    SerializationGeneration serialization
) {
    using PackContentStore content = ContentPackReader.Open(ownedPack, descriptor, serialization);
    using ContentReadLease lease = content.Acquire(new ContentKey("Data/example.bin"));
    using Stream input = lease.OpenRead();
    return input.ReadByte();
}
```

## 测试与扩展

`tests/content/Inno.Content.Tests` 覆盖路径、索引、完整性、预算、并发游标与租约关闭顺序。新内容来源实现 `IRuntimeContentStore`，并确保独立读取 pin 与 immutable metadata；不改变 AssetDatabase 或 Player 的读取协议。原始 Pack、文件缓存和生成物的位置由各自宿主决定。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Content.ContentEntry`

<!-- api: T:Inno.Content.ContentEntry 10a54653ba16f56f75d410b039b542106e21ec1dcee9da49a9fc41931f41e691 -->
<!-- api: M:Inno.Content.ContentEntry.#ctor(Inno.Content.ContentKey,System.Int64,System.String) f0329ebb807dbd50ffe2748d3c73e817d6addd4a729295c2ed6e58504e633e39 -->
<!-- api: P:Inno.Content.ContentEntry.key 0a186cfce392465084f45a3f13e859c3836f79c32feb0cbde185236ee68623c9 -->
<!-- api: P:Inno.Content.ContentEntry.length e081f4b803fda884c5eb14c18ca5dbeaa47646f2330953dc7eaa415a1efa8aab -->
<!-- api: P:Inno.Content.ContentEntry.contentHash f1478211b51504c4d61d04f3d5ef6a6ebfa2c6ec9d0f17fe0277b49acc6f8f49 -->
<!-- api: M:Inno.Content.ContentEntry.ToString 000f7ac910cbf073a009d00a5dcd436d27ead4202301c8ac3c597d2113bb65a7 -->
<!-- api: M:Inno.Content.ContentEntry.op_Inequality(Inno.Content.ContentEntry,Inno.Content.ContentEntry) 8438864278cfc4cedf34dca2cec0ee0aa4585fbb5b0d5d87b1ecb7f7cddf1c15 -->
<!-- api: M:Inno.Content.ContentEntry.op_Equality(Inno.Content.ContentEntry,Inno.Content.ContentEntry) c54dcf1a3f491b6039e70e4be47db8fa4c9dc046b3b2b20698a3626fe968db5d -->
<!-- api: M:Inno.Content.ContentEntry.GetHashCode 73aa87e65f90064ac18ed66055ac77b9891149f86aaadf0c86926a617a0faafc -->
<!-- api: M:Inno.Content.ContentEntry.Equals(System.Object) 2e54e60a13a764fd257de3e5f8196339f9874ac1407b1f41c159ded4b088a750 -->
<!-- api: M:Inno.Content.ContentEntry.Equals(Inno.Content.ContentEntry) b39a605a45a7a76700dea96de75164518c3f3218fac54fa1a1e5670259eb6c7b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-efa69590cc799319"></a>[`public Inno.Content.ContentEntry.ContentEntry(Inno.Content.ContentKey key, long length, string contentHash)`](../../src/content/deployment/Inno.Content/ContentEntry.cs#L25) | Freezes a portable key, byte length, and SHA-256 identity.<br>`key`：The assigned logical content key.<br>`length`：The exact nonnegative decoded byte count.<br>`contentHash`：The hexadecimal SHA-256 of decoded bytes, normalized to uppercase. | — | `System.ArgumentException`：The key is unassigned or the hash is malformed. |
| <a id="api-0b73fc4466797440"></a>[`public bool Inno.Content.ContentEntry.Equals(Inno.Content.ContentEntry? other)`](../../src/content/deployment/Inno.Content/ContentEntry.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-f802a4dc83ad7d3a"></a>[`public override bool Inno.Content.ContentEntry.Equals(object? obj)`](../../src/content/deployment/Inno.Content/ContentEntry.cs#L8) | Determines whether the specified object is equal to the current object.<br>返回：true if the specified object is equal to the current object; otherwise, false.<br>`obj`：The object to compare with the current object. | — | — |
| <a id="api-e534d0722169859a"></a>[`public override int Inno.Content.ContentEntry.GetHashCode()`](../../src/content/deployment/Inno.Content/ContentEntry.cs#L8) | Serves as the default hash function.<br>返回：A hash code for the current object. | — | — |
| <a id="api-32ae1e71b440c5d6"></a>[`public static bool Inno.Content.ContentEntry.operator ==(Inno.Content.ContentEntry? left, Inno.Content.ContentEntry? right)`](../../src/content/deployment/Inno.Content/ContentEntry.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-02c9f1dd4535a869"></a>[`public static bool Inno.Content.ContentEntry.operator !=(Inno.Content.ContentEntry? left, Inno.Content.ContentEntry? right)`](../../src/content/deployment/Inno.Content/ContentEntry.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-f6192812f6cce0bf"></a>[`public override string Inno.Content.ContentEntry.ToString()`](../../src/content/deployment/Inno.Content/ContentEntry.cs#L8) | Returns a string that represents the current object.<br>返回：A string that represents the current object. | — | — |
| <a id="api-8a1c1ce588142e94"></a>[`public string Inno.Content.ContentEntry.contentHash { get; }`](../../src/content/deployment/Inno.Content/ContentEntry.cs#L51) | Gets the uppercase SHA-256 of decoded bytes. | — | — |
| <a id="api-627530ca8b0defde"></a>[`public Inno.Content.ContentKey Inno.Content.ContentEntry.key { get; }`](../../src/content/deployment/Inno.Content/ContentEntry.cs#L41) | Gets the portable entry identity. | — | — |
| <a id="api-ee999adab93c2814"></a>[`public long Inno.Content.ContentEntry.length { get; }`](../../src/content/deployment/Inno.Content/ContentEntry.cs#L46) | Gets the exact decoded byte count. | — | — |
| <a id="api-d7aeb77ac09608e2"></a>[`public sealed record Inno.Content.ContentEntry : System.IEquatable<Inno.Content.ContentEntry>`](../../src/content/deployment/Inno.Content/ContentEntry.cs#L8) | Describes the exact uncompressed bytes of one immutable content entry. | — | — |

### `Inno.Content.ContentKey`

<!-- api: T:Inno.Content.ContentKey b5281256626517c46582303ba312381c415b2fadb59273481e36e0063e37b16f -->
<!-- api: M:Inno.Content.ContentKey.#ctor(System.String) 2d3f72b384910e3050f3b62e280b4621854f8f41318c40e2760db8984d87fee0 -->
<!-- api: P:Inno.Content.ContentKey.value b420072ed712b457db56de043afa7dfcd96b744ad03e84ded613b55f116f13a1 -->
<!-- api: M:Inno.Content.ContentKey.ToString 8d21587b8dabd04cd3084261abf82b64bc884bb83534a63f6f8c4146dbda0334 -->
<!-- api: M:Inno.Content.ContentKey.op_Inequality(Inno.Content.ContentKey,Inno.Content.ContentKey) 73a9d60129a03d66e678282df1df1a4fd25777ee46e16c1483a3691d5641d173 -->
<!-- api: M:Inno.Content.ContentKey.op_Equality(Inno.Content.ContentKey,Inno.Content.ContentKey) 330bda2664a1224ef08d5a12b35ac7f8f655022b2d952c59705b20ca4488cac5 -->
<!-- api: M:Inno.Content.ContentKey.GetHashCode e318a298632687dbb4c3a2283776f38ea737bce277a13eeda557ce1f7a2e322f -->
<!-- api: M:Inno.Content.ContentKey.Equals(System.Object) e2378172afb16577bee517b064c7338f416405e4cccec14d8ad9ba8d12131bb2 -->
<!-- api: M:Inno.Content.ContentKey.Equals(Inno.Content.ContentKey) b084810f2092690a9c0c1b800ade3bd8b9ca4f94e78fb94a69368f8876b7d565 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3bebb0de27d03c24"></a>[`public Inno.Content.ContentKey.ContentKey(string value)`](../../src/content/deployment/Inno.Content/ContentKey.cs#L19) | Validates a logical key without interpreting it as a host filesystem path.<br>`value`：A nonempty relative key with no traversal, ambiguous segments, or platform-reserved names. | — | `System.ArgumentException`：The key cannot be represented consistently by supported content sources. |
| <a id="api-10a3386a12a3991f"></a>[`public bool Inno.Content.ContentKey.Equals(Inno.Content.ContentKey other)`](../../src/content/deployment/Inno.Content/ContentKey.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-a9b5263e2b2f9817"></a>[`public override bool Inno.Content.ContentKey.Equals(object obj)`](../../src/content/deployment/Inno.Content/ContentKey.cs#L8) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-24a69277d12f716c"></a>[`public override int Inno.Content.ContentKey.GetHashCode()`](../../src/content/deployment/Inno.Content/ContentKey.cs#L8) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-5c59f87ab4f8a113"></a>[`public static bool Inno.Content.ContentKey.operator ==(Inno.Content.ContentKey left, Inno.Content.ContentKey right)`](../../src/content/deployment/Inno.Content/ContentKey.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-89ac5d29dfe3f13f"></a>[`public static bool Inno.Content.ContentKey.operator !=(Inno.Content.ContentKey left, Inno.Content.ContentKey right)`](../../src/content/deployment/Inno.Content/ContentKey.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-684e70b1ea6ff9f9"></a>[`public override string Inno.Content.ContentKey.ToString()`](../../src/content/deployment/Inno.Content/ContentKey.cs#L42) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-2af027371085ee05"></a>[`public string? Inno.Content.ContentKey.value { get; }`](../../src/content/deployment/Inno.Content/ContentKey.cs#L39) | Gets the exact logical key; a default value is unassigned and cannot be read from a store. | — | — |
| <a id="api-e2a8347d1bec4d8a"></a>[`public readonly record struct Inno.Content.ContentKey : System.IEquatable<Inno.Content.ContentKey>`](../../src/content/deployment/Inno.Content/ContentKey.cs#L8) | Identifies one content entry with case-sensitive, portable, slash-separated segments. | — | — |

### `Inno.Content.ContentPackDescriptor`

<!-- api: T:Inno.Content.ContentPackDescriptor 90b9f2b1f3459deed3553f0d8c391595568b215d856f7d7572ab869899b392d7 -->
<!-- api: M:Inno.Content.ContentPackDescriptor.#ctor(System.String,System.String) 0513f92c0a46164687c4399a528e2382b4bcf92350506459a0749532c2fb9901 -->
<!-- api: P:Inno.Content.ContentPackDescriptor.contentHash e6243a13ca9ef1ea9fd638f7a7b43ac41a8734135e1b5e6e251e7d088accb864 -->
<!-- api: P:Inno.Content.ContentPackDescriptor.fileName f3baf9cfad39db493eec90936e5650df6754380ac05f2466ffac4d5f6dab1fa3 -->
<!-- api: M:Inno.Content.ContentPackDescriptor.ToString 08e509efa904976250b08730f1e1f8a88801dc117d15a6db0ba764fce601fe00 -->
<!-- api: M:Inno.Content.ContentPackDescriptor.op_Inequality(Inno.Content.ContentPackDescriptor,Inno.Content.ContentPackDescriptor) 41f98f7825c3e63e9918bb1b0093cc50f11b3e35fea456d5fb086418ed891ebe -->
<!-- api: M:Inno.Content.ContentPackDescriptor.op_Equality(Inno.Content.ContentPackDescriptor,Inno.Content.ContentPackDescriptor) 83f92eb91de424a7c11948ad5db5ccc616ca1637e7aff8946890785e8276f62c -->
<!-- api: M:Inno.Content.ContentPackDescriptor.GetHashCode a374407c638ce5d9714c40d1684d0637b2f9f4372119326063cd27327f9497e5 -->
<!-- api: M:Inno.Content.ContentPackDescriptor.Equals(System.Object) 4ee9b3dda9f9433236eebdf6f6c77d98569321586e52ae44c24cd3fd8e730463 -->
<!-- api: M:Inno.Content.ContentPackDescriptor.Equals(Inno.Content.ContentPackDescriptor) be9ac43459df6aad26623c0941675330e3830683f6e635bf594980dd574a2615 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-ebc1fd9b59136eb3"></a>[`public Inno.Content.ContentPackDescriptor.ContentPackDescriptor(string contentHash, string fileName)`](../../src/content/deployment/Inno.Content/ContentPackDescriptor.cs#L22) | Validates the complete pack identity and its single portable file name.<br>`contentHash`：The SHA-256 of the complete pack bytes.<br>`fileName`：The name content-{uppercase SHA-256}.pack relative to the source. | — | `System.ArgumentException`：The hash or file name does not identify one current-format pack. |
| <a id="api-e161167b9c0339f4"></a>[`public bool Inno.Content.ContentPackDescriptor.Equals(Inno.Content.ContentPackDescriptor? other)`](../../src/content/deployment/Inno.Content/ContentPackDescriptor.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-2b2b24d746c188b2"></a>[`public override bool Inno.Content.ContentPackDescriptor.Equals(object? obj)`](../../src/content/deployment/Inno.Content/ContentPackDescriptor.cs#L8) | Determines whether the specified object is equal to the current object.<br>返回：true if the specified object is equal to the current object; otherwise, false.<br>`obj`：The object to compare with the current object. | — | — |
| <a id="api-f0d6f20a15d22a1f"></a>[`public override int Inno.Content.ContentPackDescriptor.GetHashCode()`](../../src/content/deployment/Inno.Content/ContentPackDescriptor.cs#L8) | Serves as the default hash function.<br>返回：A hash code for the current object. | — | — |
| <a id="api-f541cba3cd3cf70c"></a>[`public static bool Inno.Content.ContentPackDescriptor.operator ==(Inno.Content.ContentPackDescriptor? left, Inno.Content.ContentPackDescriptor? right)`](../../src/content/deployment/Inno.Content/ContentPackDescriptor.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-6881108dab4b68dd"></a>[`public static bool Inno.Content.ContentPackDescriptor.operator !=(Inno.Content.ContentPackDescriptor? left, Inno.Content.ContentPackDescriptor? right)`](../../src/content/deployment/Inno.Content/ContentPackDescriptor.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-eb31b9aafb1d170c"></a>[`public override string Inno.Content.ContentPackDescriptor.ToString()`](../../src/content/deployment/Inno.Content/ContentPackDescriptor.cs#L8) | Returns a string that represents the current object.<br>返回：A string that represents the current object. | — | — |
| <a id="api-fa459c32a2e5e657"></a>[`public string Inno.Content.ContentPackDescriptor.contentHash { get; }`](../../src/content/deployment/Inno.Content/ContentPackDescriptor.cs#L35) | Gets the SHA-256 of all encoded bytes, including the index. | — | — |
| <a id="api-3d823a44bf7f9ee3"></a>[`public string Inno.Content.ContentPackDescriptor.fileName { get; }`](../../src/content/deployment/Inno.Content/ContentPackDescriptor.cs#L40) | Gets the validated single file name used by the selected content source. | — | — |
| <a id="api-aed87967c441d4fe"></a>[`public sealed record Inno.Content.ContentPackDescriptor : System.IEquatable<Inno.Content.ContentPackDescriptor>`](../../src/content/deployment/Inno.Content/ContentPackDescriptor.cs#L8) | Identifies the complete encoded pack independently of its physical source. | — | — |

### `Inno.Content.ContentPackIndex`

<!-- api: T:Inno.Content.ContentPackIndex 8c11deb22d14a6749e3cf7c748d55480cb58dc7469c001860ce823980a6440d1 -->
<!-- api: F:Inno.Content.ContentPackIndex.C_INDEX_KEY 154923d760c7879a8254a46bd5ddd7a65e872d73b290e5ae7718cfa305eef862 -->
<!-- api: M:Inno.Content.ContentPackIndex.#ctor(System.Collections.Generic.IEnumerable{Inno.Content.ContentEntry}) e4a68c7205b429cba906b7f340f9cc5c3b14d0287fb56b63f6f99b2eade93ad3 -->
<!-- api: P:Inno.Content.ContentPackIndex.entries 9f52e094f56d3ba64fbe8d2a6186cefc795556830b8f5babff4322e4484748ba -->
<!-- api: P:Inno.Content.ContentPackIndex.totalLength 4e67d152b3c21ebd38bdbec7d6a54636cb53dbdc0cb6e9988a300cb76a97ad40 -->
<!-- api: M:Inno.Content.ContentPackIndex.TryGetEntry(Inno.Content.ContentKey,Inno.Content.ContentEntry@) e12b6cd6867d8547c5e7a511e8e55e5c6b22a7ae89c8e13f474181e8ead93f09 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e387a236e7a481ac"></a>[`public const string Inno.Content.ContentPackIndex.C_INDEX_KEY = "Content.index"`](../../src/content/deployment/Inno.Content/ContentPackIndex.cs#L16) | Gets the reserved archive entry that carries this index and never indexes itself. | — | — |
| <a id="api-39e5a35e70481743"></a>[`public Inno.Content.ContentPackIndex.ContentPackIndex(System.Collections.Generic.IEnumerable<Inno.Content.ContentEntry> entries)`](../../src/content/deployment/Inno.Content/ContentPackIndex.cs#L29) | Copies and sorts a complete payload inventory.<br>`entries`：Immutable payload descriptions; ownership of the input enumeration remains with the caller. | — | `System.ArgumentException`：An entry is null, repeats a folded key, or uses the reserved index identity. |
| <a id="api-e05f0a006aa403ae"></a>[`public bool Inno.Content.ContentPackIndex.TryGetEntry(Inno.Content.ContentKey key, out Inno.Content.ContentEntry? entry)`](../../src/content/deployment/Inno.Content/ContentPackIndex.cs#L74) | Finds the metadata for an exact assigned key.<br>返回：True only when the exact key is present.<br>`key`：The case-sensitive logical identity.<br>`entry`：The immutable metadata when present; otherwise null. | — | — |
| <a id="api-26b5f208e1f1bd8a"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Content.ContentEntry> Inno.Content.ContentPackIndex.entries { get; }`](../../src/content/deployment/Inno.Content/ContentPackIndex.cs#L55) | Gets the immutable inventory in deterministic logical-key order. | — | — |
| <a id="api-d9533d3b8102dff8"></a>[`public long Inno.Content.ContentPackIndex.totalLength { get; }`](../../src/content/deployment/Inno.Content/ContentPackIndex.cs#L60) | Gets the checked sum of all decoded payload lengths, excluding the index. | — | — |
| <a id="api-e62fc7378b965840"></a>[`public sealed class Inno.Content.ContentPackIndex`](../../src/content/deployment/Inno.Content/ContentPackIndex.cs#L11) | Freezes the complete payload set and rejects ambiguous keys before publication. | — | — |

### `Inno.Content.ContentPackIndexCodec`

<!-- api: T:Inno.Content.ContentPackIndexCodec 9bad0704360149706a8c82b204cb16369cc4940f98fa0529225fc9d570e42731 -->
<!-- api: M:Inno.Content.ContentPackIndexCodec.Encode(Inno.Content.ContentPackIndex,Inno.Core.Serialization.SerializationGeneration) 1b7fb98b4b485467808aca70539a17612bb9cafd6285158692eec7880e5acede -->
<!-- api: M:Inno.Content.ContentPackIndexCodec.Decode(System.ReadOnlySpan{System.Byte},Inno.Core.Serialization.SerializationGeneration) cfc7ef407b34d8e33651d027fcfd3dd0334539e5fa58f0c9258ff6c2cdb0eee0 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-6226d3cc335c7019"></a>[`public static Inno.Content.ContentPackIndex Inno.Content.ContentPackIndexCodec.Decode(System.ReadOnlySpan<byte> data, Inno.Core.Serialization.SerializationGeneration serialization)`](../../src/content/deployment/Inno.Content/ContentPackIndexCodec.cs#L57) | Restores and validates one current-format payload inventory.<br>返回：A newly owned immutable inventory with no duplicate or ambiguous identities.<br>`data`：The complete structured index bytes.<br>`serialization`：The borrowed converter generation pinned by the enclosing preparation operation. | — | `System.IO.InvalidDataException`：The bytes or inventory are malformed; no partial index is returned. |
| <a id="api-5ac952013cf2b4dc"></a>[`public static byte[] Inno.Content.ContentPackIndexCodec.Encode(Inno.Content.ContentPackIndex index, Inno.Core.Serialization.SerializationGeneration serialization)`](../../src/content/deployment/Inno.Content/ContentPackIndexCodec.cs#L25) | Captures the entire index with the operation's pinned converter generation.<br>返回：Newly owned deterministic index bytes.<br>`index`：The validated immutable inventory.<br>`serialization`：The borrowed generation shared by the enclosing build transaction. | — | — |
| <a id="api-54ba251305b40c2d"></a>[`public static class Inno.Content.ContentPackIndexCodec`](../../src/content/deployment/Inno.Content/ContentPackIndexCodec.cs#L11) | Encodes the current immutable content inventory through the common serialization protocol. | — | — |

### `Inno.Content.ContentPackReader`

<!-- api: T:Inno.Content.ContentPackReader b43966ca05a36645f198e94be52390dc59532059e60d89be9784286e5796a085 -->
<!-- api: M:Inno.Content.ContentPackReader.Open(System.IO.Stream,Inno.Content.ContentPackDescriptor,Inno.Core.Serialization.SerializationGeneration,Inno.Content.ContentReadLimits,System.Threading.CancellationToken) bba2680063420e8648255f38a1da651b5ce9b6d0df66ddce62e7092510530d07 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-53193dd347f1bc6f"></a>[`public static Inno.Content.PackContentStore Inno.Content.ContentPackReader.Open(System.IO.Stream pack, Inno.Content.ContentPackDescriptor descriptor, Inno.Core.Serialization.SerializationGeneration serialization, Inno.Content.ContentReadLimits? limits = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/content/deployment/Inno.Content/ContentPackReader.cs#L44) | Takes ownership of a readable seekable pack and validates all bytes, entries, and budgets.<br>返回：A caller-owned store that retains the stream until every lease and stream has retired.<br>`pack`：The owned stream; callers must not access it after this call. Failure also disposes it.<br>`descriptor`：The expected complete-pack identity received through validated deployment metadata.<br>`serialization`：The borrowed converter generation used to decode the common content index.<br>`limits`：Explicit finite reading and expansion budgets; null selects the documented defaults.<br>`cancellationToken`：Cancels validation before a store is returned. | — | `System.IO.InvalidDataException`：The encoded hash, index, entry inventory, decoded bytes, or reading budgets are invalid.<br>`System.OperationCanceledException`：Validation was canceled; the input stream is disposed. |
| <a id="api-42db43b94f3374bf"></a>[`public static class Inno.Content.ContentPackReader`](../../src/content/deployment/Inno.Content/ContentPackReader.cs#L15) | Verifies complete encoded and decoded identities before transferring a pack into an immutable store. | — | — |

### `Inno.Content.ContentReadLease`

<!-- api: T:Inno.Content.ContentReadLease fe9e090b8afea57e765714afc643ee077f7aa5664afe6770a9ae4e521a4e960c -->
<!-- api: M:Inno.Content.ContentReadLease.#ctor(Inno.Content.ContentEntry,System.Func{System.IO.Stream},System.Action) 8453d222b0c9c1b10608646f6533ec0ea9076aea9bdd02fd6d216fbd9ff84e2b -->
<!-- api: P:Inno.Content.ContentReadLease.entry ca6a07cda65fe92b381a60ad26018f2b4af68edf49e476f51730464fe7d9f750 -->
<!-- api: M:Inno.Content.ContentReadLease.OpenRead 6a73493b485a55ddbb32ffb3a10f7b716fbfc936f05643950a9a64f6a9f64eab -->
<!-- api: M:Inno.Content.ContentReadLease.Dispose edb1a97eb6efbaebdebba350c9e9d8056543df53581d19db9cbf4cfaec7a42fa -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-eece6119e79cf0ea"></a>[`public Inno.Content.ContentReadLease.ContentReadLease(Inno.Content.ContentEntry entry, System.Func<System.IO.Stream> openRead, System.Action release)`](../../src/content/deployment/Inno.Content/ContentReadLease.cs#L27) | Transfers a provider pin and independent-stream factory into a caller-owned lease.<br>`entry`：Immutable metadata identifying the pinned bytes.<br>`openRead`：Opens a stream with its own provider pin; stream disposal must release that pin.<br>`release`：Releases this lease's provider pin exactly once, independently of any open stream. | — | — |
| <a id="api-7895de49b77328f0"></a>[`public void Inno.Content.ContentReadLease.Dispose()`](../../src/content/deployment/Inno.Content/ContentReadLease.cs#L73) | Releases this lease's pin once; existing streams retain their independent pins. | — | — |
| <a id="api-592bc595f7b2513a"></a>[`public System.IO.Stream Inno.Content.ContentReadLease.OpenRead()`](../../src/content/deployment/Inno.Content/ContentReadLease.cs#L54) | Opens a new independently pinned stream without transferring this lease's ownership.<br>返回：A caller-owned stream; disposing this lease or its store does not invalidate the stream. | — | `System.ObjectDisposedException`：This lease has already released its pin.<br>`System.InvalidOperationException`：The provider did not return a readable stream. |
| <a id="api-c15d15f149c5791a"></a>[`public Inno.Content.ContentEntry Inno.Content.ContentReadLease.entry { get; }`](../../src/content/deployment/Inno.Content/ContentReadLease.cs#L40) | Gets the immutable metadata, which does not retain a source or extension generation. | — | — |
| <a id="api-378dd2aaf3742146"></a>[`public sealed class Inno.Content.ContentReadLease : System.IDisposable`](../../src/content/deployment/Inno.Content/ContentReadLease.cs#L9) | Pins immutable entry bytes while independent streams obtain their own retirement protection. | — | — |

### `Inno.Content.ContentReadLimits`

<!-- api: T:Inno.Content.ContentReadLimits be5a535b483ee3494b514433e960c9eb0eafad3092dd5ab4e3c45f802ab7b34e -->
<!-- api: M:Inno.Content.ContentReadLimits.#ctor(System.Int64,System.Int64,System.Int64,System.Int32,System.Int32) 8b7bf604038398c18256d1d6ba7618a4863eb95dfeed891586892e520b934929 -->
<!-- api: P:Inno.Content.ContentReadLimits.packBytes b6dcbc00b1369acf1e340752442ae6585bb6e50d9c6e47c6f1f5b2e28873adc2 -->
<!-- api: P:Inno.Content.ContentReadLimits.entryBytes 3acc10af10a59908e32ed21eeec15fed9c87c1cd560b7cea2c4f0c7e870f5519 -->
<!-- api: P:Inno.Content.ContentReadLimits.totalBytes 439a176ca8d08c6e3254fa9d1669b716c578a24b0ab1c0efb18de79fb54be5b3 -->
<!-- api: P:Inno.Content.ContentReadLimits.indexBytes fe64beb536b264ee5b81dda4f82196b20e0d3a45953a17c8e8a1d23ee41c9989 -->
<!-- api: P:Inno.Content.ContentReadLimits.entryCount 4dd5394e601dc54ac636f30e42867900aac12e244bb641a2abe46c55a2143d61 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f66a9904df946838"></a>[`public Inno.Content.ContentReadLimits.ContentReadLimits(long packBytes = 2147483648, long entryBytes = 536870912, long totalBytes = 8589934592, int indexBytes = 33554432, int entryCount = 100000)`](../../src/content/deployment/Inno.Content/ContentReadLimits.cs#L28) | Creates explicit positive budgets for one content preparation operation.<br>`packBytes`：The maximum encoded pack length.<br>`entryBytes`：The maximum decoded length of one payload.<br>`totalBytes`：The maximum sum of all decoded payload lengths.<br>`indexBytes`：The maximum decoded inventory document length.<br>`entryCount`：The maximum number of payload entries. | — | — |
| <a id="api-6aab874fea1bcb8f"></a>[`public long Inno.Content.ContentReadLimits.entryBytes { get; }`](../../src/content/deployment/Inno.Content/ContentReadLimits.cs#L55) | Gets the decoded single-payload budget. | — | — |
| <a id="api-e86c2a7f90bc9197"></a>[`public int Inno.Content.ContentReadLimits.entryCount { get; }`](../../src/content/deployment/Inno.Content/ContentReadLimits.cs#L70) | Gets the payload count budget. | — | — |
| <a id="api-e690386a97bb2ecf"></a>[`public int Inno.Content.ContentReadLimits.indexBytes { get; }`](../../src/content/deployment/Inno.Content/ContentReadLimits.cs#L65) | Gets the decoded index budget. | — | — |
| <a id="api-4058995bb47541c7"></a>[`public long Inno.Content.ContentReadLimits.packBytes { get; }`](../../src/content/deployment/Inno.Content/ContentReadLimits.cs#L50) | Gets the encoded pack budget. | — | — |
| <a id="api-01792d52499b36b0"></a>[`public long Inno.Content.ContentReadLimits.totalBytes { get; }`](../../src/content/deployment/Inno.Content/ContentReadLimits.cs#L60) | Gets the decoded total payload budget. | — | — |
| <a id="api-ac50138c4fb60523"></a>[`public sealed class Inno.Content.ContentReadLimits`](../../src/content/deployment/Inno.Content/ContentReadLimits.cs#L8) | Bounds encoded packs, decoded entries, inventory size, and total expansion before any content is accepted. | — | — |

### `Inno.Content.IRuntimeContentStore`

<!-- api: T:Inno.Content.IRuntimeContentStore cb4c1d8d56d5cfa7b6bdf9c874bf1d7e0feb6e29b620b44418b655b06a61345a -->
<!-- api: P:Inno.Content.IRuntimeContentStore.descriptor 21315c3f15d05b49c3fcfd303ff5c570a53e547e5cfdcbbc2556b092c68581bc -->
<!-- api: P:Inno.Content.IRuntimeContentStore.index 84c3dc21a712481e8f7e415622dd4eb30dd00c9f439ce79b4106a3a822606956 -->
<!-- api: M:Inno.Content.IRuntimeContentStore.Acquire(Inno.Content.ContentKey) f6c9e24161da3be621d2a2161f1d2832a91a7126981b380f1c4fea54b7025254 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f0088b3a3100231f"></a>[`Inno.Content.ContentReadLease Inno.Content.IRuntimeContentStore.Acquire(Inno.Content.ContentKey key)`](../../src/content/deployment/Inno.Content/IRuntimeContentStore.cs#L35) | Pins an entry against source retirement and supplies independent read streams.<br>返回：A caller-owned lease whose streams remain usable after this store closes.<br>`key`：The exact assigned logical key. | — | `System.IO.FileNotFoundException`：The exact content key is not present.<br>`System.ObjectDisposedException`：The store has closed and cannot issue new leases. |
| <a id="api-cbd5b32e92f438e2"></a>[`Inno.Content.ContentPackDescriptor Inno.Content.IRuntimeContentStore.descriptor { get; }`](../../src/content/deployment/Inno.Content/IRuntimeContentStore.cs#L13) | Gets the immutable complete-pack identity. | — | — |
| <a id="api-52a476edb19d4b4a"></a>[`Inno.Content.ContentPackIndex Inno.Content.IRuntimeContentStore.index { get; }`](../../src/content/deployment/Inno.Content/IRuntimeContentStore.cs#L18) | Gets the immutable complete payload inventory. | — | — |
| <a id="api-cc18454e1f82346a"></a>[`public interface Inno.Content.IRuntimeContentStore : System.IDisposable`](../../src/content/deployment/Inno.Content/IRuntimeContentStore.cs#L8) | Provides immutable content leases without exposing deployment paths or writable authoring sources. | — | — |

### `Inno.Content.PackContentStore`

<!-- api: T:Inno.Content.PackContentStore cc5cb85700808a0dda4a0671194a809086ba22de03a0b7d84c71578340f7ff40 -->
<!-- api: P:Inno.Content.PackContentStore.descriptor db474a8d5db1537816a36df6be570bccb17ae02e9a7dadb438d509d917b68366 -->
<!-- api: P:Inno.Content.PackContentStore.index 3e21ea281b80f014517d8c3aa5a82902fa8655343ada94977a1f34222e34126c -->
<!-- api: M:Inno.Content.PackContentStore.Acquire(Inno.Content.ContentKey) 002290c71aa02755c5c01b86c4644a6d680cf91f0f3d8b1a81030c00bc5d407a -->
<!-- api: M:Inno.Content.PackContentStore.Dispose 27663953b7b44c5bdb47a7b327db749c1bfc35b3d99548d99630ab7dca43dfb3 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b4a41c6ef7bcff4a"></a>[`public Inno.Content.ContentReadLease Inno.Content.PackContentStore.Acquire(Inno.Content.ContentKey key)`](../../src/content/deployment/Inno.Content/PackContentStore.cs#L38) | Pins an entry against source retirement and supplies independent read streams.<br>返回：A caller-owned lease whose streams remain usable after this store closes.<br>`key`：The exact assigned logical key. | — | `System.IO.FileNotFoundException`：The exact content key is not present.<br>`System.ObjectDisposedException`：The store has closed and cannot issue new leases. |
| <a id="api-8506599be2c32717"></a>[`public void Inno.Content.PackContentStore.Dispose()`](../../src/content/deployment/Inno.Content/PackContentStore.cs#L53) | Stops issuing leases and closes the archive after every existing lease and stream releases its pin. | — | — |
| <a id="api-9ecaecbd2b816783"></a>[`public Inno.Content.ContentPackDescriptor Inno.Content.PackContentStore.descriptor { get; }`](../../src/content/deployment/Inno.Content/PackContentStore.cs#L32) | Gets the immutable complete-pack identity. | — | — |
| <a id="api-7a46dbbaf8c261d7"></a>[`public Inno.Content.ContentPackIndex Inno.Content.PackContentStore.index { get; }`](../../src/content/deployment/Inno.Content/PackContentStore.cs#L35) | Gets the immutable complete payload inventory. | — | — |
| <a id="api-bcb5e05313840dd6"></a>[`public sealed class Inno.Content.PackContentStore : Inno.Content.IRuntimeContentStore`](../../src/content/deployment/Inno.Content/PackContentStore.cs#L11) | Owns one validated archive with independent entry cursors and synchronized shared-stream positioning. | — | — |

## 项目依赖

- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
