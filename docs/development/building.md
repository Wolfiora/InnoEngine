# 开发入口

[Wiki 首页](../README.md) · [产品启动](../platform/PRODUCT_STARTUP.md) · [启动架构](entry/README.md)

| 项目 | 职责 |
| --- | --- |
| [InnoEngine.Editor](entry/InnoEngine.Editor.md) | 普通可执行项目；构建当前宿主 Editor 依赖并在主线程启动。 |
| [InnoEngine.Build](entry/InnoEngine.Build.md) | 普通可执行项目；默认准备当前宿主支持产物，提供显式构建和发布命令。 |

安装仓库要求的 .NET SDK 与当前平台工具链，递归 clone 后，用 Rider 或 Visual Studio 打开 Solution，选择 InnoEngine.Editor 运行即可。不需要共享 .run、外部脚本或先运行 Build。

两个入口使用真实 ProjectReference。宿主选择发生在项目求值阶段，普通 C# 由 SDK 增量构建；Native 支持由现有产品构建机制准备。其他生产层不引用 entry。

命令行构建 `.sln` 或 `.slnx` 时，根目录 `Directory.Solution.targets` 为同次调用的所有项目根分配同一个 bootstrap owner。普通项目构建继续沿实际 ProjectReference 传递该 owner，私有 Task 加载各自保持独立所有权；并行项目不会仅因入口各自生成随机 owner 而重复写入相同托管输出。下一次 solution 调用获得新的 owner，调用方显式指定的 owner 则保留。该设置使用 [MSBuild 的 solution 扩展入口](https://learn.microsoft.com/en-us/visualstudio/msbuild/customize-solution-build)，不改变 Native 输入指纹或允许跨调用继承成功状态。
