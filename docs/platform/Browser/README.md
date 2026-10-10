# Browser 平台包

[平台索引](../README.md) · [Wiki 首页](../../README.md)

已实现 Browser Wasm32 Player 解释执行/AOT 发布链，不提供 Browser Editor。

运行目标、工具宿主、产品和托管部署分别声明。共享 SDL、BGFX、MiniAudio 等由 backends 提供，不在平台重复实现。

## 项目

- [Inno.Adapter.Storage.Browser](Inno.Adapter.Storage.Browser.md)
- [Inno.Build.Browser](Inno.Build.Browser.md)
- [Inno.Player.Browser](Inno.Player.Browser.md)

- [Inno.Integration.Browser.Bgfx](Inno.Integration.Browser.Bgfx.md)：平台与共享后端的明确接入。

- [Inno.Integration.Browser.Sdl3](Inno.Integration.Browser.Sdl3.md)：平台与共享后端的明确接入。

## BGFX 运行接入

- [Inno.Integration.Browser.Bgfx.Runtime](Inno.Integration.Browser.Bgfx.Runtime.md)：独立于构建 integration 的 surface 解析和能力声明。
