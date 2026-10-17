# Inno.Build.Composition

[分类索引](README.md) · [Wiki 首页](../README.md) · [平台归属与扩展](../architecture/PLATFORM_EXTENSION_GUIDE.md)

## 职责与边界

中立的构建组合契约。只依赖共同 Build、Managed、Support Pack、Toolchains 及必要创作输入，不注册平台、DotNet compiler 或 backend。

## 组合、生命周期与扩展

`BuildPlatformContribution` 将目标描述、打包工厂、Support Pack source、SDK provider 和可选 Editor 项目绑定成一个平台贡献。`GameBuildContribution` 再绑定内容编译工厂；`BuildDistribution` 接收完整游戏贡献，拒绝重复 ID、错配 source、错误 Editor 能力和重复 Native-only ID。没有目录扫描或自动发现。

`BuildDistribution.TryResolveNativeToolchain(targetId, out provider)` 只查询已注册的同目标 SDK 贡献。独立绑定定义可以不注册游戏发布目标；查询失败不会选择其他目标，也不屏蔽已注册 provider 的解析失败。

`BuildPipelineFactory.Create` 接收借用的作者端服务与冻结 distribution。运行目标和 `BuildCompositionContext.host` 分离；`toolsTarget` 描述作者工具的执行目标，不替代游戏目标。factory、catalog 与 provider 为借用；Pipeline 拥有每次构建事务，source 拥有准备任务。

具体注册属于 [标准发行组合](Inno.Build.Distribution.Standard.md)。共享 Editor Hosting 接收此项目的 distribution，不引用标准发行组合。新增发行或平台先组成完整贡献，然后在自己的发行入口注册。

```csharp
using System.Collections.Generic;
using Inno.Build.Composition;
using Inno.Build.Managed;

static BuildDistribution Compose(
    IReadOnlyList<GameBuildContribution> contributions,
    IReadOnlyList<IManagedDeploymentCompiler> deployments
) {
    return new BuildDistribution(contributions, deployments);
}
```

没有额外 protected 扩展点。SDK 不支持宿主/目标组合时明确失败；发布前的失败和取消保留旧完整输出。契约测试位于 `tests/build/Inno.Build.Tests/BuildCompositionTests.cs`。

平台贡献还声明目标自身的不可变 `nativeProducts`。`BuildDistribution.ResolveNativeProduct(targetId, productId)` 是普通 MSBuild Native 准备的唯一选择入口；Task 不维护 Editor/Player 的 backend 列表或产品 switch。不同平台可以给同一 product ID 选择不同闭包；未注册的产品在 SDK 解析和 staging 前明确失败。Browser 的聚合/最终链接由自身发布链处理，不注册普通 Editor Native 构建。

## 生成服务组合

BuildCompositionContext 必须提供 INativeBindingGenerator。平台目标和 Support Pack 只借用此服务；BuildPipelineFactory 将其传递给完整执行链，不引用具体 BGCS 实现。

## 一次组合的检查顺序

1. 创建 `BuildDistribution` 时冻结完整贡献，拒绝重复目标和部署实现。此时不启动 SDK、生成器或编译器。
2. `CreateBindings` 调用平台 packager 工厂，核对 ID、RID 和默认部署能力，再用当前 Assets、SerializationRegistry、TypeCatalog 创建内容 compiler。compiler 与 packager 必须具有相同目标 ID。
3. `BuildPipelineFactory.Create` 将这些绑定与当前创作服务交给 Pipeline。它不会接管 EngineHost、Assets、PluginEnvironment、Settings 或 ScriptCompiler 的释放权。
4. 请求发布时才解析所选部署与 Support Pack，并进入 Pipeline 的构建事务。注册一个目标不表示当前机器已经安装其 SDK。

`BuildDistribution` 的工厂和平台配置可以是长生命周期的不可变配置；`CreateBindings` 的结果借用了当前创作服务，不能把它当作跨 Session 的静态内容 compiler 缓存。创作 Host 必须等待使用这些服务的构建任务结束后再退休其资源。

