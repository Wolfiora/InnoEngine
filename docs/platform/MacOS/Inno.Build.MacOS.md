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

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Build.MacOS.MacOSArm64GameBuildTarget`

<!-- api: T:Inno.Build.MacOS.MacOSArm64GameBuildTarget 529bc9f172d8515afea7d2ac5ab27bc5852dfd3aaab93ae1e6f0713fdaf308aa -->
<!-- api: M:Inno.Build.MacOS.MacOSArm64GameBuildTarget.Validate(System.String) f94875d4f5b1e62085cbe43fcb42d6f080564b9bbc30e2890a18dcf754f140f0 -->
<!-- api: P:Inno.Build.MacOS.MacOSArm64GameBuildTarget.id df48e880ac0fbf260028d57491909d059f93332a142fda50c46dbaad81fbfc4f -->
<!-- api: P:Inno.Build.MacOS.MacOSArm64GameBuildTarget.defaultManagedDeployment 04a0e6f9f7cdd498e17a36201fe5d9c7c635a5721b967456443095210cba839b -->
<!-- api: P:Inno.Build.MacOS.MacOSArm64GameBuildTarget.runtimeIdentifier c5d1129becbc48c9b24814fed07fd4ad57684b7d1c650ecd0b72a16130d0a052 -->
<!-- api: P:Inno.Build.MacOS.MacOSArm64GameBuildTarget.displayName 7475ff21ccc09a593645bd13c21d1159cf456c0337fb0fd6bb84156972d11f3a -->
<!-- api: M:Inno.Build.MacOS.MacOSArm64GameBuildTarget.PackageAsync(Inno.Build.GameBuildPackageContext,System.Threading.CancellationToken) d52ee58761a92203a3ae428d4fcf783a40b65464c3e6581f8778fcb0530bcbfc -->
<!-- api: M:Inno.Build.MacOS.MacOSArm64GameBuildTarget.#ctor 0868d261cafd02715c8563cf923d060d9b9e073e45a0ac2eff42878bd59e2221 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e3b87d100fa9f583"></a>[`public Inno.Build.MacOS.MacOSArm64GameBuildTarget.MacOSArm64GameBuildTarget()`](../../../platforms/MacOS/build/Inno.Build.MacOS/Targets/MacOSArm64GameBuildTarget.cs#L13) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-2bf8e2a8829b436b"></a>[`public System.Threading.Tasks.ValueTask<string> Inno.Build.MacOS.MacOSArm64GameBuildTarget.PackageAsync(Inno.Build.GameBuildPackageContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../platforms/MacOS/build/Inno.Build.MacOS/Targets/MacOSArm64GameBuildTarget.cs#L59) | Composes a macOS application bundle in isolated staging.<br>返回：The staged application bundle path.<br>`context`：The verified Support Pack, packaged content, and output staging paths.<br>`cancellationToken`：The token that cancels composition before commit. | — | — |
| <a id="api-b9c2811f98240e20"></a>[`public void Inno.Build.MacOS.MacOSArm64GameBuildTarget.Validate(string directory)`](../../../platforms/MacOS/build/Inno.Build.MacOS/Targets/MacOSArm64GameBuildTarget.cs#L27) | Validates the target closure before runtime script compilation.<br>`directory`：The Support Pack directory selected by the build catalog. | — | `System.IO.InvalidDataException`：The required platform inputs are incomplete or incompatible. |
| <a id="api-202dbff08bab8987"></a>[`public Inno.Build.Managed.ManagedDeploymentId Inno.Build.MacOS.MacOSArm64GameBuildTarget.defaultManagedDeployment { get; }`](../../../platforms/MacOS/build/Inno.Build.MacOS/Targets/MacOSArm64GameBuildTarget.cs#L35) | Gets the managed publisher selected when the profile does not specify an override. | — | — |
| <a id="api-467c8ba888e8bbec"></a>[`public string Inno.Build.MacOS.MacOSArm64GameBuildTarget.displayName { get; }`](../../../platforms/MacOS/build/Inno.Build.MacOS/Targets/MacOSArm64GameBuildTarget.cs#L43) | Gets the target name presented by authoring hosts. | — | — |
| <a id="api-d1fe9a86124f41b8"></a>[`public Inno.Build.BuildTargetId Inno.Build.MacOS.MacOSArm64GameBuildTarget.id { get; }`](../../../platforms/MacOS/build/Inno.Build.MacOS/Targets/MacOSArm64GameBuildTarget.cs#L32) | Gets the macOS ARM64 target identity. | — | — |
| <a id="api-6a05a35b07befad3"></a>[`public string Inno.Build.MacOS.MacOSArm64GameBuildTarget.runtimeIdentifier { get; }`](../../../platforms/MacOS/build/Inno.Build.MacOS/Targets/MacOSArm64GameBuildTarget.cs#L38) | Gets the managed toolchain target identifier required by this platform composition. | — | — |
| <a id="api-8f2139128eb50faf"></a>[`public sealed class Inno.Build.MacOS.MacOSArm64GameBuildTarget : Inno.Build.IGameBuildTarget`](../../../platforms/MacOS/build/Inno.Build.MacOS/Targets/MacOSArm64GameBuildTarget.cs#L13) | Packages a verified macOS ARM64 Support Pack as a native application bundle. | — | — |

### `Inno.Build.MacOS.MacOSBuildModule`

<!-- api: T:Inno.Build.MacOS.MacOSBuildModule db1f0e185df22c27843e8fc40b5f839cfe59bdf24dffd04870c56caba04ff894 -->
<!-- api: P:Inno.Build.MacOS.MacOSBuildModule.target 20f9d402893cfebcc6068ff6b19cecd6c9473d8b84e5986f3104f406395205cd -->
<!-- api: M:Inno.Build.MacOS.MacOSBuildModule.CreateContribution(Inno.Build.Composition.BuildCompositionContext,Inno.Build.Toolchains.ProductNativeBuildPlan,Inno.Build.Toolchains.ProductNativeBuildPlan,Inno.Build.Toolchains.ProductNativeBuildPlan) f155af5a8e2793f571de711610339f4bdae9fc66488ba9394d55727bb238cfd5 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-714c29ae73d4b0a3"></a>[`public static Inno.Build.Composition.BuildPlatformContribution Inno.Build.MacOS.MacOSBuildModule.CreateContribution(Inno.Build.Composition.BuildCompositionContext context, Inno.Build.Toolchains.ProductNativeBuildPlan nativePlan, Inno.Build.Toolchains.ProductNativeBuildPlan editorNativePlan, Inno.Build.Toolchains.ProductNativeBuildPlan shaderToolsPlan)`](../../../platforms/MacOS/build/Inno.Build.MacOS/MacOSBuildModule.cs#L36) | Contributes the platform's target, product inputs and SDK resolver as one registration.<br>返回：A complete immutable contribution without starting tools or creating files.<br>`context`：The borrowed SDK and execution-host selection.<br>`nativePlan`：The selected shared Player component closure.<br>`editorNativePlan`：The explicitly selected Editor component closure for ordinary product builds.<br>`shaderToolsPlan`：The host-executed offline compiler product for this explicit target. | — | — |
| <a id="api-ca3b715a18d9b59b"></a>[`public static Inno.Build.PlatformTargetDescriptor Inno.Build.MacOS.MacOSBuildModule.target { get; }`](../../../platforms/MacOS/build/Inno.Build.MacOS/MacOSBuildModule.cs#L15) | Gets the explicit implemented product target, independent of the tool execution host. | — | — |
| <a id="api-572fe6dafd641703"></a>[`public static class Inno.Build.MacOS.MacOSBuildModule`](../../../platforms/MacOS/build/Inno.Build.MacOS/MacOSBuildModule.cs#L10) | Owns the implemented MacOS target facts and complete publication contribution. | — | — |

### `Inno.Build.MacOS.MacOSNativeToolchainProvider`

<!-- api: T:Inno.Build.MacOS.MacOSNativeToolchainProvider e67aac3f207a2ca216b44b57f3552cb4aa7839a9e175f6044ef0e69884bb27e3 -->
<!-- api: M:Inno.Build.MacOS.MacOSNativeToolchainProvider.#ctor(System.String) 844bb60a249b4109fc9b32bf0e0209b2dece0adcfe792bd608ace71db4f24483 -->
<!-- api: M:Inno.Build.MacOS.MacOSNativeToolchainProvider.ResolveAsync(Inno.Build.Toolchains.NativeBuildContext,Inno.Build.Toolchains.BuildHostDescriptor,System.String,System.Threading.CancellationToken) fd77d1732edde52ee55b9094c99d2dfba695305ac5096b6a710e10246f33f689 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-acc44a4da4240af3"></a>[`public Inno.Build.MacOS.MacOSNativeToolchainProvider.MacOSNativeToolchainProvider(string dotnetHost)`](../../../platforms/MacOS/build/Inno.Build.MacOS/Toolchain/MacOSNativeToolchainProvider.cs#L27) | Captures the explicit managed executable used by Native binding generation.<br>`dotnetHost`：The explicitly selected SDK executable path or command. | — | `System.ArgumentException`：The host command is empty. |
| <a id="api-52b35f5f60f94785"></a>[`public System.Threading.Tasks.ValueTask<Inno.Build.Toolchains.NativeToolchainSelection> Inno.Build.MacOS.MacOSNativeToolchainProvider.ResolveAsync(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.BuildHostDescriptor host, string targetId, System.Threading.CancellationToken cancellationToken)`](../../../platforms/MacOS/build/Inno.Build.MacOS/Toolchain/MacOSNativeToolchainProvider.cs#L34) | Freezes compiler tools, SDK inputs and child environment before a native operation starts.<br>返回：A complete immutable tool selection for the requested host and target.<br>`context`：The checkout and configuration owned by the requesting operation.<br>`host`：The explicitly declared execution host, independent of the target.<br>`targetId`：The exact requested target; unsupported combinations must fail.<br>`cancellationToken`：Cancels SDK discovery before any product staging. | — | — |
| <a id="api-fac4b5429ada4483"></a>[`public sealed class Inno.Build.MacOS.MacOSNativeToolchainProvider : Inno.Build.Toolchains.INativeToolchainProvider`](../../../platforms/MacOS/build/Inno.Build.MacOS/Toolchain/MacOSNativeToolchainProvider.cs#L14) | Resolves the MacOS SDK for a declared target without deriving publication policy from this process. | — | — |

### `Inno.Build.MacOS.MacOSPlayerSupportPackSource`

<!-- api: T:Inno.Build.MacOS.MacOSPlayerSupportPackSource 4c3deb6f52cd83ce7e600dd8f35e13a41b99f06cbb9ccca9313500c208292e9f -->
<!-- api: M:Inno.Build.MacOS.MacOSPlayerSupportPackSource.#ctor(Inno.Build.Toolchains.ProductNativeBuildPlan,Inno.Build.Toolchains.BuildHostDescriptor,System.String,Inno.Build.Toolchains.INativeBindingGenerator) 15d5ad3acbbca79df6888fa30fd9d41770061dc241dd777c043b29ae066d0613 -->
<!-- api: P:Inno.Build.MacOS.MacOSPlayerSupportPackSource.target 14f1d0a69b9563905a4af5d488221bff535053118b1a2313fdaf349cfecd9b1e -->
<!-- api: M:Inno.Build.MacOS.MacOSPlayerSupportPackSource.CreatePlanAsync(Inno.Build.SupportPacks.PlayerSupportPackPlanningContext,System.Threading.CancellationToken) 826edb4a2d2b6d13d914004b2a04ef8964e1148d0b5e5f39e3f4154c6c0b228a -->
<!-- api: M:Inno.Build.MacOS.MacOSPlayerSupportPackSource.Validate(System.String) fb768f7d812e658ec16fd8a697af62ee491c2d9d528280b0af93224007877b46 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-fc870b8756d4dcfb"></a>[`public Inno.Build.MacOS.MacOSPlayerSupportPackSource.MacOSPlayerSupportPackSource(Inno.Build.Toolchains.ProductNativeBuildPlan nativePlan, Inno.Build.Toolchains.BuildHostDescriptor host, string dotnetHost, Inno.Build.Toolchains.INativeBindingGenerator bindingGenerator)`](../../../platforms/MacOS/build/Inno.Build.MacOS/SupportPacks/MacOSPlayerSupportPackSource.cs#L30) | Captures the selected product closure and tool execution host.<br>`nativePlan`：The explicit Player component closure.<br>`host`：The declared machine executing platform tools.<br>`bindingGenerator`：The borrowed shared binding implementation selected by distribution.<br>`dotnetHost`：The explicit SDK executable for binding generation. | — | — |
| <a id="api-5cd69294ec2be92d"></a>[`public System.Threading.Tasks.ValueTask<Inno.Build.SupportPacks.PlayerSupportPackPlan> Inno.Build.MacOS.MacOSPlayerSupportPackSource.CreatePlanAsync(Inno.Build.SupportPacks.PlayerSupportPackPlanningContext context, System.Threading.CancellationToken cancellationToken)`](../../../platforms/MacOS/build/Inno.Build.MacOS/SupportPacks/MacOSPlayerSupportPackSource.cs#L50) | Validates discovery inputs and freezes a plan before publication output exists.<br>返回：A frozen plan with no running work; missing tools and unsupported hosts fail here.<br>`context`：The source checkout and host used for read-only SDK discovery.<br>`cancellationToken`：Cancels preparation before installation. | — | — |
| <a id="api-07a3f34ef74aaa88"></a>[`public void Inno.Build.MacOS.MacOSPlayerSupportPackSource.Validate(string directory)`](../../../platforms/MacOS/build/Inno.Build.MacOS/SupportPacks/MacOSPlayerSupportPackSource.cs#L56) | Rejects an incomplete or incompatible platform closure.<br>`directory`：The prepared or installed Support Pack directory. | — | `System.IO.InvalidDataException`：A required platform input is absent or incompatible. |
| <a id="api-4c83a2ef15c005af"></a>[`public Inno.Build.BuildTargetId Inno.Build.MacOS.MacOSPlayerSupportPackSource.target { get; }`](../../../platforms/MacOS/build/Inno.Build.MacOS/SupportPacks/MacOSPlayerSupportPackSource.cs#L47) | Gets the stable target identity implemented by this source. | — | — |
| <a id="api-902199e75cb75602"></a>[`public sealed class Inno.Build.MacOS.MacOSPlayerSupportPackSource : Inno.Build.SupportPacks.IPlayerSupportPackSource`](../../../platforms/MacOS/build/Inno.Build.MacOS/SupportPacks/MacOSPlayerSupportPackSource.cs#L11) | Prepares the MacOS Player's explicit layout through shared publication mechanisms. | — | — |

### `Inno.Build.MacOS.MacOSSupportPackValidator`

<!-- api: T:Inno.Build.MacOS.MacOSSupportPackValidator 2e27e5e2beae617b57c3cc58ff7df2c421248a0182c6f1925ba65788d14d584d -->
<!-- api: M:Inno.Build.MacOS.MacOSSupportPackValidator.Validate(System.String) 5e2e147eedde1368e588b0da3e40ce55a3bb94995dec46cae2340956d07ba454 -->
<!-- api: M:Inno.Build.MacOS.MacOSSupportPackValidator.#ctor 57bd684594a977a2b865e1a43212a6b14932c4ff5670e61794cf79430ae3d403 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-370f03da4a13f948"></a>[`public Inno.Build.MacOS.MacOSSupportPackValidator.MacOSSupportPackValidator()`](../../../platforms/MacOS/build/Inno.Build.MacOS/SupportPacks/MacOSSupportPackValidator.cs#L10) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-45306a400e49beaa"></a>[`public void Inno.Build.MacOS.MacOSSupportPackValidator.Validate(string directory)`](../../../platforms/MacOS/build/Inno.Build.MacOS/SupportPacks/MacOSSupportPackValidator.cs#L21) | Rejects incomplete or foreign platform inputs in the supplied Support Pack.<br>`directory`：The isolated or installed target directory to validate. | — | `System.IO.InvalidDataException`：A required input is absent or the closure contains a foreign native binary. |
| <a id="api-ff3fd6ae43ac13e9"></a>[`public sealed class Inno.Build.MacOS.MacOSSupportPackValidator : Inno.Build.IPlayerSupportPackValidator`](../../../platforms/MacOS/build/Inno.Build.MacOS/SupportPacks/MacOSSupportPackValidator.cs#L10) | Validates the platform runtime or linker closure before compilation and installation. | — | — |

## 项目依赖

- [Inno.Build](../../build/Inno.Build.md)：公开引用边界由实际签名核对。
- [Inno.Build.Managed](../../build/Inno.Build.Managed.md)：公开引用边界由实际签名核对。
- [Inno.Build.Composition](../../build/Inno.Build.Composition.md)：公开引用边界由实际签名核对。
- [Inno.Build.SupportPacks.Core](../../build/Inno.Build.SupportPacks.Core.md)：公开引用边界由实际签名核对。
- [Inno.Build.Toolchains](../../build/Inno.Build.Toolchains.md)：公开引用边界由实际签名核对。
