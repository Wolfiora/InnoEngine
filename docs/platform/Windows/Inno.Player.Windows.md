# Inno.Player.Windows

[分类索引](README.md) · [Wiki 首页](../../README.md) · [平台归属与扩展](../../architecture/PLATFORM_EXTENSION_GUIDE.md)

## 职责与边界

Windows Player 的薄产品入口。选择明确运行目标、共享 backend 和平台系统服务；共同功能不在此复制。

## 组合、生命周期与扩展

`Program` 只进入 `WindowsPlayerComposition`。运行目标固定为当前已实现的 windows-x64；不同 CPU 将复用产品源码，只增加已实现的目标描述和工具链参数。

产品调用共享 PlayerApplication，共用静态注册、内容完整性、只读 Settings、输入与 Session 生命周期。平台 content source 只决定明确应用布局；文件读取和缓存复用 FileSystem backend。CoreCLR 和 NativeAOT 发布使用同一共享逻辑。

产品持有调用者资源与配置；共享应用入口接管自身创建的资源。OS 用户目录由 `Inno.Platform.Windows` 提供。窗口、Input、Rendering、Audio、Text 和 UI 选择已有 backend，不在平台复制 SDL/BGFX/MiniAudio。

本项目没有稳定 public/protected API；入口运行导出流程组合的游戏代码、`Content/runtime.manifest`、`Content/catalog.inno` 与对应 Pack。它不接收创作 Project 路径。普通源码 Build 不会生成某个游戏的完整发布内容；运行导出的游戏 `.exe`，步骤见[产品启动指南](../PRODUCT_STARTUP.md)。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

本项目没有公开入口，通过组合或扩展发现使用内部实现。

## 项目依赖

- [Inno.Integration.Windows.Sdl3](Inno.Integration.Windows.Sdl3.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Execution](../../core/Inno.Core.Execution.md)：公开引用边界由实际签名核对。
- [Inno.Player.Runtime](../../runtime/Inno.Player.Runtime.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Default](../../runtime/Inno.Adapter.Default.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Content.FileSystem](../../backends/FileSystem/Inno.Adapter.Content.FileSystem.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Storage.FileSystem](../../backends/FileSystem/Inno.Adapter.Storage.FileSystem.md)：公开引用边界由实际签名核对。
- [Inno.Platform.Windows](Inno.Platform.Windows.md)：公开引用边界由实际签名核对。
