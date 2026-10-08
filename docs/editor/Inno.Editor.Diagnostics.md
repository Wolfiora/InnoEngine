# Inno.Editor.Diagnostics

[Editor 索引](README.md) · [Logging Panel](Inno.Editor.Panel.Logging.md) · [Core Logging](../core/Inno.Core.Logging.md)

该 project 是 Editor Console 后端，不绘制 ImGui。它组合 `ILogSink` 与 `IDiagnosticSink`，按 Session identity 保存 occurrence、执行 Clear-on-Play policy，并生成不可变 collapse snapshot。

## 公开 API

- `IEditorConsole`：Panel/feature 使用的只读 snapshot 和过滤入口。
- `EditorConsole`：Application Composition Root 创建和拥有的实现。
- `EditorConsoleSnapshot`, `EditorConsoleGroup`, `EditorConsoleOccurrence`：不可变展示模型。
- `EditorConsoleEntryKind`：Log/Diagnostic domain。
- `IEditorConsole.clearOnPlay`：Console backend 的当前有效策略；正式用户入口位于 `Editor/Diagnostics/Console/Clear on Play` Settings，默认开启，current diagnostics 始终按 producer report 生命周期管理。

Fingerprint 包含 domain、severity、source、code、message、location、stack identity 和 `LogSessionId`；同一 Session 的非连续等价项全局聚合，不同 Session 不会误合并。Buffer、capacity queue、fingerprint builder 与 clear-on-play implementation 均保持 internal。

Console timeline 与 collapse group 都按 occurrence sequence 正序排列：最旧在上、最新在下。Panel 仅在用户已经位于底部或明确请求滚动时跟随新条目；用户向上查看历史后不会被强制拉回。因此新启动错误显示在最下方是正常的时间线语义，不是 Error 优先级异常。编译器、Importer、Rendering 等可恢复问题使用 `DiagnosticHub` 的当前状态报告；普通 `Log` 才保留历史调用栈。

