# Inno.Build.Browser

[分类索引](README.md) · [Wiki 首页](../../README.md) · [平台归属与扩展](../../architecture/PLATFORM_EXTENSION_GUIDE.md)

## 职责与边界

Browser Wasm32 发布、Emscripten SDK、静态聚合、Support Pack 和页面布局的唯一 owner。

`ProductBuild.props` 明确声明 BrowserWasm/OpenGLES Shader 配置，Player 经 `InnoProductBuildProperties` 将它传入共享 BGFX 构建。SDK 自动追加的间接引用采用相同配置；共享后端没有 Browser 目标选择表。Browser 不注册 Editor 产品，也不引入 ImGui 资源。

## 组合、生命周期与扩展

`BrowserBuildModule` 贡献 browser-wasm 目标，描述明确 wasm32/RID/产品能力。Emscripten provider 从所选 .NET SDK/workload 获取 clang、sysroot、node 与 python；CMake 和 Ninja 是构建宿主的独立工具，必须在启动构建前配置到 PATH。所有工具的实际路径、内容身份和环境在操作开始时一起冻结。构建宿主可为 Windows/macOS/Linux，输出目标始终为 Browser。

Native 聚合只导入选中组件的 backend-owned StaticLibraries.cmake；第三方源码、依赖、语义编译选项不在 Browser 重复维护。Components.cmake 传入显式引擎、组件和目标生成桥位置。SDK所需 wasm_sjlj_shim.c 仅在本工具链边界。

Native 与 .NET 最终链接消费同一次冻结的目标 bindings、archive closure 和工具身份。WebGL2、回调、异常/longjmp 等实际要求保持。解释执行与 AOT 由独立 DotNet deployment compiler 处理。

BrowserSupportPackValidator 检查完整链接输入和受支持页面资源；source 用隔离 staging、完整校验、指纹与原子发布。浏览器未来替换托管运行时只新增 compiler/link 接入并重验 interop，不改 Rendering/Input/玩法。

Validator 由平台贡献传入同一个不可变 `ProductNativeBuildPlan`，直接使用 backend 声明的 archive/binding 闭包。归档保持 `Native/<component>/browser-wasm/<archive>` 布局；缺失、多余或外国原生二进制均拒绝。静态直接调用绑定不依赖动态加载用的 BGCS.Runtime；Windows/macOS 动态绑定的依赖仍由各自产品保留。

## 平台基础与集成

