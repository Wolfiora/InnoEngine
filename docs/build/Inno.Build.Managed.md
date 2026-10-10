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
using Inno.Build.Managed;
using Inno.Build.Managed.DotNet;

var publishers = new ManagedDeploymentCatalog([
    new CoreClrDeploymentCompiler(dotnetHost),
    new MonoWasmDeploymentCompiler(dotnetHost, aheadOfTime: false),
    new MonoWasmDeploymentCompiler(dotnetHost, aheadOfTime: true),
    new NativeAotDeploymentCompiler(dotnetHost)
]);
IManagedDeploymentCompiler compiler = publishers.Resolve(ManagedDeploymentId.monoWasm, "browser-wasm");
ManagedDeploymentResult output = await compiler.CompileAsync(request, cancellationToken);
```

Provider 不安装最终输出。Build Pipeline 独立验证 result identity、staging owner 和完整文件清单，再交平台布局；失败和取消保留上次完整产品。来源代码、generator 与 link template 留在 staging/Support Pack，不进入用户游戏内容。

新增 CoreCLR Wasm 或其他 runtime 时增加一个 provider 并由 composition 注册，复用目录、内容、事件、Scene 和 Player。当前尚未实现的 runtime 不注册、不提供占位 publisher。

## 测试

`ManagedDeploymentTests` 使用开放自定义 provider 验证 catalog 与 immutable boundary；`BuildPipelineTests` 验证真实 pipeline 的发布、失败、取消与原子提交。真实 .NET、Web AOT、NativeAOT 的独立验收结果由本轮验收报告记录。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Build.Managed.IManagedDeploymentCompiler`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Managed.IManagedDeploymentCompiler`](../../build/managed/Inno.Build.Managed/IManagedDeploymentCompiler.cs#L9) | Publishes an explicit code closure independently of platform layout, content packaging and signing. |
| [`Inno.Build.Managed.ManagedDeploymentCapabilities Inno.Build.Managed.IManagedDeploymentCompiler.capabilities`](../../build/managed/Inno.Build.Managed/IManagedDeploymentCompiler.cs#L19) | Gets the provider's actual target and execution capabilities. |
| [`Inno.Build.Managed.ManagedDeploymentId Inno.Build.Managed.IManagedDeploymentCompiler.id`](../../build/managed/Inno.Build.Managed/IManagedDeploymentCompiler.cs#L14) | Gets the open stable implementation identity. |
| [`System.Threading.Tasks.ValueTask<Inno.Build.Managed.ManagedDeploymentResult> Inno.Build.Managed.IManagedDeploymentCompiler.CompileAsync(Inno.Build.Managed.ManagedDeploymentRequest request, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/managed/Inno.Build.Managed/IManagedDeploymentCompiler.cs#L39) | Compiles or publishes the prepared code closure into isolated managed staging. |

### `Inno.Build.Managed.ManagedDeploymentCapabilities`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Managed.ManagedDeploymentCapabilities`](../../build/managed/Inno.Build.Managed/ManagedDeploymentCapabilities.cs#L10) | Describes the execution and native-link capabilities of one publisher over explicit runtime targets. |
| [`Inno.Build.Managed.ManagedDeploymentCapabilities.ManagedDeploymentCapabilities(System.Collections.Generic.IReadOnlyList<string> runtimeIdentifiers, bool dynamicCode, bool aheadOfTime, bool nativeStaticLinking)`](../../build/managed/Inno.Build.Managed/ManagedDeploymentCapabilities.cs#L27) | Freezes the execution policy and supported runtime identifiers declared by a provider. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Build.Managed.ManagedDeploymentCapabilities.runtimeIdentifiers`](../../build/managed/Inno.Build.Managed/ManagedDeploymentCapabilities.cs#L46) | Gets the immutable supported runtime identifiers. |
| [`bool Inno.Build.Managed.ManagedDeploymentCapabilities.aheadOfTime`](../../build/managed/Inno.Build.Managed/ManagedDeploymentCapabilities.cs#L56) | Gets whether managed code is compiled ahead of runtime execution. |
| [`bool Inno.Build.Managed.ManagedDeploymentCapabilities.dynamicCode`](../../build/managed/Inno.Build.Managed/ManagedDeploymentCapabilities.cs#L51) | Gets whether the deployed runtime permits dynamic executable code generation. |
| [`bool Inno.Build.Managed.ManagedDeploymentCapabilities.nativeStaticLinking`](../../build/managed/Inno.Build.Managed/ManagedDeploymentCapabilities.cs#L61) | Gets whether native objects participate in final linking. |

### `Inno.Build.Managed.ManagedDeploymentCatalog`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Managed.IManagedDeploymentCompiler Inno.Build.Managed.ManagedDeploymentCatalog.Resolve(Inno.Build.Managed.ManagedDeploymentId deployment, string runtimeIdentifier)`](../../build/managed/Inno.Build.Managed/ManagedDeploymentCatalog.cs#L68) | Resolves a provider and verifies target support before staging begins. |
| [`Inno.Build.Managed.ManagedDeploymentCatalog`](../../build/managed/Inno.Build.Managed/ManagedDeploymentCatalog.cs#L10) | Owns an immutable collection of deployment providers selected explicitly by the build composition. |
| [`Inno.Build.Managed.ManagedDeploymentCatalog.ManagedDeploymentCatalog(System.Collections.Generic.IReadOnlyList<Inno.Build.Managed.IManagedDeploymentCompiler> compilers)`](../../build/managed/Inno.Build.Managed/ManagedDeploymentCatalog.cs#L23) | Copies providers and rejects ambiguous identities before any publication starts. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Build.Managed.ManagedDeploymentId> Inno.Build.Managed.ManagedDeploymentCatalog.GetSupportedDeployments(string runtimeIdentifier)`](../../build/managed/Inno.Build.Managed/ManagedDeploymentCatalog.cs#L49) | Returns the registered deployment identities that support one managed target. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Build.Managed.ManagedDeploymentId> Inno.Build.Managed.ManagedDeploymentCatalog.availableDeployments`](../../build/managed/Inno.Build.Managed/ManagedDeploymentCatalog.cs#L38) | Gets the immutable provider identities in stable order. |

### `Inno.Build.Managed.ManagedDeploymentId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Managed.ManagedDeploymentId`](../../build/managed/Inno.Build.Managed/ManagedDeploymentId.cs#L8) | Identifies an open managed deployment implementation independently of the publication platform. |
| [`Inno.Build.Managed.ManagedDeploymentId.ManagedDeploymentId(string value)`](../../build/managed/Inno.Build.Managed/ManagedDeploymentId.cs#L39) | Validates a stable portable provider identity. |
| [`override string Inno.Build.Managed.ManagedDeploymentId.ToString()`](../../build/managed/Inno.Build.Managed/ManagedDeploymentId.cs#L54) | See the implemented contract. |
| [`static Inno.Build.Managed.ManagedDeploymentId Inno.Build.Managed.ManagedDeploymentId.coreClr`](../../build/managed/Inno.Build.Managed/ManagedDeploymentId.cs#L13) | Identifies the desktop CoreCLR publisher. |
| [`static Inno.Build.Managed.ManagedDeploymentId Inno.Build.Managed.ManagedDeploymentId.monoWasm`](../../build/managed/Inno.Build.Managed/ManagedDeploymentId.cs#L18) | Identifies the Mono WebAssembly interpreter publisher. |
| [`static Inno.Build.Managed.ManagedDeploymentId Inno.Build.Managed.ManagedDeploymentId.monoWasmAot`](../../build/managed/Inno.Build.Managed/ManagedDeploymentId.cs#L23) | Identifies the Mono WebAssembly ahead-of-time publisher. |
| [`static Inno.Build.Managed.ManagedDeploymentId Inno.Build.Managed.ManagedDeploymentId.nativeAot`](../../build/managed/Inno.Build.Managed/ManagedDeploymentId.cs#L28) | Identifies the .NET NativeAOT publisher. |
| [`string Inno.Build.Managed.ManagedDeploymentId.value`](../../build/managed/Inno.Build.Managed/ManagedDeploymentId.cs#L51) | Gets the exact stable provider identity. |

### `Inno.Build.Managed.ManagedDeploymentRequest`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Managed.ManagedDeploymentRequest`](../../build/managed/Inno.Build.Managed/ManagedDeploymentRequest.cs#L9) | Freezes the build-owned project, code closure and output locations supplied to a managed publisher. |
| [`Inno.Build.Managed.ManagedDeploymentRequest.ManagedDeploymentRequest(string projectPath, string runtimeIdentifier, string codeInputDirectory, string outputDirectory, string logDirectory)`](../../build/managed/Inno.Build.Managed/ManagedDeploymentRequest.cs#L29) | Resolves all filesystem locations without starting processes or creating output directories. |
| [`string Inno.Build.Managed.ManagedDeploymentRequest.codeInputDirectory`](../../build/managed/Inno.Build.Managed/ManagedDeploymentRequest.cs#L61) | Gets the isolated frozen code input directory. |
| [`string Inno.Build.Managed.ManagedDeploymentRequest.logDirectory`](../../build/managed/Inno.Build.Managed/ManagedDeploymentRequest.cs#L71) | Gets the directory receiving process output and SDK selection evidence. |
| [`string Inno.Build.Managed.ManagedDeploymentRequest.outputDirectory`](../../build/managed/Inno.Build.Managed/ManagedDeploymentRequest.cs#L66) | Gets the managed publication staging directory. |
| [`string Inno.Build.Managed.ManagedDeploymentRequest.projectPath`](../../build/managed/Inno.Build.Managed/ManagedDeploymentRequest.cs#L51) | Gets the prepared entry project owned by this build. |
| [`string Inno.Build.Managed.ManagedDeploymentRequest.runtimeIdentifier`](../../build/managed/Inno.Build.Managed/ManagedDeploymentRequest.cs#L56) | Gets the exact target identifier accepted by the selected managed toolchain. |

### `Inno.Build.Managed.ManagedDeploymentResult`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Managed.ManagedDeploymentId Inno.Build.Managed.ManagedDeploymentResult.deployment`](../../build/managed/Inno.Build.Managed/ManagedDeploymentResult.cs#L51) | Gets the managed compiler identity that produced this publication. |
| [`Inno.Build.Managed.ManagedDeploymentResult`](../../build/managed/Inno.Build.Managed/ManagedDeploymentResult.cs#L11) | Describes verified staging output produced by one managed deployment compiler. |
| [`Inno.Build.Managed.ManagedDeploymentResult.ManagedDeploymentResult(Inno.Build.Managed.ManagedDeploymentId deployment, string outputDirectory, string sdkIdentity, System.Collections.Generic.IReadOnlyList<string> files)`](../../build/managed/Inno.Build.Managed/ManagedDeploymentResult.cs#L28) | Freezes successful publication evidence without granting ownership of the pipeline's staging root. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Build.Managed.ManagedDeploymentResult.files`](../../build/managed/Inno.Build.Managed/ManagedDeploymentResult.cs#L66) | Gets the frozen relative output file paths. |
| [`string Inno.Build.Managed.ManagedDeploymentResult.outputDirectory`](../../build/managed/Inno.Build.Managed/ManagedDeploymentResult.cs#L56) | Gets the completed build-owned publication directory. |
| [`string Inno.Build.Managed.ManagedDeploymentResult.sdkIdentity`](../../build/managed/Inno.Build.Managed/ManagedDeploymentResult.cs#L61) | Gets the project-selected managed SDK identity. |

## 项目依赖

- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
