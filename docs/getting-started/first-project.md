# 项目与内容的第一条工作流

[入门索引](README.md) · [Editor 模块](../modules/editor/README.md)

启动时选取自己的游戏 Project 目录，也可显式传入绝对路径：

```sh
dotnet run --project entry/InnoEngine.Editor/InnoEngine.Editor.csproj -- /absolute/MyGame
```

项目目录与引擎 checkout 分开。目录选择只决定打开位置，不能据此假定场景、资产或脚本模板已自动创建。先按所属模块的公开流程建立内容，再选择启动场景；发布所需的支持产物和目标另由 Build 流程准备。

| 下一项任务 | 阅读入口 |
| --- | --- |
| 读取、导入与构建资产 | [Assets](../modules/assets/README.md) |
| 组织 Scene、对象与组件 | [Scene](../modules/scene/README.md) |
| 编写脚本与理解导出边界 | [Scripting](../modules/scripting/README.md) |
| 缺失类型或资产后恢复 | [References](../modules/references/README.md) |
| 查看日志与当前诊断 | [Console](../modules/editor/Inno.Editor.Panel.Logging.md) |
| 准备游戏发布 | [平台产品指南](../platform/PRODUCT_STARTUP.md) |

脚本编译失败保留诊断；Missing 保留恢复所需的身份与中立状态，不等同于删除数据。先修复当前失败，再沿同一项目工作流重试；不要用手工清空身份、缓存或日志掩盖问题。
