# 平台、运行时与构建分层

[架构索引](README.md) · [Wiki 首页](../README.md) · [完整执行计划](PLATFORM_RUNTIME_REFACTOR_PLAN.md) · [Web 启动](WEB_PLAYER_ARCHITECTURE.md)

本文按当前源码说明运行、内容与代际契约。平台归属以[当前总览](ENGINE_ARCHITECTURE_OVERVIEW.md)和[扩展指南](PLATFORM_EXTENSION_GUIDE.md)为准；本轮实际 gate 另见平台归属验收。未来接入不能视为已实现或实测。

## 1. 五个独立选择

| 选择 | 归属 | 当前入口 | 负责的差异 |
| --- | --- | --- | --- |
| 发布平台 | `platforms/<platform>/build` | `IGameBuildTarget` | 内容约束、输出布局、平台包校验 |
| 托管部署 | `build/managed` 契约 / `backends/DotNet/build` 实现 | `IManagedDeploymentCompiler` | CoreCLR、Mono Wasm 解释执行/AOT、NativeAOT 的实际发布 |
| 平台宿主 | `platforms/<platform>/player` | 明确平台 composition | 启动、下载、系统回调、帧调度与平台生命周期 |
| 原生工具链 | `build/toolchains` 机制 / 平台 SDK / backend recipe | 冻结工具链与组件描述 | compiler、sysroot、ABI、生成身份、编译与链接 |
| 领域实现 | `backends` 与平台 runtime | 各领域 provider catalog | 窗口、输入、存储、图形、音频、文本、UI 和 presentation |

`browser-wasm` 是原生目标和发布平台的选择；它不表示生成器只能在 Windows 运行。
所用托管运行时另由部署 ID 决定。更换托管运行时不需要把领域服务改成另一套实现。
具体后端能否在新平台复用，必须由 SDK、能力和实际测试确认。

## 2. 源码归属与依赖

```text
src/foundation    基础值、事件、身份、模块目录、类型目录和代际事务
src/content       Asset、引用、Scene 与 Animation
src/services      后端中立的领域契约及运行服务
src/runtime       引擎组合、部署内容、运行子系统、创作态脚本编译/reload
src/adapters      中立 SPI、provider catalog 与 EventInput
src/composition   默认引擎、Shell、Player 和 Editor 组合入口
backends          共享具体实现、Native facade、BGCS 和组件 recipe
platforms         系统/SDK、产品启动、布局和 Support Pack
build             CLI、Task、Pipeline、Managed、Toolchain、Support Pack
tools             架构与源码验证库
```

平台组合入口向共享宿主注入 Adapter、模块来源、类型来源和帧驱动。
共享运行时依赖领域契约及 Foundation。具体 Adapter 可以引用自己的 Native 组件；
原生句柄和第三方类型不能进入领域 public/protected 或脚本 API。
Build 不通过 Editor Panel 执行任务，Panel 只提交请求并展示进度。

## 3. 模块来源和类型发现

### 目录事务

`IModuleSource` 声明模块名、domain、scope、是否可退休、上游模块和程序集作用域。
`GetAssemblyNames()` 先提供完整身份，`Prepare(ModuleSourceContext)` 获取尚未发布的贡献。
`ModuleHost` 负责依赖顺序、候选校验、目录快照与统一事务。

动态来源属于 `Inno.Adapter.Modules.DotNet`：collectible ALC、shadow copy、依赖解析及弱监测都在此层。
静态来源为 `Inno.Runtime.StaticModuleSource`，接受已经链接的非 collectible 程序集，进入同一目录机制。
静态模块的代码由进程持有，不能伪装成可以卸载的动态 generation。

### 类型与工厂

`ITypeCatalogSource` 提供完整类型集合、发现元数据、泛型构造解析、工厂可用性和实例创建。
共享序列化通过该来源解析泛型，不执行 MakeGenericType。动态构造归 DotNet Adapter，
静态构造来自生成的完全封闭工厂目录。

目录的基础事实及 `ITypeCatalogRegistrar` 属于无引擎依赖的
[`Inno.Extensibility.Catalogs`](../extensibility/Inno.Extensibility.Catalogs.md)。
参与类型发现的生成目录直接依赖该叶契约；纯 Native 绑定不生成扩展目录，也不依赖此契约，避免 Core 基础程序集反向依赖 ModuleHost/TypeRegistry。
声明与泛型工厂分开注册；工厂不会重复进入扩展发现或 Stable Type ID 索引。
注册器只在组合线程开放，并在成功或失败结束时封闭。
反射来源与生成目录进入同一 `TypeCatalog` 和 Registry；共同业务不决定使用哪一种来源。
生成入口由所属程序集提供，实际 Player 代码闭包组合这些目录。
序列化 owner 和生成访问代码归对应类型，不通过扩大可见性或测试后门访问私有状态。

