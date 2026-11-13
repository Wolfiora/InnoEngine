# Source certificate normalization

## Reproduced failure

[Full run 43224643776](https://github.com/Wolfiora/InnoEngine/actions/runs/43224643776) tested `4471fe4d341195c61d36ecaf858ff74466b7f892`. Its Windows and macOS producers succeeded. The aggregate failed in `ConvertTo-Json` while serializing the source certificate. This remains a failed hosted aggregate; platform success is not a substitute for aggregation.

Each executable example embedded the entire compilation input closure. The two authenticated diagnostic archives reconstruct 352 API input proofs and 267 example contexts. Compact serialization of the repeated input payload alone would require **3,879,637,808 bytes**. Increasing runner memory or retrying the platform builds would not remove that duplication.

## Final mechanism

`inno-documentation-source/2` stores each path/content pair in `sourceFiles`, exact closure membership in `sourceSets`, and each example compilation context in `inputGroups`. API proofs reference source sets. Example pages and script comparisons reference input groups bound to platform, configuration, target and successful report identity. All other input fields, source identities, dependency packages and comparison provenance remain intact.

The producer validates and normalizes each execution group once. Compatibility checks resolve and validate a group once per current platform plan, retaining the original report and prior-certificate provenance. PowerShell, the structured query library and the website evidence checker reject missing references, path escapes and context substitution. Retired schema 1 is not accepted or automatically upgraded. The normalized metadata import boundary is 16 MiB; exceeding it is a visible failure, never a truncated proof.

Failed aggregate reconciliation now retains its already-produced input receipt for diagnosis. A failed job still cannot become a trusted certificate source.

## Actual acceptance

- 47 producer and shared-table rejection cases passed, including 1,000 pages sharing the same closures.
- 27 unchanged-input compatibility cases and 23 authenticated baseline cases passed.
- All 77 documentation query/library tests passed, including exact script surfaces, missing projection proof, malformed shared tables, cancellation and digest tampering.
- All 199 Docs Node tests passed. The affected evidence/projection tests cover missing groups, negative references, changed reports/platforms, corrupt source identities and escaping paths.
- Build CLI compiled with zero warnings and errors. The 3,656-page website built successfully; the canonical/rendered projection check passed with no errors.
- The actual full-run receipts were reconciled locally without rerunning engine builds or tests. Field-by-field decoding preserved all 352 API proofs, all four example input groups and all 267 example contexts. There are 4,351 shared file identities and 342 source sets.
- Final generation took **55.928251 seconds**, producing **6,317,318 bytes** with a process peak working set of **1,076,359,168 bytes**, including parsing the original large receipts. This is one generation observation, not a five-sample benchmark. A separate real-certificate read through the public query API took 294.683 ms; website example verification covered all 134 required pages in 46.5372 ms.

The final local certificate identity is `cc83ae4b6e3fb6b26e0c1e979ba137165d4cbdb8e35b0b4ccc28d1e8bf9a30dc`; its checker policy is `44b06881528a0e1bce37f830174d396138012c1ed4426d4e134bd0831cecb810`. It describes the original tested source tree `797afce049f6f4fb1daffdd4a91bcd29347898de`, not a later engine revision. Local reconstruction is not an authenticated hosted import and does not retroactively change the failed run's conclusion.

## Evidence and remaining boundary

Original diagnostic archives are retained under `artifacts/acceptance/english-documentation/` with authenticated digests:

| Platform | Artifact | SHA-256 |
| --- | --- | --- |
| Windows | `13973537819` | `9db61b4423a95f187fdff40d476ca7e551ed18fda940da4073bcbbfa0fb227f5` |
| macOS | `13963129886` | `1b4417d729a8cb6d6de76357f7128d88c48f729bb25074e46a332a4b88eed747` |

`full-reconciliation/` retains exact plans, catalogs, results, reconciled receipts, the original normalization sample, final certificate and full input-equality report. `full-aggregate-43224643776.log` preserves the original memory failure. First failures are not deleted.

At this checkpoint, the later actual-script candidate run `43264340393` is still executing. This repair needs hosted follow-up; the successful local reconstruction does not certify that later candidate. Protected cross-repository import, prepared-candidate merge ordering and Pages promotion still require the restricted GitHub App and protected default-branch workflow activation.

## Subsequent hosted reconciliation and execution-target correction

Run `43293893886` at candidate `a6e294ea097024f4b1fcc4b4d43a406b6868bd7f` completed successfully on both platforms and in `Engine validation`. Its tested merge was `5eef7dac945a739a48bfa76ad5d8427b20f4af3c`, tree `9d0d2da1e089a3850ea64ab12c27dc0ca98f4c52`. Windows reconciled 339 passed and three not-applicable checks; macOS reconciled 337 passed and six not-applicable checks. This closes the hosted serialization-memory regression.

The independent prepared-snapshot consumer then rejected that run's certificate: four script projection proofs omitted their execution target, although their shared input groups correctly named Windows x64 or macOS ARM64. Platform-neutral reference identity does not make the producing execution platform-neutral. The producer now retains the execution target alongside configuration, report identity and input group. Consumer checks remain strict.

The original authenticated certificate (`cc0a5f06cb0bb90da995452bdebfeb04ae872aba0b12614eb70c2f6f9e6765a2`) is preserved under `artifacts/acceptance/english-documentation/final-source-certification/`; it must not be represented as successfully consumed evidence. The follow-up passes 48 producer cases, including real target-specific script contexts, 27 compatibility cases and 23 authenticated baseline cases. Exact-report reconstruction and final candidate reconciliation remain required at this checkpoint. No engine behavior or API declaration changed.

A local diagnostic copy that restores only those four execution targets from their existing input groups was accepted by the unchanged public .NET reader and complete website checker: 3,656 pages, 252 projects, all 18,874 selected declarations and 134 executable-example pages. This isolates the producer omission; the diagnostic copy is explicitly not an authenticated hosted replacement. Original certificate bytes remain unchanged.
