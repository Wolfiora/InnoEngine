# Inno.Rendering.Runtime

[分类索引](README.md) · [Wiki 首页](../README.md) · [本轮整改计划](../architecture/ARCHITECTURE_CLEANUP_PLAN_2026_10_06.md)

## 职责与边界

本层拥有输出模型、RenderRequest、Pipeline/Feature、资产到 GPU 的解析、资源缓存及退休；明确引用运行资产，不引用 Authoring、Assets Pipeline、Shaders 或编译工具链。ContentRenderTargetArtifactProvider 使用 content store，删除文件型 provider。Contributor 注册变化时生成 immutable snapshot，帧开始固定，帧中注册/注销在下一帧生效。私有 RenderFrameScratch 保留容量，每帧/异常/退出清空所有 extension 引用。Validate 与最终 Compile 分离；RenderFrameStatistics.graphCompileCount 记录最近完成帧的完整编译次数。无主输出时跳过主输出建图与输入换算，离屏输出继续。

## 初始化、扩展与资源退休

宿主注入 TypeCatalog、IRenderDevice、诊断与内容 artifact provider。Runtime 拥有模型、Pipeline generation、资源 service、输出 target 和退休队列；设备属于宿主。注册 Contributor 后在帧开始固定 snapshot，帧内变化下一帧生效。失败 Contributor 的 mutation 完整回滚，当前帧继续采用已接受图；最后只构造一次完整 CompiledRenderGraph。

```csharp
using Inno.Core.Diagnostics;
using Inno.Extensibility.Types;
using Inno.Rendering;
using Inno.Rendering.Runtime;

static RenderRuntime CreateRuntime(
    TypeCatalog types,
    IRenderDevice device,
    IDiagnosticReporter diagnostics
) {
    return new RenderRuntime(types, device, diagnostics);
}
```

ContentRenderTargetArtifactProvider 使用逻辑定位读取冻结 Shader、Texture 等产物；运行时不编译创作源码。输出缺失、能力缺失及损坏产物明确报告，候选失败保留 last-good。Material、Geometry、readback 和 framebuffer 各 owner 在对应 generation 安全点退休；scratch、snapshot 和 callback 在结束帧、异常和退出时清空旧扩展引用。

