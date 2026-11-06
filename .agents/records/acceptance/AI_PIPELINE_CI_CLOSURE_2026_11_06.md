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
- 精简调用后的五轮编译回归通过，用时 10 分 00.147 秒，原 20 分钟预算及全部行为断言保持；TRX 为 `compilation-direct-plan-trx/`。源码架构和记录链接检查通过。Native ImGui 修订已独立提交为 `731617678fb75f938eb763aa1f39823d23229976`；macOS 时限问题仍需新托管运行验证。推送修订会让既有并发策略取消旧 `42127319050` 的未完成 Windows 工作，原因是 macOS 已明确失败且旧树仍包含已复现的 Native ImGui 输出污染，不能通过重跑原树解决。
