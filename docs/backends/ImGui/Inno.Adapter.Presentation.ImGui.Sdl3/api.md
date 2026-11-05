# Inno.Adapter.Presentation.ImGui.Sdl3 API

[项目入口](../Inno.Adapter.Presentation.ImGui.Sdl3.md)

## 公开 API

Viewport 的位置与逻辑尺寸由 ImGui 的平台 Move/Resize 请求及 getter/setter 协议协调。
每帧的 drawable metrics 同步只更新 FramebufferScale、初始 DPI、像素尺寸与 renderer 输出，
不回写 Pos/WorkPos/Size/WorkSize，也不根据二者的暂时差异制造 Move/Resize 请求。
SDL 的真实窗口事件仍通过同一 Application extension 入口发出平台请求，避免拖动过程中把新位置拉回旧位置。

- `PlatformImGuiContext`、`ImGuiContextFlags`：ImGui/viewport session ownership。
- `PlatformImGuiContext.TryGetWindowId`：把主 viewport 或独立 viewport 的 ImGui ID 映射到当前 SDL window ID；窗口销毁后返回 `false`。
- `IPlatformImGuiRenderer`, `PlatformImGuiViewportTarget`, `ImGuiTextureHandle`：平台与 renderer bridge。
- `Sdl3PlatformApplicationImGuiExtensions`：在 SDL3 Application 上组合 ImGui。
- `ImGuiFont`, `ImGuiFontStyle`, `ImGuiIcon`：Editor presentation assets。

该项目属于 Editor deployment，不进入 Player。Context 必须在 SDL3 Application 活跃期间 Dispose；viewport 与 GPU renderer 的 retiring resources 在帧安全点释放。

每个 `PlatformImGuiContext` 独立拥有自己的 viewport、SDL window 与 renderer target 映射。Native
ImGui callback 只使用当前 `ImGuiContext` 查找 owner backend；创建第二个 context 不会覆盖第一个
context 的路由，Dispose 只注销精确 owner。该 router 保存 Host-owned backend，不保存 Plugin 类型、
实例或 delegate，因此不会延长 collectible extension generation。跨窗口拖拽时 SDL mouse capture 仅用于保持同一次按下/释放事件完整；释放鼠标不主动改变窗口焦点或层级。只有操作系统焦点事件或 ImGui 显式的 `PlatformSetWindowFocus` 请求会改变 native 窗口焦点。

这项 context 隔离不改变 BGFX 的进程约束：平台层可以正确管理多个 ImGui context，但当前 BGFX
Adapter 同一进程只允许一个活动设备。Editor Application 是唯一图形 composition root；若未来要
同时运行多个图形 Host，应选择多进程隔离或实现真正支持多 device 的另一个 Rendering backend。
