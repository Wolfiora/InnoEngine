# Inno.Editor.Scripting.Tests

原生 UI fixture 显式关闭默认 `imgui.ini` 自动持久化；布局与状态断言仍使用原来的内存／独立临时输入。测试不得向编译输出目录写入运行状态；需要文件持久化的场景由各自临时 owner 持有。

[测试与 fixture 索引](README.md)

[测试索引](README.md) · [Wiki 首页](../../README.md) · [当前验收](../../../.agents/records/acceptance/current.md)

## 职责与边界

验证以下项目的公开行为与组合契约。该程序集属于测试闭包，不进入 Editor 或 Player 发行；测试类、fixture 和断言辅助类型不属于稳定产品 API。

- [Inno.Editor.Annotations](../../modules/editor/Inno.Editor.Annotations.md)
- [Inno.Editor.Panel.ShaderEditor](../../modules/editor/Inno.Editor.Panel.ShaderEditor.md)
- [Inno.Editor.Shaders](../../modules/editor/Inno.Editor.Shaders.md)
- [Inno.Editor.Panel.FileBrowser](../../modules/editor/Inno.Editor.Panel.FileBrowser.md)
- [Inno.Editor.Graph](../../modules/editor/Inno.Editor.Graph.md)
- [Inno.Rendering.Assets](../../modules/rendering/Inno.Rendering.Assets.md)
- [Inno.Build.Toolchains.Bgfx.Tools](../../backends/Bgfx/Inno.Build.Toolchains.Bgfx.Tools.md)
- [Inno.Rendering.Shaders](../../modules/rendering/Inno.Rendering.Shaders.md)
- [Inno.Core.Diagnostics](../../modules/core/Inno.Core.Diagnostics.md)
- [Inno.Core.Graphs](../../modules/core/Inno.Core.Graphs.md)
- [Inno.Editor.Scripting](../../modules/editor/Inno.Editor.Scripting.md)
- [Inno.Editor.Core](../../modules/editor/Inno.Editor.Core.md)
- [Inno.Editor.ImGui](../../modules/editor/Inno.Editor.ImGui.md)
- [Inno.Editor.Inspection](../../modules/editor/Inno.Editor.Inspection.md)
- [Inno.Editor.Interactions](../../modules/editor/Inno.Editor.Interactions.md)
- [Inno.Editor.PlayMode](../../modules/editor/Inno.Editor.PlayMode.md)
- [Inno.Editor.Rendering](../../modules/editor/Inno.Editor.Rendering.md)
- [Inno.Editor.Scene](../../modules/editor/Inno.Editor.Scene.md)
- [Inno.Editor.Settings](../../modules/editor/Inno.Editor.Settings.md)
- [Inno.Assets.Pipeline](../../modules/assets/Inno.Assets.Pipeline.md)
- [Inno.Plugins.Authoring](../../modules/plugins/Inno.Plugins.Authoring.md)
- [Inno.Assets](../../modules/assets/Inno.Assets.md)
- [Inno.Extensibility.Modules](../../modules/extensibility/Inno.Extensibility.Modules.md)
- [Inno.Core.Identity](../../modules/core/Inno.Core.Identity.md)
- [Inno.Core.Serialization](../../modules/core/Inno.Core.Serialization.md)
- [Inno.Core.Logging](../../modules/core/Inno.Core.Logging.md)
- [Inno.Core.Settings](../../modules/core/Inno.Core.Settings.md)
- [Inno.References](../../modules/references/Inno.References.md)
- [Inno.Runtime](../../modules/runtime/Inno.Runtime.md)
- [Inno.Scene](../../modules/scene/Inno.Scene.md)
- [Inno.Scripting.Compiler](../../modules/scripting/Inno.Scripting.Compiler.md)
- [Inno.Scripting.Reload](../../modules/scripting/Inno.Scripting.Reload.md)
- [Inno.Extensibility.Reload](../../modules/extensibility/Inno.Extensibility.Reload.md)
- [Inno.Adapter.Modules.DotNet](../../backends/DotNet/Inno.Adapter.Modules.DotNet.md)
- [Inno.Adapter.Serialization.DotNet](../../backends/DotNet/Inno.Adapter.Serialization.DotNet.md)
- [Inno.Rendering.Assets.Authoring](../../modules/rendering/Inno.Rendering.Assets.Authoring.md)
- [Inno.Rendering.Runtime](../../modules/rendering/Inno.Rendering.Runtime.md)

## 运行

