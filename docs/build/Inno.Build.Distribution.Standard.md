# Inno.Build.Distribution.Standard

[分类索引](README.md) · [Wiki 首页](../README.md) · [平台归属与扩展](../architecture/PLATFORM_EXTENSION_GUIDE.md)

## 职责与边界

唯一标准发行注册入口，将已实现的平台贡献、共享 DotNet 托管部署和产品 Native 闭包组合起来。它是具体组合，不是中立领域。

## 组合、生命周期与扩展

`StandardBuildDistribution.Create(context, tools)` 注册 WindowsX64、MacOSArm64、BrowserWasm32 游戏目标。CoreCLR、NativeAOT、Mono Wasm 解释执行/AOT 的实现注册一次。Editor 产品入口、CLI 和 MSBuild Task 调用同一入口。`tools` 为 null 时使用应用显式部署索引；项目 CLI 传入同一受保护工具产品的 runner，发行将其借给内容 compiler 工厂。

`StandardBuildDistribution` 实例的 `build` 提供中立发行契约；`ResolveShaderTarget(target)` 从同一个
平台绑定列表取得具体 Shader 配置。共享 Build Composition 不引用 BGFX。平台与 Shader ID 不一致在组合阶段失败。

`StandardNativeBuildPlans.CreatePlayer(profile)` / `CreateEditor(profile)` / `CreateShaderTools(profile)` 声明 Player、Editor 和 Shader tools 的准确组件闭包。Player 不包括 ImGui、ImGuizmo 或离线工具；Editor 在 Player 能力之上增加呈现和创作工具。Shader tools 仅选择离线编译器。闭包是 immutable plan，执行前先冻结目标绑定与 SDK。
`CreateStaticPlayer(nativeOptions)` 提供 Browser 聚合闭包，每个 step 有真实 `staticBuild`，`build` 为 null；
独立执行器拒绝这种闭包，Browser aggregate 才负责构建。

```csharp
using Inno.Build.Composition;
using Inno.Build.Distribution.Standard;

static BuildDistribution CreateStandard(BuildCompositionContext context)
{
    return StandardBuildDistribution.Create(context).build;
}
```

`StandardBuildEnvironment.Capture(applicationDirectory, toolsTarget, dotnetHost)` 记录实际执行宿主与选定 SDK；调用者必须明确提供 tools target。`dotnetHost` 可明确选择拥有所需 workload 的 SDK，省略时使用当前工具进程选定的 host。导出目标仍独立声明。新增平台注册位置只有这里，领域服务和共享 Editor 不修改。注册不等于 SDK 已存在或设备验收已通过。

## 缓存与部署组合

`StandardBuildCaches.CreateOwners(engineRoot)` 明确登记标准 Native、绑定和 Support Pack 发布边界。其清单来自发布者的 ownership record；当前根、原生产物引用的绑定 generation 和活动读取均受保护。清理不扫描任意用户目录。

动态产品的 Native 计划通过产品所属 metadata factory 生成 `Deployment.index`，声明逻辑库名到实际部署文件的映射。布局与库名别名策略留在标准组合；中立工具链只安装明确的 metadata，Interop loader 不按命名平台推导路径。

## 绑定生成实现

StandardBuildEnvironment 创建 NativeBindingGenerator，StandardBuildDistribution 通过中立 composition 传递。内置产品 Native 计划仍在此唯一声明；平台模块、Task 和 CLI 不维护另一份 backend/生成列表。

## 宿主、工具目标与发布目标

这三个选择有不同用途：

| 选择 | 用途 |
| --- | --- |
| `BuildCompositionContext.host` | 判断构建工具能否在当前机器执行；不会自动改变游戏目标 |
| `toolsTarget` | 选择需要在创作机器运行的 Native 工具，例如离线 Shader 编译器 |
| 游戏的 `BuildTargetId` | 选择内容格式、托管部署能力、Native 闭包与最终包装布局 |

Windows Editor 导出 Browser 游戏时，离线工具仍需能在 Windows 执行，生成内容和最终链接则遵循 Browser 目标。改变导出目标不能改变正在运行的 Editor 呈现配置。

`Capture` 解析托管 host 的绝对路径，并创建绑定生成服务；`Create` 只组装注册。真正的 SDK 发现和缺工具诊断发生在所选操作的预检阶段。因此可以查看标准发行的目标集合，但不能据此宣称跨宿主编译或实机运行已经受支持。

