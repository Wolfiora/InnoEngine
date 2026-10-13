# Browser 平台包

[平台索引](../README.md) · [Wiki 首页](../../README.md)

已实现 Browser Wasm32 Player 解释执行/AOT 发布链，不提供 Browser Editor。

运行目标、工具宿主、产品和托管部署分别声明。共享 SDL、BGFX、MiniAudio 等由 backends 提供，不在平台重复实现。

## 项目

- [Inno.Adapter.Storage.BrowserWasm32](Inno.Adapter.Storage.BrowserWasm32.md)
- [Inno.Build.BrowserWasm32](Inno.Build.BrowserWasm32.md)
- [Inno.Player.BrowserWasm32](Inno.Player.BrowserWasm32.md)

- [Inno.Integration.BrowserWasm32.Bgfx](Inno.Integration.BrowserWasm32.Bgfx.md)：平台与共享后端的明确接入。

- [Inno.Integration.BrowserWasm32.Sdl3](Inno.Integration.BrowserWasm32.Sdl3.md)：平台与共享后端的明确接入。

## BGFX 运行接入

- [Inno.Integration.BrowserWasm32.Bgfx.Runtime](Inno.Integration.BrowserWasm32.Bgfx.Runtime.md)：独立于构建 integration 的 surface 解析和能力声明。
