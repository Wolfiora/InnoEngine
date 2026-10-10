# 2026-10-06 架构整改验收

[架构索引](README.md) · [完整执行计划](ARCHITECTURE_CLEANUP_PLAN_2026_10_06.md) · [当前项目总览](ENGINE_ARCHITECTURE_OVERVIEW.md)

**A01–A08 的源码整改、当前消费者及本机必需验证已完成。** 最终 Release Solution、Debug Editor 构建均为零警告、零错误；仓库架构验证通过。Windows CoreCLR、Windows NativeAOT、Web 解释执行和 Web AOT 四条 FlappyBird 路径重新发布并实际运行。

## 1. 测试、环境与源码身份

最终按程序集、类和参数化测试名称取最新结果：**2,727 passed、0 failed、16 skipped**。

| 范围 | 最新结果 |
| --- | --- |
| InnoEngine | 1,699 passed / 16 skipped |
| BGCS | 1,022 个唯一测试名称通过；原始 TRX 为 1,023 次通过执行，其中一个显示名称重复 |
| Canvas | 6 passed |

跳过项为 Windows 不能执行的 macOS Metal 测试，详细名称、来源 TRX、历史失败和成功重跑见 [test-summary-final.json](../../artifacts/acceptance/architecture-cleanup/results/test-summary-final.json)。没有删除失败证据后假称首次全部通过。

宿主为 Windows 11 10.0.26200 / win-x64；引擎工程选择 .NET SDK 9.0.318 / Runtime 9.0.20，BGCS 工具工程选择 SDK 10.0.401。Web 使用 .NET 9 workload 对应的 Emscripten 3.1.56 / 9.0.20，目标 wasm32；桌面运行 D3D11，浏览器运行 WebGL 2。

| 仓库 | 当前 HEAD |
| --- | --- |
| InnoEngine | `d348a5da67c6a88c8511ef9bfaeda5019681b372`，本轮整改尚未提交 |
| BGCS | `d4bfe3939b6ab2c404080f9da58e4b5f64c861be`，当前工作区干净 |
| Canvas | `f0e5b6cbfb1a37e09e77400d5446daeb5be7de81` |
| Rendering2D | `c299a3ed43a63f0f68ed0cda334124e8c2f94fce` |
| Samples | `6e3dd0043b6a7ee0d1f0b43d805b6d5c5a9280cb` |

完整信息见 [environment.json](../../artifacts/acceptance/architecture-cleanup/baseline/environment.json)、[起始 revision/已有改动](../../artifacts/acceptance/architecture-cleanup/baseline/revisions.json)和[最终工作区状态](../../artifacts/acceptance/architecture-cleanup/results/revisions-final.json)。HEAD 不代表未提交源码，最终文件清单与工作区记录共同标识本次结果。既有 Editor Native 普通 Build/Publish 修复保留并重新验证。

## 2. A01–A08 逐项结果

