# 平台职责、BGCS 3.0.1 与缓存治理验收

## 范围与证据边界

实施基线为 `dotnet10` 的 `18f281510daf48968bdd7a60248e17ece9fbe420`。实施前四个测试锁文件已有修改，已保留并逐文件合并。原始状态、完整日志、binlog、TRX、采样与包哈希位于仓库本地 `artifacts/acceptance/platform-build-2026-10-22/`；该目录是可重建的验收产物，不进入 Git。

本文按实际命令区分通过、修正后复验、外部阻塞和未执行；原始失败日志保留。当前 API 与职责见[平台所有权](../../../docs/architecture/NATIVE_BUILD_PLATFORM_OWNERSHIP.md)、[缓存协议](../../../docs/architecture/BUILD_CACHE_ARCHITECTURE.md)和[平台扩展指南](../../../docs/architecture/PLATFORM_EXTENSION_GUIDE.md)。

## 已实现的实际边界

| 实际 owner | 实现与调用链 |
| --- | --- |
| 共享 backend | 七组件各自唯一的 `Bindings/bindings.props` 和 `bindgen.json`；保留组件语义、facade、生成扩展及可移植 recipe，删除组件中的目标 JSON 与未实现的 Linux 预设。 |
| 平台基础 build | 唯一目标 descriptor、SDK provider、冻结编译器输入、链接能力及格式；基础模块不引用 backend 或 BGCS。 |
| 平台 Build integration | 官方 BGCS Profile、组件参数、Native 产品贡献与部署布局；macOS 的兼容头和加载布局归本平台。 |
| 构建宿主 | Windows junction 在独立 Windows Host；macOS 宿主实现位于已有 build 项目；Browser 只借用中立 workspace 能力。 |
| 标准产品库 | `Inno.Build.Products.Standard` 声明组件组合及逻辑依赖；根发行仅注册完整平台贡献及托管部署 compiler。 |
| 中立执行 | `NativeProductContribution` 明确绑定计划和执行器，Static/Shared 与逐组件/聚合正交；结果统一持有绑定及产物读取保护。 |
| 产物协议 | `NativeArtifact` 显式声明 SharedLibrary、ImportLibrary、StaticArchive、SupportFile，角色清单参与完整性校验；ImGuizmo 不再通过替换后缀推断链接库。 |
| Core IO 与缓存 | `CoordinatedFileLease` 复用 FileLease，在等待和持有叶锁期间保留 owner 共享权；清理取得 owner 独占权，关闭叶锁后退休已证明归属的元数据。 |
| 成功根操作 | Editor 根 Build/Publish 与 CLI 共用有界维护；Task bootstrap 和子构建不重复维护。预算包含 owner 清单读取。 |

职责文件树在平台所有权页，本轮完整变更位置见[交付文件树](PLATFORM_OWNERSHIP_CACHE_FILE_TREE_2026_10_22.md)。新增项目均承载实际实现，没有为目录对称建立空项目。普通开发入口仍为 Editor 和 Build；没有启动时构建、预构建启动器或第三个日常入口。

## BGCS 的不可修改边界

统一声明精确固定 BGCS **3.0.1**。接入使用官方 `GenerationRequest.FromFile`、`WithConfiguration`、不可变配置、分析和生成 API。平台先解析 SDK 与有序编译输入，接入层不解释 INCLUDE、SDKROOT 或 EM_CACHE。`HostSource` 仅改变输出 owner，仍要求明确 ABI。

BGCS 工作区按 SDK 公开能力指定的整体目录登记，不解析内部缓存格式。引擎公共配置中的 `schemaVersion` 是 BGCS 官方配置字段，不是引擎旧格式读取协议。未创建 BGCS fork、私有反射、源码引用、包补丁或手写生成结果。

`bgcs-package-final.json` 对实施前 129 个 BGCS 3.0.1 包文件重新计算 SHA256：129 个一致、0 个变化。`bgcs-private-loads.json` 记录实际私有 Task 装载目录中的 BGCS 文件，其 ProductVersion 为 `3.0.1+e619418c0c9f8422aad706de9b6544de63ebeff8`。外部仓库状态见 `external-repositories-final.json`。

## 真实发现与修正

