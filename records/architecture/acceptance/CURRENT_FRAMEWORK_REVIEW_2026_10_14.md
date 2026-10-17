# 当前框架审查与过期产物清理

[当前问题](../../../docs/architecture/CURRENT_ISSUES.md) · [上一轮验收](BUILD_COST_TOOL_HOST_DISK_CLOSURE_ACCEPTANCE.md) · [记录索引](../README.md)

## 结论与范围

2026-10-14，审查提交 `48b33e5d089e8a1500f03d1abae69fd61ffa9d62`。本轮没有修改生产源码、测试或第三方依赖，没有自动提交，没有使用 Computer Use。执行了源码审查、现有公开边界回归、帧分配测量和实际磁盘清理。

**整体分层清楚，扩展路径明确；但不能说没有问题。** 本轮发现条件性的文件系统正确性风险和具体性能成本。上一轮的本机通过结论只覆盖其验收范围，不替代本次发现。

日常两入口、平台与 backend 集成、领域中立、generation/Identity、资源 owner、缓存读取保护和原子发布保持有效。没有证据要求重新推翻整体架构。以下问题应在各自 owner 内修复；当前 Windows 结果不能证明 MacOSArm64 在所有卷格式上正确。

证据根：`artifacts/acceptance/framework-review-disk/20261014-current/`。测试、磁盘扫描、清理提案和执行逐项保存于 `results/`，日志保存于 `logs/`。

## 发现的问题

### F01：文件系统大小写策略以 OS 代替实际目录语义

优先级：**P1（在区分大小写的 macOS 卷上的条件性边界正确性风险）**。静态调用链确认，本机没有 macOS 实机复现。

位置：

