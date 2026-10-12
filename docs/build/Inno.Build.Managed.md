# Inno.Build.Managed

[Build 索引](README.md) · [Wiki 首页](../README.md) · [DotNet 发布器](../backends/DotNet/Inno.Build.Managed.DotNet.md) · [Build Pipeline](Inno.Build.md)

## 职责与依赖

本库定义托管部署的中立契约，不引用平台 packager、Editor、Runtime、原生 SDK 或具体 .NET 发布实现。平台提供所需 target，Profile 选择 publisher，组合入口注入 catalog。领域 runtime 不知道使用哪种托管部署。

## 全部公开契约

| API | 当前行为 |
| --- | --- |
| `ManagedDeploymentId(value)` / `value` / `ToString()` | 开放、稳定的 provider identity；内建 coreClr、monoWasm、monoWasmAot、nativeAot 是已提供的 ID，不是封闭枚举。 |
| `ManagedDeploymentCapabilities(runtimeIdentifiers, dynamicCode, aheadOfTime, nativeStaticLinking)` | 防御性复制 target 列表，描述实际执行与原生链接能力。 |
| `ManagedDeploymentRequest(projectPath, runtimeIdentifier, codeInputDirectory, outputDirectory, logDirectory)` | 冻结项目、代码输入和隔离输出；构造时规范绝对路径，不启动工具或创建输出。 |
| `ManagedDeploymentResult(deployment, outputDirectory, sdkIdentity, files)` | 成功产物描述；复制 relative files，拒绝空产物、重复或越界路径。 |
| `IManagedDeploymentCompiler.id / capabilities / CompileAsync(request, cancellationToken)` | 唯一可替换 publisher 边界；错误/取消不得返回成功结果。 |
| `ManagedDeploymentCatalog(compilers)` | 冻结 provider 集合，拒绝空项与重复 identity。 |
| `availableDeployments / GetSupportedDeployments(runtimeIdentifier)` | 以稳定顺序返回不可变的已注册、支持目标的选择。 |
| `Resolve(deployment, runtimeIdentifier)` | 在 staging 前检查 provider 与 target；缺失能力明确失败。 |

没有 protected 扩展点；第三方 publisher 实现接口即可。

## 使用与生命周期

```csharp
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Managed;

static ValueTask<ManagedDeploymentResult> CompileAsync(
    ManagedDeploymentCatalog publishers,
    ManagedDeploymentId deployment,
    ManagedDeploymentRequest request,
    CancellationToken cancellationToken
) {
    IManagedDeploymentCompiler compiler = publishers.Resolve(
        deployment, request.runtimeIdentifier);
    return compiler.CompileAsync(request, cancellationToken);
}
```

Provider 不安装最终输出。Build Pipeline 独立验证 result identity、staging owner 和完整文件清单，再交平台布局；失败和取消保留上次完整产品。来源代码、generator 与 link template 留在 staging/Support Pack，不进入用户游戏内容。

新增 CoreCLR Wasm 或其他 runtime 时增加一个 provider 并由 composition 注册，复用目录、内容、事件、Scene 和 Player。当前尚未实现的 runtime 不注册、不提供占位 publisher。

## 测试

