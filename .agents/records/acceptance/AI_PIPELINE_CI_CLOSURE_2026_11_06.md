# AI pipeline CI/CD 收口

## 范围与输入

- 用户于 2026-11-06 恢复 #75 原计划全部验收，允许必要提交、推送和 Windows/macOS 托管矩阵；不自动合并或正式发布。
- 起点：`6b09276dfc42b8d18eeab2f8a037aa500f327e45`，干净的 `ai-pipeline`。相邻仓库只读。
- 当前权威计划：[AI pipeline implementation](../plans/AI_PIPELINE_IMPLEMENTATION.md)。此前阶段证据保留原身份，不升级为当前通过。

## 首次失败与修复

- [Engine CI 42071338407](https://github.com/Wolfiora/InnoEngine/actions/runs/42071338407)：Windows/macOS 均在 `agent-documents` 失败，后续检查被明确阻塞，汇总失败。
- 原因：历史记录中八处本地链接依赖未检出的相邻仓库或未受版本控制的日志。本机存在的文件掩盖了独立检出缺失。
- 将这些引用改为明确的外部仓库历史路径或本地产物路径，保留文字、源码行号和历史结论；没有放宽链接验证器。
- 用 `git archive HEAD` 创建仅含跟踪文件的独立快照，覆盖本次改动后执行 Build `verify <snapshot> --agent-documents-only`：exit 0。快照位于本仓库产物目录，其父目录没有相邻仓库；不复制本机未跟踪日志。
- 首次 Windows/macOS 日志保存在 `artifacts/validation/overnight-2026-11-06/first-failure-*.log`。

## 远程约束

- [Engine CI 42090517681](https://github.com/Wolfiora/InnoEngine/actions/runs/42090517681)：两个平台的托管编译和 selected compiled API 通过，`wiki:managed` 失败。Native API 分片错误地按文件名划入 managed 上下文；Windows 的混合分隔符还使分片丢失项目归属。
- 修复使用现有文档 owner 映射选择上下文，项目入口及分片路径统一为完整规范路径；没有删除示例或放宽检查。36 项现有 Wiki 回归通过；新增两个真实 SDK 分片编译回归和一个 native 分片计划回归通过。新增测试最初遗漏 SDK 编译准备而失败，原日志保留，随后补齐真实 SDK 准备。
- Console 探索样本在 1035.43 秒时主动取消：先修复已知 Wiki 验证缺陷，避免对失效验收链继续重复计时。原源码已恢复；这不是通过或有效 warm 性能样本。选中 Hosting/PlayMode/Presentation，未选中 Build integration。Native 记录显示零编译和绑定生成，但消费者仍有显著输入重读成本，继续追踪。
- 自动审批拒绝删除本轮 `artifacts/validation/overnight-2026-11-06/tracked-source` 及其 ZIP，仅返回 `blocked by policy`；未绕过，保留为本轮临时产物。最后观测 C 盘约 74 GiB 可用。

- 实查 ruleset `13788396` 为 active，默认分支必须满足 `Engine validation`、strict base freshness 和原有讨论解决要求；没有启用合并队列。
- 默认分支仍是 `master`，尚未采用本分支的新工作流。定时触发及正向草稿发布需要默认分支采用和成功完整认证；不得用 PR 产物冒充默认分支产物。
- 本轮未完成全部验收，不关闭 #75。