`Clear()` 只清理历史 Log；当前 Diagnostic 的权威数据属于 producer，直到 producer 报告恢复才撤销。相同内容的 Issue 重复发布不会刷新 Console 时间线；Rendering 的帧级 Issue 在下一次成功帧自动对账清退。Panel header 显示 `[Issue]` 与 `[Log]`，便于识别当前问题和过去事件。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Editor.Diagnostics.EditorConsole`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Diagnostics.EditorConsole`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsole.cs#L15) | Owns one editor's bounded Console history, current diagnostics, grouping, and Play Mode clearing policy. |
| [`Inno.Editor.Diagnostics.EditorConsole.EditorConsole(Inno.Core.Logging.LogRouter logRouter, Inno.Core.Diagnostics.DiagnosticHub diagnosticHub, Inno.Editor.PlayMode.IEditorPlayMode playMode)`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsole.cs#L45) | Creates a Console service that observes one editor Play Mode controller. |
| [`Inno.Editor.Diagnostics.EditorConsoleSnapshot Inno.Editor.Diagnostics.EditorConsole.Capture()`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsole.cs#L158) | Captures an immutable snapshot containing both individual occurrences and global groups. |
| [`bool Inno.Editor.Diagnostics.EditorConsole.clearOnPlay`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsole.cs#L83) | Gets or sets whether ordinary Console logs are cleared when a new Play Mode request begins. Current diagnostics remain visible because they represent active compiler and subsystem state. |
| [`int Inno.Editor.Diagnostics.EditorConsole.capacity`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsole.cs#L58) | Gets or sets the maximum number of ordinary log occurrences retained in memory. |
| [`void Inno.Editor.Diagnostics.EditorConsole.Clear()`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsole.cs#L178) | Removes retained logs while keeping currently reported issues visible until their owners resolve them. |
| [`void Inno.Editor.Diagnostics.EditorConsole.Clear(Inno.Core.Diagnostics.DiagnosticSource source)`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsole.cs#L143) | Removes the current diagnostic report for one producer. |
| [`void Inno.Editor.Diagnostics.EditorConsole.Dispose()`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsole.cs#L192) | Detaches this service and releases its subscriptions. |
| [`void Inno.Editor.Diagnostics.EditorConsole.Receive(Inno.Core.Logging.LogEntry entry)`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsole.cs#L108) | Appends one asynchronously dispatched log entry. |
| [`void Inno.Editor.Diagnostics.EditorConsole.Replace(Inno.Core.Diagnostics.DiagnosticReport report)`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsole.cs#L124) | Replaces the complete current diagnostic report for one producer. |
| [`void Inno.Editor.Diagnostics.EditorConsole.Start()`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsole.cs#L91) | Attaches this service to the process logging, diagnostic, and Play Mode sources. |

### `Inno.Editor.Diagnostics.EditorConsoleEntryKind`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Diagnostics.EditorConsoleEntryKind`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleEntryKind.cs#L6) | Identifies the source protocol represented by one editor Console occurrence. |
| [`Inno.Editor.Diagnostics.EditorConsoleEntryKind.Diagnostic`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleEntryKind.cs#L16) | The occurrence represents current state from a diagnostic producer. |
| [`Inno.Editor.Diagnostics.EditorConsoleEntryKind.Log`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleEntryKind.cs#L11) | The occurrence was emitted through the runtime logging pipeline. |

### `Inno.Editor.Diagnostics.EditorConsoleGroup`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Diagnostics.EditorConsoleGroup`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleGroup.cs#L9) | Groups equivalent Console occurrences regardless of whether they arrived consecutively. |
| [`Inno.Editor.Diagnostics.EditorConsoleOccurrence Inno.Editor.Diagnostics.EditorConsoleGroup.latest`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleGroup.cs#L33) | Gets the most recently received matching occurrence. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Editor.Diagnostics.EditorConsoleOccurrence> Inno.Editor.Diagnostics.EditorConsoleGroup.occurrences`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleGroup.cs#L28) | Gets every matching occurrence in arrival order. |
| [`int Inno.Editor.Diagnostics.EditorConsoleGroup.count`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleGroup.cs#L38) | Gets the total number of matching occurrences. |
| [`string Inno.Editor.Diagnostics.EditorConsoleGroup.identity`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleGroup.cs#L23) | Gets the deterministic content fingerprint used as the presentation identity. |

### `Inno.Editor.Diagnostics.EditorConsoleOccurrence`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Logging.LogLevel Inno.Editor.Diagnostics.EditorConsoleOccurrence.level`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleOccurrence.cs#L57) | Gets the normalized Console severity. |
| [`Inno.Core.Logging.LogSessionId Inno.Editor.Diagnostics.EditorConsoleOccurrence.sessionId`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleOccurrence.cs#L117) | Gets the isolated runtime session that emitted this occurrence. |
| [`Inno.Editor.Diagnostics.EditorConsoleEntryKind Inno.Editor.Diagnostics.EditorConsoleOccurrence.kind`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleOccurrence.cs#L52) | Gets the source protocol represented by this occurrence. |
| [`Inno.Editor.Diagnostics.EditorConsoleOccurrence`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleOccurrence.cs#L10) | Describes one immutable log or diagnostic occurrence in the editor Console timeline. |
| [`System.DateTime Inno.Editor.Diagnostics.EditorConsoleOccurrence.time`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleOccurrence.cs#L92) | Gets the occurrence timestamp. |
| [`int Inno.Editor.Diagnostics.EditorConsoleOccurrence.column`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleOccurrence.cs#L107) | Gets the one-based source column, or zero when unavailable. |
| [`int Inno.Editor.Diagnostics.EditorConsoleOccurrence.line`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleOccurrence.cs#L102) | Gets the one-based source line, or zero when unavailable. |
| [`long Inno.Editor.Diagnostics.EditorConsoleOccurrence.sequence`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleOccurrence.cs#L47) | Gets the process-local monotonic sequence assigned when this occurrence entered the Console. |
| [`string Inno.Editor.Diagnostics.EditorConsoleOccurrence.category`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleOccurrence.cs#L77) | Gets the producer-defined category. |
| [`string Inno.Editor.Diagnostics.EditorConsoleOccurrence.code`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleOccurrence.cs#L72) | Gets the diagnostic code, or an empty string for uncoded entries. |
| [`string Inno.Editor.Diagnostics.EditorConsoleOccurrence.displayMessage`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleOccurrence.cs#L87) | Gets the message text prefixed by its diagnostic code when one is present. |
| [`string Inno.Editor.Diagnostics.EditorConsoleOccurrence.file`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleOccurrence.cs#L97) | Gets the related source path, or an empty string when unavailable. |
| [`string Inno.Editor.Diagnostics.EditorConsoleOccurrence.message`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleOccurrence.cs#L82) | Gets the original message text. |
| [`string Inno.Editor.Diagnostics.EditorConsoleOccurrence.source`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleOccurrence.cs#L62) | Gets the human-readable producer name. |
| [`string Inno.Editor.Diagnostics.EditorConsoleOccurrence.sourceId`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleOccurrence.cs#L67) | Gets the stable diagnostic producer identifier, or an empty string for log entries. |
| [`string Inno.Editor.Diagnostics.EditorConsoleOccurrence.stackTrace`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleOccurrence.cs#L112) | Gets the captured stack trace, or an empty string when unavailable. |

### `Inno.Editor.Diagnostics.EditorConsoleSnapshot`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Diagnostics.EditorConsoleSnapshot`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleSnapshot.cs#L9) | Provides one immutable and internally consistent view of editor Console state. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Editor.Diagnostics.EditorConsoleGroup> Inno.Editor.Diagnostics.EditorConsoleSnapshot.groups`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleSnapshot.cs#L34) | Gets global fingerprint groups ordered by their latest occurrence. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Editor.Diagnostics.EditorConsoleOccurrence> Inno.Editor.Diagnostics.EditorConsoleSnapshot.occurrences`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleSnapshot.cs#L29) | Gets every current occurrence in chronological arrival order. |
| [`long Inno.Editor.Diagnostics.EditorConsoleSnapshot.revision`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleSnapshot.cs#L24) | Gets the monotonic revision of the captured Console state. |

### `Inno.Editor.Diagnostics.IEditorConsole`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Diagnostics.EditorConsoleSnapshot Inno.Editor.Diagnostics.IEditorConsole.Capture()`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/IEditorConsole.cs#L25) | Captures an immutable snapshot containing both individual occurrences and global groups. |
| [`Inno.Editor.Diagnostics.IEditorConsole`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/IEditorConsole.cs#L6) | Exposes read-only editor Console snapshots and explicit retention controls to editor features. |
| [`bool Inno.Editor.Diagnostics.IEditorConsole.clearOnPlay`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/IEditorConsole.cs#L17) | Gets or sets whether ordinary Console logs are cleared when a new Play Mode request begins. Current diagnostics remain visible because they represent active compiler and subsystem state. |
| [`int Inno.Editor.Diagnostics.IEditorConsole.capacity`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/IEditorConsole.cs#L11) | Gets or sets the maximum number of ordinary log occurrences retained in memory. |
| [`void Inno.Editor.Diagnostics.IEditorConsole.Clear()`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/IEditorConsole.cs#L30) | Removes retained logs while keeping currently reported issues visible until their owners resolve them. |

## 项目依赖

- [Inno.Editor.PlayMode](Inno.Editor.PlayMode.md)：公开引用边界由实际签名核对。
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：公开引用边界由实际签名核对。
- [Inno.Core.Logging](../core/Inno.Core.Logging.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
