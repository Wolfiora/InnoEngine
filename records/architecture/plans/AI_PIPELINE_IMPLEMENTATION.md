# AI pipeline implementation — issue #75

Status: in progress; this record does not certify completion. [Issue #75](https://github.com/Wolfiora/InnoEngine/issues/75) remains open. [Draft PR #76](https://github.com/Wolfiora/InnoEngine/pull/76) contains the committed implementation.

## Scope and decisions

- Branch `ai-pipeline`, starting commit `7f56dd64b`. Only InnoEngine is writable; adjacent repositories and third-party/generated sources remain untouched.
- Preserve architecture, public contracts, Identity/Missing/reload, serialization, ownership and real native/lifecycle assertions. Test changes are limited to task-required regressions, separation and verification tools.
- One Build CLI owns plan/run/full/evidence. Actual SDK graphs own project references and outputs; no second project/platform list or legacy forwarding entry point.
- Stable registered workspaces hold writer/read leases; logs are per execution. Managed compilation uses at most four workers; native preparation retains one owner.
- Hosted Windows/macOS, daily new-commit certification, weekly cold/ten-warm/long-running acceptance, immutable artifact transfer and manual draft-only releases. No merge or formal publication is authorized.
- Explicit local SDK: `C:/Users/23842/.dotnet/dotnet.exe`, 10.0.402. The system SDK 9 host is unsuitable.

## Current implementation identity

- `fdaaf5b4`: preserved rule mappings, canonical domain rules and four bounded repository skills.
- `5b14a2df`: unified validation, actual SDK graph variants, test separation, native preparation reuse, cache transport/inventory, hosted CI and draft delivery. Both commits are on the remote branch.
- `b8ce42dc`: SDK/runtime/shell/external-import content identity; restored package and output identity; forced compilation when content changes despite timestamps; stricter TRX reconciliation; test-only API exclusions; equivalent graph payload deduplication and stage timings; retirement checkpoint cleanup; cancellation artifact retention. Pushed; corrected hosted run [41936778114](https://github.com/Wolfiora/InnoEngine/actions/runs/41936778114) is in progress on both platforms.
- Current follow-up shares syntax trees within one architecture invocation. No persistent source cache or rule exclusion is introduced.
- Real hosted run [41923463340](https://github.com/Wolfiora/InnoEngine/actions/runs/41923463340) validated merge candidate `d7477209` for `5b14a2df`. Both platforms failed at the migrated Presentation test's missing `Inno.Editor.Interactions` import and detected managed/native output overlap. The aggregate correctly failed; its oversized summary exposed a separate reporting defect. Follow-up fixes add the import, isolate context output roots and bound the summary while retaining original reports. These fixes still need the next real matrix.
- Default-branch ruleset `13788396` now actively requires `Engine validation` from the verified GitHub Actions integration `15368`, with strict base freshness. The original PR/review/discussion-resolution/non-fast-forward rules and empty bypass list were preserved and read back. No merge queue was enabled. Workflow adoption still requires the user's merge decision.

## Rules, owners and removed mechanisms

The checker retains 241 original bullet mappings, including nested requirements/examples; the old absolute test-edit prohibition is explicitly replaced by the approved bounded test policy. Root instructions are 3,275 bytes; representative C# instructions 7,999 bytes and Wiki instructions 5,002 bytes. Full provenance is in `docs/architecture/rules/coverage.json`.

Input and UI domain suites no longer depend on SDL/RmlUi preparation. Original native assertions belong to those backends. Editor Graph/history/reload and Interactions remain domain owners; real ImGui projection, runtime lifecycle, asset-browser history and logging-panel settings discovery moved to Presentation. Inspection/Hosting retain their real backend prerequisites.

Removed `Invoke-ManagedChecks.ps1`, `Invoke-EditorUiAcceptance.ps1`, `Invoke-WikiChecks.ps1`, `Select-ValidationScope.ps1`, `Test-ValidationScope.ps1` and `New-ValidationSolutionFilter.ps1`. Isolated process execution, product journeys and build-cache sampling keep their distinct implementations, invoked by the unified plan. Removed duplicate whole-repository xUnit architecture/Wiki invocations; equivalent required checks live in the plan, while validator regressions remain. Solution restore now follows actual solution/filter references.

Removed the duplicate repository NuGet submission workflow after comparing real runs 41871525488 (platform automatic submission, Linux) and 41326947632 (custom Windows submission). Automatic submission remains the snapshot owner; actual Windows/macOS, configuration and NativeAOT restore acceptance remain in Engine CI. These changes take effect on default-branch adoption.

## Evidence and first failures

Raw logs, TRX, plans and timings remain under `artifacts/validation/`. Results are tied to their recorded inputs, not automatically valid for subsequent edits.

- Architecture regression: 167 passed with `BuildGraph!=Complete`; later two documentation-structure cases passed. This does not certify the full repository/product graph.
- Shared syntax-tree follow-up: all 168 current architecture regressions passed with `BuildGraph!=Complete`; CLI compilation passed with zero warnings/errors (`shared-syntax*`). Five fixed-source runs before and after passed the same whole-repository source checks.
- Input domain 17, UI domain 1 and Editor domain 58 passed with zero native preparations. SDL/RmlUi migration later passed all 22 real native tests (`native-migration-*-2`); the first cold build failed only at a missing `using Inno.UI` after native preparation and is retained separately.
- Validation public-entry regressions: 19 passed together (`validation-content.log`). New external source and import identity cases passed after a clean tool compilation (`external-instance-rebuilt.log`). The first external-source failure used a stale tool binary during overlapping editing/compilation; its log remains. Two actual SDK context-isolation cases subsequently passed (`context-isolation.log`).
- Toolchains selected regressions passed 24 cases for preparation/transfer/inventory/SDK capture; four retirement inventory cases subsequently passed, including an active reader retaining both payload and checkpoint. Two graph compaction cases passed (`graph-compaction.log`), retaining conditional inputs and every originating property set.
- CI PowerShell contracts: previous 72 passed (aggregate 12, cadence 9, release source 19, archive 19, isolated execution 7, Support Pack 6). Aggregate now passes 15, including dependency identity requirements; the other unchanged cases have not been repeated unnecessarily.
- Real content-compilation fixture snapshot `ecf6dda0f17f0df4447e8e04b3b36c4cdc387778`: 18 Input cases passed (`content-inputs-second`). Changing the sentinel result from 1 to 2 while preserving byte length and timestamp forced recompilation and produced a real xUnit failure (`content-inputs-negative`). Restoring the exact original bytes/timestamp passed again (`content-inputs-restored`). No native preparation. Initial format-rule failure is retained (`content-inputs-first`), not counted as executed behavioral evidence.
- Actual compiled-instance selection passes the 18-case Input integration at snapshot `8674f042` (`instances-first`, 126.0689104 seconds including initial tool/input rebuild). The preceding `compact-first` attempt correctly failed because output discovery incorrectly included an unselected consumer's variant; output discovery now traverses actual SDK instance edges. Two SDK graph regressions cover those edges and parallel evaluation (`graph-instance.log`).
- Real failed platform reports were downloaded and checked against GitHub artifact digests. The summary fix processes both original reports into 10,652 bytes instead of exceeding GitHub's 1 MiB summary limit; full 3.6 MB result reports remain (`hosted-41923463340`). Windows native compilation recorded seven preparation consumers, five reused preparations and one consumer launching native processes; this is not a full functional pass.
- After restoring the missing namespace import, the migrated Editor Presentation owner compiled and passed all 99 real cases (`presentation-migration.log`). Required native bindings were generated through the normal product-selected pipeline; no assertion or discovery case was removed.
- Read-only release rehearsal against real failed run 41923463340 rejected the source before deliverable download or any release creation (`actual-release-negative`). Early failures now retain a JSON receipt as well as successful verification receipts. Qualified default-branch positive rehearsal remains outstanding.
- Follow-up closure: CLI build passed with zero warnings/errors; source architecture verified all 245 project owners; documentation structure, selected Validation/Toolchains compiled APIs and affected Validation/Architecture Wiki pages passed (`closure-*`). No source changes followed those checks. Remote acceptance of this follow-up is still required.
- Other retained first failures: missing fixture checkout marker and SDK optional-import condition in graph capture; uninitialized submodule borrowing parent identity; semantic inheritdoc check depending on old DLLs; nine Editor settings fixtures requiring actual logging-panel discovery; escaped solution restore list interpreted as one path. Corrections retained required checks/assertions.

## Performance evidence and limits

Same machine/SDK samples; cold and warm are separate. These historical workloads precede the latest identity hardening and are not final target claims.

| Workload | Raw seconds | Median / limitation |
|---|---|---|
| Old per-project graph, 241 SDK starts | 164.9761196, 139.2029125 | Only two completed samples; five-sample before comparison outstanding |
| Editor Graph + Interactions, build and 58 tests | 17.8616954, 17.7969079, 17.5690239, 17.9237441, 18.0319758 | 17.8616954; zero native; matched before pending |
| Earlier full bounded Input plan | 60.2114318, 60.2863779, 59.7630361, 60.6245840, 59.8384172 | 60.2114318; still included subsequently removed redundant API/Wiki checks |
| Earlier exact local evidence reuse | 25.1801216 | One sample, before current environment/dependency hashing |
| SDL/RmlUi warm build and tests | 297.6490421 | One sample; build 288.38; second consumer reused preparation, zero native compiler/binding batches |
| Content identity positive / negative / restored | 149.8709854 / 125.9299483 / 124.4569868 | Functional experiments, not warm medians; negative intentionally fails |
| Current Input plan, build and 18 executed tests | 59.7370494, 61.6675461, 61.0067472, 61.4504776, 60.9168456 | 61.0067472; no evidence reuse and zero native preparation; `instances-warm-samples.json` |
| Source guard before shared parsing | 15.2999308, 12.8455759, 12.5374201, 13.5397644, 12.9118925 | 12.9118925; tool `b8ce42dc`, fixed source snapshot `8674f042` |
| Same source guard after shared parsing | 11.7006008, 11.1743260, 11.5339936, 11.0259891, 12.0639953 | 11.5339936; same source/SDK/machine/command, 10.67% lower median; `source-shared-syntax-samples.json` |

Native freshness hashing still contributes substantial cost. Repeated equivalent graph payloads reached 37.6 MB for a managed plan and 66.2 MB for full planning. Equivalent payload compaction reduced the managed plan to 30.8 MB; preserving actual instance edges adds about 3.1 MB. Removing an unnecessary outer evaluation lock reduced one graph phase from 45.88 to 21.48 seconds (including a cold evaluator); the first warm phase was 17.60 seconds. The current five-sample bounded managed workload meets two minutes; the 30-second ordinary feedback target and matched before baselines remain outstanding. Performance targets cannot be claimed by omitting checks.

## Remaining acceptance and external dependencies

1. Finish syntax-sharing source/document closure, commit the independently validated follow-up and verify complete owner metadata against the final matrix.
2. Measure current plan stages and five matched warm samples; finish before comparison and optimize measured bottlenecks until targets are met or an explicit measured blocker remains.
3. Complete actual Windows/macOS CI, full product/Release/reload/Support Pack/AOT and cancellation retention evidence; preserve first failed runs.
4. Finish read-only downstream consumption and cache interruption/large inventory acceptance. Do not modify other sessions' neighboring worktrees.
5. Required branch enforcement is active and verified; obtain a successful actual `Engine validation` run before considering the matrix complete.
6. A valid default-branch release rehearsal needs adopted workflow plus a qualified default-branch run. Source/negative contracts are not that rehearsal. Do not merge this PR or publish to manufacture proof.

Next: finish syntax-sharing source/document closure while the corrected real hosted matrix executes; measure retained-evidence/docs feedback and remaining matched before baselines. Keep #75 open until all applicable migration and evidence requirements are satisfied.
