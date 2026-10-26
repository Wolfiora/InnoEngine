# 当前源码验收

[架构索引](README.md) · [当前问题](CURRENT_ISSUES.md) · [Wiki 首页](../README.md)

当前 Editor UI 的分层、Session、Selection、History、插件调用方及逐项质量评价见[Editor UI 验收](../../records/architecture/acceptance/EDITOR_UI_ACCEPTANCE_2026_10_27.md)和[当前 UI 架构](EDITOR_UI_ARCHITECTURE.md)。该报告分别列出自动验证、产品运行与未执行的人工/设备检查，不将此前数量累加为本次结果。

当前依赖通过标准 `.csproj` / `PackageReference` 和 SDK Restore 解析，源码不保存独立依赖锁清单；本轮范围与证据见[标准 NuGet 还原验收](../../records/architecture/acceptance/STANDARD_NUGET_RESTORE_ACCEPTANCE_2026_10_26.md)。基线保持 .NET SDK 10.0.402、C# 14、BGCS 4.1.2。

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

## 尚未完成的矩阵与问题

- macOS / Metal 无设备，未执行。
- Windows Player 六条有界图形路径已通过；Editor Debug/Release、完整视觉、焦点、DPI、音频及长期运行矩阵未在本轮重跑。
- 示例产品已观察存储读取，尚未触发新的最高分写入，不能宣称分数跨启动存储已验收。
- 非致命 console 资源诊断仍待归因；自动维护预算、热构建成本及其他审计问题继续开放，均按报告链接进入 issue。
- 没有使用 computer use，也没有把 CLI/MSBuild 回归当作实际点击 Rider Run 的证明。

## 历史证据

[全仓核心审计](../../records/architecture/acceptance/FULL_ENGINE_AUDIT_2026_10_24.md)、[三项优先修复](../../records/architecture/acceptance/PRIORITY_FIX_ACCEPTANCE_2026_10_25.md)、[BGCS 4.1.1 消费验收](../../records/architecture/acceptance/BGCS411_CONSUMER_ACCEPTANCE_2026_10_24.md)及更早平台记录按其 revision / 检查范围保留。本页当前结果优先；历史通过数量不重复计入本轮。
