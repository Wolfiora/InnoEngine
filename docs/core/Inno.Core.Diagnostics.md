# Inno.Core.Diagnostics

[上一页：Input](Inno.Core.Input.md) · [Core 索引](README.md) · [下一页：Logging](Inno.Core.Logging.md)

`Inno.Core.Diagnostics` 管理 Compiler、Importer、Validator、Shader Processor 和 Build Pipeline 等生产者的“当前问题状态”。它独立于追加式 Logging：同一拥有者再次设置诊断会原子替换旧结果，成功后清除结果，后注册的工具仍能立即获得所有当前问题。

项目不执行具体编译、导入或验证，也不保存历史日志。引擎基础设施注入 `IDiagnosticReporter`；`Diagnostics.Set/Clear` 是已有 scope-bound 便捷入口。Editor Console、命令行工具和构建报告通过 sink 消费不可变 report。

## Owner-bound producer

```csharp
using Inno.Core.Diagnostics;

var hub = new DiagnosticHub();
using DiagnosticReporter reporter = hub.CreateReporter(new DiagnosticSource("audio.session.example", "Audio"));
reporter.Publish(new Diagnostic("audio.device.lost", "Output device is unavailable.", DiagnosticSeverity.Warning));
reporter.Resolve("audio.device.lost");
```

| API | 稳定语义 |
| --- | --- |
| `Diagnostic(code, message, severity, semanticId?, objectId?, location?)` | 中立问题；semantic ID、persistent object ID 与文件位置各有独立语义 |
| `DiagnosticHub.CreateReporter(source)` | 创建当前 producer registration，同 ID 的旧 registration 被撤销 |
| `IDiagnosticReporter.Publish(diagnostic)` | 以 code + semanticId + objectId 更新同一问题，message 不参与身份；内容完全相同时不重新通知 sink |
| `Resolve(code, semanticId?, objectId?)` | 条件恢复后撤销指定问题；已撤销的身份重复 Resolve 不重新通知 sink |
| `Replace(diagnostics)` | 冻结并替换整个问题集合，拒绝重复问题身份 |
| `DiagnosticReporter.Dispose()` | 释放该 registration 的报告；不能清除同 ID 的较新 producer |

Reporter 是生产者，`IDiagnosticSink` 是 presentation 消费者；两者不是同一角色。Audio、Rendering、Shader、Graph、Scripting 和 Scene reload 统一使用 `DiagnosticSeverity`。没有领域自己的 severity enum 或 Audio/Render diagnostic sink。`DiagnosticLogSink` 位于 Logging，将当前问题的变化投影到追加日志，不成为第二个诊断状态 owner。

## 核心语义

```text
Producer completes one validation pass
→ Build the complete current Diagnostic collection
→ Diagnostics.Set(group, diagnostics)
→ Resolve caller type + explicit group
→ Atomically replace the previous report
→ Replay the new current state to every sink
```

- `Diagnostic` 是尚未发布的一条不可变问题数据。
- `Diagnostics` 是业务代码使用的静态状态入口。
- `Set` 始终替换完整集合，不逐条追加。
- `Set` 空集合等价于 `Clear`。
- `Clear` 只清除调用类型拥有的指定 group，不会清除全局状态。
- 动态目标 overload 使用 `Guid` 将不同 Asset、Shader、Scene 或其他对象隔离。
- 后注册的 sink 会立即收到所有当前 report。
- 一个 sink 抛出异常不会影响生产者或其他 sink。

## 创建诊断

```csharp
using Inno.Core.Diagnostics;

Diagnostic error = Diagnostic.Error(
    "CS1002",
    "Expected ';'.",
    new DiagnosticLocation("Assets/Test.cs", line: 10, column: 24));

Diagnostic warning = Diagnostic.Warning(
    "INNO1001",
    "StableTypeId is implicit.");
```

`Diagnostic.Info`、`Diagnostic.Warning` 和 `Diagnostic.Error` 只创建数据，不修改全局状态。

## 固定职责

同一调用类型中的 group 名称用于标识一项持续职责：

```csharp
using System.Collections.Generic;
using Inno.Core.Diagnostics;

internal sealed class ScriptManager
{
    private const string C_COMPILATION_DIAGNOSTICS = "Compilation";

    internal void PublishCompilation(IReadOnlyList<Diagnostic> diagnostics)
    {
        Diagnostics.Set(C_COMPILATION_DIAGNOSTICS, diagnostics);
    }

    internal void ClearCompilation()
    {
        Diagnostics.Clear(C_COMPILATION_DIAGNOSTICS);
    }
}
```

隐藏 source ID 由调用 Assembly、逻辑调用类型和 group 组成。不同类型可以安全复用 `Compilation`、`Import` 等局部名称。方法重命名不会改变身份；发布与清除只要发生在相同调用类型并使用相同 group 即可。

## 动态目标

同一个职责处理多个对象时，使用稳定 target ID：

```csharp
using System;
using System.Collections.Generic;
using Inno.Core.Diagnostics;

static void PublishImport(
    Guid assetId,
    string displayPath,
    IReadOnlyList<Diagnostic> importDiagnostics
) {
    Diagnostics.Set(assetId, "Import", importDiagnostics, displayName: displayPath);
}

static void ClearImport(Guid assetId) => Diagnostics.Clear(assetId, "Import");
```

隐藏 source ID 额外包含 target ID，因此不同 Asset 的报告互不覆盖。`displayName` 只影响工具展示，不参与身份比较；Asset 重命名后可以用相同 persistent ID 和新路径替换原报告。

## Public API

### Diagnostic

| API | 说明 |
| --- | --- |
| `Info(code, message, location?)` | 创建 informational 诊断值。 |
| `Warning(code, message, location?)` | 创建 warning 诊断值。 |
| `Error(code, message, location?)` | 创建 error 诊断值。 |
| `severity` | `Info`、`Warning` 或 `Error`。 |
| `code` | Producer 定义的稳定编号。 |
| `message` | 面向用户的问题描述。 |
| `location` | 可选的 source path、one-based line 和 column。 |

### Diagnostics

| API | 说明 |
| --- | --- |
| `Set(group, diagnostic)` | 用单条诊断替换调用类型的指定 group。 |
| `Set(group, diagnostics)` | 用完整集合替换调用类型的指定 group；空集合会清除。 |
| `Set(targetId, group, diagnostic, displayName?)` | 设置一个动态目标的单条当前诊断。 |
| `Set(targetId, group, diagnostics, displayName?)` | 设置一个动态目标的完整当前集合。 |
| `Clear(group)` | 清除调用类型的指定 group。 |
| `Clear(targetId, group)` | 清除一个动态目标的指定 group。 |

### DiagnosticManager

