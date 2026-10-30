# Inno.Editor.Assets.ImGui

[Editor 索引](README.md) · [Wiki 首页](../README.md) · [UI 架构](../architecture/EDITOR_UI_ARCHITECTURE.md)

## 职责与边界

Asset 浏览呈现与图标发现。依赖 Assets 编辑 feature、Inspection、Settings 和 ImGui；File Browser 消费此呈现模块。编辑操作、文件归档及 History 不属于此程序集。

## 依赖与初始化

产品通过 `EditorProduct.props` 显式部署此程序集。由当前 TypeRegistry/Editor Registry 发现声明并在候选成功后切换；不扫描目录、不建立独立注册表或全局状态。

标准产品组合 Workbench 的默认外观设置贡献。嵌入 Host 也必须组合所需的图标设置贡献；缺少 `Editor/Appearance/Icons/Folder` 等必需设置时明确失败，不从另一个 owner 隐式补齐配置。

## API 与扩展点

AssetBrowserState、AssetBrowserRoot、AssetIconAttribute 和 AssetIconRegistry 提供当前浏览位置、来源、图标声明与查询。AssetBrowserModule 组合图标及无界面的 AssetEditingModule；BasicAssetInspectionDrawer 只做显示。

`AssetBrowserStyle(EditorStyleMetrics)` 在一次绘制前冻结当前用户 Zoom，不保留 Session，也不再次乘上 DPI。默认值使用 100% Zoom。它包含 Browser 图标、网格、面包屑及列比例；通用 pane/splitter/toolbar 指标仍由共享 metrics 提供。尺度在帧边界更新后，调用方重新取得值快照。

## 工作流

使用逻辑 namespace `InnoEditor.Assets` 声明 AssetIconAttribute；修改源内容时调用 Assets feature，不能直接写安装包。

## 错误、生命周期与热重载

控件只在当前 EditorUiSession 的 owner thread、作用域与原生上下文中使用。跨帧只保留稳定 ID 和中立状态；Missing、只读和未赋值状态保持区别。手势失败保留 History 所有权和诊断；扩展在所属 generation 退休，不保留插件 Type、delegate 或实例。

所有公开签名的当前映射见本页 API 清单；内部实现不构成稳定扩展契约。

## 当前源码公开 API 清单

此清单对应当前 SDK 求值与源码符号；内部类型不构成公开 API。

