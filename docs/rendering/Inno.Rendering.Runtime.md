# Inno.Rendering.Runtime

[分类索引](README.md) · [Wiki 首页](../README.md)

## 职责与边界

本层把输出请求、运行资产和图形设备组合成可执行的帧。它拥有模型、Pipeline/Feature generation、资产到 GPU 的资源缓存及退休队列。

直接依赖运行资产和中立契约，不引用 Authoring、Assets Pipeline 或 Shader 编译工具。`ContentRenderTargetArtifactProvider` 通过逻辑内容定位读取已编译产物，运行时不编译创作源码。

## 初始化、扩展与资源退休

宿主提供 TypeCatalog、IRenderDevice、诊断和所需的 artifact provider。设备由宿主持有；Runtime 管理自己创建的模型、资源服务、输出 target 和 generation。构造函数只完成组合，帧驱动由其所属 Session 的 Subsystem 生命周期执行。

| 扩展需求 | 入口与责任 |
| --- | --- |
| 解释一种内容并生成输出 | 实现 `IRenderModel`，使用稳定模型扩展身份进入注册 |
| 定义绘制流程和可选功能 | 实现 Pipeline/Feature 扩展，声明所需设备能力 |
| 为最终帧追加中立图操作 | 注册 `IRenderFrameGraphContributor`，退出前解除注册 |
| 提供部署中的 Shader/Texture | 注入 `IRenderTargetArtifactProvider`，保持内容 owner 存活 |

Contributor 集合变化时才创建不可变 snapshot。帧开始固定一个 snapshot；帧内注册或注销影响下一帧，调用者不能在当前帧仍可能使用实例时立即释放它。注册不转移 Contributor 的生命周期所有权。

每个 Contributor 在自己的 graph mutation scope 中构建。普通扩展失败会报告诊断并回滚该 scope，已经接受的图仍保留；退休 Pending 会继续向 owner 传播。Validate 只做分析，最终 Compile 构造完整图；`RenderFrameStatistics.graphCompileCount` 用于核查实际次数。

下面只展示宿主创建 Runtime 的最小组合，不能单独替代 Session 启动和关闭流程：
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

主输出不可用时跳过其建图与输入换算，独立离屏输出继续运行。缺失产物、能力不足和内容损坏分别报告，候选失败保留 last-good。

退出先撤销候选并退休 generation，再清空请求和 Contributor 引用，释放输出 target、合成器、上传与资源服务，最后关闭设备帧。设备最终销毁仍属于宿主；退休 Pending 或失败不能当作正常完成。私有 scratch 保留容量，但在帧结束、异常和退出时清空 extension 引用，不是对外可持有的快照。

