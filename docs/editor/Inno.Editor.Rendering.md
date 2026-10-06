# Inno.Editor.Rendering

## 离屏预览与编译候选

`IEditorPreviewService.TryRender(EditorViewportComposition, out EditorPreviewHandle)` 将模型贡献提交到现有 `IEditorRenderingHost` 离屏链；只返回当前设备 generation 的可绘制图像。首次分配或 resize 等待完整新目标，不暴露失效原生句柄。
`ReleaseRendered(viewportId)` 可以释放尚未产生首张图像的预览，同时退休同名 `RenderPersistentResourceId` 范围的 Shader 程序。`Release(handle)` 和 `ReleaseAll()` 同样覆盖生成预览。
纹理预览每次核对当前 resident texture、generation 与尺寸；依赖重导入之后重新注册图像，不永久显示旧贴图。

`EditorShaderCompilation.RequestArtifact(shader, variant)` 为 Material 预览读取当前正式 Shader 的不可变候选和编译状态，不改变 Material 值。
`ReadDefinition(artifact)` 使用 owner 引用上下文读取该产物携带的精确接口。Shader 草稿仍通过 `RequestDraft` 独立缓存编译；预览错误和正式保存状态互不冒充。
交互式 `Check` 在工具链编译完成后还会通过 `IEditorShaderArtifactValidator` 把不可变候选排入下一次渲染帧安全点；Host 使用正式 `IRenderResourceService.ValidateShaderArtifact` 为每个 Pass 创建并立即退休真实 Pipeline。只有二进制加载、Program link 和 Reflection 合同全部成功，当前 revision 才进入可预览状态。设备校验失败只产生本次 Check 诊断，不发布候选，也不回退显示旧预览。

[Editor 索引](README.md) · [Rendering](../rendering/README.md) · [Scene View](Inno.Editor.Panel.SceneView.md) · [Game View](Inno.Editor.Panel.GameView.md)

`Inno.Editor.Rendering` 是 Editor viewport 与任意 Plugin 渲染模型之间的后端中立合成边界。Viewport kind 只表示“Scene View”“Game View”或自定义预览等用途，不再等同于某一种 2D/3D 渲染器。Editor 不知道 Camera、Scene snapshot、Picking buffer、Render Path 或材质世界观。

## Pipeline Inspector 与源草稿

`PipelineDocuments` 是内置 Editor Module，使用 [通用资产草稿](Inno.Editor.Assets.md) 编辑 `.irenderpipeline`，不保存第二份配置。
选择原生 Pipeline 或导入失败的源均进入统一 Inspector。强类型设置按当前 stableTypeId 恢复，并使用实际 AssetPipeline owner 捕获引用与依赖。
字段复用共享属性 Drawer 和 Header/Tooltip，不添加 2D 分支。缺失设置保留中立 bytes、依赖与 Feature 顺序。

公开 API（同时导出到 `InnoEditor.Rendering`）：

- `Open(path)` / `Read(id)`：打开源并解码独立 Pipeline 草稿。
- `Replace(id, candidate, finishGesture)` / `Commit(id)`：一手势一次共享 History，不保存、不改变 Scene/Game。
- `ReplaceSettings<TSettings>(id, settings, finishGesture)`：自动捕获设置的完整依赖，写入草稿 pipelineState。

Save/Revert 使用共享文档服务，保存与导入/激活状态分开呈现；源冲突拒绝覆盖。
当前 Inspector 支持既有设置类型的字段、Feature 开关和顺序。Pipeline 类型及新增 Feature 由配置创建 API/插件模板提供。
范例：`var id = documents.Open(path); documents.ReplaceSettings(id, settings);`；显式 Save 是另一个文档操作。

## Contributor 与 Composition 协议

`EditorRenderingModule` 从完成帧统计发布 Stats 数据，不直接依赖具体后端。除 View/Draw/Dispatch 外，
还展示 Allocation Generation 与 transient Texture/Buffer/Framebuffer 的设备生命周期累计分配数；
后端不提供分配诊断时显示 `Unavailable`，不显示误导性的零。

| API | 说明 |
| --- | --- |
| `EditorViewportKindId` | 开放 viewport 用途，例如 `inno.editor.viewport.scene`。 |
| `EditorViewportContributorExtensionAttribute` | 声明 Stable ID、kind、合成顺序与交互控制优先级的热重载入口。 |
| `EditorViewportContributor` | 一个渲染模型对 viewport 的可选贡献；负责 participation、frame data、Pipeline，以及可选导航/工具/pointer。 |
| `EditorViewportContribution` | 单个模型提供的 frame data、可选 Pipeline、共享目标格式与 manipulation space。 |
| `EditorViewportLayer` | Host 接受后的中立模型层；只含 Stable ID、Pipeline、frame data 与 order。 |
| `EditorViewportComposition` | 一个 viewport 的非空模型输出；多模型时先各自绘制，再按 route 合成。 |
| `EditorViewportContext` | 当前 Editor、交互服务、viewport ID、物理尺寸、导航状态、显式内容作用域与呈现偏好。 |
| `EditorViewportNavigationState` | Host 持有的 position/rotation、正交/透视参数、pivot、focus distance、移动速度与 Planar/Orbit/Fly 模式。 |
| `EditorViewportNavigationProfile` | 当前交互控制者声明的 Pan、Zoom、Orbit、Fly、Frame Selection 能力与边界。 |
| `RenderContentScope` | Host 显式选择的有序、frame-scoped 内容集合；Contributor 不扫描全局 Loaded Scene。 |
| `EditorViewportPresentation` | Host 提供的呈现偏好；包含线性背景色及物理渲染像素与逻辑显示单位的比例 `pixelDensity`（默认 `1`）。 |
| `EditorViewportManipulationPlane` | 中立的操作维度：`Spatial` 默认完整空间，`XY`、`XZ`、`YZ` 声明由模型选择的平面；不包含 ImGuizmo 类型。 |
| `EditorViewportManipulationSpace` | 控制者可选提供的本帧精确 view/projection、是否正交及独立的操作平面；正交投影不会自动切换平面手柄。 |
| `EditorViewportOutput` | Host 拥有的 opaque `PresentationTextureHandle` 输出；`isReady` 只有在当前目标修订的 Graph attachment 写入命令成功录制后才为 true，首次分配/resize/Shader 仍准备中时只绘制 Preparing 占位，不采样未初始化的 Vulkan RT。 |
| `EditorRenderingModule` | Contributor generation、参与判断、控制者选择、Composition、Submit/Draw/Release 与逐 Contributor 异常隔离。 |

同一种 kind 可以注册多个 Contributor。`EditorRenderingModule` 每帧询问全部候选的 `CanContribute`；只有一个适用模型时直接提交。多个模型同时适用时必须为该 viewport 配置 `RenderOutputRoute`，明确层顺序及专属世界内容源。Host 为每个模型建立独立目标，并校验共同的目标格式后按预乘 Alpha 合成；未配置 route 或任一模型失败时拒绝整个输出并显示诊断。跨模型不共享几何深度。

显式 `RenderRequest` 写入同一目标重叠区域时会收到 `RenderPipelineContext.preservePresentationTarget=true`。该 Pipeline 必须 Load/Preserve 已有颜色，不能再次清屏。多 Contributor 使用独立模型目标与图层合成。

渲染顺序和交互所有权是两个正交维度。`controllerPriority` 只选择一个 Contributor 负责导航、Toolbar、Pointer 与 Gizmo manipulation space；它不提供多模型合成。控制者可以将 `EditorViewportManipulationSpace.plane` 声明为 `XY` 等平面：Scene View 只依据这个中立几何契约选择轴和中心拖动，无需引用任何 2D Plugin 类型；未声明时保持完整的 3D 手柄。正交投影和 Planar 导航本身都不代表操作维度。错误、导航和 target 状态均按稳定 `viewportId` 隔离，多开同 kind viewport 不会串状态。

Plugin 示例：

```csharp
using Inno.Editor.Rendering;
using Inno.Rendering;

[EditorViewportContributorExtension(
    "sample.scene-model",
    "inno.editor.viewport.scene",
    order: 0,
    controllerPriority: 100)]
public sealed class SampleSceneContributor : EditorViewportContributor
{
    public override bool CanContribute(EditorViewportContext context)
        => context.content.contents.Count > 0;

    public override EditorViewportContribution Build(EditorViewportContext context)
    {
        var data = new RenderFrameData();
        data.Set(new RenderDataChannelId("sample.scene"), BuildFrame(context));
        return new EditorViewportContribution(data, LoadPipeline());
    }
}
```

