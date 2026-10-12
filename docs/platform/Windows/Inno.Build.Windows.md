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

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Build.Windows.WindowsBuildModule`

<!-- api: T:Inno.Build.Windows.WindowsBuildModule 6209911b76fe958f7236ca4c524eb57685b5487c6b2876b2849477ba43cf222f -->
<!-- api: P:Inno.Build.Windows.WindowsBuildModule.target ee4268bea89c86359ad988027edf2252cb926a63890a19ffac3435c19396dab2 -->
<!-- api: M:Inno.Build.Windows.WindowsBuildModule.CreateContribution(Inno.Build.Composition.BuildCompositionContext,Inno.Build.Toolchains.ProductNativeBuildPlan,Inno.Build.Toolchains.ProductNativeBuildPlan,Inno.Build.Toolchains.ProductNativeBuildPlan) 7280ba2a15120024678c719c0975ef9ff2540c4652ea5cbd3a02543f53136d43 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c44c090151009dbc"></a>[`public static Inno.Build.Composition.BuildPlatformContribution Inno.Build.Windows.WindowsBuildModule.CreateContribution(Inno.Build.Composition.BuildCompositionContext context, Inno.Build.Toolchains.ProductNativeBuildPlan nativePlan, Inno.Build.Toolchains.ProductNativeBuildPlan editorNativePlan, Inno.Build.Toolchains.ProductNativeBuildPlan shaderToolsPlan)`](../../../platforms/Windows/build/Inno.Build.Windows/WindowsBuildModule.cs#L36) | Contributes the platform's target, product inputs and SDK resolver as one registration.<br>返回：A complete immutable contribution without starting tools or creating files.<br>`context`：The borrowed SDK and execution-host selection.<br>`nativePlan`：The selected shared Player component closure.<br>`editorNativePlan`：The explicitly selected Editor component closure for ordinary product builds.<br>`shaderToolsPlan`：The host-executed offline compiler product for this explicit target. | — | — |
| <a id="api-fb983c9b1da4d2c3"></a>[`public static Inno.Build.PlatformTargetDescriptor Inno.Build.Windows.WindowsBuildModule.target { get; }`](../../../platforms/Windows/build/Inno.Build.Windows/WindowsBuildModule.cs#L15) | Gets the explicit implemented product target, independent of the tool execution host. | — | — |
| <a id="api-b5d017b7aee103f4"></a>[`public static class Inno.Build.Windows.WindowsBuildModule`](../../../platforms/Windows/build/Inno.Build.Windows/WindowsBuildModule.cs#L10) | Owns the implemented Windows target facts and complete publication contribution. | — | — |

### `Inno.Build.Windows.WindowsNativeToolchainProvider`

<!-- api: T:Inno.Build.Windows.WindowsNativeToolchainProvider 916689a1841a0501c22f2efd11066a9ffdfc3d9be2bf12aca2d8afe773cb7b9b -->
<!-- api: M:Inno.Build.Windows.WindowsNativeToolchainProvider.#ctor(System.String) 2955859e4a0ebb8e8d0d7f47c6e4dee9cb2a374d1263690127a79a96df8d7dcc -->
<!-- api: M:Inno.Build.Windows.WindowsNativeToolchainProvider.ResolveAsync(Inno.Build.Toolchains.NativeBuildContext,Inno.Build.Toolchains.BuildHostDescriptor,System.String,System.Threading.CancellationToken) 2520274bc82a8dbe892d4c78dbcc4ff3d5fa64d3201d46c5f4d441e9385e25b7 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9728f00f7871c3a3"></a>[`public Inno.Build.Windows.WindowsNativeToolchainProvider.WindowsNativeToolchainProvider(string dotnetHost)`](../../../platforms/Windows/build/Inno.Build.Windows/Toolchain/WindowsNativeToolchainProvider.cs#L27) | Captures the explicit managed executable used by Native binding generation.<br>`dotnetHost`：The explicitly selected SDK executable path or command. | — | `System.ArgumentException`：The host command is empty. |
| <a id="api-e9c585d8142d8f13"></a>[`public System.Threading.Tasks.ValueTask<Inno.Build.Toolchains.NativeToolchainSelection> Inno.Build.Windows.WindowsNativeToolchainProvider.ResolveAsync(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.BuildHostDescriptor host, string targetId, System.Threading.CancellationToken cancellationToken)`](../../../platforms/Windows/build/Inno.Build.Windows/Toolchain/WindowsNativeToolchainProvider.cs#L34) | Freezes compiler tools, SDK inputs and child environment before a native operation starts.<br>返回：A complete immutable tool selection for the requested host and target.<br>`context`：The checkout and configuration owned by the requesting operation.<br>`host`：The explicitly declared execution host, independent of the target.<br>`targetId`：The exact requested target; unsupported combinations must fail.<br>`cancellationToken`：Cancels SDK discovery before any product staging. | — | — |
| <a id="api-081d0be9a51a878f"></a>[`public sealed class Inno.Build.Windows.WindowsNativeToolchainProvider : Inno.Build.Toolchains.INativeToolchainProvider`](../../../platforms/Windows/build/Inno.Build.Windows/Toolchain/WindowsNativeToolchainProvider.cs#L14) | Resolves the Windows SDK for a declared target without deriving publication policy from this process. | — | — |

### `Inno.Build.Windows.WindowsPlayerSupportPackSource`

<!-- api: T:Inno.Build.Windows.WindowsPlayerSupportPackSource 67fc0f86b0f97bc166fb4ab72694fd22c87826a2b9e55bdeabf7a5caab020dc9 -->
<!-- api: M:Inno.Build.Windows.WindowsPlayerSupportPackSource.#ctor(Inno.Build.Toolchains.ProductNativeBuildPlan,Inno.Build.Toolchains.BuildHostDescriptor,System.String,Inno.Build.Toolchains.INativeBindingGenerator) 680e25a6588f68a1729bf1a5bcb538f718e70e007deb18103542ac3ebd066f46 -->
<!-- api: P:Inno.Build.Windows.WindowsPlayerSupportPackSource.target 8a38137b293be72189145daf47657437c2b44e5eb570f9ef35c113b6325c0692 -->
<!-- api: M:Inno.Build.Windows.WindowsPlayerSupportPackSource.CreatePlanAsync(Inno.Build.SupportPacks.PlayerSupportPackPlanningContext,System.Threading.CancellationToken) c00b449c517bb3a87b33162416d21a44b1788a137d909f4c4e893bf1cac9a15d -->
<!-- api: M:Inno.Build.Windows.WindowsPlayerSupportPackSource.Validate(System.String) eceba3876d7d525a04b950e000aca5383d09c1a1344b68e9b58ed1ab71c840df -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-fd7ac37e85da9401"></a>[`public Inno.Build.Windows.WindowsPlayerSupportPackSource.WindowsPlayerSupportPackSource(Inno.Build.Toolchains.ProductNativeBuildPlan nativePlan, Inno.Build.Toolchains.BuildHostDescriptor host, string dotnetHost, Inno.Build.Toolchains.INativeBindingGenerator bindingGenerator)`](../../../platforms/Windows/build/Inno.Build.Windows/SupportPacks/WindowsPlayerSupportPackSource.cs#L30) | Captures the selected product closure and tool execution host.<br>`nativePlan`：The explicit Player component closure.<br>`host`：The declared machine executing platform tools.<br>`bindingGenerator`：The borrowed shared binding implementation selected by distribution.<br>`dotnetHost`：The explicit SDK executable for binding generation. | — | — |
| <a id="api-1107f006a9dcde36"></a>[`public System.Threading.Tasks.ValueTask<Inno.Build.SupportPacks.PlayerSupportPackPlan> Inno.Build.Windows.WindowsPlayerSupportPackSource.CreatePlanAsync(Inno.Build.SupportPacks.PlayerSupportPackPlanningContext context, System.Threading.CancellationToken cancellationToken)`](../../../platforms/Windows/build/Inno.Build.Windows/SupportPacks/WindowsPlayerSupportPackSource.cs#L50) | Validates discovery inputs and freezes a plan before publication output exists.<br>返回：A frozen plan with no running work; missing tools and unsupported hosts fail here.<br>`context`：The source checkout and host used for read-only SDK discovery.<br>`cancellationToken`：Cancels preparation before installation. | — | — |
| <a id="api-55284a6bdf1c7299"></a>[`public void Inno.Build.Windows.WindowsPlayerSupportPackSource.Validate(string directory)`](../../../platforms/Windows/build/Inno.Build.Windows/SupportPacks/WindowsPlayerSupportPackSource.cs#L56) | Rejects an incomplete or incompatible platform closure.<br>`directory`：The prepared or installed Support Pack directory. | — | `System.IO.InvalidDataException`：A required platform input is absent or incompatible. |
| <a id="api-e9b2a670d3325a41"></a>[`public Inno.Build.BuildTargetId Inno.Build.Windows.WindowsPlayerSupportPackSource.target { get; }`](../../../platforms/Windows/build/Inno.Build.Windows/SupportPacks/WindowsPlayerSupportPackSource.cs#L47) | Gets the stable target identity implemented by this source. | — | — |
| <a id="api-92e11a98687965f6"></a>[`public sealed class Inno.Build.Windows.WindowsPlayerSupportPackSource : Inno.Build.SupportPacks.IPlayerSupportPackSource`](../../../platforms/Windows/build/Inno.Build.Windows/SupportPacks/WindowsPlayerSupportPackSource.cs#L11) | Prepares the Windows Player's explicit layout through shared publication mechanisms. | — | — |

### `Inno.Build.Windows.WindowsSupportPackValidator`

<!-- api: T:Inno.Build.Windows.WindowsSupportPackValidator e4fd498bfd41654bb1fec44e20004b6e8ca0d57e889be0c8f7a1dfe4f098de83 -->
<!-- api: M:Inno.Build.Windows.WindowsSupportPackValidator.Validate(System.String) ce1094c65f1e9984120b7782ac24d102be47bf26c85a5df8348e4c6163b98ec3 -->
<!-- api: M:Inno.Build.Windows.WindowsSupportPackValidator.#ctor 152509fcb7e0be4f63e17b50430aa5b2d38a3741635a104a7737190e6f6fb23c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4d48237ae5c183ea"></a>[`public Inno.Build.Windows.WindowsSupportPackValidator.WindowsSupportPackValidator()`](../../../platforms/Windows/build/Inno.Build.Windows/SupportPacks/WindowsSupportPackValidator.cs#L10) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-261ed119ebb05374"></a>[`public void Inno.Build.Windows.WindowsSupportPackValidator.Validate(string directory)`](../../../platforms/Windows/build/Inno.Build.Windows/SupportPacks/WindowsSupportPackValidator.cs#L21) | Rejects incomplete or foreign platform inputs in the supplied Support Pack.<br>`directory`：The isolated or installed target directory to validate. | — | `System.IO.InvalidDataException`：A required input is absent or the closure contains a foreign native binary. |
| <a id="api-0623b72afc0af68f"></a>[`public sealed class Inno.Build.Windows.WindowsSupportPackValidator : Inno.Build.IPlayerSupportPackValidator`](../../../platforms/Windows/build/Inno.Build.Windows/SupportPacks/WindowsSupportPackValidator.cs#L10) | Validates the platform runtime or linker closure before compilation and installation. | — | — |

### `Inno.Build.Windows.WindowsX64GameBuildTarget`

<!-- api: T:Inno.Build.Windows.WindowsX64GameBuildTarget c1de517e8cbf21172d966912d9bb3a4036b909dba0b667f0a214b1607c57707b -->
<!-- api: M:Inno.Build.Windows.WindowsX64GameBuildTarget.Validate(System.String) b3e314837ac668dd4b7a7df54be525eb4b958b465bbf027b11660a3e3b41c15b -->
<!-- api: P:Inno.Build.Windows.WindowsX64GameBuildTarget.id c27126a16c05466682d56b62198ba0b18a7e954e67f07d2762532207e0d083e5 -->
<!-- api: P:Inno.Build.Windows.WindowsX64GameBuildTarget.defaultManagedDeployment f49044979fd3efee336fda46ed01491348dd2f1d9ec9742cfa9bfeaa7369f4b6 -->
<!-- api: P:Inno.Build.Windows.WindowsX64GameBuildTarget.runtimeIdentifier bbd52b00b0cb631d8ca8e9d432717f9c84471bc0c38715f7dcfc046dcbd77931 -->
<!-- api: P:Inno.Build.Windows.WindowsX64GameBuildTarget.displayName d709cfec3d919999582a4eccf6e04ff71a65e67e989725abf3cd19d25b440c96 -->
<!-- api: M:Inno.Build.Windows.WindowsX64GameBuildTarget.PackageAsync(Inno.Build.GameBuildPackageContext,System.Threading.CancellationToken) a571de4643467f949866724ae7eb0d93d4021ec18e19133dc711ca713a8b1263 -->
<!-- api: M:Inno.Build.Windows.WindowsX64GameBuildTarget.#ctor 70d391fd0c52ab5fd620fe0ac3722d9da6b97aab6898e9b3ae66d8bfca3e3bf1 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-eee59bb0cf7c981c"></a>[`public Inno.Build.Windows.WindowsX64GameBuildTarget.WindowsX64GameBuildTarget()`](../../../platforms/Windows/build/Inno.Build.Windows/Targets/WindowsX64GameBuildTarget.cs#L12) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-8966e8af1d07f0c9"></a>[`public System.Threading.Tasks.ValueTask<string> Inno.Build.Windows.WindowsX64GameBuildTarget.PackageAsync(Inno.Build.GameBuildPackageContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../platforms/Windows/build/Inno.Build.Windows/Targets/WindowsX64GameBuildTarget.cs#L58) | Composes a Windows application directory in isolated staging.<br>返回：The staged Windows application directory.<br>`context`：The verified Support Pack, packaged content, and output staging paths.<br>`cancellationToken`：The token that cancels composition before commit. | — | — |
| <a id="api-f7ae164305c8bda6"></a>[`public void Inno.Build.Windows.WindowsX64GameBuildTarget.Validate(string directory)`](../../../platforms/Windows/build/Inno.Build.Windows/Targets/WindowsX64GameBuildTarget.cs#L26) | Validates the target closure before runtime script compilation.<br>`directory`：The Support Pack directory selected by the build catalog. | — | `System.IO.InvalidDataException`：The required platform inputs are incomplete or incompatible. |
| <a id="api-61a1155e5ccc1b74"></a>[`public Inno.Build.Managed.ManagedDeploymentId Inno.Build.Windows.WindowsX64GameBuildTarget.defaultManagedDeployment { get; }`](../../../platforms/Windows/build/Inno.Build.Windows/Targets/WindowsX64GameBuildTarget.cs#L34) | Gets the managed publisher selected when the profile does not specify an override. | — | — |
| <a id="api-2ce4b303330c32ed"></a>[`public string Inno.Build.Windows.WindowsX64GameBuildTarget.displayName { get; }`](../../../platforms/Windows/build/Inno.Build.Windows/Targets/WindowsX64GameBuildTarget.cs#L42) | Gets the target name presented by authoring hosts. | — | — |
| <a id="api-87004b0aff0d4bde"></a>[`public Inno.Build.BuildTargetId Inno.Build.Windows.WindowsX64GameBuildTarget.id { get; }`](../../../platforms/Windows/build/Inno.Build.Windows/Targets/WindowsX64GameBuildTarget.cs#L31) | Gets the Windows x64 target identity. | — | — |
| <a id="api-e9d29834a331e0ba"></a>[`public string Inno.Build.Windows.WindowsX64GameBuildTarget.runtimeIdentifier { get; }`](../../../platforms/Windows/build/Inno.Build.Windows/Targets/WindowsX64GameBuildTarget.cs#L37) | Gets the managed toolchain target identifier required by this platform composition. | — | — |
| <a id="api-05ddbc1eaf9e7198"></a>[`public sealed class Inno.Build.Windows.WindowsX64GameBuildTarget : Inno.Build.IGameBuildTarget`](../../../platforms/Windows/build/Inno.Build.Windows/Targets/WindowsX64GameBuildTarget.cs#L12) | Packages a verified Windows x64 Support Pack as a portable application directory. | — | — |

## 项目依赖

- [Inno.Build](../../build/Inno.Build.md)：公开引用边界由实际签名核对。
- [Inno.Build.Managed](../../build/Inno.Build.Managed.md)：公开引用边界由实际签名核对。
- [Inno.Build.Composition](../../build/Inno.Build.Composition.md)：公开引用边界由实际签名核对。
- [Inno.Build.SupportPacks.Core](../../build/Inno.Build.SupportPacks.Core.md)：公开引用边界由实际签名核对。
- [Inno.Build.Toolchains](../../build/Inno.Build.Toolchains.md)：公开引用边界由实际签名核对。
