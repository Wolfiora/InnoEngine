# Engine CI/CD acceptance

## Event-driven engine validation

On base `ed416cb0` with the test changes in `1c9b576c`, engine validation runs on pushes, pull requests, merge candidates and explicit dispatch. The retired timer has no successor. A release can still explicitly request completion of missing evidence. Prior time-based runs below are historical measurements.

Local verification parsed the four workflow trigger maps, their job dependencies and six composite actions. Nine focused PowerShell suites passed for event routing, the final gate, release provenance and completion, continuation, backend evidence and publication, and summary reporting. The workflow configuration change required no engine build.

## Independent backend publication

On base `baed28a5`, Windows contract fixtures promoted two platform archives in sequence. Each publication had only its own target's candidate and results. The catalog retained the other archive digests across both updates, and retrying either publication reused the current release. The fixtures also exercised the initial complete catalog, a pinned reader during rotation, interrupted draft ownership and rejection of incomplete SDK evidence.

A local replay verified the five original Windows plan, join and shard artifacts from PR #91 for merge candidate `2719885180c0e17cf12e8a83cc40cddb3cd30572`. The new platform publication path accepted 21 passed checks and 76 explicit exclusions using attempt 1 evidence in a simulated second attempt. It emitted Windows results without issuing whole-engine certification. Live publication requires a trusted default-branch event. [Original Windows execution](https://github.com/Wolfiora/InnoEngine/actions/runs/45183406559/job/137835300349)

## Windows cancellation and failed batch evidence

