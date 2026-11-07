# Editor UI snapshot and product workflow verification

This is an intermediate acceptance record for #27, #35, #38 and #46. It does not close the complete approved UI plan or make PR #26 Ready.

## Implementation and ownership

- Core Serialization owns immutable, neutral `SerializationSnapshot` values. Repeated capture still executes all getters and converters. Copy-on-write only reuses already captured neutral nodes; it is not a change notification system.
- Snapshot values contain ordinary scalar/string/copied binary data and neutral structural metadata. They do not retain source instances, plugin types, converters, delegates, context services or generation leases.
- Scene documents own their latest observed snapshot/hash. The saved hash remains independent. Encoding and SHA256 use the ordinary binary format; failed capture/hash does not replace the previous valid observation.
- The writer borrows the converter generation's actual type snapshot for consistent Stable Type IDs. Nested state capture uses the same converter lease and complete owner context.
- Context storage uses a private immutable array of exact contract/value pairs. Replacement copies that small owned array without retaining the replaced service. Object nodes use the BCL generic OrderedDictionary for one ordered storage authority rather than a dictionary plus a duplicated entry cache. Array enumeration and component callbacks reuse operation-local mechanics. Graph membership comes from the actual capture owner; subtree boundaries remain distinct from the complete reference map.
- No additional serializer format, History stack, event bus, identity registry, mutation tracker, platform branch or public test hook was introduced. BGCS, vendor sources and generated bindings were not modified.

## Required public surface

`CaptureSnapshot<T>` and `SerializationSnapshot` allow another assembly to retain the exact neutral capture without retaining its source. `ToArray` and `WriteTo` provide owned bytes and caller-owned streaming respectively. `SerializationWriter.WritePropertiesData` captures nested independently restorable state with the current converter lease. Its borrowed `typeSnapshot` avoids an independent live type-generation decision. Node representation and reuse remain internal; snapshot/type-generation APIs are not added to the script surface.

## Recorded evidence

Evidence owner: `D:/InnoEngine-Verification/editor-ui-closure-2026-10-27`.

| Check | Result | Evidence / scope |
| --- | --- | --- |
| Core serialization | Passed: 74 | `e46/snapshot/core-bcl-ordered.trx`; includes ordinary byte-array copy semantics, structural changes, floating point bits, wide objects, failure, parallel readers, source retirement and context replacement lifetime |
| Scene contracts | Passed: 53 | `e46/snapshot/scene-stack-final.trx`; capture references, subtree, Prefab and current serialization contracts |
| Real workspace changes | Passed: 3 | `stack-controlled-final.trx`; independent ordinary serialization/hash oracle, direct mutation, converter state, failure recovery and component ordering |
| Snapshot across collectible retirement | Passed: 1 | `snapshot-generation.trx`; retained neutral bytes survive the existing strict unload barrier while the old script type becomes unreachable; later final-source rerun required |
| Independent Editor output | Passed | `product-independent-final.log/binlog`; official SDK graph, zero warnings/errors; native `processes=0`, `bindingGenerations=0`; later comment/context changes require final rebuild |
| FlappyBird drawn workflows | Passed: 8 stages | `e08/product-journey-d3d12-complete/result.json`; actual hidden D3D12 product, gestures, independent Undo/Redo, references, mixed axes, duplicate components, batch recovery, failed mutation rollback, Save/reopen and advancing Play behavior with Edit isolation |
| Whole source build | Passed on successive implementation candidates | `source-wide-final.log`, `source-documented-final.log`; zero errors; final-source gate remains separate |
| Full Contracts / Wiki | Pending rerun | The first operation was aborted after confirmed XML formatting violations. A diagnostic trace identifies its existing whole-repository Wiki test. The direct diagnostic Wiki invocation also lacked logical example references; neither invocation is counted as passed. New XML is expanded and API markers are synchronized. |

The product journey uses a fresh owned project copy and ordinary source extensions. It never edits the original FlappyBird project. It drives public feature commands while real panels draw intermediate state; native pointer/keyboard tests and physical-device experience are separate evidence. Short smoke runs that exit before script activation do not prove these workflows.

