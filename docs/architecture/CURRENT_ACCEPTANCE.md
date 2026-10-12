# 当前验收状态

[架构索引](README.md) · [当前问题](CURRENT_ISSUES.md) · [Wiki 首页](../README.md)

开发入口、构建缓存与 Wiki 整改仍在实施。当前不能将整份计划标记为完成。

## 当前结果

| 范围 | 已取得的证据 | 限制 |
| --- | --- | --- |
| 开发入口 | `InnoEngine.Editor`、`InnoEngine.Build` 与 CLI 闭包编译成功；准备后 Editor 不执行构建 | 当前 Build 工具准备复用的第二次实测 8.86 秒、无 CLI 再编译；完整产品矩阵待执行 |
| Solution 与架构 | 当前 232/232 个项目的真实 SDK 求值图通过；重新编译后 11 项选定架构回归通过；只有两个日常生产可执行，无失效路径、游离项目或空 Folder | 严格 Wiki 和产品运行门禁单独执行 |
| Native 输入与 workspace | 内容哈希、阶段重新验证、同长度同 mtime 修改、CMake object 复用和取消保护已有回归 | 每次完整 Build 的输入读取成本仍需要收口 |
| 绑定与 Task | 精确 generation 消费、读取保护、嵌套 owner、并行 SDK 图和私有载入目录回归通过 | 跨节点与完整成本矩阵尚未闭合 |
| Support Pack | Windows 准备及热命中已有成功证据；本次探针进一步核查输入和提交失败边界 | 固定执行与提交后诊断的专项回归已修复；本轮完整 Windows/Browser Pack 与最终游戏发布仍待闭合 |
| 缓存生命周期 | Native、Binding、Pack 消费持有读取保护；损坏及取消已有专项回归 | 遗漏根已拒绝；Native、绑定、Pack 与 workspace 登记、手动/自动退休已接入并有回归；实际产品与完整预算范围仍待核对 |
| Wiki | 历史记录迁出；全部项目已有页面；四个 Native 页已有可折叠 ABI 索引；资产与缓存公开契约已补齐 | 全量行为审查、生成 API 覆盖和部分精确目标引用尚未通过严格门禁 |

专项测试数量存在重叠；不能将不同筛选批次简单相加作为唯一测试总数。

## 本轮专项验收

基准 kernel 提交 `e17a4a419721a283b3c9a8e3a856ffefa7b7acac`。77 项关键回归和 38 项 owner 集成回归分别通过，数量有重叠。232 项有效 SDK 图检查完成；本轮严格 Wiki 检查报告 81 条问题，当前正在修复并复验，不能标记最终通过。详细过程见[本轮记录](../../records/architecture/acceptance/BUILD_ENTRY_CACHE_WIKI_REMEDIATION_2026_10_12.md)。

## 此前 Windows 运行证据（不替代最新源码验收）

- Debug Editor 普通 Build、准备后启动、正常退出及重新打开已有实际结果。
- 真实界面已检查一次 Play、GameView 按钮输入、停止、浮动窗口创建/位移/关闭，以及带附加窗口的最终退出。测试后恢复原有项目设置。
- 准备后启动测量：部署完整性校验约 1.3–1.5 秒；首帧日志约 4.9–8.8 秒。此记录早于最新 Pack 输入规划变更。
- 当前 Pack 准备约 239.3 秒；第二次请求约 69.5 秒，其中入口 CLI 托管构建约 39.8 秒。热命中结论来自日志，未作为操作系统进程计数。

启动成功不能代替渲染、焦点、音频、存储或 reload GC barrier 验收。

## 尚未完成

- 实际 Pack 固定源码执行，以及缓存完整预算范围、依赖与实构建回收证据。
- 全量 Wiki 严格门禁、逐页行为审查与最终 API/示例映射。
- 当前源码的完整 Editor 性能矩阵、定量 viewport 拖动/resize/DPI、完整 reload/GC barrier。
- Windows CoreCLR、NativeAOT 和 Browser 解释执行/AOT 的最终发布及实际运行。
- 外部消费者的最终编译与插件/脚本消费复验。
- macOS 实机 Native/Editor/Player；Linux 仅有 Native 工具能力，不标记完整产品支持。

详细命令、revision、重叠测试批次、历史测量及原始证据位置见[本轮验收记录](../../records/architecture/acceptance/BUILD_ENTRY_CACHE_WIKI_REFINEMENT_ACCEPTANCE.md)。
当前架构判断及公开边界探针见[提交条件审查](../../records/architecture/acceptance/CURRENT_ARCHITECTURE_REVIEW_2026_10_12.md)。目前可以记录阶段成果，尚不能以整份计划完成的状态合入 main。
