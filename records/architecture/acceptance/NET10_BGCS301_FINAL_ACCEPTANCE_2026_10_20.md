# .NET 10 / C# 14 / BGCS 3.0.1 阶段验收报告

日期：2026-10-20。环境：Windows x64；源码为四仓库当前未提交工作区。原有修改保留，未提交或推送。

2026-10-21 后续结果见[继续验收记录](NET10_BGCS301_CONTINUATION_2026_10_21.md)。本页保留 10-20 阶段事实；当前 Editor Debug、脚本和架构审计状态以后续记录及当前验收为准。

本报告记录已取得的证据，不声明完整产品矩阵全部通过。详细职责、文件树和操作记录见[实施记录](NET10_BGCS301_PROGRESS_2026_10_20.md)。原始日志位于引擎的 `artifacts/acceptance/bgcs301-2026-10-20/`。

## 当前结论

- 四仓库的当前开发基线统一为 SDK **10.0.402**、`latestPatch`、禁用预览 SDK；使用 **.NET 10 / C# 14**。两个生成器保留 `netstandard2.0` 工具目标。
- BGCS 包族使用公共 NuGet **3.0.1**；全部当前工作树锁文件的 **446 项**相关解析均为 3.0.1。未引用或修改 BindGen-CS 源码。
- 保持普通 using。未发现实际 global using、MSBuild Using item 或启用 implicit using；负例 fixture 和规则诊断不属于实际导入。
- Windows Release 的核心构建、测试、生成、脚本、Editor 和游戏发布闭环已有通过证据。完整 Editor Debug 产品、Browser 与 macOS/设备矩阵仍有明确缺口。

## 通过证据

| 范围 | 结果 | 主要证据文件 |
| --- | --- | --- |
| SourceRelease 完整项目图 | 0 编译告警、0 错误 | `managed-windows-release-build-05.log` |
| SourceDebug 完整项目图 | 0 编译告警、0 错误 | `managed-windows-debug-build-01.log` |
| 后续 CLI 源码输入修正 | 两配置下 Build entry 与 Build.Tests 四次相关构建全部成功；两配置的入口/源码预检各 11 项通过 | `*-source-input-*-build-01.log`、`project-source-input-*-01.trx` |
| 完整普通及隔离运行 | 第二轮 2,094 通过、3 失败、18 跳过；修正后原三项分别复验成功，合并为 2,097 通过、18 跳过 | `full-release-second-totals.json`、`release-tests-reconciled-01.json` |
| 隔离执行协议 | 59 个程序集隔离发现通过，28 个隔离用例全部通过 | `full-release-tests-02/execution-summary.json` |
| 生成和中立构建专项 | 正式 Task、取消、损坏、并发、静态选择、冻结输入及真实 CMake 共 20 项通过 | `binding-final-08.trx` |
| 七组件输出 | Windows 七项 HostSource 完整比对、七项目标生成后 CheckBindings 均通过 | `check-bindings-reconciled-01.json` |
| Native exports | 七个部署 DLL 实际加载，4,002 个必需导出存在；负例拒绝且模块释放 | `editor-native-exports-final.json` |
| C# 14 与 IDE | 5 项语言和 IDE 一致性专项通过 | `csharp14-final-01.trx` |
| Host 运行闭包 | 12 项实际 SDK 构建、发现及私有编译边界用例通过 | `host-runtime-closure-01.trx` |
| Canvas 测试 | 5 通过、0 失败、1 项原生 Metal 用例明确跳过 | `canvas-release-final-01.trx` |
| 项目脚本与 IDE | Canvas、Rendering2D、示例项目正式脚本和 IDE 生成/编译通过；D 盘 CLI 实际 Canvas 流程通过 | `*-scripts-01.log`、`*-ide-build-02.log`、`external-cli-source-after.log` |
| Windows Editor Release | 正式完整产品构建通过；D3D11、D3D12、Vulkan 的当前项目 smoke 正常退出 | `windows-editor-release-03.log`、`editor-shutdown-fixed-results.json` |
| Windows Support Pack | 冻结源码编译、完整发布及校验通过 | `windows-support-pack-final-02.log` |
| CoreCLR 游戏 | 正式发布成功；三种图形 API 各 60 帧、2 Views / 25 Draws、退出 0 | `flappybird-coreclr-publish-final-01.log`、`flappybird-coreclr-smoke-final-02.json` |
| NativeAOT 游戏 | 正式发布成功；三种图形 API 各 60 帧、2 Views / 25 Draws、退出 0 | `flappybird-nativeaot-publish-final-01.log`、`flappybird-nativeaot-smoke-final-01.json` |
| 当前工程与脚本静态检查 | 269 个工程 XML 有效；3 个独立 PowerShell 脚本语法有效；四仓库手写 diff 检查通过 | `project-xml-final-02.json`、`powershell-syntax-final-02.json`、`four-repository-static-audit-03.json` |
| 最终架构及 Wiki 门禁 | 233/233 项目检查通过；191 个示例编译通过、0 错误、0 未执行；其他平台产品 API 编译按所选 Windows 目标明确未执行 | `architecture-wiki-final-06.log`、`project-graph-final-09.json`、`wiki-examples-final-06.json` |

主套件汇总包含失败项复验，不能表述为一次整套零失败运行。新增源码预检用例在后续两配置的 11 项回归中单独计证，不把重复执行的十项再次加进主套件。18 项跳过分别要求 Unix 文件链接语义或 macOS Metal 工具链；Canvas 的额外 Metal 跳过另计。

