# Inno.Build.Tests

[测试索引](README.md) · [Wiki 首页](../README.md) · [当前验收](../architecture/CURRENT_ACCEPTANCE.md)

## 本轮构建边界回归

绑定增量回归使用真实 SDK 检查：新的绑定选择文件不会使中立项目重编译；相同选择保持 import 时间戳；产品选择在 Build/Publish 后仍可核对。生成回归同时检查未选目标映射不失效、选中配置变化仍产生独立 generation。

架构命令路由通过公开 `BuildApplication.Run` 验证；负向源码必须返回失败，不得启动 Task runtime 准备或创建构建产物。规则正反例由架构测试项目直接验证。

`PreparedEditorStartupTests` 在独立进程调用公开 `DevelopmentEntry.RunEditor`：真实准备记录包含精确文件哈希，fixture 应用检查两份读取保护在整个运行期间生效，并分别验证正常退出与异常退出后的释放。同长度、同时间戳的内容篡改仍会阻止启动。该测试不创建真实窗口，不代表 SDL/BGFX 或 GUI 已验收。

Wiki 验收 Host 导入实际产品使用的共享 `EditorProduct.props`，通过公开 Catalog 与 ScriptCompiler 生成逻辑脚本引用。专项测试检查 Settings、Shaders 等已声明的产品扩展确实进入 `InnoEditor.*` 契约，不以手写类型白名单或实现侧 namespace 补齐失败示例。该组合不启动平台窗口或 Native 设备。

## 职责与边界

验证以下项目的公开行为与组合契约。该程序集属于测试闭包，不进入 Editor 或 Player 发行；测试类、fixture 和断言辅助类型不属于稳定产品 API。

- [Inno.Entry.Bootstrap](../entry/Inno.Entry.Bootstrap.md)
- [Inno.Build.Cli](../build/Inno.Build.Cli.md)
- [Inno.Build.Bindings](../build/Inno.Build.Bindings.md)
- [Inno.Build.Toolchains.Bgfx.Tools](../backends/Bgfx/Inno.Build.Toolchains.Bgfx.Tools.md)
- [Inno.Build.Toolchains.Bgfx](../backends/Bgfx/Inno.Build.Toolchains.Bgfx.md)
- [Inno.Integration.Linux.Bgfx](../platform/Linux/Inno.Integration.Linux.Bgfx.md)
- [Inno.Integration.Browser.Bgfx](../platform/Browser/Inno.Integration.Browser.Bgfx.md)
- [Inno.Integration.MacOS.Bgfx](../platform/MacOS/Inno.Integration.MacOS.Bgfx.md)
- [Inno.Integration.Windows.Bgfx](../platform/Windows/Inno.Integration.Windows.Bgfx.md)
- [Inno.Build.Composition](../build/Inno.Build.Composition.md)
- [Inno.Adapter.Content.FileSystem](../backends/FileSystem/Inno.Adapter.Content.FileSystem.md)
- [Inno.Build.Managed.DotNet](../backends/DotNet/Inno.Build.Managed.DotNet.md)
- [Inno.Build.Tasks](../build/Inno.Build.Tasks.md)
- [Inno.Build.TaskHosting](../build/Inno.Build.TaskHosting.md)
- [Inno.Editor.Exporting](../editor/Inno.Editor.Exporting.md)
- [Inno.Editor.Hosting](../editor/Inno.Editor.Hosting.md)
- [Inno.Editor.Core](../editor/Inno.Editor.Core.md)
- [Inno.Editor.Interactions](../editor/Inno.Editor.Interactions.md)
- [Inno.Native.ImGui](../backends/ImGui/Inno.Native.ImGui.md)
- [Inno.Build.Toolchains](../build/Inno.Build.Toolchains.md)
- [Inno.Build.Browser](../platform/Browser/Inno.Build.Browser.md)
- [Inno.Build](../build/Inno.Build.md)
- [Inno.Build.MacOS](../platform/MacOS/Inno.Build.MacOS.md)
- [Inno.Build.Windows](../platform/Windows/Inno.Build.Windows.md)
- [Inno.Assets.Pipeline](../assets/Inno.Assets.Pipeline.md)
- [Inno.Assets](../assets/Inno.Assets.md)
- [Inno.Plugins.Authoring](../plugins/Inno.Plugins.Authoring.md)
- [Inno.Core.Identity](../core/Inno.Core.Identity.md)
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)
- [Inno.Core.Settings](../core/Inno.Core.Settings.md)
- [Inno.Extensibility.Modules](../extensibility/Inno.Extensibility.Modules.md)
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)
- [Inno.Plugins](../plugins/Inno.Plugins.md)
- [Inno.Runtime](../runtime/Inno.Runtime.md)
- [Inno.Scene](../scene/Inno.Scene.md)
- [Inno.Scene.Assets](../scene/Inno.Scene.Assets.md)
- [Inno.Scripting.Compiler](../scripting/Inno.Scripting.Compiler.md)
- [Inno.Build.SupportPacks.Core](../build/Inno.Build.SupportPacks.Core.md)
- [Inno.Adapter.Modules.DotNet](../backends/DotNet/Inno.Adapter.Modules.DotNet.md)
- [Inno.Adapter.Serialization.DotNet](../backends/DotNet/Inno.Adapter.Serialization.DotNet.md)
- [Inno.Build.Distribution.Standard](../build/Inno.Build.Distribution.Standard.md)

