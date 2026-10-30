# Editor UI script-cache publication correction

Issue: [#55](https://github.com/Wolfiora/InnoEngine/issues/55), parent [#27](https://github.com/Wolfiora/InnoEngine/issues/27). This is focused correction evidence; complete corrected-source product, quantitative and CI acceptance remains a separate final gate.

## Confirmed failure and diagnostic limits

At `5e329bbe96e4812fcec1207151dd98c520237bbd`, eleven preparation stages and Contracts, Workflows, Visual and Lifecycle passed. The subsequent formal companion script/IDE command failed in `ScriptCompilerEngine.CommitGenerationCache`: raw `Directory.Move` reported Windows access denied for an owned staging directory. The failed command is preserved; a successful explicit repeat does not turn it into a passing execution.

Evidence owner: `D:/InnoEngine-Verification/editor-ui-closure-2026-10-27`. Original log: `candidate-5e329bbe/companions/Canvas/generation.log`. Independent diagnosis and focused regression evidence belong to `script-publication-investigation/`.

Public Roslyn 5.9.0 probes used a small emitted assembly and a copy of the actual companion assembly. File-backed references, owned metadata images, explicit metadata disposal and `AssemblyName.GetAssemblyName` did not reproduce the rename failure. A public BCL control holding a readable file with delete sharing reproduced directory rename denial in three of three trials. These observations establish a real filesystem condition; they do not identify the original reader. No antivirus, Roslyn or BGCS cause is asserted.

## Actual ownership correction

- Core IO already owns atomic directory publication, bounded filesystem rename handling and rollback-safe replacement. Its existing two-second/25-millisecond Windows policy is unchanged.
- Compiler now uses `AtomicDirectory.Publish` for an absent destination and `AtomicDirectory.Install` for an existing invalid cache tree. A complete valid cache is still reused. Best-effort private staging cleanup no longer serves as destructive replacement of a published cache tree.
- The existing compilation token reaches both assembly and generation commits. `Install` has one optional cancellation parameter, with no compatibility overload. Cancellation can interrupt preparation and precommit rename waits. Restoration deliberately does not use the canceled request token. A committed cache is not canceled afterward.
- Postcommit backup cleanup failure remains `DirectoryCleanupException`; installation plus restoration failure remains `AggregateException` with both failures and the preserved old-tree location. Cache completion is distinct from runtime generation activation.
- `Inno.Scripting.Compiler` references Core IO as a private implementation dependency. It gains no dependency on Build, MSBuild, named platforms, BGCS or Editor Panels. No new public Compiler API, cache manager, retry loop, event system or identity table is introduced.

## Public-boundary regression evidence

| Evidence | Actual result |
| --- | --- |
| `compiler-publication-before-exact.log` / `.trx` | The public compiler regression failed before correction with the same raw `Directory.Move` denial and preserved original stack. |
| `compiler-publication-after.log` / `.trx` | The identical temporary-reader regression passed after Core publication reuse. |
| `compiler-publication-final.log` / `.trx` | All 17 Compiler tests passed, including temporary staging observation, canceled publication, persistent blockage and invalid-tree replacement failure followed by explicit repair. |
| `core-publication-suite.log` / `.trx` | All 41 Core IO tests passed, including prior cancellation with/without an existing destination and cancellation after the old tree is evacuated. Rollback restores the previous bytes while the request token remains canceled. |
| `roslyn-directory-probe.log`, `roslyn-actual-payload.log`, `assembly-name-probe.log` | Independent public API probes and the reproducible BCL control; retain all observations rather than attributing an unproven cause. |

The compiler tests discover artifacts through public results/progress and ordinary filesystem observation inside their own fixture. The replacement test compares the entire existing tree after rejection, then compares repaired output against the original complete bytes. Cancellation and persistent-block tests preserve another completed generation and require a subsequent explicit request to succeed. All owned compiler work drains before fixture retirement. No IVT, private reflection, production test hook, forced GC or process termination is used to make publication succeed.

The Windows sharing fixtures are explicitly unexecuted on other hosts; they do not silently return a passing result. Portable Core replacement behavior and ordinary Compiler suites remain applicable across hosts.

## Remaining final gate

Five independent cold companion copies and fixed-input warm publication, formal companion projections, complete UI/product/lifecycle/visual acceptance, controlled paired performance, architecture/XML/Wiki and exact delivery-head CI must pass on the corrected candidate. Their results and source identity belong in the final acceptance report. This focused report does not mark #55 Verified or make PR #26 Ready by itself.

## Deep self-review

The change removes a duplicate, weaker filesystem transaction from a real execution chain and reuses Core IO at the actual publication boundary. It preserves Compiler ownership of validation/staging/cancellation, Core ownership of rename/rollback, and Host ownership of activation. There is no platform policy in Compiler and no BGCS, package, vendor or generated-binding modification. Cancellation does not disable restoration; cleanup failure is not confused with failure to commit. Complete final acceptance remains required before claiming plan closure.
