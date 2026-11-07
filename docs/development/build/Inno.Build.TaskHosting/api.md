# Inno.Build.TaskHosting API

[项目入口](../Inno.Build.TaskHosting.md)

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
