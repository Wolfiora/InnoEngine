# BGCS 4.1.0 消费与平台构建验收

[当前验收](../../../docs/architecture/CURRENT_ACCEPTANCE.md) · [问题台账](../../../docs/architecture/CURRENT_ISSUES.md) · [绑定职责](../../../docs/native/BindingGeneration.md)

本轮基线为 InnoEngine `569dde52979104c95e2ba9dba15e5d74225b8f91`，当前分支为 `dotnet10`，环境为 Windows x64。四个消费仓库实施前均无未提交修改。BGCS 源码基线为正式 `v4.1.0` 的 `adbe62f863002015c32fbd734375d12cc9c423b8`；只读取其公开契约，不修改源码、NuGet 内容或生成结果。

本地证据属于 `artifacts/acceptance/bgcs4-2026-10-24/`，不进入产品依赖。以下状态随本轮最终验证更新；没有最终证据的检查不记作通过。

## 实现边界

- 包族统一精确固定 4.1.0；456 份含 BGCS 的锁文件经结构化核对，BGCS 包族全部解析为该版本。其他包自己的版本号保持独立语义。
- 公共契约使用 `BGCS.Core`、`ManagedRuntimeId`、`BindingGenerator.CreateBuilder()` 和显式 host 路径服务；文件请求先经过官方 `MaterializeAsync`，再应用冻结的目标 SDK 与组件输入。
- Browser integration 显式注册官方 BrowserWasm 扩展。共享 Toolchains 不引用 BGCS，不识别 Browser 扩展或平台 ABI 名单。
- MSBuild 私有加载使用公开 frontend factory 与 `WorkerAssetLocator` 定位 SDK 自身部署的 worker，不自行解包或改写 SDK。
- 生成清单的逻辑输入根由引擎映射到冻结复制树；引擎负责独立编译、链接、校验、原子发布、部署和维护。
- Native 生成源使用独立 object target，只取得生成清单声明的公共头、宏和语言要求。生产库的私有构建宏不能进入公共头消费者；桥的宏也不能进入生产库源码。
- BGFX 桌面 GENie producer 通过共同 CMake manifest hook 编译桥接归档，再经独立薄脚本组合上游项目。平台提供依赖引用及保留全部对象的有序链接参数，共享 recipe 只映射到 `links` / `linkoptions`，不判断命名平台。未使用无法保留外部归档对象的 GENie `wholearchive` 路径。源复制复用共同冻结实现，删除重复的 BGFX 递归复制流程。
- SDK 默认 aggregate pointer bridge 由真实调用方消费。BGFX 后端和 Editor 内部只在同步调用范围借用栈值地址，不保留指针，不增加平台条件或公开原生 API。能够复用的 Panel 绘制改用现有 `ImGui.Draw*`。
- 缓存身份分别保存配置及插件来源快照、实际 SDK 实现、扩展、worker 资源与解析输入。绑定契约通过明确源码闭包参与实现身份，不把整个 Toolchains 程序集的无关改动计作绑定变化。
- 引擎绑定实现消费构建时生成的内容身份，包含其源码闭包、实际 Roslyn 编译器及相关编译选项；不使用受独立 bin/obj 输出位置影响的引擎 DLL 字节作为该实现身份。SDK 与扩展二进制仍按实际内容核验。

## 实际修改位置

```text
build/
  msbuild/Inno.Packages.props
  bindings/Inno.Build.Bindings/
    Configuration/{BindingGenerationConfiguration,NativeBindingProfile}.cs
    Generation/{BindingHostFactory,BindingHostExecution,BindingNativeCompilation}.cs
    Identity/{NativeBindingGenerationIdentity,BindingInputPlanning}.cs
  toolchains/Inno.Build.Toolchains/Native/NativeBindingCMake.cs
backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/
  BgfxBindingProject.cs
  BgfxBuildRequest.cs
  BgfxBuildSession.cs
  BgfxNativeBuildProfile.cs
  Native/SharedBindings/CMakeLists.txt
platforms/<platform>/integrations/Inno.Integration.<platform>.Build/
  Bindings/<platform>BindingProfile.cs
backends/<component>/native/Inno.Native.<component>/
  Bindings/bindgen.json
  Generated/Bindings.cs                       # Official generation only.
  Native/Generated/                         # Official sources and consumer metadata.
backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/
  BgfxNativeCalls.cs
  Existing resource and command consumers
src/composition/editor/
  presentation/Inno.Editor.ImGui/
    ImGui.cs
    Runtime/ and Widgets/ consumers
  panels/
    Existing drawing consumers
    Inno.Editor.Panel.ShaderEditor/Canvas/ShaderEditorCanvas.Drawing.cs
tests/build/Inno.Build.Tests/
  NativeBindingProfileTests.cs
  NativeBindingCMakeTests.cs
  Existing generation fixtures
docs/
  native/BindingGeneration.md
  build/Inno.Build.Bindings.md
  architecture/ and affected API pages
```

