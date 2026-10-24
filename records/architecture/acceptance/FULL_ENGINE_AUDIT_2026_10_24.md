# 四仓库核心代码、平台与构建验收报告

日期：2026-10-24。环境：Windows x64，.NET SDK 10.0.402 / runtime 10.0.13 / C# 14，当前 `dotnet10` 工作区。源码 HEAD 为 `06598ce0d07acf33b71eda58c846c3d6154ff32a`，叠加审计开始时已有修改；不是一个干净提交的发行验收。

[当前问题](../../../docs/architecture/CURRENT_ISSUES.md) · [当前验收](../../../docs/architecture/CURRENT_ACCEPTANCE.md) · [身份与重载标准](../../../docs/architecture/IDENTITY_REFERENCE_RELOAD_STANDARD.md)

## 1. 验收结论

**不能判定“以前所有问题已经干净解决”，也不能判定“整个引擎已经没有严重核心问题”。**

平台归属、共享 backend、中立构建与发行注册已经形成真实边界。本轮正式架构校验完成 **238/238 工程的 MSBuild 求值并通过**；没有发现共享领域反向引用命名平台、backend 反向依赖平台或平台互相引用的门禁违规。BGCS 使用正式 **4.1.1**，源码和已安装包未被本轮修改。

但是，运行与源码深审发现了门禁无法替代的缺口：Browser Mono 解释执行仍会崩溃；Core 协程停止没有执行迭代器清理；History 的错误顺序提交会先破坏事务栈再报错；Canvas 和 Rendering2D 存在明确热路径浪费；热构建与默认缓存维护预算也没有达到高效、可靠收敛的目标。

本轮任务是审计与验收报告，**没有修改这些生产实现，也没有把已复现缺陷写成已修复**。本报告给出修复 owner、约束与关闭条件，不能代替后续修复和复验。

## 2. 范围、方法与证据等级

### 2.1 全仓范围

| 仓库 | 扫描手写源码/工程/脚本文件 | 行数 | 工程数 |
| --- | ---: | ---: | ---: |
| InnoEngine | 2,418 | 334,881 | 238 |
| InnoEngine.Canvas | 17 | 2,540 | 1 |
| InnoEngine.Rendering2D | 67 | 18,547 | 1 |
| InnoEngine.Samples | 3 | 544 | 0 |
| 合计 | 2,505 | 356,512 | 240 |

计数包括适用的 C#、手写 C/C++、MSBuild、PowerShell、JavaScript 和测试源码；排除第三方 `extern`、生成的 `Generated`、bin/obj、Library、artifacts、Git 和 node_modules。它是扫描范围，不是“逐行人工审阅 35 万行”的声明。可编译脚本工程由正式流程派生，不按独立创作工程重复统计。

执行了全仓候选扫描、有效工程图门禁、关键执行链人工深审、现有公开契约测试，以及只使用 public API 的独立反例。重点深入了：

- 输入冻结 → SDK/Profile → 官方生成 → Native 执行 → 校验 → 原子发布 → 部署 → Task 消费。
- owner 协调 → reader/writer/等待者 → current/依赖保护 → payload/metadata 退休。
- Identity → ContentReadScope → Missing/候选 → 历史记录 → collectible ALC 退休屏障。
- Core Events / Execution / IO / Coroutines / Jobs / Serialization 与上层复用。
- RenderRuntime → 模型 → Rendering2D 收集/编码 → Canvas → GPU 持久资源服务。
- 脚本编译器、框架引用、IDE 投影、Plugin 安装源与 FlappyBird。

### 2.2 证据等级

- **实测确认**：公共边界反例或实际产品执行，有命令、日志、结果。
- **源码确认**：完整调用链可确定行为，但未做独立压力/设备测量。
- **风险/未执行**：缺少触发证据或设备，不能当作已证实缺陷，也不能标记通过。
- **历史证据**：只证明原记录对应 revision 和范围，不自动计入本轮通过数。

本地证据根为 [`artifacts/acceptance/core-audit-2026-10-24/`](../../../artifacts/acceptance/core-audit-2026-10-24/)。前序 4.1.1 消费证据为 [`bgcs411-2026-10-24/`](../../../artifacts/acceptance/bgcs411-2026-10-24/)。这些目录不属于生产输入，也不要求进入 Git；报告保留关键结论，便于证据被清理后仍能理解问题。

## 3. 以前的问题逐项复核