通过[统一 Native 验证入口](README.md#native-validation)生成并执行实际修改的计划；该入口准备目标产品、部署索引及隔离测试进程，不依赖已有 bin 中的偶然文件。

包含 Native 或窗口相关消费；执行前准备匹配目标与配置的原生产物。缺库、缺 SDK 或无法建立真实窗口属于未通过/未执行，不能记为通过。

定位失败时可使用计划中已求值的程序集与 SDK 参数执行局部诊断；局部过滤结果不能代替计划要求的完整 owner 证据。

## 源码场景

下表是当前测试文件索引，并列举各文件的部分入口，完整断言与参数化用例位于链接源码。列出场景不表示本轮已执行；结果只在当前验收中登记。

| 测试文件 | 场景入口示例 |
| --- | --- |
| [EditorStyleMetricsTests.cs](../../../tests/scripting/Inno.Editor.Scripting.Tests/EditorStyleMetricsTests.cs) | `ZoomScalesDimensionsAndFontWhilePreservingRatios`、`ZoomCommandsClampAndResetToTheSupportedRange`、`WrappedTextRendersThroughTheSafeLiteralTextPath`、`CenteredWrappedTextStaysInsidePaddingAndUsesMultipleLines` |
| [EditorViewportCompositionTests.cs](../../../tests/scripting/Inno.Editor.Scripting.Tests/EditorViewportCompositionTests.cs) | `CompositionCanonicalizesModelLayersByOrderAndStableIdentity`、`CompositionSnapshotsTheCallerCollection`、`CompositionRejectsEmptyNullAndDuplicateModelLayers` |
| [EditorViewportNavigationTests.cs](../../../tests/scripting/Inno.Editor.Scripting.Tests/EditorViewportNavigationTests.cs) | `Profile_RejectsDefaultModeMissingFromCapabilities`、`ConfigurePerspective_IsAtomicWhenValidationFails`、`State_RejectsUndefinedProjectionAndMode` |
| [InspectionPipelineTests.cs](../../../tests/scripting/Inno.Editor.Scripting.Tests/InspectionPipelineTests.cs) | `InlineChildFailureIsConsumedAndReadonlyDisabledScopeRemainsBalanced`、`PanelWindowFailureStillEndsTheWindowAndAllowsLaterContent`、`TextEditStateIsScopedByRendererOwnerAndPropertyPath`、`PresentationAttributesComposeAndCustomDrawersRemainExtensible` |
| [InspectorPresentationLayoutTests.cs](../../../tests/scripting/Inno.Editor.Scripting.Tests/InspectorPresentationLayoutTests.cs) | `FloatingPanelCloseButtonClosesOnMouseRelease`、`TooltipAtBottomRightFitsParentViewportAndAllText`、`HelpBoxUsesWrappedCardHeightWithoutHorizontalOverflow`、`RoundedWindowTitleAndBodyHaveContinuousOpaqueCoverage` |
| [ProjectSettingsExecutionContextTests.cs](../../../tests/scripting/Inno.Editor.Scripting.Tests/ProjectSettingsExecutionContextTests.cs) | `Settings_WithoutAnExecutionScope_FailsExplicitly`、`NestedScopes_RestoreThePreviousSettingsLookup`、`Scopes_DisposedOutOfOrder_AreRejectedWithoutLosingTheActiveLookup`、`ParallelAsyncFlows_ResolveTheirOwnSettingsLookups` |
| [ScriptingPipelineTests.cs](../../../tests/scripting/Inno.Editor.Scripting.Tests/ScriptingPipelineTests.cs) | `SourceLocalAssetPathsUseCompileTimeOwnershipAcrossDeployments`、`SourceLocalAssetPathsFollowInstalledPluginOwnership`、`SourceLocalAssetPathsPreserveCallerSemanticsInIdeReferencesAndProjectMapping`、`GeneratedRegistrationUsesTargetFrameworkReferenceContracts` |
| [ShaderEditorWorkflowTests.cs](../../../tests/scripting/Inno.Editor.Scripting.Tests/ShaderEditorWorkflowTests.cs) | `ParameterPresentationRoundTripsWithoutChangingTheRuntimeDefinitionOrSemanticProgram`、`ParameterPresentationRejectsInvalidBoundsBeforeMutatingTheGraph`、`DiagnosticCheckUsesTheStandardTransientModalWithoutChangingFileSelectionOrHistory`、`SourceSettingsUndoRedoRestoresValidAndInvalidImportStates` |

## 生命周期与失败

测试通过公开入口及可替换契约检查行为，fixture 只负责自身创建的会话、临时目录和子进程。需要窗口、设备或 owner thread 的测试应保持其声明的执行线程与释放顺序。取消或失败后检查相应断言与清理结果，不通过删除共享锁文件解除等待。

## 扩展与维护

新增行为用例放在最接近所属契约的测试文件。测试不能依赖反射穿透、`InternalsVisibleTo` 或扩大的生产可见性。修改契约后同时核对本项目及其直接生产依赖页面；测试中的具体类型不加入游戏脚本导出清单。

## 相邻页面

- [项目文件](../../../tests/scripting/Inno.Editor.Scripting.Tests/Inno.Editor.Scripting.Tests.csproj)：框架、引用和实际构建配置。
- [架构索引](../../architecture/README.md)：所属层及禁止依赖。
- [当前验收](../../../.agents/records/acceptance/current.md)：实际运行状态与环境限制。
