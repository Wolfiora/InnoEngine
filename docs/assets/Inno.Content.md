# Inno.Content

[Assets 索引](README.md) · [Wiki 首页](../README.md) · [文件缓存 Adapter](Inno.Adapter.Content.FileSystem.md)

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

以下仅列出当前程序集自己声明的 public/protected 契约；继承成员遵循所属基类页面。internal/private 实现不作为稳定公开 API。签名依据当前源码语义模型生成，行为、参数、异常与所有权说明同时以对应英文 XML 为准。

### `Inno.Content.ContentEntry`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Content.ContentEntry.ContentEntry(Inno.Content.ContentKey key, long length, string contentHash)`](../../src/content/deployment/Inno.Content/ContentEntry.cs#L25) | Freezes a portable key, byte length, and SHA-256 identity. |
| [`string Inno.Content.ContentEntry.contentHash`](../../src/content/deployment/Inno.Content/ContentEntry.cs#L51) | Gets the uppercase SHA-256 of decoded bytes. |
| [`Inno.Content.ContentKey Inno.Content.ContentEntry.key`](../../src/content/deployment/Inno.Content/ContentEntry.cs#L41) | Gets the portable entry identity. |
| [`long Inno.Content.ContentEntry.length`](../../src/content/deployment/Inno.Content/ContentEntry.cs#L46) | Gets the exact decoded byte count. |
| [`Inno.Content.ContentEntry`](../../src/content/deployment/Inno.Content/ContentEntry.cs#L8) | Describes the exact uncompressed bytes of one immutable content entry. |

### `Inno.Content.ContentKey`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Content.ContentKey.ContentKey(string value)`](../../src/content/deployment/Inno.Content/ContentKey.cs#L19) | Validates a logical key without interpreting it as a host filesystem path. |
| [`override string Inno.Content.ContentKey.ToString()`](../../src/content/deployment/Inno.Content/ContentKey.cs#L42) | See the implemented contract. |
| [`string? Inno.Content.ContentKey.value`](../../src/content/deployment/Inno.Content/ContentKey.cs#L39) | Gets the exact logical key; a default value is unassigned and cannot be read from a store. |
| [`Inno.Content.ContentKey`](../../src/content/deployment/Inno.Content/ContentKey.cs#L8) | Identifies one content entry with case-sensitive, portable, slash-separated segments. |

### `Inno.Content.ContentPackDescriptor`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Content.ContentPackDescriptor.ContentPackDescriptor(string contentHash, string fileName)`](../../src/content/deployment/Inno.Content/ContentPackDescriptor.cs#L22) | Validates the complete pack identity and its single portable file name. |
| [`string Inno.Content.ContentPackDescriptor.contentHash`](../../src/content/deployment/Inno.Content/ContentPackDescriptor.cs#L35) | Gets the SHA-256 of all encoded bytes, including the index. |
| [`string Inno.Content.ContentPackDescriptor.fileName`](../../src/content/deployment/Inno.Content/ContentPackDescriptor.cs#L40) | Gets the validated single file name used by the selected content source. |
| [`Inno.Content.ContentPackDescriptor`](../../src/content/deployment/Inno.Content/ContentPackDescriptor.cs#L8) | Identifies the complete encoded pack independently of its physical source. |

### `Inno.Content.ContentPackIndex`

| 当前声明 | 行为 |
| --- | --- |
| [`const string Inno.Content.ContentPackIndex.C_INDEX_KEY`](../../src/content/deployment/Inno.Content/ContentPackIndex.cs#L16) | Gets the reserved archive entry that carries this index and never indexes itself. |
| [`Inno.Content.ContentPackIndex.ContentPackIndex(System.Collections.Generic.IEnumerable<Inno.Content.ContentEntry> entries)`](../../src/content/deployment/Inno.Content/ContentPackIndex.cs#L29) | Copies and sorts a complete payload inventory. |
| [`bool Inno.Content.ContentPackIndex.TryGetEntry(Inno.Content.ContentKey key, out Inno.Content.ContentEntry? entry)`](../../src/content/deployment/Inno.Content/ContentPackIndex.cs#L74) | Finds the metadata for an exact assigned key. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Content.ContentEntry> Inno.Content.ContentPackIndex.entries`](../../src/content/deployment/Inno.Content/ContentPackIndex.cs#L55) | Gets the immutable inventory in deterministic logical-key order. |
| [`long Inno.Content.ContentPackIndex.totalLength`](../../src/content/deployment/Inno.Content/ContentPackIndex.cs#L60) | Gets the checked sum of all decoded payload lengths, excluding the index. |
| [`Inno.Content.ContentPackIndex`](../../src/content/deployment/Inno.Content/ContentPackIndex.cs#L11) | Freezes the complete payload set and rejects ambiguous keys before publication. |

### `Inno.Content.ContentPackIndexCodec`

| 当前声明 | 行为 |
| --- | --- |
| [`static Inno.Content.ContentPackIndex Inno.Content.ContentPackIndexCodec.Decode(System.ReadOnlySpan<byte> data, Inno.Core.Serialization.SerializationGeneration serialization)`](../../src/content/deployment/Inno.Content/ContentPackIndexCodec.cs#L57) | Restores and validates one current-format payload inventory. |
| [`static byte[] Inno.Content.ContentPackIndexCodec.Encode(Inno.Content.ContentPackIndex index, Inno.Core.Serialization.SerializationGeneration serialization)`](../../src/content/deployment/Inno.Content/ContentPackIndexCodec.cs#L25) | Captures the entire index with the operation's pinned converter generation. |
| [`Inno.Content.ContentPackIndexCodec`](../../src/content/deployment/Inno.Content/ContentPackIndexCodec.cs#L11) | Encodes the current immutable content inventory through the common serialization protocol. |

### `Inno.Content.ContentPackReader`

| 当前声明 | 行为 |
| --- | --- |
| [`static Inno.Content.PackContentStore Inno.Content.ContentPackReader.Open(System.IO.Stream pack, Inno.Content.ContentPackDescriptor descriptor, Inno.Core.Serialization.SerializationGeneration serialization, Inno.Content.ContentReadLimits? limits = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/content/deployment/Inno.Content/ContentPackReader.cs#L44) | Takes ownership of a readable seekable pack and validates all bytes, entries, and budgets. |
| [`Inno.Content.ContentPackReader`](../../src/content/deployment/Inno.Content/ContentPackReader.cs#L15) | Verifies complete encoded and decoded identities before transferring a pack into an immutable store. |

### `Inno.Content.ContentReadLease`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Content.ContentReadLease.ContentReadLease(Inno.Content.ContentEntry entry, System.Func<System.IO.Stream> openRead, System.Action release)`](../../src/content/deployment/Inno.Content/ContentReadLease.cs#L27) | Transfers a provider pin and independent-stream factory into a caller-owned lease. |
| [`void Inno.Content.ContentReadLease.Dispose()`](../../src/content/deployment/Inno.Content/ContentReadLease.cs#L73) | Releases this lease's pin once; existing streams retain their independent pins. |
| [`System.IO.Stream Inno.Content.ContentReadLease.OpenRead()`](../../src/content/deployment/Inno.Content/ContentReadLease.cs#L54) | Opens a new independently pinned stream without transferring this lease's ownership. |
| [`Inno.Content.ContentEntry Inno.Content.ContentReadLease.entry`](../../src/content/deployment/Inno.Content/ContentReadLease.cs#L40) | Gets the immutable metadata, which does not retain a source or extension generation. |
| [`Inno.Content.ContentReadLease`](../../src/content/deployment/Inno.Content/ContentReadLease.cs#L9) | Pins immutable entry bytes while independent streams obtain their own retirement protection. |

### `Inno.Content.ContentReadLimits`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Content.ContentReadLimits.ContentReadLimits(long packBytes = 2147483648, long entryBytes = 536870912, long totalBytes = 8589934592, int indexBytes = 33554432, int entryCount = 100000)`](../../src/content/deployment/Inno.Content/ContentReadLimits.cs#L28) | Creates explicit positive budgets for one content preparation operation. |
| [`long Inno.Content.ContentReadLimits.entryBytes`](../../src/content/deployment/Inno.Content/ContentReadLimits.cs#L55) | Gets the decoded single-payload budget. |
| [`int Inno.Content.ContentReadLimits.entryCount`](../../src/content/deployment/Inno.Content/ContentReadLimits.cs#L70) | Gets the payload count budget. |
| [`int Inno.Content.ContentReadLimits.indexBytes`](../../src/content/deployment/Inno.Content/ContentReadLimits.cs#L65) | Gets the decoded index budget. |
| [`long Inno.Content.ContentReadLimits.packBytes`](../../src/content/deployment/Inno.Content/ContentReadLimits.cs#L50) | Gets the encoded pack budget. |
| [`long Inno.Content.ContentReadLimits.totalBytes`](../../src/content/deployment/Inno.Content/ContentReadLimits.cs#L60) | Gets the decoded total payload budget. |
| [`Inno.Content.ContentReadLimits`](../../src/content/deployment/Inno.Content/ContentReadLimits.cs#L8) | Bounds encoded packs, decoded entries, inventory size, and total expansion before any content is accepted. |

### `Inno.Content.IRuntimeContentStore`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Content.ContentReadLease Inno.Content.IRuntimeContentStore.Acquire(Inno.Content.ContentKey key)`](../../src/content/deployment/Inno.Content/IRuntimeContentStore.cs#L35) | Pins an entry against source retirement and supplies independent read streams. |
| [`Inno.Content.ContentPackDescriptor Inno.Content.IRuntimeContentStore.descriptor`](../../src/content/deployment/Inno.Content/IRuntimeContentStore.cs#L13) | Gets the immutable complete-pack identity. |
| [`Inno.Content.ContentPackIndex Inno.Content.IRuntimeContentStore.index`](../../src/content/deployment/Inno.Content/IRuntimeContentStore.cs#L18) | Gets the immutable complete payload inventory. |
| [`Inno.Content.IRuntimeContentStore`](../../src/content/deployment/Inno.Content/IRuntimeContentStore.cs#L8) | Provides immutable content leases without exposing deployment paths or writable authoring sources. |

### `Inno.Content.PackContentStore`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Content.ContentReadLease Inno.Content.PackContentStore.Acquire(Inno.Content.ContentKey key)`](../../src/content/deployment/Inno.Content/PackContentStore.cs#L38) | See the implemented contract. |
| [`void Inno.Content.PackContentStore.Dispose()`](../../src/content/deployment/Inno.Content/PackContentStore.cs#L53) | Stops issuing leases and closes the archive after every existing lease and stream releases its pin. |
| [`Inno.Content.ContentPackDescriptor Inno.Content.PackContentStore.descriptor`](../../src/content/deployment/Inno.Content/PackContentStore.cs#L32) | See the implemented contract. |
| [`Inno.Content.ContentPackIndex Inno.Content.PackContentStore.index`](../../src/content/deployment/Inno.Content/PackContentStore.cs#L35) | See the implemented contract. |
| [`Inno.Content.PackContentStore`](../../src/content/deployment/Inno.Content/PackContentStore.cs#L11) | Owns one validated archive with independent entry cursors and synchronized shared-stream positioning. |

## 项目依赖

- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：项目引用；公开签名可见性由语义边界检查确认。

共同 MSBuild 注入的 analyzer 与编译规则属于构建依赖，完整有效项目图记录在本轮验收证据中。
