# Inno.Editor.Shaders

[Editor 索引](README.md) · [Wiki 首页](../README.md) · [Shader 显示扩展](Inno.Editor.Shaders.ImGui.md) · [Rendering](Inno.Editor.Rendering.md)

## 职责与边界

本项目拥有无界面的 Material 源草稿与 Shader 创作数据服务。绘制、节点 Drawer 和预览控件由 Shaders.ImGui 持有；本项目不引用 ImGui、Panel 或原生 UI。

## 初始化与依赖

MaterialDocuments 由 Editor Module Registry 创建，依赖 AssetPipeline、SerializationRegistry、EditorInteractions 和中立 IEditorPreviewService。底层 AssetDraftDocuments 提供编解码、恢复、dirty 状态和中立 History。Shutdown 清理本代缓存并保留未保存的 recovery。

## 公开 API 与工作流

Open(path) 打开源，Read(id) 返回 detached Material，Replace(id, candidate, mergeKey) 立即记录修改。ReplaceMany 预验证并原子记录多文档 sample。连续输入由共享 EditorEditGesture 定义；mergeKey 不代表保存。Save/Revert/Close 通过统一 documents 服务完成。

ShaderParameterPresentation 存储 Editor-only 的 group、description、visible 和有限范围。它使用原生 Serialization、稳定绑定 ID 和 owner reference context；不修改 runtime Shader 接口或现有参数值。

```csharp
using System;
using InnoEditor.Shaders;

static void ReadCurrentDraft(
    MaterialDocuments documents,
    Guid assetId
) {
    _ = documents.Read(assetId);
}
```

## 生命周期、错误与热重载

Canonical Material、引用资产与运行时发布值始终由 Assets owner 管理。草稿只保存中立 bytes，显式保存前不发布。源冲突保留草稿与诊断；重开文档后 History 按 persistent ID 恢复。缺少源或类型形成可恢复 barrier。不要跨 callback 保存 detached Asset、Type、preview 或 Registry 实例。

公开签名与对应源码见下方清单；其他内部类型不是稳定 API。

## 当前源码公开 API 清单

此清单对应当前 SDK 求值与源码符号；内部类型不构成公开 API。