导航状态由 Host 按稳定 viewport ID 保存，因此 Plugin reload 不会重置视图，也不会让 Host 长期持有 Plugin Camera 类型。每帧顺序固定为：Host 设置 content/presentation → 全部 Contributor 判断参与 → 选择控制者并配置导航 → Panel 处理输入 → 全部参与者 Build → 冻结 Composition → 提交有序 RenderRequest。滚轮、Orbit、Fly 和 Frame Selection 的结果会在同一帧进入 snapshot。

`RenderContentScope` 是类型擦除但带 Stable ID 的当前帧边界。Contributor 可用 `GetValues<T>()` 取得它理解的 Scene/Document 类型；`RenderContentReference.value` 不得跨帧或跨 generation 保存。每个渲染模型自行判断哪些 Scene 选择了该模型。因此同一个 scope 可以同时包含纯 3D Scene、纯 2D Scene，以及同时挂载两种 extraction system 的混合 Scene；缺少某个模型的 system 只表示该模型跳过此 Scene，不会令整个 viewport 失败。

`manipulationSpace` 完全可选，不向 Host 引入具体 Camera、渲染模型或 ImGuizmo 类型；几何约束通过中立 `EditorViewportManipulationPlane` 表示。只有控制者提供的 manipulation space 会被接受；矩阵必须来自该控制者同一帧提交的 snapshot，避免画面、Picking 与 Gizmo 使用不同相机状态。一次连续拖拽只在释放时通过 `SceneEdits` 的最小 Transform payload 组成一个 History transaction，不捕获 Plugin delegate 或 runtime `Type`。

`[EditorGizmoProviderExtension(id)]` 注册代际化的 `EditorGizmoProvider.Collect(EditorGizmoContext, IEditorGizmoSink)`。`EditorGizmoContext` 提供当前内容作用域、选中对象的 runtime identity 与视口物理尺寸；`IEditorGizmoSink.Icon(owner, position, iconId)` 接受开放的语义图标 ID，内置 `camera`、`light`、`canvas`，未知 ID 显示统一问号图标；`Line(start, end)` 提交选中范围。`EditorGizmoFrame.icons`/`lines` 分别是 `EditorGizmoIcon`/`EditorGizmoLine` 的当前帧列表，不持久化对象引用。插件不传颜色或 ImGui 字体代码点，Scene View 统一绘制大号白色图标与白色轮廓线，重叠图标仍位于各自世界位置并在点击时提供目标选择。调用示例：`sink.Icon(owner.identity, owner.transform.worldPosition, "camera"); sink.Line(start, end);`。

Host 创建或 resize `RenderTexture`，首个目标写入成功录制后再将 GPU texture 注册为 `PresentationTextureHandle`。不存在 CPU readback，也不向 Panel 暴露 BGFX handle。单个 Contributor 的 participation、导航、Build、Toolbar 或 Pointer 异常只隔离该 Contributor；没有任何适用 Contributor 时显示居中的不可用状态，Editor 其他功能继续运行。

## Editor 目标产物编译

`EditorRenderTargetArtifactProvider` 是 authoring 边界：它根据 Asset `contentVersion` 异步编译 Shader 与 Texture，并只向 Rendering Runtime 暴露无源码目标产物。首次请求会返回 `RenderTargetArtifactStatus.Pending`；这表示工作已排队，不是产物丢失，因此启动 Editor 时不会产生 `RENDER_SHADER_TARGET_UNAVAILABLE`。只有不可变部署中确实没有文件时才返回 `Unavailable`，编译器明确失败时则返回 `Failed` 并发布精确工具链诊断。

每个缓存项保留 last-good artifact。新候选编译期间继续返回 `Ready` 和 last-good；候选失败时也不破坏已工作的 GPU 资源。诊断按完整 code/source/message/severity 去重，并以 code/source 作为可恢复状态范围进入 `DiagnosticHub`；同一文件的多条编译诊断不会互相覆盖，成功重编译、资源恢复或 Contributor Dispose 时会显式解析并清除。Console 因而显示 `Diagnostic` 的真实 Asset 位置，不再显示没有排障价值的日志调用栈。

## 当前源码公开 API 清单

以下仅列出当前程序集自己声明的 public/protected 契约；继承成员遵循所属基类页面。internal/private 实现不作为稳定公开 API。签名依据当前源码语义模型生成，行为、参数、异常与所有权说明同时以对应英文 XML 为准。