- CMake 已使用受控源码和输出路径，但进程工作目录仍指向原 checkout。跨卷、含空格且超过 260 字符的探针实际复现 Windows CreateProcess 错误 267；两个 CMake 调用统一改用冻结源码根，共享层未增加 Windows 或盘符判断。
- owner 协调预算原先未包含 catalog 等待；现在完整自动预算涵盖清单读取、规划和执行。阻塞 catalog 的公共边界回归覆盖此约束。
- 不受保护的已退出进程工作目录原先要等字节压力才入选；现在每次维护均选择此类工作目录，immutable generation 和 reusable workspace 继续按压力策略选择。
- 稳定 current 内容相同时保留时间戳；工作 claim、请求 index、登记与可退休叶锁在 owner 授权下收敛。未知记录、活动进程、等待者、损坏 current 和不能证明归属的路径均保护或明确报错。
- `verify-native` 在生成前拒绝无已注册 Editor runner 的目标，冻结一次 SDK，并使用 SDK ArtifactsPath 隔离验证输出；删除自定义 Build 工具输出路径协议及用户目录 SDK 推导。
- 隔离测试入口区分项目模式与 DLL 模式的 DOTNET_HOST_PATH 注入，统一使用选定 SDK；筛选同时作用于普通套件、隔离发现和隔离执行。
- 项目 CLI 的 game/plugin/scripts/import-sample 成功路径现先退出 Workspace 和 Native 读取保护，再调用同一根维护服务；失败与取消不进入成功维护，不新增并行清理系统。
- 游戏发布实际发现冻结源码树推测了错误的绑定产物 owner。generation、selection 和生成 Task 现传递真正的发布 owner，materialization 直接消费该输入；保留严格子路径与读取保护，并增加冻结源码消费原工作区产物的真实 MSBuild 回归。
- 共享 ImGui CMake 的 Windows 导出宏归 Windows integration 配置；共享 .NET 发布器的 RID 名单改由平台 contribution 的部署声明与 descriptor 提供。新目标 fixture 验证公共发布器可接受开放的 runtime target，并冻结调用方输入。
- 两个真实编译 fixture 改为提供完整的公开宿主/Profile 输入，没有生产 fallback、测试后门或放宽校验。
- 并发的源码构建与游戏发布实际复现普通 NuGet obj 还原图互相覆盖。辅助输入求值现使用 SDK ArtifactsPath 的请求隔离，额外输入项目使用各自子作用域；还原、求值、读取包和哈希期间持有既有 Core 协调保护。真实 App/Shared 与 Tool/Shared、RID/无 RID、并发普通还原回归验证原 obj 内容未被修改。
- Native 根目录在操作开始时解析一次物理位置，readonly source alias 与物理路径映射到同一冻结副本；外部 SDK 不重定向。真实 Windows junction + CMake 编译回归验证别名输入，源码副本仍为真实复制。ReusableWorkspace 不再写入不参与保护判断的逐进程 PID 元数据。
- 最终逐作用域检查以 locked restore 实际复现 macOS、Browser 及部分中立/RID 锁图的 NU1004。所有现有作用域通过官方 NuGet 还原更新，在独立 SDK ArtifactsPath 下再次 locked restore 通过；没有复用或覆盖 Windows 活动构建的 obj。这是托管锁图验收，不是跨平台原生或设备通过。
- 十次热构建门禁的第一次尝试明确失败：首次热构建仍有 8 个 Native 进程和 4 份绑定生成。独立 SDK 进程的公共 API 探针证明配置值完全相同，仅不可变字典的枚举顺序不同，原始 JSON 字节与引擎指纹随进程变化。引擎指纹边界现稳定排序对象键，保留数组顺序、全部输入与 fresh 校验；不修改 BGCS。`identity-cross-process-proof.json` 保存修正前复现，`hot-current-final/` 保留失败批次，新批次为 `hot-canonical-final/`。新增独立 VSTest 子进程回归同时检查相同配置复用和真实映射变化失效。
- 最后产品目录自查发现 Support Pack 将嵌套 Native 部署事务的工作 claim、协调锁带入不可变内容，使操作身份污染内容指纹。新增 `ProductNativeDeployment.StageAsync`，在调用方已独占的未发布候选中复用相同复制和验证机制；外层 Pack publisher 保有唯一事务。普通 `InstallAsync` 的原子替换行为保留，没有按后缀删除包内文件或跳过输入校验。回归覆盖两个候选内容一致、根目录不带操作元数据、已有 native 树拒绝、预取消和源产物损坏。
- 准备索引通过既有 `BuildCachePublisher.RecordIndex` 关联已发布 Pack，外层活动保护覆盖写入和登记；旧包退休时回收已登记索引。相同内容不重写索引和 current。新增公开边界回归验证旧包与旧索引退休、当前包及其索引仍保留，没有第二套清理协议。
- Pack 准备身份显式包含 Native 部署写入实现，写入逻辑变化会使准备缓存失效；不依赖手工删除旧包来触发新流程。
- 并发最终验收实际发现 Task 方法返回后，可复用编译 workspace 缺少贯穿 MSBuild 生命周期的保护。维护退休工作区后，同一 MSBuild 缓存的 Restore 结果不重新写入 assets，BGFX 与 ImGuizmo 后续 Task 因依赖未还原而失败。修复在输入求值前按路径顺序取得 host/load/workspace 的 owner 活动权，既有 load 进程登记引用 workspace；重复进程登记累积依赖，不能覆盖仍被使用的 host/workspace。没有新增 PID 协议、重试、Restore fallback 或 registry lease。

