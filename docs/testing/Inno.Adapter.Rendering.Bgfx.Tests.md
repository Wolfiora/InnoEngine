# Inno.Adapter.Rendering.Bgfx.Tests

创建真实 SDL/Cocoa 窗口的 surface 退休用例通过测试程序集自己的主线程入口执行，独立进程协议仍保留。入口只直接调用已声明的原断言，失败和超时传播给 xUnit；Noop 与进程生命周期测试仍使用各自独立 host，不创建生产可执行项目。

[测试索引](README.md) · [Wiki 首页](../README.md) · [当前验收](../architecture/CURRENT_ACCEPTANCE.md)

## 职责与边界

验证以下项目的公开行为与组合契约。该程序集属于测试闭包，不进入 Editor 或 Player 发行；测试类、fixture 和断言辅助类型不属于稳定产品 API。

- [Inno.Adapter.Platform.Sdl3](../backends/Sdl3/Inno.Adapter.Platform.Sdl3.md)
- [Inno.Integration.WindowsX64.Sdl3](../platform/WindowsX64/Inno.Integration.WindowsX64.Sdl3.md)
- [Inno.Integration.MacOSArm64.Sdl3](../platform/MacOSArm64/Inno.Integration.MacOSArm64.Sdl3.md)
- [Inno.Integration.WindowsX64.Bgfx.Runtime](../platform/WindowsX64/Inno.Integration.WindowsX64.Bgfx.Runtime.md)
- [Inno.Integration.MacOSArm64.Bgfx.Runtime](../platform/MacOSArm64/Inno.Integration.MacOSArm64.Bgfx.Runtime.md)
- [Inno.Integration.BrowserWasm32.Bgfx.Runtime](../platform/BrowserWasm32/Inno.Integration.BrowserWasm32.Bgfx.Runtime.md)
- [Inno.Extensibility.Modules](../extensibility/Inno.Extensibility.Modules.md)
- [Inno.Build.Toolchains.Bgfx.Tools](../backends/Bgfx/Inno.Build.Toolchains.Bgfx.Tools.md)
- [Inno.Adapter.Rendering.Bgfx](../backends/Bgfx/Inno.Adapter.Rendering.Bgfx.md)
- [Inno.Adapter.Presentation.ImGui.Bgfx](../backends/ImGui/Inno.Adapter.Presentation.ImGui.Bgfx.md)
- [Inno.Adapter.Modules.DotNet](../backends/DotNet/Inno.Adapter.Modules.DotNet.md)
- [Inno.Adapter.Serialization.DotNet](../backends/DotNet/Inno.Adapter.Serialization.DotNet.md)
- [Inno.Rendering.Assets](../rendering/Inno.Rendering.Assets.md)
- [Inno.Rendering.Assets.Authoring](../rendering/Inno.Rendering.Assets.Authoring.md)

## 运行

从仓库根目录运行：

```powershell
$productProps = Join-Path $PWD 'platforms/WindowsX64/build/Inno.Build.WindowsX64/EditorProduct.props'
dotnet test backends/Bgfx/tests/Inno.Adapter.Rendering.Bgfx.Tests/Inno.Adapter.Rendering.Bgfx.Tests.csproj --configuration Debug "-p:InnoProductBuildProperties=$productProps" -p:InnoNativeTarget=windows-x64 -p:InnoToolTarget=windows-x64 -p:InnoTestNativeProduct=editor -p:InnoTestNativeTarget=windows-x64 --filter 'ProcessIsolation!=Required' --disable-build-servers -m:1 -nodeReuse:false
$tests = 'backends/Bgfx/tests/Inno.Adapter.Rendering.Bgfx.Tests/bin/Debug/net9.0/Inno.Adapter.Rendering.Bgfx.Tests.dll'
dotnet test $tests --filter 'FullyQualifiedName~BgfxDeviceTests'
dotnet test $tests --filter 'FullyQualifiedName~MultiThreadedSurfaceAcknowledgesDestructionAfterTheFollowingSubmission'
dotnet test $tests --filter 'FullyQualifiedName~InlineSurfaceUsesTheSameConservativeAcknowledgment'
dotnet test $tests --filter 'FullyQualifiedName~InvalidIntegrationFailsBeforeTakingTheProcessOwner'
```

包含 Native 或窗口相关消费；执行前准备匹配目标与配置的原生产物。缺库、缺 SDK 或无法建立真实窗口属于未通过/未执行，不能记为通过。

可以用 `--filter FullyQualifiedName~方法名` 选择下表的场景。完整验证包含上面的所有进程：单线程 BGFX 的进程状态不能在退出设备后重新初始化，必须保持该真实约束。产品 props 同时选择嵌入 Shader 与绑定目标；中立 SourceDebug Adapter 不具备窗口产品的目标 Shader，不能用于替代这一部署。

## 源码场景

