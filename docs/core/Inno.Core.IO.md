# Inno.Core.IO

[分类索引](README.md) · [Wiki 首页](../README.md)

## 职责与边界

提供原子文件/目录发布、明确文件 lease、路径边界、小型 byte document 与独立 stream pin。领域模型通过文档或内容契约读取，不把物理位置泄漏给共享 Runtime。

## 所有权与失败

`FileLease` 支持共享读取与独占写入协调；Windows 与 Unix 遵循同一语义。锁文件保留，不能删除锁文件代替解锁。等待受取消与有界 timeout 控制。

`IByteDocumentStore` 仅用于小型文档。`FileByteDocumentStore` 复用 AtomicFile；`ReadOnlyByteDocumentStore` 捕获 owned bytes，写入明确失败。它不是 Asset catalog 或事件协议。

`AtomicDirectory.Publish` 只向不存在的目标发布完整 staging。需要替换目录时使用 `Install`，由调用方先协调全部读写 owner；这两个原子目录入口不会自行取得读取 lease。提交后的旧目录清理失败与提交前失败具有不同结果，不能把清理异常解释为新产物未安装。

`DirectoryCleanupException` 只表示提交后的 backup 删除失败；`installedDirectory` 指向已经完整安装的新目录，`backupDirectory` 指向保留的旧目录。消费方记录独立清理诊断并继续使用已提交输出，不重试安装、不回滚新目录。其他 IO 异常仍表示安装阶段失败，遵循原来的恢复语义。

`OwnedReadStream` 独立持有读取 pin，外层 lease 提前关闭不破坏打开的读取流。关闭 stream 才释放它自己的 pin；同一 lease 的多个 stream 分别结束各自的读取生命周期。

## 使用示例

`ProcessOwnerStamp` 固定 PID 与 UTC 启动 ticks；PID 被复用时返回 `Exited`。无法读取进程身份、访问被拒绝或无效默认值返回 `Unknown`，清理方必须保留对应目录。它不持有 `Process` 实例。

这些 claim 用于具备操作系统进程生命周期的构建宿主。`CaptureCurrent` 在不提供该能力的执行环境中抛出 `PlatformNotSupportedException`；Browser Player 不创建构建进程 claim，不能用虚构 PID 建立可删除的目录 owner。

`ProcessOwnedDirectory.CreateAsync` 在生产者声明的绝对根下创建全新工作目录，先取得持久 lease 并原子写入进程 claim，再开放 payload 写入。已有未登记目录不会被认领。子进程开始消费前调用 `RegisterCurrentProcess`，父进程等待子进程退出后才 `Dispose`。其他 owner 活动或身份未知时，释放 lease 并报告清理失败，保留 claim 供后续核查。`ReadClaims` 只读取当前严格声明，不根据目录名猜测归属；持久锁文件不删除。

创建方与子进程登记使用共享读取保护；退休先释放创建方的读取，再取得同一持久路径的独占权，重新核对参与进程。登记与删除不能同时越过这一边界。无法取得退休权时保留 claim，并报告清理延期。

```csharp
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;

static async Task WriteTemporary(
    string producerRoot,
    CancellationToken cancellation
) {
    using ProcessOwnedDirectory working = await ProcessOwnedDirectory.CreateAsync(producerRoot, cancellation);
    await File.WriteAllTextAsync(Path.Combine(working.directory, "request.txt"), "owned", cancellation);
}
```

这些协议属于进程和文件生命周期，不进入游戏脚本导出。构建缓存通过自己的 owner 登记接入清理；Core.IO 不引用构建或具体后端。

SDK Task 引导只能保留 BCL 路径时，`CreateForProcessAsync(root, identity)` 使用同一操作传播的明确身份创建严格 claim，或登记已有活跃 claim 的参与进程，然后将保护转交给进程生命周期，返回路径。退出或无法验证的 claim、已存在但无登记的目录均拒绝复用。它不把 lease 或 SDK 对象塞入 Build registry；进程退出后由明确的生产者退休。一般作用域仍优先使用可释放的 `CreateAsync`。

```csharp
using Inno.Core.IO;

static IByteDocumentStore CreateDocument(string absolutePath)
{
    return new FileByteDocumentStore(absolutePath);
}
```

文件路径的选择属于调用此 factory 的宿主。`tests/core/Inno.Core.IO.Tests` 覆盖 lease 协调、取消、原子写入、只读文档与读取 pin。

## 规范拼写与文件系统所有权

PathBoundary 对规范化绝对路径使用 ordinal 拼写比较，不根据宿主系统推测卷是否区分大小写。调用者应从同一 root 构造子路径；即使卷忽略大小写，root 的大小写别名仍被拒绝。RequireUnlinkedPath 使用已通过边界校验的后缀逐段检查链接，不通过大小写不确定的相对路径推导检查对象。

AtomicFile.Install 要求两个 parent 的规范拼写完全一致；候选与目标名称仅大小写不同也会安全拒绝。路径检查不提供对外部进程改写目录的保护；参与者仍须持有 owner lease。

## 当前源码公开 API 清单

签名来自当前 SDK 求值与语义符号；按类型列出 public/protected 契约。工作流与生命周期见本页前文。

### `Inno.Core.IO.AtomicDirectory`

