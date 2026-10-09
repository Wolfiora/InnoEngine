# Inno.Build.Toolchains

[分类索引](README.md) · [Wiki 首页](../README.md) · [平台归属与扩展](../architecture/PLATFORM_EXTENSION_GUIDE.md)

## 职责与边界

平台中立的 SDK 选择契约、工具执行、组件描述、内容指纹、Native 发布与产品部署机制。不探测当前 OS 来选择产品目标。

## 组合、生命周期与扩展

`BuildHostDescriptor` 是实际执行工具的宿主；`NativeToolchainSelection` 是已解析的明确目标、绝对工具、SDK 输入、环境及参数。平台 provider 负责 SDK 选择；共同机制只执行冻结选择。

`NativeComponentDescriptor` 显式指定 Native、Toolchain、binding 配置和可选静态构建定义的唯一 owner。不同 checkout 的逻辑位置与物理读取分开。`NativeBuildContext.GetNativeBuildRoot` 不按程序集名称猜目录。

`ProductNativeBuildPlan.BuildAsync` 在所有步骤开始前批量准备同一目标的 bindings，再按明确依赖执行组件 recipe。`NativeBindingPreparation` 复用 BGCS 库的 MSBuild 集成，结果按目标/指纹隔离。缺少 profile、工具或 SDK 在候选发布前明确失败。

`NativeArtifactPublisher` 使用完整内容哈希、operation-owned snapshot、写 lease、再次稳定性验证和原子提交。工具、SDK、参数、生成身份、真实 source/executor closure 共同参与指纹。热命中仍校验产物与输入内容，不依赖 mtime。

`NativeBuildRecipe.CreateForComponent` 默认包含独立组件 owner 的全部实现；多职责平台模块通过 `implementationPaths` 显式声明实际原生执行源码。Browser 聚合只包含聚合实现与产物描述，Support Pack、打包和 Target 代码不参与原生归档指纹。共用 executor、组件 recipe、SDK、生成桥与参数继续参与失效；缩小无关范围不削弱发布前稳定性校验。

`ProductNativeDeployment` 从 plan 的明确布局生成 exact deployment；相同内容不替换加载中的 DLL。失败/取消保留旧完整部署，提交后清理失败独立报告。工具工作目录 alias 和文件权限属于内部执行机制，不能决定输出目标。

托管子进程通过 `DotNetSdkEnvironment.Create` 取得只作用于子进程的环境。它保留所选 Native SDK 环境，移除父 IDE/MSBuild 注入的 SDK resolver、MSBuild 程序集与扩展根，使所选 dotnet 根据被调用工程的 `global.json` 解析 SDK。共同进程执行器以 `null` 表示移除继承变量，以空字符串表示显式空值；不修改父进程，也不要求所有消费者锁定同一个 SDK。

`DotNetSdkResolver` 先经现有 `ResolveExecutable` 将调用者选择的 host 冻结为绝对路径，再在原工程目录解析一次 SDK，记录实际 Base Path 与 managed CLI 入口 `DotNetSdkDescriptor.cliPath`。发布执行使用相同的绝对 host 和冻结入口，不重新查询 PATH，也不从临时 Windows alias 的父目录选择 SDK。SDK 信息查询只在子进程中固定英文输出，以便严格读取所选身份和位置，不能从宿主安装列表猜最高版本。当前所选 host 或 SDK 入口缺失时明确失败，不复制或改写工程及其父目录的 `global.json`。

Inno 引用独立 BGCS.Runtime 源码时，产物写入当前 checkout 的 `artifacts/managed/interop`，按 SDK、明确目标、Native ABI 指纹、RID、框架与实际 symbols/AOT/trim 属性隔离。没有修改 BGCS 工程，也不将 Inno 的编译状态写入该仓库的普通 bin/obj。SDK 自身继续在隔离根内区分配置，两个产品可以同时准备不同的调试符号配置。

共同 MSBuild 规则同时处理手写直接引用和 SDK 在执行阶段自动加入的间接引用。后者在 `IncludeTransitiveProjectReferences` 完成后获得相同产品目标、Shader 配置与 interop 隔离 metadata，不把属性复制要求分散到各消费者项目；真实 SDK 求值与普通产品 Build 都是验收入口。Analyzer 引用不传递产品属性，并明确移除父项目已经继承的全局产品/发布/Native/输出属性，始终作为宿主编译工具。

当前开发闭包明确消费 `BindGenRoot` 所选仓库的 BGCS.Runtime 项目；不存在自动选择旧 NuGet Runtime 的分支。缺少所选生成器或 Runtime 项目会明确失败。BGCS 自身的 NuGet 发布与独立消费者验收继续由其仓库维护。

