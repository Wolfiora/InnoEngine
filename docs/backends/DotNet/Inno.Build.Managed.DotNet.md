# Inno.Build.Managed.DotNet

[分类索引](README.md) · [Wiki 首页](../../README.md) · [托管契约](../../build/Inno.Build.Managed.md) · [Toolchains](../../build/Inno.Build.Toolchains.md)

## 分层与公开入口

本库依赖 Managed 契约与 Toolchains 的项目 SDK/进程执行能力。它不负责 Scene、内容 pack、平台布局、签名或 Editor UI。

| 公开类型 | 入口与能力 |
| --- | --- |
| `CoreClrDeploymentCompiler(hostPath)` | id=coreclr；支持 win-x64、osx-arm64、linux-x64/arm64；动态代码可用，self-contained、单文件、静态注册目录发布。 |
| `MonoWasmDeploymentCompiler(hostPath, aheadOfTime)` | id=mono-wasm 或 mono-wasm-aot；支持 browser-wasm；同一代码闭包、不同 SDK 编译策略，原生静态链接。 |
| `NativeAotDeploymentCompiler(hostPath)` | id=nativeaot；支持桌面 runtime identifiers；PublishAot，没有运行时 JIT 或 collectible loader。 |
| 各 publisher 的 `id / capabilities / CompileAsync` | 实现中立托管发布契约；preflight、SDK 解析、隐藏进程、日志与完成验证。 |

支持目标声明不代表某台宿主已经安装其交叉编译 SDK。缺少 workload、平台 SDK 或 native linker 会明确失败。本机实际执行矩阵与其他硬件验收状态独立记录。

## 发布和取消

内部 `DotNetPublishRequest / DotNetPublishExecutor / DotNetDeploymentPublisher` 收口参数、日志和发布，不是稳定公开 API。entry project 的 global.json 控制 SDK；`DotNetSdkResolver` 调用所选 host 从项目目录解析，不猜最高已安装版本。原生 inputs 在 prepared project 声明，SDK/workload 决定最终链接。

桌面模板的 `native/` 是明确声明的外部原生闭包。CoreCLR 单文件发布保留该目录，不把这些库嵌入托管可执行文件；同一模板也供桌面 NativeAOT 使用。发布前拒绝空闭包，发布后逐项检查声明的文件确实存在。运行时只加载部署声明的路径，发布检查和实际启动共同构成验收。

进程生命周期复用 `ToolchainEnvironment`。输出完整写入 managed-output.log / managed-error.log，异常只保留有界尾部；SDK identity 与冻结的 CLI 入口分别记录在 managed-sdk.txt / managed-cli.txt。取消或 output reader 失败先终止完整子进程树、等待退出和排空，再传播原异常。executor 不接管最终产品目录。

prepared project 的执行目录复用 `ToolchainWorkingDirectory`。Windows 工具使用经所有权保护的短 junction，
`managed-working-directory.txt` 记录物理目录与执行目录；SDK 仍从原项目解析，代码输入、最终 staging 和日志的 owner 不变。
这同时服务桌面和 Web 发布，不在共同运行时加入宿主判断。进程退出并排空后删除 alias，物理产物保留。

调用示例见 [Managed](../../build/Inno.Build.Managed.md)。自动化验证不得以 build 成功代替真实 runtime 调用、游戏启动或视觉验收。

## 源码归属

当前唯一源码 owner：`backends/DotNet/build/Inno.Build.Managed.DotNet/Inno.Build.Managed.DotNet.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Build.Managed.DotNet.CoreClrDeploymentCompiler`

