# Independent documentation merge gate

The final review found that input selection and source certification did not independently require a successful, prepared Docs candidate before engine merge. The protected reconciliation workflow now publishes **Documentation pairing** on the actual PR test-merge or merge-group commit. It does not build source or wait inside Engine validation, avoiding a certification dependency cycle.

The engine reads only the exact Docs binding and authenticated Actions metadata through a Docs-scoped read token. It requires the candidate to be in the protected Docs default branch, the latest matching Documentation run to succeed, exactly one successful aggregate, and the current attempt's retained verified site artifact and digest. PR head, base, merge commit and metadata are checked again before success. Candidate changes, missing access, unmerged content and missing evidence remain blocked. Metadata/callback reconciliation refreshes all open candidates so coalesced GitHub notifications cannot lose an earlier candidate's refresh. Merge-group checks use their own concurrency identity.

The Docs protected completion callback authenticates its producer before requesting a fixed engine workflow dispatch. Its token cannot write checks. The engine independently revalidates the candidate; receiving a notification is not proof. The protected engine workflow never checks out PR or merge-group code. Its ordinary workflow token writes the candidate check; App credentials only read Docs evidence. If token acquisition fails after checkout, reconciliation still records blocked pairing.

Docs now confirms that the tested merge tree equals the immutable head selected by pairing. A changed base cannot be certified under the old head identity. Publication records that confirmed identity instead of confusing GitHub's temporary merge SHA with the artifact producer's head.

## Local acceptance

- 18 exact evidence scenarios passed, including failed/canceled/running and newer failed runs, wrong source tree, unmerged candidate, forks, missing/duplicate/skipped aggregate, expired artifacts and wrong attempt/producer.
- Nine status/race scenarios passed, including PR head/base/merge/metadata changes, closure, missing merge/access and the exact merge-group candidate.
- Four real Git candidate-tree scenarios passed: identical revision, different commit with identical tree, changed tree rejection and nonimmutable target rejection.
- Docs Node regression passed 168 cases, including authenticated completion callbacks and protected workflow boundaries. No cases were skipped.
- The first status-fixture failure remains in `artifacts/acceptance/english-documentation/pairing-status-tests.log`. Its mock used PowerShell's `?` wildcard for a literal query separator, intercepting individual PR reads. Correcting that mock preserved all race assertions; the final output is `pairing-status-tests-03.log`. Production behavior was not weakened.

The unified engine CI contract discovery includes the new PowerShell suites. Docs candidate identity checks run alongside archive checks. Local protocol success is not a hosted pairing result.

## Activation and preserved acceptance

The required status must be configured as `Documentation pairing` from GitHub Actions alongside `Engine validation`, preserving strict base freshness and review/discussion requirements. Activation requires the protected workflows on their default branches and restricted App credentials. Those external prerequisites remain unresolved; the added requirement is not yet claimed as enforced. Neither PR is automatically merged. Pages and draft-release exercises remain separately outstanding.

Hosted run 43164720103 predates this change and continues without cancellation. Its results cannot certify this implementation. The original Windows archive remains intact. A read-only review of its TRX attributes much of the validation-tools suite's duration to actual isolated engine/SDK integration cycles: compilation-input mutation took 19m38.639s, union ownership/failure took 17m02.383s and host/TRX accounting took 14m24.992s, with overlapping execution. These durations must not be summed as sequential wall time. Each test intentionally exercises multiple real validation cycles; no assertion or execution was removed to improve a timing claim.
