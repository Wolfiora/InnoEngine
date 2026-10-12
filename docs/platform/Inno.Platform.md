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
using Inno.Core.Events;
using Inno.Platform;

static void PumpOnce(IPlatformApplication application)
{
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

示例借用已创建的 application，只拥有本次创建的窗口；具体后端由 [Platform Adapter](Inno.Adapter.Platform.md) 工厂在组合入口选择。实际 Player 与 Editor 由 Shell 管理事件泵、Input、Rendering、帧调度和退出，业务代码不另建循环。正常退出先停止产品工作，再释放渲染与输入、窗口，最后 application。每个资源只由其 owner 最终释放。

## 尺寸、焦点和系统生命周期

`width` / `height` 是布局与输入使用的逻辑尺寸；`pixelWidth` / `pixelHeight` 是 GPU drawable 像素。HiDPI 下二者可以不同。逻辑 resize 和 pixel resize 分别沿 Core Events 通知。

焦点由 `WindowFocusChangedEvent` 通知。可见性由 `WindowVisibilityChangedEvent` 通知；应用进入后台由 `ApplicationSuspensionChangedEvent` 通知。Shell 合并暂停原因，停帧并释放输入状态；恢复不会把后台经过的时间补成一大帧。隐藏验收窗口是否暂停由 Shell 的显式 `suspendWhenHidden` 策略决定。

原生渲染集成通过 [Adapter 的 `INativeWindowSurface`](Inno.Adapter.Platform.md) 获取借用句柄。中立窗口 API、脚本和持久化状态均不保存这些句柄。布局持久化属于 Editor presentation，见 [Editor ImGui](../editor/Inno.Editor.ImGui.md)。

## 扩展与验收

新增平台实现 `IPlatformApplication` / `IPlatformWindow` 并在 composition 注册 provider；公共服务不增加平台枚举或判断分支。窗口创建、尺寸、焦点、暂停和退出由公开边界验证；Shell 的替换 Adapter 测试覆盖多暂停原因、取消、失败清理和回调注销。实机图形 surface 和系统生命周期须在对应平台独立验证。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Platform.FramePacingOptions`

<!-- api: T:Inno.Platform.FramePacingOptions 92b43b439d5e13ae023bedd8a662029c563d84bd8ed51159895ebe713c715df3 -->
<!-- api: P:Inno.Platform.FramePacingOptions.verticalSync 1e186660257f7ec48d5d05bb9df682119fa983e0345dffa58fb3eed62f678d6e -->
<!-- api: P:Inno.Platform.FramePacingOptions.maximumFrameRate 143136c3466d3fabbb87ea32812550328da8ff2a7802ded64aaf91bcb175d50d -->
<!-- api: M:Inno.Platform.FramePacingOptions.#ctor 2ab231ca399ba3c7c6fc166eb6a0c3122c44cfbc3685ad68927f888b690ecc35 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-6df1ace530fe2350"></a>[`public Inno.Platform.FramePacingOptions.FramePacingOptions()`](../../src/services/platform/Inno.Platform/FramePacingOptions.cs#L8) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-ef6bedbb76004e38"></a>[`public int Inno.Platform.FramePacingOptions.maximumFrameRate { get; set; }`](../../src/services/platform/Inno.Platform/FramePacingOptions.cs#L23) | Gets or sets the software frame-rate ceiling. Zero leaves the frame rate unlimited. | — | `System.ArgumentOutOfRangeException`：The value is negative. |
| <a id="api-f91aaee63f0389ef"></a>[`public bool Inno.Platform.FramePacingOptions.verticalSync { get; set; }`](../../src/services/platform/Inno.Platform/FramePacingOptions.cs#L15) | Gets or sets whether presentation waits for display synchronization. | — | — |
| <a id="api-2ab34d297d9deb35"></a>[`public sealed class Inno.Platform.FramePacingOptions`](../../src/services/platform/Inno.Platform/FramePacingOptions.cs#L8) | Defines host presentation cadence independently of simulation and fixed-step timing. | — | — |

### `Inno.Platform.IPlatformApplication`

<!-- api: T:Inno.Platform.IPlatformApplication 6e271827ad48a94e23ed8193400adfa541b2e120103ce68ac7f2be6e04815b36 -->
<!-- api: E:Inno.Platform.IPlatformApplication.redrawRequested da8f49bae4842d65a2e673620f68e80a07c3ca14326aeddfdfdf96f67863ad68 -->
<!-- api: M:Inno.Platform.IPlatformApplication.CreateWindow(Inno.Platform.PlatformWindowOptions) ed3d8e9bd8a0adef1107e0d9e7b9c54757bcb3e3c059142f94e030c480db5b09 -->
<!-- api: M:Inno.Platform.IPlatformApplication.PollEvent(Inno.Core.Events.Event@) da5ced128c064a0cba259b1f7e198d88526031de55e84b14f3b616f511afe368 -->
<!-- api: M:Inno.Platform.IPlatformApplication.GetWindows 079c7a0e0bd4a0b5e4e84a7b0f66db97bb88e45133be93f54f367b0f6edaff51 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b6cee2d6f0c1a5ba"></a>[`System.Action<uint>? Inno.Platform.IPlatformApplication.redrawRequested`](../../src/services/platform/Inno.Platform/IPlatformApplication.cs#L16) | Requests one complete host frame on the event-pump thread while the operating system owns a modal window resize loop. Handlers must not poll events or reenter an active frame. | — | — |
| <a id="api-b8e711d55426817d"></a>[`Inno.Platform.IPlatformWindow Inno.Platform.IPlatformApplication.CreateWindow(Inno.Platform.PlatformWindowOptions options)`](../../src/services/platform/Inno.Platform/IPlatformApplication.cs#L30) | Creates a window owned by this platform session.<br>返回：The newly created window. The caller may dispose it before disposing the application.<br>`options`：The validated initial window properties. | — | `System.ObjectDisposedException`：Thrown when the platform application has already been disposed. |
| <a id="api-97cbf9cefd01b1bf"></a>[`System.Collections.Generic.IReadOnlyList<Inno.Platform.IPlatformWindow> Inno.Platform.IPlatformApplication.GetWindows()`](../../src/services/platform/Inno.Platform/IPlatformApplication.cs#L55) | Captures the currently valid windows owned or tracked by this platform session.<br>返回：An immutable-by-convention snapshot of the current windows. | — | `System.ObjectDisposedException`：Thrown when the platform application has already been disposed. |
| <a id="api-17446ca6c565e2c5"></a>[`bool Inno.Platform.IPlatformApplication.PollEvent(out Inno.Core.Events.Event? evnt)`](../../src/services/platform/Inno.Platform/IPlatformApplication.cs#L44) | Attempts to dequeue the next backend-neutral platform event.<br>返回：true when an event was returned; otherwise, false.<br>`evnt`：The translated event when one is available; otherwise, null. | — | `System.ObjectDisposedException`：Thrown when the platform application has already been disposed. |
| <a id="api-5c7ce70fc414f6ba"></a>[`public interface Inno.Platform.IPlatformApplication : System.IDisposable`](../../src/services/platform/Inno.Platform/IPlatformApplication.cs#L10) | Defines the backend-neutral lifetime, window creation, and event-polling contract for a platform session. | — | — |

### `Inno.Platform.IPlatformWindow`

<!-- api: T:Inno.Platform.IPlatformWindow 25b505eb1978162e7bd213c903f06f51545c9ce0e673ab11b64c229e7039770d -->
<!-- api: P:Inno.Platform.IPlatformWindow.windowId ca3c238e2d9312b985edc8809282ec91dd4429b54b92c32afb38b8f0ca64e4ae -->
<!-- api: P:Inno.Platform.IPlatformWindow.title fb68cd2b4b13f070f7468db56f11e0ce5deaeb069c4facfe8bae3c19c61fe124 -->
<!-- api: P:Inno.Platform.IPlatformWindow.width aa3b06f75a12cd3225078e24346ca673ee08af5f9dc275aa8f6de37b575ad1d4 -->
<!-- api: P:Inno.Platform.IPlatformWindow.height 1d204722c997a97aa8ff6d12b7b51e1b3ec32d626c4062dea1fb6aeafe43aa16 -->
<!-- api: P:Inno.Platform.IPlatformWindow.pixelWidth 15f6f2065802c169a951e1fc816e00da0747e49c39a13daa59487ea002ad867f -->
<!-- api: P:Inno.Platform.IPlatformWindow.pixelHeight 2e7494b7e43815d5b493a73d007107039812be61c256c2a33c60c6055528880e -->
<!-- api: P:Inno.Platform.IPlatformWindow.isClosed 96921df2d0d4fe2da199135d5996a126759bb37ff626044995666b0fcaf5219e -->
<!-- api: P:Inno.Platform.IPlatformWindow.isFocused 04926ba655ff7ee838c703af63f656f5c733d97b5d20ed46466b5e6009add358 -->
<!-- api: M:Inno.Platform.IPlatformWindow.RequestClose 335c840ec205bdff18b42f43c56335bca532606699fb2c3766b14d589779c8fb -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a94f61659f5580ee"></a>[`void Inno.Platform.IPlatformWindow.RequestClose()`](../../src/services/platform/Inno.Platform/IPlatformWindow.cs#L53) | Marks the window as requesting closure without destroying it immediately. | — | — |
| <a id="api-c229cf29773defc5"></a>[`int Inno.Platform.IPlatformWindow.height { get; }`](../../src/services/platform/Inno.Platform/IPlatformWindow.cs#L28) | Gets the logical client height. | — | — |
| <a id="api-147e739a87694d51"></a>[`bool Inno.Platform.IPlatformWindow.isClosed { get; }`](../../src/services/platform/Inno.Platform/IPlatformWindow.cs#L43) | Gets whether the window has received or requested a close operation. | — | — |
| <a id="api-842a7f0164f7fc41"></a>[`bool Inno.Platform.IPlatformWindow.isFocused { get; }`](../../src/services/platform/Inno.Platform/IPlatformWindow.cs#L48) | Gets whether the window currently owns platform input focus. | — | — |
| <a id="api-f5d693043f934321"></a>[`int Inno.Platform.IPlatformWindow.pixelHeight { get; }`](../../src/services/platform/Inno.Platform/IPlatformWindow.cs#L38) | Gets the drawable height in physical pixels. | — | — |
| <a id="api-f959b252ef9332db"></a>[`int Inno.Platform.IPlatformWindow.pixelWidth { get; }`](../../src/services/platform/Inno.Platform/IPlatformWindow.cs#L33) | Gets the drawable width in physical pixels. | — | — |
| <a id="api-ddaacf8fa44ff4c7"></a>[`string Inno.Platform.IPlatformWindow.title { get; }`](../../src/services/platform/Inno.Platform/IPlatformWindow.cs#L18) | Gets the title captured when the window was created. | — | — |
| <a id="api-14cd4118f9481def"></a>[`int Inno.Platform.IPlatformWindow.width { get; }`](../../src/services/platform/Inno.Platform/IPlatformWindow.cs#L23) | Gets the logical client width. | — | — |
| <a id="api-f09157c3208a9cf9"></a>[`uint Inno.Platform.IPlatformWindow.windowId { get; }`](../../src/services/platform/Inno.Platform/IPlatformWindow.cs#L13) | Gets the application-local identifier of this window. | — | — |
| <a id="api-7895aefc5837fac5"></a>[`public interface Inno.Platform.IPlatformWindow : System.IDisposable`](../../src/services/platform/Inno.Platform/IPlatformWindow.cs#L8) | Represents a backend-neutral native window owned by a platform application. | — | — |

### `Inno.Platform.PlatformWindowOptions`

<!-- api: T:Inno.Platform.PlatformWindowOptions 92d282015a8f2176b743f4a100031216084983e945bf48c753db6900761da753 -->
<!-- api: M:Inno.Platform.PlatformWindowOptions.#ctor 43aed5f19cd1b2ecaf7dbc55a971fe2edd54594d8af8b19b60a10ef02c54eae8 -->
<!-- api: P:Inno.Platform.PlatformWindowOptions.title e444ab8c7e51619d570092fe802a2c28f77bf14531627e3e3a3850a6c4c6f44d -->
<!-- api: P:Inno.Platform.PlatformWindowOptions.width 047284322b8f5e8249103f566f2213d1bcd6e4643f1579c34c3e7ec38d7769d3 -->
<!-- api: P:Inno.Platform.PlatformWindowOptions.height e6400c1d4a19c46a3248bd65ca653413c3d3c5f275ed792abf2ab091ad5bbd75 -->
<!-- api: P:Inno.Platform.PlatformWindowOptions.resizable 81c85ca2f9e25c2f5beb438f8da5280bbb24a22f1f965cdeb480d66941fa7150 -->
<!-- api: P:Inno.Platform.PlatformWindowOptions.highPixelDensity e9f5d5914984967591e17981012516e4b97063f0cc34dde0a20c955dc1f6a2eb -->
<!-- api: P:Inno.Platform.PlatformWindowOptions.visible 8b285e405a5ad9cc327b417574fbd28772e6a63401f34848d9d72971dd27cc16 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4c3cb042dc8164db"></a>[`public Inno.Platform.PlatformWindowOptions.PlatformWindowOptions()`](../../src/services/platform/Inno.Platform/PlatformWindowOptions.cs#L9) | Defines backend-neutral properties used to create a platform window.<br>返回：A validated immutable window description. | — | — |
| <a id="api-1ab0286a6d475e70"></a>[`public int Inno.Platform.PlatformWindowOptions.height { get; init; }`](../../src/services/platform/Inno.Platform/PlatformWindowOptions.cs#L24) | Gets the initial window height in platform-independent logical units. | — | — |
| <a id="api-5d5991a7e7f8e1c4"></a>[`public bool Inno.Platform.PlatformWindowOptions.highPixelDensity { get; init; }`](../../src/services/platform/Inno.Platform/PlatformWindowOptions.cs#L34) | Gets whether high pixel density is requested for the window. | — | — |
| <a id="api-c42a0306bf7fb05d"></a>[`public bool Inno.Platform.PlatformWindowOptions.resizable { get; init; }`](../../src/services/platform/Inno.Platform/PlatformWindowOptions.cs#L29) | Gets whether the window is user-resizable. | — | — |
| <a id="api-cb3a229bd9d25339"></a>[`public string Inno.Platform.PlatformWindowOptions.title { get; init; }`](../../src/services/platform/Inno.Platform/PlatformWindowOptions.cs#L14) | Gets the window title. | — | — |
| <a id="api-666852d6be8ca18b"></a>[`public bool Inno.Platform.PlatformWindowOptions.visible { get; init; }`](../../src/services/platform/Inno.Platform/PlatformWindowOptions.cs#L39) | Gets whether the window is initially shown; hidden windows retain their rendering surface. | — | — |
| <a id="api-2cc448d97c1ebab6"></a>[`public int Inno.Platform.PlatformWindowOptions.width { get; init; }`](../../src/services/platform/Inno.Platform/PlatformWindowOptions.cs#L19) | Gets the initial window width in platform-independent logical units. | — | — |
| <a id="api-5f26ef4ef9de4a6b"></a>[`public readonly struct Inno.Platform.PlatformWindowOptions`](../../src/services/platform/Inno.Platform/PlatformWindowOptions.cs#L9) | Defines backend-neutral properties used to create a platform window.<br>返回：A validated immutable window description. | — | — |

## 项目依赖

- [Inno.Core.Events](../core/Inno.Core.Events.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
