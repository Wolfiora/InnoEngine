# Inno.Build.Products.Standard

[分类索引](README.md) · [Wiki 首页](../README.md) · [职责与文件树](../architecture/NATIVE_BUILD_PLATFORM_OWNERSHIP.md)

## 职责、依赖与生命周期

标准 Player、Editor 与离线 Shader 工具的组件闭包、依赖和部署清单组合。平台提供完整组件选项和输出布局。本库不发现 SDK、不选择平台、不执行原生编译。

`StandardNativeComponents.CreatePlayer`、`CreateEditor`、`CreateShaderTools` 接收 graphics profile、组件配置函数与布局函数；`CreateStaticPlayer` 接收显式 Static 配置与布局。`StandardSupportPackRequirements` 从实际计划和链接格式产生闭包要求；`StandardNativeDeployment` 编码已验证的 Native 加载清单。

调用方先取得平台贡献，再使用贡献的执行器。布局函数只在构造部署清单时消费相对输出；不得捕获插件 generation 或 live domain 对象。

## 使用示例

```csharp
using System;
using Inno.Build.Products.Standard;
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.Bgfx;

static ProductNativeBuildPlan ComposePlayer(
    BgfxNativeBuildProfile graphics,
    Func<string, NativeComponentBuildOptions> options,
    Func<string, string, string, string?> layout
) => StandardNativeComponents.CreatePlayer(graphics, options, layout);
```

示例借用已预检的宿主和发行上下文；实际执行需要明确工具链、绑定生成器与完整源码输入。失败、取消和不可用能力不发布部分结果。此构建协议不进入游戏脚本导出，也不保存 collectible generation 状态。

## 当前源码公开 API 清单

当前声明由源码语义清单核对，包含 public 和可供外部派生实现使用的 protected 成员。构建契约不作为游戏脚本导出；线程、失败与所有权要求见前文及每项 XML。

### `Inno.Build.Products.Standard.StandardNativeComponents`

