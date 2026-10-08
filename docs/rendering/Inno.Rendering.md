# Inno.Rendering

[分类索引](README.md) · [Wiki 首页](../README.md) · [本轮整改计划](../architecture/ARCHITECTURE_CLEANUP_PLAN_2026_10_06.md)

## 职责与边界

只提供设备、能力、资源描述、命令、绑定反射和 RenderGraph 机制。仅依赖必要 Foundation 契约；不引用 Assets、References、Scene、Editor、Adapter 或 Native。不声明 Camera、Light、PBR、2D/3D 模型，也不解析资产或 Shader 源码。

## 组合与生命周期

设备由具体 Adapter 创建，宿主持有唯一设备 owner。图中的资源、pass 与 handle 只属于该图及其 generation；已提交 pass 声明冻结，不能在后续 mutation 中修改。GPU 资源由资源 owner 明确退休，资产持久化不保存 GPU handle。

`IRenderDevice.primaryPresentationSize` 必须由实现提供：有效值是真实物理像素尺寸，`null` 表示当前没有可用主输出。`SetPrimaryPresentationSize` 在帧安全点发布变化；零尺寸不是有效值。原生 SDK 内部的最小资源尺寸不代表可呈现输出。离屏目标使用自己的有效尺寸。

`Validate()` 返回 `RenderGraphValidationResult`，只执行关系、能力、初始化、依赖、cycle 和 view 数量检查。`Compile()` 复用当前 revision 的验证分析，然后分配资源并构造最终图。任何 mutation 都使过期分析失效。完整编译每个正常 Runtime 帧只执行一次。

`BeginMutationScope()` 提供 LIFO 事务：失败回滚资源、pass、output、name scope 和此前冻结的验证结果；成功 `Commit()` 验证并冻结新声明。回滚不重用已发放的资源身份，旧 handle 不能误指向新资源。

## 使用示例

```csharp
using Inno.Rendering;

static RenderGraphValidationResult ValidateGraph(RenderGraphBuilder graph)
{
    return graph.Validate();
}
```

实际 Pipeline、Material 与请求组合见 [Runtime](Inno.Rendering.Runtime.md)；运行资产见 [Assets](Inno.Rendering.Assets.md)；Shader 图与 IR 见 [Shaders](Inno.Rendering.Shaders.md)。`ShaderInterface` 仅保存编译绑定事实，Material 默认值与 Asset 引用属于 Assets。

`RenderDeviceAllocationCounters` 是设备 generation 的累计分配快照，未提供测量的实现返回 null。`primaryPresentationEncodesSrgb` 报告真实输出编码能力；输出传递与图层合成程序由宿主注入。

## 验证

`tests/rendering/Inno.Rendering.Tests` 验证完整图语义、mutation rollback、revision 失效与严格二进制协议；架构验证器禁止本项目引用资产或实现层。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Rendering.CompiledRenderAttachment`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.CompiledRenderAttachment`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L164) | Describes one compiled raster attachment. |
| [`Inno.Rendering.RenderClearColor Inno.Rendering.CompiledRenderAttachment.clearColor`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L218) | Gets the linear color clear value. |
| [`Inno.Rendering.RenderLoadAction Inno.Rendering.CompiledRenderAttachment.loadAction`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L208) | Gets initial content behavior. |
| [`Inno.Rendering.RenderStoreAction Inno.Rendering.CompiledRenderAttachment.storeAction`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L213) | Gets final content behavior. |
| [`Inno.Rendering.RenderTextureHandle Inno.Rendering.CompiledRenderAttachment.texture`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L183) | Gets the attached graph texture. |
| [`bool Inno.Rendering.CompiledRenderAttachment.isDepth`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L193) | Gets whether this is a depth or depth-stencil attachment. |
| [`byte Inno.Rendering.CompiledRenderAttachment.clearStencil`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L228) | Gets the stencil clear value. |
| [`float Inno.Rendering.CompiledRenderAttachment.clearDepth`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L223) | Gets the depth clear value. |
| [`int Inno.Rendering.CompiledRenderAttachment.arrayLayer`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L203) | Gets the attached texture-array layer. |
| [`int Inno.Rendering.CompiledRenderAttachment.mipLevel`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L198) | Gets the attached mip level. |
| [`int Inno.Rendering.CompiledRenderAttachment.slot`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L188) | Gets the zero-based color slot, or zero for a depth attachment. |

### `Inno.Rendering.CompiledRenderBuffer`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.CompiledRenderBuffer`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L112) | Describes one buffer allocation selected by render-graph compilation. |
| [`Inno.Rendering.PersistentBufferHandle Inno.Rendering.CompiledRenderBuffer.persistentHandle`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L153) | Gets the persistent device buffer for imported resources. |
| [`Inno.Rendering.RenderBufferDescriptor Inno.Rendering.CompiledRenderBuffer.descriptor`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L143) | Gets buffer requirements. |
| [`Inno.Rendering.RenderBufferHandle Inno.Rendering.CompiledRenderBuffer.handle`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L133) | Gets the frame-scoped logical handle. |
| [`bool Inno.Rendering.CompiledRenderBuffer.imported`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L148) | Gets whether the resource was imported from persistent device state. |
| [`int Inno.Rendering.CompiledRenderBuffer.physicalSlot`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L158) | Gets the aliasing allocation slot, or negative one for imported resources. |
| [`string Inno.Rendering.CompiledRenderBuffer.name`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L138) | Gets the debug and diagnostic name. |

### `Inno.Rendering.CompiledRenderGraph`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.CompiledRenderGraph`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L362) | Contains validated, culled and topologically scheduled frame work. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.CompiledRenderBuffer> Inno.Rendering.CompiledRenderGraph.buffers`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L398) | Gets compiled logical buffer allocations. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.CompiledRenderPass> Inno.Rendering.CompiledRenderGraph.passes`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L388) | Gets scheduled passes in backend view order. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.CompiledRenderTexture> Inno.Rendering.CompiledRenderGraph.textures`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L393) | Gets compiled logical texture allocations. |
| [`uint Inno.Rendering.CompiledRenderGraph.generation`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L383) | Gets the source frame generation. |
| [`void Inno.Rendering.CompiledRenderGraph.Execute(Inno.Rendering.IRenderGraphBackend backend, ulong frameIndex)`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L412) | Executes pass callbacks through a concrete backend with complete unwind. |

### `Inno.Rendering.CompiledRenderPass`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.CompiledRenderPass`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L234) | Provides immutable backend-facing metadata for one scheduled pass. |
| [`Inno.Rendering.RenderClearColor Inno.Rendering.CompiledRenderPass.presentationClearColor`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L303) | Gets the linear clear color for a presentation target. |
| [`Inno.Rendering.RenderPassKind Inno.Rendering.CompiledRenderPass.kind`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L278) | Gets the pass command domain. |
| [`Inno.Rendering.RenderPassRecordingMode Inno.Rendering.CompiledRenderPass.recordingMode`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L313) | Gets whether callback recording is serial or worker-thread eligible. |
| [`Inno.Rendering.RenderPhaseId Inno.Rendering.CompiledRenderPass.phase`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L273) | Gets the open render phase identifier. |
| [`Inno.Rendering.RenderSurfaceHandle Inno.Rendering.CompiledRenderPass.surface`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L293) | Gets the detached presentation surface, or an invalid handle for the primary backbuffer. |
| [`Inno.Rendering.RenderViewTransform? Inno.Rendering.CompiledRenderPass.viewTransform`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L308) | Gets backend-ready view transforms, or for a matrix-free pass. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.CompiledRenderAttachment> Inno.Rendering.CompiledRenderPass.attachments`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L288) | Gets raster attachments in declaration order. |
| [`bool Inno.Rendering.CompiledRenderPass.clearsPresentationTarget`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L298) | Gets whether this pass clears its presentation target before drawing. |
| [`int Inno.Rendering.CompiledRenderPass.viewIndex`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L283) | Gets the backend-neutral logical view order. |
| [`string Inno.Rendering.CompiledRenderPass.name`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L268) | Gets the unique diagnostic name. |

### `Inno.Rendering.CompiledRenderTexture`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.CompiledRenderTexture`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L60) | Describes one texture allocation selected by render-graph compilation. |
| [`Inno.Rendering.PersistentTextureHandle Inno.Rendering.CompiledRenderTexture.persistentHandle`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L101) | Gets the persistent device texture for imported resources. |
| [`Inno.Rendering.RenderTextureDescriptor Inno.Rendering.CompiledRenderTexture.descriptor`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L91) | Gets texture requirements. |
| [`Inno.Rendering.RenderTextureHandle Inno.Rendering.CompiledRenderTexture.handle`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L81) | Gets the frame-scoped logical handle. |
| [`bool Inno.Rendering.CompiledRenderTexture.imported`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L96) | Gets whether the resource was imported from persistent device state. |
| [`int Inno.Rendering.CompiledRenderTexture.physicalSlot`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L106) | Gets the aliasing allocation slot, or negative one for imported resources. |
| [`string Inno.Rendering.CompiledRenderTexture.name`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L86) | Gets the debug and diagnostic name. |

