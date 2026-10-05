# Inno.Platform

[Platform 索引](README.md) · [Wiki 首页](../README.md) · [Platform Adapter](Inno.Adapter.Platform.md) · [Shell](../runtime/Inno.Shell.md)

## 职责与依赖

定义应用、窗口和帧呈现策略。只依赖 `Inno.Core.Events`；不包含 SDL3、原生指针、图形后端或 Editor 类型。平台实现属于 Adapter，产品生命周期由 Shell 组合。

## 全部公开 API

| 类型及成员 | 当前契约 |
| --- | --- |
| `IPlatformApplication : IDisposable` | 一个平台会话的窗口、事件泵和资源 owner。 |
| `CreateWindow(PlatformWindowOptions)` | 创建会话拥有的窗口；调用方可以提前释放窗口。已释放的 application 抛出 `ObjectDisposedException`。 |
| `PollEvent(out Event?)` | 取出下一条中立事件；无事件返回 false，输出 null。已释放时抛出 `ObjectDisposedException`。 |
| `GetWindows()` | 当前有效窗口的快照；包含 implementation 跟踪的附加视口。已释放时抛出 `ObjectDisposedException`。 |
| `redrawRequested : Action<uint>` | 在事件泵线程请求一次完整宿主帧；处理方不得重新轮询或重入已执行的帧。 |
| `IPlatformWindow : IDisposable` | 公开 `windowId`、`title`、`width`、`height`、`pixelWidth`、`pixelHeight`、`isClosed`、`isFocused`。不携带原生 ABI。 |
| `IPlatformWindow.RequestClose()` | 标记关闭请求，不立即销毁窗口。 |
| `PlatformWindowOptions()` | 初始化 `title = "Inno Window"`、`width = 1280`、`height = 720`、`resizable = true`、`highPixelDensity = true`、`visible = true`。这些属性为 init-only。 |
| `FramePacingOptions()` | 每个 Shell 独立的可变呈现策略。 |
| `FramePacingOptions.verticalSync` | 在完整帧的安全点应用显示同步策略。 |
| `FramePacingOptions.maximumFrameRate` | 非负帧率上限；0 无软件限速，负值抛出 `ArgumentOutOfRangeException`。 |

这两个接口通过实现提供扩展点，没有 protected 成员。原生事件扩展通过具体 Adapter 的 SPI 注册，不属于本项目。

## 创建与退出

```csharp
using Inno.Adapter.Platform;
using Inno.Core.Events;
using Inno.Platform;

static void PumpOnce(IPlatformBackendFactory factory)
{
    using IPlatformApplication application = factory.CreateApplication(PlatformBackendId.sdl3);
    using IPlatformWindow window = application.CreateWindow(new PlatformWindowOptions
    {
        title = "Example",
        width = 1280,
        height = 720
    });
    while (application.PollEvent(out Event? e))
    {
        if (e is ApplicationQuitEvent)
            window.RequestClose();
    }
}
```

示例展示公开组合边界。实际 Player 与 Editor 由 Shell 管理事件泵、Input、Rendering、帧调度和退出，业务代码不另建循环。正常退出先停止产品工作，再释放渲染与输入、窗口，最后 application。每个资源只由其 owner 最终释放。

## 尺寸、焦点和系统生命周期

`width` / `height` 是布局与输入使用的逻辑尺寸；`pixelWidth` / `pixelHeight` 是 GPU drawable 像素。HiDPI 下二者可以不同。逻辑 resize 和 pixel resize 分别沿 Core Events 通知。

焦点由 `WindowFocusChangedEvent` 通知。可见性由 `WindowVisibilityChangedEvent` 通知；应用进入后台由 `ApplicationSuspensionChangedEvent` 通知。Shell 合并暂停原因，停帧并释放输入状态；恢复不会把后台经过的时间补成一大帧。隐藏验收窗口是否暂停由 Shell 的显式 `suspendWhenHidden` 策略决定。

原生渲染集成通过 [Adapter 的 `INativeWindowSurface`](Inno.Adapter.Platform.md) 获取借用句柄。中立窗口 API、脚本和持久化状态均不保存这些句柄。布局持久化属于 Editor presentation，见 [Editor ImGui](../editor/Inno.Editor.ImGui.md)。

## 扩展与验收

新增平台实现 `IPlatformApplication` / `IPlatformWindow` 并在 composition 注册 provider；公共服务不增加平台枚举或判断分支。窗口创建、尺寸、焦点、暂停和退出由公开边界验证；Shell 的替换 Adapter 测试覆盖多暂停原因、取消、失败清理和回调注销。实机图形 surface 和系统生命周期须在对应平台独立验证。
