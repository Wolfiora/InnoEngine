# 当前源码验收

[架构索引](README.md) · [当前问题](CURRENT_ISSUES.md) · [Wiki 首页](../README.md)

最新结果见[三项优先修复专项验收](../../records/architecture/acceptance/PRIORITY_FIX_ACCEPTANCE_2026_10_25.md)。基线为 .NET SDK 10.0.402、C# 14、BGCS 4.1.1；本轮平台为 Windows x64。

## 本轮已执行

| 范围 | 当前结果 |
| --- | --- |
| Core Coroutine | Debug / Release 各 40 项通过；清理、失败传播、重入停止与其他活动项推进均有公开边界回归 |
| Runtime 回归 | 62 项通过；不将其当作私有 session scheduler admission 已落地的证据 |
| Binding / native 构建契约 | 43 项通过；含实际 SDK 生成、导入身份、链接独立性、取消与完整产物检查 |
| 架构 / XML / Wiki | 238/238 工程通过；33,567 API symbols；196 示例通过 |
| Build 入口与独立 binding owner | 零警告、零错误；独立 Browser owner 不依赖产品 selection 也能取得生成机制 |
| Browser native | 五组件官方生成、538 步原生编译与原子发布通过 |
| Browser 产品 | Mono 解释执行和 AOT 均正式发布，各完成 3,000 帧、输入、存储读取和有序退出 |
| BGCS 完整性 | 148 个包文件未改变；仓库 clean；Agent 文件 0 |

当前 Browser 平台通过官方 LibraryImport 获取运行时可发现的真实调用签名。共享 backend 根据冻结机制初始化，不判断平台名或返回类型；没有 SDK / generated 补丁。FunctionTable 的当前 Mono 负面对照与 SDK 缓存指纹缺口仍有独立 issue。

专项合计 185 项通过，0 失败 / 跳过。首轮源码在途变化、Wiki 映射缺口与工具缺失的失败日志保留；最终复验范围及替代关系见报告。

## 尚未完成的矩阵与问题

- macOS / Metal 无设备，未执行。
- Windows 全图形 API、Editor Debug/Release、CoreCLR/NativeAOT 和完整视觉、焦点、DPI、音频矩阵未在本轮重跑。
- 示例产品已观察存储读取，尚未触发新的最高分写入，不能宣称分数跨启动存储已验收。
- 非致命 console 资源诊断仍待归因；自动维护预算、热构建成本及其他审计问题继续开放，均按报告链接进入 issue。
- 没有使用 computer use，也没有把 CLI/MSBuild 回归当作实际点击 Rider Run 的证明。

## 历史证据

[全仓核心审计](../../records/architecture/acceptance/FULL_ENGINE_AUDIT_2026_10_24.md)、[BGCS 4.1.1 消费验收](../../records/architecture/acceptance/BGCS411_CONSUMER_ACCEPTANCE_2026_10_24.md)及更早平台记录按其 revision / 检查范围保留。本页当前结果优先；历史通过数量不重复计入本轮。
