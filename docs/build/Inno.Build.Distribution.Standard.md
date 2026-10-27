# Inno.Build.Distribution.Standard

[分类索引](README.md) · [Wiki 首页](../README.md) · [平台归属与扩展](../architecture/PLATFORM_EXTENSION_GUIDE.md)

## 职责与边界

唯一标准发行注册入口，将已实现的平台贡献、共享 DotNet 托管部署和产品 Native 闭包组合起来。它是具体组合，不是中立领域。

## 组合、生命周期与扩展

`StandardBuildDistribution.Create(context, tools)` 注册 WindowsX64、MacOSArm64、BrowserWasm32 游戏目标。CoreCLR、NativeAOT、Mono Wasm 解释执行/AOT 的实现注册一次。Editor 产品入口、CLI 和 MSBuild Task 调用同一入口。`tools` 为 null 时使用应用显式部署索引；项目 CLI 传入同一受保护工具产品的 runner，发行将其借给内容 compiler 工厂。

`StandardBuildDistribution` 实例的 `build` 提供中立发行契约；`ResolveShaderTarget(target)` 从同一个
平台绑定列表取得具体 Shader 配置。共享 Build Composition 不引用 BGFX。平台与 Shader ID 不一致在组合阶段失败。

`StandardNativeComponents.CreatePlayer(profile)` / `CreateEditor(profile)` / `CreateShaderTools(profile)` 声明 Player、Editor 和 Shader tools 的准确组件闭包。Player 不包括 ImGui、ImGuizmo 或离线工具；Editor 在 Player 能力之上增加呈现和创作工具。Shader tools 仅选择离线编译器。闭包是 immutable plan，执行前先冻结目标绑定与 SDK。
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

此清单对应当前 SDK 求值与源码符号；内部类型不构成公开 API。

<!-- api: T:Inno.Build.Distribution.Standard.StandardBuildCaches 1c1e788f0e8f2cc952295abf34db06accb94ec27cfae698207cdea979dbbbc02 -->
<!-- api: M:Inno.Build.Distribution.Standard.StandardBuildCaches.CreateOwners(System.String,System.Collections.Generic.IReadOnlyList{Inno.Build.Toolchains.NativeComponentDescriptor}) 25ef827f97b70d2a13cec2c8e2d2dafd0e76faa6a876191173548f040605ac31 -->
<!-- api: T:Inno.Build.Distribution.Standard.StandardBuildDistribution 30950658a0f6e43303b5fa9aaece0172f155d97f5afa911a82f07da7b2d28a45 -->
<!-- api: P:Inno.Build.Distribution.Standard.StandardBuildDistribution.build ef7c994c8a067a0f2d5e137f23cda77f86bd61400062ca75382c414934f6031c -->
<!-- api: M:Inno.Build.Distribution.Standard.StandardBuildDistribution.Create(Inno.Build.Composition.BuildCompositionContext,Inno.Build.Toolchains.Bgfx.Tools.ToolRunner) de68ca30c9323e7a37c0dc06a21517f49fd1bbc2a5a8b6d45e959b9ee9c25300 -->
<!-- api: M:Inno.Build.Distribution.Standard.StandardBuildDistribution.ResolveShaderTarget(Inno.Build.BuildTargetId) a705cc7b662b6cba4b163c0b2fe95802b0bc0202a76fa905edbdf589b31f9ab0 -->
<!-- api: T:Inno.Build.Distribution.Standard.StandardBuildEnvironment 394337b4425899a30bb2d41c27a9dd2aeb0d3d5acf91e15945d186b2ec9f442e -->
<!-- api: M:Inno.Build.Distribution.Standard.StandardBuildEnvironment.Capture(System.String,Inno.Build.BuildTargetId,System.String) ffdd261f9c1dda713fd996b771830e607b5788e277217e39e3a41d22bd1b4dba -->
<!-- api: T:Inno.Build.Distribution.Standard.StandardBuildHosts 997bb8b0cb07d9b567b60c03ef2c6a7861072a44568ebd0ee760f85e1efe1ec7 -->
<!-- api: M:Inno.Build.Distribution.Standard.StandardBuildHosts.Capture 7472805f697cc9502717f200fd052d128e385e00091ce6ebfbdd7f6505b76df9 -->

