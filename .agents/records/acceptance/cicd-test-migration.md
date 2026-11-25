# Test ownership migration

The inventory accounts for every original test method. The [acceptance ledger](cicd-performance.md) records platform verification and execution identities.

The [class and method inventory](cicd-test-inventory.json) maps all 353 original C# test classes and 2,190 test methods to their current owners, execution layers and native prerequisites. No original method is unmapped. The one renamed performance-policy contract is explicitly superseded by the approved event policy; it is not silently removed. Mapping is a migration audit, not execution proof. PowerShell contracts and shared helpers are described below.

| Original source | Current source | Disposition |
|---|---|---|
| `backends/Bgfx/tests/Inno.Adapter.Rendering.Bgfx.Tests/BgfxCompositionProgramProviderTests.cs` | `tests/backends/Bgfx/Inno.Adapter.Rendering.Bgfx.Tests/BgfxCompositionProgramProviderTests.cs` | Preserved; location/reference changes only |
| `backends/Bgfx/tests/Inno.Adapter.Rendering.Bgfx.Tests/BgfxDeviceTests.cs` | `tests/backends/Bgfx/Inno.Adapter.Rendering.Bgfx.Tests/BgfxDeviceTests.cs` | Preserved; location/reference changes only |
| `backends/Bgfx/tests/Inno.Adapter.Rendering.Bgfx.Tests/BgfxImGuiColorContractTests.cs` | `tests/backends/Bgfx/Inno.Adapter.Rendering.Bgfx.Tests/BgfxImGuiColorContractTests.cs` | Preserved; location/reference changes only |
| `backends/Bgfx/tests/Inno.Adapter.Rendering.Bgfx.Tests/BgfxProcessLifetimeTests.cs` | `tests/backends/Bgfx/Inno.Adapter.Rendering.Bgfx.Tests/BgfxProcessLifetimeTests.cs` | Preserved; location/reference changes only |
| `backends/Bgfx/tests/Inno.Adapter.Rendering.Bgfx.Tests/BgfxShaderProfileTests.cs` | `tests/backends/Bgfx/Inno.Build.Toolchains.Bgfx.Tests/BgfxShaderProfileTests.cs` | Preserved; owner/prerequisites updated |
| `backends/Bgfx/tests/Inno.Adapter.Rendering.Bgfx.Tests/BgfxShutdownTests.cs` | `tests/backends/Bgfx/Inno.Adapter.Rendering.Bgfx.Tests/BgfxShutdownTests.cs` | Preserved; location/reference changes only |
| `backends/Bgfx/tests/Inno.Adapter.Rendering.Bgfx.Tests/BgfxSurfaceIntegrationTests.cs` | `tests/backends/Bgfx/Inno.Adapter.Rendering.Bgfx.Tests/BgfxSurfaceIntegrationTests.cs` | Preserved; location/reference changes only |
| `backends/Bgfx/tests/Inno.Adapter.Rendering.Bgfx.Tests/BgfxSurfaceRetirementTests.cs` | `tests/backends/Bgfx/Inno.Adapter.Rendering.Bgfx.Tests/BgfxSurfaceRetirementTests.cs` | Preserved; location/reference changes only |
| `backends/Bgfx/tests/Inno.Adapter.Rendering.Bgfx.Tests/BgfxToolchainTests.cs` | `tests/backends/Bgfx/Inno.Build.Toolchains.Bgfx.Integration.Tests/BgfxToolchainTests.cs`<br>`tests/backends/Bgfx/Inno.Build.Toolchains.Bgfx.Tests/BgfxShaderTargetTests.cs` | Preserved; responsibilities split |
| `backends/Bgfx/tests/Inno.Adapter.Rendering.Bgfx.Tests/NativeHostFixture.cs` | `tests/backends/Bgfx/Inno.Adapter.Rendering.Bgfx.Tests/NativeHostFixture.cs` | Preserved; location/reference changes only |
| `backends/Bgfx/tests/Inno.Adapter.Rendering.Bgfx.Tests/NativeWindowTestHost.cs` | `tests/backends/Bgfx/Inno.Adapter.Rendering.Bgfx.Tests/NativeWindowTestHost.cs` | Preserved; location/reference changes only |
| `backends/Bgfx/tests/Inno.Adapter.Rendering.Bgfx.Tests/ShaderProfileFixture.cs` | `tests/backends/Bgfx/fixtures/Inno.Testing.Bgfx/ShaderProfileFixture.cs` | Preserved; owner/prerequisites updated |
| `backends/Bgfx/tests/Inno.Adapter.Rendering.Bgfx.Tests/ToolRunnerTests.cs` | `tests/backends/Bgfx/Inno.Build.Toolchains.Bgfx.Integration.Tests/ToolRunnerTests.cs`<br>`tests/backends/Bgfx/Inno.Build.Toolchains.Bgfx.Tests/ToolRunnerTests.cs` | Preserved; responsibilities split |
| `backends/Bgfx/tests/Inno.Native.Bgfx.Tests/BgfxInitTests.cs` | `tests/backends/Bgfx/Inno.Native.Bgfx.Tests/BgfxInitTests.cs` | Preserved; location/reference changes only |
| `backends/FileSystem/tests/Inno.Adapter.Content.FileSystem.Tests/ContentCacheCancellationTests.cs` | `tests/backends/FileSystem/Inno.Adapter.Content.FileSystem.Tests/ContentCacheCancellationTests.cs` | Preserved; location/reference changes only |
| `backends/FileSystem/tests/Inno.Adapter.Content.FileSystem.Tests/ContentCacheConcurrencyTests.cs` | `tests/backends/FileSystem/Inno.Adapter.Content.FileSystem.Tests/ContentCacheConcurrencyTests.cs` | Preserved; location/reference changes only |
| `backends/FileSystem/tests/Inno.Adapter.Content.FileSystem.Tests/ContentCacheFixture.cs` | `tests/backends/FileSystem/Inno.Adapter.Content.FileSystem.Tests/ContentCacheFixture.cs` | Preserved; location/reference changes only |
| `backends/FileSystem/tests/Inno.Adapter.Content.FileSystem.Tests/ContentCacheIntegrityTests.cs` | `tests/backends/FileSystem/Inno.Adapter.Content.FileSystem.Tests/ContentCacheIntegrityTests.cs` | Preserved; location/reference changes only |
| `backends/FileSystem/tests/Inno.Adapter.Content.FileSystem.Tests/ContentCacheProcessTests.cs` | `tests/backends/FileSystem/Inno.Adapter.Content.FileSystem.Tests/ContentCacheProcessTests.cs` | Preserved; location/reference changes only |
| `backends/FileSystem/tests/Inno.Adapter.Content.FileSystem.Tests/ContentCachePublicationTests.cs` | `tests/backends/FileSystem/Inno.Adapter.Content.FileSystem.Tests/ContentCachePublicationTests.cs` | Preserved; location/reference changes only |
| `backends/FileSystem/tests/Inno.Adapter.Content.FileSystem.Tests/ContentCacheRetirementTests.cs` | `tests/backends/FileSystem/Inno.Adapter.Content.FileSystem.Tests/ContentCacheRetirementTests.cs` | Preserved; location/reference changes only |
| `backends/ImGui/tests/Inno.Native.ImGui.Tests/ImGuiAbiLayoutTests.cs` | `tests/backends/ImGui/Inno.Native.ImGui.Tests/ImGuiAbiLayoutTests.cs` | Preserved; location/reference changes only |
| `backends/ImGui/tests/Inno.Native.ImGui.Tests/ImGuiInitTests.cs` | `tests/backends/ImGui/Inno.Native.ImGui.Tests/ImGuiInitTests.cs` | Preserved; location/reference changes only |
| `backends/ImGui/tests/Inno.Native.ImGui.Tests/ImGuiPlatformCallbackTests.cs` | `tests/backends/ImGui/Inno.Native.ImGui.Tests/ImGuiPlatformCallbackTests.cs` | Preserved; location/reference changes only |
| `backends/ImGui/tests/Inno.Native.ImGuizmo.Tests/ImGuizmoInitTests.cs` | `tests/backends/ImGui/Inno.Native.ImGuizmo.Tests/ImGuizmoInitTests.cs` | Preserved; location/reference changes only |
| `backends/MiniAudio/tests/Inno.Adapter.Audio.MiniAudio.Tests/EncodedAudioTestSource.cs` | `tests/backends/MiniAudio/Inno.Adapter.Audio.MiniAudio.Tests/EncodedAudioTestSource.cs` | Preserved; location/reference changes only |
| `backends/MiniAudio/tests/Inno.Adapter.Audio.MiniAudio.Tests/EncodedContentTests.cs` | `tests/backends/MiniAudio/Inno.Adapter.Audio.MiniAudio.Tests/EncodedContentTests.cs` | Preserved; location/reference changes only |
| `backends/MiniAudio/tests/Inno.Adapter.Audio.MiniAudio.Tests/MiniAudioDeviceTests.cs` | `tests/backends/MiniAudio/Inno.Adapter.Audio.MiniAudio.Tests/MiniAudioDeviceTests.cs` | Preserved; location/reference changes only |
| `backends/MiniAudio/tests/Inno.Native.MiniAudio.Tests/MiniAudioInitTests.cs` | `tests/backends/MiniAudio/Inno.Native.MiniAudio.Tests/MiniAudioInitTests.cs` | Preserved; location/reference changes only |
| `backends/RmlUi/tests/Inno.Adapter.UI.RmlUi.Tests/UiServiceTests.cs` | `tests/backends/RmlUi/Inno.Adapter.UI.RmlUi.Tests/UiServiceTests.cs` | Preserved; location/reference changes only |
| `backends/Sdl3/tests/Inno.Adapter.Platform.Sdl3.Tests/HostIntegrationTests.cs` | `tests/backends/Sdl3/Inno.Adapter.Platform.Sdl3.Tests/HostIntegrationTests.cs` | Preserved; location/reference changes only |
| `backends/Sdl3/tests/Inno.Adapter.Platform.Sdl3.Tests/NativeHostFixture.cs` | `tests/backends/Sdl3/Inno.Adapter.Platform.Sdl3.Tests/NativeHostFixture.cs` | Preserved; location/reference changes only |
| `backends/Sdl3/tests/Inno.Adapter.Platform.Sdl3.Tests/NativeSurfaceLifetimeTests.cs` | `tests/backends/Sdl3/Inno.Adapter.Platform.Sdl3.Tests/NativeSurfaceLifetimeTests.cs` | Preserved; location/reference changes only |
| `backends/Sdl3/tests/Inno.Adapter.Platform.Sdl3.Tests/NativeWindowTestHost.cs` | `tests/backends/Sdl3/Inno.Adapter.Platform.Sdl3.Tests/NativeWindowTestHost.cs` | Preserved; location/reference changes only |
| `backends/Sdl3/tests/Inno.Adapter.Platform.Sdl3.Tests/Sdl3WindowOperationsTests.cs` | `tests/backends/Sdl3/Inno.Adapter.Platform.Sdl3.Tests/Sdl3WindowOperationsTests.cs` | Preserved; location/reference changes only |
| `backends/Sdl3/tests/Inno.Adapter.Platform.Sdl3.Tests/WindowOwnershipTests.cs` | `tests/backends/Sdl3/Inno.Adapter.Platform.Sdl3.Tests/WindowOwnershipTests.cs` | Preserved; location/reference changes only |
| `backends/Sdl3/tests/Inno.Adapter.Platform.Sdl3.Tests/WindowSurfaceTests.cs` | `tests/backends/Sdl3/Inno.Adapter.Platform.Sdl3.Tests/WindowSurfaceTests.cs` | Preserved; location/reference changes only |
| `backends/Sdl3/tests/Inno.Native.Sdl3.Tests/NativeDllLoaderTests.cs` | `tests/backends/Sdl3/Inno.Native.Sdl3.Tests/NativeDllLoaderTests.cs` | Preserved; location/reference changes only |
| `backends/Sdl3/tests/Inno.Native.Sdl3.Tests/Sdl3InitTests.cs` | `tests/backends/Sdl3/Inno.Native.Sdl3.Tests/Sdl3InitTests.cs` | Preserved; location/reference changes only |
| `platforms/Windows/build/Inno.Build.Windows.Host/WindowsBuildHostProvider.cs` | `platforms/WindowsX64/build/Inno.Build.WindowsX64/Host/WindowsBuildHostProvider.cs` | Preserved; location/reference changes only |
| `platforms/Windows/build/Inno.Build.Windows.Host/Workspace/WindowsToolchainDirectory.cs` | `platforms/WindowsX64/build/Inno.Build.WindowsX64/Host/Workspace/WindowsToolchainDirectory.cs` | Preserved; location/reference changes only |
| `platforms/Windows/build/Inno.Build.Windows.Host/Workspace/WindowsToolchainWorkspaceProvider.cs` | `platforms/WindowsX64/build/Inno.Build.WindowsX64/Host/Workspace/WindowsToolchainWorkspaceProvider.cs` | Preserved; location/reference changes only |

