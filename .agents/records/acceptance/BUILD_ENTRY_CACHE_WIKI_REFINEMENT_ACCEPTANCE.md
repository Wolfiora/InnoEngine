# 开发入口、缓存与 Wiki 整改：执行中

更新：2026-10-12。基线 HEAD：`8070302d0daa7ea19b2211b224c2bd6cb4dca1b9`。
保留既有未提交修改；没有提交代码。以下结果只对应本轮明确记录的命令，不能解释为整份计划完成。

## 用户问题与处理状态

| 编号 | 当前处理 | 尚需完成 |
| --- | --- | --- |
| B01 | 两入口改为 InnoEngine.Editor / InnoEngine.Build；严格命令解析、默认增量、显式 clean | 完整 IDE 和产品启动验证 |
| B02 | 组件显式执行源码闭包，删除全平台工具目录与全绑定目录默认扫描 | 当前目标/无关目标修改的实际 Native 失效矩阵 |
| B03 | 独立 support-pack 命令可用；发布内容完整性与读取保护已接入 | preparationIdentity、准备前索引及完全跳过 Native/managed 准备仍未实现 |
| B04 | 产品/目标/配置清理计划、预检、产品共享/独占保护 | 活动产品、dry-run 和完整失败边界实测 |
| B05 | 稳定 workspace 与最终 fingerprint 分开；固定输入，等内容不改时间 | 实际小改动增量、并发与源文件删除实验 |
| B06 | Native/Binding/Pack 返回值拥有读取 leases；绑定消费副本脱离缓存生命周期 | 统一 owner 注册、当前依赖根保护、预算及自动/手动回收未完成 |
| B07 | 请求初始及锁后各验证阶段复用 ProjectGraph；Build 生命周期成功结果复用 | 实测仍多次私有 publisher bootstrap；准备次数和整体成本未收口 |
| B08 | BGFX 工具由集成贡献，共享后端删除 OS 路径 fallback | 工具失败矩阵与全产品构建 |
| W01 | 232 个有效项目有唯一同名项目页；生产符号清单持续生成 | 所有页面的职责/所有权逐页复核，不以页面存在证明质量 |
| W02 | 历史计划与验收迁入 records；当前说明独立 | 全部正文旧行为排查仍在进行 |
| W03 | 当前 Wiki 已去除已识别的特定外部产品引用 | 全内容边界最终检查 |
| W04 | 全签名、参数、返回值和异常从当前语义/XML生成，记录待编辑复核 | 中文行为说明、继承/组合边界逐项人工复核 |
| W05 | 项目与 Solution 对应检查通过，链接门禁已建立 | 最终导航、锚点与移页链接检查 |
| W06 | 每个 C# 示例独立编译；宿主与逻辑脚本引用环境隔离，显式 reference 身份及哈希校验 | 最近完整审计 182 例中 154 通过、28 失败；此后修复及逻辑引用接入尚需完整复跑 |
| W07 | 已有 Wiki 项目/API/链接/内容/示例检查及正反例测试 | 尚未作为默认仓库通过门禁；不能报告全仓通过 |

后端程序集保留 Native、Adapter、Toolchains 的职责命名。源码属于 extern/backend；工作目录属于 backend obj/native；最终 Native 产物属于 artifacts/native。Temp 短路径是受控执行别名，不是新的长期成品 owner。

## 验证证据

根：`artifacts/acceptance/build-entry-cache-wiki/20261011-implementation`。下列测试存在交集，不能相加宣称独立总数。

| 证据 | 结果 |
| --- | --- |
| logs/cli-ownership-build.log | CLI 及闭包编译通过，0 warning/error，91.91 秒；不是热 Native 测量 |
| results/entry-native.trx | 25 通过 |
| results/native-task-regression.trx | 17 通过 |
| results/artifact-lease.trx | 22 通过 |
| results/binding-lease-and-pack-2.trx | 14 通过，仅实际匹配 Binding 测试；名称不代表 Pack 验收 |
| results/pack-repair-binding-consumption.trx | 15 通过，含 Pack 修复及真实 SDK 绑定消费 |
| results/build-ownership-regression.trx | 65 通过，含入口、workspace、身份、发布、绑定、Pack、Task |
| results/binding-materialization.trx | 4 通过：保留消费副本、损坏保护、绝对路径、锁等待取消 |
| results/wiki-compile-closure-final.trx | 16 通过：文档契约、编译依赖及旧命令正反例 |
| logs/evaluated-architecture.log | 有效 MSBuild 项目图 232/232 通过；不等于全部 Wiki/产品通过 |
| results/solution-page-inventory.json | 232 项目 / 232 Solution 项目；无失效路径、空 Folder、重复权威项目页 |
| logs/wiki-reference-and-example-audit-3.log | 全量 Wiki 审计仍失败；失败项在持续修复 |
| logs/wiki-complete-product-reference-audit.log | 本次全量自动审计通过：184/184 C# 示例、11,967 个公开符号映射、项目覆盖、链接和内容边界；Host 编译引用仍是仓库可用集合，严格项目闭包与人工行为审查未完成 |
| logs/windows-editor-preparation.log | Windows Editor Debug 正常准备成功，总计 2588.03 秒；内部 Native 构建有 C4819/MSB8029 警告，不能以末尾零警告汇总代替整体 |
| results/wiki-script-references-fixed.trx | 19 通过，包含逻辑 reference 角色、冻结身份和宿主实现隔离 |
| results/wiki-all-pages-boundary.trx | 26 通过，补充架构/索引页示例及外部产品名称、旧 CLI DLL 启动正反例；与前项重叠 |
| results/export-shutdown-native.trx | 1 通过：Editor 退出等待导出准备任务清理；Native 文件来源及 SHA-256 见 export-shutdown-native-deployment.json |
| results/authoring-annotation-loading.trx | 2 通过：真实模块加载、Player 擦除及普通 Editor API 拒绝 |
| results/interop-restore-ownership.trx | 3 通过：真实 SDK 加入传递引用后，仅直接引用项目拥有隔离 restore |
| results/task-publisher-scope.trx | 1 通过：两个真实 SDK Build，各自只有一次 publisher 编译；并行菱形闭包保留四个私有加载目录，跨 Build owner 不复用 |
| results/windows-editor-annotation-rebuild.json | 普通入口构建成功，2650.37 秒；日志仍有 17 次 publisher 编译，不能视为成本整改完成 |
| results/windows-editor-annotation-smoke-30000.json | 修复后 30000 帧正常退出，282.77 秒，无活动错误诊断；未独立证明脚本 Ready/Play/视觉状态 |
| results/windows-editor-smoke-600.json / windows-editor-repeat-smoke.json | 三次 600 帧后台启动/退出通过，15.23 / 10.60 / 10.45 秒；未运行编译工具，不是视觉或 Play 验收 |
| results/windows-editor-smoke-30000.json | 失败：151.71 秒，退出码 1；发现游戏脚本引用纯创作注解时被模块加载边界拒绝 |
| results/editor-picker-observation.json / picker-forced-cleanup.json | 无参数启动出现目录选择窗口；桌面锁定，未输入。测试自有空选择窗口已强制清理，不算正常退出验收 |