<!-- api: T:Inno.Build.Managed.DotNet.CoreClrDeploymentCompiler 870286e2ae6d1ca65709949c1568dfd2714bb63eb2dbad0bc0a5c092c9b82bad -->
<!-- api: M:Inno.Build.Managed.DotNet.CoreClrDeploymentCompiler.#ctor(System.String) bf98bf9abb2b7677926190133fb71275ed887b2369d77f88e9209107b1bfdf69 -->
<!-- api: P:Inno.Build.Managed.DotNet.CoreClrDeploymentCompiler.id 28ee9aae65b6406f87a6d41a1cb640e22889d8d2716d3a36a62134d3214798a4 -->
<!-- api: P:Inno.Build.Managed.DotNet.CoreClrDeploymentCompiler.capabilities 096258fcf8c4c2a4a7a2f6e459cef04c96998b59e7bfff1384609f4cfc9faf13 -->
<!-- api: M:Inno.Build.Managed.DotNet.CoreClrDeploymentCompiler.CompileAsync(Inno.Build.Managed.ManagedDeploymentRequest,System.Threading.CancellationToken) 11a845da37d7e9d352827107c7367b20af9df0af5737b3083e55bdfe5abee1b6 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1513eba7828adea0"></a>[`public Inno.Build.Managed.DotNet.CoreClrDeploymentCompiler.CoreClrDeploymentCompiler(string hostPath)`](../../../backends/DotNet/build/Inno.Build.Managed.DotNet/Desktop/CoreClrDeploymentCompiler.cs#L20) | Selects the SDK host used for project-scoped publication.<br>`hostPath`：The .NET executable selected by the build composition root. | — | — |
| <a id="api-b6b3491024c85356"></a>[`public System.Threading.Tasks.ValueTask<Inno.Build.Managed.ManagedDeploymentResult> Inno.Build.Managed.DotNet.CoreClrDeploymentCompiler.CompileAsync(Inno.Build.Managed.ManagedDeploymentRequest request, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/DotNet/build/Inno.Build.Managed.DotNet/Desktop/CoreClrDeploymentCompiler.cs#L35) | Compiles or publishes the prepared code closure into isolated managed staging.<br>返回：Verified output evidence; failed or canceled publication never returns a successful result.<br>`request`：The immutable prepared project, target, frozen inputs and staging locations.<br>`cancellationToken`：Cancels the publisher and waits for its process tree and output readers to retire. | — | `System.InvalidOperationException`：The target is unsupported, required toolchain inputs are absent, or publication fails.<br>`System.OperationCanceledException`：Publication was canceled and all owned work has stopped. |
| <a id="api-84d91987256a760a"></a>[`public Inno.Build.Managed.ManagedDeploymentCapabilities Inno.Build.Managed.DotNet.CoreClrDeploymentCompiler.capabilities { get; }`](../../../backends/DotNet/build/Inno.Build.Managed.DotNet/Desktop/CoreClrDeploymentCompiler.cs#L30) | Gets the provider's actual target and execution capabilities. | — | — |
| <a id="api-41b1176c74353b70"></a>[`public Inno.Build.Managed.ManagedDeploymentId Inno.Build.Managed.DotNet.CoreClrDeploymentCompiler.id { get; }`](../../../backends/DotNet/build/Inno.Build.Managed.DotNet/Desktop/CoreClrDeploymentCompiler.cs#L27) | Gets the open stable implementation identity. | — | — |
| <a id="api-e0ed560b676ff39d"></a>[`public sealed class Inno.Build.Managed.DotNet.CoreClrDeploymentCompiler : Inno.Build.Managed.IManagedDeploymentCompiler`](../../../backends/DotNet/build/Inno.Build.Managed.DotNet/Desktop/CoreClrDeploymentCompiler.cs#L10) | Publishes explicitly linked game code with the .NET desktop CoreCLR runtime. | — | — |

### `Inno.Build.Managed.DotNet.MonoWasmDeploymentCompiler`

<!-- api: T:Inno.Build.Managed.DotNet.MonoWasmDeploymentCompiler abde8d30e97fe4bd7aac8a6266c0006b025280dc0f61591712690a969d888c7d -->
<!-- api: M:Inno.Build.Managed.DotNet.MonoWasmDeploymentCompiler.#ctor(System.String,System.Boolean) 497dc8cfa63da9f89478b1cd96cc023c321b28176e71c7a278ccbec888acd312 -->
<!-- api: P:Inno.Build.Managed.DotNet.MonoWasmDeploymentCompiler.id cbcec7fbfa3ab59b5c4e46ece3b85dedcd9e6c3eed6df998ef46af092b39f1c5 -->
<!-- api: P:Inno.Build.Managed.DotNet.MonoWasmDeploymentCompiler.capabilities 489c1cb44973092851846c52e45f8a62e5479cebdf6356a0b245b6e998d411f6 -->
<!-- api: M:Inno.Build.Managed.DotNet.MonoWasmDeploymentCompiler.CompileAsync(Inno.Build.Managed.ManagedDeploymentRequest,System.Threading.CancellationToken) 375c3a3f8efed24fc7119483f6b22a94bf139988c0bd3b6cd7468070172e7209 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1a3575b7a001c68b"></a>[`public Inno.Build.Managed.DotNet.MonoWasmDeploymentCompiler.MonoWasmDeploymentCompiler(string hostPath, bool aheadOfTime)`](../../../backends/DotNet/build/Inno.Build.Managed.DotNet/WebAssembly/MonoWasmDeploymentCompiler.cs#L24) | Selects the project-scoped SDK host and one explicit compilation policy.<br>`hostPath`：The .NET executable whose project resolution selects the matching WebAssembly workload.<br>`aheadOfTime`：Whether managed code is compiled ahead of browser execution. | — | — |
| <a id="api-d31544a639775d37"></a>[`public System.Threading.Tasks.ValueTask<Inno.Build.Managed.ManagedDeploymentResult> Inno.Build.Managed.DotNet.MonoWasmDeploymentCompiler.CompileAsync(Inno.Build.Managed.ManagedDeploymentRequest request, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/DotNet/build/Inno.Build.Managed.DotNet/WebAssembly/MonoWasmDeploymentCompiler.cs#L42) | Compiles or publishes the prepared code closure into isolated managed staging.<br>返回：Verified output evidence; failed or canceled publication never returns a successful result.<br>`request`：The immutable prepared project, target, frozen inputs and staging locations.<br>`cancellationToken`：Cancels the publisher and waits for its process tree and output readers to retire. | — | `System.InvalidOperationException`：The target is unsupported, required toolchain inputs are absent, or publication fails.<br>`System.OperationCanceledException`：Publication was canceled and all owned work has stopped. |
| <a id="api-d0820d7192553b21"></a>[`public Inno.Build.Managed.ManagedDeploymentCapabilities Inno.Build.Managed.DotNet.MonoWasmDeploymentCompiler.capabilities { get; }`](../../../backends/DotNet/build/Inno.Build.Managed.DotNet/WebAssembly/MonoWasmDeploymentCompiler.cs#L39) | Gets the provider's actual target and execution capabilities. | — | — |
| <a id="api-b5a48ed5e854f36e"></a>[`public Inno.Build.Managed.ManagedDeploymentId Inno.Build.Managed.DotNet.MonoWasmDeploymentCompiler.id { get; }`](../../../backends/DotNet/build/Inno.Build.Managed.DotNet/WebAssembly/MonoWasmDeploymentCompiler.cs#L36) | Gets the open stable implementation identity. | — | — |
| <a id="api-0b153a76603c4de3"></a>[`public sealed class Inno.Build.Managed.DotNet.MonoWasmDeploymentCompiler : Inno.Build.Managed.IManagedDeploymentCompiler`](../../../backends/DotNet/build/Inno.Build.Managed.DotNet/WebAssembly/MonoWasmDeploymentCompiler.cs#L10) | Publishes explicitly linked code through the SDK's Mono WebAssembly interpreter or AOT toolchain. | — | — |

### `Inno.Build.Managed.DotNet.NativeAotDeploymentCompiler`

<!-- api: T:Inno.Build.Managed.DotNet.NativeAotDeploymentCompiler 257d2865852a334f2465ed40270829a5a9965331c68a260f89be6ba2d9cbee55 -->
<!-- api: M:Inno.Build.Managed.DotNet.NativeAotDeploymentCompiler.#ctor(System.String) f7861ff79aa6d0cc75aa695eabbb13aa387dfd23152310f145adc1532a5452db -->
<!-- api: P:Inno.Build.Managed.DotNet.NativeAotDeploymentCompiler.id c388e8f40484abb2d7de3944816858def8fef30e6aa2b5017877d840ba19ccf8 -->
<!-- api: P:Inno.Build.Managed.DotNet.NativeAotDeploymentCompiler.capabilities eb97dbc242453653e2caf69bcb6f7d912224684067c496b14f5ea9794ed78b7b -->
<!-- api: M:Inno.Build.Managed.DotNet.NativeAotDeploymentCompiler.CompileAsync(Inno.Build.Managed.ManagedDeploymentRequest,System.Threading.CancellationToken) 657dbf68ced60b7a78379e429f66da11c549c6b5496db4806f8b9016ef853817 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8807bebf0c3f8dc2"></a>[`public Inno.Build.Managed.DotNet.NativeAotDeploymentCompiler.NativeAotDeploymentCompiler(string hostPath)`](../../../backends/DotNet/build/Inno.Build.Managed.DotNet/NativeAot/NativeAotDeploymentCompiler.cs#L20) | Selects the project-scoped .NET SDK host that invokes the installed native compiler toolchain.<br>`hostPath`：The .NET executable selected by the build composition root. | — | — |
| <a id="api-890e635e3095614a"></a>[`public System.Threading.Tasks.ValueTask<Inno.Build.Managed.ManagedDeploymentResult> Inno.Build.Managed.DotNet.NativeAotDeploymentCompiler.CompileAsync(Inno.Build.Managed.ManagedDeploymentRequest request, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/DotNet/build/Inno.Build.Managed.DotNet/NativeAot/NativeAotDeploymentCompiler.cs#L35) | Compiles or publishes the prepared code closure into isolated managed staging.<br>返回：Verified output evidence; failed or canceled publication never returns a successful result.<br>`request`：The immutable prepared project, target, frozen inputs and staging locations.<br>`cancellationToken`：Cancels the publisher and waits for its process tree and output readers to retire. | — | `System.InvalidOperationException`：The target is unsupported, required toolchain inputs are absent, or publication fails.<br>`System.OperationCanceledException`：Publication was canceled and all owned work has stopped. |
| <a id="api-7325ee45590b9641"></a>[`public Inno.Build.Managed.ManagedDeploymentCapabilities Inno.Build.Managed.DotNet.NativeAotDeploymentCompiler.capabilities { get; }`](../../../backends/DotNet/build/Inno.Build.Managed.DotNet/NativeAot/NativeAotDeploymentCompiler.cs#L30) | Gets the provider's actual target and execution capabilities. | — | — |
| <a id="api-c354fbdc53da7a04"></a>[`public Inno.Build.Managed.ManagedDeploymentId Inno.Build.Managed.DotNet.NativeAotDeploymentCompiler.id { get; }`](../../../backends/DotNet/build/Inno.Build.Managed.DotNet/NativeAot/NativeAotDeploymentCompiler.cs#L27) | Gets the open stable implementation identity. | — | — |
| <a id="api-e0f574edd36fed16"></a>[`public sealed class Inno.Build.Managed.DotNet.NativeAotDeploymentCompiler : Inno.Build.Managed.IManagedDeploymentCompiler`](../../../backends/DotNet/build/Inno.Build.Managed.DotNet/NativeAot/NativeAotDeploymentCompiler.cs#L10) | Publishes explicitly linked game code as a native executable through .NET NativeAOT. | — | — |

## 项目依赖

- [Inno.Build.Managed](../../build/Inno.Build.Managed.md)：公开引用边界由实际签名核对。
- [Inno.Build.Toolchains](../../build/Inno.Build.Toolchains.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
