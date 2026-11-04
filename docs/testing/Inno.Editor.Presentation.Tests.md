# Inno.Editor.Presentation.Tests

[测试索引](README.md) · [Wiki 首页](../README.md) · [统一验证](../tooling/Inno.Tooling.Validation.md)

## 职责与边界

覆盖 Graph Canvas 几何与交互投影、Editor 面板与工作台生命周期、资源浏览撤销/重做以及真实呈现程序集的组合契约。原 Graph 与 Interactions 项目中的断言迁入本项目，领域项目不再为这些呈现依赖准备 native。

本项目引用 [Graph ImGui](../editor/Inno.Editor.Graph.ImGui.md)、[Editor ImGui](../editor/Inno.Editor.ImGui.md)、[Workbench](../editor/Inno.Editor.Workbench.md) 和真实资源浏览实现。编译需要 development product 声明的 native 输入；验证计划推导其依赖，不使用测试过滤模拟隔离。程序集内包含性能采样，成功结果不跨次复用。原有禁用类间并行的策略继续保留。

## 场景与所有权

- GraphCanvasViewTests、GraphCanvasPresentationTests、GraphCanvasPointerTests：投影、命中、框选、导航及端口几何。
- EditorRuntimeTests 及各 partial 文件：发现、命令、选择、面板退休、失败补偿、状态恢复与性能。
- AssetHistoryTests：资源与目录历史、身份元数据、FileBrowser 拖放及失败回滚。

完整场景见 [测试源码](../../tests/editor/Inno.Editor.Presentation.Tests) 与 [项目依赖](../../tests/editor/Inno.Editor.Presentation.Tests/Inno.Editor.Presentation.Tests.csproj)。测试 fixture 只拥有自己创建的目录、模块和呈现会话；生产可见性与脚本公开契约不变。

## 运行

从仓库根目录经 Build 入口执行 `validate plan`，检查 native 上下文及所需产物，再执行 `validate run`。执行日志、TRX 和有效证据由统一执行器保留；本页不表示任何尚未执行的产品环境已通过。
`EditorSettingsTests` 保留真实 Console 模块的动态发现、设置持久化、共享历史和失败事件断言；其日志面板依赖属于本呈现执行闭包。
