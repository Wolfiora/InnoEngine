# Editor UI acceptance — 2026-10-27

[Current acceptance](../../../docs/architecture/CURRENT_ACCEPTANCE.md) · [Architecture](../../../docs/architecture/EDITOR_UI_ARCHITECTURE.md) · [Implementation ledger](../EDITOR_UI_IMPLEMENTATION_PLAN.md)

## Scope and conclusion

This report covers the approved Editor UI refactor on `editor-ui`, starting at `b303d9f10cbaf5a905f7cb529e52cf8293b0d8bd`, together with the Rendering2D authoring caller updates and the officially exported FlappyBird package. It does not certify the entire engine, every platform or every interaction as perfect.

The implemented subset passed its local correctness checks, including the final SDK Wiki gate. This is not completion of every requirement in the approved plan. The [plan-by-plan review](EDITOR_UI_PLAN_REVIEW_2026_10_27.md) supersedes the earlier completion assessment: menu indexing, generic graph presentation, Console/Stats workflows and the complete scale/lifecycle acceptance matrix remain incomplete. The results below retain their original scope.

Remote CI subsequently passed all six checks at `2dc37114b7c9b5137e866efc057a66e0dfd72046`. The review records exact Windows/macOS outcomes and distinguishes those passes from plan requirements that CI does not exercise. PR #26 remains a draft pending that work.

## Implemented boundaries and behavior

- Headless Assets, Shaders, Rendering, Scripting and Exporting features own domain operations, drafts, compilation and export lifetime. ImGui presentation owns windows, controls and transient state. Panels do not reference other Panels.
- Reusable asset editing and its neutral History protocols belong to `Inno.Editor.Assets`; Browser presentation belongs to `Inno.Editor.Assets.ImGui`. Shader presentation belongs to `Inno.Editor.Shaders.ImGui`. Generic graph coordinates, curves and hit tests belong to `Inno.Editor.Graph.ImGui`.
- `EditorUiSession` owns controls, styles and per-owner transient data. Native pointers, generated binding types and native option enums stay inside the ImGui adapter and product hosting boundary. Draw-list borrows reject use outside their frame/window/context.
- Continuous input uses `EditorEditGesture` over the existing History transaction. Each sample records immediately. Release commits, Escape rolls back, and commands or document operations finish the gesture before their own transaction. Failed completion preserves ownership.
- Ordered selection resolves Core identities, retains Missing references and supports anchors/ranges/toggles. Batch asset and scene edits preflight the whole selection and use the existing atomic History protocols. Mixed vector edits affect only the selected scalar component.
- Menus, shortcuts, toolbars and command search use the same Action availability and execution path. Deferred targets resolve Core identities instead of retaining live scene objects.
- Temporary Play identity domains have a Host routing lease over the existing Core allocator. Selection, focus, queued actions and drag/drop use the same routing; Edit selection is restored before retiring the domain. No second object registry was created.
- Play Mode notifications and settings/UI lifecycle notifications use Core Events with explicit subscription ownership. No parallel event dispatcher implementation, Undo stack, object identity registry or persistence format was added.
- Rendering2D uses the current session and edit contract. Its package in FlappyBird was produced by the official exporter, never edited as an archive.

- Pure identifier formatting moved out of session-bound widgets into `EditorDisplayNames` in Editor Core. Menu metadata can be queried without a native UI context. The old widget member was removed and every engine/plugin caller updated.
- Non-identity targets retain only known neutral scalars strongly. Boxed host structs can contain extension objects, so selection, focus and snapshots hold those targets weakly. A failing GC regression reproduced the retention before the correction; the complete 121-case Interactions suite passed afterward.
- The script SDK references reusable Assets, Shaders and Graph API owners directly. Hierarchy and Inspector remain direct SDK references for their own four public drop-target contracts. A fresh SDK output and a real EditorScripts probe verify these exports; no Panel is used only to carry another module's API.