## 运行

从仓库根目录运行：

```powershell
dotnet test tests/build/Inno.Build.Tests/Inno.Build.Tests.csproj --configuration Debug --disable-build-servers -m:1 -nodeReuse:false
```

包含 Native 或窗口相关消费；执行前准备匹配目标与配置的原生产物。缺库、缺 SDK 或无法建立真实窗口属于未通过/未执行，不能记为通过。

可以用 `--filter FullyQualifiedName~方法名` 选择下表的场景；完整项目验证仍使用上面的命令。

## 源码场景

下表是当前测试文件索引，并列举各文件的部分入口，完整断言与参数化用例位于链接源码。列出场景不表示本轮已执行；结果只在当前验收中登记。

| 测试文件 | 场景入口示例 |
| --- | --- |
| [InteropRestoreOwnershipTests.cs](../../tests/build/Inno.Build.Tests/InteropRestoreOwnershipTests.cs) | 真实 SDK 求值后的直接/传递 BGCS Runtime 引用仍存在；只有直接引用 owner 负责隔离 restore |
| [BgfxNativeProfileTests.cs](../../tests/build/Inno.Build.Tests/BgfxNativeProfileTests.cs) | `OpenTargetAcceptsItsExplicitSdkInvocationBeforeSourceValidation`、`ForeignTargetIsRejectedBeforeInvokingTheProfile`、`InvocationOwnsAnImmutableArgumentSnapshot` |
| [BindingDefinitionTests.cs](../../tests/build/Inno.Build.Tests/BindingDefinitionTests.cs) | `ExecutableOrNonLiteralDefinitionsFailBeforeOutputCreation`、`UndeclaredTargetNeverFallsBackToTheHostAbi` |
| [BindingGenerationBatchTests.cs](../../tests/build/Inno.Build.Tests/BindingGenerationBatchTests.cs) | `OneBatchReusesSharedReadsOnlyWithinEachFreshPhase`、`ActiveConsumersBlockReplacementAndReleaseAllowsRepair`、`AFailedBatchPublishesNoPartialClosureAndPreservesEarlierOutputs`、`FrozenSdkPathsSelectIndependentHeadersWithoutChangingTheProcessEnvironment` |
| [BindingSelectionLifetimeTests.cs](../../tests/build/Inno.Build.Tests/BindingSelectionLifetimeTests.cs) | `PublicationWithoutItsCompiledSelectionFailsBeforePreparingTheTaskRuntime`、`SelectionRemainsOwnedThroughBuildAndIsReleasedAfterPublish` |
| [BindingToolchainTaskTests.cs](../../tests/build/Inno.Build.Tests/BindingToolchainTaskTests.cs) | `MissingOrCanceledDefinitionCannotCreateGenerationCandidates` |
| [BrowserSupportPackTests.cs](../../tests/build/Inno.Build.Tests/BrowserSupportPackTests.cs) | `BrowserPackRequiresItsStaticGameLinkerClosure` |
| [BrowserToolchainTests.cs](../../tests/build/Inno.Build.Tests/BrowserToolchainTests.cs) | `EnvironmentUsesTheProjectSelectedToolsWithoutChangingTheParent`、`EnvironmentRejectsAnIncompleteSelectedToolchain`、`EnvironmentRejectsAProjectWithoutWorkloadSelection`、`CapturePreservesStructuredArgumentsAndChildOnlyEnvironment` |
| [BuildCompositionTests.cs](../../tests/build/Inno.Build.Tests/BuildCompositionTests.cs) | `EveryHostObtainsTheSameFrozenBuiltInDistribution`、`TargetsContributeDistinctImmutableProductClosuresWithoutChangingTheInvokingHost`、`CustomTargetComposesAndRejectsInconsistentDefaultsBeforePublication`、`DistributionRejectsAmbiguousOrIncompleteRegistrationSets` |
| [BuildOutputOwnershipTests.cs](../../tests/build/Inno.Build.Tests/BuildOutputOwnershipTests.cs) | `ConcurrentSdkRequestsKeepExplicitOutputOwnersForTheSameTarget` |
| [BuildPipelineTests.cs](../../tests/build/Inno.Build.Tests/BuildPipelineTests.cs) | `ProjectSettingsUseTheHostSuppliedInitialIdentity`、`PlatformPackRejectsAMissingContentSource`、`BuildTargetsAreOpenIdsOwnedByTheRegisteredAdapterCatalog`、`BuildTargetInventoryCannotBeMutatedThroughItsCollectionView` |
| [DevelopmentEntryCommandTests.cs](../../tests/build/Inno.Build.Tests/DevelopmentEntryCommandTests.cs) | `InvalidCommandsFailBeforeSdkDiscoveryOrCleanup`、`CancellationReturnsBeforeCreatingBuildState` |
| [DotNetSdkEnvironmentTests.cs](../../tests/build/Inno.Build.Tests/DotNetSdkEnvironmentTests.cs) | `SelectedSdkIgnoresAnInvokingBuildsSdkIdentityAndPreservesItsNativeEnvironment` |
| [EditorShaderProfileTests.cs](../../tests/build/Inno.Build.Tests/EditorShaderProfileTests.cs) | `SdkAddedReferencesRetainTheProductTargetAndShaderProfile`、`SharedCompositionShadersConsumeExplicitProductFacts`、`BackendConsumesAProductProfileWithoutSelectingAKnownNativeTarget` |
| [FileProductPublicationTests.cs](../../tests/build/Inno.Build.Tests/FileProductPublicationTests.cs) | `SingleFilePublicationRetainsTheNativeClosureAndRejectsMissingInputs` |
| [GameContentCompilerBindingTests.cs](../../tests/build/Inno.Build.Tests/GameContentCompilerBindingTests.cs) | `BindingRejectsMissingOrMismatchedComponentsBeforeAnyWork`、`ContentCompilerCanBeReplacedWithoutChangingPlatformPackaging` |
| [InteropRuntimeIsolationTests.cs](../../tests/build/Inno.Build.Tests/InteropRuntimeIsolationTests.cs) | `ExternalConsumerBuildPreparesItsIsolatedRuntimeRestore`、`SdkAddedRuntimeReferencesRetainConsumerIsolation`、`RestoreRetainsTheReferencedRuntimeArtifactProfile`、`ReferencedRuntimeSeparatesSdkTargetAbiAndCompilationProfiles` |
| [ManagedDeploymentTests.cs](../../tests/build/Inno.Build.Tests/ManagedDeploymentTests.cs) | `OpenProviderCatalogDispatchesCustomIdentityWithoutPlatformBranches`、`CapabilityInputsAndPublicationArtifactsAreImmutable` |
| [NativeArtifactIdentityTests.cs](../../tests/build/Inno.Build.Tests/NativeArtifactIdentityTests.cs) | `MultipurposeRecipeOwnerIncludesOnlyItsDeclaredImplementationClosure`、`FingerprintPreservesDeclarationOrderAndChangesForSourceOrSdkIdentity`、`LogicalInputsCanMoveWithoutChangingIdentityAndSameTimestampTamperingInvalidatesIt`、`ManifestChecksIdentityBytesAndTheCompleteOutputSet` |
| [NativeArtifactPublicationTests.cs](../../tests/build/Inno.Build.Tests/NativeArtifactPublicationTests.cs) | `RepairWaitsForReadersAndCancellationPreservesThePinnedDirectory`、`MaterializationPreservesIdenticalTimestampsAndRejectsChangesAfterTheSourceSnapshot`、`CanceledMaterializationPreservesTheOwnedIntermediate`、`SdkDirectoryAliasesContributeContentAndDirectoryCyclesFailBeforeProducingOutput` |
| [NativeBindingCompilationTests.cs](../../tests/build/Inno.Build.Tests/NativeBindingCompilationTests.cs) | `TargetCompilationReplacesHostSourceAndRejectsAMissingSelection` |
| [NativeBindingGenerationTests.cs](../../tests/build/Inno.Build.Tests/NativeBindingGenerationTests.cs) | `ManagedSelectionPreservesExactNativeIdentitiesInStableProjectOrder`、`PublicationRejectsChangedOrIncompleteManagedSelection`、`CollidingManagedSelectionDoesNotReplaceAnExistingCompleteSelection`、`UnsafeManagedProjectNameIsRejectedBeforeCreatingSelectionOutput` |
| [NativeBuildContextTests.cs](../../tests/build/Inno.Build.Tests/NativeBuildContextTests.cs) | `SelectedCheckoutOwnsIntermediatesEvenWhenTheToolRunsFromAnotherCheckout`、`ContextRejectsMissingCheckoutAndUnsupportedConfiguration`、`CanceledBuildDoesNotStartAComponentOrCreateIntermediates`、`UnresolvedToolchainsFailBeforeIntermediatesOrSourceDiscovery` |
| [NativeComponentBuildOptionsTests.cs](../../tests/build/Inno.Build.Tests/NativeComponentBuildOptionsTests.cs) | `OptionsFreezeOrderedDefinitionsAndRejectAmbiguousSdkOverrides`、`RecipeOwnedDefinitionsCannotBeReplacedBeforeAnyToolStarts`、`ProductPlanDeclaresStaticArchivesAndDoesNotGuessLinkageFromTheHost`、`LinkageOrderedArgumentsAndConfigurationBytesAllChangeRecipeIdentity` |
| [NativeVerificationPhaseTests.cs](../../tests/build/Inno.Build.Tests/NativeVerificationPhaseTests.cs) | `FreshPhasesDetectSameLengthSameTimestampChangesAndReuseSharedReads` |
| [NativeRecipeClosureTests.cs](../../tests/build/Inno.Build.Tests/NativeRecipeClosureTests.cs) | 无生成输出时声明源码输入；缺 generation 在执行前失败；最终绑定字节与 generation identity 变化使产物失效，热命中不重复 producer |
| [NativeWorkspaceReuseTests.cs](../../tests/build/Inno.Build.Tests/NativeWorkspaceReuseTests.cs) | 内容变化与不兼容参数的 workspace 选择；Windows 实际 CMake 编译验证同长度、同 mtime 修改、未改变 `.obj` 复用及第三次缓存命中 |
| [OwnedRestoreTests.cs](../../tests/build/Inno.Build.Tests/OwnedRestoreTests.cs) | `EmptyProjectSelectionFailsBeforeTakingOwnership`、`ExactPropertiesAreForwardedAndFailureReleasesOwnership`、`RestoreRemovesTheParentSolutionEntryPoint` |
| [PlatformBackendIntegrationTests.cs](../../tests/build/Inno.Build.Tests/PlatformBackendIntegrationTests.cs) | `IntegrationsPreserveExactShaderAndNativeTargetIdentities` |
| [PlatformOwnershipContractTests.cs](../../tests/build/Inno.Build.Tests/PlatformOwnershipContractTests.cs) | `RealPlatformPreflightRejectsUnavailableSdkOrHostWithoutCreatingOutput`、`StaticComponentsHaveOneAggregateOwnerAndNoUnusedStandaloneProducer`、`StandardShaderTargetIsIndependentOfTheOfflineToolTarget`、`DistributionDoesNotDeriveProductTargetsFromTheToolHost` |
| [StaticGraphRestoreTests.cs](../../tests/build/Inno.Build.Tests/StaticGraphRestoreTests.cs) | `ProductRestoreAcceptsLargeInheritedProperties` |
| [SupportPackPublisherTests.cs](../../tests/build/Inno.Build.Tests/SupportPackPublisherTests.cs) | `PreflightFailuresLeaveNoPublicationDirectory`、`CancellationDuringPlanningLeavesNoOutputAndExecutesNoPlan`、`PreparedPlanIsExecutedOnceWithoutRepeatingDiscovery`、`RegisteredCustomTargetInstallsWithoutCorePlatformBranches` |
| [TaskHostPublicationTests.cs](../../tests/build/Inno.Build.Tests/TaskHostPublicationTests.cs) | `RepeatedPublicationReusesOneVerifiedRuntimeWithoutChangingItsFiles`、`ChangedSelectedSourceCannotPublishOrReplaceAnExistingRuntime`、`CacheValidationDetectsSameLengthAndTimestampTamperingAndExtraPayload`、`CanceledBootstrapNeverCreatesACacheOrReader` |
| [TaskRuntimeBuildTests.cs](../../tests/build/Inno.Build.Tests/TaskRuntimeBuildTests.cs) | `SharedBuildsYieldWhileWaitingAndSerializeWrites`、`CancellationReleasesWaitAndAllowsTheNextBuild`、`FailedBuildDoesNotRetainSharedOwnership`、`InvalidPropertiesFailBeforeCreatingSharedOutput` |
| [ToolchainEnvironmentTests.cs](../../tests/build/Inno.Build.Tests/ToolchainEnvironmentTests.cs) | `NativeBuildRootBelongsToTheToolchainProject`、`NativeBuildRootRejectsAnUnrelatedOwner`、`NativeBuildRootRequiresAnOwner` |
| [ToolchainProcessTests.cs](../../tests/build/Inno.Build.Tests/ToolchainProcessTests.cs) | `CancellationTerminatesTheActiveProcessAndItsChild`、`RawRunnerRejectsFailedAndAlreadyCanceledTools`、`OutputFailureTerminatesTheActiveProcessAndItsChild` |
| [ToolchainWorkingDirectoryTests.cs](../../tests/build/Inno.Build.Tests/ToolchainWorkingDirectoryTests.cs) | `BoundedExecutionRetainsFilesAndStableIdentityAcrossOwners`、`CanceledWaitDoesNotRemoveAnotherOperationsExecutionPath`、`OccupiedAliasFailsWithoutRemovingTheOccupantAndReleasesOwnership`、`RelativePathsAreRejectedBeforeCreatingAnAlias` |

