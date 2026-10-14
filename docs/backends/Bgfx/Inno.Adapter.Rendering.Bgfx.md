# Inno.Adapter.Rendering.Bgfx

## 帧同步与 framebuffer 缓存

`SetVerticalSync(bool enabled)` 更新设备 reset flags，并在下一 BeginFrame 与 backbuffer resize 合并应用；不改变 graph 或资源 generation。Framebuffer 缓存按实际 attachment（texture、mip、layer、depth/color）匹配，而不是依赖可能变化的 Request/pass 名称。原生 framebuffer handle 在 BGFX 帧推进后才回收，不能假设 destroy 调用立即释放容量。

[分类索引](README.md) · [Wiki 首页](../../README.md) · [后端中立 API](../../rendering/Inno.Rendering.md)

## 职责与边界

`Inno.Adapter.Rendering.Bgfx` 是唯一允许引用 `Inno.Native.Bgfx` 的运行时托管渲染程序集。它把 `IRenderDevice`、Compiled RenderGraph、资源描述和 `RenderCommandEncoder` 映射为 BGFX 设备、View、Encoder、Framebuffer 和延迟销毁队列。shaderc/texturec 只存在于 Build Toolchain，不进入运行时 Adapter 或 Player。

Device 接收 `IBgfxSurfaceIntegration`，不识别 Win32/Cocoa/Canvas 名单。平台运行 integration 负责验证 ABI/角色并返回 `BgfxSurfaceDescriptor`，窗口和显示句柄均借用。Browser 的 Canvas 规则只属于 Browser integration。此 SPI 不进入 Rendering Core、游戏脚本或 Player Runtime。

领域 API 使用中立 Rendering handle；backend 自身公开 SPI 可以表达必要的借用 surface。窗口型 `BgfxDeviceOptions` 必须提供 surfaceIntegration，无窗口设备不要求虚构 integration；所有接入验证先于 Native process lease。

共同 Composition/OutputTransfer Shader 的编译配置由平台 `ProductBuild.props` 通过 `InnoProductBuildProperties` 传入；本项目消费 `InnoBgfxShaderTarget` 与 `InnoBgfxShaderApis`，不维护平台或 CPU 选择表。每个 Shader/API 的候选、哈希清单与 `Outputs` 有独立 owner；共同 Task 校验源码、工具及输出内容，不能仅根据时间戳跳过。运行时只消费已嵌入的产物。

## 初始化顺序

1. 在 BGFX API thread 创建 `BgfxDevice`；进程中同时只能存在一个实例。
   传入 `IPlatformWindow` 时，初始 backbuffer 直接使用其物理 `pixelWidth`/`pixelHeight`，避免 HiDPI 窗口在首帧进行一次逻辑尺寸到 drawable 尺寸的重复 reset。
2. 每帧调用 `BeginFrame`，处理 resize 与到期资源释放。
3. 编译一个或多个 RenderGraph 后调用 `Execute`。
4. 所有 Encoder 结束后调用一次 `EndFrame`；正常帧和最终退休 drain 共用一个 Native 提交入口。
5. 在相同 API thread 调用 `Dispose`，释放存活资源并执行 `bgfx.shutdown`。

`BgfxDevice` 通过进程设备 lease 明确表达 BGFX 的真实平台约束：一个进程同一时间只能拥有一个
活动图形设备。第二次并发创建会在进入 native API 前失败；前一个设备完成 shutdown 后可以创建
下一个普通设备。Dear ImGui 的 viewport/window/renderer 路由则按 `ImGuiContext` 分区，不再依赖
一个可被后创建 Host 覆盖的全局 backend map。因此当前能力是“多个 Runtime/Host 可隔离，但同一
进程只允许一个活动图形 Host”，而不是虚假声明同进程多 GPU device 支持。

Noop 测试可启用 `forceSingleThreaded`。BGFX 的该模式是进程级一次性配置，同一进程 shutdown 后不能创建第二个单线程设备；实现会明确抛出异常，避免原生 fatal 或挂起。生产窗口后端不应开启此测试选项。

## 公开 API

| API | 语义 |
| --- | --- |
| `BgfxDeviceOptions` | 后端偏好、窗口、backbuffer、VSync/sRGB、延迟销毁帧数和 Noop 单线程测试设置 |
| `BgfxDevice` | BGFX 设备所有权、能力映射、帧边界、RenderGraph 执行、帧命令计数、KTX/普通纹理、Buffer、Program 与延迟销毁 |
| `BgfxCompositionProgramProvider` | 从适配器内嵌的目标 Shader 产物创建线性图层合成与最终 sRGB 输出传递 Pipeline 描述。 |
| `BgfxDevice.backbufferIsSrgb`、`IRenderDevice.primaryPresentationEncodesSrgb`、`WindowSurfaceIsSrgb(surface)` | 报告主 backbuffer 和有效独立窗口的真实线性 RGB→sRGB 输出传递。浏览器 WebGL 默认画布没有可用的 sRGB 写入控制，主目标报告不编码；Windows D3D11/D3D12 的附加 BGFX swapchain 使用 UNORM RTV，也不会自动编码。无效或过期的 surface 明确失败。 |

Shader 与纹理目标产物分别由 `Inno.Build.Toolchains.Bgfx` 和 `Inno.Build.Toolchains.Bgfx.Tools` 生成；这里不公开编译工具链 API。

图层合成与输出传递的 `.ishader` 源与离线编译目标产物属于本适配器；Runtime 不承担 BGFX profile 或主机架构选择。Provider 从当前适配器内嵌的产物按实际 `GraphicsApi` 精确选取唯一程序，不在运行时硬编码主机 OS/架构。Host 通过 `IRenderingBackendFactory` 取得该 Provider，多模型 route 在线性空间按原有预乘 Alpha 顺序合成。若主 backbuffer 不自动编码 sRGB，Runtime 用线性中间纹理完成全部合成，再以一次独立输出传递写入 backbuffer。没有适配器产物的目标会明确失败，不会把某个后端的二进制当成其他后端的程序。

`BgfxCapabilityMapper` 与 `BgfxCommandEncoder` 是内部实现，不属于稳定脚本契约。

Metal、D3D、Vulkan 等 BGFX renderer 不要求分别维护业务 Shader：同一 Shader IR/`.sc` 由 `BgfxShadercToolchain` 根据目标 profile 生成不同 artifact。这里的“公开底层 API”指后端中立的 Buffer、Texture、Pipeline、Draw/Dispatch 命令，不是把 BGFX handle 或 Metal API 暴露给 Plugin。

若未来完全替换 BGFX，应新增另一个 `IRenderDevice`/`IRenderGraphBackend`、资源映射和 Shader compiler backend；RenderGraph、Material、Pipeline、Plugin 与 Scene 语义无需修改。需要诚实区分的一点是：手写 `.sc` 是 BGFX shaderc 方言，虽然能跨 BGFX 的 Metal/D3D/Vulkan renderer，但新的非 BGFX 后端仍需提供 `.sc` 转换层，或要求手写 Shader 也先进入更高层 Shader IR。该成本被限制在编译/后端程序集，不会扩散到用户 Pipeline API。

## 当前实现行为