## 必要公开契约与稳定语义

| 契约 | 必要性与调用边界 |
| --- | --- |
| `CoordinatedFileLease` | 将既有 FileLease 组合成 owner 共享权与可退休叶锁；owner 权限覆盖等待和持有全过程，叶锁先释放，协调锁不删除。普通 FileLease 语义不变。 |
| `PhysicalPathResolver` | 多个构建读取方复用唯一的 phase-local 物理路径解析；只读、非线程安全，不授予写权限。每次 fresh 校验重新创建，mutation 仍须独立 PathBoundary 和 lease。 |
| Host / workspace provider | 平台贡献真实宿主工具能力和物理/执行目录关系，中立层不创建 Windows junction 或跨平台依赖。 |
| `NativeCompilerInputs` / toolchain selection | SDK owner 完成发现后冻结编译输入、环境、工具和闭包；下游不再重新猜测 SDK。 |
| Product contribution / executor / result | 将执行方式与链接方式分开，结果统一拥有读取保护；平台聚合与共享逐组件执行通过同一入口组合，失败不发布。 |
| `NativeLinkPlan` / `NativeArtifact` | 显式表达链接能力、实际文件及角色，避免文件后缀推导；不支持的组合在 staging 前失败。 |
| `ProductNativeDeployment.StageAsync` | 让已拥有外层事务的 Pack 准备复用完整复制与校验，候选内不创建第二套锁、claim 或发布状态。候选必须由调用方独占，且不存在 native 树；失败必须废弃整个候选。普通应用输出使用独立的 `InstallAsync` 原子替换。 |
| BGCS Profile / binding descriptor | BGCS 官方类型只停留在绑定接入与平台 integration；中立生成器消费已冻结请求。descriptor 明确提供原发布 owner，冻结源码的消费者无需猜路径。 |
| Platform managed deployment IDs | 平台声明实际可用的部署与 RID；共享 .NET compiler 接收冻结集合，不维护平台表。开放目标 fixture 验证接入。 |

上述变更不增加游戏/插件领域 API、兼容 overload、全局服务或并行持久化协议。辅助托管求值 scope 为构建内部机制，不新增要求用户重复填写的公开配置。

初步构建和无部署测试的失败日志保留。构建期间继续修改输入导致一次 Native 候选被完整性检查拒绝；这是已执行的失败保护证据，不作为最终构建通过证据。未准备 Native 部署的测试批次已中止，后续以完整产品部署重新执行。

最终手动 SourceDebug 命令曾误传基础 `ProductBuild.props`，触发 ImGui 对 Editor Shader 输入的明确拒绝；CI 已使用正确的 `EditorProduct.props`。按平台 Editor 工程声明修正命令后，完整 SourceDebug 构建通过，0 警告、0 错误，13 分 49.85 秒。没有增加共享层默认值或跳过 Shader 校验。

完整普通套件发现三项测试调用/fixture 未同步：长路径 ToolRunner fixture 未传入明确的 Windows workspace provider；两个 Task publication 用例仍期待发布时自动退休，并以旧 PID 标记伪造退出状态。测试现通过公开 provider、真实 SDK/MSBuild 子进程和统一维护协议检查活跃读取者、进程退出、依赖保护及退休。相关 11 项先复验通过；字典身份修正后扩大到全部绑定相关与上述两个类，该阶段 72/72 通过，0 跳过、0 失败；`canonical-related-tests.log` 和对应 TRX 保留阶段结果，最终相关范围扩大为下述 175 项。旧失败及早期 TRX 均保留，不重复累加。