### 创作态编译与激活

`ScriptCompilationResult.moduleDeployments` 是中立编译产物：模块身份、程序集位置、作用域和依赖。
它不携带 DotNet 来源、ALC 或 collectible 配置。
`ScriptReloadHost` 接收 composition 提供的 `Func<ScriptModuleDeployment, IModuleSource>`；
Editor 和 Build 工作区在各自组合入口创建 DotNet 来源。
编译器、共享 reload 协调器和 Build Pipeline 因此不引用具体模块加载 Adapter。

编译成功、候选激活成功与旧代成功退休是不同阶段。
退休必须执行 Full GC、等待 finalizers、再次 Full GC，并确认弱监测全部不可达。
超时进入 Faulted；仍 Pending 时禁止下一次 reload、Play 或 Export。
参见 [Identity 与 reload 标准](IDENTITY_REFERENCE_RELOAD_STANDARD.md)。

## 4. Player 与输入生命周期

Desktop/Browser 的 Program 只进入各自 composition。共享 Player 校验逻辑代码部署与内容部署，
再通过共同 EngineHost、RuntimeSession、Shell 和领域服务运行。
静态模块激活不要求目录中存在动态 DLL；它校验构建模块目录与部署清单一致。

Polling 与外部回调调度通过帧驱动边界进入同一 Shell。
Shell 的显式状态由 Ready 进入 Running，在应用暂停或选中的主窗口隐藏策略下进入 Suspended。
平台服务的窗口接口只表达逻辑窗口语义；原生呈现使用 Adapter SPI `INativeWindowSurface` 与开放 `PlatformNativeHandleId`，不进入 Shell、Player 或脚本契约。
独立暂停原因全部解除后恢复，暂停时停止产品时钟并继续处理退出与平台事件。
有效状态通过 Core Events 通知 Session，音频保留用户 pause 值及仍在使用的 mixer generation。
浏览器下载、JS 回调和页面生命周期属于 Browser composition，不能扩散为玩法或领域分支。
平台输入进入 `Inno.Core.Events`，Input Runtime 按同一协议处理。
Editor Play Session 根据 Game View 的焦点与前景交互状态筛选事件，失焦释放按键和指针状态。
停止时先拒绝新工作，取消并退休异步任务，注销回调，提交存储，最后释放领域与原生资源。

## 5. 开放 Adapter

各领域使用稳定 ID、provider 与不可变 catalog。默认组合提供内置实现；显式空集合表示不安装实现，
不能静默回退。未知 ID、重复 provider、缺失能力和 provider 返回空上下文都明确失败。
Presentation 同样使用 `PresentationBackendId`、`PresentationBackendProvider` 和 `PresentationBackendCatalog`。
具体 ImGui 实现由默认创作组合注册，核心不通过封闭 enum/switch 决定后端。

## 6. 原生生成与发布所有权

```text
native/Inno.Native.<component>/
  Native/include + src                   手写窄语义 facade 和第三方适配
  Bindings/common.json                   共同映射及 ownership
  Bindings/bindgen*.json                  当前目标定义
  Bindings/<facade>.bridge*.json          C++ 到 C 的桥定义
  Native/Generated                       宿主生成 C 桥
  Generated/Bindings.cs                  宿主单文件 managed 绑定
  obj/<target>/<generationFingerprint>/
    Native                              目标生成 C 桥
    Generated/Bindings.cs               同一身份的 managed 绑定
    generation-manifest.json            精确文件集合及内容散列
```

`GenerateBindingsTask` 通过 BGCS 公共库入口运行，无第二个生成器进程。
目标桥和 managed source 在共同 staging 内生成，验证完成后提交；失败和取消保留已完成 generation。
缓存身份包括生成程序集、配置、扩展、目标描述、实际编译器、sysroot 和输入内容。
缓存命中仍检查完整文件集合和 bytes，不能仅依赖目录存在。

工具链读取本次请求独占的描述文件，得到桥与 managed source 的明确路径。
MSBuild 编译 Task 输出的 Compile item，CMake 接收显式桥目录。
Browser 工具链的 native 中间态与最终产物按自身构建指纹隔离；Support Pack 从返回产物路径复制。
`BindingSelection.props` 冻结各组件的生成指纹，后续托管构建发现输入变化时失败。
托管引用的 bin/obj 也按目标和 native 指纹隔离。

独占 publication 复用 `Inno.Core.IO.FileLease`，完整目录安装复用 `AtomicDirectory`。
目录提交是协作所有权保护的两次移动，不能宣称对任意未协调读者是一次原子文件替换。
取消等待不影响其他 owner；staging 退出时清理，完成的产物保留。

## 7. 新平台接入