- 编译后的逻辑 Pass 按拓扑顺序映射到 BGFX View，并使用 `set_view_order` 固定执行顺序。
- Raster attachment 在 Pass 开始时组成临时 Framebuffer；离开 Graph 后进入延迟销毁队列。
- `BeginFrame` 会先重置上一帧实际使用过的 BGFX View，再处理到期销毁；直接 shutdown 也执行同一重置，从而解除 View 对 framebuffer/program 的跨帧引用。
- 阶段 Shader 在 Program 创建成功后立即把唯一剩余引用交给 Program；Pipeline 生命周期只延迟销毁 Program，由 BGFX 按后端安全顺序释放关联 Shader，避免并行维护第二套阶段资源所有权。
- 同一物理别名槽只创建一个 transient texture；跨帧纹理必须通过 `PersistentTextureHandle` 导入。
- 所有 BGFX 字符串 API 使用显式 UTF-8 字节长度，避免绑定层把负长度解释成超大拷贝。
- Noop 后端不执行无意义的 backbuffer reset，但仍更新逻辑尺寸并推进 frame。
- `CopyTexture`/`BlitTexture`、2D/3D/Cube texture、完整/局部 texture update、异步 texture readback、Program、Vertex/Index/Storage Buffer、Storage Texture、普通/Indexed/Instanced/Indirect/Procedural Draw、Dispatch、uniform 与 KTX texture container 已映射；所有外部 API 仍只使用 Rendering opaque handle。
- Storage Texture 通过 `encoder_set_image` 绑定。BGFX 的 `TextureImageRead`/`TextureImageWrite` 格式位分别映射为 Core 的 access-specific capability，`RenderStorageAccess.ReadWrite` 要求两者同时成立；graphics program 明确拒绝 BGFX 无法表达的 storage binding，compute program 才接受 StorageTexture/StorageBuffer slot。
- BGFX capability 会映射 sampled format、2D/Cube Array、3D、StorageTexture、UInt32 Index、Instancing、VertexID、Half/10:10:10:2 vertex、Alpha-to-Coverage、SwapChain 等中立 feature；通用 RenderGraph 和直接资源/命令入口都会拒绝不支持组合，Plugin 可以根据同一 snapshot 明确降级。
- BGFX `TextureReadBack` 映射为 `GraphicsCapability.TextureReadback`。Readback 资源使用 BGFX transfer flags，`read_texture` 返回的目标 frame 到达前由后端持有 unmanaged buffer；完成或取消后在 API thread 安全释放。当前契约读取完整 mip，且拒绝 multisample/attachment/storage 混用；调用方先显式 Copy/Blit 到 readback texture。
- `UpdateTextureRegion` 分别映射 2D、3D 与 Cube update API，并在进入 native call 前校验 mip texel bounds、层/face 与精确 byte count；持久 handle 和设备 generation 保持不变。
- `Draw` 不会在缺少 Vertex Buffer 时隐式转成 procedural；调用方必须使用 `DrawProcedural`。Indirect Draw 会先提交当前 Vertex/Index range，无 Vertex Buffer 时要求 ProceduralDraw capability。
- 每次直接或间接 draw/dispatch 成功交给 BGFX Encoder 后更新 `frameCounters`；`BeginFrame` 原子清零，因此 Runtime 读取的是本帧真实提交量。
- `allocationCounters` 实现通用设备诊断契约，报告当前 generation 的 transient Texture、Buffer、Framebuffer 累计分配；每项只在原生创建成功后递增。外部通过 `IRenderDevice` 或完成帧统计读取，不公开 BGFX 专属分配计数器属性。
- `SetVerticalSync` 完整实现两种幂等策略，变更与下一次 `BeginFrame` 的 resize/reset 合并；Noop 保留策略与尺寸，但不执行原生 reset。
- shaderc/profile 和 texturec 不位于通用 Assets、Runtime 或 Player；Build 在导出时生成目标产物，Player 只消费已经冻结的 KTX 与 Shader 二进制。

## 失败与资源安全

- 非 API thread、未开启帧、嵌套 Graph/Encoder、跨 generation handle 和 frame 前未结束 Encoder 都会抛出明确异常。
- GPU 资源不会在 finalizer 或 Asset 回调线程销毁；销毁请求按 BGFX 帧号延迟处理。
- Surface 的 resize 旧 framebuffer 和最终 framebuffer 属于同一退休组。RetireWindowSurface 停止后续提交，Task 在单调 managed 提交序号确认命令处理后完成；不以 deferredDestroyFrames 或 uint 帧号比较作为窗口释放信号。
- Dispose 先完成窗口 surface 退休；退休失败/超时保留设备及窗口 owner。成功后才继续资源闭包检查、native shutdown、readback 与 process lease 释放。
- 普通产品 Build/Publish 通过统一 Native Task 准备明确部署；运行时 loader 只读取部署产物，不扫描仓库缓存或参与构建。

双平台 Rendering CI 固定验证 Windows x64 与 macOS arm64 runner 架构，并在真实 Editor 冒烟日志中断言 D3D11/D3D12 或 Metal 后端、约定帧数和完整关闭；原生 BGFX 输出与 Editor boot log 会一同作为诊断 artifact 上传。