workspace 生命周期修正后，扩大到绑定、Task runtime/publication、缓存与维护、Support Pack、路径、Native publication 和 executor 的 175 项：173 通过、2 项已有 Unix 条件跳过、0 失败。统一入口的 Shared 与 Discovery 通过，本次筛选实际发现 0 项隔离用例；不虚构本轮隔离执行。全套此前 28 个隔离阶段仍按原范围报告。另单独执行预求值工程/外部 SDK worker 注册回归，1/1 通过。最终证据为 `lifecycle-related-tests/`、`lifecycle-registration-tests/` 和 `resolved-test-results.json`。

新增跨进程用例的首轮 fixture 把结果写入监测的头文件搜索目录，正确触发目录成员身份变化；源码与结果隔离后独立用例通过，随后纳入上述 72 项全部通过。未放宽目录成员、文件字节或真实配置变化的失效检查；`canonical-tests.log` 保留该 fixture 失败，`canonical-process-tests.log` 和 `canonical-related-tests.log` 保存修正后的证据。

Support Pack 收尾复验的早期 122 项批次有两项失败：一项在编译期间遇到尚在修改的输入，被 Task 完整性检查明确拒绝；另一项 Editor shutdown 用例通过 DLL 直接运行但未准备 Native 部署，被加载器明确拒绝。原始 `owned-stage-tests.log` 和 TRX 保留。最终相关批次固定源码，通过正式产品准备与统一普通/隔离入口执行，不修改生产校验或测试隔离要求。

新增 Stage 方法的首轮正式门禁发现 9 处 XML 元素未展开，已按仓库排版要求修正，原 `wiki-owned-stage-final-run.log` 保留。另一轮 SourceRelease 与运行中的验收 CLI 共用普通输出，产生 55 个 DLL 复制重试告警，最终编译 0 错误；该批次不记作零告警通过。最终使用 SDK 独立 ArtifactsPath 的源码全集复验，保留各次结果，不放宽文件锁或工程规则。

`owned-stage-current-build.log` 保存上述真实 workspace 生命周期失败，不能归类为单纯命令或 fixture 错误。修正后的定向测试 61 项中 59 通过、2 项 Unix 条件跳过、0 失败。真实 SDK 产品准备期间另执行三次公开缓存清理，72 份已选 assets 与 workspace 全部保留；后续 Task 复用成功，完整构建最终 0 警告、0 错误。见 `workspace-live-maintenance-results.json`、三份清理日志和 `workspace-race-fixed-build.log/binlog`。32 分 45.42 秒包含等待其他 Native producer、绑定与 C++ 冷准备，不是热构建性能数据。

## 最终验证结果

最终游戏复验脚本首轮漏传必填的 `--support-packs`，在参数校验阶段拒绝，尚未创建游戏发布事务。补齐该参数，并同步修正 `PRODUCT_STARTUP.md` 的命令示例；没有修改生产参数协议或增加默认 fallback。原始错误保存在 `game-coreclr-canonical-argument-failure.log`，后续发布结果分别记录。

