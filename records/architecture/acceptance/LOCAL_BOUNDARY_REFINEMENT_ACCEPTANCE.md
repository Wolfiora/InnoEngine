# 五项局部边界整改与 Kernel CI 验收

[当前问题](../../../docs/architecture/CURRENT_ISSUES.md) · [执行计划](../plans/LOCAL_BOUNDARY_REFINEMENT_PLAN.md) · [记录索引](../README.md)

## 范围与结论

2026-10-14，基线 `48b33e5d089e8a1500f03d1abae69fd61ffa9d62`。五项局部整改已实现，本机受影响源码、公开边界、实际 Native、后台 Editor、Wiki 与缓存回收通过。不使用 Computer Use，不执行 GUI/视觉/声学验收，不合并 master。

证据：`artifacts/acceptance/local-boundary-refinement/20261014-implementation/`。本地原提交使用仓库配置 Gmail；通过已授权 GitHub 连接发布时，author/committer 均为 Aaron Liao，主邮箱 2384288968@qq.com，关联同一 FLwolfy 账户（ID 124116176）；不添加 Co-authored-by 或 AI footer。远端 CI 必须以新提交的实际 run 为准；本机结果不能替代远端。

## 五项整改

| 问题 | 实现与公开语义 |
| --- | --- |
| F01 路径大小写 | Core.IO ownership 使用严格 ordinal 拼写；拒绝 root case alias、sibling、traversal 与链接。不从 OS 推断卷语义。AtomicFile 同目录约束同步，case alias 自覆盖保守拒绝。 |
| F02 累计图验证 | graph 内复用已接受前缀和 scratch；新的资源、pass、依赖增量分析，完整候选继续执行输出、初始化、循环、排序、存活和 view limit 检查。rollback/未冻结编辑重建状态，不保留 extension callback。 |
| F03 工具部署 | CLI、Bindings、Task、Extension 与 Toolchains 根据所选 SDK 冻结工具宿主 RID；SDK Publish 使用新的 owned deployment，防止旧 RID 残留。产品目标独立，没有手动 DLL 删除名单。 |
| F04 长期帧号 | resource、readback、framebuffer/transient cache 使用 ulong 提交序号；Native uint readyFrame 只转换有限未来距离。surface acknowledgment 保留。进入 inline Native 模式立即记录进程限制；未知 renderer 在 Native 前拒绝。 |
| F05 冷维护 | 中立 ManagedToolRuntime 统一发布；BuildMaintenanceApplication 与 publisher catalog 只读取实际 owner 记录。cache clean 不准备 CLI、BGCS 或 backend 图。普通 Native/Shader Task 使用标准唯一登记点，未记录历史目录不被认领。 |

没有新增生产项目或可执行程序；删除 CLI 专属工具发布 API，全部消费者迁到 Toolchains。不修改 BGCS、extern 或生成绑定。标准发行仍是具体选择的唯一 owner，backend 不依赖平台，共同领域不识别具体 backend。

必要新增 public API：ManagedToolRuntime 用于两个工具闭包的同一固定输入发布；BuildMaintenanceApplication 提供可调用维护入口；BuildCacheOwnerCatalog 持久化明确 publisher roots；BuildCachePublisher.role 供唯一登记点表达真实生命周期。内部 parser、capture、图 scratch 与 SDK 节点排序不扩大公开表面。

## 本机门禁

| 门禁 | 新证据 |
| --- | --- |
| 最终 SourceDebug | 0 errors / 0 warnings，109.047 秒。 |
| SourceRelease | 0 errors / 0 warnings，488.469 秒；后续 Task 登记及测试 target 调整经实际 Debug 产品/Native 闭包编译。 |
| Core.IO | 21/21。 |
| Rendering / Runtime | 52/52、86/86；新增未来 phase 循环、共享资源等价、未冻结 pass 修改。 |
| 架构测试 | 122/122；严格 CLI 架构求值 230/230。 |
| 完整 Build 套件 | 首次 328 项，326 通过、2 失败；失败根因及复验见下节。最终按名称合并复验，剩余失败 0，清单在 unique-build-outcomes.json。 |
| 工具 publication | 3/3；真实连续求值、重复发布与预取消。 |
| 真实 worker / input / catalog | 8/8；十次真实检查、源码/资源/框架/输入稳定性与登记回归。 |
| worker fixture | 30 个默认 JIT 生命周期、非法 nonce、取消、crash/terminate、真实十分钟截止通过。 |
| Native 产品 | 明确 WindowsX64 Editor 计划实际构建与部署，21/21 Shader/组合契约通过；五个独立进程 surface/失败/restart 回归通过，Noop 19/19。 |
| Editor 正式入口 | 新 Debug 产品构建成功；两次后台 120 帧正常退出，无 Native/绑定/Pack 再准备，首帧 8.414 / 4.095 秒。 |
| Wiki | 45,600 符号、190 示例，错误 0；实际裁剪 Runtime/Editor references，不使用 foreign DLL 补齐。 |
| Solution | 230 个受跟踪项目均已纳入；架构门禁检查归属、空 Folder、引用、公开边界。日常生产入口仍两个。 |

## 首次失败与修复

