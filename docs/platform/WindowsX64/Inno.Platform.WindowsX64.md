# Inno.Platform.WindowsX64

[分类索引](README.md) · [Wiki 首页](../../README.md) · [平台归属与扩展](../../architecture/PLATFORM_EXTENSION_GUIDE.md)

## 职责与边界

Windows 实际 OS 应用位置规则。只回答应用内容位置和用户数据根，不实现内容、文件存储、输入或窗口 backend。

## 组合、生命周期与扩展

位置由平台产品使用并传给具体 Content/Storage/Log 服务。错误宿主或无效应用目录明确失败；无跨平台猜测、旧目录回退或第二份设置。

该服务不拥有 Session 或设备，不在领域程序集或脚本中暴露 OS 路径策略。生命周期为产品启动时解析一次，再注入明确 owner。

## 当前源码公开 API 清单

签名来自当前 SDK 求值与语义符号；按类型列出 public/protected 契约。工作流与生命周期见本页前文。

### `Inno.Platform.WindowsX64.WindowsX64ApplicationLocations`

<!-- api: T:Inno.Platform.WindowsX64.WindowsX64ApplicationLocations 9c2c3039d594bc60e7bb48d22967d5b8646cbd1aa9abe3181dd0fcab40181c83 -->
<!-- api: P:Inno.Platform.WindowsX64.WindowsX64ApplicationLocations.userDataRoot 82e31f9a536519fa0cbe7cdc85bf8fa0206ce7a44fe176c99e75bb530132035f -->
<!-- api: M:Inno.Platform.WindowsX64.WindowsX64ApplicationLocations.GetPlayerContentDirectory(System.String) 976b9f70ce377ce3c119b46e6792e612cb3e4a9d8211351ffcbb6eb94171926f -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d27851c77cf45965"></a>[`public static string Inno.Platform.WindowsX64.WindowsX64ApplicationLocations.GetPlayerContentDirectory(string applicationDirectory)`](../../../platforms/WindowsX64/runtime/Inno.Platform.WindowsX64/WindowsX64ApplicationLocations.cs#L45) | Resolves the sole content location in a published Windows application layout.<br>返回：The explicit content directory, without probing alternative layouts.<br>`applicationDirectory`：The absolute product executable directory. | 约束由所属类型与本页生命周期定义。 | `System.ArgumentException`：The application directory is empty or relative. |
| <a id="api-1db45ff1aa055cda"></a>[`public static string Inno.Platform.WindowsX64.WindowsX64ApplicationLocations.userDataRoot { get; }`](../../../platforms/WindowsX64/runtime/Inno.Platform.WindowsX64/WindowsX64ApplicationLocations.cs#L20) | Gets the current user's application data root according to Windows system conventions. | 约束由所属类型与本页生命周期定义。 | `System.PlatformNotSupportedException`：The application is running on another operating system.<br>`System.InvalidOperationException`：The system did not provide a usable user location. |
| <a id="api-1f56baab13e20783"></a>[`public static class Inno.Platform.WindowsX64.WindowsX64ApplicationLocations`](../../../platforms/WindowsX64/runtime/Inno.Platform.WindowsX64/WindowsX64ApplicationLocations.cs#L9) | Defines Windows application layout and user data locations without implementing storage services. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

## 项目依赖

- [Inno.Extensibility.Modules](../../extensibility/Inno.Extensibility.Modules.md)：实现依赖，PrivateAssets="compile"。
