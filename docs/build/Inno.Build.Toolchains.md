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
