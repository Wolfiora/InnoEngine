# Inno.Build.TaskHosting

[Build 索引](README.md) · [Wiki 首页](../README.md) · [Task 集成](Inno.Build.Tasks.md)

## Restore 与编译的求值边界

owned Restore 使用 NuGet 标准属性 `ExcludeRestorePackageImports=true`；Task runtime 编译显式使用 `false`。两个阶段因此拥有不同的 MSBuild 配置身份，编译读取已经恢复的 package imports，不能复用 Restore 开始前缓存的 content items。没有通过改写生成的 NuGet imports、删除缓存或按某个包名特殊复制资源来修复。

完整编译请求身份包含该阶段属性，并继续校验冻结输入、lease 与原子 publication。它也使错误的既有请求缓存不被当作当前完整请求复用。公开边界的真实 MSBuild 回归同时覆盖私有 runtime 准备与独立 owned Restore。

锁文件路径由工程的统一规则求值：工具保留实际 SDK RID，产品标签不自动扩展文件名。输入快照读取实际选定的清单和包内容，不回查另一配置的默认 obj；独立 ArtifactsPath 只改变所属 assets / 编译输出，不制造第二套源码清单。规则与更新方式见[依赖锁定](DEPENDENCY_SUBMISSION.md)。

## 职责与边界

这是 MSBuild 引导边界的库，负责协调 Task 运行闭包的编译、发布及退休，并在读取保护期间复制生成绑定。它依赖 Core IO、中立 Toolchains 与 MSBuild 契约，
不引用 BGCS、具体平台、后端、发行组合或完整 `Inno.Build.Tasks`。发布宿主无需加载正在构建的任务库，避免引导循环与 bootstrap DLL 锁定。

## 初始化与所有权

Solution 命令构建通过 `Directory.Solution.targets` 为同一次构建传递统一 bootstrap owner。单项目构建在项目引用闭包内传递其 owner。共同 targets 为该 owner 分配独立的 publisher 编译目录，并在执行项目引用目标及嵌套 Task runtime 准备时传递同一 owner。相同配置的依赖项目复用这次编译；每个实际加载边界仍复制到自己的私有目录。下一次根项目构建使用新的编译 owner，不依赖上次的可变中间态。
owner 表示本次执行的所有权，其随机值本身不进入完整 Task runtime 的输入指纹。请求求值仍保留该属性；若导入文件、源码选择或 `DefineConstants` 等实际编译输入由它改变，这些求值结果仍参与指纹，不能误用上次结果。Design-time Build 不准备 publisher。
IDE 可以把已经求值的 `ProjectInstance` 交给另一个 MSBuild worker。worker 重建 `UsingTask` 注册时可能再次求值，因此不能假设其随机加载身份仍等于传入的普通属性。共同 targets 使用 SDK inline task 捕获任务注册阶段的加载身份，再令 publisher 复制、runtime 发布与后续输入使用该身份。加载路径仍按实际注册隔离，不依赖 IDE 设置、固定机器路径或预先存在的缓存。
最小 owner 引导消费 SDK Build 实际返回的 Foundation 程序集路径，并保留调用方明确选择的 `ArtifactsPath`，不拼接目标框架或强制写入普通 bin。它从已构建文件读取字节执行公开协议，不让长寿命 IDE 构建宿主锁住 SDK 输出；下一次编译可以正常更新该文件。此阶段只交换 BCL 字符串与 Task，不把 Foundation 类型或委托保存到共享注册表。
`BuildTaskRuntimeTask` 使用同一 `artifacts/build-tools/tasks/workspaces/shared` 根的 `FileLease` 协调完整 Task 项目编译；等待时通过 MSBuild `Yield`/`Reacquire` 释放节点，失败、取消或异常都释放写 lease。
私有 Task 编译明确禁用共享 C# compiler server；可复用源码 workspace 的 SDK 子进程同样使用 `--disable-build-servers`。编译完成后不会让另一个长寿命编译宿主持有 workspace 中的生成器 DLL。普通 IDE 的编译服务器配置不由此改变。
SDK 与项目构建仍由当前 MSBuild engine 执行，保留明确属性和移除产品属性的机制，不创建第二个 CLI 或通过 PATH 重新选择 SDK。完整 Task 项目返回实际运行文件的逻辑路径及 SHA-256。
工具引导和 `RestoreOwnedProjectTask` 移除父 IDE 的 `SolutionPath`、相关 Solution metadata 与 restore graph entry point。
NuGet 静态图据此只读取本次明确选择的 project closure；Windows Editor 不会因为内部 Task restore 意外访问 Browser Player。
外层 Solution restore 保留自己的 entry point，按开发入口或中立源码配置选择闭包；实际 Browser 发布工程才要求 Wasm workload。
BGCS.Runtime 由产品的真实 NuGet Restore 图解析，不存在源码 Runtime 的独立还原入口。
Analyzer 引用保留 Solution 构建上下文，只移除产品与输出属性；它们属于编译依赖，不采用私有 Task 引导的独立 Solution 策略。
Publisher 在 fingerprint lease 下验证来源及缓存，发布完整 staging，然后登记当前进程的私有 reader。
MSBuild 从已验证的不可变快照准备自己的私有加载目录。宿主快照按内容身份共享，加载目录按进程保留。
这些私有目录的复制属于 Task 生命周期准备，而不是可脱离 owner 重放的项目输出。共同 inline adapter 直接复用所选 SDK 的 Copy 实现，保留原有复制、hardlink 和未变化文件策略；不会把私有路径作为独立 Copy task 暴露给项目输出调度器。普通程序集输出仍由 SDK 的标准目标管理。

退出后，后续 publisher 只退休所有 owner 已退出的 reader；只有不存在引用且取得 snapshot lease 时才退休其来源快照。
无法检查 owner、文件仍被 loader 持有或另一 publisher 持有 lease 时保留缓存，不能影响已经完成的当前发布。