<!-- api: T:Inno.Editor.Assets.ImGui.AssetIconAttribute b66a9127807a6e0d561ac325c01b415937bb7748b0825d4bc194a4e4990b8fde -->
<!-- api: M:Inno.Editor.Assets.ImGui.AssetIconAttribute.#ctor(System.Type,System.String,System.Boolean,System.Int32) cd9aa19285c639fbccbdd41ac0295d460d8eac1be0b04c7f44f49b40c03264fb -->
<!-- api: M:Inno.Editor.Assets.ImGui.AssetIconAttribute.#ctor(System.String,System.String,System.Int32) eec6dd6a5f1105610c55b4118c452e8ac49280688489586040bb3c66a566e021 -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetIconAttribute.assetType f817a57ab160dcc7cb20a4b058a87ec2dfb73d8642d9754bb4838e49ab6365cd -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetIconAttribute.extension e737313d90f6f0c242820a18b96bf76d7b9e2ae3cfa2a762813aef387c15b9d3 -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetIconAttribute.icon 7d5797a61ada6ab5a8428e56f4ebe2b6d11d2f286126138b1f4735f420332380 -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetIconAttribute.useForChildren 54908b93f505f9e225988dbb23e3d00666c7709405c577c279e0c39c1a4d78a4 -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetIconAttribute.priority f689f55ac7437a19026858599ad0e115ac24a14188ddd27b76d0e021ceaa2f90 -->
<!-- api: T:Inno.Editor.Assets.ImGui.AssetBrowserModule e9d7841d2b1a3ac5f342bbce8ada1901aa47f568dff5637ecbe6deaec93c4f32 -->
<!-- api: M:Inno.Editor.Assets.ImGui.AssetBrowserModule.#ctor(Inno.Editor.Assets.AssetEditingModule,Inno.Editor.Interactions.EditorInteractions,Inno.Editor.Settings.EditorSettings,Inno.Extensibility.Types.TypeCatalog) 8a5ade5a916afc7a2bc04d5df93f179eda4731a1bf14829a4d77f839800fa7be -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetBrowserModule.edits d7d1d25fccf702e0950ddefa87dd4110834768cd134746a6fb5a1dca13c320a5 -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetBrowserModule.browser 7d2f8d69bb244626343b6b4a265eaa48ed3dbbd62967ed497b81c7fdd2686db0 -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetBrowserModule.folderIcon 4baaf2ac4d1f74ee2f96c599f398ab5b0ec1e32517f3160844804d5025001c72 -->
<!-- api: M:Inno.Editor.Assets.ImGui.AssetBrowserModule.Capture(Inno.Editor.Core.EditorState) 10e05f0d1aee510812cae9f23ed5191b48ebb58222e231097c98a425054920ca -->
<!-- api: M:Inno.Editor.Assets.ImGui.AssetBrowserModule.Restore(Inno.Editor.Core.EditorState) 98122ed393084078029b7f215bc03a121110258a3e1e6522b8a44ff08119be34 -->
<!-- api: M:Inno.Editor.Assets.ImGui.AssetBrowserModule.GetIcon(Inno.Assets.Pipeline.AssetFileEntry) 385208982fd7f18d60085000527be95aee2b05dc8b00695f4544a7a7f8ffd597 -->
<!-- api: M:Inno.Editor.Assets.ImGui.AssetBrowserModule.OnDispose 7bc14a8a5138e3215090bc7be72fda9bd292125ed3544e37461c79b2d385c7f1 -->
<!-- api: T:Inno.Editor.Assets.ImGui.AssetBrowserRoot 538163c13e69c1a4ec1e03e3e284dad2523590c85321e79d3188040f58ee6b27 -->
<!-- api: F:Inno.Editor.Assets.ImGui.AssetBrowserRoot.Assets 86229f740944f550948c8d0de52f150743ff25806d386e3a8c3db394a1da944f -->
<!-- api: F:Inno.Editor.Assets.ImGui.AssetBrowserRoot.Plugins 520a0c7977a8024e68c5a8e3ea185d5ab4e4cb2549d0d9dc9930d1acd5422903 -->
<!-- api: T:Inno.Editor.Assets.ImGui.AssetBrowserState 440d7e3800e563470d9dd3cd6b1ff2aa69dc9840ad04f09ca73cb1a314b56057 -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetBrowserState.root 67ca816fd5ad845328d704260d632e63f66ac4deb4b37a7efea0145c9b1edd72 -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetBrowserState.currentDirectory 88cfbea34673940877e06a0a120d809558de5cfcf9e55f46d8c6f682cecf0604 -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetBrowserState.projectDirectory bc29a59bfc2003952d89ac1485a2820ab84def53828730d754e79a7077d26faf -->
<!-- api: M:Inno.Editor.Assets.ImGui.AssetBrowserState.GetSelectedPath(Inno.Editor.Core.EditorContext) 47b1a602ed028bd9d5d9712137ad52cac59fe9f61473b8429a522f82cf7e76fa -->
<!-- api: M:Inno.Editor.Assets.ImGui.AssetBrowserState.IsSelected(Inno.Editor.Core.EditorContext,Inno.Assets.Pipeline.AssetFileEntry) 5b0e724b04fdbbbe8a3893508fca37a36c228a233924da66facdd139cdf180a9 -->
<!-- api: M:Inno.Editor.Assets.ImGui.AssetBrowserState.ToggleSelection(Inno.Editor.Core.EditorContext,Inno.Assets.Pipeline.AssetFileEntry) 27a752697ee2026df4b7c165b6a7824e1bc313e22c1cdd75d403f5ff84a3075f -->
<!-- api: M:Inno.Editor.Assets.ImGui.AssetBrowserState.SetRoot(Inno.Editor.Assets.ImGui.AssetBrowserRoot) 204205eb57b29cea137e1ab7c7f7aedb1f01a1b9e7831565462e92101f401806 -->
<!-- api: M:Inno.Editor.Assets.ImGui.AssetBrowserState.SetCurrentDirectory(System.String) 5f2bd5c87f8b68c6a18dbaa0d194d2c7a6f712dccee89b68441387a1b4cecbd3 -->
<!-- api: M:Inno.Editor.Assets.ImGui.AssetBrowserState.Select(Inno.Editor.Core.EditorContext,System.String) 93116e16d53057741d1df770d65f727333e6e40ea4a613d9cf4ed3da5b339303 -->
<!-- api: M:Inno.Editor.Assets.ImGui.AssetBrowserState.GetDirectory(Inno.Editor.Assets.ImGui.AssetBrowserRoot) fe33ce780404a67377c77d7fa433d87ab291a812c865bc938c8813e16a4e1c77 -->
<!-- api: M:Inno.Editor.Assets.ImGui.AssetBrowserState.SetLocation(Inno.Editor.Assets.ImGui.AssetBrowserRoot,System.String) 2187f05b1dd429435c9767628cb17752c7ff94ec902f0217c55d11ca7759830a -->
<!-- api: T:Inno.Editor.Assets.ImGui.AssetBrowserStyle ad35c1deec2700fa10b3e8cafa9afd9cd297547b8fa88cabea5cfe11a201535c -->
<!-- api: M:Inno.Editor.Assets.ImGui.AssetBrowserStyle.#ctor(Inno.Editor.ImGui.EditorStyleMetrics) 18183ecaef92497ebd74e2ac4e31ad44e664ce84e3ca9a2578bc2630fbc4d529 -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetBrowserStyle.windowPadding 2411320024008f91de6c1f7cb60a7cfe4ec1dadbe6fb0dade37dc31fd567c1d1 -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetBrowserStyle.cellPadding 69f76c0d77c8ba95c993aa05110b249021752d11732eabf6ae2133d495ce2801 -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetBrowserStyle.itemSpacing 040d68d12f944ce3f52198652cc254120cf6eb9c2b1b5d940ede6698e52a1dec -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetBrowserStyle.frameRounding 0e9373d34401d94d885b833df43b6f4905340476324834399921c9e9a14f3ee4 -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetBrowserStyle.breadcrumbHeight cccb7f2cf62d7071d6914744b783389528d810f2fb70c68fea75e0f4d8d01931 -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetBrowserStyle.gridMinimumCellSize e7c11984710ff279827869aa54ccdab1e46be040e7837ae0894e6da49519e862 -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetBrowserStyle.gridCellPadding 339f6d829d645475913e9a9625ffa5300707c9a6199304fe540d22bfc8b6d317 -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetBrowserStyle.gridDefaultScale 03f6620ac7ed31869db169526c373fc4a23a3092748b1ab5cb34a3ebd25063bc -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetBrowserStyle.gridMinimumScale b92c5339fe877a9e3bc76aca2363a68151ae8c18340978f4d9eec0ed38ab4c82 -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetBrowserStyle.gridMaximumScale 9bc49e7142d9584bc515e2f4485e5a21ea67a76242fa038c0aa64bc5d4b2ce5d -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetBrowserStyle.listRowSpacing 0cfd5ba9f76ffad163b38ef0c2093b6611bdc9404c839dc0d257bb17603dc95e -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetBrowserStyle.listNameSeparatorPosition 85eb8bbcd41d202116923860f6773307194ab71b89a9996e652c935c143ca56c -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetBrowserStyle.listTypeSeparatorPosition 66e94bde6c7ff8449a0ecd835ae85e81344260c1da40524c0aaf97eddfcc4193 -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetBrowserStyle.listMinimumColumnRatio 11f783c33a5a3dd246d14bc16449606d573cdedca58c1a4503f405c69fac38b2 -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetBrowserStyle.listSeparatorHitWidth c88aa4c2f3c50040ec87604a749dd77eff4c88bae5e1db540059cd06d7dae9f7 -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetBrowserStyle.listContentHorizontalPadding 57ea8ab3e8e4f37bea62e5bd81fbf6cbf40012f42da3be441e815c5c97494de6 -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetBrowserStyle.gridLabelHorizontalPadding 3e3e3a7ec54db54e963ab590ecd337a4069027b2ef36b452fd217ff07da7331f -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetBrowserStyle.gridLabelBottomPadding d1d3d5a23fd3f98e54cc0f07be61340ba9a4b13fff413730af5bfd7ec8310eb9 -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetBrowserStyle.gridLabelLineSpacing 49243f4b97a5a98e0a50bb16c461ece97e31896b1e2865df37e6c8830a92c8c6 -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetBrowserStyle.gridFixedCellPadding 829046eb03c9721e5033480ba850eac37c82c227367e884cd4e68b542ad43fc1 -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetBrowserStyle.gridScaleBias 4068bfeb70877a20f64a6ffb4b89a3227f7186edc5173e55149f7e95296f3609 -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetBrowserStyle.gridIconTopPadding b26135d2706c6ae465b9549fb5d46047d928126d546df1874c5bffd88b8a850e -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetBrowserStyle.gridIconHorizontalPadding a00b73aba21f45cfd1d92e14c7bcb4ec781f9252fbbc19c8c8829fad9889be6a -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetBrowserStyle.gridIconLabelSpacing 64d4b63243a541ac9263a304ea877d29bf6d9c47e6505ed5a0210fe970ea53ac -->
<!-- api: P:Inno.Editor.Assets.ImGui.AssetBrowserStyle.breadcrumbSpacing 6e0e2dc6c18c33a081c3c615480d7f512072d999d4101216a74c36d070cfca9e -->

