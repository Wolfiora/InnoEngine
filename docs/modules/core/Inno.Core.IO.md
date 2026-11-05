# Inno.Core.IO

[分类索引](README.md) · [Wiki 首页](../../README.md)

## 职责与边界

提供原子文件/目录发布、明确文件 lease、路径边界、小型 byte document 与独立 stream pin。领域模型通过文档或内容契约读取，不把物理位置泄漏给共享 Runtime。

## 所有权与失败

`FileLease` 支持共享读取与独占写入协调；Windows 与 Unix 遵循同一语义。锁文件保留，不能删除锁文件代替解锁。等待受取消与有界 timeout 控制。

`PhysicalPathResolver` 是每个读取阶段独立持有的路径解析器，复用已观察的 link/junction 关系。新的校验阶段必须使用新实例；解析结果不构成写入或删除权限，仍要经过 `PathBoundary` 与相应 owner lease。Native 输入读取和构建根选择共用该实现。

`IByteDocumentStore` 仅用于小型文档。`FileByteDocumentStore` 复用 AtomicFile；`ReadOnlyByteDocumentStore` 捕获 owned bytes，写入明确失败。它不是 Asset catalog 或事件协议。

`FileByteDocumentStore.Read` 用同一个声明读取和删除共享的句柄固定当前发布文件，完整读取后才释放；返回的 bytes 独立归调用方。Windows 原子重命名的目标名称可能在系统关闭重命名句柄前可见，读取方必须允许该句柄完成；这不表示允许原地写入。写入方始终原子替换文件，读取期间替换不改变已打开句柄选中的内容。缺失返回 null，空文档返回空数组，其他读取失败及过大的文档明确抛出异常，不以重试或空内容掩盖错误。

`AtomicFile` 只发布已经完整写入的同目录候选。`overwrite: false` 的冲突检查和发布必须是同一个文件系统操作，不能先检查不存在再 rename。Windows 使用系统的不可覆盖移动；Unix 使用公开 POSIX `link` 原语原子创建目标，再退休候选，不调用 SDK 私有导出。此处只选择宿主文件系统原语，不选择引擎 target、backend 或产品平台。Unix 文件系统必须支持同目录 hard link；不支持时明确抛出 `IOException`，不回退成可能暴露部分内容的复制。不能提供该宿主原语的执行环境明确抛出 `PlatformNotSupportedException`，其 inner exception 保留能力缺失原因；不维护引擎平台名单。普通原子替换不受影响。

`Install` 在发布失败时保留调用方候选；发布成功后移除候选。若目标已经发布而候选删除失败，异常明确说明目标完整存在，调用方不能重试覆盖他人的目标。`WriteAllBytes` 自己拥有 staging 并负责在退出时清理。空的现有文档也属于已有目标，不能因为长度为零而覆盖。父目录和候选仍需要调用方的所有权保护；原子发布不抵御非合作进程替换整棵父目录。

`AtomicFile.IsCandidatePath` 只识别本 publisher 保留的完整 staging 文件名，供 source discovery 排除尚未发布的临时 bytes。识别名称不授予读取、清理或删除权限。

`AtomicDirectory.Publish` 只向不存在的目标发布完整 staging。需要替换目录时使用 `Install`，由调用方先协调全部读写 owner；这两个原子目录入口不会自行取得读取 lease。提交后的旧目录清理失败与提交前失败具有不同结果，不能把清理异常解释为新产物未安装。

`AtomicDirectory.Install(source, destination, cancellationToken)` 沿用 Core 的有界 rename 策略。准备或提交前等待可取消；旧目标已移到 backup 时，恢复使用不受已取消 token 影响的同一文件系统协议。提交后的取消不撤销完整产物，backup 清理失败仍通过 `DirectoryCleanupException` 区分。若安装和恢复都失败，`AggregateException` 保留两次失败及旧目录位置，不能删除候选或旧目录伪装恢复成功。已有调用方省略 token 时保持不可取消的调用语义，不保留第二个 overload。

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

## API 参考

[完整声明、行为与调用约束](Inno.Core.IO/api.md)

<!-- api-include: Inno.Core.IO/api.md -->

## 项目依赖

- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。


