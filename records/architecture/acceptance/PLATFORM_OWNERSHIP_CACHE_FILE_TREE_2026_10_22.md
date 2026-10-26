# 平台职责与缓存治理交付文件树

[验收报告](PLATFORM_OWNERSHIP_CACHE_ACCEPTANCE_2026_10_22.md) · [职责与依赖](../../../docs/architecture/NATIVE_BUILD_PLATFORM_OWNERSHIP.md)

下列为本轮新增或修改且仍存在的手写源码、配置、生成结果与文档位置。未改业务文件继续归属于原 owner；包括本轮锁文件和记录页。生成结果仅由正式 BGCS 流程更新。

```text
InnoEngine/
├── .github/
│   └── workflows/
│       └── rendering-ci.yml
├── AGENTS.md
├── Directory.Build.props
├── Directory.Build.targets
├── InnoEngine.sln
├── README.md
├── backends/
│   ├── Bgfx/
│   │   ├── build/
│   │   │   ├── Inno.Build.Toolchains.Bgfx.Shaders/
│   │   │   │   ├── packages.win-x64.lock.json
│   │   │   │   └── packages.windows-x64.win-x64.lock.json
│   │   │   ├── Inno.Build.Toolchains.Bgfx.Tools/
│   │   │   │   ├── Execution/
│   │   │   │   │   └── ToolRunner.cs
│   │   │   │   ├── Inno.Build.Toolchains.Bgfx.Tools.csproj
│   │   │   │   ├── packages.win-x64.lock.json
│   │   │   │   └── packages.windows-x64.win-x64.lock.json
│   │   │   └── Inno.Build.Toolchains.Bgfx/
│   │   │       ├── BgfxBuildSession.cs
│   │   │       ├── BgfxNativeBuild.cs
│   │   │       ├── packages.win-x64.lock.json
│   │   │       └── packages.windows-x64.win-x64.lock.json
│   │   └── native/
│   │       └── Inno.Native.Bgfx/
│   │           ├── Bindings/
│   │           │   ├── bindgen.json
│   │           │   └── bindings.props
│   │           ├── Native/
│   │           │   └── Generated/
│   │           │       └── compilation.json
│   │           └── packages.win-x64.lock.json
│   ├── DotNet/
│   │   ├── build/
│   │   │   └── Inno.Build.Managed.DotNet/
│   │   │       ├── Desktop/
│   │   │       │   └── CoreClrDeploymentCompiler.cs
│   │   │       ├── NativeAot/
│   │   │       │   └── NativeAotDeploymentCompiler.cs
│   │   │       ├── Publishing/
│   │   │       │   ├── DotNetDeploymentPublisher.cs
│   │   │       │   ├── DotNetPublishExecutor.cs
│   │   │       │   └── DotNetPublishRequest.cs
│   │   │       ├── WebAssembly/
│   │   │       │   └── MonoWasmDeploymentCompiler.cs
│   │   │       ├── packages.win-x64.lock.json
│   │   │       └── packages.windows-x64.win-x64.lock.json
│   │   └── runtime/
│   │       ├── Inno.Adapter.Modules.DotNet/
│   │       │   ├── packages.win-x64.lock.json
│   │       │   └── packages.windows-x64.win-x64.lock.json
│   │       └── Inno.Adapter.Serialization.DotNet/
│   │           ├── packages.win-x64.lock.json
│   │           └── packages.windows-x64.win-x64.lock.json
│   ├── ImGui/
│   │   ├── build/
│   │   │   ├── Inno.Build.Toolchains.ImGui/
│   │   │   │   ├── CMakeLists.txt
│   │   │   │   ├── ImGuiToolchain.cs
│   │   │   │   ├── packages.win-x64.lock.json
│   │   │   │   └── packages.windows-x64.win-x64.lock.json
│   │   │   └── Inno.Build.Toolchains.ImGuizmo/
│   │   │       ├── CMakeLists.txt
│   │   │       ├── ImGuizmoToolchain.cs
│   │   │       ├── packages.win-x64.lock.json
│   │   │       └── packages.windows-x64.win-x64.lock.json
│   │   ├── native/
│   │   │   ├── Inno.Native.ImGui/
│   │   │   │   ├── Bindings/
│   │   │   │   │   ├── bindgen.json
│   │   │   │   │   └── bindings.props
│   │   │   │   ├── Native/
│   │   │   │   │   └── Generated/
│   │   │   │   │       └── compilation.json
│   │   │   │   ├── packages.win-x64.lock.json
│   │   │   │   └── packages.windows-x64.win-x64.lock.json
│   │   │   └── Inno.Native.ImGuizmo/
│   │   │       ├── Bindings/
│   │   │       │   ├── bindgen.json
│   │   │       │   └── bindings.props
│   │   │       └── Native/
│   │   │           └── Generated/
│   │   │               └── compilation.json
│   │   └── runtime/
│   │       └── Inno.Adapter.Presentation.ImGui.Sdl3/
│   │           ├── packages.win-x64.lock.json
│   │           └── packages.windows-x64.win-x64.lock.json
│   ├── Interop/
│   │   └── native/
│   │       └── Inno.Native.LibraryLoading/
│   │           └── packages.win-x64.lock.json
│   ├── MiniAudio/
│   │   ├── build/
│   │   │   └── Inno.Build.Toolchains.MiniAudio/
│   │   │       ├── MiniAudioToolchain.cs
│   │   │       ├── packages.win-x64.lock.json
│   │   │       └── packages.windows-x64.win-x64.lock.json
│   │   └── native/
│   │       └── Inno.Native.MiniAudio/
│   │           ├── Bindings/
│   │           │   ├── bindgen.json
│   │           │   └── bindings.props
│   │           └── Native/
│   │               └── Generated/
│   │                   └── compilation.json
│   ├── RmlUi/
│   │   ├── build/
│   │   │   └── Inno.Build.Toolchains.UI/
│   │   │       ├── UiToolchain.cs
│   │   │       ├── packages.win-x64.lock.json
│   │   │       └── packages.windows-x64.win-x64.lock.json
│   │   ├── native/
│   │   │   └── Inno.Native.UI/
│   │   │       ├── Bindings/
│   │   │       │   ├── bindgen.json
│   │   │       │   └── bindings.props
│   │   │       ├── Native/
│   │   │       │   └── Generated/
│   │   │       │       └── compilation.json
│   │   │       └── packages.win-x64.lock.json
│   │   └── runtime/
│   │       ├── Inno.Adapter.UI.RmlUi.Authoring/
│   │       │   ├── packages.win-x64.lock.json
│   │       │   └── packages.windows-x64.win-x64.lock.json
│   │       └── Inno.Adapter.UI.RmlUi/
│   │           └── packages.win-x64.lock.json
│   ├── Sdl3/
│   │   ├── build/
│   │   │   └── Inno.Build.Toolchains.Sdl3/
│   │   │       ├── Sdl3Toolchain.cs
│   │   │       ├── packages.win-x64.lock.json
│   │   │       └── packages.windows-x64.win-x64.lock.json
│   │   ├── native/
│   │   │   └── Inno.Native.Sdl3/
│   │   │       ├── Bindings/
│   │   │       │   ├── bindgen.json
│   │   │       │   └── bindings.props
│   │   │       ├── Native/
│   │   │       │   └── Generated/
│   │   │       │       └── compilation.json
│   │   │       └── packages.win-x64.lock.json
│   │   └── runtime/
│   │       └── Inno.Adapter.Platform.Sdl3/
│   │           └── packages.win-x64.lock.json
│   └── Text/
│       ├── build/
│       │   └── Inno.Build.Toolchains.Text/
│       │       ├── TextToolchain.cs
│       │       ├── packages.win-x64.lock.json
│       │       └── packages.windows-x64.win-x64.lock.json
│       └── native/
│           └── Inno.Native.Text/
│               ├── Bindings/
│               │   ├── bindgen.json
│               │   └── bindings.props
│               └── Native/
│                   └── Generated/
│                       └── compilation.json
├── build/
│   ├── bindings/
│   │   └── Inno.Build.Bindings/
│   │       ├── Configuration/
│   │       │   ├── BindingDefinitionReader.cs
│   │       │   ├── BindingGenerationConfiguration.cs
│   │       │   └── NativeBindingProfile.cs
│   │       ├── Generation/
│   │       │   ├── BindingGenerationOperation.cs
│   │       │   └── TargetBindingGeneration.cs
│   │       ├── Identity/
│   │       │   ├── BindingInputPlanning.cs
│   │       │   └── NativeBindingGenerationIdentity.cs
│   │       ├── Inno.Build.Bindings.csproj
│   │       ├── NativeBindingGenerator.Inputs.cs
│   │       ├── NativeBindingGenerator.cs
│   │       ├── Output/
│   │       │   └── BindingGenerationDiagnostics.cs
│   │       ├── packages.browser-wasm.win-x64.lock.json
│   │       └── packages.macos-arm64.win-x64.lock.json
│   ├── cli/
│   │   └── Inno.Build.Cli/
│   │       ├── BuildComposition.cs
│   │       ├── Commands/
│   │       │   ├── BindingCommand.cs
│   │       │   ├── CleanCommand.cs
│   │       │   ├── EditorBuildCommand.cs
│   │       │   ├── ShaderCommand.cs
│   │       │   └── SupportBuildCommand.cs
│   │       ├── Inno.Build.Cli.csproj
│   │       ├── NativeBindingsVerification.cs
│   │       ├── NativeVerificationOptions.cs
│   │       ├── ProjectBuildWorkflow.cs
│   │       ├── packages.browser-wasm.win-x64.lock.json
│   │       ├── packages.macos-arm64.win-x64.lock.json
│   │       ├── packages.win-x64.lock.json
│   │       └── packages.windows-x64.win-x64.lock.json
│   ├── composition/
│   │   └── Inno.Build.Composition/
│   │       ├── BuildCompositionContext.cs
│   │       ├── BuildDistribution.cs
│   │       ├── BuildPlatformContribution.cs
│   │       ├── NativeProductContribution.cs
│   │       ├── NativeToolchainContribution.cs
│   │       ├── packages.win-x64.lock.json
│   │       └── packages.windows-x64.win-x64.lock.json
│   ├── distributions/
│   │   └── Inno.Build.Distribution.Standard/
│   │       ├── Inno.Build.Distribution.Standard.csproj
│   │       ├── StandardBuildCaches.cs
│   │       ├── StandardBuildDistribution.cs
│   │       ├── StandardBuildEnvironment.cs
│   │       ├── StandardBuildHosts.cs
│   │       ├── StandardDevelopmentProducts.props
│   │       ├── packages.browser-wasm.lock.json
│   │       ├── packages.lock.json
│   │       ├── packages.macos-arm64.lock.json
│   │       ├── packages.win-x64.lock.json
│   │       ├── packages.windows-x64.lock.json
│   │       └── packages.windows-x64.win-x64.lock.json
│   ├── managed/
│   │   └── Inno.Build.Managed/
│   │       ├── packages.win-x64.lock.json
│   │       └── packages.windows-x64.win-x64.lock.json
│   ├── msbuild/
│   │   └── Inno.Build.Tasks.targets
│   ├── pipeline/
│   │   └── Inno.Build/
│   │       ├── BuildTargetId.cs
│   │       ├── Player/
│   │       │   └── PlayerSupportPackCatalog.cs
│   │       ├── packages.win-x64.lock.json
│   │       └── packages.windows-x64.win-x64.lock.json
│   ├── products/
│   │   └── Inno.Build.Products.Standard/
│   │       ├── Inno.Build.Products.Standard.csproj
│   │       ├── StandardNativeComponents.cs
│   │       ├── StandardNativeDeployment.cs
│   │       ├── StandardSupportPackRequirements.cs
│   │       ├── packages.browser-wasm.lock.json
│   │       ├── packages.lock.json
│   │       ├── packages.macos-arm64.lock.json
│   │       ├── packages.win-x64.lock.json
│   │       ├── packages.windows-x64.lock.json
│   │       └── packages.windows-x64.win-x64.lock.json
│   ├── support/
│   │   └── Inno.Build.SupportPacks.Core/
│   │       ├── FilePlayerSupportPackPreparation.cs
│   │       ├── PlayerSupportPackPreparationIdentity.cs
│   │       ├── PlayerSupportPackPreparationIndex.cs
│   │       ├── packages.win-x64.lock.json
│   │       └── packages.windows-x64.win-x64.lock.json
│   ├── tasks/
│   │   ├── Inno.Build.TaskHosting/
│   │   │   ├── BuildTaskRuntimeTask.cs
│   │   │   ├── MaterializeBindingsTask.cs
│   │   │   ├── PublishTaskHostTask.cs
│   │   │   └── TaskRuntimePreparation.cs
│   │   └── Inno.Build.Tasks/
│   │       ├── CompileShaderTask.cs
│   │       ├── GenerateBindingsTask.cs
│   │       ├── MaintainBuildCachesTask.cs
│   │       ├── PrepareProductNativeTask.cs
│   │       ├── PublishSupportPackTask.cs
│   │       ├── packages.browser-wasm.win-x64.lock.json
│   │       ├── packages.macos-arm64.win-x64.lock.json
│   │       ├── packages.win-x64.lock.json
│   │       └── packages.windows-x64.win-x64.lock.json
│   └── toolchains/
│       └── Inno.Build.Toolchains/
│           ├── Cache/
│           │   ├── BuildArtifactReadLease.cs
│           │   ├── BuildArtifactWriteLease.cs
│           │   ├── BuildCacheCleanupPlan.cs
│           │   ├── BuildCacheOwnerDescriptor.cs
│           │   ├── BuildCachePublisher.cs
│           │   └── BuildCacheRetirement.cs
│           ├── Host/
│           │   ├── BuildHostServices.cs
│           │   ├── IBuildHostProvider.cs
│           │   ├── IToolchainWorkspaceProvider.cs
│           │   └── PhysicalToolchainWorkspaceProvider.cs
│           ├── Maintenance/
│           │   └── BuildMaintenanceApplication.cs
│           ├── Managed/
│           │   ├── ManagedBuildExecution.cs
│           │   ├── ManagedBuildInputReader.cs
│           │   ├── ManagedBuildInputSnapshot.cs
│           │   ├── ManagedInputEvaluation.cs
│           │   ├── ManagedInputEvaluatorPreparation.cs
│           │   └── ManagedInputGraphTask.cs.txt
│           ├── Native/
│           │   ├── BuildHostDescriptor.cs
│           │   ├── ComponentNativeProductExecutor.cs
│           │   ├── INativeProductExecutor.cs
│           │   ├── NativeArtifact.cs
│           │   ├── NativeArtifactPublisher.cs
│           │   ├── NativeBindingGenerationDescriptor.cs
│           │   ├── NativeBindingGenerationRequest.cs
│           │   ├── NativeBindingPreparation.cs
│           │   ├── NativeBuildProduct.cs
│           │   ├── NativeBuildRecipe.cs
│           │   ├── NativeCMakeExecutor.cs
│           │   ├── NativeCMakeSource.cs
│           │   ├── NativeCompilerInputs.cs
│           │   ├── NativeComponentBuildOptions.cs
│           │   ├── NativeInputReadCache.cs
│           │   ├── NativeLinkPlan.cs
│           │   ├── NativeStaticBuildDefinition.cs
│           │   ├── NativeToolchainPreparation.cs
│           │   ├── NativeToolchainSelection.cs
│           │   ├── NativeWorkspaceIdentity.cs
│           │   ├── ProductNativeBuildPlan.cs
│           │   ├── ProductNativeBuildResult.cs
│           │   ├── ProductNativeBuildStep.cs
│           │   ├── ProductNativeCacheLookup.cs
│           │   └── ProductNativeDeployment.cs
│           ├── NativeBuildContext.cs
│           ├── ToolchainWorkingDirectory.cs
│           ├── packages.browser-wasm.win-x64.lock.json
│           └── packages.macos-arm64.win-x64.lock.json
├── docs/
│   ├── README.md
│   ├── architecture/
│   │   ├── BUILD_CACHE_ARCHITECTURE.md
│   │   ├── CURRENT_ACCEPTANCE.md
│   │   ├── CURRENT_ISSUES.md
│   │   ├── NATIVE_BUILD_PLATFORM_OWNERSHIP.md
│   │   ├── PLATFORM_EXTENSION_GUIDE.md
│   │   ├── PLATFORM_RUNTIME_ARCHITECTURE.md
│   │   └── README.md
│   ├── backends/
│   │   ├── Bgfx/
│   │   │   ├── Inno.Build.Toolchains.Bgfx.Tools.md
│   │   │   ├── Inno.Build.Toolchains.Bgfx.md
│   │   │   └── Inno.Native.Bgfx.md
│   │   ├── DotNet/
│   │   │   └── Inno.Build.Managed.DotNet.md
│   │   ├── ImGui/
│   │   │   ├── Inno.Build.Toolchains.ImGui.md
│   │   │   └── Inno.Build.Toolchains.ImGuizmo.md
│   │   ├── MiniAudio/
│   │   │   ├── Inno.Build.Toolchains.MiniAudio.md
│   │   │   └── Inno.Native.MiniAudio.md
│   │   ├── RmlUi/
│   │   │   ├── Inno.Build.Toolchains.UI.md
│   │   │   └── Inno.Native.UI.md
│   │   ├── Sdl3/
│   │   │   ├── Inno.Build.Toolchains.Sdl3.md
│   │   │   └── Inno.Native.Sdl3.md
│   │   └── Text/
│   │       ├── Inno.Build.Toolchains.Text.md
│   │       └── Inno.Native.Text.md
│   ├── build/
│   │   ├── Inno.Build.Bindings.md
│   │   ├── Inno.Build.Cli.md
│   │   ├── Inno.Build.Composition.md
│   │   ├── Inno.Build.Distribution.Standard.md
│   │   ├── Inno.Build.Products.Standard.md
│   │   ├── Inno.Build.SupportPacks.Core.md
│   │   ├── Inno.Build.TaskHosting.md
│   │   ├── Inno.Build.Tasks.md
│   │   ├── Inno.Build.Toolchains.md
│   │   ├── Inno.Build.md
│   │   └── README.md
│   ├── core/
│   │   └── Inno.Core.IO.md
│   ├── native/
│   │   └── BindingGeneration.md
│   ├── platform/
│   │   ├── BrowserWasm32/
│   │   │   ├── Inno.Build.BrowserWasm32.md
│   │   │   ├── Inno.Integration.BrowserWasm32.Bgfx.md
│   │   │   ├── Inno.Integration.BrowserWasm32.Build.md
│   │   │   └── README.md
│   │   ├── MacOSArm64/
│   │   │   ├── Inno.Build.MacOSArm64.md
│   │   │   ├── Inno.Integration.MacOSArm64.Bgfx.md
│   │   │   ├── Inno.Integration.MacOSArm64.Build.md
│   │   │   └── README.md
│   │   ├── PRODUCT_STARTUP.md
│   │   ├── README.md
│   │   ├── Windows/
│   │   │   ├── Inno.Build.Windows.Host.md
│   │   │   └── README.md
│   │   └── WindowsX64/
│   │       ├── Inno.Build.WindowsX64.md
│   │       ├── Inno.Editor.WindowsX64.md
│   │       ├── Inno.Integration.WindowsX64.Bgfx.md
│   │       ├── Inno.Integration.WindowsX64.Build.md
│   │       └── README.md
│   └── tooling/
│       └── Inno.Tooling.Architecture.md
├── entry/
│   ├── DevelopmentHost.props
│   ├── InnoEngine.Build/
│   │   ├── InnoEngine.Build.csproj
│   │   ├── Program.cs
│   │   ├── packages.browser-wasm.lock.json
│   │   ├── packages.lock.json
│   │   ├── packages.macos-arm64.lock.json
│   │   └── packages.windows-x64.lock.json
│   └── InnoEngine.Editor/
│       ├── InnoEngine.Editor.csproj
│       └── packages.lock.json
├── platforms/
│   ├── BrowserWasm32/
│   │   ├── build/
│   │   │   └── Inno.Build.BrowserWasm32/
│   │   │       ├── BrowserWasm32BuildModule.cs
│   │   │       ├── Native/
│   │   │       │   ├── BrowserWasm32LinkerSettings.cs
│   │   │       │   └── BrowserWasm32NativeProductExecutor.cs
│   │   │       ├── SupportPacks/
│   │   │       │   └── BrowserWasm32PlayerSupportPackSource.cs
│   │   │       ├── Targets/
│   │   │       │   └── BrowserWasm32GameBuildTarget.cs
│   │   │       ├── Toolchain/
│   │   │       │   └── EmscriptenNativeToolchainProvider.cs
│   │   │       ├── packages.win-x64.lock.json
│   │   │       └── packages.windows-x64.win-x64.lock.json
│   │   └── integrations/
│   │       ├── Inno.Integration.BrowserWasm32.Bgfx/
│   │       │   ├── packages.win-x64.lock.json
│   │       │   └── packages.windows-x64.win-x64.lock.json
│   │       └── Inno.Integration.BrowserWasm32.Build/
│   │           ├── Bindings/
│   │           │   └── BrowserWasm32BindingProfile.cs
│   │           ├── BrowserWasm32BuildIntegration.cs
│   │           ├── Deployment/
│   │           │   └── BrowserWasm32NativeLayout.cs
│   │           ├── Inno.Integration.BrowserWasm32.Build.csproj
│   │           ├── Products/
│   │           │   └── BrowserWasm32NativeProducts.cs
│   │           ├── packages.browser-wasm.lock.json
│   │           ├── packages.lock.json
│   │           ├── packages.macos-arm64.lock.json
│   │           ├── packages.win-x64.lock.json
│   │           ├── packages.windows-x64.lock.json
│   │           └── packages.windows-x64.win-x64.lock.json
│   ├── MacOSArm64/
│   │   ├── build/
│   │   │   └── Inno.Build.MacOSArm64/
│   │   │       ├── DevelopmentProduct.props
│   │   │       ├── Host/
│   │   │       │   └── MacOSArm64BuildHostProvider.cs
│   │   │       ├── MacOSArm64BuildModule.cs
│   │   │       ├── Native/
│   │   │       │   └── MacOSArm64LinkerSettings.cs
│   │   │       ├── SupportPacks/
│   │   │       │   ├── MacOSArm64PlayerSupportPackSource.cs
│   │   │       │   └── MacOSArm64SupportPackValidator.cs
│   │   │       ├── Targets/
│   │   │       │   └── MacOSArm64GameBuildTarget.cs
│   │   │       ├── Toolchain/
│   │   │       │   └── MacOSArm64NativeToolchainProvider.cs
│   │   │       ├── packages.win-x64.lock.json
│   │   │       └── packages.windows-x64.win-x64.lock.json
│   │   ├── editor/
│   │   │   └── Inno.Editor.MacOSArm64/
│   │   │       └── MacOSArm64EditorComposition.cs
│   │   └── integrations/
│   │       ├── Inno.Integration.MacOSArm64.Bgfx/
│   │       │   ├── MacOSArm64BgfxToolContribution.cs
│   │       │   ├── packages.win-x64.lock.json
│   │       │   └── packages.windows-x64.win-x64.lock.json
│   │       └── Inno.Integration.MacOSArm64.Build/
│   │           ├── Bindings/
│   │           │   └── MacOSArm64BindingProfile.cs
│   │           ├── Deployment/
│   │           │   └── MacOSArm64NativeLayout.cs
│   │           ├── Inno.Integration.MacOSArm64.Build.csproj
│   │           ├── MacOSArm64BuildIntegration.cs
│   │           ├── Products/
│   │           │   └── MacOSArm64NativeProducts.cs
│   │           ├── packages.browser-wasm.lock.json
│   │           ├── packages.lock.json
│   │           ├── packages.macos-arm64.lock.json
│   │           ├── packages.win-x64.lock.json
│   │           ├── packages.windows-x64.lock.json
│   │           └── packages.windows-x64.win-x64.lock.json
│   ├── Windows/
│   │   └── build/
│   │       └── Inno.Build.Windows.Host/
│   │           ├── Inno.Build.Windows.Host.csproj
│   │           ├── WindowsBuildHostProvider.cs
│   │           ├── Workspace/
│   │           │   ├── WindowsToolchainDirectory.cs
│   │           │   └── WindowsToolchainWorkspaceProvider.cs
│   │           ├── packages.browser-wasm.lock.json
│   │           ├── packages.lock.json
│   │           ├── packages.macos-arm64.lock.json
│   │           ├── packages.win-x64.lock.json
│   │           ├── packages.windows-x64.lock.json
│   │           └── packages.windows-x64.win-x64.lock.json
│   └── WindowsX64/
│       ├── build/
│       │   └── Inno.Build.WindowsX64/
│       │       ├── DevelopmentProduct.props
│       │       ├── Native/
│       │       │   └── WindowsX64LinkerSettings.cs
│       │       ├── SupportPacks/
│       │       │   ├── WindowsX64PlayerSupportPackSource.cs
│       │       │   └── WindowsX64SupportPackValidator.cs
│       │       ├── Targets/
│       │       │   └── WindowsX64GameBuildTarget.cs
│       │       ├── Toolchain/
│       │       │   └── WindowsX64NativeToolchainProvider.cs
│       │       ├── WindowsX64BuildModule.cs
│       │       ├── packages.win-x64.lock.json
│       │       └── packages.windows-x64.win-x64.lock.json
│       ├── editor/
│       │   └── Inno.Editor.WindowsX64/
│       │       ├── WindowsX64EditorComposition.cs
│       │       └── packages.windows-x64.lock.json
│       └── integrations/
│           ├── Inno.Integration.WindowsX64.Bgfx/
│           │   ├── WindowsX64BgfxToolContribution.cs
│           │   ├── packages.win-x64.lock.json
│           │   └── packages.windows-x64.win-x64.lock.json
│           └── Inno.Integration.WindowsX64.Build/
│               ├── Bindings/
│               │   └── WindowsX64BindingProfile.cs
│               ├── Deployment/
│               │   └── WindowsX64NativeLayout.cs
│               ├── Inno.Integration.WindowsX64.Build.csproj
│               ├── Products/
│               │   └── WindowsX64NativeProducts.cs
│               ├── WindowsX64BuildIntegration.cs
│               ├── packages.browser-wasm.lock.json
│               ├── packages.lock.json
│               ├── packages.macos-arm64.lock.json
│               ├── packages.win-x64.lock.json
│               ├── packages.windows-x64.lock.json
│               └── packages.windows-x64.win-x64.lock.json
├── records/
│   └── architecture/
│       └── acceptance/
│           ├── PLATFORM_OWNERSHIP_CACHE_ACCEPTANCE_2026_10_22.md
│           └── PLATFORM_OWNERSHIP_CACHE_FILE_TREE_2026_10_22.md
├── src/
│   ├── adapters/
│   │   ├── audio/
│   │   │   └── Inno.Adapter.Audio/
│   │   │       └── packages.win-x64.lock.json
│   │   ├── common/
│   │   │   └── Inno.Adapter/
│   │   │       └── packages.win-x64.lock.json
│   │   ├── input/
│   │   │   └── Inno.Adapter.Input/
│   │   │       └── packages.win-x64.lock.json
│   │   ├── platform/
│   │   │   └── Inno.Adapter.Platform/
│   │   │       └── packages.win-x64.lock.json
│   │   ├── presentation/
│   │   │   └── Inno.Adapter.Presentation/
│   │   │       ├── packages.win-x64.lock.json
│   │   │       └── packages.windows-x64.win-x64.lock.json
│   │   ├── rendering/
│   │   │   ├── Inno.Adapter.Rendering.Authoring/
│   │   │   │   ├── packages.win-x64.lock.json
│   │   │   │   └── packages.windows-x64.win-x64.lock.json
│   │   │   └── Inno.Adapter.Rendering/
│   │   │       └── packages.win-x64.lock.json
│   │   ├── storage/
│   │   │   └── Inno.Adapter.Storage/
│   │   │       └── packages.win-x64.lock.json
│   │   ├── text/
│   │   │   └── Inno.Adapter.Text/
│   │   │       └── packages.win-x64.lock.json
│   │   └── ui/
│   │       └── Inno.Adapter.UI/
│   │           └── packages.win-x64.lock.json
│   ├── composition/
│   │   └── editor/
│   │       ├── contracts/
│   │       │   └── Inno.Editor.Annotations/
│   │       │       ├── packages.win-x64.lock.json
│   │       │       └── packages.windows-x64.win-x64.lock.json
│   │       ├── features/
│   │       │   ├── Inno.Editor.Assets/
│   │       │   │   ├── packages.win-x64.lock.json
│   │       │   │   └── packages.windows-x64.win-x64.lock.json
│   │       │   ├── Inno.Editor.Audio/
│   │       │   │   ├── packages.win-x64.lock.json
│   │       │   │   └── packages.windows-x64.win-x64.lock.json
│   │       │   ├── Inno.Editor.PlayMode/
│   │       │   │   ├── packages.win-x64.lock.json
│   │       │   │   └── packages.windows-x64.win-x64.lock.json
│   │       │   ├── Inno.Editor.Rendering/
│   │       │   │   ├── packages.win-x64.lock.json
│   │       │   │   └── packages.windows-x64.win-x64.lock.json
│   │       │   ├── Inno.Editor.Scene/
│   │       │   │   ├── packages.win-x64.lock.json
│   │       │   │   └── packages.windows-x64.win-x64.lock.json
│   │       │   ├── Inno.Editor.Scripting/
│   │       │   │   ├── packages.win-x64.lock.json
│   │       │   │   └── packages.windows-x64.win-x64.lock.json
│   │       │   └── Inno.Editor.Shaders/
│   │       │       ├── packages.win-x64.lock.json
│   │       │       └── packages.windows-x64.win-x64.lock.json
│   │       ├── framework/
│   │       │   ├── Inno.Editor.Core/
│   │       │   │   ├── packages.win-x64.lock.json
│   │       │   │   └── packages.windows-x64.win-x64.lock.json
│   │       │   ├── Inno.Editor.Graph/
│   │       │   │   ├── packages.win-x64.lock.json
│   │       │   │   └── packages.windows-x64.win-x64.lock.json
│   │       │   ├── Inno.Editor.Inspection/
│   │       │   │   ├── packages.win-x64.lock.json
│   │       │   │   └── packages.windows-x64.win-x64.lock.json
│   │       │   ├── Inno.Editor.Interactions/
│   │       │   │   ├── packages.win-x64.lock.json
│   │       │   │   └── packages.windows-x64.win-x64.lock.json
│   │       │   └── Inno.Editor.Settings/
│   │       │       ├── packages.win-x64.lock.json
│   │       │       └── packages.windows-x64.win-x64.lock.json
│   │       ├── panels/
│   │       │   ├── Inno.Editor.Panel.FileBrowser/
│   │       │   │   ├── packages.win-x64.lock.json
│   │       │   │   └── packages.windows-x64.win-x64.lock.json
│   │       │   ├── Inno.Editor.Panel.Hierarchy/
│   │       │   │   ├── packages.win-x64.lock.json
│   │       │   │   └── packages.windows-x64.win-x64.lock.json
│   │       │   ├── Inno.Editor.Panel.Inspector/
│   │       │   │   ├── packages.win-x64.lock.json
│   │       │   │   └── packages.windows-x64.win-x64.lock.json
│   │       │   └── Inno.Editor.Panel.ShaderEditor/
│   │       │       ├── packages.win-x64.lock.json
│   │       │       └── packages.windows-x64.win-x64.lock.json
│   │       └── presentation/
│   │           └── Inno.Editor.ImGui/
│   │               ├── packages.win-x64.lock.json
│   │               └── packages.windows-x64.win-x64.lock.json
│   ├── content/
│   │   ├── animation/
│   │   │   ├── Inno.Animation.Assets/
│   │   │   │   ├── packages.win-x64.lock.json
│   │   │   │   └── packages.windows-x64.win-x64.lock.json
│   │   │   └── Inno.Animation/
│   │   │       └── packages.win-x64.lock.json
│   │   ├── assets/
│   │   │   ├── Inno.Assets.Pipeline/
│   │   │   │   ├── packages.win-x64.lock.json
│   │   │   │   └── packages.windows-x64.win-x64.lock.json
│   │   │   └── Inno.Assets/
│   │   │       └── packages.win-x64.lock.json
│   │   ├── deployment/
│   │   │   └── Inno.Content/
│   │   │       └── packages.win-x64.lock.json
│   │   ├── references/
│   │   │   └── Inno.References/
│   │   │       └── packages.win-x64.lock.json
│   │   └── scene/
│   │       ├── Inno.Scene.Assets/
│   │       │   ├── packages.win-x64.lock.json
│   │       │   └── packages.windows-x64.win-x64.lock.json
│   │       └── Inno.Scene/
│   │           └── packages.win-x64.lock.json
│   ├── foundation/
│   │   ├── core/
│   │   │   ├── Inno.Core.Collections/
│   │   │   │   └── packages.win-x64.lock.json
│   │   │   ├── Inno.Core.Coroutines/
│   │   │   │   └── packages.win-x64.lock.json
│   │   │   ├── Inno.Core.Diagnostics/
│   │   │   │   └── packages.win-x64.lock.json
│   │   │   ├── Inno.Core.Events/
│   │   │   │   └── packages.win-x64.lock.json
│   │   │   ├── Inno.Core.Execution/
│   │   │   │   └── packages.win-x64.lock.json
│   │   │   ├── Inno.Core.Graphs/
│   │   │   │   ├── packages.win-x64.lock.json
│   │   │   │   └── packages.windows-x64.win-x64.lock.json
│   │   │   ├── Inno.Core.IO/
│   │   │   │   ├── CoordinatedFileLease.cs
│   │   │   │   ├── PhysicalPathResolver.cs
│   │   │   │   ├── ProcessOwnedDirectory.cs
│   │   │   │   └── packages.win-x64.lock.json
│   │   │   ├── Inno.Core.Identity/
│   │   │   │   └── packages.win-x64.lock.json
│   │   │   ├── Inno.Core.Input/
│   │   │   │   └── packages.win-x64.lock.json
│   │   │   ├── Inno.Core.Jobs/
│   │   │   │   └── packages.win-x64.lock.json
│   │   │   ├── Inno.Core.Logging/
│   │   │   │   └── packages.win-x64.lock.json
│   │   │   ├── Inno.Core.Mathematics/
│   │   │   │   └── packages.win-x64.lock.json
│   │   │   ├── Inno.Core.Serialization/
│   │   │   │   └── packages.win-x64.lock.json
│   │   │   └── Inno.Core.Settings/
│   │   │       └── packages.win-x64.lock.json
│   │   ├── extensibility/
│   │   │   ├── Inno.Extensibility.Catalogs/
│   │   │   │   └── packages.win-x64.lock.json
│   │   │   ├── Inno.Extensibility.Modules/
│   │   │   │   └── packages.win-x64.lock.json
│   │   │   ├── Inno.Extensibility.Reload/
│   │   │   │   └── packages.win-x64.lock.json
│   │   │   └── Inno.Extensibility.Types/
│   │   │       └── packages.win-x64.lock.json
│   │   └── scripting/
│   │       └── Inno.Scripting.Api/
│   │           └── packages.win-x64.lock.json
│   ├── runtime/
│   │   ├── contracts/
│   │   │   └── Inno.Runtime.Contracts/
│   │   │       └── packages.win-x64.lock.json
│   │   ├── engine/
│   │   │   └── Inno.Runtime/
│   │   │       └── packages.win-x64.lock.json
│   │   ├── plugins/
│   │   │   ├── Inno.Plugins.Authoring/
│   │   │   │   ├── packages.win-x64.lock.json
│   │   │   │   └── packages.windows-x64.win-x64.lock.json
│   │   │   └── Inno.Plugins/
│   │   │       ├── packages.win-x64.lock.json
│   │   │       └── packages.windows-x64.win-x64.lock.json
│   │   └── scripting/
│   │       ├── Inno.Scripting.Compiler/
│   │       │   ├── packages.win-x64.lock.json
│   │       │   └── packages.windows-x64.win-x64.lock.json
│   │       └── Inno.Scripting.Reload/
│   │           ├── packages.win-x64.lock.json
│   │           └── packages.windows-x64.win-x64.lock.json
│   └── services/
│       ├── audio/
│       │   ├── Inno.Audio.Assets/
│       │   │   ├── packages.win-x64.lock.json
│       │   │   └── packages.windows-x64.win-x64.lock.json
│       │   ├── Inno.Audio.Runtime/
│       │   │   └── packages.win-x64.lock.json
│       │   └── Inno.Audio/
│       │       └── packages.win-x64.lock.json
│       ├── input/
│       │   └── Inno.Input/
│       │       └── packages.win-x64.lock.json
│       ├── platform/
│       │   └── Inno.Platform/
│       │       └── packages.win-x64.lock.json
│       ├── rendering/
│       │   ├── Inno.Rendering.Assets.Authoring/
│       │   │   ├── packages.win-x64.lock.json
│       │   │   └── packages.windows-x64.win-x64.lock.json
│       │   ├── Inno.Rendering.Assets/
│       │   │   └── packages.win-x64.lock.json
│       │   ├── Inno.Rendering.Runtime/
│       │   │   └── packages.win-x64.lock.json
│       │   ├── Inno.Rendering.Shaders/
│       │   │   ├── packages.win-x64.lock.json
│       │   │   └── packages.windows-x64.win-x64.lock.json
│       │   └── Inno.Rendering/
│       │       └── packages.win-x64.lock.json
│       ├── storage/
│       │   └── Inno.Storage/
│       │       └── packages.win-x64.lock.json
│       ├── text/
│       │   ├── Inno.Text.Assets/
│       │   │   ├── packages.win-x64.lock.json
│       │   │   └── packages.windows-x64.win-x64.lock.json
│       │   └── Inno.Text/
│       │       └── packages.win-x64.lock.json
│       └── ui/
│           ├── Inno.UI.Assets/
│           │   ├── packages.win-x64.lock.json
│           │   └── packages.windows-x64.win-x64.lock.json
│           ├── Inno.UI.Runtime/
│           │   └── packages.win-x64.lock.json
│           └── Inno.UI/
│               └── packages.win-x64.lock.json
├── tests/
│   ├── Invoke-BuildCacheChecks.ps1
│   ├── Invoke-IsolatedTests.ps1
│   ├── Invoke-WikiChecks.ps1
│   ├── build/
│   │   └── Inno.Build.Tests/
│   │       ├── ArchitectureVerificationCommandTests.cs
│   │       ├── BgfxNativeProfileTests.cs
│   │       ├── BindingDefinitionTests.cs
│   │       ├── BindingGenerationBatchTests.cs
│   │       ├── BindingGenerationProcessTests.cs
│   │       ├── BindingMaterializationTests.cs
│   │       ├── BrowserSupportPackTests.cs
│   │       ├── BuildCacheMetadataRetirementTests.cs
│   │       ├── BuildCachePublisherTests.cs
│   │       ├── BuildCacheRetirementTests.cs
│   │       ├── BuildCompositionTests.cs
│   │       ├── BuildPipelineTests.cs
│   │       ├── BuildTestProducts.cs
│   │       ├── FileProductPublicationTests.cs
│   │       ├── Inno.Build.Tests.csproj
│   │       ├── ManagedBuildInputSnapshotTests.cs
│   │       ├── ManagedDeploymentTests.cs
│   │       ├── NativeArtifactIdentityTests.cs
│   │       ├── NativeArtifactPublicationTests.cs
│   │       ├── NativeBindingCMakeTests.cs
│   │       ├── NativeBindingCompilationTests.cs
│   │       ├── NativeBindingConfigurationTests.cs
│   │       ├── NativeBindingGenerationTests.cs
│   │       ├── NativeBuildContextTests.cs
│   │       ├── NativeComponentBuildOptionsTests.cs
│   │       ├── NativeLinkPlanTests.cs
│   │       ├── NativeProductExecutorTests.cs
│   │       ├── NativeRecipeClosureTests.cs
│   │       ├── NativeWorkspaceReuseTests.cs
│   │       ├── PlatformBackendIntegrationTests.cs
│   │       ├── PlatformOwnershipContractTests.cs
│   │       ├── ProductNativeCacheLookupTests.cs
│   │       ├── ProductNativeCostTests.cs
│   │       ├── ShaderToolSelectionTests.cs
│   │       ├── SupportPackPreparationCacheTests.cs
│   │       ├── TaskHostPublicationProcess.cs
│   │       ├── TaskHostPublicationTests.cs
│   │       ├── TaskRuntimeBuildTests.cs
│   │       ├── ToolRunnerPathTests.cs
│   │       ├── ToolchainWorkingDirectoryBuildTests.cs
│   │       ├── ToolchainWorkingDirectoryTests.cs
│   │       ├── packages.browser-wasm.lock.json
│   │       ├── packages.lock.json
│   │       ├── packages.macos-arm64.lock.json
│   │       └── packages.windows-x64.lock.json
│   ├── core/
│   │   └── Inno.Core.IO.Tests/
│   │       ├── CoordinatedFileLeaseTests.cs
│   │       └── PhysicalPathResolverTests.cs
│   ├── editor/
│   │   └── Inno.Editor.Hosting.Tests/
│   │       ├── EditorApplicationTests.cs
│   │       ├── packages.browser-wasm.lock.json
│   │       ├── packages.lock.json
│   │       ├── packages.macos-arm64.lock.json
│   │       └── packages.windows-x64.lock.json
│   ├── rendering/
│   │   └── Inno.Rendering.Runtime.Tests/
│   │       ├── packages.browser-wasm.lock.json
│   │       ├── packages.lock.json
│   │       └── packages.macos-arm64.lock.json
│   ├── scripting/
│   │   └── Inno.Scripting.Compiler.Tests/
│   │       ├── packages.browser-wasm.lock.json
│   │       ├── packages.lock.json
│   │       └── packages.macos-arm64.lock.json
│   └── tooling/
│       └── Inno.Tooling.Architecture.Tests/
│           ├── ArchitectureCommandTests.cs
│           ├── NativePlatformPolicyTests.cs
│           ├── packages.browser-wasm.lock.json
│           ├── packages.lock.json
│           └── packages.macos-arm64.lock.json
└── tools/
    └── Inno.Tooling.Architecture/
        ├── ArchitectureRules.cs
        ├── NativeBindingOwnershipValidator.cs
        ├── NativePlatformPolicyValidator.cs
        ├── PackageDependencyBoundaryValidator.cs
        ├── PlatformOwnershipValidator.cs
        ├── packages.win-x64.lock.json
        └── packages.windows-x64.win-x64.lock.json
```