<!-- api: T:Inno.Editor.Shaders.MaterialDocuments 1b0656a2573ae7a939d4308c8276064d4624c9540437f2b9590f61cda25c43d7 -->
<!-- api: P:Inno.Editor.Shaders.MaterialDocuments.assets 40dfd346e1392c353f8c7f8569e610ce1f1f7f5aae8119aa2cf869af63fc7826 -->
<!-- api: P:Inno.Editor.Shaders.MaterialDocuments.previews ceca0a7b49afe76121a6f621f3d91674af0d4693331162fea929762ce33410c9 -->
<!-- api: M:Inno.Editor.Shaders.MaterialDocuments.Open(Inno.Assets.AssetPath) 9963afc9b37f8f4169e5a9c694e13654b3425238f22391da19e780faa7b77de6 -->
<!-- api: M:Inno.Editor.Shaders.MaterialDocuments.Read(System.Guid) aaf3afc046125423a8d8ac4039a91a97f21c2b86578835c7b43a3f6513ad4f65 -->
<!-- api: M:Inno.Editor.Shaders.MaterialDocuments.Replace(System.Guid,Inno.Rendering.Assets.MaterialAsset,System.String) 6765e5cdb2a23b336ed23fd889058857ff2769fbee6b671ddef33a616d57ba5b -->
<!-- api: M:Inno.Editor.Shaders.MaterialDocuments.ReplaceMany(System.Collections.Generic.IReadOnlyDictionary{System.Guid,Inno.Rendering.Assets.MaterialAsset},System.String) 14fa2491cbb3f7608230e63e0ec4bb8ba234838ad918c12dddf16dc126bc6d84 -->
<!-- api: M:Inno.Editor.Shaders.MaterialDocuments.OpenDraft(Inno.Assets.AssetPath) 0fc30c928fa638897bc0b60ce59a2d57ce8021f01811e14113f224d92038617e -->
<!-- api: M:Inno.Editor.Shaders.MaterialDocuments.Open(Inno.Rendering.Assets.MaterialAsset) 998106c5d6d6111b5967294e6fe8c21792d2d6c3a2a214baf812cf8cf7c17264 -->
<!-- api: M:Inno.Editor.Shaders.MaterialDocuments.Presentation(Inno.Rendering.Assets.ShaderAsset,Inno.Rendering.ShaderPropertyId,System.String@) ad8a27634184814d70c804087779022b7334b63f46a577c79c10476cfde176bf -->
<!-- api: M:Inno.Editor.Shaders.MaterialDocuments.OnStart(Inno.Editor.Core.EditorContext) 978c23e17cb008269041800a42474e92675b096f36de8db95624554b38d623b7 -->
<!-- api: M:Inno.Editor.Shaders.MaterialDocuments.OnUpdate(Inno.Editor.Core.EditorContext) 23d0c13311e2b8af244cf0d8f1e5e30e0895f003e1561159696e8120ea3fe6bb -->
<!-- api: M:Inno.Editor.Shaders.MaterialDocuments.OnStop(Inno.Editor.Core.EditorContext) 9f17c2507991b95522197194c5372005b0a57fd4c5a83c8d2d5094a673831ea0 -->
<!-- api: T:Inno.Editor.Shaders.ShaderParameterPresentation bf055a131ee27d3be6127dbce06f81fd4836b3d77def0cb7bb3c7c49cdaad273 -->
<!-- api: P:Inno.Editor.Shaders.ShaderParameterPresentation.group aec8914f4237a9e3104b0974134deb6f88a282ffb170940825c89b9e7b68aaa3 -->
<!-- api: P:Inno.Editor.Shaders.ShaderParameterPresentation.description 21f76d15b10c89b3636e512554731ba98162fff735f9404c80aebb776edef66d -->
<!-- api: P:Inno.Editor.Shaders.ShaderParameterPresentation.visible 77ef7951f58952276b51f602d1a30c4c4cb00ac412fe733be08030658af8a815 -->
<!-- api: P:Inno.Editor.Shaders.ShaderParameterPresentation.hasRange fa225d762d527e6de8eda33abc506ca93bfdb87ccc930e862779f0e04f29ae4a -->
<!-- api: P:Inno.Editor.Shaders.ShaderParameterPresentation.minimum 58f04d36948dc7ca2478047dd1ea776fb2be837c333165119a7be272d6b5afec -->
<!-- api: P:Inno.Editor.Shaders.ShaderParameterPresentation.maximum e58585bd246ba115c3a6d0d0722de27f7757a3d11ab7a8071cf6f58cc0b8fa74 -->
<!-- api: M:Inno.Editor.Shaders.ShaderParameterPresentation.Read(Inno.Core.Graphs.GraphDocument,Inno.Rendering.ShaderPropertyId,Inno.Core.Serialization.SerializationRegistry,Inno.Core.Serialization.SerializationContext) aec7eac40d00ef2d240e16e021a6c100b5e20f1d87dd85cf0024f6f5c53b7e17 -->
<!-- api: M:Inno.Editor.Shaders.ShaderParameterPresentation.Write(Inno.Core.Graphs.GraphDocument,Inno.Rendering.ShaderPropertyId,Inno.Editor.Shaders.ShaderParameterPresentation,Inno.Core.Serialization.SerializationRegistry,Inno.Core.Serialization.SerializationContext) 2149ba4eee00a0f7cd562ab135733f0ea4753816e240d7f0afbfee6a5ebc0fea -->
<!-- api: M:Inno.Editor.Shaders.ShaderParameterPresentation.#ctor e4bb5c5c18bdc71df51dcb600dace4c21ddf7e2feb3675304fb17bac97e1df2c -->
<!-- api: T:Inno.Editor.Shaders.ShaderInspectionSelection 8e06df79ba9dcd40a45ba763c5f9858979da6381980a512c17248dd5030d5308 -->
<!-- api: M:Inno.Editor.Shaders.ShaderInspectionSelection.#ctor(System.Guid,System.Collections.Generic.IEnumerable{Inno.Core.Graphs.GraphNodeId}) e4446925d124a6c7ae5fc9ac72bd21c34c363986396f4f024010b54f97a4a349 -->
<!-- api: P:Inno.Editor.Shaders.ShaderInspectionSelection.assetId e9c4903274ec0328cef80cd28681803a1d2ec9530359f98237f349abfe7ebd83 -->
<!-- api: P:Inno.Editor.Shaders.ShaderInspectionSelection.nodes de51770b383d5be45f16e5f44adf75c52eb22a50a2d08fe86cc74c079c03de27 -->
<!-- api: T:Inno.Editor.Shaders.ShaderPreviewProviderAttribute 8106a2ae83678cea9fcf60a3bb7a02e0c26ad5fdcfcf0dbc658d937df8df0fb7 -->
<!-- api: M:Inno.Editor.Shaders.ShaderPreviewProviderAttribute.#ctor(System.String) ad3da8bc12d35ca8c80bc2ac0c7c4199e662e495fa7e1cf654cf13c549fc676a -->
<!-- api: P:Inno.Editor.Shaders.ShaderPreviewProviderAttribute.contractId 2328ae5fedf74d182f11a281cad8755f34fab7946806ab1cd9dba428b5ce082d -->
<!-- api: T:Inno.Editor.Shaders.ShaderPreviewProvider 8b7304ab6221436f7731d1bad238b71ab3616192d3b3f102981714db0d49c309 -->
<!-- api: M:Inno.Editor.Shaders.ShaderPreviewProvider.CreateLayer(Inno.Editor.Shaders.ShaderPreviewContext) 67ba65bd49cb7ef9617f19475108be1248d36e4436e3951feb4341f99eae5376 -->
<!-- api: M:Inno.Editor.Shaders.ShaderPreviewProvider.#ctor 78cfacb38ebdbbea60efc9535e2219856e52c7230050928843052b560e24417a -->
<!-- api: T:Inno.Editor.Shaders.ShaderPreviewContext 24fcfecf325a9d26687613f89fc534353d725b927f6f174481edcfbfc286460f -->
<!-- api: M:Inno.Editor.Shaders.ShaderPreviewContext.#ctor(Inno.Rendering.Runtime.RenderPersistentResourceId,Inno.Rendering.Assets.MaterialAsset,Inno.Rendering.Assets.RenderShaderArtifact,Inno.Rendering.Assets.ShaderDefinition,Inno.Core.Diagnostics.IDiagnosticReporter,System.Int32,System.Int32) 5283b0ab256415081f4e3da9cec378594d15f5d2351a49de03b7884acf56baff -->
<!-- api: P:Inno.Editor.Shaders.ShaderPreviewContext.resourceId 492cf38f165d7d65343774d1a4d6fc53fe89d863d5768bf43042d6510b8ba69c -->
<!-- api: P:Inno.Editor.Shaders.ShaderPreviewContext.material 665845cc4fa42fa6764f578c58a40b7415d9321e359efd618285780c4bb8e835 -->
<!-- api: P:Inno.Editor.Shaders.ShaderPreviewContext.artifact 72617ceb9f7dc6498c5fd325c242e8b7aac7d0afc9e366a863fd8ac03678719c -->
<!-- api: P:Inno.Editor.Shaders.ShaderPreviewContext.definition 48eba862c7c6011abe1d5a5a07b185e22f4be4fe75086a01843e12c54dcad3ec -->
<!-- api: P:Inno.Editor.Shaders.ShaderPreviewContext.diagnostics 1bc0795fa425f7e2e5ca5562d41450504000accd15dc150cb4e693a2c1df3393 -->
<!-- api: P:Inno.Editor.Shaders.ShaderPreviewContext.pixelWidth ccfaa88ebaea8b63b9e28b9564712b25628b1e7110b6387cb79eaf22c3885235 -->
<!-- api: P:Inno.Editor.Shaders.ShaderPreviewContext.pixelHeight a6d1c2947b6045d0447c60a5d3c9114f05e054933e9f19899cd7cade972d04a1 -->

