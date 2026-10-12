# Inno.Core.Logging

本轮收口新增 `DiagnosticLogSink(hub, logs)`：作为 `IDiagnosticSink` 接收 Replace/Clear，只把新增或变化的问题写入 LogRouter；Dispose 注销并释放去重快照。hub 与 logs 由 Host 显式持有，必须比 sink 长寿。

`LogRouter(queueCapacity = 65536, drainBudget = 4096)` 限制积压与单批排空。`TryDispatch(entry)` 在容量不足时返回 false，`Dispatch(entry)` 明确抛出；Flush 所需 barrier 也受容量限制，满时需要由调用方重试。不能在日志 worker 自己的 callback 中等待自身退出。

[上一页：Diagnose](Inno.Core.Diagnostics.md) · [Core 索引](README.md) · [下一页：Mathematics](Inno.Core.Mathematics.md)

Logging 是基础、追加式日志系统。`LogRouter` 显式拥有一个 Host 的有界队列、过滤策略和 Sink；由 Host 显式选择 worker 或调用线程排空；核心不判断运行平台。`Log` 只是解析当前执行上下文的脚本便利门面。Compiler、Importer 和 Validator 的可替换当前问题属于独立的 [Inno.Core.Diagnostics](Inno.Core.Diagnostics.md)。

## 初始化

```csharp
using System.IO;
using Inno.Core.Logging;

static void ReportLoadedAssets(
    string projectRoot,
    int count
) {
    using var router = new LogRouter();
    router.RegisterSink(new ConsoleLogSink());
    router.RegisterSink(new FileLogSink(Path.Combine(projectRoot, "Logs")));
    router.SetMinimumLevel(LogLevel.Info);
    using (router.EnterScope())
        Log.Info("Loaded {0} assets", [count]);
    router.Flush();
}
```

`Dispose` 会停止 worker、排空队列，并 Dispose 所有实现 `IDisposable` 的已注册 sink。`EngineHost` 正常情况下统一拥有并释放 Router。

## Log

每个等级都有 `object?` 与 `(string message, IReadOnlyList<object?>? arguments)` overload。格式参数通过集合传入；两个入口均由编译器补充 `CallerFilePath` 与 `CallerLineNumber`：

- `Debug`：仅在 `DEBUG` 条件编译存在调用。
- `Info`
- `Warn`
- `Error`
- `Fatal`

格式化采用 `string.Format`。调用源文件名（去掉扩展名）作为 `category`；调用程序集的当前分类解析为 `domain` 与 `scope`。程序集分类使用弱缓存，不固定热重载 ALC；日志 entry 只复制 enum、字符串、时间与行号，不保存调用方 `Type`/`Assembly`。源文件和行号不依赖调试符号或反射方法元数据，因此相同入口适用于 JIT、裁剪和 AOT。`stackTrace` 是可用运行时栈的文本，完整程度由运行时与发布符号决定。

`Log` 已作为 Runtime Scripting API 导出到逻辑 namespace `InnoEngine.Logging`。Project 脚本不引用真实的 `Inno.Core.Logging` namespace：

```csharp
using InnoEngine.Logging;

static void ReportHealth(float health)
{
    Log.Info("Health updated: {0}", [health]);
    if (health < 10f)
        Log.Warn("Health is low: {0}", [health]);
}
```

只导出便捷门面 `Log`；`LogRouter`、sink 和日志分发配置仍由 Host 管理，不向游戏脚本开放。

## LogRouter

| API | 说明 |
| --- | --- |
| `RegisterSink(ILogSink)` | 添加接收器；允许多个。 |
| `UnregisterSink(ILogSink)` | 移除接收器，但不自动 Dispose。 |
| `SetMinimumLevel(LogLevel)` | 设置最低分发等级。 |
| `CreateLogger<TOwner>()` | 为实例服务创建显式 category logger。 |
| `Dispatch(LogEntry)` | 直接投递构造好的日志项。 |
| `Flush()` | 等待调用前已入队的日志全部送达 sink；只用于低频生命周期边界。 |
| `EnterScope()` | 将 Router 绑定到当前异步执行上下文，供脚本 `Log` 门面解析。 |
| `Dispose()` | 停 worker、flush、清空并释放 sink。 |

单个 sink 的 `Receive` 异常会被隔离并将该 sink 从 Router 原子隔离；`sinkFailed` 会报告失败，而健康 sink 继续接收日志。
失败 observer 之间也独立隔离。没有 observer 或 observer 自己失败时，stderr 仅作为次级报告渠道；
该渠道关闭或写入失败不能终止日志 worker、阻断后续 observer 或健康 sink。需要可靠收集失败时应订阅 `sinkFailed`。

`LogRouter` 构造函数的第三个参数 `deliveryMode` 接受 `Background`（默认）或 `Inline`。
两种模式均只有一个交付执行者，均使用队列 barrier 保证 `Flush()` 等待调用前的日志完整送达。
Inline 的并发 producer 串行交付；sink callback 中新写的日志先排队，当前 entry 交付给全部 sink 后再处理，
不会递归进入 callback 或改变不同 sink 观察到的顺序。
两种模式的交付 callback 均禁止调用自身 Router 的 `Flush()` 或 `Dispose()`，会明确抛出 `InvalidOperationException`。
外部 `Dispose()` 等待当前交付完成，再释放 sink。sink 快照仅在注册、移除或隔离时重新创建。
该行为由 Host 选择的策略决定，与浏览器或桌面平台无关。

## LogEntry 与 LogLevel

`LogLevel` 按严重程度递增：`Debug`、`Info`、`Warn`、`Error`、`Fatal`。

`LogEntry` readonly struct 构造参数/字段：

| 字段 | 类型 | 说明 |
| --- | --- | --- |
| `level` | `LogLevel` | 严重程度。 |
| `domain` | `AssemblyDomain` | InnoInternal/InnoScripting/InnoPlugin 所有权。 |
| `scope` | `AssemblyScope` | Runtime/Editor 依赖范围。 |
| `category` | `string` | 脚本门面使用调用源文件名；显式 logger 使用所属类型名。 |
| `message` | `string` | 已渲染文本。 |
| `time` | `DateTime` | 构造时本地时间。 |
| `file` | `string` | 编译器提供的调用源文件，不要求发布调试符号。 |
| `line` | `int` | 调用行号。 |

## Sink API

`ILogSink.Receive(LogEntry)` 是唯一契约。

### ConsoleLogSink

`Receive` 输出时间、domain/scope、category 与消息。桌面终端根据等级设置颜色，并在写入失败时也恢复原颜色；重定向输出及浏览器、Android、iOS、tvOS 使用普通文本，不调用这些平台不支持的终端颜色 API。日志内容保留，因此渲染或构建诊断不会因颜色功能不可用而让 sink 被隔离。

### FileLogSink

```csharp
using Inno.Core.Logging;

static FileLogSink CreateOwnedFileSink(string logDirectory)
{
    return new FileLogSink(logDirectory, maxFileSizeBytes: 10 * 1024 * 1024, maxFiles: 10);
}
```

