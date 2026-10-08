# Windows 平台包

[平台索引](../README.md) · [Wiki 首页](../../README.md)

已实现 WindowsX64 Editor/Player 源码与发布链，实际 gate 见当前验收。

运行目标、工具宿主、产品和托管部署分别声明。共享 SDL、BGFX、MiniAudio 等由 backends 提供，不在平台重复实现。

## 项目

- [Inno.Build.Windows](Inno.Build.Windows.md)
- [Inno.Editor.Windows](Inno.Editor.Windows.md)
- [Inno.Platform.Windows](Inno.Platform.Windows.md)
- [Inno.Player.Windows](Inno.Player.Windows.md)

- [Inno.Integration.Windows.Bgfx](Inno.Integration.Windows.Bgfx.md)：平台与共享后端的明确接入。

- [Inno.Integration.Windows.Sdl3](Inno.Integration.Windows.Sdl3.md)：平台与共享后端的明确接入。