## 当前已取得的证据

| 范围 | 结果与证据 |
| --- | --- |
| 正式包来源 | 正式 release 与公共包验证清单已读取；本地 Core/Browser 扩展程序集 SHA256 与发布证据相符。130 个包文件与操作前哈希一致，`package-integrity.json` 无变化。BGCS Git 工作区干净，递归检查没有 Agent 类 Markdown 文件。 |
| 源码全集 SourceDebug | 最终源码通过，0 警告、0 错误；`source-delivery-final.log/binlog`，包含配置物化测试的最终修正。 |
| 正式 Build 入口 | 最终链接配置签名下通过，0 警告、0 错误；`cli-archive-retention.log/binlog`。 |
| Windows 七组件 HostSource | 正式生成及最终 `CheckBindings` 全部通过，0 错误；七份 `host-generate-*.log/binlog`、`host-check-*.log` 与 `host-check-results.json`。 |
| Browser 五组件生成 | 通过；`browser-bindings-selected-surface.log`。这只证明生成阶段，不代表产品运行。 |
| 绑定回归首轮 | 17 通过、1 失败；失败 fixture 将输入头与输出放在同一目录，触发输入变化保护。原始 `bindings.trx` 保留。 |
| 新增边界复验 | 2 通过、0 失败；`profile-cmake-final.trx`。包括未注册 Browser 扩展时拒绝、aggregate bridge、热缓存复用及真实 MSVC 下的双向宏隔离。 |
| 扩展绑定回归 | 27 通过、0 失败、0 跳过；`binding-final.trx`。包括冻结 SDK、失败/取消、读取保护、生成一致性、宏隔离与工具闭包；后续内容身份修改的相关复验单独记录。 |
| 内容身份最终回归 | 27 通过、0 失败、0 跳过；`binding-identity-final.trx`。包括独立进程流程；与前一组存在重叠，不相加报告。最终链接配置的 7 项回归通过，`archive-link-final.trx`。 |
| 正式架构 / Wiki | 最终源码下通过；238/238 工程求值通过；33,564 个 API 条目、151 个 API 项目、196 个示例，0 示例错误、0 未执行示例。`wiki-final/validation-delivery.log`、`api-delivery.json`、`examples-delivery.json`。 |
| 架构验证器回归 | 144 通过、0 失败、0 跳过，`architecture-final.trx`。 |
| CLI / Task 内容身份 | 编译内容身份一致，均为 `c6a4892be63aebd158e1e52cc28f0544941928bec31c8e0f290dbf59fb4e3d6e`；`compiler-identity-cross-host.json`。一次 Editor 复编记录 Native processes=0、bindingGenerations=0、materializedBytes=0；不是十次热构建验收的替代。 |
| BGFX 导出集合 | 平台显式保留归档后，最终 DLL 有 409 个导出；202 个必需桥接符号全部存在，缺失数 0。`bgfx-export-conformance.json` 和 `bgfx-retained-exports.log`。 |
| 最终普通/隔离测试 | 有效去重结果为 2,141 通过、0 失败、18 项平台跳过，共 2,159 项；59 项隔离发现、28 次隔离执行全部通过。初次全量的失败记录保留，最终完整工程复验为 357 通过、0 失败、2 项 Unix 跳过；范围及合并方法见下文。 |
| Windows 产品 | Editor Debug/Release、CoreCLR Player、NativeAOT Player 分别在 D3D11、D3D12、Vulkan 下完成 3,000 帧并正常退出，共 12 项通过；Player 均报告 views=2、draws=25。见 `editor-retained-results.json`、`game-results.json` 及对应运行日志。自动维护超过 5 秒预算的明确延期警告保留，0 编译错误；首次缺符号失败与取消日志同样保留。 |
| Browser 产品 | 暂缓。生成已成功；未修改的 SDK 生成源在系统匿名 union 存储断言中包含非法 C。独立 SDK 工程已复现，见[复现及 SDK 修复验收条件](BGCS410_ANONYMOUS_SYSTEM_RECORD_REPRO.md)。 |
| Wiki、API、脚本与 IDE | 正式门禁通过；Canvas、Rendering2D、Samples 各自的正式脚本编译、IDE 投影及 Release IDE 工程编译全部通过。`scripts-*.log`、`ide-*.log`。插件创作源码未改变，无需重新导出安装包。 |
| 十次热构建 | 10/10 通过；原生编译进程、绑定生成和源码复制均为 0，两个 selection 内容及时间戳稳定。总耗时中位数 190.904 秒，Native 准备中位数 105.997 秒，每次 Native 统计读取 4,497,920,815 bytes。`hot-build-summary.json`、`editor-hot-results.json`。有其他验收任务并行，不能用作空闲基线或整体提速结论。 |
| Git 检出完整性 | Native 生成目录保留 SDK 原始字节，显式识别 CRLF 行尾并保留其他空白检查。28 个文件在 autocrlf=true/false/input 下的 84 次检出过滤检查全部一致，21 项生成清单 SHA256 匹配；`generated-native-checkout.json`。没有手改生成文件。 |
| macOS | 无设备，未执行。 |

