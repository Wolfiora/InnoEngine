# Inno.Adapter.Rendering.Bgfx API

[项目入口](../Inno.Adapter.Rendering.Bgfx.md)

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
