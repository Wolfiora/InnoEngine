# 构建成本、工具宿主与磁盘收口验收

[执行计划](../plans/BUILD_COST_TOOL_HOST_DISK_CLOSURE_PLAN.md) · [当前验收](../../../docs/architecture/CURRENT_ACCEPTANCE.md)

## 最终结论

2026-10-14，基线 `b488d5d8588ec56e6ca1ab50bb1f30c680e81e8f` 后的本轮源码整改与非 GUI 本机门禁通过，源码可以提交。没有自动提交。**200,000,000,000 字节可用空间目标未完成**，历史目录删除被自动审批阻断；这不写成磁盘结案。GUI、焦点、视觉和声学按用户要求不执行，MacOSArm64 实机未实测。

当前检查没有发现本轮范围内尚需修复的 P1/P2。该结论受下文测试和环境范围限制，不表示引擎、第三方库或其他机器绝无问题。

所有原始证据归属 `artifacts/acceptance/build-cost-tool-host-disk/20261013-implementation/`。`acceptance-matrix.tsv`、`file-map.tsv`、`wiki-page-map.tsv`、`wiki-api-map.tsv`、`wiki-example-map.tsv` 给出逐项映射；失败日志和修改前检查点继续保留。

## 问题与实现结果

| 问题 | 当前处理与结果 |
| --- | --- |
| 热 Native 重复 SDK 读取 | 完整热命中先固定候选，释放预读 reader，按规范路径取得全部 workspace 锁，再取得完整 reader；使用新的 union verification，阶段内物理输入去重，逻辑身份和顺序保留。缺失/损坏释放完整闭包，进入原冷路径。 |
| Task 命中后仍编译 | 输入快照与私有 owner 分离；成功 publication 在 SDK Build 前取得 activity/read 保护、验证完整文件与哈希、登记私有 load 依赖后交付。Build registry 只保存 BCL 数据。 |
| 工具 collectible inspection / JIT 规避 | 删除 InspectionContext 与 TieredCompilation=false；原 Build 可执行入口启动固定 inspect/publish worker，默认 JIT，BCL 请求/响应、nonce、明确 SDK、十分钟 deadline、取消进程树退休。 |
| PID 复用和无主临时目录 | Core.IO 保存 PID + UTC 启动 ticks；Active/Exited/Unknown 保守查询。新的 claim 在 payload 首次写入前建立；子进程登记与退休共用 lease；现有未登记目录拒绝认领。 |
| workspace 永久保护 | 明确 ImmutableArtifact、ReusableWorkspace、ProcessWorkingDirectory 三种 role；当前产物和活动依赖保护，空闲 workspace 可以在压力下退休。 |
| 同一次构建的工具身份漂移 | Roslyn generator 使用中立 lock，不继承产品 RID/ABI；私有工具编译不保留共享 compiler server owner，修复真实 CS2012。 |
| --dotnet 未作用于工具准备 | Bootstrap 和实际命令共用所选 SDK，非法/重复/缺值 SDK 在 checkout 操作前拒绝。Browser 同一显式 SDK 的第二次 Pack 请求不重新编译工具。 |
| 新 owner JSON 不满足 NativeAOT | Core.IO 改为显式 JsonDocument/Utf8JsonWriter，不依赖反射 JSON；重新 NativeAOT 发布、实际运行通过。 |
| CLI 裁剪脚本引用缺少 Animation | 真实 CLI 根闭包增加 Animation.Assets 实现引用，不维护类型白名单；实际重新生成裁剪 references、脚本编译与 190 个 Wiki 示例通过。 |
| 磁盘清理 | 只通过注册 owner 清理，保留当前产品/Pack、reader 和验收证据；37 个审批拒绝目录保留。空间目标未达成。 |

## 文件与程序集边界

