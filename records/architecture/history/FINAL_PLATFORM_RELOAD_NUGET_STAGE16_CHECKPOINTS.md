# 最终平台、Reload 与 NuGet 收口验收

基准：`a82fb6b9ed50405029ea653c1d5283854ef2ca0e`，分支 `kernel`，执行日期 2026-10-13。

本记录持续更新。只有实际完成的检查写为通过；本轮没有自动提交。

## 最新检查点（2026-10-13，阶段 14，接续执行）

- 阶段 13 的 Browser Native 与 managed 编译完成，但 fresh preparation 验证拒绝了已变化的输入；保留失败日志，未选择该候选。独占、固定源码复验成功：251.216 秒，发布 `c860cd8f2016af2ae89d873f3f13a89ee420b47796409c497975f94f968c6ff0`。这一复验支持并发输入变化触发保护，但没有声称已定位到某一个被改变文件。
- 第二次相同 Pack 请求 158.752 秒，返回同一 Pack；Player managed Build、Native compiler、binding generation 日志计数均为 0。该请求包含 CLI bootstrap 一次 21.04 秒编译，不能将总时间描述为纯 Pack 检索。
- SDK Python bytecode 已作为派生缓存排除；实际 Python 源码、LLVM bin、Python runtime 则显式纳入输入。CMake 将 Python bytecode 写入受 SDK 身份隔离的 owned workspace，未清理 SDK 全局缓存。相关 input/workspace/browser 20 项、架构 122 项及 SourceDebug 全量再次通过。
- Wasm AOT 实际日志确认 88 个程序集完成 AOT。再次浏览器运行，持久化计数 2→3→4，输入后 DSP 时钟从 0.1707 增至 15.3173；实际渲染、夜景及 error/warn 检查保留。
- 官方 NuGet flat-container 索引重新核查 BGCS、Runtime、Cpp2C、Core、CppAst、Intermediate，最新稳定版均为 2.1.0。
- 当前正在准备最新 Windows Debug 部署；随后继续 Windows Player 存储/DSP、消费者、完整 GUI 与性能门禁。暂未声明最终本机收口。

证据：`stage13-*`、`stage14-*` 文件。当前卷可用 47,773,913,088 字节（18:10:42）。

## 历史检查点（2026-10-13，阶段 12）

最终本机收口仍在执行；下面列出中断后新证据，后文日期化检查点是历史过程。

- SourceDebug 全量 48.95 秒、SourceRelease 全量 61.18 秒通过，均为 0 warnings/errors。随后只修改 CLI 使用提示与 CI 锁文件路径匹配，最终源码门禁还会再运行。
- 完整架构 122 项；最终 Wiki 2 项通过，45,548 个公开/可扩展符号、189 段示例无编译错误。Wiki 总项目页覆盖由完整有效图门禁检查；有 API 的 149 个项目不能当作全项目数量。
- 最新 PlayMode/Hosting 59、Console 原生布局 4、ImGui 窗口/退休 40、Cache/Pack/Task 40、NuGet/recipe/tools 19、publication/framework 6 项通过；实际 GameView 聚焦的 Ctrl+P 路由回归通过。
- Browser Pack 完整准备通过，1195.88 秒。解释执行最终发布通过，676.85 秒包含等待另一冷构建与构建应用准备，不是单独发布性能基准。网页实际启动、PLAY 输入、夜间星光、无 error/warn、localStorage 跨重载计数 0→1→2 已验证。音频初始化 Ready；不能据此宣称声学听音通过。
- Windows Release Editor 冷准备总计 1688.80 秒，产品 SDK 构建 23:44.70；Native 711.52 秒、14 个进程、2 次 binding generation、26,113,066,857 哈希读取字节。它包含新的 Release 原生闭包准备，不能当成热构建结果。
- Release 首次准备后启动约 10.15 秒（进程记录到首帧），其中 Host 创建到首帧 7.55 秒；没有 Native/BGCS/Pack 准备。现场验证 Playing 禁用两项 Reload、20 个脚本 DLL 不变、Ctrl+P 停止后处理最新变化、GC 卸载完成、Console 等级标题及 Kind 详情、候选编译失败保留 last-good、修正后清除诊断、Editing Plugin Reload 恢复。
- Release 真实附加 GameView 创建、移动、缩放、关闭、再次打开成功；最终退出时保留附加窗口测试 drain。16:08:54.480 保存 Workspace，16:08:56.069 完成 Dispose，未强制终止。
- 磁盘正式 owner 退休逻辑回收 32,044,021,138 字节，实际卷增加 28,498,821,120 字节。先前手工删除 Bootstrap 残留的自动审批拒绝后没有绕过；改用正式 owner 清理成功。用户、SDK、NuGet 缓存未清理。
- Windows CoreCLR/NativeAOT 的先前真实发布、玩法与正常退出保留；最新存储/DSP 探针发布、Web AOT、Debug 新部署 GUI、消费者、热构建/三次启动、并发及最终映射仍须闭合。

