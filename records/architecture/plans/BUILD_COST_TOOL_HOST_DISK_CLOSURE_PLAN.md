# 构建成本、工具宿主与磁盘收口执行计划

[执行规格索引](README.md) · [验收](../acceptance/BUILD_COST_TOOL_HOST_DISK_CLOSURE_ACCEPTANCE.md)

用户批准于 2026-10-13。实施基线 `b488d5d8588ec56e6ca1ab50bb1f30c680e81e8f`，保留现有源码；不自动提交。

## 固定目标

1. 热构建不执行 Native compiler、绑定生成或重复 Task 编译；阶段内物理输入和 ProjectGraph 复用，必要 fresh 校验保留。
2. 完整 Native 热命中在全部必要协调权取得后使用 union verification；缺失或损坏时完整释放并进入既有冷路径。
3. Build 工具输入检查、runtime 发布使用现有 Build 入口的隔离 worker，移除 collectible inspection 和 TieredCompilation=false。
4. owner 记录 PID 与 UTC 启动 ticks，区分 Active/Exited/Unknown；Unknown 不删除。区分 immutable、workspace、process working 生命周期。
5. 临时工作在首次 payload 写入前登记，异常退出后可安全退休。Bootstrap 不依赖 Toolchains/BGCS，基础进程身份属于 Core.IO。
6. 只清理可证明由执行产生、无活动 owner 的产物，争取至少 200,000,000,000 字节空闲；审批拒绝不绕过，范围不足不删除用户/SDK/NuGet/系统数据。
7. Windows CoreCLR/NativeAOT、Browser 解释执行/AOT、Editor 后台生命周期、消费者和性能取得新证据；不使用 Computer Use、不控制用户桌面或浏览器。

## 实现归属

当前实施的文件归属如下；已有类承担的快照、统计与发布职责继续复用，不为文件树建立空类型。

```text
entry/Inno.Entry.Bootstrap/
  DevelopmentEntry.cs
  DevelopmentBuildTool.cs
  DevelopmentBuildWorker.cs
  DevelopmentBuildWorkerRequest.cs
  DevelopmentBuildWorkerProcess.cs
src/foundation/core/Inno.Core.IO/
  ProcessOwnerStamp.cs
  ProcessOwnerState.cs
  ProcessOwnedDirectory.cs
  ProcessOwnedDirectoryDescriptor.cs
build/toolchains/Inno.Build.Toolchains/
  Native/ProductNativeCacheLookup.cs
  Native/ProductNativeBuildPlan.cs
  Native/ProductNativeBuildStep.cs
  Native/NativeInputReadCache.cs
  Native/NativeBuildRecipe.cs
  Native/NativeWorkspaceIdentity.cs
  Native/NativeArtifactPublisher.cs
  Native/ProductNativeDeployment.cs
  Cache/BuildCacheArtifactRole.cs
  Cache/BuildCacheArtifactDescriptor.cs
  Cache/BuildCachePublisher.cs
  Cache/BuildCacheRetirement.cs
  Managed/ManagedBuildExecution.cs
  Managed/ManagedBuildInputReader.cs
  Managed/ManagedInputEvaluatorPreparation.cs
build/tasks/Inno.Build.TaskHosting/
  TaskRuntimeBootstrapOwner.cs
  TaskRuntimeRequestKey.cs
  TaskRuntimePreparation.cs
  BuildTaskRuntimeTask.cs
  PublishTaskHostTask.cs
build/tasks/Inno.Build.Tasks/CompileShaderTask.cs
build/msbuild/Inno.Build.Tasks.targets
build/bindings/Inno.Build.Bindings/
  Generation/BindingExtensionPreparation.cs
  Generation/BindingGenerationOperation.cs
  Publication/BindingGenerationPublication.cs
build/distributions/Inno.Build.Distribution.Standard/
  StandardBuildCaches.cs
  StandardNativeBuildPlans.cs
build/cli/Inno.Build.Cli/
  BuildApplication.Runtime.cs
  BuildWorkspace.cs
build/pipeline/Inno.Build/Game/GameBuildPipeline.cs
build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPublisher.cs
backends/ImGui/build/Inno.Build.Toolchains.ImGuizmo/ImGuizmoToolchain.cs
tests/build/Inno.Build.Tests/
  ProcessOwnershipTests.cs
  DevelopmentBuildWorkerTests.cs
  GeneratorRestoreScopeTests.cs
  ProductNativeCacheLookupTests.cs
  ProductNativeCostTests.cs
  existing identity/cache/task/publication regressions
tests/editor/Inno.Editor.Hosting.Tests/EditorCommandLineOptionsTests.cs
docs/core/Inno.Core.IO.md
docs/build/
docs/entry/
docs/editor/Inno.Editor.Hosting.md
docs/architecture/
records/architecture/acceptance/BUILD_COST_TOOL_HOST_DISK_CLOSURE_ACCEPTANCE.md
```

实测新增收口：共享 Roslyn 生成器使用中立 NuGet lock；私有工具编译不把生成器 DLL 留在共享 compiler server。后台 Editor 显式窗口可见性使用同一 Host 生命周期；不增加生产可执行项目、窗口替身或 GUI 验收。逐文件完整清单保存在本轮 `file-map.tsv`。

- Core.IO：ProcessOwnerStamp、ProcessOwnerState，以及确有需要的通用文件工作 owner；不依赖 Build。
- Bootstrap：DevelopmentBuildWorker/Request/Process；worker 仅 inspect/publish，BCL 数据交接，十分钟超时，不重取父进程 checkout 锁。
- Toolchains：ProductNativeCacheLookup、批次验证、实际执行闭包、明确 Cache role 与工作目录退休。
- TaskHosting：语义输入与私有 owner 分离，有效 publication 在 SDK Build 前取得保护、验证并交付；IBuildEngine4 复用保留。
- Standard Distribution：实际 owner 根注册，平台和 backend 继续保持独立边界。
- 同步 public XML、所有消费者、Solution、Wiki、测试；删除被替代入口，不添加 legacy reader、IVT 或私有反射。

## 阶段与验收

基线/空间 → owner/工作登记 → worker/JIT → Native 热批次 → Task 提前复用 → 产品/性能 → Wiki/架构/最后回收。

每阶段编译和公开边界回归后继续。测试覆盖同长度同 mtime、输入增删、等锁/编译中变化、损坏/多余输出、并发无死锁、取消/权限失败、PID 复用、未知 owner 保护和异常退出 staging。

默认 tiered compilation 下执行三十轮最小 worker、十次真实检查、三次热 Editor Build、两个并发 Build。父进程不加载 worker 的工具程序集。

产品使用独立验收副本：Windows 两部署实际后台运行；Browser 两部署 headless 启动/日志/callback/存储/暂停恢复；Editor Debug/Release 完整部署及正常退出；一次可恢复的小 Native 修改；两次相同 Pack；公开消费者编译/导出。

统计总/阶段耗时、图求值、进程、哈希/复制字节、准备/部署、卷空间。热 Native/binding/Task 实际编译为零，阶段无重复读取，热中位耗时及读取低于同环境基线。同内容部署不替换文件。

## 约束与完成判定

仅 WindowsX64/MacOSArm64/BrowserWasm32；不升级框架、不修改 extern/BGCS 源码或手改绑定、不新增生产 Exe。64 GiB 默认缓存预算保留。macOS 实机独立未实测，GUI/焦点/视觉/声学不执行。

源码与非 GUI 门禁通过，空间达到目标或存在确实无法解除的外部阻塞后交付。所有阻塞、未执行项和成本未改善均如实记录，不能写为完成。