失败记录：`support-pack-lease.trx` 为 52 通过、1 失败，失败因 Editor 退出测试缺少 libcimgui-debug；补齐明确的测试 Native 部署后，该失败项在 `export-shutdown-native.trx` 单独复验通过。原失败日志保留。曾发生自身并行编译导致 CS2012，改为顺序构建后通过，原日志保留。

## 基线与清洁度

已保存 revision、原有 staged/unstaged diff、项目依赖和磁盘信息。实施前完整 API、persistent identity、性能未采集，后采集数据不充作基线。
file-map.tsv / wiki-page-map.tsv / wiki-api-map.tsv 是执行清单，仍有 Pending/editorial-review-required，未宣称完整闭环。
Windows Editor 准备后记录 C 盘空余 133,246,312,448 bytes。未清理用户、SDK、NuGet 或浏览器全局缓存，未启用未经保护计算的缓存删除。

## 运行、性能与环境限制

用户已授权本轮 Computer Use。实际桌面锁定，停止输入，尚无本轮 GUI/视觉/焦点通过证据。CoreCLR、NativeAOT、Web 解释与 AOT、消费者、reload、三次热构建与并发性能均待执行。Build 后三次短启动已执行，但长运行暴露的重载失败需要独立复验。

2026-10-12 后台复验已经完成 30000 帧并正常退出；初始脚本状态没有独立日志证据，不将无诊断推断为完整激活。产品完整构建的 Native 汇总为 4 个工具进程、1 次 binding batch、1 次 binding generation、17859806527 哈希字节、93068258 materialization 字节；仅后续 shader-tools 阶段为 0 个 Native 进程。不同阶段不可混为整个构建的统计。

Wiki 使用本轮已构建 Windows 产品的程序集及其 `.deps.json`，经公开 ScriptCompiler.GenerateProjectFiles 重新生成逻辑引用。首次验证程序漏带 `.deps.json`，导致四个 Editor 示例缺少导出契约；补齐完整产品声明后通过，未更改示例以回避该错误。`wiki-reference-project` 为本轮专属空项目，未替换用户项目的 API 缓存。原失败日志保留；`wiki-example-map.tsv` 已更新为本轮编译结果。

随后增加 operation-owned publisher 编译复用，真实并行 SDK fixture 已通过，普通产品复验位于 `logs/windows-editor-shared-bootstrap-build.log`。编译目录共享与私有加载目录隔离分别验收。C 盘可用空间约 125.64 GiB，未清理全局缓存。

## 本轮运行发现与修复

纯注解项目 `Inno.Editor.Annotations` 的 `AssemblyScope.Editor` 与它在创作态 Runtime 脚本中保留的注解引用冲突。其加载范围已改为 Runtime；`ScriptingApiScope.Authoring` 保持不变，Player 编译仍擦除应用及自定义派生注解。未放宽模块加载器的 Runtime → Editor 禁止规则。专项测试现在真实加载候选模块，并验证 Player 不引用该程序集。

实际准备中发现 `RestoreInnoInteropRuntime` 在 SDK 添加传递引用后选择 BGCS Runtime，导致许多无直接引用的 Feature/Panel 重复准备私有 publisher。已在求值阶段固定直接引用，由声明依赖的 owner 执行隔离 restore；传递引用的目标与输出属性继续保留。正常产品复验日志为 `logs/windows-editor-annotation-rebuild.log`，最终结果由同名 JSON 记录。

原准备日志记录 57 个不同私有 publisher 目录、1 次完整 Task runtime prepared、2 次 reused；这是日志标记统计，不是完整 OS 进程追踪。此次正常构建包含构建机制修改，不能当作连续热构建。
macOS Native/产品需要 macOS；Linux 当前只有已有 Native 工具能力。提示音 Glass.aiff 在当前环境不可用。

## 2026-10-12 SDK-backed Wiki closure validation