下表是当前测试文件索引，并列举各文件的部分入口，完整断言与参数化用例位于链接源码。列出场景不表示本轮已执行；结果只在当前验收中登记。

| 测试文件 | 场景入口示例 |
| --- | --- |
| [BgfxCompositionProgramProviderTests.cs](../../backends/Bgfx/tests/Inno.Adapter.Rendering.Bgfx.Tests/BgfxCompositionProgramProviderTests.cs) | `EmbeddedTargetProgramMatchesTheDeviceBackendAndFullscreenLayout` |
| [BgfxDeviceTests.cs](../../backends/Bgfx/tests/Inno.Adapter.Rendering.Bgfx.Tests/BgfxDeviceTests.cs) | `NoopDevice_ExecutesOneGraphAndSubmitsOneFrame`、`NoopDevice_ResetsPublicCommandCountersPerFrame`、`CreateWindowSurface_RejectsUnsupportedPlatformHandlesBeforeNativeCreation`、`NoopDevice_CreatesAndDefersPersistentTextureDestruction` |
| [BgfxImGuiColorContractTests.cs](../../backends/Bgfx/tests/Inno.Adapter.Rendering.Bgfx.Tests/BgfxImGuiColorContractTests.cs) | `DefaultBackbufferAndImGuiShaderApplyExactlyOneSrgbEncoding`、`DistributedImGuiGraphLoadsTextureAndOutputTransferBindings` |
| [BgfxShaderProfileTests.cs](../../backends/Bgfx/tests/Inno.Adapter.Rendering.Bgfx.Tests/BgfxShaderProfileTests.cs) | `NewTargetAndRendererUseThePublicConfigurationBoundary`、`DialectDefinesAndCapabilitiesAllParticipateInIdentity`、`InvalidAndDuplicateProfilesFailBeforeCreatingACompiler` |
| [BgfxSurfaceIntegrationTests.cs](../../backends/Bgfx/tests/Inno.Adapter.Rendering.Bgfx.Tests/BgfxSurfaceIntegrationTests.cs) | `EachIntegrationValidatesItsOwnAbiAndRole`、`ProvidersRequireAnExplicitBorrowedIntegration` |
| [BgfxSurfaceRetirementTests.cs](../../backends/Bgfx/tests/Inno.Adapter.Rendering.Bgfx.Tests/BgfxSurfaceRetirementTests.cs) | `MultiThreadedSurfaceAcknowledgesDestructionAfterTheFollowingSubmission`、`InlineSurfaceUsesTheSameConservativeAcknowledgment`、`InvalidIntegrationFailsBeforeTakingTheProcessOwner` |
| [BgfxToolchainTests.cs](../../backends/Bgfx/tests/Inno.Adapter.Rendering.Bgfx.Tests/BgfxToolchainTests.cs) | `MetalProfileIsOwnedByMacBgfxToolchain`、`Direct3DAndMetalTargetsSelectProfilesWithoutForkingShaderSource`、`BrowserProfileRequiresWebGlWithoutCompute`、`BrowserProfileCompilesTheCommonGraphOnWindows` |
| [ToolRunnerTests.cs](../../backends/Bgfx/tests/Inno.Adapter.Rendering.Bgfx.Tests/ToolRunnerTests.cs) | `Run_UsesExplicitlyDeployedTools`、`Run_Throws_WhenToolMissing` |

## 生命周期与失败

测试通过公开入口及可替换契约检查行为，fixture 只负责自身创建的会话、临时目录和子进程。需要窗口、设备或 owner thread 的测试应保持其声明的执行线程与释放顺序。取消或失败后检查相应断言与清理结果，不通过删除共享锁文件解除等待。

## 扩展与维护

新增行为用例放在最接近所属契约的测试文件。测试不能依赖反射穿透、`InternalsVisibleTo` 或扩大的生产可见性。修改契约后同时核对本项目及其直接生产依赖页面；测试中的具体类型不加入游戏脚本导出清单。

## 相邻页面

- [项目文件](../../backends/Bgfx/tests/Inno.Adapter.Rendering.Bgfx.Tests/Inno.Adapter.Rendering.Bgfx.Tests.csproj)：框架、引用和实际构建配置。
- [架构索引](../architecture/README.md)：所属层及禁止依赖。
- [当前验收](../architecture/CURRENT_ACCEPTANCE.md)：实际运行状态与环境限制。

## 明确宿主的 Native 验证

WindowsX64 与 MacOSArm64 窗口 fixture 分别使用真实 Win32/Cocoa 集成，不借用另一个平台的 surface。CI 通过 `tests/Invoke-IsolatedTests.ps1` 的 `Target` 和 `ProductProperties` 参数选择产品闭包；`ProcessIsolation=Required` 用例分别运行，避免污染第三方进程级状态。未准备产品时的缺库不是通过证据。
