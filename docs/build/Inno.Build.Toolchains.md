# Inno.Build.Toolchains

`ToolchainEnvironment.ResolveExecutable(name)` 在启动前解析明确指定的文件或 PATH 命令，返回绝对路径，
用于执行和工具指纹。带目录的缺失路径直接抛出 `FileNotFoundException`，不能悄悄选择 PATH 上的同名工具。
它不改变 PATH，也不选择 SDK；SDK 选择属于具体 resolver。

[Build 索引](README.md) · [Wiki 首页](../README.md) · [宿主组合](Inno.Build.Toolchains.Host.md) · [Native](../native/README.md)

## 公开 API

- `ToolchainEnvironment`：建立受控工具进程环境。
- `NativeBuildContext(engineRoot, configuration)`：验证并冻结显式 checkout 和 `debug`/`release` 配置；公开 `engineRoot`、`configuration`、可空 `hostToolchain` 与 `GetNativeBuildRoot(Assembly)`。
- `BuildArtifactOptions`：描述 debug/release 与目标 artifact 选择。
- 其公开构造参数/只读成员为 `buildDirName`、`libraryTokens`、`extensions`、`requiredPathTokens`、`normalizeOutputName`。
- `BuildArtifactCopier`：复制明确的 Native 产物。
- `ToolchainLayout`：解析仓库内 toolchain/source/output 布局。
- `ToolchainWorkingDirectory.OpenAsync(physicalPath, cancellationToken)`：取得执行目录的独占所有权。`toolPath` 在 Windows 为有界 junction，在其他宿主为原目录；工具必须全部退出后才 `Dispose()`。释放只删除经校验的 alias，不删除真实中间产物；取消等待保留其他 owner，错误占位明确失败。
- `NativeBuildFingerprint.Create(declarations, files)`：按确定顺序散列工具链声明与显式绝对文件的 bytes，重复路径只读取一次；文件缺失或相对路径明确失败。
- `BuildArtifactManifest.Write(directory, fingerprint, outputDirectories)`：在受 owner 保护的 staging 内记录输出相对路径及 SHA-256；输出目录必须位于根内，空产物失败。
- `BuildArtifactManifest.IsComplete(directory, fingerprint, outputDirectories)`：检查身份、精确文件集合和 bytes；缺失、损坏或不匹配返回 false，实际 IO 失败传播。
- `NativeBindingGenerationDescriptor`：必填 `fingerprint`、`bindingsPath`、`bridgeDirectory`；空 bridge 表示直接 C 绑定。`Write(path)` 原子写入请求独占的描述文件，`Load(path)` 验证当前源文件和可选桥目录存在，错误描述抛出 `InvalidDataException`。

### 按身份发布的原生产物

`NativeArtifactPublisher.PublishAsync(context, owner, component, targetId, inputPaths, declarations, build, cancellationToken)`
统一目录事务。`inputPaths` 可以是完整源码目录或显式工具文件；相对路径从选中的 checkout 解析。
声明补充 SDK、编译参数和 binding generation 身份。输入列表和声明在进入异步流程前复制。
目录闭包在获取 owner 后及候选完成后重新枚举，新增、删除或修改源文件都会使当前候选失败。

producer 得到当前目标及指纹限定的 `NativeBuildContext`、私有输出目录和 cancellation token。
Native producer 和托管 publisher 共用 `ToolchainWorkingDirectory`。Windows 的 CMake、Mono AOT 等工具
仍可能使用受传统路径长度限制的文件 API；短路径属于构建机器的执行边界，与游戏目标和运行服务无关。
它必须等待全部工作退出并验证所需产物。只有完整输出经过内容清单校验后，才会安装到
`artifacts/native/<component>/<target>/<fingerprint>`；对应中间态属于组件项目的
`obj/native/<target>/<fingerprint>`。producer 不发布可变的全局“最新路径”。

`NativeBuildProduct` 只由 publisher 返回，其公开只读成员为 `component`、`targetId`、
`fingerprint`、`directory` 和 `files`。`files` 是被验证的确切绝对输出集合，供 Support Pack
和 build composition 消费；不能把递归扫描其他产物目录当作此次构建的输入。

复用前核对身份、精确文件集合和 SHA-256。损坏缓存重建；失败、取消或输入变化不会删除
其他已经完成的 product。`FileLease` 和 `AtomicDirectory` 复用 Core.IO 的 owner 与回滚协议。