```text
entry/
  InnoEngine.Build/                       唯一 Build/worker 可执行入口
  InnoEngine.Editor/                      唯一 Editor 可执行入口
  Inno.Entry.Bootstrap/
    DevelopmentBuildWorker.cs
    DevelopmentBuildWorkerRequest.cs
    DevelopmentBuildWorkerProcess.cs
    DevelopmentBuildRequest.cs            明确 SDK 请求
src/foundation/core/Inno.Core.IO/
  ProcessOwnerStamp.cs
  ProcessOwnerState.cs
  ProcessOwnedDirectory.cs
  ProcessOwnedDirectoryDescriptor.cs
build/toolchains/Inno.Build.Toolchains/
  Native/ProductNativeCacheLookup.cs      internal 完整热命中执行
  Native/NativeInputReadCache.cs           每阶段物理读取去重
  Native/NativeBuildRecipe.cs              backend 自有依赖声明
  Cache/BuildCacheArtifactRole.cs
  Cache/BuildCachePublisher.cs
  Cache/BuildCacheRetirement.cs
build/tasks/Inno.Build.TaskHosting/
  TaskRuntimeBootstrapOwner.cs            SDK adapter
  TaskRuntimeRequestKey.cs
  TaskRuntimePreparation.cs
  BuildTaskRuntimeTask.cs
build/cli/Inno.Build.Cli/                  实际创作闭包和固定源码编译
build/distributions/.../StandardBuildCaches.cs
tests/build/Inno.Build.Tests/             成本、owner、worker、SDK 契约
tests/editor/Inno.Editor.Hosting.Tests/   窗口可见性命令契约
docs/                                    当前行为与全部 API/示例
records/architecture/                     执行计划及本报告
```

依赖保持单向：Bootstrap → Core.IO；Core.IO 不依赖 Build。Toolchains → Core.IO、中立构建契约。TaskHosting → Toolchains、MSBuild 公开 API。标准发行贡献实际 backend/平台配置和 owner。Player runtime closure 不包含 Build、Editor、BGCS generator 或 Task；Browser/Windows/Mac 产品与共享 Player 的闭包分别为 67/69/69/51 个项目。

新增公开表面只服务实际跨程序集生命周期：

| API | 必要性与稳定语义 |
| --- | --- |
| ProcessOwnerStamp / ProcessOwnerState | 表达精确进程生命周期；不可确认状态不能授权删除；不持有 Process，不导出到脚本。 |
| ProcessOwnedDirectory / Descriptor | 通用文件工作 owner，首次 payload 前建立 claim，当前子进程协同与异常退出回收；不依赖具体构建组件。 |
| BuildCacheArtifactRole、descriptor.role、publisher role | 明确产物与工作目录退休规则，不能以“最近使用”替代活动所有权。 |
| BuildCachePublisher.BeginWorkingDirectory | 在生产工作目录写入前登记并校验，不认领任意历史目录。 |
| NativeBuildRecipe.WithDependency / ProductNativeBuildStep.bindDependencies | backend 声明实际借用依赖产物，热查询与真实编译消费同一 recipe；共享执行器不识别组件名单。 |
| TaskRuntimeBootstrapOwner | 必要的 MSBuild adapter，仅动态调用 Foundation 公开契约，避免 SDK inline compiler 引入完整产品依赖；不反射穿透私有实现。 |
| EditorCommandLineOptions.windowVisible | 真实 Host 的显式运行选项，默认 true；后台运行仍使用完整产品生命周期。 |

其余 worker、查找、请求快照和物理去重保持 internal；没有新增生产 Program、空项目、兼容 wrapper、旧 reader 或测试后门。

## 环境与执行方式

- Windows X64；系统 .NET SDK 9.0.318 / runtime 9.0.20。
- Web 使用已有 `artifacts/tools/dotnet/9.0.318/dotnet.exe`，wasm-tools 9.0.20/9.0.100；系统 SDK 未安装该 workload，不能偷偷推导替代。
- Web 构建子进程 PATH 明确加入 VS 2022 BuildTools 的 CMake/Ninja；不改用户全局环境。
- 隔离 headless 使用已安装 Edge，独立 context、临时本地服务，不控制用户现有浏览器。
- SDK 构建串行，`--disable-build-servers -m:1 -nodeReuse:false`；具体参数、UTC、耗时和 exit code 在各 `*-commands.json`。
- 产品使用独立 FlappyBird 验收副本，测试脚本/存储探针归属本轮 artifacts；不修改 extern/BGCS 源码或生成绑定。

主要命令如下，`<Project>`/`<SDK>`/`<Output>` 的实际绝对值见证据 JSON：

