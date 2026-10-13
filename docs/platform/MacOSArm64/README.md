# MacOS 平台包

[平台索引](../README.md) · [Wiki 首页](../../README.md)

已实现 MacOSArm64 Editor/Player 源码与组合；本次 Windows 环境不能提供 macOS 实机验收。

运行目标、工具宿主、产品和托管部署分别声明。共享 SDL、BGFX、MiniAudio 等由 backends 提供，不在平台重复实现。

## 项目

- [Inno.Build.MacOSArm64](Inno.Build.MacOSArm64.md)
- [Inno.Editor.MacOSArm64](Inno.Editor.MacOSArm64.md)
- [Inno.Platform.MacOSArm64](Inno.Platform.MacOSArm64.md)
- [Inno.Player.MacOSArm64](Inno.Player.MacOSArm64.md)

- [Inno.Integration.MacOSArm64.Bgfx](Inno.Integration.MacOSArm64.Bgfx.md)：平台与共享后端的明确接入。

- [Inno.Integration.MacOSArm64.Sdl3](Inno.Integration.MacOSArm64.Sdl3.md)：平台与共享后端的明确接入。

## BGFX 运行接入

- [Inno.Integration.MacOSArm64.Bgfx.Runtime](Inno.Integration.MacOSArm64.Bgfx.Runtime.md)：独立于构建 integration 的 surface 解析和能力声明。