## 本轮实际修正

1. 接入 SDK 的原生源码清单，在绑定层转换为中立冻结编译描述；保留平台 SDK 输入、输出 owner、lease、fresh 校验和原子发布。
2. 更新 Raw ABI 调用方、原生回调、UTF-8 和资源所有权；不手改 SDK 生成输出和 extern。
3. 修复 Host 私有运行依赖和 `.deps.json` 的实际部署闭包，不增加可发现类型名单或扩大编译可见性。
4. 修复 Vulkan 退出队列排空对已 reset view 的 touch。修正前真实窗口回归使 testhost 崩溃；修正后三 API 回归和七次产品 smoke 通过，保留原帧数及退休屏障。
5. 将测试源码输入归属收口到单一测试支持程序集；修正独立 SDK 输出下猜测 checkout 的 fixture。恢复完整项目图部署后关闭三项入口失败。
6. 项目 CLI 支持已有构建命令使用的 `--engine-root`；Canvas 工程传递已知源码路径。源码预检在工具启动和 Project 工作状态创建前失败。
7. `verify-native` 的仓库源码比较明确选择 HostSource，目标缓存由正式 Native 产品准备。UI 的过期头文件清单由正式 SDK 重新生成；只有清单输入哈希变化，managed 和 C++ 生成源码内容均未变化。
8. Canvas Metal 测试在不适用系统明确跳过；插件导出示例补齐工具目标。未添加兼容 API、旧格式读取、迁移工具或双路径实现。

公开领域 API、Stable ID 和序列化形状保持原有契约。两个直接依赖 Native 类型的 Widget 技术接口同步采用实际指针：`GetGlyphVisualBounds(ImFont*, ...)`、`AddGlyphCentered(ImDrawList*, ImFont*, ...)`；调用方与 Wiki 同步，不保留已移除的 Ptr 包装。

## 暂缓与未执行

| 范围 | 状态与原因 | 继续条件 |
| --- | --- | --- |
| Editor Debug 完整产品 | 原生准备时接近 C 盘 50 GiB 保留线，已取消且确认进程退出；不计为通过。SourceDebug 和相关 Debug 编译通过不替代该产品。 | 提供足够容量并保留 50 GiB，重新执行正式完整构建与运行。 |
| Browser Mono Wasm / Wasm AOT | BGCS 3.0.1 实际报 `BGCS-CSHARP-ABI-PLAN`，缺少 Browser 聚合按值调用证据/桥接能力。 | SDK 正式补齐后按[问题记录](BGCS301_BROWSER_AGGREGATE_ABI.md)重新生成、链接、发布及运行；不关闭 ABI 校验。 |
| macOS ARM64 产品 | 当前机器无法执行完整 Metal、Editor、Pack 和游戏矩阵；相关托管 integration 编译不等于设备通过。 | 在相应设备或 runner 上验证当前完整代码；旧 Metal surface 退休问题仍按台账复核。 |
| 视觉、焦点、DPI、输入、音频、长期运行 | 隐藏窗口短运行只证明启动、场景/绘制、帧推进与退出；未完成全部人工和长期验收。 | 记录实际设备和操作结果；Rendering2D 验证脚本语法检查不等于其完整 GUI 流程已执行。 |
| 远端 CI | 已更新配置并执行本地相关构建、语法和契约校验；未运行当前未提交状态的远端流水线。 | 在当前变更对应的 CI revision 上执行。 |

## 性能结论边界

同一 .NET 10 SDK 下的源码前后采样及分配结果见[非 BGCS 报告](NET10_NON_BGCS_ACCEPTANCE_2026_10_18.md)。命令录制、十六进制转换和脚本缓存收益按具体实现分别归因，不归为 C# 14 语法本身的收益。本轮产品 smoke 未测得可用于比较的帧时间、GC 或长期资源曲线，不声称全产品性能提升；完整 Editor 热构建成本仍按问题台账保留。

## 安全收尾记录

构建与临时目录使用本轮登记的 D 盘 owner。C 盘空间接近保留线时取消，随后只通过 SDK Clean 和正式 cache cleaner 回收明确 owner 的输出；全局 SDK、NuGet 和 BindGen-CS 工作区保留。逻辑缓存大小不作为实际释放空间，前后空间均另存证据。

Editor Debug 第二次尝试期间，观测到 C 盘余量由约 54 GiB 突降至约 45.93 GiB，未能守住 50 GiB 保留线；已立即取消且确认编译进程退出，不能隐去这次资源约束失守。当时系统分页文件分配约 21 GiB，但缺少同口径起始数据，不能将全部下降归因于分页文件。随后空间回升，最后观测约 58.5 GiB；其中本轮后续 SDK Clean 释放约 1.1 GiB，其他空间变化不计入我的清理收益。没有修改分页文件或终止用户的其他进程。

空的 `tests/Infrastructure` 目录删除，以及六个经核对的旧工程 `bin/.../net9.0` 输出删除，均被自动审批以 `blocked by policy` 拒绝，未提供进一步理由；这些路径保留，没有换工具绕过拒绝。此前另行记录的 37 个拒绝目录也保留。当前 Windows 无法访问 `/System/Library/Sounds/Glass.aiff`，未播放提示音。

最后完整架构/Wiki 门禁已通过，详情见上表。未通过、暂缓和未执行项不能计入完成，完整产品矩阵仍未全部完成。