```text
dotnet build InnoEngine.sln --configuration SourceDebug --disable-build-servers -m:1 -nodeReuse:false
dotnet build InnoEngine.sln --configuration SourceRelease --disable-build-servers -m:1 -nodeReuse:false
dotnet test tests/tooling/Inno.Tooling.Architecture.Tests/Inno.Tooling.Architecture.Tests.csproj --no-restore ...
dotnet test tests/build/Inno.Build.Tests/Inno.Build.Tests.csproj --no-restore ...
dotnet InnoEngine.Build.dll editor --configuration Debug
dotnet InnoEngine.Build.dll support-pack --target windows-x64
dotnet InnoEngine.Build.dll support-pack --target browser-wasm --dotnet <SDK>
dotnet InnoEngine.Build.dll scripts --project <Project> --tools-target windows-x64 --output <Output>
dotnet InnoEngine.Build.dll game --project <Project> --target <Target> --deployment <Deployment> ...
dotnet InnoEngine.Build.dll verify <Root> --wiki --wiki-runtime-reference <ActualCliRuntime> --wiki-editor-reference <ActualCliEditor> ...
dotnet InnoEngine.Build.dll clean --scope cache --cache-time-seconds 600 --dry-run
dotnet InnoEngine.Build.dll clean --scope cache --cache-time-seconds 600
```

## 正确性与工程门禁

| 门禁 | 新证据 |
| --- | --- |
| 最后 SourceDebug / SourceRelease | 61.875 / 59.406 秒，均 0 errors / 0 warnings；包含最后 CLI Animation 引用修复。 |
| Build 契约 | 初轮 320 个唯一用例含 7 个失败；修正当前夹具后受影响 23 项通过。加上 SDK 4 项、CLI 根引用 1 项，按 testName 合并为 **325 个唯一用例、剩余失败 0**，不是重叠次数求和。 |
| Hosting | 10/10；后台运行参数和默认可见性通过。 |
| 架构 | 最后 122/122；严格 CLI verify exit=0。 |
| worker | 30 次正常 fixture、10 次真实身份检查；nonce、崩溃、kill、取消及真实十分钟截止通过。deadline 实际 602.135 秒。 |
| Native / owner / Task | 损坏/多余/缺失输出、同长度同 mtime、输入增删、锁后/编译变化、取消、并发、PID 复用和未知 owner 保护通过。 |
| 小 Native 修改 | TextRuntime.cpp 临时注释触发增量编译，原字节与 mtime 已恢复，git diff=0；恢复后热命中。 |
| Solution | 230 个实际项目全部在 Solution；空 Folder、依赖循环、重复生产程序集为 0。三个同名 TestModule 是 Invalid/V1/V2 替换测试夹具，有意保留。 |
| 日常入口 | 只有 InnoEngine.Editor / InnoEngine.Build；平台库不加入默认外来宿主构建，不为 not built 标签扩大默认闭包。 |
| Wiki | 最后实际 CLI references：45,592 个符号、190 个示例，错误 0；项目页、符号、签名、链接、旧入口和内容边界门禁通过。 |
| 消费者 | 两个独立公开 API 消费者编译及 Plugin 导出成功，408.593 / 39.578 秒；外部产品不成为 Wiki 职责依赖。 |

原始失败保留，不降低生产校验：旧平台名字、伪 archive、旧 staging 名和请求目录布局被正确拒绝；损坏 child claim 在父进程 Active 时仍不能退休。Wiki 最初的测试根含额外程序集，实际 CLI 校验揭示 Animation 缺口；现在使用真实 CLI 生成的 references，不复制其他配置 DLL 补齐。

## 发布与非 GUI 实际运行

| 产品 | 当前结果 |
| --- | --- |
| Windows CoreCLR | 发布 75.922 秒；两次 360 帧后台运行 exit=0、stderr 空，存储递增，Ready DSP 两次观察值推进。 |
| Windows NativeAOT | 发布 87.000 秒；实际两次运行相同生命周期/存储/DSP 门禁通过，无 IL2026/IL3050。 |
| Web 解释执行 | 发布 127.906 秒；隔离 headless 两次输入/回调、帧推进、存储 0→1→2、冻结与恢复通过，JS errors=0。 |
| Web AOT | 发布 426.188 秒；最终链接与实际 headless 两次运行同矩阵通过。 |
| Editor Debug / Release | 清理后再次运行：Debug 三次、Release 一次，各 120 帧正常退出；没有 Native/Binding/Task/Pack 准备或 teardown 诊断。 |
| 最后 CLI 修复 | 公共运行产品代码未再改变；CLI 组合补引用后的实际脚本重新编译和 Wiki 全例复验通过，Source 两配置及架构重跑通过。 |

