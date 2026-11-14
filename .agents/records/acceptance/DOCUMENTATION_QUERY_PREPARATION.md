# Exact queries and keyword-search preparation

Opening a prepared documentation snapshot previously materialized its complete inverted keyword map before the caller selected an operation. Exact symbol lookup, section reads and binding-status requests therefore paid for keyword search that they never used.

`DocumentationSnapshot` now initializes keyword postings once, on the first keyword search within the session, using thread-safe publication. Exact reads and symbol lookup retain their direct maps, payload integrity verification and shared generation protection. Search results, ranking and necessary contract continuations are unchanged. The query implementation identity changes; explicit preparation can reuse unchanged parsed pages without claiming an old query generation is current.

## Local verification

All 36 documentation regressions passed with zero skipped tests. The added test starts concurrent first searches, symbol lookups and section reads through the public query API after a canceled search. It verifies consistent snapshot identity, the required ownership continuation, successful later queries and unchanged persisted cache bytes. Existing corruption, mismatch, paging, source removal, writer cancellation and reader protection assertions remain.

The first new test compilation failed because the modulo selector needed parentheses before its switch expression. Only that expression was corrected. The original build log and corrected TRX remain under `artifacts/acceptance/english-documentation/query-lazy-search*`.

The existing Build CLI compiled successfully with zero warnings and errors in 44.90 seconds, using SDK 10.0.402 and at most four managed workers. This was a managed tooling build; no Native preparation was invoked. Query performance after rebinding is recorded separately rather than inferred from this implementation change.

## Natural task ranking, 2026-11-11

The Scene migration exposed an equal-score search result: `instantiate prefab` returned an API member before the actual task guide because the final tie-breaker was page identity. Keyword scoring remains unchanged; equally ranked multiword heading matches now prefer how-to/tutorial pages. Exact symbol lookup still uses the direct symbol map, single identifiers retain reference ordering, and body-only task mentions do not outrank stronger headings. No parser, network operation, extra content scan or per-query index publication was added.

All 37 documentation regressions pass with zero skipped tests. The added public-contract test checks natural task order, the next page of results, an exact identifier, an exact qualified symbol, body-only noise and unchanged persisted cache bytes. TRX: `artifacts/acceptance/english-documentation/query-task-ranking/task-ranking.trx`. The original observed search remains in the Docs artifact `scene-query-prefab.json`; the actual migrated corpus is checked after rebuilding the shared tool.

The existing Build CLI compiled with zero warnings/errors in 46.28 seconds on the same SDK and bounded managed build. This prepares the changed shared query library for real CLI callers; no Native producer ran. The previous native-context over-selection evidence below remains applicable and is not recertified by this focused build.

## Remaining validation-selection problem

The unified plan against the two changed source/test files selected 11 owners and 24 checks. It still propagates through Architecture, Validation, Build and Editor.Hosting test consumers and creates a native context through their product references. The recorded plan is `artifacts/acceptance/english-documentation/query-lazy-plan/plan.json`; planning evaluated the graph but did not execute those native checks.

This is evidence that the approved behavior-obligation/test-boundary refinement remains incomplete. The scoped library tests and CLI build do not certify that full plan. The old Wiki cutover failures and broader native/product acceptance remain open; do not rerun or mark them passed merely because this local change compiled.
