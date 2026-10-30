# Inno.Editor.Inspection

[Editor 索引](README.md) · [Inspector Panel](Inno.Editor.Panel.Inspector.md) · [Interactions](Inno.Editor.Interactions.md)

该项目是与具体 Panel 无关的检查绘制基础设施。它拥有 `InspectionDrawer<TTarget>`、`PropertyDrawer` 契约、TypeCache Registry、draw context、serialized property renderer 以及 bool、number、string、enum、collection、struct、nullable、math 等通用内建 PropertyDrawer。

`Inno.Editor.Panel.Inspector` 只负责窗口、统一 Target Header 和 Scene 的 Component/System 操作；其他 feature 不需要引用 Inspector Panel。Feature project 只需引用 `Inno.Editor.Inspection`，把自己的内部 Drawer 与目标类型放在同一个项目中，TypeCache 会自动发现它。

## InspectionDrawer

```csharp
using System;
using Inno.Assets;
using Inno.Editor.Inspection;

[InspectionDrawer(typeof(TextAsset), priority: 100)]
public sealed class TextAssetDrawer : InspectionDrawer<TextAsset>
{
    public override string icon => "T";

    protected override (string name, Action<string>? setter) BindName(
        InspectionDrawContext context,
        TextAsset target
    ) => (target.name, null);

    protected override void Draw(
        InspectionDrawContext context,
        TextAsset target
    ) {
        context.DrawProperties();
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
using Inno.Editor.ImGui;
using Inno.Editor.ImGui.ImGuiWidget;

public readonly record struct AuthoringWeight(float value);

[PropertyDrawer(typeof(AuthoringWeight), priority: 100)]
public sealed class AuthoringWeightDrawer : IPropertyDrawer
{
    public void Draw(PropertyDrawContext context)
    {
        float value = context.GetValue() is AuthoringWeight current ? current.value : 0f;
        if (EditorUiSession.current.widgets.CompactSliderFloat($"##{context.path}", ref value, 0f, 1f))
            context.SetValue(new AuthoringWeight(value));
    }
}
```

PropertyDrawer 通过 declared property type 匹配。`PropertyDrawContext.SetValue` 会把修改交给所属 feature 的 edit service，由它写入中立的 Undo/Redo payload；Drawer 不应该绕过 context 直接修改 serialized owner。`PropertyDrawContext.tooltip` 暴露已经组合完成的 hover help；bool 与 nullable 内建 Drawer 会把它同时绑定到 checkbox 本身，因此 label 和 control 都能解释同一属性。

`SerializedPropertyRenderer` 本身不依赖 Scene。创建 renderer 的 feature 提供 `IInspectionPropertyEditService`，负责把通用的 owner、root property 与 mutation 转换成自己的 Undo/Redo 协议。Inspector Panel 使用 Scene adapter；Material、Pipeline 和插件检查器使用各自的 history adapter，而不用把 Scene 引入通用 Inspection 项目。

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
using InnoEngine.Serialization;
using InnoEditor.Annotations;

public sealed class AuthoredValues : ISerializable
{
    [SerializableProperty]
    [Text("This message remains visible above the control.")]
    public float authoredValue { get; set; }
}
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

`IInspectionIconProvider<TTarget>` 用于共享目标所属 feature 的图标规则，而不引入 Panel→Panel 引用。例如 FileBrowser 的 `AssetEditingModule` 实现 `IInspectionIconProvider<AssetFileEntry>`，它自己的 `AssetSelectionInspectionDrawer` 因而可以复用同一个 Asset icon registry。Inspector 组合根只依赖该接口，不引用 FileBrowser 项目。Scene/GameObject Drawer 直接注入 `EditorSettings`，通过原始完整路径读取与 Hierarchy、Asset Browser 一致的 icon object；Settings 项目不提供 icon resolver。

## Scripting API

EditorScripts 通过显式 `using InnoEditor.Inspection;` 使用裁剪后的 `InspectionDrawer`、`PropertyDrawer`、`InspectorAttributeDrawer`、`IInspectionIconProvider<TTarget>` 与 property-edit 契约。`AssetFileEntry` 条件 Drawer 因而能从 File Browser 注入统一图标解析，而不复制 Appearance 设置。Registry、snapshot、Activator 和各 feature 的具体 mutation adapter 不导出。

## 当前源码公开 API 清单

此清单对应当前 SDK 求值与源码符号；内部类型不构成公开 API。

