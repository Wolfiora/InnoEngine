# Inno.Engine.Default

[Runtime 索引](README.md) · [Wiki 首页](../README.md) · [Shell](Inno.Shell.md) · [Adapter catalog](Inno.Adapter.Default.md)

## 定位

这是 Composition 中的默认引擎装配政策，不是 backend，也不是全局 Service Locator。它引用中立 adapter catalog 和领域 Runtime；具体 SDL3/BGFX/MiniAudio 选择仍由 `Inno.Adapter.Default` 的实现工厂完成。

```text
Shell
  ├─ EditorHost / GamePlayerHost
  ├─ Inno.Engine.Default → generated subsystem factory catalog
  └─ Inno.Adapter.Default → backend-neutral factory → concrete adapter → native
```

## 公开入口

| API | 语义 |
| --- | --- |
| `EngineSessionComposition` | 显式绑定 session、adapters、selection、input、artifacts、audioSettings 和可选 audioOverride；不查找隐藏全局状态 |
| `DefaultEngine.CreateSessionSubsystems(context)` | 生成 Input、Storage、Animation、Audio、Text、UI 的 factory 集合；Scene 模拟由 RuntimeSession 自身的 Scene bridge 加入 |
| `DefaultEngine.CreateHostSubsystems(renderRuntime)` | 生成 Host 范围的 Rendering factory；Editor 与 Play 不各自提交一次同一 GPU device frame |

内部 `Audio/`、`Input/`、`Storage/`、`Animation/`、`Text/`、`UI/`、`Rendering/` 文件各声明本领域装配。UI 依赖 Input 和 Text；Audio 初始化失败明确报告 muted，Animation 创建 TypeCatalog-driven binding runtime；Reporter 和辅助 registration 转交 context.resources，最终由 pipeline 逆序释放。

Session 级 Reporter 的 source ID 必须包含实际 owner 范围。Animation 使用 `RuntimeSubsystemContext.identities.domainId`，因此共用一个 DiagnosticHub 的 Edit、Play 和其他并存 Session 不会互相撤销 Reporter。这个 domain ID 只区分运行期诊断所有权，不写入资产或 History。相同 source 的新 Reporter 仍会撤销旧代，过期发布仍明确失败；不能通过吞掉异常或取消代际检查来处理会话冲突。资源顺序保持为先创建 Reporter、再创建借用它的 binding owner，退出时先释放 bindings、最后释放 Reporter。

生成器通过 `OutputItemType="Analyzer"`、`ReferenceOutputAssembly="false"` 引入，不是运行时依赖。Editor 可传入自己的 session audio factory，以保留预览/Play 管理；它不需要复制整个默认子系统列表。

## 错误与扩展

重复 ID、错误 owner lifetime、缺少依赖或循环依赖拒绝启动。设备选择是中立 enum，不允许在 Host public/protected API 返回 concrete adapter 或 Native handle。这里不自动安装、创建或启用任何玩法 Plugin。

### Edit / Play 诊断隔离验证（2026-09-08）

原先 Animation 使用固定 source `inno.animation.bindings`，创建 Play 会话会撤销仍在使用的 Edit Reporter；随后更新或退出触发 `DiagnosticReporter` 过期异常，并使退休 gate 进入 Faulted。修复位于默认工厂的 source 构造，不改变 Core Reporter 的代际保护或 GC 屏障。

