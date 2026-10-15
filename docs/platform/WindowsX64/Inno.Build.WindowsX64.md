# Inno.Build.WindowsX64

[分类索引](README.md) · [Wiki 首页](../../README.md) · [平台归属与扩展](../../architecture/PLATFORM_EXTENSION_GUIDE.md)

## 职责与边界

Windows SDK、已实现发布目标、Support Pack、布局和模板的唯一平台 owner。组件源码、Native facade 和第三方 recipe 属于各 backend。

`ProductBuild.props` 声明 Windows 产品的共享 Shader 配置：WindowsX64 平台和 Direct3D11、Direct3D12、Vulkan、OpenGL。`EditorProduct.props` 导入它并增加 Editor 的 ImGui 配置与功能闭包；Player 导入共同产品配置。产品通过 `InnoProductBuildProperties` 注入，SDK 自动追加的间接引用也传递同一配置；BGFX 和 ImGui backend 不维护 Windows 目标分支。新增 CPU 应根据实际图形编译要求复用或提供配置，并与目标、SDK、ABI 验收一同更新。

## 组合、生命周期与扩展

`WindowsX64BuildModule.CreateContribution` 产生绑定完整的目标贡献；`WindowsX64NativeToolchainProvider` 根据显式 BuildHostDescriptor 和目标解析 SDK。运行目标为 WindowsX64/win-x64，不由当前进程 CPU 推导。

Support Pack source 复用共同 FilePlayerSupportPackPreparation，注入产品项目、模板、平台布局、SDK provider 和精确 Native plan。validator 在提交前核对源代码、generator、managed/native 闭包与平台文件类型。最终玩家布局由对应 target 创建；失败和取消保留旧完整输出。

发布模板使用 MSBuild 的路径规范化组合 `PublishDir` 与精确 Native 相对路径，不依赖末尾分隔符。Pack 本体与 `PlayerLink` 各保留一份相同内容的 Native 输入，分别服务部署和最终链接；CI 只检查 `current` 选中的完整 generation，逐一校验两处文件数量与内容哈希。

新增同平台 CPU 先实现真实 SDK、ABI、布局与 runtime 支持，再增加目标贡献；不复制产品、Editor 或共享 backend。打包/签名只有存在真实实现时增加文件，不建立空目录。

本项目为库，无 Program；共享 Build 不反向引用它。测试通过公开 target/provider/source 验证正向与缺 SDK、错宿主、错误闭包路径。

## 平台基础与集成

本项目不引用具体 backend，也不持有内容 compiler。BGFX 的 Native/Shader profile 与内容工厂移入同平台 integration；完整注册由 Standard Distribution 绑定。打包工厂不接收创作服务。产品 props 只声明产品编译选择，不承担 SDK 或第三方实现。

## 当前源码公开 API 清单

签名来自当前 SDK 求值与语义符号；按类型列出 public/protected 契约。工作流与生命周期见本页前文。

### `Inno.Build.WindowsX64.WindowsX64BuildModule`