扩展时分别提供平台贡献和内容编译工厂，再组成 `GameBuildContribution`。替换图形内容 compiler 不要求修改平台 packager；替换 SDK provider 也不要求修改共同 Pipeline。只有产品确实支持的 Native 闭包才加入 `nativeProducts`，不能凭目标名称自动推导。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Build.Composition.BuildCompositionContext`

<!-- api: T:Inno.Build.Composition.BuildCompositionContext ddfa616d6ce0d6008cfd6940196a7b1b48e39e9a80d928efd4c35b428306216d -->
<!-- api: M:Inno.Build.Composition.BuildCompositionContext.#ctor(System.String,System.String,Inno.Build.Toolchains.BuildHostDescriptor,Inno.Build.BuildTargetId,Inno.Build.Toolchains.INativeBindingGenerator) ffd7beee7c3ecc31d7d2e25303e57ecb0490e1d627ff3090bbf346c32e030889 -->
<!-- api: P:Inno.Build.Composition.BuildCompositionContext.dotnetHost 3b760e14e02f360d825bfa87efcc4079cff517917dce139cef74d779ddb8e482 -->
<!-- api: P:Inno.Build.Composition.BuildCompositionContext.applicationDirectory 5001204ec8771c0ce709ebecc6a8b9d95e6bc89eaf469cf30101f86446996c55 -->
<!-- api: P:Inno.Build.Composition.BuildCompositionContext.host 123da1cdeecb0904f926f8d63ecad1fd978a494efcca5cf9313e0aa6c6a8d1e0 -->
<!-- api: P:Inno.Build.Composition.BuildCompositionContext.toolsTarget 454a492de064383d4da549f83ada547620e72f2b054d9496c27736dd55180411 -->
<!-- api: P:Inno.Build.Composition.BuildCompositionContext.bindingGenerator 50fa09b220f6deeb3fc43507901f9ed241539a90e819fde127f627ba6a262b4b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-67d07d51f57277a7"></a>[`public Inno.Build.Composition.BuildCompositionContext.BuildCompositionContext(string dotnetHost, string applicationDirectory, Inno.Build.Toolchains.BuildHostDescriptor host, Inno.Build.BuildTargetId toolsTarget, Inno.Build.Toolchains.INativeBindingGenerator bindingGenerator)`](../../build/composition/Inno.Build.Composition/BuildCompositionContext.cs#L33) | Captures the host dependencies used by deployment and Support Pack preparation.<br>`dotnetHost`：The SDK executable selected by the host, either an executable path or a command name.<br>`applicationDirectory`：The absolute directory from which source-based Support Pack provisioning can be resolved.<br>`host`：The explicitly declared tool execution machine, independent of any publication target.<br>`toolsTarget`：The explicit native target of tools executed by this composition, independent of game publication.<br>`bindingGenerator`：The borrowed shared generation service, independent of the build host and platform. | — | `System.ArgumentException`：The SDK command is blank or the application directory is not absolute. |
| <a id="api-bbab17cf216d8b8c"></a>[`public string Inno.Build.Composition.BuildCompositionContext.applicationDirectory { get; }`](../../build/composition/Inno.Build.Composition/BuildCompositionContext.cs#L63) | Gets the host location used for source provisioning discovery. | — | — |
| <a id="api-3742b583658dee26"></a>[`public Inno.Build.Toolchains.INativeBindingGenerator Inno.Build.Composition.BuildCompositionContext.bindingGenerator { get; }`](../../build/composition/Inno.Build.Composition/BuildCompositionContext.cs#L78) | Gets the borrowed generation provider selected once by distribution composition. | — | — |
| <a id="api-9469a32a6a2d9ba2"></a>[`public string Inno.Build.Composition.BuildCompositionContext.dotnetHost { get; }`](../../build/composition/Inno.Build.Composition/BuildCompositionContext.cs#L58) | Gets the explicitly selected SDK executable. | — | — |
| <a id="api-14f88865af3ac062"></a>[`public Inno.Build.Toolchains.BuildHostDescriptor Inno.Build.Composition.BuildCompositionContext.host { get; }`](../../build/composition/Inno.Build.Composition/BuildCompositionContext.cs#L68) | Gets the declared execution host without selecting a product target. | — | — |
| <a id="api-7d51b7e9a65113b5"></a>[`public Inno.Build.BuildTargetId Inno.Build.Composition.BuildCompositionContext.toolsTarget { get; }`](../../build/composition/Inno.Build.Composition/BuildCompositionContext.cs#L73) | Gets the independently declared native target of build tools executed by this host. | — | — |
| <a id="api-0062aa326cdf4f6b"></a>[`public sealed class Inno.Build.Composition.BuildCompositionContext`](../../build/composition/Inno.Build.Composition/BuildCompositionContext.cs#L10) | Captures host-owned SDK selection and application location without probing global process state. | — | — |

### `Inno.Build.Composition.BuildDistribution`

<!-- api: T:Inno.Build.Composition.BuildDistribution 79d14f78883dbf822163ea1913d9660cdd8e3c800bf06a389fcdefa6c939bbb2 -->
<!-- api: M:Inno.Build.Composition.BuildDistribution.#ctor(System.Collections.Generic.IReadOnlyList{Inno.Build.Composition.GameBuildContribution},System.Collections.Generic.IReadOnlyList{Inno.Build.Managed.IManagedDeploymentCompiler},System.Collections.Generic.IReadOnlyList{Inno.Build.Composition.NativeToolchainContribution}) 3cb54eddc5372a9c04556c480856a6080571c7dc01943601a2d54249aee44938 -->
<!-- api: P:Inno.Build.Composition.BuildDistribution.availableTargets 0e8b0b4d964321e803379f95e3b7c7d7ba4daa2aa961f2bf73b740d74f98323b -->
<!-- api: P:Inno.Build.Composition.BuildDistribution.platforms 0735bdebd3a2e9bdf3a779377f1a8b862c9aea0e79a99b97aa606a6c6774ae9e -->
<!-- api: P:Inno.Build.Composition.BuildDistribution.managedDeployments 0a78dbc2ea24a586f28ac3d1740f63cd123a51b2fb20a9964d4b8c26302e1ffa -->
<!-- api: P:Inno.Build.Composition.BuildDistribution.supportPacks d4023e1b396ef3e8d0bfad902d9fa549516177339f60ecc87ed010cb1b178dfb -->
<!-- api: M:Inno.Build.Composition.BuildDistribution.ResolveNativeToolchain(System.String) 331c150611e0de890f268195ffcbe7cc4257d18e464aedcc611db40e3659d0a1 -->
<!-- api: M:Inno.Build.Composition.BuildDistribution.TryResolveNativeToolchain(System.String,Inno.Build.Toolchains.INativeToolchainProvider@) 8a884e540338200b8da920e09df669e42649ae2a99c6b95e1bbf83469eed4825 -->
<!-- api: M:Inno.Build.Composition.BuildDistribution.ResolveNativeProduct(System.String,System.String) 1a5a1314ee5a6521e8a24d0ae18f053b5f49061678dc7c95c50abccd8ee50627 -->
<!-- api: M:Inno.Build.Composition.BuildDistribution.CreateBindings(Inno.Assets.Pipeline.AssetPipeline,Inno.Core.Serialization.SerializationRegistry,Inno.Extensibility.Types.TypeCatalog) 24d2a24338f4021e99e6f764fc15443f0fc48dec3d1d42be2add14a4504481cf -->
<!-- api: M:Inno.Build.Composition.BuildDistribution.ValidateDeployment(Inno.Build.IGameBuildTarget,Inno.Build.Managed.ManagedDeploymentId) 42d80b879ef29b4ac7ec314bdd79459be70ff431b791441ef9a29ef7c36afdaa -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-6f3ae276221be033"></a>[`public Inno.Build.Composition.BuildDistribution.BuildDistribution(System.Collections.Generic.IReadOnlyList<Inno.Build.Composition.GameBuildContribution> games, System.Collections.Generic.IReadOnlyList<Inno.Build.Managed.IManagedDeploymentCompiler> managedCompilers, System.Collections.Generic.IReadOnlyList<Inno.Build.Composition.NativeToolchainContribution>? nativeOnlyContributions = null)`](../../build/composition/Inno.Build.Composition/BuildDistribution.cs#L44) | Captures a complete distribution without creating authoring services or starting any tool.<br>`games`：Complete target contributions with explicitly bound factories, sources and SDK providers.<br>`managedCompilers`：The deployment implementations owned by this composition.<br>`nativeOnlyContributions`：Additional implemented SDK capabilities that do not register game publication targets. | — | `System.ArgumentNullException`：A required collection is null.<br>`System.ArgumentException`：Contributions are empty, null, duplicated, or unassigned. |
| <a id="api-c6da94a0fbd266ac"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Build.GameBuildTargetBinding> Inno.Build.Composition.BuildDistribution.CreateBindings(Inno.Assets.Pipeline.AssetPipeline assets, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Extensibility.Types.TypeCatalog types)`](../../build/composition/Inno.Build.Composition/BuildDistribution.cs#L190) | Creates platform targets and validates their identities and default deployment capabilities.<br>返回：An immutable target collection ready for pipeline construction.<br>`assets`：The active borrowed authoring asset owner.<br>`serialization`：The matching borrowed serialization registry.<br>`types`：The matching borrowed extension catalog. | — | `System.InvalidOperationException`：A factory returns no target, targets disagree with Support Pack identities, or a default managed deployment cannot publish its platform. |
| <a id="api-aca5b758ccee4d5a"></a>[`public Inno.Build.Toolchains.ProductNativeBuildPlan Inno.Build.Composition.BuildDistribution.ResolveNativeProduct(string targetId, string productId)`](../../build/composition/Inno.Build.Composition/BuildDistribution.cs#L159) | Resolves a platform-contributed product closure without selecting backends inside the invoking host.<br>返回：The borrowed immutable plan declared by the target's composition.<br>`targetId`：The explicit registered product target.<br>`productId`：The product whose ordinary native build is requested. | — | `System.NotSupportedException`：The target or its ordinary native product build is not registered.<br>`System.ArgumentException`：A target or product identity is empty. |
| <a id="api-2a30f01d0b267f9f"></a>[`public Inno.Build.Toolchains.INativeToolchainProvider Inno.Build.Composition.BuildDistribution.ResolveNativeToolchain(string targetId)`](../../build/composition/Inno.Build.Composition/BuildDistribution.cs#L109) | Resolves the SDK integration bound to an explicitly registered publication target.<br>返回：The borrowed provider; resolving it freezes tools before component work begins.<br>`targetId`：The requested native target identity, independently selected from the execution host. | — | `System.NotSupportedException`：No native toolchain contribution is registered for the target. |
| <a id="api-18d6ee306133472a"></a>[`public bool Inno.Build.Composition.BuildDistribution.TryResolveNativeToolchain(string targetId, out Inno.Build.Toolchains.INativeToolchainProvider? provider)`](../../build/composition/Inno.Build.Composition/BuildDistribution.cs#L133) | Checks whether this distribution contributes an SDK for an explicitly requested binding target. A standalone binding definition may describe a target without registering a product publisher.<br>返回：True when the target has an explicit SDK contribution; false does not imply SDK-free configuration.<br>`targetId`：The exact target identity; no host-derived target is substituted.<br>`provider`：The borrowed SDK provider when registered, or null when the distribution has no such contribution. | — | `System.ArgumentException`：The target identity is empty. |
| <a id="api-b5dd9ba0375c9138"></a>[`public void Inno.Build.Composition.BuildDistribution.ValidateDeployment(Inno.Build.IGameBuildTarget target, Inno.Build.Managed.ManagedDeploymentId deployment)`](../../build/composition/Inno.Build.Composition/BuildDistribution.cs#L228) | Rejects unsupported target and deployment pairs before any staging or publication begins.<br>`target`：A target composed by this distribution.<br>`deployment`：The explicit deployment selected for publication. | — | `System.InvalidOperationException`：The platform is absent or the deployment lacks its runtime capability. |
| <a id="api-d8e938dd581831d0"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Build.BuildTargetId> Inno.Build.Composition.BuildDistribution.availableTargets { get; }`](../../build/composition/Inno.Build.Composition/BuildDistribution.cs#L80) | Gets the immutable platform identities shared by all hosts. | — | — |
| <a id="api-d7a161536e62d231"></a>[`public Inno.Build.Managed.ManagedDeploymentCatalog Inno.Build.Composition.BuildDistribution.managedDeployments { get; }`](../../build/composition/Inno.Build.Composition/BuildDistribution.cs#L90) | Gets the immutable deployment implementation catalog. | — | — |
| <a id="api-9cb03e24f37555f3"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Build.Composition.BuildPlatformContribution> Inno.Build.Composition.BuildDistribution.platforms { get; }`](../../build/composition/Inno.Build.Composition/BuildDistribution.cs#L85) | Gets complete platform registrations in their explicit composition order. | — | — |
| <a id="api-a2b64727108ba31a"></a>[`public Inno.Build.SupportPacks.PlayerSupportPackPublisher Inno.Build.Composition.BuildDistribution.supportPacks { get; }`](../../build/composition/Inno.Build.Composition/BuildDistribution.cs#L95) | Gets the publisher containing the same platform input providers. | — | — |
| <a id="api-762eabbced8955c7"></a>[`public sealed class Inno.Build.Composition.BuildDistribution`](../../build/composition/Inno.Build.Composition/BuildDistribution.cs#L17) | Freezes platform factories, managed publishers, and matching Support Pack sources as one distribution. | — | — |

### `Inno.Build.Composition.BuildPipelineFactory`

<!-- api: T:Inno.Build.Composition.BuildPipelineFactory e5b8297ea65c27399b6a37c477d86ef44956c6f46c47ceed7d499c7baa156152 -->
<!-- api: M:Inno.Build.Composition.BuildPipelineFactory.Create(Inno.Build.Composition.BuildCompositionContext,Inno.Build.Composition.BuildDistribution,Inno.Runtime.EngineHost,Inno.Assets.Pipeline.AssetPipeline,Inno.Plugins.Authoring.PluginEnvironment,Inno.Core.Settings.ProjectSettingsStore,Inno.Scripting.Compiler.ScriptCompiler,System.String) f89da029c21a6694d858d40b3930be8d0fc205114bb3596a3448c6e830770a39 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-709f5830a832fada"></a>[`public static Inno.Build.BuildPipeline Inno.Build.Composition.BuildPipelineFactory.Create(Inno.Build.Composition.BuildCompositionContext context, Inno.Build.Composition.BuildDistribution distribution, Inno.Runtime.EngineHost engine, Inno.Assets.Pipeline.AssetPipeline assets, Inno.Plugins.Authoring.PluginEnvironment plugins, Inno.Core.Settings.ProjectSettingsStore settings, Inno.Scripting.Compiler.ScriptCompiler compiler, string supportPackRoot)`](../../build/composition/Inno.Build.Composition/BuildPipelineFactory.cs#L46) | Validates and binds the distribution to one authoring host without starting a build.<br>返回：A pipeline retaining the supplied owners without taking responsibility for their disposal.<br>`context`：The SDK selection and host location used for optional source provisioning.<br>`distribution`：The frozen platform and deployment registrations shared by all entry points.<br>`engine`：The borrowed engine owning type, serialization, and generation transactions.<br>`assets`：The borrowed active authoring assets.<br>`plugins`：The borrowed active plugin generation.<br>`settings`：The borrowed project settings owner.<br>`compiler`：The borrowed compiler that freezes project code for each publication.<br>`supportPackRoot`：The host's installed Support Pack location. | — | — |
| <a id="api-93b45356aa16263c"></a>[`public static class Inno.Build.Composition.BuildPipelineFactory`](../../build/composition/Inno.Build.Composition/BuildPipelineFactory.cs#L14) | Creates common build pipelines from a single distribution and explicitly borrowed authoring owners. | — | — |

### `Inno.Build.Composition.BuildPlatformContribution`

<!-- api: T:Inno.Build.Composition.BuildPlatformContribution e379ff8dfce63c50e47e0be68586248eb051cfd223c1ed2992acd9df278821ef -->
<!-- api: M:Inno.Build.Composition.BuildPlatformContribution.#ctor(Inno.Build.PlatformTargetDescriptor,Inno.Build.Composition.BuildTargetFactory,Inno.Build.SupportPacks.IPlayerSupportPackSource,Inno.Build.Toolchains.INativeToolchainProvider,System.String,System.Collections.Generic.IReadOnlyList{Inno.Build.Toolchains.ProductNativeBuildPlan}) 0579a85ae9ab384d0db46ed926b4c1521cc7f729d5d85563e9908fb565ce4314 -->
<!-- api: P:Inno.Build.Composition.BuildPlatformContribution.descriptor 413c79c342ccd47b16e43df7ef191fc6cda1492f1865bf1f0963702f51a35b19 -->
<!-- api: P:Inno.Build.Composition.BuildPlatformContribution.factory 1aece24c0c5e8eb5b2c2e17ff30efa7a1ad2f88599f681793911ca753c0b98fd -->
<!-- api: P:Inno.Build.Composition.BuildPlatformContribution.supportPackSource 864dbf19b7e68a7ee74b933d02f5633b57ea2d3313c421179889e3c59ec0abe3 -->
<!-- api: P:Inno.Build.Composition.BuildPlatformContribution.toolchainProvider e8c3f06925c96da482583441a8648eae05d27b4176952396ac694cd91c341fbc -->
<!-- api: P:Inno.Build.Composition.BuildPlatformContribution.editorProject 25c15ac885014c8c909e280d0518873135a1aa9a0fe0fd4aa82378a3ea4cefd8 -->
<!-- api: P:Inno.Build.Composition.BuildPlatformContribution.nativeProducts 9203ef3dd535c9bf3f47c360898fcd12b005d2bfcba71e6f29a336e94d311d88 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a8bbcb5b62b87a49"></a>[`public Inno.Build.Composition.BuildPlatformContribution.BuildPlatformContribution(Inno.Build.PlatformTargetDescriptor descriptor, Inno.Build.Composition.BuildTargetFactory factory, Inno.Build.SupportPacks.IPlayerSupportPackSource supportPackSource, Inno.Build.Toolchains.INativeToolchainProvider toolchainProvider, string? editorProject = null, System.Collections.Generic.IReadOnlyList<Inno.Build.Toolchains.ProductNativeBuildPlan>? nativeProducts = null)`](../../build/composition/Inno.Build.Composition/BuildPlatformContribution.cs#L43) | Creates a complete target registration and rejects mismatched source identities immediately.<br>`descriptor`：The immutable target facts owned by the platform package.<br>`factory`：The authoring target factory for these exact target facts.<br>`supportPackSource`：The matching runtime and compiler input source.<br>`toolchainProvider`：The SDK resolver for this target, independent of the host.<br>`editorProject`：The portable checkout-relative Editor product, or null when no Editor is supported.<br>`nativeProducts`：Explicit product-native plans owned by this target; null declares no ordinary native product build. | — | `System.ArgumentNullException`：A required contribution is null.<br>`System.ArgumentException`：A source identity differs from its target, an Editor declaration is inconsistent, or native product plans are null or duplicated. |
| <a id="api-d97453467d7b4c2f"></a>[`public Inno.Build.PlatformTargetDescriptor Inno.Build.Composition.BuildPlatformContribution.descriptor { get; }`](../../build/composition/Inno.Build.Composition/BuildPlatformContribution.cs#L78) | Gets the platform-owned, explicit target mapping. | — | — |
| <a id="api-c5572f0e44d46ac7"></a>[`public string? Inno.Build.Composition.BuildPlatformContribution.editorProject { get; }`](../../build/composition/Inno.Build.Composition/BuildPlatformContribution.cs#L98) | Gets the implemented Editor product location without deriving a project from target naming. | — | — |
| <a id="api-47c0d6d31dab95dd"></a>[`public Inno.Build.Composition.BuildTargetFactory Inno.Build.Composition.BuildPlatformContribution.factory { get; }`](../../build/composition/Inno.Build.Composition/BuildPlatformContribution.cs#L83) | Gets the matching authoring target factory. | — | — |
| <a id="api-65e7f6f9e7a7810a"></a>[`public System.Collections.Generic.IReadOnlyDictionary<string, Inno.Build.Toolchains.ProductNativeBuildPlan> Inno.Build.Composition.BuildPlatformContribution.nativeProducts { get; }`](../../build/composition/Inno.Build.Composition/BuildPlatformContribution.cs#L103) | Gets the frozen ordinary product-build closures explicitly contributed by this target. | — | — |
| <a id="api-3c9831053813332e"></a>[`public Inno.Build.SupportPacks.IPlayerSupportPackSource Inno.Build.Composition.BuildPlatformContribution.supportPackSource { get; }`](../../build/composition/Inno.Build.Composition/BuildPlatformContribution.cs#L88) | Gets the matching product input source. | — | — |
| <a id="api-373e14f9906d74b9"></a>[`public Inno.Build.Toolchains.INativeToolchainProvider Inno.Build.Composition.BuildPlatformContribution.toolchainProvider { get; }`](../../build/composition/Inno.Build.Composition/BuildPlatformContribution.cs#L93) | Gets the explicit SDK resolver for this target. | — | — |
| <a id="api-c2c41402f03da6ab"></a>[`public sealed class Inno.Build.Composition.BuildPlatformContribution`](../../build/composition/Inno.Build.Composition/BuildPlatformContribution.cs#L13) | Binds one implemented target to its factory, Support Pack source and SDK provider as a single registration. | — | — |

### `Inno.Build.Composition.BuildTargetFactory`

<!-- api: T:Inno.Build.Composition.BuildTargetFactory cc01359deafd042bb96500e87de33aae3da766ffd914e53efd946c6bb52dbde1 -->
<!-- api: M:Inno.Build.Composition.BuildTargetFactory.#ctor(System.Object,System.IntPtr) 54ebbe9166e5dafe725732d98882308ed1dcda0022b5c37cda3df79b8c4aed69 -->
<!-- api: M:Inno.Build.Composition.BuildTargetFactory.Invoke c35b05255cf2b50d0b413ba4e2f4d8b90f424766de3b1d26cf198a15779c9117 -->
<!-- api: M:Inno.Build.Composition.BuildTargetFactory.BeginInvoke(System.AsyncCallback,System.Object) ebb8b93157f5661093281e74397984c6e501d7edf83d451ced40f016525f5e63 -->
<!-- api: M:Inno.Build.Composition.BuildTargetFactory.EndInvoke(System.IAsyncResult) e01098ef278a090a5b2cd8ed407ea7fbfe9c50e2fb0452dd99a7662b27c33933 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b0024b8a5cdcd14c"></a>[`public Inno.Build.Composition.BuildTargetFactory.BuildTargetFactory(object @object, nint method)`](../../build/composition/Inno.Build.Composition/BuildTargetFactory.cs#L9) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-aaf1f87d73f7dfaf"></a>[`public virtual System.IAsyncResult Inno.Build.Composition.BuildTargetFactory.BeginInvoke(System.AsyncCallback callback, object @object)`](../../build/composition/Inno.Build.Composition/BuildTargetFactory.cs#L9) | — | — | — |
| <a id="api-c4daafebeb3c9a9b"></a>[`public virtual Inno.Build.IGameBuildTarget Inno.Build.Composition.BuildTargetFactory.EndInvoke(System.IAsyncResult result)`](../../build/composition/Inno.Build.Composition/BuildTargetFactory.cs#L9) | — | — | — |
| <a id="api-ed951b174f811459"></a>[`public virtual Inno.Build.IGameBuildTarget Inno.Build.Composition.BuildTargetFactory.Invoke()`](../../build/composition/Inno.Build.Composition/BuildTargetFactory.cs#L9) | Creates a platform packager without borrowing authoring or content compiler services.<br>返回：A non-null packaging implementation retained by the pipeline without a separate lifetime. | — | — |
| <a id="api-fc13ccd2c8248070"></a>[`public delegate Inno.Build.IGameBuildTarget Inno.Build.Composition.BuildTargetFactory()`](../../build/composition/Inno.Build.Composition/BuildTargetFactory.cs#L9) | Creates a platform packager without borrowing authoring or content compiler services.<br>返回：A non-null packaging implementation retained by the pipeline without a separate lifetime. | — | — |

### `Inno.Build.Composition.GameBuildContribution`

<!-- api: T:Inno.Build.Composition.GameBuildContribution f92b05f4ecf3d6925991b666ec982b001cf98817586baa893a4af6c8d7f02f06 -->
<!-- api: M:Inno.Build.Composition.GameBuildContribution.#ctor(Inno.Build.Composition.BuildPlatformContribution,Inno.Build.Composition.GameContentCompilerFactory) 48095eb420014e83fa126e2ff8497a515d8773f1dd6052d873cd2b8d959bb54a -->
<!-- api: P:Inno.Build.Composition.GameBuildContribution.platform 9dae79ec6c378ee4df2e23bafb91df7cd700988d17867ee4b5b05347d2b72e55 -->
<!-- api: P:Inno.Build.Composition.GameBuildContribution.compilerFactory 233d9c3b6df4c50bcf5f080f163f110337d523418e0d8b7697aef4a89b91840c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7b1e44821e5b5183"></a>[`public Inno.Build.Composition.GameBuildContribution.GameBuildContribution(Inno.Build.Composition.BuildPlatformContribution platform, Inno.Build.Composition.GameContentCompilerFactory compilerFactory)`](../../build/composition/Inno.Build.Composition/GameBuildContribution.cs#L22) | Captures the complete contribution without instantiating authoring services or tools.<br>`platform`：The platform-owned target, packaging, Support Pack and SDK contribution.<br>`compilerFactory`：The selected content implementation factory, invoked once per pipeline composition. | — | `System.ArgumentNullException`：A contribution or factory is null. |
| <a id="api-1b513b9e6512dcaf"></a>[`public Inno.Build.Composition.GameContentCompilerFactory Inno.Build.Composition.GameBuildContribution.compilerFactory { get; }`](../../build/composition/Inno.Build.Composition/GameBuildContribution.cs#L40) | Gets the distribution-selected factory borrowing each pipeline's authoring generation. | — | — |
| <a id="api-f60b53fc9505370e"></a>[`public Inno.Build.Composition.BuildPlatformContribution Inno.Build.Composition.GameBuildContribution.platform { get; }`](../../build/composition/Inno.Build.Composition/GameBuildContribution.cs#L35) | Gets the platform-owned contribution, independent of backend selection. | — | — |
| <a id="api-79bc0b2210ad5226"></a>[`public sealed class Inno.Build.Composition.GameBuildContribution`](../../build/composition/Inno.Build.Composition/GameBuildContribution.cs#L8) | Registers a platform contribution with the content implementation selected by a distribution. | — | — |

### `Inno.Build.Composition.GameContentCompilerFactory`

<!-- api: T:Inno.Build.Composition.GameContentCompilerFactory 7dcc571a8d52c3e67ed7bed330d93cd76426137118368c952b84d3dc79cf313a -->
<!-- api: M:Inno.Build.Composition.GameContentCompilerFactory.#ctor(System.Object,System.IntPtr) 121e6c134b64240521699b8ef3679ada6737f424aadd3ad9cc8c2cf36830f73e -->
<!-- api: M:Inno.Build.Composition.GameContentCompilerFactory.Invoke(Inno.Assets.Pipeline.AssetPipeline,Inno.Core.Serialization.SerializationRegistry,Inno.Extensibility.Types.TypeCatalog) 9c53151756ad5704e6953ff6aca13216e60f04e020cfda82fee55983e158195a -->
<!-- api: M:Inno.Build.Composition.GameContentCompilerFactory.BeginInvoke(Inno.Assets.Pipeline.AssetPipeline,Inno.Core.Serialization.SerializationRegistry,Inno.Extensibility.Types.TypeCatalog,System.AsyncCallback,System.Object) 8de295d57e948c755d9e8802d450c7d015cdb387c358264e25336d07afee6b97 -->
<!-- api: M:Inno.Build.Composition.GameContentCompilerFactory.EndInvoke(System.IAsyncResult) 5e75c63dfd27a76ed9c052d991ca515ef3bc739ae96836df57d61ce96a0a6885 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-08fd47241d7f6484"></a>[`public Inno.Build.Composition.GameContentCompilerFactory.GameContentCompilerFactory(object @object, nint method)`](../../build/composition/Inno.Build.Composition/GameContentCompilerFactory.cs#L22) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-fdcd84a40fa3fac0"></a>[`public virtual System.IAsyncResult Inno.Build.Composition.GameContentCompilerFactory.BeginInvoke(Inno.Assets.Pipeline.AssetPipeline assets, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Extensibility.Types.TypeCatalog types, System.AsyncCallback callback, object @object)`](../../build/composition/Inno.Build.Composition/GameContentCompilerFactory.cs#L22) | — | — | — |
| <a id="api-1962fb798ade77cd"></a>[`public virtual Inno.Build.IGameContentCompiler Inno.Build.Composition.GameContentCompilerFactory.EndInvoke(System.IAsyncResult result)`](../../build/composition/Inno.Build.Composition/GameContentCompilerFactory.cs#L22) | — | — | — |
| <a id="api-5f93f2e3efb87431"></a>[`public virtual Inno.Build.IGameContentCompiler Inno.Build.Composition.GameContentCompilerFactory.Invoke(Inno.Assets.Pipeline.AssetPipeline assets, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Extensibility.Types.TypeCatalog types)`](../../build/composition/Inno.Build.Composition/GameContentCompilerFactory.cs#L22) | Creates a content compiler borrowing the active authoring services for one pipeline.<br>返回：A non-null scoped compiler that never disposes or globally retains these services.<br>`assets`：The asset owner whose content the build freezes.<br>`serialization`：The matching serialization registry; ownership remains with the host.<br>`types`：The matching extension catalog; the compiler must not outlive its pipeline. | — | — |
| <a id="api-ad995aebfba1a445"></a>[`public delegate Inno.Build.IGameContentCompiler Inno.Build.Composition.GameContentCompilerFactory(Inno.Assets.Pipeline.AssetPipeline assets, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Extensibility.Types.TypeCatalog types)`](../../build/composition/Inno.Build.Composition/GameContentCompilerFactory.cs#L22) | Creates a content compiler borrowing the active authoring services for one pipeline.<br>返回：A non-null scoped compiler that never disposes or globally retains these services.<br>`assets`：The asset owner whose content the build freezes.<br>`serialization`：The matching serialization registry; ownership remains with the host.<br>`types`：The matching extension catalog; the compiler must not outlive its pipeline. | — | — |

### `Inno.Build.Composition.NativeToolchainContribution`

<!-- api: T:Inno.Build.Composition.NativeToolchainContribution 2e1e6ed94c034fc718239f86f4c765cfd83206b3bb1594412cf8c955dfdd6894 -->
<!-- api: M:Inno.Build.Composition.NativeToolchainContribution.#ctor(Inno.Build.PlatformTargetDescriptor,Inno.Build.Toolchains.INativeToolchainProvider,System.Collections.Generic.IReadOnlyList{Inno.Build.Toolchains.ProductNativeBuildPlan}) 9cba6a4fc30598296496d1d71f0409defc15e6e2336684c9ea2796bda40bcbcc -->
<!-- api: P:Inno.Build.Composition.NativeToolchainContribution.descriptor 69d673b7e59887a756282c2898cdabc7a4ae997571aeb5acf4c53a27566fdcd6 -->
<!-- api: P:Inno.Build.Composition.NativeToolchainContribution.provider 4917882d008bb8e062233edecbb93bca9db66ace55b39e9225328f95504bd99e -->
<!-- api: P:Inno.Build.Composition.NativeToolchainContribution.nativeProducts a28414dd6a86a96096d6e0c4b9ad7509c511cf1b4f01d20aa391e596b4f8db62 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e59bced9d832ded1"></a>[`public Inno.Build.Composition.NativeToolchainContribution.NativeToolchainContribution(Inno.Build.PlatformTargetDescriptor descriptor, Inno.Build.Toolchains.INativeToolchainProvider provider, System.Collections.Generic.IReadOnlyList<Inno.Build.Toolchains.ProductNativeBuildPlan>? nativeProducts = null)`](../../build/composition/Inno.Build.Composition/NativeToolchainContribution.cs#L29) | Binds immutable target facts to the provider that owns SDK resolution.<br>`descriptor`：The exact target and ABI supported by this contribution.<br>`provider`：The borrowed provider that resolves executable tools for that target.<br>`nativeProducts`：Optional implemented native-only products, without game publication registration. | — | `System.ArgumentNullException`：A required contribution member is null. |
| <a id="api-7b7678030fc8c857"></a>[`public Inno.Build.PlatformTargetDescriptor Inno.Build.Composition.NativeToolchainContribution.descriptor { get; }`](../../build/composition/Inno.Build.Composition/NativeToolchainContribution.cs#L48) | Gets explicit target facts independently of the current execution machine. | — | — |
| <a id="api-c068fe51842ec0ab"></a>[`public System.Collections.Generic.IReadOnlyDictionary<string, Inno.Build.Toolchains.ProductNativeBuildPlan> Inno.Build.Composition.NativeToolchainContribution.nativeProducts { get; }`](../../build/composition/Inno.Build.Composition/NativeToolchainContribution.cs#L57) | Gets explicitly implemented native-only product closures keyed by stable product identity. | — | — |
| <a id="api-4178bcc627547c25"></a>[`public Inno.Build.Toolchains.INativeToolchainProvider Inno.Build.Composition.NativeToolchainContribution.provider { get; }`](../../build/composition/Inno.Build.Composition/NativeToolchainContribution.cs#L53) | Gets the registered SDK boundary; registration does not start tools or inspect the environment. | — | — |
| <a id="api-bed4b4c380091c3e"></a>[`public sealed class Inno.Build.Composition.NativeToolchainContribution`](../../build/composition/Inno.Build.Composition/NativeToolchainContribution.cs#L12) | Registers an implemented native SDK capability without declaring a complete game product. | — | — |

## 项目依赖

- [Inno.Build.Toolchains](Inno.Build.Toolchains.md)：公开引用边界由实际签名核对。
- [Inno.Build](Inno.Build.md)：公开引用边界由实际签名核对。
- [Inno.Build.Managed](Inno.Build.Managed.md)：公开引用边界由实际签名核对。
- [Inno.Build.SupportPacks.Core](Inno.Build.SupportPacks.Core.md)：公开引用边界由实际签名核对。
- [Inno.Assets.Pipeline](../assets/Inno.Assets.Pipeline.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：公开引用边界由实际签名核对。
- [Inno.Runtime](../runtime/Inno.Runtime.md)：公开引用边界由实际签名核对。
- [Inno.Plugins.Authoring](../plugins/Inno.Plugins.Authoring.md)：公开引用边界由实际签名核对。
- [Inno.Scripting.Compiler](../scripting/Inno.Scripting.Compiler.md)：公开引用边界由实际签名核对。
- [Inno.Core.Settings](../core/Inno.Core.Settings.md)：公开引用边界由实际签名核对。
