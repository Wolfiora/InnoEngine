# Inno.Editor.Annotations

[Editor 索引](README.md) · [Inspection](Inno.Editor.Inspection.md) · [Wiki 首页](../README.md)

## 职责与依赖

独立、无状态的 Inspector 创作标注契约。它只依赖 System.Attribute 与 Scripting.Api 导出声明，不引用 Serialization、Scene、Rendering、ImGui 或 Panel。Inspection 负责解释及渲染；Core.Serialization 只负责数据。不存在旧 namespace alias。

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

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Editor.Annotations.HeaderAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Annotations.HeaderAttribute`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L26) | Starts a visually separated Inspector section before the annotated property. |
| [`Inno.Editor.Annotations.HeaderAttribute.HeaderAttribute(string title, string description = "")`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L38) | Creates a section header with optional explanatory text. |
| [`string Inno.Editor.Annotations.HeaderAttribute.description`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L55) | Gets the optional explanatory text. |
| [`string Inno.Editor.Annotations.HeaderAttribute.title`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L50) | Gets the visible section title. |

### `Inno.Editor.Annotations.HelpBoxAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Annotations.HelpBoxAttribute`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L426) | Draws a wrapped explanatory message before the annotated Inspector property. |
| [`Inno.Editor.Annotations.HelpBoxAttribute.HelpBoxAttribute(string text, Inno.Editor.Annotations.InspectorMessageType messageType = Inno.Editor.Annotations.InspectorMessageType.Info)`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L438) | Creates an Inspector help message. |
| [`Inno.Editor.Annotations.HelpBoxAttribute.HelpBoxAttribute(string text, string conditionMember, Inno.Editor.Annotations.InspectorCondition condition, Inno.Editor.Annotations.InspectorMessageType messageType = Inno.Editor.Annotations.InspectorMessageType.Info)`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L461) | Creates an Inspector help message shown when a sibling satisfies a named condition. |
| [`Inno.Editor.Annotations.HelpBoxAttribute.HelpBoxAttribute(string text, string conditionMember, object? expectedValue, Inno.Editor.Annotations.InspectorMessageType messageType = Inno.Editor.Annotations.InspectorMessageType.Info)`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L486) | Creates an Inspector help message shown when a sibling equals a compile-time value. |
| [`Inno.Editor.Annotations.InspectorCondition Inno.Editor.Annotations.HelpBoxAttribute.condition`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L529) | Gets the optional sibling comparison. |
| [`Inno.Editor.Annotations.InspectorMessageType Inno.Editor.Annotations.HelpBoxAttribute.messageType`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L519) | Gets the visual severity. |
| [`object? Inno.Editor.Annotations.HelpBoxAttribute.expectedValue`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L534) | Gets the optional expected constant. |
| [`string Inno.Editor.Annotations.HelpBoxAttribute.conditionMember`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L524) | Gets the optional sibling member controlling message visibility. |
| [`string Inno.Editor.Annotations.HelpBoxAttribute.text`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L514) | Gets the message text. |

### `Inno.Editor.Annotations.HideIfAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Annotations.HideIfAttribute`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L327) | Hides the annotated Inspector property when a sibling member satisfies a condition. |
| [`Inno.Editor.Annotations.HideIfAttribute.HideIfAttribute(string memberName)`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L336) | Creates a truthiness condition. |
| [`Inno.Editor.Annotations.HideIfAttribute.HideIfAttribute(string memberName, Inno.Editor.Annotations.InspectorCondition condition)`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L367) | Creates a named condition that does not require an expected value. |
| [`Inno.Editor.Annotations.HideIfAttribute.HideIfAttribute(string memberName, object? expectedValue)`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L350) | Creates an equality condition. |
| [`Inno.Editor.Annotations.InspectorCondition Inno.Editor.Annotations.HideIfAttribute.condition`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L394) | Gets the comparison. |
| [`object? Inno.Editor.Annotations.HideIfAttribute.expectedValue`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L399) | Gets the optional expected constant. |
| [`string Inno.Editor.Annotations.HideIfAttribute.memberName`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L389) | Gets the sibling member name. |

### `Inno.Editor.Annotations.InspectorCondition`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Annotations.InspectorCondition`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L203) | Defines the comparison used by conditional Inspector presentation. |
| [`Inno.Editor.Annotations.InspectorCondition.Assigned`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L238) | Requires a reference-like value to expose an assigned resource. |
| [`Inno.Editor.Annotations.InspectorCondition.Equal`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L218) | Compares the value with the supplied expected constant. |
| [`Inno.Editor.Annotations.InspectorCondition.Falsy`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L213) | Shows a value when it evaluates as false, null, or empty. |
| [`Inno.Editor.Annotations.InspectorCondition.NotAssigned`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L243) | Requires a reference-like value not to expose an assigned resource. |
| [`Inno.Editor.Annotations.InspectorCondition.NotEqual`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L223) | Requires the value to differ from the supplied expected constant. |
| [`Inno.Editor.Annotations.InspectorCondition.NotNull`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L233) | Requires a non-null value. |
| [`Inno.Editor.Annotations.InspectorCondition.Null`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L228) | Requires a null value. |
| [`Inno.Editor.Annotations.InspectorCondition.Truthy`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L208) | Shows a value when it evaluates as true or non-empty. |

