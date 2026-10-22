# Inno.Build.Toolchains.Bgfx

[分类索引](README.md) · [BGFX Native](Inno.Native.Bgfx.md) · [Wiki 首页](../../README.md)

这是 BGFX Native library 与离线工具的构建库，按明确目标与配置生成 `artifacts/native/<component>/<target>/<fingerprint>` 产物。运行库使用产品配置；标准发行中的 shaderc/texturec 等工具统一使用 Release，与 Editor 的 Debug/Release 配置分别表达。

它允许引用 BGFX Native/Tools 与公共 Toolchains，但不得被 Runtime、Editor feature 或 Player 引用。

## 统一构建入口

本项目现在是库，没有 Program。全部公开构建入口为 `BgfxNativeBuild.BuildAsync(NativeBuildContext, BgfxNativeBuildProfile, CancellationToken = default)` 和 `BgfxToolsBuild.BuildAsync(NativeBuildContext, BgfxNativeBuildProfile, CancellationToken = default)`；前者构建图形 Native，后者独立构建宿主 shaderc/texturec 等离线工具。两者返回 `NativeBuildProduct`。两者验证 debug/release 配置并传播构建失败。正常工作流使用 InnoEngine.Build editor，不启动单独组件 CLI。

两者均使用显式 checkout/configuration；所有 GENie、make 和 MSBuild 子进程接受取消并等待进程树退出。两者在兼容工作目录 `obj/native/<target>/<workspaceIdentity>/Sources` 中更新冻结的 bgfx、bx、bimg 源码并运行上游 GENie；`.build` 只出现在这个私有快照中。源码 checkout 不接收编译产物。工具由集成显式贡献，运行前冻结并计入指纹。

`BgfxNativeBuild.componentDescriptor` 声明运行库及其绑定定义。`BgfxToolsBuild.componentDescriptor` 声明 `bgfx-tools` 产物，复用相同源码与 recipe owner，不消费 managed binding。Editor、独立工具和 Shader Task 选择同一 Release 工具步骤，因此 Runtime 绑定 generation 的变化不会触发工具编译。工具源码、SDK、GENie 与实际参数变化仍会失效。

工具产物使用配置无关的文件名，例如 `shaderc.exe`；配置已经进入工作目录与产物指纹。产品部署索引将逻辑 `shaderc` 映射到本次选中的实际文件。工具消费者不根据自己的 `DEBUG` 符号重新选择配置，也不扫描另一套目录。

```csharp
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.Bgfx;

static Task<NativeBuildProduct> PrepareGraphicsAsync(
    NativeBuildContext preparedContext,
    BgfxNativeBuildProfile integrationProfile,
    CancellationToken cancellationToken
) {
    return BgfxNativeBuild.BuildAsync(preparedContext, integrationProfile, cancellationToken);
}

static Task<NativeBuildProduct> PrepareToolsAsync(
    NativeBuildContext preparedContext,
    BgfxNativeBuildProfile integrationProfile,
    CancellationToken cancellationToken
) {
    return BgfxToolsBuild.BuildAsync(preparedContext, integrationProfile, cancellationToken);
}
```

上述调用用于已有构建操作：`preparedContext` 必须包含冻结的工具链、该组件的构建选项及所需绑定 generation。调用方在整个编译期间保留这些输入的读取保护，并在部署或链接完成后 Dispose 返回的 `NativeBuildProduct`。正常产品构建由发行组合的 `ProductNativeBuildPlan` 统一准备这些依赖。


## 平台配置与所有权

`BgfxNativeBuildProfile` 仅贡献目标 ID、GENie 参数、产物路径 token 和 `CreateBuildInvocation`。
`BgfxBuildInvocation` 冻结工具名称及有序参数，不执行进程。Windows/MSBuild、macOS/Make
的具体配置由所属平台维护。共享 recipe 在 staging 前取得一次 invocation，并把真实参数、工具和布局纳入指纹。
NativeBuildContext target 与 profile 不一致明确失败，不猜测当前 OS 的产品目标。

共享 owner 负责源快照、组件选项、取消、输出验证和原子发布。profile 不持有设备、回调、进程或临时资源。
Browser 静态构建复用组件 CMake recipe，由其平台 aggregate 执行，不使用 GENie profile。

## 准备前源码声明