| 原问题 | 当前判断 | 证据及边界 |
| --- | --- | --- |
| .NET 9 / C# 旧语言设置 | 当前配置已统一 | SDK 10.0.402、latestPatch、禁用预览；普通 net10.0、Browser net10.0-browser；两个生成器的 netstandard2.0 是编译器工具目标。源码/模板扫描和架构门禁未发现当前生产路径回退 .NET 9。 |
| global / implicit using | 当前门禁通过 | 根工程与 IDE writer 显式 disable，脚本解析使用 CSharp14；解析门禁检查 global 指令、MSBuild Using 与隐式导入。负例测试中的字符串和 `global::` 限定名不等于 global using。 |
| 是否只替换到了 BGCS 4.1.0 | 当前实际为 4.1.1 | 统一声明精确包族版本；当前存在的 623 份相关锁文件、982 条 BGCS 包记录均为 4.1.1。历史验收文档和已安装旧缓存不是当前依赖。 |
| 3.0.1 aggregate ABI 阻塞 | 原阻塞已被替代并通过 Native 阶段 | 4.1.1 正式 Browser 五组件生成、实际 Clang/CMake 编译、归档和发布通过；不能据此推断所有托管执行模式都通过。 |
| 4.1.0 系统匿名 union 输出非法声明 | 原精简输入已修复 | 正式 4.1.1、不引用引擎的 pthread 复现，生成和原样 C17 编译均退出 0；完整 Browser Native 也通过。旧交接文件保持为历史证据。 |
| BGCS 是否改源码、改包、手补生成结果 | 未发现本轮这样做 | 148 个已选包文件哈希不变；实际 CLI 的 13 个 SDK DLL 与发布摘要一致；BGCS 工作区干净，HEAD 为 `7f5cb7cfb2bc4fb10b23a30117c99e511201c1e2`；递归未发现 Agent/AGENTS Markdown 文件。 |
| 组件内的平台 JSON/平台表 | 归属整改已落地 | 七组件各一份 bindgen.json，目标由 platform integration 的 Profile 贡献；中立 Toolchains 不消费 BGCS 类型。 |
| Browser 直接耦合 Windows、CLI 专用分支 | 现有边界通过 | Browser executor 只消费冻结工具与中立宿主服务；Windows workspace/junction 在 Windows host；发行注册具体贡献，CLI 调度统一接口。 |
| C/D 盘路径写死、CMake 复用错误目录 | 未发现生产源码硬编码开发机路径 | 实际路径通过 owner、SDK 与 workspace 解析；物理复制树与执行别名分开；身份进入 workspace/fingerprint。历史路径/Rider 回归见原报告，本轮没有操控 Rider，也没有重新完成跨卷/全新 clone 矩阵。 |
| Task runtime 加载/输出 DLL 锁 | 已有正式协议，当前构建有复用证据 | Task publication、Build 生命周期 registry、不可变私有加载与独立 ArtifactsPath 已存在；当前 SourceDebug 日志出现同身份 reused。本轮不把它等同于 Rider Run 实机通过。 |
| lock/current 无限累积 | 协议已整改，性能/收敛尚未完全验收 | owner 协调、current 内容相同不重写、metadata 退休都已存在；但本机 5 秒预算在 inventory 阶段失败，见 F07。不能承诺“以后绝不会膨胀”。 |
| Rendering2D Events 重复造轮子 | 该具体缺口已关闭 | SpriteAnimator2D.Listen 使用 Core EventDispatcher/EventHub；没有另一个监听器表。队列用于 flood fill 等算法不属于第二个事件系统。 |
| Canvas 强持有 owner/失效对象 | 该具体缺口已关闭 | CanvasState 通过 Identity 弱快照重解 owner；外部 UI 调用后复核生命周期。历史十项生命周期用例和安装态证据保留；本轮不扩大成所有引用均完美。 |
| 完整产品、全部测试、文档语义 | 没有全部闭合 | AOT smoke、架构与部分当前测试已取得证据；Mono 解释路径失败，完整 SourceDebug/全套验证仍需对应最终结果；Mac、Rider GUI、听感等单列。 |

BGCS 的旧生成缺陷修好了，与新的 Mono `calli` 故障同时成立。不能把后者称为“匿名 union 又坏了”，也不能把所有 Browser 问题继续归责 BGCS 核心。

## 4. 当前确认的问题与优先级

P1 表示应优先处理的产品不可用/资源生命周期问题；P2 表示有明确正确性、吞吐或维护成本影响的缺口。未发现 P0 证据不代表已经形式化证明全仓没有 P0。

| ID | 优先级 | 问题 | 证据 |
| --- | --- | --- | --- |
| F01 | P1 | Browser Mono 解释执行的 UInt64 函数指针调用中止 | 引擎复现 + 无引擎/无 BGCS 的成对对照 |
| F02 | P1 | Coroutine 停止/Dispose 不清理迭代器，异常也没有可观察出口 | public API 反例 + 实际 RuntimeSession 消费链 |
| F03 | P2 | History 非 LIFO Commit 先弹栈后校验，破坏后续回滚与 Undo | public history extension 反例 |
| F04 | P2 | Canvas 每条纹理绘制先复制完整像素，即使 GPU 缓存命中 | 调用链 + public 上传对象分配测量 |
| F05 | P2 | Rendering2D 正常模型路径固定绕过完整帧缓存，反复编码 | 正常调用链源码确认，未测最终帧耗时 |
| F06 | P2 | Native 热准备依然反复读取巨量输入，并在不同工程重复 | 当前正式 SourceDebug 的分阶段统计 |
| F07 | P2 | 5 秒维护预算无法完成当前 inventory，自动 metadata 收敛不足 | 5 秒失败、30 秒 dry-run 成功的对照 |
| F08 | P1 | Coroutine 执行中停止自己会重复删除活动项，越界或遗失其他协程 | 两种 public API 重入反例 |
| F09 | P2 | 包身份测试写死默认 obj，隔离构建误读旧 assets | 当前套件失败 + 默认/隔离/锁文件三方核对 |

### F01：当前 Mono Wasm 故障，已排除匿名类型问题

