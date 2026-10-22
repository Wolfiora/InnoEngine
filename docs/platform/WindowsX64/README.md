# Windows 平台包

[平台索引](../README.md) · [Wiki 首页](../../README.md)

已实现 WindowsX64 Editor/Player 源码与发布链，实际 gate 见当前验收。

运行目标、工具宿主、产品和托管部署分别声明。共享 SDL、BGFX、MiniAudio 等由 backends 提供，不在平台重复实现。

## 项目

- [Inno.Build.WindowsX64](Inno.Build.WindowsX64.md)
- [Inno.Editor.WindowsX64](Inno.Editor.WindowsX64.md)
- [Inno.Platform.WindowsX64](Inno.Platform.WindowsX64.md)
- [Inno.Player.WindowsX64](Inno.Player.WindowsX64.md)

- [Inno.Integration.WindowsX64.Bgfx](Inno.Integration.WindowsX64.Bgfx.md)：平台与共享后端的明确接入。

- [Inno.Integration.WindowsX64.Sdl3](Inno.Integration.WindowsX64.Sdl3.md)：平台与共享后端的明确接入。

## BGFX 运行接入

- [Inno.Integration.WindowsX64.Bgfx.Runtime](Inno.Integration.WindowsX64.Bgfx.Runtime.md)：独立于构建 integration 的 surface 解析和能力声明。

- [Inno.Integration.WindowsX64.Build](Inno.Integration.WindowsX64.Build.md)：平台职责与最小公开构建入口。