`BgfxNativeBuild.CreateRecipe` 与 `BgfxToolsBuild.CreateRecipe` 分别声明运行库和离线工具输入。内部 `BgfxBuildRequest` 统一校验 profile、冻结 GENie/编译调用，并提供实际 BuildAsync 消费的 recipe。它是当前实现细节，不是外部扩展类型。源码声明不要求生成输出已经存在；真实 Native 发布继续加入选定绑定内容与 generation，并执行锁后/编译后的 fresh 验证。

## 源码归属

当前唯一源码 owner：`backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/Inno.Build.Toolchains.Bgfx.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

当前声明由源码语义清单核对，包含 public 和可供外部派生实现使用的 protected 成员。构建契约不作为游戏脚本导出；线程、失败与所有权要求见前文及每项 XML。

### `Inno.Build.Toolchains.Bgfx.BgfxBuildInvocation`

<!-- api: T:Inno.Build.Toolchains.Bgfx.BgfxBuildInvocation 63d1b19f036c2d810679bf17f5a3845b3422603af6f2c75d575c7cd10b68c9f2 -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.BgfxBuildInvocation.#ctor(System.String,System.Collections.Generic.IEnumerable{System.String}) 30cc91956f116ee964e75531fa8caea5553f74f3c251d97f69446707a84ff1e7 -->
<!-- api: P:Inno.Build.Toolchains.Bgfx.BgfxBuildInvocation.tool 7b0df42077b042a2138526be044b246e24e35beec726a720decb9954fd175509 -->
<!-- api: P:Inno.Build.Toolchains.Bgfx.BgfxBuildInvocation.arguments be76344e9e436a2f04b32cb9bc6f24215a886b1175f961a5f55f1ae3ff01b6aa -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-2b9a0bd857ae4542"></a>[`public Inno.Build.Toolchains.Bgfx.BgfxBuildInvocation.BgfxBuildInvocation(string tool, System.Collections.Generic.IEnumerable<string> arguments)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/BgfxBuildInvocation.cs#L27) | Snapshots the invocation so later caller mutations cannot change its fingerprint or execution.<br>`tool`：The logical executable declared by the selected native toolchain.<br>`arguments`：Ordered arguments, with source paths relative to the owned source root.<br>T:System.ArgumentException：The tool is blank or an argument is null.<br>T:System.ArgumentNullException：The argument sequence is null. |
| <a id="api-970024d9c94199ea"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Build.Toolchains.Bgfx.BgfxBuildInvocation.arguments { get; }`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/BgfxBuildInvocation.cs#L48) | Gets immutable, ordered compiler arguments included in the recipe identity. |
| <a id="api-b704b4c0dee79094"></a>[`public string Inno.Build.Toolchains.Bgfx.BgfxBuildInvocation.tool { get; }`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/BgfxBuildInvocation.cs#L43) | Gets the declared tool name resolved by the operation's frozen toolchain. |
| <a id="api-9c08ad1b177688d5"></a>[`public sealed class Inno.Build.Toolchains.Bgfx.BgfxBuildInvocation`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/BgfxBuildInvocation.cs#L10) | Freezes an SDK tool name and ordered arguments for execution from the component source root. |

### `Inno.Build.Toolchains.Bgfx.BgfxNativeBuild`

<!-- api: T:Inno.Build.Toolchains.Bgfx.BgfxNativeBuild bd39a7e683d89fa28429822112db4b51c28c45e8eaf0bbf8664c70c72f53fb22 -->
<!-- api: P:Inno.Build.Toolchains.Bgfx.BgfxNativeBuild.componentDescriptor 34362a03da536396f21026f677a9d2b0fc8bb7eb5ae68c3b3a8bb5ec236f2b42 -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.BgfxNativeBuild.BuildAsync(Inno.Build.Toolchains.NativeBuildContext,Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile,System.Threading.CancellationToken) f83583fc113706e3738f7682ec6fe87ebe9cf902b668e64a404ffe42963620d3 -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.BgfxNativeBuild.CreateRecipe(Inno.Build.Toolchains.NativeBuildContext,Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile) 361cabf88a937b156b72a12cb477d66ade2a0147fd234afe792df8d319e5811e -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-f35b349aab2aee01"></a>[`public static System.Threading.Tasks.Task<Inno.Build.Toolchains.NativeBuildProduct> Inno.Build.Toolchains.Bgfx.BgfxNativeBuild.BuildAsync(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile profile, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/BgfxNativeBuild.cs#L49) | Builds and installs graphics artifacts using explicit platform-owned SDK configuration.<br>`context`：The checkout and configuration whose sources and outputs belong to this operation.<br>T:System.ArgumentException：The configuration is not debug or release.<br>T:System.InvalidOperationException：A native build process fails.<br>`profile`：The platform-owned project generation, SDK invocation and output selection.<br>`cancellationToken`：Cancels child processes and prevents artifact installation after cancellation.<br>返回：The validated product with an exact graphics library or offline-tool output closure.<br>T:System.OperationCanceledException：The operation was canceled. |
| <a id="api-68ea1e1acbccfc81"></a>[`public static Inno.Build.Toolchains.NativeBuildRecipe Inno.Build.Toolchains.Bgfx.BgfxNativeBuild.CreateRecipe(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile profile)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/BgfxNativeBuild.cs#L82) | Freezes runtime source, selected tools and profile arguments without generating or compiling.<br>`context`：The operation with an explicit SDK target and shared component options.<br>`profile`：The borrowed immutable integration profile supplying tools, invocation and output layout.<br>返回：The same source declaration consumed by the runtime producer, without selected binding output.<br>T:System.ArgumentNullException：The context or profile is null.<br>T:System.ArgumentException：The profile target or required arguments are invalid.<br>T:System.InvalidOperationException：SDK tools or component options have not been selected.<br>T:System.NotSupportedException：The request uses unsupported linkage or component CMake options.<br>T:System.IO.IOException：A selected tool, build owner or required source checkout is unavailable. |
| <a id="api-c5727f867ca09580"></a>[`public static Inno.Build.Toolchains.NativeComponentDescriptor Inno.Build.Toolchains.Bgfx.BgfxNativeBuild.componentDescriptor { get; }`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/BgfxNativeBuild.cs#L15) | Gets the unique native and recipe owners used for sources and target-scoped intermediates. |
| <a id="api-54ac13d3ef1e7671"></a>[`public static class Inno.Build.Toolchains.Bgfx.BgfxNativeBuild`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/BgfxNativeBuild.cs#L10) | Publishes pinned graphics runtime libraries from an isolated source snapshot. |

### `Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile`

<!-- api: T:Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile f1584927a412b203586956b4161debf2cdac7d3ea6efcc88e41d350672ac957b -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile.SelectTools(Inno.Build.Toolchains.NativeBuildContext) db86d12f00070e6484e8d5fc113b92c53d0d5ca01d31f573216976afa1dadc31 -->
<!-- api: P:Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile.targetId 48436f25ae5ebf072fdad9cfb0bc7da9f35ecafad483bc4b267a45a2caee23f1 -->
<!-- api: P:Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile.generatorArguments 88c4b382ce7c7422b8553b276f1fcc75abf9092c829f44f6407811578716a9da -->
<!-- api: P:Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile.artifactPathToken b2e10e3e9f464c5fa19a77515d8f3b49906e0a1f96b4034db696e42f2d5ff80e -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile.CreateBuildInvocation(Inno.Build.Toolchains.NativeBuildContext,System.Boolean) a3e99fb2c4a81a5436a2df0ddd98fde0c5bb82b7ec45bad71d04daddfdd125fe -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile.#ctor a6033636e494736212d9c9903d8a628bb5d8d3a801e54921bc793f03523e8735 -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-6c94662cc4c57d40"></a>[`protected Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile.BgfxNativeBuildProfile()`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/BgfxNativeBuildProfile.cs#L8) | 由 C# 生成，遵循所属类型的值语义与本页的生命周期约束。 |
| <a id="api-15d33f873c9a0f80"></a>[`public abstract Inno.Build.Toolchains.Bgfx.BgfxBuildInvocation Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile.CreateBuildInvocation(Inno.Build.Toolchains.NativeBuildContext context, bool includeTools)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/BgfxNativeBuildProfile.cs#L48) | Freezes the selected SDK invocation without executing tools or creating staging.<br>`context`：The frozen target tools and configuration used by this operation.<br>`includeTools`：Whether the component must prepare offline compiler executables.<br>返回：Immutable arguments executed from the owned BGFX source snapshot. |
| <a id="api-9e9f54ea66c32ded"></a>[`public abstract Inno.Build.Toolchains.NativeToolchainSelection Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile.SelectTools(Inno.Build.Toolchains.NativeBuildContext context)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/BgfxNativeBuildProfile.cs#L19) | Adds the integration's explicit generator executable and its configuration inputs before preparation.<br>`context`：The requested target, checkout and already selected platform SDK.<br>返回：A frozen selection declaring the generator and all existing SDK commands. |
| <a id="api-7e99189992f57762"></a>[`public abstract string Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile.artifactPathToken { get; }`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/BgfxNativeBuildProfile.cs#L34) | Gets the vendor output directory token used to select this target's artifacts. |
| <a id="api-5158501d7faaa62f"></a>[`public abstract System.Collections.Generic.IReadOnlyList<string> Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile.generatorArguments { get; }`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/BgfxNativeBuildProfile.cs#L29) | Gets ordered vendor project-generation arguments, excluding shared component options. |
| <a id="api-24b79e0ba190fc3f"></a>[`public abstract string Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile.targetId { get; }`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/BgfxNativeBuildProfile.cs#L24) | Gets the exact native target accepted by this configuration. |
| <a id="api-3e3c4725c3e0811d"></a>[`public abstract class Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/BgfxNativeBuildProfile.cs#L8) | Supplies platform-owned project generation and SDK invocation data to the shared BGFX recipe. |

### `Inno.Build.Toolchains.Bgfx.BgfxToolsBuild`

<!-- api: T:Inno.Build.Toolchains.Bgfx.BgfxToolsBuild f6805e97f161dcd8ac82a8a6ad772b81726a8935ce8ee71d8e1b335a1ccff605 -->
<!-- api: P:Inno.Build.Toolchains.Bgfx.BgfxToolsBuild.componentDescriptor 1a18c6af79e7e8bffa0ca57ab3e4a9224725e71b217fa5213fdb0f65f91d4776 -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.BgfxToolsBuild.BuildAsync(Inno.Build.Toolchains.NativeBuildContext,Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile,System.Threading.CancellationToken) ff01bf15a9d27e5554694fdcb7545d8d5996bdfd388c2ed89c7a220aca934904 -->
<!-- api: M:Inno.Build.Toolchains.Bgfx.BgfxToolsBuild.CreateRecipe(Inno.Build.Toolchains.NativeBuildContext,Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile) de96d99179b260f766cf2c49462bab5a81e706dee7cce450fa09416991a1dafe -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-663da76f35a46ed2"></a>[`public static System.Threading.Tasks.Task<Inno.Build.Toolchains.NativeBuildProduct> Inno.Build.Toolchains.Bgfx.BgfxToolsBuild.BuildAsync(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile profile, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/BgfxToolsBuild.cs#L44) | Builds and installs graphics artifacts using explicit platform-owned SDK configuration.<br>`context`：The checkout and configuration whose sources and outputs belong to this operation.<br>T:System.ArgumentException：The configuration is not debug or release.<br>T:System.InvalidOperationException：A native build process fails.<br>`profile`：The platform-owned project generation, SDK invocation and output selection.<br>`cancellationToken`：Cancels child processes and prevents artifact installation after cancellation.<br>返回：The validated product with an exact graphics library or offline-tool output closure.<br>T:System.OperationCanceledException：The operation was canceled. |
| <a id="api-505f57b2e0891049"></a>[`public static Inno.Build.Toolchains.NativeBuildRecipe Inno.Build.Toolchains.Bgfx.BgfxToolsBuild.CreateRecipe(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.Bgfx.BgfxNativeBuildProfile profile)`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/BgfxToolsBuild.cs#L77) | Freezes offline-tool source, selected tools and profile arguments without compiling.<br>`context`：The operation with an explicit SDK target and shared component options.<br>`profile`：The borrowed immutable integration profile supplying tools, invocation and output layout.<br>返回：The same source declaration consumed by the offline-tool producer, without selected binding output.<br>T:System.ArgumentNullException：The context or profile is null.<br>T:System.ArgumentException：The profile target or required arguments are invalid.<br>T:System.InvalidOperationException：SDK tools or component options have not been selected.<br>T:System.NotSupportedException：The request uses unsupported linkage or component CMake options.<br>T:System.IO.IOException：A selected tool, build owner or required source checkout is unavailable. |
| <a id="api-64b190f9999d7be2"></a>[`public static Inno.Build.Toolchains.NativeComponentDescriptor Inno.Build.Toolchains.Bgfx.BgfxToolsBuild.componentDescriptor { get; }`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/BgfxToolsBuild.cs#L15) | Gets the offline-tool producer sharing BGFX source and recipe owners without consuming runtime bindings. |
| <a id="api-a955502f9aa2326f"></a>[`public static class Inno.Build.Toolchains.Bgfx.BgfxToolsBuild`](../../../backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/BgfxToolsBuild.cs#L10) | Publishes pinned graphics tools from an isolated source snapshot. |

## 项目依赖

- [Inno.Build.Toolchains](../../build/Inno.Build.Toolchains.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
