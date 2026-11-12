# Protected documentation evidence relay

The engine adds a separate protected workflow that inspects a completed Engine CI run and requests the fixed Docs source-evidence receiver. It never checks out or executes the candidate in the credential-bearing job. The ordinary repository token reads engine metadata; a restricted installation token has Actions write only in InnoEngine.Docs for dispatch.

The receiver independently authenticates the exact source producer and artifact digest. Candidate Docs jobs use only their own repository's read token to consume the imported certificate. They do not receive an installation key or token. The Docs pipeline separates website projection/browser acceptance from source certification, allowing late evidence to resume the source job and its aggregate while retaining the original static files and first failed attempt.

## Acceptance

Fourteen engine protocol cases passed: exact successful producer, internal PR input, forks, changed workflow, failed/unfinished execution, untrusted branches, stale PR metadata, missing aggregate/certificate, expired or unhashed artifacts, wrong attempt and wrong producer. Evidence: `artifacts/acceptance/english-documentation/relay-protocol-tests.log`.

Engine rule routing and byte/provenance validation are recorded in `relay-rules-final.log`. The new PowerShell fixture is discovered by the existing CI-contract runner; no separate test inventory was added.

The Docs implementation separately tests artifact selection, partial-job resumption, stale head/base rejection, credential separation and retained preview identity. Its acceptance record supplies the actual results.

## Boundaries and pending work

This is implementation and local protocol acceptance, not a successful hosted dispatch. GitHub App credentials remain unconfigured at repository level, organization credential inspection was denied, and the protected receiver/default-branch workflows are not merged. Manual bootstrap is restricted to the explicitly approved migration branches. No PR was merged or made ready, no engine release was published, and #75 remains open.

Engine CI run 43164720103 continues against 7e12d767 and Docs 01bc7e4. It must finish on its original inputs; these follow-up changes do not retroactively belong to that run. Final paired publication, complete source/example certification, default-branch delivery, and the planned remaining acceptance require their own matching evidence.

## Incremental producer selection

The relay now inspects eligibility with the ordinary read-only repository token before acquiring App access. A successful incremental run without complete source evidence returns explicit NotApplicable and does not dispatch or demand unused credentials. A missing certificate on a producer claiming full certification, or a malformed/expired certificate, remains a failure. Full Engine validation also rejects incomplete source/example certification directly. The dispatch path reauthenticates eligibility after access acquisition.

All nineteen relay protocol cases pass in `relay-selection-tests.log`, including successful selection without a dispatch credential, incremental absence, full-certification absence and invalid retained artifacts. No workflow dispatch was inferred from these tests. Browser inspection of organization App settings redirected to GitHub sign-in; it did not establish an authenticated organization setup session.
