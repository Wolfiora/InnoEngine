> 执行基线：2026-10-04；InnoEngine `50fc7f52`，BGCS `3fc481d`。两仓库开始时工作区干净。
> 本文保存用户批准的完整方案。下列清单只在实现和相应验证完成后勾选。

## 执行状态

- [x] 固化规范、架构检查和基线。
- [x] BGCS：依赖解耦、IR 所有权、开放目标与 Cpp2C IR。
- [x] BGCS：全量规范及独立 Native/Wasm/AOT 验收（1018 项托管测试；三个部署路径各 56 项真实调用；负向与独立 package 消费通过）。
- [x] InnoEngine：模块来源、静态目录、序列化与代码部署。
- [x] InnoEngine：Adapter、统一 Build、Native 与 Support Pack。
- [x] 消费者、FlappyBird、Editor、文档与本机最终验收（51 个项目，1566 项通过；16 项 Metal 路径跳过；其他平台实机状态独立记录）。

## 实施中的职责细化

- 基础类型目录与生成注册协议归独立的 `Inno.Extensibility.Catalogs` 叶契约。生成目录不因此反向依赖模块宿主或运行服务。
- 动态序列化反射归 `Inno.Adapter.Serialization.DotNet`，静态序列化由生成元数据提供；共同序列化不自行构造反射泛型。
- BGCS 的 C# 预处理入口为 `BGCS.Language.CSharp.Preprocessing.CSharpPreprocessor`。
  条件编译规则属于具体语言；C/C++ 继续由 Clang 前端处理，删除了没有预处理行为的通用空入口。
- 目标结构中的 `[未来]` 节点仍是扩展说明，不创建空项目或占位 compiler。
  Windows 的实际结果不代替其他宿主与设备验收。

# InnoEngine 与 BGCS 完整重构计划：分层、文件结构、执行与验收

## 一、目标、结构约定与统一规范

完整落实前面确定的架构，并将下面的详细结构、依赖规则、公开契约、生命周期、平台接入方法和验收矩阵写入两仓库文档。

以下是**重构后的目标结构**。文件名称作为实施约定；既有业务文件按所属领域保留。新增目录随对应实现建立。

标记说明：

- **[新增]**：本次实现。
- **[重构]**：保留职责，调整实现、位置或契约。
- **[生成]**：由生成器产生。
- **[产物]**：可重建，不作为创作源码。
- **[未来]**：记录接入设计，实际接入时建立项目和实现。

### 1. 五个独立维度

| 维度 | 管理内容 | 所属位置 |
|---|---|---|
| 发布平台 | 应用布局、平台约束、打包、签名 | `build/pipeline` |
| 托管部署 | CoreCLR、Mono Wasm、NativeAOT、未来运行时 | `build/managed` |
| 平台宿主 | 启动、帧调度、系统生命周期、平台回调 | `src/composition/player` |
| 原生工具链 | compiler、triple、SDK、sysroot、ABI、链接 | `build/toolchains` |
| 领域实现 | Rendering、Audio、Input、Storage 等具体后端 | `src/adapters` |

依赖方向：

```text
平台组合入口
├─ 共享 Player / Shell
│  └─ 领域 Runtime
│     └─ 领域契约 / Core
└─ 具体 Adapter
   ├─ 领域契约
   └─ Native facade / 生成绑定
      └─ SDK / 第三方库
```

平台差异由所属边界负责。共同领域代码通过契约获得能力。

### 2. 文档结构与完整记录要求

```text
C:\Dev\GameEngineDev\InnoEngine\docs\
├─ README.md                                      Wiki 总入口
├─ architecture/
│  ├─ README.md                                    架构索引
│  ├─ CSHARP_DEVELOPMENT_STANDARD.md                [新增] 通用 C# 开发规范
│  ├─ PLATFORM_RUNTIME_REFACTOR_PLAN.md             [新增] 本完整计划及执行清单
│  ├─ PLATFORM_RUNTIME_ARCHITECTURE.md              [新增] 完成后的稳定架构
│  ├─ PLATFORM_RUNTIME_ACCEPTANCE.md                [新增] 本次验收证据
│  ├─ ENGINE_ARCHITECTURE_OVERVIEW.md               [重构] 全程序集目录与依赖
│  ├─ IDENTITY_REFERENCE_RELOAD_STANDARD.md         同步核对 reload 不变量
│  └─ WEB_PLAYER_ARCHITECTURE.md                    [重构] 当前 Web 构建和启动流程
├─ core/
├─ assets/
├─ engine/
├─ rendering/
├─ editor/
├─ platform/
└─ build/                                          [新增] 构建体系 Wiki
   ├─ README.md
   ├─ Inno.Build.md
   ├─ Inno.Build.Managed.md
   ├─ Inno.Build.Managed.DotNet.md
   └─ 各 Toolchain、Platform、Support Pack 项目页
```

每个架构节点记录：

1. 职责、所属层及允许依赖。
2. 主要文件、公开入口及扩展点。
3. 创建、使用、暂停、退出和失败时的所有权。
4. 输入、输出、生成物及缓存位置。
5. 当前平台组合方式。
6. 新平台接入方式。
7. 测试入口及完成标准。

### 3. 通用 C# 规范

通用规范总结 InnoEngine 的现有要求，作为以后 C# 项目的独立标准：

- 类型、方法、枚举项使用 PascalCase；属性、参数、局部变量使用 camelCase；接口以 `I` 开头；私有字段使用 `m_`。
- 常量使用 `C_`，静态只读成员采用清晰的语义名称。
- 文件名对应主要类型；目录表达职责；命名空间遵循项目规则。
- 成员按常量与静态成员、字段、构造函数、公开成员、受限成员、私有实现组织。
- 多参数声明严格采用 AGENTS 中的逐参数换行及 `) {` 排版。
- 短声明保持完整；复杂声明、调用、条件和集合按语义分行。
- 显式普通 `using`；禁止 global using、隐式导入及 MSBuild `Using`。
- 公开和可重写成员具有完整英文 XML，解释行为、所有权、异常及返回语义。
- 最小必要公开 API，组合优先，依赖由明确边界传入。
- 明确资源 owner、线程归属、取消、回调注销和最终释放。
- 热路径避免重复扫描、完整复制、重复编码及无必要分配。
- 测试通过公开契约和可替换边界验证，禁止测试后门。
- 当前 API 修改同步消费者、文档和测试，删除被替代的实现。

外部接口实现遵循原始签名。生成绑定命名由生成配置决定。

BGCS 独立采用通用规范。InnoEngine 的 Identity、Scene、Serialization、Editor、Rendering 等领域协议继续在引擎规范中管理。

纯排版、命名修改和行为重构分阶段进行；纯排版阶段核对语法 token。

## 二、InnoEngine 详细目标结构

### 1. 完整源码分层与项目归属

