# Inno.Core.Logging

本轮收口新增 `DiagnosticLogSink(hub, logs)`：作为 `IDiagnosticSink` 接收 Replace/Clear，只把新增或变化的问题写入 LogRouter；Dispose 注销并释放去重快照。hub 与 logs 由 Host 显式持有，必须比 sink 长寿。

`LogRouter(queueCapacity = 65536, drainBudget = 4096)` 限制积压与单批排空。`TryDispatch(entry)` 在容量不足时返回 false，`Dispatch(entry)` 明确抛出；Flush 所需 barrier 也受容量限制，满时需要由调用方重试。不能在日志 worker 自己的 callback 中等待自身退出。

[上一页：Diagnose](Inno.Core.Diagnostics.md) · [Core 索引](README.md) · [下一页：Mathematics](Inno.Core.Mathematics.md)

Logging 是基础、追加式日志系统。`LogRouter` 显式拥有一个 Host 的有界队列、过滤策略和 Sink；由 Host 显式选择 worker 或调用线程排空；核心不判断运行平台。`Log` 只是解析当前执行上下文的脚本便利门面。Compiler、Importer 和 Validator 的可替换当前问题属于独立的 [Inno.Core.Diagnostics](Inno.Core.Diagnostics.md)。

## 初始化

```csharp
using var router = new LogRouter();
router.RegisterSink(new ConsoleLogSink());
router.RegisterSink(new FileLogSink(Path.Combine(projectRoot, "Logs")));
router.SetMinimumLevel(LogLevel.Info);

using (router.EnterScope())
    Log.Info("Loaded {0} assets", [count]);

router.Flush();
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

Log.Info("Player spawned at {0}", [transform.localPosition]);
Log.Warn("Health is low: {0}", [health]);
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
using FileLogSink sink = new(
    logDirectory,
    maxFileSizeBytes: 10 * 1024 * 1024,
    maxFiles: 10);
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

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Core.Logging.ConsoleLogSink`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Logging.ConsoleLogSink`](../../src/foundation/core/Inno.Core.Logging/ConsoleLogSink.cs#L8) | Writes log entries to the process console, using level-based colors when a terminal is available. |
| [`Inno.Core.Logging.ConsoleLogSink.ConsoleLogSink(bool useColors = false)`](../../src/foundation/core/Inno.Core.Logging/ConsoleLogSink.cs#L18) | Creates a console sink with explicitly selected terminal capabilities. |
| [`void Inno.Core.Logging.ConsoleLogSink.Receive(Inno.Core.Logging.LogEntry entry)`](../../src/foundation/core/Inno.Core.Logging/ConsoleLogSink.cs#L26) | Writes the specified entry to standard output. |

### `Inno.Core.Logging.DiagnosticLogSink`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Logging.DiagnosticLogSink`](../../src/foundation/core/Inno.Core.Logging/DiagnosticLogSink.cs#L12) | Projects changed diagnostic issues into a host's log stream without introducing another diagnostic owner. |
| [`Inno.Core.Logging.DiagnosticLogSink.DiagnosticLogSink(Inno.Core.Diagnostics.DiagnosticHub hub, Inno.Core.Logging.LogRouter logs)`](../../src/foundation/core/Inno.Core.Logging/DiagnosticLogSink.cs#L28) | Registers a log presentation for all current and future reports in one hub. |
| [`void Inno.Core.Logging.DiagnosticLogSink.Clear(Inno.Core.Diagnostics.DiagnosticSource source)`](../../src/foundation/core/Inno.Core.Logging/DiagnosticLogSink.cs#L71) | Removes the current report for one diagnostic source. |
| [`void Inno.Core.Logging.DiagnosticLogSink.Dispose()`](../../src/foundation/core/Inno.Core.Logging/DiagnosticLogSink.cs#L76) | Unregisters this presentation and releases its neutral de-duplication snapshot. |
| [`void Inno.Core.Logging.DiagnosticLogSink.Replace(Inno.Core.Diagnostics.DiagnosticReport report)`](../../src/foundation/core/Inno.Core.Logging/DiagnosticLogSink.cs#L42) | Replaces the current report for one diagnostic source. |

### `Inno.Core.Logging.FileLogSink`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Logging.FileLogSink`](../../src/foundation/core/Inno.Core.Logging/FileLogSink.cs#L11) | Writes complete log entries to uniquely named rotating files under the router's delivery policy. |
| [`Inno.Core.Logging.FileLogSink.FileLogSink(string logDirectory, long maxFileSizeBytes = 10485760, int maxFiles = 10)`](../../src/foundation/core/Inno.Core.Logging/FileLogSink.cs#L55) | Opens an isolated log file without creating another queue or delivery worker. |
| [`const string Inno.Core.Logging.FileLogSink.C_LOG_FILE_PREFIX`](../../src/foundation/core/Inno.Core.Logging/FileLogSink.cs#L16) | Prefix used for generated log file names. |
| [`void Inno.Core.Logging.FileLogSink.Dispose()`](../../src/foundation/core/Inno.Core.Logging/FileLogSink.cs#L124) | Waits for an active write and closes the owned file, reporting any final flush failure. |
| [`void Inno.Core.Logging.FileLogSink.Receive(Inno.Core.Logging.LogEntry entry)`](../../src/foundation/core/Inno.Core.Logging/FileLogSink.cs#L101) | Writes and flushes one complete entry before returning, serializing concurrent callers. |

### `Inno.Core.Logging.ILogSink`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Logging.ILogSink`](../../src/foundation/core/Inno.Core.Logging/ILogSink.cs#L6) | Defines a log sink that consumes log entries from a . |
| [`void Inno.Core.Logging.ILogSink.Receive(Inno.Core.Logging.LogEntry entry)`](../../src/foundation/core/Inno.Core.Logging/ILogSink.cs#L14) | Receives a log entry for processing. |

### `Inno.Core.Logging.Log`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Logging.Log`](../../src/foundation/core/Inno.Core.Logging/Log.cs#L15) | Writes script logs with compiler-provided source locations and the calling assembly's ownership. |
| [`static void Inno.Core.Logging.Log.Debug(object? obj, string filePath = "", int lineNumber = 0)`](../../src/foundation/core/Inno.Core.Logging/Log.cs#L32) | Writes a debug-level message using the object's string representation. |
| [`static void Inno.Core.Logging.Log.Debug(string message, System.Collections.Generic.IReadOnlyList<object?>? arguments, string filePath = "", int lineNumber = 0)`](../../src/foundation/core/Inno.Core.Logging/Log.cs#L58) | Writes a formatted debug-level message with its source location. |
| [`static void Inno.Core.Logging.Log.Error(object? obj, string filePath = "", int lineNumber = 0)`](../../src/foundation/core/Inno.Core.Logging/Log.cs#L169) | Writes a error-level message using the object's string representation. |
| [`static void Inno.Core.Logging.Log.Error(string message, System.Collections.Generic.IReadOnlyList<object?>? arguments, string filePath = "", int lineNumber = 0)`](../../src/foundation/core/Inno.Core.Logging/Log.cs#L194) | Writes a formatted error-level message with its source location. |
| [`static void Inno.Core.Logging.Log.Fatal(object? obj, string filePath = "", int lineNumber = 0)`](../../src/foundation/core/Inno.Core.Logging/Log.cs#L214) | Writes a fatal-level message using the object's string representation. |
| [`static void Inno.Core.Logging.Log.Fatal(string message, System.Collections.Generic.IReadOnlyList<object?>? arguments, string filePath = "", int lineNumber = 0)`](../../src/foundation/core/Inno.Core.Logging/Log.cs#L239) | Writes a formatted fatal-level message with its source location. |
| [`static void Inno.Core.Logging.Log.Info(object? obj, string filePath = "", int lineNumber = 0)`](../../src/foundation/core/Inno.Core.Logging/Log.cs#L79) | Writes a info-level message using the object's string representation. |
| [`static void Inno.Core.Logging.Log.Info(string message, System.Collections.Generic.IReadOnlyList<object?>? arguments, string filePath = "", int lineNumber = 0)`](../../src/foundation/core/Inno.Core.Logging/Log.cs#L104) | Writes a formatted info-level message with its source location. |
| [`static void Inno.Core.Logging.Log.Warn(object? obj, string filePath = "", int lineNumber = 0)`](../../src/foundation/core/Inno.Core.Logging/Log.cs#L124) | Writes a warn-level message using the object's string representation. |
| [`static void Inno.Core.Logging.Log.Warn(string message, System.Collections.Generic.IReadOnlyList<object?>? arguments, string filePath = "", int lineNumber = 0)`](../../src/foundation/core/Inno.Core.Logging/Log.cs#L149) | Writes a formatted warn-level message with its source location. |

### `Inno.Core.Logging.LogDeliveryMode`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Logging.LogDeliveryMode`](../../src/foundation/core/Inno.Core.Logging/LogDeliveryMode.cs#L6) | Selects the delivery policy independently of the operating system or execution host. |
| [`Inno.Core.Logging.LogDeliveryMode.Background`](../../src/foundation/core/Inno.Core.Logging/LogDeliveryMode.cs#L11) | Delivers queued entries through a dedicated background worker. |
| [`Inno.Core.Logging.LogDeliveryMode.Inline`](../../src/foundation/core/Inno.Core.Logging/LogDeliveryMode.cs#L16) | Delivers entries synchronously on the producer's thread. |

### `Inno.Core.Logging.LogEntry`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Logging.LogEntry`](../../src/foundation/core/Inno.Core.Logging/LogEntry.cs#L37) | Represents an immutable log message dispatched by a . |
| [`readonly Inno.Core.Logging.LogLevel Inno.Core.Logging.LogEntry.level`](../../src/foundation/core/Inno.Core.Logging/LogEntry.cs#L51) | Gets the severity of this entry. |
| [`readonly Inno.Core.Logging.LogSessionId Inno.Core.Logging.LogEntry.sessionId`](../../src/foundation/core/Inno.Core.Logging/LogEntry.cs#L96) | Gets the isolated runtime session that produced this entry. |
| [`readonly Inno.Extensibility.Modules.AssemblyDomain Inno.Core.Logging.LogEntry.domain`](../../src/foundation/core/Inno.Core.Logging/LogEntry.cs#L56) | Gets the assembly ownership domain for this entry. |
| [`readonly Inno.Extensibility.Modules.AssemblyScope Inno.Core.Logging.LogEntry.scope`](../../src/foundation/core/Inno.Core.Logging/LogEntry.cs#L61) | Gets the runtime or editor scope for this entry. |
| [`readonly System.DateTime Inno.Core.Logging.LogEntry.time`](../../src/foundation/core/Inno.Core.Logging/LogEntry.cs#L76) | Gets the timestamp captured when this entry was created. |
| [`readonly int Inno.Core.Logging.LogEntry.line`](../../src/foundation/core/Inno.Core.Logging/LogEntry.cs#L86) | Gets the source line number if available. |
| [`readonly string Inno.Core.Logging.LogEntry.category`](../../src/foundation/core/Inno.Core.Logging/LogEntry.cs#L66) | Gets the category name for this entry. |
| [`readonly string Inno.Core.Logging.LogEntry.file`](../../src/foundation/core/Inno.Core.Logging/LogEntry.cs#L81) | Gets the source file name if available; otherwise a fallback name. |
| [`readonly string Inno.Core.Logging.LogEntry.message`](../../src/foundation/core/Inno.Core.Logging/LogEntry.cs#L71) | Gets the rendered message text. |
| [`readonly string Inno.Core.Logging.LogEntry.stackTrace`](../../src/foundation/core/Inno.Core.Logging/LogEntry.cs#L91) | Gets the managed stack trace captured at the logging call site. |

### `Inno.Core.Logging.LogLevel`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Logging.LogLevel`](../../src/foundation/core/Inno.Core.Logging/LogLevel.cs#L6) | Represents log severity levels in ascending order. |
| [`Inno.Core.Logging.LogLevel.Debug`](../../src/foundation/core/Inno.Core.Logging/LogLevel.cs#L11) | Diagnostic logs intended for local debugging. |
| [`Inno.Core.Logging.LogLevel.Error`](../../src/foundation/core/Inno.Core.Logging/LogLevel.cs#L26) | Error logs for failures that should be investigated. |
| [`Inno.Core.Logging.LogLevel.Fatal`](../../src/foundation/core/Inno.Core.Logging/LogLevel.cs#L31) | Fatal logs for unrecoverable failures. |
| [`Inno.Core.Logging.LogLevel.Info`](../../src/foundation/core/Inno.Core.Logging/LogLevel.cs#L16) | Informational logs for normal runtime flow. |
| [`Inno.Core.Logging.LogLevel.Warn`](../../src/foundation/core/Inno.Core.Logging/LogLevel.cs#L21) | Warning logs for recoverable problems. |

### `Inno.Core.Logging.LogRouter`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Logging.LogRouter`](../../src/foundation/core/Inno.Core.Logging/LogRouter.cs#L15) | Owns one host's bounded log queue, filtering policy, and sink collection. |
| [`Inno.Core.Logging.LogRouter.LogRouter(int queueCapacity = 65536, int drainBudget = 4096, Inno.Core.Logging.LogDeliveryMode deliveryMode = Inno.Core.Logging.LogDeliveryMode.Background)`](../../src/foundation/core/Inno.Core.Logging/LogRouter.cs#L55) | Creates an isolated logging router with an explicit delivery policy. |
| [`Inno.Core.Logging.Logger Inno.Core.Logging.LogRouter.CreateLogger<TOwner>()`](../../src/foundation/core/Inno.Core.Logging/LogRouter.cs#L167) | Creates a category-bound logger for one engine service or extension type. |
| [`System.Action<Inno.Core.Logging.ILogSink, System.Exception>? Inno.Core.Logging.LogRouter.sinkFailed`](../../src/foundation/core/Inno.Core.Logging/LogRouter.cs#L38) | Occurs after a failing sink has been quarantined from this router. |
| [`System.IDisposable Inno.Core.Logging.LogRouter.EnterScope()`](../../src/foundation/core/Inno.Core.Logging/LogRouter.cs#L88) | Binds this router to the current asynchronous execution context. |
| [`bool Inno.Core.Logging.LogRouter.TryDispatch(Inno.Core.Logging.LogEntry entry)`](../../src/foundation/core/Inno.Core.Logging/LogRouter.cs#L212) | Attempts to queue an immutable entry, draining it inline when the host has no logging worker. |
| [`void Inno.Core.Logging.LogRouter.Dispatch(Inno.Core.Logging.LogEntry entry)`](../../src/foundation/core/Inno.Core.Logging/LogRouter.cs#L190) | Queues an immutable entry under the configured worker or inline delivery policy. |
| [`void Inno.Core.Logging.LogRouter.Dispose()`](../../src/foundation/core/Inno.Core.Logging/LogRouter.cs#L263) | Drains pending entries, stops the worker, and disposes every sink still owned by this router. |
| [`void Inno.Core.Logging.LogRouter.Flush()`](../../src/foundation/core/Inno.Core.Logging/LogRouter.cs#L237) | Blocks until every entry enqueued before this call has reached the current sink snapshot. |
| [`void Inno.Core.Logging.LogRouter.RegisterSink(Inno.Core.Logging.ILogSink sink)`](../../src/foundation/core/Inno.Core.Logging/LogRouter.cs#L106) | Registers a sink to receive future entries from this router. |
| [`void Inno.Core.Logging.LogRouter.SetMinimumLevel(Inno.Core.Logging.LogLevel level)`](../../src/foundation/core/Inno.Core.Logging/LogRouter.cs#L149) | Sets the lowest severity accepted by this router. |
| [`void Inno.Core.Logging.LogRouter.UnregisterSink(Inno.Core.Logging.ILogSink sink)`](../../src/foundation/core/Inno.Core.Logging/LogRouter.cs#L130) | Unregisters a sink so it receives no future entries from this router. |

### `Inno.Core.Logging.LogSessionContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Logging.LogSessionContext`](../../src/foundation/core/Inno.Core.Logging/LogSessionContext.cs#L10) | Associates logs written on one asynchronous execution flow with an isolated runtime session. |
| [`static Inno.Core.Logging.LogSessionId Inno.Core.Logging.LogSessionContext.current`](../../src/foundation/core/Inno.Core.Logging/LogSessionContext.cs#L18) | Gets the runtime session associated with the current asynchronous execution flow. |
| [`static System.IDisposable Inno.Core.Logging.LogSessionContext.Enter(Inno.Core.Logging.LogSessionId sessionId)`](../../src/foundation/core/Inno.Core.Logging/LogSessionContext.cs#L32) | Enters a nested execution scope whose emitted logs belong to the supplied runtime session. |

### `Inno.Core.Logging.LogSessionId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Logging.LogSessionId`](../../src/foundation/core/Inno.Core.Logging/LogSessionId.cs#L8) | Identifies the isolated runtime session that produced a log entry. |
| [`bool Inno.Core.Logging.LogSessionId.isAssigned`](../../src/foundation/core/Inno.Core.Logging/LogSessionId.cs#L25) | Gets whether this identifier names an isolated runtime session. |
| [`override string Inno.Core.Logging.LogSessionId.ToString()`](../../src/foundation/core/Inno.Core.Logging/LogSessionId.cs#L41) | Returns the stable textual representation used by diagnostics and persisted logs. |
| [`static Inno.Core.Logging.LogSessionId Inno.Core.Logging.LogSessionId.Create()`](../../src/foundation/core/Inno.Core.Logging/LogSessionId.cs#L33) | Creates a unique identifier for a new isolated runtime session. |
| [`static Inno.Core.Logging.LogSessionId Inno.Core.Logging.LogSessionId.none`](../../src/foundation/core/Inno.Core.Logging/LogSessionId.cs#L20) | Gets an identifier that represents process-level work outside an isolated runtime session. |

### `Inno.Core.Logging.Logger`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Logging.Logger`](../../src/foundation/core/Inno.Core.Logging/Logger.cs#L14) | Writes category-bound entries through one explicitly owned log router. |
| [`void Inno.Core.Logging.Logger.Write(Inno.Core.Logging.LogLevel level, string message, System.Collections.Generic.IReadOnlyList<object?>? arguments = null, string filePath = "", int lineNumber = 0)`](../../src/foundation/core/Inno.Core.Logging/Logger.cs#L57) | Writes one formatted entry with source information captured from the caller. |

## 项目依赖

- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Modules](../extensibility/Inno.Extensibility.Modules.md)：公开引用边界由实际签名核对。
- [Inno.Core.Diagnostics](Inno.Core.Diagnostics.md)：公开引用边界由实际签名核对。
- [Inno.Core.Execution](Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
