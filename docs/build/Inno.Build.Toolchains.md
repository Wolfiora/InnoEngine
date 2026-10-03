# Inno.Build.Toolchains

[Build 索引](README.md) · [Wiki 首页](../README.md) · [宿主组合](Inno.Build.Toolchains.Host.md) · [Native](../native/README.md)

## 公开 API

- `ToolchainEnvironment`：建立受控工具进程环境。
- `NativeBuildContext(engineRoot, configuration)`：验证并冻结显式 checkout 和 `debug`/`release` 配置；公开 `engineRoot`、`configuration` 与 `GetNativeBuildRoot(Assembly)`。
- `BuildArtifactOptions`：描述 debug/release 与目标 artifact 选择。
- 其公开构造参数/只读成员为 `buildDirName`、`libraryTokens`、`extensions`、`requiredPathTokens`、`normalizeOutputName`。
- `BuildArtifactCopier`：复制明确的 Native 产物。
- `ToolchainLayout`：解析仓库内 toolchain/source/output 布局。

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
ImGui 的受校验源码 overlay 也在工具链缓存内，第三方源保持只读。BGFX 的上游 GENie
工程仍使用其声明的 `.build` 路径（不属于 CMake 工程），最终产物统一进入 `.lib`。

Windows 的 `msbuild` / `cl` 通过 Visual Studio 官方 `vswhere` 发现 C++ 工具安装；
编译器需要的 PATH、INCLUDE、LIB 和 LIBPATH 仅进入子进程，不要求用户打开 Developer Terminal，
不修改当前进程或系统环境。缺少 C++ Build Tools 明确失败。CMake 与 Windows MSBuild 顺序编译。

该项目只服务构建机器，不进入 Runtime 或 Player。路径缺失、产物歧义和进程失败必须明确抛出，不使用 fallback tool location。`BuildArtifactCopier.CopyArtifacts` 按最接近产物的 Debug/Release 目录或文件后缀选择目标配置；没有目标文件或多个源落到同一输出名时直接失败，绝不把已有 Debug DLL 当作 Release 产物发布。

没有供外部派生者使用的 protected 扩展点。ToolchainLayout 的全部公开常量为 `C_REPOSITORY_MARKER_FILE`、
`C_EXTERNAL_DIRECTORY_NAME`、`C_OUTPUT_DIRECTORY_NAME`、`C_DEBUG_CONFIGURATION`、`C_RELEASE_CONFIGURATION`。

```csharp
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.Sdl3;

await Sdl3Toolchain.BuildAsync(new NativeBuildContext(engineRoot, "release"), cancellationToken);
```