| API 与源码 | 契约与参数 | 返回值 | 重要异常 |
| --- | --- | --- | --- |
| <a id="api-f3cfffca15badf5b"></a>[`public sealed class Inno.Editor.Shaders.MaterialDocuments : Inno.Editor.Core.EditorModule`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/MaterialDocuments.cs#L23) | Owns Material authoring sessions; native source editing uses the shared asset draft lifecycle. | — | — |
| <a id="api-907ea2fe17ff5173"></a>[`public Inno.Assets.Pipeline.AssetPipeline Inno.Editor.Shaders.MaterialDocuments.assets { get; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/MaterialDocuments.cs#L33) | Gets the authoritative asset catalog used by source-aware inspectors. | — | — |
| <a id="api-33f9da429cf4160e"></a>[`public Inno.Editor.Rendering.IEditorPreviewService Inno.Editor.Shaders.MaterialDocuments.previews { get; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/MaterialDocuments.cs#L40) | Gets the generation-scoped preview service shared by material presentations. | — | — |
| <a id="api-464ce7a6e4e865e7"></a>[`public System.Guid Inno.Editor.Shaders.MaterialDocuments.Open(Inno.Assets.AssetPath path)`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/MaterialDocuments.cs#L64) | Opens native source, including a source with failed import, without publishing its draft.<br>`path`: Mounted Material source path. | The persistent identity used for subsequent draft operations. | — |
| <a id="api-a2d5385fcfa3ab20"></a>[`public Inno.Rendering.Assets.MaterialAsset Inno.Editor.Shaders.MaterialDocuments.Read(System.Guid assetId)`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/MaterialDocuments.cs#L74) | Reads a detached current-generation Material; referenced assets remain canonical read-only inputs.<br>`assetId`: Open Material source identity. | The current unsaved Material value. | — |
| <a id="api-505edbee219e69f6"></a>[`public void Inno.Editor.Shaders.MaterialDocuments.Replace(System.Guid assetId, Inno.Rendering.Assets.MaterialAsset candidate, string? mergeKey = null)`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/MaterialDocuments.cs#L87) | Changes a draft through shared History without modifying its source or Scene/Game.<br>`assetId`: Open Material source identity.<br>`candidate`: Detached edited Material.<br>`mergeKey`: The current gesture key, or null for a discrete edit. | — | — |
| <a id="api-2a4ebf7f3be3a910"></a>[`public void Inno.Editor.Shaders.MaterialDocuments.ReplaceMany(System.Collections.Generic.IReadOnlyDictionary<System.Guid, Inno.Rendering.Assets.MaterialAsset> candidates, string? mergeKey = null)`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/MaterialDocuments.cs#L101) | Changes compatible selected drafts within one shared gesture.<br>`candidates`: Detached Material values keyed by persistent source identity.<br>`mergeKey`: The current gesture key, or null for a discrete edit. | — | — |
| <a id="api-248fad302df7a1d2"></a>[`public Inno.Editor.Assets.AssetDraftDocuments<Inno.Rendering.Assets.MaterialAsset>.Draft Inno.Editor.Shaders.MaterialDocuments.OpenDraft(Inno.Assets.AssetPath path)`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/MaterialDocuments.cs#L115) | Opens the current source draft without publishing it.<br>`path`: The isolated Material source path. | The generation-scoped draft; callers must not retain it across a reload. | — |
| <a id="api-738ddcbc341d8003"></a>[`public Inno.Editor.Assets.AssetDraftDocuments<Inno.Rendering.Assets.MaterialAsset>.Draft Inno.Editor.Shaders.MaterialDocuments.Open(Inno.Rendering.Assets.MaterialAsset material)`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/MaterialDocuments.cs#L126) | Resolves the source draft for a canonical Material identity.<br>`material`: The current canonical Material whose source is edited. | The generation-scoped editable source draft. | — |
| <a id="api-8bf7707677bcbf4c"></a>[`public Inno.Editor.Shaders.ShaderParameterPresentation Inno.Editor.Shaders.MaterialDocuments.Presentation(Inno.Rendering.Assets.ShaderAsset shader, Inno.Rendering.ShaderPropertyId propertyId, out string error)`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/MaterialDocuments.cs#L149) | Reads parameter presentation from the current authoring Shader graph.<br>`shader`: The canonical Shader used by the Material.<br>`propertyId`: The stable binding identity.<br>`error`: Receives the source diagnostic, or an empty string when presentation is available. | Detached presentation values; an unavailable graph uses the ordinary presentation defaults. | — |
| <a id="api-049e169aefa5a985"></a>[`protected override void Inno.Editor.Shaders.MaterialDocuments.OnStart(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/MaterialDocuments.cs#L195) | Initializes this feature when its owning runtime becomes active.<br>`context`: The context that supplies state and services for this operation. | — | — |
| <a id="api-f8470e3f67eb49af"></a>[`protected override void Inno.Editor.Shaders.MaterialDocuments.OnUpdate(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/MaterialDocuments.cs#L202) | Advances this feature using the current runtime state.<br>`context`: The context that supplies state and services for this operation. | — | — |
| <a id="api-268a6df1ecdcaf3c"></a>[`protected override void Inno.Editor.Shaders.MaterialDocuments.OnStop(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/MaterialDocuments.cs#L209) | Stops this feature before its owning runtime releases the active generation.<br>`context`: The context that supplies state and services for this operation. | — | — |
| <a id="api-e23a4c6f42c32517"></a>[`public sealed class Inno.Editor.Shaders.ShaderParameterPresentation : Inno.Core.Serialization.ISerializable`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/ShaderParameterPresentation.cs#L12) | Stores Editor-only parameter presentation by stable binding identity in the Shader's authoring graph. | — | — |
| <a id="api-0d132986831bde97"></a>[`public string Inno.Editor.Shaders.ShaderParameterPresentation.group { get; set; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/ShaderParameterPresentation.cs#L19) | Gets or sets the Material Inspector section; empty uses the standard Parameters section. | — | — |
| <a id="api-fd0476de74326342"></a>[`public string Inno.Editor.Shaders.ShaderParameterPresentation.description { get; set; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/ShaderParameterPresentation.cs#L23) | Gets or sets the hover description without adding a permanent paragraph. | — | — |
| <a id="api-9cc39cd1353549be"></a>[`public bool Inno.Editor.Shaders.ShaderParameterPresentation.visible { get; set; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/ShaderParameterPresentation.cs#L27) | Gets or sets whether this Material-owned parameter appears in ordinary Material Inspectors. | — | — |
| <a id="api-eb971e371f6ecc11"></a>[`public bool Inno.Editor.Shaders.ShaderParameterPresentation.hasRange { get; set; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/ShaderParameterPresentation.cs#L31) | Gets or sets whether a scalar Float control uses the declared editing bounds. | — | — |
| <a id="api-f2b0e4d2b1208ef3"></a>[`public double Inno.Editor.Shaders.ShaderParameterPresentation.minimum { get; set; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/ShaderParameterPresentation.cs#L35) | Gets or sets the inclusive editing minimum; existing values are not changed by presentation metadata. | — | — |
| <a id="api-76f06870eba00aa9"></a>[`public double Inno.Editor.Shaders.ShaderParameterPresentation.maximum { get; set; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/ShaderParameterPresentation.cs#L39) | Gets or sets the inclusive editing maximum. | — | — |
| <a id="api-c7993f1d4d510246"></a>[`public static Inno.Editor.Shaders.ShaderParameterPresentation Inno.Editor.Shaders.ShaderParameterPresentation.Read(Inno.Core.Graphs.GraphDocument graph, Inno.Rendering.ShaderPropertyId propertyId, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/ShaderParameterPresentation.cs#L59) | Reads detached presentation for one binding, defaulting only when no presentation was authored.<br>`graph`: Authoring graph, not a runtime Shader program.<br>`propertyId`: Stable parameter identity shared by all its nodes.<br>`serialization`: Current owner converters.<br>`context`: Complete owner serialization context. | The authored presentation, or ordinary visible, unbounded defaults. | — |
| <a id="api-c8029dc6f11147fd"></a>[`public static void Inno.Editor.Shaders.ShaderParameterPresentation.Write(Inno.Core.Graphs.GraphDocument graph, Inno.Rendering.ShaderPropertyId propertyId, Inno.Editor.Shaders.ShaderParameterPresentation presentation, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/ShaderParameterPresentation.cs#L92) | Writes presentation to a detached graph without changing any default, Material, or compiled program.<br>`graph`: Detached graph owned by the caller's History transaction.<br>`propertyId`: Stable parameter identity.<br>`presentation`: Detached Inspector settings.<br>`serialization`: Current owner converters.<br>`context`: Complete owner serialization context. | — | `System.ArgumentException`: The identity or range is invalid, or a bound cannot be represented by a scalar Shader Float. |
| <a id="api-5916bcf5fdb517f3"></a>[`public Inno.Editor.Shaders.ShaderParameterPresentation.ShaderParameterPresentation()`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/ShaderParameterPresentation.cs#L12) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3a7d973faf8a593f"></a>[`public sealed class Inno.Editor.Shaders.ShaderInspectionSelection`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderInspectionSelection.cs#L11) | Identifies a shader document and selected nodes without retaining a graph, extension object or delegate. | — | — |
| <a id="api-9b9c7a7246c3c287"></a>[`public Inno.Editor.Shaders.ShaderInspectionSelection.ShaderInspectionSelection(System.Guid assetId, System.Collections.Generic.IEnumerable<Inno.Core.Graphs.GraphNodeId> nodes)`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderInspectionSelection.cs#L23) | Creates a neutral Inspector selection separate from the Asset Browser's navigation.<br>`assetId`: Persistent shader asset identity.<br>`nodes`: Selected stable node identities; empty selects document settings. | — | — |
| <a id="api-978a076c153d9685"></a>[`public System.Guid Inno.Editor.Shaders.ShaderInspectionSelection.assetId { get; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderInspectionSelection.cs#L36) | Gets the persistent shader identity. | — | — |
| <a id="api-236e5adaf565c713"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Core.Graphs.GraphNodeId> Inno.Editor.Shaders.ShaderInspectionSelection.nodes { get; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderInspectionSelection.cs#L40) | Gets selected stable node identities. | — | — |
| <a id="api-e11c53e0bc4d5178"></a>[`public sealed class Inno.Editor.Shaders.ShaderPreviewProviderAttribute : System.Attribute`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviewProvider.cs#L17) | Associates a reloadable preview implementation with one open Shader contract.<br>`contractId`: Non-empty domain-owned contract identity. | — | — |
| <a id="api-38ee86e254ac59f3"></a>[`public Inno.Editor.Shaders.ShaderPreviewProviderAttribute.ShaderPreviewProviderAttribute(string contractId)`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviewProvider.cs#L17) | Associates a reloadable preview implementation with one open Shader contract.<br>`contractId`: Non-empty domain-owned contract identity. | — | — |
| <a id="api-1a29a7c07f6c72ed"></a>[`public string Inno.Editor.Shaders.ShaderPreviewProviderAttribute.contractId { get; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviewProvider.cs#L22) | Gets the non-empty Shader contract consumed by this preview. | — | — |
| <a id="api-155f73b4d6305de8"></a>[`public abstract class Inno.Editor.Shaders.ShaderPreviewProvider`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviewProvider.cs#L29) | Builds domain-owned preview geometry and pass inputs without changing a scene or canonical asset. | — | — |
| <a id="api-459cb3d4f863a3c5"></a>[`public abstract Inno.Editor.Rendering.EditorViewportLayer Inno.Editor.Shaders.ShaderPreviewProvider.CreateLayer(Inno.Editor.Shaders.ShaderPreviewContext context)`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviewProvider.cs#L40) | Creates one frame-local rendering layer through the ordinary Render Graph.<br>`context`: Detached Material, immutable candidate and isolated diagnostics for this invocation. | A domain-selected pipeline and frame payload; no instance may be retained beyond the submitted frame. | — |
| <a id="api-8857e0785476e17e"></a>[`protected Inno.Editor.Shaders.ShaderPreviewProvider.ShaderPreviewProvider()`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviewProvider.cs#L29) | Inherited or compiler-provided contract. | — | — |
| <a id="api-cec268921d9dc5c8"></a>[`public sealed class Inno.Editor.Shaders.ShaderPreviewContext`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviewProvider.cs#L46) | Carries frame-only preview inputs; persistent provider state must retain stable values rather than this object. | — | — |
| <a id="api-ee053b07553c0c68"></a>[`public Inno.Editor.Shaders.ShaderPreviewContext.ShaderPreviewContext(Inno.Rendering.Runtime.RenderPersistentResourceId resourceId, Inno.Rendering.Assets.MaterialAsset material, Inno.Rendering.Assets.RenderShaderArtifact artifact, Inno.Rendering.Assets.ShaderDefinition definition, Inno.Core.Diagnostics.IDiagnosticReporter diagnostics, int pixelWidth, int pixelHeight)`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviewProvider.cs#L72) | Creates a preview invocation from host-owned detached inputs.<br>`resourceId`: Isolated material publication and viewport resource identity.<br>`material`: Detached values with a current Shader reference; do not modify the reference.<br>`artifact`: Compiled candidate, never published as a canonical Shader.<br>`definition`: Detached contract captured by the candidate.<br>`diagnostics`: Preview-local reporter; errors must not contaminate project diagnostics.<br>`pixelWidth`: Positive target width.<br>`pixelHeight`: Positive target height. | — | — |
| <a id="api-db863f0e472dc24a"></a>[`public Inno.Rendering.Runtime.RenderPersistentResourceId Inno.Editor.Shaders.ShaderPreviewContext.resourceId { get; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviewProvider.cs#L92) | Gets the publication scope released by the host when this preview closes. | — | — |
| <a id="api-f0eb503b1dbd15e9"></a>[`public Inno.Rendering.Assets.MaterialAsset Inno.Editor.Shaders.ShaderPreviewContext.material { get; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviewProvider.cs#L96) | Gets the invocation-local Material values. | — | — |
| <a id="api-ed75c7021f5ffc78"></a>[`public Inno.Rendering.Assets.RenderShaderArtifact Inno.Editor.Shaders.ShaderPreviewContext.artifact { get; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviewProvider.cs#L100) | Gets the complete immutable GPU candidate. | — | — |
| <a id="api-123745de6ec130ed"></a>[`public Inno.Rendering.Assets.ShaderDefinition Inno.Editor.Shaders.ShaderPreviewContext.definition { get; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviewProvider.cs#L104) | Gets the candidate's exact contract, not the latest uncompiled source interface. | — | — |
| <a id="api-4bc576943224c4c9"></a>[`public Inno.Core.Diagnostics.IDiagnosticReporter Inno.Editor.Shaders.ShaderPreviewContext.diagnostics { get; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviewProvider.cs#L108) | Gets the host-owned, isolated diagnostic producer. | — | — |
| <a id="api-5b0ac395c4ba3bb7"></a>[`public int Inno.Editor.Shaders.ShaderPreviewContext.pixelWidth { get; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviewProvider.cs#L112) | Gets target width in physical pixels. | — | — |
| <a id="api-a33bab3b2d2aedf8"></a>[`public int Inno.Editor.Shaders.ShaderPreviewContext.pixelHeight { get; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviewProvider.cs#L116) | Gets target height in physical pixels. | — | — |

## 项目依赖

| 工程 | 边界 |
| --- | --- |
| `Inno.Core.Execution` | 实现依赖；不传递 compile surface |
| `Inno.Core.IO` | 实现依赖；不传递 compile surface |
| `Inno.Extensibility.Types` | 实现依赖；不传递 compile surface |
| `Inno.Scripting.Api` | 实现依赖；不传递 compile surface |
| `Inno.Rendering.Assets.Authoring` | 实现依赖；不传递 compile surface |
| `Inno.Rendering.Shaders` | 实现依赖；不传递 compile surface |
| `Inno.Editor.Interactions` | 实现依赖；不传递 compile surface |
| `Inno.Assets.Pipeline` | 公开签名依赖 |
| `Inno.Assets` | 公开签名依赖 |
| `Inno.Core.Diagnostics` | 公开签名依赖 |
| `Inno.Core.Graphs` | 公开签名依赖 |
| `Inno.Core.Serialization` | 公开签名依赖 |
| `Inno.Extensibility.Catalogs` | 公开签名依赖 |
| `Inno.Rendering.Assets` | 公开签名依赖 |
| `Inno.Rendering.Runtime` | 公开签名依赖 |
| `Inno.Rendering` | 公开签名依赖 |
| `Inno.Editor.Core` | 公开签名依赖 |
| `Inno.Editor.Assets` | 公开签名依赖 |
| `Inno.Editor.Rendering` | 公开签名依赖 |
