# Inno.Platform.MacOS

[分类索引](README.md) · [Wiki 首页](../../README.md) · [平台归属与扩展](../../architecture/PLATFORM_EXTENSION_GUIDE.md)

## 职责与边界

MacOS 实际 OS 应用位置规则。只回答应用内容位置和用户数据根，不实现内容、文件存储、输入或窗口 backend。

## 组合、生命周期与扩展

位置由平台产品使用并传给具体 Content/Storage/Log 服务。错误宿主或无效应用目录明确失败；无跨平台猜测、旧目录回退或第二份设置。

该服务不拥有 Session 或设备，不在领域程序集或脚本中暴露 OS 路径策略。生命周期为产品启动时解析一次，再注入明确 owner。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Platform.MacOS.MacOSApplicationLocations`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Platform.MacOS.MacOSApplicationLocations`](../../../platforms/MacOS/runtime/Inno.Platform.MacOS/MacOSApplicationLocations.cs#L9) | Defines MacOS application layout and user data locations without implementing storage services. |
| [`static string Inno.Platform.MacOS.MacOSApplicationLocations.GetPlayerContentDirectory(string applicationDirectory)`](../../../platforms/MacOS/runtime/Inno.Platform.MacOS/MacOSApplicationLocations.cs#L45) | Resolves the sole content location in a published MacOS application layout. |
| [`static string Inno.Platform.MacOS.MacOSApplicationLocations.userDataRoot`](../../../platforms/MacOS/runtime/Inno.Platform.MacOS/MacOSApplicationLocations.cs#L20) | Gets the current user's application data root according to MacOS system conventions. |

## 项目依赖

- [Inno.Extensibility.Modules](../../extensibility/Inno.Extensibility.Modules.md)：实现依赖，PrivateAssets="compile"。
