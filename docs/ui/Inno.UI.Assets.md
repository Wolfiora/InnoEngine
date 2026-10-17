# Inno.UI.Assets

[UI 索引](README.md) · [Assets](../assets/README.md)

`UiDocumentImporter` 通过语言前端分析 `.rml` 源，输出 `runtime` artifact；RML 前端验证根元素并解析内联 `@font-face`。字体文件成为标准资产依赖，文档产物保存隔离后的字体族和 face 声明。`UiDocumentAsset` 通过标准资产身份和 Missing 语义进入 Player；导入器不执行 DOM 渲染或图形上传。

## 公开契约与工作流

`UiDocumentImporter : AssetImporter<UiDocumentAsset>` 的公开 `supportedExtensions` 为 `.rml`；受保护的 `ImportAsync` 使用现有 Asset Pipeline 写入资产，调用方不需要另一套 UI 文件索引。将 `Hud.rml` 放入 Project `Assets` 后，在有效 Asset scope 中加载 `UiDocumentAsset`，再交给 [Inno.UI](Inno.UI.md) 的 `UI.LoadDocument`。安装在 Plugin 中的 RML 仍经同一 importer 进入只读 mount。

空文件、无 RML 根节点、无效字体引用和非法 UTF-8 在导入时报告错误；Missing 资产不会被替换为默认空文档。RCSS 可内联在 RML 中；本 importer 不负责读取任意外部 CSS、网络图片或系统字体，也不属于 Player runtime closure。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.UI.Assets.IUiDocumentFrontend`

<!-- api: T:Inno.UI.Assets.IUiDocumentFrontend 912f7c7b466709182713eebd1691df724d05f7f70b27d323d9d2af12b0815c62 -->
<!-- api: P:Inno.UI.Assets.IUiDocumentFrontend.languageId 200a4cb4f45359d10fc44cec498034d8f1279b11b77fa0b22fa29ed6b7ea714d -->
<!-- api: M:Inno.UI.Assets.IUiDocumentFrontend.Analyze(Inno.UI.Assets.UiDocumentSourceFile) 1b57f4c88d43d245002eb24d1b1efb20ad21c3d91a4c5586d953a0dcbc4d8cee -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-0afe849e6b277078"></a>[`Inno.UI.Assets.UiDocumentAnalysis Inno.UI.Assets.IUiDocumentFrontend.Analyze(Inno.UI.Assets.UiDocumentSourceFile source)`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L135) | Analyzes one immutable source snapshot.<br>返回：Canonical text or neutral diagnostics.<br>`source`：Original source snapshot. | — | — |
| <a id="api-4ba196cb5daf8ebb"></a>[`Inno.UI.UiDocumentLanguageId Inno.UI.Assets.IUiDocumentFrontend.languageId { get; }`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L125) | Gets the open source-language identity. | — | — |
| <a id="api-016cd54dccf8c937"></a>[`public interface Inno.UI.Assets.IUiDocumentFrontend`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L120) | Validates one UI document language without creating runtime UI objects. | — | — |

### `Inno.UI.Assets.UiDocumentAnalysis`

<!-- api: T:Inno.UI.Assets.UiDocumentAnalysis 5f7fd29183bc1a4b516ad3cc3eba5819957e2c4cd860c8b8512868ca08032d7c -->
<!-- api: M:Inno.UI.Assets.UiDocumentAnalysis.#ctor(System.String,System.Collections.Generic.IEnumerable{Inno.UI.Assets.UiDocumentDiagnostic},System.Collections.Generic.IEnumerable{Inno.UI.Assets.UiDocumentFontDeclaration}) c2e5a79461e499f98a1d54aecc93ce6d5747a3bc9683acfa9182ea7a447ebfca -->
<!-- api: P:Inno.UI.Assets.UiDocumentAnalysis.text b7bfe422f61a35f017b19a31d936ec472ff57a964b35929bbfdfd462cefd8808 -->
<!-- api: P:Inno.UI.Assets.UiDocumentAnalysis.diagnostics 00afa058d32d4a9cd5fe97257c0880321719ad465ac2f6ad8cbec482a94a4268 -->
<!-- api: P:Inno.UI.Assets.UiDocumentAnalysis.fonts cecf02a4810e3eab45ddf2e651dc76b8d3068740358b24a9de065e7f437ccf07 -->
<!-- api: P:Inno.UI.Assets.UiDocumentAnalysis.succeeded 86b1223368f73fb9e4ba64cbf49296bb2c61f79fe77dee7199b0282b46c524c3 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-64dfd6a408f60fc6"></a>[`public Inno.UI.Assets.UiDocumentAnalysis.UiDocumentAnalysis(string? text, System.Collections.Generic.IEnumerable<Inno.UI.Assets.UiDocumentDiagnostic> diagnostics, System.Collections.Generic.IEnumerable<Inno.UI.Assets.UiDocumentFontDeclaration>? fonts = null)`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L89) | Creates a frozen analysis result.<br>`text`：Validated canonical text, or null on failure.<br>`diagnostics`：Complete frontend diagnostics.<br>`fonts`：Font assets declared by the document. | — | — |
| <a id="api-218f125e0b646702"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.UI.Assets.UiDocumentDiagnostic> Inno.UI.Assets.UiDocumentAnalysis.diagnostics { get; }`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L106) | Gets frozen diagnostics. | — | — |
| <a id="api-9e35a6ccd25f769f"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.UI.Assets.UiDocumentFontDeclaration> Inno.UI.Assets.UiDocumentAnalysis.fonts { get; }`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L110) | Gets font assets declared by the canonical document. | — | — |
| <a id="api-19899398dd0472ee"></a>[`public bool Inno.UI.Assets.UiDocumentAnalysis.succeeded { get; }`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L114) | Gets whether the frontend accepted the source. | — | — |
| <a id="api-828be11737d89378"></a>[`public string? Inno.UI.Assets.UiDocumentAnalysis.text { get; }`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L102) | Gets canonical source text, or null after failure. | — | — |
| <a id="api-8c8e3d35bdc91a98"></a>[`public sealed class Inno.UI.Assets.UiDocumentAnalysis`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L75) | Returns validated text without retaining parser or implementation objects. | — | — |

### `Inno.UI.Assets.UiDocumentDiagnostic`

<!-- api: T:Inno.UI.Assets.UiDocumentDiagnostic 5b2325ac8b1f919202e2ab7651e82a38e0adc01aa467c43c320d96cf9407c18d -->
<!-- api: M:Inno.UI.Assets.UiDocumentDiagnostic.#ctor(System.String,System.String,System.Int32,System.Int32) 2f49c3b2553cb4f098ca7de4f931f8cdd009eadd4d9acd88da7f2c8399f67043 -->
<!-- api: P:Inno.UI.Assets.UiDocumentDiagnostic.code 3bbfc51563a090d0fdbc597e590126da802724519f447e32a834cb8340084577 -->
<!-- api: P:Inno.UI.Assets.UiDocumentDiagnostic.message 84ed351bdeaca477c1e08c37ce69094187e82e72ac3bb0beadb691228d973f80 -->
<!-- api: P:Inno.UI.Assets.UiDocumentDiagnostic.line b8db3f37e11816db4e64fa61a3bbe1d995ab7c81457681e0bcc412ee5558e445 -->
<!-- api: P:Inno.UI.Assets.UiDocumentDiagnostic.column 63d064ae1cb763e5970498e771fce006e4b7d91a9bba9db84750ff98dc9d685c -->
<!-- api: M:Inno.UI.Assets.UiDocumentDiagnostic.ToString 0689257f351000264afb8e6e01a8ca64e7ce7463331005fb45817fb213b68824 -->
<!-- api: M:Inno.UI.Assets.UiDocumentDiagnostic.op_Inequality(Inno.UI.Assets.UiDocumentDiagnostic,Inno.UI.Assets.UiDocumentDiagnostic) 859b60dc241e36ee9241d3a1518e4dac6c3033e0f23c3c0bfc27751fdbf5af76 -->
<!-- api: M:Inno.UI.Assets.UiDocumentDiagnostic.op_Equality(Inno.UI.Assets.UiDocumentDiagnostic,Inno.UI.Assets.UiDocumentDiagnostic) d197cfc9384d640e162f9c8396df4a4ff87a114562ed0361339e0893fec1acf5 -->
<!-- api: M:Inno.UI.Assets.UiDocumentDiagnostic.GetHashCode d3928961aa7d640ddb5d19c9ee7763efa7c0fd97d6e848b14d73b702f83ed4f9 -->
<!-- api: M:Inno.UI.Assets.UiDocumentDiagnostic.Equals(System.Object) 1962d8bb24ca798415b281653e69b428a8d1dfa7870dde4113039d0e08abdcbd -->
<!-- api: M:Inno.UI.Assets.UiDocumentDiagnostic.Equals(Inno.UI.Assets.UiDocumentDiagnostic) 6d46a98819d439d0b389199d1108577d15e24a1d99365dd32c13946820713cae -->
<!-- api: M:Inno.UI.Assets.UiDocumentDiagnostic.Deconstruct(System.String@,System.String@,System.Int32@,System.Int32@) b9a9cf635056f84abeba5ff88747fb06c19301f8a19085ffb54a954fd73ffa72 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-94f76afbdde49a90"></a>[`public Inno.UI.Assets.UiDocumentDiagnostic.UiDocumentDiagnostic(string code, string message, int line, int column)`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L43) | Reports a language-neutral source problem.<br>`code`：Stable diagnostic code.<br>`message`：Actionable description.<br>`line`：One-based source line.<br>`column`：One-based source column. | — | — |
| <a id="api-4b7e544b5a8adbce"></a>[`public void Inno.UI.Assets.UiDocumentDiagnostic.Deconstruct(out string code, out string message, out int line, out int column)`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L43) | C# record 生成的解构入口，按主构造声明顺序返回位置成员。 | — | — |
| <a id="api-d17eb9b348cbfb87"></a>[`public bool Inno.UI.Assets.UiDocumentDiagnostic.Equals(Inno.UI.Assets.UiDocumentDiagnostic? other)`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L43) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-bf68766c5e548788"></a>[`public override bool Inno.UI.Assets.UiDocumentDiagnostic.Equals(object? obj)`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L43) | Determines whether the specified object is equal to the current object.<br>返回：true if the specified object is equal to the current object; otherwise, false.<br>`obj`：The object to compare with the current object. | — | — |
| <a id="api-9f9a86951a9247c2"></a>[`public override int Inno.UI.Assets.UiDocumentDiagnostic.GetHashCode()`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L43) | Serves as the default hash function.<br>返回：A hash code for the current object. | — | — |
| <a id="api-10d1297a8dd62b9a"></a>[`public static bool Inno.UI.Assets.UiDocumentDiagnostic.operator ==(Inno.UI.Assets.UiDocumentDiagnostic? left, Inno.UI.Assets.UiDocumentDiagnostic? right)`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L43) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-d119972c4042c999"></a>[`public static bool Inno.UI.Assets.UiDocumentDiagnostic.operator !=(Inno.UI.Assets.UiDocumentDiagnostic? left, Inno.UI.Assets.UiDocumentDiagnostic? right)`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L43) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-1998f52398caf77e"></a>[`public override string Inno.UI.Assets.UiDocumentDiagnostic.ToString()`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L43) | Returns a string that represents the current object.<br>返回：A string that represents the current object. | — | — |
| <a id="api-78026b04e8059836"></a>[`public string Inno.UI.Assets.UiDocumentDiagnostic.code { get; init; }`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L44) | Stable diagnostic code. | — | — |
| <a id="api-f837e1c91ce96ba5"></a>[`public int Inno.UI.Assets.UiDocumentDiagnostic.column { get; init; }`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L47) | One-based source column. | — | — |
| <a id="api-07ffd182a1340210"></a>[`public int Inno.UI.Assets.UiDocumentDiagnostic.line { get; init; }`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L46) | One-based source line. | — | — |
| <a id="api-466b81dac3ac165f"></a>[`public string Inno.UI.Assets.UiDocumentDiagnostic.message { get; init; }`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L45) | Actionable description. | — | — |
| <a id="api-40e6e893c3c97988"></a>[`public sealed record Inno.UI.Assets.UiDocumentDiagnostic : System.IEquatable<Inno.UI.Assets.UiDocumentDiagnostic>`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L43) | Reports a language-neutral source problem.<br>`code`：Stable diagnostic code.<br>`message`：Actionable description.<br>`line`：One-based source line.<br>`column`：One-based source column. | — | — |

### `Inno.UI.Assets.UiDocumentFontDeclaration`

<!-- api: T:Inno.UI.Assets.UiDocumentFontDeclaration e1eed83a12a10bcdbd8e4e81070d5f63b3f1aca5d2f042ec7f8803b4283aa767 -->
<!-- api: M:Inno.UI.Assets.UiDocumentFontDeclaration.#ctor(System.String,System.String,Inno.Text.TextFontStyle,System.Int32) e6bd3c9fcd87a53931cb6ef8108bad83e37eca68a76dbece92daa3d73ac481c6 -->
<!-- api: P:Inno.UI.Assets.UiDocumentFontDeclaration.assetPath da38e167c56c301ea6b1a4deafb768c1ea1e20865faedab64e95d94102fc8991 -->
<!-- api: P:Inno.UI.Assets.UiDocumentFontDeclaration.family 1fb53f8288a83e9e4608fdfaec1c6b556603c43b9308b1186aeb8268fcfbd30b -->
<!-- api: P:Inno.UI.Assets.UiDocumentFontDeclaration.style 77b0e090b567e15a84e252397c0c1d2e860468f0ad6248b451d88367a0530ffe -->
<!-- api: P:Inno.UI.Assets.UiDocumentFontDeclaration.weight c6a0f1d9d4f2e8251847dabf08a819d72498c9d096e296fa88f33981300e0ab2 -->
<!-- api: M:Inno.UI.Assets.UiDocumentFontDeclaration.ToString b942571ce3b52dabb53959f024961735e82d18699500ae47472491ac29a29ed4 -->
<!-- api: M:Inno.UI.Assets.UiDocumentFontDeclaration.op_Inequality(Inno.UI.Assets.UiDocumentFontDeclaration,Inno.UI.Assets.UiDocumentFontDeclaration) b6c20ee43674f0759ac262397893ceb4746e48550194f12b8006dce7fe5c61aa -->
<!-- api: M:Inno.UI.Assets.UiDocumentFontDeclaration.op_Equality(Inno.UI.Assets.UiDocumentFontDeclaration,Inno.UI.Assets.UiDocumentFontDeclaration) 26436c0e62bfb88a528c7c79b9fbe69894212f531cadf21d590c0c3602929d06 -->
<!-- api: M:Inno.UI.Assets.UiDocumentFontDeclaration.GetHashCode a422be636b55eb9fb315d0d09f0e27a688bf4f7471a48ddc4783418687e5e63f -->
<!-- api: M:Inno.UI.Assets.UiDocumentFontDeclaration.Equals(System.Object) a2ebc384fb64354f121cbdf1b6255339ff67d16b6c080e0f4d595b8a05400a25 -->
<!-- api: M:Inno.UI.Assets.UiDocumentFontDeclaration.Equals(Inno.UI.Assets.UiDocumentFontDeclaration) fcdcd59a605cf394da2258cf3f5cc8c3c3b925273afd70471c71a34af5f2d918 -->
<!-- api: M:Inno.UI.Assets.UiDocumentFontDeclaration.Deconstruct(System.String@,System.String@,Inno.Text.TextFontStyle@,System.Int32@) fea74b9e0bf3cab0799ccec35feb3812a2ef669d028d5f5842de77cb6cbd4d15 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a98da02c4c0f6e6a"></a>[`public Inno.UI.Assets.UiDocumentFontDeclaration.UiDocumentFontDeclaration(string assetPath, string family, Inno.Text.TextFontStyle style, int weight)`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L65) | One font asset declared by a UI document language frontend.<br>`assetPath`：Isolated font asset path.<br>`family`：Document-private physical family used in canonical text.<br>`style`：CSS-compatible face style.<br>`weight`：CSS-compatible face weight. | — | — |
| <a id="api-c03b4a516ab5cf6a"></a>[`public void Inno.UI.Assets.UiDocumentFontDeclaration.Deconstruct(out string assetPath, out string family, out Inno.Text.TextFontStyle style, out int weight)`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L65) | C# record 生成的解构入口，按主构造声明顺序返回位置成员。 | — | — |
| <a id="api-af2b6dc9ee7d8615"></a>[`public bool Inno.UI.Assets.UiDocumentFontDeclaration.Equals(Inno.UI.Assets.UiDocumentFontDeclaration? other)`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L65) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-35b6c2a12658e33b"></a>[`public override bool Inno.UI.Assets.UiDocumentFontDeclaration.Equals(object? obj)`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L65) | Determines whether the specified object is equal to the current object.<br>返回：true if the specified object is equal to the current object; otherwise, false.<br>`obj`：The object to compare with the current object. | — | — |
| <a id="api-40c0649719cf5fa6"></a>[`public override int Inno.UI.Assets.UiDocumentFontDeclaration.GetHashCode()`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L65) | Serves as the default hash function.<br>返回：A hash code for the current object. | — | — |
| <a id="api-ba726c93181502ed"></a>[`public static bool Inno.UI.Assets.UiDocumentFontDeclaration.operator ==(Inno.UI.Assets.UiDocumentFontDeclaration? left, Inno.UI.Assets.UiDocumentFontDeclaration? right)`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L65) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-0f893c6f4432d14c"></a>[`public static bool Inno.UI.Assets.UiDocumentFontDeclaration.operator !=(Inno.UI.Assets.UiDocumentFontDeclaration? left, Inno.UI.Assets.UiDocumentFontDeclaration? right)`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L65) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-27084b1bb6fb5f7b"></a>[`public override string Inno.UI.Assets.UiDocumentFontDeclaration.ToString()`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L65) | Returns a string that represents the current object.<br>返回：A string that represents the current object. | — | — |
| <a id="api-017802b29733ca2e"></a>[`public string Inno.UI.Assets.UiDocumentFontDeclaration.assetPath { get; init; }`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L66) | Isolated font asset path. | — | — |
| <a id="api-5ba0bcd443f24c3d"></a>[`public string Inno.UI.Assets.UiDocumentFontDeclaration.family { get; init; }`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L67) | Document-private physical family used in canonical text. | — | — |
| <a id="api-3c193ee5cc09c6f4"></a>[`public Inno.Text.TextFontStyle Inno.UI.Assets.UiDocumentFontDeclaration.style { get; init; }`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L68) | CSS-compatible face style. | — | — |
| <a id="api-08bf7a8c9fc2ada9"></a>[`public int Inno.UI.Assets.UiDocumentFontDeclaration.weight { get; init; }`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L69) | CSS-compatible face weight. | — | — |
| <a id="api-f357a8957541e868"></a>[`public sealed record Inno.UI.Assets.UiDocumentFontDeclaration : System.IEquatable<Inno.UI.Assets.UiDocumentFontDeclaration>`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L65) | One font asset declared by a UI document language frontend.<br>`assetPath`：Isolated font asset path.<br>`family`：Document-private physical family used in canonical text.<br>`style`：CSS-compatible face style.<br>`weight`：CSS-compatible face weight. | — | — |

### `Inno.UI.Assets.UiDocumentFrontendCatalog`

<!-- api: T:Inno.UI.Assets.UiDocumentFrontendCatalog 01dada62ddc3714ebc3373aba25948a10b72e333d36c1f9f2a19fe1bef3e6e1c -->
<!-- api: M:Inno.UI.Assets.UiDocumentFrontendCatalog.#ctor(System.Collections.Generic.IEnumerable{Inno.UI.Assets.IUiDocumentFrontend}) 4e74ed9fb4ef38c24a1785abd984d06337a6dbcc4666791d2cd51fdf04aed3b6 -->
<!-- api: P:Inno.UI.Assets.UiDocumentFrontendCatalog.languageIds f3a45c6ff572313029123024ee1ef0fc0fed2062a06a48733b966c1cfd04de6a -->
<!-- api: M:Inno.UI.Assets.UiDocumentFrontendCatalog.Analyze(Inno.UI.UiDocumentLanguageId,Inno.UI.Assets.UiDocumentSourceFile) b1ea2cd3ef84303de5686784606fed0967f592487e8d6d2855cedb29d4d6af79 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-6858533be0ca33b3"></a>[`public Inno.UI.Assets.UiDocumentFrontendCatalog.UiDocumentFrontendCatalog(System.Collections.Generic.IEnumerable<Inno.UI.Assets.IUiDocumentFrontend> frontends)`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L150) | Captures a validated provider set.<br>`frontends`：Providers owned by the authoring generation. | — | — |
| <a id="api-48d9ace104bcb118"></a>[`public Inno.UI.Assets.UiDocumentAnalysis Inno.UI.Assets.UiDocumentFrontendCatalog.Analyze(Inno.UI.UiDocumentLanguageId language, Inno.UI.Assets.UiDocumentSourceFile source)`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L175) | Analyzes source using an explicit language.<br>返回：Neutral analysis result.<br>`language`：Registered language identity.<br>`source`：Immutable source snapshot. | — | — |
| <a id="api-5f5e0b955cc65f8e"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.UI.UiDocumentLanguageId> Inno.UI.Assets.UiDocumentFrontendCatalog.languageIds { get; }`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L161) | Gets registered language identities. | — | — |
| <a id="api-87c1b02e0fffe81b"></a>[`public sealed class Inno.UI.Assets.UiDocumentFrontendCatalog`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L141) | Owns one immutable UI language registration snapshot. | — | — |

