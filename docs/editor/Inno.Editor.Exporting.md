# Inno.Editor.Exporting

[Editor 索引](README.md) · [Build API](../build/Inno.Build.md) · [Support Pack](../build/Inno.Build.SupportPacks.Core.md) · [Wiki 首页](../README.md)

## 职责与边界

本项目拥有 File 菜单的 `Export as Plugin...`、`Export as Game...` Action，以及 Game 设置、Game 进度和 Plugin 三个 modal。它只保存本次导出的 draft、进度与取消状态，实际快照、脚本编译、内容打包、平台布局及原子提交属于 `Inno.Build`。Project `Assets` 是唯一创作源，导出结果不回写项目设置。

该项目没有新的公共类型或稳定 `protected` 扩展点。`ExportWindowModule`、Action 和 Modal 均为 Editor 内部实现，不能替代 [BuildPipeline](../build/Inno.Build.md) 的公开契约。

## Game 工作流

1. 打开 modal 时从 `Settings.Build.inno` 复制 Game 默认值；Application ID 始终由当前 Project ID 给出。
2. 用户点击 Export 后，设置窗口立即关闭，进度以不可拖动且阻断其他交互的 modal 显示，并提供 Cancel。先调用 `BuildPipeline.EnsurePlayerSupportPackAsync(target)`。源码工作区中若目标 Pack 不存在，会在后台发布并校验；已有有效 Pack 直接通过，损坏 Pack 明确失败。构建成功、失败或取消后进度 modal 自动关闭，结果写入 Console。
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
