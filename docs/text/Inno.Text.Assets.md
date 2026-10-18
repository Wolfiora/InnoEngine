# Inno.Text.Assets

[Text 索引](README.md) · [Assets](../assets/README.md)

`FontImporter` 处理 `.ttf`、`.otf`、`.ttc`、`.otc`。导入时验证 sfnt/collection 容器头，输出 `runtime` metadata 和不可变 `font-data` artifact。运行时只消费 artifact，不重新读取 Project 源文件；格式错误在导入边界报告。

## 公开契约与工作流

`FontImporter : AssetImporter<FontAsset>` 的公开 `supportedExtensions` 列出四种格式；创作侧通过标准 Asset Pipeline 发现该 importer，无须单独调用。它的 `ImportAsync` 是标准受保护扩展点，读取当前源文件、写入 `FontAsset` 和 `font-data`，并交由统一 Catalog 管理身份、Missing 与依赖。业务脚本仅通过 [Inno.Text](Inno.Text.md) 的 `FontAsset` 使用结果，不引用此创作项目。

将 `Fonts/Interface.ttf` 放入 Project `Assets` 后，脚本可用 `Assets.Load<FontAsset>(Assets.LocalPath("Fonts/Interface.ttf"))` 获得资产；必须在 Asset scope 中调用。字体源缺失或头损坏时导入失败，不生成可被运行时误用的空 face。运行时部署只保留经验证的字体数据和 metadata，不部署 importer。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Text.Assets.FontImporter`

<!-- api: T:Inno.Text.Assets.FontImporter 550c947138e75fe96f9ae533890adac3ceca05a30e333f61ff4656f17504c287 -->
<!-- api: P:Inno.Text.Assets.FontImporter.supportedExtensions c79e4558d0f8b06a7b17d8813e97acddafcb30cb2f7c21301f994ecc3a729f8d -->
<!-- api: M:Inno.Text.Assets.FontImporter.ImportAsync(Inno.Assets.Pipeline.AssetImportContext,Inno.Assets.Pipeline.AssetImportWriter{Inno.Text.FontAsset},System.Threading.CancellationToken) 758ccdd86e98e2cc3241988968ac8daad50999e8166b81598cfb75a9f7fc1839 -->
<!-- api: M:Inno.Text.Assets.FontImporter.#ctor 648b45149e24f73a4ffe3195cf20190ae1fc414f4c7d8a6e6918b54a3eeda827 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d55e485df986749e"></a>[`public Inno.Text.Assets.FontImporter.FontImporter()`](../../src/services/text/Inno.Text.Assets/FontImporter.cs#L16) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-0ab65585af4f0788"></a>[`protected override System.Threading.Tasks.ValueTask Inno.Text.Assets.FontImporter.ImportAsync(Inno.Assets.Pipeline.AssetImportContext context, Inno.Assets.Pipeline.AssetImportWriter<Inno.Text.FontAsset> output, System.Threading.CancellationToken cancellationToken)`](../../src/services/text/Inno.Text.Assets/FontImporter.cs#L38) | Validates the source container and emits runtime metadata plus encoded font data.<br>返回：An operation that completes after both artifacts are staged.<br>`context`：The source import context.<br>`output`：The candidate artifact writer.<br>`cancellationToken`：Cancellation observed by artifact writes. | — | — |
| <a id="api-91b4822cbda82ce0"></a>[`public override System.Collections.Generic.IReadOnlyList<string> Inno.Text.Assets.FontImporter.supportedExtensions { get; }`](../../src/services/text/Inno.Text.Assets/FontImporter.cs#L21) | Gets the supported OpenType source extensions. | — | — |
| <a id="api-bb62dea43dbbbe91"></a>[`public sealed class Inno.Text.Assets.FontImporter : Inno.Assets.Pipeline.AssetImporter<Inno.Text.FontAsset>`](../../src/services/text/Inno.Text.Assets/FontImporter.cs#L16) | Imports OpenType fonts into compact metadata and immutable encoded font artifacts. | — | — |

## 项目依赖

- [Inno.Text](Inno.Text.md)：公开引用边界由实际签名核对。
- [Inno.Assets.Pipeline](../assets/Inno.Assets.Pipeline.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
