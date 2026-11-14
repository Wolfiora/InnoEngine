# Classify before native source preparation — 2026-11-11

Hosted validation previously fetched all native submodules before it knew the selected scope. Moving that fetch alone was insufficient: compiling the normal Build product also enters Editor/ImGui binding inputs. CI now generates a transport over the existing managed Validation and Documentation libraries. It has no independent policy, project list or query implementation; the existing Build CLI remains the daily executable.

The planner returns a blocked preparation request when selected native owners lack pinned sources. It requires a selected documentation snapshot first. The request is not an executable plan or validation evidence. Hosted preparation checks the exact clean candidate, indexed Git pins and path boundaries, prepares those sources explicitly, and generates a new plan once. It preserves the first request and both planning logs. No local validation operation fetches sources implicitly.

This removes unconditional native checkout from the workflow. It does not claim that every private change has a minimal behavioral selection, that all native source dependencies are independently partitioned, or that native compilation is unnecessary for affected integration/product checks.

## Evidence

Raw files are under `artifacts/acceptance/english-documentation/`.

| Evidence | Observed result |
| --- | --- |
| `managed-validation-bootstrap.log` | Actual isolated managed transport build passed with zero warnings/errors in 33.13 seconds. Outputs contain the tooling/foundation closure and transport, without Editor, native binding or product compilation. This is one build observation with existing dependency caches, not a cold/warm benchmark comparison. |
| `native-preparation/native-preparation-planning.trx` | Seven tests passed, zero skipped, approximately 65 seconds. Six cases cover plan/run/full with and without Docs; a seventh preserves managed selection with an uninitialized submodule. Blocked cases produce no executable plan or results and do not initialize source checkouts. |
| `ci-source-preparation.log` | Nine real local-Git cases passed: exact source bytes, wrong candidate/pin/path, empty pins/owners, wrong request kind, dirty inputs and changed HEAD. File-only transport prevents fixture network access. |
| `ci-native-preflight-planning.log` | Fifteen hosted protocol cases passed. Four added cases cover preparation followed by fresh planning, continued blocking, wrong source pin and missing Docs. No downstream output is published on failure, and planning does not retry repeatedly until green. SDK behavior is simulated in these protocol cases; actual managed compilation is recorded separately. |
| `native-preparation-source-policy.log` | Source architecture policy passed. |
| `native-preparation-agent-links.log` | Agent document structure/link validation passed. |

All five changed PowerShell scripts parsed successfully. The owned-file whitespace check passed. No first-failure evidence was removed and the unrelated Editor Assets edit remains untouched.

## Remaining obligations

Real hosted Windows/macOS execution, trusted Docs source certificates and compatibility carry-forward, missing target captures, legacy removal, cross-repository App setup, Pages deployment and draft-release evidence remain open. Local protocol tests do not certify those external results.