<!-- api: T:Inno.Editor.Inspection.IInspectorAttributeDrawer 18644eb38577f1b6e96cf1574c157d2846a5be2aabc23cd4b230d6827f6af42f -->
<!-- api: M:Inno.Editor.Inspection.IInspectorAttributeDrawer.Update(Inno.Editor.Inspection.InspectorAttributeDrawContext) e93e658937bc57ade26263ce2f0a1a308013fd73297d0f3220cd8d4b864be04c -->
<!-- api: M:Inno.Editor.Inspection.IInspectorAttributeDrawer.DrawBefore(Inno.Editor.Inspection.InspectorAttributeDrawContext) e66003ec24b3738ee7b55f861bd4c72a0056cc4ab277f427d698e2523f3a7aee -->
<!-- api: M:Inno.Editor.Inspection.IInspectorAttributeDrawer.DrawAfter(Inno.Editor.Inspection.InspectorAttributeDrawContext) 2eef14833aeaed6845a55a4c16c31650022393744eb49ee163c62a52ee0f5d60 -->
<!-- api: T:Inno.Editor.Inspection.InspectorAttributeDrawContext ef5afeabb0c7f89de3412a33d1a8ce707158e0e133d120744cec873c0c571692 -->
<!-- api: P:Inno.Editor.Inspection.InspectorAttributeDrawContext.owner 33bde627abf779ee9dbdbf9747ea9205b01e443cc28cb83dc8fc8f67aa7588c1 -->
<!-- api: P:Inno.Editor.Inspection.InspectorAttributeDrawContext.member 2a25c3215e481a57a41d0c52f82e82f7c364ee79917dc04eab9bac30c3950234 -->
<!-- api: P:Inno.Editor.Inspection.InspectorAttributeDrawContext.property 32bbe473b7c4d6278a1a9d0037f7aff8715bea2fb17a5bc7f9d909c6ba03a9b4 -->
<!-- api: P:Inno.Editor.Inspection.InspectorAttributeDrawContext.path 94c997fd6d632d82e3466d626ac4d68e78f10f501e3978374a9f5e732bac43a1 -->
<!-- api: P:Inno.Editor.Inspection.InspectorAttributeDrawContext.attribute bde489f50a3507aa616c048377637cce937e7023d15e83e4a89ba1027b857ca7 -->
<!-- api: P:Inno.Editor.Inspection.InspectorAttributeDrawContext.isVisible 817fc1b0bc24dccec70fa07b6cf226ba7bb52cf46a70ba477bc9371d3441ac4e -->
<!-- api: P:Inno.Editor.Inspection.InspectorAttributeDrawContext.isReadOnly be7f60ba3ad12d73e9ae1881a50681a258d0c93544a6cb18c9eb4b0fe4978413 -->
<!-- api: P:Inno.Editor.Inspection.InspectorAttributeDrawContext.label 25c677d4e53e196d83f39420689ba79edc8e5f21a62a8eae2ff221cb52be6635 -->
<!-- api: P:Inno.Editor.Inspection.InspectorAttributeDrawContext.tooltip dbf860298667006ab03a515228e56aab2123bad7c639a4ba63b75a4f9cfcbccc -->
<!-- api: P:Inno.Editor.Inspection.InspectorAttributeDrawContext.minimum b70ac149122bb62e82a5de31f747472cd0f257fea3d8fdf2b34f3ad23a5bcffd -->
<!-- api: P:Inno.Editor.Inspection.InspectorAttributeDrawContext.maximum 084ac41f988078f15028f7282377540043b5045a24cce8288a3028887fab8a64 -->
<!-- api: M:Inno.Editor.Inspection.InspectorAttributeDrawContext.GetSiblingValue(System.String) 42c775c2a4402341300c36d5d3883e6d5bfc56633e0ea2135c43d9cb76bb5df1 -->
<!-- api: T:Inno.Editor.Inspection.InspectorAttributeDrawerAttribute 4ca42ec066c8940d14ff54ec2b0b68e8c0f7bbba38c5cce5eeb4ce4b5e1672bf -->
<!-- api: M:Inno.Editor.Inspection.InspectorAttributeDrawerAttribute.#ctor(System.Type,System.Boolean,System.Int32) 61d3f71254d953901ed71e3804bfb443ff1c3c154e49bb0b25b203e97c7f5d43 -->
<!-- api: P:Inno.Editor.Inspection.InspectorAttributeDrawerAttribute.targetType 8ea6fc5436d21415cab20ee7ed317ad204b2fa7fedc7665e30dd46a67b8270a7 -->
<!-- api: P:Inno.Editor.Inspection.InspectorAttributeDrawerAttribute.useForChildren daaede29bf6114d4015afc43cc5cdf3921498c33ef0b86276e0a1281a77f9de5 -->
<!-- api: P:Inno.Editor.Inspection.InspectorAttributeDrawerAttribute.priority f74f154e380d819a089cdd0f3f7d33badc9e0263a5f8032c46009b0d9cd967c2 -->
<!-- api: T:Inno.Editor.Inspection.InspectorAttributeDrawerRegistry f24acc3509fa71891448561501bc7b8e7b923d529d0a20471adfb4f76cf8e718 -->
<!-- api: M:Inno.Editor.Inspection.InspectorAttributeDrawerRegistry.#ctor(Inno.Editor.Interactions.EditorInteractions,Inno.Extensibility.Types.TypeCatalog,Inno.Core.Serialization.SerializationRegistry,System.Collections.Generic.IEnumerable{System.Object}) 908ac77fead43e73e0caa00683e6e3995fa36722e73f64441c0cb2d0f3bb900c -->
<!-- api: M:Inno.Editor.Inspection.InspectorAttributeDrawerRegistry.Dispose a273db182fbbb3c42314fa2347b8d8f2cf947ea00c416cff4992f6287530b728 -->
<!-- api: T:Inno.Editor.Inspection.IInspectionIconProvider`1 414ee9b41893705002f4736ac76b1e236eca4163eb8fb9e6b4f51ee3d645c0cb -->
<!-- api: M:Inno.Editor.Inspection.IInspectionIconProvider`1.GetIcon(`0) 51a64cd8b8def812bb789f56c432beec9a15d848064c55b661a88a2ac91092a5 -->
<!-- api: T:Inno.Editor.Inspection.InspectionDrawContext 3351d5a657b34a4c4adfe4e98b32f2a1086b0b18aa8531a2517380ae98d39df3 -->
<!-- api: P:Inno.Editor.Inspection.InspectionDrawContext.editorContext cd113e1468e2c950fcfd3aeac829f0829a97289145906cbc60a66b3c0acbf590 -->
<!-- api: P:Inno.Editor.Inspection.InspectionDrawContext.interactions abbceb44b7af736d29a49f612ada01570b7ec2b73c6fc1b3ab28ade7c6a94d2f -->
<!-- api: P:Inno.Editor.Inspection.InspectionDrawContext.target c29de2b4588e0253cce06859ba4b5b7766e2d020f6dd1924fdb0e041d4223955 -->
<!-- api: P:Inno.Editor.Inspection.InspectionDrawContext.properties dd8ff7308719f6951051e2f7e86a180c43223d54e93f8e61a656fc05542d333d -->
<!-- api: M:Inno.Editor.Inspection.InspectionDrawContext.TryDrawInline(System.Object) 14fde7e67b68158d3802ac25095c5133463249ef99ac5c886230fec305680626 -->
<!-- api: M:Inno.Editor.Inspection.InspectionDrawContext.HasProperty(System.String) b1167e9f171d548561467aae727bd358a0acd7ba15efc15d5204e56ecf465a70 -->
<!-- api: M:Inno.Editor.Inspection.InspectionDrawContext.GetProperty``1(System.String) c6119d8b900d09ff2cf9ffae24a728dcc8f4b50b6e44cc7d49bc9556c584e733 -->
<!-- api: M:Inno.Editor.Inspection.InspectionDrawContext.DrawProperty(System.String) 269e8d791af0952974cdd843308b066d0c32e38915c9aecc8b79473d1ad0e4fb -->
<!-- api: M:Inno.Editor.Inspection.InspectionDrawContext.DrawProperties 7d35416211bad59beb37a6ab04ea3d86226d2aa5d2679947fcd9f91203c16a11 -->
<!-- api: T:Inno.Editor.Inspection.InspectionDrawer`1 a94ec9faad7c4118bbf06db23f440f8036a3bb6788bd1909f7d18b8b3af633db -->
<!-- api: P:Inno.Editor.Inspection.InspectionDrawer`1.icon 045b901b9d2cd68c6496cfc8207a2f6c37545c989d32d617d8bf9e26765c62c5 -->
<!-- api: M:Inno.Editor.Inspection.InspectionDrawer`1.CanInspect(`0) 031cd141abdaf74f73f1a7ccd0a87955ce4b32974dd34cc829aea4593eb47ed4 -->
<!-- api: M:Inno.Editor.Inspection.InspectionDrawer`1.BindName(Inno.Editor.Inspection.InspectionDrawContext,`0) 596bd733153cd129d1c93d5c17fc0305a5af18fb3fe590c9368568e456562217 -->
<!-- api: M:Inno.Editor.Inspection.InspectionDrawer`1.GetIcon(Inno.Editor.Inspection.InspectionDrawContext,`0) 61e519ed468b28e671fe7eba3c981a8c380f91cd831d967826df869b3d4b077d -->
<!-- api: M:Inno.Editor.Inspection.InspectionDrawer`1.DrawHeader(Inno.Editor.Inspection.InspectionDrawContext,`0) ee3fb714105107840b82f67b52f63cfde4e644593b40374393440556b37a343e -->
<!-- api: M:Inno.Editor.Inspection.InspectionDrawer`1.Draw(Inno.Editor.Inspection.InspectionDrawContext,`0) c6d8cbc82cb4bb24243af3f7c4d048a5a1c7f349e094646bd81a7a51b8606fd2 -->
<!-- api: M:Inno.Editor.Inspection.InspectionDrawer`1.#ctor 4e27e1ec42ee2e675757055f32f961e016947453e8611928c2902b414ad2cd7e -->
<!-- api: T:Inno.Editor.Inspection.IInspectionDrawer aae41da311a16b5b343a2f7a33902ec5c645a63bb1f1f64a6d7fc866f378611c -->
<!-- api: M:Inno.Editor.Inspection.IInspectionDrawer.CanInspect(System.Object) 5b59d278fb1100909e8200a4dd7d3895a7bb530011d4163367c3553f7be1bcf0 -->
<!-- api: M:Inno.Editor.Inspection.IInspectionDrawer.GetIcon(Inno.Editor.Inspection.InspectionDrawContext) 62f346be27e61598fd0864e10a785e4d9694bb338eb7faea715b2c77232a946e -->
<!-- api: M:Inno.Editor.Inspection.IInspectionDrawer.BindName(Inno.Editor.Inspection.InspectionDrawContext) 48e887e263b956fde7449669bf4f34c26da4b31f4640ec5e49178046888c98fc -->
<!-- api: M:Inno.Editor.Inspection.IInspectionDrawer.DrawHeader(Inno.Editor.Inspection.InspectionDrawContext) 7487ac96f8ec28a45f5852a30beaefcd27910c7d8b57ba2fa171528ecb5fa9a2 -->
<!-- api: M:Inno.Editor.Inspection.IInspectionDrawer.Draw(Inno.Editor.Inspection.InspectionDrawContext) f806d5e907d3fe903c0cbe20b17dbd6a1d9bc1a3d2ae33c11aceefd360663e71 -->
<!-- api: T:Inno.Editor.Inspection.InspectionDrawerAttribute 10a8e7fe6ddf9ddbdbf5fde964ed9b29b424cc14852d716e9a98b7f2bdd7c6b2 -->
<!-- api: P:Inno.Editor.Inspection.InspectionDrawerAttribute.targetType 99506bbd72ce8ebcbb84c47ad4f011aa991a2cb9ba7ad6a9c755859ee9f0b50e -->
<!-- api: P:Inno.Editor.Inspection.InspectionDrawerAttribute.useForChildren 465c12479d14202e2ff88fc7ab2325daa9cc398aa31741e5aae13a2ccbc2cb72 -->
<!-- api: P:Inno.Editor.Inspection.InspectionDrawerAttribute.priority 902d554ab25358c34e638f423c989fadcbd2d0ff18eb7587727da7b8466b0eda -->
<!-- api: P:Inno.Editor.Inspection.InspectionDrawerAttribute.conditional 2a7ab567832699903457f5503d334dbb90d2b6cf2efe59f4e70f9ddb49e75774 -->
<!-- api: M:Inno.Editor.Inspection.InspectionDrawerAttribute.#ctor(System.Type,System.Boolean,System.Int32,System.Boolean) 3d55ad7bb48f25fd6624f020043d032da1e60d51d66c104b1ec26850788af76c -->
<!-- api: T:Inno.Editor.Inspection.InspectionDrawerFactory c9bee7861e5d06f2273f385bd98ec7091545b07f7eca3388804baaf0394b9823 -->
<!-- api: M:Inno.Editor.Inspection.InspectionDrawerFactory.#ctor(System.Object,System.IntPtr) 4884a5d1e7243975856b1cc39276b83540223965f455d94d6898dd2e183c99b9 -->
<!-- api: M:Inno.Editor.Inspection.InspectionDrawerFactory.Invoke(System.Type) d02237d6d8ca6216dced21511443dbbe500d553768ae30db59520b3af3f773bd -->
<!-- api: M:Inno.Editor.Inspection.InspectionDrawerFactory.BeginInvoke(System.Type,System.AsyncCallback,System.Object) 2caf336145e34748da53034706780d9a0d9f0b01491eeb4bd89ab045518dae25 -->
<!-- api: M:Inno.Editor.Inspection.InspectionDrawerFactory.EndInvoke(System.IAsyncResult) 1d692387b1dd8a9ced7855e4a60a527b3571aa7da5407bd7408ecfefbc632d55 -->
<!-- api: T:Inno.Editor.Inspection.InspectionDrawerRegistry d1606c4cdca829f947673fec247513eaf770605e58d37534ce3059521173d73d -->
<!-- api: M:Inno.Editor.Inspection.InspectionDrawerRegistry.#ctor(Inno.Editor.Interactions.EditorInteractions,Inno.Editor.Inspection.InspectionDrawerFactory,Inno.Extensibility.Types.TypeCatalog,Inno.Core.Serialization.SerializationRegistry) ad14ed100b563f08aa44c13c62c43360190bc969b8c59d2232911354dffe27af -->
<!-- api: M:Inno.Editor.Inspection.InspectionDrawerRegistry.TryResolve(Inno.Editor.Core.EditorContext,System.Object,Inno.Editor.Inspection.SerializedPropertyRenderer,Inno.Editor.Inspection.IInspectionDrawer@,Inno.Editor.Inspection.InspectionDrawContext@) 78a53b7e3be187776fcc87a63de52a277c4749ced9e5dd345a1d3a24d560b48b -->
<!-- api: M:Inno.Editor.Inspection.InspectionDrawerRegistry.TryResolveExact(Inno.Editor.Core.EditorContext,System.Object,Inno.Editor.Inspection.SerializedPropertyRenderer,Inno.Editor.Inspection.IInspectionDrawer@,Inno.Editor.Inspection.InspectionDrawContext@) 9592a630564b636f8fd155d0ae14bbe85c02bb5f57e5cf1a49e951f31c8600f9 -->
<!-- api: M:Inno.Editor.Inspection.InspectionDrawerRegistry.Dispose 5ac770056e3cb797295910b17b71886aca1ca406772aff358a1105d24742e805 -->
<!-- api: T:Inno.Editor.Inspection.IInspectionPropertyEditService 8ceb2f6119943474d6163fe8201f1437d96c6f6d7c816da8f0707323912210e0 -->
<!-- api: M:Inno.Editor.Inspection.IInspectionPropertyEditService.ChangeProperty(System.Object,System.String,System.Action,System.String,System.String,System.String) ecb4fa76aeb0c5fac0debb6f06c99c84d8f608747a9acfcf3bd4ee57930e7cf4 -->
<!-- api: T:Inno.Editor.Inspection.IPropertyDrawer 919dfdbb1c668b339f2af2cc08e4cf4ae72d0e00a07490bb872272f71e30a886 -->
<!-- api: M:Inno.Editor.Inspection.IPropertyDrawer.Draw(Inno.Editor.Inspection.PropertyDrawContext) ce95979753e7ca49b9685f3ea27d06dadd0d71f272bfd2ff9375a78eb5f2e5c3 -->
<!-- api: T:Inno.Editor.Inspection.PropertyDrawContext cf4186b369c8a8a5d910430ac6dd56ba28fd5a810285fbce258cea9ab14ee801 -->
<!-- api: P:Inno.Editor.Inspection.PropertyDrawContext.editorContext 542ff8f08346ff62124498f4bea0f0398acbb2b4bad8600d9a4d95673d3db28e -->
<!-- api: P:Inno.Editor.Inspection.PropertyDrawContext.interactions 166e298112fd4f8f6fae874a19306d3dc371c209174222229c527cd88410f65e -->
<!-- api: P:Inno.Editor.Inspection.PropertyDrawContext.path 82e8b0d625b397a265a8d72691048a8cd50870f5c18651e1b7a4525a398f355a -->
<!-- api: P:Inno.Editor.Inspection.PropertyDrawContext.label b3a301f4c22a89fb5b34f992127158a3c9e029b86727534f9c0f5ff7728b197e -->
<!-- api: P:Inno.Editor.Inspection.PropertyDrawContext.propertyType e558a1310010d81eb22388cd6b493ca0d0e71966e426164e2bda7c538357859e -->
<!-- api: P:Inno.Editor.Inspection.PropertyDrawContext.visibility f07a0dbb1e91f5571da1fe9b303186fa7d27314e734f817370a6821421248b70 -->
<!-- api: P:Inno.Editor.Inspection.PropertyDrawContext.isReadOnly e6c6bdf52b43eba2541aa3681520e9906cea50bce406df85d72292b4f287cd61 -->
<!-- api: P:Inno.Editor.Inspection.PropertyDrawContext.minimum f64b7760a6c0939dab8c3577fbaef0c042142a010769800b788ef75e4134aeac -->
<!-- api: P:Inno.Editor.Inspection.PropertyDrawContext.maximum b49c51a44b54207a3c399acfd7e7bfa91ff286a3dffc297f2b87a439c1eb6e4e -->
<!-- api: P:Inno.Editor.Inspection.PropertyDrawContext.tooltip e9ec5739e80c55dd6543e849b2a4533f874cd4416e55fa71aa7cc639f652bc61 -->
<!-- api: P:Inno.Editor.Inspection.PropertyDrawContext.hdrColor a39274d009d37343a477c17cc05029f0ab9be4f183ab4e70bfd56abcbf48c066 -->
<!-- api: M:Inno.Editor.Inspection.PropertyDrawContext.GetValue dc2e1642ef50ceef19fb0499f9458a72a116df5f0ec50acf4a269aaba8675e87 -->
<!-- api: M:Inno.Editor.Inspection.PropertyDrawContext.TryGetTextState(System.String,System.String@) 8fe0f6d8962fb106d04c0204395447ef744915d11f632eb988dd407b89bc51b5 -->
<!-- api: M:Inno.Editor.Inspection.PropertyDrawContext.SetTextState(System.String,System.String) 6938c87ae4ccc340b58101e1747ae052bff64979681b4a97e2a9a0d5b6fcc6e7 -->
<!-- api: M:Inno.Editor.Inspection.PropertyDrawContext.ClearTextState(System.String) 1b97a0edb5709e0699f94c9f80eee87be16884afed9f7ae73b91452bf98dd696 -->
<!-- api: M:Inno.Editor.Inspection.PropertyDrawContext.SetValue(System.Object,System.String) 74c599f85dd35f7511cd5643df95bbf79a441d8f27df6883f783a19d8a3565c3 -->
<!-- api: M:Inno.Editor.Inspection.PropertyDrawContext.DrawChild(System.String,System.Type,System.Func{System.Object},System.Action{System.Object},System.Boolean) 2b7fc8a26ca0d67f2ceed34b1bda91332e3dd1951dbc6fe2fa3ba33acda7b561 -->
<!-- api: M:Inno.Editor.Inspection.PropertyDrawContext.DrawChild(Inno.Core.Serialization.SerializedProperty) cfb0c784be1f6d46576e751104f9e363ea263db96ab1e0bfaef2d294d9c1666d -->
<!-- api: M:Inno.Editor.Inspection.PropertyDrawContext.DrawChild(System.Object,Inno.Core.Serialization.SerializedProperty) dbe1636cfd11a252494d78b748b382a62ae80ab233141a9dfa6508b761718407 -->
<!-- api: M:Inno.Editor.Inspection.PropertyDrawContext.DrawInlineChild(System.String,System.Type,System.Func{System.Object},System.Action{System.Object},System.Boolean) f0569fdc3f6e1ee724415ba5d9dd049915cd03be459a2aa07e31b18618cd5548 -->
<!-- api: T:Inno.Editor.Inspection.PropertyDrawerAttribute cdf6cf8391e11455d430b9267e4f977e625bafa300c7ace1741034142e26f602 -->
<!-- api: P:Inno.Editor.Inspection.PropertyDrawerAttribute.targetType 46bc38f1b43f4f0316caa1eca4d19c37d99afb15276a9189939ef06c7118a533 -->
<!-- api: P:Inno.Editor.Inspection.PropertyDrawerAttribute.useForChildren 5d70033480ebc1a31d07c1b513cc972b69646c65819aa16ff43718a39a885c5d -->
<!-- api: P:Inno.Editor.Inspection.PropertyDrawerAttribute.priority 94dd6c2a9d7532a7050ed7b24699b701a009c7dfbf438baa2ccd3a2283a2638d -->
<!-- api: M:Inno.Editor.Inspection.PropertyDrawerAttribute.#ctor(System.Type,System.Boolean,System.Int32) 20c8a13cbb53e0d089923394303b77b96aa7e713d2f59646c919c39bf7a1511d -->
<!-- api: T:Inno.Editor.Inspection.PropertyDrawerRegistry 0e4208f085cb9c21a749d46a4ac2509295dba0de5a4a211fefa9a1ed1decaeff -->
<!-- api: M:Inno.Editor.Inspection.PropertyDrawerRegistry.#ctor(Inno.Editor.Interactions.EditorInteractions,Inno.Extensibility.Types.TypeCatalog,Inno.Core.Serialization.SerializationRegistry,System.Collections.Generic.IEnumerable{System.Object}) 2c4ab4f8202a82848efcddc407e0157ffe4b4f8f73436bc2a3d185aa37168259 -->
<!-- api: M:Inno.Editor.Inspection.PropertyDrawerRegistry.Resolve(System.Type) 9460177231d9372b5690df74b300b8347e4cbe9410968a07d4b3c9f4b4bb05e6 -->
<!-- api: M:Inno.Editor.Inspection.PropertyDrawerRegistry.Dispose f8686b918c74e626c526a9f409d6a9507eaada4817aa3cc19d79515b39b43e48 -->
<!-- api: T:Inno.Editor.Inspection.SerializedPropertyRenderer 331e09afc807a4db40f41d3e997e471e11bbf217097d77669d639a4a359f5468 -->
<!-- api: M:Inno.Editor.Inspection.SerializedPropertyRenderer.#ctor(Inno.Editor.Inspection.PropertyDrawerRegistry,Inno.Editor.Inspection.InspectorAttributeDrawerRegistry,Inno.Editor.Interactions.EditorInteractions,Inno.Editor.Inspection.IInspectionPropertyEditService,Inno.Core.Logging.LogRouter) d725a7903ae6563e12178df7aa2961da10e7716e16fd57d452ac2822ba7880d3 -->
<!-- api: M:Inno.Editor.Inspection.SerializedPropertyRenderer.Draw(Inno.Editor.Core.EditorContext,System.Object,System.String,Inno.Core.Serialization.SerializedProperty) 3c960965fe1961e376dcf6cf6423fca50a7b15a7b78ddba5c40e25f8bceb31ce -->
<!-- api: M:Inno.Editor.Inspection.SerializedPropertyRenderer.DrawValue(Inno.Editor.Core.EditorContext,System.Object,System.String,System.String,System.Type,System.Func{System.Object},System.Action{System.Object},Inno.Editor.Inspection.IInspectionPropertyEditService,System.Boolean,System.Boolean,System.String,System.Nullable{System.Double},System.Nullable{System.Double}) 873f37309dc9414303dcb9fa6134048a1033a08f1cb63526ea02f0bba1651b7c -->
<!-- api: M:Inno.Editor.Inspection.SerializedPropertyRenderer.DrawDraftProperty(Inno.Editor.Core.EditorContext,System.Object,System.Object,System.String,Inno.Core.Serialization.SerializedProperty,Inno.Editor.Inspection.IInspectionPropertyEditService,System.Boolean) 5aee98252a4d10566fb2dfd8f18286a7d0d114cef453db8e0a1013e2dfca6b61 -->
<!-- api: M:Inno.Editor.Inspection.SerializedPropertyRenderer.GetPropertyAvailability(System.Object,Inno.Core.Serialization.SerializedProperty) e5e8fde85f3f048ba42d28faf067d835ce165a28444f1c57241bc8139a658bfe -->

| API 与源码 | 契约与参数 | 返回值 | 重要异常 |
| --- | --- | --- | --- |
| <a id="api-9250e0ed5043e709"></a>[`public interface Inno.Editor.Inspection.IInspectorAttributeDrawer`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/IInspectorAttributeDrawer.cs#L6) | Extends serialized-property presentation with reusable attribute-driven behavior. | — | — |
| <a id="api-c40fc8f72779cd64"></a>[`void Inno.Editor.Inspection.IInspectorAttributeDrawer.Update(Inno.Editor.Inspection.InspectorAttributeDrawContext context)`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/IInspectorAttributeDrawer.cs#L14) | Updates visibility, interactivity, labeling, help, or numeric constraints before layout.<br>`context`: Mutable presentation state for the annotated property. | — | — |
| <a id="api-7fdad10f777bfb7c"></a>[`void Inno.Editor.Inspection.IInspectorAttributeDrawer.DrawBefore(Inno.Editor.Inspection.InspectorAttributeDrawContext context)`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/IInspectorAttributeDrawer.cs#L24) | Draws a decorator immediately before the property row.<br>`context`: Current presentation state for the annotated property. | — | — |
| <a id="api-53080d5ed05c6cbf"></a>[`void Inno.Editor.Inspection.IInspectorAttributeDrawer.DrawAfter(Inno.Editor.Inspection.InspectorAttributeDrawContext context)`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/IInspectorAttributeDrawer.cs#L34) | Draws a decorator immediately after the property row.<br>`context`: Current presentation state for the annotated property. | — | — |
| <a id="api-cd6df616944d84ca"></a>[`public sealed class Inno.Editor.Inspection.InspectorAttributeDrawContext`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawContext.cs#L10) | Carries the mutable presentation state of one attribute-annotated Inspector property. | — | — |
| <a id="api-100db3211f759f70"></a>[`public object Inno.Editor.Inspection.InspectorAttributeDrawContext.owner { get; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawContext.cs#L35) | Gets the object that owns the annotated member. | — | — |
| <a id="api-4d1f1cfe0da61865"></a>[`public System.Reflection.MemberInfo Inno.Editor.Inspection.InspectorAttributeDrawContext.member { get; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawContext.cs#L40) | Gets the reflected field or property carrying the current attribute. | — | — |
| <a id="api-5112bb562ced2542"></a>[`public Inno.Core.Serialization.SerializedProperty? Inno.Editor.Inspection.InspectorAttributeDrawContext.property { get; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawContext.cs#L45) | Gets the root serialized property when one is available. | — | — |
| <a id="api-0160bdc8aa5ee7a8"></a>[`public string Inno.Editor.Inspection.InspectorAttributeDrawContext.path { get; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawContext.cs#L50) | Gets the stable Inspector control path. | — | — |
| <a id="api-6e4a11a92672ddb7"></a>[`public System.Attribute Inno.Editor.Inspection.InspectorAttributeDrawContext.attribute { get; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawContext.cs#L55) | Gets the attribute currently invoking a drawer. | — | — |
| <a id="api-c4a8b1c46ecb94d9"></a>[`public bool Inno.Editor.Inspection.InspectorAttributeDrawContext.isVisible { get; set; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawContext.cs#L60) | Gets or sets whether the complete property row is visible. | — | — |
| <a id="api-4a1dba40f97242cd"></a>[`public bool Inno.Editor.Inspection.InspectorAttributeDrawContext.isReadOnly { get; set; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawContext.cs#L65) | Gets or sets whether the property control is read-only in the Inspector. | — | — |
| <a id="api-29fe396cff026488"></a>[`public string Inno.Editor.Inspection.InspectorAttributeDrawContext.label { get; set; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawContext.cs#L70) | Gets or sets the visible property label. | — | — |
| <a id="api-983e467096232c6e"></a>[`public string Inno.Editor.Inspection.InspectorAttributeDrawContext.tooltip { get; set; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawContext.cs#L75) | Gets or sets optional hover help for the property label. | — | — |
| <a id="api-ef064efacf915d8e"></a>[`public double? Inno.Editor.Inspection.InspectorAttributeDrawContext.minimum { get; set; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawContext.cs#L80) | Gets or sets an optional inclusive numeric lower bound. | — | — |
| <a id="api-a40a7e7ceb07728b"></a>[`public double? Inno.Editor.Inspection.InspectorAttributeDrawContext.maximum { get; set; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawContext.cs#L85) | Gets or sets an optional inclusive numeric upper bound. | — | — |
| <a id="api-c6e0f7ba40ba4809"></a>[`public object? Inno.Editor.Inspection.InspectorAttributeDrawContext.GetSiblingValue(string memberName)`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawContext.cs#L99) | Reads a sibling field or property from the current owner, including non-public base members.<br>`memberName`: Exact CLR member name. | The current sibling value. | `System.InvalidOperationException`: Thrown when the sibling is absent, ambiguous, indexed, or unreadable. |
| <a id="api-c7b3c1c27c9208db"></a>[`public sealed class Inno.Editor.Inspection.InspectorAttributeDrawerAttribute : System.Attribute`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawerAttribute.cs#L9) | Associates an Inspector attribute drawer with an attribute type. | — | — |
| <a id="api-b1a7358a971b1060"></a>[`public Inno.Editor.Inspection.InspectorAttributeDrawerAttribute.InspectorAttributeDrawerAttribute(System.Type targetType, bool useForChildren = false, int priority = 0)`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawerAttribute.cs#L23) | Creates an Inspector attribute drawer registration.<br>`targetType`: Attribute type handled by the drawer.<br>`useForChildren`: Whether derived attribute types are accepted.<br>`priority`: Tie-breaking priority after inheritance distance. | — | — |
| <a id="api-055c85dfa6e6a7ec"></a>[`public System.Type Inno.Editor.Inspection.InspectorAttributeDrawerAttribute.targetType { get; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawerAttribute.cs#L39) | Gets the handled attribute type. | — | — |
| <a id="api-e25ef8917ffe4e04"></a>[`public bool Inno.Editor.Inspection.InspectorAttributeDrawerAttribute.useForChildren { get; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawerAttribute.cs#L44) | Gets whether derived attribute types are accepted. | — | — |
| <a id="api-191636c909d9ccf5"></a>[`public int Inno.Editor.Inspection.InspectorAttributeDrawerAttribute.priority { get; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawerAttribute.cs#L49) | Gets the registration priority. | — | — |
| <a id="api-e185291d28d48ed6"></a>[`public sealed class Inno.Editor.Inspection.InspectorAttributeDrawerRegistry : System.IDisposable`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawerRegistry.cs#L14) | Discovers generation-aware Inspector attribute drawers and applies them in declaration order. | — | — |
| <a id="api-1694b3669a0bc028"></a>[`public Inno.Editor.Inspection.InspectorAttributeDrawerRegistry.InspectorAttributeDrawerRegistry(Inno.Editor.Interactions.EditorInteractions interactions, Inno.Extensibility.Types.TypeCatalog types, Inno.Core.Serialization.SerializationRegistry serialization, System.Collections.Generic.IEnumerable<object> drawerServices)`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawerRegistry.cs#L33) | Creates an Inspector attribute drawer registry.<br>`interactions`: Active editor interaction entry point.<br>`types`: Host-owned type catalog.<br>`serialization`: Active serialization registry.<br>`drawerServices`: Additional constructor-injected drawer services. | — | — |
| <a id="api-18345765c0b08daf"></a>[`public void Inno.Editor.Inspection.InspectorAttributeDrawerRegistry.Dispose()`](../../src/composition/editor/framework/Inno.Editor.Inspection/AttributeDrawing/InspectorAttributeDrawerRegistry.cs#L88) | Releases active drawer generations. | — | — |
| <a id="api-5cd4c955d9919aa1"></a>[`public interface Inno.Editor.Inspection.IInspectionIconProvider<TTarget>`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/IInspectionIconProvider.cs#L9) | Resolves a presentation icon for inspected targets owned by another editor feature.<br>`TTarget`: The target type whose icon can be resolved. | — | — |
| <a id="api-6407cab121c7ef14"></a>[`string Inno.Editor.Inspection.IInspectionIconProvider<TTarget>.GetIcon(TTarget target)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/IInspectionIconProvider.cs#L20) | Resolves the presentation icon for one inspected target.<br>`target`: The target whose icon should be resolved. | The icon glyph registered by the owning feature. | — |
| <a id="api-03c64b5591aa5f92"></a>[`public sealed class Inno.Editor.Inspection.InspectionDrawContext`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawContext.cs#L15) | Provides services and state to an inspector drawer. | — | — |
| <a id="api-9b4e610802256428"></a>[`public Inno.Editor.Core.EditorContext Inno.Editor.Inspection.InspectionDrawContext.editorContext { get; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawContext.cs#L22) | Gets the shared editor context. | — | — |
| <a id="api-624e86869a649aae"></a>[`public Inno.Editor.Interactions.EditorInteractions Inno.Editor.Inspection.InspectionDrawContext.interactions { get; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawContext.cs#L27) | Gets the active editor interaction entry point. | — | — |
| <a id="api-8030c3472b70997c"></a>[`public object Inno.Editor.Inspection.InspectionDrawContext.target { get; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawContext.cs#L32) | Gets the selected target. | — | — |
| <a id="api-6860d9be176cc5a3"></a>[`public Inno.Editor.Inspection.SerializedPropertyRenderer Inno.Editor.Inspection.InspectionDrawContext.properties { get; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawContext.cs#L37) | Gets the serialized property renderer. | — | — |
| <a id="api-b7060aed018bd937"></a>[`public bool Inno.Editor.Inspection.InspectionDrawContext.TryDrawInline(object target)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawContext.cs#L48) | Draws an exact target-specific body inside the caller's existing Inspector card.<br>`target`: Nested target whose exact registered drawer should render. | when an exact drawer rendered the target. | — |
| <a id="api-fd90c47a4171c194"></a>[`public bool Inno.Editor.Inspection.InspectionDrawContext.HasProperty(string propertyName)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawContext.cs#L75) | Gets whether the target exposes one runtime-visible serialized property.<br>`propertyName`: Exact serialized property name. | when the property is available. | — |
| <a id="api-7c7a19d83652ff11"></a>[`public T Inno.Editor.Inspection.InspectionDrawContext.GetProperty<T>(string propertyName)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawContext.cs#L94) | Reads one runtime-visible serialized property from the target.<br>`propertyName`: Exact serialized property name.<br>`T`: Expected property value type. | The current strongly typed property value. | — |
| <a id="api-e85f1abb8e0c05c0"></a>[`public void Inno.Editor.Inspection.InspectionDrawContext.DrawProperty(string propertyName)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawContext.cs#L110) | Draws one named property with its ordinary editor, history, and validation behavior.<br>`propertyName`: Exact serialized property name. | — | — |
| <a id="api-88f1d869e25a209d"></a>[`public void Inno.Editor.Inspection.InspectionDrawContext.DrawProperties()`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawContext.cs#L128) | Draws every runtime-visible serialized property through the shared attribute and property drawer pipeline. | — | — |
| <a id="api-44e2432333856a19"></a>[`public abstract class Inno.Editor.Inspection.InspectionDrawer<TTarget> where TTarget : class : Inno.Editor.Inspection.IInspectionDrawer`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawer.cs#L11) | Provides the target-specific header presentation and body drawing behavior for an inspected target.<br>`TTarget`: The target type inspected by the drawer. | — | — |
| <a id="api-c18ee56fe3963d47"></a>[`public abstract string Inno.Editor.Inspection.InspectionDrawer<TTarget>.icon { get; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawer.cs#L17) | Gets the icon glyph displayed in the large leading slot of the Inspector target header. | — | — |
| <a id="api-8109811c8eb1218d"></a>[`protected virtual bool Inno.Editor.Inspection.InspectionDrawer<TTarget>.CanInspect(TTarget target)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawer.cs#L28) | Determines whether this drawer handles the selected value, in addition to its registered type.<br>`target`: Current target. The check must not mutate it. | Whether this drawer accepts the target. | — |
| <a id="api-38149573ec950dc3"></a>[`protected abstract (string name, System.Action<string>? setter) Inno.Editor.Inspection.InspectionDrawer<TTarget>.BindName(Inno.Editor.Inspection.InspectionDrawContext context, TTarget target)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawer.cs#L45) | Binds the name displayed in the first row of the Inspector target header.<br>`context`: The active Inspector drawing context.<br>`target`: The strongly typed target being inspected. | The current target name and a callback that accepts a requested replacement name, or for the callback when the name is read-only. | — |
| <a id="api-9f89ebd29db17759"></a>[`protected virtual string Inno.Editor.Inspection.InspectionDrawer<TTarget>.GetIcon(Inno.Editor.Inspection.InspectionDrawContext context, TTarget target)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawer.cs#L62) | Resolves the icon glyph displayed for the current target.<br>`context`: The active Inspector drawing context.<br>`target`: The strongly typed target being inspected. | The target-specific icon glyph. The default implementation returns . | — |
| <a id="api-b839e8728616c277"></a>[`protected virtual void Inno.Editor.Inspection.InspectionDrawer<TTarget>.DrawHeader(Inno.Editor.Inspection.InspectionDrawContext context, TTarget target)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawer.cs#L80) | Draws target-specific controls in the second row of the Inspector target header.<br>`context`: The active Inspector drawing context.<br>`target`: The strongly typed target being inspected. | — | — |
| <a id="api-554c579798e1472a"></a>[`protected abstract void Inno.Editor.Inspection.InspectionDrawer<TTarget>.Draw(Inno.Editor.Inspection.InspectionDrawContext context, TTarget target)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawer.cs#L95) | Draws the target-specific Inspector body below the shared target header.<br>`context`: The active Inspector drawing context.<br>`target`: The strongly typed target being inspected. | — | — |
| <a id="api-3c3494468e70c3fe"></a>[`protected Inno.Editor.Inspection.InspectionDrawer<TTarget>.InspectionDrawer()`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawer.cs#L11) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ec646fac9cbd98a4"></a>[`public interface Inno.Editor.Inspection.IInspectionDrawer`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawer.cs#L125) | Defines the runtime bridge used to render a discovered inspection drawer. | — | — |
| <a id="api-dc451e07c269cbf6"></a>[`bool Inno.Editor.Inspection.IInspectionDrawer.CanInspect(object target)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawer.cs#L136) | Determines whether a type-compatible selected value can be inspected by this drawer.<br>`target`: Selected value, never mutated by this query. | Whether this drawer accepts the value; ordinary type-only drawers accept by default. | — |
| <a id="api-8fd01f4102d28656"></a>[`string Inno.Editor.Inspection.IInspectionDrawer.GetIcon(Inno.Editor.Inspection.InspectionDrawContext context)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawer.cs#L147) | Gets the icon glyph displayed in the Inspector target header.<br>`context`: The active Inspector drawing context. | The icon glyph for the current target. | — |
| <a id="api-313ac89b147dc459"></a>[`(string name, System.Action<string>? setter) Inno.Editor.Inspection.IInspectionDrawer.BindName(Inno.Editor.Inspection.InspectionDrawContext context)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawer.cs#L159) | Binds the target name displayed by the Inspector target header.<br>`context`: The active Inspector drawing context. | The current target name and a callback that accepts a requested replacement name, or for the callback when the name is read-only. | — |
| <a id="api-b7f2713ba42da33e"></a>[`void Inno.Editor.Inspection.IInspectionDrawer.DrawHeader(Inno.Editor.Inspection.InspectionDrawContext context)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawer.cs#L167) | Draws the target-specific second header row.<br>`context`: The active Inspector drawing context. | — | — |
| <a id="api-37313934abb73644"></a>[`void Inno.Editor.Inspection.IInspectionDrawer.Draw(Inno.Editor.Inspection.InspectionDrawContext context)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawer.cs#L175) | Draws the target-specific Inspector body.<br>`context`: The active Inspector drawing context. | — | — |
| <a id="api-a615fad2241a5874"></a>[`public sealed class Inno.Editor.Inspection.InspectionDrawerAttribute : System.Attribute`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawerAttribute.cs#L9) | Associates an inspector drawer with a selected target type. | — | — |
| <a id="api-dea42b64f6f37de8"></a>[`public System.Type Inno.Editor.Inspection.InspectionDrawerAttribute.targetType { get; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawerAttribute.cs#L14) | Gets the selected target type handled by the drawer. | — | — |
| <a id="api-ccd259e21c1364a4"></a>[`public bool Inno.Editor.Inspection.InspectionDrawerAttribute.useForChildren { get; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawerAttribute.cs#L19) | Gets whether derived target types are accepted. | — | — |
| <a id="api-1598f5245a1ad3c2"></a>[`public int Inno.Editor.Inspection.InspectionDrawerAttribute.priority { get; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawerAttribute.cs#L24) | Gets the tie-breaking registration priority. | — | — |
| <a id="api-bb83316734280166"></a>[`public bool Inno.Editor.Inspection.InspectionDrawerAttribute.conditional { get; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawerAttribute.cs#L29) | Gets whether CanInspect selects a subset of targets, allowing peer conditional registrations at the same priority. | — | — |
| <a id="api-53a73716a536ff5f"></a>[`public Inno.Editor.Inspection.InspectionDrawerAttribute.InspectionDrawerAttribute(System.Type targetType, bool useForChildren = false, int priority = 0, bool conditional = false)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawerAttribute.cs#L49) | Creates an inspector drawer registration.<br>`targetType`: The selected object type handled by the drawer.<br>`useForChildren`: Whether the drawer may handle assignable derived target types.<br>`priority`: The tie-breaking priority after exactness and inheritance distance.<br>`conditional`: Whether CanInspect is a target-dependent predicate. Equally specific accepted peers remain an explicit ambiguity error. | — | `System.ArgumentNullException`: Thrown when is . |
| <a id="api-c40f722122401247"></a>[`public delegate Inno.Editor.Inspection.IInspectionDrawer Inno.Editor.Inspection.InspectionDrawerFactory(System.Type drawerType)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawerFactory.cs#L17) | Creates one discovered inspection drawer through the owning editor composition root.<br>`drawerType`: The concrete drawer type selected from the active type catalog. | A new drawer instance whose runtime type is . | `System.ArgumentNullException`: Implementations may throw when is . |
| <a id="api-03ea580155776a5b"></a>[`public Inno.Editor.Inspection.InspectionDrawerFactory.InspectionDrawerFactory(object @object, nint method)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawerFactory.cs#L17) | Inherited or compiler-provided contract. | — | — |
| <a id="api-19b43da9dfd163ca"></a>[`public virtual Inno.Editor.Inspection.IInspectionDrawer Inno.Editor.Inspection.InspectionDrawerFactory.Invoke(System.Type drawerType)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawerFactory.cs#L17) | Creates one discovered inspection drawer through the owning editor composition root.<br>`drawerType`: The concrete drawer type selected from the active type catalog. | A new drawer instance whose runtime type is . | `System.ArgumentNullException`: Implementations may throw when is . |
| <a id="api-84a683a6b06db517"></a>[`public virtual System.IAsyncResult Inno.Editor.Inspection.InspectionDrawerFactory.BeginInvoke(System.Type drawerType, System.AsyncCallback callback, object @object)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawerFactory.cs#L17) | Inherited or compiler-provided contract. | — | — |
| <a id="api-08597c8b25e8d025"></a>[`public virtual Inno.Editor.Inspection.IInspectionDrawer Inno.Editor.Inspection.InspectionDrawerFactory.EndInvoke(System.IAsyncResult result)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawerFactory.cs#L17) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2103c4fb0f7019bf"></a>[`public sealed class Inno.Editor.Inspection.InspectionDrawerRegistry : System.IDisposable`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawerRegistry.cs#L16) | Discovers and resolves inspection drawers through the active type catalog. | — | — |
| <a id="api-738b626880b52911"></a>[`public Inno.Editor.Inspection.InspectionDrawerRegistry.InspectionDrawerRegistry(Inno.Editor.Interactions.EditorInteractions interactions, Inno.Editor.Inspection.InspectionDrawerFactory factory, Inno.Extensibility.Types.TypeCatalog types, Inno.Core.Serialization.SerializationRegistry serialization)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawerRegistry.cs#L41) | Creates a generation-aware inspection drawer registry.<br>`interactions`: The active editor interaction entry point exposed to draw contexts.<br>`factory`: The composition-root factory used to construct discovered drawer types and resolve their module-specific dependencies.<br>`types`: The host-owned type catalog that coordinates drawer generations.<br>`serialization`: Serialization registry used to expose the selected target's visible properties to custom drawers. | — | `System.ArgumentNullException`: Thrown when or is . |
| <a id="api-eff9f902711b05c5"></a>[`public bool Inno.Editor.Inspection.InspectionDrawerRegistry.TryResolve(Inno.Editor.Core.EditorContext editorContext, object target, Inno.Editor.Inspection.SerializedPropertyRenderer renderer, out Inno.Editor.Inspection.IInspectionDrawer? drawer, out Inno.Editor.Inspection.InspectionDrawContext? context)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawerRegistry.cs#L76) | Resolves the most specific registered drawer and creates its drawing context.<br>`editorContext`: The shared editor context exposed to the selected drawer.<br>`target`: The selected object whose runtime type determines the drawer.<br>`renderer`: The serialized property renderer exposed to the selected drawer.<br>`drawer`: The resolved drawer when the method succeeds.<br>`context`: The target-specific drawing context when the method succeeds. | when a matching drawer was resolved. | `System.ArgumentNullException`: Thrown when , , or is . |
| <a id="api-dcb330b8e7c8b591"></a>[`public bool Inno.Editor.Inspection.InspectionDrawerRegistry.TryResolveExact(Inno.Editor.Core.EditorContext editorContext, object target, Inno.Editor.Inspection.SerializedPropertyRenderer renderer, out Inno.Editor.Inspection.IInspectionDrawer? drawer, out Inno.Editor.Inspection.InspectionDrawContext? context)`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawerRegistry.cs#L127) | Resolves only a drawer explicitly registered for the target's exact runtime type.<br>`editorContext`: The shared editor context exposed to the selected drawer.<br>`target`: The selected object whose exact runtime type determines the drawer.<br>`renderer`: The serialized property renderer exposed to the selected drawer.<br>`drawer`: The exact drawer when the method succeeds.<br>`context`: The target-specific drawing context when the method succeeds. | when an exact registration exists; inherited and fallback drawers are ignored. | `System.ArgumentNullException`: Thrown when , , or is . |
| <a id="api-df3ae6f0294238e1"></a>[`public void Inno.Editor.Inspection.InspectionDrawerRegistry.Dispose()`](../../src/composition/editor/framework/Inno.Editor.Inspection/ObjectDrawing/InspectionDrawerRegistry.cs#L161) | Releases every active drawer snapshot and unregisters the registry from type refreshes. | — | — |
| <a id="api-91ed65d66592eaf6"></a>[`public interface Inno.Editor.Inspection.IInspectionPropertyEditService`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/IInspectionPropertyEditService.cs#L7) | Applies one root serialized-property mutation through the history mechanism owned by a feature. | — | — |
| <a id="api-2ffd06eae1f681cd"></a>[`bool Inno.Editor.Inspection.IInspectionPropertyEditService.ChangeProperty(object owner, string propertyName, System.Action mutation, string historyName, string? mergeKey, string? componentName)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/IInspectionPropertyEditService.cs#L41) | Applies a property mutation and records it when the owning feature observes a value change.<br>`owner`: The domain object that owns the root serialized property.<br>`propertyName`: The exact root serialized member name.<br>`mutation`: The callback that assigns the requested value.<br>`historyName`: The user-facing name for the resulting history entry.<br>`mergeKey`: The current shared gesture key, or null for a discrete edit.<br>`componentName`: An optional public value component; null replaces the complete root value. | when the value changed and the owning feature recorded the edit; otherwise, . | `System.ArgumentNullException`: Thrown when or is .<br>`System.ArgumentException`: Thrown when or is empty. |
| <a id="api-d4b21d69fced31ad"></a>[`public interface Inno.Editor.Inspection.IPropertyDrawer`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/IPropertyDrawer.cs#L6) | Draws and optionally edits one serialized property value. | — | — |
| <a id="api-2cfa3d7a3fe751de"></a>[`void Inno.Editor.Inspection.IPropertyDrawer.Draw(Inno.Editor.Inspection.PropertyDrawContext context)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/IPropertyDrawer.cs#L14) | Draws a property through its encapsulated value accessors.<br>`context`: Property drawing context. | — | — |
| <a id="api-d936c80bac14ed55"></a>[`public sealed class Inno.Editor.Inspection.PropertyDrawContext`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L12) | Encapsulates one editable property path and its drawing services. | — | — |
| <a id="api-5fb7e544dc75167c"></a>[`public Inno.Editor.Core.EditorContext Inno.Editor.Inspection.PropertyDrawContext.editorContext { get; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L24) | Gets the shared editor context. | — | — |
| <a id="api-fb3129e8bc2d2795"></a>[`public Inno.Editor.Interactions.EditorInteractions Inno.Editor.Inspection.PropertyDrawContext.interactions { get; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L28) | Gets the active editor interaction entry point. | — | — |
| <a id="api-42c0328fc9af7f41"></a>[`public string Inno.Editor.Inspection.PropertyDrawContext.path { get; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L32) | Gets the stable control path. | — | — |
| <a id="api-d6cfd4639c5dd55d"></a>[`public string Inno.Editor.Inspection.PropertyDrawContext.label { get; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L36) | Gets the display label. | — | — |
| <a id="api-db33e683322b5ca9"></a>[`public System.Type Inno.Editor.Inspection.PropertyDrawContext.propertyType { get; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L40) | Gets the declared property type. | — | — |
| <a id="api-07832da0fee5f727"></a>[`public Inno.Core.Serialization.PropertyVisibility Inno.Editor.Inspection.PropertyDrawContext.visibility { get; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L44) | Gets the serialization visibility flags. | — | — |
| <a id="api-6d8a3a5d19577874"></a>[`public bool Inno.Editor.Inspection.PropertyDrawContext.isReadOnly { get; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L48) | Gets whether assignments are disabled. | — | — |
| <a id="api-7ae2e01180d74825"></a>[`public double? Inno.Editor.Inspection.PropertyDrawContext.minimum { get; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L52) | Gets the optional inclusive numeric lower bound supplied by Inspector metadata. | — | — |
| <a id="api-eec3c694ee0959aa"></a>[`public double? Inno.Editor.Inspection.PropertyDrawContext.maximum { get; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L56) | Gets the optional inclusive numeric upper bound supplied by Inspector metadata. | — | — |
| <a id="api-e4a68169520ed7a5"></a>[`public string? Inno.Editor.Inspection.PropertyDrawContext.tooltip { get; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L60) | Gets optional hover help supplied by Inspector metadata. | — | — |
| <a id="api-5d8a0e3dd4680f30"></a>[`public bool Inno.Editor.Inspection.PropertyDrawContext.hdrColor { get; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L64) | Gets whether Color values are unclamped linear floating-point channels. | — | — |
| <a id="api-6d49f7e7afaab6e9"></a>[`public object? Inno.Editor.Inspection.PropertyDrawContext.GetValue()`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L114) | Gets the latest value. | The latest value returned by the serialized property's getter. | — |
| <a id="api-5cdca52431c760bc"></a>[`public bool Inno.Editor.Inspection.PropertyDrawContext.TryGetTextState(string key, out string? value)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L127) | Tries to read text-edit state scoped to this renderer, inspected owner, and property path.<br>`key`: Drawer-local state identity within the current property path.<br>`value`: Receives the stored text when the state exists. | when text state exists for the exact owner and property path. | — |
| <a id="api-43417ada2757d973"></a>[`public void Inno.Editor.Inspection.PropertyDrawContext.SetTextState(string key, string value)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L140) | Stores text-edit state scoped to this renderer, inspected owner, and property path.<br>`key`: Drawer-local state identity within the current property path.<br>`value`: Current neutral text state to retain between frames. | — | — |
| <a id="api-1deb3e52efbe69ee"></a>[`public void Inno.Editor.Inspection.PropertyDrawContext.ClearTextState(string key)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L150) | Removes text-edit state for this renderer, inspected owner, and property path.<br>`key`: Drawer-local state identity within the current property path. | — | — |
| <a id="api-60f984021553711f"></a>[`public void Inno.Editor.Inspection.PropertyDrawContext.SetValue(object? value, string? componentName = null)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L160) | Assigns a value when the property is writable.<br>`value`: New value.<br>`componentName`: An optional public scalar component, or an Euler axis such as euler.x, for precise batch value edits. | — | — |
| <a id="api-ac30372944accab6"></a>[`public void Inno.Editor.Inspection.PropertyDrawContext.DrawChild(string childName, System.Type childType, System.Func<object?> getter, System.Action<object?> setter, bool readOnly = false)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L199) | Draws a nested value whose setter writes through this property path.<br>`childName`: The stable child member name appended to this property path.<br>`childType`: The declared type used to resolve a property drawer.<br>`getter`: The callback that reads the latest child value.<br>`setter`: The callback that writes an edited child value.<br>`readOnly`: Whether the child should be presented without assignment support. | — | — |
| <a id="api-91f1252ee3ca138b"></a>[`public void Inno.Editor.Inspection.PropertyDrawContext.DrawChild(Inno.Core.Serialization.SerializedProperty property)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L216) | Draws a nested serialized property.<br>`property`: Nested property. | — | — |
| <a id="api-2908f3db2eeb8df7"></a>[`public void Inno.Editor.Inspection.PropertyDrawContext.DrawChild(object metadataOwner, Inno.Core.Serialization.SerializedProperty property)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L226) | Draws a nested serialized property with presentation metadata resolved from its immediate owner.<br>`metadataOwner`: Object that declares the nested serialized member.<br>`property`: Nested property. | — | — |
| <a id="api-2e3ea53d14d77236"></a>[`public void Inno.Editor.Inspection.PropertyDrawContext.DrawInlineChild(string childName, System.Type childType, System.Func<object?> getter, System.Action<object?> setter, bool readOnly = false)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawContext.cs#L295) | Draws a nested value inline without adding another label row.<br>`childName`: The stable child member name appended to this property path.<br>`childType`: The declared type used to resolve a property drawer.<br>`getter`: The callback that reads the latest child value.<br>`setter`: The callback that writes an edited child value.<br>`readOnly`: Whether the child should be presented without assignment support. | — | — |
| <a id="api-5d459635b7133dc8"></a>[`public sealed class Inno.Editor.Inspection.PropertyDrawerAttribute : System.Attribute`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawerAttribute.cs#L9) | Associates a property drawer with a declared property type. | — | — |
| <a id="api-f0d559c52e216e52"></a>[`public System.Type Inno.Editor.Inspection.PropertyDrawerAttribute.targetType { get; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawerAttribute.cs#L14) | Gets the declared property type handled by the drawer. | — | — |
| <a id="api-81b0dfcd650c0233"></a>[`public bool Inno.Editor.Inspection.PropertyDrawerAttribute.useForChildren { get; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawerAttribute.cs#L19) | Gets whether assignable derived types are accepted. | — | — |
| <a id="api-c0bed46c82d28385"></a>[`public int Inno.Editor.Inspection.PropertyDrawerAttribute.priority { get; }`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawerAttribute.cs#L24) | Gets the tie-breaking registration priority. | — | — |
| <a id="api-ff3c5603ed80c0e6"></a>[`public Inno.Editor.Inspection.PropertyDrawerAttribute.PropertyDrawerAttribute(System.Type targetType, bool useForChildren = false, int priority = 0)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawerAttribute.cs#L41) | Creates a property drawer registration.<br>`targetType`: The declared serialized property type handled by the drawer.<br>`useForChildren`: Whether the drawer may handle assignable derived property types.<br>`priority`: The tie-breaking priority after exactness and inheritance distance. | — | `System.ArgumentNullException`: Thrown when is . |
| <a id="api-0fa464d5ce66b3bd"></a>[`public sealed class Inno.Editor.Inspection.PropertyDrawerRegistry : System.IDisposable`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawerRegistry.cs#L15) | Discovers and resolves serialized property drawers through the active type catalog. | — | — |
| <a id="api-6e20d9e60d8cf4e8"></a>[`public Inno.Editor.Inspection.PropertyDrawerRegistry.PropertyDrawerRegistry(Inno.Editor.Interactions.EditorInteractions interactions, Inno.Extensibility.Types.TypeCatalog types, Inno.Core.Serialization.SerializationRegistry serialization, System.Collections.Generic.IEnumerable<object> drawerServices)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawerRegistry.cs#L37) | Creates a generation-aware property drawer registry.<br>`interactions`: The active editor interaction entry point available to property drawer constructors.<br>`types`: The host-owned type catalog that coordinates drawer generations.<br>`serialization`: The serialization registry available to structured-object drawers.<br>`drawerServices`: Additional generation-bound services available to feature-specific drawer constructors. | — | `System.ArgumentNullException`: Thrown when is . |
| <a id="api-1354059c1a1fbca0"></a>[`public Inno.Editor.Inspection.IPropertyDrawer Inno.Editor.Inspection.PropertyDrawerRegistry.Resolve(System.Type propertyType)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawerRegistry.cs#L58) | Resolves the most specific drawer for a declared property type.<br>`propertyType`: The declared serialized property type to resolve. | The most specific registered drawer, or the unsupported-value fallback. | `System.ArgumentNullException`: Thrown when is . |
| <a id="api-2c65205568217927"></a>[`public void Inno.Editor.Inspection.PropertyDrawerRegistry.Dispose()`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/PropertyDrawerRegistry.cs#L67) | Releases every active property drawer snapshot and unregisters the registry from type refreshes. | — | — |
| <a id="api-c662ee939dfb8ba5"></a>[`public sealed class Inno.Editor.Inspection.SerializedPropertyRenderer`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/SerializedPropertyRenderer.cs#L19) | Resolves drawers and renders serialized property paths with isolated error handling. | — | — |
| <a id="api-ded750e0985e38ff"></a>[`public Inno.Editor.Inspection.SerializedPropertyRenderer.SerializedPropertyRenderer(Inno.Editor.Inspection.PropertyDrawerRegistry drawers, Inno.Editor.Inspection.InspectorAttributeDrawerRegistry attributes, Inno.Editor.Interactions.EditorInteractions interactions, Inno.Editor.Inspection.IInspectionPropertyEditService edits, Inno.Core.Logging.LogRouter logs)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/SerializedPropertyRenderer.cs#L51) | Creates a serialized property renderer over one drawer registry and feature-owned edit service.<br>`drawers`: The property drawer registry used for runtime type resolution.<br>`attributes`: The generation-aware registry that interprets Inspector presentation attributes.<br>`interactions`: The active editor interaction entry point.<br>`edits`: The feature-owned service used to apply and record property changes.<br>`logs`: The host-owned log router used to report isolated drawer failures. | — | `System.ArgumentNullException`: Thrown when , , or is . |
| <a id="api-4cab78320c06ead0"></a>[`public void Inno.Editor.Inspection.SerializedPropertyRenderer.Draw(Inno.Editor.Core.EditorContext editorContext, object owner, string ownerPath, Inno.Core.Serialization.SerializedProperty property)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/SerializedPropertyRenderer.cs#L81) | Draws a root serialized property.<br>`editorContext`: Shared editor context.<br>`owner`: The live domain object that owns the root property.<br>`ownerPath`: Stable owner path.<br>`property`: Serialized property. | — | — |
| <a id="api-8123137305545789"></a>[`public void Inno.Editor.Inspection.SerializedPropertyRenderer.DrawValue(Inno.Editor.Core.EditorContext editorContext, object owner, string path, string label, System.Type propertyType, System.Func<object?> getter, System.Action<object?> setter, Inno.Editor.Inspection.IInspectionPropertyEditService edits, bool readOnly = false, bool hdrColor = false, string? tooltip = null, double? minimum = null, double? maximum = null)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/SerializedPropertyRenderer.cs#L148) | Draws a declared authoring value with existing property drawers and a feature-owned draft edit service.<br>`editorContext`: Current Editor frame.<br>`owner`: Stable owner for text state and edit routing; not necessarily a canonical serialized object.<br>`path`: Stable unique property path.<br>`label`: User-facing label.<br>`propertyType`: Declared value type used to select its registered drawer.<br>`getter`: Reads the detached value.<br>`setter`: Changes the detached value within the supplied edit service.<br>`edits`: Feature-owned transaction boundary, propagated to nested properties.<br>`readOnly`: Whether assignment is disabled.<br>`hdrColor`: Whether Color values use unclamped linear floating-point channels.<br>`tooltip`: Optional authoring description rendered by the shared property tooltip.<br>`minimum`: Optional inclusive numeric editing bound.<br>`maximum`: Optional inclusive numeric editing bound. | — | — |
| <a id="api-133afe933690234e"></a>[`public void Inno.Editor.Inspection.SerializedPropertyRenderer.DrawDraftProperty(Inno.Editor.Core.EditorContext editorContext, object stateOwner, object valueOwner, string ownerPath, Inno.Core.Serialization.SerializedProperty property, Inno.Editor.Inspection.IInspectionPropertyEditService edits, bool readOnly = false)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/SerializedPropertyRenderer.cs#L210) | Draws a detached serialized property with its attributes and a feature-owned draft transaction.<br>`editorContext`: Current Editor frame.<br>`stateOwner`: Stable host-owned presentation state, containing no collectible objects.<br>`valueOwner`: Invocation-local detached value declaring the property and attributes.<br>`ownerPath`: Stable document or settings path.<br>`property`: Detached native serialized property.<br>`edits`: Draft mutation and shared History boundary.<br>`readOnly`: Whether source ownership prohibits edits. | — | — |
| <a id="api-130a784161c4a481"></a>[`public (bool isVisible, bool isReadOnly) Inno.Editor.Inspection.SerializedPropertyRenderer.GetPropertyAvailability(object owner, Inno.Core.Serialization.SerializedProperty property)`](../../src/composition/editor/framework/Inno.Editor.Inspection/PropertyDrawing/SerializedPropertyRenderer.cs#L253) | Evaluates the same current-generation presentation attributes used by drawing without creating any UI state.<br>`owner`: The current object declaring the property's conditional metadata.<br>`property`: The root serialized property to inspect. | Visibility and editability for this exact owner; callers must combine all selected owners before a batch edit. | — |

## 项目依赖

| 工程 | 边界 |
| --- | --- |
| `Inno.Editor.ImGui` | 实现依赖；不传递 compile surface |
| `Inno.Core.Mathematics` | 实现依赖；不传递 compile surface |
| `Inno.Scripting.Api` | 实现依赖；不传递 compile surface |
| `Inno.Editor.Annotations` | 实现依赖；不传递 compile surface |
| `Inno.Editor.Core` | 公开签名依赖 |
| `Inno.Editor.Interactions` | 公开签名依赖 |
| `Inno.Core.Serialization` | 公开签名依赖 |
| `Inno.Extensibility.Catalogs` | 公开签名依赖 |
| `Inno.Core.Logging` | 公开签名依赖 |
| `Inno.Extensibility.Types` | 公开签名依赖 |
