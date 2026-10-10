# Inno.Editor.Windows

[分类索引](README.md) · [Wiki 首页](../../README.md) · [平台归属与扩展](../../architecture/PLATFORM_EXTENSION_GUIDE.md)

## 职责与边界

Windows Editor 的薄产品入口。选择明确运行目标、共享 backend 和平台系统服务；共同功能不在此复制。

## 组合、生命周期与扩展

`Program` 只进入 `WindowsEditorComposition`。运行目标固定为当前已实现的 windows-x64；不同 CPU 将复用产品源码，只增加已实现的目标描述和工具链参数。

产品调用共享 EditorApplication.RunAsync，显式注入键盘、作者端 provider、动态模块、序列化和构建 distribution。普通 IDE Build/Publish 准备 Editor Native 闭包；ImGui Shader 以当前 Editor 目标编译。

产品持有调用者资源与配置；共享应用入口接管自身创建的资源。OS 用户目录由 `Inno.Platform.Windows` 提供。窗口、Input、Rendering、Audio、Text 和 UI 选择已有 backend，不在平台复制 SDL/BGFX/MiniAudio。

本项目没有稳定 public/protected API；命令行的第一个参数是 Project 目录，例如 `Inno.Editor.Windows.exe C:/Projects/MyGame`。不支持 `--project` 参数；该选项属于 Build CLI。普通产品 Build 自动准备 Native，启动步骤见[产品启动指南](../PRODUCT_STARTUP.md)。

## 当前源码公开 API 清单

只列当前源码 public/protected 表面；内部机制不是稳定 API，参数、返回、失败及所有权以英文 XML 为准。

## 项目依赖

- [Inno.Integration.Windows.Bgfx.Runtime](Inno.Integration.Windows.Bgfx.Runtime.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Integration.Windows.Sdl3](Inno.Integration.Windows.Sdl3.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Integration.Windows.Bgfx](Inno.Integration.Windows.Bgfx.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Adapter.Presentation.ImGui.Bgfx](../../backends/ImGui/Inno.Adapter.Presentation.ImGui.Bgfx.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Windows](Inno.Build.Windows.md)：公开引用边界由实际签名核对。
- [Inno.Editor.Hosting](../../editor/Inno.Editor.Hosting.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Authoring.Default](../../runtime/Inno.Adapter.Authoring.Default.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Storage.FileSystem](../../backends/FileSystem/Inno.Adapter.Storage.FileSystem.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Modules.DotNet](../../backends/DotNet/Inno.Adapter.Modules.DotNet.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Serialization.DotNet](../../backends/DotNet/Inno.Adapter.Serialization.DotNet.md)：公开引用边界由实际签名核对。
- [Inno.Build.Distribution.Standard](../../build/Inno.Build.Distribution.Standard.md)：公开引用边界由实际签名核对。
- [Inno.Core.Execution](../../core/Inno.Core.Execution.md)：公开引用边界由实际签名核对。
- [Inno.Platform.Windows](Inno.Platform.Windows.md)：公开引用边界由实际签名核对。
- [Inno.Build.Toolchains.Bgfx.Tools](../../backends/Bgfx/Inno.Build.Toolchains.Bgfx.Tools.md)：公开引用边界由实际签名核对。
