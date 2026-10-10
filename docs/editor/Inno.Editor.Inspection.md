# Inno.Editor.Inspection

[Editor 索引](README.md) · [Inspector Panel](Inno.Editor.Panel.Inspector.md) · [Interactions](Inno.Editor.Interactions.md)

该项目是与具体 Panel 无关的检查绘制基础设施。它拥有 `InspectionDrawer<TTarget>`、`PropertyDrawer` 契约、TypeCache Registry、draw context、serialized property renderer 以及 bool、number、string、enum、collection、struct、nullable、math 等通用内建 PropertyDrawer。

`Inno.Editor.Panel.Inspector` 只负责窗口、统一 Target Header 和 Scene 的 Component/System 操作；其他 feature 不需要引用 Inspector Panel。Feature project 只需引用 `Inno.Editor.Inspection`，把自己的内部 Drawer 与目标类型放在同一个项目中，TypeCache 会自动发现它。

## InspectionDrawer

```csharp
using System;

using Inno.Editor.Inspection;
using Inno.Adapter.Presentation.ImGui;

[InspectionDrawer(typeof(AnimationController))]
internal sealed class AnimationControllerInspectionDrawer
    : InspectionDrawer<AnimationController>
{
    public override string icon => ImGuiIcon.DiagramProject;

    protected override (string name, Action<string>? setter) BindName(
        InspectionDrawContext context,
        AnimationController target)
        => (target.name, null);

    protected override void Draw(
        InspectionDrawContext context,
        AnimationController target)
    {
        // Draw the feature-owned target without referencing InspectorPanel.
    }
}
```

`BindName` 在一次调用中返回当前显示名称和可选 setter。只读名称返回 `(name, null)`；可编辑名称返回 `(name, value => ...)`。Inspector Header 只解析一次该 tuple，避免名称值和编辑能力来自不同快照。

解析顺序是 exact target、继承距离、priority、稳定类型名。Registry 在新的 TypeCache generation 上旁路构建完整 snapshot；构造失败或 registration 冲突不会替换当前可用 snapshot。

`InspectionDrawContext` 提供：

- 当前 `EditorContext`。
- 当前 `EditorInteractions`。
- 当前 target。
- `SerializedPropertyRenderer`。
- `DrawProperties()`，把当前 target 的全部 runtime-visible `SerializableProperty` 送入同一 Attribute/PropertyDrawer 管线；没有显式 `[Header]` 的首组属性自动进入 `Properties` fieldset。
- `InspectionDrawerAttribute(..., conditional: true)` 声明由 CanInspect 筛选目标的 Drawer，允许同类型、同优先级的条件注册。实际同时接受同一目标仍明确报歧义，不依赖发现顺序；普通无条件冲突继续在候选构建时报错。
- `DrawValue(..., readOnly, hdrColor, tooltip, minimum, maximum)` 用共享控件编辑声明值，允许动态声明贡献 Tooltip/数值范围，不伪造 CLR Attribute 或建立第二套控件；`DrawDraftProperty(editorContext, stateOwner, valueOwner, ownerPath, property, edits, readOnly)` 还保留原生属性 Header/Tooltip/范围。stateOwner 为中立文档状态，valueOwner 是仅本帧使用的设置对象，避免文本状态保留插件代际。

`Color` PropertyDrawer 与 `hdrColor` 设置共用线性值契约：Inspector 色块按显示 sRGB 绘制，编辑后的 RGB 解码为线性值再交给 `PropertyDrawContext.SetValue`；alpha 原样传递，不把 Inspector 预览当成另一种场景颜色。

通用 context 不包含 `SceneEdits`、AssetPipeline 或其他 feature service。具体 Drawer 需要领域能力时，由宿主组合根通过构造函数注入。例如 GameObject/Scene Drawer 在 Inspector Panel 内部取得 `SceneEdits`，而资产 Drawer 只取得资产 icon provider。

## PropertyDrawer

```csharp
using Inno.Editor.Inspection;

[PropertyDrawer(typeof(AnimationCurve), priority: 100)]
internal sealed class AnimationCurvePropertyDrawer : IPropertyDrawer
{
    public void Draw(PropertyDrawContext context)
    {
        AnimationCurve value = (AnimationCurve)context.GetValue();
        // Draw and call context.SetValue(updatedValue) after an edit.
    }
}
```

