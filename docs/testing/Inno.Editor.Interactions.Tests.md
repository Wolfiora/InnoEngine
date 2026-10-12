# Inno.Editor.Interactions.Tests

[测试索引](README.md) · [Wiki 首页](../README.md) · [当前验收](../architecture/CURRENT_ACCEPTANCE.md)

## 职责与边界

验证以下项目的公开行为与组合契约。该程序集属于测试闭包，不进入 Editor 或 Player 发行；测试类、fixture 和断言辅助类型不属于稳定产品 API。

- [Inno.Extensibility.Reload](../extensibility/Inno.Extensibility.Reload.md)
- [Inno.Scene.Reload.TestModule 项目](../../tests/scene/fixtures/Inno.Scene.Reload.TestModule/Inno.Scene.Reload.TestModule.csproj)
- [Inno.Core.Execution](../core/Inno.Core.Execution.md)
- [Inno.Assets](../assets/Inno.Assets.md)
- [Inno.Assets.Pipeline](../assets/Inno.Assets.Pipeline.md)
- [Inno.Core.Identity](../core/Inno.Core.Identity.md)
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)
- [Inno.Core.Logging](../core/Inno.Core.Logging.md)
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)
- [Inno.Core.Settings](../core/Inno.Core.Settings.md)
- [Inno.References](../references/Inno.References.md)
- [Inno.Editor.Scene](../editor/Inno.Editor.Scene.md)
- [Inno.Editor.Diagnostics](../editor/Inno.Editor.Diagnostics.md)
- [Inno.Editor.PlayMode](../editor/Inno.Editor.PlayMode.md)
- [Inno.Editor.Settings](../editor/Inno.Editor.Settings.md)
- [Inno.Editor.Panel.FileBrowser](../editor/Inno.Editor.Panel.FileBrowser.md)
- [Inno.Editor.Panel.Global](../editor/Inno.Editor.Panel.Global.md)
- [Inno.Editor.Panel.Logging](../editor/Inno.Editor.Panel.Logging.md)
- [Inno.Extensibility.Modules](../extensibility/Inno.Extensibility.Modules.md)
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)
- [Inno.Scene](../scene/Inno.Scene.md)
- [Inno.Scene.Assets](../scene/Inno.Scene.Assets.md)
- [Inno.Runtime](../runtime/Inno.Runtime.md)
- [Inno.Editor.Interactions](../editor/Inno.Editor.Interactions.md)
- [Inno.Editor.Core](../editor/Inno.Editor.Core.md)
- [Inno.Editor.ImGui](../editor/Inno.Editor.ImGui.md)
- [Inno.Adapter.Modules.DotNet](../backends/DotNet/Inno.Adapter.Modules.DotNet.md)
- [Inno.Adapter.Serialization.DotNet](../backends/DotNet/Inno.Adapter.Serialization.DotNet.md)

## 运行

从仓库根目录运行：

```powershell
dotnet test tests/editor/Inno.Editor.Interactions.Tests/Inno.Editor.Interactions.Tests.csproj --configuration Debug --disable-build-servers -m:1 -nodeReuse:false
```

包含 Native 或窗口相关消费；执行前准备匹配目标与配置的原生产物。缺库、缺 SDK 或无法建立真实窗口属于未通过/未执行，不能记为通过。

可以用 `--filter FullyQualifiedName~方法名` 选择下表的场景；完整项目验证仍使用上面的命令。

## 源码场景

下表是当前测试文件索引，并列举各文件的部分入口，完整断言与参数化用例位于链接源码。列出场景不表示本轮已执行；结果只在当前验收中登记。

| 测试文件 | 场景入口示例 |
| --- | --- |
| [AssetHistoryTests.cs](../../tests/editor/Inno.Editor.Interactions.Tests/AssetHistoryTests.cs) | `CreateFolderUndoAndRedoUseTheAssetDatabase`、`DeleteFolderUndoRestoresIdentityMetadataAndEmptyDescendants`、`AssetDeleteHistorySurvivesATypeCatalogRefresh`、`FileBrowserDropMovesFilesAndFoldersWithUndoRedo` |
| [EditorHistoryTests.cs](../../tests/editor/Inno.Editor.Interactions.Tests/EditorHistoryTests.cs) | `ExecuteUndoAndRedoMoveNeutralChangeBetweenStacks`、`FailedTransitionRemainsAvailableForRetry`、`AdjacentChangesWithTheSameStableMergeKeyCoalesce`、`TransactionCommitsAndRevertsAtomically` |
| [EditorPlanarNavigationTests.cs](../../tests/editor/Inno.Editor.Interactions.Tests/EditorPlanarNavigationTests.cs) | `PanOwnsPointerOutsideCanvasAndReleasesWithoutLocalReleaseEvent`、`AltOrbitDoesNotStealPanAndCancelReleasesCapture`、`ZoomPreservesPointerAnchorAndIsIndependentOfDisplayDensity` |
| [EditorRuntimeTests.cs](../../tests/editor/Inno.Editor.Interactions.Tests/EditorRuntimeTests.cs) | `FeatureLookupReturnsOnlyStartedNonQuarantinedModules`、`HistoryAndExtensionStateContractsDoNotExposeStandaloneWorkspaceTypes`、`EditorDensityDefaultsToComfortableAndCanSwitchIdempotently`、`InteractionRuntimeInjectsOneStableAssignableHostService` |
| [EditorSettingsTests.cs](../../tests/editor/Inno.Editor.Interactions.Tests/EditorSettingsTests.cs) | `ApplyPersistsObjectsAtTheProjectRootAndPublishesOnlyTheService`、`ChangedSubscriberFailureDoesNotChangeHistoryResultOrSkipLaterSubscribers`、`GetReturnsIsolatedObjectsAndRejectsPagesOrMissingPaths`、`ResetRemovesThePathOverrideAndRestoresTheDefinitionObject` |
| [SceneHistoryTests.cs](../../tests/editor/Inno.Editor.Interactions.Tests/SceneHistoryTests.cs) | `PropertyUndoRestoresOnlyThePropertyOnTheExistingSceneGraph`、`PropertyUndoRestoresEverySiblingChangedByThePrimarySetter`、`TransformManipulationTransactionUndoesAllLocalValuesAtomically`、`WorldPositionEditRestoresLocalStorageAndWorldResultThroughHistory` |

## 生命周期与失败

测试通过公开入口及可替换契约检查行为，fixture 只负责自身创建的会话、临时目录和子进程。需要窗口、设备或 owner thread 的测试应保持其声明的执行线程与释放顺序。取消或失败后检查相应断言与清理结果，不通过删除共享锁文件解除等待。

## 扩展与维护

新增行为用例放在最接近所属契约的测试文件。测试不能依赖反射穿透、`InternalsVisibleTo` 或扩大的生产可见性。修改契约后同时核对本项目及其直接生产依赖页面；测试中的具体类型不加入游戏脚本导出清单。

## 相邻页面

- [项目文件](../../tests/editor/Inno.Editor.Interactions.Tests/Inno.Editor.Interactions.Tests.csproj)：框架、引用和实际构建配置。
- [架构索引](../architecture/README.md)：所属层及禁止依赖。
- [当前验收](../architecture/CURRENT_ACCEPTANCE.md)：实际运行状态与环境限制。
