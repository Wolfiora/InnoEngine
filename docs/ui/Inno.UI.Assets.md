# Inno.UI.Assets

[UI 索引](README.md) · [Assets](../assets/README.md)

`UiDocumentImporter` 通过语言前端分析 `.rml` 源，输出 `runtime` artifact；RML 前端验证根元素并解析内联 `@font-face`。字体文件成为标准资产依赖，文档产物保存隔离后的字体族和 face 声明。`UiDocumentAsset` 通过标准资产身份和 Missing 语义进入 Player；导入器不执行 DOM 渲染或图形上传。

## 公开契约与工作流

`UiDocumentImporter : AssetImporter<UiDocumentAsset>` 的公开 `supportedExtensions` 为 `.rml`；受保护的 `ImportAsync` 使用现有 Asset Pipeline 写入资产，调用方不需要另一套 UI 文件索引。将 `Hud.rml` 放入 Project `Assets` 后，在有效 Asset scope 中加载 `UiDocumentAsset`，再交给 [Inno.UI](Inno.UI.md) 的 `UI.LoadDocument`。安装在 Plugin 中的 RML 仍经同一 importer 进入只读 mount。

空文件、无 RML 根节点、无效字体引用和非法 UTF-8 在导入时报告错误；Missing 资产不会被替换为默认空文档。RCSS 可内联在 RML 中；本 importer 不负责读取任意外部 CSS、网络图片或系统字体，也不属于 Player runtime closure。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.UI.Assets.IUiDocumentFrontend`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.Assets.IUiDocumentFrontend`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L120) | Validates one UI document language without creating runtime UI objects. |
| [`Inno.UI.Assets.UiDocumentAnalysis Inno.UI.Assets.IUiDocumentFrontend.Analyze(Inno.UI.Assets.UiDocumentSourceFile source)`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L135) | Analyzes one immutable source snapshot. |
| [`Inno.UI.UiDocumentLanguageId Inno.UI.Assets.IUiDocumentFrontend.languageId`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L125) | Gets the open source-language identity. |

### `Inno.UI.Assets.UiDocumentAnalysis`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.Assets.UiDocumentAnalysis`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L75) | Returns validated text without retaining parser or implementation objects. |
| [`Inno.UI.Assets.UiDocumentAnalysis.UiDocumentAnalysis(string? text, System.Collections.Generic.IEnumerable<Inno.UI.Assets.UiDocumentDiagnostic> diagnostics, System.Collections.Generic.IEnumerable<Inno.UI.Assets.UiDocumentFontDeclaration>? fonts = null)`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L89) | Creates a frozen analysis result. |
| [`System.Collections.Generic.IReadOnlyList<Inno.UI.Assets.UiDocumentDiagnostic> Inno.UI.Assets.UiDocumentAnalysis.diagnostics`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L106) | Gets frozen diagnostics. |
| [`System.Collections.Generic.IReadOnlyList<Inno.UI.Assets.UiDocumentFontDeclaration> Inno.UI.Assets.UiDocumentAnalysis.fonts`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L110) | Gets font assets declared by the canonical document. |
| [`bool Inno.UI.Assets.UiDocumentAnalysis.succeeded`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L114) | Gets whether the frontend accepted the source. |
| [`string? Inno.UI.Assets.UiDocumentAnalysis.text`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L102) | Gets canonical source text, or null after failure. |

### `Inno.UI.Assets.UiDocumentDiagnostic`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.Assets.UiDocumentDiagnostic`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L43) | Reports a language-neutral source problem. |

### `Inno.UI.Assets.UiDocumentFontDeclaration`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.Assets.UiDocumentFontDeclaration`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L65) | One font asset declared by a UI document language frontend. |

### `Inno.UI.Assets.UiDocumentFrontendCatalog`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.Assets.UiDocumentAnalysis Inno.UI.Assets.UiDocumentFrontendCatalog.Analyze(Inno.UI.UiDocumentLanguageId language, Inno.UI.Assets.UiDocumentSourceFile source)`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L175) | Analyzes source using an explicit language. |
| [`Inno.UI.Assets.UiDocumentFrontendCatalog`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L141) | Owns one immutable UI language registration snapshot. |
| [`Inno.UI.Assets.UiDocumentFrontendCatalog.UiDocumentFrontendCatalog(System.Collections.Generic.IEnumerable<Inno.UI.Assets.IUiDocumentFrontend> frontends)`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L150) | Captures a validated provider set. |
| [`System.Collections.Generic.IReadOnlyList<Inno.UI.UiDocumentLanguageId> Inno.UI.Assets.UiDocumentFrontendCatalog.languageIds`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L161) | Gets registered language identities. |