<!-- api: T:Inno.Build.Products.Standard.StandardNativeComponents 3a678e37a83d88d83b6d645e7fc720c1b2aee7cb2d26479d8a1eb2361ce34f0d -->
<!-- api: M:Inno.Build.Products.Standard.StandardNativeComponents.CreatePlayer(Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile,System.Func{System.String,Inno.Build.Toolchains.NativeComponentBuildOptions},System.Func{System.String,System.String,System.String,System.String}) 75911df82f820d874c775d8410cb1cef0d1fa17ccd115a9a73ca4d7abd5a3211 -->
<!-- api: M:Inno.Build.Products.Standard.StandardNativeComponents.CreateShaderTools(Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile,System.Func{System.String,Inno.Build.Toolchains.NativeComponentBuildOptions},System.Func{System.String,System.String,System.String,System.String}) 65791916a0c081585cd256e71d81f729c93c92dca013abf35cd7db4d97b490d5 -->
<!-- api: M:Inno.Build.Products.Standard.StandardNativeComponents.CreateEditor(Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile,System.Func{System.String,Inno.Build.Toolchains.NativeComponentBuildOptions},System.Func{System.String,System.String,System.String,System.String}) 49a03cdef041e6bc4dfb487d5b716d7d8606e257155befb290edb20b2b7965a3 -->
<!-- api: M:Inno.Build.Products.Standard.StandardNativeComponents.CreateStaticPlayer(System.Func{System.String,Inno.Build.Toolchains.NativeComponentBuildOptions},System.Func{System.String,System.String,System.String,System.String}) e0df76e2f6bd4cb407641652d8ac17c4517a30a59218b70e56f8a862453b4348 -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-02201969b70e41b6"></a>[`public static Inno.Build.Toolchains.ProductNativeBuildPlan Inno.Build.Products.Standard.StandardNativeComponents.CreateEditor(Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile profile, System.Func<string, Inno.Build.Toolchains.NativeComponentBuildOptions> options, System.Func<string, string, string, string?> layout)`](../../build/products/Inno.Build.Products.Standard/StandardNativeComponents.cs#L77) | Composes the shared runtime closure and the Editor's presentation and authoring tools.<br>返回：A fresh immutable Editor plan with the ImGuizmo dependency declared explicitly.<br>`profile`：The selected integration-owned BGFX SDK invocation for standalone component builds.<br>`options`：The platform-owned configuration for each named component.<br>`layout`：The platform-owned destination of each component output, or null to exclude it. |
| <a id="api-75e6d05a4bf15784"></a>[`public static Inno.Build.Toolchains.ProductNativeBuildPlan Inno.Build.Products.Standard.StandardNativeComponents.CreatePlayer(Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile profile, System.Func<string, Inno.Build.Toolchains.NativeComponentBuildOptions> options, System.Func<string, string, string, string?> layout)`](../../build/products/Inno.Build.Products.Standard/StandardNativeComponents.cs#L35) | Composes the standard Player's reusable native components with explicit deployment policies.<br>返回：A fresh immutable Player plan without resolving tools or touching source files.<br>`profile`：The selected integration-owned BGFX SDK invocation for standalone component builds.<br>`options`：The platform-owned configuration for each named component.<br>`layout`：The platform-owned destination of each component output, or null to exclude it. |
| <a id="api-dc3a0dcc33466d83"></a>[`public static Inno.Build.Toolchains.ProductNativeBuildPlan Inno.Build.Products.Standard.StandardNativeComponents.CreateShaderTools(Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile profile, System.Func<string, Inno.Build.Toolchains.NativeComponentBuildOptions> options, System.Func<string, string, string, string?> layout)`](../../build/products/Inno.Build.Products.Standard/StandardNativeComponents.cs#L56) | Selects only the offline graphics compiler component needed for shader publication.<br>返回：An immutable tool product without runtime or Editor presentation dependencies.<br>`profile`：The selected integration-owned BGFX SDK invocation for standalone component builds.<br>`options`：The platform-owned configuration for each named component.<br>`layout`：The platform-owned destination of each component output, or null to exclude it. |
| <a id="api-cf06c85b2ae1f422"></a>[`public static Inno.Build.Toolchains.ProductNativeBuildPlan Inno.Build.Products.Standard.StandardNativeComponents.CreateStaticPlayer(System.Func<string, Inno.Build.Toolchains.NativeComponentBuildOptions> options, System.Func<string, string, string, string?> layout)`](../../build/products/Inno.Build.Products.Standard/StandardNativeComponents.cs#L126) | Declares the real static component closure consumed by an explicit aggregate executor.<br>返回：A Player closure with static recipes and no unused standalone producers.<br>`options`：The platform-owned configuration for each named component.<br>`layout`：The platform-owned destination of each component output, or null to exclude it. |
| <a id="api-81123f2d536f208d"></a>[`public static class Inno.Build.Products.Standard.StandardNativeComponents`](../../build/products/Inno.Build.Products.Standard/StandardNativeComponents.cs#L18) | Declares the standard product closures while keeping backend selection out of shared build mechanisms. |

### `Inno.Build.Products.Standard.StandardNativeDeployment`

<!-- api: T:Inno.Build.Products.Standard.StandardNativeDeployment cf99372eb9bc379f21925be2dda4799abf6467076b2e744562608abe9f60f14b -->
<!-- api: M:Inno.Build.Products.Standard.StandardNativeDeployment.CreateValidator(Inno.Build.BuildTargetId,System.String) 1934b827669361c8bf173548f21778886614dc208ac5abe0a09983051af741e8 -->
<!-- api: M:Inno.Build.Products.Standard.StandardNativeDeployment.CreateMetadata(System.Collections.Generic.IReadOnlyList{Inno.Build.Toolchains.NativeBuildProduct},System.Collections.Generic.IReadOnlyDictionary{System.String,System.String}) 84cb74d563fc2fba46db3196bb9b5605528941e38aa73bd99cea8bcc718926e2 -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-4e6d420c82ea8b41"></a>[`public static System.Collections.Generic.IReadOnlyDictionary<string, byte[]> Inno.Build.Products.Standard.StandardNativeDeployment.CreateMetadata(System.Collections.Generic.IReadOnlyList<Inno.Build.Toolchains.NativeBuildProduct> products, System.Collections.Generic.IReadOnlyDictionary<string, string> payloads)`](../../build/products/Inno.Build.Products.Standard/StandardNativeDeployment.cs#L45) | Encodes the exact runtime payload selected by the platform layout.<br>`products`：The validated product closure.<br>`payloads`：The exact destination mapping.<br>返回：Native loading metadata for the complete closure. |
| <a id="api-86488ba3f218e2a4"></a>[`public static Inno.Build.IPlayerSupportPackValidator Inno.Build.Products.Standard.StandardNativeDeployment.CreateValidator(Inno.Build.BuildTargetId target, string extension)`](../../build/products/Inno.Build.Products.Standard/StandardNativeDeployment.cs#L28) | Creates a validator for the platform-selected native suffix.<br>`target`：The platform target.<br>`extension`：The platform runtime-library suffix.<br>返回：A validator that rejects foreign or incomplete deployment metadata. |
| <a id="api-f8a7c4af670eb12c"></a>[`public static class Inno.Build.Products.Standard.StandardNativeDeployment`](../../build/products/Inno.Build.Products.Standard/StandardNativeDeployment.cs#L14) | Composes validated standard product deployment metadata. |

### `Inno.Build.Products.Standard.StandardSupportPackRequirements`

<!-- api: T:Inno.Build.Products.Standard.StandardSupportPackRequirements ce08dd65ef3058715621eaad32d91987fc656e828b94d77c4bdd9507c930e3a7 -->
<!-- api: M:Inno.Build.Products.Standard.StandardSupportPackRequirements.CreateDynamic(Inno.Build.Toolchains.ProductNativeBuildPlan,Inno.Build.BuildTargetId,System.String) 5507ecf8c7019c53fe27a1c5967aa2a65504bc6fa1e21334bc66efe54daf4baa -->
<!-- api: M:Inno.Build.Products.Standard.StandardSupportPackRequirements.CreateStatic(Inno.Build.Toolchains.ProductNativeBuildPlan,Inno.Build.BuildTargetId,Inno.Build.Toolchains.NativeLinkPlan) ddbdc4fec18062bbe10d8ff8634f6e0b57df68ac1c9cd57494830ea0ea182eb6 -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-0a6162089e00223f"></a>[`public static Inno.Build.SupportPacks.PlayerSupportPackRequirements Inno.Build.Products.Standard.StandardSupportPackRequirements.CreateDynamic(Inno.Build.Toolchains.ProductNativeBuildPlan plan, Inno.Build.BuildTargetId target, string extension)`](../../build/products/Inno.Build.Products.Standard/StandardSupportPackRequirements.cs#L30) | Declares the complete standard Player inputs for the selected linkage.<br>`plan`：The platform-configured component graph.<br>`target`：The exact platform target.<br>`extension`：The platform runtime-library suffix.<br>返回：Requirements matching the plan's actual component closure. |
| <a id="api-85d806cd2a868c7e"></a>[`public static Inno.Build.SupportPacks.PlayerSupportPackRequirements Inno.Build.Products.Standard.StandardSupportPackRequirements.CreateStatic(Inno.Build.Toolchains.ProductNativeBuildPlan plan, Inno.Build.BuildTargetId target, Inno.Build.Toolchains.NativeLinkPlan linkPlan)`](../../build/products/Inno.Build.Products.Standard/StandardSupportPackRequirements.cs#L58) | Declares the complete standard Player inputs for the selected linkage.<br>`plan`：The platform-configured component graph.<br>`target`：The exact platform target.<br>返回：Requirements matching the plan's actual component closure.<br>`linkPlan`：The explicit platform archive naming plan. |
| <a id="api-660299cc7e807fa0"></a>[`public static class Inno.Build.Products.Standard.StandardSupportPackRequirements`](../../build/products/Inno.Build.Products.Standard/StandardSupportPackRequirements.cs#L13) | Composes validated standard product Support Pack requirements. |
