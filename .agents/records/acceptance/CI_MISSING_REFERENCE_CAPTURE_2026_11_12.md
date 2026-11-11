# Missing source reference capture in hosted validation

The hosted planner previously aborted every downstream job when any selected owner lacked a source API capture. That prevented a clean platform runner from preparing the very references needed to export the missing facts, and suppressed independent engine checks.

The catalog remains a planning prerequisite. Individual missing captures remain required `Blocked` results. If an owner already has a selected build, the plan places its capture after that build in the same context. The executor exports unverified facts from the resolved compilation graph into the retained diagnostic directory. It neither passes the missing comparison nor builds additional products for a documentation-only deletion.

`DocumentationReferences.ExportApi` now accepts the same explicit prepared artifacts and SDK graph inputs as comparison. Context, owner and source-resolution validation remain in the existing extractor. Export does not certify prose, examples, script projections or publication provenance.

## Local acceptance

- Fifteen hosted planning protocol cases passed, including missing catalog rejection, retained missing-owner requirements, stale pairing rejection and native source preparation.
- Fifteen reference extraction/comparison cases passed. A prepared graph avoids another reference resolution; mismatched graphs cannot replace prior facts.
- Two missing-capture cases passed. A documentation-only capture deletion selects no build. An actual changed owner builds, exports its real type, and still reports `Blocked`; the overall execution rejects certification.
- The first execution fixture failed because two fixture policies attempted to create the same log. Its initial TRX and diagnostic retry remain under `artifacts/acceptance/english-documentation/query-cutover/`. Distinct policy responsibilities fixed the fixture without weakening the assertions or changing production logging.

Passing local evidence is retained in `missing-capture-execution-policy.trx`, `prepared-api-export.trx`, and `missing-capture-ci-planning.log`. These results do not certify the actual macOS or Browser source captures, the full hosted engine matrix, or publication. The preceding hosted run is [43060870028](https://github.com/Wolfiora/InnoEngine/actions/runs/43060870028); its results remain revision-specific.
