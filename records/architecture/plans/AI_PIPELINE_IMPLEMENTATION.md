# AI pipeline implementation — issue #75

Status: in progress. This record does not certify completion.

- Branch: `ai-pipeline`; starting commit: `7f56dd64b`.
- Scope: InnoEngine instructions, skills, validation, test isolation, native preparation, cache ownership, CI and draft release delivery. Adjacent repositories remain read-only.
- Preserved constraints: architecture and public contracts, Identity/Missing/reload, serialization, resource ownership, real native and lifecycle assertions. No compatibility entry points or generated-binding changes.
- Approved test changes include regression and verification-tool tests. Root rule 8 is explicitly superseded; every other original bullet is retained and checked against its original hash.
- SDK: explicitly selected `C:/Users/23842/.dotnet/dotnet.exe`, 10.0.402. The default system host has only SDK 9 and is not suitable.

## Work remaining before completion

- [ ] Complete and test shared SDK evaluation and validation planning/execution/evidence.
- [ ] Verify all test-owner metadata and transitive isolation; preserve native Input/UI assertions.
- [ ] Complete frozen native request reuse, consumer reading protection, concurrency and cancellation regression coverage.
- [ ] Complete resumable cache inventory and immutable cross-runner import/export verification.
- [ ] Replace old orchestration and remove obsolete selectors after caller migration.
- [ ] Implement and exercise hosted Windows/macOS CI, schedule gating, aggregate reconciliation and draft release provenance checks.
- [ ] Configure required branch status while preserving existing review rules, or record an actual permission blocker.
- [ ] Record five warm samples for each workload, cold separately, raw timings and medians.
- [ ] Run complete closure validation and read-only downstream consumer checks.
- [ ] Obtain real remote CI and release-rehearsal evidence; do not substitute YAML inspection for execution.

## Evidence and continuation

The rule checker currently verifies 241 original bullet mappings. Root instructions are 3,275 bytes; a representative ordinary C# task loads 7,999 bytes and a Wiki task 5,002 bytes. Broad cross-domain tasks must split their instruction reading without truncation.

- Committed foundation: `fdaaf5b4`, preserving all original sections including nested requirements/examples (section 8 explicitly replaced by the approved test policy). The coverage JSON is tracked through a precise ignore exception.
- Validation planner public-entry regression: 9 passed, including added/moved/deleted inputs, imports, dynamic inputs, unknown ownership, domain/native isolation, stale/missing evidence, advancing merge bases and declared native contexts. Newer output-manifest, product journeys and workspace-lease changes still require their own integration validation.
- Input domain: 17 passed; UI domain: 1 passed; no native preparation reported. The original native assertions moved to SDL/RmlUi backend owners; real native execution remains pending.
- Toolchains regression: 17 passed, including independent preparation readers, input invalidation, waiting/cancellation/corruption and actual batched SDK reference resolution. The first batch-resolution failure exposed a missing fixture checkout marker; the second exposed an SDK optional-import condition. Both original failures and subsequent fixed-run logs are retained.
- Native preparation Task compiled with zero warnings/errors (212.77 s). This does not yet prove same-root production request reuse.
- Architecture regression: 166 passed with the explicit `BuildGraph!=Complete` filter. The full repository gate is a distinct pending check; this filtered result does not certify the complete graph.
- New evidence records hash complete selected output directories, reject stale environments, and block work after failed prerequisites. Stable validation output ownership now uses the existing cache registration and Core IO coordinated lease; unregistered existing directories cannot be silently adopted.
- Wiki reference resolution now uses the shared SDK evaluator; scoped Wiki selection and output evidence are under active validation. Old orchestration remains until all callers are migrated.
- Retired duplicate gates: `RepositoryArchitectureTests.CurrentRepositoryPassesArchitectureRules` moves to required source/compiled-API plan checks; `RepositoryWikiTests.CurrentWikiCompilesWithTheCurrentLogicalScriptContracts` moves to required scoped Wiki checks and current CLI-generated script references. Validator regression cases and the logical public-symbol assertions remain. Complete integrated evidence is still pending.

Raw local measurements and build logs belong to `artifacts/validation/`, outside tracked source. The initial per-project SDK baseline has two completed samples (164.9761196 s and 139.2029125 s for 241 SDK starts). The measurement process was interrupted; incomplete samples are excluded and the required five-sample comparison remains pending.

Next: finish native versus managed execution contexts and scoped Wiki integration, verify workspace cancellation/output competition, then migrate cache maintenance and hosted CI/CD. No remote configuration has changed, and no real Windows/macOS workflow result or release rehearsal is claimed.

## Editor isolation and integrated restore update

