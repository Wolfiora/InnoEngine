# Full matrix and draft release acceptance

## Full platform execution

[Run 43224643776](https://github.com/Wolfiora/InnoEngine/actions/runs/43224643776) executed the full and extended plan at `4471fe4d341195c61d36ecaf858ff74466b7f892`. Both platform jobs succeeded. The aggregate subsequently failed during certificate serialization; [normalization acceptance](SOURCE_CERTIFICATE_NORMALIZATION_2026_11_13.md) preserves the failure and its correction. Do not present this run as a successful overall workflow.

| Observation | Windows 2022 | macOS 15 |
| --- | --- | --- |
| Required checks passed | 340 | 338 |
| Explicitly not applicable | 3 | 6 |
| TRX passed cases | 3,178 | 3,184 |
| TRX not executed, reconciled by platform applicability | 18 | 6 |
| Actual Debug panel lifecycle | 1,000 cycles, 101 full-GC observations | 1,000 cycles, 101 full-GC observations |
| Actual Release panel lifecycle | 1,000 cycles, 101 full-GC observations | 1,000 cycles, 101 full-GC observations |
| Ten warm-build median | 169,272.23005 ms | 197,523.91375 ms |
| Native producer processes in all warm samples | 0 | 0 |
| Binding generations in all warm samples | 0 | 0 |

Each warm sample selected 15 real products. These are full product workloads, not the feedback time for a private Console edit. Cold preparation is recorded separately in the original plans/results. Windows and macOS measurements use different hosted machines and are not a controlled cross-platform speed comparison.

Windows warm samples in milliseconds: `289175.9209, 170671.7572, 169843.0051, 169448.7243, 167765.6481, 169095.7358, 169850.712, 168054.6466, 167282.0926, 168133.106`.

macOS warm samples in milliseconds: `180878.5965, 157040.8164, 177355.9675, 185771.3506, 201012.8331, 216452.7216, 202194.5988, 198815.6505, 197766.3343, 197281.4932`.

Native integrity validation still hashes real immutable input/output closures. The macOS observations include 100,990 file hashes and approximately 1.98 GB in the main Native integrity scope, plus a separate ShaderTools scope. This is measurable verification cost, not repeated third-party compilation. Whole union build/test durations assigned to an initial owner must not be mistaken for that owner's individual cost.

## Exact-artifact draft release

[Release run 43263420182](https://github.com/Wolfiora/InnoEngine/actions/runs/43263420182) successfully consumed qualified default-branch run `43067972545` at `259e8a636c19c447f2c703c0a64e2f070f9030c3`. It created draft release `443247193`, tag `pipeline-rehearsal-20261113`, without rebuilding products. The release remains a draft with no publication date.

| Uploaded asset | Bytes | SHA-256 |
| --- | --- | --- |
| macOS Editor | 521,350,725 | `ccf58ac24f00a8d333263d3e7ca9d612fe25a23c5a296b44939477a048f6061b` |
| macOS Support Pack | 16,219,406 | `0af927211cc09b59140827e096f8d10dc7ba967a1ce72bbc2a8d8e445e25f613` |
| Windows Editor | 517,890,254 | `961dd7ae5efce1116046ba4f197a85bfbf29be96e9965b8253301da346946893` |
| Windows Support Pack | 14,944,991 | `e78492b77a1e4fa0b4e95c4ba194fae6b75c97cca98113539a6610912161526f` |

The uploaded asset digests were independently checked through GitHub's release metadata. Negative run `43226370116` rejected canceled producer `43191512147`; verify-only run `43226580823` passed before the actual upload. The release workflow and its helpers were identical between the approved default branch and the candidate. This rehearses release delivery of the qualified default-branch products; it does not release or certify the unmerged modernization branch.

## Ownership and remaining acceptance

The obsolete, registered local `artifacts/validation/workspaces/Debug` workspace was retired through the existing owner/dependency graph and exclusive leases, protecting every other candidate with live process dependencies. Its recorded size was 18,345,191,800 bytes. The first attempt retained its failure on read-only fetched Git objects; the retry normalized only three verified private fetched objects under the same exclusive ownership and completed retirement. The target no longer exists. Unverifiable metadata owned by another process was retained. Current query snapshots and first-failure archives remained readable.

The independent Editor Assets edit still has SHA-256 `b99daf9359b26ba1dd5eb1d46d69868bde8153b0c66ed037edcb4e8fa687ed61` and is excluded from this work. Current C-drive free space is approximately 46 GiB, a point-in-time observation.

The final script/reference candidate and certificate repair still require their matching hosted reconciliation. Cross-repository App credentials, protected default-branch activation and production Pages promotion remain external prerequisites. Physical-device, IME, subjective usability and unsupported-platform checks are not implied by these hosted results. No PR was merged and no formal release was published.
