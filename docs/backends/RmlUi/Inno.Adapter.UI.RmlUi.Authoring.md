# Inno.Adapter.UI.RmlUi.Authoring

[分类索引](README.md) · [Wiki 首页](../../README.md) · [UI 资产契约](../../ui/Inno.UI.Assets.md) · [RmlUi 后端](Inno.Adapter.UI.RmlUi.md)

## 职责与依赖

本项目拥有 RML 创作语言的解析、依赖分析和 `.rml` 导入约定。
依赖 RmlUi 的显式语言/实现身份及中立 UI 资产契约；共同 UI 服务不解析 RML 语法。
生成类型目录依赖 Extensibility.Catalogs 叶契约。此项目属于创作闭包，不进入 Player。

## 全部公开 API 与派生边界

| 类型/成员 | 行为 |
| --- | --- |
| `RmlUiDocumentFrontend` | 无运行会话状态的 RML 前端，实现 `IUiDocumentFrontend`。 |
| `languageId` | 返回 RmlUiIdentifiers 中的显式 RML 语言身份。 |
| `Analyze(UiDocumentSourceFile)` | 展开受控 stylesheet 依赖，验证 root/font 声明，输出中立分析结果与结构化诊断；空输入不作为成功文档。 |
| `RmlUiDocumentImporter` | 通过 `AssetImporter("inno.ui.rml-document")` 被当前类型目录发现。 |
| `supportedExtensions` | 接受 `.rml`，把扩展名映射到显式语言和实现身份。 |
| protected `ImportAsync(context, output, cancellationToken)` | 调用共同 UiDocumentImportPipeline，提交资产、依赖和 artifact；遵守取消和 import owner。 |

两个实现均为 sealed，没有供外部派生覆盖的新增扩展点。
null source 是参数异常；缺失样式、引用循环和无效语法通过分析诊断返回，不发布不完整文档。

## 工作流

先由 Engine composition 将本程序集纳入创作模块目录，再由 UI frontend registry 构建候选快照。
导入器取得完整 Asset owner/context，所有资源依赖继续进入统一 Asset Pipeline。

```csharp
using Inno.Adapter.UI.RmlUi.Authoring;
using Inno.UI.Assets;

static UiDocumentAnalysis AnalyzeRml(UiDocumentSourceFile source)
    => new RmlUiDocumentFrontend().Analyze(source);
```

`UiDocumentSourceFile` 的受控依赖读取边界由调用方提供，前端不会为其他 Source 建立独立数据库。
生成结果属于本次 import；Registry 在 generation 安全点切换，旧实例和借用 source 不进入持久数据。
验证入口为 `Inno.UI.Tests` 的文档、依赖、语言选择和失败保全测试。

## 源码归属

当前唯一源码 owner：`backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi.Authoring/Inno.Adapter.UI.RmlUi.Authoring.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Adapter.UI.RmlUi.Authoring.RmlUiDocumentFrontend`

