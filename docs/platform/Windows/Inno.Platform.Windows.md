# Inno.Platform.Windows

[分类索引](README.md) · [Wiki 首页](../../README.md) · [平台归属与扩展](../../architecture/PLATFORM_EXTENSION_GUIDE.md)

## 职责与边界

Windows 实际 OS 应用位置规则。只回答应用内容位置和用户数据根，不实现内容、文件存储、输入或窗口 backend。

## 组合、生命周期与扩展

位置由平台产品使用并传给具体 Content/Storage/Log 服务。错误宿主或无效应用目录明确失败；无跨平台猜测、旧目录回退或第二份设置。

该服务不拥有 Session 或设备，不在领域程序集或脚本中暴露 OS 路径策略。生命周期为产品启动时解析一次，再注入明确 owner。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Platform.Windows.WindowsApplicationLocations`

<!-- api: T:Inno.Platform.Windows.WindowsApplicationLocations 9f6a7cf45841909940133b13b2925b6d565e78632a94aeb88901dc04fcd7d435 -->
<!-- api: P:Inno.Platform.Windows.WindowsApplicationLocations.userDataRoot 4f6428dd5407d6da537ee2f12f478d5818bd632eea586890d5df872864e54e59 -->
<!-- api: M:Inno.Platform.Windows.WindowsApplicationLocations.GetPlayerContentDirectory(System.String) 1e6a1b42a5ce3597f57f322642ce4969ae28ccf4ed1c47d83efa1aeed305cd0c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4b5ec469f4217c5a"></a>[`public static string Inno.Platform.Windows.WindowsApplicationLocations.GetPlayerContentDirectory(string applicationDirectory)`](../../../platforms/Windows/runtime/Inno.Platform.Windows/WindowsApplicationLocations.cs#L45) | Resolves the sole content location in a published Windows application layout.<br>返回：The explicit content directory, without probing alternative layouts.<br>`applicationDirectory`：The absolute product executable directory. | — | `System.ArgumentException`：The application directory is empty or relative. |
| <a id="api-48dfd08655deb0b7"></a>[`public static string Inno.Platform.Windows.WindowsApplicationLocations.userDataRoot { get; }`](../../../platforms/Windows/runtime/Inno.Platform.Windows/WindowsApplicationLocations.cs#L20) | Gets the current user's application data root according to Windows system conventions. | — | `System.PlatformNotSupportedException`：The application is running on another operating system.<br>`System.InvalidOperationException`：The system did not provide a usable user location. |
| <a id="api-c68533cdb4be2a47"></a>[`public static class Inno.Platform.Windows.WindowsApplicationLocations`](../../../platforms/Windows/runtime/Inno.Platform.Windows/WindowsApplicationLocations.cs#L9) | Defines Windows application layout and user data locations without implementing storage services. | — | — |

## 项目依赖

- [Inno.Extensibility.Modules](../../extensibility/Inno.Extensibility.Modules.md)：实现依赖，PrivateAssets="compile"。