资源描述与 GPU 图机制见 [Core](Inno.Rendering.md)，持久资产见 [Assets](Inno.Rendering.Assets.md)，创作编译见 [Authoring](Inno.Rendering.Assets.Authoring.md)。该程序集不引用创作或编译工具，不内建 2D/3D 世界模型。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Rendering.Runtime.ContentRenderTargetArtifactProvider`

<!-- api: T:Inno.Rendering.Runtime.ContentRenderTargetArtifactProvider b28935a2457721ed8704a24f130f2f667f489a3490497cef5b52ede58e2551a6 -->
<!-- api: M:Inno.Rendering.Runtime.ContentRenderTargetArtifactProvider.#ctor(Inno.Content.IRuntimeContentStore,Inno.Core.Serialization.SerializationRegistry,Inno.Core.Serialization.SerializationContext) e479ec35bedc1c98a5b5405b9b34554135043d3f3f4e68fc7bf58f4bdce92553 -->
<!-- api: M:Inno.Rendering.Runtime.ContentRenderTargetArtifactProvider.ReadShaderDefinition(Inno.Rendering.Assets.RenderShaderArtifact) 31bf8bd7b7e84252746f0dcb495f7b4dc43a74ace60cb5727f33deddba10ff0d -->
<!-- api: M:Inno.Rendering.Runtime.ContentRenderTargetArtifactProvider.GetShaderArtifact(Inno.Rendering.Assets.ShaderAsset,Inno.Rendering.Assets.RenderShaderVariant,Inno.Rendering.GraphicsCapabilities,Inno.Rendering.Assets.RenderShaderArtifact@) aba6c02c6a313ad3fd6e5e78328cfab02151ceb8613fda15fbc0bfac43f1ce3b -->
<!-- api: M:Inno.Rendering.Runtime.ContentRenderTargetArtifactProvider.GetTextureArtifact(Inno.Rendering.Assets.RenderTextureArtifactReference,System.ReadOnlyMemory{System.Byte}@) 6a144a3c2cca6eedf2012b305de8e6674956bfd87b8edf419653e62d80ead606 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-bde5d8add0b04ba5"></a>[`public Inno.Rendering.Runtime.ContentRenderTargetArtifactProvider.ContentRenderTargetArtifactProvider(Inno.Content.IRuntimeContentStore content, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Runtime/Deployment/ContentRenderTargetArtifactProvider.cs#L37) | Creates a provider borrowing one source-free content store.<br>`content`：The verified immutable content store, borrowed for this provider's lifetime.<br>`serialization`：The runtime owner serialization registry.<br>`context`：Complete runtime asset/reference context used to resolve captured texture defaults. | — | `System.ArgumentNullException`：A required content or serialization service is null. |
| <a id="api-a5e6f5bfe491df90"></a>[`public Inno.Rendering.Runtime.RenderTargetArtifactStatus Inno.Rendering.Runtime.ContentRenderTargetArtifactProvider.GetShaderArtifact(Inno.Rendering.Assets.ShaderAsset shader, Inno.Rendering.Assets.RenderShaderVariant variant, Inno.Rendering.GraphicsCapabilities capabilities, out Inno.Rendering.Assets.RenderShaderArtifact? artifact)`](../../src/services/rendering/Inno.Rendering.Runtime/Deployment/ContentRenderTargetArtifactProvider.cs#L87) | Loads and validates one packaged shader target artifact when it exists.<br>返回：Inno.Rendering.Runtime.RenderTargetArtifactStatus.Ready when the deployed artifact exists; otherwise, Inno.Rendering.Runtime.RenderTargetArtifactStatus.Unavailable.<br>`shader`：The imported backend-neutral shader asset.<br>`variant`：The exact material keyword selection.<br>`capabilities`：The active graphics capability snapshot.<br>`artifact`：Receives the decoded artifact when present. | — | `System.IO.InvalidDataException`：Thrown when a deployed artifact exists but is corrupt or mismatched.<br>`System.ArgumentNullException`：Thrown when shader or capabilities is null. |
| <a id="api-b97261d90a146217"></a>[`public Inno.Rendering.Runtime.RenderTargetArtifactStatus Inno.Rendering.Runtime.ContentRenderTargetArtifactProvider.GetTextureArtifact(Inno.Rendering.Assets.RenderTextureArtifactReference texture, out System.ReadOnlyMemory<byte> artifact)`](../../src/services/rendering/Inno.Rendering.Runtime/Deployment/ContentRenderTargetArtifactProvider.cs#L145) | Loads one packaged portable texture artifact when it exists.<br>返回：Inno.Rendering.Runtime.RenderTargetArtifactStatus.Ready when the deployed artifact exists; otherwise, Inno.Rendering.Runtime.RenderTargetArtifactStatus.Unavailable.<br>`texture`：The imported texture description.<br>`artifact`：Receives immutable KTX bytes when present. | — | `System.IO.InvalidDataException`：Thrown when a deployed texture artifact is empty.<br>`System.ArgumentNullException`：Thrown when texture is null. |
| <a id="api-13492b94b5aec691"></a>[`public Inno.Rendering.Assets.ShaderDefinition Inno.Rendering.Runtime.ContentRenderTargetArtifactProvider.ReadShaderDefinition(Inno.Rendering.Assets.RenderShaderArtifact artifact)`](../../src/services/rendering/Inno.Rendering.Runtime/Deployment/ContentRenderTargetArtifactProvider.cs#L56) | Reads and validates the shader definition value from its authoritative source.<br>返回：The validated shader definition that represents the completed operation.<br>`artifact`：The resolved immutable artifact payload returned to the caller. | — | — |
| <a id="api-66fac24184577469"></a>[`public sealed class Inno.Rendering.Runtime.ContentRenderTargetArtifactProvider : Inno.Rendering.Runtime.IRenderTargetArtifactProvider`](../../src/services/rendering/Inno.Rendering.Runtime/Deployment/ContentRenderTargetArtifactProvider.cs#L15) | Reads immutable render target artifacts from one verified logical content store. | — | — |

### `Inno.Rendering.Runtime.GraphicsSettings`

<!-- api: T:Inno.Rendering.Runtime.GraphicsSettings 70246586dc6d7732d0e905e2462c5b3bb61d4f0bf078e503acc8683e8e91c8c6 -->
<!-- api: P:Inno.Rendering.Runtime.GraphicsSettings.capabilities 614f819052baed016c9f4ddc2a0b904f3020762b758407cf30b03e826bbfbefc -->
<!-- api: P:Inno.Rendering.Runtime.GraphicsSettings.defaultPipeline 6b3be29e356396f786c6ec8282898e6ced24d6a38f7bcea667d4c356bc8a88e7 -->
<!-- api: P:Inno.Rendering.Runtime.GraphicsSettings.frameStatistics 0be79ed1e4fcf1e463b8e7dde1c9a14ffb62062e43c8871a6d5535072d7af4b8 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1a27f9418ed0277a"></a>[`public static Inno.Rendering.GraphicsCapabilities? Inno.Rendering.Runtime.GraphicsSettings.capabilities { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/GraphicsSettings.cs#L18) | Gets current device capabilities, or null before device initialization. | — | — |
| <a id="api-e62e2990a4b8ba13"></a>[`public static Inno.Rendering.Assets.RenderPipelineAsset? Inno.Rendering.Runtime.GraphicsSettings.defaultPipeline { get; set; }`](../../src/services/rendering/Inno.Rendering.Runtime/GraphicsSettings.cs#L23) | Gets or sets the project default pipeline used by requests without an override. | — | — |
| <a id="api-eca7e927475556e0"></a>[`public static Inno.Rendering.Runtime.RenderFrameStatistics? Inno.Rendering.Runtime.GraphicsSettings.frameStatistics { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/GraphicsSettings.cs#L32) | Gets statistics for the last completed frame, or null before the first frame. | — | — |
| <a id="api-c8d216836353b9aa"></a>[`public static class Inno.Rendering.Runtime.GraphicsSettings`](../../src/services/rendering/Inno.Rendering.Runtime/GraphicsSettings.cs#L13) | Exposes current rendering configuration and immutable device state. | — | — |

### `Inno.Rendering.Runtime.IPreparedViewDrawable`

<!-- api: T:Inno.Rendering.Runtime.IPreparedViewDrawable 8b1af62b9578b35cffde3193448313ec8b4d59b9d84252787c0541ac5ee765a4 -->
<!-- api: M:Inno.Rendering.Runtime.IPreparedViewDrawable.Encode(Inno.Rendering.RenderCommandEncoder) 86a3fd62435fdb6269ada8782a0e836a15230952369d20fdb16ad0d4817604f4 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-516fcae546c20abb"></a>[`void Inno.Rendering.Runtime.IPreparedViewDrawable.Encode(Inno.Rendering.RenderCommandEncoder commands)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L207) | Encodes commands while the owning model controls attachments and view state.<br>`commands`：Backend-neutral scene pass encoder. | — | — |
| <a id="api-d847eeed8ed6a979"></a>[`public interface Inno.Rendering.Runtime.IPreparedViewDrawable`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L199) | Encodes a resource-ready draw into the owning model's scene pass. | — | — |

### `Inno.Rendering.Runtime.IRenderModel`

<!-- api: T:Inno.Rendering.Runtime.IRenderModel b483cc64107fc42290a6f05fe75ab64c0b2ea496b6abd5baca0a94cfe9f372ae -->
<!-- api: M:Inno.Rendering.Runtime.IRenderModel.CanRender(Inno.Rendering.Runtime.RenderOutputSession) ad5b1615fa188027f7db726dd4c1654438548a060c6f01591768693a57287219 -->
<!-- api: M:Inno.Rendering.Runtime.IRenderModel.Build(Inno.Rendering.Runtime.RenderOutputSession) 22400d14e451e5136502e62ae8e354d00a8208a700b2a3e69c7437eba89f5934 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-553cd40fe5ca2885"></a>[`Inno.Rendering.Runtime.RenderModelOutput Inno.Rendering.Runtime.IRenderModel.Build(Inno.Rendering.Runtime.RenderOutputSession session)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/IRenderModel.cs#L37) | Builds one model output after acceptance.<br>返回：Prepared frame data and pipeline.<br>`session`：Host output session. | — | — |
| <a id="api-4bdfd6b7f329d58a"></a>[`bool Inno.Rendering.Runtime.IRenderModel.CanRender(Inno.Rendering.Runtime.RenderOutputSession session)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/IRenderModel.cs#L26) | Determines whether this model accepts the session's content.<br>返回：Whether the model can render the selected content.<br>`session`：Host output session. | — | — |
| <a id="api-e7c07b28da53eae0"></a>[`public interface Inno.Rendering.Runtime.IRenderModel : System.IDisposable`](../../src/services/rendering/Inno.Rendering.Runtime/Models/IRenderModel.cs#L15) | Builds view requests from host output sessions without owning a host window. | — | — |

### `Inno.Rendering.Runtime.IRenderRequestSink`

<!-- api: T:Inno.Rendering.Runtime.IRenderRequestSink 7d196fc97344e2435c89f2397e8184ddff07a1294e2b614909778897e2f1264b -->
<!-- api: M:Inno.Rendering.Runtime.IRenderRequestSink.Submit(Inno.Rendering.Runtime.RenderRequest) 5cebc318e9801300a5c8fb37f6eeb491e9e7ce636f0a2edde0fca2279f19b177 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-009eed5ffe5a4b97"></a>[`void Inno.Rendering.Runtime.IRenderRequestSink.Submit(Inno.Rendering.Runtime.RenderRequest request)`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/IRenderRequestSink.cs#L18) | Queues one immutable view request for the current or next render frame.<br>`request`：Immutable pipeline-defined request. | — | — |
| <a id="api-f1bfbd76d3921eb2"></a>[`public interface Inno.Rendering.Runtime.IRenderRequestSink`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/IRenderRequestSink.cs#L10) | Accepts rendering-model-neutral requests without exposing runtime or backend ownership. | — | — |

### `Inno.Rendering.Runtime.IRenderResourceService`

<!-- api: T:Inno.Rendering.Runtime.IRenderResourceService 8271ccc091b7b57dfd54a15c76ff6949abec9a7f0151be1ba68f0e040c31c894 -->
<!-- api: P:Inno.Rendering.Runtime.IRenderResourceService.capabilities b442df629b813f2ac3e6ea86bfdcea5f613b0228c2d2304c2349ac9b8a5f006c -->
<!-- api: M:Inno.Rendering.Runtime.IRenderResourceService.ValidateShaderArtifact(Inno.Rendering.Assets.RenderShaderArtifact) 334efe5121f5e316a805709e217bb6b25e686c73d731f7a87b60d7aa3b321aa7 -->
<!-- api: M:Inno.Rendering.Runtime.IRenderResourceService.PrewarmMaterial(Inno.Rendering.Assets.MaterialAsset) 934b4aac80b5952de42a4b5a21a01a841f71d661e0f38394456e68df96e4e77c -->
<!-- api: M:Inno.Rendering.Runtime.IRenderResourceService.PrewarmTexture(Inno.Rendering.Assets.TextureAsset) 8fbba8e7506337f1f6d1c9a5b80af4ab032cff84381433ce73832f2f38c4eccb -->
<!-- api: M:Inno.Rendering.Runtime.IRenderResourceService.PrewarmTextureArtifact(Inno.Rendering.Assets.RenderTextureArtifactReference) 598142f4bc27c17af7dc531416fc15e6263d6f311970188310deb3b9e70374ac -->
<!-- api: M:Inno.Rendering.Runtime.IRenderResourceService.AcquireBuffer(Inno.Rendering.Runtime.RenderPersistentResourceId,System.Int64,Inno.Rendering.PersistentBufferDescriptor,System.ReadOnlyMemory{System.Byte},System.String) c196473eddbb0b5af3e17ce56b2788be22c4d82f2992f5bf1766b6afa869fe7f -->
<!-- api: M:Inno.Rendering.Runtime.IRenderResourceService.AcquireTexture(Inno.Rendering.Runtime.RenderPersistentResourceId,System.Int64,Inno.Rendering.RenderTextureDescriptor,System.Collections.Generic.IReadOnlyList{Inno.Rendering.Runtime.RenderTextureSubresourceData},System.String) e35394d7e0bafae4fe672d1601a1ca9fe30b673563fafd6bd7117781a2bf9b4f -->
<!-- api: M:Inno.Rendering.Runtime.IRenderResourceService.AcquireKtxTexture(Inno.Rendering.Runtime.RenderPersistentResourceId,System.Int64,System.ReadOnlyMemory{System.Byte},System.Boolean,System.String) 21c5bfc02ad0828ac8c7eaf3f4f59ed02bf6272bb69b1ac644e972da72b1b98f -->
<!-- api: M:Inno.Rendering.Runtime.IRenderResourceService.UpdateTexture(Inno.Rendering.PersistentTextureHandle,Inno.Rendering.RenderTextureRegion,System.ReadOnlyMemory{System.Byte}) 04245b1a8fc6ad569a1fcb18d1d2a4312acca06631ee0d759f85fe7ed16097e1 -->
<!-- api: M:Inno.Rendering.Runtime.IRenderResourceService.ReadTextureAsync(Inno.Rendering.PersistentTextureHandle,System.Int32,System.Threading.CancellationToken) c3c153614ca627eb72567aa017b0fdd6c8123793e862664ae3748aba7908572e -->
<!-- api: M:Inno.Rendering.Runtime.IRenderResourceService.AcquireGraphicsPipeline(Inno.Rendering.Runtime.RenderPersistentResourceId,System.Int64,Inno.Rendering.GraphicsPipelineDescriptor,System.String) ce31ccd6480d3ab4ecacd8c3f71b5cc38a4cb6b5d0bee2cd211165a74bb49dd4 -->
<!-- api: M:Inno.Rendering.Runtime.IRenderResourceService.AcquireComputePipeline(Inno.Rendering.Runtime.RenderPersistentResourceId,System.Int64,Inno.Rendering.ComputePipelineDescriptor,System.String) e6915bc18dbb05856c0ea1b906c6228c99eeb750dd3a3abc26dec91044fe4f58 -->
<!-- api: M:Inno.Rendering.Runtime.IRenderResourceService.TryResolveGraphicsMaterial(Inno.Rendering.Assets.MaterialAsset,Inno.Rendering.Assets.ShaderContractId,Inno.Rendering.Assets.ShaderPassRoleId,Inno.Rendering.RenderVertexLayout,Inno.Rendering.Assets.MaterialPropertyBlock,Inno.Rendering.Runtime.RenderMaterialPass@) cf9f4caf697e4f47b8f2d0ebba333e982986339210e6812f12097b1f1d877666 -->
<!-- api: M:Inno.Rendering.Runtime.IRenderResourceService.TryResolveMaterialArtifact(Inno.Rendering.Runtime.RenderPersistentResourceId,Inno.Rendering.Assets.RenderShaderArtifact,Inno.Rendering.Assets.MaterialAsset,Inno.Rendering.Assets.ShaderContractId,Inno.Rendering.Assets.ShaderPassRoleId,Inno.Rendering.ShaderProgramKind,Inno.Rendering.RenderVertexLayout,Inno.Rendering.Assets.MaterialPropertyBlock,Inno.Core.Diagnostics.IDiagnosticReporter,Inno.Rendering.Runtime.RenderMaterialPass@) 4923a93cf3f7ecdd761d921878cfdf1bb025c48166691bc1253c0703b4cfc088 -->
<!-- api: M:Inno.Rendering.Runtime.IRenderResourceService.TryResolveComputeMaterial(Inno.Rendering.Assets.MaterialAsset,Inno.Rendering.Assets.ShaderContractId,Inno.Rendering.Assets.ShaderPassRoleId,Inno.Rendering.Assets.MaterialPropertyBlock,Inno.Rendering.Runtime.RenderMaterialPass@) 6e8d413d796c346e30b58bcf6bdae1daade1d7a2c26328521561af19509afcde -->
<!-- api: M:Inno.Rendering.Runtime.IRenderResourceService.TryResolveGeometry(Inno.Rendering.Assets.GeometryAsset,Inno.Rendering.Runtime.RenderGeometry@) c14942af940319af5bc5f6c60add26b11744589a481af39234c0f08723084738 -->
<!-- api: M:Inno.Rendering.Runtime.IRenderResourceService.TryResolveTexture(Inno.Rendering.Assets.TextureAsset,Inno.Rendering.PersistentTextureHandle@) a1fcf06d6095972c5c0e09a2e65e30da7e86477d070b75e85ba618f6f81323a9 -->
<!-- api: M:Inno.Rendering.Runtime.IRenderResourceService.TryResolveTextureArtifact(Inno.Rendering.Assets.RenderTextureArtifactReference,Inno.Rendering.PersistentTextureHandle@) 0e300cdca8633872c24620b8a141cf842d1e6906e986a4d3a2d70e017bcf4f9a -->
<!-- api: M:Inno.Rendering.Runtime.IRenderResourceService.Release(Inno.Rendering.Runtime.RenderPersistentResourceId) 13d746639110de9857bdaa83c2ec7851796fc00fddda9cd723c1860bb3c1abd4 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-88887d83fabf7ff1"></a>[`Inno.Rendering.PersistentBufferHandle Inno.Rendering.Runtime.IRenderResourceService.AcquireBuffer(Inno.Rendering.Runtime.RenderPersistentResourceId id, long revision, Inno.Rendering.PersistentBufferDescriptor descriptor, System.ReadOnlyMemory<byte> initialData, string name)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L89) | Acquires or atomically replaces a provider-owned persistent buffer.<br>返回：A handle owned by the active device generation.<br>`id`：Globally stable provider-owned resource ID.<br>`revision`：Provider-controlled content revision.<br>`descriptor`：Complete buffer descriptor.<br>`initialData`：Complete initial bytes.<br>`name`：Diagnostic name. | — | — |
| <a id="api-b36b3bc41557306a"></a>[`Inno.Rendering.ComputePipelineHandle Inno.Rendering.Runtime.IRenderResourceService.AcquireComputePipeline(Inno.Rendering.Runtime.RenderPersistentResourceId id, long revision, Inno.Rendering.ComputePipelineDescriptor descriptor, string name)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L237) | Acquires or atomically replaces a provider-owned compute pipeline.<br>返回：A compute pipeline owned by the active device generation.<br>`id`：Globally stable provider-owned resource ID.<br>`revision`：Provider-controlled content revision.<br>`descriptor`：Complete target compute program and interface.<br>`name`：Diagnostic name. | — | — |
| <a id="api-e7b89e2336455cba"></a>[`Inno.Rendering.GraphicsPipelineHandle Inno.Rendering.Runtime.IRenderResourceService.AcquireGraphicsPipeline(Inno.Rendering.Runtime.RenderPersistentResourceId id, long revision, Inno.Rendering.GraphicsPipelineDescriptor descriptor, string name)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L212) | Acquires or atomically replaces a provider-owned graphics pipeline.<br>返回：A graphics pipeline owned by the active device generation.<br>`id`：Globally stable provider-owned resource ID.<br>`revision`：Provider-controlled content revision.<br>`descriptor`：Complete target program, interface, layout, and raster state.<br>`name`：Diagnostic name. | — | — |
| <a id="api-d31224e2e2256acd"></a>[`Inno.Rendering.PersistentTextureHandle Inno.Rendering.Runtime.IRenderResourceService.AcquireKtxTexture(Inno.Rendering.Runtime.RenderPersistentResourceId id, long revision, System.ReadOnlyMemory<byte> containerData, bool sRgb, string name)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L147) | Acquires or atomically replaces a sampled texture from a portable KTX container.<br>返回：A handle owned by the active device generation.<br>`id`：Globally stable provider-owned resource ID.<br>`revision`：Provider-controlled content revision.<br>`containerData`：Complete validated KTX bytes.<br>`sRgb`：Whether texture sampling performs sRGB decoding.<br>`name`：Diagnostic name. | — | — |
| <a id="api-3d2a93ef925f3884"></a>[`Inno.Rendering.PersistentTextureHandle Inno.Rendering.Runtime.IRenderResourceService.AcquireTexture(Inno.Rendering.Runtime.RenderPersistentResourceId id, long revision, Inno.Rendering.RenderTextureDescriptor descriptor, System.Collections.Generic.IReadOnlyList<Inno.Rendering.Runtime.RenderTextureSubresourceData> subresources, string name)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L118) | Acquires or atomically replaces a provider-owned persistent texture.<br>返回：A handle owned by the active device generation.<br>`id`：Globally stable provider-owned resource ID.<br>`revision`：Provider-controlled content revision.<br>`descriptor`：Complete texture descriptor.<br>`subresources`：Complete mip and layer uploads.<br>`name`：Diagnostic name. | — | — |
| <a id="api-4f157b290cb5c29f"></a>[`Inno.Rendering.Runtime.RenderTargetArtifactStatus Inno.Rendering.Runtime.IRenderResourceService.PrewarmMaterial(Inno.Rendering.Assets.MaterialAsset material)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L50) | Queues all target shader work required by a material without blocking the render thread.<br>返回：The exact target artifact state for this material variant: Ready includes a usable last-good artifact, Pending means compilation is ongoing, and Failed or Unavailable means no artifact can currently be used.<br>`material`：Material whose selected static variant should be prepared. | — | `System.ArgumentException`：The material has no shader or declares an invalid static variant.<br>`System.InvalidOperationException`：No target artifact provider is available for this runtime. |
| <a id="api-3f0fa174289b7ebe"></a>[`void Inno.Rendering.Runtime.IRenderResourceService.PrewarmTexture(Inno.Rendering.Assets.TextureAsset texture)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L58) | Queues target texture conversion without blocking the render thread.<br>`texture`：Texture source to prepare. | — | — |
| <a id="api-ea9783c64f660b81"></a>[`void Inno.Rendering.Runtime.IRenderResourceService.PrewarmTextureArtifact(Inno.Rendering.Assets.RenderTextureArtifactReference texture)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L66) | Queues target conversion for a texture slot owned by any imported asset.<br>`texture`：Stable texture artifact reference to prepare. | — | — |
| <a id="api-a80b792801acaf29"></a>[`System.Threading.Tasks.ValueTask<Inno.Rendering.RenderTextureReadbackResult> Inno.Rendering.Runtime.IRenderResourceService.ReadTextureAsync(Inno.Rendering.PersistentTextureHandle texture, int mipLevel = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L188) | Asynchronously copies one complete persistent texture mip into CPU-visible memory.<br>返回：The immutable readback result after a later GPU frame completes it.<br>`texture`：Texture created with Inno.Rendering.RenderTextureUsage.Readback.<br>`mipLevel`：Zero-based mip level.<br>`cancellationToken`：Cancellation for the caller's wait. | — | — |
| <a id="api-437525fb433feea5"></a>[`void Inno.Rendering.Runtime.IRenderResourceService.Release(Inno.Rendering.Runtime.RenderPersistentResourceId id)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L417) | Releases cached resources with this provider-owned identifier at a safe GPU mutation point.<br>`id`：Stable resource identifier to release. | Calls between frames or during graph execution queue neutral identities until the next frame; runtime shutdown also retires them. Calls must occur on the rendering owner thread. Unused identities are ignored. | — |
| <a id="api-48780bf270a5a596"></a>[`bool Inno.Rendering.Runtime.IRenderResourceService.TryResolveComputeMaterial(Inno.Rendering.Assets.MaterialAsset material, Inno.Rendering.Assets.ShaderContractId contractId, Inno.Rendering.Assets.ShaderPassRoleId passRoleId, Inno.Rendering.Assets.MaterialPropertyBlock? overrides, out Inno.Rendering.Runtime.RenderMaterialPass? materialPass)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L350) | Resolves one compute material pass through an open provider contract and role.<br>返回：True when a current or last-good pass is usable.<br>`material`：Material asset to resolve.<br>`contractId`：Provider-owned shader contract.<br>`passRoleId`：Provider-owned pass role.<br>`overrides`：Optional frame-local material values.<br>`materialPass`：Receives the resolved pass when successful. | — | — |
| <a id="api-067795da2a83cfa5"></a>[`bool Inno.Rendering.Runtime.IRenderResourceService.TryResolveGeometry(Inno.Rendering.Assets.GeometryAsset geometry, out Inno.Rendering.Runtime.RenderGeometry? resolvedGeometry)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L370) | Resolves imported helper geometry into persistent vertex and index buffers.<br>返回：True when current or last-good geometry is usable.<br>`geometry`：Imported geometry asset.<br>`resolvedGeometry`：Receives generation-scoped GPU geometry when successful. | — | — |
| <a id="api-b02c6b5d267ee1a0"></a>[`bool Inno.Rendering.Runtime.IRenderResourceService.TryResolveGraphicsMaterial(Inno.Rendering.Assets.MaterialAsset material, Inno.Rendering.Assets.ShaderContractId contractId, Inno.Rendering.Assets.ShaderPassRoleId passRoleId, Inno.Rendering.RenderVertexLayout? vertexLayout, Inno.Rendering.Assets.MaterialPropertyBlock? overrides, out Inno.Rendering.Runtime.RenderMaterialPass? materialPass)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L268) | Resolves one graphics material pass through an open provider contract and role.<br>返回：True when a current or last-good pass is usable.<br>`material`：Material asset to resolve.<br>`contractId`：Provider-owned shader contract.<br>`passRoleId`：Provider-owned pass role.<br>`vertexLayout`：Expected vertex layout, or null for procedural geometry.<br>`overrides`：Optional frame-local material values.<br>`materialPass`：Receives the resolved pass when successful. | — | — |
| <a id="api-830568ad6e5578e5"></a>[`bool Inno.Rendering.Runtime.IRenderResourceService.TryResolveMaterialArtifact(Inno.Rendering.Runtime.RenderPersistentResourceId scope, Inno.Rendering.Assets.RenderShaderArtifact artifact, Inno.Rendering.Assets.MaterialAsset material, Inno.Rendering.Assets.ShaderContractId contractId, Inno.Rendering.Assets.ShaderPassRoleId passRoleId, Inno.Rendering.ShaderProgramKind programKind, Inno.Rendering.RenderVertexLayout? vertexLayout, Inno.Rendering.Assets.MaterialPropertyBlock? overrides, Inno.Core.Diagnostics.IDiagnosticReporter diagnostics, out Inno.Rendering.Runtime.RenderMaterialPass? materialPass)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L316) | Resolves an explicitly compiled candidate in a caller-owned publication scope, without publishing it as an asset.<br>返回：True when the isolated publication can satisfy this request; failure never changes canonical programs.<br>`scope`：Non-empty resource identity for this isolated consumer; release it when the consumer closes.<br>`artifact`：Complete immutable candidate built for the active device and exact Material variant.<br>`material`：Invocation-local values and a persistent Shader reference; neither asset is changed or retained.<br>`contractId`：Required provider-owned Shader contract.<br>`passRoleId`：Required role within the selected Technique.<br>`programKind`：Required graphics or compute program kind.<br>`vertexLayout`：Graphics layout, or null for procedural or compute work.<br>`overrides`：Optional invocation-local Material values.<br>`diagnostics`：Consumer-owned diagnostic reporter, not retained or forwarded to canonical asset diagnostics.<br>`materialPass`：Receives the current or complete last-good scoped pass. | — | `System.InvalidOperationException`：No artifact decoder is configured, or resource retirement cannot complete. |
| <a id="api-d69edd1905522226"></a>[`bool Inno.Rendering.Runtime.IRenderResourceService.TryResolveTexture(Inno.Rendering.Assets.TextureAsset texture, out Inno.Rendering.PersistentTextureHandle resolvedTexture)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L387) | Resolves an imported texture into a persistent sampled texture.<br>返回：True when current or last-good texture content is usable.<br>`texture`：Imported texture asset.<br>`resolvedTexture`：Receives a generation-scoped texture handle when successful. | — | — |
| <a id="api-2d343557a12a4412"></a>[`bool Inno.Rendering.Runtime.IRenderResourceService.TryResolveTextureArtifact(Inno.Rendering.Assets.RenderTextureArtifactReference texture, out Inno.Rendering.PersistentTextureHandle resolvedTexture)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L404) | Resolves one imported artifact texture into a persistent sampled texture.<br>返回：true when current or last-good texture content is usable.<br>`texture`：Stable texture artifact reference.<br>`resolvedTexture`：Receives a generation-scoped texture handle when successful. | — | — |
| <a id="api-e54c149883d2fc15"></a>[`void Inno.Rendering.Runtime.IRenderResourceService.UpdateTexture(Inno.Rendering.PersistentTextureHandle texture, Inno.Rendering.RenderTextureRegion region, System.ReadOnlyMemory<byte> data)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L167) | Updates a rectangular region of an active persistent texture without recreating it.<br>`texture`：Texture owned by the active device generation.<br>`region`：Destination mip, texel rectangle, and layer range.<br>`data`：Tightly packed region bytes. | — | — |
| <a id="api-4bcee44788459485"></a>[`void Inno.Rendering.Runtime.IRenderResourceService.ValidateShaderArtifact(Inno.Rendering.Assets.RenderShaderArtifact artifact)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L32) | Creates and retires every program in one compiled artifact through the active device without publishing it.<br>`artifact`：Complete target artifact whose binaries, links, and reflected interfaces must be accepted by the active device. | — | `System.InvalidOperationException`：The call is outside a frame resource-mutation safety point, or the artifact fails device validation. |
| <a id="api-b1bf3a3054981a7e"></a>[`Inno.Rendering.GraphicsCapabilities Inno.Rendering.Runtime.IRenderResourceService.capabilities { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L21) | Gets the active backend-neutral capability snapshot. | — | — |
| <a id="api-edeeacffe2ee4bd7"></a>[`public interface Inno.Rendering.Runtime.IRenderResourceService`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/IRenderResourceService.cs#L16) | Resolves neutral assets and provider-owned uploads into opaque resources for the active device generation. | — | — |

### `Inno.Rendering.Runtime.IRenderRuntimeReloadTransaction`

<!-- api: T:Inno.Rendering.Runtime.IRenderRuntimeReloadTransaction 668f5fdf074747f04ee8facb42035026f331f235cebc0d86b42b40ab56794c92 -->
<!-- api: M:Inno.Rendering.Runtime.IRenderRuntimeReloadTransaction.Prepare e2f5b3eb0d3b9fd283e8879cf98c8a1faf96fa644c5a244785443e8002506ff9 -->
<!-- api: M:Inno.Rendering.Runtime.IRenderRuntimeReloadTransaction.Activate 12514f9bf973aa6345baba303b45ff95d618739d394d83fe6bba707cd0173945 -->
<!-- api: M:Inno.Rendering.Runtime.IRenderRuntimeReloadTransaction.Complete 3e4cee8f675cf1e2dc8407c2a5af701d9776316b93022744bd7a4c4c396cc796 -->
<!-- api: M:Inno.Rendering.Runtime.IRenderRuntimeReloadTransaction.Rollback 5809ce74f9026559f6a8e8d77a4122e2df3ca173a7313b0f45a3c4bf33396f4d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3fc982648beee4f1"></a>[`void Inno.Rendering.Runtime.IRenderRuntimeReloadTransaction.Activate()`](../../src/services/rendering/Inno.Rendering.Runtime/IRenderRuntimeReloadTransaction.cs#L16) | Atomically selects the prepared candidate without retiring the previous generation. | — | — |
| <a id="api-1d4334d44b8473e6"></a>[`void Inno.Rendering.Runtime.IRenderRuntimeReloadTransaction.Complete()`](../../src/services/rendering/Inno.Rendering.Runtime/IRenderRuntimeReloadTransaction.cs#L21) | Commits an activated candidate and retires the previous generation. | — | — |
| <a id="api-2b108b290bafbb67"></a>[`void Inno.Rendering.Runtime.IRenderRuntimeReloadTransaction.Prepare()`](../../src/services/rendering/Inno.Rendering.Runtime/IRenderRuntimeReloadTransaction.cs#L11) | Builds and validates every candidate registry and last-good rendering generation. | — | — |
| <a id="api-c863881914735069"></a>[`void Inno.Rendering.Runtime.IRenderRuntimeReloadTransaction.Rollback()`](../../src/services/rendering/Inno.Rendering.Runtime/IRenderRuntimeReloadTransaction.cs#L26) | Discards the candidate and restores the previous generation after provisional activation. | — | — |
| <a id="api-8f69ea17f6da3d43"></a>[`public interface Inno.Rendering.Runtime.IRenderRuntimeReloadTransaction`](../../src/services/rendering/Inno.Rendering.Runtime/IRenderRuntimeReloadTransaction.cs#L6) | Controls one isolated rendering-extension candidate from preparation through atomic completion. | — | — |

### `Inno.Rendering.Runtime.IRenderTargetArtifactProvider`

<!-- api: T:Inno.Rendering.Runtime.IRenderTargetArtifactProvider 57f8de17a9013b434219af33ad9729df387374d3a28c6ce6e632bcf28d0c464f -->
<!-- api: M:Inno.Rendering.Runtime.IRenderTargetArtifactProvider.ReadShaderDefinition(Inno.Rendering.Assets.RenderShaderArtifact) 5c14d9497306e7e3628884c30d823bd6a5744f47ed1641a28b36d3ee7ba80e82 -->
<!-- api: M:Inno.Rendering.Runtime.IRenderTargetArtifactProvider.GetShaderArtifact(Inno.Rendering.Assets.ShaderAsset,Inno.Rendering.Assets.RenderShaderVariant,Inno.Rendering.GraphicsCapabilities,Inno.Rendering.Assets.RenderShaderArtifact@) 3609c14c9ed374fcd32a3c2cc29ef1c4843cbcd5d4ba6f1ad93771a975cbadd5 -->
<!-- api: M:Inno.Rendering.Runtime.IRenderTargetArtifactProvider.GetTextureArtifact(Inno.Rendering.Assets.RenderTextureArtifactReference,System.ReadOnlyMemory{System.Byte}@) d9af13029a2a0f33e5856c2f966d5cccb0fb8752ad56e28289080827dff7fdf6 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-944da27f32e5f1a4"></a>[`Inno.Rendering.Runtime.RenderTargetArtifactStatus Inno.Rendering.Runtime.IRenderTargetArtifactProvider.GetShaderArtifact(Inno.Rendering.Assets.ShaderAsset shader, Inno.Rendering.Assets.RenderShaderVariant variant, Inno.Rendering.GraphicsCapabilities capabilities, out Inno.Rendering.Assets.RenderShaderArtifact? artifact)`](../../src/services/rendering/Inno.Rendering.Runtime/Deployment/IRenderTargetArtifactProvider.cs#L49) | Resolves the target shader matching one runtime asset, variant, and device capability snapshot.<br>返回：The current artifact availability. Inno.Rendering.Runtime.RenderTargetArtifactStatus.Ready guarantees that artifact is non-null.<br>`shader`：The imported runtime shader description.<br>`variant`：The exact material keyword selection.<br>`capabilities`：The active device capabilities used to select a packaged backend target.<br>`artifact`：Receives the validated source-free artifact when it exists. | — | `System.ArgumentException`：Thrown when shader or capabilities is null. |
| <a id="api-ad1576c3a9c153f6"></a>[`Inno.Rendering.Runtime.RenderTargetArtifactStatus Inno.Rendering.Runtime.IRenderTargetArtifactProvider.GetTextureArtifact(Inno.Rendering.Assets.RenderTextureArtifactReference texture, out System.ReadOnlyMemory<byte> artifact)`](../../src/services/rendering/Inno.Rendering.Runtime/Deployment/IRenderTargetArtifactProvider.cs#L72) | Resolves the portable KTX artifact for one imported runtime texture.<br>返回：The current artifact availability. Inno.Rendering.Runtime.RenderTargetArtifactStatus.Ready guarantees that artifact is non-empty.<br>`texture`：Stable reference to the imported texture slot.<br>`artifact`：Receives immutable KTX bytes when the artifact exists and is non-empty. | — | `System.ArgumentNullException`：Thrown when texture is invalid. |
| <a id="api-5d4f1ca42118655a"></a>[`Inno.Rendering.Assets.ShaderDefinition Inno.Rendering.Runtime.IRenderTargetArtifactProvider.ReadShaderDefinition(Inno.Rendering.Assets.RenderShaderArtifact artifact)`](../../src/services/rendering/Inno.Rendering.Runtime/Deployment/IRenderTargetArtifactProvider.cs#L25) | Resolves the exact material contract stored with a compiled program using the current owner reference context.<br>返回：A detached definition belonging to this artifact, never the latest uncompiled source definition.<br>`artifact`：One immutable program publication, including its native-serialized runtime definition. | — | `System.InvalidOperationException`：The publication cannot be decoded in the current owner generation. |
| <a id="api-e4b5ceb7a8cb047c"></a>[`public interface Inno.Rendering.Runtime.IRenderTargetArtifactProvider`](../../src/services/rendering/Inno.Rendering.Runtime/Deployment/IRenderTargetArtifactProvider.cs#L11) | Resolves immutable, source-free target artifacts for the active rendering device. | — | — |

### `Inno.Rendering.Runtime.IViewContentCollector`

<!-- api: T:Inno.Rendering.Runtime.IViewContentCollector b2fa72d6f64cf10b66280530e7a3cfc5961f1aba5223724cc2441ae2f7fc2700 -->
<!-- api: M:Inno.Rendering.Runtime.IViewContentCollector.Collect(Inno.Rendering.Runtime.ViewContentContext) 7361c9676c780d682d8cf0ba8a65d25141459cc1c0c88227db032e06cd83a5b7 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-708cc3336ca8b7ba"></a>[`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Runtime.ViewContentItem> Inno.Rendering.Runtime.IViewContentCollector.Collect(Inno.Rendering.Runtime.ViewContentContext context)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L439) | Collects active source items in deterministic source order.<br>返回：Frame-local items; the caller owns their sort and composition.<br>`context`：Frame-scoped host content and view. | — | — |
| <a id="api-81770319858ed7dd"></a>[`public interface Inno.Rendering.Runtime.IViewContentCollector`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L428) | Collects all active world-content sources for one exact view. | — | — |

### `Inno.Rendering.Runtime.IViewContentFrameSource`

<!-- api: T:Inno.Rendering.Runtime.IViewContentFrameSource 2316c20c65f10d3f5329dd03d5d094719833dce346dd70878f9e2e4cb2992911 -->
<!-- api: M:Inno.Rendering.Runtime.IViewContentFrameSource.CompleteFrame(System.UInt64) bacd130ab498acb4bb0004ca24dcf1b53a935abfee625b56c5040bc23e2d3c32 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-022a33cccc2c727e"></a>[`void Inno.Rendering.Runtime.IViewContentFrameSource.CompleteFrame(ulong frameIndex)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L396) | Advances retained content once after all output views have supplied input.<br>`frameIndex`：The shared render frame whose input routing has completed. | — | — |
| <a id="api-1fb33473ed530fa8"></a>[`public interface Inno.Rendering.Runtime.IViewContentFrameSource`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L388) | Completes frame-local input after every output has routed its views. | — | — |

### `Inno.Rendering.Runtime.IViewContentSink`

<!-- api: T:Inno.Rendering.Runtime.IViewContentSink 7b45b52b43acffdc5de2abb6ce86635d80c96569f33adab2f3913505a348c8c1 -->
<!-- api: M:Inno.Rendering.Runtime.IViewContentSink.Submit(Inno.Rendering.Runtime.ViewContentItem) 163060482ec732b67c53fd2f5399d56bbe0465f21942d5a7b2cf0155dfe91785 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-16325c554344042a"></a>[`void Inno.Rendering.Runtime.IViewContentSink.Submit(Inno.Rendering.Runtime.ViewContentItem item)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L360) | Adds an item to the current view.<br>`item`：Frame-local content item. | — | — |
| <a id="api-994b9d57d43bbe97"></a>[`public interface Inno.Rendering.Runtime.IViewContentSink`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L352) | Receives world items without imposing a rendering-model sort key. | — | — |

### `Inno.Rendering.Runtime.IViewContentSource`

<!-- api: T:Inno.Rendering.Runtime.IViewContentSource 4030936ee8ad87d90c09364f50018ea330d3ad0db86eeb98d7ba513ce8eff575 -->
<!-- api: M:Inno.Rendering.Runtime.IViewContentSource.Collect(Inno.Rendering.Runtime.ViewContentContext,Inno.Rendering.Runtime.IViewContentSink) e2a8e792a9adbd3765698a662bb7f7a3852fc785f6dca2ea0bee35bdccdc20f1 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a659d34c5c041c1c"></a>[`void Inno.Rendering.Runtime.IViewContentSource.Collect(Inno.Rendering.Runtime.ViewContentContext context, Inno.Rendering.Runtime.IViewContentSink sink)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L378) | Collects items for one exact view.<br>`context`：Frame-scoped host content and view.<br>`sink`：Collector receiving items. | — | — |
| <a id="api-49f0f73bdcec6d13"></a>[`public interface Inno.Rendering.Runtime.IViewContentSource : System.IDisposable`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L367) | Contributes model-independent world items to selected views. | — | — |

### `Inno.Rendering.Runtime.IViewDrawable`

<!-- api: T:Inno.Rendering.Runtime.IViewDrawable 7c16f9602f99650abc396e801f6399f682ccd83cf7498de1ab45f2af96448a3d -->
<!-- api: M:Inno.Rendering.Runtime.IViewDrawable.TryPrepare(Inno.Rendering.Runtime.RenderPipelineContext,Inno.Rendering.Runtime.RenderView,Inno.Rendering.Runtime.IPreparedViewDrawable@) 0afcd31f204fde1c442f54852fc99d8711ed6460f7ad0b2ca22303b098ce8d07 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-47e5ac96c2353b0c"></a>[`bool Inno.Rendering.Runtime.IViewDrawable.TryPrepare(Inno.Rendering.Runtime.RenderPipelineContext context, Inno.Rendering.Runtime.RenderView view, out Inno.Rendering.Runtime.IPreparedViewDrawable? prepared)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L188) | Resolves reusable resources before render graph execution.<br>返回：Whether the drawable is ready for this pass.<br>`context`：Active pipeline build context.<br>`view`：View that will draw this item.<br>`prepared`：Prepared frame-local draw when resources are available. | — | — |
| <a id="api-d9068c01082c62f6"></a>[`public interface Inno.Rendering.Runtime.IViewDrawable`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L171) | Prepares one model-independent drawable for a specific render pass. | — | — |

### `Inno.Rendering.Runtime.IViewPointerTarget`

<!-- api: T:Inno.Rendering.Runtime.IViewPointerTarget b6f30bbcde6a577e01c71e504b57e3f63d4d3b26ae67e4c4dc1e4dd13bab0243 -->
<!-- api: P:Inno.Rendering.Runtime.IViewPointerTarget.hasPointerCapture cf7ece2fe16f388c1796c7b8236d82ae6b0402783bba8be956f8273bf4401476 -->
<!-- api: P:Inno.Rendering.Runtime.IViewPointerTarget.hasKeyboardFocus d14ce882fd916a61816cb1ebf4dc17be11a473410b9c388da816ce44b73fef4c -->
<!-- api: M:Inno.Rendering.Runtime.IViewPointerTarget.SetKeyboardFocus(System.Boolean) 7d5bb79ab98f8b9beb90e3f26f5538bfd4599f9555d2a0f0c5b27e77c9d1b53e -->
<!-- api: M:Inno.Rendering.Runtime.IViewPointerTarget.TryHit(Inno.Rendering.Runtime.RenderView,Inno.Rendering.Runtime.RenderOutputInput,Inno.Core.Mathematics.Vector2@) 9165cf061a158893da294e4c72f5b5785dbb6f6ae57f126d031fabaac1195011 -->
<!-- api: M:Inno.Rendering.Runtime.IViewPointerTarget.Advance(Inno.Rendering.Runtime.RenderOutputInput,Inno.Core.Mathematics.Vector2,System.UInt64) b5d9039ccb8d406d3c98cd53317df217207f5bf3a5f7cd7fe9211c6ccc089423 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9a36b1eebb5a8e68"></a>[`void Inno.Rendering.Runtime.IViewPointerTarget.Advance(Inno.Rendering.Runtime.RenderOutputInput input, Inno.Core.Mathematics.Vector2 localPosition, ulong frameIndex)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L268) | Advances this target once for the frame with either routed input or an empty snapshot.<br>`input`：Routed viewport-local input, or an empty snapshot.<br>`localPosition`：Target-local pointer coordinates.<br>`frameIndex`：Shared output frame index. | — | — |
| <a id="api-eb2e42ab982d14e5"></a>[`void Inno.Rendering.Runtime.IViewPointerTarget.SetKeyboardFocus(bool focused)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L233) | Changes keyboard focus after the rendering model resolves a pointer press.<br>`focused`：Whether this target owns subsequent keyboard input. | — | — |
| <a id="api-cf88b5549af22fb5"></a>[`bool Inno.Rendering.Runtime.IViewPointerTarget.TryHit(Inno.Rendering.Runtime.RenderView view, Inno.Rendering.Runtime.RenderOutputInput input, out Inno.Core.Mathematics.Vector2 localPosition)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L250) | Maps one output pointer onto this item's local surface.<br>返回：Whether this target is eligible to receive pointer input.<br>`view`：Exact rendered view.<br>`input`：Viewport-local input.<br>`localPosition`：Local target coordinates when hit. | — | — |
| <a id="api-b1d78deff6ba6d6e"></a>[`bool Inno.Rendering.Runtime.IViewPointerTarget.hasKeyboardFocus { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L225) | Gets whether keyboard and text input should continue reaching this target. | — | — |
| <a id="api-7c085abc7c8bfba0"></a>[`bool Inno.Rendering.Runtime.IViewPointerTarget.hasPointerCapture { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L220) | Gets whether this target retains pointer ownership from an earlier press. Rendering models route captured input before ordinary hit testing. | — | — |
| <a id="api-9dae3c019ed5e492"></a>[`public interface Inno.Rendering.Runtime.IViewPointerTarget`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L214) | Receives pointer input after the rendering model resolves visible draw order. | — | — |

### `Inno.Rendering.Runtime.RenderExtensionStateContext`

<!-- api: T:Inno.Rendering.Runtime.RenderExtensionStateContext d4c3e3262683d09595aebe4e78b9b4d5a6cd178648953bd7836ecc333a26eaeb -->
<!-- api: M:Inno.Rendering.Runtime.RenderExtensionStateContext.#ctor(Inno.Assets.AssetObject) d54e371612d1dc14a2d871778b40cb9c17816c8bebcd19e03edf9aa88e87bc1f -->
<!-- api: M:Inno.Rendering.Runtime.RenderExtensionStateContext.Restore``1(Inno.Rendering.Assets.SerializedRenderExtensionState,``0) 332e89f23ebc29c9477aae6514d27c97bf167a515cab4660d301465edf51408e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7e0f932279960240"></a>[`public Inno.Rendering.Runtime.RenderExtensionStateContext.RenderExtensionStateContext(Inno.Assets.AssetObject owner)`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderExtensionStateContext.cs#L26) | Binds restoration to a canonical asset, independently of the caller's ambient session.<br>`owner`：Asset whose database and pinned converter generation own the settings. | It must not outlive the asset generation. Authoring captures use the asset system's property snapshots. | — |
| <a id="api-5d83d614b6a96694"></a>[`public void Inno.Rendering.Runtime.RenderExtensionStateContext.Restore<TSettings>(Inno.Rendering.Assets.SerializedRenderExtensionState state, TSettings target) where TSettings : class, Inno.Core.Serialization.ISerializable`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderExtensionStateContext.cs#L40) | Restores a matching settings contract with this owner's asset references.<br>`state`：Detached neutral source state.<br>`target`：Current generation target, never retained by this context. | — | — |
| <a id="api-9750a5e278e01ef2"></a>[`public sealed class Inno.Rendering.Runtime.RenderExtensionStateContext`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderExtensionStateContext.cs#L13) | Restores neutral render settings through the canonical asset's actual owner and pinned references. | — | — |

### `Inno.Rendering.Runtime.RenderFeatureContext`

<!-- api: T:Inno.Rendering.Runtime.RenderFeatureContext 81ce4395c4c5427b2a2bc6ebb64b67ba757363633d9896c81ada986c03886848 -->
<!-- api: M:Inno.Rendering.Runtime.RenderFeatureContext.#ctor(Inno.Rendering.Runtime.RenderPipelineContext,Inno.Rendering.Assets.RenderFeatureConfiguration) 5dd97550c8c52c0e638c8c03178143115adb8fb36bbe4f424761b3d79411e76a -->
<!-- api: P:Inno.Rendering.Runtime.RenderFeatureContext.pipeline e5614700f3780f68ea36c51a2bc5250a05548526eb94aac6fd94e17901c99d0d -->
<!-- api: P:Inno.Rendering.Runtime.RenderFeatureContext.configuration 50199a7bdedd8ec1693b60e7123e0589a36b261319283577d813b9c6a1747781 -->
<!-- api: P:Inno.Rendering.Runtime.RenderFeatureContext.graph 0731e05063deb78f177d2de9f91894f16f4647cfad5de374d316c5fcd8ed9e16 -->
<!-- api: P:Inno.Rendering.Runtime.RenderFeatureContext.resources 434ed48068ddc10f5de28a791b0620552d3b628ccf664228d8fa40b82f60e316 -->
<!-- api: P:Inno.Rendering.Runtime.RenderFeatureContext.capabilities 500014e0f34c818457fe5308fc0c32813f387f0be56683cf5b46ae9bd3a4fe9a -->
<!-- api: P:Inno.Rendering.Runtime.RenderFeatureContext.resourceService dee1cd0c58728eaa41a5c6d6bc512725c09e12dad2488d355698928d26c90a24 -->
<!-- api: P:Inno.Rendering.Runtime.RenderFeatureContext.uploads 0179c68a4f5155ccd75ef51cb839beadb33be46743cfcaf9de0f247711b591f8 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-841368e3c146f31a"></a>[`public Inno.Rendering.Runtime.RenderFeatureContext.RenderFeatureContext(Inno.Rendering.Runtime.RenderPipelineContext pipeline, Inno.Rendering.Assets.RenderFeatureConfiguration configuration)`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderFeatureContext.cs#L26) | Creates a feature build context.<br>`pipeline`：Owning pipeline context.<br>`configuration`：Stable feature configuration. | — | — |
| <a id="api-e9dcfd2e3983b239"></a>[`public Inno.Rendering.GraphicsCapabilities Inno.Rendering.Runtime.RenderFeatureContext.capabilities { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderFeatureContext.cs#L57) | Gets current device capabilities. | — | — |
| <a id="api-ff27cd1d292b836c"></a>[`public Inno.Rendering.Assets.RenderFeatureConfiguration Inno.Rendering.Runtime.RenderFeatureContext.configuration { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderFeatureContext.cs#L42) | Gets stable feature configuration. | — | — |
| <a id="api-2da2e457180ce471"></a>[`public Inno.Rendering.RenderGraphBuilder Inno.Rendering.Runtime.RenderFeatureContext.graph { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderFeatureContext.cs#L47) | Gets the current frame graph builder. | — | — |
| <a id="api-396bd515de3bde7a"></a>[`public Inno.Rendering.Runtime.RenderPipelineContext Inno.Rendering.Runtime.RenderFeatureContext.pipeline { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderFeatureContext.cs#L37) | Gets the owning pipeline context. | — | — |
| <a id="api-6eb6b87e1b58e879"></a>[`public Inno.Rendering.Runtime.RenderResourceMap Inno.Rendering.Runtime.RenderFeatureContext.resources { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderFeatureContext.cs#L52) | Gets open semantic resources for the request. | — | — |
| <a id="api-3cbc20ab96a40392"></a>[`public Inno.Rendering.Runtime.IRenderResourceService Inno.Rendering.Runtime.RenderFeatureContext.resourceService { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderFeatureContext.cs#L62) | Gets the generation-aware neutral GPU resource service. | — | — |
| <a id="api-1869ae9cea224ec7"></a>[`public Inno.Rendering.IRenderFrameUploadService Inno.Rendering.Runtime.RenderFeatureContext.uploads { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderFeatureContext.cs#L67) | Gets the frame-scoped streaming buffer service. | — | — |
| <a id="api-1043e043c9d38ec8"></a>[`public sealed class Inno.Rendering.Runtime.RenderFeatureContext`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderFeatureContext.cs#L15) | Supplies one configured feature with frame-scoped graph services. | — | — |

### `Inno.Rendering.Runtime.RenderFeatureExtensionAttribute`

<!-- api: T:Inno.Rendering.Runtime.RenderFeatureExtensionAttribute 143e1bc1d827ac0164ed895b7e03bac69d054e1c98ac14e8eceaaeb69939e201 -->
<!-- api: M:Inno.Rendering.Runtime.RenderFeatureExtensionAttribute.#ctor(System.String) 8331cff184a00dd8e5c5a1427fe9d0f7f511ca8794097254aafcff7c8a96882b -->
<!-- api: P:Inno.Rendering.Runtime.RenderFeatureExtensionAttribute.id aa70cca7c04ff300248621d95c0a5a16bcb7fcfb854b4404344b16a7e503f2da -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-247bb6f04f5ac566"></a>[`public Inno.Rendering.Runtime.RenderFeatureExtensionAttribute.RenderFeatureExtensionAttribute(string id)`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderFeatureExtensionAttribute.cs#L24) | Creates a feature extension declaration.<br>`id`：Globally stable feature extension identifier. | — | — |
| <a id="api-cba6125b7eef4277"></a>[`public string Inno.Rendering.Runtime.RenderFeatureExtensionAttribute.id { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderFeatureExtensionAttribute.cs#L33) | Gets the globally stable feature extension identifier. | — | — |
| <a id="api-0c30728ce3360219"></a>[`public sealed class Inno.Rendering.Runtime.RenderFeatureExtensionAttribute : System.Attribute`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderFeatureExtensionAttribute.cs#L16) | Marks a reloadable pipeline feature implementation with a stable extension identifier. | — | — |

### `Inno.Rendering.Runtime.RenderFrameData`

<!-- api: T:Inno.Rendering.Runtime.RenderFrameData 847bb3f383f2ab639ffe76608df82dba80690e66c7d462a059457070e5a0e1bf -->
<!-- api: P:Inno.Rendering.Runtime.RenderFrameData.count 97b7c29067e4bd821e68989e24b58dc9fbaeae9b1ae7563e3e5968fccc0c368d -->
<!-- api: M:Inno.Rendering.Runtime.RenderFrameData.Set``1(Inno.Rendering.RenderDataChannelId,``0) 96605c9a000b07f88b1677bbfcf97a9fbe7f0421982c660f0816d12f910c52f9 -->
<!-- api: M:Inno.Rendering.Runtime.RenderFrameData.TryGet``1(Inno.Rendering.RenderDataChannelId,``0@) 024e4367177ca0b0a0c8c13aa716b10c8b8af88c14a2766ee1bcc572d4660113 -->
<!-- api: M:Inno.Rendering.Runtime.RenderFrameData.Clear 3bc2fc2323cbb3dfbda68f4f61025dbeb2610170bcd9c449600e9b3971bfbd1c -->
<!-- api: M:Inno.Rendering.Runtime.RenderFrameData.#ctor d05f57188545c394a4d1f19aa00a579592305e0925fda20a8ae9955fddac2438 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e8e57884f62b6c47"></a>[`public Inno.Rendering.Runtime.RenderFrameData.RenderFrameData()`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderFrameData.cs#L16) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-0c91ca0c63a3af9e"></a>[`public void Inno.Rendering.Runtime.RenderFrameData.Clear()`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderFrameData.cs#L92) | Removes all values before this object enters a submitted request. | — | `System.InvalidOperationException`：Thrown after the data has entered a request. |
| <a id="api-8ec70c9ed2ce4ee7"></a>[`public void Inno.Rendering.Runtime.RenderFrameData.Set<TValue>(Inno.Rendering.RenderDataChannelId channel, TValue value) where TValue : notnull`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderFrameData.cs#L41) | Adds or replaces one typed value in an open data channel.<br>`channel`：Pipeline-defined stable data channel.<br>`value`：Value retained until the current frame completes. | — | `System.InvalidOperationException`：Thrown after the data has entered a request. |
| <a id="api-7fa803dd9289994f"></a>[`public bool Inno.Rendering.Runtime.RenderFrameData.TryGet<TValue>(Inno.Rendering.RenderDataChannelId channel, out TValue? value)`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderFrameData.cs#L70) | Tries to read one typed value from an open data channel.<br>返回：true when the channel contains the exact requested type.<br>`channel`：Pipeline-defined stable data channel.<br>`value`：Receives the stored value when present. | — | — |
| <a id="api-afe8e2fb1c27e36b"></a>[`public int Inno.Rendering.Runtime.RenderFrameData.count { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderFrameData.cs#L24) | Gets the number of populated channel and value-type pairs. | — | — |
| <a id="api-9879687249dc6b19"></a>[`public sealed class Inno.Rendering.Runtime.RenderFrameData`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderFrameData.cs#L16) | Carries generation-scoped, frame-only payloads between a request producer and its pipeline. | Values may use reloadable plugin types because a snapshot is retained only until the owning render frame completes. Persistent configuration must use stable identifiers and serialized bytes. | — |

### `Inno.Rendering.Runtime.RenderFrameStatistics`

<!-- api: T:Inno.Rendering.Runtime.RenderFrameStatistics 9cbdc91e947511c2567f63884c098b643f2e0c6e1e6ed9a7d8331c4fd70f3f6b -->
<!-- api: M:Inno.Rendering.Runtime.RenderFrameStatistics.#ctor(System.UInt64,System.Int32,System.Int32,System.Int32,System.Int32,System.Nullable{Inno.Rendering.RenderDeviceAllocationCounters},System.Int32) 087e76afdf49b03b69df95a69f6738408ce5bb556958d52139737fe3da275f5e -->
<!-- api: P:Inno.Rendering.Runtime.RenderFrameStatistics.frameIndex 3cb4112c682c197cad1a08d6d234207a6c16017d6b374b69f1734554b2616772 -->
<!-- api: P:Inno.Rendering.Runtime.RenderFrameStatistics.viewCount 927a4df778f6d66417b15f51aa292a8de48a017dfee8aeb2f8a0eedf5deca757 -->
<!-- api: P:Inno.Rendering.Runtime.RenderFrameStatistics.drawCount 5740497624d60c63841b0e3e41000bb6360ebbef8b13cfdcae3227853e012e37 -->
<!-- api: P:Inno.Rendering.Runtime.RenderFrameStatistics.dispatchCount 9832517b1e3048c65fd1ba025bd9ede4ed6c7bb7c254429d6bc120f52a294dac -->
<!-- api: P:Inno.Rendering.Runtime.RenderFrameStatistics.culledPassCount 7fd2b0149caaed64d13caa3517b0ef5928e6d31c19dacef53d5f19723d458d73 -->
<!-- api: P:Inno.Rendering.Runtime.RenderFrameStatistics.allocationCounters 81bd1a822f969fba1028d741ef24734cdd3d1c8e084a453b49801bbe858ebffc -->
<!-- api: P:Inno.Rendering.Runtime.RenderFrameStatistics.graphCompileCount 8442e31d929748d5f139deaecf84efcbe2bca7a4660f43d4296bb550d1dbdebc -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-84bc83361d277b48"></a>[`public Inno.Rendering.Runtime.RenderFrameStatistics.RenderFrameStatistics(ulong frameIndex, int viewCount, int drawCount, int dispatchCount, int culledPassCount, Inno.Rendering.RenderDeviceAllocationCounters? allocationCounters, int graphCompileCount)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderFrameStatistics.cs#L36) | Creates an immutable frame statistics snapshot.<br>`frameIndex`：Monotonic render frame index.<br>`viewCount`：Executed logical view count.<br>`drawCount`：Recorded draw count.<br>`dispatchCount`：Recorded compute dispatch count.<br>`culledPassCount`：Passes removed by graph compilation.<br>`allocationCounters`：Device-generation cumulative transient allocations at frame completion, or null when unavailable.<br>`graphCompileCount`：Complete graph compilation attempts for this frame, excluding validation-only analysis. | — | — |
| <a id="api-47ede03cd9fe1b9b"></a>[`public Inno.Rendering.RenderDeviceAllocationCounters? Inno.Rendering.Runtime.RenderFrameStatistics.allocationCounters { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderFrameStatistics.cs#L88) | Gets the device-generation cumulative transient allocation snapshot at frame completion. Null means the backend does not report allocation accounting; it does not mean zero allocations. | — | — |
| <a id="api-6fb30c554de11684"></a>[`public int Inno.Rendering.Runtime.RenderFrameStatistics.culledPassCount { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderFrameStatistics.cs#L82) | Gets passes removed by graph compilation. | — | — |
| <a id="api-f5691ac5ae1988af"></a>[`public int Inno.Rendering.Runtime.RenderFrameStatistics.dispatchCount { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderFrameStatistics.cs#L77) | Gets the recorded compute dispatch count. | — | — |
| <a id="api-3c921e399998b16c"></a>[`public int Inno.Rendering.Runtime.RenderFrameStatistics.drawCount { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderFrameStatistics.cs#L72) | Gets the recorded draw count. | — | — |
| <a id="api-fce316b9c6c2bb2a"></a>[`public ulong Inno.Rendering.Runtime.RenderFrameStatistics.frameIndex { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderFrameStatistics.cs#L62) | Gets the monotonic render frame index. | — | — |
| <a id="api-6ff5bcd0c225a0a9"></a>[`public int Inno.Rendering.Runtime.RenderFrameStatistics.graphCompileCount { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderFrameStatistics.cs#L93) | Gets complete graph compilation attempts for the frame. Validation does not increase this count. | — | — |
| <a id="api-bfaadf80559d8ffe"></a>[`public int Inno.Rendering.Runtime.RenderFrameStatistics.viewCount { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderFrameStatistics.cs#L67) | Gets the executed logical view count. | — | — |
| <a id="api-469f8c262f4343f0"></a>[`public sealed class Inno.Rendering.Runtime.RenderFrameStatistics`](../../src/services/rendering/Inno.Rendering.Runtime/RenderFrameStatistics.cs#L10) | Reports read-only statistics for the most recently completed render frame. | — | — |

### `Inno.Rendering.Runtime.RenderGeometry`

<!-- api: T:Inno.Rendering.Runtime.RenderGeometry c99c934628e6cf3a7ff24d4422a55e60c77317ab1cef6547715b7c4b1428a8d9 -->
<!-- api: P:Inno.Rendering.Runtime.RenderGeometry.vertexBuffer ebc55d924f0d3db276eb2bcf38d0b1c19c16fe02fc440ff7aa64189db8729425 -->
<!-- api: P:Inno.Rendering.Runtime.RenderGeometry.indexBuffer de5038fa47c6df520cbb5d3604cd07917e9aaa5a9e1a3889029cf2e62680c697 -->
<!-- api: P:Inno.Rendering.Runtime.RenderGeometry.vertexLayout 959d45784c718ebcd5de2cc19471c3b7c4598358548af3a40b3db5d8d6c8a29d -->
<!-- api: P:Inno.Rendering.Runtime.RenderGeometry.vertexCount 8c3a81c4bf1cde5587a0d03b19de0ccc31044b6ab4eae1c53178a988912413d5 -->
<!-- api: P:Inno.Rendering.Runtime.RenderGeometry.indexCount 495b10e989323ed7477cb12de54601d684a0d9b8ca8af63ab26081e43fa1f6f2 -->
<!-- api: P:Inno.Rendering.Runtime.RenderGeometry.sections d1ec8e851a73a931123932bbc5505f5dc8249b92a97d1ae8007c813127954781 -->
<!-- api: M:Inno.Rendering.Runtime.RenderGeometry.Bind(Inno.Rendering.RenderCommandEncoder) 7cfe971992de41e717837326681f140e939a181f9ec46ebf24d037ec0063eae7 -->
<!-- api: M:Inno.Rendering.Runtime.RenderGeometry.DrawSection(Inno.Rendering.RenderCommandEncoder,System.Int32,System.Int32) 2b08daef5675473fb1d3087cf31c6909114445f8305ce08a6a573e61ababad4f -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8ea6394e06bb09f1"></a>[`public void Inno.Rendering.Runtime.RenderGeometry.Bind(Inno.Rendering.RenderCommandEncoder commands)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderGeometry.cs#L72) | Binds both geometry streams at their first element.<br>`commands`：Current pass command encoder. | — | — |
| <a id="api-b6924bf56a346d98"></a>[`public void Inno.Rendering.Runtime.RenderGeometry.DrawSection(Inno.Rendering.RenderCommandEncoder commands, int sectionIndex, int instanceCount = 1)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderGeometry.cs#L91) | Binds and draws one indexed section.<br>`commands`：Current raster pass command encoder.<br>`sectionIndex`：Zero-based section index.<br>`instanceCount`：Positive instance count. | — | — |
| <a id="api-5296d98370177cd9"></a>[`public Inno.Rendering.PersistentBufferHandle Inno.Rendering.Runtime.RenderGeometry.indexBuffer { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderGeometry.cs#L44) | Gets the persistent index buffer. | — | — |
| <a id="api-bf73e1479462c367"></a>[`public int Inno.Rendering.Runtime.RenderGeometry.indexCount { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderGeometry.cs#L59) | Gets the total index count. | — | — |
| <a id="api-f5bad7e94c00b63e"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Runtime.RenderGeometrySection> Inno.Rendering.Runtime.RenderGeometry.sections { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderGeometry.cs#L64) | Gets independently drawable indexed ranges. | — | — |
| <a id="api-040bc14ba20a6a2b"></a>[`public Inno.Rendering.PersistentBufferHandle Inno.Rendering.Runtime.RenderGeometry.vertexBuffer { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderGeometry.cs#L39) | Gets the persistent vertex buffer. | — | — |
| <a id="api-76a98201dcabeedb"></a>[`public int Inno.Rendering.Runtime.RenderGeometry.vertexCount { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderGeometry.cs#L54) | Gets the number of vertices. | — | — |
| <a id="api-6dcc9d6a8cb91273"></a>[`public Inno.Rendering.RenderVertexLayout Inno.Rendering.Runtime.RenderGeometry.vertexLayout { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderGeometry.cs#L49) | Gets the imported interleaved vertex layout. | — | — |
| <a id="api-5b25767f96eb9f6b"></a>[`public sealed class Inno.Rendering.Runtime.RenderGeometry`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderGeometry.cs#L16) | Provides generation-scoped GPU buffers for an imported geometry helper asset. | — | — |

### `Inno.Rendering.Runtime.RenderGeometrySection`

<!-- api: T:Inno.Rendering.Runtime.RenderGeometrySection 0c1cbd45893f3af81f42edf69c345d7eff537bbaa6fdae9b12c74531362ff221 -->
<!-- api: M:Inno.Rendering.Runtime.RenderGeometrySection.#ctor(System.Int32,System.Int32) c502f9b7116fe2dd113e21e3655918b4a45a770d39e9464b1ce8619a137dfa6c -->
<!-- api: P:Inno.Rendering.Runtime.RenderGeometrySection.firstIndex 389d6871f00080ff2eb69e62bfa44f3ac392ff6b1fb35352077e38b1edd10451 -->
<!-- api: P:Inno.Rendering.Runtime.RenderGeometrySection.indexCount a9ea013b8cd0c9862eabcb2611a9762eb9bee829d124063ee6a7477657d2d8fa -->
<!-- api: M:Inno.Rendering.Runtime.RenderGeometrySection.ToString 760a814885e1708708baf20a525aabd2737e7b971e83954375dcc794c126dcc3 -->
<!-- api: M:Inno.Rendering.Runtime.RenderGeometrySection.op_Inequality(Inno.Rendering.Runtime.RenderGeometrySection,Inno.Rendering.Runtime.RenderGeometrySection) 601a8d2d607aa46aa75b0a3d5f615a63178cb111246e0d82e86128264548a6ab -->
<!-- api: M:Inno.Rendering.Runtime.RenderGeometrySection.op_Equality(Inno.Rendering.Runtime.RenderGeometrySection,Inno.Rendering.Runtime.RenderGeometrySection) 6c92d6ec673cab6848e83ef06d4966fb594f9efc3210c4723cfbb196481d6bfa -->
<!-- api: M:Inno.Rendering.Runtime.RenderGeometrySection.GetHashCode 487c9b41a8d057ae9f6b2d2e63f7ed33a547b1a2bab1572b58f99d7d6698dbfa -->
<!-- api: M:Inno.Rendering.Runtime.RenderGeometrySection.Equals(System.Object) 4d444e5ccabe5d1f0b98bde0e05006dbfc58c8f458031bd59beb131f7042e508 -->
<!-- api: M:Inno.Rendering.Runtime.RenderGeometrySection.Equals(Inno.Rendering.Runtime.RenderGeometrySection) ebabd138a39a4448f71478745836bb18bceb382e7a530bada0235b837a9d4cb4 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-08667da2923f5c25"></a>[`public Inno.Rendering.Runtime.RenderGeometrySection.RenderGeometrySection(int firstIndex, int indexCount)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderGeometrySection.cs#L27) | Creates one indexed geometry range.<br>`firstIndex`：First index in the shared index buffer.<br>`indexCount`：Positive index count. | — | — |
| <a id="api-3f28c243b754a56b"></a>[`public bool Inno.Rendering.Runtime.RenderGeometrySection.Equals(Inno.Rendering.Runtime.RenderGeometrySection other)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderGeometrySection.cs#L16) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-d8427e1aaf893152"></a>[`public override bool Inno.Rendering.Runtime.RenderGeometrySection.Equals(object obj)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderGeometrySection.cs#L16) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-17c1c8f952685033"></a>[`public override int Inno.Rendering.Runtime.RenderGeometrySection.GetHashCode()`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderGeometrySection.cs#L16) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-90f5bd13a5f961ea"></a>[`public static bool Inno.Rendering.Runtime.RenderGeometrySection.operator ==(Inno.Rendering.Runtime.RenderGeometrySection left, Inno.Rendering.Runtime.RenderGeometrySection right)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderGeometrySection.cs#L16) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-420a9bf6fcbc3fa9"></a>[`public static bool Inno.Rendering.Runtime.RenderGeometrySection.operator !=(Inno.Rendering.Runtime.RenderGeometrySection left, Inno.Rendering.Runtime.RenderGeometrySection right)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderGeometrySection.cs#L16) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-5297a70ae2368f97"></a>[`public override string Inno.Rendering.Runtime.RenderGeometrySection.ToString()`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderGeometrySection.cs#L16) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-9865e50ed132744e"></a>[`public int Inno.Rendering.Runtime.RenderGeometrySection.firstIndex { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderGeometrySection.cs#L40) | Gets the first index in the shared index buffer. | — | — |
| <a id="api-21505ed1cfde4d99"></a>[`public int Inno.Rendering.Runtime.RenderGeometrySection.indexCount { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderGeometrySection.cs#L45) | Gets the number of indices in this range. | — | — |
| <a id="api-dd107c8ec961a5cf"></a>[`public readonly record struct Inno.Rendering.Runtime.RenderGeometrySection : System.IEquatable<Inno.Rendering.Runtime.RenderGeometrySection>`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderGeometrySection.cs#L16) | Describes one indexed range in resolved backend-neutral geometry. | — | — |

### `Inno.Rendering.Runtime.RenderMaterialPass`

<!-- api: T:Inno.Rendering.Runtime.RenderMaterialPass 006a40fab3f4be9517460d2e045d951e52389ce9856f7199e869354e2b80e2d7 -->
<!-- api: P:Inno.Rendering.Runtime.RenderMaterialPass.definition 3bf98f878195093ec54b3f767f858d9544c5aab421f7bdcd60a4965f23372f22 -->
<!-- api: P:Inno.Rendering.Runtime.RenderMaterialPass.graphicsPipeline e12e48af73e16ceff29e064ff8fce7521d4282922a83a6fd28b6274d7581bae7 -->
<!-- api: P:Inno.Rendering.Runtime.RenderMaterialPass.computePipeline e5120f0eee779cfd0b2c0d50a290d9a6f7738155a691140bb32dbe51d97b59f3 -->
<!-- api: P:Inno.Rendering.Runtime.RenderMaterialPass.isGraphics 66b1452da6d09dadea1485e4ef3c62a9e053439e9b36aa656027099ee549517f -->
<!-- api: P:Inno.Rendering.Runtime.RenderMaterialPass.isCompute 9ca75bca0f1b89a9f7872a29efa88d6076959ca631c07cbf4fe0b5a8e00b707d -->
<!-- api: M:Inno.Rendering.Runtime.RenderMaterialPass.UsesBinding(Inno.Rendering.RenderBindingId,Inno.Rendering.RenderShaderBindingKind) 63e614d2c815ac4c3cc762979f04f0272ed6d9c0c3d9bccbf4bd88a907468f50 -->
<!-- api: M:Inno.Rendering.Runtime.RenderMaterialPass.Bind(Inno.Rendering.RenderCommandEncoder) 707281679ee6e01173f2563d527c105fbd7e474eda7fd031f0b57ee82192ce80 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3d1efa2ced714ed6"></a>[`public void Inno.Rendering.Runtime.RenderMaterialPass.Bind(Inno.Rendering.RenderCommandEncoder commands)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderMaterialPass.cs#L132) | Binds the program and all material-owned values and textures.<br>`commands`：Current pass command encoder. | — | — |
| <a id="api-6619c34a8e870f54"></a>[`public bool Inno.Rendering.Runtime.RenderMaterialPass.UsesBinding(Inno.Rendering.RenderBindingId binding, Inno.Rendering.RenderShaderBindingKind kind)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderMaterialPass.cs#L109) | Gets whether the compiled program actively consumes one declared binding.<br>返回：true when the final compiled pass reflects the binding; otherwise, false when the compiler proved the declared binding unused and removed it.<br>`binding`：Stable binding declared by the shader contract.<br>`kind`：Expected command-binding domain. | — | `System.ArgumentException`：The binding is not declared by the shader, or its declared kind differs from kind. |
| <a id="api-d32d2e12de6915a5"></a>[`public Inno.Rendering.ComputePipelineHandle Inno.Rendering.Runtime.RenderMaterialPass.computePipeline { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderMaterialPass.cs#L81) | Gets the compute pipeline, or an invalid handle for a raster pass. | — | — |
| <a id="api-2d24b97f8b7db04b"></a>[`public Inno.Rendering.Assets.ShaderPassDefinition Inno.Rendering.Runtime.RenderMaterialPass.definition { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderMaterialPass.cs#L71) | Gets the selected provider-defined shader pass with detached metadata storage. | — | — |
| <a id="api-d205fac18fa1744f"></a>[`public Inno.Rendering.GraphicsPipelineHandle Inno.Rendering.Runtime.RenderMaterialPass.graphicsPipeline { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderMaterialPass.cs#L76) | Gets the graphics pipeline, or an invalid handle for a compute pass. | — | — |
| <a id="api-7ef17b7946d6606c"></a>[`public bool Inno.Rendering.Runtime.RenderMaterialPass.isCompute { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderMaterialPass.cs#L91) | Gets whether this is a compute material pass. | — | — |
| <a id="api-8cb78a8517bb5bc4"></a>[`public bool Inno.Rendering.Runtime.RenderMaterialPass.isGraphics { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderMaterialPass.cs#L86) | Gets whether this is a graphics material pass. | — | — |
| <a id="api-2901e18d9236794a"></a>[`public sealed class Inno.Rendering.Runtime.RenderMaterialPass`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderMaterialPass.cs#L36) | Represents one frame-resolved material pass and its material-owned bindings. | The instance is scoped to the active render device generation. Pipeline-owned graph textures and buffers remain explicit and are bound by the caller after this material state is applied. | — |

### `Inno.Rendering.Runtime.RenderModelExtensionAttribute`

<!-- api: T:Inno.Rendering.Runtime.RenderModelExtensionAttribute 4c2b871d0d6bb7f6ab975bf27f97e9decee03f3004bc349692e9c3bd69c10955 -->
<!-- api: M:Inno.Rendering.Runtime.RenderModelExtensionAttribute.#ctor(System.String) b102dd45452ff40b76d2c59cf63288e0ba1c0df57e11a2d4a8a4ed0c68fe218e -->
<!-- api: P:Inno.Rendering.Runtime.RenderModelExtensionAttribute.id 7b9e6f17d01aaf0d838a17921cb27b5d2dcacfb5c141cf03eff660c56389aa06 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9091fe3e02f8c8d2"></a>[`public Inno.Rendering.Runtime.RenderModelExtensionAttribute.RenderModelExtensionAttribute(string id)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderModelExtensionAttribute.cs#L24) | Creates a model declaration.<br>`id`：Globally stable model identity. | — | — |
| <a id="api-a9085bab0b017183"></a>[`public string Inno.Rendering.Runtime.RenderModelExtensionAttribute.id { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderModelExtensionAttribute.cs#L33) | Gets the globally stable model identity. | — | — |
| <a id="api-2229f458e2666bad"></a>[`public sealed class Inno.Rendering.Runtime.RenderModelExtensionAttribute : System.Attribute`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderModelExtensionAttribute.cs#L16) | Marks a reloadable rendering model with a stable identity. | — | — |

### `Inno.Rendering.Runtime.RenderModelOutput`

<!-- api: T:Inno.Rendering.Runtime.RenderModelOutput e7524c9b5276ec954a88ca6bf46302d06ed156187eec8520a3beac1075982528 -->
<!-- api: M:Inno.Rendering.Runtime.RenderModelOutput.#ctor(System.String,Inno.Rendering.Assets.RenderPipelineAsset,Inno.Rendering.Runtime.RenderFrameData,Inno.Rendering.RenderTextureFormat) de80305b38a653c56ea8c721db9f07c4321355e6f1df6f807379394ad66975a0 -->
<!-- api: P:Inno.Rendering.Runtime.RenderModelOutput.name 859deddb802e83f0676255452b9e087fa471820c93d5a7aff8e7ef68294bfef2 -->
<!-- api: P:Inno.Rendering.Runtime.RenderModelOutput.pipeline da9287d4c58944acea6d5fbbf449cb5c131256274040b6cb658e315f3172b35d -->
<!-- api: P:Inno.Rendering.Runtime.RenderModelOutput.data 8227c359ee0345c64d45e72fdf0da6fa18709a57a1d28aad08eaed6f349e50e6 -->
<!-- api: P:Inno.Rendering.Runtime.RenderModelOutput.targetFormat 24e94c9bba4f6e074644a5dda22aae40f3e3aee3a48ade72251f32929f26c35d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-6e4bb7287baf97d3"></a>[`public Inno.Rendering.Runtime.RenderModelOutput.RenderModelOutput(string name, Inno.Rendering.Assets.RenderPipelineAsset pipeline, Inno.Rendering.Runtime.RenderFrameData data, Inno.Rendering.RenderTextureFormat targetFormat = Inno.Rendering.RenderTextureFormat.RGBA8Srgb)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderModelOutput.cs#L32) | Creates prepared model output.<br>`name`：Frame-local diagnostic name.<br>`pipeline`：Exact model pipeline.<br>`data`：Immutable model frame data.<br>`targetFormat`：Required color target format. | — | — |
| <a id="api-cff537101f08926d"></a>[`public Inno.Rendering.Runtime.RenderFrameData Inno.Rendering.Runtime.RenderModelOutput.data { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderModelOutput.cs#L56) | Gets model frame data. | — | — |
| <a id="api-82a5c3678a4f2e1b"></a>[`public string Inno.Rendering.Runtime.RenderModelOutput.name { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderModelOutput.cs#L48) | Gets the diagnostic name. | — | — |
| <a id="api-a14d0745da81a660"></a>[`public Inno.Rendering.Assets.RenderPipelineAsset Inno.Rendering.Runtime.RenderModelOutput.pipeline { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderModelOutput.cs#L52) | Gets the model's pipeline. | — | — |
| <a id="api-aafb30ea379cdc44"></a>[`public Inno.Rendering.RenderTextureFormat Inno.Rendering.Runtime.RenderModelOutput.targetFormat { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderModelOutput.cs#L60) | Gets required target format. | — | — |
| <a id="api-ad95a30e6bdacf91"></a>[`public sealed class Inno.Rendering.Runtime.RenderModelOutput`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderModelOutput.cs#L15) | Gets one model's prepared pipeline data for a host-owned target. | — | — |

### `Inno.Rendering.Runtime.RenderOutputInput`

<!-- api: T:Inno.Rendering.Runtime.RenderOutputInput ed9f5717530c762270c04fd08e171e0db75509ce5ecc660246d8d3174b226937 -->
<!-- api: M:Inno.Rendering.Runtime.RenderOutputInput.#ctor(Inno.Core.Mathematics.Vector2,System.Boolean,Inno.Core.Mathematics.Vector2,Inno.Core.Input.KeyModifier,System.Collections.Generic.IReadOnlyCollection{Inno.Core.Input.KeyCode},System.Collections.Generic.IReadOnlyCollection{Inno.Core.Input.KeyCode},System.Collections.Generic.IReadOnlyCollection{Inno.Core.Input.MouseButton},System.Collections.Generic.IReadOnlyCollection{Inno.Core.Input.MouseButton},System.Collections.Generic.IReadOnlyList{System.String},System.Boolean) 071cc42277537df426b11d3fbe6f95d7de1139b7de26077afa4089acf9a21b6d -->
<!-- api: P:Inno.Rendering.Runtime.RenderOutputInput.empty c80305851253fc2def91e3257fc9d930f7d237c34ca2e8964807212958ec83b5 -->
<!-- api: P:Inno.Rendering.Runtime.RenderOutputInput.suspended 58154cfaab69adfce1e9ef77866ba309ea5f6bb0f25c0424232898758ba41540 -->
<!-- api: P:Inno.Rendering.Runtime.RenderOutputInput.pointerPosition fe4310e3451b91a50b42d0700cce34746ced8d73857000ab2ac6a7809a79edeb -->
<!-- api: P:Inno.Rendering.Runtime.RenderOutputInput.pointerInside 4300b77cdca5abf64123ff1a48067c1e2d658cc765a4ccdc7f0b629f5b8502a6 -->
<!-- api: P:Inno.Rendering.Runtime.RenderOutputInput.scrollDelta 171c7e3c4645c94f7bac4816f48e7d8da35062cc4759ad538c8893e9dc600759 -->
<!-- api: P:Inno.Rendering.Runtime.RenderOutputInput.modifiers 114bf36974e382040500a2353377f5335ca777d2d7d8b67e71efa389b4b8784b -->
<!-- api: P:Inno.Rendering.Runtime.RenderOutputInput.keysPressed 723a4803c0a51c1d59506164a2277ce758da05f152d96f789d5aca847d7c6458 -->
<!-- api: P:Inno.Rendering.Runtime.RenderOutputInput.keysReleased 066ee87233ea8edbb793c9ae6524c01804d94d4056fd45fe644e1e5a57534fa9 -->
<!-- api: P:Inno.Rendering.Runtime.RenderOutputInput.buttonsPressed c573a699f4b859f07c6d83cd9dedab3aef7b65faf9f2a9f8dbc6e8d5945be0b7 -->
<!-- api: P:Inno.Rendering.Runtime.RenderOutputInput.buttonsReleased 58abc8d2f26010e9d243326305f6009408b3f5e0cae2595cc00057500052e676 -->
<!-- api: P:Inno.Rendering.Runtime.RenderOutputInput.textInput 754b0db91fe11e72da7c9a26faacdfebb02e2d5b56ceb49909b28a1aeef58495 -->
<!-- api: P:Inno.Rendering.Runtime.RenderOutputInput.interactionEnabled 606ecc61c95131d7692a9b7202ac4e1171c6585ea8fc57ff2068d575fdea9d76 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-dfea55919233be50"></a>[`public Inno.Rendering.Runtime.RenderOutputInput.RenderOutputInput(Inno.Core.Mathematics.Vector2 pointerPosition, bool pointerInside, Inno.Core.Mathematics.Vector2 scrollDelta, Inno.Core.Input.KeyModifier modifiers, System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.KeyCode> keysPressed, System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.KeyCode> keysReleased, System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.MouseButton> buttonsPressed, System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.MouseButton> buttonsReleased, System.Collections.Generic.IReadOnlyList<string> textInput, bool interactionEnabled = true)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputInput.cs#L48) | Creates an input snapshot for one output viewport.<br>`pointerPosition`：Pointer coordinates relative to the viewport's top-left corner.<br>`pointerInside`：Whether the pointer is within this output.<br>`scrollDelta`：Pointer wheel movement.<br>`modifiers`：Current keyboard modifiers.<br>`keysPressed`：Keys pressed this frame.<br>`keysReleased`：Keys released this frame.<br>`buttonsPressed`：Pointer buttons pressed this frame.<br>`buttonsReleased`：Pointer buttons released this frame.<br>`textInput`：Ordered Unicode text commits.<br>`interactionEnabled`：Whether the output may dispatch game interaction. | — | — |
| <a id="api-e64fbd8701a335ad"></a>[`public System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.MouseButton> Inno.Rendering.Runtime.RenderOutputInput.buttonsPressed { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputInput.cs#L111) | Gets buttons pressed this frame. | — | — |
| <a id="api-f678dd6702876547"></a>[`public System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.MouseButton> Inno.Rendering.Runtime.RenderOutputInput.buttonsReleased { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputInput.cs#L115) | Gets buttons released this frame. | — | — |
| <a id="api-cebdff4932252186"></a>[`public static Inno.Rendering.Runtime.RenderOutputInput Inno.Rendering.Runtime.RenderOutputInput.empty { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputInput.cs#L75) | Gets a snapshot with no active input. | — | — |
| <a id="api-7e5d3b0d6b51616a"></a>[`public bool Inno.Rendering.Runtime.RenderOutputInput.interactionEnabled { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputInput.cs#L124) | Gets whether game pointer and keyboard interaction is enabled. | — | — |
| <a id="api-62b87e17160461cf"></a>[`public System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.KeyCode> Inno.Rendering.Runtime.RenderOutputInput.keysPressed { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputInput.cs#L103) | Gets keys pressed this frame. | — | — |
| <a id="api-2a51578cf9bce11e"></a>[`public System.Collections.Generic.IReadOnlyCollection<Inno.Core.Input.KeyCode> Inno.Rendering.Runtime.RenderOutputInput.keysReleased { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputInput.cs#L107) | Gets keys released this frame. | — | — |
| <a id="api-25c39bfe800a1b04"></a>[`public Inno.Core.Input.KeyModifier Inno.Rendering.Runtime.RenderOutputInput.modifiers { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputInput.cs#L99) | Gets active keyboard modifiers. | — | — |
| <a id="api-06c02e96864956d8"></a>[`public bool Inno.Rendering.Runtime.RenderOutputInput.pointerInside { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputInput.cs#L91) | Gets whether the pointer is inside this output. | — | — |
| <a id="api-708ffdef5fc098a7"></a>[`public Inno.Core.Mathematics.Vector2 Inno.Rendering.Runtime.RenderOutputInput.pointerPosition { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputInput.cs#L87) | Gets viewport-local pointer coordinates. | — | — |
| <a id="api-f16862041c9b629d"></a>[`public Inno.Core.Mathematics.Vector2 Inno.Rendering.Runtime.RenderOutputInput.scrollDelta { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputInput.cs#L95) | Gets wheel movement. | — | — |
| <a id="api-b0d85cf398fa8be8"></a>[`public static Inno.Rendering.Runtime.RenderOutputInput Inno.Rendering.Runtime.RenderOutputInput.suspended { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputInput.cs#L81) | Gets a snapshot that clears gameplay interaction while retaining the rendered output. | — | — |
| <a id="api-367aba706777155b"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Rendering.Runtime.RenderOutputInput.textInput { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputInput.cs#L119) | Gets ordered text commits. | — | — |
| <a id="api-1ad5c8435553595a"></a>[`public sealed class Inno.Rendering.Runtime.RenderOutputInput`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputInput.cs#L13) | Frame-local input located in the output viewport's physical pixels. | — | — |

### `Inno.Rendering.Runtime.RenderOutputLayer`

<!-- api: T:Inno.Rendering.Runtime.RenderOutputLayer e4a30caa5c541486cd61bfd09d7cbe06d6bc57eb51f4cb1675747209881da91c -->
<!-- api: M:Inno.Rendering.Runtime.RenderOutputLayer.#ctor(System.String,System.Collections.Generic.IEnumerable{System.String}) 0c962b8e46f5b7467218b8fea61f9c5d798a13823d8710964a83e6cb0a2621a8 -->
<!-- api: P:Inno.Rendering.Runtime.RenderOutputLayer.modelId cac50b0b25fe92ab9d178fb8be78983e149a219f918471323b61fe81ed32d2e2 -->
<!-- api: P:Inno.Rendering.Runtime.RenderOutputLayer.sourceIds f90404689a74eb6183703dbe2d6ba5ee84053663a30bb9813106ba3bdff3ca4a -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-aa6c8429c40397ab"></a>[`public Inno.Rendering.Runtime.RenderOutputLayer.RenderOutputLayer(string modelId, System.Collections.Generic.IEnumerable<string> sourceIds)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputLayer.cs#L26) | Creates a model layer with explicit, distinct content-source IDs.<br>`modelId`：Stable render-model or Editor contributor ID.<br>`sourceIds`：World-content sources exclusively owned by this layer. | — | — |
| <a id="api-86972b19e7f16778"></a>[`public string Inno.Rendering.Runtime.RenderOutputLayer.modelId { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputLayer.cs#L43) | Gets the exact rendering model ID. | — | — |
| <a id="api-ec6368c47b5735a5"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Rendering.Runtime.RenderOutputLayer.sourceIds { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputLayer.cs#L47) | Gets the assigned world-content source IDs. | — | — |
| <a id="api-235e743ed94ca600"></a>[`public sealed class Inno.Rendering.Runtime.RenderOutputLayer`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputLayer.cs#L15) | Assigns exactly one rendering model and its world-content sources to an output layer. | — | — |

### `Inno.Rendering.Runtime.RenderOutputRoute`

<!-- api: T:Inno.Rendering.Runtime.RenderOutputRoute b7f6c344ea1d05b96d4a3a56e04fada190ccb51bb665c55a88a16150740b1eb6 -->
<!-- api: M:Inno.Rendering.Runtime.RenderOutputRoute.#ctor(System.Collections.Generic.IEnumerable{Inno.Rendering.Runtime.RenderOutputLayer}) 41d625077e4607e1b4cf22351b6ebc24cf80aee248a4a8eab153aec81fdf173e -->
<!-- api: P:Inno.Rendering.Runtime.RenderOutputRoute.layers 8968c4f94732083660a4a9e776f63934f91bb1b44c029fa63c9c031aba571cba -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-011b4f7bd60821c7"></a>[`public Inno.Rendering.Runtime.RenderOutputRoute.RenderOutputRoute(System.Collections.Generic.IEnumerable<Inno.Rendering.Runtime.RenderOutputLayer> layers)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputRoute.cs#L23) | Creates a route from explicitly assigned model layers in draw order.<br>`layers`：Exact model identities and exclusive source assignments in draw order. | — | — |
| <a id="api-df3c72fda518891d"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Runtime.RenderOutputLayer> Inno.Rendering.Runtime.RenderOutputRoute.layers { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputRoute.cs#L39) | Gets exact model and source assignments in draw order. | — | — |
| <a id="api-83fc599e68a6b64a"></a>[`public sealed class Inno.Rendering.Runtime.RenderOutputRoute`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputRoute.cs#L15) | States the exact model order and exclusive world-content assignment for one output. | — | — |

### `Inno.Rendering.Runtime.RenderOutputSession`

<!-- api: T:Inno.Rendering.Runtime.RenderOutputSession ea62a17d2f73778d3dc93e085a8883bcba6fd593f435a34f0b80c457937854c3 -->
<!-- api: M:Inno.Rendering.Runtime.RenderOutputSession.#ctor(System.String,Inno.References.ContentReadScope,Inno.Rendering.RenderViewport,System.UInt64,System.Single,Inno.Rendering.Runtime.IViewContentCollector,Inno.Rendering.Runtime.RenderOutputInput,Inno.Rendering.Runtime.RenderOutputRoute) 98325b1bf2f755c9d69b1811ffaf933da4ea17f55ba5d432b1b3ef4f60b42db0 -->
<!-- api: P:Inno.Rendering.Runtime.RenderOutputSession.id 042c358979c09b4a073847ea70f3ca8d9f1836aba5e29c19968a86a500a25bf4 -->
<!-- api: P:Inno.Rendering.Runtime.RenderOutputSession.content 005be421f8ac3f71f9a362420bb95b640c3aa9ee5ae45db59b6fc3a9bc4210e7 -->
<!-- api: P:Inno.Rendering.Runtime.RenderOutputSession.viewport 95d0a4fc6c77fac4ccef9bfa832b57a9fa57e18c5d7e5173aca3f6a6feb8705f -->
<!-- api: P:Inno.Rendering.Runtime.RenderOutputSession.frameIndex ca85ec36613b9327cac11624ae3d06220bf54f33c8cd926334a0f9911b4689f0 -->
<!-- api: P:Inno.Rendering.Runtime.RenderOutputSession.deltaTime 77f6073b6c73290c47b40bca0af1b5cc5bca0d998ac00b30360c63414aeb9b97 -->
<!-- api: P:Inno.Rendering.Runtime.RenderOutputSession.viewContent e7c8a05bbc751802b5e458b3142a332af5be152e2554a0d93f8416437fa6266b -->
<!-- api: P:Inno.Rendering.Runtime.RenderOutputSession.input 78cbb71bc5a8c6f580c92acd70dc9b48edee605d26f3da551242166592437426 -->
<!-- api: P:Inno.Rendering.Runtime.RenderOutputSession.route bf99d44d7ce30649d614bff9205baf60faf2cdd5799dbfa0edd1cb8545f31148 -->
<!-- api: M:Inno.Rendering.Runtime.RenderOutputSession.ForLayer(Inno.Rendering.Runtime.RenderOutputLayer) 41cfb47e1bf8421ac0e5aefe434a929a581de6f2c8317a426a1c26e54ffd278d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e9cf890e36c83672"></a>[`public Inno.Rendering.Runtime.RenderOutputSession.RenderOutputSession(string id, Inno.References.ContentReadScope content, Inno.Rendering.RenderViewport viewport, ulong frameIndex, float deltaTime, Inno.Rendering.Runtime.IViewContentCollector viewContent, Inno.Rendering.Runtime.RenderOutputInput? input = null, Inno.Rendering.Runtime.RenderOutputRoute? route = null)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputSession.cs#L44) | Creates a frame-scoped output session.<br>`id`：Stable host output identity.<br>`content`：Host-selected content roots.<br>`viewport`：Pixel rectangle within the target.<br>`frameIndex`：Shared output frame index.<br>`deltaTime`：Elapsed frame time in seconds.<br>`viewContent`：Model-independent world content collector.<br>`input`：Viewport-local input.<br>`route`：Explicit composition route when several models are applicable. | — | — |
| <a id="api-258e8792b14d2fe7"></a>[`public Inno.Rendering.Runtime.RenderOutputSession Inno.Rendering.Runtime.RenderOutputSession.ForLayer(Inno.Rendering.Runtime.RenderOutputLayer layer)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputSession.cs#L107) | Creates the session seen by one explicitly assigned model layer.<br>返回：A session with only the assigned world-content sources.<br>`layer`：The model and content-source assignment. | — | — |
| <a id="api-16c44bf84b8f80da"></a>[`public Inno.References.ContentReadScope Inno.Rendering.Runtime.RenderOutputSession.content { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputSession.cs#L72) | Gets selected content roots. | — | — |
| <a id="api-b29e641f00b2f1ea"></a>[`public float Inno.Rendering.Runtime.RenderOutputSession.deltaTime { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputSession.cs#L84) | Gets elapsed frame time. | — | — |
| <a id="api-08c7aa0df76a5baf"></a>[`public ulong Inno.Rendering.Runtime.RenderOutputSession.frameIndex { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputSession.cs#L80) | Gets the shared output frame index. | — | — |
| <a id="api-913ea916cc590e04"></a>[`public string Inno.Rendering.Runtime.RenderOutputSession.id { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputSession.cs#L68) | Gets the stable host output identity. | — | — |
| <a id="api-2aa4e755d31bb7df"></a>[`public Inno.Rendering.Runtime.RenderOutputInput Inno.Rendering.Runtime.RenderOutputSession.input { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputSession.cs#L92) | Gets viewport-local input. | — | — |
| <a id="api-287db8048f0f447c"></a>[`public Inno.Rendering.Runtime.RenderOutputRoute? Inno.Rendering.Runtime.RenderOutputSession.route { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputSession.cs#L96) | Gets the explicit model route, if configured. | — | — |
| <a id="api-bd8dc7101b7af7e7"></a>[`public Inno.Rendering.Runtime.IViewContentCollector Inno.Rendering.Runtime.RenderOutputSession.viewContent { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputSession.cs#L88) | Gets the world content collector. | — | — |
| <a id="api-52f11d087827386c"></a>[`public Inno.Rendering.RenderViewport Inno.Rendering.Runtime.RenderOutputSession.viewport { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputSession.cs#L76) | Gets the output pixel viewport. | — | — |
| <a id="api-35d0cd0b8c744ffd"></a>[`public sealed class Inno.Rendering.Runtime.RenderOutputSession`](../../src/services/rendering/Inno.Rendering.Runtime/Models/RenderOutputSession.cs#L15) | Declares a host output without naming a camera or scene model. | — | — |

### `Inno.Rendering.Runtime.RenderPersistentResourceId`

<!-- api: T:Inno.Rendering.Runtime.RenderPersistentResourceId 98e2a67f64b8b4834db0e1418395bcc8a27a690fdfddd38131aaa531f29cb84e -->
<!-- api: M:Inno.Rendering.Runtime.RenderPersistentResourceId.#ctor(System.String) 5823736104958384bed7424cfbdcedfec1b8a3d9ca0a2269753b10213d127341 -->
<!-- api: P:Inno.Rendering.Runtime.RenderPersistentResourceId.value e9c990191267f53733a2a79c2ab84a7ef3f821be5b437a4399b272d3a1fcf43c -->
<!-- api: P:Inno.Rendering.Runtime.RenderPersistentResourceId.isValid 351fa87ee8999e8779b570d6e7d743f55f3509331ba69f0414812e603ad64a7f -->
<!-- api: M:Inno.Rendering.Runtime.RenderPersistentResourceId.ToString d9d050fd09580e9e7921495bb4a1f15305e89cbf525c1071a2aadfa34cfea66c -->
<!-- api: M:Inno.Rendering.Runtime.RenderPersistentResourceId.op_Inequality(Inno.Rendering.Runtime.RenderPersistentResourceId,Inno.Rendering.Runtime.RenderPersistentResourceId) c648a345ed6cec8c0825691aaf70f8b44f010c64f43e2e1551e0f2152a3a085a -->
<!-- api: M:Inno.Rendering.Runtime.RenderPersistentResourceId.op_Equality(Inno.Rendering.Runtime.RenderPersistentResourceId,Inno.Rendering.Runtime.RenderPersistentResourceId) b315bc732fa32ff0499030fc68ea138c6260d257a36fcffc2652a024df42c246 -->
<!-- api: M:Inno.Rendering.Runtime.RenderPersistentResourceId.GetHashCode 2a020fac2e53e05a1f26d71cd7b18aabd801b4f7317226e8e485c30d8352bfad -->
<!-- api: M:Inno.Rendering.Runtime.RenderPersistentResourceId.Equals(System.Object) 18a3f1edaba584205722e2e7e87ec362b39de4ba31890937a64f5e6a49691717 -->
<!-- api: M:Inno.Rendering.Runtime.RenderPersistentResourceId.Equals(Inno.Rendering.Runtime.RenderPersistentResourceId) 16aab8b51081ee46792340e446350c40b9cc53704380fe34a200c22b9086cf33 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f42f0c468828486c"></a>[`public Inno.Rendering.Runtime.RenderPersistentResourceId.RenderPersistentResourceId(string value)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderPersistentResourceId.cs#L24) | Creates a globally stable persistent resource identifier.<br>`value`：Provider-qualified stable identifier. | — | — |
| <a id="api-c14877fcf81304ad"></a>[`public bool Inno.Rendering.Runtime.RenderPersistentResourceId.Equals(Inno.Rendering.Runtime.RenderPersistentResourceId other)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderPersistentResourceId.cs#L16) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-dd96737948b0bf5a"></a>[`public override bool Inno.Rendering.Runtime.RenderPersistentResourceId.Equals(object obj)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderPersistentResourceId.cs#L16) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-e267fb1af40a1316"></a>[`public override int Inno.Rendering.Runtime.RenderPersistentResourceId.GetHashCode()`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderPersistentResourceId.cs#L16) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-2a60720d3b2c485c"></a>[`public static bool Inno.Rendering.Runtime.RenderPersistentResourceId.operator ==(Inno.Rendering.Runtime.RenderPersistentResourceId left, Inno.Rendering.Runtime.RenderPersistentResourceId right)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderPersistentResourceId.cs#L16) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-9b55eed424c9c5ba"></a>[`public static bool Inno.Rendering.Runtime.RenderPersistentResourceId.operator !=(Inno.Rendering.Runtime.RenderPersistentResourceId left, Inno.Rendering.Runtime.RenderPersistentResourceId right)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderPersistentResourceId.cs#L16) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-f8010f2ef111cad2"></a>[`public override string Inno.Rendering.Runtime.RenderPersistentResourceId.ToString()`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderPersistentResourceId.cs#L46) | Formats this value as a human-readable representation.<br>返回：The human-readable representation of this value. | — | — |
| <a id="api-30cdd3940621b160"></a>[`public bool Inno.Rendering.Runtime.RenderPersistentResourceId.isValid { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderPersistentResourceId.cs#L38) | Gets whether the identifier contains a usable value. | — | — |
| <a id="api-3d8e92835234f6dd"></a>[`public string Inno.Rendering.Runtime.RenderPersistentResourceId.value { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderPersistentResourceId.cs#L33) | Gets the provider-qualified stable identifier. | — | — |
| <a id="api-ddae5a9cb65fa10b"></a>[`public readonly record struct Inno.Rendering.Runtime.RenderPersistentResourceId : System.IEquatable<Inno.Rendering.Runtime.RenderPersistentResourceId>`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderPersistentResourceId.cs#L16) | Identifies one provider-owned persistent GPU resource without exposing a native handle. | — | — |

### `Inno.Rendering.Runtime.RenderPipeline`

<!-- api: T:Inno.Rendering.Runtime.RenderPipeline 92cfc7006e285171a1eb6af65b600eae761f764db3bdef421f0995bdd8dc6911 -->
<!-- api: M:Inno.Rendering.Runtime.RenderPipeline.Configure(Inno.Rendering.Assets.SerializedRenderExtensionState,Inno.Rendering.Runtime.RenderExtensionStateContext) 48344eb4166a5625832c37d2963dae2e43bb699783b5990606d6785d28de67f1 -->
<!-- api: M:Inno.Rendering.Runtime.RenderPipeline.Build(Inno.Rendering.Runtime.RenderPipelineContext) 9130d3a2f1a2c1a6498a4fb3f0f9935dbcccb6d907045e9dd6fa0e6bd3df6590 -->
<!-- api: M:Inno.Rendering.Runtime.RenderPipeline.Dispose 951e1a4927f7351f3f7819363d7f79ff6e08a8dcf54f8a022e1bdab7b0463b0a -->
<!-- api: M:Inno.Rendering.Runtime.RenderPipeline.OnConfigure(Inno.Rendering.Assets.SerializedRenderExtensionState,Inno.Rendering.Runtime.RenderExtensionStateContext) f88d4a19d7f4ba95774ab40ee6dd9f59c3f6133e134ca51cdc9230d5ac5941c4 -->
<!-- api: M:Inno.Rendering.Runtime.RenderPipeline.Dispose(System.Boolean) 89a5d30c466f7caf30939ebb0393a4af2d6bbfb808080b8fdbdf453380102e74 -->
<!-- api: M:Inno.Rendering.Runtime.RenderPipeline.#ctor da54562ef09a87a43c3f5973e2df8552f2653b0daf6e08f38f8a9f78883d3942 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-bf4a5ad7c6b4f336"></a>[`protected Inno.Rendering.Runtime.RenderPipeline.RenderPipeline()`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipeline.cs#L15) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-1b584b8fb582c082"></a>[`public abstract void Inno.Rendering.Runtime.RenderPipeline.Build(Inno.Rendering.Runtime.RenderPipelineContext context)`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipeline.cs#L42) | Builds all passes for one request.<br>`context`：Frame-scoped pipeline context. | — | — |
| <a id="api-ac001f756b04119c"></a>[`public void Inno.Rendering.Runtime.RenderPipeline.Configure(Inno.Rendering.Assets.SerializedRenderExtensionState state, Inno.Rendering.Runtime.RenderExtensionStateContext settings)`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipeline.cs#L28) | Applies reload-safe pipeline settings to this generation.<br>`state`：Stable type identity and neutral property bytes.<br>`settings`：Native context bound to the canonical Pipeline asset's actual owner. | — | — |
| <a id="api-ecdfbb5eed64387b"></a>[`public void Inno.Rendering.Runtime.RenderPipeline.Dispose()`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipeline.cs#L50) | Releases generation-scoped pipeline state. | — | `Inno.Core.Execution.RetirementPendingException`：Pipeline work is still active. The owner must retain this instance and retry before releasing dependencies. |
| <a id="api-169a5a43487bd6b5"></a>[`protected virtual void Inno.Rendering.Runtime.RenderPipeline.Dispose(bool disposing)`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipeline.cs#L94) | Releases managed generation-scoped state.<br>`disposing`：Always true for explicit disposal. | — | `Inno.Core.Execution.RetirementPendingException`：Retirement cannot complete yet; subsequent calls resume this hook with its remaining owned resources. |
| <a id="api-f7c5b7c3e9369fea"></a>[`protected virtual void Inno.Rendering.Runtime.RenderPipeline.OnConfigure(Inno.Rendering.Assets.SerializedRenderExtensionState state, Inno.Rendering.Runtime.RenderExtensionStateContext settings)`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipeline.cs#L80) | Reads pipeline-owned settings from neutral state.<br>`state`：Reload-safe extension state.<br>`settings`：Native context bound to the canonical Pipeline asset's actual owner. | — | — |
| <a id="api-fdb8892602472dbf"></a>[`public abstract class Inno.Rendering.Runtime.RenderPipeline : System.IDisposable`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipeline.cs#L15) | Builds frame-local passes without prescribing a rendering model. | — | — |

### `Inno.Rendering.Runtime.RenderPipelineContext`

<!-- api: T:Inno.Rendering.Runtime.RenderPipelineContext a08fe1b55fdba61c65a8e62c19d2b312315ac35a521bf7363095249da8d28246 -->
<!-- api: M:Inno.Rendering.Runtime.RenderPipelineContext.#ctor(Inno.Rendering.Runtime.RenderRequest,Inno.Rendering.Assets.RenderPipelineAsset,Inno.Rendering.RenderGraphBuilder,Inno.Rendering.GraphicsCapabilities,Inno.Rendering.Runtime.RenderResourceMap,Inno.Core.Diagnostics.IDiagnosticReporter,Inno.Rendering.Runtime.IRenderResourceService,Inno.Rendering.IRenderFrameUploadService,System.UInt64,System.Boolean,Inno.Rendering.RenderTextureHandle) 1e62ff9665f5179e9e20036b421c9fae54b0c2711c9ca0c3a42ad7f9884ee929 -->
<!-- api: P:Inno.Rendering.Runtime.RenderPipelineContext.request a78cdd71ed950f3df3aef5df780781a85760e8796aed08338f361255f9b4e08f -->
<!-- api: P:Inno.Rendering.Runtime.RenderPipelineContext.pipelineAsset 3a691be6cacb33b33dec0f16c7a511473f7c55fa56fac34c793b74b9a0140a93 -->
<!-- api: P:Inno.Rendering.Runtime.RenderPipelineContext.graph 20c76576249300aa7ff3006f52993023111308c251bf48e2cbce49c16c1a4269 -->
<!-- api: P:Inno.Rendering.Runtime.RenderPipelineContext.capabilities 0281f39b58e2c7837ae80d19596ad86a51e39c13081a75baa9ba579b79ee8980 -->
<!-- api: P:Inno.Rendering.Runtime.RenderPipelineContext.resources 14a09c12ad1807d82140a8d1a9baf7ee78fcf5bc1818a23edbed9691f89275f5 -->
<!-- api: P:Inno.Rendering.Runtime.RenderPipelineContext.diagnostics 0e1d47608980d6c971d9bab98869201284c142fe3f078949ff93c374f7470313 -->
<!-- api: P:Inno.Rendering.Runtime.RenderPipelineContext.resourceService 51fc0cf845dddc738e800204bdc52fc24f5821492fd533093656912f66a6c76d -->
<!-- api: P:Inno.Rendering.Runtime.RenderPipelineContext.uploads af9956793bccb7cf39e35f55b47146aa4caa8f50da08f3e0b8ec97a09d82faf2 -->
<!-- api: P:Inno.Rendering.Runtime.RenderPipelineContext.frameIndex b284669a4894224123978b84c556a91e166fff586557fc92a1492653abe2d901 -->
<!-- api: P:Inno.Rendering.Runtime.RenderPipelineContext.preservePresentationTarget bec18458deffea6a361f38940f5a92719d1010c127fa2b4caeffb30b7dbfe4fe -->
<!-- api: P:Inno.Rendering.Runtime.RenderPipelineContext.outputTexture d1a2bb6af43ac720006c818b496cc45f80a28a8b073ac27d1e9bb9514a0d7403 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-99384ef805f45270"></a>[`public Inno.Rendering.Runtime.RenderPipelineContext.RenderPipelineContext(Inno.Rendering.Runtime.RenderRequest request, Inno.Rendering.Assets.RenderPipelineAsset pipelineAsset, Inno.Rendering.RenderGraphBuilder graph, Inno.Rendering.GraphicsCapabilities capabilities, Inno.Rendering.Runtime.RenderResourceMap resources, Inno.Core.Diagnostics.IDiagnosticReporter diagnostics, Inno.Rendering.Runtime.IRenderResourceService resourceService, Inno.Rendering.IRenderFrameUploadService uploads, ulong frameIndex, bool preservePresentationTarget, Inno.Rendering.RenderTextureHandle outputTexture = default(Inno.Rendering.RenderTextureHandle))`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineContext.cs#L53) | Creates a pipeline build context.<br>`request`：Pipeline-defined render request.<br>`pipelineAsset`：Selected persistent pipeline configuration.<br>`graph`：Current frame graph builder.<br>`capabilities`：Current device capability snapshot.<br>`resources`：Open semantic resource registry.<br>`diagnostics`：Structured diagnostic sink.<br>`resourceService`：Generation-aware shader, material and persistent GPU resource service.<br>`uploads`：Frame-scoped streaming buffer service.<br>`frameIndex`：Monotonic render frame index.<br>`preservePresentationTarget`：Whether an earlier successful model contribution already initialized the same presentation target.<br>`outputTexture`：Imported offscreen target, or an invalid handle for the backbuffer. | — | — |
| <a id="api-0e006bb7b8550742"></a>[`public Inno.Rendering.GraphicsCapabilities Inno.Rendering.Runtime.RenderPipelineContext.capabilities { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineContext.cs#L105) | Gets current device capabilities. | — | — |
| <a id="api-7ae2c07557b9db21"></a>[`public Inno.Core.Diagnostics.IDiagnosticReporter Inno.Rendering.Runtime.RenderPipelineContext.diagnostics { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineContext.cs#L115) | Gets the structured diagnostic sink. | — | — |
| <a id="api-23e910bc64af267a"></a>[`public ulong Inno.Rendering.Runtime.RenderPipelineContext.frameIndex { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineContext.cs#L130) | Gets the monotonic render frame index. | — | — |
| <a id="api-a8de581702a7f370"></a>[`public Inno.Rendering.RenderGraphBuilder Inno.Rendering.Runtime.RenderPipelineContext.graph { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineContext.cs#L100) | Gets the current frame graph builder. | — | — |
| <a id="api-205109fa6edd3401"></a>[`public Inno.Rendering.RenderTextureHandle Inno.Rendering.Runtime.RenderPipelineContext.outputTexture { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineContext.cs#L143) | Gets the imported offscreen target, or an invalid handle for the backbuffer. | — | — |
| <a id="api-0b92b3bf20ac8997"></a>[`public Inno.Rendering.Assets.RenderPipelineAsset Inno.Rendering.Runtime.RenderPipelineContext.pipelineAsset { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineContext.cs#L95) | Gets selected pipeline configuration. | — | — |
| <a id="api-c93b34aa82be7d43"></a>[`public bool Inno.Rendering.Runtime.RenderPipelineContext.preservePresentationTarget { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineContext.cs#L138) | Gets whether this model contribution must load and preserve an earlier contribution to the same target. | A pipeline must not clear its presentation color target when this value is true. | — |
| <a id="api-0872ecbb753504b6"></a>[`public Inno.Rendering.Runtime.RenderRequest Inno.Rendering.Runtime.RenderPipelineContext.request { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineContext.cs#L90) | Gets the current request. | — | — |
| <a id="api-322df3b9b0f84558"></a>[`public Inno.Rendering.Runtime.RenderResourceMap Inno.Rendering.Runtime.RenderPipelineContext.resources { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineContext.cs#L110) | Gets open semantic resources for this request. | — | — |
| <a id="api-349b6e7a5404a210"></a>[`public Inno.Rendering.Runtime.IRenderResourceService Inno.Rendering.Runtime.RenderPipelineContext.resourceService { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineContext.cs#L120) | Gets the generation-aware neutral GPU resource service. | — | — |
| <a id="api-ade6aa9d492a0791"></a>[`public Inno.Rendering.IRenderFrameUploadService Inno.Rendering.Runtime.RenderPipelineContext.uploads { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineContext.cs#L125) | Gets the frame-scoped streaming buffer service. | — | — |
| <a id="api-0ec8dea9180a4bdd"></a>[`public sealed class Inno.Rendering.Runtime.RenderPipelineContext`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineContext.cs#L15) | Supplies one request and frame-scoped services to a render pipeline. | — | — |

### `Inno.Rendering.Runtime.RenderPipelineExtensionAttribute`

<!-- api: T:Inno.Rendering.Runtime.RenderPipelineExtensionAttribute f33fe00eb0b6869f07cd9e7c0f898fadb9c689cca607d256b5bb6893011e040c -->
<!-- api: M:Inno.Rendering.Runtime.RenderPipelineExtensionAttribute.#ctor(System.String) dbd6c77e19e0eb229f16d1f206237b1e3fc583746e903fea5716192901ef07ea -->
<!-- api: P:Inno.Rendering.Runtime.RenderPipelineExtensionAttribute.id 2c9af5a58344150e86bc958b130b7581f469b176df3d90e23571ef7193dca57e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e1be440c72df2fee"></a>[`public Inno.Rendering.Runtime.RenderPipelineExtensionAttribute.RenderPipelineExtensionAttribute(string id)`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineExtensionAttribute.cs#L24) | Creates a pipeline extension declaration.<br>`id`：Globally stable pipeline extension identifier. | — | — |
| <a id="api-c6e6bd7b55add9f1"></a>[`public string Inno.Rendering.Runtime.RenderPipelineExtensionAttribute.id { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineExtensionAttribute.cs#L33) | Gets the globally stable pipeline extension identifier. | — | — |
| <a id="api-73bdc015fe7840f5"></a>[`public sealed class Inno.Rendering.Runtime.RenderPipelineExtensionAttribute : System.Attribute`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineExtensionAttribute.cs#L16) | Marks a reloadable render pipeline implementation with a stable extension identifier. | — | — |

### `Inno.Rendering.Runtime.RenderPipelineFeature`

<!-- api: T:Inno.Rendering.Runtime.RenderPipelineFeature e13ea9f674e590ea1a6f0e49818ebd47b3ad6ed7b82fc1b590114334d9fd5b61 -->
<!-- api: M:Inno.Rendering.Runtime.RenderPipelineFeature.Configure(Inno.Rendering.Assets.RenderFeatureConfiguration,Inno.Rendering.Runtime.RenderExtensionStateContext) 2388f43e5d3180482ca0342e7445fe4956d404213ec3d7a9b5c258a4b741572b -->
<!-- api: M:Inno.Rendering.Runtime.RenderPipelineFeature.AddRenderPasses(Inno.Rendering.Runtime.RenderFeatureContext) 71b0ce315fc527b1ea6119d44c278dcc77d822d206694655e001f83cd9fe6fa8 -->
<!-- api: M:Inno.Rendering.Runtime.RenderPipelineFeature.OnConfigure(Inno.Rendering.Assets.SerializedRenderExtensionState,Inno.Rendering.Runtime.RenderExtensionStateContext) a4dd17a39e821f7a0787263db3f669bec32876638fc712c40e059b839347e884 -->
<!-- api: M:Inno.Rendering.Runtime.RenderPipelineFeature.#ctor ade60dc4e255640732610c94390ff7a1eab7627f96e8151b4c4724496855c246 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3ef1cca084ed8d98"></a>[`protected Inno.Rendering.Runtime.RenderPipelineFeature.RenderPipelineFeature()`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineFeature.cs#L15) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-6578a1e17a044599"></a>[`public abstract void Inno.Rendering.Runtime.RenderPipelineFeature.AddRenderPasses(Inno.Rendering.Runtime.RenderFeatureContext context)`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineFeature.cs#L40) | Adds frame-scoped passes and dependencies.<br>`context`：Frame-scoped feature context. | — | — |
| <a id="api-e857243ba3d6a519"></a>[`public void Inno.Rendering.Runtime.RenderPipelineFeature.Configure(Inno.Rendering.Assets.RenderFeatureConfiguration configuration, Inno.Rendering.Runtime.RenderExtensionStateContext settings)`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineFeature.cs#L26) | Applies reload-safe settings to this feature generation.<br>`configuration`：Stable feature configuration.<br>`settings`：Native context bound to the canonical Pipeline asset's actual owner. | — | — |
| <a id="api-18d8ea91415c8518"></a>[`protected virtual void Inno.Rendering.Runtime.RenderPipelineFeature.OnConfigure(Inno.Rendering.Assets.SerializedRenderExtensionState state, Inno.Rendering.Runtime.RenderExtensionStateContext settings)`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineFeature.cs#L51) | Reads feature-owned settings from neutral state.<br>`state`：Reload-safe extension state.<br>`settings`：Native context bound to the canonical Pipeline asset's actual owner. | — | — |
| <a id="api-1227515dd306beb4"></a>[`public abstract class Inno.Rendering.Runtime.RenderPipelineFeature`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderPipelineFeature.cs#L15) | Adds capability-aware passes without owning frame graph state. | — | — |

### `Inno.Rendering.Runtime.RenderRequest`

<!-- api: T:Inno.Rendering.Runtime.RenderRequest fc5bbe81261c6f2083c4d1bbcff6fcd868b9c4933a61e1b70d7d1653a276ae64 -->
<!-- api: M:Inno.Rendering.Runtime.RenderRequest.#ctor(System.String,Inno.Rendering.RenderTarget,Inno.Rendering.RenderViewport,Inno.Rendering.Assets.RenderPipelineAsset,Inno.Rendering.Runtime.RenderFrameData,System.Int32) 54596198018d0e6eeb7813f2ae0c4d1f7019b98af249a475909bf492e8f2df43 -->
<!-- api: P:Inno.Rendering.Runtime.RenderRequest.name 7816099b44efe01df9889a87ceb1121017adbafd0d9b3609ad9475a96814b4fc -->
<!-- api: P:Inno.Rendering.Runtime.RenderRequest.target d9555ea5f42faffbd31ed18bfc9cdb61899a9b0435f9996076f8ee6c7a2ad7aa -->
<!-- api: P:Inno.Rendering.Runtime.RenderRequest.viewport 1cdc95780236bdd95d2a5a45984fffad4c794e766f400383ded6ce63cbb778e2 -->
<!-- api: P:Inno.Rendering.Runtime.RenderRequest.pipeline 31267f18ae11f6023799e703d9063b1372ea03605e053b82b49fb98aca3909ce -->
<!-- api: P:Inno.Rendering.Runtime.RenderRequest.data 369d678c2490e4ce81f6c96ffc2652b356ff687b70f3751a9ee09a30819e4a24 -->
<!-- api: P:Inno.Rendering.Runtime.RenderRequest.priority 532e5927366b29551026c81688c8fe2009d6a93d36b843422175f00fd1b58cfc -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-08c03228ab0b3de8"></a>[`public Inno.Rendering.Runtime.RenderRequest.RenderRequest(string name, Inno.Rendering.RenderTarget target, Inno.Rendering.RenderViewport viewport, Inno.Rendering.Assets.RenderPipelineAsset? pipeline = null, Inno.Rendering.Runtime.RenderFrameData? data = null, int priority = 0)`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequest.cs#L35) | Creates an immutable render request.<br>`name`：Frame-local diagnostic name.<br>`target`：Render destination.<br>`viewport`：Destination pixel viewport.<br>`pipeline`：Optional per-request pipeline asset; the project default is used when null.<br>`data`：Optional pipeline-defined frame data copied into an immutable snapshot.<br>`priority`：Ascending frame scheduling priority. | — | — |
| <a id="api-b08f518d312ee56a"></a>[`public Inno.Rendering.Runtime.RenderFrameData Inno.Rendering.Runtime.RenderRequest.data { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequest.cs#L75) | Gets immutable pipeline-defined frame data. | — | — |
| <a id="api-c8ab18245fdf1be1"></a>[`public string Inno.Rendering.Runtime.RenderRequest.name { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequest.cs#L55) | Gets the frame-local diagnostic name. | — | — |
| <a id="api-7d726c77409f55ec"></a>[`public Inno.Rendering.Assets.RenderPipelineAsset? Inno.Rendering.Runtime.RenderRequest.pipeline { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequest.cs#L70) | Gets the per-request pipeline asset, or null to use the project default. | — | — |
| <a id="api-9d1847160f418e00"></a>[`public int Inno.Rendering.Runtime.RenderRequest.priority { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequest.cs#L80) | Gets the ascending frame scheduling priority. | — | — |
| <a id="api-5747a52bba243eec"></a>[`public Inno.Rendering.RenderTarget Inno.Rendering.Runtime.RenderRequest.target { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequest.cs#L60) | Gets the render destination. | — | — |
| <a id="api-1ecf3df0c876c8ce"></a>[`public Inno.Rendering.RenderViewport Inno.Rendering.Runtime.RenderRequest.viewport { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequest.cs#L65) | Gets the destination pixel viewport. | — | — |
| <a id="api-ef4b313500d5f534"></a>[`public sealed class Inno.Rendering.Runtime.RenderRequest`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequest.cs#L12) | Requests one pipeline-defined rendering operation without prescribing world semantics. | — | — |

### `Inno.Rendering.Runtime.RenderRequestProvider`

<!-- api: T:Inno.Rendering.Runtime.RenderRequestProvider d609d1300de8c4ab1468a767abaca5f3b127aeb154a22d54a49732ce4b7a53d1 -->
<!-- api: M:Inno.Rendering.Runtime.RenderRequestProvider.Submit(Inno.Rendering.Runtime.RenderRequestProviderContext) 92335523cd41926ed230db3409a56142abc91cb7486b14e9f1bca936ee1a2540 -->
<!-- api: M:Inno.Rendering.Runtime.RenderRequestProvider.Dispose 7565dac4785296dac34c7b81df5f48c6bb3642293d93c9b38cdf6f945e1cebeb -->
<!-- api: M:Inno.Rendering.Runtime.RenderRequestProvider.Dispose(System.Boolean) 8bbf98e86f5309525153e3e1874a97be832d1bc8e60a147ff58c82bb6832fea8 -->
<!-- api: M:Inno.Rendering.Runtime.RenderRequestProvider.#ctor 95c714440b935d844911790169ecf1249b06e3e7d87bc692d83a91fce04d4b10 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-50589c9741f477a5"></a>[`protected Inno.Rendering.Runtime.RenderRequestProvider.RenderRequestProvider()`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L162) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-ed330ccdae3ce491"></a>[`public void Inno.Rendering.Runtime.RenderRequestProvider.Dispose()`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L180) | Releases generation-scoped provider state. | — | `Inno.Core.Execution.RetirementPendingException`：Provider work is still active. The owner must retain this instance and retry before releasing dependencies. |
| <a id="api-44cc12008a7e068f"></a>[`protected virtual void Inno.Rendering.Runtime.RenderRequestProvider.Dispose(bool disposing)`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L210) | Releases managed generation-scoped state.<br>`disposing`：Always true for explicit disposal. | — | `Inno.Core.Execution.RetirementPendingException`：Retirement cannot complete yet; subsequent calls resume this hook with its remaining owned resources. |
| <a id="api-7dde9b79b22f298b"></a>[`public abstract void Inno.Rendering.Runtime.RenderRequestProvider.Submit(Inno.Rendering.Runtime.RenderRequestProviderContext context)`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L172) | Submits zero or more requests for the current frame.<br>`context`：Frame-scoped provider context. | — | — |
| <a id="api-d97f66c0c7a431ad"></a>[`public abstract class Inno.Rendering.Runtime.RenderRequestProvider : System.IDisposable`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L162) | Produces arbitrary render requests without prescribing a scene or rendering model. | — | — |

### `Inno.Rendering.Runtime.RenderRequestProviderContext`

<!-- api: T:Inno.Rendering.Runtime.RenderRequestProviderContext af1153befb3c609c7b58edfcc0e14de68f0ff5b7d56db4de050112afb8d98fdb -->
<!-- api: M:Inno.Rendering.Runtime.RenderRequestProviderContext.#ctor(Inno.Rendering.Runtime.IRenderRequestSink,Inno.References.ContentReadScope,Inno.Rendering.GraphicsCapabilities,System.Nullable{Inno.Rendering.RenderPresentationSize},System.Nullable{Inno.Rendering.RenderViewport},System.UInt64,System.Single,Inno.Rendering.Runtime.IViewContentCollector,Inno.Rendering.Runtime.RenderOutputInput) f1c131591fde1c185fe7b3132e9953068284b49aabdcec8f2c92813524984627 -->
<!-- api: P:Inno.Rendering.Runtime.RenderRequestProviderContext.requests d441b57b7e627d97aa4efeb2484660b2186b8e60defa4c41f41c481fd8a68af0 -->
<!-- api: P:Inno.Rendering.Runtime.RenderRequestProviderContext.content c30bca0dbe41add04bb2ade5a8f9085d0e09d3168b8016e6be9e0560e70cf671 -->
<!-- api: P:Inno.Rendering.Runtime.RenderRequestProviderContext.capabilities 0c482104fadf6cb15df0c8969e0f588ec384309743e34aa5d6735344cdea0791 -->
<!-- api: P:Inno.Rendering.Runtime.RenderRequestProviderContext.primaryPresentationSize aefbb008c7fce9b50291ab5d03af35e0ad0f35629beba509596489a232d0e83e -->
<!-- api: P:Inno.Rendering.Runtime.RenderRequestProviderContext.primaryPresentationViewport a0807b4bff565688100a8564ff669a1075c96e793ceec541e973a06b7e03f756 -->
<!-- api: P:Inno.Rendering.Runtime.RenderRequestProviderContext.frameIndex d73997a7937b9d2cefd480b5032290242d192b0f52220a5a1227c94117ffcc2f -->
<!-- api: P:Inno.Rendering.Runtime.RenderRequestProviderContext.deltaTime 0b59ca4ebf0ff5bca73c76a1ab1026afcd4d364084663ab77982c956f055e4d0 -->
<!-- api: P:Inno.Rendering.Runtime.RenderRequestProviderContext.viewContent 0536459fde9837460d9aa9b7488c030220394ebe6946024c07320e5b0d2c6fe7 -->
<!-- api: P:Inno.Rendering.Runtime.RenderRequestProviderContext.input 1fb04762f02b5c26ef6c57ae84c61cf60ca63ff08dd8974c132931a450a9411b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c42665e409a56e55"></a>[`public Inno.Rendering.Runtime.RenderRequestProviderContext.RenderRequestProviderContext(Inno.Rendering.Runtime.IRenderRequestSink requests, Inno.References.ContentReadScope content, Inno.Rendering.GraphicsCapabilities capabilities, Inno.Rendering.RenderPresentationSize? primaryPresentationSize, Inno.Rendering.RenderViewport? primaryPresentationViewport, ulong frameIndex, float deltaTime, Inno.Rendering.Runtime.IViewContentCollector viewContent, Inno.Rendering.Runtime.RenderOutputInput? input = null)`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L80) | Creates a frame-scoped provider context.<br>`requests`：Sink accepting requests for the current frame.<br>`content`：Host-selected content roots visible to request providers this frame.<br>`capabilities`：Active backend-neutral capability snapshot.<br>`primaryPresentationSize`：Current primary pixel extent, or null while no primary output is available.<br>`primaryPresentationViewport`：Host-selected content region, or null when the primary extent is unavailable.<br>`frameIndex`：Monotonic render frame index.<br>`deltaTime`：Elapsed frame time in seconds.<br>`viewContent`：Collector for model-independent world items.<br>`input`：Viewport-local input for the primary output. | — | — |
| <a id="api-4bcdf9164e1ebb5a"></a>[`public Inno.Rendering.GraphicsCapabilities Inno.Rendering.Runtime.RenderRequestProviderContext.capabilities { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L126) | Gets the active backend-neutral capability snapshot. | — | — |
| <a id="api-1b1f74c8c4b998cb"></a>[`public Inno.References.ContentReadScope Inno.Rendering.Runtime.RenderRequestProviderContext.content { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L121) | Gets the explicit ordered host content visible to request providers this frame. | — | — |
| <a id="api-29c42259fa50a3a6"></a>[`public float Inno.Rendering.Runtime.RenderRequestProviderContext.deltaTime { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L146) | Gets the elapsed frame time in seconds. | — | — |
| <a id="api-c4c15b024303d7ef"></a>[`public ulong Inno.Rendering.Runtime.RenderRequestProviderContext.frameIndex { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L141) | Gets the monotonic render frame index. | — | — |
| <a id="api-0e47148b8c6837dc"></a>[`public Inno.Rendering.Runtime.RenderOutputInput Inno.Rendering.Runtime.RenderRequestProviderContext.input { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L155) | Gets viewport-local input for the primary output. | — | — |
| <a id="api-fed3c9b0aa5d189f"></a>[`public Inno.Rendering.RenderPresentationSize? Inno.Rendering.Runtime.RenderRequestProviderContext.primaryPresentationSize { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L131) | Gets the current primary pixel extent, or null while only offscreen outputs are available. | — | — |
| <a id="api-34f3221cc84a6b55"></a>[`public Inno.Rendering.RenderViewport? Inno.Rendering.Runtime.RenderRequestProviderContext.primaryPresentationViewport { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L136) | Gets the host-selected primary region, or null while no primary output is available. | — | — |
| <a id="api-6a0acd64311e5c6e"></a>[`public Inno.Rendering.Runtime.IRenderRequestSink Inno.Rendering.Runtime.RenderRequestProviderContext.requests { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L116) | Gets the sink accepting requests for the current frame. | — | — |
| <a id="api-9916e644a7fb7cff"></a>[`public Inno.Rendering.Runtime.IViewContentCollector Inno.Rendering.Runtime.RenderRequestProviderContext.viewContent { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L151) | Gets the active generation's world-content collector. | — | — |
| <a id="api-1ab30f796c6e509a"></a>[`public sealed class Inno.Rendering.Runtime.RenderRequestProviderContext`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L48) | Supplies frame timing, capabilities and the request sink to one provider invocation. | — | — |

### `Inno.Rendering.Runtime.RenderRequestProviderExtensionAttribute`

<!-- api: T:Inno.Rendering.Runtime.RenderRequestProviderExtensionAttribute 36fe4d12a588c75522b8f91f8970f99059d06045c8f36bdd36af07b1f04b95c1 -->
<!-- api: M:Inno.Rendering.Runtime.RenderRequestProviderExtensionAttribute.#ctor(System.String,System.Int32) 90f2e1429fdb43dd8612e8b6ba226f566dfafa11c6bdd02bf7e280e47ad63c90 -->
<!-- api: P:Inno.Rendering.Runtime.RenderRequestProviderExtensionAttribute.id 6ec522338b7f3c322dd7e4c0a7c4c7f22c54b6d1e2be40b30692efc8efbf3e0f -->
<!-- api: P:Inno.Rendering.Runtime.RenderRequestProviderExtensionAttribute.priority 126f283fdf6b2df9ce5cd7253bf64fb210269dbc11ffda7899e4967f930483e5 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-52a817d1f34e948c"></a>[`public Inno.Rendering.Runtime.RenderRequestProviderExtensionAttribute.RenderRequestProviderExtensionAttribute(string id, int priority = 0)`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L24) | Creates a render request provider declaration.<br>`id`：Globally stable provider identifier.<br>`priority`：Provider invocation priority; lower values run first. | — | — |
| <a id="api-062af3b553b588e0"></a>[`public string Inno.Rendering.Runtime.RenderRequestProviderExtensionAttribute.id { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L36) | Gets the globally stable provider identifier. | — | — |
| <a id="api-057a2f295750f561"></a>[`public int Inno.Rendering.Runtime.RenderRequestProviderExtensionAttribute.priority { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L41) | Gets the provider invocation priority. | — | — |
| <a id="api-cfc44dd99f335900"></a>[`public sealed class Inno.Rendering.Runtime.RenderRequestProviderExtensionAttribute : System.Attribute`](../../src/services/rendering/Inno.Rendering.Runtime/Requests/RenderRequestProvider.cs#L13) | Marks a reloadable provider that produces model-neutral render requests each frame. | — | — |

### `Inno.Rendering.Runtime.RenderResourceLimits`

<!-- api: T:Inno.Rendering.Runtime.RenderResourceLimits 0c112ffe6ff68a6fe19ac2aa8d8731cfe5f9dcd8e5b561e0ece85e349a4132a5 -->
<!-- api: P:Inno.Rendering.Runtime.RenderResourceLimits.resourcesPerKind 25d19f3a8676cfe501c756c432671d8704a593ade2d0877f7f33c6d1dee4d844 -->
<!-- api: P:Inno.Rendering.Runtime.RenderResourceLimits.pendingReadbacks eedf74ee660a441d1ccc99816b9102876a753dcf0ea7a8a8be00255ca1c928e3 -->
<!-- api: P:Inno.Rendering.Runtime.RenderResourceLimits.uploadPages ca005b65c010a8a25da9b6c65836981605353d3ca005ddea1d07e87d6ab0babe -->
<!-- api: P:Inno.Rendering.Runtime.RenderResourceLimits.uploadResidentBytes badce4909daeb79205b1a194e7467bb0d04aebc7f632f570acc3c9f24e044ed4 -->
<!-- api: P:Inno.Rendering.Runtime.RenderResourceLimits.uploadBytesPerFrame 5208904dd00c1a6f1f51141e5c270831427e49320724eccee2670c06497e6572 -->
<!-- api: P:Inno.Rendering.Runtime.RenderResourceLimits.targets fa52b12065aa7ae5ae78b23cf57f14f0e2754b127750554a1a4319289e0c865f -->
<!-- api: M:Inno.Rendering.Runtime.RenderResourceLimits.#ctor 398c478d5779c3fd2e5548a6b76cdae5b8db3d80d70f64a32dc990a3f1027f2e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8bcc239e47748e8f"></a>[`public Inno.Rendering.Runtime.RenderResourceLimits.RenderResourceLimits()`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceLimits.cs#L8) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-c81132f27f05218a"></a>[`public int Inno.Rendering.Runtime.RenderResourceLimits.pendingReadbacks { get; init; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceLimits.cs#L18) | Gets the maximum readbacks that can retain GPU resources before owner-thread completion. | — | — |
| <a id="api-2c213ac43aa7826c"></a>[`public int Inno.Rendering.Runtime.RenderResourceLimits.resourcesPerKind { get; init; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceLimits.cs#L13) | Gets the maximum active entries in each buffer, texture or pipeline cache. | — | — |
| <a id="api-868654d1bfa50a15"></a>[`public int Inno.Rendering.Runtime.RenderResourceLimits.targets { get; init; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceLimits.cs#L35) | Gets the offscreen target count and per-frame target allocation capacity. | — | — |
| <a id="api-66668c01512251a5"></a>[`public long Inno.Rendering.Runtime.RenderResourceLimits.uploadBytesPerFrame { get; init; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceLimits.cs#L31) | Gets the maximum bytes uploaded during one frame. | — | — |
| <a id="api-3075bcf5a72778f2"></a>[`public int Inno.Rendering.Runtime.RenderResourceLimits.uploadPages { get; init; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceLimits.cs#L23) | Gets the maximum resident frame-upload pages. | — | — |
| <a id="api-3624e9a7a75d8381"></a>[`public long Inno.Rendering.Runtime.RenderResourceLimits.uploadResidentBytes { get; init; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceLimits.cs#L27) | Gets the maximum resident frame-upload bytes, including retired pages. | — | — |
| <a id="api-166b1e6dec7d3e84"></a>[`public sealed class Inno.Rendering.Runtime.RenderResourceLimits`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceLimits.cs#L8) | Sets finite native resource and asynchronous readback admission limits for one rendering owner. | — | — |

### `Inno.Rendering.Runtime.RenderResourceMap`

<!-- api: T:Inno.Rendering.Runtime.RenderResourceMap 3c37210b5e83fedbfbe92450c4f4d61704292783e7d92a40abf6116a6220b58b -->
<!-- api: M:Inno.Rendering.Runtime.RenderResourceMap.PublishTexture(Inno.Rendering.RenderResourceId,Inno.Rendering.RenderTextureHandle) 63376f0d9334728f80ed68f4c7100ed49d1572467d514cc54de75a2d055ed713 -->
<!-- api: M:Inno.Rendering.Runtime.RenderResourceMap.PublishBuffer(Inno.Rendering.RenderResourceId,Inno.Rendering.RenderBufferHandle) a02eced3af3f73a316cbc6da2327bc6f2c8272134742fbe90e9c6b525d3696ee -->
<!-- api: M:Inno.Rendering.Runtime.RenderResourceMap.TryGetTexture(Inno.Rendering.RenderResourceId,Inno.Rendering.RenderTextureHandle@) 7edb20ef312b93b23d671007919f0bf079ce1ba20cc34ca8a1d00a44f1b9c681 -->
<!-- api: M:Inno.Rendering.Runtime.RenderResourceMap.TryGetBuffer(Inno.Rendering.RenderResourceId,Inno.Rendering.RenderBufferHandle@) 21f7439aedd144b0c13b83b33b7f1e12925aa370f30516bc4b77fff6fc4f2456 -->
<!-- api: M:Inno.Rendering.Runtime.RenderResourceMap.#ctor 594127611b698866f53e1f4d0921fd76da10893331e140d13e15834dcf6123d0 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c58c6afb0d7ab1be"></a>[`public Inno.Rendering.Runtime.RenderResourceMap.RenderResourceMap()`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderResourceMap.cs#L15) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-1f4b4f9a11e30596"></a>[`public void Inno.Rendering.Runtime.RenderResourceMap.PublishBuffer(Inno.Rendering.RenderResourceId id, Inno.Rendering.RenderBufferHandle buffer)`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderResourceMap.cs#L49) | Publishes a buffer under a pipeline-defined semantic identifier.<br>`id`：Open semantic resource identifier.<br>`buffer`：Valid current-graph buffer. | — | — |
| <a id="api-faee89979b68a877"></a>[`public void Inno.Rendering.Runtime.RenderResourceMap.PublishTexture(Inno.Rendering.RenderResourceId id, Inno.Rendering.RenderTextureHandle texture)`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderResourceMap.cs#L29) | Publishes a texture under a pipeline-defined semantic identifier.<br>`id`：Open semantic resource identifier.<br>`texture`：Valid current-graph texture. | — | — |
| <a id="api-0c5fe7cafe738102"></a>[`public bool Inno.Rendering.Runtime.RenderResourceMap.TryGetBuffer(Inno.Rendering.RenderResourceId id, out Inno.Rendering.RenderBufferHandle buffer)`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderResourceMap.cs#L89) | Tries to get a published buffer.<br>返回：true when the buffer has been published.<br>`id`：Open semantic resource identifier.<br>`buffer`：Receives the current-graph buffer. | — | — |
| <a id="api-222aa1a37dfcb47f"></a>[`public bool Inno.Rendering.Runtime.RenderResourceMap.TryGetTexture(Inno.Rendering.RenderResourceId id, out Inno.Rendering.RenderTextureHandle texture)`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderResourceMap.cs#L72) | Tries to get a published texture.<br>返回：true when the texture has been published.<br>`id`：Open semantic resource identifier.<br>`texture`：Receives the current-graph texture. | — | — |
| <a id="api-afd6cfc2a37a17a5"></a>[`public sealed class Inno.Rendering.Runtime.RenderResourceMap`](../../src/services/rendering/Inno.Rendering.Runtime/Pipelines/RenderResourceMap.cs#L15) | Stores open semantic graph resources for one pipeline request. | — | — |

### `Inno.Rendering.Runtime.RenderResourceProvider`

<!-- api: T:Inno.Rendering.Runtime.RenderResourceProvider 6a9d840283dadda1a4178583fb0a5626a39924fc376e5d0473aaab5e2acc5285 -->
<!-- api: M:Inno.Rendering.Runtime.RenderResourceProvider.CreateUniformBinding(Inno.Rendering.RenderBindingId,System.ReadOnlySpan{System.Byte}) 82f26b58392a2feaa9918deb07efde0a844cfe7a215ed5cde01eb8d6339f3903 -->
<!-- api: M:Inno.Rendering.Runtime.RenderResourceProvider.CreateTextureBinding(Inno.Rendering.RenderBindingId,Inno.Rendering.PersistentTextureHandle,Inno.Rendering.RenderSamplerState) f1f5f338e740d37a51608a4f7c934d7825f54b56690b5d95e07694db4916b6e4 -->
<!-- api: M:Inno.Rendering.Runtime.RenderResourceProvider.CreateMaterialPass(Inno.Rendering.Assets.ShaderPassDefinition,Inno.Rendering.GraphicsPipelineHandle,Inno.Rendering.ComputePipelineHandle,System.Collections.Generic.IReadOnlyList{Inno.Rendering.Runtime.RenderResourceProvider.MaterialBinding}) 4a397a030971db908c5fd5161c1e950f194c934cc38ffa0418b6bcee74d9e789 -->
<!-- api: M:Inno.Rendering.Runtime.RenderResourceProvider.CreateMaterialPass(Inno.Rendering.Assets.ShaderPassDefinition,Inno.Rendering.GraphicsPipelineHandle,Inno.Rendering.ComputePipelineHandle,System.Collections.Generic.IReadOnlyList{Inno.Rendering.Assets.ShaderPropertyDefinition},Inno.Rendering.ShaderInterface,System.Collections.Generic.IReadOnlyList{Inno.Rendering.Runtime.RenderResourceProvider.MaterialBinding}) 406b662a15963d7a68dca2e6170ce18a852cebaef62d9ece7e8ed430691e677d -->
<!-- api: M:Inno.Rendering.Runtime.RenderResourceProvider.CreateGeometry(Inno.Rendering.PersistentBufferHandle,Inno.Rendering.PersistentBufferHandle,Inno.Rendering.RenderVertexLayout,System.Int32,System.Int32,System.Collections.Generic.IReadOnlyList{Inno.Rendering.Runtime.RenderGeometrySection}) e7cafb9aa2e4bf48f698a92746c60ee762a69677fb63c5c390a614a444f8f0ae -->
<!-- api: M:Inno.Rendering.Runtime.RenderResourceProvider.#ctor bbb46b9cdca02aed2afb1d02138058459ef3750490a62e340096eafe2b92e37c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-ab192a61a9e60965"></a>[`protected Inno.Rendering.Runtime.RenderResourceProvider.RenderResourceProvider()`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderResourceProvider.cs#L17) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-61989c0a0ae7b8ab"></a>[`protected static Inno.Rendering.Runtime.RenderGeometry Inno.Rendering.Runtime.RenderResourceProvider.CreateGeometry(Inno.Rendering.PersistentBufferHandle vertexBuffer, Inno.Rendering.PersistentBufferHandle indexBuffer, Inno.Rendering.RenderVertexLayout vertexLayout, int vertexCount, int indexCount, System.Collections.Generic.IReadOnlyList<Inno.Rendering.Runtime.RenderGeometrySection> sections)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderResourceProvider.cs#L163) | Creates generation-scoped resolved geometry from persistent buffers and immutable sections.<br>返回：Resolved geometry owned by the current device generation.<br>`vertexBuffer`：The persistent vertex buffer.<br>`indexBuffer`：The persistent index buffer.<br>`vertexLayout`：The interleaved vertex layout.<br>`vertexCount`：The number of vertices.<br>`indexCount`：The number of indices.<br>`sections`：The independently drawable indexed ranges. | — | — |
| <a id="api-0a3b44ba9b4334ec"></a>[`protected static Inno.Rendering.Runtime.RenderMaterialPass Inno.Rendering.Runtime.RenderResourceProvider.CreateMaterialPass(Inno.Rendering.Assets.ShaderPassDefinition definition, Inno.Rendering.GraphicsPipelineHandle graphicsPipeline, Inno.Rendering.ComputePipelineHandle computePipeline, System.Collections.Generic.IReadOnlyList<Inno.Rendering.Assets.ShaderPropertyDefinition> declaredBindings, Inno.Rendering.ShaderInterface activeInterface, System.Collections.Generic.IReadOnlyList<Inno.Rendering.Runtime.RenderResourceProvider.MaterialBinding> bindings)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderResourceProvider.cs#L123) | Creates a generation-scoped resolved material pass.<br>返回：A resolved material pass ready for command binding.<br>`definition`：The selected shader pass definition.<br>`graphicsPipeline`：The graphics pipeline handle, or an invalid handle for compute.<br>`computePipeline`：The compute pipeline handle, or an invalid handle for rasterization.<br>`declaredBindings`：Complete shader binding declarations before backend optimization.<br>`activeInterface`：Exact bindings reflected by the compiled pass after backend optimization.<br>`bindings`：The complete material-owned binding snapshot. | — | — |
| <a id="api-dfe68a57abc0ec52"></a>[`protected static Inno.Rendering.Runtime.RenderMaterialPass Inno.Rendering.Runtime.RenderResourceProvider.CreateMaterialPass(Inno.Rendering.Assets.ShaderPassDefinition definition, Inno.Rendering.GraphicsPipelineHandle graphicsPipeline, Inno.Rendering.ComputePipelineHandle computePipeline, System.Collections.Generic.IReadOnlyList<Inno.Rendering.Runtime.RenderResourceProvider.MaterialBinding> bindings)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderResourceProvider.cs#L91) | Creates a generation-scoped resolved material pass when no compiled-interface query is required.<br>返回：A resolved material pass ready for automatic material binding. Calls to Inno.Rendering.Runtime.RenderMaterialPass.UsesBinding(Inno.Rendering.RenderBindingId,Inno.Rendering.RenderShaderBindingKind) require the overload carrying the compiled interface.<br>`definition`：The selected shader pass definition.<br>`graphicsPipeline`：The graphics pipeline handle, or an invalid handle for compute.<br>`computePipeline`：The compute pipeline handle, or an invalid handle for rasterization.<br>`bindings`：The complete material-owned binding snapshot. | — | — |
| <a id="api-7ac995b5639464fd"></a>[`protected static Inno.Rendering.Runtime.RenderResourceProvider.MaterialBinding Inno.Rendering.Runtime.RenderResourceProvider.CreateTextureBinding(Inno.Rendering.RenderBindingId id, Inno.Rendering.PersistentTextureHandle texture, Inno.Rendering.RenderSamplerState sampler)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderResourceProvider.cs#L65) | Creates a protected sampled-texture binding value.<br>返回：A binding token accepted by the material-pass construction helpers.<br>`id`：The stable shader binding identifier.<br>`texture`：The persistent texture handle.<br>`sampler`：The backend-neutral sampler state. | — | — |
| <a id="api-e4a3b69c0d2e0d7d"></a>[`protected static Inno.Rendering.Runtime.RenderResourceProvider.MaterialBinding Inno.Rendering.Runtime.RenderResourceProvider.CreateUniformBinding(Inno.Rendering.RenderBindingId id, System.ReadOnlySpan<byte> data)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderResourceProvider.cs#L44) | Creates a protected uniform binding value.<br>返回：A binding token accepted by the material-pass construction helpers.<br>`id`：The stable shader binding identifier.<br>`data`：The complete encoded uniform bytes. | — | — |
| <a id="api-28839771cfe53b84"></a>[`public abstract class Inno.Rendering.Runtime.RenderResourceProvider`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderResourceProvider.cs#L17) | Provides protected construction operations for replaceable render resource-service implementations. | The boundary lets a runtime resource service create generation-scoped values while their mutable representation remains private to the Rendering assembly. | — |

### `Inno.Rendering.Runtime.RenderResourceProvider.MaterialBinding`

<!-- api: T:Inno.Rendering.Runtime.RenderResourceProvider.MaterialBinding cbbf9632fd00225f13c650667cf8f146bf8b28693b6f1094a947860e68cdd6df -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-48fa320138e06291"></a>[`protected readonly struct Inno.Rendering.Runtime.RenderResourceProvider.MaterialBinding`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderResourceProvider.cs#L22) | Represents one material binding staged by a derived resource service. | — | — |

### `Inno.Rendering.Runtime.RenderResourceStatistics`

<!-- api: T:Inno.Rendering.Runtime.RenderResourceStatistics a9e1d787986facc4de7a77a77a81877c77e8d5273358211a2ccd0522fb2cfa96 -->
<!-- api: P:Inno.Rendering.Runtime.RenderResourceStatistics.activeResources c3e95c79f95306abcddf9cbc4f7ff183ecb9460bc48b4178e57c04a9c5a9a999 -->
<!-- api: P:Inno.Rendering.Runtime.RenderResourceStatistics.retiringResources 29557f4ffcde3429856b8dd8cb3e5ec7e09435436478531e2787d9e7e0813120 -->
<!-- api: P:Inno.Rendering.Runtime.RenderResourceStatistics.rejectedResources e110af666546c81b26409a18eb999a3560507cba5299b936779b72d2366467dd -->
<!-- api: P:Inno.Rendering.Runtime.RenderResourceStatistics.pendingReadbacks 4e2b0592211e9f83b8e84cc681d0c5c4835f445362a51825ab9ba7d9a1a87ba6 -->
<!-- api: P:Inno.Rendering.Runtime.RenderResourceStatistics.peakReadbacks 0c88a1429d9cfdc528f09aaee5042a8beb5e115f460699b2d7cf95091eaf3b96 -->
<!-- api: P:Inno.Rendering.Runtime.RenderResourceStatistics.rejectedReadbacks 834579b2dba13cd519a78a361e635f5ba4f27cb40ba9aa351e7cc99ef0aba93d -->
<!-- api: P:Inno.Rendering.Runtime.RenderResourceStatistics.uploadPages 72f0b185464faaf209d3973b85a3e52a28b955740a41378b68fa3804de594e00 -->
<!-- api: P:Inno.Rendering.Runtime.RenderResourceStatistics.uploadResidentBytes 17f5ada6ef0330e7e4cc4171cf53591b2ddf1c255ec45a802b03b780109c41ca -->
<!-- api: P:Inno.Rendering.Runtime.RenderResourceStatistics.uploadPeakBytes 1a0ede453a21d661910f2e45700237bd0ae96b52cc1aad92fd5a0b0c13765105 -->
<!-- api: P:Inno.Rendering.Runtime.RenderResourceStatistics.uploadedFrameBytes 2c22059da3d7891e9be3377c3dd2555cb1a30a34878b4f72294640b6d3b03479 -->
<!-- api: P:Inno.Rendering.Runtime.RenderResourceStatistics.rejectedUploads 1655f0ba87348292baf5137de66c9604d48c7ebece0a253ce84c948dac4ede06 -->
<!-- api: P:Inno.Rendering.Runtime.RenderResourceStatistics.targets 890cdb1e89c339a8794cdeb903095b453ffb37a7bc9d58d041e734d48916834b -->
<!-- api: P:Inno.Rendering.Runtime.RenderResourceStatistics.rejectedTargets 7ea77e2566839d6d26e6bade9f3fd90f115372a5bf1f30079b1da62e5650b044 -->
<!-- api: M:Inno.Rendering.Runtime.RenderResourceStatistics.ToString 35cfca7280268360d32c60c86229f23af01f9f2d025ea9e48e5c402e4ce3b750 -->
<!-- api: M:Inno.Rendering.Runtime.RenderResourceStatistics.op_Inequality(Inno.Rendering.Runtime.RenderResourceStatistics,Inno.Rendering.Runtime.RenderResourceStatistics) 82e67aa1d8b4095f188cca01c91f344913804102a921fa5172281927be30719e -->
<!-- api: M:Inno.Rendering.Runtime.RenderResourceStatistics.op_Equality(Inno.Rendering.Runtime.RenderResourceStatistics,Inno.Rendering.Runtime.RenderResourceStatistics) 44142a085f9262636be57475d3a9a426fbe6544410374aa81c5a2bef6f86ca55 -->
<!-- api: M:Inno.Rendering.Runtime.RenderResourceStatistics.GetHashCode 1ce4223fd7ce2924c6f9cf1d265dddca34ada8fc99b92f9ab62652c12d3601c9 -->
<!-- api: M:Inno.Rendering.Runtime.RenderResourceStatistics.Equals(System.Object) b0fff4ed98f94191c44a2a8a726d824e1539c2ecfb8feeb3d145f14b03b0bd5a -->
<!-- api: M:Inno.Rendering.Runtime.RenderResourceStatistics.Equals(Inno.Rendering.Runtime.RenderResourceStatistics) 1bf05e2b95936612f56fddf61a3ce5d2a1a7277bd7bd1cacd60905bb8d70f8c0 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-74e1b9c40dd7f5eb"></a>[`public bool Inno.Rendering.Runtime.RenderResourceStatistics.Equals(Inno.Rendering.Runtime.RenderResourceStatistics other)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceStatistics.cs#L6) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-17c0b72034296049"></a>[`public override bool Inno.Rendering.Runtime.RenderResourceStatistics.Equals(object obj)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceStatistics.cs#L6) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-3e355e4f81da39da"></a>[`public override int Inno.Rendering.Runtime.RenderResourceStatistics.GetHashCode()`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceStatistics.cs#L6) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-2eb401b510b4b5d5"></a>[`public static bool Inno.Rendering.Runtime.RenderResourceStatistics.operator ==(Inno.Rendering.Runtime.RenderResourceStatistics left, Inno.Rendering.Runtime.RenderResourceStatistics right)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceStatistics.cs#L6) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-9cc60bae364614ff"></a>[`public static bool Inno.Rendering.Runtime.RenderResourceStatistics.operator !=(Inno.Rendering.Runtime.RenderResourceStatistics left, Inno.Rendering.Runtime.RenderResourceStatistics right)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceStatistics.cs#L6) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-fae6c93ff2dd0de7"></a>[`public override string Inno.Rendering.Runtime.RenderResourceStatistics.ToString()`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceStatistics.cs#L6) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-7b891f354f748d29"></a>[`public int Inno.Rendering.Runtime.RenderResourceStatistics.activeResources { get; init; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceStatistics.cs#L11) | Gets active persistent buffers, textures, pipelines, material programs and geometry pairs. | — | — |
| <a id="api-91401e61ee6b56b7"></a>[`public int Inno.Rendering.Runtime.RenderResourceStatistics.peakReadbacks { get; init; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceStatistics.cs#L27) | Gets the largest simultaneous readback occupancy. | — | — |
| <a id="api-876a42238e5596e9"></a>[`public int Inno.Rendering.Runtime.RenderResourceStatistics.pendingReadbacks { get; init; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceStatistics.cs#L23) | Gets readbacks that still own native operations. | — | — |
| <a id="api-9c5beebf5b45c8af"></a>[`public long Inno.Rendering.Runtime.RenderResourceStatistics.rejectedReadbacks { get; init; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceStatistics.cs#L31) | Gets readbacks rejected before native allocation. | — | — |
| <a id="api-971b49a476388794"></a>[`public long Inno.Rendering.Runtime.RenderResourceStatistics.rejectedResources { get; init; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceStatistics.cs#L19) | Gets cache admissions rejected by per-kind capacity. | — | — |
| <a id="api-4d4f430b060e54b1"></a>[`public long Inno.Rendering.Runtime.RenderResourceStatistics.rejectedTargets { get; init; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceStatistics.cs#L59) | Gets offscreen target requests rejected by finite capacity. | — | — |
| <a id="api-f117124850fbb0c4"></a>[`public long Inno.Rendering.Runtime.RenderResourceStatistics.rejectedUploads { get; init; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceStatistics.cs#L51) | Gets upload requests rejected by resident or per-frame limits. | — | — |
| <a id="api-8f6eceed85a38e03"></a>[`public int Inno.Rendering.Runtime.RenderResourceStatistics.retiringResources { get; init; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceStatistics.cs#L15) | Gets resource entries whose native retirement is still pending. | — | — |
| <a id="api-0740edadc2482270"></a>[`public int Inno.Rendering.Runtime.RenderResourceStatistics.targets { get; init; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceStatistics.cs#L55) | Gets active offscreen targets. | — | — |
| <a id="api-9307c156078a23b2"></a>[`public long Inno.Rendering.Runtime.RenderResourceStatistics.uploadedFrameBytes { get; init; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceStatistics.cs#L47) | Gets successfully uploaded bytes during the current frame. | — | — |
| <a id="api-688044c2eed3620c"></a>[`public int Inno.Rendering.Runtime.RenderResourceStatistics.uploadPages { get; init; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceStatistics.cs#L35) | Gets retained upload pages, including pages awaiting retirement. | — | — |
| <a id="api-4f55680c58ff0250"></a>[`public long Inno.Rendering.Runtime.RenderResourceStatistics.uploadPeakBytes { get; init; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceStatistics.cs#L43) | Gets the high-water mark of resident upload bytes. | — | — |
| <a id="api-793ecba2153c24e6"></a>[`public long Inno.Rendering.Runtime.RenderResourceStatistics.uploadResidentBytes { get; init; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceStatistics.cs#L39) | Gets resident upload bytes, including pages awaiting retirement. | — | — |
| <a id="api-ee7bf0989a3e6b7e"></a>[`public readonly record struct Inno.Rendering.Runtime.RenderResourceStatistics : System.IEquatable<Inno.Rendering.Runtime.RenderResourceStatistics>`](../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceStatistics.cs#L6) | Reports control-thread resource occupancy and admission pressure without exposing backend objects. | — | — |

### `Inno.Rendering.Runtime.RenderRuntime`

<!-- api: T:Inno.Rendering.Runtime.RenderRuntime 1ab46c99fa0a7b67010cf973813772e53315073d685e3c940081060f49ceb524 -->
<!-- api: M:Inno.Rendering.Runtime.RenderRuntime.RegisterContributor(Inno.Rendering.IRenderFrameGraphContributor) 1d54b8d640ad32f3dd32fdf0f57a2a1657e58b6e2e72463e367ff5f9a876cfad -->
<!-- api: M:Inno.Rendering.Runtime.RenderRuntime.UnregisterContributor(Inno.Rendering.IRenderFrameGraphContributor) 4023e46288cd77750e6c88c2b20983448f2625eca06d0a03dc5c6343c2b20abe -->
<!-- api: M:Inno.Rendering.Runtime.RenderRuntime.#ctor(Inno.Extensibility.Types.TypeCatalog,Inno.Rendering.IRenderDevice,Inno.Core.Diagnostics.IDiagnosticReporter,System.Collections.Generic.IEnumerable{Inno.Rendering.IRenderFrameGraphContributor},Inno.Rendering.Runtime.IRenderTargetArtifactProvider,System.Func{Inno.References.ContentReadScope},System.Func{Inno.Rendering.RenderPresentationSize,Inno.Rendering.RenderViewport},Inno.Rendering.Runtime.RenderResourceLimits,System.Func{Inno.Input.InputSnapshot},System.Func{System.Nullable{Inno.Rendering.RenderPresentationSize}},Inno.Rendering.IRenderLayerCompositionProgramProvider) 1fee43c930154ff531e5c98dd68a89fcaee6c6af102bb53254923d3bb67d6087 -->
<!-- api: P:Inno.Rendering.Runtime.RenderRuntime.targets d16ed624d8b58c5a0aa6aa3285de1ca581497241ea9e59458452c8e9d065c837 -->
<!-- api: P:Inno.Rendering.Runtime.RenderRuntime.resources 5a41936be0f8ad51195c575d7dd2b4a713ffaf9bc249d5a8eb63a110d84825c0 -->
<!-- api: P:Inno.Rendering.Runtime.RenderRuntime.viewContent fc1a9d08e776efb52d46c3afc0f93fc060217fa54befe1bd40dbdc3492a7cc7a -->
<!-- api: P:Inno.Rendering.Runtime.RenderRuntime.currentFrameIndex 9f83cb0f7f540b71d3d7be46cf8ffd44b5743462453c10f902277d38e74c61ef -->
<!-- api: M:Inno.Rendering.Runtime.RenderRuntime.Collect(Inno.Rendering.Runtime.ViewContentContext) db25d9826d7ac9763ec8636461c91e35dea7e712c61881596d38d21d644da1d6 -->
<!-- api: P:Inno.Rendering.Runtime.RenderRuntime.deviceGeneration 7735adb44af1e0150d81eecc94118fab4624268970fcaffef6022dfa29e54491 -->
<!-- api: P:Inno.Rendering.Runtime.RenderRuntime.resourceStatistics 35e0c078997369df0d47a654e6847daae3d13a48d672451ba9a186b1ebe2f275 -->
<!-- api: M:Inno.Rendering.Runtime.RenderRuntime.EnterExecutionScope 6262c9883e5d38e8bc75a4ad9fd927dbf843d5e5a0f56ee37dc51ef17e43a473 -->
<!-- api: M:Inno.Rendering.Runtime.RenderRuntime.OnStart 4052ba58e12cc011d7388ad9e1b9119d5692934b4633c250fc0ad3e88e1c5bcf -->
<!-- api: M:Inno.Rendering.Runtime.RenderRuntime.OnStop 106ea51ae55c69ef6ff9d7643b645b42321d065160282348c54208446731757a -->
<!-- api: M:Inno.Rendering.Runtime.RenderRuntime.OnBeginFrame(Inno.Runtime.Contracts.RuntimeFrame) 5fbc82ca690fa08ce51c2452175983eab34987e308b90c7cfaada5057fd4935d -->
<!-- api: M:Inno.Rendering.Runtime.RenderRuntime.OnPrepareOutput(Inno.Runtime.Contracts.RuntimeFrame) 4c50ce63c681c92402db26aec9ff4480ea49bfe4c5382edabfc33508c1ecbeb0 -->
<!-- api: M:Inno.Rendering.Runtime.RenderRuntime.OnProduceOutput(Inno.Runtime.Contracts.RuntimeFrame) fc1a5e6c94e0a456f46fe55d6d4442007ee176bda7322c04ee76e4d260105664 -->
<!-- api: M:Inno.Rendering.Runtime.RenderRuntime.OnCompleteOutput(Inno.Runtime.Contracts.RuntimeFrame) 2faca6b29953ce0c9b49ff4ff161b77a7c7942d464242d51b827ee61913cdb65 -->
<!-- api: M:Inno.Rendering.Runtime.RenderRuntime.SetPrimaryRoute(Inno.Rendering.Runtime.RenderOutputRoute) 2c5d0db54ee476eb0be3271e228c82fd1467bae919309d835bc00e0e48e35dc1 -->
<!-- api: M:Inno.Rendering.Runtime.RenderRuntime.SetPrimaryModelOutputEnabled(System.Boolean) 7016812913ac2ee3301fb383520dafca55973782821b790d3e30dda20dd2bbda -->
<!-- api: M:Inno.Rendering.Runtime.RenderRuntime.TryActivateDefaultPipeline(Inno.Rendering.Assets.RenderPipelineAsset) 33b39dad5ca14348f056edbf71854b83bb4517e4dcf49a9ac599932286179d02 -->
<!-- api: M:Inno.Rendering.Runtime.RenderRuntime.BeginExtensionReload c09ef99babe1d579c765dc4f466dd8f61d603336c9ff653a927d82b1130bb238 -->
<!-- api: M:Inno.Rendering.Runtime.RenderRuntime.Submit(Inno.Rendering.Runtime.RenderRequest) 4c190bb538ae80c3ffcca111284ddf46d7b5760deded6ba3d49efe7dc3bdaf9a -->
<!-- api: M:Inno.Rendering.Runtime.RenderRuntime.SubmitComposition(System.String,Inno.Rendering.RenderTarget,Inno.Rendering.RenderViewport,Inno.Rendering.RenderTextureFormat,System.Collections.Generic.IReadOnlyList{Inno.Rendering.Runtime.RenderRequest},System.Int32) 0962d19146359f83195e6720c681c0616ff53f111a9446efcdd93fe96667ea1f -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c641e35d939ca9dd"></a>[`public Inno.Rendering.Runtime.RenderRuntime.RenderRuntime(Inno.Extensibility.Types.TypeCatalog types, Inno.Rendering.IRenderDevice device, Inno.Core.Diagnostics.IDiagnosticReporter diagnostics, System.Collections.Generic.IEnumerable<Inno.Rendering.IRenderFrameGraphContributor>? contributors = null, Inno.Rendering.Runtime.IRenderTargetArtifactProvider? targetArtifacts = null, System.Func<Inno.References.ContentReadScope>? contentScopeProvider = null, System.Func<Inno.Rendering.RenderPresentationSize, Inno.Rendering.RenderViewport>? primaryPresentationViewportProvider = null, Inno.Rendering.Runtime.RenderResourceLimits? resourceLimits = null, System.Func<Inno.Input.InputSnapshot>? inputSnapshotProvider = null, System.Func<Inno.Rendering.RenderPresentationSize?>? primaryInputSurfaceSizeProvider = null, Inno.Rendering.IRenderLayerCompositionProgramProvider? compositionProgramProvider = null)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.cs#L109) | Creates a render runtime without installing any concrete pipeline.<br>`device`：Active backend-neutral device.<br>`types`：The type catalog that owns render extension generations.<br>`diagnostics`：Owner-bound structured diagnostic producer.<br>`contributors`：Optional frame-final contributors such as an ImGui backend.<br>`targetArtifacts`：Provider of immutable target artifacts. Editor hosts may provide an asynchronous authoring-backed implementation, while Player hosts provide a source-free deployment implementation.<br>`contentScopeProvider`：Optional host callback that supplies explicit current-frame content without coupling Rendering to Scene or documents.<br>`primaryPresentationViewportProvider`：Optional host callback that selects the content region within the primary presentation surface. The complete surface is used when omitted.<br>`resourceLimits`：Finite native cache and asynchronous readback limits; omitted values use the engine defaults.<br>`inputSnapshotProvider`：Optional host callback that reads the completed session input snapshot when output is collected.<br>`primaryInputSurfaceSizeProvider`：Optional logical input extent used for pointer scaling; returns null while input has no surface.<br>`compositionProgramProvider`：Backend-specific program provider used when the host composites multiple render layers. | — | `System.ArgumentException`：The contributor sequence contains a null entry or the same contributor more than once. No rendering generation is registered in this case. |
| <a id="api-b3b09c19db8bc875"></a>[`public Inno.Rendering.Runtime.IRenderRuntimeReloadTransaction Inno.Rendering.Runtime.RenderRuntime.BeginExtensionReload()`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.Pipelines.cs#L55) | Begins an isolated rendering-extension reload transaction at a frame boundary.<br>返回：A transaction that preserves the active generation until its candidate is explicitly completed. | — | `System.InvalidOperationException`：Thrown during an open frame, during retirement, after a terminal generation failure, or while another rendering reload transaction is active. |
| <a id="api-8d666c8af917f3db"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Runtime.ViewContentItem> Inno.Rendering.Runtime.RenderRuntime.Collect(Inno.Rendering.Runtime.ViewContentContext context)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.cs#L182) | Collects frame requests from rendering models and registered providers.<br>返回：An immutable snapshot of the values selected by the operation.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-f0fc85d1e5af23d0"></a>[`public System.IDisposable Inno.Rendering.Runtime.RenderRuntime.EnterExecutionScope()`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.cs#L239) | Binds this rendering runtime to script-facing graphics APIs for the current asynchronous execution flow.<br>返回：A strict last-in-first-out execution scope owned by the caller. | — | `System.ObjectDisposedException`：Thrown when this rendering runtime has been disposed.<br>`System.InvalidOperationException`：Retirement has started or generation cleanup has faulted the owner. |
| <a id="api-34835b59876b433d"></a>[`protected override void Inno.Rendering.Runtime.RenderRuntime.OnBeginFrame(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.Frames.cs#L25) | Captures snapshots and binds service façades.<br>`frame`：The variable frame timing. | — | — |
| <a id="api-342e45b214e6cee5"></a>[`protected override void Inno.Rendering.Runtime.RenderRuntime.OnCompleteOutput(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.Frames.cs#L51) | Submits output and closes output-specific temporary resources.<br>`frame`：The variable frame timing. | — | — |
| <a id="api-9ed44c3a15e153a6"></a>[`protected override void Inno.Rendering.Runtime.RenderRuntime.OnPrepareOutput(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.Frames.cs#L37) | Opens resources required for this frame's output.<br>`frame`：The variable frame timing. | — | — |
| <a id="api-042403c2a388c868"></a>[`protected override void Inno.Rendering.Runtime.RenderRuntime.OnProduceOutput(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.Frames.cs#L44) | Collects output commands without executing managed code on a native realtime callback.<br>`frame`：The variable frame timing. | — | — |
| <a id="api-15720398b37ec1d1"></a>[`protected override void Inno.Rendering.Runtime.RenderRuntime.OnStart()`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.cs#L248) | Attaches this feature to its owning runtime generation. | — | — |
| <a id="api-ec0616ec057f2ba6"></a>[`protected override void Inno.Rendering.Runtime.RenderRuntime.OnStop()`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.cs#L265) | Releases every runtime rendering generation, persistent target, upload, and GPU resource owned by this instance. | — | — |
| <a id="api-88500b9d855ab96e"></a>[`public void Inno.Rendering.Runtime.RenderRuntime.RegisterContributor(Inno.Rendering.IRenderFrameGraphContributor contributor)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.Contributors.cs#L31) | Registers frame-final work without transferring frame ownership.<br>`contributor`：Contributor invoked after all user pipeline requests. | — | `System.ArgumentException`：Thrown when the instance is already registered.<br>`System.InvalidOperationException`：Retirement has started or generation cleanup has faulted the owner. |
| <a id="api-5a1e27ec92db6c14"></a>[`public void Inno.Rendering.Runtime.RenderRuntime.SetPrimaryModelOutputEnabled(bool enabled)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.Models.cs#L40) | Enables or disables model rendering to the host's primary backbuffer. Editor hosts disable this because their Game and Scene sessions own offscreen outputs.<br>`enabled`：Whether the primary backbuffer is a model output. | — | — |
| <a id="api-7115fed28ffea743"></a>[`public void Inno.Rendering.Runtime.RenderRuntime.SetPrimaryRoute(Inno.Rendering.Runtime.RenderOutputRoute? route)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.Models.cs#L25) | Sets the explicit model composition route for the primary output at a frame boundary.<br>`route`：Ordered model identities, or null for automatic single-model selection. | — | — |
| <a id="api-84f7487ac40aa600"></a>[`public void Inno.Rendering.Runtime.RenderRuntime.Submit(Inno.Rendering.Runtime.RenderRequest request)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.Requests.cs#L32) | Submits validated work to the active backend for ordered processing.<br>`request`：The validated immutable request that defines this operation. | — | `System.InvalidOperationException`：The bounded frame queue has reached capacity, retirement has started, or generation cleanup has faulted the owner. The request was not accepted.<br>`System.ObjectDisposedException`：The rendering owner has retired. |
| <a id="api-c8dcef181b81403f"></a>[`public void Inno.Rendering.Runtime.RenderRuntime.SubmitComposition(string name, Inno.Rendering.RenderTarget target, Inno.Rendering.RenderViewport viewport, Inno.Rendering.RenderTextureFormat format, System.Collections.Generic.IReadOnlyList<Inno.Rendering.Runtime.RenderRequest> layers, int priority = 0)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.Requests.cs#L72) | Submits one or more independently rendered model layers for premultiplied-alpha output composition.<br>`name`：Stable frame-local composition identity.<br>`target`：Final host-owned output target.<br>`viewport`：Output viewport in physical pixels.<br>`format`：Shared sampled color format required by every layer.<br>`layers`：Ordered requests, one per rendering model.<br>`priority`：Ascending scheduling priority relative to ordinary requests. | — | `System.ArgumentException`：The layer set or format is invalid.<br>`System.InvalidOperationException`：The bounded frame queue is full or retirement has started. |
| <a id="api-c55d584f98011b12"></a>[`public bool Inno.Rendering.Runtime.RenderRuntime.TryActivateDefaultPipeline(Inno.Rendering.Assets.RenderPipelineAsset pipelineAsset)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.Pipelines.cs#L31) | Validates and selects a project default while preserving its last-good generation.<br>返回：true when a usable generation exists for the candidate.<br>`pipelineAsset`：Candidate project default pipeline asset. | — | `System.InvalidOperationException`：Thrown during an open graphics frame, during retirement, or after generation cleanup faults the owner. |
| <a id="api-2d186cc09ea6bf8d"></a>[`public bool Inno.Rendering.Runtime.RenderRuntime.UnregisterContributor(Inno.Rendering.IRenderFrameGraphContributor contributor)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.Contributors.cs#L53) | Stops invoking a previously registered frame-final contributor.<br>返回：true when registration existed and was removed.<br>`contributor`：Contributor to remove. | — | — |
| <a id="api-a5a239b47e88054e"></a>[`public ulong Inno.Rendering.Runtime.RenderRuntime.currentFrameIndex { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.cs#L171) | Gets the monotonic index of the current or most recently completed output frame. | — | — |
| <a id="api-6eb215d234abc5b1"></a>[`public uint Inno.Rendering.Runtime.RenderRuntime.deviceGeneration { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.cs#L215) | Gets the non-zero rendering-device generation that owns persistent handles. | — | — |
| <a id="api-14db66e3868f3151"></a>[`public Inno.Rendering.Runtime.IRenderResourceService Inno.Rendering.Runtime.RenderRuntime.resources { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.cs#L161) | Gets backend-neutral persistent resource resolution for host-owned previews and rendering integrations. | — | — |
| <a id="api-6d590908fd483622"></a>[`public Inno.Rendering.Runtime.RenderResourceStatistics Inno.Rendering.Runtime.RenderRuntime.resourceStatistics { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.cs#L220) | Gets a detached control-thread snapshot of resource occupancy, high-water marks and rejected admissions. | — | — |
| <a id="api-da2f332616d2ec98"></a>[`public Inno.Rendering.Runtime.RenderTargetStore Inno.Rendering.Runtime.RenderRuntime.targets { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.cs#L156) | Gets persistent offscreen target services for viewport presentation. | — | — |
| <a id="api-2026acdf364a951a"></a>[`public Inno.Rendering.Runtime.IViewContentCollector Inno.Rendering.Runtime.RenderRuntime.viewContent { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.cs#L166) | Gets the generation-scoped collector used by rendering models for world content. | — | — |
| <a id="api-c1bd87a2dbbbc970"></a>[`public sealed class Inno.Rendering.Runtime.RenderRuntime : Inno.Runtime.Contracts.RuntimeSubsystem, Inno.Rendering.Runtime.IRenderRequestSink, Inno.Rendering.Runtime.IViewContentCollector`](../../src/services/rendering/Inno.Rendering.Runtime/RenderFrameScratch.cs#L7) | Owns the sole graphics frame boundary and executes model-neutral render requests. | — | — |

### `Inno.Rendering.Runtime.RenderRuntimeFactory`

<!-- api: T:Inno.Rendering.Runtime.RenderRuntimeFactory 57d7d34de941851b1c06ab4dcd2c16a352c6153783460dc477101e899753c6f2 -->
<!-- api: M:Inno.Rendering.Runtime.RenderRuntimeFactory.#ctor(System.Func{Inno.Runtime.Contracts.RuntimeSubsystemContext,Inno.Rendering.Runtime.RenderRuntime}) 034a631f10d4f771d9f029b0a657b497693c9d5e23c6f0d038c3cfff846581fa -->
<!-- api: P:Inno.Rendering.Runtime.RenderRuntimeFactory.descriptor fda304ec4f1abcc0081e92e1704b7b2ef33eacb2af745efe524471e47f5755b8 -->
<!-- api: M:Inno.Rendering.Runtime.RenderRuntimeFactory.Create(Inno.Runtime.Contracts.RuntimeSubsystemContext) 835f7138bd9b60e8960b5fe1a59bab42563ebd25b1054f50cdd405cb53750ad3 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-5a3724bc3e36248c"></a>[`public Inno.Rendering.Runtime.RenderRuntimeFactory.RenderRuntimeFactory(System.Func<Inno.Runtime.Contracts.RuntimeSubsystemContext, Inno.Rendering.Runtime.RenderRuntime> runtimeFactory)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntimeFactory.cs#L21) | Creates a reusable factory around a composition-owned rendering runtime callback.<br>`runtimeFactory`：The callback that creates a distinct rendering runtime for the supplied session. | — | — |
| <a id="api-4736e6265b7ae34b"></a>[`public Inno.Runtime.Contracts.IRuntimeSubsystem Inno.Rendering.Runtime.RenderRuntimeFactory.Create(Inno.Runtime.Contracts.RuntimeSubsystemContext context)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntimeFactory.cs#L43) | Creates a rendering feature over a newly allocated runtime layer.<br>返回：A new feature that exclusively owns its rendering runtime layer.<br>`context`：The isolated session context passed to the rendering runtime factory. | — | — |
| <a id="api-4922f7ff8022b836"></a>[`public Inno.Runtime.Contracts.RuntimeSubsystemDescriptor Inno.Rendering.Runtime.RenderRuntimeFactory.descriptor { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntimeFactory.cs#L29) | Gets stable ordering metadata that places rendering after simulation features. | — | — |
| <a id="api-cbad366b06cfa760"></a>[`public sealed class Inno.Rendering.Runtime.RenderRuntimeFactory : Inno.Runtime.Contracts.IRuntimeSubsystemFactory`](../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntimeFactory.cs#L11) | Creates one rendering lifecycle feature for every isolated runtime session. | — | — |

### `Inno.Rendering.Runtime.RenderTargetArtifactStatus`

<!-- api: T:Inno.Rendering.Runtime.RenderTargetArtifactStatus 23622e3c5ad4efb340e5ce609c458a2eaef0392d6a525edc362a4740687e3125 -->
<!-- api: F:Inno.Rendering.Runtime.RenderTargetArtifactStatus.Ready 2f524e5f2d190ff24a474b8eb25920a71151a9cc4fc615500e0e27a98d4c5717 -->
<!-- api: F:Inno.Rendering.Runtime.RenderTargetArtifactStatus.Pending 8d45a06e69b82b7fd1e829328fdc08d87fe48498ca121e9bb8ef0ef3b3fbd5f9 -->
<!-- api: F:Inno.Rendering.Runtime.RenderTargetArtifactStatus.Unavailable 6de3641e6d501e9c8a83301b7c8d8e31ee177d47921da42ad46cb94469701e53 -->
<!-- api: F:Inno.Rendering.Runtime.RenderTargetArtifactStatus.Failed 16e0c83a6c0f9cf7f2ec950eef3a9b6c1793f93679e7dc0d704dfeeebd3c7484 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9b90962777ba6215"></a>[`Inno.Rendering.Runtime.RenderTargetArtifactStatus.Failed = 3`](../../src/services/rendering/Inno.Rendering.Runtime/Deployment/RenderTargetArtifactStatus.cs#L29) | Artifact production completed unsuccessfully and published a specific diagnostic. | — | — |
| <a id="api-c0341c9097a4e9f9"></a>[`Inno.Rendering.Runtime.RenderTargetArtifactStatus.Pending = 1`](../../src/services/rendering/Inno.Rendering.Runtime/Deployment/RenderTargetArtifactStatus.cs#L19) | Artifact production is still running and no usable artifact is available yet. | — | — |
| <a id="api-2edff6c2e50f0d3c"></a>[`Inno.Rendering.Runtime.RenderTargetArtifactStatus.Ready = 0`](../../src/services/rendering/Inno.Rendering.Runtime/Deployment/RenderTargetArtifactStatus.cs#L14) | A validated artifact is available for immediate use. | — | — |
| <a id="api-318c29d3a30b1530"></a>[`Inno.Rendering.Runtime.RenderTargetArtifactStatus.Unavailable = 2`](../../src/services/rendering/Inno.Rendering.Runtime/Deployment/RenderTargetArtifactStatus.cs#L24) | The active deployment does not contain the requested artifact. | — | — |
| <a id="api-d1080169fb72c7e7"></a>[`public enum Inno.Rendering.Runtime.RenderTargetArtifactStatus : int`](../../src/services/rendering/Inno.Rendering.Runtime/Deployment/RenderTargetArtifactStatus.cs#L9) | Describes the current availability of one target-specific rendering artifact. | — | — |

### `Inno.Rendering.Runtime.RenderTargetStore`

<!-- api: T:Inno.Rendering.Runtime.RenderTargetStore 317b0463dce893260c89f98ea8e20b094b71c85d1dff131a7f0dfbedc42ba18a -->
<!-- api: M:Inno.Rendering.Runtime.RenderTargetStore.#ctor(Inno.Rendering.IRenderDevice,System.Int32) 2c8086c603fcbe4ced395d1a92a8b409c96aebcea81cf851c2b9757705c2161b -->
<!-- api: P:Inno.Rendering.Runtime.RenderTargetStore.count 9ca49baf4b10a7daf598cb7d7e21adb757f981458a0c038b4998fd66bb7bb1b8 -->
<!-- api: P:Inno.Rendering.Runtime.RenderTargetStore.rejectedCount f66414327958cba1e0bc73628ee9c7d623d228e633b3d49d5847d6200a7aa8f3 -->
<!-- api: M:Inno.Rendering.Runtime.RenderTargetStore.Import(Inno.Rendering.RenderGraphBuilder,Inno.Rendering.RenderTexture) f47008fce4f387b9bdc1f1992d6034faa91ea1ae0d27f6d67eec375aa70335a5 -->
<!-- api: M:Inno.Rendering.Runtime.RenderTargetStore.TryGetTexture(Inno.Rendering.RenderTexture,Inno.Rendering.PersistentTextureHandle@) 78869be08b353b8e9090d1535dfd7cc489f72261fd8835e01d90576b653b6893 -->
<!-- api: M:Inno.Rendering.Runtime.RenderTargetStore.Release(Inno.Rendering.RenderTexture) d04b28669de4aea989704e5e56915b83b7fc639db57e3b469dfc7f244c7112b5 -->
<!-- api: M:Inno.Rendering.Runtime.RenderTargetStore.PrepareFrame ad0efa832762d55c2a49f0f0b07ca42e2eba216eb29eec3f3c7c3b2ece2e5a47 -->
<!-- api: M:Inno.Rendering.Runtime.RenderTargetStore.Dispose 6931cbda2dbc91d638c25dafa2182a2b3b0123e5721a48de82035c9b3816f88c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c92b686c6e8772df"></a>[`public Inno.Rendering.Runtime.RenderTargetStore.RenderTargetStore(Inno.Rendering.IRenderDevice device, int capacity = 1024)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderTargetStore.cs#L34) | Creates a target registry for one device generation.<br>`device`：Device that owns persistent target resources.<br>`capacity`：Positive target and per-frame allocation capacity. | — | — |
| <a id="api-083a1bf90329cf55"></a>[`public void Inno.Rendering.Runtime.RenderTargetStore.Dispose()`](../../src/services/rendering/Inno.Rendering.Runtime/RenderTargetStore.cs#L203) | Queues all owned resources for device-safe destruction. | — | `Inno.Core.Execution.RetirementPendingException`：A backend resource is still active. Remaining resources stay owned until disposal is retried.<br>`System.AggregateException`：Quiescent resources were all attempted, but one or more backend releases failed. |
| <a id="api-84a2a58d05c5ef86"></a>[`public Inno.Rendering.RenderTextureHandle Inno.Rendering.Runtime.RenderTargetStore.Import(Inno.Rendering.RenderGraphBuilder graph, Inno.Rendering.RenderTexture target)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderTargetStore.cs#L65) | Imports or creates one target in the current frame graph.<br>返回：A graph-scoped handle for the current target resource.<br>`graph`：Current frame graph builder.<br>`target`：Persistent target description. | — | — |
| <a id="api-44a82307fe237591"></a>[`public void Inno.Rendering.Runtime.RenderTargetStore.PrepareFrame()`](../../src/services/rendering/Inno.Rendering.Runtime/RenderTargetStore.cs#L158) | Advances queued target releases at a frame safety point. | Retired textures remain active across one complete submitted frame so a prepared UI or presentation packet cannot observe an invalidated handle. | — |
| <a id="api-e267099c62bf8574"></a>[`public void Inno.Rendering.Runtime.RenderTargetStore.Release(Inno.Rendering.RenderTexture target)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderTargetStore.cs#L143) | Queues one target for frame-safe retirement.<br>`target`：Target no longer used by request producers. | — | — |
| <a id="api-0c27716c6e85106c"></a>[`public bool Inno.Rendering.Runtime.RenderTargetStore.TryGetTexture(Inno.Rendering.RenderTexture target, out Inno.Rendering.PersistentTextureHandle texture)`](../../src/services/rendering/Inno.Rendering.Runtime/RenderTargetStore.cs#L106) | Tries to get the current opaque device texture for UI presentation.<br>返回：true after a graph write to the current target revision has been recorded successfully.<br>`target`：Persistent target description.<br>`texture`：Receives a backend-neutral persistent handle. | — | — |
| <a id="api-c398e628954ea505"></a>[`public int Inno.Rendering.Runtime.RenderTargetStore.count { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderTargetStore.cs#L46) | Gets the number of retained offscreen targets. | — | — |
| <a id="api-638c2adb8c74819d"></a>[`public long Inno.Rendering.Runtime.RenderTargetStore.rejectedCount { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/RenderTargetStore.cs#L51) | Gets target admissions rejected by finite capacity. | — | — |
| <a id="api-b53387e4a9e38c9e"></a>[`public sealed class Inno.Rendering.Runtime.RenderTargetStore : System.IDisposable`](../../src/services/rendering/Inno.Rendering.Runtime/RenderTargetStore.cs#L11) | Owns persistent offscreen targets without exposing backend-native handles. | — | — |

### `Inno.Rendering.Runtime.RenderTextureSubresourceData`

<!-- api: T:Inno.Rendering.Runtime.RenderTextureSubresourceData d4a71d2a84854ceca0d98e60a59c6baec2fcceb869d36a37c11520ea1616fab2 -->
<!-- api: M:Inno.Rendering.Runtime.RenderTextureSubresourceData.#ctor(System.Int32,System.Int32,System.ReadOnlySpan{System.Byte}) 6da43f3890c68ee98eaf8df10a409e35f9d0a6a4dede6c64ce9c14908678e4d3 -->
<!-- api: P:Inno.Rendering.Runtime.RenderTextureSubresourceData.mipLevel 358a37f7b92839e1bf06bc3d70129c9a2edd3997ccc3e5009b6caa1effef70b0 -->
<!-- api: P:Inno.Rendering.Runtime.RenderTextureSubresourceData.arrayLayer 7f49c72fae3d94a2c82dbf0688dd066caa942b17bb19589e09094bcca3966427 -->
<!-- api: P:Inno.Rendering.Runtime.RenderTextureSubresourceData.data 8d721f74e4c84e9264719d06ddacb68d7062d225c49041f3d8dc6f09df1078b1 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-55bdeb7924fb0c8a"></a>[`public Inno.Rendering.Runtime.RenderTextureSubresourceData.RenderTextureSubresourceData(int mipLevel, int arrayLayer, System.ReadOnlySpan<byte> data)`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderTextureSubresourceData.cs#L32) | Creates one immutable texture subresource upload.<br>`mipLevel`：Zero-based mip level.<br>`arrayLayer`：Zero-based 2D array layer, 3D Z slice, or flattened cubemap face using cube-layer * 6 + face.<br>`data`：Tightly packed complete subresource bytes. | — | — |
| <a id="api-9aff4a038c9fa885"></a>[`public int Inno.Rendering.Runtime.RenderTextureSubresourceData.arrayLayer { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderTextureSubresourceData.cs#L54) | Gets the zero-based array layer, volume slice, or flattened cubemap face. | — | — |
| <a id="api-20cce4789c7c4d52"></a>[`public System.ReadOnlyMemory<byte> Inno.Rendering.Runtime.RenderTextureSubresourceData.data { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderTextureSubresourceData.cs#L59) | Gets immutable tightly packed bytes. | — | — |
| <a id="api-884e970c7630b973"></a>[`public int Inno.Rendering.Runtime.RenderTextureSubresourceData.mipLevel { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderTextureSubresourceData.cs#L49) | Gets the zero-based mip level. | — | — |
| <a id="api-5f9e766b3ea3638f"></a>[`public sealed class Inno.Rendering.Runtime.RenderTextureSubresourceData`](../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderTextureSubresourceData.cs#L16) | Stores one complete texture mip and addressable layer, slice, or cubemap-face upload. | — | — |

### `Inno.Rendering.Runtime.RenderView`

<!-- api: T:Inno.Rendering.Runtime.RenderView b7fb19c63e365f0a514f721762c2ed3a67c47304ea5d111f21e81b897e11dec2 -->
<!-- api: M:Inno.Rendering.Runtime.RenderView.#ctor(System.String,Inno.Rendering.RenderViewport,Inno.Core.Mathematics.Matrix,Inno.Core.Mathematics.Matrix,System.UInt64) 22106a9cfd994a18cbe152ac384e0617917c0c1693e7088c94145d632eba1040 -->
<!-- api: P:Inno.Rendering.Runtime.RenderView.id 776c59224c887f4c7c1938844b90c290ac170e6723042f716189c30968c54009 -->
<!-- api: P:Inno.Rendering.Runtime.RenderView.viewport c4ca38d0a47df10e64b61a338439295f2b72bcfdbe94a045eb20729cec20872c -->
<!-- api: P:Inno.Rendering.Runtime.RenderView.viewMatrix 6302e5a3235dab069e616458b54555d441011adf66f83f26a4a5ea33727619b6 -->
<!-- api: P:Inno.Rendering.Runtime.RenderView.projectionMatrix 284782ddbaa865b94aef2a5a90b72cfe7afd29062f7339af6754372c073e1afa -->
<!-- api: P:Inno.Rendering.Runtime.RenderView.visibilityMask ccef4772d547c5083efb27bbcb90e9afc724947ea5d60d70ead66086b430b732 -->
<!-- api: M:Inno.Rendering.Runtime.RenderView.ToString 6087c2615773f6ac8d5494042de4b85880c7ee946aaa9c7c1cc4f54bdaa3b0a0 -->
<!-- api: M:Inno.Rendering.Runtime.RenderView.op_Inequality(Inno.Rendering.Runtime.RenderView,Inno.Rendering.Runtime.RenderView) c95115ab0dd9f96a9f2fbb584aba2643cae97d3ff6f4dc8bbfe5d482c3649340 -->
<!-- api: M:Inno.Rendering.Runtime.RenderView.op_Equality(Inno.Rendering.Runtime.RenderView,Inno.Rendering.Runtime.RenderView) 5dda01c46826d964805b7a16d1b6c5032214d521054b6016849f3bc53b4dbdc3 -->
<!-- api: M:Inno.Rendering.Runtime.RenderView.GetHashCode 944275da68bb451b35eaa5db34a7a201bf53e94456c77257cfe61fd4d6435d3b -->
<!-- api: M:Inno.Rendering.Runtime.RenderView.Equals(System.Object) 8e027a855b912a377e10c352a152bd5b1caa43e0578fa201e54f8f5739ea737a -->
<!-- api: M:Inno.Rendering.Runtime.RenderView.Equals(Inno.Rendering.Runtime.RenderView) 054772bf545e571023deb43e58a680668dc500958a42307e4f7e2e6a4477a166 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-93069f90a3a5442b"></a>[`public Inno.Rendering.Runtime.RenderView.RenderView(string id, Inno.Rendering.RenderViewport viewport, Inno.Core.Mathematics.Matrix viewMatrix, Inno.Core.Mathematics.Matrix projectionMatrix, ulong visibilityMask = 18446744073709551615)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L34) | Creates the exact view used to render content into a viewport.<br>`id`：Stable identity within the current output session.<br>`viewport`：Destination pixel rectangle.<br>`viewMatrix`：World-to-view transform.<br>`projectionMatrix`：View-to-clip transform.<br>`visibilityMask`：Model-defined visible content bits. | — | — |
| <a id="api-a0e0ee13cd4918aa"></a>[`public bool Inno.Rendering.Runtime.RenderView.Equals(Inno.Rendering.Runtime.RenderView other)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L14) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-d1d2780580da5857"></a>[`public override bool Inno.Rendering.Runtime.RenderView.Equals(object obj)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L14) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-19973bf30936f41b"></a>[`public override int Inno.Rendering.Runtime.RenderView.GetHashCode()`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L14) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-356afd7c867f3f50"></a>[`public static bool Inno.Rendering.Runtime.RenderView.operator ==(Inno.Rendering.Runtime.RenderView left, Inno.Rendering.Runtime.RenderView right)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L14) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-15c6d37c96772f7d"></a>[`public static bool Inno.Rendering.Runtime.RenderView.operator !=(Inno.Rendering.Runtime.RenderView left, Inno.Rendering.Runtime.RenderView right)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L14) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-671b73d368168a14"></a>[`public override string Inno.Rendering.Runtime.RenderView.ToString()`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L14) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-57c95fd439c66545"></a>[`public string Inno.Rendering.Runtime.RenderView.id { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L52) | Gets the view identity within its output session. | — | — |
| <a id="api-216dcfb2e5193033"></a>[`public Inno.Core.Mathematics.Matrix Inno.Rendering.Runtime.RenderView.projectionMatrix { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L67) | Gets the view-to-clip transform. | — | — |
| <a id="api-84814aa928a264fe"></a>[`public Inno.Core.Mathematics.Matrix Inno.Rendering.Runtime.RenderView.viewMatrix { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L62) | Gets the world-to-view transform. | — | — |
| <a id="api-650923d3718f02f3"></a>[`public Inno.Rendering.RenderViewport Inno.Rendering.Runtime.RenderView.viewport { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L57) | Gets the destination pixel rectangle. | — | — |
| <a id="api-10683a23a288c896"></a>[`public ulong Inno.Rendering.Runtime.RenderView.visibilityMask { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L72) | Gets the model-defined visible content bits. | — | — |
| <a id="api-b63529ca165e3d1b"></a>[`public readonly record struct Inno.Rendering.Runtime.RenderView : System.IEquatable<Inno.Rendering.Runtime.RenderView>`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L14) | Describes one backend-neutral view produced by a rendering model. | — | — |

### `Inno.Rendering.Runtime.SelectedViewContentCollector`

<!-- api: T:Inno.Rendering.Runtime.SelectedViewContentCollector 8961e8ce6ce579eff11a94f6e42c23d78036b36e835ee211ac8abdc62646bade -->
<!-- api: M:Inno.Rendering.Runtime.SelectedViewContentCollector.#ctor(Inno.Rendering.Runtime.IViewContentCollector,System.Collections.Generic.IReadOnlyList{System.String}) a0afcf7036fb256505688dbae265c847485597fce714fe47e3db09f068c5fdb0 -->
<!-- api: M:Inno.Rendering.Runtime.SelectedViewContentCollector.Collect(Inno.Rendering.Runtime.ViewContentContext) 566f03d8beda70710837ea2b547b0b99a4c788d2897aea2750597bb1a16ea1f7 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-76560d5e098102e3"></a>[`public Inno.Rendering.Runtime.SelectedViewContentCollector.SelectedViewContentCollector(Inno.Rendering.Runtime.IViewContentCollector inner, System.Collections.Generic.IReadOnlyList<string> sourceIds)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L460) | Creates a collector that forwards only the selected source identities.<br>`inner`：Active generation's source collector.<br>`sourceIds`：Source IDs exclusively assigned to this layer. | — | — |
| <a id="api-3c932a1c15f37f40"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Runtime.ViewContentItem> Inno.Rendering.Runtime.SelectedViewContentCollector.Collect(Inno.Rendering.Runtime.ViewContentContext context)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L477) | Collects only the assigned world-content sources for one exact view.<br>返回：Frame-local content emitted by the assigned sources.<br>`context`：The original model view, output input, and frame timing. | — | — |
| <a id="api-8a473b6fd2e539be"></a>[`public sealed class Inno.Rendering.Runtime.SelectedViewContentCollector : Inno.Rendering.Runtime.IViewContentCollector`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L446) | Restricts a neutral world-content collector to an output layer's source IDs. | — | — |

### `Inno.Rendering.Runtime.ViewContentContext`

<!-- api: T:Inno.Rendering.Runtime.ViewContentContext 55eaf05596344346da2c76387283487831b765e18ab745e2504feb0df4eb95eb -->
<!-- api: M:Inno.Rendering.Runtime.ViewContentContext.#ctor(Inno.References.ContentReadScope,System.String,Inno.Rendering.Runtime.RenderView,System.UInt64,System.Single,Inno.Rendering.Runtime.RenderOutputInput,System.Collections.Generic.IReadOnlyList{Inno.Rendering.Runtime.RenderView},System.Collections.Generic.IReadOnlyList{System.String}) 8cf5f1294955519dac9752c1cad410cfb87726b8f2d12e55f01c19244bb93542 -->
<!-- api: P:Inno.Rendering.Runtime.ViewContentContext.content 601d31315abbe5951a9c8641facd488b24aabdc22d6afb302e93b0c3d861e647 -->
<!-- api: P:Inno.Rendering.Runtime.ViewContentContext.sessionId 32a8fe2199ae7c38a63cc0cfb75528a5ebaf78ba6a963bb8355df1599d4745cb -->
<!-- api: P:Inno.Rendering.Runtime.ViewContentContext.view 454dd72d5e72feb0f27b31a4a0c902e8c5f3ccdbf1ccfbcf0be192eede6b31ab -->
<!-- api: P:Inno.Rendering.Runtime.ViewContentContext.frameIndex 6a359ed29f921313d12fa6b449dac1f821ea8a6eab7db461ef3bfad5c050f696 -->
<!-- api: P:Inno.Rendering.Runtime.ViewContentContext.deltaTime c52f441746ead71e79fd065cf38442d2cfbd22c1aa4621de62750d76163b1af7 -->
<!-- api: P:Inno.Rendering.Runtime.ViewContentContext.input 1223a9340d3a11d32e22430f7e0b9855e6be5dce4502a4c3eb90e3f53604bf8c -->
<!-- api: P:Inno.Rendering.Runtime.ViewContentContext.views 58a55d4136d7ede2290f098d30d9b112b6fc32ede116c4fd4cc61a27391a708a -->
<!-- api: P:Inno.Rendering.Runtime.ViewContentContext.sourceIds 7b3caaaec54f0289941fe67167beaaab2c0773bee36985d87561d6082148cb32 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-865a93a3386ec9db"></a>[`public Inno.Rendering.Runtime.ViewContentContext.ViewContentContext(Inno.References.ContentReadScope content, string sessionId, Inno.Rendering.Runtime.RenderView view, ulong frameIndex, float deltaTime, Inno.Rendering.Runtime.RenderOutputInput? input = null, System.Collections.Generic.IReadOnlyList<Inno.Rendering.Runtime.RenderView>? views = null, System.Collections.Generic.IReadOnlyList<string>? sourceIds = null)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L108) | Creates a frame-scoped content collection context.<br>`content`：Ordered roots selected by the host.<br>`sessionId`：Stable identity of the host output session.<br>`view`：Exact view that will consume the content.<br>`frameIndex`：Monotonic output frame number.<br>`deltaTime`：Elapsed frame time in seconds.<br>`input`：Pointer and keyboard input in output pixel coordinates.<br>`views`：All views in the same model output, used to choose shared content resolution.<br>`sourceIds`：Optional explicit source allowlist for a routed model layer. | — | — |
| <a id="api-4d755eba9e22a916"></a>[`public Inno.References.ContentReadScope Inno.Rendering.Runtime.ViewContentContext.content { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L132) | Gets the ordered host-selected content roots. | — | — |
| <a id="api-b2fc297d2b06c350"></a>[`public float Inno.Rendering.Runtime.ViewContentContext.deltaTime { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L152) | Gets elapsed time in seconds. | — | — |
| <a id="api-949f614f8b32758d"></a>[`public ulong Inno.Rendering.Runtime.ViewContentContext.frameIndex { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L147) | Gets the monotonic output frame number. | — | — |
| <a id="api-ad3e3ac5464e6741"></a>[`public Inno.Rendering.Runtime.RenderOutputInput Inno.Rendering.Runtime.ViewContentContext.input { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L156) | Gets frame-local input for this output. | — | — |
| <a id="api-052ff0311605b8d2"></a>[`public string Inno.Rendering.Runtime.ViewContentContext.sessionId { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L137) | Gets the stable host output session identity. | — | — |
| <a id="api-1aa84809d6f504e0"></a>[`public System.Collections.Generic.IReadOnlyList<string>? Inno.Rendering.Runtime.ViewContentContext.sourceIds { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L164) | Gets the exclusive source allowlist; null collects every active source. | — | — |
| <a id="api-c37d4f18b658bd22"></a>[`public Inno.Rendering.Runtime.RenderView Inno.Rendering.Runtime.ViewContentContext.view { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L142) | Gets the exact destination view. | — | — |
| <a id="api-c55b1ae1d53515cb"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Runtime.RenderView> Inno.Rendering.Runtime.ViewContentContext.views { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L160) | Gets every view in the same model output. | — | — |
| <a id="api-2b0cd5668333190a"></a>[`public sealed class Inno.Rendering.Runtime.ViewContentContext`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L79) | Supplies one content source with the host-selected scene roots and an exact view. | — | — |

### `Inno.Rendering.Runtime.ViewContentItem`

<!-- api: T:Inno.Rendering.Runtime.ViewContentItem a3a9a8956f8940c1eb222a1167f45a0d32ffec9ba990acd25406336997dc6e43 -->
<!-- api: M:Inno.Rendering.Runtime.ViewContentItem.#ctor(Inno.Core.Identity.Identity,Inno.Core.Mathematics.Matrix,Inno.Core.Mathematics.Vector3,Inno.Core.Mathematics.Vector3,Inno.Rendering.Runtime.IViewDrawable,Inno.Rendering.Runtime.IViewPointerTarget) 6413da0c8a102d2695fa8f2293deb86af5659ddf755455af84085443e4df5a5a -->
<!-- api: P:Inno.Rendering.Runtime.ViewContentItem.owner 9bb919b12bb0574883d4f5e28cb062bfa2d572455115c9e808575bf6c25cbc23 -->
<!-- api: P:Inno.Rendering.Runtime.ViewContentItem.localToWorld 82cfa172ec4b220077c586de5f8a26d216437ea635382e93c66d836fa4e53489 -->
<!-- api: P:Inno.Rendering.Runtime.ViewContentItem.localBoundsMin e15a84fe028b27338940c7bf0b5b7855d2fe69e9861b7dfa878d0187731c5070 -->
<!-- api: P:Inno.Rendering.Runtime.ViewContentItem.localBoundsMax 659ef84aef209925dc0846358d8a1b8a1a3bcbe8be3e1d0b84d5cac69655c3ae -->
<!-- api: P:Inno.Rendering.Runtime.ViewContentItem.drawable c890510ca8c2a09ba7f7b600a5ed8891307bda01df5dc84304a783e1fe5c420a -->
<!-- api: P:Inno.Rendering.Runtime.ViewContentItem.pointerTarget 6130582157268055b8b9cda6c25cd7881ad43361396aff0b8207981ef98ba326 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-eb0a61e9c6e61b5c"></a>[`public Inno.Rendering.Runtime.ViewContentItem.ViewContentItem(Inno.Core.Identity.Identity owner, Inno.Core.Mathematics.Matrix localToWorld, Inno.Core.Mathematics.Vector3 localBoundsMin, Inno.Core.Mathematics.Vector3 localBoundsMax, Inno.Rendering.Runtime.IViewDrawable drawable, Inno.Rendering.Runtime.IViewPointerTarget? pointerTarget = null)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L302) | Creates an item whose lifetime is limited to the current frame.<br>`owner`：Scene object identity used for model-owned sorting and visibility.<br>`localToWorld`：Exact transform of its local geometry.<br>`localBoundsMin`：Local minimum corner for conservative culling.<br>`localBoundsMax`：Local maximum corner for conservative culling.<br>`drawable`：Drawable inserted into the model's scene pass.<br>`pointerTarget`：Optional input target routed by the rendering model. | — | — |
| <a id="api-e24def516f41774b"></a>[`public Inno.Rendering.Runtime.IViewDrawable Inno.Rendering.Runtime.ViewContentItem.drawable { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L341) | Gets the drawable encoded by the selected rendering model. | — | — |
| <a id="api-ec13f21926eb114b"></a>[`public Inno.Core.Mathematics.Vector3 Inno.Rendering.Runtime.ViewContentItem.localBoundsMax { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L336) | Gets the local maximum corner. | — | — |
| <a id="api-8cb5dea6ec572dcb"></a>[`public Inno.Core.Mathematics.Vector3 Inno.Rendering.Runtime.ViewContentItem.localBoundsMin { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L331) | Gets the local minimum corner. | — | — |
| <a id="api-a490c5530c6f6052"></a>[`public Inno.Core.Mathematics.Matrix Inno.Rendering.Runtime.ViewContentItem.localToWorld { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L326) | Gets the local-to-world transform. | — | — |
| <a id="api-5b1f896629d89a01"></a>[`public Inno.Core.Identity.Identity Inno.Rendering.Runtime.ViewContentItem.owner { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L321) | Gets the owner identity for model-owned sorting and visibility. | — | — |
| <a id="api-4b8d337a665818cd"></a>[`public Inno.Rendering.Runtime.IViewPointerTarget? Inno.Rendering.Runtime.ViewContentItem.pointerTarget { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L345) | Gets the optional pointer target associated with this draw item. | — | — |
| <a id="api-024cc1bbc9548a86"></a>[`public sealed class Inno.Rendering.Runtime.ViewContentItem`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L279) | One world item supplied to a rendering model without choosing its sort policy. | — | — |

### `Inno.Rendering.Runtime.ViewContentSourceExtensionAttribute`

<!-- api: T:Inno.Rendering.Runtime.ViewContentSourceExtensionAttribute 115fe6b8f32354b892e6d25de6031150f554984cce0ab1971192d7fa4ab06046 -->
<!-- api: M:Inno.Rendering.Runtime.ViewContentSourceExtensionAttribute.#ctor(System.String) 46beb858171de4abfd3eb3447b2204b172adc42370f2a0e6f82a3a0298bb75f8 -->
<!-- api: P:Inno.Rendering.Runtime.ViewContentSourceExtensionAttribute.id 7fde74b5f85cf1ef8a01965f478105615c912262d32999c4db6c2811996eee03 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b98e550ea2ac1ca4"></a>[`public Inno.Rendering.Runtime.ViewContentSourceExtensionAttribute.ViewContentSourceExtensionAttribute(string id)`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L412) | Creates a source declaration with a stable identifier.<br>`id`：Globally stable source identifier. | — | — |
| <a id="api-95121548bb51621f"></a>[`public string Inno.Rendering.Runtime.ViewContentSourceExtensionAttribute.id { get; }`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L421) | Gets the globally stable source identifier. | — | — |
| <a id="api-5a3b0d303345aaf0"></a>[`public sealed class Inno.Rendering.Runtime.ViewContentSourceExtensionAttribute : System.Attribute`](../../src/services/rendering/Inno.Rendering.Runtime/Models/ViewContent.cs#L404) | Discovers an independently reloadable world-content source. | — | — |

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
