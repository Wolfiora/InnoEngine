# 平台与运行时重构验收

[架构索引](README.md) · [完整计划](PLATFORM_RUNTIME_REFACTOR_PLAN.md) · [当前架构](PLATFORM_RUNTIME_ARCHITECTURE.md) · [Wiki 首页](../README.md)

## 状态与范围

**已完成批准 Plan 的重构实现和本机验收。原 Plan 的未来节点及其他实机平台状态单独列出；改动未自动提交。**

执行基线为 InnoEngine `50fc7f52`、独立 BGCS `3fc481d`。本轮从 2026-10-04 开始，
终版证据重新来自当前手写源码、重新生成的绑定与当前消费方。
阶段性失败和修复前结果保留在证据根的 `historical-progress.md` 及旧日志中，不计入终版通过。

证据根：`artifacts/acceptance/2026-10-04-refactor/`；表格中的相对路径均从该目录起算。
全部项目路径和改动文件 SHA-256 记录在 `source-state.json`。各项目的 API 和生命周期详见对应 Wiki。
完整要求对应见 [Plan 逐项核对](PLATFORM_RUNTIME_PLAN_AUDIT.md)。明确源码文件节点核对为
456 项：434 项位于指定位置，22 项复用已记录的共同边界，未解释缺项为零。

## 环境

- Windows x64，Visual Studio 2022 Build Tools、LLVM、CMake 和 Ninja。
- 引擎 SDK 9.0.318；浏览器 runtime/workload 9.0.20；Emscripten 3.1.56、wasm32。
- BGCS 按自己的 `global.json` 选择 SDK 10.0.401；独立互操作消费者使用 SDK 9.0.318。
- 浏览器验证使用独立后台 Edge、专属 profile 和软件 WebGL，未操作用户已有浏览器。
- 桌面 Player 使用隐藏窗口、真实 Direct3D11，完成 120 帧运行。
- 本机没有 macOS、iOS 或主机 SDK/设备实机验收；这些状态在下文独立列出。

## 完成的结构

```text
src/foundation   Core、目录事实、模块/类型发现和代际事务
src/content      Assets、References、Scene、Animation
src/services     后端中立领域契约与运行服务
src/runtime      Runtime、部署、生成注册、Plugin、创作态脚本流程
src/adapters     具体领域实现、DotNet 动态模块和反射序列化
src/composition  默认组合、共享 Shell/Player、Desktop/Browser、Editor
native           单组件 facade、共同 BGCS 定义、宿主和目标生成物
build            CLI、Tasks、Pipeline、Managed、Toolchains、Support Packs
tools            架构、公开边界、文档和源码排版验证库
```

发布平台、托管部署、平台宿主、原生工具链和领域实现分别组合。
动态模块和静态目录进入同一 TypeCatalog/Registry；共同服务不判断浏览器，也不自行构造反射泛型。
生成工厂与序列化元数据支持静态代码闭包，发布不依赖 collectible loader 或运行时编译器。

职责细化为两个真实项目：`Inno.Extensibility.Catalogs` 持有无引擎依赖的目录事实和注册协议，
`Inno.Adapter.Serialization.DotNet` 持有创作态反射；这些变化已记录在完整计划和项目 Wiki。

生产入口保持最少的实际宿主：

- InnoEngine 四个 Program：统一 Build CLI、Editor、Desktop Player、Browser Player。
- BGCS 一个生产 Program：`BGCS.Tool`。组件构建、验证和生成流程均是库。
- 示例和测试消费者拥有自己的运行入口，不属于生产构建入口。

通用 C# 规范保存在 `CSHARP_DEVELOPMENT_STANDARD.md`，BGCS 的独立副本内容一致。
两仓库配置 `.editorconfig`；复杂声明排版由源码验证器执行。
AGENTS 的源码目录与 Rendering 项目名称已同步当前结构。

## 必要公开边界

