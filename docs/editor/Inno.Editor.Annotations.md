# Inno.Editor.Annotations

[Editor 索引](README.md) · [Inspection](Inno.Editor.Inspection.md) · [Wiki 首页](../README.md)

## 职责与依赖

独立、无状态的 Inspector 创作标注契约。它使用 System.Attribute，并引用 Catalogs 的程序集身份和 Scripting.Api 的导出声明，不引用 Serialization、Scene、Rendering、ImGui 或 Panel。Inspection 负责解释及渲染；Core.Serialization 只负责数据。

程序集加载范围为 `AssemblyScope.Runtime`，允许创作态游戏模块保留这些纯元数据。脚本导出范围为 `ScriptingApiScope.Authoring`，表示 Player 编译会擦除它们。这两个范围分别管理加载依赖与发布语义；注解不提供 Editor 行为，也不允许游戏脚本引用 Editor Host、Panel 或绘制器。

脚本入口为 `using InnoEditor.Annotations;`，引擎 Editor 扩展使用 `Inno.Editor.Annotations`。无需初始化；所有类型以 `ScriptingApiScope.Authoring` 导出。创作编译保留 `INNO_EDITOR` metadata；Player 编译在绑定目标平台 DLL 前移除标注、自定义派生标注声明与相关 using，最终无该程序集引用。

## 公开 API

所有标注继承 `InspectorPresentationAttribute`，共有可写 `order` 排序属性。

| 类型/构造 | 属性与语义 |
| --- | --- |
| `Header(string title, string description = "")` | `title` 分组标题，`description` 仅悬停可见。 |
| `Text(string text)` | `text` 常驻中性说明。 |
| `Space(float height = 8)` | `height` 逻辑像素间距。 |
| `Tooltip(string text)` | `text` 属性行悬停说明。 |
| `InspectorName(string name)` | `name` 显示名称，不改变持久键。 |
| `Range(double minimum, double maximum)` | 数值编辑范围，不改变运行时值校验。 |
| `InspectorReadOnly()` | 禁用字段编辑。 |
| `ShowIf` / `HideIf` | `(string memberName)`、`(string memberName, object expectedValue)`、`(string memberName, InspectorCondition condition)`；公开 `memberName`、`condition`、`expectedValue`。 |
| `HelpBox` | `text`、`messageType`、`conditionMember`、`condition`、`expectedValue`；支持无条件、条件谓词或条件相等值构造。 |
| `InspectorCondition` | Truthy/Falsy、Equal/NotEqual、Null/NotNull、Assigned/NotAssigned。 |
| `InspectorMessageType` | Info、Warning、Error。 |

HelpBox 是带状态的提示卡片；Text 是普通说明；Tooltip 不占常驻布局空间。相同语义用于脚本与原生 Inspector。

## 扩展工作流

```csharp
using InnoEngine.Scene;
using InnoEngine.Serialization;
using InnoEditor.Annotations;

public sealed class Mover : GameBehavior
{
    [SerializableProperty]
    [Header("Movement", "World units per second.")]
    [Tooltip("Maximum movement speed.")]
    [Range(0, 30)]
    public float speed { get; set; } = 6;
}
```

新标注派生 `InspectorPresentationAttribute`；对应 EditorScripts 使用 `InspectorAttributeDrawer` 注册，具体接口见 [Inspection](Inno.Editor.Inspection.md)。标注只保存标量/字符串/枚举，不保存运行时服务或对象。没有需重写的 protected 生命周期方法。绘制器由 TypeCache generation 重建，不应静态缓存 collectible 类型。

构造函数拒绝空标题、成员名及无效范围/间距；非法条件会在 Inspector 元数据解析时给出诊断。标注不自动令字段可序列化，仍需显式 `SerializableProperty`。

## 当前源码公开 API 清单

此清单对应当前 SDK 求值与源码符号；内部类型不构成公开 API。

