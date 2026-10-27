# Editor UI closure execution ledger

[Original-plan review](acceptance/EDITOR_UI_PLAN_REVIEW_2026_10_27.md) · [Parent issue #27](https://github.com/Wolfiora/InnoEngine/issues/27) · [PR #26](https://github.com/Wolfiora/InnoEngine/pull/26)

Baseline: `30d18489fbf68763229cd8ed35a7005f543019ad`. Evidence owner: `D:/InnoEngine-Verification/editor-ui-closure-2026-10-27`. The original UI plan's hash recorded by the review is `a55f25b76db09befd6cdd2db06a607d0e2a70d660dca6ca5429c64f3d5623646`. The desktop file now contains the approved issue-driven closure plan, SHA-256 `cbb7d50a26a08e4a0d50c7e0f9aabe6c8538ae3fbe229957d2094a4b60325d9e`; these are different documents.

Every work item below is a real GitHub issue created before implementation. Issues remain open after branch verification. Only completely verified items receive an individual `Fixes #N` in PR #26; they close on merge. The broader #18 uses `Refs #18`. No automatic merge is authorized.

## Issue execution map

| Work | Issue | Owner | State |
| --- | --- | --- | --- |
| E01 scopes and failures | [#28](https://github.com/Wolfiora/InnoEngine/issues/28) | ImGui adapter and concrete presentation | Focused native scopes passed; final graph/lifecycle coverage pending |
| E02 command indexes | [#29](https://github.com/Wolfiora/InnoEngine/issues/29) | Interactions generation catalog | Implemented; focused contracts passed; final retirement/performance pending |
| E03 styles | [#30](https://github.com/Wolfiora/InnoEngine/issues/30) | Shared and domain presentation | Domain styles moved to their actual owners; same-frame style deferral and foreign-context contracts passed; complete physical visual matrix pending |
| E04 keyboard and Console | [#31](https://github.com/Wolfiora/InnoEngine/issues/31) | Selection, controls and Diagnostics views | Focused actual Panel workflows passed; full grid/scale/lifecycle matrix pending |
| E05 graph | [#32](https://github.com/Wolfiora/InnoEngine/issues/32) | Graph/Graph.ImGui and Shader consumer | Neutral nodes, layout and pointer intents implemented; 13 contracts and one native failure-scope test passed; complete workflow/performance pending |
| E06 statistics | [#33](https://github.com/Wolfiora/InnoEngine/issues/33) | EditorStatistics and Stats Panel | Core snapshot and three real native sampling contracts passed; complete lifecycle/performance pending |
| E07 large views | [#34](https://github.com/Wolfiora/InnoEngine/issues/34) | Browser/tree/Console/graph presentation | 10k tree/list and bounded Console presentation implemented; fixed-input paired measurements pending |
| E08 workflows | [#35](https://github.com/Wolfiora/InnoEngine/issues/35) | Existing domain features and History | Focused workflows plus actual FlappyBird Inspector edit/Undo/Redo and Play/stop executed; complete journey matrix pending |
| Actual restore/load scope | [#16](https://github.com/Wolfiora/InnoEngine/issues/16) | Build public SDK tests | Four real SDK/load cases passed, including isolated mismatch rejection and strict collectible retirement; final suite/CI pending |
| Frozen framework inputs | [#19](https://github.com/Wolfiora/InnoEngine/issues/19) | Host/Build capture and Compiler | SDK-owned provider replaces reference-pack selection; 11 frozen SDK/C# 14 contracts passed; final companion projections and CI pending |
| E09 scale/input | [#36](https://github.com/Wolfiora/InnoEngine/issues/36) | Native public-input fixtures | 56 native simulated-metric combinations and selected real FlappyBird input/screenshots executed; final comprehensive matrix pending |
| E10 retirement/reload | [#37](https://github.com/Wolfiora/InnoEngine/issues/37) | Host and generation owners | Actual nine-panel 100-cycle product fixture, saved Scene and Shader cycles, and focused generation tests executed; complete convergence matrix pending |
| E11 performance | [#38](https://github.com/Wolfiora/InnoEngine/issues/38) | Deterministic public-host workloads | Original/current deterministic harnesses prepared; identical current FlappyBird plugin input cannot compile against the original API; full comparison blocked |
| E12 documentation/review | [#39](https://github.com/Wolfiora/InnoEngine/issues/39) | Each changed API and Wiki owner | Design standard added; SDK-based architecture/Wiki gate passed for 241 projects before the final style cleanup; final synchronization pending |
| Current SDK-owned validation | [#41](https://github.com/Wolfiora/InnoEngine/issues/41) | Tooling SDK capture and current public source models | SDK output owner and framework references implemented; 92 focused regressions passed; full policy/documentation audit pending |
| Existing History rejection fix | [#13](https://github.com/Wolfiora/InnoEngine/issues/13) | History | Previously verified; final rerun required |
| Required build regression | [#40](https://github.com/Wolfiora/InnoEngine/issues/40) | SDK/compiler-tool graph and load ownership | Compiler-tool graph fix committed; four SDK contracts passed; full normal build/load matrix pending |

## Original requirement traceability

Each row must ultimately record a final commit, executable command and evidence path. Empty evidence is not a pass. Historical results in the review are background only.

| Requirement | Issues | Required final evidence | State |
| --- | --- | --- | --- |
| Features independent of native UI/Panels; no Panel references | #30, #32, #39 | Evaluated graph and architecture tests | Pending |
| Session-owned state and per-owner retirement | #28, #37 | Independent contexts, failure and close cycles | Pending |
| Frame/window/context/thread native borrows | #28, #36 | Public misuse and recovery tests | Pending |
| Core Events/Identity/Serialization/IO reuse | #28, #35, #39 | Call-chain and lifecycle review | Pending |
| Ordered identity selection, anchor/range/toggle | #31, #35 | Filtered/sorted mouse and keyboard inputs | Pending |
| Deferred identity and Play domain routing | #35, #37 | Play/Edit and generation recovery | Pending |
| Static menu and shortcut generation indexes | #29 | Fresh queries, reuse, conflicts and retirement | Pending |
| Shared Action availability/execute/search/toolbar | #29, #35 | Native and headless entry equivalence | Pending |
| Stable menu order, no empty groups, disabled reasons | #29, #36 | Menu model/input matrix | Pending |
| Non-LIFO rejection preserves transactions | #13, #35 | Commit/Rollback/Dispose and legal completion | Pending |
| Immediate gestures, cancel and independent drags | #35 | Inspector and viewport actual inputs | Pending |
| Atomic batches and Missing barriers | #35, #37 | Partial failure and recovery without stack movement | Pending |
| Granular Scene payload and owner serialization context | #35, #39 | Mutation/payload contract review and regressions | Pending |
| Mixed fields, exact vector axes, duplicate components | #35, #36 | Multi-object/property input tests | Pending |
| Reference select/ping/clear/drop, Missing/read-only | #35, #36 | Shared field interactions and source protection | Pending |
| Three style layers and domain ownership | #30 | References, API and scale tests | Pending |
| Label/input ratio, single scroll owner, bounded combos | #30, #36 | Native geometry and input assertions | Pending |
| Workbench and global workflows | #29, #35 | Commands/settings/export success/failure/cancel | Pending |
| Hierarchy and Browser complete operations | #31, #34, #35 | Multi-select, parent/source edits and rollback | Pending |
| Scene navigation/Gizmo/picking/drawing separation | #35, #39 | Owner review and multi-transform input | Pending |
| Game input/focus/aspect/scale and Play isolation | #35, #36 | Public Host input and Play/Edit journey | Pending |
| Generic graph nodes/hits/box/edges and neutral intents | #32 | Real non-Shader consumer plus Shader regression | Pending |
| Graph document/clipboard/save/history | #32, #35 | Actual graph workflow tests | Pending |
| Settings validation/Apply/Cancel/close | #35 | Domain/presentation failure and reopen tests | Pending |
| Console search/filter/group/detail/copy/keyboard | #31, #34 | Snapshot and native input tests | Pending |
| Stats sampled snapshots and trends | #33 | Frame ordering, reuse, 120-sample/60-second bounds | Pending |
| Export operation ownership/progress/cancel/retry | #28, #35, #37 | Exact owner and Pending retirement protocol | Pending |
| Visible-range submission and revision-driven display | #34 | 10k/2k functional and work-count evidence | Pending |
| New Panel/Action/Drawer/Graph without central switches | #29, #32, #35 | Independent public extension fixtures | Pending |
| Runtime/IDE inputs and C#14 capabilities agree | #19 | Multi SDK/root/pack and language matrix | Pending |
| Actual assets/load identity, output scopes and mismatch | #16 | Default/isolated positive and negative SDK tests | Pending |
| Zoom 75/85/90/100/110/125/150; DPI 100/125/150/200 | #36 | At least 28 combinations and window variants | Pending |
| Unicode/clipboard/focus/occlusion/tree same-frame inputs | #36 | Real context/public input assertions | Pending |
| Same-layout/input/scale baseline/final screenshots | #36 | Matched images; human judgment distinct | Pending |
| 100 cycles per stateful Panel/document kind | #37 | Owner/resources return to expected inventory | Pending |
| 10 reloads, strict ALC barrier and Host shutdown | #37 | Weak monitors, subscriptions, gestures and fault paths | Pending |
| Five-repeat FlappyBird/10k/2k CPU/alloc/GC/median/P95 | #38 | Controlled baseline/final raw measurements | Pending |
| No allocation growth or repeatable >10% regression | #38 | Paired comparison with noise and limits | Pending |
| Windows Debug/Release D3D11/D3D12/Vulkan | #35, #39 | Final revision FlappyBird product journeys | Pending |
| Canvas/Rendering2D development and installed plugins | #19, #35, #39 | Formal export and compiler/Drawer regressions | Pending |
| Mac ARM64 CI; manual device limits explicit | #39 | Final CI plus unexecuted manual disclosure | Pending |
| XML/Wiki/Solution/projects/current design standard | #18, #39 | Semantic review and automated gates | Pending |
| Eleven quality dimensions and deep self-review | #39 | Final evidence-based individual assessment | Pending |

## Delivery constraints

.NET SDK 10.0.402, C# 14 and BGCS 4.1.2 remain fixed. No BGCS/vendor/generated modifications, global/implicit imports, IVT, private reflection, test backdoors, compatibility readers or parallel infrastructure. Author and committer are Aaron Liao <hsuankailiao@gmail.com>, with no co-author. Preserve exact source ownership and clean only proven inactive owned outputs; retain current products and evidence.

## Current evidence, not final acceptance

- Baseline inventory is in `baseline.json` and `device.json` under the evidence owner. All five repositories were initially clean, including BGCS.
- `65d222ae` implements the first scope-safety corrections. `e01/scopes.trx` has 55 passes and `e01/console-scopes-verified.trx` has 5 passes. The initial Console failure fixture omitted its required DiagnosticHub scope; the correction and original log remain recorded.
- `88df3c79` implements generation-owned command indexes and fresh contextual resolution. `e02/commands-statistics.trx` has 129 passes, including new menu/shortcut and Core statistics contracts in the working candidate. This is not the final committed matrix.
- Preliminary focused samples in `baseline/command-statistics.json` and `e02/focused-comparison.json` show menu allocation about 8,680 to 4,240 bytes/op and unchanged-statistics path about 16,792 to 184 bytes/op, including test assertions. Concurrent build activity prevents controlled whole-editor CPU claims.
- `e03/styles.trx` has 22 passes. Full scale, lifetime and plugin acceptance is pending.
- Ordinary Debug and the explicitly owned D: SDK output both encountered a generator CS2012 write conflict. Issue #40 is required to restore reliable repeated validation; no IDE process or global package was changed. Read-only Restart Manager inspection found no persistent current lock; the file has no extra hardlink. Binlog diagnosis remains pending.

- `6a656b67` normalizes SDK-negotiated compiler-tool references across analyzer/runtime/transitive consumers. `e40/compiler-tool-scope-final.trx`: 4 passed; final native/task suites remain required.
- `b7a8924d` commits snapshot reuse and its three Core contracts. `e04/controls-verified.trx`: 40 native control tests passed.
- `e04/console-statistics-verified.trx`: 8 passed, 2 failed. The two Console fixtures used different log call sites while expecting collapse; grouping intentionally includes location. The fixture is corrected to repeat one source call; actual search verification is rerunning. Stats native sampling/window/capacity cases passed.
- The first architecture audit found 27 new XML layout violations and a stale-default-bin public-contract classification. XML layout is corrected; #41 tracks the SDK-owned validation correction before implementation.

- `e04/keyboard-panels-mode.trx`: 14 passed. Actual Hierarchy, Browser list/tree, Console search/copy and Stats Panels use public SDK/native contexts and public domain operations. The tree regression includes non-leaf nodes and verifies that Ctrl+Space does not expand them; an independently focused native overlay button retains keyboard activation. The Console fixture installs an owned clipboard transport through public ImGui PlatformIO callbacks, without modifying the user's OS clipboard.
- Early embedded-Panel fixtures omitted the standard Workbench setting contributions and the Edit-session identity domain. These were corrected to match the explicit host composition; production validation was not relaxed. Asset entry references are re-resolved after initial source reconciliation. All earlier failure logs remain local evidence.
- Iterative native runs used standard SDK test discovery/build plus VSTest against the already prepared native deployment. The final candidate still requires a complete normal SDK build/product run; these iterations are not the final revision matrix.
- `cache-maintenance-preview.log` and `cache-maintenance.log` record the standard cache maintenance entry. It reported 10,071,041,837 logical bytes reclaimed; C: free space increased from about 95.14 to 97.12 GiB. No unknown owner, SDK/NuGet global cache or user content was removed. The large owned MSBuild diagnostic was hash-verified and compressed locally; its private archive is not a publication artifact.


## SDK and presentation implementation progress (2026-10-28)

- PR head `b7a8924d` completed all six remote checks successfully, including actual Windows and macOS native jobs. Uncommitted candidate changes are not covered by that result.
- `e05/panels-current.trx`: 60 passing native cases, including 56 simulated DPI/Zoom/window-size combinations, non-Shader pointer/History and Console search. This is real ImGui Context geometry/input acceptance, not physical-display or GPU visual proof.
- `e05/graph-lifecycle-current.trx`: 18 passing graph contracts, including 100 close/reopen cycles with weak payload collection and 10 consecutive successful or rejected assembly publications through the strict generation barrier.
- `e19/sdk-inputs-current.trx`: seven passing cases after source-owner SDK selection. `e19/apphost-sdk-corrected.trx` verifies a real direct apphost with no DOTNET_HOST_PATH and an unrelated PATH. A later runtime-installation-shape guard still requires the final rerun.
- `source-current.log`: complete SourceDebug solution build passed with zero warnings/errors. Later candidate refinements require the current second build and final rerun.
- The unified `Invoke-EditorUiAcceptance.ps1` uses existing VSTest and isolated execution, rejects empty selected projects, records SDK/candidate state and creates fresh evidence operations. Its first Lifecycle run passed three cases; the expanded Shader lifecycle profile must rerun.

## Root ordering and visible-view work (2026-10-28)

[#42](https://github.com/Wolfiora/InnoEngine/issues/42) was created before the additional Scene owner fix. A real sampling stack identified repeated List.IndexOf inside sibling sorting. Twenty-two public Scene/Transform regressions passed; the root-position index reduced the 10000-root median from approximately 3.7 seconds to 156 ms. A public read-only hierarchyRevision is now justified by the actual flattened-view consumer and never enters persistence.

`e07/large-views-after.json` records five first-round medians around 140 ms for 10000 asset directories and 3.7 seconds for hierarchy. `large-hierarchy-indexed.json` records the next 156 ms result. These are intermediate results with separate fixture identities, not the completed paired E11 matrix. Flat row submission, neutral-ID view reuse and further measurements remain under #34/#38.

Current source implements cached ID/depth rows for Hierarchy and File Browser, iterative traversal, SDK-owned hierarchy invalidation and native clipper submission. Asset table/grid snapshots contain IDs and neutral display values, never cached live AssetFileEntry objects. Fifty-two widget/viewport regressions and six focused native keyboard/Graph workflows passed before the final Browser cache changes; those changes require the next replay.

- `e19/sdk-inputs.trx`: seven passing C# 14, cross-plugin IDE, content invalidation, SDK failure/cancellation and immutable-input cases. A subsequent refinement selects the SDK from the source-owner directory before creating its private evaluation project; the final suite must rerun it.
- Compiler owns `IScriptCompilationEnvironmentProvider` and immutable `ScriptCompilationInputs`; the Build provider maps the existing SDK resolver and Core IO process-owned directories into that boundary. Compiler does not reference Build, MSBuild or platforms. Framework pack scanning was removed from its resolver. IDE projects use the same explicit references and symbols and reject a different SDK.
- `e41/tooling-regressions-corrected.trx`: 92 passing related symbol/Wiki/SDK output-owner cases. Explicit `--artifacts-path` is forwarded to SDK graph and Wiki evaluation; public contract inspection consumes evaluated TargetPath. SDK Wiki framework references no longer come from the validator runtime. Non-SDK inspection policy and the final repository-wide result remain under review.
- `e05/pointer.trx`: 13 neutral Graph geometry, pointer, Missing-port, read-only and cancellation contracts passed. `e05/native-nodes.trx`: a real non-Shader node consumer restores its native clip after an extension exception and culls offscreen callbacks across three frames. Shader now uses the same neutral geometry, hit, selection and pointer mechanisms; its semantics and History application remain in the Shader owner. Full document/clipboard/reload journeys remain pending.
- `e07/widgets-clipping.trx`: 45 related real-native Widget and renderer-capability cases passed. This includes 10000 tree rows, explicit retained clipped rows, clipper callback failure cleanup and both renderer-capability values. Full Panel/scale/retirement checks must still rerun the candidate.

## Large-view baseline and confirmed failure

The first unbounded workload aborted the native host with CRT exit code 3. A diagnostic-only startup hook under the evidence owner redirected CRT assertion reporting to stderr without removing assertions or changing engine/vendor/package code. `e07-crash/hierarchy-assertion.log` records Dear ImGui's 16-bit vertex limit assertion. The diagnostic setup follows [Microsoft's CRT error-output contract](https://learn.microsoft.com/en-us/cpp/c-runtime-library/reference/set-error-mode?view=msvc-170).

Two distinct responsibilities were identified: clipped ordinary tree rows still submitted custom geometry, and the existing SDL/BGFX draw consumers applied command vertex offsets without the context advertising that capability. The selected renderer now reports it through its existing SPI. Headless performance hosts explicitly validate every indexed draw command before claiming that capability; they do not represent GPU or physical-display acceptance.

`e11-baseline/large-views-before.trx` passed both synthetic workloads with five repetitions of ten measured frames, after five warmup frames. The raw samples are in `large-views-before.json`. Median samples were approximately 5.5 seconds/frame and 263 MB/frame for 10000 asset-directory rows, and 3.2 seconds/frame and 58 MB/frame for 10000 hierarchy rows. These are pre-clipping observations, not acceptable final performance. The original pre-refactor comparison, controlled paired runs, other workloads and final product measurements are still required.

The exact pre-clipping native test runtime was copied (not hardlinked) to `e11-baseline/runtime-before`; `runtime-before-manifest.json` verifies all 275 files (70,661,631 bytes) by SHA-256. The measured fixture names/counts are deterministic, but persistent IDs were generated by each authoring owner; a stricter frozen-input paired replay remains required before E11 can close.

Current deep review: display caches retain neutral IDs, geometry and strings; frame callbacks are not cached. Selection and drag sources continue to resolve through Core Identity; History remains authoritative in existing feature controllers. SDK execution reuses the existing process runner and Core IO ownership instead of adding a second resolver or temporary-directory protocol. No BGCS, package, generated binding or third-party source was changed. These findings do not substitute for the outstanding full acceptance matrix.

## Additional verified evidence and corrections (2026-10-28)

- `273c42ad` fixes [#43](https://github.com/Wolfiora/InnoEngine/issues/43) in the AssetFileSystem owner: an unchanged refresh preserves existing Core identities, preparation failure preserves the previous index, and only removed entries retire. The real FlappyBird Browser-empty-after-Play failure and two native regressions exposed the defect. Asset Pipeline has 121 passes; six related native Browser/Scene cases passed. Final real-product replay is still required.
- `e16/loaded-package-shared.trx` has three passes for default output, isolated output without default assets, and isolated output beside deliberately stale default assets. `e16/loaded-package-isolated.trx` has one fresh-host pass. Content-mismatched test copies with the same assembly name/MVID are actually loaded and rejected; full GC/finalizer/full GC confirms their ALC retirement. Original packages remain unchanged.
- `e01/window-geometry.trx` has 49 passes. Copied window/clip bounds and draw-list construction require an active native frame. Native getters retain their write-access side effects; no private native structure is dereferenced before that guard.
- `e19/sdk-contract-final.trx` has 11 passes, including C# 14 Runtime/IDE agreement and cross-plugin extensions. IDE input guards also check implicit configuration defines, because the SDK can otherwise append DEBUG after imports.
- `e05/graph-final.trx` has 26 passes. Shift-box selection preserves its original selection; native generic-node and Shader workflows remain separately tracked.
- Console popup reporting was corrected: requested capacity 1 is normalized to 16. The first attempted eviction test therefore did not evict anything. The corrected test proves the original occurrence is absent before checking popup retirement; both clear and real eviction pass against the existing implementation (`e07/console-retired-real-before.trx`). The speculative production change was withdrawn. Retained-window diagnostics now report the actual record count.
- The original `b303d9f1` production source builds unchanged. Test-only harness adaptations supply equivalent public fixtures; `e11/preflight-original-hierarchy-3.trx` and `preflight-original-assets.trx` pass with shared frozen input. Their concurrent-build timings are preparation evidence, not controlled final performance results.
- `e41/current-wiki-refresh.log` reports SDK ownership for 241/241 projects and a passing architecture/Wiki gate. API pages were then refreshed from that inventory; the final changed state still requires its gate.
- `source-final.log/binlog` records a SourceDebug solution build with zero compile errors before the remaining style ownership cleanup. `contracts-interrupted.json` explicitly invalidates an incomplete no-build Contracts run: native deployment had not been prepared, and a subsequent overlapping discovery rebuild encountered loaded-output locks. Both owned operations were stopped. The unified build now includes native test preparation and stages test temporary directories under its result owner; this does not change production file-lock protocols.

No item above closes its issue by itself. The PR remains Draft until the complete applicable matrix passes on the final candidate.


## Candidate verification update (2026-10-28 06:22 +08:00)

- Unified entry: Workflows 297 passed, Visual 135 passed, Lifecycle 5 passed. These profiles report selected automated contracts, not the entire issue matrix.
- The final D3D12 FlappyBird acceptance copy verified actual Play/stop, the recovered asset Browser after refresh, Settings edit/cancel/reopen, and normal resource retirement (exit 0). Screenshots: `e09/flappybird-final-play-stop-browser.png` and `e09/flappybird-final-settings-cancel-reopen.png`.
- `e03/all-native-frame-contracts.trx`: all 72 ImGui contracts passed. Production presentation now has an explicit pre-frame callback and owned failure retirement; Session metrics commit before frame creation.
- `e16/loaded-package-shared.trx`: 3 passed; the isolated loaded-package mismatch contract also passed. Negative content belongs only to test-owned copies, never global NuGet or BGCS files.
- The first full Contracts attempt was interrupted and is invalid: a test output was missing its native deployment and an overlapping test compilation encountered the live testhost's file handle. The unified builder now supplies the actual test product selection. The diagnostic dump was retired after preserving its text analysis; no production assertion, timeout or file-lock protocol was relaxed.
- The Console eviction premise was corrected to its actual minimum capacity. Both real clear/evict cases pass against the existing focus logic; a speculative production focus change was withdrawn.