## Build responsibility separation

| Original source | Current source | Disposition |
|---|---|---|
| `tests/build/Inno.Build.Tests/BuildCacheMetadataRetirementTests.cs` | `tests/build/Inno.Build.Toolchains.Tests/BuildCacheMetadataRetirementTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/BuildCacheOwnerCatalogTests.cs` | `tests/build/Inno.Build.Toolchains.Tests/BuildCacheOwnerCatalogTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/BuildCacheRetirementTests.cs` | `tests/build/Inno.Build.Toolchains.Tests/BuildCacheRetirementTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/BuildCacheWorkspaceLinkTests.cs` | `tests/build/Inno.Build.Toolchains.Tests/BuildCacheWorkspaceLinkTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/UnixFileLinksFactAttribute.cs` | `tests/build/Inno.Build.Toolchains.Tests/UnixFileLinksFactAttribute.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/BuildOutputOwnershipTests.cs` | `tests/build/Inno.Build.Toolchains.Tests/BuildOutputOwnershipTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/NativeDeploymentIndexTests.cs` | `tests/build/Inno.Build.Toolchains.Tests/NativeDeploymentIndexTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/NativeLinkPlanTests.cs` | `tests/build/Inno.Build.Toolchains.Tests/NativeLinkPlanTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/NativeProductExecutorTests.cs` | `tests/build/Inno.Build.Toolchains.Tests/NativeProductExecutorTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/ProductNativeCacheLookupTests.cs` | `tests/build/Inno.Build.Toolchains.Tests/ProductNativeCacheLookupTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/ProcessOwnershipTests.cs` | `tests/build/Inno.Build.Toolchains.Tests/ProcessOwnershipTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/ToolchainProcessTests.cs` | `tests/build/Inno.Build.Toolchains.Tests/ToolchainProcessTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/BindingMaterializationTests.cs` | `tests/build/Inno.Build.Bindings.Tests/BindingMaterializationTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/BindingSelectionLifetimeTests.cs` | `tests/build/Inno.Build.Bindings.Tests/BindingSelectionLifetimeTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/SupportPackPublisherTests.cs` | `tests/build/Inno.Build.SupportPacks.Tests/SupportPackPublisherTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/SupportPackPreparationCacheTests.cs` | `tests/build/Inno.Build.SupportPacks.Tests/SupportPackPreparationCacheTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/SupportPackClosureValidationTests.cs` | `tests/build/Inno.Build.SupportPacks.Tests/SupportPackClosureValidationTests.cs`<br>`tests/platforms/BrowserWasm32/Inno.Build.BrowserWasm32.Tests/BrowserSupportPackAbiTests.cs` | Preserved; responsibilities split |
| `tests/build/Inno.Build.Tests/BuildCompositionTests.cs` | `tests/build/Inno.Build.Products.Tests/BuildCompositionTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/BuildCachePublisherTests.cs` | `tests/build/Inno.Build.Products.Tests/BuildCachePublisherTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/BuildTestProducts.cs` | `tests/build/Inno.Build.Products.Tests/BuildTestProducts.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/NativeBuildContextTests.cs` | `tests/build/Inno.Build.Products.Tests/NativeBuildContextTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/NativeComponentBuildOptionsTests.cs` | `tests/build/Inno.Build.Products.Tests/NativeComponentBuildOptionsTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/NativeRecipeClosureTests.cs` | `tests/build/Inno.Build.Products.Tests/NativeRecipeClosureTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/PlatformBackendIntegrationTests.cs` | `tests/build/Inno.Build.Products.Tests/PlatformBackendIntegrationTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/PlatformOwnershipContractTests.cs` | `tests/build/Inno.Build.Products.Tests/PlatformOwnershipContractTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/ShaderBuildInputsTests.cs` | `tests/build/Inno.Build.Products.Tests/ShaderBuildInputsTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/ShaderToolSelectionTests.cs` | `tests/build/Inno.Build.Products.Tests/ShaderToolSelectionTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/ManagedDeploymentTests.cs` | `tests/build/Inno.Build.Products.Tests/ManagedDeploymentTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/GameContentCompilerBindingTests.cs` | `tests/build/Inno.Build.Products.Tests/GameContentCompilerBindingTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/BindingToolchainTaskTests.cs` | `tests/build/Inno.Build.Integration.Tests/BindingToolchainTaskTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/CompilerToolBuildScopeTests.cs` | `tests/build/Inno.Build.Integration.Tests/CompilerToolBuildScopeTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/DevelopmentAppHostSdkTests.cs` | `tests/build/Inno.Build.Integration.Tests/DevelopmentAppHostSdkTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/DevelopmentEntryBuildTests.cs` | `tests/build/Inno.Build.Integration.Tests/DevelopmentEntryBuildTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/DotNetSdkEnvironmentTests.cs` | `tests/build/Inno.Build.Integration.Tests/DotNetSdkEnvironmentTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/EditorShaderProfileTests.cs` | `tests/build/Inno.Build.Integration.Tests/EditorShaderProfileTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/FileProductPublicationTests.cs` | `tests/build/Inno.Build.Integration.Tests/FileProductPublicationTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/GeneratorRestoreScopeTests.cs` | `tests/build/Inno.Build.Integration.Tests/GeneratorRestoreScopeTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/HostRuntimeClosureBuildTests.cs` | `tests/build/Inno.Build.Integration.Tests/HostRuntimeClosureBuildTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/ManagedBuildInputSnapshotTests.cs` | `tests/build/Inno.Build.Integration.Tests/ManagedBuildInputSnapshotTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/ManagedOutputOwnershipTests.cs` | `tests/build/Inno.Build.Integration.Tests/ManagedOutputOwnershipTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/NativeBindingConfigurationTests.cs` | `tests/build/Inno.Build.Integration.Tests/NativeBindingConfigurationTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/NativeBindingSelectionBuildTests.cs` | `tests/build/Inno.Build.Integration.Tests/NativeBindingSelectionBuildTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/NuGetBindingDependencyTests.cs` | `tests/build/Inno.Build.Integration.Tests/NuGetBindingDependencyTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/NuGetLoadedIdentityTests.cs` | `tests/build/Inno.Build.Integration.Tests/NuGetLoadedIdentityTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/OwnedRestoreTests.cs` | `tests/build/Inno.Build.Integration.Tests/OwnedRestoreTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/SdkRestoreTests.cs` | `tests/build/Inno.Build.Integration.Tests/SdkRestoreTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/SolutionBootstrapOwnershipTests.cs` | `tests/build/Inno.Build.Integration.Tests/SolutionBootstrapOwnershipTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/StaticGraphRestoreTests.cs` | `tests/build/Inno.Build.Integration.Tests/StaticGraphRestoreTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/TaskHostPublicationProcess.cs` | `tests/build/Inno.Build.Integration.Tests/TaskHostPublicationProcess.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/TaskHostPublicationTests.cs` | `tests/build/Inno.Build.Integration.Tests/TaskHostPublicationTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/TaskPublisherBootstrapTests.cs` | `tests/build/Inno.Build.Integration.Tests/TaskPublisherBootstrapTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/TaskRegistrationBuildTests.cs` | `tests/build/Inno.Build.Integration.Tests/TaskRegistrationBuildTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/TaskRestoreEvaluationTests.cs` | `tests/build/Inno.Build.Integration.Tests/TaskRestoreEvaluationTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/TaskRuntimeBuildTests.cs` | `tests/build/Inno.Build.Integration.Tests/TaskRuntimeBuildTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/ToolchainEnvironmentTests.cs` | `tests/build/Inno.Build.Integration.Tests/ToolchainEnvironmentTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/TestExecution.cs` | `tests/build/Inno.Build.Integration.Tests/TestExecution.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/ToolchainWorkingDirectoryTests.cs` | `tests/platforms/WindowsX64/Inno.Build.WindowsX64.Tests/ToolchainWorkingDirectoryTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/ToolRunnerPathTests.cs` | `tests/platforms/WindowsX64/Inno.Build.WindowsX64.Tests/ToolRunnerPathTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/NativeBindingCMakeTests.cs` | `tests/platforms/WindowsX64/Inno.Build.WindowsX64.Integration.Tests/NativeBindingCMakeTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/ToolchainWorkingDirectoryBuildTests.cs` | `tests/platforms/WindowsX64/Inno.Build.WindowsX64.Integration.Tests/ToolchainWorkingDirectoryBuildTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/BrowserToolchainTests.cs` | `tests/platforms/BrowserWasm32/Inno.Build.BrowserWasm32.Tests/BrowserToolchainTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/BrowserSupportPackTests.cs` | `tests/platforms/BrowserWasm32/Inno.Build.BrowserWasm32.Tests/BrowserSupportPackTests.cs` | Preserved; isolated execution prerequisites |
| `tests/build/Inno.Build.Tests/BgfxNativeProfileTests.cs` | `tests/backends/Bgfx/Inno.Build.Toolchains.Bgfx.Tests/BgfxNativeProfileTests.cs` | Preserved; isolated execution prerequisites |

