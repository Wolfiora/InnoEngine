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

只列当前源码 public/protected 表面；内部机制不是稳定 API，参数、返回、失败及所有权以英文 XML 为准。

### `Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L17) | Copies Dear ImGui command lists into frame-owned data and composites them through BGFX RenderGraph passes. |
| [`Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.BgfxImGuiRenderer(Inno.Rendering.IRenderDevice device, Inno.Rendering.GraphicsPipelineDescriptor pipelineDescriptor)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L78) | Creates a frame-contributing ImGui renderer around a backend-neutral shader artifact. |
| [`Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.RegisterTexture(Inno.Rendering.PersistentTextureHandle texture)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L132) | Registers a persistent render texture as an opaque ImGui texture token. |
| [`System.Threading.Tasks.Task Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.RetireViewport(Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget target)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L297) | Destroys the auxiliary viewport and releases its rendering resources. |
| [`bool Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.UnregisterTexture(Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle texture)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L157) | Unregisters an opaque texture token without taking ownership of the source texture. |
| [`bool Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.supportsViewports`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L104) | Gets whether supports viewports is enabled for this implementation. |
| [`static Inno.Rendering.RenderVertexLayout Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.vertexLayout`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L94) | Gets the exact interleaved vertex layout required by the built-in ImGui shaders. |
| [`string? Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.lastShaderError`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L99) | Gets the last recoverable shader replacement error while the last-good pipeline remains active. |
| [`void Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.AddRenderPasses(Inno.Rendering.RenderGraphBuilder graph, ulong frameIndex)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L367) | Adds the renderer's frame passes and resource declarations to the render graph. |
| [`void Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.CreateViewport(Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget target)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L219) | Creates a viewport using this implementation's validated inputs. |
| [`void Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.Dispose()`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L423) | Marks renderer-owned GPU resources for release at the next frame safety point or device teardown. |
| [`void Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.DrainViewportRetirements()`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L315) | See the implemented contract. |
| [`void Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.PrepareFrame(ulong frameIndex)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L323) | Prepares frame-owned resources before render graph recording begins. |
| [`void Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.PresentViewport(Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget target)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L283) | Presents the completed frame for the supplied auxiliary viewport. |
| [`void Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.RenderMain(nint drawData)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L176) | Records ImGui draw data for the main application viewport. |
| [`void Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.RenderViewport(Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget target, nint drawData)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L260) | Records ImGui draw data for the supplied auxiliary viewport. |
| [`void Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.ReplaceShaderArtifact(Inno.Rendering.GraphicsPipelineDescriptor pipelineDescriptor)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L112) | Queues a compiled shader replacement that commits atomically at a frame safety point. |
| [`void Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.ResizeViewport(Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget target)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L238) | Resizes an auxiliary viewport surface to its current platform dimensions. |
| [`void Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.SynchronizeMainOutput(int pixelWidth, int pixelHeight)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L199) | Invalidates a captured main packet when its drawable extent changes. The Shell owns device presentation availability and publishes it at frame boundaries. |

### `Inno.Adapter.Presentation.ImGui.BgfxImGuiShaderArtifacts`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Presentation.ImGui.BgfxImGuiShaderArtifacts`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiShaderArtifacts.cs#L11) | Loads the graph-built, source-free ImGui program distributed with this presentation adapter. |
| [`static Inno.Rendering.GraphicsPipelineDescriptor Inno.Adapter.Presentation.ImGui.BgfxImGuiShaderArtifacts.Load(Inno.Rendering.GraphicsApi api)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiShaderArtifacts.cs#L25) | Reads the precompiled program for the declared product and requested graphics API without invoking a compiler. |

### `Inno.Adapter.Presentation.ImGui.ImGuiPresentationProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Presentation.ImGui.ImGuiPresentationProvider`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/ImGuiPresentationProvider.cs#L9) | Registers SDL3 window integration with BGFX ImGui presentation. |
| [`Inno.Adapter.Presentation.ImGui.ImGuiPresentationProvider.ImGuiPresentationProvider(Inno.Adapter.Presentation.ImGui.ImGuiInteractionOptions interaction)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/ImGuiPresentationProvider.cs#L22) | Registers the bundled presentation implementation without creating a native context. |
| [`override Inno.Adapter.Presentation.IPresentationContext Inno.Adapter.Presentation.ImGui.ImGuiPresentationProvider.CreateContext(Inno.Adapter.Presentation.PresentationBackendOptions options)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/ImGuiPresentationProvider.cs#L29) | See the implemented contract. |

## 项目依赖

- [Inno.Native.ImGui](Inno.Native.ImGui.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Rendering.Bgfx](../Bgfx/Inno.Adapter.Rendering.Bgfx.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Presentation](../../platform/Inno.Adapter.Presentation.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Presentation.ImGui.Sdl3](Inno.Adapter.Presentation.ImGui.Sdl3.md)：公开引用边界由实际签名核对。
- [Inno.Rendering](../../rendering/Inno.Rendering.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Platform.Sdl3](../Sdl3/Inno.Adapter.Platform.Sdl3.md)：公开引用边界由实际签名核对。