| 检查 | 当前记录 |
| --- | --- |
| 长路径与跨卷 CMake | 通过：D 盘物理工作区、含空格、263 字符，C 盘宿主执行映射，三次内容变化成功、第四次热命中；`path-probe-results.json`。此探针验证真实公共 publisher/executor，不等同于完整引擎跨卷 clone。 |
| SourceRelease | 通过：最终 workspace 生命周期修正后的源码全集，SDK 独立 ArtifactsPath，0 警告、0 错误，8 分 56.46 秒；`source-release-lifecycle-final.log/binlog`。新增项目使用实际 Release 配置。此轮在并行验收负载下执行，不作为空闲性能基线。 |
| SourceDebug | 通过：最终 workspace 生命周期修正后的正式 Native 命令完成独立 SourceDebug 全集构建，0 警告、0 错误，12 分 19.58 秒；完整统计见 `verify-native-lifecycle-final.log`。错误命令及前序结果单独保留。 |
| 普通与隔离测试 | 完整批次原始共 2,155 项：2,134 通过、3 失败、18 跳过；59 个发现阶段及 28 个隔离执行阶段全部通过。最终相关回归 175 项中 173 通过、2 项已有条件跳过、0 失败，覆盖三项旧失败与三项新增回归；本次筛选没有隔离用例。合并同名用例后为 2,140 通过、18 跳过、0 未解决失败，共 2,158 项；不能把原始 Shared 阶段改写为通过。`target-corrected-final-tests/`、`lifecycle-related-tests/`、`resolved-test-results.json`。 |
| Core IO | 最终完整测试包含 26 项 Core IO 用例；结果计入上述全集，不重复累加。 |
| Stage / Pack 索引 | 最终源码的 18 项公共边界回归全部通过，0 跳过、0 失败；`final-stage-index-targeted-tests.log` 及 TRX。包括新增的旧包与准备索引共同退休用例；与后续全集合并时按用例去重。 |
| Windows Editor Release | 通过：最终源码使用独立 SDK ArtifactsPath 构建，0 警告、0 错误，15 分 07.73 秒；`editor-release-lifecycle-final.log/binlog`。根维护保持 5 秒预算，不扩大预算或屏蔽告警。 |
| 七组件 HostSource | 通过：逐组件官方 GenerateBindings 和 CheckBindings，明确 Windows ABI；`host-binding-summary.txt` 及十四份生成/检查日志。受版本控制的生成清单、Text/UI bridge 更新均来自此流程，没有手改生成代码。 |
| FlappyBird Editor Debug 产品 | 通过：最终 workspace 生命周期修正、十次热构建后的实际产物，D3D11、D3D12、Vulkan 各 3,000 帧，均 exit 0 并完成 Dispose；`editor-debug-lifecycle-final-results.json`、`editor-debug-*-lifecycle-final-smoke.log`。 |
| FlappyBird Editor Release 产品 | 通过：最终源码产物 D3D11、D3D12、Vulkan 各 3,000 帧，均 exit 0 并完成 Dispose；`editor-release-lifecycle-final-results.json`、`editor-release-*-lifecycle-final-smoke.log`。只检查本次 stdout。 |
| FlappyBird CoreCLR / NativeAOT | 通过：最终源码的两种正式发布均完成冻结源码编译及严格闭包校验；各自 D3D11、D3D12、Vulkan 3,000 帧，6 次 exit 0，每帧 2 views / 25 draws；`game-coreclr-lifecycle-final.log`、`game-nativeaot-lifecycle-final.log`、`game-lifecycle-final-results.json`、`game-lifecycle-final-binaries.json`。 |
| FlappyBird GUI | 通过已执行部分：Play、鼠标开始、N 昼夜切换、Ctrl+P 返回 Edit 并恢复场景、最大化/还原、Console 无运行期诊断、窗口正常关闭与保存；`flappy-gui-result.json`。Space 重新开始未单独确认，音频听感和长期运行未执行。机器同时具备 AMD 核显与 RTX 3070 Ti Laptop GPU；设备与驱动见 `gpu-device.json`，没有推断各 API 实际选择的 adapter。 |
| Native 专项正式命令 | 通过：最终源码的 `verify-native --target windows-x64 --configuration Debug` 完成七组件确定性比对、Native 构建、无手写 import 审计、独立 SourceDebug 完整构建及 7 个测试工程；36 项通过、0 跳过、0 失败。`verify-native-lifecycle-final.log`、`native-acceptance-lifecycle-final-report.json`、`native-lifecycle-final-totals.json` 及报告指向的 TRX；不与全集重复累加。 |
| 正式 Wiki 与架构 | 通过：最终 workspace 生命周期修正后的正式投影、238/238 工程 SDK 求值与完整架构/Wiki 门禁；`wiki-lifecycle-final-run.log`、`wiki-lifecycle-final/api.json`、`examples.json` 与 `wiki-lifecycle-final-totals.json`。32,551 条 API 记录覆盖 151 个含公开 API 的项目，包含 Stage 的真实签名；非所选平台产品编译保持未求值标记。 |
| 当前 Wiki 示例 | 196 个示例均编译通过，0 失败、0 未求值；未选平台产品编译与本行示例计数分别报告，不混为实机通过。 |
| 最终交付链接 | 57 个新增或修改 Markdown 页的 1,905 个本地文件链接全部存在，缺失 0；`delivery-links-final.json`。完整 Wiki 门禁另行验证 API、项目覆盖和示例，不以链接存在代替其结果。 |
| 测试 fixture 与交付自查 | 进程 publication fixture 后通过完整架构检查，238/238 工程 SDK 求值；`architecture-post-fixture-final.log` 与工程图。热构建随后发现的字典身份问题另列真实修正及复验，不把此前结果冒充之后状态。 |
| 插件与项目 CLI | 通过：最终源码对 Canvas、Rendering2D、FlappyBird 逐个执行正式 scripts --ide 和 SDK IDE 编译，全部通过；`scripts-lifecycle-*.log`、`ide-lifecycle-*.log`。插件源未修改，无需重导出安装包。 |
| 现有锁图作用域 | 通过：13 个正式 locked restore 入口检查覆盖全部六种现有作用域，包括中立、三个目标、无目标/RID 及 Windows/RID；相关初始 NU1004 经官方 force-evaluate 更新后再次 locked restore 通过。`locks-*-final.log`、两份 `locks-*-final-results.json`。 |
| BGCS / 源码完整性 | 129 个包文件哈希不变，实际装载版本 3.0.1；BGCS 与三个调用方仓库保持干净。工程 XML、所有锁文件版本和手写生产源码的禁止导入规则检查无违规；`final-integrity.json`。 |
| 十次连续热构建 | 通过：最终 workspace 生命周期修正后的相同源码、SDK 和 Debug 配置下，十次均 Native processes=0、bindingGenerations=0；22 个 Native/Binding current 的数量、内容及时间戳全部保持。每次完整日志和 binlog 在 `lifecycle-hot-final/`，汇总为 `lifecycle-hot-final-summary.json`。各次早期失败、修正及先前完成的批次分别保留。 |

