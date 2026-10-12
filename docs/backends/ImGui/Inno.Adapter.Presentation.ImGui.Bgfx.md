# Inno.Adapter.Presentation.ImGui.Bgfx

[分类索引](README.md) · [BGFX](../Bgfx/Inno.Adapter.Rendering.Bgfx.md) · [Platform ImGui](Inno.Adapter.Presentation.ImGui.Sdl3.md) · [Wiki 首页](../../README.md)

该 adapter 把 ImGui draw data 合成到 BGFX，不属于用户 Render Pipeline，也不进入普通游戏脚本 API。

内置 ImGui Shader Graph 声明 `s_tex` 纹理与 `outputEncoding` RenderPass uniform。主窗口与可执行 sRGB 写入的窗口直接在线性空间混合，再由窗口 framebuffer 编码。BGFX 的 Windows D3D11/D3D12 附加 swapchain 使用 UNORM RTV，不能对每个半透明图元提前编码，否则混合会落在错误的色彩空间；这类窗口先合成到 transient RGBA8 sRGB 纹理，再用一次全屏 Pass 仅在最终输出边界把合成后的线性 RGB 编码到 UNORM swapchain。两种路径共用同一 shader、纹理采样和 Alpha 混合契约。GPU 资源在 BGFX device 仍活跃时释放，窗口/viewport 关闭不直接销毁正在提交的资源。

## 源码归属

当前唯一源码 owner：`backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/Inno.Adapter.Presentation.ImGui.Bgfx.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 产品 Shader 配置与产物

Editor 产品显式传入 `InnoProductBuildProperties`，由所属平台的 `EditorProduct.props` 声明 `InnoImGuiShaderTarget` 与 `InnoImGuiShaderApis`。本项目消费该配置，不根据宿主 OS、进程 CPU 或目标 ID 维护选择表。共同 MSBuild 将配置传递给产品依赖，并从构建工具引导中移除产品属性。

Editor 配置导入同平台的 `ProductBuild.props`，复用已经声明的共同 BGFX Shader 平台/API 事实；Player 只使用共同配置，不引入 Editor UI。SDK 在执行阶段补入的间接依赖也由共同规则传递产品目标及配置，避免生成另一套无目标产物。

每种 API 的产物位于本项目目标隔离的 `obj` 下 `Shaders/<api>/Outputs/<api>.bin`；完整性 manifest 保存在对应 API 目录。构建任务验证源码、编译器闭包、目标、API 与输出内容哈希，使用写 lease 和原子发布；同长度、同时间戳的损坏不能被视为有效产物。运行时只读取嵌入的已编译 Shader，不启动工具。游戏导出目标不改变当前 Editor 的配置。

## 产品交互配置

ImGuiPresentationProvider 构造必须接收 ImGuiInteractionOptions；标准 Authoring Catalog 接收显式 provider 列表，不隐式创建它。资源仍归 context/renderer，输入沿 SDL 与 Core Events 原有入口。

## BGFX 视口生命周期

RetireViewport 清除该视口后续绘制并委托 Device.RetireWindowSurface；DrainViewportRetirements 仅在 owner thread 闭帧安全点推进。ImGuiPresentationContext 的唯一 renderer owner 是成功创建的 PlatformImGuiContext，外层不再重复 Dispose。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer`

