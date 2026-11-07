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

## 独立托管环境暴露的测试准备缺口

- `7217408f1cde43f45cdae9b06aad355f6bb61c17` 已提交联合 owner、只读 SDK 查询和 performance 隔离整改。最后两项实际联合编译／property variant 回归均通过，合计 15 分 11 秒；7 项图捕获、39 项 Wiki 和 24 项文档查询回归通过。对应 `readonly-compilation-final*`、`reference-query-fixed*`、`readonly-wiki-regression*`、`fixture-query-regression*`。源码、文档链接、Toolchains 的 462 项公开符号及 CLI 编译通过。
- [Engine CI 42104761730](https://github.com/Wolfiora/InnoEngine/actions/runs/42104761730) 的 Windows/macOS 和 `Engine validation` 最终均失败；它们属于 `0f0df3e`，不覆盖后续本地修订。两平台诊断 ZIP 均按 GitHub 提供的 SHA-256 校验，再由现有安全提取器展开。首次日志、TRX 和结果保留在 `ci-421047-windows*`、`ci-421047-macos*`。
- 两平台共通的主要故障是 9 个实际消费 Native／离线工具的测试 owner 未声明产品部署，加载器因此正确拒绝缺少部署索引的输出。修订按实际需要声明 player、editor 或 shader-tools；部署 target 直接遵循显式产品声明，移除依赖 `Inno.Native.*` 文件名才部署的猜测。Native 层缺少声明现在在计划阶段明确失败；42 项计划回归中 41 项初次通过，一项旧 fixture 也缺少声明，补齐后该用例通过。没有弱化原断言或加入隐式 DLL 搜索。
- Windows 另外发现 7 项现有 Metal 编译用例缺少 `SupportedOSPlatform("macos")`，严格汇总拒绝了这些无契约的 skip。补齐方法级平台声明；macOS 仍执行真实 Metal 编译，缺工具或编译失败仍然失败。
- 两平台的 4 个 UI／Build 测试 owner 在执行后改变了自己的 bin 目录；本地实际文件确认默认 `imgui.ini` 写入构建输出。非持久化测试上下文现在显式关闭默认 ini 写入，保留真正持久化场景的独立临时目录及断言；未放宽输出新鲜度检查。
- `EditorShaderProfileTests` 的 SDK 查询未使用 CI 实际 ArtifactsPath、ToolTarget 和配置。本地指定未准备的输出 owner 复现 NETSDK1004，改用已准备的实际 owner 后查询通过；远程首次日志只有 exit 1，不能将本地诊断冒充远程原始错误。增加实际 SDK 失败回归，捕获输出现在保留原诊断和内层失败，该回归通过。
- Console v3 首次准备在发现上述 UI 输出污染后主动取消，取消与原日志保留在 `console-v3-cancellation.json` 及 `console-v3-0*`。驱动 finally 已恢复源码字节和时间戳；该样本不作为通过或 warm 基准。后续重测须使用修复后输入，冷／首次准备与至少五次 warm 分开。
- 当前正在执行 13 个相关 Native 测试 owner 的联合准备与局部真实回归，使用现有 workspace producer lease，未复制 DLL 或创建另一套产品准备器。C 盘最近观测约 78 GiB 可用；保留活动输出和首次失败。

## 测试文档反馈边界复验

- 更新 Native 测试页的执行说明时，实际计划错误选择了 14 个 Native owner 编译；首次计划 `native-guide-plan/` 保留。原因是项目页归属直接变成编译前置，没有区分 runner 的普通说明与实际 C# 示例。
- `7466973461e8575b2fa99a26d7211121b6269a54` 修正该边界：只有明确的测试 runner 可在没有 C# 示例时省去文档编译；公开支持库继续参与 API 检查，inclusion 示例继续选择实际 owner 的 Native 上下文。4 项新回归和 4 项相关原回归均通过；CLI、源码架构和文档链接检查通过。
- 同一批 15 个实际文档文件修改经修复后统一计划与执行通过：instructions、architecture、documentation、wiki:managed 四项，零构建、零测试执行、零 Native 上下文。分别耗时 4.04、11.40、3.32、1.45 秒；这些是有其他构建并发的正确性记录，不是五轮空闲 warm 基准。原始证据 `native-guide-fixed/`，已完成状态为 true。
- Native 套件文档改为链接同一个实际计划入口，删除省略目标属性的直接 `dotnet test` 示例和重复的 BGFX 隔离名单；平台、进程与资源约束保留。运行说明明确 `--base HEAD` 只包含未提交变化，不能把空范围误当成已运行某个套件。
- `5f6c4dd824769371ad6aed3316580489e0320a17` 已推送；新 [Engine CI 42127319050](https://github.com/Wolfiora/InnoEngine/actions/runs/42127319050) 正在两个托管平台执行，后续范围选择修订仍为本地证据，不冒充该运行已覆盖。

## Native 输出与托管回归时限

- 13 个实际 Native 测试 owner 的本地联合编译完成，零警告／错误，耗时 46 分 02 秒。该轮包含工具输入更新后的首次原生产物准备，不作为 warm 样本。相同冻结 Editor 请求的后续消费者均记录 `reusedPreparation=1`、零 Native 编译及绑定生成；独立消费者仍完整读取 27,756 文件／2,248,079,121 字节，耗时约 40–102 秒，尚不能声明完整 UI 反馈达标。
- 联合回归得到 311 项 Passed、16 项具有 macOS 契约的 Metal NotExecuted；后者仍需 macOS 实际执行。运行前后全部选中测试输出仅新增 Native ImGui 的 `imgui.ini`。`TextDisabled_UsesTheLoadedCimguiSymbol` 保留所有操作与断言，关闭默认 ini 写入；修订后的单 owner 回归正在执行。原证据为 `deployment-closure-trx/`、`deployment-output-before.json`、`deployment-output-after.json` 和 `native-imgui-output-first-failure.json`。
- Build 套件此次 53 项全部通过，耗时 11 分 27 秒。长时间无控制台输出时使用官方 dotnet-stack 10.0.750501 取得线程栈，定位到真实 fixture 的 Support Pack 指纹读取；临时目录持续正常推进／回收，不是锁死。诊断工具及原栈位于本轮产物目录，不进入产品或工作流依赖。
- 本地联合驱动的 `finally` 将 PowerShell `$null` 转成空字符串，导致公共 publisher 的 `Record` 正确拒绝空 targetId。首次异常保留；只在相同 workspace lease 下用显式 `NullString.Value` 补正登记，不重跑已完成测试，不改 publisher 的参数校验。该问题属于临时验收驱动，正式 C# 验证入口仍传递真正的 null。
- `42127319050` 的 macOS job `127594831396` 在托管工具回归失败，后续 Native／Release 检查被阻塞。SHA-256 验证的原始诊断 ZIP 为 `ci-421273-macos.zip`（`4346aa55fe5665ade89daf75341d24d6c10a96ce726a6c3ecfdb2ac59c56db35`）。唯一失败用例 `ChangedCompilationInputsCannotPublishReusableBuildIdentity` 用时 20 分 07 秒；五轮共用的 20 分钟 token 在最后一次执行前的 SDK 环境复核耗尽。未发现可冒充通过的陈旧证据；也未完成最后一轮的预期失败断言。
- 前三轮没有中间计划断言，改为调用既有 `validate run` 的自动计划路径，避免单独 plan 后紧接 supplied-plan 环境复核；repaired evidence 明确引用该次执行保存的计划。后两轮仍验证分离计划，五轮行为及全部断言、20 分钟限制保持。修订后的本地真实 SDK 回归正在执行，不能将方法调整本身视为已解决托管时限问题。
- Native ImGui 修订后的独立编译零警告／错误，5 项测试通过，120 个输出文件哈希不变且没有重新创建 ini。`native-imgui-output-receipt.json` 与前后清单保留；正在运行的临时脚本仍使用旧 AST，在最后登记处再次暴露同一 PowerShell 空字符串错误，随后 `Record-DeploymentWorkspace.ps1` 在同一 producer lease 下完成正式登记，`deployment-workspace-recorded.json` 确认成功。没有为登记错误重复构建或执行测试。

## Console 首次完整通过与产品预算试验

- `367af11fb2d3c4c6d549f92ee297c79552bdb665` 的 Console 内部颜色修改完整计划首次通过，共 544 项测试，零跳过，未选择 Build integration 或规模性能套件；真实启动、八项工作流和全部实际面板生命周期均通过。总耗时 1656.793 秒，产品旅程 480.058 秒，其他反馈 1176.735 秒。首次联合构建 14 分 27 秒，不作为 warm 成绩。`console-v4-0/` 保存完整输入、计划、TRX 和结果。
- 临时计时驱动对 OrderedDictionary 使用属性过滤，导致产品耗时字段写成 null；逐项检查中原始 480.058 秒保持完整。修正派生总计单独保存在 `console-v4-timing-correction.json`，不覆盖首次记录。后续驱动将阶段行显式转为对象。
- 产品帧预算从 30,000 缩到 3,000 的探索运行在 17.768 秒后正常退出，但异步脚本尚未完成，验证明确失败：`The source extension did not complete the journey.`。保留 `journey-frame-Workflows/`，恢复原 30,000 帧，不把退出码零当成功。普通／扩展的循环、资源采样、帧预算和期限保持；报告新增实际耗时及循环数，修复扩展模式仍写“100 cycles”的错误文字。
- Windows/macOS 运行 `42135531646` 当前仍在执行；此前成功只属于上述局部输入，不能替代完整托管认证。

## Warm 瓶颈与输入快照事务

- `988c5563f528083c808d922c5d5b752c42845968` 的第一轮 warm Console 计划全部通过：544 项测试、八项工作流、100 次面板循环及十一轮 full-GC 采样。总计 1390.562 秒，其中产品 481.323 秒，其他反馈 909.239 秒。联合构建保留未变化依赖，仅清理六个根，仍耗时 10 分 03.82 秒；首次 Native 准备 182.759 秒、55,948 文件／4,498,462,802 字节，零原生编译和绑定生成。`console-v5-0/`、`console-v5-samples.json` 是一个诊断样本，不是五轮中位数，也未达到五分钟目标。
- 输入读取改为物理去重后的最多四路完整 SHA-256，所有读取者结束后再归并统计和登记整批结果；原有 fresh 阶段与消费者读取保护保留。新增真实文件回归在修改前 1 通过、2 失败，复现失败批次留下旧哈希及枚举末尾取消仍返回指纹。修改后 19 项批量／消费者回归通过；原两份纯输入身份／阶段测试迁入中立 Toolchains owner，所有断言原字节保留，仅改变命名空间，组合 28 项全部通过、零跳过，不执行 Native 产品准备。
- 首次修改后手动构建仍得到旧失败，原因是复制待应用源码保留较早时间戳，直接 SDK 增量构建未重编译。该临时驱动随后先完成串行 Clean，再独立 Build，得到上述通过；统一 validate 仍以内容身份判定清理，不受该手动时间戳问题影响。`input-batch-before*`、`input-batch-after*`、`input-batch-rebuilt*` 和 `input-batch-migrated*` 全部保留。
- 已有依赖内容证明下的 SDK 引用执行策略：solution 仍编译所有选中根，只用 GetTargetPath 解析普通已验证依赖，私有 Task 引导隔离该策略。实际 dependency Build target 计数断言确认未变依赖不重复执行 Build；联合根、有效属性变体及五轮编译身份三项真实 SDK 回归全部通过、零跳过，合计 22 分 53 秒，分别为 5 分 43.097 秒、7 分 30.222 秒、9 分 40.397 秒。原编译值、文件字节／时间戳、证据失效和恢复断言保持，证据为 `retained-targets-regression.log`、`retained-targets-trx/`。后续真实 Native 产品与至少五轮 warm 指标仍须执行。
- 相同 SDK 目录的完整内容读取对照（3,757 文件、418,543,223 字节，每次新 context）：串行五轮 warm 为 0.7155012、0.6481566、0.6933324、0.6590714、0.6791858 秒，中位数 0.6791858；并行五轮为 0.3637868、0.3350335、0.3539226、0.3305985、0.3673488 秒，中位数 0.3539226。两实现全部指纹相同；首次进程样本单列。完整 DLL 内容身份、样本及工作负载位于 `sdk-read-serial-samples.json` 和 `sdk-read-parallel-samples.json`。这个 418 MB 子负载证明读取收益，不能解释或代表完整 Native 准备的数分钟成本，下一轮使用实际 SDK target/task 计时继续定位。
- `a495157c` 的 `console-v6-0` 首次完整样本通过，包含工具输入变化后的真实 Native 冷准备：18 次 native process、7 组绑定生成；第二消费者独立核验并复用同一请求，零编译／生成。544 项测试、工作流与生命周期全部通过，总计 2355.863 秒，其中产品 478.792 秒；不计作 warm。随后 `console-v6-1` 在 331.579 秒后明确失败：选中根之间的引用被过早改成 GetTargetPath，产品准备后确定的 WindowsX64／Logging 输出实例尚未编译。测试及产品被阻塞，没有绿色结果，源码已恢复。
- 复用策略收窄到 solution 名单之外的未变化依赖；选中根的实际引用继续执行 Build，保留后期产品属性实例。首个新增 fixture 把两种 AssemblyName 放进同一 intermediate 目录，冷构建即发生输出冲突；改为声明动态输入并在 AssignProjectConfiguration 前添加实际引用，原独立属性变体断言仍保留。小型实际 MSBuild 查询另外暴露并修复了条件中嵌套引号、target 内 Update 作用范围的问题。`selected-root-reference-metadata-scoped.json` 现在证实 Editor 的平台根保留 Build，而 Core 依赖使用 GetTargetPath；对应首个失败和修正查询均保留。真实编译回归及后续产品 warm 样本仍待完成，不将元数据查询等同于编译通过。
- 精简调用后的五轮编译回归通过，用时 10 分 00.147 秒，原 20 分钟预算及全部行为断言保持；TRX 为 `compilation-direct-plan-trx/`。源码架构和记录链接检查通过。Native ImGui 修订已独立提交为 `731617678fb75f938eb763aa1f39823d23229976`；macOS 时限问题仍需新托管运行验证。推送修订会让既有并发策略取消旧 `42127319050` 的未完成 Windows 工作，原因是 macOS 已明确失败且旧树仍包含已复现的 Native ImGui 输出污染，不能通过重跑原树解决。
- 动态引用修正的真实 SDK 回归已通过、零跳过：`ChangedRootsCompileReferencesSelectedDuringBuild` 用时 8 分 25.583 秒（`late-reference-compilation-trx/`），在单处理器下验证初始 SDK 图未包含的实际引用仍正确编译后续变化。普通联合根与独立属性变体也各有通过证据；原错误 fixture、字面路径架构失败和旧目标条件失败均保留，不重写为通过。源码架构与文档检查通过；真实 Console 产品样本继续单独验证。

## Selected configuration and hosted product closure

- `8a590bd1` 的 `console-v7-0` 首次完整样本通过，1740.567 秒，产品 480.763 秒，其余 1259.804 秒。随后 `console-v7-1` warm 样本完整通过，1061.443 秒，产品 480.663 秒，其余 580.780 秒；544 项契约、八项工作流和 100 次生命周期循环全部通过。warm 联合构建 4 分 39.48 秒；首个 Native 准备 130.192 秒，第二消费者独立核验复用 38.409 秒，均无 native 编译或绑定生成。五分钟非产品反馈目标仍未达到；这是一轮诊断，不是五轮中位数。
- 托管运行 `42135531646` 已完成且汇总失败。macOS 的托管／原生测试、实际 Debug 产品旅程及十轮热缓存检查通过，Release 输出核对失败。实际 Release Editor 根依赖被 SDK 编译到 Debug 目录：solution 外的引用默认清除父 Configuration／Platform，根成功不能证明整条依赖使用 Release。首份诊断从经过认证的 artifact 读取具体日志，完整 ZIP 哈希核验另行进行，范围读取不冒充完整产物认证。
- 统一计划现在在图求值和执行属性中显式保留未选中依赖的父配置，仍让 SDK 处理显式引用映射，并保留全部真实输出核对。实际 SDK 回归覆盖 Debug、SourceDebug、Release，依赖不列为 solution 根。初次 SourceDebug 复现错误；新增 Release fixture 起初缺少 IDE solution 的 Release 声明，补齐后组合三项通过、零跳过（`release-configuration-declared-trx/`，43 秒）。中途失败记录保留。源码架构与文档检查通过。
- Windows 同次运行的契约测试通过，但 Debug 产品工作流等待退出超过 600 秒；后续热缓存和 Release 等检查被阻塞。已有日志确认 Direct3D11 初始化及首帧成功，artifact 包含 workflow-journey.tsv，仍需核对其内容和超时原因，不能据文件存在声称工作流通过。
- Windows artifact `13476860376` 完整 SHA-256 为 `00a7775bbf01bceb9d958fe7a076baba5cce3834ad87df272cfca18fe94d2acb`，已核验并安全提取。八项 workflow TSV 完整，stderr 为空；超时发生在场景完成后继续等待固定帧上限，首次结果仍保持失败。产品旅程改为原子完成通知、全部原有证据核对、操作系统正常退出请求、退出后证据与 Dispose 核对。没有增加引擎运行入口或改变产品 API；frameLimit 仍为保护上限，普通／扩展循环与资源预算保持。
- 本机真实 Workflows、Lifecycle 100 和 Lifecycle 1000 三项通过，分别 55.032、51.524、66.438 秒；正常 WindowCloseEvent、状态保存、产品资源释放和 Dispose end 均有实际日志。前两项保留 30,000 帧上限，扩展项保留 180,000 帧／5,400 秒上限；不再把上限报告为实际完成帧数。证据为 `journey-completion-workflows/`、`journey-completion-lifecycle/` 和 `journey-completion-extended/`。macOS 的正常取消退出仍待托管认证。
- 产品驱动协议回归通过 16 项，包括缺失场景／采样／循环、错误完成通知、非零退出、超时、扩展 1,000 循环、内存增长、待处理事件及释放失败。首次 15 项断言通过但手动调用保留了最后一个预期失败的 native exit code，脚本现显式在全部成功和清理后返回零；原日志保留，`journey-protocol-final.log` 为最终成功入口证据。测试临时目录在自己的边界内清理。原生应用真实验收与协议 fixture 分开记录，不以模拟进程替代实际产品。

## Binding SDK test ownership

- 五份绑定 SDK 回归迁入 `Inno.Build.Bindings.Tests`，只引用 Bindings 与中立 Toolchains；产品原生准备、窗口和 GPU 不在该闭包。全部原断言通过反向命名空间／helper 替换与 Git 原字节比较，迁移清单为 `binding-test-owner-migration.json`。源码项目总数及层次仍由 SDK 图求值，不新增测试名单。
- 独立 owner 的 30 项实际 SDK 回归全部通过、零跳过，执行约 2 分 56 秒；`binding-owner-serial.log` 与 `binding-owner-serial-trx/` 保留。此次运行没有 Editor 或 Native 产品准备。原套件的顺序调度属性随后同步保留；真正的竞争仍在用例内部显式执行，托管 CI 会验证最终项目结构。
- 源码架构与文档链接检查通过。解决方案仅增加新 owner 的声明、配置与分组，不接受 `dotnet sln add` 对其他已有项目配置的重排。

## Current published candidate and complete Console samples

- Published implementation: `aadf63c34f4f701d505ed9f944a1bf1cce3681c9`. Exact Git trees and commits were checked before the fast-forward; transient transport failures are retained. PR certification is run `42164882784`; the independent full/cold/extended certification is run `42169862040`. Both Windows/macOS matrices are still executing at this update. Dispatching or starting a job is not acceptance.
- The evaluated managed graph now contains 249 projects and 65 test owners. The Console color change still selects only Hosting, PlayMode and Presentation; neither Build integration nor Binding SDK tests is selected. Evidence: `console-v8-4/managed-sdk-graph.json` and its plan. The new SDK owner does not introduce an engine Native product preparation dependency.
- `console-v8-0` is a first-after-tool-change sample, not a pristine cold build: complete success in 1389.1803794 seconds, including 126.6439922 seconds of products. Its initial joint compilation took 15:58.70, with no errors and one safely deferred cache-maintenance budget warning. Valid unchanged dependencies were retained in all subsequent samples.
- Five warm runs all passed the same 544 contract cases, eight workflows and 100 lifecycle cycles. Total seconds: 770.5404397, 714.0332937, 696.1765828, 718.1395177, 698.9560799; median 714.0332937. Product seconds: 128.0152984, 122.7118673, 124.2039723, 124.4243408, 121.735802; median 124.2039723. Non-product feedback seconds: 642.5251413, 591.3214264, 571.9726105, 593.7151769, 577.2202779; median 591.3214264. The 300-second Editor contract target remains unmet. No checks or assertions were removed to improve the result.
- Raw inputs, check timings, TRX and completed receipts are in `console-v8-0/` through `console-v8-5/`; the sample series and derived summary are `console-v8-samples.json` and `console-v8-summary.json`. Console source bytes were restored and compared with their saved original; the main checkout was clean after the series. No other local compilation ran during these warm samples.
- The matched original `7f56dd64b` Editor comparison is being prepared in the previously owned isolated fixture. The first driver used the later Presentation project path; the corrected driver maps its original Interactions/Graph owners and the two moved script projection cases. Subsequent preflight identified uninitialized pinned source modules, including FreeType's nested dlg. First failures and setup logs are retained; only the owned fixture's Git checkouts/configuration were populated. Main and adjacent third-party sources were not edited. The complete recursive pin preflight now passes. The first valid baseline sample will be labelled initial preparation, since earlier failed setup left reusable tool outputs; it must not be presented as pristine cold performance.

## Acceptance policy superseded by the user

On 2026-11-06 the user withdrew fixed 30-second/two-minute/five-minute feedback thresholds and the extra work spent chasing them. Earlier measurements and unmet-target statements above remain historical, not current blockers. Required correctness, ownership, cache, cancellation and lifecycle checks remain. Long third-party builds are acceptable when they are progressing; investigate actual redundant work, lock/process stalls and relative regressions instead of enforcing a universal time budget.

The optional original Editor comparison was canceled while its initial build was generating bindings. `original-editor-v4/cancellation.json` records the explicit scope change; the driver exit/failure file is a consequence of cancellation, not evidence of an engine defect. No baseline was declared passed. Current five warm samples remain observations and are not rerun. PR certification `42164882784` and full/cold/extended certification `42169862040` continue unchanged.

## Foreground inventory regression and canceled-output retirement

- Extended Windows job `127734982408` in run `42169862040` failed on `InterruptedInventoryResumesCompletedShardsBeforeRetiringAnyArtifact`. Its first cancellation assertions completed; the subsequent full resume at line 118 exhausted the implicit five-second background maintenance budget. The first TRX and results are retained as `ci-421698-windows-toolchains.trx` and `ci-421698-windows-results.json`. These are bounded diagnostic extractions, not certification of the entire artifact archive.
- The inventory fixture now explicitly budgets foreground work independently of the automatic maintenance slice. All cancellation, checkpoint reuse, reader protection, incomplete-graph protection and final retirement assertions remain. Five local inventory cases pass with no skips (`inventory-foreground-trx/`); source architecture validation also passes. Production defaults and dedicated timeout-contract tests are unchanged. Hosted verification of this correction remains pending.
- The canceled original Editor fixture returned to its clean original `61f2d2b9` source identity. Its exact `original-editor/bin`, `obj` and `.inno-task-foundation` outputs were registered under a non-overlapping publisher, previewed, then retired using catalog/dependency protection and artifact leases. `canceled-editor-retirement.log` reports 140,881,472 logical bytes reclaimed and zero skipped candidates; other owners and first-failure logs remain. No raw recursive cache deletion was used.

## macOS product completion transport

- PR job `127719008116` completed with failure on the real Metal workflow journey. `ci-421648-macos-workflow-result.json` records completed-journey exit code 2. Its boot log records saved project state, ended run loop and released Editor resources, but no final `Dispose end.` marker. The existing application reports normal shutdown completion only after successful return from its run/using scope; `SIGINT` takes the separately defined cancellation path. These bounded artifact diagnostics retain the original failure and do not establish complete retirement.
- The driver now sends `SIGTERM`, which the pinned SDL quit handler translates into the ordinary application quit event when no prior handler owns that signal. The repository uses .NET 10, whose runtime no longer installs a default SIGTERM termination handler ([Microsoft contract](https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/10.0/sigterm-signal-handler)); the SDL behavior is documented [upstream](https://wiki.libsdl.org/SDL3/SDL_HINT_NO_SIGNAL_HANDLERS) and matches the pinned source. The driver now rejects every nonzero exit, including cancellation, and retains the full retirement assertion. No Editor, vendor or generated code changed.
- Windows protocol regression passes 17 cases, including rejection of exit code 2. Three additional macOS-only cases exercise actual signal delivery, retirement failure and nonzero exit after a requested stop; their hosted execution and real Metal product completion remain pending. `journey-term-protocol.log` is local protocol evidence, not native product certification.
- The actual extended Windows plan contains 249 owners, 65 distinct test owners and 333 checks: 17 domain, 33 contract, five integration, eight native and two performance test owners. All domain checks use the managed context. `owner-coverage-aadf63c3.json` records the source plan digest and complete classification; this is selection coverage only.

## SDK correctness fixture watchdog

- Candidate `61834b57` was published with the inventory and macOS transport corrections. The superseded Windows PR job and remaining extended macOS job were canceled; old runs `42164882784` and `42169862040` both retain failed aggregate results and partial diagnostics, not certification.
- Run `42180419875` macOS job `127768993388` passes all 20 journey protocol cases, including actual SIGTERM delivery and rejection of canceled/failed completion. `ci-421804-macos-journey-protocol.log` records the result. Real Metal products were blocked by an earlier managed regression failure, so ordinary product shutdown remains unverified.
- `ChangedCompilationInputsCannotPublishReusableBuildIdentity` again exhausted its aggregate 20-minute token: 20:07.352 elapsed, with the final build canceled after 20.221 seconds and marked `Blocked`. The failed assertion at line 279 expected an actual failed behavioral test, correctly refusing to accept cancellation as that evidence. The preceding mutation, repair and unchanged-dependency assertions completed. First TRX: `ci-421804-macos-validation.trx` (bounded diagnostic extraction).
- The five-cycle correctness fixture now has a one-hour runaway safeguard instead of the accidental aggregate latency limit. No operation, assertion or production cancellation policy changed. The affected tool test project compiles with zero warnings/errors (`validation-fixture-budget-build.log`); the full hosted fixture still must complete before certification. No optional local five-cycle timing rerun is added for this budget-only correction.

Current candidate and hosted run status are maintained in [issue #75](https://github.com/Wolfiora/InnoEngine/issues/75) and [PR #76](https://github.com/Wolfiora/InnoEngine/pull/76). Each dated checkpoint above retains its original source and execution state; report updates alone must not trigger another local build or benchmark series.

## Cocoa normal-quit transport — 2026-11-06

- Run `42189144909` at head `eecdbebe` passed all 64 validation-tool cases and 191 architecture-tool cases on macOS. The five-cycle SDK correctness fixture now completes without exhausting its aggregate safeguard. Native compiled API, Wiki and the native/Editor test owners also passed before product acceptance; these are scoped results, not full certification.
- Actual Metal Workflows completed its scenario, then exited with code 143 after SIGTERM. The boot log ended after the first frame and lacked all normal-retirement markers. The prior assumption that SDL would receive the signal was invalid for this hosted process; the simulated SIGTERM receiver did not cover that environment. The first failure remains failed.
- The driver now uses `NSRunningApplication.terminate` for the exact owned process ID. The pinned SDL Cocoa application overrides `terminate:` to enqueue its normal quit event. No signal fallback or nonzero-exit allowance remains. Windows still requests closure of the owned primary window.
- macOS protocol cases now use a real AppKit application and event pump, covering normal completion, retirement failure and nonzero exit after the request. They no longer install a synthetic SIGTERM handler. Actual product disposal remains a separate required check. No product, vendor or generated code changed.
- Bounded diagnostics: artifact `13496176318`, 25,324,223 bytes, server SHA-256 `29f1aa10643d50513332ac921a9d376d6b612d4efe91a523d68800fa5568d1f7`; `ci-421891-macos-workflow-boot.log` and `ci-421891-macos-workflow-result.json` are range-extracted diagnostics, not a whole-archive digest certification. Source run: https://github.com/Wolfiora/InnoEngine/actions/runs/42189144909.
- Local correction evidence: 17 Windows product-protocol cases pass (`journey-appkit-protocol-windows.log`); source architecture, product documentation and agent-document checks pass. The three AppKit transport cases and actual Metal journeys remain pending hosted execution. No local engine rebuild was required for this test-driver change.

## Release bundle compiled roots — 2026-11-06

- The Windows job in run `42189144909` completed Debug products (158.43 seconds), ten unchanged native builds (1,049.58 seconds), Release API checks, Release products (120.07 seconds), Support Pack publication/closure (392.47 seconds), and NativeAOT restore (2.10 seconds). These are actual scoped successes; the aggregate remains failed.
- Its final `release-bundle` failed because output capture treated the NativeAOT restore-only Player dependency closure as compiled Release roots. `Inno.Adapter.Content.FileSystem` belongs to the actual Player references, not the two selected Release build roots; its standalone DLL was correctly absent from that compilation layout.
- The plan now explicitly selects the same Editor/Build roots for bundle output capture that it selected for Release compilation. Actual SDK reference traversal, archive digests and every prerequisite remain required. Player/restore-only source inputs remain in the bundle identity, so this correction neither drops freshness checks nor claims NativeAOT product execution.
- Debug and Release plan regressions include a Player-only dependency. Both failed before the correction; both pass after it and after the final explanatory source comment (`release-bundle-before-trx/`, `release-bundle-final-trx/`). The CLI builds with zero warnings/errors in 28.27 seconds; source architecture and product documentation checks pass. No local engine native build or broad timing comparison was run.
- Windows first-failure artifact: `13499617781`, 140,757,600 bytes, server SHA-256 `3681bb39e9df3292ac73e78cfa0bf85d05d5f89b698d0e536846557ea6f7154d`. Its whole archive has not been downloaded or digest-certified locally; the authenticated job log supplies the reported failure and check results.
- The Cocoa correction also passed the actual [macOS AppKit-only probe](https://github.com/Wolfiora/InnoEngine/actions/runs/42207683736): 20 protocol cases, including normal quit, retirement failure and nonzero exit. Probe commit `99cb6dac` differs from implementation `f025a783` only in its temporary workflow; this was checked before execution. The temporary branch was deleted after completion, and its diagnostic artifact remains retained. It is not a real Metal product certification.

## Test deployment output ownership — 2026-11-07

- PR run `42215053486` at candidate `59d7c0c9` passes macOS Debug/Release products, ten hot builds, Support Pack, NativeAOT restore and release bundle creation. Final reconciliation fails four output-stability checks: Build, PlayMode and Scripting test deployments, plus their Native Wiki consumer. This remains a failed certification. The user paused execution; remaining jobs in that run and extended run `42215649686` were cancelled, then work resumed at the user's request.
- Comparing build and test receipts isolates each mutation to the test assembly's own output directory; shared dependency directories remain unchanged. Bounded extraction `ci-422150-macos-results.json` retains provenance from artifact `13514715875` and does not claim whole-archive digest verification.
- Local `output-mutation-before/` and `output-mutation-clean-before/` reproduce SDK fixture owner/lock metadata under the Build deployment and `imgui.ini` under PlayMode/Scripting deployments. An existing local PlayMode ini initially masked the new-file mutation; its bytes were preserved under the explicit workspace writer lease before the clean reproduction.
- The SDK app-host fixture now uses a process-owned temporary root outside the deployment. Console entry, popup and Shader Editor fixtures explicitly disable automatic ImGui ini persistence; real layout persistence tests and all original assertions remain unchanged. No runtime, public API, generated/vendor code or validation-integrity rule changed.
- Three-owner compilation passes with zero warnings/errors in 6m34.31s. The final local regression passes 153 cases (1 SDK host, 6 layout, 146 Shader Editor), with zero skips and identical complete before/after output file hashes for all three owners. Raw TRX, manifests and build log are under `output-mutation-after/`. Native preparation starts no native process or binding generation; consumers reuse the same request while retaining integrity reads.
- Source architecture and product-document checks pass. The existing staged, detached `pipeline-acceptance-fixes` worktree is untouched; implementation uses the clean published `ai-pipeline` checkout. Hosted certification of this correction and default-branch-dependent release adoption remain outstanding; current run status is maintained in #75 and PR #76 without report-only source commits.
