# Editor and game UI capability implementation

## Baseline

- Engine: `editor-ui`, `7f56dd64bd29231351f5441c18877b0c3ad3229b`.
- Samples: `main`, `3044cdc93809ae247f7876f3bbc6ce993a7a537c`.
- Canvas: `main`, `eed5fd9bdb13d5f29351aff2b8c52eed58138266`.
- Rendering2D: `main`, `067d4eb023b4dea0df114872247d9b44f69fe20f`.
- All recorded working trees were clean before implementation on 2026-11-03.
- Initial free space: C 51,595,444,224 bytes; D 171,321,458,688 bytes.

## Execution and acceptance ledger

### Current scope authority (2026-11-04)

The user's latest instruction limits further implementation and acceptance to the engine repository. Do not modify, build, export, install, or test any external plugin repository. Preserve existing companion changes without staging, reverting, or extending them. Historical companion results below are evidence of earlier work, not remaining delivery requirements.

Exercise extensibility and scripting contracts with generic engine-owned fixtures and the existing public boundaries. External plugin integration is outside the current scope; do not claim its acceptance. Engine UI, input, isolated preview, product closure, lifecycle, performance and architecture requirements remain in scope. Imported image acceptance must not depend on completing a companion renderer or regenerating its authored assets.

GitHub #64–#74 were checked and remain open. Implementation and focused regressions are available for several items, but #65 isolated preview and #66 IME/gamepad remain incomplete, and final applicable acceptance has not finished. No overall completion or issue closure is claimed.

| Order | Issue | Owner | State |
|---|---|---|---|
| 1 | #72 dependent creation History | Editor Scene / Interactions | Reproduced; 49 focused tests passed, including Prefab creation/replay; committed as `3e6a7cb7` |
| 2 | #74 Player reference closure | Shared product / scripting / build | Shared Graph owner reference added; product acceptance pending |
| 3 | #73 activation diagnosis; Refs #18 | Scripting / Diagnostics / presentation | Exact activation identity, candidate construction ownership and runtime-scoped rejection diagnostics implemented; 70 runtime and four inspection tests passed; formal plugin/reload/source presentation acceptance pending |
| 4 | #64 typed values, #66 input, #71 focus | Core Input / Input / UI / SDL / RmlUi | Values/focus implemented; real backend tests passed; IME/gamepad pending |
| 5 | #70 time, #67 layout | UI / RmlUi | Per-context clock and independent layout/input implemented; native build and 23 UI tests passed, including interleaved CSS animation and 28 density combinations; layout query implemented; corrected real-backend suite passes 29 tests, including 28 density combinations; formal consumer and final acceptance pending |
| 6 | #68 images, #69 font metadata | UI / Rendering Assets / Assets | Font metadata fixed; imported image contracts, source-aware import and resource retirement implemented; 47 engine-owned UI tests passed; complete product/rendering acceptance pending |
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

## Imported image bridge (in progress)

Official BGCS HostSource generation succeeded with 4.1.2; its existing friendly error-message projection warning is retained, and the raw bridge remains validated. The Windows native product build succeeded in 8m28 (ui-images-native.log). Three real-backend image tests pass, including 100 replacement/context-retirement cycles, rejected cross-thread disposal, no pixel publication for imported artifacts, and two-context isolation. A subsequent prepared-layout retirement regression reproduces a stale draw command (ui-images-prepared-before.trx, one failure). The adapter now invalidates its prepared frame after image registration/retirement; rerun is pending. Imported document dependencies, complete rendering acceptance and API documentation are not yet complete.

The generic image source/import contracts compile with zero warnings/errors. Three focused import/builder tests pass (ui-images-import-contracts.trx): relative image and external sprite-sheet paths share a declared dependency, path escape fails, and runtime registration URIs cannot enter persisted payloads. UI remains language-neutral; resource syntax is interpreted only by its language backend/frontend. Native frame invalidation and the complete updated UI suite still require the next official target build.

The corrected native product build passes and ui-images-complete.trx passes all 40 UI tests. Imported image source artifacts are now pinned through the existing asset lookup lease and released with document/context/runtime ownership; ui-images-pinned.trx again passes all 40 tests, including the retained-key assertion. Canvas sampling and bundled graph regeneration, read-only mounts, full publication/closure and final API inventory remain in progress. This is not issue #68 closure.

## Engine-only image acceptance follow-up

The scope authority above supersedes the earlier companion renderer work: no further external plugin changes or acceptance are included.

`ui-retirement-before.trx` reproduces two lifecycle defects: backend retirement released dependent images too early, and a context with pending resource retirement still admitted new work. The runtime now uses Core `RetirementPendingException`, preserves dependencies until native retirement succeeds, and retries context resources without repeating native destruction. New work is rejected during retirement. No parallel lifecycle protocol or new public runtime API was introduced.

`ui-images-content.trx` passes all 47 UI tests, zero skipped. New cases cover read-only source mounts, immutable references across replacement/Missing/recovery, two-context artifact residency, and actual exported content loaded by `AssetDatabase` after the authoring loader and source files are gone. This validates content publication, not a complete executable Player/GPU acceptance.

Runtime no longer rewrites document text to substitute font families. It supplies immutable resource bindings; the language backend interprets them in document styles, fragments and subsequent attribute updates. A real backend test verifies text values remain intact. Bindings are retired with their native context/document and contain no collectible callbacks or asset objects.

