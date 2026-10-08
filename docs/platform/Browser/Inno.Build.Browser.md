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

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Build.Browser.BrowserBuildModule`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Browser.BrowserBuildModule`](../../../platforms/Browser/build/Inno.Build.Browser/BrowserBuildModule.cs#L10) | Owns the implemented Browser target facts and complete publication contribution. |
| [`static Inno.Build.Composition.BuildPlatformContribution Inno.Build.Browser.BrowserBuildModule.CreateContribution(Inno.Build.Composition.BuildCompositionContext context, Inno.Build.Toolchains.ProductNativeBuildPlan nativePlan)`](../../../platforms/Browser/build/Inno.Build.Browser/BrowserBuildModule.cs#L30) | Contributes the platform's target, product inputs and SDK resolver as one registration. |
| [`static Inno.Build.PlatformTargetDescriptor Inno.Build.Browser.BrowserBuildModule.target`](../../../platforms/Browser/build/Inno.Build.Browser/BrowserBuildModule.cs#L15) | Gets the explicit implemented product target, independent of the tool execution host. |

### `Inno.Build.Browser.BrowserNativeArtifacts`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Browser.BrowserNativeArtifacts`](../../../platforms/Browser/build/Inno.Build.Browser/BrowserNativeArtifacts.cs#L8) | Identifies the immutable native closure produced by one browser toolchain request. |
| [`string Inno.Build.Browser.BrowserNativeArtifacts.bindingSelectionPath`](../../../platforms/Browser/build/Inno.Build.Browser/BrowserNativeArtifacts.cs#L31) | Gets the immutable MSBuild selection file used to reject mismatched managed binding inputs. |
| [`string Inno.Build.Browser.BrowserNativeArtifacts.directory`](../../../platforms/Browser/build/Inno.Build.Browser/BrowserNativeArtifacts.cs#L26) | Gets the absolute immutable install root containing component archive directories. |
| [`string Inno.Build.Browser.BrowserNativeArtifacts.fingerprint`](../../../platforms/Browser/build/Inno.Build.Browser/BrowserNativeArtifacts.cs#L21) | Gets the identity of the SDK, source, configuration and selected binding generations. |

### `Inno.Build.Browser.BrowserPlayerSupportPackSource`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Browser.BrowserPlayerSupportPackSource`](../../../platforms/Browser/build/Inno.Build.Browser/SupportPacks/BrowserPlayerSupportPackSource.cs#L14) | Prepares Browser runtime sources and one frozen native closure for isolated publication. |
| [`Inno.Build.Browser.BrowserPlayerSupportPackSource.BrowserPlayerSupportPackSource(Inno.Build.Toolchains.BuildHostDescriptor host, Inno.Build.BuildTargetId toolsTarget, Inno.Build.Toolchains.ProductNativeBuildPlan nativePlan)`](../../../platforms/Browser/build/Inno.Build.Browser/SupportPacks/BrowserPlayerSupportPackSource.cs#L38) | Captures independent Browser product and offline compiler selections. |
| [`Inno.Build.BuildTargetId Inno.Build.Browser.BrowserPlayerSupportPackSource.target`](../../../platforms/Browser/build/Inno.Build.Browser/SupportPacks/BrowserPlayerSupportPackSource.cs#L52) | See the implemented contract. |
| [`System.Threading.Tasks.ValueTask<Inno.Build.SupportPacks.PlayerSupportPackPlan> Inno.Build.Browser.BrowserPlayerSupportPackSource.CreatePlanAsync(Inno.Build.SupportPacks.PlayerSupportPackPlanningContext context, System.Threading.CancellationToken cancellationToken)`](../../../platforms/Browser/build/Inno.Build.Browser/SupportPacks/BrowserPlayerSupportPackSource.cs#L55) | See the implemented contract. |
| [`void Inno.Build.Browser.BrowserPlayerSupportPackSource.Validate(string directory)`](../../../platforms/Browser/build/Inno.Build.Browser/SupportPacks/BrowserPlayerSupportPackSource.cs#L104) | See the implemented contract. |

### `Inno.Build.Browser.BrowserSupportPackValidator`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Browser.BrowserSupportPackValidator`](../../../platforms/Browser/build/Inno.Build.Browser/SupportPacks/BrowserSupportPackValidator.cs#L12) | Validates the platform runtime or linker closure before compilation and installation. |
| [`Inno.Build.Browser.BrowserSupportPackValidator.BrowserSupportPackValidator(Inno.Build.Toolchains.ProductNativeBuildPlan nativePlan)`](../../../platforms/Browser/build/Inno.Build.Browser/SupportPacks/BrowserSupportPackValidator.cs#L29) | Captures the selected static closure so validation and publication use the same backend declarations. |
| [`void Inno.Build.Browser.BrowserSupportPackValidator.Validate(string directory)`](../../../platforms/Browser/build/Inno.Build.Browser/SupportPacks/BrowserSupportPackValidator.cs#L49) | Rejects incomplete or foreign platform inputs in the supplied Support Pack. |

### `Inno.Build.Browser.BrowserToolchain`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Browser.BrowserToolchain`](../../../platforms/Browser/build/Inno.Build.Browser/BrowserToolchain.cs#L16) | Aggregates explicitly selected backend static recipes using a frozen Emscripten SDK. |
| [`static Inno.Build.Toolchains.NativeComponentDescriptor Inno.Build.Browser.BrowserToolchain.componentDescriptor`](../../../platforms/Browser/build/Inno.Build.Browser/BrowserToolchain.cs#L21) | Gets the sole owner of Browser aggregation, ABI constraints and SDK link support. |
| [`static System.Threading.Tasks.Task<Inno.Build.Browser.BrowserNativeArtifacts> Inno.Build.Browser.BrowserToolchain.BuildAsync(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.ProductNativeBuildPlan plan, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../platforms/Browser/build/Inno.Build.Browser/BrowserToolchain.cs#L50) | Publishes one exact static closure without discovering components or reselecting tools. |

### `Inno.Build.Browser.BrowserWasm32GameBuildTarget`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Browser.BrowserWasm32GameBuildTarget`](../../../platforms/Browser/build/Inno.Build.Browser/Targets/BrowserWasm32GameBuildTarget.cs#L12) | Packages a verified browser Player as an independently hostable static site. |
| [`Inno.Build.Browser.BrowserWasm32GameBuildTarget.BrowserWasm32GameBuildTarget(Inno.Build.IPlayerSupportPackValidator supportPackValidator)`](../../../platforms/Browser/build/Inno.Build.Browser/Targets/BrowserWasm32GameBuildTarget.cs#L26) | Creates a site packager bound to the selected native closure validator. |
| [`Inno.Build.BuildTargetId Inno.Build.Browser.BrowserWasm32GameBuildTarget.id`](../../../platforms/Browser/build/Inno.Build.Browser/Targets/BrowserWasm32GameBuildTarget.cs#L46) | Gets the browser WebAssembly target identity. |
| [`Inno.Build.Managed.ManagedDeploymentId Inno.Build.Browser.BrowserWasm32GameBuildTarget.defaultManagedDeployment`](../../../platforms/Browser/build/Inno.Build.Browser/Targets/BrowserWasm32GameBuildTarget.cs#L49) | See the implemented contract. |
| [`System.Threading.Tasks.ValueTask<string> Inno.Build.Browser.BrowserWasm32GameBuildTarget.PackageAsync(Inno.Build.GameBuildPackageContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../platforms/Browser/build/Inno.Build.Browser/Targets/BrowserWasm32GameBuildTarget.cs#L73) | Composes an independently hostable site from a verified browser Support Pack and content pack. |
| [`string Inno.Build.Browser.BrowserWasm32GameBuildTarget.displayName`](../../../platforms/Browser/build/Inno.Build.Browser/Targets/BrowserWasm32GameBuildTarget.cs#L57) | Gets the name shown by authoring hosts. |
| [`string Inno.Build.Browser.BrowserWasm32GameBuildTarget.runtimeIdentifier`](../../../platforms/Browser/build/Inno.Build.Browser/Targets/BrowserWasm32GameBuildTarget.cs#L52) | See the implemented contract. |
| [`void Inno.Build.Browser.BrowserWasm32GameBuildTarget.Validate(string directory)`](../../../platforms/Browser/build/Inno.Build.Browser/Targets/BrowserWasm32GameBuildTarget.cs#L41) | Validates the target closure before runtime script compilation. |

### `Inno.Build.Browser.EmscriptenNativeToolchainProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Browser.EmscriptenNativeToolchainProvider`](../../../platforms/Browser/build/Inno.Build.Browser/Toolchain/EmscriptenNativeToolchainProvider.cs#L13) | Selects Emscripten from the Browser product's actual SDK workload for an explicit wasm32 target. |
| [`Inno.Build.Browser.EmscriptenNativeToolchainProvider.EmscriptenNativeToolchainProvider(string dotnetHost)`](../../../platforms/Browser/build/Inno.Build.Browser/Toolchain/EmscriptenNativeToolchainProvider.cs#L26) | Captures the SDK executable selected by the build host without probing installed SDK versions. |
| [`System.Threading.Tasks.ValueTask<Inno.Build.Toolchains.NativeToolchainSelection> Inno.Build.Browser.EmscriptenNativeToolchainProvider.ResolveAsync(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.BuildHostDescriptor host, string targetId, System.Threading.CancellationToken cancellationToken)`](../../../platforms/Browser/build/Inno.Build.Browser/Toolchain/EmscriptenNativeToolchainProvider.cs#L33) | See the implemented contract. |

### `Inno.Build.Browser.EmscriptenToolchainResolver`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Browser.EmscriptenToolchainResolver`](../../../platforms/Browser/build/Inno.Build.Browser/EmscriptenToolchainResolver.cs#L14) | Resolves Emscripten from the workload selected by one application project. |
| [`static System.Threading.Tasks.Task<System.Collections.Generic.IReadOnlyDictionary<string, string>> Inno.Build.Browser.EmscriptenToolchainResolver.ResolveAsync(string dotnetHost, string projectPath, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../platforms/Browser/build/Inno.Build.Browser/EmscriptenToolchainResolver.cs#L50) | Resolves the native toolchain actually selected by MSBuild for one browser project. |

## 项目依赖

- [Inno.Build](../../build/Inno.Build.md)：公开引用边界由实际签名核对。
- [Inno.Build.Managed](../../build/Inno.Build.Managed.md)：公开引用边界由实际签名核对。
- [Inno.Build.Toolchains](../../build/Inno.Build.Toolchains.md)：公开引用边界由实际签名核对。
- [Inno.Core.IO](../../core/Inno.Core.IO.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Composition](../../build/Inno.Build.Composition.md)：公开引用边界由实际签名核对。
- [Inno.Build.SupportPacks.Core](../../build/Inno.Build.SupportPacks.Core.md)：公开引用边界由实际签名核对。