NativeWorkspaceReuseTests: retained three generic publication scenarios in Toolchains.Tests; real Windows CMake scenario moved to WindowsNativeWorkspaceTests. Browser ABI scenario moved out of generic Support Pack closure tests. Assertions retained.

## Validation execution separation

ValidationRepository moved to tests/tooling/fixtures/Inno.Testing.Validation; its public surface is test support only. ValidationCompletionTests, ValidationRuntimeClosureTests, ValidationSolutionBuildTests, ValidationSolutionTests, ValidationTestOwnershipTests, ValidationProgressTests moved to Inno.Tooling.Validation.Integration.Tests without removing assertions. Planning/evidence contracts remain in Inno.Tooling.Validation.Tests.

BuildPipelineTests and its presentation partial were split by syntax members into BuildProfileTests, SupportPackWorkflowTests, GameBuildWorkflowTests and ExportWorkflowTests. Shared setup is owned solely by the test-only BuildPipelineFixture. All test methods and assertions retained. NativeArtifactPublicationTests retains generic cases; WindowsColdPublicationTests owns its real compiler regression.

Platform pack validation methods were separated from SupportPackWorkflowTests into WindowsX64 and MacOSArm64 test owners. Each boolean platform theory case is retained as the corresponding platform Fact; all MacOS pack runtime and authoring-boundary assertions remain.