- Inno 会在设备关闭前释放 ImGui、Render Runtime、offscreen target、detached surface 与全部托管 handle，并排空延迟销毁队列。HiDPI 主窗口从首个 native frame 起使用物理 drawable 尺寸，不再通过首次 resize 修正。
- Objective-C `retainCount` 不能区分 Inno/BGFX 所有权与 Metal framework/driver 的内部 retain；BGFX 官方示例也能复现原检查的误报，见 [bkaradzic/bgfx#3642](https://github.com/bkaradzic/bgfx/issues/3642)。当前 vendor patch 保留真实 `release()`，移除这一不可证明的计数断言，并以 Inno 的确定性 managed resource closure、BGFX handle 销毁顺序和完整 native shutdown 作为可验证不变量。这不是日志过滤：真实资源表未清空会在关闭前被记录，并在 native runtime 与 process lease 已安全释放后明确抛出。macOS ARM64 Debug Editor 的 120-frame smoke 已验证无 `RefCount is`、无 BGFX Fatal 且出现 `BGFX Shutdown complete`。
- Graph 执行异常仍由 Core 的 complete-unwind 契约依次结束 Encoder 与 Graph。

## 相邻页面

- [Inno.Rendering](../../rendering/Inno.Rendering.md)：设备接口、资源描述、RenderGraph、脚本与 Pipeline 扩展 API。

## Composition provider

`BgfxRenderingBackendProvider()` 只创建注册描述，不初始化原生服务。`CreateDevice(RenderingBackendOptions) / CreateCompositionProgramProvider()` 是继承的 provider 创建扩展点，返回调用方拥有的服务。`id` 来自所属领域的内置稳定 ID；同一 provider 可在 composition 生命周期内创建独立服务，具体线程及进程 owner 约束仍由该实现执行。

## 源码归属

当前唯一源码 owner：`backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/Inno.Adapter.Rendering.Bgfx.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 提交序号与长期运行

内部资源退休、readback 等待和缓存保留采用 managed ulong 单调提交序号。BGFX 的 uint frame 仅保留为 Native 协议与公开诊断值；read_texture 返回的未来 frame 在请求时通过无符号模差转换成单调 deadline，完成和取消清理均不直接比较两个 uint frame。

BGFX 的 inline 模式会消耗进程全局启动状态；设备退出后禁止用任何线程策略在同一进程重新初始化。需要新模式时应重启宿主，不重置第三方静态字段。Native 测试通过 ProcessIsolation trait 使用独立 testhost。

窗口 surface Task 的完成仍表示渲染命令已处理、窗口使用已解除，不是通用 GPU fence。关闭流程保留 owner thread、安全点、最终 drain 和超时保护。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Adapter.Rendering.Bgfx.BgfxCompositionProgramProvider`

<!-- api: T:Inno.Adapter.Rendering.Bgfx.BgfxCompositionProgramProvider 89350e0f2759f0a09901271bfddccb5f03cd294407db0fda295b43338e46ab55 -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxCompositionProgramProvider.CreateDescriptor(Inno.Rendering.GraphicsCapabilities,Inno.Rendering.RenderVertexLayout) b5890f677944a73509fdd905eb2f0582a5cbdffeb4cfa4918d87fb0225e86e62 -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxCompositionProgramProvider.CreateOutputTransferDescriptor(Inno.Rendering.GraphicsCapabilities,Inno.Rendering.RenderVertexLayout) 22474d423ec3438549cabcbe27111f9ee2acb9a5dc22b98ffda4379a38ef4de8 -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxCompositionProgramProvider.#ctor 1d86c4e0de1e73035827832884bba48f5947fee15c5ad4780912123162e4b769 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-5aa4d92c9acff699"></a>[`public Inno.Adapter.Rendering.Bgfx.BgfxCompositionProgramProvider.BgfxCompositionProgramProvider()`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxCompositionProgramProvider.cs#L12) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-1230ca7f245588d9"></a>[`public Inno.Rendering.GraphicsPipelineDescriptor Inno.Adapter.Rendering.Bgfx.BgfxCompositionProgramProvider.CreateDescriptor(Inno.Rendering.GraphicsCapabilities capabilities, Inno.Rendering.RenderVertexLayout vertexLayout)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxCompositionProgramProvider.cs#L31) | Loads the BGFX target program matching the active device renderer.<br>返回：A validated premultiplied-alpha pipeline descriptor for the target renderer.<br>`capabilities`：The device capabilities whose backend identifies the target shader artifact.<br>`vertexLayout`：The fullscreen quad layout that the returned pipeline must accept. | — | `System.IO.InvalidDataException`：The target shader artifact is missing or has an invalid texture binding. |
| <a id="api-84811b97b2e66e1e"></a>[`public Inno.Rendering.GraphicsPipelineDescriptor Inno.Adapter.Rendering.Bgfx.BgfxCompositionProgramProvider.CreateOutputTransferDescriptor(Inno.Rendering.GraphicsCapabilities capabilities, Inno.Rendering.RenderVertexLayout vertexLayout)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxCompositionProgramProvider.cs#L48) | Loads the target program that encodes the completed linear composition for presentation.<br>返回：An opaque pipeline descriptor for the final linear-to-sRGB output transfer.<br>`capabilities`：The active graphics backend used to select the compiled shader artifact.<br>`vertexLayout`：The fullscreen quad layout accepted by the output pipeline. | — | — |
| <a id="api-20f3e88dced8c17a"></a>[`public sealed class Inno.Adapter.Rendering.Bgfx.BgfxCompositionProgramProvider : Inno.Rendering.IRenderLayerCompositionProgramProvider`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxCompositionProgramProvider.cs#L12) | Supplies the BGFX shader program used to composite model output layers. | — | — |

### `Inno.Adapter.Rendering.Bgfx.BgfxDevice`

<!-- api: T:Inno.Adapter.Rendering.Bgfx.BgfxDevice cb3c46ba198ac48d401fb5aef1ed647da6ad6597657df49b1b8d2fd9210c682b -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxDevice.#ctor(Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions) 12909cec468bc0129f1be577d7081756aefff5fef6f071672c3843a52b8cd29b -->
<!-- api: P:Inno.Adapter.Rendering.Bgfx.BgfxDevice.capabilities 41ca587bf7060c8ec884eeedea0a0a5fb02deb663dbf6e93e2dcafbcbfb53989 -->
<!-- api: P:Inno.Adapter.Rendering.Bgfx.BgfxDevice.backbufferIsSrgb df795a6242c60ccd1c57387caca400d31f2261c09f1c13be78c866841395a2e6 -->
<!-- api: P:Inno.Adapter.Rendering.Bgfx.BgfxDevice.primaryPresentationEncodesSrgb 1b67cd97f47e22a61b7776e3b53de9b5ae529b3589683ddc09b072963842a031 -->
<!-- api: P:Inno.Adapter.Rendering.Bgfx.BgfxDevice.supportsAdditionalSurfaces fd5e1488fc2dbd92e0034a49af2c3c16ba83f333a6f5a7b9f3ce4c2ec4ff4175 -->
<!-- api: P:Inno.Adapter.Rendering.Bgfx.BgfxDevice.generation a0a38924b9deb464febf2cd87c19e1e045506bcdcbb52d2bea5f4b24df207c1d -->
<!-- api: P:Inno.Adapter.Rendering.Bgfx.BgfxDevice.primaryPresentationSize 6df59745516c176852051265e963aefed7c2d086f3120271f144d8fd663b5261 -->
<!-- api: P:Inno.Adapter.Rendering.Bgfx.BgfxDevice.frameCounters 7043004c01507f5ce9f64c77ae5b0cb15c6c8c721b3767a96c98df203422b025 -->
<!-- api: P:Inno.Adapter.Rendering.Bgfx.BgfxDevice.allocationCounters 93e1ea6bfba5afd71361b234d2a55645b0325645c39ee8f03573023bf3dd3366 -->
<!-- api: P:Inno.Adapter.Rendering.Bgfx.BgfxDevice.backendFrame 2ed817fb63ea205b05cd23a47c7e8a0b0ad8444d07fabf05df49f565b7631081 -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxDevice.BeginFrame 34e7c3938cacd22469e737bdabb0d2b9dcd8cbea54aa430c4b2ee819cc86d2d1 -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxDevice.Execute(Inno.Rendering.CompiledRenderGraph,System.UInt64) 80db0b0b3c5ea51852ad865c70c874288b2b41a92d3c0c05636cb65f7dbbecca -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxDevice.EndFrame 45dae6c1e629a69570833680a0f060a5687ab87484ce5392255643478300351a -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxDevice.SetPrimaryPresentationSize(System.Nullable{Inno.Rendering.RenderPresentationSize}) c4e42e32aabbd52c49804491e832dac65d2d8426889a63aef38dd8140b0027fe -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxDevice.SetVerticalSync(System.Boolean) ab74712c2347f6dbcff4506c7dad4760eb2c3db3eca21e3f5a636f79decefaa5 -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxDevice.BeginGraph(Inno.Rendering.CompiledRenderGraph) 0c0d4dfc45787dc7cf8f3e2526f354fb902e55f006e525826ec770fdb18009d5 -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxDevice.BeginPass(Inno.Rendering.CompiledRenderPass) cbcac4ad4632b070bc96b9ec8e2b36ab07ebbc38333fb4a842dad632de3b9c3a -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxDevice.EndPass(Inno.Rendering.CompiledRenderPass) 430889614d5a23c7e1f4609bb57c77f9c9e581e23f5773a2baaef679813a0aa4 -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxDevice.EndGraph(Inno.Rendering.CompiledRenderGraph) ba0ce533434fd93c5175698b5738aff1eac3b8822677ed6eb13eeef96223df39 -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxDevice.BeginTextureReadback(Inno.Rendering.PersistentTextureHandle,System.Int32) 9614d9bb806ce4188bf26d447b24e00d50d1c76f0abc63e8bcd83066b4a2df49 -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxDevice.TryGetTextureReadback(Inno.Rendering.RenderTextureReadbackHandle,Inno.Rendering.RenderTextureReadbackResult@) a258beaac5b94401dc8892feabb873bcac6268e351ddb910ec0a3308f24c0712 -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxDevice.CancelTextureReadback(Inno.Rendering.RenderTextureReadbackHandle) 111ef28499c629874ed48fc372bef239df243f82a99427bc34a0226a5688d7fb -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxDevice.CreateBuffer(Inno.Rendering.PersistentBufferDescriptor,System.ReadOnlySpan{System.Byte},System.String) 296b3f86360796b16e1e40090332109f0e012accfe4bf1c2894108559aff32ec -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxDevice.DestroyBuffer(Inno.Rendering.PersistentBufferHandle) d7327da169e08e9d07aed1df038919c8ab7b18b4726a73af4f186317d98f6e7e -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxDevice.UpdateBuffer(Inno.Rendering.PersistentBufferHandle,System.ReadOnlySpan{System.Byte},System.Int32) be03ff14400a9bc08d0c6ed550e2906d6aa907a76cba8777bdd37e67682c4ae5 -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxDevice.CreateGraphicsPipeline(Inno.Rendering.GraphicsPipelineDescriptor,System.String) 25775612df829b6222efb0dd2623cc9f3391ccd9edecf23c8782f15af934495d -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxDevice.DestroyGraphicsPipeline(Inno.Rendering.GraphicsPipelineHandle) cf9812e2be0474da97a295d38763960d209a202c70b54f5d3f9c6fb626b2839c -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxDevice.CreateComputePipeline(Inno.Rendering.ComputePipelineDescriptor,System.String) 888385fbb612f184ffd7cb96a6d79a96b1f4f119267f4e46040080f90c58e22c -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxDevice.DestroyComputePipeline(Inno.Rendering.ComputePipelineHandle) 10fde0b72cf7e769e9592a34aac1475118dc912d8ceb735bfdcc61601839caf9 -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxDevice.Dispose fa22e54cca64dab213aebf20a0d0ebb8bcf3bbc4194442189109411b8d147589 -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxDevice.CreateWindowSurface(Inno.Adapter.Platform.PlatformNativeHandles,System.Int32,System.Int32,System.String) a557533c9fca16cededabf1d9d577462ed3dc5de158afaf2c59a88ceabd4e691 -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxDevice.WindowSurfaceIsSrgb(Inno.Rendering.RenderSurfaceHandle) 0f366a7048ef912a5653f41224d8f71363c0737609872f34ea57b0cf0f27ddc1 -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxDevice.ResizeWindowSurface(Inno.Rendering.RenderSurfaceHandle,System.Int32,System.Int32) 466db8ce339d1cfa2ec5c455236dbd8655fbe427c2ad34957a0ae31038bf667c -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxDevice.RetireWindowSurface(Inno.Rendering.RenderSurfaceHandle) 2b00d1203bd601c192844217573197e11ed4267bd663cf84e2b5f7d4a12f1a08 -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxDevice.DrainWindowSurfaceRetirements 5f87e93163630dbc84bdd87dddd95260d31176d6a372e7e4a605662c458f8a9b -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxDevice.CreateTexture(Inno.Rendering.RenderTextureDescriptor,System.String) 5f885cdc846b013136447fbddfe09cd36636607d5f7f3bd6c840a1a7aeeda3b1 -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxDevice.CreateTexture(Inno.Rendering.RenderTextureContainer,System.ReadOnlySpan{System.Byte},System.Boolean,System.String) d3ca5f1fc99764e3877a3c9744f5d7787e2f754e42261742c4b79089df0d111d -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxDevice.UpdateTexture(Inno.Rendering.PersistentTextureHandle,System.ReadOnlySpan{System.Byte},System.Int32,System.Int32) 82bde877a36cf9354e772d259a55ae4e70f7f49cad6d847d09ff5891ff307b96 -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxDevice.UpdateTextureRegion(Inno.Rendering.PersistentTextureHandle,Inno.Rendering.RenderTextureRegion,System.ReadOnlySpan{System.Byte}) b5dcc4b8b9e6eea4c9a7ccdc00a48489e19d1526683b73b284629a60db59de6d -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxDevice.DestroyTexture(Inno.Rendering.PersistentTextureHandle) e575118ebd1f278a6bbacb5e0e3b0eef6184a5f9848f43b9f81ce8da1d2b104a -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-45b7390e2b0eae9b"></a>[`public Inno.Adapter.Rendering.Bgfx.BgfxDevice.BgfxDevice(Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions options)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.cs#L74) | Initializes BGFX and captures immutable device capabilities.<br>`options`：Backend-neutral initialization options. | — | `System.InvalidOperationException`：Thrown when another BGFX device is active or initialization fails.<br>`System.NotSupportedException`：The selected window does not expose the native surface SPI required by BGFX. |
| <a id="api-3523dcdb82bf259f"></a>[`public void Inno.Adapter.Rendering.Bgfx.BgfxDevice.BeginFrame()`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Frames.cs#L19) | Begins a frame-scoped operation and makes queued work visible. | — | — |
| <a id="api-c89b099e64fb688e"></a>[`public void Inno.Adapter.Rendering.Bgfx.BgfxDevice.BeginGraph(Inno.Rendering.CompiledRenderGraph graph)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Graphs.cs#L22) | Begins recording commands for one validated compiled render graph.<br>`graph`：The graph consumed by begin graph; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-2a85e9f70c46cd2f"></a>[`public Inno.Rendering.RenderCommandEncoder Inno.Adapter.Rendering.Bgfx.BgfxDevice.BeginPass(Inno.Rendering.CompiledRenderPass pass)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Graphs.cs#L120) | Begins recording commands for one compiled render pass.<br>返回：The validated render command encoder that represents the completed operation.<br>`pass`：The pass consumed by begin pass; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-68fbfd8ae67b2fe8"></a>[`public Inno.Rendering.RenderTextureReadbackHandle Inno.Adapter.Rendering.Bgfx.BgfxDevice.BeginTextureReadback(Inno.Rendering.PersistentTextureHandle texture, int mipLevel = 0)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Readback.cs#L28) | Schedules asynchronous texture readback into caller-provided destination storage.<br>返回：The validated render texture readback handle that represents the completed operation.<br>`texture`：The texture consumed by begin texture readback; ownership remains with the caller unless explicitly stated otherwise.<br>`mipLevel`：The mip level consumed by begin texture readback; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-b1316bdf5d906631"></a>[`public void Inno.Adapter.Rendering.Bgfx.BgfxDevice.CancelTextureReadback(Inno.Rendering.RenderTextureReadbackHandle readback)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Readback.cs#L124) | Cancels a pending texture readback and releases its retained state.<br>`readback`：The readback consumed by cancel texture readback; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-37bba9ec74d5f38e"></a>[`public Inno.Rendering.PersistentBufferHandle Inno.Adapter.Rendering.Bgfx.BgfxDevice.CreateBuffer(Inno.Rendering.PersistentBufferDescriptor descriptor, System.ReadOnlySpan<byte> initialData, string name)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Resources.cs#L38) | Creates a buffer using this implementation's validated inputs.<br>返回：The validated persistent buffer handle that represents the completed operation.<br>`descriptor`：The descriptor consumed by create buffer; ownership remains with the caller unless explicitly stated otherwise.<br>`initialData`：The initial data consumed by create buffer; ownership remains with the caller unless explicitly stated otherwise.<br>`name`：The human-readable name used for presentation and diagnostics. | — | — |
| <a id="api-3ba8954825e99a77"></a>[`public Inno.Rendering.ComputePipelineHandle Inno.Adapter.Rendering.Bgfx.BgfxDevice.CreateComputePipeline(Inno.Rendering.ComputePipelineDescriptor descriptor, string name)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Resources.cs#L203) | Creates a compute pipeline using this implementation's validated inputs.<br>返回：The validated compute pipeline handle that represents the completed operation.<br>`descriptor`：The descriptor consumed by create compute pipeline; ownership remains with the caller unless explicitly stated otherwise.<br>`name`：The human-readable name used for presentation and diagnostics. | — | — |
| <a id="api-4d6287219773d59f"></a>[`public Inno.Rendering.GraphicsPipelineHandle Inno.Adapter.Rendering.Bgfx.BgfxDevice.CreateGraphicsPipeline(Inno.Rendering.GraphicsPipelineDescriptor descriptor, string name)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Resources.cs#L156) | Creates a graphics pipeline using this implementation's validated inputs.<br>返回：The validated graphics pipeline handle that represents the completed operation.<br>`descriptor`：The descriptor consumed by create graphics pipeline; ownership remains with the caller unless explicitly stated otherwise.<br>`name`：The human-readable name used for presentation and diagnostics. | — | — |
| <a id="api-1a8204a37cbd5939"></a>[`public Inno.Rendering.PersistentTextureHandle Inno.Adapter.Rendering.Bgfx.BgfxDevice.CreateTexture(Inno.Rendering.RenderTextureContainer container, System.ReadOnlySpan<byte> data, bool sRgb, string name)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Textures.cs#L66) | Creates a texture using this implementation's validated inputs.<br>返回：The validated persistent texture handle that represents the completed operation.<br>`container`：The container consumed by create texture; ownership remains with the caller unless explicitly stated otherwise.<br>`data`：The complete immutable byte payload consumed by this operation.<br>`sRgb`：Whether s rgb behavior is enabled while create texture executes.<br>`name`：The human-readable name used for presentation and diagnostics. | — | — |
| <a id="api-9f6024a7112992e0"></a>[`public Inno.Rendering.PersistentTextureHandle Inno.Adapter.Rendering.Bgfx.BgfxDevice.CreateTexture(Inno.Rendering.RenderTextureDescriptor descriptor, string name)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Textures.cs#L28) | Creates a texture using this implementation's validated inputs.<br>返回：The validated persistent texture handle that represents the completed operation.<br>`descriptor`：The descriptor consumed by create texture; ownership remains with the caller unless explicitly stated otherwise.<br>`name`：The human-readable name used for presentation and diagnostics. | — | — |
| <a id="api-49dd7643995a2ceb"></a>[`public Inno.Rendering.RenderSurfaceHandle Inno.Adapter.Rendering.Bgfx.BgfxDevice.CreateWindowSurface(Inno.Adapter.Platform.PlatformNativeHandles nativeHandles, int width, int height, string name)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Surfaces.cs#L43) | Creates a detached-window presentation surface at a frame safety point.<br>返回：An opaque surface handle consumable by Inno.Rendering.RasterPassBuilder.UseSurface(Inno.Rendering.RenderSurfaceHandle).<br>`nativeHandles`：Platform window handles supplied by the active window backend.<br>`width`：Drawable width in pixels.<br>`height`：Drawable height in pixels.<br>`name`：Debug and diagnostic name. | — | `System.NotSupportedException`：The selected integration or renderer cannot provide the requested surface.<br>`System.InvalidOperationException`：Thrown when BGFX cannot create the window framebuffer. |
| <a id="api-f8e43c7b7493ab5d"></a>[`public void Inno.Adapter.Rendering.Bgfx.BgfxDevice.DestroyBuffer(Inno.Rendering.PersistentBufferHandle buffer)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Resources.cs#L66) | Destroys the buffer after all in-flight references have retired.<br>`buffer`：The buffer consumed by destroy buffer; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-d9f24b4ce50fecc7"></a>[`public void Inno.Adapter.Rendering.Bgfx.BgfxDevice.DestroyComputePipeline(Inno.Rendering.ComputePipelineHandle pipeline)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Resources.cs#L227) | Destroys the compute pipeline after all in-flight references have retired.<br>`pipeline`：The pipeline consumed by destroy compute pipeline; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-49c695fb8d472370"></a>[`public void Inno.Adapter.Rendering.Bgfx.BgfxDevice.DestroyGraphicsPipeline(Inno.Rendering.GraphicsPipelineHandle pipeline)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Resources.cs#L175) | Destroys the graphics pipeline after all in-flight references have retired.<br>`pipeline`：The pipeline consumed by destroy graphics pipeline; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-0d7c88ae18aef322"></a>[`public void Inno.Adapter.Rendering.Bgfx.BgfxDevice.DestroyTexture(Inno.Rendering.PersistentTextureHandle texture)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Textures.cs#L307) | Destroys the texture after all in-flight references have retired.<br>`texture`：The texture consumed by destroy texture; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-8d53e98eea396824"></a>[`public void Inno.Adapter.Rendering.Bgfx.BgfxDevice.Dispose()`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Retirement.cs#L23) | Shuts down BGFX after releasing all active and queued backend resources. | — | `System.InvalidOperationException`：The caller is not on the owner thread, rendering is active, or retirement cannot complete. A failed retirement retains the device and its dependencies for diagnosis and a safe retry. |
| <a id="api-7815c5ef8a4ae4d2"></a>[`public void Inno.Adapter.Rendering.Bgfx.BgfxDevice.DrainWindowSurfaceRetirements()`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Surfaces.cs#L181) | Advances nonpresenting frames until all requested window retirements have been acknowledged. | Call only on the API thread with no open frame, graph, encoder or native UI callback. A failed drain retains the device and the borrowed native windows. | `System.InvalidOperationException`：The caller is not at a closed-frame API-thread safety point.<br>`Inno.Core.Execution.RetirementTimeoutException`：The bounded retirement deadline expired; dependent owners must remain alive. |
| <a id="api-1aa5ddcbeb5718a2"></a>[`public uint Inno.Adapter.Rendering.Bgfx.BgfxDevice.EndFrame()`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Frames.cs#L86) | Commits the current frame-scoped operation and returns its completion identity.<br>返回：The validated uint that represents the completed operation. | — | — |
| <a id="api-9024adefac3078cf"></a>[`public void Inno.Adapter.Rendering.Bgfx.BgfxDevice.EndGraph(Inno.Rendering.CompiledRenderGraph graph)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Graphs.cs#L167) | Finishes graph recording and submits its completed command stream.<br>`graph`：The graph consumed by end graph; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-382d2c00628a8809"></a>[`public void Inno.Adapter.Rendering.Bgfx.BgfxDevice.EndPass(Inno.Rendering.CompiledRenderPass pass)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Graphs.cs#L149) | Ends the active render pass and seals its recorded commands.<br>`pass`：The pass consumed by end pass; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-15d97833f8887d9e"></a>[`public void Inno.Adapter.Rendering.Bgfx.BgfxDevice.Execute(Inno.Rendering.CompiledRenderGraph graph, ulong frameIndex)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Frames.cs#L71) | Executes the prepared operation and publishes only a completed result.<br>`graph`：The graph consumed by execute; ownership remains with the caller unless explicitly stated otherwise.<br>`frameIndex`：The monotonic frame identity associated with this operation. | — | — |
| <a id="api-f7ede0cee68e9097"></a>[`public void Inno.Adapter.Rendering.Bgfx.BgfxDevice.ResizeWindowSurface(Inno.Rendering.RenderSurfaceHandle surface, int width, int height)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Surfaces.cs#L111) | Recreates a detached-window presentation surface for a new drawable extent.<br>`surface`：Surface owned by this device generation.<br>`width`：New drawable width in pixels.<br>`height`：New drawable height in pixels. | — | `System.ArgumentException`：Thrown when the surface is stale or no longer active. |
| <a id="api-35f66b8e7b6ac111"></a>[`public System.Threading.Tasks.Task Inno.Adapter.Rendering.Bgfx.BgfxDevice.RetireWindowSurface(Inno.Rendering.RenderSurfaceHandle surface)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Surfaces.cs#L156) | Stops accepting work for a surface and queues all of its framebuffer generations for retirement.<br>返回：A task completed after the render thread has processed every associated destruction command. This confirms release of the borrowed window, not a general GPU fence.<br>`surface`：The active surface owned by this device generation. | — | `System.ArgumentException`：The surface is stale or no longer active.<br>`System.InvalidOperationException`：The caller is not the API thread or a graph or encoder is active. |
| <a id="api-65561b6c2ea0a11f"></a>[`public void Inno.Adapter.Rendering.Bgfx.BgfxDevice.SetPrimaryPresentationSize(Inno.Rendering.RenderPresentationSize? size)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Frames.cs#L122) | Publishes drawable availability at the next BeginFrame without inventing a windowless output.<br>`size`：The real primary pixel extent, or null while the existing surface is unavailable. | — | `System.ArgumentException`：The assigned extent is invalid.<br>`System.NotSupportedException`：A windowless device is asked to advertise a primary output. |
| <a id="api-76372bc9cfdd611f"></a>[`public void Inno.Adapter.Rendering.Bgfx.BgfxDevice.SetVerticalSync(bool enabled)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Frames.cs#L141) | Queues an idempotent presentation policy update for the next BeginFrame reset. Noop devices retain the policy without issuing a native presentation reset.<br>`enabled`：Whether presentation should wait for display refresh. | — | — |
| <a id="api-bf66db57aeb85707"></a>[`public bool Inno.Adapter.Rendering.Bgfx.BgfxDevice.TryGetTextureReadback(Inno.Rendering.RenderTextureReadbackHandle readback, out Inno.Rendering.RenderTextureReadbackResult? result)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Readback.cs#L87) | Attempts to get texture readback without changing state when the operation cannot complete.<br>返回：true when the operation succeeds or its condition is satisfied; otherwise, false.<br>`readback`：The readback consumed by try get texture readback; ownership remains with the caller unless explicitly stated otherwise.<br>`result`：The result produced or completed by the preceding operation. | — | — |
| <a id="api-f8ef90e1eb88655d"></a>[`public void Inno.Adapter.Rendering.Bgfx.BgfxDevice.UpdateBuffer(Inno.Rendering.PersistentBufferHandle buffer, System.ReadOnlySpan<byte> data, int startElement = 0)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Resources.cs#L90) | Updates buffer state from the current authoritative inputs.<br>`buffer`：The buffer consumed by update buffer; ownership remains with the caller unless explicitly stated otherwise.<br>`data`：The complete immutable byte payload consumed by this operation.<br>`startElement`：The start element consumed by update buffer; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-fd133bdc402f5cb6"></a>[`public void Inno.Adapter.Rendering.Bgfx.BgfxDevice.UpdateTexture(Inno.Rendering.PersistentTextureHandle texture, System.ReadOnlySpan<byte> data, int mipLevel = 0, int arrayLayer = 0)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Textures.cs#L122) | Updates texture state from the current authoritative inputs.<br>`texture`：The texture consumed by update texture; ownership remains with the caller unless explicitly stated otherwise.<br>`data`：The complete immutable byte payload consumed by this operation.<br>`mipLevel`：The mip level consumed by update texture; ownership remains with the caller unless explicitly stated otherwise.<br>`arrayLayer`：The array layer consumed by update texture; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-ad130c59a3aae730"></a>[`public void Inno.Adapter.Rendering.Bgfx.BgfxDevice.UpdateTextureRegion(Inno.Rendering.PersistentTextureHandle texture, Inno.Rendering.RenderTextureRegion region, System.ReadOnlySpan<byte> data)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Textures.cs#L216) | Updates texture region state from the current authoritative inputs.<br>`texture`：The texture consumed by update texture region; ownership remains with the caller unless explicitly stated otherwise.<br>`region`：The region consumed by update texture region; ownership remains with the caller unless explicitly stated otherwise.<br>`data`：The complete immutable byte payload consumed by this operation. | — | — |
| <a id="api-a0583cb7165530d5"></a>[`public bool Inno.Adapter.Rendering.Bgfx.BgfxDevice.WindowSurfaceIsSrgb(Inno.Rendering.RenderSurfaceHandle surface)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Surfaces.cs#L88) | Reports whether the native window surface encodes linear render-target writes as sRGB.<br>返回：True for an sRGB presentation target; false when a final output transfer is required.<br>`surface`：Active detached-window surface owned by this device generation. | — | `System.ArgumentException`：The surface is stale or not active on this device. |
| <a id="api-55516e79850814d8"></a>[`public Inno.Rendering.RenderDeviceAllocationCounters? Inno.Adapter.Rendering.Bgfx.BgfxDevice.allocationCounters { get; }`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.cs#L184) | Gets API-thread allocation diagnostics for the current device generation's native transient pools. | — | — |
| <a id="api-dc8e0ae4b30a6a67"></a>[`public bool Inno.Adapter.Rendering.Bgfx.BgfxDevice.backbufferIsSrgb { get; }`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.cs#L153) | Gets whether the primary presentation surface encodes linear color as sRGB. | — | — |
| <a id="api-afcaba906ac0e1c0"></a>[`public uint Inno.Adapter.Rendering.Bgfx.BgfxDevice.backendFrame { get; }`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.cs#L198) | Gets the last frame number returned by BGFX submission. | — | — |
| <a id="api-fe7cbdbd07426c1e"></a>[`public Inno.Rendering.GraphicsCapabilities Inno.Adapter.Rendering.Bgfx.BgfxDevice.capabilities { get; }`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.cs#L148) | Gets the immutable feature and limit set reported by the active graphics backend. | — | — |
| <a id="api-52121730468c7d6d"></a>[`public Inno.Rendering.RenderDeviceFrameCounters Inno.Adapter.Rendering.Bgfx.BgfxDevice.frameCounters { get; }`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.cs#L179) | Gets the submitted and completed frame counters used for deferred retirement. | — | — |
| <a id="api-6b0eb03265cc6c82"></a>[`public uint Inno.Adapter.Rendering.Bgfx.BgfxDevice.generation { get; private set; }`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.cs#L169) | Gets the generation identity that owns this value. | — | — |
| <a id="api-68a2dc70d4c1faae"></a>[`public bool Inno.Adapter.Rendering.Bgfx.BgfxDevice.primaryPresentationEncodesSrgb { get; }`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.cs#L158) | Gets whether the primary BGFX surface automatically encodes linear RGB to sRGB. | — | — |
| <a id="api-1b9293dba41f905e"></a>[`public Inno.Rendering.RenderPresentationSize? Inno.Adapter.Rendering.Bgfx.BgfxDevice.primaryPresentationSize { get; private set; }`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.cs#L174) | Gets the current drawable pixel size of the main presentation surface. | — | — |
| <a id="api-22e946b42ed2f5db"></a>[`public bool Inno.Adapter.Rendering.Bgfx.BgfxDevice.supportsAdditionalSurfaces { get; }`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.cs#L163) | Gets whether both the selected host and active renderer permit additional window surfaces. | — | — |
| <a id="api-16095b01e921a908"></a>[`public sealed class Inno.Adapter.Rendering.Bgfx.BgfxDevice : Inno.Rendering.RenderDevice, Inno.Rendering.IRenderDevice, Inno.Rendering.IRenderGraphBackend`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.cs#L17) | Implements the sole BGFX device generation, API-thread frame boundary and graph backend. | — | — |

### `Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions`

<!-- api: T:Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions c622e70578b976a1fe120ead12e302358d76654c60340164e70071d805140a86 -->
<!-- api: P:Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions.preferredBackend 79ffd50408bc9609714d87173b8aecf48a96e067b1339c5d27e4eb4838077435 -->
<!-- api: P:Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions.window 3ae1ba401a16fd8dd3ad312d5f607527edf9f0298927ef09e92382215aa4164a -->
<!-- api: P:Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions.surfaceIntegration db14a41bd14310f8f3ae3b946d58a8037c46e8d06857b06474496e805739da3e -->
<!-- api: P:Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions.backbufferWidth 93095b9fe0364e45e723a0ac5a4c0cc26c9c294a3ef8d5f426abd0f618b1f6de -->
<!-- api: P:Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions.backbufferHeight 3ef27be3e711c654b01d10fd4461ab3559e508228c77b9a50fb26a5583b1db4e -->
<!-- api: P:Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions.verticalSync edbc43a89b5ee9160b5ecbe0cb33a72eefea8d09fe3085d0239da19c3f388a86 -->
<!-- api: P:Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions.sRgbBackbuffer 13b88974a84369ee965b68597b1bf648980b6500fd1d03fd1ec602bbe7759b0f -->
<!-- api: P:Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions.forceSingleThreaded d9f2940e0f7b859f3fd557b469f593659297b5bcdf9f32e790ed629f8cbe0cb1 -->
<!-- api: P:Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions.deferredDestroyFrames cdafa6cc814f0e9b99783280452a8da88f8d9b51acd645d5a6ab42398f4e303e -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions.#ctor 2c108a58dd4c9ed4aa5f83ffde1e2f1a39c5f50b2ae0bc2978ab9a61e8e6ddfc -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9dc4d08832ea55af"></a>[`public Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions.BgfxDeviceOptions()`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDeviceOptions.cs#L10) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-e1616ec58bbc93c7"></a>[`public int Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions.backbufferHeight { get; set; }`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDeviceOptions.cs#L46) | Gets or sets the initial backbuffer height in physical pixels when no window supplies one. | — | — |
| <a id="api-bca93c2cac04fc00"></a>[`public int Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions.backbufferWidth { get; set; }`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDeviceOptions.cs#L35) | Gets or sets the initial backbuffer width in physical pixels when no window supplies one. | — | — |
| <a id="api-1cd3d02a824bf8ac"></a>[`public int Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions.deferredDestroyFrames { get; set; }`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDeviceOptions.cs#L76) | Gets or sets the number of submitted frames before queued native destruction. | — | — |
| <a id="api-85d66c77bd588097"></a>[`public bool Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions.forceSingleThreaded { get; set; }`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDeviceOptions.cs#L71) | Gets or sets whether BGFX rendering is driven inline on the API thread. | This is intended for Noop tests and hosts that cannot provide a dedicated BGFX render thread. Production windowed hosts normally leave it disabled. | — |
| <a id="api-5e0f54f7140d0cbe"></a>[`public Inno.Rendering.GraphicsApi? Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions.preferredBackend { get; set; }`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDeviceOptions.cs#L19) | Gets or sets the preferred renderer, or null for platform default. | — | — |
| <a id="api-155b33f7324f8872"></a>[`public bool Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions.sRgbBackbuffer { get; set; }`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDeviceOptions.cs#L62) | Gets or sets whether the main backbuffer performs sRGB encoding. | — | — |
| <a id="api-d86b16d21b7d6e47"></a>[`public Inno.Adapter.Rendering.Bgfx.IBgfxSurfaceIntegration? Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions.surfaceIntegration { get; set; }`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDeviceOptions.cs#L30) | Gets or sets the borrowed immutable surface integration, required for a windowed device. A windowless device may omit it and cannot create additional windows without one. | — | — |
| <a id="api-2ecdf4508fb08702"></a>[`public bool Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions.verticalSync { get; set; }`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDeviceOptions.cs#L57) | Gets or sets whether submission waits for display synchronization. | — | — |
| <a id="api-417f42d6d68d26fe"></a>[`public Inno.Platform.IPlatformWindow? Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions.window { get; set; }`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDeviceOptions.cs#L24) | Gets or sets the platform window used as the main swapchain surface. | — | — |
| <a id="api-d6b4064888ba48be"></a>[`public sealed class Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDeviceOptions.cs#L10) | Configures BGFX initialization without exposing native BGFX structures. | — | — |

### `Inno.Adapter.Rendering.Bgfx.BgfxRenderingBackendProvider`

<!-- api: T:Inno.Adapter.Rendering.Bgfx.BgfxRenderingBackendProvider 8ab535dca9a83e145848d9d8404f3479f931dc190d95fcecfcbf9d1674d6786c -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxRenderingBackendProvider.#ctor(Inno.Adapter.Rendering.Bgfx.IBgfxSurfaceIntegration) 16f34cae284b2fc737114672c421105ca08d18b2228592dc3da99a0364b3fdbb -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxRenderingBackendProvider.CreateDevice(Inno.Adapter.Rendering.RenderingBackendOptions) fc87fc8cae50816c74aa829a4cbced6317f194f834c647f50c7a513331e1fe1c -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxRenderingBackendProvider.CreateCompositionProgramProvider 51d70af6ec2a1b93c8d68293c60ae2b99e2c8995fbf41712da51e2fe72c6276f -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-adaeede87f7b70b2"></a>[`public Inno.Adapter.Rendering.Bgfx.BgfxRenderingBackendProvider.BgfxRenderingBackendProvider(Inno.Adapter.Rendering.Bgfx.IBgfxSurfaceIntegration surfaceIntegration)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxRenderingBackendProvider.cs#L22) | Registers BGFX with an explicitly selected, borrowed host surface integration.<br>`surfaceIntegration`：Immutable host configuration shared by devices created from this provider. | — | `System.ArgumentNullException`：The integration is null. |
| <a id="api-10ad1bb650b043fb"></a>[`public override Inno.Rendering.IRenderLayerCompositionProgramProvider Inno.Adapter.Rendering.Bgfx.BgfxRenderingBackendProvider.CreateCompositionProgramProvider()`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxRenderingBackendProvider.cs#L44) | Creates the backend-owned program provider used when the host composites render layers.<br>返回：A provider compatible with devices created by this rendering implementation. | — | — |
| <a id="api-44d333250479c564"></a>[`public override Inno.Rendering.IRenderDevice Inno.Adapter.Rendering.Bgfx.BgfxRenderingBackendProvider.CreateDevice(Inno.Adapter.Rendering.RenderingBackendOptions options)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxRenderingBackendProvider.cs#L29) | Creates a caller-owned device using backend-neutral surface options.<br>返回：A new device owned by the caller.<br>`options`：Validated surface and device creation options. | — | — |
| <a id="api-b9e40a937fb56937"></a>[`public sealed class Inno.Adapter.Rendering.Bgfx.BgfxRenderingBackendProvider : Inno.Adapter.Rendering.RenderingBackendProvider`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxRenderingBackendProvider.cs#L9) | Supplies BGFX rendering devices and compatible layer composition programs. | — | — |

### `Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor`

<!-- api: T:Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor 1252350e85add49de0953bbd5e271805bc50d579e6be6a98f8a39009547be46e -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor.#ctor(System.IntPtr,System.IntPtr,System.Boolean) b3be3f67788064627bf8d4387b8d37b7508bdd5c4758236d2d602bed48e65889 -->
<!-- api: P:Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor.windowHandle 8c0a6605f81fb5fbea62d70ee892cfe1536cd21036e4b8f72b6b2b6a40f8377c -->
<!-- api: P:Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor.displayHandle bd0f88610ac327577cafc830d4a33501254bf313a5cf1ed8eb75e9d135cb038f -->
<!-- api: P:Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor.supportsSrgbReset 28e4a159fc116d85c2bc27eb106f7bfe23e2f2fb2b3dc6910f60398f5de904df -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor.ToString 9b18f05722a18aef128522ae766a2d1e9f0ca395054d9e150570a1477c739a04 -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor.op_Inequality(Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor,Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor) a643ef9199db17ef6d6d82268e0d3aabf328ee6a25e53f44dceb89e792c87c48 -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor.op_Equality(Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor,Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor) 3d0923a54aa9c46137e7901d883df395d26362526aa935de92670a377ce053bf -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor.GetHashCode aea8e55f618893b7e03d911a695622c4a3bce58a48a50ded525e509552a30b7a -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor.Equals(System.Object) 5c18129bc23bbde1e43637c71014669e422c426cf0e95942e6b386454afb8192 -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor.Equals(Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor) b0f1431011b2abf89fb342797bcc2259ac0eff16def3b81d31b20182545a6af5 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-96e05e0a2ae8bdcb"></a>[`public Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor.BgfxSurfaceDescriptor(nint windowHandle, nint displayHandle, bool supportsSrgbReset)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/Surfaces/BgfxSurfaceDescriptor.cs#L29) | Creates a validated borrowing description for a selected host integration.<br>`windowHandle`：The nonzero opaque native window value or stable canvas selector.<br>`displayHandle`：The optional display value required by the selected ABI.<br>`supportsSrgbReset`：Whether this primary surface permits BGFX's sRGB reset option. | — | `System.ArgumentException`：The window value is zero. |
| <a id="api-003e8d6d376ac300"></a>[`public bool Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor.Equals(Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor other)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/Surfaces/BgfxSurfaceDescriptor.cs#L12) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-52513a806f55f8c6"></a>[`public override bool Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor.Equals(object obj)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/Surfaces/BgfxSurfaceDescriptor.cs#L12) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-72cee18f163ff406"></a>[`public override int Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor.GetHashCode()`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/Surfaces/BgfxSurfaceDescriptor.cs#L12) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-dcd2b059ce758bce"></a>[`public static bool Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor.operator ==(Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor left, Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor right)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/Surfaces/BgfxSurfaceDescriptor.cs#L12) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-25c192dfa243b7d7"></a>[`public static bool Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor.operator !=(Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor left, Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor right)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/Surfaces/BgfxSurfaceDescriptor.cs#L12) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-10b3a944fa0177bd"></a>[`public override string Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor.ToString()`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/Surfaces/BgfxSurfaceDescriptor.cs#L12) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-9fed1236eaffcf2b"></a>[`public nint Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor.displayHandle { get; }`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/Surfaces/BgfxSurfaceDescriptor.cs#L49) | Gets the borrowed display value, or zero when this ABI does not require one. | — | — |
| <a id="api-ae660a8832c68e3b"></a>[`public bool Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor.supportsSrgbReset { get; }`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/Surfaces/BgfxSurfaceDescriptor.cs#L54) | Gets whether this primary surface permits sRGB reset. | — | — |
| <a id="api-8cffe725e6a7dbc0"></a>[`public nint Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor.windowHandle { get; }`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/Surfaces/BgfxSurfaceDescriptor.cs#L44) | Gets the borrowed window value; the default descriptor has no valid window. | — | — |
| <a id="api-5562af1015afb894"></a>[`public readonly record struct Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor : System.IEquatable<Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor>`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/Surfaces/BgfxSurfaceDescriptor.cs#L12) | Freezes borrowed window and display values without exposing BGFX native structures. | The original window owner must keep these values valid until rendering retirement completes. The default value is invalid and is rejected before native initialization or framebuffer creation. | — |

### `Inno.Adapter.Rendering.Bgfx.BgfxSurfaceRole`

<!-- api: T:Inno.Adapter.Rendering.Bgfx.BgfxSurfaceRole 00ea9584898bdbd523b6291f1592830fc208bc446abc41bbcd2a2f2a1d22a91f -->
<!-- api: F:Inno.Adapter.Rendering.Bgfx.BgfxSurfaceRole.Primary b534315d545cab4e6bee5587e4148620d6f1b14ea21274503687337360ef7882 -->
<!-- api: F:Inno.Adapter.Rendering.Bgfx.BgfxSurfaceRole.Additional c7f5c560346abf23c1c9495ec73a6ac9fdd9f8da001932ed151a06f440f60f0c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-af7cb54b88b7eb4d"></a>[`Inno.Adapter.Rendering.Bgfx.BgfxSurfaceRole.Additional = 1`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/Surfaces/IBgfxSurfaceIntegration.cs#L17) | An additional window whose framebuffer is owned by an active device generation. | — | — |
| <a id="api-0a4b39bc74e6c219"></a>[`Inno.Adapter.Rendering.Bgfx.BgfxSurfaceRole.Primary = 0`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/Surfaces/IBgfxSurfaceIntegration.cs#L13) | The primary window supplied before device initialization. | — | — |
| <a id="api-bf46ff7472b88918"></a>[`public enum Inno.Adapter.Rendering.Bgfx.BgfxSurfaceRole : int`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/Surfaces/IBgfxSurfaceIntegration.cs#L8) | Identifies how a borrowed window is used by a BGFX device, independently of its system ABI. | — | — |

### `Inno.Adapter.Rendering.Bgfx.IBgfxSurfaceIntegration`

<!-- api: T:Inno.Adapter.Rendering.Bgfx.IBgfxSurfaceIntegration a37a301ff683e9a5397da6c8baf3f0ce6d2f6dc0e7026bc217e28bcf101fb574 -->
<!-- api: P:Inno.Adapter.Rendering.Bgfx.IBgfxSurfaceIntegration.supportsAdditionalSurfaces 842501b0baa1b48051e3db004b20e31683d2dfc12b306ab817b0c5c06d5ec911 -->
<!-- api: M:Inno.Adapter.Rendering.Bgfx.IBgfxSurfaceIntegration.Resolve(Inno.Adapter.Platform.PlatformNativeHandles,Inno.Adapter.Rendering.Bgfx.BgfxSurfaceRole) 5c8712a2a1912690ef82d8f0fc615bee1d5810a11e18126fe31c482714a034b5 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-880327ab6677f614"></a>[`Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor Inno.Adapter.Rendering.Bgfx.IBgfxSurfaceIntegration.Resolve(Inno.Adapter.Platform.PlatformNativeHandles handles, Inno.Adapter.Rendering.Bgfx.BgfxSurfaceRole role)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/Surfaces/IBgfxSurfaceIntegration.cs#L52) | Validates a host ABI and freezes the handle values needed by the requested surface role.<br>返回：A valid immutable borrowing description; unsupported ABIs or roles must fail explicitly.<br>`handles`：The borrowed window and optional display, kept alive by their original owner until retirement.<br>`role`：The primary or additional use requested by the backend. | — | `System.ArgumentException`：A handle or role is invalid.<br>`System.NotSupportedException`：The host cannot connect the supplied ABI or requested role. |
| <a id="api-1ead9a59f11409b5"></a>[`bool Inno.Adapter.Rendering.Bgfx.IBgfxSurfaceIntegration.supportsAdditionalSurfaces { get; }`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/Surfaces/IBgfxSurfaceIntegration.cs#L32) | Gets whether this host permits additional surfaces, subject to the device's SwapChain capability. | — | — |
| <a id="api-311009687970aade"></a>[`public interface Inno.Adapter.Rendering.Bgfx.IBgfxSurfaceIntegration`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/Surfaces/IBgfxSurfaceIntegration.cs#L27) | Resolves an explicitly selected host's borrowed surface handles for the shared BGFX backend. | The device borrows this immutable integration. Resolution runs once per surface on its owner thread, before acquiring native ownership. Neither the integration nor the device owns the native window. | — |

## 项目依赖

- [Inno.Core.Execution](../../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Native.Bgfx](Inno.Native.Bgfx.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Platform](../../platform/Inno.Adapter.Platform.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Rendering](../../rendering/Inno.Adapter.Rendering.md)：公开引用边界由实际签名核对。
- [Inno.Platform](../../platform/Inno.Platform.md)：公开引用边界由实际签名核对。
- [Inno.Rendering](../../rendering/Inno.Rendering.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
