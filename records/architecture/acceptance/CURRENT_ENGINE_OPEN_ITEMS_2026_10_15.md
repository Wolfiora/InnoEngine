# 当前引擎真实问题、条件性风险与未确认项

[当前问题](../../../docs/architecture/CURRENT_ISSUES.md) · [当前验收](../../../docs/architecture/CURRENT_ACCEPTANCE.md) · [记录索引](../README.md)

## 基线、范围与结论

2026-10-15（台北），生产源码 `2f3fbc487520bc9eee28a35b588fd0f952a8cd8d`，kernel。本次只审查源码/实际证据、补充中立 Rendering 公开边界测量和文档标注；没有修改生产源码或 tests，没有提交，没有使用 Computer Use，没有修改 BGCS/extern/生成绑定。

审查覆盖当前五项修复及消费者、输入/读取保护/构建、平台与后端组合、Reload/PlayMode scope、公开 API/Wiki、现有全仓架构门禁与当前 CI。不是逐行形式化证明，不保证所有未知设备或未来工作流没有缺陷。

- **确认问题**有源码或实际测量/失败证据。
- **条件性风险**有具体代码假设，但条件未有效复现，不能写为已发生缺陷。
- **未确认/未执行**表示证据缺口，不推定实现错误。

整体职责边界成立，未发现需要推翻整体架构的依据，也没有本次新确认的核心生产 P0/P1 架构缺陷。但当前 PR 未全绿，存在以下真实问题和限制。P2 表示功能、性能或验证问题，P3 表示语义文档/维护问题；是否阻断合并单独说明。

## 一、已确认且仍开放

### O01 · P2：macOS inline Metal surface 超时，阻断合并