<!-- api: T:Inno.Editor.Annotations.InspectorPresentationAttribute 5c01b7ea7bae3a248a014eaff5b195f00a64ee5471f062edc9fc85c1777ab2da -->
<!-- api: P:Inno.Editor.Annotations.InspectorPresentationAttribute.order 6acdf9810eed910813cc400e8c321ed2c4dad9f446d1ef004a89e07a9184b691 -->
<!-- api: M:Inno.Editor.Annotations.InspectorPresentationAttribute.#ctor 02c8c3876f9a51c163311ee93dc02825af49760059a485cd25aeedeb85de2480 -->
<!-- api: T:Inno.Editor.Annotations.HeaderAttribute fbf95d5aab837f1b5e7a99b6c837eec477e997ccceddb236805d849fa29798d4 -->
<!-- api: M:Inno.Editor.Annotations.HeaderAttribute.#ctor(System.String,System.String) ef848f9f36b6f40634caa994deb243ee84bb8e0bb6ce04e5382706467252af7a -->
<!-- api: P:Inno.Editor.Annotations.HeaderAttribute.title 08f9b6a789f7607d4e61fd67ae3d218c26f83e63c6ca0ac3b175437d7a9144c9 -->
<!-- api: P:Inno.Editor.Annotations.HeaderAttribute.description 5629fe67b3face4e1863edcf6d4b0934c525bb2dbeb4028df6febc13b65077c1 -->
<!-- api: T:Inno.Editor.Annotations.TextAttribute 0cfbf0900b069bb4cd4d2475568481ce67ca2e97fa9e34c49bb5b6183c9f5eb6 -->
<!-- api: M:Inno.Editor.Annotations.TextAttribute.#ctor(System.String) bfe0e4cbd638a9dd461249fa42d3c8461137f2e0deed9a3d5fdde327d073f875 -->
<!-- api: P:Inno.Editor.Annotations.TextAttribute.text bd2195dd732ca79fbd07b0a1091dc33282296e383daec5c353d27089ecd3b5db -->
<!-- api: T:Inno.Editor.Annotations.SpaceAttribute 34c5b712da42b892440f8929a2c3600811987711916905f3c46e5c609eaa8531 -->
<!-- api: M:Inno.Editor.Annotations.SpaceAttribute.#ctor(System.Single) 8f2d6432752fe2c6088669f171cae03f4709af8c3ae85e9a15451289c7a71f48 -->
<!-- api: P:Inno.Editor.Annotations.SpaceAttribute.height 2ba9b05990479667fd663d66d694ba01ce3aba9772e28ae17a80fda5cd340ccd -->
<!-- api: T:Inno.Editor.Annotations.TooltipAttribute 9c7c6e594c0486f28654f4e8c0b25a35681cecfc3ba8790a7151f38efee5244c -->
<!-- api: M:Inno.Editor.Annotations.TooltipAttribute.#ctor(System.String) 34e5846b8ad9f2bc0593675a7397a6c0d3894d105385519bff498f4e40f0209d -->
<!-- api: P:Inno.Editor.Annotations.TooltipAttribute.text 760ffd8dfb8b393deb2cb3bfcfd31076e13abbfaa4c8eb012220cd5b865ee9ec -->
<!-- api: T:Inno.Editor.Annotations.InspectorNameAttribute d8a32b97bc0f8337533c21e6ebbb7f6da52b21c286fc14156c37b00eb877f79a -->
<!-- api: M:Inno.Editor.Annotations.InspectorNameAttribute.#ctor(System.String) eefc6e9f3e1642348f4f05f95880bef2dd770416d8fd578974aca1d37595f45b -->
<!-- api: P:Inno.Editor.Annotations.InspectorNameAttribute.name bc040dc8cb41ae13f2f0b551e028b8639efba17bb5ffe8c3e2731fca3880728c -->
<!-- api: T:Inno.Editor.Annotations.RangeAttribute d44f111da092ba1302018b6083197d393b96d292ae771266822337444261ec91 -->
<!-- api: M:Inno.Editor.Annotations.RangeAttribute.#ctor(System.Double,System.Double) 7205d4b1e834c59865cf055738b99dba9bd4d41d4b1150606e8fb9bf3e8592ca -->
<!-- api: P:Inno.Editor.Annotations.RangeAttribute.minimum 099507951bf9705a9bc6fd29347d8f54a411059d5cb311d2f6e0b06a903bd897 -->
<!-- api: P:Inno.Editor.Annotations.RangeAttribute.maximum 0c570b2f3457a228ebfe958009d12713f556bd97c2bdea9e299cf4debeb3ab6d -->
<!-- api: T:Inno.Editor.Annotations.InspectorReadOnlyAttribute ca847b6ccdfc654390c6a3ac435f8474d4e9842aa82bc6f0498f70908b3bf034 -->
<!-- api: M:Inno.Editor.Annotations.InspectorReadOnlyAttribute.#ctor 2e0f07316dc0c1f610479c07d72fc02ce4d37f55e2d2a5b9f7549d991b1a035e -->
<!-- api: T:Inno.Editor.Annotations.InspectorCondition 75dd3a58b5f98b9fb6fb3f45c3ea4d59bffcc6057ed37344a3c54eee9f721c36 -->
<!-- api: F:Inno.Editor.Annotations.InspectorCondition.Truthy 42e4ea10df82e26eec7b4cbe4dcc62758ed091edba5f0fbc35d221568e661c3b -->
<!-- api: F:Inno.Editor.Annotations.InspectorCondition.Falsy e3f3afe5ddd2fdd14c0325cf4eb066586399f41648dfb7d63e2f38640319fb2f -->
<!-- api: F:Inno.Editor.Annotations.InspectorCondition.Equal 080bb1e3bb1aa22cc1e7f7c72b05fb6a6af526f063ce0c11531e3477ccdb308f -->
<!-- api: F:Inno.Editor.Annotations.InspectorCondition.NotEqual 7c456ae26a56f53626d26a31fa9bcd7c283d1d424cbd480c0ec01415efd45703 -->
<!-- api: F:Inno.Editor.Annotations.InspectorCondition.Null e071ccff757abc86a6094c82748a79fb20145a443fed515fe588a0abf5dc3f0d -->
<!-- api: F:Inno.Editor.Annotations.InspectorCondition.NotNull 8f9a1e293b34aeb6ee81e243de2b31553c7d67b4b5e7af01af0b775ccdd8351a -->
<!-- api: F:Inno.Editor.Annotations.InspectorCondition.Assigned 129b2cca0c6bddbdf67056c94600b1d6db019b6a096ef8de041c6aac956377c5 -->
<!-- api: F:Inno.Editor.Annotations.InspectorCondition.NotAssigned 4bbb342f88b0b0f33a2e541509b1008b1386057d8d316992daca7970b8cae064 -->
<!-- api: T:Inno.Editor.Annotations.ShowIfAttribute 81f361e8b1dd3d8dda567678d75a9cc96bc0485ecda269cf9b3aece345d71ae4 -->
<!-- api: M:Inno.Editor.Annotations.ShowIfAttribute.#ctor(System.String) 82ad59c1f2e3dcb62e2d088d4c163f2563d007c066d7c475e7f2aae850346a4b -->
<!-- api: M:Inno.Editor.Annotations.ShowIfAttribute.#ctor(System.String,System.Object) 9f43bf58ca5e0e95f736eb89754321a9e059ea170c0ff03957bf130ac2fef994 -->
<!-- api: M:Inno.Editor.Annotations.ShowIfAttribute.#ctor(System.String,Inno.Editor.Annotations.InspectorCondition) deb0a2be6e974427c69fa2fd624f0232da96ec52b8bfa3a44470597614dc2c82 -->
<!-- api: P:Inno.Editor.Annotations.ShowIfAttribute.memberName 8ff9d0d4979663dcf806317c69cf896e63ed473b5a9651cb661889b838d8a06a -->
<!-- api: P:Inno.Editor.Annotations.ShowIfAttribute.condition d01fea4029b7dbe87fe315b675fd8796729aee0c92110357dc022002d49f2af1 -->
<!-- api: P:Inno.Editor.Annotations.ShowIfAttribute.expectedValue 78dd3a652b81abb99497f8737c9eacdbceaaaef7466e73991c3dcf186eb863b5 -->
<!-- api: T:Inno.Editor.Annotations.HideIfAttribute c58eb5e62bf519a6d2f2aa2ecd64ad9ab1fd3b385f3aaac39971c3bdd7b7ff8d -->
<!-- api: M:Inno.Editor.Annotations.HideIfAttribute.#ctor(System.String) faa5b79c5ce0133199b40359cf05e155b7e8866fe546cf2db3b285c3ec65b1fa -->
<!-- api: M:Inno.Editor.Annotations.HideIfAttribute.#ctor(System.String,System.Object) 2fdcb49cf4e0cbbc25d24fc0cb8f09f71c21961d8466695ad87d2bf18267f131 -->
<!-- api: M:Inno.Editor.Annotations.HideIfAttribute.#ctor(System.String,Inno.Editor.Annotations.InspectorCondition) c0c43a68e05a57a62b07dbd1c13eeeb5c98ca0a46fa3d5e98b1938bed992e053 -->
<!-- api: P:Inno.Editor.Annotations.HideIfAttribute.memberName 1909cf024d89542636c0827d2cb9fa0a475f4169aef27ab59198caae73c9a950 -->
<!-- api: P:Inno.Editor.Annotations.HideIfAttribute.condition 61434ff3abb37581df7319492cc07ef86486a473c67c06b4982966e9ac2b5683 -->
<!-- api: P:Inno.Editor.Annotations.HideIfAttribute.expectedValue 56c8be9f16ce27b5b1123ef12644751f2af0e034b3d22805e137d5cad6024196 -->
<!-- api: T:Inno.Editor.Annotations.InspectorMessageType 8822c9de5342907860fdd649106156a3840cf7557272256aabe1d79882accace -->
<!-- api: F:Inno.Editor.Annotations.InspectorMessageType.Info 21240990e4c34b9e5eb5bc19c310acab99cb45810d0b47c58c994a79ae97ef5e -->
<!-- api: F:Inno.Editor.Annotations.InspectorMessageType.Warning 70914c805b2f7bf5e50c5c5f9f01c24eb074ab75d7d1b06c0f17e38da744ccc5 -->
<!-- api: F:Inno.Editor.Annotations.InspectorMessageType.Error fccff932efa7b94314a21e1a4860ada2ddc760ef766652a0e2856cdc61cd560c -->
<!-- api: T:Inno.Editor.Annotations.HelpBoxAttribute c1af68e0d57908ab21ff5912af4fc9cd085b3d0a33d5ff4b3db85594fffcd089 -->
<!-- api: M:Inno.Editor.Annotations.HelpBoxAttribute.#ctor(System.String,Inno.Editor.Annotations.InspectorMessageType) c7d9ab3b548518be001c0d3a35f2de10224b85fc630dd17f88fc1a91a25ce94a -->
<!-- api: M:Inno.Editor.Annotations.HelpBoxAttribute.#ctor(System.String,System.String,Inno.Editor.Annotations.InspectorCondition,Inno.Editor.Annotations.InspectorMessageType) f6a36d03de1267ae114220c1535ec6dd8a9416e81b92d592af1a96df049b6145 -->
<!-- api: M:Inno.Editor.Annotations.HelpBoxAttribute.#ctor(System.String,System.String,System.Object,Inno.Editor.Annotations.InspectorMessageType) 9222ec2eed95e56f7d733976b5fa0951acc359f93de79da73d227d10fc8a20ed -->
<!-- api: P:Inno.Editor.Annotations.HelpBoxAttribute.text 628aa84d0d000dd1fe51de56bf737aad320c0cb2bd80b45ba234fb1cb5ca765b -->
<!-- api: P:Inno.Editor.Annotations.HelpBoxAttribute.messageType 62fe3924e765c3c12264bf9020c4303cecdc17080437ec95d723f78c88e3c108 -->
<!-- api: P:Inno.Editor.Annotations.HelpBoxAttribute.conditionMember 5745c599a1dda8ff06efb342be0f7547bf9236971be777a674261234e39441e2 -->
<!-- api: P:Inno.Editor.Annotations.HelpBoxAttribute.condition 1e1449c4881d5efa860bb638c8ec1a8334219c365ee563b96009704c4af294bf -->
<!-- api: P:Inno.Editor.Annotations.HelpBoxAttribute.expectedValue 2e4da25f77d5aa4b4de720727d5471a47937f49235dd4bf539e2e80830f53524 -->