### `Inno.Editor.Annotations.InspectorMessageType`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Annotations.InspectorMessageType`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L405) | Selects the visual severity of an Inspector help message. |
| [`Inno.Editor.Annotations.InspectorMessageType.Error`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L420) | An invalid or unusable configuration. |
| [`Inno.Editor.Annotations.InspectorMessageType.Info`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L410) | Neutral explanatory information. |
| [`Inno.Editor.Annotations.InspectorMessageType.Warning`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L415) | A non-blocking configuration warning. |

### `Inno.Editor.Annotations.InspectorNameAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Annotations.InspectorNameAttribute`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L133) | Replaces the generated Inspector label for one property. |
| [`Inno.Editor.Annotations.InspectorNameAttribute.InspectorNameAttribute(string name)`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L142) | Creates a label override. |
| [`string Inno.Editor.Annotations.InspectorNameAttribute.name`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L151) | Gets the visible label. |

### `Inno.Editor.Annotations.InspectorPresentationAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Annotations.InspectorPresentationAttribute`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L13) | Provides editor-only presentation metadata for one serialized field or property. |
| [`int Inno.Editor.Annotations.InspectorPresentationAttribute.order`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L20) | Gets or sets the relative order among presentation attributes declared on the same member. |

### `Inno.Editor.Annotations.InspectorReadOnlyAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Annotations.InspectorReadOnlyAttribute`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L195) | Makes an otherwise writable serialized property read-only in the Inspector. |

### `Inno.Editor.Annotations.RangeAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Annotations.RangeAttribute`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L157) | Constrains a numeric Inspector control to an inclusive range. |
| [`Inno.Editor.Annotations.RangeAttribute.RangeAttribute(double minimum, double maximum)`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L169) | Creates an inclusive numeric range. |
| [`double Inno.Editor.Annotations.RangeAttribute.maximum`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L189) | Gets the inclusive upper bound. |
| [`double Inno.Editor.Annotations.RangeAttribute.minimum`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L184) | Gets the inclusive lower bound. |

### `Inno.Editor.Annotations.ShowIfAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Annotations.InspectorCondition Inno.Editor.Annotations.ShowIfAttribute.condition`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L316) | Gets the comparison. |
| [`Inno.Editor.Annotations.ShowIfAttribute`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L249) | Shows the annotated Inspector property only when a sibling member satisfies a condition. |
| [`Inno.Editor.Annotations.ShowIfAttribute.ShowIfAttribute(string memberName)`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L258) | Creates a truthiness condition. |
| [`Inno.Editor.Annotations.ShowIfAttribute.ShowIfAttribute(string memberName, Inno.Editor.Annotations.InspectorCondition condition)`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L289) | Creates a named condition that does not require an expected value. |
| [`Inno.Editor.Annotations.ShowIfAttribute.ShowIfAttribute(string memberName, object? expectedValue)`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L272) | Creates an equality condition. |
| [`object? Inno.Editor.Annotations.ShowIfAttribute.expectedValue`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L321) | Gets the optional expected constant. |
| [`string Inno.Editor.Annotations.ShowIfAttribute.memberName`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L311) | Gets the sibling member name. |

### `Inno.Editor.Annotations.SpaceAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Annotations.SpaceAttribute`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L85) | Inserts vertical spacing before the annotated Inspector property. |
| [`Inno.Editor.Annotations.SpaceAttribute.SpaceAttribute(float height = 8)`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L94) | Creates a spacing decorator. |
| [`float Inno.Editor.Annotations.SpaceAttribute.height`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L103) | Gets the logical pixel height. |

### `Inno.Editor.Annotations.TextAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Annotations.TextAttribute`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L61) | Draws persistent wrapped explanatory text before the annotated Inspector property. |
| [`Inno.Editor.Annotations.TextAttribute.TextAttribute(string text)`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L70) | Creates persistent Inspector text. |
| [`string Inno.Editor.Annotations.TextAttribute.text`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L79) | Gets the persistent Inspector text. |

### `Inno.Editor.Annotations.TooltipAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Annotations.TooltipAttribute`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L109) | Provides hover help for one Inspector property label. |
| [`Inno.Editor.Annotations.TooltipAttribute.TooltipAttribute(string text)`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L118) | Creates property hover help. |
| [`string Inno.Editor.Annotations.TooltipAttribute.text`](../../src/composition/editor/contracts/Inno.Editor.Annotations/InspectorPresentationAttributes.cs#L127) | Gets the tooltip text. |

## 项目依赖

- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
