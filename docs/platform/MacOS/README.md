# MacOS 平台包

[平台索引](../README.md) · [Wiki 首页](../../README.md)

已实现 MacOSArm64 Editor/Player 源码与组合；本次 Windows 环境不能提供 macOS 实机验收。

运行目标、工具宿主、产品和托管部署分别声明。共享 SDL、BGFX、MiniAudio 等由 backends 提供，不在平台重复实现。

## 项目

- [Inno.Build.MacOS](Inno.Build.MacOS.md)
- [Inno.Editor.MacOS](Inno.Editor.MacOS.md)
- [Inno.Platform.MacOS](Inno.Platform.MacOS.md)
- [Inno.Player.MacOS](Inno.Player.MacOS.md)

- [Inno.Integration.MacOS.Bgfx](Inno.Integration.MacOS.Bgfx.md)：平台与共享后端的明确接入。

- [Inno.Integration.MacOS.Sdl3](Inno.Integration.MacOS.Sdl3.md)：平台与共享后端的明确接入。