原始证据：`artifacts/acceptance/final-platform-reload-nuget/20261013-final-closure/` 中 `stage12-*` 日志、TRX、JSON 和截图。验收副本的存储/DSP 探针及预期 `#error` 不属于引擎或原始 Samples；预期错误已撤销，全部探针将在验收结束后恢复。

## 用户问题与执行范围

处理 Play 中 Reload 准入、Edit/Play Identity 作用域、Console 标题、WindowsX64/MacOSArm64/BrowserWasm32 命名、Linux 删除、BGCS 全 NuGet 化，以及目标缓存清理、Pack 后端边界、重复工具准备、Task 读取交接、Wiki 和完整本机矩阵。

日常启动仅保留 InnoEngine.Editor 和 InnoEngine.Build。平台库在默认 Solution 中未被选择构建，不代表已准备产品不能启动。最终报告将区分阶段提交、最终本机收口和 macOS 实机结果。

## 当前结果（持续复验中）

| 项目 | 当前状态 | 新证据 |
| --- | --- | --- |
| Reload/Play | 专项通过，完整 GUI 继续 | PlayMode 15、owner/rollback 2、真实 reload 4、watcher 最新请求 1 |
| Console | 实际窗口确认等级标题与 Kind 详情 | captures/console-header-and-tools-failure.png；完整布局继续 |
| BGCS / 平台 / Linux | 源码迁移完成，产品继续验证 | 229 锁文件、128 BGCS 资产均为 2.1.0，有 content hash；两个旧物理目录已移出仓库 |
| cache / Pack / Task | 专项通过，完整矩阵继续 | cache/task/platform 62、Pack ABI 14；旧登记已按 owner 协调失效 |
| Solution / 架构 | 最新 SourceDebug 62.15 秒、0 warnings/errors；Bootstrap 读取交接继续复验 | 架构 61、工具/发布/owner 12；实际 Solution graph 230/230 |
| Wiki | 当前 45,544 条 API 映射、189 段示例无错误，完整门禁待固定源码复验 | 最新完整运行仅发现新增 lease 文件的 XML 注释；已补齐，未提前计为整体验收通过 |
| Windows Editor Debug | 首次构建和启动完成；GUI 发现并修复工具配置问题 | stage8-editor-debug-build.log：52m46.71s，包含冷编译与重复工具准备；不能作为热构建结果 |
| Windows/Web 最终矩阵 | 未完成，继续执行 | Release、Player、Wasm、视觉、性能和清理尚需新证据 |
| MacOSArm64 | 托管源码检查通过，Native/产品未实测 | Editor 61.54 秒、Player 35.25 秒；未复制 Windows DLL 代替目标引用 |

实际 GUI 发现 `shaderc-debug` 与统一 Release 工具不一致。工具文件名现改为配置无关的逻辑文件名，配置进入所选步骤及指纹；Shader 与纹理编译复用同一工具输入。还删除 Shader include 向仓库目录回退的路径。修改后正在重新准备产品，先前视觉错误不计通过。

重复工具 workspace 的实际差异为同一 `executionRoot` 带或不带末尾斜杠。两个 recipe 的全部 15,936 个逻辑输入哈希相同；已在 NativeBuildContext 规范化路径，保留 checkout 隔离。身份回归 1 项通过，实际产品流程继续验证。

完整 Identity 11、generation 退休 22、Scene 51、Runtime 61、Hosting 回归通过。PlayMode 完整运行 46 项通过、10 项因缺少显式 Native 测试部署失败；等待本次 Native 闭包部署后重跑，未计全通过。

### 新一轮运行发现与复验

