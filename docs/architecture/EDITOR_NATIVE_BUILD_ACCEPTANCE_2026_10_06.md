# Editor 普通构建与原生部署修复验收

[架构索引](README.md) · [Editor Application](../editor/Inno.Editor.Application.md) · [Host 工具链](../build/Inno.Build.Toolchains.Host.md) · [MSBuild Task](../build/Inno.Build.Tasks.md)

## 原因与证据

用户使用 Samples 的 FlappyBird 启动 Debug Editor，`TextNative` 初始化缺少 `inno_text_GetLastError`。
实际加载位置为 `Inno.Editor.Application/bin/Debug/net9.0/native/text/windows-x64/inno-text-debug.dll`。
该文件修改时间为 2026-09-26，SHA-256 为 `FB386DB214702FAC674D3236B220B021FFF36016C75F325B0CD5AD75FC283102`。
`dumpbin /exports` 显示其仍为旧的八个 `inno_text_create` 等符号；当前 Cpp2C 桥和托管绑定使用
`inno_text_RuntimeCreate`、`inno_text_GetLastError` 等符号。

当前 Native CMake 已编译生成的 `Classes.cpp`，Windows export 声明正确。已有 Release DLL 也包含所需符号。
缺口在构建入口：此前 CLI `engine` 和 Publish 会准备并部署原生库，普通 IDE/MSBuild 只更新托管输出，
保留历史 Debug DLL。严格绑定初始化因此正确拒绝不匹配的原生 ABI。

## 实现与边界

- Editor 普通 Build 在成功返回前调用现有 `PrepareEditorNativeTask`，按 `Configuration` 准备八个 product，并安装到实际 `TargetDir`。
- 设计期构建跳过原生准备；关闭 Editor 的 IDE 快速跳过检查，让原生输入进入实际 MSBuild 校验。
- Publish 复用同一 Task；CLI 删除重复的原生准备、安装及固定 `bin/<configuration>/net9.0` 路径，复用 Editor Build。
- Task 宿主输出属于 `artifacts/build-tools/editor`，不增加 Editor 的 runtime ProjectReference。
- `HostNativeDeployment` 复用 Core.IO 的 FileLease、PathBoundary 和 AtomicDirectory。完整校验 product，再比较精确文件集合、SHA-256 与 Unix 权限；相同部署不复制、不替换 DLL。
- 缺失、多余、损坏或变化的部署整体替换；取消、产品变化或安装失败明确失败。持有待更新 DLL 的进程需要退出后再构建。
- Native facade、BGCS、生成绑定、服务层和渲染算法均无需修改；未增加旧符号兼容或运行时源码/cache 搜索。
- AGENTS 第 21 节已规定架构优先；第 25 节补充普通构建、Publish、Loader 与一致部署的职责约束。

## 当前验证

环境：Windows x64，工程 SDK .NET 9.0.318，VS 2022 Build Tools。
证据目录：`artifacts/acceptance/2026-10-06-editor-native`。

| 验证 | 结果与证据 |
| --- | --- |
| 普通 Debug Build | 成功；`build-debug.log`。第一次准备此前缺失的 Debug 缓存。顶层 managed 构建 0 错误；第三方 Native 编译警告另见下文。 |
| 模拟 IDE 的重复 Build | 使用 `BuildProjectReferences=false`、`BuildingInsideVisualStudio=true`，成功；`build-debug-ide-repeat.log`。Task 显式准备自己的引用，清除 IDE/solution 调度属性。 |
| 部署复用 | 完整部署全部文件的 SHA-256、长度与写入时间不变；`deployment-repeat-result.json`。 |
| 实际 Debug Editor | Samples/FlappyBird，D3D11，120 帧，exit 0；8 个 Panel，首帧完成、正常保存与 Dispose；`editor-debug-boot.log`、`editor-debug-result.json`。Smoke 入口检查当前错误 Diagnostic，为零才成功。 |
| 普通 Release Build | 成功；`build-release.log`。通过同一构建目标准备独立的 Release 闭包。 |
| 实际 Release Editor | 同一 FlappyBird、D3D11、120 帧、exit 0；8 个 Panel，完整启动、渲染和正常退出；`editor-release-boot.log`、`editor-release-result.json`。 |
| 部署公开契约消费者 | 10 项通过；`deployment-consumer.log`。覆盖初次安装、import library 排除、锁定文件的相同部署、损坏/缺失/多余文件、并发、取消、篡改 product 拒绝与 staging 清理。 |
| 现有原生构建/部署回归 | `NativeArtifactPublicationTests` 与 `NativeBuildContextTests`：11 passed，0 failed，0 skipped。 |
| 现有 Text/UI 回归 | Text 3 passed，UI 12 passed；均使用本次 Release Editor 的完整原生部署。 |
| 现有 ImGui/SDL 回归 | ImGui 3 passed，SDL 5 passed；使用同一本次部署。 |
| CLI 编译 | 当前修改的唯一构建入口成功编译，0 warning、0 error；`build-cli.log`。 |
| 设计期跳过 | `dotnet msbuild ... -t:PrepareEditorNativeOutput -p:DesignTimeBuild=true` 成功，不启动 Task 或原生工具链；`design-time.log`。 |
| 架构验证 | 修改后的 CLI 再次检查全部生产源码、依赖、公开 XML 和排版，通过；`architecture-final.log`。 |
| 文档与差异 | Markdown 相对链接检查通过；`git diff --check` 通过。 |

