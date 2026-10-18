# Inno.Editor.Shaders

[Editor 索引](README.md) · [Wiki 首页](../README.md) · [Shader Editor](Inno.Editor.Panel.ShaderEditor.md) · [Shader 编译](../rendering/Inno.Rendering.Shaders.md)

## 职责与边界

可复用的 Shader/Material 创作功能。节点编译属于 Rendering.Shaders；呈现协议独立于具体 Panel。节点配置在统一 Inspector 编辑，画布保留摘要与连接。

优先用 `.ishader` 的 Node Inputs / Node Outputs 定义组合型多输入、多输出节点；宿主会自动提供创建菜单、端口 Inspector、依赖、内联和循环诊断。只有新增公共 IR 无法表达的原语时才实现 `IShaderNodeCompiler`，只有节点确实需要专用交互时才增加 `ShaderNodeDrawer`。

## 初始化与生命周期

宿主在 TypeCatalog 与预览服务建立后创建 ShaderNodeDrawerRegistry，在 Editor feature 停止时 Dispose。
每次查询与绘制持有共享 generation operation；候选发现重复 ID 时失败，旧 provider 按 TypeRegistry 的事务与退休协议管理。
Drawer 不得保存 DrawContext、PreviewHandle、Asset 或历史回调到下一帧／下一代。

## 公开 API

| API | 语义 |
| --- | --- |
| `ShaderNodeDrawerAttribute(definitionId, displayName, createPath, createOrder, separatorBefore)` | 稳定节点 ID、可选显示名称与节点创建菜单呈现；插件可贡献自己的分组、顺序和同级分隔，不参与编译语义 |
| `ShaderNodeDrawer.Draw(context)` | 统一 Inspector 中的帧内绘制入口 |
| `ShaderNodeDrawContext(node, serialization, context, write, previews, inspection, readOnly)` | 独立节点快照、完整 owner context、统一草稿 History 写入、预览与共享 Inspector 上下文 |
| `nodeId`、`previews` | 稳定控件身份和当前 generation 预览服务 |
| `Read<T>(key, defaultValue)` | 解码独立属性，缺失使用声明默认值，损坏明确失败 |
| `Write<T>(key, value, continuous)` | 向宿主提交中立值；进入草稿 History，不保存或发布 Asset |
| `DrawProperty<T>(key, label, defaultValue)` | 复用现有属性 Drawer、资产选择/拖放和数值控件；受草稿 History 与只读检查约束 |
| `ShaderNodeDrawerRegistry(types)`、`TryDraw(id, context)`、`Dispose()` | 按 generation 发现、调用和退休呈现；缺少 Drawer 时返回 false |
| `GetDisplayName(definitionId)` | 获取本代际中立显示名称；未贡献名称时返回 null，由宿主提供通用名称 |
| `TryGetPresentation(definitionId, out presentation)` | 获取本代际不可变创建菜单呈现；未贡献时由 Shader Editor 使用内置中立分类 |
| `MaterialDocuments.Open(path)`、`Read(id)`、`Replace(id, material, finishGesture)`、`Commit(id)` | 原生 Material 草稿；读回的是独立可编辑对象，显式保存前不发布 |
| `MaterialDocuments.ReplaceMany(candidates, finishGesture)`、`CommitMany(ids)` | 一组兼容材质共享一次 History 事务，不提供跨文件原子保存承诺 |
| `ShaderPropertyInspector.Draw(...)`、`Compatible(type, kind)` | Shader 默认值与 Material 覆盖复用属性 Drawer；Float/向量/线性 HDR Color/Matrix/Texture/Sampler，精确类型匹配 |
| `ShaderParameterPresentation`：`group`、`description`、`visible`、`hasRange`、`minimum`、`maximum` | Editor-only 参数展示。按稳定绑定 ID 共享，不改变已有数值，不进入运行时定义或 Player |
| `ShaderParameterPresentation.Read(graph, propertyId, serialization, context)`、`Write(...)` | 读写独立图中的原生展示元数据；调用方负责共享 History。范围必须有限且有序；不是资产保存 API |
| `ShaderPreviewProviderAttribute(contractId)`、`contractId` | 按开放 Shader Contract 注册预览；重复契约阻止候选激活 |
| `ShaderPreviewProvider.CreateLayer(context)` | 插件贡献几何、环境、Pass 输入和 Pipeline，不修改 Scene 或 canonical Material |
| `ShaderPreviewContext(resourceId, material, artifact, definition, diagnostics, pixelWidth, pixelHeight)` 及同名只读属性 | 帧内的独立发布范围、草稿值、不可变编译产物、精确接口、隔离诊断和像素尺寸；禁止跨帧保留 |
| `ShaderPreviews.DrawMaterial(ownerId, material, logicalSize)` | 对独立 Material 覆盖值执行实际 GPU 预览，不保存或应用源资产 |
| `ShaderPreviews.Draw(ownerId, material, compilation, logicalSize)` | Shader 草稿编译结果的实际 GPU 预览；失败时明确标识 last-good |
| `ShaderPreviews.Release(ownerId)` | 关闭预览并释放对应视口和 GPU 程序；未绘制的预览自动退休 |

公共签名涉及 Graph、Serialization、Rendering Preview 与 TypeCatalog，其项目引用向下游公开。
节点编译与脚本导出依赖仅作为实现依赖。

```csharp
using InnoEditor.Shaders;

[ShaderNodeDrawer("example.surface")]
public sealed class SurfaceDrawer : ShaderNodeDrawer
{
    public override void Draw(ShaderNodeDrawContext context)
    {
        context.DrawProperty("gain", "Gain", 1f);
    }
}
```

Editor 脚本导出 Drawer、预览 Provider、标记、帧内 Context、MaterialDocuments、ShaderPreviews 和 ShaderPropertyInspector，命名空间为 `InnoEditor.Shaders`。Registry 由宿主管理。

## 实际渲染预览

Material Inspector 顶部独立的 Preview 区域与 Shader Inspector 中始终可见的 Preview 共用此模块。没有可编辑属性与关键词时，Parameters 区域显示空状态说明。输出预览表示整个 Shader 的结果，不冒充任意中间节点的数值可视化。
领域通过 `ShaderPreviewProvider` 提供与 Shader Contract 匹配的预览，使用普通 Render Graph 和独立材质覆盖绘制；预览资源由该预览作用域持有。
不同领域通过自己的 Contract 提供网格、环境与 Pass 参数，通用引擎没有 Sprite 分支。若多种可预览 Contract 同时匹配，要求选择 Technique，不按发现顺序猜测。

预览 Shader 使用隔离编译产物；Material 使用当前 Shader 产物和独立覆盖。GPU 创建、反射/能力校验、绑定以及 last-good 复用正式资源链，但缓存键包含独立 owner scope，错误只进入预览 reporter。
预览帧只保留本帧的 Provider 结果；后续状态仅保存稳定 ID、计数和中立诊断。宿主 generation 切换时统一清理视口及注册，Provider 通过 TypeRegistry 候选事务退休。

## Material 工作流

File Browser 的 Create 菜单支持创建 Material 和从选中 Shader 创建 Material。单击材质进入 Inspector；Command/Ctrl+单击可选择多个材质，兼容参数在同一 Inspector 中共同编辑。
继承值与独立 override 分开显示，支持单项/全部重置、关键词、Technique，以及更换 Shader 后保留的未解析参数。
颜色值按线性 RGBA、允许 HDR 输入，不对底层浮点数作显示精度截断。纹理选择/拖放与采样器使用既有属性 Drawer。

