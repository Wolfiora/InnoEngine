# Script reference surfaces and source evidence

This change closes the distinction between host CLR documentation and actual Runtime/Editor script declarations. It does not change engine public signatures or script runtime behavior. Generated backend-native bindings remain outside documentation scope.

## Implementation

The compiler emits type origin metadata from its resolved exports beside each logical reference assembly. Source XML bytes and the documentation schema participate in the frozen reference identity; input changes during preparation reject the result. Documentation tooling selects declarations from the actual PE metadata, resolves their source owners against existing captures, and rejects absent or conflicting origins. XML presence alone cannot invent an exported member.

Explicit `docs export-api --surface runtime-script|editor-script` preparation takes the real reference files, source manifests and one actual framework project. Queries remain read-only and do not perform this preparation. The first attempted export evaluated every source owner (837 graph nodes, approximately 95 MB); the corrected boundary evaluates six framework graph nodes (370,199 bytes). Both observations remain in the original logs.

Actual example compilation now also compares the selected script reference surfaces. CI receipts and certificates bind those comparisons to the real DLL/XML identities and source-owner inputs. Changed script facts select their required compiler preparation. Unrelated projects sharing a source manifest do not invalidate the selected example identity. Missing or failed comparisons cannot become source certification.

## Local acceptance

Evidence is retained under `artifacts/acceptance/english-documentation/`.

| Scope | Result | Evidence |
| --- | --- | --- |
| Real PE extraction, origin mapping, overloads, private/unexported exclusion, surface qualification, unsupported projected constructors, conflicting source facts, cancellation and atomic output preservation | Passed; real example compilation and PowerShell receipt reconciliation included | `query-tests/script-api-capture-04.trx` |
| Independent selected source owners and example compilation | Five example cases passed before the final conflict guard; the affected capture case was repeated above | `query-tests/script-api-example-suite-01.trx`, `query-tests/script-api-capture-03.trx` |
| Per-owner projection impact and unrelated batch member changes | Passed | `query-tests/script-api-impact-01.trx` |
| Existing documentation planning and input evidence | 16 passed | `query-tests/script-api-planning-01.trx` |
| Same-length, preserved-timestamp XML change and framework-content invalidation | Two passed; prior generated XML remained byte-identical | `query-tests/script-xml-02.trx` |
| Receipt, complete source certificate and compatibility contracts | 38, 37 and 27 PowerShell cases passed respectively | Original script receipt/certificate/compatibility logs |

The first XML regression failed because its expected logical name omitted the actual `Scene` namespace. The expectation was corrected from the real compiler mapping; the first failure remains in `query-tests/script-xml-01.trx`. No production mapping or assertion was weakened.

Production managed script preparation and export captured 3,475 Runtime declarations across 24 owners and 3,123 Editor declarations across 20 owners. These are extracted facts, not source certificates. All explicit source-backed declarations have XML; implicit declarations retain their classification.

## Existing projection limitation

Actual references contain 25 Runtime and 52 Editor parameterless constructors without a corresponding public/protected engine constructor. The current stub emitter leaves an implicit constructor in these cases. The capture marks them `projectionOnly` and explicitly disallows construction; it does not silently advertise them as supported or change runtime behavior. See [open items](../open-items.md). A separate compiler correction must verify both runtime compilation and IDE projection.

## Remaining acceptance

Full generated script pages, paired binding, website checks and hosted source evidence must be reconciled against their final candidate. The running platform matrix at engine `4471fe4d` does not certify this later implementation. Protected cross-repository App credentials and default-branch activation remain external prerequisites. No Pages deployment, PR merge or release is claimed here.
