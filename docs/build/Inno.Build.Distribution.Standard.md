# Inno.Build.Distribution.Standard

[分类索引](README.md) · [Wiki 首页](../README.md) · [平台归属与扩展](../architecture/PLATFORM_EXTENSION_GUIDE.md)

## 职责与边界

唯一标准发行注册入口，将已实现的平台贡献、共享 DotNet 托管部署和产品 Native 闭包组合起来。它是具体组合，不是中立领域。

## 组合、生命周期与扩展

`StandardBuildDistribution.Create(context)` 注册 WindowsX64、MacOSArm64、BrowserWasm32 游戏目标，以及仅提供 Native 工具链的 Linux 能力。CoreCLR、NativeAOT、Mono Wasm 解释执行/AOT 的实现注册一次。Editor 产品入口、CLI 和 MSBuild Task 调用同一入口。

`StandardBuildDistribution` 实例的 `build` 提供中立发行契约；`ResolveShaderTarget(target)` 从同一个
平台绑定列表取得具体 Shader 配置。共享 Build Composition 不引用 BGFX。平台与 Shader ID 不一致在组合阶段失败。

`StandardNativeBuildPlans.CreatePlayer(profile)` / `CreateEditor(profile)` / `CreateShaderTools(profile)` 声明 Player、Editor 和 Shader tools 的准确组件闭包。Player 不包括 ImGui、ImGuizmo 或离线工具；Editor 在 Player 能力之上增加呈现和创作工具。Shader tools 仅选择离线编译器。闭包是 immutable plan，执行前先冻结目标绑定与 SDK。
`CreateStaticPlayer(nativeOptions)` 提供 Browser 聚合闭包，每个 step 有真实 `staticBuild`，`build` 为 null；
独立执行器拒绝这种闭包，Browser aggregate 才负责构建。Linux 仅注册已有 Native SDK 与 Shader tools，不宣称完整 Player。

```csharp
using Inno.Build.Composition;
using Inno.Build.Distribution.Standard;

static BuildDistribution CreateStandard(BuildCompositionContext context)
{
    return StandardBuildDistribution.Create(context).build;
}
```