- `C_LOG_FILE_PREFIX == "log_"`。
- `FileLogSink(logDirectory, maxFileSizeBytes = 10 * 1024 * 1024, maxFiles = 10)` 拒绝空目录及非正预算。
- 文件使用 `log_<UTC timestamp>_<unique ID>.log` 和 CreateNew 创建，多个 sink 或快速轮换不共用文件。
- `Receive` 串行写入完整 entry 并 flush；队列、线程与背压统一由 `LogRouter` 拥有。文件 sink 不再有第二个队列或 delivery policy 参数。
- 下一 entry 会超过 `maxFileSizeBytes` 时先轮换；单条超大 entry 保持完整，独占一个可超出该阈值的文件。
- retention 保留当前文件与最近的其他文件，预算包含当前文件。其他活动 writer 锁定的旧文件不能强制删除，后续轮换会重试。
- 写入、flush 和创建失败向调用方传播；Router 可以按统一 sink failure 机制隔离，不在隐藏线程中吞掉或抛出未处理异常。
- `Dispose()` 等待活动写入，再关闭 writer；关闭失败明确传播。并发读取活动文件时需使用 `FileShare.ReadWrite`。
- Retention 以 exclusive open + DeleteOnClose 清理过期文件，在支持文件锁的文件系统中保留活动 writer，关闭后下次轮换再清理。
  保留预算属于尽力执行，锁定、权限受限或不支持共享锁的文件系统不能提供严格并发预算保证。浏览器 Player 每个虚拟文件系统只使用自身 Session 的 sink。

## 注意事项

- Router 的 `Background` 使用 worker，`Inline` 使用调用线程；两种模式的 `Flush()` 都等待此前 entry 的文件写入完成。浏览器入口选择 Inline，文件日志位于 WASM 虚拟文件系统，游戏数据跨刷新持久化由 Storage adapter 提供。文件 sink 的 flush 不表示断电后的磁盘耐久性承诺。
- `Debug` 不是单纯运行时过滤；非 DEBUG 构建中调用会被编译器移除。
- 日志格式参数错误会在生产日志的一侧抛出；不要把不可信文本直接当 composite format。

## 宿主能力策略

