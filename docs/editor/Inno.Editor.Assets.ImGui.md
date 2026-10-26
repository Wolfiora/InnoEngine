# Inno.Editor.Assets.ImGui

[Editor 索引](README.md) · [Wiki 首页](../README.md) · [UI 架构](../architecture/EDITOR_UI_ARCHITECTURE.md)

## 职责与边界

Asset 浏览呈现与图标发现。依赖 Assets 编辑 feature、Inspection、Settings 和 ImGui；File Browser 消费此呈现模块。编辑操作、文件归档及 History 不属于此程序集。

## 依赖与初始化

产品通过 `EditorProduct.props` 显式部署此程序集。由当前 TypeRegistry/Editor Registry 发现声明并在候选成功后切换；不扫描目录、不建立独立注册表或全局状态。

## API 与扩展点

AssetBrowserState、AssetBrowserRoot、AssetIconAttribute 和 AssetIconRegistry 提供当前浏览位置、来源、图标声明与查询。AssetBrowserModule 组合图标及无界面的 AssetEditingModule；BasicAssetInspectionDrawer 只做显示。

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

## 项目依赖

| 工程 | 边界 |
| --- | --- |
| `Inno.Scripting.Api` | 实现依赖；不传递 compile surface |
| `Inno.Editor.ImGui` | 实现依赖；不传递 compile surface |
| `Inno.Assets.Pipeline` | 公开签名依赖 |
| `Inno.Core.Serialization` | 公开签名依赖 |
| `Inno.Extensibility.Catalogs` | 公开签名依赖 |
| `Inno.Extensibility.Types` | 公开签名依赖 |
| `Inno.Editor.Assets` | 公开签名依赖 |
| `Inno.Editor.Core` | 公开签名依赖 |
| `Inno.Editor.Inspection` | 公开签名依赖 |
| `Inno.Editor.Interactions` | 公开签名依赖 |
| `Inno.Editor.Settings` | 公开签名依赖 |