| 项目 | 已实现结果 | 验收 |
| --- | --- | --- |
| A01 Rendering 拆层 | Core 仅含图形机制；运行资产、Authoring、Runtime 独立。模型、请求和 GPU owner 归 Runtime；BGFX Adapter 消费中立 Core。 | 四层编译、资产 round-trip/材质/几何/last-good、Stable ID、脚本消费、Player closure 和禁止依赖门禁通过。 |
| A02 内容完整性 | Pack 索引、完整条目集合、长度、SHA-256 和预算统一验证；generation cache 使用写 lease、原子 current 和 reader lease 修复。 | 损坏、缺失/额外条目、同长度篡改、并发、取消、旧 reader 固定 generation 的 Content/FileSystem/Core.IO 测试通过。 |
| A03 通用输入 | `EventInputSource/Backend/Provider` 归通用 Input；Platform 只转换 Core Events。Desktop、Browser、Editor Play 共用入口。 | Input/PlayMode/Interactions 测试通过；浮动 GameView 聚焦可输入，主 Editor 聚焦后阻止游戏输入。 |
| A04 路径中立 | 内容 store、ArtifactLease、Player 内容来源、文档 store、storage/log factories 隔离物理布局；运行领域只读 lease。 | 纯内存公开 Player 生命周期 7 项、Asset 流 pin、只读 Settings、音频 Decode/Stream/取消/退休及四条实际部署通过。 |
| A05 唯一 Build 组合 | Composition 同时提供 target、deployment、Support Pack；Editor/CLI/MSBuild 共用。 | fixture、配对负向测试和实际构建通过；未知/不支持的 deployment 在 provisioner 调用前失败。 |
| A06 Presentation | device 必须声明真实 nullable 主输出尺寸；无 surface 为 null，离屏继续；无默认 1×1。 | 无主输出、离屏、恢复、尺寸和输入坐标测试通过；实际最小化/恢复和浮动 viewport 正常。 |
| A07 Native 增量 | 共同 Task、组件 recipe、operation-owned snapshot、实际工具/SDK 输入、完整产物及 export 校验。相同内容不替换加载中的 DLL。 | 冷/三次热构建、recipe/损坏/并发/取消/export/加载 DLL 测试和完整 Native 消费链通过。 |
| A08 owner 与帧 | 九个 owner 按职责展开；注册变化发布 Contributor snapshot，scratch 在结束/失败/退休清空；Validate 与 Compile 分离。 | token 等价、mutation rollback、诊断/调度、0/1/8/32 成功/异常/退休测量及 collectible ALC 退休通过。 |

执行中额外修复：Artifact 释放回调移出 residency gate，避免读取退休锁重入；脚本 XML 一次冻结替换片段，消除重复全量字符串扫描；架构 XML 继承优先解析所属项目主程序集，避免依赖闭包旧副本；分数缩放菜单的尺寸取整与 ImGui 保持一致；Browser 工具链配置归一化；Support Pack 准备先验证 deployment。对应 Assets 30 项、Scripting 148 项、Architecture 77 项以及最新 preflight/Contributor/原生 Widget 回归通过。

## 3. 最终依赖、项目与文件

```text
Rendering Core → Foundation
Rendering Assets → Core + Assets/References + Foundation
Rendering Shaders → Core + Assets + Graph/Foundation
Rendering Assets.Authoring → Assets + Shaders + Assets.Pipeline
Rendering Runtime → Core + Assets + Runtime.Contracts + Content/References
Rendering Adapter → Core + Adapter SPI + 对应 Native

Player.Runtime → 共享 Shell/Runtime + 内容/存储/日志契约
Desktop/Browser composition → Player.Runtime + 对应内容来源/Adapter

Inno.Build → 通用构建契约
Inno.Build.Composition → Build + Platform/Managed/Support 实现
Editor/CLI/MSBuild → 同一个 Build.Composition

Platform Adapter → Core Events → EventInput → Input Runtime
```

新增四个生产库：`Inno.Content`、`Inno.Adapter.Content.FileSystem`、`Inno.Rendering.Assets.Authoring`、`Inno.Build.Composition`。内容、文件缓存和资产创作增加对应测试项目；五类创作测试迁入 Authoring。删除 `Inno.Adapter.Input.Sdl3` 及项目页，当前消费者使用 events ID，没有 alias。

替换后删除 `RuntimeContentDeployment`、`BrowserContentLoader`、`FileRenderTargetArtifactProvider`、`BuiltInPlayerSupportPacks` 和原 `PipelineResources.cs`。迁移没有保留旧 namespace 转发或兼容 overload。graph raster/compute/copy builder 按实际职责分文件，owner 字段仍集中，没有第二状态 owner。

