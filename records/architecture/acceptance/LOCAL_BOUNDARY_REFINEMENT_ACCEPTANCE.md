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

提交 3dff6853 的 NuGet run 38546460829 成功；绑定批次 6/6、架构测试 122/122 本机复验通过。macOS Engine job 115793622505 已完成生成及 SDL 编译，随后登记 CMake workspace 中 libSDL3.dylib 符号链接时失败。修复按生命周期区分清单：仅 workspace 允许同一目录边界内最终指向普通文件的文件别名，不重复计数；目录链接、越界、循环、悬空链接及 immutable/process 内容中的链接继续拒绝。Native workspace 在最终产物提交前完成验证。Windows 缓存回归 16 通过、2 个 Unix 文件链接用例明确 Skip；Unix 行为等待新 macOS run 实测，不能将 Skip 计为通过。

加入 NativeArtifactPublicationTests 复验后，最终 workspace-link-final.trx 为 26 通过、2 Skip、0 失败；Toolchains 编译 0 errors / 0 warnings。退休与登记共用同一内部角色清单，不放宽 Core.IO 的通用无链接边界，也不允许最终 Native 产物保存符号链接。

提交 af3ad428 的 macOS job 115807017981 已编译 SDL、Text、UI 及 BGFX 工具，随后拒绝 ImGui 的未声明生成 CMake 目录。整改保持 CMake 工程为 recipe 声明的冻结文件，窗口源码变换读取冻结上游副本，经明确参数传入组件 workspace 生成文件。删除两份无调用方的内部构建辅助文件。显式启用 INNO_ACCEPTANCE_IMGUI_BUILD 后的 WindowsX64 ImGui/ImGuizmo 真实构建 1/1 通过（imgui-overlay-cold-build.trx），不能用普通套件中的未启用结果冒充实际构建。实际 recipe 也覆盖缓存文件清单实现。

先前 Windows job 115778668157 已通过 Native、托管及架构阶段，完整套件返回 12 项失败：Build fixture 的时序/共享输出竞争、runner 上 SDK 10 的隐式选择、Wiki 验证，以及 Reload fixture 的错误 domain。Build 用例顺序调度，显式并发契约不删减；CI 将 .NET 9 安装到独立目录并始终上传测试/Wiki 证据。Reload 用例在创建编辑场景及组件时进入所属 Edit Session，保留生产 Identity owner 拒绝规则，单项复验 1/1 通过（scene-reload-owner-fixture.trx）。Wiki 及后续完整 CI 仍需新 revision 实际结果，不能把这些诊断写成全绿。

顺序调度后的受影响 SDK/ownership/restore/publication 回归为 30/30（serialized-sdk-regression.trx）；真正的并发、取消及锁等待断言仍执行。独立 SDK 安装使用 setup-dotnet 的 DOTNET_INSTALL_DIR 正式入口，未改变引擎目标框架或通过 JIT/错误忽略参数绕过失败。

## 81ebb653 的远端完整套件与跨平台修复

Engine run 38558695831 的 Windows 完整套件只剩 RepositoryWikiTests 失败；该 Build 程序集为 331 passed、2 Unix 权限环境 Skip、1 failed。macOS 的 177 个失败以实际上传 TRX 分组，不按重复日志行累加。Native Debug/Release、托管 SourceDebug 与基础架构阶段在两个 runner 均通过；NuGet run 38558695699 成功。失败未标记为通过。

