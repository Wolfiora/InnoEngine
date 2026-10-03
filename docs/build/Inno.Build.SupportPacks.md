# Inno.Build.SupportPacks

[Build 索引](README.md) · [Wiki 首页](../README.md) · [事务核心](Inno.Build.SupportPacks.Core.md) · [统一 CLI](Inno.Build.Cli.md)

## 职责与分层

这是内置平台发布的组合库，没有 Program。桌面 source 负责 self-contained Player 和动态 Native closure；Web source 负责静态 Native 构建、目标绑定、共享 Player 引用及链接模板。各自实现 IPlayerSupportPackSource，并复用平台校验器和核心原子安装事务。

## 全部 public API

`BuiltInPlayerSupportPacks.CreatePublisher()` 返回独立且已冻结的平台注册 publisher，包含 Windows x64、macOS ARM64、browser-wasm。没有 protected 扩展点。

```csharp
using Inno.Build;
using Inno.Build.SupportPacks;

string installed = await BuiltInPlayerSupportPacks.CreatePublisher().PublishAsync(
    engineRoot,
    outputRoot,
    BuildTargetId.browserWasm,
    dotnetHost);
```

路径变量由调用方显式提供。CLI、Editor 自动供给和 MSBuild 发布使用同一组合。

## 初始化、失败和生命周期

Web 先生成目标 BGCS binding，再从当前 Native facades 和 extern 构建静态库，之后编译共享 Player 引用。源内链接模板位于 BrowserLink，Pack 内中立输入目录为 PlayerLink；最终游戏不会部署这些输入。

桌面 source 先调用 `HostNativeBuild.BuildRuntimeAsync(new NativeBuildContext(engineRoot, "release"), token)`，
再发布 Release Player 并复制闭包。Debug Editor 构建和独立 `support-pack` 都能主动准备 Release 输入，
不依赖以前遗留的 `.lib` 缓存。平台 source 只组合五个运行时组件，不为 Player 构建 ImGui 或 ImGuizmo。
macOS 和 Windows 使用同一桌面 source 的配置实例，并由各自平台校验器验证实际 closure。
缺少 SDK/Native 输出、子进程失败或不完整 closure 均明确失败；安装事务不在平台 source 内重复实现。
