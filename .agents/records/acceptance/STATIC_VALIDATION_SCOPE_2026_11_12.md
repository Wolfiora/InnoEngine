# Static validation ownership and rule-source batching

The planner previously treated an unowned CI regression script as an unknown input even though the CI contract executor already owned that script. This selected every product. Planning and execution now share `ValidationCiContracts`: only the actual top-level `.Tests.ps1` discovery boundary and the explicit rule loader avoid that unknown-input fallback. Actual SDK-declared consumers still participate. Unknown helpers and nested scripts remain conservative; no production or native test requirement is removed.

Eight public-entry regressions passed using the real SDK, including known static owners, unknown helpers, nested scripts, explicit project consumers, empty required discovery and existing journey inputs. The managed test scope built in 11.67 seconds with zero warnings/errors; tests took 72 seconds. Evidence is under `artifacts/acceptance/english-documentation/static-contract-scope/`, with build and execution logs beside it. These are bounded checks, not full-engine certification.

## Rule reads

The rule loader now reads all immutable original blobs through one `git cat-file --batch` process. Size-delimited reads preserve complete UTF-8 content; missing objects, non-blob objects, invalid request identities, truncated responses and failed Git execution fail explicitly. The read disables lazy network acquisition. Explicit preparation remains responsible for missing provenance. No persistent source cache or second authority was introduced.

The original full rule regression, updated regression and depth-one-parent regression passed. The updated cases additionally reject a missing original, a tree instead of a source file and a newline-injected source request. All 241 original rule mappings, translation lines, complete-unit budgets and external-authority assertions remain checked.

Five samples used the same machine, PowerShell 7.6.9 and `Test-AgentRules.ps1 -Paths src/services/input/Example.cs -Rules completion` workload. The original loader at `3c15d77c` measured 4672.3362, 3365.6228, 4637.3420, 3513.9711 and 4325.6428 ms (median 4325.6428). The changed loader SHA-256 `83b72a07f36385db1cb0ef971c26a8675a15ff2af176034adaef34c62e5f9fc3` measured 3176.5868, 2460.2047, 1854.1764, 1693.0751 and 1638.6537 ms (median 1854.1764). The observed median reduction is 57.1%; this is rule-loading evidence, not a whole-pipeline benchmark. Raw JSON remains in `rule-loader-before.json` and `rule-loader-after.json` in the same acceptance output directory.

## Hosted status and limits

Docs run [43195272800](https://github.com/Wolfiora/InnoEngine.Docs/actions/runs/43195272800) at `09216692bf0f9b5ff0b4e039c6d892fef267b430` passed its projection job, including 188 Node cases, ten archive scenarios, four candidate-identity cases and 72 browser journeys. It includes the earlier shallow-provenance fixture fix, not the later batching or scope changes described here. Source certification remains blocked waiting for authenticated matching engine evidence; the aggregate correctly fails.

The incremental complete-source-certificate gap remains separately actionable. These changes do not claim to resolve it, activate an organization App, deploy Pages or certify the running engine matrix. Cleanup of the explicit shallow fixture checkout was blocked by automatic approval review; no deletion bypass was attempted.
