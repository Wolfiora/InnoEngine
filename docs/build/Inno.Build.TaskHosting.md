# Inno.Build.TaskHosting

[Build 索引](README.md) · [Wiki 首页](../README.md) · [Task 集成](Inno.Build.Tasks.md) · [平台归属验收](../architecture/PLATFORM_OWNERSHIP_REFACTOR_ACCEPTANCE.md)

## 职责与边界

这是 MSBuild 引导边界的小型库，唯一职责是发布及退休 Task 的不可变运行闭包。它只依赖 Core IO 和 MSBuild 契约，
不引用 BGCS、具体平台、后端、发行组合或完整 `Inno.Build.Tasks`。发布宿主无需加载正在构建的任务库，避免引导循环与 bootstrap DLL 锁定。

## 初始化与所有权

共同 targets 先构建小型 publisher，并将其复制到私有引导目录；完整 Task 项目返回实际运行文件的逻辑路径及 SHA-256。
Publisher 在 fingerprint lease 下验证来源及缓存，发布完整 staging，然后登记当前进程的私有 reader。
MSBuild 从已验证的不可变快照准备自己的私有加载目录。宿主快照按内容身份共享，加载目录按进程保留。

退出后，后续 publisher 只退休所有 owner 已退出的 reader；只有不存在引用且取得 snapshot lease 时才退休其来源快照。
无法检查 owner、文件仍被 loader 持有或另一 publisher 持有 lease 时保留缓存，不能影响已经完成的当前发布。

## 公开 API

| API | 语义 |
| --- | --- |
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
普通产品 Build/Publish 还验证真实引导闭包；Design-time Build 不执行该 Task。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

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
| [`void Inno.Build.TaskHosting.PublishTaskHostTask.Cancel()`](../../build/tasks/Inno.Build.TaskHosting/PublishTaskHostTask.cs#L115) | Cancels ownership waits and unpublished copies without racing token retirement. |

## 项目依赖

- [Inno.Core.IO](../core/Inno.Core.IO.md)：实现依赖，PrivateAssets="compile"。
