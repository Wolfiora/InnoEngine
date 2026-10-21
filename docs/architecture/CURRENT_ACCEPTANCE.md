# 当前验收

[架构索引](README.md) · [Wiki 首页](../README.md) · [问题台账](CURRENT_ISSUES.md)

## 工具链与范围

当前源码使用 SDK 10.0.402、C# 14，运行目标为 `net10.0` / `net10.0-browser`；两个编译器生成器保留 `netstandard2.0` 工具产物。SDK 禁止预览并采用 `latestPatch`。源码和脚本必须逐文件声明普通 using，禁止全局及隐式导入。

2026-10-21 当前源码的 BGCS 包精确固定为 3.0.1。七组件生成、正式 Task、Windows 完整原生支持、Editor Debug/Release 及两种游戏部署已通过对应检查。主套件按完整运行及失败项复验合并记录；Browser ABI 和 macOS 设备仍限制完整产品矩阵。

本轮环境为 Windows x64。托管构建与真实原生编译、链接、部署和产品运行分别记录；局部通过不能代替最终统一代码状态的整体验收。

Rider 暴露的 Task 注册、产品引用和 CMake 路径问题另见 [MSBuild 专项验收](../../records/architecture/acceptance/MSBUILD_IDE_TASK_LOADING_2026_10_21.md)。该记录区分 SDK 构建、进程外 worker 回归、产品 smoke 与未执行的 Rider UI 验证。

## 本轮结果

| 范围 | 当前结果 |
| --- | --- |
| SDK/IDE 构建协议专项（2026-10-21） | 最终 Editor Debug 完整构建通过，SDK 汇总 0 警告、0 错误；92 项相关回归全部通过。包含预求值进程外 worker、私有加载、入口引用图、CMake TEMP 隔离和可取消绑定消费。Rider UI 及资源取消的跨 TEMP 完整重复构建未计作通过，见专项记录。 |
| 完整 SourceRelease | 当前 BGCS 3.0.1 调用方及测试源码统一构建通过，0 警告、0 错误；运行结果及后续入口修正另按下表记录。 |
| 完整 SourceDebug 及入口修正 | SourceDebug 完整项目图构建通过，0 警告、0 错误；随后 CLI 显式源码输入修正在 Debug/Release 的相关四次构建均通过，两配置的预检及开发入口回归各 11/11 通过。 |
| 仓库架构及 Wiki 门禁 | 233/233 工程有效 MSBuild 归属及整体架构校验通过；191 个当前 Wiki 示例编译通过，0 错误、0 未执行。包含测试输入程序集，macOS 与 Browser 产品 API 编译按所选 Windows 目标明确标为未执行。 |
| BGFX、MiniAudio、SDL、Text、UI | Debug/Release 共 10 次最终托管构建通过，全部 0 警告、0 错误。实际原生编译、链接和运行仍单独验收。 |
| 架构命令回归 | Release 31 项通过，0 失败、0 跳过；包含普通 using 策略与当前 binding 声明归属。后续完整仓库门禁已通过。 |
| 普通与进程隔离测试 | 第二轮完整 BGCS 3.0.1 运行为 2,094 通过、3 失败、18 跳过；59 个程序集隔离发现成功，28 个隔离用例全部通过。完整项目图重建后，原三项入口 fixture 失败均复验通过；按完整运行加失败项复验合并为 2,097 通过、18 跳过，原失败日志保留。18 项跳过分别要求 Unix 文件链接或 macOS Metal 编译器。 |
| Rendering 命令录制 | Debug/Release 均为 61 项回归通过，覆盖 31 种命令、缓冲扩容、输入复用、单独和组合失败的引用释放与异常聚合。三次同 SDK/Release 最终采样中，10,000 Draw 每帧分配由约 1.15 MB 降至约 3.14 KB；1,000 与 10,000 Draw 录制/回放耗时均低于改良前采样范围。完整产品帧时间尚未验收。 |
| ImGui/SDL 调用方 | 适配器 Release 构建通过，0 警告、0 错误；BGFX ImGui renderer 编译通过。完整 Build 测试工程及其生产 Editor 引用闭包已编译通过，最终整套测试继续复核。 |
| 原生测试工程 | SDL、MiniAudio、BGFX 的 Release 托管测试工程均编译通过，0 警告、0 错误。部署路径参数与 SDL flag 宽度 10 项通过；这不代表已执行原生初始化或产品运行。 |
| 冻结原生输入 | Windows Editor 八组件正式原生支持准备通过。当前部署的七个 DLL 实际加载后，4,002 个必需导出全部存在，逐库负例符号正确拒绝并释放模块；中立清单回归 8 项通过。 |
| BGCS 生成专项 | 正式 MSBuild Task、静态选择与传递消费已通过；生成、CheckOnly、取消、损坏、并发、配置、Browser 工具链与真实 CMake 回归统一重跑 20 项通过，0 失败、0 跳过。Windows 七组件的 HostSource 完整输出比对与目标生成后 CheckBindings 均通过，UI 输入清单过期项经正式 SDK 重新生成后复验通过；其他目标及产品仍单独验收。 |
| 当前 Project 与 IDE | 三个当前 Project 的正式脚本、安装态插件编译、IDE 投影及 SDK Debug 构建通过。插件包 Assets 与源逐文件一致，无需重新导出。 |
| Canvas 独立输出测试 | D 盘 CLI 正式生成脚本与 IDE 成功；实际测试构建后 5 通过、1 跳过、0 失败。Metal 原生编译用例在 Windows 明确跳过，其余适用用例执行。 |
| Windows Editor | 正式 Release 构建 0 警告、0 错误。D3D11 / D3D12 的启动、帧推进及退出通过；当前 Project 的 D3D11 smoke 通过。隐藏窗口不代替视觉、焦点、DPI 或声学检查。 |
| Windows Editor Debug 产品 | 2026-10-21 正式完整构建通过，SDK 汇总 0 警告、0 错误，12 分 52.90 秒；切换测试项目后的再次完整构建为 9 分 39.93 秒、0 警告/错误。当前示例项目在 D3D11 / D3D12 / Vulkan 上分别完成 3,000 帧并正常退出。原生 CMake 提示保留。 |
| Windows Player | 正式 Support Pack 冻结源码编译及完整发布校验通过，托管阶段 0 警告、0 错误。当前示例项目的 CoreCLR、NativeAOT 游戏发布均成功；两种部署各在 D3D11 / D3D12 / Vulkan 上完成 3,000 帧、2 Views / 25 Draws，并正常退出。 |
| 实际窗口交互 | 当前示例游戏的 CoreCLR 画面、PLAY、Enter 重开、昼夜输入、最大化/还原及退出已观察；Editor 完成 Play、Game 输入、昼夜切换及返回 Edit Mode，创作状态恢复，正常退出。Console 首次点击未切换、再次成功，需复核输入时序；完整 DPI/焦点、音频及长期运行未完成。 |
| Vulkan | 关闭排空移除对已重置 view 的 touch；修正前新增真实窗口回归导致 testhost 崩溃。修正后空 Project 两次、三个当前 Project 各一次 Vulkan smoke 均完成 60 帧并正常退出；Canvas D3D11 / D3D12 同样通过。完整回归继续复验，见[专项记录](../../records/architecture/acceptance/WINDOWS_VULKAN_EXIT_FAILURE_2026_10_20.md)。 |
| Browser | 正式 BGFX 生成以 BGCS-CSHARP-ABI-PLAN 拒绝按值结构体调用；当前 SDK 没有对应 aggregate profile，后续 Wasm 解释执行/AOT 产品验收被阻塞。 |
| 其余发布与设备 | macOS ARM64 与完整视觉、输入、音频、长期运行矩阵继续单独记录，不能由 Windows 短运行推断通过。 |