### `Inno.Editor.Rendering.EditorGizmoContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Rendering.EditorGizmoContext.EditorGizmoContext(Inno.References.ContentReadScope content, Inno.Core.Identity.RuntimeIdentity? selected, int pixelWidth, int pixelHeight)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorGizmoProvider.cs#L29) | Creates a frame-scoped gizmo request. |
| [`Inno.References.ContentReadScope Inno.Editor.Rendering.EditorGizmoContext.content`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorGizmoProvider.cs#L44) | Gets the scene roots visible in this Editor viewport. |
| [`int Inno.Editor.Rendering.EditorGizmoContext.pixelHeight`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorGizmoProvider.cs#L56) | Gets the viewport height in physical pixels. |
| [`int Inno.Editor.Rendering.EditorGizmoContext.pixelWidth`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorGizmoProvider.cs#L52) | Gets the viewport width in physical pixels. |
| [`Inno.Core.Identity.RuntimeIdentity? Inno.Editor.Rendering.EditorGizmoContext.selected`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorGizmoProvider.cs#L48) | Gets the transient selected identity. |
| [`Inno.Editor.Rendering.EditorGizmoContext`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorGizmoProvider.cs#L12) | Provides scene content and selection for transient Editor gizmos. |

### `Inno.Editor.Rendering.EditorGizmoFrame`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Editor.Rendering.EditorGizmoFrame.Icon(Inno.Core.Identity.Identity owner, Inno.Core.Mathematics.Vector3 position, string iconId)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorGizmoProvider.cs#L202) | Appends a selectable scene icon to the current gizmo frame. |
| [`void Inno.Editor.Rendering.EditorGizmoFrame.Line(Inno.Core.Mathematics.Vector3 start, Inno.Core.Mathematics.Vector3 end)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorGizmoProvider.cs#L220) | Appends a world space line to the current gizmo frame. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Editor.Rendering.EditorGizmoIcon> Inno.Editor.Rendering.EditorGizmoFrame.icons`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorGizmoProvider.cs#L184) | Gets the ordered selectable icons. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Editor.Rendering.EditorGizmoLine> Inno.Editor.Rendering.EditorGizmoFrame.lines`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorGizmoProvider.cs#L188) | Gets the ordered non-interactive lines. |
| [`Inno.Editor.Rendering.EditorGizmoFrame`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorGizmoProvider.cs#L176) | Holds the transient gizmo primitives for one Scene viewport frame. |

### `Inno.Editor.Rendering.EditorGizmoIcon`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Rendering.EditorGizmoIcon`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorGizmoProvider.cs#L153) | One selectable Editor icon. |

### `Inno.Editor.Rendering.EditorGizmoLine`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Rendering.EditorGizmoLine`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorGizmoProvider.cs#L168) | One non-interactive Editor line. |

### `Inno.Editor.Rendering.EditorGizmoProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`abstract void Inno.Editor.Rendering.EditorGizmoProvider.Collect(Inno.Editor.Rendering.EditorGizmoContext context, Inno.Editor.Rendering.IEditorGizmoSink sink)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorGizmoProvider.cs#L111) | Submits transient icons and lines for the presented scenes. |
| [`Inno.Editor.Rendering.EditorGizmoProvider`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorGizmoProvider.cs#L100) | Contributes Editor-only icons and bounds without modifying a rendering model. |

### `Inno.Editor.Rendering.EditorGizmoProviderExtensionAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Rendering.EditorGizmoProviderExtensionAttribute.EditorGizmoProviderExtensionAttribute(string id)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorGizmoProvider.cs#L129) | Creates a provider declaration with a stable identifier. |
| [`string Inno.Editor.Rendering.EditorGizmoProviderExtensionAttribute.id`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorGizmoProvider.cs#L138) | Gets the stable provider identifier. |
| [`Inno.Editor.Rendering.EditorGizmoProviderExtensionAttribute`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorGizmoProvider.cs#L120) | Marks one reloadable Editor gizmo provider. |

### `Inno.Editor.Rendering.EditorPreviewHandle`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Rendering.EditorPreviewHandle.EditorPreviewHandle(ulong value, uint deviceGeneration, int pixelWidth, int pixelHeight)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorPreviewService.cs#L28) | Creates one generation-scoped preview handle. |
| [`uint Inno.Editor.Rendering.EditorPreviewHandle.deviceGeneration`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorPreviewService.cs#L52) | Gets the rendering-device generation that owns this handle. |
| [`bool Inno.Editor.Rendering.EditorPreviewHandle.isValid`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorPreviewService.cs#L67) | Gets whether this handle identifies a usable generation-scoped preview. |
| [`int Inno.Editor.Rendering.EditorPreviewHandle.pixelHeight`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorPreviewService.cs#L62) | Gets source height in pixels. |
| [`int Inno.Editor.Rendering.EditorPreviewHandle.pixelWidth`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorPreviewService.cs#L57) | Gets source width in pixels. |
| [`ulong Inno.Editor.Rendering.EditorPreviewHandle.value`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorPreviewService.cs#L47) | Gets the opaque preview identity. |
| [`Inno.Editor.Rendering.EditorPreviewHandle`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorPreviewService.cs#L11) | Identifies one preview texture owned by a specific rendering-device generation. |

### `Inno.Editor.Rendering.EditorRenderTargetArtifactProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Rendering.EditorRenderTargetArtifactProvider.EditorRenderTargetArtifactProvider(Inno.Assets.Pipeline.AssetPipeline assets, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Extensibility.Types.TypeCatalog types, Inno.Rendering.Assets.Authoring.ShaderCompiler shaderCompiler, Inno.Rendering.Assets.Authoring.ITextureTargetCompiler textureCompiler, Inno.Core.Diagnostics.IDiagnosticReporter diagnostics)`](../../src/composition/editor/features/Inno.Editor.Rendering/Compilation/EditorRenderTargetArtifactProvider.cs#L63) | Creates an Editor artifact provider backed by explicit shader and texture toolchains. |
| [`void Inno.Editor.Rendering.EditorRenderTargetArtifactProvider.Dispose()`](../../src/composition/editor/features/Inno.Editor.Rendering/Compilation/EditorRenderTargetArtifactProvider.cs#L240) | Cancels pending toolchain work and releases every cached authoring artifact. |
| [`Inno.Rendering.Runtime.RenderTargetArtifactStatus Inno.Editor.Rendering.EditorRenderTargetArtifactProvider.GetShaderArtifact(Inno.Rendering.Assets.ShaderAsset shader, Inno.Rendering.Assets.RenderShaderVariant variant, Inno.Rendering.GraphicsCapabilities capabilities, out Inno.Rendering.Assets.RenderShaderArtifact? artifact)`](../../src/composition/editor/features/Inno.Editor.Rendering/Compilation/EditorRenderTargetArtifactProvider.cs#L104) | Returns a matching compiled shader when available and schedules a new candidate when source state changed. |
| [`Inno.Rendering.Runtime.RenderTargetArtifactStatus Inno.Editor.Rendering.EditorRenderTargetArtifactProvider.GetTextureArtifact(Inno.Rendering.Assets.RenderTextureArtifactReference texture, out System.ReadOnlyMemory<byte> artifact)`](../../src/composition/editor/features/Inno.Editor.Rendering/Compilation/EditorRenderTargetArtifactProvider.cs#L212) | Returns a matching compiled texture when available and schedules a replacement when source state changed. |
| [`Inno.Rendering.Assets.ShaderDefinition Inno.Editor.Rendering.EditorRenderTargetArtifactProvider.ReadShaderDefinition(Inno.Rendering.Assets.RenderShaderArtifact artifact)`](../../src/composition/editor/features/Inno.Editor.Rendering/Compilation/EditorRenderTargetArtifactProvider.cs#L154) | Reads and validates the shader definition value from its authoritative source. |
| [`Inno.Editor.Rendering.EditorShaderCompilationSnapshot Inno.Editor.Rendering.EditorRenderTargetArtifactProvider.RequestShaderCompilation(Inno.Rendering.Assets.ShaderAsset shader, Inno.Rendering.Assets.RenderShaderVariant variant, Inno.Rendering.GraphicsCapabilities capabilities)`](../../src/composition/editor/features/Inno.Editor.Rendering/Compilation/EditorRenderTargetArtifactProvider.cs#L177) | Schedules compilation if required and reports saving-independent state for the exact shader, variant and device target. |
| [`Inno.Editor.Rendering.EditorRenderTargetArtifactProvider`](../../src/composition/editor/features/Inno.Editor.Rendering/Compilation/EditorRenderTargetArtifactProvider.cs#L23) | Produces target-specific render artifacts from imported authoring assets without exposing source access to the backend-neutral render runtime. |

### `Inno.Editor.Rendering.EditorRenderingModule`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Rendering.EditorRenderingModule.EditorRenderingModule(Inno.Editor.Rendering.IEditorRenderingHost host, Inno.Editor.Interactions.EditorInteractions interactions, Inno.Extensibility.Types.TypeCatalog types)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorRenderingModule.cs#L55) | Creates the composition host around stable rendering and interaction services. |
| [`Inno.Editor.Rendering.EditorGizmoFrame Inno.Editor.Rendering.EditorRenderingModule.CollectGizmos(string viewportId, int pixelWidth, int pixelHeight)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorRenderingModule.cs#L83) | Collects Editor-only icons and bounds for the current Scene viewport. |
| [`void Inno.Editor.Rendering.EditorRenderingModule.Draw(Inno.Editor.Rendering.EditorViewportOutput output, System.Numerics.Vector2 logicalSize)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorRenderingModule.cs#L558) | Draws a ready output in the current panel. |
| [`void Inno.Editor.Rendering.EditorRenderingModule.DrawControllerToolbar(Inno.Editor.Rendering.EditorViewportKindId kind, string viewportId, int pixelWidth, int pixelHeight)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorRenderingModule.cs#L319) | Draws toolbar controls owned by the selected model controller. |
| [`string? Inno.Editor.Rendering.EditorRenderingModule.GetCompositionError(string viewportId)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorRenderingModule.cs#L158) | Gets the most recent isolated contribution or composition failure for one stable viewport. |
| [`Inno.Editor.Rendering.EditorViewportNavigationState Inno.Editor.Rendering.EditorRenderingModule.GetNavigationState(string viewportId)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorRenderingModule.cs#L173) | Gets the host-owned neutral navigation state for one stable Editor viewport. |
| [`void Inno.Editor.Rendering.EditorRenderingModule.HandlePointer(Inno.Editor.Rendering.EditorViewportKindId kind, string viewportId, int pixelWidth, int pixelHeight, float x, float y, int button)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorRenderingModule.cs#L518) | Forwards a normalized click to the selected model controller. |
| [`bool Inno.Editor.Rendering.EditorRenderingModule.HasContributors(Inno.Editor.Rendering.EditorViewportKindId kind)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorRenderingModule.cs#L110) | Gets whether the current extension generation contributes to one viewport purpose. |
| [`override void Inno.Editor.Rendering.EditorRenderingModule.OnDispose()`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorRenderingModule.cs#L672) | Releases resources retained by this feature after it has stopped. |
| [`override void Inno.Editor.Rendering.EditorRenderingModule.OnStart(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorRenderingModule.cs#L587) | Initializes this feature when its owning runtime becomes active. |
| [`override void Inno.Editor.Rendering.EditorRenderingModule.OnStop(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorRenderingModule.cs#L654) | Stops this feature before its owning runtime releases the active generation. |
| [`override void Inno.Editor.Rendering.EditorRenderingModule.OnUpdate(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorRenderingModule.cs#L599) | Advances this feature using the current runtime state. |
| [`void Inno.Editor.Rendering.EditorRenderingModule.Release(string viewportId)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorRenderingModule.cs#L569) | Stops retaining one viewport target. |
| [`void Inno.Editor.Rendering.EditorRenderingModule.SetContentScope(string viewportId, Inno.References.ContentReadScope content)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorRenderingModule.cs#L194) | Sets the explicit ordered host content visible to one Editor viewport. |
| [`void Inno.Editor.Rendering.EditorRenderingModule.SetOutputInput(string viewportId, Inno.Rendering.Runtime.RenderOutputInput input)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorRenderingModule.cs#L121) | Supplies viewport-local input captured by the output panel for this frame. |
| [`void Inno.Editor.Rendering.EditorRenderingModule.SetOutputRoute(string viewportId, Inno.Rendering.Runtime.RenderOutputRoute? route)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorRenderingModule.cs#L138) | Sets the exact model contributor order for an output with several applicable models. |
| [`void Inno.Editor.Rendering.EditorRenderingModule.SetPresentation(string viewportId, Inno.Editor.Rendering.EditorViewportPresentation presentation)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorRenderingModule.cs#L274) | Sets presentation preferences supplied to every contributor for one Editor viewport. |
| [`bool Inno.Editor.Rendering.EditorRenderingModule.TryConfigureNavigation(Inno.Editor.Rendering.EditorViewportKindId kind, string viewportId, int pixelWidth, int pixelHeight, out Inno.Editor.Rendering.EditorViewportNavigationProfile profile)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorRenderingModule.cs#L227) | Queries the selected controller's navigation contract before viewport input is processed. |
| [`bool Inno.Editor.Rendering.EditorRenderingModule.TryGetManipulationSpace(string viewportId, out Inno.Editor.Rendering.EditorViewportManipulationSpace space)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorRenderingModule.cs#L295) | Tries to get the manipulation space from the selected controller's latest contribution. |
| [`bool Inno.Editor.Rendering.EditorRenderingModule.TrySubmit(Inno.Editor.Rendering.EditorViewportKindId kind, string viewportId, int pixelWidth, int pixelHeight, out Inno.Editor.Rendering.EditorViewportOutput output)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorRenderingModule.cs#L368) | Builds, composes, submits, and returns one host-owned offscreen viewport. |
| [`Inno.Editor.Rendering.EditorRenderingModule`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorRenderingModule.cs#L21) | Composes reloadable rendering-model contributors while retaining only opaque presentation outputs. |

### `Inno.Editor.Rendering.EditorShaderArtifactValidationSnapshot`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Rendering.EditorShaderArtifactValidationSnapshot`](../../src/composition/editor/features/Inno.Editor.Rendering/Compilation/EditorShaderArtifactValidation.cs#L17) | Contains active-device validation state for one immutable Editor shader candidate. |

### `Inno.Editor.Rendering.EditorShaderCompilation`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Rendering.EditorShaderCompilation.EditorShaderCompilation(Inno.Editor.Rendering.EditorRenderTargetArtifactProvider provider, Inno.Editor.Rendering.IEditorShaderArtifactValidator validator)`](../../src/composition/editor/features/Inno.Editor.Rendering/Compilation/EditorShaderCompilation.cs#L103) | Pairs the authoring artifact owner with active-device validation at frame safety points. |
| [`Inno.Editor.Rendering.EditorShaderCompilation.EditorShaderCompilation(Inno.Editor.Rendering.EditorRenderTargetArtifactProvider provider, Inno.Rendering.GraphicsCapabilities capabilities)`](../../src/composition/editor/features/Inno.Editor.Rendering/Compilation/EditorShaderCompilation.cs#L86) | Pairs the authoring artifact owner with the active device's immutable capability snapshot. |
| [`Inno.Rendering.Assets.ShaderDefinition Inno.Editor.Rendering.EditorShaderCompilation.ReadDefinition(Inno.Rendering.Assets.RenderShaderArtifact artifact)`](../../src/composition/editor/features/Inno.Editor.Rendering/Compilation/EditorShaderCompilation.cs#L159) | Decodes the exact contract captured with a candidate using the current owner's references. |
| [`void Inno.Editor.Rendering.EditorShaderCompilation.ReleaseDraft(System.Guid documentId)`](../../src/composition/editor/features/Inno.Editor.Rendering/Compilation/EditorShaderCompilation.cs#L216) | Cancels a document's preview work and retires its cached candidate without touching canonical artifacts. |
| [`Inno.Editor.Rendering.EditorShaderCompilationSnapshot Inno.Editor.Rendering.EditorShaderCompilation.Request(Inno.Rendering.Assets.ShaderAsset shader, Inno.Rendering.Assets.RenderShaderVariant variant)`](../../src/composition/editor/features/Inno.Editor.Rendering/Compilation/EditorShaderCompilation.cs#L123) | Requests current compilation and reads its explicit last-good status. |
| [`Inno.Editor.Rendering.EditorShaderDraftCompilationSnapshot Inno.Editor.Rendering.EditorShaderCompilation.RequestArtifact(Inno.Rendering.Assets.ShaderAsset shader, Inno.Rendering.Assets.RenderShaderVariant variant)`](../../src/composition/editor/features/Inno.Editor.Rendering/Compilation/EditorShaderCompilation.cs#L141) | Reads the canonical Shader candidate for an isolated Material preview without modifying Material values. |
| [`Inno.Editor.Rendering.EditorShaderDraftCompilationSnapshot Inno.Editor.Rendering.EditorShaderCompilation.RequestDraft(System.Guid documentId, Inno.Core.Graphs.GraphDocument graph, ulong revision, Inno.Rendering.Assets.RenderShaderVariant variant)`](../../src/composition/editor/features/Inno.Editor.Rendering/Compilation/EditorShaderCompilation.cs#L179) | Compiles a detached document through the same graph toolchain in an isolated preview cache. |
| [`Inno.Editor.Rendering.EditorShaderCompilation`](../../src/composition/editor/features/Inno.Editor.Rendering/Compilation/EditorShaderCompilation.cs#L72) | Schedules Editor shader compilation for the active device without exposing native handles to authoring panels. |

### `Inno.Editor.Rendering.EditorShaderCompilationSnapshot`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Rendering.EditorShaderCompilationSnapshot`](../../src/composition/editor/features/Inno.Editor.Rendering/Compilation/EditorShaderCompilation.cs#L41) | Contains detached compiler state without retaining assets, tasks or extension instances. |

### `Inno.Editor.Rendering.EditorShaderCompilationState`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Rendering.EditorShaderCompilationState.Compiling`](../../src/composition/editor/features/Inno.Editor.Rendering/Compilation/EditorShaderCompilation.cs#L18) | The latest semantic candidate is being compiled. |
| [`Inno.Editor.Rendering.EditorShaderCompilationState.Failed`](../../src/composition/editor/features/Inno.Editor.Rendering/Compilation/EditorShaderCompilation.cs#L26) | The latest semantic candidate failed. |
| [`Inno.Editor.Rendering.EditorShaderCompilationState.Succeeded`](../../src/composition/editor/features/Inno.Editor.Rendering/Compilation/EditorShaderCompilation.cs#L22) | The latest semantic candidate compiled successfully. |
| [`Inno.Editor.Rendering.EditorShaderCompilationState`](../../src/composition/editor/features/Inno.Editor.Rendering/Compilation/EditorShaderCompilation.cs#L13) | Identifies native compilation state independently of document persistence. |

### `Inno.Editor.Rendering.EditorShaderDraftCompilationSnapshot`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Rendering.EditorShaderDraftCompilationSnapshot`](../../src/composition/editor/features/Inno.Editor.Rendering/Compilation/EditorShaderCompilation.cs#L62) | Contains a preview-only immutable candidate, never registered as a canonical asset artifact. |

### `Inno.Editor.Rendering.EditorViewportComposition`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Rendering.EditorViewportComposition.EditorViewportComposition(string viewportId, int pixelWidth, int pixelHeight, Inno.Rendering.RenderTextureFormat targetFormat, System.Collections.Generic.IEnumerable<Inno.Editor.Rendering.EditorViewportLayer> layers)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportOutput.cs#L207) | Creates an immutable viewport composition. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Editor.Rendering.EditorViewportLayer> Inno.Editor.Rendering.EditorViewportComposition.layers`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportOutput.cs#L261) | Gets the ordered rendering-model layers. |
| [`int Inno.Editor.Rendering.EditorViewportComposition.pixelHeight`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportOutput.cs#L251) | Gets the target height. |
| [`int Inno.Editor.Rendering.EditorViewportComposition.pixelWidth`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportOutput.cs#L246) | Gets the target width. |
| [`Inno.Rendering.RenderTextureFormat Inno.Editor.Rendering.EditorViewportComposition.targetFormat`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportOutput.cs#L256) | Gets the shared presentation target format. |
| [`string Inno.Editor.Rendering.EditorViewportComposition.viewportId`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportOutput.cs#L241) | Gets the stable viewport identity. |
| [`Inno.Editor.Rendering.EditorViewportComposition`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportOutput.cs#L185) | Describes a complete ordered set of rendering-model layers targeting one Editor viewport. |

### `Inno.Editor.Rendering.EditorViewportContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.References.ContentReadScope Inno.Editor.Rendering.EditorViewportContext.content`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L184) | Gets the explicit ordered host content visible to this viewport. |
| [`Inno.Editor.Core.EditorContext Inno.Editor.Rendering.EditorViewportContext.editor`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L149) | Gets the current Editor context. |
| [`ulong Inno.Editor.Rendering.EditorViewportContext.frameIndex`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L199) | Gets the frame index shared by all Editor views in this output frame. |
| [`Inno.Rendering.Runtime.RenderOutputInput Inno.Editor.Rendering.EditorViewportContext.input`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L203) | Gets input located in this viewport's physical pixels. |
| [`Inno.Editor.Interactions.EditorInteractions Inno.Editor.Rendering.EditorViewportContext.interactions`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L154) | Gets the shared Editor interaction and selection service. |
| [`Inno.Editor.Rendering.EditorViewportKindId Inno.Editor.Rendering.EditorViewportContext.kind`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L159) | Gets the open viewport purpose. |
| [`Inno.Editor.Rendering.EditorViewportNavigationState Inno.Editor.Rendering.EditorViewportContext.navigation`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L179) | Gets host-owned neutral navigation state that the selected controller can map to its camera model. |
| [`int Inno.Editor.Rendering.EditorViewportContext.pixelHeight`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L174) | Gets target height in physical pixels. |
| [`int Inno.Editor.Rendering.EditorViewportContext.pixelWidth`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L169) | Gets target width in physical pixels. |
| [`Inno.Editor.Rendering.EditorViewportPresentation Inno.Editor.Rendering.EditorViewportContext.presentation`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L189) | Gets host-selected presentation preferences for this viewport. |
| [`Inno.Rendering.Runtime.IViewContentCollector Inno.Editor.Rendering.EditorViewportContext.viewContent`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L194) | Gets the active generation's model-independent world-content collector. |
| [`string Inno.Editor.Rendering.EditorViewportContext.viewportId`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L164) | Gets the stable panel viewport identity. |
| [`Inno.Editor.Rendering.EditorViewportContext`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L108) | Supplies frame-only Editor interaction and output dimensions to a contributor. |

### `Inno.Editor.Rendering.EditorViewportContribution`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Rendering.EditorViewportContribution.EditorViewportContribution(Inno.Rendering.Runtime.RenderFrameData data, Inno.Rendering.Assets.RenderPipelineAsset? pipeline = null, Inno.Rendering.RenderTextureFormat targetFormat = Inno.Rendering.RenderTextureFormat.RGBA8Srgb, Inno.Editor.Rendering.EditorViewportManipulationSpace? manipulationSpace = null)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L346) | Creates a viewport model contribution. |
| [`Inno.Rendering.Runtime.RenderFrameData Inno.Editor.Rendering.EditorViewportContribution.data`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L361) | Gets pipeline-defined frame-only data. |
| [`Inno.Editor.Rendering.EditorViewportManipulationSpace? Inno.Editor.Rendering.EditorViewportContribution.manipulationSpace`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L376) | Gets the optional exact view/projection contract used by host-owned transform manipulation tools. |
| [`Inno.Rendering.Assets.RenderPipelineAsset? Inno.Editor.Rendering.EditorViewportContribution.pipeline`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L366) | Gets the selected pipeline, or null for the project default. |
| [`Inno.Rendering.RenderTextureFormat Inno.Editor.Rendering.EditorViewportContribution.targetFormat`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L371) | Gets the presentation target format expected by the pipeline. |
| [`Inno.Editor.Rendering.EditorViewportContribution`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L329) | Returns one rendering-model contribution without exposing a GPU backend. |

### `Inno.Editor.Rendering.EditorViewportContributor`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Rendering.EditorViewportContributor.EditorViewportContributor()`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L425) | Creates a parameterless reloadable viewport contributor. |
| [`abstract Inno.Editor.Rendering.EditorViewportContribution Inno.Editor.Rendering.EditorViewportContributor.Build(Inno.Editor.Rendering.EditorViewportContext context)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L461) | Builds one model-neutral render contribution for the current frame. |
| [`abstract bool Inno.Editor.Rendering.EditorViewportContributor.CanContribute(Inno.Editor.Rendering.EditorViewportContext context)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L438) | Determines whether this rendering model participates in the supplied viewport content. |
| [`virtual Inno.Editor.Rendering.EditorViewportNavigationProfile Inno.Editor.Rendering.EditorViewportContributor.ConfigureNavigation(Inno.Editor.Rendering.EditorViewportContext context)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L449) | Configures neutral host navigation before input is processed and a request is built. |
| [`virtual void Inno.Editor.Rendering.EditorViewportContributor.DrawToolbar(Inno.Editor.Rendering.EditorViewportContext context)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L469) | Draws optional controller-specific toolbar controls when this contributor owns viewport interaction. |
| [`virtual void Inno.Editor.Rendering.EditorViewportContributor.HandlePointer(Inno.Editor.Rendering.EditorViewportPointerContext context)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L479) | Handles one pointer click when this contributor owns viewport interaction. |
| [`Inno.Editor.Rendering.EditorViewportContributor`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L420) | Contributes one rendering model while the host owns viewport composition, targets, and presentation. |

### `Inno.Editor.Rendering.EditorViewportContributorExtensionAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Rendering.EditorViewportContributorExtensionAttribute.EditorViewportContributorExtensionAttribute(string id, string kind, int order = 0, int controllerPriority = 0)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L70) | Creates a viewport contributor declaration. |
| [`int Inno.Editor.Rendering.EditorViewportContributorExtensionAttribute.controllerPriority`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L102) | Gets the priority used to select the viewport interaction controller. |
| [`string Inno.Editor.Rendering.EditorViewportContributorExtensionAttribute.id`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L87) | Gets the globally stable contributor identity. |
| [`Inno.Editor.Rendering.EditorViewportKindId Inno.Editor.Rendering.EditorViewportContributorExtensionAttribute.kind`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L92) | Gets the open viewport purpose handled by the contributor. |
| [`int Inno.Editor.Rendering.EditorViewportContributorExtensionAttribute.order`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L97) | Gets ascending model-composition order. |
| [`Inno.Editor.Rendering.EditorViewportContributorExtensionAttribute`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L52) | Marks a reloadable rendering-model contributor for one Editor viewport purpose. |

### `Inno.Editor.Rendering.EditorViewportFocusBounds`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Rendering.EditorViewportFocusBounds.EditorViewportFocusBounds(Inno.Core.Mathematics.Vector3 center, float radius)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L142) | Creates a finite focus bound. |
| [`Inno.Core.Mathematics.Vector3 Inno.Editor.Rendering.EditorViewportFocusBounds.center`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L157) | Gets the world-space focus center. |
| [`float Inno.Editor.Rendering.EditorViewportFocusBounds.radius`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L162) | Gets the non-negative world-space radius. |
| [`Inno.Editor.Rendering.EditorViewportFocusBounds`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L125) | Describes a world-space sphere that can be framed by host navigation. |

### `Inno.Editor.Rendering.EditorViewportKindId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Rendering.EditorViewportKindId.EditorViewportKindId(string value)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L24) | Creates a stable viewport purpose identifier. |
| [`override string Inno.Editor.Rendering.EditorViewportKindId.ToString()`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L46) | Formats this value as a human-readable representation. |
| [`bool Inno.Editor.Rendering.EditorViewportKindId.isValid`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L38) | Gets whether this identifier is usable. |
| [`string Inno.Editor.Rendering.EditorViewportKindId.value`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L33) | Gets the stable viewport purpose. |
| [`Inno.Editor.Rendering.EditorViewportKindId`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L16) | Identifies one open Editor viewport purpose without prescribing rendering semantics. |

### `Inno.Editor.Rendering.EditorViewportLayer`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Rendering.EditorViewportLayer.EditorViewportLayer(string contributorId, Inno.Rendering.Assets.RenderPipelineAsset? pipeline, Inno.Rendering.Runtime.RenderFrameData data, int order)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportOutput.cs#L147) | Creates one immutable model-neutral viewport layer. |
| [`string Inno.Editor.Rendering.EditorViewportLayer.contributorId`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportOutput.cs#L164) | Gets the stable identity of the rendering-model contributor. |
| [`Inno.Rendering.Runtime.RenderFrameData Inno.Editor.Rendering.EditorViewportLayer.data`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportOutput.cs#L174) | Gets contributor-defined frame-only data. |
| [`int Inno.Editor.Rendering.EditorViewportLayer.order`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportOutput.cs#L179) | Gets ascending model-composition order. |
| [`Inno.Rendering.Assets.RenderPipelineAsset? Inno.Editor.Rendering.EditorViewportLayer.pipeline`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportOutput.cs#L169) | Gets the contributor-selected pipeline, or null for the project default. |
| [`Inno.Editor.Rendering.EditorViewportLayer`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportOutput.cs#L130) | Describes one model-neutral layer in an Editor viewport composition. |

### `Inno.Editor.Rendering.EditorViewportManipulationPlane`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Rendering.EditorViewportManipulationPlane.Spatial`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L249) | Keeps the full three-dimensional translation, rotation, and scale controls. |
| [`Inno.Editor.Rendering.EditorViewportManipulationPlane.XY`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L254) | Uses the X/Y translation and scale axes with the Z rotation handle. |
| [`Inno.Editor.Rendering.EditorViewportManipulationPlane.XZ`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L259) | Uses the X/Z translation and scale axes with the Y rotation handle. |
| [`Inno.Editor.Rendering.EditorViewportManipulationPlane.YZ`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L264) | Uses the Y/Z translation and scale axes with the X rotation handle. |
| [`Inno.Editor.Rendering.EditorViewportManipulationPlane`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L244) | Selects whether transform handles use all axes or a contributor-declared planar orientation. |

### `Inno.Editor.Rendering.EditorViewportManipulationSpace`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Rendering.EditorViewportManipulationSpace.EditorViewportManipulationSpace(Inno.Core.Mathematics.Matrix viewMatrix, Inno.Core.Mathematics.Matrix projectionMatrix, bool isOrthographic, Inno.Editor.Rendering.EditorViewportManipulationPlane plane = Inno.Editor.Rendering.EditorViewportManipulationPlane.Spatial)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L291) | Creates a manipulation space matching one rendered viewport frame. |
| [`bool Inno.Editor.Rendering.EditorViewportManipulationSpace.isOrthographic`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L318) | Gets whether the submitted frame used an orthographic projection. |
| [`Inno.Editor.Rendering.EditorViewportManipulationPlane Inno.Editor.Rendering.EditorViewportManipulationSpace.plane`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L323) | Gets the contributor-declared neutral manipulation plane, or Spatial for all axes. |
| [`Inno.Core.Mathematics.Matrix Inno.Editor.Rendering.EditorViewportManipulationSpace.projectionMatrix`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L313) | Gets the view-to-clip matrix used by the submitted frame. |
| [`Inno.Core.Mathematics.Matrix Inno.Editor.Rendering.EditorViewportManipulationSpace.viewMatrix`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L308) | Gets the world-to-view matrix used by the submitted frame. |
| [`Inno.Editor.Rendering.EditorViewportManipulationSpace`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L271) | Describes the exact backend-neutral view and projection used to draw a viewport so host tools can manipulate selected scene transforms without knowing the controller's camera model. |

### `Inno.Editor.Rendering.EditorViewportNavigationCapabilities`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Rendering.EditorViewportNavigationCapabilities.Fly`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L78) | Allows free-look movement. |
| [`Inno.Editor.Rendering.EditorViewportNavigationCapabilities.FrameSelection`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L83) | Allows framing a provider-supplied or host-derived selection bound. |
| [`Inno.Editor.Rendering.EditorViewportNavigationCapabilities.None`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L53) | Disables host-owned navigation. |
| [`Inno.Editor.Rendering.EditorViewportNavigationCapabilities.Orbit`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L73) | Allows pivot-oriented orbit and dolly. |
| [`Inno.Editor.Rendering.EditorViewportNavigationCapabilities.Pan`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L58) | Allows translating the view parallel to its image plane. |
| [`Inno.Editor.Rendering.EditorViewportNavigationCapabilities.Planar`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L68) | Allows the complete plane-oriented pan and zoom interaction model. |
| [`Inno.Editor.Rendering.EditorViewportNavigationCapabilities.Zoom`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L63) | Allows changing orthographic size or perspective focus distance. |
| [`Inno.Editor.Rendering.EditorViewportNavigationCapabilities`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L47) | Declares navigation operations supported by one viewport interaction controller. |

### `Inno.Editor.Rendering.EditorViewportNavigationMode`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Rendering.EditorViewportNavigationMode.Fly`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L41) | Moves and looks freely from the current view position. |
| [`Inno.Editor.Rendering.EditorViewportNavigationMode.Orbit`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L36) | Rotates around a focus pivot and dollies along the view direction. |
| [`Inno.Editor.Rendering.EditorViewportNavigationMode.Planar`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L31) | Pans and zooms over a provider-defined plane. |
| [`Inno.Editor.Rendering.EditorViewportNavigationMode`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L26) | Identifies the active interaction model without prescribing a rendering camera type. |

### `Inno.Editor.Rendering.EditorViewportNavigationProfile`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Rendering.EditorViewportNavigationProfile.EditorViewportNavigationProfile(Inno.Editor.Rendering.EditorViewportNavigationProfileId id, Inno.Editor.Rendering.EditorViewportNavigationCapabilities capabilities, Inno.Editor.Rendering.EditorViewportNavigationMode defaultMode)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L184) | Creates a provider-defined navigation profile. |
| [`Inno.Editor.Rendering.EditorViewportNavigationCapabilities Inno.Editor.Rendering.EditorViewportNavigationProfile.capabilities`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L219) | Gets the operations accepted by this provider. |
| [`Inno.Editor.Rendering.EditorViewportNavigationMode Inno.Editor.Rendering.EditorViewportNavigationProfile.defaultMode`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L224) | Gets the mode selected when current state is not supported. |
| [`static Inno.Editor.Rendering.EditorViewportNavigationProfile Inno.Editor.Rendering.EditorViewportNavigationProfile.disabled`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L279) | Gets a disabled fallback profile used when no provider navigation contract exists. |
| [`float Inno.Editor.Rendering.EditorViewportNavigationProfile.fastMovementMultiplier`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L269) | Gets or sets the multiplier used while fast fly movement is requested. |
| [`Inno.Editor.Rendering.EditorViewportFocusBounds? Inno.Editor.Rendering.EditorViewportNavigationProfile.focusBounds`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L234) | Gets or sets the optional current selection bound. |
| [`float Inno.Editor.Rendering.EditorViewportNavigationProfile.framePadding`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L274) | Gets or sets additional framing space around selection bounds. |
| [`Inno.Editor.Rendering.EditorViewportNavigationProfileId Inno.Editor.Rendering.EditorViewportNavigationProfile.id`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L214) | Gets the stable profile identity. |
| [`float Inno.Editor.Rendering.EditorViewportNavigationProfile.maximumFocusDistance`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L264) | Gets or sets maximum orbit or framing distance. |
| [`float Inno.Editor.Rendering.EditorViewportNavigationProfile.maximumOrthographicSize`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L254) | Gets or sets maximum orthographic half-height. |
| [`float Inno.Editor.Rendering.EditorViewportNavigationProfile.minimumFocusDistance`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L259) | Gets or sets minimum orbit or framing distance. |
| [`float Inno.Editor.Rendering.EditorViewportNavigationProfile.minimumOrthographicSize`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L249) | Gets or sets minimum orthographic half-height. |
| [`float Inno.Editor.Rendering.EditorViewportNavigationProfile.rotationSensitivity`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L239) | Gets or sets pointer rotation sensitivity in radians per pixel. |
| [`Inno.Core.Mathematics.Vector3 Inno.Editor.Rendering.EditorViewportNavigationProfile.worldUp`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L229) | Gets or sets the provider-defined world-up direction. |
| [`float Inno.Editor.Rendering.EditorViewportNavigationProfile.zoomSensitivity`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L244) | Gets or sets exponential wheel zoom sensitivity. |
| [`Inno.Editor.Rendering.EditorViewportNavigationProfile`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L170) | Describes how the host may navigate one provider-owned viewport without exposing the provider's camera model. |

### `Inno.Editor.Rendering.EditorViewportNavigationProfileId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Rendering.EditorViewportNavigationProfileId.EditorViewportNavigationProfileId(string value)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L97) | Creates a stable profile identifier. |
| [`override string Inno.Editor.Rendering.EditorViewportNavigationProfileId.ToString()`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L119) | Formats this value as a human-readable representation. |
| [`bool Inno.Editor.Rendering.EditorViewportNavigationProfileId.isValid`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L111) | Gets whether this identifier contains a usable value. |
| [`string Inno.Editor.Rendering.EditorViewportNavigationProfileId.value`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L106) | Gets the stable profile identity. |
| [`Inno.Editor.Rendering.EditorViewportNavigationProfileId`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L89) | Identifies a provider-defined navigation profile across reload generations. |

### `Inno.Editor.Rendering.EditorViewportNavigationState`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Editor.Rendering.EditorViewportNavigationState.ConfigureOrthographic(Inno.Core.Mathematics.Vector3 position, Inno.Core.Mathematics.Quaternion rotation, float orthographicSize)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L501) | Atomically initializes an orthographic navigation view. |
| [`void Inno.Editor.Rendering.EditorViewportNavigationState.ConfigurePerspective(Inno.Core.Mathematics.Vector3 position, Inno.Core.Mathematics.Quaternion rotation, float fieldOfView, float nearClip, float farClip)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L538) | Atomically initializes a perspective navigation view. |
| [`float Inno.Editor.Rendering.EditorViewportNavigationState.farClip`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L448) | Gets or sets the far clipping distance greater than the near distance. |
| [`float Inno.Editor.Rendering.EditorViewportNavigationState.fieldOfView`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L418) | Gets or sets the perspective vertical field of view in degrees. |
| [`float Inno.Editor.Rendering.EditorViewportNavigationState.focusDistance`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L463) | Gets or sets the positive orbit and framing distance. |
| [`bool Inno.Editor.Rendering.EditorViewportNavigationState.isInitialized`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L324) | Gets whether a provider or restored panel state initialized this state. |
| [`Inno.Editor.Rendering.EditorViewportNavigationMode Inno.Editor.Rendering.EditorViewportNavigationState.mode`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L344) | Gets or sets the active host navigation mode. |
| [`float Inno.Editor.Rendering.EditorViewportNavigationState.movementSpeed`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L478) | Gets or sets the positive base movement speed in world units per second. |
| [`float Inno.Editor.Rendering.EditorViewportNavigationState.nearClip`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L433) | Gets or sets the positive near clipping distance. |
| [`float Inno.Editor.Rendering.EditorViewportNavigationState.orthographicSize`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L403) | Gets or sets the positive orthographic half-height in provider world units. |
| [`Inno.Core.Mathematics.Vector3 Inno.Editor.Rendering.EditorViewportNavigationState.pivot`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L389) | Gets or sets the world-space orbit and framing pivot. |
| [`Inno.Core.Mathematics.Vector3 Inno.Editor.Rendering.EditorViewportNavigationState.position`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L359) | Gets or sets the provider-defined world-space view position. |
| [`Inno.Editor.Rendering.EditorViewportProjection Inno.Editor.Rendering.EditorViewportNavigationState.projection`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L329) | Gets or sets the current projection family. |
| [`Inno.Core.Mathematics.Quaternion Inno.Editor.Rendering.EditorViewportNavigationState.rotation`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L373) | Gets or sets the normalized provider-defined world-space view rotation. |
| [`Inno.Editor.Rendering.EditorViewportNavigationState`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L303) | Stores rendering-model-neutral navigation state for one Editor viewport across provider reloads. |

### `Inno.Editor.Rendering.EditorViewportOutput`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Rendering.EditorViewportOutput.EditorViewportOutput(string viewportId, Inno.Adapter.Presentation.PresentationTextureHandle texture, int pixelWidth, int pixelHeight)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportOutput.cs#L32) | Creates an editor viewport output snapshot. |
| [`bool Inno.Editor.Rendering.EditorViewportOutput.isReady`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportOutput.cs#L70) | Gets whether a completed target can be drawn. |
| [`int Inno.Editor.Rendering.EditorViewportOutput.pixelHeight`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportOutput.cs#L65) | Gets the current target height. |
| [`int Inno.Editor.Rendering.EditorViewportOutput.pixelWidth`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportOutput.cs#L60) | Gets the current target width. |
| [`Inno.Adapter.Presentation.PresentationTextureHandle Inno.Editor.Rendering.EditorViewportOutput.texture`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportOutput.cs#L55) | Gets the opaque presentation token. |
| [`string Inno.Editor.Rendering.EditorViewportOutput.viewportId`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportOutput.cs#L50) | Gets the stable viewport identity. |
| [`Inno.Editor.Rendering.EditorViewportOutput`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportOutput.cs#L15) | Identifies a renderer-owned editor viewport image through an opaque presentation token. |

### `Inno.Editor.Rendering.EditorViewportPointerContext`

| 当前声明 | 行为 |
| --- | --- |
| [`int Inno.Editor.Rendering.EditorViewportPointerContext.button`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L414) | Gets the platform-independent pointer button index. |
| [`Inno.Editor.Rendering.EditorViewportContext Inno.Editor.Rendering.EditorViewportPointerContext.viewport`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L399) | Gets the owning frame-only viewport context. |
| [`float Inno.Editor.Rendering.EditorViewportPointerContext.x`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L404) | Gets normalized horizontal pointer position. |
| [`float Inno.Editor.Rendering.EditorViewportPointerContext.y`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L409) | Gets normalized vertical pointer position. |
| [`Inno.Editor.Rendering.EditorViewportPointerContext`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L382) | Supplies normalized pointer interaction over one rendered viewport. |

### `Inno.Editor.Rendering.EditorViewportPresentation`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Rendering.EditorViewportPresentation.EditorViewportPresentation(Inno.Core.Mathematics.Color backgroundColor, float pixelDensity = 1)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L220) | Creates presentation preferences for one Editor viewport. |
| [`Inno.Core.Mathematics.Color Inno.Editor.Rendering.EditorViewportPresentation.backgroundColor`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L233) | Gets the linear clear color preferred by the host panel. |
| [`float Inno.Editor.Rendering.EditorViewportPresentation.pixelDensity`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L238) | Gets physical render pixels per logical presentation unit. |
| [`Inno.Editor.Rendering.EditorViewportPresentation`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportContributor.cs#L209) | Describes host-selected presentation preferences without prescribing rendering behavior. |

### `Inno.Editor.Rendering.EditorViewportProjection`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Rendering.EditorViewportProjection.Orthographic`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L15) | Uses a parallel projection controlled by an orthographic half-height. |
| [`Inno.Editor.Rendering.EditorViewportProjection.Perspective`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L20) | Uses a perspective projection controlled by a vertical field of view. |
| [`Inno.Editor.Rendering.EditorViewportProjection`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportNavigation.cs#L10) | Identifies the projection family represented by neutral Editor viewport navigation state. |

### `Inno.Editor.Rendering.IEditorGizmoSink`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Editor.Rendering.IEditorGizmoSink.Icon(Inno.Core.Identity.Identity owner, Inno.Core.Mathematics.Vector3 position, string iconId)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorGizmoProvider.cs#L76) | Adds a visible, selectable icon at an object's world position. |
| [`void Inno.Editor.Rendering.IEditorGizmoSink.Line(Inno.Core.Mathematics.Vector3 start, Inno.Core.Mathematics.Vector3 end)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorGizmoProvider.cs#L91) | Adds a non-interactive world-space line, normally for selected bounds. |
| [`Inno.Editor.Rendering.IEditorGizmoSink`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorGizmoProvider.cs#L62) | Receives transient world-space gizmo geometry from independent Editor extensions. |

### `Inno.Editor.Rendering.IEditorPreviewService`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Editor.Rendering.IEditorPreviewService.Draw(Inno.Editor.Rendering.EditorPreviewHandle handle, System.Numerics.Vector2 logicalSize)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorPreviewService.cs#L156) | Draws one current-generation preview into the active presentation surface. |
| [`bool Inno.Editor.Rendering.IEditorPreviewService.Release(Inno.Editor.Rendering.EditorPreviewHandle handle)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorPreviewService.cs#L170) | Releases one cached preview registration. |
| [`void Inno.Editor.Rendering.IEditorPreviewService.ReleaseAll()`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorPreviewService.cs#L175) | Releases every cached preview registration. |
| [`void Inno.Editor.Rendering.IEditorPreviewService.ReleaseRendered(string viewportId)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorPreviewService.cs#L103) | Releases an offscreen preview, including one which has not produced its first handle. |
| [`bool Inno.Editor.Rendering.IEditorPreviewService.TryGetTexture(Inno.Rendering.Assets.TextureAsset texture, out Inno.Editor.Rendering.EditorPreviewHandle handle)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorPreviewService.cs#L117) | Tries to resolve a standalone texture preview without blocking target compilation. |
| [`bool Inno.Editor.Rendering.IEditorPreviewService.TryGetTextureArtifact(Inno.Rendering.Assets.RenderTextureArtifactReference texture, int pixelWidth, int pixelHeight, out Inno.Editor.Rendering.EditorPreviewHandle handle)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorPreviewService.cs#L140) | Tries to resolve a named texture artifact preview without blocking target compilation. |
| [`bool Inno.Editor.Rendering.IEditorPreviewService.TryRender(Inno.Editor.Rendering.EditorViewportComposition composition, out Inno.Editor.Rendering.EditorPreviewHandle handle)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorPreviewService.cs#L92) | Submits an isolated rendering composition through the shared offscreen viewport bridge. |
| [`uint Inno.Editor.Rendering.IEditorPreviewService.deviceGeneration`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorPreviewService.cs#L78) | Gets the active rendering-device generation. |
| [`Inno.Editor.Rendering.IEditorPreviewService`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorPreviewService.cs#L73) | Provides one shared preview texture bridge for browsers, inspectors, and document canvases. |

### `Inno.Editor.Rendering.IEditorRenderingHost`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Editor.Rendering.IEditorRenderingHost.Draw(Inno.Editor.Rendering.EditorViewportOutput output, System.Numerics.Vector2 logicalSize)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportOutput.cs#L108) | Draws a ready viewport output inside the current ImGui window. |
| [`void Inno.Editor.Rendering.IEditorRenderingHost.Release(string viewportId)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportOutput.cs#L119) | Releases one viewport and queues its GPU target for frame-safe destruction. |
| [`void Inno.Editor.Rendering.IEditorRenderingHost.ReleaseAll()`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportOutput.cs#L124) | Releases every viewport owned by this editor host service. |
| [`Inno.Editor.Rendering.EditorViewportOutput Inno.Editor.Rendering.IEditorRenderingHost.Submit(Inno.Editor.Rendering.EditorViewportComposition composition)`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportOutput.cs#L97) | Submits or updates one composed offscreen editor viewport. |
| [`ulong Inno.Editor.Rendering.IEditorRenderingHost.currentFrameIndex`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportOutput.cs#L86) | Gets the current output frame index shared by Editor views. |
| [`Inno.Rendering.Runtime.IViewContentCollector Inno.Editor.Rendering.IEditorRenderingHost.viewContent`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportOutput.cs#L81) | Gets the active generation's model-independent world-content collector. |
| [`Inno.Editor.Rendering.IEditorRenderingHost`](../../src/composition/editor/features/Inno.Editor.Rendering/EditorViewportOutput.cs#L76) | Bridges editor extensions to a host-owned render request sink and opaque texture presenter. |

### `Inno.Editor.Rendering.IEditorShaderArtifactValidator`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Editor.Rendering.IEditorShaderArtifactValidator.Release(System.Guid documentId)`](../../src/composition/editor/features/Inno.Editor.Rendering/Compilation/EditorShaderArtifactValidation.cs#L59) | Forgets pending and completed validation state owned by one document. |
| [`Inno.Editor.Rendering.EditorShaderArtifactValidationSnapshot Inno.Editor.Rendering.IEditorShaderArtifactValidator.Request(System.Guid documentId, ulong revision, Inno.Rendering.Assets.RenderShaderArtifact artifact)`](../../src/composition/editor/features/Inno.Editor.Rendering/Compilation/EditorShaderArtifactValidation.cs#L47) | Requests or reads validation for an exact document revision and immutable artifact. |
| [`Inno.Rendering.GraphicsCapabilities Inno.Editor.Rendering.IEditorShaderArtifactValidator.capabilities`](../../src/composition/editor/features/Inno.Editor.Rendering/Compilation/EditorShaderArtifactValidation.cs#L30) | Gets the capability snapshot used to compile candidates for this validator. |
| [`Inno.Editor.Rendering.IEditorShaderArtifactValidator`](../../src/composition/editor/features/Inno.Editor.Rendering/Compilation/EditorShaderArtifactValidation.cs#L25) | Queues immutable shader candidates for validation at the active device's next frame safety point. |

### `Inno.Editor.Rendering.PipelineDocuments`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Editor.Rendering.PipelineDocuments.Commit(System.Guid assetId)`](../../src/composition/editor/features/Inno.Editor.Rendering/Pipelines/PipelineDocuments.cs#L84) | Completes one gesture without saving the source. |
| [`override void Inno.Editor.Rendering.PipelineDocuments.OnStart(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/features/Inno.Editor.Rendering/Pipelines/PipelineDocuments.cs#L133) | Initializes this feature when its owning runtime becomes active. |
| [`override void Inno.Editor.Rendering.PipelineDocuments.OnStop(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/features/Inno.Editor.Rendering/Pipelines/PipelineDocuments.cs#L147) | Stops this feature before its owning runtime releases the active generation. |
| [`override void Inno.Editor.Rendering.PipelineDocuments.OnUpdate(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/features/Inno.Editor.Rendering/Pipelines/PipelineDocuments.cs#L140) | Advances this feature using the current runtime state. |
| [`System.Guid Inno.Editor.Rendering.PipelineDocuments.Open(Inno.Assets.AssetPath path)`](../../src/composition/editor/features/Inno.Editor.Rendering/Pipelines/PipelineDocuments.cs#L46) | Opens a native Pipeline source in the shared document service, including failed imports. |
| [`Inno.Rendering.Assets.RenderPipelineAsset Inno.Editor.Rendering.PipelineDocuments.Read(System.Guid assetId)`](../../src/composition/editor/features/Inno.Editor.Rendering/Pipelines/PipelineDocuments.cs#L57) | Reads a detached current-generation Pipeline value; its referenced canonical assets must not be mutated. |
| [`void Inno.Editor.Rendering.PipelineDocuments.Replace(System.Guid assetId, Inno.Rendering.Assets.RenderPipelineAsset candidate, bool finishGesture = true)`](../../src/composition/editor/features/Inno.Editor.Rendering/Pipelines/PipelineDocuments.cs#L71) | Changes a draft through shared History without publishing it to Scene/Game. |
| [`void Inno.Editor.Rendering.PipelineDocuments.ReplaceSettings<TSettings>(System.Guid assetId, TSettings settings, bool finishGesture = true)`](../../src/composition/editor/features/Inno.Editor.Rendering/Pipelines/PipelineDocuments.cs#L101) | Captures typed Pipeline settings with this source owner's complete reference and dependency context. |
| [`Inno.Editor.Rendering.PipelineDocuments`](../../src/composition/editor/features/Inno.Editor.Rendering/Pipelines/PipelineDocuments.cs#L18) | Edits Pipeline sources and typed extension settings without changing canonical rendering assets before Save. |

## 项目依赖

- [Inno.Editor.Assets](Inno.Editor.Assets.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Editor.Inspection](Inno.Editor.Inspection.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Editor.ImGui](Inno.Editor.ImGui.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Native.ImGui](../native/Inno.Native.ImGui.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Adapter.Presentation.ImGui.Sdl3](../platform/Inno.Adapter.Presentation.ImGui.Sdl3.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Rendering.Runtime](../rendering/Inno.Rendering.Runtime.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Core.Mathematics](../core/Inno.Core.Mathematics.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Editor.Interactions](Inno.Editor.Interactions.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Editor.Core](Inno.Editor.Core.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Adapter.Presentation](../platform/Inno.Adapter.Presentation.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Rendering](../rendering/Inno.Rendering.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.References](../references/Inno.References.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Core.Identity](../core/Inno.Core.Identity.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Assets](../assets/Inno.Assets.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Core.Graphs](../core/Inno.Core.Graphs.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Assets.Pipeline](../assets/Inno.Assets.Pipeline.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Rendering.Assets](../rendering/Inno.Rendering.Assets.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Rendering.Assets.Authoring](../rendering/Inno.Rendering.Assets.Authoring.md)：项目引用；公开签名可见性由语义边界检查确认。

共同 MSBuild 注入的 analyzer 与编译规则属于构建依赖，完整有效项目图记录在本轮验收证据中。
