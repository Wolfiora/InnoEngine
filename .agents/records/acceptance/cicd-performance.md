# Engine CI/CD acceptance

## Current CI backend prerelease

The three registered targets passed full functional validation at `0fed32331010ddf20b2a96ea13ed45b94b7b5533`. The package publisher checked the complete gate receipts and the platform consumption receipts before promotion. Repeating promotion at the same inputs selected the same release. On independent hosted workers, a cold download of each published archive and a required warm cache hit supplied working product consumers. [Full engine gate](https://github.com/Wolfiora/InnoEngine/actions/runs/44519061480) · [Cold and warm consumers](https://github.com/Wolfiora/InnoEngine/actions/runs/44567924287) · [CI prerelease](https://github.com/Wolfiora/InnoEngine/releases/tag/backend-prebuilts-2026.11.19.2)

The functional gate passed 356 required checks on Windows (3,314 test cases) and 352 on macOS (3,311 cases). Nine and thirteen checks respectively had explicit platform exclusions. The full run included fresh platform production, certification and product execution. The wall time to the gate was 4h24m01s, including a 15-minute hosted runner-capacity rejection. The active critical path was 4h09m39s. Windows functional execution took 2h35m20s and macOS took 1h51m26s.

| Target | ZIP bytes | Manifest files | Cold SDK consumption checks |
| --- | ---: | ---: | ---: |
| windows-x64 | 62,172,452 | 615 | 4 |
| macos-arm64 | 66,657,994 | 615 | 4 |
| browser-wasm | 11,653,750 | 125 | 1 |

The planner uses effective compiler inputs to select backend projects by target and configuration. Changed projects and their downstream consumers enter production. The publisher assembles each ZIP from the current file inventory, so retired projects and variants disappear on the next publication. Release retention allows the current CI prerelease and two rollback versions. Product version tags follow the independent product release workflow.

A branch commit gets quick feedback. An open PR and a merge-queue candidate get affected functional checks. Master expands product and packaging coverage. Nightly covers the registered functional matrix and weekly covers performance and lifecycle. A host with a single matching archive can plan and execute in place. If a PR needs a producer, its final runner freezes the executable plan against its own environment. The final gate checks every required host receipt. Source builds remain the default developer workflow.

The current weekly caller passed on both desktop hosts in 38m57s, with ten required performance or lifecycle checks and 81 exclusions per host. The workers consumed the published desktop digests and ran no backend production or publication jobs. GitHub rejected the original weekly caller before assigning a runner. The nightly caller had the same permission declaration. The caller job now admits the reusable workflow's job permission ceiling. The shared validator grants read access to testing jobs. Weekly rejects missing precompiled packages before scheduling production. [Startup rejection](https://github.com/Wolfiora/InnoEngine/actions/runs/44576057746) / [Corrected weekly caller](https://github.com/Wolfiora/InnoEngine/actions/runs/44577168827)

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

The event chooses the depth of validation. A branch without an open PR gets fast checks. An open PR or merge-queue candidate gets affected integration tests on the registered hosts. A master push covers affected product and release behavior. Nightly checks the full functional matrix and can reuse matching, complete acceptance. Weekly runs performance and lifecycle checks. API export has its own preflight and publishes to Docs draft after integration.

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