- 修正目标根路径后的 Debug 产品构建成功：实际 Native 工具只进入规范化 workspace，发布工具采用 `shaderc.exe` 等准确逻辑名。总计 2981.57 秒；此运行同时包含源码重新准备和并行 Wiki 检查，不能作为隔离性能结果。
- 无参数 Editor 启动已出现项目选择窗口；选定项目后 7.807 秒完成首帧，场景、纹理、光照和浮动 GameView 呈现正常。Play 中 Reload Scripts/Plugins 均禁用，点击未触发重载；浮动 GameView 的 N 键成功切换夜景。
- Play 中修改源文件未生成新程序集，但错误显示了阻塞编译 Modal，挡住停止按钮与 Ctrl+P。失败截图与程序集时间证据已保留。已修复为非阻塞等待 Info，专项先失败再通过；实际 GUI 正在重新准备复验。原测试源文件已逐字节恢复。
- 显式 Native 清单复制前后全部哈希一致。重新运行 PlayMode 56、ImGui 41、SDL 10 项全部通过。
- 新 `DirectoryCleanupException` 区分提交后的 backup 清理失败。Core.IO 19 项通过；真实游戏输出 backup 只读失败及最终 progress observer 异常两项通过，已提交输出仍成功并报告 Warning。
- 全量 SourceDebug 运行发现一个新增局部变量冲突，已更名且目标编译通过；该失败的完整运行没有计为通过，最终仍须重新运行门禁。
- 系统 SDK 缺少 Wasm workload，安装需要提权。本轮改用仓库内独立 9.0.318 SDK，wasm-tools / Emscripten 3.1.56 包 9.0.20 安装成功；没有升级项目目标框架或改变系统 SDK 路径。CLI 支持显式 `--dotnet` 并把该选择传入完整发布链。

并发源码构建发现 CLI 从可写 bin 载入导致 DLL 占用。Build 工具现使用独占输出和受保护不可变 publication，读取交接及实际 SDK 输出可写测试通过；进一步复验从固定输入编译工具自身。

正常 GUI 退出已记录 Workspace 保存在退出清理之前，Dispose 于 2026-10-13 04:30:45 完成；实际 Editing 下 Plugin Reload 未出现截图所示 Identity release 错误。Playing 禁用、停止后重载和视觉项目继续验收。

## 原始证据

`artifacts/acceptance/final-platform-reload-nuget/20261013-final-closure/` 保存命令日志、TRX、基线与后续结果。

macOS 实机需要对应环境；未执行时保持未实测。Windows 中不存在指定 Glass.aiff，提示音无法播放。

## 本轮续跑结果（最终收口仍在进行）

- `dependencies` 与 `platforms/Linux` 的物理目录通过 Windows Explorer 移入回收站；Git 子模块、平台项目与发行注册已移除。未清空回收站，不能将此操作计为卷空间已回收。
- BGCS 包族固定 2.1.0，版本查询与 NuGet 锁文件证据位于本轮 baseline/results。绑定库及 CLI 真实编译通过，最新 CLI 命令记录 0 warnings / 0 errors、32.81 秒；实际生成与完整产品运行尚待验证。
- Reload/PlayMode 专项 15 项、Scene Plugin Missing/recovery 1 项通过；自动变更暂存、真实 Plugin reload 与 GUI 验收继续执行。
- 缓存、Task 与平台组合专项 62 项通过；架构专项 61 项中 60 项通过，仓库门禁指出注释排版和缺少迁移后程序集，正在重新构建。
- Pack/Pipeline 测试发现旧文件名 fixture 与缺少显式 Native 测试部署。修正后 38 项中 34 项通过，3 项错误消息预期和 1 项 Native 前置条件继续修复；未记录为全通过。
- 新增平台二进制 ABI 检查，WindowsX64 验证 PE32+/AMD64 DLL，MacOSArm64 验证 Mach-O ARM64 dylib；BrowserWasm32 验证归档成员为 Wasm 对象。静态闭包同时拒绝已选组件目录内的多余归档。
- 全量 SourceDebug 首次运行与第一次重跑因新增 XML 排版问题中止；问题已修正，当前执行 `stage7-source-debug-final.log`。中止运行不计通过。

证据根：`artifacts/acceptance/final-platform-reload-nuget/20261013-final-closure/`。

## 2026-10-13 中断后复验

最终本机收口仍在执行，尚不能作为最终提交验收。

