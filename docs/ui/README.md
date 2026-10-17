# UI API

[Wiki 首页](../README.md) · [Text](../text/README.md) · [Rendering](../rendering/README.md)

UI 是内建的 retained-mode 文档与交互基础服务；它接收显式语言的文档、输入、字体和命名纹理，输出后端中立的帧几何及事件，不持有图形设备。调用方通过 Rendering 的资源与命令契约上传增量几何，并在所属渲染模型中决定混合、排序和输出目标。

| 项目 | 职责 |
| --- | --- |
| [Inno.UI](Inno.UI.md) | 文档、Context、帧与事件的脚本契约 |
| [Inno.UI.Assets](Inno.UI.Assets.md) | 中立文档资产、前端和共同导入流程 |
| [Inno.UI.Runtime](Inno.UI.Runtime.md) | 每 Session 的 Context、输入与 artifact 生命周期 |
| [Inno.Adapter.UI](Inno.Adapter.UI.md) | 中立 backend factory/selection |
| [Inno.Adapter.UI.RmlUi](../backends/RmlUi/Inno.Adapter.UI.RmlUi.md) | RmlUi 后端与原生转换 |
| [Inno.Adapter.UI.RmlUi.Authoring](../backends/RmlUi/Inno.Adapter.UI.RmlUi.Authoring.md) | RML 前端及 `.rml` 导入约定 |
| [Inno.UI.Tests](Inno.UI.Tests.md) | 导入、DOM、输入、帧与生命周期回归 |

脚本使用逻辑 namespace `InnoEngine.UI`。RmlUi Native 类型不得跨出 adapter。
