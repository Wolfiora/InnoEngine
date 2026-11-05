# .NET 10 / BGCS 3.0.1 后续验收

日期：2026-10-21。范围为四仓库本轮提交的源码。此前通过结果和资源约束事件保留于[10-20 阶段报告](NET10_BGCS301_FINAL_ACCEPTANCE_2026_10_20.md)，本页记录后续新增证据。

[架构与 Core 复用审计](ARCHITECTURE_BGCS_CORE_REUSE_2026_10_21.md) · [Mac 入口](MACOS_ARM64_BGCS301_ACCEPTANCE.md) · [Browser SDK 阻塞](BGCS301_BROWSER_AGGREGATE_ABI.md)

## 当前结论

- 完整 Windows Editor Debug 正式构建已通过，SDK 汇总 0 警告、0 错误，耗时 12 分 52.90 秒。原生 CMake 提示保留；最低观测 C 盘余量 57.38 GiB。
- 按用户指定，后续产品运行统一使用 Samples/FlappyBird。Editor Debug 在 D3D11、D3D12、Vulkan 上各完成 3,000 帧并正常退出；已发布 CoreCLR、NativeAOT 游戏的六组同类运行全部通过，每组记录 2 Views、25 Draws。
- 实际窗口检查覆盖 CoreCLR 游戏画面、PLAY、Enter 重开、昼夜键盘/按钮、最大化及还原；Editor 完成 Play、玩法输入、昼夜切换及返回 Edit Mode。两进程正常退出。Console 首次点击未切换、再次成功，作为待复核现象登记，不以短运行覆盖完整设备矩阵。
- Canvas 测试源码路径改为复用已有 `Inno.Testing.Source` 声明，不从 DLL 目录推测 checkout。D 盘独立输出实际构建与运行 5 通过、0 失败、1 项 Metal 明确跳过；实际测试程序集经公开 `TestRepository.GetRoot` 返回当前 checkout。
- Rendering2D 的两个验收脚本均显式调用正式 `scripts --ide`，再编译生成工程。Windows 脚本支持明确配置及 SDK/Task 输出 owner。首轮脚本因未生成 IDE 工程而失败，原始结果保留；修正后 D3D11、D3D12 全流程通过。用户随后指定产品测试使用 Samples 的 FlappyBird，切换时已开始的 Vulkan 构建已取消，后续运行统一使用该示例。
- BGCS 工作区无修改，四仓库无 BGCS 源码工程引用；正式包内容比对和标准 NuGet 内容哈希核对通过。详细边界及 R01/R02 见架构审计。
- 最后一次架构/Wiki 门禁通过：233/233 工程有效归属，191 个示例全部编译通过。提交前额外核对 270 个源码工程/props/targets 的 XML 与排版、Solution 的 233 个项目及 141 个非空分组，没有重复项目、缺失路径或遗漏源码工程。
- Browser 仍受正式包聚合 ABI 能力阻塞。用户当前无法使用 Mac，macOS 产品保持未执行；不计入通过。

## 新增文件职责

| 文件 | 当前职责 |
| --- | --- |
| Canvas `Tests/InnoEngine.Canvas.Tests/InnoEngine.Canvas.Tests.csproj` | 导入已有引擎测试源码声明，保持 SDK 输出位置独立。 |
| Canvas `Tests/InnoEngine.Canvas.Tests/CanvasAuthoringTests.cs` | Metal Shader 工具准备复用公开测试支持库，不增加第二个路径查找器。 |
| Rendering2D `Tools/Validate-Rendering2D.ps1` | 同一配置/输出参数用于 Build 和 TargetPath；正式生成 IDE 后编译。 |
| Rendering2D `Tools/Validate-Rendering2D.sh` | Mac 验收同样显式执行正式 IDE 生成；本机只做语法检查。 |
| Rendering2D `.github/workflows/rendering2d-gpu.yml` | 恢复 push/PR 自动验收，手动验收默认开启；远端 runner 本轮未运行。 |
| 引擎 `.gitignore` | 排除 SDK 的 `.bgcs` 本地阶段缓存，正式生成产物仍按原协议纳入。 |
| `ARCHITECTURE_BGCS_CORE_REUSE_2026_10_21.md` | 回答 BGCS 来源、职责边界和基础能力复用，列出事实与待处理点。 |
| `MACOS_ARM64_BGCS301_ACCEPTANCE.md` | 环境、绑定预检、源码/隔离测试、Editor、插件、Pack、游戏及设备验收顺序。 |

