# Issue #23：NuGet 锁文件治理验收

日期：2026-10-25；主工作区：Windows x64；分支：`dotnet10`；实施前 revision：`aa668f6aab069e28c6a9dad782906ee2cda7a21f`。

## 当前阶段

本环境适用关卡全部通过：19 个正式／跨 RID／宿主路由请求 locked restore、59 项相关测试、238 工程架构门禁、196 个 Wiki 示例、独立 Editor Debug / Release、最终清单集合连续 10 次热构建及四种 FlappyBird 部署。五次／五次同目录性能对照未触发 10% 回退门槛。最终有用途清单 725 份，untracked 清单 0；可按本报告关闭 Issue #23 的当前 Windows／Browser 范围。

## 文件数量与处置

| 项目 | 数量 / 处理 |
| --- | --- |
| 原 tracked 清单 | 1,415 |
| 原 untracked 清单 | 438 |
| 原文件合计 | 1,853 |
| 原文件继续保留 | 228，均为 tracked |
| 原文件退出源码目录 | 1,625：1,187 tracked＋438 untracked |
| 官方 NuGet 新生成、保留的清单 | 497 |
| 当前有正式请求使用的清单 | 725 |
| 当前 untracked NuGet 清单 | 0 |

原 1,853 个文件逐项处置见 [ISSUE23_NUGET_LOCK_DISPOSITION.tsv](ISSUE23_NUGET_LOCK_DISPOSITION.tsv)，包含基线 SHA-256、原 Git 状态、保留／退休动作和该工程当前使用的清单。详细请求与每个文件的对应关系在验收目录 `lock-classification-final.json`，不作为生产运行注册表。

首次批量删除命令被自动审批拒绝，返回 `blocked by policy`。没有重试同一删除操作；改为核对完整 owner、源／目标绝对路径、reparse point、基线哈希和备份后，逐文件搬入本次验收隔离目录。没有递归删除，原始内容完整保留。SDK 工具包维度完善时退出的 67 份本轮中间清单同样归档，不留在源码目录。

## 实际生成规则

唯一规则位于 `build/msbuild/Inno.NuGetLockFiles.targets`。根 targets 通过官方 `AfterMicrosoftNETSdkTargets` 扩展点追加它，保留已有扩展列表，读取 SDK 已推导的实际能力。

| 范围 | 当前文件数量 | 命名 |
| --- | --- | --- |
| 中立 | 233 | `packages.lock.json` |
| Windows RID | 75 | `packages.rid-win-x64.lock.json` |
| macOS RID | 72 | `packages.rid-osx-arm64.lock.json` |
| Browser RID 与 SDK link 工具包 | 67 | `packages.rid-browser-wasm.link.lock.json` |
| Windows RID 与 NativeAOT 工具包 | 69 | `packages.rid-win-x64.aot.lock.json` |
| Windows RID 与 link 工具包 | 69 | `packages.rid-win-x64.link.lock.json` |
| macOS RID 与 NativeAOT 工具包 | 69 | `packages.rid-osx-arm64.aot.lock.json` |
| macOS RID 与 link 工具包 | 69 | `packages.rid-osx-arm64.link.lock.json` |
| Editor Windows 宿主路由 | 1 | `packages.host-win-x64.lock.json` |
| Editor macOS 宿主路由 | 1 | `packages.host-osx-arm64.lock.json` |

宿主上下文与实际 RID 同时存在时组合使用；该命名规则有真实 SDK 求值回归，不等于为未运行组合提前生成无用途的文件。

`InnoNativeTarget` 继续参与原生构建、Shader 参数和输出隔离，不再自动成为所有工程的 NuGet 清单维度。生成器正常通过既有工具引用边界保持中立；显式传入真实 RID 时保留它，不能只凭 `IsRoslynComponent` 忽略。

### 对原计划必要的完善

最初只有 host / RID 的规则不足：同一 `win-x64` 下，`PublishAot=true` 让 SDK 隐式增加 `Microsoft.DotNet.ILCompiler`、`Microsoft.NET.ILLink.Tasks`。直接共用清单会造成覆盖。初始还原图比较捕获了 69 个同路径不同依赖形状。

