# Editor UI owner and acceptance continuation

[Execution ledger](../EDITOR_UI_CLOSURE_PLAN.md) · [Parent #27](https://github.com/Wolfiora/InnoEngine/issues/27) · [PR #26](https://github.com/Wolfiora/InnoEngine/pull/26)

This is a scoped continuation record, not whole-plan completion. Evidence root: `D:/InnoEngine-Verification/editor-ui-closure-2026-10-27`. SDK 10.0.402 / .NET 10 / C# 14 / BGCS 4.1.2 remain unchanged.

## Implemented owners

- `e3f02ebe`, `23f0ceb2`, `397ae3ed`: the existing SDL/ImGui backend owns one content-validated, pinned CJK font source per Context; four text faces borrow it until native destruction. Successful Context retirement unregisters the exact application owner. Pending/failure preserves ownership; application shutdown freezes its set and rejects new Context publication. The upstream font/license bytes are preserved, without OS font discovery, a font manager, BGCS or generated-binding changes.
- `8fd31cee`: Scene Workspace preserves an assigned persistent source identity, path, content and saved baseline when Asset Pipeline reconciliation marks the source unavailable. Ordinary Save refuses that unavailable assignment; explicit SaveToDirectory can establish a different source. Recovery, rename, Save and reopen use the existing asset/document protocols.
- `6277db99`: GameObject borrows the existing Core-indexed SceneObjectRecord for owner-local liveness checks. Add/remove/Clear/transfer preserve exact-owner attachment. Core scalar captures avoid repeated ordinal lookup while retaining duplicate rejection, canonical bytes and converter execution. No new identity table, event bus, serialization format, public API or compatibility path is added.

## Ordinary SDK results

| Gate | Result | Evidence / precise scope |
| --- | --- | --- |
| Complete neutral Release solution | Passed | `candidate-next/Contracts/7e2833f5448346a09253da45e91aa2cc/Contracts/source-build.log`; zero warnings/errors, before the final scalar/attachment refinements |
| Native glyph/context/retirement | 12 passed | `candidate-next/native-owner/fonts-context-selected-sdk.trx`; actual SDK output, including copied missing/corrupt font input and ten-context strict collection |
| Core serialization | 75 passed | `candidate-next/native-owner/core-scene-assignment.trx`; includes reordered canonical scalar keys and duplicate rejection |
| Scene | 53 passed | Same TRX; includes destruction, hierarchy transfers, restore and collectible recovery |
| Assigned missing Scene source | 3 passed | Same TRX; two clean/edited recovery cases and explicit SaveToDirectory without recreating the missing original |
| Expanded automated Visual | 524 passed | `candidate-next/Visual/76a7afdc3abf477d9a4521f651ce10be/summary.json`; actual native style preparation with independent zoom/density/size inputs, Console/Shader/input coverage; profiles overlap |
| Complete Contracts | Failed; corrections/reverification in progress | The initial run lacked a selected Player compile closure and an explicit partial-type XML summary. Both corrections are implemented. A later test-owned DLL cleanup failure remains under #16 investigation. Preserve the failed run, not a success claim. |

The first direct native command used the machine's older default dotnet for child processes and failed two cases. The official isolation entry already injects the selected SDK; the corrected direct command follows that same boundary and passes all twelve. Exploratory pre-reconciliation Missing assertions were invalid: ID loading intentionally retains last-good content until Asset Pipeline observes removal. The retained failed TRX and corrected public-boundary tests distinguish that premise from the actual #50 fix.

## Acceptance orchestration

The UI Contracts entry first prepares the neutral source graph and SDK-evaluated products for its declared target, then the selected test graph. It does not reuse an Editor/neutral DLL as a Player reference or relax Wiki checks. CI moves its existing selected-Player preparation before dependent architecture/tests; no duplicate product table is added. YAML parsing and prerequisite order/uniqueness checks pass.

Actual product acceptance reuses the existing journey runner for Workflows and Lifecycle. The latter requires every actual Panel to complete 100 cycles, eleven full-GC observations and at most 4 MiB final live-memory growth. These requirements precede measurement; the new integrated product run is pending. Unattended windows are hidden. This does not certify physical monitors or manual visual quality.

## Deep review and remaining sequence

The execution-chain review preserves current owners, Core reuse, SDK output isolation, strict generation barriers, rollback, Missing state and failure diagnostics. Attachment backlinks and font completion callbacks do not create another authority; neither persists plugin instances or generation delegates. No BGCS, package, third-party source or generated binding is modified.

1. Resolve the #16 cleanup observation without weakening context/assembly retirement or altering restored packages; complete corrected Contracts.
2. Freeze and verify all current source/workflow/visual/lifecycle gates and actual FlappyBird Debug/Release/API journeys and panel loops.
3. Complete #46 and controlled original/current performance. Earlier five independent due-inspection processes remain 41–43 ms and about 8.37 MB. Later 37 ms complete-inspection and 1.35–1.87 ms metadata-only prototype diagnostics are not final paired acceptance and do not pass the absolute latency budget.
4. Finish matching visual evidence and all original-plan rows, update each issue's actual checklist/evidence, then produce the eleven-dimension final assessment and exact-head CI result.

PR #26 and parent #27 remain incomplete while these gates are open. No physical Mac/manual display claim is made. Previously rejected cleanup of four diagnostic runtime copies remains unperformed; automatic approval review returned `blocked by policy` without a detailed reason. User/SDK/NuGet global data is untouched.


## Exact-candidate continuation

At `564e00d0`, ordinary Release Workflows passed 699 and Lifecycle passed 15. Debug and Release official Editor builds succeeded; each reported that automatic cache maintenance exceeded its five-second budget and retired no artifact. This warning is retained, not reported as a zero-warning product build.

The complete Contracts source build failed on a live `Vertex.ishadersource.imeta.staging-<operation>` file. New issue #52 fixes actual source ownership: Core IO recognizes only its reserved candidate naming; source policy reuses it; shader build captures permanent sources and import metadata, hashes the same neutral declarations used by the Task, copies into a `ProcessOwnedDirectory`, and imports only the copy. Complete source inventory/content is verified before publication. Generic Native hashing still includes explicitly declared candidate-shaped files.

Initial ordinary-SDK #52 checks: shader input cases 7 passed, Core IO suite 37 passed, build zero warnings/errors. Complete concurrent build and final Wiki acceptance remain pending. Related test-entry review also fixes a local product-evaluation variable overwriting its test solution selection.

The strengthened #16 identity regression passed ten normal/isolated groups (40 executions), observing both loaded Assembly and collectible Context retirement. One historical test-copy deletion denial remains unexplained and preserved; no cleanup suppression, increased timeout or retry is added.

A diagnostic catalog-only assignment prototype separately measures the existing `TryGetInfo` boundary for document path/status instead of importing source contents during a label query. It remains outside production pending semantic tests and controlled measurements. Its noisy complete inspection median is 31.24 ms, with an initial 123 ms P95; it does not meet #46 and is not closure evidence.


### Shader input boundary refinement

The frozen shader identity includes permanent directory inventory as well as source/import-setting bytes. Empty authored directories are preserved; transient atomic file names and derived artifacts remain excluded. The Task consumes the backend-owned frozen fingerprint, not its physical file list. Core IO candidate-name recognition does not authorize deletion and general Native input hashing remains unchanged.

The compiler now borrows an explicit `ProcessOwnedDirectory` supplied by actual callers. Standard distribution registers `artifacts/build-tools/shaders/requests`; Task/CLI retain the directory through completion, and existing owner-exclusive maintenance retires absent payload claims and leaf locks. There is no new unmanaged global temporary owner. Workspace retirement precedes authoritative Task publication.

Nine updated focused cases pass using actual SDK compilation plus an evidence-owned copy of the already built native test runtime (`e52/frozen-input-workspace.trx`); this is distinct from a complete final normal build. Normal shader-library build and focused test compilation report no warnings/errors. Initial full workspace build passed with four xUnit blocking-operation warnings; the tests now use async/await, and the frozen compilation has no such warnings. The earlier eight-case run and all initial failures are retained.

The first simultaneous source/Editor experiment is not a concurrency pass: source build passed, while Debug correctly rejected changed `bgfx-tools` inputs after the Core IO source was edited during the build. `NativeBuildRecipe.Create` explicitly includes `AtomicFile.cs`; the strict post-wait validation is retained. Re-run with a complete frozen source inventory. Cache maintenance inspected 29,060,240,489 logical bytes, protected 5,345,891,157, reclaimed zero under the current budget and skipped 441; no unknown data was deleted. C free space subsequently measured 83.27 GiB, D 29.66 GiB.

The Contracts source prerequisite now selects the existing `SourceDebug` / `SourceRelease` Solution configuration, preserving a neutral graph and preparing selected product references separately. Product evaluation no longer overwrites the test-solution path. Final SDK/contract and Wiki checks remain required.

### Frozen 329568ba verification and lifecycle preparation

- Ordinary Editor Debug and Release builds passed with zero errors. Each reports the existing automatic cache-maintenance budget deferral warning; no artifact was retired after the budget expired.
- Two independent output owners completed while the exact test-created metadata candidate remained exclusively open. The source inventory contains ten permanent files before and after, with zero path/hash differences (`e52/frozen-authored-comparison.json`). The guard released and removed its exact candidate in `finally`; it did not modify authored metadata or authorize cleanup of unrelated candidates.
- Complete Contracts executed 816 passing cases, one failure and two platform workspace-link fixture skips. The single failure is a Wiki content-boundary violation in `CURRENT_ISSUES.md`, not a shader/compiler/build failure. Its external consumer name has been replaced with the engine-owned description; the validator remains unchanged. The complete focused Wiki rerun is recorded under `e52/wiki-corrected` and is pending at this checkpoint.
- The initial actual Debug/D3D11 product completed its eight workflows and all nine panels' 100 cycles, but retained 4,488,688 bytes against a 4 MiB budget. That run remains Failed. Public diagnostic heap observations identify initial asset/render preparation followed by a plateau; the tool warns that .NET 10 uses lossy heap-event buffering, so heap counts are diagnostic rather than complete ownership proof.
- A separately prepared diagnostic drew all panels for 120 frames before its baseline and retained 712,056 bytes over the subsequent 100 cycles. The ordinary fixture now records both cold preparation observations in `panel-lifecycle-preparation.tsv`, then runs the existing eleven full-GC observations and unchanged limit. Pending events must also return to zero. Fresh ordinary Debug/D3D11 and Debug/D3D12 product runs passed under `candidate-panel-prepared`; other configuration/API cases remain pending.
- The public presentation SPI produces actual native UI geometry and available CPU atlas pixels after rendering, without desktop control. Initial captures were rejected for stale readiness/initial GPU preparation. A later capture verifies both viewport image commands; software replay marks those two unavailable GPU textures explicitly. This is UI geometry/atlas evidence, not a physical screenshot or GPU-content claim. The matching original producer is being captured independently.
- #46 experiments, including metadata-only source queries, topology-owned capture maps, primitive-field comparison and explicit state revisions, remain outside production. Reduced allocation or a best timing does not meet the absolute latency gate. No new serialization/domain API is committed on the strength of these experiments.

The deep review still finds outstanding quantitative and original-plan evidence. Parent #27 and PR #26 remain incomplete; none of the results above is whole-plan closure. No BGCS, package, generated binding or vendor source was changed. Rejected cleanup of four earlier owned runtime copies remains unperformed; no alternate deletion route was used.
