# 当前问题与待确认范围

[架构索引](README.md) · [当前验收](CURRENT_ACCEPTANCE.md) · [Wiki 首页](../README.md)

当前工作区使用 BGCS 3.0.1，实际状态见[当前验收](CURRENT_ACCEPTANCE.md)。.NET 10 / C# 14 的阶段测量见[非 BGCS 报告](../../records/architecture/acceptance/NET10_NON_BGCS_ACCEPTANCE_2026_10_18.md)。未验证不等于存在缺陷，契约通过不等于所有设备已验收。

## 已确认且仍开放

| ID | 优先级 / 所属部分 | 事实与影响 |
| --- | --- | --- |
| O01 | P2 / BGFX、macOS验收 | 10-15 所查 revision 的两条 macOS CI 在单线程 Metal surface 退休用例超时，根因未确认。本轮未复验该设备链；后续隔离、Pack 与 renderer smoke 仍未闭合，不能直接归责 BGCS。 |
| O02 | P2 / 构建性能 | 此前热 Editor Build 为465.297 / 575.609 / 777.781秒，哈希约5.23GB且同期高负载。当前 .NET 10 的完整 Editor 空闲基线未取得；本轮已补独立托管热构建与脚本冷/热/增量基线，不能用不同范围的测量关闭此项。 |
| O06 | P3 / XML与Wiki | 部分注释仍为模板化参数说明，值类型也笼统描述ownership。符号/签名/示例覆盖不等于行为说明充分，应补有实际意义的约束和失败语义。 |
| G01 | BGCS / 产品闭环 | Windows 七组件 HostSource 比对与目标生成后 CheckBindings 均通过，UI 过期输入清单经正式 SDK 重新生成后复验通过。Windows Editor 八组件原生支持已通过正式入口构建，七个部署 DLL 的 4,002 个必需导出全部存在；正式 Task 和生成/工具链统一 20 项回归通过。发布矩阵仍继续验收。 |
| G02 | 调用方 / 最终回归 | 完整 SourceRelease、SourceDebug 源码构建通过；233 工程架构及 Wiki 门禁、191 个示例均通过。完整 Release 运行加三项入口 fixture 复验合并为 2,097 通过、18 跳过，原失败日志保留；后续 CLI 源码输入修正的两配置相关构建及回归通过。2026-10-21 Editor Debug 正式完整构建通过，运行验收另行记录，详见当前验收。 |
| G03 | SDK 能力 / Browser | BGCS 3.0.1 在实际 Browser 生成中以 `BGCS-CSHARP-ABI-PLAN` 拒绝 BGFX 按值结构体调用，Browser aggregate calling profile 尚未实现。该失败阻塞 Browser Player 闭包及 Mono Wasm / Wasm AOT 产品验收；不关闭 ABI 校验或借用桌面 profile。详情及理想修复边界见[专项记录](../../records/architecture/acceptance/BGCS301_BROWSER_AGGREGATE_ABI.md)。 |
| R01 | P2 / 插件事件一致性 | 一个外部插件的公开动画帧事件仍使用直接 C# delegate，尚未统一 Core 的 hub/token 生命周期。没有第二套完整事件总线；替换公开事件会改变本轮保持签名的边界。事实、理想方案与范围见[基础能力审计](../../records/architecture/acceptance/ARCHITECTURE_BGCS_CORE_REUSE_2026_10_21.md)。 |
| R02 | P2 / 插件跨帧 Identity | 一个外部 UI 插件的跨帧状态虽以 RuntimeIdentity 为 key，仍直接持有并使用 live owner；应在后续使用边界通过原 Identity scope 解析。存在 Collect 清理及 Dispose 保护，尚未复现泄漏或崩溃；详见同一审计。 |
| R03 | P2 / Native 验收范围 | `verify-native` 在绑定发现阶段允许选择 Player，后续却固定 Editor 并使用宿主 testhost。应在生成前依据发行声明明确验证产品/runner 能力；不能声称它已覆盖 Browser 静态产品。当前分项证据与整条命令的范围不同，详见基础能力审计。 |