当前规则保留这些 SDK 属性，并区分 `.aot` / `.link`，使用 SDK 完成推导后的 public publish / analyzer 能力；不是根据引擎部署名称建立第二份平台表。最终 19 个图对应 725 个清单，声明依赖形状冲突为 0，随后官方 locked restore 全部通过。

规则不解析或选择包版本，不添加 PackageReference，不改 NuGet JSON 格式，也不手改内容哈希。实际依赖、TFM、RID、原生包资产和包验证仍由 NuGet 负责。

## 源码、测试和 CI

- 根 targets 删除旧 target×RID 拼接；Editor 入口只声明自己的宿主引用图上下文。
- 输入快照继续读取实际求值的 `NuGetLockFilePath`；包身份用例通过真实 SDK 还原到独立 ArtifactsPath，读取对应 assets 和 lock，不再猜默认 obj。
- 新回归通过公开 `ManagedBuildInputSnapshot` / `ManagedBuildExecution` 验证：当前选中的清单进入冻结输入，未选 RID 清单不被复制，其变化不使当前身份失效；当前输入变化被拒绝。
- 既有工程图验证器增加锁路径、RID、host 和隐式工具包范围检查，没有新增 public C# API。
- `tests/Invoke-NuGetLockChecks.ps1` 使用官方 SDK 图和 locked restore，比较文件集合、哈希和时间戳，检查所有源码清单受控且未被 ignore。
- 依赖 CI 调用同一脚本，上传 graph / restore / inventory；构建 CI 覆盖源码、Editor 与 NativeAOT 请求。
- CI 的 BGCS 版本检查读取统一声明，移除过期的 3.0.1 字符串断言。
- README、构建 Wiki、工具 Wiki 和 AGENTS 同步当前规则；`.gitignore` 没有用笼统模式隐藏源码清单。

### 实际捕获并修复的检查问题

- 负例错误地携带 `--force-evaluate`：NuGet 明确允许这个更新选项重新生成清单，不能与 locked 验收混用。改为纯 locked 调用，独立工程复现和 NU1004 对照保留。
- Browser RID 会触发 SDK 推导的 link 工具包范围；修正测试中仍假设普通 RID 文件名的断言。没有改变 SDK 属性去迎合断言。
- `git check-ignore` 在没有匹配项时返回 1，检查脚本必须在所有断言通过后明确返回成功，不能继承该状态。
- Windows PowerShell 的文本管道给路径追加 CRLF，Git 的逐行 stdin 路径协议把 CR 留在路径内，造成特定 ignore 规则漏检。改为有界参数批次，避免管道编码／换行问题及 Windows 命令行长度上限。真实 ignored 负例现已明确失败。

## 已取得的证据

证据根：`D:/InnoEngine-Verification/issue23-2026-10-25`。它只属于本次本机验收，不是源码内的固定构建路径。