- `wiki-sdk-closure.trx`: 27 passed. A real SDK fixture accepts the owner's contract and rejects an unrelated assembly that merely exists elsewhere in the repository.
- `wiki-sdk-audit.log`: the first strict run exposed 10 examples using undeclared references. The diagnostics are retained.
- The sample import compiler workflow moved to the Scripting Compiler page; BGFX source analysis moved to the BGFX tooling page; input event dispatch moved to Core Events. Lower-level pages now demonstrate their own contracts and link to the composing layer.
- `wiki-sdk-audit-fixed.log`: complete automated audit passed, including 232 evaluated project owners. `wiki-sdk-audit-summary.json`: 186 examples, zero failures (158 host-sdk, 16 runtime-script, 11 editor-script, one framework example), 11,967 public symbol mappings.
- This closes the example reference check only. The API source model still needs exact compilation-input review, and whole-page behavior/ownership review remains incomplete.
- `RepositoryWikiTests` is being integrated into the regular test suite. Its first run lacked the complete Editor metadata closure; the shared Hosting reference and public metadata roots have been added. Do not count this durable gate as passed until its rerun finishes.
- `editor-workspace-restoration.json`: after saving the smoke-generated editor.ini as evidence, restored the original pre-test project layout. No Editor process was using the file.

## 2026-10-12 02:48 — prepared Editor and evaluated Wiki source inventory

- Normal Windows Debug preparation completed successfully: 2653.71 seconds, zero warnings/errors. This run included source/target changes and concurrent documentation tests; it is not a hot-build baseline.
- Publisher compilations: 3. Task runtime: 1 preparation, 2 reuses. Native preparation: 344640.9 ms, 28658667090 hashed bytes, 16 tool processes. Shader tools separately reported 5013449274 hashed bytes and 2 native processes.
- Three subsequent background 600-frame Editor runs used the prepared deployment and exited with code 0. No entry build preparation appeared in their logs. Complete run durations were 23.05, 21.52 and 20.22 seconds, including 600 frames and cleanup. Managed test compilation was concurrent; these are smoke results, not controlled startup timings or visual acceptance.
- The prepared manifest covered 522 files. Sample editor.ini was restored byte-for-byte after these tests.
- Wiki source analysis now uses the selected SDK Compile items, ReferencePath, defines, nullable and unsafe settings. The public fixture verifies Compile Remove and conditional source selection. All 27 Wiki contract tests passed in 51 seconds. A subsequent constructor optimization avoids collecting unrelated deployment references in SDK mode; final build/audit is pending.
- The normal Build test closure and durable RepositoryWikiTests are still running. The full audit must be rerun with the stricter source model; the earlier 186-example audit does not certify the new source model.
- A real Windows CMake workspace regression has been added and is pending execution. It checks changed native return values, same-length/same-mtime input invalidation, retained unchanged object files, and a third request without compiler execution.
- B03 preparation-before-build cache and B06 cache ownership roots/budget retirement remain incomplete. Desktop remains locked; no GUI interaction/visual result was claimed.

## 2026-10-12 03:00 — stricter source gate and ownership regressions

- `results/wiki-lazy-documentation.trx`: 27 passed. XML documentation is loaded lazily so an unused malformed third-party documentation file does not prevent reference resolution; requested malformed documentation still fails explicitly.
- `results/native-workspace-real.trx`: 3 passed, including a real Windows CMake build. Changed source preserved length and mtime, changed the DLL result, retained the unchanged object, and the third request started no native producer.
- `results/native-task-ownership-regressions.trx`: 19 passed for publication, phase verification and Task ownership.
- `results/repository-wiki-sdk-source-current.trx`: failed. Exact SDK source analysis requires a Windows-target FileSystem compile reference which has not been built. The test's Tooling DLL was hash-checked against its owning build before execution. This is an unresolved validation prerequisite, not a passing gate; no foreign-configuration DLL fallback was introduced.
- The earlier broad API source audit and the 186-example audit remain historical observations. They do not certify the stricter source gate. Whole-page editorial review is also incomplete.
- A first current hot Editor build was started separately; results will be recorded when it finishes. No performance improvement is inferred from the preceding preparation build.

## 2026-10-12 03:15 — reproduced broad managed invalidation

- `selection-regression-before.trx`: three failures reproduced with the original implementation: a new selection import recompiles a neutral SDK project, identical selection bytes rewrite the import timestamp, and an unselected target mapping changes the selected generation identity.
- The diagnostic `editor-hot-1` run was intentionally terminated after 693.03 seconds once these defects were confirmed. It is not a completed hot-build measurement. Its Native phase completed in 59936.6 ms with zero native processes, zero binding generation, zero materialization, and 14753134836 hashed bytes. An isolated SDK regression ran concurrently near the end, so total duration is not a controlled benchmark.
- Common MSBuild now imports binding selections only for binding-owning projects, after their explicit BindGenGeneratedBindings declaration. The product SDK intermediate directory owns the stable selection file. Identical bytes preserve its timestamp; changes use AtomicFile. Build/Publish retain it until product clean, while Publish still checks the exact compiled selection.
- Binding identity records the selected definition facts and actual input content. Unselected target mappings no longer invalidate the selected generation. Fresh phase validation rereads the definition and still rejects changes to the selected contract.
- `selection-regression-after.trx`: 19 passed, including generation, definition rejection, selection ownership and the real neutral-project incremental build regression. The exact Toolchains and Bindings DLLs copied to the test output were verified by SHA-256.
- Real generated-source compilation regression and normal product preparation remain in progress. The previous ready receipt must not be used as proof for the interrupted diagnostic build or these new edits.

- The Wiki tool and its isolated tests now explicitly use Roslyn 4.14, matching the version already resolved in the Build/BGCS tool closure. The previous isolated 4.8 parser silently treated an unknown SDK language version as Preview. Unsupported language versions now fail explicitly. `wiki-parser-identity.trx`: 27 passed, including a C# 13 params ReadOnlySpan example and a rejected unknown language version.

## 2026-10-12 — Architecture command fixture closure

