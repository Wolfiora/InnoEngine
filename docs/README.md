# InnoEngine Wiki

本文档按当前程序集与 bounded context 组织。公开契约以源码和英文 XML 为准；Wiki 解释 owner、依赖方向、组合方式和生命周期。历史项目、旧 namespace 和兼容入口不在 Wiki 中保留。

## 分类入口

| 分类 | 稳定职责 |
| --- | --- |
| [Core](core/README.md) | 无业务世界观的基础设施 |
| [Extensibility](extensibility/README.md) | collectible module generation、Stable Type ID 与 Registry snapshot |
| [Scripting](scripting/README.md) | 脚本 API、编译与原子 reload |
| [Assets](assets/README.md) | Player-safe runtime assets 与 authoring pipeline |
| [References](references/README.md) | 跨领域持久引用、Missing 与恢复事务 |
| [Input](input/README.md) | 每 Session 的物理输入快照、脚本 façade 与通用 Core Events backend |
| [Storage](storage/README.md) | 沙箱化应用持久数据契约、Runtime Subsystem 与文件系统 adapter |
| [Animation](animation/README.md) | 后端中立 Clip、采样、混合、事件、资产与 Runtime Subsystem |
| [Audio](audio/README.md) | 后端中立播放/Mixer 契约、Runtime、资产与 MiniAudio adapter |
| [Text](text/README.md) | 字体资产、Unicode shaping、字形光栅化与 FreeType/HarfBuzz adapter |
| [UI](ui/README.md) | RML 文档、交互、后端中立帧与 RmlUi adapter |
| [Plugins](plugins/README.md) | Plugin manifest、安装源、只读 mount 与候选激活 |
| [Scene](scene/README.md) | SceneWorld、GameBehavior、GameSystem、Scene/Prefab asset integration |
| [Rendering](rendering/README.md) | 后端中立 Rendering、运行资产、独立创作层、Runtime 与 BGFX |
| [Platform](platform/README.md) | 中立平台契约，以及各平台系统/产品/SDK 包 |
| [Backends](backends/README.md) | 共享第三方实现、Native 和组件构建的唯一 owner |
| [Runtime](runtime/README.md) | Subsystem Contracts、声明生成器、默认装配、EngineHost、RuntimeSession 与 Player |
| [Editor](editor/README.md) | Editor feature、Panel、Play Mode、Diagnostics 与 Export UI |
| [Build](build/README.md) | Build Pipeline、平台 target、Support Pack 与 toolchain |
| [Native](native/README.md) | 原生绑定与动态库加载 |
| [Architecture](architecture/README.md) | Foundation、Content/Services/Runtime、Adapter、Composition 的边界，以及 Identity、Missing、Undo/Redo 与 GC-safe reload 标准 |
| [Architecture Tooling](tooling/README.md) | 可执行架构规则 |
| [Issues](issues/README.md) | 唯一问题台账、审查记录与整改规格 |

Build 分类已覆盖新增的 [Inno.Build.SupportPacks.Core](build/Inno.Build.SupportPacks.Core.md) 项目页；源码工作区 Export 的缺包自动发布由该项目提供，发布契约与错误边界见对应页面。
浏览器导出新增的 [Browser target](platform/Browser/Inno.Build.Browser.md)、[Browser Player](platform/Browser/Inno.Player.Browser.md)、[Browser storage](platform/Browser/Inno.Adapter.Storage.Browser.md) 以及 [统一 Native 绑定与目标 profile](native/README.md) 文档已纳入各分类索引；正式 FlappyBird 浏览器实测见 [Web Player 验收](architecture/WEB_PLAYER_ACCEPTANCE_2026_10_01.md)。

## 当前架构与覆盖

本轮执行范围见[平台归属计划](architecture/PLATFORM_OWNERSHIP_REFACTOR_PLAN.md)，源码位置见[项目总览](architecture/ENGINE_ARCHITECTURE_OVERVIEW.md)，新平台接入见[扩展指南](architecture/PLATFORM_EXTENSION_GUIDE.md)。当前全部生产项目均有独立 API 页；实际运行 gate 在本輪验收报告中单独记录。

平台产品入口 → 共享 Host / 所选 backend / 平台系统；共同领域 → Foundation。Standard Distribution → 平台 Build Module / shared managed compiler；Build Composition 与共同构建机制不反向引用具体发行。