## 公开构建 API 的必要性

- `NativeBindingProfile` 的 `configureGenerator` 只在 BGCS 接入边界注册官方扩展。注册动作必须不捕获可变状态；有效配置、声明源码与 SDK 扩展参与身份。它不要求游戏或脚本配置平台，也不进入中立 Toolchains。
- `BgfxNativeBuildProfile.SelectTools(context, includeTools)` 区分运行库与离线工具实际消费的工具集合；不把 CMake 或托管 CLI 无条件加进离线工具执行集合。
- `bindingCmakeArguments` 表达平台桥接归档的编译约束，例如与生产库一致的 CRT；`CreateBindingLinkSettings(archivePath)` 返回归档依赖引用及有序保留参数。共享 recipe 不推导文件后缀或选择 Windows/macOS 分支。
- 被替换的单参数 `SelectTools` 和中间 `GetBindingLinkReference` 已删除；全部消费者、fixture、XML 与 API Wiki 使用最终签名，不保留转发包装。

## 深层自查

- **职责和依赖：**发行选择明确平台贡献；平台 integration 注册 SDK 扩展及链接策略；共享 recipe 消费 manifest 和平台返回值。中立 Toolchains 没有 BGCS 类型或 Browser ABI 分支。不是仅调整目录名称。
- **真实输入：**文件请求先通过官方物化；分析、指纹和生成消费冻结的目标与 SDK。逻辑源码根映射到复制树，SDK 输入保持独立 owner；没有仓库根字符串替换或写死磁盘位置。
- **原生隔离：**桥是公共头的消费者，不继承 producer 私有宏。真实 MSVC fixture 验证双向隔离、冻结头及实际调用；BGFX 最终导出集合和六项 Editor 运行验证桥确实被链接。
- **失败和生命周期：**仍通过现有生成事务、完整性记录、协调 lease、取消和原子发布；生成或编译失败不成为有效产物。同步栈值指针不越过调用，未增加长期保存原生借用地址的状态。
- **基础能力复用：**BGFX 复制流程改用共同 `NativeCMakeExecutor`，脚本发布使用 Core `AtomicFile`；Panel 绘制优先复用既有中立绘制入口。本轮没有新增 Events、Identity、Serialization 管理器或第二套缓存协议。
- **验证边界：**测试通过公开 SDK、正式 Task 和真实产品调用，不使用 IVT、私有反射或测试入口绕过。普通测试仅在存在真实 Native 引用时部署产品闭包，纯托管测试不会因此执行 Native 生成。
- **仓库隔离：**BGCS 仓库不得出现 Agent.md/AGENTS.md，包括大小写变体。规则只记录在引擎 AGENTS；BGCS 源码和包文件没有本轮修改。

当前热构建仍有完整 SDK/源码哈希读取和有界维护延期，完整性能对照未闭合。架构门禁和运行通过不等于没有性能改进空间，也不能消除 Browser SDK 阻塞或代替 Mac/设备实测。

`nativeProcesses=0` 的统计范围是工具链启动的原生编译/构建进程，不代表整个操作没有 SDK frontend worker 或 MSBuild/托管工具进程。当前仍执行输入分析和完整性校验，不能把缓存命中描述成零成本。

## 缓存与验收输出

正式 owner 维护使用现有 `clean --scope cache`，64 GiB 预算、20 GiB 空闲阈值及显式 600 秒预算。并发构建期间的一次结果为逻辑 28,944,392,343 bytes、保护 9,440,164,573 bytes、回收 0、跳过 498；见 `cache-space-pressure.log`。不能把临时编译结束后的磁盘空间变化说成维护回收收益。