完整对应见 [file-map.tsv](../../artifacts/acceptance/architecture-cleanup/file-map.tsv)、[consumer-file-map.tsv](../../artifacts/acceptance/architecture-cleanup/consumer-file-map.tsv)及[file-map-summary.json](../../artifacts/acceptance/architecture-cleanup/results/file-map-summary.json)：2,394 条执行记录、68 条消费者记录，旧路径未分类项为空。Consumer 清单覆盖 Canvas、Rendering2D 源码、当前 Plugin 和 Samples；不是只做字符串替换，脚本编译、Plugin 输出、安装和运行闭包实际重新消费。当前文件长度与 SHA-256 另存于 [final-source-manifest.json](../../artifacts/acceptance/architecture-cleanup/results/final-source-manifest.json)，机器可读 gate 汇总见 [acceptance-summary-final.json](../../artifacts/acceptance/architecture-cleanup/results/acceptance-summary-final.json)。

当前 **146 个生产项目**包含 Native 生成扩展库，全部有 Wiki；69 个受影响项目页同步 **4,872 个 public/protected 声明**。Solution 无空 folder、缺失项目和无效 csproj XML。生产 Program 保持 Editor、Desktop Player、Browser Player、统一 Build CLI 四个入口；BGCS 仅一个 Tool 入口。证据见 [solution-structure-final.json](../../artifacts/acceptance/architecture-cleanup/results/solution-structure-final.json)。

## 4. 公开 API 与身份

| 契约变化 | 必要性和稳定语义 |
| --- | --- |
| ContentKey、Pack Index/Reader、IRuntimeContentStore、ContentReadLease | 统一逻辑定位、完整性、预算和读取生命周期，不向领域提供路径。 |
| IByteDocumentStore、共享 FileLease | Settings 可以来自文件或 owned 只读 bytes；读/写者复用共同 IO 协调。 |
| ArtifactLease.OpenRead / metadata | 保留已有读取入口；移除 absolutePath，打开的流独立 pin 内容。 |
| IPlayerContentSource、PlayerContentMetadata、PlayerLaunchOptions | 平台准备 owned metadata/store；共享 Player 验证生命周期，宿主注入 storage/log 工厂。 |
| Session/Settings/Module context | 去除共享部署/持久目录要求；动态 Adapter 自持 shadow-copy root。 |
| StorageScope | 中立应用/会话 namespace；具体 provider 映射物理位置或 Browser origin。 |
| IAudioClipSource、AudioClipDescriptor | Runtime 提供编码 lease；MiniAudio 的文件物化和按内容缓存归 Adapter。 |
| IRenderDevice 主输出/SetPrimaryPresentationSize | 必须实现真实 nullable 输出，null 表示不可呈现，非法尺寸明确拒绝。 |
| RenderGraphValidationResult | 冻结诊断/计数，不构造可执行图；最终 Compile 复用同 revision 分析。 |
| NativeBuildRecipe / NativeBuildInput | 明确输入、逻辑身份、实际工具/参数和实现闭包，不依赖无关程序集 MVID。 |
| BuildDistribution / Context / PipelineFactory | 一次组合 target、deployment、Support Pack，三个宿主共享。 |
| EnsurePlayerSupportPackAsync(target, deployment, token) | null 使用 target 默认；显式选择在工具启动前验证，旧 overload 删除。 |

完整源码签名见 [project-public-api.json](../../artifacts/acceptance/architecture-cleanup/results/project-public-api.json)及相应 Wiki。内部 cache generation、scratch 和 validation state 明确是内部实现。

165 个迁移声明更新实现归属，脚本仍使用 `InnoEngine.Rendering`；每个项目仅自己的 ScriptingApi 清单，没有中央反向白名单。运行时与 IDE references、Plugin、NativeAOT/Web 静态注册重新验证。

五个原先推导 ID 的嵌套持久 DTO 固定为原 Stable ID，并从最终 Release 元数据核对。[stable-id-comparison-final.json](../../artifacts/acceptance/architecture-cleanup/results/stable-id-comparison-final.json)列出的九个未保留 ID 属于非持久 parser/resolver/cache key/frame key/私有 GPU binding，生成内部类型另行排除；没有 former ID 或 legacy reader。