此页描述标准发行的选型。自定义发行直接使用 [Build Composition](Inno.Build.Composition.md) 的公开贡献契约，不必修改共同领域服务，也不必引用本项目。标准发行的 provider 为借用配置；每次构建返回的 Native、binding 和 Pack leases 由本次消费者持有并释放，不能存进静态发行对象。

## 当前源码公开 API 清单

签名来自当前 SDK 求值与语义符号；按类型列出 public/protected 契约。工作流与生命周期见本页前文。

### `Inno.Build.Distribution.Standard.StandardBuildCaches`

<!-- api: T:Inno.Build.Distribution.Standard.StandardBuildCaches 1c1e788f0e8f2cc952295abf34db06accb94ec27cfae698207cdea979dbbbc02 -->
<!-- api: M:Inno.Build.Distribution.Standard.StandardBuildCaches.CreateOwners(System.String,System.Collections.Generic.IReadOnlyList{Inno.Build.Toolchains.NativeComponentDescriptor}) 25ef827f97b70d2a13cec2c8e2d2dafd0e76faa6a876191173548f040605ac31 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b6b67ab9377b97be"></a>[`public static System.Collections.Generic.IReadOnlyList<Inno.Build.Toolchains.BuildCacheOwnerDescriptor> Inno.Build.Distribution.Standard.StandardBuildCaches.CreateOwners(string engineRoot, System.Collections.Generic.IReadOnlyList<Inno.Build.Toolchains.NativeComponentDescriptor> components)`](../../build/distributions/Inno.Build.Distribution.Standard/StandardBuildCaches.cs#L30) | Registers exact Native, binding and Support Pack ownership recorded by their publishers. Unrecorded directories remain outside the cleanup authority.<br>返回：Immutable owner registrations with current-generation and transitive dependency protection.<br>`engineRoot`：The absolute engine checkout containing the artifact roots.<br>`components`：The actual registered component owners; shared toolchain roots are registered once. | 约束由所属类型与本页生命周期定义。 | `System.ArgumentException`：The root is blank or relative. |
| <a id="api-ea6d9e002cae9c43"></a>[`public static class Inno.Build.Distribution.Standard.StandardBuildCaches`](../../build/distributions/Inno.Build.Distribution.Standard/StandardBuildCaches.cs#L12) | Registers the standard distribution's actual immutable artifact publishers for coordinated cleanup. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Build.Distribution.Standard.StandardBuildDistribution`

<!-- api: T:Inno.Build.Distribution.Standard.StandardBuildDistribution 30950658a0f6e43303b5fa9aaece0172f155d97f5afa911a82f07da7b2d28a45 -->
<!-- api: P:Inno.Build.Distribution.Standard.StandardBuildDistribution.build ef7c994c8a067a0f2d5e137f23cda77f86bd61400062ca75382c414934f6031c -->
<!-- api: M:Inno.Build.Distribution.Standard.StandardBuildDistribution.Create(Inno.Build.Composition.BuildCompositionContext,Inno.Build.Toolchains.Bgfx.Tools.ToolRunner) de68ca30c9323e7a37c0dc06a21517f49fd1bbc2a5a8b6d45e959b9ee9c25300 -->
<!-- api: M:Inno.Build.Distribution.Standard.StandardBuildDistribution.ResolveShaderTarget(Inno.Build.BuildTargetId) a705cc7b662b6cba4b163c0b2fe95802b0bc0202a76fa905edbdf589b31f9ab0 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-bf845cdc31dae974"></a>[`public static Inno.Build.Distribution.Standard.StandardBuildDistribution Inno.Build.Distribution.Standard.StandardBuildDistribution.Create(Inno.Build.Composition.BuildCompositionContext context, Inno.Build.Toolchains.Bgfx.Tools.ToolRunner? tools = null)`](../../build/distributions/Inno.Build.Distribution.Standard/StandardBuildDistribution.cs#L51) | Composes one explicit binding per implemented platform without resolving tools.<br>返回：A frozen standard distribution with backend configuration bound to the same platform identities.<br>`context`：The explicitly selected SDK and execution host.<br>`tools`：Borrowed frozen content-compilation tools, or null to consume this application's explicit deployment. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-eaf0b5b001274dff"></a>[`public Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile Inno.Build.Distribution.Standard.StandardBuildDistribution.ResolveShaderTarget(Inno.Build.BuildTargetId target)`](../../build/distributions/Inno.Build.Distribution.Standard/StandardBuildDistribution.cs#L105) | Resolves explicitly registered shader configuration independently of tool execution target.<br>返回：The borrowed immutable configuration; unavailable targets fail before native preparation.<br>`target`：The artifact's actual publication target. | 约束由所属类型与本页生命周期定义。 | `System.NotSupportedException`：The distribution has no shader configuration for this target. |
| <a id="api-2ee139cff662ba02"></a>[`public Inno.Build.Composition.BuildDistribution Inno.Build.Distribution.Standard.StandardBuildDistribution.build { get; }`](../../build/distributions/Inno.Build.Distribution.Standard/StandardBuildDistribution.cs#L37) | Gets the neutral distribution borrowed by product hosts and shared pipeline mechanisms. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-a86fde7406b2e1bb"></a>[`public sealed class Inno.Build.Distribution.Standard.StandardBuildDistribution`](../../build/distributions/Inno.Build.Distribution.Standard/StandardBuildDistribution.cs#L22) | Binds platform publication and backend configuration once at the standard distribution boundary. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Build.Distribution.Standard.StandardBuildEnvironment`

<!-- api: T:Inno.Build.Distribution.Standard.StandardBuildEnvironment 394337b4425899a30bb2d41c27a9dd2aeb0d3d5acf91e15945d186b2ec9f442e -->
<!-- api: M:Inno.Build.Distribution.Standard.StandardBuildEnvironment.Capture(System.String,Inno.Build.BuildTargetId,System.String) ffdd261f9c1dda713fd996b771830e607b5788e277217e39e3a41d22bd1b4dba -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-25327c3983a2684a"></a>[`public static Inno.Build.Composition.BuildCompositionContext Inno.Build.Distribution.Standard.StandardBuildEnvironment.Capture(string applicationDirectory, Inno.Build.BuildTargetId toolsTarget, string? dotnetHost = null)`](../../build/distributions/Inno.Build.Distribution.Standard/StandardBuildEnvironment.cs#L36) | Resolves the selected managed host and captures execution capabilities at the composition boundary.<br>返回：A frozen execution context; publication targets must be selected independently.<br>`applicationDirectory`：The product or tool's installed location used for source provisioning.<br>`toolsTarget`：The native target explicitly selected for executable build tools, independently of exported games.<br>`dotnetHost`：An explicit managed SDK executable, or null to use the current tool process's selected host. | 约束由所属类型与本页生命周期定义。 | `System.PlatformNotSupportedException`：The distribution has no build execution integration for this system or processor.<br>`System.IO.FileNotFoundException`：The selected managed SDK host cannot be located. |
| <a id="api-34ce61062e19e5cc"></a>[`public static class Inno.Build.Distribution.Standard.StandardBuildEnvironment`](../../build/distributions/Inno.Build.Distribution.Standard/StandardBuildEnvironment.cs#L13) | Captures the actual machine running distribution tools without choosing an output target. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Build.Distribution.Standard.StandardNativeBuildPlans`

<!-- api: T:Inno.Build.Distribution.Standard.StandardNativeBuildPlans 1f698124b3873c1076e09dffafb051ecce68b4a3c83472bdd193126073768ab7 -->
<!-- api: M:Inno.Build.Distribution.Standard.StandardNativeBuildPlans.CreatePlayer(Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile) 264f13f35563ace5799e1373e277b742fd2a3197644c8648c6d3ff3f200d67fb -->
<!-- api: M:Inno.Build.Distribution.Standard.StandardNativeBuildPlans.CreateShaderTools(Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile) 890eed3f16afb1011793a9813912a36d414931599adbc0908f051ca989a428a8 -->
<!-- api: M:Inno.Build.Distribution.Standard.StandardNativeBuildPlans.CreateEditor(Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile) d74762fa760a1f2950eb500b797d41bb2530d481039fc84f5c7bb7a1ee0c3f20 -->
<!-- api: M:Inno.Build.Distribution.Standard.StandardNativeBuildPlans.CreateStaticPlayer(Inno.Build.Toolchains.NativeComponentBuildOptions) 7217047d08ee5e215ba6891878d4251220dfb0a95884f61646601e08d363424a -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b5897980618c5052"></a>[`public static Inno.Build.Toolchains.ProductNativeBuildPlan Inno.Build.Distribution.Standard.StandardNativeBuildPlans.CreateEditor(Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile profile)`](../../build/distributions/Inno.Build.Distribution.Standard/StandardNativeBuildPlans.cs#L51) | Composes the shared runtime closure and the Editor's presentation and authoring tools.<br>返回：A fresh immutable Editor plan with the ImGuizmo dependency declared explicitly.<br>`profile`：The selected integration-owned BGFX SDK invocation for standalone component builds. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-47dae7cb628b2853"></a>[`public static Inno.Build.Toolchains.ProductNativeBuildPlan Inno.Build.Distribution.Standard.StandardNativeBuildPlans.CreatePlayer(Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile profile)`](../../build/distributions/Inno.Build.Distribution.Standard/StandardNativeBuildPlans.cs#L29) | Composes the standard Player's reusable native components with explicit deployment policies.<br>返回：A fresh immutable Player plan without resolving tools or touching source files.<br>`profile`：The selected integration-owned BGFX SDK invocation for standalone component builds. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-7fe23ed93edc6bc2"></a>[`public static Inno.Build.Toolchains.ProductNativeBuildPlan Inno.Build.Distribution.Standard.StandardNativeBuildPlans.CreateShaderTools(Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile profile)`](../../build/distributions/Inno.Build.Distribution.Standard/StandardNativeBuildPlans.cs#L40) | Selects only the offline graphics compiler component needed for shader publication.<br>返回：An immutable tool product without runtime or Editor presentation dependencies.<br>`profile`：The selected integration-owned BGFX SDK invocation for standalone component builds. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-6f2c88d15309aa11"></a>[`public static Inno.Build.Toolchains.ProductNativeBuildPlan Inno.Build.Distribution.Standard.StandardNativeBuildPlans.CreateStaticPlayer(Inno.Build.Toolchains.NativeComponentBuildOptions graphicsOptions)`](../../build/distributions/Inno.Build.Distribution.Standard/StandardNativeBuildPlans.cs#L95) | Declares the real static component closure consumed by an explicit aggregate executor.<br>返回：A Player closure with static recipes and no unused standalone producers.<br>`graphicsOptions`：The selected platform and graphics integration's explicit static component configuration. | 约束由所属类型与本页生命周期定义。 | `System.ArgumentNullException`：Graphics configuration is null.<br>`System.ArgumentException`：Graphics configuration requests shared rather than static linkage. |
| <a id="api-9b544a3d7e56a71a"></a>[`public static class Inno.Build.Distribution.Standard.StandardNativeBuildPlans`](../../build/distributions/Inno.Build.Distribution.Standard/StandardNativeBuildPlans.cs#L18) | Declares the standard product closures while keeping backend selection out of shared build mechanisms. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

## 项目依赖

- [Inno.Build.Bindings](Inno.Build.Bindings.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Integration.WindowsX64.Bgfx](../platform/WindowsX64/Inno.Integration.WindowsX64.Bgfx.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Integration.MacOSArm64.Bgfx](../platform/MacOSArm64/Inno.Integration.MacOSArm64.Bgfx.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Integration.BrowserWasm32.Bgfx](../platform/BrowserWasm32/Inno.Integration.BrowserWasm32.Bgfx.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Toolchains.Bgfx.Tools](../backends/Bgfx/Inno.Build.Toolchains.Bgfx.Tools.md)：公开引用边界由实际签名核对。
- [Inno.Build.Composition](Inno.Build.Composition.md)：公开引用边界由实际签名核对。
- [Inno.Build.Managed.DotNet](../backends/DotNet/Inno.Build.Managed.DotNet.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.WindowsX64](../platform/WindowsX64/Inno.Build.WindowsX64.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.MacOSArm64](../platform/MacOSArm64/Inno.Build.MacOSArm64.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.BrowserWasm32](../platform/BrowserWasm32/Inno.Build.BrowserWasm32.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Toolchains.Bgfx](../backends/Bgfx/Inno.Build.Toolchains.Bgfx.md)：公开引用边界由实际签名核对。
- [Inno.Build.Toolchains.ImGui](../backends/ImGui/Inno.Build.Toolchains.ImGui.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Toolchains.ImGuizmo](../backends/ImGui/Inno.Build.Toolchains.ImGuizmo.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Toolchains.MiniAudio](../backends/MiniAudio/Inno.Build.Toolchains.MiniAudio.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Toolchains.Sdl3](../backends/Sdl3/Inno.Build.Toolchains.Sdl3.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Toolchains.Text](../backends/Text/Inno.Build.Toolchains.Text.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Toolchains.UI](../backends/RmlUi/Inno.Build.Toolchains.UI.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Toolchains](Inno.Build.Toolchains.md)：公开引用边界由实际签名核对。