- [PathBoundary.cs](../../../src/foundation/core/Inno.Core.IO/PathBoundary.cs#L161)：macOS 一律使用 `OrdinalIgnoreCase` 判断包含关系。
- [AtomicFile.cs](../../../src/foundation/core/Inno.Core.IO/AtomicFile.cs#L99)：同样策略用于源/目标与同目录约束。

macOS 支持区分大小写的 APFS，不能从 OS 推断目录等价。[Apple 官方文件系统说明](https://developer.apple.com/documentation/technologyoverviews/files-and-directories)。

例如区分大小写的卷同时存在 `/Work/Root` 与 `/Work/root`，候选 `/Work/root/item` 可以通过根 `/Work/Root` 的当前忽略大小写前缀检查，但它实际是另一个目录。`RequireUnlinkedPath` 后续调用的词法 relative path 与真正物理目录也可能不一致。

理想修复：由 Core.IO 统一定义安全路径边界与目录身份规则，严格拒绝真正的 sibling/traversal；不要给每个平台、缓存或插件另建判断。大小写敏感与不敏感目录均需测试；Windows 的目录特性也应明确，而不是再次把 OS 当作文件系统能力。原子安装必须同时验证真实同目录要求。修复后补 MacOSArm64 的两种卷格式实测。

这不是要求领域层新增平台分支；问题归属基础文件 IO。

### F02：每个 contributor 对累计 RenderGraph 做完整验证

优先级：**P2（已测量的帧 CPU/分配成本）**。

位置：

- [RenderRuntime.Contributors.cs](../../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.Contributors.cs#L99)：每个 contributor 加入后调用 `graph.Validate()`。
- [RenderGraphBuilder.Mutations.cs](../../../src/services/rendering/Inno.Rendering/Graph/RenderGraphBuilder.Mutations.cs#L83)：每次图变更使缓存失效，重新调用完整 validator。
- [RenderGraphValidator.cs](../../../src/services/rendering/Inno.Rendering/Graph/RenderGraphValidator.cs#L18)：重新验证资源、pass、依赖、存活与拓扑。

新测量使用现有公开生命周期测试，预热 16 帧、测量 64 帧、真实 RenderRuntime + fixture device；不是 GPU 性能或完整游戏帧时间。每帧最终完整 Compile 次数仍为 1。

| contributor 数 | 成功路径分配/帧 | CPU 毫秒/帧 |
| --- | ---: | ---: |
| 0 | 4,032 bytes | 0.01345 |
| 1 | 6,440 bytes | 0.01667 |
| 8 | 40,368 bytes | 0.13591 |
| 32 | 371,104 bytes | 1.57957 |

32 个简单 contributor 仅此 fixture 路径在 60 FPS 下约分配 22.27 MB/s。累计图被反复全量分析，随着贡献数量放大成本；不能从这一个场景推导所有图的精确复杂度。

理想修复：保留 contributor 独立事务、失败回滚、已接受 pass 不可修改和最后一次完整编译；在 Rendering Core 内让已接受验证状态可复用，增量验证本次追加声明与边界依赖，使用有明确 frame 生命周期的 scratch。不能简单删除中间验证后让错误贡献污染整帧，也不能把 backend 实现带入 Graph。

证据：`frame-allocation-review.trx`、`current-frame-allocation-measurements.json`。当前只是审查与测量，本轮未修改该算法。

### F03：Windows Build 工具部署携带全部 RID Native 资产

优先级：**P2（已确认的磁盘和校验成本）**。

位置：

- [Inno.Build.Cli.csproj](../../../build/cli/Inno.Build.Cli/Inno.Build.Cli.csproj#L4)：`CopyLocalLockFileAssemblies=true`，当前工具 Restore 没有收敛到宿主 RID。
- [BuildApplication.Runtime.cs](../../../build/cli/Inno.Build.Cli/BuildApplication.Runtime.cs#L185)：完整输出目录全部哈希与发布。

当前 WindowsX64 工具 publication 总计 **1,163,319,881 bytes**，包括：

| RID Native 资产 | bytes |
| --- | ---: |
| win-x64 | 119,873,536 |
| win-arm64 | 116,404,224 |
| linux-x64 | 247,796,944 |
| linux-arm64 | 234,909,152 |
| osx-x64 | 221,193,872 |
| osx-arm64 | 175,080,200 |

约 995 MB 是当前 WindowsX64 进程不使用的其他 RID Native 资产。这里是工具宿主部署闭包问题，不意味着引擎重新注册了 Linux 产品。

理想修复：Restore 与 publication 显式冻结工具宿主 RID，按实际 Restore 结果选择宿主 Native 资产；游戏目标和工具宿主保持分离。Task、CLI、binding extension 共用这一机制。不能硬编码删除 `runtimes` 文件夹或猜测 DLL 名，也不能把 Browser 的 RID 传给 Windows 工具。

证据：`current-tool-runtime-assets.json`；本轮没有改包引用或裁剪产物。

### F04：普通 BGFX 资源/readback 的 uint 帧号比较仍不处理 wraparound

优先级：**P3（长时间设备生命周期边界）**。静态确认，未通过数十亿次实际 frame 复现。

- [BgfxDevice.Retirement.cs](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Retirement.cs#L121)：到期帧使用 uint 加法和直接大小比较。
- [BgfxDevice.Readback.cs](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Readback.cs#L96)、[取消路径](../../../backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/BgfxDevice.Readback.cs#L136)：直接比较 Native uint readyFrame。

跨越 uint 最大值时，普通大小比较可能提前认为资源/readback 已到期。60 FPS 下约 828.5 天连续设备运行才达到该边界，因此不是本机短时验收的故障解释。窗口 surface 的新单调提交序号已经正确独立处理，不能把普通资源问题归给该实现。

理想修复：在 BGFX backend 统一把 Native 帧计数的有限未来距离映射到 managed 单调提交时间，或采用经过约束的 wrap-safe 比较；覆盖正常推进、边界及取消，不能用私人反射或测试初始帧号后门制造证据。

### F05：维护命令在工具失效时承担完整创作工具准备成本

优先级：**P2（冷路径工程体验）**。

本轮首次 `clean --dry-run` 遇到工具输入失效，日志记录了两次 SDK 构建，分别 **8:03.24 / 8:02.11**。前者提供当前准备实现，后者使用固定输入生成正式 publication。不是热缓存 compiler 次数，也不是 Native compiler 耗时。期间有另一个仓库的构建活动，因此不能把这些时间作为空闲环境基准。

同一工具准备成功后的实际 cache clean 为 **19.134 秒**，没有重新执行完整工具构建。两个阶段有固定输入正确性的理由，但让磁盘维护依赖完整 Editor/创作工具图仍有改进空间。

理想修复：在现有两个可执行入口下提供轻量维护应用闭包，通过中立 owner/distribution 契约复用同一缓存清理机制；创作工具按需要加载/准备。不能为了快速清理借用已经失效的旧实现、保留 legacy fallback、创建第二份 backend/owner 清单或新增生产 Exe。

## 架构、耦合、功能与扩展判断

| 维度 | 当前判断 |
| --- | --- |
| 逻辑与生命周期 | Scene、Runtime、PlayMode/reload 与 generation 回归通过。未发现新的 Edit/Play 事务混用。F01/F04 属于独立边界风险。 |
| 解耦 | 中立服务/内容/Runtime 没有 BGFX、SDL、MiniAudio、RmlUi 的实现名或项目引用；平台/backend/integration/组合的依赖方向保留。反射搜索中的 serializer/discovery 和 IDE stub 不等同于测试后门。 |
| 跨平台 | 三个明确目标与工具宿主分开；Windows/Web 有上一轮实际运行证据。MacOSArm64 未实测，且 F01 必须处理，不能宣称跨平台完全无问题。 |
| 干净程度 | 无新生产项目或 Program，当前 Solution 门禁通过。本轮清掉过期二进制产物，不删除原始验收事实。当前文档新增问题台账，不再用无问题完成宣言覆盖新发现。 |
| 功能 | 当前公开契约、运行和 reload 路径有回归证据，但本轮不是全功能、设备、视觉和声学覆盖。 |
| 可扩展性 | 新 backend 归属 backends；必要接入归属目标 integrations；发行选择 factory、compiler、Native plan。扩展有固定位置，但仍需要真实 SDK、ABI、capability、生命周期和实机验证。 |
| 性能 | 热 Native/Task 重用有效；整轮热 Build 仍约 138 秒。F02/F03/F05 显示运行与工具成本不能称为已经优秀或零成本。 |

没有证据支持整体重构；应先解决上述局部问题，随后以新证据评估。新增 GPU 机制可能合理扩展中立能力契约，这与让领域代码识别 backend/platform 不同。

## 本轮非 GUI 验证

| 套件 | 通过 |
| --- | ---: |
| Architecture | 122/122 |
| Scene | 51/51 |
| Runtime | 61/61 |
| PlayMode / reload 准入 | 16/16 |
| Generation reload | 22/22 |
| Contributor 帧分配测量 | 8/8 |

清理后 Editor 在独立验收 Project 中隐藏窗口运行 120 帧，正常保存并退出，exit=0；没有使用 Computer Use。该测试证明当前部署未被清理破坏，不替代 GUI 验收。

上一轮 Windows/Web 发布、存储、DSP、callback 和暂停恢复证据继续保留；本轮没有把它们冒充新一轮全发布。没有修改生产源码，因此没有为审查额外执行完整 Native 重建。

## 磁盘调查与清理

扫描当前整个 InnoEngine 仓库：**132,522,110,910 bytes 逻辑大小**，**113,573,141,220 bytes 去重分配量**，778,787 个物理文件身份，扫描错误 0。初次 DirEntry.stat 的 Windows inode 结果无效，已经单独保留并排除；正式扫描使用 os.stat 与 GetCompressedFileSize，不重复计算文件身份。

主要占用包括：

| 区域 | 调查时逻辑大小 |
| --- | ---: |
| BGFX build | 30.17 GB（分配约 13.72 GB） |
| Build CLI 工具与工作区 | 19.76 GB |
| managed workspaces | 12.21 GB |
| Native generations | 7.59 GB |
| 旧入口/Wiki 验收 | 4.92 GB |
| Task 工具 | 4.58 GB |
| 其他 backend、当前编译输出、SDK、extern、Git | 分别记录，不全部视为可删除 AI 缓存 |

在正式扫描时 C 盘约 67.03 GB 空闲。**即使把整个仓库的约 113.57 GB 分配量全部删掉，也只有约 180.60 GB 空闲；源码、Git、extern、SDK 和当前产品当然不能这样删除。单靠 InnoEngine 无法实现 200 GB。** 目录/MFT 开销和卷并发变化不影响这个安全范围不足的结论；不能把 132.52 GB 逻辑大小当成释放量。

专属 Inno/BGCS/Playwright Temp 目录只读盘点共约 1.18 GB。观察到独立 BGCS 仓库仍有活动构建，未清理其输出或全局缓存。

### 已执行清理

| 操作 | 结果 | 实际卷增加 |
| --- | --- | ---: |
| 历史验收 SDK 输出与已废弃 editor 工具目录 | 删除 124 个明确提案；源 fixture、日志、JSON/TRX 和 captures 保留 | 7,699,582,976 bytes |
| 正式注册 owner 清理 | 按当前保护根、依赖与活动 lease 执行，exit=0 | 2,878,525,440 bytes |
| 完整旧 Native generations | 删除 536 个；fingerprint 与完整输出集合匹配、非 current/registered/referenced/active/blocked；owner 与读取协调权下执行 | 7,383,076,864 bytes |
| 各操作卷增量合计 | 期间工具准备又分配空间，且其他进程也有写入，不当作整个时间段的净变化 | **17,961,185,280 bytes** |

最后读取 C 盘空闲约 **82.13 GB**，200 GB 目标未达成。详细路径、证明、跳过项与结果分别为 `manual-clean-plan.json`、`manual-clean-execution.json`、`stale-generation-clean-plan.json`、`stale-generation-clean-execution.json`、`registered-clean-execution.json`、`final-disk-state.json`。

37 个之前被自动审批拒绝的目录全部保留，没有补写 owner、重试同一路径或换工具绕过。完整 generation 清理也排除这些路径及其父子目录；有未知进程 owner、未声明额外文件、缺失清单或 staging 身份的目录继续保留。锁文件保留，当前产品与依赖保护。

本轮清理是明确授权的一次性维护，不为历史数据添加生产 reader 或 owner adoption。后续生产缓存继续使用当前登记与退休机制。

## 后续优先顺序

1. 修复 Core.IO 大小写与物理目录边界，补真实卷格式验证。
2. 优化 RenderGraph 的累计验证和 frame scratch，保留故障隔离与单次编译。
3. 精确工具宿主 Native 资产闭包，测量 publication、输入读取与磁盘收益。
4. 收敛冷维护命令的准备图，复用同一 owner/清理协议。
5. 补普通 BGFX 资源/readback 的计数回绕语义。
6. MacOSArm64、GUI 与完整 GPU 性能在具备环境和授权后单独验证。

本轮只审查、记录和清理，没有将这些新问题描述为已修复。提示音 `/System/Library/Sounds/Glass.aiff` 在当前 Windows 环境不可用，未播放。