PropertyDrawer 通过 declared property type 匹配。`PropertyDrawContext.SetValue` 会把修改交给所属 feature 的 edit service，由它写入中立的 Undo/Redo payload；Drawer 不应该绕过 context 直接修改 serialized owner。`PropertyDrawContext.tooltip` 暴露已经组合完成的 hover help；bool 与 nullable 内建 Drawer 会把它同时绑定到 checkbox 本身，因此 label 和 control 都能解释同一属性。

`SerializedPropertyRenderer` 本身不依赖 Scene。创建 renderer 的 feature 提供 `IInspectionPropertyEditService`，负责把通用的 owner、root property 与 mutation 转换成自己的 Undo/Redo 协议。Inspector Panel 使用 Scene adapter；未来 Material、Animation 或 RenderGraph 检查器可以使用各自的 history adapter，而不用把 Scene 引入通用 Inspection 项目。

## Inspector presentation attributes

GameScripts 可以通过独立的 [Inno.Editor.Annotations](Inno.Editor.Annotations.md) 在 `SerializableProperty` 之外组合纯展示 Attribute；它们不决定成员是否持久化，也不改变序列化 schema：

```csharp
using InnoEngine.Serialization;
using InnoEditor.Annotations;

public sealed class CharacterPresentation : ISerializable
{
    [SerializableProperty]
    [Header("Movement", "Values are authored in world units per second.")]
    [InspectorName("Move Speed")]
    [Tooltip("Maximum planar speed.")]
    [Range(0, 30)]
    public float speed { get; set; } = 6f;

    [SerializableProperty]
    public bool useAcceleration { get; set; }

    [SerializableProperty]
    [ShowIf(nameof(useAcceleration))]
    [Range(0, 100)]
    public float acceleration { get; set; } = 20f;
}
```

`Header` 的 description 是标题的 hover tooltip，不占用 Inspector 的纵向空间。需要常驻说明文字时使用独立的 `Text` decorator：

```csharp
[SerializableProperty]
[Text("This message remains visible above the control.")]
public float authoredValue { get; set; }
```

内建集合包括 `Header`、`Text`、`Space`、`Tooltip`、`InspectorName`、`Range`、`InspectorReadOnly`、`ShowIf`、`HideIf` 和 `HelpBox`。多个条件按 AND 组合；`InspectorCondition` 支持 truthy/falsy、equal/not-equal、null/not-null，以及资源引用的 assigned/not-assigned。`HelpBox` 既能始终显示，也能由 sibling member 条件控制。它使用独立图标、语义色侧边、背景与边框表达 Info/Warning/Error 状态；`Text` 是无状态、常驻的普通说明；`Tooltip` 仅在悬停时展示。标题 description 也只作为 tooltip。默认 Drawer 不复制 fieldset、tooltip 或 property layout：它只调用 `InspectionDrawContext.DrawProperties()`；Asset/Panel 的定制 Drawer 可以复用同一入口，也可以只替换确实属于领域的部分。

Inspector 的浮点标量以及 `Vector2/3/4`、`Rect`、Quaternion Euler 和 Transform 轴字段默认以一位小数显示，但底层值不会按显示精度舍入。双击字段会进入九位有效数字的 round-trip 文本编辑；带 `Range` 的 float 常态保持 slider，进入精确编辑时仍执行相同的上下界约束。

这些 Attribute 的使用点带 `Conditional("INNO_EDITOR")`。Authoring GameScripts 和 IDE 投影定义 `INNO_EDITOR`；Player deployment 还会按 `Authoring` 导出清单做语义擦除，移除使用点、自定义派生标注声明及 import，避免残留 Editor.Annotations 程序集引用。`SerializableProperty` 仍保留并负责运行时序列化。ImGui、metadata cache 和 drawer discovery 实现只存在于 Editor。

自定义展示 Attribute 继承 `InspectorPresentationAttribute`。对应 EditorScript 实现 `IInspectorAttributeDrawer`，并用 `[InspectorAttributeDrawer(typeof(MyAttribute))]` 注册；`Update` 可以组合 visibility、read-only、label、tooltip 和 numeric bounds，`DrawBefore`/`DrawAfter` 可以添加 decorator。Registry 使用 TypeCache generation snapshot 和稳定 priority 解析，所以 Plugin 可以热重载扩展而无需修改 Inspector 或中心 switch。

