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
    Log.Info("Loaded {0} assets", count);

router.Flush();
```

`Dispose` 会停止 worker、排空队列，并 Dispose 所有实现 `IDisposable` 的已注册 sink。`EngineHost` 正常情况下统一拥有并释放 Router。

## Log

每个等级都有 `object?` 与 `(string message, params object[]? args)` overload：

- `Debug`：仅在 `DEBUG` 条件编译存在调用。
- `Info`
- `Warn`
- `Error`
- `Fatal`

格式化采用 `string.Format`。调用方类型名作为 `category`；调用程序集的当前分类解析为 `domain` 与 `scope`。分类使用弱缓存，不固定热重载 ALC；日志 entry 只复制 enum、字符串、时间与行号，不保存调用方 `Type`/`Assembly`。

`Log` 已作为 Runtime Scripting API 导出到逻辑 namespace `InnoEngine.Logging`。Project 脚本不引用真实的 `Inno.Core.Logging` namespace：

```csharp
using InnoEngine.Logging;

Log.Info("Player spawned at {0}", transform.localPosition);
Log.Warn("Health is low: {0}", health);
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
| `category` | `string` | 通常是调用类型名。 |
| `message` | `string` | 已渲染文本。 |
| `time` | `DateTime` | 构造时本地时间。 |
| `file` | `string` | 调试符号可用时的调用文件。 |
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