## 生命周期与失败

`RepositoryWikiTests` 是全仓当前 Wiki 的持续门禁，随现有完整测试套件执行。它在自己拥有的空项目中通过公开 ScriptCompiler 生成当前逻辑脚本引用，然后调用公开 BuildApplication 的 `verify --wiki`。API 和示例清单保存在 `artifacts/acceptance/wiki-gate/<operation>/`；临时项目在测试结束后释放，不修改用户项目。该测试串行进入构建/序列化测试集合，避免同时运行多个创作态全局上下文。

`TaskPublisherBootstrapTests` 使用两次真实 SDK 构建验证 publisher 编译复用、并行引用闭包、私有加载目录隔离和新的根构建 owner；`InteropRestoreOwnershipTests` 验证直接引用与 SDK 传递引用各自的 restore 职责。

测试通过公开入口及可替换契约检查行为，fixture 只负责自身创建的会话、临时目录和子进程。需要窗口、设备或 owner thread 的测试应保持其声明的执行线程与释放顺序。取消或失败后检查相应断言与清理结果，不通过删除共享锁文件解除等待。

## 扩展与维护

新增行为用例放在最接近所属契约的测试文件。测试不能依赖反射穿透、`InternalsVisibleTo` 或扩大的生产可见性。修改契约后同时核对本项目及其直接生产依赖页面；测试中的具体类型不加入游戏脚本导出清单。

