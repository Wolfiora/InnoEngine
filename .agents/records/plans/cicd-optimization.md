# CI/CD and test ownership implementation

The implementation is delivered on `cicd-optimization` from `8d5ddc8a00463852667170afcb4c33986ee0767c`, the head of the closed docs-navigation PR #84. The migration baseline is `32cd34c21e4bc0c29d9989cf28c745bd1316fecb`.

| Responsibility | Delivery |
| --- | --- |
| Platform ownership | WindowsX64 owns the Windows host implementation. Host architecture comes from the execution environment. |
| Test ownership | Repository-owned tests and fixtures live under `tests/`. The inventory identifies the destinations of all 2,190 original methods. |
| Backend artifacts | CI resolves the SDK and native closures from an immutable prerelease catalog, builds changed owners, and distributes one verified ZIP per target. Local builds use source. |
| Validation | The managed planner selects owners, prerequisites, environments and shards. Platform workers certify their own results. The stable PR and merge-group gate verifies the expected matrix. |
| Cadence | Ordinary branches use fast checks. PRs and master select affected behavior. Nightly verifies complete acceptance. Weekly executes warm sampling and lifecycle checks. |
| Release and documentation | Release continuation uses matching evidence. API export retains its independent preflight and Docs draft publication boundary. |

For the original component-package baseline, full hosted dependency certification ran in `44265138920`, cold consumption of the published release in `44301436204`, and scoped weekly sampling in `44271552325`. The final single-owner PR median was 234s against 189s on the previous workflow. See the measured results for the ten-minute and relative reduction targets. The default branch activates the renamed scheduled and manual workflows after a maintainer integrates the branch.

[Acceptance and measured latency](../acceptance/cicd-performance.md)  |  [Test migration](../acceptance/cicd-test-migration.md)  |  [Class and method inventory](../acceptance/cicd-test-inventory.json)