```text
C:\Dev\GameEngineDev\InnoEngine\
├─ InnoEngine.sln
├─ AGENTS.md                                       同步规范及架构要求
├─ .editorconfig                                   [新增] 可自动表达的通用规则
├─ Directory.Build.props                           共同编译设置与产物根
├─ Directory.Build.targets                         薄集成入口
├─ src/
│  ├─ foundation/
│  │  ├─ core/
│  │  │  ├─ Inno.Core.Collections/
│  │  │  ├─ Inno.Core.Coroutines/
│  │  │  ├─ Inno.Core.Diagnostics/
│  │  │  ├─ Inno.Core.Events/
│  │  │  ├─ Inno.Core.Execution/
│  │  │  ├─ Inno.Core.Graphs/
│  │  │  ├─ Inno.Core.Identity/
│  │  │  ├─ Inno.Core.Input/
│  │  │  ├─ Inno.Core.IO/
│  │  │  ├─ Inno.Core.Jobs/
│  │  │  ├─ Inno.Core.Layers/
│  │  │  ├─ Inno.Core.Logging/
│  │  │  ├─ Inno.Core.Mathematics/
│  │  │  ├─ Inno.Core.Serialization/
│  │  │  ├─ Inno.Core.Serialization.Generators/
│  │  │  └─ Inno.Core.Settings/
│  │  ├─ extensibility/
│  │  │  ├─ Inno.Extensibility.Modules/
│  │  │  ├─ Inno.Extensibility.Types/
│  │  │  └─ Inno.Extensibility.Reload/
│  │  └─ scripting/
│  │     └─ Inno.Scripting.Api/
│  ├─ content/
│  │  ├─ assets/
│  │  │  ├─ Inno.Assets/
│  │  │  └─ Inno.Assets.Pipeline/
│  │  ├─ references/
│  │  │  └─ Inno.References/
│  │  ├─ scene/
│  │  │  ├─ Inno.Scene/
│  │  │  └─ Inno.Scene.Assets/
│  │  └─ animation/
│  │     ├─ Inno.Animation/
│  │     ├─ Inno.Animation.Assets/
│  │     └─ Inno.Animation.Runtime/
│  ├─ services/
│  │  ├─ platform/
│  │  │  └─ Inno.Platform/
│  │  ├─ input/
│  │  │  ├─ Inno.Input/
│  │  │  └─ Inno.Input.Runtime/
│  │  ├─ storage/
│  │  │  ├─ Inno.Storage/
│  │  │  └─ Inno.Storage.Runtime/
│  │  ├─ rendering/
│  │  │  ├─ Inno.Rendering/
│  │  │  ├─ Inno.Rendering.Assets/
│  │  │  ├─ Inno.Rendering.Runtime/
│  │  │  └─ Inno.Rendering.Shaders/
│  │  ├─ audio/
│  │  │  ├─ Inno.Audio/
│  │  │  ├─ Inno.Audio.Assets/
│  │  │  └─ Inno.Audio.Runtime/
│  │  ├─ text/
│  │  │  ├─ Inno.Text/
│  │  │  ├─ Inno.Text.Assets/
│  │  │  └─ Inno.Text.Runtime/
│  │  └─ ui/
│  │     ├─ Inno.UI/
│  │     ├─ Inno.UI.Assets/
│  │     └─ Inno.UI.Runtime/
│  ├─ runtime/
│  │  ├─ contracts/
│  │  │  └─ Inno.Runtime.Contracts/
│  │  ├─ engine/
│  │  │  └─ Inno.Runtime/
│  │  ├─ generators/
│  │  │  └─ Inno.Runtime.Generators/
│  │  ├─ plugins/
│  │  │  ├─ Inno.Plugins/
│  │  │  └─ Inno.Plugins.Authoring/
│  │  └─ scripting/
│  │     ├─ Inno.Scripting.Compiler/
│  │     └─ Inno.Scripting.Reload/
│  ├─ adapters/
│  │  ├─ common/
│  │  │  └─ Inno.Adapter/
│  │  ├─ default/
│  │  │  ├─ Inno.Adapter.Default/
│  │  │  └─ Inno.Adapter.Authoring.Default/
│  │  ├─ modules/
│  │  │  └─ Inno.Adapter.Modules.DotNet/             [新增]
│  │  ├─ platform/
│  │  │  ├─ Inno.Adapter.Platform/
│  │  │  └─ Inno.Adapter.Platform.Sdl3/
│  │  ├─ input/
│  │  │  ├─ Inno.Adapter.Input/
│  │  │  └─ Inno.Adapter.Input.Sdl3/
│  │  ├─ storage/
│  │  │  ├─ Inno.Adapter.Storage/
│  │  │  ├─ Inno.Adapter.Storage.FileSystem/
│  │  │  └─ Inno.Adapter.Storage.Browser/
│  │  ├─ rendering/
│  │  │  ├─ Inno.Adapter.Rendering/
│  │  │  ├─ Inno.Adapter.Rendering.Authoring/
│  │  │  └─ Inno.Adapter.Rendering.Bgfx/
│  │  ├─ audio/
│  │  │  ├─ Inno.Adapter.Audio/
│  │  │  └─ Inno.Adapter.Audio.MiniAudio/
│  │  ├─ text/
│  │  │  ├─ Inno.Adapter.Text/
│  │  │  └─ Inno.Adapter.Text.FreeTypeHarfBuzz/
│  │  ├─ ui/
│  │  │  ├─ Inno.Adapter.UI/
│  │  │  ├─ Inno.Adapter.UI.RmlUi/
│  │  │  └─ Inno.Adapter.UI.RmlUi.Authoring/
│  │  └─ presentation/
│  │     ├─ Inno.Adapter.Presentation/
│  │     ├─ Inno.Adapter.Presentation.ImGui.Sdl3/
│  │     └─ Inno.Adapter.Presentation.ImGui.Bgfx/
│  └─ composition/
│     ├─ default/
│     │  └─ Inno.Engine.Default/
│     ├─ shell/
│     │  └─ Inno.Shell/
│     ├─ player/
│     │  ├─ Inno.Player.Runtime/
│     │  ├─ Inno.Player/
│     │  ├─ Inno.Player.Browser/
│     │  └─ Inno.Player.IOS/                          [未来]
│     └─ editor/                                    见 Editor 展开结构
├─ native/                                          见 Native 展开结构
├─ build/                                           见 Build 展开结构
├─ tools/
├─ tests/
├─ docs/
├─ extern/                                          第三方源
└─ artifacts/                                       [产物] 可重建输出与验收证据
```

### 2. 模块、类型发现与代际管理

```text
src/foundation/extensibility/
├─ Inno.Extensibility.Modules/
│  ├─ Inno.Extensibility.Modules.csproj
│  ├─ ModuleHost.cs                                 [重构] 模块目录和事务 owner
│  ├─ ModuleHostOptions.cs                          [重构] 注入来源及领域配置
│  ├─ Catalog/
│  │  ├─ IModuleSource.cs                          [新增] 模块来源边界
│  │  ├─ ModuleCatalogContribution.cs              [新增] 不可变模块贡献
│  │  ├─ AssemblyCatalogSnapshot.cs                [重构] 当前目录快照
│  │  ├─ IAssemblyCatalogParticipant.cs
│  │  ├─ IAssemblyCatalogTransaction.cs
│  │  └─ AssemblyCatalogCoordinator.cs             收口目录事务
│  ├─ Metadata/
│  │  ├─ AssemblyDomain.cs
│  │  └─ AssemblyScope.cs
│  └─ Modules/
│     ├─ AssemblyModuleHandle.cs
│     ├─ AssemblyModuleInfo.cs
│     └─ AssemblyModuleEntry.cs
├─ Inno.Extensibility.Types/
│  ├─ Inno.Extensibility.Types.csproj
│  ├─ TypeRegistry.cs
│  ├─ TypeRegistryCoordinator.cs
│  ├─ TypeCache/
│  │  ├─ TypeCatalog.cs                            [重构] 共用类型发现入口
│  │  ├─ TypeCacheSnapshot.cs                      [重构] 接受显式类型元数据
│  │  ├─ ITypeCatalogSource.cs                     [新增] 类型元数据来源
│  │  ├─ TypeRef.cs
│  │  ├─ TypeIdentityRegistry.cs
│  │  ├─ TypeQueryRegistry.cs
│  │  └─ Attributes/
│  │     └─ StableTypeIdAttribute.cs
│  └─ Properties/
│     └─ ScriptingApi.cs
└─ Inno.Extensibility.Reload/
   ├─ Inno.Extensibility.Reload.csproj
   ├─ GenerationCoordinator.cs
   ├─ GenerationState.cs
   ├─ IGenerationChange.cs
   ├─ IGenerationPublication.cs
   ├─ IAssemblyUnloadProbe.cs
   ├─ AssemblyUnloadBarrier.cs
   ├─ AssemblyUnloadBarrierOptions.cs
   ├─ AssemblyUnloadBarrierState.cs
   └─ AssemblyUnloadException.cs

src/adapters/modules/Inno.Adapter.Modules.DotNet/
├─ Inno.Adapter.Modules.DotNet.csproj               [新增]
├─ DotNetModuleSource.cs                            动态模块来源
├─ DotNetModuleSourceOptions.cs                     cache 与加载配置
├─ Loading/
│  ├─ ModuleLoadContext.cs                          [迁入] collectible ALC
│  ├─ AssemblyLoadRequest.cs                        [迁入] 文件加载请求
│  └─ HostDependencyManifest.cs                     [迁入] 依赖解析
├─ Discovery/
│  └─ ReflectionTypeCatalogSource.cs                动态反射元数据来源
└─ Reloading/
   ├─ AssemblyReloadSession.cs                      [迁入]
   ├─ AssemblyReloadContext.cs                      [迁入]
   └─ AssemblyUnloadMonitor.cs                      [迁入] ALC 弱监测
```