独立输入还原项目通过 `InnoBuildInputRestoreProject` 显式声明。Task 在编译身份冻结前完成主图及这些依赖的 Restore，编译阶段禁止再次进行同一还原；编译后继续重新检查完整输入。Host、load、publisher 和编译 workspace 进入相同预算系统；进程声明继续保护私有加载目录及其来源。

## 公开 API

| API | 语义 |
| --- | --- |
| `BuildTaskRuntimeTask` | 一次实例协调一次共享编译；调用者提供支持 Yield 的真实 MSBuild engine。 |
| `ProjectFile` / `Targets` | 绝对任务项目位置及返回完整 runtime 的有序目标；目标为空或项目不存在明确失败。 |
| `ArtifactsDirectory` | 绝对共享编译 owner；任务直接设置 ArtifactsPath，并取得该目录外的 `<ArtifactsDirectory>.lock`。 |
| `Properties` / `RemoveProperties` | 明确 name=value 编译属性及从父构建移除的全局属性；显式值优先于同名父属性移除。拒绝重复声明和 ArtifactsPath 覆盖。 |
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
请求初始阶段用同一 ProjectGraph 计算 restore 状态和输入身份。已求值的 Compile、EmbeddedResource、Analyzer、AdditionalFiles、EditorConfigFiles、GlobalAnalyzerConfigFiles 与 AdditionalCompileInputs 都按实际内容参与身份；修改附加输入不能继续复用同一 Build 中的旧 Task runtime。

取得跨进程写 lease 后及编译后仍重新验证。初始读取复用不延伸到这些边界；同长度、同 mtime 的输入变化也必须失效。输出完整性另按实际文件集合和字节验证。

初始求值前按规范化路径顺序取得 host、load 和 workspace 的 owner 活动权，等待时 Yield/Reacquire MSBuild node。load 进程记录保留该操作使用的完整 host/workspace 依赖，防止维护删除同一 MSBuild 已完成 Restore、但后续项目仍会使用的中间文件；私有 Task 返回不会提前解除这种进程保护。Build registry 继续只保存中立 BCL 数据，不保存 lease 或 SDK 实例。
缓存修复使用 Core IO 原子目录安装，不向加载中的共享文件原地写入代码。

## 验证

`tests/build/Inno.Build.Tests/TaskHostPublicationTests.cs` 通过真实 Task 契约验证复用、并发、损坏、取消、路径拒绝及 reader 退休。
`TaskRuntimeBuildTests.cs` 通过公开 Task/IBuildEngine4/FileLease 验证共享写入、同 Build 复用、附加编译输入失效、取消、失败释放及属性拒绝；并发真实 MSBuild 进程另行验证实际引导闭包。
`TaskPublisherBootstrapTests.cs` 使用真实 SDK 的并行菱形项目闭包，检查一次构建只编译一个 publisher、四个消费者保留独立加载目录，以及下一次构建使用新的编译 owner。
`TaskRegistrationBuildTests.cs` 通过公开 `ProjectInstance` / `BuildManager` API 把预求值项目交给进程外 worker，验证普通属性与任务注册重新求值后，实际 publisher 仍在注册路径加载；连续两次构建保留不同的加载身份，并在 worker 仍存活时确认普通 Foundation 输出未被锁定。
普通产品 Build/Publish 还验证真实引导闭包；Design-time Build 不执行该 Task。

## Build 生命周期内复用与 restore ownership

BuildTaskRuntimeTask 在 IBuildEngine4 的 Build registry 注册 BCL string[][] 不可变 publication metadata。复用前重新读取完整请求身份和实际输出哈希；源码、SDK、配置变化、输出损坏、失败或取消不会命中成功缓存。不同 Build 和私有载入边界保持独立。共享 Task runtime 编译和 restore graph 写入分别取得 Core.IO FileLease，等待时 Yield/Reacquire；RestoreOwnedProjectTask 传递明确 project metadata/属性，不重新解析 SDK。registry 不持有 lease 或 SDK owner，后续 publisher 按 reader 协议退休。

## 生成绑定消费

`MaterializeBindingsTask` 对目标生成缓存取得 `BuildArtifactReadLease`，验证完整输出后，把唯一 `Bindings.cs` 原子复制到当前 SDK 的中间目录。Host source 使用组件 generation lease。相同内容不重复写入；任务返回后编译器使用自己拥有的副本，不再依赖缓存目录继续存在。

消费端等待 publication owner 时通过 `IBuildEngine3.Yield` 释放节点，完成或取消后 `Reacquire`，不能占着构建节点等待同一构建中的 producer。Host source 的等待由构建取消令牌终止，不以固定 30 秒判断耗时生成失败；取消不写入消费副本，节点重新取得失败也会释放已经取得的 owner。

`RegisterTaskPublisherTask` 在第一次消费前登记实际加载程序集所属的 `PublisherDirectory`，并登记同一操作的 `BootstrapDirectory` 编译目录。它核对受管根和实际程序集位置，不把其他任务的加载目录登记为自己的 owner。只执行绑定复制的构建也会留下可识别的进程所有权记录；编译目录不作为程序集加载位置。

完整 Task runtime 的 Build registry 复用与轻量 publisher 的引导是两个阶段。同一根构建可以复用 publisher 编译；每个加载边界仍需准备自己的文件副本并登记 owner。入口准备、产品构建和私有 Task 构建可能属于不同根调用，成本需要分别记录，不能将 runtime 复用解释为整个引导链只执行一次。

## 当前源码公开 API 清单

当前声明由源码语义清单核对，包含 public 和可供外部派生实现使用的 protected 成员。构建契约不作为游戏脚本导出；线程、失败与所有权要求见前文及每项 XML。

### `Inno.Build.TaskHosting.BuildTaskRuntimeTask`

