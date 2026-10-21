# MSBuild 任务加载与开发入口验收

[记录索引](../README.md) · [当前验收](../../../docs/architecture/CURRENT_ACCEPTANCE.md)

日期：2026-10-21。SDK：10.0.402，MSBuild：18.9，Rider：2026.1.2，宿主：Windows x64。

## 已确认的原因

### Task 注册与执行属性来自不同求值

原始 `MSB4062` 指向 `RegisterTaskPublisherTask`。六个 Native 消费者的共同引导已经把 DLL 编译、复制到一个随机目录，任务注册却指向另一个随机目录。

使用公开 `ProjectInstance`、`BuildManager` 和进程外 worker 可以独立复现：预求值属性传入 worker，worker 同时重新求值任务注册。求值阶段的 `Guid.NewGuid()` 因此不能被当作跨这两个状态的共同身份。直接从项目路径启动的 SDK 构建未必触发这一状态组合；它通过不能证明 IDE 的预求值构建也通过。

日志中的绝对路径由当前 checkout 与输出位置生成，不表示 Git 固定了开发机器的路径。重克隆、删除全部缓存或者改变 BGCS 不能修复上述身份分裂。

### 普通输出被引导程序集锁定

owner 引导使用 `Assembly.LoadFrom` 打开普通 Foundation 输出。长寿命 IDE 构建宿主可以继续持有该文件，后续 SDK 编译因而出现 `MSB3026` / `MSB3027`。源文件未损坏，缓存也不是这一失败的判断依据。

### 入口没有实际构建其平台产品

修复任务加载后，Rider 的实际构建继续暴露 `CS0006`：所选平台 Editor 的引用程序集未生成。外层 Solution 调度器关闭引用构建，而平台产品不作为另一个 Solution 构建根。入口必须通过标准 SDK 引用构建完成自身产品闭包。

SDK 的 `ResolveProjectReferences` 还会在 `BuildingInsideVisualStudio=true` 时只查询引用产物；仅设置 `BuildProjectReferences=true` 不足以取得引用构建所有权。带这两个全局属性的独立冷输出构建已复现缺失引用。正式入口在非 design-time 状态声明 SDK 拥有自身引用图，并把同一策略传递给引用；design-time 保留父调度器上下文。

### CMake 工作区复用了另一个 TEMP 下的绝对路径

Rider backend 日志明确记录：当前 `C:/Users/23842/AppData/Local/Temp/InnoTools/.../CMake` 与缓存中的 `D:/InnoTmp/bgcs301-a7ec1155/InnoTools/.../CMake` 不同，SDL 的源目录也不一致。两者指向相同物理工作区，但旧执行 alias 根据父进程 TEMP 选择位置。CMake 中间态会保留绝对路径，因而拒绝继续配置。这不是 BGCS 生成源码或 Git 仓库写死 checkout 路径。

### 私有运行库的复制进入了项目输出重放清单

Rider backend 的实际 `RdCopyTaskResult` 保存了 `PrepareInnoBuildTaskHost` 的 127 条私有加载复制，以及 publisher bootstrap 的临时路径。部分 bootstrap 源目录已经随进程退休而不存在。这类复制不能作为脱离原 owner 的可重用项目输出。当前截图中全部复制诊断没有完整写入 SDK FileLogger，不能仅根据条数断言每条错误的具体路径；共同准备流程仍必须消除这一生命周期错配。

### 并发绑定消费使用固定等待上限

独立测试构建与产品 Native 准备重叠时，`MaterializeBindingsTask` 在组件 `generation.lock` 等待 30 秒后失败。生成 batch 的 owner 可以覆盖耗时原生准备；消费端不能据此认定 publisher 已失败，也不能占用 MSBuild 节点阻碍其继续执行。失败构建日志保留在 `final-regression-concurrent-host-lock.log`。

## 统一实现

- SDK inline task 捕获当前任务注册使用的加载身份。publisher 复制、Task runtime 发布和消费者路径绑定同一身份；没有 IDE、操作系统或 SDK 版本分支。
- Foundation 路径取自 SDK Build 返回值，保留明确选择的 `ArtifactsPath`，删除对普通 bin 和固定 TFM 路径的拼接。引导从文件字节执行公共 owner 协议，不锁定 SDK 输出文件。
- Editor entry 使用 `TreatAsLocalProperty` 和 `ProjectReference` metadata 声明自己的正常引用构建。Design-time 保留外层策略；程序仍在主线程直接调用平台入口，没有运行时构建或第二个启动器。
- 私有运行库复制由生命周期准备 adapter 直接调用 SDK Copy 实现，保留原有复制语义，不生成可独立重放的项目 Copy task；普通项目输出策略不变。
- Windows 编译执行 alias 尊重宿主 TEMP；执行锁保留在临时根内，实际执行根进入 workspace 身份。不同执行根的进程可共享不可变最终产物，各自使用路径兼容的中间态。非 Windows 宿主继续直接使用真实目录，没有 IDE 专用构建路径。
- 绑定消费使用相同 Core IO owner，等待时 `Yield/Reacquire`。可变 HostSource 等待由构建取消令牌控制；取消保留 publisher 和已有消费副本，节点重新取得失败时释放已取得的 owner。未删除锁或绕开完整性检查。
- 独立加载目录、内容指纹、完整运行闭包、Core IO lease、取消和原子发布规则保留。BGCS、Native facade、第三方源码及插件未修改；未手改生成文件。

