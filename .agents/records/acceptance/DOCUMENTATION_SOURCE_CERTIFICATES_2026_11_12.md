# Explicit documentation source certification

Master commit `259e8a63` (the logo PR merge) was merged into `ai-pipeline` as `bc916ab40d4d7176947886b6bb1fb2cb978c576d`. The resulting source tree remains `218e0198bf00fbce195c7f78662257e981d50184`. The unrelated `ScriptingApi.cs` working edit was preserved with SHA-256 `b99daf9359b26ba1dd5eb1d46d69868bde8153b0c66ed037edcb4e8fa687ed61`.

## Trust and ownership

Engine reconciliation now records exact passing source-comparison coverage, report identities and the canonical catalog identity. The aggregate combines coverage across actual platform receipts. Missing owner/reference coverage produces a blocked diagnostic and no source certificate. A complete certificate binds the engine source tree, project inventory, API payloads, contexts and owners, and retains its reconciled producer identity.

The query library accepts an explicitly selected immutable `DocumentationEvidence` value. Its digest must be authenticated independently during preparation; hashing arbitrary downloaded JSON is not authentication. Opening a session compares complete reference coverage and the project inventory. Ordinary reads without evidence remain Unverified. Matching evidence is returned separately as `sourceEvidenceIdentity`, without changing content identity, rewriting an index, making a network request or starting SDK work. Excerpts and necessary contract continuations retain their previous semantics.

The Docs transport imports the source artifact only from a successful exact engine workflow, verifies GitHub's archive digest, and supplies the resulting certificate to the canonical adapter. PR code receives no installation credential. Actual installation provisioning, successful complete engine coverage and live publication remain separate requirements.

## Local acceptance

- 68 documentation tests passed, including exact source/inventory/reference/context matching, missing and duplicate coverage, tampering, cancellation, concurrent reads, preserved query completeness, prose-only reuse and unverified defaults. Logs: `source-evidence-complete.log`; TRX: `query-cutover/source-evidence-complete.trx`.
- 16 source receipt reconciliation cases passed: `source-receipt-tests.log`.
- Nine combined source certificate cases passed: `source-certificate-tests.log`.
- 31 aggregate reconciliation cases passed: `source-receipt-aggregation-tests.log`.
- The Docs authenticated source selector passed four new transport cases in addition to its existing publication tests. Local API protocol fixtures do not establish real GitHub access.
- The first source-policy check found eleven XML/parameter-layout violations. Their formatting was corrected; the initial log is retained as `source-evidence-source-policy.log`.
- The corrected real source architecture check passed for all 252 projects (`source-evidence-source-policy-corrected.log`); all 15 hosted planning protocol cases passed (`source-evidence-ci-planning.log`).

## Observed CI latency

The retained canceled Windows run `43060870028` reached the managed validation tooling tests before cancellation; it had not started Native production or Support Pack creation. Its managed test log is preserved alongside the digest-verified original archive.

The real `ChangedUnionRootsRecompileWithoutCleaningUnchangedDependencies` case passed locally in 5 minutes 43 seconds, including three compilation cycles and the unchanged-dependency assertion. Its last managed build took 8.5 seconds, while the preceding compilation after restored dependency inputs changed took 57.3 seconds. These are distinct phases, not comparable end-to-end warm performance samples. Evidence: `union-progress.log` and `query-cutover/union-progress.trx`.

The fixture's isolated checkout was retired by its owner after completion. This result does not certify the other heavyweight integration cases or prove that all hosted delays have the same cause. The current hosted run still requires its actual outcome; canceled or queued macOS execution is not a pass.

## Outstanding acceptance

Full source coverage still requires actual macOS and Browser product captures and matching comparisons. The broader impact-analysis refinement, real Windows/macOS matrix, cross-repository automation, draft release acceptance and performance comparison remain open. Pages deployment may be unavailable under the user's explicitly allowed external exception; such an exception must be reported, not labeled deployed. This record does not close the modernization plan or issue #75.