具体规则：

- Foundation 管理目录、身份、快照和代际事务。
- DotNet Adapter 管理 ALC、shadow copy、动态依赖探测和反射扫描。
- 静态、动态来源共同进入同一个 TypeCatalog 与 Registry。
- 模块来源和类型来源由同一个组合入口配置，使用统一 generation coordinator。
- Editor retirement 完整保留 Full GC → finalizers → Full GC、弱监测和 Faulted gate。
- 移动后的 API 同步修改调用方、命名空间、项目引用及 Wiki。

### 3. Runtime、生成注册与代码部署

```text
src/runtime/
├─ contracts/Inno.Runtime.Contracts/
│  ├─ Inno.Runtime.Contracts.csproj
│  ├─ IRuntimeSubsystem.cs
│  ├─ IRuntimeSubsystemFactory.cs
│  ├─ RuntimeSubsystemDescriptor.cs
│  ├─ RuntimeSubsystemRequirement.cs
│  ├─ RuntimeSubsystemLifetime.cs
│  ├─ RuntimeSubsystemId.cs
│  ├─ RuntimeCapabilityId.cs
│  ├─ RuntimeFrame.cs
│  └─ RuntimeFixedFrame.cs
├─ engine/Inno.Runtime/
│  ├─ Inno.Runtime.csproj
│  ├─ Hosting/
│  │  ├─ EngineHost.cs
│  │  ├─ EngineHostBuilder.cs                       [重构] 注入模块及类型来源
│  │  ├─ RuntimeSession.cs
│  │  ├─ RuntimeSessionOptions.cs
│  │  └─ RuntimeJobExecutionMode.cs
│  ├─ Deployment/
│  │  ├─ GameCodeDeployment.cs                     [新增] 验证后的逻辑代码部署
│  │  ├─ GameRuntimeModule.cs                      [重构] 模块身份及内容身份
│  │  ├─ GameRuntimePlugin.cs
│  │  ├─ GameRuntimeManifest.cs                    [重构] 代码与内容部署描述
│  │  └─ RuntimeContentDeployment.cs               内容物化与校验
│  ├─ Registration/
│  │  └─ StaticModuleSource.cs                     [新增] 静态目录来源
│  ├─ Subsystems/
│  │  ├─ RuntimeSubsystemPipeline.cs
│  │  └─ SceneRuntimeSubsystem.cs
│  ├─ Execution/
│  │  └─ Time.cs
│  └─ Properties/
│     └─ ScriptingApi.cs
└─ generators/Inno.Runtime.Generators/
   ├─ Inno.Runtime.Generators.csproj
   ├─ RuntimeSubsystemGenerator.cs
   ├─ Registration/
   │  ├─ RuntimeModuleCatalogGenerator.cs          [新增]
   │  ├─ RuntimeTypeCatalogGenerator.cs            [新增]
   │  └─ RuntimeFactoryGenerator.cs                [新增]
   └─ Diagnostics/
      └─ RuntimeRegistrationDiagnostics.cs         [新增]

src/foundation/core/Inno.Core.Serialization.Generators/
├─ Inno.Core.Serialization.Generators.csproj
└─ SerializationConverterGenerator.cs              [重构] AOT 可用注册与访问代码
```

生成代码包括模块目录、类型元数据、Stable ID、构造工厂、序列化注册、必要泛型实例及回调入口。

```text
所属项目/obj/<配置>/<目标>/<生成指纹>/
└─ Generated/
   └─ Registration/
      ├─ RuntimeModules.g.cs                       [生成]
      ├─ RuntimeTypes.g.cs                         [生成]
      ├─ RuntimeFactories.g.cs                     [生成]
      └─ RuntimeSerialization.g.cs                 [生成]
```

每个程序集生成自己的注册入口，最终 Player 组合实际代码闭包。

- CoreCLR 发布也使用静态注册，JIT 由运行时负责。
- AOT 发布不携带运行时编译器或 collectible loader。
- 类型关系、扩展属性与生成元数据只声明一次。
- 删除整批 `TrimmerRootAssembly` 的兜底保留。
- 无法表达的 AOT 行为在构建阶段诊断。
- 稳定数据仅保存 ID、标量和中立 bytes；runtime 引用受所属 generation 管理。

### 4. Player 与 Shell

```text
src/composition/
├─ shell/Inno.Shell/
│  ├─ Inno.Shell.csproj
│  ├─ Shell.cs                                     共用事件与运行生命周期
│  ├─ ShellOptions.cs
│  ├─ ShellFrame.cs
│  ├─ IShellFrameDriver.cs
│  ├─ PollingShellFrameDriver.cs                    桌面轮询调度
│  └─ ScheduledShellFrameDriver.cs                  外部回调调度
└─ player/
   ├─ Inno.Player.Runtime/
   │  ├─ Inno.Player.Runtime.csproj
   │  ├─ PlayerApplication.cs
   │  ├─ PlayerLaunchOptions.cs
   │  ├─ GamePlayerHost.cs                         [重构] 共用运行流程
   │  └─ Deployment/
   │     ├─ IPlayerModuleActivator.cs              [重构] 接受 GameCodeDeployment
   │     └─ StaticPlayerModuleActivator.cs         [新增]
   ├─ Inno.Player/
   │  ├─ Inno.Player.csproj
   │  ├─ Program.cs                                薄启动入口
   │  └─ DesktopPlayerComposition.cs               [新增] 桌面组合
   ├─ Inno.Player.Browser/
   │  ├─ Inno.Player.Browser.csproj
   │  ├─ Program.cs                                薄启动入口
   │  ├─ BrowserPlayerComposition.cs               [新增] 浏览器组合
   │  ├─ BrowserContentLoader.cs                   [新增] 下载与内容校验
   │  ├─ BrowserBridge.cs                          [新增] JS 生命周期边界
   │  └─ wwwroot/
   │     ├─ index.html
   │     └─ main.js                                启动及浏览器系统回调
   └─ Inno.Player.IOS/                              [未来]
      ├─ Inno.Player.IOS.csproj
      ├─ Program.cs
      ├─ AppDelegate.cs
      ├─ IosPlayerComposition.cs
      ├─ IosFrameDriver.cs
      └─ Resources/
         └─ 应用资源
```

共同生命周期：

```text
Initialize → Run ↔ Suspend → Stop → Stopped
                 └───────────────→ Faulted
```

输入、焦点、暂停和恢复沿同一平台事件入口进入 Core Events。退出依次停止新工作、取消任务、注销回调、提交存储并释放资源。

`IPlayerModuleActivator` 不要求 DLL 目录。静态激活校验构建模块目录与部署清单的一致性。

### 5. Adapter 的统一结构

以 Input 为例，其他领域沿相同边界组织：

```text
src/adapters/input/
├─ Inno.Adapter.Input/
│  ├─ Inno.Adapter.Input.csproj
│  ├─ InputBackendId.cs                            [新增] 开放稳定 ID
│  ├─ InputBackendProvider.cs                      [新增] 发现元数据
│  ├─ InputBackendCatalog.cs                       [新增] 不可变 provider 快照
│  ├─ IInputBackendFactory.cs
│  └─ InputBackend.cs                              [重构] 删除封闭平台选择
└─ Inno.Adapter.Input.Sdl3/
   ├─ Inno.Adapter.Input.Sdl3.csproj
   ├─ Sdl3InputBackendFactory.cs
   └─ SDL 输入实现
```

Platform、Storage、Audio、Text 同步采用对应领域的开放 ID 与 catalog。Rendering、UI 使用一致的组合规则。

`IAdapterCatalog` 保持领域化的类型入口。能力缺失明确失败；能力描述留在对应领域。

原生句柄、第三方类型和实现枚举仅存在于具体 Adapter 及 Native 边界内。

### 6. Editor 项目与 UI 职责

