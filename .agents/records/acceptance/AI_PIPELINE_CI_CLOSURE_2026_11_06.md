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
- [Engine CI 42094568938](https://github.com/Wolfiora/InnoEngine/actions/runs/42094568938)：两平台均在 managed Wiki 的 18 个脚本示例失败。项目入口与脚本分片被分到不同上下文，而执行器会展开完整项目文档。改为同一项目文档及编译前提共用投影上下文，并通过 Markdig 语法树识别 C# 围栏；40 项计划回归通过，包括反引号和波浪号围栏。首次失败日志保留为 `script-wiki-failure-*.log`。
- 原生消费者复用移除等待读取保护前的重复完整冻结；取得独立 lease 后，在一个新的核验阶段重算完整请求并验证所有历史读取内容及目录成员。Task 的串行候选域不作为输入身份，仍按完整请求区分成功结果，登记仅保存中立数组。16 项消费者回归通过，包括等长且保留时间戳的修改、SDK、配置、目标、recipe、产品、目录成员、等待期间失效、损坏和取消；真实产品耗时仍需后续记录。新增 recipe 用例最初未声明 fixture 的静态链接能力而失败，补齐测试目标的真实声明后通过，首次日志保留。
- 当前源码架构、文档结构／链接、Build CLI 与 Task 编译通过。8 个 CI 协议脚本全部通过：9 项排程、19 项发布来源、19 项发布包／归档、18 项汇总、7 项隔离执行、6 项 Support Pack 闭包、5 项失败／取消产物保留，以及完整规则路由回归。这些协议模拟不代替实际托管矩阵或发布演练。
- Git HTTPS 三次连接失败；通过 GitHub API 传送已验证的相同文件树，作者、提交者、父提交与时间保持。首次传送遗漏提交消息末尾换行，交付身份为 `0a924a2c22553a2c9b0d5e9c44ccfd95ab5e3ae4`，本地对象已精确核对并接续；后续传送保留原始消息字节。对应诊断为 `api-commit-transfer.json`。
- Console 探索样本在 1035.43 秒时主动取消：先修复已知 Wiki 验证缺陷，避免对失效验收链继续重复计时。原源码已恢复；这不是通过或有效 warm 性能样本。选中 Hosting/PlayMode/Presentation，未选中 Build integration。Native 记录显示零编译和绑定生成，但消费者仍有显著输入重读成本，继续追踪。
- 自动审批拒绝删除本轮 `artifacts/validation/overnight-2026-11-06/tracked-source` 及其 ZIP，仅返回 `blocked by policy`；未绕过，保留为本轮临时产物。最后观测 C 盘约 74 GiB 可用。

- 实查 ruleset `13788396` 为 active，默认分支必须满足 `Engine validation`、strict base freshness 和原有讨论解决要求；没有启用合并队列。
- 默认分支仍是 `master`，尚未采用本分支的新工作流。定时触发及正向草稿发布需要默认分支采用和成功完整认证；不得用 PR 产物冒充默认分支产物。
- 本轮未完成全部验收，不关闭 #75。
