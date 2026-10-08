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

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Adapter.UI.RmlUi.Authoring.RmlUiDocumentFrontend`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.UI.RmlUi.Authoring.RmlUiDocumentFrontend`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi.Authoring/RmlUiDocumentFrontend.cs#L18) | Validates RML source for the bundled RmlUi runtime adapter. |
| [`Inno.UI.Assets.UiDocumentAnalysis Inno.Adapter.UI.RmlUi.Authoring.RmlUiDocumentFrontend.Analyze(Inno.UI.Assets.UiDocumentSourceFile source)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi.Authoring/RmlUiDocumentFrontend.cs#L51) | Analyzes source text and returns validated output with diagnostics. |
| [`Inno.UI.UiDocumentLanguageId Inno.Adapter.UI.RmlUi.Authoring.RmlUiDocumentFrontend.languageId`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi.Authoring/RmlUiDocumentFrontend.cs#L40) | Gets the RML source language identifier accepted by this frontend. |

### `Inno.Adapter.UI.RmlUi.Authoring.RmlUiDocumentImporter`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.UI.RmlUi.Authoring.RmlUiDocumentImporter`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi.Authoring/RmlUiDocumentImporter.cs#L14) | Maps the conventional .rml extension to explicit RML and RmlUi identities. |
| [`override System.Collections.Generic.IReadOnlyList<string> Inno.Adapter.UI.RmlUi.Authoring.RmlUiDocumentImporter.supportedExtensions`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi.Authoring/RmlUiDocumentImporter.cs#L20) | Gets the normalized source extensions accepted by this importer. |
| [`override System.Threading.Tasks.ValueTask Inno.Adapter.UI.RmlUi.Authoring.RmlUiDocumentImporter.ImportAsync(Inno.Assets.Pipeline.AssetImportContext context, Inno.Assets.Pipeline.AssetImportWriter<Inno.UI.UiDocumentAsset> output, System.Threading.CancellationToken cancellationToken)`](../../../backends/RmlUi/runtime/Inno.Adapter.UI.RmlUi.Authoring/RmlUiDocumentImporter.cs#L37) | Imports source content into a validated runtime asset and artifact set. |

## 项目依赖

- [Inno.Adapter.UI.RmlUi](Inno.Adapter.UI.RmlUi.md)：公开引用边界由实际签名核对。
- [Inno.UI.Assets](../../ui/Inno.UI.Assets.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
