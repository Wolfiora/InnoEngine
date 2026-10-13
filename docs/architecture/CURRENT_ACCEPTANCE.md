# 当前验收

[架构索引](README.md) · [Wiki 首页](../README.md) · [问题台账](CURRENT_ISSUES.md)

## 当前执行范围

本轮基线为 `a82fb6b9ed50405029ea653c1d5283854ef2ca0e`，分支 `kernel`，2026-10-13 更新。用户已取消本轮 GUI 与 Computer Use 验收。源码整改及以下非 GUI 功能门禁已通过，未发现新的阻止源码提交的架构或功能缺陷；完整原计划仍有性能测量和磁盘清理剩余项，不能描述为所有验收全部完成。详细过程位于 [验收记录](../../records/architecture/acceptance/FINAL_PLATFORM_RELOAD_NUGET_ACCEPTANCE.md)。

## 已取得的新证据

| 范围 | 本轮结果 |
| --- | --- |
| Reload guard / Play generation | 最新 PlayMode 53 项与 Hosting 6 项通过；GUI 已确认 Playing 禁用 Reload、自动变化暂存、停止后激活和 GC 完成。GameView 聚焦的停止快捷键修复另通过公开路由测试。 |
| Scene Plugin Missing / recovery | 2 项 owner/rollback 回归通过；Missing/recovery 与真实 Plugin reload 已复验。 |
| Cache / Task / platform composition | 62 项专项测试通过。 |
| BGCS NuGet 消费 | 绑定库和 CLI 编译通过；包族固定 2.1.0，包内容记录在锁文件。 |
| 目标与源码依赖删除 | 当前目标 WindowsX64、MacOSArm64、BrowserWasm32；源码依赖目录和未选择的平台模块已删除。 |
| 架构规则 | 最新完整架构 122 项通过；SourceDebug/SourceRelease 均 0 errors、0 warnings。 |
| Wiki / Solution | 完整严格 Wiki 门禁 2 项通过：352 页、45,548 个公开/可扩展符号、189 段示例。有效图和 Solution 均为 230 个项目，无遗漏项目或空 Solution Folder。 |
| Console / 窗口生命周期 | Console 原生布局 4 项、窗口/退休回归 40 项通过；标题仅显示等级，类型在详情。 |
| Windows Player | 最新 CoreCLR、NativeAOT 发布通过，各两次 360 帧后台运行正常退出，stderr 为空；存储跨启动递增、音频 DSP 时钟推进。后台运行不等于视觉或声学验收。 |
| Browser Player | 最新解释执行/AOT 最终发布通过，AOT 链接包含 88 个程序集；此前网页实际运行证据保留，最新日志 API 修正后的交互未重跑。 |
| 公开消费者 | 最新源码编译、Plugin 导出和脚本 API 消费通过。 |
| Editor 部署 | Debug 558 个、Release 507 个 payload 哈希一致，无缺失或额外文件；锁文件按入口协议排除。 |
| 包一致性 | 653 份锁文件中的 286 条 BGCS 包记录均为固定 2.1.0；生成器不进入 Player 运行闭包。 |
| 磁盘回收 | 最新正式 owner 清理实际释放 11.54 GiB。额外历史目录删除被自动审批拒绝，未达到约 200 GB 可用空间目标。 |

NativeAOT 实测发现并修复了 `Assembly.GetCallingAssembly()` 不可用问题。静态日志 API 现在显式借用来源类型，例如 `Log.Info(typeof(PlayerController), "Started.")`，日志记录不持有该类型或程序集。详见 [Logging](../core/Inno.Core.Logging.md)。

## 尚未完成

三次热 Editor Build 为 281.564、194.049、251.685 秒，Native compiler 均未启动。完整构建仍为分钟级，不能宣称性能问题全部消失。此前 Release 真实启动从进程记录到首帧约 10.15 秒，没有 Native、绑定或 Pack 准备；不能以 `--status` 的宿主查询时间冒充 Editor 启动耗时。

相同 Support Pack 请求返回同一内容身份，未重新执行 Native compiler、绑定生成或 Player managed Build；请求仍包含项目求值、哈希和完整性验证。真实小 Native 修改后的产品级耗时仍缺独立测量，公开边界的 workspace 复用测试不能替代该性能数据。

最新 GUI、完整焦点/DPI/视觉/声学矩阵按用户要求未执行。额外历史中间目录删除被自动审批拒绝，空间目标仍未完成，见 [磁盘报告](../../records/architecture/acceptance/DISK_CLEANUP_20261013.md)。当前可以提交已通过源码门禁的修改，提交说明须保留限制；没有自动提交，也不将未执行项写成通过。

MacOSArm64 只在 Windows 上进行源码、项目与组合核查；Native/Editor/Player 实机运行 **未实测**。未实现宿主不注册产品。声学听音与多屏 DPI 必须单列实际设备条件。

默认 Solution 的平台库显示 not built，仅表示未参加当前配置的批量构建。两个开发入口与独立产品准备方式见 [启动说明](../platform/PRODUCT_STARTUP.md)。
