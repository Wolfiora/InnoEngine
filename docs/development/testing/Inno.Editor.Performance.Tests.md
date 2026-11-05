# Inno.Editor.Performance.Tests

[测试与 fixture 索引](README.md) · [PlayMode 契约](Inno.Editor.PlayMode.Tests.md) · [原生 fixture](Inno.Testing.Editor.Native.md)

## 职责与使用

该程序集拥有万条场景对象、资产目录、实际原生输入及连续编辑的性能与规模验收。普通契约保留在 PlayMode；原 `LargePanelPerformanceTests` 的三个部分整体迁入本项目，场景规模、采样次数、输入动作和断言不变。

执行需要匹配目标、配置及绑定选择的真实 Editor 原生产物。性能 owner 由统一计划依据实际项目图选择：扩展验收、构建／Native／reload 修改以及本套件或其测试 fixture 修改会执行它。普通内部颜色修改不需要重新测量万条资产目录的性能。每周完整性能矩阵仍必须执行本套件；缺失结果不能视为通过。

独立诊断可使用 SDK `dotnet test tests/editor/Inno.Editor.Performance.Tests/Inno.Editor.Performance.Tests.csproj --configuration Debug`，并传入与统一计划一致的目标、产品属性及产物路径。它不是替代统一计划的另一个测试名单。

## 设计与实现

通过 [Inno.Testing.Editor.Native](Inno.Testing.Editor.Native.md) 复用真实 ImGui、Editor 模块、项目资产和场景生命周期。测试程序集显式作为 discovery root，动态类型不依赖进程中偶然加载的其他测试程序集。程序集禁止并行，以保护 ImGui 当前上下文和 owner thread。

| 源码 | 验收内容 |
| --- | --- |
| [LargePanelPerformanceTests.cs](../../../tests/editor/Inno.Editor.Performance.Tests/LargePanelPerformanceTests.cs) | 10,000 对象与资产行、完整 dirty 检查、序列化快照、来源新鲜度和公开元数据读取；保留原始样本。 |
| [LargePanelPerformanceTests.Input.cs](../../../tests/editor/Inno.Editor.Performance.Tests/LargePanelPerformanceTests.Input.cs) | 实际滚轮、搜索、连续编辑、Undo/Redo 及原生绘制。 |
| [LargePanelPerformanceTests.Workflows.cs](../../../tests/editor/Inno.Editor.Performance.Tests/LargePanelPerformanceTests.Workflows.cs) | 大规模裁剪视图中的范围选择、重命名与拖拽；保留全部功能断言。 |

## 使用契约

测试创建的临时项目由 fixture 释放；输入快照 `INNO_UI_PERFORMANCE_INPUT_ROOT` 由调用方拥有，不能当作任意可删除缓存。默认保留五轮采样；冷构建、初始化、操作采样及完整进程耗时分别记录。性能测试不复用旧绿色结果，也不通过减少采样或缩小规模宣称提速。