<!-- api: T:Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer 8615eacfa19cd3382b581dc5c6396208b5fcb4bf063634dcd57768acb8dc99e9 -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.#ctor(Inno.Rendering.IRenderDevice,Inno.Rendering.GraphicsPipelineDescriptor) eaea7cc6d67fe03ad662e77fdd264dc04d09aa39c22485c71549a82084354c41 -->
<!-- api: P:Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.vertexLayout 6cce2354f63a69b8dbd554d5f807cc001837ab8814816ea36e0594e7ab026d92 -->
<!-- api: P:Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.lastShaderError f7ed0e41b0d1561532f05108b3f7720e5b8bd6526288218a9112055d80bdbb49 -->
<!-- api: P:Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.supportsViewports 9a0fdae6fa90677963b813d73e59cb7d133cb80e14870c8f8c0a6d35bd48de40 -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.ReplaceShaderArtifact(Inno.Rendering.GraphicsPipelineDescriptor) e4953a4382e9ea9e1becf5939e15e5199b67463f1563b7687f0c784089576169 -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.RegisterTexture(Inno.Rendering.PersistentTextureHandle) e4565f3d7e080457d1a04f84329722b07a8d066efc5ae82c386de2ddc6c002f0 -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.UnregisterTexture(Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle) a0fbfe9ae81f8ecf3d66fa7ca23192196bc28461027d56df64287534effcdfa4 -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.RenderMain(System.IntPtr) c9473bb7f38e2bb0c94c2e965321561b841db46862a6a0cfad92120febb5fdb7 -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.SynchronizeMainOutput(System.Int32,System.Int32) 7cc24fef910e1c14f8a05536aa38a7ec4ab3adea0a92de3492357ae495d649de -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.CreateViewport(Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget) a93907afd59a9b2950adc9adf56c35d7e818a1b8477314beaf73f7363008b3cc -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.ResizeViewport(Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget) 7ea3f91e15aaf3aebfd4d0757622186223990a140c6cd2b4ffdd970d16101ae5 -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.RenderViewport(Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget,System.IntPtr) c9b591f04cfcdfe1e89e4467e68d206c70ebf1c650dbbf03b572131decbf002a -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.PresentViewport(Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget) 85aa04017d40bde4e981a18e5fe66cb81a46714ae88097e27e8db758aa5b73b8 -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.RetireViewport(Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget) 7ccc9d27f4ad6efc9668b26f06d3af04e73d493dbc4491d5cf9ad36206ffa512 -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.DrainViewportRetirements 0ef8a342d89a3c02065cc69980cedf0eaf06469976eb84f071a3e690d047b1d0 -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.PrepareFrame(System.UInt64) 4b97d9597d2b6d91c73aae8436812d512e86325904dde5dd5448b33b5aac2494 -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.AddRenderPasses(Inno.Rendering.RenderGraphBuilder,System.UInt64) 0fa89887eff090a28e94ee272bb14007e581fdbd9e537a772679ca43efe27878 -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.Dispose a9c9461aace6757edfb91d6f030945a2e0fded7e0cdf85c3506fd63102298c9c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-30621fff76b34e0c"></a>[`public Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.BgfxImGuiRenderer(Inno.Rendering.IRenderDevice device, Inno.Rendering.GraphicsPipelineDescriptor pipelineDescriptor)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L78) | Creates a frame-contributing ImGui renderer around a backend-neutral shader artifact.<br>`device`：Active BGFX-backed neutral device.<br>`pipelineDescriptor`：Compiled ImGui shader stages and reflected interface. | — | `System.ArgumentException`：Thrown when the shader artifact does not use the required ImGui layout.<br>`System.NotSupportedException`：Thrown when device is not BGFX-backed. |
| <a id="api-96d37bf176384efc"></a>[`public void Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.AddRenderPasses(Inno.Rendering.RenderGraphBuilder graph, ulong frameIndex)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L367) | Adds the renderer's frame passes and resource declarations to the render graph.<br>`graph`：The graph consumed by add render passes; ownership remains with the caller unless explicitly stated otherwise.<br>`frameIndex`：The monotonic frame identity associated with this operation. | — | — |
| <a id="api-11adbc751273e280"></a>[`public void Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.CreateViewport(Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget target)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L219) | Creates a viewport using this implementation's validated inputs.<br>`target`：The existing target that receives the validated result. | — | — |
| <a id="api-e0b09b5c652c83d2"></a>[`public void Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.Dispose()`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L423) | Marks renderer-owned GPU resources for release at the next frame safety point or device teardown. | — | — |
| <a id="api-5ba5e5f644b251bf"></a>[`public void Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.DrainViewportRetirements()`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L315) | Drains requested retirements at a closed-frame owner-thread point outside native UI callbacks. | Failure or timeout retains all unfinished viewport and renderer owners. | — |
| <a id="api-b41e6e765a45f3a4"></a>[`public void Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.PrepareFrame(ulong frameIndex)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L323) | Prepares frame-owned resources before render graph recording begins.<br>`frameIndex`：The monotonic frame identity associated with this operation. | — | — |
| <a id="api-fdc65709af4e37e4"></a>[`public void Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.PresentViewport(Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget target)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L283) | Presents the completed frame for the supplied auxiliary viewport.<br>`target`：The existing target that receives the validated result. | — | — |
| <a id="api-2c051e64cdeef573"></a>[`public Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.RegisterTexture(Inno.Rendering.PersistentTextureHandle texture)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L132) | Registers a persistent render texture as an opaque ImGui texture token.<br>返回：An opaque token accepted by Inno.Adapter.Presentation.ImGui.PlatformImGuiContext.DrawImage(Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2).<br>`texture`：Texture owned by the active device generation. | — | — |
| <a id="api-0ef12a1db90af66e"></a>[`public void Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.RenderMain(nint drawData)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L176) | Records ImGui draw data for the main application viewport.<br>`drawData`：The draw data consumed by render main; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-b0c6b12372be99c4"></a>[`public void Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.RenderViewport(Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget target, nint drawData)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L260) | Records ImGui draw data for the supplied auxiliary viewport.<br>`target`：The existing target that receives the validated result.<br>`drawData`：The draw data consumed by render viewport; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-7528892b12ccbcb1"></a>[`public void Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.ReplaceShaderArtifact(Inno.Rendering.GraphicsPipelineDescriptor pipelineDescriptor)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L112) | Queues a compiled shader replacement that commits atomically at a frame safety point.<br>`pipelineDescriptor`：Complete candidate artifact. | — | — |
| <a id="api-72a3b1e6a9aad52a"></a>[`public void Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.ResizeViewport(Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget target)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L238) | Resizes an auxiliary viewport surface to its current platform dimensions.<br>`target`：The existing target that receives the validated result. | — | — |
| <a id="api-a425acb0b1e82e8a"></a>[`public System.Threading.Tasks.Task Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.RetireViewport(Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget target)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L297) | Destroys the auxiliary viewport and releases its rendering resources.<br>返回：Completion of render-thread window release, or an already completed task for an unallocated viewport.<br>`target`：The existing target that receives the validated result. | — | — |
| <a id="api-fc943f2a0d86aaf8"></a>[`public void Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.SynchronizeMainOutput(int pixelWidth, int pixelHeight)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L199) | Invalidates a captured main packet when its drawable extent changes. The Shell owns device presentation availability and publishes it at frame boundaries.<br>`pixelWidth`：The pixel width consumed by synchronize main output; ownership remains with the caller unless explicitly stated otherwise.<br>`pixelHeight`：The pixel height consumed by synchronize main output; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-32adb35b12efa6e6"></a>[`public bool Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.UnregisterTexture(Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle texture)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L157) | Unregisters an opaque texture token without taking ownership of the source texture.<br>返回：true when the token was an external registration.<br>`texture`：Previously allocated token. | — | — |
| <a id="api-fbee6f0dc00bdbbf"></a>[`public string? Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.lastShaderError { get; private set; }`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L99) | Gets the last recoverable shader replacement error while the last-good pipeline remains active. | — | — |
| <a id="api-e4101263d3d54599"></a>[`public bool Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.supportsViewports { get; }`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L104) | Gets whether supports viewports is enabled for this implementation. | — | — |
| <a id="api-f8a21ab60178268b"></a>[`public static Inno.Rendering.RenderVertexLayout Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.vertexLayout { get; }`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L94) | Gets the exact interleaved vertex layout required by the built-in ImGui shaders. | — | — |
| <a id="api-2a06981ed0c9b411"></a>[`public sealed class Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer : Inno.Adapter.Presentation.ImGui.IPlatformImGuiRenderer, Inno.Rendering.IRenderFrameGraphContributor`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L17) | Copies Dear ImGui command lists into frame-owned data and composites them through BGFX RenderGraph passes. | — | — |

