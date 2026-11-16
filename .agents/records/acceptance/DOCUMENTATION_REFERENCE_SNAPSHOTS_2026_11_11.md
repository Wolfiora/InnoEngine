# Protected documentation reference snapshots

Project and API reference payloads now participate in the existing documentation generation lifecycle. A generation records their exact bytes, source provenance and owners. Identical payloads share a content-addressed registration; page-only updates do not rewrite them. Readers retain the generation lease and read verified cached bytes without consulting the authored checkout, evaluating an SDK or accessing the network.

`docs validate-api` accepts an explicit `--cache-root` / `--snapshot` pair and logical `--reference` paths. Optional repeated `--project` arguments select actual reference owners before the shared SDK capture. Unknown owners fail. Without the snapshot pair, explicit local reference files remain an authoring input and are rechecked before report publication. Reports retain snapshot provenance and per-project reference identities; they do not claim complete source, prose, script, example or publication certification.

## Bounded validation

| Evidence | Result | Scope |
| --- | --- | --- |
| Documentation snapshot regressions | 45 passed, zero skipped | Immutable reads, binding checks, same-length edits, corruption, concurrent reads, cancellation and existing query behavior |
| Reference comparison regressions | 13 passed, zero skipped | Real SDK capture without product builds; frozen references after checkout removal; selected owner excludes an unavailable unrelated SDK; invalid selection cannot replace evidence |
| Snapshot retirement integration | 1 passed, zero skipped | Existing cache retirement protects old reference dependencies while a reader is active, then retires them after release; current generation remains readable |
| Build entry compilation | Passed, zero warnings/errors | Managed Build CLI and its dependency closure |
| Source architecture validation | Passed | Source policies; compiled API coverage remains separate |

The initial snapshot test run exposed five incorrect test assumptions that `InvalidDataException` derives from `IOException`. Assertions were corrected to the exact exception contract; production checks were not weakened. Both initial and corrected logs/TRX remain under the owned acceptance output.

Raw outputs are in `artifacts/acceptance/english-documentation/`: `reference-snapshots.log`, `reference-snapshots-corrected.log`, `prepared-reference-comparison.log`, `reference-retirement.log`, `reference-snapshot-entry.log`, `reference-snapshot-source-check.log`, and `reference-snapshots/` TRX files. No native preparation, product execution, website rebuild or remote matrix was required for these bounded checks.

## Remaining delivery obligations

The engine planner and legacy Wiki callers still require migration. The new prepared-reference path is an integration prerequisite, not proof that the complete pipeline has switched. Refresh the two changed tooling API captures and Docs binding after committing these inputs. Three platform-specific source captures, final source certification, paired-PR automation, actual Windows/macOS CI, Pages deployment and release rehearsal remain outstanding. Historical successes are not promoted to current completion.