ValidationConsumerInputTests and ValidationHostContractsTests also belong to Validation.Integration.Tests: they execute real SDK compilation, consumer source preparation and actual TRX discovery. Portable plan fixtures explicitly bind their logical workspace paths before local filesystem assertions; no old production plan reader is retained.

The retired CI retention adapter tests were replaced by eight shard adapter contracts. Native producer immutable cache transfer remains covered by native artifact transfer tests; ordinary CI no longer owns a Retain operation. Seven additional transport contracts cover workflow retries and native download applicability. Existing producer cost tests retain their assertions and declare InnoValidationRequiresNativeProducer; consume-only performance plans exclude them and execute warm consumer sampling plus lifecycle checks instead.

## Further prerequisite separation and integrity coverage

- `tests/build/fixtures/Inno.Testing.SupportPacks` owns the minimal shared pack metadata fixture. Windows and Mac metadata validation no longer initialize EngineHost or assemble its full runtime closure. `Inno.Testing.Build` derives from that fixture and supplies the real reference closure required by game compilation. Both helpers use the existing `IsTestSupportProject` declaration.
- `tests/editor/Inno.Editor.Exporting.Tests` retains managed progress and stale-cancellation behavior without native deployment. `tests/editor/Inno.Editor.Exporting.Native.Tests` owns the three native presentation/lifecycle methods (four parameterized executions), retaining all native calls and assertions. An unprepared direct run recorded four native deployment failures; these remain unavailable product evidence, not passes.
- `EditorShaderProfileTests` now restores its exact, owned SDK fixture before querying transitive references. It no longer depends on another suite having restored an entire Editor product.
- The SDL adapter test deployment now follows `InnoNativeTarget`; direct MSBuild evaluation confirms both WindowsX64 and MacOSArm64 selections. Its former fixed Windows target was masked by the CI planner's global override and was incorrect for direct Mac execution.
- `SuccessfulSourceComparisonRetainsSeparateOwnerAndPackageInputs` moves from the validation contract project into `ValidationDocumentationCompilationTests` in the integration project. It performs real SDK compilation, restored-package mutation and example execution; all method contents and assertions remain intact. The source-only foreign-surface case stays in the contract project.
- Dependency contracts now cover generated binding transfer through real BGCS generation, exact dependency closure rejection, hidden mutable source changes and uninitialized submodule consumption. Consumer checks must not trust Git index timestamp or assume-unchanged flags.
- Nightly comparison tests preserve complete plan/check/environment identity; transport tests reject foreign producers, incomplete host matrices, altered receipts and missing evidence. `Write-ValidationSummary.Tests.ps1` preserves selected host scope, first failures and unavailable-result reporting.

