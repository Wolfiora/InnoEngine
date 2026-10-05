# Inno.Runtime

[Runtime 索引](README.md) · [Player](Inno.Player.md) · [Scene](../scene/Inno.Scene.md) · [Assets](../assets/Inno.Assets.md)

## 未退休 generation 的依赖保护

EngineHost / RuntimeSession 退出前通过共享 `GenerationCoordinator.EnsureRetirementSafe()` 检查仍在使用的代际资源。
普通退出错误仍可继续逆序清理；已报告 Pending/timeout 的 generation 不允许销毁 Session、metadata 或设备所依赖的服务。
该状态要求重启 Host，不提供 Reset、跳过或旧 generation 的静默复用。

## 退休顺序与重试

Pipeline 进入 stopping 后拒绝新帧；pending 子系统不会被标为已释放，context resources 保留。Session 仅在实际退休后从 Host 注销；pending 时会有界排空 Job 主线程队列，允许完成依赖主线程的回调。Host 在所有 Session/Host Pipeline 退休前保留 metadata owner，并拒绝新增 owner。

该行为由 `RuntimeRetirementTests` 通过公开 factory、Session 与 Host 接口验证；没有测试访问后门。

构造器不再运行用户 factory/Attach。Host 先登记 Session/Pipeline，随后内部 Start；失败启动由同一 owner
按 `EngineHostBuilder.UseRetirementTimeout(timeout)` 有界排空（默认 30 秒）。无法完成时保留未退休 owner，
Fault 共享 generation gate；普通启动失败在完整补偿后可以重试。退休途中已发生的清理异常不会因 Pending 重试丢失。

`RuntimeSessionOptions.assetPreparationBudgetBytes` 默认 64 MiB，独立于 `assetResidencyBudgetBytes`：前者限制并发冷读暂存 bytes，后者限制已物化 payload 驻留。Player 创建 AssetDatabase 时同时传入两者，预算无效在创建 Session 时拒绝。

`Inno.Runtime` 是 Editor Play Mode 与独立 Player 共用的实例化执行宿主。它拥有 Host/Session 生命周期、脚本执行上下文和部署清单，但不拥有窗口、图形后端、Build 或 Editor UI。

## 所有权模型

`EngineHost` 持有可跨 Session 共享但仍按 Host 隔离的 Module、Type、Serialization、Logging 和 Diagnostics 服务。`RuntimeSession` 持有 SceneWorld、Identity、Job、Coroutine、Event、Clock、Session Log，以及可选的只读 `AssetDatabase`。一个 Host 可以同时创建多个互不污染的 Edit、Play 或 Player Session。

```csharp
using EngineHost host = new EngineHostBuilder()
    .UseMetadataSources(moduleSource, typeSource, serializationMetadataSource)
    .UseMetadataCache(metadataCacheDirectory)
    .Build();

using RuntimeSession play = host.CreateSession(new RuntimeSessionOptions
{
    kind = RuntimeSessionKind.Play,
    applicationId = "sample.game",
    persistentDataDirectory = Path.Combine(userDataRoot, "sample.game")
});

play.Tick(deltaTime);
```

`RuntimeSessionOptions.persistentDataDirectory` 由宿主选择并必须是有效的可写目录；Player 导出默认使用系统 Local Application Data 下的 Application ID 目录，也可以在 Build Settings 或一次导出中指定其下的可移植相对子目录。`Player` Session 还必须提供已经物化的 `runtimeContentDirectory`；Edit/Play 可以由 Editor 组合 authoring 资产服务。

## 公开 API

生命周期协议类型已经位于 [Inno.Runtime.Contracts](Inno.Runtime.Contracts.md)；`RuntimeSubsystemPipeline` 仍属于本项目。`RuntimeSession.subsystems` 是 Session pipeline；`EngineHost.CreateHostPipeline` 持有 Host pipeline；Shell 借用 Host pipeline 驱动输出。场景模拟是 Runtime 内部 bridge，不被低层 Contracts 引用。

