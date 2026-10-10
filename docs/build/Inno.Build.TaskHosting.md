# Inno.Build.TaskHosting

[Build 索引](README.md) · [Wiki 首页](../README.md) · [Task 集成](Inno.Build.Tasks.md) · [平台归属验收](../architecture/PLATFORM_OWNERSHIP_REFACTOR_ACCEPTANCE.md)

## 职责与边界

这是 MSBuild 引导边界的小型库，负责协调 Task 运行闭包的编译、发布及退休。它只依赖 Core IO 和 MSBuild 契约，
不引用 BGCS、具体平台、后端、发行组合或完整 `Inno.Build.Tasks`。发布宿主无需加载正在构建的任务库，避免引导循环与 bootstrap DLL 锁定。

## 初始化与所有权

共同 targets 在操作私有中间目录构建小型 publisher，避免多个 MSBuild 进程争用尚未加载的引导 DLL。
`BuildTaskRuntimeTask` 使用同一共享 bootstrap 根的 `FileLease` 协调完整 Task 项目编译；等待时通过 MSBuild `Yield`/`Reacquire` 释放节点，失败、取消或异常都释放写 lease。
SDK 与项目构建仍由当前 MSBuild engine 执行，保留明确属性和移除产品属性的机制，不创建第二个 CLI 或通过 PATH 重新选择 SDK。完整 Task 项目返回实际运行文件的逻辑路径及 SHA-256。
Publisher 在 fingerprint lease 下验证来源及缓存，发布完整 staging，然后登记当前进程的私有 reader。
MSBuild 从已验证的不可变快照准备自己的私有加载目录。宿主快照按内容身份共享，加载目录按进程保留。

退出后，后续 publisher 只退休所有 owner 已退出的 reader；只有不存在引用且取得 snapshot lease 时才退休其来源快照。
无法检查 owner、文件仍被 loader 持有或另一 publisher 持有 lease 时保留缓存，不能影响已经完成的当前发布。

## 公开 API

| API | 语义 |
| --- | --- |
| `BuildTaskRuntimeTask` | 一次实例协调一次共享编译；调用者提供支持 Yield 的真实 MSBuild engine。 |
| `ProjectFile` / `Targets` | 绝对任务项目位置及返回完整 runtime 的有序目标；目标为空或项目不存在明确失败。 |
| `ArtifactsDirectory` | 绝对共享编译 owner；任务直接设置 ArtifactsPath，并在此取得 build.lock。 |
| `Properties` / `RemoveProperties` | 明确 name=value 编译属性及从父构建移除的全局属性；拒绝重复声明和 ArtifactsPath 覆盖。 |
| `TargetOutputs` | 成功返回的实际文件及原始哈希 metadata；失败不发布闭包。 |
| `PublishTaskHostTask` | 一次实例用于一次 MSBuild 发布；依赖通过公开 Task 属性及 `BuildEngine` 注入。 |
| `InputFiles` | 冻结文件集合；每项提供 `RelativePath` 与 `FileHash`，必须包括 Task 主程序集。 |
| `CacheDirectory` | 绝对共享 host 缓存 owner；不同内容身份分别发布。 |
| `LoadDirectory` | 绝对私有加载位置，必须位于同一 Task 根的 `loads` 内。 |
| `PublisherDirectory` | 可选私有小型 bootstrap 目录；提供时在同一进程 owner 下登记并退休。 |
| `PublishedAssembly` | 成功后的已验证共享程序集位置；MSBuild 复制完整闭包后加载。 |
| `Execute()` | 完整验证、发布、登记及退休；失败记录 MSBuild error 并返回 false。 |
| `Cancel()` | 取消未提交工作和 lease 等待；与 token 释放同步。 |

## 工作流

```xml
<Inno.Build.TaskHosting.PublishTaskHostTask
  InputFiles="@(_HashedRuntimeFiles)"
  CacheDirectory="$(TaskCacheRoot)/hosts"
  LoadDirectory="$(TaskCacheRoot)/loads/$(OperationId)/$(Configuration)">
  <Output TaskParameter="PublishedAssembly" PropertyName="VerifiedTaskAssembly" />
</Inno.Build.TaskHosting.PublishTaskHostTask>
```

完整集成入口是 `build/msbuild/Inno.Build.Tasks.targets`；产品项目不自行复制此流程。
输入变化拒绝候选；同长度同 mtime 损坏和未声明文件通过集合与实际内容哈希检测。
缓存修复使用 Core IO 原子目录安装，不向加载中的共享文件原地写入代码。

## 验证

