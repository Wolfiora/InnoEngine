# Inno.Editor.Panel.GameView

[Editor 索引](README.md) · [Editor Rendering](Inno.Editor.Rendering.md) · [Scene View](Inno.Editor.Panel.SceneView.md)

Game View 是开放 kind `inno.editor.viewport.game` 的通用 viewport host。Panel 不查找内建 Camera，也不解释 Scene。Panel 从 `IEditorGameScenePresentation` 捕获内容范围，并提供输出尺寸、viewport 和指针输入。匹配的输出贡献者把这些数据交给 `IRenderModel`，由模型决定内容采集、帧数据与图构建。

Panel 在单次绘制期间借用 `EditorPlayModeLoop.EnterPresentationScope()`：Playing 时，内容采集和交互使用同一 Play Session 的 Identity、UI 和音频服务；退出绘制立即恢复 Edit 作用域。Editor 模块更新、脚本激活和重载事务不进入这个呈现作用域。

Play Mode 使用与 Edit Session 分离的 Input backend。Game View 每帧向 `EditorGameInputCapture` 报告所属 presentation viewport、画面矩形、ImGui 前景悬停状态与焦点；Host 通过 `IPresentationContext.TryGetWindowId` 解析主窗口或独立窗口。键盘与文本只在 Game View 获得焦点时进入 Play Session；鼠标仅在画面处于前景并被命中时开始输入，按住后的移动与释放保持完整。失焦、隐藏或关闭 Game View 时发送焦点释放事件，清空已按下的按键和鼠标按钮。渲染模型收到的 `RenderOutputInput` 使用同一焦点与命中条件。

`IViewContentSource` 负责提供可采集内容；输出贡献者负责提出视图请求；`IRenderModel` 负责将请求与内容组合成渲染工作。只有内容来源而没有匹配的输出贡献者或模型时，Game View 显示无可用输出状态。多个模型接受同一个 Game View 输出时必须指定 `RenderOutputRoute`，否则显示诊断。

`IEditorGameScenePresentation` 的 owner 是 `Inno.Editor.Scene`。Editing、Compiling 和尚未提交完成的 Preparing 阶段返回 Edit Session；Play Scene 全部物化成功后一次切换到隔离 Runtime Session；停止时先切回 Edit Session，再释放 Play 世界。Game View、Scene View、Hierarchy、Inspector、Selection 与 Gizmo 因此观察并操作同一个脚本驱动 runtime graph。Play workspace 禁止持久化且使用独立 History 分支，所以这些临时修改不会污染 Edit 文档；Panel 不访问 `RuntimeSession`，PlayMode 也不依赖 Rendering。

内容尺寸变化会在渲染安全点 resize `RenderTexture`。输出通过 presentation 合成到界面，不做 CPU 回读。`Editor/Appearance/Viewports/Game Background` 是 Game View 的默认背景色，并通过中立 `EditorViewportPresentation` 交给渲染模型。

`Project/Player/Presentation` 编辑项目级 `GamePresentationSettings`，由 Game View 与导出 Player 共同消费。默认参考帧为 `1280×720` 且开启 `Preserve Aspect Ratio`：Panel 先在可用区域内计算完整可见的最大内接矩形，以该内矩形对应的物理像素尺寸申请 composition target，再把输出居中合成，剩余区域绘制纯黑 letterbox/pillarbox。关闭后 target 直接跟随 Panel 完整尺寸。该流程不会先把图像拉伸再遮罩，因此各模型投影使用的 aspect 与最终显示严格一致；同一项目设置进入部署内容，Player resize 后也计算完全相同的 content viewport。

在高像素密度显示器上，Panel 仍以 ImGui 逻辑单位计算可见区域，却按 framebuffer scale 为 composition target 分配物理像素。`EditorViewportPresentation.pixelDensity` 把这一比例交给 Contributor；布局类内容可保持原有逻辑大小，同时按物理分辨率栅格化。Panel 绘制输出时使用逻辑区域大小。

无适用 Contributor、模型争用、贡献失败或 GPU target 尚未准备好时，Panel 使用背景色填充完整区域，并在中央显示状态；不可用输出会被释放，不影响 Editor 主界面。

Editor 的通用 `RenderRuntime` 不注入任何隐式 Scene content。Scene View 与 Game View 都必须显式提交自己的 `RenderContentScope`，因此不会在 viewport 请求之外额外把 Edit 世界渲染到 Editor backbuffer，也不存在同一帧两个互相矛盾的游戏世界来源。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

本项目没有公开入口，通过组合或扩展发现使用内部实现。

## 项目依赖

- [Inno.Native.ImGui](../backends/ImGui/Inno.Native.ImGui.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Settings](../core/Inno.Core.Settings.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Runtime](../runtime/Inno.Runtime.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Editor.Core](Inno.Editor.Core.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Editor.ImGui](Inno.Editor.ImGui.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Editor.Rendering](Inno.Editor.Rendering.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Editor.PlayMode](Inno.Editor.PlayMode.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Editor.Scene](Inno.Editor.Scene.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Editor.Settings](Inno.Editor.Settings.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Rendering](../rendering/Inno.Rendering.md)：实现依赖，PrivateAssets="compile"。
- [Inno.References](../references/Inno.References.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Rendering.Runtime](../rendering/Inno.Rendering.Runtime.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
