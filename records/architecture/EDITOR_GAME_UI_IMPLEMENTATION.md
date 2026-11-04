# Editor and game UI capability implementation

## Baseline

- Engine: `editor-ui`, `7f56dd64bd29231351f5441c18877b0c3ad3229b`.
- Samples: `main`, `3044cdc93809ae247f7876f3bbc6ce993a7a537c`.
- Canvas: `main`, `eed5fd9bdb13d5f29351aff2b8c52eed58138266`.
- Rendering2D: `main`, `067d4eb023b4dea0df114872247d9b44f69fe20f`.
- All recorded working trees were clean before implementation on 2026-11-03.
- Initial free space: C 51,595,444,224 bytes; D 171,321,458,688 bytes.

## Execution and acceptance ledger

| Order | Issue | Owner | State |
|---|---|---|---|
| 1 | #72 dependent creation History | Editor Scene / Interactions | Reproduced; 49 focused tests passed, including Prefab creation/replay; committed as `3e6a7cb7` |
| 2 | #74 Player reference closure | Shared product / scripting / build | Shared Graph owner reference added; product acceptance pending |
| 3 | #73 activation diagnosis; Refs #18 | Scripting / Diagnostics / presentation | Exact activation identity, candidate construction ownership and runtime-scoped rejection diagnostics implemented; 70 runtime and four inspection tests passed; formal plugin/reload/source presentation acceptance pending |
| 4 | #64 typed values, #66 input, #71 focus | Core Input / Input / UI / SDL / RmlUi | Values/focus implemented; real backend tests passed; IME/gamepad pending |
| 5 | #70 time, #67 layout | UI / RmlUi | Per-context clock and independent layout/input implemented; native build and 23 UI tests passed, including interleaved CSS animation and 28 density combinations; layout query implemented; corrected real-backend suite passes 29 tests, including 28 density combinations; formal consumer and final acceptance pending |
| 6 | #68 images, #69 font metadata | UI / Rendering Assets / Assets | Font rename path overwrite reproduced and fixed; 19 UI tests pass including source metadata; images pending |
| 7 | #65 isolated preview | PlayMode / Scene / Hosting / Rendering | Not started |
| 8 | Product, lifecycle, performance, API and architecture acceptance | Respective owners | Not started |

The approved plan requires real backend and formal plugin acceptance, 100 UI/preview open-close cycles, ten reload cycles, scale coverage and five paired performance samples. A focused test is not a substitute for these final gates. All issues stay open until an accepted PR is merged. Only fully verified issues may use `Fixes`.

## Boundaries

No computer use. No edits to BGCS, packages, third-party source or generated bindings. Binding changes use the official generation pipeline. No global/implicit imports, compatibility readers, duplicate event/history/identity/asset systems, private reflection or test-only production APIs. Core ownership and strict generation retirement remain mandatory.

#63 runtime snapshot serialization, #57 preparation progress and unrelated performance issues remain separate. Closed #35/#37/#19 retain their historical acceptance; #72/#73/#74 track the new consumer scenarios.

## Completion review

### History evidence

Evidence is retained in the external, task-owned acceptance directory.
`history-before.trx` reproduces the rejected dependent creation Redo on the baseline production code (one failing test).
`history-after.trx` passes all 45 selected SceneHistory/EditorHistory tests after separating protocol queries from execution-state checks.
Expanded tests in `history-complete.trx` pass all 48 cases, including getter-free queries, nested failure compensation/retry and a missing plugin generation barrier. The additional Prefab case is not included in that result.
`history-prefab.trx` passes all 49 cases, including Prefab instantiation and dependent property replay with persistent identities.

`ui-values-focus-font-final.trx`: 19 tests pass, zero skipped. Includes real RmlUi values/events/focus, 100 document replacements, cross-context/thread isolation and source-aware font rename/copy/Missing recovery/read-only mount checks. This does not establish the full product or IME matrix.

`assets-pipeline.trx`: 123 tests pass, zero skipped, with a separate SDK output owner on D. Font metadata fix committed as `1fbba732`.

`ui-controls-time-scale.trx`: 23 tests pass, zero skipped. Clock steps and layout refresh preserve independent contexts; actual CSS geometry follows interleaved small steps; native Tab and reverse Tab share explicit focus across 28 density combinations. These are automated backend metrics, not physical display or IME device acceptance. RmlUi caps individual animation advances at 0.1 seconds; this upstream behavior is documented, and no arbitrary seek is claimed.


Pending. Review real execution ownership, failure compensation, input and resource lifetime, scripting exports, reference direction and Core reuse. Record unexecuted device checks separately. This ledger does not assert implementation or acceptance completion.