| 边界 | 稳定语义及必要性 |
| --- | --- |
| `IModuleSource`、`ModuleCatalogContribution` | 共同目录接收模块贡献，来源承担准备和释放，宿主管理候选事务 |
| `ITypeCatalogSource`、生成注册协议 | 动态反射和静态元数据进入同一发现/工厂机制，不向共同业务泄漏 ALC |
| `GameCodeDeployment`、`IPlayerModuleActivator` | 根据模块身份和内容身份验证逻辑代码部署，静态激活不要求 DLL 目录 |
| `IManagedDeploymentCompiler`、request/result/capabilities/catalog | 冻结代码闭包、发布和链接可替换，运行时选择不进入平台打包逻辑 |
| 各领域 backend ID/provider/catalog | 开放发现与能力校验；未知、重复或不支持的实现明确失败 |
| `NativeArtifactPublisher` | 目标与指纹产物的协作所有权、输入校验、失败保留和完整提交 |
| `ToolchainWorkingDirectory` | 同一公共 lease 管理原生与托管工具的执行路径，Windows 短 alias 不改变物理 owner |
| `SerializationRegistry.GetMetadata` | 值访问使用已组合的生成元数据；动态反射保留在所属 Adapter，不导出为脚本 API |

完整签名、异常和所有权由当前公开 XML 与各项目 Wiki 管理；此表不将 internal helper 提升为稳定 API。
BGCS 的目标 provider、冻结 Binding/Bridge IR 和互操作 API 变化见其独立报告。

## 本轮修正

- Native 每组件只有一个项目；宿主绑定与目标绑定分别发布。目标 C 桥和 managed source
  使用同一 generation bundle，缺失/变动的选择清单在编译前失败。
- 组件 native 产品和 Support Pack 按目标及内容指纹不可变保存；取消和失败保留上次完整输出。
  缓存命中检查实际文件集合、长度和 bytes，拒绝 owner tree 内的链接。
- Windows 短期共享/访问冲突在 IO 发布边界有界重试；永久失败保留原异常和旧树。
  Plugin materialization 复用 Core.IO，并验证缓存精确内容及并发相同包的完整快照。
- Text/UI 工具链声明实际 CMake、include、src、Generated 输入，避免把生成器锁文件算成原生源码。
  相同绑定重新生成后产品目录完全一致；没有引入针对 BGCS 文件名的过滤协议。
- 托管与原生工具共用短执行路径。真实 Mono AOT 长路径失败经过最小复现定位；
  SDK 从原工程解析，并由实际发布测试确认移动执行路径仍使用所选 SDK。
- Desktop 共用模板保留外置 native closure，发布前后逐项校验；CoreCLR 单文件不吞掉声明的原生部署。
- 静态序列化访问、泛型工厂和日志调用位置使用中立元数据。裁剪和 AOT 发布严格检查警告，
  不使用整批 `TrimmerRootAssembly` 或共享层 IL 警告抑制兜底。
- BGCS 按值枚举统一转为声明的整数 ABI carrier，适用于所有目标和三种 import mode。
  共同生成规则启用 `DisableRuntimeMarshalling`，不在各 native 组件重复 assembly attribute。
- FlappyBird 使用当前 Canvas/Rendering2D `.iplugin`。旧安装包包含已修正前的光照采样逻辑；
  本轮重新导出并安装当前消费方，再执行四条真实发布路径。

## 终版 Gate

| Gate | 结果 | 证据 |
| --- | --- | --- |
| 完整 Release Solution | 零托管警告、零错误 | `results/solution-plan-audit-final.log` |
| 全部引擎测试 | 51 个项目，1566 项通过、零失败，16 项 Metal 路径跳过 | `results/full-tests-plan-audit-final.log`、`results/full-tests/`、`results/test-matrix-summary.json` |
| 架构、公开边界、XML、声明排版 | 通过，零违规 | `results/architecture-plan-audit-final.log` |
| Native 生成/导入/真实原生 | 七个绑定组件、八个产品、七个测试项目通过 | `results/native-verification-current-final.log`；仓库 `artifacts/bindings/acceptance/windows-x64-msvc/report.json` |
| Text/UI 原生缓存复用 | 相同生成前后产品目录一致 | `results/native-cache-reuse-final.json`、`results/engine-cache-reuse-final.log` |
| Plugin 缓存和并发 | 34/34 专项通过，全矩阵复验 | `results/plugin-materialization-final.log`、`.trx` |
| 原生 ImGui 布局和事件 | 12/12 通过，全矩阵复验 | `results/imgui-layout-native-final.log`、`.trx` |
| 工具执行路径/发布约束 | 5/5 专项及所选 SDK 实际发布回归通过，全矩阵复验 | `results/toolchain-directory-publication.log`、`results/sdk-alias-selection.log` |
| Canvas 消费 | 当前源码编译成功，6/6 集成测试通过 | `results/canvas-consumer-current-final.log`、`.trx` |
| Rendering2D 消费 | 当前逻辑 namespace 脚本编译与 Plugin 导出通过 | `results/rendering2d-scripts-final.log`、`results/rendering2d-plugin-final.log` |
| Wiki 相对链接 | 252 页，零失效相对链接 | `results/markdown-links-plan-audit-final.json` |
| 完整 Plan 文件归属 | 456 项，零未解释缺项；共同边界逐项记录 | `results/plan-completion-audit.json` |
| 通用规范一致及外部源码未改 | 两份规范/配置 SHA-256 一致；五仓库 extern 零改动 | `source-state.json`、`results/workspace-integrity-final.json` |