共同构建使用[静态项目图还原](https://learn.microsoft.com/en-us/nuget/reference/msbuild-targets#restore-with-static-graph-evaluation)。BGCS.Runtime 的包依赖图是中立的：按 SDK、RID、框架共享还原目录；Native 目标、ABI 和调试符号不改变该库的包依赖图。编译与 symbols 的中间态仍按完整消费配置分开。NuGet 按项目身份合并图节点，因此不把每个编译配置当成不同包项目。

外部项目可以使用普通、基于路径的 restore；该遍历不保留每条 ProjectReference 的 AdditionalProperties。共同 targets 在解析引用前，通过同一实际引用及其 metadata 执行 BGCS.Runtime 的隔离 restore，保证随后编译使用匹配的 assets。MSBuild 对相同项目/全局属性/target 复用一次构建闭包内的结果，不在各产品中重复设置准备流程。Design-time 与明确禁止构建引用的操作不执行此准备。没有复制 assets、修改 BGCS 项目或要求外部消费者更换 SDK；仓库外 fixture 使用普通 restore、新隔离目录和真实间接引用构建验证该边界。

普通 IDE 产品输出保留产品项目的 `bin/<target>/<configuration>` 与 `obj/<target>`，方便启动和调试；它们是可替换的 IDE 输出，不作为不可变缓存。Native、bindings、Support Pack 与按游戏闭包发布的托管中间态采用明确目标和指纹隔离，部署前校验匹配闭包。最终游戏目录由 BuildProfile 指定。

```csharp
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Inno.Build.Toolchains;

static Task<IReadOnlyList<NativeBuildProduct>> Prepare(
    ProductNativeBuildPlan plan,
    NativeBuildContext selected,
    CancellationToken cancellation
) {
    return plan.BuildAsync(selected, cancellation);
}
```

扩展 SDK 实现 `INativeToolchainProvider`，组件提供自己的 descriptor、recipe 与 CMake 定义；具体发行负责选择。没有固定内置组件列表或额外生产 Program。验证包括内容篡改、并发、取消、冷 CMake、绑定/export 一致性及产品热构建测量。

## 显式组件配置

NativeBuildContext.WithComponentOptions 固定对应 NativeComponentDescriptor 与 NativeComponentBuildOptions；ProductNativeBuildStep 必须提供它。libraryKind、有序 CMake definitions、输入文件 bytes 与目标、SDK、binding generation 共同参与 recipe identity。NativeStaticBuildDefinition 声明准确 targetIds；当前 .a 聚合仅实现 browser-wasm，未知格式或链接请求在构建前拒绝。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Build.Toolchains.BuildArtifactCopier`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.BuildArtifactCopier`](../../build/toolchains/Inno.Build.Toolchains/BuildArtifactOptions.cs#L37) | Copies filtered native outputs into the engine's rebuildable dependency store. |
| [`static void Inno.Build.Toolchains.BuildArtifactCopier.CopyArtifacts(string buildRoot, string outputDir, string config, Inno.Build.Toolchains.BuildArtifactOptions options)`](../../build/toolchains/Inno.Build.Toolchains/BuildArtifactOptions.cs#L56) | Copies all artifacts accepted by one product policy into its output directory. |

### `Inno.Build.Toolchains.BuildArtifactManifest`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.BuildArtifactManifest`](../../build/toolchains/Inno.Build.Toolchains/BuildArtifactManifest.cs#L14) | Validates complete build outputs against their input identity and recorded file hashes. |
| [`static bool Inno.Build.Toolchains.BuildArtifactManifest.IsComplete(string directory, string fingerprint, System.Collections.Generic.IReadOnlyList<string> outputDirectories)`](../../build/toolchains/Inno.Build.Toolchains/BuildArtifactManifest.cs#L70) | Checks the identity, exact output file set and bytes before reusing a cached artifact. |
| [`static void Inno.Build.Toolchains.BuildArtifactManifest.Write(string directory, string fingerprint, System.Collections.Generic.IReadOnlyList<string> outputDirectories)`](../../build/toolchains/Inno.Build.Toolchains/BuildArtifactManifest.cs#L36) | Records a completed staging tree before its owner publishes the directory. |

### `Inno.Build.Toolchains.BuildArtifactOptions`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.BuildArtifactOptions`](../../build/toolchains/Inno.Build.Toolchains/BuildArtifactOptions.cs#L26) | Defines the deterministic filter and naming policy used to collect one native product. |

### `Inno.Build.Toolchains.BuildHostDescriptor`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.BuildHostDescriptor`](../../build/toolchains/Inno.Build.Toolchains/Native/BuildHostDescriptor.cs#L8) | Identifies the tool execution machine without selecting any product target. |
| [`Inno.Build.Toolchains.BuildHostDescriptor.BuildHostDescriptor(string system, string architecture)`](../../build/toolchains/Inno.Build.Toolchains/Native/BuildHostDescriptor.cs#L22) | Captures host facts supplied by a product or command composition boundary. |
| [`string Inno.Build.Toolchains.BuildHostDescriptor.architecture`](../../build/toolchains/Inno.Build.Toolchains/Native/BuildHostDescriptor.cs#L40) | Gets the processor architecture on which tools execute. |
| [`string Inno.Build.Toolchains.BuildHostDescriptor.system`](../../build/toolchains/Inno.Build.Toolchains/Native/BuildHostDescriptor.cs#L35) | Gets the operating system on which tools execute. |

### `Inno.Build.Toolchains.DotNetSdkDescriptor`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.DotNetSdkDescriptor`](../../build/toolchains/Inno.Build.Toolchains/Managed/DotNetSdkDescriptor.cs#L6) | Freezes the host, SDK identity and managed CLI entry selected by one managed entry project. |
| [`string Inno.Build.Toolchains.DotNetSdkDescriptor.cliPath`](../../build/toolchains/Inno.Build.Toolchains/Managed/DotNetSdkDescriptor.cs#L32) | Gets the selected SDK's managed CLI entry assembly, invoked with the recorded host without resolving another SDK from a temporary execution directory. |
| [`string Inno.Build.Toolchains.DotNetSdkDescriptor.hostPath`](../../build/toolchains/Inno.Build.Toolchains/Managed/DotNetSdkDescriptor.cs#L21) | Gets the executable used for project-scoped SDK and workload resolution. |
| [`string Inno.Build.Toolchains.DotNetSdkDescriptor.sdkIdentity`](../../build/toolchains/Inno.Build.Toolchains/Managed/DotNetSdkDescriptor.cs#L26) | Gets the exact SDK identity selected by the project's global.json resolution rules. |

### `Inno.Build.Toolchains.DotNetSdkEnvironment`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.DotNetSdkEnvironment`](../../build/toolchains/Inno.Build.Toolchains/Managed/DotNetSdkEnvironment.cs#L11) | Isolates a managed tool invocation from the invoking IDE's SDK and MSBuild process identity. |
| [`static System.Collections.Generic.IReadOnlyDictionary<string, string?> Inno.Build.Toolchains.DotNetSdkEnvironment.Create(string hostPath, System.Collections.Generic.IReadOnlyDictionary<string, string>? environment = null)`](../../build/toolchains/Inno.Build.Toolchains/Managed/DotNetSdkEnvironment.cs#L40) | Freezes child-only overrides so the selected executable resolves the project's own SDK. Null SDK overrides remove inherited variables rather than borrowing a parent's loaded MSBuild. |

### `Inno.Build.Toolchains.DotNetSdkResolver`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.DotNetSdkResolver`](../../build/toolchains/Inno.Build.Toolchains/Managed/DotNetSdkResolver.cs#L12) | Resolves the managed SDK using the prepared entry project's directory and normal global.json rules. |
| [`static System.Threading.Tasks.ValueTask<Inno.Build.Toolchains.DotNetSdkDescriptor> Inno.Build.Toolchains.DotNetSdkResolver.ResolveAsync(string hostPath, string projectPath, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/toolchains/Inno.Build.Toolchains/Managed/DotNetSdkResolver.cs#L35) | Asks the selected host to resolve its SDK from the project location without guessing installed versions. |

### `Inno.Build.Toolchains.INativeToolchainProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.INativeToolchainProvider`](../../build/toolchains/Inno.Build.Toolchains/Native/INativeToolchainProvider.cs#L9) | Resolves one platform's SDK for an explicit target on a declared tool execution host. |
| [`System.Threading.Tasks.ValueTask<Inno.Build.Toolchains.NativeToolchainSelection> Inno.Build.Toolchains.INativeToolchainProvider.ResolveAsync(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.BuildHostDescriptor host, string targetId, System.Threading.CancellationToken cancellationToken)`](../../build/toolchains/Inno.Build.Toolchains/Native/INativeToolchainProvider.cs#L29) | Freezes compiler tools, SDK inputs and child environment before a native operation starts. |

### `Inno.Build.Toolchains.NativeArtifactPublisher`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.NativeArtifactPublisher`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeArtifactPublisher.cs#L13) | Publishes complete native products by source and toolchain identity under process-shared ownership. |
| [`static System.Threading.Tasks.Task<Inno.Build.Toolchains.NativeBuildProduct> Inno.Build.Toolchains.NativeArtifactPublisher.PublishAsync(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.NativeBuildRecipe recipe, System.Func<Inno.Build.Toolchains.NativeBuildContext, string, System.Threading.CancellationToken, System.Threading.Tasks.Task> build, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeArtifactPublisher.cs#L46) | Reuses a validated product or builds an isolated candidate and publishes it after input stability checks. |

### `Inno.Build.Toolchains.NativeBindingGenerationDescriptor`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.NativeBindingGenerationDescriptor`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingGenerationDescriptor.cs#L10) | Describes the complete binding generation selected by one native build request. |
| [`required string Inno.Build.Toolchains.NativeBindingGenerationDescriptor.bindingsPath`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingGenerationDescriptor.cs#L20) | Gets the absolute managed source path selected by the component project. |
| [`required string Inno.Build.Toolchains.NativeBindingGenerationDescriptor.bridgeDirectory`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingGenerationDescriptor.cs#L25) | Gets the complete native bridge directory, or an empty string for a direct C binding. |
| [`required string Inno.Build.Toolchains.NativeBindingGenerationDescriptor.fingerprint`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingGenerationDescriptor.cs#L15) | Gets the immutable input identity assigned to this target generation. |
| [`static Inno.Build.Toolchains.NativeBindingGenerationDescriptor Inno.Build.Toolchains.NativeBindingGenerationDescriptor.Load(string path)`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingGenerationDescriptor.cs#L42) | Reads and validates the result of a completed component generation request. |
| [`void Inno.Build.Toolchains.NativeBindingGenerationDescriptor.Write(string path)`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingGenerationDescriptor.cs#L61) | Atomically writes this completed generation to a request-owned descriptor file. |

### `Inno.Build.Toolchains.NativeBindingPreparation`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.NativeBindingPreparation`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingPreparation.cs#L14) | Requests one coherent target binding closure through the shared MSBuild generator integration. |
| [`static System.Threading.Tasks.Task<System.Collections.Generic.IReadOnlyDictionary<string, Inno.Build.Toolchains.NativeBindingGenerationDescriptor>> Inno.Build.Toolchains.NativeBindingPreparation.PrepareAsync(Inno.Build.Toolchains.NativeBuildContext context, System.Collections.Generic.IReadOnlyList<Inno.Build.Toolchains.NativeComponentDescriptor> components, System.Threading.CancellationToken cancellationToken)`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingPreparation.cs#L40) | Generates or verifies target-isolated bindings for the explicitly selected component owners. |

### `Inno.Build.Toolchains.NativeBuildContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.NativeBindingGenerationDescriptor Inno.Build.Toolchains.NativeBuildContext.RequireBindings(Inno.Build.Toolchains.NativeComponentDescriptor component)`](../../build/toolchains/Inno.Build.Toolchains/NativeBuildContext.cs#L278) | Requires the target binding generation prepared for an explicitly declared Native owner. |
| [`Inno.Build.Toolchains.NativeBuildContext`](../../build/toolchains/Inno.Build.Toolchains/NativeBuildContext.cs#L12) | Identifies the checkout, configuration and frozen initial inputs owned by one native build operation. Create a new context for each operation; changes during its lifetime fail stability verification. |
| [`Inno.Build.Toolchains.NativeBuildContext Inno.Build.Toolchains.NativeBuildContext.WithBindings(System.Collections.Generic.IReadOnlyDictionary<string, Inno.Build.Toolchains.NativeBindingGenerationDescriptor> bindings)`](../../build/toolchains/Inno.Build.Toolchains/NativeBuildContext.cs#L257) | Creates a scoped operation with an immutable, explicitly prepared target binding closure. |
| [`Inno.Build.Toolchains.NativeBuildContext Inno.Build.Toolchains.NativeBuildContext.WithComponentOptions(Inno.Build.Toolchains.NativeComponentDescriptor component, Inno.Build.Toolchains.NativeComponentBuildOptions options)`](../../build/toolchains/Inno.Build.Toolchains/NativeBuildContext.cs#L219) | Scopes one component's explicit product configuration without changing the frozen SDK. |
| [`Inno.Build.Toolchains.NativeBuildContext Inno.Build.Toolchains.NativeBuildContext.WithToolchain(Inno.Build.Toolchains.NativeToolchainSelection toolchain)`](../../build/toolchains/Inno.Build.Toolchains/NativeBuildContext.cs#L186) | Attaches a provider's immutable tool selection while preserving this operation's input ownership. |
| [`Inno.Build.Toolchains.NativeBuildContext.NativeBuildContext(string engineRoot, string configuration)`](../../build/toolchains/Inno.Build.Toolchains/NativeBuildContext.cs#L38) | Creates a build context without consulting the tool assembly's checkout. |
| [`Inno.Build.Toolchains.NativeBuildStatistics Inno.Build.Toolchains.NativeBuildContext.statistics`](../../build/toolchains/Inno.Build.Toolchains/NativeBuildContext.cs#L164) | Gets actual hashing and native execution work accumulated by this operation and its scoped contexts. |
| [`Inno.Build.Toolchains.NativeComponentBuildOptions Inno.Build.Toolchains.NativeBuildContext.RequireComponentOptions(Inno.Build.Toolchains.NativeComponentDescriptor component)`](../../build/toolchains/Inno.Build.Toolchains/NativeBuildContext.cs#L240) | Requires product configuration for the exact component before its recipe or tools execute. |
| [`Inno.Build.Toolchains.NativeToolchainSelection Inno.Build.Toolchains.NativeBuildContext.RequireToolchain()`](../../build/toolchains/Inno.Build.Toolchains/NativeBuildContext.cs#L201) | Requires an explicit selection before a component can build or start a native tool. |
| [`Inno.Build.Toolchains.NativeToolchainSelection? Inno.Build.Toolchains.NativeBuildContext.toolchain`](../../build/toolchains/Inno.Build.Toolchains/NativeBuildContext.cs#L127) | Gets the explicit frozen target compiler and SDK selection, or null before composition resolves it. Browser toolchains resolve their own workload-specific SDK independently. |
| [`string Inno.Build.Toolchains.NativeBuildContext.GetNativeBuildRoot(Inno.Build.Toolchains.NativeComponentDescriptor component)`](../../build/toolchains/Inno.Build.Toolchains/NativeBuildContext.cs#L146) | Resolves native intermediates under the owning toolchain project in this checkout. |
| [`string Inno.Build.Toolchains.NativeBuildContext.configuration`](../../build/toolchains/Inno.Build.Toolchains/NativeBuildContext.cs#L121) | Gets the normalized debug or release configuration prepared by this operation. |
| [`string Inno.Build.Toolchains.NativeBuildContext.engineRoot`](../../build/toolchains/Inno.Build.Toolchains/NativeBuildContext.cs#L116) | Gets the absolute checkout used for every source, intermediate and output path. |

### `Inno.Build.Toolchains.NativeBuildFingerprint`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.NativeBuildFingerprint`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildFingerprint.cs#L13) | Derives recipe identities from ordered declarations, logical input names and complete source bytes. |
| [`static string Inno.Build.Toolchains.NativeBuildFingerprint.Create(System.Collections.Generic.IEnumerable<string> declarations, System.Collections.Generic.IEnumerable<Inno.Build.Toolchains.NativeBuildInput> inputs, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildFingerprint.cs#L39) | Hashes a complete declared input closure independently of checkout location and file timestamps. |

### `Inno.Build.Toolchains.NativeBuildInput`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.NativeBuildInput`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildInput.cs#L9) | Separates the reproducible identity of an input from its physical reading location. |
| [`Inno.Build.Toolchains.NativeBuildInput.NativeBuildInput(string logicalPath, string physicalPath)`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildInput.cs#L23) | Declares one file or directory consumed by a native recipe. |
| [`static Inno.Build.Toolchains.NativeBuildInput Inno.Build.Toolchains.NativeBuildInput.FromPath(string engineRoot, string path)`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildInput.cs#L61) | Declares a checkout-relative input, or an external SDK location whose path affects tool selection. |
| [`string Inno.Build.Toolchains.NativeBuildInput.logicalPath`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildInput.cs#L42) | Gets the identity included in the recipe fingerprint. |
| [`string Inno.Build.Toolchains.NativeBuildInput.physicalPath`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildInput.cs#L47) | Gets the reading location, which is not implicitly included in the fingerprint. |

### `Inno.Build.Toolchains.NativeBuildProduct`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.NativeBuildProduct`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildProduct.cs#L11) | Identifies one complete, integrity-checked native artifact and its exact published file closure. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Build.Toolchains.NativeBuildProduct.files`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildProduct.cs#L50) | Gets the exact absolute output file paths validated by the publisher. |
| [`string Inno.Build.Toolchains.NativeBuildProduct.component`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildProduct.cs#L30) | Gets the component identity supplied by the owning toolchain. |
| [`string Inno.Build.Toolchains.NativeBuildProduct.directory`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildProduct.cs#L45) | Gets the immutable artifact root containing Outputs and its integrity manifest. |
| [`string Inno.Build.Toolchains.NativeBuildProduct.fingerprint`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildProduct.cs#L40) | Gets the content fingerprint covering sources, tool executables and toolchain declarations. |
| [`string Inno.Build.Toolchains.NativeBuildProduct.targetId`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildProduct.cs#L35) | Gets the target ABI selected for this build. |

### `Inno.Build.Toolchains.NativeBuildRecipe`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.NativeBuildRecipe`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildRecipe.cs#L11) | Freezes a component's input closure, ordered tool selection and intermediate owner. |
| [`Inno.Build.Toolchains.NativeBuildRecipe.NativeBuildRecipe(Inno.Build.Toolchains.NativeComponentDescriptor owner, string component, string targetId, System.Collections.Generic.IEnumerable<Inno.Build.Toolchains.NativeBuildInput> inputs, System.Collections.Generic.IEnumerable<string> declarations)`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildRecipe.cs#L37) | Creates a complete recipe without inferring implementation identity from an assembly MVID. |
| [`Inno.Build.Toolchains.NativeComponentDescriptor Inno.Build.Toolchains.NativeBuildRecipe.owner`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildRecipe.cs#L64) | Gets the declared intermediate owner independently of assembly location and MVID. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Build.Toolchains.NativeBuildInput> Inno.Build.Toolchains.NativeBuildRecipe.inputs`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildRecipe.cs#L79) | Gets the frozen declarations of physical inputs and their logical identities. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Build.Toolchains.NativeBuildRecipe.declarations`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildRecipe.cs#L84) | Gets ordered non-file inputs; callers must explicitly sort unordered metadata before construction. |
| [`static Inno.Build.Toolchains.NativeBuildRecipe Inno.Build.Toolchains.NativeBuildRecipe.CreateForComponent(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.NativeComponentDescriptor owner, string component, string targetId, System.Collections.Generic.IEnumerable<string> inputPaths, System.Collections.Generic.IEnumerable<string> declarations, System.Collections.Generic.IEnumerable<string>? implementationPaths = null)`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildRecipe.cs#L117) | Declares a built-in component with its own implementation, common executor and frozen host SDK. |
| [`string Inno.Build.Toolchains.NativeBuildRecipe.component`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildRecipe.cs#L69) | Gets the component's artifact path segment. |
| [`string Inno.Build.Toolchains.NativeBuildRecipe.targetId`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildRecipe.cs#L74) | Gets the target ABI's artifact path segment. |

### `Inno.Build.Toolchains.NativeBuildStatistics`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.NativeBuildStatistics`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildStatistics.cs#L6) | Reports actual input reading and compiler process work accumulated by one build context. |
| [`long Inno.Build.Toolchains.NativeBuildStatistics.hashedBytes`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildStatistics.cs#L16) | Gets the actual bytes consumed by input hashing, excluding output integrity validation. |
| [`long Inno.Build.Toolchains.NativeBuildStatistics.hashedFiles`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildStatistics.cs#L11) | Gets the number of complete file hashes read, including required stability verification. |
| [`long Inno.Build.Toolchains.NativeBuildStatistics.nativeProcesses`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildStatistics.cs#L22) | Gets native execution processes started through the context's frozen tool selection. SDK discovery and managed task bootstrapping are recorded by their separate build logs. |

### `Inno.Build.Toolchains.NativeCMakeExecutor`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.NativeCMakeExecutor`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeCMakeExecutor.cs#L13) | Executes component-owned CMake definitions using an already frozen target SDK selection. |
| [`static System.Threading.Tasks.Task<string> Inno.Build.Toolchains.NativeCMakeExecutor.BuildAsync(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.NativeComponentDescriptor component, string sourceDirectory, string? target, System.Collections.Generic.IReadOnlyList<string> componentArguments, System.Threading.CancellationToken cancellationToken)`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeCMakeExecutor.cs#L48) | Configures and builds an explicit component target without detecting the current platform. |
| [`static string Inno.Build.Toolchains.NativeCMakeExecutor.FindOutput(Inno.Build.Toolchains.NativeBuildContext context, string buildDirectory, string filePattern)`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeCMakeExecutor.cs#L114) | Selects one declared output while excluding CMake's compiler probes and other build configurations. |

### `Inno.Build.Toolchains.NativeComponentBuildOptions`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.NativeComponentBuildOptions`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeComponentBuildOptions.cs#L27) | Freezes product-owned linkage, ordered component definitions and their configuration inputs. |
| [`Inno.Build.Toolchains.NativeComponentBuildOptions.NativeComponentBuildOptions(Inno.Build.Toolchains.NativeLibraryKind libraryKind, System.Collections.Generic.IEnumerable<string>? cmakeArguments = null, System.Collections.Generic.IEnumerable<string>? inputPaths = null)`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeComponentBuildOptions.cs#L46) | Captures component configuration without selecting an SDK or starting a build. |
| [`Inno.Build.Toolchains.NativeLibraryKind Inno.Build.Toolchains.NativeComponentBuildOptions.libraryKind`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeComponentBuildOptions.cs#L74) | Gets the linkage selected by the product rather than by the execution host. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Build.Toolchains.NativeComponentBuildOptions.cmakeArguments`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeComponentBuildOptions.cs#L79) | Gets the immutable ordered component definitions; order contributes to artifact identity. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Build.Toolchains.NativeComponentBuildOptions.inputPaths`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeComponentBuildOptions.cs#L84) | Gets logical configuration input paths whose actual bytes participate in the recipe. |

### `Inno.Build.Toolchains.NativeComponentDescriptor`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.NativeComponentDescriptor`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeComponentDescriptor.cs#L10) | Declares the unique source and build owners of a reusable native component. |
| [`Inno.Build.Toolchains.NativeComponentDescriptor.NativeComponentDescriptor(string id, string nativeProject, string toolchainProject, Inno.Build.Toolchains.NativeStaticBuildDefinition? staticBuild = null, string? bindingConfig = null)`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeComponentDescriptor.cs#L33) | Freezes portable checkout-relative owner locations without assembly-name or directory inference. |
| [`Inno.Build.Toolchains.NativeStaticBuildDefinition? Inno.Build.Toolchains.NativeComponentDescriptor.staticBuild`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeComponentDescriptor.cs#L74) | Gets the declared static build capability without requiring a platform-specific component project. |
| [`string Inno.Build.Toolchains.NativeComponentDescriptor.GetNativeRoot(string engineRoot)`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeComponentDescriptor.cs#L90) | Resolves the declared native owner within an explicitly supplied checkout. |
| [`string Inno.Build.Toolchains.NativeComponentDescriptor.GetToolchainRoot(string engineRoot)`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeComponentDescriptor.cs#L101) | Resolves the declared recipe owner within an explicitly supplied checkout. |
| [`string Inno.Build.Toolchains.NativeComponentDescriptor.id`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeComponentDescriptor.cs#L59) | Gets the portable artifact identity. |
| [`string Inno.Build.Toolchains.NativeComponentDescriptor.nativeProject`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeComponentDescriptor.cs#L64) | Gets the explicit checkout-relative Native owner project. |
| [`string Inno.Build.Toolchains.NativeComponentDescriptor.toolchainProject`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeComponentDescriptor.cs#L69) | Gets the explicit checkout-relative recipe owner project. |
| [`string? Inno.Build.Toolchains.NativeComponentDescriptor.bindingConfig`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeComponentDescriptor.cs#L79) | Gets the declared binding definition, or null for a component without generated bindings. |

### `Inno.Build.Toolchains.NativeInputMaterializer`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.NativeInputMaterializer`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeInputMaterializer.cs#L13) | Materializes frozen native inputs without rewriting identical intermediate files. |
| [`static System.Threading.Tasks.ValueTask<bool> Inno.Build.Toolchains.NativeInputMaterializer.CopyAsync(Inno.Build.Toolchains.NativeBuildContext context, string source, string destination, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeInputMaterializer.cs#L39) | Copies a declared input atomically when its destination differs, preserving identical file timestamps. |

### `Inno.Build.Toolchains.NativeLibraryKind`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.NativeLibraryKind`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeComponentBuildOptions.cs#L11) | Specifies the linkage required by a product's native component operation. |
| [`Inno.Build.Toolchains.NativeLibraryKind.Shared`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeComponentBuildOptions.cs#L21) | Produces libraries loaded by the deployed product. |
| [`Inno.Build.Toolchains.NativeLibraryKind.Static`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeComponentBuildOptions.cs#L16) | Produces archives for the selected final linker. |

### `Inno.Build.Toolchains.NativeStaticBuildDefinition`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.NativeStaticBuildDefinition`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeStaticBuildDefinition.cs#L10) | Declares a backend-owned static-library recipe and its complete checkout input closure. |
| [`Inno.Build.Toolchains.NativeStaticBuildDefinition.NativeStaticBuildDefinition(string cmakeFile, System.Collections.Generic.IEnumerable<string> inputPaths, System.Collections.Generic.IEnumerable<string> archiveNames, System.Collections.Generic.IEnumerable<string> targetIds)`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeStaticBuildDefinition.cs#L33) | Freezes the component's CMake entry point, source dependencies and exact installed archive names. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Build.Toolchains.NativeStaticBuildDefinition.archiveNames`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeStaticBuildDefinition.cs#L69) | Gets the exact required output closure within this component's target directory. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Build.Toolchains.NativeStaticBuildDefinition.inputPaths`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeStaticBuildDefinition.cs#L64) | Gets the immutable complete source dependency paths relative to the checkout. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Build.Toolchains.NativeStaticBuildDefinition.targetIds`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeStaticBuildDefinition.cs#L74) | Gets the exact supported target identities; other archive formats require a separate recipe. |
| [`string Inno.Build.Toolchains.NativeStaticBuildDefinition.cmakeFile`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeStaticBuildDefinition.cs#L59) | Gets the component-owned CMake definition, which participates in artifact identity. |

### `Inno.Build.Toolchains.NativeToolchainPreparation`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.NativeToolchainPreparation`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeToolchainPreparation.cs#L11) | Supplies shared environment isolation and SDK input validation without selecting a platform. |
| [`static Inno.Build.Toolchains.NativeToolchainSelection Inno.Build.Toolchains.NativeToolchainPreparation.Freeze(string targetId, Inno.Build.Toolchains.BuildHostDescriptor host, System.Collections.Generic.IReadOnlyDictionary<string, string> tools, System.Collections.Generic.IReadOnlyDictionary<string, string> environment, System.Collections.Generic.IEnumerable<string> inputs, System.Collections.Generic.IEnumerable<string> cmakeArguments, string sharedLibraryExtension, bool multiConfiguration)`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeToolchainPreparation.cs#L62) | Validates and freezes a provider's complete tool selection before product staging. |
| [`static System.Collections.Generic.Dictionary<string, string> Inno.Build.Toolchains.NativeToolchainPreparation.CreateEnvironment()`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeToolchainPreparation.cs#L26) | Creates a child-process environment that clears undeclared ambient compiler policy. |

### `Inno.Build.Toolchains.NativeToolchainSelection`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.BuildHostDescriptor Inno.Build.Toolchains.NativeToolchainSelection.host`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeToolchainSelection.cs#L115) | Gets the declared tool execution host. |
| [`Inno.Build.Toolchains.NativeToolchainSelection`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeToolchainSelection.cs#L12) | Freezes target compiler tools and SDK inputs independently of discovery and platform policy. |
| [`Inno.Build.Toolchains.NativeToolchainSelection.NativeToolchainSelection(string targetId, Inno.Build.Toolchains.BuildHostDescriptor host, System.Collections.Generic.IReadOnlyDictionary<string, string> tools, System.Collections.Generic.IReadOnlyDictionary<string, string> environment, System.Collections.Generic.IEnumerable<string> inputPaths, System.Collections.Generic.IEnumerable<string> cmakeArguments, string sharedLibraryExtension, bool multiConfiguration)`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeToolchainSelection.cs#L52) | Captures an immutable, explicit compiler and SDK selection supplied by a platform provider. |
| [`System.Collections.Generic.IReadOnlyDictionary<string, string> Inno.Build.Toolchains.NativeToolchainSelection.environment`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeToolchainSelection.cs#L135) | Gets the environment supplied exclusively to owned tools. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Build.Toolchains.NativeToolchainSelection.cmakeArguments`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeToolchainSelection.cs#L130) | Gets configuration arguments selecting the compiler, generator and SDK. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Build.Toolchains.NativeToolchainSelection.declarations`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeToolchainSelection.cs#L125) | Gets ordered tool and environment facts included in the recipe fingerprint. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Build.Toolchains.NativeToolchainSelection.inputPaths`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeToolchainSelection.cs#L120) | Gets the immutable SDK and compiler input closure. |
| [`bool Inno.Build.Toolchains.NativeToolchainSelection.TryResolveExecutable(string name, out string executable)`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeToolchainSelection.cs#L178) | Checks an optional tool capability without discovering a command from the process environment. |
| [`bool Inno.Build.Toolchains.NativeToolchainSelection.multiConfiguration`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeToolchainSelection.cs#L145) | Gets whether the selected generator supports build-time configuration selection. |
| [`string Inno.Build.Toolchains.NativeToolchainSelection.ResolveExecutable(string name)`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeToolchainSelection.cs#L159) | Resolves an executable from the frozen selection rather than a mutable process PATH. |
| [`string Inno.Build.Toolchains.NativeToolchainSelection.sharedLibraryExtension`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeToolchainSelection.cs#L140) | Gets the target's shared-library suffix. |
| [`string Inno.Build.Toolchains.NativeToolchainSelection.targetId`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeToolchainSelection.cs#L110) | Gets the target identity, without inferring it from the execution host. |

### `Inno.Build.Toolchains.ProductNativeBuildPlan`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.ProductNativeBuildPlan`](../../build/toolchains/Inno.Build.Toolchains/Native/ProductNativeBuildPlan.cs#L14) | Executes a frozen product closure with one target toolchain and no implicit backend selection. |
| [`Inno.Build.Toolchains.ProductNativeBuildPlan.ProductNativeBuildPlan(string productId, System.Collections.Generic.IReadOnlyList<Inno.Build.Toolchains.ProductNativeBuildStep> steps)`](../../build/toolchains/Inno.Build.Toolchains/Native/ProductNativeBuildPlan.cs#L33) | Validates the complete ordered component graph before tools or staging can start. |
| [`System.Collections.Generic.IReadOnlyDictionary<string, string> Inno.Build.Toolchains.ProductNativeBuildPlan.CreateDeploymentFiles(System.Collections.Generic.IReadOnlyList<Inno.Build.Toolchains.NativeBuildProduct> products)`](../../build/toolchains/Inno.Build.Toolchains/Native/ProductNativeBuildPlan.cs#L119) | Resolves exact deployment paths from the declared layout rather than component-name conventions. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Build.Toolchains.ProductNativeBuildStep> Inno.Build.Toolchains.ProductNativeBuildPlan.steps`](../../build/toolchains/Inno.Build.Toolchains/Native/ProductNativeBuildPlan.cs#L60) | Gets the frozen dependency-ordered component closure. |
| [`System.Threading.Tasks.Task<System.Collections.Generic.IReadOnlyList<Inno.Build.Toolchains.NativeBuildProduct>> Inno.Build.Toolchains.ProductNativeBuildPlan.BuildAsync(Inno.Build.Toolchains.NativeBuildContext context, System.Threading.CancellationToken cancellationToken)`](../../build/toolchains/Inno.Build.Toolchains/Native/ProductNativeBuildPlan.cs#L83) | Prepares the declared closure using the context's already resolved target tools. |
| [`string Inno.Build.Toolchains.ProductNativeBuildPlan.productId`](../../build/toolchains/Inno.Build.Toolchains/Native/ProductNativeBuildPlan.cs#L55) | Gets the explicit product identity. |

### `Inno.Build.Toolchains.ProductNativeBuildStep`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.NativeComponentBuildOptions Inno.Build.Toolchains.ProductNativeBuildStep.options`](../../build/toolchains/Inno.Build.Toolchains/Native/ProductNativeBuildStep.cs#L83) | Gets product-owned configuration independently of the selected SDK. |
| [`Inno.Build.Toolchains.NativeComponentDescriptor Inno.Build.Toolchains.ProductNativeBuildStep.component`](../../build/toolchains/Inno.Build.Toolchains/Native/ProductNativeBuildStep.cs#L78) | Gets the explicit source and recipe owners. |
| [`Inno.Build.Toolchains.ProductNativeBuildStep`](../../build/toolchains/Inno.Build.Toolchains/Native/ProductNativeBuildStep.cs#L12) | Binds one product component to an owned build operation and explicit deployment layout. |
| [`Inno.Build.Toolchains.ProductNativeBuildStep.ProductNativeBuildStep(string id, Inno.Build.Toolchains.NativeComponentDescriptor component, Inno.Build.Toolchains.NativeComponentBuildOptions options, System.Collections.Generic.IReadOnlyList<string> dependencies, System.Func<Inno.Build.Toolchains.NativeBuildContext, System.Collections.Generic.IReadOnlyDictionary<string, Inno.Build.Toolchains.NativeBuildProduct>, System.Threading.CancellationToken, System.Threading.Tasks.Task<Inno.Build.Toolchains.NativeBuildProduct>>? build, System.Func<string, string, string?> deploymentPath)`](../../build/toolchains/Inno.Build.Toolchains/Native/ProductNativeBuildStep.cs#L41) | Freezes a component step without embedding any backend list in the shared executor. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Build.Toolchains.ProductNativeBuildStep.dependencies`](../../build/toolchains/Inno.Build.Toolchains/Native/ProductNativeBuildStep.cs#L88) | Gets dependencies that must precede this step. |
| [`System.Func<Inno.Build.Toolchains.NativeBuildContext, System.Collections.Generic.IReadOnlyDictionary<string, Inno.Build.Toolchains.NativeBuildProduct>, System.Threading.CancellationToken, System.Threading.Tasks.Task<Inno.Build.Toolchains.NativeBuildProduct>>? Inno.Build.Toolchains.ProductNativeBuildStep.build`](../../build/toolchains/Inno.Build.Toolchains/Native/ProductNativeBuildStep.cs#L93) | Gets the standalone producer, or null for a component owned by a static aggregate executor. |
| [`System.Func<string, string, string?> Inno.Build.Toolchains.ProductNativeBuildStep.deploymentPath`](../../build/toolchains/Inno.Build.Toolchains/Native/ProductNativeBuildStep.cs#L98) | Gets the explicit output layout policy; null results exclude build-only files. |
| [`string Inno.Build.Toolchains.ProductNativeBuildStep.id`](../../build/toolchains/Inno.Build.Toolchains/Native/ProductNativeBuildStep.cs#L73) | Gets the exact expected component identity. |

### `Inno.Build.Toolchains.ProductNativeDeployment`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.ProductNativeDeployment`](../../build/toolchains/Inno.Build.Toolchains/Native/ProductNativeDeployment.cs#L15) | Materializes an explicitly built host-native closure into one application's deployment directory. |
| [`static System.Threading.Tasks.ValueTask Inno.Build.Toolchains.ProductNativeDeployment.InstallAsync(System.Collections.Generic.IReadOnlyList<Inno.Build.Toolchains.NativeBuildProduct> products, Inno.Build.Toolchains.ProductNativeBuildPlan plan, string applicationDirectory, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/toolchains/Inno.Build.Toolchains/Native/ProductNativeDeployment.cs#L46) | Validates the application's native tree and atomically replaces it when its exact contents differ. |

### `Inno.Build.Toolchains.ToolchainEnvironment`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.ToolchainEnvironment`](../../build/toolchains/Inno.Build.Toolchains/ToolchainEnvironment.cs#L15) | Provides deterministic host-process and workspace operations shared by native dependency toolchains. |
| [`static System.Threading.Tasks.Task Inno.Build.Toolchains.ToolchainEnvironment.RunAsync(Inno.Build.Toolchains.NativeBuildContext context, string fileName, System.Collections.Generic.IReadOnlyList<string> arguments, string workingDirectory, System.Threading.CancellationToken cancellationToken)`](../../build/toolchains/Inno.Build.Toolchains/ToolchainEnvironment.cs#L93) | Runs a host-native process with the context's frozen compiler and SDK selection. |
| [`static System.Threading.Tasks.Task Inno.Build.Toolchains.ToolchainEnvironment.RunAsync(Inno.Build.Toolchains.NativeBuildContext context, string fileName, System.Collections.Generic.IReadOnlyList<string> arguments, string workingDirectory, System.Threading.CancellationToken cancellationToken, System.Collections.Generic.IReadOnlyDictionary<string, string> environment)`](../../build/toolchains/Inno.Build.Toolchains/ToolchainEnvironment.cs#L50) | Executes a declared absolute tool with an explicitly resolved SDK environment and records the operation cost. |
| [`static System.Threading.Tasks.Task Inno.Build.Toolchains.ToolchainEnvironment.RunAsync(Inno.Build.Toolchains.NativeBuildContext context, string fileName, string arguments, string workingDirectory, System.Threading.CancellationToken cancellationToken)`](../../build/toolchains/Inno.Build.Toolchains/ToolchainEnvironment.cs#L134) | Runs a quoted host command with a frozen compiler and SDK environment. |
| [`static System.Threading.Tasks.Task Inno.Build.Toolchains.ToolchainEnvironment.RunAsync(string fileName, System.Collections.Generic.IReadOnlyList<string> arguments, string workingDirectory, System.Threading.CancellationToken cancellationToken, System.Collections.Generic.IReadOnlyDictionary<string, string?>? environment = null)`](../../build/toolchains/Inno.Build.Toolchains/ToolchainEnvironment.cs#L216) | Runs a child build with structured arguments, cancellation and hidden windows. |
| [`static System.Threading.Tasks.Task Inno.Build.Toolchains.ToolchainEnvironment.RunAsync(string fileName, System.Collections.Generic.IReadOnlyList<string> arguments, string workingDirectory, System.Threading.CancellationToken cancellationToken, System.Collections.Generic.IReadOnlyDictionary<string, string?>? environment, System.IO.TextWriter standardOutput, System.IO.TextWriter standardError)`](../../build/toolchains/Inno.Build.Toolchains/ToolchainEnvironment.cs#L261) | Runs a child build with caller-owned output destinations and the shared process retirement protocol. |
| [`static System.Threading.Tasks.Task Inno.Build.Toolchains.ToolchainEnvironment.RunAsync(string fileName, string arguments, string workingDirectory, System.Threading.CancellationToken cancellationToken)`](../../build/toolchains/Inno.Build.Toolchains/ToolchainEnvironment.cs#L390) | Runs a child build using a complete argument string, without a shell. |
| [`static System.Threading.Tasks.Task<string> Inno.Build.Toolchains.ToolchainEnvironment.CaptureOutputAsync(System.Diagnostics.ProcessStartInfo start, System.Threading.CancellationToken cancellationToken)`](../../build/toolchains/Inno.Build.Toolchains/ToolchainEnvironment.cs#L512) | Captures platform tool discovery through the same cancellable process lifecycle as builds. |
| [`static System.Threading.Tasks.Task<string> Inno.Build.Toolchains.ToolchainEnvironment.CaptureOutputAsync(string fileName, System.Collections.Generic.IReadOnlyList<string> arguments, string workingDirectory, System.Threading.CancellationToken cancellationToken, System.Collections.Generic.IReadOnlyDictionary<string, string?>? environment = null)`](../../build/toolchains/Inno.Build.Toolchains/ToolchainEnvironment.cs#L309) | Captures a tool's standard output while forwarding errors and preserving the common process lifecycle. |
| [`static bool Inno.Build.Toolchains.ToolchainEnvironment.ContainsAny(string value, params string[] needles)`](../../build/toolchains/Inno.Build.Toolchains/ToolchainEnvironment.cs#L417) | Determines whether a value contains at least one token without regard to casing. |
| [`static string Inno.Build.Toolchains.ToolchainEnvironment.FindRepoRoot()`](../../build/toolchains/Inno.Build.Toolchains/ToolchainEnvironment.cs#L345) | Resolves the repository containing the currently executing toolchain assembly. |
| [`static string Inno.Build.Toolchains.ToolchainEnvironment.NormalizeOutputName(string fileName, string config)`](../../build/toolchains/Inno.Build.Toolchains/ToolchainEnvironment.cs#L444) | Normalizes a native artifact name and appends its build configuration. |
| [`static string Inno.Build.Toolchains.ToolchainEnvironment.ResolveExecutable(string name)`](../../build/toolchains/Inno.Build.Toolchains/ToolchainEnvironment.cs#L164) | Resolves one explicitly selected executable before it becomes a build input. |
| [`static string Inno.Build.Toolchains.ToolchainEnvironment.TrimConfigSuffix(string baseName)`](../../build/toolchains/Inno.Build.Toolchains/ToolchainEnvironment.cs#L463) | Removes a trailing native debug or release token from an artifact stem. |
| [`static void Inno.Build.Toolchains.ToolchainEnvironment.DeleteDirectory(string path)`](../../build/toolchains/Inno.Build.Toolchains/ToolchainEnvironment.cs#L486) | Deletes one explicitly resolved toolchain output directory when it exists. |
| [`static void Inno.Build.Toolchains.ToolchainEnvironment.ValidateConfiguration(string configuration)`](../../build/toolchains/Inno.Build.Toolchains/ToolchainEnvironment.cs#L330) | Validates the configuration shared by all native component builds. |

### `Inno.Build.Toolchains.ToolchainLayout`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.ToolchainLayout`](../../build/toolchains/Inno.Build.Toolchains/ToolchainLayout.cs#L6) | Defines stable repository-relative names shared by native dependency toolchains. |
| [`const string Inno.Build.Toolchains.ToolchainLayout.C_DEBUG_CONFIGURATION`](../../build/toolchains/Inno.Build.Toolchains/ToolchainLayout.cs#L21) | Identifies the normalized debug configuration token. |
| [`const string Inno.Build.Toolchains.ToolchainLayout.C_EXTERNAL_DIRECTORY_NAME`](../../build/toolchains/Inno.Build.Toolchains/ToolchainLayout.cs#L16) | Identifies the directory containing checked-out native dependency sources. |
| [`const string Inno.Build.Toolchains.ToolchainLayout.C_RELEASE_CONFIGURATION`](../../build/toolchains/Inno.Build.Toolchains/ToolchainLayout.cs#L26) | Identifies the normalized release configuration token. |
| [`const string Inno.Build.Toolchains.ToolchainLayout.C_REPOSITORY_MARKER_FILE`](../../build/toolchains/Inno.Build.Toolchains/ToolchainLayout.cs#L11) | Identifies the repository marker used while resolving a toolchain workspace. |

### `Inno.Build.Toolchains.ToolchainWorkingDirectory`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.ToolchainWorkingDirectory`](../../build/toolchains/Inno.Build.Toolchains/ToolchainWorkingDirectory.cs#L15) | Owns a stable execution path for tools whose Windows filesystem APIs require bounded paths. The physical directory remains the caller's build-owned storage on every host. |
| [`static System.Threading.Tasks.ValueTask<Inno.Build.Toolchains.ToolchainWorkingDirectory> Inno.Build.Toolchains.ToolchainWorkingDirectory.OpenAsync(string physicalPath, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/toolchains/Inno.Build.Toolchains/ToolchainWorkingDirectory.cs#L63) | Waits for exclusive use of the directory's execution alias and creates the physical directory. The alias has a stable identity so tool caches can be reused across completed operations. |
| [`string Inno.Build.Toolchains.ToolchainWorkingDirectory.toolPath`](../../build/toolchains/Inno.Build.Toolchains/ToolchainWorkingDirectory.cs#L33) | Gets the execution directory, which is a temporary Windows junction or the physical path. All processes using this path must stop before this owner is disposed. |
| [`void Inno.Build.Toolchains.ToolchainWorkingDirectory.Dispose()`](../../build/toolchains/Inno.Build.Toolchains/ToolchainWorkingDirectory.cs#L92) | Removes the verified execution alias and releases ownership after tool processes have stopped. Physical files are retained; repeated disposal has no effect. |

## 项目依赖

- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
- [Inno.Core.IO](../core/Inno.Core.IO.md)：公开引用边界由实际签名核对。