- Architecture rule fixtures now invoke the existing public `ArchitectureValidator.Execute` boundary. They no longer prepare the full development Build entry for every negative fixture. Console diagnostic capture has a serialized test collection and is restored in `finally`.
- `architecture-command-direct.trx` retained the initial three failures: native assembly fixtures still used the removed root `native/` directory. Updating those fixtures to their real backend ownership fixed the failures without weakening the assertions or changing production policy.
- `architecture-command-fixture-fixed.trx`: 119 passed, 41 seconds. The test project compiled with zero warnings/errors.
- `architecture-route.trx`: one passed through public `BuildApplication.Run`, verifying that `verify` reports an invalid source without preparing Task runtime or creating build artifacts.
- `native-binding-compilation-selection.trx`: one actual SDK generation/consumption test passed in 12 minutes 3 seconds, including shared/static target selections, transitive selection, and missing-selection rejection. This duration includes fixture SDK preparations; it is not an Editor startup measurement.
- The normal Editor preparation with selection-import fixes is still running. No new hot-build or GUI success is inferred from these test results.

## 2026-10-12 — Prepared Editor startup ownership

- `DevelopmentEntry.RunEditor` now acquires checkout and product reading ownership before its first deployment validation. The normal prepared path hashes the deployed file set once. After preparation and lock transitions it still performs a fresh validation.
- SDK-host resolution is deferred until actual preparation is needed. Missing products retain the existing explicit preparation path.
- Both owners are released on normal exit and application exceptions; product-release failure cannot skip checkout release.
- `prepared-entry-locks-final.trx`: two passed in a separate process through the public startup entry. A fixture product verifies exclusive access to both lock files is denied during execution; after normal or exceptional return, the test reacquires both locks. This exercises managed startup and ownership, not actual SDL/BGFX or visual output.
- Bootstrap, Editor entry, and Build tests compile with zero warnings/errors. The full Editor product build remains a separate running verification.
- `prepared-entry-integrity.trx`: three passed, including same-length/same-mtime damage rejection before application startup. The former two-case results remain historical evidence. `prepared-entry-bootstrap-identities.json` confirms the tested Bootstrap DLL and the actual Editor entry's Bootstrap DLL have the same SHA-256.

## 2026-10-12 — Selection fixes: real Editor preparation and direct startup

- `windows-editor-selection-fixed-build.json`: normal `InnoEngine.Build editor --configuration Debug` succeeded, 2843.19 seconds. This includes changed generation/build implementation, not a hot measurement. Nested Native logs contain C4819/MSB8029 warnings despite the outer zero-warning summary.
- Native preparation reported 464757.6 ms, 28413035298 hashed bytes, 16 native processes, 8 products, one binding batch, nine generation operations, one managed process, and 342212660 materialized bytes. Counts describe this changed-input preparation only.
- `editor-selection-prepared-smokes.json`: three real 600-frame Editor runs succeeded with no build preparation. Console-observed deployment-ready times: 1.514 / 1.286 / 1.359 seconds; first-frame times: 8.808 / 4.920 / 5.015 seconds. Total runs including 600 frames and shutdown: 17.229 / 14.013 / 13.575 seconds.
- The actual tested Bootstrap DLL matches the Editor entry's copy by SHA-256. The original project editor.ini was saved before testing and restored byte-for-byte after each run, with each generated layout retained as evidence.
- These runs do not prove Play, gameplay, visual effects, focus, sound or GUI interaction. Computer Use remains authorized, but no new visual interaction result is asserted.
- Three consecutive unchanged-source hot builds are now running; their results will be recorded separately in `editor-selection-hot-builds.json`.

## 2026-10-12 — Controlled unchanged-source Editor builds

- `editor-selection-hot-builds.json`: three consecutive actual `InnoEngine.Build editor --configuration Debug` requests succeeded in 283.95 / 274.16 / 259.33 seconds. Source remained unchanged and no competing heavy builds or tests were started during this series.
- Each Native stage reported zero compiler processes, zero binding generation, zero input materialization, and 14753122058 hashed bytes. Native phase times: 33.47 / 30.12 / 30.81 seconds. Shader-tool validation independently reported 3325211162 hashed bytes and zero Native processes each time.
- Each request prepared Task runtime once and reused it twice. Private publisher bootstrap compilation remains a material cost; these results do not close B07 or establish an overall performance improvement against a controlled old implementation.
- The sampled neutral assembly timestamps remained unchanged. This confirms the binding selection import no longer forces the observed unrelated managed recompilation.
- No GUI action was performed during the measurements. The Computer Use window inventory exposed no Editor; the earlier locked-desktop restriction remains unresolved.

## 2026-10-12 — Current repository rules and additional compiler inputs

- `current-repository-architecture.trx`: the actual checkout passed the public ArchitectureValidator entry in 19 seconds. This covers repository rules; it does not close the stricter SDK Wiki gate or product execution matrix.
- `windows-player-wiki-contract-build.log`: Windows Player managed contract build succeeded in 158.67 seconds, zero warnings/errors. It consumed the already selected Windows binding closure with Native preparation disabled. This is not a published or running game.
- The next strict Wiki run reached macOS Player and rejected its missing macos-arm64 FileSystem compile reference. `macos-filesystem-managed-contract.log` prepared that exact managed closure successfully in 76.35 seconds. No macOS Native compilation or execution was performed.
- `task-compiler-input-before.trx`: four failures reproduced stale Task runtime reuse after changes to AdditionalFiles, EditorConfigFiles, GlobalAnalyzerConfigFiles and AdditionalCompileInputs. File size and mtime were held constant.
- TaskRuntimeRequestKey now hashes those evaluated compiler input items in the same per-phase input set. `task-compiler-input-after.trx`: all 22 runtime/publication tests passed. TaskHosting and the affected tests compile without warnings/errors. No public API was added.
- Core IO documentation now distinguishes atomic rename from reader coordination; Rendering Runtime documentation explains borrowed device/contributor ownership and the Session-driven frame/retirement sequence.

## 2026-10-12 — Strict Wiki environment diagnostics