`tests/build/Inno.Build.Tests/TaskHostPublicationTests.cs` 通过真实 Task 契约验证复用、并发、损坏、取消、路径拒绝及 reader 退休。
`TaskRuntimeBuildTests.cs` 通过公开 Task/IBuildEngine3/FileLease 验证共享写入、取消、失败释放及属性拒绝；并发真实 MSBuild 进程另行验证实际引导闭包。
普通产品 Build/Publish 还验证真实引导闭包；Design-time Build 不执行该 Task。

## Build 生命周期内复用与 restore ownership

BuildTaskRuntimeTask 在 IBuildEngine4 的 Build registry 注册 BCL string[][] 不可变 publication metadata。复用前重新读取完整请求身份和实际输出哈希；源码、SDK、配置变化、输出损坏、失败或取消不会命中成功缓存。不同 Build 和私有载入边界保持独立。共享 Task runtime 编译和 restore graph 写入分别取得 Core.IO FileLease，等待时 Yield/Reacquire；RestoreOwnedProjectTask 传递明确 project metadata/属性，不重新解析 SDK。registry 不持有 lease 或 SDK owner，后续 publisher 按 reader 协议退休。

## Build 生命周期复用

IBuildEngine4 的 Build registry 只缓存已成功验证的不可变 publication，完整身份覆盖当前源码、SDK、配置、明确属性及输出 owner。记录使用 BCL 数据，不持有私有 ALC 类型、SDK、文件 lease 或失败/取消结果；各实际载入边界仍保留自己的私有 owner。跨进程编译和共享 restore 写入由 Core.IO lease 协调，等待期间 Yield/Reacquire。

## 当前源码公开 API 清单

只列当前源码 public/protected 表面；内部机制不是稳定 API，参数、返回、失败及所有权以英文 XML 为准。

### `Inno.Build.TaskHosting.BuildTaskRuntimeTask`