## 进程与构建所有权 API

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-87ebbef5c9f0f02d"></a>[`public sealed class Inno.Core.IO.ProcessOwnedDirectory : System.IDisposable`](../../../src/foundation/core/Inno.Core.IO/ProcessOwnedDirectory.cs#L16) | 首次写入前建立所有权，并在正常结束或异常进程退出后安全退休临时目录。 | 目录 claim 属于明确 producer；子进程必须在消费前登记。 | 越界、链接、无记录或不可验证的 owner 拒绝使用/退休。 |
| <a id="api-eed5f32b6f829b08"></a>[`public string Inno.Core.IO.ProcessOwnedDirectory.directory { get; }`](../../../src/foundation/core/Inno.Core.IO/ProcessOwnedDirectory.cs#L40) | 明确 producer 根下的绝对 payload 目录。 | 目录 claim 属于明确 producer；子进程必须在消费前登记。 | 越界、链接、无记录或不可验证的 owner 拒绝使用/退休。 |
| <a id="api-0df8fee3aadaeed1"></a>[`public static System.Threading.Tasks.ValueTask<string> Inno.Core.IO.ProcessOwnedDirectory.CreateForProcessAsync(string root, string identity, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../src/foundation/core/Inno.Core.IO/ProcessOwnedDirectory.cs#L68) | 按当前操作的 32 位十六进制身份，在首次 payload 前登记进程；允许同一活跃操作的参与进程登记，不接管既有无登记目录。 | 返回路径由进程生命周期保护，不把托管 lease 交给 SDK registry。 | 越界、链接、无记录或不可验证的 owner 拒绝使用/退休。<br>`System.IO.InvalidDataException`：当前 claim 损坏、身份不一致，或无法授权本次操作。 |
| <a id="api-1813e712c22e926e"></a>[`public static System.Threading.Tasks.ValueTask<Inno.Core.IO.ProcessOwnedDirectory> Inno.Core.IO.ProcessOwnedDirectory.CreateAsync(string root, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../src/foundation/core/Inno.Core.IO/ProcessOwnedDirectory.cs#L122) | 创建新 GUID 目录，先写 claim 再创建 payload；返回独占、可释放的目录 owner。 | 调用方在所有子进程结束后 Dispose；取消不返回半个 owner。 | 越界、链接、无记录或不可验证的 owner 拒绝使用/退休。 |
| <a id="api-171b96d1bdc39c2f"></a>[`public static void Inno.Core.IO.ProcessOwnedDirectory.RegisterCurrentProcess(string root, string directory)`](../../../src/foundation/core/Inno.Core.IO/ProcessOwnedDirectory.cs#L172) | 为已存在的当前 claim 登记 PID 和 UTC 启动 ticks；不创建或认领目录。 | 目录 claim 属于明确 producer；子进程必须在消费前登记。 | 越界、链接、无记录或不可验证的 owner 拒绝使用/退休。<br>`System.IO.InvalidDataException`：当前 claim 损坏、身份不一致，或无法授权本次操作。 |
| <a id="api-2d160976e9cf0d57"></a>[`public static System.Collections.Generic.IReadOnlyList<Inno.Core.IO.ProcessOwnedDirectoryDescriptor> Inno.Core.IO.ProcessOwnedDirectory.ReadClaims(string root, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../src/foundation/core/Inno.Core.IO/ProcessOwnedDirectory.cs#L200) | 读取明确 claim，返回目录、协调路径及合并的保守进程状态；不通过目录名推测所有权。 | 目录 claim 属于明确 producer；子进程必须在消费前登记。 | 越界、链接、无记录或不可验证的 owner 拒绝使用/退休。<br>`System.IO.InvalidDataException`：当前 claim 损坏、身份不一致，或无法授权本次操作。 |
| <a id="api-6c41eb0c5c0d5ce2"></a>[`public void Inno.Core.IO.ProcessOwnedDirectory.Dispose()`](../../../src/foundation/core/Inno.Core.IO/ProcessOwnedDirectory.cs#L226) | 其他登记进程均退出后删除自己的临时 payload 与 claim；persistent lock 保留。 | 目录 claim 属于明确 producer；子进程必须在消费前登记。 | 活动/Unknown 子进程或 IO 失败时保留 claim 并抛 IOException，之后由 producer 退休。 |
| <a id="api-f6f890bacaa026b7"></a>[`public sealed class Inno.Core.IO.ProcessOwnedDirectoryDescriptor`](../../../src/foundation/core/Inno.Core.IO/ProcessOwnedDirectoryDescriptor.cs#L6) | 只读 claim 检查结果；不是可自行释放目录的 owner。 | 目录 claim 属于明确 producer；子进程必须在消费前登记。 | 越界、链接、无记录或不可验证的 owner 拒绝使用/退休。 |
| <a id="api-a19255fa13cd7def"></a>[`public string Inno.Core.IO.ProcessOwnedDirectoryDescriptor.directory { get; }`](../../../src/foundation/core/Inno.Core.IO/ProcessOwnedDirectoryDescriptor.cs#L21) | 明确 producer 根下的绝对 payload 目录。 | 目录 claim 属于明确 producer；子进程必须在消费前登记。 | 越界、链接、无记录或不可验证的 owner 拒绝使用/退休。 |
| <a id="api-d855aff025d9eb01"></a>[`public string Inno.Core.IO.ProcessOwnedDirectoryDescriptor.leasePath { get; }`](../../../src/foundation/core/Inno.Core.IO/ProcessOwnedDirectoryDescriptor.cs#L26) | 读取/退休该临时目录必须协调的绝对 lease 路径。 | 目录 claim 属于明确 producer；子进程必须在消费前登记。 | 越界、链接、无记录或不可验证的 owner 拒绝使用/退休。 |
| <a id="api-68f00a21fecc1af8"></a>[`public Inno.Core.IO.ProcessOwnerState Inno.Core.IO.ProcessOwnedDirectoryDescriptor.ownerState { get; }`](../../../src/foundation/core/Inno.Core.IO/ProcessOwnedDirectoryDescriptor.cs#L31) | 所有已登记参与进程的合并状态；只有 Exited 允许自动退休。 | 目录 claim 属于明确 producer；子进程必须在消费前登记。 | 越界、链接、无记录或不可验证的 owner 拒绝使用/退休。 |
| <a id="api-7f2f8185cdf95dcb"></a>[`public readonly record struct Inno.Core.IO.ProcessOwnerStamp : System.IEquatable<Inno.Core.IO.ProcessOwnerStamp>`](../../../src/foundation/core/Inno.Core.IO/ProcessOwnerStamp.cs#L10) | 不持有 Process 对象的精确进程生命周期身份。 | 不可变元数据；不持有进程或文件 lease。 | 值查询没有写入副作用。 |
| <a id="api-a576197e6f4fdf71"></a>[`public Inno.Core.IO.ProcessOwnerStamp.ProcessOwnerStamp(int processId, long startedUtcTicks)`](../../../src/foundation/core/Inno.Core.IO/ProcessOwnerStamp.cs#L24) | 冻结正 PID 与有效 UTC 启动 ticks；PID 相同而启动时间不同视为不同进程。 | 不可变元数据；不持有进程或文件 lease。 | 无效 PID/ticks 抛 ArgumentOutOfRangeException。 |
| <a id="api-237d1dc4b9fcc9ad"></a>[`public int Inno.Core.IO.ProcessOwnerStamp.processId { get; }`](../../../src/foundation/core/Inno.Core.IO/ProcessOwnerStamp.cs#L39) | OS PID；必须与 startedUtcTicks 联合使用。 | 不可变元数据；不持有进程或文件 lease。 | 值查询没有写入副作用。 |
| <a id="api-32d7270ae6ab9208"></a>[`public long Inno.Core.IO.ProcessOwnerStamp.startedUtcTicks { get; }`](../../../src/foundation/core/Inno.Core.IO/ProcessOwnerStamp.cs#L44) | 进程 UTC 启动时间 ticks，与 PID 一起识别生命周期。 | 不可变元数据；不持有进程或文件 lease。 | 值查询没有写入副作用。 |
| <a id="api-01d9211921710308"></a>[`public static Inno.Core.IO.ProcessOwnerStamp Inno.Core.IO.ProcessOwnerStamp.CaptureCurrent()`](../../../src/foundation/core/Inno.Core.IO/ProcessOwnerStamp.cs#L58) | 读取当前 OS 进程的 PID 和启动时间，返回值身份并释放临时 Process 对象。 | 不可变元数据；不持有进程或文件 lease。 | 不支持进程生命周期时抛 PlatformNotSupportedException；OS 查询失败抛 Win32Exception。 |
| <a id="api-55ed7b0446634bc4"></a>[`public Inno.Core.IO.ProcessOwnerState Inno.Core.IO.ProcessOwnerStamp.Query()`](../../../src/foundation/core/Inno.Core.IO/ProcessOwnerStamp.cs#L71) | 精确生命周期仍运行返回 Active；不存在、已退出或 PID 被复用返回 Exited；无法确认返回 Unknown。 | 不可变元数据；不持有进程或文件 lease。 | 权限拒绝、默认无效值或不可确认状态不能作为删除许可。 |
| <a id="api-b850d12b46102d03"></a>[`public override string Inno.Core.IO.ProcessOwnerStamp.ToString()`](../../../src/foundation/core/Inno.Core.IO/ProcessOwnerStamp.cs#L10) | record 值的文本表示，包含 PID 与启动 ticks；不用于持久路由。 | 不可变元数据；不持有进程或文件 lease。 | 值查询没有写入副作用。 |
| <a id="api-a08f1f92d3fe367c"></a>[`public static bool Inno.Core.IO.ProcessOwnerStamp.operator !=(Inno.Core.IO.ProcessOwnerStamp left, Inno.Core.IO.ProcessOwnerStamp right)`](../../../src/foundation/core/Inno.Core.IO/ProcessOwnerStamp.cs#L10) | record 值比较/哈希同时使用 PID 与启动 ticks，不执行进程存活查询。 | 不可变元数据；不持有进程或文件 lease。 | 值查询没有写入副作用。 |
| <a id="api-f96efba737217cbc"></a>[`public static bool Inno.Core.IO.ProcessOwnerStamp.operator ==(Inno.Core.IO.ProcessOwnerStamp left, Inno.Core.IO.ProcessOwnerStamp right)`](../../../src/foundation/core/Inno.Core.IO/ProcessOwnerStamp.cs#L10) | record 值比较/哈希同时使用 PID 与启动 ticks，不执行进程存活查询。 | 不可变元数据；不持有进程或文件 lease。 | 值查询没有写入副作用。 |
| <a id="api-e8063f74cf8bf03e"></a>[`public override int Inno.Core.IO.ProcessOwnerStamp.GetHashCode()`](../../../src/foundation/core/Inno.Core.IO/ProcessOwnerStamp.cs#L10) | record 值比较/哈希同时使用 PID 与启动 ticks，不执行进程存活查询。 | 不可变元数据；不持有进程或文件 lease。 | 值查询没有写入副作用。 |
| <a id="api-d1355b2550b12c95"></a>[`public override bool Inno.Core.IO.ProcessOwnerStamp.Equals(object obj)`](../../../src/foundation/core/Inno.Core.IO/ProcessOwnerStamp.cs#L10) | record 值比较/哈希同时使用 PID 与启动 ticks，不执行进程存活查询。 | 不可变元数据；不持有进程或文件 lease。 | 值查询没有写入副作用。 |
| <a id="api-fc8e523a89f9c32e"></a>[`public bool Inno.Core.IO.ProcessOwnerStamp.Equals(Inno.Core.IO.ProcessOwnerStamp other)`](../../../src/foundation/core/Inno.Core.IO/ProcessOwnerStamp.cs#L10) | record 值比较/哈希同时使用 PID 与启动 ticks，不执行进程存活查询。 | 不可变元数据；不持有进程或文件 lease。 | 值查询没有写入副作用。 |
| <a id="api-a4c34d306c8b3c98"></a>[`public enum Inno.Core.IO.ProcessOwnerState : int`](../../../src/foundation/core/Inno.Core.IO/ProcessOwnerState.cs#L6) | 区分活动、已退出与无法确认；Unknown 不是退出。 | 不可变元数据；不持有进程或文件 lease。 | 值查询没有写入副作用。 |
| <a id="api-846ad5bf06cee1e3"></a>[`Inno.Core.IO.ProcessOwnerState.Active = 0`](../../../src/foundation/core/Inno.Core.IO/ProcessOwnerState.cs#L11) | 匹配的进程生命周期仍运行，保护其目录。 | 不可变元数据；不持有进程或文件 lease。 | 值查询没有写入副作用。 |
| <a id="api-de87ac83d742c5a5"></a>[`Inno.Core.IO.ProcessOwnerState.Exited = 1`](../../../src/foundation/core/Inno.Core.IO/ProcessOwnerState.cs#L16) | 已证明原进程生命周期结束，可继续协调退休。 | 不可变元数据；不持有进程或文件 lease。 | 值查询没有写入副作用。 |
| <a id="api-618f147e8fbaabb7"></a>[`Inno.Core.IO.ProcessOwnerState.Unknown = 2`](../../../src/foundation/core/Inno.Core.IO/ProcessOwnerState.cs#L21) | 身份或权限无法确认，必须保留目录。 | 不可变元数据；不持有进程或文件 lease。 | 值查询没有写入副作用。 |

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
