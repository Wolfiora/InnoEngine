# API draft publication implementation

Status: In progress. This record supersedes the paired-PR publication design for documentation; historical acceptance remains historical.

## Revised acceptance boundary

The user explicitly removed performance sampling, stress/volume testing and large build acceptance from this delivery on 2026-11-13. Continue implementation and necessary functional regression checks. Compilation required to produce real API facts is still an implementation prerequisite; it must not grow into another full engine, Native or product matrix. Preserve existing measurements as historical evidence without repeating them. Full Engine validation run 43430034014 was requested to stop under this revised boundary. Unexecuted or canceled broader acceptance is reported, not passed.

## Approved behavior

Engine master produces an independent, complete C# API export. A protected writer imports it into InnoEngine.Docs draft. Draft runs documentation validation and non-blocking editorial coverage; only an explicitly reviewed draft-to-main merge may publish. No automatic PR merge or engine release is authorized.

The final user-approved Engine API scope is public/protected C# contracts owned by src/ and build/ only, excluding declared tests and fixtures. Every other root is excluded. Platform implementation project guides are also retired; shared contracts and availability limits remain. Script categories come from Runtime, Editor and Authoring exports, separately from compilation surfaces. Every type/member has a Markdown source: xml or absent means generator-owned; manual protects prose. Declarations always come from captured facts. Missing editorial prose is a neutral check, not failed validation.

## Baseline and protected work

- Engine: ai-pipeline, 6d51527e7aeeb40c321b508209794b8c73255d81.
- Docs: docs-modernization, local c06d2c6, including the unpublished automatic promotion change.
- Preserve the other session's src/composition/editor/features/Inno.Editor.Assets/Properties/ScriptingApi.cs edit (SHA-256 b99daf9359b26ba1dd5eb1d46d69868bde8153b0c66ed037edcb4e8fa687ed61).
- Preserve historical evidence. Do not repeat the completed engine product matrix for documentation-only implementation.
- Local inventory and diagnostic artifacts belong to artifacts/acceptance/api-draft-pipeline; ordinary query/cache ownership remains unchanged.

## Execution and gates

1. Inventory symbols, authored pages, sources and callers before activating overwrite semantics.
2. Implement canonical API fragments, XML rendering, manual protection, composition and query scope.
3. Migrate all eligible content and links; remove excluded API reference while retaining integration guides.
4. Add independent API export/import, neutral editorial reporting and draft/main publication.
5. Remove paired-candidate requirements, obsolete callbacks and duplicate renderers; update rules and skills.
6. Verify source cases, mutation/race failures, website and query equivalence, actual CI permissions and deployment.

## External completion gates

GitHub App configuration, protected workflow activation, manual production PR merges and successful Pages publication require actual evidence. Missing access is a blocker, never a local-test pass. The original Windows/macOS engine acceptance is not rerun merely because this record changes. Engine Draft CI tiering remains a separate task.

## Implemented local foundation

- Actual export-attribute Scope capture, including foreign owner declarations and compiler-produced projection origins.
- Per-symbol XML/manual ownership, complete captured-owner checks, immutable facts, canonical type/member composition and Scope queries.
- Complete hashed API package/import with rollback, repeat-content no-op and protected publisher logic using ordinary draft pushes. The publisher never executes Docs candidate code or pushes main.
- Ordinary engine validation no longer requires a paired documentation candidate; missing Native prerequisites remain blocked.
- Updated engine instructions and four skills; six inherited documentation clauses have explicit user-approved amendment mappings while all original source identities remain preserved.

Local evidence under artifacts/acceptance/api-draft-pipeline:

| Check | Observed result |
| --- | --- |
| Documentation library regression suite, documentation-tests-07.log | 101 passed, 0 failed, 0 skipped |
| Source contracts and executable examples, extraction-tests-03.log | 12 passed, 0 failed, 0 skipped |
| Git publisher fixture, publication-tests-04.log | Complete draft push, untouched main/Manual, repeat no-op and superseded-export rejection passed |
| Hosted engine planning fixture, hosted-planning-tests-02.log | 7 cases passed |
| Rule mapping, rule-check-01.log | Original rule identities retained; complete-unit staged loading passed |
| Real local managed capture, managed-export.log | 136 prepared owners captured together |
| Actual script references, runtime-script-export-02.log / editor-script-export-02.log | 3,475 Runtime-context and 3,123 Editor-context declarations exported |
| Large corpus fixture, corpus-sync-01.log | 18,331 symbols synchronized; not full platform coverage |
| One warm corpus synchronization, corpus-warm-01-time.json | 23,456.9096 ms, zero changed files; one diagnostic sample, not the required five-sample performance acceptance |

The corpus fixture uses explicit zero revision identities and a reduced prepared-owner inventory. It is not a publishable package or a substitute for complete platform acceptance. macos-managed-reference-build.log preserves the failed attempt: actual target binding generation requires a macOS SDK and cannot run on Windows. No missing references were replaced with fake or copied assemblies.

## Still required

The foundation table above is historical. The [current acceptance record](../acceptance/API_DRAFT_PIPELINE_2026_11_13.md) and the Docs acceptance record distinguish later completed migration, source/example decoupling, legacy removal, browser checks and warm measurements from outstanding work.

1. Completed the selected Windows/macOS Debug package at `6eca1055adf859efd335bccb7761552ec5de9fd6`: two successful producer artifacts form seven explicitly Debug API contexts. Import synchronized 18,336 symbols and preserved all six manual files byte-for-byte. The user removed broader build acceptance; the remaining Release producer and full Engine run were canceled, not certified. Production export retains its normal configuration matrix.
2. Verify hosted initial example preparation, complete Docs validation and its exact site artifact against the new API package. Previous example receipts cannot certify a changed tooling/API input.
3. Implemented per-owner authored C# extraction reuse using frozen source/reference/XML inputs, embedded extractor identity, verified immutable payloads and existing owner/lease/retirement protocols. Functional regressions cover same-length source edits, inherited XML changes, corrupt cache quarantine and cross-workspace payload import without copied ownership. Ten targeted cache/example regressions passed after the final changes. Script projection remains a fresh extraction; no additional performance samples are required or claimed.
4. Activate and exercise protected editorial reporting, automatic master-to-draft synchronization and manual draft-to-main deployment. Required App credentials and human production merges remain external actions. Branch protection and Pages configuration alone do not establish an actual successful publication.
5. Record the resulting hosted evidence, source/manual-prose preservation, final legacy inventory and remaining external blockers. Keep exact first failures and retire only this task's owned intermediate artifacts.

Existing feature and migration branches have not been merged. Do not mark this plan complete from local tests or an incomplete producer set.