`StandardBuildEnvironment.Capture` 只记录实际执行宿主；调用者必须明确提供 tools target。新增平台注册位置只有这里，领域服务和共享 Editor 不修改。注册不等于 SDK 已存在或设备验收已通过。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Build.Distribution.Standard.StandardBuildDistribution`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Composition.BuildDistribution Inno.Build.Distribution.Standard.StandardBuildDistribution.build`](../../build/distributions/Inno.Build.Distribution.Standard/StandardBuildDistribution.cs#L37) | Gets the neutral distribution borrowed by product hosts and shared pipeline mechanisms. |
| [`Inno.Build.Distribution.Standard.StandardBuildDistribution`](../../build/distributions/Inno.Build.Distribution.Standard/StandardBuildDistribution.cs#L22) | Binds platform publication and backend configuration once at the standard distribution boundary. |
| [`Inno.Build.Toolchains.Bgfx.Tools.BgfxShaderTargetProfile Inno.Build.Distribution.Standard.StandardBuildDistribution.ResolveShaderTarget(Inno.Build.BuildTargetId target)`](../../build/distributions/Inno.Build.Distribution.Standard/StandardBuildDistribution.cs#L90) | Resolves explicitly registered shader configuration independently of tool execution target. |
| [`static Inno.Build.Distribution.Standard.StandardBuildDistribution Inno.Build.Distribution.Standard.StandardBuildDistribution.Create(Inno.Build.Composition.BuildCompositionContext context)`](../../build/distributions/Inno.Build.Distribution.Standard/StandardBuildDistribution.cs#L48) | Composes one explicit binding per implemented platform without resolving tools. |

### `Inno.Build.Distribution.Standard.StandardBuildEnvironment`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Distribution.Standard.StandardBuildEnvironment`](../../build/distributions/Inno.Build.Distribution.Standard/StandardBuildEnvironment.cs#L12) | Captures the actual machine running distribution tools without choosing an output target. |
| [`static Inno.Build.Composition.BuildCompositionContext Inno.Build.Distribution.Standard.StandardBuildEnvironment.Capture(string applicationDirectory, Inno.Build.BuildTargetId toolsTarget)`](../../build/distributions/Inno.Build.Distribution.Standard/StandardBuildEnvironment.cs#L32) | Resolves the selected managed host and captures execution capabilities at the composition boundary. |

### `Inno.Build.Distribution.Standard.StandardNativeBuildPlans`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Distribution.Standard.StandardNativeBuildPlans`](../../build/distributions/Inno.Build.Distribution.Standard/StandardNativeBuildPlans.cs#L18) | Declares the standard product closures while keeping backend selection out of shared build mechanisms. |
| [`static Inno.Build.Toolchains.ProductNativeBuildPlan Inno.Build.Distribution.Standard.StandardNativeBuildPlans.CreateEditor(Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile profile)`](../../build/distributions/Inno.Build.Distribution.Standard/StandardNativeBuildPlans.cs#L51) | Composes the shared runtime closure and the Editor's presentation and authoring tools. |
| [`static Inno.Build.Toolchains.ProductNativeBuildPlan Inno.Build.Distribution.Standard.StandardNativeBuildPlans.CreatePlayer(Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile profile)`](../../build/distributions/Inno.Build.Distribution.Standard/StandardNativeBuildPlans.cs#L29) | Composes the standard Player's reusable native components with explicit deployment policies. |
| [`static Inno.Build.Toolchains.ProductNativeBuildPlan Inno.Build.Distribution.Standard.StandardNativeBuildPlans.CreateShaderTools(Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile profile)`](../../build/distributions/Inno.Build.Distribution.Standard/StandardNativeBuildPlans.cs#L40) | Selects only the offline graphics compiler component needed for shader publication. |
| [`static Inno.Build.Toolchains.ProductNativeBuildPlan Inno.Build.Distribution.Standard.StandardNativeBuildPlans.CreateStaticPlayer(Inno.Build.Toolchains.NativeComponentBuildOptions graphicsOptions)`](../../build/distributions/Inno.Build.Distribution.Standard/StandardNativeBuildPlans.cs#L87) | Declares the real static component closure consumed by an explicit aggregate executor. |

## 项目依赖

- [Inno.Integration.Windows.Bgfx](../platform/Windows/Inno.Integration.Windows.Bgfx.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Integration.MacOS.Bgfx](../platform/MacOS/Inno.Integration.MacOS.Bgfx.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Integration.Browser.Bgfx](../platform/Browser/Inno.Integration.Browser.Bgfx.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Integration.Linux.Bgfx](../platform/Linux/Inno.Integration.Linux.Bgfx.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Toolchains.Bgfx.Tools](../backends/Bgfx/Inno.Build.Toolchains.Bgfx.Tools.md)：公开引用边界由实际签名核对。
- [Inno.Build.Composition](Inno.Build.Composition.md)：公开引用边界由实际签名核对。
- [Inno.Build.Managed.DotNet](../backends/DotNet/Inno.Build.Managed.DotNet.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Windows](../platform/Windows/Inno.Build.Windows.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.MacOS](../platform/MacOS/Inno.Build.MacOS.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Browser](../platform/Browser/Inno.Build.Browser.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Linux](../platform/Linux/Inno.Build.Linux.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Toolchains.Bgfx](../backends/Bgfx/Inno.Build.Toolchains.Bgfx.md)：公开引用边界由实际签名核对。
- [Inno.Build.Toolchains.ImGui](../backends/ImGui/Inno.Build.Toolchains.ImGui.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Toolchains.ImGuizmo](../backends/ImGui/Inno.Build.Toolchains.ImGuizmo.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Toolchains.MiniAudio](../backends/MiniAudio/Inno.Build.Toolchains.MiniAudio.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Toolchains.Sdl3](../backends/Sdl3/Inno.Build.Toolchains.Sdl3.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Toolchains.Text](../backends/Text/Inno.Build.Toolchains.Text.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Toolchains.UI](../backends/RmlUi/Inno.Build.Toolchains.UI.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Toolchains](Inno.Build.Toolchains.md)：公开引用边界由实际签名核对。