`PropertyDrawContext.DrawInlineChild` 同样进入 `SerializedPropertyRenderer.DrawInline`，不会自行 Resolve 或直接调用 Drawer。Inline 与普通属性共用 drawer resolution、readonly disabled scope、按完整 child path 去重的异常日志和错误呈现，但不创建额外 `PropertyRow`；Drawer 异常在当前 child 被消费，父属性与后续 Inspector 内容继续绘制。本次契约不改变 `SetValue` 的返回或 readonly 行为。

需要跨帧保留尚未提交的数字、Guid 或集合文本时，Drawer 使用 `TryGetTextState`、`SetTextState` 和
`ClearTextState`。状态由 `SerializedPropertyRenderer` 实例拥有，并以 inspected owner 弱引用、
完整 property path 和 drawer-local key 三层隔离；不同 Inspector/Editor Host、相同路径的两个对象
以及同一对象的两个 renderer 都不会串值。缓存只保存中立字符串，owner 消失后整组状态可自动
回收，不会通过静态字典固定 Scene 对象或 Plugin ALC。

## Feature 间 presentation 契约

`IInspectionIconProvider<TTarget>` 用于共享目标所属 feature 的图标规则，而不引入 Panel→Panel 引用。例如 FileBrowser 的 `AssetEditorModule` 实现 `IInspectionIconProvider<AssetFileEntry>`，它自己的 `AssetSelectionInspectionDrawer` 因而可以复用同一个 Asset icon registry。Inspector 组合根只依赖该接口，不引用 FileBrowser 项目。Scene/GameObject Drawer 直接注入 `EditorSettings`，通过原始完整路径读取与 Hierarchy、Asset Browser 一致的 icon object；Settings 项目不提供 icon resolver。

## Scripting API