The machine-readable inventory maps all 2,190 original methods in 353 classes to current owners. No original method is unmapped and no original parameterized case count was reduced. Current runtime results and unavailable platform/product execution remain separately recorded; mapping is not execution evidence.

### Bgfx prerequisite split and native test preparation

The Bgfx profile, target capability and missing-tool contracts moved from the native Adapter test owner into `Inno.Build.Toolchains.Bgfx.Tests`; its thirteen cases passed before the final offline compiler reference cleanup. Real shader and texture process cases moved into `Inno.Build.Toolchains.Bgfx.Integration.Tests`, which requires shader-tools only. `ShaderProfileFixture` and its capability constructor are shared by these two test owners through `Inno.Testing.Bgfx`. All original assertions and failure cases are retained. The shader compiler owns its binary uniform tags and no longer references the native renderer assembly just to read four enum values.

Native test product declarations now enter the existing pre-compilation product preparation path, replacing their too-late after-Build task. Actual no-extern ImGui consumption proves binding propagation, imported native ownership and five real ABI/callback cases.

The complete restored graph exposed an xUnit default on `Inno.Testing.Build`: its package imports set IsTestProject=true unless explicitly overridden. The shared helper now declares IsTestProject=false alongside IsTestSupportProject=true. Actual SDK property evaluation confirms both values; no test execution or architecture assertion was removed. The Bgfx integration owner has now passed all eight actual packaged shader/texture compiler executions on the no-extern consumer; the pure Bgfx owner also passed all thirteen cases in the complete managed run.

