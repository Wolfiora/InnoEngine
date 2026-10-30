# Atomic create-only file publication

Tracking: [#48](https://github.com/Wolfiora/InnoEngine/issues/48), [#47](https://github.com/Wolfiora/InnoEngine/issues/47), [#40](https://github.com/Wolfiora/InnoEngine/issues/40), [parent #27](https://github.com/Wolfiora/InnoEngine/issues/27).

## Confirmed failures

At `bb5b4bb9`, the complete Windows and macOS CI suites rejected the output-owner fixture's hard-coded `bin/Fixture/debug` path. The SDK builds succeeded. Product-aware `ArtifactsProjectName` now participates in the actual output owner, so the fixture must inspect SDK outputs rather than restore an obsolete path convention.

The same macOS ARM64 run reproduced a separate production defect: competing new-project layout initializers observed different winning documents. Core `AtomicFile` delegated create-only installation to `File.Move(..., overwrite: false)`. The exact [.NET 10.0.13 Unix implementation](https://github.com/dotnet/runtime/blob/v10.0.13/src/libraries/System.Private.CoreLib/src/System/IO/FileSystem.Unix.cs#L193-L214) can check destination absence and then perform a replacing rename. That sequence cannot satisfy Core's stronger atomic no-clobber promise.

This is not a BGCS failure. No SDK, package, BGCS, generated binding or vendor file was modified.

## Correct ownership and implementation

- Core IO retains its existing portable atomic publication responsibility. Editor, asset creation and document consumers all receive the correction through the same API.
- Windows retains no-replace file movement. Other hosts must provide the public POSIX `link` filesystem primitive: a complete same-directory candidate gains its destination name atomically, then the candidate name retires.
- Native host details remain private to Core IO. There is no engine platform ID, backend, target, product routing table, new provider registry or Editor-specific lock manager. The existing platform ownership rule already distinguishes portable Core IO primitives from product policy.
- An existing file, empty file, directory or competing creator cannot be replaced in create-only mode. Losing `Install` candidates remain caller-owned. `WriteAllBytes` owns its staging file and cleans it on exit.
- Missing host primitives fail with `PlatformNotSupportedException` and their original cause. Unsupported filesystem operations fail with `IOException`; there is no copy fallback exposing partial destination content.
- Failure to retire a candidate after successful linking explicitly reports that the complete target has already been published. Parent and candidate ownership remain the caller's responsibility.
- The SDK fixture now reads `TargetPath`, `ProjectAssetsFile` and the actual `IntermediateAssembly` item through the SDK's structured result output. It checks existence, containment and disjoint owners while retaining two concurrent real SDK builds.

## Verification

Evidence owner: `D:/InnoEngine-Verification/editor-ui-closure-2026-10-27`.

| Gate | Result | Evidence and limits |
| --- | --- | --- |
| Core IO, Windows Release | 31 passed | `e48/io-publication-capability.trx`; final capability-based correction, no analyzer suppression |
| Simultaneous candidates | Passed within Core IO suite | 12 rounds, 16 independent publishing threads per round, exactly one complete winner, losing candidates preserved |
| Independent processes and reader | Passed within Core IO suite | Eight actual .NET publishing processes, public atomic API, 1 MiB distinct payloads, complete repeated reads, exactly one success, no staging leftovers |
| Existing owners and replacement | Passed within Core IO suite | Empty/custom destination, directory collision, normal replacement, parent boundary and existing filesystem safety contracts |
| Actual SDK output-owner fixture | Passed, Windows Release | `e40/sdk-owner-executed.trx`; two concurrent real SDK builds, actual output/assets/intermediate files, containment and disjoint owners |
| Real new-project layout concurrency | Pending final candidate | Earlier Windows passes do not substitute for the final source or actual ARM64 CI |
| macOS ARM64 | Pending exact-head CI | No local Mac; the observed red CI result is retained and will only be superseded by a real successful remote job |
| Architecture / Wiki | Pending final candidate | An intermediate explicit Browser check was rejected by the existing shared-layer policy and removed; capability absence now comes from the actual native boundary without a product-platform branch or validator exception |

An intermediate Task runtime build correctly rejected changed Core IO source during compilation. That failed preparation was not cached. The source is frozen for the replacement build; no input check or publication guarantee was relaxed.

Two SDK fixture preparation failures are retained: the first omitted the structured-result parent directory; the second omitted the explicit Build target, so SDK get* switches performed evaluation only. The final request explicitly executes Build and reads its resulting values. Neither attempt is counted as a successful build or a production defect.

## Deep self-review

The fix acts at the authoritative filesystem operation, not at the Editor symptom. It does not introduce a second IO/lease protocol, weaken atomic visibility, guess package/output locations or expand the public API. Independent process contention exercises a real public boundary. The Unix implementation still requires actual ARM64 CI; Windows success cannot certify it. Issues remain Open until complete verification and merge through PR #26.