- `repository-wiki-after-target-contracts.trx` still failed: macOS Player also lacked its exact target Default Adapter contract. A no-reference build confirmed missing macos-arm64 metadata rather than a source syntax error.
- A complete macOS Player managed preparation on Windows reached the platform SDK guard: the MacOS provider rejects macos-arm64 binding preparation on Windows/x64. No Windows binding was substituted and no macOS execution is claimed.
- Wiki validation now records missing evaluated compile references per project, keeps the run unsuccessful, and continues collecting diagnostics/inventories for available projects and examples. Unavailable projects are not reported as deleted API symbols and are not counted as verified contracts.
- `wiki-missing-contract.trx`: 27 cases passed, including exact SDK references, unavailable contract diagnostics, continued guide example validation, and rejection of an unknown compiler language version. Source/tool and test builds had no compilation errors.

## 2026-10-12 — Resolved Task package input integrity

- `task-package-input-before.trx` reproduced stale reuse after same-length/same-mtime mutation of a restored package compile asset.
- Task request identity now includes actual selected NuGet target assets, analyzer package inputs, the restored manifest and imported NuGet build definitions. Missing selected files fail before compilation. No package cache is modified by production validation.
- `task-package-input-final.trx`: 25 passed. Coverage includes compile/runtime/analyzer changes, unchanged and unselected-framework inputs, missing selected output, cancellation, locking and publication. The mutation fixtures operate only in their own temporary package roots.
- `task-package-input-real-bootstrap.log`: actual `dotnet msbuild build/tasks/Inno.Build.Tasks/Inno.Build.Tasks.csproj -t:PrepareInnoBuildTaskHost -p:Configuration=Debug -nologo -m:1 -nr:false` succeeded with the real restored dependency graph. This does not establish a general preparation identity for arbitrary managed build targets or close Support Pack caching.
- C: free space observed after verification: 133004271616 bytes. No global SDK, NuGet or user cache cleanup was performed.

## 2026-10-12 — Complete strict Wiki diagnostic pass

- `repository-wiki-current-tool.trx`: failed after 10 minutes 12 seconds, with 7,143 diagnostics. The tool assembly in the test output was compared by SHA-256 with its owning project's output before execution (`wiki-current-tool-identity.json`). An earlier run retained an older transitive copy of the tool and is not evidence for the revised diagnostics behavior.
- The retained inventory is `artifacts/acceptance/wiki-gate/89f1e805b5a94d83a6410fbbb01ffbb6`: 19,080 symbols from available projects, plus example results. The larger set includes generated members of partial source types. Their missing mappings are real documentation work, not permission to exclude those members from the gate.
- Four logical Editor examples fail because the test Host's declared closure lacks Settings/Shaders contracts. Their project-local scripting exports exist. The test must use the actual complete Editor product contract rather than substitute implementation namespaces or add a central scripting allowlist.
- Exact compile references remain unavailable for MacOS Editor, MacOS Player and Browser Player. The attempted macOS managed build took 6 minutes 47.85 seconds and reached five explicit Windows-host SDK rejections. No foreign ABI output was substituted.
- Repository ownership evaluation remains 232/232. Missing references continue to fail the gate while inventories and independent diagnostics are retained. This run does not close W01–W07.
- A new Native workspace regression reproduces generation-specific bridge paths changing compatible workspace identity (`workspace-binding-before.trx`). Final artifact fingerprints must remain content-sensitive; copied bridge locations and compatible intermediate ownership still need correction.

## 2026-10-12 — Evaluated generated contracts and Editor script fixture

- `wiki-evaluated-input-coverage.trx`: 27 passed. API discovery now enumerates actual SDK Compile inputs, including selected generated-only and nested types. A fixture includes one generated contract from obj and rejects an unselected sibling. This corrects the earlier handwritten-type-only discovery gap; full API documentation remains incomplete.
- The Wiki script fixture imports the same shared EditorProduct.props used by real Editor products. `wiki-product-contract.trx`: 1 passed, confirming the generated logical references contain Settings and Shader authoring contracts without substituting implementation namespaces.
- A new complete strict run is retained separately as `wiki-selected-generated.trx`; until that run completes, these fixture results do not establish repository-wide Wiki success.
- Native Text workflow documentation was checked against the facade, implementation and binding mappings. It now distinguishes Runtime ownership, copied font bytes, borrowed pointer wrappers, UTF-8 cluster offsets and buffer-size queries. The remaining generated member inventory is still pending.

## 2026-10-12 — Independent Windows Support Pack publication

- `InnoEngine.Build support-pack --target windows-x64` completed with exit code 0. Published content fingerprint: `b30045b17548fdabb827e19a3118aba3b041cacff9f48cecb5f20e8c3870f385`; the current index selects that directory and the operation-owned candidate was removed.
- `windows-support-pack-current.log` retains the complete output; `windows-support-pack-current.json` records file count, size and runtime closure checks. The Player managed-build subprocess reported 18 minutes 37.13 seconds. This is not the total command duration. Its nested cold BGFX tool build reported 8 minutes 2.89 seconds and native compiler warnings.
- This run completed before the generated-input workspace fix and validates independent preparation/publication only. It does not prove preparation-before-build cache hits, final game publication or GUI behavior. Tests executed concurrently, so it is not a controlled performance comparison.

## 2026-10-12 — Stable copied binding inputs

