# Inno.Build.MacOS

[分类索引](README.md) · [Wiki 首页](../../README.md) · [平台归属与扩展](../../architecture/PLATFORM_EXTENSION_GUIDE.md)

## 职责与边界

`ProductBuild.props` 声明 macOS 产品的共享 Shader 配置：MacOSArm64 平台和 Metal、Vulkan、OpenGL。`EditorProduct.props` 导入它并增加 ImGui 配置与 Editor 功能闭包；Player 使用共同产品配置。产品通过 `InnoProductBuildProperties` 注入，直接及 SDK 自动追加的间接引用采用相同配置。Shader 工具的执行宿主与产物平台分别表达，BGFX 和 ImGui backend 不根据当前 OS 选择目标。新增 CPU 应根据真实 SDK 与图形编译要求复用或提供配置，并完成实机验证。

MacOS SDK、已实现发布目标、Support Pack、布局和模板的唯一平台 owner。组件源码、Native facade 和第三方 recipe 属于各 backend。

## 组合、生命周期与扩展

`MacOSBuildModule.CreateContribution` 产生绑定完整的目标贡献；`MacOSNativeToolchainProvider` 根据显式 BuildHostDescriptor 和目标解析 SDK。运行目标为 MacOSArm64/osx-arm64，不由当前进程 CPU 推导。

Support Pack source 复用共同 FilePlayerSupportPackPreparation，注入产品项目、模板、平台布局、SDK provider 和精确 Native plan。validator 在提交前核对源代码、generator、managed/native 闭包与平台文件类型。最终玩家布局由对应 target 创建；失败和取消保留旧完整输出。

新增同平台 CPU 先实现真实 SDK、ABI、布局与 runtime 支持，再增加目标贡献；不复制产品、Editor 或共享 backend。打包/签名只有存在真实实现时增加文件，不建立空目录。

本项目为库，无 Program；共享 Build 不反向引用它。测试通过公开 target/provider/source 验证正向与缺 SDK、错宿主、错误闭包路径。

## 平台基础与集成

本项目不引用具体 backend，也不持有内容 compiler。BGFX 的 Native/Shader profile 与内容工厂移入同平台 integration；完整注册由 Standard Distribution 绑定。打包工厂不接收创作服务。产品 props 只声明产品编译选择，不承担 SDK 或第三方实现。

## 当前源码公开 API 清单

只列当前源码 public/protected 表面；内部机制不是稳定 API，参数、返回、失败及所有权以英文 XML 为准。