The initial journey used host-only Scene lookup APIs that are deliberately absent from script references. The corrected fixture uses the public current scene/component collections and Core persistent identities; no API was exposed for testing. Another initial assertion incorrectly required Selection count zero after closure. The actual contract retains temporarily unavailable identities, so the corrected assertion rejects a resolved stale target while preserving Missing semantics. These attempts remain failed records.

## Quantitative gate still open

`e46/snapshot/absolute-array-processes/measurements.json` records five independent Release VSTest processes, each with five warmups and ten deliberately due complete inspections. The 110 ms preparation delay is excluded from frame timing; it ensures every sample runs the unchanged 100 ms policy. These measurements precede the OrderedDictionary storage cleanup and are not final-candidate measurements.

| Process | Median ms | P95 ms | Allocated bytes / inspection |
| --- | ---: | ---: | ---: |
| 0 | 40.98200 | 42.3213 | 8,370,499.2 |
| 1 | 42.83055 | 44.2989 | 8,370,499.2 |
| 2 | 41.33805 | 61.8373 | 8,370,524.8 |
| 3 | 43.39640 | 50.5440 | 8,370,499.2 |
| 4 | 42.02650 | 43.2985 | 8,370,499.2 |

Allocation is below the announced 8 MiB limit; latency does **not** meet the 16 ms median / 25 ms P95 target. #46 remains Open and must not appear as `Fixes` yet. Earlier stack-context results remain in `stack-controlled-final.json` (49–51 ms, approximately 10.45 MB per inspection). Do not compare due inspections with ordinary frames as though they were identical operations.

Diagnostic decomposition measures the public canonical Asset load and current source freshness separately from complete neutral capture. The public metadata-getter workload costs approximately 9 ms for 10,000 objects. These diagnostic runs locate work; they do not authorize skipping liveness or source verification and are not controlled final comparisons. An experimental scalar converter cache was rejected rather than added to production without sufficient benefit.

## Additional applicable checks

- `e09/native-current-matrix.trx`: 236 real-context checks passed, including the hierarchy and Browser keyboard workflows at both logical sizes and all 28 zoom/framebuffer-scale combinations. These are native input/layout simulations, not physical-monitor or human visual approval.
- `core-bcl-ordered.trx`: 74 Core serialization cases pass after replacing duplicated ordered-entry storage with the existing .NET collection.
- CI at `a38437f7d27307aadcf964f76e9228256d26185f`: Windows and macOS managed jobs passed. The macOS log explicitly reports all 31 Core IO tests passed, including the atomic publication regressions. Native job results are tracked separately; this does not certify subsequent uncommitted snapshot work.

## Original product baseline

The original engine `b303d9f1`, Rendering2D `3e8fafe`, Samples `6eb52f3` and unchanged Canvas restore a consistent historical producer combination. The Samples source differs only in the Rendering2D package; package inspection shows identical runtime/assets and changed Editor code/manifest identity. Both producers share the explicit neutral layout and test source. This is a multi-repository before/after comparison, not an assertion that the original engine can load the current package unchanged.

The unchanged original platform builds and runs 300 D3D12 frames. A diagnostic SDK host links its unmodified entry program and complete official platform output. The cancelled historical entry-graph operation is preserved separately. Complete paired product measurements are still pending.

## Self-review and continuation

The correction preserves domain direction and reuses Core serialization, identity and IO protocols. Snapshot publication owns only neutral data; contexts and converter leases remain operation-local. Failure does not replace valid data, and replacement-context tests verify that discarded services are collectible. These concrete checks support the ownership design; they do not certify the whole engine as flawless.

Still required: #46 quantitative closure, final frozen-source profiles and Wiki, final product/paired performance, remaining per-view and lifetime evidence, exact-head CI, issue-by-issue review and the eleven-dimension report. Mac physical checks remain unexecuted. PR #26 stays Draft until its applicable completion conditions are met.

Automatic approval review rejected deletion of four completed diagnostic runtime copies with `blocked by policy` and no more specific reason. They remain intact. No user, SDK or NuGet global content was removed. The Windows environment has no specified `Glass.aiff`; no completion sound has been played.