当前代码例子、平台中立范围及未来 iOS 接入步骤见[平台扩展指南](PLATFORM_EXTENSION_GUIDE.md)。
平台差异由边界管理，不表示整个仓库没有平台代码。

| 能力 | 新增或替换 | 继续复用 |
| --- | --- | --- |
| iOS | Player.IOS composition、Apple SDK Toolchain、iOS 托管 compiler、平台 packager/signer | 共享 Player、事件、Registry、内容与领域契约 |
| CoreCLR WebAssembly | 对应 deployment compiler、SDK/link resolver 和 interop 验收 | Browser 系统宿主和经能力验证可复用的 Adapter |
| 主机 | 官方 SDK 宿主、帧驱动、toolchain、packager；需要时增加 Adapter | 共同生命周期和构建契约 |
| 图形后端 | Rendering Adapter、Shader 前端/编译 Toolchain、原生语义边界 | 中立 Rendering、RenderGraph 和玩法 |

iOS、主机和未来 CoreCLR WebAssembly 的项目仍是明确的未来接入设计。
设备/模拟器、线程、GPU surface、后台恢复、回调、存储与签名必须分别验收。
本机 Windows 的通过记录不证明 macOS、iOS 或主机已经通过。

## 8. BGCS 的独立边界

BGCS 是独立的 C/C++ 到 C# 工具链。中立目标描述、Clang provider、冻结 IR、emitter 和互操作 Runtime
各自承担明确职责。Emscripten provider 提供 wasm32 ABI 与 SDK 描述，不包含引擎模块名单或引擎玩法。
BGCS 的测试、示例、Native/Wasm/AOT 调用证据属于 BGCS 仓库。
本仓库的五个组件联调只证明本仓库的消费流程，不能替代 BGCS 的独立验收。

## 9. 验证入口

- `Inno.Build.Cli verify`：源码归属、依赖、公开 API、XML、Editor 引用和解决方案归类。
- 模块/类型/reload 测试：成功、失败、Missing、强引用残留和 GC/Faulted gate。
- Build 与 Core.IO 测试：真实进程退休、SDK 查询、内容身份、取消和发布所有权。
- 生成器测试：生成代码编译、运行、私有 owner 访问和 AOT 目录。
- FlappyBird：Windows NativeAOT、Web 解释执行/AOT 的实际导出和后台运行。
- BGCS 独立测试：托管测试、C/C++ ABI 调用、回调、ownership、Wasm/AOT 及注入错误后的失败报告。

每项结果必须包含本次命令、环境、日志或 TRX 路径；历史报告单独保留。

## Support Pack 的读写所有权

准备阶段在 target lease 下执行 source，产物按内容指纹写入不可变目录。
当前选择使用单文件 `current` 的原子提交；读者选择一次具体目录后持续使用该完整目录。
发布新内容保留旧目录，禁止用固定 target 目录替换正在被编译器或链接器读取的输入。
原生构建产物校验与 Support Pack 指纹枚举复用 Core.IO 的无链接 owner-tree 契约。
Windows 工具链使用短 junction 路径运行编译器；物理 owner 仍是组件 `obj/native/<target>/<fingerprint>`，
构建退出时只删除已验证的 alias，不删除其目标。
托管发布与原生 producer 共用 `ToolchainWorkingDirectory`，避免 Windows Mono AOT 等工具的传统长路径限制。
SDK 从原始工程解析；`FileLease` 串行化同一执行路径，取消等待不删除另一操作的 alias。
工具进程全部退出并排空输出后才释放 alias；代码输入、日志、staging 和最终安装位置仍由原 owner 管理。


## 2026-10-06 内容与职责收口


### 内容、文档、日志与存储

`Inno.Content` 保存可移植 ContentKey、完整目录、长度/哈希与只读读取 lease，只依赖 Foundation。Desktop 与 HTTP 来源共用 Pack reader。Content.index 是唯一保留条目，索引不包含自身；catalog 保存唯一完整 Pack 身份。FileSystem 缓存实际核对全部文件和 SHA-256，不信任完成标记或 mtime。

共享 Player 先读取 owned metadata，再核对代码和内容描述，通过 IPlayerContentSource 准备 store；AssetDatabase、Runtime Session 与运行服务只通过 lease 读取。打开的 artifact/content stream 独立持有读取 pin。修复缓存时发布新 generation，旧 reader 固定旧 generation，最后一个读者退出后才退休旧目录。

Settings 使用 IByteDocumentStore。Editor/Build 注入文件实现，Player 注入从内容 store 捕获的只读 bytes；Player 写 Settings 明确失败。Session 的日志 sink 由宿主 factory 交付并由该 Session 释放。StorageScope 只表示 namespace，FileSystem 的物理根由宿主配置，Browser 将 namespace 映射到 origin。