构建和测试退出后的显式维护成功：逻辑总量 38,668,211,274 bytes，保护 128,881,174 bytes，退休 38,539,330,100 bytes（约 35.9 GiB 逻辑数据），跳过 0；`cache-delivery-final.log`。逻辑字节不等于实际卷空间；该结果不包含下述被拒绝的六个验收目录。

维护后已部署的 Debug Editor 仍能打开同一示例项目，在 D3D11 下推进 30 帧并正常退出；`editor-after-maintenance.log`。未执行源码重建或手工复制依赖，验证产品运行不依赖被退休的构建工作区。之后观察 C 卷空闲约 29.34 GiB、D 卷约 256.56 GiB；该快照不当作精确物理回收量。

另有六个本轮创建、已结束的早期验收输出目录，合计逻辑约 9.7 GiB；检查未发现重解析点、命令行匹配的活动进程或缓存 owner 登记。清理命令被自动审批审查以 `blocked by policy` 拒绝，未执行删除，不重试该动作。目录与检查结果保留在 `inactive-output-cleanup-plan.json`；这部分不计作已回收。

Windows 宿主下显式目标还原新增的 `packages.browser-wasm.win-x64.lock.json` 与 `packages.macos-arm64.win-x64.lock.json` 各 115 份，记录目标作用域中实际宿主工具的 NuGet RID 图。它们不是新平台项目，也不证明 macOS 实机或 Browser 产品已经运行。456 份包含 BGCS 的锁文件全部精确解析 4.1.0，见 `lock-inventory-final.json`。

源码编译通过与局部回归不等于整个产品矩阵通过；最终记录必须列明失败、暂缓和未执行项，以及对应继续条件。

## 完整测试的修正与有效证据

首轮公共入口完成 59 个工程的共享执行、59 项隔离发现和 28 次隔离执行。首轮 2,159 个结果中有 2,140 项通过、1 项失败和 18 项跳过；唯一失败是测试直接读取未物化的文件请求。测试已改用官方 `GenerationRequest.MaterializeAsync`，移除无效的旧 `Config` 节点查询；定向复验通过，生产流程不需要因此改变。

一次工程复验错误地使用 `BuildProjectReferences=false`，没有形成完整运行依赖闭包，造成三个 Wiki 工作流测试缺少 `Inno.Native.UI.dll`。该次 354 通过、3 失败、2 跳过的结果保留在 `build-shared-final.trx`，不作为最终有效结果。验证命令已修正为正常完整项目图，工程构建 0 警告、0 错误，输出确认包含所需程序集；`build-tests-complete-final-build.log/binlog`。完整共享工程复验单独记录，不改生产 Loader，不手工补 DLL。

18 项平台跳过分别为 2 项 Unix 文件链接语义和 16 项 macOS Metal 编译器场景。不是 BGCS 整体排除，也不是通过筛选隐藏失败。初次公共入口的失败状态及日志保留，最终报告分别列出首轮与对应工程的有效复验，不重复累计用例。

完整项目图下的最后一次共享工程复验为 **357 通过、0 失败、2 跳过**，`tests/build-shared-complete-final.trx`、`build-tests-shared-complete-final.log`。按程序集与 TRX test ID 核对，它完整覆盖并替换首轮同一工程的 359 个共享用例；其他共享及全部隔离结果保留。显示名可能被测试框架截断，不能用显示名去重。`test-evidence-final.json` 保存原始计数、替换计数、完整平台跳过清单及最终 2,141/0/18 结果。没有把初次公共入口的非零退出改写为成功。

## 当前结论

本轮 Windows 消费接入已通过源码、构建入口、正式生成、实际链接、普通/隔离测试、架构、Wiki、脚本/IDE、产品和十次热构建的适用验证。BGCS 源码基线不变，Git 工作区干净、Agent 类文件为 0；包内容核验无变化。Canvas、Rendering2D、Samples 工作区没有本轮源码修改。

这不能称为整个引擎已经完美：Browser 受独立复现的 SDK 存储断言问题阻塞；Mac/Unix 和 16 项 Metal 场景没有本机条件；完整性能基线、视觉、DPI、焦点、音频与长期设备验收仍按台账范围开放。本轮没有为这些缺口添加 fallback、兼容路径或伪造通过结果。

提示音文件 `/System/Library/Sounds/Glass.aiff` 在当前 Windows 环境不存在，本次未播放。
