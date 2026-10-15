# 当前问题与验证范围

[架构索引](README.md) · [当前验收](CURRENT_ACCEPTANCE.md) · [Wiki 首页](../README.md)

2026-10-14：本轮构建成本、owner、SDK 一致性和 NativeAOT 消费已有当前源码的契约与运行证据。以下限制保留，不能用本机通过推断所有环境没有问题。

本轮[五项局部整改](../../records/architecture/acceptance/LOCAL_BOUNDARY_REFINEMENT_ACCEPTANCE.md)已实现，本机受影响门禁与首次失败的复验通过。历史审查保留在记录区，不作为当前未修复 API 指南。

| 整改 | 当前状态 |
| --- | --- |
| 文件系统大小写边界 | Core.IO 使用严格 ownership 拼写，不从 OS 猜测目录等价；本机契约通过。macOS 不同卷格式实测仍待独立环境。 |
| RenderGraph 验证 | 已接受前缀与 graph scratch 复用，完整候选继续验证；32 contributor fixture 58,920 bytes/frame、0.311 ms/frame。 |
| 工具宿主资产 | 明确 SDK host RID，实际 CLI deployment 167,948,917 bytes；不手动删除包资产。 |
| 维护命令 | cache clean 选择中立 Toolchains 维护闭包及明确 publisher catalog，不准备完整 CLI/后端图。 |
| BGFX 长期帧号 | 普通资源/readback/cache 使用单调提交序号；实际 Noop 与 surface 退休通过。未执行数十亿帧运行。 |

| 项目 | 状态与边界 |
| --- | --- |
| 200 GB 空间目标 | 未达成。本轮另回收约 17.96 GB 实际卷空间，本轮清理后空闲约 83.03 GB；单靠当前 InnoEngine 可清理范围不足以达到 200 GB。37 个历史目录仍按审批拒绝保留，未绕过或认领。 |
| Web 第三方警告 | 隔离 headless 有 WebGL 能力查询与 ScriptProcessorNode 弃用警告；JS errors=0、非 GUI 功能通过，不等同于零告警。 |
| 热 Build 成本 | 中位数 138.343 秒，Native / binding / 完整 Task 编译为 0；SDK 求值与完整哈希仍有成本。 |
| 自动回收预算 | 五秒预算可能安全延期；显式清理用独立预算。延期不误报已成功产品，也不扩大删除边界。 |
| GUI / 视觉 / 声学 | 用户要求本轮不执行；后台/headless 不替代真实焦点、DPI、拖动观感或听音。 |
| MacOSArm64 | 当前源码与组合检查不替代 Native / Editor / Player 实机，仍未实测。 |
| 远端 CI | 本机结果不代表远端 workflow 已通过。 |

不再作为待实现项：Native 热批次、Task publication 提前复用、默认 JIT worker、首次写入前 owner、SDK 参数一致性、AOT owner JSON、最新 Windows/Web 后台运行、公开消费与严格 Wiki。

历史 Reload/Identity、Console、明确目标与 BGCS NuGet 化记录保留；当前行为以项目页为准。完整证据见[本轮验收](../../records/architecture/acceptance/BUILD_COST_TOOL_HOST_DISK_CLOSURE_ACCEPTANCE.md)。

### 最新 CI 范围

远端验收要求 Windows/macOS 的源码、架构、完整契约、所选 Pack 和实际渲染 smoke 全部通过；结果必须对应 [PR 当前 head](https://github.com/Wolfiora/InnoEngine/pull/11/checks)。每轮失败、修复与实测记录见[局部验收报告](../../records/architecture/acceptance/LOCAL_BOUNDARY_REFINEMENT_ACCEPTANCE.md)，旧 revision 的成功不代替新 revision。目标限定 Wiki 验证只证明共同项目与所选产品编译；外国产品的未执行记录不计为通过，完整目标覆盖需要各自环境。

BGCS 正在独立重构；按当前验收范围，新增第三方库与 Binding 专项矩阵留待新包切换后执行。现有引擎功能、稳定性、架构与非 GUI 性能检查继续执行；不因该范围调整跳过当前仓库的 CI 门禁。
