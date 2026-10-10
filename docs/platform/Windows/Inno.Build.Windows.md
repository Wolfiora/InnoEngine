# Inno.Build.Windows

[分类索引](README.md) · [Wiki 首页](../../README.md) · [平台归属与扩展](../../architecture/PLATFORM_EXTENSION_GUIDE.md)

## 职责与边界

Windows SDK、已实现发布目标、Support Pack、布局和模板的唯一平台 owner。组件源码、Native facade 和第三方 recipe 属于各 backend。

`ProductBuild.props` 声明 Windows 产品的共享 Shader 配置：WindowsX64 平台和 Direct3D11、Direct3D12、Vulkan、OpenGL。`EditorProduct.props` 导入它并增加 Editor 的 ImGui 配置与功能闭包；Player 导入共同产品配置。产品通过 `InnoProductBuildProperties` 注入，SDK 自动追加的间接引用也传递同一配置；BGFX 和 ImGui backend 不维护 Windows 目标分支。新增 CPU 应根据实际图形编译要求复用或提供配置，并与目标、SDK、ABI 验收一同更新。

## 组合、生命周期与扩展

`WindowsBuildModule.CreateContribution` 产生绑定完整的目标贡献；`WindowsNativeToolchainProvider` 根据显式 BuildHostDescriptor 和目标解析 SDK。运行目标为 WindowsX64/win-x64，不由当前进程 CPU 推导。

Support Pack source 复用共同 FilePlayerSupportPackPreparation，注入产品项目、模板、平台布局、SDK provider 和精确 Native plan。validator 在提交前核对源代码、generator、managed/native 闭包与平台文件类型。最终玩家布局由对应 target 创建；失败和取消保留旧完整输出。

新增同平台 CPU 先实现真实 SDK、ABI、布局与 runtime 支持，再增加目标贡献；不复制产品、Editor 或共享 backend。打包/签名只有存在真实实现时增加文件，不建立空目录。

本项目为库，无 Program；共享 Build 不反向引用它。测试通过公开 target/provider/source 验证正向与缺 SDK、错宿主、错误闭包路径。

## 平台基础与集成

本项目不引用具体 backend，也不持有内容 compiler。BGFX 的 Native/Shader profile 与内容工厂移入同平台 integration；完整注册由 Standard Distribution 绑定。打包工厂不接收创作服务。产品 props 只声明产品编译选择，不承担 SDK 或第三方实现。

## 当前源码公开 API 清单

只列当前源码 public/protected 表面；内部机制不是稳定 API，参数、返回、失败及所有权以英文 XML 为准。