The [first PR 88 candidate](https://github.com/Wolfiora/InnoEngine/actions/runs/44760191698/job/136373208390) returned all 300 planned Windows managed checks. One owner failed, and a 301st process record hid its diagnostic during reconciliation. The archived case took 2.0144153 seconds. The managed test process took 6,409.8 seconds. Its TRX did not capture the instant cancellation was requested.

The retained incident fixture covers the complete failed receipt and a genuinely missing owner. Both fail certification. Version 2 keeps process and closing failures separate from planned checks, and treats timing as diagnostic data. A real Windows sharing denial exercises cancellation before and after the retry deadline through the public directory API. Gate-only planning and explicit project consumption have paired regression cases. The hosted candidate and its timing are recorded by the PR checks.

## Superseded validation runs

The [cancelled branch run](https://github.com/Wolfiora/InnoEngine/actions/runs/44678353728/job/136095922201) reported a matrix error after planning was cancelled. The gate now follows workflow cancellation and checks planning success before interpreting dispatch outputs on a live run. Local verification against base `28ba7a8f` reproduced the misleading diagnostic and exercised seven rejection cases from the workflow script. Actionlint and the eleven gate transport contracts passed. Hosted cancellation was not replayed.

## Dependency scheduling and SDK entry validation

The comparison baseline is the tree merged in PR #86 at `1fade7f94a78a9cafe21dab53e82a45418753238`. The Core, Editor SDL, SDL source and Windows properties probes replay the original one-file changes against implementation `df11979c`. The dispatch uses the candidate parent for affected validation. Each probe checks that the original file's base blob matches before using its measured change. Distinguish the active runner interval, cumulative runner usage, queue time and merge-gate wall time.

| Input | Baseline | Candidate |
| --- | --- | --- |
| Core branch / PR profile | [Branch](https://github.com/Wolfiora/InnoEngine/actions/runs/44392798899), [PR sample](https://github.com/Wolfiora/InnoEngine/actions/runs/44397510265) | [Branch](https://github.com/Wolfiora/InnoEngine/actions/runs/44680423979), [PR profile](https://github.com/Wolfiora/InnoEngine/actions/runs/44680477624) |
| Editor SDL consumer | [685s wall](https://github.com/Wolfiora/InnoEngine/actions/runs/44567919981) | [Replay](https://github.com/Wolfiora/InnoEngine/actions/runs/44680528477) |
| SDL implementation | [7,387s wall](https://github.com/Wolfiora/InnoEngine/actions/runs/44567995562) | [Replay](https://github.com/Wolfiora/InnoEngine/actions/runs/44680533546) |
| Windows product properties | [18,621s wall](https://github.com/Wolfiora/InnoEngine/actions/runs/44568062949) | [Replay](https://github.com/Wolfiora/InnoEngine/actions/runs/44680537485) |

The first Core replay failed the architecture shard on declaration formatting. Its wall time is excluded from performance comparisons. The correction passed the repository source architecture command on Windows with 270 evaluated projects and 919 SDK nodes. The broader local validation suite also exposed a distinction between an affected project owner and an independently scheduled SDK entry. The conditional-source regression and three related SDK-entry cases passed after preserving that distinction.

Local SDK fixtures exercised conditional imports on two actual entry instances, an input move across projects, a distinct platform product and backend compiler inputs. The publisher fixture rejected missing or untrusted SDK consumption evidence. Existing platform catalogs predate version 2 consumer receipts. The initial publication on the trusted default branch establishes the evidence required for cross-release product-level reuse. Product tags and the documentation release retain separate workflows.

The controlled Core branch push supplied a real previous push as its baseline. It passed in 286 seconds including 39 seconds before its first runner. The single Windows execution job took 204 seconds. The first affected Core PR-profile replay passed 19 mathematics cases on each host. It used 355 seconds with active runners and 372 seconds of wall time. The first Editor consumer replay passed in 877 seconds including queue transitions. These PR-profile runs exposed a second bootstrap and graph evaluation on the final runners. [Core branch](https://github.com/Wolfiora/InnoEngine/actions/runs/44682831488) / [Core PR profile](https://github.com/Wolfiora/InnoEngine/actions/runs/44681895031) / [Editor consumer](https://github.com/Wolfiora/InnoEngine/actions/runs/44681964963)

The Windows properties replay scheduled Windows native acquisition at 05:39:14 UTC. The independent macOS execution started at 05:45:05 UTC, with Windows SDK production starting at 05:49:16 UTC during macOS execution. Its Mac plan still required 75 test projects. None of their SDK entry source sets contained the changed file. The merged validator tool inputs did contain it. Binding the validator's own inputs to its managed SDK entry removes that shared invalidation. Cross-target SDK subprocess contracts declare the files they read in the product profile integration test project. The test matrix retains the original Debug/Release and development/product entry assertions. [Concurrent execution and captured plans](https://github.com/Wolfiora/InnoEngine/actions/runs/44681970123)

The portable plan replay encountered two actual Windows images, 20261116.370.1 for scheduling and 20261112.366.7 for execution. It captured the new environment, recomputed the plan and passed all 19 cases. The local SDK regression also checks an exact environment match and rejects a changed environment before writing a bound plan. [Windows image change](https://github.com/Wolfiora/InnoEngine/actions/runs/44684134277)

The refined Windows properties replay reduced the Mac plan from 338 required checks to four. Its acquisition step reported zero backend transfer and compilation. Windows retained 51 required checks. The controlled Core branch passed in 204 seconds, including three seconds before its first runner. On the same implementation the Editor consumer passed 94 tests per host with exact platform cache hits. It took 850 seconds of wall time against 685 seconds on the previous workflow. Both final runners had different image versions than their scheduling runners and regenerated their plans. The current candidate's Windows tests passed all 100 validation cases and all 17 product profile SDK cases. The source architecture inventory evaluated all 271 projects and 920 SDK nodes. [Windows properties](https://github.com/Wolfiora/InnoEngine/actions/runs/44686516530) / [Core branch](https://github.com/Wolfiora/InnoEngine/actions/runs/44686433049) / [Editor consumer](https://github.com/Wolfiora/InnoEngine/actions/runs/44686531253)

The former nightly preflight could select an authenticated partial master certification of the same commit for each host. The existing continuation executor rechecks inputs, environment, dependency identity and outputs for each reusable check. Expired optional compilation archives leave the build obligation on the execution host. Eleven discovery, nine host-matrix and ten continuation contracts exercised the decision and authentication boundaries locally.

## Current CI backend prerelease

The three registered targets passed full functional validation at `0fed32331010ddf20b2a96ea13ed45b94b7b5533`. The package publisher checked the complete gate receipts and the platform consumption receipts before promotion. Repeating promotion at the same inputs selected the same release. On independent hosted workers, a cold download of each published archive and a required warm cache hit supplied working product consumers. [Full engine gate](https://github.com/Wolfiora/InnoEngine/actions/runs/44519061480) · [Cold and warm consumers](https://github.com/Wolfiora/InnoEngine/actions/runs/44567924287) · [CI prerelease](https://github.com/Wolfiora/InnoEngine/releases/tag/backend-prebuilts-2026.11.19.2)

The functional gate passed 356 required checks on Windows (3,314 test cases) and 352 on macOS (3,311 cases). Nine and thirteen checks respectively had explicit platform exclusions. The full run included fresh platform production, certification and product execution. The wall time to the gate was 4h24m01s, including a 15-minute hosted runner-capacity rejection. The active critical path was 4h09m39s. Windows functional execution took 2h35m20s and macOS took 1h51m26s.

| Target | ZIP bytes | Manifest files | Cold SDK consumption checks |
| --- | ---: | ---: | ---: |
| windows-x64 | 62,172,452 | 615 | 4 |
| macos-arm64 | 66,657,994 | 615 | 4 |
| browser-wasm | 11,653,750 | 125 | 1 |

The planner uses effective compiler inputs to select backend projects by target and configuration. Changed projects and their downstream consumers enter production. The publisher assembles each ZIP from the current file inventory, so retired projects and variants disappear on the next publication. Release retention allows the current CI prerelease and two rollback versions. Product version tags follow the independent product release workflow.

A branch commit gets quick feedback. An open PR and a merge-queue candidate get affected functional checks. Master expands product and packaging coverage. At the time of these measurements, recurring runs covered the functional matrix, performance and lifecycle. A host with a single matching archive can plan and execute in place. If a PR needs a producer, its final runner freezes the executable plan against its own environment. The final gate checks every required host receipt. Source builds remain the default developer workflow.

The former weekly caller passed on both desktop hosts in 38m57s, with ten required performance or lifecycle checks and 81 exclusions per host. The workers consumed the published desktop digests and ran no backend production or publication jobs. GitHub rejected the original weekly caller before assigning a runner. The nightly caller had the same permission declaration. The caller job now admits the reusable workflow's job permission ceiling. The shared validator grants read access to testing jobs. Weekly rejects missing precompiled packages before scheduling production. [Startup rejection](https://github.com/Wolfiora/InnoEngine/actions/runs/44576057746) / [Corrected weekly caller](https://github.com/Wolfiora/InnoEngine/actions/runs/44577168827)

For the Windows product properties probe, the acquisition plan selected Windows production and retained the exact macOS and Browser digests. macOS still ran functional checks. Comparing its frozen plan with the full certificate showed an identical host environment and identical backend payloads. The evaluated solution included Windows-target variants that actually imported the changed file. Their inputs occur in the validation closures of backend tests, including FileSystem and Bgfx, and invalidated the older test receipts. Platform package independence and host test selection have different costs for this infrastructure change. [Windows properties probe](https://github.com/Wolfiora/InnoEngine/actions/runs/44568062949)

## Matched PR measurements

The baseline and current probes use the same single changed file, runner classes and profile. Wall time includes GitHub scheduling. Active time sums the longest job in each dependent workflow stage. The Windows build-property baseline ended with a macOS SDK fixture timeout and is shown separately from successful comparisons.

| Input | Baseline active / wall | Current active / wall | Windows / macOS passing cases | Change in wall time |
| --- | ---: | ---: | ---: | ---: |
| Editor SDL consumer | [849s / 862s](https://github.com/Wolfiora/InnoEngine/actions/runs/44398269046) | [674s / 685s](https://github.com/Wolfiora/InnoEngine/actions/runs/44567919981) | 94 / 94 | -20.5% |
| SDL adapter source | [2,900s / 2,944s](https://github.com/Wolfiora/InnoEngine/actions/runs/44398313561) | [7,067s / 7,387s](https://github.com/Wolfiora/InnoEngine/actions/runs/44567995562) | 969 / 969 | +150.9% |
| Windows product build properties | [6,566s / 6,883s](https://github.com/Wolfiora/InnoEngine/actions/runs/44398358719) | [18,297s / 18,621s](https://github.com/Wolfiora/InnoEngine/actions/runs/44568062949) | 3,310 / 3,307 | Baseline failed |

The consumer-only SDL probe ran 94 cases per host. A cold runner fetched 62,172,452 bytes on Windows and 66,657,994 on macOS. Neither host scheduled a producer or ran backend self-tests. This saved 177 seconds of wall time while increasing cold download size compared with eight component archives.

The SDL source probe compiled three backend project owners per desktop platform and configuration. The other 27 owners were supplied from the warm platform package. Its seed transfers from Releases were zero bytes on both hosts. The Windows producer spent 4,308 seconds preparing and cold-certifying the complete new package. The slower functional consumer took 2,143 seconds. Both platforms passed 969 cases. The complete product certification made this workload 2.51 times as long as the earlier component-cache baseline.

The Windows build-property probe spent 5,094 active seconds in its longest planning job, 5,786 seconds in platform production and 6,746 seconds in final host execution. The macOS planning job executed its selected checks in place. The shared workflow waited for it before scheduling Windows production. An infrastructure change with this dependency graph can take several hours.

| Windows property probe producer | Compiled / prebuilt owners |
| --- | ---: |
| windows-x64 Debug | 30/0 |
| windows-x64 Release | 30/0 |

- macos-15 selected 66,657,994 archive bytes and fetched 0 bytes from Releases.
- windows-x64 selected 62,172,452 archive bytes and fetched 0 bytes from Releases.
- browser-wasm selected 11,653,750 archive bytes and fetched 11,653,750 bytes from Releases.

## Earlier component-cache implementation

The following measurements and operating instructions describe the superseded component archive protocol.

### Release and product verification

Manual producer run `44181196358` built Windows x64 in 78m21s, macOS Arm64 in 67m49s and Browser Wasm32 in 13m22s at `f693bd5fa2e202e9861b6978b3930d3131b22126`. Run `44265138920` certified its retained archives against functional acceptance source `abd03a21eccde36ccbf811b468ca00e7f4229921`. Host receipts recorded windows-2022 356 Passed, 9 NotApplicable and macos-15 352 Passed, 13 NotApplicable (macos-15 125 min, windows-2022 214 min). Checks include real bindings, Editor startup and rendering, Support Pack, AOT restore, and release packaging.

The hosted publish job (134779747972) returned HTTP 403. Its token had Contents write permission, but this source changes workflows relative to master. The repository owner's separately authorized publication reused the same publisher, verified all five original GitHub artifact digests and both complete host receipts, then uploaded and verified the accepted assets. The hosted run retains its failure status. [GitHub release permissions](https://docs.github.com/en/rest/releases/releases#create-a-release)

Release `native-deps-f693bd5fa2e202e9861b6978b3930d3131b22126-44181196358-1` contains 32 files including its selector. The adopted selector binds 52 Windows x64 and macOS Arm64 component selections plus one Browser Wasm32 aggregate selection, covering three targets. Each archive digest matched the uploaded digest. Cold published-consumer run `44301436204` started with empty caches on both hosts, ran the SDL and Core mathematics owners, and checked every downloaded archive digest. The cold run skipped its dependent warm probe because GitHub propagated a skipped prerequisite. Two fresh runners in dedicated run `44305208577` then restored the exact cache keys and verified the same bytes.

Weekly run `44271552325` passed ten required checks on each platform and marked 81 explicitly inapplicable. Ten unchanged Editor builds per host had zero third-party compiler processes and zero binding generations. Median build times were 52.581s on Windows and 67.914s on macOS. Weekly scope included lifecycle and performance checks and excluded cold production and release packaging.

### Measured latency

The branch probe changed a single Core mathematics test. The PR-profile probe used the same file change and the same Windows 2022 and macOS 15 hosted runner classes on .NET 10.0.402. Its dispatch adapter provided the previous commit as the PR base. All final measurements used the planner and execution code at `0a96a9fc85aecfaa75c6982aa092c2b0ee74e3a9` and skipped native downloads. The execution critical path adds routing, the slower concurrent platform and the final gate. Wall time also includes queue transitions.

| Workload | Run IDs | Critical path | Median critical path | Median wall time |
| --- | --- | --- | --- | --- |
| Final branch | 44290494346 | 226s | 226s | 236s |
| Previous PR profile | 44244957478, 44248119088, 44248686576 | 195s, 176s, 189s | 189s | 199s |
| Final PR profile | 44290444934, 44292881016, 44293527110 | 267s, 231s, 234s | 234s | 564s |

The measured PR median changed by +23.8%. This workload missed the 50% relative improvement target. The previous workflow already selected managed work only. In the median final run, routing took 20s, the slower host took 187s, and the gate took 27s. The host's principal steps were Run actions/checkout@3d3c42e5aac5ba805825da76410c181273ba90b1 10s, setup-dotnet 19s, freeze-validation 59s, Run frozen shards on the image that captured their environment 84s. This sample measures one controlled workload.

### How a change reaches the gate

The event chooses the depth of validation. A branch without an open PR gets fast checks. An open PR or merge-queue candidate gets affected integration tests on the registered hosts. A master push covers affected product and release behavior. Manual dispatch and release completion can request the full functional matrix. API export has its own preflight and publishes to Docs draft after integration.

The planner reads evaluated SDK inputs, project references and product declarations. It records which checks this candidate needs and the reason for each selection. Within each runner the workers share a frozen environment. Independent managed and native work gets separate writable output directories. The final gate checks the expected hosts, checks and evidence digests. A missing required receipt fails the gate.

| Change | Branch feedback | What the PR proves |
| --- | --- | --- |
| Edit one Core mathematics test | Compile the affected managed closure and run its selected tests. | Run the selected managed behavior without downloading native libraries or preparing the Editor. |
| Edit functionality that loads SDL | Run applicable fast managed checks. | Download the selected archives on a cold runner or restore the exact cache key on a warm runner. Verify the bytes, build the affected engine code and exercise the native consumer. |
| Change third-party source, binding inputs or a native recipe | Fast managed checks can still run independently. | A mismatched selection blocks that consumer. The manual Native dependencies workflow produces and certifies a matching release. Adopt its selector in the branch and rerun the consumer. |

The Release is the durable copy of third-party binaries. Actions Cache accelerates downloads. Eviction causes another download. Managed engine assemblies, the Editor, Player and Support Packs still follow their own source and output identities.

### Regression and evidence scope

- Local functional snapshot `8b772406b7467a74ade0d75d96851c2ee62e76ac` completed 355 required checks (306 passed, 49 reused), plus seven explicit exclusions. Windows executed 1,016 native cases. The migration inventory accounts for all 2,190 original test methods.
- The same SDL DLL failed to load using a 252-character ordinary Windows path. Real Windows tests pass with the corrected loader at 252 and 300 characters and with an already extended root. File existence, link and containment checks remain in force.
- VSTest batch discovery records real case owners and isolation traits. A real partial inventory with 33 cases and an adapter error was rejected. Isolation and failure retention ran against real SDK tests. Jobs timing measures scheduling through completion in the weekly suite.
- The full certification used the source identified above. Subsequent focused checks cover incomplete discovery, parallel worker output, weekly scope and exact download cache keys. During earlier timing run `44288425695`, a second concurrent SDK query exited 137 on macOS. The final planner shares one selected SDK across its managed, native and release graph captures. Six selector tests check cross-platform UTF-8 and LF bytes. Fifteen publication contracts cover permission failures, draft identity, immutable assets and resumption after an interrupted upload. Four real SDK tests passed, including closing freshness and reference-pack invalidation. The original failed run is excluded from the corrected-source sample.
- The existing master ruleset requires the GitHub Actions `Engine validation` check. PR #84 was closed upstream without merging and its exact tip is included in the optimization branch. Default-branch schedules and API publication to Docs draft activate after integration.

The implementation commit `80f4cccd898bbf3b64103cc49182bdc432fe0c01` passed its branch gate in [run 44306564151](https://github.com/Wolfiora/InnoEngine/actions/runs/44306564151).

### Local storage

Registered retirement recovered 4,004,084,787 logical bytes. SDK clean removed obsolete task-owned compiled outputs. Automatic approval review blocked recursive deletion of older diagnostic directories and the temporary publication directory. Registered retirement, SDK clean and reversible compression recovered disk space. Exact removal of the five completed transfer ZIPs also reclaimed 86,055,281 bytes. At delivery the C drive had 34.1 GiB free.

## CI-only backend package measurements

The Core probe changed the same mathematics test as the previous measurements. The branch run used the normal push event. The PR-profile runs used the existing dispatch adapter with the immediately preceding commit as base, Windows 2022 and macOS 15, and the same .NET SDK. Times start at GitHub run creation for wall time. The active critical path sums the participating jobs on the longer dependency chain and omits scheduling gaps.

| Probe | GitHub run | Active critical path | Wall time |
| --- | --- | --- | --- |
| Branch | [44392798899](https://github.com/Wolfiora/InnoEngine/actions/runs/44392798899) | 225 s | 238 s |
| PR profile | [44393311972](https://github.com/Wolfiora/InnoEngine/actions/runs/44393311972) | 280 s | 598 s |
| PR profile | [44395527109](https://github.com/Wolfiora/InnoEngine/actions/runs/44395527109) | 259 s | 276 s |
| PR profile | [44395534813](https://github.com/Wolfiora/InnoEngine/actions/runs/44395534813) | 265 s | 282 s |

The branch passed four required checks, including all 19 mathematics tests. Backend downloads, compilations, deployments and backend tests were zero. The branch met its five-minute target. Its previous wall time was 236 seconds. The PR-profile median met the ten-minute target at 282 seconds wall time. Its previous median was 564 seconds. Active time rose from 234 to 265 seconds, so the reduction in wall time cannot be attributed to faster computation. On the median active run, routing took 12 seconds, the slower host took 206 seconds, backend matrix reconciliation took 17 seconds and the final gate took 30 seconds. These samples measure managed changes only.

The acquisition job was subsequently removed from the critical path when every host completes its frozen plan independently. Three real runs of the same two-host PR probe passed with the acquisition jobs skipped. The final gate still required both hosts' complete receipts.

| Final PR-profile sample | Active critical path | Wall time |
| --- | --- | --- |
| [44397503485](https://github.com/Wolfiora/InnoEngine/actions/runs/44397503485) | 259 s | 271 s |
| [44397510265](https://github.com/Wolfiora/InnoEngine/actions/runs/44397510265) | 253 s | 265 s |
| [44397515882](https://github.com/Wolfiora/InnoEngine/actions/runs/44397515882) | 247 s | 259 s |

The final median was 265 seconds including queueing, 53.0% below the 564-second baseline. Median active time was 253 seconds, 8.1% above the 234-second baseline. None of these six PR samples needed a backend archive. The additional native-consumer and producer scenarios measure a different cost.

### Product and platform comparison

The controlled PR-profile probes below retain the same changed file and runner classes on each side of the migration. Counts come from the joined functional receipts on each host. Active time includes routing, the slower parallel host and the gate.

| Baseline input | GitHub run | Active critical path | Wall time | Test cases per host |
| --- | --- | --- | --- | --- |
| ImGui Editor consumer of SDL | [44398269046](https://github.com/Wolfiora/InnoEngine/actions/runs/44398269046) | 849 s | 862 s | 94 |
| SDL backend source | [44398313561](https://github.com/Wolfiora/InnoEngine/actions/runs/44398313561) | 2,900 s | 2,944 s | 969 |

Both baseline SDL consumer jobs selected eight native component archives. A cold transfer of those archives required 23,452,130 bytes on Windows and 27,733,095 bytes on macOS. Each job reported 26 distinct backend outputs. Those output lines establish the size of the build graph and cannot establish the number of compiler invocations.

The Windows player build-properties baseline ran for 6,883 seconds including scheduling and 6,566 seconds on the active critical path. Windows completed 3,298 cases. macOS completed 3,272 passing cases in its successful projects and failed a real SDK recovery regression. Its five-minute deadline covered two independent validation processes. The corrected fixture gives each execution its own deadline and retains the original cancellation and evidence assertions. The corrected regression passed on macOS in 53 seconds. The failed baseline is excluded from successful-run medians. [Baseline run](https://github.com/Wolfiora/InnoEngine/actions/runs/44398358719) · [macOS regression](https://github.com/Wolfiora/InnoEngine/actions/runs/44418085426)

### Platform archive inventory and build reuse

A second producer run with matching inputs selected 30 prebuilt backend projects and zero source projects in each of Windows Debug, Windows Release, macOS Debug, macOS Release and Browser Release. A separate Browser producer used the certified Windows SDK host and selected zero host backend projects for compilation and 30 for prebuilt consumption. [Five configurations](https://github.com/Wolfiora/InnoEngine/actions/runs/44408684164) · [Cross-target host](https://github.com/Wolfiora/InnoEngine/actions/runs/44424871879)

| Platform | ZIP bytes | Files listed in the manifest | Native product requests |
| --- | ---: | ---: | ---: |
| Windows x64 | 55,923,936 | 552 | 6 |
| macOS arm64 | 60,425,958 | 552 | 6 |
| Browser Wasm32 | 11,653,524 | 125 | 1 |

For source `7cb9d9563011e6840d1ecfd5333fd4798ef53838`, each configured platform compiled 25 changed backend projects and reused five. Each verified ZIP contained exactly its manifest entries and the native archives required by its product requests. The desktop producers exercised Editor and Player in Debug and Release. The Browser Release Player built its Support Pack in 287.8 seconds. The original cross-target acceptance adapter failed before invoking its producer. Its isolated retry used the same source and preserved the successful desktop packages. [Desktop production](https://github.com/Wolfiora/InnoEngine/actions/runs/44415140293) · [Browser production](https://github.com/Wolfiora/InnoEngine/actions/runs/44417007068)

A single-platform transport rehearsal deliberately failed its consumer after a successful plan upload. GitHub reran the failed job. Attempt 2 extracted the original attempt 1 request into the named platform directory and joined the frozen request successfully. Acceptance receipts retain their attempt identities. [Failed-job recovery](https://github.com/Wolfiora/InnoEngine/actions/runs/44429680867/attempts/2)

The first complete host runs rejected the package before release. Windows passed 297 managed checks and macOS passed 295; both rejected the same source-generation fixture under an inherited product manifest. The native SDK build also required direct project roots under the declared development Editor properties. The product-only producer had certified the actual Editor and Player entries but omitted those direct compiler variants. The corrected producer builds and cold-consumes the development solution, and the fixture now owns its temporary process environment. Running the real test with a deliberately invalid outer product manifest passed in 4m46s on Windows. Five backend planner tests and the source architecture check passed locally. [Original Windows diagnostic](https://github.com/Wolfiora/InnoEngine/actions/runs/44424493530) · [Original macOS diagnostic](https://github.com/Wolfiora/InnoEngine/actions/runs/44424463543)

### Native SDK solution scheduling

A fresh Windows worker compiled the standalone SDL development root in 183 seconds and exported the binding-enabled variant missing from the first host certification. The same Debug environment with 30 backend roots and two MSBuild nodes timed out at 12 minutes. The last 613 seconds produced no further build output. Cancellation identified concurrent TaskHosting.BuildTaskRuntimeTask requests in ImGui, Bgfx and MiniAudio. A common bootstrap owner alone also timed out before preparing a runtime. The native validation runner's single-node scheduling completed the full 30-project SDK build in 748 seconds, with one runtime preparation and eight reuses. Independent products and platforms retain separate scheduling. [SDL root](https://github.com/Wolfiora/InnoEngine/actions/runs/44454087433) · [Parallel solution](https://github.com/Wolfiora/InnoEngine/actions/runs/44454810767) · [Shared-owner control](https://github.com/Wolfiora/InnoEngine/actions/runs/44456251949) · [Completed native solution](https://github.com/Wolfiora/InnoEngine/actions/runs/44456639992)

Production run 44437701516 exhausted its 180-minute limit on both desktop hosts. On Windows, 10,050 seconds elapsed between the last SDK output and cancellation. Both logs stopped inside the first development solution, before the package assembly stage. Browser retained its certified bytes and completed. The fix uses the existing native validation scheduler for the backend solutions and a fresh task bootstrap owner per SDK invocation. [Original producer](https://github.com/Wolfiora/InnoEngine/actions/runs/44437701516)

### Release dependency configuration

The corrected scheduler completed the macOS Debug development solution and both Debug products. Release certification then rejected conflicting DLLs before archive publication. An isolated Windows replay showed that the partial solution fed Debug reference assemblies and a Debug source generator to its Release backend root. The real product selected the Release versions. Their compiler arguments only differed in these three inputs. Binding the partial solution to the planner's reference-configuration policy made the two Release DLL hashes identical in the replay. Backend compiler identities include that policy, so uncertified archives cannot seed the corrected managed outputs. The Debug and Release source configurations and integrity checks remain enabled. [Producer diagnostic](https://github.com/Wolfiora/InnoEngine/actions/runs/44458535314)

The hosted macOS compiler comparison reproduced the same three differing arguments. Generated assembly metadata, analyzer configuration and Source Link were byte-identical. The real Editor compiler used Release versions of Inno.Core.IO, Inno.Extensibility.Catalogs and Inno.Core.Serialization.Generators. [Compiler comparison](https://github.com/Wolfiora/InnoEngine/actions/runs/44464844499)

### Configuration-correct platform certifications

Source `85c421a43d88f8f8d4dfbe4b73ec02d6bcd1762c` invalidated the affected compiler identities. Each of the five registered build configurations selected 30 managed backend owners once. The source and archive receipts matched their authenticated outer workflow artifacts. Each ZIP contained exactly its manifest and enumerated files. [Three-platform producer](https://github.com/Wolfiora/InnoEngine/actions/runs/44465534873)

| Target | ZIP bytes | Files | Native product requests | Producer job | Cold product checks |
| --- | ---: | ---: | ---: | ---: | --- |
| Windows x64 | 62,172,265 | 615 | 6 | 83m03s | Debug Editor 278s, Player 133s. Release Editor 240s, Player and Support Pack 375s. |
| macOS arm64 | 66,657,984 | 615 | 6 | 81m24s | Debug Editor 276s, Player 129s. Release Editor 267s, Player and Support Pack 436s. |
| Browser Wasm32 | 11,653,972 | 125 | 1 | 30m53s | Release Player Support Pack 296s. |

These are commissioning times for a compiler-configuration change that touched all project identities. Each desktop ZIP includes the SDK's development-root variants in addition to its product-entry variants. The Browser inventory stayed at 125 files.

### Full functional acceptance of the platform archives

Windows certified 356 required checks, with nine explicitly inapplicable. The first macOS run passed all 296 managed checks and 50 native checks. Two Editor sample-import cases reached their 15-second startup deadline, blocking the dependent product packaging check. On a fresh macOS runner with the same prebuilt bytes, both script compilations succeeded. The compiler finished in 7.9s and 6.0s. Generation activation and the subsequent Editor frame reached Ready in 9.8s and 8.5s. The full cases completed in 13.6s and 9.1s. The sample startup fixture now reports terminal compiler failures immediately and uses the existing 60-second integration deadline for startup only. [Windows full validation](https://github.com/Wolfiora/InnoEngine/actions/runs/44475524102)  /  [First macOS validation](https://github.com/Wolfiora/InnoEngine/actions/runs/44475408421)  /  [Isolated sample observations](https://github.com/Wolfiora/InnoEngine/actions/runs/44493484495)

The macOS recovery rejected earlier receipts when GitHub assigned runner image 20261104.0514.1 instead of 20261112.0539.3. Reuse requires a matching plan and environment. [Image mismatch](https://github.com/Wolfiora/InnoEngine/actions/runs/44495262517)

The revised fixture committed at d5f62a57 passed both cases on both desktop hosts with the certified package. [Fixture verification](https://github.com/Wolfiora/InnoEngine/actions/runs/44495596368)

The default-branch publisher takes the paired plan and results from the successful merge gate. In a real GitHub release-only retry, attempt two recovered the sole attempt-one evidence archive, copied the original four fixture files and passed the publisher interruption and integrity tests. The gate retained its original execution timestamps. The recovery check had read-only permissions and never published a fixture. [Release-only retry](https://github.com/Wolfiora/InnoEngine/actions/runs/44497929498/attempts/2)

The fresh macOS image executed the full plan. It passed 352 checks with 13 explicitly inapplicable, including all 260 Editor scripting cases and release packaging. The native shard ran from 09:33 to 10:14 UTC, followed by the managed shard. The Windows and macOS reports identify 3313 and 3310 required passing test cases respectively. [Completed macOS verification](https://github.com/Wolfiora/InnoEngine/actions/runs/44496131961)

The publisher staged three approved archives, verified GitHub's SHA-256 for each, published Backend Prebuilts 2026.11.19.1 and reused that exact release on a second invocation. All three empty download owners fetched the published bytes. Three fresh consumers then required exact Actions cache hits, including the Windows build host for Browser. Both desktop SDL and Editor consumers and the Browser Player Support Pack passed. [CI prerelease](https://github.com/Wolfiora/InnoEngine/releases/tag/backend-prebuilts-2026.11.19.1) / [Cold and warm consumers](https://github.com/Wolfiora/InnoEngine/actions/runs/44516372818)

The first Windows SDL profile rejected a plan transported from Windows image 20261112.366.7 to 20261116.370.1. Desktop jobs that need one certified archive now consume it in their original workspace. Jobs waiting on a producer freeze their executable plan on the final runner. The acquisition matrix rejects an inline operation that would hide a manual producer. The Windows product props probe also exposed a cross-target SDK default None item in the shader task's Standard Distribution dependency. Backend input capture now distinguishes a passive build definition from its effective imports, explicitly declared inputs and copied payloads. A regression with three real SDK projects checks all three cases. Six backend input tests passed locally. [Windows image rejection](https://github.com/Wolfiora/InnoEngine/actions/runs/44516232360) / [Original platform probe](https://github.com/Wolfiora/InnoEngine/actions/runs/44516360157)

## Affected validation cutover, November 25, 2026

The previous 96-minute managed batch included validation-tool workloads. The four-round validator fixture previously took 11m35s. Its replacement runs one failing xUnit case in a small SDK project through the real executor and rejects the resulting evidence. On the Windows development host this test took 21s. The focused planner and cadence-profile suite passed 25 cases in 3m41s on that machine. These are local measurements. The mixed infrastructure PR and a small managed change have different selected workloads.

Two affected manual runs on the PR branch isolate a single changed input and a single changed tooling script. Each uses the same planner and host workers as the pull request. Times are wall clock from the Actions job and step timestamps. Upload figures add the client transfer and finalization intervals on Windows. The workers overlap.

| Changed input | Queue to first job | Route | Windows checkout / SDK setup / freeze | Resolve | Windows execution step | Managed build / test process | Windows uploads | End to end |
| --- | ---: | ---: | ---: | ---: | ---: | --- | ---: | ---: |
| Input test project | 5s | 22s | 11s / 19s / 67s | 7s | 154s | 65.1s / 5.0s, 8 Input cases passed | 3.50s | 8m44s |
| Tool summary script | 4s | 8s | 10s / 19s / 52s | 1s | 46s | No engine compilation or xUnit. Three PowerShell scripts took 2.04s. | 4.07s | 3m12s |

The managed run used the exact Windows backend archive with zero producers. Its Windows job finished at 4m53s. The macOS job started at 5m33s, ran repository checks for 2m40s, and selected zero engine test projects. The managed sample missed the five-minute end-to-end target. The tooling sample met it. These two samples measure different changes and do not establish an average or a latency guarantee. [Managed run](https://github.com/Wolfiora/InnoEngine/actions/runs/45444455737) / [Tooling run](https://github.com/Wolfiora/InnoEngine/actions/runs/45444719532)

On the updated branch, changing only `GameScene.cs` in a disposable checkout selected the same nine projects under the pull-request and master profiles. The five expensive tooling suites were absent. Both plans requested the Windows Editor backend for native Editor tests. An explicit Browser Wasm32 rebuild selected one Player request and zero unrelated engine tests. The full plan selected the registered applicable engine checks. The focused selection and profile suite passed 26 cases in 4m17s. The executor-ownership regression passed in 9s, and five integration cases covering real failure and shard execution passed in 56s. The source architecture checker passed on all 51 C# files changed by the PR.

The managed validator fixture now contains only the SDK projects and build rules exercised by its compilation, host-filter and failure cases. It runs the real architecture prerequisite and xUnit executor. A local before/after comparison retained all six independent compiler and host assertions. Durations below are per case from TRX and include concurrent test activity.

| Case | Engine checkout fixture | Small SDK fixture |
| --- | ---: | ---: |
| Compilation inputs change during execution | 9m12s | 1m48s |
| Changed property variant | 8m41s | 1m37s |
| Compiled host exclusions vs missing test | 6m51s | 1m30s |
| Reference selected during a build | 6m25s | 1m55s |
| Changed union roots, stable dependency | 5m31s | 1m32s |
| Closing read fails after passing checks | 2m58s | 24s |

These six cases and the real failing-xUnit fixture passed together in 7m14s. The failure fixture took 43s in this batch and ran the original frozen plan through the real architecture prerequisite. The complete validator unit project passed 94 cases in 10m19s. The complete validator integration project passed 20 cases in 7m39s on the same host. The source architecture check covered all 55 changed C# files. These suites are selected for validator changes.

The mixed PR run exposed an incorrect exact hit for the Browser Wasm32 archive. Windows completed four real desktop consumer builds before the Browser Player Support Pack test rejected five changed native project inputs. The catalog lookup had checked the requested archive digests and omitted their recorded source identities. Replaying the same published selection at its original master commit (`ed416cb09`) preserves the exact hit. Replaying it against the changed native project files selects just the Browser Release Player for production. The planner reads committed tree identities and rejects dirty source paths before acquiring the ZIP. Seven focused backend input cases passed, including rejection of changed source underneath the same certified digest. In that hosted run the macOS native preparation and backend build completed; GitHub reported that its subsequent engine job could not acquire a hosted arm64 runner. [Hosted jobs](https://github.com/Wolfiora/InnoEngine/actions/runs/45461586611)

The following hosted run rebuilt the Browser package. Its 114 recorded native inputs included the five updated project identities; its native library ZIP digest stayed identical. The new Release Player consumption check ran for 3m22s and the original Support Pack regression passed in 5m24s. Windows passed 2,843 cases from 54 xUnit projects and validated the two publication inputs. macOS built its package and executed its engine checks, but its validator fixtures inherited the outer runner's shared-test allocation and omitted their own managed prerequisites. The hosted planner now receives this allocation explicitly. An eight-second fixture checks both a secondary-host plan and an independent local plan against the same project. [Platform results](https://github.com/Wolfiora/InnoEngine/actions/runs/45493673535)

### PR backend candidates at landing

For a published-package miss, the landed push can publish a certified target from the same-repository PR or merge queue. The producer commit may differ from the landing commit when their Git trees agree. The temporary Actions artifact supplies the original bytes. The published catalog records the landed revision and the producer's run, attempt, artifact ID and digest.

On Windows with SDK 10.0.402, the candidate lookup and ZIP transfer fixture passed in 1.02s, the affected-target routing fixture in 0.43s, the native preparation fixture in 5.08s, and the publisher fixture in 2.34s. Together they cover a successful target beside a failed sibling, skipped verification, expiry, changed bytes, a different tree, a fork, forced production, and independent publication with preserved producer identity. Release-hit routing schedules zero builds and creates zero candidate uploads. These timings cover local fixtures. Production release writing requires a human merge.
