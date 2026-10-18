# Inno.Runtime

[分类索引](README.md) · [Wiki 首页](../README.md)

## 职责与边界

Session 通过 `contentStore` 读取运行内容，通过 `createLogSink` 接管自己的日志 sink；不自行创建文件日志或部署目录。EngineHostBuilder 的 metadata 来源显式注入；动态 shadow-copy 根属于 DotNet module source。Settings 来自注入的文档来源，Player 使用只读文档。Session 退出先停止新工作、取消并完成任务，再退休子系统、资源与事件订阅；失败保留 Pending/Faulted 语义。

## 组合与生命周期

EngineHost 由模块、类型及序列化来源建立；Session 的内容 store 是借用边界，日志 factory 创建的 sink 由 Session 独占释放。Edit、Play 和 Player 使用独立 identity、event、diagnostic 与资源作用域。

```csharp
using Inno.Content;
using Inno.Runtime;

static RuntimeSession CreateSession(
    EngineHost host,
    IRuntimeContentStore content
) {
    return host.CreateSession(new RuntimeSessionOptions
    {
        kind = RuntimeSessionKind.Player,
        applicationId = "sample.game",
        contentStore = content,
        jobExecutionMode = RuntimeJobExecutionMode.SingleThread
    });
}
```

宿主先配置 metadata、完整引用解析上下文和 subsystem factories，再创建 Session。先退出 Session，再关闭 content；创建失败按反序退休已取得资源。Pending 退休保留 owner，Faulted generation 禁止继续 Play/Build/Export，需重启 Host。动态模块路径仅属于 DotNet source，静态模块不需要 shadow-copy cache。