## 5. 所有权和失败边界

| 资源 | owner / 停止顺序 |
| --- | --- |
| Pack archive/stream | store 持有；关闭拒绝新 lease，已有 lease 可完成；读取独立状态，共享定位内部同步。 |
| cache generation | preparation 验证后发布，reader 固定 generation；current 原子切换，仅无 reader 的旧代可退休。 |
| Asset/读取流 | Residency 管理依赖与 pin；外层 lease 释放不提前销毁打开流。 |
| Player | 来源是宿主借用对象；准备的 store、模块、Session 服务按公开契约转移并失败清理。 |
| MiniAudio | 取消并完成准备工作，再释放 source lease；callback 留在原生，Stream 保持流式解码。 |
| GPU/帧 | Runtime、ResourceService、Device 保持所属 owner；scratch 在结束/异常清空，GPU 通过既有退休队列释放。 |
| Contributor | 注册变化发布 snapshot，当前帧固定，下一安全点接受变化；不跨帧保存旧扩展委托。 |
| reload | candidate transaction → Full GC → finalizers → Full GC → 弱 monitor 不可达；残留进入 Faulted，不报告 Success 或继续 Play/Build/Export。 |
| Editor | 捕获 Module/Panel/layout 并原子保存 editor.ini，再停止模块/卸载 Scene；Selection 不持久化。 |

`.complete` 不再决定有效性。取消在提交前清理候选，提交后不撤销新有效 generation；退休清理失败独立报告。namespace 默认 application ID，位置属于 provider，保留 Project/Edit/Play 隔离，没有 `InnoEngine` 产品前缀。

## 6. 实际游戏与 Editor UI

项目固定为 `C:/Dev/GameEngineDev/InnoEngine.Samples/FlappyBird`，输出位于证据根 `builds/`。

| 路径 | 发布和运行 | 图像/交互 |
| --- | --- | --- |
| CoreCLR | self-contained single-file 发布成功；180 帧退出 0，views=2、draws=25 | [白天](../../artifacts/acceptance/architecture-cleanup/captures/coreclr-day.png)、[夜晚星光](../../artifacts/acceptance/architecture-cleanup/captures/coreclr-night.png)、[输入/碰撞](../../artifacts/acceptance/architecture-cleanup/captures/coreclr-gameplay.png) |
| NativeAOT | 发布、静态链接成功；180 帧退出 0，views=2、draws=25 | [白天](../../artifacts/acceptance/architecture-cleanup/captures/nativeaot-day.png)、[夜晚](../../artifacts/acceptance/architecture-cleanup/captures/nativeaot-night.png)、[玩法](../../artifacts/acceptance/architecture-cleanup/captures/nativeaot-gameplay.png) |
| Web 解释执行 | 当前发布、浏览器输入/渲染、存储成功，无 console error | [白天](../../artifacts/acceptance/architecture-cleanup/captures/web-interpreted-day.png)、[夜晚](../../artifacts/acceptance/architecture-cleanup/captures/web-interpreted-night.png)、[得分](../../artifacts/acceptance/architecture-cleanup/captures/web-interpreted-scored.png)、[Best 恢复](../../artifacts/acceptance/architecture-cleanup/captures/web-interpreted-best-restored.png) |
| Web AOT | AOT、链接、浏览器运行成功，无 console error | [白天](../../artifacts/acceptance/architecture-cleanup/captures/web-aot-day.png)、[夜晚](../../artifacts/acceptance/architecture-cleanup/captures/web-aot-night.png)、[得分](../../artifacts/acceptance/architecture-cleanup/captures/web-aot-scored.png)、[Best 恢复](../../artifacts/acceptance/architecture-cleanup/captures/web-aot-best-restored.png) |

Web 两路均真实操作得到 Score 1，同 origin 重载后显示 Score 0 / Best 1，没有用 JavaScript 改游戏状态。桌面截图的 Score 0 不作为得分持久化证明；存储和会话隔离另有公开回归。最终 Debug/Release Editor 各跑完 120 帧，正常保存状态并退出。

