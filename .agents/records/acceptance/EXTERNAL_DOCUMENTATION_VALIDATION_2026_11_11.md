# External documentation validation — implementation evidence

This stage replaces the unified planner's engine-local Wiki selection with explicit, protected InnoEngine.Docs snapshots. It does not complete the overall modernization or certify a hosted deployment.

## Implemented boundaries

- `validate plan/run/full` accept an explicit documentation cache and snapshot, with an optional baseline snapshot for documentation deltas. A supplied plan cannot be silently rebound to different documentation inputs.
- Project ownership, includes, necessary contracts, examples and reference contexts come from the canonical catalog. Validation no longer reparses Markdown or guesses example references from namespaces.
- Prose changes select content checks. Declared examples select their actual compilation context. Source reference checks use selected owner facts; unrelated prose does not invalidate those facts. Removed required captures remain blocked requirements.
- Missing external documentation cannot fall back to the old engine Wiki. Missing discovery, unavailable compilation inputs and damaged immutable payloads cannot become successful receipts.
- Standalone evidence reconciliation opens the same protected snapshot and verifies its inventory, pages and selected source facts. A receipt cannot certify a damaged or missing input generation.
- Hosted preparation requires an exact clean Docs checkout and matches its canonical binding to the actual engine candidate tree. The retained pairing record does not promote Unverified source API status into a certificate.
- Compiled API inspection, documentation API comparisons and example compilation share one resolved SDK reference union per eligible execution context. The caller maintains the frozen execution boundary and closing checks. A supplied graph is not independent freshness evidence.
- Existing late-selected references, property variants, preserved timestamps, dependency output identity and build-count assertions remain in the compilation tests. Their documentation fixture now captures real source facts instead of writing old Wiki markers.

## Evidence retained so far

| Evidence | Result and scope |
| --- | --- |
| `external-documentation-plans.trx` | Eight selected documentation planning cases passed before the later reference-reuse changes. |
| `external-documentation-contexts.trx` | Eighteen selected context and scope cases passed before the final additions. |
| `resolved-documentation-graphs.trx` | Sixteen Architecture documentation-reference/example cases passed. The fixture rejects a repeated `ResolveReferences` invocation after graph capture; prepared graph reads succeed. Wrong configuration, missing owners and empty graphs are rejected. |
| `resolved-documentation-union-fixed.trx` | One additional multi-owner reference case passed unattended in eight seconds. Two selected owners consume the same resolved union without repeating SDK resolution. |
| `external-validation-selection.trx` | Seventy-three Validation cases outside `ValidationCompletionTests` passed, with zero skipped, in 14 minutes 9 seconds. This includes corrupt prepared payloads, removed captures, unrelated prose and owner/context selection. |
| `external-validation-compilation-pilot.trx` | Two real compilation/closing-read cases passed, with zero skipped, in 8 minutes 15 seconds. The later consumer-root variant refinement is not covered by this result. |
| `external-validation-compilation-variants.trx` | Two real compilation cases passed, with zero skipped, in 14 minutes 41 seconds: reference-edge property variants and references selected during execution. This precedes the final resolved-union variant assertion and evidence-payload refinement. |
| `external-validation-frozen-inputs.trx` | Two focused cases passed, with zero skipped, in 8 minutes 32 seconds. The real variant case now checks the shared resolved graph preserves consumer-selected output variants, accepts valid evidence, then rejects a corrupted reference payload during standalone reconciliation. The execution-entry corruption case also passes. These overlap earlier cases and are not additional unique coverage counts. |
| `external-ci-planning-contracts-fixed.log` | Six hosted planning protocol cases passed: missing snapshot, missing capture, prose-only selection, prepared Native selection, empty discovery and selector failure. These simulated protocol cases are not engine or Native certification. |
| `external-ci-candidate-contracts.log` | Eleven planning protocol cases passed: the original six plus exact candidate binding, stale tree rejection, wrong revision rejection, dirty checkout rejection and wrong source repository rejection. Matching input records retain Unverified source API status. |
| `external-report-contracts.log` | Twenty-two aggregate-report cases passed, including missing compiled outputs. |
| `external-ci-retention-contracts.log` | Five retention protocol cases passed; these are not actual Native exports. |
| `external-ci-retention-final.log` | The same five retention cases passed after the hosted planning guard was added. This is a rerun of existing cases, not five additional distinct cases. |
| `external-validation-all.log` | Partial unsuccessful broad run. The first recorded failure is the old compilation fixture's missing external documentation snapshot. Its Input suite passed 18 tests. No final TRX exists for this run, so no complete-suite pass is claimed. |

Logs and TRX files are under `artifacts/acceptance/english-documentation/`; TRX files are in its `external-validation/` child. Subsequent results must be appended only after completion, with their actual scope.

Source-only architecture validation, agent document structure/link validation and the owned-file whitespace check passed. The final daily-entry compilation, including consumer-root preservation and shared snapshot verification for execution/reconciliation, passed with zero warnings and errors in 42.88 seconds (`external-validation-final-entry.log`). The final source-only architecture check passed (`external-validation-final-source-policy.log`). Subsequent shell candidate-binding changes are covered by the eleven protocol cases above; they do not change that compiled implementation.

## Diagnosed fixture stall

The first multi-owner fixture restore enabled SDK node reuse. Its completed parent left an idle MSBuild child holding the redirected output pipe, preventing the test's process reader from completing. After inspecting the exact process, parent, start time and inactivity, only that fixture-owned child was stopped. The resulting eight-minute run is retained as intervention evidence, not an ordinary performance sample.

The fixture now restores with one SDK node, node reuse disabled and parallel restore disabled. Its unattended rerun passed in eight seconds. Both the original `resolved-documentation-union` evidence and the `-fixed` result remain available. This was a demonstrated pipe-drain stall, not a timeout imposed on legitimate compilation.

The first hosted planning harness attempt also remains in `external-ci-planning-contracts.log`: its fake SDK accidentally interpreted `-p` as an ambiguous PowerShell common parameter. The corrected argument-preserving harness passed all six cases; no production check was suppressed.

## Remaining measured cost

One isolated compilation fixture emitted a 34,855,211-byte formatted plan and a separate 33,226,962-byte SDK graph. In the compact representation, context data occupied 26,809,626 bytes, including 18,088,531 bytes of effective variants. These are observations from that fixture, not universal latency measurements. A later normalization must preserve all effective property instances and inputs; simply dropping variants would weaken correctness.

## Cleanup and external boundaries

Registered cache retirement for the interrupted temporary fixture reported 209,349 logical bytes reclaimed and zero protected bytes. Automatic approval review rejected deletion of the outer temporary checkout with `blocked by policy`; it was left in place, without attempting another deletion mechanism. First-failure evidence remains retained.

Repository metadata reports administrator access to both repositories. Docs Pages is configured for Actions deployment, but this is not a successful deployment. Repository-level secret lists were empty; the available credential could not authenticate the App installation endpoint. Cross-repository App setup remains unverified, rather than being described as a lack of repository write access.

The hosted workflows still need explicit candidate checkout/binding, trusted source certificates, real Windows/macOS runs and deployment evidence. The old documentation tree and standalone Wiki callers have not yet been removed. None of these pending requirements are marked passed by this stage.