<!-- api: T:Inno.Core.IO.AtomicDirectory 7665e379a901034bd0daa47622182e4bdef12df933a075b61a39ffab6fa3484c -->
<!-- api: M:Inno.Core.IO.AtomicDirectory.Publish(System.String,System.String,System.Threading.CancellationToken) 73c0b6ee2f22ce61e25218bad0f3e9829c53b955308d787c1d61baf7f4b5bcbc -->
<!-- api: M:Inno.Core.IO.AtomicDirectory.Install(System.String,System.String) 65b5fae575a752a5553cac84a054c596ff89920e4187173be6523668548fe86a -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-dad8d3e413606647"></a>[`public static void Inno.Core.IO.AtomicDirectory.Install(string source, string destination)`](../../src/foundation/core/Inno.Core.IO/AtomicDirectory.cs#L96) | Installs a disjoint candidate tree and restores the previous destination if installation fails.<br>`source`：The complete candidate directory.<br>`destination`：The destination directory. | The caller must serialize readers and writers while the two directory moves run. Backup cleanup runs after commit; its failure preserves the installed candidate and remaining backup. | `System.ArgumentException`：Either path is blank, or the candidate and destination trees overlap.<br>`System.IO.DirectoryNotFoundException`：The candidate directory does not exist.<br>`System.IO.IOException`：Installation fails before the candidate becomes authoritative.<br>`Inno.Core.IO.DirectoryCleanupException`：Installation committed, but the old backup could not be removed. The installed tree remains complete.<br>`System.UnauthorizedAccessException`：The operating system denies access before the candidate can be installed.<br>`System.AggregateException`：Installation and restoration both fail; neither tree is deleted to force a rollback. |
| <a id="api-6fa5437f2e3ec329"></a>[`public static void Inno.Core.IO.AtomicDirectory.Publish(string source, string destination, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/foundation/core/Inno.Core.IO/AtomicDirectory.cs#L43) | Publishes a complete directory at an unoccupied path without replacing an existing destination.<br>`source`：The complete candidate directory, disjoint from the destination.<br>`destination`：The unoccupied publication path on the same filesystem.<br>`cancellationToken`：Cancels preparation or bounded waits before the rename commits. | Windows access and sharing failures are retried for at most two seconds to tolerate temporary readers. Failure preserves the candidate and any existing destination. A committed rename is not canceled afterward. | `System.ArgumentException`：A path is blank or the two directory trees overlap.<br>`System.IO.DirectoryNotFoundException`：The source directory does not exist.<br>`System.IO.IOException`：The destination exists, the filesystem cannot rename the tree, or a transient block persists.<br>`System.UnauthorizedAccessException`：The operating system continues denying access after the bounded retry window.<br>`System.OperationCanceledException`：Cancellation is requested before the directory is published. |
| <a id="api-caf46c1a57af110f"></a>[`public static class Inno.Core.IO.AtomicDirectory`](../../src/foundation/core/Inno.Core.IO/AtomicDirectory.cs#L10) | Provides rollback-safe installation of complete directory trees. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Core.IO.AtomicFile`

<!-- api: T:Inno.Core.IO.AtomicFile 1e80794d7c609d4533a84d84795da5c6be12b01a22e69c34825e0ebddbec963e -->
<!-- api: M:Inno.Core.IO.AtomicFile.WriteAllBytes(System.String,System.ReadOnlySpan{System.Byte},System.Boolean) 752a360022972a45a702098a150364da63a8b2d9f3e5cbd52ffe27d8db5f5a88 -->
<!-- api: M:Inno.Core.IO.AtomicFile.Install(System.String,System.String,System.Boolean) 8822ceeac332cd33204d41099512f0c18d843ccf4a2f6248fc818155ba6a4a48 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-5d7c305f369e5c62"></a>[`public static void Inno.Core.IO.AtomicFile.Install(string source, string destination, bool overwrite = true)`](../../src/foundation/core/Inno.Core.IO/AtomicFile.cs#L64) | Atomically installs an existing same-directory candidate file.<br>`source`：The complete candidate file beside the destination.<br>`destination`：The destination file path beside the candidate.<br>`overwrite`：Whether an existing destination may be replaced; false never overwrites an external creator. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-131b9c8a528b9757"></a>[`public static void Inno.Core.IO.AtomicFile.WriteAllBytes(string path, System.ReadOnlySpan<byte> data, bool overwrite = true)`](../../src/foundation/core/Inno.Core.IO/AtomicFile.cs#L23) | Writes a complete byte payload and atomically installs it at the destination.<br>`path`：The destination file path.<br>`data`：The complete file payload.<br>`overwrite`：Whether an existing destination may be replaced; false atomically rejects name collisions. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-50bb21c877feddc1"></a>[`public static class Inno.Core.IO.AtomicFile`](../../src/foundation/core/Inno.Core.IO/AtomicFile.cs#L9) | Provides durable same-directory file replacement without exposing partial destination content. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Core.IO.DirectoryCleanupException`

<!-- api: T:Inno.Core.IO.DirectoryCleanupException 18074fe765a0e7e93df1e5b96fe688a7f20766884172a85cf5fbd1156e913efe -->
<!-- api: P:Inno.Core.IO.DirectoryCleanupException.installedDirectory 2b6ae47baf5b93a7cc72f936dfecb9587506ae8dfbcd70011ae2e90f4c494f08 -->
<!-- api: P:Inno.Core.IO.DirectoryCleanupException.backupDirectory 9aa3cfd1676d9b145e2d12e58b5b4116e669679f101f131802bbcd864e226dbb -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e924516aa40c5950"></a>[`public string Inno.Core.IO.DirectoryCleanupException.backupDirectory { get; }`](../../src/foundation/core/Inno.Core.IO/DirectoryCleanupException.cs#L30) | Gets the previous tree retained for a later owner-coordinated cleanup attempt. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-6774490d7602ba84"></a>[`public string Inno.Core.IO.DirectoryCleanupException.installedDirectory { get; }`](../../src/foundation/core/Inno.Core.IO/DirectoryCleanupException.cs#L25) | Gets the complete installed tree that remains authoritative after cleanup failed. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-b292b985feb0da6c"></a>[`public sealed class Inno.Core.IO.DirectoryCleanupException : System.IO.IOException`](../../src/foundation/core/Inno.Core.IO/DirectoryCleanupException.cs#L10) | Reports backup cleanup failure after a complete directory installation has already committed. The installed tree remains authoritative and must not be treated as a failed installation. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Core.IO.FileByteDocumentStore`

<!-- api: T:Inno.Core.IO.FileByteDocumentStore 91569048f049ae940238c1b5a4c039ee814d461bb470c9b957601a56abb44859 -->
<!-- api: M:Inno.Core.IO.FileByteDocumentStore.#ctor(System.String) aae905368090a51cb2d512373c2316cae1bca9b1bc5f0dce4fee106c485904cc -->
<!-- api: P:Inno.Core.IO.FileByteDocumentStore.documentName 7f3cc7a26e53843c56caddfddfb81dc393ad3603844d8a05f71e35fec3201ec2 -->
<!-- api: P:Inno.Core.IO.FileByteDocumentStore.canWrite efaae70a5a31b1b182a5099146281b75bc46a8c5ff8218b2924f087b15f40d2c -->
<!-- api: P:Inno.Core.IO.FileByteDocumentStore.exists 550f458e06557b4a2c9100c3adf4cd161db971122beca75b9aee0156a3370f2b -->
<!-- api: M:Inno.Core.IO.FileByteDocumentStore.Read 05060e917d0f3c7428c9c8dd0e449e1f9e13e87bee66f4109731f34d51362c0d -->
<!-- api: M:Inno.Core.IO.FileByteDocumentStore.Write(System.ReadOnlySpan{System.Byte}) 4cf2b7e624453bfd3fdba844cc58f56dc597bc95bcc00890dd84ac8bc1b1ad17 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-fef31f060f75abcd"></a>[`public Inno.Core.IO.FileByteDocumentStore.FileByteDocumentStore(string path)`](../../src/foundation/core/Inno.Core.IO/FileByteDocumentStore.cs#L22) | Selects one host-owned absolute document location.<br>`path`：The absolute file path; its parent directory is prepared only when writing. | 约束由所属类型与本页生命周期定义。 | `System.ArgumentException`：The path is empty or relative. |
| <a id="api-1d8edfcca7e0762a"></a>[`public byte[]? Inno.Core.IO.FileByteDocumentStore.Read()`](../../src/foundation/core/Inno.Core.IO/FileByteDocumentStore.cs#L40) | Obtains a newly owned complete document snapshot.<br>返回：Owned document bytes, or null when no document exists; an empty document remains distinct from absence. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-93b5aabe94ff0cc0"></a>[`public void Inno.Core.IO.FileByteDocumentStore.Write(System.ReadOnlySpan<byte> data)`](../../src/foundation/core/Inno.Core.IO/FileByteDocumentStore.cs#L57) | Replaces the document atomically after preparation succeeds.<br>`data`：The complete candidate bytes; the implementation cannot retain borrowed caller memory. | 约束由所属类型与本页生命周期定义。 | `System.NotSupportedException`：The source is read-only. |
| <a id="api-7d4c928042d60203"></a>[`public bool Inno.Core.IO.FileByteDocumentStore.canWrite { get; }`](../../src/foundation/core/Inno.Core.IO/FileByteDocumentStore.cs#L34) | Gets whether this source permits atomic replacement. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-fd32944e8ef37e93"></a>[`public string Inno.Core.IO.FileByteDocumentStore.documentName { get; }`](../../src/foundation/core/Inno.Core.IO/FileByteDocumentStore.cs#L31) | Gets a stable diagnostic name for this document, independent of its storage implementation. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-f38b421295ef83da"></a>[`public bool Inno.Core.IO.FileByteDocumentStore.exists { get; }`](../../src/foundation/core/Inno.Core.IO/FileByteDocumentStore.cs#L37) | Gets whether a document currently exists; callers must still handle absence during a later read. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-d4ae6dc0b0e6fbc9"></a>[`public sealed class Inno.Core.IO.FileByteDocumentStore : Inno.Core.IO.IByteDocumentStore`](../../src/foundation/core/Inno.Core.IO/FileByteDocumentStore.cs#L9) | Stores one file document with atomic replacement using the common filesystem boundary. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Core.IO.FileLease`

<!-- api: T:Inno.Core.IO.FileLease 7d52408dbb67bc4519bf988da4006cef2252415bfcb1a063af7e73c869326975 -->
<!-- api: M:Inno.Core.IO.FileLease.AcquireAsync(System.String,System.TimeSpan,System.Threading.CancellationToken) fb42377fed0a00e1f73f8615a36ae7f11e209c431e36a90e6d73fd3e904c3261 -->
<!-- api: M:Inno.Core.IO.FileLease.AcquireSharedAsync(System.String,System.TimeSpan,System.Threading.CancellationToken) 398e4018412b6403c21edf9a73b0bd94ceabcc5bfe0e1396ad5721d754d87a96 -->
<!-- api: M:Inno.Core.IO.FileLease.Dispose 0f0eb882bd3b84003d59c367c63d7da8a344f9448da71049e6bb56b18b0a0cdc -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-05f88da98b063b98"></a>[`public static System.Threading.Tasks.ValueTask<Inno.Core.IO.FileLease> Inno.Core.IO.FileLease.AcquireAsync(string path, System.TimeSpan timeout, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/foundation/core/Inno.Core.IO/FileLease.cs#L52) | Waits for exclusive ownership without blocking an owner thread or changing shared data.<br>返回：The exclusive lease whose disposal permits the next participant to acquire ownership.<br>`path`：The absolute lease file path; all participants must use the same persistent path.<br>`timeout`：The maximum wait, or F:System.Threading.Timeout.InfiniteTimeSpan for no time limit.<br>`cancellationToken`：Cancels waiting before ownership is returned. | Lease files remain after disposal. Removing them would let Unix processes lock different inodes for the same path while an earlier participant still owns the original inode. | `System.ArgumentException`：The path is empty or relative, or the timeout is negative and not infinite.<br>`System.TimeoutException`：Another participant retains ownership beyond the permitted wait.<br>`System.OperationCanceledException`：Waiting was canceled; no lease is returned.<br>`System.IO.IOException`：The lease cannot be opened for a reason other than competing ownership.<br>`System.UnauthorizedAccessException`：The operating system denies access to the lease path. |
| <a id="api-d95e7b761992a35e"></a>[`public static System.Threading.Tasks.ValueTask<Inno.Core.IO.FileLease> Inno.Core.IO.FileLease.AcquireSharedAsync(string path, System.TimeSpan timeout, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/foundation/core/Inno.Core.IO/FileLease.cs#L83) | Pins an existing generation against exclusive retirement while permitting other readers.<br>返回：A shared lease; an exclusive lease cannot be acquired until every reader releases ownership.<br>`path`：The absolute persistent lease path previously initialized by an exclusive owner.<br>`timeout`：The maximum wait, or F:System.Threading.Timeout.InfiniteTimeSpan for no time limit.<br>`cancellationToken`：Cancels waiting before ownership is returned. | 约束由所属类型与本页生命周期定义。 | `System.IO.FileNotFoundException`：The owner has not initialized the persistent lease file.<br>`System.TimeoutException`：An exclusive owner retains the file beyond the permitted wait.<br>`System.OperationCanceledException`：Waiting was canceled; no lease is returned. |
| <a id="api-81ed969f476b3edd"></a>[`public void Inno.Core.IO.FileLease.Dispose()`](../../src/foundation/core/Inno.Core.IO/FileLease.cs#L135) | Releases ownership; repeated disposal has no effect and the lease file is retained. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-965d4a56845471e3"></a>[`public sealed class Inno.Core.IO.FileLease : System.IDisposable`](../../src/foundation/core/Inno.Core.IO/FileLease.cs#L12) | Owns shared reading or exclusive writing rights coordinated by independent processes. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Core.IO.IByteDocumentStore`

<!-- api: T:Inno.Core.IO.IByteDocumentStore f05a3247c06ecaabd12c6574dbe05e90a9472556d68b63fd10c353643f358c0a -->
<!-- api: P:Inno.Core.IO.IByteDocumentStore.documentName 1edc9b5b68808b8a59749fbaa3342c6a279b9b90f9f417413c3df1164bfd9e31 -->
<!-- api: P:Inno.Core.IO.IByteDocumentStore.canWrite 808a2b9d375664995cb63cc1f05bd85fb750402dba176f880d1e9f8619fcc87d -->
<!-- api: P:Inno.Core.IO.IByteDocumentStore.exists 2b6db274042de670e347e24d714eb297a34a14638153837a93d0f57f6de7306a -->
<!-- api: M:Inno.Core.IO.IByteDocumentStore.Read 1768807c3b029f02381f0b95f7f9e55070b05993d0633493183c58ea6dd8cf4e -->
<!-- api: M:Inno.Core.IO.IByteDocumentStore.Write(System.ReadOnlySpan{System.Byte}) af94e7842408b0db9617929e657aa961aca692dbb07d2fb3c1209a7d668a2481 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2c8a681febc596e3"></a>[`byte[]? Inno.Core.IO.IByteDocumentStore.Read()`](../../src/foundation/core/Inno.Core.IO/IByteDocumentStore.cs#L31) | Obtains a newly owned complete document snapshot.<br>返回：Owned document bytes, or null when no document exists; an empty document remains distinct from absence. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-6fcdecd43cf59faa"></a>[`void Inno.Core.IO.IByteDocumentStore.Write(System.ReadOnlySpan<byte> data)`](../../src/foundation/core/Inno.Core.IO/IByteDocumentStore.cs#L42) | Replaces the document atomically after preparation succeeds.<br>`data`：The complete candidate bytes; the implementation cannot retain borrowed caller memory. | 约束由所属类型与本页生命周期定义。 | `System.NotSupportedException`：The source is read-only. |
| <a id="api-7b72a2e587528e44"></a>[`bool Inno.Core.IO.IByteDocumentStore.canWrite { get; }`](../../src/foundation/core/Inno.Core.IO/IByteDocumentStore.cs#L18) | Gets whether this source permits atomic replacement. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-60e105027dd54a28"></a>[`string Inno.Core.IO.IByteDocumentStore.documentName { get; }`](../../src/foundation/core/Inno.Core.IO/IByteDocumentStore.cs#L13) | Gets a stable diagnostic name for this document, independent of its storage implementation. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-5b6bb8c5a66081e6"></a>[`bool Inno.Core.IO.IByteDocumentStore.exists { get; }`](../../src/foundation/core/Inno.Core.IO/IByteDocumentStore.cs#L23) | Gets whether a document currently exists; callers must still handle absence during a later read. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-967cb24b732eb289"></a>[`public interface Inno.Core.IO.IByteDocumentStore`](../../src/foundation/core/Inno.Core.IO/IByteDocumentStore.cs#L8) | Reads and replaces one complete document without prescribing its physical location. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Core.IO.OwnedReadStream`

<!-- api: T:Inno.Core.IO.OwnedReadStream 52e334ee89175723d0e0f4e8737a33ebe0cb08adc7e4f5411eddd1146e72b698 -->
<!-- api: M:Inno.Core.IO.OwnedReadStream.#ctor(System.IO.Stream,System.IDisposable) 5caff3ecde411c84839d5a6d491e94635829d7a82ec69c5d21524ad3bbe42137 -->
<!-- api: P:Inno.Core.IO.OwnedReadStream.CanRead 15c5fa18463de3149e286226dad6d23032e7ef6d159b027e1743a7d1bd7af86d -->
<!-- api: P:Inno.Core.IO.OwnedReadStream.CanSeek 284fb08d402e3d8d0c35d670ffee2e4b125d6a541ea56fabc7807bd2111c6710 -->
<!-- api: P:Inno.Core.IO.OwnedReadStream.CanWrite 5e4b025d19ff660525d818225d7a7b1329dc93a4c802d6b06969646132f98209 -->
<!-- api: P:Inno.Core.IO.OwnedReadStream.Length e72c8859ea066722e27d7e49ab9061515a4194246ca58b7fd52804f264885d32 -->
<!-- api: P:Inno.Core.IO.OwnedReadStream.Position cd283fa4a2e1c0ebe65655bf5e7a87973abfb45f7428ae6fd5ab4052fd043bea -->
<!-- api: M:Inno.Core.IO.OwnedReadStream.Read(System.Byte[],System.Int32,System.Int32) 54bac806a05986eb36478c1bb04c9c888d2e9ca5bf08b526e16c2fb16d02118e -->
<!-- api: M:Inno.Core.IO.OwnedReadStream.Read(System.Span{System.Byte}) 1dc5a0ee0730d85311f2179cef727388f53c225dc2c04bf97226b480ddecd59a -->
<!-- api: M:Inno.Core.IO.OwnedReadStream.ReadByte 5e13b77e5c793de96c113f3dd127c7a6d97769932dcbebf4798c59561b389e78 -->
<!-- api: M:Inno.Core.IO.OwnedReadStream.ReadAsync(System.Memory{System.Byte},System.Threading.CancellationToken) c6a6cf00c2fdf65dfa28552b5cac33445379ce7a74dae17664e6d1ded431bfa7 -->
<!-- api: M:Inno.Core.IO.OwnedReadStream.ReadAsync(System.Byte[],System.Int32,System.Int32,System.Threading.CancellationToken) b1caaff9bc5cfb0708b298efc60601df0befc9ec198c189b23763399ca8b4614 -->
<!-- api: M:Inno.Core.IO.OwnedReadStream.Seek(System.Int64,System.IO.SeekOrigin) a894996558f5cf94c0efdf0c6a646049e07ecdde2e778c214de645708bc63456 -->
<!-- api: M:Inno.Core.IO.OwnedReadStream.Flush 6871a040860c6b32c9cc4024465ec7809e9f9cf6df1f6740949e8f86b85ce984 -->
<!-- api: M:Inno.Core.IO.OwnedReadStream.SetLength(System.Int64) c9f8a21dcea309a07e3af9fc1591dfcafa01d164ea7cec5f94f315dfc44006f4 -->
<!-- api: M:Inno.Core.IO.OwnedReadStream.Write(System.Byte[],System.Int32,System.Int32) 004333b7e0b8a45d50b025f543823df59fa56855002f8b509ddcb7cb80e67b45 -->
<!-- api: M:Inno.Core.IO.OwnedReadStream.Dispose(System.Boolean) ec2c747c769fb8fdf94e2ac5e033255dfa4d373cb9dc6bbabf36bad373eb778e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a5111cce5638a7a4"></a>[`public Inno.Core.IO.OwnedReadStream.OwnedReadStream(System.IO.Stream input, System.IDisposable owner)`](../../src/foundation/core/Inno.Core.IO/OwnedReadStream.cs#L30) | Transfers a readable stream and its independent lifetime pin into this wrapper.<br>`input`：The owned readable stream; it is closed before its pin is released.<br>`owner`：The owned pin whose repeated disposal must safely retry unfinished retirement. | 约束由所属类型与本页生命周期定义。 | `System.ArgumentException`：The stream is not readable. |
| <a id="api-6378a75066e63a2b"></a>[`protected override void Inno.Core.IO.OwnedReadStream.Dispose(bool disposing)`](../../src/foundation/core/Inno.Core.IO/OwnedReadStream.cs#L96) | Releases the unmanaged resources used by the System.IO.Stream and optionally releases the managed resources.<br>`disposing`：true to release both managed and unmanaged resources; false to release only unmanaged resources. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-4cc2560470f9d4e2"></a>[`public override void Inno.Core.IO.OwnedReadStream.Flush()`](../../src/foundation/core/Inno.Core.IO/OwnedReadStream.cs#L85) | When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device. | 约束由所属类型与本页生命周期定义。 | `System.IO.IOException`：An I/O error occurs. |
| <a id="api-b2621c533f73ae26"></a>[`public override int Inno.Core.IO.OwnedReadStream.Read(byte[] buffer, int offset, int count)`](../../src/foundation/core/Inno.Core.IO/OwnedReadStream.cs#L58) | When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.<br>返回：The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if count is 0 or the end of the stream has been reached.<br>`buffer`：An array of bytes. When this method returns, the buffer contains the specified byte array with the values between offset and (offset + count - 1) replaced by the bytes read from the current source.<br>`offset`：The zero-based byte offset in buffer at which to begin storing the data read from the current stream.<br>`count`：The maximum number of bytes to be read from the current stream. | 约束由所属类型与本页生命周期定义。 | `System.ArgumentException`：The sum of offset and count is larger than the buffer length.<br>`System.ArgumentNullException`：buffer is null.<br>`System.ArgumentOutOfRangeException`：offset or count is negative.<br>`System.IO.IOException`：An I/O error occurs.<br>`System.NotSupportedException`：The stream does not support reading.<br>`System.ObjectDisposedException`：Methods were called after the stream was closed. |
| <a id="api-84dd3235004171d5"></a>[`public override int Inno.Core.IO.OwnedReadStream.Read(System.Span<byte> buffer)`](../../src/foundation/core/Inno.Core.IO/OwnedReadStream.cs#L64) | When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.<br>返回：The total number of bytes read into the buffer. This can be less than the size of the buffer if that many bytes are not currently available, or zero (0) if the buffer's length is zero or the end of the stream has been reached.<br>`buffer`：A region of memory. When this method returns, the contents of this region are replaced by the bytes read from the current source. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-ff10f8b11df504c1"></a>[`public override System.Threading.Tasks.Task<int> Inno.Core.IO.OwnedReadStream.ReadAsync(byte[] buffer, int offset, int count, System.Threading.CancellationToken cancellationToken)`](../../src/foundation/core/Inno.Core.IO/OwnedReadStream.cs#L73) | Asynchronously reads a sequence of bytes from the current stream, advances the position within the stream by the number of bytes read, and monitors cancellation requests.<br>返回：A task that represents the asynchronous read operation. The value of the TResult parameter contains the total number of bytes read into the buffer. The result value can be less than the number of bytes requested if the number of bytes currently available is less than the requested number, or it can be 0 (zero) if count is 0 or if the end of the stream has been reached.<br>`buffer`：The buffer to write the data into.<br>`offset`：The byte offset in buffer at which to begin writing data from the stream.<br>`count`：The maximum number of bytes to read.<br>`cancellationToken`：The token to monitor for cancellation requests. The default value is P:System.Threading.CancellationToken.None. | 约束由所属类型与本页生命周期定义。 | `System.ArgumentNullException`：buffer is null.<br>`System.ArgumentOutOfRangeException`：offset or count is negative.<br>`System.ArgumentException`：The sum of offset and count is larger than the buffer length.<br>`System.NotSupportedException`：The stream does not support reading.<br>`System.ObjectDisposedException`：The stream has been disposed.<br>`System.InvalidOperationException`：The stream is currently in use by a previous read operation.<br>`System.OperationCanceledException`：The cancellation token was canceled. This exception is stored into the returned task. |
| <a id="api-072812672a40878d"></a>[`public override System.Threading.Tasks.ValueTask<int> Inno.Core.IO.OwnedReadStream.ReadAsync(System.Memory<byte> buffer, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/foundation/core/Inno.Core.IO/OwnedReadStream.cs#L68) | Asynchronously reads a sequence of bytes from the current stream, advances the position within the stream by the number of bytes read, and monitors cancellation requests.<br>返回：A task that represents the asynchronous read operation. The value of its P:System.Threading.Tasks.ValueTask`1.Result property contains the total number of bytes read into the buffer. The result value can be less than the length of the buffer if that many bytes are not currently available, or it can be 0 (zero) if the length of the buffer is 0 or if the end of the stream has been reached.<br>`buffer`：The region of memory to write the data into.<br>`cancellationToken`：The token to monitor for cancellation requests. The default value is P:System.Threading.CancellationToken.None. | 约束由所属类型与本页生命周期定义。 | `System.OperationCanceledException`：The cancellation token was canceled. This exception is stored into the returned task. |
| <a id="api-ed8c8fe02b614b53"></a>[`public override int Inno.Core.IO.OwnedReadStream.ReadByte()`](../../src/foundation/core/Inno.Core.IO/OwnedReadStream.cs#L66) | Reads a byte from the stream and advances the position within the stream by one byte, or returns -1 if at the end of the stream.<br>返回：The unsigned byte cast to an System.Int32, or -1 if at the end of the stream. | 约束由所属类型与本页生命周期定义。 | `System.NotSupportedException`：The stream does not support reading.<br>`System.ObjectDisposedException`：Methods were called after the stream was closed. |
| <a id="api-c13da1e544a860e9"></a>[`public override long Inno.Core.IO.OwnedReadStream.Seek(long offset, System.IO.SeekOrigin origin)`](../../src/foundation/core/Inno.Core.IO/OwnedReadStream.cs#L80) | When overridden in a derived class, sets the position within the current stream.<br>返回：The new position within the current stream.<br>`offset`：A byte offset relative to the origin parameter.<br>`origin`：A value of type System.IO.SeekOrigin indicating the reference point used to obtain the new position. | 约束由所属类型与本页生命周期定义。 | `System.IO.IOException`：An I/O error occurs.<br>`System.NotSupportedException`：The stream does not support seeking, such as if the stream is constructed from a pipe or console output.<br>`System.ObjectDisposedException`：Methods were called after the stream was closed. |
| <a id="api-06ad11dc269ac7aa"></a>[`public override void Inno.Core.IO.OwnedReadStream.SetLength(long value)`](../../src/foundation/core/Inno.Core.IO/OwnedReadStream.cs#L87) | When overridden in a derived class, sets the length of the current stream.<br>`value`：The desired length of the current stream in bytes. | 约束由所属类型与本页生命周期定义。 | `System.IO.IOException`：An I/O error occurs.<br>`System.NotSupportedException`：The stream does not support both writing and seeking, such as if the stream is constructed from a pipe or console output.<br>`System.ObjectDisposedException`：Methods were called after the stream was closed. |
| <a id="api-44ad0d683b5fdd8b"></a>[`public override void Inno.Core.IO.OwnedReadStream.Write(byte[] buffer, int offset, int count)`](../../src/foundation/core/Inno.Core.IO/OwnedReadStream.cs#L89) | When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.<br>`buffer`：An array of bytes. This method copies count bytes from buffer to the current stream.<br>`offset`：The zero-based byte offset in buffer at which to begin copying bytes to the current stream.<br>`count`：The number of bytes to be written to the current stream. | 约束由所属类型与本页生命周期定义。 | `System.ArgumentException`：The sum of offset and count is greater than the buffer length.<br>`System.ArgumentNullException`：buffer is null.<br>`System.ArgumentOutOfRangeException`：offset or count is negative.<br>`System.IO.IOException`：An I/O error occurred, such as the specified file cannot be found.<br>`System.NotSupportedException`：The stream does not support writing.<br>`System.ObjectDisposedException`：M:System.IO.Stream.Write(System.Byte[],System.Int32,System.Int32) was called after the stream was closed. |
| <a id="api-702d8f2b6fd9e965"></a>[`public override bool Inno.Core.IO.OwnedReadStream.CanRead { get; }`](../../src/foundation/core/Inno.Core.IO/OwnedReadStream.cs#L43) | When overridden in a derived class, gets a value indicating whether the current stream supports reading.<br>返回：true if the stream supports reading; otherwise, false. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-12772094ae010f1d"></a>[`public override bool Inno.Core.IO.OwnedReadStream.CanSeek { get; }`](../../src/foundation/core/Inno.Core.IO/OwnedReadStream.cs#L45) | When overridden in a derived class, gets a value indicating whether the current stream supports seeking.<br>返回：true if the stream supports seeking; otherwise, false. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-7e19233d935b077f"></a>[`public override bool Inno.Core.IO.OwnedReadStream.CanWrite { get; }`](../../src/foundation/core/Inno.Core.IO/OwnedReadStream.cs#L47) | When overridden in a derived class, gets a value indicating whether the current stream supports writing.<br>返回：true if the stream supports writing; otherwise, false. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-7fdc4f9a5da4a54d"></a>[`public override long Inno.Core.IO.OwnedReadStream.Length { get; }`](../../src/foundation/core/Inno.Core.IO/OwnedReadStream.cs#L49) | When overridden in a derived class, gets the length in bytes of the stream.<br>返回：A long value representing the length of the stream in bytes. | 约束由所属类型与本页生命周期定义。 | `System.NotSupportedException`：A class derived from Stream does not support seeking and the length is unknown.<br>`System.ObjectDisposedException`：Methods were called after the stream was closed. |
| <a id="api-4bf51a842179227c"></a>[`public override long Inno.Core.IO.OwnedReadStream.Position { get; set; }`](../../src/foundation/core/Inno.Core.IO/OwnedReadStream.cs#L51) | When overridden in a derived class, gets or sets the position within the current stream.<br>返回：The current position within the stream. | 约束由所属类型与本页生命周期定义。 | `System.IO.IOException`：An I/O error occurs.<br>`System.NotSupportedException`：The stream does not support seeking.<br>`System.ObjectDisposedException`：Methods were called after the stream was closed. |
| <a id="api-dceb4780149c0e51"></a>[`public sealed class Inno.Core.IO.OwnedReadStream : System.IO.Stream`](../../src/foundation/core/Inno.Core.IO/OwnedReadStream.cs#L11) | Keeps an independent read stream and its lifetime pin owned together until both retire. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Core.IO.PathBoundary`

<!-- api: T:Inno.Core.IO.PathBoundary 0ad3e89ceadb1714e27e89c0d056376b5a84c4e64d9e9a2dafae34bb4fea6a7e -->
<!-- api: M:Inno.Core.IO.PathBoundary.Resolve(System.String,System.String) cc99c2d6f00316405ddf0393de8ec8c9f6a7bfdc74bb87140a7c775f36421f15 -->
<!-- api: M:Inno.Core.IO.PathBoundary.RequireContained(System.String,System.String) 37c0667f4908b4082cab988fe20ca9c7643ae7de2e0495f6a192988f9b817760 -->
<!-- api: M:Inno.Core.IO.PathBoundary.RequireUnlinkedPath(System.String,System.String) 5ad613260e635819e1900f9b4b60f767e3648ca3ced8872adb54470dd1688245 -->
<!-- api: M:Inno.Core.IO.PathBoundary.EnumerateFiles(System.String) 040f40bd02c1fe7f75238b925d0d710c6a0cc2b551d6f4d0e1d485086bcb7c82 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d8f02a011244d08f"></a>[`public static System.Collections.Generic.IEnumerable<string> Inno.Core.IO.PathBoundary.EnumerateFiles(string root)`](../../src/foundation/core/Inno.Core.IO/PathBoundary.cs#L125) | Enumerates regular files in an owned tree without following filesystem links.<br>返回：Absolute file paths in unspecified order; enumeration does not retain open file handles.<br>`root`：The existing physical root whose files and subdirectories belong to the caller. | 约束由所属类型与本页生命周期定义。 | `System.IO.IOException`：The root or any entry is a symbolic link, junction or other reparse point, or cannot be inspected.<br>`System.IO.DirectoryNotFoundException`：The root does not exist. |
| <a id="api-c6174c709948650d"></a>[`public static string Inno.Core.IO.PathBoundary.RequireContained(string root, string path)`](../../src/foundation/core/Inno.Core.IO/PathBoundary.cs#L50) | Validates and normalizes an absolute path beneath a root.<br>返回：The normalized absolute contained path.<br>`root`：The owning root directory.<br>`path`：The path to validate. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-85d791b42e880687"></a>[`public static string Inno.Core.IO.PathBoundary.RequireUnlinkedPath(string root, string path)`](../../src/foundation/core/Inno.Core.IO/PathBoundary.cs#L81) | Resolves an owned path while rejecting existing links in its containment chain.<br>返回：The normalized contained path after every existing entry from the root has been inspected.<br>`root`：The trusted ownership root, which may not exist yet.<br>`path`：The absolute existing or future path beneath that root. | This validates the observed filesystem state. Concurrent mutations require an owner lease; it does not grant protection against unrelated actors replacing paths after validation. | `System.IO.IOException`：The path escapes the boundary or an existing entry is a link or reparse point. |
| <a id="api-2e31127307b4c891"></a>[`public static string Inno.Core.IO.PathBoundary.Resolve(string root, string relativePath)`](../../src/foundation/core/Inno.Core.IO/PathBoundary.cs#L24) | Resolves a relative path beneath a root and rejects traversal outside that root.<br>返回：The normalized absolute contained path.<br>`root`：The owning root directory.<br>`relativePath`：The relative path to resolve. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-6cedee446ec53227"></a>[`public static class Inno.Core.IO.PathBoundary`](../../src/foundation/core/Inno.Core.IO/PathBoundary.cs#L10) | Resolves paths while enforcing an explicit filesystem ownership boundary. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Core.IO.ReadOnlyByteDocumentStore`

<!-- api: T:Inno.Core.IO.ReadOnlyByteDocumentStore 2579adb0e50827ae565cccbd40d22dc2f9a5ed3d4ee758cc104830763e2bb77b -->
<!-- api: M:Inno.Core.IO.ReadOnlyByteDocumentStore.#ctor(System.String,System.ReadOnlySpan{System.Byte}) c8a600d5223525fe0407f110ee9ebaaf003e26f9b9f589cf00885e35b3cc1f32 -->
<!-- api: P:Inno.Core.IO.ReadOnlyByteDocumentStore.documentName 789b243382b964998b88f5226becb0c1f07138a091246dbd5a4cab253855f58c -->
<!-- api: P:Inno.Core.IO.ReadOnlyByteDocumentStore.canWrite 88afdd37cc4135344ae60bb90210ce1bcb1f400a66ea877644b3382b9293c698 -->
<!-- api: P:Inno.Core.IO.ReadOnlyByteDocumentStore.exists 871224ae479290eb0b95d8aacf9ba501367594ae2f01e2adab3ebff7374e9685 -->
<!-- api: M:Inno.Core.IO.ReadOnlyByteDocumentStore.Read c8388f1c55bd8f9e78de4f27070f31a776c0a49ed732e42d55090534fc2e398d -->
<!-- api: M:Inno.Core.IO.ReadOnlyByteDocumentStore.Write(System.ReadOnlySpan{System.Byte}) a69e8430b6d34a668670ab5d5bf43e14d98fd139bd9ac9aa83e70c926f609047 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e63cea19f9b54d09"></a>[`public Inno.Core.IO.ReadOnlyByteDocumentStore.ReadOnlyByteDocumentStore(string documentName, System.ReadOnlySpan<byte> data)`](../../src/foundation/core/Inno.Core.IO/ReadOnlyByteDocumentStore.cs#L21) | Copies one complete document into this source.<br>`documentName`：The stable logical diagnostic name.<br>`data`：Borrowed bytes copied before construction completes. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-f5b9e715ccf93fe8"></a>[`public byte[] Inno.Core.IO.ReadOnlyByteDocumentStore.Read()`](../../src/foundation/core/Inno.Core.IO/ReadOnlyByteDocumentStore.cs#L40) | Obtains a newly owned complete document snapshot.<br>返回：Owned document bytes, or null when no document exists; an empty document remains distinct from absence. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-eda866d4161b8a09"></a>[`public void Inno.Core.IO.ReadOnlyByteDocumentStore.Write(System.ReadOnlySpan<byte> data)`](../../src/foundation/core/Inno.Core.IO/ReadOnlyByteDocumentStore.cs#L43) | Replaces the document atomically after preparation succeeds.<br>`data`：The complete candidate bytes; the implementation cannot retain borrowed caller memory. | 约束由所属类型与本页生命周期定义。 | `System.NotSupportedException`：The source is read-only. |
| <a id="api-c2f61e390ef6225d"></a>[`public bool Inno.Core.IO.ReadOnlyByteDocumentStore.canWrite { get; }`](../../src/foundation/core/Inno.Core.IO/ReadOnlyByteDocumentStore.cs#L34) | Gets whether this source permits atomic replacement. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-763f0addb751a265"></a>[`public string Inno.Core.IO.ReadOnlyByteDocumentStore.documentName { get; }`](../../src/foundation/core/Inno.Core.IO/ReadOnlyByteDocumentStore.cs#L31) | Gets a stable diagnostic name for this document, independent of its storage implementation. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-27a9c89cca91c402"></a>[`public bool Inno.Core.IO.ReadOnlyByteDocumentStore.exists { get; }`](../../src/foundation/core/Inno.Core.IO/ReadOnlyByteDocumentStore.cs#L37) | Gets whether a document currently exists; callers must still handle absence during a later read. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-2b41b1ce4515b100"></a>[`public sealed class Inno.Core.IO.ReadOnlyByteDocumentStore : Inno.Core.IO.IByteDocumentStore`](../../src/foundation/core/Inno.Core.IO/ReadOnlyByteDocumentStore.cs#L8) | Owns an immutable document snapshot that cannot be replaced through its read boundary. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

## 项目依赖

- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。


## 进程与构建所有权 API

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-87ebbef5c9f0f02d"></a>[`public sealed class Inno.Core.IO.ProcessOwnedDirectory : System.IDisposable`](../../src/foundation/core/Inno.Core.IO/ProcessOwnedDirectory.cs#L16) | 首次写入前建立所有权，并在正常结束或异常进程退出后安全退休临时目录。 | 目录 claim 属于明确 producer；子进程必须在消费前登记。 | 越界、链接、无记录或不可验证的 owner 拒绝使用/退休。 |
| <a id="api-eed5f32b6f829b08"></a>[`public string Inno.Core.IO.ProcessOwnedDirectory.directory { get; }`](../../src/foundation/core/Inno.Core.IO/ProcessOwnedDirectory.cs#L40) | 明确 producer 根下的绝对 payload 目录。 | 目录 claim 属于明确 producer；子进程必须在消费前登记。 | 越界、链接、无记录或不可验证的 owner 拒绝使用/退休。 |
| <a id="api-0df8fee3aadaeed1"></a>[`public static System.Threading.Tasks.ValueTask<string> Inno.Core.IO.ProcessOwnedDirectory.CreateForProcessAsync(string root, string identity, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/foundation/core/Inno.Core.IO/ProcessOwnedDirectory.cs#L68) | 按当前操作的 32 位十六进制身份，在首次 payload 前登记进程；允许同一活跃操作的参与进程登记，不接管既有无登记目录。 | 返回路径由进程生命周期保护，不把托管 lease 交给 SDK registry。 | 越界、链接、无记录或不可验证的 owner 拒绝使用/退休。<br>`System.IO.InvalidDataException`：当前 claim 损坏、身份不一致，或无法授权本次操作。 |
| <a id="api-1813e712c22e926e"></a>[`public static System.Threading.Tasks.ValueTask<Inno.Core.IO.ProcessOwnedDirectory> Inno.Core.IO.ProcessOwnedDirectory.CreateAsync(string root, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/foundation/core/Inno.Core.IO/ProcessOwnedDirectory.cs#L122) | 创建新 GUID 目录，先写 claim 再创建 payload；返回独占、可释放的目录 owner。 | 调用方在所有子进程结束后 Dispose；取消不返回半个 owner。 | 越界、链接、无记录或不可验证的 owner 拒绝使用/退休。 |
| <a id="api-171b96d1bdc39c2f"></a>[`public static void Inno.Core.IO.ProcessOwnedDirectory.RegisterCurrentProcess(string root, string directory)`](../../src/foundation/core/Inno.Core.IO/ProcessOwnedDirectory.cs#L172) | 为已存在的当前 claim 登记 PID 和 UTC 启动 ticks；不创建或认领目录。 | 目录 claim 属于明确 producer；子进程必须在消费前登记。 | 越界、链接、无记录或不可验证的 owner 拒绝使用/退休。<br>`System.IO.InvalidDataException`：当前 claim 损坏、身份不一致，或无法授权本次操作。 |
| <a id="api-2d160976e9cf0d57"></a>[`public static System.Collections.Generic.IReadOnlyList<Inno.Core.IO.ProcessOwnedDirectoryDescriptor> Inno.Core.IO.ProcessOwnedDirectory.ReadClaims(string root, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/foundation/core/Inno.Core.IO/ProcessOwnedDirectory.cs#L200) | 读取明确 claim，返回目录、协调路径及合并的保守进程状态；不通过目录名推测所有权。 | 目录 claim 属于明确 producer；子进程必须在消费前登记。 | 越界、链接、无记录或不可验证的 owner 拒绝使用/退休。<br>`System.IO.InvalidDataException`：当前 claim 损坏、身份不一致，或无法授权本次操作。 |
| <a id="api-6c41eb0c5c0d5ce2"></a>[`public void Inno.Core.IO.ProcessOwnedDirectory.Dispose()`](../../src/foundation/core/Inno.Core.IO/ProcessOwnedDirectory.cs#L226) | 其他登记进程均退出后删除自己的临时 payload 与 claim；persistent lock 保留。 | 目录 claim 属于明确 producer；子进程必须在消费前登记。 | 活动/Unknown 子进程或 IO 失败时保留 claim 并抛 IOException，之后由 producer 退休。 |
| <a id="api-f6f890bacaa026b7"></a>[`public sealed class Inno.Core.IO.ProcessOwnedDirectoryDescriptor`](../../src/foundation/core/Inno.Core.IO/ProcessOwnedDirectoryDescriptor.cs#L6) | 只读 claim 检查结果；不是可自行释放目录的 owner。 | 目录 claim 属于明确 producer；子进程必须在消费前登记。 | 越界、链接、无记录或不可验证的 owner 拒绝使用/退休。 |
| <a id="api-a19255fa13cd7def"></a>[`public string Inno.Core.IO.ProcessOwnedDirectoryDescriptor.directory { get; }`](../../src/foundation/core/Inno.Core.IO/ProcessOwnedDirectoryDescriptor.cs#L21) | 明确 producer 根下的绝对 payload 目录。 | 目录 claim 属于明确 producer；子进程必须在消费前登记。 | 越界、链接、无记录或不可验证的 owner 拒绝使用/退休。 |
| <a id="api-d855aff025d9eb01"></a>[`public string Inno.Core.IO.ProcessOwnedDirectoryDescriptor.leasePath { get; }`](../../src/foundation/core/Inno.Core.IO/ProcessOwnedDirectoryDescriptor.cs#L26) | 读取/退休该临时目录必须协调的绝对 lease 路径。 | 目录 claim 属于明确 producer；子进程必须在消费前登记。 | 越界、链接、无记录或不可验证的 owner 拒绝使用/退休。 |
| <a id="api-68f00a21fecc1af8"></a>[`public Inno.Core.IO.ProcessOwnerState Inno.Core.IO.ProcessOwnedDirectoryDescriptor.ownerState { get; }`](../../src/foundation/core/Inno.Core.IO/ProcessOwnedDirectoryDescriptor.cs#L31) | 所有已登记参与进程的合并状态；只有 Exited 允许自动退休。 | 目录 claim 属于明确 producer；子进程必须在消费前登记。 | 越界、链接、无记录或不可验证的 owner 拒绝使用/退休。 |
| <a id="api-7f2f8185cdf95dcb"></a>[`public readonly record struct Inno.Core.IO.ProcessOwnerStamp : System.IEquatable<Inno.Core.IO.ProcessOwnerStamp>`](../../src/foundation/core/Inno.Core.IO/ProcessOwnerStamp.cs#L10) | 不持有 Process 对象的精确进程生命周期身份。 | 不可变元数据；不持有进程或文件 lease。 | 值查询没有写入副作用。 |
| <a id="api-a576197e6f4fdf71"></a>[`public Inno.Core.IO.ProcessOwnerStamp.ProcessOwnerStamp(int processId, long startedUtcTicks)`](../../src/foundation/core/Inno.Core.IO/ProcessOwnerStamp.cs#L24) | 冻结正 PID 与有效 UTC 启动 ticks；PID 相同而启动时间不同视为不同进程。 | 不可变元数据；不持有进程或文件 lease。 | 无效 PID/ticks 抛 ArgumentOutOfRangeException。 |
| <a id="api-237d1dc4b9fcc9ad"></a>[`public int Inno.Core.IO.ProcessOwnerStamp.processId { get; }`](../../src/foundation/core/Inno.Core.IO/ProcessOwnerStamp.cs#L39) | OS PID；必须与 startedUtcTicks 联合使用。 | 不可变元数据；不持有进程或文件 lease。 | 值查询没有写入副作用。 |
| <a id="api-32d7270ae6ab9208"></a>[`public long Inno.Core.IO.ProcessOwnerStamp.startedUtcTicks { get; }`](../../src/foundation/core/Inno.Core.IO/ProcessOwnerStamp.cs#L44) | 进程 UTC 启动时间 ticks，与 PID 一起识别生命周期。 | 不可变元数据；不持有进程或文件 lease。 | 值查询没有写入副作用。 |
| <a id="api-01d9211921710308"></a>[`public static Inno.Core.IO.ProcessOwnerStamp Inno.Core.IO.ProcessOwnerStamp.CaptureCurrent()`](../../src/foundation/core/Inno.Core.IO/ProcessOwnerStamp.cs#L58) | 读取当前 OS 进程的 PID 和启动时间，返回值身份并释放临时 Process 对象。 | 不可变元数据；不持有进程或文件 lease。 | 不支持进程生命周期时抛 PlatformNotSupportedException；OS 查询失败抛 Win32Exception。 |
| <a id="api-55ed7b0446634bc4"></a>[`public Inno.Core.IO.ProcessOwnerState Inno.Core.IO.ProcessOwnerStamp.Query()`](../../src/foundation/core/Inno.Core.IO/ProcessOwnerStamp.cs#L71) | 精确生命周期仍运行返回 Active；不存在、已退出或 PID 被复用返回 Exited；无法确认返回 Unknown。 | 不可变元数据；不持有进程或文件 lease。 | 权限拒绝、默认无效值或不可确认状态不能作为删除许可。 |
| <a id="api-b850d12b46102d03"></a>[`public override string Inno.Core.IO.ProcessOwnerStamp.ToString()`](../../src/foundation/core/Inno.Core.IO/ProcessOwnerStamp.cs#L10) | record 值的文本表示，包含 PID 与启动 ticks；不用于持久路由。 | 不可变元数据；不持有进程或文件 lease。 | 值查询没有写入副作用。 |
| <a id="api-a08f1f92d3fe367c"></a>[`public static bool Inno.Core.IO.ProcessOwnerStamp.operator !=(Inno.Core.IO.ProcessOwnerStamp left, Inno.Core.IO.ProcessOwnerStamp right)`](../../src/foundation/core/Inno.Core.IO/ProcessOwnerStamp.cs#L10) | record 值比较/哈希同时使用 PID 与启动 ticks，不执行进程存活查询。 | 不可变元数据；不持有进程或文件 lease。 | 值查询没有写入副作用。 |
| <a id="api-f96efba737217cbc"></a>[`public static bool Inno.Core.IO.ProcessOwnerStamp.operator ==(Inno.Core.IO.ProcessOwnerStamp left, Inno.Core.IO.ProcessOwnerStamp right)`](../../src/foundation/core/Inno.Core.IO/ProcessOwnerStamp.cs#L10) | record 值比较/哈希同时使用 PID 与启动 ticks，不执行进程存活查询。 | 不可变元数据；不持有进程或文件 lease。 | 值查询没有写入副作用。 |
| <a id="api-e8063f74cf8bf03e"></a>[`public override int Inno.Core.IO.ProcessOwnerStamp.GetHashCode()`](../../src/foundation/core/Inno.Core.IO/ProcessOwnerStamp.cs#L10) | record 值比较/哈希同时使用 PID 与启动 ticks，不执行进程存活查询。 | 不可变元数据；不持有进程或文件 lease。 | 值查询没有写入副作用。 |
| <a id="api-d1355b2550b12c95"></a>[`public override bool Inno.Core.IO.ProcessOwnerStamp.Equals(object obj)`](../../src/foundation/core/Inno.Core.IO/ProcessOwnerStamp.cs#L10) | record 值比较/哈希同时使用 PID 与启动 ticks，不执行进程存活查询。 | 不可变元数据；不持有进程或文件 lease。 | 值查询没有写入副作用。 |
| <a id="api-fc8e523a89f9c32e"></a>[`public bool Inno.Core.IO.ProcessOwnerStamp.Equals(Inno.Core.IO.ProcessOwnerStamp other)`](../../src/foundation/core/Inno.Core.IO/ProcessOwnerStamp.cs#L10) | record 值比较/哈希同时使用 PID 与启动 ticks，不执行进程存活查询。 | 不可变元数据；不持有进程或文件 lease。 | 值查询没有写入副作用。 |
| <a id="api-a4c34d306c8b3c98"></a>[`public enum Inno.Core.IO.ProcessOwnerState : int`](../../src/foundation/core/Inno.Core.IO/ProcessOwnerState.cs#L6) | 区分活动、已退出与无法确认；Unknown 不是退出。 | 不可变元数据；不持有进程或文件 lease。 | 值查询没有写入副作用。 |
| <a id="api-846ad5bf06cee1e3"></a>[`Inno.Core.IO.ProcessOwnerState.Active = 0`](../../src/foundation/core/Inno.Core.IO/ProcessOwnerState.cs#L11) | 匹配的进程生命周期仍运行，保护其目录。 | 不可变元数据；不持有进程或文件 lease。 | 值查询没有写入副作用。 |
| <a id="api-de87ac83d742c5a5"></a>[`Inno.Core.IO.ProcessOwnerState.Exited = 1`](../../src/foundation/core/Inno.Core.IO/ProcessOwnerState.cs#L16) | 已证明原进程生命周期结束，可继续协调退休。 | 不可变元数据；不持有进程或文件 lease。 | 值查询没有写入副作用。 |
| <a id="api-618f147e8fbaabb7"></a>[`Inno.Core.IO.ProcessOwnerState.Unknown = 2`](../../src/foundation/core/Inno.Core.IO/ProcessOwnerState.cs#L21) | 身份或权限无法确认，必须保留目录。 | 不可变元数据；不持有进程或文件 lease。 | 值查询没有写入副作用。 |

<!-- api: T:Inno.Core.IO.ProcessOwnedDirectory 294f9bfed71e69c62253d904ec73ebe0ac2fd6e2ed5e4cf2ac45a22faaef5908 -->
<!-- api: P:Inno.Core.IO.ProcessOwnedDirectory.directory dfb650555c5ee4fab5e8efb406b368c1bcb167e5c04271e6bdcb5777ec98eacb -->
<!-- api: M:Inno.Core.IO.ProcessOwnedDirectory.CreateForProcessAsync(System.String,System.String,System.Threading.CancellationToken) c0a8a979c537bbd1a06bdaf2d9ef62f343b7d47b35e3ee13d4fcf16cb0c127f1 -->
<!-- api: M:Inno.Core.IO.ProcessOwnedDirectory.CreateAsync(System.String,System.Threading.CancellationToken) 889d45fadf2cbecefc6cf952ddc13689c47c8f5a0f1ecd4ac97568785caf69bc -->
<!-- api: M:Inno.Core.IO.ProcessOwnedDirectory.RegisterCurrentProcess(System.String,System.String) a684d6ce2b36c1b367677820f95c078d1b33fd2fe3b9d9cd8d2efbb08a1743c8 -->
<!-- api: M:Inno.Core.IO.ProcessOwnedDirectory.ReadClaims(System.String,System.Threading.CancellationToken) afa088e7302dbd2fb5feb74846dacbee5901ed9728517eebd94aa2a22c120f13 -->
<!-- api: M:Inno.Core.IO.ProcessOwnedDirectory.Dispose 862b400cced4844e46f0ddc44d8d1616dde42e404b64dbebec7c90237a88c9b5 -->
<!-- api: T:Inno.Core.IO.ProcessOwnedDirectoryDescriptor 9b6d473e84cc393e61ca23c8786e22b5f38a3d430a238f73b96ef1efa4b84c65 -->
<!-- api: P:Inno.Core.IO.ProcessOwnedDirectoryDescriptor.directory 967b3bd74e180661c6ef2012c75820577a3cb4538d39a7a44413491a87219c49 -->
<!-- api: P:Inno.Core.IO.ProcessOwnedDirectoryDescriptor.leasePath 52fc0b6f12d4f1f70cf6557f0d399c0c8b1750a44f12a56267909d4770b8a7fa -->
<!-- api: P:Inno.Core.IO.ProcessOwnedDirectoryDescriptor.ownerState 0c52a52a23f5b022f9b74ea0bcccf891ee22edf244fe5d086c8871a25f71df7a -->
<!-- api: T:Inno.Core.IO.ProcessOwnerStamp 0977df34360c86333460bdac5d71bbf1d41bc814a9be2bcb449dea602a254878 -->
<!-- api: M:Inno.Core.IO.ProcessOwnerStamp.#ctor(System.Int32,System.Int64) a2f06260165c37ec0b8b11ce583ee8876e4f12cc55d501b92cd9964a20dba798 -->
<!-- api: P:Inno.Core.IO.ProcessOwnerStamp.processId 63c33a420485b05bf86608d5ab7814d4e1a6c79aad8dc9dade6e32d00aae05c3 -->
<!-- api: P:Inno.Core.IO.ProcessOwnerStamp.startedUtcTicks 14c05ad68e01438544d82298082e1b68420fc3af6ec929ad091ea1b91049d758 -->
<!-- api: M:Inno.Core.IO.ProcessOwnerStamp.CaptureCurrent 5291ac1399ce2bbd0c1fd5f0d841beba6f626f2e22249f5609528d611c6361d4 -->
<!-- api: M:Inno.Core.IO.ProcessOwnerStamp.Query 9017ed62b3dfb691ebfb91478dcf443a7e906c0cb2d4fbb1a563ba2c90d3a0f8 -->
<!-- api: M:Inno.Core.IO.ProcessOwnerStamp.ToString d4808c98063fbd9fcc3aa8ba84d730226ef190d0754180d774826e3656d2407b -->
<!-- api: M:Inno.Core.IO.ProcessOwnerStamp.op_Inequality(Inno.Core.IO.ProcessOwnerStamp,Inno.Core.IO.ProcessOwnerStamp) 7782b229a1774fa0fa74efab348aced7b5b693492ac62b45987c879973aa9b37 -->
<!-- api: M:Inno.Core.IO.ProcessOwnerStamp.op_Equality(Inno.Core.IO.ProcessOwnerStamp,Inno.Core.IO.ProcessOwnerStamp) 7a8e4c70cf68b19e444b48ccc46522d6e2a582fdb9282bb3952e00eb11a71175 -->
<!-- api: M:Inno.Core.IO.ProcessOwnerStamp.GetHashCode f774573e60af1955c25b3fba2bd7b5c77481700ff7ce05c4f230c809fd0697c9 -->
<!-- api: M:Inno.Core.IO.ProcessOwnerStamp.Equals(System.Object) e0dbf5c3e0f7c925713ff655d1fe4f0b55d1519f7974d638864649d0aa2fa72d -->
<!-- api: M:Inno.Core.IO.ProcessOwnerStamp.Equals(Inno.Core.IO.ProcessOwnerStamp) 1b5398fb5c1e4b4d22c919883da0a813c90b10cab857764cd0caa614c6c73798 -->
<!-- api: T:Inno.Core.IO.ProcessOwnerState e4bb5a252ed7a55b46dcb95fd6f9e37f4ccf584e7298496f31426beeebfc7a3b -->
<!-- api: F:Inno.Core.IO.ProcessOwnerState.Active 9250ebf74d79d28f36fb73eeae6e73cbb7dbf627bd28f89501f71556b5f8914a -->
<!-- api: F:Inno.Core.IO.ProcessOwnerState.Exited 0bb1a037f834d7fff9b9d43172cf9c500301dc02de51a9caa59d2316a7b428fe -->
<!-- api: F:Inno.Core.IO.ProcessOwnerState.Unknown 230441c073783d8f482c910eebd83a605e7b49fc4663e09024ff8bfbe195a869 -->

### Unix 文件租约竞争

`FileLease` 只对真实的共享/独占竞争执行有界等待和取消。Windows 使用 sharing/lock violation；.NET 的 Unix `flock` 竞争异常携带原始 errno，macOS 为 35。权限、路径和其他 IO 错误立即返回，锁文件保持原位，避免 Unix inode 被替换后出现双 owner。