| API 与源码 | 契约与参数 | 返回值 | 重要异常 |
| --- | --- | --- | --- |
| <a id="api-ea6d9e002cae9c43"></a>[`public static class Inno.Build.Distribution.Standard.StandardBuildCaches`](../../build/distributions/Inno.Build.Distribution.Standard/StandardBuildCaches.cs#L12) | Registers the standard distribution's actual immutable artifact publishers for coordinated cleanup. | — | — |
| <a id="api-b6b67ab9377b97be"></a>[`public static System.Collections.Generic.IReadOnlyList<Inno.Build.Toolchains.BuildCacheOwnerDescriptor> Inno.Build.Distribution.Standard.StandardBuildCaches.CreateOwners(string engineRoot, System.Collections.Generic.IReadOnlyList<Inno.Build.Toolchains.NativeComponentDescriptor> components)`](../../build/distributions/Inno.Build.Distribution.Standard/StandardBuildCaches.cs#L30) | Registers exact Native, binding and Support Pack ownership recorded by their publishers. Unrecorded directories remain outside the cleanup authority.<br>`engineRoot`: The absolute engine checkout containing the artifact roots.<br>`components`: The actual registered component owners; shared toolchain roots are registered once. | Immutable owner registrations with current-generation and transitive dependency protection. | `System.ArgumentException`: The root is blank or relative. |
| <a id="api-a86fde7406b2e1bb"></a>[`public sealed class Inno.Build.Distribution.Standard.StandardBuildDistribution`](../../build/distributions/Inno.Build.Distribution.Standard/StandardBuildDistribution.cs#L23) | Binds platform publication and backend configuration once at the standard distribution boundary. | — | — |
| <a id="api-2ee139cff662ba02"></a>[`public Inno.Build.Composition.BuildDistribution Inno.Build.Distribution.Standard.StandardBuildDistribution.build { get; }`](../../build/distributions/Inno.Build.Distribution.Standard/StandardBuildDistribution.cs#L38) | Gets the neutral distribution borrowed by product hosts and shared pipeline mechanisms. | — | — |
| <a id="api-bf845cdc31dae974"></a>[`public static Inno.Build.Distribution.Standard.StandardBuildDistribution Inno.Build.Distribution.Standard.StandardBuildDistribution.Create(Inno.Build.Composition.BuildCompositionContext context, Inno.Build.Toolchains.Bgfx.Tools.ToolRunner? tools = null)`](../../build/distributions/Inno.Build.Distribution.Standard/StandardBuildDistribution.cs#L52) | Composes one explicit binding per implemented platform without resolving tools.<br>`context`: The explicitly selected SDK and execution host.<br>`tools`: Borrowed frozen content-compilation tools, or null to consume this application's explicit deployment. | A frozen standard distribution with backend configuration bound to the same platform identities. | — |
| <a id="api-eaf0b5b001274dff"></a>[`public Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile Inno.Build.Distribution.Standard.StandardBuildDistribution.ResolveShaderTarget(Inno.Build.BuildTargetId target)`](../../build/distributions/Inno.Build.Distribution.Standard/StandardBuildDistribution.cs#L91) | Resolves explicitly registered shader configuration independently of tool execution target.<br>`target`: The artifact's actual publication target. | The borrowed immutable configuration; unavailable targets fail before native preparation. | `System.NotSupportedException`: The distribution has no shader configuration for this target. |
| <a id="api-34ce61062e19e5cc"></a>[`public static class Inno.Build.Distribution.Standard.StandardBuildEnvironment`](../../build/distributions/Inno.Build.Distribution.Standard/StandardBuildEnvironment.cs#L12) | Captures the actual machine running distribution tools without choosing an output target. | — | — |
| <a id="api-25327c3983a2684a"></a>[`public static Inno.Build.Composition.BuildCompositionContext Inno.Build.Distribution.Standard.StandardBuildEnvironment.Capture(string applicationDirectory, Inno.Build.BuildTargetId toolsTarget, string? dotnetHost = null)`](../../build/distributions/Inno.Build.Distribution.Standard/StandardBuildEnvironment.cs#L36) | Resolves the selected managed host and captures execution capabilities at the composition boundary.<br>`applicationDirectory`: The product or tool's installed location used for source provisioning.<br>`toolsTarget`: The native target explicitly selected for executable build tools, independently of exported games.<br>`dotnetHost`: An explicit SDK executable, or null to use DOTNET_HOST_PATH, the loaded shared runtime's muxer, then normal executable discovery when this process has no shared runtime installation. | A frozen execution context; publication targets must be selected independently. | `System.PlatformNotSupportedException`: The distribution has no build execution integration for this system or processor.<br>`System.IO.FileNotFoundException`: The selected managed SDK host cannot be located. |
| <a id="api-0a5c106f8b70c978"></a>[`public static class Inno.Build.Distribution.Standard.StandardBuildHosts`](../../build/distributions/Inno.Build.Distribution.Standard/StandardBuildHosts.cs#L11) | Registers supported build hosts without duplicating their execution facts. | — | — |
| <a id="api-3ce661633c816e3d"></a>[`public static Inno.Build.Toolchains.BuildHostDescriptor Inno.Build.Distribution.Standard.StandardBuildHosts.Capture()`](../../build/distributions/Inno.Build.Distribution.Standard/StandardBuildHosts.cs#L22) | Captures the current execution machine from registered platform providers. | The selected immutable host and execution capabilities. | `System.PlatformNotSupportedException`: No registered provider supports the current machine. |

## 项目依赖

| 工程 | 边界 |
| --- | --- |
| `Inno.Native.LibraryLoading` | 实现依赖；不传递 compile surface |
| `Inno.Build.Windows.Host` | 实现依赖；不传递 compile surface |
| `Inno.Build.Bindings` | 实现依赖；不传递 compile surface |
| `Inno.Integration.WindowsX64.Bgfx` | 实现依赖；不传递 compile surface |
| `Inno.Integration.MacOSArm64.Bgfx` | 实现依赖；不传递 compile surface |
| `Inno.Integration.BrowserWasm32.Bgfx` | 实现依赖；不传递 compile surface |
| `Inno.Build.Toolchains.Bgfx.Tools` | 公开签名依赖 |
| `Inno.Build.Composition` | 公开签名依赖 |
| `Inno.Build.Managed.DotNet` | 实现依赖；不传递 compile surface |
| `Inno.Integration.WindowsX64.Build` | 实现依赖；不传递 compile surface |
| `Inno.Integration.MacOSArm64.Build` | 实现依赖；不传递 compile surface |
| `Inno.Integration.BrowserWasm32.Build` | 实现依赖；不传递 compile surface |
| `Inno.Build.Toolchains.Bgfx` | 公开签名依赖 |
| `Inno.Build.Toolchains` | 公开签名依赖 |