Asset、Settings 和 Rendering 的内容读取分别由其公开契约完成；共享 Session 不读取部署目录，也不创建日志文件。完整 Player 工作流见 [Player Runtime](Inno.Player.Runtime.md)，代际规则见 [Identity 与 Reload](../architecture/IDENTITY_REFERENCE_RELOAD_STANDARD.md)。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Runtime.EngineHost`

<!-- api: T:Inno.Runtime.EngineHost a2619ea3ae97bcf6111a5ad004f097e89fbc675d92fdc52ff2eb3193f704e7f4 -->
<!-- api: P:Inno.Runtime.EngineHost.logs 5cf41455aa860fa5aa1fb2f8766fadfd3b6cd61a3eeacf77e91189e73c7870cd -->
<!-- api: P:Inno.Runtime.EngineHost.diagnostics 5dcd40b98e202c66e179f31005105521521950dcc4912be71aa773a4866bdcd6 -->
<!-- api: P:Inno.Runtime.EngineHost.generations 3c10002f13d1d5a64d3972fbda8b8f99ba015acc0bb780a3713d1e0b5eca5aec -->
<!-- api: P:Inno.Runtime.EngineHost.modules a99110599afd24c99c90f9b9ecb3c387fbc6965aa7ef7d6deb9417bf7dd0b32b -->
<!-- api: P:Inno.Runtime.EngineHost.types 1988b43d8108e7561aebba904e2dc374d6b6c9a1db0e4270de9035a8d2d299ab -->
<!-- api: P:Inno.Runtime.EngineHost.serialization e4047c7255bb1bd358c54194df26322d832ca405492e2bc7d4be5ca9464b57ef -->
<!-- api: M:Inno.Runtime.EngineHost.CreateSession(Inno.Runtime.RuntimeSessionOptions) 774f2140e2a8c485f5dd77ae4bf0c82514e37c1e3d3cc3b7439ded531f2aa15e -->
<!-- api: M:Inno.Runtime.EngineHost.CreateHostPipeline(System.Collections.Generic.IReadOnlyList{Inno.Runtime.Contracts.IRuntimeSubsystemFactory},System.Collections.Generic.IReadOnlyList{Inno.Runtime.Contracts.RuntimeCapabilityId}) 009ee2483dfcdb50a8071a59d17a610f40cf00d7b3b35e3188f04d37c29c5296 -->
<!-- api: M:Inno.Runtime.EngineHost.Dispose 5fc52ceadf46d3f6d0e012205a45572fb2c06c9cdfd36d1e83925a1ca1642708 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b24b9c88102d5b84"></a>[`public Inno.Runtime.RuntimeSubsystemPipeline Inno.Runtime.EngineHost.CreateHostPipeline(System.Collections.Generic.IReadOnlyList<Inno.Runtime.Contracts.IRuntimeSubsystemFactory> factories, System.Collections.Generic.IReadOnlyList<Inno.Runtime.Contracts.RuntimeCapabilityId>? capabilities = null)`](../../src/runtime/engine/Inno.Runtime/Hosting/EngineHost.cs#L143) | Creates host-owned subsystems that outlive individual Edit, Play or Player sessions.<br>返回：A pipeline owned by this EngineHost and borrowed by the application's Shell.<br>`factories`：The complete host factory set; session-lifetime factories are rejected.<br>`capabilities`：Backend-neutral capabilities verified by the application's selected services. | — | `System.ObjectDisposedException`：The engine host is disposed. |
| <a id="api-6b21628f9d91b3d8"></a>[`public Inno.Runtime.RuntimeSession Inno.Runtime.EngineHost.CreateSession(Inno.Runtime.RuntimeSessionOptions options)`](../../src/runtime/engine/Inno.Runtime/Hosting/EngineHost.cs#L104) | Creates an isolated Edit, Play, or Player runtime session.<br>返回：A started session owned by this host and the caller.<br>`options`：The validated ownership, storage, and timing options for the session. | — | `System.ArgumentNullException`：Thrown when options is null.<br>`System.ObjectDisposedException`：Thrown when this host has been disposed. |
| <a id="api-d8f34e31f8504934"></a>[`public void Inno.Runtime.EngineHost.Dispose()`](../../src/runtime/engine/Inno.Runtime/Hosting/EngineHost.cs#L178) | Disposes every owned session before releasing application metadata services. | — | `Inno.Core.Execution.RetirementPendingException`：Owned work is still draining, or a generation deadline failed. Dependencies remain owned. |
| <a id="api-d15bf2c3cb7f27e9"></a>[`public Inno.Core.Diagnostics.DiagnosticHub Inno.Runtime.EngineHost.diagnostics { get; }`](../../src/runtime/engine/Inno.Runtime/Hosting/EngineHost.cs#L67) | Gets the isolated diagnostic state hub owned by this host. | — | — |
| <a id="api-b774e691e5e5dbe4"></a>[`public Inno.Extensibility.Reload.GenerationCoordinator Inno.Runtime.EngineHost.generations { get; }`](../../src/runtime/engine/Inno.Runtime/Hosting/EngineHost.cs#L72) | Gets the shared admission gate for reload, recovery, Play, Build and Export. | — | — |
| <a id="api-4c4ea6455c64f172"></a>[`public Inno.Core.Logging.LogRouter Inno.Runtime.EngineHost.logs { get; }`](../../src/runtime/engine/Inno.Runtime/Hosting/EngineHost.cs#L62) | Gets the isolated logging router using this host's configured delivery policy. | — | — |
| <a id="api-8a6d9a9ac9391a28"></a>[`public Inno.Extensibility.Modules.ModuleHost Inno.Runtime.EngineHost.modules { get; private set; }`](../../src/runtime/engine/Inno.Runtime/Hosting/EngineHost.cs#L77) | Gets the isolated managed module host that owns this engine host's reload generations. | — | — |
| <a id="api-b8f3002c8cccd3e0"></a>[`public Inno.Core.Serialization.SerializationRegistry Inno.Runtime.EngineHost.serialization { get; private set; }`](../../src/runtime/engine/Inno.Runtime/Hosting/EngineHost.cs#L87) | Gets the isolated serialization registry derived from this host's active type generation. | — | — |
| <a id="api-26174eb31fe2f38a"></a>[`public Inno.Extensibility.Types.TypeCatalog Inno.Runtime.EngineHost.types { get; private set; }`](../../src/runtime/engine/Inno.Runtime/Hosting/EngineHost.cs#L82) | Gets the isolated immutable type catalog derived from this host's active modules. | — | — |
| <a id="api-bce3909bbd9352b8"></a>[`public sealed class Inno.Runtime.EngineHost : System.IDisposable`](../../src/runtime/engine/Inno.Runtime/Hosting/EngineHost.cs#L20) | Owns application-level engine services and creates isolated runtime sessions. | — | — |

### `Inno.Runtime.EngineHostBuilder`

<!-- api: T:Inno.Runtime.EngineHostBuilder 1ded3595939014b605676688ea929f51813ed5b7fdcb5709688cb85c16f19e31 -->
<!-- api: M:Inno.Runtime.EngineHostBuilder.UseRetirementTimeout(System.TimeSpan) 57186bcebbb07e3c1b6e9b8e13b993b0fabff80efe634118e0630a7d2ec369f7 -->
<!-- api: M:Inno.Runtime.EngineHostBuilder.Build 3122be57dd532348170518f0327d4630d5d3c4c022975f7f042c486b366db92d -->
<!-- api: M:Inno.Runtime.EngineHostBuilder.UseMetadataSources(Inno.Extensibility.Modules.IAssemblyCatalogSource,Inno.Extensibility.Types.ITypeCatalogSource,Inno.Core.Serialization.ISerializationMetadataSource) 65bf11931fc5e0062fb2c903341de9328b2fde24679f5260bdfc46afb89b8d2e -->
<!-- api: M:Inno.Runtime.EngineHostBuilder.UseLogDelivery(Inno.Core.Logging.LogDeliveryMode) bd83ff02e950ca9c1bc3042822dfde94d7754a0cda2160d918efde84df13ce78 -->
<!-- api: M:Inno.Runtime.EngineHostBuilder.#ctor ec5824b6952798b47e25c801ac97df37c937d3da92a1527ce4852623619063b2 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9add284ee3ac519d"></a>[`public Inno.Runtime.EngineHostBuilder.EngineHostBuilder()`](../../src/runtime/engine/Inno.Runtime/Hosting/EngineHostBuilder.cs#L12) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-c91d390e81b80540"></a>[`public Inno.Runtime.EngineHost Inno.Runtime.EngineHostBuilder.Build()`](../../src/runtime/engine/Inno.Runtime/Hosting/EngineHostBuilder.cs#L46) | Creates an application host and acquires its immutable metadata services.<br>返回：A host owned by the caller. | — | — |
| <a id="api-7280e6cd2d6588f4"></a>[`public Inno.Runtime.EngineHostBuilder Inno.Runtime.EngineHostBuilder.UseLogDelivery(Inno.Core.Logging.LogDeliveryMode deliveryMode)`](../../src/runtime/engine/Inno.Runtime/Hosting/EngineHostBuilder.cs#L101) | Selects the host router's delivery policy for host and session sinks.<br>返回：This builder for fluent host composition.<br>`deliveryMode`：Background worker or inline producer-thread delivery. | — | `System.ArgumentOutOfRangeException`：The policy is undefined. |
| <a id="api-2803bf9bd7a1e777"></a>[`public Inno.Runtime.EngineHostBuilder Inno.Runtime.EngineHostBuilder.UseMetadataSources(Inno.Extensibility.Modules.IAssemblyCatalogSource modules, Inno.Extensibility.Types.ITypeCatalogSource types, Inno.Core.Serialization.ISerializationMetadataSource serialization)`](../../src/runtime/engine/Inno.Runtime/Hosting/EngineHostBuilder.cs#L73) | Selects the code and metadata implementation before creating the application host.<br>返回：This builder for fluent composition.<br>`modules`：The host catalog source; ownership transfers to the built EngineHost.<br>`types`：The type metadata provider corresponding to the same code deployment strategy.<br>`serialization`：The declaration access provider corresponding to the same managed deployment. | — | — |
| <a id="api-01ab5206907e994d"></a>[`public Inno.Runtime.EngineHostBuilder Inno.Runtime.EngineHostBuilder.UseRetirementTimeout(System.TimeSpan timeout)`](../../src/runtime/engine/Inno.Runtime/Hosting/EngineHostBuilder.cs#L32) | Sets the maximum owner-thread drain duration before retirement faults the host.<br>返回：This builder for fluent configuration.<br>`timeout`：A positive deadline shared by failed startup, session stop and host shutdown. | — | `System.ArgumentOutOfRangeException`：The timeout is not positive. |
| <a id="api-5b69221d0cee0cef"></a>[`public sealed class Inno.Runtime.EngineHostBuilder`](../../src/runtime/engine/Inno.Runtime/Hosting/EngineHostBuilder.cs#L12) | Collects application-level runtime services before creating an Inno.Runtime.EngineHost. | — | — |

### `Inno.Runtime.GameCodeAssembly`

<!-- api: T:Inno.Runtime.GameCodeAssembly c416822ab7b5a24833b2a7d911b6749bcab624d5cb002ff1106ed1a3a3e64abc -->
<!-- api: M:Inno.Runtime.GameCodeAssembly.#ctor(System.String,System.String) 34775b0a4d3ca2dbe4cea3fb38dc319377d691e4e7911c0b393862d032834d83 -->
<!-- api: P:Inno.Runtime.GameCodeAssembly.name 131c91bc4d7b049de37cb7a84ad509ccebe71f3d81fe8756b51d6e237d0ddfe7 -->
<!-- api: P:Inno.Runtime.GameCodeAssembly.contentFingerprint e01b0a865985cae428551d38b2d608520b44cacc3a0ee8bc893893490d21ed7c -->
<!-- api: M:Inno.Runtime.GameCodeAssembly.ToString 23983d6cb5fd3092ae218cde7bb25f9f3921126eddc3c22d3a0b1c103a3185eb -->
<!-- api: M:Inno.Runtime.GameCodeAssembly.op_Inequality(Inno.Runtime.GameCodeAssembly,Inno.Runtime.GameCodeAssembly) a40ce82e12284ff1c82b743051c76bff2da9fcd90e6c191d8623f12bd8397637 -->
<!-- api: M:Inno.Runtime.GameCodeAssembly.op_Equality(Inno.Runtime.GameCodeAssembly,Inno.Runtime.GameCodeAssembly) 3cd8797cf55fd90b22e59909befeb4d1f2845e65d2122538d8d2657c83010387 -->
<!-- api: M:Inno.Runtime.GameCodeAssembly.GetHashCode fdbf5945c5f83b6333c21af187f8d02d4f4bbf6f2f3c992e65de8eccd4d8a6cf -->
<!-- api: M:Inno.Runtime.GameCodeAssembly.Equals(System.Object) f6f4d722d82f8636ccf3d02e3aa2c547bd363262a4a7d9cac86b5eef02592197 -->
<!-- api: M:Inno.Runtime.GameCodeAssembly.Equals(Inno.Runtime.GameCodeAssembly) 4facc2885481a2b77db2d3438c4e32b326140015f35837f7700c4d7fae494099 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b329cff2b369d9e1"></a>[`public Inno.Runtime.GameCodeAssembly.GameCodeAssembly(string name, string contentFingerprint)`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeAssembly.cs#L22) | Validates and freezes one logical assembly identity.<br>`name`：The exact simple assembly name.<br>`contentFingerprint`：The lowercase SHA-256 fingerprint of the frozen compiler output. | — | `System.IO.InvalidDataException`：The supplied code identity is malformed. |
| <a id="api-e7231b4556d45668"></a>[`public bool Inno.Runtime.GameCodeAssembly.Equals(Inno.Runtime.GameCodeAssembly? other)`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeAssembly.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-6abce00409773312"></a>[`public override bool Inno.Runtime.GameCodeAssembly.Equals(object? obj)`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeAssembly.cs#L8) | Determines whether the specified object is equal to the current object.<br>返回：true if the specified object is equal to the current object; otherwise, false.<br>`obj`：The object to compare with the current object. | — | — |
| <a id="api-78fa811f6f6e5c20"></a>[`public override int Inno.Runtime.GameCodeAssembly.GetHashCode()`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeAssembly.cs#L8) | Serves as the default hash function.<br>返回：A hash code for the current object. | — | — |
| <a id="api-9b9460d601540076"></a>[`public static bool Inno.Runtime.GameCodeAssembly.operator ==(Inno.Runtime.GameCodeAssembly? left, Inno.Runtime.GameCodeAssembly? right)`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeAssembly.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-e75909ad8deb874b"></a>[`public static bool Inno.Runtime.GameCodeAssembly.operator !=(Inno.Runtime.GameCodeAssembly? left, Inno.Runtime.GameCodeAssembly? right)`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeAssembly.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-481ac67e741f85de"></a>[`public override string Inno.Runtime.GameCodeAssembly.ToString()`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeAssembly.cs#L8) | Returns a string that represents the current object.<br>返回：A string that represents the current object. | — | — |
| <a id="api-495c5592d0aafdaf"></a>[`public string Inno.Runtime.GameCodeAssembly.contentFingerprint { get; }`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeAssembly.cs#L39) | Gets the build input's content fingerprint. | — | — |
| <a id="api-6c307d6d57d9ccce"></a>[`public string Inno.Runtime.GameCodeAssembly.name { get; }`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeAssembly.cs#L34) | Gets the exact simple assembly name. | — | — |
| <a id="api-992b8a56ea05d1a3"></a>[`public sealed record Inno.Runtime.GameCodeAssembly : System.IEquatable<Inno.Runtime.GameCodeAssembly>`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeAssembly.cs#L8) | Identifies one immutable code input independently of its physical deployment representation. | — | — |

### `Inno.Runtime.GameCodeDeployment`

<!-- api: T:Inno.Runtime.GameCodeDeployment 2b6718ff4c71247306783f619811c5670408d35252b184f3e1e383b5d22f6ca4 -->
<!-- api: M:Inno.Runtime.GameCodeDeployment.#ctor(System.Collections.Generic.IReadOnlyList{Inno.Runtime.GameCodeModule}) 7ea0b4cbe5d0abd6a6a1c0167de76acabada6c54d6324a482d505ccab058bdb9 -->
<!-- api: P:Inno.Runtime.GameCodeDeployment.modules c21cf68c79497fa5f1423ebe2061964490be4600bfd30b6823e6886d30419e47 -->
<!-- api: M:Inno.Runtime.GameCodeDeployment.FromManifest(System.Collections.Generic.IReadOnlyList{Inno.Runtime.GameRuntimeModule}) b20bb456f22bd8fc89e0fb6ec3605167fbfe2416b36687e814ce614d171c25aa -->
<!-- api: M:Inno.Runtime.GameCodeDeployment.ValidateMatches(Inno.Runtime.GameCodeDeployment) 2710f2da5f32975b15b79af928dd1e78e5ab701a45ca284159feffd2084acfaf -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-90f055bfa1288241"></a>[`public Inno.Runtime.GameCodeDeployment.GameCodeDeployment(System.Collections.Generic.IReadOnlyList<Inno.Runtime.GameCodeModule> modules)`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeDeployment.cs#L22) | Copies an explicit code closure and verifies unique ownership and dependency ordering.<br>`modules`：Ordered immutable module contributions; an empty closure describes an engine-only composition. | — | `System.IO.InvalidDataException`：A module is null or duplicated, an assembly has multiple owners, or an upstream module is absent. |
| <a id="api-5a5c17c6f19e7d62"></a>[`public static Inno.Runtime.GameCodeDeployment Inno.Runtime.GameCodeDeployment.FromManifest(System.Collections.Generic.IReadOnlyList<Inno.Runtime.GameRuntimeModule> modules)`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeDeployment.cs#L58) | Freezes validated manifest declarations without retaining their mutable arrays or entries.<br>返回：An immutable logical deployment; invalid declarations fail before activation.<br>`modules`：Current-format runtime manifest declarations. | — | `System.IO.InvalidDataException`：A declaration or its ownership and dependencies are invalid. |
| <a id="api-c32544cc2708e388"></a>[`public void Inno.Runtime.GameCodeDeployment.ValidateMatches(Inno.Runtime.GameCodeDeployment expected)`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeDeployment.cs#L81) | Rejects a manifest that differs from the code closure linked by the build.<br>`expected`：The independently frozen deployment to compare with this closure. | — | `System.IO.InvalidDataException`：Module ordering, ownership, dependencies, code names or fingerprints differ. |
| <a id="api-0a60e88148613896"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Runtime.GameCodeModule> Inno.Runtime.GameCodeDeployment.modules { get; }`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeDeployment.cs#L44) | Gets the immutable dependency-ordered code closure. | — | — |
| <a id="api-707ec0b2cbae55b5"></a>[`public sealed class Inno.Runtime.GameCodeDeployment`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeDeployment.cs#L11) | Owns a validated, immutable and dependency-ordered logical game code closure. | — | — |

### `Inno.Runtime.GameCodeModule`

<!-- api: T:Inno.Runtime.GameCodeModule 164becca76765b40d72962c740d19f68a6de2fc8256667e0706d542cde5c37d7 -->
<!-- api: M:Inno.Runtime.GameCodeModule.#ctor(System.String,Inno.Extensibility.Modules.AssemblyDomain,System.Collections.Generic.IReadOnlyList{Inno.Runtime.GameCodeAssembly},System.Collections.Generic.IReadOnlyList{System.String}) 4fb6ab6740fe99e590260ec7efdb572f30ed898216d66e8ec7fa1aac2e7ebf52 -->
<!-- api: P:Inno.Runtime.GameCodeModule.name 37304e8e80e0a4128f3090a8509e17d14b3f744656d79a4851ad33d88ae65709 -->
<!-- api: P:Inno.Runtime.GameCodeModule.domain 7da385a9923709cd4ba4450e8f1d3fac072b7e81f3ec82e170ed34081120948e -->
<!-- api: P:Inno.Runtime.GameCodeModule.assemblies 7c200fd904eb1b608a5e84755aaaedf3a3300efe972a788be0791fa887d4f7d0 -->
<!-- api: P:Inno.Runtime.GameCodeModule.dependencies c243813babf2676d83a09b2a8fa0bd5cc0fc4abc0782fc1e403e3f6a452225bb -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-6e89260d5eb4009b"></a>[`public Inno.Runtime.GameCodeModule.GameCodeModule(string name, Inno.Extensibility.Modules.AssemblyDomain domain, System.Collections.Generic.IReadOnlyList<Inno.Runtime.GameCodeAssembly> assemblies, System.Collections.Generic.IReadOnlyList<string> dependencies)`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeModule.cs#L31) | Copies and validates a module contribution without retaining mutable manifest objects.<br>`name`：The stable module identity.<br>`domain`：The ownership domain of this game's code.<br>`assemblies`：The exact ordered code identities to activate.<br>`dependencies`：The logical modules that precede this contribution. | — | `System.IO.InvalidDataException`：The module identity, domain, code or dependencies are invalid. |
| <a id="api-7e2b231a3680a4f8"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Runtime.GameCodeAssembly> Inno.Runtime.GameCodeModule.assemblies { get; }`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeModule.cs#L72) | Gets the exact ordered code identities. | — | — |
| <a id="api-149ba466d769778c"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Runtime.GameCodeModule.dependencies { get; }`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeModule.cs#L77) | Gets the logical modules required before activation. | — | — |
| <a id="api-bfc10ef924d51398"></a>[`public Inno.Extensibility.Modules.AssemblyDomain Inno.Runtime.GameCodeModule.domain { get; }`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeModule.cs#L67) | Gets the ownership domain of this contribution. | — | — |
| <a id="api-823f8c4847e058dd"></a>[`public string Inno.Runtime.GameCodeModule.name { get; }`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeModule.cs#L62) | Gets the stable logical module identity. | — | — |
| <a id="api-31b0eca3978c6646"></a>[`public sealed class Inno.Runtime.GameCodeModule`](../../src/runtime/engine/Inno.Runtime/Deployment/GameCodeModule.cs#L11) | Freezes the code ownership, ordered assembly identities and dependencies of one logical module. | — | — |

### `Inno.Runtime.GamePresentationSettings`

<!-- api: T:Inno.Runtime.GamePresentationSettings 74f9f4dab2dd85613997215eb04b8fa6a981f9a398df3d17fe048e012c02d551 -->
<!-- api: F:Inno.Runtime.GamePresentationSettings.settingProtocolId d45debbf52dfadb0db494afe1d5bcd5cda0ac740ab501e6bd264c74ed74c6abc -->
<!-- api: P:Inno.Runtime.GamePresentationSettings.settingId 54c858c344998b27921614dbe283a327b01c28de3ea778a27257ea33baa10a7d -->
<!-- api: P:Inno.Runtime.GamePresentationSettings.preserveAspectRatio 3fa6f335ecb6e862be610f3028a71f60d99bcf7e6db832e3c19b35b888419b38 -->
<!-- api: P:Inno.Runtime.GamePresentationSettings.referenceWidth 803a85933d3a2fbf37028b9b7a6be2da6413e5bd6c17b2e7cd6b0f73989905fb -->
<!-- api: P:Inno.Runtime.GamePresentationSettings.referenceHeight 03d8798ab3da587eddc1d0ec947fb753f776e1d59533de446bffd7e4d4fd3514 -->
<!-- api: M:Inno.Runtime.GamePresentationSettings.CalculateViewport(System.Int32,System.Int32) cdd26540f2ddea68bb9215674a16c1d1bae631bc199bc12bdef8096c13b2385e -->
<!-- api: M:Inno.Runtime.GamePresentationSettings.#ctor 0572c89817c2312ef08dc9f1a3dd74ccd1381eb0b39b068ef1eca4e5f36a4cbd -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-298b2eadc745cf06"></a>[`public const string Inno.Runtime.GamePresentationSettings.settingProtocolId = "inno.runtime.game-presentation"`](../../src/runtime/engine/Inno.Runtime/Presentation/GamePresentationSettings.cs#L20) | Gets the immutable project-setting protocol value used by discovery metadata. | — | — |
| <a id="api-1ffe99dd59dceb68"></a>[`public Inno.Runtime.GamePresentationSettings.GamePresentationSettings()`](../../src/runtime/engine/Inno.Runtime/Presentation/GamePresentationSettings.cs#L15) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-9b02f58e226e3919"></a>[`public Inno.Runtime.GamePresentationViewport Inno.Runtime.GamePresentationSettings.CalculateViewport(int availableWidth, int availableHeight)`](../../src/runtime/engine/Inno.Runtime/Presentation/GamePresentationSettings.cs#L60) | Calculates the centered content region for an available presentation surface.<br>返回：A positive region that fills the surface or fits the configured reference aspect ratio.<br>`availableWidth`：Positive available surface width in pixels or logical preview units.<br>`availableHeight`：Positive available surface height in pixels or logical preview units. | — | `System.ArgumentOutOfRangeException`：Thrown when an available or reference dimension is not positive. |
| <a id="api-79427885e25456f3"></a>[`public bool Inno.Runtime.GamePresentationSettings.preserveAspectRatio { get; set; }`](../../src/runtime/engine/Inno.Runtime/Presentation/GamePresentationSettings.cs#L31) | Gets or sets whether the complete reference frame is fitted inside the available surface. | — | — |
| <a id="api-ff8ca3a427fe5f0b"></a>[`public int Inno.Runtime.GamePresentationSettings.referenceHeight { get; set; }`](../../src/runtime/engine/Inno.Runtime/Presentation/GamePresentationSettings.cs#L43) | Gets or sets the positive reference-frame height used to derive the presentation aspect ratio. | — | — |
| <a id="api-439123508a26a260"></a>[`public int Inno.Runtime.GamePresentationSettings.referenceWidth { get; set; }`](../../src/runtime/engine/Inno.Runtime/Presentation/GamePresentationSettings.cs#L37) | Gets or sets the positive reference-frame width used to derive the presentation aspect ratio. | — | — |
| <a id="api-4f7b3945cd705fbb"></a>[`public static Inno.Core.Settings.ProjectSettingId Inno.Runtime.GamePresentationSettings.settingId { get; }`](../../src/runtime/engine/Inno.Runtime/Presentation/GamePresentationSettings.cs#L25) | Gets the stable project-setting identity for game presentation. | — | — |
| <a id="api-9cd830f217603b7d"></a>[`public sealed class Inno.Runtime.GamePresentationSettings : Inno.Core.Serialization.ISerializable`](../../src/runtime/engine/Inno.Runtime/Presentation/GamePresentationSettings.cs#L15) | Defines the project-wide game presentation area shared by Editor previews and deployed Players. | — | — |

### `Inno.Runtime.GamePresentationViewport`

<!-- api: T:Inno.Runtime.GamePresentationViewport 831c200026047f17971c6d10993b74c27dffd1aa7d01f6a1065c75ec8729bd41 -->
<!-- api: M:Inno.Runtime.GamePresentationViewport.#ctor(System.Int32,System.Int32,System.Int32,System.Int32) 005915cfec9020f1b05bbe2ae985b9a75d2b3219a0b1338103cf31804781ed82 -->
<!-- api: P:Inno.Runtime.GamePresentationViewport.x fa48075e06242882253f8a541e7c2919994c1790e678c17e1da9befba400f546 -->
<!-- api: P:Inno.Runtime.GamePresentationViewport.y 4c958263b5b6e30f11df619925e82f1b17bcb5c453c5d7f800359ecbbba74210 -->
<!-- api: P:Inno.Runtime.GamePresentationViewport.width c1e27291aadc1d29c2c194803dba1c39e6ce6c735ec22fb4d4a857658bb01012 -->
<!-- api: P:Inno.Runtime.GamePresentationViewport.height 3384888e12f8bb9670bf81afa4256bf8109e651d81596dc86a915cdf2616bcea -->
<!-- api: M:Inno.Runtime.GamePresentationViewport.ToString 94efdd4f65bc9144182eeb993c046b888c225f428e78b2074c70db693b8e74e4 -->
<!-- api: M:Inno.Runtime.GamePresentationViewport.op_Inequality(Inno.Runtime.GamePresentationViewport,Inno.Runtime.GamePresentationViewport) 378589d7625cf6c200bdbb218ae4d3f4067bf6a1b164098041f270c9bb96f83f -->
<!-- api: M:Inno.Runtime.GamePresentationViewport.op_Equality(Inno.Runtime.GamePresentationViewport,Inno.Runtime.GamePresentationViewport) 06bfddce8487e3c97a8e7d7fe63089cafde1bbb97fac39ddcae344a2c575f95f -->
<!-- api: M:Inno.Runtime.GamePresentationViewport.GetHashCode 90099ce567fae7800a14002a5b895349e93e6fd21b922ccfcecd71148df726ec -->
<!-- api: M:Inno.Runtime.GamePresentationViewport.Equals(System.Object) ced2181adc0610057de465baa42ccb6e8bc247c8088e7d4c9422830703079671 -->
<!-- api: M:Inno.Runtime.GamePresentationViewport.Equals(Inno.Runtime.GamePresentationViewport) b08d0338d0e7b7540102d016e82519516406a82bd80622a701c4c2636159a0dd -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a18d6bccfed3350a"></a>[`public Inno.Runtime.GamePresentationViewport.GamePresentationViewport(int x, int y, int width, int height)`](../../src/runtime/engine/Inno.Runtime/Presentation/GamePresentationSettings.cs#L108) | Creates a validated game presentation viewport.<br>`x`：Non-negative horizontal offset.<br>`y`：Non-negative vertical offset.<br>`width`：Positive content width.<br>`height`：Positive content height. | — | — |
| <a id="api-daf8809c83568db9"></a>[`public bool Inno.Runtime.GamePresentationViewport.Equals(Inno.Runtime.GamePresentationViewport other)`](../../src/runtime/engine/Inno.Runtime/Presentation/GamePresentationSettings.cs#L91) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-6492ccff69f3d702"></a>[`public override bool Inno.Runtime.GamePresentationViewport.Equals(object obj)`](../../src/runtime/engine/Inno.Runtime/Presentation/GamePresentationSettings.cs#L91) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-948fecd1c4b2dc4a"></a>[`public override int Inno.Runtime.GamePresentationViewport.GetHashCode()`](../../src/runtime/engine/Inno.Runtime/Presentation/GamePresentationSettings.cs#L91) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-17114c33123fa55a"></a>[`public static bool Inno.Runtime.GamePresentationViewport.operator ==(Inno.Runtime.GamePresentationViewport left, Inno.Runtime.GamePresentationViewport right)`](../../src/runtime/engine/Inno.Runtime/Presentation/GamePresentationSettings.cs#L91) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-9bfa52abe0c2a69f"></a>[`public static bool Inno.Runtime.GamePresentationViewport.operator !=(Inno.Runtime.GamePresentationViewport left, Inno.Runtime.GamePresentationViewport right)`](../../src/runtime/engine/Inno.Runtime/Presentation/GamePresentationSettings.cs#L91) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-34445be6524770ce"></a>[`public override string Inno.Runtime.GamePresentationViewport.ToString()`](../../src/runtime/engine/Inno.Runtime/Presentation/GamePresentationSettings.cs#L91) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-2d6892fa4e63d2d6"></a>[`public int Inno.Runtime.GamePresentationViewport.height { get; }`](../../src/runtime/engine/Inno.Runtime/Presentation/GamePresentationSettings.cs#L142) | Gets the content height. | — | — |
| <a id="api-db3d6300482fbef3"></a>[`public int Inno.Runtime.GamePresentationViewport.width { get; }`](../../src/runtime/engine/Inno.Runtime/Presentation/GamePresentationSettings.cs#L137) | Gets the content width. | — | — |
| <a id="api-a301b3f14291e6bc"></a>[`public int Inno.Runtime.GamePresentationViewport.x { get; }`](../../src/runtime/engine/Inno.Runtime/Presentation/GamePresentationSettings.cs#L127) | Gets the horizontal content offset. | — | — |
| <a id="api-b76c84c6970a6a0f"></a>[`public int Inno.Runtime.GamePresentationViewport.y { get; }`](../../src/runtime/engine/Inno.Runtime/Presentation/GamePresentationSettings.cs#L132) | Gets the vertical content offset. | — | — |
| <a id="api-c912cfbcb90ac865"></a>[`public readonly record struct Inno.Runtime.GamePresentationViewport : System.IEquatable<Inno.Runtime.GamePresentationViewport>`](../../src/runtime/engine/Inno.Runtime/Presentation/GamePresentationSettings.cs#L91) | Stores one centered game-content region within a presentation surface. | — | — |

### `Inno.Runtime.GameRuntimeAssembly`

<!-- api: T:Inno.Runtime.GameRuntimeAssembly 83787b25009649b84b3ce7eee704f99d5abdaf1ec7b9ee4282ec247166174d95 -->
<!-- api: P:Inno.Runtime.GameRuntimeAssembly.name 4ae294dc26fde17f727b9248f766d2ca3886520a89f8824cbb23e7255b7956d1 -->
<!-- api: P:Inno.Runtime.GameRuntimeAssembly.contentFingerprint 7bcb1775d7e02376eb5755cb9b4a7df11496ab00d1b0b9822c2d69090f5de318 -->
<!-- api: M:Inno.Runtime.GameRuntimeAssembly.Validate 7245b9b515cd19ee774273ef2081462c071640517d537ccdc758c224dafc7015 -->
<!-- api: M:Inno.Runtime.GameRuntimeAssembly.#ctor a8313fc78e39c41ae42a8506d1fd04f75df1d25dedee5fd6c02a23c7960b47ce -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-521ab7184150084d"></a>[`public Inno.Runtime.GameRuntimeAssembly.GameRuntimeAssembly()`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeAssembly.cs#L12) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-562b3bda87f068b9"></a>[`public void Inno.Runtime.GameRuntimeAssembly.Validate()`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeAssembly.cs#L32) | Rejects identities or fingerprints that cannot describe a frozen code input. | — | `System.IO.InvalidDataException`：The assembly name is empty or nonportable, or the fingerprint is not a SHA-256 value. |
| <a id="api-2e0004b1cf867463"></a>[`public string Inno.Runtime.GameRuntimeAssembly.contentFingerprint { get; set; }`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeAssembly.cs#L24) | Gets or sets the lowercase SHA-256 fingerprint of the frozen compiler output. | — | — |
| <a id="api-4333ce0305159744"></a>[`public string Inno.Runtime.GameRuntimeAssembly.name { get; set; }`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeAssembly.cs#L18) | Gets or sets the exact simple assembly name, independent of deployment file layout. | — | — |
| <a id="api-01536293a9a3df7a"></a>[`public sealed class Inno.Runtime.GameRuntimeAssembly : Inno.Core.Serialization.ISerializable`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeAssembly.cs#L12) | Records a logical assembly identity and the content fingerprint of its build input. | — | — |

### `Inno.Runtime.GameRuntimeManifest`

<!-- api: T:Inno.Runtime.GameRuntimeManifest fdd57a41e1dd4505b42676e64193157e4c6cb582ac5fe9c502f4bde440d6a943 -->
<!-- api: P:Inno.Runtime.GameRuntimeManifest.applicationId 3457bb7d21e4d99e8b2ade7ed90304398e3198db7f07cd7bcf518073b7801080 -->
<!-- api: P:Inno.Runtime.GameRuntimeManifest.productName b28bfc9bb101c74f5965880275ef3fc447b36086ccd9f3c34e9b982460097d1d -->
<!-- api: P:Inno.Runtime.GameRuntimeManifest.persistentDataPath 7af39e412437c906b14069ee81e6f362f8d6ebb1eb8bdbac6e72fec7ad883423 -->
<!-- api: P:Inno.Runtime.GameRuntimeManifest.startupScene 5b255737263b66fd9e39005879ad6e9578e3f5e6fd77bba25dbbef52728208b5 -->
<!-- api: P:Inno.Runtime.GameRuntimeManifest.windowWidth 30add5ccb4abf3dc3997a69cb471ab56e15418a37d13ede066ff460261cccc49 -->
<!-- api: P:Inno.Runtime.GameRuntimeManifest.windowHeight 5e9d38249d00566802cdb45a88480a15b5720578c26b14b0be4e14207366ddba -->
<!-- api: P:Inno.Runtime.GameRuntimeManifest.plugins d591a8ffc341da3710e77b38f44dbc60225d5baacfd9619ad866eb25b78b23d5 -->
<!-- api: P:Inno.Runtime.GameRuntimeManifest.modules 2d3cf8dc8105254ddbc74f821cbd02d6dc81aa8d1c05a1bcd3a59b513fad5f8d -->
<!-- api: M:Inno.Runtime.GameRuntimeManifest.Validate 455801b198414d62e2bbd3d1cf235199bf09c0ecdfb79c469691b7c99d8895ba -->
<!-- api: M:Inno.Runtime.GameRuntimeManifest.CreateSettingContributors 0e06de42ecff12e8257cc2e21cd0900e34e4e4da5f29b5c9f1ef448c356baf5d -->
<!-- api: M:Inno.Runtime.GameRuntimeManifest.#ctor ef7919dd112f6b3e0519190e67a0bd7b3dfb67baaa996421f3d90b1c4a82eab4 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-0697a3ef7237c373"></a>[`public Inno.Runtime.GameRuntimeManifest.GameRuntimeManifest()`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeManifest.cs#L13) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-93229ae387bc763e"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Core.Settings.ProjectSettingsContributor> Inno.Runtime.GameRuntimeManifest.CreateSettingContributors()`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeManifest.cs#L132) | Creates current-generation setting contributors from neutral manifest data.<br>返回：Dependency-ordered contributors ready for Inno.Core.Settings.ProjectSettingsStore. | — | — |
| <a id="api-1a8712a398a7543a"></a>[`public void Inno.Runtime.GameRuntimeManifest.Validate()`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeManifest.cs#L69) | Validates startup identity, dimensions, scene, and Plugin ordering. | — | `System.IO.InvalidDataException`：Thrown when manifest data cannot be executed safely. |
| <a id="api-19046f1c972e85da"></a>[`public string Inno.Runtime.GameRuntimeManifest.applicationId { get; set; }`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeManifest.cs#L19) | Gets or sets the stable lowercase application identifier. | — | — |
| <a id="api-6cd722d74c1c47cb"></a>[`public Inno.Runtime.GameRuntimeModule[] Inno.Runtime.GameRuntimeManifest.modules { get; set; }`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeManifest.cs#L61) | Gets or sets the dependency-ordered managed modules in the frozen Player generation. | — | — |
| <a id="api-d62f803b165a6ecc"></a>[`public string Inno.Runtime.GameRuntimeManifest.persistentDataPath { get; set; }`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeManifest.cs#L31) | Gets or sets the writable data folder below local application data; empty uses the application ID. | — | — |
| <a id="api-ba6bb9dd431539e6"></a>[`public Inno.Runtime.GameRuntimePlugin[] Inno.Runtime.GameRuntimeManifest.plugins { get; set; }`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeManifest.cs#L55) | Gets or sets dependency-ordered runtime Plugin setting contributions. | — | — |
| <a id="api-2a7424cfd371bf27"></a>[`public string Inno.Runtime.GameRuntimeManifest.productName { get; set; }`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeManifest.cs#L25) | Gets or sets the player-facing product name. | — | — |
| <a id="api-150cdcdcf212fc6a"></a>[`public string Inno.Runtime.GameRuntimeManifest.startupScene { get; set; }`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeManifest.cs#L37) | Gets or sets the mount-qualified startup scene path. | — | — |
| <a id="api-777a9905bfceac3c"></a>[`public int Inno.Runtime.GameRuntimeManifest.windowHeight { get; set; }`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeManifest.cs#L49) | Gets or sets the initial logical window height. | — | — |
| <a id="api-f5b57b32212d13b9"></a>[`public int Inno.Runtime.GameRuntimeManifest.windowWidth { get; set; }`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeManifest.cs#L43) | Gets or sets the initial logical window width. | — | — |
| <a id="api-9cb45ed79e2262b3"></a>[`public sealed class Inno.Runtime.GameRuntimeManifest : Inno.Core.Serialization.ISerializable`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeManifest.cs#L13) | Stores the immutable startup contract consumed by a deployed game Player. | — | — |

### `Inno.Runtime.GameRuntimeModule`

<!-- api: T:Inno.Runtime.GameRuntimeModule 4b82d0ff2df681db6971fc8620367592d6d0205df5416f55de52ce9ba75e78ae -->
<!-- api: P:Inno.Runtime.GameRuntimeModule.name 452b3b4c4a7f90539b0b3f3b751d2b24ffbc244d9d3bda5b93408ab5dfe138a8 -->
<!-- api: P:Inno.Runtime.GameRuntimeModule.domain c579092bb3365cdd9edf7c7d2432bfc602052bea47d6c03329a8fffbeb009521 -->
<!-- api: P:Inno.Runtime.GameRuntimeModule.assemblies 83b9ea9fe207b1fe0a48a3d9b43ae11da7feb95aad2bd10380e11004fba02cea -->
<!-- api: P:Inno.Runtime.GameRuntimeModule.dependencies eb2487cf92e122619ebf143289df9368744f97c79783f83480ebbbd2ecb7e206 -->
<!-- api: M:Inno.Runtime.GameRuntimeModule.Validate 8aa55bbc8b68a251d1b51d2753486d6093e92281bf9bf6d860afb9626418cca5 -->
<!-- api: M:Inno.Runtime.GameRuntimeModule.#ctor fef57f6423001ce7af8e1cbe56c21c1f58ddf8001b8b3cfef3b829b3d10d0daf -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9f2699b2babfee41"></a>[`public Inno.Runtime.GameRuntimeModule.GameRuntimeModule()`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeModule.cs#L13) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-e0e2c4503e268f26"></a>[`public void Inno.Runtime.GameRuntimeModule.Validate()`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeModule.cs#L45) | Validates module identity, ownership, file names, and dependency declarations. | — | `System.IO.InvalidDataException`：Thrown when the module cannot be activated as part of a frozen Player generation. |
| <a id="api-43f601118dfb0401"></a>[`public Inno.Runtime.GameRuntimeAssembly[] Inno.Runtime.GameRuntimeModule.assemblies { get; set; }`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeModule.cs#L31) | Gets or sets the exact ordered code identities contributed by this module. | — | — |
| <a id="api-d416f3bea7c6288a"></a>[`public string[] Inno.Runtime.GameRuntimeModule.dependencies { get; set; }`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeModule.cs#L37) | Gets or sets stable module names that must be active before this module. | — | — |
| <a id="api-c7f2cd065da5ef9f"></a>[`public Inno.Extensibility.Modules.AssemblyDomain Inno.Runtime.GameRuntimeModule.domain { get; set; }`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeModule.cs#L25) | Gets or sets the ownership domain declared by the deployed module. | — | — |
| <a id="api-1c2ee4615fc86b37"></a>[`public string Inno.Runtime.GameRuntimeModule.name { get; set; }`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeModule.cs#L19) | Gets or sets the stable module name used by the managed module host. | — | — |
| <a id="api-ab9be9549997e631"></a>[`public sealed class Inno.Runtime.GameRuntimeModule : Inno.Core.Serialization.ISerializable`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimeModule.cs#L13) | Describes one dependency-ordered managed module in a frozen Player generation. | — | — |

### `Inno.Runtime.GameRuntimePlugin`

<!-- api: T:Inno.Runtime.GameRuntimePlugin 48569ed4d0a77c8c98e6bcc93d28a799123e23a8bd7f1797ff3fe16a611f798c -->
<!-- api: P:Inno.Runtime.GameRuntimePlugin.id 2890b6b649933194ee5d2b8ab89171943886054a0a120244854defc890c2502c -->
<!-- api: P:Inno.Runtime.GameRuntimePlugin.dependencies 839ea3627572d8682c32fe350c8e73accd393b1b451af36be857e86519a00259 -->
<!-- api: P:Inno.Runtime.GameRuntimePlugin.overrides c18f5bd1acf562f0c3684f48dd7bcbb38f63b18e9b413704bf7925118ac3d506 -->
<!-- api: P:Inno.Runtime.GameRuntimePlugin.settings 25c4a5cbf527401455447418d8c1971fc65844dade295fd5a4dd75fc8a05d7b0 -->
<!-- api: M:Inno.Runtime.GameRuntimePlugin.Validate b6fb3159fa3bff1fed46609433377b603a700f0373260034bbea940175d1472c -->
<!-- api: M:Inno.Runtime.GameRuntimePlugin.#ctor eb003999fcc80a8646affdb09f16aa801e9dcc82eda809b50f3c59c9127a08f2 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8d32aa40cf539185"></a>[`public Inno.Runtime.GameRuntimePlugin.GameRuntimePlugin()`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimePlugin.cs#L13) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-66159b3cd5b34602"></a>[`public void Inno.Runtime.GameRuntimePlugin.Validate()`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimePlugin.cs#L45) | Validates identity and ownership declarations. | — | `System.IO.InvalidDataException`：Thrown when the contribution is malformed. |
| <a id="api-7b82fcd480260ec0"></a>[`public string[] Inno.Runtime.GameRuntimePlugin.dependencies { get; set; }`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimePlugin.cs#L25) | Gets or sets Plugin IDs that must precede this Plugin. | — | — |
| <a id="api-b84949da5651775d"></a>[`public string Inno.Runtime.GameRuntimePlugin.id { get; set; }`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimePlugin.cs#L19) | Gets or sets the stable Plugin identifier. | — | — |
| <a id="api-532cf236315bf6d7"></a>[`public string[] Inno.Runtime.GameRuntimePlugin.overrides { get; set; }`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimePlugin.cs#L31) | Gets or sets dependencies whose setting defaults may be replaced. | — | — |
| <a id="api-ce65c5cd462b5c6b"></a>[`public Inno.Core.Settings.ProjectSettingRecord[] Inno.Runtime.GameRuntimePlugin.settings { get; set; }`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimePlugin.cs#L37) | Gets or sets neutral setting contribution records. | — | — |
| <a id="api-4a374a0ab0e62719"></a>[`public sealed class Inno.Runtime.GameRuntimePlugin : Inno.Core.Serialization.ISerializable`](../../src/runtime/engine/Inno.Runtime/Deployment/GameRuntimePlugin.cs#L13) | Stores one Plugin's runtime-only project setting contribution. | — | — |

### `Inno.Runtime.RuntimeContentCatalog`

<!-- api: T:Inno.Runtime.RuntimeContentCatalog 10cea87f72cff896afff7c45f964cb632c65b4cc3c453cd93988b4491f41c03f -->
<!-- api: P:Inno.Runtime.RuntimeContentCatalog.contentHash 6b846e47caa942747ed327a86a8d120f2367358c2ccc148d86c160cddc854992 -->
<!-- api: P:Inno.Runtime.RuntimeContentCatalog.packFileName 3c0b237bf9a2b4513a10b58d0bf56a15a5ed5f4646e3b40c62569b1c538bcc8d -->
<!-- api: P:Inno.Runtime.RuntimeContentCatalog.snapshotFingerprint b69a6cb4ddc1243b9c98e8c6f709e2a247d6b0573ec1224a091272b4289270ea -->
<!-- api: P:Inno.Runtime.RuntimeContentCatalog.assetCount e2ef88e0a8370a4936863d1c2f9f16b2a55542f481b9ff62eb1309fb64ed9c97 -->
<!-- api: P:Inno.Runtime.RuntimeContentCatalog.artifactBundleCount 7c27202a700328fdb467e878cf85e8d5ec1a9064221ec3f14ac29c5fb45c9579 -->
<!-- api: P:Inno.Runtime.RuntimeContentCatalog.runtimeAssemblyCount 0b00e0099a4dd69376d2ca14820310863ad5718b10ed2a3f27e7b53db317c777 -->
<!-- api: M:Inno.Runtime.RuntimeContentCatalog.Validate 5b283687a64788a04913f5f0272453a2a6846af4d7cbb82d358c971832737f0a -->
<!-- api: M:Inno.Runtime.RuntimeContentCatalog.#ctor 41914f02dfed9e90fbba9c6318f9a14e48811f0b3b5de40fc8bc5a0c541e10fd -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-12241bb08d656ba4"></a>[`public Inno.Runtime.RuntimeContentCatalog.RuntimeContentCatalog()`](../../src/runtime/engine/Inno.Runtime/Deployment/RuntimeContentCatalog.cs#L11) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-cecbc6d760de1531"></a>[`public void Inno.Runtime.RuntimeContentCatalog.Validate()`](../../src/runtime/engine/Inno.Runtime/Deployment/RuntimeContentCatalog.cs#L55) | Validates content identity, file naming, snapshot identity, and counts. | — | `System.IO.InvalidDataException`：Thrown when the catalog cannot identify one complete content-addressed deployment. |
| <a id="api-665dd38df4da9b7c"></a>[`public int Inno.Runtime.RuntimeContentCatalog.artifactBundleCount { get; set; }`](../../src/runtime/engine/Inno.Runtime/Deployment/RuntimeContentCatalog.cs#L41) | Gets or sets the number of content-addressed artifact bundles in the pack. | — | — |
| <a id="api-d6e12172765dec0b"></a>[`public int Inno.Runtime.RuntimeContentCatalog.assetCount { get; set; }`](../../src/runtime/engine/Inno.Runtime/Deployment/RuntimeContentCatalog.cs#L35) | Gets or sets the number of runtime assets in the enclosed asset catalog. | — | — |
| <a id="api-45afedc8acd93939"></a>[`public string Inno.Runtime.RuntimeContentCatalog.contentHash { get; set; }`](../../src/runtime/engine/Inno.Runtime/Deployment/RuntimeContentCatalog.cs#L17) | Gets or sets the SHA-256 identity of the complete content pack bytes. | — | — |
| <a id="api-9f85ac70dd476458"></a>[`public string Inno.Runtime.RuntimeContentCatalog.packFileName { get; set; }`](../../src/runtime/engine/Inno.Runtime/Deployment/RuntimeContentCatalog.cs#L23) | Gets or sets the content pack file name relative to the packaged Content directory. | — | — |
| <a id="api-b1cd2e15dfb3ff3b"></a>[`public int Inno.Runtime.RuntimeContentCatalog.runtimeAssemblyCount { get; set; }`](../../src/runtime/engine/Inno.Runtime/Deployment/RuntimeContentCatalog.cs#L47) | Gets or sets the number of runtime assemblies in the pack. | — | — |
| <a id="api-35c921aa5ed8c474"></a>[`public string Inno.Runtime.RuntimeContentCatalog.snapshotFingerprint { get; set; }`](../../src/runtime/engine/Inno.Runtime/Deployment/RuntimeContentCatalog.cs#L29) | Gets or sets the combined build input snapshot fingerprint. | — | — |
| <a id="api-ffffdadca72c0d81"></a>[`public sealed class Inno.Runtime.RuntimeContentCatalog : Inno.Core.Serialization.ISerializable`](../../src/runtime/engine/Inno.Runtime/Deployment/RuntimeContentCatalog.cs#L11) | Describes the single immutable content pack deployed with a Player build. | — | — |

### `Inno.Runtime.RuntimeJobExecutionMode`

<!-- api: T:Inno.Runtime.RuntimeJobExecutionMode 0837e79f3d1f3b7cca16c09d5cc00a4dbcfeb301cb741c0d9e3ee55755e5ec5d -->
<!-- api: F:Inno.Runtime.RuntimeJobExecutionMode.SingleThread 692d7089aee2f1877ecde8d3636fe2ceafaafc4f5b589d65ec134c75ebeb61a4 -->
<!-- api: F:Inno.Runtime.RuntimeJobExecutionMode.WorkerPool 0535b75eb7370d96ec7f7ffe01b88670e8a95f9c08280e3c71c248f8ed31f0d1 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-67efb15c322d4fd4"></a>[`Inno.Runtime.RuntimeJobExecutionMode.SingleThread = 0`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeJobExecutionMode.cs#L11) | Executes jobs deterministically on the session owner thread. | — | — |
| <a id="api-63eecc1ea4fd3c48"></a>[`Inno.Runtime.RuntimeJobExecutionMode.WorkerPool = 1`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeJobExecutionMode.cs#L16) | Executes ready jobs on a bounded worker pool owned by the session. | — | — |
| <a id="api-e490bdc5692108e7"></a>[`public enum Inno.Runtime.RuntimeJobExecutionMode : int`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeJobExecutionMode.cs#L6) | Selects how one runtime session executes its frame jobs. | — | — |

### `Inno.Runtime.RuntimeManifestEnvelope`

<!-- api: T:Inno.Runtime.RuntimeManifestEnvelope 275ac0d1e091ecbe65d5eedd2e4e3b83edd6758b8bb990e126612ff350e3c276 -->
<!-- api: M:Inno.Runtime.RuntimeManifestEnvelope.Encode(Inno.Runtime.GameRuntimeManifest,Inno.Core.Serialization.SerializationGeneration) c19cd8a1cbba0ef6d2a4032cd663668472c0b7433af0ceb5fb626be776e2203f -->
<!-- api: M:Inno.Runtime.RuntimeManifestEnvelope.ReadApplicationId(System.ReadOnlySpan{System.Byte}) 9ee880560dbaa8a5041e7e036e8f485c56777d0aa643c9a8effe6720f4d35883 -->
<!-- api: M:Inno.Runtime.RuntimeManifestEnvelope.ReadPersistentDataPath(System.ReadOnlySpan{System.Byte}) eb000df58f5bb6e02baa45f4eb5a623e5ce2add4fed0c1b7a8cdd9d4d1bbe735 -->
<!-- api: M:Inno.Runtime.RuntimeManifestEnvelope.Decode(System.ReadOnlySpan{System.Byte},Inno.Core.Serialization.SerializationGeneration) 04fafcd8ca20c85b228eb246f9c31d9c8bb5468895b338c06f37f7caf78cd1be -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-0dfdaa86fae48964"></a>[`public static Inno.Runtime.GameRuntimeManifest Inno.Runtime.RuntimeManifestEnvelope.Decode(System.ReadOnlySpan<byte> data, Inno.Core.Serialization.SerializationGeneration serialization)`](../../src/runtime/engine/Inno.Runtime/Deployment/RuntimeManifestEnvelope.cs#L118) | Deserializes and validates the complete runtime manifest after engine serialization is available.<br>返回：The validated runtime manifest.<br>`data`：The complete runtime manifest envelope.<br>`serialization`：The immutable converter generation selected for Player startup. | — | `System.IO.InvalidDataException`：Thrown when the envelope or manifest is malformed, or when the framed identity disagrees with the payload. |
| <a id="api-3a321cd411492037"></a>[`public static byte[] Inno.Runtime.RuntimeManifestEnvelope.Encode(Inno.Runtime.GameRuntimeManifest manifest, Inno.Core.Serialization.SerializationGeneration serialization)`](../../src/runtime/engine/Inno.Runtime/Deployment/RuntimeManifestEnvelope.cs#L34) | Encodes a validated runtime manifest into the strict deployment envelope.<br>返回：The complete deterministic deployment envelope.<br>`manifest`：The manifest whose application identity and serialized payload must agree.<br>`serialization`：The immutable converter generation captured for the surrounding build transaction. | — | `System.ArgumentNullException`：Thrown when manifest is null.<br>`System.IO.InvalidDataException`：Thrown when the manifest is not valid for Player startup. |
| <a id="api-33d7d18033aa2444"></a>[`public static string Inno.Runtime.RuntimeManifestEnvelope.ReadApplicationId(System.ReadOnlySpan<byte> data)`](../../src/runtime/engine/Inno.Runtime/Deployment/RuntimeManifestEnvelope.cs#L76) | Reads and validates the application identity without requiring serialization services to be initialized.<br>返回：The stable application identity declared by the Player build.<br>`data`：The complete runtime manifest envelope. | — | `System.IO.InvalidDataException`：Thrown when the envelope is truncated, malformed, or contains an invalid application identity. |
| <a id="api-c3835af6b383615f"></a>[`public static string Inno.Runtime.RuntimeManifestEnvelope.ReadPersistentDataPath(System.ReadOnlySpan<byte> data)`](../../src/runtime/engine/Inno.Runtime/Deployment/RuntimeManifestEnvelope.cs#L95) | Reads the validated writable data folder before engine serialization is initialized.<br>返回：A portable folder path relative to the operating system's local application data directory.<br>`data`：The complete runtime manifest envelope. | — | `System.IO.InvalidDataException`：Thrown when the envelope or persistent data path is malformed. |
| <a id="api-89d8314626c3a524"></a>[`public static class Inno.Runtime.RuntimeManifestEnvelope`](../../src/runtime/engine/Inno.Runtime/Deployment/RuntimeManifestEnvelope.cs#L12) | Frames the serialized runtime manifest with the application identity required before engine startup. | — | — |

### `Inno.Runtime.RuntimeSession`

<!-- api: T:Inno.Runtime.RuntimeSession de79fe40996887a0f22057241ec1b26e8116207bf7aaf1a990694250f72d45f6 -->
<!-- api: P:Inno.Runtime.RuntimeSession.options af9b76a8e8fbc49590846a2da90dd486b1d1f8912c8bc50102f11ab35badd27f -->
<!-- api: P:Inno.Runtime.RuntimeSession.sessionId c32120ee90d6a0f6cf824cdece64c060bc0f95bc13242498df18825ec1225b12 -->
<!-- api: P:Inno.Runtime.RuntimeSession.events afbaeda4b88a318f0270b49692268b4e46a67d2a79b71bdc3e3dcc8941d0dd5c -->
<!-- api: P:Inno.Runtime.RuntimeSession.identities 831683661f5f261458f50b098afbe5ae5cc3f158cb9030d4682d32197d1ca3ca -->
<!-- api: P:Inno.Runtime.RuntimeSession.references ca74f5267d2f1c87fb7f604acfe6768083d3efaf911c85b40177a8cd234adc64 -->
<!-- api: P:Inno.Runtime.RuntimeSession.subsystems 31d51fa71894ec3abbcaddc47085edbd84f60de37550774e3d41a17ad0ae63c4 -->
<!-- api: P:Inno.Runtime.RuntimeSession.timeScale a0e173e3af15e03f7a825bf32e320c9f67ebb3a6916e340d6b6ce9e570e8998d -->
<!-- api: P:Inno.Runtime.RuntimeSession.isPaused 533a282373cfb61abe443b730e5aa6335461a85259f64d8861f8bc2efbe70077 -->
<!-- api: P:Inno.Runtime.RuntimeSession.scenes 9bec5ca2f2a2b1898506f76e47543974ff980511237a68758330549232235a32 -->
<!-- api: P:Inno.Runtime.RuntimeSession.assets bdf1f755b8d22d6fccfd16ea424a73bdc890b28d500c212a72a005a073b1207f -->
<!-- api: M:Inno.Runtime.RuntimeSession.EnterExecutionScope 3e2d086a7cf1398aaa883710fd43e338bf79dc4c378e72a4c19853dde9e5cf6a -->
<!-- api: M:Inno.Runtime.RuntimeSession.Tick(System.Single) fde6690443341a7c5a6aa6a558774093ddd9735feb87e97b3c3e4d9fabbbf3c1 -->
<!-- api: M:Inno.Runtime.RuntimeSession.StopCoroutines(System.Object) ab40e74514720828bfca1f1b1bdaee8ddb03ddfaa59ce301213c9800a9abb198 -->
<!-- api: M:Inno.Runtime.RuntimeSession.Dispose f0772fb34283ba17c2ad175020f0928c61ba26c8092b1a50b942a72ce9fbcc4c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-79a4935802ccf59e"></a>[`public void Inno.Runtime.RuntimeSession.Dispose()`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSession.cs#L299) | Releases scene, asset, scheduling, serialization, and logging ownership for this session. | — | `Inno.Core.Execution.RetirementPendingException`：Session work has not drained or a shared generation still owns live dependencies. |
| <a id="api-e5d606b73ceba010"></a>[`public System.IDisposable Inno.Runtime.RuntimeSession.EnterExecutionScope()`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSession.cs#L181) | Binds this session's script façades to the current asynchronous execution context.<br>返回：A strict last-in-first-out scope owned by the caller. | — | `System.ObjectDisposedException`：Thrown when this session has been disposed. |
| <a id="api-7db9f85302f443cd"></a>[`public void Inno.Runtime.RuntimeSession.StopCoroutines(object owner)`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSession.cs#L286) | Stops every coroutine owned by a runtime object before that object is retired by an atomic reload.<br>`owner`：The exact owner identity supplied when its coroutines were started. | — | `System.ArgumentNullException`：Thrown when owner is null.<br>`System.ObjectDisposedException`：Thrown when this session has been disposed. |
| <a id="api-38412f8fde2655df"></a>[`public void Inno.Runtime.RuntimeSession.Tick(float deltaTime)`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSession.cs#L216) | Advances session events, jobs, coroutines, and scene lifecycle by one frame.<br>`deltaTime`：The elapsed frame time in seconds; negative values are treated as zero. | — | `System.ObjectDisposedException`：Thrown when this session has been disposed.<br>`Inno.Core.Execution.RetirementPendingException`：A faulted generation still owns live dependencies and cannot execute another frame. |
| <a id="api-840040aae9596476"></a>[`public Inno.Assets.AssetDatabase Inno.Runtime.RuntimeSession.assets { get; }`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSession.cs#L168) | Gets the source-free runtime asset database configured for this session. | — | `System.InvalidOperationException`：Thrown when this Edit or Play session was composed without a runtime content deployment. |
| <a id="api-f98a36b2ab875d1b"></a>[`public Inno.Core.Events.EventDispatcher Inno.Runtime.RuntimeSession.events { get; }`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSession.cs#L118) | Gets the event dispatcher owned by this session. | — | — |
| <a id="api-8c2769cd03e67acc"></a>[`public Inno.Core.Identity.IdentityAllocator Inno.Runtime.RuntimeSession.identities { get; }`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSession.cs#L123) | Gets the identity domain that owns every live object in this isolated session. | — | — |
| <a id="api-e5f3399229993f58"></a>[`public bool Inno.Runtime.RuntimeSession.isPaused { get; set; }`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSession.cs#L151) | Gets or sets whether scaled simulation is paused while unscaled subsystems continue to receive frames. | — | — |
| <a id="api-58a022ad38a092d9"></a>[`public Inno.Runtime.RuntimeSessionOptions Inno.Runtime.RuntimeSession.options { get; }`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSession.cs#L108) | Gets the validated immutable options used to create this session. | — | — |
| <a id="api-7dae707facad8f64"></a>[`public Inno.References.ReferenceCatalog Inno.Runtime.RuntimeSession.references { get; }`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSession.cs#L128) | Gets the immutable cross-domain reference resolver generation owned by this session. | — | — |
| <a id="api-7b3383ce85b1bf26"></a>[`public Inno.Scene.SceneWorld Inno.Runtime.RuntimeSession.scenes { get; private set; }`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSession.cs#L160) | Gets the isolated scene world owned by this session. | — | — |
| <a id="api-aaf89f39b1478f29"></a>[`public Inno.Core.Logging.LogSessionId Inno.Runtime.RuntimeSession.sessionId { get; }`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSession.cs#L113) | Gets the unique logging identity assigned to this session. | — | — |
| <a id="api-ba9340270ae16b56"></a>[`public Inno.Runtime.RuntimeSubsystemPipeline Inno.Runtime.RuntimeSession.subsystems { get; }`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSession.cs#L133) | Gets the dependency-ordered subsystem generation owned by this session. | — | — |
| <a id="api-2175ebc7a85c836e"></a>[`public float Inno.Runtime.RuntimeSession.timeScale { get; set; }`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSession.cs#L142) | Gets or sets the finite non-negative simulation time multiplier. | — | `System.ArgumentOutOfRangeException`：Thrown when the assigned value is negative, NaN, or infinite. |
| <a id="api-e413326fc8cd828f"></a>[`public sealed class Inno.Runtime.RuntimeSession : System.IDisposable`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSession.cs#L22) | Owns all mutable simulation, identity, asset, scheduling, and logging state for one isolated execution session. | — | — |

### `Inno.Runtime.RuntimeSessionKind`

<!-- api: T:Inno.Runtime.RuntimeSessionKind 823cd51ac54ad8da8065b938a67fa4c8235aa66bfe43077e95c151a111d52e1c -->
<!-- api: F:Inno.Runtime.RuntimeSessionKind.Edit 69531f77588c58dd133e2f37454f877af9121fea0ab2ac7d952de507436c631c -->
<!-- api: F:Inno.Runtime.RuntimeSessionKind.Play cfcb4380cb7246ad15f13bfe1278f044167e78fbda1328bd8385e957e0e211ef -->
<!-- api: F:Inno.Runtime.RuntimeSessionKind.Player 58683a1d779712e7e5995f7d3415f16404212e77aa7af486f2e4880fff27e165 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3656152e6bf46334"></a>[`Inno.Runtime.RuntimeSessionKind.Edit = 0`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionKind.cs#L11) | An authoring session whose scene world remains editable. | — | — |
| <a id="api-e575e4e562ce8a76"></a>[`Inno.Runtime.RuntimeSessionKind.Play = 1`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionKind.cs#L16) | A disposable Editor play-test session created from an immutable start snapshot. | — | — |
| <a id="api-8b49b0273aeae0e5"></a>[`Inno.Runtime.RuntimeSessionKind.Player = 2`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionKind.cs#L21) | A deployed standalone game session backed only by runtime artifacts. | — | — |
| <a id="api-2d0639efb337203c"></a>[`public enum Inno.Runtime.RuntimeSessionKind : int`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionKind.cs#L6) | Identifies the ownership and lifecycle semantics of an isolated runtime session. | — | — |

### `Inno.Runtime.RuntimeSessionOptions`

<!-- api: T:Inno.Runtime.RuntimeSessionOptions 66445aa3d7474f4574bb34543a57b5f8b59a2fad776062d49614dacec299c305 -->
<!-- api: P:Inno.Runtime.RuntimeSessionOptions.kind 77f02ebd37d4dffedce5a47c934e431127833d5248cf54c207ff7c5fb455ae09 -->
<!-- api: P:Inno.Runtime.RuntimeSessionOptions.applicationId c1ede67ff72fec60c41870d43a558e222a10d528ab367e53e75441d66eecf20e -->
<!-- api: P:Inno.Runtime.RuntimeSessionOptions.contentStore c10b1eefba8049fe9029ac0afd41f6995411c0413013f2d373ef8608ec4a952b -->
<!-- api: P:Inno.Runtime.RuntimeSessionOptions.createLogSink ba8e2a4dad9793798ccb9764e2ee5dc979cf2cbd32edb8a7741b246cbfea465d -->
<!-- api: P:Inno.Runtime.RuntimeSessionOptions.assetResidencyBudgetBytes 76ec62e8c52479838de3c686c7e9dd9571d625063f81c655979ba25070040be5 -->
<!-- api: P:Inno.Runtime.RuntimeSessionOptions.assetPreparationBudgetBytes b11ee5f8cca90fc3f64ea1d36375f241aa8fdaf96c5051618766c1294c6cf2ee -->
<!-- api: P:Inno.Runtime.RuntimeSessionOptions.fixedDeltaTime 4f9eb80fe8be639951bccd76962d85a753350a586511be702a842132b23ec6b0 -->
<!-- api: P:Inno.Runtime.RuntimeSessionOptions.maxFrameDeltaTime d0958314a1cb04fdd7ad40557d8e84934f7a1801896a6543f0bdbf773f36c88f -->
<!-- api: P:Inno.Runtime.RuntimeSessionOptions.maxFixedStepsPerFrame a966be5eb983cf49b0696226096b8c0e8842158075619849bb373977cd0cb099 -->
<!-- api: P:Inno.Runtime.RuntimeSessionOptions.jobExecutionMode 02331f15d66ba2d7a42e871242e055695d9aee30329c30cb77b9629d14918a27 -->
<!-- api: P:Inno.Runtime.RuntimeSessionOptions.jobWorkerCount 31b8383887a267e2fd46844eba05d8227880da5ef44f1f0e16564ce346869e0e -->
<!-- api: P:Inno.Runtime.RuntimeSessionOptions.createSubsystems e4395d2394a541b7a6436204e7d7b820921ebd6558027b680f91062e356eda63 -->
<!-- api: P:Inno.Runtime.RuntimeSessionOptions.capabilities ca2020fcdccf52e86ade5591449dd4067869a477ae373174b05363d6dac5333c -->
<!-- api: P:Inno.Runtime.RuntimeSessionOptions.referenceResolvers c92d82be56a045bc4435d7ea865af8ec15363a940eefbcf1f7c3775088fd6a8d -->
<!-- api: M:Inno.Runtime.RuntimeSessionOptions.#ctor 0e4b272ec4e06c22d1869e90a563ae9ad9b7721b3a2ba4c0b76aa12da6e4ec20 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-5e30b58d11883fbd"></a>[`public Inno.Runtime.RuntimeSessionOptions.RuntimeSessionOptions()`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionOptions.cs#L14) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-1fb174095a4a3d5d"></a>[`public string Inno.Runtime.RuntimeSessionOptions.applicationId { get; init; }`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionOptions.cs#L24) | Gets or initializes the stable application identifier used to isolate persistent data. | — | — |
| <a id="api-a89f4e50029a7b05"></a>[`public long Inno.Runtime.RuntimeSessionOptions.assetPreparationBudgetBytes { get; init; }`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionOptions.cs#L53) | Gets or initializes the maximum simultaneous cold asset payload preparation bytes before admission is rejected. | — | — |
| <a id="api-e9cd508cab831a11"></a>[`public long Inno.Runtime.RuntimeSessionOptions.assetResidencyBudgetBytes { get; init; }`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionOptions.cs#L48) | Gets or initializes the runtime asset payload residency budget in bytes. | — | — |
| <a id="api-0334b11652adc73f"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Runtime.Contracts.RuntimeCapabilityId> Inno.Runtime.RuntimeSessionOptions.capabilities { get; init; }`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionOptions.cs#L89) | Gets or initializes capabilities verified by composition for this session's selected services. | — | — |
| <a id="api-3ead7b03c4415f48"></a>[`public Inno.Content.IRuntimeContentStore? Inno.Runtime.RuntimeSessionOptions.contentStore { get; init; }`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionOptions.cs#L33) | Gets or initializes the immutable content store borrowed for the session lifetime. | Player sessions require this store. Edit and Play sessions may omit it when an authoring asset service is composed by the Editor. | — |
| <a id="api-d5b8699f0b3a7208"></a>[`public System.Func<Inno.Core.Logging.LogSessionId, Inno.Core.Logging.ILogSink>? Inno.Runtime.RuntimeSessionOptions.createLogSink { get; init; }`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionOptions.cs#L43) | Gets or initializes the optional factory that transfers one session log sink to this session. | The factory receives the session identity. The returned sink receives only this session's entries and is disposed during retirement when it implements IDisposable. A null factory leaves logging to the host router's existing sinks. | — |
| <a id="api-f15ff6e5b6118a1c"></a>[`public System.Func<Inno.Runtime.RuntimeSession, System.Collections.Generic.IReadOnlyList<Inno.Runtime.Contracts.IRuntimeSubsystemFactory>> Inno.Runtime.RuntimeSessionOptions.createSubsystems { get; init; }`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionOptions.cs#L83) | Gets or initializes the backend-neutral subsystem factories composed into each session. | — | — |
| <a id="api-b2acb2096fc20457"></a>[`public float Inno.Runtime.RuntimeSessionOptions.fixedDeltaTime { get; init; }`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionOptions.cs#L58) | Gets or initializes the fixed simulation interval in seconds. | — | — |
| <a id="api-1f30690285b766b9"></a>[`public Inno.Runtime.RuntimeJobExecutionMode Inno.Runtime.RuntimeSessionOptions.jobExecutionMode { get; init; }`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionOptions.cs#L73) | Gets or initializes the job execution strategy owned by this session. | — | — |
| <a id="api-bbedfdae9874115d"></a>[`public int Inno.Runtime.RuntimeSessionOptions.jobWorkerCount { get; init; }`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionOptions.cs#L78) | Gets or initializes the worker count used by the work-stealing scheduler; zero selects the default. | — | — |
| <a id="api-a13f8aebe50d37ad"></a>[`public Inno.Runtime.RuntimeSessionKind Inno.Runtime.RuntimeSessionOptions.kind { get; init; }`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionOptions.cs#L19) | Gets or initializes the session role. | — | — |
| <a id="api-af4168babc4a8bd7"></a>[`public int Inno.Runtime.RuntimeSessionOptions.maxFixedStepsPerFrame { get; init; }`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionOptions.cs#L68) | Gets or initializes the maximum number of fixed updates performed by one frame tick. | — | — |
| <a id="api-19f6c2a994801ac3"></a>[`public float Inno.Runtime.RuntimeSessionOptions.maxFrameDeltaTime { get; init; }`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionOptions.cs#L63) | Gets or initializes the maximum accepted variable frame interval in seconds. | — | — |
| <a id="api-32533e7f8474d191"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.References.IReferenceResolver> Inno.Runtime.RuntimeSessionOptions.referenceResolvers { get; init; }`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionOptions.cs#L97) | Gets or initializes owner-provided resolvers included in this session's immutable reference catalog. | A deployed AssetDatabase automatically contributes its asset resolver and must not be repeated here. | — |
| <a id="api-4f43c7f1378d7e55"></a>[`public sealed class Inno.Runtime.RuntimeSessionOptions`](../../src/runtime/engine/Inno.Runtime/Hosting/RuntimeSessionOptions.cs#L14) | Defines storage, scheduling, and timing policy for one runtime session. | — | — |

### `Inno.Runtime.RuntimeSubsystemPipeline`

<!-- api: T:Inno.Runtime.RuntimeSubsystemPipeline 2de6961c20f64acf0bdd0f4da39707955f03c2e212be6cae42aaf2ec745c7f61 -->
<!-- api: P:Inno.Runtime.RuntimeSubsystemPipeline.descriptors caf51530649fcd0fc0a3d849c73ce451bb542bf9db0768a675438e62dcdf9908 -->
<!-- api: P:Inno.Runtime.RuntimeSubsystemPipeline.startupDiagnostics f70184560076f18b951f160862a8557516188f4bf713fd6aa71145bd43244ee6 -->
<!-- api: M:Inno.Runtime.RuntimeSubsystemPipeline.GetRequiredSubsystem``1 3c42a7966759f1b8070b5c0959cc1168361466abd7de3fd20fe924620394f6d5 -->
<!-- api: M:Inno.Runtime.RuntimeSubsystemPipeline.BeginFrame(Inno.Runtime.Contracts.RuntimeFrame) 3774e7df004d17f2d41a039d511a3b0a265ee257c1ce267e275eaae1454b5478 -->
<!-- api: M:Inno.Runtime.RuntimeSubsystemPipeline.FixedUpdate(Inno.Runtime.Contracts.RuntimeFixedFrame) 2a1b5f51025eacb00bf7ff30d87ab0917f55692b14bcb7141cf9e4d26e04fe85 -->
<!-- api: M:Inno.Runtime.RuntimeSubsystemPipeline.Update(Inno.Runtime.Contracts.RuntimeFrame) 2b86a4d6369d2c41735826d0461c8d2081e24927a668f6a5da6ffa32a729c28d -->
<!-- api: M:Inno.Runtime.RuntimeSubsystemPipeline.LateUpdate(Inno.Runtime.Contracts.RuntimeFrame) fbd8c78af00b3a4b9e828e7bad5190aebdf3dc4cbfcc4452d3a609caa8223590 -->
<!-- api: M:Inno.Runtime.RuntimeSubsystemPipeline.RenderFrame(Inno.Runtime.Contracts.RuntimeFrame,System.Action) 0e3e4b02a383618d39e2b9416d0c048e7dbaccee93d1f39ffab24dd015eb69c1 -->
<!-- api: M:Inno.Runtime.RuntimeSubsystemPipeline.EndFrame(Inno.Runtime.Contracts.RuntimeFrame) 1a0ded5fa71ab33c606ced62d7f32f1a8c8d1a7744661bb5ea7f917911a5ff6f -->
<!-- api: M:Inno.Runtime.RuntimeSubsystemPipeline.Dispose 063cf770d0600fe4b7591488285a6a7a736f4a9a91fbd788b928d21c17bbd822 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7982c75dc5e7cbfe"></a>[`public void Inno.Runtime.RuntimeSubsystemPipeline.BeginFrame(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/engine/Inno.Runtime/Subsystems/RuntimeSubsystemPipeline.cs#L164) | Opens foundation scopes and immutable snapshots for one owner frame.<br>`frame`：The frame timing snapshot. | — | — |
| <a id="api-e971f23160e47d85"></a>[`public void Inno.Runtime.RuntimeSubsystemPipeline.Dispose()`](../../src/runtime/engine/Inno.Runtime/Subsystems/RuntimeSubsystemPipeline.cs#L309) | Detaches and disposes every subsystem in reverse dependency order. | — | `System.AggregateException`：Thrown after every release stage is attempted when one or more subsystems fail to detach or dispose. |
| <a id="api-480cbc02997fdee3"></a>[`public void Inno.Runtime.RuntimeSubsystemPipeline.EndFrame(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/engine/Inno.Runtime/Subsystems/RuntimeSubsystemPipeline.cs#L282) | Closes every successfully opened frame scope in reverse dependency order.<br>`frame`：The frame being closed. | — | — |
| <a id="api-69c538a0095776f6"></a>[`public void Inno.Runtime.RuntimeSubsystemPipeline.FixedUpdate(Inno.Runtime.Contracts.RuntimeFixedFrame frame)`](../../src/runtime/engine/Inno.Runtime/Subsystems/RuntimeSubsystemPipeline.cs#L184) | Advances every subsystem by one deterministic fixed step.<br>`frame`：The fixed-step timing snapshot. | — | — |
| <a id="api-d1fd208575bd91ec"></a>[`public TFeature Inno.Runtime.RuntimeSubsystemPipeline.GetRequiredSubsystem<TFeature>() where TFeature : class, Inno.Runtime.Contracts.IRuntimeSubsystem`](../../src/runtime/engine/Inno.Runtime/Subsystems/RuntimeSubsystemPipeline.cs#L140) | Resolves the unique active subsystem that implements the requested contract.<br>返回：The unique compatible subsystem owned by this pipeline. | — | `System.InvalidOperationException`：Thrown when no compatible subsystem exists or more than one compatible subsystem is active.<br>`System.ObjectDisposedException`：Thrown after this pipeline has been disposed. |
| <a id="api-bdb0528d16fc22c6"></a>[`public void Inno.Runtime.RuntimeSubsystemPipeline.LateUpdate(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/engine/Inno.Runtime/Subsystems/RuntimeSubsystemPipeline.cs#L214) | Advances state that depends on completed simulation.<br>`frame`：The frame timing snapshot. | — | — |
| <a id="api-2a74b319e144cb11"></a>[`public void Inno.Runtime.RuntimeSubsystemPipeline.RenderFrame(Inno.Runtime.Contracts.RuntimeFrame frame, System.Action? submit = null)`](../../src/runtime/engine/Inno.Runtime/Subsystems/RuntimeSubsystemPipeline.cs#L232) | Opens output resources, accepts product requests and submits each owner's output once.<br>`frame`：The frame timing snapshot.<br>`submit`：Optional control-thread presentation code that submits requests while output is open. | — | — |
| <a id="api-84ea9998526cfad5"></a>[`public void Inno.Runtime.RuntimeSubsystemPipeline.Update(Inno.Runtime.Contracts.RuntimeFrame frame)`](../../src/runtime/engine/Inno.Runtime/Subsystems/RuntimeSubsystemPipeline.cs#L199) | Advances variable-clock domain state.<br>`frame`：The frame timing snapshot. | — | — |
| <a id="api-88eaa4a177778640"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Runtime.Contracts.RuntimeSubsystemDescriptor> Inno.Runtime.RuntimeSubsystemPipeline.descriptors { get; }`](../../src/runtime/engine/Inno.Runtime/Subsystems/RuntimeSubsystemPipeline.cs#L118) | Gets the active immutable subsystem descriptors in execution order. | — | — |
| <a id="api-54f805cb06783ae6"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Core.Diagnostics.Diagnostic> Inno.Runtime.RuntimeSubsystemPipeline.startupDiagnostics { get; }`](../../src/runtime/engine/Inno.Runtime/Subsystems/RuntimeSubsystemPipeline.cs#L123) | Gets immutable diagnostics for unavailable optional subsystems; no exception or extension instance is retained. | — | — |
| <a id="api-9ad25beaaa9b8c59"></a>[`public sealed class Inno.Runtime.RuntimeSubsystemPipeline : System.IDisposable`](../../src/runtime/engine/Inno.Runtime/Subsystems/RuntimeSubsystemPipeline.cs#L14) | Owns the dependency-ordered runtime subsystem generation for one Host or Session scope. | — | — |

### `Inno.Runtime.StaticAssemblyCatalogSource`

<!-- api: T:Inno.Runtime.StaticAssemblyCatalogSource 038a81bf8c97acbc1ae05b1160d9cbaa4577a8ddcb01d9047308e6ec3d5f419a -->
<!-- api: M:Inno.Runtime.StaticAssemblyCatalogSource.#ctor(System.Collections.Generic.IReadOnlyList{System.Reflection.Assembly},System.Collections.Generic.IReadOnlyList{System.Reflection.Assembly}) 030eb471417a1ea9c58b0a74b8bf3aac1a0d5c79d2016a6e64c6bc8acdbc0022 -->
<!-- api: E:Inno.Runtime.StaticAssemblyCatalogSource.changed 256c3d6480ace2e4c3779dd7d34b092ab50b83c7ec644247841b765b1b1acdc7 -->
<!-- api: M:Inno.Runtime.StaticAssemblyCatalogSource.GetAssemblies 6b99fb32f77aa2dcbd3958e7e63c3326b0a741ddad374db17172e0ce65f41d8e -->
<!-- api: M:Inno.Runtime.StaticAssemblyCatalogSource.GetSharedAssemblies 00c1bee19d8ea4ebc9493e581f29f82cecb47f645d57d9c115a211fa1301e98d -->
<!-- api: M:Inno.Runtime.StaticAssemblyCatalogSource.IsFrameworkReference(System.String) a429226029343865403046c99e09d757e7cae7ae1fd25fb677a6a5a926abc51a -->
<!-- api: M:Inno.Runtime.StaticAssemblyCatalogSource.IsFrameworkAssembly(System.Reflection.Assembly) d4c4229c8f3c1c7f49cb2f729748cacbe33ed3b7071cf07189208c0b70c7acb5 -->
<!-- api: M:Inno.Runtime.StaticAssemblyCatalogSource.Dispose 0d02254242e247f882057938993017aac14d0a9c3df365ad91ffeb28e7ad1b41 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-ad6c014139cfd5e2"></a>[`public System.Action? Inno.Runtime.StaticAssemblyCatalogSource.changed`](../../src/runtime/engine/Inno.Runtime/Registration/StaticAssemblyCatalogSource.cs#L46) | Notifies the owner that the host assembly snapshot should be refreshed at a safe point. | — | — |
| <a id="api-ea4536f8796ba050"></a>[`public Inno.Runtime.StaticAssemblyCatalogSource.StaticAssemblyCatalogSource(System.Collections.Generic.IReadOnlyList<System.Reflection.Assembly> assemblies, System.Collections.Generic.IReadOnlyList<System.Reflection.Assembly> frameworkAssemblies)`](../../src/runtime/engine/Inno.Runtime/Registration/StaticAssemblyCatalogSource.cs#L29) | Copies the host and framework identities provided by the generated platform composition.<br>`assemblies`：The complete statically linked Inno host closure.<br>`frameworkAssemblies`：Framework contracts shared by every contributed module. | — | — |
| <a id="api-cc77f800b49f2600"></a>[`public void Inno.Runtime.StaticAssemblyCatalogSource.Dispose()`](../../src/runtime/engine/Inno.Runtime/Registration/StaticAssemblyCatalogSource.cs#L81) | Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. | — | — |
| <a id="api-8ab84bee6c83629c"></a>[`public System.Collections.Generic.IReadOnlyList<System.Reflection.Assembly> Inno.Runtime.StaticAssemblyCatalogSource.GetAssemblies()`](../../src/runtime/engine/Inno.Runtime/Registration/StaticAssemblyCatalogSource.cs#L53) | Gets the complete host assemblies that participate in extension discovery.<br>返回：The current host snapshot; the caller must not retain it beyond its catalog generation. | — | — |
| <a id="api-dfc04779bbb47306"></a>[`public System.Collections.Generic.IReadOnlyList<System.Reflection.Assembly> Inno.Runtime.StaticAssemblyCatalogSource.GetSharedAssemblies()`](../../src/runtime/engine/Inno.Runtime/Registration/StaticAssemblyCatalogSource.cs#L60) | Gets host contracts and framework assemblies available to candidate modules.<br>返回：A snapshot of shareable assemblies owned by the host implementation. | — | — |
| <a id="api-3b59706299670d8e"></a>[`public bool Inno.Runtime.StaticAssemblyCatalogSource.IsFrameworkAssembly(System.Reflection.Assembly assembly)`](../../src/runtime/engine/Inno.Runtime/Registration/StaticAssemblyCatalogSource.cs#L74) | Determines whether an assembly is a framework contract rather than a contributed module.<br>返回：Whether the assembly is owned by the selected managed framework.<br>`assembly`：The candidate shared assembly. | — | — |
| <a id="api-916a02933c65ab56"></a>[`public bool Inno.Runtime.StaticAssemblyCatalogSource.IsFrameworkReference(string assemblyName)`](../../src/runtime/engine/Inno.Runtime/Registration/StaticAssemblyCatalogSource.cs#L67) | Determines whether a referenced simple name belongs to the selected managed framework.<br>返回：Whether the framework owns this dependency.<br>`assemblyName`：The referenced managed assembly simple name. | — | — |
| <a id="api-e40327a24fd0b626"></a>[`public sealed class Inno.Runtime.StaticAssemblyCatalogSource : Inno.Extensibility.Modules.IAssemblyCatalogSource`](../../src/runtime/engine/Inno.Runtime/Registration/StaticAssemblyCatalogSource.cs#L12) | Supplies an explicit linked host closure without runtime assembly probing or dynamic loading. | — | — |

### `Inno.Runtime.StaticModuleSource`

<!-- api: T:Inno.Runtime.StaticModuleSource c82881142826800025676ea2a2dd31068bfbc7065ce96fda167a35ec514ce023 -->
<!-- api: M:Inno.Runtime.StaticModuleSource.#ctor(System.String,Inno.Extensibility.Modules.AssemblyDomain,Inno.Extensibility.Modules.AssemblyScope,System.Collections.Generic.IReadOnlyList{System.Reflection.Assembly},System.Collections.Generic.IReadOnlyList{System.String}) 22d91b7376ed85dff3e279105c5aad629e478645bd3e0f8e6723108cac48e60b -->
<!-- api: P:Inno.Runtime.StaticModuleSource.moduleName 9ea95f2f8f96735c56bee34300e1b4cc7777815603d9f27c291261b36620c5fc -->
<!-- api: P:Inno.Runtime.StaticModuleSource.domain 9902d3577770257e8bc6d45ca36d4834d37dadcce3c056800c3c3e43132ec912 -->
<!-- api: P:Inno.Runtime.StaticModuleSource.scope 0b14ab5b857fc71698b23fafa0ab14f97fe030c5cfe6cf1f89b5350d2f60273f -->
<!-- api: P:Inno.Runtime.StaticModuleSource.collectible b678f42573ca5f95339b983e3413d95d4ec18d4ee79c799d9ac0f21eea3f3072 -->
<!-- api: P:Inno.Runtime.StaticModuleSource.upstreamModuleNames ef39976cb1c765679b6d5c973761a1daae366bc473f44e2abb4d315db0a95bda -->
<!-- api: P:Inno.Runtime.StaticModuleSource.assemblyScopes 959ec3e62eee5e178226a0b8d6a65d49a144989948f3717056e9d11e6351cec2 -->
<!-- api: M:Inno.Runtime.StaticModuleSource.GetAssemblyNames e6ee281219d205dffe1ef552bad61bae9647bffb481bacee3dede4b22a273588 -->
<!-- api: M:Inno.Runtime.StaticModuleSource.Prepare(Inno.Extensibility.Modules.ModuleSourceContext) 724a858983dda57b578dd88822d8b180809dc8202e40a7b30c9c9cd5bf2bc4a9 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-0110e25775e3a282"></a>[`public Inno.Runtime.StaticModuleSource.StaticModuleSource(string moduleName, Inno.Extensibility.Modules.AssemblyDomain domain, Inno.Extensibility.Modules.AssemblyScope scope, System.Collections.Generic.IReadOnlyList<System.Reflection.Assembly> assemblies, System.Collections.Generic.IReadOnlyList<string> dependencies)`](../../src/runtime/engine/Inno.Runtime/Registration/StaticModuleSource.cs#L35) | Freezes a linked module's explicit assemblies and logical dependencies.<br>`moduleName`：The stable logical module name.<br>`domain`：The code ownership domain.<br>`scope`：The dependency scope of every supplied assembly.<br>`assemblies`：The linked assemblies; the runtime owns their process lifetime.<br>`dependencies`：The explicitly declared logical upstream modules. | — | — |
| <a id="api-c8f2249f460c17f0"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Runtime.StaticModuleSource.GetAssemblyNames()`](../../src/runtime/engine/Inno.Runtime/Registration/StaticModuleSource.cs#L66) | Reads the complete owned assembly identities before any generation is acquired.<br>返回：Distinct, nonempty simple names; invalid or unavailable source data causes an exception. | — | — |
| <a id="api-4101d7eaa8f2de4c"></a>[`public Inno.Extensibility.Modules.ModuleCatalogContribution Inno.Runtime.StaticModuleSource.Prepare(Inno.Extensibility.Modules.ModuleSourceContext context)`](../../src/runtime/engine/Inno.Runtime/Registration/StaticModuleSource.cs#L69) | Acquires a validated candidate and transfers its lifetime to the caller on success.<br>返回：A complete unpublished contribution; failure retires every acquired resource through the context.<br>`context`：The immutable candidate dependency closure and retirement owner. | — | — |
| <a id="api-79e2e3c28053822f"></a>[`public System.Collections.Generic.IReadOnlyDictionary<string, Inno.Extensibility.Modules.AssemblyScope> Inno.Runtime.StaticModuleSource.assemblyScopes { get; }`](../../src/runtime/engine/Inno.Runtime/Registration/StaticModuleSource.cs#L64) | Gets explicit scope overrides keyed by contributed assembly simple name. | — | — |
| <a id="api-c13e9df78fe6213c"></a>[`public bool Inno.Runtime.StaticModuleSource.collectible { get; }`](../../src/runtime/engine/Inno.Runtime/Registration/StaticModuleSource.cs#L60) | Gets whether retirement can release this source's code generation. | — | — |
| <a id="api-7772a066831ae3c1"></a>[`public Inno.Extensibility.Modules.AssemblyDomain Inno.Runtime.StaticModuleSource.domain { get; }`](../../src/runtime/engine/Inno.Runtime/Registration/StaticModuleSource.cs#L56) | Gets the ownership domain of the contribution. | — | — |
| <a id="api-07265119a97c365f"></a>[`public string Inno.Runtime.StaticModuleSource.moduleName { get; }`](../../src/runtime/engine/Inno.Runtime/Registration/StaticModuleSource.cs#L54) | Gets the stable logical name used by dependency ordering and replacement transactions. | — | — |
| <a id="api-89c2292eb2a94516"></a>[`public Inno.Extensibility.Modules.AssemblyScope Inno.Runtime.StaticModuleSource.scope { get; }`](../../src/runtime/engine/Inno.Runtime/Registration/StaticModuleSource.cs#L58) | Gets the default dependency scope of contributed assemblies. | — | — |
| <a id="api-74a65504c0ba8496"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Runtime.StaticModuleSource.upstreamModuleNames { get; }`](../../src/runtime/engine/Inno.Runtime/Registration/StaticModuleSource.cs#L62) | Gets the explicit logical upstream module names. | — | — |
| <a id="api-431dd5b2af473d0e"></a>[`public sealed class Inno.Runtime.StaticModuleSource : Inno.Extensibility.Modules.IModuleSource`](../../src/runtime/engine/Inno.Runtime/Registration/StaticModuleSource.cs#L13) | Contributes statically linked module code to the same catalog transactions used by authoring hosts. | — | — |

### `Inno.Runtime.StaticTypeCatalogSource`

<!-- api: T:Inno.Runtime.StaticTypeCatalogSource 10f72f1edd9567edf740a4032c328d5cba3c066f102b545f12b1eab7c569c5bd -->
<!-- api: M:Inno.Runtime.StaticTypeCatalogSource.#ctor(System.Collections.Generic.IReadOnlyList{System.Action{Inno.Extensibility.Catalogs.ITypeCatalogRegistrar}}) 8f29d4802968e14fbb33f7c4e61ba5099df36bbb4deae3a1135eb35cbfdfafda -->
<!-- api: M:Inno.Runtime.StaticTypeCatalogSource.GetTypes(System.Reflection.Assembly) 26eb3a42094c1bf78478ded0c0f1be407402a249e82bf5805f9f0e05934a1b16 -->
<!-- api: M:Inno.Runtime.StaticTypeCatalogSource.GetMetadata(System.Type) cde402621faf0cc9c16a15572667d72c6505a9bf5691f10e44c663a68718a67f -->
<!-- api: M:Inno.Runtime.StaticTypeCatalogSource.ConstructGenericType(System.Type,System.Collections.Generic.IReadOnlyList{System.Type}) e51125bd89f62d8cef9e98407a7c078bd47a318a2fd097bb71666799b52af6ba -->
<!-- api: M:Inno.Runtime.StaticTypeCatalogSource.CanCreateInstance(System.Type) 5edd711f12045ba04e4905388cd32c04b8a4d8e42be8471da475cff4b73f454c -->
<!-- api: M:Inno.Runtime.StaticTypeCatalogSource.CreateInstance(System.Type) ea90b75d56b0f781db0ff5105104071c498e9a823b7b34234e1d6847a84183ce -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-5c1285e1823143ff"></a>[`public Inno.Runtime.StaticTypeCatalogSource.StaticTypeCatalogSource(System.Collections.Generic.IReadOnlyList<System.Action<Inno.Extensibility.Catalogs.ITypeCatalogRegistrar>> catalogs)`](../../src/runtime/engine/Inno.Runtime/Registration/StaticTypeCatalogSource.cs#L31) | Executes generated contributions once and freezes the resulting metadata and factory closure.<br>`catalogs`：Assembly-local registration methods. Callbacks and the registrar are released after construction. | — | `System.ArgumentException`：A declaration is registered twice, a generic construction lacks its definition, or a contributor is null. |
| <a id="api-d731a4aa84c0bcaf"></a>[`public bool Inno.Runtime.StaticTypeCatalogSource.CanCreateInstance(System.Type type)`](../../src/runtime/engine/Inno.Runtime/Registration/StaticTypeCatalogSource.cs#L110) | Determines whether this deployment supplies a parameterless factory for a declared type.<br>返回：Whether instance construction is supported by the selected deployment.<br>`type`：The declared runtime type. | — | — |
| <a id="api-64ecf2ed92547df2"></a>[`public System.Type? Inno.Runtime.StaticTypeCatalogSource.ConstructGenericType(System.Type definition, System.Collections.Generic.IReadOnlyList<System.Type> arguments)`](../../src/runtime/engine/Inno.Runtime/Registration/StaticTypeCatalogSource.cs#L87) | Resolves a generic construction through the deployment's linked or dynamic type mechanism.<br>返回：The closed type, or null when its generic constraints reject these arguments.<br>`definition`：A generic declaration owned by the candidate generation.<br>`arguments`：The complete ordered set of closed type arguments. | — | `System.NotSupportedException`：A static deployment did not link the requested construction.<br>`System.ArgumentException`：The declaration is not generic, or the argument count or closedness is invalid. |
| <a id="api-d356130202d5ab50"></a>[`public object Inno.Runtime.StaticTypeCatalogSource.CreateInstance(System.Type type)`](../../src/runtime/engine/Inno.Runtime/Registration/StaticTypeCatalogSource.cs#L114) | Constructs a declared type through the selected deployment's factory mechanism.<br>返回：A new instance owned by the caller; missing factories or failed constructors throw.<br>`type`：The declared runtime type. | — | — |
| <a id="api-84b799bd972ae235"></a>[`public Inno.Extensibility.Catalogs.TypeCatalogMetadata Inno.Runtime.StaticTypeCatalogSource.GetMetadata(System.Type type)`](../../src/runtime/engine/Inno.Runtime/Registration/StaticTypeCatalogSource.cs#L82) | Resolves immutable discovery facts for a declaration in this source.<br>返回：Complete metadata owned by the caller's candidate generation; unavailable declarations throw.<br>`type`：The declaration whose relationships and attributes are required. | — | — |
| <a id="api-9a915f8bb2969980"></a>[`public System.Collections.Generic.IReadOnlyList<System.Type> Inno.Runtime.StaticTypeCatalogSource.GetTypes(System.Reflection.Assembly assembly)`](../../src/runtime/engine/Inno.Runtime/Registration/StaticTypeCatalogSource.cs#L77) | Resolves the complete type set without publishing a partial generation.<br>返回：The complete type metadata for this assembly; unsupported or invalid metadata throws.<br>`assembly`：An assembly owned by the candidate module catalog. | — | — |
| <a id="api-2e6fb5db9ad312de"></a>[`public sealed class Inno.Runtime.StaticTypeCatalogSource : Inno.Extensibility.Types.ITypeCatalogSource`](../../src/runtime/engine/Inno.Runtime/Registration/StaticTypeCatalogSource.cs#L14) | Resolves type metadata and factories from linked registrations without runtime type discovery. | — | — |

### `Inno.Runtime.Time`

<!-- api: T:Inno.Runtime.Time b5c143cf5625fe613f4a46edaf6d61be61d953488b4c2ed6c2beabc6ff0eceeb -->
<!-- api: P:Inno.Runtime.Time.time 54020d95d75b49f9f654618b801a73c2bf195036c01450fb2d9e71ab75c23a90 -->
<!-- api: P:Inno.Runtime.Time.deltaTime d7074f01975120bc7b1a80fd319b14e24af583c84e755296b4138ab83af4f111 -->
<!-- api: P:Inno.Runtime.Time.unscaledTime 8e2db702d90c9edcc4acb8438ef85d5717beedfaa0278cab8807d4b61f7245cd -->
<!-- api: P:Inno.Runtime.Time.unscaledDeltaTime 41fde65f24c6715eeb7e3fe555ae89f542df11590bb27911a182c9848aa96433 -->
<!-- api: P:Inno.Runtime.Time.fixedTime ce336fa28e4b8e24235ea433cd4778a18b82490f5978ff0ce472e6e11476b7c2 -->
<!-- api: P:Inno.Runtime.Time.fixedDeltaTime a46970652e5b9b940ec1118c654a3a7d37ed9b2106f6778666c876ae13ab7a80 -->
<!-- api: P:Inno.Runtime.Time.timeScale 7fd79493d5050a248dd49ddb4c65df9901a4dc6e1feab39d4a8943ddb0ae3710 -->
<!-- api: P:Inno.Runtime.Time.isPaused fb6a26ba576825ba599cca06597955728d6f3e13d9522700fd501e4ce80f3281 -->
<!-- api: P:Inno.Runtime.Time.frameCount 27d5302947b0a324941ca84a0fe1cca1b685da5e618d7fadbdde29ca4b2456d3 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-6a1da77fbef91a51"></a>[`public static float Inno.Runtime.Time.deltaTime { get; }`](../../src/runtime/engine/Inno.Runtime/Execution/Time.cs#L31) | Gets the current variable frame interval in seconds. | — | `System.InvalidOperationException`：Thrown when no runtime session is bound to the current execution context. |
| <a id="api-02df7f326429cc93"></a>[`public static float Inno.Runtime.Time.fixedDeltaTime { get; }`](../../src/runtime/engine/Inno.Runtime/Execution/Time.cs#L63) | Gets the interval of the active fixed simulation step in seconds. | — | `System.InvalidOperationException`：Thrown when no runtime session is bound to the current execution context. |
| <a id="api-b90af850734bbcfd"></a>[`public static float Inno.Runtime.Time.fixedTime { get; }`](../../src/runtime/engine/Inno.Runtime/Execution/Time.cs#L55) | Gets the accumulated fixed simulation time in seconds. | — | `System.InvalidOperationException`：Thrown when no runtime session is bound to the current execution context. |
| <a id="api-081956847d82890b"></a>[`public static long Inno.Runtime.Time.frameCount { get; }`](../../src/runtime/engine/Inno.Runtime/Execution/Time.cs#L95) | Gets the number of variable frames begun by this session. | — | `System.InvalidOperationException`：Thrown when no runtime session is bound to the current execution context. |
| <a id="api-8ceda0eec656c837"></a>[`public static bool Inno.Runtime.Time.isPaused { get; set; }`](../../src/runtime/engine/Inno.Runtime/Execution/Time.cs#L83) | Gets whether scaled simulation is paused. | — | `System.InvalidOperationException`：Thrown when no runtime session is bound to the current execution context. |
| <a id="api-24abd07269b42912"></a>[`public static float Inno.Runtime.Time.time { get; }`](../../src/runtime/engine/Inno.Runtime/Execution/Time.cs#L23) | Gets the total elapsed session time in seconds. | — | `System.InvalidOperationException`：Thrown when no runtime session is bound to the current execution context. |
| <a id="api-1c1d741254c3dc67"></a>[`public static float Inno.Runtime.Time.timeScale { get; set; }`](../../src/runtime/engine/Inno.Runtime/Execution/Time.cs#L71) | Gets the current simulation time multiplier. | — | `System.InvalidOperationException`：Thrown when no runtime session is bound to the current execution context. |
| <a id="api-00c2364ff406ddfe"></a>[`public static float Inno.Runtime.Time.unscaledDeltaTime { get; }`](../../src/runtime/engine/Inno.Runtime/Execution/Time.cs#L47) | Gets the current frame interval unaffected by pause or time scaling. | — | `System.InvalidOperationException`：Thrown when no runtime session is bound to the current execution context. |
| <a id="api-1a9e1607856a5f95"></a>[`public static float Inno.Runtime.Time.unscaledTime { get; }`](../../src/runtime/engine/Inno.Runtime/Execution/Time.cs#L39) | Gets total elapsed session time unaffected by pause or time scaling. | — | `System.InvalidOperationException`：Thrown when no runtime session is bound to the current execution context. |
| <a id="api-b97859746caf3165"></a>[`public static class Inno.Runtime.Time`](../../src/runtime/engine/Inno.Runtime/Execution/Time.cs#L15) | Provides Unity-style timing values for the runtime session bound to the current execution context. | This façade owns no process-global timing state. Engine systems update an instance clock and scripts resolve that clock through the active Inno.Runtime.RuntimeSession scope. | — |

## 项目依赖

- [Inno.Extensibility.Modules](../extensibility/Inno.Extensibility.Modules.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Coroutines](../core/Inno.Core.Coroutines.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：公开引用边界由实际签名核对。
- [Inno.Core.Jobs](../core/Inno.Core.Jobs.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Assets](../assets/Inno.Assets.md)：公开引用边界由实际签名核对。
- [Inno.Content](../assets/Inno.Content.md)：公开引用边界由实际签名核对。
- [Inno.Core.Events](../core/Inno.Core.Events.md)：公开引用边界由实际签名核对。
- [Inno.Core.Identity](../core/Inno.Core.Identity.md)：公开引用边界由实际签名核对。
- [Inno.Core.Logging](../core/Inno.Core.Logging.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
- [Inno.Core.Settings](../core/Inno.Core.Settings.md)：公开引用边界由实际签名核对。
- [Inno.References](../references/Inno.References.md)：公开引用边界由实际签名核对。
- [Inno.Scene](../scene/Inno.Scene.md)：公开引用边界由实际签名核对。
- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Runtime.Contracts](Inno.Runtime.Contracts.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Reload](../extensibility/Inno.Extensibility.Reload.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