### `Inno.Build.Windows.WindowsBuildModule`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Windows.WindowsBuildModule`](../../../platforms/Windows/build/Inno.Build.Windows/WindowsBuildModule.cs#L10) | Owns the implemented Windows target facts and complete publication contribution. |
| [`static Inno.Build.Composition.BuildPlatformContribution Inno.Build.Windows.WindowsBuildModule.CreateContribution(Inno.Build.Composition.BuildCompositionContext context, Inno.Build.Toolchains.ProductNativeBuildPlan nativePlan, Inno.Build.Toolchains.ProductNativeBuildPlan editorNativePlan, Inno.Build.Toolchains.ProductNativeBuildPlan shaderToolsPlan)`](../../../platforms/Windows/build/Inno.Build.Windows/WindowsBuildModule.cs#L36) | Contributes the platform's target, product inputs and SDK resolver as one registration. |
| [`static Inno.Build.PlatformTargetDescriptor Inno.Build.Windows.WindowsBuildModule.target`](../../../platforms/Windows/build/Inno.Build.Windows/WindowsBuildModule.cs#L15) | Gets the explicit implemented product target, independent of the tool execution host. |

### `Inno.Build.Windows.WindowsNativeToolchainProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Windows.WindowsNativeToolchainProvider`](../../../platforms/Windows/build/Inno.Build.Windows/Toolchain/WindowsNativeToolchainProvider.cs#L14) | Resolves the Windows SDK for a declared target without deriving publication policy from this process. |
| [`Inno.Build.Windows.WindowsNativeToolchainProvider.WindowsNativeToolchainProvider(string dotnetHost)`](../../../platforms/Windows/build/Inno.Build.Windows/Toolchain/WindowsNativeToolchainProvider.cs#L27) | Captures the explicit managed executable used by Native binding generation. |
| [`System.Threading.Tasks.ValueTask<Inno.Build.Toolchains.NativeToolchainSelection> Inno.Build.Windows.WindowsNativeToolchainProvider.ResolveAsync(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.BuildHostDescriptor host, string targetId, System.Threading.CancellationToken cancellationToken)`](../../../platforms/Windows/build/Inno.Build.Windows/Toolchain/WindowsNativeToolchainProvider.cs#L34) | See the implemented contract. |

### `Inno.Build.Windows.WindowsPlayerSupportPackSource`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.BuildTargetId Inno.Build.Windows.WindowsPlayerSupportPackSource.target`](../../../platforms/Windows/build/Inno.Build.Windows/SupportPacks/WindowsPlayerSupportPackSource.cs#L45) | See the implemented contract. |
| [`Inno.Build.Windows.WindowsPlayerSupportPackSource`](../../../platforms/Windows/build/Inno.Build.Windows/SupportPacks/WindowsPlayerSupportPackSource.cs#L11) | Prepares the Windows Player's explicit layout through shared publication mechanisms. |
| [`Inno.Build.Windows.WindowsPlayerSupportPackSource.WindowsPlayerSupportPackSource(Inno.Build.Toolchains.ProductNativeBuildPlan nativePlan, Inno.Build.Toolchains.BuildHostDescriptor host, string dotnetHost, Inno.Build.Toolchains.INativeBindingGenerator bindingGenerator)`](../../../platforms/Windows/build/Inno.Build.Windows/SupportPacks/WindowsPlayerSupportPackSource.cs#L30) | Captures the selected product closure and tool execution host. |
| [`System.Threading.Tasks.ValueTask<Inno.Build.SupportPacks.PlayerSupportPackPlan> Inno.Build.Windows.WindowsPlayerSupportPackSource.CreatePlanAsync(Inno.Build.SupportPacks.PlayerSupportPackPlanningContext context, System.Threading.CancellationToken cancellationToken)`](../../../platforms/Windows/build/Inno.Build.Windows/SupportPacks/WindowsPlayerSupportPackSource.cs#L48) | See the implemented contract. |
| [`void Inno.Build.Windows.WindowsPlayerSupportPackSource.Validate(string directory)`](../../../platforms/Windows/build/Inno.Build.Windows/SupportPacks/WindowsPlayerSupportPackSource.cs#L54) | See the implemented contract. |

### `Inno.Build.Windows.WindowsSupportPackValidator`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Windows.WindowsSupportPackValidator`](../../../platforms/Windows/build/Inno.Build.Windows/SupportPacks/WindowsSupportPackValidator.cs#L10) | Validates the platform runtime or linker closure before compilation and installation. |
| [`void Inno.Build.Windows.WindowsSupportPackValidator.Validate(string directory)`](../../../platforms/Windows/build/Inno.Build.Windows/SupportPacks/WindowsSupportPackValidator.cs#L21) | Rejects incomplete or foreign platform inputs in the supplied Support Pack. |

### `Inno.Build.Windows.WindowsX64GameBuildTarget`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.BuildTargetId Inno.Build.Windows.WindowsX64GameBuildTarget.id`](../../../platforms/Windows/build/Inno.Build.Windows/Targets/WindowsX64GameBuildTarget.cs#L31) | Gets the Windows x64 target identity. |
| [`Inno.Build.Managed.ManagedDeploymentId Inno.Build.Windows.WindowsX64GameBuildTarget.defaultManagedDeployment`](../../../platforms/Windows/build/Inno.Build.Windows/Targets/WindowsX64GameBuildTarget.cs#L34) | See the implemented contract. |
| [`Inno.Build.Windows.WindowsX64GameBuildTarget`](../../../platforms/Windows/build/Inno.Build.Windows/Targets/WindowsX64GameBuildTarget.cs#L12) | Packages a verified Windows x64 Support Pack as a portable application directory. |
| [`System.Threading.Tasks.ValueTask<string> Inno.Build.Windows.WindowsX64GameBuildTarget.PackageAsync(Inno.Build.GameBuildPackageContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../platforms/Windows/build/Inno.Build.Windows/Targets/WindowsX64GameBuildTarget.cs#L58) | Composes a Windows application directory in isolated staging. |
| [`string Inno.Build.Windows.WindowsX64GameBuildTarget.displayName`](../../../platforms/Windows/build/Inno.Build.Windows/Targets/WindowsX64GameBuildTarget.cs#L42) | Gets the target name presented by authoring hosts. |
| [`string Inno.Build.Windows.WindowsX64GameBuildTarget.runtimeIdentifier`](../../../platforms/Windows/build/Inno.Build.Windows/Targets/WindowsX64GameBuildTarget.cs#L37) | See the implemented contract. |
| [`void Inno.Build.Windows.WindowsX64GameBuildTarget.Validate(string directory)`](../../../platforms/Windows/build/Inno.Build.Windows/Targets/WindowsX64GameBuildTarget.cs#L26) | Validates the target closure before runtime script compilation. |

## 项目依赖

- [Inno.Build](../../build/Inno.Build.md)：公开引用边界由实际签名核对。
- [Inno.Build.Managed](../../build/Inno.Build.Managed.md)：公开引用边界由实际签名核对。
- [Inno.Build.Composition](../../build/Inno.Build.Composition.md)：公开引用边界由实际签名核对。
- [Inno.Build.SupportPacks.Core](../../build/Inno.Build.SupportPacks.Core.md)：公开引用边界由实际签名核对。
- [Inno.Build.Toolchains](../../build/Inno.Build.Toolchains.md)：公开引用边界由实际签名核对。