1. 合并 BGFX 用例停住：inline 已消费后重新初始化另一模式。保留第三方进程约束，单线程模式进入后立即记录，随后两种模式均明确拒绝；Native 用例分别运行。
2. 完整 Build 的两项失败：同路径多组属性 MSBuild 节点只按路径排序，跨进程枚举改变声明顺序。按路径和完整 global properties 排序，保留实际参数/include/编译项顺序；连续求值与 publication 复验通过。失败仍拒绝混合输入，并提供文件/声明变化诊断。
3. Native 测试缺运行 DLL：Before VSTest 提前调用 ResolveReferences，缓存不完整结果。部署改为成功 Build 后执行，CI 的目标 Build 显式提供测试产品/目标；不扩大 PrivateAssets、不补旧 DLL。复验 21/21。
4. 工具准备在源码或 Restore 仍变化时两次拒绝 candidate；保留日志，固定源码后重新验收，不把拒绝当成成功。

## 测量与磁盘

32 contributor 成功 fixture：58,920 bytes/frame、0.3108765625 ms/frame，最终 Compile=1；原测量 371,104 bytes/frame、1.57957 ms/frame。该 fixture 不代表 GPU 或完整游戏帧率。

CLI 当前 deployment 167,953,222 bytes（原 1,163,319,881）；维护 deployment 781,991 bytes、10 文件。维护准备后请求 23.765 秒，三次热请求 1.547 / 1.563 / 1.719 秒；热请求没有 compiler/generator 准备。

正式清理先预览，再调用同一 owner/依赖保护计划：逻辑退休 13,358,255,590 bytes，实际卷空闲增加 5,931,876,352 bytes，清理后 83,032,342,528 bytes。未把 junction 或逻辑重复字节当成两份占用。活动 owner、当前产品、必要依赖及验收日志保留。

200,000,000,000 bytes 目标未达到。37 个此前被自动审批拒绝的历史目录保留，没有绕过或补登记；SDK、NuGet、用户/系统数据不清理。现有授权清理范围不足以恢复 200 GB，不能标记空间目标完成。

## PR #11

上次基线 Engine CI：Windows job 115638045779 因隐式并行入口构建出现 CS2012；macOS job 115638046024 缺源码 CLI assets。NuGet dependency graph / dynamic submit-nuget 当时已成功。

本轮修复显式串行入口构建、源码根 Restore、真正的 Native 测试产品前置、SourceDebug→Debug 的独立项目配置映射及按 ProcessIsolation 单独执行。独立用例直接消费同一已部署程序集，避免重复 Native 准备；测试 DLL 路径由 SDK 求值获得，Solution 路径在 macOS 正确规范化。

新提交之后的运行证据单独写入本轮 results/github-ci.json，最终以 [PR #11 Checks](https://github.com/Wolfiora/InnoEngine/pull/11/checks) 的 head revision 为准。远端尚未实际通过时不得宣布 CI 完成。

## 限制

MacOSArm64 Native/产品本机未实测；新的 macOS runner 结果独立记录，不替代用户设备验收。GUI/焦点/DPI/拖动观感/视觉/听音按用户要求未执行。uint 回绕未跑数十亿帧，只验证代码时间转换与普通实际 Native 生命周期。Windows/Web 上一轮产品结果保留为历史证据，不声称本轮重新执行整个 AOT/发布矩阵。

没有发现需要推翻整体分层的新问题；上述结论限定于已核查和已运行范围，不能证明所有设备、长期运行和所有未来扩展都没有缺陷。Glass.aiff 在当前 Windows 不存在，提示音未播放。

## 远端首次运行与后续修复

源码提交 73b9c7b0d256b94954aca2fe42fb6266bacba1c6 的文件树与本地原提交 c3281e40 完全一致。NuGet run 38538859631 已成功；Engine run 38538859621 的 Windows/macOS 都缺声明为辅助输入的 TaskHosting 源码 assets，说明只 Restore CLI 根项目仍不完整。

后续修复使用现有 SDK graph evaluator 的独立准备阶段：从 InnoBuildInputProject 与其清理后的属性发现辅助输入，使用同一 SDK Restore，然后重新建立只读 Capture。没有平台分支或写死 TaskHosting 名称。新 fixture 证明仅声明、从未 Restore 的辅助项目在 publication 前得到还原并实际消费，1/1 通过。远端后续结果仍以新 revision 的 run 为准。

提交 1e921fcc 的 NuGet run 38541975031 成功。Engine run 38541975019 的 macOS 在首次 BindingExtension 编译后拒绝输入变化：首次宿主 Restore 新建 lock 文件，原先目录身份在 Restore 前计算。修复在共享 Restore lease 中先完成所选 SDK 的 Release Restore，再冻结扩展目录，保留锁后及编译后 fresh 校验。

真实扩展 fixture 的生成和热身份检查通过；首次同进程用例在删除已加载 DLL 时失败，因此改为父进程持有已登记临时目录、子进程执行公开生成 API、退出后清理。最终独立进程用例 1/1 通过，证据 extension-restore-isolated.trx；没有更改 BGCS、跳过哈希或用 GC/JIT 开关掩盖 DLL 生命周期。远端最终结果仍等待后续 revision 验证。