没有新增生产领域 API、兼容路径或生成器源码依赖；没有改动插件 Assets，本轮无需重新导出安装包。

## 新增证据

原始文件位于 `artifacts/acceptance/bgcs301-2026-10-20/`。

| 范围 | 证据与结果 |
| --- | --- |
| Editor Debug 正式构建 | `windows-editor-debug-final-03-result.json`、对应 stdout/stderr；通过。 |
| 切换测试项目后的重建 | `editor-debug-after-steering-2026-10-21-result.json`：9 分 39.93 秒，0 警告、0 错误；确认取消的前序构建未成为运行依据。 |
| FlappyBird Editor Debug | `editor-debug-product-smokes-2026-10-21.json`：三 API 各 3,000 帧通过；`*-02-boot.log` 仅保留本次启动段。首轮脚本误将追加日志的历史诊断计入本次，进程退出 0；原始误报保留。 |
| FlappyBird 发布游戏 | `player-product-smokes-3000-2026-10-21.json`：CoreCLR、NativeAOT 各三 API，六组全部通过。使用正式发布物，不重新拼装游戏。 |
| 实际窗口交互 | `flappybird-*-interactive-2026-10-21-result.json`：CoreCLR、Editor 均退出 0；UI 观察与范围见下节，未保存声学结论。 |
| 磁盘守护 | 每 0.5 秒采样，低于 56 GiB 终止所属树，保留 50 GiB；本次上述构建未触发。`windows-editor-debug-final-03-space.json`。 |
| Canvas 独立输出 | `canvas-repository-input-2026-10-21.trx`：5 通过、1 跳过；`canvas-source-root-public-probe-2026-10-21.json`：源码输入正确。 |
| Rendering2D 首轮失败 | `rendering2d-d3d11-validation-2026-10-21-result.json`：产品 3,000 帧退出后，IDE 工程不存在导致脚本退出 1。不是产品启动失败。 |
| Rendering2D 修正复验 | D3D11 / D3D12 的 `rendering2d-*-validation-2026-10-21-02-result.json` 均通过，含各 3,000 帧和正式 IDE 生成/编译；Vulkan 后续构建按用户测试项目要求取消，见 `rendering2d-validation-user-steering-2026-10-21.json`，不记为失败或通过。 |
| BGCS 安装内容 | `bgcs-installed-package-integrity-final-2026-10-21.json`：102 个实际安装文件与包内容一致；无意外缺失或修改。 |
| 正式 SDK 能力 | `bgcs-published-runtime-capability-final-2026-10-21.json`：公开二进制查询支持 Windows x64 aggregate profile；Browser 和 ARM64 CoreCLR 未实现，不等于实机生成/运行结果。 |
| 工程与脚本静态检查 | `continuation-project-xml-01.json`：276 个 XML 有效；Mac 指令 8 个 PowerShell 代码块无语法错误。两平台验收脚本语法检查通过。 |
| 手写 diff | `continuation-four-repository-diff-02.json`：四仓库均通过。SDK 生成的 MiniAudio 文档注释有尾随空格，不手工改生成输出；不把全生成文件格式检查记为通过。 |
| 最终架构/Wiki | `architecture-wiki-final-07-result.json`、`project-graph-final-07.json`、`wiki-examples-final-07.json`：通过；233 工程、191 示例，macOS/Browser 产品 API 明确未执行。 |
| 工程整洁度 | `project-layout-review-2026-10-21.json`：270 个源码 XML、233 个 Solution 项目、141 个非空分组；无待处理问题。XML 注释内的英文续行对齐不作为元素缩进错误。 |
| 提交前检查 | `final-commit-readiness-2026-10-21.json`：SDK、446 个包族锁定项、111 个记录/当前文档链接及示例统计通过。四仓库暂存内容的手写 diff 检查通过；三个新手写 Native 调用文件仅移除末尾多余空行，去除末尾换行后的内容逐字符一致。 |

