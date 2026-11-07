# 五项局部边界整改执行计划

基线：`48b33e5d089e8a1500f03d1abae69fd61ffa9d62`。保留现有 Wiki 审查修改；不提交、不修改 extern/BGCS/生成绑定、不执行 GUI 验收。

## 设计与完成条件

| 项目 | 唯一职责与方案 | 门禁 |
|---|---|---|
| F01 | Core.IO 的所有权路径采用规范化后 ordinal 拼写；拒绝大小写别名。逐段 link 检查使用已经验证的后缀，不再用 OS 推测卷语义 | 大小写 sibling、遍历、卷根、link、原子替换公开契约测试 |
| F02 | Rendering 在图内保存已接受前缀的资源状态与依赖；新增声明增量分析，完整候选仍检查 live passes、循环、输出与 view limits；修改未冻结 pass 或回滚时重置分析 | 前缀/完整构建等价、未来 phase、共享读写、嵌套回滚、失败隔离；真实帧分配重新测量 |
| F03 | CLI、Task、BindingExtension 按所选 SDK 的 NETCoreSdkRuntimeIdentifier 恢复 Native 包资产，独立于游戏目标 | 实际 restore 图与输出 RID；生成器和 extension 实际加载；包锁同步 |
| F04 | 延迟资源使用现有 managed ulong 提交序号；Native readback 的 uint ready frame 在请求时映射为单调序号 | Native readback/取消/关闭回归；回绕算术检查；不得提前释放 |
| F05 | 中立 Toolchains 提供通用工具 publication 和轻量缓存维护；Bootstrap 显式路由 cache clean。发行登记唯一 owner catalog，维护仅消费已登记根，不依赖 backend/创作工具 | 冷维护闭包不含 BGCS/Editor；相同清理与保护规则；未知登记拒绝；worker 失败与取消保持旧 publication |

## 文件归属

```text
src/foundation/core/Inno.Core.IO/
  PathBoundary.cs
  AtomicFile.cs
src/services/rendering/Inno.Rendering/Graph/
  RenderGraphValidationWorkspace.cs
  RenderGraphBuilder[.Mutations].cs
  RenderGraphValidator.cs
  RenderGraphDependencyAnalysis.cs
  RenderGraphScheduling.cs
backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/
  BgfxDevice.Retirement.cs
  BgfxDevice.Readback.cs
build/toolchains/Inno.Build.Toolchains/
  Managed/ManagedToolRuntime.cs
  Cache/BuildCacheOwnerCatalog.cs
  Maintenance/BuildMaintenanceApplication.cs
build/distributions/Inno.Build.Distribution.Standard/StandardBuildCaches.cs
build/cli/Inno.Build.Cli/BuildApplication.cs
build/tasks/Inno.Build.TaskHosting/TaskRuntimeRequestKey.cs
entry/Inno.Entry.Bootstrap/
  DevelopmentToolDefinition.cs
  DevelopmentEntry.cs
  DevelopmentBuildTool.cs
  DevelopmentBuildWorker[Request,Process].cs
tests/core/Inno.Core.IO.Tests/
tests/rendering/Inno.Rendering.Tests/
tests/rendering/Inno.Rendering.Runtime.Tests/
tests/build/Inno.Build.Tests/
docs/core/, docs/rendering/, docs/build/, docs/entry/, docs/backends/Bgfx/
records/architecture/acceptance/LOCAL_BOUNDARY_REFINEMENT_ACCEPTANCE.md
artifacts/acceptance/local-boundary-refinement/20261014-implementation/
  baseline/, results/, logs/
```

删除被替代的 CLI 专用 publication 实现和签名，同步全部调用方。只有两个生产启动项目，不新增生产可执行程序或第二份 backend 组件列表。维护 catalog 仅登记 publisher 根，不认领目录中的未登记 generation。

## 顺序与验证

1. 保存工作区与磁盘基线。
2. F01/F04 源码、公开契约与文档。
3. F02 增量分析、回滚、实际分配测试。
4. F03 工具 RID、F05 维护与通用 publication 的真实消费者闭包。
5. SourceDebug/SourceRelease、架构、API/Wiki/链接、受影响构建与非 GUI 运行。
6. 记录实际耗时、输出大小及未实测 macOS/GUI 限制；仅回收本轮明确 owner 的失效中间产物。

200 GB 空间目标受前轮已记录的授权范围和外部占用限制；禁止绕过先前拒绝的 37 个目录删除。当前产物、日志、源码、SDK 和 NuGet 保留。