- `workspace-binding-before.trx` reproduced generation paths entering workspace configuration. The additional aggregate/external-input cases were added during the fix and have only an after result.
- Internal `NativeBindingInputPaths` maps the operation's selected generation roots to Native-owner-specific copied input locations. Workspace declarations normalize only those selected roots and aggregate generation declarations. Complete artifact fingerprints still include the original declarations, files and generation identity. External selected binding outputs are copied as frozen inputs; SDK paths remain external.
- `binding-workspace-after.trx`: 17 passed in 31 seconds. Includes repository/external binding caches, standalone/aggregate declarations, immutable artifact separation, unchanged-object CMake reuse, source mutation, corruption repair, cancellation and publication ownership. `binding-workspace-tool-identity.json` confirms the tests consumed the current owning Toolchains assembly.
- Toolchains and test builds succeeded with zero warnings/errors. `architecture-binding-workspace.trx`: the current repository architecture test passed in 21 seconds. No public type or member was added for the workspace mapping.
- `wiki-selected-generated.trx` used an older transitive tool copy, as its recorded hashes demonstrate, and is not evidence for generated-only API discovery. It does verify that the corrected Editor fixture produces 186 example results without compilation errors under that earlier validator. A subsequent run asserts identical owner/consumer tool hashes before starting (`wiki-generated-current-identity.json`).

## 2026-10-12 — Current generated API inventory and nested Task ownership

- `wiki-generated-current.trx` failed after 7 minutes 59 seconds using the owning Tooling assembly, with SHA-256 equality asserted before execution. The retained inventory `artifacts/acceptance/wiki-gate/c1420af70d3446a6ad9a685c148d39dd` contains 45,441 public/protected declarations and 186 examples with no compilation errors. Exact foreign target references and generated API documentation remain incomplete; this is not a passed Wiki gate.
- Native Text now documents all 124 selected declarations against its handwritten facade, actual implementation and generated call bodies. This includes pointer aliases, enum values, UTF-8 byte lengths, ref overload capacity obligations, FunctionTable context ownership and Runtime disposal. Generated binding files were not modified. `text-api-review.json` is the per-symbol review inventory; the page's new example compiled with zero warnings/errors (`text-api-example.log`). No claim is made about all other Native project pages.
- Nested Task preparation now forwards the current `InnoTaskBootstrapOwner` explicitly. Request identity excludes the random execution-owner value itself while retaining evaluated source/import/compiler effects. `task-nested-owner.trx`: 27 passed, including reuse across owner changes, invalidation when that owner changes DefineConstants, cancellation, restored-package content identity and publication protection. The real SDK diamond graph test is tracked independently; unit results are not a substitute for it.

## 2026-10-12 — Actual nested publisher regression closed

- The first real SDK diamond run failed its one-publisher assertion despite a successful build (`task-publisher-nested-owner.trx`). The MSBuild task API applied undefinitions to the final child request and removed the explicitly supplied bootstrap owner. Two bootstrap compilations were observed; the unit mock had not reproduced that SDK behavior.
- BuildTaskRuntimeTask now removes a name from the undefinition list when the frozen final request explicitly supplies it. The public Properties documentation states this precedence. Unit coverage supplies the same name in Properties and RemoveProperties.
- `task-owner-undefinition.trx`: 27 passed. `task-publisher-owner-final.trx`: the actual two-root-build, parallel diamond graph passed in 3 minutes 10 seconds. Each root build asserts one publisher compilation, four private load paths and one common bootstrap owner; the next root build asserts a different owner. This duration is not a before/after performance comparison with the failed run.
- Native UI's 314 selected public declarations now have checked behavior, ownership, pointer/Span constraints and frame/event consumption rules (`ui-api-review.json`). Its event-copy example compiled with zero warnings/errors (`ui-api-example.log`). Neither this nor the Text example is an actual GUI run.

## 2026-10-12 — Post-fix build series and resumed GUI acceptance

- `editor-owner-workspace-builds.json`: one preparation after source changes completed in 1520.31 seconds; subsequent hot requests completed in 172.42, 151.49 and 146.45 seconds. All returned zero. Preparation reported 16 Native process invocations and two Shader-tool invocations; every hot request reported zero Native/Shader-tool invocations, zero binding generations and zero materialization bytes. Task runtime preparation/reuse counts were 1/2 per request.
- Each hot request still read 14,753,165,672 Native input bytes and 3,325,220,854 Shader-tool input bytes. These measurements do not establish that remaining input-analysis cost is closed. The earlier 284/274/259-second series predates the final fixes and is not a controlled before/after comparison.
- `editor-current-gui.json` and `editor-gui-boot.log` record actual Windows Debug/Direct3D11 UI observations using Computer Use. Native folder selection, rendering, Play, a focused GameView button, stop, a native additional Game window, window displacement and close were observed. Closing the additional window reduced active views from five to three and left primary rendering active; Console displayed no diagnostics.
- The first primary close recorded state capture, run-loop termination and Dispose end. A second no-argument entry launch reopened the same project through its folder dialog. The second close retained an additional Game window at shutdown and still completed Dispose. Tests saved the post-GUI editor.ini and restored the pre-existing captured settings; SHA-256 equality was checked.
- A attempted viewport corner drag did not change geometry and is not accepted as a resize test. Menu reload with unchanged sources showed no error but did not provide generation/GC barrier evidence. Drag latency, multi-monitor DPI, audio, night rendering and final game/Web publication remain pending.
- `wiki-ref-signatures-final.trx`: 28 Wiki contract tests passed. Type accessibility/class modifiers and ref/ref readonly returns now participate in signature identity. The first test exposed duplicate readonly/ref formatting; that failure is retained in `wiki-ref-signatures.trx`, and the final implementation uses Roslyn's own readonly/ref rendering.
- Native ImGuizmo's selected 153 declarations were reviewed against generated calls and native behavior. Its begin/end string projection flaw was discovered during that review. Consumption configuration is being narrowed and regenerated; this entry is not yet a passed corrected-binding acceptance.

## 2026-10-12 — ImGuizmo consumption contract closed and Wiki signatures refreshed

