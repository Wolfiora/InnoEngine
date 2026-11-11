# Live validation process progress

## Problem and change

ValidationProcess drained child stdout/stderr into the owned log and retained returned stdout, but the runner primarily printed each check after it completed. A long native build therefore looked quiet in the Actions console even when its detailed log was advancing.

Logged process operations now print their identity, PID and log location when started, then report every 30 seconds: elapsed time, output character count, age of the last output, a bounded single-line output excerpt and the parent process's CPU time. Exit reporting includes the exit code. Check selection and the initial workspace/input phase are visible before their work starts. Unlogged metadata queries retain their existing output behavior.

This is diagnostic progress, not a completion deadline or automatic stall verdict. Parent CPU does not measure child-process CPU; a quiet parent can be waiting for an active compiler. The complete original output remains in the log, and command arguments or standard input are not copied into telemetry. Control characters in the short excerpt are replaced so child text cannot start a new workflow-command line.

The periodic reporter is canceled and joined after output draining, including cancellation and process failure. Existing process-tree cancellation and retained first-failure behavior remain authoritative. No engine product behavior or public signature changes.

## Validation

Using .NET SDK 10.0.402 on Windows, the managed validation test project and its real dependency closure compiled. Two focused tests passed, none skipped, in 46 seconds:

- A real PowerShell child emits stdout/stderr and then waits. The public ValidationCommand reports activity while it is still running. Cancellation drains the process, retains both output streams, records Blocked and leaves completion false. The test also rejects an unprefixed child workflow-command line in progress output.
- An existing failing contract still blocks its dependent check while an independent policy check executes successfully.

The test's finite harness wait prevents a broken regression from hanging the test run. It does not impose a build deadline. No Native product preparation or full engine suite was needed for these process-runner changes.

Raw build/test output and TRX are retained under `artifacts/acceptance/english-documentation/validation-progress*`. Rule routing retained all 241 original identities and loaded the applicable complete units. Full remote Windows/macOS validation and the running older candidate remain separate obligations; this local test does not certify them.

## Architecture review

The change stays inside the existing validation process owner and does not add another executor, public test hook, process registry, timeout policy or checker inventory. Reporting does not change evidence status, ownership acquisition or prerequisite selection. Remaining broader migration, precision and deployment work continues under the approved plan.