The full current file tree and execution flow are in the architecture document. Solution registration, project reference visibility, scripting exports, English XML and individual Editor Wiki pages were updated together.

## Verification

Evidence owner: `D:/InnoEngine-Verification/editor-ui-2026-10-27`.

| Check | Final result | Evidence |
| --- | --- | --- |
| Windows UI source build | Passed; zero compiler warnings/errors | `ui-boxed-build.log` and binlog |
| Shared UI contracts | 558 passed; 0 failed/skipped | `boxed-suite/` TRX, with its earlier 150 architecture cases replaced by the final 152 in `architecture-sdk-owner-tests/` |
| Isolated contracts | 4 passed; 0 failed/skipped | One native retirement case in `boxed-suite/`; three unchanged Core/compiler/build cases in `isolation-final-tests/` |
| Rendering2D authoring | 42 passed, 0 failed/skipped against the fresh SDK output | `rendering2d-sdk-owner-tests/results/authoring.trx` |
| Export shutdown integration | Passed three consecutive runs; counted as one distinct case | `ci-export-retirement/export-retirement.trx`, `ci-export-retirement-repeat/` |
| Official package export | Passed | `rendering2d-export-accepted.log` |
| Architecture and public XML | Passed; final Wiki graph also checks evaluated ownership | `architecture-final.log`, `xml-domain.log`; new public source compiles with XML warnings as errors |
| SDK Wiki graph, signatures, links and examples | Passed; 241 project ownership evaluations, 34,493 API entries and 197 examples | `wiki-sdk-owner.log`, `wiki-sdk-owner-api.json`, `wiki-sdk-owner-examples.json` |
| Editor Debug and Release | Passed; zero warnings/errors | `editor-Debug-boxed.*`, `editor-Release-boxed.*`; earlier maintenance deferrals and cold vendor warnings remain in their original logs |
| Fresh formal SDK and public drop contracts | Passed; no old output directory required | `sdk-actual-owners.*`, `build-entry-sdk-owners.*`, `sdk-api-probe-after.log` |
| FlappyBird product | Debug: 15,000 frames; Release: 5,000 frames; both complete 300 Play frames and return to Edit. D3D12 and Vulkan: 1,000 renderer frames each; all orderly exits | `flappy-Debug-boxed*`, `flappy-Release-boxed*`, `flappy-Debug-d3d12.log`, `flappy-Debug-vulkan.log` |
| GitHub CI | Tracked separately on the pull request for this branch | Its exact head revision, run links and final results accompany delivery; this report does not claim an unexecuted remote pass |

Initial failures remain in the evidence owner: the wrong native product context, a missing direct test annotation dependency, fixtures missing the new session/presentation boundary, an EventHub whose dispatcher was not retained, and boxed target retention (`boxed-before/`). The SDK probe also rejected an exploratory removal of actual Panel contract owners (`sdk-api-probe-before.log`); their required direct references were restored before delivery. Their corrective runs passed; these failures are not labeled as upstream BGCS defects. The full SourceDebug run interrupted during contract changes is not counted as a pass. The 605 distinct local passes are 558 shared contracts, four isolated contracts, 42 Rendering2D cases and the export shutdown integration case; focused reruns are not added again.

## Performance and resource ownership

The isolated UTF-8 benchmark uses .NET SDK 10.0.402, Release, the same text/input, 1,000 warm-up operations and seven samples of 10,000 operations. Hardware: AMD Ryzen 9 6900HX, 8 cores / 16 logical processors. Results are medians for the encoding buffer operation, not whole-editor frame performance.

| Buffer capacity | Previous allocation/op | Current allocation/op | Previous time/10,000 | Current time/10,000 |
| --- | --- | --- | --- | --- |
| 256 | 328 B | 0 B | 1.158 ms | 0.440 ms |
| 1,024 | 1,096 B | 0 B | 1.472 ms | 0.588 ms |
| 4,096 | 4,168 B | 0 B | 5.853 ms | 1.773 ms |