```text
src/composition/editor/
├─ contracts/
│  └─ Inno.Editor.Annotations/
├─ host/
│  └─ Inno.Editor.Application/
│     ├─ Inno.Editor.Application.csproj
│     └─ 应用启动与 Editor composition
├─ framework/
│  ├─ Inno.Editor.Core/
│  │  └─ Extensions/
│  │     ├─ EditorModal.cs
│  │     └─ EditorModalAttribute.cs
│  ├─ Inno.Editor.Diagnostics/
│  ├─ Inno.Editor.Graph/
│  │  ├─ GraphDocumentController.cs
│  │  ├─ GraphDocumentSession.cs
│  │  ├─ GraphCanvasState.cs
│  │  └─ GraphHistory.cs
│  ├─ Inno.Editor.Inspection/
│  │  └─ PropertyDrawing/
│  │     ├─ IPropertyDrawer.cs
│  │     ├─ PropertyDrawerRegistry.cs
│  │     ├─ PropertyDrawContext.cs
│  │     └─ SerializedPropertyRenderer.cs
│  ├─ Inno.Editor.Interactions/
│  │  ├─ Actions/                                  Action 路由与快捷键
│  │  ├─ DragDrop/                                 Identity payload 与 delivery
│  │  ├─ History/                                  唯一数据 Undo/Redo
│  │  ├─ State/                                    中立 Editor 状态
│  │  ├─ Documents/                                文档工作流
│  │  ├─ Viewport/                                 工具及视口交互
│  │  └─ Discovery/                                扩展候选快照
│  └─ Inno.Editor.Settings/
├─ presentation/
│  └─ Inno.Editor.ImGui/
│     ├─ Inno.Editor.ImGui.csproj
│     ├─ Widgets/
│     │  ├─ ImGuiWidget.Property.cs                2:3 label/input 与自动换行
│     │  ├─ ImGuiWidget.Selector.cs                向下展开及窗口内高度限制
│     │  ├─ ImGuiWidget.ContextMenu.cs             搜索宽度与菜单交互
│     │  ├─ ImGuiWidget.Search.cs
│     │  ├─ ImGuiWidget.Panel.cs                   单一滚动 owner
│     │  ├─ ImGuiWidget.DragDrop.cs
│     │  └─ 其他现有 ImGuiWidget.*.cs
│     └─ Runtime/
│        ├─ EditorModalHost.cs
│        └─ EditorModalRenderer.cs
├─ features/
│  ├─ Inno.Editor.Assets/
│  ├─ Inno.Editor.Audio/
│  ├─ Inno.Editor.Exporting/
│  │  ├─ Interactions/
│  │  │  └─ ExportActions.cs
│  │  ├─ Runtime/
│  │  │  └─ ExportWindowModule.cs
│  │  └─ Presentation/
│  │     ├─ GameExportModal.cs
│  │     ├─ GameExportProgressModal.cs
│  │     └─ PluginExportModal.cs
│  ├─ Inno.Editor.PlayMode/
│  │  └─ Runtime/
│  │     ├─ EditorPlayModeModule.cs
│  │     ├─ EditorPlayModeController.cs
│  │     └─ EditorPlayModeLoop.cs
│  ├─ Inno.Editor.Rendering/
│  ├─ Inno.Editor.Scene/
│  ├─ Inno.Editor.Scripting/
│  │  └─ Runtime/
│  │     └─ ScriptCompilationModal.cs
│  └─ Inno.Editor.Shaders/
└─ panels/
   ├─ Inno.Editor.Panel.FileBrowser/
   │  ├─ Interactions/
   │  ├─ AssetEditors/
   │  └─ Samples/                                  异步导入及 owner-thread 提交
   ├─ Inno.Editor.Panel.GameView/
   ├─ Inno.Editor.Panel.Global/
   ├─ Inno.Editor.Panel.Hierarchy/
   ├─ Inno.Editor.Panel.Inspector/
   ├─ Inno.Editor.Panel.Logging/
   ├─ Inno.Editor.Panel.SceneView/
   ├─ Inno.Editor.Panel.Settings/
   ├─ Inno.Editor.Panel.ShaderEditor/
   │  ├─ Canvas/
   │  │  ├─ ShaderEditorCanvas.cs
   │  │  └─ ShaderEditorCanvas.MiniMap.cs
   │  └─ Presentation/
   │     └─ ShaderCheckModal.cs
   └─ Inno.Editor.Panel.Stats/
```

Editor 业务项目遵循程序集级命名空间规则。Widgets 的 presentation、options、result 与状态留在对应 `ImGuiWidget.*.cs` 中。

重构过程中持续验证：

- 单一滚动 owner、2:3 表单比例、label 换行。
- 下拉框向下展开、最大高度和窗口边界。
- Modal 独占交互，Export 结束自动关闭。
- 前景窗口事件消费与浮动 GameView 输入焦点。
- PlayMode 状态隔离、Sample 导入异步执行。
- History、Workspace、Missing 与 reload 的现有行为。

### 7. Native 组件标准布局

原生组件统一归属：

```text
C:\Dev\GameEngineDev\InnoEngine\native\
├─ Inno.Native.Bgfx/
├─ Inno.Native.ImGui/
├─ Inno.Native.ImGuizmo/
├─ Inno.Native.LibraryLoading/
├─ Inno.Native.MiniAudio/
├─ Inno.Native.Sdl3/                                目录大小写与项目名统一
├─ Inno.Native.Text/
└─ Inno.Native.UI/
```

`Inno.Native.UI` 的完整目标结构：

```text
C:\Dev\GameEngineDev\InnoEngine\native\Inno.Native.UI\
├─ Inno.Native.UI.csproj
├─ UiNative.cs                                     managed 适配入口
├─ Native/
│  ├─ include/
│  │  └─ RmlUiRuntime.hpp                          手写窄 C++ facade
│  ├─ src/
│  │  ├─ RmlUiRuntime.cpp                          第三方语义适配
│  │  ├─ RmlUiProcessHost.hpp
│  │  ├─ RmlUiProcessHost.cpp                      显式进程 owner
│  │  ├─ RmlUiRenderInterface.hpp
│  │  ├─ RmlUiRenderInterface.cpp
│  │  ├─ RmlUiEventListener.hpp
│  │  └─ RmlUiEventListener.cpp
│  ├─ Generated/                                   [生成] 宿主 C 桥
│  │  ├─ 生成的 C header
│  │  └─ 生成的 C++ implementation
│  └─ CMakeLists.txt
├─ Bindings/
│  ├─ common.json                                  共同映射与 ownership
│  ├─ bindgen.json                                 共同 managed 生成定义
│  ├─ rmlui.bridge.json                            共同 C++ 桥定义
│  └─ Extension/                                   有实际需求时提供组件扩展
├─ Generated/
│  └─ Bindings.cs                                  [生成] 宿主 managed 绑定
└─ obj/
   └─ <targetId>/
      └─ <generationFingerprint>/
         ├─ Native/                                [生成] 目标 C 桥
         │  ├─ 生成的 C header
         │  └─ 生成的 C++ implementation
         ├─ Generated/
         │  └─ Bindings.cs                          [生成] 目标 managed 绑定
         └─ generation-manifest                     [产物] 当前生成身份
```

其他 Native 组件采用同样的源码、定义、生成物和目标隔离规则；纯 C API 组件使用对应的 C 解析入口。

- target、ABI、sysroot 和链接策略从工具链注入。
- 共同定义承载业务映射，目标配置只承载实际差异。
- C 桥与 managed 绑定具有独立输出根。
- CMake 中间文件属于组件 Toolchain 的 `obj/native`。
- 修改 facade、配置或生成器，随后重新生成绑定。
- 第三方源保持在 `extern`。
- 全局原生状态由显式进程 owner 管理引用计数、线程和会话隔离。

### 8. Build 完整结构

