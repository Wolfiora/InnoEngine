# One documentation rule authority

Eight complete writing/API/example units moved from the engine rulebook to the Docs rulebook. The old engine authority file is removed. Root instructions, the rule index and `inno-api-docs` route through the same registry and an explicitly selected Docs checkout. All 241 original rules, 58 units and supplemental source identities remain registered.

## Verification

`external-rules-tests-final-02.log` passes the complete existing provenance/translation/routing suite plus missing external authority, unknown repository, escaping path, actual complete external loading, root/nested equivalence, changed external clause and unregistered external unit cases. The first fixture failed because its isolated checkout lacked root instruction link targets; the correction copies only those actual files under a checked fixture boundary. Its original failure remains in `external-rules-tests.log`. A subsequent PowerShell syntax failure is retained in `external-rules-tests-final.log`.

The actual documentation task selects all eight units in one 13,984-byte stage, including root instructions, under the 16 KiB budget. This size predates later prose-only reporting and does not replace subsequent loader output. The engine-only result explicitly excludes unread external bodies; a documentation task cannot silently omit them. The checker performs no network access. Docs CI uses explicit, pinned preparation of original Git objects before checking source provenance and actual external bodies.

The source/target clause review is recorded in the existing English rule preservation decision. No source API, engine behavior, native dependency or generated binding changed. Current hosted run 43164720103 predates this cutover. Final paired CI must use the updated engine registry and Docs authority together.