## 实际窗口检查

使用 computer-use 的 Windows `@oai/sky`，只操作本次启动的 FlappyBird 和 Inno Editor 窗口；没有操作系统设置、用户其他应用或第三方服务。

| 产品 | 实际观察 |
| --- | --- |
| FlappyBird CoreCLR / D3D11 | 日间待机画面有鸟、水管、Canvas 文本和按钮。PLAY 后进入玩法并显示死亡页；Enter 重开观察到运行画面。N 切换夜景，Canvas 昼夜按钮恢复日景。最大化、还原后保持画面与 UI，Alt+F4 后退出 0。没有取得非零分存储往返的交互证据。 |
| FlappyBird Editor Debug / D3D11 | 场景与 Game 面板显示当前场景；Play 激活，Game 键盘输入进入玩法，N 改变运行副本；停止后恢复日间待机与原预览布局。Console 最终显示 `No logs or diagnostics yet.`；关闭后日志记录工作区保存和 `Dispose end.`，退出 0，Samples 没有新增创作源差异。 |
| 需要复核 | Console 标签从另一位置首次点击没有切换，保持悬停后再次点击成功。未将其判定为已定位缺陷；需要区分焦点、自动输入时序和同帧移动/按下路径。未完成完整 DPI、跨窗口焦点、拖动、音频听感、非零分持久化和长期资源曲线。 |

界面中短暂显示的 FPS 是当前负载下的观察值，不作为性能比较基线；本轮没有宣称完整产品帧性能提升。

## 提交与整洁度

四仓库保留当前分支：引擎 `dotnet10`，其余三个仓库 `main`。使用各仓库已有的 Aaron Liao 身份，标题遵守 `type(scope): ...`，不添加共同作者；不推送远端。实际提交身份、SHA、分支和最终状态记录于本地 `commit-audit-2026-10-21.json`，并在交付消息列出。

开始本轮前已有的源码修正及插件包已经纳入前序状态记录，本次保留并随相应调用方共同提交；没有重置或覆盖这些修改。构建输出、SDK 阶段缓存、原始测试日志和本机 IDE 工程不进入提交；验收结论与原始日志位置写入本页。

## 验收边界与未关闭项

- 原主套件仍按“完整运行加原失败项复验”合并为 2,097 通过、18 跳过；本次 Canvas 复跑单独计证，不重复累加。
- R01：一个插件动画帧事件未统一 Core；改变公开事件需要明确调整原计划保持签名的边界，未悄悄加入双协议包装。
- R02：一个 UI 插件跨帧状态仍直接使用 live owner，应按 Identity 标准收口；尚未复现泄漏或崩溃，不能说已经验证无问题。
- R03：`verify-native` 的产品发现与实际宿主 runner 范围不一致，不能宣称其已支持 Browser 全套验收；详见架构审计。
- 完整热构建成本、部分 XML 语义质量、上述未完成的交互/设备场景和长期资源曲线仍按问题台账保留。
- macOS 无设备，Browser 缺 SDK 能力；远端 CI 未在当前未提交状态运行。本页不声明完整跨平台矩阵完成。

之前被自动审批拒绝的目录仍保留，本次未换方式重试清理，也未修改全局 SDK、NuGet 或系统分页设置。当前 Windows 无法访问 `/System/Library/Sounds/Glass.aiff`，提示音未播放。
