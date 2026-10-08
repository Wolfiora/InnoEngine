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

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Build.Linux.LinuxBuildModule`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Linux.LinuxBuildModule`](../../../platforms/Linux/build/Inno.Build.Linux/LinuxBuildModule.cs#L10) | Contributes the existing Linux native SDK capabilities without registering an unsupported Player product. |
| [`static System.Collections.Generic.IReadOnlyList<Inno.Build.Composition.NativeToolchainContribution> Inno.Build.Linux.LinuxBuildModule.CreateNativeContributions(Inno.Build.Composition.BuildCompositionContext context)`](../../../platforms/Linux/build/Inno.Build.Linux/LinuxBuildModule.cs#L21) | Gets the implemented explicit native targets and their SDK providers. |

### `Inno.Build.Linux.LinuxNativeToolchainProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Linux.LinuxNativeToolchainProvider`](../../../platforms/Linux/build/Inno.Build.Linux/Toolchain/LinuxNativeToolchainProvider.cs#L14) | Resolves the Linux SDK for a declared target without deriving publication policy from this process. |
| [`Inno.Build.Linux.LinuxNativeToolchainProvider.LinuxNativeToolchainProvider(string dotnetHost)`](../../../platforms/Linux/build/Inno.Build.Linux/Toolchain/LinuxNativeToolchainProvider.cs#L27) | Captures the explicit managed executable used by Native binding generation. |
| [`System.Threading.Tasks.ValueTask<Inno.Build.Toolchains.NativeToolchainSelection> Inno.Build.Linux.LinuxNativeToolchainProvider.ResolveAsync(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.BuildHostDescriptor host, string targetId, System.Threading.CancellationToken cancellationToken)`](../../../platforms/Linux/build/Inno.Build.Linux/Toolchain/LinuxNativeToolchainProvider.cs#L34) | See the implemented contract. |

## 项目依赖

- [Inno.Build.Composition](../../build/Inno.Build.Composition.md)：公开引用边界由实际签名核对。
- [Inno.Build.Toolchains](../../build/Inno.Build.Toolchains.md)：公开引用边界由实际签名核对。
