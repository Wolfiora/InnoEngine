# Engine CI/CD acceptance

## Release and product verification

Manual producer run `44181196358` built Windows x64 in 78m21s, macOS Arm64 in 67m49s and Browser Wasm32 in 13m22s at `f693bd5fa2e202e9861b6978b3930d3131b22126`. Run `44265138920` certified its retained archives against functional acceptance source `abd03a21eccde36ccbf811b468ca00e7f4229921`. Host receipts recorded windows-2022 356 Passed, 9 NotApplicable and macos-15 352 Passed, 13 NotApplicable (macos-15 125 min, windows-2022 214 min). Checks include real bindings, Editor startup and rendering, Support Pack, AOT restore, and release packaging.

The hosted publish job (134779747972) returned HTTP 403. Its token had Contents write permission, but this source changes workflows relative to master. The repository owner's separately authorized publication reused the same publisher, verified all five original GitHub artifact digests and both complete host receipts, then uploaded and verified the accepted assets. The hosted run retains its failure status. [GitHub release permissions](https://docs.github.com/en/rest/releases/releases#create-a-release)

Release `native-deps-f693bd5fa2e202e9861b6978b3930d3131b22126-44181196358-1` contains 32 files including its selector. The adopted selector binds 52 component selections and 1 aggregate selection across 2 targets. Each archive digest matched the uploaded digest. Cold published-consumer run `44301436204` started with empty caches on both hosts, ran the SDL and Core mathematics owners, and checked every downloaded archive digest. The cold run skipped its dependent warm probe because GitHub propagated a skipped prerequisite. Two fresh runners in dedicated run `44305208577` then restored the exact cache keys and verified the same bytes.

Weekly run `44271552325` passed ten required checks on each platform and marked 81 explicitly inapplicable. Ten unchanged Editor builds per host had zero third-party compiler processes and zero binding generations. Median build times were 52.581s on Windows and 67.914s on macOS. Weekly scope included lifecycle and performance checks and excluded cold production and release packaging.

## Measured latency

The branch probe changed a single Core mathematics test. The PR-profile probe used the same file change and the same Windows 2022 and macOS 15 hosted runner classes on .NET 10.0.402. Its dispatch adapter provided the previous commit as the PR base. All final measurements used the planner and execution code at `0a96a9fc85aecfaa75c6982aa092c2b0ee74e3a9` and skipped native downloads. The execution critical path adds routing, the slower concurrent platform and the final gate. Wall time also includes queue transitions.

| Workload | Run IDs | Critical path | Median critical path | Median wall time |
| --- | --- | --- | --- | --- |
| Final branch | 44290494346 | 226s | 226s | 236s |
| Previous PR profile | 44244957478, 44248119088, 44248686576 | 195s, 176s, 189s | 189s | 199s |
| Final PR profile | 44290444934, 44292881016, 44293527110 | 267s, 231s, 234s | 234s | 564s |

The measured PR median changed by +23.8%. This workload missed the 50% relative improvement target. The previous workflow already selected managed work only. In the median final run, routing took 20s, the slower host took 187s, and the gate took 27s. The host's principal steps were Run actions/checkout@3d3c42e5aac5ba805825da76410c181273ba90b1 10s, setup-dotnet 19s, freeze-validation 59s, Run frozen shards on the image that captured their environment 84s. This sample measures one controlled workload.

## Regression and evidence scope

- Local functional snapshot `8b772406b7467a74ade0d75d96851c2ee62e76ac` completed 355 required checks (306 passed, 49 reused), plus seven explicit exclusions. Windows executed 1,016 native cases. The migration inventory accounts for all 2,190 original test methods.
- The same SDL DLL failed to load using a 252-character ordinary Windows path. Real Windows tests pass with the corrected loader at 252 and 300 characters and with an already extended root. File existence, link and containment checks remain in force.
- VSTest batch discovery records real case owners and isolation traits. A real partial inventory with 33 cases and an adapter error was rejected. Isolation and failure retention ran against real SDK tests. Jobs timing measures scheduling through completion in the weekly suite.
- The full certification used the source identified above. Subsequent focused checks cover incomplete discovery, parallel worker output, weekly scope and exact download cache keys. During earlier timing run `44288425695`, a second concurrent SDK query exited 137 on macOS. The final planner shares one selected SDK across its managed, native and release graph captures. Six selector tests check cross-platform UTF-8 and LF bytes. Fifteen publication contracts cover permission failures, draft identity, immutable assets and resumption after an interrupted upload. Four real SDK tests passed, including closing freshness and reference-pack invalidation. The original failed run is excluded from the corrected-source sample.
- The existing master ruleset requires the GitHub Actions `Engine validation` check. PR #84 was closed upstream without merging and its exact tip is included in the optimization branch. Default-branch schedules and API publication to Docs draft activate after integration.

## Local storage

Registered retirement recovered 4,004,084,787 logical bytes. SDK clean removed obsolete task-owned compiled outputs. Direct recursive deletion of older diagnostic directories was rejected by automatic approval review, so disk recovery used registered retirement, SDK clean and reversible compression. At delivery the C drive had 34.1 GiB free.