```text
C:\Dev\GameEngineDev\InnoEngine\build\
├─ Directory.Build.props
├─ cli/
│  └─ Inno.Build.Cli/
│     ├─ Inno.Build.Cli.csproj
│     ├─ Program.cs                                唯一生产构建入口
│     ├─ BuildComposition.cs                       [新增] provider 组合根
│     ├─ BuildCommand.cs
│     ├─ CliOptions.cs
│     ├─ BuildWorkspace.cs
│     ├─ EngineBuildWorkflow.cs
│     ├─ ProjectBuildWorkflow.cs
│     └─ NativeBindingsVerification.cs
├─ tasks/
│  └─ Inno.Build.Tasks/
│     ├─ Inno.Build.Tasks.csproj
│     ├─ GenerateBindingsTask.cs                   [新增] BGCS 库入口集成
│     ├─ CompileShaderTask.cs
│     └─ PublishSupportPackTask.cs
├─ pipeline/
│  ├─ Inno.Build/
│  │  ├─ Inno.Build.csproj
│  │  ├─ BuildPipeline.cs
│  │  ├─ BuildProfile.cs                           增加稳定部署选择
│  │  ├─ BuildSettings.cs
│  │  ├─ BuildTargetId.cs
│  │  ├─ BuildDiagnostic.cs
│  │  ├─ BuildProgress.cs
│  │  ├─ BuildResult.cs
│  │  ├─ Game/
│  │  │  ├─ GameBuildPipeline.cs
│  │  │  ├─ IGameBuildTarget.cs                    平台内容约束与打包边界
│  │  │  ├─ GameBuildContentContext.cs
│  │  │  └─ GameBuildPackageContext.cs
│  │  ├─ Player/
│  │  │  ├─ PlayerSupportPackCatalog.cs
│  │  │  ├─ IPlayerSupportPackValidator.cs
│  │  │  └─ IPlayerSupportPackProvisioner.cs
│  │  ├─ Content/
│  │  │  └─ ContentPackWriter.cs
│  │  ├─ Plugins/
│  │  │  └─ PluginPackageBuilder.cs
│  │  ├─ Snapshots/
│  │  │  └─ BuildSnapshotFingerprint.cs
│  │  └─ Pipeline/
│  │     └─ BuildFileSystem.cs                     staging 与原子提交
│  ├─ Inno.Build.Platform.Windows/
│  │  ├─ Inno.Build.Platform.Windows.csproj
│  │  ├─ WindowsX64GameBuildTarget.cs
│  │  └─ WindowsSupportPackValidator.cs
│  ├─ Inno.Build.Platform.MacOS/
│  │  ├─ Inno.Build.Platform.MacOS.csproj
│  │  ├─ MacOSArm64GameBuildTarget.cs
│  │  └─ MacOSSupportPackValidator.cs
│  ├─ Inno.Build.Platform.Browser/
│  │  ├─ Inno.Build.Platform.Browser.csproj
│  │  ├─ BrowserWasmGameBuildTarget.cs
│  │  └─ BrowserSupportPackValidator.cs
│  └─ Inno.Build.Platform.IOS/                      [未来]
│     ├─ Inno.Build.Platform.IOS.csproj
│     ├─ IosArm64GameBuildTarget.cs
│     ├─ IosSupportPackValidator.cs
│     └─ Packaging/
│        ├─ IosApplicationPackager.cs
│        └─ IosApplicationSigner.cs
├─ managed/
│  ├─ Inno.Build.Managed/                          [新增]
│  │  ├─ Inno.Build.Managed.csproj
│  │  ├─ IManagedDeploymentCompiler.cs
│  │  ├─ ManagedDeploymentId.cs
│  │  ├─ ManagedDeploymentRequest.cs
│  │  ├─ ManagedDeploymentResult.cs
│  │  ├─ ManagedDeploymentCapabilities.cs
│  │  └─ ManagedDeploymentCatalog.cs
│  └─ Inno.Build.Managed.DotNet/                   [新增]
│     ├─ Inno.Build.Managed.DotNet.csproj
│     ├─ Desktop/
│     │  └─ CoreClrDeploymentCompiler.cs
│     ├─ WebAssembly/
│     │  ├─ MonoWasmDeploymentCompiler.cs          解释执行与 AOT 发布
│     │  └─ CoreClrWasmDeploymentCompiler.cs       [未来]
│     ├─ NativeAot/
│     │  └─ NativeAotDeploymentCompiler.cs
│     ├─ Apple/
│     │  └─ IosAotDeploymentCompiler.cs            [未来]
│     └─ Publishing/
│        ├─ DotNetPublishRequest.cs
│        └─ DotNetPublishExecutor.cs              取消、输出及错误收口
├─ toolchains/
│  ├─ Inno.Build.Toolchains/
│  │  ├─ Inno.Build.Toolchains.csproj
│  │  ├─ NativeBuildContext.cs
│  │  ├─ ToolchainEnvironment.cs
│  │  ├─ ToolchainLayout.cs
│  │  ├─ BuildArtifactOptions.cs
│  │  ├─ Managed/
│  │  │  ├─ DotNetSdkResolver.cs                  [新增] 按工程解析 SDK
│  │  │  └─ DotNetSdkDescriptor.cs
│  │  └─ Platforms/
│  │     └─ WindowsCppBuildEnvironment.cs
│  ├─ Inno.Build.Toolchains.Host/
│  │  └─ HostNativeBuild.cs
│  ├─ Inno.Build.Toolchains.Browser/
│  │  ├─ BrowserToolchain.cs
│  │  ├─ EmscriptenToolchainResolver.cs            [新增]
│  │  └─ Native/
│  │     ├─ CMakeLists.txt
│  │     └─ wasm_sjlj_shim.c                       工具链所需桥接
│  ├─ Inno.Build.Toolchains.Apple/                 [未来]
│  │  ├─ AppleToolchain.cs
│  │  ├─ AppleSdkResolver.cs
│  │  └─ AppleNativeBuildEnvironment.cs
│  ├─ Inno.Build.Toolchains.Bgfx/
│  ├─ Inno.Build.Toolchains.Bgfx.Tools/
│  │  ├─ Sources/                                 具体语言前端
│  │  └─ Intermediate/                            IR 降低与反射
│  ├─ Inno.Build.Toolchains.Bgfx.Shaders/
│  ├─ Inno.Build.Toolchains.ImGui/
│  ├─ Inno.Build.Toolchains.ImGuizmo/
│  ├─ Inno.Build.Toolchains.MiniAudio/
│  ├─ Inno.Build.Toolchains.Sdl3/
│  ├─ Inno.Build.Toolchains.Text/
│  └─ Inno.Build.Toolchains.UI/
└─ support/
   ├─ Inno.Build.SupportPacks.Core/
   │  ├─ IPlayerSupportPackSource.cs
   │  ├─ PlayerSupportPackBuildContext.cs
   │  ├─ PlayerSupportPackPublisher.cs
   │  └─ SourcePlayerSupportPackProvisioner.cs
   └─ Inno.Build.SupportPacks/
      ├─ BuiltInPlayerSupportPacks.cs
      ├─ PlayerSupportPackFiles.cs
      ├─ DesktopPlayerSupportPackSource.cs
      ├─ BrowserPlayerSupportPackSource.cs
      ├─ IosPlayerSupportPackSource.cs             [未来]
      └─ Templates/
         ├─ Desktop/                              [新增] 按游戏闭包发布模板
         └─ Browser/                              迁入现有 BrowserLink 模板
```

所有 Toolchain、Platform、Support Pack 项目具有对应 `.csproj`，采用库输出。

组件 Toolchain 的内部组织按真实功能划分：

```text
build/toolchains/Inno.Build.Toolchains.UI/
├─ Inno.Build.Toolchains.UI.csproj
├─ UiToolchain.cs                                   构建入口
├─ Native/
│  └─ CMake 配置补充文件                            有实际需要时提供
└─ obj/
   └─ native/
      └─ <targetId>/
         └─ <toolchainFingerprint>/                [产物] CMake、对象文件及链接中间态
```

发布产物与缓存统一描述：

```text
artifacts/
├─ native/<component>/<targetId>/<fingerprint>/     [产物] 原生库及描述
├─ managed/<deploymentId>/<targetId>/<fingerprint>/ [产物] 托管部署
├─ support-packs/<targetId>/<deploymentId>/<fingerprint>/
├─ builds/<buildId>/
│  ├─ staging/                                     提交前临时输出
│  └─ logs/
└─ acceptance/
   └─ <runId>/
      ├─ environment.json
      ├─ results/
      ├─ logs/
      └─ captures/
```

最终用户输出目录仍由 BuildProfile 决定。构建失败和取消保留上次完整输出。

## 三、BGCS 详细目标结构