- `stage9-source-debug.log`：SourceDebug 36.21 秒，0 warnings/errors；后续锁文件与呈现 scope 修改另行重建。
- `stage9-wiki.trx`：完整 Wiki 两项通过，189 段示例通过实际逻辑引用编译。后续文档修改须再次核查。
- `stage9-architecture-final.trx`：完整架构 122 项通过。后续 SceneView 实现依赖需要再次检查。
- BGFX 真实产品部署的 Device 19、surface 退休 3、配置/Shader 21 项通过。普通 SDK 产品构建及部署流程另通过 21 项。首轮中立 Adapter 缺 Shader 与同进程单线程重初始化失败保留，不计通过。
- 重复 Build publication 在活动 reader 下申请写入锁，导致真实 30 秒超时。修复为共享读取下复用；读取验证、登记依赖及交付受 owner 连续保护。`stage9-tool-publication-fixed.trx` 两项通过。
- Player Restore 覆盖中立 NuGet 锁文件导致 NU1004。锁文件现在按目标/RID 分开；快照包含实际选中的锁文件。`stage9-lock-shader-owner-fixed.trx` 16 项通过，包含锁选择、明确平台 profile 与提交后只读 backup 清理。
- 固定托管源 workspace 的 Shader Task 曾把副本当成另一个工具源码仓库。实际 recipe 对照保存在 `stage9-frozen-shader-tool-identity-difference.json`。已显式传入原始离线工具 owner，Shader 内容继续消费固定副本；实际 Pack 复验继续。
- 并发 SDK 求值改变 Build publication 规划输入时，本次候选被稳定性检查拒绝，没有接受错误产物。重新准备和性能测量必须在源码固定且没有其他 SDK 写入时执行。
- 新 GUI 在 Play 中确认昼夜切换、动态管道和星光，但暴露视口 UI 使用 Edit 服务而反复重建。截图 `stage9-play-ui-owner-mismatch.png` 保留。两个视口现限定使用 Play 呈现 scope，整体 Editor 更新仍在 Edit scope；正在重建，不能将修复前玩法算通过。
- Editor 正常退出在 11:34:57 保存 Workspace，11:34:59 完成 Dispose，没有强制终止。
## Computer Use 停止检查点

用户以物理 Escape 停止 Computer Use，停止继续交互验收。本轮最终本机收口尚未完成。

- Windows Pack 独立准备成功，699.98 秒；同一请求复用成功，93.73 秒。命中日志无 Native/BGCS/Player 编译输出。
- Pack References 共 69 个 DLL，只包含 BGCS.Runtime，不包含 Editor/Build/BGCS generator/ImGui/ImGuizmo。
- Windows CoreCLR FlappyBird 发布成功。已尝试启动真实 Player，但 Computer Use 在首次观察前被停止，不能记为运行或玩法通过。
- Windows NativeAOT 发布已发起，日志 `stage9-windows-nativeaot-publish.log`；本检查点未确认最终结果。
- 后续仍需：NativeAOT/Web、修复后 Editor Debug/Release GUI、消费者、热构建/启动、并发、清理及最终 Wiki/架构门禁。
## 2026-10-13 本次续跑检查点

用户以物理 Escape 停止 Computer Use，本次立即停止后续交互。最终本机收口仍未完成。

- 最新 SourceDebug 两次编译通过：39.06 秒与 25.62 秒，均为 0 warnings / 0 errors。
- Emscripten 预检改为求值实际 WebAssembly Player 发布模板；明确所选 SDK 的四项 Emscripten 路径已取得，Browser toolchain 契约 6 项通过。宿主 CMake/Ninja 已安装，通过本次进程 PATH 显式选择，没有修改系统环境。
- 最新架构 93 项及 Wiki 规则 29 项通过；Native 输入/阶段校验/workspace 回归 18 项通过；ImGui Faulted retirement 独立进程 1 项通过。
- Windows CoreCLR 发布成功，103.35 秒；真实 Player 已实际启动、点击开始、进入 Game Over、键盘/按钮操作、夜间星光与亮度切换，并正常退出。
- Windows NativeAOT 发布成功，86.18 秒；真实 AOT Player 已实际启动、点击开始、进入 Game Over、夜间切换，并正常退出。实际听音、非零分数写入和跨启动存储仍需补证，未用成功启动替代。
- 本轮实际截图：stage10-coreclr-night-stars.png、stage10-nativeaot-ready-day.png、stage10-nativeaot-night-stars.png。
- Browser 完整 Pack 尝试在 Build 引导阶段失败，尚未进入有效发布；失败日志保留。Bootstrap 异常现在输出完整堆栈，正在运行的诊断命令为 stage10-browser-diagnostics.log；不能把此路径记为通过。
- 当前源码补入 DirectoryCleanupException 作为实际 Native recipe 公共 IO 输入；兼容工作目录仍可复用，完整输入身份继续覆盖真实行为。
- 仍需完成：Web Pack/解释执行/AOT、修复后的 Editor Debug/Release GUI、消费者、性能/并发/清理、最终 Wiki 和完整报告。

