# Inno.Build.MacOSArm64

[分类索引](README.md) · [Wiki 首页](../../README.md) · [平台归属与扩展](../../architecture/PLATFORM_EXTENSION_GUIDE.md)

## 职责与边界

`ProductBuild.props` 声明 macOS 产品的共享 Shader 配置：MacOSArm64 平台和 Metal、Vulkan、OpenGL。`EditorProduct.props` 导入它并增加 ImGui 配置与 Editor 功能闭包；Player 使用共同产品配置。产品通过 `InnoProductBuildProperties` 注入，直接及 SDK 自动追加的间接引用采用相同配置。Shader 工具的执行宿主与产物平台分别表达，BGFX 和 ImGui backend 不根据当前 OS 选择目标。新增 CPU 应根据真实 SDK 与图形编译要求复用或提供配置，并完成实机验证。

MacOS SDK、已实现发布目标、Support Pack、布局和模板的唯一平台 owner。组件源码、Native facade 和第三方 recipe 属于各 backend。

## 组合、生命周期与扩展

`MacOSArm64BuildModule.CreateContribution` 产生绑定完整的目标贡献；`MacOSArm64NativeToolchainProvider` 根据显式 BuildHostDescriptor 和目标解析 SDK。运行目标为 MacOSArm64/osx-arm64，不由当前进程 CPU 推导。

Support Pack source 复用共同 FilePlayerSupportPackPreparation，注入产品项目、模板、平台布局、SDK provider 和精确 Native plan。validator 在提交前核对源代码、generator、managed/native 闭包与平台文件类型。最终玩家布局由对应 target 创建；失败和取消保留旧完整输出。

发布模板使用 MSBuild 的路径规范化组合 `PublishDir` 与精确 Native 相对路径，不依赖末尾分隔符。Pack 本体与 `PlayerLink` 各保留一份相同内容的 Native 输入，分别服务部署和最终链接；CI 只检查 `current` 选中的完整 generation，逐一校验两处文件数量与内容哈希。

新增同平台 CPU 先实现真实 SDK、ABI、布局与 runtime 支持，再增加目标贡献；不复制产品、Editor 或共享 backend。打包/签名只有存在真实实现时增加文件，不建立空目录。

本项目为库，无 Program；共享 Build 不反向引用它。测试通过公开 target/provider/source 验证正向与缺 SDK、错宿主、错误闭包路径。

## 平台基础与集成

本项目不引用具体 backend，也不持有内容 compiler。BGFX 的 Native/Shader profile 与内容工厂移入同平台 integration；完整注册由 Standard Distribution 绑定。打包工厂不接收创作服务。产品 props 只声明产品编译选择，不承担 SDK 或第三方实现。

## 当前源码公开 API 清单

签名来自当前 SDK 求值与语义符号；按类型列出 public/protected 契约。工作流与生命周期见本页前文。

### `Inno.Build.MacOSArm64.MacOSArm64BuildModule`

