# InnoEngine Wiki

从这里了解开发入口、引擎契约、资源所有权和扩展位置。正文描述当前源码；实现状态与已验证范围见[当前验收](architecture/CURRENT_ACCEPTANCE.md)，未解决项见[当前问题](architecture/CURRENT_ISSUES.md)。

## 第一次使用

1. 阅读[启动指南](platform/PRODUCT_STARTUP.md)，确认本机 SDK 和支持范围。
2. 在 IDE 直接启动 [InnoEngine.Editor](entry/InnoEngine.Editor.md)，正常 Build 会准备完整依赖。没有传入 Project 路径时打开目录选择窗口。
3. 需要独立准备支持或显式清理时使用 [InnoEngine.Build](entry/InnoEngine.Build.md)。
4. 阅读[架构总览](architecture/ENGINE_ARCHITECTURE_OVERVIEW.md)，再进入所需项目页。

引擎源码修改后运行 Build。游戏内容修改由导入和 reload 流程处理。清理使用显式命令，不属于默认构建。

## 按职责阅读

| 分类 | 内容 |
| --- | --- |
| [开发入口](entry/README.md) | 宿主路由、准备和启动 |
| [Core](core/README.md) | 集合、身份、事件、序列化、IO 等基础机制 |
| [Extensibility](extensibility/README.md) | Stable Type ID、模块和代际注册 |
| [Scripting](scripting/README.md) | 脚本公开表面、编译和候选激活 |
| [Assets](assets/README.md) | 内容读取、运行资产与创作流程 |
| [References](references/README.md) | 持久引用、Missing 和恢复 |
| [Scene](scene/README.md) | 对象、组件、系统、Scene 和 Prefab |
| [Animation](animation/README.md) | 采样、混合和动画事件 |
| [Input](input/README.md) | 输入快照、焦点与 Session 隔离 |
| [Storage](storage/README.md) | 应用持久数据与存储生命周期 |
| [Rendering](rendering/README.md) | 图形机制、运行资产、创作与运行服务 |
| [Audio](audio/README.md) | 播放、混音、资产和运行服务 |
| [Text](text/README.md) | 字体、文本布局和字形资源 |
| [UI](ui/README.md) | 文档、输入、交互和增量资源 |
| [Plugins](plugins/README.md) | 引擎的内容与源码扩展包机制 |
| [Runtime](runtime/README.md) | Host、Session、Subsystem、Shell 和 Player |
| [Editor](editor/README.md) | Feature、Panel、History、Workspace 和导出 |
| [Platform](platform/README.md) | 系统契约、平台产品、SDK 和集成 |
| [Backends](backends/README.md) | 共享实现、Native 边界和组件构建 |
| [Native 导航](native/README.md) | 生成定义、互操作与加载 |
| [Build](build/README.md) | 目标、内容编译、发布、Support Pack 和工具链 |
| [架构](architecture/README.md) | 跨项目边界、生命周期与扩展指南 |
| [测试与验证](testing/README.md) | 契约测试、fixture、运行前提与结果边界 |
| [Tooling](tooling/README.md) | 架构、源码和文档验证 |

## 扩展从哪里开始

- 添加平台：读[平台扩展指南](architecture/PLATFORM_EXTENSION_GUIDE.md)，提供系统与 SDK 实现，再组合可用后端。
- 添加后端：实现所属领域的 Adapter 契约；确有平台差异时增加 integration。
- 添加游戏模型或 Editor 功能：使用公开扩展协议、Stable ID 和现有注册机制。
- 修改生命周期或跨代引用：先读[Identity 与热重载标准](architecture/IDENTITY_REFERENCE_RELOAD_STANDARD.md)。

## 文档范围

每个项目只有一个权威项目页。项目页说明 API 行为、借用与转移、线程、失败和退出步骤；架构页解释跨项目规则。内部实现必须显式标注，不作为稳定扩展入口。

全量项目、API 和示例审查正在进行；有同名页面不代表其内容已经完整验收。历史执行记录保存在仓库的 records 目录，不作为当前 API 使用说明。当前格式随源码共同演进，不提供旧格式读取或旧接口转发。

独立的 [Inno.Scripting.Compiler.Tests](testing/Inno.Scripting.Compiler.Tests.md) 覆盖当前语言契约、跨插件 IDE 引用和框架内容缓存失效，纳入托管验证图。[Inno.Build.Toolchains.Tests](testing/Inno.Build.Toolchains.Tests.md) 独立验证中立原生编译清单、冻结根映射与完整性契约。

[Inno.Testing.Source](testing/Inno.Testing.Source.md) 为需要 checkout 的测试读取显式 SDK 输入，源码只属于该测试支持程序集。

平台构建接入新增独立标准产品库、Windows 宿主及三个平台 Build integration；各项目的当前公开契约和签名映射见对应分类索引。

Editor UI 的程序集边界、当前文件树、手势、选择与基础能力复用见 [Editor UI 架构](architecture/EDITOR_UI_ARCHITECTURE.md)。[Editor 索引](editor/README.md) 包含 Assets.ImGui、Shaders.ImGui、Graph.ImGui 和 Workbench 的独立项目页。