Web 每次记录 155 个第三方 WebGL capability-query 警告及 1 个 ScriptProcessorNode 弃用警告。没有 JS error，当前非 GUI 功能判据通过；不能据此宣称无警告、渲染视觉或音频听感已验收。不修改第三方源码来隐藏警告。

## 性能

| 测量 | 结果与边界 |
| --- | --- |
| 准备 Debug / Release | AOT 修正后 448.594 / 308.203 秒；准备次数不混入热命中。 |
| 三次热 Debug Build | 119.047 / 146.297 / 138.343 秒，中位数 **138.343 秒**；compiler、binding generation、完整 Task 编译、materialization 均 0。 |
| 相同环境 Native 串行 / 批次 | 38.225 / **11.976 秒**；14,729,595,091 / **5,227,530,280 字节**；两者 compiler/binding=0。独立 fresh 阶段仍分别校验，不能跨阶段复用过期结果。 |
| 两个并发 Build | exit [0,0]，合计 302.219 秒；同一产品由 checkout owner 串行协调。 |
| 小 Native 修改 | 179.015 秒，2 个 Native 工具进程，9,821 字节复制，binding=0；恢复 144.203 秒，compiler=0。 |
| Pack 第二请求 | Windows **32.188 秒**；Browser SDK 修正后 **32.578 秒**；同一 Pack 内容身份，无工具/Native/BGCS/Player 编译。 |
| 清理后 Debug 首帧 | **7.268 / 3.665 / 3.626 秒**；包含部署校验、Host 和项目初始化。 |
| 清理后 Release 首帧 | **7.072 秒**；不混入 GUI/DPI/拖动验收。 |

历史同环境整轮 Build 中位数 248.420 秒、阶段性修正 151.203 秒与最后 138.343 秒分别记录。受磁盘、文件缓存、SDK 切换和负载影响，不承诺其他机器同比例改善。热 Build 仍有分钟级 SDK 求值与内容哈希成本；已准备 Editor 的直接启动不走该完整流程。

同内容部署、阶段内输入去重和图求值复用通过公开契约回归；必要输出哈希、exports、原子提交、冷编译与锁后 fresh 验证没有删除。

## 磁盘与外部阻塞

| 正式清理轮次 | 实际卷空间增加 |
| --- | --- |
| 第一轮 | 12,201,340,928 字节 |
| 大规模登记 owner 清理 | 30,742,011,904 字节；逻辑退休 116,634,794,343 字节。 |
| 最后一轮 | **14,342,897,664 字节**；逻辑退休 17,754,104,774 字节，预览 13.968 秒、执行 50.047 秒、exit=0。 |
| 各次实际增量合计 | 57,286,250,496 字节；期间重新构建和发布又分配空间，不能把合计当成最终净增加。 |
| 最后一轮清理后 | **65,568,768,000 字节可用**。 |
| 最后报告读取 | 65,570,340,864 字节可用；系统并发写入会改变数值。 |

37 个历史未登记目录仍存在，基线逻辑合计约 55.11 GB。之前批量删除及单目录删除被自动审批拒绝，工具原因 `blocked by policy`。本轮没有重试同一路径、补登记、换工具或越过范围；清单 `blocked-historical-directories-current.json`。原卷盘点中用户、SDK、驱动、分页文件与独立仓库分别归类，不能全算为 AI 中间产物。

**空间目标未达成**。即便按逻辑大小相加也不能证明可恢复到 200 GB：hardlink/junction、文件分配与其他占用必须区分。日志、当前产品、有效 Pack、活跃 load/reader 保留；不清理 NuGet/SDK/用户数据，不删除锁文件解锁。默认 64 GiB 缓存预算和五秒自动回收预算保持；时间不足安全延期，显式清理使用独立预算。

## 架构审查与扩展结论