| 检查 | 结果 | 证据 |
| --- | --- | --- |
| SDK / package 基线 | 10.0.402；BGCS 4.1.2 | `baseline.json` |
| 原文件完整清点 | 1,853 个 SHA-256 与原始副本 | `baseline-locks/`、`baseline.json` |
| 文件 owner 与正式请求 | 725 个当前清单 | `lock-classification-final.json`、本报告 TSV |
| SDK 图对照 | 19 个请求；同清单依赖形状冲突 0 | `graph-cases.json`、`restore-shape-comparison-final.json` |
| 正式 locked restore | 19/19 成功 | `locked-final-cases.json`、`locked-final-*.log` |
| Build 入口 | 零警告、零错误 | `cli-final.log`、`cli-final.binlog` |
| 相关回归 | 59 通过，0 失败 / 跳过 | `tests-final-quieter.log`、`tests-final-quieter/*.trx` |
| 架构 | 238/238 工程通过 | `architecture.log`、`project-graph.json` |
| 脚本正反例 | valid exit 0；untracked / ignored 均 exit 1 | `script-contract-results.json`、`script-accepted-*.log` |
| 独立冷检出 locked restore | 232 个工程通过 | `cold-locks-final.log` |
| 独立冷 Editor Debug | 无复用主工作区输出；首次构建零警告、零错误 | `cold-editor.log`、`cold-editor.binlog` |
| Wiki | 33,567 API symbols；196 示例；零错误／未求值示例 | `wiki.log`、`wiki-api.json`、`wiki-examples.json` |
| Windows FlappyBird | CoreCLR / NativeAOT 各 300 帧，D3D11，exit 0 | `game-*.log`、`run-*-result.json` |
| Browser FlappyBird | Mono / Wasm AOT 各 3,000 帧、输入、存储读取，exit 0 | `browser-*-final-result.json` |
| 最终候选 Editor | Debug / Release 均零警告、零错误 | `cold-final-Debug.log`、`cold-final-Release.log`、对应 binlog |
| 固定输入热构建 | 连续 10 次；725 清单不改写；28 指针稳定；Native / binding generation 均 0 | `final-ten-hot-builds.json`、`final-hot-lock-observations.json` |
| 对照性能 | 各 5 次；当前／基线中位数比例 0.9912 | `performance/comparison.json`、各轮 log / binlog |
| 清理后默认输出还原 | 232 工程通过；725 清单不改写 | `final-default-source-check.log` |
| 对照后副本还原 | 当前规则 locked restore 通过，副本 Git clean | `cold-post-control-check.log` |
| Markdown 链接 | 变更页面本地链接无缺失 | `final-markdown-links.json` |
| BGCS / 其他仓库完整性 | 130 包文件哈希不变；四个相邻仓库 revision / worktree 不变 | `final-integrity.json` |

首轮测试、检查脚本和命令失败日志保留，不覆盖为成功。最终成功结果与其替代关系按上述文件区分；不重复累计多轮测试数量。

### 重跑与浏览器观察方式

并发冷构建、发布和大规模归档期间，一项既有静态图测试触发三分钟超时。保留失败的 `tests-final-input-lock.log` / TRX；降低负载后按原超时两项定向用例通过，随后 `tests-final-quieter` 整轮 59 项通过。没有放宽超时、排除用例或更换测试平台。

Browser 首轮观察器对 `catalog.inno` 的 `ERR_ABORTED` 提前终止了测试，尽管运行时已经继续请求内容包。最终测试要求应用完成 3,000 帧并从公开 `runMain` 返回 0，非取消类网络失败仍明确失败；如出现取消，还要求对应服务端成功响应已完成。最终两种部署的取消、HTTP 失败及 console error 均为 0。

退出观察器仅在测试 HTTP 响应中，在原有 `await runMain()` 后追加返回值记录；不改发布文件、SDK、managed/native 生成物或生产入口。两种发布的 `main.js` 文件 SHA-256 均为 `7a890f660bebafb359f409dded2ddb1c3288cc8df19ad8922d39f313d9de6fad`，与观察器记录一致。测试使用自有 headless Edge 156.0.4314.42，WebGL 2 / ANGLE / NVIDIA RTX 3070 Ti Laptop GPU；没有操作桌面或用户现有浏览器。

### 构建稳定性与性能

原循环第 3 轮进行期间补齐 Mac 的 AOT / link 请求，清单固定为 725 份。最终状态的完整十轮取连续第 4–13 轮：前七轮与追加三轮分别核对全部清单哈希／时间戳，零遗漏观察；十轮 Native processes / binding generations 均为 0，28 个选择指针不重写。中位数 220.095 秒；早期与冷构建／归档并发的高耗时样本保留，不冒充空闲性能基线。

性能对照使用同一独立目录、候选 `132c3c488f5836f26f08ab0ab0c43bd91c6ee329`、SDK 10.0.402、Debug 和相同源码／输入。只在可丢弃副本中切换命名规则，官方 restore 生成对照清单；每组先预热一次，再采样五次。对照后恢复当前规则、归档实验新增清单，全部 725 个正式清单内容与对照前一致，Git clean。