可见 Editor 验收：

- [浮动 GameView 输入](../../artifacts/acceptance/architecture-cleanup/captures/editor-floating-game-focused-input.png)、[主 Editor 阻止游戏输入](../../artifacts/acceptance/architecture-cleanup/captures/editor-play-input-blocked-main.png)、[Stop Play 恢复 Edit](../../artifacts/acceptance/architecture-cleanup/captures/editor-play-edit-restored.png)。
- [重叠窗口消费点击](../../artifacts/acceptance/architecture-cleanup/captures/editor-overlap-consumption.png)，下层 File Panel 没有被关闭。
- [短 selector](../../artifacts/acceptance/architecture-cleanup/captures/editor-small-selector-fixed.png)、[菜单搜索宽度](../../artifacts/acceptance/architecture-cleanup/captures/editor-context-menu-fixed.png)、[长 selector 向下有界](../../artifacts/acceptance/architecture-cleanup/captures/editor-long-selector-bounded.png)。
- [Shader 小地图](../../artifacts/acceptance/architecture-cleanup/captures/editor-shader-minimap-before.png)和[实际交互平移](../../artifacts/acceptance/architecture-cleanup/captures/editor-shader-minimap-after.png)。
- [2:3 Export 表单](../../artifacts/acceptance/architecture-cleanup/captures/editor-export-form-final.png)、[独占进度](../../artifacts/acceptance/architecture-cleanup/captures/editor-export-exclusive-progress.png)、[取消关闭](../../artifacts/acceptance/architecture-cleanup/captures/editor-export-cancel-closed.png)、[成功自动关闭](../../artifacts/acceptance/architecture-cleanup/captures/editor-export-success-closed.png)、[失败自动关闭](../../artifacts/acceptance/architecture-cleanup/captures/editor-export-failure-closed.png)。失败使用验收目录中的阻塞文件，既有输出未覆盖，Console 保留正确记录。
- [最小化后恢复](../../artifacts/acceptance/architecture-cleanup/captures/editor-minimize-restored.png)及正常退出保存。

实际 MiniAudio Decode/Stream、取消和退休通过；没有声学设备/主观听音测量。原生 Widget 覆盖 0.85、0.9、1.1 分数缩放；没有更改系统 DPI 或验收任意多屏 DPI 组合。Play 中破坏场景的隔离通过公开事务测试验证，没有把正常 Stop 截图当作全部破坏场景的证明。

## 7. 性能

### Native 冷/热

| 构建 | 总耗时 | Native 阶段 | 原生进程 | 哈希读取 |
| --- | --- | --- | --- | --- |
| Debug 冷 | 11:53.40 | 337.079 s | 13 | 241,791 次 / 24,376,971,471 bytes |
| Debug 热 1 | 85.245 s | 23.405 s | 0 | 140,044 次 / 13,947,413,919 bytes |
| Debug 热 2 | 82.456 s | 23.532 s | 0 | 同上 |
| Debug 热 3 | 73.417 s | 23.131 s | 0 | 同上 |

同一 Task 身份只准备一次；热命中不启动 compiler 或重复生成相同绑定，仍完整验证输入与产物。**热构建不是零 IO，SDK/原生闭包哈希仍有成本。** 没有同环境整改前 Native 基线，不引用历史 193 秒记录宣称改善百分比。最终 Release Solution 为 3:05.71，不属于上述受控样本。

### Runtime

独立验收消费者重建拆分前 Runtime 成员 token 和分离前 Graph 作为 before，after 使用当前代码；只适配新统计 DTO 参数，没有改旧帧算法。相同运行时、512 帧预热、4,096 帧测量、顺序重复三次，取中位数：

