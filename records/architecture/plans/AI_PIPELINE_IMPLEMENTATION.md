# AI pipeline implementation — issue #75

Status: in progress, approximately 80%; no complete hosted certification yet. [Issue #75](https://github.com/Wolfiora/InnoEngine/issues/75) and [draft PR #76](https://github.com/Wolfiora/InnoEngine/pull/76) remain open.

## Scope and fixed decisions

- Work only in InnoEngine on `ai-pipeline`, starting at `7f56dd64b`. Adjacent repositories, vendor sources and generated bindings remain untouched.
- Preserve architecture, public contracts, Identity/Missing/reload, serialization, ownership and actual native/lifecycle assertions. No production test hooks, IVT, compatibility layer or reduced required acceptance.
- The existing Build CLI owns `validate plan/run/full/evidence`. Actual SDK graphs own project/target/input selection. Stable registered output owners and independent consumer leases remain authoritative.
- Hosted Windows/macOS; daily changed-commit functional certification, weekly cold/ten-warm/long-running checks, verified immutable transfer and manual draft-only releases. No merge or formal publication is authorized.
- Local SDK: `C:/Users/23842/.dotnet/dotnet.exe`, 10.0.402. Build tool output: `artifacts/validation/build`; reusable validation output: `artifacts/validation/workspaces/Debug`; prepared native output: `artifacts/validation/native`. Logs use separate run directories.
- Prior detailed evidence/checkpoint history remains in this file at commit `98b51feb`; raw diagnostics remain under `artifacts/validation`. This checkpoint keeps current decisions and evidence instead of appending a chronological work transcript.

## Implemented coverage and retired mechanisms

- 241 preserved rule mappings in `docs/architecture/rules/coverage.json`; root AGENTS is 3,275 bytes. Root-start discovery, links and byte budgets pass. Current verification-tool instruction set is 8,559 bytes. Four repository skills use the unified plan and canonical rules.
- Source architecture covers all 246 evaluated projects. All 63 test owners declare execution requirements: 17 domain, 33 contract, 4 integration, 8 native, 1 performance. Product/static evidence is represented by actual plan checks. `owner-evidence-coverage.json` records the complete inventory; its native flag is a direct declaration, not a transitive-closure claim.
- Input/UI domain owners are independent of SDL/RmlUi. Editor Graph and Interactions remain domain owners; real presentation/lifecycle cases retain backend prerequisites. Architecture guards traverse actual domain dependencies and reject native preparation.
- Removed `Invoke-ManagedChecks.ps1`, `Invoke-EditorUiAcceptance.ps1`, `Invoke-WikiChecks.ps1`, `Select-ValidationScope.ps1`, `Test-ValidationScope.ps1`, `New-ValidationSolutionFilter.ps1`, duplicate full-repository xUnit policy/Wiki execution and the duplicate repository NuGet submission workflow. Isolated execution, product journeys and performance sampling keep distinct implementations called by the plan.
- Dependency submission ownership was compared using actual automatic run 41871525488 and custom run 41326947632. Platform automatic submission remains responsible; Windows/macOS/configuration/NativeAOT restore checks remain in Engine CI.
- Ruleset 13788396 actively requires `Engine validation`, strict base freshness and existing review/discussion requirements. No merge queue was enabled. Default-branch workflow adoption still requires the user's merge decision.

## Current changes and verified behavior

Current hosted input: `7fa6018c` (includes the finalization-evidence and combined API/Wiki reference-scope corrections). PR run 41995632303 and explicit extended run 41995973925 are executing on both hosts; neither is certified yet.

- Selection includes merge-base/worktree changes, actual SDK property instances, imports, generated/dynamic inputs, submodule/link bytes and same-size/same-time mutations. Unknown ownership, empty discovery, unavailable requirements and skipped required execution fail closed.
- Exact SDK unions separate managed/native/Release outputs. Clean completes before Build; bootstrap claims are shared only inside their owning process. Managed compilation is bounded to four workers; native preparation remains serial.
- Per-owner TRX/discovery reconciliation retains successful owners even if another fails, while preserving the union failure. Every declared isolated contract needs a receipt. The shared executor is a direct test-evidence input without entering native recipe keys.
- Local reuse requires matching source/test/verifier/environment/package/output identities. Closing source and environment snapshots overlap independent reads; all current bytes are still read. CI never inherits a different source tree's green status.
- Native requests reuse frozen immutable preparation within their Build lifetime; consumers independently protect deployment inputs. Transfer imports verify inventories/hashes, cache inventory resumes completed shards, and retirement protects dependencies/readers. First failures and cancellation outputs are retained.
- Finalization defect reproduced through the public entry: a real instruction-check handshake locks an external import, causing closing reads to fail after every check passed; the old evidence operation incorrectly accepted the intermediate results. Writers/readers now require `completed=true` only after complete closing verification and owner recording. Missing/false/malformed completion cannot certify or supply local reuse. Original failures: `completion-before*`; corrected public integration: `completion-final`.
- A second actual finalization defect is now corrected: compiling changed input before closing verification published a reusable compilation identity too early. Restoring the original bytes/timestamp then executed the wrong assembly. `compilation-finalization-before` reproduces the failed repair. Compilation records now publish only after stable closing source/environment/output verification. The expanded five-cycle public-entry regression passes in 11m11s: warm preparation, in-run mutation, repaired compilation, owner-only compilation with unchanged dependency bytes/write time, and same-size/same-time dependency invalidation on a new Git baseline.
- Managed Clean can retain dependency outputs only when complete content proofs differ solely in an owner's exclusive SDK Compile inputs. Shared inputs, imports, resources, generators, SDK/environment/package changes and missing/damaged outputs retain recursive cleaning. Native cleaning is unchanged. `compilation-ownership-source` and `compilation-ownership-docs` pass; CLI compilation passes with zero warnings/errors (205.87s). Updated bounded timings remain to be measured.
- API metadata resolves the audited owner's actual SDK references. Full architecture plus selected Wiki pages retains full architecture reference scope; Wiki-only fixtures explicitly request their independent operation. All original Wiki assertions remain. `wiki-api-before` reproduces the missing-owner failure; `wiki-api-final` passes 95 symbol/output/Wiki cases. Current CI aggregation passes 18 cases.

## Local evidence index

All paths below are relative to `artifacts/validation`; source identities and raw samples are retained with the artifacts.

| Evidence | Result / location |
|---|---|
| Domain/backend separation | Input 17, UI 1, Editor domain 58; SDL/RmlUi 22; Presentation 99. Domain runs have zero native preparation. `native-migration-*-2`, `presentation-migration.log`, current feedback samples |
| Content mutation and skips | Actual same-size/same-time sentinel assertion fails, exact restoration passes; ordinary Skip rejected. `content-inputs-*`, `tracked-content-*`, `skipped-test-*` in the owned integration fixture |
| Per-owner test receipts | Four real SDK/test cycles pass: warm restore, mixed failure, repaired local reuse, deliberately omitted isolated receipt. `owner-evidence-restored` (9m15s); initial restore-identity failures preserved |
| Closing freshness | Four source/environment/evidence cases pass; closing failure plus repaired positive run passes. `closing-freshness`, `completion-final`, `completion-failed-owner` |
| API reference closure | 63 initial symbol/output cases; 95 after combined Wiki scope correction. Actual API inspection passes 30 prepared native owners; 25 other native owners lack local outputs and require hosted acceptance. `api-closure-*`, `wiki-api-final` |
| Ownership and caches | Actual SDK process-lifetime/Editor Clean cases, native preparation/import cases, active-reader protection, corruption/path rejection and resumed 64-shard inventory. `bootstrap-process-lifetime`, `native-fixture-physical-owners`, `inventory-interruption` |
| Cancellation | Actual build cancellation retains every planned result and its cancellation receipt and drains owned children. Intermediate finalization records now also fail certification |
| Consumers, read-only | Pinned Canvas/Rendering2D and FlappyBird/NeonZero script and IDE paths pass; Rendering2D 42 tests. Dirty neighboring worktrees retain their original changes; their unfinished APIs are not imported |
| Release rejection | Actual failed hosted run rejected before artifact promotion/remote release creation. `actual-release-negative`; positive default-branch rehearsal remains external |

## Performance and limits

Same machine, SDK 10.0.402. Fresh and warm samples are separate. Current bounded workloads change test-source comments, force the selected real build, and execute unchanged assertions; they are not general production-feature latency claims.

| Workload | Warm samples (seconds) | Median |
|---|---|---|
| Current documentation, `7fa6018c` | 28.4549259, 28.3953405, 28.0210578, 28.4789563, 27.7113554 | 28.3953405; fresh 44.9066603; no build/native; finalized receipts |
| Current Input, `98b51feb` | 110.9927312, 115.4725809, 113.0319976, 114.2631887, 120.6638332 | 114.2631887; fresh 115.9639259; all 17 cases each run |
| Current Editor Graph/Interactions, `98b51feb` | 129.2405153, 135.8382999, 134.2540366, 142.1013194, 131.5891378 | 134.2540366; fresh 132.2376108; all 58 cases each run |
| Native serial public producers | 100.2590921, 100.1234052, 183.0382771, 100.0830656, 101.1956443 | 100.2590921 |
| Native batched product | 85.3230356, 86.648996, 85.5369846, 85.147525, 85.2593032 | 85.3230356 |

Native six-case acceptance passes in 17.7893 minutes. Every serial sample reads 14,012,345,529 bytes; batch reads 4,498,462,318 (67.896% fewer), with zero native processes/binding generations and one batch native-product-lock phase. Raw evidence: `native-performance-matched*`. This is the same public-producer workload, not a whole-pipeline commit-to-commit claim.

Documentation raw series: `docs-finalized-evidence-samples.json`; the preceding 27.9552066-second median remains in `docs-closing-parallel-samples.json`. Prior initialized median 53.4944495 and the later missed target 31.3512644 remain in `docs-initialized-serial-samples.json` and `docs-final-identity-samples.json`. Fixed 245-owner graph comparison measures old per-project query median 128.4305531 versus batch 18.0282207; fixed source-policy median 12.9118925 versus 11.5339936. Earlier uninitialized Input results are not a paired baseline for current initialized feedback.

Current medians meet 30 seconds / two minutes / five minutes, but one Input warm sample exceeds two minutes by 0.664 seconds. Its selected Clean/Build takes 74.8628125 seconds versus 65.7978287–70.0151069 in the other four samples; assertion execution remains 5.4830671 seconds. Full selected recompilation is the measured bottleneck, not native preparation or test discovery. Retaining integrity while reducing unnecessary recompilation remains a follow-up. No sample is discarded. Remaining paired baselines and hosted cold/ten-warm/long-running acceptance are not claimed complete. The existing isolated fixture has no initialized native submodules; using it for the old coupled domain workload requires a separately budgeted baseline preparation, not relabeling current native-free timings as a paired comparison.

The original `7f56dd64b` Input dependency closure has now been measured in the existing isolated fixture, with the same SDK and all 17 current domain assertions matched by actual TRX names. Cold build: 359.8499171s. Five warm build/assertion totals: 50.2559835, 50.0658199, 50.3611405, 50.4767691, 50.1021274s (median 50.2559835s); corresponding build times: 48.346793, 48.2853314, 48.5766677, 48.6905187, 48.3164317s. This excludes policy/planning/discovery overhead and the migrated SDL lifecycle assertion; it is not an end-to-end speedup claim. It exposed slower recursive compilation in the prior new pipeline (65.80–74.86s), motivating the content-proven dependency retention above. Raw data: `input-original-warm-samples.json`, `input-original-baseline-cold-build.json`. The fixture's original commit was restored and verified; the local SDL source clone stayed isolated from the main checkout.

## Hosted state and next actions

- Runs 41923463340, 41936778114, 41948692532, 41956130789 and 41969543399 retain the first namespace/output, solution, Clean, physical-path and symlink failures; corrections preserve assertions.
- Run 41976978174 passes managed acceptance on both hosts, then fails native selected API resolution. Windows cache restore/reimport of 15 immutable candidates is confirmed. SHA-verified diagnostics: `ci-sixth-windows` and prior hosted directories.
- PR run 41987990053 and explicit extended run 41988085204 both fail the same Wiki fixture scope regression during managed acceptance. The 95-case correction above is local, not yet hosted. Extended cold/ten-warm/long-running work therefore remains unexecuted; original reports are retained (`ci-seventh-windows`).
- Current follow-up closure passes: 95 architecture/Wiki/symbol cases, three completion/freshness cases plus the completed-failure receipt assertion, 18 CI report cases, 246-owner source/documentation/rule checks, and CLI compilation with zero warnings/errors (33.60s). Corrections are pushed as `7fa6018c`; PR 41995632303 and full extended 41995973925 are executing. Next: inspect their first failures; do not rerun unchanged failed inputs. A current search of active skills, workflows, docs, tests, Build and tooling finds no references to the six retired entrypoints.
- Required before epic closure: successful actual Windows/macOS functional/product/Release/reload/Support Pack/NativeAOT and extended matrices; complete owner/retirement/rule mapping closure; remaining performance gaps; positive draft-release rehearsal from a qualified default-branch run. Adoption/positive release rehearsal require the user's merge decision; no merge or publication is implied.

## Disk and continuation

Catalog retirement reclaimed 6,263,871,751 logical bytes. Closed evidence compression subsequently released about 3.3 GiB, then another 1.20 GiB (`closing-evidence-compression.json`), then 297,082,880 bytes from six completed documentation runs (`finalized-evidence-compression.json`). Current C: free space is about 29.4 GiB. Preserve active reusable outputs and first failures. Cache deletion must use registered owner/lease retirement, not guessed paths; no adjacent cleanup is authorized.

Completed baseline outputs were explicitly registered under their own non-overlapping owner and retired through the existing lease protocol: another 1,802,502,305 logical bytes reclaimed (`original-baseline-cache-retirement-corrected.log`). The initially rejected overlapping registration and its diagnostics are retained; only that newly created registration was corrected under the catalog lock. No manual cache payload deletion was used. `input-original-baseline-owner.json` records restoration and retirement.

Keep future continuation updates to scope, decisions, current identity, valid evidence, blockers and next action. Do not recreate the retired selectors or duplicate full scans/builds merely for commit, push, report changes or session resumption.