诊断命令随后确认 Web 当前阻断点为 ManagedInputGraphTask：`A framework reference has no selected SDK targeting declaration.`。SDK 查询已成功；下一次需修正 net9.0-browser 的实际 framework 输入捕获，再继续完整发布。磁盘可用约 53.89 GiB，本次未执行缓存删除。

Latest Debug GUI: Play UI preserved, keyboard gameplay, Reload disabled while Playing, watcher pending Info without compilation, then toolbar Stop and activation/GC completed. After an Editing Plugin Reload request, Computer Use could not activate/capture the monitor (0x80070057) even after window recovery. The task-owned acceptance Editor was terminated to release its checkout reader; this particular run is not normal-exit evidence. GUI closure remains pending.

### 阶段 14 GUI 停止检查点

最新 Debug 构建成功（产品 SDK 14:49.32，0 warnings/errors），准备后实际首帧于 18:28:34.371 完成，未启动 Native/BGCS/Pack 准备。实际进入 Play，两项 Reload 禁用，浮动 GameView Space 输入与音频 DSP=147.49 正常。窗口实际移动与缩放后仍有正确图像。验收副本通过公开 Update 记录每 180 帧的墙钟间隔，常见均值 7–8 ms；不能把 Computer Use 手势耗时当作渲染帧时间。

尝试从 resize 后的 GameView 执行 Ctrl+P 时，Computer Use 报告前景窗口无进程 ID，随后明确报告用户物理 Escape 停止。收到停止信号后没有再发送界面输入。此次停止/关闭未确认，不计通过；此前 Release 的正常完整停止/退出证据仍属于其对应部署。

本机最终收口尚未完成：剩余 Windows Player 探针发布/运行、消费者、Debug 正常退出、热构建/三次启动/并发、最终门禁与清理仍须接续。当前实际 Editor 可能仍运行，后续首先重新检查 owner 和进程状态，不能强制删除其使用目录。验收副本的存储/DSP/frame 探针仍在，最终需要恢复 baseline 原文件。
随后后台检查确认 Debug Editor 已退出，日志明确记录 WindowCloseEvent → Workspace 保存 → Dispose end（18:36:21.506–18:36:22.572），stderr 为空。因此存在正常关闭证据，未发现崩溃；关闭事件的实际操作者未确认，不能将它描述为 Ctrl+P 成功停止。正在执行的 GUI 验收被物理 Esc 停止，其余待验收状态保持不变。
### 阶段 16：当前验收范围与 AOT 日志来源修复

用户明确取消本轮 GUI/Computer Use 验收；后续只执行后台编译、契约测试、发布、无窗口运行及检查。取消项不写成通过。

阶段 15 的 Windows NativeAOT 发布成功，但首次实际调用 `Log.Info` 触发 `Assembly.GetCallingAssembly()` 的 PlatformNotSupportedException，记录为真实阻塞，未通过。源码现正式替换为 `Log.Info(typeof(Owner), ...)` 等显式来源参数，删除运行时调用程序集探测和第二份来源缓存；来源类型仅借用一次，entry 不保存 Type/Assembly，复用 Modules 的弱分类缓存。没有 NativeAOT 判断或旧 overload。全部测试、当前 Wiki 示例、两个实际消费者源码和验收副本同步。新增静态来源/空类型测试，Logging 41、Runtime 61 项通过；最新 SourceDebug 48.81 秒、SourceRelease 235.38 秒，均 0 warnings/errors。SourceRelease 是首次重新编译大量受影响的 Release 项目，不作为热构建结果。

三个平台 Player 组合的 343 字符初始化已展开，仅调整排版。完整发布、NativeAOT 真正调用和最终 Wiki/架构继续复验，不能用前一轮通过结果替代此次 API 改动后的证据。
最新显式来源日志代码的架构 122 项通过；实际 Log.Info 签名改动后的 NativeAOT 与 Web 发布仍在复验。

阶段 16 的实际 NativeAOT 修复复验通过：最新发布 86.780 秒；两个 360 帧、window-visible=false 的独立进程均 exit=0、stderr 空。日志来源 InnoScripting/Runtime 正确，存储 5→6→7，DSP 0.03→0.97 / 0.03→0.94。CoreCLR 两次后台运行也 exit=0、stderr 空，存储 3→4→5，DSP 0.25→2.05 / 0.25→2.26。真实 Native 调用与持久化通过，不描述成 GUI 或声学听音通过。修复后的 Wiki 完整 2 项与架构 122 项已通过。最新 Plugin 导出完成并同步到验收项目和当前 Samples。