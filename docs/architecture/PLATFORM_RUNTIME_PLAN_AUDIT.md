# 完整 Plan 逐项核对

[架构索引](README.md) · [批准的 Plan](PLATFORM_RUNTIME_REFACTOR_PLAN.md) · [验收报告](PLATFORM_RUNTIME_ACCEPTANCE.md) · [当前架构](PLATFORM_RUNTIME_ARCHITECTURE.md)

本页将批准的执行规格与当前实现、实际证据对应起来。目录树中的未来节点、生成物和产物不能作为
尚未实现的生产源码；同样，测试通过不能代替没有执行过的平台实机验收。

## 实现与验证对应

| Plan 要求 | 当前实现 | 验证入口或终版证据 |
| --- | --- | --- |
| 五个独立维度和依赖方向 | `build/pipeline`、`build/managed`、Player composition、`build/toolchains`、`src/adapters` | `Inno.Build.Cli verify` 的依赖图、循环、概念层和 Player closure 检查 |
| 通用 C# 规范、显式 using、公开 XML、统一排版 | 通用规范、两仓库 `.editorconfig`、源码与公开 API 验证器 | `results/architecture-current-final.log`；规范和配置内容散列一致 |
| 完整结构、项目归类和 Wiki | 当前 Overview、完整 Plan、项目页及分类索引 | Solution 构建、架构验证、相对链接检查 |
| 中立模块来源和目录事务 | `IModuleSource`、`ModuleCatalogContribution`、`ModuleHost`、目录 participant | Modules 的实际加载、依赖、候选失败、回滚与退休测试 |
| ALC、shadow copy 和动态反射归 Adapter | `Inno.Adapter.Modules.DotNet` 的 Loading、Discovery、Reloading | Player closure 禁止动态 Adapter；共享 Foundation 无 ALC |
| 共同类型发现与静态工厂 | Catalogs 叶契约、`ITypeCatalogSource`、静态/反射来源、TypeCatalog/Registry | `StaticGenericFactoryTests`、`RuntimeModuleCatalogGeneratorTests`；生成目录编译并运行 |
| 静态序列化和 AOT 诊断 | Serialization generators、生成元数据与静态泛型目录；反射归 Serialization.DotNet | 生成器负向诊断、实际 NativeAOT/Web AOT 发布；没有整批 trimmer root |
| 逻辑代码部署与静态激活 | `GameCodeDeployment`、静态 Player activator、模块/内容身份 | `StaticDeploymentTests`、实际四条 FlappyBird 发布路径 |
| 共同 Shell/Player 生命周期 | 平台 composition 注入来源、Adapter、帧驱动和生命周期事件 | `ShellLifecycleTests`、`ShellFrameDriverTests`、Input/Storage/Audio 契约测试 |
| 开放领域后端 | 各领域 ID、provider、不可变 catalog；默认组合显式注册 | Adapter catalog 的重复、未知、缺失能力与失败构造测试 |
| 单 Native 项目与窄 facade | 八个 Native 项目；共同 BGCS 定义、手写语义 facade、独立生成根 | 七个 binding component、八个 native product、七个 native 测试项目 |
| 目标与生成身份隔离 | 目标生成 bundle、manifest、BindingSelection；组件 toolchain 的 `obj/native` | Native 验证与 Build 的生成身份、缓存篡改、并发和部署测试 |
| 统一 Build 与最少生产 Program | 一个 Build CLI；Tasks、toolchains、platforms、support 为库 | 生产入口清单和 architecture gate；BGCS 只有一个生产 CLI |
| 托管部署与平台打包解耦 | CoreCLR、Mono Wasm、NativeAOT compiler 与公共 request/result/capabilities | `ManagedDeploymentTests`、真实 SDK 发布、四条实际游戏路径 |
| SDK、工具执行和取消 | 工程 SDK resolver、executor、共同 `ToolchainWorkingDirectory` lease | 实际 SDK 选择回归、进程输出/取消/退休测试、Windows 长路径复现 |
| Support Pack、缓存和最终输出保全 | 不可变 pack/product、精确内容校验、staging 与共同 Core.IO 发布 | `NativeArtifactPublicationTests`、`SupportPackPublisherTests`、Core.IO 与 Plugin 并发测试 |
| Editor UI 显示和事件 | 2:3 换行、单滚动 owner、向下有界 selector、前景命中、GameView 输入筛选 | 12 项实际原生 ImGui 帧测试、PlayMode/Interactions/Inspection 测试 |
| Reload、Missing、History、Session 隔离 | 共同 generation transaction、弱 probe、Full GC/finalizer/Full GC、Faulted gate | Modules/Reload/Scripting/PlayMode 的成功、残留引用、失败和隔离测试 |
| Sample 异步导入与取消 | 后台候选复制/编译，owner-thread 提交及任务退休 | FileBrowser/Scripting/Plugin 集成回归；不在 Panel 同步等待编译 |
| 消费方和实际游戏行为 | 当前 Canvas/Rendering2D、Samples FlappyBird 及当前安装包 | Canvas 6 项、脚本/Plugin 消费；昼夜、星光、光照方向、输入、音频和持久化检查 |
| BGCS 独立架构与验收 | 自身 target provider、AST 边界、冻结 IR、emitter、Runtime 和 fixture | [BGCS 独立报告](../../../BindGen-CS/docs/architecture-refactor-acceptance.cn.md)；不以引擎联调替代 |
| 清理旧路径、入口与无用协议 | 删除 AST 输出旁路、重复工具 Program、旧 Native 目标项目与旧 loader 归属 | 架构验证、源码搜索、Source 状态清单；没有 legacy alias 或测试后门 |