### `Inno.Rendering.ComputePassBuilder`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.ComputePassBuilder`](../../src/services/rendering/Inno.Rendering/Graph/ComputePassBuilder.cs#L9) | Declares compute resource reads and unordered writes. |
| [`Inno.Rendering.ComputePassBuilder Inno.Rendering.ComputePassBuilder.ReadStorageBuffer(Inno.Rendering.RenderBufferHandle buffer)`](../../src/services/rendering/Inno.Rendering/Graph/ComputePassBuilder.cs#L104) | Declares an unordered buffer read. |
| [`Inno.Rendering.ComputePassBuilder Inno.Rendering.ComputePassBuilder.ReadStorageTexture(Inno.Rendering.RenderTextureHandle texture)`](../../src/services/rendering/Inno.Rendering/Graph/ComputePassBuilder.cs#L47) | Declares an unordered texture read. |
| [`Inno.Rendering.ComputePassBuilder Inno.Rendering.ComputePassBuilder.ReadWriteStorageBuffer(Inno.Rendering.RenderBufferHandle buffer)`](../../src/services/rendering/Inno.Rendering/Graph/ComputePassBuilder.cs#L142) | Declares an unordered buffer read and write. |
| [`Inno.Rendering.ComputePassBuilder Inno.Rendering.ComputePassBuilder.ReadWriteStorageTexture(Inno.Rendering.RenderTextureHandle texture)`](../../src/services/rendering/Inno.Rendering/Graph/ComputePassBuilder.cs#L85) | Declares an unordered texture read and write. |
| [`Inno.Rendering.ComputePassBuilder Inno.Rendering.ComputePassBuilder.SetViewTransform(System.ReadOnlySpan<float> viewMatrix, System.ReadOnlySpan<float> projectionMatrix)`](../../src/services/rendering/Inno.Rendering/Graph/ComputePassBuilder.cs#L29) | Sets backend-ready column-major transform matrices exposed to this compute view. |
| [`Inno.Rendering.ComputePassBuilder Inno.Rendering.ComputePassBuilder.WriteStorageBuffer(Inno.Rendering.RenderBufferHandle buffer)`](../../src/services/rendering/Inno.Rendering/Graph/ComputePassBuilder.cs#L123) | Declares an unordered buffer write. |
| [`Inno.Rendering.ComputePassBuilder Inno.Rendering.ComputePassBuilder.WriteStorageTexture(Inno.Rendering.RenderTextureHandle texture)`](../../src/services/rendering/Inno.Rendering/Graph/ComputePassBuilder.cs#L66) | Declares an unordered texture write. |

### `Inno.Rendering.ComputePipelineDescriptor`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.ComputePipelineDescriptor`](../../src/services/rendering/Inno.Rendering/Device/ComputePipelineDescriptor.cs#L10) | Describes a compute program candidate and reflected interface contract. |
| [`Inno.Rendering.ComputePipelineDescriptor.ComputePipelineDescriptor(System.ReadOnlySpan<byte> computeShader, System.Collections.Generic.IReadOnlyList<Inno.Rendering.RenderShaderBindingDescriptor> bindings)`](../../src/services/rendering/Inno.Rendering/Device/ComputePipelineDescriptor.cs#L24) | Creates a compute pipeline descriptor. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.RenderShaderBindingDescriptor> Inno.Rendering.ComputePipelineDescriptor.bindings`](../../src/services/rendering/Inno.Rendering/Device/ComputePipelineDescriptor.cs#L51) | Gets the manifest-derived interface contract. |
| [`System.ReadOnlyMemory<byte> Inno.Rendering.ComputePipelineDescriptor.computeShader`](../../src/services/rendering/Inno.Rendering/Device/ComputePipelineDescriptor.cs#L46) | Gets the target backend compute shader binary. |

### `Inno.Rendering.ComputePipelineHandle`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.ComputePipelineHandle`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L58) | Identifies a backend-neutral compute pipeline object. |
| [`bool Inno.Rendering.ComputePipelineHandle.isValid`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L74) | Gets whether the handle identifies a compute pipeline. |

### `Inno.Rendering.CopyPassBuilder`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.CopyPassBuilder`](../../src/services/rendering/Inno.Rendering/Graph/CopyPassBuilder.cs#L9) | Declares explicit resource copy access. |
| [`Inno.Rendering.CopyPassBuilder Inno.Rendering.CopyPassBuilder.CopyBuffer(Inno.Rendering.RenderBufferHandle source, Inno.Rendering.RenderBufferHandle destination)`](../../src/services/rendering/Inno.Rendering/Graph/CopyPassBuilder.cs#L58) | Declares one buffer copy operation. |
| [`Inno.Rendering.CopyPassBuilder Inno.Rendering.CopyPassBuilder.CopyTexture(Inno.Rendering.RenderTextureHandle source, Inno.Rendering.RenderTextureHandle destination)`](../../src/services/rendering/Inno.Rendering/Graph/CopyPassBuilder.cs#L29) | Declares one texture copy operation. |

### `Inno.Rendering.GraphicsApi`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.GraphicsApi`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L11) | Identifies the graphics API family selected by the host without exposing a backend-native enum. Pipelines use this value only for capability-aware choices and target artifact selection; it does not require separate pipeline or shader source implementations. |
| [`Inno.Rendering.GraphicsApi.GraphicsApi(string value)`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L55) | Creates an open, stable backend identity without registering a backend implementation. |
| [`bool Inno.Rendering.GraphicsApi.isValid`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L72) | Gets whether this value contains an initialized backend identity. |
| [`int Inno.Rendering.GraphicsApi.CompareTo(Inno.Rendering.GraphicsApi other)`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L106) | Compares two backend identities using stable ordinal ordering. |
| [`override string Inno.Rendering.GraphicsApi.ToString()`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L114) | Formats the exact stable backend identity. |
| [`static Inno.Rendering.GraphicsApi Inno.Rendering.GraphicsApi.Direct3D11`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L20) | Direct3D 11 renderer. |
| [`static Inno.Rendering.GraphicsApi Inno.Rendering.GraphicsApi.Direct3D12`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L24) | Direct3D 12 renderer. |
| [`static Inno.Rendering.GraphicsApi Inno.Rendering.GraphicsApi.Metal`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L28) | Apple Metal renderer. |
| [`static Inno.Rendering.GraphicsApi Inno.Rendering.GraphicsApi.Noop`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L16) | Headless validation backend. |
| [`static Inno.Rendering.GraphicsApi Inno.Rendering.GraphicsApi.OpenGL`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L36) | Desktop OpenGL renderer. |
| [`static Inno.Rendering.GraphicsApi Inno.Rendering.GraphicsApi.OpenGLES`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L40) | OpenGL ES renderer. |
| [`static Inno.Rendering.GraphicsApi Inno.Rendering.GraphicsApi.Vulkan`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L32) | Khronos Vulkan renderer. |
| [`static Inno.Rendering.GraphicsApi Inno.Rendering.GraphicsApi.WebGPU`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L44) | WebGPU renderer. |
| [`static bool Inno.Rendering.GraphicsApi.TryParse(string? value, out Inno.Rendering.GraphicsApi api)`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L86) | Parses an open backend identity without consulting a closed engine-side backend list. |
| [`string Inno.Rendering.GraphicsApi.value`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L67) | Gets the stable backend identity used by target artifacts and provider selection. |

### `Inno.Rendering.GraphicsCapabilities`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.GraphicsApi Inno.Rendering.GraphicsCapabilities.backend`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L367) | Gets the active graphics API family. |
| [`Inno.Rendering.GraphicsCapabilities`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L279) | Provides an immutable capability snapshot for one device generation. |
| [`Inno.Rendering.GraphicsCapabilities.GraphicsCapabilities(Inno.Rendering.GraphicsApi backend, Inno.Rendering.GraphicsCapability features, Inno.Rendering.GraphicsLimits limits, System.Collections.Generic.IEnumerable<Inno.Rendering.RenderTextureFormat> sampledFormats, System.Collections.Generic.IEnumerable<Inno.Rendering.RenderTextureFormat> renderTargetFormats, System.Collections.Generic.IEnumerable<Inno.Rendering.RenderTextureFormat> storageReadFormats, System.Collections.Generic.IEnumerable<Inno.Rendering.RenderTextureFormat> storageWriteFormats, bool originBottomLeft, bool homogeneousDepth, System.Collections.Generic.IEnumerable<Inno.Rendering.RenderTextureFormat>? sampled3DFormats = null, System.Collections.Generic.IEnumerable<Inno.Rendering.RenderTextureFormat>? sampledCubeFormats = null, System.Collections.Generic.IEnumerable<Inno.Rendering.RenderTextureFormat>? multisampleRenderTargetFormats = null)`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L328) | Creates a device capability snapshot. |
| [`Inno.Rendering.GraphicsCapability Inno.Rendering.GraphicsCapabilities.features`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L372) | Gets all supported optional features. |
| [`Inno.Rendering.GraphicsLimits Inno.Rendering.GraphicsCapabilities.limits`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L377) | Gets device limits. |
| [`bool Inno.Rendering.GraphicsCapabilities.Supports(Inno.Rendering.GraphicsCapability required)`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L398) | Tests whether every requested optional feature is supported. |
| [`bool Inno.Rendering.GraphicsCapabilities.SupportsMultisampleRenderTarget(Inno.Rendering.RenderTextureFormat format)`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L455) | Tests whether a format is valid as a multisampled raster attachment. |
| [`bool Inno.Rendering.GraphicsCapabilities.SupportsRenderTarget(Inno.Rendering.RenderTextureFormat format)`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L444) | Tests whether a format is valid as a raster attachment. |
| [`bool Inno.Rendering.GraphicsCapabilities.SupportsSampled(Inno.Rendering.RenderTextureFormat format)`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L409) | Tests whether a format is valid for sampled two-dimensional textures. |
| [`bool Inno.Rendering.GraphicsCapabilities.SupportsSampled(Inno.Rendering.RenderTextureFormat format, Inno.Rendering.RenderTextureDimension dimension)`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L423) | Tests whether a format is valid for sampled textures of one dimensional shape. |
| [`bool Inno.Rendering.GraphicsCapabilities.SupportsStorage(Inno.Rendering.RenderTextureFormat format, Inno.Rendering.RenderStorageAccess access)`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L469) | Tests whether a format supports the requested unordered shader access. |
| [`bool Inno.Rendering.GraphicsCapabilities.homogeneousDepth`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L387) | Gets whether clip-space depth uses the negative-one-to-one range. |
| [`bool Inno.Rendering.GraphicsCapabilities.originBottomLeft`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L382) | Gets whether render-target coordinates start at the bottom-left. |

### `Inno.Rendering.GraphicsCapability`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.GraphicsCapability`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L130) | Declares backend-neutral optional graphics functionality. |
| [`Inno.Rendering.GraphicsCapability.AlphaToCoverage`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L168) | Alpha-to-coverage rasterization is supported. |
| [`Inno.Rendering.GraphicsCapability.BufferCopy`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L164) | General GPU buffer copy operations are supported directly. |
| [`Inno.Rendering.GraphicsCapability.Compute`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L140) | Compute shader dispatch. |
| [`Inno.Rendering.GraphicsCapability.ConcurrentEncoders`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L156) | Multiple command encoders may record concurrently. |
| [`Inno.Rendering.GraphicsCapability.FragmentDepth`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L208) | Fragment shaders may write depth explicitly. |
| [`Inno.Rendering.GraphicsCapability.IndependentBlend`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L152) | Independent blend state per color attachment. |
| [`Inno.Rendering.GraphicsCapability.Index32`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L172) | Unsigned 32-bit index buffers are supported. |
| [`Inno.Rendering.GraphicsCapability.Indirect`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L148) | Indirect draw and dispatch commands. |
| [`Inno.Rendering.GraphicsCapability.Instancing`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L176) | Instanced draw input is supported. |
| [`Inno.Rendering.GraphicsCapability.None`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L136) | No optional feature. |
| [`Inno.Rendering.GraphicsCapability.ProceduralDraw`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L204) | Procedural draws using shader vertex identifiers are supported. |
| [`Inno.Rendering.GraphicsCapability.StorageBuffer`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L144) | Shader-readable and writable storage buffers. |
| [`Inno.Rendering.GraphicsCapability.StorageTexture`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L212) | Shader-readable and writable storage textures are supported. |
| [`Inno.Rendering.GraphicsCapability.SwapChain`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L180) | Additional native presentation surfaces are supported. |
| [`Inno.Rendering.GraphicsCapability.Texture2DArray`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L184) | Two-dimensional texture arrays are supported. |
| [`Inno.Rendering.GraphicsCapability.Texture3D`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L188) | Three-dimensional textures are supported. |
| [`Inno.Rendering.GraphicsCapability.TextureBlit`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L160) | Texture copy operations are supported directly. |
| [`Inno.Rendering.GraphicsCapability.TextureCubeArray`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L192) | Cubemap texture arrays are supported. |
| [`Inno.Rendering.GraphicsCapability.TextureReadback`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L216) | Asynchronous texture transfer from GPU memory to CPU-visible bytes. |
| [`Inno.Rendering.GraphicsCapability.VertexAttributeHalf`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L196) | Half-precision vertex attributes are supported. |
| [`Inno.Rendering.GraphicsCapability.VertexAttributeUInt10`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L200) | Packed 10:10:10:2 vertex attributes are supported. |

### `Inno.Rendering.GraphicsLimits`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.GraphicsLimits`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L222) | Captures device limits used to validate and compile render graphs. |
| [`Inno.Rendering.GraphicsLimits.GraphicsLimits(int maxViews, int maxColorAttachments, int maxTextureSize, int maxComputeBindings)`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L239) | Creates a graphics limits snapshot. |
| [`int Inno.Rendering.GraphicsLimits.maxColorAttachments`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L263) | Gets the maximum color attachments in one raster pass. |
| [`int Inno.Rendering.GraphicsLimits.maxComputeBindings`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L273) | Gets the maximum storage bindings in one compute pass. |
| [`int Inno.Rendering.GraphicsLimits.maxTextureSize`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L268) | Gets the maximum two-dimensional texture extent. |
| [`int Inno.Rendering.GraphicsLimits.maxViews`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L258) | Gets the maximum logical backend views in one frame. |

### `Inno.Rendering.GraphicsPipelineDescriptor`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.GraphicsPipelineDescriptor`](../../src/services/rendering/Inno.Rendering/Device/GraphicsPipelineDescriptor.cs#L10) | Describes a graphics program candidate and reflected interface contract. |
| [`Inno.Rendering.GraphicsPipelineDescriptor.GraphicsPipelineDescriptor(System.ReadOnlySpan<byte> vertexShader, System.ReadOnlySpan<byte> fragmentShader, System.Collections.Generic.IReadOnlyList<Inno.Rendering.RenderShaderBindingDescriptor> bindings, Inno.Rendering.RenderVertexLayout? vertexLayout, Inno.Rendering.RenderRasterState? rasterState = null)`](../../src/services/rendering/Inno.Rendering/Device/GraphicsPipelineDescriptor.cs#L34) | Creates a graphics pipeline descriptor. |
| [`Inno.Rendering.RenderRasterState Inno.Rendering.GraphicsPipelineDescriptor.rasterState`](../../src/services/rendering/Inno.Rendering/Device/GraphicsPipelineDescriptor.cs#L83) | Gets fixed-function raster state. |
| [`Inno.Rendering.RenderVertexLayout? Inno.Rendering.GraphicsPipelineDescriptor.vertexLayout`](../../src/services/rendering/Inno.Rendering/Device/GraphicsPipelineDescriptor.cs#L78) | Gets the required mesh vertex layout, or for procedural vertices. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.RenderShaderBindingDescriptor> Inno.Rendering.GraphicsPipelineDescriptor.bindings`](../../src/services/rendering/Inno.Rendering/Device/GraphicsPipelineDescriptor.cs#L73) | Gets the manifest-derived interface contract. |
| [`System.ReadOnlyMemory<byte> Inno.Rendering.GraphicsPipelineDescriptor.fragmentShader`](../../src/services/rendering/Inno.Rendering/Device/GraphicsPipelineDescriptor.cs#L68) | Gets the target backend fragment shader binary. |
| [`System.ReadOnlyMemory<byte> Inno.Rendering.GraphicsPipelineDescriptor.vertexShader`](../../src/services/rendering/Inno.Rendering/Device/GraphicsPipelineDescriptor.cs#L63) | Gets the target backend vertex shader binary. |

### `Inno.Rendering.GraphicsPipelineHandle`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.GraphicsPipelineHandle`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L36) | Identifies a backend-neutral graphics pipeline object. |
| [`bool Inno.Rendering.GraphicsPipelineHandle.isValid`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L52) | Gets whether the handle identifies a graphics pipeline. |

### `Inno.Rendering.GraphicsProgramArtifactCodec`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.GraphicsProgramArtifactCodec`](../../src/services/rendering/Inno.Rendering/Shaders/GraphicsProgramArtifactCodec.cs#L10) | Transports compiled graphics programs and device binding facts independently of authored assets. |
| [`static Inno.Rendering.GraphicsPipelineDescriptor Inno.Rendering.GraphicsProgramArtifactCodec.Decode(System.ReadOnlySpan<byte> bytes, Inno.Rendering.RenderVertexLayout? vertexLayout = null)`](../../src/services/rendering/Inno.Rendering/Shaders/GraphicsProgramArtifactCodec.cs#L75) | Validates a complete device program artifact before constructing an owned pipeline descriptor. |
| [`static byte[] Inno.Rendering.GraphicsProgramArtifactCodec.Encode(Inno.Rendering.GraphicsPipelineDescriptor descriptor)`](../../src/services/rendering/Inno.Rendering/Shaders/GraphicsProgramArtifactCodec.cs#L34) | Encodes owned program binaries, reflected bindings, and raster state for device distribution. Vertex layout belongs to the eventual caller and is not part of this artifact. |

### `Inno.Rendering.IRenderDevice`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.ComputePipelineHandle Inno.Rendering.IRenderDevice.CreateComputePipeline(Inno.Rendering.ComputePipelineDescriptor descriptor, string name)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L356) | Creates and reflection-validates a compute pipeline at a frame safety point. |
| [`Inno.Rendering.GraphicsCapabilities Inno.Rendering.IRenderDevice.capabilities`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L59) | Gets immutable capabilities for the active device generation. |
| [`Inno.Rendering.GraphicsPipelineHandle Inno.Rendering.IRenderDevice.CreateGraphicsPipeline(Inno.Rendering.GraphicsPipelineDescriptor descriptor, string name)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L331) | Creates and reflection-validates a graphics pipeline at a frame safety point. |
| [`Inno.Rendering.IRenderDevice`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L54) | Owns one graphics backend generation and its frame submission boundary. |
| [`Inno.Rendering.PersistentBufferHandle Inno.Rendering.IRenderDevice.CreateBuffer(Inno.Rendering.PersistentBufferDescriptor descriptor, System.ReadOnlySpan<byte> initialData, string name)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L287) | Creates a persistent vertex, index or storage buffer at a frame safety point. |
| [`Inno.Rendering.PersistentTextureHandle Inno.Rendering.IRenderDevice.CreateTexture(Inno.Rendering.RenderTextureContainer container, System.ReadOnlySpan<byte> data, bool sRgb, string name)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L174) | Creates a persistent sampled texture from a validated portable container. |
| [`Inno.Rendering.PersistentTextureHandle Inno.Rendering.IRenderDevice.CreateTexture(Inno.Rendering.RenderTextureDescriptor descriptor, string name)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L148) | Creates a persistent texture at a frame safety point. |
| [`Inno.Rendering.RenderDeviceAllocationCounters? Inno.Rendering.IRenderDevice.allocationCounters`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L86) | Gets cumulative native transient allocations for this device generation on the API thread. A null snapshot means allocation accounting is unavailable, not that no resources were allocated. |
| [`Inno.Rendering.RenderDeviceFrameCounters Inno.Rendering.IRenderDevice.frameCounters`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L80) | Gets command counts recorded since the latest call. Backends that cannot provide command accounting return zero counters. |
| [`Inno.Rendering.RenderPresentationSize? Inno.Rendering.IRenderDevice.primaryPresentationSize`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L69) | Gets the real primary presentation extent in physical pixels, or null while no output is available. |
| [`Inno.Rendering.RenderTextureReadbackHandle Inno.Rendering.IRenderDevice.BeginTextureReadback(Inno.Rendering.PersistentTextureHandle texture, int mipLevel = 0)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L234) | Begins an asynchronous readback of one complete persistent texture mip. |
| [`bool Inno.Rendering.IRenderDevice.TryGetTextureReadback(Inno.Rendering.RenderTextureReadbackHandle readback, out Inno.Rendering.RenderTextureReadbackResult? result)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L251) | Tries to complete one previously requested texture readback. |
| [`bool Inno.Rendering.IRenderDevice.primaryPresentationEncodesSrgb`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L74) | Gets whether the primary presentation target encodes linear RGB to sRGB during writes. |
| [`uint Inno.Rendering.IRenderDevice.EndFrame()`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L113) | Ends all encoders and advances the graphics backend exactly once. |
| [`uint Inno.Rendering.IRenderDevice.generation`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L64) | Gets the non-zero device generation used to reject stale persistent handles. |
| [`void Inno.Rendering.IRenderDevice.BeginFrame()`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L91) | Begins the sole API-thread frame scope and processes queued resource work. |
| [`void Inno.Rendering.IRenderDevice.CancelTextureReadback(Inno.Rendering.RenderTextureReadbackHandle readback)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L262) | Stops retaining a readback result that is no longer needed by its caller. |
| [`void Inno.Rendering.IRenderDevice.DestroyBuffer(Inno.Rendering.PersistentBufferHandle buffer)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L317) | Queues a persistent buffer for delayed GPU-safe destruction. |
| [`void Inno.Rendering.IRenderDevice.DestroyComputePipeline(Inno.Rendering.ComputePipelineHandle pipeline)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L367) | Queues a compute pipeline for delayed GPU-safe destruction. |
| [`void Inno.Rendering.IRenderDevice.DestroyGraphicsPipeline(Inno.Rendering.GraphicsPipelineHandle pipeline)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L342) | Queues a graphics pipeline for delayed GPU-safe destruction. |
| [`void Inno.Rendering.IRenderDevice.DestroyTexture(Inno.Rendering.PersistentTextureHandle texture)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L270) | Queues a persistent texture for delayed GPU-safe destruction. |
| [`void Inno.Rendering.IRenderDevice.Execute(Inno.Rendering.CompiledRenderGraph graph, ulong frameIndex)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L102) | Executes one compiled graph without presenting or advancing another frame. |
| [`void Inno.Rendering.IRenderDevice.SetPrimaryPresentationSize(Inno.Rendering.RenderPresentationSize? size)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L124) | Queues primary output availability and its real pixel extent for the next frame safety point. |
| [`void Inno.Rendering.IRenderDevice.SetVerticalSync(bool enabled)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L134) | Queues display synchronization policy on the API thread for the next frame boundary. Every backend must implement both policies. Repeated values are idempotent and must not reset an active pass. Windowless devices retain the requested policy without performing presentation. |
| [`void Inno.Rendering.IRenderDevice.UpdateBuffer(Inno.Rendering.PersistentBufferHandle buffer, System.ReadOnlySpan<byte> data, int startElement = 0)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L305) | Replaces a contiguous range in a dynamic persistent buffer at a frame safety point. |
| [`void Inno.Rendering.IRenderDevice.UpdateTexture(Inno.Rendering.PersistentTextureHandle texture, System.ReadOnlySpan<byte> data, int mipLevel = 0, int arrayLayer = 0)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L197) | Replaces one complete mip and addressable layer, slice, or cubemap face. |
| [`void Inno.Rendering.IRenderDevice.UpdateTextureRegion(Inno.Rendering.PersistentTextureHandle texture, Inno.Rendering.RenderTextureRegion region, System.ReadOnlySpan<byte> data)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L216) | Replaces a tightly packed rectangular region in one persistent texture subresource. |

### `Inno.Rendering.IRenderFrameGraphContributor`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.IRenderFrameGraphContributor`](../../src/services/rendering/Inno.Rendering/Graph/IRenderFrameGraphContributor.cs#L6) | Adds transient frame-final work, such as UI composition, without owning or advancing the graphics frame. |
| [`void Inno.Rendering.IRenderFrameGraphContributor.AddRenderPasses(Inno.Rendering.RenderGraphBuilder graph, ulong frameIndex)`](../../src/services/rendering/Inno.Rendering/Graph/IRenderFrameGraphContributor.cs#L25) | Adds frame-scoped passes after all user render-request graphs have executed. |
| [`void Inno.Rendering.IRenderFrameGraphContributor.PrepareFrame(ulong frameIndex)`](../../src/services/rendering/Inno.Rendering/Graph/IRenderFrameGraphContributor.cs#L14) | Applies queued resource changes at the current frame safety point. |

### `Inno.Rendering.IRenderFrameUploadService`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.IRenderFrameUploadService`](../../src/services/rendering/Inno.Rendering/IRenderFrameUploadService.cs#L10) | Streams immutable CPU data into reusable GPU pages for the current frame. |
| [`Inno.Rendering.RenderBufferSlice Inno.Rendering.IRenderFrameUploadService.UploadBuffer(Inno.Rendering.RenderBufferUploadDescriptor descriptor, System.ReadOnlyMemory<byte> data, string name)`](../../src/services/rendering/Inno.Rendering/IRenderFrameUploadService.cs#L27) | Uploads complete elements and returns a slice valid only during the current frame. |

### `Inno.Rendering.IRenderGraphBackend`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.IRenderGraphBackend`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L321) | Bridges compiled graph execution to a concrete graphics backend. |
| [`Inno.Rendering.RenderCommandEncoder Inno.Rendering.IRenderGraphBackend.BeginPass(Inno.Rendering.CompiledRenderPass pass)`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L340) | Begins one scheduled pass and acquires its command encoder. |
| [`void Inno.Rendering.IRenderGraphBackend.BeginGraph(Inno.Rendering.CompiledRenderGraph graph)`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L329) | Begins execution and prepares transient allocations. |
| [`void Inno.Rendering.IRenderGraphBackend.EndGraph(Inno.Rendering.CompiledRenderGraph graph)`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L356) | Ends graph execution without presenting an additional frame. |
| [`void Inno.Rendering.IRenderGraphBackend.EndPass(Inno.Rendering.CompiledRenderPass pass)`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L348) | Ends one scheduled pass and releases its encoder. |

### `Inno.Rendering.IRenderLayerCompositionProgramProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.GraphicsPipelineDescriptor Inno.Rendering.IRenderLayerCompositionProgramProvider.CreateDescriptor(Inno.Rendering.GraphicsCapabilities capabilities, Inno.Rendering.RenderVertexLayout vertexLayout)`](../../src/services/rendering/Inno.Rendering/Device/IRenderLayerCompositionProgramProvider.cs#L20) | Creates a pipeline descriptor compatible with the supplied fullscreen vertex layout. |
| [`Inno.Rendering.GraphicsPipelineDescriptor Inno.Rendering.IRenderLayerCompositionProgramProvider.CreateOutputTransferDescriptor(Inno.Rendering.GraphicsCapabilities capabilities, Inno.Rendering.RenderVertexLayout vertexLayout)`](../../src/services/rendering/Inno.Rendering/Device/IRenderLayerCompositionProgramProvider.cs#L37) | Creates the final transfer pipeline for a presentation target without automatic sRGB encoding. |
| [`Inno.Rendering.IRenderLayerCompositionProgramProvider`](../../src/services/rendering/Inno.Rendering/Device/IRenderLayerCompositionProgramProvider.cs#L6) | Supplies a backend-specific graphics program for compositing ordered render layers. |

### `Inno.Rendering.PersistentBufferDescriptor`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.PersistentBufferDescriptor`](../../src/services/rendering/Inno.Rendering/Device/PersistentBufferDescriptor.cs#L10) | Describes a persistent buffer and any vertex/index interpretation required at creation. |
| [`Inno.Rendering.PersistentBufferDescriptor.PersistentBufferDescriptor(Inno.Rendering.RenderBufferDescriptor buffer, Inno.Rendering.RenderVertexLayout? vertexLayout = null, Inno.Rendering.RenderIndexFormat indexFormat = Inno.Rendering.RenderIndexFormat.UInt32)`](../../src/services/rendering/Inno.Rendering/Device/PersistentBufferDescriptor.cs#L24) | Creates a persistent buffer descriptor. |
| [`Inno.Rendering.RenderBufferDescriptor Inno.Rendering.PersistentBufferDescriptor.buffer`](../../src/services/rendering/Inno.Rendering/Device/PersistentBufferDescriptor.cs#L66) | Gets buffer capacity and usage. |
| [`Inno.Rendering.RenderIndexFormat Inno.Rendering.PersistentBufferDescriptor.indexFormat`](../../src/services/rendering/Inno.Rendering/Device/PersistentBufferDescriptor.cs#L76) | Gets the index representation when this is an index buffer. |
| [`Inno.Rendering.RenderVertexLayout? Inno.Rendering.PersistentBufferDescriptor.vertexLayout`](../../src/services/rendering/Inno.Rendering/Device/PersistentBufferDescriptor.cs#L71) | Gets the vertex layout when this is a vertex buffer. |

### `Inno.Rendering.PersistentBufferHandle`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.PersistentBufferHandle`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L515) | Identifies a persistent device buffer without exposing a backend-native handle. |
| [`bool Inno.Rendering.PersistentBufferHandle.isValid`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L531) | Gets whether the handle identifies a device buffer. |

### `Inno.Rendering.PersistentTextureHandle`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.PersistentTextureHandle`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L493) | Identifies a persistent device texture without exposing a backend-native handle. |
| [`bool Inno.Rendering.PersistentTextureHandle.isValid`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L509) | Gets whether the handle identifies a device texture. |

### `Inno.Rendering.RasterPassBuilder`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RasterPassBuilder`](../../src/services/rendering/Inno.Rendering/Graph/RasterPassBuilder.cs#L9) | Declares raster attachments and resource access. |
| [`Inno.Rendering.RasterPassBuilder Inno.Rendering.RasterPassBuilder.ClearPresentationTarget(Inno.Rendering.RenderClearColor clearColor)`](../../src/services/rendering/Inno.Rendering/Graph/RasterPassBuilder.cs#L71) | Clears the primary backbuffer or detached presentation surface before this pass. |
| [`Inno.Rendering.RasterPassBuilder Inno.Rendering.RasterPassBuilder.SetViewTransform(System.ReadOnlySpan<float> viewMatrix, System.ReadOnlySpan<float> projectionMatrix)`](../../src/services/rendering/Inno.Rendering/Graph/RasterPassBuilder.cs#L29) | Sets backend-ready column-major transform matrices for this raster view. |
| [`Inno.Rendering.RasterPassBuilder Inno.Rendering.RasterPassBuilder.UseColorAttachment(Inno.Rendering.RenderTextureHandle texture, int slot, Inno.Rendering.RenderLoadAction loadAction, Inno.Rendering.RenderStoreAction storeAction = Inno.Rendering.RenderStoreAction.Store, Inno.Rendering.RenderClearColor clearColor = default(Inno.Rendering.RenderClearColor), int mipLevel = 0, int arrayLayer = 0)`](../../src/services/rendering/Inno.Rendering/Graph/RasterPassBuilder.cs#L106) | Attaches a color texture. |
| [`Inno.Rendering.RasterPassBuilder Inno.Rendering.RasterPassBuilder.UseDepthAttachment(Inno.Rendering.RenderTextureHandle texture, Inno.Rendering.RenderLoadAction loadAction, Inno.Rendering.RenderStoreAction storeAction = Inno.Rendering.RenderStoreAction.Store, float clearDepth = 1, byte clearStencil = 0, int mipLevel = 0, int arrayLayer = 0)`](../../src/services/rendering/Inno.Rendering/Graph/RasterPassBuilder.cs#L159) | Attaches a depth or depth-stencil texture. |
| [`Inno.Rendering.RasterPassBuilder Inno.Rendering.RasterPassBuilder.UseSurface(Inno.Rendering.RenderSurfaceHandle surface)`](../../src/services/rendering/Inno.Rendering/Graph/RasterPassBuilder.cs#L50) | Directs this pass to a persistent presentation surface instead of the primary backbuffer. |

### `Inno.Rendering.RenderBindingId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderBindingId`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L8) | Identifies a shader binding by stable manifest name. |
| [`Inno.Rendering.RenderBindingId.RenderBindingId(string value)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L16) | Creates a stable shader binding identifier. |
| [`bool Inno.Rendering.RenderBindingId.isValid`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L30) | Gets whether the identifier contains a stable manifest name. |
| [`string Inno.Rendering.RenderBindingId.value`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L25) | Gets the stable manifest binding name. |

### `Inno.Rendering.RenderBlendEquation`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderBlendEquation`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L69) | Selects the arithmetic operation combining source and destination blend terms. |
| [`Inno.Rendering.RenderBlendEquation.Add`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L74) | Adds source and destination terms. |
| [`Inno.Rendering.RenderBlendEquation.Maximum`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L90) | Selects the component-wise maximum. |
| [`Inno.Rendering.RenderBlendEquation.Minimum`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L86) | Selects the component-wise minimum. |
| [`Inno.Rendering.RenderBlendEquation.ReverseSubtract`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L82) | Subtracts the source term from the destination term. |
| [`Inno.Rendering.RenderBlendEquation.Subtract`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L78) | Subtracts the destination term from the source term. |

### `Inno.Rendering.RenderBlendFactor`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderBlendFactor`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L10) | Selects one source or destination blend multiplier. |
| [`Inno.Rendering.RenderBlendFactor.Constant`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L59) | Multiplies by the packed constant blend color. |
| [`Inno.Rendering.RenderBlendFactor.DestinationAlpha`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L39) | Multiplies by destination alpha. |
| [`Inno.Rendering.RenderBlendFactor.DestinationColor`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L47) | Multiplies by destination color. |
| [`Inno.Rendering.RenderBlendFactor.InverseConstant`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L63) | Multiplies by one minus the packed constant blend color. |
| [`Inno.Rendering.RenderBlendFactor.InverseDestinationAlpha`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L43) | Multiplies by one minus destination alpha. |
| [`Inno.Rendering.RenderBlendFactor.InverseDestinationColor`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L51) | Multiplies by one minus destination color. |
| [`Inno.Rendering.RenderBlendFactor.InverseSourceAlpha`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L35) | Multiplies by one minus source alpha. |
| [`Inno.Rendering.RenderBlendFactor.InverseSourceColor`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L27) | Multiplies by one minus source color. |
| [`Inno.Rendering.RenderBlendFactor.One`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L19) | Multiplies by one. |
| [`Inno.Rendering.RenderBlendFactor.SourceAlpha`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L31) | Multiplies by source alpha. |
| [`Inno.Rendering.RenderBlendFactor.SourceAlphaSaturate`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L55) | Uses the saturated source-alpha factor. |
| [`Inno.Rendering.RenderBlendFactor.SourceColor`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L23) | Multiplies by source color. |
| [`Inno.Rendering.RenderBlendFactor.Zero`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L15) | Multiplies by zero. |

### `Inno.Rendering.RenderBlendState`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderBlendEquation Inno.Rendering.RenderBlendState.alphaEquation`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L179) | Gets or sets the alpha combination equation. |
| [`Inno.Rendering.RenderBlendEquation Inno.Rendering.RenderBlendState.colorEquation`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L164) | Gets or sets the RGB combination equation. |
| [`Inno.Rendering.RenderBlendFactor Inno.Rendering.RenderBlendState.alphaDestination`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L174) | Gets or sets the destination multiplier for alpha. |
| [`Inno.Rendering.RenderBlendFactor Inno.Rendering.RenderBlendState.alphaSource`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L169) | Gets or sets the source multiplier for alpha. |
| [`Inno.Rendering.RenderBlendFactor Inno.Rendering.RenderBlendState.colorDestination`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L159) | Gets or sets the destination multiplier for RGB channels. |
| [`Inno.Rendering.RenderBlendFactor Inno.Rendering.RenderBlendState.colorSource`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L154) | Gets or sets the source multiplier for RGB channels. |
| [`Inno.Rendering.RenderBlendState`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L96) | Describes independent color and alpha blending without backend-native flags. |
| [`bool Inno.Rendering.RenderBlendState.alphaToCoverage`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L189) | Gets or sets whether alpha-to-coverage is enabled. |
| [`bool Inno.Rendering.RenderBlendState.enabled`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L149) | Gets or sets whether blending is enabled. |
| [`static Inno.Rendering.RenderBlendState Inno.Rendering.RenderBlendState.additive`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L125) | Gets additive source-alpha blending. |
| [`static Inno.Rendering.RenderBlendState Inno.Rendering.RenderBlendState.alpha`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L113) | Gets conventional straight-alpha blending. |
| [`static Inno.Rendering.RenderBlendState Inno.Rendering.RenderBlendState.opaque`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L101) | Gets the disabled opaque blend state. |
| [`static Inno.Rendering.RenderBlendState Inno.Rendering.RenderBlendState.premultiplied`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L137) | Gets conventional premultiplied-alpha blending. |
| [`uint Inno.Rendering.RenderBlendState.constantRgba`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L184) | Gets or sets the packed RGBA8 constant used by constant blend factors. |

### `Inno.Rendering.RenderBufferDescriptor`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderBufferDescriptor`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L364) | Describes a render-graph buffer independently from a graphics backend. |
| [`Inno.Rendering.RenderBufferDescriptor.RenderBufferDescriptor(int elementCount, int elementStride, Inno.Rendering.RenderBufferUsage usage)`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L378) | Creates a buffer descriptor. |
| [`Inno.Rendering.RenderBufferUsage Inno.Rendering.RenderBufferDescriptor.usage`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L403) | Gets permitted operations. |
| [`bool Inno.Rendering.RenderBufferDescriptor.Equals(Inno.Rendering.RenderBufferDescriptor? other)`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L414) | Determines whether this instance and the supplied value represent the same logical state. |
| [`int Inno.Rendering.RenderBufferDescriptor.elementCount`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L393) | Gets the number of addressable elements. |
| [`int Inno.Rendering.RenderBufferDescriptor.elementStride`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L398) | Gets the element size in bytes. |
| [`override bool Inno.Rendering.RenderBufferDescriptor.Equals(object? obj)`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L429) | Determines whether this instance and the supplied value represent the same logical state. |
| [`override int Inno.Rendering.RenderBufferDescriptor.GetHashCode()`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L437) | Computes a hash code from the fields that participate in logical equality. |

### `Inno.Rendering.RenderBufferHandle`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderBufferHandle`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L468) | Identifies a frame-scoped render-graph buffer. |
| [`bool Inno.Rendering.RenderBufferHandle.isValid`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L487) | Gets whether the handle was created by a render graph. |

### `Inno.Rendering.RenderBufferSlice`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderBufferSlice`](../../src/services/rendering/Inno.Rendering/Device/RenderBufferUploads.cs#L84) | Identifies one range in a frame upload page without exposing its persistent backing buffer. |
| [`Inno.Rendering.RenderBufferUsage Inno.Rendering.RenderBufferSlice.usage`](../../src/services/rendering/Inno.Rendering/Device/RenderBufferUploads.cs#L116) | Gets permitted GPU uses for this slice. |
| [`bool Inno.Rendering.RenderBufferSlice.isValid`](../../src/services/rendering/Inno.Rendering/Device/RenderBufferUploads.cs#L121) | Gets whether this slice was produced by a frame upload service. |
| [`int Inno.Rendering.RenderBufferSlice.elementCount`](../../src/services/rendering/Inno.Rendering/Device/RenderBufferUploads.cs#L111) | Gets the number of uploaded elements. |
| [`int Inno.Rendering.RenderBufferSlice.firstElement`](../../src/services/rendering/Inno.Rendering/Device/RenderBufferUploads.cs#L106) | Gets the first uploaded element in the backing frame page. |

### `Inno.Rendering.RenderBufferUploadDescriptor`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderBufferUploadDescriptor`](../../src/services/rendering/Inno.Rendering/Device/RenderBufferUploads.cs#L8) | Describes one CPU-to-GPU buffer upload that remains valid for the current frame only. |
| [`Inno.Rendering.RenderBufferUploadDescriptor.RenderBufferUploadDescriptor(int elementStride, Inno.Rendering.RenderBufferUsage usage, Inno.Rendering.RenderVertexLayout? vertexLayout = null, Inno.Rendering.RenderIndexFormat indexFormat = Inno.Rendering.RenderIndexFormat.UInt32)`](../../src/services/rendering/Inno.Rendering/Device/RenderBufferUploads.cs#L25) | Creates a frame upload descriptor. |
| [`Inno.Rendering.RenderBufferUsage Inno.Rendering.RenderBufferUploadDescriptor.usage`](../../src/services/rendering/Inno.Rendering/Device/RenderBufferUploads.cs#L68) | Gets permitted GPU uses. |
| [`Inno.Rendering.RenderIndexFormat Inno.Rendering.RenderBufferUploadDescriptor.indexFormat`](../../src/services/rendering/Inno.Rendering/Device/RenderBufferUploads.cs#L78) | Gets the index representation when index usage is present. |
| [`Inno.Rendering.RenderVertexLayout? Inno.Rendering.RenderBufferUploadDescriptor.vertexLayout`](../../src/services/rendering/Inno.Rendering/Device/RenderBufferUploads.cs#L73) | Gets the vertex layout when vertex usage is present. |
| [`int Inno.Rendering.RenderBufferUploadDescriptor.elementStride`](../../src/services/rendering/Inno.Rendering/Device/RenderBufferUploads.cs#L63) | Gets the element size in bytes. |

### `Inno.Rendering.RenderBufferUsage`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderBufferUsage`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L110) | Declares intended buffer operations for capability and hazard validation. |
| [`Inno.Rendering.RenderBufferUsage.CopyDestination`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L140) | Copy operation destination. |
| [`Inno.Rendering.RenderBufferUsage.CopySource`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L136) | Copy operation source. |
| [`Inno.Rendering.RenderBufferUsage.Dynamic`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L144) | Contents may be replaced at frame safety points. |
| [`Inno.Rendering.RenderBufferUsage.Index`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L120) | Index input data. |
| [`Inno.Rendering.RenderBufferUsage.Indirect`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L132) | Indirect command data. |
| [`Inno.Rendering.RenderBufferUsage.Storage`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L128) | Shader-readable and writable storage data. |
| [`Inno.Rendering.RenderBufferUsage.Uniform`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L124) | Read-only shader data. |
| [`Inno.Rendering.RenderBufferUsage.Vertex`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L116) | Vertex input data. |

### `Inno.Rendering.RenderClearColor`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderClearColor`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L109) | Stores a linear clear color without depending on an engine math assembly. |
| [`Inno.Rendering.RenderClearColor.RenderClearColor(float r, float g, float b, float a = 1)`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L126) | Creates a linear clear color. |
| [`float Inno.Rendering.RenderClearColor.a`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L156) | Gets the alpha channel. |
| [`float Inno.Rendering.RenderClearColor.b`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L151) | Gets the blue channel. |
| [`float Inno.Rendering.RenderClearColor.g`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L146) | Gets the green channel. |
| [`float Inno.Rendering.RenderClearColor.r`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L141) | Gets the red channel. |

### `Inno.Rendering.RenderCommandEncoder`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderCommandEncoder`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L170) | Provides backend-neutral draw, dispatch and copy commands for one compiled pass. |
| [`abstract void Inno.Rendering.RenderCommandEncoder.BindBuffer(Inno.Rendering.RenderBindingId binding, Inno.Rendering.PersistentBufferHandle buffer)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L316) | Binds a persistent storage buffer to a shader interface slot. |
| [`abstract void Inno.Rendering.RenderCommandEncoder.BindBuffer(Inno.Rendering.RenderBindingId binding, Inno.Rendering.RenderBufferHandle buffer)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L302) | Binds a graph buffer to a shader interface slot. |
| [`abstract void Inno.Rendering.RenderCommandEncoder.BindComputePipeline(Inno.Rendering.ComputePipelineHandle pipeline)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L189) | Binds a compute pipeline for subsequent dispatch commands. |
| [`abstract void Inno.Rendering.RenderCommandEncoder.BindGraphicsPipeline(Inno.Rendering.GraphicsPipelineHandle pipeline)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L181) | Binds a graphics pipeline for subsequent draw commands. |
| [`abstract void Inno.Rendering.RenderCommandEncoder.BindIndexBuffer(Inno.Rendering.PersistentBufferHandle buffer, int firstIndex = 0)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L489) | Binds a persistent buffer as index input. |
| [`abstract void Inno.Rendering.RenderCommandEncoder.BindIndexBuffer(Inno.Rendering.RenderBufferHandle buffer, int firstIndex = 0)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L475) | Binds a graph buffer as index input. |
| [`abstract void Inno.Rendering.RenderCommandEncoder.BindInstanceBuffer(Inno.Rendering.PersistentBufferHandle buffer, int firstInstance, int instanceCount)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L536) | Binds persistent buffer elements as per-instance input. |
| [`abstract void Inno.Rendering.RenderCommandEncoder.BindInstanceBuffer(Inno.Rendering.RenderBufferHandle buffer, int firstInstance, int instanceCount)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L518) | Binds graph buffer elements as per-instance input. |
| [`abstract void Inno.Rendering.RenderCommandEncoder.BindStorageTexture(Inno.Rendering.RenderBindingId binding, Inno.Rendering.PersistentTextureHandle texture, int mipLevel = 0)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L287) | Binds a persistent texture for shader storage access. |
| [`abstract void Inno.Rendering.RenderCommandEncoder.BindStorageTexture(Inno.Rendering.RenderBindingId binding, Inno.Rendering.RenderTextureHandle texture, int mipLevel = 0)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L269) | Binds a graph texture for shader storage access. |
| [`abstract void Inno.Rendering.RenderCommandEncoder.BindTexture(Inno.Rendering.RenderBindingId binding, Inno.Rendering.PersistentTextureHandle texture, Inno.Rendering.RenderSamplerState sampler)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L251) | Binds a persistent texture and explicit sampler state. |
| [`abstract void Inno.Rendering.RenderCommandEncoder.BindTexture(Inno.Rendering.RenderBindingId binding, Inno.Rendering.RenderTextureHandle texture, Inno.Rendering.RenderSamplerState sampler)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L218) | Binds a graph texture and explicit sampler state. |
| [`abstract void Inno.Rendering.RenderCommandEncoder.BindVertexBuffer(Inno.Rendering.PersistentBufferHandle buffer, int firstVertex = 0)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L449) | Binds a persistent buffer as vertex input. |
| [`abstract void Inno.Rendering.RenderCommandEncoder.BindVertexBuffer(Inno.Rendering.RenderBufferHandle buffer, int firstVertex = 0)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L435) | Binds a graph buffer as vertex input. |
| [`abstract void Inno.Rendering.RenderCommandEncoder.BlitTexture(Inno.Rendering.RenderTextureHandle source, Inno.Rendering.RenderTextureRegion sourceRegion, Inno.Rendering.RenderTextureHandle destination, Inno.Rendering.RenderTextureRegion destinationRegion)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L715) | Blits compatible texture regions without CPU readback. |
| [`abstract void Inno.Rendering.RenderCommandEncoder.CopyBuffer(Inno.Rendering.RenderBufferHandle source, Inno.Rendering.RenderBufferHandle destination)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L734) | Copies complete compatible graph buffer ranges. |
| [`abstract void Inno.Rendering.RenderCommandEncoder.CopyTexture(Inno.Rendering.RenderTextureHandle source, Inno.Rendering.RenderTextureHandle destination)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L695) | Copies all compatible subresources between graph textures. |
| [`abstract void Inno.Rendering.RenderCommandEncoder.Dispatch(int groupCountX, int groupCountY = 1, int groupCountZ = 1)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L644) | Dispatches compute workgroups. |
| [`abstract void Inno.Rendering.RenderCommandEncoder.DispatchIndirect(Inno.Rendering.PersistentBufferHandle buffer, int firstCommand = 0, int commandCount = 1)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L680) | Dispatches compute commands stored in an indirect persistent buffer. |
| [`abstract void Inno.Rendering.RenderCommandEncoder.DispatchIndirect(Inno.Rendering.RenderBufferHandle buffer, int firstCommand = 0, int commandCount = 1)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L662) | Dispatches compute commands stored in an indirect graph buffer. |
| [`abstract void Inno.Rendering.RenderCommandEncoder.Draw(int vertexCount, int instanceCount = 1)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L563) | Issues a non-indexed draw. |
| [`abstract void Inno.Rendering.RenderCommandEncoder.DrawIndexed(int indexCount, int instanceCount = 1)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L591) | Issues an indexed draw. |
| [`abstract void Inno.Rendering.RenderCommandEncoder.DrawIndirect(Inno.Rendering.PersistentBufferHandle buffer, int firstCommand = 0, int commandCount = 1)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L626) | Issues graphics commands stored in an indirect persistent buffer. |
| [`abstract void Inno.Rendering.RenderCommandEncoder.DrawIndirect(Inno.Rendering.RenderBufferHandle buffer, int firstCommand = 0, int commandCount = 1)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L608) | Issues graphics commands stored in an indirect graph buffer. |
| [`abstract void Inno.Rendering.RenderCommandEncoder.DrawProcedural(int vertexCount, int instanceCount = 1)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L577) | Issues a procedural non-indexed draw that does not consume a vertex buffer. |
| [`abstract void Inno.Rendering.RenderCommandEncoder.SetRasterState(Inno.Rendering.RenderRasterState state)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L372) | Overrides raster state for subsequent draws in the current pass. |
| [`abstract void Inno.Rendering.RenderCommandEncoder.SetScissor(int x, int y, int width, int height)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L419) | Restricts subsequent rasterization to a pixel rectangle in the active view. |
| [`abstract void Inno.Rendering.RenderCommandEncoder.SetStencil(Inno.Rendering.RenderStencilState state)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L380) | Sets two-sided stencil state for subsequent draws. |
| [`abstract void Inno.Rendering.RenderCommandEncoder.SetTransform(System.ReadOnlySpan<float> columnMajorMatrix)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L364) | Sets the current object transform from one column-major 4x4 matrix. |
| [`abstract void Inno.Rendering.RenderCommandEncoder.SetUniform(Inno.Rendering.RenderBindingId binding, System.ReadOnlySpan<byte> value)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L353) | Uploads one uniform value using manifest-validated bytes. |
| [`abstract void Inno.Rendering.RenderCommandEncoder.SetViewport(int x, int y, int width, int height)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L397) | Changes the viewport rectangle for the current pass view. |
| [`void Inno.Rendering.RenderCommandEncoder.BindBuffer(Inno.Rendering.RenderBindingId binding, Inno.Rendering.RenderBufferSlice buffer)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L330) | Binds a frame-uploaded storage buffer to a shader interface slot. |
| [`void Inno.Rendering.RenderCommandEncoder.BindIndexBuffer(Inno.Rendering.RenderBufferSlice buffer)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L500) | Binds a frame-uploaded index slice. |
| [`void Inno.Rendering.RenderCommandEncoder.BindInstanceBuffer(Inno.Rendering.RenderBufferSlice buffer)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L548) | Binds a frame-uploaded vertex slice as per-instance input. |
| [`void Inno.Rendering.RenderCommandEncoder.BindTexture(Inno.Rendering.RenderBindingId binding, Inno.Rendering.PersistentTextureHandle texture)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L233) | Binds a persistent texture to a shader interface slot. |
| [`void Inno.Rendering.RenderCommandEncoder.BindTexture(Inno.Rendering.RenderBindingId binding, Inno.Rendering.RenderTextureHandle texture)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L200) | Binds a graph texture to a shader interface slot. |
| [`void Inno.Rendering.RenderCommandEncoder.BindVertexBuffer(Inno.Rendering.RenderBufferSlice buffer)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L460) | Binds a frame-uploaded vertex slice. |

### `Inno.Rendering.RenderCullMode`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderCullMode`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L10) | Selects triangle culling for a backend-neutral raster pipeline. |
| [`Inno.Rendering.RenderCullMode.Back`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L23) | Culls back-facing triangles. |
| [`Inno.Rendering.RenderCullMode.Front`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L19) | Culls front-facing triangles. |
| [`Inno.Rendering.RenderCullMode.None`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L15) | Disables face culling. |

### `Inno.Rendering.RenderDataChannelId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderDataChannelId`](../../src/services/rendering/Inno.Rendering/Device/RenderProtocolIds.cs#L44) | Identifies one pipeline-owned frame-data channel without constraining its payload model. |
| [`Inno.Rendering.RenderDataChannelId.RenderDataChannelId(string value)`](../../src/services/rendering/Inno.Rendering/Device/RenderProtocolIds.cs#L52) | Creates an open frame-data channel identifier. |
| [`bool Inno.Rendering.RenderDataChannelId.isValid`](../../src/services/rendering/Inno.Rendering/Device/RenderProtocolIds.cs#L66) | Gets whether the identifier contains a protocol value. |
| [`override string Inno.Rendering.RenderDataChannelId.ToString()`](../../src/services/rendering/Inno.Rendering/Device/RenderProtocolIds.cs#L74) | Formats this value as a human-readable representation. |
| [`string Inno.Rendering.RenderDataChannelId.value`](../../src/services/rendering/Inno.Rendering/Device/RenderProtocolIds.cs#L61) | Gets the globally stable protocol value. |

### `Inno.Rendering.RenderDepthCompare`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderDepthCompare`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L44) | Selects depth comparison for a backend-neutral raster pipeline. |
| [`Inno.Rendering.RenderDepthCompare.Always`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L77) | Always accepts. |
| [`Inno.Rendering.RenderDepthCompare.Equal`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L57) | Accepts equal depth. |
| [`Inno.Rendering.RenderDepthCompare.Greater`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L65) | Accepts greater depth. |
| [`Inno.Rendering.RenderDepthCompare.GreaterEqual`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L73) | Accepts greater or equal depth. |
| [`Inno.Rendering.RenderDepthCompare.Less`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L53) | Accepts smaller depth. |
| [`Inno.Rendering.RenderDepthCompare.LessEqual`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L61) | Accepts smaller or equal depth. |
| [`Inno.Rendering.RenderDepthCompare.Never`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L49) | Always rejects. |
| [`Inno.Rendering.RenderDepthCompare.NotEqual`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L69) | Accepts unequal depth. |

### `Inno.Rendering.RenderDevice`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderDevice`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L10) | Provides the protected opaque-handle boundary required by replaceable render-device backends. |
| [`static Inno.Rendering.ComputePipelineHandle Inno.Rendering.RenderDevice.CreateComputePipelineHandle(ulong value, uint generation)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L103) | Encodes a compute pipeline identity into a backend-neutral handle. |
| [`static Inno.Rendering.GraphicsPipelineHandle Inno.Rendering.RenderDevice.CreateGraphicsPipelineHandle(ulong value, uint generation)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L86) | Encodes a graphics pipeline identity into a backend-neutral handle. |
| [`static Inno.Rendering.PersistentBufferHandle Inno.Rendering.RenderDevice.CreatePersistentBufferHandle(ulong value, uint generation)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L69) | Encodes a persistent buffer identity into a backend-neutral handle. |
| [`static Inno.Rendering.PersistentTextureHandle Inno.Rendering.RenderDevice.CreatePersistentTextureHandle(ulong value, uint generation)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L52) | Encodes a persistent texture identity into a backend-neutral handle. |
| [`static Inno.Rendering.RenderDevice.DeviceHandleIdentity Inno.Rendering.RenderDevice.GetHandleIdentity(Inno.Rendering.ComputePipelineHandle handle)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L184) | Decodes a compute pipeline handle for backend lookup and generation validation. |
| [`static Inno.Rendering.RenderDevice.DeviceHandleIdentity Inno.Rendering.RenderDevice.GetHandleIdentity(Inno.Rendering.GraphicsPipelineHandle handle)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L173) | Decodes a graphics pipeline handle for backend lookup and generation validation. |
| [`static Inno.Rendering.RenderDevice.DeviceHandleIdentity Inno.Rendering.RenderDevice.GetHandleIdentity(Inno.Rendering.PersistentBufferHandle handle)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L162) | Decodes a persistent buffer handle for backend lookup and generation validation. |
| [`static Inno.Rendering.RenderDevice.DeviceHandleIdentity Inno.Rendering.RenderDevice.GetHandleIdentity(Inno.Rendering.PersistentTextureHandle handle)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L151) | Decodes a persistent texture handle for backend lookup and generation validation. |
| [`static Inno.Rendering.RenderDevice.DeviceHandleIdentity Inno.Rendering.RenderDevice.GetHandleIdentity(Inno.Rendering.RenderSurfaceHandle handle)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L195) | Decodes a presentation-surface handle for backend lookup and generation validation. |
| [`static Inno.Rendering.RenderDevice.DeviceHandleIdentity Inno.Rendering.RenderDevice.GetHandleIdentity(Inno.Rendering.RenderTextureReadbackHandle handle)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L206) | Decodes a texture-readback handle for backend lookup and generation validation. |
| [`static Inno.Rendering.RenderDevice.GraphHandleIdentity Inno.Rendering.RenderDevice.GetHandleIdentity(Inno.Rendering.RenderBufferHandle handle)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L229) | Decodes a frame-scoped buffer handle for graph resource lookup. |
| [`static Inno.Rendering.RenderDevice.GraphHandleIdentity Inno.Rendering.RenderDevice.GetHandleIdentity(Inno.Rendering.RenderTextureHandle handle)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L218) | Decodes a frame-scoped texture handle for graph resource lookup. |
| [`static Inno.Rendering.RenderSurfaceHandle Inno.Rendering.RenderDevice.CreateRenderSurfaceHandle(ulong value, uint generation)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L120) | Encodes a presentation-surface identity into a backend-neutral handle. |
| [`static Inno.Rendering.RenderTextureReadbackHandle Inno.Rendering.RenderDevice.CreateRenderTextureReadbackHandle(ulong value, uint generation)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L137) | Encodes a texture-readback identity into a backend-neutral handle. |

### `Inno.Rendering.RenderDevice.DeviceHandleIdentity`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderDevice.DeviceHandleIdentity`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L21) | Stores one decoded persistent device identity for use inside a concrete backend. |

### `Inno.Rendering.RenderDevice.GraphHandleIdentity`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderDevice.GraphHandleIdentity`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L35) | Stores one decoded frame-scoped graph identity for use inside a concrete backend. |

### `Inno.Rendering.RenderDeviceAllocationCounters`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderDeviceAllocationCounters`](../../src/services/rendering/Inno.Rendering/Device/RenderDeviceAllocationCounters.cs#L10) | Reports cumulative successful native allocations made by a device's transient graph resource pools. Pool reuse does not increment these counters; retirement does not decrement them. They are not live resource counts, managed allocation counts, byte measurements or per-frame command counts. |
| [`Inno.Rendering.RenderDeviceAllocationCounters.RenderDeviceAllocationCounters(uint deviceGeneration, ulong textureAllocations, ulong bufferAllocations, ulong frameBufferAllocations)`](../../src/services/rendering/Inno.Rendering/Device/RenderDeviceAllocationCounters.cs#L30) | Creates an immutable allocation snapshot scoped to one device generation. |
| [`uint Inno.Rendering.RenderDeviceAllocationCounters.deviceGeneration`](../../src/services/rendering/Inno.Rendering/Device/RenderDeviceAllocationCounters.cs#L46) | Gets the device generation whose cumulative counters are represented by this snapshot. |
| [`ulong Inno.Rendering.RenderDeviceAllocationCounters.bufferAllocations`](../../src/services/rendering/Inno.Rendering/Device/RenderDeviceAllocationCounters.cs#L56) | Gets successful native transient buffer allocations since device creation. |
| [`ulong Inno.Rendering.RenderDeviceAllocationCounters.frameBufferAllocations`](../../src/services/rendering/Inno.Rendering/Device/RenderDeviceAllocationCounters.cs#L61) | Gets successful native transient attachment framebuffer allocations since device creation. |
| [`ulong Inno.Rendering.RenderDeviceAllocationCounters.textureAllocations`](../../src/services/rendering/Inno.Rendering/Device/RenderDeviceAllocationCounters.cs#L51) | Gets successful native transient texture allocations since device creation. |

### `Inno.Rendering.RenderDeviceFrameCounters`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderDeviceFrameCounters`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L19) | Contains backend-recorded GPU command submissions for the current frame. |
| [`Inno.Rendering.RenderDeviceFrameCounters.RenderDeviceFrameCounters(int drawCount, int dispatchCount)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L30) | Creates immutable device frame counters. |
| [`int Inno.Rendering.RenderDeviceFrameCounters.dispatchCount`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L48) | Gets direct and indirect compute dispatches submitted during the current frame. |
| [`int Inno.Rendering.RenderDeviceFrameCounters.drawCount`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L43) | Gets direct and indirect draw commands submitted during the current frame. |

### `Inno.Rendering.RenderFrameUploadProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderFrameUploadProvider`](../../src/services/rendering/Inno.Rendering/Device/RenderBufferUploads.cs#L127) | Provides protected slice construction for replaceable frame-upload service implementations. |
| [`static Inno.Rendering.RenderBufferSlice Inno.Rendering.RenderFrameUploadProvider.CreateBufferSlice(Inno.Rendering.PersistentBufferHandle buffer, int firstElement, int elementCount, Inno.Rendering.RenderBufferUsage usage, ulong frameIndex)`](../../src/services/rendering/Inno.Rendering/Device/RenderBufferUploads.cs#L150) | Creates a frame-scoped slice over a provider-owned persistent upload buffer. |

### `Inno.Rendering.RenderFrontFace`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderFrontFace`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L29) | Selects the winding order interpreted as the front face. |
| [`Inno.Rendering.RenderFrontFace.Clockwise`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L34) | Clockwise vertices form a front-facing triangle. |
| [`Inno.Rendering.RenderFrontFace.CounterClockwise`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L38) | Counter-clockwise vertices form a front-facing triangle. |

### `Inno.Rendering.RenderGraphBuilder`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.ComputePassBuilder Inno.Rendering.RenderGraphBuilder.AddComputePass<TPassData>(string name, Inno.Rendering.RenderPhaseId phase, TPassData passData, Inno.Rendering.RenderPassExecute<TPassData> execute)`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphBuilder.cs#L277) | Adds a frame-scoped compute pass. |
| [`Inno.Rendering.CopyPassBuilder Inno.Rendering.RenderGraphBuilder.AddCopyPass<TPassData>(string name, Inno.Rendering.RenderPhaseId phase, TPassData passData, Inno.Rendering.RenderPassExecute<TPassData> execute)`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphBuilder.cs#L307) | Adds a frame-scoped resource copy pass. |
| [`Inno.Rendering.RasterPassBuilder Inno.Rendering.RenderGraphBuilder.AddRasterPass<TPassData>(string name, Inno.Rendering.RenderPhaseId phase, TPassData passData, Inno.Rendering.RenderPassExecute<TPassData> execute)`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphBuilder.cs#L247) | Adds a frame-scoped raster pass. |
| [`Inno.Rendering.RenderBufferHandle Inno.Rendering.RenderGraphBuilder.CreateBuffer(string name, Inno.Rendering.RenderBufferDescriptor descriptor)`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphBuilder.cs#L122) | Creates a transient buffer eligible for lifetime aliasing. |
| [`Inno.Rendering.RenderBufferHandle Inno.Rendering.RenderGraphBuilder.ImportBuffer(string name, Inno.Rendering.PersistentBufferHandle buffer, Inno.Rendering.RenderBufferDescriptor descriptor)`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphBuilder.cs#L149) | Imports a persistent device buffer into the current graph. |
| [`Inno.Rendering.RenderGraphBuilder`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphBuilder.cs#L11) | Builds one generation-scoped render graph from explicit passes and resources. |
| [`Inno.Rendering.RenderGraphBuilder.RenderGraphBuilder(uint generation, Inno.Rendering.GraphicsCapabilities capabilities)`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphBuilder.cs#L39) | Creates a render graph builder for one frame generation. |
| [`Inno.Rendering.RenderGraphCompileResult Inno.Rendering.RenderGraphBuilder.Compile()`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphBuilder.cs#L322) | Validates, culls and schedules this graph exactly once. |
| [`Inno.Rendering.RenderGraphMutationScope Inno.Rendering.RenderGraphBuilder.BeginMutationScope()`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphBuilder.cs#L197) | Begins an isolated graph mutation that rolls back every added pass and resource unless committed. |
| [`Inno.Rendering.RenderGraphNameScope Inno.Rendering.RenderGraphBuilder.BeginNameScope(string name)`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphBuilder.cs#L217) | Prefixes pass and resource diagnostic names until the returned scope is disposed. |
| [`Inno.Rendering.RenderGraphValidationResult Inno.Rendering.RenderGraphBuilder.Validate()`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphBuilder.cs#L339) | Validates current declarations and caches their analysis without constructing an executable graph. |
| [`Inno.Rendering.RenderTextureHandle Inno.Rendering.RenderGraphBuilder.CreateTexture(string name, Inno.Rendering.RenderTextureDescriptor descriptor)`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphBuilder.cs#L65) | Creates a transient texture eligible for lifetime aliasing. |
| [`Inno.Rendering.RenderTextureHandle Inno.Rendering.RenderGraphBuilder.ImportTexture(string name, Inno.Rendering.PersistentTextureHandle texture, Inno.Rendering.RenderTextureDescriptor descriptor)`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphBuilder.cs#L92) | Imports a persistent device texture into the current graph. |
| [`void Inno.Rendering.RenderGraphBuilder.MarkOutput(Inno.Rendering.RenderBufferHandle buffer)`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphBuilder.cs#L185) | Marks a buffer as a graph output that keeps its producers alive. |
| [`void Inno.Rendering.RenderGraphBuilder.MarkOutput(Inno.Rendering.RenderTextureHandle texture)`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphBuilder.cs#L173) | Marks a texture as a graph output that keeps its producers alive. |

### `Inno.Rendering.RenderGraphCompileResult`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.CompiledRenderGraph? Inno.Rendering.RenderGraphCompileResult.graph`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L531) | Gets the executable graph, or when compilation failed. |
| [`Inno.Rendering.RenderGraphCompileResult`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L513) | Contains render-graph compilation diagnostics and an optional executable graph. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.RenderGraphDiagnostic> Inno.Rendering.RenderGraphCompileResult.diagnostics`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L536) | Gets deterministic compilation diagnostics. |
| [`bool Inno.Rendering.RenderGraphCompileResult.succeeded`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L546) | Gets whether an executable graph was produced. |
| [`int Inno.Rendering.RenderGraphCompileResult.culledPassCount`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L541) | Gets passes removed because they did not contribute to an output or side effect. |

### `Inno.Rendering.RenderGraphDiagnostic`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Diagnostics.DiagnosticSeverity Inno.Rendering.RenderGraphDiagnostic.severity`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L211) | Gets diagnostic impact. |
| [`Inno.Rendering.RenderGraphDiagnostic`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L162) | Reports one structured render-graph compilation problem. |
| [`Inno.Rendering.RenderGraphDiagnostic.RenderGraphDiagnostic(string code, string message, Inno.Core.Diagnostics.DiagnosticSeverity severity, string? passName = null, string? resourceName = null)`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L182) | Creates a render-graph diagnostic. |
| [`string Inno.Rendering.RenderGraphDiagnostic.code`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L201) | Gets the stable machine-readable code. |
| [`string Inno.Rendering.RenderGraphDiagnostic.message`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L206) | Gets actionable diagnostic text. |
| [`string? Inno.Rendering.RenderGraphDiagnostic.passName`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L216) | Gets the related pass name, if any. |
| [`string? Inno.Rendering.RenderGraphDiagnostic.resourceName`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L221) | Gets the related resource name, if any. |

### `Inno.Rendering.RenderGraphMutationScope`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderGraphMutationScope`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphMutationScope.cs#L9) | Owns isolated graph additions; earlier pass declarations are frozen and failure removes every addition. |
| [`void Inno.Rendering.RenderGraphMutationScope.Commit()`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphMutationScope.cs#L40) | Validates and freezes additions in the current scope. Dispose must still end the scope before new work. |
| [`void Inno.Rendering.RenderGraphMutationScope.Dispose()`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphMutationScope.cs#L52) | Ends the current scope and removes all uncommitted additions, including output and name declarations. |

### `Inno.Rendering.RenderGraphNameScope`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderGraphNameScope`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphNameScope.cs#L9) | Owns one nested diagnostic-name prefix in a render graph builder. |
| [`void Inno.Rendering.RenderGraphNameScope.Dispose()`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphNameScope.cs#L25) | Ends the name prefix scope. |

### `Inno.Rendering.RenderGraphValidationResult`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderGraphValidationResult`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphValidationResult.cs#L10) | Describes graph validity and scheduling counts without creating resources or executable callbacks. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.RenderGraphDiagnostic> Inno.Rendering.RenderGraphValidationResult.diagnostics`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphValidationResult.cs#L36) | Gets frozen deterministic diagnostics for the validated revision. |
| [`bool Inno.Rendering.RenderGraphValidationResult.isValid`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphValidationResult.cs#L31) | Gets whether the current graph satisfies device, dependency and presentation limits. |
| [`int Inno.Rendering.RenderGraphValidationResult.bufferCount`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphValidationResult.cs#L61) | Gets the number of declared transient and imported buffers. |
| [`int Inno.Rendering.RenderGraphValidationResult.culledPassCount`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphValidationResult.cs#L51) | Gets passes removed by valid output and side-effect analysis; returns zero for an invalid graph. |
| [`int Inno.Rendering.RenderGraphValidationResult.passCount`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphValidationResult.cs#L41) | Gets the number of declared passes, including passes not contributing to an output. |
| [`int Inno.Rendering.RenderGraphValidationResult.scheduledPassCount`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphValidationResult.cs#L46) | Gets scheduled passes, or zero when validation could not produce a valid schedule. |
| [`int Inno.Rendering.RenderGraphValidationResult.textureCount`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphValidationResult.cs#L56) | Gets the number of declared transient and imported textures. |

### `Inno.Rendering.RenderIndexFormat`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderIndexFormat`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L340) | Selects the integer representation of an index buffer. |
| [`Inno.Rendering.RenderIndexFormat.UInt16`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L345) | Unsigned 16-bit indices. |
| [`Inno.Rendering.RenderIndexFormat.UInt32`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L349) | Unsigned 32-bit indices. |

### `Inno.Rendering.RenderLoadAction`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderLoadAction`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L75) | Controls how existing attachment contents enter a raster pass. |
| [`Inno.Rendering.RenderLoadAction.Clear`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L84) | Clears the attachment before rendering. |
| [`Inno.Rendering.RenderLoadAction.Discard`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L88) | Does not require prior attachment contents. |
| [`Inno.Rendering.RenderLoadAction.Load`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L80) | Preserves prior attachment contents. |

### `Inno.Rendering.RenderPassBuilder`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderPassBuilder`](../../src/services/rendering/Inno.Rendering/Graph/RenderPassBuilder.cs#L9) | Provides ordering and common resource declarations for one pass. |
| [`Inno.Rendering.RenderPassBuilder Inno.Rendering.RenderPassBuilder.After(Inno.Rendering.RenderPhaseId phase)`](../../src/services/rendering/Inno.Rendering/Graph/RenderPassBuilder.cs#L50) | Orders this pass after every pass in a target phase. |
| [`Inno.Rendering.RenderPassBuilder Inno.Rendering.RenderPassBuilder.AllowParallelRecording()`](../../src/services/rendering/Inno.Rendering/Graph/RenderPassBuilder.cs#L80) | Opts this pass into worker-thread recording through an isolated backend-neutral command list. |
| [`Inno.Rendering.RenderPassBuilder Inno.Rendering.RenderPassBuilder.Before(Inno.Rendering.RenderPhaseId phase)`](../../src/services/rendering/Inno.Rendering/Graph/RenderPassBuilder.cs#L34) | Orders this pass before every pass in a target phase. |
| [`Inno.Rendering.RenderPassBuilder Inno.Rendering.RenderPassBuilder.HasSideEffect()`](../../src/services/rendering/Inno.Rendering/Graph/RenderPassBuilder.cs#L63) | Prevents pass culling because execution has an externally observable effect. |
| [`Inno.Rendering.RenderPassBuilder Inno.Rendering.RenderPassBuilder.ReadBuffer(Inno.Rendering.RenderBufferHandle buffer)`](../../src/services/rendering/Inno.Rendering/Graph/RenderPassBuilder.cs#L115) | Declares a shader read from a buffer. |
| [`Inno.Rendering.RenderPassBuilder Inno.Rendering.RenderPassBuilder.ReadTexture(Inno.Rendering.RenderTextureHandle texture)`](../../src/services/rendering/Inno.Rendering/Graph/RenderPassBuilder.cs#L96) | Declares a shader read from a texture. |

### `Inno.Rendering.RenderPassContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderCommandEncoder Inno.Rendering.RenderPassContext.commands`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L782) | Gets the command encoder scoped to the current pass. |
| [`Inno.Rendering.RenderPassContext`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L769) | Supplies the command encoder and immutable frame identity to a pass callback. |
| [`ulong Inno.Rendering.RenderPassContext.frameIndex`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L787) | Gets the monotonic frame index. |

### `Inno.Rendering.RenderPassExecute<TPassData>`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderPassExecute<TPassData>`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L802) | Executes one frame-scoped pass payload. |

### `Inno.Rendering.RenderPassKind`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderPassKind`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L41) | Distinguishes pass command domains for validation and backend execution. |
| [`Inno.Rendering.RenderPassKind.Compute`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L50) | Compute dispatch commands. |
| [`Inno.Rendering.RenderPassKind.Copy`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L54) | Resource copy commands. |
| [`Inno.Rendering.RenderPassKind.Raster`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L46) | Raster draw commands with optional attachments. |

### `Inno.Rendering.RenderPassRecordingMode`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderPassRecordingMode`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L60) | Controls where a frame-local pass callback records backend-neutral commands. |
| [`Inno.Rendering.RenderPassRecordingMode.Parallel`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L69) | Records into an isolated command list on a worker thread, then replays in graph order. |
| [`Inno.Rendering.RenderPassRecordingMode.Serial`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L65) | Records directly on the graph execution thread. |

### `Inno.Rendering.RenderPhaseId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderPhaseId`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L10) | Identifies an open render phase protocol value. |
| [`Inno.Rendering.RenderPhaseId.RenderPhaseId(string value)`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L18) | Creates an open render phase identifier. |
| [`override string Inno.Rendering.RenderPhaseId.ToString()`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L35) | Formats this value as a human-readable representation. |
| [`string Inno.Rendering.RenderPhaseId.value`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L27) | Gets the globally stable phase value. |

### `Inno.Rendering.RenderPresentationSize`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderPresentationSize`](../../src/services/rendering/Inno.Rendering/Device/RenderPresentationSize.cs#L8) | Stores one backend-neutral presentation extent in physical pixels. |
| [`Inno.Rendering.RenderPresentationSize.RenderPresentationSize(int width, int height)`](../../src/services/rendering/Inno.Rendering/Device/RenderPresentationSize.cs#L19) | Creates a positive presentation extent. |
| [`bool Inno.Rendering.RenderPresentationSize.isValid`](../../src/services/rendering/Inno.Rendering/Device/RenderPresentationSize.cs#L42) | Gets whether both dimensions describe a real drawable extent; a default struct is invalid. |
| [`int Inno.Rendering.RenderPresentationSize.height`](../../src/services/rendering/Inno.Rendering/Device/RenderPresentationSize.cs#L37) | Gets the physical-pixel height. |
| [`int Inno.Rendering.RenderPresentationSize.width`](../../src/services/rendering/Inno.Rendering/Device/RenderPresentationSize.cs#L32) | Gets the physical-pixel width. |

### `Inno.Rendering.RenderPrimitiveTopology`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderPrimitiveTopology`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L355) | Selects the primitive assembly used by raster draw commands. |
| [`Inno.Rendering.RenderPrimitiveTopology.LineList`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L368) | Independent line pairs. |
| [`Inno.Rendering.RenderPrimitiveTopology.LineStrip`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L372) | Connected line strip. |
| [`Inno.Rendering.RenderPrimitiveTopology.PointList`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L376) | Independent points. |
| [`Inno.Rendering.RenderPrimitiveTopology.TriangleList`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L360) | Independent triangle triplets. |
| [`Inno.Rendering.RenderPrimitiveTopology.TriangleStrip`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L364) | Connected triangle strip. |

### `Inno.Rendering.RenderRasterState`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderBlendState Inno.Rendering.RenderRasterState.blend`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L167) | Gets independent RGB and alpha blending. |
| [`Inno.Rendering.RenderCullMode Inno.Rendering.RenderRasterState.cull`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L147) | Gets the face culling mode. |
| [`Inno.Rendering.RenderDepthCompare Inno.Rendering.RenderRasterState.depthCompare`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L157) | Gets depth comparison. |
| [`Inno.Rendering.RenderFrontFace Inno.Rendering.RenderRasterState.frontFace`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L152) | Gets the winding order interpreted as the front face. |
| [`Inno.Rendering.RenderPrimitiveTopology Inno.Rendering.RenderRasterState.topology`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L182) | Gets primitive assembly for subsequent draw commands. |
| [`Inno.Rendering.RenderRasterState`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L83) | Stores backend-neutral fixed-function raster state. |
| [`Inno.Rendering.RenderRasterState.RenderRasterState()`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L88) | Creates the default opaque raster state. |
| [`Inno.Rendering.RenderRasterState.RenderRasterState(Inno.Rendering.RenderCullMode cull, Inno.Rendering.RenderFrontFace frontFace, Inno.Rendering.RenderDepthCompare depthCompare, bool depthWrite, Inno.Rendering.RenderBlendState blend, byte colorWriteMask, bool multisampling, Inno.Rendering.RenderPrimitiveTopology topology)`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L119) | Creates a complete immutable raster configuration without relying on init-only setters. |
| [`bool Inno.Rendering.RenderRasterState.depthWrite`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L162) | Gets whether accepted fragments update depth. |
| [`bool Inno.Rendering.RenderRasterState.multisampling`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L177) | Gets whether multisample rasterization is enabled for compatible targets. |
| [`byte Inno.Rendering.RenderRasterState.colorWriteMask`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L172) | Gets the four-bit RGBA write mask. |
| [`static Inno.Rendering.RenderRasterState Inno.Rendering.RenderRasterState.opaque`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L142) | Gets the default opaque raster state. |

### `Inno.Rendering.RenderResourceId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderResourceId`](../../src/services/rendering/Inno.Rendering/Device/RenderProtocolIds.cs#L8) | Identifies a pipeline-defined semantic resource without imposing a central resource catalog. |
| [`Inno.Rendering.RenderResourceId.RenderResourceId(string value)`](../../src/services/rendering/Inno.Rendering/Device/RenderProtocolIds.cs#L16) | Creates an open semantic resource identifier. |
| [`bool Inno.Rendering.RenderResourceId.isValid`](../../src/services/rendering/Inno.Rendering/Device/RenderProtocolIds.cs#L30) | Gets whether the identifier contains a protocol value. |
| [`override string Inno.Rendering.RenderResourceId.ToString()`](../../src/services/rendering/Inno.Rendering/Device/RenderProtocolIds.cs#L38) | Formats this value as a human-readable representation. |
| [`string Inno.Rendering.RenderResourceId.value`](../../src/services/rendering/Inno.Rendering/Device/RenderProtocolIds.cs#L25) | Gets the globally stable protocol value. |

### `Inno.Rendering.RenderSamplerAddressMode`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderSamplerAddressMode`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L29) | Selects texture addressing independently for each coordinate axis. |
| [`Inno.Rendering.RenderSamplerAddressMode.Border`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L46) | Samples the backend border color outside the texture. |
| [`Inno.Rendering.RenderSamplerAddressMode.Clamp`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L42) | Clamps coordinates to the texture edge. |
| [`Inno.Rendering.RenderSamplerAddressMode.Mirror`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L38) | Mirrors repeated texture coordinates. |
| [`Inno.Rendering.RenderSamplerAddressMode.Repeat`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L34) | Repeats texture coordinates. |

### `Inno.Rendering.RenderSamplerFilter`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderSamplerFilter`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L10) | Selects texture filtering independently from a graphics backend. |
| [`Inno.Rendering.RenderSamplerFilter.Anisotropic`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L23) | Uses anisotropic filtering when supported. |
| [`Inno.Rendering.RenderSamplerFilter.Linear`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L19) | Uses linear filtering. |
| [`Inno.Rendering.RenderSamplerFilter.Point`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L15) | Uses nearest-neighbor filtering. |

### `Inno.Rendering.RenderSamplerState`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderSamplerAddressMode Inno.Rendering.RenderSamplerState.addressU`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L98) | Gets horizontal addressing. |
| [`Inno.Rendering.RenderSamplerAddressMode Inno.Rendering.RenderSamplerState.addressV`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L103) | Gets vertical addressing. |
| [`Inno.Rendering.RenderSamplerAddressMode Inno.Rendering.RenderSamplerState.addressW`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L108) | Gets depth or cube addressing. |
| [`Inno.Rendering.RenderSamplerFilter Inno.Rendering.RenderSamplerState.filter`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L93) | Gets the filter contract. |
| [`Inno.Rendering.RenderSamplerState`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L52) | Describes one native-serializable backend-neutral sampler binding. |
| [`Inno.Rendering.RenderSamplerState.RenderSamplerState(Inno.Rendering.RenderSamplerFilter filter, Inno.Rendering.RenderSamplerAddressMode addressU, Inno.Rendering.RenderSamplerAddressMode addressV, Inno.Rendering.RenderSamplerAddressMode addressW)`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L78) | Creates a sampler state. |
| [`bool Inno.Rendering.RenderSamplerState.Equals(Inno.Rendering.RenderSamplerState other)`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L119) | Determines whether this instance and the supplied value represent the same logical state. |
| [`override bool Inno.Rendering.RenderSamplerState.Equals(object? obj)`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L134) | Determines whether this instance and the supplied value represent the same logical state. |
| [`override int Inno.Rendering.RenderSamplerState.GetHashCode()`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L142) | Computes a hash code from the fields that participate in logical equality. |
| [`static Inno.Rendering.RenderSamplerState Inno.Rendering.RenderSamplerState.linearClamp`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L57) | Gets linear filtering with clamped addressing. |
| [`static bool Inno.Rendering.RenderSamplerState.operator !=(Inno.Rendering.RenderSamplerState left, Inno.Rendering.RenderSamplerState right)`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L173) | Determines whether two sampler descriptions differ. |
| [`static bool Inno.Rendering.RenderSamplerState.operator ==(Inno.Rendering.RenderSamplerState left, Inno.Rendering.RenderSamplerState right)`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L156) | Determines whether two sampler descriptions are equal. |

### `Inno.Rendering.RenderShaderBindingDescriptor`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderBindingId Inno.Rendering.RenderShaderBindingDescriptor.id`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L132) | Gets the stable manifest binding name. |
| [`Inno.Rendering.RenderShaderBindingDescriptor`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L71) | Declares one manifest-derived shader binding used for reflection validation. |
| [`Inno.Rendering.RenderShaderBindingDescriptor.RenderShaderBindingDescriptor(Inno.Rendering.RenderBindingId id, Inno.Rendering.RenderShaderBindingKind kind, int slot = 0, Inno.Rendering.RenderUniformType uniformType = Inno.Rendering.RenderUniformType.Vector4, int count = 1, Inno.Rendering.RenderStorageAccess storageAccess = Inno.Rendering.RenderStorageAccess.Read, string? nativeName = null)`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L97) | Creates a shader binding descriptor. |
| [`Inno.Rendering.RenderShaderBindingKind Inno.Rendering.RenderShaderBindingDescriptor.kind`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L137) | Gets the binding domain. |
| [`Inno.Rendering.RenderStorageAccess Inno.Rendering.RenderShaderBindingDescriptor.storageAccess`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L157) | Gets storage texture or buffer access. |
| [`Inno.Rendering.RenderUniformType Inno.Rendering.RenderShaderBindingDescriptor.uniformType`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L147) | Gets the uniform shape. |
| [`int Inno.Rendering.RenderShaderBindingDescriptor.count`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L152) | Gets uniform array element count. |
| [`int Inno.Rendering.RenderShaderBindingDescriptor.slot`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L142) | Gets the backend-neutral texture or storage slot. |
| [`string Inno.Rendering.RenderShaderBindingDescriptor.nativeName`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L162) | Gets the exact adapter-generated symbol used only for reflection and native resource creation. |

### `Inno.Rendering.RenderShaderBindingKind`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderShaderBindingKind`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L10) | Identifies a shader interface binding domain. |
| [`Inno.Rendering.RenderShaderBindingKind.StorageBuffer`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L27) | Compute-readable or writable buffer. |
| [`Inno.Rendering.RenderShaderBindingKind.StorageTexture`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L23) | Shader-readable or writable storage texture. |
| [`Inno.Rendering.RenderShaderBindingKind.Texture`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L19) | Sampled texture and sampler state. |
| [`Inno.Rendering.RenderShaderBindingKind.Uniform`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L15) | Vector or matrix uniform data. |

### `Inno.Rendering.RenderStencilCompare`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderStencilCompare`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L10) | Selects the comparison applied to stencil reference and stored values. |
| [`Inno.Rendering.RenderStencilCompare.Always`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L43) | Always passes. |
| [`Inno.Rendering.RenderStencilCompare.Equal`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L23) | Passes when values are equal. |
| [`Inno.Rendering.RenderStencilCompare.Greater`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L31) | Passes when reference is greater. |
| [`Inno.Rendering.RenderStencilCompare.GreaterEqual`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L39) | Passes when reference is greater or equal. |
| [`Inno.Rendering.RenderStencilCompare.Less`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L19) | Passes when reference is smaller. |
| [`Inno.Rendering.RenderStencilCompare.LessEqual`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L27) | Passes when reference is smaller or equal. |
| [`Inno.Rendering.RenderStencilCompare.Never`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L15) | Never passes. |
| [`Inno.Rendering.RenderStencilCompare.NotEqual`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L35) | Passes when values differ. |

### `Inno.Rendering.RenderStencilFaceState`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderStencilCompare Inno.Rendering.RenderStencilFaceState.compare`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L120) | Gets stencil comparison. |
| [`Inno.Rendering.RenderStencilFaceState`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L88) | Describes stencil behavior for one triangle face orientation. |
| [`Inno.Rendering.RenderStencilFaceState.RenderStencilFaceState(Inno.Rendering.RenderStencilCompare compare, Inno.Rendering.RenderStencilOperation fail, Inno.Rendering.RenderStencilOperation depthFail, Inno.Rendering.RenderStencilOperation pass)`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L105) | Creates one face stencil state. |
| [`Inno.Rendering.RenderStencilOperation Inno.Rendering.RenderStencilFaceState.depthFail`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L130) | Gets the depth-failure operation. |
| [`Inno.Rendering.RenderStencilOperation Inno.Rendering.RenderStencilFaceState.fail`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L125) | Gets the stencil-failure operation. |
| [`Inno.Rendering.RenderStencilOperation Inno.Rendering.RenderStencilFaceState.pass`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L135) | Gets the complete-pass operation. |

### `Inno.Rendering.RenderStencilOperation`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderStencilOperation`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L49) | Selects the update applied to a stencil value. |
| [`Inno.Rendering.RenderStencilOperation.DecrementClamp`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L74) | Decrements and clamps the stored value. |
| [`Inno.Rendering.RenderStencilOperation.DecrementWrap`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L78) | Decrements and wraps the stored value. |
| [`Inno.Rendering.RenderStencilOperation.IncrementClamp`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L66) | Increments and clamps the stored value. |
| [`Inno.Rendering.RenderStencilOperation.IncrementWrap`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L70) | Increments and wraps the stored value. |
| [`Inno.Rendering.RenderStencilOperation.Invert`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L82) | Bitwise-inverts the stored value. |
| [`Inno.Rendering.RenderStencilOperation.Keep`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L54) | Keeps the stored value. |
| [`Inno.Rendering.RenderStencilOperation.Replace`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L62) | Replaces the stored value with the reference. |
| [`Inno.Rendering.RenderStencilOperation.Zero`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L58) | Clears the stored value to zero. |

### `Inno.Rendering.RenderStencilState`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderStencilFaceState Inno.Rendering.RenderStencilState.back`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L224) | Gets back-face stencil behavior. |
| [`Inno.Rendering.RenderStencilFaceState Inno.Rendering.RenderStencilState.front`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L215) | Gets front-face stencil behavior. |
| [`Inno.Rendering.RenderStencilState`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L141) | Describes complete two-sided stencil state for one draw. |
| [`Inno.Rendering.RenderStencilState.RenderStencilState()`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L146) | Creates the default disabled stencil state. |
| [`Inno.Rendering.RenderStencilState.RenderStencilState(bool enabled, byte reference, byte readMask, byte writeMask, Inno.Rendering.RenderStencilFaceState front, Inno.Rendering.RenderStencilFaceState back)`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L171) | Creates a complete immutable stencil configuration without relying on init-only setters. |
| [`bool Inno.Rendering.RenderStencilState.enabled`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L195) | Gets whether stencil testing and updates are active. |
| [`byte Inno.Rendering.RenderStencilState.readMask`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L205) | Gets the mask applied while reading stored stencil. |
| [`byte Inno.Rendering.RenderStencilState.reference`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L200) | Gets the eight-bit stencil reference value. |
| [`byte Inno.Rendering.RenderStencilState.writeMask`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L210) | Gets the mask applied while writing stencil. |
| [`static Inno.Rendering.RenderStencilState Inno.Rendering.RenderStencilState.disabled`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L190) | Gets disabled stencil state. |

### `Inno.Rendering.RenderStorageAccess`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderStorageAccess`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L52) | Selects unordered storage-resource access for one shader binding. |
| [`Inno.Rendering.RenderStorageAccess.Read`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L57) | Shader read-only access. |
| [`Inno.Rendering.RenderStorageAccess.ReadWrite`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L65) | Shader read and write access. |
| [`Inno.Rendering.RenderStorageAccess.Write`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L61) | Shader write-only access. |

### `Inno.Rendering.RenderStoreAction`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderStoreAction`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L94) | Controls whether attachment contents remain valid after a raster pass. |
| [`Inno.Rendering.RenderStoreAction.Discard`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L103) | Allows the backend to discard rendered contents. |
| [`Inno.Rendering.RenderStoreAction.Store`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L99) | Preserves rendered contents. |

### `Inno.Rendering.RenderSurfaceHandle`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderSurfaceHandle`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L537) | Identifies a persistent presentation surface without exposing a backend framebuffer or swapchain handle. |
| [`bool Inno.Rendering.RenderSurfaceHandle.isValid`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L553) | Gets whether the handle identifies a presentation surface. |

### `Inno.Rendering.RenderTarget`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderTarget`](../../src/services/rendering/Inno.Rendering/Targets/RenderTarget.cs#L23) | Selects one render destination without exposing a swapchain or framebuffer handle. |
| [`Inno.Rendering.RenderTargetKind Inno.Rendering.RenderTarget.kind`](../../src/services/rendering/Inno.Rendering/Targets/RenderTarget.cs#L41) | Gets the target kind. |
| [`Inno.Rendering.RenderTexture? Inno.Rendering.RenderTarget.texture`](../../src/services/rendering/Inno.Rendering/Targets/RenderTarget.cs#L46) | Gets the offscreen texture, or for the backbuffer. |
| [`static Inno.Rendering.RenderTarget Inno.Rendering.RenderTarget.FromTexture(Inno.Rendering.RenderTexture texture)`](../../src/services/rendering/Inno.Rendering/Targets/RenderTarget.cs#L57) | Creates an offscreen render target. |
| [`static Inno.Rendering.RenderTarget Inno.Rendering.RenderTarget.backbuffer`](../../src/services/rendering/Inno.Rendering/Targets/RenderTarget.cs#L36) | Gets a target representing the main application backbuffer. |

### `Inno.Rendering.RenderTargetKind`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderTargetKind`](../../src/services/rendering/Inno.Rendering/Targets/RenderTarget.cs#L8) | Identifies whether a request renders to the main swapchain or an offscreen target. |
| [`Inno.Rendering.RenderTargetKind.Backbuffer`](../../src/services/rendering/Inno.Rendering/Targets/RenderTarget.cs#L13) | Main application window swapchain. |
| [`Inno.Rendering.RenderTargetKind.Texture`](../../src/services/rendering/Inno.Rendering/Targets/RenderTarget.cs#L17) | Persistent offscreen render texture. |

### `Inno.Rendering.RenderTexture`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderTexture`](../../src/services/rendering/Inno.Rendering/Targets/RenderTexture.cs#L8) | Describes a persistent offscreen target without owning a backend-native handle. |
| [`Inno.Rendering.RenderTexture.RenderTexture(string name, Inno.Rendering.RenderTextureDescriptor descriptor)`](../../src/services/rendering/Inno.Rendering/Targets/RenderTexture.cs#L21) | Creates an offscreen render target description. |
| [`Inno.Rendering.RenderTextureDescriptor Inno.Rendering.RenderTexture.descriptor`](../../src/services/rendering/Inno.Rendering/Targets/RenderTexture.cs#L39) | Gets the current texture requirements. |
| [`long Inno.Rendering.RenderTexture.contentRevision`](../../src/services/rendering/Inno.Rendering/Targets/RenderTexture.cs#L44) | Gets a counter incremented whenever the descriptor changes. |
| [`string Inno.Rendering.RenderTexture.name`](../../src/services/rendering/Inno.Rendering/Targets/RenderTexture.cs#L34) | Gets the artist-facing and diagnostic name. |
| [`void Inno.Rendering.RenderTexture.Resize(Inno.Rendering.RenderTextureDescriptor descriptor)`](../../src/services/rendering/Inno.Rendering/Targets/RenderTexture.cs#L52) | Replaces texture requirements at the next render-frame safety point. |

### `Inno.Rendering.RenderTextureContainer`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderTextureContainer`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L8) | Identifies a portable encoded texture container accepted by a render device. |
| [`Inno.Rendering.RenderTextureContainer.Ktx`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L13) | Khronos Texture container containing validated GPU texture payloads. |

### `Inno.Rendering.RenderTextureDescriptor`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderTextureDescriptor`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L150) | Describes a render-graph texture independently from a graphics backend. |
| [`Inno.Rendering.RenderTextureDescriptor.RenderTextureDescriptor(int width, int height, Inno.Rendering.RenderTextureFormat format, Inno.Rendering.RenderTextureUsage usage, int mipCount = 1, int arrayLayers = 1, int sampleCount = 1, Inno.Rendering.RenderTextureDimension dimension = Inno.Rendering.RenderTextureDimension.Texture2D, int depth = 1)`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L182) | Creates a texture descriptor. |
| [`Inno.Rendering.RenderTextureDimension Inno.Rendering.RenderTextureDescriptor.dimension`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L278) | Gets the dimensional texture shape. |
| [`Inno.Rendering.RenderTextureFormat Inno.Rendering.RenderTextureDescriptor.format`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L253) | Gets the storage format. |
| [`Inno.Rendering.RenderTextureUsage Inno.Rendering.RenderTextureDescriptor.usage`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L258) | Gets permitted operations. |
| [`bool Inno.Rendering.RenderTextureDescriptor.Equals(Inno.Rendering.RenderTextureDescriptor? other)`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L316) | Determines whether this instance and the supplied value represent the same logical state. |
| [`int Inno.Rendering.RenderTextureDescriptor.GetSubresourceLayerCount(int mipLevel)`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L294) | Gets the number of independently addressable layers or slices at one mip level. |
| [`int Inno.Rendering.RenderTextureDescriptor.arrayLayers`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L268) | Gets the number of array layers. |
| [`int Inno.Rendering.RenderTextureDescriptor.depth`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L283) | Gets the base mip depth for a three-dimensional texture. |
| [`int Inno.Rendering.RenderTextureDescriptor.height`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L248) | Gets the texture height in pixels. |
| [`int Inno.Rendering.RenderTextureDescriptor.mipCount`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L263) | Gets the number of mip levels. |
| [`int Inno.Rendering.RenderTextureDescriptor.sampleCount`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L273) | Gets the raster sample count. |
| [`int Inno.Rendering.RenderTextureDescriptor.width`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L243) | Gets the texture width in pixels. |
| [`override bool Inno.Rendering.RenderTextureDescriptor.Equals(object? obj)`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L337) | Determines whether this instance and the supplied value represent the same logical state. |
| [`override int Inno.Rendering.RenderTextureDescriptor.GetHashCode()`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L345) | Computes a hash code from the fields that participate in logical equality. |

### `Inno.Rendering.RenderTextureDimension`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderTextureDimension`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L55) | Identifies the dimensional shape of a backend-neutral texture resource. |
| [`Inno.Rendering.RenderTextureDimension.Cube`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L68) | Cubemap texture with optional cubemap array layers. |
| [`Inno.Rendering.RenderTextureDimension.Texture2D`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L60) | Two-dimensional texture with optional array layers. |
| [`Inno.Rendering.RenderTextureDimension.Texture3D`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L64) | Three-dimensional volume texture. |

### `Inno.Rendering.RenderTextureFormat`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderTextureFormat`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L8) | Declares backend-neutral texture storage formats. |
| [`Inno.Rendering.RenderTextureFormat.Depth24Stencil8`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L45) | Twenty-four-bit depth and eight-bit stencil format. |
| [`Inno.Rendering.RenderTextureFormat.Depth32Float`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L49) | Thirty-two-bit floating-point depth format. |
| [`Inno.Rendering.RenderTextureFormat.R32Float`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L41) | Single-channel 32-bit floating-point format. |
| [`Inno.Rendering.RenderTextureFormat.R8`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L13) | Eight-bit single-channel normalized format. |
| [`Inno.Rendering.RenderTextureFormat.RG11B10Float`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L33) | Eleven-bit RGB floating-point format. |
| [`Inno.Rendering.RenderTextureFormat.RG8`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L17) | Eight-bit two-channel normalized format. |
| [`Inno.Rendering.RenderTextureFormat.RGB10A2`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L29) | Ten-bit RGB and two-bit alpha normalized format. |
| [`Inno.Rendering.RenderTextureFormat.RGBA16Float`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L37) | Half-precision four-channel floating-point format. |
| [`Inno.Rendering.RenderTextureFormat.RGBA8`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L21) | Eight-bit four-channel linear normalized format. |
| [`Inno.Rendering.RenderTextureFormat.RGBA8Srgb`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L25) | Eight-bit four-channel sRGB format. |

### `Inno.Rendering.RenderTextureHandle`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderTextureHandle`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L443) | Identifies a frame-scoped render-graph texture. |
| [`bool Inno.Rendering.RenderTextureHandle.isValid`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L462) | Gets whether the handle was created by a render graph. |

### `Inno.Rendering.RenderTextureReadbackHandle`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderTextureReadbackHandle`](../../src/services/rendering/Inno.Rendering/Device/RenderTextureReadback.cs#L8) | Identifies one backend-neutral asynchronous texture readback operation. |
| [`bool Inno.Rendering.RenderTextureReadbackHandle.isValid`](../../src/services/rendering/Inno.Rendering/Device/RenderTextureReadback.cs#L24) | Gets whether the handle identifies a device readback operation. |

### `Inno.Rendering.RenderTextureReadbackResult`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderTextureDescriptor Inno.Rendering.RenderTextureReadbackResult.descriptor`](../../src/services/rendering/Inno.Rendering/Device/RenderTextureReadback.cs#L71) | Gets the source texture descriptor. |
| [`Inno.Rendering.RenderTextureReadbackResult`](../../src/services/rendering/Inno.Rendering/Device/RenderTextureReadback.cs#L30) | Contains immutable bytes copied from one complete texture mip. |
| [`Inno.Rendering.RenderTextureReadbackResult.RenderTextureReadbackResult(Inno.Rendering.RenderTextureDescriptor descriptor, int mipLevel, int rowPitch, System.ReadOnlySpan<byte> data)`](../../src/services/rendering/Inno.Rendering/Device/RenderTextureReadback.cs#L49) | Creates one immutable texture readback result. |
| [`System.ReadOnlyMemory<byte> Inno.Rendering.RenderTextureReadbackResult.data`](../../src/services/rendering/Inno.Rendering/Device/RenderTextureReadback.cs#L86) | Gets complete tightly packed mip bytes for every addressable layer. |
| [`int Inno.Rendering.RenderTextureReadbackResult.mipLevel`](../../src/services/rendering/Inno.Rendering/Device/RenderTextureReadback.cs#L76) | Gets the source mip level. |
| [`int Inno.Rendering.RenderTextureReadbackResult.rowPitch`](../../src/services/rendering/Inno.Rendering/Device/RenderTextureReadback.cs#L81) | Gets the byte distance between adjacent rows. |

### `Inno.Rendering.RenderTextureRegion`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderTextureRegion`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L80) | Describes one texture subresource box for copy and blit commands. |
| [`Inno.Rendering.RenderTextureRegion.RenderTextureRegion(int mip, int x, int y, int layer, int width, int height, int depth = 1)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L106) | Creates a texture subresource box. |
| [`int Inno.Rendering.RenderTextureRegion.depth`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L164) | Gets the layer or depth-slice count. |
| [`int Inno.Rendering.RenderTextureRegion.height`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L159) | Gets the texel height. |
| [`int Inno.Rendering.RenderTextureRegion.layer`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L149) | Gets the array layer or depth-slice origin. |
| [`int Inno.Rendering.RenderTextureRegion.mip`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L134) | Gets the zero-based mip level. |
| [`int Inno.Rendering.RenderTextureRegion.width`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L154) | Gets the texel width. |
| [`int Inno.Rendering.RenderTextureRegion.x`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L139) | Gets the horizontal texel origin. |
| [`int Inno.Rendering.RenderTextureRegion.y`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L144) | Gets the vertical texel origin. |

### `Inno.Rendering.RenderTextureUsage`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderTextureUsage`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L74) | Declares intended texture operations for capability and hazard validation. |
| [`Inno.Rendering.RenderTextureUsage.ColorAttachment`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L84) | Raster color attachment. |
| [`Inno.Rendering.RenderTextureUsage.CopyDestination`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L100) | Copy operation destination. |
| [`Inno.Rendering.RenderTextureUsage.CopySource`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L96) | Copy operation source. |
| [`Inno.Rendering.RenderTextureUsage.DepthStencilAttachment`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L88) | Raster depth or stencil attachment. |
| [`Inno.Rendering.RenderTextureUsage.Readback`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L104) | Asynchronous transfer from GPU texture memory to CPU-visible bytes. |
| [`Inno.Rendering.RenderTextureUsage.Sampled`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L80) | Shader-readable texture. |
| [`Inno.Rendering.RenderTextureUsage.Storage`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L92) | Shader-readable and writable unordered texture. |

### `Inno.Rendering.RenderUniformType`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderUniformType`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L33) | Identifies the native-independent shape of one uniform binding. |
| [`Inno.Rendering.RenderUniformType.Matrix3x3`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L42) | Three-by-three 32-bit floating-point matrix. |
| [`Inno.Rendering.RenderUniformType.Matrix4x4`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L46) | Four-by-four 32-bit floating-point matrix. |
| [`Inno.Rendering.RenderUniformType.Vector4`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L38) | Four-component 32-bit floating-point vector. |

### `Inno.Rendering.RenderVertexAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderVertexAttribute`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L156) | Describes one vertex attribute in stream order. |
| [`Inno.Rendering.RenderVertexAttribute.RenderVertexAttribute(Inno.Rendering.RenderVertexSemantic semantic, Inno.Rendering.RenderVertexFormat format, int byteOffset = -1)`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L170) | Creates a vertex attribute declaration. |
| [`Inno.Rendering.RenderVertexFormat Inno.Rendering.RenderVertexAttribute.format`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L190) | Gets the packed component representation. |
| [`Inno.Rendering.RenderVertexSemantic Inno.Rendering.RenderVertexAttribute.semantic`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L185) | Gets the shader input semantic. |
| [`int Inno.Rendering.RenderVertexAttribute.byteOffset`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L195) | Gets the explicit byte offset in the resolved stream layout, or -1 before a layout resolves automatic placement. |
| [`int Inno.Rendering.RenderVertexAttribute.byteSize`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L200) | Gets the packed byte size. |

### `Inno.Rendering.RenderVertexFormat`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderVertexFormat`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L89) | Identifies one packed vertex attribute representation. |
| [`Inno.Rendering.RenderVertexFormat.Float1`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L94) | One 32-bit floating-point component. |
| [`Inno.Rendering.RenderVertexFormat.Float2`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L98) | Two 32-bit floating-point components. |
| [`Inno.Rendering.RenderVertexFormat.Float3`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L102) | Three 32-bit floating-point components. |
| [`Inno.Rendering.RenderVertexFormat.Float4`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L106) | Four 32-bit floating-point components. |
| [`Inno.Rendering.RenderVertexFormat.Half2`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L110) | Two 16-bit floating-point components. |
| [`Inno.Rendering.RenderVertexFormat.Half4`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L114) | Four 16-bit floating-point components. |
| [`Inno.Rendering.RenderVertexFormat.Int16Integer2`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L146) | Two signed 16-bit components interpreted as integers. |
| [`Inno.Rendering.RenderVertexFormat.Int16Integer4`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L150) | Four signed 16-bit components interpreted as integers. |
| [`Inno.Rendering.RenderVertexFormat.Int16Normalized2`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L138) | Two normalized signed 16-bit components. |
| [`Inno.Rendering.RenderVertexFormat.Int16Normalized4`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L142) | Four normalized signed 16-bit components. |
| [`Inno.Rendering.RenderVertexFormat.UInt10Normalized4`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L134) | Four normalized unsigned components packed into 10:10:10:2 bits. |
| [`Inno.Rendering.RenderVertexFormat.UInt8Integer2`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L130) | Two unsigned bytes interpreted as integers. |
| [`Inno.Rendering.RenderVertexFormat.UInt8Integer4`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L126) | Four unsigned bytes interpreted as integers. |
| [`Inno.Rendering.RenderVertexFormat.UInt8Normalized2`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L122) | Two normalized unsigned bytes. |
| [`Inno.Rendering.RenderVertexFormat.UInt8Normalized4`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L118) | Four normalized unsigned bytes. |

### `Inno.Rendering.RenderVertexLayout`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderVertexLayout`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L224) | Defines one interleaved vertex stream independently from a graphics backend. |
| [`Inno.Rendering.RenderVertexLayout.RenderVertexLayout(System.Collections.Generic.IReadOnlyList<Inno.Rendering.RenderVertexAttribute> attributes, int stride = 0)`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L238) | Creates an interleaved vertex layout. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.RenderVertexAttribute> Inno.Rendering.RenderVertexLayout.attributes`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L285) | Gets attributes in byte-stream order. |
| [`bool Inno.Rendering.RenderVertexLayout.Equals(Inno.Rendering.RenderVertexLayout? other)`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L301) | Determines whether this instance and the supplied value represent the same logical state. |
| [`int Inno.Rendering.RenderVertexLayout.stride`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L290) | Gets the interleaved vertex stride in bytes. |
| [`override bool Inno.Rendering.RenderVertexLayout.Equals(object? obj)`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L315) | Determines whether this instance and the supplied value represent the same logical state. |
| [`override int Inno.Rendering.RenderVertexLayout.GetHashCode()`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L323) | Computes a hash code from the fields that participate in logical equality. |

### `Inno.Rendering.RenderVertexSemantic`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderVertexSemantic`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L10) | Identifies one backend-neutral vertex attribute semantic. |
| [`Inno.Rendering.RenderVertexSemantic.Bitangent`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L27) | Object-space bitangent. |
| [`Inno.Rendering.RenderVertexSemantic.BlendIndices`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L79) | Skinning indices. |
| [`Inno.Rendering.RenderVertexSemantic.BlendWeights`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L83) | Skinning weights. |
| [`Inno.Rendering.RenderVertexSemantic.Color0`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L31) | Primary vertex color. |
| [`Inno.Rendering.RenderVertexSemantic.Color1`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L35) | Secondary vertex color. |
| [`Inno.Rendering.RenderVertexSemantic.Color2`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L39) | Third vertex color channel. |
| [`Inno.Rendering.RenderVertexSemantic.Color3`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L43) | Fourth vertex color channel. |
| [`Inno.Rendering.RenderVertexSemantic.Normal`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L19) | Object-space normal. |
| [`Inno.Rendering.RenderVertexSemantic.Position`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L15) | Object-space position. |
| [`Inno.Rendering.RenderVertexSemantic.Tangent`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L23) | Object-space tangent and handedness. |
| [`Inno.Rendering.RenderVertexSemantic.TextureCoordinate0`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L47) | Primary texture coordinate. |
| [`Inno.Rendering.RenderVertexSemantic.TextureCoordinate1`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L51) | Secondary texture coordinate. |
| [`Inno.Rendering.RenderVertexSemantic.TextureCoordinate2`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L55) | Third texture coordinate. |
| [`Inno.Rendering.RenderVertexSemantic.TextureCoordinate3`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L59) | Fourth texture coordinate. |
| [`Inno.Rendering.RenderVertexSemantic.TextureCoordinate4`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L63) | Fifth texture coordinate. |
| [`Inno.Rendering.RenderVertexSemantic.TextureCoordinate5`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L67) | Sixth texture coordinate. |
| [`Inno.Rendering.RenderVertexSemantic.TextureCoordinate6`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L71) | Seventh texture coordinate. |
| [`Inno.Rendering.RenderVertexSemantic.TextureCoordinate7`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L75) | Eighth texture coordinate. |

### `Inno.Rendering.RenderViewTransform`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderViewTransform`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L12) | Stores backend-ready column-major transform matrices for one programmable view. |
| [`Inno.Rendering.RenderViewTransform.RenderViewTransform(System.ReadOnlySpan<float> viewMatrix, System.ReadOnlySpan<float> projectionMatrix)`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L26) | Creates a raster view transform. |
| [`System.ReadOnlyMemory<float> Inno.Rendering.RenderViewTransform.projectionMatrix`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L54) | Gets the immutable column-major projection matrix. |
| [`System.ReadOnlyMemory<float> Inno.Rendering.RenderViewTransform.viewMatrix`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L49) | Gets the immutable column-major view matrix. |

### `Inno.Rendering.RenderViewport`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderViewport`](../../src/services/rendering/Inno.Rendering/Views/RenderViewport.cs#L9) | Defines a destination pixel rectangle without assuming producer or rendering semantics. |
| [`Inno.Rendering.RenderViewport.RenderViewport(int x, int y, int width, int height)`](../../src/services/rendering/Inno.Rendering/Views/RenderViewport.cs#L26) | Creates a render viewport. |
| [`int Inno.Rendering.RenderViewport.height`](../../src/services/rendering/Inno.Rendering/Views/RenderViewport.cs#L60) | Gets the viewport height. |
| [`int Inno.Rendering.RenderViewport.width`](../../src/services/rendering/Inno.Rendering/Views/RenderViewport.cs#L55) | Gets the viewport width. |
| [`int Inno.Rendering.RenderViewport.x`](../../src/services/rendering/Inno.Rendering/Views/RenderViewport.cs#L45) | Gets the left pixel offset. |
| [`int Inno.Rendering.RenderViewport.y`](../../src/services/rendering/Inno.Rendering/Views/RenderViewport.cs#L50) | Gets the top pixel offset. |

### `Inno.Rendering.ShaderInterface`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.ShaderInterface`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderInterface.cs#L10) | Stores the manifest-derived binding contract verified after backend program creation. |
| [`Inno.Rendering.ShaderInterface.ShaderInterface(System.Collections.Generic.IReadOnlyList<Inno.Rendering.ShaderInterfaceBinding> bindings)`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderInterface.cs#L18) | Creates a shader interface contract. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.ShaderInterfaceBinding> Inno.Rendering.ShaderInterface.bindings`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderInterface.cs#L27) | Gets stable expected bindings. |

### `Inno.Rendering.ShaderInterfaceBinding`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.RenderStorageAccess Inno.Rendering.ShaderInterfaceBinding.storageAccess`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderInterfaceBinding.cs#L95) | Gets required access for storage resources. |
| [`Inno.Rendering.ShaderInterfaceBinding`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderInterfaceBinding.cs#L10) | Describes one reflected material binding expected by compiled programs. |
| [`Inno.Rendering.ShaderInterfaceBinding.ShaderInterfaceBinding(Inno.Rendering.ShaderPropertyId id, Inno.Rendering.ShaderPropertyType type, Inno.Rendering.ShaderStage stages, int arrayCount = 1, Inno.Rendering.ShaderPropertyBindingKind bindingKind = Inno.Rendering.ShaderPropertyBindingKind.Uniform, Inno.Rendering.RenderStorageAccess storageAccess = Inno.Rendering.RenderStorageAccess.Read, string? nativeName = null, int? location = null)`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderInterfaceBinding.cs#L39) | Creates a reflected interface binding. |
| [`Inno.Rendering.ShaderPropertyBindingKind Inno.Rendering.ShaderInterfaceBinding.bindingKind`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderInterfaceBinding.cs#L90) | Gets the backend-neutral interface binding domain. |
| [`Inno.Rendering.ShaderPropertyId Inno.Rendering.ShaderInterfaceBinding.id`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderInterfaceBinding.cs#L70) | Gets the stable property ID. |
| [`Inno.Rendering.ShaderPropertyType Inno.Rendering.ShaderInterfaceBinding.type`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderInterfaceBinding.cs#L75) | Gets the expected value or resource type. |
| [`Inno.Rendering.ShaderStage Inno.Rendering.ShaderInterfaceBinding.stages`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderInterfaceBinding.cs#L80) | Gets stages that consume the binding. |
| [`int Inno.Rendering.ShaderInterfaceBinding.arrayCount`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderInterfaceBinding.cs#L85) | Gets the required array element count. |
| [`int? Inno.Rendering.ShaderInterfaceBinding.location`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderInterfaceBinding.cs#L104) | Gets an explicitly compiled resource slot, or null for a caller-assigned layout. |
| [`string Inno.Rendering.ShaderInterfaceBinding.nativeName`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderInterfaceBinding.cs#L100) | Gets the exact reflected symbol emitted by the adapter, distinct from the logical property ID. |

### `Inno.Rendering.ShaderProgramKind`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.ShaderProgramKind`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderProgramKind.cs#L14) | Selects the programmable stage combination of a pass. |
| [`Inno.Rendering.ShaderProgramKind.Compute`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderProgramKind.cs#L23) | A compute stage used by a compute pass. |
| [`Inno.Rendering.ShaderProgramKind.Raster`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderProgramKind.cs#L19) | Vertex and fragment stages used by a raster pass. |

### `Inno.Rendering.ShaderPropertyBindingKind`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.ShaderPropertyBindingKind`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyBindingKind.cs#L14) | Defines how one shader property enters the backend-neutral resource interface. |
| [`Inno.Rendering.ShaderPropertyBindingKind.SampledTexture`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyBindingKind.cs#L23) | Texture sampled through an explicit material sampler. |
| [`Inno.Rendering.ShaderPropertyBindingKind.StorageBuffer`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyBindingKind.cs#L31) | Buffer bound for unordered shader access by a Pipeline. |
| [`Inno.Rendering.ShaderPropertyBindingKind.StorageTexture`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyBindingKind.cs#L27) | Texture bound for unordered shader access by a Pipeline. |
| [`Inno.Rendering.ShaderPropertyBindingKind.Uniform`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyBindingKind.cs#L19) | Vector or matrix uniform data. |

### `Inno.Rendering.ShaderPropertyId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.ShaderPropertyId`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyId.cs#L14) | Identifies a material property using a stable serialized string. |
| [`Inno.Rendering.ShaderPropertyId.ShaderPropertyId(string value)`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyId.cs#L22) | Creates a stable shader property identifier. |
| [`bool Inno.Rendering.ShaderPropertyId.isValid`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyId.cs#L36) | Gets whether this identifier has a usable value. |
| [`override string Inno.Rendering.ShaderPropertyId.ToString()`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyId.cs#L44) | Formats this value as a human-readable representation. |
| [`string Inno.Rendering.ShaderPropertyId.value`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyId.cs#L31) | Gets or sets the stable manifest property identifier. |

### `Inno.Rendering.ShaderPropertyType`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.ShaderPropertyType`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyType.cs#L14) | Identifies an artist-facing shader property type. |
| [`Inno.Rendering.ShaderPropertyType.Buffer`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyType.cs#L59) | Read-only or read-write buffer. |
| [`Inno.Rendering.ShaderPropertyType.Color`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyType.cs#L35) | Linear RGBA color. |
| [`Inno.Rendering.ShaderPropertyType.Float`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyType.cs#L19) | Scalar floating-point value. |
| [`Inno.Rendering.ShaderPropertyType.Matrix4x4`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyType.cs#L39) | Four-by-four matrix. |
| [`Inno.Rendering.ShaderPropertyType.Texture2D`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyType.cs#L43) | Two-dimensional texture. |
| [`Inno.Rendering.ShaderPropertyType.Texture2DArray`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyType.cs#L47) | Layered two-dimensional texture. |
| [`Inno.Rendering.ShaderPropertyType.Texture3D`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyType.cs#L51) | Three-dimensional volume texture. |
| [`Inno.Rendering.ShaderPropertyType.TextureCube`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyType.cs#L55) | Cube texture. |
| [`Inno.Rendering.ShaderPropertyType.Vector2`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyType.cs#L23) | Two-component floating-point vector. |
| [`Inno.Rendering.ShaderPropertyType.Vector3`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyType.cs#L27) | Three-component floating-point vector. |
| [`Inno.Rendering.ShaderPropertyType.Vector4`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyType.cs#L31) | Four-component floating-point vector. |

### `Inno.Rendering.ShaderStage`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.ShaderStage`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderStage.cs#L14) | Identifies a programmable shader stage. |
| [`Inno.Rendering.ShaderStage.Compute`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderStage.cs#L32) | Compute shader stage. |
| [`Inno.Rendering.ShaderStage.Fragment`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderStage.cs#L28) | Fragment shader stage. |
| [`Inno.Rendering.ShaderStage.None`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderStage.cs#L20) | No shader stage. |
| [`Inno.Rendering.ShaderStage.Vertex`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderStage.cs#L24) | Vertex shader stage. |

## 项目依赖

- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Identity](../core/Inno.Core.Identity.md)：公开引用边界由实际签名核对。
- [Inno.Core.Input](../core/Inno.Core.Input.md)：公开引用边界由实际签名核对。
- [Inno.Core.Mathematics](../core/Inno.Core.Mathematics.md)：公开引用边界由实际签名核对。
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
