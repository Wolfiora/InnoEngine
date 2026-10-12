# Inno.Adapter.Platform.Sdl3

## 原生文件夹选择

`Sdl3FolderDialog.SelectFolder(Sdl3PlatformApplication application) : string?` 借用 live application，在其 owner thread 泵送事件并等待一次 native callback。
成功返回目录，取消返回 null，SDL 错误抛出 InvalidOperationException。callback owner 从打开到完成保持存活；不创建 Project、全局窗口目录或第二事件总线。
平台 Editor 在没有项目参数时创建短生命周期应用调用它，再进入正式 Host。

SDL 的进入后台/恢复前台通知转换为 Core Events 的 `ApplicationSuspensionChangedEvent`。
隐藏、显示、最小化和恢复通知转换为 `WindowVisibilityChangedEvent`；每个窗口分别保存隐藏与最小化原因，
不能用后一条通知错误清除另一原因。浏览器 document visibility 同样由 SDL 原始入口进入，领域服务无需认识浏览器。

[分类索引](README.md) · [Neutral contract](../../platform/Inno.Platform.md) · [SDL3 Native](Inno.Native.Sdl3.md) · [Wiki 首页](../../README.md)

## 公开 API

- `Sdl3PlatformApplication : IPlatformApplication`：SDL init、event pump、window ownership 和 extension composition。
- `Sdl3PlatformWindow : IPlatformWindow`：中立 window contract 的 SDL 实现。
- `ISdl3ApplicationExtension`：只供真实 SDL adapter extension 使用的生命周期接口。

Application/Window 的 SDL handle、event union 和 pointer 都停留在本程序集。`Sdl3PlatformWindow` 实现 Adapter SPI `INativeWindowSurface`，具体图形 Adapter 通过 `PlatformNativeHandles` 的开放 ABI ID 和借用句柄进行 surface interop；公共平台服务与产品宿主不依赖这些值或 SDL enum。

在浏览器宿主中，SDL 窗口标记为 `BrowserCanvas`，不尝试读取 Win32/Cocoa 指针。`windowHandle` 指向 SDL 窗口属性持有的 UTF-8 Canvas 选择器，图形后端只在窗口存活期间使用它。

## Composition provider

`Sdl3PlatformBackendProvider()` 只创建注册描述，不初始化原生服务。`CreateApplication()` 是继承的 provider 创建扩展点，返回调用方拥有的服务。`id` 来自所属领域的内置稳定 ID；同一 provider 可在 composition 生命周期内创建独立服务，具体线程及进程 owner 约束仍由该实现执行。

## 源码归属

当前唯一源码 owner：`backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Inno.Adapter.Platform.Sdl3.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 显式宿主接入

应用构造必须接收 ISdl3HostIntegration；没有当前 OS 默认策略。Sdl3HostCapabilities 声明多窗口和 live-resize。自建窗口归应用，AdoptWindow 是借用登记，ReleaseWindow 先失效后由原 owner 销毁；GetWindows 仅返回本应用窗口。窗口包装失败销毁新建窗口，借用包装失败不销毁外部窗口。

## 共同窗口操作

Sdl3WindowOperations.CreateWindow 使用统一 SDL properties 并总是释放临时 properties；成功 handle 的销毁权交给应用。ReadFocus 只读取。Windows/macOS 不要求 OpenGL，Browser 要求 OpenGL 且初始焦点继续等待真实事件；surface 解析/SDK 配置属于各 integration。应用唯一管理登记、owner thread、包装失败清理和外部窗口借用。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Adapter.Platform.Sdl3.ISdl3ApplicationExtension`