删除内容包括七组件的目标 JSON 与 common.json、共享 Windows 执行目录实现、根发行的产品组合、旧 Browser 执行 facade 及被替换的配置读取器；现行入口不读取这些路径。

## 已删除文件

- `backends/Bgfx/native/Inno.Native.Bgfx/Bindings/bindgen.browser-wasm.json`
- `backends/Bgfx/native/Inno.Native.Bgfx/Bindings/bindgen.linux-arm64.json`
- `backends/Bgfx/native/Inno.Native.Bgfx/Bindings/bindgen.linux-x64.json`
- `backends/Bgfx/native/Inno.Native.Bgfx/Bindings/bindgen.macos-arm64.json`
- `backends/Bgfx/native/Inno.Native.Bgfx/Bindings/bindgen.windows-x64.json`
- `backends/Bgfx/native/Inno.Native.Bgfx/Bindings/common.json`
- `backends/ImGui/native/Inno.Native.ImGui/Bindings/bindgen.linux-arm64.json`
- `backends/ImGui/native/Inno.Native.ImGui/Bindings/bindgen.linux-x64.json`
- `backends/ImGui/native/Inno.Native.ImGui/Bindings/bindgen.macos-arm64.json`
- `backends/ImGui/native/Inno.Native.ImGui/Bindings/bindgen.windows-x64.json`
- `backends/ImGui/native/Inno.Native.ImGui/Bindings/common.json`
- `backends/ImGui/native/Inno.Native.ImGuizmo/Bindings/bindgen.linux-arm64.json`
- `backends/ImGui/native/Inno.Native.ImGuizmo/Bindings/bindgen.linux-x64.json`
- `backends/ImGui/native/Inno.Native.ImGuizmo/Bindings/bindgen.macos-arm64.json`
- `backends/ImGui/native/Inno.Native.ImGuizmo/Bindings/bindgen.windows-x64.json`
- `backends/ImGui/native/Inno.Native.ImGuizmo/Bindings/common.json`
- `backends/MiniAudio/native/Inno.Native.MiniAudio/Bindings/bindgen.browser-wasm.json`
- `backends/MiniAudio/native/Inno.Native.MiniAudio/Bindings/bindgen.linux-arm64.json`
- `backends/MiniAudio/native/Inno.Native.MiniAudio/Bindings/bindgen.linux-x64.json`
- `backends/MiniAudio/native/Inno.Native.MiniAudio/Bindings/bindgen.macos-arm64.json`
- `backends/MiniAudio/native/Inno.Native.MiniAudio/Bindings/bindgen.windows-x64.json`
- `backends/MiniAudio/native/Inno.Native.MiniAudio/Bindings/common.json`
- `backends/RmlUi/native/Inno.Native.UI/Bindings/bindgen.browser-wasm.json`
- `backends/RmlUi/native/Inno.Native.UI/Bindings/bindgen.linux-arm64.json`
- `backends/RmlUi/native/Inno.Native.UI/Bindings/bindgen.linux-x64.json`
- `backends/RmlUi/native/Inno.Native.UI/Bindings/bindgen.macos-arm64.json`
- `backends/RmlUi/native/Inno.Native.UI/Bindings/bindgen.windows-x64.json`
- `backends/RmlUi/native/Inno.Native.UI/Bindings/common.json`
- `backends/Sdl3/native/Inno.Native.Sdl3/Bindings/bindgen.browser-wasm.json`
- `backends/Sdl3/native/Inno.Native.Sdl3/Bindings/bindgen.linux-arm64.json`
- `backends/Sdl3/native/Inno.Native.Sdl3/Bindings/bindgen.linux-x64.json`
- `backends/Sdl3/native/Inno.Native.Sdl3/Bindings/bindgen.macos-arm64.json`
- `backends/Sdl3/native/Inno.Native.Sdl3/Bindings/bindgen.windows-x64.json`
- `backends/Sdl3/native/Inno.Native.Sdl3/Bindings/common.json`
- `backends/Text/native/Inno.Native.Text/Bindings/bindgen.browser-wasm.json`
- `backends/Text/native/Inno.Native.Text/Bindings/bindgen.linux-arm64.json`
- `backends/Text/native/Inno.Native.Text/Bindings/bindgen.linux-x64.json`
- `backends/Text/native/Inno.Native.Text/Bindings/bindgen.macos-arm64.json`
- `backends/Text/native/Inno.Native.Text/Bindings/bindgen.windows-x64.json`
- `backends/Text/native/Inno.Native.Text/Bindings/common.json`
- `build/bindings/Inno.Build.Bindings/Configuration/BindingConfigurationPaths.cs`
- `build/distributions/Inno.Build.Distribution.Standard/StandardNativeBuildPlans.cs`
- `build/distributions/Inno.Build.Distribution.Standard/StandardNativeDeployment.cs`
- `build/distributions/Inno.Build.Distribution.Standard/StandardSupportPackRequirements.cs`
- `build/tasks/Inno.Build.TaskHosting/PublishTaskHostTask.Retirement.cs`
- `build/toolchains/Inno.Build.Toolchains/Platforms/WindowsToolchainDirectory.cs`
- `platforms/BrowserWasm32/build/Inno.Build.BrowserWasm32/BrowserWasm32NativeArtifacts.cs`
- `platforms/BrowserWasm32/build/Inno.Build.BrowserWasm32/BrowserWasm32Toolchain.cs`
