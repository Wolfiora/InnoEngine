# 当前源码验收

[架构索引](README.md) · [当前问题](CURRENT_ISSUES.md) · [Wiki 首页](../README.md)

当前 Editor UI 的分层、Session、Selection、History、插件调用方及十一项评价见[完整计划验收](../../records/architecture/acceptance/EDITOR_UI_FINAL_ACCEPTANCE_2026_10_30.md)、[当前 UI 架构](EDITOR_UI_ARCHITECTURE.md)及[issue 闭合台账](../../records/architecture/EDITOR_UI_CLOSURE_PLAN.md)。生产实现 `dd5979c6` 的本地适用功能与验收已完成。最终交付 SHA、只含文档/证据的后续差异、精确提交 CI 与 PR Ready 状态以 [PR #26 验证附件](https://github.com/Wolfiora/InnoEngine/pull/26#issuecomment-6516741338)为准；本地结果不代替该附件，也不复用历史提交的绿色结果。

| 当前 UI 范围 | 本地结果 |
| --- | --- |
| 源码、工具、入口和产品准备 | 11/11 阶段通过 |
| Contracts / Workflows / Visual / Lifecycle | 分别 822 / 704 / 524 / 15 次通过；Contracts 另有 2 次 Unix 专用测试在 Windows 未执行；各 profile 有重叠，不累加为独立测试数量 |
| Core Serialization / Scene / Core IO | 97/97、67/67、41/41 通过 |
| 工程、公开依赖、XML、Wiki | 241 工程求值；34,582 API inventory；197/197 实际编译示例通过 |
| Windows Editor | Debug/Release × D3D11/D3D12/Vulkan 六组通过，实际界面工作流及每类 Panel 100 次开关 |
| 原生输入与尺度 | 真实 Context、公开输入、56 个 Zoom/DPI/尺寸组合；模拟 metrics 与物理设备分开 |
| 生命周期 | 文档 100 次、十次连续 reload、严格代际回收和失败/恢复边界通过 |
| 受控性能 | 150 条成对记录；图尾部额外十进程复验；原始回退保留，无未解决的可重复门槛回退 |

原始 producer 已恢复为可运行且输入可核对的对照；此前基线建立失败不再是当前阻塞。图尾部初始十帧采样出现回退，增加既有采样轮数后未重现持续 P95 回退；两批结果均在报告中，不把初次失败改写为通过。完整检查最差 P95 24.5695 ms，接近 25 ms 预算，不能声称性能余量无限。

真实物理显示器、OS IME、人工 Mac 和主观可用性未执行；真实 Mac 自动结果由精确提交 CI 附件提供。32 项完整解决的 issue 在分支验证后仍保持 Open，等待 PR 合并；更广文档 issue #18 仅部分相关。整个引擎其他开放问题见[当前问题](CURRENT_ISSUES.md)，本轮 UI 完成不表示它们也已解决。

当前依赖通过标准 `.csproj` / `PackageReference` 和 SDK Restore 解析，源码不保存独立依赖锁清单；本轮范围与证据见[标准 NuGet 还原验收](../../records/architecture/acceptance/STANDARD_NUGET_RESTORE_ACCEPTANCE_2026_10_26.md)。基线保持 .NET SDK 10.0.402、C# 14、BGCS 4.1.2。

新项目的默认 `editor.ini` 已完成本地专项验收，见[默认布局报告](../../records/architecture/acceptance/EDITOR_DEFAULT_LAYOUT_ACCEPTANCE_2026_10_29.md)。标准 Host 提供布局，Core 复用原子文档写入；已有文件不被默认值重置。该结果不替代完整 UI 原计划或最终提交的 CI。

此前 BGCS 消费与三项优先修复的独立证据见[BGCS 4.1.2 消费验收](../../records/architecture/acceptance/BGCS412_CONSUMER_ACCEPTANCE_2026_10_25.md)及[专项报告](../../records/architecture/acceptance/PRIORITY_FIX_ACCEPTANCE_2026_10_25.md)。历史通过数量不能当作当前源码已重跑。

以下表格保留此前 BGCS 消费验收的独立范围和数量；不与本轮标准还原检查重复累计，也不代表本轮重跑完整产品矩阵。

## 此前 BGCS 专项证据

| 范围 | 当前结果 |
| --- | --- |
| BGCS 包身份 | 公共 NuGet 恢复；该次检查的 BGCS 包族均为 4.1.2 |
| Binding / native / Task 构建契约 | 65 项通过；含真实 MSBuild Restore 求值回归、SDK 生成、身份、取消与完整产物检查 |
| 独立 Restore 回归 | 9 项通过；owned Restore、静态图及 generator restore scope |
| 架构 / XML / Wiki | 238/238 工程通过；33,567 API symbols；196 示例通过 |
| Build 入口与独立 binding owner | 零警告、零错误；七个 HostSource owner 正式生成及检查通过 |
| Browser native | 五组件官方生成、原生编译与原子发布通过 |
| Browser 产品 | Mono 解释执行和 AOT 均正式发布，各完成 3,000 帧、输入、存储读取和有序退出 |
| Windows Player | CoreCLR / NativeAOT 正式发布，D3D11、D3D12、Vulkan 各完成 300 帧并退出，共六次 |
| BGCS 完整性 | 148 个包文件未改变；仓库 clean；Agent 文件 0 |

当前 Browser 平台继续使用官方 LibraryImport，桌面 FunctionTable 保持。BGCS 4.1.2 会在发布前拒绝 Browser FunctionTable；独立公开 SDK 探针确认拒绝后 last-good 文件不变，并确认 eligible frontend / emission 缓存冷 miss、热 hit。两个 SDK issue 已由上游关闭，本地消费复验通过；没有 SDK / generated 补丁。

本轮构建专项合计 74 项通过，0 失败 / 跳过，不重复计入此前 Coroutine / Runtime 的 142 项。本次发现的旧 package content 与新 SDK DLL 混装由 owned Restore / compilation 求值隔离修复；初始失败、哈希证明及最终复验见报告。

## 此前 BGCS 专项的矩阵与问题

- 该次 BGCS 专项没有本地 macOS / Metal 设备验收；后续 Editor 的 Mac 自动 CI 结果见上方最新复审，人工设备检查仍未执行。
- Windows Player 六条有界图形路径已通过；Editor Debug/Release、完整视觉、焦点、DPI、音频及长期运行矩阵未在本轮重跑。
- 示例产品已观察存储读取，尚未触发新的最高分写入，不能宣称分数跨启动存储已验收。
- 非致命 console 资源诊断仍待归因；自动维护预算、热构建成本及其他审计问题继续开放，均按报告链接进入 issue。
- 没有使用 computer use，也没有把 CLI/MSBuild 回归当作实际点击 Rider Run 的证明。

## 历史证据

[全仓核心审计](../../records/architecture/acceptance/FULL_ENGINE_AUDIT_2026_10_24.md)、[三项优先修复](../../records/architecture/acceptance/PRIORITY_FIX_ACCEPTANCE_2026_10_25.md)、[BGCS 4.1.1 消费验收](../../records/architecture/acceptance/BGCS411_CONSUMER_ACCEPTANCE_2026_10_24.md)及更早平台记录按其 revision / 检查范围保留。本页当前结果优先；历史通过数量不重复计入本轮。