This gain comes from the implementation's stack/pooled buffer and span encoding. It is not attributed to a runtime upgrade. Native unchanged-input allocation checks are separate evidence. No claim is made about long-duration frame/GC performance or a whole-editor speedup.

Native preparation recorded `processes=0`, `bindingGenerations=0` and `materializedBytes=0` on fixed inputs. Its complete input hashing still has a substantial cost; this refactor does not claim to solve the separately tracked native build performance work.

## Deep self-review

The review follows real call chains, not only folders: input → widget/session → Action or feature operation → owner serialization context → shared History → reload-safe handler. It also covers dispatcher ownership, observer release, failed rollback, per-axis batch changes, deferred identity resolution, session/owner retirement and native borrowed memory. Existing Core Events, Identity, Serialization and IO remain authoritative.

BGCS is pinned to 4.1.2. The directly restored SDK package payloads matched their original NuGet archives; the BGCS repository remains at its baseline with no changes. No backend/platform/native/generated source changes were made by this refactor. A direct SDK payload comparison covered 122 files with zero mismatches. No global/implicit using, IVT, private reflection access, migration reader, compatibility overload or placeholder project was introduced.

## Individual quality assessment

These assessments describe verified engineering properties. “Enterprise-grade” and “phenomenal” are not automated test results.

| Dimension | Assessment | Evidence and remaining improvement |
| --- | --- | --- |
| Architecture | Enforceable ownership; local architecture passed | Headless feature boundaries and no Panel-to-Panel dependencies are validator rules. ShaderEditor remains a substantial domain coordinator. |
| Code organization | Improved | Feature-oriented files, project namespaces and visible dependency groups. Further shorten the largest domain-specific canvas workflows when a concrete reusable boundary emerges. |
| State/lifetime cleanliness | Explicit ownership; local lifecycle tests passed | Explicit session/owner, subscriptions, identities and borrow guards. Continue covering future extension failure paths with the same ownership rules. |
| Abstraction | Purposeful, bounded | Domain mechanisms are separated from ImGui adaptation. This is not a promise that replacing the entire UI backend requires no presentation work. |
| Reuse | Core capabilities reused | Events, Identity, Serialization, IO, Graph and History remain shared mechanisms. No replacement infrastructure was added. |
| Visual consistency | Shared foundation; complete visual certification unavailable | Common metrics, fonts, icons, rows, mixed states and feedback; the existing dark ImGui identity is preserved. Full keyboard/DPI/accessibility review remains unexecuted. |
| Extensibility | Improved and guarded | Existing Registry/TypeCache and generation transactions remain the extension boundary; plugins use the current session and neutral operation contracts. |
| Interaction logic | Improved; product/manual limits apply | Ordered multi-selection, common properties, precise axes and batch preflight. Long real-user keyboard/drag/focus journeys remain a future acceptance exercise. |
| Undo/Redo | Strengthened; local tests passed | One authoritative History, immediate samples, rejected LIFO preservation, atomic batches and Missing barriers. No second undo stack. |
| Menu clarity | More consistent | One Action model across search/menu/shortcut/toolbar; enabled/checked rules are shared. Label taxonomy can still benefit from user observation. |
| Component granularity | Improved without empty abstractions | Assets/Shader/Graph presentation assemblies have real consumers. The largest ShaderEditor view remains an honest refinement opportunity. |

## Unexecuted or limited acceptance

- No local macOS device is available; local Metal and interactive macOS acceptance are unexecuted. Remote runner results are tracked separately and do not certify manual device journeys.
- Computer Use captured the real FlappyBird Scene/Game view, but window activation failed twice. Following the tool's recovery limit, further manual input was stopped. Mouse interaction was not certified; the owned visual session was terminated. Separate bounded runs verify orderly exit.
- Full accessibility, all focus/DPI combinations, audible audio quality, extended usability sessions and long-duration GPU/GC behavior are not certified by native layout tests or bounded smoke runs.
- Existing engine issues outside this UI plan remain independently tracked. This refactor cannot establish that the entire engine is perfect.

