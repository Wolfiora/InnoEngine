# Inno.Build.Linux

[分类索引](README.md) · [Wiki 首页](../../README.md) · [平台归属与扩展](../../architecture/PLATFORM_EXTENSION_GUIDE.md)

## 职责与边界

已有 Linux Native SDK 能力的 owner。没有完整 Linux Player 发布链，因此不注册游戏目标。

## 组合、生命周期与扩展

LinuxBuildModule 提供明确 linux-x64/linux-arm64 的 Native-only contribution。provider 校验实际宿主与请求目标，选择 GCC/Clang/Make/CMake、环境与 ABI 参数。平台支持状态与 SDK 实际可执行性分别验证。

未来完整 Player 支持还必须实现真实产品、布局、Support Pack、部署兼容性及设备运行验收。存在 SDK provider 不等于完整引擎发行支持。

## 平台基础与集成

本项目不引用具体 backend，也不持有内容 compiler。BGFX 的 Native/Shader profile 与内容工厂移入同平台 integration；完整注册由 Standard Distribution 绑定。打包工厂不接收创作服务。产品 props 只声明产品编译选择，不承担 SDK 或第三方实现。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Build.Linux.LinuxBuildModule`

<!-- api: T:Inno.Build.Linux.LinuxBuildModule d8ea6206c3a2b32ed343f9a02c0487b0d13db855138e7de314d0e8fb0a418533 -->
<!-- api: M:Inno.Build.Linux.LinuxBuildModule.CreateNativeContributions(Inno.Build.Composition.BuildCompositionContext) fba28d7d563a47bf0dc5de493b4474422ffc3e57ba7100b2240b87d447f19f05 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-18db470a6cb576f9"></a>[`public static System.Collections.Generic.IReadOnlyList<Inno.Build.Composition.NativeToolchainContribution> Inno.Build.Linux.LinuxBuildModule.CreateNativeContributions(Inno.Build.Composition.BuildCompositionContext context)`](../../../platforms/Linux/build/Inno.Build.Linux/LinuxBuildModule.cs#L21) | Gets the implemented explicit native targets and their SDK providers.<br>返回：Immutable SDK contributions; these targets do not appear in game publication choices.<br>`context`：The explicit managed generator and tool execution host. | — | — |
| <a id="api-e602b911a3f4a169"></a>[`public static class Inno.Build.Linux.LinuxBuildModule`](../../../platforms/Linux/build/Inno.Build.Linux/LinuxBuildModule.cs#L10) | Contributes the existing Linux native SDK capabilities without registering an unsupported Player product. | — | — |

### `Inno.Build.Linux.LinuxNativeToolchainProvider`

<!-- api: T:Inno.Build.Linux.LinuxNativeToolchainProvider 4b9b0be3bf36152d903318601f9a65e34b16f65b345ba7aff79495071f4c674f -->
<!-- api: M:Inno.Build.Linux.LinuxNativeToolchainProvider.#ctor(System.String) f30fcd432538b1d0af0f03497f0e9e7027fb70dfa3877954b45256ce81ac8452 -->
<!-- api: M:Inno.Build.Linux.LinuxNativeToolchainProvider.ResolveAsync(Inno.Build.Toolchains.NativeBuildContext,Inno.Build.Toolchains.BuildHostDescriptor,System.String,System.Threading.CancellationToken) c0cc0e7f2519556b2f665d290e896f8b8e24d17eca63711c0c4d0acfddc65ae4 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-62ef1ee0d05a62af"></a>[`public Inno.Build.Linux.LinuxNativeToolchainProvider.LinuxNativeToolchainProvider(string dotnetHost)`](../../../platforms/Linux/build/Inno.Build.Linux/Toolchain/LinuxNativeToolchainProvider.cs#L27) | Captures the explicit managed executable used by Native binding generation.<br>`dotnetHost`：The explicitly selected SDK executable path or command. | — | `System.ArgumentException`：The host command is empty. |
| <a id="api-23aa4730c4af6c9c"></a>[`public System.Threading.Tasks.ValueTask<Inno.Build.Toolchains.NativeToolchainSelection> Inno.Build.Linux.LinuxNativeToolchainProvider.ResolveAsync(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.BuildHostDescriptor host, string targetId, System.Threading.CancellationToken cancellationToken)`](../../../platforms/Linux/build/Inno.Build.Linux/Toolchain/LinuxNativeToolchainProvider.cs#L34) | Freezes compiler tools, SDK inputs and child environment before a native operation starts.<br>返回：A complete immutable tool selection for the requested host and target.<br>`context`：The checkout and configuration owned by the requesting operation.<br>`host`：The explicitly declared execution host, independent of the target.<br>`targetId`：The exact requested target; unsupported combinations must fail.<br>`cancellationToken`：Cancels SDK discovery before any product staging. | — | — |
| <a id="api-60572c4ec2081b70"></a>[`public sealed class Inno.Build.Linux.LinuxNativeToolchainProvider : Inno.Build.Toolchains.INativeToolchainProvider`](../../../platforms/Linux/build/Inno.Build.Linux/Toolchain/LinuxNativeToolchainProvider.cs#L14) | Resolves the Linux SDK for a declared target without deriving publication policy from this process. | — | — |

## 项目依赖

- [Inno.Build.Composition](../../build/Inno.Build.Composition.md)：公开引用边界由实际签名核对。
- [Inno.Build.Toolchains](../../build/Inno.Build.Toolchains.md)：公开引用边界由实际签名核对。