The complete clean-consumer run exposed five SceneHistory failures in `Inno.Editor.Interactions.Tests`: the declared reload fixture was built but never copied to `Modules/SceneReload`. Removing the duplicate ProjectReference had discarded its OutputItemType metadata. The single retained reference now carries `OutputItemType=SceneReloadTestModule`, and the empty group is removed. Existing history/recovery assertions and module ownership remain unchanged. The full-verified TRX files retain the first failures.

The focused frozen plan on isolated snapshot 133c035f selected only the Editor Interactions test owner. Instructions, source architecture, compilation and all 48 required cases passed, including the five original failures. The actual fixture DLL exists under the test output's Modules/SceneReload directory. Compilation/evidence capture took 113.729s and execution took 14.851s; no native request was selected. Evidence: producer worktree artifacts/validation/native-acceptance/editor-fixture-fixed. The complete local join for this correction is recorded in the acceptance ledger.


The host-worker adapter has five fixture cases covering sequential execution, isolated concurrent checkouts, independent completion after a peer failure, a missing result and cyclic prerequisites. Eleven gate cases bind the original executor, platform, plan, shard digests and joined results. The existing seven transfer cases continue to cover input ownership when parallel workers bind a portable plan.

### Scheduler performance and discovery ownership

The three `JobSystemPerformanceTests` methods now belong to `tests/core/Inno.Core.Jobs.Performance.Tests`. Timing includes scheduling and completion. Worker-count and throughput assertions run in the performance profile. The domain project retains functional job-system contracts.

