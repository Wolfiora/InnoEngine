# Inno.Build.Windows.Host

[分类索引](README.md) · [Wiki 首页](../../README.md) · [职责与文件树](../../architecture/NATIVE_BUILD_PLATFORM_OWNERSHIP.md)

## 职责、依赖与生命周期

提供 Windows 构建宿主能力，不选择游戏发布目标、不引用 WindowsX64 产品或 Browser。`WindowsBuildHostProvider.TryCapture` 只在支持的当前宿主返回 descriptor；`WindowsToolchainWorkspaceProvider.OpenAsync` 为物理 owner 取得稳定 junction。

执行路径必须持有到全部工具进程结束。Dispose 只删除验证过目标的 junction，保留物理数据；叶锁仅在 owner 独占权内退休。取消释放本次等待，不干扰其他 owner。其他平台通过中立接口消费，不引用本项目。

## 使用示例

```csharp
using Inno.Build.Toolchains;
using Inno.Build.Windows.Host;

static BuildHostDescriptor? CaptureWindows() => new WindowsBuildHostProvider().TryCapture();
```

示例借用已预检的宿主和发行上下文；实际执行需要明确工具链、绑定生成器与完整源码输入。失败、取消和不可用能力不发布部分结果。此构建协议不进入游戏脚本导出，也不保存 collectible generation 状态。

## 当前源码公开 API 清单

当前声明由源码语义清单核对，包含 public 和可供外部派生实现使用的 protected 成员。构建契约不作为游戏脚本导出；线程、失败与所有权要求见前文及每项 XML。

### `Inno.Build.Windows.Host.WindowsBuildHostProvider`

<!-- api: T:Inno.Build.Windows.Host.WindowsBuildHostProvider e3ccd7d16c85eb9436baf315b9f8f82bcef80feb8835c09bc0a521e2d6f71a01 -->
<!-- api: M:Inno.Build.Windows.Host.WindowsBuildHostProvider.TryCapture c5005bdd7d29850caccec167b45e6016c1aa2bff157f3f935229deeacfb1aee5 -->
<!-- api: M:Inno.Build.Windows.Host.WindowsBuildHostProvider.#ctor 1d7a2df2204e1c99ac2c3aef2dd3a4b60e13283ea8b59c438664f4b84560988d -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-0ede06c7e9a152d1"></a>[`public Inno.Build.Windows.Host.WindowsBuildHostProvider.WindowsBuildHostProvider()`](../../../platforms/Windows/build/Inno.Build.Windows.Host/WindowsBuildHostProvider.cs#L10) | 由 C# 生成，遵循所属类型的值语义与本页的生命周期约束。 |
| <a id="api-6c3527025262e5c3"></a>[`public Inno.Build.Toolchains.BuildHostDescriptor? Inno.Build.Windows.Host.WindowsBuildHostProvider.TryCapture()`](../../../platforms/Windows/build/Inno.Build.Windows.Host/WindowsBuildHostProvider.cs#L13) | Captures the current execution machine when this provider supports it.<br>返回：The immutable host, or null when the current machine is outside this provider's capabilities. |
| <a id="api-d3ec2a781e4d301c"></a>[`public sealed class Inno.Build.Windows.Host.WindowsBuildHostProvider : Inno.Build.Toolchains.IBuildHostProvider`](../../../platforms/Windows/build/Inno.Build.Windows.Host/WindowsBuildHostProvider.cs#L10) | Contributes Windows process and filesystem execution capabilities. |

### `Inno.Build.Windows.Host.WindowsToolchainWorkspaceProvider`

<!-- api: T:Inno.Build.Windows.Host.WindowsToolchainWorkspaceProvider 8779f5339a54243fc3cf3a5feb7acb1f44caa5c807caa793e1ba4b64b88cd8b8 -->
<!-- api: P:Inno.Build.Windows.Host.WindowsToolchainWorkspaceProvider.identity 20b16bd7657068a51806a5b72ede2ad77f5744c991eac9db514087b82b3ba325 -->
<!-- api: M:Inno.Build.Windows.Host.WindowsToolchainWorkspaceProvider.OpenAsync(System.String,System.Threading.CancellationToken) eac9b42a50127a7448b2e7a9ad63fded9f3b04081f31356aff823e9a3481c40a -->
<!-- api: M:Inno.Build.Windows.Host.WindowsToolchainWorkspaceProvider.#ctor 626e0212d68374ce31806e625429254525fb8eb3620e1758365a9c0ddb7363d9 -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-f3e73bd08c800fd9"></a>[`public Inno.Build.Windows.Host.WindowsToolchainWorkspaceProvider.WindowsToolchainWorkspaceProvider()`](../../../platforms/Windows/build/Inno.Build.Windows.Host/Workspace/WindowsToolchainWorkspaceProvider.cs#L15) | 由 C# 生成，遵循所属类型的值语义与本页的生命周期约束。 |
| <a id="api-3cabe936462c5980"></a>[`public System.Threading.Tasks.ValueTask<Inno.Build.Toolchains.ToolchainWorkingDirectory> Inno.Build.Windows.Host.WindowsToolchainWorkspaceProvider.OpenAsync(string physicalPath, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../platforms/Windows/build/Inno.Build.Windows.Host/Workspace/WindowsToolchainWorkspaceProvider.cs#L21) | Acquires an execution path for an absolute build-owned directory.<br>`physicalPath`：The retained physical directory.<br>`cancellationToken`：Cancellation while waiting for exclusive alias ownership.<br>返回：A scoped path whose processes must finish before disposal. |
| <a id="api-9293065eec2dcf40"></a>[`public string Inno.Build.Windows.Host.WindowsToolchainWorkspaceProvider.identity { get; }`](../../../platforms/Windows/build/Inno.Build.Windows.Host/Workspace/WindowsToolchainWorkspaceProvider.cs#L18) | Gets the stable execution namespace included in compiler workspace identity. |
| <a id="api-a33b0037c9479398"></a>[`public sealed class Inno.Build.Windows.Host.WindowsToolchainWorkspaceProvider : Inno.Build.Toolchains.IToolchainWorkspaceProvider`](../../../platforms/Windows/build/Inno.Build.Windows.Host/Workspace/WindowsToolchainWorkspaceProvider.cs#L15) | Owns stable bounded Windows junctions for native compiler workspaces. |