The configured macOS `Glass.aiff` is unavailable on this Windows host; the completion sound cannot be played.


## Necessary public API changes

- `EditorUiSession`, its controls, pointer-free options/font/input/layout values and guarded `EditorDrawList` express explicit host/session/frame ownership without generated native types in consumer APIs.
- `EditorSelectionSnapshot`, ordered selection operations and `EditorEditGesture` provide shared selection and continuous History boundaries. The inspection edit contract carries an optional scalar component and merge key so batch edits preserve untouched values.
- Asset editing, export ownership and generic graph geometry now have reusable feature/presentation entry points. Existing registries remain their discovery mechanism.
- `RegisterIdentityDomain` extends the Host selection coordinator for temporary runtime worlds. The allocator remains Core-owned; callers retire the routing lease on the owner thread after restoring Edit selection. This coordinator is Host-only and is no longer a project scripting export. Scripts use `EditorInteractions` without registering domains.
- `EditorDisplayNames.Nicify` provides pure label formatting for menu metadata, Inspector and plugins. The session-bound widget method was removed.
- Play Mode and settings notifications return disposable Core Events subscriptions. The Console releases its subscription explicitly.

## Verification entry and retained diagnostics

VSTest assembly discovery derives `DOTNET_ROOT` from the selected `DOTNET_HOST_PATH`. Passing both as command-line environment settings caused a duplicate-key discovery exception with a zero exit code in this SDK. The shared runner now supplies the supported host input without the duplicate root, preserves discovery output and treats stderr discovery errors as failures. Four isolated facts were actually executed, rather than inferred from a zero exit code.

One attempted Build-entry rebuild overlapped a running Wiki CLI and encountered the ordinary Windows lock on that executing CLI DLL. Rebuilding after the owned process exited passed; no production copy/loading workaround was added.

Explicit owner-aware cache maintenance reclaimed 20,782,799,012 logical bytes and protected 174,384,757 bytes while active verification was running. C: free space rose from about 92.5 to 95.5 GiB; logical bytes and physical volume recovery are different measurements. SDK and NuGet global caches were not cleaned.

The full local SourceDebug build interrupted during contract changes remains uncounted. The final focused UI graph, three additional isolation projects and standard Debug/Release product entries passed; the complete SourceDebug matrix belongs to remote CI. No test filter excludes an unresolved UI failure.

The first complete remote matrix at `61f855c0` passed both platforms' Debug/Release products, renderer smoke, BGCS identity, ten hot builds, Solution, architecture, Support Pack and Wiki. Its sole failing fact on both platforms was `EditorShutdownDrainsSupportPackCleanupBeforeReleasingExportServices`: the Build fixture drew a modal without the new UI session and still selected an arbitrary module rather than the export service. The fixture now explicitly composes both owners, enters the session, resolves `EditorExportService` and retries the existing Pending retirement protocol after releasing the fake cleanup barrier. All original UI, cancellation, release and generation-readiness assertions remain. The production implementation and BGCS were not changed for this correction. The replacement commit's complete CI result is tracked on the PR, not inferred from the earlier head.


## Companion source commits

- Rendering2D: `067d4eb023b4dea0df114872247d9b44f69fe20f` — current-session controls, precise edit contract, Core label formatting and generation-only History queries.
- Samples: `3e331fd0fae08491fe11938faa5955c6e79e5089` — officially exported Rendering2D package in FlappyBird.
- Canvas and BGCS repositories have no task changes. Companion commits were created on their existing local branches; no remote main branch was updated.

Both author and committer are Aaron Liao <hsuankailiao@gmail.com>; no co-author trailer is used.