<!-- api: T:Inno.Build.MacOSArm64.MacOSArm64BuildModule 2344e03f97d417eb757d9d85672d0dd729eca207b90a378fab2ce0bb89438f64 -->
<!-- api: P:Inno.Build.MacOSArm64.MacOSArm64BuildModule.target 2b865622017ea3302598c0856a9f32b652417536cc7b7468e983c19f45933563 -->
<!-- api: M:Inno.Build.MacOSArm64.MacOSArm64BuildModule.CreateContribution(Inno.Build.Composition.BuildCompositionContext,Inno.Build.Toolchains.ProductNativeBuildPlan,Inno.Build.Toolchains.ProductNativeBuildPlan,Inno.Build.Toolchains.ProductNativeBuildPlan,Inno.Build.IPlayerSupportPackValidator) 7f05a84c29cc3ce372a7d19f5f2bcaec859bee6da94529c36bbb7272151246d7 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-df6c31b1ad5dd883"></a>[`public static Inno.Build.Composition.BuildPlatformContribution Inno.Build.MacOSArm64.MacOSArm64BuildModule.CreateContribution(Inno.Build.Composition.BuildCompositionContext context, Inno.Build.Toolchains.ProductNativeBuildPlan nativePlan, Inno.Build.Toolchains.ProductNativeBuildPlan editorNativePlan, Inno.Build.Toolchains.ProductNativeBuildPlan shaderToolsPlan, Inno.Build.IPlayerSupportPackValidator supportPackValidator)`](../../../platforms/MacOSArm64/build/Inno.Build.MacOSArm64/MacOSArm64BuildModule.cs#L39) | Contributes the platform's target, product inputs and SDK resolver as one registration.<br>返回：A complete immutable contribution without starting tools or creating files.<br>`context`：The borrowed SDK and execution-host selection.<br>`nativePlan`：The selected shared Player component closure.<br>`editorNativePlan`：The explicitly selected Editor component closure for ordinary product builds.<br>`shaderToolsPlan`：The host-executed offline compiler product for this explicit target.<br>`supportPackValidator`：The complete borrowed platform and selected component validator. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-10145138dd0b02a4"></a>[`public static Inno.Build.PlatformTargetDescriptor Inno.Build.MacOSArm64.MacOSArm64BuildModule.target { get; }`](../../../platforms/MacOSArm64/build/Inno.Build.MacOSArm64/MacOSArm64BuildModule.cs#L15) | Gets the explicit implemented product target, independent of the tool execution host. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-696eadcfdd89a7a4"></a>[`public static class Inno.Build.MacOSArm64.MacOSArm64BuildModule`](../../../platforms/MacOSArm64/build/Inno.Build.MacOSArm64/MacOSArm64BuildModule.cs#L10) | Owns the implemented MacOS target facts and complete publication contribution. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Build.MacOSArm64.MacOSArm64GameBuildTarget`

<!-- api: T:Inno.Build.MacOSArm64.MacOSArm64GameBuildTarget 809612a1c26bdfbc3c22c0b9dcd480c8fb2626bc3e0840cf621592daacf906ed -->
<!-- api: M:Inno.Build.MacOSArm64.MacOSArm64GameBuildTarget.#ctor(Inno.Build.IPlayerSupportPackValidator) 9a622ec007be695ffba4e34962e8bf0d077988c1600075c0e777340d682b5b22 -->
<!-- api: M:Inno.Build.MacOSArm64.MacOSArm64GameBuildTarget.Validate(System.String) 196a43d9f503197c994a673d33bc3c11fd76e847eba3959ec6f6df59ba85b41f -->
<!-- api: P:Inno.Build.MacOSArm64.MacOSArm64GameBuildTarget.id b48a2fd9b753e1e6737168f40517ac073ad14255483dfd51080e0d6b2ab1a922 -->
<!-- api: P:Inno.Build.MacOSArm64.MacOSArm64GameBuildTarget.defaultManagedDeployment 72ae248ef23f40077bffe1ca72eceaa93faa60d3d8a02a38de2cbfe08cdfd6b7 -->
<!-- api: P:Inno.Build.MacOSArm64.MacOSArm64GameBuildTarget.runtimeIdentifier 6fa533e52ff34e3787c70c1ab2dc24db6b4ed26dfd21a7be0e14cfb5064d127f -->
<!-- api: P:Inno.Build.MacOSArm64.MacOSArm64GameBuildTarget.displayName 33256e52522c73ea38c74a093a06fc12f20220d0b208b488963640a564381a4f -->
<!-- api: M:Inno.Build.MacOSArm64.MacOSArm64GameBuildTarget.PackageAsync(Inno.Build.GameBuildPackageContext,System.Threading.CancellationToken) 3c9d56578d16aeff3cc63c36efd2237bf82d0919792748ae910e6f1d301a1fe6 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-832f3e6eeb2dfd77"></a>[`public Inno.Build.MacOSArm64.MacOSArm64GameBuildTarget.MacOSArm64GameBuildTarget(Inno.Build.IPlayerSupportPackValidator supportPackValidator)`](../../../platforms/MacOSArm64/build/Inno.Build.MacOSArm64/Targets/MacOSArm64GameBuildTarget.cs#L26) | Creates a platform packager with the complete validator selected by its distribution.<br>`supportPackValidator`：The borrowed platform and component-closure validator. | 约束由所属类型与本页生命周期定义。 | `System.ArgumentNullException`：The validator is null. |
| <a id="api-f4ebd03d132ed6fd"></a>[`public System.Threading.Tasks.ValueTask<string> Inno.Build.MacOSArm64.MacOSArm64GameBuildTarget.PackageAsync(Inno.Build.GameBuildPackageContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../platforms/MacOSArm64/build/Inno.Build.MacOSArm64/Targets/MacOSArm64GameBuildTarget.cs#L73) | Composes a macOS application bundle in isolated staging.<br>返回：The staged application bundle path.<br>`context`：The verified Support Pack, packaged content, and output staging paths.<br>`cancellationToken`：The token that cancels composition before commit. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-9f3fec00f10206d4"></a>[`public void Inno.Build.MacOSArm64.MacOSArm64GameBuildTarget.Validate(string directory)`](../../../platforms/MacOSArm64/build/Inno.Build.MacOSArm64/Targets/MacOSArm64GameBuildTarget.cs#L41) | Validates the target closure before runtime script compilation.<br>`directory`：The Support Pack directory selected by the build catalog. | 约束由所属类型与本页生命周期定义。 | `System.IO.InvalidDataException`：The required platform inputs are incomplete or incompatible. |
| <a id="api-b5ed5e67a7ed238c"></a>[`public Inno.Build.Managed.ManagedDeploymentId Inno.Build.MacOSArm64.MacOSArm64GameBuildTarget.defaultManagedDeployment { get; }`](../../../platforms/MacOSArm64/build/Inno.Build.MacOSArm64/Targets/MacOSArm64GameBuildTarget.cs#L49) | Gets the managed publisher selected when the profile does not specify an override. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-a4081fbcf5bda9f1"></a>[`public string Inno.Build.MacOSArm64.MacOSArm64GameBuildTarget.displayName { get; }`](../../../platforms/MacOSArm64/build/Inno.Build.MacOSArm64/Targets/MacOSArm64GameBuildTarget.cs#L57) | Gets the target name presented by authoring hosts. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-0edf1067fb6c8f71"></a>[`public Inno.Build.BuildTargetId Inno.Build.MacOSArm64.MacOSArm64GameBuildTarget.id { get; }`](../../../platforms/MacOSArm64/build/Inno.Build.MacOSArm64/Targets/MacOSArm64GameBuildTarget.cs#L46) | Gets the macOS ARM64 target identity. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-155e46dd320bbf98"></a>[`public string Inno.Build.MacOSArm64.MacOSArm64GameBuildTarget.runtimeIdentifier { get; }`](../../../platforms/MacOSArm64/build/Inno.Build.MacOSArm64/Targets/MacOSArm64GameBuildTarget.cs#L52) | Gets the managed toolchain target identifier required by this platform composition. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-30d57e0cfc7fae92"></a>[`public sealed class Inno.Build.MacOSArm64.MacOSArm64GameBuildTarget : Inno.Build.IGameBuildTarget`](../../../platforms/MacOSArm64/build/Inno.Build.MacOSArm64/Targets/MacOSArm64GameBuildTarget.cs#L13) | Packages a verified macOS ARM64 Support Pack as a native application bundle. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Build.MacOSArm64.MacOSArm64NativeToolchainProvider`

<!-- api: T:Inno.Build.MacOSArm64.MacOSArm64NativeToolchainProvider 16a5766b5d77bccf9a2ac9c84b9793f1d05b4f31f48db2a59c02f951c0602461 -->
<!-- api: M:Inno.Build.MacOSArm64.MacOSArm64NativeToolchainProvider.#ctor(System.String) 40846e41da32e9998c9d9e21c65e00b5ca1a5f68a8cf24690456e8b0996e89dc -->
<!-- api: M:Inno.Build.MacOSArm64.MacOSArm64NativeToolchainProvider.ResolveAsync(Inno.Build.Toolchains.NativeBuildContext,Inno.Build.Toolchains.BuildHostDescriptor,System.String,System.Threading.CancellationToken) 61a0386dbe47bababc0d78f5baa660d60978817204c9daa4016e4234f807855c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-0184dc8ab90c6544"></a>[`public Inno.Build.MacOSArm64.MacOSArm64NativeToolchainProvider.MacOSArm64NativeToolchainProvider(string dotnetHost)`](../../../platforms/MacOSArm64/build/Inno.Build.MacOSArm64/Toolchain/MacOSArm64NativeToolchainProvider.cs#L27) | Captures the explicit managed executable used by Native binding generation.<br>`dotnetHost`：The explicitly selected SDK executable path or command. | 约束由所属类型与本页生命周期定义。 | `System.ArgumentException`：The host command is empty. |
| <a id="api-fe48d86578398ae9"></a>[`public System.Threading.Tasks.ValueTask<Inno.Build.Toolchains.NativeToolchainSelection> Inno.Build.MacOSArm64.MacOSArm64NativeToolchainProvider.ResolveAsync(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.BuildHostDescriptor host, string targetId, System.Threading.CancellationToken cancellationToken)`](../../../platforms/MacOSArm64/build/Inno.Build.MacOSArm64/Toolchain/MacOSArm64NativeToolchainProvider.cs#L34) | Freezes compiler tools, SDK inputs and child environment before a native operation starts.<br>返回：A complete immutable tool selection for the requested host and target.<br>`context`：The checkout and configuration owned by the requesting operation.<br>`host`：The explicitly declared execution host, independent of the target.<br>`targetId`：The exact requested target; unsupported combinations must fail.<br>`cancellationToken`：Cancels SDK discovery before any product staging. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-4e05fea90467910c"></a>[`public sealed class Inno.Build.MacOSArm64.MacOSArm64NativeToolchainProvider : Inno.Build.Toolchains.INativeToolchainProvider`](../../../platforms/MacOSArm64/build/Inno.Build.MacOSArm64/Toolchain/MacOSArm64NativeToolchainProvider.cs#L14) | Resolves the MacOS SDK for a declared target without deriving publication policy from this process. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Build.MacOSArm64.MacOSArm64PlayerSupportPackSource`

<!-- api: T:Inno.Build.MacOSArm64.MacOSArm64PlayerSupportPackSource 4437b9de92b1f4a681838d6aa264cc9ac97e3e1f9ef19b89f50639f1ecd0ccd8 -->
<!-- api: M:Inno.Build.MacOSArm64.MacOSArm64PlayerSupportPackSource.#ctor(Inno.Build.Toolchains.ProductNativeBuildPlan,Inno.Build.Toolchains.BuildHostDescriptor,System.String,Inno.Build.Toolchains.INativeBindingGenerator,Inno.Build.IPlayerSupportPackValidator) b67bfc3c2ba71a2ab139dd4cd4a52031a9bc59036f557fdbef086702e385e308 -->
<!-- api: P:Inno.Build.MacOSArm64.MacOSArm64PlayerSupportPackSource.target e8bb94795e87b3efcd31760ab606d6bfe452a2f48d223628aac4cf04d07991c0 -->
<!-- api: M:Inno.Build.MacOSArm64.MacOSArm64PlayerSupportPackSource.CreatePlanAsync(Inno.Build.SupportPacks.PlayerSupportPackPlanningContext,System.Threading.CancellationToken) cbe9ec1c06acbd66a7310710dc9192569e3af8d6ec10d375f8d94d5f10a73a91 -->
<!-- api: M:Inno.Build.MacOSArm64.MacOSArm64PlayerSupportPackSource.Validate(System.String) ce4f8ed820ba30f1e131cbe96dbb34a95f3560d217e9229ae0af27bce37bf4b4 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-470a4a4313eb8635"></a>[`public Inno.Build.MacOSArm64.MacOSArm64PlayerSupportPackSource.MacOSArm64PlayerSupportPackSource(Inno.Build.Toolchains.ProductNativeBuildPlan nativePlan, Inno.Build.Toolchains.BuildHostDescriptor host, string dotnetHost, Inno.Build.Toolchains.INativeBindingGenerator bindingGenerator, Inno.Build.IPlayerSupportPackValidator supportPackValidator)`](../../../platforms/MacOSArm64/build/Inno.Build.MacOSArm64/SupportPacks/MacOSArm64PlayerSupportPackSource.cs#L33) | Captures the selected product closure and tool execution host.<br>`nativePlan`：The explicit Player component closure.<br>`host`：The declared machine executing platform tools.<br>`bindingGenerator`：The borrowed shared binding implementation selected by distribution.<br>`dotnetHost`：The explicit SDK executable for binding generation.<br>`supportPackValidator`：The borrowed validator selected by the distribution. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-c827347dd7f3a84a"></a>[`public System.Threading.Tasks.ValueTask<Inno.Build.SupportPacks.PlayerSupportPackPlan> Inno.Build.MacOSArm64.MacOSArm64PlayerSupportPackSource.CreatePlanAsync(Inno.Build.SupportPacks.PlayerSupportPackPlanningContext context, System.Threading.CancellationToken cancellationToken)`](../../../platforms/MacOSArm64/build/Inno.Build.MacOSArm64/SupportPacks/MacOSArm64PlayerSupportPackSource.cs#L54) | Validates discovery inputs and freezes a plan before publication output exists.<br>返回：A frozen plan with no running work; missing tools and unsupported hosts fail here.<br>`context`：The source checkout and host used for read-only SDK discovery.<br>`cancellationToken`：Cancels preparation before installation. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-353f962d007d2097"></a>[`public void Inno.Build.MacOSArm64.MacOSArm64PlayerSupportPackSource.Validate(string directory)`](../../../platforms/MacOSArm64/build/Inno.Build.MacOSArm64/SupportPacks/MacOSArm64PlayerSupportPackSource.cs#L60) | Rejects an incomplete or incompatible platform closure.<br>`directory`：The prepared or installed Support Pack directory. | 约束由所属类型与本页生命周期定义。 | `System.IO.InvalidDataException`：A required platform input is absent or incompatible. |
| <a id="api-40a37174ff79fddf"></a>[`public Inno.Build.BuildTargetId Inno.Build.MacOSArm64.MacOSArm64PlayerSupportPackSource.target { get; }`](../../../platforms/MacOSArm64/build/Inno.Build.MacOSArm64/SupportPacks/MacOSArm64PlayerSupportPackSource.cs#L51) | Gets the stable target identity implemented by this source. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-80d1c3b6048d409b"></a>[`public sealed class Inno.Build.MacOSArm64.MacOSArm64PlayerSupportPackSource : Inno.Build.SupportPacks.IPlayerSupportPackSource`](../../../platforms/MacOSArm64/build/Inno.Build.MacOSArm64/SupportPacks/MacOSArm64PlayerSupportPackSource.cs#L11) | Prepares the MacOS Player's explicit layout through shared publication mechanisms. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Build.MacOSArm64.MacOSArm64SupportPackValidator`

<!-- api: T:Inno.Build.MacOSArm64.MacOSArm64SupportPackValidator b812c35543027f7f2a71d5045b7b4ec6b75d6e845c4da80e096679a18ba7c1e0 -->
<!-- api: M:Inno.Build.MacOSArm64.MacOSArm64SupportPackValidator.Validate(System.String) 6e97b7ff9f8dcf6510aaab1ef87a7bba22e1f8d707825a3eed58ce0d975bdba0 -->
<!-- api: M:Inno.Build.MacOSArm64.MacOSArm64SupportPackValidator.#ctor 4fd3875e465bd5fde550e1c1965fcf770184549d34e15f41ff4c3612b1139985 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2a735687b0580758"></a>[`public Inno.Build.MacOSArm64.MacOSArm64SupportPackValidator.MacOSArm64SupportPackValidator()`](../../../platforms/MacOSArm64/build/Inno.Build.MacOSArm64/SupportPacks/MacOSArm64SupportPackValidator.cs#L10) | C# 生成的声明；状态与值语义遵循所属类型契约。 | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-1450c70abfbae298"></a>[`public void Inno.Build.MacOSArm64.MacOSArm64SupportPackValidator.Validate(string directory)`](../../../platforms/MacOSArm64/build/Inno.Build.MacOSArm64/SupportPacks/MacOSArm64SupportPackValidator.cs#L21) | Rejects incomplete or foreign platform inputs in the supplied Support Pack.<br>`directory`：The isolated or installed target directory to validate. | 约束由所属类型与本页生命周期定义。 | `System.IO.InvalidDataException`：A required input is absent or the closure contains a foreign native binary. |
| <a id="api-b54dd9ab61f50da5"></a>[`public sealed class Inno.Build.MacOSArm64.MacOSArm64SupportPackValidator : Inno.Build.IPlayerSupportPackValidator`](../../../platforms/MacOSArm64/build/Inno.Build.MacOSArm64/SupportPacks/MacOSArm64SupportPackValidator.cs#L10) | Validates the platform runtime or linker closure before compilation and installation. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

## 项目依赖

- [Inno.Build](../../build/Inno.Build.md)：公开引用边界由实际签名核对。
- [Inno.Build.Managed](../../build/Inno.Build.Managed.md)：公开引用边界由实际签名核对。
- [Inno.Build.Composition](../../build/Inno.Build.Composition.md)：公开引用边界由实际签名核对。
- [Inno.Build.SupportPacks.Core](../../build/Inno.Build.SupportPacks.Core.md)：公开引用边界由实际签名核对。
- [Inno.Build.Toolchains](../../build/Inno.Build.Toolchains.md)：公开引用边界由实际签名核对。
