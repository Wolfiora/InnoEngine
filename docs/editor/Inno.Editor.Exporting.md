# Inno.Editor.Exporting

[Editor 索引](README.md) · [Build API](../build/Inno.Build.md) · [Support Pack](../build/Inno.Build.SupportPacks.Core.md) · [Wiki 首页](../README.md)

## 职责与边界

本项目拥有 File 菜单的 `Export as Plugin...`、`Export as Game...` Action，以及 Game 设置、Game 进度和 Plugin 三个 modal。它只保存本次导出的 draft、进度与取消状态，实际快照、脚本编译、内容打包、平台布局及原子提交属于 `Inno.Build`。Project `Assets` 是唯一创作源，导出结果不回写项目设置。

该项目没有新的公共类型或稳定 `protected` 扩展点。`ExportWindowModule`、Action 和 Modal 均为 Editor 内部实现，不能替代 [BuildPipeline](../build/Inno.Build.md) 的公开契约。

## Game 工作流

1. 打开 modal 时从 `Settings.Build.inno` 复制 Game 默认值；Application ID 始终由当前 Project ID 给出。
2. 用户点击 Export 后，设置窗口立即关闭，进度以不可拖动且阻断其他交互的 modal 显示，并提供 Cancel。先调用 `BuildPipeline.EnsurePlayerSupportPackAsync(target, deployment)`，在启动外部工具前检查目标与托管部署能力。源码工作区中若目标 Pack 不存在，会在后台发布并校验；已有有效 Pack 直接通过，损坏 Pack 明确失败。构建成功、失败或取消后进度 modal 自动关闭，结果写入 Console。
3. 预备任务完成后，`OnUpdate` 在 Editor 主线程启动 `BuildGameAsync`。资产数据库与脚本发现因此保持原来的 owner-thread 边界；Support Pack 发布期间 UI 可继续刷新、显示状态并响应取消。
4. Build 管线捕获当前组合 generation，编译目标 runtime scripts 和 Shader/Texture 产物，导出 runtime Asset closure，组合已验证的 Pack，最后以带回滚的目录安装提交目标产物。

导出期间不能打开或开始另一种导出，避免两个任务共享或提前释放取消源。Module 停止时先取消准备与构建，再通过现有 `RetirementPendingException` / Editor 退休屏障等待其清理完成；只有全部任务完成后才观察结果、清空进度及释放取消源。退出期间完成的准备任务不会启动新的构建。
直接使用 Build API 时先显式调用 `EnsurePlayerSupportPackAsync`，再在 owner thread 启动 `BuildGameAsync`。
Editor/CLI 已完成这个协调；源码工作区中的 Project 用户不需要手动生成。独立发行的 Editor 不带引擎源码，必须预装目标 Pack。

## Plugin 工作流

`Export as Plugin...` 使用同一 `BuildPipeline.BuildPluginAsync`；依赖声明、只读安装内容和源资产闭包由 Build/Plugin 层处理。取消或失败不会把半成品安装成 Plugin。

## 错误与生命周期

取消 token 同时覆盖 Pack 预备和 Game 构建。Pack 发布失败、Startup Scene 不可部署、脚本诊断、损坏 Artifact 或平台打包错误写入 Console；提交前失败不安装新的游戏输出。提交后旧备份清理失败会保留完整新输出，并报告残留备份路径。设置表单只由外层 modal 滚动，不嵌套第二个可滚动 Child；字段使用共享的 2:3 标签/输入列，标签随列宽换行。窗口关闭时进行中的构建会取消。`Settings.Build.inno` 只提供默认值，modal 中的临时改动不会写回。Support Pack 与游戏内容都不进入 Project `Assets`。

## 相邻页面

- [Inno.Build](../build/Inno.Build.md)：公开构建请求、结果与目标边界。
- [Inno.Build.Cli](../build/Inno.Build.Cli.md)：使用同一 Build Pipeline 的无界面入口。



## 异步服务与界面分离

EditorExportService 是无界面的导出 owner：StartGame/StartPlugin、Cancel、progress、错误和任务 drain 全部复用 Build pipeline 与 LifetimeScope。Workbench 拥有输入表单与阻塞 modal。服务不引用 ImGui、Panel 或 hosting；关闭时先取消并等待任务释放资源，不能让隐藏窗口丢弃活动导出。

## 当前源码公开 API 清单

此清单对应当前 SDK 求值与源码符号；内部类型不构成公开 API。