## 相邻页面

- [项目文件](../../tests/build/Inno.Build.Tests/Inno.Build.Tests.csproj)：框架、引用和实际构建配置。
- [架构索引](../architecture/README.md)：所属层及禁止依赖。
- [当前验收](../architecture/CURRENT_ACCEPTANCE.md)：实际运行状态与环境限制。

## Support Pack 准备前输入与缓存

- ManagedBuildInputSnapshotTests：真实 SDK 求值、相同输入复用、同长度同 mtime 的源码/资源/import 修改、新增源码以及请求目录清理。
- NativeRecipeClosureTests：producer 与纯输入声明一同注册、foreign owner/target 拒绝，规划不执行编译。
- BindingGenerationBatchTests：当前 config/继承/headers 输入身份、未选目标配置不失效、无需生成输出。
- SupportPackPreparationCacheTests：并发只准备一次、命中跳过 producer、损坏/额外输出修复、等待后或 validation 中输入变化拒绝；已安装 current 保持。

这些契约 fixture 与 SDK 图测试不能代替实际 Player/Web 发布、GUI 或性能证据。

## 缓存退休

`BuildCacheRetirementTests` 通过公开 owner、提案及 FileLease 协议验证跨 owner 依赖保护、活动 reader、候选外活动父产物、预览后保护根变化、忙碌 owner、清单变化、取消、时间预算和无效所有权。测试还检查持久锁文件与未登记用户目录保持存在。它验证退休基础协议，不代表标准发行和自动回收已经接入。