`ToolchainEnvironment.RunAsync` 使用结构化参数和 child-only 环境运行隐藏进程，实时转交输出。
`CaptureOutputAsync` 使用同一生命周期捕获标准输出，供 SDK/property 查询使用；错误流仍转交。
两者取消时杀死整个进程树，等待退出并排空输出后才返回，失败不进入下一构建阶段。
标准输出或错误输出读取/转交失败时，也会立即终止进程树并观察全部输出任务；不会等到子进程写满管道后永久挂起。
`ValidateConfiguration` 只接受 `debug`/`release`；`FindRepoRoot` 从工具链程序集所在位置解析 checkout，
单文件宿主没有程序集位置时使用宿主目录。该方法只供 composition root 选择默认 checkout，组件禁止隐式解析根目录。
`RunAsync(fileName, string arguments, workingDirectory, token)` 接受按目标进程规则引用的完整参数串，复用相同取消与输出生命周期。
`ContainsAny`、`NormalizeOutputName`、`TrimConfigSuffix` 分别匹配产物 token、统一配置后缀和移除已有后缀。
`DeleteDirectory` 要求显式绝对路径；调用方负责先验证输出 owner 范围。

`NativeBuildContext.GetNativeBuildRoot(Assembly)` 根据工具链程序集名称验证选中 checkout 的对应项目，并返回其 `obj/native`。
SDL3、MiniAudio、ImGui 和 ImGuizmo 的各平台中间产物统一位于这里，并按平台和配置隔离；
ImGui 的受校验源码 overlay 也在工具链缓存内。BGFX 的上游 GENie 在工具链拥有的源码快照中运行；所有第三方 checkout 保持只读。

Windows 的 `msbuild` / `cl` 通过 Visual Studio 官方 `vswhere` 发现 C++ 工具安装；
编译器需要的 PATH、INCLUDE、LIB 和 LIBPATH 仅进入子进程，不要求用户打开 Developer Terminal，
不修改当前进程或系统环境。缺少 C++ Build Tools 明确失败。CMake 与 Windows MSBuild 顺序编译。

该项目只服务构建机器，不进入 Runtime 或 Player。路径缺失、产物歧义和进程失败必须明确抛出，不使用 fallback tool location。`BuildArtifactCopier.CopyArtifacts` 按最接近产物的 Debug/Release 目录或文件后缀选择目标配置；没有目标文件或多个源落到同一输出名时直接失败，绝不把已有 Debug DLL 当作 Release 产物发布。

没有供外部派生者使用的 protected 扩展点。ToolchainLayout 的全部公开常量为 `C_REPOSITORY_MARKER_FILE`、
`C_EXTERNAL_DIRECTORY_NAME`、`C_DEBUG_CONFIGURATION`、`C_RELEASE_CONFIGURATION`。

```csharp
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.Sdl3;

await Sdl3Toolchain.BuildAsync(new NativeBuildContext(engineRoot, "release"), cancellationToken);
```

## 宿主工具链选择

`HostNativeToolchain.ResolveAsync(context, cancellationToken)` 返回带冻结选择的新 context；已经选中的 context 直接复用。
公开只读成员为 `targetId`、`inputPaths`、`declarations`、`cmakeArguments`、`environment`；
`ResolveExecutable(name)` 只返回此次选择中的绝对工具路径。未知命令明确失败。
Windows 使用 vswhere 与选中安装的 VsDevCmd，固定 MSVC、Windows SDK、MSBuild 和该安装自带的 CMake；
macOS 使用 xcrun 固定编译器和 macOS SDK；Linux 固定 PATH 解析所得的具体编译器。
编译器/工具文件及所选 SDK include/lib 目录进入产物指纹，环境变量只传给拥有的子进程。
宿主工具链清空隐式的 CL、LINK、CFLAGS/CXXFLAGS、额外 include/lib 搜索路径和 CMake generator overrides。
这些环境变量不能绕过当前请求的明确工具/SDK 选择，也不能让同一指纹生成不同的二进制；父进程环境保持原值。
`ToolchainEnvironment.RunAsync(context, fileName, arguments, workingDirectory, cancellationToken)`
接受结构化列表或已引用的参数串，使用冻结环境；配置 CMake 时自动附加明确的 compiler/SDK 选择。
这个宿主机制不替代 Browser 的 workload SDK resolver。






## 本轮边界与所有权