<!-- api: T:Inno.Editor.Exporting.EditorExportService 995c4025904eeb6bc8f8f50e1b6ab431bb91fb26009a7e04f0661c48d9eddc5c -->
<!-- api: P:Inno.Editor.Exporting.EditorExportService.isPluginBusy fe9db57d1a5c07126b2ec6c208f1dfbf96338d6ab0ea91f5b78a192a47016b25 -->
<!-- api: P:Inno.Editor.Exporting.EditorExportService.isGameBusy 1b2696e78d42b3c681568dc4f1f67c3a880791d37e29191e0fea167f3f2db73d -->
<!-- api: P:Inno.Editor.Exporting.EditorExportService.status efafd6b704c12b0d0060d0fff1c349b537f5ef9ca8261f6c3188f0920ecefa60 -->
<!-- api: P:Inno.Editor.Exporting.EditorExportService.error b13bdbd34fd3f2350925e397aa04b19939419c397855e3eb1cccdbe1e3213ad2 -->
<!-- api: P:Inno.Editor.Exporting.EditorExportService.gameProgress 59240d5bb41db71108d067948ec06dbc19423f8ee39fe10154de11c1ada54d21 -->
<!-- api: P:Inno.Editor.Exporting.EditorExportService.availableGameTargets b8daf0ac539e01967606cd0bf67863da344a8168cbf43552f0fcbc7f671dc78d -->
<!-- api: M:Inno.Editor.Exporting.EditorExportService.GetManagedDeployments(Inno.Build.BuildTargetId) 9913b861d9d5b3d9c40e4b435301ce68d28e238ec29770a54b56b581d7f7f600 -->
<!-- api: M:Inno.Editor.Exporting.EditorExportService.GetGameTargetDisplayName(Inno.Build.BuildTargetId) 6a1a2d5419b5aef08b4af3fb37c09042955638ffe1722b920acb70716bcf6589 -->
<!-- api: M:Inno.Editor.Exporting.EditorExportService.StartPlugin(Inno.Build.PluginBuildRequest) f571fbeaae48a62bd09420f8ac5b6d993e254f45c7990a6bceabaec81d18e7e0 -->
<!-- api: M:Inno.Editor.Exporting.EditorExportService.StartGame(Inno.Build.GameBuildRequest) 0232370852b1f315ca278efa7b9dc92d01aee677621c0b5fa8183ec7a2973083 -->
<!-- api: M:Inno.Editor.Exporting.EditorExportService.Cancel 4e2be7e852c21798cd5c369a23e0cdc827ad13cef1f580ec713d943cbfaeb8a9 -->
<!-- api: M:Inno.Editor.Exporting.EditorExportService.OnUpdate(Inno.Editor.Core.EditorContext) ba8a98ba76386aeea98f247eaddfe6f24acbfe92eedd2d4e2118d52f38e7efb4 -->
<!-- api: M:Inno.Editor.Exporting.EditorExportService.OnStop(Inno.Editor.Core.EditorContext) 4eece8075a998fc217e07dc375997a413c3f6451ef8bfe4b2a42fa0c528bc317 -->
<!-- api: M:Inno.Editor.Exporting.EditorExportService.OnDispose 7248a0baa9aee7cc40a31fcb54a3bfc0507cc8b9123dc632cb7235e09d986c79 -->