### `Inno.UI.Assets.UiDocumentFrontendRegistry`

<!-- api: T:Inno.UI.Assets.UiDocumentFrontendRegistry 10a320522e3b77d652b8bbba93fd7a04e2576f8a4714995000b42e2c6d6837c4 -->
<!-- api: M:Inno.UI.Assets.UiDocumentFrontendRegistry.#ctor(Inno.Extensibility.Types.TypeCatalog) 7fcbb7239c126d12913a69613ef0c708bdc4dc1bf92b65a86e726602271f10e2 -->
<!-- api: M:Inno.UI.Assets.UiDocumentFrontendRegistry.Analyze(Inno.UI.UiDocumentLanguageId,Inno.UI.Assets.UiDocumentSourceFile) 4639a22687e7aec9590a91b20b8b231c2d50ec7a40f78caed61ed68f6883de25 -->
<!-- api: M:Inno.UI.Assets.UiDocumentFrontendRegistry.Build(Inno.Extensibility.Types.TypeCacheSnapshot) 93caa7eaab6d2d5c484e382ca5de77b4ef2eb749b02eb6a86847c820b769114b -->
<!-- api: M:Inno.UI.Assets.UiDocumentFrontendRegistry.DisposeSnapshot(Inno.UI.Assets.UiDocumentFrontendCatalog) 3ab7b1e3f62053f1335bd08aba51402072f6fc04b2f87d7f0e69664f5784153b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-15ceccad4e314c88"></a>[`public Inno.UI.Assets.UiDocumentFrontendRegistry.UiDocumentFrontendRegistry(Inno.Extensibility.Types.TypeCatalog types)`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L196) | Registers with the owner type catalog.<br>`types`：Catalog that outlives this registry. | — | — |
| <a id="api-2333aad5e95f4932"></a>[`public Inno.UI.Assets.UiDocumentAnalysis Inno.UI.Assets.UiDocumentFrontendRegistry.Analyze(Inno.UI.UiDocumentLanguageId language, Inno.UI.Assets.UiDocumentSourceFile source)`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L209) | Analyzes source while preventing provider retirement.<br>返回：Detached neutral analysis.<br>`language`：Exact language identity.<br>`source`：Immutable source snapshot. | — | — |
| <a id="api-c6a9659692bd1a9b"></a>[`protected override Inno.UI.Assets.UiDocumentFrontendCatalog Inno.UI.Assets.UiDocumentFrontendRegistry.Build(Inno.Extensibility.Types.TypeCacheSnapshot types)`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L225) | Builds a validated result from the current immutable input snapshot.<br>返回：The validated ui document frontend catalog that represents the completed operation.<br>`types`：The active type catalog generation used for extension resolution. | — | — |
| <a id="api-c8b509adceebfd7d"></a>[`protected override void Inno.UI.Assets.UiDocumentFrontendRegistry.DisposeSnapshot(Inno.UI.Assets.UiDocumentFrontendCatalog snapshot)`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L240) | Releases the generation lease retained by an immutable registry snapshot.<br>`snapshot`：The immutable state snapshot consumed by this operation. | — | — |
| <a id="api-aea441748cd94f69"></a>[`public sealed class Inno.UI.Assets.UiDocumentFrontendRegistry : Inno.Extensibility.Types.TypeRegistry<Inno.UI.Assets.UiDocumentFrontendCatalog>`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L187) | Discovers UI language frontends through the shared type-generation transaction. | — | — |

