# Owned cache retirement and failure reporting

The user requested continued modernization and urgent C-drive cleanup. This operation used registered publishers, full dependency protection and the existing exclusive retirement protocol. It did not adopt arbitrary directories, remove reusable compilation workspaces or rewrite unverifiable ownership records.

## Inventory and actual cleanup

- Docs cleanup retired 5,070 obsolete query-cache candidates with 856,122,776 logical payload bytes. The published snapshot and its 22,785,695-byte protected closure remained available.
- The engine dry run inventoried 97,689,909,322 logical cache bytes. A separate proposal with no byte pressure selected only `ProcessWorkingDirectory` artifacts: 1,470 candidates totaling 70,096,685,608 logical bytes. Every candidate resolved inside the explicit engine checkout; the adapter asserted every candidate's actual registered role before execution.
- Engine retirement removed the idle process payloads. A subsequent complete inventory contained 27,593,223,714 bytes and no process-only candidates. Task load directories fell to zero; six protected/unselected publisher directories remained. Reusable managed/validation workspaces, immutable native artifacts and current publication roots were not selected.
- Logical bytes are not recovered disk space: private runtime files use hardlinks. The observed volume free space changed from 37,354,942,464 to 39,019,433,984 bytes (about 34.79 to 36.34 GiB). Other processes can affect this observation. Do not report the 65.28 GiB logical retirement as physical recovery.

## First failure and correction

The engine payload retirement completed before metadata retirement encountered an unverifiable former process claim in `artifacts/build-tools/managed-inputs/requests`. `InvalidDataException` escaped the metadata boundary, losing the completed result rather than returning its failures. No ownership record was edited to force cleanup.

The toolchain now records metadata-integrity failures in `BuildCacheCleanupResult.failures`, retains the uncertain metadata and continues independent metadata retirement. Completed payload results survive. Explicit maintenance still returns failure; corruption in the initial protection graph still prevents payload retirement. No Core.IO process-identity behavior or product runtime changed.

The real follow-up returns zero candidates, zero additional retired bytes and one visible metadata failure for the same owner. That claim is unresolved and retained, not a successful cleanup certification.

## Verification and evidence

The managed Toolchains suite initially passed 74 existing tests and failed two new fixtures. The fixtures incorrectly retained the live test process stamp, so they did not reproduce the former-owner failure. Correcting only the fixture's timestamp to model a prior lifetime made both focused tests pass (248 ms). Assertions were retained: completed payload bytes, independent metadata cleanup, unchanged invalid claims, explicit maintenance failure. The production implementation was unchanged between these runs. No Native product was prepared.

Engine evidence: `artifacts/acceptance/english-documentation/cache-retirement.log`, `cache-retirement/cache-retirement.trx`, `cache-retirement-fixture-corrected.log` and `cache-retirement/cache-retirement-fixture-corrected.trx`.

Docs evidence: `artifacts/owned-cache-clean-executed.log`, `owned-process-clean-dry-run.log`, `owned-process-clean-executed.log` and `owned-process-clean-reconcile.log`. The disposable maintenance adapter lives under the ignored Docs `artifacts/cache-maintenance/` owner; it calls public Toolchains APIs, retains the complete owner set and is not a new daily executable project. Its first compilation error used an internal overload; the corrected adapter uses the public catalog API without changing production visibility.

The ordinary Build CLI rejects a Docs checkout before cache dispatch. The existing maintenance application accepts registered workspaces directly. Generalizing that daily command and arranging maintenance after validation-owned child processes exit remain workflow work; read-only document queries must not acquire cleanup/update locks or scan unrelated engine caches.

An earlier direct cleanup request for the Identity example's bin/obj was rejected by automatic approval review with the reason "blocked by policy". It was not retried through this mechanism. Original failure logs, historical evidence and the current Docs snapshot remain retained.

Overall modernization, content migration, cross-platform CI, paired publication and Pages deployment are incomplete.