证据路径从 `artifacts/acceptance/2026-10-04-refactor/` 起算；命令、环境和限制见验收报告。
本次最后的文件归属核对另有 `results/plan-file-moves.json` 与 `results/plan-completion-audit.json`。

## 文件树与实际职责的对应

批准的 Plan 同时要求最少公开入口、消除重复协议、Foundation 持有共同代际事务，并允许既有业务文件
按领域保留。以下节点因此采用实际的共同边界，未为了满足名称创建空类或转发 API。

| Plan 中的节点 | 当前归属与原因 |
| --- | --- |
| DotNet 的 `AssemblyReloadSession / AssemblyReloadContext` | 留在 Foundation Modules 的 `Reloading`。它们只协调共同目录 publication，不操作 ALC；DotNet 的 lifetime 和弱 monitor 才属于 Adapter。静态来源使用同一事务。 |
| `DotNetModuleSourceOptions / AssemblyLoadRequest` | `DotNetModuleSource` 本身声明加载请求；产物根由 `ModuleSourceContext` 提供。没有第二份须同步的 options/request。 |
| `InputBackend / Sdl3InputBackendFactory` | 封闭选择被开放 `InputBackendId / InputBackendCatalog` 替代；SDL 具体实现是 `Sdl3InputBackendProvider`，共同 factory 由 catalog 实现。 |
| `ModuleSourceTests / CatalogTransactionTests` | 实际来源和目录事务在现有 `AssemblyManagerTests.cs` 的 `ModuleHostTests` 与 `ModuleRetirementTests` 中验证，包含成功、回滚、依赖和退休失败。 |
| `StaticTypeCatalogTests / RegistrationGeneratorTests` | `StaticGenericFactoryTests` 与 `RuntimeModuleCatalogGeneratorTests` 验证目录冻结、线程归属、生成代码编译/运行和负向诊断。 |
| `PlayerLifecycleTests` | 生命周期实际属于共享 Shell；验证文件是 `tests/input/Inno.Input.Tests/ShellLifecycleTests.cs`。平台 Player 使用该共同实现。 |
| `ToolchainResolutionTests / AtomicPublicationTests` | 具体回归分属 `BrowserToolchainTests`、`ToolchainWorkingDirectoryTests`、`DesktopPublicationTests`、`NativeArtifactPublicationTests`、`SupportPackPublisherTests` 和 Core.IO。 |

最终文件归属核对补齐 `Catalog/AssemblyCatalogCoordinator.cs`、`Modules/AssemblyModuleEntry.cs` 和
`Deployment/GameRuntimeManifest.cs / GameRuntimePlugin.cs`。前两者仍为 internal；公开部署类型的
命名空间和数据协议没有改变。BGCS 的对应清单在它自己的文档中维护。

## 原始默认边界

- Windows、macOS、Web 的架构替换已经实现；Windows CoreCLR/NativeAOT、Web 解释执行/AOT 有本机实际发布与运行证据。
- macOS 实机以及 Metal 编译没有本机执行证据；Windows 的 16 项 Metal 测试明确跳过。
- iOS、主机、未来 CoreCLR WebAssembly 是原 Plan 标注的未来接入节点；具体新增位置和接入契约已记录，没有空项目或占位 compiler。
- 原生 ImGui 帧与公开焦点契约通过；OS 浮动 viewport 的人工交互、真实页面冻结和硬件 GPU 性能不据此宣称已经验收。
- 代码没有自动提交；未执行远程 CI、签名或发布。所有权、失败和取消的本机 gate 与外部设备验证状态分别记录。