`LogDeliveryMode.Background` 使用有界后台交付，`Inline` 在调用线程交付；两者保留过滤、Flush 和失败 sink 隔离。`LogRouter(queueCapacity, drainBudget, deliveryMode)` 拒绝未知枚举值。`ConsoleLogSink(useColors)` 显式控制终端颜色能力，重定向输出不改色。Host 通过 `EngineHostBuilder.UseLogDelivery(mode)` 配置唯一的引擎交付策略，不改变诊断 Replace/Clear 与追加 Log 的区别。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Core.Logging.ConsoleLogSink`

<!-- api: T:Inno.Core.Logging.ConsoleLogSink 5410ee93264c2c283f9656c1749fb75515112ac6ad0b29cc145ab33719df7247 -->
<!-- api: M:Inno.Core.Logging.ConsoleLogSink.#ctor(System.Boolean) 285081b26454852b2936318b79d2896a1673c7cf2623904b051ce1edf1b995d4 -->
<!-- api: M:Inno.Core.Logging.ConsoleLogSink.Receive(Inno.Core.Logging.LogEntry) d021c602bd023d4330bf793cabab1be8c635f9834869e1dce7ac4d40785a6597 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-bd56cfc647d2fccd"></a>[`public Inno.Core.Logging.ConsoleLogSink.ConsoleLogSink(bool useColors = false)`](../../src/foundation/core/Inno.Core.Logging/ConsoleLogSink.cs#L18) | Creates a console sink with explicitly selected terminal capabilities.<br>`useColors`：Whether the host supports console colors; redirected output always uses plain text. | — | — |
| <a id="api-b66075167bc754a0"></a>[`public void Inno.Core.Logging.ConsoleLogSink.Receive(Inno.Core.Logging.LogEntry entry)`](../../src/foundation/core/Inno.Core.Logging/ConsoleLogSink.cs#L26) | Writes the specified entry to standard output.<br>`entry`：The log entry to print. | — | — |
| <a id="api-ddc49b8633bfff5b"></a>[`public class Inno.Core.Logging.ConsoleLogSink : Inno.Core.Logging.ILogSink`](../../src/foundation/core/Inno.Core.Logging/ConsoleLogSink.cs#L8) | Writes log entries to the process console, using level-based colors when a terminal is available. | — | — |

### `Inno.Core.Logging.DiagnosticLogSink`

<!-- api: T:Inno.Core.Logging.DiagnosticLogSink a9a72eb8c5ffc91e4c7d6b72616bf52439459665012a467a6ed71428a60b74ad -->
<!-- api: M:Inno.Core.Logging.DiagnosticLogSink.#ctor(Inno.Core.Diagnostics.DiagnosticHub,Inno.Core.Logging.LogRouter) 2b4654ffee07e62e939024d31065379d2573e7b762164d40f7ea425c51523b39 -->
<!-- api: M:Inno.Core.Logging.DiagnosticLogSink.Replace(Inno.Core.Diagnostics.DiagnosticReport) 3884f32568c9b13ec7cb9e525c60652d24bd2968740da38e026dac1f6b8fe33f -->
<!-- api: M:Inno.Core.Logging.DiagnosticLogSink.Clear(Inno.Core.Diagnostics.DiagnosticSource) 30542be7d77eaedc99c0118cc3b7ca61ba21e03d6bbb706e811dc1e2be61eb94 -->
<!-- api: M:Inno.Core.Logging.DiagnosticLogSink.Dispose d15f6bb31632fd2a977d0d52976f19f4de989f22a95dddbd8d385903ba67208c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-90071a9ca246fe20"></a>[`public Inno.Core.Logging.DiagnosticLogSink.DiagnosticLogSink(Inno.Core.Diagnostics.DiagnosticHub hub, Inno.Core.Logging.LogRouter logs)`](../../src/foundation/core/Inno.Core.Logging/DiagnosticLogSink.cs#L28) | Registers a log presentation for all current and future reports in one hub.<br>`hub`：The authoritative diagnostic owner.<br>`logs`：The borrowed destination router, which must outlive this sink. | — | — |
| <a id="api-ef78bacdb911694c"></a>[`public void Inno.Core.Logging.DiagnosticLogSink.Clear(Inno.Core.Diagnostics.DiagnosticSource source)`](../../src/foundation/core/Inno.Core.Logging/DiagnosticLogSink.cs#L71) | Removes the current report for one diagnostic source.<br>`source`：The source whose current report was cleared. | — | — |
| <a id="api-6cb8509a9604c567"></a>[`public void Inno.Core.Logging.DiagnosticLogSink.Dispose()`](../../src/foundation/core/Inno.Core.Logging/DiagnosticLogSink.cs#L76) | Unregisters this presentation and releases its neutral de-duplication snapshot. | — | — |
| <a id="api-79425c305186f579"></a>[`public void Inno.Core.Logging.DiagnosticLogSink.Replace(Inno.Core.Diagnostics.DiagnosticReport report)`](../../src/foundation/core/Inno.Core.Logging/DiagnosticLogSink.cs#L42) | Replaces the current report for one diagnostic source.<br>`report`：The complete current report to store or present. | — | — |
| <a id="api-c80d38be9edc96c0"></a>[`public sealed class Inno.Core.Logging.DiagnosticLogSink : Inno.Core.Diagnostics.IDiagnosticSink, System.IDisposable`](../../src/foundation/core/Inno.Core.Logging/DiagnosticLogSink.cs#L12) | Projects changed diagnostic issues into a host's log stream without introducing another diagnostic owner. | — | — |

### `Inno.Core.Logging.FileLogSink`

<!-- api: T:Inno.Core.Logging.FileLogSink ac48e5621915fa6299bc33b67f3322a0478ffec9f683893b76d70c5445a88ab8 -->
<!-- api: F:Inno.Core.Logging.FileLogSink.C_LOG_FILE_PREFIX bdf37a9ae4c689218c095b542a71d0ac2c945f02787ee1dcee749fcfd6152c2d -->
<!-- api: M:Inno.Core.Logging.FileLogSink.#ctor(System.String,System.Int64,System.Int32) 25cf075a0f5955b46d3b5dd1cc81d4b3a9346ee6abb9212c1ff29a80038e7e22 -->
<!-- api: M:Inno.Core.Logging.FileLogSink.Receive(Inno.Core.Logging.LogEntry) 027b6a4d142c8a7a9630e72bdc026e881525a12ca8e27b7a1822df71282460ef -->
<!-- api: M:Inno.Core.Logging.FileLogSink.Dispose 493259673c11050788754e7a9c2dd5063fdab9bef33b324e90d4526d9b67c7d9 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4ebfe637e5d16333"></a>[`public const string Inno.Core.Logging.FileLogSink.C_LOG_FILE_PREFIX = "log_"`](../../src/foundation/core/Inno.Core.Logging/FileLogSink.cs#L16) | Prefix used for generated log file names. | — | — |
| <a id="api-6e132e30682179b8"></a>[`public Inno.Core.Logging.FileLogSink.FileLogSink(string logDirectory, long maxFileSizeBytes = 10485760, int maxFiles = 10)`](../../src/foundation/core/Inno.Core.Logging/FileLogSink.cs#L55) | Opens an isolated log file without creating another queue or delivery worker.<br>`logDirectory`：Directory in which complete log files are retained.<br>`maxFileSizeBytes`：Positive rotation threshold. An entry larger than this threshold occupies its own file.<br>`maxFiles`：Positive number of retained files, including the current file. | — | `System.ArgumentException`：The directory is blank or invalid.<br>`System.ArgumentOutOfRangeException`：A rotation or retention limit is not positive.<br>`System.IO.IOException`：The directory or initial file cannot be opened.<br>`System.UnauthorizedAccessException`：The caller cannot create or access the log directory. |
| <a id="api-7bbe7222ca06d972"></a>[`public void Inno.Core.Logging.FileLogSink.Dispose()`](../../src/foundation/core/Inno.Core.Logging/FileLogSink.cs#L124) | Waits for an active write and closes the owned file, reporting any final flush failure. | — | `System.IO.IOException`：The owned writer cannot finish flushing or closing its file. |
| <a id="api-040a2af01c33f82e"></a>[`public void Inno.Core.Logging.FileLogSink.Receive(Inno.Core.Logging.LogEntry entry)`](../../src/foundation/core/Inno.Core.Logging/FileLogSink.cs#L101) | Writes and flushes one complete entry before returning, serializing concurrent callers.<br>`entry`：The immutable entry to persist. | The router owns scheduling and backpressure. Write failures propagate to its sink quarantine. Retention cannot remove files locked by another active writer and retries them on later rotation. | `System.ObjectDisposedException`：The sink has been disposed.<br>`System.IO.IOException`：An entry cannot be written, flushed or rotated.<br>`System.UnauthorizedAccessException`：The caller cannot create the next log file or access the directory.<br>`System.InvalidOperationException`：A previous failed rotation left no writable file. |
| <a id="api-b070f14c80256f37"></a>[`public class Inno.Core.Logging.FileLogSink : Inno.Core.Logging.ILogSink, System.IDisposable`](../../src/foundation/core/Inno.Core.Logging/FileLogSink.cs#L11) | Writes complete log entries to uniquely named rotating files under the router's delivery policy. | — | — |

### `Inno.Core.Logging.ILogSink`

<!-- api: T:Inno.Core.Logging.ILogSink 4853db20574b25501084d659c8c74a3e0d27bd518eeb684e7c47917c9af47695 -->
<!-- api: M:Inno.Core.Logging.ILogSink.Receive(Inno.Core.Logging.LogEntry) 78663b86c9ed2b78d8249e8bf226f9a6e4cc4e826e6a3c00e158b677fd811a22 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b4bc64ada2005b39"></a>[`void Inno.Core.Logging.ILogSink.Receive(Inno.Core.Logging.LogEntry entry)`](../../src/foundation/core/Inno.Core.Logging/ILogSink.cs#L14) | Receives a log entry for processing.<br>`entry`：The log entry to consume. | — | — |
| <a id="api-7c7adc3fdcdb7926"></a>[`public interface Inno.Core.Logging.ILogSink`](../../src/foundation/core/Inno.Core.Logging/ILogSink.cs#L6) | Defines a log sink that consumes log entries from a Inno.Core.Logging.LogRouter. | — | — |

### `Inno.Core.Logging.Log`

<!-- api: T:Inno.Core.Logging.Log 6ec393e54bd6780c1a507dc2c7f02eeb2c7b6fd8841aa278e9c68405af5513d3 -->
<!-- api: M:Inno.Core.Logging.Log.Debug(System.Object,System.String,System.Int32) dca57ee6dec29822fe43a44e6d2fdab3a05c25d1d42130f21f1711a6c9656a51 -->
<!-- api: M:Inno.Core.Logging.Log.Debug(System.String,System.Collections.Generic.IReadOnlyList{System.Object},System.String,System.Int32) a3e979d1006d2ee0e407d583d7084fd2be385e73228abe01861e2cdfd6ed7383 -->
<!-- api: M:Inno.Core.Logging.Log.Info(System.Object,System.String,System.Int32) f9a1365023d3f5d79566b031084446e4f2dff5eff260cc3d588c68c5f195d95a -->
<!-- api: M:Inno.Core.Logging.Log.Info(System.String,System.Collections.Generic.IReadOnlyList{System.Object},System.String,System.Int32) d65351dd1f4a4b498adc456b88598be94a27fed3c531f321b74436035fcfc0b0 -->
<!-- api: M:Inno.Core.Logging.Log.Warn(System.Object,System.String,System.Int32) a4100821ce4cf3c7202b1ccef950fb020b67abd90d5cf54deb7f5276cc21be1d -->
<!-- api: M:Inno.Core.Logging.Log.Warn(System.String,System.Collections.Generic.IReadOnlyList{System.Object},System.String,System.Int32) 61fb596e476c75e87fd5c39804c4f733692e6d348eb68da062035c14b66aae71 -->
<!-- api: M:Inno.Core.Logging.Log.Error(System.Object,System.String,System.Int32) 5c845b0cb68abf77b4d44d6e96f96f5886d342fc25d78ef12881c32ccb6c473c -->
<!-- api: M:Inno.Core.Logging.Log.Error(System.String,System.Collections.Generic.IReadOnlyList{System.Object},System.String,System.Int32) 2abfa499422d30c6103d7f7297660fe68a7edf42dba59ccf2de1cd15c907e82a -->
<!-- api: M:Inno.Core.Logging.Log.Fatal(System.Object,System.String,System.Int32) 6914ef11724305c60e6aea5f7de47924dd35c9cc411d9815850519edd75c9b0a -->
<!-- api: M:Inno.Core.Logging.Log.Fatal(System.String,System.Collections.Generic.IReadOnlyList{System.Object},System.String,System.Int32) 39be795b19ce9286325570a7b7677d8811bd345d2907e0b0833e2609e5a4a215 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d734f14b67a29564"></a>[`public static void Inno.Core.Logging.Log.Debug(object? obj, string filePath = "", int lineNumber = 0)`](../../src/foundation/core/Inno.Core.Logging/Log.cs#L34) | Writes a debug-level message using the object's string representation.<br>`obj`：The object to log; null produces an empty message.<br>`filePath`：The source file supplied by the compiler; its file name is the category.<br>`lineNumber`：The source line supplied by the compiler. | — | — |
| <a id="api-c32df4fcbbc88f5a"></a>[`public static void Inno.Core.Logging.Log.Debug(string message, System.Collections.Generic.IReadOnlyList<object?>? arguments, string filePath = "", int lineNumber = 0)`](../../src/foundation/core/Inno.Core.Logging/Log.cs#L60) | Writes a formatted debug-level message with its source location.<br>`message`：The composite format string or final message.<br>`arguments`：Optional composite-format arguments; null preserves the message verbatim.<br>`filePath`：The source file supplied by the compiler; its file name is the category.<br>`lineNumber`：The source line supplied by the compiler. | — | `System.FormatException`：The composite format string is invalid for the supplied arguments. |
| <a id="api-09bf2f0f05819079"></a>[`public static void Inno.Core.Logging.Log.Error(object? obj, string filePath = "", int lineNumber = 0)`](../../src/foundation/core/Inno.Core.Logging/Log.cs#L170) | Writes a error-level message using the object's string representation.<br>`obj`：The object to log; null produces an empty message.<br>`filePath`：The source file supplied by the compiler; its file name is the category.<br>`lineNumber`：The source line supplied by the compiler. | — | — |
| <a id="api-ddd1e4cb41916e66"></a>[`public static void Inno.Core.Logging.Log.Error(string message, System.Collections.Generic.IReadOnlyList<object?>? arguments, string filePath = "", int lineNumber = 0)`](../../src/foundation/core/Inno.Core.Logging/Log.cs#L195) | Writes a formatted error-level message with its source location.<br>`message`：The composite format string or final message.<br>`arguments`：Optional composite-format arguments; null preserves the message verbatim.<br>`filePath`：The source file supplied by the compiler; its file name is the category.<br>`lineNumber`：The source line supplied by the compiler. | — | `System.FormatException`：The composite format string is invalid for the supplied arguments. |
| <a id="api-a3c3259117500090"></a>[`public static void Inno.Core.Logging.Log.Fatal(object? obj, string filePath = "", int lineNumber = 0)`](../../src/foundation/core/Inno.Core.Logging/Log.cs#L215) | Writes a fatal-level message using the object's string representation.<br>`obj`：The object to log; null produces an empty message.<br>`filePath`：The source file supplied by the compiler; its file name is the category.<br>`lineNumber`：The source line supplied by the compiler. | — | — |
| <a id="api-4f0148e2a43cf635"></a>[`public static void Inno.Core.Logging.Log.Fatal(string message, System.Collections.Generic.IReadOnlyList<object?>? arguments, string filePath = "", int lineNumber = 0)`](../../src/foundation/core/Inno.Core.Logging/Log.cs#L240) | Writes a formatted fatal-level message with its source location.<br>`message`：The composite format string or final message.<br>`arguments`：Optional composite-format arguments; null preserves the message verbatim.<br>`filePath`：The source file supplied by the compiler; its file name is the category.<br>`lineNumber`：The source line supplied by the compiler. | — | `System.FormatException`：The composite format string is invalid for the supplied arguments. |
| <a id="api-d71bea389b989152"></a>[`public static void Inno.Core.Logging.Log.Info(object? obj, string filePath = "", int lineNumber = 0)`](../../src/foundation/core/Inno.Core.Logging/Log.cs#L80) | Writes a info-level message using the object's string representation.<br>`obj`：The object to log; null produces an empty message.<br>`filePath`：The source file supplied by the compiler; its file name is the category.<br>`lineNumber`：The source line supplied by the compiler. | — | — |
| <a id="api-d0664067ca09c1e4"></a>[`public static void Inno.Core.Logging.Log.Info(string message, System.Collections.Generic.IReadOnlyList<object?>? arguments, string filePath = "", int lineNumber = 0)`](../../src/foundation/core/Inno.Core.Logging/Log.cs#L105) | Writes a formatted info-level message with its source location.<br>`message`：The composite format string or final message.<br>`arguments`：Optional composite-format arguments; null preserves the message verbatim.<br>`filePath`：The source file supplied by the compiler; its file name is the category.<br>`lineNumber`：The source line supplied by the compiler. | — | `System.FormatException`：The composite format string is invalid for the supplied arguments. |
| <a id="api-903db7dd94f5a83e"></a>[`public static void Inno.Core.Logging.Log.Warn(object? obj, string filePath = "", int lineNumber = 0)`](../../src/foundation/core/Inno.Core.Logging/Log.cs#L125) | Writes a warn-level message using the object's string representation.<br>`obj`：The object to log; null produces an empty message.<br>`filePath`：The source file supplied by the compiler; its file name is the category.<br>`lineNumber`：The source line supplied by the compiler. | — | — |
| <a id="api-a73b72d0efd8c856"></a>[`public static void Inno.Core.Logging.Log.Warn(string message, System.Collections.Generic.IReadOnlyList<object?>? arguments, string filePath = "", int lineNumber = 0)`](../../src/foundation/core/Inno.Core.Logging/Log.cs#L150) | Writes a formatted warn-level message with its source location.<br>`message`：The composite format string or final message.<br>`arguments`：Optional composite-format arguments; null preserves the message verbatim.<br>`filePath`：The source file supplied by the compiler; its file name is the category.<br>`lineNumber`：The source line supplied by the compiler. | — | `System.FormatException`：The composite format string is invalid for the supplied arguments. |
| <a id="api-9b07728a634a751d"></a>[`public static class Inno.Core.Logging.Log`](../../src/foundation/core/Inno.Core.Logging/Log.cs#L15) | Writes script logs with compiler-provided source locations and the calling assembly's ownership. | — | — |

### `Inno.Core.Logging.LogDeliveryMode`

<!-- api: T:Inno.Core.Logging.LogDeliveryMode 3a0930eaca2da807c0688583b39f006071b2f1adebec72477319bdad1382ed0d -->
<!-- api: F:Inno.Core.Logging.LogDeliveryMode.Background 6dc08b7c5fa1b430f85fc2871247745b47c0b3a7e1566505152640726b933d4f -->
<!-- api: F:Inno.Core.Logging.LogDeliveryMode.Inline cd94b6c1fb7cb5a3ff340a1a6d2540995cb068165cd55bd5cc0a2a209d398731 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2736198d7c260879"></a>[`Inno.Core.Logging.LogDeliveryMode.Background = 0`](../../src/foundation/core/Inno.Core.Logging/LogDeliveryMode.cs#L11) | Delivers queued entries through a dedicated background worker. | — | — |
| <a id="api-5422f9921f2b488a"></a>[`Inno.Core.Logging.LogDeliveryMode.Inline = 1`](../../src/foundation/core/Inno.Core.Logging/LogDeliveryMode.cs#L16) | Delivers entries synchronously on the producer's thread. | — | — |
| <a id="api-a33f86962b1349a5"></a>[`public enum Inno.Core.Logging.LogDeliveryMode : int`](../../src/foundation/core/Inno.Core.Logging/LogDeliveryMode.cs#L6) | Selects the delivery policy independently of the operating system or execution host. | — | — |

### `Inno.Core.Logging.LogEntry`

<!-- api: T:Inno.Core.Logging.LogEntry 08ec6e44bab8ac7dcbb92aeacfa0be8df8acaac237039b2133e47bb4e5d54ec2 -->
<!-- api: M:Inno.Core.Logging.LogEntry.#ctor(Inno.Core.Logging.LogLevel,Inno.Extensibility.Modules.AssemblyDomain,Inno.Extensibility.Modules.AssemblyScope,System.String,System.String,System.String,System.Int32,System.String,Inno.Core.Logging.LogSessionId) 1ba66914638abf8a1db4437f30e28e4b45d5275c3d8b67deb1933dfa77df1cb9 -->
<!-- api: F:Inno.Core.Logging.LogEntry.level e80ce21da876c740e7c2deb7fdb90f6360b2101ebd21cc6eb3bf69a4235cbb93 -->
<!-- api: F:Inno.Core.Logging.LogEntry.domain 24b6c102d2bfa6efaead74f2b02e5813af25114700b963db6523d3bc2aa191d6 -->
<!-- api: F:Inno.Core.Logging.LogEntry.scope 72f999af67bad74edf0b67798c2f620fba54a84b0bf0d45517f859ac819bb63b -->
<!-- api: F:Inno.Core.Logging.LogEntry.category f818f915368a0987e2ca529307ea1a934234e89485ecabd3321e6836fa5cf93b -->
<!-- api: F:Inno.Core.Logging.LogEntry.message 3e4d10b8d31cc57d3530d944596f49cacd537b38e6e3660dd6d030cab56bf44a -->
<!-- api: F:Inno.Core.Logging.LogEntry.time 55ba3f189068ae350cef8796684eb1e7467ff6d4d20ccd210a9176b6a1f1b57d -->
<!-- api: F:Inno.Core.Logging.LogEntry.file 44c7084e05546fdd0df76907b861eda83bed46cd01a8d06356339aed4c666ec6 -->
<!-- api: F:Inno.Core.Logging.LogEntry.line 74691958aeedf92b023278432ca3c5cf22bc002199e6f99e6d3365248b1272c9 -->
<!-- api: F:Inno.Core.Logging.LogEntry.stackTrace 59ad634f0499e51fb515069e1cb6f961cce0693aec2c9fa4d52bac26a35677dd -->
<!-- api: F:Inno.Core.Logging.LogEntry.sessionId e69f835f472f57f37e99e82af97a4e53761a179d1a8483a45077ca419d2459dd -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d2e35aeb0c281842"></a>[`public readonly string Inno.Core.Logging.LogEntry.category`](../../src/foundation/core/Inno.Core.Logging/LogEntry.cs#L66) | Gets the category name for this entry. | — | — |
| <a id="api-69a7fed42d8a3617"></a>[`public readonly Inno.Extensibility.Modules.AssemblyDomain Inno.Core.Logging.LogEntry.domain`](../../src/foundation/core/Inno.Core.Logging/LogEntry.cs#L56) | Gets the assembly ownership domain for this entry. | — | — |
| <a id="api-126a836a0daddb1c"></a>[`public readonly string Inno.Core.Logging.LogEntry.file`](../../src/foundation/core/Inno.Core.Logging/LogEntry.cs#L81) | Gets the source file name if available; otherwise a fallback name. | — | — |
| <a id="api-866660a41f9ebaa7"></a>[`public readonly Inno.Core.Logging.LogLevel Inno.Core.Logging.LogEntry.level`](../../src/foundation/core/Inno.Core.Logging/LogEntry.cs#L51) | Gets the severity of this entry. | — | — |
| <a id="api-dc93e15a48a42fa4"></a>[`public readonly int Inno.Core.Logging.LogEntry.line`](../../src/foundation/core/Inno.Core.Logging/LogEntry.cs#L86) | Gets the source line number if available. | — | — |
| <a id="api-696745a10a34d864"></a>[`public readonly string Inno.Core.Logging.LogEntry.message`](../../src/foundation/core/Inno.Core.Logging/LogEntry.cs#L71) | Gets the rendered message text. | — | — |
| <a id="api-b88ab2774ba40bea"></a>[`public readonly Inno.Extensibility.Modules.AssemblyScope Inno.Core.Logging.LogEntry.scope`](../../src/foundation/core/Inno.Core.Logging/LogEntry.cs#L61) | Gets the runtime or editor scope for this entry. | — | — |
| <a id="api-05669310141edfca"></a>[`public readonly Inno.Core.Logging.LogSessionId Inno.Core.Logging.LogEntry.sessionId`](../../src/foundation/core/Inno.Core.Logging/LogEntry.cs#L96) | Gets the isolated runtime session that produced this entry. | — | — |
| <a id="api-bef00d3ff5ab7a3c"></a>[`public readonly string Inno.Core.Logging.LogEntry.stackTrace`](../../src/foundation/core/Inno.Core.Logging/LogEntry.cs#L91) | Gets the managed stack trace captured at the logging call site. | — | — |
| <a id="api-87a7b90ffb000d9a"></a>[`public readonly System.DateTime Inno.Core.Logging.LogEntry.time`](../../src/foundation/core/Inno.Core.Logging/LogEntry.cs#L76) | Gets the timestamp captured when this entry was created. | — | — |
| <a id="api-706e84c35e195652"></a>[`public Inno.Core.Logging.LogEntry.LogEntry(Inno.Core.Logging.LogLevel level, Inno.Extensibility.Modules.AssemblyDomain domain, Inno.Extensibility.Modules.AssemblyScope scope, string category, string message, string file, int line, string stackTrace, Inno.Core.Logging.LogSessionId sessionId)`](../../src/foundation/core/Inno.Core.Logging/LogEntry.cs#L37) | Represents an immutable log message dispatched by a Inno.Core.Logging.LogRouter.<br>`level`：The severity of the log message.<br>`domain`：The resolved assembly ownership domain.<br>`scope`：The resolved runtime or editor scope.<br>`category`：The log category, typically the declaring type name.<br>`message`：The rendered log message text.<br>`file`：The source file name when available.<br>`line`：The source line number when available.<br>`stackTrace`：The captured managed stack trace, or an empty string when no trace is available.<br>`sessionId`：The isolated runtime session that produced the entry, or Inno.Core.Logging.LogSessionId.none for process-level work. | — | — |
| <a id="api-5791799ed731929c"></a>[`public readonly struct Inno.Core.Logging.LogEntry`](../../src/foundation/core/Inno.Core.Logging/LogEntry.cs#L37) | Represents an immutable log message dispatched by a Inno.Core.Logging.LogRouter.<br>`level`：The severity of the log message.<br>`domain`：The resolved assembly ownership domain.<br>`scope`：The resolved runtime or editor scope.<br>`category`：The log category, typically the declaring type name.<br>`message`：The rendered log message text.<br>`file`：The source file name when available.<br>`line`：The source line number when available.<br>`stackTrace`：The captured managed stack trace, or an empty string when no trace is available.<br>`sessionId`：The isolated runtime session that produced the entry, or Inno.Core.Logging.LogSessionId.none for process-level work. | — | — |

### `Inno.Core.Logging.Logger`

<!-- api: T:Inno.Core.Logging.Logger 7b898fbcd013600b61e274cfc38e6327e03d9c31cb9e7bd455af45b6c1f70293 -->
<!-- api: M:Inno.Core.Logging.Logger.Write(Inno.Core.Logging.LogLevel,System.String,System.Collections.Generic.IReadOnlyList{System.Object},System.String,System.Int32) 206b20287c1d3cd9f3361fedc19a18189f631706a2b0854de2d90e83ca0d0bdf -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-712aa18b0da47632"></a>[`public void Inno.Core.Logging.Logger.Write(Inno.Core.Logging.LogLevel level, string message, System.Collections.Generic.IReadOnlyList<object?>? arguments = null, string filePath = "", int lineNumber = 0)`](../../src/foundation/core/Inno.Core.Logging/Logger.cs#L57) | Writes one formatted entry with source information captured from the caller.<br>`level`：The severity assigned to the entry.<br>`message`：The composite format string or final message text.<br>`arguments`：Optional composite-format arguments.<br>`filePath`：Source file supplied by the caller information attribute.<br>`lineNumber`：Source line supplied by the caller information attribute. | — | `System.ArgumentException`：Thrown when message is empty.<br>`System.ObjectDisposedException`：Thrown after the owning router has been disposed. |
| <a id="api-8e9bde170830a7e6"></a>[`public sealed class Inno.Core.Logging.Logger`](../../src/foundation/core/Inno.Core.Logging/Logger.cs#L14) | Writes category-bound entries through one explicitly owned log router. | — | — |

### `Inno.Core.Logging.LogLevel`

<!-- api: T:Inno.Core.Logging.LogLevel cb1b0c874cf3543a4745e1a55c4cda0be55196ffeb3974498c15c56d163a6016 -->
<!-- api: F:Inno.Core.Logging.LogLevel.Debug 1cf5870fbf5dcb27ad114f57fe6f79d45e8fb403d38a8e00c9ebf8835414a718 -->
<!-- api: F:Inno.Core.Logging.LogLevel.Info 90c48267842f4ba4e97d74f09936943843330fe7790c91faebf5c5bf7a1c53db -->
<!-- api: F:Inno.Core.Logging.LogLevel.Warn 5ec8051daa9503c8e8704cc90866465c2ed4fa2acfd32fbe67a505f77b2abd78 -->
<!-- api: F:Inno.Core.Logging.LogLevel.Error 95f31790d816de0956af823b546385753ac32743bbeea28cd0802b54bb977665 -->
<!-- api: F:Inno.Core.Logging.LogLevel.Fatal a56d16ae7f6c095dd8289494a1e48c08244d3dbe11ecbb169e9349b034579803 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f9dcebae441867c0"></a>[`Inno.Core.Logging.LogLevel.Debug = 0`](../../src/foundation/core/Inno.Core.Logging/LogLevel.cs#L11) | Diagnostic logs intended for local debugging. | — | — |
| <a id="api-6d5777fda1050352"></a>[`Inno.Core.Logging.LogLevel.Error = 3`](../../src/foundation/core/Inno.Core.Logging/LogLevel.cs#L26) | Error logs for failures that should be investigated. | — | — |
| <a id="api-84fa4f01e4cba61a"></a>[`Inno.Core.Logging.LogLevel.Fatal = 4`](../../src/foundation/core/Inno.Core.Logging/LogLevel.cs#L31) | Fatal logs for unrecoverable failures. | — | — |
| <a id="api-d33d5da919a39a48"></a>[`Inno.Core.Logging.LogLevel.Info = 1`](../../src/foundation/core/Inno.Core.Logging/LogLevel.cs#L16) | Informational logs for normal runtime flow. | — | — |
| <a id="api-d94b453ab2247495"></a>[`Inno.Core.Logging.LogLevel.Warn = 2`](../../src/foundation/core/Inno.Core.Logging/LogLevel.cs#L21) | Warning logs for recoverable problems. | — | — |
| <a id="api-8459dde37b559ef8"></a>[`public enum Inno.Core.Logging.LogLevel : int`](../../src/foundation/core/Inno.Core.Logging/LogLevel.cs#L6) | Represents log severity levels in ascending order. | — | — |

### `Inno.Core.Logging.LogRouter`

<!-- api: T:Inno.Core.Logging.LogRouter 6ae9f1ee4fb48f5dbcc4a3a63fb976677afebfa089185bab09d8f751a7907c94 -->
<!-- api: E:Inno.Core.Logging.LogRouter.sinkFailed 8d84be2afa650d6621c19e8549af758eac1fd778891f00f1bee31d83f2f6e946 -->
<!-- api: M:Inno.Core.Logging.LogRouter.#ctor(System.Int32,System.Int32,Inno.Core.Logging.LogDeliveryMode) 1c412e79cdeaea55df724a6b09ad56c880b34ee0e0354bf14d4c3137454e47df -->
<!-- api: M:Inno.Core.Logging.LogRouter.EnterScope d43a5299c500c8d851c484edf5926934fa3eba76a3bad108e549c0a5502ee152 -->
<!-- api: M:Inno.Core.Logging.LogRouter.RegisterSink(Inno.Core.Logging.ILogSink) 4665d44d5a977911c341c19ac43f04191320fbb96586671b9cc3d18046feb638 -->
<!-- api: M:Inno.Core.Logging.LogRouter.UnregisterSink(Inno.Core.Logging.ILogSink) 7731544638e42b4cb1a8b037f105c16d41f4835cabd6ca08633d6f0056a0cf99 -->
<!-- api: M:Inno.Core.Logging.LogRouter.SetMinimumLevel(Inno.Core.Logging.LogLevel) e978aa34ccadae07658fc69f426512718e40e7e1282332b95fdc4ab6216f5e94 -->
<!-- api: M:Inno.Core.Logging.LogRouter.CreateLogger``1 cc1e261f7e8258d8ce2ec2b7f9a5dac78abb6f3f9be221eca679c41170db4000 -->
<!-- api: M:Inno.Core.Logging.LogRouter.Dispatch(Inno.Core.Logging.LogEntry) cd03ddac7140bc3747cc6952a5aae96ca2622695da4b6b07d9d46371b91b137c -->
<!-- api: M:Inno.Core.Logging.LogRouter.TryDispatch(Inno.Core.Logging.LogEntry) 8a5997cdad093bf81a998b2191a1bab1cfb33409d82e7da481940b4b76623cfe -->
<!-- api: M:Inno.Core.Logging.LogRouter.Flush 9127df56f8af402b427f3b0b2cf47f3a10b532e11af95ae7055e979030f3a4f4 -->
<!-- api: M:Inno.Core.Logging.LogRouter.Dispose 53a033d1e9718139ebd6139496c0f7e97768195d2ddec78b74b8c9f8e47e8ef7 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-18a83fc30033096e"></a>[`public System.Action<Inno.Core.Logging.ILogSink, System.Exception>? Inno.Core.Logging.LogRouter.sinkFailed`](../../src/foundation/core/Inno.Core.Logging/LogRouter.cs#L38) | Occurs after a failing sink has been quarantined from this router. | — | — |
| <a id="api-8064ea46b47ad839"></a>[`public Inno.Core.Logging.LogRouter.LogRouter(int queueCapacity = 65536, int drainBudget = 4096, Inno.Core.Logging.LogDeliveryMode deliveryMode = Inno.Core.Logging.LogDeliveryMode.Background)`](../../src/foundation/core/Inno.Core.Logging/LogRouter.cs#L55) | Creates an isolated logging router with an explicit delivery policy.<br>`queueCapacity`：Maximum pending entries and flush barriers before explicit rejection.<br>`drainBudget`：Maximum work items delivered using one sink snapshot.<br>`deliveryMode`：Selects worker or inline delivery independently of the platform. | — | `System.ArgumentOutOfRangeException`：A capacity or budget is not positive, or the delivery policy is not a defined mode. |
| <a id="api-63de34d89162e3bd"></a>[`public Inno.Core.Logging.Logger Inno.Core.Logging.LogRouter.CreateLogger<TOwner>()`](../../src/foundation/core/Inno.Core.Logging/LogRouter.cs#L167) | Creates a category-bound logger for one engine service or extension type.<br>返回：A stateless logger bound to this router and the supplied owner type. | — | `System.ObjectDisposedException`：Thrown after this router has been disposed. |
| <a id="api-fa5aa34b064a9f7a"></a>[`public void Inno.Core.Logging.LogRouter.Dispatch(Inno.Core.Logging.LogEntry entry)`](../../src/foundation/core/Inno.Core.Logging/LogRouter.cs#L190) | Queues an immutable entry under the configured worker or inline delivery policy.<br>`entry`：The entry to dispatch when it satisfies the current severity policy. | — | `System.InvalidOperationException`：The bounded queue is full and the producer must apply backpressure.<br>`System.ObjectDisposedException`：Thrown after this router has been disposed. |
| <a id="api-d5b211097bc9423c"></a>[`public void Inno.Core.Logging.LogRouter.Dispose()`](../../src/foundation/core/Inno.Core.Logging/LogRouter.cs#L263) | Drains pending entries, stops the worker, and disposes every sink still owned by this router. | — | `System.InvalidOperationException`：A delivery callback attempts to dispose its own router.<br>`System.AggregateException`：One or more owned sinks fail to dispose after pending entries have drained. |
| <a id="api-456d27d9191d5db1"></a>[`public System.IDisposable Inno.Core.Logging.LogRouter.EnterScope()`](../../src/foundation/core/Inno.Core.Logging/LogRouter.cs#L88) | Binds this router to the current asynchronous execution context.<br>返回：A strict last-in-first-out scope owned by the caller. | — | `System.ObjectDisposedException`：Thrown after this router has been disposed. |
| <a id="api-109d37e201f254cc"></a>[`public void Inno.Core.Logging.LogRouter.Flush()`](../../src/foundation/core/Inno.Core.Logging/LogRouter.cs#L237) | Blocks until every entry enqueued before this call has reached the current sink snapshot. | — | `System.InvalidOperationException`：A delivery callback attempts to wait for its own completion, or the queue cannot accept a flush barrier.<br>`System.ObjectDisposedException`：Thrown after this router has been disposed. |
| <a id="api-30e7af2a2948c10c"></a>[`public void Inno.Core.Logging.LogRouter.RegisterSink(Inno.Core.Logging.ILogSink sink)`](../../src/foundation/core/Inno.Core.Logging/LogRouter.cs#L106) | Registers a sink to receive future entries from this router.<br>`sink`：The sink to register exactly once. | — | `System.ArgumentNullException`：Thrown when sink is null.<br>`System.ObjectDisposedException`：Thrown after this router has been disposed. |
| <a id="api-769ba1236de06b3d"></a>[`public void Inno.Core.Logging.LogRouter.SetMinimumLevel(Inno.Core.Logging.LogLevel level)`](../../src/foundation/core/Inno.Core.Logging/LogRouter.cs#L149) | Sets the lowest severity accepted by this router.<br>`level`：The minimum dispatched severity. | — | `System.ObjectDisposedException`：Thrown after this router has been disposed. |
| <a id="api-84788e628a3a59ec"></a>[`public bool Inno.Core.Logging.LogRouter.TryDispatch(Inno.Core.Logging.LogEntry entry)`](../../src/foundation/core/Inno.Core.Logging/LogRouter.cs#L212) | Attempts to queue an immutable entry, draining it inline when the host has no logging worker.<br>返回：True when accepted or filtered by policy; false when bounded queue capacity is exhausted.<br>`entry`：The message to accept under the current severity policy. | Inline callers serialize delivery. Entries written by a sink callback are queued until every sink has received the current entry, without recursively invoking callbacks. | `System.ObjectDisposedException`：The router has stopped accepting work. |
| <a id="api-fb7bbd0e8840dafd"></a>[`public void Inno.Core.Logging.LogRouter.UnregisterSink(Inno.Core.Logging.ILogSink sink)`](../../src/foundation/core/Inno.Core.Logging/LogRouter.cs#L130) | Unregisters a sink so it receives no future entries from this router.<br>`sink`：The sink to remove when registered. | — | `System.ArgumentNullException`：Thrown when sink is null. |
| <a id="api-447b7e9618a45ea4"></a>[`public sealed class Inno.Core.Logging.LogRouter : System.IDisposable`](../../src/foundation/core/Inno.Core.Logging/LogRouter.cs#L15) | Owns one host's bounded log queue, filtering policy, and sink collection. | — | — |

### `Inno.Core.Logging.LogSessionContext`

<!-- api: T:Inno.Core.Logging.LogSessionContext a444a33845903f2a80386ca52acbbf210915acd2aee05cf7676ff1c0bb9267c7 -->
<!-- api: P:Inno.Core.Logging.LogSessionContext.current 2551f5af5c04f14a2c5528527dee17462ab70bd84bc43500da6288f82e865d52 -->
<!-- api: M:Inno.Core.Logging.LogSessionContext.Enter(Inno.Core.Logging.LogSessionId) c0e1ea25d6519de797cc4e1125a5a6ee01e40ce5de064b55f1dab31233cc3c9c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9d065e5274b8013d"></a>[`public static System.IDisposable Inno.Core.Logging.LogSessionContext.Enter(Inno.Core.Logging.LogSessionId sessionId)`](../../src/foundation/core/Inno.Core.Logging/LogSessionContext.cs#L32) | Enters a nested execution scope whose emitted logs belong to the supplied runtime session.<br>返回：A scope that restores the previous session when disposed in reverse creation order.<br>`sessionId`：The non-empty identifier of the runtime session receiving execution. | — | `System.ArgumentException`：Thrown when sessionId does not identify a runtime session. |
| <a id="api-2dc07f026080490b"></a>[`public static Inno.Core.Logging.LogSessionId Inno.Core.Logging.LogSessionContext.current { get; }`](../../src/foundation/core/Inno.Core.Logging/LogSessionContext.cs#L18) | Gets the runtime session associated with the current asynchronous execution flow. | — | — |
| <a id="api-5f0194f840af3f28"></a>[`public static class Inno.Core.Logging.LogSessionContext`](../../src/foundation/core/Inno.Core.Logging/LogSessionContext.cs#L10) | Associates logs written on one asynchronous execution flow with an isolated runtime session. | — | — |

### `Inno.Core.Logging.LogSessionId`

<!-- api: T:Inno.Core.Logging.LogSessionId e9131116c6d2bfd839469f7d7976d1dee5422da3064b6b1a0deb4731cc339a4b -->
<!-- api: P:Inno.Core.Logging.LogSessionId.none 4a41b49cbc6c0fcb09078aa575872fec7b01fa330358e93dc40e6d72ce6baa2a -->
<!-- api: P:Inno.Core.Logging.LogSessionId.isAssigned 1b13482f269878402adba64ac80c2b1d0727f4d6d3c873ea3ecc69e1dc74612c -->
<!-- api: M:Inno.Core.Logging.LogSessionId.Create 03f2a1782ad0a27cb1a56292f1d34a5fa4fc909c9dcb20402391e8563ed41ac7 -->
<!-- api: M:Inno.Core.Logging.LogSessionId.ToString be3b1f0b53b6bb8948a59053d914d5f062359fff78aa6c11007b718f50bef427 -->
<!-- api: M:Inno.Core.Logging.LogSessionId.op_Inequality(Inno.Core.Logging.LogSessionId,Inno.Core.Logging.LogSessionId) b2476ddaa68d497afa6adc2d3c9e200410fd727b7228036f942ae7dff3468c1f -->
<!-- api: M:Inno.Core.Logging.LogSessionId.op_Equality(Inno.Core.Logging.LogSessionId,Inno.Core.Logging.LogSessionId) c41781d4dd464ba5cac9937c0e29a435a0831d1d3a323b8a38f3085262498fcd -->
<!-- api: M:Inno.Core.Logging.LogSessionId.GetHashCode 2db208a6c272b081a2cd7df22126b79750c6521e1591eb89c2c469751637558b -->
<!-- api: M:Inno.Core.Logging.LogSessionId.Equals(System.Object) 5f3544a879f66cf6cfb9a42fceb6e64053dd7f8f6a9bf1cca17c7546dab07eac -->
<!-- api: M:Inno.Core.Logging.LogSessionId.Equals(Inno.Core.Logging.LogSessionId) c4fa95e2f19605574a0d9e5df41c07fda118d34e9128a9da5bca505c0b62a02b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2fb479cb2cfb6772"></a>[`public static Inno.Core.Logging.LogSessionId Inno.Core.Logging.LogSessionId.Create()`](../../src/foundation/core/Inno.Core.Logging/LogSessionId.cs#L33) | Creates a unique identifier for a new isolated runtime session.<br>返回：A newly allocated non-empty session identifier. | — | — |
| <a id="api-272f9713aa822c80"></a>[`public bool Inno.Core.Logging.LogSessionId.Equals(Inno.Core.Logging.LogSessionId other)`](../../src/foundation/core/Inno.Core.Logging/LogSessionId.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-9552126eb2e33b5d"></a>[`public override bool Inno.Core.Logging.LogSessionId.Equals(object obj)`](../../src/foundation/core/Inno.Core.Logging/LogSessionId.cs#L8) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-b5855fae1ecdea52"></a>[`public override int Inno.Core.Logging.LogSessionId.GetHashCode()`](../../src/foundation/core/Inno.Core.Logging/LogSessionId.cs#L8) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-b204966815874cf4"></a>[`public static bool Inno.Core.Logging.LogSessionId.operator ==(Inno.Core.Logging.LogSessionId left, Inno.Core.Logging.LogSessionId right)`](../../src/foundation/core/Inno.Core.Logging/LogSessionId.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-c791e1ca26200be1"></a>[`public static bool Inno.Core.Logging.LogSessionId.operator !=(Inno.Core.Logging.LogSessionId left, Inno.Core.Logging.LogSessionId right)`](../../src/foundation/core/Inno.Core.Logging/LogSessionId.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-c6e1bc1a9c8b79e8"></a>[`public override string Inno.Core.Logging.LogSessionId.ToString()`](../../src/foundation/core/Inno.Core.Logging/LogSessionId.cs#L41) | Returns the stable textual representation used by diagnostics and persisted logs.<br>返回：The lowercase hexadecimal identifier, or an empty string for process-level work. | — | — |
| <a id="api-912f00173511fab9"></a>[`public bool Inno.Core.Logging.LogSessionId.isAssigned { get; }`](../../src/foundation/core/Inno.Core.Logging/LogSessionId.cs#L25) | Gets whether this identifier names an isolated runtime session. | — | — |
| <a id="api-7af7bb15e14dda5f"></a>[`public static Inno.Core.Logging.LogSessionId Inno.Core.Logging.LogSessionId.none { get; }`](../../src/foundation/core/Inno.Core.Logging/LogSessionId.cs#L20) | Gets an identifier that represents process-level work outside an isolated runtime session. | — | — |
| <a id="api-c169673038288000"></a>[`public readonly record struct Inno.Core.Logging.LogSessionId : System.IEquatable<Inno.Core.Logging.LogSessionId>`](../../src/foundation/core/Inno.Core.Logging/LogSessionId.cs#L8) | Identifies the isolated runtime session that produced a log entry. | — | — |

## 项目依赖

- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Modules](../extensibility/Inno.Extensibility.Modules.md)：公开引用边界由实际签名核对。
- [Inno.Core.Diagnostics](Inno.Core.Diagnostics.md)：公开引用边界由实际签名核对。
- [Inno.Core.Execution](Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
