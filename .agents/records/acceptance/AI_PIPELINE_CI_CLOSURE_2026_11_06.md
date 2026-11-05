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
- 联合编译复用覆盖同一上下文的全部选中根源码；已验证未变化依赖可以保留，原生上下文的原生准备与部署核验不变。真实回归连续修改 fixture 和消费者，保留文件长度与时间戳，运行实际断言，并确认 Core IO 输出字节与时间戳不变；修订后的用例通过。首次比较跨越空工作区的第一次 NuGet 还原，实际 SDK 外部导入集合扩大，环境身份变化导致合理的保守清理；现在先建立完整还原环境，再比较相同环境下的 warm 编译，未移除输出断言。原始失败、环境差异和 Clean 日志保留。既有运行中源码变更、恢复源码、依赖等长修改及旧证据拒绝回归同样通过。
- 完整 extended 计划由实际图生成：246 个项目、63 个测试 owner（17 domain、33 contract、4 integration、8 native、1 performance），17 个领域 owner 均为 managed；冷输入、产品、Release、Support Pack、NativeAOT 还原与交付验收仍被选择。覆盖摘要为 `owner-coverage.json`，它是计划覆盖证据，不是 325 项必需检查已执行通过的声明。
- 实际失败 PR 运行 `42094568938` 被发布脚本在任何写入前拒绝：`The source run did not complete successfully.`。证据为 `live-release-rejection/`；正向默认分支演练仍未完成。
- Console 探索样本在 1035.43 秒时主动取消：先修复已知 Wiki 验证缺陷，避免对失效验收链继续重复计时。原源码已恢复；这不是通过或有效 warm 性能样本。选中 Hosting/PlayMode/Presentation，未选中 Build integration。Native 记录显示零编译和绑定生成，但消费者仍有显著输入重读成本，继续追踪。
- 自动审批拒绝删除本轮 `artifacts/validation/overnight-2026-11-06/tracked-source` 及其 ZIP，仅返回 `blocked by policy`；未绕过，保留为本轮临时产物。最后观测 C 盘约 74 GiB 可用。

- 实查 ruleset `13788396` 为 active，默认分支必须满足 `Engine validation`、strict base freshness 和原有讨论解决要求；没有启用合并队列。
- 默认分支仍是 `master`，尚未采用本分支的新工作流。定时触发及正向草稿发布需要默认分支采用和成功完整认证；不得用 PR 产物冒充默认分支产物。
- 本轮未完成全部验收，不关闭 #75。

## Console 反馈与测试边界整改

