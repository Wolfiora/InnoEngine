# Inno.Core.IO

[分类索引](README.md) · [Wiki 首页](../README.md) · [本轮整改计划](../architecture/ARCHITECTURE_CLEANUP_PLAN_2026_10_06.md)

## 职责与边界

提供原子文件/目录发布、明确文件 lease、路径边界、小型 byte document 与独立 stream pin。领域模型通过文档或内容契约读取，不把物理位置泄漏给共享 Runtime。

## 所有权与失败

`FileLease` 支持共享读取与独占写入协调；Windows 与 Unix 遵循同一语义。锁文件保留，不能删除锁文件代替解锁。等待受取消与有界 timeout 控制。

`IByteDocumentStore` 仅用于小型文档。`FileByteDocumentStore` 复用 AtomicFile；`ReadOnlyByteDocumentStore` 捕获 owned bytes，写入明确失败。它不是 Asset catalog 或事件协议。

`AtomicDirectory.Publish` 发布完整 staging，不覆盖活跃 reader 所持 generation。`OwnedReadStream` 独立持有读取 pin，外层 lease 提前关闭不破坏打开的读取流。失败补偿和退出确保 pin 仅释放一次。

## 使用示例

```csharp
using Inno.Core.IO;

static IByteDocumentStore CreateDocument(string absolutePath)
{
    return new FileByteDocumentStore(absolutePath);
}
```

文件路径的选择属于调用此 factory 的宿主。`tests/core/Inno.Core.IO.Tests` 覆盖 lease 协调、取消、原子写入、只读文档与读取 pin。

## 当前源码公开 API 清单

以下仅列出当前程序集自己声明的 public/protected 契约；继承成员遵循所属基类页面。internal/private 实现不作为稳定公开 API。签名依据当前源码语义模型生成，行为、参数、异常与所有权说明同时以对应英文 XML 为准。

### `Inno.Core.IO.AtomicDirectory`