没有为性能引入平台直连或第二缓存权威表。新增 backend 继续在 `backends/<Component>` 维护 runtime/native/build owner；必要的平台接入归属 `platforms/<Target>/integrations`，发行只选用 factory、内容 compiler、Native plan 和真实 owner。共享领域、平台 packager、Player 和 Editor Hosting 不修改为具体 backend 分支。

本轮公开 API 为表达真实所有权与执行边界而增加，其余实现保持 internal；同一个 source/recipe 只有一个 owner。通过实际 MSBuild 图、脚本裁剪 references、Player runtime closure、公开 XML/Wiki 和失败路径验证，而非仅按目录宣称解耦。

剩余已知限制：分钟级热 Build 成本、Web 第三方警告、五秒自动回收可能延期、200 GB 空间审批阻塞、MacOSArm64 实机和远端 CI 无本轮证据。GUI/视觉/声学明确排除，不冒充完成。

最后文档审阅补齐 ProcessOwnedDirectory 的 InvalidDataException/ArgumentException 英文 XML 和 Wiki 说明；只有注释变化，Core.IO 再编译 2.76 秒，0 errors / 0 warnings。之前运行证据对应的实现语义没有变化。

## 提交与完整结案条件

当前源码可提交为经过非 GUI 本机验收的整改；报告必须随提交保留。建议标题：

```text
fix(build): finalize cache reuse and tool host ownership
```

本轮工程实现与非 GUI 门禁已收口；磁盘 200 GB 目标仍为外部阻塞。跨平台实机结案需 MacOSArm64 证据；本轮不重新要求 GUI。当前 Windows 无法访问 `/System/Library/Sounds/Glass.aiff`，提示音未播放。

---

## 执行中检查点（历史，不替代上文最终状态）

实施基线 `b488d5d8588ec56e6ca1ab50bb1f30c680e81e8f`。2026-10-14 持续执行；未声明最终完成，不使用 Computer Use，不自动提交。

证据：`artifacts/acceptance/build-cost-tool-host-disk/20261013-implementation/`。

| 阶段 | 当前证据 |
| --- | --- |
| 基线 | 工作区开始干净，可用 49,364,123,648 字节 |
| owner / role | PID + 启动 ticks，严格 claim，三种 role 已实现；空闲 workspace 不永久保护 |
| worker / JIT | 移除 collectible inspection 与 TieredCompilation=false；30 个正常 worker 生命周期及 nonce 反例通过 |
| Native 热批次 | 完整命中批次、损坏回退、并发 fixture 通过；真实产品性能待跑 |
| Task publication | 真实输入索引提前命中，跨 Build 复用及同长度/mtime 修改拒绝命中通过 |
| 生成 | 新 claim staging 与 Host 输出集回滚，11 项生成测试通过 |
| 回归 | owner/cache/task 55 项通过；worker/batch/binding 15 项通过；新工作登记相关 35 项通过。不同套件存在重叠，不能累加为唯一测试总数 |
| 当前编译 | CLI Debug 最新一次 0 errors / 0 warnings；入口 Debug 0/0。后续源码继续修改，最终编译仍需重新执行 |
| 磁盘 | 正式 owner 清理退休逻辑 14,396,913,521 字节，卷实际释放 12,201,340,928 字节，清理后可用 59,033,391,104 字节；200 GB 目标未达到 |
| 产品 / 性能 / Wiki | 待当前源码闭包验证 |

## 约束与阻塞

历史 37 个未登记目录删除曾被自动审批拒绝，本轮未删除、未补登记、未绕过。新增工作登记拒绝认领已存在的未登记目录。当前空间目标尚未完成；不会扩大到用户、NuGet、SDK、驱动或系统文件。

首次普通 Build 的 MSBuild 节点/DLL 锁造成 CS2012；关闭本次启动的构建服务后，使用 --disable-build-servers -m:1 -nodeReuse:false 重试通过。第一轮 Task 测试的旧跨 Build 编译预期被新契约替换后通过，失败日志保留。

GUI、焦点、视觉、声学不执行；macOS 实机独立未实测。旧产品结果不充当本次代码修改后的运行证据。

## 01:14 检查点