<!-- api: T:Inno.Build.TaskHosting.BuildTaskRuntimeTask d9d3fc8dd66d531a6b3d102baeaeef01b9d847c16f613d8d45009f89b320deb8 -->
<!-- api: P:Inno.Build.TaskHosting.BuildTaskRuntimeTask.ProjectFile b306a36b8f89aa7b446efd2cb47d9568637b065be703f0df8dee109c0efc7d02 -->
<!-- api: P:Inno.Build.TaskHosting.BuildTaskRuntimeTask.ArtifactsDirectory 427a9fee80a47247cbdf286cc5305f4a17c0e48d890ca9d68eaa3c2f57a0c495 -->
<!-- api: P:Inno.Build.TaskHosting.BuildTaskRuntimeTask.LoadDirectory 1e9400edd967d354d278f9378c7b0d5a0818cff5a97c281a3d70c4da734b2593 -->
<!-- api: P:Inno.Build.TaskHosting.BuildTaskRuntimeTask.Targets 09883ba636210c54a8164490f730036185bf7407e3bf7277fcc42f6bae1ebd2b -->
<!-- api: P:Inno.Build.TaskHosting.BuildTaskRuntimeTask.Properties 6f78436bf1c532fb9c48998778b3ce4315a0d16ecdc0cd71f4ffacdd0e9bb3cc -->
<!-- api: P:Inno.Build.TaskHosting.BuildTaskRuntimeTask.RemoveProperties 2a2eab884727ea604b4a27db22fe51a02c7537f0de4a8ad908440ecb22eae1c6 -->
<!-- api: P:Inno.Build.TaskHosting.BuildTaskRuntimeTask.TargetOutputs e41ef260467e6dd53f456b159f6d119c730a735f91e5f13db56db41b8cab2e53 -->
<!-- api: M:Inno.Build.TaskHosting.BuildTaskRuntimeTask.Execute 74a4fd3f0468709cc7515426d299f4a8433b30110de197235c32d7b826663de3 -->
<!-- api: M:Inno.Build.TaskHosting.BuildTaskRuntimeTask.Cancel b998de6a7ea284b64fce669070c6d33344f97110a3c4a4c04c3a801bd0088306 -->
<!-- api: M:Inno.Build.TaskHosting.BuildTaskRuntimeTask.#ctor 652b04defb0ba8da19c2470b821d603a9ef704cbada67cf0353bd7dd4f228949 -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-4f350c8ca30259bc"></a>[`public Inno.Build.TaskHosting.BuildTaskRuntimeTask.BuildTaskRuntimeTask()`](../../build/tasks/Inno.Build.TaskHosting/BuildTaskRuntimeTask.cs#L16) | 由 C# 生成，遵循所属类型的值语义与本页的生命周期约束。 |
| <a id="api-09a77c1ac5da17ab"></a>[`public void Inno.Build.TaskHosting.BuildTaskRuntimeTask.Cancel()`](../../build/tasks/Inno.Build.TaskHosting/BuildTaskRuntimeTask.cs#L213) | Cancels an ownership wait without racing retirement of the cancellation source. |
| <a id="api-94e4e76de32f1d3e"></a>[`public override bool Inno.Build.TaskHosting.BuildTaskRuntimeTask.Execute()`](../../build/tasks/Inno.Build.TaskHosting/BuildTaskRuntimeTask.cs#L70) | Serializes shared writes across processes while yielding the MSBuild node during ownership waits.<br>返回：True after a complete build; false with a logged error after failure or cancellation. |
| <a id="api-c4e5dfbeda0505e4"></a>[`public string Inno.Build.TaskHosting.BuildTaskRuntimeTask.ArtifactsDirectory { get; set; }`](../../build/tasks/Inno.Build.TaskHosting/BuildTaskRuntimeTask.cs#L32) | Gets or sets the absolute shared intermediate owner, also supplied as ArtifactsPath. |
| <a id="api-9a721288367b48a5"></a>[`public string Inno.Build.TaskHosting.BuildTaskRuntimeTask.LoadDirectory { get; set; }`](../../build/tasks/Inno.Build.TaskHosting/BuildTaskRuntimeTask.cs#L38) | Gets or sets the private reader location registered before returning an immutable runtime. |
| <a id="api-eb6622ae93b2bdd1"></a>[`public string Inno.Build.TaskHosting.BuildTaskRuntimeTask.ProjectFile { get; set; }`](../../build/tasks/Inno.Build.TaskHosting/BuildTaskRuntimeTask.cs#L26) | Gets or sets the task project whose complete runtime is built by the calling MSBuild engine. |
| <a id="api-cd916444f9a72d7b"></a>[`public string[] Inno.Build.TaskHosting.BuildTaskRuntimeTask.Properties { get; set; }`](../../build/tasks/Inno.Build.TaskHosting/BuildTaskRuntimeTask.cs#L51) | Gets or sets explicit name=value properties after product-specific global properties are removed. Explicit values override removal of inherited properties with the same name. ArtifactsPath is owned by this task and cannot be overridden. |
| <a id="api-2aa6626ed21fc71d"></a>[`public string[] Inno.Build.TaskHosting.BuildTaskRuntimeTask.RemoveProperties { get; set; }`](../../build/tasks/Inno.Build.TaskHosting/BuildTaskRuntimeTask.cs#L56) | Gets or sets parent global properties that must not enter the host tool build. |
| <a id="api-fb0edd64fec02d2f"></a>[`public Microsoft.Build.Framework.ITaskItem[] Inno.Build.TaskHosting.BuildTaskRuntimeTask.TargetOutputs { get; private set; }`](../../build/tasks/Inno.Build.TaskHosting/BuildTaskRuntimeTask.cs#L62) | Gets the returned runtime files and their hash metadata after successful compilation. |
| <a id="api-e2761437678872f8"></a>[`public string[] Inno.Build.TaskHosting.BuildTaskRuntimeTask.Targets { get; set; }`](../../build/tasks/Inno.Build.TaskHosting/BuildTaskRuntimeTask.cs#L44) | Gets or sets the ordered targets returning the complete hashed runtime closure. |
| <a id="api-3c145afa5eec3ea4"></a>[`public sealed class Inno.Build.TaskHosting.BuildTaskRuntimeTask : Microsoft.Build.Utilities.Task, Microsoft.Build.Framework.ICancelableTask`](../../build/tasks/Inno.Build.TaskHosting/BuildTaskRuntimeTask.cs#L16) | Coordinates shared task-runtime compilation before immutable publication and private loading. |

### `Inno.Build.TaskHosting.MaterializeBindingsTask`

<!-- api: T:Inno.Build.TaskHosting.MaterializeBindingsTask c1763bdc37660ac222d8f855356137d2ede918c42e82b9dc19c99633ca538b2f -->
<!-- api: P:Inno.Build.TaskHosting.MaterializeBindingsTask.Source 77cc73b1bc03bf902037a264f28334d313d4abe2645ceb36a0deaa741ee9c668 -->
<!-- api: P:Inno.Build.TaskHosting.MaterializeBindingsTask.IntermediateDirectory 02291bd45806b57910d0a7ba474f25f0ebfbf212d9917233cbb37308f1761261 -->
<!-- api: P:Inno.Build.TaskHosting.MaterializeBindingsTask.ComponentProject f9ecfefe1761b1c9df0fce3d9e25547754525466ea939a7f974e083b7beb582e -->
<!-- api: P:Inno.Build.TaskHosting.MaterializeBindingsTask.ArtifactDirectory 6ad5463fb3e6edb2a347079eacd3106de42db62aa127b1729582e12ec9842043 -->
<!-- api: P:Inno.Build.TaskHosting.MaterializeBindingsTask.ArtifactOwnerDirectory 0d9e17760f6c9ac9aa00e4a11f85dc3562aa11a3d6bd1ef8a14c9da88e5a5085 -->
<!-- api: P:Inno.Build.TaskHosting.MaterializeBindingsTask.Fingerprint a9f7b5915c7b0f3fef1a92276bf6c27c451d31e26bddd5ed1a236f47b742148d -->
<!-- api: P:Inno.Build.TaskHosting.MaterializeBindingsTask.MaterializedSource 3112bbdf70d491d1a65368ad080bd565d0c930e80d076431baedb4178f7454ec -->
<!-- api: M:Inno.Build.TaskHosting.MaterializeBindingsTask.Cancel 65b5175e762346b5696c9e35e24147fb20af149e9cfaecefa1789cdfa6ea58c4 -->
<!-- api: M:Inno.Build.TaskHosting.MaterializeBindingsTask.Execute a92126735c9df25c8d84518cb4754b66e928866970ea4e93bbf8cba4981429ce -->
<!-- api: M:Inno.Build.TaskHosting.MaterializeBindingsTask.#ctor bbb14862a929e0e886d4779f47c6904e21424965512a65d93b65b823b0eb4318 -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-9dbca49a28856b54"></a>[`public Inno.Build.TaskHosting.MaterializeBindingsTask.MaterializeBindingsTask()`](../../build/tasks/Inno.Build.TaskHosting/MaterializeBindingsTask.cs#L14) | 由 C# 生成，遵循所属类型的值语义与本页的生命周期约束。 |
| <a id="api-e5767f9bdf685263"></a>[`public void Inno.Build.TaskHosting.MaterializeBindingsTask.Cancel()`](../../build/tasks/Inno.Build.TaskHosting/MaterializeBindingsTask.cs#L62) | Cancels ownership acquisition without disturbing an active publisher. |
| <a id="api-7610b75f0d746f23"></a>[`public override bool Inno.Build.TaskHosting.MaterializeBindingsTask.Execute()`](../../build/tasks/Inno.Build.TaskHosting/MaterializeBindingsTask.cs#L75) | Validates and copies the source while holding its publication owner.<br>返回：True for a complete private source; false after logging a failed or canceled copy. |
| <a id="api-6733745ec3335081"></a>[`public string Inno.Build.TaskHosting.MaterializeBindingsTask.ArtifactDirectory { get; set; }`](../../build/tasks/Inno.Build.TaskHosting/MaterializeBindingsTask.cs#L41) | Gets or sets the immutable artifact root; empty denotes mutable host-source publication. |
| <a id="api-d1f660332f096078"></a>[`public string Inno.Build.TaskHosting.MaterializeBindingsTask.ArtifactOwnerDirectory { get; set; }`](../../build/tasks/Inno.Build.TaskHosting/MaterializeBindingsTask.cs#L46) | Gets or sets the exact publisher root coordinating immutable binding readers and retirement. |
| <a id="api-e3e8ffd0991fcd6e"></a>[`public string Inno.Build.TaskHosting.MaterializeBindingsTask.ComponentProject { get; set; }`](../../build/tasks/Inno.Build.TaskHosting/MaterializeBindingsTask.cs#L36) | Gets or sets the Native project owning mutable host-source publication. |
| <a id="api-b60abceaf43401bc"></a>[`public string Inno.Build.TaskHosting.MaterializeBindingsTask.Fingerprint { get; set; }`](../../build/tasks/Inno.Build.TaskHosting/MaterializeBindingsTask.cs#L51) | Gets or sets the exact generation identity required when consuming an immutable artifact. |
| <a id="api-fbff53108ad64cc2"></a>[`public string Inno.Build.TaskHosting.MaterializeBindingsTask.IntermediateDirectory { get; set; }`](../../build/tasks/Inno.Build.TaskHosting/MaterializeBindingsTask.cs#L30) | Gets or sets the absolute SDK-owned intermediate root. |
| <a id="api-c86cf064bcd8f170"></a>[`public string Inno.Build.TaskHosting.MaterializeBindingsTask.MaterializedSource { get; private set; }`](../../build/tasks/Inno.Build.TaskHosting/MaterializeBindingsTask.cs#L57) | Gets the only generated Compile source, owned by this SDK build after the task returns. |
| <a id="api-b087270b4dc07de0"></a>[`public string Inno.Build.TaskHosting.MaterializeBindingsTask.Source { get; set; }`](../../build/tasks/Inno.Build.TaskHosting/MaterializeBindingsTask.cs#L24) | Gets or sets the exact selected managed source, without selecting another generation. |
| <a id="api-26b0f1776c89752a"></a>[`public sealed class Inno.Build.TaskHosting.MaterializeBindingsTask : Microsoft.Build.Utilities.Task, Microsoft.Build.Framework.ICancelableTask`](../../build/tasks/Inno.Build.TaskHosting/MaterializeBindingsTask.cs#L14) | Copies one protected generated source into the consuming SDK build's own intermediate directory. |

### `Inno.Build.TaskHosting.PublishTaskHostTask`

<!-- api: T:Inno.Build.TaskHosting.PublishTaskHostTask d696247cb076b034b2bba04aa92df91512535af9f1f4f1d312c6eaf9322d75f6 -->
<!-- api: P:Inno.Build.TaskHosting.PublishTaskHostTask.InputFiles 33b622830609c165b4b6560c0ffcfb2f3a14a6d184d8ccb065fdbb84af5bf106 -->
<!-- api: P:Inno.Build.TaskHosting.PublishTaskHostTask.CacheDirectory 66167aae3f49adc96c623dad87a6f189c65ac6353baadca5230c3642e2a1464e -->
<!-- api: P:Inno.Build.TaskHosting.PublishTaskHostTask.LoadDirectory f270466b8fe028ad1cd52f20ada58f0200881b2bcaf7ba768d80055186b048f3 -->
<!-- api: P:Inno.Build.TaskHosting.PublishTaskHostTask.PublisherDirectory 2c6d4e5b4b18e74f47788a47936b04fe03fa1eec999675026dda108e9f4989a8 -->
<!-- api: P:Inno.Build.TaskHosting.PublishTaskHostTask.PublishedAssembly 06aa075e1c39e2656c4814cbaab92dc8f41cd64bc629d66cb84a34fa6f2814f5 -->
<!-- api: M:Inno.Build.TaskHosting.PublishTaskHostTask.Execute fd847577c17f7763f8fe20514f57f793aa82ac903aa798e8b169f47d2eb9b9f0 -->
<!-- api: M:Inno.Build.TaskHosting.PublishTaskHostTask.Cancel 8a9d2bb9fc1142a94f4aa1f3e67e75fb9c6561c26148297fd81015f7b592ffd5 -->
<!-- api: M:Inno.Build.TaskHosting.PublishTaskHostTask.#ctor ab62439019e01cf1583686f2f480c1911935374c9f901092e08c29aa672fd9e0 -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-64a83f71a923936b"></a>[`public Inno.Build.TaskHosting.PublishTaskHostTask.PublishTaskHostTask()`](../../build/tasks/Inno.Build.TaskHosting/PublishTaskHostTask.cs#L19) | 由 C# 生成，遵循所属类型的值语义与本页的生命周期约束。 |
| <a id="api-c66051fe232266e1"></a>[`public void Inno.Build.TaskHosting.PublishTaskHostTask.Cancel()`](../../build/tasks/Inno.Build.TaskHosting/PublishTaskHostTask.cs#L127) | Cancels ownership waits and unpublished copies without racing token retirement. |
| <a id="api-8134b8f157f6c713"></a>[`public override bool Inno.Build.TaskHosting.PublishTaskHostTask.Execute()`](../../build/tasks/Inno.Build.TaskHosting/PublishTaskHostTask.cs#L62) | Verifies sources and cached files, publishes a complete candidate under exclusive ownership, and registers the private reader before returning its shared runtime.<br>返回：True when a complete runtime was published or reused; false with a logged error otherwise. |
| <a id="api-100a520324c68aca"></a>[`public string Inno.Build.TaskHosting.PublishTaskHostTask.CacheDirectory { get; set; }`](../../build/tasks/Inno.Build.TaskHosting/PublishTaskHostTask.cs#L35) | Gets or sets the absolute owner of content-addressed, shared task runtime snapshots. |
| <a id="api-1a63ce224cb7903e"></a>[`public Microsoft.Build.Framework.ITaskItem[] Inno.Build.TaskHosting.PublishTaskHostTask.InputFiles { get; set; }`](../../build/tasks/Inno.Build.TaskHosting/PublishTaskHostTask.cs#L29) | Gets or sets the frozen runtime files, each declaring RelativePath and FileHash metadata. |
| <a id="api-0d9ad49213c70af8"></a>[`public string Inno.Build.TaskHosting.PublishTaskHostTask.LoadDirectory { get; set; }`](../../build/tasks/Inno.Build.TaskHosting/PublishTaskHostTask.cs#L41) | Gets or sets the absolute private load directory whose parent receives this process's ownership marker. |
| <a id="api-6465ee9dda416a69"></a>[`public string Inno.Build.TaskHosting.PublishTaskHostTask.PublishedAssembly { get; private set; }`](../../build/tasks/Inno.Build.TaskHosting/PublishTaskHostTask.cs#L53) | Gets the verified immutable task assembly path after successful publication. |
| <a id="api-89c47e64b5a8aabe"></a>[`public string Inno.Build.TaskHosting.PublishTaskHostTask.PublisherDirectory { get; set; }`](../../build/tasks/Inno.Build.TaskHosting/PublishTaskHostTask.cs#L47) | Gets or sets an optional private bootstrap directory to register under the same process ownership. An empty value leaves the caller's publisher assembly lifetime outside this cache. |
| <a id="api-16b3e5e4461b4feb"></a>[`public sealed class Inno.Build.TaskHosting.PublishTaskHostTask : Microsoft.Build.Utilities.Task, Microsoft.Build.Framework.ICancelableTask`](../../build/tasks/Inno.Build.TaskHosting/PublishTaskHostTask.cs#L19) | Publishes one immutable task runtime per content identity before a build loads its private closure. This bootstrap depends only on filesystem primitives and never loads the task runtime being built. |

### `Inno.Build.TaskHosting.RegisterTaskPublisherTask`

<!-- api: T:Inno.Build.TaskHosting.RegisterTaskPublisherTask 3ef705e4897445289d182a9f1f6e601ac7715020723892d6d68905421c795019 -->
<!-- api: P:Inno.Build.TaskHosting.RegisterTaskPublisherTask.PublisherRoot 2c0906caba9977a8082e3fd499c42fc8051fdbf91bf00c14cce2e0bcc5fef933 -->
<!-- api: P:Inno.Build.TaskHosting.RegisterTaskPublisherTask.PublisherDirectory 7d9cfb56c8722f2053168c56edf1998f38a1387c0c3ba38dbbf7c33b2294e52a -->
<!-- api: P:Inno.Build.TaskHosting.RegisterTaskPublisherTask.BootstrapDirectory 907b600532607423af1e90701c2146678198e12b6d53941e4be04dd39fb5a543 -->
<!-- api: M:Inno.Build.TaskHosting.RegisterTaskPublisherTask.Execute 3fca9448756f3a0ea0b47f58ae02251597a3c8a4c85c465723457cbf23216bcd -->
<!-- api: M:Inno.Build.TaskHosting.RegisterTaskPublisherTask.#ctor 2039ab855039cd8fd80d80b08ffc0869fe7a90ba4933810e1f1ea7e3383e0825 -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-60a8a728a76540d4"></a>[`public Inno.Build.TaskHosting.RegisterTaskPublisherTask.RegisterTaskPublisherTask()`](../../build/tasks/Inno.Build.TaskHosting/RegisterTaskPublisherTask.cs#L12) | 由 C# 生成，遵循所属类型的值语义与本页的生命周期约束。 |
| <a id="api-49ae2d2fe2fd7664"></a>[`public override bool Inno.Build.TaskHosting.RegisterTaskPublisherTask.Execute()`](../../build/tasks/Inno.Build.TaskHosting/RegisterTaskPublisherTask.cs#L39) | Records ownership using the same protocol as full task-runtime publication.<br>返回：True after recording the current process, or false after reporting an invalid owner or IO failure. |
| <a id="api-d1b6ba8af37cbce0"></a>[`public string Inno.Build.TaskHosting.RegisterTaskPublisherTask.BootstrapDirectory { get; set; }`](../../build/tasks/Inno.Build.TaskHosting/RegisterTaskPublisherTask.cs#L31) | Gets or sets the operation-owned compilation directory shared by publishers in this project invocation. This directory is never used as a task assembly load location. |
| <a id="api-f986bbab253e19ec"></a>[`public string Inno.Build.TaskHosting.RegisterTaskPublisherTask.PublisherDirectory { get; set; }`](../../build/tasks/Inno.Build.TaskHosting/RegisterTaskPublisherTask.cs#L24) | Gets or sets the private configuration directory whose assembly was loaded for this invocation. |
| <a id="api-2aca6f43282329b5"></a>[`public string Inno.Build.TaskHosting.RegisterTaskPublisherTask.PublisherRoot { get; set; }`](../../build/tasks/Inno.Build.TaskHosting/RegisterTaskPublisherTask.cs#L18) | Gets or sets the registered cache root containing private publisher directories. |
| <a id="api-5be0507a3a95ef62"></a>[`public sealed class Inno.Build.TaskHosting.RegisterTaskPublisherTask : Microsoft.Build.Utilities.Task`](../../build/tasks/Inno.Build.TaskHosting/RegisterTaskPublisherTask.cs#L12) | Registers the lightweight publisher's process owner before any task uses its private load directory. |

### `Inno.Build.TaskHosting.RestoreOwnedProjectTask`

<!-- api: T:Inno.Build.TaskHosting.RestoreOwnedProjectTask 6f31b95a8f496caf02c2483c4894e4c83bfc3251b9607d0f8702f536556e434f -->
<!-- api: P:Inno.Build.TaskHosting.RestoreOwnedProjectTask.Projects 70a40a4f1f014e46d93fe2e5ebfb9fd551a1bcbcd694d406eac6e2fb5c7d477d -->
<!-- api: P:Inno.Build.TaskHosting.RestoreOwnedProjectTask.LockPath ee801d4b0d603e4397cd9056fc099d65513b063e1a7893867a377496bdfd3796 -->
<!-- api: P:Inno.Build.TaskHosting.RestoreOwnedProjectTask.Properties a309b4d03281a1d0f68f419a7663ef426d55d6c522a3dc732ce0048fc7b5b6c2 -->
<!-- api: M:Inno.Build.TaskHosting.RestoreOwnedProjectTask.Execute 9caf84b62ec67f927bd580e55ff31192baeebfd1e237749da05207c4a9cf3bb0 -->
<!-- api: M:Inno.Build.TaskHosting.RestoreOwnedProjectTask.Cancel 436559386966a95db2741b448c6c47b16f38c755eff61221a61ab10f42ec1dd5 -->
<!-- api: M:Inno.Build.TaskHosting.RestoreOwnedProjectTask.#ctor 6bab79746becdd1c14dfbc5595803730a0d5054e5408e9960b39a4c165fd372b -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-30f2e219471d99ee"></a>[`public Inno.Build.TaskHosting.RestoreOwnedProjectTask.RestoreOwnedProjectTask()`](../../build/tasks/Inno.Build.TaskHosting/RestoreOwnedProjectTask.cs#L16) | 由 C# 生成，遵循所属类型的值语义与本页的生命周期约束。 |
| <a id="api-d47b8a52fe06cb8e"></a>[`public void Inno.Build.TaskHosting.RestoreOwnedProjectTask.Cancel()`](../../build/tasks/Inno.Build.TaskHosting/RestoreOwnedProjectTask.cs#L119) | Cancels this writer's ownership wait without canceling another active restore. |
| <a id="api-68d8ec3db254ce20"></a>[`public override bool Inno.Build.TaskHosting.RestoreOwnedProjectTask.Execute()`](../../build/tasks/Inno.Build.TaskHosting/RestoreOwnedProjectTask.cs#L46) | Waits outside the MSBuild node, then restores under exclusive cross-process write ownership.<br>返回：True after every requested restore succeeds; false with a logged failure or cancellation. |
| <a id="api-8e8ab3225a285e79"></a>[`public string Inno.Build.TaskHosting.RestoreOwnedProjectTask.LockPath { get; set; }`](../../build/tasks/Inno.Build.TaskHosting/RestoreOwnedProjectTask.cs#L33) | Gets or sets the absolute lock belonging to all writers of the shared restore graph. The lock file remains after release. |
| <a id="api-ff2328862d146380"></a>[`public Microsoft.Build.Framework.ITaskItem[] Inno.Build.TaskHosting.RestoreOwnedProjectTask.Projects { get; set; }`](../../build/tasks/Inno.Build.TaskHosting/RestoreOwnedProjectTask.cs#L26) | Gets or sets projects with explicit AdditionalProperties and GlobalPropertiesToRemove metadata. |
| <a id="api-35f3e62b2ee88af7"></a>[`public string[] Inno.Build.TaskHosting.RestoreOwnedProjectTask.Properties { get; set; }`](../../build/tasks/Inno.Build.TaskHosting/RestoreOwnedProjectTask.cs#L38) | Gets or sets explicit name=value restore properties, independent of product discovery. |
| <a id="api-2e577b3d3a13e7f9"></a>[`public sealed class Inno.Build.TaskHosting.RestoreOwnedProjectTask : Microsoft.Build.Utilities.Task, Microsoft.Build.Framework.ICancelableTask`](../../build/tasks/Inno.Build.TaskHosting/RestoreOwnedProjectTask.cs#L16) | Coordinates SDK restore writes for explicitly selected project closures. Parent solution entry points are excluded from these owned restores. |

### `Inno.Build.TaskHosting.TaskRuntimeBootstrapOwner`

<!-- api: T:Inno.Build.TaskHosting.TaskRuntimeBootstrapOwner 6c885efa1310f5c64df9c9d750c2741d682ac57330e2a1037baa70c9ece8d88b -->
<!-- api: P:Inno.Build.TaskHosting.TaskRuntimeBootstrapOwner.Root cb738fb5bb94c4605d56ceec74da71011624901a5ec6b4b17be6a21832a2adc6 -->
<!-- api: P:Inno.Build.TaskHosting.TaskRuntimeBootstrapOwner.Identity 9d9a33375fdb66a11c1982e7eb507ae212a07a7bba38d609e6905a5b0de6e114 -->
<!-- api: P:Inno.Build.TaskHosting.TaskRuntimeBootstrapOwner.FoundationAssembly 68eb508d0b15d2c62cacbd3e28b2c450b0fd6dc4946db56fbb42d72ba9c47330 -->
<!-- api: P:Inno.Build.TaskHosting.TaskRuntimeBootstrapOwner.Directory 8ded160ca212b760915954fe9ca6860aa34c7df69bcbfcf57c9ed55f4f80f593 -->
<!-- api: M:Inno.Build.TaskHosting.TaskRuntimeBootstrapOwner.Execute a92eb78bb503eed207c7e3cc3013c09a0821af431cbe2af8843d596681160a11 -->
<!-- api: M:Inno.Build.TaskHosting.TaskRuntimeBootstrapOwner.#ctor e8d9c89fcf975310aa42199c9caccd7c15f394dd6a435b96571cd2b3ba1809c0 -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-b8337cdb3323d9d9"></a>[`public Inno.Build.TaskHosting.TaskRuntimeBootstrapOwner.TaskRuntimeBootstrapOwner()`](../../build/tasks/Inno.Build.TaskHosting/TaskRuntimeBootstrapOwner.cs#L13) | 由 C# 生成，遵循所属类型的值语义与本页的生命周期约束。 |
| <a id="api-65479adfba3085af"></a>[`public override bool Inno.Build.TaskHosting.TaskRuntimeBootstrapOwner.Execute()`](../../build/tasks/Inno.Build.TaskHosting/TaskRuntimeBootstrapOwner.cs#L45) | Establishes ownership through Core.IO; no SDK object or lease enters the Build registry.<br>返回：True after a new claim, or false with an error before payload compilation. |
| <a id="api-ac8a6472636a0774"></a>[`public string Inno.Build.TaskHosting.TaskRuntimeBootstrapOwner.Directory { get; private set; }`](../../build/tasks/Inno.Build.TaskHosting/TaskRuntimeBootstrapOwner.cs#L37) | Gets the new process-owned directory whose claim precedes all bootstrap payloads. |
| <a id="api-4c7de22179e2ea95"></a>[`public string Inno.Build.TaskHosting.TaskRuntimeBootstrapOwner.FoundationAssembly { get; set; }`](../../build/tasks/Inno.Build.TaskHosting/TaskRuntimeBootstrapOwner.cs#L31) | Gets or sets the freshly built Foundation assembly selected by this SDK bootstrap. |
| <a id="api-6cb80b7cfa19243c"></a>[`public string Inno.Build.TaskHosting.TaskRuntimeBootstrapOwner.Identity { get; set; }`](../../build/tasks/Inno.Build.TaskHosting/TaskRuntimeBootstrapOwner.cs#L25) | Gets or sets the current operation identity already propagated through the project graph. |
| <a id="api-e44083ab7509d2ff"></a>[`public string Inno.Build.TaskHosting.TaskRuntimeBootstrapOwner.Root { get; set; }`](../../build/tasks/Inno.Build.TaskHosting/TaskRuntimeBootstrapOwner.cs#L19) | Gets or sets the absolute publisher root managed by the standard distribution. |
| <a id="api-4e36554364566d29"></a>[`public sealed class Inno.Build.TaskHosting.TaskRuntimeBootstrapOwner : Microsoft.Build.Utilities.Task`](../../build/tasks/Inno.Build.TaskHosting/TaskRuntimeBootstrapOwner.cs#L13) | Claims bootstrap storage before SDK compilation without depending on the full task runtime. |

## 项目依赖

Owned restore 移除实际引用声明的 `GlobalPropertiesToRemove` 和父 Solution 入口，在 Core.IO 写 lease 下还原明确闭包。代码生成器在 SDK props 之前声明工具身份，共同产物规则让其还原和编译使用同一中立目录；不在编译阶段补写 NuGet imports。CLI 不维护生成器项目名单，不复制另一目标的 assets。此行为同时服务普通 IDE Build 与 Rebuild。

- [Inno.Core.IO](../core/Inno.Core.IO.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Toolchains](Inno.Build.Toolchains.md)：绑定产物完整性及读取 lease 的实现依赖，PrivateAssets="compile"。

### 已解析包输入

Task 请求身份同时读取 SDK 还原清单、所选目标的编译/运行/构建资产和包内 C# analyzer 输入。校验实际字节，不把包版本字符串或缓存时间戳当作完整性证明。选中资产缺失立即失败；不从其他框架选择替代 DLL。包内未选框架的普通 DLL 不进入该目标的文件集合。

这是 Task 运行闭包的内部输入分析；它尚不是 Support Pack 的完整准备身份。任意自定义 target 在执行时生成的新输入，仍需由其实际编译流程声明和验证。


## 进程与构建所有权 API

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4e36554364566d29"></a>[`public sealed class Inno.Build.TaskHosting.TaskRuntimeBootstrapOwner : Microsoft.Build.Utilities.Task`](../../build/tasks/Inno.Build.TaskHosting/TaskRuntimeBootstrapOwner.cs#L12) | 轻量 SDK Task 适配，不包含另一套 claim writer。 | SDK 调用 Core.IO 的公开 claim；只交付 BCL 字符串，不保存 ALC 或 lease。 | 初始化/边界失败报告 MSBuild error，并阻止 payload 编译。 |
| <a id="api-e44083ab7509d2ff"></a>[`public string Inno.Build.TaskHosting.TaskRuntimeBootstrapOwner.Root { get; set; }`](../../build/tasks/Inno.Build.TaskHosting/TaskRuntimeBootstrapOwner.cs#L18) | 标准发行登记的绝对 publisher 根。 | SDK 调用 Core.IO 的公开 claim；只交付 BCL 字符串，不保存 ALC 或 lease。 | 初始化/边界失败报告 MSBuild error，并阻止 payload 编译。 |
| <a id="api-6cb80b7cfa19243c"></a>[`public string Inno.Build.TaskHosting.TaskRuntimeBootstrapOwner.Identity { get; set; }`](../../build/tasks/Inno.Build.TaskHosting/TaskRuntimeBootstrapOwner.cs#L24) | 引用图共享的当前操作 GUID，不能每个 target 重新生成。 | SDK 调用 Core.IO 的公开 claim；只交付 BCL 字符串，不保存 ALC 或 lease。 | 初始化/边界失败报告 MSBuild error，并阻止 payload 编译。 |
| <a id="api-4c7de22179e2ea95"></a>[`public string Inno.Build.TaskHosting.TaskRuntimeBootstrapOwner.FoundationAssembly { get; set; }`](../../build/tasks/Inno.Build.TaskHosting/TaskRuntimeBootstrapOwner.cs#L30) | 当前 SDK 构建的 Foundation 程序集路径，用于调用公开 Core.IO 入口。 | SDK 调用 Core.IO 的公开 claim；只交付 BCL 字符串，不保存 ALC 或 lease。 | 初始化/边界失败报告 MSBuild error，并阻止 payload 编译。 |
| <a id="api-ac8a6472636a0774"></a>[`public string Inno.Build.TaskHosting.TaskRuntimeBootstrapOwner.Directory { get; private set; }`](../../build/tasks/Inno.Build.TaskHosting/TaskRuntimeBootstrapOwner.cs#L36) | 首次 payload 前已经登记的 bootstrap 目录。 | SDK 调用 Core.IO 的公开 claim；只交付 BCL 字符串，不保存 ALC 或 lease。 | 初始化/边界失败报告 MSBuild error，并阻止 payload 编译。 |
| <a id="api-65479adfba3085af"></a>[`public override bool Inno.Build.TaskHosting.TaskRuntimeBootstrapOwner.Execute()`](../../build/tasks/Inno.Build.TaskHosting/TaskRuntimeBootstrapOwner.cs#L44) | 建立当前进程 claim，成功返回 true；失败记录诊断并返回 false。 | SDK 调用 Core.IO 的公开 claim；只交付 BCL 字符串，不保存 ALC 或 lease。 | 初始化/边界失败报告 MSBuild error，并阻止 payload 编译。 |
| <a id="api-b8337cdb3323d9d9"></a>[`public Inno.Build.TaskHosting.TaskRuntimeBootstrapOwner.TaskRuntimeBootstrapOwner()`](../../build/tasks/Inno.Build.TaskHosting/TaskRuntimeBootstrapOwner.cs#L12) | 创建尚未配置的 SDK Task；执行前设置 Required 属性。 | SDK 调用 Core.IO 的公开 claim；只交付 BCL 字符串，不保存 ALC 或 lease。 | 初始化/边界失败报告 MSBuild error，并阻止 payload 编译。 |

<!-- api: T:Inno.Build.TaskHosting.TaskRuntimeBootstrapOwner 6c885efa1310f5c64df9c9d750c2741d682ac57330e2a1037baa70c9ece8d88b -->
<!-- api: P:Inno.Build.TaskHosting.TaskRuntimeBootstrapOwner.Root cb738fb5bb94c4605d56ceec74da71011624901a5ec6b4b17be6a21832a2adc6 -->
<!-- api: P:Inno.Build.TaskHosting.TaskRuntimeBootstrapOwner.Identity 9d9a33375fdb66a11c1982e7eb507ae212a07a7bba38d609e6905a5b0de6e114 -->
<!-- api: P:Inno.Build.TaskHosting.TaskRuntimeBootstrapOwner.FoundationAssembly 68eb508d0b15d2c62cacbd3e28b2c450b0fd6dc4946db56fbb42d72ba9c47330 -->
<!-- api: P:Inno.Build.TaskHosting.TaskRuntimeBootstrapOwner.Directory 8ded160ca212b760915954fe9ca6860aa34c7df69bcbfcf57c9ed55f4f80f593 -->
<!-- api: M:Inno.Build.TaskHosting.TaskRuntimeBootstrapOwner.Execute a92eb78bb503eed207c7e3cc3013c09a0821af431cbe2af8843d596681160a11 -->
<!-- api: M:Inno.Build.TaskHosting.TaskRuntimeBootstrapOwner.#ctor e8d9c89fcf975310aa42199c9caccd7c15f394dd6a435b96571cd2b3ba1809c0 -->
