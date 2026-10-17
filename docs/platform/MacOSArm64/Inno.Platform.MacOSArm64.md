# Inno.Platform.MacOSArm64

[分类索引](README.md) · [Wiki 首页](../../README.md) · [平台归属与扩展](../../architecture/PLATFORM_EXTENSION_GUIDE.md)

## 职责与边界

MacOS 实际 OS 应用位置规则。只回答应用内容位置和用户数据根，不实现内容、文件存储、输入或窗口 backend。

## 组合、生命周期与扩展

位置由平台产品使用并传给具体 Content/Storage/Log 服务。错误宿主或无效应用目录明确失败；无跨平台猜测、旧目录回退或第二份设置。

该服务不拥有 Session 或设备，不在领域程序集或脚本中暴露 OS 路径策略。生命周期为产品启动时解析一次，再注入明确 owner。

## 当前源码公开 API 清单

签名来自当前 SDK 求值与语义符号；按类型列出 public/protected 契约。工作流与生命周期见本页前文。

### `Inno.Platform.MacOSArm64.MacOSArm64ApplicationLocations`

<!-- api: T:Inno.Platform.MacOSArm64.MacOSArm64ApplicationLocations 13cedcf7de393eb7ac90739d7a192f55664d259978fd7823be2366e94e2313fb -->
<!-- api: P:Inno.Platform.MacOSArm64.MacOSArm64ApplicationLocations.userDataRoot 96fe8290263d5159d7b6fa6595a7561ae8b27b1a8f88902dd6b3bdcdd523bcbc -->
<!-- api: M:Inno.Platform.MacOSArm64.MacOSArm64ApplicationLocations.GetPlayerContentDirectory(System.String) 71c548741dfa37ee3b729a2f3bd83b40cc6400548c2ed45a29fff52edbdf7e9a -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8fa75851b0d1c75c"></a>[`public static string Inno.Platform.MacOSArm64.MacOSArm64ApplicationLocations.GetPlayerContentDirectory(string applicationDirectory)`](../../../platforms/MacOSArm64/runtime/Inno.Platform.MacOSArm64/MacOSArm64ApplicationLocations.cs#L45) | Resolves the sole content location in a published MacOS application layout.<br>返回：The explicit content directory, without probing alternative layouts.<br>`applicationDirectory`：The absolute product executable directory. | 约束由所属类型与本页生命周期定义。 | `System.ArgumentException`：The application directory is empty or relative. |
| <a id="api-89a7dd87e1c30fd6"></a>[`public static string Inno.Platform.MacOSArm64.MacOSArm64ApplicationLocations.userDataRoot { get; }`](../../../platforms/MacOSArm64/runtime/Inno.Platform.MacOSArm64/MacOSArm64ApplicationLocations.cs#L20) | Gets the current user's application data root according to MacOS system conventions. | 约束由所属类型与本页生命周期定义。 | `System.PlatformNotSupportedException`：The application is running on another operating system.<br>`System.InvalidOperationException`：The system did not provide a usable user location. |
| <a id="api-1114a22e1ae9665c"></a>[`public static class Inno.Platform.MacOSArm64.MacOSArm64ApplicationLocations`](../../../platforms/MacOSArm64/runtime/Inno.Platform.MacOSArm64/MacOSArm64ApplicationLocations.cs#L9) | Defines MacOS application layout and user data locations without implementing storage services. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

## 项目依赖

- [Inno.Extensibility.Modules](../../extensibility/Inno.Extensibility.Modules.md)：实现依赖，PrivateAssets="compile"。