| Contributor | 前 bytes/frame | 后 bytes/frame | 减少 | 前 ms/frame | 后 ms/frame |
| --- | --- | --- | --- | --- | --- |
| 0 | 5,728 | 5,352 | 6.56% | 0.00914 | 0.00830 |
| 1 | 11,008.30 | 7,744 | 29.65% | 0.01745 | 0.01324 |
| 8 | 64,558.10 | 41,336.03 | 35.97% | 0.12890 | 0.09120 |
| 32 | 540,698.21 | 367,879.25 | 31.96% | 0.18074 | 0.13014 |

另有 0/1/8/32 Contributor 异常、注册/注销/下一空帧的退休循环，所有可执行帧最终 Compile 次数为 1。32 Contributor 退休循环约 494,486 bytes/cycle、0.957 ms/cycle，含两个帧和注册变化，不与稳定帧直接比较。ALC 退休由独立 Full GC/弱引用测试证明，这些计时不含 GC barrier。

此对照不是旧引擎完整 checkout 或 GPU benchmark，不能推导游戏 FPS。图分析/最终图仍有分配；改变的 revision 执行完整分析，Compile 复用，不宣称局部增量拓扑算法。cycle、初始化读取、attachment/store、capability、view limit 和最终完整验证保留。原始数据和方法见 [performance-final.json](../../artifacts/acceptance/architecture-cleanup/results/performance-final.json)，纯职责展开见 [token proof](../../artifacts/acceptance/architecture-cleanup/results/responsibility-split-token-proof.json)。

## 8. 命令和日志

从仓库根运行，使用工程 SDK：

```powershell
$env:DOTNET_ROOT = 'C:/Users/23842/AppData/Local/InnoWebDotnet'
$env:DOTNET_HOST_PATH = "$env:DOTNET_ROOT/dotnet.exe"
$env:MSBUILDDISABLENODEREUSE = '1'
& $env:DOTNET_HOST_PATH build InnoEngine.sln -c Release -nr:false --nologo
& $env:DOTNET_HOST_PATH build src/composition/editor/host/Inno.Editor.Application/Inno.Editor.Application.csproj -c Debug -nr:false --nologo
& $env:DOTNET_HOST_PATH build/cli/Inno.Build.Cli/bin/Release/net9.0/Inno.Build.Cli.dll verify C:/Dev/GameEngineDev/InnoEngine --configuration Release
& $env:DOTNET_HOST_PATH test tests/rendering/Inno.Rendering.Runtime.Tests/Inno.Rendering.Runtime.Tests.csproj --no-build -c Debug --logger trx
```

统一发布使用 `game --project C:/Dev/GameEngineDev/InnoEngine.Samples/FlappyBird --target windows-x64 --deployment nativeaot --startup-scene FlappyBird/FlappyBird.iscene`，并传入本轮 `--support-packs` / `--output` 证据目录。其他路径用注册 ID `coreclr`、`mono-wasm`、`mono-wasm-aot` 及对应 target；不建立 `native-aot` 拼写 alias。