测试跳过项是当前 Windows 不具备的 Metal shader 编译路径；逐项名称记录在测试汇总。
Editor 测试覆盖 reload 成功/失败、强引用残留、GC barrier、Faulted gate、Missing/History 和 Session 隔离。
原生 ImGui 测试实际提交布局、鼠标和窗口帧，检查 2:3 换行、不同 zoom、
小下拉无滚动、大下拉向下及 45% 上限、搜索宽度和前景窗口命中。
PlayMode/Interactions/Shell 测试覆盖 GameView 焦点筛选、失焦释放、Modal 与共同输入生命周期。

电脑操作曾被 Escape 停止，之后只运行 CLI 和独立后台验证；
不将这些契约/原生帧测试写成完成了 OS 浮动 viewport 的人工交互验收。

## FlappyBird 四条实际发布路径

项目均为 `InnoEngine.Samples/FlappyBird`，当前安装包身份位于 `results/installed-plugins-current.json`。

| 路径 | 实际运行 | 证据 |
| --- | --- | --- |
| Windows CoreCLR | 隐藏 D3D11，120 帧、2 views、25 draws，退出码 0 | `results/flappy-coreclr-current-plugins.log`、`results/flappy-coreclr-current-plugins-run.log` |
| Windows NativeAOT | 严格静态发布，隐藏 D3D11，120 帧、2 views、25 draws，退出码 0 | `results/flappy-nativeaot-current-plugins.log`、`results/flappy-nativeaot-current-plugins-run.log` |
| Web 解释执行 | 10 项真实行为通过；107 个发光星星像素；分数 1；音频峰值 1.037 | `flappy-web-current-plugins-run-fast/report.json`、同目录 PNG；`results/flappy-web-current-plugins.log` |
| Web AOT | 87 个程序集实际 AOT；同样 10 项行为通过；109 个星星像素；分数 1；音频峰值 0.840 | `flappy-web-aot-current-plugins-run/report.json`、同目录 PNG；`results/flappy-web-aot-current-plugins.log` |

Web 检查包括昼夜画面、指针切换/开始、键盘飞行及得分、音频样本、最高分写入与页面重启后读取、
光照和鸟的屏幕位置同向移动、焦点键盘操作。
鸟的垂直运动分别为 124/126 像素，光照中心最大偏差 5.92/3.67 像素；没有脚本异常或游戏错误面板。
浏览器持久值使用当前 Storage Adapter，实际 key 为
`inno-storage:/persistent/flappybird/Storage/flappy-bird/best-score.txt`。
这是 Adapter 的浏览器键；文件系统持久根由 Application identity/options 配置，不加入 `InnoEngine` 产品目录。

发布后最后的行为修改只涉及 authoring 的目录发布、native 输入声明及规范入口。
最终归属核对另外移动部署文件、收口 Foundation 内部 namespace，并将 BGCS 五个 provider 移至
`Build.Providers`；这些改动没有改变运行算法、原生 C++ 或绑定 ABI。
各已运行部署保留自己的冻结 References/内容；不从后续工作区动态读取程序集。
`results/runtime-evidence-audit.json` 校验四条游戏路径以及三个正向、两个负向独立 ABI 报告。

## BGCS 独立验收与联调

BGCS 独立报告：[架构重构验收](../../../BindGen-CS/docs/architecture-refactor-acceptance.cn.md)。