引擎精简设备入口能够创建 MiniAudioDevice，但读取 `dspTime` 时中止。调用最终进入 `EngineGetTimeInPcmFrames(MaEngine*) -> ulong`，生成 binding 使用 `delegate* unmanaged[Cdecl]`。

源码入口：[`MiniAudioDevice.cs:117`](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio/MiniAudioDevice.cs#L117)、[`BrowserWasm32BindingProfile.cs`](../../../platforms/BrowserWasm32/integrations/Inno.Integration.BrowserWasm32.Build/Bindings/BrowserWasm32BindingProfile.cs)、组件 [`bindgen.json:32`](../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Bindings/bindgen.json#L32)。

进一步建立不引用 InnoEngine/BGCS、不使用第三方头的 C/托管工程。C 函数返回 `0xfedcba9876543210`，接受一个指针；同一 C 实现、同一 SDK/workload：

| 调用方式 | 发布 | Edge 实际执行 | 生成的 LI trampoline |
| --- | --- | --- | --- |
| 函数指针，`delegate* unmanaged[Cdecl]<nint, ulong>` | 通过 | Mono aot-runtime-wasm.c 断言，失败 | 不存在 |
| 直接 DllImport 到同一 C 函数 | 通过 | UInt64 精确返回，通过 | 存在 |

证据：[`probe-runtime-results.json`](../../../artifacts/acceptance/core-audit-2026-10-24/probe-runtime-results.json)、[`interop-signatures.json`](../../../artifacts/acceptance/core-audit-2026-10-24/interop-signatures.json)、[`calli-probe/`](../../../artifacts/acceptance/core-audit-2026-10-24/calli-probe/)。这里不存在 aggregate 或匿名 union。

.NET 10.0.13 的 [ManagedToNativeGenerator](https://github.com/dotnet/runtime/blob/v10.0.13/src/tasks/WasmAppBuilder/ManagedToNativeGenerator.cs) 从 P/Invoke/icall 收集签名，再交给 [InterpToNativeGenerator](https://github.com/dotnet/runtime/blob/v10.0.13/src/tasks/WasmAppBuilder/InterpToNativeGenerator.cs) 生成查找表；找不到 cookie 时返回空。结合本机表差异和对照实验，当前证据指向解释执行的函数指针签名发现/桥接缺口。不能由此断言所有函数指针都不能用，或所有 .NET 版本都同样失败。

**正确 owner 与方向：**.NET Wasm 互操作能力，以及官方 Browser 扩展/引擎 Browser integration 对实际执行模型和 import 策略的表达。应先验证 BGCS 现有公开的静态 import/桥接能力能否保持现有 Raw API、会话保护和 ABI；如现有扩展契约不足，再在 SDK 正确扩展 owner 实现能力。中立引擎和共享 backend 不增加 `ulong` 特判，不手补某个 `LI` stub，不向无关函数添加假 import 来偶然“凑齐表”。

关闭条件：独立 calli/受支持 import 对照、SDK 正式生成、实际标量/aggregate/回调 ABI、裁剪、解释执行与 AOT 都有运行证据；两个 FlappyBird 部署均完成启动、输入、存储及退出。AOT 通过不能作为删除解释执行路径或静默回退的理由。

### F02：Core 协程资源退休不完整

[`CoroutineScheduler.cs`](../../../src/foundation/core/Inno.Core.Coroutines/CoroutineScheduler.cs) 的 Stop/StopByOwner/StopAll、删除活动项和 Dispose 清空集合，没有对仍在栈中的 IEnumerator 执行 IDisposable.Dispose；正常耗尽时也直接 Pop。`MoveNext` 的异常在 292 行 catch 后直接结束，没有诊断、结果或异常传播出口。

通过真实 public API、普通 C# `yield` + `try/finally` 的结果：

```text
normalFinally = true
stopFinally = false
ownerFinally = false
disposeFinally = false
faultObserved = false
```

这不只是枚举器是否能 GC：`finally` 中的事件 token、owned stream、lease 或其他资源释放不会因对象被回收自动执行。实际 [`RuntimeSession.cs:290`](../../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSession.cs#L290) 在停止 owner 工作时直接使用该 scheduler，因此影响能进入场景/会话生命周期。

**修复 owner：**`Inno.Core.Coroutines`。定义唯一的枚举器所有权和终止流程，按内到外释放嵌套栈，覆盖完成、停止、owner 停止、全停、异常和 scheduler 退休；清理失败必须继续收集其他释放结果，保留必要 owner 并使用已有 Core 退休/诊断契约。不得在 Scene 或各组件补一套平行清理表。

回归必须包含真实订阅/lease、嵌套 finally、自定义 IDisposable 枚举器、尚未第一次 Tick 的 pending routine、重入停止、重复 Dispose、一个清理失败时其余项仍被处理，以及 collectible generation 的弱回收确认。故障报告应使用中立可观察结果，不凭空要求 Core 绑定某个 Editor/平台日志服务。

### F03：History 拒绝非法顺序时破坏状态

[`EditorHistory.cs:489`](../../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistory.cs#L489) 的 `PopTransaction` 先 `TryPop`，再比较 transaction ID。外层先 Commit 时会弹出内层后抛错，内层随后正常 Rollback 找不到自己。

本轮使用公开 `EditorInteractionRuntime`、`IEditorHistory` 和正常 attribute-discovered handler，不使用反射/IVT。内层实际把标量从 0 改为 5；结果：

```text
controlLifo = true
rejectedOuterCommit = "Editor history transactions must complete in stack order."
innerRollbackAfterRejection = false
outerRolledBack = true
childApplied = true
valueAfterOuterRollback = 5
canUndoAfterOuterRollback = false
```

正确的拒绝应保持栈和 payload 完整。修复应在同一 owner 中先校验栈顶再进行 Pop；沿用已有事务和中立 History payload，不添加第二套 Undo 或兼容入口。补齐错误 Commit/Dispose/Rollback 顺序及异常后正确恢复的反例，并检查大 payload 的释放。

F02/F03 所在源码在本次审计及当前未提交的 BGCS 包升级中均没有改动；Git 最后修改记录均指向既有 `9b38edc9`。因此它们是本次发现的既有实现缺口，不应归责 BGCS 4.1.1 或本次审计；这不等于已经证明其更早引入时间。

### F04：Canvas 的 GPU 命中前已经发生完整纹理复制

正常链为 [`CanvasViewDrawable.TryPrepare:100–141`](../../../../InnoEngine.Canvas/Assets/Runtime/Rendering/CanvasViewDrawable.cs#L100) 的逐 draw 循环 → `new RenderTextureSubresourceData(..., update.pixels.Span)` → [`RenderTextureSubresourceData.cs`](../../../src/services/rendering/Inno.Rendering.Runtime/Resources/RenderTextureSubresourceData.cs) 的 `data.ToArray()` → [`RenderResourceService.AcquireTexture`](../../../src/services/rendering/Inno.Rendering.Runtime/RenderResourceService.cs#L192) 才比较相同 revision。

GPU 不重传不等于 managed 不复制。同一张不变纹理被多个 draw 使用时，也会逐 draw 复制。公开上传对象探针输入 1,048,576 bytes、预热后构造 32 次，多次反例运行观察到约 **33.6 MB（32 MiB 像素载荷加对象等开销）**分配；这是构造分配实测，不是完整 Canvas 帧时间测试，原始数字见 result.json。

修复应由 Canvas 在纹理 revision 改变时创建并复用不可变 upload，使用现有 Canvas render state 和 Core Rendering resource service。保留复制带来的所有权保护，不直接改成借用用户内存；释放纹理、Context/尺寸/代际更换时同步退休缓存。一个稳定纹理的 1/32/128 draw、连续热帧与真实更新分别测量，确认热帧没有与像素大小×draw 数成正比的分配。

### F05：Rendering2D 正常路径无法复用完整帧缓存

[`RenderRuntime.Frames.cs:139`](../../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.Frames.cs#L139) 总是给 RenderOutputSession 传入非空的 collector。[`Rendering2DModel.Build:1178`](../../../../InnoEngine.Rendering2D/Assets/Runtime/Pipeline/Rendering2DRenderer.cs#L1178) 原样交给 options，而 [`CreateCameraStackFrame:430`](../../../../InnoEngine.Rendering2D/Assets/Runtime/Pipeline/Rendering2DRenderer.cs#L430) / `CreateViewportFrame:518` 只有 `viewContent is null` 才复用完整帧。

因此不能把该缓存的存在当作正常 Player 静态场景已免重建的证据。Collect 会重新组合/排序、建立 pick records、逐 batch 创建并编码实例 byte[]；[`BuildBatches:2133`](../../../../InnoEngine.Rendering2D/Assets/Runtime/Runtime/Rendering2DFrame.cs#L2133) 为每 quad 80 bytes，persistent revision 也在编码后计算。10,000 quads 单次收集的实例载荷为 800,000 bytes，此数为源码计算，不是实测帧分配。

正确方向是分离稳定 2D 几何/实例与本帧外部内容、输入及交互路由，按真实 component/asset revision 更新；复用现有 snapshot、persistent resource、generation 生命周期。不能直接删除 `viewContent` 条件缓存整个帧，否则会冻结 Canvas 更新与输入。修复后要覆盖静态大场景、单对象变化、动态粒子、tilemap chunk、多个 camera、Canvas 与 reload，并测实际 CPU/分配。

### F06：热 Native 准备成本依然过高

当前 SourceDebug 正式日志中的暖命中，单次仍读取 **55,942 files / 4,498,416,361 bytes**，同时 `processes=0`、`bindingGenerations=0`、`materializedBytes=0`。采样快照已有 14 次暖准备，总计 62,977,829,054 bytes，单次约 143–312 秒。

这个样本伴有并行测试/编译负载，不能用来宣称改造前后回退百分比；但重复的文件/字节计数确认了规模成本。主要链为 [`PrepareProductNativeTask.Execute`](../../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L74) 新建操作 → `ProductNativeCacheLookup` 初始/锁后输入读取 → 多个工程分别执行 Native 准备。

优化必须保留“等待后 fresh 验证”与内容完整性。先收拢同一根 Build 的冻结计划和生产结果，独立部署者取得自己的 reader/owner；减少对相同完整产品重复准备，不把跨等待哈希永久缓存，也不以 mtime、长度、无关 MVID 代替内容。只在确认 immutable publication 的边界复用，继续使用 Core IO 和现有 Task Build scope。

关闭条件包括：空闲、同 SDK/配置/输入下至少五次热 Editor，十次固定输入构建，明确命中/文件/字节/进程统计；源码、SDK、Profile、工具改变以及等待期间变化仍正确失效。性能结论必须给中位数和分配，不能仅看 native compiler processes 为零。

### F07：预算内 metadata 维护尚未可靠收敛

本轮通过正式 CLI **只执行 dry-run**：5 秒预算在 inventory 阶段超时、退出 1、未退休任何内容；30 秒预算约 9 秒完成 dry-run，报告逻辑缓存 17,892,583,555 bytes、protected 9,432,415,728 bytes、106 个未实际退休的候选，reclaimed 为 0。

原有自动操作日志也多次报告 maintenance deferred。64 GiB 是当前保留策略，不是能无条件执行的磁盘硬上限；没有完成 inventory，就无法据此承诺过期 metadata 在日常操作中必然收敛。本机并行 IO 负载影响预算，不能推断所有机器或空闲状态都失败。

修复应围绕现有 owner catalog 做有界、可续作的元数据发现/退休，将必要的候选 fresh 检查保留在独占退休阶段，避免每次预算先耗尽在全量盘点。不要创建平行缓存数据库、按 `.lock` 后缀强删、删除 current 或降低 reader/等待者保护。最后以固定输入多次构建、活动读者退出、残留登记和叶锁数量收敛实测关闭；本轮没有删除用户缓存或旧验收目录。

### F08：Coroutine 重入停止会破坏活动列表

[`StopAllCoroutines(owner)`](../../../src/foundation/core/Inno.Core.Coroutines/CoroutineScheduler.cs#L104) 立即执行 ApplyPendingCommands，允许同一 Tick 在调用用户迭代器时重入。它会从 active 列表移除当前协程；但 TickActiveCoroutines 在 MoveNext 返回 false 后，仍按调用前的索引执行 RemoveAtActiveIndex，没有重新确认当前项是否仍在该位置。

正常 public 使用就能触发：第一个协程在执行体中停止自己的 owner，然后 `yield break`。

```text
只有这个协程：Tick 抛 ArgumentOutOfRangeException
后面还有另一个协程：没有异常，但连续两次 Tick 的 otherTicks=0
另一个协程的 otherHandleStillValid=true
```

第二种情况下，其他协程的活动项被误删，但 state/live ID 还存在，形成“handle 有效、永远不推进”的状态。游戏行为停止自身工作或销毁 owner 都可能到达这样的调用模式；不像 F03 那样需要错误的事务提交顺序。

修复仍归 Core.Coroutines：用户回调前后的调度状态必须按稳定 ID fresh 确认，避免索引在重入修改后失效。一起定义执行中枚举器的安全退休点，不能在 MoveNext 尚未退出时机械 Dispose，也不能仅新增 Dispose 以为解决了活动列表问题。复用既有调度与 Core 退休能力，不在组件层捕获越界或重新启动丢失任务。

关闭条件：自停/停止其他 owner/StopAll、嵌套迭代器、当前回调中启动新协程、回调异常、一次 Tick 多个协程的顺序及 handle 状态都有成功/失败/边界证据，并与 F02 的 exactly-once 清理和弱回收共同验证。

### F09：隔离构建的包身份验收读取了另一作用域

[`NuGetBindingDependencyTests.cs:40`](../../../tests/build/Inno.Build.Tests/NuGetBindingDependencyTests.cs#L40) 把 `tests/build/Inno.Build.Tests/obj/project.assets.json` 当作本次产物位置；该路径没有通过实际 MSBuild 求值取得。当前真实结果是：

| 输入 | BGCS.Runtime |
| --- | --- |
| 测试读到的默认 obj/project.assets.json | 4.1.0 |
| 本轮 SourceDebug 隔离 ArtifactsPath 的 project.assets.json | 4.1.1 |
| 当前 packages.lock.json | 4.1.1 |

因此出现 Expected 4.1.0 / Actual 4.1.1 的失败。它不能证明当前实际加载旧包，但证明该测试不适用于已经支持的隔离输出；全新环境默认 obj 缺失时也无法可靠验收。证据为 [`test-assets-path-mismatch.json`](../../../artifacts/acceptance/core-audit-2026-10-24/test-assets-path-mismatch.json)。

修复归测试输入/公共 MSBuild 消费边界：取得本次工程实际 `ProjectAssetsFile`、锁文件作用域和所选属性，或者以已有公开构建元数据传递准确输入。不要猜测输出目录层级，不读取另一个作用域作 fallback，不把删除缓存、额外正常还原或把两边版本字符串改成相同当作修复。本次报告没有改 tests。

关闭条件：普通输出、自定义 ArtifactsPath、目标/RID 作用域、默认 obj 不存在及默认 obj 故意保留另一包身份时，都能只验证本次实际恢复/消费的包身份和内容哈希。其他合成架构 fixture 中任意精确包版本示例，不应机械误判为生产依赖残留。

## 5. 架构、平台层与扩展性判断

### 5.1 当前成立的边界

```text
entry：当前宿主薄路由、两个开发入口
  ↓
发行注册：StandardBuildDistribution / StandardBuildHosts
  ├── 平台基础：目标、SDK、宿主执行能力、模板
  ├── 同平台 integration：backend 选择、BGCS Profile、布局与链接
  └── 部署实现：CoreCLR / NativeAOT / Mono Wasm
  ↓
中立构建契约：计划、组件能力、执行器、冻结输入、产物角色、事务
  ↓
共享 backend recipe / adapter：组件语义和可移植实现
  ↓
Core IO / Execution / Events / Identity / Serialization
```

箭头表示消费方向而非每个项目都直接引用全部下层。平台基础不依赖具体 backend，平台 integration 才做结合；共享 backend 不反向引用平台。Browser 聚合 executor 遍历 plan 中的组件/静态能力，不维护五组件名字 switch。StaticArchive/ImportLibrary 等角色与执行方式分别表达。

BGCS API 留在 Bindings 接入和平台 integration 内；中立 Toolchains 只消费 INativeBindingGenerator。Source/Input owner、SDK owner、产物 owner 各有边界；冻结源码真实复制，私有不可变发布才允许安全复制/hardlink。CLI/MSBuild 最终消费同一 distribution 和执行协议，没有独立 Rider 生产实现。

在现有 SPI 能力内新增 backend，应该修改自身实现/recipe、需要的同平台 integration/产品贡献以及发行注册；无需在共享执行器和游戏领域增加名称分支。新增平台需要真实 SDK、目标、运行入口、布局、能力与验收；新增托管执行模型需要对应编译/部署适配。这些正常新增代码不等于架构耦合，也不能承诺任何未知能力都无需扩展中立契约。

### 5.2 Core 复用和未认定为缺陷的项目

- Canvas 与 SpriteAnimator2D 使用 Core Events；没有把领域事件类型或算法队列误认成第二个事件总线。
- Canvas owner、ContentReadScope、Editor DragDrop 复用 Identity；DragDrop 保存 domain-qualified runtime identity，落盘仍应使用 persistent identity。
- ExecutionSlot 在失序 Dispose 前检查 LIFO，不会像 F03 先破坏栈；它还撤销继承 binding 并清除强引用。
- 反射元数据使用 ConditionalWeakTable；ScriptApiStubSourceBuilder 的线程局部 Type 引用在 finally 清空。不能把所有 Type 字典都机械判为 ALC 泄漏。
- GenerationCoordinator/AssemblyUnloadBarrier 保留 pending monitor，执行 Full GC → finalizers → Full GC，超时进入 Faulted；没有在本轮审阅链中发现清空 pending 后伪造成功的逻辑。
- History 通过 stable kind/persistent ID/中立 bytes 交接，正常 Undo/Redo 成功后才移动指针；F03 是错误顺序处理的具体缺口，不需要重建整个 History。
- Coroutines、Jobs、Events 分别表达迭代调度、作业执行、领域事件；职责差异不意味着重复事件系统。F02 应修在 Core 既有 owner 内。

**结论是“主要分层真实成立，内部仍有缺口”，不能升级成“整个架构已达到绝对干净”。**F02、F03 是生命周期/错误原子性，F04–F07 是数据构造和执行成本；接口数量和目录整齐不会自动解决它们。

## 6. 本轮验收矩阵

### 已取得的当前证据

| 检查 | 结果 | 限定 |
| --- | --- | --- |
| 完整 SourceDebug | 通过，0 警告、0 错误 | 本轮 4.1.1 对应全部源码；日志及 binlog 位于 bgcs411 证据目录，耗时 01:35:13.17。编译通过不替代运行或效率验收。 |
| 238 工程架构及有效项目求值 | 通过 | 本轮重新执行，见 architecture.log。 |
| 架构验证器回归 | 144 通过、0 失败、0 跳过 | 本轮当前源码程序集；architecture-tests/architecture.trx。 |
| 全部 59 个现有测试工程 | 原始合并结果 2,139 通过、2 失败、18 跳过 | Core/Build 14 工程、其余 44 工程、架构 1 工程分别执行；失败与后续文档复验见下文，不重复累计架构测试。 |
| 普通/隔离执行 | 原有隔离策略保留，28/28 隔离执行通过 | 两个正式入口分区完成 58 项发现；独立执行的架构项目无进程隔离 trait。普通阶段的两个失败不因隔离通过而消失。 |
| 修正后正式 XML / Wiki 门禁 | 通过 | 33,564 API symbols / 151 个 API 工程，196 个示例全部求值且无编译错误；其他平台产品 API 明确不在本次 windows-x64 求值范围。 |
| BGCS 正式包/源码完整性 | 通过 | 148 包文件、13 消费 DLL、BGCS 工作区/Agent 检查。 |
| 当前依赖锁 BGCS 身份 | 通过 | 623 相关文件、982 条记录均 4.1.1；不证明每个可能的产品目标都已构建。 |
| HostSource 正式生成/CheckBindings | 7/7、7/7 通过 | 使用官方命令，生成物不手改。 |
| 原 pthread 匿名类型 C17 复现 | 通过 | 生成和实际 workload Clang 编译退出 0。 |
| Browser 五组件 Native 聚合 | 通过 | 实际 CMake/Ninja、校验与发布；不是模拟工具链。 |
| FlappyBird Mono Wasm / AOT 发布 | 两种发布通过 | 发布成功不能代替运行。 |
| FlappyBird AOT 运行 | 有限帧 smoke 通过 | Edge 156、RTX 3070 Ti Laptop、WebGL2/ANGLE；3,000 帧、views=6、draws=94；输入已投递，看到实际游戏画面，正常结束。 |
| AOT 存储/资源边界 | 部分覆盖 | best-score 读取发生，未观察到写入；不能宣称完整游戏得分跨启动保存通过。仍有 WebGL 探测、ScriptProcessorNode 和 404 控制台信息，未全部归因/清零。 |
| Browser storage 公共契约 fixture | 通过 | 实际 localStorage：缺失、二进制、空值、列表、隔离、取消、删除；独立于游戏交互。 |
| FlappyBird Mono 解释执行 | 失败 | F01；无引擎/无 BGCS 对照亦复现。 |
| Canvas / Rendering2D 正式脚本与 IDE project Release | 通过 | 使用当前 4.1.1 Build host；并非复制旧通过数。 |
| FlappyBird 正式脚本与 IDE project Release | 通过 | 本轮生成；GameScripts 与 EditorScripts 均 0 警告/0 错误。 |
| Rendering2D 现有作者测试 | 42 通过、0 失败、0 跳过 | 真实当前脚本程序集、独立输出路径；不是 F04/F05 性能反例的替代品。 |
| Core/History 独立反例 | 缺陷实测确认 | 停止/finally、重入自停及非法顺序 Commit 均复现；探针返回 2 表示反例成立，不能计为产品通过。 |
| 5 秒 / 30 秒缓存 dry-run | 失败 / 通过 | 没有删除 payload、metadata、指针或锁。 |
| 包身份验收的输出作用域 | 失败 | F09；当前实际选中 4.1.1，但测试误读默认 obj。本轮不把此失败排除或伪装成包身份通过。 |

本轮分区执行明细：Core/Build 为 647 通过、2 失败、2 跳过；其余 44 工程为 1,348 通过、0 失败、16 跳过；架构工程为 144 通过。合计为本轮真实 TRX 的 2,139/2/18，不沿用前序 2,141/0/18。

两个失败分别是 F09 的 assets 作用域错误，以及首轮 Wiki 台账的外部消费者专名。后者已在文档中修正，并通过正式 XML/Wiki 全门禁独立复验；没有单独重跑原测试方法，因此保留首轮失败 TRX，不把它改写成一次测试方法通过。F09 仍未修复。18 跳过分别是两个当前 Windows 无法执行的文件 alias 用例与 16 个 Metal 用例；不是 18 项成功。

前序 Windows 12 项产品及十次热构建只保留原记录范围，不作为当前 4.1.1 全通过。本轮没有重新执行完整 Windows 产品矩阵。

### 未执行或不足以关闭的范围

- 无 Mac：Metal、ARM64、macOS Editor/CoreCLR/NativeAOT 和 Unix 实机不验收。
- 本轮不使用 computer use：没有实际点击 Rider Run，不以 CLI 结果冒充 IDE 交互结果。
- Windows 当前包完整 D3D11/D3D12/Vulkan × Debug/Release × Editor/Player 矩阵未在本轮重新全跑。
- 全新 clone、长/空格/跨卷路径、Windows 大小写敏感目录及不同 macOS 卷格式没有全部重新执行。
- 原生音频听感、完整 DPI/焦点/输入、多小时稳定性、游戏实际得分保存/重启、Browser 各引擎矩阵未完整执行。
- 没有同 SDK/同输入/空闲环境的完整改造前五次 Editor 性能基线，不报告“全面提速”或“没有性能回退”。

首轮 Wiki 门禁指出当前问题台账使用了外部示例/插件的专名，不符合 Wiki 只描述引擎自身契约的边界。已把 Wiki 条目改为中立描述，具体消费者继续留在本报告；保留首轮失败日志，修正后的门禁单独复验，不放宽验证器或混淆两轮结果。

## 7. 仍保留的边界风险

1. **U07 文件系统大小写语义：**多个 Native 路径缓存按宿主 OS 选择 comparer，Windows 大小写敏感目录不能仅靠物理路径解析证明正确。没有有效实机反例，本报告不将其写成已复现数据错误；正式支持前应增加真实目录测试。
2. **XML/Wiki 语义：**有效 summary/param/signature 和示例可编译不等于说明充分。当前存在模板式说明；公开 ownership、失败和生命周期仍需语义审阅。
3. **框架引用选择：**FrameworkReferenceResolver 在候选 root 下选择最高稳定 10.0 reference pack，而 IDE 工程主要由 SDK 选择框架引用。当前仅一个对应 SDK 的测试可通过；多 pack/多 root/不同 SDK 情况需要独立一致性反例，不能提前宣称已发现 API 不一致。
4. **历史 U08 SDK MissingMethodException：**当前未重现，保留历史观察；不宣称已根治，也不因此修改 SDK。
5. **交付状态：**开始时引擎已有 924 个 dirty/untracked 项，包含前序升级与大量作用域锁文件。本轮保留它们，未批量清理、提交或把它们冒称本次修复。最终实现交付仍应审查哪些锁作用域是实际需要的。

## 8. 建议实施顺序及关闭验收

| 顺序 | owner 与工作 | 关闭条件 |
| --- | --- | --- |
| 1 | Core.Coroutines 的资源终止、重入安全与故障出口；Editor History 拒绝操作的原子性 | F02/F08/F03 的公开反例变为通过，正常路径及失败清理、嵌套和 reload 验证通过。优先保障停止/回滚可靠。 |
| 2 | Browser 扩展/integration 与 .NET 互操作能力闭合 | F01 独立成对工程和正式 BGCS 生成通过；解释执行/AOT FlappyBird 都运行，通过真实参数/返回/callback ABI，仍不修改第三方包或生成物。此项可与第 1 项独立推进。 |
| 3 | Canvas 不可变 texture upload 复用；Rendering2D 稳定几何/动态内容分离 | F04/F05 热帧分配不随不变像素×draw 线性增长；静态/动态、camera、输入、Canvas、资源释放与 reload 一致。 |
| 4 | 根 Build 内的 Native 准备复用与 cache inventory 有界化 | F06/F07 在空闲可重复采样及十次固定输入下闭合；错误输入仍失效、活动 owner 受保护、metadata 数量收敛。 |
| 5 | 测试输入作用域修复及当前最终状态全回归 | F09 普通/隔离产物反例通过；完整普通/隔离、架构、XML/Wiki、脚本/IDE、Host/Browser 生成与 ABI；Windows 与可运行 Browser 产品矩阵逐项留证。 |
| 6 | 整理实际变更、文档与提交 | 只提交已完成且验证的实现和相符文档；保留明确未执行矩阵。不把历史记录删除误当消除 legacy，不保留生产兼容双路径。 |

每一项都应优先复用既有 Core/公开 SPI。无需创建平台版本全局服务、第二个事件/Identity 管理器、独立 Undo 栈或专属 Rider 流水线。确实需要增加的公开契约应先说明 owner、不可变输入、失败和生命周期，再同步 XML/Wiki、引用和架构规则。

## 9. 复现入口和证据索引

```text
artifacts/acceptance/core-audit-2026-10-24/
├── inventory.json / scan-candidates.json
├── architecture.log / architecture-result.json
├── architecture-tests/architecture.trx
├── bgcs-integrity.json / dependency-locks.json
├── source-integrity.json / source-build-result.json
├── native-preparation-cost.json
├── cache-dry-run-{5s,30s}.log / *-result.json
├── calli-probe/
│   ├── CalliProbe.csproj / Program.cs / probe.c
│   └── wwwroot/index.html / main.js
├── probe-{calli,direct}-publish.log
├── probe-{calli,direct}-runtime.log
├── probe-runtime-results.json / interop-signatures.json
├── history-probe/HistoryProbe.csproj / Program.cs
├── history-probe-project/result.json / history-probe.log
├── rendering2d-tests.log / rendering2d-tests/authoring.trx
├── scripts-FlappyBird.log / ide-FlappyBird.log
├── wiki.log / wiki-api.json / wiki-examples.json / wiki-summary.json
├── tests/ / tests.log
├── remaining-tests/ / remaining-tests.log
└── test-summary.json / test-assets-path-mismatch.json
```

选定 .NET CLI 后，历史/协程/上传对象公开反例的入口为：

```powershell
dotnet run --project artifacts/acceptance/core-audit-2026-10-24/history-probe/HistoryProbe.csproj `
    -c Release -- artifacts/acceptance/core-audit-2026-10-24/history-probe-project
```

该探针引用本轮正式 CLI 的实际引擎程序集；证据目录缺失时，应先构建对应当前引擎，而不是换成任意旧二进制。预期返回 2 是当前缺陷证据，修复后需要重新编译消费程序集并更新预期。

独立 Wasm 反例的 `ProbeMode=calli/direct` 只选择测试调用方式，不是生产双路径。用实际 10.0.402/workload 发布至不同输出，再经本地 HTTP 服务在实际 Browser 中运行；不以 dotnet build 通过替代 JS/Wasm 执行。

保留的无效验收命令也如实区分：`verify --configuration SourceDebug` 不符合验证器的 Debug/Release 参数契约；派生 IDE Solution 只有 Debug 配置，Release 验证已改用正式 project 入口并通过；独立 Wasm 探针首次使用不匹配的 `__Internal` 模块名，修正为实际 NativeFileReference 模块后才取得成对证据。它们不是本报告新增的生产缺陷。

## 10. 完成前自查

- 已主动检验“是否真的达到超干净要求”：答案为**尚未达到**。238 工程门禁和正确目录支持分层结论，但 F01–F09 的执行/资源/分配及验收作用域证据阻止全面验收通过。
- 检查深入输入来源、冻结、实际调度、错误后状态、owner/lease、GC 退休与帧数据构造；不是按接口数量评价架构。
- Core IO/Events/Identity/Execution/Serialization 已有能力得到复核；建议修复都留在相应 owner，避免上层平行系统。
- 没有修改 BGCS 源码、NuGet 包、第三方头或生成文本；没有改生产行为、原测试工程、引入 IVT/反射后门、global/implicit using、legacy reader 或兼容层。
- 复核审计基线中 2,505 个源码/工程/脚本文件的 SHA256 均未变化；三个调用方仓库的 Git 工作区仍干净。本次只增加验收反例/证据和报告，并同步当前 Wiki 状态，没有暗中修复后再混用旧证据。
- 自查结论不能替代尚未完成的测试和平台矩阵。报告区分已通过、失败、源码确认、历史证据与未执行。
- 当前 Windows 不存在指定 `/System/Library/Sounds/Glass.aiff`，未播放提示音。