现有测试共 34 passed、0 failed、0 skipped，TRX 位于证据目录的 `results/`；另有上述 10 项独立部署验收。
没有修改测试源码。证据汇总为 `regression-summary.json`，环境与仓库 revision 为 `environment.json`。

本次普通 Debug 首次构建 26 分 9 秒、Release 首次构建 22 分 24 秒，包含该 Task 输出身份下的首次原生与工具准备。
模拟 IDE 的重复 Debug 构建 3 分 13 秒，没有重新安装任何原生文件。当前每次实际 Build 仍会执行工具依赖检查和完整源/产物指纹校验；
该时间不能描述为“即时构建”。本轮优先保证当前源码、绑定与部署一致，不放宽校验以跳过过期产物。

首次 Native 构建保留第三方源码的 C4819、C4245、C4701、C4702、MSB8029、LNK4217 等 warning；未修改 `extern`。
消费者模板初次直接以 `.project.xml` 编译时，SDK 未按 C# 项目导入语言 targets；该验收脚本问题的日志保留为
`deployment-consumer-build-invalid-extension.log`。正式执行把同一模板复制为独立临时 `.csproj`，10 项全部通过，
没有将该失败算作产品测试通过，也没有在仓库中放置额外的生产或测试项目。

## 可重复的验证命令

```powershell
dotnet build src/composition/editor/host/Inno.Editor.Application/Inno.Editor.Application.csproj -c Debug --disable-build-servers -m:1 -nodeReuse:false
dotnet build src/composition/editor/host/Inno.Editor.Application/Inno.Editor.Application.csproj -c Debug --disable-build-servers -m:1 -nodeReuse:false -p:BuildProjectReferences=false -p:BuildingInsideVisualStudio=true
dotnet build src/composition/editor/host/Inno.Editor.Application/Inno.Editor.Application.csproj -c Release --disable-build-servers -m:1 -nodeReuse:false
src/composition/editor/host/Inno.Editor.Application/bin/Debug/net9.0/Inno.Editor.Application.exe C:/Dev/GameEngineDev/InnoEngine.Samples/FlappyBird --graphics-api d3d11 --smoke-frames 120
src/composition/editor/host/Inno.Editor.Application/bin/Release/net9.0/Inno.Editor.Application.exe C:/Dev/GameEngineDev/InnoEngine.Samples/FlappyBird --graphics-api d3d11 --smoke-frames 120
dotnet run --project build/cli/Inno.Build.Cli/Inno.Build.Cli.csproj -c Release --no-build -- verify . --configuration Release
```

部署消费者与回归执行脚本保存在证据目录，可查看完整 fixture 与命令。测试仅复制本次已经验证的 Editor 原生部署，
不从历史 artifact 中猜测库位置。消费者通过公开 API 生成独立 fixture，未使用反射或 internal 后门。

## 验证范围

本轮验证普通 Debug / Release 构建、部署契约与 Windows FlappyBird Editor 启动。
macOS 使用相同 Build/Task/部署流程，本机不能替代 macOS 实机验收。
不将本次启动修复扩展为全引擎、所有平台的完整功能验收。
本次没有自动提交。Windows 无法访问指定的 `/System/Library/Sounds/Glass.aiff`，提示音未播放。