- Editor Debug 准备构建通过，产品 MSBuild 39:25.31，0 errors / 0 warnings；新 Shader 工具冷编译约 11:54。当前源码随后补齐 Task 引导登记，不能以此前构建代表最终验证。
- Task 引导 adapter 动态调用 Foundation 公开 API，SDK inline compiler 的框架差异不通过私有反射解决；直接 PrepareInnoTaskPublisher 门禁通过。
- 发现 owner GUID 抖动造成重复 SDK 项目准备，保留既有操作身份并在同一活跃 claim 下登记参与进程后，直接门禁再次通过。原失败和诊断日志保留。
- 仍待最终完整回归、Wiki/API 门禁、真实热构建、十次实际 inspection、产品发布运行和最终磁盘盘点。

## 01:56 检查点

- 最新源码契约套件：70 passed / 0 failed，closure-contracts-final.trx。
- Task bootstrap shared claim 直接 MSBuild 验证通过；保留原操作身份，不重新生成引用图身份。
- Wiki 初轮库存 45,591 个符号、190 个示例；新增/修改映射已同步 8 页，待带真实裁剪脚本 references 的完整重跑。首轮没有传 script references，相关示例缺前置条件，不算源码错误。
- Editor 最终源码准备当前 session 9860，editor-final-prepare.log；源码固定后会测量热构建。
- 剩余：十次真实 worker、真实 Native 串行/批次对比、三次热 Build、并发 Build、当前产品矩阵/消费者/Pack/小 Native 修改、Wiki/架构/SourceRelease、最终磁盘清理和报告。GUI 不执行。

## 03:48 当前源码检查点

- 构建契约 77/77、Hosting 10/10；后续工具私有编译增加禁用共享 compiler server，最终完整套件仍需重跑。
- 真实串行/批次 Native 对照：73.17 / 11.42 秒，14,729,568,172 / 5,227,524,088 字节，两者 Native 进程/生成均为 0。十次真实 worker identity 检查通过。
- 三次最终 Debug Build：151.203、145.844、175.281 秒，中位数 151.203；每次 Native 进程=0、绑定生成=0、完整 Task 编译=0，CLI 工具连续有效复用。共享生成器中立 lock 路径修复消除了无源码变化的工具输入漂移。
- 两个并发入口均 exit=0，总计 302.219 秒；checkout 所有权串行协调同一产品。
- Text 实现末尾临时注释测量：179.015 秒，Native 进程=2、绑定生成=0、复制=9,821 字节。原字节、mtime 已恢复并证明 git diff=0；恢复后 144.203 秒，Native 进程/绑定生成=0。
- SDK compiler server 持有固定源码 workspace 的生成器 DLL 曾导致 CS2012。失败日志保留；CLI 私有编译使用 --disable-build-servers，Task 私有编译 UseSharedCompilation=false，工具宿主退出后不遗留该 compiler owner。
- Solution 230/230，无遗漏文件或项目；日常 Exe 仍仅两个。Native ABI 重复副本和旧构造函数文档行已清理，正式严格 Wiki 门禁待跑。
- 正在运行 Release Editor/Support Pack/四发布矩阵：session 79063，packs-products-commands.json 逐项写入；后续还需后台/headless 运行、消费者、Source 配置、严格 Wiki/架构和最后空间回收。
- 自动缓存盘点在部分热构建中可完整完成，积累工作目录后仍可能按五秒预算安全延期；显式清理与最终回收需要另外执行，不能称预算盘点始终零成本。

## 当前产品复验与 AOT 修正

- Release Editor 准备 1,109.828 秒；Windows Pack 两次请求 580.954 / 34.203 秒，返回同一内容身份，第二次未执行 Native、绑定或 Player 托管编译。
- Browser 使用仓库现存 wasm-tools SDK 与显式进程 CMake/Ninja PATH；Pack 两次请求 638.437 / 149.000 秒，返回同一内容身份。默认 SDK 缺少 workload 和未加入 CMake PATH 的失败属于执行环境选择，失败原因保留。
- 当前脚本编译与 CoreCLR 发布通过。NativeAOT 首次发布发现新增 ProcessOwnedDirectory 反射式 JSON 读取违反 IL2026 / IL3050，改为显式 JsonDocument / Utf8JsonWriter；补测损坏子进程 claim 为 Unknown，不允许退休。
- 修正后 SourceDebug 213.141 秒，0 errors / 0 warnings；SourceRelease、架构、完整构建测试与重新发布仍在执行。此前结果不替代此修正后的门禁。