资源描述与 GPU 图机制见 [Core](Inno.Rendering.md)，持久资产见 [Assets](Inno.Rendering.Assets.md)，创作编译见 [Authoring](Inno.Rendering.Assets.Authoring.md)。该程序集不引用创作或编译工具，不内建 2D/3D 世界模型。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Rendering.Runtime.ContentRenderTargetArtifactProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.ShaderDefinition Inno.Rendering.Runtime.ContentRenderTargetArtifactProvider.ReadShaderDefinition(Inno.Rendering.Assets.RenderShaderArtifact artifact)`](../../src/services/rendering/Inno.Rendering.Runtime/Deployment/ContentRenderTargetArtifactProvider.cs#L56) | Reads and validates the shader definition value from its authoritative source. |
| [`Inno.Rendering.Runtime.ContentRenderTargetArtifactProvider`](../../src/services/rendering/Inno.Rendering.Runtime/Deployment/ContentRenderTargetArtifactProvider.cs#L15) | Reads immutable render target artifacts from one verified logical content store. |
| [`Inno.Rendering.Runtime.ContentRenderTargetArtifactProvider.ContentRenderTargetArtifactProvider(Inno.Content.IRuntimeContentStore content, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Runtime/Deployment/ContentRenderTargetArtifactProvider.cs#L37) | Creates a provider borrowing one source-free content store. |
| [`Inno.Rendering.Runtime.RenderTargetArtifactStatus Inno.Rendering.Runtime.ContentRenderTargetArtifactProvider.GetShaderArtifact(Inno.Rendering.Assets.ShaderAsset shader, Inno.Rendering.Assets.RenderShaderVariant variant, Inno.Rendering.GraphicsCapabilities capabilities, out Inno.Rendering.Assets.RenderShaderArtifact? artifact)`](../../src/services/rendering/Inno.Rendering.Runtime/Deployment/ContentRenderTargetArtifactProvider.cs#L87) | Loads and validates one packaged shader target artifact when it exists. |
| [`Inno.Rendering.Runtime.RenderTargetArtifactStatus Inno.Rendering.Runtime.ContentRenderTargetArtifactProvider.GetTextureArtifact(Inno.Rendering.Assets.RenderTextureArtifactReference texture, out System.ReadOnlyMemory<byte> artifact)`](../../src/services/rendering/Inno.Rendering.Runtime/Deployment/ContentRenderTargetArtifactProvider.cs#L145) | Loads one packaged portable texture artifact when it exists. |

### `Inno.Rendering.Runtime.GraphicsSettings`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.GraphicsSettings`](../../src/services/rendering/Inno.Rendering.Runtime/GraphicsSettings.cs#L12) | Exposes current rendering configuration and immutable device state. |
| [`static Inno.Rendering.Assets.RenderPipelineAsset? Inno.Rendering.Runtime.GraphicsSettings.defaultPipeline`](../../src/services/rendering/Inno.Rendering.Runtime/GraphicsSettings.cs#L23) | Gets or sets the project default pipeline used by requests without an override. |
| [`static Inno.Rendering.GraphicsCapabilities? Inno.Rendering.Runtime.GraphicsSettings.capabilities`](../../src/services/rendering/Inno.Rendering.Runtime/GraphicsSettings.cs#L18) | Gets current device capabilities, or before device initialization. |
| [`static Inno.Rendering.Runtime.RenderFrameStatistics? Inno.Rendering.Runtime.GraphicsSettings.frameStatistics`](../../src/services/rendering/Inno.Rendering.Runtime/GraphicsSettings.cs#L32) | Gets statistics for the last completed frame, or before the first frame. |

### `Inno.Rendering.Runtime.IPreparedViewDrawable`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.IPreparedViewDrawable`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L198) | Encodes a resource-ready draw into the owning model's scene pass. |
| [`void Inno.Rendering.Runtime.IPreparedViewDrawable.Encode(Inno.Rendering.RenderCommandEncoder commands)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L207) | Encodes commands while the owning model controls attachments and view state. |

### `Inno.Rendering.Runtime.IRenderModel`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.IRenderModel`](../../src/services/rendering/Inno.Rendering.Runtime/Models/IRenderModel.cs#L14) | Builds view requests from host output sessions without owning a host window. |
| [`Inno.Rendering.Runtime.RenderModelOutput Inno.Rendering.Runtime.IRenderModel.Build(Inno.Rendering.Runtime.RenderOutputSession session)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/IRenderModel.cs#L37) | Builds one model output after acceptance. |
| [`bool Inno.Rendering.Runtime.IRenderModel.CanRender(Inno.Rendering.Runtime.RenderOutputSession session)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/IRenderModel.cs#L26) | Determines whether this model accepts the session's content. |

### `Inno.Rendering.Runtime.IRenderRequestSink`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.IRenderRequestSink`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/IRenderRequestSink.cs#L9) | Accepts rendering-model-neutral requests without exposing runtime or backend ownership. |
| [`void Inno.Rendering.Runtime.IRenderRequestSink.Submit(Inno.Rendering.Runtime.RenderRequest request)`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/IRenderRequestSink.cs#L18) | Queues one immutable view request for the current or next render frame. |

### `Inno.Rendering.Runtime.IRenderResourceService`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.ComputePipelineHandle Inno.Rendering.Runtime.IRenderResourceService.AcquireComputePipeline(Inno.Rendering.Runtime.RenderPersistentResourceId id, long revision, Inno.Rendering.ComputePipelineDescriptor descriptor, string name)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L237) | Acquires or atomically replaces a provider-owned compute pipeline. |
| [`Inno.Rendering.GraphicsCapabilities Inno.Rendering.Runtime.IRenderResourceService.capabilities`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L21) | Gets the active backend-neutral capability snapshot. |
| [`Inno.Rendering.GraphicsPipelineHandle Inno.Rendering.Runtime.IRenderResourceService.AcquireGraphicsPipeline(Inno.Rendering.Runtime.RenderPersistentResourceId id, long revision, Inno.Rendering.GraphicsPipelineDescriptor descriptor, string name)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L212) | Acquires or atomically replaces a provider-owned graphics pipeline. |
| [`Inno.Rendering.PersistentBufferHandle Inno.Rendering.Runtime.IRenderResourceService.AcquireBuffer(Inno.Rendering.Runtime.RenderPersistentResourceId id, long revision, Inno.Rendering.PersistentBufferDescriptor descriptor, System.ReadOnlyMemory<byte> initialData, string name)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L89) | Acquires or atomically replaces a provider-owned persistent buffer. |
| [`Inno.Rendering.PersistentTextureHandle Inno.Rendering.Runtime.IRenderResourceService.AcquireKtxTexture(Inno.Rendering.Runtime.RenderPersistentResourceId id, long revision, System.ReadOnlyMemory<byte> containerData, bool sRgb, string name)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L147) | Acquires or atomically replaces a sampled texture from a portable KTX container. |
| [`Inno.Rendering.PersistentTextureHandle Inno.Rendering.Runtime.IRenderResourceService.AcquireTexture(Inno.Rendering.Runtime.RenderPersistentResourceId id, long revision, Inno.Rendering.RenderTextureDescriptor descriptor, System.Collections.Generic.IReadOnlyList<Inno.Rendering.Runtime.RenderTextureSubresourceData> subresources, string name)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L118) | Acquires or atomically replaces a provider-owned persistent texture. |
| [`Inno.Rendering.Runtime.IRenderResourceService`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L15) | Resolves neutral assets and provider-owned uploads into opaque resources for the active device generation. |
| [`Inno.Rendering.Runtime.RenderTargetArtifactStatus Inno.Rendering.Runtime.IRenderResourceService.PrewarmMaterial(Inno.Rendering.Assets.MaterialAsset material)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L50) | Queues all target shader work required by a material without blocking the render thread. |
| [`System.Threading.Tasks.ValueTask<Inno.Rendering.RenderTextureReadbackResult> Inno.Rendering.Runtime.IRenderResourceService.ReadTextureAsync(Inno.Rendering.PersistentTextureHandle texture, int mipLevel = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L188) | Asynchronously copies one complete persistent texture mip into CPU-visible memory. |
| [`bool Inno.Rendering.Runtime.IRenderResourceService.TryResolveComputeMaterial(Inno.Rendering.Assets.MaterialAsset material, Inno.Rendering.Assets.ShaderContractId contractId, Inno.Rendering.Assets.ShaderPassRoleId passRoleId, Inno.Rendering.Assets.MaterialPropertyBlock? overrides, out Inno.Rendering.Runtime.RenderMaterialPass? materialPass)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L350) | Resolves one compute material pass through an open provider contract and role. |
| [`bool Inno.Rendering.Runtime.IRenderResourceService.TryResolveGeometry(Inno.Rendering.Assets.GeometryAsset geometry, out Inno.Rendering.Runtime.RenderGeometry? resolvedGeometry)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L370) | Resolves imported helper geometry into persistent vertex and index buffers. |
| [`bool Inno.Rendering.Runtime.IRenderResourceService.TryResolveGraphicsMaterial(Inno.Rendering.Assets.MaterialAsset material, Inno.Rendering.Assets.ShaderContractId contractId, Inno.Rendering.Assets.ShaderPassRoleId passRoleId, Inno.Rendering.RenderVertexLayout? vertexLayout, Inno.Rendering.Assets.MaterialPropertyBlock? overrides, out Inno.Rendering.Runtime.RenderMaterialPass? materialPass)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L268) | Resolves one graphics material pass through an open provider contract and role. |
| [`bool Inno.Rendering.Runtime.IRenderResourceService.TryResolveMaterialArtifact(Inno.Rendering.Runtime.RenderPersistentResourceId scope, Inno.Rendering.Assets.RenderShaderArtifact artifact, Inno.Rendering.Assets.MaterialAsset material, Inno.Rendering.Assets.ShaderContractId contractId, Inno.Rendering.Assets.ShaderPassRoleId passRoleId, Inno.Rendering.ShaderProgramKind programKind, Inno.Rendering.RenderVertexLayout? vertexLayout, Inno.Rendering.Assets.MaterialPropertyBlock? overrides, Inno.Core.Diagnostics.IDiagnosticReporter diagnostics, out Inno.Rendering.Runtime.RenderMaterialPass? materialPass)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L316) | Resolves an explicitly compiled candidate in a caller-owned publication scope, without publishing it as an asset. |
| [`bool Inno.Rendering.Runtime.IRenderResourceService.TryResolveTexture(Inno.Rendering.Assets.TextureAsset texture, out Inno.Rendering.PersistentTextureHandle resolvedTexture)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L387) | Resolves an imported texture into a persistent sampled texture. |
| [`bool Inno.Rendering.Runtime.IRenderResourceService.TryResolveTextureArtifact(Inno.Rendering.Assets.RenderTextureArtifactReference texture, out Inno.Rendering.PersistentTextureHandle resolvedTexture)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L404) | Resolves one imported artifact texture into a persistent sampled texture. |
| [`void Inno.Rendering.Runtime.IRenderResourceService.PrewarmTexture(Inno.Rendering.Assets.TextureAsset texture)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L58) | Queues target texture conversion without blocking the render thread. |
| [`void Inno.Rendering.Runtime.IRenderResourceService.PrewarmTextureArtifact(Inno.Rendering.Assets.RenderTextureArtifactReference texture)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L66) | Queues target conversion for a texture slot owned by any imported asset. |
| [`void Inno.Rendering.Runtime.IRenderResourceService.Release(Inno.Rendering.Runtime.RenderPersistentResourceId id)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L417) | Releases cached resources with this provider-owned identifier at a safe GPU mutation point. |
| [`void Inno.Rendering.Runtime.IRenderResourceService.UpdateTexture(Inno.Rendering.PersistentTextureHandle texture, Inno.Rendering.RenderTextureRegion region, System.ReadOnlyMemory<byte> data)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L167) | Updates a rectangular region of an active persistent texture without recreating it. |
| [`void Inno.Rendering.Runtime.IRenderResourceService.ValidateShaderArtifact(Inno.Rendering.Assets.RenderShaderArtifact artifact)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L32) | Creates and retires every program in one compiled artifact through the active device without publishing it. |

### `Inno.Rendering.Runtime.IRenderRuntimeReloadTransaction`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.IRenderRuntimeReloadTransaction`](../../src/services/rendering/Inno.Rendering.Runtime/IRenderRuntimeReloadTransaction.cs#L6) | Controls one isolated rendering-extension candidate from preparation through atomic completion. |
| [`void Inno.Rendering.Runtime.IRenderRuntimeReloadTransaction.Activate()`](../../src/services/rendering/Inno.Rendering.Runtime/IRenderRuntimeReloadTransaction.cs#L16) | Atomically selects the prepared candidate without retiring the previous generation. |
| [`void Inno.Rendering.Runtime.IRenderRuntimeReloadTransaction.Complete()`](../../src/services/rendering/Inno.Rendering.Runtime/IRenderRuntimeReloadTransaction.cs#L21) | Commits an activated candidate and retires the previous generation. |
| [`void Inno.Rendering.Runtime.IRenderRuntimeReloadTransaction.Prepare()`](../../src/services/rendering/Inno.Rendering.Runtime/IRenderRuntimeReloadTransaction.cs#L11) | Builds and validates every candidate registry and last-good rendering generation. |
| [`void Inno.Rendering.Runtime.IRenderRuntimeReloadTransaction.Rollback()`](../../src/services/rendering/Inno.Rendering.Runtime/IRenderRuntimeReloadTransaction.cs#L26) | Discards the candidate and restores the previous generation after provisional activation. |

### `Inno.Rendering.Runtime.IRenderTargetArtifactProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.ShaderDefinition Inno.Rendering.Runtime.IRenderTargetArtifactProvider.ReadShaderDefinition(Inno.Rendering.Assets.RenderShaderArtifact artifact)`](../../src/services/rendering/Inno.Rendering.Runtime/Deployment/IRenderTargetArtifactProvider.cs#L25) | Resolves the exact material contract stored with a compiled program using the current owner reference context. |
| [`Inno.Rendering.Runtime.IRenderTargetArtifactProvider`](../../src/services/rendering/Inno.Rendering.Runtime/Deployment/IRenderTargetArtifactProvider.cs#L10) | Resolves immutable, source-free target artifacts for the active rendering device. |
| [`Inno.Rendering.Runtime.RenderTargetArtifactStatus Inno.Rendering.Runtime.IRenderTargetArtifactProvider.GetShaderArtifact(Inno.Rendering.Assets.ShaderAsset shader, Inno.Rendering.Assets.RenderShaderVariant variant, Inno.Rendering.GraphicsCapabilities capabilities, out Inno.Rendering.Assets.RenderShaderArtifact? artifact)`](../../src/services/rendering/Inno.Rendering.Runtime/Deployment/IRenderTargetArtifactProvider.cs#L49) | Resolves the target shader matching one runtime asset, variant, and device capability snapshot. |
| [`Inno.Rendering.Runtime.RenderTargetArtifactStatus Inno.Rendering.Runtime.IRenderTargetArtifactProvider.GetTextureArtifact(Inno.Rendering.Assets.RenderTextureArtifactReference texture, out System.ReadOnlyMemory<byte> artifact)`](../../src/services/rendering/Inno.Rendering.Runtime/Deployment/IRenderTargetArtifactProvider.cs#L72) | Resolves the portable KTX artifact for one imported runtime texture. |

### `Inno.Rendering.Runtime.IViewContentCollector`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.IViewContentCollector`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L427) | Collects all active world-content sources for one exact view. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Runtime.ViewContentItem> Inno.Rendering.Runtime.IViewContentCollector.Collect(Inno.Rendering.Runtime.ViewContentContext context)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L439) | Collects active source items in deterministic source order. |

### `Inno.Rendering.Runtime.IViewContentFrameSource`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.IViewContentFrameSource`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L387) | Completes frame-local input after every output has routed its views. |
| [`void Inno.Rendering.Runtime.IViewContentFrameSource.CompleteFrame(ulong frameIndex)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L396) | Advances retained content once after all output views have supplied input. |

### `Inno.Rendering.Runtime.IViewContentSink`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.IViewContentSink`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L351) | Receives world items without imposing a rendering-model sort key. |
| [`void Inno.Rendering.Runtime.IViewContentSink.Submit(Inno.Rendering.Runtime.ViewContentItem item)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L360) | Adds an item to the current view. |

### `Inno.Rendering.Runtime.IViewContentSource`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.IViewContentSource`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L366) | Contributes model-independent world items to selected views. |
| [`void Inno.Rendering.Runtime.IViewContentSource.Collect(Inno.Rendering.Runtime.ViewContentContext context, Inno.Rendering.Runtime.IViewContentSink sink)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L378) | Collects items for one exact view. |

### `Inno.Rendering.Runtime.IViewDrawable`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.IViewDrawable`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L170) | Prepares one model-independent drawable for a specific render pass. |
| [`bool Inno.Rendering.Runtime.IViewDrawable.TryPrepare(Inno.Rendering.Runtime.RenderPipelineContext context, Inno.Rendering.Runtime.RenderView view, out Inno.Rendering.Runtime.IPreparedViewDrawable? prepared)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L188) | Resolves reusable resources before render graph execution. |

### `Inno.Rendering.Runtime.IViewPointerTarget`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.IViewPointerTarget`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L213) | Receives pointer input after the rendering model resolves visible draw order. |
| [`bool Inno.Rendering.Runtime.IViewPointerTarget.TryHit(Inno.Rendering.Runtime.RenderView view, Inno.Rendering.Runtime.RenderOutputInput input, out Inno.Core.Mathematics.Vector2 localPosition)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L250) | Maps one output pointer onto this item's local surface. |
| [`bool Inno.Rendering.Runtime.IViewPointerTarget.hasKeyboardFocus`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L225) | Gets whether keyboard and text input should continue reaching this target. |
| [`bool Inno.Rendering.Runtime.IViewPointerTarget.hasPointerCapture`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L220) | Gets whether this target retains pointer ownership from an earlier press. Rendering models route captured input before ordinary hit testing. |
| [`void Inno.Rendering.Runtime.IViewPointerTarget.Advance(Inno.Rendering.Runtime.RenderOutputInput input, Inno.Core.Mathematics.Vector2 localPosition, ulong frameIndex)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L268) | Advances this target once for the frame with either routed input or an empty snapshot. |
| [`void Inno.Rendering.Runtime.IViewPointerTarget.SetKeyboardFocus(bool focused)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L233) | Changes keyboard focus after the rendering model resolves a pointer press. |

### `Inno.Rendering.Runtime.RenderExtensionStateContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.RenderExtensionStateContext`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderExtensionStateContext.cs#L12) | Restores neutral render settings through the canonical asset's actual owner and pinned references. |
| [`Inno.Rendering.Runtime.RenderExtensionStateContext.RenderExtensionStateContext(Inno.Assets.AssetObject owner)`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderExtensionStateContext.cs#L26) | Binds restoration to a canonical asset, independently of the caller's ambient session. |
| [`void Inno.Rendering.Runtime.RenderExtensionStateContext.Restore<TSettings>(Inno.Rendering.Assets.SerializedRenderExtensionState state, TSettings target)`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderExtensionStateContext.cs#L40) | Restores a matching settings contract with this owner's asset references. |

### `Inno.Rendering.Runtime.RenderFeatureContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.RenderFeatureConfiguration Inno.Rendering.Runtime.RenderFeatureContext.configuration`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderFeatureContext.cs#L42) | Gets stable feature configuration. |
| [`Inno.Rendering.GraphicsCapabilities Inno.Rendering.Runtime.RenderFeatureContext.capabilities`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderFeatureContext.cs#L57) | Gets current device capabilities. |
| [`Inno.Rendering.IRenderFrameUploadService Inno.Rendering.Runtime.RenderFeatureContext.uploads`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderFeatureContext.cs#L67) | Gets the frame-scoped streaming buffer service. |
| [`Inno.Rendering.RenderGraphBuilder Inno.Rendering.Runtime.RenderFeatureContext.graph`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderFeatureContext.cs#L47) | Gets the current frame graph builder. |
| [`Inno.Rendering.Runtime.IRenderResourceService Inno.Rendering.Runtime.RenderFeatureContext.resourceService`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderFeatureContext.cs#L62) | Gets the generation-aware neutral GPU resource service. |
| [`Inno.Rendering.Runtime.RenderFeatureContext`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderFeatureContext.cs#L14) | Supplies one configured feature with frame-scoped graph services. |
| [`Inno.Rendering.Runtime.RenderFeatureContext.RenderFeatureContext(Inno.Rendering.Runtime.RenderPipelineContext pipeline, Inno.Rendering.Assets.RenderFeatureConfiguration configuration)`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderFeatureContext.cs#L26) | Creates a feature build context. |
| [`Inno.Rendering.Runtime.RenderPipelineContext Inno.Rendering.Runtime.RenderFeatureContext.pipeline`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderFeatureContext.cs#L37) | Gets the owning pipeline context. |
| [`Inno.Rendering.Runtime.RenderResourceMap Inno.Rendering.Runtime.RenderFeatureContext.resources`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderFeatureContext.cs#L52) | Gets open semantic resources for the request. |

### `Inno.Rendering.Runtime.RenderFeatureExtensionAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.RenderFeatureExtensionAttribute`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderFeatureExtensionAttribute.cs#L14) | Marks a reloadable pipeline feature implementation with a stable extension identifier. |
| [`Inno.Rendering.Runtime.RenderFeatureExtensionAttribute.RenderFeatureExtensionAttribute(string id)`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderFeatureExtensionAttribute.cs#L24) | Creates a feature extension declaration. |
| [`string Inno.Rendering.Runtime.RenderFeatureExtensionAttribute.id`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderFeatureExtensionAttribute.cs#L33) | Gets the globally stable feature extension identifier. |

### `Inno.Rendering.Runtime.RenderFrameData`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.RenderFrameData`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderFrameData.cs#L15) | Carries generation-scoped, frame-only payloads between a request producer and its pipeline. |
| [`bool Inno.Rendering.Runtime.RenderFrameData.TryGet<TValue>(Inno.Rendering.RenderDataChannelId channel, out TValue? value)`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderFrameData.cs#L70) | Tries to read one typed value from an open data channel. |
| [`int Inno.Rendering.Runtime.RenderFrameData.count`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderFrameData.cs#L24) | Gets the number of populated channel and value-type pairs. |
| [`void Inno.Rendering.Runtime.RenderFrameData.Clear()`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderFrameData.cs#L92) | Removes all values before this object enters a submitted request. |
| [`void Inno.Rendering.Runtime.RenderFrameData.Set<TValue>(Inno.Rendering.RenderDataChannelId channel, TValue value)`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderFrameData.cs#L41) | Adds or replaces one typed value in an open data channel. |

### `Inno.Rendering.Runtime.RenderFrameStatistics`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderDeviceAllocationCounters? Inno.Rendering.Runtime.RenderFrameStatistics.allocationCounters`](../../src/services/rendering/Inno.Rendering.Runtime/RenderFrameStatistics.cs#L88) | Gets the device-generation cumulative transient allocation snapshot at frame completion. Null means the backend does not report allocation accounting; it does not mean zero allocations. |
| [`Inno.Rendering.Runtime.RenderFrameStatistics`](../../src/services/rendering/Inno.Rendering.Runtime/RenderFrameStatistics.cs#L9) | Reports read-only statistics for the most recently completed render frame. |
| [`Inno.Rendering.Runtime.RenderFrameStatistics.RenderFrameStatistics(ulong frameIndex, int viewCount, int drawCount, int dispatchCount, int culledPassCount, Inno.Rendering.RenderDeviceAllocationCounters? allocationCounters, int graphCompileCount)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderFrameStatistics.cs#L36) | Creates an immutable frame statistics snapshot. |
| [`int Inno.Rendering.Runtime.RenderFrameStatistics.culledPassCount`](../../src/services/rendering/Inno.Rendering.Runtime/RenderFrameStatistics.cs#L82) | Gets passes removed by graph compilation. |
| [`int Inno.Rendering.Runtime.RenderFrameStatistics.dispatchCount`](../../src/services/rendering/Inno.Rendering.Runtime/RenderFrameStatistics.cs#L77) | Gets the recorded compute dispatch count. |
| [`int Inno.Rendering.Runtime.RenderFrameStatistics.drawCount`](../../src/services/rendering/Inno.Rendering.Runtime/RenderFrameStatistics.cs#L72) | Gets the recorded draw count. |
| [`int Inno.Rendering.Runtime.RenderFrameStatistics.graphCompileCount`](../../src/services/rendering/Inno.Rendering.Runtime/RenderFrameStatistics.cs#L93) | Gets complete graph compilation attempts for the frame. Validation does not increase this count. |
| [`int Inno.Rendering.Runtime.RenderFrameStatistics.viewCount`](../../src/services/rendering/Inno.Rendering.Runtime/RenderFrameStatistics.cs#L67) | Gets the executed logical view count. |
| [`ulong Inno.Rendering.Runtime.RenderFrameStatistics.frameIndex`](../../src/services/rendering/Inno.Rendering.Runtime/RenderFrameStatistics.cs#L62) | Gets the monotonic render frame index. |

### `Inno.Rendering.Runtime.RenderGeometry`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.PersistentBufferHandle Inno.Rendering.Runtime.RenderGeometry.indexBuffer`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderGeometry.cs#L44) | Gets the persistent index buffer. |
| [`Inno.Rendering.PersistentBufferHandle Inno.Rendering.Runtime.RenderGeometry.vertexBuffer`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderGeometry.cs#L39) | Gets the persistent vertex buffer. |
| [`Inno.Rendering.RenderVertexLayout Inno.Rendering.Runtime.RenderGeometry.vertexLayout`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderGeometry.cs#L49) | Gets the imported interleaved vertex layout. |
| [`Inno.Rendering.Runtime.RenderGeometry`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderGeometry.cs#L15) | Provides generation-scoped GPU buffers for an imported geometry helper asset. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Runtime.RenderGeometrySection> Inno.Rendering.Runtime.RenderGeometry.sections`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderGeometry.cs#L64) | Gets independently drawable indexed ranges. |
| [`int Inno.Rendering.Runtime.RenderGeometry.indexCount`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderGeometry.cs#L59) | Gets the total index count. |
| [`int Inno.Rendering.Runtime.RenderGeometry.vertexCount`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderGeometry.cs#L54) | Gets the number of vertices. |
| [`void Inno.Rendering.Runtime.RenderGeometry.Bind(Inno.Rendering.RenderCommandEncoder commands)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderGeometry.cs#L72) | Binds both geometry streams at their first element. |
| [`void Inno.Rendering.Runtime.RenderGeometry.DrawSection(Inno.Rendering.RenderCommandEncoder commands, int sectionIndex, int instanceCount = 1)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderGeometry.cs#L91) | Binds and draws one indexed section. |

### `Inno.Rendering.Runtime.RenderGeometrySection`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.RenderGeometrySection`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderGeometrySection.cs#L15) | Describes one indexed range in resolved backend-neutral geometry. |
| [`Inno.Rendering.Runtime.RenderGeometrySection.RenderGeometrySection(int firstIndex, int indexCount)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderGeometrySection.cs#L27) | Creates one indexed geometry range. |
| [`int Inno.Rendering.Runtime.RenderGeometrySection.firstIndex`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderGeometrySection.cs#L40) | Gets the first index in the shared index buffer. |
| [`int Inno.Rendering.Runtime.RenderGeometrySection.indexCount`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderGeometrySection.cs#L45) | Gets the number of indices in this range. |

### `Inno.Rendering.Runtime.RenderMaterialPass`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.ShaderPassDefinition Inno.Rendering.Runtime.RenderMaterialPass.definition`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderMaterialPass.cs#L71) | Gets the selected provider-defined shader pass with detached metadata storage. |
| [`Inno.Rendering.ComputePipelineHandle Inno.Rendering.Runtime.RenderMaterialPass.computePipeline`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderMaterialPass.cs#L81) | Gets the compute pipeline, or an invalid handle for a raster pass. |
| [`Inno.Rendering.GraphicsPipelineHandle Inno.Rendering.Runtime.RenderMaterialPass.graphicsPipeline`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderMaterialPass.cs#L76) | Gets the graphics pipeline, or an invalid handle for a compute pass. |
| [`Inno.Rendering.Runtime.RenderMaterialPass`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderMaterialPass.cs#L35) | Represents one frame-resolved material pass and its material-owned bindings. |
| [`bool Inno.Rendering.Runtime.RenderMaterialPass.UsesBinding(Inno.Rendering.RenderBindingId binding, Inno.Rendering.RenderShaderBindingKind kind)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderMaterialPass.cs#L109) | Gets whether the compiled program actively consumes one declared binding. |
| [`bool Inno.Rendering.Runtime.RenderMaterialPass.isCompute`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderMaterialPass.cs#L91) | Gets whether this is a compute material pass. |
| [`bool Inno.Rendering.Runtime.RenderMaterialPass.isGraphics`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderMaterialPass.cs#L86) | Gets whether this is a graphics material pass. |
| [`void Inno.Rendering.Runtime.RenderMaterialPass.Bind(Inno.Rendering.RenderCommandEncoder commands)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderMaterialPass.cs#L132) | Binds the program and all material-owned values and textures. |

### `Inno.Rendering.Runtime.RenderModelExtensionAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.RenderModelExtensionAttribute`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderModelExtensionAttribute.cs#L14) | Marks a reloadable rendering model with a stable identity. |
| [`Inno.Rendering.Runtime.RenderModelExtensionAttribute.RenderModelExtensionAttribute(string id)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderModelExtensionAttribute.cs#L24) | Creates a model declaration. |
| [`string Inno.Rendering.Runtime.RenderModelExtensionAttribute.id`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderModelExtensionAttribute.cs#L33) | Gets the globally stable model identity. |

### `Inno.Rendering.Runtime.RenderModelOutput`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.RenderPipelineAsset Inno.Rendering.Runtime.RenderModelOutput.pipeline`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderModelOutput.cs#L52) | Gets the model's pipeline. |
| [`Inno.Rendering.RenderTextureFormat Inno.Rendering.Runtime.RenderModelOutput.targetFormat`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderModelOutput.cs#L60) | Gets required target format. |
| [`Inno.Rendering.Runtime.RenderFrameData Inno.Rendering.Runtime.RenderModelOutput.data`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderModelOutput.cs#L56) | Gets model frame data. |
| [`Inno.Rendering.Runtime.RenderModelOutput`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderModelOutput.cs#L14) | Gets one model's prepared pipeline data for a host-owned target. |
| [`Inno.Rendering.Runtime.RenderModelOutput.RenderModelOutput(string name, Inno.Rendering.Assets.RenderPipelineAsset pipeline, Inno.Rendering.Runtime.RenderFrameData data, Inno.Rendering.RenderTextureFormat targetFormat = Inno.Rendering.RenderTextureFormat.RGBA8Srgb)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderModelOutput.cs#L32) | Creates prepared model output. |
| [`string Inno.Rendering.Runtime.RenderModelOutput.name`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderModelOutput.cs#L48) | Gets the diagnostic name. |

### `Inno.Rendering.Runtime.RenderOutputInput`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Input.KeyModifier Inno.Rendering.Runtime.RenderOutputInput.modifiers`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputInput.cs#L99) | Gets active keyboard modifiers. |
| [`Inno.Core.Mathematics.Vector2 Inno.Rendering.Runtime.RenderOutputInput.pointerPosition`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputInput.cs#L87) | Gets viewport-local pointer coordinates. |
| [`Inno.Core.Mathematics.Vector2 Inno.Rendering.Runtime.RenderOutputInput.scrollDelta`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputInput.cs#L95) | Gets wheel movement. |
| [`Inno.Rendering.Runtime.RenderOutputInput`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputInput.cs#L12) | Frame-local input located in the output viewport's physical pixels. |
| [`Inno.Rendering.Runtime.RenderOutputInput.RenderOutputInput(Inno.Core.Mathematics.Vector2 pointerPosition, bool pointerInside, Inno.Core.Mathematics.Vector2 scrollDelta, Inno.Core.Input.KeyModifier modifiers, System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.KeyCode> keysPressed, System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.KeyCode> keysReleased, System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.MouseButton> buttonsPressed, System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.MouseButton> buttonsReleased, System.Collections.Generic.IReadOnlyList<string> textInput, bool interactionEnabled = true)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputInput.cs#L48) | Creates an input snapshot for one output viewport. |
| [`System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.KeyCode> Inno.Rendering.Runtime.RenderOutputInput.keysPressed`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputInput.cs#L103) | Gets keys pressed this frame. |
| [`System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.KeyCode> Inno.Rendering.Runtime.RenderOutputInput.keysReleased`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputInput.cs#L107) | Gets keys released this frame. |
| [`System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.MouseButton> Inno.Rendering.Runtime.RenderOutputInput.buttonsPressed`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputInput.cs#L111) | Gets buttons pressed this frame. |
| [`System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.MouseButton> Inno.Rendering.Runtime.RenderOutputInput.buttonsReleased`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputInput.cs#L115) | Gets buttons released this frame. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Rendering.Runtime.RenderOutputInput.textInput`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputInput.cs#L119) | Gets ordered text commits. |
| [`bool Inno.Rendering.Runtime.RenderOutputInput.interactionEnabled`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputInput.cs#L124) | Gets whether game pointer and keyboard interaction is enabled. |
| [`bool Inno.Rendering.Runtime.RenderOutputInput.pointerInside`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputInput.cs#L91) | Gets whether the pointer is inside this output. |
| [`static Inno.Rendering.Runtime.RenderOutputInput Inno.Rendering.Runtime.RenderOutputInput.empty`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputInput.cs#L75) | Gets a snapshot with no active input. |
| [`static Inno.Rendering.Runtime.RenderOutputInput Inno.Rendering.Runtime.RenderOutputInput.suspended`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputInput.cs#L81) | Gets a snapshot that clears gameplay interaction while retaining the rendered output. |

### `Inno.Rendering.Runtime.RenderOutputLayer`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.RenderOutputLayer`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputLayer.cs#L14) | Assigns exactly one rendering model and its world-content sources to an output layer. |
| [`Inno.Rendering.Runtime.RenderOutputLayer.RenderOutputLayer(string modelId, System.Collections.Generic.IEnumerable<string> sourceIds)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputLayer.cs#L26) | Creates a model layer with explicit, distinct content-source IDs. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Rendering.Runtime.RenderOutputLayer.sourceIds`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputLayer.cs#L47) | Gets the assigned world-content source IDs. |
| [`string Inno.Rendering.Runtime.RenderOutputLayer.modelId`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputLayer.cs#L43) | Gets the exact rendering model ID. |

### `Inno.Rendering.Runtime.RenderOutputRoute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.RenderOutputRoute`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputRoute.cs#L14) | States the exact model order and exclusive world-content assignment for one output. |
| [`Inno.Rendering.Runtime.RenderOutputRoute.RenderOutputRoute(System.Collections.Generic.IEnumerable<Inno.Rendering.Runtime.RenderOutputLayer> layers)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputRoute.cs#L23) | Creates a route from explicitly assigned model layers in draw order. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Runtime.RenderOutputLayer> Inno.Rendering.Runtime.RenderOutputRoute.layers`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputRoute.cs#L39) | Gets exact model and source assignments in draw order. |

### `Inno.Rendering.Runtime.RenderOutputSession`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.References.ContentReadScope Inno.Rendering.Runtime.RenderOutputSession.content`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputSession.cs#L72) | Gets selected content roots. |
| [`Inno.Rendering.RenderViewport Inno.Rendering.Runtime.RenderOutputSession.viewport`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputSession.cs#L76) | Gets the output pixel viewport. |
| [`Inno.Rendering.Runtime.IViewContentCollector Inno.Rendering.Runtime.RenderOutputSession.viewContent`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputSession.cs#L88) | Gets the world content collector. |
| [`Inno.Rendering.Runtime.RenderOutputInput Inno.Rendering.Runtime.RenderOutputSession.input`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputSession.cs#L92) | Gets viewport-local input. |
| [`Inno.Rendering.Runtime.RenderOutputRoute? Inno.Rendering.Runtime.RenderOutputSession.route`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputSession.cs#L96) | Gets the explicit model route, if configured. |
| [`Inno.Rendering.Runtime.RenderOutputSession`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputSession.cs#L14) | Declares a host output without naming a camera or scene model. |
| [`Inno.Rendering.Runtime.RenderOutputSession Inno.Rendering.Runtime.RenderOutputSession.ForLayer(Inno.Rendering.Runtime.RenderOutputLayer layer)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputSession.cs#L107) | Creates the session seen by one explicitly assigned model layer. |
| [`Inno.Rendering.Runtime.RenderOutputSession.RenderOutputSession(string id, Inno.References.ContentReadScope content, Inno.Rendering.RenderViewport viewport, ulong frameIndex, float deltaTime, Inno.Rendering.Runtime.IViewContentCollector viewContent, Inno.Rendering.Runtime.RenderOutputInput? input = null, Inno.Rendering.Runtime.RenderOutputRoute? route = null)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputSession.cs#L44) | Creates a frame-scoped output session. |
| [`float Inno.Rendering.Runtime.RenderOutputSession.deltaTime`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputSession.cs#L84) | Gets elapsed frame time. |
| [`string Inno.Rendering.Runtime.RenderOutputSession.id`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputSession.cs#L68) | Gets the stable host output identity. |
| [`ulong Inno.Rendering.Runtime.RenderOutputSession.frameIndex`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputSession.cs#L80) | Gets the shared output frame index. |

### `Inno.Rendering.Runtime.RenderPersistentResourceId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.RenderPersistentResourceId`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderPersistentResourceId.cs#L15) | Identifies one provider-owned persistent GPU resource without exposing a native handle. |
| [`Inno.Rendering.Runtime.RenderPersistentResourceId.RenderPersistentResourceId(string value)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderPersistentResourceId.cs#L24) | Creates a globally stable persistent resource identifier. |
| [`bool Inno.Rendering.Runtime.RenderPersistentResourceId.isValid`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderPersistentResourceId.cs#L38) | Gets whether the identifier contains a usable value. |
| [`override string Inno.Rendering.Runtime.RenderPersistentResourceId.ToString()`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderPersistentResourceId.cs#L46) | Formats this value as a human-readable representation. |
| [`string Inno.Rendering.Runtime.RenderPersistentResourceId.value`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderPersistentResourceId.cs#L33) | Gets the provider-qualified stable identifier. |

### `Inno.Rendering.Runtime.RenderPipeline`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.RenderPipeline`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipeline.cs#L14) | Builds frame-local passes without prescribing a rendering model. |
| [`abstract void Inno.Rendering.Runtime.RenderPipeline.Build(Inno.Rendering.Runtime.RenderPipelineContext context)`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipeline.cs#L42) | Builds all passes for one request. |
| [`virtual void Inno.Rendering.Runtime.RenderPipeline.Dispose(bool disposing)`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipeline.cs#L94) | Releases managed generation-scoped state. |
| [`virtual void Inno.Rendering.Runtime.RenderPipeline.OnConfigure(Inno.Rendering.Assets.SerializedRenderExtensionState state, Inno.Rendering.Runtime.RenderExtensionStateContext settings)`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipeline.cs#L80) | Reads pipeline-owned settings from neutral state. |
| [`void Inno.Rendering.Runtime.RenderPipeline.Configure(Inno.Rendering.Assets.SerializedRenderExtensionState state, Inno.Rendering.Runtime.RenderExtensionStateContext settings)`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipeline.cs#L28) | Applies reload-safe pipeline settings to this generation. |
| [`void Inno.Rendering.Runtime.RenderPipeline.Dispose()`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipeline.cs#L50) | Releases generation-scoped pipeline state. |

### `Inno.Rendering.Runtime.RenderPipelineContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Diagnostics.IDiagnosticReporter Inno.Rendering.Runtime.RenderPipelineContext.diagnostics`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineContext.cs#L115) | Gets the structured diagnostic sink. |
| [`Inno.Rendering.Assets.RenderPipelineAsset Inno.Rendering.Runtime.RenderPipelineContext.pipelineAsset`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineContext.cs#L95) | Gets selected pipeline configuration. |
| [`Inno.Rendering.GraphicsCapabilities Inno.Rendering.Runtime.RenderPipelineContext.capabilities`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineContext.cs#L105) | Gets current device capabilities. |
| [`Inno.Rendering.IRenderFrameUploadService Inno.Rendering.Runtime.RenderPipelineContext.uploads`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineContext.cs#L125) | Gets the frame-scoped streaming buffer service. |
| [`Inno.Rendering.RenderGraphBuilder Inno.Rendering.Runtime.RenderPipelineContext.graph`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineContext.cs#L100) | Gets the current frame graph builder. |
| [`Inno.Rendering.RenderTextureHandle Inno.Rendering.Runtime.RenderPipelineContext.outputTexture`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineContext.cs#L143) | Gets the imported offscreen target, or an invalid handle for the backbuffer. |
| [`Inno.Rendering.Runtime.IRenderResourceService Inno.Rendering.Runtime.RenderPipelineContext.resourceService`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineContext.cs#L120) | Gets the generation-aware neutral GPU resource service. |
| [`Inno.Rendering.Runtime.RenderPipelineContext`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineContext.cs#L14) | Supplies one request and frame-scoped services to a render pipeline. |
| [`Inno.Rendering.Runtime.RenderPipelineContext.RenderPipelineContext(Inno.Rendering.Runtime.RenderRequest request, Inno.Rendering.Assets.RenderPipelineAsset pipelineAsset, Inno.Rendering.RenderGraphBuilder graph, Inno.Rendering.GraphicsCapabilities capabilities, Inno.Rendering.Runtime.RenderResourceMap resources, Inno.Core.Diagnostics.IDiagnosticReporter diagnostics, Inno.Rendering.Runtime.IRenderResourceService resourceService, Inno.Rendering.IRenderFrameUploadService uploads, ulong frameIndex, bool preservePresentationTarget, Inno.Rendering.RenderTextureHandle outputTexture = default(Inno.Rendering.RenderTextureHandle))`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineContext.cs#L53) | Creates a pipeline build context. |
| [`Inno.Rendering.Runtime.RenderRequest Inno.Rendering.Runtime.RenderPipelineContext.request`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineContext.cs#L90) | Gets the current request. |
| [`Inno.Rendering.Runtime.RenderResourceMap Inno.Rendering.Runtime.RenderPipelineContext.resources`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineContext.cs#L110) | Gets open semantic resources for this request. |
| [`bool Inno.Rendering.Runtime.RenderPipelineContext.preservePresentationTarget`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineContext.cs#L138) | Gets whether this model contribution must load and preserve an earlier contribution to the same target. |
| [`ulong Inno.Rendering.Runtime.RenderPipelineContext.frameIndex`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineContext.cs#L130) | Gets the monotonic render frame index. |

### `Inno.Rendering.Runtime.RenderPipelineExtensionAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.RenderPipelineExtensionAttribute`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineExtensionAttribute.cs#L14) | Marks a reloadable render pipeline implementation with a stable extension identifier. |
| [`Inno.Rendering.Runtime.RenderPipelineExtensionAttribute.RenderPipelineExtensionAttribute(string id)`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineExtensionAttribute.cs#L24) | Creates a pipeline extension declaration. |
| [`string Inno.Rendering.Runtime.RenderPipelineExtensionAttribute.id`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineExtensionAttribute.cs#L33) | Gets the globally stable pipeline extension identifier. |

### `Inno.Rendering.Runtime.RenderPipelineFeature`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.RenderPipelineFeature`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineFeature.cs#L14) | Adds capability-aware passes without owning frame graph state. |
| [`abstract void Inno.Rendering.Runtime.RenderPipelineFeature.AddRenderPasses(Inno.Rendering.Runtime.RenderFeatureContext context)`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineFeature.cs#L40) | Adds frame-scoped passes and dependencies. |
| [`virtual void Inno.Rendering.Runtime.RenderPipelineFeature.OnConfigure(Inno.Rendering.Assets.SerializedRenderExtensionState state, Inno.Rendering.Runtime.RenderExtensionStateContext settings)`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineFeature.cs#L51) | Reads feature-owned settings from neutral state. |
| [`void Inno.Rendering.Runtime.RenderPipelineFeature.Configure(Inno.Rendering.Assets.RenderFeatureConfiguration configuration, Inno.Rendering.Runtime.RenderExtensionStateContext settings)`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineFeature.cs#L26) | Applies reload-safe settings to this feature generation. |

### `Inno.Rendering.Runtime.RenderRequest`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.RenderPipelineAsset? Inno.Rendering.Runtime.RenderRequest.pipeline`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequest.cs#L70) | Gets the per-request pipeline asset, or null to use the project default. |
| [`Inno.Rendering.RenderTarget Inno.Rendering.Runtime.RenderRequest.target`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequest.cs#L60) | Gets the render destination. |
| [`Inno.Rendering.RenderViewport Inno.Rendering.Runtime.RenderRequest.viewport`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequest.cs#L65) | Gets the destination pixel viewport. |
| [`Inno.Rendering.Runtime.RenderFrameData Inno.Rendering.Runtime.RenderRequest.data`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequest.cs#L75) | Gets immutable pipeline-defined frame data. |
| [`Inno.Rendering.Runtime.RenderRequest`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequest.cs#L11) | Requests one pipeline-defined rendering operation without prescribing world semantics. |
| [`Inno.Rendering.Runtime.RenderRequest.RenderRequest(string name, Inno.Rendering.RenderTarget target, Inno.Rendering.RenderViewport viewport, Inno.Rendering.Assets.RenderPipelineAsset? pipeline = null, Inno.Rendering.Runtime.RenderFrameData? data = null, int priority = 0)`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequest.cs#L35) | Creates an immutable render request. |
| [`int Inno.Rendering.Runtime.RenderRequest.priority`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequest.cs#L80) | Gets the ascending frame scheduling priority. |
| [`string Inno.Rendering.Runtime.RenderRequest.name`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequest.cs#L55) | Gets the frame-local diagnostic name. |

### `Inno.Rendering.Runtime.RenderRequestProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.RenderRequestProvider`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L161) | Produces arbitrary render requests without prescribing a scene or rendering model. |
| [`abstract void Inno.Rendering.Runtime.RenderRequestProvider.Submit(Inno.Rendering.Runtime.RenderRequestProviderContext context)`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L172) | Submits zero or more requests for the current frame. |
| [`virtual void Inno.Rendering.Runtime.RenderRequestProvider.Dispose(bool disposing)`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L210) | Releases managed generation-scoped state. |
| [`void Inno.Rendering.Runtime.RenderRequestProvider.Dispose()`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L180) | Releases generation-scoped provider state. |

### `Inno.Rendering.Runtime.RenderRequestProviderContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.References.ContentReadScope Inno.Rendering.Runtime.RenderRequestProviderContext.content`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L121) | Gets the explicit ordered host content visible to request providers this frame. |
| [`Inno.Rendering.GraphicsCapabilities Inno.Rendering.Runtime.RenderRequestProviderContext.capabilities`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L126) | Gets the active backend-neutral capability snapshot. |
| [`Inno.Rendering.RenderPresentationSize? Inno.Rendering.Runtime.RenderRequestProviderContext.primaryPresentationSize`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L131) | Gets the current primary pixel extent, or null while only offscreen outputs are available. |
| [`Inno.Rendering.RenderViewport? Inno.Rendering.Runtime.RenderRequestProviderContext.primaryPresentationViewport`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L136) | Gets the host-selected primary region, or null while no primary output is available. |
| [`Inno.Rendering.Runtime.IRenderRequestSink Inno.Rendering.Runtime.RenderRequestProviderContext.requests`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L116) | Gets the sink accepting requests for the current frame. |
| [`Inno.Rendering.Runtime.IViewContentCollector Inno.Rendering.Runtime.RenderRequestProviderContext.viewContent`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L151) | Gets the active generation's world-content collector. |
| [`Inno.Rendering.Runtime.RenderOutputInput Inno.Rendering.Runtime.RenderRequestProviderContext.input`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L155) | Gets viewport-local input for the primary output. |
| [`Inno.Rendering.Runtime.RenderRequestProviderContext`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L47) | Supplies frame timing, capabilities and the request sink to one provider invocation. |
| [`Inno.Rendering.Runtime.RenderRequestProviderContext.RenderRequestProviderContext(Inno.Rendering.Runtime.IRenderRequestSink requests, Inno.References.ContentReadScope content, Inno.Rendering.GraphicsCapabilities capabilities, Inno.Rendering.RenderPresentationSize? primaryPresentationSize, Inno.Rendering.RenderViewport? primaryPresentationViewport, ulong frameIndex, float deltaTime, Inno.Rendering.Runtime.IViewContentCollector viewContent, Inno.Rendering.Runtime.RenderOutputInput? input = null)`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L80) | Creates a frame-scoped provider context. |
| [`float Inno.Rendering.Runtime.RenderRequestProviderContext.deltaTime`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L146) | Gets the elapsed frame time in seconds. |
| [`ulong Inno.Rendering.Runtime.RenderRequestProviderContext.frameIndex`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L141) | Gets the monotonic render frame index. |

### `Inno.Rendering.Runtime.RenderRequestProviderExtensionAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.RenderRequestProviderExtensionAttribute`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L11) | Marks a reloadable provider that produces model-neutral render requests each frame. |
| [`Inno.Rendering.Runtime.RenderRequestProviderExtensionAttribute.RenderRequestProviderExtensionAttribute(string id, int priority = 0)`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L24) | Creates a render request provider declaration. |
| [`int Inno.Rendering.Runtime.RenderRequestProviderExtensionAttribute.priority`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L41) | Gets the provider invocation priority. |
| [`string Inno.Rendering.Runtime.RenderRequestProviderExtensionAttribute.id`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L36) | Gets the globally stable provider identifier. |

### `Inno.Rendering.Runtime.RenderResourceLimits`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.RenderResourceLimits`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceLimits.cs#L8) | Sets finite native resource and asynchronous readback admission limits for one rendering owner. |
| [`int Inno.Rendering.Runtime.RenderResourceLimits.pendingReadbacks`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceLimits.cs#L18) | Gets the maximum readbacks that can retain GPU resources before owner-thread completion. |
| [`int Inno.Rendering.Runtime.RenderResourceLimits.resourcesPerKind`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceLimits.cs#L13) | Gets the maximum active entries in each buffer, texture or pipeline cache. |
| [`int Inno.Rendering.Runtime.RenderResourceLimits.targets`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceLimits.cs#L35) | Gets the offscreen target count and per-frame target allocation capacity. |
| [`int Inno.Rendering.Runtime.RenderResourceLimits.uploadPages`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceLimits.cs#L23) | Gets the maximum resident frame-upload pages. |
| [`long Inno.Rendering.Runtime.RenderResourceLimits.uploadBytesPerFrame`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceLimits.cs#L31) | Gets the maximum bytes uploaded during one frame. |
| [`long Inno.Rendering.Runtime.RenderResourceLimits.uploadResidentBytes`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceLimits.cs#L27) | Gets the maximum resident frame-upload bytes, including retired pages. |

### `Inno.Rendering.Runtime.RenderResourceMap`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.RenderResourceMap`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderResourceMap.cs#L14) | Stores open semantic graph resources for one pipeline request. |
| [`bool Inno.Rendering.Runtime.RenderResourceMap.TryGetBuffer(Inno.Rendering.RenderResourceId id, out Inno.Rendering.RenderBufferHandle buffer)`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderResourceMap.cs#L89) | Tries to get a published buffer. |
| [`bool Inno.Rendering.Runtime.RenderResourceMap.TryGetTexture(Inno.Rendering.RenderResourceId id, out Inno.Rendering.RenderTextureHandle texture)`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderResourceMap.cs#L72) | Tries to get a published texture. |
| [`void Inno.Rendering.Runtime.RenderResourceMap.PublishBuffer(Inno.Rendering.RenderResourceId id, Inno.Rendering.RenderBufferHandle buffer)`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderResourceMap.cs#L49) | Publishes a buffer under a pipeline-defined semantic identifier. |
| [`void Inno.Rendering.Runtime.RenderResourceMap.PublishTexture(Inno.Rendering.RenderResourceId id, Inno.Rendering.RenderTextureHandle texture)`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderResourceMap.cs#L29) | Publishes a texture under a pipeline-defined semantic identifier. |

### `Inno.Rendering.Runtime.RenderResourceProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.RenderResourceProvider`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderResourceProvider.cs#L16) | Provides protected construction operations for replaceable render resource-service implementations. |
| [`static Inno.Rendering.Runtime.RenderGeometry Inno.Rendering.Runtime.RenderResourceProvider.CreateGeometry(Inno.Rendering.PersistentBufferHandle vertexBuffer, Inno.Rendering.PersistentBufferHandle indexBuffer, Inno.Rendering.RenderVertexLayout vertexLayout, int vertexCount, int indexCount, System.Collections.Generic.IReadOnlyList<Inno.Rendering.Runtime.RenderGeometrySection> sections)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderResourceProvider.cs#L163) | Creates generation-scoped resolved geometry from persistent buffers and immutable sections. |
| [`static Inno.Rendering.Runtime.RenderMaterialPass Inno.Rendering.Runtime.RenderResourceProvider.CreateMaterialPass(Inno.Rendering.Assets.ShaderPassDefinition definition, Inno.Rendering.GraphicsPipelineHandle graphicsPipeline, Inno.Rendering.ComputePipelineHandle computePipeline, System.Collections.Generic.IReadOnlyList<Inno.Rendering.Assets.ShaderPropertyDefinition> declaredBindings, Inno.Rendering.ShaderInterface activeInterface, System.Collections.Generic.IReadOnlyList<Inno.Rendering.Runtime.RenderResourceProvider.MaterialBinding> bindings)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderResourceProvider.cs#L123) | Creates a generation-scoped resolved material pass. |
| [`static Inno.Rendering.Runtime.RenderMaterialPass Inno.Rendering.Runtime.RenderResourceProvider.CreateMaterialPass(Inno.Rendering.Assets.ShaderPassDefinition definition, Inno.Rendering.GraphicsPipelineHandle graphicsPipeline, Inno.Rendering.ComputePipelineHandle computePipeline, System.Collections.Generic.IReadOnlyList<Inno.Rendering.Runtime.RenderResourceProvider.MaterialBinding> bindings)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderResourceProvider.cs#L91) | Creates a generation-scoped resolved material pass when no compiled-interface query is required. |
| [`static Inno.Rendering.Runtime.RenderResourceProvider.MaterialBinding Inno.Rendering.Runtime.RenderResourceProvider.CreateTextureBinding(Inno.Rendering.RenderBindingId id, Inno.Rendering.PersistentTextureHandle texture, Inno.Rendering.RenderSamplerState sampler)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderResourceProvider.cs#L65) | Creates a protected sampled-texture binding value. |
| [`static Inno.Rendering.Runtime.RenderResourceProvider.MaterialBinding Inno.Rendering.Runtime.RenderResourceProvider.CreateUniformBinding(Inno.Rendering.RenderBindingId id, System.ReadOnlySpan<byte> data)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderResourceProvider.cs#L44) | Creates a protected uniform binding value. |

### `Inno.Rendering.Runtime.RenderResourceProvider.MaterialBinding`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.RenderResourceProvider.MaterialBinding`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderResourceProvider.cs#L22) | Represents one material binding staged by a derived resource service. |

### `Inno.Rendering.Runtime.RenderResourceStatistics`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.RenderResourceStatistics`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceStatistics.cs#L6) | Reports control-thread resource occupancy and admission pressure without exposing backend objects. |
| [`int Inno.Rendering.Runtime.RenderResourceStatistics.activeResources`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceStatistics.cs#L11) | Gets active persistent buffers, textures, pipelines, material programs and geometry pairs. |
| [`int Inno.Rendering.Runtime.RenderResourceStatistics.peakReadbacks`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceStatistics.cs#L27) | Gets the largest simultaneous readback occupancy. |
| [`int Inno.Rendering.Runtime.RenderResourceStatistics.pendingReadbacks`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceStatistics.cs#L23) | Gets readbacks that still own native operations. |
| [`int Inno.Rendering.Runtime.RenderResourceStatistics.retiringResources`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceStatistics.cs#L15) | Gets resource entries whose native retirement is still pending. |
| [`int Inno.Rendering.Runtime.RenderResourceStatistics.targets`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceStatistics.cs#L55) | Gets active offscreen targets. |
| [`int Inno.Rendering.Runtime.RenderResourceStatistics.uploadPages`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceStatistics.cs#L35) | Gets retained upload pages, including pages awaiting retirement. |
| [`long Inno.Rendering.Runtime.RenderResourceStatistics.rejectedReadbacks`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceStatistics.cs#L31) | Gets readbacks rejected before native allocation. |
| [`long Inno.Rendering.Runtime.RenderResourceStatistics.rejectedResources`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceStatistics.cs#L19) | Gets cache admissions rejected by per-kind capacity. |
| [`long Inno.Rendering.Runtime.RenderResourceStatistics.rejectedTargets`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceStatistics.cs#L59) | Gets offscreen target requests rejected by finite capacity. |
| [`long Inno.Rendering.Runtime.RenderResourceStatistics.rejectedUploads`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceStatistics.cs#L51) | Gets upload requests rejected by resident or per-frame limits. |
| [`long Inno.Rendering.Runtime.RenderResourceStatistics.uploadPeakBytes`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceStatistics.cs#L43) | Gets the high-water mark of resident upload bytes. |
| [`long Inno.Rendering.Runtime.RenderResourceStatistics.uploadResidentBytes`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceStatistics.cs#L39) | Gets resident upload bytes, including pages awaiting retirement. |
| [`long Inno.Rendering.Runtime.RenderResourceStatistics.uploadedFrameBytes`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceStatistics.cs#L47) | Gets successfully uploaded bytes during the current frame. |

### `Inno.Rendering.Runtime.RenderRuntime`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.IRenderResourceService Inno.Rendering.Runtime.RenderRuntime.resources`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.cs#L161) | Gets backend-neutral persistent resource resolution for host-owned previews and rendering integrations. |
| [`Inno.Rendering.Runtime.IRenderRuntimeReloadTransaction Inno.Rendering.Runtime.RenderRuntime.BeginExtensionReload()`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.Pipelines.cs#L55) | Begins an isolated rendering-extension reload transaction at a frame boundary. |
| [`Inno.Rendering.Runtime.IViewContentCollector Inno.Rendering.Runtime.RenderRuntime.viewContent`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.cs#L166) | Gets the generation-scoped collector used by rendering models for world content. |
| [`Inno.Rendering.Runtime.RenderResourceStatistics Inno.Rendering.Runtime.RenderRuntime.resourceStatistics`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.cs#L220) | Gets a detached control-thread snapshot of resource occupancy, high-water marks and rejected admissions. |
| [`Inno.Rendering.Runtime.RenderRuntime`](../../src/services/rendering/Inno.Rendering.Runtime/RenderFrameScratch.cs#L7) | Owns the sole graphics frame boundary and executes model-neutral render requests. |
| [`Inno.Rendering.Runtime.RenderRuntime.RenderRuntime(Inno.Extensibility.Types.TypeCatalog types, Inno.Rendering.IRenderDevice device, Inno.Core.Diagnostics.IDiagnosticReporter diagnostics, System.Collections.Generic.IEnumerable<Inno.Rendering.IRenderFrameGraphContributor>? contributors = null, Inno.Rendering.Runtime.IRenderTargetArtifactProvider? targetArtifacts = null, System.Func<Inno.References.ContentReadScope>? contentScopeProvider = null, System.Func<Inno.Rendering.RenderPresentationSize, Inno.Rendering.RenderViewport>? primaryPresentationViewportProvider = null, Inno.Rendering.Runtime.RenderResourceLimits? resourceLimits = null, System.Func<Inno.Input.InputSnapshot>? inputSnapshotProvider = null, System.Func<Inno.Rendering.RenderPresentationSize?>? primaryInputSurfaceSizeProvider = null, Inno.Rendering.IRenderLayerCompositionProgramProvider? compositionProgramProvider = null)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.cs#L109) | Creates a render runtime without installing any concrete pipeline. |
| [`Inno.Rendering.Runtime.RenderTargetStore Inno.Rendering.Runtime.RenderRuntime.targets`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.cs#L156) | Gets persistent offscreen target services for viewport presentation. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Runtime.ViewContentItem> Inno.Rendering.Runtime.RenderRuntime.Collect(Inno.Rendering.Runtime.ViewContentContext context)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.cs#L182) | Collects frame requests from rendering models and registered providers. |
| [`System.IDisposable Inno.Rendering.Runtime.RenderRuntime.EnterExecutionScope()`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.cs#L239) | Binds this rendering runtime to script-facing graphics APIs for the current asynchronous execution flow. |
| [`bool Inno.Rendering.Runtime.RenderRuntime.TryActivateDefaultPipeline(Inno.Rendering.Assets.RenderPipelineAsset pipelineAsset)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.Pipelines.cs#L31) | Validates and selects a project default while preserving its last-good generation. |
| [`bool Inno.Rendering.Runtime.RenderRuntime.UnregisterContributor(Inno.Rendering.IRenderFrameGraphContributor contributor)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.Contributors.cs#L53) | Stops invoking a previously registered frame-final contributor. |
| [`override void Inno.Rendering.Runtime.RenderRuntime.OnBeginFrame(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.Frames.cs#L25) | Captures snapshots and binds service façades. |
| [`override void Inno.Rendering.Runtime.RenderRuntime.OnCompleteOutput(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.Frames.cs#L51) | Submits output and closes output-specific temporary resources. |
| [`override void Inno.Rendering.Runtime.RenderRuntime.OnPrepareOutput(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.Frames.cs#L37) | Opens resources required for this frame's output. |
| [`override void Inno.Rendering.Runtime.RenderRuntime.OnProduceOutput(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.Frames.cs#L44) | Collects output commands without executing managed code on a native realtime callback. |
| [`override void Inno.Rendering.Runtime.RenderRuntime.OnStart()`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.cs#L248) | Attaches this feature to its owning runtime generation. |
| [`override void Inno.Rendering.Runtime.RenderRuntime.OnStop()`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.cs#L265) | Releases every runtime rendering generation, persistent target, upload, and GPU resource owned by this instance. |
| [`uint Inno.Rendering.Runtime.RenderRuntime.deviceGeneration`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.cs#L215) | Gets the non-zero rendering-device generation that owns persistent handles. |
| [`ulong Inno.Rendering.Runtime.RenderRuntime.currentFrameIndex`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.cs#L171) | Gets the monotonic index of the current or most recently completed output frame. |
| [`void Inno.Rendering.Runtime.RenderRuntime.RegisterContributor(Inno.Rendering.IRenderFrameGraphContributor contributor)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.Contributors.cs#L31) | Registers frame-final work without transferring frame ownership. |
| [`void Inno.Rendering.Runtime.RenderRuntime.SetPrimaryModelOutputEnabled(bool enabled)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.Models.cs#L40) | Enables or disables model rendering to the host's primary backbuffer. Editor hosts disable this because their Game and Scene sessions own offscreen outputs. |
| [`void Inno.Rendering.Runtime.RenderRuntime.SetPrimaryRoute(Inno.Rendering.Runtime.RenderOutputRoute? route)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.Models.cs#L25) | Sets the explicit model composition route for the primary output at a frame boundary. |
| [`void Inno.Rendering.Runtime.RenderRuntime.Submit(Inno.Rendering.Runtime.RenderRequest request)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.Requests.cs#L32) | Submits validated work to the active backend for ordered processing. |
| [`void Inno.Rendering.Runtime.RenderRuntime.SubmitComposition(string name, Inno.Rendering.RenderTarget target, Inno.Rendering.RenderViewport viewport, Inno.Rendering.RenderTextureFormat format, System.Collections.Generic.IReadOnlyList<Inno.Rendering.Runtime.RenderRequest> layers, int priority = 0)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.Requests.cs#L72) | Submits one or more independently rendered model layers for premultiplied-alpha output composition. |

### `Inno.Rendering.Runtime.RenderRuntimeFactory`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.RenderRuntimeFactory`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntimeFactory.cs#L11) | Creates one rendering lifecycle feature for every isolated runtime session. |
| [`Inno.Rendering.Runtime.RenderRuntimeFactory.RenderRuntimeFactory(System.Func<Inno.Runtime.Contracts.RuntimeSubsystemContext, Inno.Rendering.Runtime.RenderRuntime> runtimeFactory)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntimeFactory.cs#L21) | Creates a reusable factory around a composition-owned rendering runtime callback. |
| [`Inno.Runtime.Contracts.IRuntimeSubsystem Inno.Rendering.Runtime.RenderRuntimeFactory.Create(Inno.Runtime.Contracts.RuntimeSubsystemContext context)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntimeFactory.cs#L43) | Creates a rendering feature over a newly allocated runtime layer. |
| [`Inno.Runtime.Contracts.RuntimeSubsystemDescriptor Inno.Rendering.Runtime.RenderRuntimeFactory.descriptor`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntimeFactory.cs#L29) | Gets stable ordering metadata that places rendering after simulation features. |

### `Inno.Rendering.Runtime.RenderTargetArtifactStatus`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.RenderTargetArtifactStatus`](../../src/services/rendering/Inno.Rendering.Runtime/Deployment/RenderTargetArtifactStatus.cs#L8) | Describes the current availability of one target-specific rendering artifact. |
| [`Inno.Rendering.Runtime.RenderTargetArtifactStatus.Failed`](../../src/services/rendering/Inno.Rendering.Runtime/Deployment/RenderTargetArtifactStatus.cs#L29) | Artifact production completed unsuccessfully and published a specific diagnostic. |
| [`Inno.Rendering.Runtime.RenderTargetArtifactStatus.Pending`](../../src/services/rendering/Inno.Rendering.Runtime/Deployment/RenderTargetArtifactStatus.cs#L19) | Artifact production is still running and no usable artifact is available yet. |
| [`Inno.Rendering.Runtime.RenderTargetArtifactStatus.Ready`](../../src/services/rendering/Inno.Rendering.Runtime/Deployment/RenderTargetArtifactStatus.cs#L14) | A validated artifact is available for immediate use. |
| [`Inno.Rendering.Runtime.RenderTargetArtifactStatus.Unavailable`](../../src/services/rendering/Inno.Rendering.Runtime/Deployment/RenderTargetArtifactStatus.cs#L24) | The active deployment does not contain the requested artifact. |

### `Inno.Rendering.Runtime.RenderTargetStore`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderTextureHandle Inno.Rendering.Runtime.RenderTargetStore.Import(Inno.Rendering.RenderGraphBuilder graph, Inno.Rendering.RenderTexture target)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderTargetStore.cs#L65) | Imports or creates one target in the current frame graph. |
| [`Inno.Rendering.Runtime.RenderTargetStore`](../../src/services/rendering/Inno.Rendering.Runtime/RenderTargetStore.cs#L11) | Owns persistent offscreen targets without exposing backend-native handles. |
| [`Inno.Rendering.Runtime.RenderTargetStore.RenderTargetStore(Inno.Rendering.IRenderDevice device, int capacity = 1024)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderTargetStore.cs#L34) | Creates a target registry for one device generation. |
| [`bool Inno.Rendering.Runtime.RenderTargetStore.TryGetTexture(Inno.Rendering.RenderTexture target, out Inno.Rendering.PersistentTextureHandle texture)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderTargetStore.cs#L106) | Tries to get the current opaque device texture for UI presentation. |
| [`int Inno.Rendering.Runtime.RenderTargetStore.count`](../../src/services/rendering/Inno.Rendering.Runtime/RenderTargetStore.cs#L46) | Gets the number of retained offscreen targets. |
| [`long Inno.Rendering.Runtime.RenderTargetStore.rejectedCount`](../../src/services/rendering/Inno.Rendering.Runtime/RenderTargetStore.cs#L51) | Gets target admissions rejected by finite capacity. |
| [`void Inno.Rendering.Runtime.RenderTargetStore.Dispose()`](../../src/services/rendering/Inno.Rendering.Runtime/RenderTargetStore.cs#L203) | Queues all owned resources for device-safe destruction. |
| [`void Inno.Rendering.Runtime.RenderTargetStore.PrepareFrame()`](../../src/services/rendering/Inno.Rendering.Runtime/RenderTargetStore.cs#L158) | Advances queued target releases at a frame safety point. |
| [`void Inno.Rendering.Runtime.RenderTargetStore.Release(Inno.Rendering.RenderTexture target)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderTargetStore.cs#L143) | Queues one target for frame-safe retirement. |

### `Inno.Rendering.Runtime.RenderTextureSubresourceData`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.RenderTextureSubresourceData`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderTextureSubresourceData.cs#L15) | Stores one complete texture mip and addressable layer, slice, or cubemap-face upload. |
| [`Inno.Rendering.Runtime.RenderTextureSubresourceData.RenderTextureSubresourceData(int mipLevel, int arrayLayer, System.ReadOnlySpan<byte> data)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderTextureSubresourceData.cs#L32) | Creates one immutable texture subresource upload. |
| [`System.ReadOnlyMemory<byte> Inno.Rendering.Runtime.RenderTextureSubresourceData.data`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderTextureSubresourceData.cs#L59) | Gets immutable tightly packed bytes. |
| [`int Inno.Rendering.Runtime.RenderTextureSubresourceData.arrayLayer`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderTextureSubresourceData.cs#L54) | Gets the zero-based array layer, volume slice, or flattened cubemap face. |
| [`int Inno.Rendering.Runtime.RenderTextureSubresourceData.mipLevel`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderTextureSubresourceData.cs#L49) | Gets the zero-based mip level. |

### `Inno.Rendering.Runtime.RenderView`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Mathematics.Matrix Inno.Rendering.Runtime.RenderView.projectionMatrix`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L67) | Gets the view-to-clip transform. |
| [`Inno.Core.Mathematics.Matrix Inno.Rendering.Runtime.RenderView.viewMatrix`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L62) | Gets the world-to-view transform. |
| [`Inno.Rendering.RenderViewport Inno.Rendering.Runtime.RenderView.viewport`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L57) | Gets the destination pixel rectangle. |
| [`Inno.Rendering.Runtime.RenderView`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L13) | Describes one backend-neutral view produced by a rendering model. |
| [`Inno.Rendering.Runtime.RenderView.RenderView(string id, Inno.Rendering.RenderViewport viewport, Inno.Core.Mathematics.Matrix viewMatrix, Inno.Core.Mathematics.Matrix projectionMatrix, ulong visibilityMask = 18446744073709551615)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L34) | Creates the exact view used to render content into a viewport. |
| [`string Inno.Rendering.Runtime.RenderView.id`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L52) | Gets the view identity within its output session. |
| [`ulong Inno.Rendering.Runtime.RenderView.visibilityMask`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L72) | Gets the model-defined visible content bits. |

### `Inno.Rendering.Runtime.SelectedViewContentCollector`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.SelectedViewContentCollector`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L445) | Restricts a neutral world-content collector to an output layer's source IDs. |
| [`Inno.Rendering.Runtime.SelectedViewContentCollector.SelectedViewContentCollector(Inno.Rendering.Runtime.IViewContentCollector inner, System.Collections.Generic.IReadOnlyList<string> sourceIds)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L460) | Creates a collector that forwards only the selected source identities. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Runtime.ViewContentItem> Inno.Rendering.Runtime.SelectedViewContentCollector.Collect(Inno.Rendering.Runtime.ViewContentContext context)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L477) | Collects only the assigned world-content sources for one exact view. |

### `Inno.Rendering.Runtime.ViewContentContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.References.ContentReadScope Inno.Rendering.Runtime.ViewContentContext.content`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L132) | Gets the ordered host-selected content roots. |
| [`Inno.Rendering.Runtime.RenderOutputInput Inno.Rendering.Runtime.ViewContentContext.input`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L156) | Gets frame-local input for this output. |
| [`Inno.Rendering.Runtime.RenderView Inno.Rendering.Runtime.ViewContentContext.view`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L142) | Gets the exact destination view. |
| [`Inno.Rendering.Runtime.ViewContentContext`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L78) | Supplies one content source with the host-selected scene roots and an exact view. |
| [`Inno.Rendering.Runtime.ViewContentContext.ViewContentContext(Inno.References.ContentReadScope content, string sessionId, Inno.Rendering.Runtime.RenderView view, ulong frameIndex, float deltaTime, Inno.Rendering.Runtime.RenderOutputInput? input = null, System.Collections.Generic.IReadOnlyList<Inno.Rendering.Runtime.RenderView>? views = null, System.Collections.Generic.IReadOnlyList<string>? sourceIds = null)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L108) | Creates a frame-scoped content collection context. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Runtime.RenderView> Inno.Rendering.Runtime.ViewContentContext.views`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L160) | Gets every view in the same model output. |
| [`System.Collections.Generic.IReadOnlyList<string>? Inno.Rendering.Runtime.ViewContentContext.sourceIds`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L164) | Gets the exclusive source allowlist; null collects every active source. |
| [`float Inno.Rendering.Runtime.ViewContentContext.deltaTime`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L152) | Gets elapsed time in seconds. |
| [`string Inno.Rendering.Runtime.ViewContentContext.sessionId`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L137) | Gets the stable host output session identity. |
| [`ulong Inno.Rendering.Runtime.ViewContentContext.frameIndex`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L147) | Gets the monotonic output frame number. |

### `Inno.Rendering.Runtime.ViewContentItem`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Identity.Identity Inno.Rendering.Runtime.ViewContentItem.owner`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L321) | Gets the owner identity for model-owned sorting and visibility. |
| [`Inno.Core.Mathematics.Matrix Inno.Rendering.Runtime.ViewContentItem.localToWorld`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L326) | Gets the local-to-world transform. |
| [`Inno.Core.Mathematics.Vector3 Inno.Rendering.Runtime.ViewContentItem.localBoundsMax`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L336) | Gets the local maximum corner. |
| [`Inno.Core.Mathematics.Vector3 Inno.Rendering.Runtime.ViewContentItem.localBoundsMin`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L331) | Gets the local minimum corner. |
| [`Inno.Rendering.Runtime.IViewDrawable Inno.Rendering.Runtime.ViewContentItem.drawable`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L341) | Gets the drawable encoded by the selected rendering model. |
| [`Inno.Rendering.Runtime.IViewPointerTarget? Inno.Rendering.Runtime.ViewContentItem.pointerTarget`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L345) | Gets the optional pointer target associated with this draw item. |
| [`Inno.Rendering.Runtime.ViewContentItem`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L278) | One world item supplied to a rendering model without choosing its sort policy. |
| [`Inno.Rendering.Runtime.ViewContentItem.ViewContentItem(Inno.Core.Identity.Identity owner, Inno.Core.Mathematics.Matrix localToWorld, Inno.Core.Mathematics.Vector3 localBoundsMin, Inno.Core.Mathematics.Vector3 localBoundsMax, Inno.Rendering.Runtime.IViewDrawable drawable, Inno.Rendering.Runtime.IViewPointerTarget? pointerTarget = null)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L302) | Creates an item whose lifetime is limited to the current frame. |

### `Inno.Rendering.Runtime.ViewContentSourceExtensionAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Runtime.ViewContentSourceExtensionAttribute`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L402) | Discovers an independently reloadable world-content source. |
| [`Inno.Rendering.Runtime.ViewContentSourceExtensionAttribute.ViewContentSourceExtensionAttribute(string id)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L412) | Creates a source declaration with a stable identifier. |
| [`string Inno.Rendering.Runtime.ViewContentSourceExtensionAttribute.id`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L421) | Gets the globally stable source identifier. |

## 项目依赖

- [Inno.Core.Identity](../core/Inno.Core.Identity.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Rendering](Inno.Rendering.md)：公开引用边界由实际签名核对。
- [Inno.Content](../assets/Inno.Content.md)：公开引用边界由实际签名核对。
- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：公开引用边界由实际签名核对。
- [Inno.Runtime.Contracts](../runtime/Inno.Runtime.Contracts.md)：公开引用边界由实际签名核对。
- [Inno.Input](../input/Inno.Input.md)：公开引用边界由实际签名核对。
- [Inno.References](../references/Inno.References.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
- [Inno.Rendering.Assets](Inno.Rendering.Assets.md)：公开引用边界由实际签名核对。