### `Inno.Build.MacOS.MacOSArm64GameBuildTarget`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.BuildTargetId Inno.Build.MacOS.MacOSArm64GameBuildTarget.id`](../../../platforms/MacOS/build/Inno.Build.MacOS/Targets/MacOSArm64GameBuildTarget.cs#L32) | Gets the macOS ARM64 target identity. |
| [`Inno.Build.MacOS.MacOSArm64GameBuildTarget`](../../../platforms/MacOS/build/Inno.Build.MacOS/Targets/MacOSArm64GameBuildTarget.cs#L13) | Packages a verified macOS ARM64 Support Pack as a native application bundle. |
| [`Inno.Build.Managed.ManagedDeploymentId Inno.Build.MacOS.MacOSArm64GameBuildTarget.defaultManagedDeployment`](../../../platforms/MacOS/build/Inno.Build.MacOS/Targets/MacOSArm64GameBuildTarget.cs#L35) | See the implemented contract. |
| [`System.Threading.Tasks.ValueTask<string> Inno.Build.MacOS.MacOSArm64GameBuildTarget.PackageAsync(Inno.Build.GameBuildPackageContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../platforms/MacOS/build/Inno.Build.MacOS/Targets/MacOSArm64GameBuildTarget.cs#L59) | Composes a macOS application bundle in isolated staging. |
| [`string Inno.Build.MacOS.MacOSArm64GameBuildTarget.displayName`](../../../platforms/MacOS/build/Inno.Build.MacOS/Targets/MacOSArm64GameBuildTarget.cs#L43) | Gets the target name presented by authoring hosts. |
| [`string Inno.Build.MacOS.MacOSArm64GameBuildTarget.runtimeIdentifier`](../../../platforms/MacOS/build/Inno.Build.MacOS/Targets/MacOSArm64GameBuildTarget.cs#L38) | See the implemented contract. |
| [`void Inno.Build.MacOS.MacOSArm64GameBuildTarget.Validate(string directory)`](../../../platforms/MacOS/build/Inno.Build.MacOS/Targets/MacOSArm64GameBuildTarget.cs#L27) | Validates the target closure before runtime script compilation. |

### `Inno.Build.MacOS.MacOSBuildModule`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.MacOS.MacOSBuildModule`](../../../platforms/MacOS/build/Inno.Build.MacOS/MacOSBuildModule.cs#L10) | Owns the implemented MacOS target facts and complete publication contribution. |
| [`static Inno.Build.Composition.BuildPlatformContribution Inno.Build.MacOS.MacOSBuildModule.CreateContribution(Inno.Build.Composition.BuildCompositionContext context, Inno.Build.Toolchains.ProductNativeBuildPlan nativePlan, Inno.Build.Toolchains.ProductNativeBuildPlan editorNativePlan, Inno.Build.Toolchains.ProductNativeBuildPlan shaderToolsPlan)`](../../../platforms/MacOS/build/Inno.Build.MacOS/MacOSBuildModule.cs#L36) | Contributes the platform's target, product inputs and SDK resolver as one registration. |
| [`static Inno.Build.PlatformTargetDescriptor Inno.Build.MacOS.MacOSBuildModule.target`](../../../platforms/MacOS/build/Inno.Build.MacOS/MacOSBuildModule.cs#L15) | Gets the explicit implemented product target, independent of the tool execution host. |

### `Inno.Build.MacOS.MacOSNativeToolchainProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.MacOS.MacOSNativeToolchainProvider`](../../../platforms/MacOS/build/Inno.Build.MacOS/Toolchain/MacOSNativeToolchainProvider.cs#L14) | Resolves the MacOS SDK for a declared target without deriving publication policy from this process. |
| [`Inno.Build.MacOS.MacOSNativeToolchainProvider.MacOSNativeToolchainProvider(string dotnetHost)`](../../../platforms/MacOS/build/Inno.Build.MacOS/Toolchain/MacOSNativeToolchainProvider.cs#L27) | Captures the explicit managed executable used by Native binding generation. |
| [`System.Threading.Tasks.ValueTask<Inno.Build.Toolchains.NativeToolchainSelection> Inno.Build.MacOS.MacOSNativeToolchainProvider.ResolveAsync(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.BuildHostDescriptor host, string targetId, System.Threading.CancellationToken cancellationToken)`](../../../platforms/MacOS/build/Inno.Build.MacOS/Toolchain/MacOSNativeToolchainProvider.cs#L34) | See the implemented contract. |

### `Inno.Build.MacOS.MacOSPlayerSupportPackSource`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.BuildTargetId Inno.Build.MacOS.MacOSPlayerSupportPackSource.target`](../../../platforms/MacOS/build/Inno.Build.MacOS/SupportPacks/MacOSPlayerSupportPackSource.cs#L45) | See the implemented contract. |
| [`Inno.Build.MacOS.MacOSPlayerSupportPackSource`](../../../platforms/MacOS/build/Inno.Build.MacOS/SupportPacks/MacOSPlayerSupportPackSource.cs#L11) | Prepares the MacOS Player's explicit layout through shared publication mechanisms. |
| [`Inno.Build.MacOS.MacOSPlayerSupportPackSource.MacOSPlayerSupportPackSource(Inno.Build.Toolchains.ProductNativeBuildPlan nativePlan, Inno.Build.Toolchains.BuildHostDescriptor host, string dotnetHost, Inno.Build.Toolchains.INativeBindingGenerator bindingGenerator)`](../../../platforms/MacOS/build/Inno.Build.MacOS/SupportPacks/MacOSPlayerSupportPackSource.cs#L30) | Captures the selected product closure and tool execution host. |
| [`System.Threading.Tasks.ValueTask<Inno.Build.SupportPacks.PlayerSupportPackPlan> Inno.Build.MacOS.MacOSPlayerSupportPackSource.CreatePlanAsync(Inno.Build.SupportPacks.PlayerSupportPackPlanningContext context, System.Threading.CancellationToken cancellationToken)`](../../../platforms/MacOS/build/Inno.Build.MacOS/SupportPacks/MacOSPlayerSupportPackSource.cs#L48) | See the implemented contract. |
| [`void Inno.Build.MacOS.MacOSPlayerSupportPackSource.Validate(string directory)`](../../../platforms/MacOS/build/Inno.Build.MacOS/SupportPacks/MacOSPlayerSupportPackSource.cs#L54) | See the implemented contract. |

### `Inno.Build.MacOS.MacOSSupportPackValidator`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.MacOS.MacOSSupportPackValidator`](../../../platforms/MacOS/build/Inno.Build.MacOS/SupportPacks/MacOSSupportPackValidator.cs#L10) | Validates the platform runtime or linker closure before compilation and installation. |
| [`void Inno.Build.MacOS.MacOSSupportPackValidator.Validate(string directory)`](../../../platforms/MacOS/build/Inno.Build.MacOS/SupportPacks/MacOSSupportPackValidator.cs#L21) | Rejects incomplete or foreign platform inputs in the supplied Support Pack. |

## 项目依赖

- [Inno.Build](../../build/Inno.Build.md)：公开引用边界由实际签名核对。
- [Inno.Build.Managed](../../build/Inno.Build.Managed.md)：公开引用边界由实际签名核对。
- [Inno.Build.Composition](../../build/Inno.Build.Composition.md)：公开引用边界由实际签名核对。
- [Inno.Build.SupportPacks.Core](../../build/Inno.Build.SupportPacks.Core.md)：公开引用边界由实际签名核对。
- [Inno.Build.Toolchains](../../build/Inno.Build.Toolchains.md)：公开引用边界由实际签名核对。