<!-- api: T:Inno.Adapter.Platform.Sdl3.ISdl3ApplicationExtension ae6296b9112751939fb5d8924d2ba51f0fcd155dc597b87266cce4cd99d6166c -->
<!-- api: M:Inno.Adapter.Platform.Sdl3.ISdl3ApplicationExtension.ProcessNativeEvent(Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication,System.ReadOnlySpan{System.Byte}) f6050c8400963100fd26b49b2fe96fe7ebfc14fccb0383258ed3c5b2adb9e6b4 -->
<!-- api: M:Inno.Adapter.Platform.Sdl3.ISdl3ApplicationExtension.PrepareLiveResizeWindow(Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication,System.UInt32) e419c6ea20cb083ad959f5bd3ed68f2216226a1f5c276cb4ccda1ec2b2199e36 -->
<!-- api: M:Inno.Adapter.Platform.Sdl3.ISdl3ApplicationExtension.OnApplicationDisposing(Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication) 815675f4758ab1d28ef0fcd8c3a969fc02169ab87522ef4fb9d4d0ef676a6cdc -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1c1baf347f8832eb"></a>[`void Inno.Adapter.Platform.Sdl3.ISdl3ApplicationExtension.OnApplicationDisposing(Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication application)`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/ISdl3ApplicationExtension.cs#L44) | Releases application-bound integration state before platform resources are destroyed.<br>`application`：The platform application being disposed. | — | — |
| <a id="api-9c52fec81a2e1639"></a>[`void Inno.Adapter.Platform.Sdl3.ISdl3ApplicationExtension.PrepareLiveResizeWindow(Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication application, uint windowId)`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/ISdl3ApplicationExtension.cs#L33) | Synchronizes integration window state before the host renders a complete live-resize frame.<br>`application`：The platform application requesting the redraw.<br>`windowId`：The platform window identifier being resized. | — | — |
| <a id="api-3fdf69d168198596"></a>[`void Inno.Adapter.Platform.Sdl3.ISdl3ApplicationExtension.ProcessNativeEvent(Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication application, scoped System.ReadOnlySpan<byte> nativeEventData)`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/ISdl3ApplicationExtension.cs#L19) | Processes one backend-native event before it is translated into an engine event.<br>`application`：The platform application dispatching the event.<br>`nativeEventData`：A read-only view over one SDL event. The span is valid only for this callback and must not be retained. | — | — |
| <a id="api-b11fe4018e52bcb9"></a>[`public interface Inno.Adapter.Platform.Sdl3.ISdl3ApplicationExtension`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/ISdl3ApplicationExtension.cs#L8) | Defines the synchronous SDL3 callbacks required by an SDL3-specific integration. | — | — |

### `Inno.Adapter.Platform.Sdl3.ISdl3HostIntegration`

<!-- api: T:Inno.Adapter.Platform.Sdl3.ISdl3HostIntegration 3ea43f36666a0e1ee37ce2cb3d6bccf92ffa7032e4dc798d89b6412bff59e3ef -->
<!-- api: P:Inno.Adapter.Platform.Sdl3.ISdl3HostIntegration.capabilities 300f015a40126f4edf4f14ee83c6e58cf9a7614bdba26c626af54f37cc4cf6a7 -->
<!-- api: M:Inno.Adapter.Platform.Sdl3.ISdl3HostIntegration.ConfigureInitialization 8182389d35e935eaf9a224e10c288fe457923bf09ecf945fb72617b7cbb99f9f -->
<!-- api: M:Inno.Adapter.Platform.Sdl3.ISdl3HostIntegration.CreateWindow(Inno.Platform.PlatformWindowOptions) edb3b400c7077c109a9ad15b442a320738747a9b2130a794f0fd13557d01a6f0 -->
<!-- api: M:Inno.Adapter.Platform.Sdl3.ISdl3HostIntegration.ResolveNativeSurface(System.IntPtr) 4a679bc4e56d864210fcb57c0e3ce48f265a8b44aa0547ac2189a4a73b65b0a7 -->
<!-- api: M:Inno.Adapter.Platform.Sdl3.ISdl3HostIntegration.GetInitialFocus(System.IntPtr) e9ecaa826893da21cc5de205606514bcfd66d215be24372cc1bd4a7037d7b240 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8123efc0bed1296e"></a>[`void Inno.Adapter.Platform.Sdl3.ISdl3HostIntegration.ConfigureInitialization()`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/ISdl3HostIntegration.cs#L23) | Applies host-specific SDK configuration before the application's SDL initialization. | — | — |
| <a id="api-e85d298888fe4487"></a>[`nint Inno.Adapter.Platform.Sdl3.ISdl3HostIntegration.CreateWindow(Inno.Platform.PlatformWindowOptions options)`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/ISdl3HostIntegration.cs#L34) | Creates one native window whose ownership transfers to the application on success.<br>返回：A nonzero opaque SDL window handle. Failure must throw without leaking a window.<br>`options`：The explicit window options supplied by the product. | — | — |
| <a id="api-887d0a8cea60a11c"></a>[`bool Inno.Adapter.Platform.Sdl3.ISdl3HostIntegration.GetInitialFocus(nint windowHandle)`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/ISdl3HostIntegration.cs#L56) | Reads the initial focus policy before platform events begin updating the wrapper.<br>返回：Whether game and editor input may initially treat the window as focused.<br>`windowHandle`：The live SDL window being registered. | — | — |
| <a id="api-21cab46df873824a"></a>[`Inno.Adapter.Platform.PlatformNativeHandles Inno.Adapter.Platform.Sdl3.ISdl3HostIntegration.ResolveNativeSurface(nint windowHandle)`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/ISdl3HostIntegration.cs#L45) | Resolves borrowed graphics surface information for a live SDL window.<br>返回：Borrowed surface handles valid until the native window is destroyed; unavailable surfaces fail.<br>`windowHandle`：The opaque handle created or explicitly adopted by the application. | — | — |
| <a id="api-1a6414a1a7e4c408"></a>[`Inno.Adapter.Platform.Sdl3.Sdl3HostCapabilities Inno.Adapter.Platform.Sdl3.ISdl3HostIntegration.capabilities { get; }`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/ISdl3HostIntegration.cs#L18) | Gets the host's supported window and scheduling behavior. | — | — |
| <a id="api-d29bc1dcef5f5475"></a>[`public interface Inno.Adapter.Platform.Sdl3.ISdl3HostIntegration`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/ISdl3HostIntegration.cs#L13) | Connects an explicitly selected system host to SDL without selecting platforms inside the backend. | The application borrows immutable integration configuration. Every operation runs on its owner thread. SDL handles are adapter-only identities and must never enter domain or scripting APIs. | — |

### `Inno.Adapter.Platform.Sdl3.Sdl3FolderDialog`

<!-- api: T:Inno.Adapter.Platform.Sdl3.Sdl3FolderDialog 81aa2f4f501e8df8ce89b9fee91bdbca6e6a9102a062e5629c6aa3882e9bf41e -->
<!-- api: M:Inno.Adapter.Platform.Sdl3.Sdl3FolderDialog.SelectFolder(Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication) 66d52efa751ac2abd34fe58b35de7ec77ad5805eaff31654092b2bc5589f2678 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2194ea5494ef16c5"></a>[`public static string? Inno.Adapter.Platform.Sdl3.Sdl3FolderDialog.SelectFolder(Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication application)`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3FolderDialog.cs#L30) | Selects one directory before a product creates its Editor session or rendering device.<br>返回：The selected directory, or null when the user cancels. No project is created or modified.<br>`application`：A live application borrowed on its owner thread for the entire modal operation. | — | `System.ArgumentNullException`：The application is absent.<br>`System.InvalidOperationException`：SDL reports a native dialog failure or the application is used outside its owner thread. |
| <a id="api-57d468f0cb0727f2"></a>[`public static class Inno.Adapter.Platform.Sdl3.Sdl3FolderDialog`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3FolderDialog.cs#L13) | Opens an SDL native directory dialog while the borrowed application pumps platform events. | — | — |

### `Inno.Adapter.Platform.Sdl3.Sdl3HostCapabilities`

<!-- api: T:Inno.Adapter.Platform.Sdl3.Sdl3HostCapabilities 026af47dfb2daa5f317301789cfe1e73708d81294fb38f9860e9c611963898a0 -->
<!-- api: M:Inno.Adapter.Platform.Sdl3.Sdl3HostCapabilities.#ctor(System.Boolean,System.Boolean) 1ca609552fa86010714dd6bd12b27e889d2f3979770e7310ce2d8c1592bf7c50 -->
<!-- api: P:Inno.Adapter.Platform.Sdl3.Sdl3HostCapabilities.multipleWindows c344ecbe3d323fc1703207c0cdf3e53390b72c243e28eb6ddafe20716d8985a0 -->
<!-- api: P:Inno.Adapter.Platform.Sdl3.Sdl3HostCapabilities.liveResize 860d7f9f832e82d4b316fa4ff341fbceccd8dc3c54307992bb8878d5329e764e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-82ce3d647a207f01"></a>[`public Inno.Adapter.Platform.Sdl3.Sdl3HostCapabilities.Sdl3HostCapabilities(bool multipleWindows, bool liveResize)`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3HostCapabilities.cs#L17) | Captures host capabilities without initializing SDL or creating windows.<br>`multipleWindows`：Whether the host supports more than one concurrently registered window.<br>`liveResize`：Whether native window move and resize loops require redraw callbacks. | — | — |
| <a id="api-c3ebb0e32afc632b"></a>[`public bool Inno.Adapter.Platform.Sdl3.Sdl3HostCapabilities.liveResize { get; }`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3HostCapabilities.cs#L33) | Gets whether the application installs the native live-resize scheduling integration. | — | — |
| <a id="api-cd293e5b8ba6096c"></a>[`public bool Inno.Adapter.Platform.Sdl3.Sdl3HostCapabilities.multipleWindows { get; }`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3HostCapabilities.cs#L28) | Gets whether the application can register additional native windows. | — | — |
| <a id="api-6367fe6f82f3145d"></a>[`public sealed class Inno.Adapter.Platform.Sdl3.Sdl3HostCapabilities`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3HostCapabilities.cs#L6) | Declares immutable window and frame scheduling capabilities supplied by the selected host. | — | — |

### `Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication`

<!-- api: T:Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication 68453ce2bd459a76f84b8d87bc4dbbc8c4cfe876851c7012ea6124a3346956b6 -->
<!-- api: M:Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication.AdoptWindow(System.IntPtr) 34bde0d236a0fe443deb3efe8d8b3b505fb8ce58b6594a2f1a0ebcec18ec6809 -->
<!-- api: M:Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication.ReleaseWindow(Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow) be699df7d8a2fcdbb3bc12f0bbc7d18ae77e4de137ba2c6ec48fdf00ea058786 -->
<!-- api: E:Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication.redrawRequested fb5b89b0b712f6ce0100c09d9d2fe42d8f622b449862950da790306c82f9ac9d -->
<!-- api: M:Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication.#ctor(Inno.Adapter.Platform.Sdl3.ISdl3HostIntegration) f7c97c3d5060ca05a00708b0a9e1e10b69f7d1b5a620378cec508dab26c92e81 -->
<!-- api: M:Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication.CreateWindow(Inno.Platform.PlatformWindowOptions) 8426bdb87365052d23f629f1b8bb5d303c62cf236e09da512e62f3fdbf81b75d -->
<!-- api: M:Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication.PollEvent(Inno.Core.Events.Event@) 9ffec1a78648b9ff49c65a6d2e36269e850e34b3340ceb98297af6751cb078b9 -->
<!-- api: M:Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication.GetWindows ce63e013ac1d42c1479ca88e133f2ccd1b0f8405fbe2e3fbdb36a6d6a606d194 -->
<!-- api: M:Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication.RegisterExtension(Inno.Adapter.Platform.Sdl3.ISdl3ApplicationExtension) 2559dee49864ca815687d9afb6f3d721e632618fcd59cc1224d1ca7c33d21dac -->
<!-- api: M:Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication.Dispose eb59b98d3d7292f2ba5edc920273196d327d1cf931fe56e9404a802ab5bda738 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-295648663dfda5f9"></a>[`public System.Action<uint>? Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication.redrawRequested`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformApplication.cs#L16) | Occurs after the observable redraw requested state changes. | — | — |
| <a id="api-f933197ced4adf53"></a>[`public Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication.Sdl3PlatformApplication(Inno.Adapter.Platform.Sdl3.ISdl3HostIntegration hostIntegration)`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformApplication.cs#L27) | Initializes platform subsystems required for windowing and input events.<br>`hostIntegration`：The borrowed immutable host configuration used for every window and native surface. | — | `System.ArgumentNullException`：Host configuration is absent. |
| <a id="api-487066d0dae21eee"></a>[`public Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication.AdoptWindow(nint windowHandle)`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Sdl3PlatformApplication.Windows.cs#L63) | Registers an externally owned window using this application's host and surface integration.<br>返回：A borrowed wrapper visible only in this application's window directory.<br>`windowHandle`：The live opaque SDL window handle; its original owner retains destruction responsibility. | — | `System.ArgumentException`：The handle is null or the window is already registered in this application.<br>`System.NotSupportedException`：The host does not support additional windows.<br>`System.InvalidOperationException`：Registration runs outside the owner thread or window inspection fails. |
| <a id="api-43146cd5e453577c"></a>[`public Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication.CreateWindow(Inno.Platform.PlatformWindowOptions options)`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformApplication.cs#L44) | Creates and registers a window through the borrowed host integration.<br>返回：A wrapper whose native window is owned by this application until disposal.<br>`options`：The logical size, visibility and title requested by the product. | — | `System.InvalidOperationException`：Creation or surface inspection fails, or execution is outside the owner thread.<br>`System.NotSupportedException`：The host does not support an additional window. |
| <a id="api-e736f83d6c114882"></a>[`public void Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication.Dispose()`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformApplication.cs#L86) | Releases the resources owned by this instance. | — | — |
| <a id="api-4c60ec48c46c2446"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow> Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication.GetWindows()`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformApplication.cs#L64) | Retrieves the requested windows value from current authoritative state.<br>返回：An immutable snapshot of the values selected by the operation. | — | — |
| <a id="api-af0800b03fbe6a2e"></a>[`public bool Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication.PollEvent(out Inno.Core.Events.Event? evnt)`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformApplication.cs#L55) | Attempts to dequeue the next backend-neutral platform event.<br>返回：true when the requested condition is satisfied; otherwise, false.<br>`evnt`：The evnt consumed by poll event; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-e1a9d2ea0f26319b"></a>[`public System.IDisposable Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication.RegisterExtension(Inno.Adapter.Platform.Sdl3.ISdl3ApplicationExtension extension)`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformApplication.cs#L81) | Registers a platform extension for the application lifetime.<br>返回：The validated idisposable that represents the completed operation.<br>`extension`：The extension consumed by register extension; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-c8cf041a703dcd08"></a>[`public void Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication.ReleaseWindow(Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow window)`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Sdl3PlatformApplication.Windows.cs#L84) | Invalidates and unregisters a borrowed window before its original owner destroys it.<br>`window`：The external wrapper returned by this application's adoption operation. | — | `System.ArgumentException`：The window is owned by SDL application creation or is not registered by this application.<br>`System.InvalidOperationException`：The operation runs outside the owner thread. |
| <a id="api-78200e7f14c62468"></a>[`public sealed class Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication : Inno.Platform.IPlatformApplication`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformApplication.cs#L11) | Platform runtime entry point responsible for window creation and platform event polling. | — | — |

### `Inno.Adapter.Platform.Sdl3.Sdl3PlatformBackendProvider`

<!-- api: T:Inno.Adapter.Platform.Sdl3.Sdl3PlatformBackendProvider 760f9b5c8a816fb8500a02297c93f781da41beb7d0affdbf2a99d028c7373215 -->
<!-- api: M:Inno.Adapter.Platform.Sdl3.Sdl3PlatformBackendProvider.#ctor(Inno.Adapter.Platform.Sdl3.ISdl3HostIntegration) 1a1a7df743c8d4456adc5908413388f4d251c3b3b55743d5459a35ae0cdcc2b6 -->
<!-- api: M:Inno.Adapter.Platform.Sdl3.Sdl3PlatformBackendProvider.CreateApplication 2fd1beb27269bf4a372f750fad65a6286113b33a6a996f4c89d7cec5482cbef6 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-93cd5184cecbf32d"></a>[`public Inno.Adapter.Platform.Sdl3.Sdl3PlatformBackendProvider.Sdl3PlatformBackendProvider(Inno.Adapter.Platform.Sdl3.ISdl3HostIntegration hostIntegration)`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Sdl3PlatformBackendProvider.cs#L22) | Creates an explicitly composed registration for the bundled implementation.<br>`hostIntegration`：The borrowed immutable system integration required by every created application. | — | `System.ArgumentNullException`：Host integration is absent. |
| <a id="api-15727256d32bdb42"></a>[`public override Inno.Platform.IPlatformApplication Inno.Adapter.Platform.Sdl3.Sdl3PlatformBackendProvider.CreateApplication()`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Sdl3PlatformBackendProvider.cs#L29) | Creates a caller-owned platform service using this implementation.<br>返回：A new service owned by the caller; providers must not return null. | — | — |
| <a id="api-e921f33c6cd13dda"></a>[`public sealed class Inno.Adapter.Platform.Sdl3.Sdl3PlatformBackendProvider : Inno.Adapter.Platform.PlatformBackendProvider`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Sdl3PlatformBackendProvider.cs#L9) | Supplies the Sdl3 implementation through the neutral platform creation boundary. | — | — |

### `Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow`

<!-- api: T:Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow 6417c146af3a4b704d077eeb56185a487c4bd85480abfb2318c66ec57d263f29 -->
<!-- api: P:Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.windowId 2ec598c5b09b4d540725c61f4a96f2ceda6c8039531fa197b9c3fc2b31a1ab3d -->
<!-- api: P:Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.title 30df8e230485d985db885063ddc68bcd415e8a361878230ee5b5339d8b4be21b -->
<!-- api: P:Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.width 8d4045a0d7b36a4d42af48c218ed15f13853ccb59378335cad97c72673cdb559 -->
<!-- api: P:Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.height dc9954a201d10cb9d2f96b68d6493622030e21a77c5fdc864b7606958e8e1e14 -->
<!-- api: P:Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.pixelWidth 07aa354f92fe75efc8e0f94c39bfa4ee146f08e05b3bd87ab219b2c5e83e2bec -->
<!-- api: P:Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.pixelHeight f2bba53e13335b3cf785a2ccb576a1195ab0a17941f1c8ee88b2ffbeceaf25b0 -->
<!-- api: P:Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.isClosed 436f05e4a04114df9757a733aa4ef0729d27a5e206366ccb81a8b2d1320fd3b8 -->
<!-- api: P:Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.isFocused ef74475c5c488f81c8335414d079401715d24e7e0957b9df214e2c32c9d7eea6 -->
<!-- api: P:Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.nativeHandles c5125afff2f2cd16e69b25840074f568055ad56ca3ae1f1e04140a10513a3bd4 -->
<!-- api: P:Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.sdlWindowHandle 578136b9f39d4252bf340f74ebb6115a6464a1c25ba8787666aad0b51e37efba -->
<!-- api: M:Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.RequestClose 2f28c5652f533d4cf9f2e0d0d2f51ae6e7dbd594eb133813dfd35ddad47bf539 -->
<!-- api: M:Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.Dispose ce218c69025a4deeed2e04d6acc2d4c0aa4a5d7e39f9f29cef700d8da21b93f1 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3f4ca3f6a4c240a1"></a>[`public void Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.Dispose()`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformWindow.cs#L83) | Releases the resources owned by this instance. | — | — |
| <a id="api-b5762052a71294fb"></a>[`public void Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.RequestClose()`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformWindow.cs#L78) | Queues a close request for processing at the next platform safety point. | — | — |
| <a id="api-f8027526ef46fdca"></a>[`public int Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.height { get; }`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformWindow.cs#L30) | Gets the current window height in platform-independent logical units. | — | — |
| <a id="api-a29cf215d555242c"></a>[`public bool Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.isClosed { get; }`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformWindow.cs#L45) | Gets whether this window has been marked as closed. | — | — |
| <a id="api-529601dbf11fe502"></a>[`public bool Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.isFocused { get; }`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformWindow.cs#L50) | Gets whether this window currently owns platform input focus. | — | — |
| <a id="api-7cdaccd3c7fdcc64"></a>[`public Inno.Adapter.Platform.PlatformNativeHandles Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.nativeHandles { get; }`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformWindow.cs#L58) | Gets native window handles for graphics backends and platform integration. | — | `System.ObjectDisposedException`：The window owner has released its native surface. |
| <a id="api-567bc0b8b7335fdb"></a>[`public int Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.pixelHeight { get; }`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformWindow.cs#L40) | Gets the current drawable height in physical pixels. | — | — |
| <a id="api-fd095466aed82a5c"></a>[`public int Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.pixelWidth { get; }`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformWindow.cs#L35) | Gets the current drawable width in physical pixels. | — | — |
| <a id="api-dc86ce109d208bf2"></a>[`public nint Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.sdlWindowHandle { get; }`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformWindow.cs#L73) | Gets the opaque SDL3 window identity used only by cooperating SDL3 adapter assemblies. | Runtime, Editor feature, and rendering contracts must use Inno.Platform.IPlatformWindow instead. | — |
| <a id="api-f3aac10c2590cfe3"></a>[`public string Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.title { get; }`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformWindow.cs#L20) | Gets the window title. | — | — |
| <a id="api-a51df2b7531f9a0c"></a>[`public int Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.width { get; }`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformWindow.cs#L25) | Gets the current window width in platform-independent logical units. | — | — |
| <a id="api-33549594baecdd42"></a>[`public uint Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow.windowId { get; }`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformWindow.cs#L15) | Gets the platform window identifier. | — | — |
| <a id="api-b8cc87840be7bd8d"></a>[`public sealed class Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow : Inno.Platform.IPlatformWindow, Inno.Adapter.Platform.INativeWindowSurface`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3PlatformWindow.cs#L10) | Represents a native platform window. | — | — |

### `Inno.Adapter.Platform.Sdl3.Sdl3WindowOperations`

<!-- api: T:Inno.Adapter.Platform.Sdl3.Sdl3WindowOperations 15e1ce6365d3c0681cde97a49a8d6cf5f55096f23b33e376cf8274f75582f688 -->
<!-- api: M:Inno.Adapter.Platform.Sdl3.Sdl3WindowOperations.CreateWindow(Inno.Platform.PlatformWindowOptions,System.Boolean) 2d9601130a6b85d3c8950c0824929c86b5544c6757a4d32c2554f330e8baf1fd -->
<!-- api: M:Inno.Adapter.Platform.Sdl3.Sdl3WindowOperations.ReadFocus(System.IntPtr) c8e3f7806e30e6a8cce7c11c7b0103e2d2bab2c371c865e37ef08e9f3b9aaba9 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-5a3bdc3e3a892423"></a>[`public static nint Inno.Adapter.Platform.Sdl3.Sdl3WindowOperations.CreateWindow(Inno.Platform.PlatformWindowOptions options, bool requireOpenGl)`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3WindowOperations.cs#L30) | Creates an SDL window and transfers its native destruction responsibility to the caller.<br>返回：A nonzero opaque SDL handle. Temporary creation properties are always released.<br>`options`：The title, logical extent, visibility, resizing and pixel density requested by the product.<br>`requireOpenGl`：Whether the selected host requires an OpenGL-capable window. | — | `System.ArgumentException`：The title is null or the logical extent is not positive.<br>`System.InvalidOperationException`：SDL could not allocate properties, apply an option or create the window. |
| <a id="api-8513762f50b84c53"></a>[`public static bool Inno.Adapter.Platform.Sdl3.Sdl3WindowOperations.ReadFocus(nint windowHandle)`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3WindowOperations.cs#L75) | Reads native focus without registering the window or changing its input state.<br>返回：Whether SDL currently reports keyboard focus for this window.<br>`windowHandle`：A live SDL window whose owner keeps it alive for this call. | — | `System.ArgumentException`：The handle is zero. |
| <a id="api-0adbdc085b94bc0e"></a>[`public static class Inno.Adapter.Platform.Sdl3.Sdl3WindowOperations`](../../../backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Api/Sdl3WindowOperations.cs#L10) | Implements shared SDL window operations without selecting a system host or registering windows. | — | — |

## 项目依赖

- [Inno.Native.Sdl3](Inno.Native.Sdl3.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Input](../../core/Inno.Core.Input.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Platform](../../platform/Inno.Adapter.Platform.md)：公开引用边界由实际签名核对。
- [Inno.Platform](../../platform/Inno.Platform.md)：公开引用边界由实际签名核对。
- [Inno.Core.Events](../../core/Inno.Core.Events.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
