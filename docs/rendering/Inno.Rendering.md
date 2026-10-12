# Inno.Rendering

[分类索引](README.md) · [Wiki 首页](../README.md)

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

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Rendering.CompiledRenderAttachment`

<!-- api: T:Inno.Rendering.CompiledRenderAttachment 8d8b5e99354188dd216ff34b210b939e8857b8d619af551de7fb2ff6b55ca8be -->
<!-- api: P:Inno.Rendering.CompiledRenderAttachment.texture 2e036b8c1425f49fb40c531bcf9cca234e027af3d42561481d79de4e307568f5 -->
<!-- api: P:Inno.Rendering.CompiledRenderAttachment.slot 53470e42464e815d89abcc80dafd6219f64ecb53e73a0b6db74d01b1869bf526 -->
<!-- api: P:Inno.Rendering.CompiledRenderAttachment.isDepth 49fff694d6b0d2780f29d185428d4fc188486d0345ec0a535e060a5cef269657 -->
<!-- api: P:Inno.Rendering.CompiledRenderAttachment.mipLevel 9ed3c2921d88588a5791a79ca577c72e6f16a262fc19962a91798222abdcd5d1 -->
<!-- api: P:Inno.Rendering.CompiledRenderAttachment.arrayLayer e2a0cbe8c5dff99fdbf00ce339352ae8382c42c5a2016ed8853102afe937cc88 -->
<!-- api: P:Inno.Rendering.CompiledRenderAttachment.loadAction 2bbcd31fd33bdf1ed2d89fda0d0bac8481da832d501b8c74d0ac513603ee6ae8 -->
<!-- api: P:Inno.Rendering.CompiledRenderAttachment.storeAction 57cfd6d334909fc65d6bed12e93471110717cd3531101d0da03a2a5caac37931 -->
<!-- api: P:Inno.Rendering.CompiledRenderAttachment.clearColor 44ff3496437ab3d81be9959236d0668536a8dba60a7dd8843491e935e94fd42b -->
<!-- api: P:Inno.Rendering.CompiledRenderAttachment.clearDepth 090888b6c186047cbca31ababe2d434751f2e3374c6871afd890a4317a9991bf -->
<!-- api: P:Inno.Rendering.CompiledRenderAttachment.clearStencil ff51f4f2b13bc550fe030e29f7eae3adca64fdb5968d7b92a19fea0799d83c22 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-ed5f5947bd756839"></a>[`public int Inno.Rendering.CompiledRenderAttachment.arrayLayer { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L203) | Gets the attached texture-array layer. | — | — |
| <a id="api-a6d8ee7d94cd60ae"></a>[`public Inno.Rendering.RenderClearColor Inno.Rendering.CompiledRenderAttachment.clearColor { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L218) | Gets the linear color clear value. | — | — |
| <a id="api-1c1f9dc546b5184a"></a>[`public float Inno.Rendering.CompiledRenderAttachment.clearDepth { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L223) | Gets the depth clear value. | — | — |
| <a id="api-f8d9aec0e5bf73f9"></a>[`public byte Inno.Rendering.CompiledRenderAttachment.clearStencil { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L228) | Gets the stencil clear value. | — | — |
| <a id="api-7dfc0075b93a49c2"></a>[`public bool Inno.Rendering.CompiledRenderAttachment.isDepth { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L193) | Gets whether this is a depth or depth-stencil attachment. | — | — |
| <a id="api-21d8dc84c9dc30a1"></a>[`public Inno.Rendering.RenderLoadAction Inno.Rendering.CompiledRenderAttachment.loadAction { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L208) | Gets initial content behavior. | — | — |
| <a id="api-e018a1101eb788cf"></a>[`public int Inno.Rendering.CompiledRenderAttachment.mipLevel { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L198) | Gets the attached mip level. | — | — |
| <a id="api-532998dc7f536551"></a>[`public int Inno.Rendering.CompiledRenderAttachment.slot { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L188) | Gets the zero-based color slot, or zero for a depth attachment. | — | — |
| <a id="api-753de37e498e4066"></a>[`public Inno.Rendering.RenderStoreAction Inno.Rendering.CompiledRenderAttachment.storeAction { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L213) | Gets final content behavior. | — | — |
| <a id="api-8c8de1157ffc8f03"></a>[`public Inno.Rendering.RenderTextureHandle Inno.Rendering.CompiledRenderAttachment.texture { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L183) | Gets the attached graph texture. | — | — |
| <a id="api-13dbca035df09e04"></a>[`public sealed class Inno.Rendering.CompiledRenderAttachment`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L164) | Describes one compiled raster attachment. | — | — |

### `Inno.Rendering.CompiledRenderBuffer`

<!-- api: T:Inno.Rendering.CompiledRenderBuffer 226fd3d3d18ebdf0b26e09b580c91675556bc03dd06b5129892eab48d4731b52 -->
<!-- api: P:Inno.Rendering.CompiledRenderBuffer.handle ff8be89d62863392af0d4c102a9de3d3869f42f908d60f05f6dadde876c0025b -->
<!-- api: P:Inno.Rendering.CompiledRenderBuffer.name b5119e15fbaddd97353be23343b5d5133f092253458539b4aa7dd363bdb2f4c9 -->
<!-- api: P:Inno.Rendering.CompiledRenderBuffer.descriptor 77e95131684a59382ec4d4dd8094c7dc9c3dc359591ce695367f71155c78f628 -->
<!-- api: P:Inno.Rendering.CompiledRenderBuffer.imported 8ae54d205c6a15f9f5e5ff6f978d649ea1c300c7d1a835e23bafa65f8d6af262 -->
<!-- api: P:Inno.Rendering.CompiledRenderBuffer.persistentHandle f43e9fb5d4d06cada2c6fdcf01520bbcca21918068972a8190400cb8bae54bf8 -->
<!-- api: P:Inno.Rendering.CompiledRenderBuffer.physicalSlot ec38161ff22c9fb88b363e11b862cba591c8a9d4c1c9b23fdb4fd0ec4f308f62 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-6e115085f8684d5e"></a>[`public Inno.Rendering.RenderBufferDescriptor Inno.Rendering.CompiledRenderBuffer.descriptor { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L143) | Gets buffer requirements. | — | — |
| <a id="api-b9cb9deda23333db"></a>[`public Inno.Rendering.RenderBufferHandle Inno.Rendering.CompiledRenderBuffer.handle { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L133) | Gets the frame-scoped logical handle. | — | — |
| <a id="api-7c49d841654dbe65"></a>[`public bool Inno.Rendering.CompiledRenderBuffer.imported { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L148) | Gets whether the resource was imported from persistent device state. | — | — |
| <a id="api-ec3dd20267a70982"></a>[`public string Inno.Rendering.CompiledRenderBuffer.name { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L138) | Gets the debug and diagnostic name. | — | — |
| <a id="api-ce062a93944702c8"></a>[`public Inno.Rendering.PersistentBufferHandle Inno.Rendering.CompiledRenderBuffer.persistentHandle { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L153) | Gets the persistent device buffer for imported resources. | — | — |
| <a id="api-d41da155509a893d"></a>[`public int Inno.Rendering.CompiledRenderBuffer.physicalSlot { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L158) | Gets the aliasing allocation slot, or negative one for imported resources. | — | — |
| <a id="api-06fa9dbfd645ae78"></a>[`public sealed class Inno.Rendering.CompiledRenderBuffer`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L112) | Describes one buffer allocation selected by render-graph compilation. | — | — |

### `Inno.Rendering.CompiledRenderGraph`

<!-- api: T:Inno.Rendering.CompiledRenderGraph 73542b14adc34b97802c3afc790efa14dfa39db4b80239c37dfd1db60f2dbcde -->
<!-- api: P:Inno.Rendering.CompiledRenderGraph.generation 5a182ad609173984df6c4f1aa04065896604a30428cd7a0efb3c4cecb771e144 -->
<!-- api: P:Inno.Rendering.CompiledRenderGraph.passes 32f449a14c37833b19d9df6162671af27fecb2972133af5b19162257c73e4493 -->
<!-- api: P:Inno.Rendering.CompiledRenderGraph.textures cb1be51dc4004724f7bdefbacbac1c2f9fa662bd80bbc0ddf8ec5c483a5756f1 -->
<!-- api: P:Inno.Rendering.CompiledRenderGraph.buffers f702e54528edd7956d064c8c5ad7b88344adeca77ae183bfc59cbc914aa0e3b0 -->
<!-- api: M:Inno.Rendering.CompiledRenderGraph.Execute(Inno.Rendering.IRenderGraphBackend,System.UInt64) 8fc4e6abe0e7a737ffb915cf1b9f5a2ee2ff60b873342b98ca788968037fe695 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7a102ed1a1f6745d"></a>[`public void Inno.Rendering.CompiledRenderGraph.Execute(Inno.Rendering.IRenderGraphBackend backend, ulong frameIndex)`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L412) | Executes pass callbacks through a concrete backend with complete unwind.<br>`backend`：Backend bridge for the current device generation.<br>`frameIndex`：Monotonic frame index exposed to pass callbacks. | — | `System.AggregateException`：Thrown when execution and cleanup both fail. |
| <a id="api-55fb57a71482222e"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.CompiledRenderBuffer> Inno.Rendering.CompiledRenderGraph.buffers { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L398) | Gets compiled logical buffer allocations. | — | — |
| <a id="api-0f84e07b20345c74"></a>[`public uint Inno.Rendering.CompiledRenderGraph.generation { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L383) | Gets the source frame generation. | — | — |
| <a id="api-945446a47e8bb866"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.CompiledRenderPass> Inno.Rendering.CompiledRenderGraph.passes { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L388) | Gets scheduled passes in backend view order. | — | — |
| <a id="api-16989b50bbcfbdc3"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.CompiledRenderTexture> Inno.Rendering.CompiledRenderGraph.textures { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L393) | Gets compiled logical texture allocations. | — | — |
| <a id="api-0db3e1c9849632fd"></a>[`public sealed class Inno.Rendering.CompiledRenderGraph`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L362) | Contains validated, culled and topologically scheduled frame work. | — | — |

### `Inno.Rendering.CompiledRenderPass`

<!-- api: T:Inno.Rendering.CompiledRenderPass 9ce14d1d761892b23ea81e4f6010c8685607c919ae1dbcea90e3b47db934ba5d -->
<!-- api: P:Inno.Rendering.CompiledRenderPass.name c75d1cd5941ebaf8bf6adc07a63cb5141edb9fb50bf76485ea06f69b6a667177 -->
<!-- api: P:Inno.Rendering.CompiledRenderPass.phase 4a10b01a402bc1f9d8293c5e823b5203a1f1d6bd9ce65cffd8bcbfc00a90c565 -->
<!-- api: P:Inno.Rendering.CompiledRenderPass.kind cf73fde3cc50e051d0955f52430b194c1a5955cda6d0dc8321787ac0bb1a787c -->
<!-- api: P:Inno.Rendering.CompiledRenderPass.viewIndex a7cd9fa8603f9dd32756b87741334e4fb9fa5a7c25aca065ce65be62dc13aff2 -->
<!-- api: P:Inno.Rendering.CompiledRenderPass.attachments 5044857ed7fbacc92297ff1c5cafb9023b3f677c36b7da402321584073047156 -->
<!-- api: P:Inno.Rendering.CompiledRenderPass.surface 884315b14e6bbf5c4094f313e859d140838003ab88a21b714f2ed89c1905635a -->
<!-- api: P:Inno.Rendering.CompiledRenderPass.clearsPresentationTarget 0abc6d754d040d379f9f7669feff033c6836f5eb7b16bd5e34df79058145a3eb -->
<!-- api: P:Inno.Rendering.CompiledRenderPass.presentationClearColor 6a41b57c9d63590657a5f5c76d32af5b59edf203a154845de30edcfe579589e7 -->
<!-- api: P:Inno.Rendering.CompiledRenderPass.viewTransform 59da592e3c50831483785fae5c069ffa74da249509b522f3488cd353cfc9fe4f -->
<!-- api: P:Inno.Rendering.CompiledRenderPass.recordingMode dd7ce57cff925817e507488492ad9871e9ce84e4d08c2c776cb712787b479776 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-ab9654e6c8b0c57a"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.CompiledRenderAttachment> Inno.Rendering.CompiledRenderPass.attachments { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L288) | Gets raster attachments in declaration order. | — | — |
| <a id="api-e5b2d400f6b0b8c5"></a>[`public bool Inno.Rendering.CompiledRenderPass.clearsPresentationTarget { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L298) | Gets whether this pass clears its presentation target before drawing. | — | — |
| <a id="api-d4a007b1145ff288"></a>[`public Inno.Rendering.RenderPassKind Inno.Rendering.CompiledRenderPass.kind { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L278) | Gets the pass command domain. | — | — |
| <a id="api-6a42ebb7affa2e30"></a>[`public string Inno.Rendering.CompiledRenderPass.name { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L268) | Gets the unique diagnostic name. | — | — |
| <a id="api-17b3d718798bc24d"></a>[`public Inno.Rendering.RenderPhaseId Inno.Rendering.CompiledRenderPass.phase { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L273) | Gets the open render phase identifier. | — | — |
| <a id="api-e2730be58c89dd79"></a>[`public Inno.Rendering.RenderClearColor Inno.Rendering.CompiledRenderPass.presentationClearColor { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L303) | Gets the linear clear color for a presentation target. | — | — |
| <a id="api-d2303544abd7b30f"></a>[`public Inno.Rendering.RenderPassRecordingMode Inno.Rendering.CompiledRenderPass.recordingMode { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L313) | Gets whether callback recording is serial or worker-thread eligible. | — | — |
| <a id="api-acbd63a69ebfafdb"></a>[`public Inno.Rendering.RenderSurfaceHandle Inno.Rendering.CompiledRenderPass.surface { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L293) | Gets the detached presentation surface, or an invalid handle for the primary backbuffer. | — | — |
| <a id="api-d95c0923756ba732"></a>[`public int Inno.Rendering.CompiledRenderPass.viewIndex { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L283) | Gets the backend-neutral logical view order. | — | — |
| <a id="api-b50303846d9908e7"></a>[`public Inno.Rendering.RenderViewTransform? Inno.Rendering.CompiledRenderPass.viewTransform { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L308) | Gets backend-ready view transforms, or null for a matrix-free pass. | — | — |
| <a id="api-b46339ec620fc376"></a>[`public sealed class Inno.Rendering.CompiledRenderPass`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L234) | Provides immutable backend-facing metadata for one scheduled pass. | — | — |

### `Inno.Rendering.CompiledRenderTexture`

<!-- api: T:Inno.Rendering.CompiledRenderTexture 02b39d9eaa6c19727140e23aa8989df4e7abe2d0e2db88e8f630e109b0978af3 -->
<!-- api: P:Inno.Rendering.CompiledRenderTexture.handle febe37e441754d0784e1545616b16c21289e068500ede68ca4fe9ca92d40a677 -->
<!-- api: P:Inno.Rendering.CompiledRenderTexture.name d00c1ca401174bfe2df798329284eb9cbae8315f7f4078289a52a85c76bc83b6 -->
<!-- api: P:Inno.Rendering.CompiledRenderTexture.descriptor 55b376dda4a64d00cb27f45a76398c8b9dc6bd268633c8b2a16387467f4086e8 -->
<!-- api: P:Inno.Rendering.CompiledRenderTexture.imported 411a803e12a7e00f70d895e08d0391649abbbdd19f114e8a9144cf92abe76628 -->
<!-- api: P:Inno.Rendering.CompiledRenderTexture.persistentHandle 5771afeb26885d4c89ed0fef1904a9eed8ba2a05c51a835582d77eca23811d26 -->
<!-- api: P:Inno.Rendering.CompiledRenderTexture.physicalSlot 8bf8ac5bf1b4ed60e856b25fd73cc307523ebebc6545388bb5f7bd0477a8b162 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-eca64ec71914c15f"></a>[`public Inno.Rendering.RenderTextureDescriptor Inno.Rendering.CompiledRenderTexture.descriptor { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L91) | Gets texture requirements. | — | — |
| <a id="api-6786983e07887712"></a>[`public Inno.Rendering.RenderTextureHandle Inno.Rendering.CompiledRenderTexture.handle { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L81) | Gets the frame-scoped logical handle. | — | — |
| <a id="api-b4c1a439c4d6c8fb"></a>[`public bool Inno.Rendering.CompiledRenderTexture.imported { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L96) | Gets whether the resource was imported from persistent device state. | — | — |
| <a id="api-dded72f642080cfa"></a>[`public string Inno.Rendering.CompiledRenderTexture.name { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L86) | Gets the debug and diagnostic name. | — | — |
| <a id="api-64a978267c72e215"></a>[`public Inno.Rendering.PersistentTextureHandle Inno.Rendering.CompiledRenderTexture.persistentHandle { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L101) | Gets the persistent device texture for imported resources. | — | — |
| <a id="api-131fe2729ecdd365"></a>[`public int Inno.Rendering.CompiledRenderTexture.physicalSlot { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L106) | Gets the aliasing allocation slot, or negative one for imported resources. | — | — |
| <a id="api-c952a2387f029b1f"></a>[`public sealed class Inno.Rendering.CompiledRenderTexture`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L60) | Describes one texture allocation selected by render-graph compilation. | — | — |

### `Inno.Rendering.ComputePassBuilder`

<!-- api: T:Inno.Rendering.ComputePassBuilder 4d83360a565805834f00378cf9df9544689116a39c095a86b2dc675bdc48487c -->
<!-- api: M:Inno.Rendering.ComputePassBuilder.SetViewTransform(System.ReadOnlySpan{System.Single},System.ReadOnlySpan{System.Single}) 0750b925fa016d28e88207aa76e0dd25ec7b4e4dc1c36dd7a7613ec8c0a1d3c2 -->
<!-- api: M:Inno.Rendering.ComputePassBuilder.ReadStorageTexture(Inno.Rendering.RenderTextureHandle) b612bfd3af4b0af27a40731851830a55aa75089b4f2cd3d8b83e214ec4a976a9 -->
<!-- api: M:Inno.Rendering.ComputePassBuilder.WriteStorageTexture(Inno.Rendering.RenderTextureHandle) bb3cb81e49fe4fa595f3c3766ef04560dedc94a9865f8e8df9d999d0a5eb2eec -->
<!-- api: M:Inno.Rendering.ComputePassBuilder.ReadWriteStorageTexture(Inno.Rendering.RenderTextureHandle) fe296cd0b2d23576542a9e2268cd43f500ad8815382bb5234a8eded94b28878e -->
<!-- api: M:Inno.Rendering.ComputePassBuilder.ReadStorageBuffer(Inno.Rendering.RenderBufferHandle) 102fd833058a1dc5e77a80d377022b5b73b1b22323e9dab13d931083cd480b96 -->
<!-- api: M:Inno.Rendering.ComputePassBuilder.WriteStorageBuffer(Inno.Rendering.RenderBufferHandle) 0abb222231066a4b70b30fd7ca4ba471558a77b67135cacd1d0d4621cdc20028 -->
<!-- api: M:Inno.Rendering.ComputePassBuilder.ReadWriteStorageBuffer(Inno.Rendering.RenderBufferHandle) 829e414ef177288dbeff385ea8c1c37a78cc17e3763675b891789ffa46f399fc -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d7c413aabbe6347c"></a>[`public Inno.Rendering.ComputePassBuilder Inno.Rendering.ComputePassBuilder.ReadStorageBuffer(Inno.Rendering.RenderBufferHandle buffer)`](../../src/services/rendering/Inno.Rendering/Graph/ComputePassBuilder.cs#L104) | Declares an unordered buffer read.<br>返回：This builder for fluent declarations.<br>`buffer`：Storage buffer read by compute work. | — | — |
| <a id="api-bea17fc6dcd6c581"></a>[`public Inno.Rendering.ComputePassBuilder Inno.Rendering.ComputePassBuilder.ReadStorageTexture(Inno.Rendering.RenderTextureHandle texture)`](../../src/services/rendering/Inno.Rendering/Graph/ComputePassBuilder.cs#L47) | Declares an unordered texture read.<br>返回：This builder for fluent declarations.<br>`texture`：Storage texture read by compute work. | — | — |
| <a id="api-246c183bd656e139"></a>[`public Inno.Rendering.ComputePassBuilder Inno.Rendering.ComputePassBuilder.ReadWriteStorageBuffer(Inno.Rendering.RenderBufferHandle buffer)`](../../src/services/rendering/Inno.Rendering/Graph/ComputePassBuilder.cs#L142) | Declares an unordered buffer read and write.<br>返回：This builder for fluent declarations.<br>`buffer`：Storage buffer read and written by compute work. | — | — |
| <a id="api-1c864a3bc37279cd"></a>[`public Inno.Rendering.ComputePassBuilder Inno.Rendering.ComputePassBuilder.ReadWriteStorageTexture(Inno.Rendering.RenderTextureHandle texture)`](../../src/services/rendering/Inno.Rendering/Graph/ComputePassBuilder.cs#L85) | Declares an unordered texture read and write.<br>返回：This builder for fluent declarations.<br>`texture`：Storage texture read and written by compute work. | — | — |
| <a id="api-11d9f8d226f7f79c"></a>[`public Inno.Rendering.ComputePassBuilder Inno.Rendering.ComputePassBuilder.SetViewTransform(System.ReadOnlySpan<float> viewMatrix, System.ReadOnlySpan<float> projectionMatrix)`](../../src/services/rendering/Inno.Rendering/Graph/ComputePassBuilder.cs#L29) | Sets backend-ready column-major transform matrices exposed to this compute view.<br>返回：This builder for fluent declarations.<br>`viewMatrix`：Exactly sixteen column-major world-to-view values.<br>`projectionMatrix`：Exactly sixteen column-major projection values. | — | — |
| <a id="api-68567816c461c19a"></a>[`public Inno.Rendering.ComputePassBuilder Inno.Rendering.ComputePassBuilder.WriteStorageBuffer(Inno.Rendering.RenderBufferHandle buffer)`](../../src/services/rendering/Inno.Rendering/Graph/ComputePassBuilder.cs#L123) | Declares an unordered buffer write.<br>返回：This builder for fluent declarations.<br>`buffer`：Storage buffer written by compute work. | — | — |
| <a id="api-21b0f5884e65091e"></a>[`public Inno.Rendering.ComputePassBuilder Inno.Rendering.ComputePassBuilder.WriteStorageTexture(Inno.Rendering.RenderTextureHandle texture)`](../../src/services/rendering/Inno.Rendering/Graph/ComputePassBuilder.cs#L66) | Declares an unordered texture write.<br>返回：This builder for fluent declarations.<br>`texture`：Storage texture written by compute work. | — | — |
| <a id="api-44012bebb6347363"></a>[`public sealed class Inno.Rendering.ComputePassBuilder : Inno.Rendering.RenderPassBuilder`](../../src/services/rendering/Inno.Rendering/Graph/ComputePassBuilder.cs#L9) | Declares compute resource reads and unordered writes. | — | — |

### `Inno.Rendering.ComputePipelineDescriptor`

<!-- api: T:Inno.Rendering.ComputePipelineDescriptor 8ecdac46002abd071a9d2be0e09fd79bd8fbf93fbfcf84e99ddb4134be743ab5 -->
<!-- api: M:Inno.Rendering.ComputePipelineDescriptor.#ctor(System.ReadOnlySpan{System.Byte},System.Collections.Generic.IReadOnlyList{Inno.Rendering.RenderShaderBindingDescriptor}) 5db6dc9076faf0d43e51305f0a3afdf383df7d70bbdeb42793c21be33a0fc5aa -->
<!-- api: P:Inno.Rendering.ComputePipelineDescriptor.computeShader 8b7ae2c31f95784f8a04329dc0ffb3a359d3c09554b16621177d9d48436e0628 -->
<!-- api: P:Inno.Rendering.ComputePipelineDescriptor.bindings 2b06fe36b54b31a5dff22e48436539c8633f785ffd3ec2ef7411461fd7ec6b65 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c7f265e46e79ebaf"></a>[`public Inno.Rendering.ComputePipelineDescriptor.ComputePipelineDescriptor(System.ReadOnlySpan<byte> computeShader, System.Collections.Generic.IReadOnlyList<Inno.Rendering.RenderShaderBindingDescriptor> bindings)`](../../src/services/rendering/Inno.Rendering/Device/ComputePipelineDescriptor.cs#L24) | Creates a compute pipeline descriptor.<br>`computeShader`：Target backend compute shader binary.<br>`bindings`：Manifest-derived interface contract. | — | — |
| <a id="api-950e679affa0b0ca"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.RenderShaderBindingDescriptor> Inno.Rendering.ComputePipelineDescriptor.bindings { get; }`](../../src/services/rendering/Inno.Rendering/Device/ComputePipelineDescriptor.cs#L51) | Gets the manifest-derived interface contract. | — | — |
| <a id="api-5917f24a981a2a02"></a>[`public System.ReadOnlyMemory<byte> Inno.Rendering.ComputePipelineDescriptor.computeShader { get; }`](../../src/services/rendering/Inno.Rendering/Device/ComputePipelineDescriptor.cs#L46) | Gets the target backend compute shader binary. | — | — |
| <a id="api-12473054302c1642"></a>[`public sealed class Inno.Rendering.ComputePipelineDescriptor`](../../src/services/rendering/Inno.Rendering/Device/ComputePipelineDescriptor.cs#L10) | Describes a compute program candidate and reflected interface contract. | — | — |

### `Inno.Rendering.ComputePipelineHandle`

<!-- api: T:Inno.Rendering.ComputePipelineHandle 3ec47638fa87333e6662e5f08473457ecbbd9dedea03054c7ee8e1d25c7f1008 -->
<!-- api: P:Inno.Rendering.ComputePipelineHandle.isValid 7b13223a4e5d9ea878aa1d2a2981fc4ee330a7f038165714064a6f166edda478 -->
<!-- api: M:Inno.Rendering.ComputePipelineHandle.ToString 555a0fa526150a037d68a1319af94e072e9b3aa1b33a805e1708a6021fdb0a77 -->
<!-- api: M:Inno.Rendering.ComputePipelineHandle.op_Inequality(Inno.Rendering.ComputePipelineHandle,Inno.Rendering.ComputePipelineHandle) b9a5e1b4ba50c1a09debf078319de8d3306447635ab4c04458f35a7f76724190 -->
<!-- api: M:Inno.Rendering.ComputePipelineHandle.op_Equality(Inno.Rendering.ComputePipelineHandle,Inno.Rendering.ComputePipelineHandle) e378a137fa16b78d7269a6a3b3b5070f27b4d95a45753cf6fb1197386af1b2b9 -->
<!-- api: M:Inno.Rendering.ComputePipelineHandle.GetHashCode 8d1ea2bee6e002b8badff5e4b2bd48e4850187fd64a71e2d14f11ac679ea683b -->
<!-- api: M:Inno.Rendering.ComputePipelineHandle.Equals(System.Object) 7c65d222f0a6d55f940cafa2c7626b735e429cab8159152a1a9917ba03fc44e5 -->
<!-- api: M:Inno.Rendering.ComputePipelineHandle.Equals(Inno.Rendering.ComputePipelineHandle) 3233285f61d75d90ffaa56adb22cb83267d281e3fdb8ca5d77a8fe5d5ad1e8b0 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-ba80df1a680d106f"></a>[`public bool Inno.Rendering.ComputePipelineHandle.Equals(Inno.Rendering.ComputePipelineHandle other)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L58) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-e7700aeb7990c171"></a>[`public override bool Inno.Rendering.ComputePipelineHandle.Equals(object obj)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L58) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-ef9d54a8310052f3"></a>[`public override int Inno.Rendering.ComputePipelineHandle.GetHashCode()`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L58) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-d2dfd84fd37b7fb9"></a>[`public static bool Inno.Rendering.ComputePipelineHandle.operator ==(Inno.Rendering.ComputePipelineHandle left, Inno.Rendering.ComputePipelineHandle right)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L58) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-5885db5d6b5126f5"></a>[`public static bool Inno.Rendering.ComputePipelineHandle.operator !=(Inno.Rendering.ComputePipelineHandle left, Inno.Rendering.ComputePipelineHandle right)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L58) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-5e8ec3fefe6786da"></a>[`public override string Inno.Rendering.ComputePipelineHandle.ToString()`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L58) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-cf7f0ece490eedbb"></a>[`public bool Inno.Rendering.ComputePipelineHandle.isValid { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L74) | Gets whether the handle identifies a compute pipeline. | — | — |
| <a id="api-6b5add83d54e1877"></a>[`public readonly record struct Inno.Rendering.ComputePipelineHandle : System.IEquatable<Inno.Rendering.ComputePipelineHandle>`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L58) | Identifies a backend-neutral compute pipeline object. | — | — |

### `Inno.Rendering.CopyPassBuilder`

<!-- api: T:Inno.Rendering.CopyPassBuilder 0c182ee5446fe8adab23906d91d1e7e9d80c49a793e87489ad3355f57a9a6d18 -->
<!-- api: M:Inno.Rendering.CopyPassBuilder.CopyTexture(Inno.Rendering.RenderTextureHandle,Inno.Rendering.RenderTextureHandle) 2c3688564d3eed7bdf2da66a80ad03945e9e6447527db2e325c6c480dce5af28 -->
<!-- api: M:Inno.Rendering.CopyPassBuilder.CopyBuffer(Inno.Rendering.RenderBufferHandle,Inno.Rendering.RenderBufferHandle) 13074798991ddc3e09259d7c6613c7c3f10a215f52a41437bd79794e58d86adc -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-5271c70c1ab73ec0"></a>[`public Inno.Rendering.CopyPassBuilder Inno.Rendering.CopyPassBuilder.CopyBuffer(Inno.Rendering.RenderBufferHandle source, Inno.Rendering.RenderBufferHandle destination)`](../../src/services/rendering/Inno.Rendering/Graph/CopyPassBuilder.cs#L58) | Declares one buffer copy operation.<br>返回：This builder for fluent declarations.<br>`source`：Copy source.<br>`destination`：Copy destination. | — | — |
| <a id="api-5e0b97483cd88b51"></a>[`public Inno.Rendering.CopyPassBuilder Inno.Rendering.CopyPassBuilder.CopyTexture(Inno.Rendering.RenderTextureHandle source, Inno.Rendering.RenderTextureHandle destination)`](../../src/services/rendering/Inno.Rendering/Graph/CopyPassBuilder.cs#L29) | Declares one texture copy operation.<br>返回：This builder for fluent declarations.<br>`source`：Copy source.<br>`destination`：Copy destination. | — | — |
| <a id="api-20bfafa6bf8d334f"></a>[`public sealed class Inno.Rendering.CopyPassBuilder : Inno.Rendering.RenderPassBuilder`](../../src/services/rendering/Inno.Rendering/Graph/CopyPassBuilder.cs#L9) | Declares explicit resource copy access. | — | — |

### `Inno.Rendering.GraphicsApi`

<!-- api: T:Inno.Rendering.GraphicsApi 888f020cd1d8392520d0dec26299fca851f572524fa96170f644b1383177e5db -->
<!-- api: P:Inno.Rendering.GraphicsApi.Noop 7aa13dd8f7ae7a38c4fd97e65dd42984db23088c2ba070911f81ec75984a1894 -->
<!-- api: P:Inno.Rendering.GraphicsApi.Direct3D11 3221d107732e10e7dfbc8d02fc7644f08626bfd6c4dd62b4dfe2c136bbeffa4b -->
<!-- api: P:Inno.Rendering.GraphicsApi.Direct3D12 708623e09e0a07bd63952b0482066fc228db79790ea47ddb0102dbdae06b4700 -->
<!-- api: P:Inno.Rendering.GraphicsApi.Metal 1840fd96ae4d1f892946353d454483115e3573f4cc70276194e0a2e792247d95 -->
<!-- api: P:Inno.Rendering.GraphicsApi.Vulkan 818ca50678a6bf4dd7be7a2b86512ef70522f71e22febfca763e3dbc033d0069 -->
<!-- api: P:Inno.Rendering.GraphicsApi.OpenGL 3337e241d96ae9575279be3fe25263bc6d1bfe9942e383ed383f2285695ed36f -->
<!-- api: P:Inno.Rendering.GraphicsApi.OpenGLES fee1087678098a424512842ba9a84e8373feee2f77bd7d3d3493e53b41ae9ead -->
<!-- api: P:Inno.Rendering.GraphicsApi.WebGPU 198c92ed49ec3b6ed1b062f27eff3e712b2ae172ea419de5171a6fab289607cc -->
<!-- api: M:Inno.Rendering.GraphicsApi.#ctor(System.String) 654355bbe3ad56fbcb432dd893101719b32a201ce47da5f0642e3186a0a9f613 -->
<!-- api: P:Inno.Rendering.GraphicsApi.value fbfc602ea000eed7b12d3cb62be29bb203e7b43394885333221f6eb6224967bc -->
<!-- api: P:Inno.Rendering.GraphicsApi.isValid d24327807322b4875b5c02d8b6f68bf1f5e3030785e3c9f29c5c355325d78ab1 -->
<!-- api: M:Inno.Rendering.GraphicsApi.TryParse(System.String,Inno.Rendering.GraphicsApi@) 9c0219bc6cffdfa7888d48f0373a044d78178fc28656f00834ae53adf8973394 -->
<!-- api: M:Inno.Rendering.GraphicsApi.CompareTo(Inno.Rendering.GraphicsApi) 6a07c9602215e064f1f4d68d7cd9a9d31c8deabe7a99ef44b356e95e04100acd -->
<!-- api: M:Inno.Rendering.GraphicsApi.ToString b5284b6c1485ae651d4c966b2e8b7d32bd0a44b7d90547de9f8515c5d7d14a0f -->
<!-- api: M:Inno.Rendering.GraphicsApi.op_Inequality(Inno.Rendering.GraphicsApi,Inno.Rendering.GraphicsApi) f3bf0bbe674af44634def6111c6cb0974c13c490864b846a2004beee32208282 -->
<!-- api: M:Inno.Rendering.GraphicsApi.op_Equality(Inno.Rendering.GraphicsApi,Inno.Rendering.GraphicsApi) acdd4edce195740067dd47fd85f0a22c14ad2eeef10f7707d4439244fdb4c0ce -->
<!-- api: M:Inno.Rendering.GraphicsApi.GetHashCode 5761563f79d7ee13860f931dbbe9ad46bb947c8fbf3c01cff858f8bb42c857a9 -->
<!-- api: M:Inno.Rendering.GraphicsApi.Equals(System.Object) 3d8d1ba1d51a0af6eec3cfd4799c9c2d7bc9eab6986cc062652457b1028bef55 -->
<!-- api: M:Inno.Rendering.GraphicsApi.Equals(Inno.Rendering.GraphicsApi) 139449d03d0d55da885ed479184ac51ecafc4f7fd1081902fa00bb039154b486 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b5c6f0a0a79c02dc"></a>[`public Inno.Rendering.GraphicsApi.GraphicsApi(string value)`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L55) | Creates an open, stable backend identity without registering a backend implementation.<br>`value`：A portable case-sensitive identifier used in target artifact paths. | — | `System.ArgumentException`：The identifier is empty or contains nonportable characters. |
| <a id="api-6704cf5864208ecf"></a>[`public int Inno.Rendering.GraphicsApi.CompareTo(Inno.Rendering.GraphicsApi other)`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L106) | Compares two backend identities using stable ordinal ordering.<br>返回：A negative, zero, or positive value according to ordinal identifier order.<br>`other`：The other backend identity. | — | — |
| <a id="api-e33548035c364635"></a>[`public bool Inno.Rendering.GraphicsApi.Equals(Inno.Rendering.GraphicsApi other)`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L11) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-8d294b7f3a57c89c"></a>[`public override bool Inno.Rendering.GraphicsApi.Equals(object obj)`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L11) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-86c6c1ade442a81b"></a>[`public override int Inno.Rendering.GraphicsApi.GetHashCode()`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L11) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-4d8b258db79adadb"></a>[`public static bool Inno.Rendering.GraphicsApi.operator ==(Inno.Rendering.GraphicsApi left, Inno.Rendering.GraphicsApi right)`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L11) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-9e44293604b016d0"></a>[`public static bool Inno.Rendering.GraphicsApi.operator !=(Inno.Rendering.GraphicsApi left, Inno.Rendering.GraphicsApi right)`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L11) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-8900e9005c56b5a7"></a>[`public override string Inno.Rendering.GraphicsApi.ToString()`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L114) | Formats the exact stable backend identity.<br>返回：The identity, or an empty string for an uninitialized value. | — | — |
| <a id="api-90c431547cae690c"></a>[`public static bool Inno.Rendering.GraphicsApi.TryParse(string? value, out Inno.Rendering.GraphicsApi api)`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L86) | Parses an open backend identity without consulting a closed engine-side backend list.<br>返回：True when the identifier is portable and nonempty.<br>`value`：The exact stable backend identity.<br>`api`：The parsed value, or default when parsing fails. | — | — |
| <a id="api-1c6f7140631a62a6"></a>[`public static Inno.Rendering.GraphicsApi Inno.Rendering.GraphicsApi.Direct3D11 { get; }`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L20) | Direct3D 11 renderer. | — | — |
| <a id="api-6912d0aaf1f2c989"></a>[`public static Inno.Rendering.GraphicsApi Inno.Rendering.GraphicsApi.Direct3D12 { get; }`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L24) | Direct3D 12 renderer. | — | — |
| <a id="api-c1b5e3d93a5ca225"></a>[`public bool Inno.Rendering.GraphicsApi.isValid { get; }`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L72) | Gets whether this value contains an initialized backend identity. | — | — |
| <a id="api-8ad99b3346fec833"></a>[`public static Inno.Rendering.GraphicsApi Inno.Rendering.GraphicsApi.Metal { get; }`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L28) | Apple Metal renderer. | — | — |
| <a id="api-67ac63e4a757a727"></a>[`public static Inno.Rendering.GraphicsApi Inno.Rendering.GraphicsApi.Noop { get; }`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L16) | Headless validation backend. | — | — |
| <a id="api-93ed4d5e9f405d92"></a>[`public static Inno.Rendering.GraphicsApi Inno.Rendering.GraphicsApi.OpenGL { get; }`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L36) | Desktop OpenGL renderer. | — | — |
| <a id="api-762a4ac0af87c857"></a>[`public static Inno.Rendering.GraphicsApi Inno.Rendering.GraphicsApi.OpenGLES { get; }`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L40) | OpenGL ES renderer. | — | — |
| <a id="api-33711c77b8e93ce7"></a>[`public string Inno.Rendering.GraphicsApi.value { get; }`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L67) | Gets the stable backend identity used by target artifacts and provider selection. | — | — |
| <a id="api-6b017a4c6b1c6bcf"></a>[`public static Inno.Rendering.GraphicsApi Inno.Rendering.GraphicsApi.Vulkan { get; }`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L32) | Khronos Vulkan renderer. | — | — |
| <a id="api-2a52337a811dacac"></a>[`public static Inno.Rendering.GraphicsApi Inno.Rendering.GraphicsApi.WebGPU { get; }`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L44) | WebGPU renderer. | — | — |
| <a id="api-06ab30df5061802e"></a>[`public readonly record struct Inno.Rendering.GraphicsApi : System.IComparable<Inno.Rendering.GraphicsApi>, System.IEquatable<Inno.Rendering.GraphicsApi>`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L11) | Identifies the graphics API family selected by the host without exposing a backend-native enum. Pipelines use this value only for capability-aware choices and target artifact selection; it does not require separate pipeline or shader source implementations. | — | — |

### `Inno.Rendering.GraphicsCapabilities`

<!-- api: T:Inno.Rendering.GraphicsCapabilities 81ed9203ce8faf195d05d217ff2703b4f5f345c749edb6383df5a20d93c31eb5 -->
<!-- api: M:Inno.Rendering.GraphicsCapabilities.#ctor(Inno.Rendering.GraphicsApi,Inno.Rendering.GraphicsCapability,Inno.Rendering.GraphicsLimits,System.Collections.Generic.IEnumerable{Inno.Rendering.RenderTextureFormat},System.Collections.Generic.IEnumerable{Inno.Rendering.RenderTextureFormat},System.Collections.Generic.IEnumerable{Inno.Rendering.RenderTextureFormat},System.Collections.Generic.IEnumerable{Inno.Rendering.RenderTextureFormat},System.Boolean,System.Boolean,System.Collections.Generic.IEnumerable{Inno.Rendering.RenderTextureFormat},System.Collections.Generic.IEnumerable{Inno.Rendering.RenderTextureFormat},System.Collections.Generic.IEnumerable{Inno.Rendering.RenderTextureFormat}) a5f6a0661d615ce11acc11786836205aeec63a3b1acfb043d6345878d93173f6 -->
<!-- api: P:Inno.Rendering.GraphicsCapabilities.backend ad705400ef076f1823387a2b19c2cfefeb4dffd1782766578d51a08c78f1ee21 -->
<!-- api: P:Inno.Rendering.GraphicsCapabilities.features d1044e7765632a0c4ded513b149716a7944e45296b4640124e97ba82a71eee19 -->
<!-- api: P:Inno.Rendering.GraphicsCapabilities.limits ae94d852b2acafc342d30f796ed3dbcaf0432a877736c74a04381028b0b70e09 -->
<!-- api: P:Inno.Rendering.GraphicsCapabilities.originBottomLeft 270c197f057b88f7f2c6b022bda8524610e2d079dd78f83d202bd9a7736d1ee8 -->
<!-- api: P:Inno.Rendering.GraphicsCapabilities.homogeneousDepth c3b0285b09aa79062af159276774c2e5455d430f4c1e101f11d4b4eab8f1900c -->
<!-- api: M:Inno.Rendering.GraphicsCapabilities.Supports(Inno.Rendering.GraphicsCapability) ad5b2e2b69db8e95842f0577cacf90632ef67222e70f0e174622429f6ea80fdd -->
<!-- api: M:Inno.Rendering.GraphicsCapabilities.SupportsSampled(Inno.Rendering.RenderTextureFormat) e22abc877b66e3f424a419be7918c1fc208cfd4e5dbde98a76458e040f8baea2 -->
<!-- api: M:Inno.Rendering.GraphicsCapabilities.SupportsSampled(Inno.Rendering.RenderTextureFormat,Inno.Rendering.RenderTextureDimension) 6c93fea4c3f1c043138ebbbcc515ba109218dfb70b65f149088831b6a17ffe78 -->
<!-- api: M:Inno.Rendering.GraphicsCapabilities.SupportsRenderTarget(Inno.Rendering.RenderTextureFormat) c067d15b0ef077357fa17fdbffbdfadc067015a149a195b722bf824251b44a54 -->
<!-- api: M:Inno.Rendering.GraphicsCapabilities.SupportsMultisampleRenderTarget(Inno.Rendering.RenderTextureFormat) d81a84d68ec9843850a272040404559a49b5674c95e47b22e7e2191f28d03b74 -->
<!-- api: M:Inno.Rendering.GraphicsCapabilities.SupportsStorage(Inno.Rendering.RenderTextureFormat,Inno.Rendering.RenderStorageAccess) a625b273e9763803d0621ed5983998624269d99eb9e3531cbb1767751043f94e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-465656d5a200663c"></a>[`public Inno.Rendering.GraphicsCapabilities.GraphicsCapabilities(Inno.Rendering.GraphicsApi backend, Inno.Rendering.GraphicsCapability features, Inno.Rendering.GraphicsLimits limits, System.Collections.Generic.IEnumerable<Inno.Rendering.RenderTextureFormat> sampledFormats, System.Collections.Generic.IEnumerable<Inno.Rendering.RenderTextureFormat> renderTargetFormats, System.Collections.Generic.IEnumerable<Inno.Rendering.RenderTextureFormat> storageReadFormats, System.Collections.Generic.IEnumerable<Inno.Rendering.RenderTextureFormat> storageWriteFormats, bool originBottomLeft, bool homogeneousDepth, System.Collections.Generic.IEnumerable<Inno.Rendering.RenderTextureFormat>? sampled3DFormats = null, System.Collections.Generic.IEnumerable<Inno.Rendering.RenderTextureFormat>? sampledCubeFormats = null, System.Collections.Generic.IEnumerable<Inno.Rendering.RenderTextureFormat>? multisampleRenderTargetFormats = null)`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L328) | Creates a device capability snapshot.<br>`backend`：Active graphics API family.<br>`features`：Supported optional features.<br>`limits`：Device limits.<br>`sampledFormats`：Formats valid for sampled two-dimensional textures.<br>`renderTargetFormats`：Formats valid as raster attachments.<br>`storageReadFormats`：Formats valid for unordered shader reads.<br>`storageWriteFormats`：Formats valid for unordered shader writes.<br>`originBottomLeft`：Whether render-target coordinates start at the bottom-left.<br>`homogeneousDepth`：Whether clip-space depth uses the negative-one-to-one range.<br>`sampled3DFormats`：Formats valid for sampled volume textures.<br>`sampledCubeFormats`：Formats valid for sampled cubemaps.<br>`multisampleRenderTargetFormats`：Formats valid as multisampled raster attachments. | — | — |
| <a id="api-18553528180289bc"></a>[`public bool Inno.Rendering.GraphicsCapabilities.Supports(Inno.Rendering.GraphicsCapability required)`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L398) | Tests whether every requested optional feature is supported.<br>返回：true when every requested feature is available.<br>`required`：The required consumed by supports; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-9d80ded1828bcac3"></a>[`public bool Inno.Rendering.GraphicsCapabilities.SupportsMultisampleRenderTarget(Inno.Rendering.RenderTextureFormat format)`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L455) | Tests whether a format is valid as a multisampled raster attachment.<br>返回：true when the format can be attached with multisampling.<br>`format`：Texture format to query. | — | — |
| <a id="api-27cf74ec5a4d24ba"></a>[`public bool Inno.Rendering.GraphicsCapabilities.SupportsRenderTarget(Inno.Rendering.RenderTextureFormat format)`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L444) | Tests whether a format is valid as a raster attachment.<br>返回：true when the format can be attached.<br>`format`：The format consumed by supports render target; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-fce8c824541bfde6"></a>[`public bool Inno.Rendering.GraphicsCapabilities.SupportsSampled(Inno.Rendering.RenderTextureFormat format, Inno.Rendering.RenderTextureDimension dimension)`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L423) | Tests whether a format is valid for sampled textures of one dimensional shape.<br>返回：true when the format and dimension can be sampled.<br>`format`：Texture format to query.<br>`dimension`：Texture shape to query. | — | — |
| <a id="api-b34f95add55d87ad"></a>[`public bool Inno.Rendering.GraphicsCapabilities.SupportsSampled(Inno.Rendering.RenderTextureFormat format)`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L409) | Tests whether a format is valid for sampled two-dimensional textures.<br>返回：true when the format can be sampled.<br>`format`：Texture format to query. | — | — |
| <a id="api-cde6e67125f10f18"></a>[`public bool Inno.Rendering.GraphicsCapabilities.SupportsStorage(Inno.Rendering.RenderTextureFormat format, Inno.Rendering.RenderStorageAccess access)`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L469) | Tests whether a format supports the requested unordered shader access.<br>返回：true when the format supports every requested access direction.<br>`format`：Texture format to query.<br>`access`：Required storage access. | — | — |
| <a id="api-8b240f354ee0006c"></a>[`public Inno.Rendering.GraphicsApi Inno.Rendering.GraphicsCapabilities.backend { get; }`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L367) | Gets the active graphics API family. | — | — |
| <a id="api-7b5f0860a1e386fe"></a>[`public Inno.Rendering.GraphicsCapability Inno.Rendering.GraphicsCapabilities.features { get; }`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L372) | Gets all supported optional features. | — | — |
| <a id="api-5cdedcc3fab6ae23"></a>[`public bool Inno.Rendering.GraphicsCapabilities.homogeneousDepth { get; }`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L387) | Gets whether clip-space depth uses the negative-one-to-one range. | — | — |
| <a id="api-a78e479abd2b06dc"></a>[`public Inno.Rendering.GraphicsLimits Inno.Rendering.GraphicsCapabilities.limits { get; }`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L377) | Gets device limits. | — | — |
| <a id="api-b595877ff5e39e0b"></a>[`public bool Inno.Rendering.GraphicsCapabilities.originBottomLeft { get; }`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L382) | Gets whether render-target coordinates start at the bottom-left. | — | — |
| <a id="api-b6f9aed55236c60c"></a>[`public sealed class Inno.Rendering.GraphicsCapabilities`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L279) | Provides an immutable capability snapshot for one device generation. | — | — |

### `Inno.Rendering.GraphicsCapability`

<!-- api: T:Inno.Rendering.GraphicsCapability 2d2ed1786ca2c921d83fd9cce23d76a92c65c96621b1c6071d92fa2e194245a9 -->
<!-- api: F:Inno.Rendering.GraphicsCapability.None 4a3d3576d455223b5e07d453bdf348f7bc09b9841ee5f2c32c3cd539703973d8 -->
<!-- api: F:Inno.Rendering.GraphicsCapability.Compute 44eb0b72ca2b157ff590f60b45e8de96498bfda545b6cb4e0260506283010a84 -->
<!-- api: F:Inno.Rendering.GraphicsCapability.StorageBuffer 24dd963e054613a4b0a758ee58cfc82704edc1dce12281c727e797a62b320496 -->
<!-- api: F:Inno.Rendering.GraphicsCapability.Indirect fb39896c338da33863377c24f89fa80f8f40d8e3d2fe68feb6762a6e490fba15 -->
<!-- api: F:Inno.Rendering.GraphicsCapability.IndependentBlend a56eec87f615f7559ac330893886dd542f293d78c21646e98118df2e9b6ea0d4 -->
<!-- api: F:Inno.Rendering.GraphicsCapability.ConcurrentEncoders 073ca14d8d12d983dfa7a6768698ee3d171205e48fb5b3ade115cbcbfaddd414 -->
<!-- api: F:Inno.Rendering.GraphicsCapability.TextureBlit 6d7bd63a154d4fb106187efce825ba267877a367984cc1d1c62218f5b8b9b2b2 -->
<!-- api: F:Inno.Rendering.GraphicsCapability.BufferCopy c30fd61849bc1162663f8bcb68007c879b28437b610bf0f686765c36a6ff1aed -->
<!-- api: F:Inno.Rendering.GraphicsCapability.AlphaToCoverage a4e1ccccb1ca05f42c6f33be30f605624e5aa23dfce1ff8fb8d9d814c3ade712 -->
<!-- api: F:Inno.Rendering.GraphicsCapability.Index32 2d935d9fb5380d40d2a4c5ae96957b1756954a2799cedb86d37d4036d85bcc4f -->
<!-- api: F:Inno.Rendering.GraphicsCapability.Instancing 5bfdb11a2d6527089cd88a65ebbd7b5a3bcead6d195b55c5ba8d4c868db352b9 -->
<!-- api: F:Inno.Rendering.GraphicsCapability.SwapChain 6970137a804199db34c46c938a1814b78361fcd1356547ff43fc8e1f18baecd1 -->
<!-- api: F:Inno.Rendering.GraphicsCapability.Texture2DArray 4d601e2b3a263861658c5226c9af0e7599907b4bf62752317c7a4cdb1641af87 -->
<!-- api: F:Inno.Rendering.GraphicsCapability.Texture3D 16a3387d8aaf90d52d3240fc369043411a008ab672f6b4f961048ba9e72f8db9 -->
<!-- api: F:Inno.Rendering.GraphicsCapability.TextureCubeArray 2ec7e698186e53b615ae6026360bf03999ccb607e67b8b113dc49cd978c01bd0 -->
<!-- api: F:Inno.Rendering.GraphicsCapability.VertexAttributeHalf cc4033229841dc34214b567ec48ebbb04f77880422d4717f9a118d9d5419e979 -->
<!-- api: F:Inno.Rendering.GraphicsCapability.VertexAttributeUInt10 49f8a4fa5440666d389ce65e9546d801c416ffcae28130124f53e4dba71de8d7 -->
<!-- api: F:Inno.Rendering.GraphicsCapability.ProceduralDraw 629cef499de85915e8d5aa4a6365d21a9c1e58ec6dfdf1f4e2991095dd671c8b -->
<!-- api: F:Inno.Rendering.GraphicsCapability.FragmentDepth 1cefa584d0b7a111d15b624ab991a6043fd8c26e7305391fdfd6d708db69a3b2 -->
<!-- api: F:Inno.Rendering.GraphicsCapability.StorageTexture 7d009e3e166d4677050a3766bc9f8cf37256bfe0fe7c1057e3d12f7d19374843 -->
<!-- api: F:Inno.Rendering.GraphicsCapability.TextureReadback 238a945909de0ed8d4de25e51ff1399e1c25076b181d85d67cd40b60cbeea00c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-bdf0759fea407ca0"></a>[`Inno.Rendering.GraphicsCapability.AlphaToCoverage = 128`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L168) | Alpha-to-coverage rasterization is supported. | — | — |
| <a id="api-1dce87f84068714c"></a>[`Inno.Rendering.GraphicsCapability.BufferCopy = 64`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L164) | General GPU buffer copy operations are supported directly. | — | — |
| <a id="api-462b321acef07800"></a>[`Inno.Rendering.GraphicsCapability.Compute = 1`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L140) | Compute shader dispatch. | — | — |
| <a id="api-bc9a17530eece0a5"></a>[`Inno.Rendering.GraphicsCapability.ConcurrentEncoders = 16`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L156) | Multiple command encoders may record concurrently. | — | — |
| <a id="api-8d7c3678bc72aa6b"></a>[`Inno.Rendering.GraphicsCapability.FragmentDepth = 131072`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L208) | Fragment shaders may write depth explicitly. | — | — |
| <a id="api-a9a3f6b67f39a2a7"></a>[`Inno.Rendering.GraphicsCapability.IndependentBlend = 8`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L152) | Independent blend state per color attachment. | — | — |
| <a id="api-f5d72a3c0990d77c"></a>[`Inno.Rendering.GraphicsCapability.Index32 = 256`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L172) | Unsigned 32-bit index buffers are supported. | — | — |
| <a id="api-a5daecaadb5ccf5e"></a>[`Inno.Rendering.GraphicsCapability.Indirect = 4`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L148) | Indirect draw and dispatch commands. | — | — |
| <a id="api-e67c173731b8a61c"></a>[`Inno.Rendering.GraphicsCapability.Instancing = 512`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L176) | Instanced draw input is supported. | — | — |
| <a id="api-634c8b3b8091d1c2"></a>[`Inno.Rendering.GraphicsCapability.None = 0`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L136) | No optional feature. | — | — |
| <a id="api-55ad586b0832520a"></a>[`Inno.Rendering.GraphicsCapability.ProceduralDraw = 65536`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L204) | Procedural draws using shader vertex identifiers are supported. | — | — |
| <a id="api-b699dbf10e66b06d"></a>[`Inno.Rendering.GraphicsCapability.StorageBuffer = 2`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L144) | Shader-readable and writable storage buffers. | — | — |
| <a id="api-40107dc445606a44"></a>[`Inno.Rendering.GraphicsCapability.StorageTexture = 262144`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L212) | Shader-readable and writable storage textures are supported. | — | — |
| <a id="api-2cccbfda3e3ff68d"></a>[`Inno.Rendering.GraphicsCapability.SwapChain = 1024`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L180) | Additional native presentation surfaces are supported. | — | — |
| <a id="api-88af4a3d122a7a6d"></a>[`Inno.Rendering.GraphicsCapability.Texture2DArray = 2048`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L184) | Two-dimensional texture arrays are supported. | — | — |
| <a id="api-d35e27f38462ae2d"></a>[`Inno.Rendering.GraphicsCapability.Texture3D = 4096`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L188) | Three-dimensional textures are supported. | — | — |
| <a id="api-22b651b4f31ee86c"></a>[`Inno.Rendering.GraphicsCapability.TextureBlit = 32`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L160) | Texture copy operations are supported directly. | — | — |
| <a id="api-2cc4ed82cb44c0aa"></a>[`Inno.Rendering.GraphicsCapability.TextureCubeArray = 8192`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L192) | Cubemap texture arrays are supported. | — | — |
| <a id="api-3487b8ef1175ea5f"></a>[`Inno.Rendering.GraphicsCapability.TextureReadback = 524288`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L216) | Asynchronous texture transfer from GPU memory to CPU-visible bytes. | — | — |
| <a id="api-62b4912a919cd00c"></a>[`Inno.Rendering.GraphicsCapability.VertexAttributeHalf = 16384`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L196) | Half-precision vertex attributes are supported. | — | — |
| <a id="api-7b855238b4ad1267"></a>[`Inno.Rendering.GraphicsCapability.VertexAttributeUInt10 = 32768`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L200) | Packed 10:10:10:2 vertex attributes are supported. | — | — |
| <a id="api-7cc0f9f5feada87f"></a>[`public enum Inno.Rendering.GraphicsCapability : int`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L131) | Declares backend-neutral optional graphics functionality. | — | — |

### `Inno.Rendering.GraphicsLimits`

<!-- api: T:Inno.Rendering.GraphicsLimits e100447812ed7c6882a48efd712db48fd0a722f4fbf9f11f37b2fefff857c0db -->
<!-- api: M:Inno.Rendering.GraphicsLimits.#ctor(System.Int32,System.Int32,System.Int32,System.Int32) 8eb2c2d93fc35687494ca0617f17d2c92278f731172eb6832bf72d9532d544d0 -->
<!-- api: P:Inno.Rendering.GraphicsLimits.maxViews d6441da1ccdd826884106c47775f79ee8cd81a1d4b7228f5b73f2dbff76d5156 -->
<!-- api: P:Inno.Rendering.GraphicsLimits.maxColorAttachments ad4d0f0229e01ef91b97dbf47b08333ad86e43ba5444e537b60cd2b732522738 -->
<!-- api: P:Inno.Rendering.GraphicsLimits.maxTextureSize 101e258c89f454bd06e463a38d914d2dcf5b0553429e00d0dc9a59bebab3dfa8 -->
<!-- api: P:Inno.Rendering.GraphicsLimits.maxComputeBindings 182e98a061c9ce36063c28974c376e988514eb762ab7c4b6f3ed82886f6254bb -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-51e557fa6a667f5a"></a>[`public Inno.Rendering.GraphicsLimits.GraphicsLimits(int maxViews, int maxColorAttachments, int maxTextureSize, int maxComputeBindings)`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L239) | Creates a graphics limits snapshot.<br>`maxViews`：Maximum logical backend views in one frame.<br>`maxColorAttachments`：Maximum color attachments in one raster pass.<br>`maxTextureSize`：Maximum two-dimensional texture extent.<br>`maxComputeBindings`：Maximum storage bindings in one compute pass. | — | — |
| <a id="api-7ebfdacb3d95a618"></a>[`public int Inno.Rendering.GraphicsLimits.maxColorAttachments { get; }`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L263) | Gets the maximum color attachments in one raster pass. | — | — |
| <a id="api-6757945de0519590"></a>[`public int Inno.Rendering.GraphicsLimits.maxComputeBindings { get; }`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L273) | Gets the maximum storage bindings in one compute pass. | — | — |
| <a id="api-417da0d0db4c40d4"></a>[`public int Inno.Rendering.GraphicsLimits.maxTextureSize { get; }`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L268) | Gets the maximum two-dimensional texture extent. | — | — |
| <a id="api-ee96d887ed172302"></a>[`public int Inno.Rendering.GraphicsLimits.maxViews { get; }`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L258) | Gets the maximum logical backend views in one frame. | — | — |
| <a id="api-55cf828dc3179762"></a>[`public sealed class Inno.Rendering.GraphicsLimits`](../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L222) | Captures device limits used to validate and compile render graphs. | — | — |

### `Inno.Rendering.GraphicsPipelineDescriptor`

<!-- api: T:Inno.Rendering.GraphicsPipelineDescriptor 105c0289c0ca30135be52268fdf8998f0dcc182162f1f5a207fd68f14eefcc02 -->
<!-- api: M:Inno.Rendering.GraphicsPipelineDescriptor.#ctor(System.ReadOnlySpan{System.Byte},System.ReadOnlySpan{System.Byte},System.Collections.Generic.IReadOnlyList{Inno.Rendering.RenderShaderBindingDescriptor},Inno.Rendering.RenderVertexLayout,Inno.Rendering.RenderRasterState) acdc8bfadad7b8d9c92385399471829792b5cf08b8b5d107bbce1d3204f02625 -->
<!-- api: P:Inno.Rendering.GraphicsPipelineDescriptor.vertexShader cc4db61e536eb7cf1b2bd8400870491affe42147a384bc2aa3a29fcc3b35458b -->
<!-- api: P:Inno.Rendering.GraphicsPipelineDescriptor.fragmentShader 3ed59486305974838c6f29cd7c70ee80e13d23fc2afde31473c01f9b98dde9f9 -->
<!-- api: P:Inno.Rendering.GraphicsPipelineDescriptor.bindings 385b0debe8c1387a95611a51f9e8d7065c730d9740fed6b32919998be55fefc6 -->
<!-- api: P:Inno.Rendering.GraphicsPipelineDescriptor.vertexLayout 65156e7208b9db7fd0e0d5f78a8ae146392e1f3e52a2fbb2779274d066feeb1b -->
<!-- api: P:Inno.Rendering.GraphicsPipelineDescriptor.rasterState 70aff2324b2640007e9975297f09bede74a2b9296ba78186e7558dd3bd086a23 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-51901aa24027938f"></a>[`public Inno.Rendering.GraphicsPipelineDescriptor.GraphicsPipelineDescriptor(System.ReadOnlySpan<byte> vertexShader, System.ReadOnlySpan<byte> fragmentShader, System.Collections.Generic.IReadOnlyList<Inno.Rendering.RenderShaderBindingDescriptor> bindings, Inno.Rendering.RenderVertexLayout? vertexLayout, Inno.Rendering.RenderRasterState? rasterState = null)`](../../src/services/rendering/Inno.Rendering/Device/GraphicsPipelineDescriptor.cs#L34) | Creates a graphics pipeline descriptor.<br>`vertexShader`：Target backend vertex shader binary.<br>`fragmentShader`：Target backend fragment shader binary.<br>`bindings`：Manifest-derived interface contract.<br>`vertexLayout`：Required mesh vertex layout, or null for procedural vertices.<br>`rasterState`：Fixed-function raster state. | — | — |
| <a id="api-8a21dbbee5e903c3"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.RenderShaderBindingDescriptor> Inno.Rendering.GraphicsPipelineDescriptor.bindings { get; }`](../../src/services/rendering/Inno.Rendering/Device/GraphicsPipelineDescriptor.cs#L73) | Gets the manifest-derived interface contract. | — | — |
| <a id="api-e0d89f88b98d542f"></a>[`public System.ReadOnlyMemory<byte> Inno.Rendering.GraphicsPipelineDescriptor.fragmentShader { get; }`](../../src/services/rendering/Inno.Rendering/Device/GraphicsPipelineDescriptor.cs#L68) | Gets the target backend fragment shader binary. | — | — |
| <a id="api-bef058d7506a8b90"></a>[`public Inno.Rendering.RenderRasterState Inno.Rendering.GraphicsPipelineDescriptor.rasterState { get; }`](../../src/services/rendering/Inno.Rendering/Device/GraphicsPipelineDescriptor.cs#L83) | Gets fixed-function raster state. | — | — |
| <a id="api-ae3872b40c79b7d2"></a>[`public Inno.Rendering.RenderVertexLayout? Inno.Rendering.GraphicsPipelineDescriptor.vertexLayout { get; }`](../../src/services/rendering/Inno.Rendering/Device/GraphicsPipelineDescriptor.cs#L78) | Gets the required mesh vertex layout, or null for procedural vertices. | — | — |
| <a id="api-aab91236bd358d11"></a>[`public System.ReadOnlyMemory<byte> Inno.Rendering.GraphicsPipelineDescriptor.vertexShader { get; }`](../../src/services/rendering/Inno.Rendering/Device/GraphicsPipelineDescriptor.cs#L63) | Gets the target backend vertex shader binary. | — | — |
| <a id="api-22a28559e2c03052"></a>[`public sealed class Inno.Rendering.GraphicsPipelineDescriptor`](../../src/services/rendering/Inno.Rendering/Device/GraphicsPipelineDescriptor.cs#L10) | Describes a graphics program candidate and reflected interface contract. | — | — |

### `Inno.Rendering.GraphicsPipelineHandle`

<!-- api: T:Inno.Rendering.GraphicsPipelineHandle d9ef714aa883f9f84b9206833a1a62e1914c3b38f2bc4eecfadf7664740e3123 -->
<!-- api: P:Inno.Rendering.GraphicsPipelineHandle.isValid 650c708a853eaf708cd7c2a8bd4e5ae7098013aa45eb4d0f68f25eb8a920871b -->
<!-- api: M:Inno.Rendering.GraphicsPipelineHandle.ToString a5be9a10b3c23033a7b0eecd19cd0103f22275247b4ea8eb6e6b99341a557763 -->
<!-- api: M:Inno.Rendering.GraphicsPipelineHandle.op_Inequality(Inno.Rendering.GraphicsPipelineHandle,Inno.Rendering.GraphicsPipelineHandle) 968a325e05392d0fbf3ad2c0052b1b726e46b71c4007b0f7d57dfd650ffd7971 -->
<!-- api: M:Inno.Rendering.GraphicsPipelineHandle.op_Equality(Inno.Rendering.GraphicsPipelineHandle,Inno.Rendering.GraphicsPipelineHandle) 16e480138542a4264ffd171cf88338e9fce6937bb0adde2dee006e55f156fb3d -->
<!-- api: M:Inno.Rendering.GraphicsPipelineHandle.GetHashCode 7709f776c97ab069d2bbc943656c383b9c9beb599ce716bc22226a8ad2c5ec6e -->
<!-- api: M:Inno.Rendering.GraphicsPipelineHandle.Equals(System.Object) 12b734b1e0048b54602b362aa261d6335a8f073d3286ee2aba6f41373f0bf679 -->
<!-- api: M:Inno.Rendering.GraphicsPipelineHandle.Equals(Inno.Rendering.GraphicsPipelineHandle) af7f05ec4856d79d41cb1ece872a12294ce20c8a439fe99a64599f9b0cb79ca1 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-49bf7628c437198c"></a>[`public bool Inno.Rendering.GraphicsPipelineHandle.Equals(Inno.Rendering.GraphicsPipelineHandle other)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L36) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-c62f660733c26c97"></a>[`public override bool Inno.Rendering.GraphicsPipelineHandle.Equals(object obj)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L36) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-8b16eb02212d6937"></a>[`public override int Inno.Rendering.GraphicsPipelineHandle.GetHashCode()`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L36) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-c5d922bca0b1c352"></a>[`public static bool Inno.Rendering.GraphicsPipelineHandle.operator ==(Inno.Rendering.GraphicsPipelineHandle left, Inno.Rendering.GraphicsPipelineHandle right)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L36) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-c7782c5b3188a153"></a>[`public static bool Inno.Rendering.GraphicsPipelineHandle.operator !=(Inno.Rendering.GraphicsPipelineHandle left, Inno.Rendering.GraphicsPipelineHandle right)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L36) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-bd2ee162fe3459ef"></a>[`public override string Inno.Rendering.GraphicsPipelineHandle.ToString()`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L36) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-301e565bd7d82bfc"></a>[`public bool Inno.Rendering.GraphicsPipelineHandle.isValid { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L52) | Gets whether the handle identifies a graphics pipeline. | — | — |
| <a id="api-7e90102b8e67963f"></a>[`public readonly record struct Inno.Rendering.GraphicsPipelineHandle : System.IEquatable<Inno.Rendering.GraphicsPipelineHandle>`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L36) | Identifies a backend-neutral graphics pipeline object. | — | — |

### `Inno.Rendering.GraphicsProgramArtifactCodec`

<!-- api: T:Inno.Rendering.GraphicsProgramArtifactCodec 6f0ba183f2727238443b31026ce2b8d6860aacf0c547955d4eb33451b3168941 -->
<!-- api: M:Inno.Rendering.GraphicsProgramArtifactCodec.Encode(Inno.Rendering.GraphicsPipelineDescriptor) 42d363c114eeabf93fc50e6ddaad79704e05f61c1e98158025f01a5cf54f4316 -->
<!-- api: M:Inno.Rendering.GraphicsProgramArtifactCodec.Decode(System.ReadOnlySpan{System.Byte},Inno.Rendering.RenderVertexLayout) ddcfbc1ba4887613f4763154c11e2d7ec3958d61e2edc5e5c3d2baa49a0c917c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2ea35f17a8d6704b"></a>[`public static Inno.Rendering.GraphicsPipelineDescriptor Inno.Rendering.GraphicsProgramArtifactCodec.Decode(System.ReadOnlySpan<byte> bytes, Inno.Rendering.RenderVertexLayout? vertexLayout = null)`](../../src/services/rendering/Inno.Rendering/Shaders/GraphicsProgramArtifactCodec.cs#L75) | Validates a complete device program artifact before constructing an owned pipeline descriptor.<br>返回：A detached descriptor containing the validated binaries, bindings, and raster state.<br>`bytes`：The complete current artifact, including its strict magic header.<br>`vertexLayout`：The caller's vertex layout, or null for a procedural pipeline. | — | `System.IO.InvalidDataException`：The input is truncated, oversized, malformed, or contains trailing data. |
| <a id="api-166424a5a01e892f"></a>[`public static byte[] Inno.Rendering.GraphicsProgramArtifactCodec.Encode(Inno.Rendering.GraphicsPipelineDescriptor descriptor)`](../../src/services/rendering/Inno.Rendering/Shaders/GraphicsProgramArtifactCodec.cs#L34) | Encodes owned program binaries, reflected bindings, and raster state for device distribution. Vertex layout belongs to the eventual caller and is not part of this artifact.<br>返回：Deterministic binary bytes owned by the caller.<br>`descriptor`：The compiled graphics program to distribute without any asset or material metadata. | — | `System.ArgumentNullException`：The descriptor is null.<br>`System.IO.InvalidDataException`：A stage, binding, or reflected name exceeds the distribution limits. |
| <a id="api-af950d762ea4c239"></a>[`public static class Inno.Rendering.GraphicsProgramArtifactCodec`](../../src/services/rendering/Inno.Rendering/Shaders/GraphicsProgramArtifactCodec.cs#L10) | Transports compiled graphics programs and device binding facts independently of authored assets. | — | — |

### `Inno.Rendering.IRenderDevice`

<!-- api: T:Inno.Rendering.IRenderDevice 2e232d27973c46fdfb5abf8a7042a7a261159cc459b385b38ff2d6b93355e287 -->
<!-- api: P:Inno.Rendering.IRenderDevice.capabilities 04f2707b3dd4fc790d2fd08d64d684269f4349fa93f38e8b3d24b1e5c7cbf162 -->
<!-- api: P:Inno.Rendering.IRenderDevice.generation 9da068b5df24a13b67585c5b6a5d85ebec83ec7ddb8359fc7dc9c8b667695917 -->
<!-- api: P:Inno.Rendering.IRenderDevice.primaryPresentationSize 751a12fa832d819789cc35bb2f8f34357184308c4d395a4d82f774b24eee18ef -->
<!-- api: P:Inno.Rendering.IRenderDevice.primaryPresentationEncodesSrgb 3179923e4a19aac9b4e9bc557bc972df97a97e773324f01af76da080b65e2b54 -->
<!-- api: P:Inno.Rendering.IRenderDevice.frameCounters 31f40b05304c24a59a0a34c804af8627f69c61e540a8185c9b3e1b3787e2c1e2 -->
<!-- api: P:Inno.Rendering.IRenderDevice.allocationCounters e71412544ddcced883aa9a6d7b374dc5b48ca5eb26af116e268ea6fa03c20807 -->
<!-- api: M:Inno.Rendering.IRenderDevice.BeginFrame 7222b89edf4b8f47f66dcb244e1593c0cf6ad513a62ca8f0f4eee88d24e7b924 -->
<!-- api: M:Inno.Rendering.IRenderDevice.Execute(Inno.Rendering.CompiledRenderGraph,System.UInt64) c1c238f7326bdada142502ed4dc0e542f9d28ca09887d87b6b596cf91269b78e -->
<!-- api: M:Inno.Rendering.IRenderDevice.EndFrame 5034dabd25780165fbff3d1be616860fa6e93c3ca485c8d3c60325732384f079 -->
<!-- api: M:Inno.Rendering.IRenderDevice.SetPrimaryPresentationSize(System.Nullable{Inno.Rendering.RenderPresentationSize}) 0fe64bb06b1dbdda1c8866ae862ac43cc4a377b1c880bd86c446f5f152d7c107 -->
<!-- api: M:Inno.Rendering.IRenderDevice.SetVerticalSync(System.Boolean) be33e7f255dd0cc92762919dd27d17fafe8b785b471495e3fc52e7fe4e7f1fa7 -->
<!-- api: M:Inno.Rendering.IRenderDevice.CreateTexture(Inno.Rendering.RenderTextureDescriptor,System.String) ed0d8f2658988a52ef4ee9d1ddbd11ee27a9084e7a98d702d88bc3ddb502a1d0 -->
<!-- api: M:Inno.Rendering.IRenderDevice.CreateTexture(Inno.Rendering.RenderTextureContainer,System.ReadOnlySpan{System.Byte},System.Boolean,System.String) 319e0a5560c753763d153084d0788ce588e31affcb781ca924bac1f3530493e6 -->
<!-- api: M:Inno.Rendering.IRenderDevice.UpdateTexture(Inno.Rendering.PersistentTextureHandle,System.ReadOnlySpan{System.Byte},System.Int32,System.Int32) 2dfad47da27a276ca83e047e8db70e2ba1d8ef84b57a51f2c6cd1a101c237796 -->
<!-- api: M:Inno.Rendering.IRenderDevice.UpdateTextureRegion(Inno.Rendering.PersistentTextureHandle,Inno.Rendering.RenderTextureRegion,System.ReadOnlySpan{System.Byte}) bfd104c950d9da4655d581433697f1a7f455255a468b9209d3d75880d67edf9c -->
<!-- api: M:Inno.Rendering.IRenderDevice.BeginTextureReadback(Inno.Rendering.PersistentTextureHandle,System.Int32) 5c3d55f6cf5a458b68d290bfe010975261fc40a310203d811e0c757e72f47faf -->
<!-- api: M:Inno.Rendering.IRenderDevice.TryGetTextureReadback(Inno.Rendering.RenderTextureReadbackHandle,Inno.Rendering.RenderTextureReadbackResult@) 44c1b46237220f7fa09558269d9c5ea9efc8625bdfa9f1a619954c362001521a -->
<!-- api: M:Inno.Rendering.IRenderDevice.CancelTextureReadback(Inno.Rendering.RenderTextureReadbackHandle) fcdf39085b14edf4e8af9976f14ab055b3f254c70bf5e1ab842de51c5969aba7 -->
<!-- api: M:Inno.Rendering.IRenderDevice.DestroyTexture(Inno.Rendering.PersistentTextureHandle) 5e589ea258990d2b471b0aa9f09cb795996f51f01ac43eeeb4d0765693dad9d5 -->
<!-- api: M:Inno.Rendering.IRenderDevice.CreateBuffer(Inno.Rendering.PersistentBufferDescriptor,System.ReadOnlySpan{System.Byte},System.String) a786873d4ff0e160624dea9d5b9d249597eb84b3f475c13cccc55d3654ddc15e -->
<!-- api: M:Inno.Rendering.IRenderDevice.UpdateBuffer(Inno.Rendering.PersistentBufferHandle,System.ReadOnlySpan{System.Byte},System.Int32) bfdc0c3dc80108d17144359a408f377792fcc8d014a94b18ca295a314affc66b -->
<!-- api: M:Inno.Rendering.IRenderDevice.DestroyBuffer(Inno.Rendering.PersistentBufferHandle) 1a8cc65dd61c0237b9e285c3e523965ddc149a1d6964ff3be5922c8ac7e1c286 -->
<!-- api: M:Inno.Rendering.IRenderDevice.CreateGraphicsPipeline(Inno.Rendering.GraphicsPipelineDescriptor,System.String) 58f1ab2804084a700c63d3b6281ac0a82cea1dba6a94ac0bba1fdf9e579f6aaf -->
<!-- api: M:Inno.Rendering.IRenderDevice.DestroyGraphicsPipeline(Inno.Rendering.GraphicsPipelineHandle) a1917a2a112965ce44d96ded3b839e41c4493e4e42a8719287bf2659e85fa911 -->
<!-- api: M:Inno.Rendering.IRenderDevice.CreateComputePipeline(Inno.Rendering.ComputePipelineDescriptor,System.String) 6288c129bc7a302330d432990cec68460998584b3a7560504e44c97d1e568e08 -->
<!-- api: M:Inno.Rendering.IRenderDevice.DestroyComputePipeline(Inno.Rendering.ComputePipelineHandle) 5da91fb68c849f8f775a61b84d98f0c1a20fa9acffb67ea7230288af92e12865 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-ed0ae98b39966189"></a>[`void Inno.Rendering.IRenderDevice.BeginFrame()`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L91) | Begins the sole API-thread frame scope and processes queued resource work. | — | — |
| <a id="api-946e1e748eb3b284"></a>[`Inno.Rendering.RenderTextureReadbackHandle Inno.Rendering.IRenderDevice.BeginTextureReadback(Inno.Rendering.PersistentTextureHandle texture, int mipLevel = 0)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L234) | Begins an asynchronous readback of one complete persistent texture mip.<br>返回：An opaque device-generation readback handle.<br>`texture`：Texture created with Inno.Rendering.RenderTextureUsage.Readback.<br>`mipLevel`：Zero-based mip level. | — | — |
| <a id="api-6f496709199b3a6e"></a>[`void Inno.Rendering.IRenderDevice.CancelTextureReadback(Inno.Rendering.RenderTextureReadbackHandle readback)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L262) | Stops retaining a readback result that is no longer needed by its caller.<br>`readback`：Pending readback handle. | — | — |
| <a id="api-28dce4fac5bde174"></a>[`Inno.Rendering.PersistentBufferHandle Inno.Rendering.IRenderDevice.CreateBuffer(Inno.Rendering.PersistentBufferDescriptor descriptor, System.ReadOnlySpan<byte> initialData, string name)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L287) | Creates a persistent vertex, index or storage buffer at a frame safety point.<br>返回：An opaque device-generation buffer handle.<br>`descriptor`：Buffer capacity and interpretation.<br>`initialData`：Optional complete initial contents.<br>`name`：Debug and diagnostic name. | — | — |
| <a id="api-4425812a716db4b6"></a>[`Inno.Rendering.ComputePipelineHandle Inno.Rendering.IRenderDevice.CreateComputePipeline(Inno.Rendering.ComputePipelineDescriptor descriptor, string name)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L356) | Creates and reflection-validates a compute pipeline at a frame safety point.<br>返回：An opaque device-generation compute pipeline handle.<br>`descriptor`：Compiled stage and interface contract.<br>`name`：Debug and diagnostic name. | — | — |
| <a id="api-d99aebb151d943f9"></a>[`Inno.Rendering.GraphicsPipelineHandle Inno.Rendering.IRenderDevice.CreateGraphicsPipeline(Inno.Rendering.GraphicsPipelineDescriptor descriptor, string name)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L331) | Creates and reflection-validates a graphics pipeline at a frame safety point.<br>返回：An opaque device-generation graphics pipeline handle.<br>`descriptor`：Compiled stages, interface, layout and raster state.<br>`name`：Debug and diagnostic name. | — | — |
| <a id="api-0d9794fd74dbf38b"></a>[`Inno.Rendering.PersistentTextureHandle Inno.Rendering.IRenderDevice.CreateTexture(Inno.Rendering.RenderTextureContainer container, System.ReadOnlySpan<byte> data, bool sRgb, string name)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L174) | Creates a persistent sampled texture from a validated portable container.<br>返回：An opaque device-generation handle.<br>`container`：Portable texture container kind.<br>`data`：Complete encoded container bytes.<br>`sRgb`：Whether sampling decodes the stored color from sRGB.<br>`name`：Debug and diagnostic name. | — | `System.NotSupportedException`：Thrown when the device cannot consume encoded containers. |
| <a id="api-c93a63a06155305d"></a>[`Inno.Rendering.PersistentTextureHandle Inno.Rendering.IRenderDevice.CreateTexture(Inno.Rendering.RenderTextureDescriptor descriptor, string name)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L148) | Creates a persistent texture at a frame safety point.<br>返回：An opaque device-generation handle.<br>`descriptor`：Texture requirements.<br>`name`：Debug and diagnostic name. | — | — |
| <a id="api-c129364a3af89017"></a>[`void Inno.Rendering.IRenderDevice.DestroyBuffer(Inno.Rendering.PersistentBufferHandle buffer)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L317) | Queues a persistent buffer for delayed GPU-safe destruction.<br>`buffer`：Buffer owned by this device generation. | — | — |
| <a id="api-c50a1bdde1e085e2"></a>[`void Inno.Rendering.IRenderDevice.DestroyComputePipeline(Inno.Rendering.ComputePipelineHandle pipeline)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L367) | Queues a compute pipeline for delayed GPU-safe destruction.<br>`pipeline`：Pipeline owned by this device generation. | — | — |
| <a id="api-8db4bf0f2caa6471"></a>[`void Inno.Rendering.IRenderDevice.DestroyGraphicsPipeline(Inno.Rendering.GraphicsPipelineHandle pipeline)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L342) | Queues a graphics pipeline for delayed GPU-safe destruction.<br>`pipeline`：Pipeline owned by this device generation. | — | — |
| <a id="api-406762cbb1f30ca3"></a>[`void Inno.Rendering.IRenderDevice.DestroyTexture(Inno.Rendering.PersistentTextureHandle texture)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L270) | Queues a persistent texture for delayed GPU-safe destruction.<br>`texture`：Texture owned by this device generation. | — | — |
| <a id="api-912d150c3a751d0d"></a>[`uint Inno.Rendering.IRenderDevice.EndFrame()`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L113) | Ends all encoders and advances the graphics backend exactly once.<br>返回：The backend frame number after submission. | — | — |
| <a id="api-4a456cbafe75c605"></a>[`void Inno.Rendering.IRenderDevice.Execute(Inno.Rendering.CompiledRenderGraph graph, ulong frameIndex)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L102) | Executes one compiled graph without presenting or advancing another frame.<br>`graph`：Compiled graph to execute.<br>`frameIndex`：Monotonic engine render frame index. | — | — |
| <a id="api-10d79fa94b973db1"></a>[`void Inno.Rendering.IRenderDevice.SetPrimaryPresentationSize(Inno.Rendering.RenderPresentationSize? size)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L124) | Queues primary output availability and its real pixel extent for the next frame safety point.<br>`size`：A valid drawable pixel extent, or null when the primary surface cannot currently present. | — | `System.ArgumentException`：The assigned extent has zero or negative dimensions, including a default struct value. |
| <a id="api-e9c20c2aa3e763c9"></a>[`void Inno.Rendering.IRenderDevice.SetVerticalSync(bool enabled)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L134) | Queues display synchronization policy on the API thread for the next frame boundary. Every backend must implement both policies. Repeated values are idempotent and must not reset an active pass. Windowless devices retain the requested policy without performing presentation.<br>`enabled`：Whether presentation should wait for display refresh when a presentation surface is present. | — | — |
| <a id="api-bc58d4df3b98f64f"></a>[`bool Inno.Rendering.IRenderDevice.TryGetTextureReadback(Inno.Rendering.RenderTextureReadbackHandle readback, out Inno.Rendering.RenderTextureReadbackResult? result)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L251) | Tries to complete one previously requested texture readback.<br>返回：true when the result completed and was consumed.<br>`readback`：Readback handle returned by Inno.Rendering.IRenderDevice.BeginTextureReadback(Inno.Rendering.PersistentTextureHandle,System.Int32).<br>`result`：Receives immutable CPU-visible bytes when complete. | — | — |
| <a id="api-ddc9da1d27188f9f"></a>[`void Inno.Rendering.IRenderDevice.UpdateBuffer(Inno.Rendering.PersistentBufferHandle buffer, System.ReadOnlySpan<byte> data, int startElement = 0)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L305) | Replaces a contiguous range in a dynamic persistent buffer at a frame safety point.<br>`buffer`：Dynamic buffer owned by this device generation.<br>`data`：Complete replacement elements.<br>`startElement`：Zero-based destination element. | — | — |
| <a id="api-d534cde82c836bba"></a>[`void Inno.Rendering.IRenderDevice.UpdateTexture(Inno.Rendering.PersistentTextureHandle texture, System.ReadOnlySpan<byte> data, int mipLevel = 0, int arrayLayer = 0)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L197) | Replaces one complete mip and addressable layer, slice, or cubemap face.<br>`texture`：Texture owned by this device generation.<br>`data`：Tightly packed complete subresource bytes.<br>`mipLevel`：Zero-based mip level.<br>`arrayLayer`：Zero-based 2D array layer, 3D Z slice, or flattened cubemap face using cube-layer * 6 + face. | — | — |
| <a id="api-961326d8086a93f2"></a>[`void Inno.Rendering.IRenderDevice.UpdateTextureRegion(Inno.Rendering.PersistentTextureHandle texture, Inno.Rendering.RenderTextureRegion region, System.ReadOnlySpan<byte> data)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L216) | Replaces a tightly packed rectangular region in one persistent texture subresource.<br>`texture`：Texture owned by this device generation.<br>`region`：Destination mip, texel rectangle, and layer range.<br>`data`：Tightly packed region bytes. | — | — |
| <a id="api-cf19a383f6824b2e"></a>[`Inno.Rendering.RenderDeviceAllocationCounters? Inno.Rendering.IRenderDevice.allocationCounters { get; }`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L86) | Gets cumulative native transient allocations for this device generation on the API thread. A null snapshot means allocation accounting is unavailable, not that no resources were allocated. | — | — |
| <a id="api-2ba1d182763756d7"></a>[`Inno.Rendering.GraphicsCapabilities Inno.Rendering.IRenderDevice.capabilities { get; }`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L59) | Gets immutable capabilities for the active device generation. | — | — |
| <a id="api-8661b23bd7e7c393"></a>[`Inno.Rendering.RenderDeviceFrameCounters Inno.Rendering.IRenderDevice.frameCounters { get; }`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L80) | Gets command counts recorded since the latest Inno.Rendering.IRenderDevice.BeginFrame call. Backends that cannot provide command accounting return zero counters. | — | — |
| <a id="api-6295e17d00504ce2"></a>[`uint Inno.Rendering.IRenderDevice.generation { get; }`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L64) | Gets the non-zero device generation used to reject stale persistent handles. | — | — |
| <a id="api-8f9e6fcf04a1919c"></a>[`bool Inno.Rendering.IRenderDevice.primaryPresentationEncodesSrgb { get; }`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L74) | Gets whether the primary presentation target encodes linear RGB to sRGB during writes. | — | — |
| <a id="api-eecc48cd8b09a895"></a>[`Inno.Rendering.RenderPresentationSize? Inno.Rendering.IRenderDevice.primaryPresentationSize { get; }`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L69) | Gets the real primary presentation extent in physical pixels, or null while no output is available. | — | — |
| <a id="api-f1d42b5f7bc80e5f"></a>[`public interface Inno.Rendering.IRenderDevice : System.IDisposable`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L54) | Owns one graphics backend generation and its frame submission boundary. | — | — |

### `Inno.Rendering.IRenderFrameGraphContributor`

<!-- api: T:Inno.Rendering.IRenderFrameGraphContributor 833844da2b6caffc8b0d7eacb47e83b622ce9ebce88dd09448d83cbc5d68a157 -->
<!-- api: M:Inno.Rendering.IRenderFrameGraphContributor.PrepareFrame(System.UInt64) 7c7df4f2e50eb9cf90980a7717973c91afe3579bcebb4b214895ede78d666fff -->
<!-- api: M:Inno.Rendering.IRenderFrameGraphContributor.AddRenderPasses(Inno.Rendering.RenderGraphBuilder,System.UInt64) 49cc8b2f8e64fd57cecaff2272a2bd100183c1eec50a2e63a1da76f21864a7b1 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7d0eb24f3b0b4377"></a>[`void Inno.Rendering.IRenderFrameGraphContributor.AddRenderPasses(Inno.Rendering.RenderGraphBuilder graph, ulong frameIndex)`](../../src/services/rendering/Inno.Rendering/Graph/IRenderFrameGraphContributor.cs#L25) | Adds frame-scoped passes after all user render-request graphs have executed.<br>`graph`：Shared final graph builder.<br>`frameIndex`：Monotonic engine render frame index. | — | — |
| <a id="api-468a089736087602"></a>[`void Inno.Rendering.IRenderFrameGraphContributor.PrepareFrame(ulong frameIndex)`](../../src/services/rendering/Inno.Rendering/Graph/IRenderFrameGraphContributor.cs#L14) | Applies queued resource changes at the current frame safety point.<br>`frameIndex`：Monotonic engine render frame index. | — | — |
| <a id="api-ff771b8a6a525c8b"></a>[`public interface Inno.Rendering.IRenderFrameGraphContributor`](../../src/services/rendering/Inno.Rendering/Graph/IRenderFrameGraphContributor.cs#L6) | Adds transient frame-final work, such as UI composition, without owning or advancing the graphics frame. | — | — |

### `Inno.Rendering.IRenderFrameUploadService`

<!-- api: T:Inno.Rendering.IRenderFrameUploadService 228d21c92971d8e881a66390bc838e9ab04b2491600de74371afbade4e4ca22d -->
<!-- api: M:Inno.Rendering.IRenderFrameUploadService.UploadBuffer(Inno.Rendering.RenderBufferUploadDescriptor,System.ReadOnlyMemory{System.Byte},System.String) 7c0efbb9d048783adc0482ca0498b755cbb5eaf82bba28a0d39cd96056725364 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-cb9fa03c812d0334"></a>[`Inno.Rendering.RenderBufferSlice Inno.Rendering.IRenderFrameUploadService.UploadBuffer(Inno.Rendering.RenderBufferUploadDescriptor descriptor, System.ReadOnlyMemory<byte> data, string name)`](../../src/services/rendering/Inno.Rendering/IRenderFrameUploadService.cs#L27) | Uploads complete elements and returns a slice valid only during the current frame.<br>返回：A current-frame slice covering the uploaded elements.<br>`descriptor`：Element interpretation and permitted GPU uses.<br>`data`：Tightly packed complete element bytes.<br>`name`：Diagnostic name used when a backing page is allocated. | — | — |
| <a id="api-f6fdd4b380de8071"></a>[`public interface Inno.Rendering.IRenderFrameUploadService`](../../src/services/rendering/Inno.Rendering/IRenderFrameUploadService.cs#L10) | Streams immutable CPU data into reusable GPU pages for the current frame. | — | — |

### `Inno.Rendering.IRenderGraphBackend`

<!-- api: T:Inno.Rendering.IRenderGraphBackend fe5a52a88aafd151a96987870145f00bc617e27c5716b4d533eac23f3da9447e -->
<!-- api: M:Inno.Rendering.IRenderGraphBackend.BeginGraph(Inno.Rendering.CompiledRenderGraph) 2fd6b57c578529195a47e50df21fb02ad89f5a0a6f5ee3eae7e63138552bcee5 -->
<!-- api: M:Inno.Rendering.IRenderGraphBackend.BeginPass(Inno.Rendering.CompiledRenderPass) e5089bd2193a39e15da5e4bb65cae2b15c3c94e792272e49fdc305bce23b7e7b -->
<!-- api: M:Inno.Rendering.IRenderGraphBackend.EndPass(Inno.Rendering.CompiledRenderPass) c226a6c903bbcf6a550620e8eefc40af083f941289592d79bce0d8da029ad72d -->
<!-- api: M:Inno.Rendering.IRenderGraphBackend.EndGraph(Inno.Rendering.CompiledRenderGraph) c442f1fd72ab6941d2622ea34ebba75b0c1e0366cb8542d981cb830e3b0982b2 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8be4475fc9912554"></a>[`void Inno.Rendering.IRenderGraphBackend.BeginGraph(Inno.Rendering.CompiledRenderGraph graph)`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L329) | Begins execution and prepares transient allocations.<br>`graph`：Compiled graph to execute. | — | — |
| <a id="api-4467008dfe3e1f9e"></a>[`Inno.Rendering.RenderCommandEncoder Inno.Rendering.IRenderGraphBackend.BeginPass(Inno.Rendering.CompiledRenderPass pass)`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L340) | Begins one scheduled pass and acquires its command encoder.<br>返回：A command encoder scoped to this pass.<br>`pass`：Scheduled pass metadata. | — | — |
| <a id="api-0177fd3ce799f0f6"></a>[`void Inno.Rendering.IRenderGraphBackend.EndGraph(Inno.Rendering.CompiledRenderGraph graph)`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L356) | Ends graph execution without presenting an additional frame.<br>`graph`：Compiled graph that finished or failed. | — | — |
| <a id="api-21a4f8770c98be9d"></a>[`void Inno.Rendering.IRenderGraphBackend.EndPass(Inno.Rendering.CompiledRenderPass pass)`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L348) | Ends one scheduled pass and releases its encoder.<br>`pass`：Scheduled pass metadata. | — | — |
| <a id="api-a6579cd4583f153f"></a>[`public interface Inno.Rendering.IRenderGraphBackend`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L321) | Bridges compiled graph execution to a concrete graphics backend. | — | — |

### `Inno.Rendering.IRenderLayerCompositionProgramProvider`

<!-- api: T:Inno.Rendering.IRenderLayerCompositionProgramProvider 9a24592766178bccba12b882344ba7461bd522a5a3a488e2e711fed26397b2cd -->
<!-- api: M:Inno.Rendering.IRenderLayerCompositionProgramProvider.CreateDescriptor(Inno.Rendering.GraphicsCapabilities,Inno.Rendering.RenderVertexLayout) 8fecb15d7d3ff4a1ee4b45004f472881d1a79fc68ad1ff17c0ec4290d0b032d1 -->
<!-- api: M:Inno.Rendering.IRenderLayerCompositionProgramProvider.CreateOutputTransferDescriptor(Inno.Rendering.GraphicsCapabilities,Inno.Rendering.RenderVertexLayout) fd63344120ee55a08c565fe8eba11785729556df317b3aacc9778202baa4757c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-311c36ed6bf13ee2"></a>[`Inno.Rendering.GraphicsPipelineDescriptor Inno.Rendering.IRenderLayerCompositionProgramProvider.CreateDescriptor(Inno.Rendering.GraphicsCapabilities capabilities, Inno.Rendering.RenderVertexLayout vertexLayout)`](../../src/services/rendering/Inno.Rendering/Device/IRenderLayerCompositionProgramProvider.cs#L20) | Creates a pipeline descriptor compatible with the supplied fullscreen vertex layout.<br>返回：A pipeline descriptor that samples a texture named s_tex with premultiplied alpha.<br>`capabilities`：Capabilities of the device that will own the pipeline.<br>`vertexLayout`：Layout of the fullscreen quad submitted by the runtime. | — | — |
| <a id="api-e3391fa8acb5df5e"></a>[`Inno.Rendering.GraphicsPipelineDescriptor Inno.Rendering.IRenderLayerCompositionProgramProvider.CreateOutputTransferDescriptor(Inno.Rendering.GraphicsCapabilities capabilities, Inno.Rendering.RenderVertexLayout vertexLayout)`](../../src/services/rendering/Inno.Rendering/Device/IRenderLayerCompositionProgramProvider.cs#L37) | Creates the final transfer pipeline for a presentation target without automatic sRGB encoding.<br>返回：An opaque pipeline descriptor that encodes premultiplied linear color to sRGB.<br>`capabilities`：Capabilities of the device that will own the pipeline.<br>`vertexLayout`：Layout of the fullscreen quad submitted by the runtime. | — | — |
| <a id="api-0ac37b19a1ba602c"></a>[`public interface Inno.Rendering.IRenderLayerCompositionProgramProvider`](../../src/services/rendering/Inno.Rendering/Device/IRenderLayerCompositionProgramProvider.cs#L6) | Supplies a backend-specific graphics program for compositing ordered render layers. | — | — |

### `Inno.Rendering.PersistentBufferDescriptor`

<!-- api: T:Inno.Rendering.PersistentBufferDescriptor 089bb24feeca1f98c5cd2099c348b1d2fea65637184affcb14c7da7873d66ab9 -->
<!-- api: M:Inno.Rendering.PersistentBufferDescriptor.#ctor(Inno.Rendering.RenderBufferDescriptor,Inno.Rendering.RenderVertexLayout,Inno.Rendering.RenderIndexFormat) cb58ec5d1856d9a69be3bf7ea4f485bccaa10c80f7ec93d0177214f1c95da565 -->
<!-- api: P:Inno.Rendering.PersistentBufferDescriptor.buffer 5f350d860950780a28d3d28da4580eb2b807021bf99ae4257ed535c05d12c852 -->
<!-- api: P:Inno.Rendering.PersistentBufferDescriptor.vertexLayout f43ece25c1d89f801fd9055115a7e673956874e74d738581285fd7c33aea4603 -->
<!-- api: P:Inno.Rendering.PersistentBufferDescriptor.indexFormat a858001599dd763504771e324d7ad9ba00917c3a209111a505d9fbf00042a4d2 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-eebe3e73ca2d42db"></a>[`public Inno.Rendering.PersistentBufferDescriptor.PersistentBufferDescriptor(Inno.Rendering.RenderBufferDescriptor buffer, Inno.Rendering.RenderVertexLayout? vertexLayout = null, Inno.Rendering.RenderIndexFormat indexFormat = Inno.Rendering.RenderIndexFormat.UInt32)`](../../src/services/rendering/Inno.Rendering/Device/PersistentBufferDescriptor.cs#L24) | Creates a persistent buffer descriptor.<br>`buffer`：Capacity and usage.<br>`vertexLayout`：Required interleaved layout for vertex buffers.<br>`indexFormat`：Index representation for index buffers. | — | — |
| <a id="api-ff128b7742111c05"></a>[`public Inno.Rendering.RenderBufferDescriptor Inno.Rendering.PersistentBufferDescriptor.buffer { get; }`](../../src/services/rendering/Inno.Rendering/Device/PersistentBufferDescriptor.cs#L66) | Gets buffer capacity and usage. | — | — |
| <a id="api-ea564714d5d883f8"></a>[`public Inno.Rendering.RenderIndexFormat Inno.Rendering.PersistentBufferDescriptor.indexFormat { get; }`](../../src/services/rendering/Inno.Rendering/Device/PersistentBufferDescriptor.cs#L76) | Gets the index representation when this is an index buffer. | — | — |
| <a id="api-faab664b5432cbe1"></a>[`public Inno.Rendering.RenderVertexLayout? Inno.Rendering.PersistentBufferDescriptor.vertexLayout { get; }`](../../src/services/rendering/Inno.Rendering/Device/PersistentBufferDescriptor.cs#L71) | Gets the vertex layout when this is a vertex buffer. | — | — |
| <a id="api-1ab85b8e47159916"></a>[`public sealed class Inno.Rendering.PersistentBufferDescriptor`](../../src/services/rendering/Inno.Rendering/Device/PersistentBufferDescriptor.cs#L10) | Describes a persistent buffer and any vertex/index interpretation required at creation. | — | — |

### `Inno.Rendering.PersistentBufferHandle`

<!-- api: T:Inno.Rendering.PersistentBufferHandle 0914b56482b8d87bd99327df316bca6d40b3b6152c8b8b8189e71f0dc246f9ba -->
<!-- api: P:Inno.Rendering.PersistentBufferHandle.isValid 5a8accf19513685f19ceba3cf3f7cb2eb1eefc124580d1ccb6af353433507a27 -->
<!-- api: M:Inno.Rendering.PersistentBufferHandle.ToString 31460c643dd005a878b5bdc87cd68864ee900e3a5bac893ed4fc6781dd4e786e -->
<!-- api: M:Inno.Rendering.PersistentBufferHandle.op_Inequality(Inno.Rendering.PersistentBufferHandle,Inno.Rendering.PersistentBufferHandle) 727b1d39bb6847a8c0fb59380172029d8ca1f0fbf049010e6cb62a1417c42efd -->
<!-- api: M:Inno.Rendering.PersistentBufferHandle.op_Equality(Inno.Rendering.PersistentBufferHandle,Inno.Rendering.PersistentBufferHandle) 11ebb4b0bb0d679d35a075eb8d2b3f365afb2f3fbb19577d80ab548e4552eaad -->
<!-- api: M:Inno.Rendering.PersistentBufferHandle.GetHashCode ce4359eb631e9bc3f414797e185c42559f249c332ff33b0933865f1bcf26cdd9 -->
<!-- api: M:Inno.Rendering.PersistentBufferHandle.Equals(System.Object) 1632562e87c53cee4dfbd25e1b145269cef08bc59830bf504f9fefb656a188b8 -->
<!-- api: M:Inno.Rendering.PersistentBufferHandle.Equals(Inno.Rendering.PersistentBufferHandle) 9272e0547a398280dd4d9259d5a6e05d9213f812a48e2b7d85bece31a2bb202d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-40faac0e88be6524"></a>[`public bool Inno.Rendering.PersistentBufferHandle.Equals(Inno.Rendering.PersistentBufferHandle other)`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L515) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-0addca786eb40386"></a>[`public override bool Inno.Rendering.PersistentBufferHandle.Equals(object obj)`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L515) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-f0e5a58b6642ffb3"></a>[`public override int Inno.Rendering.PersistentBufferHandle.GetHashCode()`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L515) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-3580b91d1ec8c356"></a>[`public static bool Inno.Rendering.PersistentBufferHandle.operator ==(Inno.Rendering.PersistentBufferHandle left, Inno.Rendering.PersistentBufferHandle right)`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L515) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-4f3bf4172a1f5e1f"></a>[`public static bool Inno.Rendering.PersistentBufferHandle.operator !=(Inno.Rendering.PersistentBufferHandle left, Inno.Rendering.PersistentBufferHandle right)`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L515) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-221ca15fc4b4a609"></a>[`public override string Inno.Rendering.PersistentBufferHandle.ToString()`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L515) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-06c03535e65729a9"></a>[`public bool Inno.Rendering.PersistentBufferHandle.isValid { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L531) | Gets whether the handle identifies a device buffer. | — | — |
| <a id="api-c7ca9847c31fd315"></a>[`public readonly record struct Inno.Rendering.PersistentBufferHandle : System.IEquatable<Inno.Rendering.PersistentBufferHandle>`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L515) | Identifies a persistent device buffer without exposing a backend-native handle. | — | — |

### `Inno.Rendering.PersistentTextureHandle`

<!-- api: T:Inno.Rendering.PersistentTextureHandle 477adcb4d607a4c221e8a8350223677b3eb390e3e9d003d3bafeb176dabb6d28 -->
<!-- api: P:Inno.Rendering.PersistentTextureHandle.isValid e3d67825d6136f973d3ec6806ca132bd5da65cfc8051773bbc7f5343135a772a -->
<!-- api: M:Inno.Rendering.PersistentTextureHandle.ToString a9939624b2de6a9d6f82f58e2341a36ab83bfa6f146e4329981f595ecd3f1595 -->
<!-- api: M:Inno.Rendering.PersistentTextureHandle.op_Inequality(Inno.Rendering.PersistentTextureHandle,Inno.Rendering.PersistentTextureHandle) e1580d54559f06abe4657e32814a503125bd9bd3280fd7616c1e05f3822085cc -->
<!-- api: M:Inno.Rendering.PersistentTextureHandle.op_Equality(Inno.Rendering.PersistentTextureHandle,Inno.Rendering.PersistentTextureHandle) 859b15d04d0088935653f7410708bb3c6388a088d36be633c1695625f73213ff -->
<!-- api: M:Inno.Rendering.PersistentTextureHandle.GetHashCode da1ac56a78aea31a19371d9a9ce2e3059170b33b897ca00a718e1d9448047a34 -->
<!-- api: M:Inno.Rendering.PersistentTextureHandle.Equals(System.Object) ca9191f91d778fd2ff5931fcecd221c66c60f3df64b30ffcc1b3c8ffaa1981a8 -->
<!-- api: M:Inno.Rendering.PersistentTextureHandle.Equals(Inno.Rendering.PersistentTextureHandle) 2e18fd80246a1013d11f05222a965f2b2fbed7248a1b094ee1ea38216063bdef -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-043ca7ee582c3087"></a>[`public bool Inno.Rendering.PersistentTextureHandle.Equals(Inno.Rendering.PersistentTextureHandle other)`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L493) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-3f0e9f0087b152e5"></a>[`public override bool Inno.Rendering.PersistentTextureHandle.Equals(object obj)`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L493) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-1959cf4755c8e9b7"></a>[`public override int Inno.Rendering.PersistentTextureHandle.GetHashCode()`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L493) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-ff60a1f9fa13bab7"></a>[`public static bool Inno.Rendering.PersistentTextureHandle.operator ==(Inno.Rendering.PersistentTextureHandle left, Inno.Rendering.PersistentTextureHandle right)`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L493) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-2cbb9a29d20d1c74"></a>[`public static bool Inno.Rendering.PersistentTextureHandle.operator !=(Inno.Rendering.PersistentTextureHandle left, Inno.Rendering.PersistentTextureHandle right)`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L493) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-a635b6ca3aece255"></a>[`public override string Inno.Rendering.PersistentTextureHandle.ToString()`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L493) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-6148d2422c1e36d9"></a>[`public bool Inno.Rendering.PersistentTextureHandle.isValid { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L509) | Gets whether the handle identifies a device texture. | — | — |
| <a id="api-ca0705af48c16077"></a>[`public readonly record struct Inno.Rendering.PersistentTextureHandle : System.IEquatable<Inno.Rendering.PersistentTextureHandle>`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L493) | Identifies a persistent device texture without exposing a backend-native handle. | — | — |

### `Inno.Rendering.RasterPassBuilder`

<!-- api: T:Inno.Rendering.RasterPassBuilder 2d58e912d27927cf0f2339d41e641c9c3e468cb0aa43d0552825c591197bd4a8 -->
<!-- api: M:Inno.Rendering.RasterPassBuilder.SetViewTransform(System.ReadOnlySpan{System.Single},System.ReadOnlySpan{System.Single}) 37b1c7bacf3f99b1d3930cec8c574c9ce7c895c9495d52636873f47c86bfa15b -->
<!-- api: M:Inno.Rendering.RasterPassBuilder.UseSurface(Inno.Rendering.RenderSurfaceHandle) d6f71cb8c862405313c955d1449607ca2270ad349f789169c2afc5d4cc122654 -->
<!-- api: M:Inno.Rendering.RasterPassBuilder.ClearPresentationTarget(Inno.Rendering.RenderClearColor) 24f4c5dc0ebe9002d078310251a90476887da2b611970979e3c43f61e97db3e1 -->
<!-- api: M:Inno.Rendering.RasterPassBuilder.UseColorAttachment(Inno.Rendering.RenderTextureHandle,System.Int32,Inno.Rendering.RenderLoadAction,Inno.Rendering.RenderStoreAction,Inno.Rendering.RenderClearColor,System.Int32,System.Int32) 7bc112e05b47f7a1440903b2caed16c53d5a1c0d13c4416b87c6fb21b0f5ba25 -->
<!-- api: M:Inno.Rendering.RasterPassBuilder.UseDepthAttachment(Inno.Rendering.RenderTextureHandle,Inno.Rendering.RenderLoadAction,Inno.Rendering.RenderStoreAction,System.Single,System.Byte,System.Int32,System.Int32) fef9eeb3ed601dca8a523d3e354a1eee046b9e6570f9be17e979e9e5c4d98077 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a37fdc3f794957f1"></a>[`public Inno.Rendering.RasterPassBuilder Inno.Rendering.RasterPassBuilder.ClearPresentationTarget(Inno.Rendering.RenderClearColor clearColor)`](../../src/services/rendering/Inno.Rendering/Graph/RasterPassBuilder.cs#L71) | Clears the primary backbuffer or detached presentation surface before this pass.<br>返回：This builder for fluent declarations.<br>`clearColor`：Linear color written before draw commands. | — | — |
| <a id="api-3db74e4561a6c9b4"></a>[`public Inno.Rendering.RasterPassBuilder Inno.Rendering.RasterPassBuilder.SetViewTransform(System.ReadOnlySpan<float> viewMatrix, System.ReadOnlySpan<float> projectionMatrix)`](../../src/services/rendering/Inno.Rendering/Graph/RasterPassBuilder.cs#L29) | Sets backend-ready column-major transform matrices for this raster view.<br>返回：This builder for fluent declarations.<br>`viewMatrix`：Exactly sixteen column-major world-to-view values.<br>`projectionMatrix`：Exactly sixteen column-major projection values. | — | — |
| <a id="api-582212eb2d875aef"></a>[`public Inno.Rendering.RasterPassBuilder Inno.Rendering.RasterPassBuilder.UseColorAttachment(Inno.Rendering.RenderTextureHandle texture, int slot, Inno.Rendering.RenderLoadAction loadAction, Inno.Rendering.RenderStoreAction storeAction = Inno.Rendering.RenderStoreAction.Store, Inno.Rendering.RenderClearColor clearColor = default(Inno.Rendering.RenderClearColor), int mipLevel = 0, int arrayLayer = 0)`](../../src/services/rendering/Inno.Rendering/Graph/RasterPassBuilder.cs#L106) | Attaches a color texture.<br>返回：This builder for fluent declarations.<br>`texture`：Color attachment texture.<br>`slot`：Zero-based color attachment slot.<br>`loadAction`：Initial content behavior.<br>`storeAction`：Final content behavior.<br>`clearColor`：Linear clear value used for Inno.Rendering.RenderLoadAction.Clear.<br>`mipLevel`：Attached mip level.<br>`arrayLayer`：Attached texture-array layer. | — | — |
| <a id="api-5510208ebc968b0b"></a>[`public Inno.Rendering.RasterPassBuilder Inno.Rendering.RasterPassBuilder.UseDepthAttachment(Inno.Rendering.RenderTextureHandle texture, Inno.Rendering.RenderLoadAction loadAction, Inno.Rendering.RenderStoreAction storeAction = Inno.Rendering.RenderStoreAction.Store, float clearDepth = 1, byte clearStencil = 0, int mipLevel = 0, int arrayLayer = 0)`](../../src/services/rendering/Inno.Rendering/Graph/RasterPassBuilder.cs#L159) | Attaches a depth or depth-stencil texture.<br>返回：This builder for fluent declarations.<br>`texture`：Depth attachment texture.<br>`loadAction`：Initial content behavior.<br>`storeAction`：Final content behavior.<br>`clearDepth`：Depth clear value.<br>`clearStencil`：Stencil clear value.<br>`mipLevel`：Attached mip level.<br>`arrayLayer`：Attached texture-array layer. | — | — |
| <a id="api-8b3252fb4f9fa558"></a>[`public Inno.Rendering.RasterPassBuilder Inno.Rendering.RasterPassBuilder.UseSurface(Inno.Rendering.RenderSurfaceHandle surface)`](../../src/services/rendering/Inno.Rendering/Graph/RasterPassBuilder.cs#L50) | Directs this pass to a persistent presentation surface instead of the primary backbuffer.<br>返回：This builder for fluent declarations.<br>`surface`：Surface owned by the active graphics-device generation. | — | `System.ArgumentException`：Thrown when surface is invalid. |
| <a id="api-c3be753d5be5c0ce"></a>[`public sealed class Inno.Rendering.RasterPassBuilder : Inno.Rendering.RenderPassBuilder`](../../src/services/rendering/Inno.Rendering/Graph/RasterPassBuilder.cs#L9) | Declares raster attachments and resource access. | — | — |

### `Inno.Rendering.RenderBindingId`

<!-- api: T:Inno.Rendering.RenderBindingId ec6480f5aa418490c712e9f94e884fbb6189cddc92f9db8bc0799adedb2fc773 -->
<!-- api: M:Inno.Rendering.RenderBindingId.#ctor(System.String) 3d509c32fe33fd76e3d57b47025a57da1feb70878df8ceca961175990dca6f76 -->
<!-- api: P:Inno.Rendering.RenderBindingId.value c06c4d4100b68ae7ec8fef2779ce3158828f3b38ba9de7b77ab138369ae7b810 -->
<!-- api: P:Inno.Rendering.RenderBindingId.isValid 6ca95b1a4fdeb512a861d006bafd75a06f28b51d7cc62b0f596f30e353bad878 -->
<!-- api: M:Inno.Rendering.RenderBindingId.ToString 5e10a7691086210fcd2a1ae8d344d78c4d36b1b9291f4d1eda4bced217a6c310 -->
<!-- api: M:Inno.Rendering.RenderBindingId.op_Inequality(Inno.Rendering.RenderBindingId,Inno.Rendering.RenderBindingId) 4c605f0bc319524d2b18387a6dc0be334e5376e0b0f2a0d49e47d7cce3e37f12 -->
<!-- api: M:Inno.Rendering.RenderBindingId.op_Equality(Inno.Rendering.RenderBindingId,Inno.Rendering.RenderBindingId) fcaed65911d2fe512f63df3da454f85924627f9af4cf9c47ecf4d18108215329 -->
<!-- api: M:Inno.Rendering.RenderBindingId.GetHashCode 8ddddaaa8f32d322bc2c59986c8b9ed1d7f1b903f7e214543d61990895a2caae -->
<!-- api: M:Inno.Rendering.RenderBindingId.Equals(System.Object) 7046e761acf44b37f35f0f9b8df69879a5524cd99c7f22df27c54eec72a59bb6 -->
<!-- api: M:Inno.Rendering.RenderBindingId.Equals(Inno.Rendering.RenderBindingId) 77192963f2ff6e98e5f984635a820ce8391e1cb41ff5d532d71008b3cc49dd59 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4d04987c51d96a21"></a>[`public Inno.Rendering.RenderBindingId.RenderBindingId(string value)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L16) | Creates a stable shader binding identifier.<br>`value`：Stable manifest binding name. | — | — |
| <a id="api-c87abeb6a59f1901"></a>[`public bool Inno.Rendering.RenderBindingId.Equals(Inno.Rendering.RenderBindingId other)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-fd61dccb1d600bcf"></a>[`public override bool Inno.Rendering.RenderBindingId.Equals(object obj)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L8) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-1f7568578974b4a0"></a>[`public override int Inno.Rendering.RenderBindingId.GetHashCode()`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L8) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-26cbc3fbc1e7abf7"></a>[`public static bool Inno.Rendering.RenderBindingId.operator ==(Inno.Rendering.RenderBindingId left, Inno.Rendering.RenderBindingId right)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-709f50081f2be7b5"></a>[`public static bool Inno.Rendering.RenderBindingId.operator !=(Inno.Rendering.RenderBindingId left, Inno.Rendering.RenderBindingId right)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-a647d99350cfa074"></a>[`public override string Inno.Rendering.RenderBindingId.ToString()`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L8) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-1f4078de00bba2d1"></a>[`public bool Inno.Rendering.RenderBindingId.isValid { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L30) | Gets whether the identifier contains a stable manifest name. | — | — |
| <a id="api-69abc29619de3104"></a>[`public string Inno.Rendering.RenderBindingId.value { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L25) | Gets the stable manifest binding name. | — | — |
| <a id="api-0e56b6fd37a0a6fc"></a>[`public readonly record struct Inno.Rendering.RenderBindingId : System.IEquatable<Inno.Rendering.RenderBindingId>`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L8) | Identifies a shader binding by stable manifest name. | — | — |

### `Inno.Rendering.RenderBlendEquation`

<!-- api: T:Inno.Rendering.RenderBlendEquation 1a1154def47f5658e781f785d95f08960d0546a0ca565a1d340b1c6a555ceea5 -->
<!-- api: F:Inno.Rendering.RenderBlendEquation.Add e7377a08c28caeb1051f4baa6780ce4f33098758d6c7494f7a65851fdc313874 -->
<!-- api: F:Inno.Rendering.RenderBlendEquation.Subtract 1ca75d0a16fd098a0f17c92dc02ecb2ed99da4deaf49cecbe59284858f331a74 -->
<!-- api: F:Inno.Rendering.RenderBlendEquation.ReverseSubtract 2af0b85ed5af4e92140212edc27947bb28cbbe5075358f8f6d2c3a8b964857ae -->
<!-- api: F:Inno.Rendering.RenderBlendEquation.Minimum 27c398e927d13827a9af5091e074ae68fa851835b4537c7890a7b73b4c5227b9 -->
<!-- api: F:Inno.Rendering.RenderBlendEquation.Maximum bbe3f7672d2db073afb0f0275dce0dd78b8164fa2816f9df685bb980d5ccd892 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2b5b4af9c15a2407"></a>[`Inno.Rendering.RenderBlendEquation.Add = 0`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L74) | Adds source and destination terms. | — | — |
| <a id="api-f13c547a46ff55e6"></a>[`Inno.Rendering.RenderBlendEquation.Maximum = 4`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L90) | Selects the component-wise maximum. | — | — |
| <a id="api-90099c36f3bb52e3"></a>[`Inno.Rendering.RenderBlendEquation.Minimum = 3`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L86) | Selects the component-wise minimum. | — | — |
| <a id="api-0f33a0b20603b69b"></a>[`Inno.Rendering.RenderBlendEquation.ReverseSubtract = 2`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L82) | Subtracts the source term from the destination term. | — | — |
| <a id="api-8f7f94de49ab3bdc"></a>[`Inno.Rendering.RenderBlendEquation.Subtract = 1`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L78) | Subtracts the destination term from the source term. | — | — |
| <a id="api-ab149a9128514c11"></a>[`public enum Inno.Rendering.RenderBlendEquation : int`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L69) | Selects the arithmetic operation combining source and destination blend terms. | — | — |

### `Inno.Rendering.RenderBlendFactor`

<!-- api: T:Inno.Rendering.RenderBlendFactor 57d937adfb777a6998ac644865888abb8e065f293bc671a4bfee6d55215ec921 -->
<!-- api: F:Inno.Rendering.RenderBlendFactor.Zero d5965e6766c24f875968d5fc98a3ca9adf71284d1cb220a585579a6189ba1b4f -->
<!-- api: F:Inno.Rendering.RenderBlendFactor.One e66040d034e12a995788fe871f511e0aa78b2f62f2381c60c3a18bbdf6dfa5b9 -->
<!-- api: F:Inno.Rendering.RenderBlendFactor.SourceColor eb16cbc9471ac7a8fdf8c05a522fd0902d8625ec777c0f8073896d22ec231b67 -->
<!-- api: F:Inno.Rendering.RenderBlendFactor.InverseSourceColor 9cf6b04d48a860663511f5f21f9560e4682730c8a2b9b2cfe1968e29f85642c6 -->
<!-- api: F:Inno.Rendering.RenderBlendFactor.SourceAlpha 8ea169fdf016c94c87745aea78effeabafdfff89a12cec638d51ae7c2f22e2bb -->
<!-- api: F:Inno.Rendering.RenderBlendFactor.InverseSourceAlpha 29a8b5185e9409a88d85f7f14f01e7992fd2e29d00acaaf14836e0aef37d422e -->
<!-- api: F:Inno.Rendering.RenderBlendFactor.DestinationAlpha 0d53da2a1d61effa8c08d0f838add43b776520b1d61aaef695f20d3a80d27c82 -->
<!-- api: F:Inno.Rendering.RenderBlendFactor.InverseDestinationAlpha a9303eb0d92c05fb27840d4c17aec5e9048b3d5e74b19a26855de3162b2710ea -->
<!-- api: F:Inno.Rendering.RenderBlendFactor.DestinationColor 169fc7dcce0983fcf77200611cc221bf063fb0da70a8cfa45e473a2ffabbf34c -->
<!-- api: F:Inno.Rendering.RenderBlendFactor.InverseDestinationColor edf46486329bd27126bda2dffb68d53b9f2dffa4ee4727de8620a5c609935f65 -->
<!-- api: F:Inno.Rendering.RenderBlendFactor.SourceAlphaSaturate 07580a54d712d93413a15aa1a15716aaad6b0b0f5e4b3acf5dd1360c35055c6c -->
<!-- api: F:Inno.Rendering.RenderBlendFactor.Constant 7951ddb8b154fdef32a28d4f36742c345be13ac623224f931df64ca49cfd93c8 -->
<!-- api: F:Inno.Rendering.RenderBlendFactor.InverseConstant cf357dcc7635ced8b28fb67b21dd6a19c292e05c5ced5773ba2acb040cc4d4b2 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-793f24c6e90c3b5b"></a>[`Inno.Rendering.RenderBlendFactor.Constant = 11`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L59) | Multiplies by the packed constant blend color. | — | — |
| <a id="api-137e2a752eddbefd"></a>[`Inno.Rendering.RenderBlendFactor.DestinationAlpha = 6`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L39) | Multiplies by destination alpha. | — | — |
| <a id="api-808d7ae0e302535d"></a>[`Inno.Rendering.RenderBlendFactor.DestinationColor = 8`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L47) | Multiplies by destination color. | — | — |
| <a id="api-540af016b597e70c"></a>[`Inno.Rendering.RenderBlendFactor.InverseConstant = 12`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L63) | Multiplies by one minus the packed constant blend color. | — | — |
| <a id="api-43e64b86095dfce5"></a>[`Inno.Rendering.RenderBlendFactor.InverseDestinationAlpha = 7`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L43) | Multiplies by one minus destination alpha. | — | — |
| <a id="api-dd3afdc3f9900629"></a>[`Inno.Rendering.RenderBlendFactor.InverseDestinationColor = 9`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L51) | Multiplies by one minus destination color. | — | — |
| <a id="api-d4be16a39c4a01d4"></a>[`Inno.Rendering.RenderBlendFactor.InverseSourceAlpha = 5`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L35) | Multiplies by one minus source alpha. | — | — |
| <a id="api-66dcc1737cfbc1f2"></a>[`Inno.Rendering.RenderBlendFactor.InverseSourceColor = 3`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L27) | Multiplies by one minus source color. | — | — |
| <a id="api-a3c6be53f8b1a62d"></a>[`Inno.Rendering.RenderBlendFactor.One = 1`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L19) | Multiplies by one. | — | — |
| <a id="api-fa95f20ec8fa49d9"></a>[`Inno.Rendering.RenderBlendFactor.SourceAlpha = 4`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L31) | Multiplies by source alpha. | — | — |
| <a id="api-2936ad9d7b4049a3"></a>[`Inno.Rendering.RenderBlendFactor.SourceAlphaSaturate = 10`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L55) | Uses the saturated source-alpha factor. | — | — |
| <a id="api-a8aa5ce55e01439b"></a>[`Inno.Rendering.RenderBlendFactor.SourceColor = 2`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L23) | Multiplies by source color. | — | — |
| <a id="api-057f311feecbb8c2"></a>[`Inno.Rendering.RenderBlendFactor.Zero = 0`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L15) | Multiplies by zero. | — | — |
| <a id="api-da31643501ed39f5"></a>[`public enum Inno.Rendering.RenderBlendFactor : int`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L10) | Selects one source or destination blend multiplier. | — | — |

### `Inno.Rendering.RenderBlendState`

<!-- api: T:Inno.Rendering.RenderBlendState 4c73b604ee0777d5973de27df02dd1735a0fef344a3101a0095a0a68884d146a -->
<!-- api: P:Inno.Rendering.RenderBlendState.opaque 003f8636e4479a1df1f33a439ce2ed8d06c86221965528c35dcb75854b0d8a39 -->
<!-- api: P:Inno.Rendering.RenderBlendState.alpha 72422c8ee24252a115af5aa884775f65ca8789ff173053765b66bee39f9770e2 -->
<!-- api: P:Inno.Rendering.RenderBlendState.additive aa605e605f71eed020a17bffee366748a54586423fb4f08870a764dff64c470a -->
<!-- api: P:Inno.Rendering.RenderBlendState.premultiplied ba414c01be58a996a20781311a0af984d4d74394130c5ba62a28d3818d1edf0e -->
<!-- api: P:Inno.Rendering.RenderBlendState.enabled ec73ee359f9042ae2978a338f7236c65e04a8becf3faf22859fc7072d6225e00 -->
<!-- api: P:Inno.Rendering.RenderBlendState.colorSource c1e504c3264804e74a9e6d132993c20107f9f65dc19ace8acff909e72a63c811 -->
<!-- api: P:Inno.Rendering.RenderBlendState.colorDestination 5812556ef36b0d2140354bae3e05e44c03be5ee3b40f53929a46a5df8a2c1348 -->
<!-- api: P:Inno.Rendering.RenderBlendState.colorEquation b629938ec4a7e7e6193c9131141105ddc848d2f21c0332897b682a0868d513f8 -->
<!-- api: P:Inno.Rendering.RenderBlendState.alphaSource ea35bb680d4cc6e2e030e0e0ca41c92789b819ec97713d9fa18e12eafff255dd -->
<!-- api: P:Inno.Rendering.RenderBlendState.alphaDestination 772f952f2cc4ab63b5f9507a7446cf68b7d9c66029a4cb5ee0d97958fb4d1b0e -->
<!-- api: P:Inno.Rendering.RenderBlendState.alphaEquation 22b11454880902d1eb97a6643a7f4dd6fe640dc2fdbb7140785965015d8ed550 -->
<!-- api: P:Inno.Rendering.RenderBlendState.constantRgba 65a4141479c15885434071cc105896769f5bb44eb74a97aea263cff4abfe401b -->
<!-- api: P:Inno.Rendering.RenderBlendState.alphaToCoverage 68412fb6222f2bc110dc7daa58d7a2523c60c3d2c9ecab2d56b6c2a60bb38023 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-642731d7adec697f"></a>[`public static Inno.Rendering.RenderBlendState Inno.Rendering.RenderBlendState.additive { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L125) | Gets additive source-alpha blending. | — | — |
| <a id="api-a5061d2d7db55898"></a>[`public static Inno.Rendering.RenderBlendState Inno.Rendering.RenderBlendState.alpha { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L113) | Gets conventional straight-alpha blending. | — | — |
| <a id="api-83a0ae855a636e1f"></a>[`public Inno.Rendering.RenderBlendFactor Inno.Rendering.RenderBlendState.alphaDestination { get; set; }`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L174) | Gets or sets the destination multiplier for alpha. | — | — |
| <a id="api-dd64b956cd7ff7aa"></a>[`public Inno.Rendering.RenderBlendEquation Inno.Rendering.RenderBlendState.alphaEquation { get; set; }`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L179) | Gets or sets the alpha combination equation. | — | — |
| <a id="api-b55fdc90ff1eacf3"></a>[`public Inno.Rendering.RenderBlendFactor Inno.Rendering.RenderBlendState.alphaSource { get; set; }`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L169) | Gets or sets the source multiplier for alpha. | — | — |
| <a id="api-a930727c7f0c4124"></a>[`public bool Inno.Rendering.RenderBlendState.alphaToCoverage { get; set; }`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L189) | Gets or sets whether alpha-to-coverage is enabled. | — | — |
| <a id="api-0d89d471b19c95d3"></a>[`public Inno.Rendering.RenderBlendFactor Inno.Rendering.RenderBlendState.colorDestination { get; set; }`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L159) | Gets or sets the destination multiplier for RGB channels. | — | — |
| <a id="api-20d6d52b337f1ddb"></a>[`public Inno.Rendering.RenderBlendEquation Inno.Rendering.RenderBlendState.colorEquation { get; set; }`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L164) | Gets or sets the RGB combination equation. | — | — |
| <a id="api-33cfc1027348b1b8"></a>[`public Inno.Rendering.RenderBlendFactor Inno.Rendering.RenderBlendState.colorSource { get; set; }`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L154) | Gets or sets the source multiplier for RGB channels. | — | — |
| <a id="api-3606982c583cecc2"></a>[`public uint Inno.Rendering.RenderBlendState.constantRgba { get; set; }`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L184) | Gets or sets the packed RGBA8 constant used by constant blend factors. | — | — |
| <a id="api-9169c23845e575fc"></a>[`public bool Inno.Rendering.RenderBlendState.enabled { get; set; }`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L149) | Gets or sets whether blending is enabled. | — | — |
| <a id="api-53d4ebe5cd83541c"></a>[`public static Inno.Rendering.RenderBlendState Inno.Rendering.RenderBlendState.opaque { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L101) | Gets the disabled opaque blend state. | — | — |
| <a id="api-6e35365a9205562b"></a>[`public static Inno.Rendering.RenderBlendState Inno.Rendering.RenderBlendState.premultiplied { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L137) | Gets conventional premultiplied-alpha blending. | — | — |
| <a id="api-a7c84da2367010a7"></a>[`public struct Inno.Rendering.RenderBlendState`](../../src/services/rendering/Inno.Rendering/Device/RenderBlendState.cs#L96) | Describes independent color and alpha blending without backend-native flags. | — | — |

### `Inno.Rendering.RenderBufferDescriptor`

<!-- api: T:Inno.Rendering.RenderBufferDescriptor 87b3aa0ecdc9c93e0a9f6c911fa8080fc0da71c2605d3d04feacdef7b58850a3 -->
<!-- api: M:Inno.Rendering.RenderBufferDescriptor.#ctor(System.Int32,System.Int32,Inno.Rendering.RenderBufferUsage) e784ad2eedda0422f2af6ae0730b9b97aa6d16e1999e1ae2fd8134b4aba4b22a -->
<!-- api: P:Inno.Rendering.RenderBufferDescriptor.elementCount b80714d689cea3ca27c475beb77c51267e66344fbaa8d51e5e299db91242c70a -->
<!-- api: P:Inno.Rendering.RenderBufferDescriptor.elementStride 046d22698554c3c46eae55293d2bbfbe9d25a69eab7bb090a15093234ca651b4 -->
<!-- api: P:Inno.Rendering.RenderBufferDescriptor.usage b8845b1f3a018c0b4939f610c23a701a3f50d2947e332381cb5b1f625d0cf9df -->
<!-- api: M:Inno.Rendering.RenderBufferDescriptor.Equals(Inno.Rendering.RenderBufferDescriptor) e91f912ab4a7902dc8e9902ea6f22cd8270e5b932d6e48cf7a9133c1132dc662 -->
<!-- api: M:Inno.Rendering.RenderBufferDescriptor.Equals(System.Object) 8bc77301e785610d844f9b58d0409536558f3e8f72a1968f93bb44b0180d6c11 -->
<!-- api: M:Inno.Rendering.RenderBufferDescriptor.GetHashCode 7e9091fb9bd8499e55655510c5e8ca96c2d4ee698c6c574302d2059b7a72a6e2 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-36a54532bc353c08"></a>[`public Inno.Rendering.RenderBufferDescriptor.RenderBufferDescriptor(int elementCount, int elementStride, Inno.Rendering.RenderBufferUsage usage)`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L378) | Creates a buffer descriptor.<br>`elementCount`：Number of addressable elements.<br>`elementStride`：Element size in bytes.<br>`usage`：Permitted operations. | — | — |
| <a id="api-80b785f3fb078179"></a>[`public bool Inno.Rendering.RenderBufferDescriptor.Equals(Inno.Rendering.RenderBufferDescriptor? other)`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L414) | Determines whether this instance and the supplied value represent the same logical state.<br>返回：true when both values represent the same logical state; otherwise, false.<br>`other`：The value to compare with this instance. | — | — |
| <a id="api-88b17caee245e87e"></a>[`public override bool Inno.Rendering.RenderBufferDescriptor.Equals(object? obj)`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L429) | Determines whether this instance and the supplied value represent the same logical state.<br>返回：true when both values represent the same logical state; otherwise, false.<br>`obj`：The object to compare with this instance. | — | — |
| <a id="api-4c646bf8e50fc9d7"></a>[`public override int Inno.Rendering.RenderBufferDescriptor.GetHashCode()`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L437) | Computes a hash code from the fields that participate in logical equality.<br>返回：A hash code consistent with the implemented equality contract. | — | — |
| <a id="api-f4a65ab50de58e4a"></a>[`public int Inno.Rendering.RenderBufferDescriptor.elementCount { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L393) | Gets the number of addressable elements. | — | — |
| <a id="api-7e0827d95c97ac69"></a>[`public int Inno.Rendering.RenderBufferDescriptor.elementStride { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L398) | Gets the element size in bytes. | — | — |
| <a id="api-311173c0dd0e2252"></a>[`public Inno.Rendering.RenderBufferUsage Inno.Rendering.RenderBufferDescriptor.usage { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L403) | Gets permitted operations. | — | — |
| <a id="api-01559dee21a33a44"></a>[`public sealed class Inno.Rendering.RenderBufferDescriptor : System.IEquatable<Inno.Rendering.RenderBufferDescriptor>`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L364) | Describes a render-graph buffer independently from a graphics backend. | — | — |

### `Inno.Rendering.RenderBufferHandle`

<!-- api: T:Inno.Rendering.RenderBufferHandle ce164fd6561d4d4bbeae8235f0adf3ee2ba41f2dc7da385713df06220f6dbcd5 -->
<!-- api: P:Inno.Rendering.RenderBufferHandle.isValid f98ca98127f6b1c250d528ed11fc24aeb8c6ffa9ae7e80fc909e2747c7948463 -->
<!-- api: M:Inno.Rendering.RenderBufferHandle.ToString 35bad0cbef7f8c1049ef112dfd09a2296df0fbcb8349c27a6f7fe10e6742c960 -->
<!-- api: M:Inno.Rendering.RenderBufferHandle.op_Inequality(Inno.Rendering.RenderBufferHandle,Inno.Rendering.RenderBufferHandle) 2c2a7dd118ce1d0fc02232a17f272e59a918b8beb0370c7fcac86c4c8d3a97a0 -->
<!-- api: M:Inno.Rendering.RenderBufferHandle.op_Equality(Inno.Rendering.RenderBufferHandle,Inno.Rendering.RenderBufferHandle) acff45e5580c38ee27ba5dd78b881794d551a9476047cc3c8f7c07f7fd548690 -->
<!-- api: M:Inno.Rendering.RenderBufferHandle.GetHashCode c98f511756d3b09dff4dcae2d601f795bd10088dc5b807384be96649d91714c2 -->
<!-- api: M:Inno.Rendering.RenderBufferHandle.Equals(System.Object) 2527a7cf62fe5c7fe555980aa5493480946679c65ba069c0916fb4ae01e832b6 -->
<!-- api: M:Inno.Rendering.RenderBufferHandle.Equals(Inno.Rendering.RenderBufferHandle) d3b5b68235f36e9f1a45318ce0437b7f549753325b6dbe8dc61410f53150da2b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c08185700b4d9c60"></a>[`public bool Inno.Rendering.RenderBufferHandle.Equals(Inno.Rendering.RenderBufferHandle other)`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L468) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-e44b1f6f123afa33"></a>[`public override bool Inno.Rendering.RenderBufferHandle.Equals(object obj)`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L468) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-79749abf62c7e830"></a>[`public override int Inno.Rendering.RenderBufferHandle.GetHashCode()`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L468) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-7f91d3499c963d63"></a>[`public static bool Inno.Rendering.RenderBufferHandle.operator ==(Inno.Rendering.RenderBufferHandle left, Inno.Rendering.RenderBufferHandle right)`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L468) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-9529416e7c2c053d"></a>[`public static bool Inno.Rendering.RenderBufferHandle.operator !=(Inno.Rendering.RenderBufferHandle left, Inno.Rendering.RenderBufferHandle right)`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L468) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-f83f02533796ec33"></a>[`public override string Inno.Rendering.RenderBufferHandle.ToString()`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L468) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-0b9e365e1347d590"></a>[`public bool Inno.Rendering.RenderBufferHandle.isValid { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L487) | Gets whether the handle was created by a render graph. | — | — |
| <a id="api-7599618a17fdca3e"></a>[`public readonly record struct Inno.Rendering.RenderBufferHandle : System.IEquatable<Inno.Rendering.RenderBufferHandle>`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L468) | Identifies a frame-scoped render-graph buffer. | — | — |

### `Inno.Rendering.RenderBufferSlice`

<!-- api: T:Inno.Rendering.RenderBufferSlice b302b1e723f8e6924ff6c298a8bf3c9e1f161fd2a370b9c27eedfbd788525e8a -->
<!-- api: P:Inno.Rendering.RenderBufferSlice.firstElement e5178ff1354eceddb2c46d0da9947c627342ad00ec5b70f64a6ed8374c877f81 -->
<!-- api: P:Inno.Rendering.RenderBufferSlice.elementCount debab4a61b4762edba20d45011135b15c6843c44febdd0fffc2d5f230a012c37 -->
<!-- api: P:Inno.Rendering.RenderBufferSlice.usage 39d77b255171cf7d8916e0cb22b24a6a8e01c3e739a6810fa48631b8cc7dc2f0 -->
<!-- api: P:Inno.Rendering.RenderBufferSlice.isValid e27a4763b96f57e2dd4c8229293642aff7c1ed3ad2d8bab357ec10d2db6d228d -->
<!-- api: M:Inno.Rendering.RenderBufferSlice.ToString eff95ddadef7eb25296508e302aae72d8cedc527ebc27f912fcb62aea9bea719 -->
<!-- api: M:Inno.Rendering.RenderBufferSlice.op_Inequality(Inno.Rendering.RenderBufferSlice,Inno.Rendering.RenderBufferSlice) c7d5563b5a528174c147c047342aeba3925135552d9ef77e32d524f902c7cbc5 -->
<!-- api: M:Inno.Rendering.RenderBufferSlice.op_Equality(Inno.Rendering.RenderBufferSlice,Inno.Rendering.RenderBufferSlice) 32a5e88cc3e4c89baec80bac71ee7eb7bf3740a32e45b2d9ef2b849b6b19a997 -->
<!-- api: M:Inno.Rendering.RenderBufferSlice.GetHashCode 286a34e6ec39dac918f4a394035f632bd012eebf18b59bdb30bc944699e44b61 -->
<!-- api: M:Inno.Rendering.RenderBufferSlice.Equals(System.Object) 0a7505990d3a805f3a0d46717695151a346930cb56dea41dd7fa6fb097ba7a59 -->
<!-- api: M:Inno.Rendering.RenderBufferSlice.Equals(Inno.Rendering.RenderBufferSlice) ab70598b5a95736165c2a8574f8534481f12e7f34795e7a6b684a5545b5d2a57 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-83b7bdcf749718f8"></a>[`public bool Inno.Rendering.RenderBufferSlice.Equals(Inno.Rendering.RenderBufferSlice other)`](../../src/services/rendering/Inno.Rendering/Device/RenderBufferUploads.cs#L84) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-f91a76635b3dd4c9"></a>[`public override bool Inno.Rendering.RenderBufferSlice.Equals(object obj)`](../../src/services/rendering/Inno.Rendering/Device/RenderBufferUploads.cs#L84) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-52e007bde37bef03"></a>[`public override int Inno.Rendering.RenderBufferSlice.GetHashCode()`](../../src/services/rendering/Inno.Rendering/Device/RenderBufferUploads.cs#L84) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-145ff974cc556866"></a>[`public static bool Inno.Rendering.RenderBufferSlice.operator ==(Inno.Rendering.RenderBufferSlice left, Inno.Rendering.RenderBufferSlice right)`](../../src/services/rendering/Inno.Rendering/Device/RenderBufferUploads.cs#L84) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-d2a1d2ef5347f762"></a>[`public static bool Inno.Rendering.RenderBufferSlice.operator !=(Inno.Rendering.RenderBufferSlice left, Inno.Rendering.RenderBufferSlice right)`](../../src/services/rendering/Inno.Rendering/Device/RenderBufferUploads.cs#L84) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-f988b8c738f18c4f"></a>[`public override string Inno.Rendering.RenderBufferSlice.ToString()`](../../src/services/rendering/Inno.Rendering/Device/RenderBufferUploads.cs#L84) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-296887fce9aa543e"></a>[`public int Inno.Rendering.RenderBufferSlice.elementCount { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderBufferUploads.cs#L111) | Gets the number of uploaded elements. | — | — |
| <a id="api-180a9ff1837339d9"></a>[`public int Inno.Rendering.RenderBufferSlice.firstElement { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderBufferUploads.cs#L106) | Gets the first uploaded element in the backing frame page. | — | — |
| <a id="api-44ace741d66bedc9"></a>[`public bool Inno.Rendering.RenderBufferSlice.isValid { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderBufferUploads.cs#L121) | Gets whether this slice was produced by a frame upload service. | — | — |
| <a id="api-900eb74e0b6b68ea"></a>[`public Inno.Rendering.RenderBufferUsage Inno.Rendering.RenderBufferSlice.usage { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderBufferUploads.cs#L116) | Gets permitted GPU uses for this slice. | — | — |
| <a id="api-d6418fb526109d94"></a>[`public readonly record struct Inno.Rendering.RenderBufferSlice : System.IEquatable<Inno.Rendering.RenderBufferSlice>`](../../src/services/rendering/Inno.Rendering/Device/RenderBufferUploads.cs#L84) | Identifies one range in a frame upload page without exposing its persistent backing buffer. | — | — |

### `Inno.Rendering.RenderBufferUploadDescriptor`

<!-- api: T:Inno.Rendering.RenderBufferUploadDescriptor e1f804f2b6f71dabea15e2409b8fd03edfadd80d0ebcea3a0ba02b30e20914de -->
<!-- api: M:Inno.Rendering.RenderBufferUploadDescriptor.#ctor(System.Int32,Inno.Rendering.RenderBufferUsage,Inno.Rendering.RenderVertexLayout,Inno.Rendering.RenderIndexFormat) f625fd4b705ea0d180205296bb3205c945aacb395b55312286e73d29cd671053 -->
<!-- api: P:Inno.Rendering.RenderBufferUploadDescriptor.elementStride 735df1c95c1f72bc776af8467c168e308e8e07979d90cfdee4117bf73899e790 -->
<!-- api: P:Inno.Rendering.RenderBufferUploadDescriptor.usage e4f270933a7c1bd9e15a8a93a4ad45962a6c28037984a4274f66e1a62c48eb4f -->
<!-- api: P:Inno.Rendering.RenderBufferUploadDescriptor.vertexLayout 66ce3a744001194e2695b20092165dfdf212c50c4cf2757d6aaf763ffabc2ad3 -->
<!-- api: P:Inno.Rendering.RenderBufferUploadDescriptor.indexFormat 7d9a938731179525fa2ff6f9573629b55df2def5e58a7307f0b2e1b1ae3b6a95 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-354702231d3dd685"></a>[`public Inno.Rendering.RenderBufferUploadDescriptor.RenderBufferUploadDescriptor(int elementStride, Inno.Rendering.RenderBufferUsage usage, Inno.Rendering.RenderVertexLayout? vertexLayout = null, Inno.Rendering.RenderIndexFormat indexFormat = Inno.Rendering.RenderIndexFormat.UInt32)`](../../src/services/rendering/Inno.Rendering/Device/RenderBufferUploads.cs#L25) | Creates a frame upload descriptor.<br>`elementStride`：Element size in bytes.<br>`usage`：Vertex, index, storage, or compatible combined usage.<br>`vertexLayout`：Required layout when vertex usage is present.<br>`indexFormat`：Index representation when index usage is present. | — | — |
| <a id="api-e86394f72f8eff42"></a>[`public int Inno.Rendering.RenderBufferUploadDescriptor.elementStride { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderBufferUploads.cs#L63) | Gets the element size in bytes. | — | — |
| <a id="api-0c64b4a54a586df5"></a>[`public Inno.Rendering.RenderIndexFormat Inno.Rendering.RenderBufferUploadDescriptor.indexFormat { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderBufferUploads.cs#L78) | Gets the index representation when index usage is present. | — | — |
| <a id="api-099c52ee13ec9d7b"></a>[`public Inno.Rendering.RenderBufferUsage Inno.Rendering.RenderBufferUploadDescriptor.usage { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderBufferUploads.cs#L68) | Gets permitted GPU uses. | — | — |
| <a id="api-ca335b635d83a5ac"></a>[`public Inno.Rendering.RenderVertexLayout? Inno.Rendering.RenderBufferUploadDescriptor.vertexLayout { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderBufferUploads.cs#L73) | Gets the vertex layout when vertex usage is present. | — | — |
| <a id="api-bdc70d16796bfeef"></a>[`public sealed class Inno.Rendering.RenderBufferUploadDescriptor`](../../src/services/rendering/Inno.Rendering/Device/RenderBufferUploads.cs#L8) | Describes one CPU-to-GPU buffer upload that remains valid for the current frame only. | — | — |

### `Inno.Rendering.RenderBufferUsage`

<!-- api: T:Inno.Rendering.RenderBufferUsage daaf3f5eade14b6caa97edcdc3bde232ba0dd31549cce9f340ea11ac08ba8846 -->
<!-- api: F:Inno.Rendering.RenderBufferUsage.Vertex 928a9682c60c69dfd7ee5c4028273fc9024c5f2c34f5e106fca775c6319bc566 -->
<!-- api: F:Inno.Rendering.RenderBufferUsage.Index b0bd33c57a6b079f6219154ecee98aa61ad21e73f89f492375acd1f66d10e90d -->
<!-- api: F:Inno.Rendering.RenderBufferUsage.Uniform b9d3e06a98571e1c1ba94cd30bd8ab6410e1591788a2cfe0f940dc2244e357fc -->
<!-- api: F:Inno.Rendering.RenderBufferUsage.Storage c1e5fbba68705d5cf188b39f34f329c05d78394caae3ad52f912542741897769 -->
<!-- api: F:Inno.Rendering.RenderBufferUsage.Indirect 9469a7ba425a6929f1d805fbad3cd11499a7310734d541fde5548add1dcf09fd -->
<!-- api: F:Inno.Rendering.RenderBufferUsage.CopySource 6e98a028c62cddecd30b52d61f9edf013a137c7395159615def59eccb1309bf6 -->
<!-- api: F:Inno.Rendering.RenderBufferUsage.CopyDestination e42e62248795d7fb494ecbd52f83ca67afd12e28f0251ca5e05c0754653ea5c9 -->
<!-- api: F:Inno.Rendering.RenderBufferUsage.Dynamic a097b6f430dd71129a7b7b6ce5a5987debc8f5877f37c3a451d865b3f47c407d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-ff4942a6dcec5936"></a>[`Inno.Rendering.RenderBufferUsage.CopyDestination = 64`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L140) | Copy operation destination. | — | — |
| <a id="api-a655ed754107e4a4"></a>[`Inno.Rendering.RenderBufferUsage.CopySource = 32`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L136) | Copy operation source. | — | — |
| <a id="api-3a439d59e03a350c"></a>[`Inno.Rendering.RenderBufferUsage.Dynamic = 128`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L144) | Contents may be replaced at frame safety points. | — | — |
| <a id="api-f8fe08600ad14298"></a>[`Inno.Rendering.RenderBufferUsage.Index = 2`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L120) | Index input data. | — | — |
| <a id="api-fe2fc974c9e28978"></a>[`Inno.Rendering.RenderBufferUsage.Indirect = 16`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L132) | Indirect command data. | — | — |
| <a id="api-9bea3698b0fad0bf"></a>[`Inno.Rendering.RenderBufferUsage.Storage = 8`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L128) | Shader-readable and writable storage data. | — | — |
| <a id="api-0f382eab2511e926"></a>[`Inno.Rendering.RenderBufferUsage.Uniform = 4`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L124) | Read-only shader data. | — | — |
| <a id="api-04d9c4a18ba5eab7"></a>[`Inno.Rendering.RenderBufferUsage.Vertex = 1`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L116) | Vertex input data. | — | — |
| <a id="api-a090cb78dac7b29c"></a>[`public enum Inno.Rendering.RenderBufferUsage : int`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L111) | Declares intended buffer operations for capability and hazard validation. | — | — |

### `Inno.Rendering.RenderClearColor`

<!-- api: T:Inno.Rendering.RenderClearColor 5b06440fcad010360ae6ce6d48ce22997b0387fef7a5e2d986c432cb62b72053 -->
<!-- api: M:Inno.Rendering.RenderClearColor.#ctor(System.Single,System.Single,System.Single,System.Single) 6eb46c9e16ae224519c37891bb4007a2760e437a43d844c35d53bf8934294977 -->
<!-- api: P:Inno.Rendering.RenderClearColor.r 1e3208755c28dc60f1959cc9a3aaab0fafe717c33e558d7e9465fe1956375021 -->
<!-- api: P:Inno.Rendering.RenderClearColor.g 67b5077226524ad4b92e75a034a92e04bf1dff9f541895ecca8e41e211d0c2d7 -->
<!-- api: P:Inno.Rendering.RenderClearColor.b a7eb2d546660a52053f0c026e87e19e553efb883dedb307e8d44d749d3c8210e -->
<!-- api: P:Inno.Rendering.RenderClearColor.a 2a2745dfa559d69e9071aa3d6d9ca4eab8452e456f78b6f85d2ec8de4f735c5b -->
<!-- api: M:Inno.Rendering.RenderClearColor.ToString a73535a36e178e607e38979330448b09077de0c47ca99f40370840260cb5f576 -->
<!-- api: M:Inno.Rendering.RenderClearColor.op_Inequality(Inno.Rendering.RenderClearColor,Inno.Rendering.RenderClearColor) c4fe26cd23ec0074bbdfdb7ecfb3e6e163facdc40b55408353dd55f796ef7534 -->
<!-- api: M:Inno.Rendering.RenderClearColor.op_Equality(Inno.Rendering.RenderClearColor,Inno.Rendering.RenderClearColor) 58195017b91745edb2d7dc1bce2a3685299ccdacd063b6eaf7e90f441a4526fc -->
<!-- api: M:Inno.Rendering.RenderClearColor.GetHashCode 6ae14c2fbe63dc1a95c36eb085e14cc839d522a4ef4b0a94508219f18c776dd3 -->
<!-- api: M:Inno.Rendering.RenderClearColor.Equals(System.Object) 79f721371feacfd48bf7ae46b53f4b388523ff0c8587701304a41db59cadb606 -->
<!-- api: M:Inno.Rendering.RenderClearColor.Equals(Inno.Rendering.RenderClearColor) 12b0c7b76d3db88e63ec6b39bdd5121bf72a761e30bda4287930c6b0695e85e9 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4fd4b9eef2cb3190"></a>[`public Inno.Rendering.RenderClearColor.RenderClearColor(float r, float g, float b, float a = 1)`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L126) | Creates a linear clear color.<br>`r`：Red channel.<br>`g`：Green channel.<br>`b`：Blue channel.<br>`a`：Alpha channel. | — | — |
| <a id="api-9ed93c60f6886bd3"></a>[`public bool Inno.Rendering.RenderClearColor.Equals(Inno.Rendering.RenderClearColor other)`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L109) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-c5f3c81c7cc28de9"></a>[`public override bool Inno.Rendering.RenderClearColor.Equals(object obj)`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L109) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-bbfc4352aa5203f2"></a>[`public override int Inno.Rendering.RenderClearColor.GetHashCode()`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L109) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-0b9bafba488ab4eb"></a>[`public static bool Inno.Rendering.RenderClearColor.operator ==(Inno.Rendering.RenderClearColor left, Inno.Rendering.RenderClearColor right)`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L109) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-2634e028184392af"></a>[`public static bool Inno.Rendering.RenderClearColor.operator !=(Inno.Rendering.RenderClearColor left, Inno.Rendering.RenderClearColor right)`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L109) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-ae0393e8ecb89fc3"></a>[`public override string Inno.Rendering.RenderClearColor.ToString()`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L109) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-d75826bd3207b6a7"></a>[`public float Inno.Rendering.RenderClearColor.a { get; }`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L156) | Gets the alpha channel. | — | — |
| <a id="api-eb54bbb7f032b044"></a>[`public float Inno.Rendering.RenderClearColor.b { get; }`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L151) | Gets the blue channel. | — | — |
| <a id="api-5a49def58c519088"></a>[`public float Inno.Rendering.RenderClearColor.g { get; }`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L146) | Gets the green channel. | — | — |
| <a id="api-2c551cb9790a9d75"></a>[`public float Inno.Rendering.RenderClearColor.r { get; }`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L141) | Gets the red channel. | — | — |
| <a id="api-3bda127f956177d7"></a>[`public readonly record struct Inno.Rendering.RenderClearColor : System.IEquatable<Inno.Rendering.RenderClearColor>`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L109) | Stores a linear clear color without depending on an engine math assembly. | — | — |

### `Inno.Rendering.RenderCommandEncoder`

<!-- api: T:Inno.Rendering.RenderCommandEncoder d186c23d5d957e49644d1603ff62210561dcd01d6265d864645be531885c7b2a -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.BindGraphicsPipeline(Inno.Rendering.GraphicsPipelineHandle) a67b30611ab50fd75ce80f710ebb8544b8fc7c21905859f9589080e144eb71ce -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.BindComputePipeline(Inno.Rendering.ComputePipelineHandle) d852c13f9d7b08ab8b77a53176627532f30411d83b485ba68ac010363f60e753 -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.BindTexture(Inno.Rendering.RenderBindingId,Inno.Rendering.RenderTextureHandle) 95af21abf9406ece4fc296d27ed1918e9b21d4641504fb039446e87022edbe1a -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.BindTexture(Inno.Rendering.RenderBindingId,Inno.Rendering.RenderTextureHandle,Inno.Rendering.RenderSamplerState) 5149741a6693943eae1e85614466f2f7a55a6d4185c701b38f2c70de3e46d72e -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.BindTexture(Inno.Rendering.RenderBindingId,Inno.Rendering.PersistentTextureHandle) 43f272d2c7552c6be1fb3c5cb3feb4487cb9693379837ad5cc73c933ba210558 -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.BindTexture(Inno.Rendering.RenderBindingId,Inno.Rendering.PersistentTextureHandle,Inno.Rendering.RenderSamplerState) 9a1b3a3d2740cf33fb6049d50ecafdf9a3dbd0f53f97f05046b36e2b1b42c0b6 -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.BindStorageTexture(Inno.Rendering.RenderBindingId,Inno.Rendering.RenderTextureHandle,System.Int32) 43f4c22488f70ac8c94009912870c5a517600271bf177c190a83be70457fc595 -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.BindStorageTexture(Inno.Rendering.RenderBindingId,Inno.Rendering.PersistentTextureHandle,System.Int32) 3bcd3ef22271e23233e0f658ab900b7a02d81cf49c64fbe4cc430b54afe9e799 -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.BindBuffer(Inno.Rendering.RenderBindingId,Inno.Rendering.RenderBufferHandle) 3ebb6acbfc9cd6b23990e418a0147f74308d361c09d5a48067cdf120154ea66f -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.BindBuffer(Inno.Rendering.RenderBindingId,Inno.Rendering.PersistentBufferHandle) 0a3e6893fcf2023d3e536c4f653567c951121b61bc588a6eef3071d6dbce357c -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.BindBuffer(Inno.Rendering.RenderBindingId,Inno.Rendering.RenderBufferSlice) 6001007c040b4ea89bc46b2ec85596463576f64d234a9efc1ae973e68f7a9626 -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.SetUniform(Inno.Rendering.RenderBindingId,System.ReadOnlySpan{System.Byte}) d6327704a35a4e0eceb68299fafda2cae2ea12b6b521e24eb356d8a696ff2346 -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.SetTransform(System.ReadOnlySpan{System.Single}) 31fcb4ef400c1e4c13bde36202fa483622474c8be4f17a123d630c225212791d -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.SetRasterState(Inno.Rendering.RenderRasterState) f11b61ba3d33abe7a5a0a84ff54eb83a6f5a90969d4a587d4e1c42ee88004e72 -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.SetStencil(Inno.Rendering.RenderStencilState) 0378718c9fda8b1b8aef0204aa1d9d892d153e243b60cdcdee7f3dae4e1e62dc -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.SetViewport(System.Int32,System.Int32,System.Int32,System.Int32) 1007f23f2d4b91be93e1cc05cbb48018429d4f47c50740ef701f18c8763f9b2d -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.SetScissor(System.Int32,System.Int32,System.Int32,System.Int32) eb0a2e88010835d6a3f4de9f2bd9c7a325195437dd44d6c30cf71f8567666978 -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.BindVertexBuffer(Inno.Rendering.RenderBufferHandle,System.Int32) fc9d1f80e9f633b5f3c15545c7cc317e4c92178206f12008e2aa25971cd4e7de -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.BindVertexBuffer(Inno.Rendering.PersistentBufferHandle,System.Int32) f8854d09f6b2d64a618ca62ea37c34a47f92815721b16ad957025a6b01457a9b -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.BindVertexBuffer(Inno.Rendering.RenderBufferSlice) 17591fcd32e7867ca463723b6c26384ef1e213f6ef4ed116e28467b2386525f2 -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.BindIndexBuffer(Inno.Rendering.RenderBufferHandle,System.Int32) 7a0f22158295b5becdbe825660c17d99c486a1f6a190fa9e6cea4d511c463303 -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.BindIndexBuffer(Inno.Rendering.PersistentBufferHandle,System.Int32) 82f02a0d1d904f04f3175d7585743c17b15bf6293667f5f1a791ab7790d05bd9 -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.BindIndexBuffer(Inno.Rendering.RenderBufferSlice) f1b108e615d724aeb5b0aa5a722c0e4986e352079cc7a2e4e68f4645e0e20567 -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.BindInstanceBuffer(Inno.Rendering.RenderBufferHandle,System.Int32,System.Int32) d8b487c3acf98935c35d5cc6cfb1bd540c788a1d32744375a585d6bff42a7588 -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.BindInstanceBuffer(Inno.Rendering.PersistentBufferHandle,System.Int32,System.Int32) 9bc2a8356917e72aafaaf818b9af53567e080bbee2525abf3fa177504354d4da -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.BindInstanceBuffer(Inno.Rendering.RenderBufferSlice) 9c70b1075fe01d575c33834bab42616fcd88187d9616b7c34cdb904f11d13745 -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.Draw(System.Int32,System.Int32) eb91a9aa481ddda9c58876605ecf8015fadd897722600920cc6ea358fcf43931 -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.DrawProcedural(System.Int32,System.Int32) cdf2aaf9ad8cc97e1db41d41402e3d71dd367a4bc2c866ac3780fd36fbc6bf6d -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.DrawIndexed(System.Int32,System.Int32) aaf5ac67d8dd484fb56511aececac9cfd8e1b4cd166a98a50b946fafb6962c50 -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.DrawIndirect(Inno.Rendering.RenderBufferHandle,System.Int32,System.Int32) d852c41031644fc1e303668bd2d4fcdf96d8233738ec72c1a72127f54b78bead -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.DrawIndirect(Inno.Rendering.PersistentBufferHandle,System.Int32,System.Int32) 3b5a61f00adb59fee8ecafa129527edacf1302b3cc2a7b763f282cef898e2db1 -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.Dispatch(System.Int32,System.Int32,System.Int32) 0c7f71c479e084e8fe9e996ffb00251a475a23daa0b14efc7e08815e23b2f6b5 -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.DispatchIndirect(Inno.Rendering.RenderBufferHandle,System.Int32,System.Int32) f3f320bc21922d42bf93737a3d61ae5ede5345996838756b1a646c5e4c18c1dc -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.DispatchIndirect(Inno.Rendering.PersistentBufferHandle,System.Int32,System.Int32) 36c7e9fedcf85f39984cea07af262fc46c15ced59fbf997e18c579d86827b7cb -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.CopyTexture(Inno.Rendering.RenderTextureHandle,Inno.Rendering.RenderTextureHandle) 559a248214074ef587a6c77d48c68b68a11c6c2f47418c24abcfcfd017d5a66a -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.BlitTexture(Inno.Rendering.RenderTextureHandle,Inno.Rendering.RenderTextureRegion,Inno.Rendering.RenderTextureHandle,Inno.Rendering.RenderTextureRegion) bddb25abf28ef659bd3914046595e20481d82c551a50ecf10c2a1c99e0c8e6ca -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.CopyBuffer(Inno.Rendering.RenderBufferHandle,Inno.Rendering.RenderBufferHandle) 75a6563ce21c0fee2ea42cbf1458413e7ea93b4f488e6f8a9201e9c2ac8794de -->
<!-- api: M:Inno.Rendering.RenderCommandEncoder.#ctor 412dedeef1db2aa788c7ed0f560dc8a7ac751f33df6b69507d64f55df1248d02 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-50c4b741ee8fe5cb"></a>[`protected Inno.Rendering.RenderCommandEncoder.RenderCommandEncoder()`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L170) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-732c0a78039c3e51"></a>[`public abstract void Inno.Rendering.RenderCommandEncoder.BindBuffer(Inno.Rendering.RenderBindingId binding, Inno.Rendering.PersistentBufferHandle buffer)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L316) | Binds a persistent storage buffer to a shader interface slot.<br>`binding`：Stable shader binding identifier.<br>`buffer`：Persistent buffer owned by the active device generation. | — | — |
| <a id="api-8a2aa8fb67177cab"></a>[`public abstract void Inno.Rendering.RenderCommandEncoder.BindBuffer(Inno.Rendering.RenderBindingId binding, Inno.Rendering.RenderBufferHandle buffer)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L302) | Binds a graph buffer to a shader interface slot.<br>`binding`：Stable shader binding identifier.<br>`buffer`：Graph buffer to bind. | — | — |
| <a id="api-8d1611438a1caa22"></a>[`public void Inno.Rendering.RenderCommandEncoder.BindBuffer(Inno.Rendering.RenderBindingId binding, Inno.Rendering.RenderBufferSlice buffer)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L330) | Binds a frame-uploaded storage buffer to a shader interface slot.<br>`binding`：Stable shader binding identifier.<br>`buffer`：Current-frame storage slice. | — | — |
| <a id="api-ea6b947b80525f08"></a>[`public abstract void Inno.Rendering.RenderCommandEncoder.BindComputePipeline(Inno.Rendering.ComputePipelineHandle pipeline)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L189) | Binds a compute pipeline for subsequent dispatch commands.<br>`pipeline`：Compute pipeline to bind. | — | — |
| <a id="api-f94c42b588843123"></a>[`public abstract void Inno.Rendering.RenderCommandEncoder.BindGraphicsPipeline(Inno.Rendering.GraphicsPipelineHandle pipeline)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L181) | Binds a graphics pipeline for subsequent draw commands.<br>`pipeline`：Graphics pipeline to bind. | — | — |
| <a id="api-7ed37ed01f78c639"></a>[`public abstract void Inno.Rendering.RenderCommandEncoder.BindIndexBuffer(Inno.Rendering.PersistentBufferHandle buffer, int firstIndex = 0)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L489) | Binds a persistent buffer as index input.<br>`buffer`：Persistent index buffer.<br>`firstIndex`：First index element. | — | — |
| <a id="api-59dbdd4beaca0329"></a>[`public abstract void Inno.Rendering.RenderCommandEncoder.BindIndexBuffer(Inno.Rendering.RenderBufferHandle buffer, int firstIndex = 0)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L475) | Binds a graph buffer as index input.<br>`buffer`：Index buffer.<br>`firstIndex`：First index element. | — | — |
| <a id="api-ea5968a213226760"></a>[`public void Inno.Rendering.RenderCommandEncoder.BindIndexBuffer(Inno.Rendering.RenderBufferSlice buffer)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L500) | Binds a frame-uploaded index slice.<br>`buffer`：Current-frame index slice. | — | — |
| <a id="api-fe0596e942e2b9ea"></a>[`public abstract void Inno.Rendering.RenderCommandEncoder.BindInstanceBuffer(Inno.Rendering.PersistentBufferHandle buffer, int firstInstance, int instanceCount)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L536) | Binds persistent buffer elements as per-instance input.<br>`buffer`：Vertex-compatible persistent buffer containing instance records.<br>`firstInstance`：First instance element.<br>`instanceCount`：Positive bound instance count. | — | — |
| <a id="api-eacb60013c5907fb"></a>[`public abstract void Inno.Rendering.RenderCommandEncoder.BindInstanceBuffer(Inno.Rendering.RenderBufferHandle buffer, int firstInstance, int instanceCount)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L518) | Binds graph buffer elements as per-instance input.<br>`buffer`：Vertex-compatible graph buffer containing instance records.<br>`firstInstance`：First instance element.<br>`instanceCount`：Positive bound instance count. | — | — |
| <a id="api-0052b35debf8e22f"></a>[`public void Inno.Rendering.RenderCommandEncoder.BindInstanceBuffer(Inno.Rendering.RenderBufferSlice buffer)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L548) | Binds a frame-uploaded vertex slice as per-instance input.<br>`buffer`：Current-frame instance records. | — | — |
| <a id="api-c8043104e611c72e"></a>[`public abstract void Inno.Rendering.RenderCommandEncoder.BindStorageTexture(Inno.Rendering.RenderBindingId binding, Inno.Rendering.PersistentTextureHandle texture, int mipLevel = 0)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L287) | Binds a persistent texture for shader storage access.<br>`binding`：Stable storage-image binding identifier.<br>`texture`：Persistent texture created with storage usage.<br>`mipLevel`：Mip level exposed to the shader. | — | — |
| <a id="api-b16f116ae459804c"></a>[`public abstract void Inno.Rendering.RenderCommandEncoder.BindStorageTexture(Inno.Rendering.RenderBindingId binding, Inno.Rendering.RenderTextureHandle texture, int mipLevel = 0)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L269) | Binds a graph texture for shader storage access.<br>`binding`：Stable storage-image binding identifier.<br>`texture`：Graph texture created with storage usage.<br>`mipLevel`：Mip level exposed to the shader. | — | — |
| <a id="api-cd2b1db609e6ba8a"></a>[`public abstract void Inno.Rendering.RenderCommandEncoder.BindTexture(Inno.Rendering.RenderBindingId binding, Inno.Rendering.PersistentTextureHandle texture, Inno.Rendering.RenderSamplerState sampler)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L251) | Binds a persistent texture and explicit sampler state.<br>`binding`：Stable shader binding identifier.<br>`texture`：Persistent texture owned by the active device generation.<br>`sampler`：Backend-neutral sample state. | — | — |
| <a id="api-ba7282edd0908eda"></a>[`public void Inno.Rendering.RenderCommandEncoder.BindTexture(Inno.Rendering.RenderBindingId binding, Inno.Rendering.PersistentTextureHandle texture)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L233) | Binds a persistent texture to a shader interface slot.<br>`binding`：Stable shader binding identifier.<br>`texture`：Persistent texture owned by the active device generation. | — | — |
| <a id="api-15767745948f0388"></a>[`public abstract void Inno.Rendering.RenderCommandEncoder.BindTexture(Inno.Rendering.RenderBindingId binding, Inno.Rendering.RenderTextureHandle texture, Inno.Rendering.RenderSamplerState sampler)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L218) | Binds a graph texture and explicit sampler state.<br>`binding`：Stable shader binding identifier.<br>`texture`：Graph texture to bind.<br>`sampler`：Backend-neutral sample state. | — | — |
| <a id="api-cabad1f315d7a988"></a>[`public void Inno.Rendering.RenderCommandEncoder.BindTexture(Inno.Rendering.RenderBindingId binding, Inno.Rendering.RenderTextureHandle texture)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L200) | Binds a graph texture to a shader interface slot.<br>`binding`：Stable shader binding identifier.<br>`texture`：Graph texture to bind. | — | — |
| <a id="api-1292a25c6f273d30"></a>[`public abstract void Inno.Rendering.RenderCommandEncoder.BindVertexBuffer(Inno.Rendering.PersistentBufferHandle buffer, int firstVertex = 0)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L449) | Binds a persistent buffer as vertex input.<br>`buffer`：Persistent vertex buffer.<br>`firstVertex`：First vertex element. | — | — |
| <a id="api-9e05b7c7dbb72d1e"></a>[`public abstract void Inno.Rendering.RenderCommandEncoder.BindVertexBuffer(Inno.Rendering.RenderBufferHandle buffer, int firstVertex = 0)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L435) | Binds a graph buffer as vertex input.<br>`buffer`：Vertex buffer.<br>`firstVertex`：First vertex element. | — | — |
| <a id="api-2befef7147ee945f"></a>[`public void Inno.Rendering.RenderCommandEncoder.BindVertexBuffer(Inno.Rendering.RenderBufferSlice buffer)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L460) | Binds a frame-uploaded vertex slice.<br>`buffer`：Current-frame vertex slice. | — | — |
| <a id="api-0c8f86b32175f83b"></a>[`public abstract void Inno.Rendering.RenderCommandEncoder.BlitTexture(Inno.Rendering.RenderTextureHandle source, Inno.Rendering.RenderTextureRegion sourceRegion, Inno.Rendering.RenderTextureHandle destination, Inno.Rendering.RenderTextureRegion destinationRegion)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L715) | Blits compatible texture regions without CPU readback.<br>`source`：Copy source texture.<br>`sourceRegion`：Source subresource box.<br>`destination`：Copy destination texture.<br>`destinationRegion`：Destination origin and compatible extent. | — | — |
| <a id="api-3adcea8c4ecf45a3"></a>[`public abstract void Inno.Rendering.RenderCommandEncoder.CopyBuffer(Inno.Rendering.RenderBufferHandle source, Inno.Rendering.RenderBufferHandle destination)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L734) | Copies complete compatible graph buffer ranges.<br>`source`：Copy source buffer.<br>`destination`：Copy destination buffer. | — | `System.NotSupportedException`：Thrown when the backend has no safe buffer-copy path. |
| <a id="api-a971ba7322aac1fc"></a>[`public abstract void Inno.Rendering.RenderCommandEncoder.CopyTexture(Inno.Rendering.RenderTextureHandle source, Inno.Rendering.RenderTextureHandle destination)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L695) | Copies all compatible subresources between graph textures.<br>`source`：Copy source texture.<br>`destination`：Copy destination texture. | — | — |
| <a id="api-fc7aad8cb08a5c79"></a>[`public abstract void Inno.Rendering.RenderCommandEncoder.Dispatch(int groupCountX, int groupCountY = 1, int groupCountZ = 1)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L644) | Dispatches compute workgroups.<br>`groupCountX`：Workgroup count on X.<br>`groupCountY`：Workgroup count on Y.<br>`groupCountZ`：Workgroup count on Z. | — | — |
| <a id="api-92ec9fff05d7b106"></a>[`public abstract void Inno.Rendering.RenderCommandEncoder.DispatchIndirect(Inno.Rendering.PersistentBufferHandle buffer, int firstCommand = 0, int commandCount = 1)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L680) | Dispatches compute commands stored in an indirect persistent buffer.<br>`buffer`：Indirect persistent buffer.<br>`firstCommand`：First command record.<br>`commandCount`：Positive command count. | — | — |
| <a id="api-55fa5cc52bd5c0b2"></a>[`public abstract void Inno.Rendering.RenderCommandEncoder.DispatchIndirect(Inno.Rendering.RenderBufferHandle buffer, int firstCommand = 0, int commandCount = 1)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L662) | Dispatches compute commands stored in an indirect graph buffer.<br>`buffer`：Indirect graph buffer.<br>`firstCommand`：First command record.<br>`commandCount`：Positive command count. | — | — |
| <a id="api-7e4dd27ea732bb81"></a>[`public abstract void Inno.Rendering.RenderCommandEncoder.Draw(int vertexCount, int instanceCount = 1)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L563) | Issues a non-indexed draw.<br>`vertexCount`：Number of vertices.<br>`instanceCount`：Number of instances. | — | — |
| <a id="api-353f8d099b75932c"></a>[`public abstract void Inno.Rendering.RenderCommandEncoder.DrawIndexed(int indexCount, int instanceCount = 1)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L591) | Issues an indexed draw.<br>`indexCount`：Number of indices.<br>`instanceCount`：Number of instances. | — | — |
| <a id="api-b3268bc5f7c095dd"></a>[`public abstract void Inno.Rendering.RenderCommandEncoder.DrawIndirect(Inno.Rendering.PersistentBufferHandle buffer, int firstCommand = 0, int commandCount = 1)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L626) | Issues graphics commands stored in an indirect persistent buffer.<br>`buffer`：Indirect persistent buffer.<br>`firstCommand`：First command record.<br>`commandCount`：Positive command count. | — | — |
| <a id="api-7f225eec7e58970d"></a>[`public abstract void Inno.Rendering.RenderCommandEncoder.DrawIndirect(Inno.Rendering.RenderBufferHandle buffer, int firstCommand = 0, int commandCount = 1)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L608) | Issues graphics commands stored in an indirect graph buffer.<br>`buffer`：Indirect graph buffer.<br>`firstCommand`：First command record.<br>`commandCount`：Positive command count. | — | — |
| <a id="api-dbaa079d174be82c"></a>[`public abstract void Inno.Rendering.RenderCommandEncoder.DrawProcedural(int vertexCount, int instanceCount = 1)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L577) | Issues a procedural non-indexed draw that does not consume a vertex buffer.<br>`vertexCount`：Number of shader-generated vertices.<br>`instanceCount`：Number of instances. | — | — |
| <a id="api-cfaf29a92c60f895"></a>[`public abstract void Inno.Rendering.RenderCommandEncoder.SetRasterState(Inno.Rendering.RenderRasterState state)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L372) | Overrides raster state for subsequent draws in the current pass.<br>`state`：Complete backend-neutral raster state. | — | — |
| <a id="api-5dd6ad01029882c4"></a>[`public abstract void Inno.Rendering.RenderCommandEncoder.SetScissor(int x, int y, int width, int height)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L419) | Restricts subsequent rasterization to a pixel rectangle in the active view.<br>`x`：Left edge in framebuffer pixels.<br>`y`：Top edge in framebuffer pixels.<br>`width`：Positive rectangle width in pixels.<br>`height`：Positive rectangle height in pixels. | — | — |
| <a id="api-4880530fbcdec308"></a>[`public abstract void Inno.Rendering.RenderCommandEncoder.SetStencil(Inno.Rendering.RenderStencilState state)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L380) | Sets two-sided stencil state for subsequent draws.<br>`state`：Complete backend-neutral stencil state. | — | — |
| <a id="api-7dae61f9083aade3"></a>[`public abstract void Inno.Rendering.RenderCommandEncoder.SetTransform(System.ReadOnlySpan<float> columnMajorMatrix)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L364) | Sets the current object transform from one column-major 4x4 matrix.<br>`columnMajorMatrix`：Exactly sixteen floating-point matrix values. | — | — |
| <a id="api-662c485b5e5902cc"></a>[`public abstract void Inno.Rendering.RenderCommandEncoder.SetUniform(Inno.Rendering.RenderBindingId binding, System.ReadOnlySpan<byte> value)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L353) | Uploads one uniform value using manifest-validated bytes.<br>`binding`：Stable shader binding identifier.<br>`value`：Uniform bytes matching the reflected shader interface. | — | — |
| <a id="api-35bfabb6eed2568b"></a>[`public abstract void Inno.Rendering.RenderCommandEncoder.SetViewport(int x, int y, int width, int height)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L397) | Changes the viewport rectangle for the current pass view.<br>`x`：Horizontal framebuffer origin.<br>`y`：Vertical framebuffer origin.<br>`width`：Positive framebuffer width.<br>`height`：Positive framebuffer height. | — | — |
| <a id="api-1a2eb7b0af88d2bd"></a>[`public abstract class Inno.Rendering.RenderCommandEncoder`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L170) | Provides backend-neutral draw, dispatch and copy commands for one compiled pass. | — | — |

### `Inno.Rendering.RenderCullMode`

<!-- api: T:Inno.Rendering.RenderCullMode c682c857c1334e429a9895dc3155dda7648a0df70b4a600471137977036b968e -->
<!-- api: F:Inno.Rendering.RenderCullMode.None 6fc45c1d8b88acfc5f17a03d7ef1bebdefcb503cadd719c7bfcd6e8fef4a95ce -->
<!-- api: F:Inno.Rendering.RenderCullMode.Front 3781b6c3db7ffd515ebd490b21b9db341ea1b3fae5136af19759479326f7a978 -->
<!-- api: F:Inno.Rendering.RenderCullMode.Back ee20c9d0f8e4e5f0b83ccb31c7763bf6ca15e6448e6e8f4cf4e471ad11c1bc15 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8cc3fb9b5b2953e0"></a>[`Inno.Rendering.RenderCullMode.Back = 2`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L23) | Culls back-facing triangles. | — | — |
| <a id="api-e22931c750fcf891"></a>[`Inno.Rendering.RenderCullMode.Front = 1`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L19) | Culls front-facing triangles. | — | — |
| <a id="api-05bbac1d3cdcc8f8"></a>[`Inno.Rendering.RenderCullMode.None = 0`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L15) | Disables face culling. | — | — |
| <a id="api-fe7baa7e03b88242"></a>[`public enum Inno.Rendering.RenderCullMode : int`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L10) | Selects triangle culling for a backend-neutral raster pipeline. | — | — |

### `Inno.Rendering.RenderDataChannelId`

<!-- api: T:Inno.Rendering.RenderDataChannelId 286d568006f2ae04a6ae8d4d06e505f7f8797bb354fb9ff75c7335ab0a14c8a8 -->
<!-- api: M:Inno.Rendering.RenderDataChannelId.#ctor(System.String) b157f8ad48ecaf2337fb863ad2e5209e515872e8df7dc97c723b9ce9ca87b38a -->
<!-- api: P:Inno.Rendering.RenderDataChannelId.value 3babb2338ad143a6476f421c416d2fd6789e1f0a7ebfaae600b27af2d1f15446 -->
<!-- api: P:Inno.Rendering.RenderDataChannelId.isValid 70353720add44a3f71dd5d4f93835bf544991227e65b5e652f53b458f2211832 -->
<!-- api: M:Inno.Rendering.RenderDataChannelId.ToString c41553d7415a048b31ebbee479152bd430938ec2335037ff072dc705008453fa -->
<!-- api: M:Inno.Rendering.RenderDataChannelId.op_Inequality(Inno.Rendering.RenderDataChannelId,Inno.Rendering.RenderDataChannelId) 4ce23d393a61491603c2245e396982668aba840b85b9406d2761e0aa72ae6bf7 -->
<!-- api: M:Inno.Rendering.RenderDataChannelId.op_Equality(Inno.Rendering.RenderDataChannelId,Inno.Rendering.RenderDataChannelId) bcc613796ae55f9fb9de8c69022cad764cc5773485a732375ca57df9c5942dca -->
<!-- api: M:Inno.Rendering.RenderDataChannelId.GetHashCode 3909a10b3eeaa7e4f5c9d8dda8ac676149f2d3d862c4c9e6d683220aa32066c9 -->
<!-- api: M:Inno.Rendering.RenderDataChannelId.Equals(System.Object) 04614408bbbf80a9194ef6ad8f16a2477e7d02cf5a96d86a58048b6b494ab39a -->
<!-- api: M:Inno.Rendering.RenderDataChannelId.Equals(Inno.Rendering.RenderDataChannelId) 46737bd547c21e73bdc5eec25c5ab123d2cf14af39a77a7c5ebd87972b830ea7 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-592dbae695ccac8e"></a>[`public Inno.Rendering.RenderDataChannelId.RenderDataChannelId(string value)`](../../src/services/rendering/Inno.Rendering/Device/RenderProtocolIds.cs#L52) | Creates an open frame-data channel identifier.<br>`value`：Globally stable channel protocol value. | — | — |
| <a id="api-c36fd61b45f9887d"></a>[`public bool Inno.Rendering.RenderDataChannelId.Equals(Inno.Rendering.RenderDataChannelId other)`](../../src/services/rendering/Inno.Rendering/Device/RenderProtocolIds.cs#L44) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-3302af378bd8378f"></a>[`public override bool Inno.Rendering.RenderDataChannelId.Equals(object obj)`](../../src/services/rendering/Inno.Rendering/Device/RenderProtocolIds.cs#L44) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-b4e2ad5eef7d56a9"></a>[`public override int Inno.Rendering.RenderDataChannelId.GetHashCode()`](../../src/services/rendering/Inno.Rendering/Device/RenderProtocolIds.cs#L44) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-13537e4b9f73fd95"></a>[`public static bool Inno.Rendering.RenderDataChannelId.operator ==(Inno.Rendering.RenderDataChannelId left, Inno.Rendering.RenderDataChannelId right)`](../../src/services/rendering/Inno.Rendering/Device/RenderProtocolIds.cs#L44) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-2cafd191db7fd509"></a>[`public static bool Inno.Rendering.RenderDataChannelId.operator !=(Inno.Rendering.RenderDataChannelId left, Inno.Rendering.RenderDataChannelId right)`](../../src/services/rendering/Inno.Rendering/Device/RenderProtocolIds.cs#L44) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-6a7df84236448fbb"></a>[`public override string Inno.Rendering.RenderDataChannelId.ToString()`](../../src/services/rendering/Inno.Rendering/Device/RenderProtocolIds.cs#L74) | Formats this value as a human-readable representation.<br>返回：The human-readable representation of this value. | — | — |
| <a id="api-f82ee8c533b0488b"></a>[`public bool Inno.Rendering.RenderDataChannelId.isValid { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderProtocolIds.cs#L66) | Gets whether the identifier contains a protocol value. | — | — |
| <a id="api-fc3fece4e264ad0f"></a>[`public string Inno.Rendering.RenderDataChannelId.value { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderProtocolIds.cs#L61) | Gets the globally stable protocol value. | — | — |
| <a id="api-cea6a6e0ab1b4cfd"></a>[`public readonly record struct Inno.Rendering.RenderDataChannelId : System.IEquatable<Inno.Rendering.RenderDataChannelId>`](../../src/services/rendering/Inno.Rendering/Device/RenderProtocolIds.cs#L44) | Identifies one pipeline-owned frame-data channel without constraining its payload model. | — | — |

### `Inno.Rendering.RenderDepthCompare`

<!-- api: T:Inno.Rendering.RenderDepthCompare 3d00ab6e4eae7fd0c4a3c6d695311e4686122d56112fe96853ece242608a8a7b -->
<!-- api: F:Inno.Rendering.RenderDepthCompare.Never 76d263340fcef9b00aece4fdb6d78b3bad608c993e9ead096e0dcd0b3a40c24d -->
<!-- api: F:Inno.Rendering.RenderDepthCompare.Less 6424e2917da995708cc5077a5f83c15177de52b98872bcad440986228cbded00 -->
<!-- api: F:Inno.Rendering.RenderDepthCompare.Equal 3c1217928b512bbcbfd0d91cbf583d1bb673c97f13fbc3fa24aab405eea7f2dd -->
<!-- api: F:Inno.Rendering.RenderDepthCompare.LessEqual 9e01c87028a307802f6c2fb482b484f18bd8f69c5238cae9ab305bc9bfaaf92c -->
<!-- api: F:Inno.Rendering.RenderDepthCompare.Greater 7dd127f7e05b7315a040e1dfbcb882df76f0a1ec5ffbf86a47f1fdd3c353ecc1 -->
<!-- api: F:Inno.Rendering.RenderDepthCompare.NotEqual cc19d867b597ffe734d64db82d31d629e920b9e47f79757bec5dd7724778efce -->
<!-- api: F:Inno.Rendering.RenderDepthCompare.GreaterEqual af3804a4fc7b47bdd4d5d92380b3de3c3c142c2c18cc07dfb047a0c3cb281a9a -->
<!-- api: F:Inno.Rendering.RenderDepthCompare.Always fd8e7e4861d2a37599b8b8536b5469f2c2f56126e7c3a3d962c52a0ba0ed8515 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-aedd0705fae1c8d6"></a>[`Inno.Rendering.RenderDepthCompare.Always = 7`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L77) | Always accepts. | — | — |
| <a id="api-823a5a3acff02eaf"></a>[`Inno.Rendering.RenderDepthCompare.Equal = 2`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L57) | Accepts equal depth. | — | — |
| <a id="api-5573bb50e5f4b0b8"></a>[`Inno.Rendering.RenderDepthCompare.Greater = 4`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L65) | Accepts greater depth. | — | — |
| <a id="api-acdbee4baaacb81b"></a>[`Inno.Rendering.RenderDepthCompare.GreaterEqual = 6`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L73) | Accepts greater or equal depth. | — | — |
| <a id="api-7c60ee2fd0f27fd9"></a>[`Inno.Rendering.RenderDepthCompare.Less = 1`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L53) | Accepts smaller depth. | — | — |
| <a id="api-1c7b475a9077550c"></a>[`Inno.Rendering.RenderDepthCompare.LessEqual = 3`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L61) | Accepts smaller or equal depth. | — | — |
| <a id="api-dca57aa54a763d54"></a>[`Inno.Rendering.RenderDepthCompare.Never = 0`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L49) | Always rejects. | — | — |
| <a id="api-5405bb65c24ef284"></a>[`Inno.Rendering.RenderDepthCompare.NotEqual = 5`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L69) | Accepts unequal depth. | — | — |
| <a id="api-72f68b42c00f3c7c"></a>[`public enum Inno.Rendering.RenderDepthCompare : int`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L44) | Selects depth comparison for a backend-neutral raster pipeline. | — | — |

### `Inno.Rendering.RenderDevice`

<!-- api: T:Inno.Rendering.RenderDevice 3a2a10f124b551a385795414a1cfb5b8d4db395cbab060b43370ff09b66fcb44 -->
<!-- api: M:Inno.Rendering.RenderDevice.CreatePersistentTextureHandle(System.UInt64,System.UInt32) 712b1396f5804f6e75a6d1b4344ad9be1b40c4890cfde7bacdccfb4934601c4e -->
<!-- api: M:Inno.Rendering.RenderDevice.CreatePersistentBufferHandle(System.UInt64,System.UInt32) 1ca43ceade569a29108deea35497d3d8156e68cf718b5d220e387c738a318870 -->
<!-- api: M:Inno.Rendering.RenderDevice.CreateGraphicsPipelineHandle(System.UInt64,System.UInt32) 7f73f3a430bebd0707ef97e80a62f597e331e1ce3f086c6b9d86603f81ae1e22 -->
<!-- api: M:Inno.Rendering.RenderDevice.CreateComputePipelineHandle(System.UInt64,System.UInt32) 0f61bf72761c758bac185987157b5d42947e2894a28eb387ebf6d25de356ee75 -->
<!-- api: M:Inno.Rendering.RenderDevice.CreateRenderSurfaceHandle(System.UInt64,System.UInt32) 628b6ad13a9ef4188ac1248e94687a05f1f2715226b75646af8a3611b511e26c -->
<!-- api: M:Inno.Rendering.RenderDevice.CreateRenderTextureReadbackHandle(System.UInt64,System.UInt32) f6f1582d58a58276a1acf07ee4aa4425da2ea90eb2afe6edd24846a4b6ab7e00 -->
<!-- api: M:Inno.Rendering.RenderDevice.GetHandleIdentity(Inno.Rendering.PersistentTextureHandle) 9d756d983642b96c59a6881afd7734d216466f8e15e52eea953ef698f6d29e6b -->
<!-- api: M:Inno.Rendering.RenderDevice.GetHandleIdentity(Inno.Rendering.PersistentBufferHandle) b1059cfb7da32f1ec7fc365096ce7b646924cdfff497dec5b64dfe2142c85ca3 -->
<!-- api: M:Inno.Rendering.RenderDevice.GetHandleIdentity(Inno.Rendering.GraphicsPipelineHandle) 841f614567117c29c9c4f1210093658270497569662a0e85b8c6f2ee94ea8665 -->
<!-- api: M:Inno.Rendering.RenderDevice.GetHandleIdentity(Inno.Rendering.ComputePipelineHandle) 848392ffb2b10277de5495de3b722713386ad961deb8d22dc0764e0dec2b2dde -->
<!-- api: M:Inno.Rendering.RenderDevice.GetHandleIdentity(Inno.Rendering.RenderSurfaceHandle) da6af7f869f2dd8d258980e1ba4574cde78b6293264d57b512a4f519a38bd43e -->
<!-- api: M:Inno.Rendering.RenderDevice.GetHandleIdentity(Inno.Rendering.RenderTextureReadbackHandle) f1747495b20b3e2b1974f88d9174dc6db664fb0d4b1bf1d23876c235d6d7136a -->
<!-- api: M:Inno.Rendering.RenderDevice.GetHandleIdentity(Inno.Rendering.RenderTextureHandle) 9fd31e60a46f16a226c2bd7343ce089e24a91e285c822fd99c9c0ae371e7d159 -->
<!-- api: M:Inno.Rendering.RenderDevice.GetHandleIdentity(Inno.Rendering.RenderBufferHandle) 0b65477e41e742261dfdb83b4a8a0c13e5bb7debc51fc949fdb4fb2f13c65cd2 -->
<!-- api: M:Inno.Rendering.RenderDevice.#ctor 535ffb732b4a4a14e35e197ef556dd6b6fd47109a9b6ca459eea7606c91b0c70 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b55ee88d5990c7a4"></a>[`protected Inno.Rendering.RenderDevice.RenderDevice()`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L10) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-731a1f837a03d959"></a>[`protected static Inno.Rendering.ComputePipelineHandle Inno.Rendering.RenderDevice.CreateComputePipelineHandle(ulong value, uint generation)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L103) | Encodes a compute pipeline identity into a backend-neutral handle.<br>返回：An opaque compute pipeline handle.<br>`value`：The backend-owned non-zero pipeline identity.<br>`generation`：The device generation that owns the pipeline. | — | — |
| <a id="api-ffd5eeeda87a358f"></a>[`protected static Inno.Rendering.GraphicsPipelineHandle Inno.Rendering.RenderDevice.CreateGraphicsPipelineHandle(ulong value, uint generation)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L86) | Encodes a graphics pipeline identity into a backend-neutral handle.<br>返回：An opaque graphics pipeline handle.<br>`value`：The backend-owned non-zero pipeline identity.<br>`generation`：The device generation that owns the pipeline. | — | — |
| <a id="api-ef3179c348c7d0e7"></a>[`protected static Inno.Rendering.PersistentBufferHandle Inno.Rendering.RenderDevice.CreatePersistentBufferHandle(ulong value, uint generation)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L69) | Encodes a persistent buffer identity into a backend-neutral handle.<br>返回：An opaque buffer handle suitable for the public render-device contract.<br>`value`：The backend-owned non-zero buffer identity.<br>`generation`：The device generation that owns the buffer. | — | — |
| <a id="api-0639e2111c810d09"></a>[`protected static Inno.Rendering.PersistentTextureHandle Inno.Rendering.RenderDevice.CreatePersistentTextureHandle(ulong value, uint generation)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L52) | Encodes a persistent texture identity into a backend-neutral handle.<br>返回：An opaque texture handle suitable for the public render-device contract.<br>`value`：The backend-owned non-zero texture identity.<br>`generation`：The device generation that owns the texture. | — | — |
| <a id="api-45e0dcdafec17db6"></a>[`protected static Inno.Rendering.RenderSurfaceHandle Inno.Rendering.RenderDevice.CreateRenderSurfaceHandle(ulong value, uint generation)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L120) | Encodes a presentation-surface identity into a backend-neutral handle.<br>返回：An opaque presentation-surface handle.<br>`value`：The backend-owned non-zero surface identity.<br>`generation`：The device generation that owns the surface. | — | — |
| <a id="api-f54692960808e003"></a>[`protected static Inno.Rendering.RenderTextureReadbackHandle Inno.Rendering.RenderDevice.CreateRenderTextureReadbackHandle(ulong value, uint generation)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L137) | Encodes a texture-readback identity into a backend-neutral handle.<br>返回：An opaque texture-readback handle.<br>`value`：The backend-owned non-zero readback identity.<br>`generation`：The device generation that owns the readback. | — | — |
| <a id="api-73680677dd4463fc"></a>[`protected static Inno.Rendering.RenderDevice.DeviceHandleIdentity Inno.Rendering.RenderDevice.GetHandleIdentity(Inno.Rendering.ComputePipelineHandle handle)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L184) | Decodes a compute pipeline handle for backend lookup and generation validation.<br>返回：The backend resource identity and owning device generation.<br>`handle`：The backend-neutral compute pipeline handle. | — | — |
| <a id="api-b5a078fba714b827"></a>[`protected static Inno.Rendering.RenderDevice.DeviceHandleIdentity Inno.Rendering.RenderDevice.GetHandleIdentity(Inno.Rendering.GraphicsPipelineHandle handle)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L173) | Decodes a graphics pipeline handle for backend lookup and generation validation.<br>返回：The backend resource identity and owning device generation.<br>`handle`：The backend-neutral graphics pipeline handle. | — | — |
| <a id="api-d205a0a73de28770"></a>[`protected static Inno.Rendering.RenderDevice.DeviceHandleIdentity Inno.Rendering.RenderDevice.GetHandleIdentity(Inno.Rendering.PersistentBufferHandle handle)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L162) | Decodes a persistent buffer handle for backend lookup and generation validation.<br>返回：The backend resource identity and owning device generation.<br>`handle`：The backend-neutral buffer handle. | — | — |
| <a id="api-ee83545d96839426"></a>[`protected static Inno.Rendering.RenderDevice.DeviceHandleIdentity Inno.Rendering.RenderDevice.GetHandleIdentity(Inno.Rendering.PersistentTextureHandle handle)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L151) | Decodes a persistent texture handle for backend lookup and generation validation.<br>返回：The backend resource identity and owning device generation.<br>`handle`：The backend-neutral texture handle. | — | — |
| <a id="api-276fabb0de0c1ace"></a>[`protected static Inno.Rendering.RenderDevice.GraphHandleIdentity Inno.Rendering.RenderDevice.GetHandleIdentity(Inno.Rendering.RenderBufferHandle handle)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L229) | Decodes a frame-scoped buffer handle for graph resource lookup.<br>返回：The graph resource index and owning graph generation.<br>`handle`：The backend-neutral graph buffer handle. | — | — |
| <a id="api-4fa172aedb53f2d5"></a>[`protected static Inno.Rendering.RenderDevice.DeviceHandleIdentity Inno.Rendering.RenderDevice.GetHandleIdentity(Inno.Rendering.RenderSurfaceHandle handle)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L195) | Decodes a presentation-surface handle for backend lookup and generation validation.<br>返回：The backend resource identity and owning device generation.<br>`handle`：The backend-neutral presentation-surface handle. | — | — |
| <a id="api-1cf6fccc58cd195d"></a>[`protected static Inno.Rendering.RenderDevice.GraphHandleIdentity Inno.Rendering.RenderDevice.GetHandleIdentity(Inno.Rendering.RenderTextureHandle handle)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L218) | Decodes a frame-scoped texture handle for graph resource lookup.<br>返回：The graph resource index and owning graph generation.<br>`handle`：The backend-neutral graph texture handle. | — | — |
| <a id="api-05fd8aa6e91cc3d2"></a>[`protected static Inno.Rendering.RenderDevice.DeviceHandleIdentity Inno.Rendering.RenderDevice.GetHandleIdentity(Inno.Rendering.RenderTextureReadbackHandle handle)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L206) | Decodes a texture-readback handle for backend lookup and generation validation.<br>返回：The backend resource identity and owning device generation.<br>`handle`：The backend-neutral texture-readback handle. | — | — |
| <a id="api-2055e89f022237d3"></a>[`public abstract class Inno.Rendering.RenderDevice`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L10) | Provides the protected opaque-handle boundary required by replaceable render-device backends. | Backend implementations derive from this type so they can encode and validate backend-neutral handles without exposing raw backend identities to application or scripting code. | — |

### `Inno.Rendering.RenderDevice.DeviceHandleIdentity`

<!-- api: T:Inno.Rendering.RenderDevice.DeviceHandleIdentity 22608d065aaec601174877d4f2350ed3b07fbb5b2af985e35c793d51463f5743 -->
<!-- api: M:Inno.Rendering.RenderDevice.DeviceHandleIdentity.#ctor(System.UInt64,System.UInt32) 536475d0f4b5721967977b9a4bc7a88ea45fe4845dab894f28f800919cd2b5d4 -->
<!-- api: P:Inno.Rendering.RenderDevice.DeviceHandleIdentity.value 7c958107293b3a5042f1139fbce8f96f26fcff270d6977dc3fd862a98c52d85a -->
<!-- api: P:Inno.Rendering.RenderDevice.DeviceHandleIdentity.generation 79a0a2aeab6171008dd44379691f36e134e4a2010aa994c55378928474d063ed -->
<!-- api: M:Inno.Rendering.RenderDevice.DeviceHandleIdentity.ToString 7781704e1754e6465f7e6e8e28881b85604c0fda7d2758b5c0a17f03de2f3e36 -->
<!-- api: M:Inno.Rendering.RenderDevice.DeviceHandleIdentity.op_Inequality(Inno.Rendering.RenderDevice.DeviceHandleIdentity,Inno.Rendering.RenderDevice.DeviceHandleIdentity) c5faa6a418fa623966387d0dc4acfa5307aea84545de7ee32ca45382741bb150 -->
<!-- api: M:Inno.Rendering.RenderDevice.DeviceHandleIdentity.op_Equality(Inno.Rendering.RenderDevice.DeviceHandleIdentity,Inno.Rendering.RenderDevice.DeviceHandleIdentity) 96c25f232a5ff5f66a28b84b2c0a5c360bb66756037d6f49bd342a2f429c7bc7 -->
<!-- api: M:Inno.Rendering.RenderDevice.DeviceHandleIdentity.GetHashCode 0b638ef7e68913f933f87daea7b64f83ef0f73ee2de617990b937ee31ca9ed1b -->
<!-- api: M:Inno.Rendering.RenderDevice.DeviceHandleIdentity.Equals(System.Object) 9d0a84bebf4cab1b3b742bd9d4b5831eb1488d0fbb61696868bbf5f1c315bb4d -->
<!-- api: M:Inno.Rendering.RenderDevice.DeviceHandleIdentity.Equals(Inno.Rendering.RenderDevice.DeviceHandleIdentity) 015c1cc5969425c2ca9a35a520d329c089f3ac84e57098be2bddfbad175dde9a -->
<!-- api: M:Inno.Rendering.RenderDevice.DeviceHandleIdentity.Deconstruct(System.UInt64@,System.UInt32@) 275bdb69f0147c2d318870ca72923fb3ceebfdb24c179a58416a8dff07bbb78b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d887c7562a592805"></a>[`public Inno.Rendering.RenderDevice.DeviceHandleIdentity.DeviceHandleIdentity(ulong value, uint generation)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L21) | Stores one decoded persistent device identity for use inside a concrete backend.<br>`value`：The backend-owned non-zero resource identity.<br>`generation`：The device generation that owns the resource. | — | — |
| <a id="api-8164b6d20f485785"></a>[`public void Inno.Rendering.RenderDevice.DeviceHandleIdentity.Deconstruct(out ulong value, out uint generation)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L21) | C# record 生成的解构入口，按主构造声明顺序返回位置成员。 | — | — |
| <a id="api-49ecf0a7b8515d42"></a>[`public bool Inno.Rendering.RenderDevice.DeviceHandleIdentity.Equals(Inno.Rendering.RenderDevice.DeviceHandleIdentity other)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L21) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-afd7da4e5b637c5f"></a>[`public override bool Inno.Rendering.RenderDevice.DeviceHandleIdentity.Equals(object obj)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L21) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-1e7004dd3d4f9c1f"></a>[`public override int Inno.Rendering.RenderDevice.DeviceHandleIdentity.GetHashCode()`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L21) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-a5c18736234c345d"></a>[`public static bool Inno.Rendering.RenderDevice.DeviceHandleIdentity.operator ==(Inno.Rendering.RenderDevice.DeviceHandleIdentity left, Inno.Rendering.RenderDevice.DeviceHandleIdentity right)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L21) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-03d4ad2ca0632f93"></a>[`public static bool Inno.Rendering.RenderDevice.DeviceHandleIdentity.operator !=(Inno.Rendering.RenderDevice.DeviceHandleIdentity left, Inno.Rendering.RenderDevice.DeviceHandleIdentity right)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L21) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-af3ec10cc0e15b99"></a>[`public override string Inno.Rendering.RenderDevice.DeviceHandleIdentity.ToString()`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L21) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-cdc13ab22eea0aee"></a>[`public uint Inno.Rendering.RenderDevice.DeviceHandleIdentity.generation { get; init; }`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L23) | The device generation that owns the resource. | — | — |
| <a id="api-75e0ea60b3ad8f78"></a>[`public ulong Inno.Rendering.RenderDevice.DeviceHandleIdentity.value { get; init; }`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L22) | The backend-owned non-zero resource identity. | — | — |
| <a id="api-0a44a2b8e14174d0"></a>[`protected readonly record struct Inno.Rendering.RenderDevice.DeviceHandleIdentity : System.IEquatable<Inno.Rendering.RenderDevice.DeviceHandleIdentity>`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L21) | Stores one decoded persistent device identity for use inside a concrete backend.<br>`value`：The backend-owned non-zero resource identity.<br>`generation`：The device generation that owns the resource. | — | — |

### `Inno.Rendering.RenderDevice.GraphHandleIdentity`

<!-- api: T:Inno.Rendering.RenderDevice.GraphHandleIdentity 98d7eb7677a3db7356f2832f82849d8fe02f3383709ed332be041e77a32d18bc -->
<!-- api: M:Inno.Rendering.RenderDevice.GraphHandleIdentity.#ctor(System.Int32,System.UInt32) c3fa488f87989499413f3a17a9e5bcd1fc46ca23f4d7c37ce3bf5e55cc83b610 -->
<!-- api: P:Inno.Rendering.RenderDevice.GraphHandleIdentity.index cc9b933c5a5b6c13cf9ff24ccb7dbc6d70d8ca0ba2e768d64f8fa48ed331247c -->
<!-- api: P:Inno.Rendering.RenderDevice.GraphHandleIdentity.generation 01e87e46d4a2c801bf60b9102721af332b47aad704abbde64199a043f9ee3b84 -->
<!-- api: M:Inno.Rendering.RenderDevice.GraphHandleIdentity.ToString e433fbe58f4881be5e4984c61ffdd3490886f07a666623d5441c9748d72eb720 -->
<!-- api: M:Inno.Rendering.RenderDevice.GraphHandleIdentity.op_Inequality(Inno.Rendering.RenderDevice.GraphHandleIdentity,Inno.Rendering.RenderDevice.GraphHandleIdentity) 49bd063259ddcf58b25d839a6b63a9d091d2d0756dce3887fefb9d0b53934a55 -->
<!-- api: M:Inno.Rendering.RenderDevice.GraphHandleIdentity.op_Equality(Inno.Rendering.RenderDevice.GraphHandleIdentity,Inno.Rendering.RenderDevice.GraphHandleIdentity) 0f38faddbf432df482275787fbc33a2fadb6c125a9d9cdb14652799fffad40ef -->
<!-- api: M:Inno.Rendering.RenderDevice.GraphHandleIdentity.GetHashCode bf6d5dacfcdd67b3430fbf79b446fb0bfadd9425b4a96b25f11c1e556fb619ea -->
<!-- api: M:Inno.Rendering.RenderDevice.GraphHandleIdentity.Equals(System.Object) 3747be2208544c0daa0a7758792e08c5b4cd15e59d6e4a8f2d9cf6ba04d010db -->
<!-- api: M:Inno.Rendering.RenderDevice.GraphHandleIdentity.Equals(Inno.Rendering.RenderDevice.GraphHandleIdentity) 3bbc62aba8645d3bd29f3dd3df37b43d021c0403520b56aa762df6cd04376981 -->
<!-- api: M:Inno.Rendering.RenderDevice.GraphHandleIdentity.Deconstruct(System.Int32@,System.UInt32@) edf02b8754d294980871bf83ca430b68af25cddad5ee19fdbb7013e0ac631e7d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c12a52880639f490"></a>[`public Inno.Rendering.RenderDevice.GraphHandleIdentity.GraphHandleIdentity(int index, uint generation)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L35) | Stores one decoded frame-scoped graph identity for use inside a concrete backend.<br>`index`：The zero-based logical resource index.<br>`generation`：The render-graph generation that owns the resource. | — | — |
| <a id="api-9b9608a4e179f5ba"></a>[`public void Inno.Rendering.RenderDevice.GraphHandleIdentity.Deconstruct(out int index, out uint generation)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L35) | C# record 生成的解构入口，按主构造声明顺序返回位置成员。 | — | — |
| <a id="api-edf8ffe0638faf98"></a>[`public bool Inno.Rendering.RenderDevice.GraphHandleIdentity.Equals(Inno.Rendering.RenderDevice.GraphHandleIdentity other)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L35) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-0dd5e8fe35f36f28"></a>[`public override bool Inno.Rendering.RenderDevice.GraphHandleIdentity.Equals(object obj)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L35) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-1e0fb9bf9b829b9e"></a>[`public override int Inno.Rendering.RenderDevice.GraphHandleIdentity.GetHashCode()`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L35) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-cf4f7437d0b30719"></a>[`public static bool Inno.Rendering.RenderDevice.GraphHandleIdentity.operator ==(Inno.Rendering.RenderDevice.GraphHandleIdentity left, Inno.Rendering.RenderDevice.GraphHandleIdentity right)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L35) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-b3b86c40beee8031"></a>[`public static bool Inno.Rendering.RenderDevice.GraphHandleIdentity.operator !=(Inno.Rendering.RenderDevice.GraphHandleIdentity left, Inno.Rendering.RenderDevice.GraphHandleIdentity right)`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L35) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-fffd566a890c6d5a"></a>[`public override string Inno.Rendering.RenderDevice.GraphHandleIdentity.ToString()`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L35) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-6ef76f8bdda8a40c"></a>[`public uint Inno.Rendering.RenderDevice.GraphHandleIdentity.generation { get; init; }`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L37) | The render-graph generation that owns the resource. | — | — |
| <a id="api-db53e22931d5dd87"></a>[`public int Inno.Rendering.RenderDevice.GraphHandleIdentity.index { get; init; }`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L36) | The zero-based logical resource index. | — | — |
| <a id="api-696466663ee6d01f"></a>[`protected readonly record struct Inno.Rendering.RenderDevice.GraphHandleIdentity : System.IEquatable<Inno.Rendering.RenderDevice.GraphHandleIdentity>`](../../src/services/rendering/Inno.Rendering/Device/RenderDevice.cs#L35) | Stores one decoded frame-scoped graph identity for use inside a concrete backend.<br>`index`：The zero-based logical resource index.<br>`generation`：The render-graph generation that owns the resource. | — | — |

### `Inno.Rendering.RenderDeviceAllocationCounters`

<!-- api: T:Inno.Rendering.RenderDeviceAllocationCounters d1e5e0843fcfd08a9030e1cf2998453855939af83caee243dd8f5c0b5c084c06 -->
<!-- api: M:Inno.Rendering.RenderDeviceAllocationCounters.#ctor(System.UInt32,System.UInt64,System.UInt64,System.UInt64) 830f6e44eaf96d682640a5d16e22fefa6eb4af639d3de5310aa8fc42b942578f -->
<!-- api: P:Inno.Rendering.RenderDeviceAllocationCounters.deviceGeneration 52cd1a9b5f6f891681051f5af21e94bd7aae7c64cfcdf93fc508dca466de85c6 -->
<!-- api: P:Inno.Rendering.RenderDeviceAllocationCounters.textureAllocations 7e74348937b928494927df95af8ca2fd889b2230f0a1c7292f92e7370f0ab61c -->
<!-- api: P:Inno.Rendering.RenderDeviceAllocationCounters.bufferAllocations 5f36b420b7e94efa85299f3807bc4785932b7b67deb90f26d2694af8bae07d64 -->
<!-- api: P:Inno.Rendering.RenderDeviceAllocationCounters.frameBufferAllocations 6465425f94415dd2df5f30456ca2f14407d52b01eb306b62d3fa2d4581881376 -->
<!-- api: M:Inno.Rendering.RenderDeviceAllocationCounters.ToString 90d0d2702a8c74d7a3732994a7d5356dc3c9d505dbebefa42ec6e5721fe3d2e7 -->
<!-- api: M:Inno.Rendering.RenderDeviceAllocationCounters.op_Inequality(Inno.Rendering.RenderDeviceAllocationCounters,Inno.Rendering.RenderDeviceAllocationCounters) 56bfe3df3be3454c982d0d400e900c4dd7a3f61ecb8444c04faf2698c189dd79 -->
<!-- api: M:Inno.Rendering.RenderDeviceAllocationCounters.op_Equality(Inno.Rendering.RenderDeviceAllocationCounters,Inno.Rendering.RenderDeviceAllocationCounters) 87cfaf8fc1af74c2b544fc308ac1bcd665fc43312daa99f273720fad609d2ecd -->
<!-- api: M:Inno.Rendering.RenderDeviceAllocationCounters.GetHashCode 37bf0acd3b36b2472159fcfb72bc3303a76b18ceb5a088744ab1386d8f7de342 -->
<!-- api: M:Inno.Rendering.RenderDeviceAllocationCounters.Equals(System.Object) 3e2cd198054d99316e9b9a14b91e23af1cbc4f8320f519532f1f5abb62523e4e -->
<!-- api: M:Inno.Rendering.RenderDeviceAllocationCounters.Equals(Inno.Rendering.RenderDeviceAllocationCounters) e1c5463d6c5c611ddac2fa88ec6d59c7386fdeb9d02651a8f0a034d31ddc9400 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9a01825475435fde"></a>[`public Inno.Rendering.RenderDeviceAllocationCounters.RenderDeviceAllocationCounters(uint deviceGeneration, ulong textureAllocations, ulong bufferAllocations, ulong frameBufferAllocations)`](../../src/services/rendering/Inno.Rendering/Device/RenderDeviceAllocationCounters.cs#L30) | Creates an immutable allocation snapshot scoped to one device generation.<br>`deviceGeneration`：Non-zero generation that owns the measured pools; counters restart for a new generation.<br>`textureAllocations`：Successful native transient texture allocations since device creation.<br>`bufferAllocations`：Successful native transient buffer allocations since device creation.<br>`frameBufferAllocations`：Successful native transient attachment framebuffer allocations since device creation. | — | `System.ArgumentOutOfRangeException`：The device generation is zero. |
| <a id="api-6431eae5df214843"></a>[`public bool Inno.Rendering.RenderDeviceAllocationCounters.Equals(Inno.Rendering.RenderDeviceAllocationCounters other)`](../../src/services/rendering/Inno.Rendering/Device/RenderDeviceAllocationCounters.cs#L10) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-ee08c5552b7ed050"></a>[`public override bool Inno.Rendering.RenderDeviceAllocationCounters.Equals(object obj)`](../../src/services/rendering/Inno.Rendering/Device/RenderDeviceAllocationCounters.cs#L10) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-efc3acd3fa090c0e"></a>[`public override int Inno.Rendering.RenderDeviceAllocationCounters.GetHashCode()`](../../src/services/rendering/Inno.Rendering/Device/RenderDeviceAllocationCounters.cs#L10) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-b57b44abfde7f0cd"></a>[`public static bool Inno.Rendering.RenderDeviceAllocationCounters.operator ==(Inno.Rendering.RenderDeviceAllocationCounters left, Inno.Rendering.RenderDeviceAllocationCounters right)`](../../src/services/rendering/Inno.Rendering/Device/RenderDeviceAllocationCounters.cs#L10) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-de28b8823cfe2b02"></a>[`public static bool Inno.Rendering.RenderDeviceAllocationCounters.operator !=(Inno.Rendering.RenderDeviceAllocationCounters left, Inno.Rendering.RenderDeviceAllocationCounters right)`](../../src/services/rendering/Inno.Rendering/Device/RenderDeviceAllocationCounters.cs#L10) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-b7364d469aab0380"></a>[`public override string Inno.Rendering.RenderDeviceAllocationCounters.ToString()`](../../src/services/rendering/Inno.Rendering/Device/RenderDeviceAllocationCounters.cs#L10) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-25916eb426f87a51"></a>[`public ulong Inno.Rendering.RenderDeviceAllocationCounters.bufferAllocations { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderDeviceAllocationCounters.cs#L56) | Gets successful native transient buffer allocations since device creation. | — | — |
| <a id="api-33ea28f67726fc3e"></a>[`public uint Inno.Rendering.RenderDeviceAllocationCounters.deviceGeneration { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderDeviceAllocationCounters.cs#L46) | Gets the device generation whose cumulative counters are represented by this snapshot. | — | — |
| <a id="api-b43312f408ae0c72"></a>[`public ulong Inno.Rendering.RenderDeviceAllocationCounters.frameBufferAllocations { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderDeviceAllocationCounters.cs#L61) | Gets successful native transient attachment framebuffer allocations since device creation. | — | — |
| <a id="api-3f034d23412097da"></a>[`public ulong Inno.Rendering.RenderDeviceAllocationCounters.textureAllocations { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderDeviceAllocationCounters.cs#L51) | Gets successful native transient texture allocations since device creation. | — | — |
| <a id="api-3aaaed440a9febd0"></a>[`public readonly record struct Inno.Rendering.RenderDeviceAllocationCounters : System.IEquatable<Inno.Rendering.RenderDeviceAllocationCounters>`](../../src/services/rendering/Inno.Rendering/Device/RenderDeviceAllocationCounters.cs#L10) | Reports cumulative successful native allocations made by a device's transient graph resource pools. Pool reuse does not increment these counters; retirement does not decrement them. They are not live resource counts, managed allocation counts, byte measurements or per-frame command counts. | — | — |

### `Inno.Rendering.RenderDeviceFrameCounters`

<!-- api: T:Inno.Rendering.RenderDeviceFrameCounters 6b5383af1058e9a9aabc10efacfe6045393723cdcbd3f3fc9214e631ea0068c4 -->
<!-- api: M:Inno.Rendering.RenderDeviceFrameCounters.#ctor(System.Int32,System.Int32) f1cb55cae912079f3e23b632e21ec9bfe222e297d3a8dff280aad62f8521ebe4 -->
<!-- api: P:Inno.Rendering.RenderDeviceFrameCounters.drawCount 6d71703293660ae5c366194dfd951a7929898a298317b0d7bb2916518dd562c3 -->
<!-- api: P:Inno.Rendering.RenderDeviceFrameCounters.dispatchCount 1bf21c640a9ad3e4a522ac4972d063f2a657ef33375864105147373db254b28e -->
<!-- api: M:Inno.Rendering.RenderDeviceFrameCounters.ToString 155438c6cc9c8b89695a962432040354a37d298c8300bd6ea19c0445ff57b322 -->
<!-- api: M:Inno.Rendering.RenderDeviceFrameCounters.op_Inequality(Inno.Rendering.RenderDeviceFrameCounters,Inno.Rendering.RenderDeviceFrameCounters) 9f55745ab9cd270483395fff51275ca8e18b7855b439d517483664cbec4d29c8 -->
<!-- api: M:Inno.Rendering.RenderDeviceFrameCounters.op_Equality(Inno.Rendering.RenderDeviceFrameCounters,Inno.Rendering.RenderDeviceFrameCounters) 0249902ac76df9c9f7e16204ee87ab336bd834a92cca04fc679319f0bf3e5484 -->
<!-- api: M:Inno.Rendering.RenderDeviceFrameCounters.GetHashCode e95f8919370dac8fe5f3273a7b781094c8f8d08fc25f4a7b99b48c52eb68c831 -->
<!-- api: M:Inno.Rendering.RenderDeviceFrameCounters.Equals(System.Object) 16f391368bf9804a4eb5e93cd35a9f66b2af982beef002b3cf11bfa3b6496df3 -->
<!-- api: M:Inno.Rendering.RenderDeviceFrameCounters.Equals(Inno.Rendering.RenderDeviceFrameCounters) 4ec2468760f8667da1e2b2ce5ef875841adae7ae45defa2aa388766295fd5a93 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-77328a58677adc60"></a>[`public Inno.Rendering.RenderDeviceFrameCounters.RenderDeviceFrameCounters(int drawCount, int dispatchCount)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L30) | Creates immutable device frame counters.<br>`drawCount`：Direct and indirect draw commands submitted so far.<br>`dispatchCount`：Direct and indirect compute dispatches submitted so far. | — | — |
| <a id="api-b75352ac65cb7d22"></a>[`public bool Inno.Rendering.RenderDeviceFrameCounters.Equals(Inno.Rendering.RenderDeviceFrameCounters other)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L19) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-c3141857a3e20f37"></a>[`public override bool Inno.Rendering.RenderDeviceFrameCounters.Equals(object obj)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L19) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-0c41a57a944d4fa0"></a>[`public override int Inno.Rendering.RenderDeviceFrameCounters.GetHashCode()`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L19) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-18f24b7515684096"></a>[`public static bool Inno.Rendering.RenderDeviceFrameCounters.operator ==(Inno.Rendering.RenderDeviceFrameCounters left, Inno.Rendering.RenderDeviceFrameCounters right)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L19) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-886e7135ca4a4ae4"></a>[`public static bool Inno.Rendering.RenderDeviceFrameCounters.operator !=(Inno.Rendering.RenderDeviceFrameCounters left, Inno.Rendering.RenderDeviceFrameCounters right)`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L19) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-02dd27675a8f3ac3"></a>[`public override string Inno.Rendering.RenderDeviceFrameCounters.ToString()`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L19) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-f99e9cf78ae25708"></a>[`public int Inno.Rendering.RenderDeviceFrameCounters.dispatchCount { get; }`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L48) | Gets direct and indirect compute dispatches submitted during the current frame. | — | — |
| <a id="api-f697095a8c300b06"></a>[`public int Inno.Rendering.RenderDeviceFrameCounters.drawCount { get; }`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L43) | Gets direct and indirect draw commands submitted during the current frame. | — | — |
| <a id="api-d2b057ab266e1ff8"></a>[`public readonly record struct Inno.Rendering.RenderDeviceFrameCounters : System.IEquatable<Inno.Rendering.RenderDeviceFrameCounters>`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L19) | Contains backend-recorded GPU command submissions for the current frame. | — | — |

### `Inno.Rendering.RenderFrameUploadProvider`

<!-- api: T:Inno.Rendering.RenderFrameUploadProvider 3955691de93596bdd2b5097313f202d7fc7533902312cba1665f842b38b76c70 -->
<!-- api: M:Inno.Rendering.RenderFrameUploadProvider.CreateBufferSlice(Inno.Rendering.PersistentBufferHandle,System.Int32,System.Int32,Inno.Rendering.RenderBufferUsage,System.UInt64) a0525bdf667c3546896a867dd348a7e9fa224e2a5da0ddc64f5d7b7592e6fb2e -->
<!-- api: M:Inno.Rendering.RenderFrameUploadProvider.#ctor d8dcd81080c47cec67b4b16eea16aea4e580bc9d17285c940ce6a5b7526bd22d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-ee5e792fae52483d"></a>[`protected Inno.Rendering.RenderFrameUploadProvider.RenderFrameUploadProvider()`](../../src/services/rendering/Inno.Rendering/Device/RenderBufferUploads.cs#L127) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-089be4f77ff37626"></a>[`protected static Inno.Rendering.RenderBufferSlice Inno.Rendering.RenderFrameUploadProvider.CreateBufferSlice(Inno.Rendering.PersistentBufferHandle buffer, int firstElement, int elementCount, Inno.Rendering.RenderBufferUsage usage, ulong frameIndex)`](../../src/services/rendering/Inno.Rendering/Device/RenderBufferUploads.cs#L150) | Creates a frame-scoped slice over a provider-owned persistent upload buffer.<br>返回：An opaque frame-scoped buffer slice.<br>`buffer`：The persistent upload page backing the slice.<br>`firstElement`：The zero-based first uploaded element.<br>`elementCount`：The positive number of uploaded elements.<br>`usage`：The GPU uses supported by the uploaded range.<br>`frameIndex`：The render frame during which the slice remains valid. | — | — |
| <a id="api-1ab289b3c7e3cba7"></a>[`public abstract class Inno.Rendering.RenderFrameUploadProvider`](../../src/services/rendering/Inno.Rendering/Device/RenderBufferUploads.cs#L127) | Provides protected slice construction for replaceable frame-upload service implementations. | — | — |

### `Inno.Rendering.RenderFrontFace`

<!-- api: T:Inno.Rendering.RenderFrontFace 6efaa3a53ae0de6765cc981145c84590972432db3034c18c3cd4c85b4bb0fdde -->
<!-- api: F:Inno.Rendering.RenderFrontFace.Clockwise 3252a330ba952f6186e38263bb8a7f22c6d7cc16ed3da6b6e60520a76be3cfae -->
<!-- api: F:Inno.Rendering.RenderFrontFace.CounterClockwise b5e725a411f871e83e1ed9fd88fb86ec7234c0cd56d27e8f20298e90209000eb -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f08d1e134a749dc1"></a>[`Inno.Rendering.RenderFrontFace.Clockwise = 0`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L34) | Clockwise vertices form a front-facing triangle. | — | — |
| <a id="api-267c3966117c41d0"></a>[`Inno.Rendering.RenderFrontFace.CounterClockwise = 1`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L38) | Counter-clockwise vertices form a front-facing triangle. | — | — |
| <a id="api-ba18040b5f39e961"></a>[`public enum Inno.Rendering.RenderFrontFace : int`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L29) | Selects the winding order interpreted as the front face. | — | — |

### `Inno.Rendering.RenderGraphBuilder`

<!-- api: T:Inno.Rendering.RenderGraphBuilder 619fd095b19a8bad7fa9a809198c967688fd7992162dee6b2f499c51a1b9087b -->
<!-- api: M:Inno.Rendering.RenderGraphBuilder.#ctor(System.UInt32,Inno.Rendering.GraphicsCapabilities) dc018db8963bc4ca878138e43ff6648d4544ab0f17e6152b5a252461dede2791 -->
<!-- api: M:Inno.Rendering.RenderGraphBuilder.CreateTexture(System.String,Inno.Rendering.RenderTextureDescriptor) f7d9a2d6787278c28ef0850b798999944da8d49266b2ec5562ad74b375778ce0 -->
<!-- api: M:Inno.Rendering.RenderGraphBuilder.ImportTexture(System.String,Inno.Rendering.PersistentTextureHandle,Inno.Rendering.RenderTextureDescriptor) ef2eb906f602dbf9210b487329e9fb989cffc172883db6004f7254b56f14dcd4 -->
<!-- api: M:Inno.Rendering.RenderGraphBuilder.CreateBuffer(System.String,Inno.Rendering.RenderBufferDescriptor) a4ad62de43346a3f255aa616668fc3be34207fa04cf7e9899cb100161845b97e -->
<!-- api: M:Inno.Rendering.RenderGraphBuilder.ImportBuffer(System.String,Inno.Rendering.PersistentBufferHandle,Inno.Rendering.RenderBufferDescriptor) a5cd33f3a86d46fc79288f4f58d14e1a526c72fcaacdf81a55921ab4aafba695 -->
<!-- api: M:Inno.Rendering.RenderGraphBuilder.MarkOutput(Inno.Rendering.RenderTextureHandle) 820c40d9f6d320b1d2dfa7ae27c68a0dd2bc9d9ebbce92c19bf8bc5e8c239cc9 -->
<!-- api: M:Inno.Rendering.RenderGraphBuilder.MarkOutput(Inno.Rendering.RenderBufferHandle) 96028d451f8c0a4b7a52fb3e225e26aa4509b3d2e2e5421eaa1fd388b2362edc -->
<!-- api: M:Inno.Rendering.RenderGraphBuilder.BeginMutationScope c96467ff29a2c371fa8a294ba222ecec9630933e5836716ee98c648bd5def88e -->
<!-- api: M:Inno.Rendering.RenderGraphBuilder.BeginNameScope(System.String) bc9f54dfa0f49201b214b4be8caf6d1c9204fc7d95bca679178b3cdcbc376f71 -->
<!-- api: M:Inno.Rendering.RenderGraphBuilder.AddRasterPass``1(System.String,Inno.Rendering.RenderPhaseId,``0,Inno.Rendering.RenderPassExecute{``0}) 5fd22616af8327f179cd4bbd510a0c8743ce9d4612e966e5f5a1abe88e0c7655 -->
<!-- api: M:Inno.Rendering.RenderGraphBuilder.AddComputePass``1(System.String,Inno.Rendering.RenderPhaseId,``0,Inno.Rendering.RenderPassExecute{``0}) 4913a33cf6bcd63cf2240f4da9cc9eb39325808df292b55b79b5422e7dcd43b2 -->
<!-- api: M:Inno.Rendering.RenderGraphBuilder.AddCopyPass``1(System.String,Inno.Rendering.RenderPhaseId,``0,Inno.Rendering.RenderPassExecute{``0}) 2404f08c93a6a3bd9862525a7429e5396f5cf16b2bed1e8b9f05f17062288d5f -->
<!-- api: M:Inno.Rendering.RenderGraphBuilder.Compile c7cea4b28dbd30839730195d91d5e3ded298bcd20eaf61d2f7fd3191081c929d -->
<!-- api: M:Inno.Rendering.RenderGraphBuilder.Validate 2c4488dbb3f8c0f189206e947fc5f1d5ad9253da6239e80693234f93111c280d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f344cb66049760de"></a>[`public Inno.Rendering.RenderGraphBuilder.RenderGraphBuilder(uint generation, Inno.Rendering.GraphicsCapabilities capabilities)`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphBuilder.cs#L39) | Creates a render graph builder for one frame generation.<br>`generation`：Non-zero frame-scoped generation.<br>`capabilities`：Device capabilities used during validation. | — | — |
| <a id="api-aa52ac1517d7d242"></a>[`public Inno.Rendering.ComputePassBuilder Inno.Rendering.RenderGraphBuilder.AddComputePass<TPassData>(string name, Inno.Rendering.RenderPhaseId phase, TPassData passData, Inno.Rendering.RenderPassExecute<TPassData> execute) where TPassData : notnull`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphBuilder.cs#L277) | Adds a frame-scoped compute pass.<br>返回：A builder for compute resources and ordering.<br>`name`：Unique diagnostic name.<br>`phase`：Open render phase identifier.<br>`passData`：Payload that lives only until this graph executes.<br>`execute`：Command recording callback. | — | — |
| <a id="api-722a2d9049bb3e81"></a>[`public Inno.Rendering.CopyPassBuilder Inno.Rendering.RenderGraphBuilder.AddCopyPass<TPassData>(string name, Inno.Rendering.RenderPhaseId phase, TPassData passData, Inno.Rendering.RenderPassExecute<TPassData> execute) where TPassData : notnull`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphBuilder.cs#L307) | Adds a frame-scoped resource copy pass.<br>返回：A builder for copy resources and ordering.<br>`name`：Unique diagnostic name.<br>`phase`：Open render phase identifier.<br>`passData`：Payload that lives only until this graph executes.<br>`execute`：Command recording callback. | — | — |
| <a id="api-c2232b642ae43ab9"></a>[`public Inno.Rendering.RasterPassBuilder Inno.Rendering.RenderGraphBuilder.AddRasterPass<TPassData>(string name, Inno.Rendering.RenderPhaseId phase, TPassData passData, Inno.Rendering.RenderPassExecute<TPassData> execute) where TPassData : notnull`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphBuilder.cs#L247) | Adds a frame-scoped raster pass.<br>返回：A builder for raster resources, attachments and ordering.<br>`name`：Unique diagnostic name.<br>`phase`：Open render phase identifier.<br>`passData`：Payload that lives only until this graph executes.<br>`execute`：Command recording callback. | — | — |
| <a id="api-d6fdcc4749e86b5b"></a>[`public Inno.Rendering.RenderGraphMutationScope Inno.Rendering.RenderGraphBuilder.BeginMutationScope()`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphBuilder.cs#L197) | Begins an isolated graph mutation that rolls back every added pass and resource unless committed.<br>返回：A mutation scope used by reloadable features and other failure-isolated producers. | — | — |
| <a id="api-67433ec2a8e54886"></a>[`public Inno.Rendering.RenderGraphNameScope Inno.Rendering.RenderGraphBuilder.BeginNameScope(string name)`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphBuilder.cs#L217) | Prefixes pass and resource diagnostic names until the returned scope is disposed.<br>返回：A disposable name scope that must be closed in nesting order.<br>`name`：Non-empty scope segment. | — | — |
| <a id="api-442a048ce188ef9f"></a>[`public Inno.Rendering.RenderGraphCompileResult Inno.Rendering.RenderGraphBuilder.Compile()`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphBuilder.cs#L323) | Validates, culls and schedules this graph exactly once.<br>返回：A compilation result containing either an executable graph or diagnostics. | — | — |
| <a id="api-8e7473b61a1195f4"></a>[`public Inno.Rendering.RenderBufferHandle Inno.Rendering.RenderGraphBuilder.CreateBuffer(string name, Inno.Rendering.RenderBufferDescriptor descriptor)`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphBuilder.cs#L122) | Creates a transient buffer eligible for lifetime aliasing.<br>返回：A handle valid only for this graph generation.<br>`name`：Debug and diagnostic name.<br>`descriptor`：Buffer requirements. | — | — |
| <a id="api-bdb163637d0c1c50"></a>[`public Inno.Rendering.RenderTextureHandle Inno.Rendering.RenderGraphBuilder.CreateTexture(string name, Inno.Rendering.RenderTextureDescriptor descriptor)`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphBuilder.cs#L65) | Creates a transient texture eligible for lifetime aliasing.<br>返回：A handle valid only for this graph generation.<br>`name`：Debug and diagnostic name.<br>`descriptor`：Texture requirements. | — | — |
| <a id="api-cac9e54107aaf47b"></a>[`public Inno.Rendering.RenderBufferHandle Inno.Rendering.RenderGraphBuilder.ImportBuffer(string name, Inno.Rendering.PersistentBufferHandle buffer, Inno.Rendering.RenderBufferDescriptor descriptor)`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphBuilder.cs#L149) | Imports a persistent device buffer into the current graph.<br>返回：A frame-scoped handle for graph declarations.<br>`name`：Debug and diagnostic name.<br>`buffer`：Persistent buffer owned by the active device generation.<br>`descriptor`：Buffer requirements and usage. | — | — |
| <a id="api-61419d6c51414f46"></a>[`public Inno.Rendering.RenderTextureHandle Inno.Rendering.RenderGraphBuilder.ImportTexture(string name, Inno.Rendering.PersistentTextureHandle texture, Inno.Rendering.RenderTextureDescriptor descriptor)`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphBuilder.cs#L92) | Imports a persistent device texture into the current graph.<br>返回：A frame-scoped handle for graph declarations.<br>`name`：Debug and diagnostic name.<br>`texture`：Persistent texture owned by the active device generation.<br>`descriptor`：Texture requirements and usage. | — | — |
| <a id="api-fef051de37d58143"></a>[`public void Inno.Rendering.RenderGraphBuilder.MarkOutput(Inno.Rendering.RenderBufferHandle buffer)`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphBuilder.cs#L185) | Marks a buffer as a graph output that keeps its producers alive.<br>`buffer`：Output buffer. | — | — |
| <a id="api-e0a37552a6d44069"></a>[`public void Inno.Rendering.RenderGraphBuilder.MarkOutput(Inno.Rendering.RenderTextureHandle texture)`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphBuilder.cs#L173) | Marks a texture as a graph output that keeps its producers alive.<br>`texture`：Output texture. | — | — |
| <a id="api-5f267f393ea513fa"></a>[`public Inno.Rendering.RenderGraphValidationResult Inno.Rendering.RenderGraphBuilder.Validate()`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphBuilder.cs#L340) | Validates current declarations and caches their analysis without constructing an executable graph.<br>返回：A frozen result for the current revision; subsequent mutations invalidate the cached analysis. | — | — |
| <a id="api-1a322b3600bde741"></a>[`public sealed class Inno.Rendering.RenderGraphBuilder`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphBuilder.cs#L11) | Builds one generation-scoped render graph from explicit passes and resources. | — | — |

### `Inno.Rendering.RenderGraphCompileResult`

<!-- api: T:Inno.Rendering.RenderGraphCompileResult b279c12d9c39c8614dc84ea9eb69fedf5fc0f0036faf8e54ffba4368789e5b46 -->
<!-- api: P:Inno.Rendering.RenderGraphCompileResult.graph ab3b38fba2ad30a18f0d678b918455ee4c4bceda66c19fdac7b303617111b7ee -->
<!-- api: P:Inno.Rendering.RenderGraphCompileResult.diagnostics 4643f6753fd07d23786cdaffb97902e1f6b21efa9d5a8db3593b06280d535b1c -->
<!-- api: P:Inno.Rendering.RenderGraphCompileResult.culledPassCount 748bdee3e3f8246bc7b3938a6e82f3a183cfd38ce93e1d2e7b8c27ca62bcdd51 -->
<!-- api: P:Inno.Rendering.RenderGraphCompileResult.succeeded c6b497e31db06d04c62b4758a6d50e6515408fc4c6642f73fb99a7dae5b1c261 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1d178572bf7a2b9f"></a>[`public int Inno.Rendering.RenderGraphCompileResult.culledPassCount { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L541) | Gets passes removed because they did not contribute to an output or side effect. | — | — |
| <a id="api-b6dff6617ff114b4"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.RenderGraphDiagnostic> Inno.Rendering.RenderGraphCompileResult.diagnostics { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L536) | Gets deterministic compilation diagnostics. | — | — |
| <a id="api-0ddc380e7a2a19d7"></a>[`public Inno.Rendering.CompiledRenderGraph? Inno.Rendering.RenderGraphCompileResult.graph { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L531) | Gets the executable graph, or null when compilation failed. | — | — |
| <a id="api-bc3d48d4047a9578"></a>[`public bool Inno.Rendering.RenderGraphCompileResult.succeeded { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L546) | Gets whether an executable graph was produced. | — | — |
| <a id="api-a1d1e2a1eb7097f4"></a>[`public sealed class Inno.Rendering.RenderGraphCompileResult`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L513) | Contains render-graph compilation diagnostics and an optional executable graph. | — | — |

### `Inno.Rendering.RenderGraphDiagnostic`

<!-- api: T:Inno.Rendering.RenderGraphDiagnostic 4f0f14bc35ef78928f487fb6930369df0097138a96da501764faf7e41aa69786 -->
<!-- api: M:Inno.Rendering.RenderGraphDiagnostic.#ctor(System.String,System.String,Inno.Core.Diagnostics.DiagnosticSeverity,System.String,System.String) d80cde0db6d3f7c679745b8753e20950e9fc0cb1225032617a0c8dd5eff70062 -->
<!-- api: P:Inno.Rendering.RenderGraphDiagnostic.code 058fdabe3c5fb086d33d3fe25c706ded7741dd8950e1e06b16326aa8bffaba6e -->
<!-- api: P:Inno.Rendering.RenderGraphDiagnostic.message 1f0cf498f9fb235d30c0ae8bcfe8c256b26f155f318be08cce93fb2b4fb0eaaf -->
<!-- api: P:Inno.Rendering.RenderGraphDiagnostic.severity b5837e2642421d18bbac7547648265ddb3ab06ea704462fe950f24811de7d32b -->
<!-- api: P:Inno.Rendering.RenderGraphDiagnostic.passName c948428c0453a9931b65f135ab36bf7f4630a7e184db5f986b120c5fa1d12d18 -->
<!-- api: P:Inno.Rendering.RenderGraphDiagnostic.resourceName b642db182bb9eaeb4fd2a66f8ded411911f91702f118820fce5d29ee4dba8adf -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d4a24eaf65ecdd8a"></a>[`public Inno.Rendering.RenderGraphDiagnostic.RenderGraphDiagnostic(string code, string message, Inno.Core.Diagnostics.DiagnosticSeverity severity, string? passName = null, string? resourceName = null)`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L182) | Creates a render-graph diagnostic.<br>`code`：Stable machine-readable code.<br>`message`：Actionable diagnostic text.<br>`severity`：Diagnostic impact.<br>`passName`：Optional related pass name.<br>`resourceName`：Optional related resource name. | — | — |
| <a id="api-560fa2defa4316a9"></a>[`public string Inno.Rendering.RenderGraphDiagnostic.code { get; }`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L201) | Gets the stable machine-readable code. | — | — |
| <a id="api-18b30b811ef8cf21"></a>[`public string Inno.Rendering.RenderGraphDiagnostic.message { get; }`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L206) | Gets actionable diagnostic text. | — | — |
| <a id="api-0498c538c29045bb"></a>[`public string? Inno.Rendering.RenderGraphDiagnostic.passName { get; }`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L216) | Gets the related pass name, if any. | — | — |
| <a id="api-4483693d27cdd57b"></a>[`public string? Inno.Rendering.RenderGraphDiagnostic.resourceName { get; }`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L221) | Gets the related resource name, if any. | — | — |
| <a id="api-e2f11fc4d09523c0"></a>[`public Inno.Core.Diagnostics.DiagnosticSeverity Inno.Rendering.RenderGraphDiagnostic.severity { get; }`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L211) | Gets diagnostic impact. | — | — |
| <a id="api-f33ae143e9fbff39"></a>[`public sealed class Inno.Rendering.RenderGraphDiagnostic`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L162) | Reports one structured render-graph compilation problem. | — | — |

### `Inno.Rendering.RenderGraphMutationScope`

<!-- api: T:Inno.Rendering.RenderGraphMutationScope ea8e39c143f22a01e667509524f668c112a7287108d535dead965dfc04a663ad -->
<!-- api: M:Inno.Rendering.RenderGraphMutationScope.Commit 87214c861367671b42684b6ed1afbc53b3be14402ed7709d0c21f38bce8692d8 -->
<!-- api: M:Inno.Rendering.RenderGraphMutationScope.Dispose cf138fc8c4ef126a13ea385d5ba2b7afdf85e9afc80b78c2720671e29b11410c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4a64fd5e8d117ed5"></a>[`public void Inno.Rendering.RenderGraphMutationScope.Commit()`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphMutationScope.cs#L40) | Validates and freezes additions in the current scope. Dispose must still end the scope before new work. | — | `System.InvalidOperationException`：The scope is not current, was already committed, or the graph additions are invalid.<br>`System.ObjectDisposedException`：This scope has already ended. |
| <a id="api-c89c7570a25bedfa"></a>[`public void Inno.Rendering.RenderGraphMutationScope.Dispose()`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphMutationScope.cs#L52) | Ends the current scope and removes all uncommitted additions, including output and name declarations. | — | `System.InvalidOperationException`：A nested mutation scope must be ended first. |
| <a id="api-13d20e7fc6aec15b"></a>[`public sealed class Inno.Rendering.RenderGraphMutationScope : System.IDisposable`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphMutationScope.cs#L9) | Owns isolated graph additions; earlier pass declarations are frozen and failure removes every addition. | — | — |

### `Inno.Rendering.RenderGraphNameScope`

<!-- api: T:Inno.Rendering.RenderGraphNameScope 03159286c166a45683a8cc852d42d7be6d1c49a17b34d753e85778505e613bd2 -->
<!-- api: M:Inno.Rendering.RenderGraphNameScope.Dispose 56562f34c1967de9ca8f7ec71000cc127cca8e06f9a9cf203677b8313b81ce98 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f70d3538816a662f"></a>[`public void Inno.Rendering.RenderGraphNameScope.Dispose()`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphNameScope.cs#L25) | Ends the name prefix scope. | — | — |
| <a id="api-2cdd2fc0cd385ad2"></a>[`public sealed class Inno.Rendering.RenderGraphNameScope : System.IDisposable`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphNameScope.cs#L9) | Owns one nested diagnostic-name prefix in a render graph builder. | — | — |

### `Inno.Rendering.RenderGraphValidationResult`

<!-- api: T:Inno.Rendering.RenderGraphValidationResult b52703c92ca391b5686edbac607d9d5692415a2df587fe398204bb19b6349583 -->
<!-- api: P:Inno.Rendering.RenderGraphValidationResult.isValid 06ce8543550307b4d860954731e5e2e16f4e065066dbc816978f457829d4ab5e -->
<!-- api: P:Inno.Rendering.RenderGraphValidationResult.diagnostics 3666d5f03fee068ff25a004430158839ce863879deb8d8c530238000b3b46748 -->
<!-- api: P:Inno.Rendering.RenderGraphValidationResult.passCount 6e01ebd74517e5d9979074924209c875b7d7457b6e633796d8d7fd153c3d0742 -->
<!-- api: P:Inno.Rendering.RenderGraphValidationResult.scheduledPassCount fb16101e565dedc05673ed3c79677221491f965ee9455aa99f47999bb66b7b98 -->
<!-- api: P:Inno.Rendering.RenderGraphValidationResult.culledPassCount 654ff3c0ee9019bdf8ff3ca3d78e84105f3c9f6efc960ca5a4b3e191ff4c1354 -->
<!-- api: P:Inno.Rendering.RenderGraphValidationResult.textureCount e0cc081254fe0174a918ffddc84a7405dd57cb5a7eddd75ce65c4d218553f8e2 -->
<!-- api: P:Inno.Rendering.RenderGraphValidationResult.bufferCount 77abf4b4e088000e81393bce0c6fcbb1cc58995dc80582b95b488a4cc5292276 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d1789ae0f3ceee25"></a>[`public int Inno.Rendering.RenderGraphValidationResult.bufferCount { get; }`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphValidationResult.cs#L61) | Gets the number of declared transient and imported buffers. | — | — |
| <a id="api-cde0979d238f4c6e"></a>[`public int Inno.Rendering.RenderGraphValidationResult.culledPassCount { get; }`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphValidationResult.cs#L51) | Gets passes removed by valid output and side-effect analysis; returns zero for an invalid graph. | — | — |
| <a id="api-0f5467756d65e2d3"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.RenderGraphDiagnostic> Inno.Rendering.RenderGraphValidationResult.diagnostics { get; }`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphValidationResult.cs#L36) | Gets frozen deterministic diagnostics for the validated revision. | — | — |
| <a id="api-89d7a597ec032199"></a>[`public bool Inno.Rendering.RenderGraphValidationResult.isValid { get; }`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphValidationResult.cs#L31) | Gets whether the current graph satisfies device, dependency and presentation limits. | — | — |
| <a id="api-01722367c559ed5d"></a>[`public int Inno.Rendering.RenderGraphValidationResult.passCount { get; }`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphValidationResult.cs#L41) | Gets the number of declared passes, including passes not contributing to an output. | — | — |
| <a id="api-653f073fdc40435e"></a>[`public int Inno.Rendering.RenderGraphValidationResult.scheduledPassCount { get; }`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphValidationResult.cs#L46) | Gets scheduled passes, or zero when validation could not produce a valid schedule. | — | — |
| <a id="api-6ff00d918ecf8005"></a>[`public int Inno.Rendering.RenderGraphValidationResult.textureCount { get; }`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphValidationResult.cs#L56) | Gets the number of declared transient and imported textures. | — | — |
| <a id="api-382e7acb537faea1"></a>[`public sealed class Inno.Rendering.RenderGraphValidationResult`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphValidationResult.cs#L10) | Describes graph validity and scheduling counts without creating resources or executable callbacks. | — | — |

### `Inno.Rendering.RenderIndexFormat`

<!-- api: T:Inno.Rendering.RenderIndexFormat 68ed5879dab932b4b734fedaf41138ef40817446d952ed611288e638c93cfcdb -->
<!-- api: F:Inno.Rendering.RenderIndexFormat.UInt16 3775afb1f1750b1f5d5cb1958057c97dd209f14d378edfdda21b5466c2cb315d -->
<!-- api: F:Inno.Rendering.RenderIndexFormat.UInt32 094c41b4fa231b7a786b99c62a61e1f3960c408e9d6ce0627f818fcadc7adad8 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d9f7fb0b6e0469e8"></a>[`Inno.Rendering.RenderIndexFormat.UInt16 = 0`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L345) | Unsigned 16-bit indices. | — | — |
| <a id="api-3280216e782b3dac"></a>[`Inno.Rendering.RenderIndexFormat.UInt32 = 1`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L349) | Unsigned 32-bit indices. | — | — |
| <a id="api-25f1e15ff50655d7"></a>[`public enum Inno.Rendering.RenderIndexFormat : int`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L340) | Selects the integer representation of an index buffer. | — | — |

### `Inno.Rendering.RenderLoadAction`

<!-- api: T:Inno.Rendering.RenderLoadAction 3e91f9e0d441743a444e9f58f16272a7e7ddd1639e53fe2483ddf26552677ec0 -->
<!-- api: F:Inno.Rendering.RenderLoadAction.Load e5ecdc950d43cb9947cf3cdebbc5959889006ba25b745aa9d417d0ab53553e96 -->
<!-- api: F:Inno.Rendering.RenderLoadAction.Clear 742fb95fca71036fefdb981878e27b3cd361b1d5357658f48747d710b88f8013 -->
<!-- api: F:Inno.Rendering.RenderLoadAction.Discard daeafa6d86b50c23c9637d24b37569f3cae2e9d0c998d78eaf72ea499cc91fda -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-fb8761c66cc876eb"></a>[`Inno.Rendering.RenderLoadAction.Clear = 1`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L84) | Clears the attachment before rendering. | — | — |
| <a id="api-aa5539ec12f74982"></a>[`Inno.Rendering.RenderLoadAction.Discard = 2`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L88) | Does not require prior attachment contents. | — | — |
| <a id="api-7b5e126b986fbf0e"></a>[`Inno.Rendering.RenderLoadAction.Load = 0`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L80) | Preserves prior attachment contents. | — | — |
| <a id="api-8bf224b7d371be04"></a>[`public enum Inno.Rendering.RenderLoadAction : int`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L75) | Controls how existing attachment contents enter a raster pass. | — | — |

### `Inno.Rendering.RenderPassBuilder`

<!-- api: T:Inno.Rendering.RenderPassBuilder 968d296ee2e6e10c39d65982bcc8b1ae327571aae615e4d970f23700982ec07b -->
<!-- api: M:Inno.Rendering.RenderPassBuilder.Before(Inno.Rendering.RenderPhaseId) 0069a684b533201c7af8d7a5628eff16cb66e2083aa515b38cb4745ce99bdb0a -->
<!-- api: M:Inno.Rendering.RenderPassBuilder.After(Inno.Rendering.RenderPhaseId) b4e9d21d12fb74d8ee05e11d043250cd85826f721743c3e2a3c71e6e56885f14 -->
<!-- api: M:Inno.Rendering.RenderPassBuilder.HasSideEffect eec70c952b3c755895f1e9266635733786cf0ef122494704e764c65fa0b206bc -->
<!-- api: M:Inno.Rendering.RenderPassBuilder.AllowParallelRecording eb410a4b23b41c637eff8cd23558d450fbdf21ffa7d53d2dfff870fc8f8b4376 -->
<!-- api: M:Inno.Rendering.RenderPassBuilder.ReadTexture(Inno.Rendering.RenderTextureHandle) 185763755ef47f446c4e10051357c7c6baa3db606fd179885dcd09c26b64eebe -->
<!-- api: M:Inno.Rendering.RenderPassBuilder.ReadBuffer(Inno.Rendering.RenderBufferHandle) ed45cee4973b0e58bc960ba845da94e1774fd0c5e58a1bc954bfaf3353b7ab98 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-95c45f6909523613"></a>[`public Inno.Rendering.RenderPassBuilder Inno.Rendering.RenderPassBuilder.After(Inno.Rendering.RenderPhaseId phase)`](../../src/services/rendering/Inno.Rendering/Graph/RenderPassBuilder.cs#L50) | Orders this pass after every pass in a target phase.<br>返回：This builder for fluent declarations.<br>`phase`：Target phase. | — | — |
| <a id="api-f249a6346e696b22"></a>[`public Inno.Rendering.RenderPassBuilder Inno.Rendering.RenderPassBuilder.AllowParallelRecording()`](../../src/services/rendering/Inno.Rendering/Graph/RenderPassBuilder.cs#L80) | Opts this pass into worker-thread recording through an isolated backend-neutral command list.<br>返回：This builder for fluent declarations. | Pass data and code reached by the callback must be safe for worker-thread access. GPU execution and backend view order remain identical to the compiled graph order. | — |
| <a id="api-1334f991a22a126f"></a>[`public Inno.Rendering.RenderPassBuilder Inno.Rendering.RenderPassBuilder.Before(Inno.Rendering.RenderPhaseId phase)`](../../src/services/rendering/Inno.Rendering/Graph/RenderPassBuilder.cs#L34) | Orders this pass before every pass in a target phase.<br>返回：This builder for fluent declarations.<br>`phase`：Target phase. | — | — |
| <a id="api-761f6fff6d280d0f"></a>[`public Inno.Rendering.RenderPassBuilder Inno.Rendering.RenderPassBuilder.HasSideEffect()`](../../src/services/rendering/Inno.Rendering/Graph/RenderPassBuilder.cs#L63) | Prevents pass culling because execution has an externally observable effect.<br>返回：This builder for fluent declarations. | — | — |
| <a id="api-d5aaed5876ec4d23"></a>[`public Inno.Rendering.RenderPassBuilder Inno.Rendering.RenderPassBuilder.ReadBuffer(Inno.Rendering.RenderBufferHandle buffer)`](../../src/services/rendering/Inno.Rendering/Graph/RenderPassBuilder.cs#L115) | Declares a shader read from a buffer.<br>返回：This builder for fluent declarations.<br>`buffer`：Buffer read by this pass. | — | — |
| <a id="api-577d69873b4fa1af"></a>[`public Inno.Rendering.RenderPassBuilder Inno.Rendering.RenderPassBuilder.ReadTexture(Inno.Rendering.RenderTextureHandle texture)`](../../src/services/rendering/Inno.Rendering/Graph/RenderPassBuilder.cs#L96) | Declares a shader read from a texture.<br>返回：This builder for fluent declarations.<br>`texture`：Texture read by this pass. | — | — |
| <a id="api-d578ddd0a91768c8"></a>[`public abstract class Inno.Rendering.RenderPassBuilder`](../../src/services/rendering/Inno.Rendering/Graph/RenderPassBuilder.cs#L9) | Provides ordering and common resource declarations for one pass. | — | — |

### `Inno.Rendering.RenderPassContext`

<!-- api: T:Inno.Rendering.RenderPassContext 0f4c8b379958452c9eadabe149cf322c143ea3ec0f3544438418ac6933dce038 -->
<!-- api: P:Inno.Rendering.RenderPassContext.commands 32afbeb1c52b4c2cae98f4aa172dcf96bd5f6fd10995e0b03bb811b58c62ae5f -->
<!-- api: P:Inno.Rendering.RenderPassContext.frameIndex e4e713b2147c99b183cbcbf789bc0abf0f25266d6c37910f728deb0ba8f1e3fc -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-738a75fac730ac69"></a>[`public Inno.Rendering.RenderCommandEncoder Inno.Rendering.RenderPassContext.commands { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L782) | Gets the command encoder scoped to the current pass. | — | — |
| <a id="api-41099b0fe4fca14a"></a>[`public ulong Inno.Rendering.RenderPassContext.frameIndex { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L787) | Gets the monotonic frame index. | — | — |
| <a id="api-9bb1190b5269d394"></a>[`public sealed class Inno.Rendering.RenderPassContext`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L769) | Supplies the command encoder and immutable frame identity to a pass callback. | — | — |

### `Inno.Rendering.RenderPassExecute`1`

<!-- api: T:Inno.Rendering.RenderPassExecute`1 85d075ba2cc8011c6432795b434cf657d699e73565b0b8f9eb65663c427231c1 -->
<!-- api: M:Inno.Rendering.RenderPassExecute`1.#ctor(System.Object,System.IntPtr) a92060b26e3c191a4b1f279dc07aabbebdb4c5cbf646fc63393cf043bf6581ca -->
<!-- api: M:Inno.Rendering.RenderPassExecute`1.Invoke(`0,Inno.Rendering.RenderPassContext) 2b2616b662d80d5dea9130b1d6eb6e2f2be69499be6441457ba93e2524c91529 -->
<!-- api: M:Inno.Rendering.RenderPassExecute`1.BeginInvoke(`0,Inno.Rendering.RenderPassContext,System.AsyncCallback,System.Object) 2236e15f4087b9f080751339e594b1ddffb4106eaed2a308f436803f20590b6d -->
<!-- api: M:Inno.Rendering.RenderPassExecute`1.EndInvoke(System.IAsyncResult) d8d37f7ef25024c6b32673915e847d070c5bb7f5990b37f50a8c5a18921d2179 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a1488e71533e2694"></a>[`public Inno.Rendering.RenderPassExecute<TPassData>.RenderPassExecute(object @object, nint method)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L802) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-8cd0df2dd6182c9c"></a>[`public virtual System.IAsyncResult Inno.Rendering.RenderPassExecute<TPassData>.BeginInvoke(TPassData passData, Inno.Rendering.RenderPassContext context, System.AsyncCallback callback, object @object)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L802) | — | — | — |
| <a id="api-305cc041aaadd862"></a>[`public virtual void Inno.Rendering.RenderPassExecute<TPassData>.EndInvoke(System.IAsyncResult result)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L802) | — | — | — |
| <a id="api-3e74abbd2882c734"></a>[`public virtual void Inno.Rendering.RenderPassExecute<TPassData>.Invoke(TPassData passData, Inno.Rendering.RenderPassContext context)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L802) | Executes one frame-scoped pass payload.<br>`passData`：Pass payload captured for this frame only.<br>`context`：Current pass execution context. | — | — |
| <a id="api-78ccd21f916e12c0"></a>[`public delegate void Inno.Rendering.RenderPassExecute<TPassData>(TPassData passData, Inno.Rendering.RenderPassContext context)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L802) | Executes one frame-scoped pass payload.<br>`passData`：Pass payload captured for this frame only.<br>`context`：Current pass execution context. | — | — |

### `Inno.Rendering.RenderPassKind`

<!-- api: T:Inno.Rendering.RenderPassKind 627f010a9a6f062e606d02f2918d5be039a9121594ab9d9dc6b4f4695da61936 -->
<!-- api: F:Inno.Rendering.RenderPassKind.Raster 0aa528fd09e57e85178e9e945fb0ecd062ea23c8eb078341dc23c8824cc65003 -->
<!-- api: F:Inno.Rendering.RenderPassKind.Compute ca73921dcae88b55c76162c5fb813f5a8d22d0b0fd4f3e2a304b7e5fd228a14e -->
<!-- api: F:Inno.Rendering.RenderPassKind.Copy 64f50e87b6d0e7df54b065ac23815504685797608eb7320353044302975ba1fb -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d321015fd36de0d8"></a>[`Inno.Rendering.RenderPassKind.Compute = 1`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L50) | Compute dispatch commands. | — | — |
| <a id="api-f5f08cb57d9d36d7"></a>[`Inno.Rendering.RenderPassKind.Copy = 2`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L54) | Resource copy commands. | — | — |
| <a id="api-0fba7060cb455c9a"></a>[`Inno.Rendering.RenderPassKind.Raster = 0`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L46) | Raster draw commands with optional attachments. | — | — |
| <a id="api-1177b6dd70c2d4ed"></a>[`public enum Inno.Rendering.RenderPassKind : int`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L41) | Distinguishes pass command domains for validation and backend execution. | — | — |

### `Inno.Rendering.RenderPassRecordingMode`

<!-- api: T:Inno.Rendering.RenderPassRecordingMode bd2b72938844a5aec56d957f4e29deb4e001a1197118e61ef39165a53f439ef1 -->
<!-- api: F:Inno.Rendering.RenderPassRecordingMode.Serial 4166fca48d3db61be4983a204ac780eb3f0bafaf3c0cc7e3c2bc5554cf7118dd -->
<!-- api: F:Inno.Rendering.RenderPassRecordingMode.Parallel 0d2324116e21ed8e65fc9addbe10dc0f696f642d95af49fd6682859c31123816 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-80e2935c8d522bce"></a>[`Inno.Rendering.RenderPassRecordingMode.Parallel = 1`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L69) | Records into an isolated command list on a worker thread, then replays in graph order. | — | — |
| <a id="api-a4625f8944854d5b"></a>[`Inno.Rendering.RenderPassRecordingMode.Serial = 0`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L65) | Records directly on the graph execution thread. | — | — |
| <a id="api-43617b04b122f7f8"></a>[`public enum Inno.Rendering.RenderPassRecordingMode : int`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L60) | Controls where a frame-local pass callback records backend-neutral commands. | — | — |

### `Inno.Rendering.RenderPhaseId`

<!-- api: T:Inno.Rendering.RenderPhaseId 06f81349a65dc44aaf93d02ccb60e50f0082924d874bfec532a71e8ea13d9351 -->
<!-- api: M:Inno.Rendering.RenderPhaseId.#ctor(System.String) 03c1158acae5a95eabfc15120a215e9789a5160022db98e306ddb4d882304431 -->
<!-- api: P:Inno.Rendering.RenderPhaseId.value f70efb42dd58d5bb2cab9c9a522a103583a6c2a80b6e61a23190f4c95091080a -->
<!-- api: M:Inno.Rendering.RenderPhaseId.ToString 14a62fa29c003c7871f104b4d4e12302bcc475dbc85ced64a5cf48dc1a3d37e3 -->
<!-- api: M:Inno.Rendering.RenderPhaseId.op_Inequality(Inno.Rendering.RenderPhaseId,Inno.Rendering.RenderPhaseId) ce9aecfadc41427b830c73b7be0b4313637635bb6a4f6b376d4ce8db0121b138 -->
<!-- api: M:Inno.Rendering.RenderPhaseId.op_Equality(Inno.Rendering.RenderPhaseId,Inno.Rendering.RenderPhaseId) ce53cc925d74c14c0eecb76495a9a0bbae641f70a47582d4b637a471fa0e8873 -->
<!-- api: M:Inno.Rendering.RenderPhaseId.GetHashCode ae962da08fdeb3d32499e8ddcda1b3dd0e485fdb79a960324bba061ed8dabd16 -->
<!-- api: M:Inno.Rendering.RenderPhaseId.Equals(System.Object) ee025187f7278a14b56f9c14d68cec4d45429cfc752f410d489f814029301c33 -->
<!-- api: M:Inno.Rendering.RenderPhaseId.Equals(Inno.Rendering.RenderPhaseId) e5bc761bf4192f82074e49c8bd8886895c554e2519ad6c2f2a8cd1c1a9bce24a -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-ed076e2b02f99561"></a>[`public Inno.Rendering.RenderPhaseId.RenderPhaseId(string value)`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L18) | Creates an open render phase identifier.<br>`value`：Globally stable phase value. | — | — |
| <a id="api-055f0b7b70c77474"></a>[`public bool Inno.Rendering.RenderPhaseId.Equals(Inno.Rendering.RenderPhaseId other)`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L10) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-09309c6614d1451a"></a>[`public override bool Inno.Rendering.RenderPhaseId.Equals(object obj)`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L10) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-6f91e1e80cde3e1d"></a>[`public override int Inno.Rendering.RenderPhaseId.GetHashCode()`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L10) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-e41e92645822a8fa"></a>[`public static bool Inno.Rendering.RenderPhaseId.operator ==(Inno.Rendering.RenderPhaseId left, Inno.Rendering.RenderPhaseId right)`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L10) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-2e16eb0f6ce6a136"></a>[`public static bool Inno.Rendering.RenderPhaseId.operator !=(Inno.Rendering.RenderPhaseId left, Inno.Rendering.RenderPhaseId right)`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L10) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-f1732638605ac561"></a>[`public override string Inno.Rendering.RenderPhaseId.ToString()`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L35) | Formats this value as a human-readable representation.<br>返回：The human-readable representation of this value. | — | — |
| <a id="api-e57b06c9ed50c60b"></a>[`public string Inno.Rendering.RenderPhaseId.value { get; }`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L27) | Gets the globally stable phase value. | — | — |
| <a id="api-940ed1ec362535e3"></a>[`public readonly record struct Inno.Rendering.RenderPhaseId : System.IEquatable<Inno.Rendering.RenderPhaseId>`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L10) | Identifies an open render phase protocol value. | — | — |

### `Inno.Rendering.RenderPresentationSize`

<!-- api: T:Inno.Rendering.RenderPresentationSize 043a891565ca98f92cddbcb476c923417f0af2b08fb4e27ec313a8b00fefbead -->
<!-- api: M:Inno.Rendering.RenderPresentationSize.#ctor(System.Int32,System.Int32) 85bad9159533da5998a609f42447b824d31ba72c3bc5c2344d3570f08c7df837 -->
<!-- api: P:Inno.Rendering.RenderPresentationSize.width 5f311a9984437f0c1aac6eb2f8e34b1e99bb68a4eb177038f03dbc2854b46f44 -->
<!-- api: P:Inno.Rendering.RenderPresentationSize.height cf012f1c2f930eef3ba86eaef950b5859837ba7f48d417b24b783a2a80963717 -->
<!-- api: P:Inno.Rendering.RenderPresentationSize.isValid a2b8d8fc7316b396eaae554b91020c24bedf781e32a757f8e900ebb3bdb56a95 -->
<!-- api: M:Inno.Rendering.RenderPresentationSize.ToString 5013b2b61a44c89397a42dc581983b50d744c0fa45a3b953247f16efc7384fd7 -->
<!-- api: M:Inno.Rendering.RenderPresentationSize.op_Inequality(Inno.Rendering.RenderPresentationSize,Inno.Rendering.RenderPresentationSize) 24cb1d9679a228620c795de9569a36a61b21f4c81ae52d1b7e5d236ae27c82e3 -->
<!-- api: M:Inno.Rendering.RenderPresentationSize.op_Equality(Inno.Rendering.RenderPresentationSize,Inno.Rendering.RenderPresentationSize) 3b4f513329775d2e8e8c94669d022cf7dd986755786f8878206dcc957bfb9cc1 -->
<!-- api: M:Inno.Rendering.RenderPresentationSize.GetHashCode c6391a4270606815896a3cf53e3ff82daa0f78f58c301434056f0e305c114796 -->
<!-- api: M:Inno.Rendering.RenderPresentationSize.Equals(System.Object) fd2c64353d5a99326c593ae72f2e61fea949708c0b794148efe500e549b18e48 -->
<!-- api: M:Inno.Rendering.RenderPresentationSize.Equals(Inno.Rendering.RenderPresentationSize) bf1ffb60125ced393ab92d48e12ed0c9de2a79fc71db3e716ec1f93b129ce059 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-423709b1ed845ef4"></a>[`public Inno.Rendering.RenderPresentationSize.RenderPresentationSize(int width, int height)`](../../src/services/rendering/Inno.Rendering/Device/RenderPresentationSize.cs#L19) | Creates a positive presentation extent.<br>`width`：Positive physical-pixel width.<br>`height`：Positive physical-pixel height. | — | — |
| <a id="api-dd088f66e192b4e0"></a>[`public bool Inno.Rendering.RenderPresentationSize.Equals(Inno.Rendering.RenderPresentationSize other)`](../../src/services/rendering/Inno.Rendering/Device/RenderPresentationSize.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-a028bc86090bc655"></a>[`public override bool Inno.Rendering.RenderPresentationSize.Equals(object obj)`](../../src/services/rendering/Inno.Rendering/Device/RenderPresentationSize.cs#L8) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-371ef4e3ff748c5d"></a>[`public override int Inno.Rendering.RenderPresentationSize.GetHashCode()`](../../src/services/rendering/Inno.Rendering/Device/RenderPresentationSize.cs#L8) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-299e12a9dd2a29a7"></a>[`public static bool Inno.Rendering.RenderPresentationSize.operator ==(Inno.Rendering.RenderPresentationSize left, Inno.Rendering.RenderPresentationSize right)`](../../src/services/rendering/Inno.Rendering/Device/RenderPresentationSize.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-94f3859abb988ffb"></a>[`public static bool Inno.Rendering.RenderPresentationSize.operator !=(Inno.Rendering.RenderPresentationSize left, Inno.Rendering.RenderPresentationSize right)`](../../src/services/rendering/Inno.Rendering/Device/RenderPresentationSize.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-ca91f84a76628463"></a>[`public override string Inno.Rendering.RenderPresentationSize.ToString()`](../../src/services/rendering/Inno.Rendering/Device/RenderPresentationSize.cs#L8) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-ab21da63f775acc7"></a>[`public int Inno.Rendering.RenderPresentationSize.height { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderPresentationSize.cs#L37) | Gets the physical-pixel height. | — | — |
| <a id="api-ab972d84b7d40105"></a>[`public bool Inno.Rendering.RenderPresentationSize.isValid { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderPresentationSize.cs#L42) | Gets whether both dimensions describe a real drawable extent; a default struct is invalid. | — | — |
| <a id="api-45b9776ee3ed8ce9"></a>[`public int Inno.Rendering.RenderPresentationSize.width { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderPresentationSize.cs#L32) | Gets the physical-pixel width. | — | — |
| <a id="api-7f3159dede05c259"></a>[`public readonly record struct Inno.Rendering.RenderPresentationSize : System.IEquatable<Inno.Rendering.RenderPresentationSize>`](../../src/services/rendering/Inno.Rendering/Device/RenderPresentationSize.cs#L8) | Stores one backend-neutral presentation extent in physical pixels. | — | — |

### `Inno.Rendering.RenderPrimitiveTopology`

<!-- api: T:Inno.Rendering.RenderPrimitiveTopology 5d181481099ee4c7ea2131163b532f039f20013d9544f20b4780c7ba446525f7 -->
<!-- api: F:Inno.Rendering.RenderPrimitiveTopology.TriangleList 0c269d7203f3a0ef403838127f498d27f44c142a4b119db366f1d421d7c77acd -->
<!-- api: F:Inno.Rendering.RenderPrimitiveTopology.TriangleStrip 12540698082c19f9f4c1573f5ea3a39cc242f630646843c4f6613e4283467f29 -->
<!-- api: F:Inno.Rendering.RenderPrimitiveTopology.LineList 2785ca28607f55ba514ae28281133076ec000af6dfd3d1dbcd8070e301a76c10 -->
<!-- api: F:Inno.Rendering.RenderPrimitiveTopology.LineStrip 49ae9b3ec1d08c73146b004b502e5b6cdd09460ad0465e7b2cd4676cd418a28d -->
<!-- api: F:Inno.Rendering.RenderPrimitiveTopology.PointList e0b054301b0b59297e784ad829f39dd70f3e9180970cca6d599b178f472f1754 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-823d749f524c80a0"></a>[`Inno.Rendering.RenderPrimitiveTopology.LineList = 2`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L368) | Independent line pairs. | — | — |
| <a id="api-9612a9ba2d43ed29"></a>[`Inno.Rendering.RenderPrimitiveTopology.LineStrip = 3`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L372) | Connected line strip. | — | — |
| <a id="api-a91afb53b34e2149"></a>[`Inno.Rendering.RenderPrimitiveTopology.PointList = 4`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L376) | Independent points. | — | — |
| <a id="api-324bcf64ef970622"></a>[`Inno.Rendering.RenderPrimitiveTopology.TriangleList = 0`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L360) | Independent triangle triplets. | — | — |
| <a id="api-51b06f4cc8017917"></a>[`Inno.Rendering.RenderPrimitiveTopology.TriangleStrip = 1`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L364) | Connected triangle strip. | — | — |
| <a id="api-b10e6e9d3f6af68e"></a>[`public enum Inno.Rendering.RenderPrimitiveTopology : int`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L355) | Selects the primitive assembly used by raster draw commands. | — | — |

### `Inno.Rendering.RenderRasterState`

<!-- api: T:Inno.Rendering.RenderRasterState 8a5aa4343c4292abacaa9254e9f061ef4a784bc48fcbab0ce9befe64f3aa85c4 -->
<!-- api: M:Inno.Rendering.RenderRasterState.#ctor ed391dad6ae0a34689bedbb016faf01d411c56272e6b98621f1e7adf73dc4859 -->
<!-- api: M:Inno.Rendering.RenderRasterState.#ctor(Inno.Rendering.RenderCullMode,Inno.Rendering.RenderFrontFace,Inno.Rendering.RenderDepthCompare,System.Boolean,Inno.Rendering.RenderBlendState,System.Byte,System.Boolean,Inno.Rendering.RenderPrimitiveTopology) 3ed623ab7a3160c338783f562fdd89a695444b57ccaedc0d5cd2b2241dbee107 -->
<!-- api: P:Inno.Rendering.RenderRasterState.opaque 2ef6902800ec65d93348416a79d6b42cd48e6c207761e9f80ace804ab3ea3d6f -->
<!-- api: P:Inno.Rendering.RenderRasterState.cull d76e988faf7c9420d983f575e0eb33745845a4335295fc12d5bc36b92ec5aac7 -->
<!-- api: P:Inno.Rendering.RenderRasterState.frontFace bc37c27d7b81a37e0877a8ee635c56cfb62b9c70a2b3ee5638a7e4816103c4a2 -->
<!-- api: P:Inno.Rendering.RenderRasterState.depthCompare c8b561c3f85304228c1d8e514c7d24075c46f4dc93663c058227a0a8be09c508 -->
<!-- api: P:Inno.Rendering.RenderRasterState.depthWrite b324dde347b8a7d7841273d8cb474d33ebbacfa86eca9c2ec37b0c343cddb7d8 -->
<!-- api: P:Inno.Rendering.RenderRasterState.blend c4ff4a1852f350dad629547cb5b2118f2b1bad550e24aad1f832d3fbc1194034 -->
<!-- api: P:Inno.Rendering.RenderRasterState.colorWriteMask b41c3b3ce2f60d05c23d8b15ddc8cf5c7cb33f95a11b5aba33e86eaca191108c -->
<!-- api: P:Inno.Rendering.RenderRasterState.multisampling 3f9354daefa0045c2f0b795152c5b29362eafe43f1730ed9a7f4ee314144a08a -->
<!-- api: P:Inno.Rendering.RenderRasterState.topology 0bb7d94ac932c70c3eaf15f1ce15240eb762ff5a1410e53dd8585ce7a04428d2 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-215f80efcdc2ac31"></a>[`public Inno.Rendering.RenderRasterState.RenderRasterState()`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L88) | Creates the default opaque raster state. | — | — |
| <a id="api-b628f3f1494087b7"></a>[`public Inno.Rendering.RenderRasterState.RenderRasterState(Inno.Rendering.RenderCullMode cull, Inno.Rendering.RenderFrontFace frontFace, Inno.Rendering.RenderDepthCompare depthCompare, bool depthWrite, Inno.Rendering.RenderBlendState blend, byte colorWriteMask, bool multisampling, Inno.Rendering.RenderPrimitiveTopology topology)`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L119) | Creates a complete immutable raster configuration without relying on init-only setters.<br>`cull`：Face culling mode.<br>`frontFace`：Winding order interpreted as the front face.<br>`depthCompare`：Depth comparison.<br>`depthWrite`：Whether accepted fragments update depth.<br>`blend`：Independent RGB and alpha blending.<br>`colorWriteMask`：Four-bit RGBA write mask.<br>`multisampling`：Whether multisample rasterization is enabled.<br>`topology`：Primitive assembly for subsequent draw commands. | — | — |
| <a id="api-13522c420fad2b5f"></a>[`public Inno.Rendering.RenderBlendState Inno.Rendering.RenderRasterState.blend { get; init; }`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L167) | Gets independent RGB and alpha blending. | — | — |
| <a id="api-1d832f1b205fa93c"></a>[`public byte Inno.Rendering.RenderRasterState.colorWriteMask { get; init; }`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L172) | Gets the four-bit RGBA write mask. | — | — |
| <a id="api-99a71b1bb3441f2a"></a>[`public Inno.Rendering.RenderCullMode Inno.Rendering.RenderRasterState.cull { get; init; }`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L147) | Gets the face culling mode. | — | — |
| <a id="api-739424f5ea113c45"></a>[`public Inno.Rendering.RenderDepthCompare Inno.Rendering.RenderRasterState.depthCompare { get; init; }`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L157) | Gets depth comparison. | — | — |
| <a id="api-cfee929853c0405a"></a>[`public bool Inno.Rendering.RenderRasterState.depthWrite { get; init; }`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L162) | Gets whether accepted fragments update depth. | — | — |
| <a id="api-db22f3d7218c4dab"></a>[`public Inno.Rendering.RenderFrontFace Inno.Rendering.RenderRasterState.frontFace { get; init; }`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L152) | Gets the winding order interpreted as the front face. | — | — |
| <a id="api-6ef4b2bdf1f64f6b"></a>[`public bool Inno.Rendering.RenderRasterState.multisampling { get; init; }`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L177) | Gets whether multisample rasterization is enabled for compatible targets. | — | — |
| <a id="api-852a89e6c4a6e53c"></a>[`public static Inno.Rendering.RenderRasterState Inno.Rendering.RenderRasterState.opaque { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L142) | Gets the default opaque raster state. | — | — |
| <a id="api-f76d1224070d014d"></a>[`public Inno.Rendering.RenderPrimitiveTopology Inno.Rendering.RenderRasterState.topology { get; init; }`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L182) | Gets primitive assembly for subsequent draw commands. | — | — |
| <a id="api-4c609944dec497a3"></a>[`public sealed class Inno.Rendering.RenderRasterState`](../../src/services/rendering/Inno.Rendering/Device/RenderRasterState.cs#L83) | Stores backend-neutral fixed-function raster state. | — | — |

### `Inno.Rendering.RenderResourceId`

<!-- api: T:Inno.Rendering.RenderResourceId 2bb432058aea766b83e36e4e13b1d79f5073dc66f179d764ad9e71fdcf4cd1e3 -->
<!-- api: M:Inno.Rendering.RenderResourceId.#ctor(System.String) c24c4b25c36531a2767439997dea1452cedd2001358d13fa8dc80d939cc236df -->
<!-- api: P:Inno.Rendering.RenderResourceId.value 2b05c7279c7e94db4683acba4606ffc8ef1b9b9833f1b7e1231c6f4e9d178fc3 -->
<!-- api: P:Inno.Rendering.RenderResourceId.isValid aa87cafceb8071e25abfe69bd050ba7fe0d376abb2149c3ee05945e70fe80015 -->
<!-- api: M:Inno.Rendering.RenderResourceId.ToString 9e88e254587ebb6524f948f5f7f39eb7a80136185087e062143471aafe145087 -->
<!-- api: M:Inno.Rendering.RenderResourceId.op_Inequality(Inno.Rendering.RenderResourceId,Inno.Rendering.RenderResourceId) 7a37f5a02c01f35c6f5058fa018088a33a50ab8e9ae9f7ab660fdaa216a97a9d -->
<!-- api: M:Inno.Rendering.RenderResourceId.op_Equality(Inno.Rendering.RenderResourceId,Inno.Rendering.RenderResourceId) 4796e585623383cf70e94f9e071ce9297d0a6fd463a3162e336372832ee3e6a1 -->
<!-- api: M:Inno.Rendering.RenderResourceId.GetHashCode c89fa15cf3caec426225ae5f82d4526f3b9e3dfa419e651c4c2a385fcd04d6f9 -->
<!-- api: M:Inno.Rendering.RenderResourceId.Equals(System.Object) 73f03b3b84f4f58ddba97ce044a748cc150710c6cbdead0c52c1be5928dcd892 -->
<!-- api: M:Inno.Rendering.RenderResourceId.Equals(Inno.Rendering.RenderResourceId) 95c6e53b2140b662c413964c8c1e744e7d5234b739327bfdba940f42b6491a7a -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7c7c97ff93fcb32d"></a>[`public Inno.Rendering.RenderResourceId.RenderResourceId(string value)`](../../src/services/rendering/Inno.Rendering/Device/RenderProtocolIds.cs#L16) | Creates an open semantic resource identifier.<br>`value`：Globally stable resource protocol value. | — | — |
| <a id="api-7ad4e25d8a86ac5e"></a>[`public bool Inno.Rendering.RenderResourceId.Equals(Inno.Rendering.RenderResourceId other)`](../../src/services/rendering/Inno.Rendering/Device/RenderProtocolIds.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-b25f9a597295b764"></a>[`public override bool Inno.Rendering.RenderResourceId.Equals(object obj)`](../../src/services/rendering/Inno.Rendering/Device/RenderProtocolIds.cs#L8) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-f9ed222eb822a25e"></a>[`public override int Inno.Rendering.RenderResourceId.GetHashCode()`](../../src/services/rendering/Inno.Rendering/Device/RenderProtocolIds.cs#L8) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-8a754ceaf3907b70"></a>[`public static bool Inno.Rendering.RenderResourceId.operator ==(Inno.Rendering.RenderResourceId left, Inno.Rendering.RenderResourceId right)`](../../src/services/rendering/Inno.Rendering/Device/RenderProtocolIds.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-da19e41ed787f943"></a>[`public static bool Inno.Rendering.RenderResourceId.operator !=(Inno.Rendering.RenderResourceId left, Inno.Rendering.RenderResourceId right)`](../../src/services/rendering/Inno.Rendering/Device/RenderProtocolIds.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-3218dde1204779df"></a>[`public override string Inno.Rendering.RenderResourceId.ToString()`](../../src/services/rendering/Inno.Rendering/Device/RenderProtocolIds.cs#L38) | Formats this value as a human-readable representation.<br>返回：The human-readable representation of this value. | — | — |
| <a id="api-252769c3bba4af91"></a>[`public bool Inno.Rendering.RenderResourceId.isValid { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderProtocolIds.cs#L30) | Gets whether the identifier contains a protocol value. | — | — |
| <a id="api-1407121d1ced6256"></a>[`public string Inno.Rendering.RenderResourceId.value { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderProtocolIds.cs#L25) | Gets the globally stable protocol value. | — | — |
| <a id="api-e3060a729e4b129e"></a>[`public readonly record struct Inno.Rendering.RenderResourceId : System.IEquatable<Inno.Rendering.RenderResourceId>`](../../src/services/rendering/Inno.Rendering/Device/RenderProtocolIds.cs#L8) | Identifies a pipeline-defined semantic resource without imposing a central resource catalog. | — | — |

### `Inno.Rendering.RenderSamplerAddressMode`

<!-- api: T:Inno.Rendering.RenderSamplerAddressMode 3a549330adbd694f3fef95dc92a938335abdae485d6ed934340fbc010f2f8120 -->
<!-- api: F:Inno.Rendering.RenderSamplerAddressMode.Repeat 2276b4141ed360405e8672d772366b65e00b2513042d4ecb26a36128d973e6bb -->
<!-- api: F:Inno.Rendering.RenderSamplerAddressMode.Mirror cf5f93328cd35a8aabbedbc70433bdca41866d4a3d9e261f8ebd91266237483f -->
<!-- api: F:Inno.Rendering.RenderSamplerAddressMode.Clamp 18fe4c9b84053fca894fc92f8ed85bcd6c7fa6d95acdf36f07c32bb0aed0ce93 -->
<!-- api: F:Inno.Rendering.RenderSamplerAddressMode.Border e0363b1948fa88cd94d60772b4501bb266f5d7c363f8071c29f3776d3faf87dc -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4f2b7835bd32a64c"></a>[`Inno.Rendering.RenderSamplerAddressMode.Border = 3`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L46) | Samples the backend border color outside the texture. | — | — |
| <a id="api-60356d1617c90bb3"></a>[`Inno.Rendering.RenderSamplerAddressMode.Clamp = 2`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L42) | Clamps coordinates to the texture edge. | — | — |
| <a id="api-4f3c061eb0f79d82"></a>[`Inno.Rendering.RenderSamplerAddressMode.Mirror = 1`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L38) | Mirrors repeated texture coordinates. | — | — |
| <a id="api-382060e051ecac50"></a>[`Inno.Rendering.RenderSamplerAddressMode.Repeat = 0`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L34) | Repeats texture coordinates. | — | — |
| <a id="api-8fa16975966de556"></a>[`public enum Inno.Rendering.RenderSamplerAddressMode : int`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L29) | Selects texture addressing independently for each coordinate axis. | — | — |

### `Inno.Rendering.RenderSamplerFilter`

<!-- api: T:Inno.Rendering.RenderSamplerFilter a65b48c81a38ff443880590b0768f6e6ed68e7ca6f6de4903f457195c931f8af -->
<!-- api: F:Inno.Rendering.RenderSamplerFilter.Point 4350a9ad23ae60dc332c0c589072beac91e28f867563d0d5b31006c524898455 -->
<!-- api: F:Inno.Rendering.RenderSamplerFilter.Linear 61be1125a1d7edc9dd2826d87b464293208f1974a578a923761b9fdde335526f -->
<!-- api: F:Inno.Rendering.RenderSamplerFilter.Anisotropic 1940d1f4b719ad0dfedaedac120dae02f0c54e9245d84a391784b0f624aa06c8 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-474f44934532ef36"></a>[`Inno.Rendering.RenderSamplerFilter.Anisotropic = 2`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L23) | Uses anisotropic filtering when supported. | — | — |
| <a id="api-addde3aa2463c8c3"></a>[`Inno.Rendering.RenderSamplerFilter.Linear = 1`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L19) | Uses linear filtering. | — | — |
| <a id="api-012a2f697e8b1fb8"></a>[`Inno.Rendering.RenderSamplerFilter.Point = 0`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L15) | Uses nearest-neighbor filtering. | — | — |
| <a id="api-7e940ed1519f0c17"></a>[`public enum Inno.Rendering.RenderSamplerFilter : int`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L10) | Selects texture filtering independently from a graphics backend. | — | — |

### `Inno.Rendering.RenderSamplerState`

<!-- api: T:Inno.Rendering.RenderSamplerState be86301bd00f0c621dcf5e4d36141b54356cfb2063ddc24814e1d185482bbf21 -->
<!-- api: P:Inno.Rendering.RenderSamplerState.linearClamp 84020b09e2cce0d7f65f7eb6866b4540e4b7bdfd4c03e3caf205975e8a846486 -->
<!-- api: M:Inno.Rendering.RenderSamplerState.#ctor(Inno.Rendering.RenderSamplerFilter,Inno.Rendering.RenderSamplerAddressMode,Inno.Rendering.RenderSamplerAddressMode,Inno.Rendering.RenderSamplerAddressMode) 5f1b869b6869a53f56a29024c0a136ac2e92247156866119f1c609aa2cdcf3be -->
<!-- api: P:Inno.Rendering.RenderSamplerState.filter 6f23ac18258d2d88f1508dcaf984a2c23e78f4c5bc57b1dd67454dd82b4eb606 -->
<!-- api: P:Inno.Rendering.RenderSamplerState.addressU d0d26d38273f19094096bf025ccf728ee29479d2a9b869c6b612ad499d5bdc44 -->
<!-- api: P:Inno.Rendering.RenderSamplerState.addressV 62dc39a489f86c232180c81e5128fd0916a70dd76a19988e06c171c801ce815d -->
<!-- api: P:Inno.Rendering.RenderSamplerState.addressW 1dd4748bbe3a556d017fa0b1323aaa63b4dede3ede4fa4abff5574a178cc43bf -->
<!-- api: M:Inno.Rendering.RenderSamplerState.Equals(Inno.Rendering.RenderSamplerState) 2c6c239c46d3b7950119a45250af2ecbf03825fd4454bebb1fdedab8447772d3 -->
<!-- api: M:Inno.Rendering.RenderSamplerState.Equals(System.Object) 02bad8aa17d2b292311a8e073e4d15b48a4d488febfc08b859a32f51f30266cd -->
<!-- api: M:Inno.Rendering.RenderSamplerState.GetHashCode 1d3cb73eb18f5ae1f940819bf31baf838a9f2c004ee4d3d5fa92e47604d4dd9d -->
<!-- api: M:Inno.Rendering.RenderSamplerState.op_Equality(Inno.Rendering.RenderSamplerState,Inno.Rendering.RenderSamplerState) c83901cd6b97cb047be19cf567a0c91081b5abe0fda9e3f25dd96f4e15e17c5a -->
<!-- api: M:Inno.Rendering.RenderSamplerState.op_Inequality(Inno.Rendering.RenderSamplerState,Inno.Rendering.RenderSamplerState) 2c651126ae4fc0d4c506118b48a10e3c6ec64742569ee803f89f6af6f235fb36 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-792f7556ae3fdcf4"></a>[`public Inno.Rendering.RenderSamplerState.RenderSamplerState(Inno.Rendering.RenderSamplerFilter filter, Inno.Rendering.RenderSamplerAddressMode addressU, Inno.Rendering.RenderSamplerAddressMode addressV, Inno.Rendering.RenderSamplerAddressMode addressW)`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L78) | Creates a sampler state.<br>`filter`：Minification, magnification, and mip filtering contract.<br>`addressU`：Horizontal address mode.<br>`addressV`：Vertical address mode.<br>`addressW`：Depth or cube address mode. | — | — |
| <a id="api-edf6ebde9df2c6f5"></a>[`public readonly bool Inno.Rendering.RenderSamplerState.Equals(Inno.Rendering.RenderSamplerState other)`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L119) | Determines whether this instance and the supplied value represent the same logical state.<br>返回：true when both values represent the same logical state; otherwise, false.<br>`other`：The value to compare with this instance. | — | — |
| <a id="api-d9a7768544b37f70"></a>[`public override readonly bool Inno.Rendering.RenderSamplerState.Equals(object? obj)`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L134) | Determines whether this instance and the supplied value represent the same logical state.<br>返回：true when both values represent the same logical state; otherwise, false.<br>`obj`：The object to compare with this instance. | — | — |
| <a id="api-ec870ae39b390812"></a>[`public override readonly int Inno.Rendering.RenderSamplerState.GetHashCode()`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L142) | Computes a hash code from the fields that participate in logical equality.<br>返回：A hash code consistent with the implemented equality contract. | — | — |
| <a id="api-3c2997778db5d9bd"></a>[`public static bool Inno.Rendering.RenderSamplerState.operator ==(Inno.Rendering.RenderSamplerState left, Inno.Rendering.RenderSamplerState right)`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L156) | Determines whether two sampler descriptions are equal.<br>返回：True when every filtering and addressing field is equal.<br>`left`：Left sampler description.<br>`right`：Right sampler description. | — | — |
| <a id="api-d5e9a3b150f2570e"></a>[`public static bool Inno.Rendering.RenderSamplerState.operator !=(Inno.Rendering.RenderSamplerState left, Inno.Rendering.RenderSamplerState right)`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L173) | Determines whether two sampler descriptions differ.<br>返回：True when at least one filtering or addressing field differs.<br>`left`：Left sampler description.<br>`right`：Right sampler description. | — | — |
| <a id="api-eff53abfb08dd7ed"></a>[`public Inno.Rendering.RenderSamplerAddressMode Inno.Rendering.RenderSamplerState.addressU { get; set; }`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L98) | Gets horizontal addressing. | — | — |
| <a id="api-112a94baca2576b2"></a>[`public Inno.Rendering.RenderSamplerAddressMode Inno.Rendering.RenderSamplerState.addressV { get; set; }`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L103) | Gets vertical addressing. | — | — |
| <a id="api-1e28ac26d768435b"></a>[`public Inno.Rendering.RenderSamplerAddressMode Inno.Rendering.RenderSamplerState.addressW { get; set; }`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L108) | Gets depth or cube addressing. | — | — |
| <a id="api-cdaf209840776926"></a>[`public Inno.Rendering.RenderSamplerFilter Inno.Rendering.RenderSamplerState.filter { get; set; }`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L93) | Gets the filter contract. | — | — |
| <a id="api-c4d4694f5ad20597"></a>[`public static Inno.Rendering.RenderSamplerState Inno.Rendering.RenderSamplerState.linearClamp { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L57) | Gets linear filtering with clamped addressing. | — | — |
| <a id="api-4c6b41a322f9d891"></a>[`public struct Inno.Rendering.RenderSamplerState : System.IEquatable<Inno.Rendering.RenderSamplerState>`](../../src/services/rendering/Inno.Rendering/Device/RenderSamplerState.cs#L52) | Describes one native-serializable backend-neutral sampler binding. | — | — |

### `Inno.Rendering.RenderShaderBindingDescriptor`

<!-- api: T:Inno.Rendering.RenderShaderBindingDescriptor f8d55055912d214f4213bd50f71962aaca258234debb230fdf76ffe883d505aa -->
<!-- api: M:Inno.Rendering.RenderShaderBindingDescriptor.#ctor(Inno.Rendering.RenderBindingId,Inno.Rendering.RenderShaderBindingKind,System.Int32,Inno.Rendering.RenderUniformType,System.Int32,Inno.Rendering.RenderStorageAccess,System.String) 4826251c8b68f9d0ce042ee133809931d83dd3f274ef49a11b0411bd7a457471 -->
<!-- api: P:Inno.Rendering.RenderShaderBindingDescriptor.id 9c369972c26e71b5a426c1802e808c82b8577d43588247312a646b73791895a0 -->
<!-- api: P:Inno.Rendering.RenderShaderBindingDescriptor.kind baff879f1f51a0d162139118e557f8f246d4f7ffc035e6ef291a45eeb23f3b97 -->
<!-- api: P:Inno.Rendering.RenderShaderBindingDescriptor.slot c63df7dea11eacdabfa0a7f15fe9da438717494989e045f37b0a67b36a87076f -->
<!-- api: P:Inno.Rendering.RenderShaderBindingDescriptor.uniformType c16235e4cfd6861977bddfd5c7d654806d2c59209c2881426e1242bc70d694cf -->
<!-- api: P:Inno.Rendering.RenderShaderBindingDescriptor.count 0662a8b3c98d1dd94cca0671c938172740b07ef8c88d3703c23e9cef26c73393 -->
<!-- api: P:Inno.Rendering.RenderShaderBindingDescriptor.storageAccess 812638b274b4eacbaa2e8077ef105e0c953bf328d4fa8cf68d9b39874c97bc7e -->
<!-- api: P:Inno.Rendering.RenderShaderBindingDescriptor.nativeName bdf727e4a515526a0cd7ed6412d3676939569e301c315d489745dc13fdec4b22 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b07c7ac647010c76"></a>[`public Inno.Rendering.RenderShaderBindingDescriptor.RenderShaderBindingDescriptor(Inno.Rendering.RenderBindingId id, Inno.Rendering.RenderShaderBindingKind kind, int slot = 0, Inno.Rendering.RenderUniformType uniformType = Inno.Rendering.RenderUniformType.Vector4, int count = 1, Inno.Rendering.RenderStorageAccess storageAccess = Inno.Rendering.RenderStorageAccess.Read, string? nativeName = null)`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L97) | Creates a shader binding descriptor.<br>`id`：Stable manifest binding name.<br>`kind`：Binding domain.<br>`slot`：Backend-neutral texture or storage slot.<br>`uniformType`：Uniform shape when kind is Uniform.<br>`count`：Uniform array element count.<br>`storageAccess`：Storage texture or buffer access.<br>`nativeName`：Adapter-generated reflected symbol, or null when the logical ID is also the symbol. | — | — |
| <a id="api-cabae5a1f81e038d"></a>[`public int Inno.Rendering.RenderShaderBindingDescriptor.count { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L152) | Gets uniform array element count. | — | — |
| <a id="api-8c30631b022e7a93"></a>[`public Inno.Rendering.RenderBindingId Inno.Rendering.RenderShaderBindingDescriptor.id { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L132) | Gets the stable manifest binding name. | — | — |
| <a id="api-c302243e56aea6fd"></a>[`public Inno.Rendering.RenderShaderBindingKind Inno.Rendering.RenderShaderBindingDescriptor.kind { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L137) | Gets the binding domain. | — | — |
| <a id="api-e21e4ddc89c2f195"></a>[`public string Inno.Rendering.RenderShaderBindingDescriptor.nativeName { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L162) | Gets the exact adapter-generated symbol used only for reflection and native resource creation. | — | — |
| <a id="api-6e86d17d3db609b5"></a>[`public int Inno.Rendering.RenderShaderBindingDescriptor.slot { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L142) | Gets the backend-neutral texture or storage slot. | — | — |
| <a id="api-f7ff239a0d21c2bf"></a>[`public Inno.Rendering.RenderStorageAccess Inno.Rendering.RenderShaderBindingDescriptor.storageAccess { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L157) | Gets storage texture or buffer access. | — | — |
| <a id="api-bb4a2b9e63391214"></a>[`public Inno.Rendering.RenderUniformType Inno.Rendering.RenderShaderBindingDescriptor.uniformType { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L147) | Gets the uniform shape. | — | — |
| <a id="api-0f5c3e025c196878"></a>[`public sealed class Inno.Rendering.RenderShaderBindingDescriptor`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L71) | Declares one manifest-derived shader binding used for reflection validation. | — | — |

### `Inno.Rendering.RenderShaderBindingKind`

<!-- api: T:Inno.Rendering.RenderShaderBindingKind 2e87a6bf5d59f21ffbfa4aa9166b81fef37e924de6623eafd7e9144ac33bbff8 -->
<!-- api: F:Inno.Rendering.RenderShaderBindingKind.Uniform 8feadd1c8cdce7b644081aa529b25e476dda70b38fa2ed74b6d407767a53cd15 -->
<!-- api: F:Inno.Rendering.RenderShaderBindingKind.Texture ab57629a40756cf282dc24e9179f47a4746650089efb789b141a0bae2d9ab67a -->
<!-- api: F:Inno.Rendering.RenderShaderBindingKind.StorageTexture b6199312df6bc117d1ba69a95dc1e675bf19c4a1c0a23d7d1cecb98285bfbeae -->
<!-- api: F:Inno.Rendering.RenderShaderBindingKind.StorageBuffer d3399c93d25cbd5e4154a925f25ac2a7216950639dff6f25c713de5b9186e9ef -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8dd9f6e157cd351a"></a>[`Inno.Rendering.RenderShaderBindingKind.StorageBuffer = 3`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L27) | Compute-readable or writable buffer. | — | — |
| <a id="api-980cc2453f2c7707"></a>[`Inno.Rendering.RenderShaderBindingKind.StorageTexture = 2`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L23) | Shader-readable or writable storage texture. | — | — |
| <a id="api-7bf4e8fb53a7d924"></a>[`Inno.Rendering.RenderShaderBindingKind.Texture = 1`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L19) | Sampled texture and sampler state. | — | — |
| <a id="api-b9cc1303917e2f2b"></a>[`Inno.Rendering.RenderShaderBindingKind.Uniform = 0`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L15) | Vector or matrix uniform data. | — | — |
| <a id="api-4b476a3ba69a8b6e"></a>[`public enum Inno.Rendering.RenderShaderBindingKind : int`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L10) | Identifies a shader interface binding domain. | — | — |

### `Inno.Rendering.RenderStencilCompare`

<!-- api: T:Inno.Rendering.RenderStencilCompare 45991c53e8f924dfaf226c0d4fd589bf39a7f4c31165ab3ac71237c2f4d315dc -->
<!-- api: F:Inno.Rendering.RenderStencilCompare.Never fc1f3a1decc5eba493a3646adebb279c3b4d6c194b4810a026a078ef3802f9fb -->
<!-- api: F:Inno.Rendering.RenderStencilCompare.Less 84335fa0c8fe260dd35614a48a41cd1fa17e87c9678cdb7f9b222ca926e5cd3a -->
<!-- api: F:Inno.Rendering.RenderStencilCompare.Equal 5c46982f36661c3258986d9cad3a39309316cfcf6c5d7aec988bc130ca7e0a34 -->
<!-- api: F:Inno.Rendering.RenderStencilCompare.LessEqual 938a8767708d97c8d569d711a422da2afa6dd73e51c80314cc0bbdcdefb73ad0 -->
<!-- api: F:Inno.Rendering.RenderStencilCompare.Greater bfa276ff98b28ac832de9f78e97b5c2bfbf40eb1d6161daa6b2046950ebd7bb2 -->
<!-- api: F:Inno.Rendering.RenderStencilCompare.NotEqual 045cc38566ba8c526bf96bd085548c38ad74e03853f8110ee01673f580050e34 -->
<!-- api: F:Inno.Rendering.RenderStencilCompare.GreaterEqual 1c62e0d257ed13ab11dbfde05987ee5ff15962cfc6c1e3886c10c70164c786c2 -->
<!-- api: F:Inno.Rendering.RenderStencilCompare.Always 8d821c7d64f6ad811f0c156d54578b79bf8fbf81517e43bcfee766ab01c9c1e6 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2104403ee479809c"></a>[`Inno.Rendering.RenderStencilCompare.Always = 7`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L43) | Always passes. | — | — |
| <a id="api-65fbbbb4ec66cdaa"></a>[`Inno.Rendering.RenderStencilCompare.Equal = 2`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L23) | Passes when values are equal. | — | — |
| <a id="api-5a2fb852f38affe0"></a>[`Inno.Rendering.RenderStencilCompare.Greater = 4`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L31) | Passes when reference is greater. | — | — |
| <a id="api-04227152a81a6615"></a>[`Inno.Rendering.RenderStencilCompare.GreaterEqual = 6`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L39) | Passes when reference is greater or equal. | — | — |
| <a id="api-32d60dad2872b960"></a>[`Inno.Rendering.RenderStencilCompare.Less = 1`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L19) | Passes when reference is smaller. | — | — |
| <a id="api-8238cfd66671d261"></a>[`Inno.Rendering.RenderStencilCompare.LessEqual = 3`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L27) | Passes when reference is smaller or equal. | — | — |
| <a id="api-9d79e1f8524c9612"></a>[`Inno.Rendering.RenderStencilCompare.Never = 0`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L15) | Never passes. | — | — |
| <a id="api-6a6d01216111a166"></a>[`Inno.Rendering.RenderStencilCompare.NotEqual = 5`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L35) | Passes when values differ. | — | — |
| <a id="api-d7a18e3b7c2bf34f"></a>[`public enum Inno.Rendering.RenderStencilCompare : int`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L10) | Selects the comparison applied to stencil reference and stored values. | — | — |

### `Inno.Rendering.RenderStencilFaceState`

<!-- api: T:Inno.Rendering.RenderStencilFaceState d9d8616669fb5a30492d44df82ef12d61db5b81331f67130a5366c5a95c2ad5c -->
<!-- api: M:Inno.Rendering.RenderStencilFaceState.#ctor(Inno.Rendering.RenderStencilCompare,Inno.Rendering.RenderStencilOperation,Inno.Rendering.RenderStencilOperation,Inno.Rendering.RenderStencilOperation) 5584f4905710e87ed04ee5ed11c4ad4748ab52eac2645516424189d4e75f49b8 -->
<!-- api: P:Inno.Rendering.RenderStencilFaceState.compare 6cb8c6d273375cffc70b5342fc6af070692c2610c63470b94d082a2229290ffb -->
<!-- api: P:Inno.Rendering.RenderStencilFaceState.fail f4390737ec5f4d7e73dcf9922786b385b4d93802439580f32d077967dc26f41e -->
<!-- api: P:Inno.Rendering.RenderStencilFaceState.depthFail 9f7beb1a13d5b192b5f294373667d27dd12842615b3a372855fc30ae210324fe -->
<!-- api: P:Inno.Rendering.RenderStencilFaceState.pass 6ad0cc7424722ca26cc2251abfd5ba3449b78f07cf35c49672a083d9c481ceb1 -->
<!-- api: M:Inno.Rendering.RenderStencilFaceState.ToString 9acc2da760e0ba699909ab77c63ad02f4cb9a19258be5814dc14f3a74a50321c -->
<!-- api: M:Inno.Rendering.RenderStencilFaceState.op_Inequality(Inno.Rendering.RenderStencilFaceState,Inno.Rendering.RenderStencilFaceState) 3e473f04f7c5e48156c73f6d5a309dabb975675f3134b8de2bcd0f42ce4b1765 -->
<!-- api: M:Inno.Rendering.RenderStencilFaceState.op_Equality(Inno.Rendering.RenderStencilFaceState,Inno.Rendering.RenderStencilFaceState) 7480c5097d910b3976ae53fc27251859f47a09de8380ac458e345cf7b5a2594f -->
<!-- api: M:Inno.Rendering.RenderStencilFaceState.GetHashCode e071ef0a392b3e58767d89d3a1a6ab3d765deb241fe4dfcad2abd5634dccfa19 -->
<!-- api: M:Inno.Rendering.RenderStencilFaceState.Equals(System.Object) 026c2cd27f4bdc9829309fcc81e096d0e4af02dedf91b940445fd02dc8d243d5 -->
<!-- api: M:Inno.Rendering.RenderStencilFaceState.Equals(Inno.Rendering.RenderStencilFaceState) 75505a08156977d1d66fd51a28404cee018dc21f46110f48223c45d8a6566e81 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b014c96485c6e080"></a>[`public Inno.Rendering.RenderStencilFaceState.RenderStencilFaceState(Inno.Rendering.RenderStencilCompare compare, Inno.Rendering.RenderStencilOperation fail, Inno.Rendering.RenderStencilOperation depthFail, Inno.Rendering.RenderStencilOperation pass)`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L105) | Creates one face stencil state.<br>`compare`：Stencil comparison.<br>`fail`：Operation after stencil comparison failure.<br>`depthFail`：Operation after stencil success and depth failure.<br>`pass`：Operation after stencil and depth success. | — | — |
| <a id="api-4f163aeb3b0fe02c"></a>[`public bool Inno.Rendering.RenderStencilFaceState.Equals(Inno.Rendering.RenderStencilFaceState other)`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L88) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-2fc59c4d06a3922a"></a>[`public override bool Inno.Rendering.RenderStencilFaceState.Equals(object obj)`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L88) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-847c5bf0dd2b853b"></a>[`public override int Inno.Rendering.RenderStencilFaceState.GetHashCode()`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L88) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-1f53d6f889039141"></a>[`public static bool Inno.Rendering.RenderStencilFaceState.operator ==(Inno.Rendering.RenderStencilFaceState left, Inno.Rendering.RenderStencilFaceState right)`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L88) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-0c4a115155f24741"></a>[`public static bool Inno.Rendering.RenderStencilFaceState.operator !=(Inno.Rendering.RenderStencilFaceState left, Inno.Rendering.RenderStencilFaceState right)`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L88) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-0ef6c13f56da17ea"></a>[`public override string Inno.Rendering.RenderStencilFaceState.ToString()`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L88) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-ff5ebd3a84eac0e6"></a>[`public Inno.Rendering.RenderStencilCompare Inno.Rendering.RenderStencilFaceState.compare { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L120) | Gets stencil comparison. | — | — |
| <a id="api-6d6a0917659eb4d6"></a>[`public Inno.Rendering.RenderStencilOperation Inno.Rendering.RenderStencilFaceState.depthFail { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L130) | Gets the depth-failure operation. | — | — |
| <a id="api-3147b811fd278c5a"></a>[`public Inno.Rendering.RenderStencilOperation Inno.Rendering.RenderStencilFaceState.fail { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L125) | Gets the stencil-failure operation. | — | — |
| <a id="api-653fd3a6834961f1"></a>[`public Inno.Rendering.RenderStencilOperation Inno.Rendering.RenderStencilFaceState.pass { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L135) | Gets the complete-pass operation. | — | — |
| <a id="api-e530a8a5c379cb20"></a>[`public readonly record struct Inno.Rendering.RenderStencilFaceState : System.IEquatable<Inno.Rendering.RenderStencilFaceState>`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L88) | Describes stencil behavior for one triangle face orientation. | — | — |

### `Inno.Rendering.RenderStencilOperation`

<!-- api: T:Inno.Rendering.RenderStencilOperation c11d5ebac97e739bc0963725d6b2c98206722196e61d10c17334490de922fcce -->
<!-- api: F:Inno.Rendering.RenderStencilOperation.Keep 93d33de59dda90da534b9b76b76eccceb3a114d203f6c024d7acd642af8db256 -->
<!-- api: F:Inno.Rendering.RenderStencilOperation.Zero 2b8d66b1cccd44fe7adf7735d9558321d2b48d4b0d323e632d37c4bdfeab9adb -->
<!-- api: F:Inno.Rendering.RenderStencilOperation.Replace ae193adf6600b9358e0ea9a6c2732fdfa4ae2fbf5f9e4496aa30779297ea7048 -->
<!-- api: F:Inno.Rendering.RenderStencilOperation.IncrementClamp af03fe6174ddfedac275b8af1d402de513f50c1a06e28c4049b28f68807a7aaf -->
<!-- api: F:Inno.Rendering.RenderStencilOperation.IncrementWrap f1d92f3a01fba2550f8a075dfb444dab5d6f3c3dd8a3a11a81a4082f106a0958 -->
<!-- api: F:Inno.Rendering.RenderStencilOperation.DecrementClamp 28e4ab4fc8b93bb09592311ff557dc42855af1c663c32b97deb61b12d0ddcf50 -->
<!-- api: F:Inno.Rendering.RenderStencilOperation.DecrementWrap 0ef82ed855c13b4c594e8d3e4624c7b6cb8949e9e2278fd5de230f6adcfdb20a -->
<!-- api: F:Inno.Rendering.RenderStencilOperation.Invert 0fce9e41f396dce20ae334869f425b478214d38d45f2d1f0eec8942512283505 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-cb70a4a3e3e9ce77"></a>[`Inno.Rendering.RenderStencilOperation.DecrementClamp = 5`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L74) | Decrements and clamps the stored value. | — | — |
| <a id="api-b18dac84cac06893"></a>[`Inno.Rendering.RenderStencilOperation.DecrementWrap = 6`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L78) | Decrements and wraps the stored value. | — | — |
| <a id="api-17f068e8eaaf6c01"></a>[`Inno.Rendering.RenderStencilOperation.IncrementClamp = 3`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L66) | Increments and clamps the stored value. | — | — |
| <a id="api-9c921dd6e78ffbe8"></a>[`Inno.Rendering.RenderStencilOperation.IncrementWrap = 4`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L70) | Increments and wraps the stored value. | — | — |
| <a id="api-153e6c6178d58df6"></a>[`Inno.Rendering.RenderStencilOperation.Invert = 7`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L82) | Bitwise-inverts the stored value. | — | — |
| <a id="api-3b0b7ae03fb21a18"></a>[`Inno.Rendering.RenderStencilOperation.Keep = 0`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L54) | Keeps the stored value. | — | — |
| <a id="api-346ab11e22dab727"></a>[`Inno.Rendering.RenderStencilOperation.Replace = 2`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L62) | Replaces the stored value with the reference. | — | — |
| <a id="api-88a0e77f7eae9d1f"></a>[`Inno.Rendering.RenderStencilOperation.Zero = 1`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L58) | Clears the stored value to zero. | — | — |
| <a id="api-b56a616857c84980"></a>[`public enum Inno.Rendering.RenderStencilOperation : int`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L49) | Selects the update applied to a stencil value. | — | — |

### `Inno.Rendering.RenderStencilState`

<!-- api: T:Inno.Rendering.RenderStencilState 19cf5c61912f17f085bf6ef808e5f7a89ed2f9f663d9cfe260cfafd3a8548a50 -->
<!-- api: M:Inno.Rendering.RenderStencilState.#ctor 9e6f42c8d1a5b8fa8e63075e6f9e2b935d27eaabacc7f2c110da501d71803ffd -->
<!-- api: M:Inno.Rendering.RenderStencilState.#ctor(System.Boolean,System.Byte,System.Byte,System.Byte,Inno.Rendering.RenderStencilFaceState,Inno.Rendering.RenderStencilFaceState) 586a348babdd26e3280833ffc88e5c7f084032c3de61515274752f16b5fcbade -->
<!-- api: P:Inno.Rendering.RenderStencilState.disabled edf6dd5da8d3fd1115e1ba5b41898ff64e7f9d3fd657738e722f34e4fdff18bd -->
<!-- api: P:Inno.Rendering.RenderStencilState.enabled 88734e7d2eb30dea6e1d1dc62fd9f13d4f35c10291e89b780f98684f7b3388bc -->
<!-- api: P:Inno.Rendering.RenderStencilState.reference 22f2e7e5f7f5ff06bd4f60f0db5dbab54ff24f341c70815ec10e673212e2eee8 -->
<!-- api: P:Inno.Rendering.RenderStencilState.readMask f9fc359949bb8a5f1a1becd037cacbe5f5ac1319d80888b0d694feac513d5799 -->
<!-- api: P:Inno.Rendering.RenderStencilState.writeMask 64bee10bf0cd27cb0c700a0f016315a894969b1b4de696823868ad01a709b736 -->
<!-- api: P:Inno.Rendering.RenderStencilState.front 230b958780409474ebeb9a1c99b329f502bf07c52f8bf93fc6cc5cf2ef41aa23 -->
<!-- api: P:Inno.Rendering.RenderStencilState.back e201b3ee4bf8355fdea7ef14d3567a2f88055d00a8d8cc46677c462c92549106 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-06e4e2679dea647d"></a>[`public Inno.Rendering.RenderStencilState.RenderStencilState()`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L146) | Creates the default disabled stencil state. | — | — |
| <a id="api-386a69aea1650a90"></a>[`public Inno.Rendering.RenderStencilState.RenderStencilState(bool enabled, byte reference, byte readMask, byte writeMask, Inno.Rendering.RenderStencilFaceState front, Inno.Rendering.RenderStencilFaceState back)`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L171) | Creates a complete immutable stencil configuration without relying on init-only setters.<br>`enabled`：Whether stencil testing and updates are active.<br>`reference`：Eight-bit stencil reference value.<br>`readMask`：Mask applied while reading stored stencil.<br>`writeMask`：Mask applied while writing stencil.<br>`front`：Front-face stencil behavior.<br>`back`：Back-face stencil behavior. | — | — |
| <a id="api-735b183371d193eb"></a>[`public Inno.Rendering.RenderStencilFaceState Inno.Rendering.RenderStencilState.back { get; init; }`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L224) | Gets back-face stencil behavior. | — | — |
| <a id="api-650c98aeb2b21fed"></a>[`public static Inno.Rendering.RenderStencilState Inno.Rendering.RenderStencilState.disabled { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L190) | Gets disabled stencil state. | — | — |
| <a id="api-ec87f5a4b38b9dc3"></a>[`public bool Inno.Rendering.RenderStencilState.enabled { get; init; }`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L195) | Gets whether stencil testing and updates are active. | — | — |
| <a id="api-74e29448a1c7095c"></a>[`public Inno.Rendering.RenderStencilFaceState Inno.Rendering.RenderStencilState.front { get; init; }`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L215) | Gets front-face stencil behavior. | — | — |
| <a id="api-4e7073fd32a31be4"></a>[`public byte Inno.Rendering.RenderStencilState.readMask { get; init; }`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L205) | Gets the mask applied while reading stored stencil. | — | — |
| <a id="api-cffda2a63e36bbb4"></a>[`public byte Inno.Rendering.RenderStencilState.reference { get; init; }`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L200) | Gets the eight-bit stencil reference value. | — | — |
| <a id="api-171a6d144cf9bf0f"></a>[`public byte Inno.Rendering.RenderStencilState.writeMask { get; init; }`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L210) | Gets the mask applied while writing stencil. | — | — |
| <a id="api-22b863f1d90f449e"></a>[`public sealed class Inno.Rendering.RenderStencilState`](../../src/services/rendering/Inno.Rendering/Device/RenderStencilState.cs#L141) | Describes complete two-sided stencil state for one draw. | — | — |

### `Inno.Rendering.RenderStorageAccess`

<!-- api: T:Inno.Rendering.RenderStorageAccess 99d50038d2c3e5d919e73d729ee37002fe259b7b9a23171c24fec4e6ab2dfebc -->
<!-- api: F:Inno.Rendering.RenderStorageAccess.Read 7a2cb701f42fa6eec5eec0cebf838ece877ec2571db5dcd6e5d28d8fde28817d -->
<!-- api: F:Inno.Rendering.RenderStorageAccess.Write 395952c0fa8a13c2c5b8557811d3579a1006aaca41cca4ca9a030f60b1985993 -->
<!-- api: F:Inno.Rendering.RenderStorageAccess.ReadWrite b8079c612c8d22926a8318c92448cde576d5deddac3f8eebe348f46272f4f8d0 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b0ad40cbe7d09567"></a>[`Inno.Rendering.RenderStorageAccess.Read = 0`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L57) | Shader read-only access. | — | — |
| <a id="api-74b7f095adf64ad8"></a>[`Inno.Rendering.RenderStorageAccess.ReadWrite = 2`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L65) | Shader read and write access. | — | — |
| <a id="api-70fee41a518c85cb"></a>[`Inno.Rendering.RenderStorageAccess.Write = 1`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L61) | Shader write-only access. | — | — |
| <a id="api-bf37019cd0e65a5a"></a>[`public enum Inno.Rendering.RenderStorageAccess : int`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L52) | Selects unordered storage-resource access for one shader binding. | — | — |

### `Inno.Rendering.RenderStoreAction`

<!-- api: T:Inno.Rendering.RenderStoreAction 759d5fae5fd2e81414d8ba8601aaaf7466aa3a7ecc3eac7840e440bdaf4ef3d9 -->
<!-- api: F:Inno.Rendering.RenderStoreAction.Store c88bab6549a8e92ef757e69f07df7f875b317fbd7ea0aa12a57c71d5643ca7d6 -->
<!-- api: F:Inno.Rendering.RenderStoreAction.Discard 139424302a31ec2d7b3c7b92049267134999e11ecc512a29d0f397dbea76ceac -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9668cbeae2afa85f"></a>[`Inno.Rendering.RenderStoreAction.Discard = 1`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L103) | Allows the backend to discard rendered contents. | — | — |
| <a id="api-e6f55a7b73e52cbc"></a>[`Inno.Rendering.RenderStoreAction.Store = 0`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L99) | Preserves rendered contents. | — | — |
| <a id="api-07a43f1641a57ac8"></a>[`public enum Inno.Rendering.RenderStoreAction : int`](../../src/services/rendering/Inno.Rendering/Graph/RenderGraphTypes.cs#L94) | Controls whether attachment contents remain valid after a raster pass. | — | — |

### `Inno.Rendering.RenderSurfaceHandle`

<!-- api: T:Inno.Rendering.RenderSurfaceHandle a2eb77ccda7d2f1a4cf7f6fbac1f2cfc41f7856e7606ca866da7bbaff228bedc -->
<!-- api: P:Inno.Rendering.RenderSurfaceHandle.isValid 8e5b06c19bc8dce8b224164ca3d4b951552d152206b61927bf62502059e38002 -->
<!-- api: M:Inno.Rendering.RenderSurfaceHandle.ToString 9cbd6d5ab00350d679cd7a5d2ab5e3b217d527cd5390a850faf395395b7b27e2 -->
<!-- api: M:Inno.Rendering.RenderSurfaceHandle.op_Inequality(Inno.Rendering.RenderSurfaceHandle,Inno.Rendering.RenderSurfaceHandle) b806dc08b01d58ff59ec2eaab3980e1d4b5bd3f934601b529e7334aaa4b679c8 -->
<!-- api: M:Inno.Rendering.RenderSurfaceHandle.op_Equality(Inno.Rendering.RenderSurfaceHandle,Inno.Rendering.RenderSurfaceHandle) b9fc1bd2f146b1a6386cc431ffec4dc3a4278a4627f024b4252d264a70184a06 -->
<!-- api: M:Inno.Rendering.RenderSurfaceHandle.GetHashCode 9d1e4df69d29cf782595cb757516f9974c5536901eb429c62c14e48a61744618 -->
<!-- api: M:Inno.Rendering.RenderSurfaceHandle.Equals(System.Object) c13f44d48067e823bf17f15ae70bdc584608e356251814056ce08608036022ca -->
<!-- api: M:Inno.Rendering.RenderSurfaceHandle.Equals(Inno.Rendering.RenderSurfaceHandle) 0573fcfad5c437db6a590fc6f1a58306c516667e5e7f7fa4c40051f61dd03fa6 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-bb44d575d42be074"></a>[`public bool Inno.Rendering.RenderSurfaceHandle.Equals(Inno.Rendering.RenderSurfaceHandle other)`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L537) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-07b3669d17d6ec55"></a>[`public override bool Inno.Rendering.RenderSurfaceHandle.Equals(object obj)`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L537) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-a5ce8590204bb6fe"></a>[`public override int Inno.Rendering.RenderSurfaceHandle.GetHashCode()`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L537) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-a59a4638781888e4"></a>[`public static bool Inno.Rendering.RenderSurfaceHandle.operator ==(Inno.Rendering.RenderSurfaceHandle left, Inno.Rendering.RenderSurfaceHandle right)`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L537) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-24433264624ae6b5"></a>[`public static bool Inno.Rendering.RenderSurfaceHandle.operator !=(Inno.Rendering.RenderSurfaceHandle left, Inno.Rendering.RenderSurfaceHandle right)`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L537) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-e1516b8426d68e44"></a>[`public override string Inno.Rendering.RenderSurfaceHandle.ToString()`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L537) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-315fad1ade7a051b"></a>[`public bool Inno.Rendering.RenderSurfaceHandle.isValid { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L553) | Gets whether the handle identifies a presentation surface. | — | — |
| <a id="api-c2334adaa5f43966"></a>[`public readonly record struct Inno.Rendering.RenderSurfaceHandle : System.IEquatable<Inno.Rendering.RenderSurfaceHandle>`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L537) | Identifies a persistent presentation surface without exposing a backend framebuffer or swapchain handle. | — | — |

### `Inno.Rendering.RenderTarget`

<!-- api: T:Inno.Rendering.RenderTarget d5126d5fa71cc529a14041b35e06dc621ceffbde4f031d536a9a5c5b0e982df8 -->
<!-- api: P:Inno.Rendering.RenderTarget.backbuffer b9c0ab0a8a91b5457ff6868a3c0d8aad5ecae158c6334209fa3b3adc06177376 -->
<!-- api: P:Inno.Rendering.RenderTarget.kind 7a18c57807e8ecfabe00481e7cbccf2f6aad4b1edd75de0582d60e9359d9238b -->
<!-- api: P:Inno.Rendering.RenderTarget.texture 43b0f901a1deef57b1095c41a02f6d0fcce3a2de8784b450d0283606c5b2d77e -->
<!-- api: M:Inno.Rendering.RenderTarget.FromTexture(Inno.Rendering.RenderTexture) 1e5a577d437a914d6ec7c34699e3f01c99795f22fe29725704fa1ea380f9b6a7 -->
<!-- api: M:Inno.Rendering.RenderTarget.ToString b7ba07620988ff6a7c17beebf5f3070d2f8bd15bce3050f44e975e2085e8fb6b -->
<!-- api: M:Inno.Rendering.RenderTarget.op_Inequality(Inno.Rendering.RenderTarget,Inno.Rendering.RenderTarget) 5fbf7f9a0a9329da9576351f666cbdab4a1a737efd354d66be6f6a1c9cfb39ff -->
<!-- api: M:Inno.Rendering.RenderTarget.op_Equality(Inno.Rendering.RenderTarget,Inno.Rendering.RenderTarget) d6365bf782204113f80af07ed46610bb72a2367a2761bded54078b07265a5d8b -->
<!-- api: M:Inno.Rendering.RenderTarget.GetHashCode bcc9ddf4d0c407479c59469c812e461ec0bdf856d8f4b83a6621690decd38239 -->
<!-- api: M:Inno.Rendering.RenderTarget.Equals(System.Object) 42d44339486a62b7d4ab7df9f18d84cfb1a6f96b0ab69954ff0578ce80067520 -->
<!-- api: M:Inno.Rendering.RenderTarget.Equals(Inno.Rendering.RenderTarget) 79af6ce62d8d613616ba57c88880db6f5df7083c8fb777561345364bdf67b2f4 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c384920bdbbb3db6"></a>[`public bool Inno.Rendering.RenderTarget.Equals(Inno.Rendering.RenderTarget other)`](../../src/services/rendering/Inno.Rendering/Targets/RenderTarget.cs#L23) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-6699d1b15ad4f16b"></a>[`public override bool Inno.Rendering.RenderTarget.Equals(object obj)`](../../src/services/rendering/Inno.Rendering/Targets/RenderTarget.cs#L23) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-05dc4308e0a101f4"></a>[`public static Inno.Rendering.RenderTarget Inno.Rendering.RenderTarget.FromTexture(Inno.Rendering.RenderTexture texture)`](../../src/services/rendering/Inno.Rendering/Targets/RenderTarget.cs#L57) | Creates an offscreen render target.<br>返回：A target referencing texture.<br>`texture`：Persistent offscreen texture description. | — | — |
| <a id="api-f3b41b74afb63094"></a>[`public override int Inno.Rendering.RenderTarget.GetHashCode()`](../../src/services/rendering/Inno.Rendering/Targets/RenderTarget.cs#L23) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-4a86fd74c03f3d64"></a>[`public static bool Inno.Rendering.RenderTarget.operator ==(Inno.Rendering.RenderTarget left, Inno.Rendering.RenderTarget right)`](../../src/services/rendering/Inno.Rendering/Targets/RenderTarget.cs#L23) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-5d00fba3b21b7264"></a>[`public static bool Inno.Rendering.RenderTarget.operator !=(Inno.Rendering.RenderTarget left, Inno.Rendering.RenderTarget right)`](../../src/services/rendering/Inno.Rendering/Targets/RenderTarget.cs#L23) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-5826c308b864a70c"></a>[`public override string Inno.Rendering.RenderTarget.ToString()`](../../src/services/rendering/Inno.Rendering/Targets/RenderTarget.cs#L23) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-57cc5c4817ad1046"></a>[`public static Inno.Rendering.RenderTarget Inno.Rendering.RenderTarget.backbuffer { get; }`](../../src/services/rendering/Inno.Rendering/Targets/RenderTarget.cs#L36) | Gets a target representing the main application backbuffer. | — | — |
| <a id="api-ffc21e886c660b19"></a>[`public Inno.Rendering.RenderTargetKind Inno.Rendering.RenderTarget.kind { get; }`](../../src/services/rendering/Inno.Rendering/Targets/RenderTarget.cs#L41) | Gets the target kind. | — | — |
| <a id="api-63d886b2a09de16c"></a>[`public Inno.Rendering.RenderTexture? Inno.Rendering.RenderTarget.texture { get; }`](../../src/services/rendering/Inno.Rendering/Targets/RenderTarget.cs#L46) | Gets the offscreen texture, or null for the backbuffer. | — | — |
| <a id="api-07b702263e26627e"></a>[`public readonly record struct Inno.Rendering.RenderTarget : System.IEquatable<Inno.Rendering.RenderTarget>`](../../src/services/rendering/Inno.Rendering/Targets/RenderTarget.cs#L23) | Selects one render destination without exposing a swapchain or framebuffer handle. | — | — |

### `Inno.Rendering.RenderTargetKind`

<!-- api: T:Inno.Rendering.RenderTargetKind 58f90723e0f7ee3eb6f44ba2f7d2b408f5e676555f7194b321d6df539f9f37b1 -->
<!-- api: F:Inno.Rendering.RenderTargetKind.Backbuffer 2393dfab4f066f5edb1ab81477ec2cf327f69bf843ee2c3294bcd617c7493c08 -->
<!-- api: F:Inno.Rendering.RenderTargetKind.Texture f2f9c45a6693b13f29beedc356b919a736e658dec9aea1ed6701d6a1e21d9131 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f14a2a49637c6b46"></a>[`Inno.Rendering.RenderTargetKind.Backbuffer = 0`](../../src/services/rendering/Inno.Rendering/Targets/RenderTarget.cs#L13) | Main application window swapchain. | — | — |
| <a id="api-503fa6268f5275cc"></a>[`Inno.Rendering.RenderTargetKind.Texture = 1`](../../src/services/rendering/Inno.Rendering/Targets/RenderTarget.cs#L17) | Persistent offscreen render texture. | — | — |
| <a id="api-21e509debd1e4d99"></a>[`public enum Inno.Rendering.RenderTargetKind : int`](../../src/services/rendering/Inno.Rendering/Targets/RenderTarget.cs#L8) | Identifies whether a request renders to the main swapchain or an offscreen target. | — | — |

### `Inno.Rendering.RenderTexture`

<!-- api: T:Inno.Rendering.RenderTexture c643258a3cadc6aa75261e3bcae7e667278e5a572abf216a05e2b436c0b6628d -->
<!-- api: M:Inno.Rendering.RenderTexture.#ctor(System.String,Inno.Rendering.RenderTextureDescriptor) b50b4a52fec6ada9538962b395e05d0b29df07c9208b630c4e7b37d32da56b56 -->
<!-- api: P:Inno.Rendering.RenderTexture.name ecb4620d5e33452aaae4c56762e4664f054530bd39a13991b7389ccbb2e7fdfb -->
<!-- api: P:Inno.Rendering.RenderTexture.descriptor 20eab772f9d05832c14ac05ec62ef7480e01935a190a80513200bbcca6931080 -->
<!-- api: P:Inno.Rendering.RenderTexture.contentRevision e273dfcdd133c132808e24ecbe76fc2f7361edb94b0afce1d54ee54db64d739c -->
<!-- api: M:Inno.Rendering.RenderTexture.Resize(Inno.Rendering.RenderTextureDescriptor) 871f262c068c268f7423517b6e0fe12ba804fe1dc97e8aa3809183ad20200d67 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-ae4843f643f70ba3"></a>[`public Inno.Rendering.RenderTexture.RenderTexture(string name, Inno.Rendering.RenderTextureDescriptor descriptor)`](../../src/services/rendering/Inno.Rendering/Targets/RenderTexture.cs#L21) | Creates an offscreen render target description.<br>`name`：Artist-facing and diagnostic name.<br>`descriptor`：Initial texture requirements. | — | — |
| <a id="api-8d595d48a01c4a98"></a>[`public void Inno.Rendering.RenderTexture.Resize(Inno.Rendering.RenderTextureDescriptor descriptor)`](../../src/services/rendering/Inno.Rendering/Targets/RenderTexture.cs#L52) | Replaces texture requirements at the next render-frame safety point.<br>`descriptor`：New texture requirements. | — | — |
| <a id="api-9e49ff0f174a13c8"></a>[`public long Inno.Rendering.RenderTexture.contentRevision { get; private set; }`](../../src/services/rendering/Inno.Rendering/Targets/RenderTexture.cs#L44) | Gets a counter incremented whenever the descriptor changes. | — | — |
| <a id="api-774ee49a5d42ab72"></a>[`public Inno.Rendering.RenderTextureDescriptor Inno.Rendering.RenderTexture.descriptor { get; }`](../../src/services/rendering/Inno.Rendering/Targets/RenderTexture.cs#L39) | Gets the current texture requirements. | — | — |
| <a id="api-dc38102db1b63344"></a>[`public string Inno.Rendering.RenderTexture.name { get; }`](../../src/services/rendering/Inno.Rendering/Targets/RenderTexture.cs#L34) | Gets the artist-facing and diagnostic name. | — | — |
| <a id="api-823ac0a5420be730"></a>[`public sealed class Inno.Rendering.RenderTexture`](../../src/services/rendering/Inno.Rendering/Targets/RenderTexture.cs#L8) | Describes a persistent offscreen target without owning a backend-native handle. | — | — |

### `Inno.Rendering.RenderTextureContainer`

<!-- api: T:Inno.Rendering.RenderTextureContainer a2a5b074728038ff8c29129fa494675e61677654c6cc49febf8713633e0908eb -->
<!-- api: F:Inno.Rendering.RenderTextureContainer.Ktx 7bc74ef9d69e3ddb0ba553685bde6592f3348353709ef1745938dd006a400f4c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-bf72250bff3c28b7"></a>[`Inno.Rendering.RenderTextureContainer.Ktx = 0`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L13) | Khronos Texture container containing validated GPU texture payloads. | — | — |
| <a id="api-0e2160faf5b379e6"></a>[`public enum Inno.Rendering.RenderTextureContainer : int`](../../src/services/rendering/Inno.Rendering/Device/IRenderDevice.cs#L8) | Identifies a portable encoded texture container accepted by a render device. | — | — |

### `Inno.Rendering.RenderTextureDescriptor`

<!-- api: T:Inno.Rendering.RenderTextureDescriptor e6af959fc6ebc3a66e8d7e6ca8111b00ca6ddb1d8cdf2de19c09950957425e02 -->
<!-- api: M:Inno.Rendering.RenderTextureDescriptor.#ctor(System.Int32,System.Int32,Inno.Rendering.RenderTextureFormat,Inno.Rendering.RenderTextureUsage,System.Int32,System.Int32,System.Int32,Inno.Rendering.RenderTextureDimension,System.Int32) ecb203a29ee1c4a2296d501f628ebe3884066cef111671fc9ac36cfd1636bdb5 -->
<!-- api: P:Inno.Rendering.RenderTextureDescriptor.width 8522637584e8f700500175d0db60561c47113c1dc45fa9f0b9ace13a85ab8ada -->
<!-- api: P:Inno.Rendering.RenderTextureDescriptor.height 19baf01d180bda7cff94c996776ee65645a78ab00e3fa26148beb4ed7e63f796 -->
<!-- api: P:Inno.Rendering.RenderTextureDescriptor.format 31599836eca3258a6c976245099702e0f257edcf2bd62eb3b6c427fc9c25ef64 -->
<!-- api: P:Inno.Rendering.RenderTextureDescriptor.usage 0b4efb47a53f4e89ad83d15dd3f0b39eff543e6f252f86c57aafc1eeaa77c697 -->
<!-- api: P:Inno.Rendering.RenderTextureDescriptor.mipCount b14c55f3a69688aaab9720506b5f888fbd2c802771f079fe0e82299aab592a15 -->
<!-- api: P:Inno.Rendering.RenderTextureDescriptor.arrayLayers c89e1da6667273900996da5b3435449475321ae28a9423de0612ea6d1e85349e -->
<!-- api: P:Inno.Rendering.RenderTextureDescriptor.sampleCount caaf4137933117b9e7190f6cb00c1d1e74f1d661192f4a822a1ddf5112be48d4 -->
<!-- api: P:Inno.Rendering.RenderTextureDescriptor.dimension def2cef57167aec37065ed9a1ebeddc5f9c04cdb4a0f5cccae3a8fa5a88077a3 -->
<!-- api: P:Inno.Rendering.RenderTextureDescriptor.depth 4103b44e0b9d6135b755fca682783cdf45abc9dc1e372ca48a72f040797e3ebf -->
<!-- api: M:Inno.Rendering.RenderTextureDescriptor.GetSubresourceLayerCount(System.Int32) da0bb89de8632a2ae4d71bf24b4b9144803eb1afa80fc65a4dde05a7c6394b45 -->
<!-- api: M:Inno.Rendering.RenderTextureDescriptor.Equals(Inno.Rendering.RenderTextureDescriptor) 09adb7a4ede0ae3a8b610909d2d3faa0bab180219ca630bea35549523c9a586e -->
<!-- api: M:Inno.Rendering.RenderTextureDescriptor.Equals(System.Object) 94ed50e7b45fac3d612c4781eb450d6edeca75a794b071538b06ae5f1a8f401d -->
<!-- api: M:Inno.Rendering.RenderTextureDescriptor.GetHashCode 8b3041a4a51749fab438d4063fb1083807cdb04f45e2d38148462ef2c16c9b74 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-dc6529fee4750aa7"></a>[`public Inno.Rendering.RenderTextureDescriptor.RenderTextureDescriptor(int width, int height, Inno.Rendering.RenderTextureFormat format, Inno.Rendering.RenderTextureUsage usage, int mipCount = 1, int arrayLayers = 1, int sampleCount = 1, Inno.Rendering.RenderTextureDimension dimension = Inno.Rendering.RenderTextureDimension.Texture2D, int depth = 1)`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L182) | Creates a texture descriptor.<br>`width`：Texture width in pixels.<br>`height`：Texture height in pixels.<br>`format`：Storage format.<br>`usage`：Permitted operations.<br>`mipCount`：Number of mip levels.<br>`arrayLayers`：Number of array layers.<br>`sampleCount`：Raster sample count.<br>`dimension`：Texture dimensional shape.<br>`depth`：Base mip depth for a three-dimensional texture. | — | — |
| <a id="api-3fcbc1657706f4ed"></a>[`public bool Inno.Rendering.RenderTextureDescriptor.Equals(Inno.Rendering.RenderTextureDescriptor? other)`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L316) | Determines whether this instance and the supplied value represent the same logical state.<br>返回：true when both values represent the same logical state; otherwise, false.<br>`other`：The value to compare with this instance. | — | — |
| <a id="api-d2f1e059087ce2ef"></a>[`public override bool Inno.Rendering.RenderTextureDescriptor.Equals(object? obj)`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L337) | Determines whether this instance and the supplied value represent the same logical state.<br>返回：true when both values represent the same logical state; otherwise, false.<br>`obj`：The object to compare with this instance. | — | — |
| <a id="api-f49f14a4ac57e518"></a>[`public override int Inno.Rendering.RenderTextureDescriptor.GetHashCode()`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L345) | Computes a hash code from the fields that participate in logical equality.<br>返回：A hash code consistent with the implemented equality contract. | — | — |
| <a id="api-5011c3bfe6ce5717"></a>[`public int Inno.Rendering.RenderTextureDescriptor.GetSubresourceLayerCount(int mipLevel)`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L294) | Gets the number of independently addressable layers or slices at one mip level.<br>返回：The array-layer count for 2D textures, six faces per cubemap layer, or the mip depth for 3D textures.<br>`mipLevel`：Zero-based mip level. | — | — |
| <a id="api-ca54cbaba374a7d9"></a>[`public int Inno.Rendering.RenderTextureDescriptor.arrayLayers { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L268) | Gets the number of array layers. | — | — |
| <a id="api-b530799f87f34804"></a>[`public int Inno.Rendering.RenderTextureDescriptor.depth { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L283) | Gets the base mip depth for a three-dimensional texture. | — | — |
| <a id="api-9a5d68e9baf692b0"></a>[`public Inno.Rendering.RenderTextureDimension Inno.Rendering.RenderTextureDescriptor.dimension { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L278) | Gets the dimensional texture shape. | — | — |
| <a id="api-95286bba1dd9d19a"></a>[`public Inno.Rendering.RenderTextureFormat Inno.Rendering.RenderTextureDescriptor.format { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L253) | Gets the storage format. | — | — |
| <a id="api-cf46d1ae3a92aa05"></a>[`public int Inno.Rendering.RenderTextureDescriptor.height { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L248) | Gets the texture height in pixels. | — | — |
| <a id="api-c9bd8676a6ad544a"></a>[`public int Inno.Rendering.RenderTextureDescriptor.mipCount { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L263) | Gets the number of mip levels. | — | — |
| <a id="api-d269b932f961fb24"></a>[`public int Inno.Rendering.RenderTextureDescriptor.sampleCount { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L273) | Gets the raster sample count. | — | — |
| <a id="api-9bedd16d60303544"></a>[`public Inno.Rendering.RenderTextureUsage Inno.Rendering.RenderTextureDescriptor.usage { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L258) | Gets permitted operations. | — | — |
| <a id="api-b2ba9a04178d58e5"></a>[`public int Inno.Rendering.RenderTextureDescriptor.width { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L243) | Gets the texture width in pixels. | — | — |
| <a id="api-b8b557bb2592ecbd"></a>[`public sealed class Inno.Rendering.RenderTextureDescriptor : System.IEquatable<Inno.Rendering.RenderTextureDescriptor>`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L150) | Describes a render-graph texture independently from a graphics backend. | — | — |

### `Inno.Rendering.RenderTextureDimension`

<!-- api: T:Inno.Rendering.RenderTextureDimension c922b2063e6eaab7f0a8f2f401d07bfdefe7239e6040aa9c28f6e05decfa751c -->
<!-- api: F:Inno.Rendering.RenderTextureDimension.Texture2D 47769565b742e2955be8bd3e57b3e6af235c375c5b7685acb9add02fcc079881 -->
<!-- api: F:Inno.Rendering.RenderTextureDimension.Texture3D 1e8c36eccc72ae3628d6ae175115ee61792a6d8c51592b8a55cf3745ac65f01f -->
<!-- api: F:Inno.Rendering.RenderTextureDimension.Cube 817b58eab9a5147d993f82ebf257c9c9fc2430e44d387cbddd2d684aad16ba45 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c767ddcfd97dd03e"></a>[`Inno.Rendering.RenderTextureDimension.Cube = 2`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L68) | Cubemap texture with optional cubemap array layers. | — | — |
| <a id="api-8e8e1555d0e7a212"></a>[`Inno.Rendering.RenderTextureDimension.Texture2D = 0`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L60) | Two-dimensional texture with optional array layers. | — | — |
| <a id="api-d7e40a3fff0dd88b"></a>[`Inno.Rendering.RenderTextureDimension.Texture3D = 1`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L64) | Three-dimensional volume texture. | — | — |
| <a id="api-31ae27fd60939d8c"></a>[`public enum Inno.Rendering.RenderTextureDimension : int`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L55) | Identifies the dimensional shape of a backend-neutral texture resource. | — | — |

### `Inno.Rendering.RenderTextureFormat`

<!-- api: T:Inno.Rendering.RenderTextureFormat c3525910fbdfc76977fcb7a4f96841091c8ea43839b239be1573249407c19f11 -->
<!-- api: F:Inno.Rendering.RenderTextureFormat.R8 6e55b87c8132564012e153f9af6b9ab9bb341f7f95a8269c6863178ac99525ac -->
<!-- api: F:Inno.Rendering.RenderTextureFormat.RG8 928dbe1292f54686f8001d62b6f46d2770d82de7de352c585dd874579d2ef8c8 -->
<!-- api: F:Inno.Rendering.RenderTextureFormat.RGBA8 2530a86cb019d76a850751d52b8409f8461cb2aab455e08f38ed445cc9734faa -->
<!-- api: F:Inno.Rendering.RenderTextureFormat.RGBA8Srgb cc66dd4008bcbfecb9f4f1b11b4051f97adc06a4468020c357780ac6add1949f -->
<!-- api: F:Inno.Rendering.RenderTextureFormat.RGB10A2 86a7f9873d1c0f810c2e6b897107ca7d21c5f457c895cc9de90f970779348fb0 -->
<!-- api: F:Inno.Rendering.RenderTextureFormat.RG11B10Float 30acc106d9c2f7b93fff3c8246dbd239d2124dbc40026688e383c68855d3d865 -->
<!-- api: F:Inno.Rendering.RenderTextureFormat.RGBA16Float 983a649925aa3de1fc5351a25a03532143166c187e6ccc857a9cf69f5c675e82 -->
<!-- api: F:Inno.Rendering.RenderTextureFormat.R32Float 3808adbd779b2e0902498da841db91b82eabc908b39bd2137ed03b04f5627fb3 -->
<!-- api: F:Inno.Rendering.RenderTextureFormat.Depth24Stencil8 468631b497fb1ea6af115e557964426757c1a7e332d3fc4b907e554f7af2cb5d -->
<!-- api: F:Inno.Rendering.RenderTextureFormat.Depth32Float 49f39ee9824cb67712d8333df41dc74fc122cf85feb78b6d677c7714ea5aabfc -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-39202eb35cc0083c"></a>[`Inno.Rendering.RenderTextureFormat.Depth24Stencil8 = 8`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L45) | Twenty-four-bit depth and eight-bit stencil format. | — | — |
| <a id="api-1375910bac0bf686"></a>[`Inno.Rendering.RenderTextureFormat.Depth32Float = 9`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L49) | Thirty-two-bit floating-point depth format. | — | — |
| <a id="api-b94070f9cae375aa"></a>[`Inno.Rendering.RenderTextureFormat.R32Float = 7`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L41) | Single-channel 32-bit floating-point format. | — | — |
| <a id="api-65eec97198485e36"></a>[`Inno.Rendering.RenderTextureFormat.R8 = 0`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L13) | Eight-bit single-channel normalized format. | — | — |
| <a id="api-1c51452b158ff870"></a>[`Inno.Rendering.RenderTextureFormat.RG11B10Float = 5`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L33) | Eleven-bit RGB floating-point format. | — | — |
| <a id="api-1b9452a736321bbe"></a>[`Inno.Rendering.RenderTextureFormat.RG8 = 1`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L17) | Eight-bit two-channel normalized format. | — | — |
| <a id="api-52ec1c49c56116bd"></a>[`Inno.Rendering.RenderTextureFormat.RGB10A2 = 4`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L29) | Ten-bit RGB and two-bit alpha normalized format. | — | — |
| <a id="api-59059c52684c68f8"></a>[`Inno.Rendering.RenderTextureFormat.RGBA16Float = 6`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L37) | Half-precision four-channel floating-point format. | — | — |
| <a id="api-feb168a00c3cee32"></a>[`Inno.Rendering.RenderTextureFormat.RGBA8 = 2`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L21) | Eight-bit four-channel linear normalized format. | — | — |
| <a id="api-484c416e1eda5751"></a>[`Inno.Rendering.RenderTextureFormat.RGBA8Srgb = 3`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L25) | Eight-bit four-channel sRGB format. | — | — |
| <a id="api-88828d28d6d782aa"></a>[`public enum Inno.Rendering.RenderTextureFormat : int`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L8) | Declares backend-neutral texture storage formats. | — | — |

### `Inno.Rendering.RenderTextureHandle`

<!-- api: T:Inno.Rendering.RenderTextureHandle 01cbfc4b77c881b059c74e7e096d3fe8b22d4951f859f8b94c5f779a2a8db3ee -->
<!-- api: P:Inno.Rendering.RenderTextureHandle.isValid c7c1574afba227276e5433ca68b5773fba4acec9bee8e0db912d8b328f315a12 -->
<!-- api: M:Inno.Rendering.RenderTextureHandle.ToString 6a1b4167c160f063eb6f410fdbb4cd799f0c39f75b6f2352a20596125e23f7f6 -->
<!-- api: M:Inno.Rendering.RenderTextureHandle.op_Inequality(Inno.Rendering.RenderTextureHandle,Inno.Rendering.RenderTextureHandle) cf9ad2c677ceab99351008c3a7c8de354c9b4b1ccd8b670140152e59ccc0c680 -->
<!-- api: M:Inno.Rendering.RenderTextureHandle.op_Equality(Inno.Rendering.RenderTextureHandle,Inno.Rendering.RenderTextureHandle) 67b62ee08b6869cbc318b57377198c3d03ccb07659afb22803d95191ec794619 -->
<!-- api: M:Inno.Rendering.RenderTextureHandle.GetHashCode 4c13e1727b3fd9affec5763b5ce1872b3eb4d8b5db14697a70804d48c8c5f900 -->
<!-- api: M:Inno.Rendering.RenderTextureHandle.Equals(System.Object) aee5521cb504ba8a372a0f8df33031c70f35d8ff6c686cb14937d4e2939e02ef -->
<!-- api: M:Inno.Rendering.RenderTextureHandle.Equals(Inno.Rendering.RenderTextureHandle) 6990fe87dc1589fb09d3507e7cca1b6007f2bc454fcdaec4723110f3a7d60ee5 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d9dde3a123a823ff"></a>[`public bool Inno.Rendering.RenderTextureHandle.Equals(Inno.Rendering.RenderTextureHandle other)`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L443) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-ae1caeb30cfd7a56"></a>[`public override bool Inno.Rendering.RenderTextureHandle.Equals(object obj)`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L443) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-c2a84a03cdea37d3"></a>[`public override int Inno.Rendering.RenderTextureHandle.GetHashCode()`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L443) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-256f07f7d11cf12a"></a>[`public static bool Inno.Rendering.RenderTextureHandle.operator ==(Inno.Rendering.RenderTextureHandle left, Inno.Rendering.RenderTextureHandle right)`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L443) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-6832c901d918cf17"></a>[`public static bool Inno.Rendering.RenderTextureHandle.operator !=(Inno.Rendering.RenderTextureHandle left, Inno.Rendering.RenderTextureHandle right)`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L443) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-bfd9b3765e1db77e"></a>[`public override string Inno.Rendering.RenderTextureHandle.ToString()`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L443) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-112b179cc5e306b6"></a>[`public bool Inno.Rendering.RenderTextureHandle.isValid { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L462) | Gets whether the handle was created by a render graph. | — | — |
| <a id="api-218f8ee49fd7c3c5"></a>[`public readonly record struct Inno.Rendering.RenderTextureHandle : System.IEquatable<Inno.Rendering.RenderTextureHandle>`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L443) | Identifies a frame-scoped render-graph texture. | — | — |

### `Inno.Rendering.RenderTextureReadbackHandle`

<!-- api: T:Inno.Rendering.RenderTextureReadbackHandle f23a3017d460933ec67010c4c5001335b41ea839cf5f93ff2cb01e2c743e9082 -->
<!-- api: P:Inno.Rendering.RenderTextureReadbackHandle.isValid 743767ace250e554b1d0a34a7ef5f7a84b6a2d9f9eb95f71f1fc490d332f7fe1 -->
<!-- api: M:Inno.Rendering.RenderTextureReadbackHandle.ToString 64069218faaf9adb045cb2d02f6acf00587fa08e1a7676333a16ffc7a101e341 -->
<!-- api: M:Inno.Rendering.RenderTextureReadbackHandle.op_Inequality(Inno.Rendering.RenderTextureReadbackHandle,Inno.Rendering.RenderTextureReadbackHandle) 43d89b4537c6fe9971f0dddf620e64d73aeaa719fa478283faecad37feb07342 -->
<!-- api: M:Inno.Rendering.RenderTextureReadbackHandle.op_Equality(Inno.Rendering.RenderTextureReadbackHandle,Inno.Rendering.RenderTextureReadbackHandle) fb20f3c618f73e3f7bc214cbc0423fe87313758ac13f5fde85832b241ea86cc1 -->
<!-- api: M:Inno.Rendering.RenderTextureReadbackHandle.GetHashCode dda78bdbea257cb1582e50bf2ede4beeab5cc1b6b7c30d21b616228c78f04aad -->
<!-- api: M:Inno.Rendering.RenderTextureReadbackHandle.Equals(System.Object) 8000c624e1ed5f4ed7d617694a67b5c47addc82245a2c916e1a3a2a2d022befc -->
<!-- api: M:Inno.Rendering.RenderTextureReadbackHandle.Equals(Inno.Rendering.RenderTextureReadbackHandle) d11f7b7d8edac23935c077ce083e312c253dc5af0435c9eb77ee79c026555cd4 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-26ae34409cfa7223"></a>[`public bool Inno.Rendering.RenderTextureReadbackHandle.Equals(Inno.Rendering.RenderTextureReadbackHandle other)`](../../src/services/rendering/Inno.Rendering/Device/RenderTextureReadback.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-eef7160075cd6071"></a>[`public override bool Inno.Rendering.RenderTextureReadbackHandle.Equals(object obj)`](../../src/services/rendering/Inno.Rendering/Device/RenderTextureReadback.cs#L8) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-910a01dd30d56665"></a>[`public override int Inno.Rendering.RenderTextureReadbackHandle.GetHashCode()`](../../src/services/rendering/Inno.Rendering/Device/RenderTextureReadback.cs#L8) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-1e42e6941a7590be"></a>[`public static bool Inno.Rendering.RenderTextureReadbackHandle.operator ==(Inno.Rendering.RenderTextureReadbackHandle left, Inno.Rendering.RenderTextureReadbackHandle right)`](../../src/services/rendering/Inno.Rendering/Device/RenderTextureReadback.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-cd86b8c18048e806"></a>[`public static bool Inno.Rendering.RenderTextureReadbackHandle.operator !=(Inno.Rendering.RenderTextureReadbackHandle left, Inno.Rendering.RenderTextureReadbackHandle right)`](../../src/services/rendering/Inno.Rendering/Device/RenderTextureReadback.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-eadbdcf21892b0db"></a>[`public override string Inno.Rendering.RenderTextureReadbackHandle.ToString()`](../../src/services/rendering/Inno.Rendering/Device/RenderTextureReadback.cs#L8) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-30bbf53d29ac4a68"></a>[`public bool Inno.Rendering.RenderTextureReadbackHandle.isValid { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderTextureReadback.cs#L24) | Gets whether the handle identifies a device readback operation. | — | — |
| <a id="api-f8746eda8c8a24fd"></a>[`public readonly record struct Inno.Rendering.RenderTextureReadbackHandle : System.IEquatable<Inno.Rendering.RenderTextureReadbackHandle>`](../../src/services/rendering/Inno.Rendering/Device/RenderTextureReadback.cs#L8) | Identifies one backend-neutral asynchronous texture readback operation. | — | — |

### `Inno.Rendering.RenderTextureReadbackResult`

<!-- api: T:Inno.Rendering.RenderTextureReadbackResult 28d8f2436a30a1c28857e655dbfa6924d4f1430f6b0c5d36822135c7c0466192 -->
<!-- api: M:Inno.Rendering.RenderTextureReadbackResult.#ctor(Inno.Rendering.RenderTextureDescriptor,System.Int32,System.Int32,System.ReadOnlySpan{System.Byte}) 175065169a15ade9f82c65f9bb66cb7954f84c6ef430c6f1e6599a88c2d95019 -->
<!-- api: P:Inno.Rendering.RenderTextureReadbackResult.descriptor e7160eef0cc9057af7d91e1f7780e6496b1e19c691217a585f887d2a936928fc -->
<!-- api: P:Inno.Rendering.RenderTextureReadbackResult.mipLevel 65dcbda1ba4b9efb940cba64593373026c6b7457edf850505a9aa0be96511bb0 -->
<!-- api: P:Inno.Rendering.RenderTextureReadbackResult.rowPitch d99f28e6bbcab2b99fb60bac153f30414a390284305114fe20b7043ba28dccfb -->
<!-- api: P:Inno.Rendering.RenderTextureReadbackResult.data cb1068acc5b02c36db5a24b1d80f31fe70df5d07fbd080cf6abd06cfabba23ca -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-bf2a69f9b37b23f3"></a>[`public Inno.Rendering.RenderTextureReadbackResult.RenderTextureReadbackResult(Inno.Rendering.RenderTextureDescriptor descriptor, int mipLevel, int rowPitch, System.ReadOnlySpan<byte> data)`](../../src/services/rendering/Inno.Rendering/Device/RenderTextureReadback.cs#L49) | Creates one immutable texture readback result.<br>`descriptor`：Source texture descriptor.<br>`mipLevel`：Source mip level.<br>`rowPitch`：Number of bytes between adjacent texel rows.<br>`data`：Complete tightly packed mip bytes for every addressable layer. | — | — |
| <a id="api-84af3f2deb9a3035"></a>[`public System.ReadOnlyMemory<byte> Inno.Rendering.RenderTextureReadbackResult.data { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderTextureReadback.cs#L86) | Gets complete tightly packed mip bytes for every addressable layer. | — | — |
| <a id="api-9b8162315c088587"></a>[`public Inno.Rendering.RenderTextureDescriptor Inno.Rendering.RenderTextureReadbackResult.descriptor { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderTextureReadback.cs#L71) | Gets the source texture descriptor. | — | — |
| <a id="api-587b3975f39df6a0"></a>[`public int Inno.Rendering.RenderTextureReadbackResult.mipLevel { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderTextureReadback.cs#L76) | Gets the source mip level. | — | — |
| <a id="api-0049feb76eb62dd0"></a>[`public int Inno.Rendering.RenderTextureReadbackResult.rowPitch { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderTextureReadback.cs#L81) | Gets the byte distance between adjacent rows. | — | — |
| <a id="api-cb9fa6a3bb0cec71"></a>[`public sealed class Inno.Rendering.RenderTextureReadbackResult`](../../src/services/rendering/Inno.Rendering/Device/RenderTextureReadback.cs#L30) | Contains immutable bytes copied from one complete texture mip. | — | — |

### `Inno.Rendering.RenderTextureRegion`

<!-- api: T:Inno.Rendering.RenderTextureRegion 7697df3508ca6bfcaab9515f3919ff0d414725c76a609d8423f1ad30d3f9b6fc -->
<!-- api: M:Inno.Rendering.RenderTextureRegion.#ctor(System.Int32,System.Int32,System.Int32,System.Int32,System.Int32,System.Int32,System.Int32) 2ec0d8895bf0a2f9563efd27bd0f864d92ebbfb0606ddc6733459d4081aac233 -->
<!-- api: P:Inno.Rendering.RenderTextureRegion.mip e76f5ac8e6d54be8f91409bdb271cdbea91b8c831768c5bad62bb38db48a4bf2 -->
<!-- api: P:Inno.Rendering.RenderTextureRegion.x d9b68fbd940907569e6d522ab5986549cc359f4f608331d01f2c55a89af4246d -->
<!-- api: P:Inno.Rendering.RenderTextureRegion.y a8a5c91c8b7be54b568a895676acb46586e969cd5010cd54ee353d0c5773c17e -->
<!-- api: P:Inno.Rendering.RenderTextureRegion.layer f3e2bc1c3fbf7aacf85bcb0e10a40b9083bd3db77276859807f584ed98fb88b1 -->
<!-- api: P:Inno.Rendering.RenderTextureRegion.width 18e73d0066ccdf89088f0caa90afed3693d2abc6c2c7f27d17f65f561ea2ec4d -->
<!-- api: P:Inno.Rendering.RenderTextureRegion.height 8e3307074b7ad31d1ec08c4d487c1a9ceb93979b95616e9014c61be8c02edd51 -->
<!-- api: P:Inno.Rendering.RenderTextureRegion.depth 78320544c1f8b60671d368ce1507dcf818a984bd3ac234f835aba3595bde938a -->
<!-- api: M:Inno.Rendering.RenderTextureRegion.ToString 047ec5fa60db8afa643126128f3a5fe5aafb1ff617dbaf687199de017030fe28 -->
<!-- api: M:Inno.Rendering.RenderTextureRegion.op_Inequality(Inno.Rendering.RenderTextureRegion,Inno.Rendering.RenderTextureRegion) 7dc7a0b3a9927d4742072952f611227ac33b65c4ea8eefce5d49507cb831eef7 -->
<!-- api: M:Inno.Rendering.RenderTextureRegion.op_Equality(Inno.Rendering.RenderTextureRegion,Inno.Rendering.RenderTextureRegion) 89d8b8efefc5a0a51a7d3a501d49e2cc6d9c19ba762a051355173d0f5c63ffd7 -->
<!-- api: M:Inno.Rendering.RenderTextureRegion.GetHashCode d135e27b33cdf46194422c2a59f9602a211cae228575244c3c2c3febce9692e9 -->
<!-- api: M:Inno.Rendering.RenderTextureRegion.Equals(System.Object) 5933382a8b13230ab8d9f2d2289544ce297c13ef5d8f81a3cca691afa3a8167f -->
<!-- api: M:Inno.Rendering.RenderTextureRegion.Equals(Inno.Rendering.RenderTextureRegion) eb5602c448224650f6c6a8136802704a36316560553388b9bf83917bec343f4e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f02208f2629ce25c"></a>[`public Inno.Rendering.RenderTextureRegion.RenderTextureRegion(int mip, int x, int y, int layer, int width, int height, int depth = 1)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L106) | Creates a texture subresource box.<br>`mip`：Zero-based mip level.<br>`x`：Horizontal texel origin.<br>`y`：Vertical texel origin.<br>`layer`：Array layer or depth-slice origin.<br>`width`：Positive texel width.<br>`height`：Positive texel height.<br>`depth`：Positive layer or depth-slice count. | — | — |
| <a id="api-af01e8cbac8b7213"></a>[`public bool Inno.Rendering.RenderTextureRegion.Equals(Inno.Rendering.RenderTextureRegion other)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L80) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-701faa4918facd99"></a>[`public override bool Inno.Rendering.RenderTextureRegion.Equals(object obj)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L80) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-29a74af6a25d0789"></a>[`public override int Inno.Rendering.RenderTextureRegion.GetHashCode()`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L80) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-693bf7c85bb09f90"></a>[`public static bool Inno.Rendering.RenderTextureRegion.operator ==(Inno.Rendering.RenderTextureRegion left, Inno.Rendering.RenderTextureRegion right)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L80) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-de711042e1f89f30"></a>[`public static bool Inno.Rendering.RenderTextureRegion.operator !=(Inno.Rendering.RenderTextureRegion left, Inno.Rendering.RenderTextureRegion right)`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L80) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-0517fd7b2e491ea2"></a>[`public override string Inno.Rendering.RenderTextureRegion.ToString()`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L80) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-cac4dabe01cc384f"></a>[`public int Inno.Rendering.RenderTextureRegion.depth { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L164) | Gets the layer or depth-slice count. | — | — |
| <a id="api-0f3e3d89305678a0"></a>[`public int Inno.Rendering.RenderTextureRegion.height { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L159) | Gets the texel height. | — | — |
| <a id="api-1c54f5aabf8d6c6c"></a>[`public int Inno.Rendering.RenderTextureRegion.layer { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L149) | Gets the array layer or depth-slice origin. | — | — |
| <a id="api-794ffeb5b32ff85e"></a>[`public int Inno.Rendering.RenderTextureRegion.mip { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L134) | Gets the zero-based mip level. | — | — |
| <a id="api-7e6f0398efd3bb1f"></a>[`public int Inno.Rendering.RenderTextureRegion.width { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L154) | Gets the texel width. | — | — |
| <a id="api-ebbf5ab68262ca30"></a>[`public int Inno.Rendering.RenderTextureRegion.x { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L139) | Gets the horizontal texel origin. | — | — |
| <a id="api-ab92292fffd92222"></a>[`public int Inno.Rendering.RenderTextureRegion.y { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L144) | Gets the vertical texel origin. | — | — |
| <a id="api-411c165c2889191f"></a>[`public readonly record struct Inno.Rendering.RenderTextureRegion : System.IEquatable<Inno.Rendering.RenderTextureRegion>`](../../src/services/rendering/Inno.Rendering/Device/RenderCommands.cs#L80) | Describes one texture subresource box for copy and blit commands. | — | — |

### `Inno.Rendering.RenderTextureUsage`

<!-- api: T:Inno.Rendering.RenderTextureUsage af3c876ba15cfdeddaf27bed6a54f4a5fd53bb2485f2ef494d4defc91de5be95 -->
<!-- api: F:Inno.Rendering.RenderTextureUsage.Sampled f64e830905121dcb50dc41f8a7bd92c101d10d39a6238a714665e084e8da1459 -->
<!-- api: F:Inno.Rendering.RenderTextureUsage.ColorAttachment 195729da9e597a1352a39eb7cb3c65a71304e6702f6d3a5b67a7467ea405b74c -->
<!-- api: F:Inno.Rendering.RenderTextureUsage.DepthStencilAttachment 40e0da4c2cf7ea92c6ffaaa8b72b978d2441e552e71476372103c7d03a1bd82d -->
<!-- api: F:Inno.Rendering.RenderTextureUsage.Storage e16a42288be7cc62403735b25f5df0ca6b2abd2fe2fff0860fd5237e98c4c9b3 -->
<!-- api: F:Inno.Rendering.RenderTextureUsage.CopySource d9283cd046f53d9c58f4bc19c5b98a76061d132bb55dc7d04dcfcbd01f6e7a28 -->
<!-- api: F:Inno.Rendering.RenderTextureUsage.CopyDestination 62e36f7ffe234938ea4efe4eeb582b4f79cfcec1f68c656ad58da247a904c15f -->
<!-- api: F:Inno.Rendering.RenderTextureUsage.Readback 722c4a79052caf68a3c85803cfee134c9fc90c505592414d61e4486e4eefda34 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3684a90f0b61cba0"></a>[`Inno.Rendering.RenderTextureUsage.ColorAttachment = 2`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L84) | Raster color attachment. | — | — |
| <a id="api-bcb4fa08d87e5c85"></a>[`Inno.Rendering.RenderTextureUsage.CopyDestination = 32`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L100) | Copy operation destination. | — | — |
| <a id="api-6671cad649e0f7cc"></a>[`Inno.Rendering.RenderTextureUsage.CopySource = 16`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L96) | Copy operation source. | — | — |
| <a id="api-2193713b2381d23f"></a>[`Inno.Rendering.RenderTextureUsage.DepthStencilAttachment = 4`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L88) | Raster depth or stencil attachment. | — | — |
| <a id="api-17897d461852b3ed"></a>[`Inno.Rendering.RenderTextureUsage.Readback = 64`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L104) | Asynchronous transfer from GPU texture memory to CPU-visible bytes. | — | — |
| <a id="api-6bfac57885d78e30"></a>[`Inno.Rendering.RenderTextureUsage.Sampled = 1`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L80) | Shader-readable texture. | — | — |
| <a id="api-e18764086922e5dd"></a>[`Inno.Rendering.RenderTextureUsage.Storage = 8`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L92) | Shader-readable and writable unordered texture. | — | — |
| <a id="api-44b96056b4638742"></a>[`public enum Inno.Rendering.RenderTextureUsage : int`](../../src/services/rendering/Inno.Rendering/Device/RenderResources.cs#L75) | Declares intended texture operations for capability and hazard validation. | — | — |

### `Inno.Rendering.RenderUniformType`

<!-- api: T:Inno.Rendering.RenderUniformType 631557d5c2e81444058315630706e6fce9d05dffe31a5165428c920e89f78745 -->
<!-- api: F:Inno.Rendering.RenderUniformType.Vector4 a1bf9c24797ff721843e866c0ada3f2b9399a7d70a57670531ad150a347c9786 -->
<!-- api: F:Inno.Rendering.RenderUniformType.Matrix3x3 ce0fc51070055745ed0c646cdfe3ae327cae58bda5bb8a506928a16bf6bf7a26 -->
<!-- api: F:Inno.Rendering.RenderUniformType.Matrix4x4 3079b968204f9d72bc337639bd8019228e8f7d9a31b1a95abb982bc1912a22bd -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4d82394b956ea825"></a>[`Inno.Rendering.RenderUniformType.Matrix3x3 = 1`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L42) | Three-by-three 32-bit floating-point matrix. | — | — |
| <a id="api-6bbc61646e298b23"></a>[`Inno.Rendering.RenderUniformType.Matrix4x4 = 2`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L46) | Four-by-four 32-bit floating-point matrix. | — | — |
| <a id="api-b205e6a7e6a90906"></a>[`Inno.Rendering.RenderUniformType.Vector4 = 0`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L38) | Four-component 32-bit floating-point vector. | — | — |
| <a id="api-519c2f2f740bffa2"></a>[`public enum Inno.Rendering.RenderUniformType : int`](../../src/services/rendering/Inno.Rendering/Device/RenderShaderBindingDescriptor.cs#L33) | Identifies the native-independent shape of one uniform binding. | — | — |

### `Inno.Rendering.RenderVertexAttribute`

<!-- api: T:Inno.Rendering.RenderVertexAttribute 14a1bc0cd206c822f348dea6c6bb541ff652e0a939a0618c26a3fa62d65626e6 -->
<!-- api: M:Inno.Rendering.RenderVertexAttribute.#ctor(Inno.Rendering.RenderVertexSemantic,Inno.Rendering.RenderVertexFormat,System.Int32) 366c6647665ae1e535b6254efb7a6b897a2be9c458e32b4eed6fc35a65705f7b -->
<!-- api: P:Inno.Rendering.RenderVertexAttribute.semantic 1cc80da6b6133a3cce98c037a7fba493515d27c89a6b8d90f2a7aa0f5c87219c -->
<!-- api: P:Inno.Rendering.RenderVertexAttribute.format b80d1d48e35a0c68a043bb46e9dff4431c3652e02d308350b02fe76c2bd69433 -->
<!-- api: P:Inno.Rendering.RenderVertexAttribute.byteOffset 7d44b08d24b7888375a125a200f4c98dab9053c4b734074543360ec96964305b -->
<!-- api: P:Inno.Rendering.RenderVertexAttribute.byteSize a04463ebaeb2a0e06b81d46a474f665bf3e1349f7ef73ed49ee236791e3d4aa9 -->
<!-- api: M:Inno.Rendering.RenderVertexAttribute.ToString f9ca8adf9e3056a95dee0f2281a8a5724a3f2075457fbe9df38f6507aac0ed25 -->
<!-- api: M:Inno.Rendering.RenderVertexAttribute.op_Inequality(Inno.Rendering.RenderVertexAttribute,Inno.Rendering.RenderVertexAttribute) 5db72ec3ccad915793e8821deebb16cf3b4b198a1511f393479948f9ac395144 -->
<!-- api: M:Inno.Rendering.RenderVertexAttribute.op_Equality(Inno.Rendering.RenderVertexAttribute,Inno.Rendering.RenderVertexAttribute) c6699b602bf5b875692860b9cf67829785488115c744c884d40f3cef305c57c6 -->
<!-- api: M:Inno.Rendering.RenderVertexAttribute.GetHashCode 73f7743d0cfc543ceb927dc3b000b58695e3bbc7b56cbf54ee90a7af1bcf4835 -->
<!-- api: M:Inno.Rendering.RenderVertexAttribute.Equals(System.Object) fa355a93f756f5cbe3034c41a852a5afa025d3ca5b27f464cb74f6094b0f61a1 -->
<!-- api: M:Inno.Rendering.RenderVertexAttribute.Equals(Inno.Rendering.RenderVertexAttribute) 1361d321d65e1adcdc9ee1aaf069b6da13c2b72f9825dcc2725dcfb7ec24e87b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f9daa493d3fc325b"></a>[`public Inno.Rendering.RenderVertexAttribute.RenderVertexAttribute(Inno.Rendering.RenderVertexSemantic semantic, Inno.Rendering.RenderVertexFormat format, int byteOffset = -1)`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L170) | Creates a vertex attribute declaration.<br>`semantic`：Shader input semantic.<br>`format`：Packed component representation.<br>`byteOffset`：Explicit byte offset in the stream, or -1 to place the attribute directly after the preceding attribute. | — | — |
| <a id="api-94e1e317c77e4dcc"></a>[`public bool Inno.Rendering.RenderVertexAttribute.Equals(Inno.Rendering.RenderVertexAttribute other)`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L156) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-53137225db881ea3"></a>[`public override bool Inno.Rendering.RenderVertexAttribute.Equals(object obj)`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L156) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-7a66fea011ffe67c"></a>[`public override int Inno.Rendering.RenderVertexAttribute.GetHashCode()`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L156) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-dab29db0f930e67f"></a>[`public static bool Inno.Rendering.RenderVertexAttribute.operator ==(Inno.Rendering.RenderVertexAttribute left, Inno.Rendering.RenderVertexAttribute right)`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L156) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-05fbeb0f3ee4512f"></a>[`public static bool Inno.Rendering.RenderVertexAttribute.operator !=(Inno.Rendering.RenderVertexAttribute left, Inno.Rendering.RenderVertexAttribute right)`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L156) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-d4d2c773a48e1a7c"></a>[`public override string Inno.Rendering.RenderVertexAttribute.ToString()`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L156) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-f8f94a88e29256d0"></a>[`public int Inno.Rendering.RenderVertexAttribute.byteOffset { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L195) | Gets the explicit byte offset in the resolved stream layout, or -1 before a layout resolves automatic placement. | — | — |
| <a id="api-58ae893a4c4b7146"></a>[`public int Inno.Rendering.RenderVertexAttribute.byteSize { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L200) | Gets the packed byte size. | — | — |
| <a id="api-4328c980742fd6a6"></a>[`public Inno.Rendering.RenderVertexFormat Inno.Rendering.RenderVertexAttribute.format { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L190) | Gets the packed component representation. | — | — |
| <a id="api-a4d7cb374169abe1"></a>[`public Inno.Rendering.RenderVertexSemantic Inno.Rendering.RenderVertexAttribute.semantic { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L185) | Gets the shader input semantic. | — | — |
| <a id="api-3962e7a08ad8d7e8"></a>[`public readonly record struct Inno.Rendering.RenderVertexAttribute : System.IEquatable<Inno.Rendering.RenderVertexAttribute>`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L156) | Describes one vertex attribute in stream order. | — | — |

### `Inno.Rendering.RenderVertexFormat`

<!-- api: T:Inno.Rendering.RenderVertexFormat 52796eec0214f6d7dea11c8c0657c21c4a57f4932744c5353b2b3f883745e649 -->
<!-- api: F:Inno.Rendering.RenderVertexFormat.Float1 45359b2a20fc49681740d115c5da2160077a9426cbea146b5723111e7dcc2c12 -->
<!-- api: F:Inno.Rendering.RenderVertexFormat.Float2 cb111c39c4880db929381830876e50a0e1882b7d64294819f41493a0e8d40bae -->
<!-- api: F:Inno.Rendering.RenderVertexFormat.Float3 a85145980a49ecbe6e219f62f61d8eb1b87270b06467d5fc347a2f779a6be87d -->
<!-- api: F:Inno.Rendering.RenderVertexFormat.Float4 bef7339e3deb7aa84028e5283c1489fb82f4b1a0fbb7c02f28a7fe5bfa5fd435 -->
<!-- api: F:Inno.Rendering.RenderVertexFormat.Half2 5791c2378bce9f483be30b6df78fcc1d45b5a08819ea72a0255762ad8e1f9b07 -->
<!-- api: F:Inno.Rendering.RenderVertexFormat.Half4 36546c23911f21156ea203b81ec629afcd16a9c80c22aabd814ecab1735c7289 -->
<!-- api: F:Inno.Rendering.RenderVertexFormat.UInt8Normalized4 a05f8491e16fd16bf13b0c80cbe7026bdf5e2fe2d3db7ee64a18f417f87b2a97 -->
<!-- api: F:Inno.Rendering.RenderVertexFormat.UInt8Normalized2 c000071dddf2f6c620ed34f7824c49e35145d0663142de02c59210edd64611c2 -->
<!-- api: F:Inno.Rendering.RenderVertexFormat.UInt8Integer4 57e7a8f05049a72dd1d86e8d0396d073833acc0c7ebbdfda3bf79d8893422148 -->
<!-- api: F:Inno.Rendering.RenderVertexFormat.UInt8Integer2 0512fba537fc68bec16ce50dfacf69fb7801fc60120da2fd6480893b5ba57e3b -->
<!-- api: F:Inno.Rendering.RenderVertexFormat.UInt10Normalized4 ade9ab3bb597e9606f7d29830d309f00eb4058f528a5de609f0e1093ab66111d -->
<!-- api: F:Inno.Rendering.RenderVertexFormat.Int16Normalized2 919ff274bba2a241ef31284db0ef9fd61f96198864f470108e96d6fde8415cca -->
<!-- api: F:Inno.Rendering.RenderVertexFormat.Int16Normalized4 57ee0e0ebb82635ecb02f241bb584b45dd683342caa258f73c2e68ea2eed694e -->
<!-- api: F:Inno.Rendering.RenderVertexFormat.Int16Integer2 3aa1acbfcdb666ad63505a2fcf038fc4c471e52b185bd7a8469d7180c886a0ce -->
<!-- api: F:Inno.Rendering.RenderVertexFormat.Int16Integer4 8fb44802b46f30ce04ed78d0ec02fec45f22e918bcc1b6142cd3b23a889edbae -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-6f3bd9e72fe29ae5"></a>[`Inno.Rendering.RenderVertexFormat.Float1 = 0`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L94) | One 32-bit floating-point component. | — | — |
| <a id="api-bcf682ab7fed10b4"></a>[`Inno.Rendering.RenderVertexFormat.Float2 = 1`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L98) | Two 32-bit floating-point components. | — | — |
| <a id="api-7e623ec071e9db95"></a>[`Inno.Rendering.RenderVertexFormat.Float3 = 2`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L102) | Three 32-bit floating-point components. | — | — |
| <a id="api-270ecdade78a3c7a"></a>[`Inno.Rendering.RenderVertexFormat.Float4 = 3`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L106) | Four 32-bit floating-point components. | — | — |
| <a id="api-d7950cd58f428b8e"></a>[`Inno.Rendering.RenderVertexFormat.Half2 = 4`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L110) | Two 16-bit floating-point components. | — | — |
| <a id="api-9d58a120f93d0d07"></a>[`Inno.Rendering.RenderVertexFormat.Half4 = 5`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L114) | Four 16-bit floating-point components. | — | — |
| <a id="api-fc74b5d9c742767f"></a>[`Inno.Rendering.RenderVertexFormat.Int16Integer2 = 13`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L146) | Two signed 16-bit components interpreted as integers. | — | — |
| <a id="api-42642eaa0e8f875f"></a>[`Inno.Rendering.RenderVertexFormat.Int16Integer4 = 14`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L150) | Four signed 16-bit components interpreted as integers. | — | — |
| <a id="api-db3e301596d7b3b3"></a>[`Inno.Rendering.RenderVertexFormat.Int16Normalized2 = 11`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L138) | Two normalized signed 16-bit components. | — | — |
| <a id="api-8e117395d9409716"></a>[`Inno.Rendering.RenderVertexFormat.Int16Normalized4 = 12`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L142) | Four normalized signed 16-bit components. | — | — |
| <a id="api-c45faf9891906f58"></a>[`Inno.Rendering.RenderVertexFormat.UInt10Normalized4 = 10`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L134) | Four normalized unsigned components packed into 10:10:10:2 bits. | — | — |
| <a id="api-45f96a8ca928a5b2"></a>[`Inno.Rendering.RenderVertexFormat.UInt8Integer2 = 9`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L130) | Two unsigned bytes interpreted as integers. | — | — |
| <a id="api-a477680502f9c288"></a>[`Inno.Rendering.RenderVertexFormat.UInt8Integer4 = 8`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L126) | Four unsigned bytes interpreted as integers. | — | — |
| <a id="api-cffc05e43b67c49d"></a>[`Inno.Rendering.RenderVertexFormat.UInt8Normalized2 = 7`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L122) | Two normalized unsigned bytes. | — | — |
| <a id="api-cedf516b9cb5d69a"></a>[`Inno.Rendering.RenderVertexFormat.UInt8Normalized4 = 6`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L118) | Four normalized unsigned bytes. | — | — |
| <a id="api-15a98858ef746e5a"></a>[`public enum Inno.Rendering.RenderVertexFormat : int`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L89) | Identifies one packed vertex attribute representation. | — | — |

### `Inno.Rendering.RenderVertexLayout`

<!-- api: T:Inno.Rendering.RenderVertexLayout a70bc9568732b67bc41314a92556bd9f0fe2e5b1fd93b3b2f0cf95e59dc48210 -->
<!-- api: M:Inno.Rendering.RenderVertexLayout.#ctor(System.Collections.Generic.IReadOnlyList{Inno.Rendering.RenderVertexAttribute},System.Int32) dc652d14918a90fd71b405e6c762e8dd3dc536833dee326c078f39ca662a3307 -->
<!-- api: P:Inno.Rendering.RenderVertexLayout.attributes 23b3cdd3b361ef300c6ab84b4d01e3a4ad1ec1bb1d571e2c6dc4ba91a8e1c672 -->
<!-- api: P:Inno.Rendering.RenderVertexLayout.stride 2cd8a94329e8a91a77e2290da7ce551decc70cc5ae0444cf7d23879111aeb1ee -->
<!-- api: M:Inno.Rendering.RenderVertexLayout.Equals(Inno.Rendering.RenderVertexLayout) baf86083e899952acff3dcde46a524712d485ec93b9134884292ae85855810ec -->
<!-- api: M:Inno.Rendering.RenderVertexLayout.Equals(System.Object) 6b2c4cf58b16e46d8f32ac4eb0f490bd6b1b683025e82946886eaa36f00e8e35 -->
<!-- api: M:Inno.Rendering.RenderVertexLayout.GetHashCode 75e6bba5b7d1bc49a4fb43f29a8dca8dd76b0d41d01f2dd7444df9b72f044ffc -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-09c8c51098872a24"></a>[`public Inno.Rendering.RenderVertexLayout.RenderVertexLayout(System.Collections.Generic.IReadOnlyList<Inno.Rendering.RenderVertexAttribute> attributes, int stride = 0)`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L238) | Creates an interleaved vertex layout.<br>`attributes`：Unique attributes in ascending byte order. Attributes with offset -1 are packed after the preceding attribute.<br>`stride`：Explicit positive stream stride, or zero to use the end of the final attribute. A larger stride preserves trailing application-defined padding. | — | — |
| <a id="api-fdbd92ad75f0e4f9"></a>[`public bool Inno.Rendering.RenderVertexLayout.Equals(Inno.Rendering.RenderVertexLayout? other)`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L301) | Determines whether this instance and the supplied value represent the same logical state.<br>返回：true when both values represent the same logical state; otherwise, false.<br>`other`：The value to compare with this instance. | — | — |
| <a id="api-c7a523fecece9942"></a>[`public override bool Inno.Rendering.RenderVertexLayout.Equals(object? obj)`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L315) | Determines whether this instance and the supplied value represent the same logical state.<br>返回：true when both values represent the same logical state; otherwise, false.<br>`obj`：The object to compare with this instance. | — | — |
| <a id="api-8a8cb5613d562566"></a>[`public override int Inno.Rendering.RenderVertexLayout.GetHashCode()`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L323) | Computes a hash code from the fields that participate in logical equality.<br>返回：A hash code consistent with the implemented equality contract. | — | — |
| <a id="api-6cb5941e1866a4cd"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.RenderVertexAttribute> Inno.Rendering.RenderVertexLayout.attributes { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L285) | Gets attributes in byte-stream order. | — | — |
| <a id="api-5e28979201c7108c"></a>[`public int Inno.Rendering.RenderVertexLayout.stride { get; }`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L290) | Gets the interleaved vertex stride in bytes. | — | — |
| <a id="api-5ffb69ad67b8d2bc"></a>[`public sealed class Inno.Rendering.RenderVertexLayout : System.IEquatable<Inno.Rendering.RenderVertexLayout>`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L224) | Defines one interleaved vertex stream independently from a graphics backend. | — | — |

### `Inno.Rendering.RenderVertexSemantic`

<!-- api: T:Inno.Rendering.RenderVertexSemantic 97a1820b04b770d6e2ad2cac24eb377d3f38a942133d72525f19d9c068f31dc6 -->
<!-- api: F:Inno.Rendering.RenderVertexSemantic.Position 678199a97401d72120423e3c4f9202113d08d84cb2cb5cfbdef9df346195731c -->
<!-- api: F:Inno.Rendering.RenderVertexSemantic.Normal c1413184cca1b7cb37cbf60a0e0894dbdcda5c44ff38722653c358a0e3d1f4ed -->
<!-- api: F:Inno.Rendering.RenderVertexSemantic.Tangent dfa87d3be2ea8d041e2475f93c34924a5bb6de9ef15b176904dc552753665cd2 -->
<!-- api: F:Inno.Rendering.RenderVertexSemantic.Bitangent 3d0b0db44c96ef79f6168f6004454c2dbe06a5c201ebdad5068856e4ab8a1ff6 -->
<!-- api: F:Inno.Rendering.RenderVertexSemantic.Color0 35abf342056a0a6232343c27b55457e725e9eed3c2e8f0fdbfac4306cc9efa14 -->
<!-- api: F:Inno.Rendering.RenderVertexSemantic.Color1 fe4036a5bb848afd2b712ae843afdbcf232248780a6de9c8262da6abdd20a210 -->
<!-- api: F:Inno.Rendering.RenderVertexSemantic.Color2 861d5af1d92d9fad0c64d0a28048b6f409e02362aafd737bc5a86c5385c4b15d -->
<!-- api: F:Inno.Rendering.RenderVertexSemantic.Color3 1a860b7e0eb069ad16dd08c483231aa0ef9838b34706351286d783a2529243c7 -->
<!-- api: F:Inno.Rendering.RenderVertexSemantic.TextureCoordinate0 16956dfbe89b791db7b7bc8bd0d7fa4b954d1ce3b2e4aef8b146b6130c41089d -->
<!-- api: F:Inno.Rendering.RenderVertexSemantic.TextureCoordinate1 6d84691c6bcc14a52924e1f75343a35face22565865fa9ad7cbbdcc278f1a70b -->
<!-- api: F:Inno.Rendering.RenderVertexSemantic.TextureCoordinate2 a4bf5b8bedfb442df28b87fefb0fe64e5c7d45a91394bb636604c6dfcb8641db -->
<!-- api: F:Inno.Rendering.RenderVertexSemantic.TextureCoordinate3 63bb9235a31c3043135468231f3a9f16138c53d8e60f879a6e8d0acf886e3ecb -->
<!-- api: F:Inno.Rendering.RenderVertexSemantic.TextureCoordinate4 37593dfffba7935deff678b279c2e26f269cab00a44d554e11c45a79b118f178 -->
<!-- api: F:Inno.Rendering.RenderVertexSemantic.TextureCoordinate5 8196d44034cf990c54d6670dcce6e54afeee2bfcdf8987c8a7ac4ebe92e4b21c -->
<!-- api: F:Inno.Rendering.RenderVertexSemantic.TextureCoordinate6 bc1b46221385c7043cfc8adccb34658a53f966ba2bb5dc6d813686dfd2825bb0 -->
<!-- api: F:Inno.Rendering.RenderVertexSemantic.TextureCoordinate7 cf9c810caa9fbd87442498ae73bc682b3a0baa05ff6afea1f7b08492a5c227eb -->
<!-- api: F:Inno.Rendering.RenderVertexSemantic.BlendIndices 5a4880e7dd1649470c897a85b79913dc2c523e75c720e0cb6fcbf9ead1b8c025 -->
<!-- api: F:Inno.Rendering.RenderVertexSemantic.BlendWeights 8aa9605fc2541427de9ce58ea396d60cc5045b2564ddacddd2f5f2b6dfe93b2a -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-876416d1b10eaa47"></a>[`Inno.Rendering.RenderVertexSemantic.Bitangent = 3`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L27) | Object-space bitangent. | — | — |
| <a id="api-1ae1f8a76d34e4ac"></a>[`Inno.Rendering.RenderVertexSemantic.BlendIndices = 16`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L79) | Skinning indices. | — | — |
| <a id="api-b97060253cf342a4"></a>[`Inno.Rendering.RenderVertexSemantic.BlendWeights = 17`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L83) | Skinning weights. | — | — |
| <a id="api-7f8f8cad6c7f5387"></a>[`Inno.Rendering.RenderVertexSemantic.Color0 = 4`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L31) | Primary vertex color. | — | — |
| <a id="api-9406a0d11455edf4"></a>[`Inno.Rendering.RenderVertexSemantic.Color1 = 5`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L35) | Secondary vertex color. | — | — |
| <a id="api-bca9db15b022dd5a"></a>[`Inno.Rendering.RenderVertexSemantic.Color2 = 6`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L39) | Third vertex color channel. | — | — |
| <a id="api-a9ab53ac59d92f9e"></a>[`Inno.Rendering.RenderVertexSemantic.Color3 = 7`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L43) | Fourth vertex color channel. | — | — |
| <a id="api-5cd25b29246979ca"></a>[`Inno.Rendering.RenderVertexSemantic.Normal = 1`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L19) | Object-space normal. | — | — |
| <a id="api-a637ddda121f5d78"></a>[`Inno.Rendering.RenderVertexSemantic.Position = 0`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L15) | Object-space position. | — | — |
| <a id="api-bba8af8fd294b59f"></a>[`Inno.Rendering.RenderVertexSemantic.Tangent = 2`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L23) | Object-space tangent and handedness. | — | — |
| <a id="api-71dfda01dd462e96"></a>[`Inno.Rendering.RenderVertexSemantic.TextureCoordinate0 = 8`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L47) | Primary texture coordinate. | — | — |
| <a id="api-9afcf32ee0c101fc"></a>[`Inno.Rendering.RenderVertexSemantic.TextureCoordinate1 = 9`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L51) | Secondary texture coordinate. | — | — |
| <a id="api-2d250a09d796ea47"></a>[`Inno.Rendering.RenderVertexSemantic.TextureCoordinate2 = 10`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L55) | Third texture coordinate. | — | — |
| <a id="api-3d8eed54c0924adc"></a>[`Inno.Rendering.RenderVertexSemantic.TextureCoordinate3 = 11`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L59) | Fourth texture coordinate. | — | — |
| <a id="api-fa0fdbd2c9f6e735"></a>[`Inno.Rendering.RenderVertexSemantic.TextureCoordinate4 = 12`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L63) | Fifth texture coordinate. | — | — |
| <a id="api-b405ac70d1115208"></a>[`Inno.Rendering.RenderVertexSemantic.TextureCoordinate5 = 13`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L67) | Sixth texture coordinate. | — | — |
| <a id="api-cb12cf31dac06cda"></a>[`Inno.Rendering.RenderVertexSemantic.TextureCoordinate6 = 14`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L71) | Seventh texture coordinate. | — | — |
| <a id="api-0f3379d80366f833"></a>[`Inno.Rendering.RenderVertexSemantic.TextureCoordinate7 = 15`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L75) | Eighth texture coordinate. | — | — |
| <a id="api-1fd69a21bfc743da"></a>[`public enum Inno.Rendering.RenderVertexSemantic : int`](../../src/services/rendering/Inno.Rendering/Device/RenderVertexLayout.cs#L10) | Identifies one backend-neutral vertex attribute semantic. | — | — |

### `Inno.Rendering.RenderViewport`

<!-- api: T:Inno.Rendering.RenderViewport 327289cf2a187e9e7f75a5fbd5ee2086ad535653ccb1d4aa58f6166d07e527ce -->
<!-- api: M:Inno.Rendering.RenderViewport.#ctor(System.Int32,System.Int32,System.Int32,System.Int32) c2dce93b3ef0f7468e00d001c5ff68054db73cffcc30c00c418f915a0a3639cc -->
<!-- api: P:Inno.Rendering.RenderViewport.x 4af7fefc11d20b2027c2b7b772c057109fc372d8c4e8d893fcb8d6e6b04cc728 -->
<!-- api: P:Inno.Rendering.RenderViewport.y 88020337822f15092aca87e534bc42c169b1d2b05266a58d7006ff1be55e5054 -->
<!-- api: P:Inno.Rendering.RenderViewport.width 43179876dd85d21388bcaffe7e2388baa18dca8c87f658c02b1be8eab198480a -->
<!-- api: P:Inno.Rendering.RenderViewport.height 0daba231c39108fb9a1c1aa91a42d4585b8188680b7acb45b1bbfc181485eb28 -->
<!-- api: M:Inno.Rendering.RenderViewport.ToString 6262c8089ec26ee5a151b6c5492a8196527abfd5316eba155268be28507ae1ea -->
<!-- api: M:Inno.Rendering.RenderViewport.op_Inequality(Inno.Rendering.RenderViewport,Inno.Rendering.RenderViewport) 58dc6cfeb7da6a2fb1ad3f600a631c1d02e682d2d4a1d4745b9b00cefc0053f9 -->
<!-- api: M:Inno.Rendering.RenderViewport.op_Equality(Inno.Rendering.RenderViewport,Inno.Rendering.RenderViewport) 6ccca674edc8b72f78a785a0ae795ab6b80f2b762c72cc0b8129015654c32d16 -->
<!-- api: M:Inno.Rendering.RenderViewport.GetHashCode e0642b512e00dea27a9534889e57439d32dc3c8c735829aa8478287740b4f1c7 -->
<!-- api: M:Inno.Rendering.RenderViewport.Equals(System.Object) f8814ea7c01b9e8285001f8ee34906665ddaf7b86b453386ee14571155687243 -->
<!-- api: M:Inno.Rendering.RenderViewport.Equals(Inno.Rendering.RenderViewport) 8f5409cb3af36bbc0cd10aaa79b00127d1bb2b4f9b2fe53cc5525ad8acce2fa0 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-136ab2122fa6eeb3"></a>[`public Inno.Rendering.RenderViewport.RenderViewport(int x, int y, int width, int height)`](../../src/services/rendering/Inno.Rendering/Views/RenderViewport.cs#L26) | Creates a render viewport.<br>`x`：Left pixel offset in the destination.<br>`y`：Top pixel offset in the destination.<br>`width`：Positive viewport width.<br>`height`：Positive viewport height. | — | — |
| <a id="api-b5d72e9629416264"></a>[`public bool Inno.Rendering.RenderViewport.Equals(Inno.Rendering.RenderViewport other)`](../../src/services/rendering/Inno.Rendering/Views/RenderViewport.cs#L9) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-a48a9de9ad0cf91c"></a>[`public override bool Inno.Rendering.RenderViewport.Equals(object obj)`](../../src/services/rendering/Inno.Rendering/Views/RenderViewport.cs#L9) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-fa8f2e36ecec0317"></a>[`public override int Inno.Rendering.RenderViewport.GetHashCode()`](../../src/services/rendering/Inno.Rendering/Views/RenderViewport.cs#L9) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-c9184822398a8a82"></a>[`public static bool Inno.Rendering.RenderViewport.operator ==(Inno.Rendering.RenderViewport left, Inno.Rendering.RenderViewport right)`](../../src/services/rendering/Inno.Rendering/Views/RenderViewport.cs#L9) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-6af9ce782f16c340"></a>[`public static bool Inno.Rendering.RenderViewport.operator !=(Inno.Rendering.RenderViewport left, Inno.Rendering.RenderViewport right)`](../../src/services/rendering/Inno.Rendering/Views/RenderViewport.cs#L9) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-3151db6b24c40ec4"></a>[`public override string Inno.Rendering.RenderViewport.ToString()`](../../src/services/rendering/Inno.Rendering/Views/RenderViewport.cs#L9) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-7d9eaca4f45def17"></a>[`public int Inno.Rendering.RenderViewport.height { get; }`](../../src/services/rendering/Inno.Rendering/Views/RenderViewport.cs#L60) | Gets the viewport height. | — | — |
| <a id="api-f0af76a49da1d694"></a>[`public int Inno.Rendering.RenderViewport.width { get; }`](../../src/services/rendering/Inno.Rendering/Views/RenderViewport.cs#L55) | Gets the viewport width. | — | — |
| <a id="api-20f2840864d248f2"></a>[`public int Inno.Rendering.RenderViewport.x { get; }`](../../src/services/rendering/Inno.Rendering/Views/RenderViewport.cs#L45) | Gets the left pixel offset. | — | — |
| <a id="api-baaa2c2122d14e94"></a>[`public int Inno.Rendering.RenderViewport.y { get; }`](../../src/services/rendering/Inno.Rendering/Views/RenderViewport.cs#L50) | Gets the top pixel offset. | — | — |
| <a id="api-c6c6bbeac7b9630d"></a>[`public readonly record struct Inno.Rendering.RenderViewport : System.IEquatable<Inno.Rendering.RenderViewport>`](../../src/services/rendering/Inno.Rendering/Views/RenderViewport.cs#L9) | Defines a destination pixel rectangle without assuming producer or rendering semantics. | — | — |

### `Inno.Rendering.RenderViewTransform`

<!-- api: T:Inno.Rendering.RenderViewTransform 365123e51ea90a4d9a60c451c8029057ec89ce8208644b2e6ded703d21fee26c -->
<!-- api: M:Inno.Rendering.RenderViewTransform.#ctor(System.ReadOnlySpan{System.Single},System.ReadOnlySpan{System.Single}) d37fec825f4a14ca83472040d9608641eb02357b65fa18bc4521e57835bceb75 -->
<!-- api: P:Inno.Rendering.RenderViewTransform.viewMatrix f4b22df6af04460c056e2419ee39df95a46f523ad4c574c2b439a34d35a04f0d -->
<!-- api: P:Inno.Rendering.RenderViewTransform.projectionMatrix e82de85babbcaa8b63b68142d0beb277647c717a801844934b645c15de05e7f9 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-544004cc9a32ff9b"></a>[`public Inno.Rendering.RenderViewTransform.RenderViewTransform(System.ReadOnlySpan<float> viewMatrix, System.ReadOnlySpan<float> projectionMatrix)`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L26) | Creates a raster view transform.<br>`viewMatrix`：Exactly sixteen column-major world-to-view values.<br>`projectionMatrix`：Exactly sixteen column-major projection values. | — | — |
| <a id="api-93e3646f4ef9e2da"></a>[`public System.ReadOnlyMemory<float> Inno.Rendering.RenderViewTransform.projectionMatrix { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L54) | Gets the immutable column-major projection matrix. | — | — |
| <a id="api-7e30c77edacbd4b6"></a>[`public System.ReadOnlyMemory<float> Inno.Rendering.RenderViewTransform.viewMatrix { get; }`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L49) | Gets the immutable column-major view matrix. | — | — |
| <a id="api-87ee9bcf4541bf10"></a>[`public sealed class Inno.Rendering.RenderViewTransform`](../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L12) | Stores backend-ready column-major transform matrices for one programmable view. | — | — |

### `Inno.Rendering.ShaderInterface`

<!-- api: T:Inno.Rendering.ShaderInterface 4d5878c736123930ce66aa320d0552fc2cd7b00892d219d6fe71b4a48ffc333b -->
<!-- api: M:Inno.Rendering.ShaderInterface.#ctor(System.Collections.Generic.IReadOnlyList{Inno.Rendering.ShaderInterfaceBinding}) 64e2e14cced7f71e58503f7c37ada93aba59a59b556eb4bd18226dc693695e3a -->
<!-- api: P:Inno.Rendering.ShaderInterface.bindings ef2a9df3502f09a9f1678598b055cf46f93e77f46cf60bfbcc78a0559576d9f4 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2763838bab4a7c61"></a>[`public Inno.Rendering.ShaderInterface.ShaderInterface(System.Collections.Generic.IReadOnlyList<Inno.Rendering.ShaderInterfaceBinding> bindings)`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderInterface.cs#L18) | Creates a shader interface contract.<br>`bindings`：Stable expected bindings. | — | — |
| <a id="api-6759b5c89282c586"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.ShaderInterfaceBinding> Inno.Rendering.ShaderInterface.bindings { get; }`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderInterface.cs#L27) | Gets stable expected bindings. | — | — |
| <a id="api-408a27bfdf1fa1dc"></a>[`public sealed class Inno.Rendering.ShaderInterface`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderInterface.cs#L10) | Stores the manifest-derived binding contract verified after backend program creation. | — | — |

### `Inno.Rendering.ShaderInterfaceBinding`

<!-- api: T:Inno.Rendering.ShaderInterfaceBinding 8b361bd85a4f9ee22ccdb22a0edf520ee8580afbd437420710238593c0f3538f -->
<!-- api: M:Inno.Rendering.ShaderInterfaceBinding.#ctor(Inno.Rendering.ShaderPropertyId,Inno.Rendering.ShaderPropertyType,Inno.Rendering.ShaderStage,System.Int32,Inno.Rendering.ShaderPropertyBindingKind,Inno.Rendering.RenderStorageAccess,System.String,System.Nullable{System.Int32}) 6159447ab0e95a29481692070fb55a0744e23fc260886ee57fdaa433c9ca965b -->
<!-- api: P:Inno.Rendering.ShaderInterfaceBinding.id 3e7851cf115da1b066be2ddc8ace5ebcc29ac0c560d145dbb4d2063398fd57c9 -->
<!-- api: P:Inno.Rendering.ShaderInterfaceBinding.type 8f219f550871d053e9b6d3d5efb04712bddd6058c741a8f994cee64a45a2f5e1 -->
<!-- api: P:Inno.Rendering.ShaderInterfaceBinding.stages 8f7e8e46273e9a66b8d82f0e46a1d30e8eadc561f9c6dce9b6ff3178ddc5da3b -->
<!-- api: P:Inno.Rendering.ShaderInterfaceBinding.arrayCount 790e2eae8d4cbd2afd93c4871208994ec0bb5302d5ba926ce0b0d1628a27097c -->
<!-- api: P:Inno.Rendering.ShaderInterfaceBinding.bindingKind 87189d70d8f38624236b79fad4cad0285c53fb264093d0f2731668b781bd0c33 -->
<!-- api: P:Inno.Rendering.ShaderInterfaceBinding.storageAccess 1531ffbbed01effbb3965f6b5f23e627e8d6c7383177fc65bf7d796ae25a539d -->
<!-- api: P:Inno.Rendering.ShaderInterfaceBinding.nativeName 9116da7c7fcd7887f78a13ab09a9eac5c49b5a0daf089f185302a15ce89036f2 -->
<!-- api: P:Inno.Rendering.ShaderInterfaceBinding.location e87e8a38bb0a65d623402d0484248f0be75d24e856bb6532ed2bfbafbac94442 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-cd9861614fb8d8d1"></a>[`public Inno.Rendering.ShaderInterfaceBinding.ShaderInterfaceBinding(Inno.Rendering.ShaderPropertyId id, Inno.Rendering.ShaderPropertyType type, Inno.Rendering.ShaderStage stages, int arrayCount = 1, Inno.Rendering.ShaderPropertyBindingKind bindingKind = Inno.Rendering.ShaderPropertyBindingKind.Uniform, Inno.Rendering.RenderStorageAccess storageAccess = Inno.Rendering.RenderStorageAccess.Read, string? nativeName = null, int? location = null)`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderInterfaceBinding.cs#L39) | Creates a reflected interface binding.<br>`id`：Stable property ID.<br>`type`：Expected value or resource type.<br>`stages`：Stages that consume the binding.<br>`arrayCount`：Required array element count.<br>`bindingKind`：Backend-neutral interface binding domain.<br>`storageAccess`：Required access for storage resources.<br>`nativeName`：Adapter-generated reflected symbol, or null when the logical ID is also the symbol.<br>`location`：Explicit resource slot, or null when the caller assigns an ordered layout. | — | — |
| <a id="api-f2c76fb455cfc638"></a>[`public int Inno.Rendering.ShaderInterfaceBinding.arrayCount { get; }`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderInterfaceBinding.cs#L85) | Gets the required array element count. | — | — |
| <a id="api-921bafa68ba1fea5"></a>[`public Inno.Rendering.ShaderPropertyBindingKind Inno.Rendering.ShaderInterfaceBinding.bindingKind { get; }`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderInterfaceBinding.cs#L90) | Gets the backend-neutral interface binding domain. | — | — |
| <a id="api-971651446b1675ed"></a>[`public Inno.Rendering.ShaderPropertyId Inno.Rendering.ShaderInterfaceBinding.id { get; }`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderInterfaceBinding.cs#L70) | Gets the stable property ID. | — | — |
| <a id="api-0f71ffb3713fc0b9"></a>[`public int? Inno.Rendering.ShaderInterfaceBinding.location { get; }`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderInterfaceBinding.cs#L104) | Gets an explicitly compiled resource slot, or null for a caller-assigned layout. | — | — |
| <a id="api-328ac9d9cc01bf18"></a>[`public string Inno.Rendering.ShaderInterfaceBinding.nativeName { get; }`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderInterfaceBinding.cs#L100) | Gets the exact reflected symbol emitted by the adapter, distinct from the logical property ID. | — | — |
| <a id="api-338f006d5cdde17c"></a>[`public Inno.Rendering.ShaderStage Inno.Rendering.ShaderInterfaceBinding.stages { get; }`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderInterfaceBinding.cs#L80) | Gets stages that consume the binding. | — | — |
| <a id="api-ca858e78db33a575"></a>[`public Inno.Rendering.RenderStorageAccess Inno.Rendering.ShaderInterfaceBinding.storageAccess { get; }`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderInterfaceBinding.cs#L95) | Gets required access for storage resources. | — | — |
| <a id="api-04a26c7b26e1ded1"></a>[`public Inno.Rendering.ShaderPropertyType Inno.Rendering.ShaderInterfaceBinding.type { get; }`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderInterfaceBinding.cs#L75) | Gets the expected value or resource type. | — | — |
| <a id="api-38e14519b5ba12b5"></a>[`public sealed class Inno.Rendering.ShaderInterfaceBinding`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderInterfaceBinding.cs#L10) | Describes one reflected material binding expected by compiled programs. | — | — |

### `Inno.Rendering.ShaderProgramKind`

<!-- api: T:Inno.Rendering.ShaderProgramKind 17f43cdb4830c7c1d4f11d16cb3b1bf343b01b60538871838921dc623826702d -->
<!-- api: F:Inno.Rendering.ShaderProgramKind.Raster 77fb25541e6dbd1ae47cb9f775c22b85955a5eda14e29323adb4bd36464b9aa9 -->
<!-- api: F:Inno.Rendering.ShaderProgramKind.Compute 1ec6184c28f01cc5a2e81a77e173d0cd84ca351ad11511a52c35cac5fcbe9371 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3d3b413a402b7bb1"></a>[`Inno.Rendering.ShaderProgramKind.Compute = 1`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderProgramKind.cs#L23) | A compute stage used by a compute pass. | — | — |
| <a id="api-85b0fb865ecedd0f"></a>[`Inno.Rendering.ShaderProgramKind.Raster = 0`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderProgramKind.cs#L19) | Vertex and fragment stages used by a raster pass. | — | — |
| <a id="api-1ed5fadeddd4ab87"></a>[`public enum Inno.Rendering.ShaderProgramKind : int`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderProgramKind.cs#L14) | Selects the programmable stage combination of a pass. | — | — |

### `Inno.Rendering.ShaderPropertyBindingKind`

<!-- api: T:Inno.Rendering.ShaderPropertyBindingKind cca4cadfcbe8a69e7c9ca2afbf28b51e625313fd15db8e3c1b234ef4e95f4025 -->
<!-- api: F:Inno.Rendering.ShaderPropertyBindingKind.Uniform 5545148ab19cebddac8fac6e1f488e2fdeae85e7d9c443c94012e356acab318e -->
<!-- api: F:Inno.Rendering.ShaderPropertyBindingKind.SampledTexture aef27821563f31ae3714d62e1368447e1360fa69fd4cfcde91fe3f369e3fdf97 -->
<!-- api: F:Inno.Rendering.ShaderPropertyBindingKind.StorageTexture a3786d4f2e8268a547e6827a60ed20a14b424d579aa0a34e3a314d64a2cf9ac9 -->
<!-- api: F:Inno.Rendering.ShaderPropertyBindingKind.StorageBuffer 5fcf7e187318a83bf6623da289e6bbc25f55bf798b30a2c7cd510f873ad86ab3 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-0887b3a481e48b34"></a>[`Inno.Rendering.ShaderPropertyBindingKind.SampledTexture = 1`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyBindingKind.cs#L23) | Texture sampled through an explicit material sampler. | — | — |
| <a id="api-95ddbcef98895998"></a>[`Inno.Rendering.ShaderPropertyBindingKind.StorageBuffer = 3`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyBindingKind.cs#L31) | Buffer bound for unordered shader access by a Pipeline. | — | — |
| <a id="api-3be13f6978fddf40"></a>[`Inno.Rendering.ShaderPropertyBindingKind.StorageTexture = 2`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyBindingKind.cs#L27) | Texture bound for unordered shader access by a Pipeline. | — | — |
| <a id="api-b6e7085d189eec73"></a>[`Inno.Rendering.ShaderPropertyBindingKind.Uniform = 0`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyBindingKind.cs#L19) | Vector or matrix uniform data. | — | — |
| <a id="api-cbcc2cf91ffecd84"></a>[`public enum Inno.Rendering.ShaderPropertyBindingKind : int`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyBindingKind.cs#L14) | Defines how one shader property enters the backend-neutral resource interface. | — | — |

### `Inno.Rendering.ShaderPropertyId`

<!-- api: T:Inno.Rendering.ShaderPropertyId f982010a3c56e958b878e5ef2e5bd17c5442c97e122b883833d6a9ddda42ebc6 -->
<!-- api: M:Inno.Rendering.ShaderPropertyId.#ctor(System.String) aa677a1ef7eaf84456ebf812360dfd3c1ae6df6790fc4d2066ca9115b5a6c8f3 -->
<!-- api: P:Inno.Rendering.ShaderPropertyId.value ddcbc1d03d22abf48add484c7ab6feaae442997b5ff173c3476ca96ea0755297 -->
<!-- api: P:Inno.Rendering.ShaderPropertyId.isValid bf1bc79674582000838443f876c3560842c19b4479488d2e33c2f5bc236acd12 -->
<!-- api: M:Inno.Rendering.ShaderPropertyId.ToString 09a0c926e23c0df08a09b1acd49b183a680310098ba082338fd450f25c99aa5f -->
<!-- api: M:Inno.Rendering.ShaderPropertyId.op_Inequality(Inno.Rendering.ShaderPropertyId,Inno.Rendering.ShaderPropertyId) 7e0c8ea0ef1a45baf88dec8b90316611b47b36d15b0140148a79714e9e6b67d2 -->
<!-- api: M:Inno.Rendering.ShaderPropertyId.op_Equality(Inno.Rendering.ShaderPropertyId,Inno.Rendering.ShaderPropertyId) f35470d7186ecdfd6a975c4bbb9aab6d593452ca2eb66319a45cb2ca4fd5cd7b -->
<!-- api: M:Inno.Rendering.ShaderPropertyId.GetHashCode 555b00835563cba6ed749b508d1614fd90f91d503e5980d289903aedfcd5d8da -->
<!-- api: M:Inno.Rendering.ShaderPropertyId.Equals(System.Object) fa80c2eb4215e599fe9198b0ddacfe3113c56adcfdcc357262baf1e8807b7960 -->
<!-- api: M:Inno.Rendering.ShaderPropertyId.Equals(Inno.Rendering.ShaderPropertyId) d10b6c53e6816bb4cdc3229fda0fcf7e4a2e6c11a6a2aa1fcf423b85ec5267fc -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-828f3d6a40f022bb"></a>[`public Inno.Rendering.ShaderPropertyId.ShaderPropertyId(string value)`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyId.cs#L22) | Creates a stable shader property identifier.<br>`value`：Stable manifest property identifier. | — | — |
| <a id="api-43cb7e2f7f621245"></a>[`public readonly bool Inno.Rendering.ShaderPropertyId.Equals(Inno.Rendering.ShaderPropertyId other)`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyId.cs#L14) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-29fdeaea9e92deec"></a>[`public override readonly bool Inno.Rendering.ShaderPropertyId.Equals(object obj)`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyId.cs#L14) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-649dc02686db27e7"></a>[`public override readonly int Inno.Rendering.ShaderPropertyId.GetHashCode()`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyId.cs#L14) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-109fd5bf46dbbd19"></a>[`public static bool Inno.Rendering.ShaderPropertyId.operator ==(Inno.Rendering.ShaderPropertyId left, Inno.Rendering.ShaderPropertyId right)`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyId.cs#L14) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-cc7b36b4dbdbf104"></a>[`public static bool Inno.Rendering.ShaderPropertyId.operator !=(Inno.Rendering.ShaderPropertyId left, Inno.Rendering.ShaderPropertyId right)`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyId.cs#L14) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-3414b74e1bef3421"></a>[`public override readonly string Inno.Rendering.ShaderPropertyId.ToString()`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyId.cs#L44) | Formats this value as a human-readable representation.<br>返回：The human-readable representation of this value. | — | — |
| <a id="api-02081a4543253391"></a>[`public readonly bool Inno.Rendering.ShaderPropertyId.isValid { get; }`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyId.cs#L36) | Gets whether this identifier has a usable value. | — | — |
| <a id="api-748299ddc4fadb51"></a>[`public string Inno.Rendering.ShaderPropertyId.value { get; set; }`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyId.cs#L31) | Gets or sets the stable manifest property identifier. | — | — |
| <a id="api-70db2c21f1373a20"></a>[`public record struct Inno.Rendering.ShaderPropertyId : System.IEquatable<Inno.Rendering.ShaderPropertyId>`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyId.cs#L14) | Identifies a material property using a stable serialized string. | — | — |

### `Inno.Rendering.ShaderPropertyType`

<!-- api: T:Inno.Rendering.ShaderPropertyType c56af6fb845c8a78c0e38f0584c5e0db45a64c644a843a8be93cabc0c837330a -->
<!-- api: F:Inno.Rendering.ShaderPropertyType.Float c8d686022047d11b810288553241c7098711e8aa9d02a980890843eb2ef274d6 -->
<!-- api: F:Inno.Rendering.ShaderPropertyType.Vector2 7bcae18f9818f4985dc7cb83c998806c6e092ebf8446652364455237eb532b50 -->
<!-- api: F:Inno.Rendering.ShaderPropertyType.Vector3 8e744adc3a21c08de71e8a5b69cea862b867d8db5cca3ae8158ead365aa956ed -->
<!-- api: F:Inno.Rendering.ShaderPropertyType.Vector4 5bd7bb6fc045f9f895c2533221d5b21438c0093a013a70f9f4f84e958202f840 -->
<!-- api: F:Inno.Rendering.ShaderPropertyType.Color 373a3f02387e3cd2ab4113d04e98d29d733b551f2ed6b61b695251a57ddb813e -->
<!-- api: F:Inno.Rendering.ShaderPropertyType.Matrix4x4 e7ddbe0c44aff80920bb58390e1a001dec4a60dd48c7252940503bddb600edce -->
<!-- api: F:Inno.Rendering.ShaderPropertyType.Texture2D 2a694231fd40cf901214be58ba7ffb93e9a4f7551b2a7bd7a3031755b4c4e883 -->
<!-- api: F:Inno.Rendering.ShaderPropertyType.Texture2DArray c88cc2e912c10bb57cd573c4c18e919341883fef5e709e85165a91f599584184 -->
<!-- api: F:Inno.Rendering.ShaderPropertyType.Texture3D ad337600e2be3a2f513477f2cd1ff8403e3b9b0f765c85f02e206f343f55dd9f -->
<!-- api: F:Inno.Rendering.ShaderPropertyType.TextureCube cd613c3a00167706b5560ec1603b75cfe3ab219f0e87b8c2de784b8f045691bb -->
<!-- api: F:Inno.Rendering.ShaderPropertyType.Buffer 8f5d772cba6c31a9df246d35b63512b9642c30e2e9a893a8e8d1014095bbf049 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-068b4509fa1c4575"></a>[`Inno.Rendering.ShaderPropertyType.Buffer = 10`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyType.cs#L59) | Read-only or read-write buffer. | — | — |
| <a id="api-39138ca907cd0787"></a>[`Inno.Rendering.ShaderPropertyType.Color = 4`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyType.cs#L35) | Linear RGBA color. | — | — |
| <a id="api-8a12d7318d2b53fe"></a>[`Inno.Rendering.ShaderPropertyType.Float = 0`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyType.cs#L19) | Scalar floating-point value. | — | — |
| <a id="api-adab3375934b939f"></a>[`Inno.Rendering.ShaderPropertyType.Matrix4x4 = 5`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyType.cs#L39) | Four-by-four matrix. | — | — |
| <a id="api-b9246ee6669e0a7d"></a>[`Inno.Rendering.ShaderPropertyType.Texture2D = 6`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyType.cs#L43) | Two-dimensional texture. | — | — |
| <a id="api-8ca10832a16447e8"></a>[`Inno.Rendering.ShaderPropertyType.Texture2DArray = 7`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyType.cs#L47) | Layered two-dimensional texture. | — | — |
| <a id="api-c471d52a3a6c6014"></a>[`Inno.Rendering.ShaderPropertyType.Texture3D = 8`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyType.cs#L51) | Three-dimensional volume texture. | — | — |
| <a id="api-07b750961685c77e"></a>[`Inno.Rendering.ShaderPropertyType.TextureCube = 9`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyType.cs#L55) | Cube texture. | — | — |
| <a id="api-f2e2d93d2442f6c0"></a>[`Inno.Rendering.ShaderPropertyType.Vector2 = 1`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyType.cs#L23) | Two-component floating-point vector. | — | — |
| <a id="api-dfaebeecad7db4ff"></a>[`Inno.Rendering.ShaderPropertyType.Vector3 = 2`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyType.cs#L27) | Three-component floating-point vector. | — | — |
| <a id="api-2fdf07dff1847803"></a>[`Inno.Rendering.ShaderPropertyType.Vector4 = 3`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyType.cs#L31) | Four-component floating-point vector. | — | — |
| <a id="api-a66d4cb8c6064530"></a>[`public enum Inno.Rendering.ShaderPropertyType : int`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderPropertyType.cs#L14) | Identifies an artist-facing shader property type. | — | — |

### `Inno.Rendering.ShaderStage`

<!-- api: T:Inno.Rendering.ShaderStage b20a744e112f4f3c246363264e116495430a6add71ea02721222e8029d79ad12 -->
<!-- api: F:Inno.Rendering.ShaderStage.None f850e7c497a0652ae40c0f5dc27d56e7c67af0cf37d68a20882aee5335eb188c -->
<!-- api: F:Inno.Rendering.ShaderStage.Vertex dc72d83a594d8c6d42a9b117da704fcdff36f46c64eabf0377bf7f9461bdabce -->
<!-- api: F:Inno.Rendering.ShaderStage.Fragment 37c77c2df6af8a06ea49b7cd357ce3e6354ad8b5e95efb8dda3ceb9d64436266 -->
<!-- api: F:Inno.Rendering.ShaderStage.Compute 97da6674d1267b5f3d7ca1007b1be71fecb23c86a144f510a79a8bf8f45d1673 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-67b34ba276ddf80b"></a>[`Inno.Rendering.ShaderStage.Compute = 4`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderStage.cs#L32) | Compute shader stage. | — | — |
| <a id="api-7f10c9acd52ac057"></a>[`Inno.Rendering.ShaderStage.Fragment = 2`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderStage.cs#L28) | Fragment shader stage. | — | — |
| <a id="api-fab8eec2ef26d8c3"></a>[`Inno.Rendering.ShaderStage.None = 0`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderStage.cs#L20) | No shader stage. | — | — |
| <a id="api-a74253767c65ca18"></a>[`Inno.Rendering.ShaderStage.Vertex = 1`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderStage.cs#L24) | Vertex shader stage. | — | — |
| <a id="api-d62d08735d03f574"></a>[`public enum Inno.Rendering.ShaderStage : int`](../../src/services/rendering/Inno.Rendering/Shaders/ShaderStage.cs#L15) | Identifies a programmable shader stage. | — | — |

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