<!-- api: T:Inno.Build.WindowsX64.WindowsX64BuildModule cc70d89bef72b6575fd43c08d607e2596550ef0c25835dd6225374aa66c264ac -->
<!-- api: P:Inno.Build.WindowsX64.WindowsX64BuildModule.target a2862673087dffc997115954946946abd46632f322d03ef68f68d015f982f664 -->
<!-- api: M:Inno.Build.WindowsX64.WindowsX64BuildModule.CreateContribution(Inno.Build.Composition.BuildCompositionContext,Inno.Build.Toolchains.ProductNativeBuildPlan,Inno.Build.Toolchains.ProductNativeBuildPlan,Inno.Build.Toolchains.ProductNativeBuildPlan,Inno.Build.IPlayerSupportPackValidator) 04d829e4a151a55ac1e480a347822cc317922dd1ea371b613f668c01bcf68853 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-487e7e96ef4842e1"></a>[`public static Inno.Build.Composition.BuildPlatformContribution Inno.Build.WindowsX64.WindowsX64BuildModule.CreateContribution(Inno.Build.Composition.BuildCompositionContext context, Inno.Build.Toolchains.ProductNativeBuildPlan nativePlan, Inno.Build.Toolchains.ProductNativeBuildPlan editorNativePlan, Inno.Build.Toolchains.ProductNativeBuildPlan shaderToolsPlan, Inno.Build.IPlayerSupportPackValidator supportPackValidator)`](../../../platforms/WindowsX64/build/Inno.Build.WindowsX64/WindowsX64BuildModule.cs#L39) | Contributes the platform's target, product inputs and SDK resolver as one registration.<br>返回：A complete immutable contribution without starting tools or creating files.<br>`context`：The borrowed SDK and execution-host selection.<br>`nativePlan`：The selected shared Player component closure.<br>`editorNativePlan`：The explicitly selected Editor component closure for ordinary product builds.<br>`shaderToolsPlan`：The host-executed offline compiler product for this explicit target.<br>`supportPackValidator`：The complete borrowed platform and selected component validator. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-14dfa7a6468059d7"></a>[`public static Inno.Build.PlatformTargetDescriptor Inno.Build.WindowsX64.WindowsX64BuildModule.target { get; }`](../../../platforms/WindowsX64/build/Inno.Build.WindowsX64/WindowsX64BuildModule.cs#L15) | Gets the explicit implemented product target, independent of the tool execution host. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-93ae69e6dcede899"></a>[`public static class Inno.Build.WindowsX64.WindowsX64BuildModule`](../../../platforms/WindowsX64/build/Inno.Build.WindowsX64/WindowsX64BuildModule.cs#L10) | Owns the implemented Windows target facts and complete publication contribution. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Build.WindowsX64.WindowsX64GameBuildTarget`

<!-- api: T:Inno.Build.WindowsX64.WindowsX64GameBuildTarget 85c6b8eee555c77755ab7d6def91696aecbc38ada42dcb51861db97f9c382c77 -->
<!-- api: M:Inno.Build.WindowsX64.WindowsX64GameBuildTarget.#ctor(Inno.Build.IPlayerSupportPackValidator) 6cc05c9f7da009a11ea7971905662767553344f2ce46a79ef54fe42096f48632 -->
<!-- api: M:Inno.Build.WindowsX64.WindowsX64GameBuildTarget.Validate(System.String) fa6b030ebb105ba32e2d507b7d065c369bee566d9a64090d1b71dcabc78bd7fe -->
<!-- api: P:Inno.Build.WindowsX64.WindowsX64GameBuildTarget.id a6f9c4aa2c198b6490e912ac1e8eeeb51f87cab747a86ae6fece2e28854d88cc -->
<!-- api: P:Inno.Build.WindowsX64.WindowsX64GameBuildTarget.defaultManagedDeployment 7f28161a8ed6b3eec0658c051a3b1e9ead84f229573a9727efdcb230b19fd112 -->
<!-- api: P:Inno.Build.WindowsX64.WindowsX64GameBuildTarget.runtimeIdentifier 119f9ce352a98333e0fe99f76e958f8d416e143897844fd553c77fc46683184a -->
<!-- api: P:Inno.Build.WindowsX64.WindowsX64GameBuildTarget.displayName 4deb83668e4f8b12aab6ee18e342b228972a7f5f521da4bec972b022cf782f00 -->
<!-- api: M:Inno.Build.WindowsX64.WindowsX64GameBuildTarget.PackageAsync(Inno.Build.GameBuildPackageContext,System.Threading.CancellationToken) c310ced0fd8753663f240a376ee15a828405bcf216c7d0dec263634c8630647d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-0af6a0bc7af216cf"></a>[`public Inno.Build.WindowsX64.WindowsX64GameBuildTarget.WindowsX64GameBuildTarget(Inno.Build.IPlayerSupportPackValidator supportPackValidator)`](../../../platforms/WindowsX64/build/Inno.Build.WindowsX64/Targets/WindowsX64GameBuildTarget.cs#L25) | Creates a platform packager with the complete validator selected by its distribution.<br>`supportPackValidator`：The borrowed platform and component-closure validator. | 约束由所属类型与本页生命周期定义。 | `System.ArgumentNullException`：The validator is null. |
| <a id="api-e365d8865642e769"></a>[`public System.Threading.Tasks.ValueTask<string> Inno.Build.WindowsX64.WindowsX64GameBuildTarget.PackageAsync(Inno.Build.GameBuildPackageContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../platforms/WindowsX64/build/Inno.Build.WindowsX64/Targets/WindowsX64GameBuildTarget.cs#L72) | Composes a Windows application directory in isolated staging.<br>返回：The staged Windows application directory.<br>`context`：The verified Support Pack, packaged content, and output staging paths.<br>`cancellationToken`：The token that cancels composition before commit. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-41c27bf1ccc21a46"></a>[`public void Inno.Build.WindowsX64.WindowsX64GameBuildTarget.Validate(string directory)`](../../../platforms/WindowsX64/build/Inno.Build.WindowsX64/Targets/WindowsX64GameBuildTarget.cs#L40) | Validates the target closure before runtime script compilation.<br>`directory`：The Support Pack directory selected by the build catalog. | 约束由所属类型与本页生命周期定义。 | `System.IO.InvalidDataException`：The required platform inputs are incomplete or incompatible. |
| <a id="api-fd3bbd9ab555393c"></a>[`public Inno.Build.Managed.ManagedDeploymentId Inno.Build.WindowsX64.WindowsX64GameBuildTarget.defaultManagedDeployment { get; }`](../../../platforms/WindowsX64/build/Inno.Build.WindowsX64/Targets/WindowsX64GameBuildTarget.cs#L48) | Gets the managed publisher selected when the profile does not specify an override. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-9cbdfcdc81c58cc6"></a>[`public string Inno.Build.WindowsX64.WindowsX64GameBuildTarget.displayName { get; }`](../../../platforms/WindowsX64/build/Inno.Build.WindowsX64/Targets/WindowsX64GameBuildTarget.cs#L56) | Gets the target name presented by authoring hosts. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-dd851144c8b44168"></a>[`public Inno.Build.BuildTargetId Inno.Build.WindowsX64.WindowsX64GameBuildTarget.id { get; }`](../../../platforms/WindowsX64/build/Inno.Build.WindowsX64/Targets/WindowsX64GameBuildTarget.cs#L45) | Gets the Windows x64 target identity. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-c66ff44711e87d19"></a>[`public string Inno.Build.WindowsX64.WindowsX64GameBuildTarget.runtimeIdentifier { get; }`](../../../platforms/WindowsX64/build/Inno.Build.WindowsX64/Targets/WindowsX64GameBuildTarget.cs#L51) | Gets the managed toolchain target identifier required by this platform composition. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-b5cdeab04f9a298f"></a>[`public sealed class Inno.Build.WindowsX64.WindowsX64GameBuildTarget : Inno.Build.IGameBuildTarget`](../../../platforms/WindowsX64/build/Inno.Build.WindowsX64/Targets/WindowsX64GameBuildTarget.cs#L12) | Packages a verified Windows x64 Support Pack as a portable application directory. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Build.WindowsX64.WindowsX64NativeToolchainProvider`

<!-- api: T:Inno.Build.WindowsX64.WindowsX64NativeToolchainProvider 060cb355c63c79686d5f449e2ff7a85dee6a2e022fef635282def6b2460157a0 -->
<!-- api: M:Inno.Build.WindowsX64.WindowsX64NativeToolchainProvider.#ctor(System.String) 6f53ffca086f1108676c6a112ab9f3a66f872f5980339a55fb59db07bdf7fd95 -->
<!-- api: M:Inno.Build.WindowsX64.WindowsX64NativeToolchainProvider.ResolveAsync(Inno.Build.Toolchains.NativeBuildContext,Inno.Build.Toolchains.BuildHostDescriptor,System.String,System.Threading.CancellationToken) 43c9b8ca487ed1d94692485d77badda2ac8cecea455cba0e93015ce4783507e3 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-603234a40e85419c"></a>[`public Inno.Build.WindowsX64.WindowsX64NativeToolchainProvider.WindowsX64NativeToolchainProvider(string dotnetHost)`](../../../platforms/WindowsX64/build/Inno.Build.WindowsX64/Toolchain/WindowsX64NativeToolchainProvider.cs#L27) | Captures the explicit managed executable used by Native binding generation.<br>`dotnetHost`：The explicitly selected SDK executable path or command. | 约束由所属类型与本页生命周期定义。 | `System.ArgumentException`：The host command is empty. |
| <a id="api-47d050e201ceb0c9"></a>[`public System.Threading.Tasks.ValueTask<Inno.Build.Toolchains.NativeToolchainSelection> Inno.Build.WindowsX64.WindowsX64NativeToolchainProvider.ResolveAsync(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.BuildHostDescriptor host, string targetId, System.Threading.CancellationToken cancellationToken)`](../../../platforms/WindowsX64/build/Inno.Build.WindowsX64/Toolchain/WindowsX64NativeToolchainProvider.cs#L34) | Freezes compiler tools, SDK inputs and child environment before a native operation starts.<br>返回：A complete immutable tool selection for the requested host and target.<br>`context`：The checkout and configuration owned by the requesting operation.<br>`host`：The explicitly declared execution host, independent of the target.<br>`targetId`：The exact requested target; unsupported combinations must fail.<br>`cancellationToken`：Cancels SDK discovery before any product staging. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-2fc394aae9d7831f"></a>[`public sealed class Inno.Build.WindowsX64.WindowsX64NativeToolchainProvider : Inno.Build.Toolchains.INativeToolchainProvider`](../../../platforms/WindowsX64/build/Inno.Build.WindowsX64/Toolchain/WindowsX64NativeToolchainProvider.cs#L14) | Resolves the Windows SDK for a declared target without deriving publication policy from this process. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Build.WindowsX64.WindowsX64PlayerSupportPackSource`

<!-- api: T:Inno.Build.WindowsX64.WindowsX64PlayerSupportPackSource 2fef933f73b7c759a32e7b3ac498b7927da9d9e799939a8f7144beef45fc1df2 -->
<!-- api: M:Inno.Build.WindowsX64.WindowsX64PlayerSupportPackSource.#ctor(Inno.Build.Toolchains.ProductNativeBuildPlan,Inno.Build.Toolchains.BuildHostDescriptor,System.String,Inno.Build.Toolchains.INativeBindingGenerator,Inno.Build.IPlayerSupportPackValidator) 74a09ab8b8e7a68d5c14fb14abd5eee4f8af6e82d21d301deb3ed5a5130b0f41 -->
<!-- api: P:Inno.Build.WindowsX64.WindowsX64PlayerSupportPackSource.target 167c3df97250515dee3f9b3cb0482ef5e3ce6ad503291126fe866c4a7c251518 -->
<!-- api: M:Inno.Build.WindowsX64.WindowsX64PlayerSupportPackSource.CreatePlanAsync(Inno.Build.SupportPacks.PlayerSupportPackPlanningContext,System.Threading.CancellationToken) 6fa758960a5563271ed13e351a8f8fd63aae893cca85990476564331cfa506c0 -->
<!-- api: M:Inno.Build.WindowsX64.WindowsX64PlayerSupportPackSource.Validate(System.String) 88906fc6c9a136b41acfda5b716ea7f6d0bf3162336920023177dcb79fcb13b6 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-963336c58317aed0"></a>[`public Inno.Build.WindowsX64.WindowsX64PlayerSupportPackSource.WindowsX64PlayerSupportPackSource(Inno.Build.Toolchains.ProductNativeBuildPlan nativePlan, Inno.Build.Toolchains.BuildHostDescriptor host, string dotnetHost, Inno.Build.Toolchains.INativeBindingGenerator bindingGenerator, Inno.Build.IPlayerSupportPackValidator supportPackValidator)`](../../../platforms/WindowsX64/build/Inno.Build.WindowsX64/SupportPacks/WindowsX64PlayerSupportPackSource.cs#L33) | Captures the selected product closure and tool execution host.<br>`nativePlan`：The explicit Player component closure.<br>`host`：The declared machine executing platform tools.<br>`bindingGenerator`：The borrowed shared binding implementation selected by distribution.<br>`dotnetHost`：The explicit SDK executable for binding generation.<br>`supportPackValidator`：The borrowed validator selected by the distribution. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-a09fa95ad4e63499"></a>[`public System.Threading.Tasks.ValueTask<Inno.Build.SupportPacks.PlayerSupportPackPlan> Inno.Build.WindowsX64.WindowsX64PlayerSupportPackSource.CreatePlanAsync(Inno.Build.SupportPacks.PlayerSupportPackPlanningContext context, System.Threading.CancellationToken cancellationToken)`](../../../platforms/WindowsX64/build/Inno.Build.WindowsX64/SupportPacks/WindowsX64PlayerSupportPackSource.cs#L54) | Validates discovery inputs and freezes a plan before publication output exists.<br>返回：A frozen plan with no running work; missing tools and unsupported hosts fail here.<br>`context`：The source checkout and host used for read-only SDK discovery.<br>`cancellationToken`：Cancels preparation before installation. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-02b483415f4b5e2b"></a>[`public void Inno.Build.WindowsX64.WindowsX64PlayerSupportPackSource.Validate(string directory)`](../../../platforms/WindowsX64/build/Inno.Build.WindowsX64/SupportPacks/WindowsX64PlayerSupportPackSource.cs#L60) | Rejects an incomplete or incompatible platform closure.<br>`directory`：The prepared or installed Support Pack directory. | 约束由所属类型与本页生命周期定义。 | `System.IO.InvalidDataException`：A required platform input is absent or incompatible. |
| <a id="api-31ec2796d07c8cbd"></a>[`public Inno.Build.BuildTargetId Inno.Build.WindowsX64.WindowsX64PlayerSupportPackSource.target { get; }`](../../../platforms/WindowsX64/build/Inno.Build.WindowsX64/SupportPacks/WindowsX64PlayerSupportPackSource.cs#L51) | Gets the stable target identity implemented by this source. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-aa2fd99a36f7416f"></a>[`public sealed class Inno.Build.WindowsX64.WindowsX64PlayerSupportPackSource : Inno.Build.SupportPacks.IPlayerSupportPackSource`](../../../platforms/WindowsX64/build/Inno.Build.WindowsX64/SupportPacks/WindowsX64PlayerSupportPackSource.cs#L11) | Prepares the Windows Player's explicit layout through shared publication mechanisms. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Build.WindowsX64.WindowsX64SupportPackValidator`

<!-- api: T:Inno.Build.WindowsX64.WindowsX64SupportPackValidator 895cca575becafaaca401412742967e12f901b49390bd5b81f019224317b5ff7 -->
<!-- api: M:Inno.Build.WindowsX64.WindowsX64SupportPackValidator.Validate(System.String) e7bd11cb92424c69006ac06f2efcbe4248630f3105a0866b34a1c6cace0fa82f -->
<!-- api: M:Inno.Build.WindowsX64.WindowsX64SupportPackValidator.#ctor 9cb8b899b2cebf42ba7500ab40c6f2321e0ad82602fd6a6bf5d2adb18199f401 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-53156fff5d6367d2"></a>[`public Inno.Build.WindowsX64.WindowsX64SupportPackValidator.WindowsX64SupportPackValidator()`](../../../platforms/WindowsX64/build/Inno.Build.WindowsX64/SupportPacks/WindowsX64SupportPackValidator.cs#L10) | C# 生成的声明；状态与值语义遵循所属类型契约。 | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-85cf47fc4b3160de"></a>[`public void Inno.Build.WindowsX64.WindowsX64SupportPackValidator.Validate(string directory)`](../../../platforms/WindowsX64/build/Inno.Build.WindowsX64/SupportPacks/WindowsX64SupportPackValidator.cs#L21) | Rejects incomplete or foreign platform inputs in the supplied Support Pack.<br>`directory`：The isolated or installed target directory to validate. | 约束由所属类型与本页生命周期定义。 | `System.IO.InvalidDataException`：A required input is absent or the closure contains a foreign native binary. |
| <a id="api-85b2eeb365c50ecf"></a>[`public sealed class Inno.Build.WindowsX64.WindowsX64SupportPackValidator : Inno.Build.IPlayerSupportPackValidator`](../../../platforms/WindowsX64/build/Inno.Build.WindowsX64/SupportPacks/WindowsX64SupportPackValidator.cs#L10) | Validates the platform runtime or linker closure before compilation and installation. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

## 项目依赖

- [Inno.Build](../../build/Inno.Build.md)：公开引用边界由实际签名核对。
- [Inno.Build.Managed](../../build/Inno.Build.Managed.md)：公开引用边界由实际签名核对。
- [Inno.Build.Composition](../../build/Inno.Build.Composition.md)：公开引用边界由实际签名核对。
- [Inno.Build.SupportPacks.Core](../../build/Inno.Build.SupportPacks.Core.md)：公开引用边界由实际签名核对。
- [Inno.Build.Toolchains](../../build/Inno.Build.Toolchains.md)：公开引用边界由实际签名核对。