实现位置：

```text
build/msbuild/Inno.Build.Tasks.targets
build/tasks/Inno.Build.TaskHosting/TaskRuntimeBootstrapOwner.cs
build/tasks/Inno.Build.TaskHosting/MaterializeBindingsTask.cs
entry/InnoEngine.Editor/InnoEngine.Editor.csproj
build/toolchains/Inno.Build.Toolchains/
  ToolchainWorkingDirectory.cs
  Native/NativeWorkspaceIdentity.cs
tests/build/Inno.Build.Tests/
  TaskRegistrationBuildTests.cs
  DevelopmentEntryBuildTests.cs
  TaskPublisherBootstrapTests.cs
  ToolchainWorkingDirectoryTests.cs
  ToolchainWorkingDirectoryBuildTests.cs
  BindingMaterializationTests.cs
docs/build/Inno.Build.TaskHosting.md
docs/entry/InnoEngine.Editor.md
docs/build/Inno.Build.Toolchains.md
```

## 验证与证据

原始命令、完整日志、TRX 和进程结果位于忽略目录 `artifacts/acceptance/rider-task-path-2026-10-21/`；独立 SDK 输出位于 `D:/InnoEngine-acceptance/rider-task-path-2026-10-21-8ab67d42/`。

| 检查 | 当前证据 |
| --- | --- |
| 最小进程外重现 | 修复前任务注册身份与执行属性不同，失败；绑定注册身份后通过。 |
| 真实完整 Task runtime 的预求值/进程外准备 | 通过，`task-host-result.json`；实际执行身份不同于初次求值，publisher 和完整 runtime 均发布到正确位置。 |
| 架构与有效项目图 | 通过，233/233；`architecture-result.json`。未执行 build targets。 |
| 相关回归 | 最终代码 92/92 通过，0 失败、0 跳过；`final-regression-result.json`、`results/final-build-regression.trx`。实际用例耗时及关键结果另见 `test-evidence-summary.json`。 |
| Editor SDK 入口 | 最终代码完整构建通过，8 分 33.12 秒，SDK 汇总 0 警告、0 错误；`editor-sdk-build-result.json`。外层明确传入 `BuildProjectReferences=false` 与 `BuildingInsideVisualStudio=true`，entry 通过自己的 SDK 引用图完成产品构建。原生工具的 C4819 等提示保留在日志中，不声称原生编译没有告警。 |
| FlappyBird | 最终完整构建之后，D3D11、D3D12、Vulkan 各完成 3,000 帧并正常退出；日志及 `flappybird-*-result.json` 保留。使用隐藏窗口，不计作视觉、焦点、DPI 或音频验收。 |
| Rider UI | 未执行；用户明确禁止 computer use。SDK 带 IDE 全局属性构建及进程外 worker 测试不等同于实际 Rider 再次启动。 |
| 手写 managed 源码与工程盘符扫描 | `build`、`entry`、`src`、`platforms`、`backends` 的 `.cs/.csproj/.props/.targets` 无硬编码盘符匹配，排除产物、Bindings/Generated、Native 和 extern；见 `path-source-scan.json`。本次没有修改这些排除目录。 |

回归通过真实 SDK/public boundary 验证：预求值项目转移到 worker；连续构建的独立身份；worker 存活期间 Foundation 输出可写；并行菱形项目闭包的共享编译与独立加载；Windows/macOS 宿主的普通和 design-time 工程求值。工程求值不代表 macOS 实机运行。

最终完整构建实际准备 8 个 Native product，执行 8 个 Native 进程、1 个 binding batch、4 次 binding generation；Shader 工具命中有效产物，执行 0 个 Native 进程。此轮存在完整哈希与原生重新编译，不把 8 分 33.12 秒当作纯热构建基线，也不据不同输入的单次耗时声明性能收益。