### 1. 仓库及八个生产项目

```text
C:\Dev\GameEngineDev\BindGen-CS\
├─ BindGen-CS.sln
├─ AGENTS.md                                       [新增] 独立开发规范入口
├─ .editorconfig                                   [新增]
├─ Directory.Build.props                           [新增] 共同编译及 XML 约束
├─ global.json                                     生成器工程 SDK 选择
├─ README.md                                       专业、入门友好的英文入口
├─ README.cn.md                                    中文入口
├─ src/
│  ├─ BGCS.Core/
│  ├─ BGCS.CppAst/
│  ├─ BGCS.Intermediate/
│  ├─ BGCS/
│  ├─ BGCS.Cpp2C/
│  ├─ BGCS.Language/
│  ├─ BGCS.Runtime/
│  └─ BGCS.Tool/
├─ tests/
├─ examples/
├─ scripts/
├─ docs/
├─ extern/                                         解析器资源及第三方内容
├─ .github/workflows/
└─ artifacts/                                      [产物] 构建和独立验收
```

依赖：

```text
BGCS.Tool → BGCS + BGCS.Cpp2C
BGCS → BGCS.Core + BGCS.CppAst + BGCS.Intermediate + BGCS.Language
BGCS.Cpp2C → BGCS.Core + BGCS.CppAst + BGCS.Intermediate
BGCS.CppAst → BGCS.Core
BGCS.Language → BGCS.Core

BGCS.Intermediate：不依赖其他 BGCS 项目
BGCS.Runtime：独立互操作库
```

### 2. Core：中立契约与基础能力

```text
src/BGCS.Core/
├─ BGCS.Core.csproj
├─ Targeting/                                      [新增]
│  ├─ NativeTargetId.cs
│  ├─ NativeTargetRequest.cs
│  ├─ NativeTargetDescriptor.cs
│  ├─ NativeToolchainDescriptor.cs
│  └─ INativeTargetProvider.cs
├─ Extensibility/
│  ├─ IBindingPlugin.cs                            拆分现有巨型契约文件
│  ├─ IBindingPluginHost.cs
│  ├─ ICacheFingerprintProvider.cs
│  ├─ BindingPluginService.cs
│  ├─ BindingPluginRegistry.cs
│  └─ BindingPluginLoader.cs
├─ Collections/
│  └─ 现有通用集合
├─ Logging/
│  ├─ LogSeverity.cs
│  └─ LogMessage.cs
├─ Caching/
│  ├─ IncrementalGenerationCache.cs
│  └─ GenCacheFile.cs
├─ IO/
│  └─ OutputDirectoryTransaction.cs
├─ Writing/                                        [重构] 通用写出机制
│  ├─ ICodeWriter.cs
│  └─ CodeWriter.cs
└─ Text/                                           [重构] 通用文本与标识符机制
   └─ 通用文本实现
```

Core 删除 CppAst 引用。AST 参数、C++ 类型和语言映射迁入对应前端或生成器。

### 3. CppAst：解析器及 Clang 边界

```text
src/BGCS.CppAst/
├─ BGCS.CppAst.csproj
├─ Parsing/
│  ├─ CppParser.cs
│  ├─ CppParserOptions.cs
│  ├─ CppParserKind.cs
│  ├─ CppModelBuilder.cs
│  ├─ CppModelBuilder.Types.cs
│  ├─ CppModelBuilder.Expressions.cs
│  ├─ CppModelBuilder.Attributes.cs
│  ├─ CppModelBuilder.VisitMember.cs
│  └─ Visitors/                                    现有 visitor 按职责归并
├─ Model/
│  ├─ CppElement.cs
│  ├─ Declarations/
│  ├─ Types/
│  ├─ Expressions/
│  └─ Attributes/
├─ Diagnostics/
│  ├─ CppDiagnosticBag.cs
│  └─ CppDiagnosticMessage.cs
├─ Targeting/
│  ├─ ClangTargetResolver.cs                        [新增] 中立目标 → Clang 参数
│  ├─ CppToolchainDiscovery.cs                      [重构] 显式宿主及 SDK 探测
│  └─ Providers/
│     ├─ WindowsNativeTargetProvider.cs            [新增]
│     ├─ UnixNativeTargetProvider.cs               [新增]
│     ├─ AppleNativeTargetProvider.cs              [新增]
│     └─ EmscriptenNativeTargetProvider.cs          [新增]
├─ Interop/
│  ├─ ClangNativeRuntime.cs
│  └─ ClangResourceHeaders.cs
├─ Collections/
├─ AttributeParsing/                               规范化现有命名
└─ Utilities/                                      限于 Parser 的实现
```

目标 provider 解析目标描述；Clang resolver 转换 triple、sysroot、include、defines 和 ABI 参数。

bundled builtin headers 与所用 libclang 匹配。宿主 SDK headers 与 builtin headers分别管理。当前 iOS 目标描述在 Apple provider 中区分设备与模拟器。

### 4. Intermediate：真正冻结的生成模型

```text
src/BGCS.Intermediate/
├─ BGCS.Intermediate.csproj
├─ BindingModule.cs                                [迁入、重构]
├─ BindingType.cs
├─ BindingFunction.cs
├─ BindingDelegate.cs
├─ BindingConstant.cs
├─ BindingImport.cs
├─ MarshallingPlan.cs
├─ BindingGenerationResult.cs
├─ Diagnostics/
│  ├─ BindingDiagnostic.cs
│  └─ BindingDiagnosticCatalog.cs
├─ Emission/
│  ├─ IBindingEmitter.cs                           [迁入]
│  └─ EmissionContext.cs                           拆分独立类型
└─ Bridges/                                        [新增]
   ├─ CppBridgeModule.cs
   ├─ CppBridgeType.cs
   ├─ CppBridgeFunction.cs
   ├─ CppBridgeOperation.cs
   └─ ICppBridgeEmitter.cs
```

- 源码实际属于本项目，删除 `Compile Link`。
- builder 在分析层，结果模型只读。
- IR 保存布局、符号、ABI、ownership 和 marshalling 事实。
- 语义模型不携带 Clang 对象、运行服务、SDK 探测或 emitter 回调。
- 独立 emission request 承载输出路径等写出选项。

### 5. BGCS：C → C# 生成链

```text
src/BGCS/
├─ BGCS.csproj
├─ Facade/
│  └─ BindingGenerator.cs                          主要公开入口
├─ Application/
│  └─ BindingGenerationPipeline.cs                 完整生成编排
├─ Configuration/
│  ├─ CsCodeGeneratorConfig.cs                      归并当前配置职责
│  ├─ IGeneratorConfig.cs                          [迁入、收窄]
│  ├─ ConfigLoader.cs
│  ├─ ConfigDocumentLoader.cs
│  ├─ ConfigValidator.cs
│  ├─ PresetResolver.cs
│  ├─ MarshallingMapping.cs
│  └─ 映射与命名配置
├─ Analysis/
│  ├─ DeclarationGraph.cs
│  ├─ BindingModuleAnalyzer.cs
│  ├─ TypeAnalyzer.cs
│  ├─ AbiLayoutAnalyzer.cs
│  ├─ OwnershipAnalyzer.cs
│  ├─ StrictSafetyAnalyzer.cs
│  └─ OverloadPlanner.cs
├─ Conversion/
│  ├─ CppTypeConverter.cs
│  └─ PlatformAbiTypeClassifier.cs
├─ Emission/
│  ├─ CSharpEmitter.cs
│  ├─ RuntimeEmitter.cs
│  └─ SingleFileComposer.cs
├─ Patching/
│  ├─ IPatch.cs
│  ├─ IPrePatch.cs
│  ├─ IPostPatch.cs
│  ├─ PatchEngine.cs
│  ├─ PatchContext.cs
│  └─ 具体 Patch
├─ Output/
│  ├─ GeneratedOutputTransaction.cs
│  └─ SingleFileOutputNameResolver.cs
└─ Metadata/
   └─ 当前生成元数据
```

收口旧 generator、builder 与 facade 中重复的编排职责。保留必要公开能力，通过唯一应用流程执行。

### 6. Cpp2C：C++ 桥生成及原生构建