- `console-v2-0` 完整依赖选择样本耗时 2510.63 秒，退出码 1；属于工具改动后的首次运行，不能称为 warm 通过样本。559 项测试全部通过，产品输入重复 HTTPS 拉取因连接重置失败。首次失败、源码身份及阶段耗时保留在 `artifacts/validation/overnight-2026-11-06/console-v2-samples.json` 和对应运行目录。
- Hosting 12 项、PlayMode 448 项、Presentation 99 项；未选择 Build integration。PlayMode 中 15 项大规模测试合计 886.61 秒，其余 433 项合计 131.92 秒。将原三份 `LargePanelPerformanceTests` 源文件归入独立 performance owner，共享真实 native fixture 归入 `Inno.Testing.Editor.Native`；保留所有操作与断言，显式传递测试组合 Assembly，避免发现范围随 fixture 所在程序集变化。源码架构检查 248 项目通过；两测试根编译通过，零警告／错误。迁移后 15 项 performance 和 433 项 PlayMode 契约全部通过、零跳过，分别耗时 16 分 33 秒和 2 分 21 秒；这些有并发工作负载的正确性验收不冒充最终 warm 基准。TRX 位于 `editor-performance-migration-trx/`。
- 新的 performance 选择沿实际引用图覆盖共享测试 fixture 的变化；普通产品源码变化不自动选择规模测试。扩展矩阵及构建／native／reload 变化仍选择相应深层证据。针对该选择行为的回归通过。
- 发现 Editor 的 `TreatAsLocalProperty` 会覆盖命令行 `BuildProjectReferences=false`，使局部 Clean 仍递归清理。增加仅在明确根清理请求时生效的 Clean target，并统一根构建与入口引用的有效属性。真实回归模拟相同覆盖，验证保留时间戳的源码变化必须重编译、未变依赖应保留；42 项计划／编译回归通过，其中根清理用例耗时 6 分 11 秒。
- 另外保护实际 SDK 图中的 property variants：另一个输出实例的源码仍属于依赖保护范围，不能用默认实例的根 Clean 冒充全部实例已清理。配合新 Clean 策略的实际变体用例通过（9 分 55 秒）；后续补强 fixture API 覆盖后，组合回归再次执行中。
- 产品消费者输入改为验证工作区所有的固定提交 Git 对象缓存；原提交对象可从身份一致的相邻仓库只读导入，绝不读取未提交工作文件。每次检查对象完整性、精确提交和提取边界；损坏隔离后重新导入，错误 origin 拒绝，初始化完成后才发布缓存目录。真实 Git 回归通过：首次导入、后续无 fetch 复用、损坏隔离重建、错误 origin 拒绝及原消费者 dirty 内容／对象库保持不变。首次发现 Windows Git 长路径限制，修复仅配置本次 bare owner；随后损坏注入遇到 Git 只读文件属性，调整测试的显式注入步骤后通过。首次失败日志均保留。真实产品旅程仍待验收。
- 实际五根 fixture 联合构建在四节点下复现相同 Catalogs 输出竞争：直接 solution 构建各根自行生成 bootstrap owner。统一验证原已显式传递同一 owner，现通过标准 `Directory.Solution.targets` 扩展使普通 `.sln`／`.slnx` 调用也统一 owner。四节点、连续两次 Rebuild 的真实回归通过，同次三个项目只用一个 owner，下一次 owner 更新；原失败的五根构建通过，Catalogs 仅执行一次。对应 `solution-owner-regression-trx/` 与 `fixture-api-build-owned.log`。共享 solution 输入纳入证据身份与规则路由，241 条规则完整保留，路由／预算回归通过。
- Wiki 验证原先按 `tests/` 路径排除整个程序集，漏掉公开测试支持库。改为只有明确声明测试 runner 的项目才免除测试方法 API 映射；14 个 fixture／支持项目补齐 94 个真实符号映射，全部签名及两份 C# 示例通过。39 项 Wiki 回归通过，包括新增 fixture 库不因目录而豁免的用例。原生 fixture 首次使用缺少产品属性的手动检查产生缺引用失败；改用已有 SDK 上下文并准备实际缺失的五根项目，没有复制 DLL 或放宽签名检查。源码／链接检查与 API 覆盖分别留证。
- `42104761730` 为 `0f0df3e` 的真实 Windows/macOS 运行，目前仍在执行，未取得绿色汇总。更新的本地修订尚未提交／推送，不把该运行算作这些修订的证据。
- 临时托管工作区 `C:/Users/23842/.codex/worktrees/pipeline-acceptance-fixes/InnoEngine` 的桌面归档被拒绝：`The task's primary checkout cannot be removed.`；原 checkout 重新附加被拒绝：`The checkout exists but is not a managed worktree.`。修订已回到原 `ai-pipeline`，临时工作区保持可恢复，未绕过应用保护删除。当前磁盘可用约 74 GiB。

- 组合回归揭示 Wiki 引用查询继承 `BuildProjectReferences=true` 后会重新编译已准备的依赖，关闭时输出新鲜度检查因此正确失败。首次组合与诊断日志保留；SDK 引用捕获现在独立关闭引用编译，并在执行边界覆盖 local property 的重新启用。两个真实 SDK 复现先失败，修复后全部 7 项图求值回归通过；原联合编译／变体用例正在复验，没有删除输出新鲜度检查。
- 更新后的完整选择清单包含 248 项目、64 测试 owner（17 domain、33 contract、4 integration、8 native、2 performance），328 项必要检查；17 个 domain 均为 managed。`owner-coverage-final.json` 仅证明选择完整性。文档查询 24 项回归通过，当前源码架构检查通过。