## 完整契约与 Wiki 门禁

- SourceRelease 532.843 秒，0 errors / 0 warnings；完整架构 122/122，通过。
- 完整 Build suite 初轮 320 项，313 passed、7 failed：两项新损坏 claim 断言误把活跃父 owner 当成 Unknown；其余夹具仍使用旧平台名、伪 archive、旧 staging 名和旧请求根布局。修正夹具后，受影响 23 项全部通过；生产校验没有降级。
- worker 崩溃、强制终止和真实十分钟截止均通过；截止用例实际 10:02.135，保留上次完整 publication，子进程退出后清理当前 claim。
- 严格 Wiki 的两项 repository 测试通过；当前 inventory 为 45,592 个符号、190 个示例，符号签名映射缺失 0。证据保存于 `artifacts/acceptance/wiki-gate/7a6159cd28ed4f60a8b0f49e403b5cee/`。
- 首轮完整套件 1,845.484 秒，保留失败 TRX 和日志。当前正在执行 AOT 修正后的产品准备，不宣称运行矩阵已经完成。

## AOT 修正后的准备与回收

- Debug / Release 准备分别 448.594 / 308.203 秒，均 0 errors / 0 warnings。
- 三次 Debug 热 Build 119.047 / 146.297 / 138.343 秒，中位数 138.343。Native compiler、binding generation、完整 Task 编译均 0；Native 校验每次 5,227,530,280 字节，materialization 0。
- 构建用例按 testName 合并初轮与修正复验：320 个唯一用例，剩余失败 0；不是将重叠测试次数相加。原始合并结果 `results/build-contract-closure-union.json`。
- 显式正式清理预览 11.641 秒，执行 96.875 秒，exit=0。逻辑退休 116,634,794,343 字节，卷实际增加 30,742,011,904 字节；清理后可用 69,499,002,880 字节，200,000,000,000 字节目标未达到。
- 37 个被拒绝删除的历史目录全部仍存在；未补写 owner、未绕过审批。当前产品、Pack、pub reader 及验收日志保留。
- 当前 session 58192 执行修正后的 Support Packs、脚本与四产品发布，逐项结果写入 `aot-fixed-products-commands.json`。运行及最终审查仍未完成。

## 当前产品与 SDK 一致性结果

- 四种发布均通过：CoreCLR 75.922 秒、Windows NativeAOT 87.000 秒、Web 解释执行 127.906 秒、Web AOT 426.188 秒。当前 NativeAOT 不再出现 IL2026 / IL3050。
- Windows 两种部署各两次 360 帧后台运行均 exit=0、stderr 为空；存储递增和两次 probe 间 DSP 时钟推进得到实际证据。首次运行断言要求独立音频日志，但当前 probe 把音频字段写入存储及帧日志；验证器修正为检查两次实际 Ready/DSP 观察值递增，未降低产品判据。
- Web 解释执行/AOT 各两次隔离 headless Edge 运行通过：输入回调、帧推进、存储 0→1→2、冻结/恢复，JS error=0。没有操作用户浏览器、桌面或截图。
- 发现 --dotnet 只传给产品、未传工具准备导致 Restore 输入漂移。Bootstrap 已统一选择；4 项提前拒绝测试通过。显式 SDK 首次准备 781.047 秒，第二次 32.578 秒；第二次只有 verified tool、Pack 路径与清理统计，无工具/Native/绑定/Player 编译。
- 两个公开消费者编译与 Plugin 导出通过，408.593 / 39.578 秒；仅用于验证，不作为当前 Wiki 的职责依赖。
- 最新真实 Native 对照：串行 38.225 秒、14,729,595,091 字节；批次 11.976 秒、5,227,530,280 字节；两者 Native compiler、绑定生成=0。最后十次真实 worker 与 SDK gate，共 6 项通过。
- 独立 Editor 首帧计时：Debug 7.206 / 3.849 / 3.657 秒，Release 6.658 秒；各 120 帧正常退出，无 Native/Task 准备、stderr 或 teardown 诊断。与浏览器并行的首轮计时另存，未混入独立计时。
- 当前 session 78285 重跑最终 SourceDebug/SourceRelease 与架构。最终文档、映射及回收仍待结案。