本项目不引用具体 backend，也不持有内容 compiler。BGFX 的 Native/Shader profile 与内容工厂移入同平台 integration；完整注册由 Standard Distribution 绑定。打包工厂不接收创作服务。产品 props 只声明产品编译选择，不承担 SDK 或第三方实现。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Build.Browser.BrowserBuildModule`

<!-- api: T:Inno.Build.Browser.BrowserBuildModule 54704d771cca1d37033ab1a8c102d050d8d5e83e8126c3466ce1cf172c0f2766 -->
<!-- api: P:Inno.Build.Browser.BrowserBuildModule.target 7faab615227268e536b2a91a20ffa30241c03b49821b0cad70f43867df2b4830 -->
<!-- api: M:Inno.Build.Browser.BrowserBuildModule.CreateContribution(Inno.Build.Composition.BuildCompositionContext,Inno.Build.Toolchains.ProductNativeBuildPlan) 2d7ab15a22be6204cff28f7ac8cb8c4dc97e198b6cf9ec28fff6cc7c4f0a8c24 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8141412a683430cc"></a>[`public static Inno.Build.Composition.BuildPlatformContribution Inno.Build.Browser.BrowserBuildModule.CreateContribution(Inno.Build.Composition.BuildCompositionContext context, Inno.Build.Toolchains.ProductNativeBuildPlan nativePlan)`](../../../platforms/Browser/build/Inno.Build.Browser/BrowserBuildModule.cs#L30) | Contributes the platform's target, product inputs and SDK resolver as one registration.<br>返回：A complete immutable contribution without starting tools or creating files.<br>`context`：The borrowed SDK and execution-host selection.<br>`nativePlan`：The selected shared Player component closure. | — | — |
| <a id="api-61a5778205f82325"></a>[`public static Inno.Build.PlatformTargetDescriptor Inno.Build.Browser.BrowserBuildModule.target { get; }`](../../../platforms/Browser/build/Inno.Build.Browser/BrowserBuildModule.cs#L15) | Gets the explicit implemented product target, independent of the tool execution host. | — | — |
| <a id="api-0aadab04a601c2e4"></a>[`public static class Inno.Build.Browser.BrowserBuildModule`](../../../platforms/Browser/build/Inno.Build.Browser/BrowserBuildModule.cs#L10) | Owns the implemented Browser target facts and complete publication contribution. | — | — |

### `Inno.Build.Browser.BrowserNativeArtifacts`

<!-- api: T:Inno.Build.Browser.BrowserNativeArtifacts 7fc1721dfdea76c087c7d288e6ab09013812ef8bfdaa3c6fa0d8b06fc8df1407 -->
<!-- api: P:Inno.Build.Browser.BrowserNativeArtifacts.fingerprint cf1d4bf25f831be6011db8317fe2866b0c1f93731e143d0d5c2e1c556a20904a -->
<!-- api: P:Inno.Build.Browser.BrowserNativeArtifacts.directory 1e02b6603641ec34abce5448218eb94bff52537a9e1d7f98aa4559a0c3a12e7a -->
<!-- api: P:Inno.Build.Browser.BrowserNativeArtifacts.bindingSelectionPath ef97edd110e9f31a4ff34e44c726728fc0154982980b95d234004e0bc88bd07e -->
<!-- api: M:Inno.Build.Browser.BrowserNativeArtifacts.Dispose 13a0a8537969bceec05bffe9d32775c29a0a59c7fc9c2402fb45dfd13af792f9 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3cd220c5207f2148"></a>[`public void Inno.Build.Browser.BrowserNativeArtifacts.Dispose()`](../../../platforms/Browser/build/Inno.Build.Browser/BrowserNativeArtifacts.cs#L44) | Releases the complete archive closure after managed compilation and copying finish. Published paths must not be accessed afterwards; repeated disposal has no effect. | — | — |
| <a id="api-bc268c0b0b50b462"></a>[`public string Inno.Build.Browser.BrowserNativeArtifacts.bindingSelectionPath { get; }`](../../../platforms/Browser/build/Inno.Build.Browser/BrowserNativeArtifacts.cs#L38) | Gets the immutable MSBuild selection file used to reject mismatched managed binding inputs. | — | — |
| <a id="api-c2c5daceeeebe2d7"></a>[`public string Inno.Build.Browser.BrowserNativeArtifacts.directory { get; }`](../../../platforms/Browser/build/Inno.Build.Browser/BrowserNativeArtifacts.cs#L33) | Gets the absolute immutable install root containing component archive directories. | — | — |
| <a id="api-844d7128a48b2018"></a>[`public string Inno.Build.Browser.BrowserNativeArtifacts.fingerprint { get; }`](../../../platforms/Browser/build/Inno.Build.Browser/BrowserNativeArtifacts.cs#L28) | Gets the identity of the SDK, source, configuration and selected binding generations. | — | — |
| <a id="api-8b17988db90fe683"></a>[`public sealed class Inno.Build.Browser.BrowserNativeArtifacts : System.IDisposable`](../../../platforms/Browser/build/Inno.Build.Browser/BrowserNativeArtifacts.cs#L10) | Identifies the immutable native closure produced by one browser toolchain request. | — | — |

### `Inno.Build.Browser.BrowserPlayerSupportPackSource`

<!-- api: T:Inno.Build.Browser.BrowserPlayerSupportPackSource dbcdf4a14e65f14aa84afbb27e71cfeef33651abbc96ab100baba5fc1caeea7d -->
<!-- api: M:Inno.Build.Browser.BrowserPlayerSupportPackSource.#ctor(Inno.Build.Toolchains.BuildHostDescriptor,Inno.Build.BuildTargetId,Inno.Build.Toolchains.ProductNativeBuildPlan,Inno.Build.Toolchains.INativeBindingGenerator) e004cf9c9c4c5e446d66bb6d543dfa5e18fe4ba4ccedbda47ce20c9287601fae -->
<!-- api: P:Inno.Build.Browser.BrowserPlayerSupportPackSource.target b1e64dddf2ff12f6f133d23a64930cd3201354d748566e7fc0166d43dfe29afe -->
<!-- api: M:Inno.Build.Browser.BrowserPlayerSupportPackSource.CreatePlanAsync(Inno.Build.SupportPacks.PlayerSupportPackPlanningContext,System.Threading.CancellationToken) f6113548fa4b6d227ff37291274dde8a8f8e9d87f0fa340b022a171edf19bc6a -->
<!-- api: M:Inno.Build.Browser.BrowserPlayerSupportPackSource.Validate(System.String) 4f41ad63e4800d0ba8782af98cf3be8c7ea457fc879180267b71faddaa8beba4 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e7d4977ee61f010e"></a>[`public Inno.Build.Browser.BrowserPlayerSupportPackSource.BrowserPlayerSupportPackSource(Inno.Build.Toolchains.BuildHostDescriptor host, Inno.Build.BuildTargetId toolsTarget, Inno.Build.Toolchains.ProductNativeBuildPlan nativePlan, Inno.Build.Toolchains.INativeBindingGenerator bindingGenerator)`](../../../platforms/Browser/build/Inno.Build.Browser/SupportPacks/BrowserPlayerSupportPackSource.cs#L44) | Captures independent Browser product and offline compiler selections.<br>`host`：The execution machine permitted to run the selected SDK.<br>`toolsTarget`：The explicit target of offline tools used while preparing the product.<br>`nativePlan`：The borrowed immutable Player component closure.<br>`bindingGenerator`：The borrowed generation service used by the Browser component batch. | — | `System.ArgumentNullException`：The host or native plan is null.<br>`System.ArgumentException`：The offline tool target is unassigned. |
| <a id="api-0bfe9ecd49770ea5"></a>[`public System.Threading.Tasks.ValueTask<Inno.Build.SupportPacks.PlayerSupportPackPlan> Inno.Build.Browser.BrowserPlayerSupportPackSource.CreatePlanAsync(Inno.Build.SupportPacks.PlayerSupportPackPlanningContext context, System.Threading.CancellationToken cancellationToken)`](../../../platforms/Browser/build/Inno.Build.Browser/SupportPacks/BrowserPlayerSupportPackSource.cs#L64) | Validates discovery inputs and freezes a plan before publication output exists.<br>返回：A frozen plan with no running work; missing tools and unsupported hosts fail here.<br>`context`：The source checkout and host used for read-only SDK discovery.<br>`cancellationToken`：Cancels preparation before installation. | — | — |
| <a id="api-f7148dc1a8e5280b"></a>[`public void Inno.Build.Browser.BrowserPlayerSupportPackSource.Validate(string directory)`](../../../platforms/Browser/build/Inno.Build.Browser/SupportPacks/BrowserPlayerSupportPackSource.cs#L130) | Rejects an incomplete or incompatible platform closure.<br>`directory`：The prepared or installed Support Pack directory. | — | `System.IO.InvalidDataException`：A required platform input is absent or incompatible. |
| <a id="api-b5611a9b5aa2d03c"></a>[`public Inno.Build.BuildTargetId Inno.Build.Browser.BrowserPlayerSupportPackSource.target { get; }`](../../../platforms/Browser/build/Inno.Build.Browser/SupportPacks/BrowserPlayerSupportPackSource.cs#L61) | Gets the stable target identity implemented by this source. | — | — |
| <a id="api-c9d459282d574c55"></a>[`public sealed class Inno.Build.Browser.BrowserPlayerSupportPackSource : Inno.Build.SupportPacks.IPlayerSupportPackSource`](../../../platforms/Browser/build/Inno.Build.Browser/SupportPacks/BrowserPlayerSupportPackSource.cs#L16) | Prepares Browser runtime sources and one frozen native closure for isolated publication. | — | — |

### `Inno.Build.Browser.BrowserSupportPackValidator`

<!-- api: T:Inno.Build.Browser.BrowserSupportPackValidator 509adc5a67350e2423b307aed14493bcdb45d73e22a62d2caa9edc1c13244698 -->
<!-- api: M:Inno.Build.Browser.BrowserSupportPackValidator.#ctor(Inno.Build.Toolchains.ProductNativeBuildPlan) f63dfbd8c2847f7db90a3b1351799e27536c6955bb45c51ce6b63d9c7af6b579 -->
<!-- api: M:Inno.Build.Browser.BrowserSupportPackValidator.Validate(System.String) 692bb266d99ad3b94334ae2e1158941255144b53ba2396db81c1817038aa192b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9a2f56e10049a266"></a>[`public Inno.Build.Browser.BrowserSupportPackValidator.BrowserSupportPackValidator(Inno.Build.Toolchains.ProductNativeBuildPlan nativePlan)`](../../../platforms/Browser/build/Inno.Build.Browser/SupportPacks/BrowserSupportPackValidator.cs#L29) | Captures the selected static closure so validation and publication use the same backend declarations.<br>`nativePlan`：The immutable Player closure whose component owners declare archives and generated assemblies. | — | `System.ArgumentNullException`：The plan is null.<br>`System.ArgumentException`：A selected component does not provide a static build definition. |
| <a id="api-1851606fce78a68f"></a>[`public void Inno.Build.Browser.BrowserSupportPackValidator.Validate(string directory)`](../../../platforms/Browser/build/Inno.Build.Browser/SupportPacks/BrowserSupportPackValidator.cs#L49) | Rejects incomplete or foreign platform inputs in the supplied Support Pack.<br>`directory`：The isolated or installed target directory to validate. | — | `System.IO.InvalidDataException`：A required input is absent or the closure contains a foreign native binary. |
| <a id="api-536fc6ea16d478d9"></a>[`public sealed class Inno.Build.Browser.BrowserSupportPackValidator : Inno.Build.IPlayerSupportPackValidator`](../../../platforms/Browser/build/Inno.Build.Browser/SupportPacks/BrowserSupportPackValidator.cs#L12) | Validates the platform runtime or linker closure before compilation and installation. | — | — |

### `Inno.Build.Browser.BrowserToolchain`

<!-- api: T:Inno.Build.Browser.BrowserToolchain 75ba1fedf1289f515f7734de5982c545dbf148e5fecb73a474d7668936f7eba3 -->
<!-- api: P:Inno.Build.Browser.BrowserToolchain.componentDescriptor 95658d0f2f028935bf5391c09a944579b18aa0673b57a540ce00e11274e70d97 -->
<!-- api: M:Inno.Build.Browser.BrowserToolchain.BuildAsync(Inno.Build.Toolchains.NativeBuildContext,Inno.Build.Toolchains.ProductNativeBuildPlan,System.Threading.CancellationToken) 7a2825cb99c6b9b0cb4c4ccac9f7276c6e31829646b763b1111b705c4a0e6989 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1ce1b58b4b7857b1"></a>[`public static System.Threading.Tasks.Task<Inno.Build.Browser.BrowserNativeArtifacts> Inno.Build.Browser.BrowserToolchain.BuildAsync(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.ProductNativeBuildPlan plan, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../platforms/Browser/build/Inno.Build.Browser/BrowserToolchain.cs#L49) | Publishes one exact static closure without discovering components or reselecting tools.<br>返回：The immutable complete native closure and corresponding managed binding selection.<br>`context`：The owned checkout and previously resolved Emscripten selection.<br>`plan`：The frozen product component graph with backend-owned static definitions.<br>`cancellationToken`：Cancels generation and drains active compilation before returning. | — | `System.InvalidOperationException`：The tools, target or static component capability are incompatible.<br>`System.IO.InvalidDataException`：A component produces an incomplete or foreign archive closure.<br>`System.OperationCanceledException`：Preparation was canceled before publication. |
| <a id="api-0b6a000f2f0dd8c5"></a>[`public static Inno.Build.Toolchains.NativeComponentDescriptor Inno.Build.Browser.BrowserToolchain.componentDescriptor { get; }`](../../../platforms/Browser/build/Inno.Build.Browser/BrowserToolchain.cs#L20) | Gets the sole owner of Browser aggregation, ABI constraints and SDK link support. | — | — |
| <a id="api-0e06fd529c8bac5b"></a>[`public static class Inno.Build.Browser.BrowserToolchain`](../../../platforms/Browser/build/Inno.Build.Browser/BrowserToolchain.cs#L15) | Aggregates explicitly selected backend static recipes using a frozen Emscripten SDK. | — | — |

### `Inno.Build.Browser.BrowserWasm32GameBuildTarget`

<!-- api: T:Inno.Build.Browser.BrowserWasm32GameBuildTarget b9c732c9b08c04f003ac5f8bb92f7237af15cf01dcb1b3d2a702778d02ac114d -->
<!-- api: M:Inno.Build.Browser.BrowserWasm32GameBuildTarget.#ctor(Inno.Build.IPlayerSupportPackValidator) 6c7714d1997d20de59c9d65d89fb25d21d81d38c22702b3f208d207c7ef75e5d -->
<!-- api: M:Inno.Build.Browser.BrowserWasm32GameBuildTarget.Validate(System.String) db20b553074a2a59524647da453106cc7db5b75adf2ae382590343bbfbb0473b -->
<!-- api: P:Inno.Build.Browser.BrowserWasm32GameBuildTarget.id c67aed68fb88d2cec694ae358f5c6b8dfdfedfeb9b18b475281bfa3c3f96ab5e -->
<!-- api: P:Inno.Build.Browser.BrowserWasm32GameBuildTarget.defaultManagedDeployment 468648de13ec1e50942b1b7222e3a32434e87dae139f7652bb61ebfe248d40ce -->
<!-- api: P:Inno.Build.Browser.BrowserWasm32GameBuildTarget.runtimeIdentifier 32d8c04d1530f0fde17919de2998c462de61267b3611302b5d0cea6cff059394 -->
<!-- api: P:Inno.Build.Browser.BrowserWasm32GameBuildTarget.displayName 7f9e00466663c62001ac15e838c73b91d2306e4215fcb47a16c4616d347b3c52 -->
<!-- api: M:Inno.Build.Browser.BrowserWasm32GameBuildTarget.PackageAsync(Inno.Build.GameBuildPackageContext,System.Threading.CancellationToken) 639f412d3eb9acab1e0e921f4e9d3abb2dca4479849a703c7324e0f1c4e26daa -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-86129c55208e82fc"></a>[`public Inno.Build.Browser.BrowserWasm32GameBuildTarget.BrowserWasm32GameBuildTarget(Inno.Build.IPlayerSupportPackValidator supportPackValidator)`](../../../platforms/Browser/build/Inno.Build.Browser/Targets/BrowserWasm32GameBuildTarget.cs#L26) | Creates a site packager bound to the selected native closure validator.<br>`supportPackValidator`：The borrowed validator for the distribution's browser Support Pack. | — | `System.ArgumentNullException`：The validator is null. |
| <a id="api-60a8e88397a6ac0d"></a>[`public System.Threading.Tasks.ValueTask<string> Inno.Build.Browser.BrowserWasm32GameBuildTarget.PackageAsync(Inno.Build.GameBuildPackageContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../platforms/Browser/build/Inno.Build.Browser/Targets/BrowserWasm32GameBuildTarget.cs#L73) | Composes an independently hostable site from a verified browser Support Pack and content pack.<br>返回：The staged static-site directory.<br>`context`：The verified runtime and packaged project content.<br>`cancellationToken`：Cancels copying before the build pipeline commits the site. | — | — |
| <a id="api-97435d4c7f1d8c3a"></a>[`public void Inno.Build.Browser.BrowserWasm32GameBuildTarget.Validate(string directory)`](../../../platforms/Browser/build/Inno.Build.Browser/Targets/BrowserWasm32GameBuildTarget.cs#L41) | Validates the target closure before runtime script compilation.<br>`directory`：The Support Pack directory selected by the build catalog. | — | `System.IO.InvalidDataException`：The required platform inputs are incomplete or incompatible. |
| <a id="api-0a3e0b5ec6d1ce56"></a>[`public Inno.Build.Managed.ManagedDeploymentId Inno.Build.Browser.BrowserWasm32GameBuildTarget.defaultManagedDeployment { get; }`](../../../platforms/Browser/build/Inno.Build.Browser/Targets/BrowserWasm32GameBuildTarget.cs#L49) | Gets the managed publisher selected when the profile does not specify an override. | — | — |
| <a id="api-d622b57a1bf6c765"></a>[`public string Inno.Build.Browser.BrowserWasm32GameBuildTarget.displayName { get; }`](../../../platforms/Browser/build/Inno.Build.Browser/Targets/BrowserWasm32GameBuildTarget.cs#L57) | Gets the name shown by authoring hosts. | — | — |
| <a id="api-19780df0dc9e2fa0"></a>[`public Inno.Build.BuildTargetId Inno.Build.Browser.BrowserWasm32GameBuildTarget.id { get; }`](../../../platforms/Browser/build/Inno.Build.Browser/Targets/BrowserWasm32GameBuildTarget.cs#L46) | Gets the browser WebAssembly target identity. | — | — |
| <a id="api-170f2f74088195ea"></a>[`public string Inno.Build.Browser.BrowserWasm32GameBuildTarget.runtimeIdentifier { get; }`](../../../platforms/Browser/build/Inno.Build.Browser/Targets/BrowserWasm32GameBuildTarget.cs#L52) | Gets the managed toolchain target identifier required by this platform composition. | — | — |
| <a id="api-5a5e48a36baa7a3a"></a>[`public sealed class Inno.Build.Browser.BrowserWasm32GameBuildTarget : Inno.Build.IGameBuildTarget`](../../../platforms/Browser/build/Inno.Build.Browser/Targets/BrowserWasm32GameBuildTarget.cs#L12) | Packages a verified browser Player as an independently hostable static site. | — | — |

### `Inno.Build.Browser.EmscriptenNativeToolchainProvider`

<!-- api: T:Inno.Build.Browser.EmscriptenNativeToolchainProvider bd82c9e2d489e924fdbdcc1df698c99a0ebcc4f2e905d6331b08bd268323b659 -->
<!-- api: M:Inno.Build.Browser.EmscriptenNativeToolchainProvider.#ctor(System.String) 7c6fc21cee6c37caa1cbf3262bff28073b15f0c6d0329108a665b92b0f3c0766 -->
<!-- api: M:Inno.Build.Browser.EmscriptenNativeToolchainProvider.ResolveAsync(Inno.Build.Toolchains.NativeBuildContext,Inno.Build.Toolchains.BuildHostDescriptor,System.String,System.Threading.CancellationToken) 96483456951852b06140ebd4ca97715cf845bb512a807dc535090c4810845eee -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-58214cb6490ffe2e"></a>[`public Inno.Build.Browser.EmscriptenNativeToolchainProvider.EmscriptenNativeToolchainProvider(string dotnetHost)`](../../../platforms/Browser/build/Inno.Build.Browser/Toolchain/EmscriptenNativeToolchainProvider.cs#L26) | Captures the SDK executable selected by the build host without probing installed SDK versions.<br>`dotnetHost`：The host-selected SDK executable or explicit command. | — | `System.ArgumentException`：The SDK command is blank. |
| <a id="api-986797988c73150b"></a>[`public System.Threading.Tasks.ValueTask<Inno.Build.Toolchains.NativeToolchainSelection> Inno.Build.Browser.EmscriptenNativeToolchainProvider.ResolveAsync(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.BuildHostDescriptor host, string targetId, System.Threading.CancellationToken cancellationToken)`](../../../platforms/Browser/build/Inno.Build.Browser/Toolchain/EmscriptenNativeToolchainProvider.cs#L33) | Freezes compiler tools, SDK inputs and child environment before a native operation starts.<br>返回：A complete immutable tool selection for the requested host and target.<br>`context`：The checkout and configuration owned by the requesting operation.<br>`host`：The explicitly declared execution host, independent of the target.<br>`targetId`：The exact requested target; unsupported combinations must fail.<br>`cancellationToken`：Cancels SDK discovery before any product staging. | — | — |
| <a id="api-dce403c02bd328a4"></a>[`public sealed class Inno.Build.Browser.EmscriptenNativeToolchainProvider : Inno.Build.Toolchains.INativeToolchainProvider`](../../../platforms/Browser/build/Inno.Build.Browser/Toolchain/EmscriptenNativeToolchainProvider.cs#L13) | Selects Emscripten from the Browser product's actual SDK workload for an explicit wasm32 target. | — | — |

### `Inno.Build.Browser.EmscriptenToolchainResolver`

<!-- api: T:Inno.Build.Browser.EmscriptenToolchainResolver c9f6915710431c58a6983a2690bf38933515343c310730df57aa1c02ef2b1055 -->
<!-- api: M:Inno.Build.Browser.EmscriptenToolchainResolver.ResolveAsync(System.String,System.String,System.Threading.CancellationToken) 3b4674d107044267d9e2b26c7b2c763c0cbb1016e08b63e22936a993bbe5bdf8 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-5146753cba620fce"></a>[`public static System.Threading.Tasks.Task<System.Collections.Generic.IReadOnlyDictionary<string, string>> Inno.Build.Browser.EmscriptenToolchainResolver.ResolveAsync(string dotnetHost, string projectPath, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../platforms/Browser/build/Inno.Build.Browser/EmscriptenToolchainResolver.cs#L50) | Resolves the native toolchain actually selected by MSBuild for one browser project.<br>返回：Variables for native builds and binding generation, without changing the parent process.<br>`dotnetHost`：The SDK host executable, either an absolute path or a name available on PATH.<br>`projectPath`：The browser project whose target framework and SDK determine workload selection.<br>`cancellationToken`：Cancels the hidden SDK query and waits for its exit. | Compiler worker budgets remain inherited from the host environment or tool defaults. Runtime threading policy does not constrain build-time parallelism. | `System.IO.FileNotFoundException`：The SDK host or a required Emscripten workload component is missing.<br>`System.ArgumentException`：The SDK host or project path is empty.<br>`System.IO.DirectoryNotFoundException`：A selected SDK directory or sysroot is missing.<br>`System.InvalidOperationException`：Project evaluation fails or does not select a complete native toolchain.<br>`System.OperationCanceledException`：The operation was canceled. |
| <a id="api-324b1673998b7e5c"></a>[`public static class Inno.Build.Browser.EmscriptenToolchainResolver`](../../../platforms/Browser/build/Inno.Build.Browser/EmscriptenToolchainResolver.cs#L14) | Resolves Emscripten from the workload selected by one application project. | — | — |

## 项目依赖

- [Inno.Build](../../build/Inno.Build.md)：公开引用边界由实际签名核对。
- [Inno.Build.Managed](../../build/Inno.Build.Managed.md)：公开引用边界由实际签名核对。
- [Inno.Build.Toolchains](../../build/Inno.Build.Toolchains.md)：公开引用边界由实际签名核对。
- [Inno.Core.IO](../../core/Inno.Core.IO.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Composition](../../build/Inno.Build.Composition.md)：公开引用边界由实际签名核对。
- [Inno.Build.SupportPacks.Core](../../build/Inno.Build.SupportPacks.Core.md)：公开引用边界由实际签名核对。