### `Inno.UI.Assets.UiDocumentFrontendRegistry`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.Assets.UiDocumentAnalysis Inno.UI.Assets.UiDocumentFrontendRegistry.Analyze(Inno.UI.UiDocumentLanguageId language, Inno.UI.Assets.UiDocumentSourceFile source)`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L209) | Analyzes source while preventing provider retirement. |
| [`Inno.UI.Assets.UiDocumentFrontendRegistry`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L187) | Discovers UI language frontends through the shared type-generation transaction. |
| [`Inno.UI.Assets.UiDocumentFrontendRegistry.UiDocumentFrontendRegistry(Inno.Extensibility.Types.TypeCatalog types)`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L196) | Registers with the owner type catalog. |
| [`override Inno.UI.Assets.UiDocumentFrontendCatalog Inno.UI.Assets.UiDocumentFrontendRegistry.Build(Inno.Extensibility.Types.TypeCacheSnapshot types)`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L225) | Builds a validated result from the current immutable input snapshot. |
| [`override void Inno.UI.Assets.UiDocumentFrontendRegistry.DisposeSnapshot(Inno.UI.Assets.UiDocumentFrontendCatalog snapshot)`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L240) | Releases the generation lease retained by an immutable registry snapshot. |

### `Inno.UI.Assets.UiDocumentImportPipeline`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.Assets.UiDocumentImportPipeline`](../../src/services/ui/Inno.UI.Assets/UiDocumentImporter.cs#L70) | Coordinates source analysis, font dependencies, and runtime UI asset emission. |
| [`static System.Threading.Tasks.ValueTask Inno.UI.Assets.UiDocumentImportPipeline.ImportAsync(Inno.Assets.Pipeline.AssetImportContext context, Inno.Assets.Pipeline.AssetImportWriter<Inno.UI.UiDocumentAsset> output, Inno.UI.UiDocumentLanguageId language, string implementationId, System.Threading.CancellationToken cancellationToken)`](../../src/services/ui/Inno.UI.Assets/UiDocumentImporter.cs#L93) | Validates and emits a deterministic runtime payload. |

### `Inno.UI.Assets.UiDocumentImportSettings`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.Assets.UiDocumentImportSettings`](../../src/services/ui/Inno.UI.Assets/UiDocumentImportSettings.cs#L9) | Explicitly selects language and runtime implementation for neutral UI source. |
| [`string Inno.UI.Assets.UiDocumentImportSettings.implementationId`](../../src/services/ui/Inno.UI.Assets/UiDocumentImportSettings.cs#L19) | Gets or sets the exact runtime implementation identity. |
| [`string Inno.UI.Assets.UiDocumentImportSettings.languageId`](../../src/services/ui/Inno.UI.Assets/UiDocumentImportSettings.cs#L15) | Gets or sets the registered source-language identity. |

### `Inno.UI.Assets.UiDocumentImporter`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.Assets.UiDocumentImporter`](../../src/services/ui/Inno.UI.Assets/UiDocumentImporter.cs#L18) | Imports implementation-neutral UTF-8 UI source using explicit sidecar settings. |
| [`override Inno.Core.Serialization.ISerializable Inno.UI.Assets.UiDocumentImporter.CreateImportSettings()`](../../src/services/ui/Inno.UI.Assets/UiDocumentImporter.cs#L32) | Creates an import settings using this implementation's validated inputs. |
| [`override System.Collections.Generic.IReadOnlyList<string> Inno.UI.Assets.UiDocumentImporter.supportedExtensions`](../../src/services/ui/Inno.UI.Assets/UiDocumentImporter.cs#L24) | Gets the normalized source extensions accepted by this importer. |
| [`override System.Threading.Tasks.ValueTask Inno.UI.Assets.UiDocumentImporter.ImportAsync(Inno.Assets.Pipeline.AssetImportContext context, Inno.Assets.Pipeline.AssetImportWriter<Inno.UI.UiDocumentAsset> output, System.Threading.CancellationToken cancellationToken)`](../../src/services/ui/Inno.UI.Assets/UiDocumentImporter.cs#L49) | Imports source content into a validated runtime asset and artifact set. |

### `Inno.UI.Assets.UiDocumentSourceFile`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.UI.Assets.UiDocumentSourceFile`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L22) | Contains immutable source text supplied to a UI language frontend. |

## 项目依赖

- [Inno.UI](Inno.UI.md)：公开引用边界由实际签名核对。
- [Inno.Assets.Pipeline](../assets/Inno.Assets.Pipeline.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