通过公开 `DefaultEngine.CreateSessionSubsystems` 和 `EngineHost.CreateSession` 的仓库外验证程序，修复前复现相同退休异常；修复后通过 32 次 Edit/Play 循环、三个并存会话、不同退出顺序、Missing 诊断互不覆盖与 Host 正常释放。现有 Diagnostics、Animation、Runtime、Editor PlayMode、Editor Audio、Editor Scripting 六个测试项目共 128 项通过；Solution 零警告/错误，Architecture 通过。本次未修改 tests，不把这些程序化验证称为真实 GUI 点击验收或全量发行验收。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Engine.Default.DefaultEngine`

<!-- api: T:Inno.Engine.Default.DefaultEngine aa43ae53b19b59c8d3a8c8dc1ae9a4c710f6324b0665ee5ed094d92e6c4a23eb -->
<!-- api: M:Inno.Engine.Default.DefaultEngine.CreateSessionSubsystems(Inno.Engine.Default.EngineSessionComposition) 8fa6bea86db4a6a311ff9b5fcfcd49f63d42c1c58751eee7c289fef62c329a7b -->
<!-- api: M:Inno.Engine.Default.DefaultEngine.CreateHostSubsystems(Inno.Rendering.Runtime.RenderRuntime) 2a061cfdc15ba9139db103fbd240e2e584e1982d1431b8d9213b436960530fb1 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-963bc67075c1d0fd"></a>[`public static System.Collections.Generic.IReadOnlyList<Inno.Runtime.Contracts.IRuntimeSubsystemFactory> Inno.Engine.Default.DefaultEngine.CreateHostSubsystems(Inno.Rendering.Runtime.RenderRuntime context)`](../../src/composition/default/Inno.Engine.Default/DefaultEngine.cs#L34) | Creates the default host factory set over the product-configured rendering runtime.<br>返回：A frozen host-lifetime factory list that is never inserted into a session pipeline.<br>`context`：The rendering runtime whose domain resources are owned by this host. | — | — |
| <a id="api-7f727ac402f953e7"></a>[`public static System.Collections.Generic.IReadOnlyList<Inno.Runtime.Contracts.IRuntimeSubsystemFactory> Inno.Engine.Default.DefaultEngine.CreateSessionSubsystems(Inno.Engine.Default.EngineSessionComposition context)`](../../src/composition/default/Inno.Engine.Default/DefaultEngine.cs#L22) | Creates the complete default session factory set from local subsystem declarations.<br>返回：A frozen deterministic factory list; the receiving RuntimeSession owns the created subsystems.<br>`context`：The strongly typed composition inputs borrowed during factory creation. | — | — |
| <a id="api-49f06bf2574683f1"></a>[`public static class Inno.Engine.Default.DefaultEngine`](../../src/composition/default/Inno.Engine.Default/DefaultEngine.cs#L10) | Provides build-generated default engine assembly, independent of Editor and Player loop implementations. | — | — |

### `Inno.Engine.Default.EngineSessionComposition`

<!-- api: T:Inno.Engine.Default.EngineSessionComposition f843297337d221158c66046e816a030a56ff6bbdc25026fa6ed25f6833e3fbce -->
<!-- api: M:Inno.Engine.Default.EngineSessionComposition.#ctor(Inno.Runtime.RuntimeSession,Inno.Adapter.IAdapterCatalog,Inno.Adapter.AdapterSelection,Inno.Adapter.Input.IInputEventSource,Inno.Assets.IAssetArtifactLookup,System.Func{Inno.Audio.AudioProjectSettings},System.Func{Inno.Storage.IApplicationStorage},Inno.Runtime.Contracts.IRuntimeSubsystemFactory) 0ef6c0c2e8c172c61365c54bfd3c0e40ffacd4f5da1ddc3826e42393b763963b -->
<!-- api: P:Inno.Engine.Default.EngineSessionComposition.session c37a7821a01ea5d56b709308ff940f661bf01184e6e64c29db862c83ccccb816 -->
<!-- api: P:Inno.Engine.Default.EngineSessionComposition.adapters 346590c8001e0eb2c3b2139306252a0c1605373817beb038d934f93da68eedc2 -->
<!-- api: P:Inno.Engine.Default.EngineSessionComposition.selection a7636a619f6a0d5859e21e0d98ed89795ab5499b4af079f9f91d433b744a9d57 -->
<!-- api: P:Inno.Engine.Default.EngineSessionComposition.inputSource c4519eba11789bf3d93d3ed7ac2f69e4f8943f391bb966c520580aa4e2042d28 -->
<!-- api: P:Inno.Engine.Default.EngineSessionComposition.artifacts a3793c1f7d1ada1698950b0af6deeb4b86d844ad25e18dbe8f12114e0355a6a9 -->
<!-- api: P:Inno.Engine.Default.EngineSessionComposition.audioSettings 367f33a4e8ccf840d84f414ce3fc34ada8b860cb2f44d700cd748319df657a63 -->
<!-- api: P:Inno.Engine.Default.EngineSessionComposition.createStorage aa17becbd076744b7eecdd7ac69b8861ef335187050e80820a2a0065238de8fe -->
<!-- api: P:Inno.Engine.Default.EngineSessionComposition.audioOverride 03499b953a0f4f7ddaf9452898ca10cb637484fc3c6fd2d7a97c9cbc19e4db32 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f0c4df947b3b4aa5"></a>[`public Inno.Engine.Default.EngineSessionComposition.EngineSessionComposition(Inno.Runtime.RuntimeSession session, Inno.Adapter.IAdapterCatalog adapters, Inno.Adapter.AdapterSelection selection, Inno.Adapter.Input.IInputEventSource inputSource, Inno.Assets.IAssetArtifactLookup artifacts, System.Func<Inno.Audio.AudioProjectSettings> audioSettings, System.Func<Inno.Storage.IApplicationStorage> createStorage, Inno.Runtime.Contracts.IRuntimeSubsystemFactory? audioOverride = null)`](../../src/composition/default/Inno.Engine.Default/EngineSessionComposition.cs#L44) | Captures the inputs borrowed during synchronous default subsystem construction.<br>`session`：The isolated owner receiving these subsystems.<br>`adapters`：The host's neutral adapter catalog.<br>`selection`：The user's backend selections interpreted only by adapter factories.<br>`inputSource`：The host input source used to create isolated session readers.<br>`artifacts`：The selected authoring or deployed artifact lookup.<br>`audioSettings`：The portable audio defaults, captured during factory activation.<br>`audioOverride`：An optional product-specific audio owner, such as Editor preview/Play isolation.<br>`createStorage`：The host factory transferring one isolated storage service to the session. | — | — |
| <a id="api-6debef8063816167"></a>[`public Inno.Adapter.IAdapterCatalog Inno.Engine.Default.EngineSessionComposition.adapters { get; }`](../../src/composition/default/Inno.Engine.Default/EngineSessionComposition.cs#L71) | Gets the backend-neutral adapter factory catalog. | — | — |
| <a id="api-5c1dfca6dce3164e"></a>[`public Inno.Assets.IAssetArtifactLookup Inno.Engine.Default.EngineSessionComposition.artifacts { get; }`](../../src/composition/default/Inno.Engine.Default/EngineSessionComposition.cs#L83) | Gets immutable encoded content artifacts from the selected host environment. | — | — |
| <a id="api-3a8cb042f6c34534"></a>[`public Inno.Runtime.Contracts.IRuntimeSubsystemFactory? Inno.Engine.Default.EngineSessionComposition.audioOverride { get; }`](../../src/composition/default/Inno.Engine.Default/EngineSessionComposition.cs#L95) | Gets an optional product-specific audio owner factory. | — | — |
| <a id="api-9a4b59ad168794c4"></a>[`public System.Func<Inno.Audio.AudioProjectSettings> Inno.Engine.Default.EngineSessionComposition.audioSettings { get; }`](../../src/composition/default/Inno.Engine.Default/EngineSessionComposition.cs#L87) | Gets the control-thread source of portable audio defaults. | — | — |
| <a id="api-648b56d4802cf8a7"></a>[`public System.Func<Inno.Storage.IApplicationStorage> Inno.Engine.Default.EngineSessionComposition.createStorage { get; }`](../../src/composition/default/Inno.Engine.Default/EngineSessionComposition.cs#L91) | Gets the factory transferring storage ownership to this session. | — | — |
| <a id="api-aa15d1190380e2a9"></a>[`public Inno.Adapter.Input.IInputEventSource Inno.Engine.Default.EngineSessionComposition.inputSource { get; }`](../../src/composition/default/Inno.Engine.Default/EngineSessionComposition.cs#L79) | Gets the host input source shared by isolated session readers. | — | — |
| <a id="api-f5f18f301a1a1985"></a>[`public Inno.Adapter.AdapterSelection Inno.Engine.Default.EngineSessionComposition.selection { get; }`](../../src/composition/default/Inno.Engine.Default/EngineSessionComposition.cs#L75) | Gets portable backend selection values. | — | — |
| <a id="api-4fef130c855dc281"></a>[`public Inno.Runtime.RuntimeSession Inno.Engine.Default.EngineSessionComposition.session { get; }`](../../src/composition/default/Inno.Engine.Default/EngineSessionComposition.cs#L67) | Gets the isolated session selected by the product. | — | — |
| <a id="api-7559557c8e79266e"></a>[`public sealed class Inno.Engine.Default.EngineSessionComposition`](../../src/composition/default/Inno.Engine.Default/EngineSessionComposition.cs#L15) | Supplies strongly typed product-owned inputs to the default engine composition without exposing concrete backends. | — | — |

## 项目依赖

- [Inno.Runtime.Generators](Inno.Runtime.Generators.md)：公开引用边界由实际签名核对。
- [Inno.Audio.Runtime](../audio/Inno.Audio.Runtime.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Input.Runtime](../input/Inno.Input.Runtime.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Storage.Runtime](../storage/Inno.Storage.Runtime.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Text.Runtime](../text/Inno.Text.Runtime.md)：实现依赖，PrivateAssets="compile"。
- [Inno.UI.Runtime](../ui/Inno.UI.Runtime.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Animation.Runtime](../animation/Inno.Animation.Runtime.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scene](../scene/Inno.Scene.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Runtime.Contracts](Inno.Runtime.Contracts.md)：公开引用边界由实际签名核对。
- [Inno.Runtime](Inno.Runtime.md)：公开引用边界由实际签名核对。
- [Inno.Adapter](Inno.Adapter.md)：公开引用边界由实际签名核对。
- [Inno.Adapter.Input](../input/Inno.Adapter.Input.md)：公开引用边界由实际签名核对。
- [Inno.Assets](../assets/Inno.Assets.md)：公开引用边界由实际签名核对。
- [Inno.Audio](../audio/Inno.Audio.md)：公开引用边界由实际签名核对。
- [Inno.Rendering.Runtime](../rendering/Inno.Rendering.Runtime.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