The test-only `Inno.Testing.Discovery.TestLogger` records VSTest source, count and isolation traits. Its build-only reference from test projects makes changes visible through their actual SDK closures. The existing isolated test runner owns execution and per-owner receipts. The real ownership regression exercises both failed shared and isolated cases and rejects missing receipts.

## Functional acceptance cleanup on cicd-optimization

Baseline `ed416cb0`. The revised `tests` tree is `b7c13fd5e1f4af6bf35337ae34b89753aecf9450` and the `tools` tree is `4da5291d9a5303d2184247f958dd21cfd6173e92`. The migration table above describes its original snapshot.

The engine retains 2,241 xUnit methods. The cleanup removes redundant setup and API-shape checks, steady-state allocation budgets, repeated product GC sampling and repeated build timing. Focused regressions still cover cross-thread delivery, asset and script ownership, rollback, streaming, UI clipping, native window reuse and retirement. Each real panel reopens and returns to its original state. The extended build check validates one unchanged rebuild.

Windows x64 validation with .NET SDK 10.0.402 passed 155 distinct xUnit cases across targeted runs and the complete Events, Coroutines, Jobs and Graphs suites. Both console input modes and all four clipped panel views passed. The journey runner passed 12 protocol cases, and both historical incident receipt cases passed. The newly built Direct3D11 Editor passed the three-frame sRGB smoke, eight product workflow stages and the lifecycle check for eight panels. One unchanged Editor rebuild reported zero native processes, zero binding generations and stable selections. The style check reported zero findings across 35 changed C# files. The Editor build emitted one deferred cache-maintenance warning about duplicate registry identities. Local results cover Windows x64. macOS and the full hosted matrix require CI evidence.
