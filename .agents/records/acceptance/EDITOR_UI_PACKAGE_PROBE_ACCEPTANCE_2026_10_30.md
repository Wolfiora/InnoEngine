# Loaded-package probe ownership

This focused #16/#27 record does not close the complete UI plan. PR #26 remains Draft pending final applicable suites, products, measurements, documentation and CI.

## Observed failure

The complete Contracts run at `1917b253` reproduced three ordinary `NuGetLoadedIdentityTests` cleanup failures. `Directory.Delete` reported `UnauthorizedAccessException` for fixture-owned `BGCS.Runtime.dll`; the finalizer cleanup can obscure an earlier probe failure. The isolated case passed in the same run. Package file attributes were observed as Archive, not ReadOnly. No BGCS defect, independent native-loader defect or antivirus cause is inferred from these observations.

Original logs/TRX are retained under `candidate-1917b253/Contracts/5696b1bd510d4465ae1544b4b6e61c05` in `D:/InnoEngine-Verification/editor-ui-closure-2026-10-27`. The run remains Failed, not replaced by its later focused passes.

## Correction

The existing test-process invocation pattern now owns the physical negative DLL load. Parent cases still perform real SDK Restore, consume the evaluated ProjectAssetsFile, preserve stale/default versus independent output semantics, and compare the actual loaded expected runtime with its package content. A fresh writable copy is created from package bytes; the package itself is never modified.

The owned child uses the existing public test entry, physically loads the same-name/same-MVID/different-content DLL through AssemblyLoadContext, verifies content rejection and performs the unchanged full-GC/finalizer/full-GC plus weak-context/weak-assembly assertions. A verdict marker is written only afterward. The parent requires that marker and successful child completion before normal directory deletion. No process-isolation assertion, empty discovery, timeout, cleanup error or package mismatch is ignored. No LoadFromStream, IVT, private reflection, production API or platform branch is added.

ToolchainEnvironment and DotNetSdkEnvironment remain the existing process/SDK boundary. The change is confined to the test fixture and this evidence record; production code remains exactly `1917b253`.

## Focused results

- Independent BCL process-ownership prototype:10/10 actual mismatch/unload/exit/deletion trials pass.
- Ordinary SDK fixture build: succeeds. All four declared public fixture cases pass.
- Existing `Invoke-IsolatedTests.ps1`, selected single-project solution filter and common NuGetLoadedIdentity filter:10/10 entry runs pass. Each run executes three shared scenarios and one required isolated scenario, including their child probes:40 parent scenarios total. Actual restoration, content, MVID, weak collection, nonempty verdict and cleanup checks remain enforced.
- Evidence: `e16/process-owned-fixture/identity.trx`, `repeat-0` through `repeat-9` execution summaries/TRX; `e16/process-image-owner/trials.log` is prototype evidence only.

Final full-suite and exact-head CI verification remain required. No unrelated engine issue or complete-plan requirement is marked resolved by these focused results.
