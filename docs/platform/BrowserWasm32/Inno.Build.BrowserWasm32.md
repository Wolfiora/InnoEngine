# Inno.Build.BrowserWasm32

[分类索引](README.md) · [Wiki 首页](../../README.md) · [平台归属与扩展](../../architecture/PLATFORM_EXTENSION_GUIDE.md)

## 职责与边界

Browser Wasm32 发布、Emscripten SDK、静态聚合、Support Pack 和页面布局的唯一 owner。

页面 `index.html` 与 `main.js` 是明确的 Pack 准备输入，按内容冻结并 materialize 到所属执行副本；模板和页面复制消费该副本，不在发布末尾读取可变仓库文件。

Emscripten 输入覆盖实际 LLVM `bin`、Python 运行时及源码、SDK 工具和 sysroot headers。Python 的 `__pycache__` 是派生输出，不属于 SDK 源身份；CMake 执行把字节码写到当前 workspace 的 `CMake/PythonCache`。SDK 源码或编译器改变会选择另一 workspace，旧字节码不能跨 SDK 身份复用；最终 .NET 发布生成的 SDK 字节码也不使共享 Native 闭包失效。

`ProductBuild.props` 明确声明 BrowserWasm/OpenGLES Shader 配置，Player 经 `InnoProductBuildProperties` 将它传入共享 BGFX 构建。SDK 自动追加的间接引用采用相同配置；共享后端没有 Browser 目标选择表。Browser 不注册 Editor 产品，也不引入 ImGui 资源。

## 组合、生命周期与扩展

`BrowserWasm32BuildModule` 贡献 browser-wasm 目标，描述明确 wasm32/RID/产品能力。Emscripten provider 求值实际 `Microsoft.NET.Sdk.WebAssembly` Player 发布模板，从所选 .NET SDK/workload 获取 clang、sysroot、node 与 python；平台源码 Library 不承担可执行产品的 SDK 选择。CMake 和 Ninja 是构建宿主的独立工具，必须在启动构建前配置到 PATH。所有工具的实际路径、内容身份和环境在操作开始时一起冻结。当前构建宿主为 WindowsX64 或 MacOSArm64，输出目标始终为 Browser。

Native 聚合只导入选中组件的 backend-owned StaticLibraries.cmake；第三方源码、依赖、语义编译选项不在 Browser 重复维护。Components.cmake 传入显式引擎、组件和目标生成桥位置。SDK所需 wasm_sjlj_shim.c 仅在本工具链边界。

Native 与 .NET 最终链接消费同一次冻结的目标 bindings、archive closure 和工具身份。WebGL2、回调、异常/longjmp 等实际要求保持。解释执行与 AOT 由独立 DotNet deployment compiler 处理。

BrowserWasm32SupportPackValidator 检查完整链接输入和受支持页面资源；source 用隔离 staging、完整校验、指纹与原子发布。浏览器未来替换托管运行时只新增 compiler/link 接入并重验 interop，不改 Rendering/Input/玩法。

Validator 由平台贡献传入同一个不可变 `ProductNativeBuildPlan`，直接使用 backend 声明的 archive/binding 闭包。归档保持 `Native/<component>/browser-wasm/<archive>` 布局；缺失、多余或外国原生二进制均拒绝。各目标绑定都使用固定的 `BGCS.Runtime` 包：Browser 从 SDK 静态导出表建立调用会话，动态目标从部署索引中的库建立调用会话。

## 平台基础与集成

本项目不引用具体 backend，也不持有内容 compiler。BGFX 的 Native/Shader profile 与内容工厂移入同平台 integration；完整注册由 Standard Distribution 绑定。打包工厂不接收创作服务。产品 props 只声明产品编译选择，不承担 SDK 或第三方实现。

## 当前源码公开 API 清单

签名来自当前 SDK 求值与语义符号；按类型列出 public/protected 契约。工作流与生命周期见本页前文。