18 项跳过均有明确条件：16 项需要 macOS Metal 原生编译器，2 项需要 Unix 文件链接语义；完整名称与原因在 `skipped-tests-final.json`。没有按 Binding 名称或 BGCS 类别整体排除测试。

## 缓存维护、元数据与空间

只清理正式登记且证明归属的 owner。首次 dry-run 报告 1,017 个候选，删除和回收字节均为 0；随后的显式维护成功退休 **51,934,408,110 逻辑 bytes**，0 失败。十次构建和产品烟测后再次显式维护，成功退休 **4,098,292,962 逻辑 bytes**，0 失败。命令及日志分别为 `cache-explicit-final-dry-run.log`、`cache-explicit-final-clean.log` 和 `cache-convergence-final-clean.log`。

| 23 个登记根的只读快照 | 逻辑 bytes | lock / work.lock | 工作 claim | 缓存登记 | current | PID 标记 |
| --- | ---: | ---: | ---: | ---: | ---: | ---: |
| 首次维护前 | 80,583,479,851 | 1,718 / 294 | 278 | 800 | 38 | 768 |
| 首次显式维护后 | 43,389,451,308 | 573 / 16 | 0 | 30 | 38 | 0 |
| 十次热构建后、再次显式维护前 | 47,487,890,063 | 659 / 17 | 1 | 97 | 38 | 16 |
| 再次显式维护后 | 43,389,451,307 | 573 / 16 | 0 | 30 | 38 | 0 |

维护前后的稳定数量及相同分配空间证明本次已完成操作的数据能够收敛，不要求所有 lock 文件归零。普通 FileLease 的锁文件、永久协调锁、未知历史内容及不能证明归属的元数据仍保护；不能按后缀或年龄把剩余文件认领为可删除缓存。

两次维护后的实际分配量均为 **27,409,440,400 bytes**：通过 Windows 文件分配信息及卷/文件身份去重 hardlink，排除 junction/symlink 和重叠路径。该值仍可能与登记根外的 hardlink 共享空间，不能解释成独占可释放空间；逻辑回收量也不等于卷空闲空间增加量。43.39 GB 包含受保护的未登记历史内容，不等同于当前可退休缓存 inventory。所有只读快照均无枚举错误；证据为 `cache-before-final-maintenance.json`、`cache-after-explicit-final.json`、`cache-after-hot-final.json` 和 `cache-converged-final.json`。

自动维护保持每个成功根操作一次、64 GiB / 20 GiB 空闲阈值 / 5 秒预算。十次日志均有根维护统计，活动 owner 或预算延期单独显示；显式维护用 600 秒预算确认进程退出后的收敛。最终产品复验后的快照另行保留，不能用尚未退出的 worker 推断泄漏，也不强制终止用户进程。

