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

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Build.Managed.DotNet.CoreClrDeploymentCompiler`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Managed.DotNet.CoreClrDeploymentCompiler`](../../../backends/DotNet/build/Inno.Build.Managed.DotNet/Desktop/CoreClrDeploymentCompiler.cs#L10) | Publishes explicitly linked game code with the .NET desktop CoreCLR runtime. |
| [`Inno.Build.Managed.DotNet.CoreClrDeploymentCompiler.CoreClrDeploymentCompiler(string hostPath)`](../../../backends/DotNet/build/Inno.Build.Managed.DotNet/Desktop/CoreClrDeploymentCompiler.cs#L20) | Selects the SDK host used for project-scoped publication. |
| [`Inno.Build.Managed.ManagedDeploymentCapabilities Inno.Build.Managed.DotNet.CoreClrDeploymentCompiler.capabilities`](../../../backends/DotNet/build/Inno.Build.Managed.DotNet/Desktop/CoreClrDeploymentCompiler.cs#L30) | See the implemented contract. |
| [`Inno.Build.Managed.ManagedDeploymentId Inno.Build.Managed.DotNet.CoreClrDeploymentCompiler.id`](../../../backends/DotNet/build/Inno.Build.Managed.DotNet/Desktop/CoreClrDeploymentCompiler.cs#L27) | See the implemented contract. |
| [`System.Threading.Tasks.ValueTask<Inno.Build.Managed.ManagedDeploymentResult> Inno.Build.Managed.DotNet.CoreClrDeploymentCompiler.CompileAsync(Inno.Build.Managed.ManagedDeploymentRequest request, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/DotNet/build/Inno.Build.Managed.DotNet/Desktop/CoreClrDeploymentCompiler.cs#L35) | See the implemented contract. |

### `Inno.Build.Managed.DotNet.MonoWasmDeploymentCompiler`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Managed.DotNet.MonoWasmDeploymentCompiler`](../../../backends/DotNet/build/Inno.Build.Managed.DotNet/WebAssembly/MonoWasmDeploymentCompiler.cs#L10) | Publishes explicitly linked code through the SDK's Mono WebAssembly interpreter or AOT toolchain. |
| [`Inno.Build.Managed.DotNet.MonoWasmDeploymentCompiler.MonoWasmDeploymentCompiler(string hostPath, bool aheadOfTime)`](../../../backends/DotNet/build/Inno.Build.Managed.DotNet/WebAssembly/MonoWasmDeploymentCompiler.cs#L24) | Selects the project-scoped SDK host and one explicit compilation policy. |
| [`Inno.Build.Managed.ManagedDeploymentCapabilities Inno.Build.Managed.DotNet.MonoWasmDeploymentCompiler.capabilities`](../../../backends/DotNet/build/Inno.Build.Managed.DotNet/WebAssembly/MonoWasmDeploymentCompiler.cs#L39) | See the implemented contract. |
| [`Inno.Build.Managed.ManagedDeploymentId Inno.Build.Managed.DotNet.MonoWasmDeploymentCompiler.id`](../../../backends/DotNet/build/Inno.Build.Managed.DotNet/WebAssembly/MonoWasmDeploymentCompiler.cs#L36) | See the implemented contract. |
| [`System.Threading.Tasks.ValueTask<Inno.Build.Managed.ManagedDeploymentResult> Inno.Build.Managed.DotNet.MonoWasmDeploymentCompiler.CompileAsync(Inno.Build.Managed.ManagedDeploymentRequest request, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/DotNet/build/Inno.Build.Managed.DotNet/WebAssembly/MonoWasmDeploymentCompiler.cs#L42) | See the implemented contract. |

### `Inno.Build.Managed.DotNet.NativeAotDeploymentCompiler`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Managed.DotNet.NativeAotDeploymentCompiler`](../../../backends/DotNet/build/Inno.Build.Managed.DotNet/NativeAot/NativeAotDeploymentCompiler.cs#L10) | Publishes explicitly linked game code as a native executable through .NET NativeAOT. |
| [`Inno.Build.Managed.DotNet.NativeAotDeploymentCompiler.NativeAotDeploymentCompiler(string hostPath)`](../../../backends/DotNet/build/Inno.Build.Managed.DotNet/NativeAot/NativeAotDeploymentCompiler.cs#L20) | Selects the project-scoped .NET SDK host that invokes the installed native compiler toolchain. |
| [`Inno.Build.Managed.ManagedDeploymentCapabilities Inno.Build.Managed.DotNet.NativeAotDeploymentCompiler.capabilities`](../../../backends/DotNet/build/Inno.Build.Managed.DotNet/NativeAot/NativeAotDeploymentCompiler.cs#L30) | See the implemented contract. |
| [`Inno.Build.Managed.ManagedDeploymentId Inno.Build.Managed.DotNet.NativeAotDeploymentCompiler.id`](../../../backends/DotNet/build/Inno.Build.Managed.DotNet/NativeAot/NativeAotDeploymentCompiler.cs#L27) | See the implemented contract. |
| [`System.Threading.Tasks.ValueTask<Inno.Build.Managed.ManagedDeploymentResult> Inno.Build.Managed.DotNet.NativeAotDeploymentCompiler.CompileAsync(Inno.Build.Managed.ManagedDeploymentRequest request, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/DotNet/build/Inno.Build.Managed.DotNet/NativeAot/NativeAotDeploymentCompiler.cs#L35) | See the implemented contract. |

## 项目依赖

- [Inno.Build.Managed](../../build/Inno.Build.Managed.md)：公开引用边界由实际签名核对。
- [Inno.Build.Toolchains](../../build/Inno.Build.Toolchains.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