| 当前声明 | 行为 |
| --- | --- |
| [`static void Inno.Core.IO.AtomicDirectory.Install(string source, string destination)`](../../src/foundation/core/Inno.Core.IO/AtomicDirectory.cs#L94) | Installs a disjoint candidate tree and restores the previous destination if installation fails. |
| [`static void Inno.Core.IO.AtomicDirectory.Publish(string source, string destination, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/foundation/core/Inno.Core.IO/AtomicDirectory.cs#L43) | Publishes a complete directory at an unoccupied path without replacing an existing destination. |
| [`Inno.Core.IO.AtomicDirectory`](../../src/foundation/core/Inno.Core.IO/AtomicDirectory.cs#L10) | Provides rollback-safe installation of complete directory trees. |

### `Inno.Core.IO.AtomicFile`

| 当前声明 | 行为 |
| --- | --- |
| [`static void Inno.Core.IO.AtomicFile.Install(string source, string destination, bool overwrite = true)`](../../src/foundation/core/Inno.Core.IO/AtomicFile.cs#L64) | Atomically installs an existing same-directory candidate file. |
| [`static void Inno.Core.IO.AtomicFile.WriteAllBytes(string path, System.ReadOnlySpan<byte> data, bool overwrite = true)`](../../src/foundation/core/Inno.Core.IO/AtomicFile.cs#L23) | Writes a complete byte payload and atomically installs it at the destination. |
| [`Inno.Core.IO.AtomicFile`](../../src/foundation/core/Inno.Core.IO/AtomicFile.cs#L9) | Provides durable same-directory file replacement without exposing partial destination content. |

### `Inno.Core.IO.FileByteDocumentStore`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.IO.FileByteDocumentStore.FileByteDocumentStore(string path)`](../../src/foundation/core/Inno.Core.IO/FileByteDocumentStore.cs#L22) | Selects one host-owned absolute document location. |
| [`byte[]? Inno.Core.IO.FileByteDocumentStore.Read()`](../../src/foundation/core/Inno.Core.IO/FileByteDocumentStore.cs#L40) | See the implemented contract. |
| [`void Inno.Core.IO.FileByteDocumentStore.Write(System.ReadOnlySpan<byte> data)`](../../src/foundation/core/Inno.Core.IO/FileByteDocumentStore.cs#L57) | See the implemented contract. |
| [`bool Inno.Core.IO.FileByteDocumentStore.canWrite`](../../src/foundation/core/Inno.Core.IO/FileByteDocumentStore.cs#L34) | See the implemented contract. |
| [`string Inno.Core.IO.FileByteDocumentStore.documentName`](../../src/foundation/core/Inno.Core.IO/FileByteDocumentStore.cs#L31) | See the implemented contract. |
| [`bool Inno.Core.IO.FileByteDocumentStore.exists`](../../src/foundation/core/Inno.Core.IO/FileByteDocumentStore.cs#L37) | See the implemented contract. |
| [`Inno.Core.IO.FileByteDocumentStore`](../../src/foundation/core/Inno.Core.IO/FileByteDocumentStore.cs#L9) | Stores one file document with atomic replacement using the common filesystem boundary. |

### `Inno.Core.IO.FileLease`

| 当前声明 | 行为 |
| --- | --- |
| [`static System.Threading.Tasks.ValueTask<Inno.Core.IO.FileLease> Inno.Core.IO.FileLease.AcquireAsync(string path, System.TimeSpan timeout, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/foundation/core/Inno.Core.IO/FileLease.cs#L52) | Waits for exclusive ownership without blocking an owner thread or changing shared data. |
| [`static System.Threading.Tasks.ValueTask<Inno.Core.IO.FileLease> Inno.Core.IO.FileLease.AcquireSharedAsync(string path, System.TimeSpan timeout, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/foundation/core/Inno.Core.IO/FileLease.cs#L83) | Pins an existing generation against exclusive retirement while permitting other readers. |
| [`void Inno.Core.IO.FileLease.Dispose()`](../../src/foundation/core/Inno.Core.IO/FileLease.cs#L135) | Releases ownership; repeated disposal has no effect and the lease file is retained. |
| [`Inno.Core.IO.FileLease`](../../src/foundation/core/Inno.Core.IO/FileLease.cs#L12) | Owns shared reading or exclusive writing rights coordinated by independent processes. |

### `Inno.Core.IO.IByteDocumentStore`

| 当前声明 | 行为 |
| --- | --- |
| [`byte[]? Inno.Core.IO.IByteDocumentStore.Read()`](../../src/foundation/core/Inno.Core.IO/IByteDocumentStore.cs#L31) | Obtains a newly owned complete document snapshot. |
| [`void Inno.Core.IO.IByteDocumentStore.Write(System.ReadOnlySpan<byte> data)`](../../src/foundation/core/Inno.Core.IO/IByteDocumentStore.cs#L42) | Replaces the document atomically after preparation succeeds. |
| [`bool Inno.Core.IO.IByteDocumentStore.canWrite`](../../src/foundation/core/Inno.Core.IO/IByteDocumentStore.cs#L18) | Gets whether this source permits atomic replacement. |
| [`string Inno.Core.IO.IByteDocumentStore.documentName`](../../src/foundation/core/Inno.Core.IO/IByteDocumentStore.cs#L13) | Gets a stable diagnostic name for this document, independent of its storage implementation. |
| [`bool Inno.Core.IO.IByteDocumentStore.exists`](../../src/foundation/core/Inno.Core.IO/IByteDocumentStore.cs#L23) | Gets whether a document currently exists; callers must still handle absence during a later read. |
| [`Inno.Core.IO.IByteDocumentStore`](../../src/foundation/core/Inno.Core.IO/IByteDocumentStore.cs#L8) | Reads and replaces one complete document without prescribing its physical location. |

### `Inno.Core.IO.OwnedReadStream`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.IO.OwnedReadStream.OwnedReadStream(System.IO.Stream input, System.IDisposable owner)`](../../src/foundation/core/Inno.Core.IO/OwnedReadStream.cs#L30) | Transfers a readable stream and its independent lifetime pin into this wrapper. |
| [`override void Inno.Core.IO.OwnedReadStream.Dispose(bool disposing)`](../../src/foundation/core/Inno.Core.IO/OwnedReadStream.cs#L96) | See the implemented contract. |
| [`override void Inno.Core.IO.OwnedReadStream.Flush()`](../../src/foundation/core/Inno.Core.IO/OwnedReadStream.cs#L85) | See the implemented contract. |
| [`override int Inno.Core.IO.OwnedReadStream.Read(byte[] buffer, int offset, int count)`](../../src/foundation/core/Inno.Core.IO/OwnedReadStream.cs#L58) | See the implemented contract. |
| [`override int Inno.Core.IO.OwnedReadStream.Read(System.Span<byte> buffer)`](../../src/foundation/core/Inno.Core.IO/OwnedReadStream.cs#L64) | See the implemented contract. |
| [`override System.Threading.Tasks.Task<int> Inno.Core.IO.OwnedReadStream.ReadAsync(byte[] buffer, int offset, int count, System.Threading.CancellationToken cancellationToken)`](../../src/foundation/core/Inno.Core.IO/OwnedReadStream.cs#L73) | See the implemented contract. |
| [`override System.Threading.Tasks.ValueTask<int> Inno.Core.IO.OwnedReadStream.ReadAsync(System.Memory<byte> buffer, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/foundation/core/Inno.Core.IO/OwnedReadStream.cs#L68) | See the implemented contract. |
| [`override int Inno.Core.IO.OwnedReadStream.ReadByte()`](../../src/foundation/core/Inno.Core.IO/OwnedReadStream.cs#L66) | See the implemented contract. |
| [`override long Inno.Core.IO.OwnedReadStream.Seek(long offset, System.IO.SeekOrigin origin)`](../../src/foundation/core/Inno.Core.IO/OwnedReadStream.cs#L80) | See the implemented contract. |
| [`override void Inno.Core.IO.OwnedReadStream.SetLength(long value)`](../../src/foundation/core/Inno.Core.IO/OwnedReadStream.cs#L87) | See the implemented contract. |
| [`override void Inno.Core.IO.OwnedReadStream.Write(byte[] buffer, int offset, int count)`](../../src/foundation/core/Inno.Core.IO/OwnedReadStream.cs#L89) | See the implemented contract. |
| [`override bool Inno.Core.IO.OwnedReadStream.CanRead`](../../src/foundation/core/Inno.Core.IO/OwnedReadStream.cs#L43) | See the implemented contract. |
| [`override bool Inno.Core.IO.OwnedReadStream.CanSeek`](../../src/foundation/core/Inno.Core.IO/OwnedReadStream.cs#L45) | See the implemented contract. |
| [`override bool Inno.Core.IO.OwnedReadStream.CanWrite`](../../src/foundation/core/Inno.Core.IO/OwnedReadStream.cs#L47) | See the implemented contract. |
| [`override long Inno.Core.IO.OwnedReadStream.Length`](../../src/foundation/core/Inno.Core.IO/OwnedReadStream.cs#L49) | See the implemented contract. |
| [`override long Inno.Core.IO.OwnedReadStream.Position`](../../src/foundation/core/Inno.Core.IO/OwnedReadStream.cs#L51) | See the implemented contract. |
| [`Inno.Core.IO.OwnedReadStream`](../../src/foundation/core/Inno.Core.IO/OwnedReadStream.cs#L11) | Keeps an independent read stream and its lifetime pin owned together until both retire. |

### `Inno.Core.IO.PathBoundary`

| 当前声明 | 行为 |
| --- | --- |
| [`static System.Collections.Generic.IEnumerable<string> Inno.Core.IO.PathBoundary.EnumerateFiles(string root)`](../../src/foundation/core/Inno.Core.IO/PathBoundary.cs#L125) | Enumerates regular files in an owned tree without following filesystem links. |
| [`static string Inno.Core.IO.PathBoundary.RequireContained(string root, string path)`](../../src/foundation/core/Inno.Core.IO/PathBoundary.cs#L50) | Validates and normalizes an absolute path beneath a root. |
| [`static string Inno.Core.IO.PathBoundary.RequireUnlinkedPath(string root, string path)`](../../src/foundation/core/Inno.Core.IO/PathBoundary.cs#L81) | Resolves an owned path while rejecting existing links in its containment chain. |
| [`static string Inno.Core.IO.PathBoundary.Resolve(string root, string relativePath)`](../../src/foundation/core/Inno.Core.IO/PathBoundary.cs#L24) | Resolves a relative path beneath a root and rejects traversal outside that root. |
| [`Inno.Core.IO.PathBoundary`](../../src/foundation/core/Inno.Core.IO/PathBoundary.cs#L10) | Resolves paths while enforcing an explicit filesystem ownership boundary. |

### `Inno.Core.IO.ReadOnlyByteDocumentStore`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.IO.ReadOnlyByteDocumentStore.ReadOnlyByteDocumentStore(string documentName, System.ReadOnlySpan<byte> data)`](../../src/foundation/core/Inno.Core.IO/ReadOnlyByteDocumentStore.cs#L21) | Copies one complete document into this source. |
| [`byte[] Inno.Core.IO.ReadOnlyByteDocumentStore.Read()`](../../src/foundation/core/Inno.Core.IO/ReadOnlyByteDocumentStore.cs#L40) | See the implemented contract. |
| [`void Inno.Core.IO.ReadOnlyByteDocumentStore.Write(System.ReadOnlySpan<byte> data)`](../../src/foundation/core/Inno.Core.IO/ReadOnlyByteDocumentStore.cs#L43) | See the implemented contract. |
| [`bool Inno.Core.IO.ReadOnlyByteDocumentStore.canWrite`](../../src/foundation/core/Inno.Core.IO/ReadOnlyByteDocumentStore.cs#L34) | See the implemented contract. |
| [`string Inno.Core.IO.ReadOnlyByteDocumentStore.documentName`](../../src/foundation/core/Inno.Core.IO/ReadOnlyByteDocumentStore.cs#L31) | See the implemented contract. |
| [`bool Inno.Core.IO.ReadOnlyByteDocumentStore.exists`](../../src/foundation/core/Inno.Core.IO/ReadOnlyByteDocumentStore.cs#L37) | See the implemented contract. |
| [`Inno.Core.IO.ReadOnlyByteDocumentStore`](../../src/foundation/core/Inno.Core.IO/ReadOnlyByteDocumentStore.cs#L8) | Owns an immutable document snapshot that cannot be replaced through its read boundary. |

## 项目依赖

- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：项目引用；公开签名可见性由语义边界检查确认。

共同 MSBuild 注入的 analyzer 与编译规则属于构建依赖，完整有效项目图记录在本轮验收证据中。
