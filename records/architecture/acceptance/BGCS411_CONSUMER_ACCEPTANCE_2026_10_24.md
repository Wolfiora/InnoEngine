# BGCS 4.1.1 消费与产品复验

[当前验收](../../../docs/architecture/CURRENT_ACCEPTANCE.md) · [问题台账](../../../docs/architecture/CURRENT_ISSUES.md) · [4.1.0 验收记录](BGCS410_CONSUMER_ACCEPTANCE_2026_10_24.md)

## 范围与基线

本轮在 Windows x64、`dotnet10` 分支和 .NET SDK 10.0.402 下恢复消费验收。引擎源码基线为 `06598ce0d07acf33b71eda58c846c3d6154ff32a`；保留工作区已有的 `BGCS410_ANONYMOUS_TYPE_DEFECT_HANDOFF.md`，不覆盖其历史复现结论。

正式 [BGCS 4.1.1](https://github.com/FLwolfy/BindGen-CS/releases/tag/v4.1.1) 对应源码 `7f5cb7cfb2bc4fb10b23a30117c99e511201c1e2`。官方公共包验证记录已下载到本轮证据目录。该 SDK 的通过记录只用于核对来源，不代替引擎的生成、原生编译、链接及产品运行验收。

本轮证据归属 `artifacts/acceptance/bgcs411-2026-10-24/`。工作区、包文件与空间基线分别记录；未通过或未执行的检查不得写成通过。

后续全仓复核与最新问题分类见[核心验收报告](FULL_ENGINE_AUDIT_2026_10_24.md)，不把生成成功或 SDK 自身测试替代引擎运行验收。

## 执行顺序与验收状态

| 阶段 | 内容 | 状态 |
| --- | --- | --- |
| 1 | 正式发布、公共验证记录、源码身份与工作区基线 | 发布及公共记录已核对；实际消费的 13 个 SDK DLL 与发布验收摘要一致。包文件最终完整性待复核。 |
| 2 | 包族统一 4.1.1，各实际作用域正式还原 | 八组 Solution 还原及五组产品根还原通过；旧锁文件逐份核对 owner，最终清单待复核。 |
| 3 | 正式 HostSource / Browser 生成、冻结输入和 native 编译 | 七组件 HostSource 生成通过；原 pthread 精简复现的生成与 C17 编译通过；Browser 五组件生成、实际原生聚合编译及发布通过。 |
| 4 | FlappyBird 的 Mono Wasm / Wasm AOT 发布与实际运行 | 两种发布通过；AOT 3,000 帧 smoke 通过；Mono 解释执行 UInt64 函数指针调用中止。无引擎/BGCS 的独立对照同样失败，直接 P/Invoke 对照通过，详见核心报告 F01。 |
| 5 | Windows、普通/隔离测试、架构、脚本与 IDE 回归 | SourceDebug 0 警告/0 错误；238 工程门禁通过；59 个现有测试工程原始结果 2,139 通过、2 失败、18 跳过，28 个隔离用例通过。文档失败修正后的 Wiki 门禁通过，assets 作用域测试失败仍开放；三个调用方脚本/IDE 通过。Windows 完整产品矩阵未在本轮重跑。 |
| 6 | 热构建、缓存维护、文档与深层自查 | 待执行。 |

## 固定边界

- 不修改 BGCS 仓库、NuGet 包、第三方头或生成文本；生成物只由官方流程更新。
- 不添加 pthread 类型名单、平台补丁、关闭断言、兼容读取或双执行路径。
- 4.1.1 的原生模型 revision 变化由 SDK 自身验证；引擎继续按实际包、扩展、工具和冻结输入计算身份，不消费旧布局产物。
- 平台 integration 注册官方扩展；中立 Toolchains 不识别 BGCS 类型、Browser ABI 或具体系统头。
- 保留普通 using、C# 14、.NET 10、输入冻结、Core IO lease、取消和原子发布。
- macOS 没有设备，实机验收单独标明未执行；本轮不使用 computer use。

## 最终结果

整体未闭合。原匿名类型生成缺陷已有真实修复证据，Browser Native 与发布通过；解释执行运行失败。SourceDebug 已通过，完整现有测试取得当前结果但有失败；新增 Core 生命周期和 History 反例亦确认缺口。具体明细及 Wiki 修正前后范围见核心验收报告，不沿用上轮统计作为本轮通过结果。

## 已确认的消费证据

- `anonymous-system-record-result.json`：正式 4.1.1 生成成功，实际 workload 的 Clang C17 编译退出 0，未修改生成源；原 SDK 缺陷的精简输入不再失败。
- `consumer-sdk-assembly-identity.json`：Build entry 实际部署的 13 个 SDK/Runtime/Browser 扩展 DLL 与发布验收摘要逐项一致。
- `host-generation-results.json`：七组件均通过正式 `GenerateBindings`，不手工整理生成目录。
- `browser-support-configured.log`：Browser 的五组件绑定生成和完整原生产品构建通过，`Native processes: 3; binding generations: 5`；结果通过现有验证及原子发布流程。
- `architecture-source.log`：238/238 项有效 MSBuild 工程求值及架构验证通过。

首次 Browser support 调用因当前进程 PATH 中缺少 CMake，在工具预检阶段失败，原始日志为 `browser-support.log`。随后使用 vswhere 定位已安装的 Visual Studio CMake/Ninja，在该验收进程暴露工具目录后通过；位置记录于 `browser-host-tools.json`。未写死安装盘符或修改生产工具解析。启动指南明确 Browser 对 PATH 工具的要求，目标 Emscripten 仍由实际 workload 选择。

## Browser 入口一致性

Browser 的 Program 将 .NET 应用参数交给既有 `PlayerCommandLineOptions`，composition 将图形 API、窗口可见性和有限帧设置传给共同 Player。页面通过 .NET 官方 `withApplicationArgumentsFromQuery()` 接收重复 `arg` 参数，非零应用退出显示错误。

例如 `index.html?arg=--smoke-frames&arg=3000` 使用现有共同帧统计、退出和资源释放路径，没有增加测试专用入口或第二个参数解析器。默认无参数行为保持持续运行。实际产品验证结果仍以本轮运行日志为准。