- 八个生产项目；11 个测试程序集，1018 项通过，零失败、零跳过。
- 架构/style/XML、真实五个 C 库和 bimg C++、API snapshot、独立 package 消费通过。
- NativeAOT、Wasm 解释执行、Wasm AOT 各 56 项真实调用，覆盖三种 import mode。
- NativeAOT 和 Wasm 错误注入均在实际 scalar 调用处按预期失败。
- 10000 声明冷生成 14 秒、缓存生成 5 秒；无已知 NuGet 漏洞。

BGCS fixture 与报告属于其自身，不使用引擎组件或游戏验证冒充库的独立支持。
本仓库的 Native 确定性、原生产品及四个 Player 路径另外证明实际消费者联调。

## 删除与清理

- 动态加载器从 Foundation 移至 DotNet Adapter，删除共同序列化的反射泛型构造路径。
- 构建和验证收口为库与唯一 CLI；删除被替代的独立工具入口及旧可变 native/support 输出选择。
- 目标产物不覆盖宿主生成物；缺少目标声明明确失败，不再用宿主绑定 fallback。
- BGCS 删除 Core→AST、IR `Compile Link`、`EmitAst`、空预处理/缓存入口、重复编排和验证 Program。
- Canvas/Rendering2D/FlappyBird 的引用、脚本及当前 Plugin 包同步；可重建生成示例和锁文件不纳入源码。
- 未增加旧 API alias、旧 schema reader、测试后门或第三方 `extern` 修改。

逐文件改动和删除列表可由 `source-state.json` 与两仓库当前 diff 审阅。生产 Program 清单由实际文件确认。

## 重现入口

在引擎根目录，以工程选择的 dotnet 执行：

```powershell
$env:DOTNET_ROOT = 'C:/Users/23842/AppData/Local/InnoWebDotnet'
$env:DOTNET_HOST_PATH = "$env:DOTNET_ROOT/dotnet.exe"
$taskDotnet = 'C:/Users/23842/AppData/Local/InnoWebDotnet/dotnet.exe'
$taskCli = 'build/cli/Inno.Build.Cli/bin/Release/net9.0/Inno.Build.Cli.dll'
& $taskDotnet build build/cli/Inno.Build.Cli/Inno.Build.Cli.csproj --configuration Release --disable-build-servers -m:1 -nodeReuse:false
& $taskDotnet $taskCli verify-native --configuration Release --dotnet $taskDotnet
& $taskDotnet $taskCli engine --target windows-x64 --configuration Release --dotnet $taskDotnet --output artifacts/support-packs
& $taskDotnet build InnoEngine.sln --configuration Release --disable-build-servers -m:1 -nodeReuse:false
& $taskDotnet $taskCli verify --configuration Release
```

完整测试命令、部署原生文件指纹和逐项目退出码保存在 `results/full-tests/report.json` 与 `native-input.json`。
`run-engine-tests.py` 顺序执行 51 个项目，并向各测试宿主安装已验证的精确 native closure；
运行时没有仓库目录探测 fallback。
`check-browser-game.py` 运行独立后台浏览器并在结束后关闭自己的 browser/server。
BGCS 独立矩阵与三种部署/错误注入命令在它自己的验收报告中。

## 验证限制与后续平台

- macOS 采用当前共用 Desktop composition/发布模板和独立 Platform.MacOS；本轮没有 macOS 实机。
- iOS、主机、未来 CoreCLR WebAssembly 按完整计划列出具体接入节点，未创建占位实现或声称已支持。
- 后台软件 WebGL 验证功能，不证明硬件 GPU 性能或真实页面冻结/恢复；共同生命周期由契约测试覆盖。
- 浏览器日志保留第三方图形能力探测 `INVALID_ENUM`、MiniAudio `ScriptProcessorNode` 弃用提示、
  软件驱动提示及资源 404；因此不宣称控制台零警告/零消息。它们未导致上述玩法检查失败。
- native 冷构建保留 HarfBuzz/CMake 和 MSB8029 Temp alias 等上游警告。
  表格的零警告结论仅针对所列托管编译，不包含这些原生供应商消息。
- 未运行远程 CI、签名或发布，未提交代码。指定 Glass.aiff 在此 Windows 主机不存在，提示音未播放。
