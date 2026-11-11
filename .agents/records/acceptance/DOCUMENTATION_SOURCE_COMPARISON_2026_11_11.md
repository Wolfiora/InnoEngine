# Batched source-reference comparison

Build `docs validate-api` compares selected source-clr manifests against one actual SDK-evaluated batch for their shared configuration and target. It reuses the existing API extractor and source inventory; no second extraction engine, Markdown parser, project-selection table or product preparation path was introduced.

The comparison covers complete symbol membership, declarations, XML, constants, source ownership and type relationships. It ignores source line offsets and the retired Wiki page field, and normalizes XML line endings. It rejects empty discovery, repeated owners/symbols, path escape/noncanonical ownership and mixed contexts. Missing compile references remain failures. Cancellation and changing reference inputs do not replace a complete report.

The report deliberately retains overall `Unverified` status. It proves only the selected source facts match. Full source-input integrity, trusted workflow provenance, examples, script surfaces, prose review, other targets and publication remain separate obligations. No status-label shortcut enables publication.

## Local evidence

Eleven `DocumentationReferenceTests` passed with zero skips. The real SDK fixture rejects product Build execution and exercises unchanged facts, XML-only changes without signature changes, source-offset changes, missing overloads, invalid required discovery, duplicate symbols, mixed target/configuration and cancellation preserving the prior report. Existing extraction and inventory tests remain intact.

The ordinary Build entry compiled with zero errors/warnings. Initial source-only architecture verification found one pre-existing two-parameter lambda-format violation in the documentation indexer. Its parameter layout was corrected without changing execution; first-failure output remains at `artifacts/acceptance/english-documentation/source-comparison-architecture.log`. The corrected source check is recorded separately.

Logs and TRX are under `artifacts/acceptance/english-documentation/source-comparison*`. This bounded change does not claim complete CI/CD, native/device execution, Pages deployment, legacy retirement or issue #75 completion.