本机系统枚举 AMD Radeon(TM) Graphics（31.0.14084.6002）及 NVIDIA GeForce RTX 3070 Ti Laptop GPU（32.0.15.9649）。产品日志确认三次实际初始化的 API；日志没有记录选中 adapter 名称，因此不把结果归因于其中某一块 GPU。

绑定消费争用测试实际持有 publisher 32 秒后才释放，consumer 仍在等待，随后复制刚提交的内容。取消测试确认 publisher 独占状态、已有消费 bytes 均保留；节点重新取得失败测试确认 owner 已释放。该等待遵守构建取消，不改变 Plugin reload 或 ALC 退休的超时规则。

两个不同 TEMP 的进程与再次返回第一个 TEMP 的进程，使用同一 Native publisher 和真实 CMake，分别验证路径隔离与工作区复用。探索过的用户目录 alias 在本机触发 junction 文件重命名失败，因此没有采用；相关失败日志保留，不记为通过。

新增 CMake fixture 首次运行因 Lambda 参数使用上下文关键字 `scoped` 而编译失败，随后改为语义明确的 `buildContext`；原 84 通过、1 失败的 TRX 保留为 `before-fixture-correction.trx`，不能冒充最终通过记录。

## 重复验证入口

从仓库根目录执行，路径由当前 SDK、仓库及调用方明确选择的输出根取得。以下使用普通工程输出；本次独立验收另外通过 SDK `--artifacts-path` 和 `InnoTaskArtifacts` 参数隔离输出，具体值只记录在本地证据文件中。

```powershell
dotnet build entry/InnoEngine.Editor/InnoEngine.Editor.csproj -c Debug --disable-build-servers -m:1 -nr:false -p:UseSharedCompilation=false -p:BuildProjectReferences=false -p:BuildingInsideVisualStudio=true -warnaserror

$classes = @(
    'TaskRegistrationBuildTests', 'TaskPublisherBootstrapTests', 'DevelopmentEntryBuildTests',
    'TaskRuntimeBuildTests', 'TaskHostPublicationTests', 'SolutionBootstrapOwnershipTests',
    'ToolchainWorkingDirectoryTests', 'ToolchainWorkingDirectoryBuildTests',
    'NativeWorkspaceReuseTests', 'NativeArtifactPublicationTests', 'NativeVerificationPhaseTests',
    'ToolchainEnvironmentTests', 'ToolchainProcessTests', 'NativeArtifactIdentityTests',
    'NativeBuildContextTests', 'ProductNativeCacheLookupTests', 'BindingMaterializationTests'
)
$filter = ($classes | ForEach-Object { 'FullyQualifiedName~' + $_ }) -join '|'
dotnet test tests/build/Inno.Build.Tests/Inno.Build.Tests.csproj -c Debug --disable-build-servers -m:1 -nr:false -p:UseSharedCompilation=false -warnaserror --filter $filter -- xUnit.ParallelizeTestCollections=false xUnit.MaxParallelThreads=1
```

本次不是全仓库全部测试或完整平台发布矩阵复跑。macOS/Browser、Rider 实际窗口、视觉和音频继续按各自边界记录。

## 资源与保留状态

验收期间发现把执行锁移到物理目录旁会向不可变工具产物增加 `Outputs.execution.lock`，触发严格完整性拒绝。该过渡实现已经撤回，锁继续留在临时根内；新增回归在 alias 存活及释放后都核对完整产物清单。对实际发布的 SDK Task runtime 运行独立公开边界探针，也确认打开工具目录后清单保持有效。未采用跳过完整性验证或删除告警的路径。

中途因 C 盘接近 50 GiB 保留线取消了若干独立构建，包括使用宿主默认 TEMP 的完整重复构建；这些取消不记为通过。守护阈值为 51 GiB，只终止本次所属进程树，未观察到低于用户要求的 50 GiB。使用现有公开维护协议预检后，按磁盘压力退役未被引用的构建产物；候选绝对路径均核对在仓库 artifacts 或组件 obj/native 下。维护日志保留实际逻辑字节、跳过和失败数；hardlink 的逻辑大小不等于实际释放量。当前受保护产物、验收日志与源文件保留，未清理 SDK、NuGet、页文件或用户应用。维护为资源收尾，不要求重克隆或全删缓存。

最终收尾使用同一公开 cache retirement 协议处理本次独立输出根里的已退出 publisher 和 private loads，保留被引用的 5 个完整 Task host。退役逻辑字节 2,244,005,888，0 失败，见 `private-runtime-retirement.log`；未删除 Editor 输出、源码、TRX 或产品日志。

保留开始时已有的四个测试锁文件差异。Windows 无法播放 `/System/Library/Sounds/Glass.aiff`。