## Activation evidence

`activation-runtime.trx`: 70 selected runtime tests pass, zero skipped. Covers independent diagnostic scopes and constructor failure retiring earlier candidate instances before a successful retry. `activation-inspection.trx`: four tests pass, including exact factory failure identity and missing injected service recovery. These results do not establish complete #73 acceptance or physical UI behavior.

`activation-console-current.trx`: nine Console retention/correlation tests pass through VSTest against the freshly built assembly. Diagnostic semantic/object identities survive presentation projection, distinct targets do not collapse, and resolved diagnostics disappear from new snapshots without mutating captured snapshots.

## Layout and deployment work in progress

The layout batch contract, immutable geometry and viewport mappings are implemented. Official BGCS generation and the first complete native product preparation succeeded. `ui-layout-backend.trx` records 26 passed and three failed tests: the adapter used storage-order matrix indexing; the scale fixture omitted native pixel snapping; and the scrolling fixture exposed a wheel-direction mismatch and did not advance the backend's scroll controller through normal updates. Corrections use the third-party public row accessor, retain exact adjacency checks with documented pixel quantization, and map neutral wheel direction in the adapter. Corrected native deployment succeeded; `ui-layout-backend-final.trx` passes 29/29 tests with zero skips. The initial failures remain recorded as regression evidence. Native preparation logged 141,010 file hashes and 14,012,613,282 bytes read over 28m57; integrity checks were not weakened.

The fixed RmlUi source supports Flex/Table and does not implement CSS Grid. Geometry queries consume supported backend layout; no second layout algorithm or fake Grid support is added.

The Player Graph regression compiles one logical Graph consumer in authoring/deployment modes, removes the target Graph owner to require failure, then restores it. Its initial scoped run rejected mixed stale UI assemblies before reaching the test. After rebuilding the normal reference graph, `player-graph-contract.trx` passes this test through VSTest (one passed, zero skipped). The redundant product-target test invocation was canceled after its current managed assembly had passed the focused test; a single official native preparation was allowed to complete. This focused contract result is not a complete Player acceptance. No package or output validation was disabled.

## Current scoped follow-up (2026-11-04)

`ui-layout-input-frozen.trx`: 30 real-backend UI tests pass. `activation-candidate-source.trx`: two real compiled script candidate/source-correlation tests pass; candidate rejection preserves last-good and corrected source activates successfully. `input-immutable.trx`: nine input tests pass, including defensive snapshot ownership.

The Console lifetime moved from the Logging Panel module into Diagnostics. Inspector consumes the existing Console contract, and no Panel references another Panel. `console-owner.trx`: ten tests pass, including discovery/injection without the Logging Panel settings dependency and deterministic subscription retirement. The first fixture incorrectly duplicated the automatically supplied LogRouter and correctly failed ambiguous-service validation; the corrected fixture does not duplicate it. `activation-inspection-owner.trx`: four tests pass. `activation-runtime-owner.trx`: 70 tests pass. These scoped managed iterations used official SDK Compile/CopyFilesToOutputDirectory and VSTest with existing native deployment; they are not a new complete native product acceptance.


`console-owner-100.trx`: ten tests pass, including 100 actual extension-runtime create/retire cycles with one live Play subscription and zero after each retirement. `activation-source-final.trx`: four candidate/source/canonical-identity/cache regressions pass. Read-only full API inventory completed and exposed stale normal-output interface XML and four undocumented partial type declarations; the normal UI/native/runtime/backend graph was rebuilt through the official targets and the partial declarations now have explicit XML. API tables are updated from the current inventory; the final Wiki gate still requires current formal logical script references and the complete evaluated output graph.

## Ordered input follow-up (2026-11-04)

Core Input now owns immutable acquisition-ordered facts. InputSnapshot, RenderOutputInput and UiInputSnapshot share the sequence; positioned pointer facts are transformed with the actual output/view. Aggregate getters derive from that sequence instead of becoming a second event authority. The existing Core Events source still performs dispatch.

Scoped evidence: input-ordered.trx passed 10 tests; ui-ordered-full.trx passed 31 tests; render-ordered-input.trx passed seven selected rendering tests. A missing-font text-navigation fixture initially triggered a native assertion; registering a real font made the isolated ordered-input test pass. Missing-font diagnostics remain a risk, not an accepted suppressed failure.


Execution review found that Play presentation bound Scene, audio and UI but not Input. Its existing scope now binds the actual Play InputRuntime, with reverse release; the new public-boundary regression is pending. No Panel stores a second input owner.

