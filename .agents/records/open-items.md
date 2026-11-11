# Open work and acceptance boundaries

[Current acceptance](acceptance/current.md) · [Active implementation](plans/ENGLISH_DOCUMENTATION_MODERNIZATION.md) · [Product documentation](https://github.com/Wolfiora/InnoEngine.Docs)

This ledger distinguishes unfinished implementation, missing evidence and external setup. Unexecuted work is not automatically a reproduced defect, and passing contracts do not certify every device.

## Active modernization

The full English documentation, AI workflow and CI/CD plan remains in progress. The implementation ledger tracks the full content inventory, API binding, query extraction, validation precision, cross-repository automation, deployment and legacy removal. The old engine documentation and old validator consumers must remain until their reviewed replacements work, then be removed before completion.

External setup remains unresolved for organization-installed GitHub App credentials. Repository administration and Pages configuration were available, but repository secrets were empty and organization secret metadata returned HTTP 403. This is not evidence of a configured cross-repository App.

Current local query, rule and validation checks do not establish complete Windows/macOS CI, publication ordering, paired-PR or draft-release acceptance. No old issue is automatically closed by this work.

## Preserved engine findings

The following findings come from the [baseline open-item ledger](https://github.com/Wolfiora/InnoEngine/blob/a4e44769715c0577fb0a4c13b2d14ad998ddd8e1/.agents/records/open-items.md) and its linked reports. Their historical evidence and unresolved distinctions remain authoritative; GitHub issue state must be checked before delivery.

| ID | Scope | Preserved finding and boundary |
| --- | --- | --- |
| R02 | Runtime coroutine admission | The private scheduler creates, ticks, stops and disposes but had no admission path. Implement the minimal real contract or remove unused state before claiming integration. |
| T01/T02 | Browser acceptance | Nonfatal console 404 attribution and score persistence across launches were not resolved by the bounded game runs. |
| B01 | Native diagnostics | Windows native source-encoding warnings remain owned by their component; dependency restore is a separately verified scope. |
| F04/F05 | Adjacent UI/2D hot paths | The original audit observed full pixel copies before GPU texture hits and repeated frame-cache bypass with nonempty viewContent. These consumer repositories are read-only in this task; do not claim their behavior was fixed here. |
| F06/F07 | Warm builds and cache maintenance | Historical preparation reread about 4.5 GB; a 5-second inventory attempt timed out while a 30-second dry-run completed. Existing integrity/ownership protocols alone do not prove low cost or eventual maintenance convergence. Preserve these as historical observations, not current measurements. |
| O01 | macOS/Metal | Keep historical timeout records. Commit 2dc37114 had passing Mac automated CI, while a Metal refcount warning also appeared on master. Its cause and manual Mac interaction were unverified; do not infer leak freedom or blame BGCS. |
| O02 | Repeated build work | The user removed fixed short feedback deadlines and extra old-version timing requirements on 2026-11-06. Detect redundant preparation, unexplained invalidation, lock/process stalls and comparable-workload regressions. Legitimate large third-party compilation does not fail merely for taking time. The current approved plan separately requires cold/warm comparable measurements. |
| O06 | XML/API semantics | Signature and example coverage does not prove every historical description states actual parameter, failure, ownership and lifecycle contracts. |

### Durable deletion identity

The original Identity requirement remains open: deleting orphan .imeta files after confirmed deletion conflicts with comprehensive recovery unless durable tombstones have explicit ownership. A rebuildable Library cache alone is insufficient. Any implementation must update writers, watchers, tests and current-format documentation together.

The previous source/XML review found AssetPipeline.Delete removes source and metadata while Library retains a tombstone. Documenting that limitation did not change engine behavior or complete the requested remediation.

## Missing or limited acceptance

### Text and Animation cleanup observations

The English documentation review observed that TextRuntime marks itself disposed, attempts each face release and artifact disposal, clears its font map, and then disposes the backend while collecting failures. Animation's binding snapshot similarly clears provider state after collecting disposal failures. These current paths do not provide the deferred retry ownership used by Audio retirement. Product runtime behavior was not changed in this documentation task, and no new failure scenario was executed. A separate owner-scoped review is needed before claiming retryable cleanup for these services.

| Scope | Boundary |
| --- | --- |
| macOS ARM64/Unix | No local physical Mac. Manual Metal, physical displays, volume variants and complete publication scenarios require their own evidence; automated CI is revision-specific. |
| Browser | Original BGCS 4.1.2 Native and interpreter/AOT publications each ran 3,000 frames. This did not complete all interaction, score persistence or Browser combinations. |
| Rider | SDK entry/Task regressions do not prove actual Rider Run interaction. No separate Rider production branch exists. |
| GUI/devices | Reported Play/Edit transitions, window changes and shutdown do not certify all DPI/focus/input, audio perception or long-running scenarios. |
| U07 filesystems | Native input comparisons require actual Windows case-sensitive directories and both macOS volume modes. An earlier attribute-enabling probe was denied; path resolution does not prove all volume semantics. |
| U08 historical SDK failure | A historical runner produced an internal MissingMethodException. Its absence in later runs does not establish a root-cause fix or a reproduced engine defect. |

## Earlier resolved scopes

Original findings and results remain in their reports, rather than being re-certified here:

- [Editor UI final acceptance](acceptance/EDITOR_UI_FINAL_ACCEPTANCE_2026_10_30.md) closed the applicable local dd5979c6 implementation gaps: menus, Graph, Console, Stats, work controls, UI workflows, input/scale, 100 open/close cycles and ten reloads. The original paired samples included worst dirty-inspection P95 of 24.5695 ms; roughly 32 ms for a 2,000-node graph and 73 ms resource search were still product costs, not a universal 60 FPS guarantee. Both initial and follow-up graph-tail samples remain.
- [Priority fixes](acceptance/PRIORITY_FIX_ACCEPTANCE_2026_10_25.md) record F02/F08 Coroutine retirement (40 Debug and 40 Release cases, plus 62 Runtime cases) and F01 Browser LibraryImport/AOT work. Runtime tests do not prove R02 admission.
- [Standard restore acceptance](acceptance/STANDARD_NUGET_RESTORE_ACCEPTANCE_2026_10_26.md) records normal csproj/PackageReference restore and actual assets/package identity, without per-project/platform dependency lock inventories. Restore/compilation evaluation isolation prevents mixed new DLLs and old package content; this does not close warm-build cost findings.
- [Plugin events/Identity acceptance](acceptance/PLUGIN_EVENTS_IDENTITY_ACCEPTANCE_2026_10_22.md) closed that report's R01/R02. Those IDs are distinct from this ledger's Runtime admission R02.
- [Logo acceptance](acceptance/LOGO_BRANDING_2026_11_07.md) records the corrected Inno.Testing.Editor.Native non-runner classification and final Windows acceptance. Its PlayMode/Performance consumers retain their original layers and native requirements.

The baseline also records explicit platform/distribution contributions replacing shared platform tables and inferred SDK paths; official BGCS generation without package/source patches; protected frozen-input consumers; lease-protected cache retirement; Task workspace protection through restore/evaluation/loading; and Support Pack staging/index ownership. Preserve those protocols and their actual evidence. They do not prove that all engine issues are resolved.
