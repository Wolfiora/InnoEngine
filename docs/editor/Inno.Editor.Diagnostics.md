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

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Editor.Diagnostics.EditorConsole`

<!-- api: T:Inno.Editor.Diagnostics.EditorConsole b3dacacf951791cb34026255af7fe0a045ecc5afb7c059d7faf698ba780e3096 -->
<!-- api: M:Inno.Editor.Diagnostics.EditorConsole.#ctor(Inno.Core.Logging.LogRouter,Inno.Core.Diagnostics.DiagnosticHub,Inno.Editor.PlayMode.IEditorPlayMode) 51dc394ee92a754a68bd0e26d7c953f2e31fab7230c9926dd865e1c4232a2d10 -->
<!-- api: P:Inno.Editor.Diagnostics.EditorConsole.capacity b1c4c06de6dd97d40f3cec4229a4fe6e310c5300ee77fd1bb6b5ea61b502d2f0 -->
<!-- api: P:Inno.Editor.Diagnostics.EditorConsole.clearOnPlay 1fdaf83d8e62c09fb64bf83c8d830f5b175d557ddbaf748b5e403ea25a7b6866 -->
<!-- api: M:Inno.Editor.Diagnostics.EditorConsole.Start fa103043609b2a0243bbe7d869c51d17e6959b4804f5f2dbe7c374d8ab483f56 -->
<!-- api: M:Inno.Editor.Diagnostics.EditorConsole.Receive(Inno.Core.Logging.LogEntry) 0db96729c293dcbe033552e0fbb43eb2b395a1fc0bb0cecca84b0c155896c1a3 -->
<!-- api: M:Inno.Editor.Diagnostics.EditorConsole.Replace(Inno.Core.Diagnostics.DiagnosticReport) 773e3b28627168b989c7e5fdcdd8e2547f89f3ea957d8d7abb429fe81d177b65 -->
<!-- api: M:Inno.Editor.Diagnostics.EditorConsole.Clear(Inno.Core.Diagnostics.DiagnosticSource) 7133d58af82dc1316782aff7b9da1ba992560190090d4057e49336566cbc0007 -->
<!-- api: M:Inno.Editor.Diagnostics.EditorConsole.Capture f58fc8bfdba06f9a99b9e2ea7e9076b283216a962edcedaa5400f0a86b5ce3fe -->
<!-- api: M:Inno.Editor.Diagnostics.EditorConsole.Clear 99546c2ab38348eb7484765ef2554c1bf93048fe005f4ffb7770574da6523a8e -->
<!-- api: M:Inno.Editor.Diagnostics.EditorConsole.Dispose 496326ddb9be5cbeb949348253365a85ddd26c81ac2e350717bc57eb38c685d8 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8a43e4a0cc1800f0"></a>[`public Inno.Editor.Diagnostics.EditorConsole.EditorConsole(Inno.Core.Logging.LogRouter logRouter, Inno.Core.Diagnostics.DiagnosticHub diagnosticHub, Inno.Editor.PlayMode.IEditorPlayMode playMode)`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsole.cs#L45) | Creates a Console service that observes one editor Play Mode controller.<br>`logRouter`：The host logging router whose entries are presented by this Console.<br>`diagnosticHub`：The host diagnostic hub whose current reports are presented by this Console.<br>`playMode`：The Play Mode source used to clear ordinary logs when a new simulation request begins. | — | `System.ArgumentNullException`：Thrown when playMode is null. |
| <a id="api-82339b252b828490"></a>[`public Inno.Editor.Diagnostics.EditorConsoleSnapshot Inno.Editor.Diagnostics.EditorConsole.Capture()`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsole.cs#L158) | Captures an immutable snapshot containing both individual occurrences and global groups.<br>返回：A consistent snapshot that remains valid while new entries arrive. | — | — |
| <a id="api-57bdf5cb23de65f0"></a>[`public void Inno.Editor.Diagnostics.EditorConsole.Clear()`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsole.cs#L178) | Removes retained logs while keeping currently reported issues visible until their owners resolve them. | — | — |
| <a id="api-4befd79f78ae09ce"></a>[`public void Inno.Editor.Diagnostics.EditorConsole.Clear(Inno.Core.Diagnostics.DiagnosticSource source)`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsole.cs#L143) | Removes the current diagnostic report for one producer.<br>`source`：The producer whose report should be removed. | — | — |
| <a id="api-e3868c17beae9b69"></a>[`public void Inno.Editor.Diagnostics.EditorConsole.Dispose()`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsole.cs#L192) | Detaches this service and releases its subscriptions. | — | — |
| <a id="api-7d5af33ead7c6d33"></a>[`public void Inno.Editor.Diagnostics.EditorConsole.Receive(Inno.Core.Logging.LogEntry entry)`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsole.cs#L108) | Appends one asynchronously dispatched log entry.<br>`entry`：The immutable entry to append. | — | — |
| <a id="api-b8ae7cf4fd3d1ad5"></a>[`public void Inno.Editor.Diagnostics.EditorConsole.Replace(Inno.Core.Diagnostics.DiagnosticReport report)`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsole.cs#L124) | Replaces the complete current diagnostic report for one producer.<br>`report`：The complete report to expose in subsequent snapshots. | — | — |
| <a id="api-4f085cbef4406d66"></a>[`public void Inno.Editor.Diagnostics.EditorConsole.Start()`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsole.cs#L91) | Attaches this service to the process logging, diagnostic, and Play Mode sources. | — | `System.ObjectDisposedException`：Thrown after this service has been disposed. |
| <a id="api-46f5de09454d3db8"></a>[`public int Inno.Editor.Diagnostics.EditorConsole.capacity { get; set; }`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsole.cs#L58) | Gets or sets the maximum number of ordinary log occurrences retained in memory. | — | — |
| <a id="api-2721ee148aeddcd1"></a>[`public bool Inno.Editor.Diagnostics.EditorConsole.clearOnPlay { get; set; }`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsole.cs#L83) | Gets or sets whether ordinary Console logs are cleared when a new Play Mode request begins. Current diagnostics remain visible because they represent active compiler and subsystem state. | — | — |
| <a id="api-bfb085ac3056ebf9"></a>[`public sealed class Inno.Editor.Diagnostics.EditorConsole : Inno.Editor.Diagnostics.IEditorConsole, Inno.Core.Logging.ILogSink, Inno.Core.Diagnostics.IDiagnosticSink, System.IDisposable`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsole.cs#L15) | Owns one editor's bounded Console history, current diagnostics, grouping, and Play Mode clearing policy. | — | — |

### `Inno.Editor.Diagnostics.EditorConsoleEntryKind`

<!-- api: T:Inno.Editor.Diagnostics.EditorConsoleEntryKind 629ab13b9ff477b5bc7ad0603620b36fc270dd7d5977dda5cb7a252ccaf54188 -->
<!-- api: F:Inno.Editor.Diagnostics.EditorConsoleEntryKind.Log 784b1ee078d607165f0fdd3874079500711f7a637fbbf7caf21669691d3884dc -->
<!-- api: F:Inno.Editor.Diagnostics.EditorConsoleEntryKind.Diagnostic 3239cc0f1cd241f23fddad8236e09484e2ab14374cf2f413765de658e874be97 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-61e5a017d674b960"></a>[`Inno.Editor.Diagnostics.EditorConsoleEntryKind.Diagnostic = 1`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleEntryKind.cs#L16) | The occurrence represents current state from a diagnostic producer. | — | — |
| <a id="api-539279507005bfa8"></a>[`Inno.Editor.Diagnostics.EditorConsoleEntryKind.Log = 0`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleEntryKind.cs#L11) | The occurrence was emitted through the runtime logging pipeline. | — | — |
| <a id="api-bb386b982bf37b2e"></a>[`public enum Inno.Editor.Diagnostics.EditorConsoleEntryKind : int`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleEntryKind.cs#L6) | Identifies the source protocol represented by one editor Console occurrence. | — | — |

### `Inno.Editor.Diagnostics.EditorConsoleGroup`

<!-- api: T:Inno.Editor.Diagnostics.EditorConsoleGroup 57fcdaea1c474252ded5dac552b4c58c290787e7f1f8933415cf5fdc741adc7a -->
<!-- api: P:Inno.Editor.Diagnostics.EditorConsoleGroup.identity dbe70ecc458d51b5ef50077fb95647d3df5ae985555e7943fcdcafffbcd8aa5c -->
<!-- api: P:Inno.Editor.Diagnostics.EditorConsoleGroup.occurrences 527857ff1db86a80ae9f838391adf0d598bd0cdbf9c74e775ba11ed5b3e201a6 -->
<!-- api: P:Inno.Editor.Diagnostics.EditorConsoleGroup.latest dec3f3dad9dc322a104ea5d3a2a32de3903d3eb8b72bbaf4632810a8e7c22ce9 -->
<!-- api: P:Inno.Editor.Diagnostics.EditorConsoleGroup.count c8c35e1761969b0bd3cf4832e752f93b7f72a9285575698605b7fa44ba358f72 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2612f0c056716db0"></a>[`public int Inno.Editor.Diagnostics.EditorConsoleGroup.count { get; }`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleGroup.cs#L38) | Gets the total number of matching occurrences. | — | — |
| <a id="api-757ebc50edbf9ec5"></a>[`public string Inno.Editor.Diagnostics.EditorConsoleGroup.identity { get; }`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleGroup.cs#L23) | Gets the deterministic content fingerprint used as the presentation identity. | — | — |
| <a id="api-02a6bb4fdc57f0ac"></a>[`public Inno.Editor.Diagnostics.EditorConsoleOccurrence Inno.Editor.Diagnostics.EditorConsoleGroup.latest { get; }`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleGroup.cs#L33) | Gets the most recently received matching occurrence. | — | — |
| <a id="api-59c37af467069de1"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Editor.Diagnostics.EditorConsoleOccurrence> Inno.Editor.Diagnostics.EditorConsoleGroup.occurrences { get; }`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleGroup.cs#L28) | Gets every matching occurrence in arrival order. | — | — |
| <a id="api-5b6004b1aaadc4e4"></a>[`public sealed class Inno.Editor.Diagnostics.EditorConsoleGroup`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleGroup.cs#L9) | Groups equivalent Console occurrences regardless of whether they arrived consecutively. | — | — |

### `Inno.Editor.Diagnostics.EditorConsoleOccurrence`

<!-- api: T:Inno.Editor.Diagnostics.EditorConsoleOccurrence 51b2174ea982393f677d0b6fcbcf0b69e6d4bc04ecc8be9b91a32d8e3d349cbd -->
<!-- api: P:Inno.Editor.Diagnostics.EditorConsoleOccurrence.sequence f769f922e9299615be6262d858548a043aff7caf1f7ab01a517f2043bc90bd08 -->
<!-- api: P:Inno.Editor.Diagnostics.EditorConsoleOccurrence.kind 43ce2ffdbaf0558553c5ecc036fe7583378e5727af201c3fcfe0e4b3c3bb9164 -->
<!-- api: P:Inno.Editor.Diagnostics.EditorConsoleOccurrence.level 2e3c07f6c9c553761e06a9f01e1608318adaa791a9ea3dcfe0222abf615a06d3 -->
<!-- api: P:Inno.Editor.Diagnostics.EditorConsoleOccurrence.source f163ae6b1f26261a71db0138ac19e9974fb0ffffb75bd417543e877fd906cf9e -->
<!-- api: P:Inno.Editor.Diagnostics.EditorConsoleOccurrence.sourceId 63912ab3f45b7b1c2a7c124b49af765d8ccedd923a851349e39bec7a18864128 -->
<!-- api: P:Inno.Editor.Diagnostics.EditorConsoleOccurrence.code 68281562e4b6b877d88d920208cbc2cd3fcf903c66854328ae4a5a99c543097b -->
<!-- api: P:Inno.Editor.Diagnostics.EditorConsoleOccurrence.category 1806ddca9d9d04e80f55c6b1596d87f7f978ee7afb5578bf9290c70e7bf0e3b3 -->
<!-- api: P:Inno.Editor.Diagnostics.EditorConsoleOccurrence.message 95ba159d0e34f611b9a7c403e76e9d15213a43c134d8aef9d753e193bef3ec99 -->
<!-- api: P:Inno.Editor.Diagnostics.EditorConsoleOccurrence.displayMessage 4056dcc71f886d27cf168f103f95feb00688595471c74d0fbf8e79d9ffa1cf46 -->
<!-- api: P:Inno.Editor.Diagnostics.EditorConsoleOccurrence.time 593c3974f0c46635a93bdeb0f6a5865ceb4a87d0261a5a565d7f0b5ce91c0683 -->
<!-- api: P:Inno.Editor.Diagnostics.EditorConsoleOccurrence.file 24658803941aa9fa2c8ae011eede1c4ddcccfde849709595a2ffc142a7366166 -->
<!-- api: P:Inno.Editor.Diagnostics.EditorConsoleOccurrence.line 5eb7a4b635f7af04c947ac33d78a36b3b854036082b791a6e3345dddcb63993f -->
<!-- api: P:Inno.Editor.Diagnostics.EditorConsoleOccurrence.column 0721c99d517ba53ae6a4920e7e497cf591e4a62403d5c91b7e2ff14fe7ddb3bb -->
<!-- api: P:Inno.Editor.Diagnostics.EditorConsoleOccurrence.stackTrace 0814141162fc25da224b7c83c4cdbe17ade1bc07ced2445c2f21a0c4dbd5ee1d -->
<!-- api: P:Inno.Editor.Diagnostics.EditorConsoleOccurrence.sessionId 00f5e1f67ea584a1743c5256ceaec03b1c9dc77f6c69642e948b248940fb21b1 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7e590b61b2a50f49"></a>[`public string Inno.Editor.Diagnostics.EditorConsoleOccurrence.category { get; }`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleOccurrence.cs#L77) | Gets the producer-defined category. | — | — |
| <a id="api-711f16e164ba3b08"></a>[`public string Inno.Editor.Diagnostics.EditorConsoleOccurrence.code { get; }`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleOccurrence.cs#L72) | Gets the diagnostic code, or an empty string for uncoded entries. | — | — |
| <a id="api-34cc794cbf62a9de"></a>[`public int Inno.Editor.Diagnostics.EditorConsoleOccurrence.column { get; }`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleOccurrence.cs#L107) | Gets the one-based source column, or zero when unavailable. | — | — |
| <a id="api-54366cdbb3ab640c"></a>[`public string Inno.Editor.Diagnostics.EditorConsoleOccurrence.displayMessage { get; }`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleOccurrence.cs#L87) | Gets the message text prefixed by its diagnostic code when one is present. | — | — |
| <a id="api-169a39679855b384"></a>[`public string Inno.Editor.Diagnostics.EditorConsoleOccurrence.file { get; }`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleOccurrence.cs#L97) | Gets the related source path, or an empty string when unavailable. | — | — |
| <a id="api-6263895c1ded5863"></a>[`public Inno.Editor.Diagnostics.EditorConsoleEntryKind Inno.Editor.Diagnostics.EditorConsoleOccurrence.kind { get; }`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleOccurrence.cs#L52) | Gets the source protocol represented by this occurrence. | — | — |
| <a id="api-60e78b56c9032844"></a>[`public Inno.Core.Logging.LogLevel Inno.Editor.Diagnostics.EditorConsoleOccurrence.level { get; }`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleOccurrence.cs#L57) | Gets the normalized Console severity. | — | — |
| <a id="api-495dff8431b940dc"></a>[`public int Inno.Editor.Diagnostics.EditorConsoleOccurrence.line { get; }`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleOccurrence.cs#L102) | Gets the one-based source line, or zero when unavailable. | — | — |
| <a id="api-e325bb813cd81daa"></a>[`public string Inno.Editor.Diagnostics.EditorConsoleOccurrence.message { get; }`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleOccurrence.cs#L82) | Gets the original message text. | — | — |
| <a id="api-6951d2d200c8bbc6"></a>[`public long Inno.Editor.Diagnostics.EditorConsoleOccurrence.sequence { get; }`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleOccurrence.cs#L47) | Gets the process-local monotonic sequence assigned when this occurrence entered the Console. | — | — |
| <a id="api-b11a651e42848def"></a>[`public Inno.Core.Logging.LogSessionId Inno.Editor.Diagnostics.EditorConsoleOccurrence.sessionId { get; }`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleOccurrence.cs#L117) | Gets the isolated runtime session that emitted this occurrence. | — | — |
| <a id="api-4b986f5d061ae3aa"></a>[`public string Inno.Editor.Diagnostics.EditorConsoleOccurrence.source { get; }`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleOccurrence.cs#L62) | Gets the human-readable producer name. | — | — |
| <a id="api-63b75e4e542579b0"></a>[`public string Inno.Editor.Diagnostics.EditorConsoleOccurrence.sourceId { get; }`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleOccurrence.cs#L67) | Gets the stable diagnostic producer identifier, or an empty string for log entries. | — | — |
| <a id="api-411ff9c0d387c06c"></a>[`public string Inno.Editor.Diagnostics.EditorConsoleOccurrence.stackTrace { get; }`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleOccurrence.cs#L112) | Gets the captured stack trace, or an empty string when unavailable. | — | — |
| <a id="api-590804c30b1693e4"></a>[`public System.DateTime Inno.Editor.Diagnostics.EditorConsoleOccurrence.time { get; }`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleOccurrence.cs#L92) | Gets the occurrence timestamp. | — | — |
| <a id="api-d5ee5c552f8f19e2"></a>[`public sealed class Inno.Editor.Diagnostics.EditorConsoleOccurrence`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleOccurrence.cs#L10) | Describes one immutable log or diagnostic occurrence in the editor Console timeline. | — | — |

### `Inno.Editor.Diagnostics.EditorConsoleSnapshot`

<!-- api: T:Inno.Editor.Diagnostics.EditorConsoleSnapshot 1794700627815f493a038449276f32f82c455b2af384b8a723e9d473b927bfba -->
<!-- api: P:Inno.Editor.Diagnostics.EditorConsoleSnapshot.revision 5753667793d15315554a9ef6d9ae405a443f704925ebdaf0167c0c97514b6f3c -->
<!-- api: P:Inno.Editor.Diagnostics.EditorConsoleSnapshot.occurrences 44025482435e34ae9eadad18aa64c057e2017ab00ee9ad33426291f0b2bac66a -->
<!-- api: P:Inno.Editor.Diagnostics.EditorConsoleSnapshot.groups 4c917775297182cd7b023896216943fd4985dad4febaf0e43f54d164178187a8 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2cbebbe055e7aa7b"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Editor.Diagnostics.EditorConsoleGroup> Inno.Editor.Diagnostics.EditorConsoleSnapshot.groups { get; }`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleSnapshot.cs#L34) | Gets global fingerprint groups ordered by their latest occurrence. | — | — |
| <a id="api-e923ef6987b13f5c"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Editor.Diagnostics.EditorConsoleOccurrence> Inno.Editor.Diagnostics.EditorConsoleSnapshot.occurrences { get; }`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleSnapshot.cs#L29) | Gets every current occurrence in chronological arrival order. | — | — |
| <a id="api-31d245a5a085b821"></a>[`public long Inno.Editor.Diagnostics.EditorConsoleSnapshot.revision { get; }`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleSnapshot.cs#L24) | Gets the monotonic revision of the captured Console state. | — | — |
| <a id="api-62973d147e728e83"></a>[`public sealed class Inno.Editor.Diagnostics.EditorConsoleSnapshot`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/EditorConsoleSnapshot.cs#L9) | Provides one immutable and internally consistent view of editor Console state. | — | — |

### `Inno.Editor.Diagnostics.IEditorConsole`

<!-- api: T:Inno.Editor.Diagnostics.IEditorConsole 23c2a0052f575253e66fb7bf0afad9368cdf69ba8710fab03216d40049818f88 -->
<!-- api: P:Inno.Editor.Diagnostics.IEditorConsole.capacity c9c7564c5d8deed7cb29d972db4af774b08f6205ad96251d17b10998aeae1311 -->
<!-- api: P:Inno.Editor.Diagnostics.IEditorConsole.clearOnPlay b2ebd59c240fd48d40bab855c66a78f584295e4fc6c7f056598982e30c0cbae6 -->
<!-- api: M:Inno.Editor.Diagnostics.IEditorConsole.Capture 4ed92703a60ed74eb3f69aa81391af98265023e0fddff00479f0a67ba218d774 -->
<!-- api: M:Inno.Editor.Diagnostics.IEditorConsole.Clear 38c0b8330f49e37fab550b2971d447d0b920fec2d710cc8d4b8e3ce25d4cdcd5 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-53c5582c46103910"></a>[`Inno.Editor.Diagnostics.EditorConsoleSnapshot Inno.Editor.Diagnostics.IEditorConsole.Capture()`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/IEditorConsole.cs#L25) | Captures an immutable snapshot containing both individual occurrences and global groups.<br>返回：A consistent snapshot that remains valid while new entries arrive. | — | — |
| <a id="api-712e2a2e3b8fe173"></a>[`void Inno.Editor.Diagnostics.IEditorConsole.Clear()`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/IEditorConsole.cs#L30) | Removes retained logs while keeping currently reported issues visible until their owners resolve them. | — | — |
| <a id="api-a8e40cde91935fd7"></a>[`int Inno.Editor.Diagnostics.IEditorConsole.capacity { get; set; }`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/IEditorConsole.cs#L11) | Gets or sets the maximum number of ordinary log occurrences retained in memory. | — | — |
| <a id="api-5902ed4ad8e07244"></a>[`bool Inno.Editor.Diagnostics.IEditorConsole.clearOnPlay { get; set; }`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/IEditorConsole.cs#L17) | Gets or sets whether ordinary Console logs are cleared when a new Play Mode request begins. Current diagnostics remain visible because they represent active compiler and subsystem state. | — | — |
| <a id="api-4a741b6472da6bd6"></a>[`public interface Inno.Editor.Diagnostics.IEditorConsole`](../../src/composition/editor/framework/Inno.Editor.Diagnostics/IEditorConsole.cs#L6) | Exposes read-only editor Console snapshots and explicit retention controls to editor features. | — | — |

## 项目依赖

- [Inno.Editor.PlayMode](Inno.Editor.PlayMode.md)：公开引用边界由实际签名核对。
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：公开引用边界由实际签名核对。
- [Inno.Core.Logging](../core/Inno.Core.Logging.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