```text
src/BGCS.Cpp2C/
├─ BGCS.Cpp2C.csproj
├─ Application/
│  └─ CppBridgeGenerationPipeline.cs                [新增] 唯一桥生成编排
├─ Configuration/
│  ├─ Cpp2CGeneratorConfig.cs                       归并当前配置职责
│  └─ Cpp2CConfigValidator.cs
├─ Analysis/
│  └─ CppBridgeModuleAnalyzer.cs                    [重构] 构建冻结 Bridge IR
├─ Lowering/
│  ├─ ICppTypeLowering.cs                          拆分既有协议
│  ├─ CppLoweringRegistry.cs
│  ├─ CppLoweringRecipes.cs
│  ├─ BuiltInCppTypeLowerings.cs
│  └─ CppExtensionArtifactEmitter.cs
├─ Emission/
│  └─ CBridgeEmitter.cs                             [重构] 只消费 Bridge IR
├─ Build/
│  ├─ NativeBuildPlan.cs
│  ├─ NativeBuildExecutor.cs
│  ├─ NativeBuildPaths.cs
│  ├─ NativeExportInspector.cs
│  ├─ NativeBinaryIdentity.cs
│  ├─ CppBridgeBuildManifest.cs
│  ├─ CppBridgeBuildManifestEmitter.cs
│  ├─ CppBridgeBuildManifestSerializer.cs
│  └─ Providers/
│     ├─ ClangNativeBuildProvider.cs
│     ├─ ClangClNativeBuildProvider.cs
│     ├─ CMakeNativeBuildProvider.cs
│     ├─ MesonNativeBuildProvider.cs
│     └─ MSBuildNativeBuildProvider.cs
└─ Metadata/
   └─ 桥生成元数据
```

唯一链路：

```text
C++ → Clang AST → 分析与 Lowering → Bridge IR
    → C header / C++ bridge → C header 解析
    → Binding IR → C# emitter
```

Bridge IR 覆盖当前支持的构造、销毁、方法、继承、模板实例化、布局及回调 lowering。删除 `EmitAst` 和 AST 直接输出路径。

### 7. Language、Runtime 与 Tool

```text
src/BGCS.Language/
├─ BGCS.Language.csproj
├─ Lexing/
│  ├─ Lexer.cs
│  ├─ Token.cs
│  └─ TokenType.cs
├─ Parsing/
│  ├─ ParserBase.cs
│  ├─ ParserContext.cs
│  └─ ParserResult.cs
├─ Preprocessing/
│  └─ Preprocessor.cs
├─ CSharp/
│  ├─ CSharpParser.cs
│  ├─ Nodes/
│  └─ Analyzers/
└─ Cpp/
   ├─ CppMacroParser.cs
   └─ Analysers/

src/BGCS.Runtime/
├─ BGCS.Runtime.csproj
├─ Primitives/
│  ├─ Pointer.cs
│  ├─ ConstPointer.cs
│  ├─ Bool8.cs
│  ├─ Bool32.cs
│  ├─ Bitfield.cs
│  ├─ Atomic.cs
│  ├─ NativeLongDouble.cs
│  └─ Aapcs64VaList.cs
├─ Interop/
│  ├─ INativeContext.cs
│  ├─ NativeLibraryContext.cs
│  ├─ NativeLibrary.cs
│  ├─ LibraryLoader.cs
│  ├─ FunctionTable.cs
│  ├─ NativeNameAttribute.cs
│  ├─ NativeCallback.cs
│  ├─ NativeCallbackRegistration.cs
│  ├─ NativeCallbackRegistry.cs
│  ├─ NativeCallbackExceptionBoundary.cs
│  ├─ NativeAotCallback.cs
│  └─ NativeAsyncOperation.cs
└─ Utilities/
   └─ 现有互操作基础实现

src/BGCS.Tool/
├─ BGCS.Tool.csproj
├─ Program.cs                                      唯一生产 CLI
├─ WorkspaceCommand.cs
├─ Commands/
│  ├─ InitCommand.cs
│  ├─ GenerateCommand.cs                           [新增] 提取命令处理
│  ├─ BridgeCommand.cs                             [新增]
│  ├─ NativeBuildCommand.cs
│  ├─ ExplainCommand.cs
│  ├─ SchemaCommand.cs
│  ├─ SupplyChainCommand.cs
│  └─ ValidateCommand.cs                           [新增]
├─ Validation/
│  ├─ CSharpStyleValidator.cs                      [新增]
│  ├─ ArchitectureValidator.cs                     [新增]
│  ├─ ApiSnapshotValidator.cs                      [迁入]
│  └─ DependencyAudit.cs                           [迁入]
└─ Output/
   ├─ GenerationDiagnosticWriter.cs
   └─ GeneratedSourceComparison.cs
```

语言前端、互操作库与 CLI 各自独立。API snapshot 和 dependency audit 的独立工具 Program 并入验证命令。

### 8. BGCS 文档、示例及输出

```text
docs/
├─ README.md
├─ README.cn.md
├─ csharp-development-standard.cn.md                [新增] 独立规范副本
├─ architecture.md                                 [重构]
├─ architecture.cn.md                              [重构]
├─ platform-architecture-refactor-plan.cn.md        [新增] 完整执行清单
├─ architecture-refactor-acceptance.cn.md           [新增] 实际验收
├─ getting-started.md
├─ getting-started.cn.md
├─ configuration-guide.md
├─ configuration-guide.cn.md
├─ capabilities.md
├─ capabilities.cn.md
├─ lowering.md
├─ lowering.cn.md
└─ testing.md

examples/
├─ QuickStart/                                     最小 C API → C# 示例
├─ NativeShim/                                     C++ facade → C 桥 → C# 示例
└─ LoweringPlugin/                                 独立扩展示例

artifacts/
├─ generation/<targetId>/<fingerprint>/
│  ├─ Native/                                      C 桥
│  ├─ Generated/                                   managed 绑定
│  └─ diagnostics/
├─ native/<targetId>/<fingerprint>/                 编译及链接产物
├─ packages/                                       本地 package 验证
└─ acceptance/<runId>/
   ├─ environment.json
   ├─ report.json
   └─ logs/
```

BGCS 使用自身 fixture 和消费者验证能力，报告与 InnoEngine 联调报告独立。

## 四、完整数据流、公开契约与平台扩展

### 1. 最小新增或调整的契约

| 契约 | 必要语义 |
|---|---|
| `IModuleSource` | 向统一模块目录贡献模块，声明能力并承担来源生命周期 |
| `ITypeCatalogSource` | 提供当前模块 generation 对应的类型元数据 |
| `IPlayerModuleActivator` | 激活经过校验的逻辑代码部署 |
| `GameCodeDeployment` | 描述模块身份、内容指纹和物理部署产物 |
| `IManagedDeploymentCompiler` | 将冻结代码闭包发布为所选托管部署形式 |
| `ManagedDeploymentRequest` | 冻结目标、代码、工具链和原生链接输入 |
| `ManagedDeploymentResult` | 返回确定性产物、诊断和启动所需描述 |
| `ManagedDeploymentCapabilities` | 明确解释执行、AOT、动态代码等实际能力 |
| `INativeTargetProvider` | 解析开放目标 ID 对应的原生目标与工具链描述 |
| `ICppBridgeEmitter` | 从冻结 Bridge IR 输出 C header 与桥实现 |

访问级别遵循最小公开原则。稳定发现元数据只声明一次，公共变化同步 Wiki、snapshot、脚本清单及消费者。

### 2. 统一构建与启动流

```text
BuildProfile
→ 目标、部署 compiler、Adapter 与能力预检
→ 解析工程 SDK 和对应 native toolchain
→ 冻结 authoring generation 与内容 closure
→ BGCS / native / managed / content 依赖任务
→ 静态注册生成
→ 托管发布及原生最终链接
→ 平台布局、打包与签名
→ 部署完整性校验
→ 原子提交最终输出

平台 Program
→ 平台 composition
→ 校验代码与内容部署
→ 注入模块来源、类型来源、Adapter 和帧驱动
→ 共用 EngineHost / RuntimeSession / Player
→ 同一事件、场景、渲染、存储生命周期
```

BGCS 输出经 staging 完整验证后替换。Inno Build 使用 BGCS 公开库入口。SDK 与系统工具可作为对应 executor 管理的子进程。

