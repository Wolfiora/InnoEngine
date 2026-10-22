# 当前验收

[架构索引](README.md) · [Wiki 首页](../README.md) · [问题台账](CURRENT_ISSUES.md)

## 工具链与范围

使用 SDK **10.0.402**、`latestPatch`、禁用预览 SDK，语言为 **C# 14**。运行目标为 `net10.0` / `net10.0-browser`；两个 Roslyn 生成器保留 `netstandard2.0` 工具产物。源码及脚本逐文件显式声明普通 using，禁止 global using、implicit using、MSBuild `Using` item 和 metadata 隐式导入。

BGCS 包族精确固定 **3.0.1**，通过正式 NuGet 包及官方 API 使用。平台 SDK、ABI、绑定 Profile、链接和产品布局归所属平台；组件保留公共绑定语义及可移植 recipe。开发入口只有 `InnoEngine.Editor` 和 `InnoEngine.Build`，Editor 运行期间不组织源码构建。

本轮基线为 `dotnet10` 的 `18f281510daf48968bdd7a60248e17ece9fbe420`，环境为 Windows x64。独立示例项目、最终结果、命令和证据见[平台职责与缓存验收](../../records/architecture/acceptance/PLATFORM_OWNERSHIP_CACHE_ACCEPTANCE_2026_10_22.md)，实际改动位置见[交付文件树](../../records/architecture/acceptance/PLATFORM_OWNERSHIP_CACHE_FILE_TREE_2026_10_22.md)。

## 当前验证

<!-- current-platform-results -->
- **编译与架构通过：** 最终源码全集 Release、正式 Native 专项中的完整 SourceDebug、Editor Debug/Release，238/238 工程 SDK 求值及架构/Wiki 门禁。196 个 Wiki 示例全部编译通过。
- **测试：** 原始完整套件 2,155 项中的三项失败经相关回归消解；最终相关 175 项中 173 通过、2 项已有条件跳过。按用例去重后的证据为 2,140 通过、18 条件跳过、0 未解决失败，共 2,158 项；原始失败阶段保留。七个 Native 工程另有 36 项通过，不重复累加。
- **Windows 产品通过：** 示例项目的 Editor Debug/Release、CoreCLR 与 NativeAOT，在 D3D11、D3D12、Vulkan 各推进 3,000 帧并正常退出。实际 UI、音频和长期运行的覆盖边界见报告。
- **增量与缓存：** 十次连续热构建均 Native processes=0、bindingGenerations=0，22 个 current 数量、内容和时间戳稳定；显式维护验证已完成操作收敛。并发维护期间保留活动 Task workspace，后续缓存 Restore 复用成功。
- **第三方完整性：** 实际装载 BGCS 3.0.1；129 个包文件哈希不变，第三方源码及三个调用方仓库保持干净。Support Pack staging 与索引退休沿用统一事务和 owner 协议。
<!-- /current-platform-results -->

## 插件边界后续复验

外部插件事件与跨帧 owner 的 R01/R02 已经正式脚本、生命周期测试和安装态运行复验关闭；详见[插件复验](../../records/architecture/acceptance/PLUGIN_EVENTS_IDENTITY_ACCEPTANCE_2026_10_22.md)。Browser 使用同一官方包再次确认阻塞，Mac 仍未执行。

## 外部限制与证据边界

- **Browser：阻塞。** 实际 workload 下，正式 BGFX 生成被 BGCS 3.0.1 的 `BGCS-CSHARP-ABI-PLAN` 拒绝，缺少 aggregate calling profile。Mono Wasm / Wasm AOT 产品不计作通过；没有改包、借用桌面 ABI 或关闭校验。见[SDK 能力记录](../../records/architecture/acceptance/BGCS301_BROWSER_AGGREGATE_ABI.md)。
- **macOS ARM64：本轮实机未执行。** 当前没有 Mac；Metal、Editor、游戏、NativeAOT 和 Unix 卷结果不能从 Windows fixture 推断。历史 CI 按原 revision 和范围保留。
- **Rider UI：未执行。** 正式 SDK/MSBuild、固定 IDE 式求值和进程外 Task 回归与实际点击 Rider Run 分别报告。
- 完整 DPI、焦点、音频听感、多小时运行和高频 reload 未闭合。短时帧推进、正常退出和部分 GUI 操作只证明已检查部分。

测试保留生产校验、输入冻结、lease、取消及原子发布；不按名称包含 Binding 批量排除，不使用 IVT、反射穿透或测试后门。普通执行、隔离发现和隔离执行共享筛选条件。

性能比较使用相同 SDK、配置、输入和重复采样。路径解析的独立实现对照不代表整个引擎提速；没有完整 Editor 改造前同条件五次基线时，不宣称已排除全部性能回退。缓存分别报告 Native 编译、绑定生成、稳定 selection、受保护工作区、逻辑字节与卷占用。

完整日志、binlog、TRX、包哈希和采样位于本地 `artifacts/acceptance/platform-build-2026-10-22/`，不进入 Git，也不是产品依赖。历史 [.NET 10 验收](../../records/architecture/acceptance/NET10_BGCS301_FINAL_ACCEPTANCE_2026_10_20.md)、[MSBuild 专项](../../records/architecture/acceptance/MSBUILD_IDE_TASK_LOADING_2026_10_21.md)、[Vulkan 退出专项](../../records/architecture/acceptance/WINDOWS_VULKAN_EXIT_FAILURE_2026_10_20.md)和[Core 审计](../../records/architecture/acceptance/ARCHITECTURE_BGCS_CORE_REUSE_2026_10_21.md)仅证明其各自记录的源码与范围。