EditorScripts 通过显式 `using InnoEditor.Inspection;` 使用裁剪后的 `InspectionDrawer`、`PropertyDrawer`、`InspectorAttributeDrawer`、`IInspectionIconProvider<TTarget>` 与 property-edit 契约。`AssetFileEntry` 条件 Drawer 因而能从 File Browser 注入统一图标解析，而不复制 Appearance 设置。Registry、snapshot、Activator 和各 feature 的具体 mutation adapter 不导出。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Editor.Inspection.AssetInspectionSelection`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Inspection.AssetInspectionSelection`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/AssetInspectionSelection.cs#L10) | Represents an ordered multi-asset selection without retaining asset or plugin-generation objects. |
| [`Inno.Editor.Inspection.AssetInspectionSelection.AssetInspectionSelection(System.Collections.Generic.IEnumerable<System.Guid> assetIds)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/AssetInspectionSelection.cs#L18) | Captures unique persistent identities in selection order. |
| [`System.Collections.Generic.IReadOnlyList<System.Guid> Inno.Editor.Inspection.AssetInspectionSelection.assetIds`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/AssetInspectionSelection.cs#L29) | Gets stable asset identities in selection order. |
| [`System.Guid Inno.Editor.Inspection.AssetInspectionSelection.primaryAssetId`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/AssetInspectionSelection.cs#L33) | Gets the most recently selected asset identity. |

### `Inno.Editor.Inspection.IInspectionDrawer`

| 当前声明 | 行为 |
| --- | --- |
| [`(string name, System.Action<string>? setter) Inno.Editor.Inspection.IInspectionDrawer.BindName(Inno.Editor.Inspection.InspectionDrawContext context)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawer.cs#L159) | Binds the target name displayed by the Inspector target header. |
| [`Inno.Editor.Inspection.IInspectionDrawer`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawer.cs#L125) | Defines the runtime bridge used to render a discovered inspection drawer. |
| [`bool Inno.Editor.Inspection.IInspectionDrawer.CanInspect(object target)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawer.cs#L136) | Determines whether a type-compatible selected value can be inspected by this drawer. |
| [`string Inno.Editor.Inspection.IInspectionDrawer.GetIcon(Inno.Editor.Inspection.InspectionDrawContext context)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawer.cs#L147) | Gets the icon glyph displayed in the Inspector target header. |
| [`void Inno.Editor.Inspection.IInspectionDrawer.Draw(Inno.Editor.Inspection.InspectionDrawContext context)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawer.cs#L175) | Draws the target-specific Inspector body. |
| [`void Inno.Editor.Inspection.IInspectionDrawer.DrawHeader(Inno.Editor.Inspection.InspectionDrawContext context)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawer.cs#L167) | Draws the target-specific second header row. |

### `Inno.Editor.Inspection.IInspectionIconProvider<TTarget>`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Inspection.IInspectionIconProvider<TTarget>`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/IInspectionIconProvider.cs#L9) | Resolves a presentation icon for inspected targets owned by another editor feature. |
| [`string Inno.Editor.Inspection.IInspectionIconProvider<TTarget>.GetIcon(TTarget target)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/IInspectionIconProvider.cs#L20) | Resolves the presentation icon for one inspected target. |

### `Inno.Editor.Inspection.IInspectionPropertyEditService`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Inspection.IInspectionPropertyEditService`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/IInspectionPropertyEditService.cs#L8) | Applies one root serialized-property mutation through the history mechanism owned by a feature. |
| [`bool Inno.Editor.Inspection.IInspectionPropertyEditService.ChangeProperty(object owner, string propertyName, System.Action mutation, string historyName)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/IInspectionPropertyEditService.cs#L36) | Applies a property mutation and records it when the owning feature observes a value change. |

### `Inno.Editor.Inspection.IInspectorAttributeDrawer`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Inspection.IInspectorAttributeDrawer`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/IInspectorAttributeDrawer.cs#L6) | Extends serialized-property presentation with reusable attribute-driven behavior. |
| [`void Inno.Editor.Inspection.IInspectorAttributeDrawer.DrawAfter(Inno.Editor.Inspection.InspectorAttributeDrawContext context)`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/IInspectorAttributeDrawer.cs#L34) | Draws a decorator immediately after the property row. |
| [`void Inno.Editor.Inspection.IInspectorAttributeDrawer.DrawBefore(Inno.Editor.Inspection.InspectorAttributeDrawContext context)`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/IInspectorAttributeDrawer.cs#L24) | Draws a decorator immediately before the property row. |
| [`void Inno.Editor.Inspection.IInspectorAttributeDrawer.Update(Inno.Editor.Inspection.InspectorAttributeDrawContext context)`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/IInspectorAttributeDrawer.cs#L14) | Updates visibility, interactivity, labeling, help, or numeric constraints before layout. |

### `Inno.Editor.Inspection.IPropertyDrawer`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Inspection.IPropertyDrawer`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/IPropertyDrawer.cs#L6) | Draws and optionally edits one serialized property value. |
| [`void Inno.Editor.Inspection.IPropertyDrawer.Draw(Inno.Editor.Inspection.PropertyDrawContext context)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/IPropertyDrawer.cs#L14) | Draws a property through its encapsulated value accessors. |

### `Inno.Editor.Inspection.InspectionDrawContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Core.EditorContext Inno.Editor.Inspection.InspectionDrawContext.editorContext`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawContext.cs#L22) | Gets the shared editor context. |
| [`Inno.Editor.Inspection.InspectionDrawContext`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawContext.cs#L15) | Provides services and state to an inspector drawer. |
| [`Inno.Editor.Inspection.SerializedPropertyRenderer Inno.Editor.Inspection.InspectionDrawContext.properties`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawContext.cs#L37) | Gets the serialized property renderer. |
| [`Inno.Editor.Interactions.EditorInteractions Inno.Editor.Inspection.InspectionDrawContext.interactions`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawContext.cs#L27) | Gets the active editor interaction entry point. |
| [`T Inno.Editor.Inspection.InspectionDrawContext.GetProperty<T>(string propertyName)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawContext.cs#L94) | Reads one runtime-visible serialized property from the target. |
| [`bool Inno.Editor.Inspection.InspectionDrawContext.HasProperty(string propertyName)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawContext.cs#L75) | Gets whether the target exposes one runtime-visible serialized property. |
| [`bool Inno.Editor.Inspection.InspectionDrawContext.TryDrawInline(object target)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawContext.cs#L48) | Draws an exact target-specific body inside the caller's existing Inspector card. |
| [`object Inno.Editor.Inspection.InspectionDrawContext.target`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawContext.cs#L32) | Gets the selected target. |
| [`void Inno.Editor.Inspection.InspectionDrawContext.DrawProperties()`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawContext.cs#L128) | Draws every runtime-visible serialized property through the shared attribute and property drawer pipeline. |
| [`void Inno.Editor.Inspection.InspectionDrawContext.DrawProperty(string propertyName)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawContext.cs#L110) | Draws one named property with its ordinary editor, history, and validation behavior. |

### `Inno.Editor.Inspection.InspectionDrawer<TTarget>`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Inspection.InspectionDrawer<TTarget>`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawer.cs#L11) | Provides the target-specific header presentation and body drawing behavior for an inspected target. |
| [`abstract (string name, System.Action<string>? setter) Inno.Editor.Inspection.InspectionDrawer<TTarget>.BindName(Inno.Editor.Inspection.InspectionDrawContext context, TTarget target)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawer.cs#L45) | Binds the name displayed in the first row of the Inspector target header. |
| [`abstract string Inno.Editor.Inspection.InspectionDrawer<TTarget>.icon`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawer.cs#L17) | Gets the icon glyph displayed in the large leading slot of the Inspector target header. |
| [`abstract void Inno.Editor.Inspection.InspectionDrawer<TTarget>.Draw(Inno.Editor.Inspection.InspectionDrawContext context, TTarget target)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawer.cs#L95) | Draws the target-specific Inspector body below the shared target header. |
| [`virtual bool Inno.Editor.Inspection.InspectionDrawer<TTarget>.CanInspect(TTarget target)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawer.cs#L28) | Determines whether this drawer handles the selected value, in addition to its registered type. |
| [`virtual string Inno.Editor.Inspection.InspectionDrawer<TTarget>.GetIcon(Inno.Editor.Inspection.InspectionDrawContext context, TTarget target)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawer.cs#L62) | Resolves the icon glyph displayed for the current target. |
| [`virtual void Inno.Editor.Inspection.InspectionDrawer<TTarget>.DrawHeader(Inno.Editor.Inspection.InspectionDrawContext context, TTarget target)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawer.cs#L80) | Draws target-specific controls in the second row of the Inspector target header. |

### `Inno.Editor.Inspection.InspectionDrawerAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Inspection.InspectionDrawerAttribute`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawerAttribute.cs#L8) | Associates an inspector drawer with a selected target type. |
| [`Inno.Editor.Inspection.InspectionDrawerAttribute.InspectionDrawerAttribute(System.Type targetType, bool useForChildren = false, int priority = 0, bool conditional = false)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawerAttribute.cs#L49) | Creates an inspector drawer registration. |
| [`System.Type Inno.Editor.Inspection.InspectionDrawerAttribute.targetType`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawerAttribute.cs#L14) | Gets the selected target type handled by the drawer. |
| [`bool Inno.Editor.Inspection.InspectionDrawerAttribute.conditional`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawerAttribute.cs#L29) | Gets whether CanInspect selects a subset of targets, allowing peer conditional registrations at the same priority. |
| [`bool Inno.Editor.Inspection.InspectionDrawerAttribute.useForChildren`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawerAttribute.cs#L19) | Gets whether derived target types are accepted. |
| [`int Inno.Editor.Inspection.InspectionDrawerAttribute.priority`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawerAttribute.cs#L24) | Gets the tie-breaking registration priority. |

### `Inno.Editor.Inspection.InspectionDrawerFactory`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Inspection.InspectionDrawerFactory`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawerFactory.cs#L17) | Creates one discovered inspection drawer through the owning editor composition root. |

### `Inno.Editor.Inspection.InspectionDrawerRegistry`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Inspection.InspectionDrawerRegistry`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawerRegistry.cs#L16) | Discovers and resolves inspection drawers through the active type catalog. |
| [`Inno.Editor.Inspection.InspectionDrawerRegistry.InspectionDrawerRegistry(Inno.Editor.Interactions.EditorInteractions interactions, Inno.Editor.Inspection.InspectionDrawerFactory factory, Inno.Extensibility.Types.TypeCatalog types, Inno.Core.Serialization.SerializationRegistry serialization)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawerRegistry.cs#L41) | Creates a generation-aware inspection drawer registry. |
| [`bool Inno.Editor.Inspection.InspectionDrawerRegistry.TryResolve(Inno.Editor.Core.EditorContext editorContext, object target, Inno.Editor.Inspection.SerializedPropertyRenderer renderer, out Inno.Editor.Inspection.IInspectionDrawer? drawer, out Inno.Editor.Inspection.InspectionDrawContext? context)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawerRegistry.cs#L76) | Resolves the most specific registered drawer and creates its drawing context. |
| [`bool Inno.Editor.Inspection.InspectionDrawerRegistry.TryResolveExact(Inno.Editor.Core.EditorContext editorContext, object target, Inno.Editor.Inspection.SerializedPropertyRenderer renderer, out Inno.Editor.Inspection.IInspectionDrawer? drawer, out Inno.Editor.Inspection.InspectionDrawContext? context)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawerRegistry.cs#L127) | Resolves only a drawer explicitly registered for the target's exact runtime type. |
| [`void Inno.Editor.Inspection.InspectionDrawerRegistry.Dispose()`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawerRegistry.cs#L161) | Releases every active drawer snapshot and unregisters the registry from type refreshes. |

### `Inno.Editor.Inspection.InspectorAttributeDrawContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Serialization.SerializedProperty? Inno.Editor.Inspection.InspectorAttributeDrawContext.property`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawContext.cs#L45) | Gets the root serialized property when one is available. |
| [`Inno.Editor.Inspection.InspectorAttributeDrawContext`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawContext.cs#L10) | Carries the mutable presentation state of one attribute-annotated Inspector property. |
| [`System.Attribute Inno.Editor.Inspection.InspectorAttributeDrawContext.attribute`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawContext.cs#L55) | Gets the attribute currently invoking a drawer. |
| [`System.Reflection.MemberInfo Inno.Editor.Inspection.InspectorAttributeDrawContext.member`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawContext.cs#L40) | Gets the reflected field or property carrying the current attribute. |
| [`bool Inno.Editor.Inspection.InspectorAttributeDrawContext.isReadOnly`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawContext.cs#L65) | Gets or sets whether the property control is read-only in the Inspector. |
| [`bool Inno.Editor.Inspection.InspectorAttributeDrawContext.isVisible`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawContext.cs#L60) | Gets or sets whether the complete property row is visible. |
| [`double? Inno.Editor.Inspection.InspectorAttributeDrawContext.maximum`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawContext.cs#L85) | Gets or sets an optional inclusive numeric upper bound. |
| [`double? Inno.Editor.Inspection.InspectorAttributeDrawContext.minimum`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawContext.cs#L80) | Gets or sets an optional inclusive numeric lower bound. |
| [`object Inno.Editor.Inspection.InspectorAttributeDrawContext.owner`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawContext.cs#L35) | Gets the object that owns the annotated member. |
| [`object? Inno.Editor.Inspection.InspectorAttributeDrawContext.GetSiblingValue(string memberName)`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawContext.cs#L99) | Reads a sibling field or property from the current owner, including non-public base members. |
| [`string Inno.Editor.Inspection.InspectorAttributeDrawContext.label`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawContext.cs#L70) | Gets or sets the visible property label. |
| [`string Inno.Editor.Inspection.InspectorAttributeDrawContext.path`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawContext.cs#L50) | Gets the stable Inspector control path. |
| [`string Inno.Editor.Inspection.InspectorAttributeDrawContext.tooltip`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawContext.cs#L75) | Gets or sets optional hover help for the property label. |

### `Inno.Editor.Inspection.InspectorAttributeDrawerAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Inspection.InspectorAttributeDrawerAttribute`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawerAttribute.cs#L8) | Associates an Inspector attribute drawer with an attribute type. |
| [`Inno.Editor.Inspection.InspectorAttributeDrawerAttribute.InspectorAttributeDrawerAttribute(System.Type targetType, bool useForChildren = false, int priority = 0)`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawerAttribute.cs#L23) | Creates an Inspector attribute drawer registration. |
| [`System.Type Inno.Editor.Inspection.InspectorAttributeDrawerAttribute.targetType`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawerAttribute.cs#L39) | Gets the handled attribute type. |
| [`bool Inno.Editor.Inspection.InspectorAttributeDrawerAttribute.useForChildren`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawerAttribute.cs#L44) | Gets whether derived attribute types are accepted. |
| [`int Inno.Editor.Inspection.InspectorAttributeDrawerAttribute.priority`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawerAttribute.cs#L49) | Gets the registration priority. |

### `Inno.Editor.Inspection.InspectorAttributeDrawerRegistry`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Inspection.InspectorAttributeDrawerRegistry`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawerRegistry.cs#L14) | Discovers generation-aware Inspector attribute drawers and applies them in declaration order. |
| [`Inno.Editor.Inspection.InspectorAttributeDrawerRegistry.InspectorAttributeDrawerRegistry(Inno.Editor.Interactions.EditorInteractions interactions, Inno.Extensibility.Types.TypeCatalog types, Inno.Core.Serialization.SerializationRegistry serialization, System.Collections.Generic.IEnumerable<object> drawerServices)`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawerRegistry.cs#L33) | Creates an Inspector attribute drawer registry. |
| [`void Inno.Editor.Inspection.InspectorAttributeDrawerRegistry.Dispose()`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawerRegistry.cs#L88) | Releases active drawer generations. |

### `Inno.Editor.Inspection.PropertyDrawContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Serialization.PropertyVisibility Inno.Editor.Inspection.PropertyDrawContext.visibility`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L50) | Gets the serialization visibility flags. |
| [`Inno.Editor.Core.EditorContext Inno.Editor.Inspection.PropertyDrawContext.editorContext`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L25) | Gets the shared editor context. |
| [`Inno.Editor.Inspection.PropertyDrawContext`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L12) | Encapsulates one editable property path and its drawing services. |
| [`Inno.Editor.Interactions.EditorInteractions Inno.Editor.Inspection.PropertyDrawContext.interactions`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L30) | Gets the active editor interaction entry point. |
| [`System.Type Inno.Editor.Inspection.PropertyDrawContext.propertyType`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L45) | Gets the declared property type. |
| [`bool Inno.Editor.Inspection.PropertyDrawContext.TryGetTextState(string key, out string? value)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L139) | Tries to read text-edit state scoped to this renderer, inspected owner, and property path. |
| [`bool Inno.Editor.Inspection.PropertyDrawContext.hdrColor`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L75) | Gets whether Color values are unclamped linear floating-point channels. |
| [`bool Inno.Editor.Inspection.PropertyDrawContext.isReadOnly`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L55) | Gets whether assignments are disabled. |
| [`double? Inno.Editor.Inspection.PropertyDrawContext.maximum`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L65) | Gets the optional inclusive numeric upper bound supplied by Inspector metadata. |
| [`double? Inno.Editor.Inspection.PropertyDrawContext.minimum`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L60) | Gets the optional inclusive numeric lower bound supplied by Inspector metadata. |
| [`object? Inno.Editor.Inspection.PropertyDrawContext.GetValue()`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L125) | Gets the latest value. |
| [`string Inno.Editor.Inspection.PropertyDrawContext.label`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L40) | Gets the display label. |
| [`string Inno.Editor.Inspection.PropertyDrawContext.path`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L35) | Gets the stable control path. |
| [`string? Inno.Editor.Inspection.PropertyDrawContext.tooltip`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L70) | Gets optional hover help supplied by Inspector metadata. |
| [`void Inno.Editor.Inspection.PropertyDrawContext.ClearTextState(string key)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L164) | Removes text-edit state for this renderer, inspected owner, and property path. |
| [`void Inno.Editor.Inspection.PropertyDrawContext.DrawChild(Inno.Core.Serialization.SerializedProperty property)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L235) | Draws a nested serialized property. |
| [`void Inno.Editor.Inspection.PropertyDrawContext.DrawChild(object metadataOwner, Inno.Core.Serialization.SerializedProperty property)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L246) | Draws a nested serialized property with presentation metadata resolved from its immediate owner. |
| [`void Inno.Editor.Inspection.PropertyDrawContext.DrawChild(string childName, System.Type childType, System.Func<object?> getter, System.Action<object?> setter, bool readOnly = false)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L203) | Draws a nested value whose setter writes through this property path. |
| [`void Inno.Editor.Inspection.PropertyDrawContext.DrawInlineChild(string childName, System.Type childType, System.Func<object?> getter, System.Action<object?> setter, bool readOnly = false)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L315) | Draws a nested value inline without adding another label row. |
| [`void Inno.Editor.Inspection.PropertyDrawContext.SetTextState(string key, string value)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L153) | Stores text-edit state scoped to this renderer, inspected owner, and property path. |
| [`void Inno.Editor.Inspection.PropertyDrawContext.SetValue(object? value)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L172) | Assigns a value when the property is writable. |

### `Inno.Editor.Inspection.PropertyDrawerAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Inspection.PropertyDrawerAttribute`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawerAttribute.cs#L8) | Associates a property drawer with a declared property type. |
| [`Inno.Editor.Inspection.PropertyDrawerAttribute.PropertyDrawerAttribute(System.Type targetType, bool useForChildren = false, int priority = 0)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawerAttribute.cs#L41) | Creates a property drawer registration. |
| [`System.Type Inno.Editor.Inspection.PropertyDrawerAttribute.targetType`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawerAttribute.cs#L14) | Gets the declared property type handled by the drawer. |
| [`bool Inno.Editor.Inspection.PropertyDrawerAttribute.useForChildren`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawerAttribute.cs#L19) | Gets whether assignable derived types are accepted. |
| [`int Inno.Editor.Inspection.PropertyDrawerAttribute.priority`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawerAttribute.cs#L24) | Gets the tie-breaking registration priority. |

### `Inno.Editor.Inspection.PropertyDrawerRegistry`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Inspection.IPropertyDrawer Inno.Editor.Inspection.PropertyDrawerRegistry.Resolve(System.Type propertyType)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawerRegistry.cs#L58) | Resolves the most specific drawer for a declared property type. |
| [`Inno.Editor.Inspection.PropertyDrawerRegistry`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawerRegistry.cs#L15) | Discovers and resolves serialized property drawers through the active type catalog. |
| [`Inno.Editor.Inspection.PropertyDrawerRegistry.PropertyDrawerRegistry(Inno.Editor.Interactions.EditorInteractions interactions, Inno.Extensibility.Types.TypeCatalog types, Inno.Core.Serialization.SerializationRegistry serialization, System.Collections.Generic.IEnumerable<object> drawerServices)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawerRegistry.cs#L37) | Creates a generation-aware property drawer registry. |
| [`void Inno.Editor.Inspection.PropertyDrawerRegistry.Dispose()`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawerRegistry.cs#L67) | Releases every active property drawer snapshot and unregisters the registry from type refreshes. |

### `Inno.Editor.Inspection.SerializedPropertyRenderer`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Inspection.SerializedPropertyRenderer`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/SerializedPropertyRenderer.cs#L21) | Resolves drawers and renders serialized property paths with isolated error handling. |
| [`Inno.Editor.Inspection.SerializedPropertyRenderer.SerializedPropertyRenderer(Inno.Editor.Inspection.PropertyDrawerRegistry drawers, Inno.Editor.Inspection.InspectorAttributeDrawerRegistry attributes, Inno.Editor.Interactions.EditorInteractions interactions, Inno.Editor.Inspection.IInspectionPropertyEditService edits, Inno.Core.Logging.LogRouter logs)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/SerializedPropertyRenderer.cs#L53) | Creates a serialized property renderer over one drawer registry and feature-owned edit service. |
| [`void Inno.Editor.Inspection.SerializedPropertyRenderer.Draw(Inno.Editor.Core.EditorContext editorContext, object owner, string ownerPath, Inno.Core.Serialization.SerializedProperty property)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/SerializedPropertyRenderer.cs#L84) | Draws a root serialized property. |
| [`void Inno.Editor.Inspection.SerializedPropertyRenderer.DrawDraftProperty(Inno.Editor.Core.EditorContext editorContext, object stateOwner, object valueOwner, string ownerPath, Inno.Core.Serialization.SerializedProperty property, Inno.Editor.Inspection.IInspectionPropertyEditService edits, bool readOnly = false)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/SerializedPropertyRenderer.cs#L194) | Draws a detached serialized property with its attributes and a feature-owned draft transaction. |
| [`void Inno.Editor.Inspection.SerializedPropertyRenderer.DrawValue(Inno.Editor.Core.EditorContext editorContext, object owner, string path, string label, System.Type propertyType, System.Func<object?> getter, System.Action<object?> setter, Inno.Editor.Inspection.IInspectionPropertyEditService edits, bool readOnly = false, bool hdrColor = false, string? tooltip = null, double? minimum = null, double? maximum = null)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/SerializedPropertyRenderer.cs#L150) | Draws a declared authoring value with existing property drawers and a feature-owned draft edit service. |

## 项目依赖

- [Inno.Editor.ImGui](Inno.Editor.ImGui.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Mathematics](../core/Inno.Core.Mathematics.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Presentation.ImGui.Sdl3](../backends/ImGui/Inno.Adapter.Presentation.ImGui.Sdl3.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Native.ImGui](../backends/ImGui/Inno.Native.ImGui.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Editor.Annotations](Inno.Editor.Annotations.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Editor.Core](Inno.Editor.Core.md)：公开引用边界由实际签名核对。
- [Inno.Editor.Interactions](Inno.Editor.Interactions.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
- [Inno.Core.Logging](../core/Inno.Core.Logging.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：公开引用边界由实际签名核对。