| API 与源码 | 契约与参数 | 返回值 | 重要异常 |
| --- | --- | --- | --- |
| <a id="api-76c78e31dbe32b8f"></a>[`public sealed class Inno.Editor.Exporting.EditorExportService : Inno.Editor.Core.EditorModule`](../../src/composition/editor/features/Inno.Editor.Exporting/Runtime/EditorExportService.cs#L20) | Owns asynchronous export, progress, cancellation and retirement independently from presentation. | — | — |
| <a id="api-cee2fa9333a51b9d"></a>[`public bool Inno.Editor.Exporting.EditorExportService.isPluginBusy { get; }`](../../src/composition/editor/features/Inno.Editor.Exporting/Runtime/EditorExportService.cs#L42) | Gets whether a Plugin source export still owns build resources. | — | — |
| <a id="api-c353af4188d49c28"></a>[`public bool Inno.Editor.Exporting.EditorExportService.isGameBusy { get; }`](../../src/composition/editor/features/Inno.Editor.Exporting/Runtime/EditorExportService.cs#L46) | Gets whether a game export or its Support Pack preparation is active. | — | — |
| <a id="api-cead7e586766aef1"></a>[`public string Inno.Editor.Exporting.EditorExportService.status { get; private set; }`](../../src/composition/editor/features/Inno.Editor.Exporting/Runtime/EditorExportService.cs#L50) | Gets the latest owner-thread progress message. | — | — |
| <a id="api-c4250ce3b5500d56"></a>[`public string Inno.Editor.Exporting.EditorExportService.error { get; private set; }`](../../src/composition/editor/features/Inno.Editor.Exporting/Runtime/EditorExportService.cs#L54) | Gets the last failure or cancellation diagnostic, or an empty string. | — | — |
| <a id="api-8b41851bf4c3226d"></a>[`public float Inno.Editor.Exporting.EditorExportService.gameProgress { get; private set; }`](../../src/composition/editor/features/Inno.Editor.Exporting/Runtime/EditorExportService.cs#L58) | Gets monotonic progress for the current game build, from zero through one. | — | — |
| <a id="api-ac046242e2e0433f"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Build.BuildTargetId> Inno.Editor.Exporting.EditorExportService.availableGameTargets { get; }`](../../src/composition/editor/features/Inno.Editor.Exporting/Runtime/EditorExportService.cs#L62) | Gets the targets registered by the active Build distribution. | — | — |
| <a id="api-0909ee23b856b7f0"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Build.Managed.ManagedDeploymentId> Inno.Editor.Exporting.EditorExportService.GetManagedDeployments(Inno.Build.BuildTargetId target)`](../../src/composition/editor/features/Inno.Editor.Exporting/Runtime/EditorExportService.cs#L73) | Gets deployment implementations registered for a target.<br>`target`: The explicitly chosen build target. | The registered deployment choices; no host fallback is introduced. | — |
| <a id="api-ce410a4cdbcb88d7"></a>[`public string Inno.Editor.Exporting.EditorExportService.GetGameTargetDisplayName(Inno.Build.BuildTargetId target)`](../../src/composition/editor/features/Inno.Editor.Exporting/Runtime/EditorExportService.cs#L83) | Gets a registered target label or a diagnostic for an unavailable target.<br>`target`: The target to describe. | A presentation label; never used as a routing identity. | — |
| <a id="api-ebbda489ac34deb3"></a>[`public void Inno.Editor.Exporting.EditorExportService.StartPlugin(Inno.Build.PluginBuildRequest request)`](../../src/composition/editor/features/Inno.Editor.Exporting/Runtime/EditorExportService.cs#L96) | Starts a source-package export using the existing Build pipeline.<br>`request`: The complete, explicitly selected package request. | — | `System.InvalidOperationException`: Another export still owns the service. |
| <a id="api-eaaffb6ea380ef07"></a>[`public void Inno.Editor.Exporting.EditorExportService.StartGame(Inno.Build.GameBuildRequest request)`](../../src/composition/editor/features/Inno.Editor.Exporting/Runtime/EditorExportService.cs#L115) | Prepares the selected Support Pack before capturing and building the game.<br>`request`: The complete game request; no form or window state is retained. | — | `System.InvalidOperationException`: Another export still owns the service. |
| <a id="api-261e1ae2f470bdb3"></a>[`public void Inno.Editor.Exporting.EditorExportService.Cancel()`](../../src/composition/editor/features/Inno.Editor.Exporting/Runtime/EditorExportService.cs#L137) | Requests cancellation without releasing resources before their owned work drains. | — | — |
| <a id="api-3f1751ea30dfa4d2"></a>[`protected override void Inno.Editor.Exporting.EditorExportService.OnUpdate(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/features/Inno.Editor.Exporting/Runtime/EditorExportService.cs#L157) | Advances compilation tickets, build progress, and completed export tasks.<br>`context`: The active Editor context for the current frame. | — | — |
| <a id="api-620c1ce2f9ed04fd"></a>[`protected override void Inno.Editor.Exporting.EditorExportService.OnStop(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/features/Inno.Editor.Exporting/Runtime/EditorExportService.cs#L177) | Cancels and drains active export work before the Editor generation is stopped.<br>`context`: The active Editor context being stopped. | — | — |
| <a id="api-d84eed66653aa6af"></a>[`protected override void Inno.Editor.Exporting.EditorExportService.OnDispose()`](../../src/composition/editor/features/Inno.Editor.Exporting/Runtime/EditorExportService.cs#L185) | Drains export cleanup before releasing cancellation resources owned by this module. | — | — |

## 项目依赖

| 工程 | 边界 |
| --- | --- |
| `Inno.Core.Execution` | 实现依赖；不传递 compile surface |
| `Inno.Core.Logging` | 实现依赖；不传递 compile surface |
| `Inno.Build.Managed` | 公开签名依赖 |
| `Inno.Build` | 公开签名依赖 |
| `Inno.Core.Serialization` | 公开签名依赖 |
| `Inno.Extensibility.Catalogs` | 公开签名依赖 |
| `Inno.Editor.Core` | 公开签名依赖 |