## 待确认和未执行

| ID | 范围 | 当前边界 |
| --- | --- | --- |
| U01 | BGCS正式包 | 当前精确固定 3.0.1，已从公共包还原，通过七组件 HostSource、正式 Task 及 Windows 八组件原生支持准备；Browser 的实际阻塞见 G03，其余产品闭环继续验收。 |
| U02 | 多目标新包生成 | Windows 七组件 HostSource/TargetArtifacts、C/C++ bridge、extension、CheckOnly 及部署必需导出已复验；取消、并发、损坏及原子发布专项回归通过。其他目标仍单独验收；Browser 不含 Editor-only ImGui/ImGuizmo，其当前阻塞见 G03。 |
| U03 | 最终产品revision | 当前源码的 Windows Support Pack 与示例项目 CoreCLR/NativeAOT 发布均已通过；两种部署在 D3D11、D3D12、Vulkan 上分别完成 3,000 帧并正常退出，Editor Debug 三 API 同样通过。Browser 解释执行/AOT 仍受 G03 阻塞，其他设备与长期运行不从本次短运行推断通过。 |
| U04 | MacOSArm64 | runner的Native编译、源码、架构与共同测试已实测；O01后的隔离、Pack/Editor smoke、最终游戏产品与用户机器未闭合，不能笼统标全部未测或全部通过。 |
| U05 | GUI/视觉/声学 | 已补当前示例的 CoreCLR 画面、鼠标/键盘、昼夜和最大化/还原，以及 Editor Play 往返与正常退出。Console 首次点击未切换、保持悬停后再次点击成功，原因尚未定位；不能据此宣称完整交互通过。viewport 拖动、完整 DPI/焦点、Reload 菜单、音频听感及长期运行仍未闭合。 |
| U06 | 负载与长期性能 | fixture、短运行与headless不证明真实GPU帧率、尾延迟、复杂项目资源增长或多小时频繁reload无泄漏。 |
| U07 | 特殊文件系统 | Core.IO严格ownership已修复；Windows大小写敏感目录的Native输入去重仍需确认，内部仍按OS选比较器。独立探针启用属性被拒绝，没有有效复现；macOS两种卷格式亦未完整对照。 |
| U08 | SDK间歇异常 | 历史runner的两个内部MissingMethodException根因未确认；当前head未再出现，不写成已根治，也不据此认定业务代码错误。 |

## 磁盘与本次修正

- **200,000,000,000字节目标未达到**。2026-10-15 18:09（台北）只读采样约73.52GB可用；其他进程仍可能改变占用。37个历史目录按此前自动审批拒绝保留，不补owner或绕过删除。
- 旧Wiki突出旧耗时和过期磁盘/macOS状态。本次重写本页和当前验收，明确revision与范围；原始测量保留在记录区。标注修正不等于源码性能问题已修复。

## 已有依据支持的架构结论

共同领域、Runtime与Hosting消费中立契约；backend、平台基础、integration和发行组合职责清楚。233个当前源码项目有Solution归属，日常生产入口为两个；未发现需要推翻整体架构的依据，也没有本次新确认的核心生产P0/P1架构缺陷。

此前五项整改已实现：严格路径、RenderGraph前缀验证、SDK宿主RID、单调退休序号、轻量缓存维护。O03为另一条录制路径，不表示前缀优化无效。项目数量、API数量、必要完整哈希、安全保留缓存和未实现平台本身不作为缺陷。

