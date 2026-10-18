# 开发入口

[Wiki 首页](../README.md) · [产品启动](../platform/PRODUCT_STARTUP.md) · [启动架构](../architecture/DEVELOPMENT_ENTRY_ARCHITECTURE.md)

| 项目 | 职责 |
| --- | --- |
| [InnoEngine.Editor](InnoEngine.Editor.md) | 普通可执行项目；构建当前宿主 Editor 依赖并在主线程启动。 |
| [InnoEngine.Build](InnoEngine.Build.md) | 普通可执行项目；默认准备当前宿主支持产物，提供显式构建和发布命令。 |

安装仓库要求的 .NET SDK 与当前平台工具链，递归 clone 后，用 Rider 或 Visual Studio 打开 Solution，选择 InnoEngine.Editor 运行即可。不需要共享 .run、外部脚本或先运行 Build。

两个入口使用真实 ProjectReference。宿主选择发生在项目求值阶段，普通 C# 由 SDK 增量构建；Native 支持由现有产品构建机制准备。其他生产层不引用 entry。
