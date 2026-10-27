# Editor UI implementation ledger

This ledger tracks the approved Editor UI plan against the current `editor-ui` branch. It is an implementation record, not acceptance evidence.

Baseline: `b303d9f10cbaf5a905f7cb529e52cf8293b0d8bd`. Evidence owner: `D:/InnoEngine-Verification/editor-ui-2026-10-27`.

## Required outcomes

- Preserve the existing ImGui visual identity while unifying layout, controls and feedback.
- Repair History rejection atomicity and unify continuous edit boundaries.
- Move reusable asset operations out of File Browser; remove Panel-to-Panel dependencies.
- Keep domain editing independent from native UI and platform implementations.
- Give UI state an explicit session and retirement owner.
- Unify identity-aware selection, focus, queued actions and multi-object operations.
- Unify Action availability, menus, shortcuts, toolbars and command search.
- Separate reusable Graph presentation from Shader semantics.
- Update every affected caller, scripting export, project, Solution entry and document.
- Run meaningful correctness, native widget, reload, product and performance checks.
- Commit as Aaron Liao without co-authors on the current branch and inspect its CI.

## Stage ledger

| Stage | State | Evidence |
| --- | --- | --- |
| Baseline and ownership | Recorded | External baseline inventory and SDK information |
| History atomicity | Implemented; local regressions passed | LIFO rejection, shared gestures, immediate draft records and generation-only queries |
| Domain and presentation boundaries | Implemented | Headless features; Assets/Shaders/Graph presentation; no Panel references |
| UI session and styles | Implemented | Session/owner state, pointer-free facade, native borrow guards; 49 native tests passed |
| Selection, gestures and commands | Implemented; local regressions passed | Ordered identities, mixed values, precise scalar components and atomic batch operations |
| Panel integration | Implemented | Inspector, Hierarchy, File Browser, SceneView, ShaderEditor and Workbench |
| Architecture, scripting and documentation | Passed locally | 241 SDK projects evaluated; current API mappings synchronized; final XML formatting, API signatures and compiled Wiki examples passed |
| Product, performance and final acceptance | Local product checks passed | Final Debug/Release builds, 15,000-frame FlappyBird run, 300 Play frames and renderer matrix; isolated UTF-8 comparison recorded |

## Acceptance rules

Tests use real public contracts and existing replaceable boundaries. No IVT, private reflection, test-only production entry points, compatibility wrappers, generated binding edits or vendor changes are permitted. Current Core IO, Events, Identity, Serialization, History and candidate-generation lifecycles remain authoritative.

The final report must assess architecture, readability, state ownership, abstraction, reuse, styling, extensibility, interaction, Undo/Redo, menu clarity and component granularity individually. Passed checks, failures and unexecuted checks must remain distinct.

## Execution observations

- Native fixtures require an explicit product target and its Editor product properties. Ordinary managed compilation alone does not establish a matched native deployment. Scripting and PlayMode fixtures now declare their actual native product dependency; ImGui fixtures consume the selected target instead of a Windows default.
- The initial full target source build used base product properties and correctly rejected missing ImGui shader inputs. The corrected complete run was cancelled when the component-edit contract changed; it is not a passing result. Final focused UI builds and remote CI must use the frozen final contract.
- Computer Use captured the real FlappyBird Scene/Game view but failed to activate the owned window twice. Manual mouse interaction is unexecuted; native input tests and bounded product smoke remain separate evidence.
- Final acceptance must replace these intermediate observations with actual final results, retaining failed/cancelled command provenance in the evidence owner.


## Closed local regressions

- 558 shared Editor/tooling tests and one real isolated viewport-retirement fact passed. Three further isolated Core/compiler/build facts passed through the corrected SDK test runner. Rendering2D authoring has 42 passing tests: 604 distinct local cases in total, excluding focused reruns.
- Temporary Play identity routing now has an explicit Host lease and retains Missing selections when a domain retires. The real FlappyBird probe verified the same persistent ID in Edit/Play and restoration to the original domain.
- Pure label formatting belongs to Editor Core; menu queries require no native drawing context. All engine/plugin callers use the shared formatter.
- VSTest discovery errors are no longer treated as successful empty discovery; raw discovery logs are preserved.
- Native vendor/cold SDK diagnostics and automatic maintenance-budget deferrals remain visible in their original logs. They were not suppressed.
- Final lifetime review reproduced boxed structs retaining referenced objects through selection/focus/snapshots. Targets now retain only known neutral scalars strongly; all other non-identity objects use weak references. The failing-before regression and complete passing Interactions suite are retained separately.
- The SDK directly references reusable API owners. Its Hierarchy/Inspector contract references remain because they explicitly export four drop targets; a fresh SDK and real script probe verify these public contracts. Non-exporting Panel carriers are rejected by syntax-aware ownership fixtures.
