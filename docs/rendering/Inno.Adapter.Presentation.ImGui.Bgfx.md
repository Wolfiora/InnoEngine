# Inno.Adapter.Presentation.ImGui.Bgfx

[Rendering 索引](README.md) · [BGFX](Inno.Adapter.Rendering.Bgfx.md) · [Platform ImGui](../platform/Inno.Adapter.Presentation.ImGui.Sdl3.md)

该 adapter 把 ImGui draw data 合成到 BGFX，不属于用户 Render Pipeline，也不进入普通游戏脚本 API。

内置 ImGui Shader Graph 声明 `s_tex` 纹理与 `outputEncoding` RenderPass uniform。主窗口与可执行 sRGB 写入的窗口直接在线性空间混合，再由窗口 framebuffer 编码。BGFX 的 Windows D3D11/D3D12 附加 swapchain 使用 UNORM RTV，不能对每个半透明图元提前编码，否则混合会落在错误的色彩空间；这类窗口先合成到 transient RGBA8 sRGB 纹理，再用一次全屏 Pass 仅在最终输出边界把合成后的线性 RGB 编码到 UNORM swapchain。两种路径共用同一 shader、纹理采样和 Alpha 混合契约。GPU 资源在 BGFX device 仍活跃时释放，窗口/viewport 关闭不直接销毁正在提交的资源。

## 当前源码公开 API 清单

以下仅列出当前程序集自己声明的 public/protected 契约；继承成员遵循所属基类页面。internal/private 实现不作为稳定公开 API。签名依据当前源码语义模型生成，行为、参数、异常与所有权说明同时以对应英文 XML 为准。

### `Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.BgfxImGuiRenderer(Inno.Rendering.IRenderDevice device, Inno.Rendering.GraphicsPipelineDescriptor pipelineDescriptor)`](../../src/adapters/presentation/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L77) | Creates a frame-contributing ImGui renderer around a backend-neutral shader artifact. |
| [`void Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.AddRenderPasses(Inno.Rendering.RenderGraphBuilder graph, ulong frameIndex)`](../../src/adapters/presentation/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L361) | Adds the renderer's frame passes and resource declarations to the render graph. |
| [`void Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.CreateViewport(Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget target)`](../../src/adapters/presentation/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L218) | Creates a viewport using this implementation's validated inputs. |
| [`void Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.DestroyViewport(Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget target)`](../../src/adapters/presentation/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L293) | Destroys the auxiliary viewport and releases its rendering resources. |
| [`void Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.Dispose()`](../../src/adapters/presentation/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L417) | Marks renderer-owned GPU resources for release at the next frame safety point or device teardown. |
| [`void Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.PrepareFrame(ulong frameIndex)`](../../src/adapters/presentation/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L317) | Prepares frame-owned resources before render graph recording begins. |
| [`void Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.PresentViewport(Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget target)`](../../src/adapters/presentation/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L282) | Presents the completed frame for the supplied auxiliary viewport. |
| [`Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.RegisterTexture(Inno.Rendering.PersistentTextureHandle texture)`](../../src/adapters/presentation/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L131) | Registers a persistent render texture as an opaque ImGui texture token. |
| [`void Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.RenderMain(nint drawData)`](../../src/adapters/presentation/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L175) | Records ImGui draw data for the main application viewport. |
| [`void Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.RenderViewport(Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget target, nint drawData)`](../../src/adapters/presentation/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L259) | Records ImGui draw data for the supplied auxiliary viewport. |
| [`void Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.ReplaceShaderArtifact(Inno.Rendering.GraphicsPipelineDescriptor pipelineDescriptor)`](../../src/adapters/presentation/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L111) | Queues a compiled shader replacement that commits atomically at a frame safety point. |
| [`void Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.ResizeViewport(Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget target)`](../../src/adapters/presentation/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L237) | Resizes an auxiliary viewport surface to its current platform dimensions. |
| [`void Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.SynchronizeMainOutput(int pixelWidth, int pixelHeight)`](../../src/adapters/presentation/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L198) | Invalidates a captured main packet when its drawable extent changes. The Shell owns device presentation availability and publishes it at frame boundaries. |
| [`bool Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.UnregisterTexture(Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle texture)`](../../src/adapters/presentation/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L156) | Unregisters an opaque texture token without taking ownership of the source texture. |
| [`string? Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.lastShaderError`](../../src/adapters/presentation/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L98) | Gets the last recoverable shader replacement error while the last-good pipeline remains active. |
| [`bool Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.supportsViewports`](../../src/adapters/presentation/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L103) | Gets whether supports viewports is enabled for this implementation. |
| [`static Inno.Rendering.RenderVertexLayout Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer.vertexLayout`](../../src/adapters/presentation/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L93) | Gets the exact interleaved vertex layout required by the built-in ImGui shaders. |
| [`Inno.Adapter.Presentation.ImGui.BgfxImGuiRenderer`](../../src/adapters/presentation/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiRenderer.cs#L16) | Copies Dear ImGui command lists into frame-owned data and composites them through BGFX RenderGraph passes. |

### `Inno.Adapter.Presentation.ImGui.BgfxImGuiShaderArtifacts`

| 当前声明 | 行为 |
| --- | --- |
| [`static Inno.Rendering.GraphicsPipelineDescriptor Inno.Adapter.Presentation.ImGui.BgfxImGuiShaderArtifacts.Load(Inno.Rendering.GraphicsApi api)`](../../src/adapters/presentation/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiShaderArtifacts.cs#L29) | Reads the precompiled program for the current host and requested graphics API without invoking a compiler. |
| [`Inno.Adapter.Presentation.ImGui.BgfxImGuiShaderArtifacts`](../../src/adapters/presentation/Inno.Adapter.Presentation.ImGui.Bgfx/BgfxImGuiShaderArtifacts.cs#L12) | Loads the graph-built, source-free ImGui program distributed with this presentation adapter. |

## 项目依赖

- [Inno.Native.ImGui](../native/Inno.Native.ImGui.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Adapter.Rendering.Bgfx](Inno.Adapter.Rendering.Bgfx.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Adapter.Presentation](../platform/Inno.Adapter.Presentation.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Adapter.Presentation.ImGui.Sdl3](../platform/Inno.Adapter.Presentation.ImGui.Sdl3.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Rendering](Inno.Rendering.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：项目引用；公开签名可见性由语义边界检查确认。

共同 MSBuild 注入的 analyzer 与编译规则属于构建依赖，完整有效项目图记录在本轮验收证据中。