| API 与源码 | 契约与参数 | 返回值 | 重要异常 |
| --- | --- | --- | --- |
| <a id="api-e3a99d4a24230396"></a>[`public sealed class Inno.Editor.Assets.ImGui.AssetIconAttribute : System.Attribute`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Icons/AssetIconAttribute.cs#L14) | Declares the icon used by the Asset Browser for an imported asset type or source extension. | — | — |
| <a id="api-48df7cec94bcb825"></a>[`public Inno.Editor.Assets.ImGui.AssetIconAttribute.AssetIconAttribute(System.Type assetType, string icon, bool useForChildren = false, int priority = 0)`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Icons/AssetIconAttribute.cs#L37) | Creates an icon declaration using a glyph from the Editor icon catalog.<br>`assetType`: The imported asset type represented by the icon.<br>`icon`: The Editor icon glyph to render.<br>`useForChildren`: Whether the declaration may also represent derived asset types.<br>`priority`: The tie-breaking priority after exactness and inheritance distance. | — | `System.ArgumentNullException`: Thrown when is .<br>`System.ArgumentException`: Thrown when is empty. |
| <a id="api-8a39fdebdbc959c7"></a>[`public Inno.Editor.Assets.ImGui.AssetIconAttribute.AssetIconAttribute(string extension, string icon, int priority = 0)`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Icons/AssetIconAttribute.cs#L65) | Creates an icon declaration for files ending with a source extension.<br>`extension`: The simple or compound file extension. A leading period is optional and matching is case-insensitive.<br>`icon`: The Editor icon glyph to render.<br>`priority`: The tie-breaking priority after extension specificity. | — | `System.ArgumentException`: Thrown when is empty or contains a path separator, or when is empty. |
| <a id="api-ee894bc1e05e7a66"></a>[`public System.Type? Inno.Editor.Assets.ImGui.AssetIconAttribute.assetType { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Icons/AssetIconAttribute.cs#L79) | Gets the imported asset type represented by this declaration, or for an extension declaration. | — | — |
| <a id="api-178d66c99ce90d46"></a>[`public string? Inno.Editor.Assets.ImGui.AssetIconAttribute.extension { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Icons/AssetIconAttribute.cs#L84) | Gets the normalized source extension represented by this declaration, or for a type declaration. | — | — |
| <a id="api-1ed250e0587e3ee0"></a>[`public string Inno.Editor.Assets.ImGui.AssetIconAttribute.icon { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Icons/AssetIconAttribute.cs#L88) | Gets the Editor icon glyph to render. | — | — |
| <a id="api-19fe75c85e4ef46a"></a>[`public bool Inno.Editor.Assets.ImGui.AssetIconAttribute.useForChildren { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Icons/AssetIconAttribute.cs#L93) | Gets whether a type declaration may also represent derived asset types. Extension declarations always return . | — | — |
| <a id="api-c7e80d97dd2af75b"></a>[`public int Inno.Editor.Assets.ImGui.AssetIconAttribute.priority { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Icons/AssetIconAttribute.cs#L97) | Gets the tie-breaking priority after target specificity. | — | — |
| <a id="api-c0fdaf5642bf05fd"></a>[`public sealed class Inno.Editor.Assets.ImGui.AssetBrowserModule : Inno.Editor.Core.EditorModule, Inno.Editor.Inspection.IInspectionIconProvider<Inno.Assets.Pipeline.AssetFileEntry>`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Navigation/AssetBrowserModule.cs#L17) | Owns asset navigation and icon presentation independently from source mutations. | — | — |
| <a id="api-4fb9acb7de696ec6"></a>[`public Inno.Editor.Assets.ImGui.AssetBrowserModule.AssetBrowserModule(Inno.Editor.Assets.AssetEditingModule edits, Inno.Editor.Interactions.EditorInteractions interactions, Inno.Editor.Settings.EditorSettings settings, Inno.Extensibility.Types.TypeCatalog types)`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Navigation/AssetBrowserModule.cs#L40) | Creates the asset presentation module for the current editor generation.<br>`edits`: The independent authoritative asset editing service.<br>`interactions`: The editor selection and action owner.<br>`settings`: The editor settings that supply semantic icon choices.<br>`types`: The active extension type catalog. | — | `System.ArgumentNullException`: A required owner is null. |
| <a id="api-781e8741b03d2a86"></a>[`public Inno.Editor.Assets.AssetEditingModule Inno.Editor.Assets.ImGui.AssetBrowserModule.edits { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Navigation/AssetBrowserModule.cs#L55) | Gets the source editing service; this module does not implement source transactions. | — | — |
| <a id="api-1cb6655414e50812"></a>[`public Inno.Editor.Assets.ImGui.AssetBrowserState Inno.Editor.Assets.ImGui.AssetBrowserModule.browser { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Navigation/AssetBrowserModule.cs#L59) | Gets navigation state; object selection belongs to the shared interaction owner. | — | — |
| <a id="api-a15c85ed4e869818"></a>[`public string Inno.Editor.Assets.ImGui.AssetBrowserModule.folderIcon { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Navigation/AssetBrowserModule.cs#L63) | Gets the configured folder glyph used by asset navigation. | — | — |
| <a id="api-586111599d0bdd85"></a>[`protected override void Inno.Editor.Assets.ImGui.AssetBrowserModule.Capture(Inno.Editor.Core.EditorState state)`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Navigation/AssetBrowserModule.cs#L71) | Captures an immutable snapshot of the current observable state.<br>`state`: The lifecycle or domain state applied by this operation. | — | — |
| <a id="api-e93b33a871c7cb3f"></a>[`protected override void Inno.Editor.Assets.ImGui.AssetBrowserModule.Restore(Inno.Editor.Core.EditorState state)`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Navigation/AssetBrowserModule.cs#L84) | Restores the supplied snapshot while preserving current invariants.<br>`state`: The lifecycle or domain state applied by this operation. | — | — |
| <a id="api-678dfa6abdd00e4a"></a>[`public string Inno.Editor.Assets.ImGui.AssetBrowserModule.GetIcon(Inno.Assets.Pipeline.AssetFileEntry entry)`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Navigation/AssetBrowserModule.cs#L127) | Resolves the presentation icon registered for an asset type or source extension.<br>`entry`: The source entry whose presentation icon should be resolved. | The most specific registered icon, or the built-in directory or file icon when no registration matches the entry. | `System.ArgumentNullException`: Thrown when is . |
| <a id="api-e4e20ac82d314170"></a>[`protected override void Inno.Editor.Assets.ImGui.AssetBrowserModule.OnDispose()`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Navigation/AssetBrowserModule.cs#L142) | Releases resources owned by this module after it has stopped and left the active extension generation. | — | — |
| <a id="api-4060d5db1ea38275"></a>[`public enum Inno.Editor.Assets.ImGui.AssetBrowserRoot : int`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Navigation/AssetBrowserState.cs#L14) | Identifies the authoring or installed-content root displayed by the Asset Browser. | — | — |
| <a id="api-3fcc8f6c568aa259"></a>[`Inno.Editor.Assets.ImGui.AssetBrowserRoot.Assets = 0`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Navigation/AssetBrowserState.cs#L19) | The writable project Assets authoring root. | — | — |
| <a id="api-edde93decfaa0387"></a>[`Inno.Editor.Assets.ImGui.AssetBrowserRoot.Plugins = 1`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Navigation/AssetBrowserState.cs#L23) | The read-only Plugins installation root. | — | — |
| <a id="api-2a60359cfae94b02"></a>[`public sealed class Inno.Editor.Assets.ImGui.AssetBrowserState`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Navigation/AssetBrowserState.cs#L29) | Stores asset browser navigation independently from global object selection. | — | — |
| <a id="api-8904e193dcd7d6ba"></a>[`public Inno.Editor.Assets.ImGui.AssetBrowserRoot Inno.Editor.Assets.ImGui.AssetBrowserState.root { get; private set; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Navigation/AssetBrowserState.cs#L58) | Gets the root currently displayed by the Asset Browser. | — | — |
| <a id="api-4952f44f382eb1de"></a>[`public string Inno.Editor.Assets.ImGui.AssetBrowserState.currentDirectory { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Navigation/AssetBrowserState.cs#L62) | Gets the current directory inside . An empty value identifies that root's overview. | — | — |
| <a id="api-31c9cb0df90ddae1"></a>[`public string Inno.Editor.Assets.ImGui.AssetBrowserState.projectDirectory { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Navigation/AssetBrowserState.cs#L66) | Gets the most recently visited writable project directory, independently of the displayed root. | — | — |
| <a id="api-9b57c171f5664143"></a>[`public string? Inno.Editor.Assets.ImGui.AssetBrowserState.GetSelectedPath(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Navigation/AssetBrowserState.cs#L80) | Gets the selected asset path when the editor-wide target belongs to the Asset Browser.<br>`context`: The shared editor context containing the global selection. | The normalized source-relative path, or when another target type is selected. | `System.ArgumentNullException`: Thrown when is . |
| <a id="api-e45025c2c251d975"></a>[`public bool Inno.Editor.Assets.ImGui.AssetBrowserState.IsSelected(Inno.Editor.Core.EditorContext context, Inno.Assets.Pipeline.AssetFileEntry entry)`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Navigation/AssetBrowserState.cs#L98) | Checks whether the source entry belongs to the current asset selection.<br>`context`: The current editor callback context.<br>`entry`: The indexed source identity to inspect. | True when the entry is selected; otherwise false. | — |
| <a id="api-1b94e68bde7f4db1"></a>[`public void Inno.Editor.Assets.ImGui.AssetBrowserState.ToggleSelection(Inno.Editor.Core.EditorContext context, Inno.Assets.Pipeline.AssetFileEntry entry)`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Navigation/AssetBrowserState.cs#L111) | Adds or removes an asset from the current selection without retaining runtime asset objects.<br>`context`: The current editor callback context.<br>`entry`: The indexed source identity to toggle. | — | — |
| <a id="api-bc1e85d0da2c7ec0"></a>[`public void Inno.Editor.Assets.ImGui.AssetBrowserState.SetRoot(Inno.Editor.Assets.ImGui.AssetBrowserRoot value)`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Navigation/AssetBrowserState.cs#L124) | Switches the displayed root while preserving the last directory visited in each root.<br>`value`: The authoring or installed-content root to display. | — | — |
| <a id="api-1eb1b769761ce36f"></a>[`public void Inno.Editor.Assets.ImGui.AssetBrowserState.SetCurrentDirectory(string relativePath)`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Navigation/AssetBrowserState.cs#L131) | Sets the current Asset Browser directory and infers its root from the isolated source identity.<br>`relativePath`: The isolated source-relative directory path, or an empty value for the currently displayed root. | — | — |
| <a id="api-955795c85ddae5e7"></a>[`public void Inno.Editor.Assets.ImGui.AssetBrowserState.Select(Inno.Editor.Core.EditorContext context, string? relativePath)`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Navigation/AssetBrowserState.cs#L158) | Selects an asset path through the editor-wide selection state, or clears Asset selection.<br>`context`: The shared editor context whose selection should be updated.<br>`relativePath`: The source-relative path to select, or an empty value to clear selection. | — | `System.ArgumentNullException`: Thrown when is . |
| <a id="api-ec79f3e8dc4a3102"></a>[`public string Inno.Editor.Assets.ImGui.AssetBrowserState.GetDirectory(Inno.Editor.Assets.ImGui.AssetBrowserRoot targetRoot)`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Navigation/AssetBrowserState.cs#L184) | Gets the last visited isolated directory for a navigation root.<br>`targetRoot`: The root whose independent navigation position is requested. | The source-relative directory, or an empty string for the root. | — |
| <a id="api-2470b55f6a175f5f"></a>[`public void Inno.Editor.Assets.ImGui.AssetBrowserState.SetLocation(Inno.Editor.Assets.ImGui.AssetBrowserRoot targetRoot, string directory)`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Navigation/AssetBrowserState.cs#L207) | Changes the displayed source root and its isolated navigation directory.<br>`targetRoot`: The navigation root that owns the requested directory.<br>`directory`: The isolated source-relative directory; empty selects the root. | — | `System.ArgumentException`: The directory belongs to a different source root. |
| <a id="api-7c2c97482f9d641d"></a>[`public readonly struct Inno.Editor.Assets.ImGui.AssetBrowserStyle`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Styling/AssetBrowserStyle.cs#L11) | Freezes Browser-specific layout values from the shared UI scale without retaining a session. The default value uses the standard 100 percent scale. | — | — |
| <a id="api-4b45cd9e30a1b949"></a>[`public Inno.Editor.Assets.ImGui.AssetBrowserStyle.AssetBrowserStyle(Inno.Editor.ImGui.EditorStyleMetrics metrics)`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Styling/AssetBrowserStyle.cs#L24) | Captures the current shared scale for one Browser presentation pass.<br>`metrics`: The existing session metrics; later mutations do not change this value. | — | `System.ArgumentNullException`: The metrics are null. |
| <a id="api-c83dd929ea4e1f6c"></a>[`public System.Numerics.Vector2 Inno.Editor.Assets.ImGui.AssetBrowserStyle.windowPadding { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Styling/AssetBrowserStyle.cs#L33) | Gets asset browser window padding. | — | — |
| <a id="api-a8997555e27c1570"></a>[`public System.Numerics.Vector2 Inno.Editor.Assets.ImGui.AssetBrowserStyle.cellPadding { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Styling/AssetBrowserStyle.cs#L38) | Gets asset browser cell padding. | — | — |
| <a id="api-b15eca00e4b4cba1"></a>[`public System.Numerics.Vector2 Inno.Editor.Assets.ImGui.AssetBrowserStyle.itemSpacing { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Styling/AssetBrowserStyle.cs#L43) | Gets asset browser item spacing. | — | — |
| <a id="api-da3bdd57282cde47"></a>[`public float Inno.Editor.Assets.ImGui.AssetBrowserStyle.frameRounding { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Styling/AssetBrowserStyle.cs#L48) | Gets asset browser frame rounding. | — | — |
| <a id="api-e17c1f1a647b86b8"></a>[`public float Inno.Editor.Assets.ImGui.AssetBrowserStyle.breadcrumbHeight { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Styling/AssetBrowserStyle.cs#L53) | Gets asset breadcrumb bar height. | — | — |
| <a id="api-45ba9450ad9beff8"></a>[`public float Inno.Editor.Assets.ImGui.AssetBrowserStyle.gridMinimumCellSize { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Styling/AssetBrowserStyle.cs#L58) | Gets minimum asset grid cell size. | — | — |
| <a id="api-9dc97a7db078f5e0"></a>[`public float Inno.Editor.Assets.ImGui.AssetBrowserStyle.gridCellPadding { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Styling/AssetBrowserStyle.cs#L63) | Gets asset grid cell padding. | — | — |
| <a id="api-be2bae0d68dae772"></a>[`public float Inno.Editor.Assets.ImGui.AssetBrowserStyle.gridDefaultScale { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Styling/AssetBrowserStyle.cs#L68) | Gets default asset grid scale. | — | — |
| <a id="api-b94d6096c5a72a37"></a>[`public float Inno.Editor.Assets.ImGui.AssetBrowserStyle.gridMinimumScale { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Styling/AssetBrowserStyle.cs#L73) | Gets minimum asset grid scale. | — | — |
| <a id="api-ca9048cb7b470b24"></a>[`public float Inno.Editor.Assets.ImGui.AssetBrowserStyle.gridMaximumScale { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Styling/AssetBrowserStyle.cs#L78) | Gets maximum asset grid scale. | — | — |
| <a id="api-bd655abef53aa1e1"></a>[`public float Inno.Editor.Assets.ImGui.AssetBrowserStyle.listRowSpacing { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Styling/AssetBrowserStyle.cs#L83) | Gets spacing between asset list rows. | — | — |
| <a id="api-3f9832ce53d89a36"></a>[`public float Inno.Editor.Assets.ImGui.AssetBrowserStyle.listNameSeparatorPosition { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Styling/AssetBrowserStyle.cs#L88) | Gets the default normalized position of the asset list name/type separator. | — | — |
| <a id="api-7a9c3e8b6a3e5795"></a>[`public float Inno.Editor.Assets.ImGui.AssetBrowserStyle.listTypeSeparatorPosition { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Styling/AssetBrowserStyle.cs#L93) | Gets the default normalized position of the asset list type/source separator. | — | — |
| <a id="api-cc2c6ad68f40d08a"></a>[`public float Inno.Editor.Assets.ImGui.AssetBrowserStyle.listMinimumColumnRatio { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Styling/AssetBrowserStyle.cs#L98) | Gets the minimum normalized width reserved for each asset list column. | — | — |
| <a id="api-ea34de16ea901e26"></a>[`public float Inno.Editor.Assets.ImGui.AssetBrowserStyle.listSeparatorHitWidth { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Styling/AssetBrowserStyle.cs#L103) | Gets the horizontal hit width of an asset list column separator. | — | — |
| <a id="api-c6844b83c01b9213"></a>[`public float Inno.Editor.Assets.ImGui.AssetBrowserStyle.listContentHorizontalPadding { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Styling/AssetBrowserStyle.cs#L108) | Gets the horizontal inset between an asset list separator and column content. | — | — |
| <a id="api-ed5336845fa404df"></a>[`public float Inno.Editor.Assets.ImGui.AssetBrowserStyle.gridLabelHorizontalPadding { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Styling/AssetBrowserStyle.cs#L113) | Gets horizontal padding for asset grid labels. | — | — |
| <a id="api-eae95d079958c257"></a>[`public float Inno.Editor.Assets.ImGui.AssetBrowserStyle.gridLabelBottomPadding { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Styling/AssetBrowserStyle.cs#L118) | Gets bottom padding for asset grid labels. | — | — |
| <a id="api-914d7f1ae2c3de9a"></a>[`public float Inno.Editor.Assets.ImGui.AssetBrowserStyle.gridLabelLineSpacing { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Styling/AssetBrowserStyle.cs#L123) | Gets the additional vertical spacing between asset grid label lines. | — | — |
| <a id="api-28abd781b2787173"></a>[`public float Inno.Editor.Assets.ImGui.AssetBrowserStyle.gridFixedCellPadding { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Styling/AssetBrowserStyle.cs#L128) | Gets fixed padding added to calculated asset grid cells. | — | — |
| <a id="api-a92b0cc639d93763"></a>[`public float Inno.Editor.Assets.ImGui.AssetBrowserStyle.gridScaleBias { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Styling/AssetBrowserStyle.cs#L133) | Gets the scale bias added to asset grid icons. | — | — |
| <a id="api-07fa7acba1dd0727"></a>[`public float Inno.Editor.Assets.ImGui.AssetBrowserStyle.gridIconTopPadding { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Styling/AssetBrowserStyle.cs#L138) | Gets the top inset reserved above an asset grid icon. | — | — |
| <a id="api-d65142fc81ddf8b1"></a>[`public float Inno.Editor.Assets.ImGui.AssetBrowserStyle.gridIconHorizontalPadding { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Styling/AssetBrowserStyle.cs#L143) | Gets the horizontal inset that constrains an asset grid icon inside its card. | — | — |
| <a id="api-d312e513a6725382"></a>[`public float Inno.Editor.Assets.ImGui.AssetBrowserStyle.gridIconLabelSpacing { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Styling/AssetBrowserStyle.cs#L148) | Gets the vertical spacing between an asset grid icon and its label. | — | — |
| <a id="api-82f070daf8ed4fe2"></a>[`public float Inno.Editor.Assets.ImGui.AssetBrowserStyle.breadcrumbSpacing { get; }`](../../src/composition/editor/presentation/Inno.Editor.Assets.ImGui/Styling/AssetBrowserStyle.cs#L153) | Gets spacing around breadcrumb separators. | — | — |

## 项目依赖

| 工程 | 边界 |
| --- | --- |
| `Inno.Scripting.Api` | 实现依赖；不传递 compile surface |
| `Inno.Editor.ImGui` | 公开签名依赖 |
| `Inno.Assets.Pipeline` | 公开签名依赖 |
| `Inno.Core.Serialization` | 公开签名依赖 |
| `Inno.Extensibility.Catalogs` | 公开签名依赖 |
| `Inno.Extensibility.Types` | 公开签名依赖 |
| `Inno.Editor.Assets` | 公开签名依赖 |
| `Inno.Editor.Core` | 公开签名依赖 |
| `Inno.Editor.Inspection` | 公开签名依赖 |
| `Inno.Editor.Interactions` | 公开签名依赖 |
| `Inno.Editor.Settings` | 公开签名依赖 |