最终生命周期修正、Native 专项、两种游戏发布和三组脚本/IDE 复验完成后，再通过公开维护入口清理。`cache-lifecycle-products-final.json` 的 23 个登记根为 **43,408,929,182 逻辑 bytes**、**27,429,161,376 去重分配 bytes**；lock / work.lock 为 **573 / 16**，工作 claim **0**、登记 **26**、current **39**、PID 标记 **0**，枚举失败 0。前表为较早阶段的收敛对照，不能与最终不同产品集合直接等同。最终清理明细见 `cache-lifecycle-products-final-clean.log`；实际分配仍受登记根外 hardlink 共享影响。

实际选中 Support Pack `5ad6f0577cabdbdae66091a3ef17a93726c78b486147459c83eb88becf6897b6` 的根目录仅有 `PlayerLink`、`References`、`native`，递归检查未发现事务锁、work claim、current 或 PID 标记混入内容；证据为 `support-pack-lifecycle-content.json`。历史被污染包已由公开维护整体退休，没有手改包内文件。

## 实现性能的受控对照

`resolver-performance.json` 使用 SDK 10.0.402、运行时 10.0.13、Release，固定 361 条实际 MSVC include 路径，先预热，再交错测量改造前的引擎路径解析实现与当前 Core 实现各五次；每条解析结果一致。控制 fixture 只复制引擎原实现，没有复制或修改 BGCS。

| 同一输入的指标 | 改造前 | 当前 | 归因 |
| --- | --- | --- | --- |
| 路径解析耗时中位数 | 56.4331 ms | 28.5175 ms | 约减少 49.46%，来自同阶段父路径复用及减少重复 metadata 查询。 |
| 线程分配量中位数 | 343,040 bytes | 306,872 bytes | 约减少 10.54%，来自同一实现的对象分配减少。 |

此处没有运行时更换因素，不作为整个构建、脚本或帧时间的提速证明。完整 Editor 没有改造前同条件五次基线，历史高负载单次耗时不参与前后结论；该比较缺口仍列入当前问题。

最终十次 Debug 热构建的总耗时中位数为 **151.392 秒**，最后五次中位数 **151.303 秒**。前一阶段同 SDK/配置采样的最后五次中位数为 **148.590 秒**，本次约增加 **1.83%**，没有超过 10% 的可重复回退。该阶段比较不代表整个分层重构前的基线；早期并行负载下的 402 秒等采样分别保留。热命中仍重新读取真实输入并校验完整产物，零 Native 编译不表示零文件 IO 或零 SDK 子进程。

## 复验入口与证据索引

从当前仓库根目录使用 `global.json` 选中的 SDK；`$dotnet` 必须是该 SDK 的实际 CLI。以下入口保留生产校验，不是跳过原生生成的替代构建：

```powershell
$dotnet = (Get-Command dotnet).Source
$product = Join-Path $PWD 'platforms/WindowsX64/build/Inno.Build.WindowsX64/EditorProduct.props'
& $dotnet build InnoEngine.sln -c SourceRelease --disable-build-servers -m:1 -nr:false
& $dotnet build InnoEngine.sln -c SourceDebug --disable-build-servers -m:1 -nr:false `
    -p:InnoNativeTarget=windows-x64 -p:InnoToolTarget=windows-x64 `
    "-p:InnoProductBuildProperties=$product"
& ./tests/Invoke-IsolatedTests.ps1 -Dotnet $dotnet -Configuration SourceDebug `
    -Target windows-x64 -ProductProperties $product
& ./tests/Invoke-WikiChecks.ps1 -Dotnet $dotnet -Configuration Release `
    -ProjectDirectory ../InnoEngine.Samples/FlappyBird -Target windows-x64
& ./tests/Invoke-BuildCacheChecks.ps1 -Dotnet $dotnet -Configuration Debug -Iterations 10
```

正式 Build entry 的 `verify-native`、`scripts --ide`、`game` 和 `clean --scope cache` 使用 README 当前命令。各次独立并发编译使用 SDK `ArtifactsPath` 隔离 bin/obj，不能将两个写入同一普通输出的进程当作并发验收。