IME design review found existing ImGui/SDL StartTextInput and StopTextInput ownership. Game UI must share the platform text-input ownership protocol rather than independently toggling those calls. Session revocation and late composition commits must be resolved before #66 can be verified. Gamepad, imported texture sharing, preview isolation and final quantitative acceptance remain incomplete.

## Current ordered-input acceptance (2026-11-04)

`canvas-ordered-capture.trx` passes 12 tests, including pointer press/move/release slices leaving the canvas in one frame without losing capture or duplicating updates.

`play-presentation-input.trx` passes the real module/Host test for isolated Play input binding, outer input restoration and rejected non-LIFO disposal followed by legal retry. `play-input-regression.trx` passes all 18 selected Play Mode tests.


`input-ordered-close-scoped.trx` passes 11 InputRuntime tests. The broader run `input-ordered-close.trx` records 20 passed and one failed: the existing test output has an SDL native module missing a symbol required by the current managed binding. An official target-specific build/deployment is in progress; no native file was manually replaced and this failure is not counted as accepted.

A Wiki inventory without SDK evaluation produced an unusable cross-project symbol graph; it is retained as a failed invocation, not used to rewrite unrelated API documentation. The corrected inventory uses the selected SDK. The source audit also identified compact XML in the new input contracts; those comments now follow the repository expanded XML convention.

The formal Windows target build and native deployment completed in 5m39 with zero warnings/errors. Native preparation reused all products (processes=0, bindingGenerations=0); it validated 55,952 files / 4,498,740,620 bytes. `input-native-final.trx` passes all 21 input tests, including the real hidden native-window lifetime test. The former symbol mismatch was a stale test deployment, not a BGCS source defect.

After mapping allocation cleanup, `input-mapping-final.trx` passes 22 tests. Five samples of 100,000 construct+map operations consistently measure 1,112 to 312 bytes per pointer operation and 2,488 to 2,184 bytes per mixed operation. No pool, global cache or weakened ownership was added. CPU times were collected during other acceptance work and are not claimed as controlled performance acceptance. These measurements compare the new ordered-input implementation before/after allocation cleanup, not the original engine or full Editor frame performance.


## Positioned input and companion verification

`rendering2d-routing-final.trx`: all 45 tests pass, including public frame collection for pointer capture, repeat keys outside the image, focus revocation and suspension.


SDL button/wheel coordinates now survive neutral Core Events and Game View projections instead of borrowing the last motion coordinate. Projections share the existing global consumption lifetime. First-observed key repeats establish held state without inventing a pressed edge. `input-positioned.trx` passes 24 tests; `play-positioned.trx` passes 42 input-capture/PlayMode tests. The current Build host build passes with zero warnings/errors. These are scoped managed regressions using the formally prepared native deployment; IME/gamepad and final product acceptance remain incomplete.

## Scope clarification (2026-11-04)

Engine contracts and acceptance use generic consumers defined in the existing engine test owners. No external application implementation or documentation is part of this delivery. Existing companion workspace changes are preserved and are not included in the engine commit. Issue requirements remain the implementation authority; no application-specific API, naming or behavior is introduced.

Current SDK-evaluated API inventory covers 241 projects. The initial Wiki gate reported 202 stale signature mappings; all affected mappings now match that inventory. A fresh full gate is still required.

## Generic UI resource and script acceptance

The source architecture gate passes (`architecture-generic-ui.log`). The official native test build passes with zero warnings/errors (`ui-font-safety-build.log`); it recompiles the UI adapter through the existing toolchain, with no binding generation or third-party edits.

`ui-font-resource-recovery.trx` records two initially failing recovery cases: dynamic inline styles did not apply the backend's existing logical font mapping. That path now reuses the same mapping as document/fragment loading. `ui-font-style-final.trx` passes all 33 UI tests, zero skipped, including explicit unresolved-font failures for single-line and multiline controls followed by normal editing recovery. Native assertions remain enabled.

An engine-owned generic source project passes the formal script compilation/IDE generation command and an actual SDK build of both generated IDE projects (zero warnings/errors, `engine-ui-sdk.log`, `engine-ui-ide-build.log`). It consumes values, focus, clocks, layouts and ordered input through logical namespaces. Deployment regression and a fresh evaluated Wiki gate are running. These do not replace the remaining IME, gamepad, texture sharing, preview or final product requirements.

`script-ui-contracts-final.trx` passes both authoring and deployment regressions after a normal SDK restore/build refreshed the test runtime dependency graph. `wiki-generic-ui.log` passes the complete selected-target Wiki/API/architecture check, with 241/241 MSBuild project owners evaluated. macOS and Browser products are outside that Windows gate and are not claimed as accepted.
