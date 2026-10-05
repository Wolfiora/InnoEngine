# Inno.Adapter.Presentation.ImGui.Bgfx

[Rendering 索引](README.md) · [BGFX](Inno.Adapter.Rendering.Bgfx.md) · [Platform ImGui](../platform/Inno.Adapter.Presentation.ImGui.Sdl3.md)

该 adapter 把 ImGui draw data 合成到 BGFX，不属于用户 Render Pipeline，也不进入普通游戏脚本 API。

内置 ImGui Shader Graph 声明 `s_tex` 纹理与 `outputEncoding` RenderPass uniform。主窗口与可执行 sRGB 写入的窗口直接在线性空间混合，再由窗口 framebuffer 编码。BGFX 的 Windows D3D11/D3D12 附加 swapchain 使用 UNORM RTV，不能对每个半透明图元提前编码，否则混合会落在错误的色彩空间；这类窗口先合成到 transient RGBA8 sRGB 纹理，再用一次全屏 Pass 仅在最终输出边界把合成后的线性 RGB 编码到 UNORM swapchain。两种路径共用同一 shader、纹理采样和 Alpha 混合契约。GPU 资源在 BGFX device 仍活跃时释放，窗口/viewport 关闭不直接销毁正在提交的资源。