| 证据 | 保存内容 |
| --- | --- |
| `sdk-info-final.txt`、`workloads-final.txt`、`compiler-version-final.txt` | 实际 SDK、运行时、workload 和编译器身份。 |
| `target-corrected-final-tests/execution-summary.json`、87 份 TRX | 原始普通、发现及隔离阶段状态；原始失败不覆盖。 |
| `lifecycle-related-tests/`、`resolved-test-results.json` | 最终契约的 175 项复验、原始三项同名失败消解关系和三项新增用例；原始阶段状态不改写。 |
| `wiki-lifecycle-final/` | 最终脚本投影、SDK 工程图、API inventory、196 个示例编译与门禁日志；早期门禁失败分别保留。 |
| `bgcs-package-baseline.json`、`bgcs-package-final.json`、`bgcs-private-loads.json` | 包内容不变及实际载入版本证明；版本声明不能代替运行时身份。 |
| `host-binding-summary.txt`、`native-acceptance-lifecycle-final-report.json` | 官方生成、检查、Native ABI 与测试结果；报告指向专项 TRX。 |
| `editor-*-lifecycle-final-*`、`game-*-lifecycle-*` | 最终 Windows 产品逐 API stdout、退出码和结果；不把历史追加日志当本次证据。 |
| `cache-lifecycle-products-final.json`、`cache-lifecycle-products-final-clean.log`、`lifecycle-hot-final/` | owner 清单、干跑/实际维护、逻辑/分配字节、稳定指针与十次热构建。 |
| `final-integrity.json`、`external-repositories-final.json` | 工程 XML、锁图 BGCS 版本、禁止导入、原有修改保留及外部仓库状态。 |

## 外部限制与未执行

- 原生完整日志保留 MSBuild `MSB8029`（TEMP 执行别名下的中间目录提示）、BGFX 第三方头在当前 936 code page 下的 `C4819`、`C4702` 及 ImGuizmo 第三方头共用导入宏产生的 `LNK4217`。没有修改第三方源码或全局禁用告警。SDK 根构建的自动维护预算告警另行统计；不能把托管汇总 0 错误描述成所有原生工具输出零诊断。BGCS 对未选入安全表面的指针报告也保留，未据名称猜测所有权。
- Browser 实际 workload 已选定，但 BGCS 3.0.1 正式生成以 `BGCS-CSHARP-ABI-PLAN` 拒绝 BGFX 按值结构体调用；SDK 尚无对应 aggregate calling profile。`browser-bindings-configured.log` 保留实际失败。没有借用桌面 ABI、伪造绑定或放宽校验；Mono Wasm / Wasm AOT 产品闭包受阻。修复边界见[专项记录](BGCS301_BROWSER_AGGREGATE_ABI.md)。
- 当前没有可用 Mac；本次真实 macOS ARM64、Metal、Unix 文件系统与设备验收未执行。跨平台契约和 fixture 不替代实机。
- 自动审批拒绝清理本轮新建的 `D:/InnoEngineAcceptance/691879f8041742a08f317fa64af5daa3`，仅返回 “blocked by policy”。已确认无 reparse point，73 个文件约 62 KB；保留该测试目录，不绕过审批。它不属于引擎日常缓存。
- 当前 Windows 无 `/System/Library/Sounds/Glass.aiff`，完成提示音无法按指定文件播放。

## 深层架构自查

检查覆盖入口 → 发行 → 平台 SDK/Profile → 绑定 → 产品执行 → 原子发布 → 托管消费 → 缓存维护，包含等待者、active reader、进程身份、取消和失败释放。扩展 fixture 使用全新的目标和 backend，Static 使用独立 producer，并通过原共享执行器；不支持的链接能力在生成与 staging 前拒绝。

新增平台需要实现本平台 SDK/Profile、产品及布局并注册贡献；新增 backend 需要组件实现、真实 integration 和发行注册。在已有 SPI 能力内，不向共同执行器或游戏领域添加平台/backend 分支。超出 SPI 能力时应正规扩展中立契约，不宣称任何新 SDK 均无需源码适配。

本轮 IO、进程 owner、路径、原子发布与 lease 复用 Core IO；没有新增事件总线、Identity 管理器或序列化旁路。实际仍开放的外部插件事件与跨帧 Identity 审计项由[当前问题](../../../docs/architecture/CURRENT_ISSUES.md)单列，不将本轮构建分层通过解释为整个引擎已经完美。持久自查要求已写入根 `AGENTS.md` 第 30 节。
