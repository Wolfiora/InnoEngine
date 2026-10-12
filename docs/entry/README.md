# 开发入口

[Wiki 首页](../README.md) · [产品启动](../platform/PRODUCT_STARTUP.md) · [架构](../architecture/ENGINE_ARCHITECTURE_OVERVIEW.md)

| 项目 | 职责 |
| --- | --- |
| [InnoEngine.Editor](InnoEngine.Editor.md) | 选择当前宿主的 Editor；未指定项目时打开目录选择窗口。 |
| [InnoEngine.Build](InnoEngine.Build.md) | 当前宿主增量构建，以及显式清理、Support Pack 和游戏发布命令。 |
| [Inno.Entry.Bootstrap](Inno.Entry.Bootstrap.md) | 共享开发启动机制，不进入游戏或领域运行闭包。 |

`entry` 是最外层开发便利边界。其 OS/CPU 判断选择一个明确目标，再调用已有构建和平台产品机制。
共享领域、backend、Editor Hosting 和 Player Runtime 不依赖它。