退出顺序为停止接收工作 → 取消并完成准备任务 → Session 子系统与资源 → Settings → content store → Engine → 外围诊断。内容来源为借用服务，返回的 store 转移给 Player；已打开的 lease/stream 仍可完成读取。

### Rendering 四层

```text
Rendering Core → 必要 Foundation
Rendering Assets → Core + Assets / References
Rendering Shaders → Core + Rendering Assets + 通用 Graph
Rendering Assets.Authoring → Assets + Shaders + Assets Pipeline
Rendering Runtime → Core + Rendering Assets + Runtime Contracts + 中立内容/引用
具体图形 Adapter → Core + Adapter SPI + Native
```

Core 只含图形机制；Material 默认值、Asset reference 和开放 Shader/Technique/Pass 资产协议属于 Assets。创作导入、源码冻结和目标编译属于 Authoring。输出模型、请求、Pipeline/Feature、GPU 解析/缓存/退休属于 Runtime。Player 闭包不包含 Authoring、Assets Pipeline、Shader 创作或编译工具。脚本逻辑 namespace 保持 InnoEngine.Rendering，各程序集自己的唯一清单映射当前 CLR namespace。

### 输入、Presentation 与帧状态

通用事件输入位于 Inno.Adapter.Input，内置 ID 为 inno.input.events；SDL 只负责产生中立平台事件。所有 Session 使用同一 Core Events 管理机制，GameView presentation 负责焦点筛选与前景命中。

设备必须报告真实 nullable primaryPresentationSize：null 是没有可用主输出。无主输出时跳过对应建图和输入坐标换算，离屏请求继续。SDK 内部最小资源不能冒充有效 surface。

Contributor 注册变化时发布 immutable snapshot，帧开始固定；帧中注册/注销对下一帧生效。RenderFrameScratch 保留私有容量，每帧、异常与退休路径清空 extension 引用。Graph Validate 只分析；最终 Compile 复用 revision 分析并完成资源分配，每帧一次。mutation 失败同时回滚图、output、name 和验证状态，已接受 pass 冻结。

### 构建组合与原生身份

Inno.Build.Composition 只承载中立组合契约与完整绑定机制；Inno.Build.Distribution.Standard 是内置平台、所选 integration、managed deployment 和 Support Pack 的唯一发行注册入口。Editor、CLI 和 MSBuild Task 注入各自上下文后消费相同 distribution；通用 Build 不反向引用具体发行。

共同 Task 引导隔离 RID/AOT/Wasm/IDE 属性，一次闭包同一工具身份只准备一次宿主。Native recipe 覆盖实际组件源码、SDK/tool、参数、BGCS 定义/实现和必需 exports；operation 共用初始输入扫描，等锁后及发布前重新验证稳定性。热命中仍校验完整产物内容，内容相同的部署保持 DLL 字节和 mtime。

本轮结构和 API 已按上述职责调整；实际 gate 的当前状态见 [本轮验收](ARCHITECTURE_CLEANUP_ACCEPTANCE_2026_10_06.md)，不使用历史结果替代本轮实测。

## 平台与后端的连接边界

平台基础 build/runtime 不引用 backend；共享 backend 不引用平台/integration。实际 SDK 与 backend 连接归 `platforms/<platform>/integrations/Inno.Integration.<platform>.<backend>`；产品和 Standard Distribution 选择它。IGameBuildTarget 只验证和打包，IGameContentCompiler 由所选 backend 提供，GameBuildContribution 绑定两者；BuildDistribution.CreateBindings 返回完整绑定。

新增 WindowsX86：补 Windows 的目标/SDK/ABI 支持，复用 Windows 产品与 packager，增加真实 BGFX/SDL 接入配置后验收并注册。换图形 backend：增加该 backend 与需要的 integration，替换 compiler/Native plan，平台 packager 不改。NS/iOS：真实 SDK、产品入口与 packaging 归平台包，backend 可复用时直接选择，仅实际差异进入 integration。Browser 换托管运行时只换部署 compiler 和 linker。

Native 步骤显式声明 Static/Shared、有序组件参数和输入 bytes；SDL 应用统一窗口 owner/surface；ImGui 在 NewFrame 前刷新尺度，不维护 WindowsX64 返回 ABI。完整树与测试见[当前批准计划](BACKEND_PLATFORM_INTEGRATION_PLAN.md)，实机状态见[验收](BACKEND_PLATFORM_INTEGRATION_ACCEPTANCE.md)。

## 当前 backend 运行边界

SDL 共用窗口操作，BGFX runtime integration 与 build integration 独立；Default 组合明确接收 rendering factory。共享 Host/Player 不识别具体 surface SPI。Binding provider 属于构建 operation，通过中立接口传递；源码变化按各 fresh verification phase 校验。窗口关闭顺序为交互脱离→渲染命令退休→Task 成功→原窗口 owner 销毁。