`EngineHost.generations` 与 `ModuleHost.generations` 指向同一个 GenerationCoordinator。CreateSession 受 gate 约束；Build/Export 在整段异步消费期间持有 read lease。Dispose 先释放 Session、host pipeline、registry/module 等 owner，再等待弱 unload monitor，不能先 GC 再假定依赖已经释放。

| API | 作用 |
| --- | --- |
| `EngineHostBuilder` | 配置显式模块、类型、序列化元数据来源与 Host 生命周期，并构建实例。 |
| `UseMetadataSources(modules, types, serialization)` | 必填的来源组合；Editor 选择动态 Adapter，Player 选择生成目录。缺失来源在 Build 时失败。 |
| `UseRetirementTimeout(timeout)` | 配置启动补偿、Session 和 Host 退休的正值 deadline；不是允许跳过清理的超时 |
| `EngineHost` | 拥有应用级实例服务并创建隔离 Session。 |
| `RuntimeSessionOptions` | 定义 Session 角色、持久目录、运行内容、资产驻留预算、固定步长、Job、Subsystem factory 与 owner-provided reference resolvers。 |
| `RuntimeSession` | 暴露只读 Session 状态、SceneWorld、EventDispatcher、Reference Catalog、可选 AssetDatabase、Subsystem pipeline、执行作用域与 `Tick`。 |
| `RuntimeSubsystemId` / `RuntimeSubsystemDescriptor` | 以开放稳定 ID、依赖 DAG 和顺序描述一个 Session 能力。 |
| `IRuntimeSubsystemFactory` / `RuntimeSubsystemContext` | Contracts 中的装配协议，由 Composition 为 Host 或 Session 创建隔离子系统。 |
| `RuntimeSubsystem` / `RuntimeSubsystemPipeline` | 固定阶段调度、严格 frame scope、逆序 detach/dispose 与依赖验证。 |
| `RuntimeSessionOptions.capabilities` | Composition 提供并在 Session 创建时冻结的中立能力 ID 集合 |
| `EngineHost.CreateHostPipeline(factories, capabilities)` | 对 Host 使用同一 Required/Optional、能力、依赖和补偿策略 |
| `RuntimeSubsystemPipeline.startupDiagnostics` | 不可用 Optional 子系统的中立诊断快照，同时进入 Core DiagnosticHub；退休时撤销 |
| `RuntimeSessionKind` | 区分 `Edit`、`Play` 和 `Player` 所有权语义。 |
| `GameRuntimeManifest` | 描述当前 Player 的应用 ID、持久数据子目录、产品名、启动 Scene、窗口、Plugin 设置贡献和冻结模块 generation。 |
| `RuntimeManifestEnvelope.ReadPersistentDataPath` | 在初始化序列化服务前读取并严格验证可移植的 Player 数据目录，与完整 manifest 解码时的值必须一致。 |
| `GameRuntimePlugin` | 保存依赖有序的中立 Plugin 设置贡献，不保存 Plugin `Type`、实例或 delegate。 |
| `GameRuntimeModule` / `GameRuntimeAssembly` | 保存依赖有序的逻辑模块、domain、程序集名称和内容指纹，不保存运行时 `Assembly`。 |
| `GameCodeDeployment` / `GameCodeModule` / `GameCodeAssembly` | 校验并冻结逻辑代码部署；静态激活器核对它与实际链接代码闭包的一致性。 |
| `GamePresentationSettings`, `GamePresentationViewport` | 定义 Game View 与 Player 共用的参考帧、aspect-preserving 策略及确定性居中内容区域。 |

## 脚本执行上下文

当前 `Time`、`SceneManager`、`Log`、`Assets`、`Input`、`Storage`、`Animation` 和 `Settings` 等 Unity 风格门面只解析当前
Session/Host 的执行作用域。`RuntimeSession.EnterExecutionScope()` 绑定 Log、Diagnostics、Session
identity、Scene、Clock 与可选 `AssetDatabase`；Player Composition Root 同时绑定它拥有的
`ProjectSettingsStore`。引擎实例服务不调用这些门面。无活动 Session、Scope 乱序释放或 Session
已 Dispose 时都会明确失败，因此并行 Session 不共享静态可变状态。