| 当前声明 | 行为 |
| --- | --- |
| [`ITaskItem[] Inno.Build.TaskHosting.BuildTaskRuntimeTask.TargetOutputs`](../../build/tasks/Inno.Build.TaskHosting/BuildTaskRuntimeTask.cs#L59) | Gets the returned runtime files and their hash metadata after successful compilation. |
| [`Inno.Build.TaskHosting.BuildTaskRuntimeTask`](../../build/tasks/Inno.Build.TaskHosting/BuildTaskRuntimeTask.cs#L15) | Coordinates shared task-runtime compilation before immutable publication and private loading. |
| [`override bool Inno.Build.TaskHosting.BuildTaskRuntimeTask.Execute()`](../../build/tasks/Inno.Build.TaskHosting/BuildTaskRuntimeTask.cs#L68) | Serializes shared writes across processes while yielding the MSBuild node during ownership waits. |
| [`string Inno.Build.TaskHosting.BuildTaskRuntimeTask.ArtifactsDirectory`](../../build/tasks/Inno.Build.TaskHosting/BuildTaskRuntimeTask.cs#L30) | Gets or sets the absolute shared intermediate owner, also supplied as ArtifactsPath. |
| [`string Inno.Build.TaskHosting.BuildTaskRuntimeTask.LoadDirectory`](../../build/tasks/Inno.Build.TaskHosting/BuildTaskRuntimeTask.cs#L36) | Gets or sets the private reader location registered before returning an immutable runtime. |
| [`string Inno.Build.TaskHosting.BuildTaskRuntimeTask.ProjectFile`](../../build/tasks/Inno.Build.TaskHosting/BuildTaskRuntimeTask.cs#L24) | Gets or sets the task project whose complete runtime is built by the calling MSBuild engine. |
| [`string[] Inno.Build.TaskHosting.BuildTaskRuntimeTask.Properties`](../../build/tasks/Inno.Build.TaskHosting/BuildTaskRuntimeTask.cs#L49) | Gets or sets explicit name=value properties after product-specific global properties are removed. ArtifactsPath is owned by this task and cannot be overridden. |
| [`string[] Inno.Build.TaskHosting.BuildTaskRuntimeTask.RemoveProperties`](../../build/tasks/Inno.Build.TaskHosting/BuildTaskRuntimeTask.cs#L54) | Gets or sets parent global properties that must not enter the host tool build. |
| [`string[] Inno.Build.TaskHosting.BuildTaskRuntimeTask.Targets`](../../build/tasks/Inno.Build.TaskHosting/BuildTaskRuntimeTask.cs#L42) | Gets or sets the ordered targets returning the complete hashed runtime closure. |
| [`void Inno.Build.TaskHosting.BuildTaskRuntimeTask.Cancel()`](../../build/tasks/Inno.Build.TaskHosting/BuildTaskRuntimeTask.cs#L171) | Cancels an ownership wait without racing retirement of the cancellation source. |

### `Inno.Build.TaskHosting.PublishTaskHostTask`

| 当前声明 | 行为 |
| --- | --- |
| [`ITaskItem[] Inno.Build.TaskHosting.PublishTaskHostTask.InputFiles`](../../build/tasks/Inno.Build.TaskHosting/PublishTaskHostTask.cs#L27) | Gets or sets the frozen runtime files, each declaring RelativePath and FileHash metadata. |
| [`Inno.Build.TaskHosting.PublishTaskHostTask`](../../build/tasks/Inno.Build.TaskHosting/PublishTaskHostTask.cs#L18) | Publishes one immutable task runtime per content identity before a build loads its private closure. This bootstrap depends only on filesystem primitives and never loads the task runtime being built. |
| [`override bool Inno.Build.TaskHosting.PublishTaskHostTask.Execute()`](../../build/tasks/Inno.Build.TaskHosting/PublishTaskHostTask.cs#L61) | Verifies sources and cached files, publishes a complete candidate under exclusive ownership, and registers the private reader before returning its shared runtime. |
| [`string Inno.Build.TaskHosting.PublishTaskHostTask.CacheDirectory`](../../build/tasks/Inno.Build.TaskHosting/PublishTaskHostTask.cs#L33) | Gets or sets the absolute owner of content-addressed, shared task runtime snapshots. |
| [`string Inno.Build.TaskHosting.PublishTaskHostTask.LoadDirectory`](../../build/tasks/Inno.Build.TaskHosting/PublishTaskHostTask.cs#L39) | Gets or sets the absolute private load directory whose parent receives this process's ownership marker. |
| [`string Inno.Build.TaskHosting.PublishTaskHostTask.PublishedAssembly`](../../build/tasks/Inno.Build.TaskHosting/PublishTaskHostTask.cs#L51) | Gets the verified immutable task assembly path after successful publication. |
| [`string Inno.Build.TaskHosting.PublishTaskHostTask.PublisherDirectory`](../../build/tasks/Inno.Build.TaskHosting/PublishTaskHostTask.cs#L46) | Gets or sets an optional private bootstrap directory to register under the same process ownership. An empty value leaves the caller's publisher assembly lifetime outside this cache. |
| [`void Inno.Build.TaskHosting.PublishTaskHostTask.Cancel()`](../../build/tasks/Inno.Build.TaskHosting/PublishTaskHostTask.cs#L122) | Cancels ownership waits and unpublished copies without racing token retirement. |

### `Inno.Build.TaskHosting.RestoreOwnedProjectTask`

| 当前声明 | 行为 |
| --- | --- |
| [`ITaskItem[] Inno.Build.TaskHosting.RestoreOwnedProjectTask.Projects`](../../build/tasks/Inno.Build.TaskHosting/RestoreOwnedProjectTask.cs#L23) | Gets or sets projects with their explicit AdditionalProperties metadata. |
| [`Inno.Build.TaskHosting.RestoreOwnedProjectTask`](../../build/tasks/Inno.Build.TaskHosting/RestoreOwnedProjectTask.cs#L14) | Coordinates SDK restore writes for the checkout's shared restore graph owner. |
| [`override bool Inno.Build.TaskHosting.RestoreOwnedProjectTask.Execute()`](../../build/tasks/Inno.Build.TaskHosting/RestoreOwnedProjectTask.cs#L44) | Waits outside the MSBuild node, then restores under exclusive cross-process write ownership. |
| [`string Inno.Build.TaskHosting.RestoreOwnedProjectTask.LockPath`](../../build/tasks/Inno.Build.TaskHosting/RestoreOwnedProjectTask.cs#L30) | Gets or sets the absolute lock belonging to all writers of the shared restore graph. The lock file remains after release. |
| [`string[] Inno.Build.TaskHosting.RestoreOwnedProjectTask.Properties`](../../build/tasks/Inno.Build.TaskHosting/RestoreOwnedProjectTask.cs#L36) | Gets or sets explicit name=value restore properties, independent of product discovery. |
| [`void Inno.Build.TaskHosting.RestoreOwnedProjectTask.Cancel()`](../../build/tasks/Inno.Build.TaskHosting/RestoreOwnedProjectTask.cs#L107) | Cancels this writer's ownership wait without canceling another active restore. |

## 项目依赖

- [Inno.Core.IO](../core/Inno.Core.IO.md)：实现依赖，PrivateAssets="compile"。