<!-- api: T:Inno.Adapter.UI.RmlUi.Authoring.RmlUiDocumentFrontend 1029b539cd61ca6c51a5dde1cca657d6b17535294f5474a52455a27032e637c4 -->
<!-- api: P:Inno.Adapter.UI.RmlUi.Authoring.RmlUiDocumentFrontend.languageId fa6f0ad9b56e115afa5083cd12902356e997089d5c7c3bb2505a4edd7aa59c9d -->
<!-- api: M:Inno.Adapter.UI.RmlUi.Authoring.RmlUiDocumentFrontend.Analyze(Inno.UI.Assets.UiDocumentSourceFile) 1f674408428624ea3de46e3298d8e1207c486e2ed755e37f0f689d990f3c5437 -->
<!-- api: M:Inno.Adapter.UI.RmlUi.Authoring.RmlUiDocumentFrontend.#ctor 02784d64733b1175e31858e4a9dd99ef7788d079023d31a9a5ac552658740889 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-0cec84033786467b"></a>[`public Inno.Adapter.UI.RmlUi.Authoring.RmlUiDocumentFrontend.RmlUiDocumentFrontend()`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi.Authoring/RmlUiDocumentFrontend.cs#L18) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-7b36357ef46ecef0"></a>[`public Inno.UI.Assets.UiDocumentAnalysis Inno.Adapter.UI.RmlUi.Authoring.RmlUiDocumentFrontend.Analyze(Inno.UI.Assets.UiDocumentSourceFile source)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi.Authoring/RmlUiDocumentFrontend.cs#L51) | Analyzes source text and returns validated output with diagnostics.<br>返回：The validated ui document analysis that represents the completed operation.<br>`source`：The source value or location read by this operation. | — | — |
| <a id="api-ca9b5b7b698076cf"></a>[`public Inno.UI.UiDocumentLanguageId Inno.Adapter.UI.RmlUi.Authoring.RmlUiDocumentFrontend.languageId { get; }`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi.Authoring/RmlUiDocumentFrontend.cs#L40) | Gets the RML source language identifier accepted by this frontend. | — | — |
| <a id="api-2eab4a2725cc5e3f"></a>[`public sealed class Inno.Adapter.UI.RmlUi.Authoring.RmlUiDocumentFrontend : Inno.UI.Assets.IUiDocumentFrontend`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi.Authoring/RmlUiDocumentFrontend.cs#L18) | Validates RML source for the bundled RmlUi runtime adapter. | — | — |

### `Inno.Adapter.UI.RmlUi.Authoring.RmlUiDocumentImporter`

<!-- api: T:Inno.Adapter.UI.RmlUi.Authoring.RmlUiDocumentImporter d5f14ce704b46951a081a87c2b37d3de946c661f125190929dfa88df0c8075a3 -->
<!-- api: P:Inno.Adapter.UI.RmlUi.Authoring.RmlUiDocumentImporter.supportedExtensions db7572c19a55d86c5995fe9feb14c47f9392868d0a5b64b8b6cf0ffe77bb0d09 -->
<!-- api: M:Inno.Adapter.UI.RmlUi.Authoring.RmlUiDocumentImporter.ImportAsync(Inno.Assets.Pipeline.AssetImportContext,Inno.Assets.Pipeline.AssetImportWriter{Inno.UI.UiDocumentAsset},System.Threading.CancellationToken) 4fa06c6f4e186853d9601f0c4087a63616b7803efe21e743d4882f4763cac14a -->
<!-- api: M:Inno.Adapter.UI.RmlUi.Authoring.RmlUiDocumentImporter.#ctor bf45aa28a62ee98bbc5847b7d48cbe6a990adeef8c1d010d7e7771ec04991b63 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a12cab916d465e7c"></a>[`public Inno.Adapter.UI.RmlUi.Authoring.RmlUiDocumentImporter.RmlUiDocumentImporter()`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi.Authoring/RmlUiDocumentImporter.cs#L15) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-2a3b11632adc0373"></a>[`protected override System.Threading.Tasks.ValueTask Inno.Adapter.UI.RmlUi.Authoring.RmlUiDocumentImporter.ImportAsync(Inno.Assets.Pipeline.AssetImportContext context, Inno.Assets.Pipeline.AssetImportWriter<Inno.UI.UiDocumentAsset> output, System.Threading.CancellationToken cancellationToken)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi.Authoring/RmlUiDocumentImporter.cs#L37) | Imports source content into a validated runtime asset and artifact set.<br>返回：An asynchronous operation that completes after all requested work has finished.<br>`context`：The context that supplies state and services for this operation.<br>`output`：The import output writer that receives runtime data and dependency declarations.<br>`cancellationToken`：The token that cancels the operation before it commits. | — | — |
| <a id="api-68da99369a801b4d"></a>[`public override System.Collections.Generic.IReadOnlyList<string> Inno.Adapter.UI.RmlUi.Authoring.RmlUiDocumentImporter.supportedExtensions { get; }`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi.Authoring/RmlUiDocumentImporter.cs#L20) | Gets the normalized source extensions accepted by this importer. | — | — |
| <a id="api-f2dcb3cd00496c25"></a>[`public sealed class Inno.Adapter.UI.RmlUi.Authoring.RmlUiDocumentImporter : Inno.Assets.Pipeline.AssetImporter<Inno.UI.UiDocumentAsset>`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi.Authoring/RmlUiDocumentImporter.cs#L15) | Maps the conventional .rml extension to explicit RML and RmlUi identities. | — | — |

## 项目依赖

- [Inno.Adapter.UI.RmlUi](Inno.Adapter.UI.RmlUi.md)：公开引用边界由实际签名核对。
- [Inno.UI.Assets](../../ui/Inno.UI.Assets.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