| 采样 | 基线规则 | 当前规则 |
| --- | --- | --- |
| 五次 wall time（秒） | 333.618、161.150、152.605、153.000、157.809 | 152.319、152.558、156.422、159.154、158.731 |
| wall time 中位数（秒） | 157.809 | 156.422 |
| 采样到的父进程 CPU 中位数（秒） | 89.766 | 88.516 |
| 峰值工作集中位数（MiB） | 671.37 | 671.47 |
| Native processes / binding generations | 全部 0 / 0 | 全部 0 / 0 |

当前／基线 wall time 比例 0.9912，没有超过 10% 的回退；约 0.9% 差异在本次噪声范围内，不宣称提速或运行时／分配收益。CPU 只统计采样父进程，不能当成全部子进程总 CPU；工作集也不是 GC 分配量。既有暖 Native 输入读取约 4.5 GB 的成本未在此修复。

## 验收边界

macOS 本机没有设备。现有 Mac 数据来自 Windows 上的明确跨 RID 还原与宿主路由 fixture，不代表运行过 Mac Editor、Metal 或原生 ARM64 产品。其他已登记的性能、缓存维护预算、History、插件热路径和设备验收问题不因本次锁文件治理自动关闭。

Windows 本轮运行检查为 D3D11 两种部署各 300 帧；完整 D3D12 / Vulkan、视觉、焦点、DPI、音频和长期运行没有重做。Browser 读取了最高分键，没有触发新的最高分写入；不能将其描述为跨启动分数保存验收。原生工具链输出中的编码、临时工作区及 BGCS 安全事实诊断按原日志保留，本报告的托管零警告计数不代表原生链无诊断。GitHub Actions 的配置已更新，本轮未推送，因此没有新的远端 Actions 运行。

## 深层自查

检查沿实际链路进行：工程／平台声明 → SDK 属性推导 → 官方还原图 → 所属清单 → assets 与包身份 → Task 求值与输入冻结 → 普通／独立输出 → 产品构建。

平台事实仍由平台 owner 声明；共同规则只处理 SDK 数据与入口显式上下文。没有新增平台名单、第二套 NuGet resolver、缓存服务、事件、Identity 或序列化系统。已有 Core IO lease、取消、读取保护、输入完整性和原子 publication 不被绕过；旧清单不会通过 fallback 进入正常路径。

源码目录中的依赖清单是受版本控制的输入，构建协调锁仍属于原输出 owner；没有混用两者的清理或 ignore 策略。一次性清点和归档只属于验收，不进入生产运行路径。

## 构建产物与磁盘空间

收到 C 盘空间压力反馈后，将此前五个已结束验收 owner 的 229,218 个文件、44,408,990,791 字节逻辑内容逐文件归档到 `D:/InnoEngine-Verification/archived-acceptance-2026-10-25`。源／目标均验证绝对 owner 边界、无 reparse point、没有活动命令引用；不覆盖已有目标。另逐个非递归删除 20,952 个已证明为空的目录。历史证据仍保留，映射见 `acceptance-archive.jsonl`。

活动构建期间，官方清理首次因变化中的 Task owner 快照明确失败，另一次按协调锁协议延期，均未强制删除。构建结束后的正式清理成功：逻辑回收 35,077,440,347 字节，跳过 0；C 盘可用空间由 78,187,569,152 增加到 86,342,316,032 字节，实测增加约 7.6 GiB。共享／硬链接文件使逻辑回收量与实际卷空间不同，不能混报。

空间压力反馈时 C 盘约剩 19.4 GiB，归档、原生临时目录正常释放及正式维护后约有 80.4 GiB 可用。本次后续发布与临时输出放在 D 盘，只修改验收进程环境，不向工程源码写入盘符，也不更改全局 SDK／NuGet 缓存位置。证据见 `cache-clean-*.log`、`cache-clean-after-builds-space.json`、`empty-archive-directories.json`。

当前 Windows 无 `/System/Library/Sounds/Glass.aiff`，未播放提示音。