### `Inno.Build.BrowserWasm32.BrowserWasm32BuildModule`

<!-- api: T:Inno.Build.BrowserWasm32.BrowserWasm32BuildModule f62ef6f888fd548f6a019750228ae6edf18ef4c1e2e0f6ecbe3476a1316490a0 -->
<!-- api: P:Inno.Build.BrowserWasm32.BrowserWasm32BuildModule.target 44688264661a9d0eda1c4cde60b07836705bd18bb8c8404cb8b0efd1bf8dd4fa -->
<!-- api: M:Inno.Build.BrowserWasm32.BrowserWasm32BuildModule.CreateContribution(Inno.Build.Composition.BuildCompositionContext,Inno.Build.Toolchains.ProductNativeBuildPlan,Inno.Build.IPlayerSupportPackValidator) e33bcc6667451595b6dbbc17357626d92920854847d4ebe84f3f1e37b81ffe7c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8088d9574ed5a8a1"></a>[`public static Inno.Build.Composition.BuildPlatformContribution Inno.Build.BrowserWasm32.BrowserWasm32BuildModule.CreateContribution(Inno.Build.Composition.BuildCompositionContext context, Inno.Build.Toolchains.ProductNativeBuildPlan nativePlan, Inno.Build.IPlayerSupportPackValidator supportPackValidator)`](../../../platforms/BrowserWasm32/build/Inno.Build.BrowserWasm32/BrowserWasm32BuildModule.cs#L33) | Contributes the platform's target, product inputs and SDK resolver as one registration.<br>返回：A complete immutable contribution without starting tools or creating files.<br>`context`：The borrowed SDK and execution-host selection.<br>`nativePlan`：The selected shared Player component closure.<br>`supportPackValidator`：The borrowed platform and selected static-closure validator. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-f3722fa3268840dd"></a>[`public static Inno.Build.PlatformTargetDescriptor Inno.Build.BrowserWasm32.BrowserWasm32BuildModule.target { get; }`](../../../platforms/BrowserWasm32/build/Inno.Build.BrowserWasm32/BrowserWasm32BuildModule.cs#L15) | Gets the explicit implemented product target, independent of the tool execution host. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-c282a3d423fc6b76"></a>[`public static class Inno.Build.BrowserWasm32.BrowserWasm32BuildModule`](../../../platforms/BrowserWasm32/build/Inno.Build.BrowserWasm32/BrowserWasm32BuildModule.cs#L10) | Owns the implemented Browser target facts and complete publication contribution. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Build.BrowserWasm32.BrowserWasm32GameBuildTarget`

<!-- api: T:Inno.Build.BrowserWasm32.BrowserWasm32GameBuildTarget 2d440c808a0a093bdc1611e6e2301a80d0e00788534c737dc5b785a121f4704e -->
<!-- api: M:Inno.Build.BrowserWasm32.BrowserWasm32GameBuildTarget.#ctor(Inno.Build.IPlayerSupportPackValidator) 9adebbeeff2538cdd45c120c0e82decf2af84c7013d6ec6ff9ca50f8c3d37ca0 -->
<!-- api: M:Inno.Build.BrowserWasm32.BrowserWasm32GameBuildTarget.Validate(System.String) 8a632752e0f154bbde4f8eff0943baa404c469b7167791137f836ea93081fab3 -->
<!-- api: P:Inno.Build.BrowserWasm32.BrowserWasm32GameBuildTarget.id 052b2c508edbc4d463315a2164a651d78b2e9a633cfcfaa1d15308f546a28fc3 -->
<!-- api: P:Inno.Build.BrowserWasm32.BrowserWasm32GameBuildTarget.defaultManagedDeployment def66f8920849fad9e67a7781c684c93d75d86631065a73393359d96c42387e4 -->
<!-- api: P:Inno.Build.BrowserWasm32.BrowserWasm32GameBuildTarget.runtimeIdentifier a52a9365f92cfa41348d1e988144dc2f0c30ce9197539c8acee8fe64073d4d4a -->
<!-- api: P:Inno.Build.BrowserWasm32.BrowserWasm32GameBuildTarget.displayName e11c142958a7b955a0f9a2c1e6995edca2f3ceeb6f8669d7e91cbd1e49e8d7ab -->
<!-- api: M:Inno.Build.BrowserWasm32.BrowserWasm32GameBuildTarget.PackageAsync(Inno.Build.GameBuildPackageContext,System.Threading.CancellationToken) 9d23f8ba659867e4f697c454d3ef2224b9940797273f826af0dc066b9714061c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-262016404859f260"></a>[`public Inno.Build.BrowserWasm32.BrowserWasm32GameBuildTarget.BrowserWasm32GameBuildTarget(Inno.Build.IPlayerSupportPackValidator supportPackValidator)`](../../../platforms/BrowserWasm32/build/Inno.Build.BrowserWasm32/Targets/BrowserWasm32GameBuildTarget.cs#L26) | Creates a site packager bound to the selected native closure validator.<br>`supportPackValidator`：The borrowed validator for the distribution's browser Support Pack. | 约束由所属类型与本页生命周期定义。 | `System.ArgumentNullException`：The validator is null. |
| <a id="api-9e9b191942d738da"></a>[`public System.Threading.Tasks.ValueTask<string> Inno.Build.BrowserWasm32.BrowserWasm32GameBuildTarget.PackageAsync(Inno.Build.GameBuildPackageContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../platforms/BrowserWasm32/build/Inno.Build.BrowserWasm32/Targets/BrowserWasm32GameBuildTarget.cs#L73) | Composes an independently hostable site from a verified browser Support Pack and content pack.<br>返回：The staged static-site directory.<br>`context`：The verified runtime and packaged project content.<br>`cancellationToken`：Cancels copying before the build pipeline commits the site. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-44d1ded00b682cf1"></a>[`public void Inno.Build.BrowserWasm32.BrowserWasm32GameBuildTarget.Validate(string directory)`](../../../platforms/BrowserWasm32/build/Inno.Build.BrowserWasm32/Targets/BrowserWasm32GameBuildTarget.cs#L41) | Validates the target closure before runtime script compilation.<br>`directory`：The Support Pack directory selected by the build catalog. | 约束由所属类型与本页生命周期定义。 | `System.IO.InvalidDataException`：The required platform inputs are incomplete or incompatible. |
| <a id="api-50f82831dd58db8b"></a>[`public Inno.Build.Managed.ManagedDeploymentId Inno.Build.BrowserWasm32.BrowserWasm32GameBuildTarget.defaultManagedDeployment { get; }`](../../../platforms/BrowserWasm32/build/Inno.Build.BrowserWasm32/Targets/BrowserWasm32GameBuildTarget.cs#L49) | Gets the managed publisher selected when the profile does not specify an override. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-369036d99455cc65"></a>[`public string Inno.Build.BrowserWasm32.BrowserWasm32GameBuildTarget.displayName { get; }`](../../../platforms/BrowserWasm32/build/Inno.Build.BrowserWasm32/Targets/BrowserWasm32GameBuildTarget.cs#L57) | Gets the name shown by authoring hosts. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-3b74c9b984dbabda"></a>[`public Inno.Build.BuildTargetId Inno.Build.BrowserWasm32.BrowserWasm32GameBuildTarget.id { get; }`](../../../platforms/BrowserWasm32/build/Inno.Build.BrowserWasm32/Targets/BrowserWasm32GameBuildTarget.cs#L46) | Gets the browser WebAssembly target identity. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-83e68237a0dadb1c"></a>[`public string Inno.Build.BrowserWasm32.BrowserWasm32GameBuildTarget.runtimeIdentifier { get; }`](../../../platforms/BrowserWasm32/build/Inno.Build.BrowserWasm32/Targets/BrowserWasm32GameBuildTarget.cs#L52) | Gets the managed toolchain target identifier required by this platform composition. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-a3a75fee6ab8411b"></a>[`public sealed class Inno.Build.BrowserWasm32.BrowserWasm32GameBuildTarget : Inno.Build.IGameBuildTarget`](../../../platforms/BrowserWasm32/build/Inno.Build.BrowserWasm32/Targets/BrowserWasm32GameBuildTarget.cs#L12) | Packages a verified browser Player as an independently hostable static site. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Build.BrowserWasm32.BrowserWasm32NativeArtifacts`

<!-- api: T:Inno.Build.BrowserWasm32.BrowserWasm32NativeArtifacts 87f90a2a0dccf0612dc8743aea313c7ab6b3e4ddd867a4bed0983be8557915bc -->
<!-- api: P:Inno.Build.BrowserWasm32.BrowserWasm32NativeArtifacts.fingerprint ea972777f4ee8c6579f6d1c4f6fcb4990e17e8ff0260177c545f58d975438df4 -->
<!-- api: P:Inno.Build.BrowserWasm32.BrowserWasm32NativeArtifacts.directory 40b8dd75a4bd1c72941270b6e81c2aae3fecd4628c991e69a40ede2afc9a78f7 -->
<!-- api: P:Inno.Build.BrowserWasm32.BrowserWasm32NativeArtifacts.bindingSelectionPath ab4902b9206b58500817348b34038b42f39358c0683478465609b879bfb99ed0 -->
<!-- api: M:Inno.Build.BrowserWasm32.BrowserWasm32NativeArtifacts.Dispose 69e57f64796cc5938fb5e32b10fcf8152eef6e7fc954a84a29a63669ef526aa7 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3402f6bf6f9d637a"></a>[`public void Inno.Build.BrowserWasm32.BrowserWasm32NativeArtifacts.Dispose()`](../../../platforms/BrowserWasm32/build/Inno.Build.BrowserWasm32/BrowserWasm32NativeArtifacts.cs#L44) | Releases the complete archive closure after managed compilation and copying finish. Published paths must not be accessed afterwards; repeated disposal has no effect. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-a45bb2ecd894b995"></a>[`public string Inno.Build.BrowserWasm32.BrowserWasm32NativeArtifacts.bindingSelectionPath { get; }`](../../../platforms/BrowserWasm32/build/Inno.Build.BrowserWasm32/BrowserWasm32NativeArtifacts.cs#L38) | Gets the immutable MSBuild selection file used to reject mismatched managed binding inputs. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-330c18c6895fc473"></a>[`public string Inno.Build.BrowserWasm32.BrowserWasm32NativeArtifacts.directory { get; }`](../../../platforms/BrowserWasm32/build/Inno.Build.BrowserWasm32/BrowserWasm32NativeArtifacts.cs#L33) | Gets the absolute immutable install root containing component archive directories. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-6ed0868b27476fd1"></a>[`public string Inno.Build.BrowserWasm32.BrowserWasm32NativeArtifacts.fingerprint { get; }`](../../../platforms/BrowserWasm32/build/Inno.Build.BrowserWasm32/BrowserWasm32NativeArtifacts.cs#L28) | Gets the identity of the SDK, source, configuration and selected binding generations. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-414699267107f016"></a>[`public sealed class Inno.Build.BrowserWasm32.BrowserWasm32NativeArtifacts : System.IDisposable`](../../../platforms/BrowserWasm32/build/Inno.Build.BrowserWasm32/BrowserWasm32NativeArtifacts.cs#L10) | Identifies the immutable native closure produced by one browser toolchain request. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Build.BrowserWasm32.BrowserWasm32PlayerSupportPackSource`

<!-- api: T:Inno.Build.BrowserWasm32.BrowserWasm32PlayerSupportPackSource 9229af74a813e8598f7471f3087f16fba6e4d802204a049e8bbf40339816d32d -->
<!-- api: M:Inno.Build.BrowserWasm32.BrowserWasm32PlayerSupportPackSource.#ctor(Inno.Build.Toolchains.BuildHostDescriptor,Inno.Build.BuildTargetId,Inno.Build.Toolchains.ProductNativeBuildPlan,Inno.Build.Toolchains.INativeBindingGenerator,Inno.Build.IPlayerSupportPackValidator) 448ad947318b6f8c232edb1745ae6578c41a3989ab1926c930d9be627a229455 -->
<!-- api: P:Inno.Build.BrowserWasm32.BrowserWasm32PlayerSupportPackSource.target cb8bcca1f7862b8027ac3313cdca8488113cfe1d9b212aa39e4319b4a6f95dfa -->
<!-- api: M:Inno.Build.BrowserWasm32.BrowserWasm32PlayerSupportPackSource.CreatePlanAsync(Inno.Build.SupportPacks.PlayerSupportPackPlanningContext,System.Threading.CancellationToken) f99770c605bc4080ee79bcf37c4516f39e8a99ea9ee953cb0570c3b60574966e -->
<!-- api: M:Inno.Build.BrowserWasm32.BrowserWasm32PlayerSupportPackSource.Validate(System.String) 4fa0dd334418d5049eb10e959addfc3d7fe153babd07c82bc33519018a5f2e49 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3aff534332d70bcd"></a>[`public Inno.Build.BrowserWasm32.BrowserWasm32PlayerSupportPackSource.BrowserWasm32PlayerSupportPackSource(Inno.Build.Toolchains.BuildHostDescriptor host, Inno.Build.BuildTargetId toolsTarget, Inno.Build.Toolchains.ProductNativeBuildPlan nativePlan, Inno.Build.Toolchains.INativeBindingGenerator bindingGenerator, Inno.Build.IPlayerSupportPackValidator supportPackValidator)`](../../../platforms/BrowserWasm32/build/Inno.Build.BrowserWasm32/SupportPacks/BrowserWasm32PlayerSupportPackSource.cs#L48) | Captures independent Browser product and offline compiler selections.<br>`host`：The execution machine permitted to run the selected SDK.<br>`toolsTarget`：The explicit target of offline tools used while preparing the product.<br>`nativePlan`：The borrowed immutable Player component closure.<br>`bindingGenerator`：The borrowed generation service used by the Browser component batch.<br>`supportPackValidator`：The borrowed platform and selected static-closure validator. | 约束由所属类型与本页生命周期定义。 | `System.ArgumentNullException`：The host or native plan is null.<br>`System.ArgumentException`：The offline tool target is unassigned. |
| <a id="api-6f5f0b89391913b0"></a>[`public System.Threading.Tasks.ValueTask<Inno.Build.SupportPacks.PlayerSupportPackPlan> Inno.Build.BrowserWasm32.BrowserWasm32PlayerSupportPackSource.CreatePlanAsync(Inno.Build.SupportPacks.PlayerSupportPackPlanningContext context, System.Threading.CancellationToken cancellationToken)`](../../../platforms/BrowserWasm32/build/Inno.Build.BrowserWasm32/SupportPacks/BrowserWasm32PlayerSupportPackSource.cs#L70) | Validates discovery inputs and freezes a plan before publication output exists.<br>返回：A frozen plan with no running work; missing tools and unsupported hosts fail here.<br>`context`：The source checkout and host used for read-only SDK discovery.<br>`cancellationToken`：Cancels preparation before installation. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-99b7550f9bc8bf9b"></a>[`public void Inno.Build.BrowserWasm32.BrowserWasm32PlayerSupportPackSource.Validate(string directory)`](../../../platforms/BrowserWasm32/build/Inno.Build.BrowserWasm32/SupportPacks/BrowserWasm32PlayerSupportPackSource.cs#L141) | Rejects an incomplete or incompatible platform closure.<br>`directory`：The prepared or installed Support Pack directory. | 约束由所属类型与本页生命周期定义。 | `System.IO.InvalidDataException`：A required platform input is absent or incompatible. |
| <a id="api-bcdd8d892e52a109"></a>[`public Inno.Build.BuildTargetId Inno.Build.BrowserWasm32.BrowserWasm32PlayerSupportPackSource.target { get; }`](../../../platforms/BrowserWasm32/build/Inno.Build.BrowserWasm32/SupportPacks/BrowserWasm32PlayerSupportPackSource.cs#L67) | Gets the stable target identity implemented by this source. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-d5b35760f336625e"></a>[`public sealed class Inno.Build.BrowserWasm32.BrowserWasm32PlayerSupportPackSource : Inno.Build.SupportPacks.IPlayerSupportPackSource`](../../../platforms/BrowserWasm32/build/Inno.Build.BrowserWasm32/SupportPacks/BrowserWasm32PlayerSupportPackSource.cs#L16) | Prepares Browser runtime sources and one frozen native closure for isolated publication. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Build.BrowserWasm32.BrowserWasm32SupportPackValidator`

<!-- api: T:Inno.Build.BrowserWasm32.BrowserWasm32SupportPackValidator e52f7ce1f37f2f27c5648ef108a55b0ecc9e084680d0e5c90a0a6d7194f99b61 -->
<!-- api: M:Inno.Build.BrowserWasm32.BrowserWasm32SupportPackValidator.Validate(System.String) 9039f19f7ad5490e4a0a1cf896b3e926854b453faf32f87608920393393f11a7 -->
<!-- api: M:Inno.Build.BrowserWasm32.BrowserWasm32SupportPackValidator.#ctor a72ad66b40755ba321ef9c63023ac7be5e4f1dba6c5ea14195323333fcacf0e3 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d9a7f43c45f02246"></a>[`public Inno.Build.BrowserWasm32.BrowserWasm32SupportPackValidator.BrowserWasm32SupportPackValidator()`](../../../platforms/BrowserWasm32/build/Inno.Build.BrowserWasm32/SupportPacks/BrowserWasm32SupportPackValidator.cs#L14) | C# 生成的声明；状态与值语义遵循所属类型契约。 | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-f5c6244678231dd7"></a>[`public void Inno.Build.BrowserWasm32.BrowserWasm32SupportPackValidator.Validate(string directory)`](../../../platforms/BrowserWasm32/build/Inno.Build.BrowserWasm32/SupportPacks/BrowserWasm32SupportPackValidator.cs#L25) | Rejects incomplete or foreign platform inputs in the supplied Support Pack.<br>`directory`：The isolated or installed target directory to validate. | 约束由所属类型与本页生命周期定义。 | `System.IO.InvalidDataException`：A required input is absent or the closure contains a foreign native binary. |
| <a id="api-7b96022fbb7f3477"></a>[`public sealed class Inno.Build.BrowserWasm32.BrowserWasm32SupportPackValidator : Inno.Build.IPlayerSupportPackValidator`](../../../platforms/BrowserWasm32/build/Inno.Build.BrowserWasm32/SupportPacks/BrowserWasm32SupportPackValidator.cs#L14) | Validates the platform runtime or linker closure before compilation and installation. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Build.BrowserWasm32.BrowserWasm32Toolchain`

<!-- api: T:Inno.Build.BrowserWasm32.BrowserWasm32Toolchain 13a11a9fc3c370260aa446231d808192eecaed6c1afbc752e37c911e8b98a801 -->
<!-- api: P:Inno.Build.BrowserWasm32.BrowserWasm32Toolchain.componentDescriptor 641f495f3b9eb97e16fe11f645364116105e3100894afec9848416fb3d503476 -->
<!-- api: M:Inno.Build.BrowserWasm32.BrowserWasm32Toolchain.BuildAsync(Inno.Build.Toolchains.NativeBuildContext,Inno.Build.Toolchains.ProductNativeBuildPlan,System.Threading.CancellationToken) 6da743dfbe5ff342bbdcc044d6be715d5d7cb8763b6ede9a2cedfa4c3784bcd6 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c87a3feb51db1752"></a>[`public static System.Threading.Tasks.Task<Inno.Build.BrowserWasm32.BrowserWasm32NativeArtifacts> Inno.Build.BrowserWasm32.BrowserWasm32Toolchain.BuildAsync(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.ProductNativeBuildPlan plan, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../platforms/BrowserWasm32/build/Inno.Build.BrowserWasm32/BrowserWasm32Toolchain.cs#L49) | Publishes one exact static closure without discovering components or reselecting tools.<br>返回：The immutable complete native closure and corresponding managed binding selection.<br>`context`：The owned checkout and previously resolved Emscripten selection.<br>`plan`：The frozen product component graph with backend-owned static definitions.<br>`cancellationToken`：Cancels generation and drains active compilation before returning. | 约束由所属类型与本页生命周期定义。 | `System.InvalidOperationException`：The tools, target or static component capability are incompatible.<br>`System.IO.InvalidDataException`：A component produces an incomplete or foreign archive closure.<br>`System.OperationCanceledException`：Preparation was canceled before publication. |
| <a id="api-88fecf1825a890b9"></a>[`public static Inno.Build.Toolchains.NativeComponentDescriptor Inno.Build.BrowserWasm32.BrowserWasm32Toolchain.componentDescriptor { get; }`](../../../platforms/BrowserWasm32/build/Inno.Build.BrowserWasm32/BrowserWasm32Toolchain.cs#L20) | Gets the sole owner of Browser aggregation, ABI constraints and SDK link support. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-6c65e3390b7581a9"></a>[`public static class Inno.Build.BrowserWasm32.BrowserWasm32Toolchain`](../../../platforms/BrowserWasm32/build/Inno.Build.BrowserWasm32/BrowserWasm32Toolchain.cs#L15) | Aggregates explicitly selected backend static recipes using a frozen Emscripten SDK. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Build.BrowserWasm32.EmscriptenNativeToolchainProvider`

<!-- api: T:Inno.Build.BrowserWasm32.EmscriptenNativeToolchainProvider 978bf965c4378a3c2f5478c2817055fa856d998d7a420ff2eaee9a9f90588306 -->
<!-- api: M:Inno.Build.BrowserWasm32.EmscriptenNativeToolchainProvider.#ctor(System.String) 3cd3343acdeb4e3a5cb214b4ce4b4eba5c5d27220b90a555ea54d009b0502f42 -->
<!-- api: M:Inno.Build.BrowserWasm32.EmscriptenNativeToolchainProvider.ResolveAsync(Inno.Build.Toolchains.NativeBuildContext,Inno.Build.Toolchains.BuildHostDescriptor,System.String,System.Threading.CancellationToken) dc82888c43fafdf932cb555b78503b47fc5ec360cc8770011d3624f61979fd91 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-670b71f38e8a0d81"></a>[`public Inno.Build.BrowserWasm32.EmscriptenNativeToolchainProvider.EmscriptenNativeToolchainProvider(string dotnetHost)`](../../../platforms/BrowserWasm32/build/Inno.Build.BrowserWasm32/Toolchain/EmscriptenNativeToolchainProvider.cs#L26) | Captures the SDK executable selected by the build host without probing installed SDK versions.<br>`dotnetHost`：The host-selected SDK executable or explicit command. | 约束由所属类型与本页生命周期定义。 | `System.ArgumentException`：The SDK command is blank. |
| <a id="api-9dd16345cb14e2df"></a>[`public System.Threading.Tasks.ValueTask<Inno.Build.Toolchains.NativeToolchainSelection> Inno.Build.BrowserWasm32.EmscriptenNativeToolchainProvider.ResolveAsync(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.BuildHostDescriptor host, string targetId, System.Threading.CancellationToken cancellationToken)`](../../../platforms/BrowserWasm32/build/Inno.Build.BrowserWasm32/Toolchain/EmscriptenNativeToolchainProvider.cs#L33) | Freezes compiler tools, SDK inputs and child environment before a native operation starts.<br>返回：A complete immutable tool selection for the requested host and target.<br>`context`：The checkout and configuration owned by the requesting operation.<br>`host`：The explicitly declared execution host, independent of the target.<br>`targetId`：The exact requested target; unsupported combinations must fail.<br>`cancellationToken`：Cancels SDK discovery before any product staging. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-55638965ef22457a"></a>[`public sealed class Inno.Build.BrowserWasm32.EmscriptenNativeToolchainProvider : Inno.Build.Toolchains.INativeToolchainProvider`](../../../platforms/BrowserWasm32/build/Inno.Build.BrowserWasm32/Toolchain/EmscriptenNativeToolchainProvider.cs#L13) | Selects Emscripten from the Browser product's actual SDK workload for an explicit wasm32 target. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Build.BrowserWasm32.EmscriptenToolchainResolver`

<!-- api: T:Inno.Build.BrowserWasm32.EmscriptenToolchainResolver bc19cecc7f9ecf813d9db4766afbd807d2ba4b95d50d1b628350d219e05dc7e9 -->
<!-- api: M:Inno.Build.BrowserWasm32.EmscriptenToolchainResolver.ResolveAsync(System.String,System.String,System.Threading.CancellationToken) a0c9bec2c903edd13b521c7f2f5c36f7626cfc73f2364cc48b4549cfeb25a730 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-fc6d95fed43b229e"></a>[`public static System.Threading.Tasks.Task<System.Collections.Generic.IReadOnlyDictionary<string, string>> Inno.Build.BrowserWasm32.EmscriptenToolchainResolver.ResolveAsync(string dotnetHost, string projectPath, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../platforms/BrowserWasm32/build/Inno.Build.BrowserWasm32/EmscriptenToolchainResolver.cs#L50) | Resolves the native toolchain actually selected by MSBuild for one browser project.<br>返回：Variables for native builds and binding generation, without changing the parent process.<br>`dotnetHost`：The SDK host executable, either an absolute path or a name available on PATH.<br>`projectPath`：The browser project whose target framework and SDK determine workload selection.<br>`cancellationToken`：Cancels the hidden SDK query and waits for its exit. | Compiler worker budgets remain inherited from the host environment or tool defaults. Runtime threading policy does not constrain build-time parallelism. | `System.IO.FileNotFoundException`：The SDK host or a required Emscripten workload component is missing.<br>`System.ArgumentException`：The SDK host or project path is empty.<br>`System.IO.DirectoryNotFoundException`：A selected SDK directory or sysroot is missing.<br>`System.InvalidOperationException`：Project evaluation fails or does not select a complete native toolchain.<br>`System.OperationCanceledException`：The operation was canceled. |
| <a id="api-66a465e498bb2235"></a>[`public static class Inno.Build.BrowserWasm32.EmscriptenToolchainResolver`](../../../platforms/BrowserWasm32/build/Inno.Build.BrowserWasm32/EmscriptenToolchainResolver.cs#L14) | Resolves Emscripten from the workload selected by one application project. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

## 项目依赖

- [Inno.Build](../../build/Inno.Build.md)：公开引用边界由实际签名核对。
- [Inno.Build.Managed](../../build/Inno.Build.Managed.md)：公开引用边界由实际签名核对。
- [Inno.Build.Toolchains](../../build/Inno.Build.Toolchains.md)：公开引用边界由实际签名核对。
- [Inno.Core.IO](../../core/Inno.Core.IO.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Composition](../../build/Inno.Build.Composition.md)：公开引用边界由实际签名核对。
- [Inno.Build.SupportPacks.Core](../../build/Inno.Build.SupportPacks.Core.md)：公开引用边界由实际签名核对。