| API 与源码 | 契约与参数 | 返回值 | 重要异常 |
| --- | --- | --- | --- |
| <a id="api-d97698f91094c2f2"></a>[`public abstract class Inno.Editor.Annotations.InspectorPresentationAttribute : System.Attribute`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L15) | Provides editor-only presentation metadata for one serialized field or property. | — | — |
| <a id="api-668a194089fbada7"></a>[`public int Inno.Editor.Annotations.InspectorPresentationAttribute.order { get; set; }`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L20) | Gets or sets the relative order among presentation attributes declared on the same member. | — | — |
| <a id="api-78d092fed6478827"></a>[`protected Inno.Editor.Annotations.InspectorPresentationAttribute.InspectorPresentationAttribute()`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L15) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c9953d25080c9cd1"></a>[`public sealed class Inno.Editor.Annotations.HeaderAttribute : Inno.Editor.Annotations.InspectorPresentationAttribute`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L27) | Starts a visually separated Inspector section before the annotated property. | — | — |
| <a id="api-030405d68c3d2c10"></a>[`public Inno.Editor.Annotations.HeaderAttribute.HeaderAttribute(string title, string description = "")`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L38) | Creates a section header with optional explanatory text.<br>`title`: Visible section title.<br>`description`: Optional explanatory text displayed when the title is hovered. | — | — |
| <a id="api-15cd1080ccbe6502"></a>[`public string Inno.Editor.Annotations.HeaderAttribute.title { get; }`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L50) | Gets the visible section title. | — | — |
| <a id="api-66f7aefaf76d92d7"></a>[`public string Inno.Editor.Annotations.HeaderAttribute.description { get; }`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L55) | Gets the optional explanatory text. | — | — |
| <a id="api-551ca5f0e0aae7e7"></a>[`public sealed class Inno.Editor.Annotations.TextAttribute : Inno.Editor.Annotations.InspectorPresentationAttribute`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L62) | Draws persistent wrapped explanatory text before the annotated Inspector property. | — | — |
| <a id="api-77a31fa540b4e72a"></a>[`public Inno.Editor.Annotations.TextAttribute.TextAttribute(string text)`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L70) | Creates persistent Inspector text.<br>`text`: Non-empty text displayed in the Inspector layout. | — | — |
| <a id="api-d8254cfb20d53085"></a>[`public string Inno.Editor.Annotations.TextAttribute.text { get; }`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L79) | Gets the persistent Inspector text. | — | — |
| <a id="api-fb128dbc166fbcaa"></a>[`public sealed class Inno.Editor.Annotations.SpaceAttribute : Inno.Editor.Annotations.InspectorPresentationAttribute`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L86) | Inserts vertical spacing before the annotated Inspector property. | — | — |
| <a id="api-dbb776ba2d4bf563"></a>[`public Inno.Editor.Annotations.SpaceAttribute.SpaceAttribute(float height = 8)`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L94) | Creates a spacing decorator.<br>`height`: Non-negative logical pixel height. | — | — |
| <a id="api-bff9fd2adf2d80ac"></a>[`public float Inno.Editor.Annotations.SpaceAttribute.height { get; }`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L103) | Gets the logical pixel height. | — | — |
| <a id="api-08195c185416d2cf"></a>[`public sealed class Inno.Editor.Annotations.TooltipAttribute : Inno.Editor.Annotations.InspectorPresentationAttribute`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L110) | Provides hover help for one Inspector property label. | — | — |
| <a id="api-673f4f588d166bb3"></a>[`public Inno.Editor.Annotations.TooltipAttribute.TooltipAttribute(string text)`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L118) | Creates property hover help.<br>`text`: Non-empty tooltip text. | — | — |
| <a id="api-be294098562ada9e"></a>[`public string Inno.Editor.Annotations.TooltipAttribute.text { get; }`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L127) | Gets the tooltip text. | — | — |
| <a id="api-787ddfcf9a178fc6"></a>[`public sealed class Inno.Editor.Annotations.InspectorNameAttribute : Inno.Editor.Annotations.InspectorPresentationAttribute`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L134) | Replaces the generated Inspector label for one property. | — | — |
| <a id="api-335743f93387e718"></a>[`public Inno.Editor.Annotations.InspectorNameAttribute.InspectorNameAttribute(string name)`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L142) | Creates a label override.<br>`name`: Non-empty visible label. | — | — |
| <a id="api-4fcabed3fb39d7cc"></a>[`public string Inno.Editor.Annotations.InspectorNameAttribute.name { get; }`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L151) | Gets the visible label. | — | — |
| <a id="api-c356abaced7b2e55"></a>[`public sealed class Inno.Editor.Annotations.RangeAttribute : Inno.Editor.Annotations.InspectorPresentationAttribute`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L158) | Constrains a numeric Inspector control to an inclusive range. | — | — |
| <a id="api-573167e80a1e3e1a"></a>[`public Inno.Editor.Annotations.RangeAttribute.RangeAttribute(double minimum, double maximum)`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L169) | Creates an inclusive numeric range.<br>`minimum`: Inclusive lower bound.<br>`maximum`: Inclusive upper bound. | — | — |
| <a id="api-c4af0478173dea83"></a>[`public double Inno.Editor.Annotations.RangeAttribute.minimum { get; }`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L184) | Gets the inclusive lower bound. | — | — |
| <a id="api-47c922fa4cd3454e"></a>[`public double Inno.Editor.Annotations.RangeAttribute.maximum { get; }`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L189) | Gets the inclusive upper bound. | — | — |
| <a id="api-cff25400e5241859"></a>[`public sealed class Inno.Editor.Annotations.InspectorReadOnlyAttribute : Inno.Editor.Annotations.InspectorPresentationAttribute`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L196) | Makes an otherwise writable serialized property read-only in the Inspector. | — | — |
| <a id="api-4f433bc367980d5a"></a>[`public Inno.Editor.Annotations.InspectorReadOnlyAttribute.InspectorReadOnlyAttribute()`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L196) | Inherited or compiler-provided contract. | — | — |
| <a id="api-069fda85c5928e67"></a>[`public enum Inno.Editor.Annotations.InspectorCondition : int`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L203) | Defines the comparison used by conditional Inspector presentation. | — | — |
| <a id="api-aef06bae2fd89a2f"></a>[`Inno.Editor.Annotations.InspectorCondition.Truthy = 0`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L208) | Shows a value when it evaluates as true or non-empty. | — | — |
| <a id="api-9a42ebc1ea7c1300"></a>[`Inno.Editor.Annotations.InspectorCondition.Falsy = 1`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L213) | Shows a value when it evaluates as false, null, or empty. | — | — |
| <a id="api-cb61260927d48807"></a>[`Inno.Editor.Annotations.InspectorCondition.Equal = 2`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L218) | Compares the value with the supplied expected constant. | — | — |
| <a id="api-aaed0cb9f42b96c1"></a>[`Inno.Editor.Annotations.InspectorCondition.NotEqual = 3`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L223) | Requires the value to differ from the supplied expected constant. | — | — |
| <a id="api-ab596b8de0abf293"></a>[`Inno.Editor.Annotations.InspectorCondition.Null = 4`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L228) | Requires a null value. | — | — |
| <a id="api-985e8e67636c2509"></a>[`Inno.Editor.Annotations.InspectorCondition.NotNull = 5`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L233) | Requires a non-null value. | — | — |
| <a id="api-e2c9dd0ccf5dddce"></a>[`Inno.Editor.Annotations.InspectorCondition.Assigned = 6`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L238) | Requires a reference-like value to expose an assigned resource. | — | — |
| <a id="api-d18a1ec0bbdb7ee8"></a>[`Inno.Editor.Annotations.InspectorCondition.NotAssigned = 7`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L243) | Requires a reference-like value not to expose an assigned resource. | — | — |
| <a id="api-09f10095a7f506b3"></a>[`public sealed class Inno.Editor.Annotations.ShowIfAttribute : Inno.Editor.Annotations.InspectorPresentationAttribute`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L250) | Shows the annotated Inspector property only when a sibling member satisfies a condition. | — | — |
| <a id="api-c5c75382824d2db6"></a>[`public Inno.Editor.Annotations.ShowIfAttribute.ShowIfAttribute(string memberName)`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L258) | Creates a truthiness condition.<br>`memberName`: Sibling field or property name. | — | — |
| <a id="api-5f2c4628811f11b1"></a>[`public Inno.Editor.Annotations.ShowIfAttribute.ShowIfAttribute(string memberName, object? expectedValue)`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L272) | Creates an equality condition.<br>`memberName`: Sibling field or property name.<br>`expectedValue`: Compile-time value required for visibility. | — | — |
| <a id="api-7144a0f705ddd9f8"></a>[`public Inno.Editor.Annotations.ShowIfAttribute.ShowIfAttribute(string memberName, Inno.Editor.Annotations.InspectorCondition condition)`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L289) | Creates a named condition that does not require an expected value.<br>`memberName`: Sibling field or property name.<br>`condition`: Comparison applied to the sibling value. | — | — |
| <a id="api-6f0fea0e46045a8b"></a>[`public string Inno.Editor.Annotations.ShowIfAttribute.memberName { get; }`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L311) | Gets the sibling member name. | — | — |
| <a id="api-b14dad49d402ede5"></a>[`public Inno.Editor.Annotations.InspectorCondition Inno.Editor.Annotations.ShowIfAttribute.condition { get; }`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L316) | Gets the comparison. | — | — |
| <a id="api-6063c83034b9393c"></a>[`public object? Inno.Editor.Annotations.ShowIfAttribute.expectedValue { get; }`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L321) | Gets the optional expected constant. | — | — |
| <a id="api-0043bd3df1594a32"></a>[`public sealed class Inno.Editor.Annotations.HideIfAttribute : Inno.Editor.Annotations.InspectorPresentationAttribute`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L328) | Hides the annotated Inspector property when a sibling member satisfies a condition. | — | — |
| <a id="api-82c9dd0764fe5881"></a>[`public Inno.Editor.Annotations.HideIfAttribute.HideIfAttribute(string memberName)`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L336) | Creates a truthiness condition.<br>`memberName`: Sibling field or property name. | — | — |
| <a id="api-33b9be8adaca3905"></a>[`public Inno.Editor.Annotations.HideIfAttribute.HideIfAttribute(string memberName, object? expectedValue)`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L350) | Creates an equality condition.<br>`memberName`: Sibling field or property name.<br>`expectedValue`: Compile-time value that hides the property. | — | — |
| <a id="api-ed64a9030e2dab3b"></a>[`public Inno.Editor.Annotations.HideIfAttribute.HideIfAttribute(string memberName, Inno.Editor.Annotations.InspectorCondition condition)`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L367) | Creates a named condition that does not require an expected value.<br>`memberName`: Sibling field or property name.<br>`condition`: Comparison applied to the sibling value. | — | — |
| <a id="api-483062ff5f410ada"></a>[`public string Inno.Editor.Annotations.HideIfAttribute.memberName { get; }`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L389) | Gets the sibling member name. | — | — |
| <a id="api-df4143be36524361"></a>[`public Inno.Editor.Annotations.InspectorCondition Inno.Editor.Annotations.HideIfAttribute.condition { get; }`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L394) | Gets the comparison. | — | — |
| <a id="api-3fcdfe0145c9358b"></a>[`public object? Inno.Editor.Annotations.HideIfAttribute.expectedValue { get; }`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L399) | Gets the optional expected constant. | — | — |
| <a id="api-f183b063a6bc054b"></a>[`public enum Inno.Editor.Annotations.InspectorMessageType : int`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L405) | Selects the visual severity of an Inspector help message. | — | — |
| <a id="api-d02b5b2db3523d82"></a>[`Inno.Editor.Annotations.InspectorMessageType.Info = 0`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L410) | Neutral explanatory information. | — | — |
| <a id="api-b0cbe63a4a3f16e7"></a>[`Inno.Editor.Annotations.InspectorMessageType.Warning = 1`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L415) | A non-blocking configuration warning. | — | — |
| <a id="api-5a058d24dd6ddba3"></a>[`Inno.Editor.Annotations.InspectorMessageType.Error = 2`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L420) | An invalid or unusable configuration. | — | — |
| <a id="api-be9f7d15efafe716"></a>[`public sealed class Inno.Editor.Annotations.HelpBoxAttribute : Inno.Editor.Annotations.InspectorPresentationAttribute`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L427) | Draws a wrapped explanatory message before the annotated Inspector property. | — | — |
| <a id="api-960656f2b0d78274"></a>[`public Inno.Editor.Annotations.HelpBoxAttribute.HelpBoxAttribute(string text, Inno.Editor.Annotations.InspectorMessageType messageType = Inno.Editor.Annotations.InspectorMessageType.Info)`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L438) | Creates an Inspector help message.<br>`text`: Non-empty message text.<br>`messageType`: Visual severity. | — | — |
| <a id="api-bbda1c55f8b0e4bd"></a>[`public Inno.Editor.Annotations.HelpBoxAttribute.HelpBoxAttribute(string text, string conditionMember, Inno.Editor.Annotations.InspectorCondition condition, Inno.Editor.Annotations.InspectorMessageType messageType = Inno.Editor.Annotations.InspectorMessageType.Info)`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L461) | Creates an Inspector help message shown when a sibling satisfies a named condition.<br>`text`: Non-empty message text.<br>`conditionMember`: Sibling field or property name.<br>`condition`: Comparison applied to the sibling value.<br>`messageType`: Visual severity. | — | — |
| <a id="api-58a147a30e5dfa32"></a>[`public Inno.Editor.Annotations.HelpBoxAttribute.HelpBoxAttribute(string text, string conditionMember, object? expectedValue, Inno.Editor.Annotations.InspectorMessageType messageType = Inno.Editor.Annotations.InspectorMessageType.Info)`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L486) | Creates an Inspector help message shown when a sibling equals a compile-time value.<br>`text`: Non-empty message text.<br>`conditionMember`: Sibling field or property name.<br>`expectedValue`: The expected value consumed by help box attribute; ownership remains with the caller unless explicitly stated otherwise.<br>`messageType`: Visual severity. | — | — |
| <a id="api-cc3cb23bbac31f3f"></a>[`public string Inno.Editor.Annotations.HelpBoxAttribute.text { get; }`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L514) | Gets the message text. | — | — |
| <a id="api-be5fbcba38440a65"></a>[`public Inno.Editor.Annotations.InspectorMessageType Inno.Editor.Annotations.HelpBoxAttribute.messageType { get; }`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L519) | Gets the visual severity. | — | — |
| <a id="api-c9c115dcbbfc9486"></a>[`public string Inno.Editor.Annotations.HelpBoxAttribute.conditionMember { get; }`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L524) | Gets the optional sibling member controlling message visibility. | — | — |
| <a id="api-7e65c229676eb40a"></a>[`public Inno.Editor.Annotations.InspectorCondition Inno.Editor.Annotations.HelpBoxAttribute.condition { get; }`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L529) | Gets the optional sibling comparison. | — | — |
| <a id="api-44994f06b13fa1fe"></a>[`public object? Inno.Editor.Annotations.HelpBoxAttribute.expectedValue { get; }`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L534) | Gets the optional expected constant. | — | — |

## 项目依赖

| 工程 | 边界 |
| --- | --- |
| `Inno.Scripting.Api` | 实现依赖；不传递 compile surface |
| `Inno.Extensibility.Catalogs` | 公开签名依赖 |
