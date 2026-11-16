# Documentation binding in engine certification

The matrix's final receipt now carries the already-established hosted documentation pair. Reconciliation requires that its source commit/tree, documentation snapshot/content identity, exact Docs revision and tooling revision agree with the uploaded plan. A missing or stale sidecar cannot produce a certificate. The per-platform runner and aggregate both use the same reconciliation implementation.

This supplies the artifact-only Pages promoter with exact engine/documentation correspondence. It does not make unverified source API facts verified, certify other platforms, or grant publication permission. The Docs producer's complete source/content gate and the live successful engine matrix remain separate prerequisites.

Local acceptance: 31 aggregation cases and five CI retention protocol cases passed. New cases cover correct pairing, missing pairing, pairing without a declared input, and invalid commit/tree/snapshot/content/revision fields. Raw results remain under `artifacts/acceptance/english-documentation/publication-report-tests.log` and `publication-retention-tests.log`. These protocol fixtures do not replace real Windows/macOS execution.

The paired Docs implementation documents and tests authenticated artifact selection, exact archive/file identities and separate deployment permissions. Actual restricted installation provisioning, source verification import and a live deployment are still outstanding.