BGCS 3.0.1 正式包已可用，专项验收正在执行；失败与未执行项如实记录，不删除断言或改过滤条件制造通过。当前证据见[实施记录](../../records/architecture/acceptance/NET10_BGCS301_PROGRESS_2026_10_20.md)。历史 CI 证据保留于[PR Checks](https://github.com/Wolfiora/InnoEngine/pull/11/checks)。

## 已实现、等待最终整体验收

- O03：并行录制改为本次 Execute 独占的命令缓冲，按值保存 handle/sampler、复制借用数据，并在所有退出路径归还缓冲及清除引用。Debug/Release 各 61 项 Rendering 回归通过；当前三次公开 Noop 边界采样的 10,000 Draw 分配约为 3.14 KB/帧。真实 GPU、长运行及完整产品性能仍需独立验收。
- O04：进程执行机制收口到 `Inno.Testing.Processes`，超时保留 stdout/stderr，终止子进程树并等待退出；独立公开边界回归 4 项通过。三个 Native Window 测试宿主通过项目引用复用唯一实现。
- O05：统一测试入口汇总普通、发现与隔离执行结果，单个独立用例失败后继续其余用例；筛选同时作用于普通执行、隔离发现和隔离执行。协议探针已验证失败聚合，当前完整运行加失败项复验合并为 2,097 通过、18 跳过，原失败证据保留。macOS 产品覆盖缺口仍然存在。
- 分类并发：首轮托管测试发现两个独立 Host 并发发布同一个静态程序集时，分类弱表的 Remove/Add 空窗可能抛出重复 key。公共 ModuleHost 回归已在修复前复现相同异常；原子 AddOrUpdate 后 Runtime 62 项与模块/退休 32 项通过，未改变卸载 barrier 或扩大类型可见性。

## 开发入口变更的验证边界（2026-10-17）

普通工程启动简化已经完成代码整改及既有支持条件下的编译。完整首次 Native/BGCS 准备和普通 Native 热命中未验证；Rider 实机由用户执行。该限制不等于本轮引入了已复现的运行缺陷，详细证据见[入口报告](../../records/architecture/acceptance/ENTRY_PROJECT_BUILD_SIMPLIFICATION_ACCEPTANCE.md)。上述历史测量和其他未决项仍按其原始范围保留。

2026-10-21 补充：Rider 日志暴露的 Task 注册身份、普通输出锁、entry 产品引用、CMake TEMP 工作区及私有复制生命周期已完成共同构建规则整改；并发消费的固定等待也已修正。最终 SDK 入口完整构建与 92 项相关回归通过，详见 [MSBuild 专项记录](../../records/architecture/acceptance/MSBUILD_IDE_TASK_LOADING_2026_10_21.md)。实际 Rider UI 再次启动尚未执行，不能用 SDK 通过替代这一结论。

## 本轮已关闭的构建问题（2026-10-18）

发现并修复了嵌套 Project Solution 的还原图被引擎接管、生成 IDE 工程继承无关 MSBuild 配置的问题。根 Solution 规则已限制到明确 owner，生成工程独立声明 SDK 编译输入；新增两个公开边界回归通过。外部插件示例的正式导入、脚本预检及 SDK IDE 构建均已复验通过。

本轮明确选择用户目录 SDK 10.0.402。Windows Player 的实际编译引用已准备，233 工程架构及 Wiki 整体门禁通过；191 个当前 Wiki 示例编译通过，包含新增测试支持项目页。独立输出下的测试源码输入已改为明确 SDK 声明，完整运行复验继续登记，不能由局部通过推断整套完成。

## 本轮已关闭的 Vulkan 退出问题（2026-10-20）

关闭队列排空不再 touch 已重置的 view，保留原有 Flush、帧数、退休等待和进程 owner 协议。修正前真实窗口回归导致 testhost 崩溃；修正后同一回归在 Vulkan、D3D11、D3D12 上分别通过。五次 Vulkan 及两次 D3D 产品 smoke 全部完成 60 帧并正常退出；详见[专项记录](../../records/architecture/acceptance/WINDOWS_VULKAN_EXIT_FAILURE_2026_10_20.md)。该结果不代替长期设备、视觉和声学验收。
