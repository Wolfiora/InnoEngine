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

## 当前源码公开 API 清单

只列当前源码 public/protected 表面；内部机制不是稳定 API，参数、返回、失败及所有权以英文 XML 为准。

### `Inno.Adapter.Rendering.Bgfx.BgfxCompositionProgramProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Rendering.Bgfx.BgfxCompositionProgramProvider`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxCompositionProgramProvider.cs#L12) | Supplies the BGFX shader program used to composite model output layers. |
| [`Inno.Rendering.GraphicsPipelineDescriptor Inno.Adapter.Rendering.Bgfx.BgfxCompositionProgramProvider.CreateDescriptor(Inno.Rendering.GraphicsCapabilities capabilities, Inno.Rendering.RenderVertexLayout vertexLayout)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxCompositionProgramProvider.cs#L31) | Loads the BGFX target program matching the active device renderer. |
| [`Inno.Rendering.GraphicsPipelineDescriptor Inno.Adapter.Rendering.Bgfx.BgfxCompositionProgramProvider.CreateOutputTransferDescriptor(Inno.Rendering.GraphicsCapabilities capabilities, Inno.Rendering.RenderVertexLayout vertexLayout)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxCompositionProgramProvider.cs#L48) | Loads the target program that encodes the completed linear composition for presentation. |

### `Inno.Adapter.Rendering.Bgfx.BgfxDevice`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Rendering.Bgfx.BgfxDevice`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.cs#L17) | Implements the sole BGFX device generation, API-thread frame boundary and graph backend. |
| [`Inno.Adapter.Rendering.Bgfx.BgfxDevice.BgfxDevice(Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions options)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.cs#L74) | Initializes BGFX and captures immutable device capabilities. |
| [`Inno.Rendering.ComputePipelineHandle Inno.Adapter.Rendering.Bgfx.BgfxDevice.CreateComputePipeline(Inno.Rendering.ComputePipelineDescriptor descriptor, string name)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Resources.cs#L203) | Creates a compute pipeline using this implementation's validated inputs. |
| [`Inno.Rendering.GraphicsCapabilities Inno.Adapter.Rendering.Bgfx.BgfxDevice.capabilities`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.cs#L148) | Gets the immutable feature and limit set reported by the active graphics backend. |
| [`Inno.Rendering.GraphicsPipelineHandle Inno.Adapter.Rendering.Bgfx.BgfxDevice.CreateGraphicsPipeline(Inno.Rendering.GraphicsPipelineDescriptor descriptor, string name)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Resources.cs#L156) | Creates a graphics pipeline using this implementation's validated inputs. |
| [`Inno.Rendering.PersistentBufferHandle Inno.Adapter.Rendering.Bgfx.BgfxDevice.CreateBuffer(Inno.Rendering.PersistentBufferDescriptor descriptor, System.ReadOnlySpan<byte> initialData, string name)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Resources.cs#L38) | Creates a buffer using this implementation's validated inputs. |
| [`Inno.Rendering.PersistentTextureHandle Inno.Adapter.Rendering.Bgfx.BgfxDevice.CreateTexture(Inno.Rendering.RenderTextureContainer container, System.ReadOnlySpan<byte> data, bool sRgb, string name)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Textures.cs#L66) | Creates a texture using this implementation's validated inputs. |
| [`Inno.Rendering.PersistentTextureHandle Inno.Adapter.Rendering.Bgfx.BgfxDevice.CreateTexture(Inno.Rendering.RenderTextureDescriptor descriptor, string name)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Textures.cs#L28) | Creates a texture using this implementation's validated inputs. |
| [`Inno.Rendering.RenderCommandEncoder Inno.Adapter.Rendering.Bgfx.BgfxDevice.BeginPass(Inno.Rendering.CompiledRenderPass pass)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Graphs.cs#L120) | Begins recording commands for one compiled render pass. |
| [`Inno.Rendering.RenderDeviceAllocationCounters? Inno.Adapter.Rendering.Bgfx.BgfxDevice.allocationCounters`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.cs#L184) | Gets API-thread allocation diagnostics for the current device generation's native transient pools. |
| [`Inno.Rendering.RenderDeviceFrameCounters Inno.Adapter.Rendering.Bgfx.BgfxDevice.frameCounters`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.cs#L179) | Gets the submitted and completed frame counters used for deferred retirement. |
| [`Inno.Rendering.RenderPresentationSize? Inno.Adapter.Rendering.Bgfx.BgfxDevice.primaryPresentationSize`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.cs#L174) | Gets the current drawable pixel size of the main presentation surface. |
| [`Inno.Rendering.RenderSurfaceHandle Inno.Adapter.Rendering.Bgfx.BgfxDevice.CreateWindowSurface(Inno.Adapter.Platform.PlatformNativeHandles nativeHandles, int width, int height, string name)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Surfaces.cs#L43) | Creates a detached-window presentation surface at a frame safety point. |
| [`Inno.Rendering.RenderTextureReadbackHandle Inno.Adapter.Rendering.Bgfx.BgfxDevice.BeginTextureReadback(Inno.Rendering.PersistentTextureHandle texture, int mipLevel = 0)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Readback.cs#L28) | Schedules asynchronous texture readback into caller-provided destination storage. |
| [`System.Threading.Tasks.Task Inno.Adapter.Rendering.Bgfx.BgfxDevice.RetireWindowSurface(Inno.Rendering.RenderSurfaceHandle surface)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Surfaces.cs#L156) | Stops accepting work for a surface and queues all of its framebuffer generations for retirement. |
| [`bool Inno.Adapter.Rendering.Bgfx.BgfxDevice.TryGetTextureReadback(Inno.Rendering.RenderTextureReadbackHandle readback, out Inno.Rendering.RenderTextureReadbackResult? result)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Readback.cs#L87) | Attempts to get texture readback without changing state when the operation cannot complete. |
| [`bool Inno.Adapter.Rendering.Bgfx.BgfxDevice.WindowSurfaceIsSrgb(Inno.Rendering.RenderSurfaceHandle surface)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Surfaces.cs#L88) | Reports whether the native window surface encodes linear render-target writes as sRGB. |
| [`bool Inno.Adapter.Rendering.Bgfx.BgfxDevice.backbufferIsSrgb`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.cs#L153) | Gets whether the primary presentation surface encodes linear color as sRGB. |
| [`bool Inno.Adapter.Rendering.Bgfx.BgfxDevice.primaryPresentationEncodesSrgb`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.cs#L158) | Gets whether the primary BGFX surface automatically encodes linear RGB to sRGB. |
| [`bool Inno.Adapter.Rendering.Bgfx.BgfxDevice.supportsAdditionalSurfaces`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.cs#L163) | Gets whether both the selected host and active renderer permit additional window surfaces. |
| [`uint Inno.Adapter.Rendering.Bgfx.BgfxDevice.EndFrame()`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Frames.cs#L86) | Commits the current frame-scoped operation and returns its completion identity. |
| [`uint Inno.Adapter.Rendering.Bgfx.BgfxDevice.backendFrame`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.cs#L198) | Gets the last frame number returned by BGFX submission. |
| [`uint Inno.Adapter.Rendering.Bgfx.BgfxDevice.generation`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.cs#L169) | Gets the generation identity that owns this value. |
| [`void Inno.Adapter.Rendering.Bgfx.BgfxDevice.BeginFrame()`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Frames.cs#L19) | Begins a frame-scoped operation and makes queued work visible. |
| [`void Inno.Adapter.Rendering.Bgfx.BgfxDevice.BeginGraph(Inno.Rendering.CompiledRenderGraph graph)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Graphs.cs#L22) | Begins recording commands for one validated compiled render graph. |
| [`void Inno.Adapter.Rendering.Bgfx.BgfxDevice.CancelTextureReadback(Inno.Rendering.RenderTextureReadbackHandle readback)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Readback.cs#L124) | Cancels a pending texture readback and releases its retained state. |
| [`void Inno.Adapter.Rendering.Bgfx.BgfxDevice.DestroyBuffer(Inno.Rendering.PersistentBufferHandle buffer)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Resources.cs#L66) | Destroys the buffer after all in-flight references have retired. |
| [`void Inno.Adapter.Rendering.Bgfx.BgfxDevice.DestroyComputePipeline(Inno.Rendering.ComputePipelineHandle pipeline)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Resources.cs#L227) | Destroys the compute pipeline after all in-flight references have retired. |
| [`void Inno.Adapter.Rendering.Bgfx.BgfxDevice.DestroyGraphicsPipeline(Inno.Rendering.GraphicsPipelineHandle pipeline)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Resources.cs#L175) | Destroys the graphics pipeline after all in-flight references have retired. |
| [`void Inno.Adapter.Rendering.Bgfx.BgfxDevice.DestroyTexture(Inno.Rendering.PersistentTextureHandle texture)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Textures.cs#L307) | Destroys the texture after all in-flight references have retired. |
| [`void Inno.Adapter.Rendering.Bgfx.BgfxDevice.Dispose()`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Retirement.cs#L23) | Shuts down BGFX after releasing all active and queued backend resources. |
| [`void Inno.Adapter.Rendering.Bgfx.BgfxDevice.DrainWindowSurfaceRetirements()`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Surfaces.cs#L181) | Advances nonpresenting frames until all requested window retirements have been acknowledged. |
| [`void Inno.Adapter.Rendering.Bgfx.BgfxDevice.EndGraph(Inno.Rendering.CompiledRenderGraph graph)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Graphs.cs#L167) | Finishes graph recording and submits its completed command stream. |
| [`void Inno.Adapter.Rendering.Bgfx.BgfxDevice.EndPass(Inno.Rendering.CompiledRenderPass pass)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Graphs.cs#L149) | Ends the active render pass and seals its recorded commands. |
| [`void Inno.Adapter.Rendering.Bgfx.BgfxDevice.Execute(Inno.Rendering.CompiledRenderGraph graph, ulong frameIndex)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Frames.cs#L71) | Executes the prepared operation and publishes only a completed result. |
| [`void Inno.Adapter.Rendering.Bgfx.BgfxDevice.ResizeWindowSurface(Inno.Rendering.RenderSurfaceHandle surface, int width, int height)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Surfaces.cs#L111) | Recreates a detached-window presentation surface for a new drawable extent. |
| [`void Inno.Adapter.Rendering.Bgfx.BgfxDevice.SetPrimaryPresentationSize(Inno.Rendering.RenderPresentationSize? size)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Frames.cs#L122) | Publishes drawable availability at the next BeginFrame without inventing a windowless output. |
| [`void Inno.Adapter.Rendering.Bgfx.BgfxDevice.SetVerticalSync(bool enabled)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Frames.cs#L141) | Queues an idempotent presentation policy update for the next BeginFrame reset. Noop devices retain the policy without issuing a native presentation reset. |
| [`void Inno.Adapter.Rendering.Bgfx.BgfxDevice.UpdateBuffer(Inno.Rendering.PersistentBufferHandle buffer, System.ReadOnlySpan<byte> data, int startElement = 0)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Resources.cs#L90) | Updates buffer state from the current authoritative inputs. |
| [`void Inno.Adapter.Rendering.Bgfx.BgfxDevice.UpdateTexture(Inno.Rendering.PersistentTextureHandle texture, System.ReadOnlySpan<byte> data, int mipLevel = 0, int arrayLayer = 0)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Textures.cs#L122) | Updates texture state from the current authoritative inputs. |
| [`void Inno.Adapter.Rendering.Bgfx.BgfxDevice.UpdateTextureRegion(Inno.Rendering.PersistentTextureHandle texture, Inno.Rendering.RenderTextureRegion region, System.ReadOnlySpan<byte> data)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Textures.cs#L216) | Updates texture region state from the current authoritative inputs. |

### `Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDeviceOptions.cs#L10) | Configures BGFX initialization without exposing native BGFX structures. |
| [`Inno.Adapter.Rendering.Bgfx.IBgfxSurfaceIntegration? Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions.surfaceIntegration`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDeviceOptions.cs#L30) | Gets or sets the borrowed immutable surface integration, required for a windowed device. A windowless device may omit it and cannot create additional windows without one. |
| [`Inno.Platform.IPlatformWindow? Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions.window`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDeviceOptions.cs#L24) | Gets or sets the platform window used as the main swapchain surface. |
| [`Inno.Rendering.GraphicsApi? Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions.preferredBackend`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDeviceOptions.cs#L19) | Gets or sets the preferred renderer, or for platform default. |
| [`bool Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions.forceSingleThreaded`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDeviceOptions.cs#L71) | Gets or sets whether BGFX rendering is driven inline on the API thread. |
| [`bool Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions.sRgbBackbuffer`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDeviceOptions.cs#L62) | Gets or sets whether the main backbuffer performs sRGB encoding. |
| [`bool Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions.verticalSync`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDeviceOptions.cs#L57) | Gets or sets whether submission waits for display synchronization. |
| [`int Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions.backbufferHeight`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDeviceOptions.cs#L46) | Gets or sets the initial backbuffer height in physical pixels when no window supplies one. |
| [`int Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions.backbufferWidth`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDeviceOptions.cs#L35) | Gets or sets the initial backbuffer width in physical pixels when no window supplies one. |
| [`int Inno.Adapter.Rendering.Bgfx.BgfxDeviceOptions.deferredDestroyFrames`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDeviceOptions.cs#L76) | Gets or sets the number of submitted frames before queued native destruction. |

### `Inno.Adapter.Rendering.Bgfx.BgfxRenderingBackendProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Rendering.Bgfx.BgfxRenderingBackendProvider`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxRenderingBackendProvider.cs#L9) | Supplies BGFX rendering devices and compatible layer composition programs. |
| [`Inno.Adapter.Rendering.Bgfx.BgfxRenderingBackendProvider.BgfxRenderingBackendProvider(Inno.Adapter.Rendering.Bgfx.IBgfxSurfaceIntegration surfaceIntegration)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxRenderingBackendProvider.cs#L22) | Registers BGFX with an explicitly selected, borrowed host surface integration. |
| [`override Inno.Rendering.IRenderDevice Inno.Adapter.Rendering.Bgfx.BgfxRenderingBackendProvider.CreateDevice(Inno.Adapter.Rendering.RenderingBackendOptions options)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxRenderingBackendProvider.cs#L29) | See the implemented contract. |
| [`override Inno.Rendering.IRenderLayerCompositionProgramProvider Inno.Adapter.Rendering.Bgfx.BgfxRenderingBackendProvider.CreateCompositionProgramProvider()`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxRenderingBackendProvider.cs#L44) | See the implemented contract. |

### `Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/Surfaces/BgfxSurfaceDescriptor.cs#L12) | Freezes borrowed window and display values without exposing BGFX native structures. |
| [`Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor.BgfxSurfaceDescriptor(nint windowHandle, nint displayHandle, bool supportsSrgbReset)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/Surfaces/BgfxSurfaceDescriptor.cs#L29) | Creates a validated borrowing description for a selected host integration. |
| [`bool Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor.supportsSrgbReset`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/Surfaces/BgfxSurfaceDescriptor.cs#L54) | Gets whether this primary surface permits sRGB reset. |
| [`nint Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor.displayHandle`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/Surfaces/BgfxSurfaceDescriptor.cs#L49) | Gets the borrowed display value, or zero when this ABI does not require one. |
| [`nint Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor.windowHandle`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/Surfaces/BgfxSurfaceDescriptor.cs#L44) | Gets the borrowed window value; the default descriptor has no valid window. |

### `Inno.Adapter.Rendering.Bgfx.BgfxSurfaceRole`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Rendering.Bgfx.BgfxSurfaceRole`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/Surfaces/IBgfxSurfaceIntegration.cs#L8) | Identifies how a borrowed window is used by a BGFX device, independently of its system ABI. |
| [`Inno.Adapter.Rendering.Bgfx.BgfxSurfaceRole.Additional`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/Surfaces/IBgfxSurfaceIntegration.cs#L17) | An additional window whose framebuffer is owned by an active device generation. |
| [`Inno.Adapter.Rendering.Bgfx.BgfxSurfaceRole.Primary`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/Surfaces/IBgfxSurfaceIntegration.cs#L13) | The primary window supplied before device initialization. |

### `Inno.Adapter.Rendering.Bgfx.IBgfxSurfaceIntegration`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Rendering.Bgfx.BgfxSurfaceDescriptor Inno.Adapter.Rendering.Bgfx.IBgfxSurfaceIntegration.Resolve(Inno.Adapter.Platform.PlatformNativeHandles handles, Inno.Adapter.Rendering.Bgfx.BgfxSurfaceRole role)`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/Surfaces/IBgfxSurfaceIntegration.cs#L52) | Validates a host ABI and freezes the handle values needed by the requested surface role. |
| [`Inno.Adapter.Rendering.Bgfx.IBgfxSurfaceIntegration`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/Surfaces/IBgfxSurfaceIntegration.cs#L27) | Resolves an explicitly selected host's borrowed surface handles for the shared BGFX backend. |
| [`bool Inno.Adapter.Rendering.Bgfx.IBgfxSurfaceIntegration.supportsAdditionalSurfaces`](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/Surfaces/IBgfxSurfaceIntegration.cs#L32) | Gets whether this host permits additional surfaces, subject to the device's SwapChain capability. |

## 项目依赖

- [Inno.Core.Execution](../../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Native.Bgfx](Inno.Native.Bgfx.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Platform](../../platform/Inno.Adapter.Platform.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Rendering](../../rendering/Inno.Adapter.Rendering.md)：公开引用边界由实际签名核对。
- [Inno.Platform](../../platform/Inno.Platform.md)：公开引用边界由实际签名核对。
- [Inno.Rendering](../../rendering/Inno.Rendering.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
