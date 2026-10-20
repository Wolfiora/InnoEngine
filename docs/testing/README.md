# 测试与验证

[Wiki 首页](../README.md) · [当前验收](../architecture/CURRENT_ACCEPTANCE.md)

按项目查找验证入口、生产依赖和环境要求。测试项目页面描述源码场景；实际运行结果、失败及未执行项目由当前验收统一记录。Native、浏览器、窗口和真实设备测试各有环境前提，托管编译通过不能替代运行。

## 项目索引

- [Inno.Scripting.Compiler.Tests](Inno.Scripting.Compiler.Tests.md)
- [Inno.Testing.Processes](Inno.Testing.Processes.md)
- [Inno.Testing.Source](Inno.Testing.Source.md)
- [Inno.Testing.Processes.Tests](Inno.Testing.Processes.Tests.md)

- [Inno.Adapter.Rendering.Bgfx.Tests](Inno.Adapter.Rendering.Bgfx.Tests.md)
- [Inno.Native.Bgfx.Tests](Inno.Native.Bgfx.Tests.md)
- [Inno.Adapter.Content.FileSystem.Tests](Inno.Adapter.Content.FileSystem.Tests.md)
- [Inno.Native.ImGui.Tests](Inno.Native.ImGui.Tests.md)
- [Inno.Native.ImGuizmo.Tests](Inno.Native.ImGuizmo.Tests.md)
- [Inno.Adapter.Audio.MiniAudio.Tests](Inno.Adapter.Audio.MiniAudio.Tests.md)
- [Inno.Native.MiniAudio.Tests](Inno.Native.MiniAudio.Tests.md)
- [Inno.Adapter.Platform.Sdl3.Tests](Inno.Adapter.Platform.Sdl3.Tests.md)
- [Inno.Native.Sdl3.Tests](Inno.Native.Sdl3.Tests.md)
- [Inno.Animation.Tests](Inno.Animation.Tests.md)
- [Inno.Assets.Pipeline.Tests](Inno.Assets.Pipeline.Tests.md)
- [Inno.Assets.Tests](Inno.Assets.Tests.md)
- [Inno.Assets.Pipeline.DuplicateImporterFixture](Inno.Assets.Pipeline.DuplicateImporterFixture.md)
- [Inno.Audio.Assets.Tests](Inno.Audio.Assets.Tests.md)
- [Inno.Audio.Runtime.Tests](Inno.Audio.Runtime.Tests.md)
- [Inno.Audio.Tests](Inno.Audio.Tests.md)
- [Inno.Build.Tests](Inno.Build.Tests.md)
- [Inno.Build.Toolchains.Tests](Inno.Build.Toolchains.Tests.md)
- [Inno.Content.Tests](Inno.Content.Tests.md)
- [Inno.Content.CacheProbe](Inno.Content.CacheProbe.md)
- [Inno.Content.Testing](Inno.Content.Testing.md)
- [Inno.Core.Collections.Tests](Inno.Core.Collections.Tests.md)
- [Inno.Core.Coroutines.Tests](Inno.Core.Coroutines.Tests.md)
- [Inno.Core.Diagnostics.Tests](Inno.Core.Diagnostics.Tests.md)
- [Inno.Core.Events.Tests](Inno.Core.Events.Tests.md)
- [Inno.Core.Execution.Tests](Inno.Core.Execution.Tests.md)
- [Inno.Core.Graphs.Tests](Inno.Core.Graphs.Tests.md)
- [Inno.Core.IO.Tests](Inno.Core.IO.Tests.md)
- [Inno.Core.Identity.Tests](Inno.Core.Identity.Tests.md)
- [Inno.Core.Jobs.Tests](Inno.Core.Jobs.Tests.md)
- [Inno.Core.Layers.Tests](Inno.Core.Layers.Tests.md)
- [Inno.Core.Logging.Tests](Inno.Core.Logging.Tests.md)
- [Inno.Core.Mathematics.Tests](Inno.Core.Mathematics.Tests.md)
- [Inno.Core.Serialization.Tests](Inno.Core.Serialization.Tests.md)
- [Inno.Editor.Audio.Tests](Inno.Editor.Audio.Tests.md)
- [Inno.Editor.Graph.Tests](Inno.Editor.Graph.Tests.md)
- [Inno.Editor.Hosting.Tests](Inno.Editor.Hosting.Tests.md)
- [Inno.Editor.ImGui.Tests](Inno.Editor.ImGui.Tests.md)
- [Inno.Editor.Inspection.Tests](Inno.Editor.Inspection.Tests.md)
- [Inno.Editor.Interactions.Tests](Inno.Editor.Interactions.Tests.md)
- [Inno.Editor.PlayMode.Tests](Inno.Editor.PlayMode.Tests.md)
- [Inno.Extensibility.Modules.Tests](Inno.Extensibility.Modules.Tests.md)
- [Inno.Extensibility.Reload.Tests](Inno.Extensibility.Reload.Tests.md)
- [Inno.Extensibility.Types.Tests](Inno.Extensibility.Types.Tests.md)
- [Inno.Extensibility.Modules.TestDependency](Inno.Extensibility.Modules.TestDependency.md)
- [Inno.Extensibility.Modules.TestModule.Invalid](Inno.Extensibility.Modules.TestModule.Invalid.md)
- [Inno.Extensibility.Modules.TestModule.V1](Inno.Extensibility.Modules.TestModule.V1.md)
- [Inno.Extensibility.Modules.TestModule.V2](Inno.Extensibility.Modules.TestModule.V2.md)
- [Inno.Extensibility.Types.TestAssemblyA](Inno.Extensibility.Types.TestAssemblyA.md)
- [Inno.Extensibility.Types.TestAssemblyB](Inno.Extensibility.Types.TestAssemblyB.md)
- [Inno.Input.Tests](Inno.Input.Tests.md)
- [Inno.Plugins.Tests](Inno.Plugins.Tests.md)
- [Inno.References.Tests](Inno.References.Tests.md)
- [Inno.Rendering.Assets.Authoring.Tests](Inno.Rendering.Assets.Authoring.Tests.md)
- [Inno.Rendering.Assets.Tests](Inno.Rendering.Assets.Tests.md)
- [Inno.Rendering.Runtime.Tests](Inno.Rendering.Runtime.Tests.md)
- [Inno.Rendering.Shaders.Tests](../rendering/Inno.Rendering.Shaders.Tests.md)
- [Inno.Rendering.Tests](Inno.Rendering.Tests.md)
- [Inno.Rendering.Runtime.Reload.TestModule](Inno.Rendering.Runtime.Reload.TestModule.md)
- [Inno.Runtime.Generators.Tests](Inno.Runtime.Generators.Tests.md)
- [Inno.Runtime.Tests](Inno.Runtime.Tests.md)
- [Inno.Scene.Tests](Inno.Scene.Tests.md)
- [Inno.Scene.Reload.TestModule](Inno.Scene.Reload.TestModule.md)
- [Inno.Editor.Scripting.Tests](Inno.Editor.Scripting.Tests.md)
- [Inno.Storage.Tests](Inno.Storage.Tests.md)
- [Inno.Text.Tests](../text/Inno.Text.Tests.md)
- [Inno.Tooling.Architecture.Tests](../tooling/Inno.Tooling.Architecture.Tests.md)
- [Inno.UI.Tests](../ui/Inno.UI.Tests.md)