| gate | 日志/结果 |
| --- | --- |
| 最终 Solution | [solution-release-completed.log](../../artifacts/acceptance/architecture-cleanup/logs/solution-release-completed.log) |
| 最终 Debug Editor | [editor-debug-completed.log](../../artifacts/acceptance/architecture-cleanup/logs/editor-debug-completed.log) |
| 架构 | [architecture-completed.log](../../artifacts/acceptance/architecture-cleanup/logs/architecture-completed.log) |
| Editor 120 帧 | [Debug](../../artifacts/acceptance/architecture-cleanup/logs/editor-debug-completed-smoke.log)、[Release](../../artifacts/acceptance/architecture-cleanup/logs/editor-release-completed-smoke.log) |
| CoreCLR 发布/180 帧 | [发布](../../artifacts/acceptance/architecture-cleanup/logs/flappybird-coreclr-final-current.log)、[运行](../../artifacts/acceptance/architecture-cleanup/logs/coreclr-player-final-current-smoke.log) |
| NativeAOT 发布/180 帧 | [发布](../../artifacts/acceptance/architecture-cleanup/logs/flappybird-nativeaot-final-current.log)、[运行](../../artifacts/acceptance/architecture-cleanup/logs/nativeaot-player-final-current-smoke.log) |
| Web 发布 | [解释执行](../../artifacts/acceptance/architecture-cleanup/logs/flappybird-web-interpreted-config-fixed.log)、[AOT](../../artifacts/acceptance/architecture-cleanup/logs/flappybird-web-aot-final.log) |
| Native 全链 | [日志](../../artifacts/acceptance/architecture-cleanup/logs/native-full-verification-final.log)、[结构化报告](../../artifacts/acceptance/architecture-cleanup/results/native-contracts-final.json) |
| 可见 Editor 导出/退出 | [editor-final-visible.out.log](../../artifacts/acceptance/architecture-cleanup/logs/editor-final-visible.out.log) |
| 最新负向/帧测试 | [preflight TRX](../../artifacts/acceptance/architecture-cleanup/results/deployment-preflight-final.trx)、[Contributor TRX](../../artifacts/acceptance/architecture-cleanup/results/contributor-matrix-final.trx) |

Native 全链为七个 binding component、八个 native product、七个 native 测试项目：确定性生成、C++ 桥、无手写 import、编译和实际调用通过。export、目标生成身份、Support Pack 与部署对应关系有回归。原始失败、负向注入和被拒绝的错误参数日志保留。

## 9. 扩展方式与 BGCS

新内容来源只实现 `IPlayerContentSource` 并返回共同 Reader 验证的 store；注入平台 composition，不改 Asset/运行服务。新平台增加 host/frame driver、SDK toolchain、packager/validator、所需 managed compiler，在唯一 Build Composition 注册配对；系统事件仍进入 Core Events。新图形后端实现 device/capability/commands 和 authoring toolchain，handle 留在 Adapter/Native。新部署实现 `IManagedDeploymentCompiler`，声明真实能力，并验收 SDK 发布/链接/回调；替换 CoreCLR Wasm 不能只改 ID。

详见[平台接入指南](PLATFORM_EXTENSION_GUIDE.md)、[Content](../assets/Inno.Content.md)、[Build Composition](../build/Inno.Build.Composition.md)。平台无关是共同机制不依赖具体平台，宿主、SDK、系统生命周期和能力验证仍各有边界。

BGCS 以自己的 NativeApi/CppFacade fixture 验收，没有引擎 reference 或游戏专用定义：NativeAOT、Wasm 解释执行、Wasm AOT 各 56 项实际调用通过，覆盖 DllImport/LibraryImport/FunctionTable。注入错误原生返回值后首个 scalar 检查明确失败。见 [BGCS 独立报告](../../artifacts/acceptance/architecture-cleanup/BGCS_INDEPENDENT_ACCEPTANCE.md)及[独立汇总](../../artifacts/acceptance/architecture-cleanup/results/bgcs-independent-summary-final.json)。引擎 binding、Plugin、FlappyBird 是另外的联调证据。

## 10. 清洁度与未实测范围

- 架构门禁覆盖新项目、公开 XML、排版、显式 using、Editor 引用边界、领域/原生消费、循环、generation cleanup 和脚本清单。
- 没有 legacy reader、schema version、alias、兼容 wrapper、IVT、测试后门、反射穿透或新增生产 Program；没有手工编辑生成绑定或 `extern`。
- Wiki 源码签名、覆盖及相对链接通过，[wiki-check-final.json](../../artifacts/acceptance/architecture-cleanup/results/wiki-check-final.json)中缺页/失效链接为空。
- macOS Native/Metal、iOS、主机、任意多屏 DPI、声学听音和 GPU 性能未实测；不以本机门禁代替设备验收。
- 本地验收游戏、Editor 和临时 Web 服务已关闭，改动供审阅，没有自动提交或远程发布。
- Windows 无法访问 `/System/Library/Sounds/Glass.aiff`，完成提示音未播放。
