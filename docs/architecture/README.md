# 架构治理

[Wiki 首页](../README.md) · [完整项目架构 Overview](ENGINE_ARCHITECTURE_OVERVIEW.md) · [Identity、可恢复引用与热重载标准](IDENTITY_REFERENCE_RELOAD_STANDARD.md) · [当前已知问题](CURRENT_ISSUES.md)

本分类记录跨越多个程序集、无法准确归属到单个项目页的架构事实。它不替代各项目的 API
Wiki，也不把未来规划描述为当前能力。

## 文档

| 页面 | 内容 | 维护要求 |
| --- | --- | --- |
| [2026-10-07 Solution 与架构边界复核](SOLUTION_CLEANUP_ACCEPTANCE_2026_10_07.md) | 全项目归属、有效引用、遗漏生成扩展与 Task 缓存退休修复 | 本轮实际构建与门禁证据 |
| [2026-10-06 架构整改计划](ARCHITECTURE_CLEANUP_PLAN_2026_10_06.md) | A01–A08 文件级实施规格 | 所有消费者与本机验收 |
| [2026-10-06 架构整改验收](ARCHITECTURE_CLEANUP_ACCEPTANCE_2026_10_06.md) | 本轮实际结构、正确性、性能和限制 | 区分历史证据与当前执行 |
| [通用 C# 开发规范](CSHARP_DEVELOPMENT_STANDARD.md) | 命名、排版、公开 API、所有权和验证规范 | InnoEngine 与 BGCS 独立采用 |
| [平台与运行时完整重构计划](PLATFORM_RUNTIME_REFACTOR_PLAN.md) | 批准的完整目录、边界、扩展点及执行清单 | 实现与验收状态分别记录 |
| [完整 Plan 逐项核对](PLATFORM_RUNTIME_PLAN_AUDIT.md) | 要求、实际边界、文件归属及终版证据对应 | 保留原始未来节点和实机验证边界 |
| [平台、运行时与构建分层](PLATFORM_RUNTIME_ARCHITECTURE.md) | 当前源码的五个维度、模块来源、Player、Adapter、原生产物所有权和平台接入 | 执行中的完成状态由本次验收记录说明 |
| [平台中立范围与新平台接入](PLATFORM_EXTENSION_GUIDE.md) | FlappyBird 共同输入/存储、真实平台差异与未来 iOS 接入步骤 | 区分当前能力、目标描述与实机验收 |
| [Solution 与项目整理验收](SOLUTION_CLEANUP_ACCEPTANCE_2026_10_05.md) | 分组清理、项目排版与有效配置核对 | 本轮整理及验证 |
| [Editor 普通构建与原生部署修复](EDITOR_NATIVE_BUILD_ACCEPTANCE_2026_10_06.md) | Debug 旧 DLL 符号不匹配、统一构建入口与完整部署校验 | 本轮 FlappyBird 启动及部署回归 |
| [本次平台与运行时验收](PLATFORM_RUNTIME_ACCEPTANCE.md) | 本次实现、测试证据与剩余 gate | 完整 Plan 的当前执行状态入口 |
| [Sample 导入修复与 BGCS 独立性复核](PRECOMMIT_SAMPLE_AUDIT_2026_10_03.md) | 后台候选导入、脚本 preflight、watcher 对账、History 与取消退休；BGCS 自有目标证据 | 2026-10-03 修复与两仓库分别验收，保留未定位的 Windows staging 访问拒绝 |
| [提交前第四轮复核与 BGCS 说明](PRECOMMIT_BGCS_AUDIT_2026_10_02.md) | 生成器 ABI、编译器进程、全部 BGCS 测试与 SDK lowering 边界 | 本轮修复、完整矩阵及仍需整改的 Sample 导入响应性 |
| [提交前全范围复核](PRECOMMIT_FULL_AUDIT_2026_10_02.md) | 目录数据保全、Core.IO 复用、输入边界、两阶段导出与退休、编译并行度及目标封装 | 八项补充修复、完整回归、实际导出与保留门禁 |
| [提交前再次核查与补充修复](PRECOMMIT_RECHECK_ACCEPTANCE_2026_10_02.md) | 文件日志交付、实际输入消费顺序、Pack 最后取消与工具输出失败 | 当前源码、完整回归、FlappyBird 两平台导出与未验收边界 |
| [提交前 1–8 项修复验收](PRECOMMIT_REPAIR_ACCEPTANCE_2026_10_02.md) | 日志、事件消费、Native 构建 root/配置/取消、Clang 资源与 minimap 性能 | 两仓库测试、FlappyBird 导出及未实测平台分别记录 |
| [共享宿主与统一构建实施计划](WEB_HOST_REFACTOR_PLAN.md) | Native 目标、共享 Player、Shell 和 Build 入口的重构边界 | 与当前源码、契约测试及最终验收保持一致 |
| [共享宿主与统一构建 2026-10-02 验收](WEB_HOST_REFACTOR_ACCEPTANCE_2026_10_02.md) | 删除 Native 副本、四个程序入口、通用宿主及 FlappyBird 回归 | 构建、契约测试、Windows 与 Web 运行及未实测平台分别记录 |
| [浏览器 Player 与 Web 导出边界](WEB_PLAYER_ARCHITECTURE.md) | 当前实现、浏览器分层与 FlappyBird 实测边界 | Windows/macOS 主机和浏览器运行结果分别记录，不把单平台验证推断为全平台通过 |
| [Web Player 2026-10-01 验收](WEB_PLAYER_ACCEPTANCE_2026_10_01.md) | Windows 正式 FlappyBird 导出、浏览器玩法与持久化结果 | 未实测的 macOS 与音频可听性明确列为验证边界 |
| [2026-09-26 Rendering / Canvas 验收记录](RENDER_CANVAS_ACCEPTANCE_2026_09_26.md) | 本轮实现边界、TestProject 实测、包核对和未收口门禁 | 未通过项目不得标记为完整验收 |
| [2026-09-08 实现交付与集中验收](ENGINE_CLOSURE_IMPLEMENTATION_2026_09_08.md) | 全清单剩余实现、公开 API、资源 owner 与集中验收 | 历史交付记录，实现与验证分开 |
| [2026-09-07—09-08 累积收口报告](ENGINE_CLOSURE_CONTINUATION_2026_09_07.md) | 之前 Source/Scene Recovery、Asset/Audio/Rendering 退休与 996 项基线 | 保留历史证据，不代替最新验收 |
| [2026-09-07 前轮收口报告](ENGINE_CLOSURE_ACCEPTANCE_2026_09_07.md) | 前轮实现与 742 项测试、发布证据 | 保留历史证据；当前状态以续轮报告为准 |
| [引擎统一收口实施与验收](ENGINE_CONSOLIDATION_IMPLEMENTATION.md) | 2026-09-07 批准的子系统、Execution、Diagnostics、Recovery、默认装配完整实施清单 | 逐项分别记录实现状态与实际验证结果 |
| [当前项目与依赖总览](ENGINE_ARCHITECTURE_OVERVIEW.md) | 当前生产项目、源码层次、平台选择、运行与产物所有权 | 新系统立项、跨域引用或 Host composition 改变时必须先核对此页 |
| [2026-09-07 至 09-08 架构审计](ENGINE_ARCHITECTURE_AUDIT_2026_09_07.md) | 早期架构审计与收口讨论 | 历史记录；当前实现从项目总览与本次验收查阅 |
| [Identity、可恢复引用与热重载强制标准](IDENTITY_REFERENCE_RELOAD_STANDARD.md) | 跨域 Identity、ImGui runtime ID payload、Missing 保留/恢复、History 和 GC unload barrier | 修改 Scripting、Plugins、Assets、Scene、Editor 引用或任何 collectible generation 时必须核对此页 |
| [当前已知问题](CURRENT_ISSUES.md) | 已确认缺陷、架构债务、验证缺口和整改顺序 | 问题修复时必须同步更新状态、证据与验证结果 |

## 优先级定义

- **P0**：已确认的运行正确性问题，或直接违反项目强制架构边界的问题。
- **P1**：阻碍可靠发布、可复现构建、跨平台交付或长期扩展的结构性问题。
- **P2**：当前可以工作，但会持续增加维护、诊断、性能或 API 使用成本的问题。

优先级不是兼容格式或持久化 schema。问题编号只用于代码审查、测试和文档之间的稳定引用。
