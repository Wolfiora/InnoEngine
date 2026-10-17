# Inno.Rendering.Runtime.Tests

[测试索引](README.md) · [Wiki 首页](../README.md) · [当前验收](../architecture/CURRENT_ACCEPTANCE.md)

## 职责与边界

验证以下项目的公开行为与组合契约。该程序集属于测试闭包，不进入 Editor 或 Player 发行；测试类、fixture 和断言辅助类型不属于稳定产品 API。

- [Inno.Content.Testing 项目](../../tests/content/fixtures/Inno.Content.Testing/Inno.Content.Testing.csproj)
- [Inno.Core.Execution](../core/Inno.Core.Execution.md)
- [Inno.Runtime.Contracts](../runtime/Inno.Runtime.Contracts.md)
- [Inno.Assets](../assets/Inno.Assets.md)
- [Inno.Extensibility.Modules](../extensibility/Inno.Extensibility.Modules.md)
- [Inno.Core.Identity](../core/Inno.Core.Identity.md)
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)
- [Inno.Rendering.Runtime](../rendering/Inno.Rendering.Runtime.md)
- [Inno.Rendering](../rendering/Inno.Rendering.md)
- [Inno.Rendering.Runtime.Reload.TestModule 项目](../../tests/rendering/fixtures/Inno.Rendering.Runtime.Reload.TestModule/Inno.Rendering.Runtime.Reload.TestModule.csproj)
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)
- [Inno.References](../references/Inno.References.md)
- [Inno.Adapter.Modules.DotNet](../backends/DotNet/Inno.Adapter.Modules.DotNet.md)
- [Inno.Adapter.Serialization.DotNet](../backends/DotNet/Inno.Adapter.Serialization.DotNet.md)
- [Inno.Rendering.Assets](../rendering/Inno.Rendering.Assets.md)

## 运行

从仓库根目录运行：

```powershell
dotnet test tests/rendering/Inno.Rendering.Runtime.Tests/Inno.Rendering.Runtime.Tests.csproj --configuration Debug --disable-build-servers -m:1 -nodeReuse:false
```

使用项目声明的 SDK、包引用与 fixture。需要启动外部工具或子进程的场景，应按照对应源码列出的前置条件准备环境。

可以用 `--filter FullyQualifiedName~方法名` 选择下表的场景；完整项目验证仍使用上面的命令。

## 源码场景

下表是当前测试文件索引，并列举各文件的部分入口，完整断言与参数化用例位于链接源码。列出场景不表示本轮已执行；结果只在当前验收中登记。

| 测试文件 | 场景入口示例 |
| --- | --- |
| [ContentRenderTargetArtifactProviderTests.cs](../../tests/rendering/Inno.Rendering.Runtime.Tests/ContentRenderTargetArtifactProviderTests.cs) | `TextureTargetArtifactLoadsWithoutAnyAuthoringSourceMount`、`MissingDeployedTextureArtifactIsReportedAsUnavailable`、`IndependentTextureSlotsUseStableDistinctDeploymentPaths` |
| [EmptyRenderingKernelTests.cs](../../tests/rendering/Inno.Rendering.Runtime.Tests/EmptyRenderingKernelTests.cs) | `OutputRouteAssignsEachWorldContentSourceOnlyOnce`、`ProductionRenderingAssemblyDoesNotDeclareConcreteRenderingWorldviews`、`OpenShaderProtocolsRemainBackendNeutral`、`GeometryCandidatePublishesACompletePairAndPreservesOldPairAfterPartialAllocationFailure` |
| [RenderRuntimeGenerationTests.Contributors.cs](../../tests/rendering/Inno.Rendering.Runtime.Tests/RenderRuntimeGenerationTests.Contributors.cs) | `CompletedOrRejectedFramesReleaseCollectibleSnapshotScratchAndGraphCallbacks`、`ContributorRegistrationChangesTakeEffectOnTheNextFrame`、`FailedContributorDoesNotPublishResourcesPassesOrPersistentDiagnostics`、`UnresolvedModelDiagnosticRemainsUntilTheCauseIsResolved` |
| [RenderRuntimeGenerationTests.Presentation.cs](../../tests/rendering/Inno.Rendering.Runtime.Tests/RenderRuntimeGenerationTests.Presentation.cs) | `UnavailablePrimaryOutputSkipsInputScalingAndKeepsOffscreenWork`、`RestoringPrimaryOutputPublishesRealExtentToTheNextCompleteFrame`、`InvalidBackendExtentFailsAndClosesTheDeviceFrame`、`UnavailableLogicalInputSurfaceDoesNotDispatchPrimaryInteraction` |
| [ShaderProgramPublicationTests.cs](../../tests/rendering/Inno.Rendering.Runtime.Tests/ShaderProgramPublicationTests.cs) | `ExplicitArtifactScopesDoNotPublishIntoCanonicalMaterialCachesAndReleaseIndependently`、`InvalidDraftProgramKeepsOnlyItsOwnLastGoodAndReportsOnlyToItsConsumer`、`ShaderProgramsFollowPublishedContentNotTheLatestAssetDefinitionOrItsVersion`、`AllPreviouslyUsedPassesMustBeCreatedBeforeAnyReplacementIsPublished` |

## 生命周期与失败

测试通过公开入口及可替换契约检查行为，fixture 只负责自身创建的会话、临时目录和子进程。需要窗口、设备或 owner thread 的测试应保持其声明的执行线程与释放顺序。取消或失败后检查相应断言与清理结果，不通过删除共享锁文件解除等待。

## 扩展与维护

新增行为用例放在最接近所属契约的测试文件。测试不能依赖反射穿透、`InternalsVisibleTo` 或扩大的生产可见性。修改契约后同时核对本项目及其直接生产依赖页面；测试中的具体类型不加入游戏脚本导出清单。

## 相邻页面

- [项目文件](../../tests/rendering/Inno.Rendering.Runtime.Tests/Inno.Rendering.Runtime.Tests.csproj)：框架、引用和实际构建配置。
- [架构索引](../architecture/README.md)：所属层及禁止依赖。
- [当前验收](../architecture/CURRENT_ACCEPTANCE.md)：实际运行状态与环境限制。