The Graph document/history/reload owner now has no ImGui build dependency. The Interactions owner retains history, scene/reload and planar navigation. Canvas projection, presentation-runtime partial tests, asset-browser history and Console-backed settings discovery moved unchanged to `Inno.Editor.Presentation.Tests`; its sampling/lifecycle evidence is not reused across runs. Inspection retains its real drawer/ImGui closure; Hosting retains its real backend composition preflight. These dependencies are intentional and must not be reported as domain isolation.

The first Editor split run found nine settings fixtures dynamically loading the real logging panel. Those fixtures moved with their dependency rather than removing discovery or assertions. First failure logs remain in `artifacts/validation/editor-domain-sdk.log`.

The old `Directory.Solution.props` full-repository restore enumeration bypassed `.slnf` selection. An attempted escaped project-list override failed as one combined path (`editor-domain.log`). Restore now consumes the SDK's actual solution/filter references. The integrated two-owner run passed Graph 10 and Interactions 48 tests with zero native preparations in 21.5805624 seconds.

Five subsequent warm build-and-test samples, SDK 10.0.402, same machine and unchanged domain workload: 17.8616954, 17.7969079, 17.5690239, 17.9237441, 18.0319758 seconds; median 17.8616954 seconds. All 58 cases passed each time with zero native preparations. Raw logs/TRX and `editor-domain-warm-samples.json` are retained. This is the after measurement; a matched five-sample before comparison is still outstanding.

Validation planning currently has 11 passing public-entry regressions. A twelfth script-only Wiki product preparation regression has been added and is pending execution. Release-source negative contracts pass 15 cases; this is not a release rehearsal.

CI source now uses the unified plan on Windows/macOS and an always-present `Engine validation` aggregate, with missing/failed platform evidence rejected. Scheduled depth, immutable artifact delivery, dependency-submission consolidation, actual remote runs and branch enforcement remain pending. The original CI file has changed locally only.

Product journeys now declare a pinned Samples revision in `tests/fixtures/editor-ui/consumer.json`, consume a separate tracked-file copy and retain the real packaged plugins. Neighboring Samples/Canvas/Rendering2D working directories are not modified; Canvas and Rendering2D currently contain other-session edits and must remain read-only.

## Clean-checkout execution, cadence and release implementation

The real-source integration checkout is a detached shared clone under `artifacts/validation/engine-integration-fixture`; the main branch and index were not changed. Source snapshot `fdc597d723974751e692f00f5a3c232ac044437e` plus one comment-only Input test probe passed its complete selected plan: instructions, source architecture, union compilation, compiled contracts, 17 Input tests and Wiki. First cold union build: 121.7935895 seconds; total: 177.1845592 seconds. This snapshot predates the later cadence/release/cache additions.

The first two integration failures were retained. An uninitialized submodule was incorrectly borrowing the parent Git identity; explicit gitlink identities now distinguish unavailable native sources. Clean source validation also exposed semantic `inheritdoc` checks depending on old DLLs; compiled semantics now execute after the union build while all cheap source rules remain global. Architecture regression passed 167 cases (`BuildGraph!=Complete`), including rejection of unrelated inherited XML after compilation. Rule extraction now explicitly uses UTF-8 when launched through redirected .NET processes.

Five unchanged-input warm full Input runs: 60.2114318, 60.2863779, 59.7630361, 60.6245840, 59.8384172 seconds; median 60.2114318. All passed with zero native preparation. Exact local evidence reuse completed in 25.1801216 seconds: rule, source, API, test and Wiki results reused; the incremental union build still executed (9.14 seconds). Raw plan/results/TRX and samples remain under `artifacts/validation/input-*`. Matched before measurements are still incomplete.

Validation public-entry regressions now pass 16 cases, including explicit extended/Release requirements. Full plan generation currently sees 245 owners and 320 required checks. This plan is stale after subsequent source changes and is not an execution certificate. The CLI and source architecture gate passed before the latest inventory/import additions.

CI source now has explicit Plan/Run stages, hosted Windows/macOS, cadence gating, cold-input rejection, ten hot builds and 1,000-cycle extended lifecycle input. Full acceptance adds Release compilation/API/product journeys, Support Pack export under its publication lease and an exact-output deliverable manifest. The default branch is the only source eligible for transport-cache saving and draft release input. No remote workflow has run yet.

Local CI contracts: aggregate 12, cadence 9, release source 15, release bundle/archive boundaries 19 passed. `draft-release.yml` supports read-only rehearsal and optional draft creation from verified existing bytes; it never publishes or rebuilds. Actual uploaded-release digest verification and real remote rehearsal remain unexecuted.