### `Inno.UI.Assets.UiDocumentImporter`

<!-- api: T:Inno.UI.Assets.UiDocumentImporter cefca682fad62c767002e3388366d34bf71aa29e315a6f92c8f6d79d0a22041a -->
<!-- api: P:Inno.UI.Assets.UiDocumentImporter.supportedExtensions e9700a28d6b7dd43d7d0a9dea10bf71234801a95d40be78724ff4d2bf51a057d -->
<!-- api: M:Inno.UI.Assets.UiDocumentImporter.CreateImportSettings 112aab4824f117a6210f725aa750acb618cfa289872d2fdf4e3e5bb2fc4a7942 -->
<!-- api: M:Inno.UI.Assets.UiDocumentImporter.ImportAsync(Inno.Assets.Pipeline.AssetImportContext,Inno.Assets.Pipeline.AssetImportWriter{Inno.UI.UiDocumentAsset},System.Threading.CancellationToken) 86cff9ce29dde70151fe7f2c178a9856edf36e93746c3833516b22451f72e6a5 -->
<!-- api: M:Inno.UI.Assets.UiDocumentImporter.#ctor c80179a0e2cda589a11352d1d4a3e3ca119de1590f93042cae15991d94856819 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-fd639e810be0a7ad"></a>[`public Inno.UI.Assets.UiDocumentImporter.UiDocumentImporter()`](../../src/services/ui/Inno.UI.Assets/UiDocumentImporter.cs#L19) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-8cf4930cb5890e17"></a>[`public override Inno.Core.Serialization.ISerializable Inno.UI.Assets.UiDocumentImporter.CreateImportSettings()`](../../src/services/ui/Inno.UI.Assets/UiDocumentImporter.cs#L32) | Creates an import settings using this implementation's validated inputs.<br>返回：The validated iserializable that represents the completed operation. | — | — |
| <a id="api-fc6b6f4c52e02ff8"></a>[`protected override System.Threading.Tasks.ValueTask Inno.UI.Assets.UiDocumentImporter.ImportAsync(Inno.Assets.Pipeline.AssetImportContext context, Inno.Assets.Pipeline.AssetImportWriter<Inno.UI.UiDocumentAsset> output, System.Threading.CancellationToken cancellationToken)`](../../src/services/ui/Inno.UI.Assets/UiDocumentImporter.cs#L49) | Imports source content into a validated runtime asset and artifact set.<br>返回：An asynchronous operation that completes after all requested work has finished.<br>`context`：The context that supplies state and services for this operation.<br>`output`：The import output writer that receives runtime data and dependency declarations.<br>`cancellationToken`：The token that cancels the operation before it commits. | — | — |
| <a id="api-fc5663a9bed23676"></a>[`public override System.Collections.Generic.IReadOnlyList<string> Inno.UI.Assets.UiDocumentImporter.supportedExtensions { get; }`](../../src/services/ui/Inno.UI.Assets/UiDocumentImporter.cs#L24) | Gets the normalized source extensions accepted by this importer. | — | — |
| <a id="api-d08fee80d090d174"></a>[`public sealed class Inno.UI.Assets.UiDocumentImporter : Inno.Assets.Pipeline.AssetImporter<Inno.UI.UiDocumentAsset>`](../../src/services/ui/Inno.UI.Assets/UiDocumentImporter.cs#L19) | Imports implementation-neutral UTF-8 UI source using explicit sidecar settings. | — | — |

### `Inno.UI.Assets.UiDocumentImportPipeline`

<!-- api: T:Inno.UI.Assets.UiDocumentImportPipeline ac837855b49bbc902efe465290b352c52f9de9e35f963f0796187a598ac9deec -->
<!-- api: M:Inno.UI.Assets.UiDocumentImportPipeline.ImportAsync(Inno.Assets.Pipeline.AssetImportContext,Inno.Assets.Pipeline.AssetImportWriter{Inno.UI.UiDocumentAsset},Inno.UI.UiDocumentLanguageId,System.String,System.Threading.CancellationToken) 56dfdc2d80cd412274ebf0a135c3ec962b2a211941740f3a666f4421824c06bc -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f2f120176788671a"></a>[`public static System.Threading.Tasks.ValueTask Inno.UI.Assets.UiDocumentImportPipeline.ImportAsync(Inno.Assets.Pipeline.AssetImportContext context, Inno.Assets.Pipeline.AssetImportWriter<Inno.UI.UiDocumentAsset> output, Inno.UI.UiDocumentLanguageId language, string implementationId, System.Threading.CancellationToken cancellationToken)`](../../src/services/ui/Inno.UI.Assets/UiDocumentImporter.cs#L93) | Validates and emits a deterministic runtime payload.<br>返回：An operation completing after the payload is staged.<br>`context`：Source import context.<br>`output`：Candidate writer.<br>`language`：Explicit source language.<br>`implementationId`：Explicit runtime implementation.<br>`cancellationToken`：Cancellation observed by artifact writes. | — | — |
| <a id="api-f4582246ee8e0fa7"></a>[`public static class Inno.UI.Assets.UiDocumentImportPipeline`](../../src/services/ui/Inno.UI.Assets/UiDocumentImporter.cs#L70) | Coordinates source analysis, font dependencies, and runtime UI asset emission. | — | — |

### `Inno.UI.Assets.UiDocumentImportSettings`

<!-- api: T:Inno.UI.Assets.UiDocumentImportSettings de27a3cd1138d59fb21180497160aa9c6ec07a83cb5b8db41ee9bdaa2a1c9672 -->
<!-- api: P:Inno.UI.Assets.UiDocumentImportSettings.languageId 1b188288b0623c55b71a00983a26880891afa04fca647aeda46ed082de16e2b9 -->
<!-- api: P:Inno.UI.Assets.UiDocumentImportSettings.implementationId a6cb6e05ba6cb18343f34a2d62c16ccb0d49a469232df78459004541947cf027 -->
<!-- api: M:Inno.UI.Assets.UiDocumentImportSettings.#ctor f661a30b4d8a0ef2d94c24b17bf66156f9703a00bdee618dc300ee0611640615 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-70464a8255ac662a"></a>[`public Inno.UI.Assets.UiDocumentImportSettings.UiDocumentImportSettings()`](../../src/services/ui/Inno.UI.Assets/UiDocumentImportSettings.cs#L10) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-8f79c1d8728452cd"></a>[`public string Inno.UI.Assets.UiDocumentImportSettings.implementationId { get; set; }`](../../src/services/ui/Inno.UI.Assets/UiDocumentImportSettings.cs#L19) | Gets or sets the exact runtime implementation identity. | — | — |
| <a id="api-05e974978f040675"></a>[`public string Inno.UI.Assets.UiDocumentImportSettings.languageId { get; set; }`](../../src/services/ui/Inno.UI.Assets/UiDocumentImportSettings.cs#L15) | Gets or sets the registered source-language identity. | — | — |
| <a id="api-8f2ffca915997fe8"></a>[`public sealed class Inno.UI.Assets.UiDocumentImportSettings : Inno.Core.Serialization.ISerializable`](../../src/services/ui/Inno.UI.Assets/UiDocumentImportSettings.cs#L10) | Explicitly selects language and runtime implementation for neutral UI source. | — | — |

### `Inno.UI.Assets.UiDocumentSourceFile`

<!-- api: T:Inno.UI.Assets.UiDocumentSourceFile f2383280f1dda31e18ca77119fda765d2488ab39ec2ae6633c838c392734b6f5 -->
<!-- api: M:Inno.UI.Assets.UiDocumentSourceFile.#ctor(System.String,System.String,System.Func{Inno.Assets.AssetPath,System.String}) 2213ad37c20073cd8fc21b25044af1a2aae427f3bb146ba6a95b79b342b09db1 -->
<!-- api: P:Inno.UI.Assets.UiDocumentSourceFile.assetPath 96274507ecb17a662eac82606f1cdf5b99fe88b0d47b5fcc3ace745206f13212 -->
<!-- api: P:Inno.UI.Assets.UiDocumentSourceFile.text aca47049ef1f2dcd1958bd9b678a9850a348ac292eb3aae49963fab55d2678d0 -->
<!-- api: P:Inno.UI.Assets.UiDocumentSourceFile.readSource 6a1f88d147615246fd233a9b81f92a35af2caf8a327c3586e886e8aef8b1463e -->
<!-- api: M:Inno.UI.Assets.UiDocumentSourceFile.ToString 6e79f5b913165cb2b4e5386a558dd7d478f82ac9f95584c09ce11f23a8baf9bb -->
<!-- api: M:Inno.UI.Assets.UiDocumentSourceFile.op_Inequality(Inno.UI.Assets.UiDocumentSourceFile,Inno.UI.Assets.UiDocumentSourceFile) df55ed9253c142b2920deaac91bc7771038cf5af05dd14875522a8bf779ebe71 -->
<!-- api: M:Inno.UI.Assets.UiDocumentSourceFile.op_Equality(Inno.UI.Assets.UiDocumentSourceFile,Inno.UI.Assets.UiDocumentSourceFile) 1c957ae55dd1b4b36141b369b033ee673ff1d6b17e001bdb1b1029625580d091 -->
<!-- api: M:Inno.UI.Assets.UiDocumentSourceFile.GetHashCode 4d728ec991cc2568239400e4b05d98878190ebd4b38438405b0c974d4ca01a73 -->
<!-- api: M:Inno.UI.Assets.UiDocumentSourceFile.Equals(System.Object) 84a1a2f167f29119a3267cd47f47430ce35458011c69a7627064b9ef93809cf4 -->
<!-- api: M:Inno.UI.Assets.UiDocumentSourceFile.Equals(Inno.UI.Assets.UiDocumentSourceFile) b7840f291eee0b28600b25fc17ab4bff34d8ebc007a00dff56b404101da2424a -->
<!-- api: M:Inno.UI.Assets.UiDocumentSourceFile.Deconstruct(System.String@,System.String@,System.Func{Inno.Assets.AssetPath,System.String}@) 019728f9de6cad94bf8c44708b35dada5352db9838b4b1f3529ca25032479edb -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-927754b5c2e8d43a"></a>[`public Inno.UI.Assets.UiDocumentSourceFile.UiDocumentSourceFile(string assetPath, string text, System.Func<Inno.Assets.AssetPath, string>? readSource = null)`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L22) | Contains immutable source text supplied to a UI language frontend.<br>`assetPath`：Original asset mount path.<br>`text`：Strictly decoded UTF-8 text.<br>`readSource`：Reads and records an optional isolated source dependency. | — | — |
| <a id="api-f78ff940ad5c2551"></a>[`public void Inno.UI.Assets.UiDocumentSourceFile.Deconstruct(out string assetPath, out string text, out System.Func<Inno.Assets.AssetPath, string>? readSource)`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L22) | C# record 生成的解构入口，按主构造声明顺序返回位置成员。 | — | — |
| <a id="api-cbe6599870fa361e"></a>[`public bool Inno.UI.Assets.UiDocumentSourceFile.Equals(Inno.UI.Assets.UiDocumentSourceFile? other)`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L22) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-2187dadc1b15a4f2"></a>[`public override bool Inno.UI.Assets.UiDocumentSourceFile.Equals(object? obj)`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L22) | Determines whether the specified object is equal to the current object.<br>返回：true if the specified object is equal to the current object; otherwise, false.<br>`obj`：The object to compare with the current object. | — | — |
| <a id="api-ec5cb0f34a1ba69f"></a>[`public override int Inno.UI.Assets.UiDocumentSourceFile.GetHashCode()`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L22) | Serves as the default hash function.<br>返回：A hash code for the current object. | — | — |
| <a id="api-131e925e222773e4"></a>[`public static bool Inno.UI.Assets.UiDocumentSourceFile.operator ==(Inno.UI.Assets.UiDocumentSourceFile? left, Inno.UI.Assets.UiDocumentSourceFile? right)`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L22) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-e5b583bc411642a8"></a>[`public static bool Inno.UI.Assets.UiDocumentSourceFile.operator !=(Inno.UI.Assets.UiDocumentSourceFile? left, Inno.UI.Assets.UiDocumentSourceFile? right)`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L22) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-27cb5233f2602fa0"></a>[`public override string Inno.UI.Assets.UiDocumentSourceFile.ToString()`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L22) | Returns a string that represents the current object.<br>返回：A string that represents the current object. | — | — |
| <a id="api-71ca497a650373c8"></a>[`public string Inno.UI.Assets.UiDocumentSourceFile.assetPath { get; init; }`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L23) | Original asset mount path. | — | — |
| <a id="api-cb77609cac7e32d9"></a>[`public System.Func<Inno.Assets.AssetPath, string>? Inno.UI.Assets.UiDocumentSourceFile.readSource { get; init; }`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L25) | Reads and records an optional isolated source dependency. | — | — |
| <a id="api-1f9fdc91d8bfaeab"></a>[`public string Inno.UI.Assets.UiDocumentSourceFile.text { get; init; }`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L24) | Strictly decoded UTF-8 text. | — | — |
| <a id="api-ad728b8164c50f97"></a>[`public sealed record Inno.UI.Assets.UiDocumentSourceFile : System.IEquatable<Inno.UI.Assets.UiDocumentSourceFile>`](../../src/services/ui/Inno.UI.Assets/UiDocumentFrontend.cs#L22) | Contains immutable source text supplied to a UI language frontend.<br>`assetPath`：Original asset mount path.<br>`text`：Strictly decoded UTF-8 text.<br>`readSource`：Reads and records an optional isolated source dependency. | — | — |

## 项目依赖

- [Inno.UI](Inno.UI.md)：公开引用边界由实际签名核对。
- [Inno.Assets.Pipeline](../assets/Inno.Assets.Pipeline.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