### `Inno.Adapter.Presentation.ImGui.BgfxImGuiShaderArtifacts`

<!-- api: T:Inno.Adapter.Presentation.ImGui.BgfxImGuiShaderArtifacts 704ed9365c07072ab4ddd45ac78acc055c8549921b0e91f185fb97b112454aec -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.BgfxImGuiShaderArtifacts.Load(Inno.Rendering.GraphicsApi) c86d35a90b315623f5ae6c4a89ee16df5c089e284447d610704169129d96b669 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e5882e00a054cfe2"></a>[`public static Inno.Rendering.GraphicsPipelineDescriptor Inno.Adapter.Presentation.ImGui.BgfxImGuiShaderArtifacts.Load(Inno.Rendering.GraphicsApi api)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiShaderArtifacts.cs#L25) | Reads the precompiled program for the declared product and requested graphics API without invoking a compiler.<br>返回：The validated texture binding, stages and render state required by ImGui.<br>`api`：The active device API. | — | `System.IO.InvalidDataException`：The installed adapter lacks a required artifact or its binding contract is invalid. |
| <a id="api-3b69f28625758ef3"></a>[`public static class Inno.Adapter.Presentation.ImGui.BgfxImGuiShaderArtifacts`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiShaderArtifacts.cs#L11) | Loads the graph-built, source-free ImGui program distributed with this presentation adapter. | — | — |

### `Inno.Adapter.Presentation.ImGui.ImGuiPresentationProvider`

<!-- api: T:Inno.Adapter.Presentation.ImGui.ImGuiPresentationProvider bac8c72219a7ddbc501e31edd3f1e3900fc100dc6203032cc43133d43996e5aa -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.ImGuiPresentationProvider.#ctor(Inno.Adapter.Presentation.ImGui.ImGuiInteractionOptions) 93edcce07f31616974bebe2882d41a15a1935b4185f14a580e7e07b98117c66e -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.ImGuiPresentationProvider.CreateContext(Inno.Adapter.Presentation.PresentationBackendOptions) 38681f14d63e63bac16d9948f0871c4e58a328a12edf7e16e152a93961d5e200 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-54adede1a0a66f0a"></a>[`public Inno.Adapter.Presentation.ImGui.ImGuiPresentationProvider.ImGuiPresentationProvider(Inno.Adapter.Presentation.ImGui.ImGuiInteractionOptions interaction)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/ImGuiPresentationProvider.cs#L22) | Registers the bundled presentation implementation without creating a native context.<br>`interaction`：The borrowed immutable conventions explicitly selected by the Editor product. | — | `System.ArgumentNullException`：Interaction configuration is absent. |
| <a id="api-776961223089ae7e"></a>[`public override Inno.Adapter.Presentation.IPresentationContext Inno.Adapter.Presentation.ImGui.ImGuiPresentationProvider.CreateContext(Inno.Adapter.Presentation.PresentationBackendOptions options)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/ImGuiPresentationProvider.cs#L29) | Creates a new caller-owned presentation context over the supplied host resources.<br>返回：A new context whose caller must dispose it before releasing the supplied resources. Providers must not return null.<br>`options`：Externally owned platform and rendering resources, together with the requested presentation policy. | — | `System.NotSupportedException`：The host resources or requested features are incompatible with this implementation. |
| <a id="api-0c7b5023d6394d3b"></a>[`public sealed class Inno.Adapter.Presentation.ImGui.ImGuiPresentationProvider : Inno.Adapter.Presentation.PresentationBackendProvider`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/ImGuiPresentationProvider.cs#L9) | Registers SDL3 window integration with BGFX ImGui presentation. | — | — |

## 项目依赖

- [Inno.Native.ImGui](Inno.Native.ImGui.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Rendering.Bgfx](../Bgfx/Inno.Adapter.Rendering.Bgfx.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Presentation](../../platform/Inno.Adapter.Presentation.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Presentation.ImGui.Sdl3](Inno.Adapter.Presentation.ImGui.Sdl3.md)：公开引用边界由实际签名核对。
- [Inno.Rendering](../../rendering/Inno.Rendering.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Platform.Sdl3](../Sdl3/Inno.Adapter.Platform.Sdl3.md)：公开引用边界由实际签名核对。