`ui-images-performance-final.trx` passes 46 tests including five alternating samples of 1,000 warm draws per path. Both imported and pixel images allocate 832,000 bytes per sample, submit 1,000 draws, and publish zero mesh/texture updates. CPU medians were approximately 11.55 ms and 11.56 ms respectively; one imported sample was 16.83 ms during other acceptance work, so this is not controlled whole-product performance evidence. The first performance run accidentally placed native sessions in separate parallel test collections and correctly rejected cross-thread process ownership; the fixture now shares the existing native UI test owner, with no production check weakened.

`ui-images-normal-build.log` and `ui-images-build-host.log` finish with zero warnings/errors. `engine-ui-images-sdk.log` uses the formal script compiler/IDE generator; `engine-ui-images-ide.log` builds the generated projects with the imported image contract. `wiki-images-final.log` passes the SDK-evaluated architecture, XML, scripting/Wiki gate for all 241 project owners in the Windows selection. Other platform products are not accepted by that gate.

Deep review: UI keeps neutral image references and dimensions; Rendering owns GPU residency; Assets owns artifact leases; the language backend owns source interpretation. Native bindings were generated officially, without BGCS/package/vendor modifications. Remaining work includes actual rendering/product acceptance, IME/gamepad, isolated previews and the final quantitative matrix. This checkpoint is not overall plan completion, and issues remain open.

## Controller input checkpoint (2026-11-04)

Issue #66 remains in progress. The engine now has neutral connection/button/axis facts, immutable held-state snapshots and explicit SDL device-handle ownership. Runtime sessions created after connection receive current state without fabricated edges. Game View communicates both focus release and acquisition through the existing Host input source; no Panel-local input manager or second event bus was introduced.

`gamepad-contracts.trx` passed five focused contracts. `gamepad-input-restored.trx` passed 29 input tests after formal output preparation; the earlier `gamepad-input-full.trx` missing-dependency failures remain recorded. `gamepad-sdl-virtual-retry.trx` passed the isolated virtual-device connection and native-event routing test; the first run exposed a double-detach assertion in test cleanup, which was corrected without weakening production release checks. This does not establish physical-controller acceptance.

`gamepad-play-focus-prepared.trx` passed 25 Game View capture tests after normal output preparation. The first scoped-only test invocation failed to advance at host startup and was terminated only after verifying its owned process identity; no test was counted as passed in that run. The exact startup cause was not established. `gamepad-build-host.log` records a successful formal Build entry compilation with zero warnings/errors.

Pending for this checkpoint: final immutable-state allocation samples, current script/IDE compilation and complete Wiki/API signatures. Pending for #66: composition/text-session ownership, backend UI navigation/repeat, full lifecycle/product acceptance and separately reported physical-device checks. No plugin repository or installed package was changed.

Controller checkpoint verification: `gamepad-suspension-input.trx` passes 33 tests; `gamepad-play-scoped.trx` passes 44 Game View/PlayMode tests, including suspension and focus reacquisition. The SDL virtual-device test remains independently recorded. `engine-gamepad-sdk.log` and `engine-gamepad-ide.log` pass the formal engine-owned script consumer and generated IDE compilation. `wiki-gamepad-final.log` passes architecture, XML, scripting and Wiki checks with 241/241 SDK project owners evaluated; nonselected platform products are explicitly not evaluated.

Five 1,000-frame allocation samples in `gamepad-input-final.trx` each report 72,000 bytes for an empty backend and 72,000 bytes with sixteen unchanged controllers. This establishes state reuse on this path, not whole-product CPU or frame-time acceptance. Formal cache maintenance reclaimed 2,847,487,429 bytes under registered owners. BGCS remains 4.1.2; no BGCS, native generated binding or third-party file changed in this controller checkpoint.

The suspension audit also fixed the existing Shell route so controller actions cannot be queued while suspended, while connection lifecycle facts remain observable. The implementation uses Core Events and per-session state; native handles remain in SDL, and viewport focus remains in the Host/capture boundary. #66 is still open because IME, UI navigation/repeat and final lifecycle/product acceptance are incomplete.

## Controller document navigation checkpoint (2026-11-04)

The UI adapter now consumes routed controller changes through the native document's directional focus, control editing and activation policies. Repeat state is context-owned, contains only connection/control/time values, and uses the existing independent context clock. No synthetic Core event, device poller, native ABI change, public service or parallel input registry was introduced. Stick activation/release hysteresis, one-shot activation, bounded repeat catch-up, input ownership loss and document retirement are explicitly documented.

`gamepad-ui-build.log` records a normal build with zero warnings/errors, zero native processes and zero binding generations. `gamepad-ui-suite.trx` passes all 50 UI tests, zero skipped, including real native focus/navigation, two independent paused clocks, held-state retirement, disconnection, document replacement and ordered press/release in one sample. The initial targeted run passed two tests and failed one because a test assertion still expected the pre-step focus after its newly inserted long step; the test ordering was corrected and the complete suite rerun. This is automated native-backend evidence, not physical-controller acceptance.

The remaining #66 work is IME composition and shared platform text-input ownership, plus final lifecycle/product validation. Other issue gates, including isolated scene previews, remain incomplete. No external plugin repository, BGCS package/source, third-party source or generated binding was changed in this checkpoint.
