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