当前 PR run38699587876/job116304252956 与 push run38699580441/job116304230468，均在 `BgfxSurfaceRetirementTests.InlineSurfaceUsesTheSameConservativeAcknowledgment` 的子进程超过90秒。位置：[用例](../../../backends/Bgfx/tests/Inno.Adapter.Rendering.Bgfx.Tests/BgfxSurfaceRetirementTests.cs#L23)、[宿主预算](../../../backends/Bgfx/tests/Inno.Adapter.Rendering.Bgfx.Tests/NativeWindowTestHost.cs#L34)。

能确认该配置/用例不能完成；尚不清楚阻塞发生于初始化、drawable、frame、退休还是最终释放。不能认定根因在BGCS，也不能推断所有Metal产品均不可用。桌面默认不强制单线程，但这一配置事实不能替代没有完成的产品smoke。

关闭条件：取得超时前输出与线程/阶段证据，修正真实owner/调用/能力边界，在支持的真实配置执行原断言，并完成后续未到达阶段。不得删断言、盲增超时或按OS直接return。该第三方后端专项按用户要求暂缓，失败继续保留。

证据：`artifacts/acceptance/local-boundary-refinement/20261014-implementation/results/current-macos-tests.json`与当前contract zip。

### O02 · P2：热 Build 完整校验及准备成本仍高

revision `c9efaa3c` 的三次热Editor Build：465.297 / 575.609 / 777.781秒。Native compiler、binding generation、完整Task重编译、materialization均为0；仍有一个managed准备/检查进程，输入哈希每次读取5,227,563,904字节。

位置：[热命中](../../../build/toolchains/Inno.Build.Toolchains/Native/ProductNativeCacheLookup.cs)、[读取缓存](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeInputReadCache.cs)、[阶段统计](../../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L118)。证据：本轮 `results/current-main-performance.json`、`current-machine-load.json`。

同期CPU79–100%、可用物理内存约0.67–0.83GB。完整哈希/SDK求值的成本存在，具体耗时归因尚未完成。不能说还在反复编译，也不能把全部耗时归于其他进程。旧138.343秒中位数属于另一组环境。

相同Pack第二次请求Windows31.547秒、Browser40.328秒，确认复用相同内容身份；该等待不证明缓存失效，应纳入同一阶段剖析。

关闭条件：固定源码/SDK，在空闲机器记录求值、输入/输出读取、锁等待与进程时间；仅压缩真实不必要输入及重复工作，保留内容哈希、锁后fresh、损坏拒绝和原子发布，再以同环境数据证明改善。

### O03 · P2：并行命令录制逐 Draw 分配闭包

[CompiledRenderGraph.RecordParallelPasses](../../../src/services/rendering/Inno.Rendering/Graph/CompiledRenderGraph.cs#L482)使用[RecordedRenderCommandEncoder](../../../src/services/rendering/Inno.Rendering/Device/RecordedRenderCommandEncoder.cs#L8)逐命令保存捕获参数的Action；uniform和transform还分别ToArray，之后在owner线程依序Replay。

本次用公开RenderGraphBuilder/CompiledRenderGraph/IRenderGraphBackend，一个side-effect raster pass，不创建Native device。核心程序集是当前源码Debug构建，驱动Release；48次预热、32次测量，GC.GetTotalAllocatedBytes(true)覆盖全部线程，逐项校验实际Draw回放次数。

| Draw/帧 | 串行 bytes/帧 | opt-in并行 bytes/帧 |
| ---: | ---: | ---: |
| 0 | 1601.25 | 3201.25 |
| 100 | 1601.25 | 14161.25 |
| 1000 | 1601.25 | 107769.25 |
| 10000 | 1601.25 | 1145874.25 |

已证明选择并行录制后分配随命令增长；不是GPU测量，不代表所有场景串行更快或当前Sample已经掉帧。单个简单pass也无法体现复杂CPU工作并行化的收益。此前32-contributor前缀验证优化与此为不同路径。

关闭条件：在中立Rendering内部采用具有明确frame所有权的命令存储/数据区，减少逐命令独立闭包和小数组；保留输入快照、并行写隔离、回放顺序、异常清理。不能直接保留调用者Span或把Native借用内存暴露到公共层。先对照分配，再测真实工作负载CPU与尾延迟。

证据根：`artifacts/acceptance/framework-open-items/20261015-review/`，包含 `parallel-recording-allocations.json`、`parallel-recording-environment.json`、构建日志与完整探针源码。普通SDK构建0错误0警告，没有关闭生产/CI门禁，也没有引用第三方或生成Binding。

### O04 · P2：超时诊断不足；测试进程执行实现重复

BGFX tests、SDL tests、Editor.ImGui tests各自的NativeWindowTestHost，在超时分支Kill/Wait后仅抛出contract名，未交付已启动读取的stdout/stderr，也没有初始化/帧/退休/Dispose阶段记录或线程证据。原始90秒预算失败因此无法定位阻塞点。

各程序集自己的Main和明确用例表有合理职责；重复的是子进程启动、预算、退出和结果交接。这不是生产层平台依赖错误。

关闭条件：在测试支持边界复用进程机制，保留各自入口；超时提供输出/阶段，必要时采集所属子进程的平台诊断；故意卡住的fixture能定位阶段且没有进程泄漏，原失败仍非零。不得为测试扩大生产API、加IVT或使用私有反射。

### O05 · P2：单一隔离失败截断后续独立验收

[Invoke-IsolatedTests.ps1](../../../tests/Invoke-IsolatedTests.ps1#L58)在第一个独立进程用例失败后throw；workflow随后跳过Pack和renderer smoke。因此macOS的2068 passed、1 failed不等于完整隔离矩阵全部执行。

这是证据覆盖不足，未证明共同业务依赖BGFX测试实现。独立用例可记录失败并继续其他独立进程，最后整体失败；产品阶段应明确依赖必要构建成功和有效产物，不能无条件强跑或忽略上游失败。

关闭条件：注入一个隔离失败后，后面的独立用例仍有结果、整体CI仍失败；构建失败时不运行无效产物。本次仅标注，不修改测试或workflow。

### O06 · P3：部分 XML/Wiki 语义说明模板化

公开[GraphicsCapabilities.Supports](../../../src/services/rendering/Inno.Rendering/Device/GraphicsCapabilities.cs#L396)的值类型参数写成“consumed by supports; ownership remains ...”，SupportsRenderTarget有类似表述，对应Wiki传播同样文字；另有泛化的“prepared operation”“applies the resulting invariants”。

本次模式检索命中988行，包含internal override和可能上下文充分的条目，**不是988个独立缺陷**。需要逐项按真实含义复核参数、单位、有效范围、generation、线程、所有权、重复调用和可观察失败；简单值查询不必虚构复杂生命周期。

符号/签名/示例门禁证明存在和一致，不证明每段行为解释充分。关闭条件是优先复核关键生命周期和交接API，同步当前XML、唯一项目页和可编译工作流示例；不能再机械换成另一种模板，也不需要因此重命名或重新分层整个引擎。

## 二、未确认与未完成验收

### U01 · 新版 BGCS NuGet 与生成实现身份

当前[唯一版本入口](../../../build/msbuild/Inno.Packages.props)固定2.1.0，没有dependencies源码fallback。用户正在独立重构BGCS，本轮没有核对未来包是否发布或切换版本。

后续应核对精确包版本/content hash、实际加载的生成程序集和Native解析资产、工具宿主RID、extension的assembly identity。当前NuGet图通过不证明未来生成语义正确；改版本号也不证明实际消费闭包一致。

### U02 · 七组件的新包生成与失败路径矩阵

| 组件owner | WindowsX64 / MacOSArm64 | BrowserWasm32 |
| --- | --- | --- |
| SDL3 | C API生成/exports/实际调用 | Player静态路径与回调 |
| BGFX | C API生成/exports/surface调用 | Player静态路径/WebGL调用 |
| MiniAudio | facade/bridge/managed实际调用 | Player静态路径与音频调用 |
| Text | C++ facade→C bridge→managed | Player静态路径 |
| RmlUi | C++ facade→C bridge→managed | Player静态路径 |
| ImGui | C API及真实BindingExtension | Editor-only，不要求不存在的Browser Editor |
| ImGuizmo | C API及managed消费 | Editor-only，不要求不存在的Browser Editor |

当前包若干路径已有生成/编译/真实调用证据，不重新描述为从未运行。缺的是**新包下完整矩阵**：HostSource、TargetArtifacts、选中config/include/bridge、CheckOnly、单一Bindings.cs、extension、ABI/结构布局、string/array/ownership、callback/unmanaged入口、exports、Native/managed身份，以及同长度mtime、损坏、并发、取消、失败保留last-good。必须通过公开边界验证，不能手改生成绑定。

### U03 · 四条最终产品运行未覆盖最终组合

最近完整CoreCLR、NativeAOT、Web解释执行、Web AOT发布/运行证据属于aaeb57ca。后续桌面模板修改有当前2f3fbc48的SDK求值/真实发布fixture，Windows当前完整CI、Pack、Editor smoke通过；四条实际游戏产品尚未在最终组合上完整重做。

未来BGCS切换后还需重做静态注册、最终链接、Native调用、JS回调、存储、暂停/恢复及退出。当前旧包短运行成功不意味着产品有缺陷，缺的是最终组合的新证据；不能以文件生成或启动成功替代。

### U04 · macOS 产品和设备完成度

真实arm64 runner已编译Native Debug/Release、SourceDebug并执行架构/共同测试，不能笼统称全部未实测。未完成的是O01后续隔离矩阵、Pack和实际Editor smoke、游戏CoreCLR/AOT、音频/存储/输入、多surface及用户机器实测。

区分大小写/不区分大小写的卷也没有完整对照。当前不支持Linux；WindowsX86、其他CPU、iOS/NS属于未实现目标，不记为现有功能缺陷。

### U05 · 原用户 GUI 问题没有修改后的实测结论

viewport拖动顿挫曾有用户报告，后续源码/契约改变未配套真实拖动计时与视觉复验，不能写为已解决。尚未实际操作复验还包括live-resize/DPI、浮动GameView焦点/失焦、Popup/Modal、Console标题、Play中Reload菜单、项目选择、Export结束关闭、Workspace退出保存、昼夜光照视觉和音频听感。

部分契约已通过，不推定GUI一定有问题；按用户要求不执行Computer Use、视觉/听音。DSP推进不等于听感，SwiftShader headless不等于真实显卡/显示器。

### U06 · 真实游戏负载性能与长期稳定性

RenderGraph fixture证明各自CPU/分配路径；后台360帧、Editor120帧和Web生命周期探针证明相应短运行。没有当前最终组合的真实GPU瓶颈/帧时间尾部、复杂大量资产、多小时反复Play/Stop/Reload、多个viewport资源增长和进程内存趋势证据。

应先受控测量后处理实际热点/泄漏，不为“现代化”提前添框架。Full GC/finalizers/Full GC与Faulted契约已有结果，长测缺失不否认这些结果。

### U07 · 条件性风险：Windows大小写敏感目录的输入去重

[NativeInputReadCache.pathComparer](../../../build/toolchains/Inno.Build.Toolchains/Native/NativeInputReadCache.cs#L32)仍按OS选择Windows忽略大小写，用于物理路径/hash/清单缓存；NativeInputSnapshot保留逻辑名。如果实际目录支持两个仅大小写不同的文件，需要证明不会合并不同读取对象。Core.IO严格ownership的修复没有自动修改这条输入路径。

仅在本次新建的独立探针目录尝试启用属性；fsutil输出0x00000005 Access denied，即使返回0也不视为成功，最后只有一个物理文件。本次**没有有效复现**“两个文件的指纹遗漏变化”。两份JSON均标为无效探针，不能据此宣称已复现或已验证正确；没有提权重试或修改现有目录。

关闭条件：在支持该语义的真实环境，通过公开NativeBuildFingerprint分别修改两文件，验证指纹、目录枚举与物理身份一致性。这是具体假设的待确认项，不是默认NTFS已确认损坏缓存。

### U08 · 历史 SDK 内部异常根因仍未知

历史runner曾出现HashAlgorithmNames.HashAlgorithmToPal、Roslyn CSharpSyntaxNodeCache.GetNodeFlags缺失；同revision重跑或当前head已通过相关阶段，但没有确定根因。不能标为已根治、当前业务代码必错或“关闭JIT即可”。

若再现，保留真实dotnet host/SDK/runtime/assembly路径与内容、环境选择及崩溃诊断，区分SDK/runner与引擎调用。不能弱化哈希、关闭analyzer或任意更改JIT掩盖；本次没有据此升级框架。

## 三、磁盘与本次修正的文档事实

### D01 · 200 GB目标未完成

2026-10-15 18:09台北只读采样73,517,252,608字节可用。37个此前自动审批拒绝目录保留，不能通过认领/补owner/删父目录/换工具绕过；约55.1GB为此前逻辑大小，不保证是实际可释放卷空间。用户、NuGet、SDK、系统缓存不在本轮清理范围。

这是未完成运维目标，不据此认定owner/lease算法错误。有效产物、活动reader、保护根和未知历史归属影响回收；后续核对允许范围内的归属与实际物理占用，不能承诺仅清引擎缓存就恢复200GB。

本次新探针的bin/obj清理也被自动审批返回blocked by policy，工具未给出更细原因。未执行删除、未换工具重试，保留约2,230,221字节逻辑中间产物以及全部源码/证据；单独记录于本次 `probe-cleanup-rejection.json`。它们与37个历史目录分别记录，不宣称已经回收。

### D02 · 旧当前验收标注本次已修正

此前CURRENT_ISSUES仍突出旧138秒中位数、过期空间值，macOS状态未准确区分runner实测与最终产品。本次同步重写CURRENT_ISSUES/CURRENT_ACCEPTANCE，明确head、测量revision与未执行范围。旧测量保留在历史记录，不再覆盖当前结论。

这只关闭文档事实滞后的标注问题，不代表O02/O06及其他问题已修复。

## 四、不应为了找问题而重构的部分

- 共同领域/Runtime/Hosting消费中立契约；backend、平台基础、integration、产品入口、发行组合的方向清楚。现有项目/公开边界架构门禁123项通过，未发现需要推翻整体结构的证据。
- Default选择标准音频/UI等属于默认组合职责；自定义platform/rendering factory由产品注入。业务领域不需要引用Default/BGFX。
- Reload guard在Core，Scripting不引用PlayMode；运行Session持generation读取保护，Scene事务进入所属Session scope。当前契约结果与GUI待实测分别记录。
- Native内容哈希、锁后fresh、原子发布、exports及完整输出验证有实际目的，不因成本高就删除。
- 明确target/ABI、host SDK、产品与部署方式分别描述。新设备需要真实SDK适配是正常工作，不是自动证明契约不优雅。backend里未注册ABI配置也不等同重新支持Linux或legacy reader。
- 230个项目、两个入口、多份小契约和稳定职责命名本身不是缺陷。当前门禁没有游离项目/空Solution Folder失败，不为“新”统一改名Backend。
- 未恢复dependencies、旧namespace wrapper、fallback reader、schema兼容或第二套Identity/Events/History系统。O03局部实现成本不证明中立Rendering公共契约错误。

## 五、处理顺序与交付范围

1. 当前可独立处理：O02空闲基线与归因、O03命令存储、O04/O05诊断与覆盖调度、O06语义文档；保留当前公开行为及失败边界。
2. 新BGCS包可用后：U01/U02，再做U03及受影响O01/U04。若取得独立证据证明O01与新包无关，在真实owner解决，不无限归责依赖。
3. 设备/环境可用且用户允许时执行U05/U06/U07；U08保留再现诊断。
4. 合并master前解决当前远端失败、执行后续未到达门禁，并将证据绑定最终head；GUI按用户选择单列，不能制造“全部通过”。

本次仅文档和本地审查证据，无生产API变化、无tests修改、无提交。探针只使用公开契约，无IVT/私有反射/测试后门。指定Glass.aiff在Windows不存在，提示音未播放。