- The ImGuizmo component configuration excludes unused SetID and begin/end native ID projections. The two-string projections previously allocated unrelated UTF-8 buffers although native code subtracts end from begin. Native source and independent BGCS code were not changed; HostSource was regenerated through GenerateBindings. The component now declares Style_Style as owned NativeFunction storage paired with Style_destroy, and GetStyle as borrowed.
- `gizmo-final-generation.log` and `gizmo-native-managed-build.log`: generation/managed compilation succeeded with zero warnings/errors. `gizmo-current-native.trx`: three actual Native calls passed, covering loaded exports, owned Style destruction with the global borrow preserved, and integer/single-string ID scope restoration. The consuming DLL SHA-256 was checked against its owning assembly; native DLLs were verified against the prepared product receipt before deployment to the test owner.
- `gizmo-api-example.log`: current page example compiled with zero warnings/errors. `gizmo-removed-api.log`: actual C# compiler rejected SetID and both two-string ID calls with CS0117/CS1501; the verification checked nonzero compilation and expected diagnostics.
- `wiki-ref-current.trx` remains a failed strict gate, retaining a fresh inventory of 45,436 declarations and the example results. This run used the current Tooling hash asserted before launch. The five removed ImGuizmo declarations account for the inventory reduction.
- `reviewed-signature-refresh.json`: 12,551 existing mappings and 12,550 existing declaration rows were refreshed for type modifiers/ref-return display and source locations while retaining their behavior prose. Missing declarations were not filled with generic placeholders. Text (124), UI (314) and ImGuizmo (148) checked tables were rebuilt from the current inventory; remaining editorial review stays open.

## 2026-10-12 — Native source declaration separated from selected output

- NativeBuildRecipe.CreateForComponent now declares handwritten source, executor and SDK inputs without requiring existing generated output. NativeArtifactPublisher attaches the current operation's binding/bridge files and exact generation identity before capturing complete inputs, acquiring workspace ownership or running a producer. Missing generation still fails before staging and production; final fingerprinting includes actual generated bytes.
- Toolchains and Build.Tests compile with zero warnings/errors. `native-input-declaration-final.trx`: 27 passed, including four new closure cases and existing identity, publication and real CMake workspace regressions. The owner/consumer Toolchains DLL hashes were checked before testing. The first new fixture lacked explicit SDK selection and failed before publication (`native-input-declaration.trx`); it was corrected to use a declared fixture toolchain.
- No public member was added. The factory's public behavior documentation was updated. This prerequisite does not implement a complete Support Pack preparation identity or pre-build index; backend declaration factories, binding input planning, effective managed inputs and cache coordination remain necessary.

## 2026-10-12 — Backend declaration factories

- SDL, MiniAudio, Text and UI now expose CreateRecipe and consume it in their actual standalone BuildAsync. The shared publisher binds generated content and bridge location before final fingerprinting. Bridge location normalization is limited to the operation's selected roots, preserving compatible workspace identity while artifact identity remains complete.
- Text/UI input lists use their backend-owned handwritten include/src/CMake declaration instead of the entire Native directory, excluding unselected HostSource bridge output and its coordination file. Actual CMake receives the operation's selected bridge only inside the producer scope.
- Removed unused internal Sdl3BuildConstants, Sdl3BuildUtils, MiniAudioBuildConstants and MiniAudioBuildUtils after checking callers; their paths were also removed from recipe identity. Both owning projects compile.
- `bridge-source-declarations.trx`: 31 passed, including source-only declarations, missing generation, output byte/generation invalidation, host-output exclusion, corrupt/canceled publication and real CMake reuse. `bgfx-source-declarations.trx`: 13 passed, including BGFX runtime/tool declarations, profile validation and the common source cases.
- BGFX preparation and execution now share internal BgfxBuildRequest. Public runtime/tool CreateRecipe methods expose the source declaration without Native generation, compilation or staging. Source validation order is preserved. New public factories are build-planning boundaries, are not scripting exports, and borrow the immutable operation/profile.
- These factories are prerequisites. Product plan registration, pure binding planning, complete managed input identity, preparation index and cache retirement are still outstanding; no whole-plan completion is claimed.

## 2026-10-12：锁屏中断后的输入规划与 Pack 缓存继续实施

- ImGui/ImGuizmo source recipe 工厂已补齐；ProductNativeBuildStep 强制 standalone producer 与 declaration 同时注册，static aggregate 保留实际 definition，所有发行消费者已更新。`product-source-plans.trx`：30 项通过。
- 新增 ManagedBuildInputSnapshot，由工程选定的 SDK 求值完整 ProjectGraph，独立保留编译输入和 SDK/package 内容校验。SDK evaluator 为缓存的库 Task，无新增生产 Program；请求目录完成后删除，有效 evaluator output 受读取 lease 保护。`managed-input-snapshot-final.trx`：真实 SDK fixture 通过，覆盖相同身份、同长度同 mtime 修改、import/resource/source 和新文件。
- INativeBindingGenerator 新增 source input fingerprint 契约；实际生成与 preparation input identity 分开。selected config/base/include/generator/extension 输入进入身份；未选择的独立 target config 内容不参与。pure planning 源文件不进入 actual generation implementation source identity，避免无关准备逻辑使 Native binding generation 失效。
- PlayerSupportPackPlan 强制 preparationIdentity 和 fresh VerifyInputsAsync；Publisher 在 preparation 前查询 target/.preparations，完整内容验证后持有读取 lease，最终 source gate 成功后才能选择 current。损坏/额外内容需要重建，Input 变化不误当成 output cache miss。
- `pack-preparation-final-gates.trx`：26 项通过；`pack-preparation-cache.trx`：19 项通过（与前者存在重复，不能相加视为唯一测试数量）。专项覆盖并发只准备一次、命中跳过 producer、同长度同 mtime 篡改修复、额外文件、ownership wait 后或 validation 中 input 修改拒绝，并保持 prior current。
- 完整 CLI managed 项目闭包编译成功，0 warnings/0 errors。真实 Windows support-pack 请求仍在执行，尚不能报告第二次请求、最终发布或性能完成。
- 真实架构门禁发现一个新 partial 类型声明缺少 XML，待当前输入验证构建结束后补齐并重跑；原始失败日志保留。
- B06 owner registry、依赖保护根和统一预算/退休仍未实施完成。完整 managed 输入冻结执行和瞬态改动验证、全量 Wiki 语义/逐页审查、发布/UI/性能矩阵仍是剩余任务。当前新增缓存不能作为整份计划已完成的声明。
- 证据目录：artifacts/acceptance/build-entry-cache-wiki/20261011-implementation；磁盘检查约 117 GiB 可用。本轮未清理用户/SDK/NuGet 全局缓存，也未自动提交。