Identity、Missing、History、collectible generation 仍遵循[热重载标准](architecture/IDENTITY_REFERENCE_RELOAD_STANDARD.md)。过往验收保存为历史证据，不代替当前迁移后的验收。

## 当前格式与状态

新增 [Text](text/README.md) 与 [UI](ui/README.md) 内建 Service 分类，分别覆盖契约、资产导入、Session Runtime、adapter、原生桥与测试项目页；Inno.Canvas 是独立 Project Plugin，提供 Scene 组件、完整默认 Shader/Material/Pipeline 与 Editor 模板。本轮 Windows 验收与 macOS 源码/组合检查分别记录；历史 macOS 结果不等于本轮实测。Linux 仅保留 Native 工具链能力。

新增 [Inno.Editor.Annotations](editor/Inno.Editor.Annotations.md) 已包含独立项目页与 Editor 索引；展示标注不再归属 Core.Serialization。
新增 [Inno.Rendering.Shaders](rendering/Inno.Rendering.Shaders.md) 已包含独立项目页与 Rendering 索引；
当前实现源码接口、多实现快照、节点降低与 typed stage/资源/分支/循环，完整 Shader 图替换的未完成项单独记录，不以单元测试或原生编译通过代替产品验收。
新增 [Shader Editor](editor/Inno.Editor.Panel.ShaderEditor.md) 与
独立的 [Editor Shader 功能层](editor/Inno.Editor.Shaders.md)，以及
可复用的 [原生资产草稿功能层](editor/Inno.Editor.Assets.md)，以及
[内置 Shader 离线工具](backends/Bgfx/Inno.Build.Toolchains.Bgfx.Shaders.md) 已有项目页和分类索引。
图资产替换、自动保存、原生启动记录以及尚未通过的验收见[当前 Shader 检查点](issues/2026-09-11-unified-shader-implementation.md)。

- Project Settings、Editor Settings、Build Profile、Plugin Manifest、Catalog 与 Artifact 只支持当前源码格式。
- `Assets` 是唯一可写创作源；`Plugins` 是只读安装源；`Library` 可完全重建。
- API 变更必须同步源码 XML、所属项目页和索引。
- 当前 C01–C18 收口状态在[最新验收报告](architecture/ENGINE_CLOSURE_IMPLEMENTATION_2026_09_08.md)维护；此前编号和历史证据保留于[2026-08-31 台账](issues/2026-08-31-complete-issue-register.md)。

共享 [Player Runtime](runtime/Inno.Player.Runtime.md)、[统一 Build CLI](build/Inno.Build.Cli.md)、[MSBuild Tasks](build/Inno.Build.Tasks.md) 和 [Web 目标工具链](platform/Browser/Inno.Build.Browser.md) 均已包含独立项目页。

[宿主 Native 构建组合](build/Inno.Build.Toolchains.md) 已加入 Build 分类：所有组件接受显式 checkout/configuration，
桌面 Support Pack 主动准备 Release 输入，并统一使用可取消的隐藏子进程生命周期。

本轮新增 [DotNet 模块来源](backends/DotNet/Inno.Adapter.Modules.DotNet.md)、[DotNet 序列化来源](backends/DotNet/Inno.Adapter.Serialization.DotNet.md)、[Managed 部署契约](build/Inno.Build.Managed.md) 与 [DotNet publishers](backends/DotNet/Inno.Build.Managed.DotNet.md)，均有独立项目页；执行与验收继续在 [平台重构计划](architecture/PLATFORM_RUNTIME_REFACTOR_PLAN.md) 记录。

[RML 创作前端](backends/RmlUi/Inno.Adapter.UI.RmlUi.Authoring.md) 与组件内的
[ImGui binding extension](backends/ImGui/Inno.Native.ImGui.BindingExtension.md) 已补齐独立项目页及分类索引。

本轮新增 [Rendering Assets Authoring](rendering/Inno.Rendering.Assets.Authoring.md) 与 [Build Composition](build/Inno.Build.Composition.md) 已有独立项目页和完整当前源码 API 清单。

## 平台与后端集成

[批准计划](architecture/BACKEND_PLATFORM_INTEGRATION_PLAN.md) · [本轮验收](architecture/BACKEND_PLATFORM_INTEGRATION_ACCEPTANCE.md)。平台基础、共享 backend 与真实 integration 由独立程序集维护；七个 integration 已纳入平台分类索引。