本轮原始日志位于 `artifacts/acceptance/dotnet10-resume-2026-10-20`，BGCS 接入探针与此前局部结果位于 `artifacts/acceptance/bgcs3`。这些本地产物不作为产品运行依赖。

本次 BGCS 3.0.1 的原始证据位于 `artifacts/acceptance/bgcs301-2026-10-20`。文件职责、命令、采样环境与详细结果见[本轮阶段记录](../../records/architecture/acceptance/NET10_BGCS301_PROGRESS_2026_10_20.md)。

完整结果与资源约束事件见[本轮阶段验收报告](../../records/architecture/acceptance/NET10_BGCS301_FINAL_ACCEPTANCE_2026_10_20.md)。233 工程与 191 示例门禁通过；Browser 和设备矩阵仍按实际边界单独报告。

2026-10-21 的[架构与基础能力审计](../../records/architecture/acceptance/ARCHITECTURE_BGCS_CORE_REUSE_2026_10_21.md)核对了 BGCS 包内容、源码引用及 Core 复用；R01/R02/R03 按事实登记，没有把规则门禁当作全部语义已满足。[后续验收](../../records/architecture/acceptance/NET10_BGCS301_CONTINUATION_2026_10_21.md)补充三 API、发布游戏、窗口交互和工程整洁度证据。用户当前无法使用 Mac；[Mac 分阶段入口](../../records/architecture/acceptance/MACOS_ARM64_BGCS301_ACCEPTANCE.md)已整理，命令语法检查不计作设备通过。

## 验收边界

- 保留生产构建校验，不伪造 binding，不手改生成文本或第三方源码。
- SDK worker、native manifest 与冻结输入接入需要验证正式发布包的真实消费。
- 非 BGCS 的中立构建协议、模拟 fixture 和公开契约测试继续执行；不能按名称包含 Binding 批量排除。
- 结果绑定其实际源码和执行范围：主套件合并复验、后续相关构建/回归与最终门禁分别保留证据，不冒充一次全平台完整运行。
- 当前仍需处理构建成本、真实产品中的命令录制性能与设备验收缺口，详见问题台账。

阶段测量和命令保存于[.NET 10 非 BGCS 验收记录](../../records/architecture/acceptance/NET10_NON_BGCS_ACCEPTANCE_2026_10_18.md)与[源码改良记录](../../records/architecture/acceptance/NET10_CSHARP14.md)。其中的通过结论只覆盖其记录时的源码和范围，不代表当前 BGCS 接入已经完成。
