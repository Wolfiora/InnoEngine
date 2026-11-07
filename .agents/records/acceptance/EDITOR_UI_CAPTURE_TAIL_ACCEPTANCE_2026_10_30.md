# Scene capture tail-latency refinement

This is focused evidence for #27/#38/#46/#53/#54, not final approved-plan closure. PR #26 remains Draft until the replacement production candidate completes its applicable full matrices and CI.

## Confirmed failure

At `98fd5a7d`, five alternating idle/edit process pairs retained all fifty complete 10,000-object dirty inspections per process. Nine of ten failed the unchanged16/25ms median/P95 gate. Edit P95 was28.22–33.50ms; allocations were2,361,582 idle and2,514,872 edit bytes per operation. Raw evidence is `e46/inspection-pairs-43d7a2040c324d8d90e3d6359647150d` under the existing verification owner.

The exact-producer EventPipe trace identifies per-row capture-result objects, interface array enumeration and the complete source-ID dictionary rebuilt for every graph capture. Profiling includes allocation samples and sampled-thread stacks; it is diagnostic, not quiet acceptance. A first small refinement reduced allocation by640KB but passed only8/10 processes; its failures remain retained in `e46/tail-refinement/inspection-pairs.log`.

## Implementation and boundaries

- The internal capture attempt is a readonly value. The borrowed public builder still retires after its callback, with the same identity, metadata and mutation checks.
- Scene iterates its frozen component list by index during row-key capture.
- `SceneStructureSnapshot`, already owned/inactivated by SceneStore, retains a read-only object/component source-ID projection. Every capture checks current Core persistent IDs; a mismatch creates a replacement without mutating an already borrowed projection.
- Systems belong to a separate lifecycle owner. Their indexed lookup is frozen only for the current graph write; the structure cache retains no System. Subtree/Prefab source mappings remain independent.
- The projection cannot resolve live objects or become another Identity registry. No public API, event bus, serializer, global pool/cache, persistent format, platform branch, timer change or BGCS/vendor/generated edit is introduced.
- Existing Writer, canonical bytes, complete serialization contexts, failure publication and strict generation retirement remain authoritative.

## Focused acceptance

Ordinary SDK Core Serialization97 and Scene67 pass. The added public Scene test holds the Scene alive, captures/removes a System, performs full GC/finalizer/full GC, verifies collection, adds a replacement and checks canonical bytes. The ordinary native PlayMode leaf builds successfully.

Five new independent alternating idle/edit process pairs pass **10/10** unchanged absolute gates, retaining every50-operation process and all group/max outliers:

| Workload | Median range, ms | P95 range, ms | Allocated bytes/operation |
|---|---:|---:|---:|
| Idle inspection |8.65–9.53|10.26–13.39|964,706–964,747|
| Inspection after each owned write |12.00–12.93|15.45–21.77|1,117,912–1,119,243|

Evidence: `e46/inspection-pairs-9910c829afbd4a24b522c3361adbe441/measurements.json` and every TRX; build/Scene regression logs are in `e46/source-projection-indexed`. Verification root: `D:/InnoEngine-Verification/editor-ui-closure-2026-10-27`. SDK10.0.402, runtime .NET10.0.13, Release, fixed10,000-object input, five warmups plus five groups of ten measured operations, unchanged110ms outside-timing sleep and100ms production throttle. No result is removed or timer relaxed.

## Required continuation

Freeze this replacement candidate, rebuild the complete applicable source/product/test graphs and rerun the final contracts, native layout/input, lifecycle, products, controlled original/current performance, companion projections, architecture/Wiki and exact-head CI. Prior98fd product/visual passes remain historical evidence, not an assertion that the new candidate has completed them. Reconcile every issue checklist and all eleven quality dimensions before Ready; close issues only through merge.