实际 Windows 首次请求：Player Release managed Build 完成，0 warnings/0 errors，约 16 分 40 秒；仍因随后补齐 Shader 输入声明导致的 input 变化而在最终 gate 拒绝选择。未标为通过，原 Pack current 保持。Native/Shader 工具缓存保留；source input 声明和 partial XML 已补齐。

## 2026-10-12：真实准备及第二次请求

- 命令：`entry/InnoEngine.Build/bin/Debug/net9.0/InnoEngine.Build.exe support-pack --target windows-x64`。
- 当前源码固定后的准备成功，退出码 0，总时间 239.325 秒；Player Release Build 为 0 warnings/0 errors，MSBuild 报告时间 128.53 秒。
- 第二次相同请求成功，退出码 0，总时间 69.549 秒。返回同一内容身份 `85503ec434394bd5b72f6c064e2ad0efb59cdbd67b93af9f7bc5419f52e896bc`。
- 热请求日志没有 Player Build、Native compiler 或绑定生成记录；入口自身 CLI 托管构建仍耗时约 39.84 秒。此处为日志观察，不是操作系统工具进程计数。
- 结果：`support-pack-complete-input-real.json`、`support-pack-complete-input-hot.json`；原始日志：`support-pack-complete-input-real.log`、`support-pack-complete-input-hot.log`，均位于本轮 artifacts 证据目录。
- `architecture-complete-preparation-inputs.trx` 当前真实架构门禁通过；之前缺 partial 类型 XML 的失败保留作为修复过程证据。
- 仍未完成：managed 编译执行的输入冻结、统一缓存预算/退休、入口准备成本收口、全量 Wiki 及产品发布运行矩阵。这些结果不构成整份计划完成。
- C 盘可用空间本次复核为 124,990,763,008 bytes（约 116.4 GiB）；没有清理 SDK、NuGet 或用户全局缓存，没有自动提交。


## 2026-10-12：缓存退休基础协议与 Wiki 核对

- 新增中立的 BuildCacheOwnerDescriptor、BuildCacheArtifactDescriptor、BuildCachePolicy、BuildCacheCleanupPlan 与 BuildCacheCleanupResult。owner 明确提供根、协调文件、产物 inventory、保护根；artifact 明确提供实际 producer/reader lease、依赖、逻辑大小及使用时间。没有按目录名或后缀发现 owner。
- 退休重新检查所有权及依赖，零等待跳过活动 reader/producer；活动父产物保护依赖，失败父产物同样保留依赖。拒绝循环、未登记依赖、重叠边界、重复 lease 或位于待删目录内的 lease。已不存在的目录不计为回收。
- 返回 retiredBytes 明确为逻辑文件大小；不能把它当作 hardlink/压缩场景实际释放的卷空间。自动/手动标准登记链仍未完成，未启用自动回收。
- cache-owner-explicit-leases.trx：17 项通过（12 项退休协议，5 项 Pack fixture）；0 skipped。cache-retirement-and-pack-regression.trx 的 14 项为较早重叠批次，不与此相加。
- 架构基础门禁 architecture-cache-retirement-contracts.trx 通过；后续显式 lease 参数与资产修复仍需最终复验。
- ArtifactKey 的默认空值此前 value 返回 null，并与显式空 key 不相等；现在由同一 getter 规范化为空字符串。运行内容摘要此前借用可变列表，现复制 source 值为只读 inventory。公开类型身份与签名保持。asset-contract-owner-regression.trx：26 项通过，0 skipped。
- 严格 SDK Wiki 中间检查 wiki-current-preparation-contracts.trx 未通过，原始输出和 api.json 保留于 wiki-gate/77359e58023149f89511a99417bd95f8。该时点 45,459 个声明、189 段示例无编译错误，主要缺口为四个 Native 项目的生成成员。它不是最终当前源码通过证据。
- 已按上述真实符号补齐四个 Native 页的可折叠 ABI 索引，新增 32,854 个映射；未注释成员只描述 ABI 形状，不虚构 Native 操作语义。原生行为审查仍未完成。没有编辑生成绑定或 extern。
- Artifact/部署摘要补充 34 个已读源码契约；已有 12,574 个 Wiki 映射及 12,570 行声明按当前签名核对。数量不等于全部页面人工审查完成。
- 当前 Wiki 验收页已压缩为当前结果、限制和未完成部分；历史详细过程保留在 records。没有自动提交。

当前缓存/资产修复后的真实架构规则门禁 architecture-current-cache-assets.trx 已通过（18 秒）。当前 CLI 闭包编译成功；缓存管理 29 个公开声明已按实际语义清单补入 Toolchains 页。该清单来自无 SDK 精确引用参数的诊断捕获，不替代最终严格 Wiki 门禁。

下一阶段明确剩余：标准发行 publisher/产品/Pack 保护根登记及自动/手动缓存退休；managed 输入冻结执行；Build 入口重复托管准备成本；生成 ABI 的原生行为审查与最终 SDK Wiki/示例门禁；所有当前源码产品/消费者发布运行及性能矩阵；未实测平台与 GUI 项目的独立记录。当前没有整份计划完成声明。C 盘可用约 116.96 GiB；本阶段未删除用户/SDK 全局缓存，未提交。