参数节点 Inspector 可编辑分组、hover 说明、材质可见性与 Float 编辑范围。多选材质只显示共同可见且类型兼容的参数，数值范围使用交集；不把改动前已超界的值静默钳制。隐藏是创作 UI 策略，不禁止运行时通过稳定 ID 设置参数。
展示记录保存在 `inno.editor.parameter.*` 图 metadata，由同一参数的所有节点共享。语义指纹排除此保留前缀；仅改展示不重编译 GPU 程序。Material 使用已导入的 Shader 创作快照，按资产 contentVersion 缓存，不每帧重读整个图；未保存 Shader 草稿不会改变 Material Inspector。

保存复用 headless Document Service；Inspector Header 提供 Save/Revert，关闭确认和全部保存仍走同一服务，不另建状态系统。MaterialDocuments 组合 [AssetDraftDocuments](Inno.Editor.Assets.md)，恢复数据在 Library/Editor/AssetDrafts/inno.material；外部修改冲突保留两份内容，不覆盖外部源。
资产/节点多选只保存稳定 ID；Drawer 重新查询当前 generation，不把节点选择当作 File Browser 选择。
完整渲染预览及 UI 验收仍见[当前验收](../architecture/CURRENT_ACCEPTANCE.md)，不能把编译预览等同为画面预览。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Editor.Shaders.MaterialDocuments`

<!-- api: T:Inno.Editor.Shaders.MaterialDocuments 1b0656a2573ae7a939d4308c8276064d4624c9540437f2b9590f61cda25c43d7 -->
<!-- api: M:Inno.Editor.Shaders.MaterialDocuments.Open(Inno.Assets.AssetPath) 9963afc9b37f8f4169e5a9c694e13654b3425238f22391da19e780faa7b77de6 -->
<!-- api: M:Inno.Editor.Shaders.MaterialDocuments.Read(System.Guid) aaf3afc046125423a8d8ac4039a91a97f21c2b86578835c7b43a3f6513ad4f65 -->
<!-- api: M:Inno.Editor.Shaders.MaterialDocuments.Replace(System.Guid,Inno.Rendering.Assets.MaterialAsset,System.Boolean) 86f4cc7e5cd7f919e050f71cabbbb7184512e3ec3f77de8fb80bef931b1debc7 -->
<!-- api: M:Inno.Editor.Shaders.MaterialDocuments.Commit(System.Guid) cfb46ddc5d2dca1376f682dc9681d5dcdf2ab95453a8a6540cd94cb12a0d435c -->
<!-- api: M:Inno.Editor.Shaders.MaterialDocuments.ReplaceMany(System.Collections.Generic.IReadOnlyDictionary{System.Guid,Inno.Rendering.Assets.MaterialAsset},System.Boolean) 37f1de1527b430f902bdebf6476cb2b44b240b35749d9db2b804456495851e10 -->
<!-- api: M:Inno.Editor.Shaders.MaterialDocuments.CommitMany(System.Collections.Generic.IEnumerable{System.Guid}) 75a8f46554e78256c0a2f72adffd7a2996cb52af43ffaff1488a08085ade6195 -->
<!-- api: M:Inno.Editor.Shaders.MaterialDocuments.OnStart(Inno.Editor.Core.EditorContext) 978c23e17cb008269041800a42474e92675b096f36de8db95624554b38d623b7 -->
<!-- api: M:Inno.Editor.Shaders.MaterialDocuments.OnUpdate(Inno.Editor.Core.EditorContext) 23d0c13311e2b8af244cf0d8f1e5e30e0895f003e1561159696e8120ea3fe6bb -->
<!-- api: M:Inno.Editor.Shaders.MaterialDocuments.OnStop(Inno.Editor.Core.EditorContext) 9f17c2507991b95522197194c5372005b0a57fd4c5a83c8d2d5094a673831ea0 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7ae31d514cac4dd9"></a>[`public void Inno.Editor.Shaders.MaterialDocuments.Commit(System.Guid assetId)`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/MaterialDocuments.cs#L92) | Finishes a draft gesture without saving its source.<br>`assetId`：Open Material source identity. | — | — |
| <a id="api-628a7b723c9462a5"></a>[`public void Inno.Editor.Shaders.MaterialDocuments.CommitMany(System.Collections.Generic.IEnumerable<System.Guid> assetIds)`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/MaterialDocuments.cs#L115) | Completes a multi-document gesture as one shared History transaction.<br>`assetIds`：Participating persistent source identities. | — | — |
| <a id="api-049e169aefa5a985"></a>[`protected override void Inno.Editor.Shaders.MaterialDocuments.OnStart(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/MaterialDocuments.cs#L172) | Initializes this feature when its owning runtime becomes active.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-268a6df1ecdcaf3c"></a>[`protected override void Inno.Editor.Shaders.MaterialDocuments.OnStop(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/MaterialDocuments.cs#L186) | Stops this feature before its owning runtime releases the active generation.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-f8470e3f67eb49af"></a>[`protected override void Inno.Editor.Shaders.MaterialDocuments.OnUpdate(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/MaterialDocuments.cs#L179) | Advances this feature using the current runtime state.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-464ce7a6e4e865e7"></a>[`public System.Guid Inno.Editor.Shaders.MaterialDocuments.Open(Inno.Assets.AssetPath path)`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/MaterialDocuments.cs#L54) | Opens native source, including a source with failed import, without publishing its draft.<br>返回：The persistent identity used for subsequent draft operations.<br>`path`：Mounted Material source path. | — | — |
| <a id="api-a2d5385fcfa3ab20"></a>[`public Inno.Rendering.Assets.MaterialAsset Inno.Editor.Shaders.MaterialDocuments.Read(System.Guid assetId)`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/MaterialDocuments.cs#L65) | Reads a detached current-generation Material; referenced assets remain canonical read-only inputs.<br>返回：The current unsaved Material value.<br>`assetId`：Open Material source identity. | — | — |
| <a id="api-2a12eb2cd283e9a6"></a>[`public void Inno.Editor.Shaders.MaterialDocuments.Replace(System.Guid assetId, Inno.Rendering.Assets.MaterialAsset candidate, bool finishGesture = true)`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/MaterialDocuments.cs#L79) | Changes a draft through shared History without modifying its source or Scene/Game.<br>`assetId`：Open Material source identity.<br>`candidate`：Detached edited Material.<br>`finishGesture`：Whether this sample finishes the active gesture. | — | — |
| <a id="api-eb953f9bd7094a60"></a>[`public void Inno.Editor.Shaders.MaterialDocuments.ReplaceMany(System.Collections.Generic.IReadOnlyDictionary<System.Guid, Inno.Rendering.Assets.MaterialAsset> candidates, bool finishGesture = true)`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/MaterialDocuments.cs#L103) | Changes compatible selected drafts within one shared gesture.<br>`candidates`：Detached Material values keyed by persistent source identity.<br>`finishGesture`：Whether this sample finishes the shared gesture. | — | — |
| <a id="api-f3cfffca15badf5b"></a>[`public sealed class Inno.Editor.Shaders.MaterialDocuments : Inno.Editor.Core.EditorModule`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/MaterialDocuments.cs#L24) | Owns Material authoring sessions; native source editing uses the shared asset draft lifecycle. | — | — |

### `Inno.Editor.Shaders.ShaderInspectionSelection`

<!-- api: T:Inno.Editor.Shaders.ShaderInspectionSelection 8e06df79ba9dcd40a45ba763c5f9858979da6381980a512c17248dd5030d5308 -->
<!-- api: M:Inno.Editor.Shaders.ShaderInspectionSelection.#ctor(System.Guid,System.Collections.Generic.IEnumerable{Inno.Core.Graphs.GraphNodeId}) e4446925d124a6c7ae5fc9ac72bd21c34c363986396f4f024010b54f97a4a349 -->
<!-- api: P:Inno.Editor.Shaders.ShaderInspectionSelection.assetId e9c4903274ec0328cef80cd28681803a1d2ec9530359f98237f349abfe7ebd83 -->
<!-- api: P:Inno.Editor.Shaders.ShaderInspectionSelection.nodes de51770b383d5be45f16e5f44adf75c52eb22a50a2d08fe86cc74c079c03de27 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9b9c7a7246c3c287"></a>[`public Inno.Editor.Shaders.ShaderInspectionSelection.ShaderInspectionSelection(System.Guid assetId, System.Collections.Generic.IEnumerable<Inno.Core.Graphs.GraphNodeId> nodes)`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderInspectionSelection.cs#L23) | Creates a neutral Inspector selection separate from the Asset Browser's navigation.<br>`assetId`：Persistent shader asset identity.<br>`nodes`：Selected stable node identities; empty selects document settings. | — | — |
| <a id="api-978a076c153d9685"></a>[`public System.Guid Inno.Editor.Shaders.ShaderInspectionSelection.assetId { get; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderInspectionSelection.cs#L36) | Gets the persistent shader identity. | — | — |
| <a id="api-236e5adaf565c713"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Core.Graphs.GraphNodeId> Inno.Editor.Shaders.ShaderInspectionSelection.nodes { get; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderInspectionSelection.cs#L40) | Gets selected stable node identities. | — | — |
| <a id="api-3a7d973faf8a593f"></a>[`public sealed class Inno.Editor.Shaders.ShaderInspectionSelection`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderInspectionSelection.cs#L11) | Identifies a shader document and selected nodes without retaining a graph, extension object or delegate. | — | — |

### `Inno.Editor.Shaders.ShaderNodeDrawContext`

<!-- api: T:Inno.Editor.Shaders.ShaderNodeDrawContext 8dd6a47b08c35441731c66cd6b3bdde06f76f18ada5db55364543c163b2452d3 -->
<!-- api: M:Inno.Editor.Shaders.ShaderNodeDrawContext.#ctor(Inno.Core.Graphs.GraphNodeRecord,Inno.Core.Serialization.SerializationRegistry,Inno.Core.Serialization.SerializationContext,System.Action{System.String,Inno.Core.Graphs.GraphSerializedValue,System.Boolean},Inno.Editor.Rendering.IEditorPreviewService,Inno.Editor.Inspection.InspectionDrawContext,System.Boolean) e24436673ad06a4b595dbf08c8444cd29b1b841d88744ec261fd9e263c87081d -->
<!-- api: P:Inno.Editor.Shaders.ShaderNodeDrawContext.previews bc7c300dfd4daae43fc172e99224e54a0047be53c0cff4cb288d36f67ef25c38 -->
<!-- api: P:Inno.Editor.Shaders.ShaderNodeDrawContext.nodeId 7c66ca394811ea4e9b42c88608740be1ae98feca38b39e6e4b813f241133d508 -->
<!-- api: M:Inno.Editor.Shaders.ShaderNodeDrawContext.Read``1(System.String,``0) eea07b48680a9bbf0159325ad54855abc7a4e46eb7f421a64b8b8f8e50e953bd -->
<!-- api: M:Inno.Editor.Shaders.ShaderNodeDrawContext.Write``1(System.String,``0,System.Boolean) ac9f3dbfc5cdbfeea6bf25826c41514d75bde006b2e6d98c00f1e63bbe1e9a7b -->
<!-- api: M:Inno.Editor.Shaders.ShaderNodeDrawContext.DrawProperty``1(System.String,System.String,``0) 2baced334f1d130d4d3f565e6ac21bf71eb4b3b78a7d3c2c85c2a864af1e8cfe -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-66e28bca8d5c22b6"></a>[`public Inno.Editor.Shaders.ShaderNodeDrawContext.ShaderNodeDrawContext(Inno.Core.Graphs.GraphNodeRecord node, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context, System.Action<string, Inno.Core.Graphs.GraphSerializedValue, bool> write, Inno.Editor.Rendering.IEditorPreviewService previews, Inno.Editor.Inspection.InspectionDrawContext inspection, bool readOnly)`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawer.cs#L172) | Creates invocation-scoped access for a graph or Inspector host.<br>`node`：Detached node values for this invocation.<br>`serialization`：Current owner converter registry.<br>`context`：Complete owner reference context.<br>`write`：Host callback recording changes in the shared draft history.<br>`previews`：Current generation preview service.<br>`inspection`：Shared Inspector context, including native property controls and drag/drop.<br>`readOnly`：Whether the source belongs to an immutable installation. | — | — |
| <a id="api-17a581a14cd8b403"></a>[`public void Inno.Editor.Shaders.ShaderNodeDrawContext.DrawProperty<T>(string key, string label, T defaultValue)`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawer.cs#L261) | Draws a node property through the existing Inspector controls, including precise numbers and asset picking.<br>`key`：Stable node property identity.<br>`label`：Artist-facing property label.<br>`defaultValue`：Value used only when no property is stored. | — | — |
| <a id="api-7af913876be550e7"></a>[`public T Inno.Editor.Shaders.ShaderNodeDrawContext.Read<T>(string key, T defaultValue)`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawer.cs#L215) | Reads a detached property value; missing values use the node's declared default.<br>返回：The detached value. Malformed stored data is not silently replaced.<br>`key`：Stable node-local property key.<br>`defaultValue`：Default used only when the property is absent. | — | — |
| <a id="api-bcdba824d2c589af"></a>[`public void Inno.Editor.Shaders.ShaderNodeDrawContext.Write<T>(string key, T value, bool continuous = false)`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawer.cs#L236) | Records an unsaved draft edit through shared history; only an explicit document save applies it to the asset.<br>`key`：Stable node-local property key.<br>`value`：New detached value.<br>`continuous`：Whether this sample belongs to the active text or numeric gesture. | — | — |
| <a id="api-65203447334ed1e9"></a>[`public Inno.Core.Graphs.GraphNodeId Inno.Editor.Shaders.ShaderNodeDrawContext.nodeId { get; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawer.cs#L198) | Gets the stable node identity for Editor widget IDs. | — | — |
| <a id="api-09f2fc2e5cc1c5e6"></a>[`public Inno.Editor.Rendering.IEditorPreviewService Inno.Editor.Shaders.ShaderNodeDrawContext.previews { get; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawer.cs#L193) | Gets the shared generation-scoped preview service for optional inline texture previews; valid only during this draw. | — | — |
| <a id="api-d3cb70bf094f2d95"></a>[`public sealed class Inno.Editor.Shaders.ShaderNodeDrawContext`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawer.cs#L140) | Exposes detached node values and one-gesture writes without exposing the live graph document. | — | — |

### `Inno.Editor.Shaders.ShaderNodeDrawer`

<!-- api: T:Inno.Editor.Shaders.ShaderNodeDrawer d3b4b7d11ce8c84a2cb4e3f481d1f0dae31ae4bf8ea24f69c26df25288ebebd8 -->
<!-- api: M:Inno.Editor.Shaders.ShaderNodeDrawer.Draw(Inno.Editor.Shaders.ShaderNodeDrawContext) b464a6244b503a5e19101d4892b1be4ca5bc39991445b4d35fa8cf4ca9b7f3a8 -->
<!-- api: M:Inno.Editor.Shaders.ShaderNodeDrawer.#ctor 200c45da7111e09b6261a35bfb098faf07af00275fb755da934d9a3f4800e5f5 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-184faeb2d9e0a657"></a>[`protected Inno.Editor.Shaders.ShaderNodeDrawer.ShaderNodeDrawer()`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawer.cs#L126) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-5e649b4efac54af3"></a>[`public abstract void Inno.Editor.Shaders.ShaderNodeDrawer.Draw(Inno.Editor.Shaders.ShaderNodeDrawContext context)`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawer.cs#L134) | Draws the selected node's controls using the shared Inspector styling.<br>`context`：Frame-scoped value access and undoable writes. | — | — |
| <a id="api-ff8def2b30c49a0b"></a>[`public abstract class Inno.Editor.Shaders.ShaderNodeDrawer`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawer.cs#L126) | Provides reloadable Inspector controls; instances must not retain frame contexts or asset objects. | — | — |

### `Inno.Editor.Shaders.ShaderNodeDrawerAttribute`

<!-- api: T:Inno.Editor.Shaders.ShaderNodeDrawerAttribute c62405ce25a19d5171bc968a45c69fa63831a5520fabde238ade9d2f595543dd -->
<!-- api: M:Inno.Editor.Shaders.ShaderNodeDrawerAttribute.#ctor(System.String,System.String,System.String,System.Int32,System.Boolean) a8332f298b494288f9e5291e0429ec981688f5a92200647fb891a2263f964333 -->
<!-- api: P:Inno.Editor.Shaders.ShaderNodeDrawerAttribute.definitionId 16a6888f043933b7dc375e3b8e0c16528ce6086b5f772630fdea953a96e842e6 -->
<!-- api: P:Inno.Editor.Shaders.ShaderNodeDrawerAttribute.displayName b4032833c2315b1fb7e8b9181042038f1cee84eb38fc0f8d640dd650d559b9af -->
<!-- api: P:Inno.Editor.Shaders.ShaderNodeDrawerAttribute.createPath bfd57d4eef0c39fc5f94236c1ed120e727dcbc7da0aab5b348c6524b7e5aaf4d -->
<!-- api: P:Inno.Editor.Shaders.ShaderNodeDrawerAttribute.createOrder a551d874f220563c4493e778cb89fbcd1be7c08bf1efc597409a2c5f41a1ff2a -->
<!-- api: P:Inno.Editor.Shaders.ShaderNodeDrawerAttribute.separatorBefore 8d1fb86c2c23eba4fb78b9b14edcf9b77bed885669e95ee37ef505e2c0ad3863 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a53e11bb94aa2110"></a>[`public Inno.Editor.Shaders.ShaderNodeDrawerAttribute.ShaderNodeDrawerAttribute(string definitionId, string displayName = "", string createPath = "", int createOrder = 0, bool separatorBefore = false)`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawer.cs#L34) | Associates the drawer with one stable node definition.<br>`definitionId`：The compiler-independent graph node identity.<br>`displayName`：Optional artist-facing name used in node headers and creation menus.<br>`createPath`：Optional slash-delimited path below Create. Empty uses the host's built-in classification.<br>`createOrder`：Stable ordering value among sibling creation groups and nodes.<br>`separatorBefore`：Whether the contributed creation group is preceded by a separator at its own level. | — | — |
| <a id="api-590299b2ac9c9653"></a>[`public int Inno.Editor.Shaders.ShaderNodeDrawerAttribute.createOrder { get; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawer.cs#L63) | Gets the creation-menu order. | — | — |
| <a id="api-295d2122cdb81175"></a>[`public string Inno.Editor.Shaders.ShaderNodeDrawerAttribute.createPath { get; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawer.cs#L59) | Gets the optional creation-menu path below Create. | — | — |
| <a id="api-9cc66a3d49d38200"></a>[`public string Inno.Editor.Shaders.ShaderNodeDrawerAttribute.definitionId { get; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawer.cs#L51) | Gets the stable node identity handled by this drawer. | — | — |
| <a id="api-250dcf68f683206d"></a>[`public string Inno.Editor.Shaders.ShaderNodeDrawerAttribute.displayName { get; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawer.cs#L55) | Gets the optional presentation-only name; it does not participate in Shader compilation or identity. | — | — |
| <a id="api-cf754edf8b1ff54d"></a>[`public bool Inno.Editor.Shaders.ShaderNodeDrawerAttribute.separatorBefore { get; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawer.cs#L67) | Gets whether the contributed creation group starts a new visual section. | — | — |
| <a id="api-76aba4cd93347ad3"></a>[`public sealed class Inno.Editor.Shaders.ShaderNodeDrawerAttribute : System.Attribute`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawer.cs#L14) | Registers Editor-only node controls independently from a node's shader compiler. | — | — |

### `Inno.Editor.Shaders.ShaderNodeDrawerRegistry`

<!-- api: T:Inno.Editor.Shaders.ShaderNodeDrawerRegistry 2f152b0b5b7d0dd455cdda31ddcaf96405fa5e99dc6ea346a85aa7ebe3236f1e -->
<!-- api: M:Inno.Editor.Shaders.ShaderNodeDrawerRegistry.#ctor(Inno.Extensibility.Types.TypeCatalog) b475f7cb8304402649e5540c65a4c5051ce5812d3bdbd32aeba65673c029a972 -->
<!-- api: M:Inno.Editor.Shaders.ShaderNodeDrawerRegistry.TryDraw(System.String,Inno.Editor.Shaders.ShaderNodeDrawContext) 987cf5495fc6586659534b90223997cef554d255d4d8bac626775f39902efd28 -->
<!-- api: M:Inno.Editor.Shaders.ShaderNodeDrawerRegistry.GetDisplayName(System.String) 183286cd61728ed3cd76393d9854f89ec3b6079d336ddad5a12b9c0f2bc1fb6c -->
<!-- api: M:Inno.Editor.Shaders.ShaderNodeDrawerRegistry.TryGetPresentation(System.String,Inno.Editor.Shaders.ShaderNodePresentation@) 3db462c60c53983879a494773d1a3722c77aa4200aad17e658591f5ae03c8401 -->
<!-- api: M:Inno.Editor.Shaders.ShaderNodeDrawerRegistry.Dispose 49c9ab5849c29044f6459dfe5fde83c7736d2b346bb8c9e26e5c180f2fa88dce -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a7417045341f08b3"></a>[`public Inno.Editor.Shaders.ShaderNodeDrawerRegistry.ShaderNodeDrawerRegistry(Inno.Extensibility.Types.TypeCatalog types)`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawerRegistry.cs#L23) | Creates a presentation registry owned by the current editor feature lifetime.<br>`types`：Shared type catalog, which must outlive this registry. | — | — |
| <a id="api-15519184f73b16d4"></a>[`public void Inno.Editor.Shaders.ShaderNodeDrawerRegistry.Dispose()`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawerRegistry.cs#L90) | Retires the current presentation providers through the shared lifecycle. | — | — |
| <a id="api-322227df68ac297a"></a>[`public string? Inno.Editor.Shaders.ShaderNodeDrawerRegistry.GetDisplayName(string definitionId)`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawerRegistry.cs#L61) | Resolves an optional artist-facing name without executing a drawer or retaining its generation.<br>返回：The contributed name, or null when the host should use its generic presentation.<br>`definitionId`：Stable compiler-independent node identity. | — | — |
| <a id="api-295dbee56b449c29"></a>[`public bool Inno.Editor.Shaders.ShaderNodeDrawerRegistry.TryDraw(string definitionId, Inno.Editor.Shaders.ShaderNodeDrawContext context)`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawerRegistry.cs#L41) | Invokes optional node controls within one generation lease.<br>返回：True when a registered drawer rendered the controls; false leaves presentation to the host.<br>`definitionId`：Stable node definition identity.<br>`context`：Invocation-scoped host values, history callback and previews. | — | — |
| <a id="api-264a7bedcda33a20"></a>[`public bool Inno.Editor.Shaders.ShaderNodeDrawerRegistry.TryGetPresentation(string definitionId, out Inno.Editor.Shaders.ShaderNodePresentation presentation)`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawerRegistry.cs#L79) | Gets optional creation-menu presentation without executing the drawer.<br>返回：True when the current generation contributes presentation for the node; otherwise false.<br>`definitionId`：Stable compiler-independent node identity.<br>`presentation`：Receives immutable creation-menu presentation when registered. | — | — |
| <a id="api-5cd2cd4900f8d737"></a>[`public sealed class Inno.Editor.Shaders.ShaderNodeDrawerRegistry : System.IDisposable`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawerRegistry.cs#L12) | Shares generation-scoped node presentation between graph and Inspector hosts. | — | — |

### `Inno.Editor.Shaders.ShaderNodePresentation`

<!-- api: T:Inno.Editor.Shaders.ShaderNodePresentation ff78161998df04ec9bec462d8438d471ef7b9da3f96a495a1c137925272c2ee3 -->
<!-- api: M:Inno.Editor.Shaders.ShaderNodePresentation.#ctor(System.String,System.String,System.Int32,System.Boolean) 1cbea3b3c129acea7e998d246726efcfbd19e19f4b4106cc2f055e050cfb5ea1 -->
<!-- api: P:Inno.Editor.Shaders.ShaderNodePresentation.displayName 033fc2f8883493485e3a387158d137996115f68c40149c73d8e334c2b58545a7 -->
<!-- api: P:Inno.Editor.Shaders.ShaderNodePresentation.createPath 04d4a7c7e549f08aef542cf61540adf76c28166f2b4a6f4abc976da6337ee809 -->
<!-- api: P:Inno.Editor.Shaders.ShaderNodePresentation.createOrder 38626a650d096cdc082a61dd0b972a86e4bb96b7b7ac6c63bba3cd19519a1892 -->
<!-- api: P:Inno.Editor.Shaders.ShaderNodePresentation.separatorBefore 25162b19afc1fac93fbf07d30e5e0139ae2d8024d3b7f64b546a6a6997cdba8d -->
<!-- api: M:Inno.Editor.Shaders.ShaderNodePresentation.ToString e83d1e416982b2c3a46647fec045d1f90d8bf161087b18556f82ea6b3d5ded18 -->
<!-- api: M:Inno.Editor.Shaders.ShaderNodePresentation.op_Inequality(Inno.Editor.Shaders.ShaderNodePresentation,Inno.Editor.Shaders.ShaderNodePresentation) dc5b408dd6fc3223bb84d5568c09ab9b66d58c607d80b18db3f335d4aed1db75 -->
<!-- api: M:Inno.Editor.Shaders.ShaderNodePresentation.op_Equality(Inno.Editor.Shaders.ShaderNodePresentation,Inno.Editor.Shaders.ShaderNodePresentation) 99bebfa3140c5a9982fa3d4eafd0cd1179370e09fad8af0126ceb1a76cc89b30 -->
<!-- api: M:Inno.Editor.Shaders.ShaderNodePresentation.GetHashCode c306d157782694dfe1736df0fff92de4c881b69cbc29898b54f40766834be8e3 -->
<!-- api: M:Inno.Editor.Shaders.ShaderNodePresentation.Equals(System.Object) 68e87e28729c41a65b2beda0cae604ed4241b78a302185b67f6161e5b57f1dfa -->
<!-- api: M:Inno.Editor.Shaders.ShaderNodePresentation.Equals(Inno.Editor.Shaders.ShaderNodePresentation) 81e41b1f51102edc7ce8277c3986a0bc2af462f048fafb0074a00c679c9e9423 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-cf9f17fc85f1d650"></a>[`public Inno.Editor.Shaders.ShaderNodePresentation.ShaderNodePresentation(string displayName, string createPath, int createOrder, bool separatorBefore)`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawer.cs#L90) | Creates presentation metadata for one Shader node definition.<br>`displayName`：Optional artist-facing node name.<br>`createPath`：Optional slash-delimited creation-menu path.<br>`createOrder`：Stable ordering value within the creation menu.<br>`separatorBefore`：Whether the contributed group starts a visual section. | — | — |
| <a id="api-36ff308ac3d02e32"></a>[`public bool Inno.Editor.Shaders.ShaderNodePresentation.Equals(Inno.Editor.Shaders.ShaderNodePresentation other)`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawer.cs#L73) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-1c89ea6d29f2a24d"></a>[`public override bool Inno.Editor.Shaders.ShaderNodePresentation.Equals(object obj)`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawer.cs#L73) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-c947dabb7bc7a74a"></a>[`public override int Inno.Editor.Shaders.ShaderNodePresentation.GetHashCode()`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawer.cs#L73) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-0242f959d1dbcdb0"></a>[`public static bool Inno.Editor.Shaders.ShaderNodePresentation.operator ==(Inno.Editor.Shaders.ShaderNodePresentation left, Inno.Editor.Shaders.ShaderNodePresentation right)`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawer.cs#L73) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-fad0865a2bcccdcd"></a>[`public static bool Inno.Editor.Shaders.ShaderNodePresentation.operator !=(Inno.Editor.Shaders.ShaderNodePresentation left, Inno.Editor.Shaders.ShaderNodePresentation right)`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawer.cs#L73) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-5c8de9ebd5b2f978"></a>[`public override string Inno.Editor.Shaders.ShaderNodePresentation.ToString()`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawer.cs#L73) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-fef39284f090077c"></a>[`public int Inno.Editor.Shaders.ShaderNodePresentation.createOrder { get; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawer.cs#L115) | Gets the stable ordering value within the creation menu. | — | — |
| <a id="api-83b29d4a8ae2179b"></a>[`public string Inno.Editor.Shaders.ShaderNodePresentation.createPath { get; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawer.cs#L110) | Gets the optional slash-delimited creation-menu path. | — | — |
| <a id="api-0e7f1b8aa7567c33"></a>[`public string Inno.Editor.Shaders.ShaderNodePresentation.displayName { get; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawer.cs#L105) | Gets the optional artist-facing node name. | — | — |
| <a id="api-469fd34ae5e5dda0"></a>[`public bool Inno.Editor.Shaders.ShaderNodePresentation.separatorBefore { get; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawer.cs#L120) | Gets whether the contributed group starts a visual section. | — | — |
| <a id="api-2cf425914a8fc599"></a>[`public readonly record struct Inno.Editor.Shaders.ShaderNodePresentation : System.IEquatable<Inno.Editor.Shaders.ShaderNodePresentation>`](../../src/composition/editor/features/Inno.Editor.Shaders/Nodes/ShaderNodeDrawer.cs#L73) | Immutable authoring presentation contributed for one Shader node definition. | — | — |

### `Inno.Editor.Shaders.ShaderParameterPresentation`

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

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-5916bcf5fdb517f3"></a>[`public Inno.Editor.Shaders.ShaderParameterPresentation.ShaderParameterPresentation()`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/ShaderParameterPresentation.cs#L12) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-c7993f1d4d510246"></a>[`public static Inno.Editor.Shaders.ShaderParameterPresentation Inno.Editor.Shaders.ShaderParameterPresentation.Read(Inno.Core.Graphs.GraphDocument graph, Inno.Rendering.ShaderPropertyId propertyId, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/ShaderParameterPresentation.cs#L59) | Reads detached presentation for one binding, defaulting only when no presentation was authored.<br>返回：The authored presentation, or ordinary visible, unbounded defaults.<br>`graph`：Authoring graph, not a runtime Shader program.<br>`propertyId`：Stable parameter identity shared by all its nodes.<br>`serialization`：Current owner converters.<br>`context`：Complete owner serialization context. | — | — |
| <a id="api-c8029dc6f11147fd"></a>[`public static void Inno.Editor.Shaders.ShaderParameterPresentation.Write(Inno.Core.Graphs.GraphDocument graph, Inno.Rendering.ShaderPropertyId propertyId, Inno.Editor.Shaders.ShaderParameterPresentation presentation, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/ShaderParameterPresentation.cs#L92) | Writes presentation to a detached graph without changing any default, Material, or compiled program.<br>`graph`：Detached graph owned by the caller's History transaction.<br>`propertyId`：Stable parameter identity.<br>`presentation`：Detached Inspector settings.<br>`serialization`：Current owner converters.<br>`context`：Complete owner serialization context. | — | `System.ArgumentException`：The identity or range is invalid, or a bound cannot be represented by a scalar Shader Float. |
| <a id="api-fd0476de74326342"></a>[`public string Inno.Editor.Shaders.ShaderParameterPresentation.description { get; set; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/ShaderParameterPresentation.cs#L23) | Gets or sets the hover description without adding a permanent paragraph. | — | — |
| <a id="api-0d132986831bde97"></a>[`public string Inno.Editor.Shaders.ShaderParameterPresentation.group { get; set; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/ShaderParameterPresentation.cs#L19) | Gets or sets the Material Inspector section; empty uses the standard Parameters section. | — | — |
| <a id="api-eb971e371f6ecc11"></a>[`public bool Inno.Editor.Shaders.ShaderParameterPresentation.hasRange { get; set; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/ShaderParameterPresentation.cs#L31) | Gets or sets whether a scalar Float control uses the declared editing bounds. | — | — |
| <a id="api-76f06870eba00aa9"></a>[`public double Inno.Editor.Shaders.ShaderParameterPresentation.maximum { get; set; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/ShaderParameterPresentation.cs#L39) | Gets or sets the inclusive editing maximum. | — | — |
| <a id="api-f2b0e4d2b1208ef3"></a>[`public double Inno.Editor.Shaders.ShaderParameterPresentation.minimum { get; set; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/ShaderParameterPresentation.cs#L35) | Gets or sets the inclusive editing minimum; existing values are not changed by presentation metadata. | — | — |
| <a id="api-9cc39cd1353549be"></a>[`public bool Inno.Editor.Shaders.ShaderParameterPresentation.visible { get; set; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/ShaderParameterPresentation.cs#L27) | Gets or sets whether this Material-owned parameter appears in ordinary Material Inspectors. | — | — |
| <a id="api-e23a4c6f42c32517"></a>[`public sealed class Inno.Editor.Shaders.ShaderParameterPresentation : Inno.Core.Serialization.ISerializable`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/ShaderParameterPresentation.cs#L12) | Stores Editor-only parameter presentation by stable binding identity in the Shader's authoring graph. | — | — |

### `Inno.Editor.Shaders.ShaderPreviewContext`

<!-- api: T:Inno.Editor.Shaders.ShaderPreviewContext 24fcfecf325a9d26687613f89fc534353d725b927f6f174481edcfbfc286460f -->
<!-- api: M:Inno.Editor.Shaders.ShaderPreviewContext.#ctor(Inno.Rendering.Runtime.RenderPersistentResourceId,Inno.Rendering.Assets.MaterialAsset,Inno.Rendering.Assets.RenderShaderArtifact,Inno.Rendering.Assets.ShaderDefinition,Inno.Core.Diagnostics.IDiagnosticReporter,System.Int32,System.Int32) 5283b0ab256415081f4e3da9cec378594d15f5d2351a49de03b7884acf56baff -->
<!-- api: P:Inno.Editor.Shaders.ShaderPreviewContext.resourceId 492cf38f165d7d65343774d1a4d6fc53fe89d863d5768bf43042d6510b8ba69c -->
<!-- api: P:Inno.Editor.Shaders.ShaderPreviewContext.material 665845cc4fa42fa6764f578c58a40b7415d9321e359efd618285780c4bb8e835 -->
<!-- api: P:Inno.Editor.Shaders.ShaderPreviewContext.artifact 72617ceb9f7dc6498c5fd325c242e8b7aac7d0afc9e366a863fd8ac03678719c -->
<!-- api: P:Inno.Editor.Shaders.ShaderPreviewContext.definition 48eba862c7c6011abe1d5a5a07b185e22f4be4fe75086a01843e12c54dcad3ec -->
<!-- api: P:Inno.Editor.Shaders.ShaderPreviewContext.diagnostics 1bc0795fa425f7e2e5ca5562d41450504000accd15dc150cb4e693a2c1df3393 -->
<!-- api: P:Inno.Editor.Shaders.ShaderPreviewContext.pixelWidth ccfaa88ebaea8b63b9e28b9564712b25628b1e7110b6387cb79eaf22c3885235 -->
<!-- api: P:Inno.Editor.Shaders.ShaderPreviewContext.pixelHeight a6d1c2947b6045d0447c60a5d3c9114f05e054933e9f19899cd7cade972d04a1 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-ee053b07553c0c68"></a>[`public Inno.Editor.Shaders.ShaderPreviewContext.ShaderPreviewContext(Inno.Rendering.Runtime.RenderPersistentResourceId resourceId, Inno.Rendering.Assets.MaterialAsset material, Inno.Rendering.Assets.RenderShaderArtifact artifact, Inno.Rendering.Assets.ShaderDefinition definition, Inno.Core.Diagnostics.IDiagnosticReporter diagnostics, int pixelWidth, int pixelHeight)`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviewProvider.cs#L72) | Creates a preview invocation from host-owned detached inputs.<br>`resourceId`：Isolated material publication and viewport resource identity.<br>`material`：Detached values with a current Shader reference; do not modify the reference.<br>`artifact`：Compiled candidate, never published as a canonical Shader.<br>`definition`：Detached contract captured by the candidate.<br>`diagnostics`：Preview-local reporter; errors must not contaminate project diagnostics.<br>`pixelWidth`：Positive target width.<br>`pixelHeight`：Positive target height. | — | — |
| <a id="api-ed75c7021f5ffc78"></a>[`public Inno.Rendering.Assets.RenderShaderArtifact Inno.Editor.Shaders.ShaderPreviewContext.artifact { get; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviewProvider.cs#L100) | Gets the complete immutable GPU candidate. | — | — |
| <a id="api-123745de6ec130ed"></a>[`public Inno.Rendering.Assets.ShaderDefinition Inno.Editor.Shaders.ShaderPreviewContext.definition { get; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviewProvider.cs#L104) | Gets the candidate's exact contract, not the latest uncompiled source interface. | — | — |
| <a id="api-4bc576943224c4c9"></a>[`public Inno.Core.Diagnostics.IDiagnosticReporter Inno.Editor.Shaders.ShaderPreviewContext.diagnostics { get; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviewProvider.cs#L108) | Gets the host-owned, isolated diagnostic producer. | — | — |
| <a id="api-f0eb503b1dbd15e9"></a>[`public Inno.Rendering.Assets.MaterialAsset Inno.Editor.Shaders.ShaderPreviewContext.material { get; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviewProvider.cs#L96) | Gets the invocation-local Material values. | — | — |
| <a id="api-a33bab3b2d2aedf8"></a>[`public int Inno.Editor.Shaders.ShaderPreviewContext.pixelHeight { get; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviewProvider.cs#L116) | Gets target height in physical pixels. | — | — |
| <a id="api-5b0ac395c4ba3bb7"></a>[`public int Inno.Editor.Shaders.ShaderPreviewContext.pixelWidth { get; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviewProvider.cs#L112) | Gets target width in physical pixels. | — | — |
| <a id="api-db863f0e472dc24a"></a>[`public Inno.Rendering.Runtime.RenderPersistentResourceId Inno.Editor.Shaders.ShaderPreviewContext.resourceId { get; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviewProvider.cs#L92) | Gets the publication scope released by the host when this preview closes. | — | — |
| <a id="api-cec268921d9dc5c8"></a>[`public sealed class Inno.Editor.Shaders.ShaderPreviewContext`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviewProvider.cs#L46) | Carries frame-only preview inputs; persistent provider state must retain stable values rather than this object. | — | — |

### `Inno.Editor.Shaders.ShaderPreviewProvider`

<!-- api: T:Inno.Editor.Shaders.ShaderPreviewProvider 8b7304ab6221436f7731d1bad238b71ab3616192d3b3f102981714db0d49c309 -->
<!-- api: M:Inno.Editor.Shaders.ShaderPreviewProvider.CreateLayer(Inno.Editor.Shaders.ShaderPreviewContext) 67ba65bd49cb7ef9617f19475108be1248d36e4436e3951feb4341f99eae5376 -->
<!-- api: M:Inno.Editor.Shaders.ShaderPreviewProvider.#ctor 78cfacb38ebdbbea60efc9535e2219856e52c7230050928843052b560e24417a -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8857e0785476e17e"></a>[`protected Inno.Editor.Shaders.ShaderPreviewProvider.ShaderPreviewProvider()`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviewProvider.cs#L29) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-459cb3d4f863a3c5"></a>[`public abstract Inno.Editor.Rendering.EditorViewportLayer Inno.Editor.Shaders.ShaderPreviewProvider.CreateLayer(Inno.Editor.Shaders.ShaderPreviewContext context)`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviewProvider.cs#L40) | Creates one frame-local rendering layer through the ordinary Render Graph.<br>返回：A domain-selected pipeline and frame payload; no instance may be retained beyond the submitted frame.<br>`context`：Detached Material, immutable candidate and isolated diagnostics for this invocation. | — | — |
| <a id="api-155f73b4d6305de8"></a>[`public abstract class Inno.Editor.Shaders.ShaderPreviewProvider`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviewProvider.cs#L29) | Builds domain-owned preview geometry and pass inputs without changing a scene or canonical asset. | — | — |

### `Inno.Editor.Shaders.ShaderPreviewProviderAttribute`

<!-- api: T:Inno.Editor.Shaders.ShaderPreviewProviderAttribute 8106a2ae83678cea9fcf60a3bb7a02e0c26ad5fdcfcf0dbc658d937df8df0fb7 -->
<!-- api: M:Inno.Editor.Shaders.ShaderPreviewProviderAttribute.#ctor(System.String) ad3da8bc12d35ca8c80bc2ac0c7c4199e662e495fa7e1cf654cf13c549fc676a -->
<!-- api: P:Inno.Editor.Shaders.ShaderPreviewProviderAttribute.contractId 2328ae5fedf74d182f11a281cad8755f34fab7946806ab1cd9dba428b5ce082d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-38ee86e254ac59f3"></a>[`public Inno.Editor.Shaders.ShaderPreviewProviderAttribute.ShaderPreviewProviderAttribute(string contractId)`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviewProvider.cs#L17) | Associates a reloadable preview implementation with one open Shader contract.<br>`contractId`：Non-empty domain-owned contract identity. | — | — |
| <a id="api-1a29a7c07f6c72ed"></a>[`public string Inno.Editor.Shaders.ShaderPreviewProviderAttribute.contractId { get; }`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviewProvider.cs#L22) | Gets the non-empty Shader contract consumed by this preview. | — | — |
| <a id="api-e11c53e0bc4d5178"></a>[`public sealed class Inno.Editor.Shaders.ShaderPreviewProviderAttribute : System.Attribute`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviewProvider.cs#L17) | Associates a reloadable preview implementation with one open Shader contract.<br>`contractId`：Non-empty domain-owned contract identity. | — | — |

### `Inno.Editor.Shaders.ShaderPreviews`

<!-- api: T:Inno.Editor.Shaders.ShaderPreviews 0c34db9756bd9e5acec143bab49074d1ccbf6f9f836b404458e45f9ae09b5871 -->
<!-- api: M:Inno.Editor.Shaders.ShaderPreviews.DrawMaterial(System.Guid,Inno.Rendering.Assets.MaterialAsset,System.Single) e3b51c66416a2aa439a4227bb1643fc0c561585572db289f0c748f1f695110b3 -->
<!-- api: M:Inno.Editor.Shaders.ShaderPreviews.Draw(System.Guid,Inno.Rendering.Assets.MaterialAsset,Inno.Editor.Rendering.EditorShaderDraftCompilationSnapshot,System.Single) b8041e269bb0359e67834dcb788771bf5a3b20319dd26b551ec3211f36f69fef -->
<!-- api: M:Inno.Editor.Shaders.ShaderPreviews.Release(System.Guid) dc9240e636bbb8172c79f17dfb7ac722fd7b445af352879a44b103abffa3a731 -->
<!-- api: M:Inno.Editor.Shaders.ShaderPreviews.OnUpdate(Inno.Editor.Core.EditorContext) 929d77ef0c7fba118861f08f73ee3c6bf5070186b684d7c73d5e9a3aa1e056ca -->
<!-- api: M:Inno.Editor.Shaders.ShaderPreviews.OnStop(Inno.Editor.Core.EditorContext) 9fb71670df5309b6ae1e0a8dd3134e6cf1db29e77881086288e8a814eecb1ce0 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3efc9b8862d89ece"></a>[`public void Inno.Editor.Shaders.ShaderPreviews.Draw(System.Guid ownerId, Inno.Rendering.Assets.MaterialAsset material, Inno.Editor.Rendering.EditorShaderDraftCompilationSnapshot compilation, float logicalSize)`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviews.cs#L88) | Draws a compiled draft without modifying the source Shader or its canonical runtime publication.<br>`ownerId`：Persistent document identity, independent of the canonical Shader publication.<br>`material`：Detached defaults or overrides with the current Shader reference.<br>`compilation`：Isolated draft compiler result, including explicit last-good state.<br>`logicalSize`：Positive square image size in logical pixels. | — | — |
| <a id="api-63e937ff49f4e8ab"></a>[`public void Inno.Editor.Shaders.ShaderPreviews.DrawMaterial(System.Guid ownerId, Inno.Rendering.Assets.MaterialAsset material, float logicalSize)`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviews.cs#L53) | Draws detached Material overrides using its Shader's compiled contract.<br>`ownerId`：Persistent document identity; no Material is retained under this identity.<br>`material`：Invocation-local Material values.<br>`logicalSize`：Positive square image size in logical pixels. | — | — |
| <a id="api-78d636a504dcaf53"></a>[`protected override void Inno.Editor.Shaders.ShaderPreviews.OnStop(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviews.cs#L170) | Stops this feature before its owning runtime releases the active generation.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-d60b0e0be1d8cc65"></a>[`protected override void Inno.Editor.Shaders.ShaderPreviews.OnUpdate(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviews.cs#L158) | Advances this feature using the current runtime state.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-c5f9b1bf437ad3df"></a>[`public void Inno.Editor.Shaders.ShaderPreviews.Release(System.Guid ownerId)`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviews.cs#L146) | Releases this document's viewport and scoped GPU publication without touching other consumers.<br>`ownerId`：Closing or disabled preview owner. | — | — |
| <a id="api-024f36ef22a8a06d"></a>[`public sealed class Inno.Editor.Shaders.ShaderPreviews : Inno.Editor.Core.EditorModule`](../../src/composition/editor/features/Inno.Editor.Shaders/Previews/ShaderPreviews.cs#L21) | Hosts isolated Shader and Material images using domain-contributed preview pipelines. | — | — |

### `Inno.Editor.Shaders.ShaderPropertyInspector`

<!-- api: T:Inno.Editor.Shaders.ShaderPropertyInspector 8d29d109f9502c66caf9ecf2fc5ab110f2339027568a3b8916d8932b04cfbaa4 -->
<!-- api: M:Inno.Editor.Shaders.ShaderPropertyInspector.Draw(Inno.Editor.Inspection.InspectionDrawContext,System.Object,System.String,Inno.Rendering.Assets.ShaderPropertyDefinition,Inno.Rendering.Assets.MaterialValue,System.Action{Inno.Rendering.Assets.MaterialValue},Inno.Editor.Inspection.IInspectionPropertyEditService,System.Boolean,Inno.Editor.Shaders.ShaderParameterPresentation) a86dc75b5e9844bbdada7701a94b8fefa0d5c8b8495f230cad9ab72a9047caf1 -->
<!-- api: M:Inno.Editor.Shaders.ShaderPropertyInspector.Compatible(Inno.Rendering.ShaderPropertyType,Inno.Rendering.Assets.MaterialValueKind) 5c78a8b788c7b8e5b774a7943492e99c5e99559807fab2a7ee7187c83be57db7 -->
<!-- api: M:Inno.Editor.Shaders.ShaderPropertyInspector.ApplyEdit(Inno.Rendering.ShaderPropertyType,Inno.Rendering.Assets.MaterialValue,Inno.Rendering.Assets.MaterialValue,Inno.Rendering.Assets.MaterialValue) c8fe0e033548aca8262ef8bc7e722677e041fff4b5ab138e9396f832a41ec527 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-89a5cb34612395af"></a>[`public static Inno.Rendering.Assets.MaterialValue Inno.Editor.Shaders.ShaderPropertyInspector.ApplyEdit(Inno.Rendering.ShaderPropertyType type, Inno.Rendering.Assets.MaterialValue before, Inno.Rendering.Assets.MaterialValue edited, Inno.Rendering.Assets.MaterialValue target)`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/ShaderPropertyInspector.cs#L111) | Applies the edited components of a representative value without replacing unrelated mixed components.<br>返回：A detached value preserving the target's untouched channels, texture or sampler.<br>`type`：Common exact Shader property type.<br>`before`：Representative value before the control gesture sample.<br>`edited`：Representative value after this sample.<br>`target`：Another selected Material's current effective value. | — | — |
| <a id="api-52651237293a1aae"></a>[`public static bool Inno.Editor.Shaders.ShaderPropertyInspector.Compatible(Inno.Rendering.ShaderPropertyType type, Inno.Rendering.Assets.MaterialValueKind kind)`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/ShaderPropertyInspector.cs#L80) | Tests whether a stored override has the exact representation expected by a declaration.<br>返回：True only for supported matching kinds; no lossy conversion is performed.<br>`type`：Declared material type.<br>`kind`：Stored representation. | — | — |
| <a id="api-d3258b8ecec87002"></a>[`public static void Inno.Editor.Shaders.ShaderPropertyInspector.Draw(Inno.Editor.Inspection.InspectionDrawContext context, object owner, string path, Inno.Rendering.Assets.ShaderPropertyDefinition property, Inno.Rendering.Assets.MaterialValue value, System.Action<Inno.Rendering.Assets.MaterialValue> setter, Inno.Editor.Inspection.IInspectionPropertyEditService edits, bool readOnly, Inno.Editor.Shaders.ShaderParameterPresentation? presentation = null)`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/ShaderPropertyInspector.cs#L44) | Draws a typed value without choosing its persistence or history policy.<br>`context`：Current Inspector invocation.<br>`owner`：Detached draft owner used for widget identity.<br>`path`：Stable draft property path.<br>`property`：Current Shader declaration.<br>`value`：Current default or override, never mutated.<br>`setter`：Writes only the host's detached draft.<br>`edits`：Host-owned gesture and history service.<br>`readOnly`：Whether this source is editable.<br>`presentation`：Optional Editor-only description and scalar bounds; it never changes inherited or stored values. | — | — |
| <a id="api-ea8bb396c7ab0ca0"></a>[`public static class Inno.Editor.Shaders.ShaderPropertyInspector`](../../src/composition/editor/features/Inno.Editor.Shaders/Materials/ShaderPropertyInspector.cs#L12) | Uses the shared Inspector drawers for both Shader defaults and Material overrides. | — | — |

## 项目依赖

- [Inno.Editor.Assets](Inno.Editor.Assets.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.IO](../core/Inno.Core.IO.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Editor.ImGui](Inno.Editor.ImGui.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Native.ImGui](../backends/ImGui/Inno.Native.ImGui.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Presentation.ImGui.Sdl3](../backends/ImGui/Inno.Adapter.Presentation.ImGui.Sdl3.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Assets.Pipeline](../assets/Inno.Assets.Pipeline.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Editor.Interactions](Inno.Editor.Interactions.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Rendering.Shaders](../rendering/Inno.Rendering.Shaders.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Rendering.Assets.Authoring](../rendering/Inno.Rendering.Assets.Authoring.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Rendering.Assets](../rendering/Inno.Rendering.Assets.md)：公开引用边界由实际签名核对。
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：公开引用边界由实际签名核对。
- [Inno.Editor.Inspection](Inno.Editor.Inspection.md)：公开引用边界由实际签名核对。
- [Inno.Assets](../assets/Inno.Assets.md)：公开引用边界由实际签名核对。
- [Inno.Editor.Core](Inno.Editor.Core.md)：公开引用边界由实际签名核对。
- [Inno.Rendering](../rendering/Inno.Rendering.md)：公开引用边界由实际签名核对。
- [Inno.Core.Graphs](../core/Inno.Core.Graphs.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
- [Inno.Editor.Rendering](Inno.Editor.Rendering.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
- [Inno.Rendering.Runtime](../rendering/Inno.Rendering.Runtime.md)：公开引用边界由实际签名核对。