Cache maintenance now checkpoints completed capacity measurements per artifact under the existing catalog. It never checkpoints retirement authority, still requires a complete current protection graph, and leaves dry-run progress unchanged. Toolchains regression passed 26 cases, including three inventory-progress and six clean-consumer import cases. The first inventory run had one nullable test warning; it was corrected before the 26-case run. Native payload import uses the existing publisher to validate and re-register owners; no leases, PID records or writable workspaces are copied.

Next: finish actual native migration execution; verify metadata contexts, complete remaining full-depth/restore requirements, remove old callers/selectors, and run complete closure validation. Native transport cache selection/size behavior, source-declared dynamic test inputs, resumable retirement metadata cleanup, bounded parallel compilation, real remote CI/branch enforcement/release rehearsal and matched performance baselines still require completion. Issue #75 remains open.

### 收口检查：隔离执行、旧入口和依赖提交（2026-11-05）

- 删除 `Invoke-ManagedChecks.ps1`、`Invoke-EditorUiAcceptance.ps1`、`Invoke-WikiChecks.ps1`、`Select-ValidationScope.ps1`、`Test-ValidationScope.ps1`、`New-ValidationSolutionFilter.ps1`；职责分别由统一计划/构建并集、保留的隔离执行器、产品旅程、热构建采样和 Wiki 检查承担，不保留兼容转发入口。相关当前文档同步迁移；历史 records 保留原始事实。
- 隔离测试新增逐用例 TRX 对账：必须执行至少一个测试、全部通过、计数与记录一致且属于所发现的契约。7 个负例/正例已通过；不能以进程退出码 0 代替执行证据。
- NativeAOT 还原与 Support Pack runtime/PlayerLink MiniAudio 内容一致性检查迁入统一计划；后者 6 个合成归档用例已通过。真实完整产品执行仍待完成。
- 已读取实际依赖提交日志：平台 Automatic Dependency Submission [run 41871525488](https://github.com/Wolfiora/InnoEngine/actions/runs/41871525488) 在 Linux SDK 10.0.403 恢复普通项目并提交 NuGet 图；仓库 [run 41326947632](https://github.com/Wolfiora/InnoEngine/actions/runs/41326947632) 在 Windows SDK 10.0.402 恢复 SourceDebug solution 并再次提交。保留平台自动提交作为唯一 snapshot owner，删除仓库重复工作流。Windows/macOS、配置与 NativeAOT 的验收继续由 Engine CI 统一计划执行；平台提交不作为这些门禁的通过证据。
- 第一次真实 SDL/RmlUi 迁移构建保留失败日志 `artifacts/validation/native-migration-build.log`：原生准备成功，迁移后的 RmlUi 测试缺少显式 `using Inno.UI`，编译失败；只修复命名空间引用后使用同一输出 owner 再验收。首次冷构建 1161.16 秒不是 warm 性能样本，也不是通过记录。

### 集成收口进展（2026-11-05）

- 修正命名空间后的 SDL/RmlUi 原生测试共 22 项通过。warm 构建 288.38 秒，构建加测试 297.649 秒；第二消费者记录 `reusedPreparation=1`、零原生编译进程、零绑定批次。输入新鲜度检查仍花费显著时间，此单次样本不冒充五次性能中位数，也不表示最终代码已通过完整矩阵。
- 批量图包含 ProjectReference 的实际属性变体；条件源码、imports 与 native 依赖不会只按根属性判断。验证器新增变体回归已通过；Architecture 167 项通过，之后文档结构入口两项定向回归通过。完整计划识别 245 owner、321 必需检查；计划不是执行证书。
- NativeArtifactTransfer 通过现有 publisher/lease 导出已登记 immutable payload；导入先验证完整归档再安装候选，不复制 owner 状态、不覆盖活动读取者。相关传输、盘点、准备和图求值的 24 项定向 Toolchains 测试通过。
- CI 合成契约共 72 项通过：汇总 12、定期选择 9、发布来源 19、归档边界 19、进程隔离 7、Support Pack 闭包 6。每次重试使用独立 attempt 产物；最新合格结果不覆盖首次失败。耗时比较仅告警，不传递旧运行的绿色状态。
- 删除隔离执行器逐项目 SDK 查询；实际测试项目身份与输出来自计划中的批量快照。文档结构检查复用现有 Wiki owner/正文/链接验证器，源码检查消费同一实际 SDK 图。
- 仍未完成：真实 Windows/macOS 远程矩阵、完整产品/Release/reload 验收、前后五样本性能对照、SDK/外部工具变化的证据边界加固、取消时已完成 immutable 产物的远程保留、盘点退休后的进度清理、分支保护接入与合格默认分支产物的 CD 演练。#75 保持打开。
