# 当前问题与验证范围

[架构索引](README.md) · [当前验收](CURRENT_ACCEPTANCE.md) · [Wiki 首页](../README.md)

2026-10-14：本轮构建成本、owner、SDK 一致性和 NativeAOT 消费已有当前源码的契约与运行证据。以下限制保留，不能用本机通过推断所有环境没有问题。

| 项目 | 状态与边界 |
| --- | --- |
| 200 GB 空间目标 | 未达成。37 个历史目录约 55.11 GB 逻辑占用，删除被自动审批拒绝；未绕过或认领。正式回收实际卷结果见本轮记录。 |
| Web 第三方警告 | 隔离 headless 有 WebGL 能力查询与 ScriptProcessorNode 弃用警告；JS errors=0、非 GUI 功能通过，不等同于零告警。 |
| 热 Build 成本 | 中位数 138.343 秒，Native / binding / 完整 Task 编译为 0；SDK 求值与完整哈希仍有成本。 |
| 自动回收预算 | 五秒预算可能安全延期；显式清理用独立预算。延期不误报已成功产品，也不扩大删除边界。 |
| GUI / 视觉 / 声学 | 用户要求本轮不执行；后台/headless 不替代真实焦点、DPI、拖动观感或听音。 |
| MacOSArm64 | 当前源码与组合检查不替代 Native / Editor / Player 实机，仍未实测。 |
| 远端 CI | 本机结果不代表远端 workflow 已通过。 |

不再作为待实现项：Native 热批次、Task publication 提前复用、默认 JIT worker、首次写入前 owner、SDK 参数一致性、AOT owner JSON、最新 Windows/Web 后台运行、公开消费与严格 Wiki。

历史 Reload/Identity、Console、明确目标与 BGCS NuGet 化记录保留；当前行为以项目页为准。完整证据见[本轮验收](../../records/architecture/acceptance/BUILD_COST_TOOL_HOST_DISK_CLOSURE_ACCEPTANCE.md)。
