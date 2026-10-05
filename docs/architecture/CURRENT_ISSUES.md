# 当前架构问题入口

[架构治理](README.md) · [Wiki 首页](../README.md) · [正式问题台账](../issues/2026-08-31-complete-issue-register.md)

2026-10-05 当前入口：[完整平台/运行时重构验收](PLATFORM_RUNTIME_ACCEPTANCE.md)与
[Plan 逐项核对](PLATFORM_RUNTIME_PLAN_AUDIT.md)记录此次结构、静态注册、四条实际 Player 路径及独立 BGCS 验收。
Windows 临时目录 rename 的共享/访问拒绝现由共同 IO 边界有界处理；持续拒绝仍明确失败并保留旧输出。
有真实占用、恢复、并发和回滚回归，未把重试视为外部占用进程已被确定。
下列旧轮次的测试数和 staging 风险属于历史证据；当前结果与 macOS/GUI/设备限制以本次报告为准。

2026-10-03 更新：[Sample 导入修复与 BGCS 独立性复核](PRECOMMIT_SAMPLE_AUDIT_2026_10_03.md)记录后台复制/身份重写、候选 Catalog/Importer、统一编译器后台 reference/Roslyn、History、取消和退出退休。下述 Sample 主线程同步 preflight 的 P2 已修复；同时移除索引前 watcher 静默等待，并补齐完整对账的 FileSystem 索引刷新。
真实 watcher 的成功/取消及外部文件对账回归通过。最终引擎 50 项目汇总为 1434 passed、0 failed、16 skipped；Solution、Release CLI 和架构验证通过，FlappyBird 最终 Windows/Web 导出与 Web 运行回归通过。汇总采用最终矩阵的 49 个项目与完整 Scripting 成功重跑，原失败证据保留，详见本轮报告。BGCS 文档已移除引擎专属验收描述，独立 11 项目 710 passed。
本轮 Windows FlappyBird 导出仍出现 staging 访问拒绝，重试及独立冷缓存副本成功；尚未定位根因，属于保留风险，不能称为已修复。macOS/Linux 与可见 GUI/高 DPI/音频证据缺口仍保留。

2026-10-02 第四轮复核：[BGCS 修改原因、补充修复与当前证据](PRECOMMIT_BGCS_AUDIT_2026_10_02.md)记录 FunctionTable 的
opaque handle 载体统一、编译器查询输出拥塞修复和五个配置测试预期同步。BGCS 全部 11 项目 708 passed；
引擎全部 50 项目 1421 passed / 0 failed / 16 平台限定 skipped，七个 Native CheckBindings 与架构检查通过。
Windows/Web Support Pack、FlappyBird 导出及静音 headless 昼夜/星光/玩法/持久化回归通过；运行 error 0，既有 browser warning 315。
该轮另确认 P2：Editor Import Sample 在 owner thread 同步等待脚本 preflight，耗时编译期间不能继续绘制进度或接收取消。
此问题在上述 2026-10-03 事务修复中完成整改；保留该条作为发现记录。

2026-10-02 最新全范围复核：[八项补充问题与最终证据](PRECOMMIT_FULL_AUDIT_2026_10_02.md)记录目录提交后的数据保全、
重叠树拒绝、Core.IO 路径复用、已消费 move 的输入边界、显式 Pack 准备、Editor 导出退休、Web 编译预算与目标封装。
50 个测试项目最终为 1421 passed / 0 failed / 16 平台限定 skipped；架构验证通过。
Windows/Web Support Pack 与 FlappyBird 导出、静音 headless 昼夜/星光/玩法/持久化回归均通过；运行 error 为 0，保留 315 条既有 warning。
下方历史 Rendering/Canvas 门禁与未实测平台继续保留，不能用本轮通过结果覆盖。

2026-10-02 再次核查：[补充修复与当前验收](PRECOMMIT_RECHECK_ACCEPTANCE_2026_10_02.md)记录文件日志、
Game Input 实际消费顺序、Support Pack 最后取消检查及工具输出失败清理的补充修正。
该轮 50 个测试项目汇总为 1408 passed / 0 failed / 16 平台限定 skipped；Windows/Web FlappyBird 导出和静音 headless 浏览器回归通过。
浏览器 warning、GUI 与 macOS/Linux 实机验证边界继续明确保留。

2026-10-02 更新：[提交前 1–8 项修复验收](PRECOMMIT_REPAIR_ACCEPTANCE_2026_10_02.md)记录本轮日志重入/并发、
输入消费、Native 配置与 root、进程取消、BGCS builtin headers、minimap 性能及代码整理的修复。
50 个引擎测试项目为 1367 passed / 0 failed / 16 平台限定 skipped；BGCS 284 项通过。
FlappyBird Windows/Web 导出成功。首次 Artifact staging access denied 的外部来源尚未确定，日志与重试结果明确保留；
本次结果不替代下方历史问题的独立验收。

2026-09-26 更新：[Rendering / Canvas / Plugin Samples 验收记录](RENDER_CANVAS_ACCEPTANCE_2026_09_26.md)显示：Canvas/Rendering2D 实现、RenderGraph route、输入汇总、CLI Import Sample 与 TestProject Player 构建均已有通过证据。
仍缺真实双模型图像、跨 View 交互及完整 Transform/高 DPI 视觉矩阵，以及 Editor Import Sample 菜单的完整留证；整体状态为**未完成无保留验收**。这些是独立验收证据缺口，不能写成上述功能没有实现或构建失败。

2026-09-12 更新：[MaterialGraph 双资产路径清理](../issues/2026-09-11-material-graph-dual-asset-path.md)
已按后续用户授权完成；单一 Shader 创作链及尚未完成的验收见
[统一 Shader 实施记录](../issues/2026-09-11-unified-shader-implementation.md)。历史全引擎验收不能替代本次改动的验证。

本次 C01–C18 收口的当前状态与阻断项统一维护在
[2026-09-08 实现交付与集中验收](ENGINE_CLOSURE_IMPLEMENTATION_2026_09_08.md)；
[实施清单](ENGINE_CONSOLIDATION_IMPLEMENTATION.md)保留阶段记录。
最新实现已交付，自动验收为 48 项目 / 1021 passed / 0 failed / 0 skipped；无保留验收仍保留 GUI 权限阻断，以及一次未复现、尚未定位原因的 Plugin 移除测试失败记录。ImGui UI 公开面是既定设计，不是待整改项。
此前 2026-08-31 的问题编号和历史证据仍见[全量问题台账](../issues/2026-08-31-complete-issue-register.md)，不能用历史通过结果替代当前验收。

历史审查证据保存在[完整架构审查结论](../issues/2026-08-31-full-architecture-audit.md)，最终目标和实施顺序保存在
[全仓架构整改总方案](../issues/2026-08-31-architecture-remediation-master-plan.md)。

同日反馈的自动编译/旧代验证循环已修复：排除当前候选的 SourceMountsChanged 通知回声，票据/IDE 投影等待 GC 完成，Faulted 不再显示无限忙碌；同时移除 LifetimeScope 自有任务多余的完成观察回调。最终验收与新自动模式测试见上述当前报告文末。