| API | 说明 |
| --- | --- |
| `RegisterSink(sink)` | 注册消费者，并立即 replay 所有当前报告。 |
| `UnregisterSink(sink)` | 停止向消费者发送后续状态变化。 |

生产者不直接调用 Manager。Manager 的状态写入入口属于程序集内部实现，防止业务代码绕过 caller/group 身份规则。

### DiagnosticSource 与 DiagnosticReport

`DiagnosticSource` 是 Manager 生成的只读身份元数据，公开 `id` 和 `displayName` 供工具建立索引与展示。业务代码不能直接构造 source。

`DiagnosticReport` 包含 source、不可变的完整 diagnostics 集合和发布时间，由 Manager 创建并发送给 sink。

### IDiagnosticSink

```csharp
using System;
using Inno.Core.Diagnostics;

internal sealed class ConsoleDiagnosticSink : IDiagnosticSink
{
    public void Replace(DiagnosticReport report) => Console.WriteLine(report);
    public void Clear(DiagnosticSource source) => Console.WriteLine(source);
}
```

Sink 必须把 `Replace` 看作完整集合替换，而不是增量追加。

## 与 Logging 的边界

```text
Log
    = 已经发生过的事件、调试时间线、异常细节

Diagnostic
    = 当前仍然存在、具有明确 owner 和 clear 时机的问题
```

意外异常导致系统持续降级时，可以将完整异常写入 Log，同时发布简洁的当前 Diagnostic。系统恢复后只清除 Diagnostic，不删除历史 Log。

当前内建生产者采用同一规则：

| 子系统 | Diagnostic | Log |
| --- | --- | --- |
| Asset Loader | 当前 Import、Build、Catalog、missing reference 与 identity conflict 状态 | 实际发生的 Import/Build/Catalog 异常及完整堆栈 |
| AssetPipeline | 增量刷新与 recovery rescan 同时失败后的 Source Database 状态 | 每次 refresh/rescan 失败事件 |
| Scripting | 当前 compiler 与 reload 结果 | reload 事务抛出的完整异常 |
| Scene Workspace | 当前无法恢复的 scene setup、持续失败的 document synchronization | 首次进入失败状态的异常，以及被跳过的 missing scene 事件 |
| Editor Workspace | 当前 capture、restore、save 失败 | 状态首次变化时的完整异常 |
| Editor Extensions | 当前无法 Attach 的 Panel 集合 | Attach/Detach 的实际失败事件 |
| Editor Application | 当前无法持久化 `editor.ini` 的状态 | 首次保存失败的完整异常 |

Action、Menu、Drag/Drop、Undo/Redo、Rename/Delete/Open 和 Inspector 单次绘制失败继续只使用 Log；它们是一次调用的结果，不是拥有明确恢复事务的长期状态。

## 注意事项

- group 必须是当前调用类型内稳定、语义明确的职责名称。
- 多个问题必须在一次 `Set` 中提交，避免后一次调用覆盖前一次结果。
- 动态对象必须使用稳定 `Guid`，不能使用 runtime hash code 或可变化路径作为身份。
- `DiagnosticLocation` 表示被诊断的源码位置，不参与 report owner 身份。
- Diagnostics 不经过 EventHub；它需要保存当前快照并 replay 给晚订阅者。
- Console UI、过滤、折叠、复制、源码跳转和 Quick Fix 属于 Editor 层。

## 相邻模块