### 3. 新平台具体接入清单

| 新增能力 | 需要实现的位置 | 共用部分 |
|---|---|---|
| iOS 发布 | `Inno.Player.IOS`、`Inno.Build.Platform.IOS`、Apple Toolchain、iOS 托管 compiler | Player Runtime、领域服务、Registry、内容链 |
| CoreCLR WebAssembly | 新 deployment compiler、匹配 SDK/link resolver、验收用例 | Browser 宿主、Core Events、Adapter 和玩法 |
| 主机平台 | SDK 对应宿主、toolchain、packager；SDK 所需 compiler 或 Adapter | 共同运行时及构建契约 |
| 新图形后端 | Rendering Adapter、对应 Shader/原生工具链 | Rendering 公共机制、Graph 和运行系统 |
| BGCS 新原生目标 | 注册 `INativeTargetProvider`，提供正确 ABI 与 SDK 描述 | Parser 分析、IR、Emitter、Runtime |
| BGCS 新 lowering | 注册类型化 lowering，实现中立冻结结果 | Cpp2C 编排、C 桥输出及 C# 链 |

iOS 复用现有 Adapter 时先验证能力、线程、窗口、GPU surface 和生命周期。CoreCLR WebAssembly 更换运行时实现时重新验证 interop、链接、回调及启动能力。

## 五、执行阶段、测试结构与最终验收

### 1. 执行顺序

1. **固化基线与规范**：保存完整结构和规则，记录 revision、SDK、现有构建结果，建立检查入口。
2. **BGCS 边界重构**：规范整理、Core 解耦、IR 迁移、目标 provider、Cpp2C 唯一 IR 流程。
3. **BGCS 独立验收**：托管测试、C/C++ 调用、三种 import mode、Wasm、NativeAOT、负向验证。
4. **Inno 模块与部署**：动态 loader 迁移、静态注册、序列化与代码部署、共享 Player。
5. **Adapter 与 Build**：开放 provider、统一生命周期、managed compiler、Native 目标隔离、Support Pack。
6. **消费者与整体验收**：Canvas、Rendering2D、Samples、Editor、FlappyBird、规范与文档同步。
7. **最终清理**：删除旧路径、冗余 Program、死代码和无用引用，重新运行必要 gate。

每阶段保持目标范围可编译。可修复的实现、架构和测试失败处理完成后继续下一阶段。

### 2. Inno 测试与验证结构

```text
tests/
├─ core/                                           现有基础能力测试
├─ extensibility/
│  ├─ Inno.Extensibility.Modules.Tests/
│  │  ├─ ModuleSourceTests.cs                      [新增]
│  │  └─ CatalogTransactionTests.cs                [新增]
│  ├─ Inno.Extensibility.Types.Tests/
│  │  └─ StaticTypeCatalogTests.cs                 [新增]
│  ├─ Inno.Extensibility.Reload.Tests/
│  └─ fixtures/                                    当前动态及失败模块
├─ runtime/
│  ├─ Inno.Runtime.Tests/
│  │  ├─ StaticDeploymentTests.cs                  [新增]
│  │  └─ PlayerLifecycleTests.cs                   [新增]
│  └─ Inno.Runtime.Generators.Tests/
│     └─ RegistrationGeneratorTests.cs             [新增]
├─ build/
│  └─ Inno.Build.Tests/
│     ├─ ManagedDeploymentTests.cs                 [新增]
│     ├─ ToolchainResolutionTests.cs               [新增]
│     └─ AtomicPublicationTests.cs                 [新增]
├─ editor/
│  ├─ Inno.Editor.PlayMode.Tests/
│  ├─ Inno.Editor.Interactions.Tests/
│  ├─ Inno.Editor.Inspection.Tests/
│  └─ Inno.Editor.Graph.Tests/
├─ native/                                         真实原生契约验证
├─ assets/
├─ scene/
├─ references/
├─ rendering/
├─ audio/
├─ input/
├─ storage/
├─ text/
├─ ui/
├─ plugins/
├─ scripting/
└─ tooling/
   └─ Inno.Tooling.Architecture.Tests/

tools/Inno.Tooling.Architecture/
├─ ArchitectureValidator.cs
├─ ArchitectureRules.cs
├─ PublicApiBoundaryValidator.cs
├─ PublicApiDocumentationValidator.cs
├─ GenerationCleanupValidator.cs
└─ CSharpStyleValidator.cs                         [新增]
```

用实际公开边界验证成功、失败、取消和代际清理。生成器测试同时检查生成代码能编译并运行。

### 3. BGCS 测试结构

```text
tests/
├─ BGCS.Core.Tests/
├─ BGCS.CppAst.Tests/
├─ BGCS.Configuration.Tests/
├─ BGCS.Generation.Tests/
├─ BGCS.Cpp2C.Configuration.Tests/
├─ BGCS.Cpp2C.Tests/
├─ BGCS.Language.Tests/
├─ BGCS.Patching.Tests/
├─ BGCS.Runtime.Tests/
├─ BGCS.Tool.Tests/
├─ BGCS.Tests/
├─ fixtures/
│  ├─ NativeApi/                                   [新增] C ABI 向量
│  └─ CppFacade/                                   [新增] 构造、继承、销毁等向量
├─ wasm/
│  ├─ Native/
│  │  ├─ api.h
│  │  └─ api.c
│  ├─ Consumer.project.xml
│  └─ wwwroot/
│     └─ main.js
└─ native-aot/                                     [新增]
   ├─ Consumer.project.xml
   └─ 验收消费者模板

scripts/
├─ test-wasm-bindings.py
├─ test-native-aot-bindings.py                      [新增]
└─ 环境准备及薄测试入口

.github/workflows/
├─ ci.yml                                          同步架构、规范和调用矩阵
└─ 发布 workflow                                  同步 package 与消费者 gate
```

三种 import mode 均验证实际调用。覆盖数据布局、bool、指针、long、返回值、回调、borrowed/owned context、重复释放、缺失符号及失败清理。

注入原生错误后报告必须失败。BGCS 测试执行独立于引擎。

### 4. 必须通过的 gate

- 源码命名、排版、显式导入、公开 XML 与项目边界检查。
- 无循环依赖、反向源码归属或共享层平台判断。
- Native 目标与生成指纹隔离，缓存及 Support Pack 不串用。
- 全部 BGCS 托管测试及独立原生调用测试。
- Windows NativeAOT Player 实际发布与运行。
- Web 解释执行与 AOT 的实际发布、链接和运行。
- Editor reload 成功、失败、强引用残留、GC barrier 与 Faulted 行为。
- GameView 焦点、失焦释放、菜单、Modal、重叠窗口与 Session 隔离。
- FlappyBird 的玩法、持久化、声音、UI、颜色、光照方向和夜晚星星效果。
- Canvas、Rendering2D、Samples 及脚本 API 的编译与消费。
- SDK 缺失、能力不支持、构建取消、并发、原子提交及旧输出保留。
- Wiki、项目索引、签名、目录树和 Markdown 链接一致。

### 5. 验收报告与默认边界

分别交付 InnoEngine、BGCS 验收报告及联调汇总，包含最终结构、API 变化、删除清单、验证命令、环境、结果和证据路径。

采用以下默认边界：

- 保持当前工程声明的目标框架，由工程解析所需 SDK 与 workload。
- 本次实现 Windows、macOS、Web 的架构替换，以及 Windows NativeAOT、Web AOT 路径。
- iOS、主机及未来 CoreCLR WebAssembly 按上面的具体结构记录扩展方案。
- FlappyBird 使用 `InnoEngine.Samples` 中的项目。
- 当前 Windows 环境执行本机可完成的验收；macOS、iOS 和主机实机状态独立报告。
- 现有报告作为历史证据，本次重新生成验证结果。
- 完成后交付可审阅改动，不自动提交。

执行模式已于 2026-10-04 开始。实际实现与验证状态以文首执行清单及验收报告为准。

## 完成后的逐项核对

实际源码归属与完整要求对应见[Plan 逐项核对](PLATFORM_RUNTIME_PLAN_AUDIT.md)和
[本次验收报告](PLATFORM_RUNTIME_ACCEPTANCE.md)。未来节点仍保持上面的原始设计边界。
