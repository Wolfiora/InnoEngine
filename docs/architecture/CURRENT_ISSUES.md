# 当前问题与验证缺口

[架构索引](README.md) · [当前验收](CURRENT_ACCEPTANCE.md) · [Wiki 首页](../README.md)

2026-10-13 复核：源码编译、架构、严格 Wiki 和已执行产品路径未发现新的提交阻断缺陷。以下项目仍开放；不能因为已有测试通过就称引擎绝无问题。

| 项目 | 状态与处理边界 |
| --- | --- |
| 历史中间产物 | 已登记的可退休缓存已清理。另有 37 个历史目录约 51.32 GiB 逻辑占用，删除被自动审批拒绝；尚未恢复约 200 GB 可用空间。 |
| 热构建成本 | 三次 Editor 热构建约 194–282 秒，Native compiler 为 0；输入求值、完整哈希和 Task 准备仍有成本，不能宣称总体性能全部优化完成。 |
| 性能矩阵 | 小 Native 修改后的真实产品级增量耗时尚缺测量。Fixture 的 workspace 复用和失效测试不代替产品性能。 |
| GUI / 视觉 / 声学 | 用户取消本轮 GUI 和 Computer Use 验收，最新完整焦点、DPI、视觉和实际听音不标记为通过。 |
| Browser 最新交互 | 最新解释执行/AOT 发布和最终链接通过；日志来源契约修正后的网页交互未重跑。此前证据属于对应部署。 |
| MacOSArm64 实机 | 源码、组合、包检查通过；Native、Editor、Player 实机未实测。 |
| Build 入口 JIT 配置 | 当前 .NET 9 宿主的 collectible inspection 路径曾出现 JIT 运行异常；Build 入口单独禁用 tiered compilation 后复验成功。不宣称已证明 CLR 内部根因。 |
| 远端 CI | 本地结果不代表远端 workflow 已通过，以对应远端记录为准。 |

MacOSArm64 的托管源码检查通过；Native、Editor、Player 实机结果未实测。多屏 DPI 与声学听音必须记录实际设备条件。远端 CI 的通过状态需要远端运行证据，不由本地结果推断。

已经修复的 Reload/Identity、Console、明确目标命名、BGCS NuGet 化、Pack 闭包和 Task 交接不继续列为待实现项。详细结果见[本轮验收记录](../../records/architecture/acceptance/FINAL_PLATFORM_RELOAD_NUGET_ACCEPTANCE.md)，空间和拒绝结果见[磁盘报告](../../records/architecture/acceptance/DISK_CLEANUP_20261013.md)。