- [Inno.Core.Logging](Inno.Core.Logging.md)：追加式历史日志。
- [Inno.Editor.Panel.Logging](../editor/Inno.Editor.Panel.Logging.md)：合并展示 Log 和当前 Diagnostics。
- [Inno.Editor.Scripting](../editor/Inno.Editor.Scripting.md)：脚本编译和 reload 诊断生产者。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Core.Diagnostics.Diagnostic`

<!-- api: T:Inno.Core.Diagnostics.Diagnostic e9494c94bbcc7407311461e629febbc0b82b7d0934d84debc7a094316edafb90 -->
<!-- api: M:Inno.Core.Diagnostics.Diagnostic.#ctor(System.String,System.String,Inno.Core.Diagnostics.DiagnosticSeverity,System.String,System.Nullable{System.Guid},System.Nullable{Inno.Core.Diagnostics.DiagnosticLocation}) 472f5f3660d44531e98c40a9ea0a2664116e76f53467698b8ea7344005342471 -->
<!-- api: P:Inno.Core.Diagnostics.Diagnostic.semanticId 078b9f73187c392da3a57305241bee34a07d0f89d81ba41dcbc0fade3f9f1e25 -->
<!-- api: P:Inno.Core.Diagnostics.Diagnostic.objectId 177cd037f1b16333f8fbe08f095d117c7be15c6c37be22ca27e52775996cfc6f -->
<!-- api: M:Inno.Core.Diagnostics.Diagnostic.Info(System.String,System.String,System.Nullable{Inno.Core.Diagnostics.DiagnosticLocation}) 5d56b0fe363e032ed25e22bf6632b9702a22edce7127ebeeb588be4deae65ecf -->
<!-- api: M:Inno.Core.Diagnostics.Diagnostic.Warning(System.String,System.String,System.Nullable{Inno.Core.Diagnostics.DiagnosticLocation}) 2d24d7f5dc96beae9348fa4db9c4f0aafc94df27329ff89dc33ada201c700286 -->
<!-- api: M:Inno.Core.Diagnostics.Diagnostic.Error(System.String,System.String,System.Nullable{Inno.Core.Diagnostics.DiagnosticLocation}) 1995ee90e00faf1748569fd40f19232a4e254f5dd2886f9611b8a4a5ae1c1855 -->
<!-- api: P:Inno.Core.Diagnostics.Diagnostic.severity 987290e00a41c2ecc3676d3966b05de2f12aac3b020025b2103fccf2444ca260 -->
<!-- api: P:Inno.Core.Diagnostics.Diagnostic.code cb18b1efba78edc9c59c362ed164aa7e2b0bdd9c6911b23d69efd9aad5982279 -->
<!-- api: P:Inno.Core.Diagnostics.Diagnostic.message af4b746658f831fe5de0556f5894918823febd4b276d5bd888d1502bb3548785 -->
<!-- api: P:Inno.Core.Diagnostics.Diagnostic.location 2347c56de7ec08db8ee08a3554b6f793f35b215a5970bfa61ab4a24b9cc0b3c1 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3361cf58aec66e78"></a>[`public Inno.Core.Diagnostics.Diagnostic.Diagnostic(string code, string message, Inno.Core.Diagnostics.DiagnosticSeverity severity, string? semanticId = null, System.Guid? objectId = null, Inno.Core.Diagnostics.DiagnosticLocation? location = null)`](../../src/foundation/core/Inno.Core.Diagnostics/Diagnostic.cs#L37) | Creates a neutral diagnostic while keeping protocol, object and file identities distinct.<br>`code`：The stable issue code within its producer.<br>`message`：The actionable human-readable message.<br>`severity`：The common engine impact classification.<br>`semanticId`：An optional extension or protocol identifier, never interpreted as a file path.<br>`objectId`：An optional persistent object identity, never a runtime handle.<br>`location`：An optional actual source file location. | — | `System.ArgumentException`：The code is empty or an object identity is empty.<br>`System.ArgumentNullException`：The message is null. |
| <a id="api-6b181e8f0c002838"></a>[`public static Inno.Core.Diagnostics.Diagnostic Inno.Core.Diagnostics.Diagnostic.Error(string code, string message, Inno.Core.Diagnostics.DiagnosticLocation? location = null)`](../../src/foundation/core/Inno.Core.Diagnostics/Diagnostic.cs#L132) | Creates an error diagnostic value without publishing it.<br>返回：An immutable error diagnostic that can be supplied to Inno.Core.Diagnostics.Diagnostics.Set(System.String,Inno.Core.Diagnostics.Diagnostic).<br>`code`：The stable producer-defined code, or an empty string when no code is available.<br>`message`：The human-readable diagnostic message.<br>`location`：The related source location, or null when unavailable. | — | `System.ArgumentNullException`：Thrown when message is null. |
| <a id="api-1a6bc4ff22b1b8cb"></a>[`public static Inno.Core.Diagnostics.Diagnostic Inno.Core.Diagnostics.Diagnostic.Info(string code, string message, Inno.Core.Diagnostics.DiagnosticLocation? location = null)`](../../src/foundation/core/Inno.Core.Diagnostics/Diagnostic.cs#L82) | Creates an informational diagnostic value without publishing it.<br>返回：An immutable informational diagnostic that can be supplied to Inno.Core.Diagnostics.Diagnostics.Set(System.String,Inno.Core.Diagnostics.Diagnostic).<br>`code`：The stable producer-defined code, or an empty string when no code is available.<br>`message`：The human-readable diagnostic message.<br>`location`：The related source location, or null when unavailable. | — | `System.ArgumentNullException`：Thrown when message is null. |
| <a id="api-f73e9cddd00c9cc5"></a>[`public static Inno.Core.Diagnostics.Diagnostic Inno.Core.Diagnostics.Diagnostic.Warning(string code, string message, Inno.Core.Diagnostics.DiagnosticLocation? location = null)`](../../src/foundation/core/Inno.Core.Diagnostics/Diagnostic.cs#L107) | Creates a warning diagnostic value without publishing it.<br>返回：An immutable warning diagnostic that can be supplied to Inno.Core.Diagnostics.Diagnostics.Set(System.String,Inno.Core.Diagnostics.Diagnostic).<br>`code`：The stable producer-defined code, or an empty string when no code is available.<br>`message`：The human-readable diagnostic message.<br>`location`：The related source location, or null when unavailable. | — | `System.ArgumentNullException`：Thrown when message is null. |
| <a id="api-cc5f1d3f3b541fb0"></a>[`public string Inno.Core.Diagnostics.Diagnostic.code { get; }`](../../src/foundation/core/Inno.Core.Diagnostics/Diagnostic.cs#L160) | Gets the stable producer-defined code, or an empty string when unavailable. | — | — |
| <a id="api-0b52a53ac00e14c4"></a>[`public Inno.Core.Diagnostics.DiagnosticLocation? Inno.Core.Diagnostics.Diagnostic.location { get; }`](../../src/foundation/core/Inno.Core.Diagnostics/Diagnostic.cs#L170) | Gets the related source location, or null when unavailable. | — | — |
| <a id="api-b21f83baeb5a16cb"></a>[`public string Inno.Core.Diagnostics.Diagnostic.message { get; }`](../../src/foundation/core/Inno.Core.Diagnostics/Diagnostic.cs#L165) | Gets the human-readable diagnostic message. | — | — |
| <a id="api-fac126a2b0b7ccb9"></a>[`public System.Guid? Inno.Core.Diagnostics.Diagnostic.objectId { get; }`](../../src/foundation/core/Inno.Core.Diagnostics/Diagnostic.cs#L62) | Gets the related persistent object identity, or null for a non-object issue. | — | — |
| <a id="api-74432e56f69e9230"></a>[`public string? Inno.Core.Diagnostics.Diagnostic.semanticId { get; }`](../../src/foundation/core/Inno.Core.Diagnostics/Diagnostic.cs#L57) | Gets the related protocol identifier without treating it as a source file. | — | — |
| <a id="api-a3191662f98a1f14"></a>[`public Inno.Core.Diagnostics.DiagnosticSeverity Inno.Core.Diagnostics.Diagnostic.severity { get; }`](../../src/foundation/core/Inno.Core.Diagnostics/Diagnostic.cs#L155) | Gets the diagnostic severity. | — | — |
| <a id="api-fe8b5aba268b5f60"></a>[`public sealed class Inno.Core.Diagnostics.Diagnostic`](../../src/foundation/core/Inno.Core.Diagnostics/Diagnostic.cs#L8) | Represents one current issue reported by a compiler, importer, validator, or other diagnostic producer. | — | — |

### `Inno.Core.Diagnostics.DiagnosticHub`

<!-- api: T:Inno.Core.Diagnostics.DiagnosticHub 89b444a8871ffaa1850f527a1115359315f76dc608b0ca9af6b6759e2b55aefb -->
<!-- api: E:Inno.Core.Diagnostics.DiagnosticHub.sinkFailed c70f8666b5138c5c321ffdd8ba94f02a31bee138c553fa7c8429e74f33b91400 -->
<!-- api: M:Inno.Core.Diagnostics.DiagnosticHub.CreateReporter(Inno.Core.Diagnostics.DiagnosticSource) 9e909533ebb13931aaf754181f66fbbb472c88176508fe2a9f149f6ec150e778 -->
<!-- api: M:Inno.Core.Diagnostics.DiagnosticHub.EnterScope d16a84e4ef7318c6f1d4534bd0a3bfb632cbc03b646c05469b6d631d653035ba -->
<!-- api: M:Inno.Core.Diagnostics.DiagnosticHub.RegisterSink(Inno.Core.Diagnostics.IDiagnosticSink) 4bd7721d5cd96ef0aebf963e57fd34a48d1c71320dc47795759ef3756ebb6e81 -->
<!-- api: M:Inno.Core.Diagnostics.DiagnosticHub.UnregisterSink(Inno.Core.Diagnostics.IDiagnosticSink) b97eafb9360014a553c34c3b19da8f698528fd800708b50b6c4d71768f183759 -->
<!-- api: M:Inno.Core.Diagnostics.DiagnosticHub.Set(Inno.Core.Diagnostics.DiagnosticSource,System.Collections.Generic.IEnumerable{Inno.Core.Diagnostics.Diagnostic}) 3ac3a3cd4fd843d62ece01ee2e4b02e9d628dc064a50ab7a88aea0942e88d95f -->
<!-- api: M:Inno.Core.Diagnostics.DiagnosticHub.Clear(Inno.Core.Diagnostics.DiagnosticSource) f72dbb00697a201214ea4427d44d92b8aadc687b7538d6cd1c50b4ebfa78095b -->
<!-- api: M:Inno.Core.Diagnostics.DiagnosticHub.#ctor fac47ba36ad16ed4cf49075d16a5c442ce2c24cdaa275e182007ac8d599000c3 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-26d9207289c31e9f"></a>[`public System.Action<System.Exception>? Inno.Core.Diagnostics.DiagnosticHub.sinkFailed`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticHub.cs#L29) | Occurs when a diagnostic presentation sink fails and is quarantined. | — | — |
| <a id="api-9427d5b7a6323dfa"></a>[`public Inno.Core.Diagnostics.DiagnosticHub.DiagnosticHub()`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticHub.cs#L12) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-b48ffbdcc2d6509c"></a>[`public void Inno.Core.Diagnostics.DiagnosticHub.Clear(Inno.Core.Diagnostics.DiagnosticSource source)`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticHub.cs#L191) | Clears the active report published by one producer.<br>`source`：The stable producer identity to clear. | — | `System.ArgumentException`：Thrown when source is invalid. |
| <a id="api-b24f932face4b93d"></a>[`public Inno.Core.Diagnostics.DiagnosticReporter Inno.Core.Diagnostics.DiagnosticHub.CreateReporter(Inno.Core.Diagnostics.DiagnosticSource source)`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticHub.cs#L43) | Activates a producer at an owner safe point, revoking any previous registration with the same ID.<br>返回：A producer whose disposal clears only its own active registration.<br>`source`：The stable owner-qualified producer identity. | — | `System.ArgumentException`：The source is not initialized. |
| <a id="api-11c980fa4541b9f3"></a>[`public System.IDisposable Inno.Core.Diagnostics.DiagnosticHub.EnterScope()`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticHub.cs#L87) | Binds this hub to the current asynchronous execution context.<br>返回：A strict last-in-first-out scope owned by the caller. | — | — |
| <a id="api-2f5c1c15d7ad44f3"></a>[`public void Inno.Core.Diagnostics.DiagnosticHub.RegisterSink(Inno.Core.Diagnostics.IDiagnosticSink sink)`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticHub.cs#L104) | Registers a sink and synchronously replays every active report before registration completes.<br>`sink`：The sink that receives current and future reports. | — | `System.ArgumentNullException`：Thrown when sink is null.<br>`System.InvalidOperationException`：Thrown when the sink cannot accept the current diagnostic snapshot. |
| <a id="api-60c0b90514a285d8"></a>[`public void Inno.Core.Diagnostics.DiagnosticHub.Set(Inno.Core.Diagnostics.DiagnosticSource source, System.Collections.Generic.IEnumerable<Inno.Core.Diagnostics.Diagnostic> diagnostics)`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticHub.cs#L157) | Atomically replaces the complete diagnostic state published by one producer.<br>`source`：The stable producer identity whose previous report is replaced.<br>`diagnostics`：The complete current report, or an empty collection to clear it. | — | `System.ArgumentException`：Thrown when the source is invalid or the collection contains a null entry.<br>`System.ArgumentNullException`：Thrown when diagnostics is null. |
| <a id="api-486d4234ffc60366"></a>[`public void Inno.Core.Diagnostics.DiagnosticHub.UnregisterSink(Inno.Core.Diagnostics.IDiagnosticSink sink)`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticHub.cs#L135) | Unregisters a sink so it receives no future diagnostic changes.<br>`sink`：The sink to remove when registered. | — | `System.ArgumentNullException`：Thrown when sink is null. |
| <a id="api-8edfa6987d946f44"></a>[`public sealed class Inno.Core.Diagnostics.DiagnosticHub`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticHub.cs#L12) | Owns one host's complete diagnostic state and its presentation subscriptions. | — | — |

### `Inno.Core.Diagnostics.DiagnosticLocation`

<!-- api: T:Inno.Core.Diagnostics.DiagnosticLocation 2e30ab56345d5991958df0c396fb17a62b92743cd01cc1b1fcb5619a1e86e3ea -->
<!-- api: M:Inno.Core.Diagnostics.DiagnosticLocation.#ctor(System.String,System.Int32,System.Int32) 51d9b2e3589c14fedbc147c60f32b7ff3c17da784d505a1bf4cd6d007416e4d9 -->
<!-- api: P:Inno.Core.Diagnostics.DiagnosticLocation.sourcePath 1fb67c1dc801fbbaa1f46828dffa45a0a4bc941fc56cf21ff8c57f05d804cf12 -->
<!-- api: P:Inno.Core.Diagnostics.DiagnosticLocation.line 426f49a6a89718191c915d7f8c90a1573b9abd5d9ba57a516a02d13f9a8f58b9 -->
<!-- api: P:Inno.Core.Diagnostics.DiagnosticLocation.column 09e46406d1fcdedab362390a378ea6349bfcbcb1fd22311de1b6eb71a04cbf96 -->
<!-- api: M:Inno.Core.Diagnostics.DiagnosticLocation.Equals(Inno.Core.Diagnostics.DiagnosticLocation) 8369dffa8b5cc639eff8a21c677bcea9fe4b257a3c90f32d2cb960ef8ce320d6 -->
<!-- api: M:Inno.Core.Diagnostics.DiagnosticLocation.Equals(System.Object) 9ae10fb9308589c626a28bae97455c16a7798b420ca6b7630f59f59b22fc3151 -->
<!-- api: M:Inno.Core.Diagnostics.DiagnosticLocation.GetHashCode 53e5c87c4d7c424686752e32054dd28547fe5d0cf78bf4b859b2bf83cc16f100 -->
<!-- api: M:Inno.Core.Diagnostics.DiagnosticLocation.op_Equality(Inno.Core.Diagnostics.DiagnosticLocation,Inno.Core.Diagnostics.DiagnosticLocation) 98c0696e17c2ee50a9c06f3494a1f283762c17092d7051c2256a925b6ba2874d -->
<!-- api: M:Inno.Core.Diagnostics.DiagnosticLocation.op_Inequality(Inno.Core.Diagnostics.DiagnosticLocation,Inno.Core.Diagnostics.DiagnosticLocation) e5570fb7fb610568603f59280d2c7c64cafa948b9c1d744667640679de9dd162 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f22b0d3ff5b1440a"></a>[`public Inno.Core.Diagnostics.DiagnosticLocation.DiagnosticLocation(string sourcePath, int line = 0, int column = 0)`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticLocation.cs#L28) | Creates a source location.<br>`sourcePath`：The source path associated with the diagnostic.<br>`line`：The one-based line number, or zero when it is unavailable.<br>`column`：The one-based column number, or zero when it is unavailable. | — | `System.ArgumentException`：Thrown when sourcePath is empty or contains only whitespace.<br>`System.ArgumentOutOfRangeException`：Thrown when line or column is negative. |
| <a id="api-5740a1db82a601ed"></a>[`public bool Inno.Core.Diagnostics.DiagnosticLocation.Equals(Inno.Core.Diagnostics.DiagnosticLocation other)`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticLocation.cs#L68) | Determines whether this instance and the supplied value represent the same logical state.<br>返回：true when both values represent the same logical state; otherwise, false.<br>`other`：The value to compare with this instance. | — | — |
| <a id="api-a6e23a90b560a62b"></a>[`public override bool Inno.Core.Diagnostics.DiagnosticLocation.Equals(object? obj)`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticLocation.cs#L82) | Determines whether this instance and the supplied value represent the same logical state.<br>返回：true when both values represent the same logical state; otherwise, false.<br>`obj`：The object to compare with this instance. | — | — |
| <a id="api-68ec11e2111e412e"></a>[`public override int Inno.Core.Diagnostics.DiagnosticLocation.GetHashCode()`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticLocation.cs#L90) | Computes a hash code from the fields that participate in logical equality.<br>返回：A hash code consistent with the implemented equality contract. | — | — |
| <a id="api-cb408902f7049ab4"></a>[`public static bool Inno.Core.Diagnostics.DiagnosticLocation.operator ==(Inno.Core.Diagnostics.DiagnosticLocation left, Inno.Core.Diagnostics.DiagnosticLocation right)`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticLocation.cs#L104) | Determines whether two locations are equal.<br>返回：true when both locations identify the same source position.<br>`left`：The left location.<br>`right`：The right location. | — | — |
| <a id="api-9a67e44078e8c953"></a>[`public static bool Inno.Core.Diagnostics.DiagnosticLocation.operator !=(Inno.Core.Diagnostics.DiagnosticLocation left, Inno.Core.Diagnostics.DiagnosticLocation right)`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticLocation.cs#L121) | Determines whether two locations are different.<br>返回：true when the locations identify different source positions.<br>`left`：The left location.<br>`right`：The right location. | — | — |
| <a id="api-b2cba54e309e3d63"></a>[`public int Inno.Core.Diagnostics.DiagnosticLocation.column { get; }`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticLocation.cs#L57) | Gets the one-based source column, or zero when it is unavailable. | — | — |
| <a id="api-bca3780f892aa73e"></a>[`public int Inno.Core.Diagnostics.DiagnosticLocation.line { get; }`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticLocation.cs#L52) | Gets the one-based source line, or zero when it is unavailable. | — | — |
| <a id="api-b6f86ef480b5a196"></a>[`public string Inno.Core.Diagnostics.DiagnosticLocation.sourcePath { get; }`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticLocation.cs#L47) | Gets the source path associated with the diagnostic. | — | — |
| <a id="api-82ba017f2a168173"></a>[`public readonly struct Inno.Core.Diagnostics.DiagnosticLocation : System.IEquatable<Inno.Core.Diagnostics.DiagnosticLocation>`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticLocation.cs#L8) | Identifies an optional source location associated with a diagnostic. | — | — |

### `Inno.Core.Diagnostics.DiagnosticReport`

<!-- api: T:Inno.Core.Diagnostics.DiagnosticReport 2c6e1555487eb567bae9f26c46e3582cc8019e40a104034524fe0cc3ec0d30a0 -->
<!-- api: P:Inno.Core.Diagnostics.DiagnosticReport.source b637752f84eeb33d038e0c403ba84331e75996f1c8e2ea5500f1b301e04e7889 -->
<!-- api: P:Inno.Core.Diagnostics.DiagnosticReport.diagnostics 601ef13b3ad29bd7290f3c059967515f076a1cd4beb8feaaa08dfddbbd8349fd -->
<!-- api: P:Inno.Core.Diagnostics.DiagnosticReport.publishedAt 6fc86856d0f3d1f08aa34169598c9679685ab8ea86baad79ad7d67f10c64b46f -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b2115dd1c143fcfc"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Core.Diagnostics.Diagnostic> Inno.Core.Diagnostics.DiagnosticReport.diagnostics { get; }`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticReport.cs#L30) | Gets the immutable diagnostics currently reported by the producer. | — | — |
| <a id="api-a76bb27e54ecf2ee"></a>[`public System.DateTime Inno.Core.Diagnostics.DiagnosticReport.publishedAt { get; }`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticReport.cs#L35) | Gets the time at which the report was published. | — | — |
| <a id="api-d7e42fd9bacf3ee5"></a>[`public Inno.Core.Diagnostics.DiagnosticSource Inno.Core.Diagnostics.DiagnosticReport.source { get; }`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticReport.cs#L25) | Gets the producer that owns this report. | — | — |
| <a id="api-ff8ecd90f437a012"></a>[`public sealed class Inno.Core.Diagnostics.DiagnosticReport`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticReport.cs#L10) | Represents the complete current diagnostic state published by one source. | — | — |

### `Inno.Core.Diagnostics.DiagnosticReporter`

<!-- api: T:Inno.Core.Diagnostics.DiagnosticReporter 9a12bd946d3eac6c1cd0e797728b7db38f0bec973ca477d90e806e4f347ce20d -->
<!-- api: M:Inno.Core.Diagnostics.DiagnosticReporter.Publish(Inno.Core.Diagnostics.Diagnostic) ee67c505bf92524c085a7006cddfe9a6aa4826c19330a73d88613043c047c734 -->
<!-- api: M:Inno.Core.Diagnostics.DiagnosticReporter.Resolve(System.String,System.String,System.Nullable{System.Guid}) 699854c0959be3835e31dd3b988828a7488a4f80a0188f9405db4d60cfd9c8c8 -->
<!-- api: M:Inno.Core.Diagnostics.DiagnosticReporter.Replace(System.Collections.Generic.IEnumerable{Inno.Core.Diagnostics.Diagnostic}) c16f3af1e956266371eaa437e17b7dddd0a74e015a23ac7804e14e566552f71e -->
<!-- api: M:Inno.Core.Diagnostics.DiagnosticReporter.Dispose 6b8f49f1aee519aa89f7872c8eb8e59d70ef6c565fd69e5766ab908c2f241f67 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-186a983c07450272"></a>[`public void Inno.Core.Diagnostics.DiagnosticReporter.Dispose()`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticReporter.cs#L119) | Revokes this registration without clearing a newer registration for the same producer. | — | — |
| <a id="api-577f72cef0f18868"></a>[`public void Inno.Core.Diagnostics.DiagnosticReporter.Publish(Inno.Core.Diagnostics.Diagnostic diagnostic)`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticReporter.cs#L37) | Replaces the issue with the same code and target, without using its message as identity.<br>`diagnostic`：The immutable current issue. | — | `System.ObjectDisposedException`：This producer has retired or been replaced. |
| <a id="api-a0dc5a8c8e887bcd"></a>[`public void Inno.Core.Diagnostics.DiagnosticReporter.Replace(System.Collections.Generic.IEnumerable<Inno.Core.Diagnostics.Diagnostic> diagnostics)`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticReporter.cs#L96) | Atomically replaces this producer's complete current issue set.<br>`diagnostics`：The full immutable issue set; empty clears the report. | — | `System.ArgumentException`：The supplied issue set contains a duplicate identity.<br>`System.ObjectDisposedException`：This producer has retired or been replaced. |
| <a id="api-21cb3130925bac6d"></a>[`public void Inno.Core.Diagnostics.DiagnosticReporter.Resolve(string code, string? semanticId = null, System.Guid? objectId = null)`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticReporter.cs#L70) | Resolves an issue after the represented condition no longer exists.<br>`code`：The stable issue code.<br>`semanticId`：The optional protocol identifier used at publication.<br>`objectId`：The optional persistent object identity used at publication. | — | `System.ObjectDisposedException`：This producer has retired or been replaced. |
| <a id="api-c48e29a31ba5e191"></a>[`public sealed class Inno.Core.Diagnostics.DiagnosticReporter : Inno.Core.Diagnostics.IDiagnosticReporter, System.IDisposable`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticReporter.cs#L10) | Owns one revocable diagnostic producer registration and releases its current report on retirement. | — | — |

### `Inno.Core.Diagnostics.Diagnostics`

<!-- api: T:Inno.Core.Diagnostics.Diagnostics a966b4d9bceb869d431a9c6d0d1eecf5cf800e3da065eab5786fe5a69fe3bd1b -->
<!-- api: M:Inno.Core.Diagnostics.Diagnostics.Set(System.String,Inno.Core.Diagnostics.Diagnostic) 8388ea64dff0845747c88887ee835478b287d42b28697d7c8d44bcd868499cd5 -->
<!-- api: M:Inno.Core.Diagnostics.Diagnostics.Set(System.String,System.Collections.Generic.IEnumerable{Inno.Core.Diagnostics.Diagnostic}) de36b7dee17480ec43887c8a7a3b1a1e3f8b75f8ab0b554bf81df94739c9169a -->
<!-- api: M:Inno.Core.Diagnostics.Diagnostics.Set(System.Guid,System.String,Inno.Core.Diagnostics.Diagnostic,System.String) 5958b010b0f2aac42a9c269598d0b737dc9cd94de0f02b69c1b0b0f8efdb3e53 -->
<!-- api: M:Inno.Core.Diagnostics.Diagnostics.Set(System.Guid,System.String,System.Collections.Generic.IEnumerable{Inno.Core.Diagnostics.Diagnostic},System.String) bdd9481b24e77e2d2f653cc1b744d716257f8b8cac742a7f92ed2da0f98663a1 -->
<!-- api: M:Inno.Core.Diagnostics.Diagnostics.Clear(System.String) f9da73d19c642ce6a26cf5de15e6d83aa1caf43afa2cc8fcde65439a76966f27 -->
<!-- api: M:Inno.Core.Diagnostics.Diagnostics.Clear(System.Guid,System.String) c603f43ff34340a9726dca9f048c84f0d744eaa596e649253193e5fbc3142eb3 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1deab0ba5375d17b"></a>[`public static void Inno.Core.Diagnostics.Diagnostics.Clear(System.Guid targetId, string group)`](../../src/foundation/core/Inno.Core.Diagnostics/Diagnostics.cs#L153) | Clears one targeted diagnostic group owned by the calling type.<br>`targetId`：The stable identity of the asset, scene element, or other dynamic target.<br>`group`：The non-empty responsibility name scoped to the calling type and target. | — | `System.ArgumentException`：Thrown when targetId is empty or group is empty. |
| <a id="api-1804cecbaec81760"></a>[`public static void Inno.Core.Diagnostics.Diagnostics.Clear(string group)`](../../src/foundation/core/Inno.Core.Diagnostics/Diagnostics.cs#L138) | Clears one diagnostic group owned by the calling type.<br>`group`：The non-empty responsibility name scoped to the calling type. | — | `System.ArgumentException`：Thrown when group is empty. |
| <a id="api-db8b6817ac7873dd"></a>[`public static void Inno.Core.Diagnostics.Diagnostics.Set(System.Guid targetId, string group, Inno.Core.Diagnostics.Diagnostic diagnostic, string? displayName = null)`](../../src/foundation/core/Inno.Core.Diagnostics/Diagnostics.cs#L84) | Atomically sets the complete current state of one targeted diagnostic group owned by the calling type.<br>`targetId`：The stable identity of the asset, scene element, or other dynamic target.<br>`group`：The non-empty responsibility name scoped to the calling type and target.<br>`diagnostic`：The single current diagnostic. It replaces every previous entry in the targeted group.<br>`displayName`：The optional target name presented by diagnostic consumers. | — | `System.ArgumentException`：Thrown when targetId is empty or group is empty.<br>`System.ArgumentNullException`：Thrown when diagnostic is null. |
| <a id="api-1216948d96dabf23"></a>[`public static void Inno.Core.Diagnostics.Diagnostics.Set(System.Guid targetId, string group, System.Collections.Generic.IEnumerable<Inno.Core.Diagnostics.Diagnostic> diagnostics, string? displayName = null)`](../../src/foundation/core/Inno.Core.Diagnostics/Diagnostics.cs#L116) | Atomically sets the complete current state of one targeted diagnostic group owned by the calling type.<br>`targetId`：The stable identity of the asset, scene element, or other dynamic target.<br>`group`：The non-empty responsibility name scoped to the calling type and target.<br>`diagnostics`：The complete current diagnostic collection. An empty collection clears the targeted group.<br>`displayName`：The optional target name presented by diagnostic consumers. | — | `System.ArgumentException`：Thrown when targetId is empty, group is empty, or the collection contains a null entry.<br>`System.ArgumentNullException`：Thrown when diagnostics is null. |
| <a id="api-f560110a8e208609"></a>[`public static void Inno.Core.Diagnostics.Diagnostics.Set(string group, Inno.Core.Diagnostics.Diagnostic diagnostic)`](../../src/foundation/core/Inno.Core.Diagnostics/Diagnostics.cs#L28) | Atomically sets the complete current state of one diagnostic group owned by the calling type.<br>`group`：The non-empty responsibility name scoped to the calling type.<br>`diagnostic`：The single current diagnostic. It replaces every previous entry in the group. | — | `System.ArgumentException`：Thrown when group is empty.<br>`System.ArgumentNullException`：Thrown when diagnostic is null. |
| <a id="api-a0cde78cb8904d2e"></a>[`public static void Inno.Core.Diagnostics.Diagnostics.Set(string group, System.Collections.Generic.IEnumerable<Inno.Core.Diagnostics.Diagnostic> diagnostics)`](../../src/foundation/core/Inno.Core.Diagnostics/Diagnostics.cs#L52) | Atomically sets the complete current state of one diagnostic group owned by the calling type.<br>`group`：The non-empty responsibility name scoped to the calling type.<br>`diagnostics`：The complete current diagnostic collection. An empty collection clears the group. | — | `System.ArgumentException`：Thrown when group is empty or the collection contains a null entry.<br>`System.ArgumentNullException`：Thrown when diagnostics is null. |
| <a id="api-5b7fdb99c353e45d"></a>[`public static class Inno.Core.Diagnostics.Diagnostics`](../../src/foundation/core/Inno.Core.Diagnostics/Diagnostics.cs#L10) | Provides concise state-oriented diagnostic publication for the calling type. | — | — |

### `Inno.Core.Diagnostics.DiagnosticSeverity`

<!-- api: T:Inno.Core.Diagnostics.DiagnosticSeverity 48fbc026d3f5b5ab349a8ab76ac4eff19cb629a39028ee96684d7aad9cd7b1fa -->
<!-- api: F:Inno.Core.Diagnostics.DiagnosticSeverity.Info e3faad621cc64cd10fd6fd12ab135c80dd78f482c31bb606b8c9797fb8dcbce2 -->
<!-- api: F:Inno.Core.Diagnostics.DiagnosticSeverity.Warning f1576435bc1cea00723646b666d6b46c003b843f44037b502860ad25dddb61c7 -->
<!-- api: F:Inno.Core.Diagnostics.DiagnosticSeverity.Error 5ab84645117bb6f77373d31c86fcc8be2cd794be103e135a1e35078b6b0b995e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-82730bc78aabb06c"></a>[`Inno.Core.Diagnostics.DiagnosticSeverity.Error = 2`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticSeverity.cs#L21) | Describes an issue that prevents the associated operation from succeeding. | — | — |
| <a id="api-8264b74b3108821d"></a>[`Inno.Core.Diagnostics.DiagnosticSeverity.Info = 0`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticSeverity.cs#L11) | Describes useful information that does not indicate a problem. | — | — |
| <a id="api-43c446a3bdcefc7f"></a>[`Inno.Core.Diagnostics.DiagnosticSeverity.Warning = 1`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticSeverity.cs#L16) | Describes a recoverable issue that should be reviewed. | — | — |
| <a id="api-e7b9d19fb883c024"></a>[`public enum Inno.Core.Diagnostics.DiagnosticSeverity : int`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticSeverity.cs#L6) | Defines the presentation severity of a diagnostic. | — | — |

### `Inno.Core.Diagnostics.DiagnosticSource`

<!-- api: T:Inno.Core.Diagnostics.DiagnosticSource 097a82321ee73d5da7277aaa71452db37400fdbc862cee803d1b39fd758c693b -->
<!-- api: M:Inno.Core.Diagnostics.DiagnosticSource.#ctor(System.String,System.String) d6d4cb285ff15baab1b3c5eea391a1e572d30ff328edd07a1b45540624212a44 -->
<!-- api: P:Inno.Core.Diagnostics.DiagnosticSource.id 3d525148afa45f4f31c0e08f73ca7096266637369012118ec5697bf171ebad3f -->
<!-- api: P:Inno.Core.Diagnostics.DiagnosticSource.displayName fadce82601e1bc2466e1abeb3a7b76accaf35bfb227e977627ec6e8cc8b761af -->
<!-- api: M:Inno.Core.Diagnostics.DiagnosticSource.Equals(Inno.Core.Diagnostics.DiagnosticSource) f805d0a2189d71d0a219e51562bfffb48d9fee54665f541285ddd0cb8434dbe9 -->
<!-- api: M:Inno.Core.Diagnostics.DiagnosticSource.Equals(System.Object) b6802c2b14114a7833241530ff154b52c60f77e970f7aed2eafacb87f2d8e2b1 -->
<!-- api: M:Inno.Core.Diagnostics.DiagnosticSource.GetHashCode 3f254891f156fddb2f16440f7acea56ff6b286139863f7d1b7d796ab3b79a5ec -->
<!-- api: M:Inno.Core.Diagnostics.DiagnosticSource.ToString 84536ac690fc07d7215b1065b33493f00b23813a0f7e38dd618e55856a36a0c9 -->
<!-- api: M:Inno.Core.Diagnostics.DiagnosticSource.op_Equality(Inno.Core.Diagnostics.DiagnosticSource,Inno.Core.Diagnostics.DiagnosticSource) 8f571aab8d439e3039afc3429955d890b5a5aacdd946770187cc6902607e2df6 -->
<!-- api: M:Inno.Core.Diagnostics.DiagnosticSource.op_Inequality(Inno.Core.Diagnostics.DiagnosticSource,Inno.Core.Diagnostics.DiagnosticSource) c9bd285f0aff1df382a1fd4adcf3560471561308c893ddcedb4e14dd908afa86 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-590436e6c5288b21"></a>[`public Inno.Core.Diagnostics.DiagnosticSource.DiagnosticSource(string id, string displayName)`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticSource.cs#L22) | Creates an immutable identity for one independently replaceable diagnostic producer.<br>`id`：The globally stable machine-readable producer identifier.<br>`displayName`：The non-empty producer name presented by diagnostic consumers. | — | `System.ArgumentException`：Thrown when either value is empty. |
| <a id="api-22d0dfb5a324697e"></a>[`public bool Inno.Core.Diagnostics.DiagnosticSource.Equals(Inno.Core.Diagnostics.DiagnosticSource other)`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticSource.cs#L53) | Determines whether this instance and the supplied value represent the same logical state.<br>返回：true when both values represent the same logical state; otherwise, false.<br>`other`：The value to compare with this instance. | — | — |
| <a id="api-1978b810cf10c4e8"></a>[`public override bool Inno.Core.Diagnostics.DiagnosticSource.Equals(object? obj)`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticSource.cs#L64) | Determines whether this instance and the supplied value represent the same logical state.<br>返回：true when both values represent the same logical state; otherwise, false.<br>`obj`：The object to compare with this instance. | — | — |
| <a id="api-c7ecffea261428b2"></a>[`public override int Inno.Core.Diagnostics.DiagnosticSource.GetHashCode()`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticSource.cs#L72) | Computes a hash code from the fields that participate in logical equality.<br>返回：A hash code consistent with the implemented equality contract. | — | — |
| <a id="api-907aeee14c837af8"></a>[`public static bool Inno.Core.Diagnostics.DiagnosticSource.operator ==(Inno.Core.Diagnostics.DiagnosticSource left, Inno.Core.Diagnostics.DiagnosticSource right)`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticSource.cs#L94) | Determines whether two sources have the same stable identifier.<br>返回：true when both source identifiers are equal.<br>`left`：The left source.<br>`right`：The right source. | — | — |
| <a id="api-b174e347192838c2"></a>[`public static bool Inno.Core.Diagnostics.DiagnosticSource.operator !=(Inno.Core.Diagnostics.DiagnosticSource left, Inno.Core.Diagnostics.DiagnosticSource right)`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticSource.cs#L111) | Determines whether two sources have different stable identifiers.<br>返回：true when the source identifiers differ.<br>`left`：The left source.<br>`right`：The right source. | — | — |
| <a id="api-a3fc9fc805d9c103"></a>[`public override string Inno.Core.Diagnostics.DiagnosticSource.ToString()`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticSource.cs#L80) | Formats this value as a human-readable representation.<br>返回：The human-readable representation of this value. | — | — |
| <a id="api-a7c9d9321ed9b2b4"></a>[`public string Inno.Core.Diagnostics.DiagnosticSource.displayName { get; }`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticSource.cs#L42) | Gets the user-facing producer name. | — | — |
| <a id="api-020c1b78691e9615"></a>[`public string Inno.Core.Diagnostics.DiagnosticSource.id { get; }`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticSource.cs#L37) | Gets the stable machine-readable producer identifier. | — | — |
| <a id="api-3e6985261dfe463c"></a>[`public readonly struct Inno.Core.Diagnostics.DiagnosticSource : System.IEquatable<Inno.Core.Diagnostics.DiagnosticSource>`](../../src/foundation/core/Inno.Core.Diagnostics/DiagnosticSource.cs#L8) | Identifies one independent producer whose diagnostics replace its previous publication. | — | — |

### `Inno.Core.Diagnostics.IDiagnosticReporter`

<!-- api: T:Inno.Core.Diagnostics.IDiagnosticReporter adf5ea7d23d1b2a7e9b7353918668b8181c879e0b6ad07446e8ff4bc33ff2d4d -->
<!-- api: M:Inno.Core.Diagnostics.IDiagnosticReporter.Publish(Inno.Core.Diagnostics.Diagnostic) 6bb71c0af6c19aef669812cbaebb52bec725a7047f510bde5df800e15d6ab9ec -->
<!-- api: M:Inno.Core.Diagnostics.IDiagnosticReporter.Resolve(System.String,System.String,System.Nullable{System.Guid}) c11548bfd6533517b42c648a3f89e6ecc5ecbf639f7d47ec39ee2a04778e4fe5 -->
<!-- api: M:Inno.Core.Diagnostics.IDiagnosticReporter.Replace(System.Collections.Generic.IEnumerable{Inno.Core.Diagnostics.Diagnostic}) 8e7a25ab27fae0492ac9a3e7441662c3466448c994ee2fa158a44742f7859405 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-ea79c51d658aab42"></a>[`void Inno.Core.Diagnostics.IDiagnosticReporter.Publish(Inno.Core.Diagnostics.Diagnostic diagnostic)`](../../src/foundation/core/Inno.Core.Diagnostics/IDiagnosticReporter.cs#L17) | Replaces the issue with the same code and target, without using its message as identity.<br>`diagnostic`：The immutable current issue. | — | — |
| <a id="api-33e56dd36b009140"></a>[`void Inno.Core.Diagnostics.IDiagnosticReporter.Replace(System.Collections.Generic.IEnumerable<Inno.Core.Diagnostics.Diagnostic> diagnostics)`](../../src/foundation/core/Inno.Core.Diagnostics/IDiagnosticReporter.cs#L43) | Atomically replaces this producer's complete current issue set.<br>`diagnostics`：The full immutable issue set; empty clears the report. | — | — |
| <a id="api-888fb4a5250ed9bf"></a>[`void Inno.Core.Diagnostics.IDiagnosticReporter.Resolve(string code, string? semanticId = null, System.Guid? objectId = null)`](../../src/foundation/core/Inno.Core.Diagnostics/IDiagnosticReporter.cs#L31) | Resolves an issue after the represented condition no longer exists.<br>`code`：The stable issue code.<br>`semanticId`：The optional protocol identifier used at publication.<br>`objectId`：The optional persistent object identity used at publication. | — | — |
| <a id="api-ffa0028db2d92064"></a>[`public interface Inno.Core.Diagnostics.IDiagnosticReporter`](../../src/foundation/core/Inno.Core.Diagnostics/IDiagnosticReporter.cs#L9) | Publishes current issues through an owner-bound producer, independently of presentation sinks. | — | — |

### `Inno.Core.Diagnostics.IDiagnosticSink`

<!-- api: T:Inno.Core.Diagnostics.IDiagnosticSink 6dc0fa90a9d47a7d75089dd908434abf878578b810b12e6ec755c51b01e75732 -->
<!-- api: M:Inno.Core.Diagnostics.IDiagnosticSink.Replace(Inno.Core.Diagnostics.DiagnosticReport) 37e59beb86c1ec513ea82e188e20bf1a3f07f32576c58869e26831f74386cc35 -->
<!-- api: M:Inno.Core.Diagnostics.IDiagnosticSink.Clear(Inno.Core.Diagnostics.DiagnosticSource) a613023fc00c2e9e09ef7dcbb496fd239fcd4d6a7c5ee013b7010b893654565c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-39173f013ea7c138"></a>[`void Inno.Core.Diagnostics.IDiagnosticSink.Clear(Inno.Core.Diagnostics.DiagnosticSource source)`](../../src/foundation/core/Inno.Core.Diagnostics/IDiagnosticSink.cs#L22) | Removes the current report for one diagnostic source.<br>`source`：The source whose current report was cleared. | — | — |
| <a id="api-365d768efbed32b5"></a>[`void Inno.Core.Diagnostics.IDiagnosticSink.Replace(Inno.Core.Diagnostics.DiagnosticReport report)`](../../src/foundation/core/Inno.Core.Diagnostics/IDiagnosticSink.cs#L14) | Replaces the current report for one diagnostic source.<br>`report`：The complete current report to store or present. | — | — |
| <a id="api-0a7dcc6746c7a698"></a>[`public interface Inno.Core.Diagnostics.IDiagnosticSink`](../../src/foundation/core/Inno.Core.Diagnostics/IDiagnosticSink.cs#L6) | Receives complete diagnostic-state changes from a Inno.Core.Diagnostics.DiagnosticHub. | — | — |

## 项目依赖

- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Execution](Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