- Darwin 的 .NET 9 flock 竞争异常使用原始 errno 35，原实现只识别 11/Windows 错误。FileLease 按当前系统的真实竞争码等待；权限、路径等错误仍立即失败。[.NET Unix 文件实现](https://raw.githubusercontent.com/dotnet/runtime/v9.0.20/src/libraries/System.Private.CoreLib/src/Microsoft/Win32/SafeHandles/SafeFileHandle.Unix.cs)与[错误转换](https://raw.githubusercontent.com/dotnet/runtime/v9.0.20/src/libraries/Common/src/Interop/Unix/Interop.IOErrors.cs)支持该判断。
- macOS /var 的最终物理路径不能覆盖 checkout 创作归属。读取仍按最终路径去重，但冻结清单保存原始读取映射；增加真实目录别名的固定 Managed execution 用例。
- ImGui 的 macOS 文件名原为 cimgui-debug.dylib，绑定请求 libcimgui-debug。CMake 明确 PREFIX 空、OUTPUT_NAME libcimgui，修复实际生产输出，未添加 loader 猜测或 alias fallback。
- Cocoa 要求进程主线程；xUnit worker 不能初始化视频。SDL/viewport 用例由测试程序集的明确主线程入口直接执行原断言，故障用例仍独立退出。没有生产 Program、反射、假窗口驱动或跳过真实断言。
- SDK 引用测试选择实际宿主产品配置；单文件发布 fixture 使用宿主模板和 Native 文件扩展名，不在 macOS 发布 Windows 组合。
- Wiki gate 原先尝试编译未准备的外国产品引用。显式 --wiki-target 限定产品编译，并保留全部共同项目及全仓页面/链接/内容检查；未选目标示例明确标记 foreign-product / notEvaluatedReason，不能计为编译通过。省略限定仍严格要求全部实际引用；不拿宿主 DLL 填补外国目标。CI Windows/macOS 各自检查所属目标。新增正反例确保所选目标的错误示例仍失败。Wiki 示例直接取所属 SDK compilation，不依赖不稳定的文件枚举选出一个不在 Compile 中的文件。

当前修改的命令和结果继续写入 results；远端通过必须由新 commit 对应 workflow 证明。GUI、Computer Use、人工焦点/视觉/声音不执行。

当前 SourceDebug 全仓构建 0 errors / 0 warnings。目录别名、固定输入、SDK profile 与单文件发布 15/15；Core.IO 21/21；Windows 强制独立主线程 SDL 10/10、viewport 9/9 与故障退休 1/1；明确启用实际 ImGui/ImGuizmo 构建 1/1；架构 123/123。新增测试入口没有进入生产清单。第一次架构失败暴露 backend/tests 的测试 Exe 被误判为生产，分类已修正并重新通过。SDK 主进程仍加载输出时发生的一次本地重建冲突已停止该 owner，源码固定后重跑通过；不把失败日志删除或计为通过。Wiki 及后续远端仍在验证中。

WindowsX64 明确目标 Wiki 门禁已通过（wiki-target-scope-repaired.trx，12m26s）：45,598 符号、190 段实际编译示例、0 错误；其他产品 API 编译明确未执行。该轮有独立 BGCS 任务及 Release Build 并行负载，不作性能比较。新记录保留 validation-errors.txt。SourceDebug 与受影响契约已通过；Release 全仓构建尚在运行，新远端 CI 尚未通过。

SourceRelease 全仓构建也已通过，0 errors / 0 warnings（source-release-ci-followup.log，12m55s）。本轮新提交前的编译与受影响非 GUI 门禁通过；Windows 明确目标 Wiki、真实 Native 构建及主线程测试均有新证据。新的 GitHub CI 尚需提交后执行，不宣称其已通过。

## dd828bd0 的完整远端结果

Engine run 38589373455 的 Windows 普通套件仍仅 Wiki 失败：WindowsX64PlayerContentSource.PrepareAsync 的 inheritdoc 无法在 runner 当前引用闭包中解析到契约说明；Build 程序集 332 passed、2 Skip、1 failed。实际约束不改变，Windows 内部 source 采用与 macOS 相同的完整英文 metadata/pack 所有权、参数、取消和失败说明；本机源码架构复验通过。

macOS job115939828795 在 Native、托管和基础架构通过后，target test closure 编译遇到 CSC CS1504：System.Security.Cryptography.HashAlgorithmNames.HashAlgorithmToPal 缺失。测试未开始，不能宣布前一轮177项失败已全部解决。具体运行时根因尚未确认；重新执行同一 revision 的 macOS job，新的 job116000075113 已开始。Windows 原 job在新 attempt中映射为116000076247，保留原失败。没有关闭 JIT、弱化哈希或忽略编译错误；CI 增加非敏感 dotnet --info 和明确编译器属性记录，固定非共享编译进程选择。NuGet run38589373988 已成功。

Windows 内部内容 source 的完整 XML 在源码架构用例中通过（windows-player-xml-contract.trx）。直接 Player Build 还触发离线 shader-tools 冷编译；仅注释变更不需要该额外冷构建，已取消所属进程并保留日志，不将取消记为编译成功。新生成的三份 windows-x64 NuGet lock 保留为当前图，BGCS.Runtime 固定 2.1.0。CI 编译器属性探测命令在本机执行通过，workflow 缩进人工复核；本机没有 YAML parser，解析有效性由 GitHub workflow 校验。

## ecac2463 的完整远端结果与发布布局修复

Engine run 38608685636：Windows Native、源码、架构、共同与独立进程测试全部通过，日志合计 2,054 passed、18 Skip、0 failed（18 项为 16 个 Metal 环境用例及两个 Unix 文件链接权限用例）。Support Pack 命令成功生成内容身份，但 workflow 随后递归统计整个目标目录，误把 Pack 本体和 PlayerLink 中各自必需的 miniaudio-release.dll 判为重复，尚未运行 renderer smoke。NuGet run 38608685756 成功。

macOS 的 Native、源码、基础架构和 Wiki 已通过；上传 TRX 为 2,044 passed、2 failed、0 Skip。剩余失败分别是单文件发布后的 Native 路径验证、viewport fixture 将 Cocoa 错写为 Win32。旧 revision 的 macOS 重跑也完成测试，未再出现 HashAlgorithmToPal 方法缺失；其运行时根因仍未确认，不能将一次未复现描述成已找出根因。新 runner 实际 SDK 9.0.319、CoreCLR 9.0.21、osx-arm64，编译器选择记录 UseSharedCompilation=false；dotnet host 自身为 10.0.13，不等于 SDK 或引擎框架已升级。

整改：两个桌面发布模板通过 NormalizeDirectory 得到明确的发布根，再验证每一个精确 Native 相对路径；不依赖 PublishDir 末尾分隔符、不添加文件名 fallback。发布 fixture 在当前 SDK 宿主分别消费两份真实模板，并单独调用公开 SDK target 验证无末尾分隔符的参数；Windows 新复验 2/2。首次 NormalizePath 嵌套 condition 存在 MSBuild 引号错误，保留失败日志，改为 target 内单一私有 property 后重新通过，不将失败算成成功。该 fixture 只验证发布协议，不把 PE 测试输入伪称 macOS Native 实机调用。

viewport 用例改为与所属 SDL 应用主窗口的 surface kind 比较；Windows 强制独立主线程复验 1/1。61 项源码/公共边界回归通过。workflow 在读取严格 current 内容身份后，分别要求 pack/native 和 pack/PlayerLink/native 各一份 MiniAudio，比较真实 SHA-256；本机当前有效 Pack 的两处计数与哈希通过。五个 PowerShell script block 经矩阵表达式替换后解析通过，GitHub YAML 解析及完整新 CI 仍待提交后验证。

这些修复不放宽 Native 闭包、平台 ABI、内容哈希、读取 lease 或 exports 规则；不创建第三份 Native 输入，不改 BGCS、extern、框架或生成绑定。新的 commit 仍须完整 CI、Support Pack 和 renderer smoke 实际通过后才能标记收口。

提交后的后续独立进程检查发现，三个 BGFX surface 退休用例也创建真实 Cocoa 窗口，之前尚未进入的独立阶段仍会从 xUnit worker 调用。按已验证的 SDL/viewport 测试方式补齐本测试程序集的明确主线程入口，直接调用原三个断言；生产启动项目不增加。Windows 强制相同子进程路径复验 3/3，源码/项目架构门禁继续复验。没有用缺平台时 return、假窗口或反射跳过真实渲染。

## 渲染 smoke 的实际能力与验收范围

2d122c24 的 Windows job116224092465 已通过完整共同/独立套件及所选 Support Pack，Editor 也完成三帧并正常退出。最后的 workflow 检查却要求 BGFX 调试文本 `[x] sRGB Back Buffer` 和 `BGFX Shutdown complete.`；上传日志实际只有 Host 诊断，没有第三方调试文本。该失败不是 sRGB 功能已被证实损坏。

Host 现在从中立 IRenderDevice.primaryPresentationEncodesSrgb 记录实际能力；workflow 继续要求该值为 True、实际后端匹配、完成三帧、无 teardown failure 和进程成功退出。EditorApplication 的 using 在返回前完成 Shell/设备释放，任何释放异常仍使运行失败。删除对第三方调试文字的强制依赖，保留可见 BGFX FATAL 检查；未引入具体后端类型或编译宏到 Hosting。最新提交仍须以自己的完整 CI 结果证明通过。

当前 aaeb57ca 的 Windows CoreCLR/NativeAOT、Web 解释执行/AOT 已重新发布。Windows 两种产品各运行两次、每次360帧，storage 连续递增、音频 DSP 时钟推进、stderr 为空、退出码0。Web 解释执行/AOT各两次headless运行通过：storage 0→1→2、Space回调、180帧周期探针、冻结期间停止推进并恢复、JS errors=0。实际日志在本轮 results/current-web-runtime.json；SwiftShader及机器并行负载下的帧耗时不用于GPU性能结论。验收驱动首次把 `.build-logs` 误算为产品目录，已按实际 index.html 选择；RAF 计数读取会扰动冻结测量，改为观察真实 managed gameplay 帧日志，在冻结期间不执行页面代码。失败日志全部保留，不能将失败尝试当成通过。

按用户最新要求，BGCS 新包切换前暂停新增第三方库与 Binding 专项矩阵；重点为引擎功能、稳定性、性能、架构、低耦合与扩展边界。当前仓库的完整 CI 不以此减项。GUI、焦点、视觉与声学不执行。最新正式缓存清理回收9,157,611,520实际字节，清理后75,901,960,192字节可用；200GB仍未达到，37个历史拒绝目录没有被清理或补登记。

Host单项目编译0错误0警告，源码/项目架构72/72通过。两个本机源码构建命令并发写入相同obj曾触发CS2012/MSB3491；保留失败日志后改为串行复验，不把此执行冲突描述为功能回归通过。

## Native 发布目标路径的显式声明

aaeb57ca 的 macOS job116225097629 共同套件仍有两个桌面模板的发布布局失败，NormalizeDirectory 修复不足；不能把 Windows 的两项通过视为 macOS 通过。两份模板现为 Native Content 明确声明 TargetPath 等于所选布局的相对路径，不让 SDK 从物理目录或别名推断目标位置。输出校验仍检查同一精确路径，失败消息增加实际 publish root；fixture 在失败时记录真实输出文件清单，再保留原始 SDK 异常，不放宽 Native 闭包或只检查 basename。

Windows 用实际两份模板再次发布、检查 bytes、无末尾分隔符验证及缺失输入拒绝，2/2通过（explicit-native-target-path.trx）。macOS 必须以新的 runner 结果确认；尚未通过时不宣称此次修复已在 macOS 实测。

## SDK Content 元数据的求值根因

c9efaa3c 的 Windows PR CI 全部通过：2,055 passed、18个环境Skip、0 failed，Support Pack及Direct3D11/sRGB/三帧/退出smoke通过。macOS job116261597072的实际输出诊断证明Native文件被复制到发布根，而非声明的相对布局，仍有两项失败。

本机直接执行SDK `-getItem:Content`，确认先前使用另一item类型限定的 `%(_PlayerNativeRuntime.Identity)` 在新建Content元数据时求值为空；Windows默认路径推导掩盖了这个错误。改为当前item的 `%(Identity)` 后，SDK返回完整且非空的TargetPath。回归在发布前使用公开SDK求值入口断言TargetPath与实际相对布局一致，再保留原发布、内容、无分隔符及缺失输入断言；不修改原生文件布局或靠basename回退。

同一head的push macOS job116261573608另有Roslyn内部MissingMethodException（CSharpSyntaxNodeCache.GetNodeFlags），PR macOS在同一编译阶段已通过。其运行时根因未确认，不能把70行重复堆栈当作70个源码错误；未通过JIT、analyzer或弱化哈希规避，也未以失败run冒充通过。
