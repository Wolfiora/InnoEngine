# API draft publication implementation

Status: In progress. This record supersedes the paired-PR publication design for documentation; historical acceptance remains historical.

## Approved behavior

Engine master produces an independent, complete C# API export. A protected writer imports it into InnoEngine.Docs draft. Draft runs documentation validation and non-blocking editorial coverage; only an explicitly reviewed draft-to-main merge may publish. No automatic PR merge or engine release is authorized.

The API scope is product and tooling public/protected contracts, excluding backends, extern, non-C#, tests and fixtures. Script categories come from Runtime, Editor and Authoring exports, separately from compilation surfaces. Every type/member has a Markdown source: xml or absent means generator-owned; manual protects prose. Declarations always come from captured facts. Missing editorial prose is a neutral check, not failed validation.

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

Complete independent export preparation/CI, all target contexts, full existing-content migration, source/example receipt decoupling, protected neutral-check reporting, actual draft/main workflows and permissions, complete legacy cleanup, visual acceptance, five warm samples and real hosted deployment/synchronization evidence. Existing feature and migration branches have not been merged. Do not mark this plan complete from the local foundation tests.