## Native 进程隔离

`tests/Invoke-IsolatedTests.ps1` 先执行普通套件，再按 `ProcessIsolation=Required` 的发现结果为每个用例启动独立 testhost。BGFX 线程模式和故意保留 Faulted owner 的用例不能与其他 Native 生命周期共用进程。发现、执行和失败均使用真实 SDK 测试入口；不改写生产状态来复用 testhost。

Native 测试部署在成功 Build 后执行；CI 的目标 Build 明确设置 InnoTestNativeProduct / InnoTestNativeTarget。--no-build 直接消费已准备程序集，不提前 ResolveReferences，不执行重复 Native 准备。

## .NET 10 托管检查

四仓库使用 `global.json` 的 SDK 10.0.402（`latestPatch`，禁止预览）和 C# 14，保留 xUnit/VSTest。执行 `./tests/Invoke-ManagedChecks.ps1` 会按 MSBuild 实际求值的引用图选出不需要绑定生成或原生产品准备的工程，构建后运行普通及进程隔离测试。`-InventoryOnly` 只生成清单。日志、SDK 信息、TRX 和明确依赖链保存在 `artifacts/acceptance/managed/`。

`Invoke-IsolatedTests.ps1 -Filter` 将调用者明确选择的同一筛选同时用于普通套件、隔离发现及隔离执行。正式完整验收包含 BGCS，用例分类不作为默认排除条件。独立托管检查只覆盖其实际选中的工程；完整原生产品准备和测试使用正常生产入口，未选中的消费工程不能计为通过。

使用 SDK `-p:ArtifactsPath=<owned-directory>` 构建的独立验收闭包，通过 `Invoke-IsolatedTests.ps1 -ArtifactsPath <same-directory>` 执行。普通执行与隔离发现使用同一输出选择，隔离用例直接运行实际求值所得程序集。不同活动构建分别拥有各自的 SDK 输出目录。

需要真实源码 checkout 的测试由自己的 SDK 项目显式导入 `tests/RepositoryInput.targets`，通过 `Inno.Testing.RepositoryRoot` assembly metadata 声明输入根，并引用唯一的 [Inno.Testing.Source](Inno.Testing.Source.md) 读取器程序集。读取器只检查公开 metadata 和仓库标记，不从独立输出目录猜测源码位置，也不修改生产工具的定位或进程当前目录。该规则不进入生产项目或发行闭包。

`BgfxShutdownTests.ShutdownAfterAnOffscreenGraphReleasesTheProcessOwner` 使用真实窗口、离屏 Graph 与设备所有资源，验证关闭后的进程 owner 可再次取得且借用窗口仍有效。默认使用当前测试宿主的 D3D11 / Metal；显式设置 `INNO_TEST_GRAPHICS_API=Vulkan` 或 `Direct3D12` 可在对应设备单独执行同一回归。该 ID 必须与实际能力一致，不提供失败后切换后端的 fallback。

脚本测试覆盖 C# 14 的 `field`、空条件赋值、Span、开放泛型 `nameof`、推断类型 Lambda 参数修饰符，以及跨插件扩展成员的引擎/IDE 一致性；另验证 framework reference 内容变化和 SDK target 生成符号的缓存失效。