`ManagedDeploymentTests` 使用开放自定义 provider 验证 catalog 与 immutable boundary；`BuildPipelineTests` 验证真实 pipeline 的发布、失败、取消与原子提交。真实 .NET、Web AOT、NativeAOT 的独立验收结果由本轮验收报告记录。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Build.Managed.IManagedDeploymentCompiler`

<!-- api: T:Inno.Build.Managed.IManagedDeploymentCompiler 543fa5bca93196563a186dde9083d2afc08abb1d18c611e068dcaaa547519ed4 -->
<!-- api: P:Inno.Build.Managed.IManagedDeploymentCompiler.id 270fa17e93d461c5f78c864f5030ee11201d3309dbd6e6d53a7e790cc48a36c8 -->
<!-- api: P:Inno.Build.Managed.IManagedDeploymentCompiler.capabilities f0acc2836f170c9c489c830a769e023d287f045d8a45970361142de3ece64a17 -->
<!-- api: M:Inno.Build.Managed.IManagedDeploymentCompiler.CompileAsync(Inno.Build.Managed.ManagedDeploymentRequest,System.Threading.CancellationToken) cebba513e6f710b75ffb758f384cab2327fdadf9825217dfc43f6bf8e18f0dd4 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-5de3fd38e8da5769"></a>[`System.Threading.Tasks.ValueTask<Inno.Build.Managed.ManagedDeploymentResult> Inno.Build.Managed.IManagedDeploymentCompiler.CompileAsync(Inno.Build.Managed.ManagedDeploymentRequest request, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/managed/Inno.Build.Managed/IManagedDeploymentCompiler.cs#L39) | Compiles or publishes the prepared code closure into isolated managed staging.<br>返回：Verified output evidence; failed or canceled publication never returns a successful result.<br>`request`：The immutable prepared project, target, frozen inputs and staging locations.<br>`cancellationToken`：Cancels the publisher and waits for its process tree and output readers to retire. | — | `System.InvalidOperationException`：The target is unsupported, required toolchain inputs are absent, or publication fails.<br>`System.OperationCanceledException`：Publication was canceled and all owned work has stopped. |
| <a id="api-94e9222424967b4c"></a>[`Inno.Build.Managed.ManagedDeploymentCapabilities Inno.Build.Managed.IManagedDeploymentCompiler.capabilities { get; }`](../../build/managed/Inno.Build.Managed/IManagedDeploymentCompiler.cs#L19) | Gets the provider's actual target and execution capabilities. | — | — |
| <a id="api-de60136130b95186"></a>[`Inno.Build.Managed.ManagedDeploymentId Inno.Build.Managed.IManagedDeploymentCompiler.id { get; }`](../../build/managed/Inno.Build.Managed/IManagedDeploymentCompiler.cs#L14) | Gets the open stable implementation identity. | — | — |
| <a id="api-635da91fa2f65d7f"></a>[`public interface Inno.Build.Managed.IManagedDeploymentCompiler`](../../build/managed/Inno.Build.Managed/IManagedDeploymentCompiler.cs#L9) | Publishes an explicit code closure independently of platform layout, content packaging and signing. | — | — |

### `Inno.Build.Managed.ManagedDeploymentCapabilities`

<!-- api: T:Inno.Build.Managed.ManagedDeploymentCapabilities eefd86ac318fd4f132f660395535075496ed508215a535b7a38f715fa6a73c26 -->
<!-- api: M:Inno.Build.Managed.ManagedDeploymentCapabilities.#ctor(System.Collections.Generic.IReadOnlyList{System.String},System.Boolean,System.Boolean,System.Boolean) 1ec928cb3fa62ab3fdca29167b8a92685eaec962ce04b39e675fe5d1ad795eca -->
<!-- api: P:Inno.Build.Managed.ManagedDeploymentCapabilities.runtimeIdentifiers 9997d0b2f1926216e06ade067bb2139ff3d022eaecfde5ae5d473da6db94be97 -->
<!-- api: P:Inno.Build.Managed.ManagedDeploymentCapabilities.dynamicCode 760ff6f17b4b64c884a8e2cc9d1b0004a3989c260f998afdb1936e67fead050d -->
<!-- api: P:Inno.Build.Managed.ManagedDeploymentCapabilities.aheadOfTime 8fd4a98ae546027060f3ee083d16171c7850c165be2f266812be62ed58e6b11a -->
<!-- api: P:Inno.Build.Managed.ManagedDeploymentCapabilities.nativeStaticLinking 006777093d6d1a918a6df49b79ce8e2aa0fadf2878a6e3e323d8ce0ab07fcce1 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8ccc55ed52c80941"></a>[`public Inno.Build.Managed.ManagedDeploymentCapabilities.ManagedDeploymentCapabilities(System.Collections.Generic.IReadOnlyList<string> runtimeIdentifiers, bool dynamicCode, bool aheadOfTime, bool nativeStaticLinking)`](../../build/managed/Inno.Build.Managed/ManagedDeploymentCapabilities.cs#L27) | Freezes the execution policy and supported runtime identifiers declared by a provider.<br>`runtimeIdentifiers`：The exact runtime identifiers accepted by the selected managed toolchain.<br>`dynamicCode`：Whether the deployed runtime supports generating executable managed code.<br>`aheadOfTime`：Whether this publisher compiles managed code before deployment.<br>`nativeStaticLinking`：Whether native object inputs can participate in the runtime's final link. | — | — |
| <a id="api-966062c6eb6397b3"></a>[`public bool Inno.Build.Managed.ManagedDeploymentCapabilities.aheadOfTime { get; }`](../../build/managed/Inno.Build.Managed/ManagedDeploymentCapabilities.cs#L56) | Gets whether managed code is compiled ahead of runtime execution. | — | — |
| <a id="api-ba499725cc1d03b2"></a>[`public bool Inno.Build.Managed.ManagedDeploymentCapabilities.dynamicCode { get; }`](../../build/managed/Inno.Build.Managed/ManagedDeploymentCapabilities.cs#L51) | Gets whether the deployed runtime permits dynamic executable code generation. | — | — |
| <a id="api-28d9e5d8e0741371"></a>[`public bool Inno.Build.Managed.ManagedDeploymentCapabilities.nativeStaticLinking { get; }`](../../build/managed/Inno.Build.Managed/ManagedDeploymentCapabilities.cs#L61) | Gets whether native objects participate in final linking. | — | — |
| <a id="api-28f886c39fd737a0"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Build.Managed.ManagedDeploymentCapabilities.runtimeIdentifiers { get; }`](../../build/managed/Inno.Build.Managed/ManagedDeploymentCapabilities.cs#L46) | Gets the immutable supported runtime identifiers. | — | — |
| <a id="api-3fd177d3b264a5aa"></a>[`public sealed class Inno.Build.Managed.ManagedDeploymentCapabilities`](../../build/managed/Inno.Build.Managed/ManagedDeploymentCapabilities.cs#L10) | Describes the execution and native-link capabilities of one publisher over explicit runtime targets. | — | — |

### `Inno.Build.Managed.ManagedDeploymentCatalog`

<!-- api: T:Inno.Build.Managed.ManagedDeploymentCatalog c8e2a4ab21f6a359e0548fda32d8e1138cbeba8715fb0452b1a38e311f9153c9 -->
<!-- api: M:Inno.Build.Managed.ManagedDeploymentCatalog.#ctor(System.Collections.Generic.IReadOnlyList{Inno.Build.Managed.IManagedDeploymentCompiler}) 42d225e9936dfa1f71c052c79be5b28eee44b357b60be777b714cc9dd1e250bf -->
<!-- api: P:Inno.Build.Managed.ManagedDeploymentCatalog.availableDeployments a7f1f20e209459d74423f2741cc3cb69c4fe29e7d53d942ec75225ae97c642f0 -->
<!-- api: M:Inno.Build.Managed.ManagedDeploymentCatalog.GetSupportedDeployments(System.String) 9a50fb01eb49f5abd4258b3a9ae4102cb29d1508b8aa117e6dd82541e5a3961a -->
<!-- api: M:Inno.Build.Managed.ManagedDeploymentCatalog.Resolve(Inno.Build.Managed.ManagedDeploymentId,System.String) 5ff8f40ca9a2cc46c5c760efadad84207c8fefef2aa05d6ec7081c61901c06d4 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-150f773622a5c621"></a>[`public Inno.Build.Managed.ManagedDeploymentCatalog.ManagedDeploymentCatalog(System.Collections.Generic.IReadOnlyList<Inno.Build.Managed.IManagedDeploymentCompiler> compilers)`](../../build/managed/Inno.Build.Managed/ManagedDeploymentCatalog.cs#L23) | Copies providers and rejects ambiguous identities before any publication starts.<br>`compilers`：The explicit provider implementations owned by the composition root. | — | `System.ArgumentException`：The collection is empty, contains null entries or has duplicate or invalid identities. |
| <a id="api-eebb4c100200af07"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Build.Managed.ManagedDeploymentId> Inno.Build.Managed.ManagedDeploymentCatalog.GetSupportedDeployments(string runtimeIdentifier)`](../../build/managed/Inno.Build.Managed/ManagedDeploymentCatalog.cs#L49) | Returns the registered deployment identities that support one managed target.<br>返回：A new immutable list in stable identity order; unsupported targets produce an empty list.<br>`runtimeIdentifier`：The managed toolchain target selected by the platform composition. | — | — |
| <a id="api-da273fad0b82d1ce"></a>[`public Inno.Build.Managed.IManagedDeploymentCompiler Inno.Build.Managed.ManagedDeploymentCatalog.Resolve(Inno.Build.Managed.ManagedDeploymentId deployment, string runtimeIdentifier)`](../../build/managed/Inno.Build.Managed/ManagedDeploymentCatalog.cs#L68) | Resolves a provider and verifies target support before staging begins.<br>返回：The provider that supports the requested target; missing capabilities throw explicitly.<br>`deployment`：The stable provider identity selected by the build profile.<br>`runtimeIdentifier`：The exact managed target required by the platform composition. | — | `System.InvalidOperationException`：No provider is registered, or the selected provider cannot publish the requested target. |
| <a id="api-688f0048e31b6000"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Build.Managed.ManagedDeploymentId> Inno.Build.Managed.ManagedDeploymentCatalog.availableDeployments { get; }`](../../build/managed/Inno.Build.Managed/ManagedDeploymentCatalog.cs#L38) | Gets the immutable provider identities in stable order. | — | — |
| <a id="api-d7d0909e210c9df0"></a>[`public sealed class Inno.Build.Managed.ManagedDeploymentCatalog`](../../build/managed/Inno.Build.Managed/ManagedDeploymentCatalog.cs#L10) | Owns an immutable collection of deployment providers selected explicitly by the build composition. | — | — |

### `Inno.Build.Managed.ManagedDeploymentId`

<!-- api: T:Inno.Build.Managed.ManagedDeploymentId d5c7a5a4b5b4ceb7ecdeaa40017f539447064d8aa175ff6e6f95087cf84995c0 -->
<!-- api: P:Inno.Build.Managed.ManagedDeploymentId.coreClr f2e2d04241de887e379af8c9c4f53b3138ed11d59a09da084c1578ac7bd2d41a -->
<!-- api: P:Inno.Build.Managed.ManagedDeploymentId.monoWasm 251a6512af93eea1d781603750e103827fe60fda7b4a8fdef4a80a0ac4adc5b3 -->
<!-- api: P:Inno.Build.Managed.ManagedDeploymentId.monoWasmAot 4626d1bbd6f91fe34544286e9617046c1253e570f639b864d0aabcfa210f1d49 -->
<!-- api: P:Inno.Build.Managed.ManagedDeploymentId.nativeAot 82ebf1578582fc46b66a08a8d8a42471a8d9ac579f10a246eebe16332c785ada -->
<!-- api: M:Inno.Build.Managed.ManagedDeploymentId.#ctor(System.String) f0d67fc4ab1c006edeeb8755b97e73b0882d390ed5346abda9ac2211b77b3b7d -->
<!-- api: P:Inno.Build.Managed.ManagedDeploymentId.value 16be2d08f8819f41f8f743fabfad02b8b692b61234915f54e367163064bd0d15 -->
<!-- api: M:Inno.Build.Managed.ManagedDeploymentId.ToString 0888c2c3e13b3c0a012f3e9cc22dfaaf4118c3544b7e89f5bea0b0528096f0ca -->
<!-- api: M:Inno.Build.Managed.ManagedDeploymentId.op_Inequality(Inno.Build.Managed.ManagedDeploymentId,Inno.Build.Managed.ManagedDeploymentId) 93772a6145e1776b0d5ecd35e8c69c9405fdfc07f567413680c8c36a7928c930 -->
<!-- api: M:Inno.Build.Managed.ManagedDeploymentId.op_Equality(Inno.Build.Managed.ManagedDeploymentId,Inno.Build.Managed.ManagedDeploymentId) 08e0d5fe0300055c7aab654b0d96ad969aea2b59bfc403d1ffb40946092d15c3 -->
<!-- api: M:Inno.Build.Managed.ManagedDeploymentId.GetHashCode ca31d24e3f78ff2cf6248484603cac3be90cbc1539a8fc3c59caf45ffd335bec -->
<!-- api: M:Inno.Build.Managed.ManagedDeploymentId.Equals(System.Object) f1fc46270d7a3ef3e171306edb86b02951af1c1847c07433ff4b5c6f0d4b85ef -->
<!-- api: M:Inno.Build.Managed.ManagedDeploymentId.Equals(Inno.Build.Managed.ManagedDeploymentId) eb8922f12d2383cd27564921a62d9219b58cf2d5a82591bcd84fcb16cb997eb3 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b22849ad5ca90e6c"></a>[`public Inno.Build.Managed.ManagedDeploymentId.ManagedDeploymentId(string value)`](../../build/managed/Inno.Build.Managed/ManagedDeploymentId.cs#L39) | Validates a stable portable provider identity.<br>`value`：A nonempty lowercase identifier composed of letters, digits and hyphens. | — | `System.ArgumentException`：The identity is empty or contains nonportable characters. |
| <a id="api-2f0333cd744d8b12"></a>[`public bool Inno.Build.Managed.ManagedDeploymentId.Equals(Inno.Build.Managed.ManagedDeploymentId other)`](../../build/managed/Inno.Build.Managed/ManagedDeploymentId.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-bc9e4e4aab999a82"></a>[`public override bool Inno.Build.Managed.ManagedDeploymentId.Equals(object obj)`](../../build/managed/Inno.Build.Managed/ManagedDeploymentId.cs#L8) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-f7ab7157e1843974"></a>[`public override int Inno.Build.Managed.ManagedDeploymentId.GetHashCode()`](../../build/managed/Inno.Build.Managed/ManagedDeploymentId.cs#L8) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-825b69158b311022"></a>[`public static bool Inno.Build.Managed.ManagedDeploymentId.operator ==(Inno.Build.Managed.ManagedDeploymentId left, Inno.Build.Managed.ManagedDeploymentId right)`](../../build/managed/Inno.Build.Managed/ManagedDeploymentId.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-974480c1a55c696a"></a>[`public static bool Inno.Build.Managed.ManagedDeploymentId.operator !=(Inno.Build.Managed.ManagedDeploymentId left, Inno.Build.Managed.ManagedDeploymentId right)`](../../build/managed/Inno.Build.Managed/ManagedDeploymentId.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-0d1f1fe11ff5fc3f"></a>[`public override string Inno.Build.Managed.ManagedDeploymentId.ToString()`](../../build/managed/Inno.Build.Managed/ManagedDeploymentId.cs#L54) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-5e7448bdba360a83"></a>[`public static Inno.Build.Managed.ManagedDeploymentId Inno.Build.Managed.ManagedDeploymentId.coreClr { get; }`](../../build/managed/Inno.Build.Managed/ManagedDeploymentId.cs#L13) | Identifies the desktop CoreCLR publisher. | — | — |
| <a id="api-c654bd8b76882928"></a>[`public static Inno.Build.Managed.ManagedDeploymentId Inno.Build.Managed.ManagedDeploymentId.monoWasm { get; }`](../../build/managed/Inno.Build.Managed/ManagedDeploymentId.cs#L18) | Identifies the Mono WebAssembly interpreter publisher. | — | — |
| <a id="api-21f4f4c3f2232579"></a>[`public static Inno.Build.Managed.ManagedDeploymentId Inno.Build.Managed.ManagedDeploymentId.monoWasmAot { get; }`](../../build/managed/Inno.Build.Managed/ManagedDeploymentId.cs#L23) | Identifies the Mono WebAssembly ahead-of-time publisher. | — | — |
| <a id="api-e1b27ff6981ecd26"></a>[`public static Inno.Build.Managed.ManagedDeploymentId Inno.Build.Managed.ManagedDeploymentId.nativeAot { get; }`](../../build/managed/Inno.Build.Managed/ManagedDeploymentId.cs#L28) | Identifies the .NET NativeAOT publisher. | — | — |
| <a id="api-5bf1dd6c932f8631"></a>[`public string Inno.Build.Managed.ManagedDeploymentId.value { get; }`](../../build/managed/Inno.Build.Managed/ManagedDeploymentId.cs#L51) | Gets the exact stable provider identity. | — | — |
| <a id="api-dda51a2f21105456"></a>[`public readonly record struct Inno.Build.Managed.ManagedDeploymentId : System.IEquatable<Inno.Build.Managed.ManagedDeploymentId>`](../../build/managed/Inno.Build.Managed/ManagedDeploymentId.cs#L8) | Identifies an open managed deployment implementation independently of the publication platform. | — | — |

### `Inno.Build.Managed.ManagedDeploymentRequest`

<!-- api: T:Inno.Build.Managed.ManagedDeploymentRequest 080d3b05637b73016b29b7122e2888e4087c699322653c03f45e14da571732e2 -->
<!-- api: M:Inno.Build.Managed.ManagedDeploymentRequest.#ctor(System.String,System.String,System.String,System.String,System.String) 96c0a519bd3910f4fbced1be799e44cafccdc62d55e588937d4409553c2e283c -->
<!-- api: P:Inno.Build.Managed.ManagedDeploymentRequest.projectPath 99abb81d06aa7b1263ae036e0044d77d1637148104fd02a2db5ae8b5627c34cc -->
<!-- api: P:Inno.Build.Managed.ManagedDeploymentRequest.runtimeIdentifier 481082b80d1ed457e5c52e0ef0659c4f2bcaee03c4181f586fdeaa7f0138fc8c -->
<!-- api: P:Inno.Build.Managed.ManagedDeploymentRequest.codeInputDirectory 2c9a4a4dbc0abd4b46efed4d8ba775bf7ee5d566698cd472b1a20028de96814f -->
<!-- api: P:Inno.Build.Managed.ManagedDeploymentRequest.outputDirectory 0598e5b2aa0590ee458c6580ec49fc0aa3cba813865d92b89fa9af70a9ebc5aa -->
<!-- api: P:Inno.Build.Managed.ManagedDeploymentRequest.logDirectory d38ab28390012a7804b1818139bc544618df52575cbc155af5e5e425d0efdeda -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-dd0046e0e6aec016"></a>[`public Inno.Build.Managed.ManagedDeploymentRequest.ManagedDeploymentRequest(string projectPath, string runtimeIdentifier, string codeInputDirectory, string outputDirectory, string logDirectory)`](../../build/managed/Inno.Build.Managed/ManagedDeploymentRequest.cs#L29) | Resolves all filesystem locations without starting processes or creating output directories.<br>`projectPath`：The prepared entry project with explicit code references, generated registrations and native inputs.<br>`runtimeIdentifier`：The managed toolchain's exact target identifier.<br>`codeInputDirectory`：The isolated directory containing the frozen code inputs referenced by the entry project.<br>`outputDirectory`：The empty build-owned staging directory; final installation remains the pipeline's responsibility.<br>`logDirectory`：The build-owned directory receiving process output and SDK selection evidence. | — | — |
| <a id="api-f31a4dc44e07fb0a"></a>[`public string Inno.Build.Managed.ManagedDeploymentRequest.codeInputDirectory { get; }`](../../build/managed/Inno.Build.Managed/ManagedDeploymentRequest.cs#L61) | Gets the isolated frozen code input directory. | — | — |
| <a id="api-a2f1b8c66c6c44b7"></a>[`public string Inno.Build.Managed.ManagedDeploymentRequest.logDirectory { get; }`](../../build/managed/Inno.Build.Managed/ManagedDeploymentRequest.cs#L71) | Gets the directory receiving process output and SDK selection evidence. | — | — |
| <a id="api-db16f0036fff2ada"></a>[`public string Inno.Build.Managed.ManagedDeploymentRequest.outputDirectory { get; }`](../../build/managed/Inno.Build.Managed/ManagedDeploymentRequest.cs#L66) | Gets the managed publication staging directory. | — | — |
| <a id="api-a41ba8f704af46d9"></a>[`public string Inno.Build.Managed.ManagedDeploymentRequest.projectPath { get; }`](../../build/managed/Inno.Build.Managed/ManagedDeploymentRequest.cs#L51) | Gets the prepared entry project owned by this build. | — | — |
| <a id="api-212a6c50fd836053"></a>[`public string Inno.Build.Managed.ManagedDeploymentRequest.runtimeIdentifier { get; }`](../../build/managed/Inno.Build.Managed/ManagedDeploymentRequest.cs#L56) | Gets the exact target identifier accepted by the selected managed toolchain. | — | — |
| <a id="api-a150216bb34fa498"></a>[`public sealed class Inno.Build.Managed.ManagedDeploymentRequest`](../../build/managed/Inno.Build.Managed/ManagedDeploymentRequest.cs#L9) | Freezes the build-owned project, code closure and output locations supplied to a managed publisher. | — | — |

### `Inno.Build.Managed.ManagedDeploymentResult`

<!-- api: T:Inno.Build.Managed.ManagedDeploymentResult ff4ccf536893e5cfe43f15bd2c9307b93668c5d7a4d87d848fd9f836ab18ff14 -->
<!-- api: M:Inno.Build.Managed.ManagedDeploymentResult.#ctor(Inno.Build.Managed.ManagedDeploymentId,System.String,System.String,System.Collections.Generic.IReadOnlyList{System.String}) c3b651fff7eefc63b75903af4eb12428a31b8b14d6636f3e827f4d220ad08baa -->
<!-- api: P:Inno.Build.Managed.ManagedDeploymentResult.deployment 21c8ce0a7f072225f9c7290d6aac3cde8be8daf8d02a777bf7e718b066db9c91 -->
<!-- api: P:Inno.Build.Managed.ManagedDeploymentResult.outputDirectory a5b95c8991e341e76ead778b57c9133c80ae7e08fa5c9b9e0373c6c27f40c212 -->
<!-- api: P:Inno.Build.Managed.ManagedDeploymentResult.sdkIdentity 71094c1cf95a3d072bd17c6a9ca7efe51aaa8a9e8b7725bbd467f386cc849758 -->
<!-- api: P:Inno.Build.Managed.ManagedDeploymentResult.files b491c7556d73a5573e2af8ef4b58ed5cc52c2011eb7639884b1fdb1b69b44f6a -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7f31c9b0a44f7ed6"></a>[`public Inno.Build.Managed.ManagedDeploymentResult.ManagedDeploymentResult(Inno.Build.Managed.ManagedDeploymentId deployment, string outputDirectory, string sdkIdentity, System.Collections.Generic.IReadOnlyList<string> files)`](../../build/managed/Inno.Build.Managed/ManagedDeploymentResult.cs#L28) | Freezes successful publication evidence without granting ownership of the pipeline's staging root.<br>`deployment`：The compiler identity that produced this output.<br>`outputDirectory`：The directory containing the completed managed publication.<br>`sdkIdentity`：The exact managed SDK selected from the entry project's configuration.<br>`files`：The nonempty set of relative output file paths, copied before returning to the pipeline. | — | — |
| <a id="api-5c2a3a4fa7ac46e3"></a>[`public Inno.Build.Managed.ManagedDeploymentId Inno.Build.Managed.ManagedDeploymentResult.deployment { get; }`](../../build/managed/Inno.Build.Managed/ManagedDeploymentResult.cs#L51) | Gets the managed compiler identity that produced this publication. | — | — |
| <a id="api-a2b1f3b106165d1e"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Build.Managed.ManagedDeploymentResult.files { get; }`](../../build/managed/Inno.Build.Managed/ManagedDeploymentResult.cs#L66) | Gets the frozen relative output file paths. | — | — |
| <a id="api-0eba48d0d4dd9276"></a>[`public string Inno.Build.Managed.ManagedDeploymentResult.outputDirectory { get; }`](../../build/managed/Inno.Build.Managed/ManagedDeploymentResult.cs#L56) | Gets the completed build-owned publication directory. | — | — |
| <a id="api-17585f78b073ffff"></a>[`public string Inno.Build.Managed.ManagedDeploymentResult.sdkIdentity { get; }`](../../build/managed/Inno.Build.Managed/ManagedDeploymentResult.cs#L61) | Gets the project-selected managed SDK identity. | — | — |
| <a id="api-7fa7025e76d36adc"></a>[`public sealed class Inno.Build.Managed.ManagedDeploymentResult`](../../build/managed/Inno.Build.Managed/ManagedDeploymentResult.cs#L11) | Describes verified staging output produced by one managed deployment compiler. | — | — |

## 项目依赖

- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