`Inno.Input.Runtime` 在 `BeginFrame` 捕获不可变键鼠快照并绑定严格 execution scope；SDL3 adapter 在 Host poll 后端事件时累积状态。Input Actions、rebinding、gamepad 与 text/IME 仍是后续增量，其中 Action 映射保持 Plugin 边界。

`RuntimeSession.references` 是本 Session 唯一的 immutable `ReferenceCatalog`。Edit/Play 的 Composition Root
通过 `RuntimeSessionOptions.referenceResolvers` 注入 authoring Asset resolver；Player 的只读
`AssetDatabase` 自动加入同一 generation。需要 reference 服务的能力由 Composition 显式注入；
低层 RuntimeSubsystemContext 不持有 Session/ReferenceCatalog，也不提供 service locator。

Runtime Subsystem 类型是 Host/Composition 的公开装配契约，但故意不进入 gameplay Scripting API；当前该项目
只向 `InnoEngine.Core` 脚本 namespace 导出 `Time`。游戏脚本使用具体领域 façade，不能改写 Session pipeline。

`RuntimeSubsystemPipeline` 没有公开构造器；它必须由 `EngineHost` 创建或从 `RuntimeSession.subsystems` 借用，
以保证 factory 失败时仍然存在能继续退休的 owner。能力判断使用 `RuntimeCapabilityId`，不暴露 native 或 adapter 实现类型。

## 部署内容

`StaticTypeCatalogSource(IReadOnlyList<Action<ITypeCatalogRegistrar>>)` 执行各程序集的生成目录，
随后冻结声明、发现事实及工厂。`GetTypes` 只返回声明，`GetMetadata` 返回 Catalogs 契约的事实；
`ConstructGenericType` 解析已经链接的封闭构造；编译期确认不满足约束的组合返回 null，
合法但缺失的构造抛出 NotSupportedException；
`CanCreateInstance` 和 `CreateInstance` 使用同一工厂表。
工厂按需执行，返回类型错误明确失败；泛型构造缺少所属定义时在组合阶段失败。
重复声明失败，同一封闭工厂重复贡献幂等。注册器不能跨线程使用或在组合结束后继续写入。

Player 的 Runtime Session 使用 `AssetDatabase` 读取物化后的 Catalog 和 Artifact Bundle。Build 将运行时代码闭包固化为逻辑模块和程序集指纹，由所选托管 compiler 完成最终链接。Player 校验 `GameCodeDeployment` 与生成目录，通过同一个 `ModuleHost` 候选事务激活 Plugin 和 Game Scripts，使 TypeCache、Serialization 和 Rendering Registry 共享同一 generation。内容只从部署产物读取；`RuntimeManifestEnvelope` 严格检查当前格式的 magic 和内容。

`FileRenderTargetArtifactProvider` 只读取部署内容，返回 `Ready` 或 `Unavailable`，不会伪造 Editor 的异步 `Pending` 状态。损坏的 Shader envelope 或空 Texture artifact 会抛出严格数据异常；Player 不调用编译器进行运行时补救。

## 生命周期与错误

- `RuntimeSession.Tick` 先打开 Feature frame，再刷新 Event、Coroutine 和 Job；fixed/update/late/render/end 阶段按 Feature DAG 顺序执行，结束与释放按逆序补偿。Edit Session 不执行游戏生命周期。
- `RuntimeSession.Dispose` 释放 Scene、Asset、Scheduler、Serialization generation 和 Session Log；`EngineHost.Dispose` 会逆序释放仍存活的 Session。
- Host 或 Session 的 Dispose 会聚合普通终态清理失败；Pending 则保留依赖并阻止后续退休，不能当作普通错误继续销毁。
- Session 日志携带明确 `LogSessionId`，Editor Console 不再根据 Assembly Scope 猜测来源。

[下一页：Player](Inno.Player.md)

`EngineHostBuilder.UseLogDelivery(LogDeliveryMode)` 显式指定 Host router 与所有 Session file sink 的后台或 Inline 交付策略；未知值在配置阶段失败。默认 Background，平台入口按实际线程能力选择。Inline 的 Session 日志在生产者调用返回前写入文件，不隐式创建日志线程。