NativeBuildRecipe 声明实际组件输入与工具身份；NativeBuildInput 将逻辑身份与物理读取位置分开。operation-owned snapshot 对共享文件去重哈希；等锁后与发布前重新验证稳定性。指纹包含真正参与 recipe 的组件/共同执行代码、参数、SDK/compiler、生成身份与必需 exports，不以无关工具程序集 MVID 广泛失效。实际影响产物的执行路径仍参与身份。

## 当前源码公开 API 清单

以下仅列出当前程序集自己声明的 public/protected 契约；继承成员遵循所属基类页面。internal/private 实现不作为稳定公开 API。签名依据当前源码语义模型生成，行为、参数、异常与所有权说明同时以对应英文 XML 为准。

### `Inno.Build.Toolchains.BuildArtifactCopier`

| 当前声明 | 行为 |
| --- | --- |
| [`static void Inno.Build.Toolchains.BuildArtifactCopier.CopyArtifacts(string buildRoot, string outputDir, string config, Inno.Build.Toolchains.BuildArtifactOptions options)`](../../build/toolchains/Inno.Build.Toolchains/BuildArtifactOptions.cs#L56) | Copies all artifacts accepted by one product policy into its output directory. |
| [`Inno.Build.Toolchains.BuildArtifactCopier`](../../build/toolchains/Inno.Build.Toolchains/BuildArtifactOptions.cs#L37) | Copies filtered native outputs into the engine's rebuildable dependency store. |

### `Inno.Build.Toolchains.BuildArtifactManifest`

| 当前声明 | 行为 |
| --- | --- |
| [`static bool Inno.Build.Toolchains.BuildArtifactManifest.IsComplete(string directory, string fingerprint, System.Collections.Generic.IReadOnlyList<string> outputDirectories)`](../../build/toolchains/Inno.Build.Toolchains/BuildArtifactManifest.cs#L70) | Checks the identity, exact output file set and bytes before reusing a cached artifact. |
| [`static void Inno.Build.Toolchains.BuildArtifactManifest.Write(string directory, string fingerprint, System.Collections.Generic.IReadOnlyList<string> outputDirectories)`](../../build/toolchains/Inno.Build.Toolchains/BuildArtifactManifest.cs#L36) | Records a completed staging tree before its owner publishes the directory. |
| [`Inno.Build.Toolchains.BuildArtifactManifest`](../../build/toolchains/Inno.Build.Toolchains/BuildArtifactManifest.cs#L14) | Validates complete build outputs against their input identity and recorded file hashes. |

### `Inno.Build.Toolchains.BuildArtifactOptions`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.BuildArtifactOptions`](../../build/toolchains/Inno.Build.Toolchains/BuildArtifactOptions.cs#L26) | Defines the deterministic filter and naming policy used to collect one native product. |

### `Inno.Build.Toolchains.DotNetSdkDescriptor`

| 当前声明 | 行为 |
| --- | --- |
| [`string Inno.Build.Toolchains.DotNetSdkDescriptor.hostPath`](../../build/toolchains/Inno.Build.Toolchains/Managed/DotNetSdkDescriptor.cs#L19) | Gets the executable used for project-scoped SDK and workload resolution. |
| [`string Inno.Build.Toolchains.DotNetSdkDescriptor.sdkIdentity`](../../build/toolchains/Inno.Build.Toolchains/Managed/DotNetSdkDescriptor.cs#L24) | Gets the exact SDK identity selected by the project's global.json resolution rules. |
| [`Inno.Build.Toolchains.DotNetSdkDescriptor`](../../build/toolchains/Inno.Build.Toolchains/Managed/DotNetSdkDescriptor.cs#L6) | Records the executable and SDK identity selected by resolving one managed entry project. |

### `Inno.Build.Toolchains.DotNetSdkResolver`

| 当前声明 | 行为 |
| --- | --- |
| [`static System.Threading.Tasks.ValueTask<Inno.Build.Toolchains.DotNetSdkDescriptor> Inno.Build.Toolchains.DotNetSdkResolver.ResolveAsync(string hostPath, string projectPath, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/toolchains/Inno.Build.Toolchains/Managed/DotNetSdkResolver.cs#L34) | Asks the selected host to resolve its SDK from the project location without guessing installed versions. |
| [`Inno.Build.Toolchains.DotNetSdkResolver`](../../build/toolchains/Inno.Build.Toolchains/Managed/DotNetSdkResolver.cs#L11) | Resolves the managed SDK using the prepared entry project's directory and normal global.json rules. |

### `Inno.Build.Toolchains.HostNativeToolchain`

| 当前声明 | 行为 |
| --- | --- |
| [`static System.Threading.Tasks.Task<Inno.Build.Toolchains.NativeBuildContext> Inno.Build.Toolchains.HostNativeToolchain.ResolveAsync(Inno.Build.Toolchains.NativeBuildContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/toolchains/Inno.Build.Toolchains/Native/HostNativeToolchain.cs#L114) | Selects the host tools once, or preserves the selection already attached to the build context. |
| [`string Inno.Build.Toolchains.HostNativeToolchain.ResolveExecutable(string name)`](../../build/toolchains/Inno.Build.Toolchains/Native/HostNativeToolchain.cs#L83) | Resolves an executable from this operation's frozen selection. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Build.Toolchains.HostNativeToolchain.cmakeArguments`](../../build/toolchains/Inno.Build.Toolchains/Native/HostNativeToolchain.cs#L63) | Gets configuration arguments that pin CMake to the selected compiler and SDK. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Build.Toolchains.HostNativeToolchain.declarations`](../../build/toolchains/Inno.Build.Toolchains/Native/HostNativeToolchain.cs#L58) | Gets the frozen compiler and SDK selection used alongside file fingerprints. |
| [`System.Collections.Generic.IReadOnlyDictionary<string, string> Inno.Build.Toolchains.HostNativeToolchain.environment`](../../build/toolchains/Inno.Build.Toolchains/Native/HostNativeToolchain.cs#L69) | Gets variables applied only to owned child processes, without changing the parent environment. Ambient compiler flags, include paths and CMake generator overrides are cleared; declared inputs select the build. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Build.Toolchains.HostNativeToolchain.inputPaths`](../../build/toolchains/Inno.Build.Toolchains/Native/HostNativeToolchain.cs#L53) | Gets compiler, SDK and tool files whose bytes participate in native product identity. |
| [`string Inno.Build.Toolchains.HostNativeToolchain.targetId`](../../build/toolchains/Inno.Build.Toolchains/Native/HostNativeToolchain.cs#L48) | Gets the native ABI selected from the operating system and process architecture. |
| [`Inno.Build.Toolchains.HostNativeToolchain`](../../build/toolchains/Inno.Build.Toolchains/Native/HostNativeToolchain.cs#L16) | Freezes the host compiler, SDK, build executables and child environment before native publication. |

### `Inno.Build.Toolchains.NativeArtifactPublisher`

| 当前声明 | 行为 |
| --- | --- |
| [`static System.Threading.Tasks.Task<Inno.Build.Toolchains.NativeBuildProduct> Inno.Build.Toolchains.NativeArtifactPublisher.PublishAsync(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.NativeBuildRecipe recipe, System.Func<Inno.Build.Toolchains.NativeBuildContext, string, System.Threading.CancellationToken, System.Threading.Tasks.Task> build, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeArtifactPublisher.cs#L47) | Reuses a validated product or builds an isolated candidate and publishes it after input stability checks. |
| [`Inno.Build.Toolchains.NativeArtifactPublisher`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeArtifactPublisher.cs#L14) | Publishes complete native products by source and toolchain identity under process-shared ownership. |

### `Inno.Build.Toolchains.NativeBindingGenerationDescriptor`

| 当前声明 | 行为 |
| --- | --- |
| [`static Inno.Build.Toolchains.NativeBindingGenerationDescriptor Inno.Build.Toolchains.NativeBindingGenerationDescriptor.Load(string path)`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingGenerationDescriptor.cs#L42) | Reads and validates the result of a completed component generation request. |
| [`void Inno.Build.Toolchains.NativeBindingGenerationDescriptor.Write(string path)`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingGenerationDescriptor.cs#L61) | Atomically writes this completed generation to a request-owned descriptor file. |
| [`required string Inno.Build.Toolchains.NativeBindingGenerationDescriptor.bindingsPath`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingGenerationDescriptor.cs#L20) | Gets the absolute managed source path selected by the component project. |
| [`required string Inno.Build.Toolchains.NativeBindingGenerationDescriptor.bridgeDirectory`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingGenerationDescriptor.cs#L25) | Gets the complete native bridge directory, or an empty string for a direct C binding. |
| [`required string Inno.Build.Toolchains.NativeBindingGenerationDescriptor.fingerprint`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingGenerationDescriptor.cs#L15) | Gets the immutable input identity assigned to this target generation. |
| [`Inno.Build.Toolchains.NativeBindingGenerationDescriptor`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBindingGenerationDescriptor.cs#L10) | Describes the complete binding generation selected by one native build request. |

### `Inno.Build.Toolchains.NativeBuildContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.NativeBuildContext.NativeBuildContext(string engineRoot, string configuration)`](../../build/toolchains/Inno.Build.Toolchains/NativeBuildContext.cs#L33) | Creates a build context without consulting the tool assembly's checkout. |
| [`string Inno.Build.Toolchains.NativeBuildContext.GetNativeBuildRoot(System.Reflection.Assembly toolchainAssembly)`](../../build/toolchains/Inno.Build.Toolchains/NativeBuildContext.cs#L116) | Resolves native intermediates under the owning toolchain project in this checkout. |
| [`string Inno.Build.Toolchains.NativeBuildContext.configuration`](../../build/toolchains/Inno.Build.Toolchains/NativeBuildContext.cs#L91) | Gets the normalized debug or release configuration prepared by this operation. |
| [`string Inno.Build.Toolchains.NativeBuildContext.engineRoot`](../../build/toolchains/Inno.Build.Toolchains/NativeBuildContext.cs#L86) | Gets the absolute checkout used for every source, intermediate and output path. |
| [`Inno.Build.Toolchains.HostNativeToolchain? Inno.Build.Toolchains.NativeBuildContext.hostToolchain`](../../build/toolchains/Inno.Build.Toolchains/NativeBuildContext.cs#L97) | Gets the frozen host compiler and SDK selection, or null before host discovery. Browser toolchains resolve their own workload-specific SDK independently. |
| [`Inno.Build.Toolchains.NativeBuildStatistics Inno.Build.Toolchains.NativeBuildContext.statistics`](../../build/toolchains/Inno.Build.Toolchains/NativeBuildContext.cs#L134) | Gets actual hashing and native execution work accumulated by this operation and its scoped contexts. |
| [`Inno.Build.Toolchains.NativeBuildContext`](../../build/toolchains/Inno.Build.Toolchains/NativeBuildContext.cs#L11) | Identifies the checkout, configuration and frozen initial inputs owned by one native build operation. Create a new context for each operation; changes during its lifetime fail stability verification. |

### `Inno.Build.Toolchains.NativeBuildFingerprint`

| 当前声明 | 行为 |
| --- | --- |
| [`static string Inno.Build.Toolchains.NativeBuildFingerprint.Create(System.Collections.Generic.IEnumerable<string> declarations, System.Collections.Generic.IEnumerable<Inno.Build.Toolchains.NativeBuildInput> inputs, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildFingerprint.cs#L39) | Hashes a complete declared input closure independently of checkout location and file timestamps. |
| [`Inno.Build.Toolchains.NativeBuildFingerprint`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildFingerprint.cs#L13) | Derives recipe identities from ordered declarations, logical input names and complete source bytes. |

### `Inno.Build.Toolchains.NativeBuildInput`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.NativeBuildInput.NativeBuildInput(string logicalPath, string physicalPath)`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildInput.cs#L23) | Declares one file or directory consumed by a native recipe. |
| [`static Inno.Build.Toolchains.NativeBuildInput Inno.Build.Toolchains.NativeBuildInput.FromPath(string engineRoot, string path)`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildInput.cs#L61) | Declares a checkout-relative input, or an external SDK location whose path affects tool selection. |
| [`string Inno.Build.Toolchains.NativeBuildInput.logicalPath`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildInput.cs#L42) | Gets the identity included in the recipe fingerprint. |
| [`string Inno.Build.Toolchains.NativeBuildInput.physicalPath`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildInput.cs#L47) | Gets the reading location, which is not implicitly included in the fingerprint. |
| [`Inno.Build.Toolchains.NativeBuildInput`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildInput.cs#L9) | Separates the reproducible identity of an input from its physical reading location. |

### `Inno.Build.Toolchains.NativeBuildProduct`

| 当前声明 | 行为 |
| --- | --- |
| [`string Inno.Build.Toolchains.NativeBuildProduct.component`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildProduct.cs#L30) | Gets the component identity supplied by the owning toolchain. |
| [`string Inno.Build.Toolchains.NativeBuildProduct.directory`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildProduct.cs#L45) | Gets the immutable artifact root containing Outputs and its integrity manifest. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Build.Toolchains.NativeBuildProduct.files`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildProduct.cs#L50) | Gets the exact absolute output file paths validated by the publisher. |
| [`string Inno.Build.Toolchains.NativeBuildProduct.fingerprint`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildProduct.cs#L40) | Gets the content fingerprint covering sources, tool executables and toolchain declarations. |
| [`string Inno.Build.Toolchains.NativeBuildProduct.targetId`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildProduct.cs#L35) | Gets the target ABI selected for this build. |
| [`Inno.Build.Toolchains.NativeBuildProduct`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildProduct.cs#L11) | Identifies one complete, integrity-checked native artifact and its exact published file closure. |

### `Inno.Build.Toolchains.NativeBuildRecipe`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.NativeBuildRecipe.NativeBuildRecipe(System.Reflection.Assembly owner, string component, string targetId, System.Collections.Generic.IEnumerable<Inno.Build.Toolchains.NativeBuildInput> inputs, System.Collections.Generic.IEnumerable<string> declarations)`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildRecipe.cs#L38) | Creates a complete recipe without inferring implementation identity from an assembly MVID. |
| [`static Inno.Build.Toolchains.NativeBuildRecipe Inno.Build.Toolchains.NativeBuildRecipe.CreateForComponent(Inno.Build.Toolchains.NativeBuildContext context, System.Reflection.Assembly owner, string component, string targetId, System.Collections.Generic.IEnumerable<string> inputPaths, System.Collections.Generic.IEnumerable<string> declarations)`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildRecipe.cs#L114) | Declares a built-in component with its own implementation, common executor and frozen host SDK. |
| [`string Inno.Build.Toolchains.NativeBuildRecipe.component`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildRecipe.cs#L70) | Gets the component's artifact path segment. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Build.Toolchains.NativeBuildRecipe.declarations`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildRecipe.cs#L85) | Gets ordered non-file inputs; callers must explicitly sort unordered metadata before construction. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Build.Toolchains.NativeBuildInput> Inno.Build.Toolchains.NativeBuildRecipe.inputs`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildRecipe.cs#L80) | Gets the frozen declarations of physical inputs and their logical identities. |
| [`System.Reflection.Assembly Inno.Build.Toolchains.NativeBuildRecipe.owner`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildRecipe.cs#L65) | Gets the assembly identifying the intermediate owner, without using its MVID as a recipe input. |
| [`string Inno.Build.Toolchains.NativeBuildRecipe.targetId`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildRecipe.cs#L75) | Gets the target ABI's artifact path segment. |
| [`Inno.Build.Toolchains.NativeBuildRecipe`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildRecipe.cs#L12) | Freezes a component's input closure, ordered tool selection and intermediate owner. |

### `Inno.Build.Toolchains.NativeBuildStatistics`

| 当前声明 | 行为 |
| --- | --- |
| [`long Inno.Build.Toolchains.NativeBuildStatistics.hashedBytes`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildStatistics.cs#L16) | Gets the actual bytes consumed by input hashing, excluding output integrity validation. |
| [`long Inno.Build.Toolchains.NativeBuildStatistics.hashedFiles`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildStatistics.cs#L11) | Gets the number of complete file hashes read, including required stability verification. |
| [`long Inno.Build.Toolchains.NativeBuildStatistics.nativeProcesses`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildStatistics.cs#L22) | Gets native execution processes started through the context's frozen tool selection. SDK discovery and managed task bootstrapping are recorded by their separate build logs. |
| [`Inno.Build.Toolchains.NativeBuildStatistics`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeBuildStatistics.cs#L6) | Reports actual input reading and compiler process work accumulated by one build context. |

### `Inno.Build.Toolchains.NativeInputMaterializer`

| 当前声明 | 行为 |
| --- | --- |
| [`static System.Threading.Tasks.ValueTask<bool> Inno.Build.Toolchains.NativeInputMaterializer.CopyAsync(Inno.Build.Toolchains.NativeBuildContext context, string source, string destination, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeInputMaterializer.cs#L39) | Copies a declared input atomically when its destination differs, preserving identical file timestamps. |
| [`Inno.Build.Toolchains.NativeInputMaterializer`](../../build/toolchains/Inno.Build.Toolchains/Native/NativeInputMaterializer.cs#L13) | Materializes frozen native inputs without rewriting identical intermediate files. |

### `Inno.Build.Toolchains.ToolchainEnvironment`

| 当前声明 | 行为 |
| --- | --- |
| [`static System.Threading.Tasks.Task<string> Inno.Build.Toolchains.ToolchainEnvironment.CaptureOutputAsync(string fileName, System.Collections.Generic.IReadOnlyList<string> arguments, string workingDirectory, System.Threading.CancellationToken cancellationToken, System.Collections.Generic.IReadOnlyDictionary<string, string>? environment = null)`](../../build/toolchains/Inno.Build.Toolchains/ToolchainEnvironment.cs#L311) | Captures a tool's standard output while forwarding errors and preserving the common process lifecycle. |
| [`static bool Inno.Build.Toolchains.ToolchainEnvironment.ContainsAny(string value, params string[] needles)`](../../build/toolchains/Inno.Build.Toolchains/ToolchainEnvironment.cs#L419) | Determines whether a value contains at least one token without regard to casing. |
| [`static void Inno.Build.Toolchains.ToolchainEnvironment.DeleteDirectory(string path)`](../../build/toolchains/Inno.Build.Toolchains/ToolchainEnvironment.cs#L488) | Deletes one explicitly resolved toolchain output directory when it exists. |
| [`static string Inno.Build.Toolchains.ToolchainEnvironment.FindRepoRoot()`](../../build/toolchains/Inno.Build.Toolchains/ToolchainEnvironment.cs#L347) | Resolves the repository containing the currently executing toolchain assembly. |
| [`static string Inno.Build.Toolchains.ToolchainEnvironment.NormalizeOutputName(string fileName, string config)`](../../build/toolchains/Inno.Build.Toolchains/ToolchainEnvironment.cs#L446) | Normalizes a native artifact name and appends its build configuration. |
| [`static string Inno.Build.Toolchains.ToolchainEnvironment.ResolveExecutable(string name)`](../../build/toolchains/Inno.Build.Toolchains/ToolchainEnvironment.cs#L169) | Resolves one explicitly selected executable before it becomes a build input. |
| [`static System.Threading.Tasks.Task Inno.Build.Toolchains.ToolchainEnvironment.RunAsync(Inno.Build.Toolchains.NativeBuildContext context, string fileName, System.Collections.Generic.IReadOnlyList<string> arguments, string workingDirectory, System.Threading.CancellationToken cancellationToken)`](../../build/toolchains/Inno.Build.Toolchains/ToolchainEnvironment.cs#L94) | Runs a host-native process with the context's frozen compiler and SDK selection. |
| [`static System.Threading.Tasks.Task Inno.Build.Toolchains.ToolchainEnvironment.RunAsync(Inno.Build.Toolchains.NativeBuildContext context, string fileName, System.Collections.Generic.IReadOnlyList<string> arguments, string workingDirectory, System.Threading.CancellationToken cancellationToken, System.Collections.Generic.IReadOnlyDictionary<string, string> environment)`](../../build/toolchains/Inno.Build.Toolchains/ToolchainEnvironment.cs#L51) | Executes a declared absolute tool with an explicitly resolved SDK environment and records the operation cost. |
| [`static System.Threading.Tasks.Task Inno.Build.Toolchains.ToolchainEnvironment.RunAsync(Inno.Build.Toolchains.NativeBuildContext context, string fileName, string arguments, string workingDirectory, System.Threading.CancellationToken cancellationToken)`](../../build/toolchains/Inno.Build.Toolchains/ToolchainEnvironment.cs#L137) | Runs a quoted host command with a frozen compiler and SDK environment. |
| [`static System.Threading.Tasks.Task Inno.Build.Toolchains.ToolchainEnvironment.RunAsync(string fileName, System.Collections.Generic.IReadOnlyList<string> arguments, string workingDirectory, System.Threading.CancellationToken cancellationToken, System.Collections.Generic.IReadOnlyDictionary<string, string>? environment = null)`](../../build/toolchains/Inno.Build.Toolchains/ToolchainEnvironment.cs#L220) | Runs a child build with structured arguments, cancellation and hidden windows. |
| [`static System.Threading.Tasks.Task Inno.Build.Toolchains.ToolchainEnvironment.RunAsync(string fileName, System.Collections.Generic.IReadOnlyList<string> arguments, string workingDirectory, System.Threading.CancellationToken cancellationToken, System.Collections.Generic.IReadOnlyDictionary<string, string>? environment, System.IO.TextWriter standardOutput, System.IO.TextWriter standardError)`](../../build/toolchains/Inno.Build.Toolchains/ToolchainEnvironment.cs#L264) | Runs a child build with caller-owned output destinations and the shared process retirement protocol. |
| [`static System.Threading.Tasks.Task Inno.Build.Toolchains.ToolchainEnvironment.RunAsync(string fileName, string arguments, string workingDirectory, System.Threading.CancellationToken cancellationToken)`](../../build/toolchains/Inno.Build.Toolchains/ToolchainEnvironment.cs#L392) | Runs a child build using a complete argument string, without a shell. |
| [`static string Inno.Build.Toolchains.ToolchainEnvironment.TrimConfigSuffix(string baseName)`](../../build/toolchains/Inno.Build.Toolchains/ToolchainEnvironment.cs#L465) | Removes a trailing native debug or release token from an artifact stem. |
| [`static void Inno.Build.Toolchains.ToolchainEnvironment.ValidateConfiguration(string configuration)`](../../build/toolchains/Inno.Build.Toolchains/ToolchainEnvironment.cs#L332) | Validates the configuration shared by all native component builds. |
| [`Inno.Build.Toolchains.ToolchainEnvironment`](../../build/toolchains/Inno.Build.Toolchains/ToolchainEnvironment.cs#L16) | Provides deterministic host-process and workspace operations shared by native dependency toolchains. |

### `Inno.Build.Toolchains.ToolchainLayout`

| 当前声明 | 行为 |
| --- | --- |
| [`const string Inno.Build.Toolchains.ToolchainLayout.C_DEBUG_CONFIGURATION`](../../build/toolchains/Inno.Build.Toolchains/ToolchainLayout.cs#L21) | Identifies the normalized debug configuration token. |
| [`const string Inno.Build.Toolchains.ToolchainLayout.C_EXTERNAL_DIRECTORY_NAME`](../../build/toolchains/Inno.Build.Toolchains/ToolchainLayout.cs#L16) | Identifies the directory containing checked-out native dependency sources. |
| [`const string Inno.Build.Toolchains.ToolchainLayout.C_RELEASE_CONFIGURATION`](../../build/toolchains/Inno.Build.Toolchains/ToolchainLayout.cs#L26) | Identifies the normalized release configuration token. |
| [`const string Inno.Build.Toolchains.ToolchainLayout.C_REPOSITORY_MARKER_FILE`](../../build/toolchains/Inno.Build.Toolchains/ToolchainLayout.cs#L11) | Identifies the repository marker used while resolving a toolchain workspace. |
| [`Inno.Build.Toolchains.ToolchainLayout`](../../build/toolchains/Inno.Build.Toolchains/ToolchainLayout.cs#L6) | Defines stable repository-relative names shared by native dependency toolchains. |

### `Inno.Build.Toolchains.ToolchainWorkingDirectory`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Build.Toolchains.ToolchainWorkingDirectory.Dispose()`](../../build/toolchains/Inno.Build.Toolchains/ToolchainWorkingDirectory.cs#L92) | Removes the verified execution alias and releases ownership after tool processes have stopped. Physical files are retained; repeated disposal has no effect. |
| [`static System.Threading.Tasks.ValueTask<Inno.Build.Toolchains.ToolchainWorkingDirectory> Inno.Build.Toolchains.ToolchainWorkingDirectory.OpenAsync(string physicalPath, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/toolchains/Inno.Build.Toolchains/ToolchainWorkingDirectory.cs#L63) | Waits for exclusive use of the directory's execution alias and creates the physical directory. The alias has a stable identity so tool caches can be reused across completed operations. |
| [`string Inno.Build.Toolchains.ToolchainWorkingDirectory.toolPath`](../../build/toolchains/Inno.Build.Toolchains/ToolchainWorkingDirectory.cs#L33) | Gets the execution directory, which is a temporary Windows junction or the physical path. All processes using this path must stop before this owner is disposed. |
| [`Inno.Build.Toolchains.ToolchainWorkingDirectory`](../../build/toolchains/Inno.Build.Toolchains/ToolchainWorkingDirectory.cs#L15) | Owns a stable execution path for tools whose Windows filesystem APIs require bounded paths. The physical directory remains the caller's build-owned storage on every host. |

## 项目依赖

- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Core.IO](../core/Inno.Core.IO.md)：项目引用；公开签名可见性由语义边界检查确认。

共同 MSBuild 注入的 analyzer 与编译规则属于构建依赖，完整有效项目图记录在本轮验收证据中。
