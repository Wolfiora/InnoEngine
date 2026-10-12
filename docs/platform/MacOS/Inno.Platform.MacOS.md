# Inno.Platform.MacOS

[分类索引](README.md) · [Wiki 首页](../../README.md) · [平台归属与扩展](../../architecture/PLATFORM_EXTENSION_GUIDE.md)

## 职责与边界

MacOS 实际 OS 应用位置规则。只回答应用内容位置和用户数据根，不实现内容、文件存储、输入或窗口 backend。

## 组合、生命周期与扩展

位置由平台产品使用并传给具体 Content/Storage/Log 服务。错误宿主或无效应用目录明确失败；无跨平台猜测、旧目录回退或第二份设置。

该服务不拥有 Session 或设备，不在领域程序集或脚本中暴露 OS 路径策略。生命周期为产品启动时解析一次，再注入明确 owner。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Platform.MacOS.MacOSApplicationLocations`

<!-- api: T:Inno.Platform.MacOS.MacOSApplicationLocations 74d89e275938298993f06759856698562b6eeb9b267e09531bd026439e2d3220 -->
<!-- api: P:Inno.Platform.MacOS.MacOSApplicationLocations.userDataRoot d33e1c3919aebbf4b7b2aafc81ff9b7e296c1be11ad1e1f627f9bf422ec6ff08 -->
<!-- api: M:Inno.Platform.MacOS.MacOSApplicationLocations.GetPlayerContentDirectory(System.String) 60bf136b385063166b08aed71b3d28eb331259151a203f864c870c516f0d3e48 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a88f3500c4e4e376"></a>[`public static string Inno.Platform.MacOS.MacOSApplicationLocations.GetPlayerContentDirectory(string applicationDirectory)`](../../../platforms/MacOS/runtime/Inno.Platform.MacOS/MacOSApplicationLocations.cs#L45) | Resolves the sole content location in a published MacOS application layout.<br>返回：The explicit content directory, without probing alternative layouts.<br>`applicationDirectory`：The absolute product executable directory. | — | `System.ArgumentException`：The application directory is empty or relative. |
| <a id="api-32ad44b654c73af1"></a>[`public static string Inno.Platform.MacOS.MacOSApplicationLocations.userDataRoot { get; }`](../../../platforms/MacOS/runtime/Inno.Platform.MacOS/MacOSApplicationLocations.cs#L20) | Gets the current user's application data root according to MacOS system conventions. | — | `System.PlatformNotSupportedException`：The application is running on another operating system.<br>`System.InvalidOperationException`：The system did not provide a usable user location. |
| <a id="api-b626103c16696ef2"></a>[`public static class Inno.Platform.MacOS.MacOSApplicationLocations`](../../../platforms/MacOS/runtime/Inno.Platform.MacOS/MacOSApplicationLocations.cs#L9) | Defines MacOS application layout and user data locations without implementing storage services. | — | — |

## 项目依赖

- [Inno.Extensibility.Modules](../../extensibility/Inno.Extensibility.Modules.md)：实现依赖，PrivateAssets="compile"。
