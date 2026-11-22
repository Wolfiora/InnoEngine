using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;
using Inno.Adapter.Modules.DotNet;
using Inno.Core.Diagnostics;
using Inno.Core.Events;
using Inno.Core.Identity;
using Inno.Core.Input;
using Inno.Core.Logging;
using Inno.Core.Mathematics;
using Inno.Editor.Core;
using Inno.Editor.ImGui;
using Inno.Editor.Interactions;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Reload;
using Inno.Extensibility.Types;

namespace Inno.Editor.Core.Tests;

public sealed partial class EditorRuntimeTests : IDisposable
{
    private readonly string m_projectRoot = Path.Combine(Path.GetTempPath(), "InnoEditorRuntimeTests", Guid.NewGuid().ToString("N"));
    private readonly DiagnosticHub m_diagnostics = new();
    private readonly LogRouter m_logs = new();
    private readonly IdentityAllocator m_identities = new();
    private readonly IdentityAllocator m_secondaryIdentities = new();
    private readonly IDisposable m_diagnosticScope;
    private readonly ModuleHost m_modules;
    private readonly TypeCatalog m_types;
    private readonly EditorInteractionRuntime m_runtime;

    public EditorRuntimeTests()
    {
        Directory.CreateDirectory(Path.Combine(m_projectRoot, "Assets"));
        m_diagnosticScope = m_diagnostics.EnterScope();
        m_modules = new ModuleHost(
            new ModuleHostOptions { catalogSource = new DotNetAssemblyCatalogSource(typeof(EditorRuntimeTests).Assembly) }
        );
        m_types = new TypeCatalog(m_modules, new ReflectionTypeCatalogSource());
        TestModule.startCount = 0;
        TestModule.stopCount = 0;
        TestModule.stateValue = 0;
        TestModule.restoredStateValue = 0;
        TestModule.rebuildDuringRestore = false;
        TestModule.captureFailure = false;
        TestModule.throwOnStart = false;
        TestModule.throwOnStop = false;
        TestModule.pendingStops = 0;
        TestPanel.attachCount = 0;
        TestPanel.detachCount = 0;
        TestPanel.firstAttachPrecededModuleStart = false;
        DeferredAction.executeCount = 0;
        NeutralHistoryHandler.value = 0;
        UpdateBarrierModule.block = false;
        UpdateBarrierModule.throwWhenRead = false;
        UpdateBarrierModule.barrierReadCount = 0;
        FollowingUpdateModule.updateCount = 0;
        TestPanel.throwFromWindowPadding = false;
        TestPanel.throwOnAttach = false;
        TestPanel.throwOnDetach = false;
        TestPanel.pendingDetaches = 0;
        MultiShortcutAction.executeCount = 0;
        ToolbarAction.isChecked = false;
        ToolbarAction.executeCount = 0;
        MetadataAction.targetTypeReadCount = 0;
        MetadataAction.throwWhenRead = false;
        ThrowAfterActivateAction.cancelCount = 0;
        ADisposeObserverModule.historyAvailableDuringStop = false;
        ADisposeObserverModule.historyDisposedDuringDispose = false;
        ADisposeObserverModule.disposeCount = 0;
        ZThrowingDisposeModule.disposeCount = 0;
        ZThrowingDisposeModule.throwOnDispose = false;
        ShutdownOrder.events.Clear();
        m_runtime = CreateRuntime(new EditorContext(m_projectRoot, new EditorKeyboardPolicy(KeyModifier.Control, "Super")));
        m_runtime.Start();
    }

    public void Dispose()
    {
        m_runtime.Dispose();
        m_types.Dispose();
        m_logs.Dispose();
        m_diagnosticScope.Dispose();
        m_modules.Dispose();
        if (Directory.Exists(m_projectRoot))
        {
            Directory.Delete(m_projectRoot, recursive: true);
        }
    }

    [Fact]
    public void FeatureLookupReturnsOnlyStartedNonQuarantinedModules()
    {
        Assert.True(m_runtime.interactions.TryGetModule<TestModule>(out TestModule? first));
        Assert.NotNull(first);
        Assert.True(m_runtime.interactions.TryGetModule<TestModule>(out TestModule? again));
        Assert.Same(first, again);
        UpdateBarrierModule.throwWhenRead = true;
        m_runtime.Update(new EditorFrame());
        Assert.False(m_runtime.interactions.TryGetModule<UpdateBarrierModule>(out UpdateBarrierModule? quarantined));
        Assert.Null(quarantined);
    }

    [Fact]
    public void HistoryAndExtensionStateContractsDoNotExposeStandaloneWorkspaceTypes()
    {
        Assembly core = typeof(EditorModule).Assembly;
        Assert.Null(core.GetType("Inno.Editor.Core.IEditorWorkspaceState"));
        Assert.Null(core.GetType("Inno.Editor.Core.EditorWorkspaceStateReader"));
        Assert.Null(core.GetType("Inno.Editor.Core.EditorWorkspaceStateWriter"));
        Assert.Null(typeof(EditorHistoryChange).GetProperty("version"));
        ConstructorInfo constructor = Assert.Single(typeof(EditorHistoryHandlerAttribute).GetConstructors());
        Assert.Collection(constructor.GetParameters(), static parameter => Assert.Equal(typeof(string), parameter.ParameterType));
    }

    [Fact]
    public void EditorDensityDefaultsToComfortableAndCanSwitchIdempotently()
    {
        var style = new EditorStyleMetrics();
        float comfortablePadding = style.framePadding.Y;
        Assert.False(style.isCompact);
        Assert.True(style.SetCompactMode(true));
        Assert.True(style.isCompact);
        Assert.True(style.framePadding.Y < comfortablePadding);
        Assert.False(style.SetCompactMode(true));
        Assert.True(style.SetCompactMode(false));
        Assert.False(style.isCompact);
        Assert.Equal(comfortablePadding, style.framePadding.Y);
    }

    [Fact]
    public void InteractionRuntimeInjectsOneStableAssignableHostService()
    {
        var service = new TestHostService();
        HostServicePanel.current = null;
        using EditorInteractionRuntime runtime = CreateRuntime(
            new EditorContext(m_projectRoot, new EditorKeyboardPolicy(KeyModifier.Control, "Super")),
            service
        );
        runtime.Start();
        Assert.Contains(runtime.panels, static panel => panel.id == "tests.host-service");
        Assert.Same(service, HostServicePanel.current);
    }

    [Fact]
    public void InteractionRuntimeRejectsExtensionWhenHostServiceIsUnavailable()
    {
        HostServicePanel.current = null;
        using EditorInteractionRuntime runtime = CreateRuntime(
            new EditorContext(m_projectRoot, new EditorKeyboardPolicy(KeyModifier.Control, "Super"))
        );
        runtime.Start();
        Assert.DoesNotContain(runtime.panels, static panel => panel.id == "tests.host-service");
        Assert.Null(HostServicePanel.current);
    }

    [Fact]
    public void InteractionIdentifiersArePlainValidatedStringsWithoutWrapperTypes()
    {
        Assembly assembly = typeof(EditorInteractions).Assembly;
        Assert.Null(assembly.GetType("Inno.Editor.Interactions.EditorActionId"));
        Assert.Null(assembly.GetType("Inno.Editor.Interactions.EditorAreaId"));
        Assert.Null(assembly.GetType("Inno.Editor.Interactions.EditorPanelId"));
        Assert.Null(assembly.GetType("Inno.Editor.Interactions.EditorCommand"));
        Assert.Null(assembly.GetType("Inno.Editor.Interactions.EditorCommand`1"));
        Assert.Equal(typeof(string), typeof(EditorInteractions).GetProperty("focusedArea")!.PropertyType);
        Assert.Equal(typeof(string), typeof(EditorInteraction).GetProperty("area")!.PropertyType);
        Assert.Equal(typeof(string), typeof(EditorPanelExtension).GetProperty("id")!.PropertyType);
        MethodInfo forMethod = Assert.Single(typeof(EditorInteractions).GetMethods().Where(static method => method.Name == "For"));
        Assert.Equal(typeof(string), forMethod.GetParameters()[0].ParameterType);
        Assert.Throws<ArgumentException>(() => m_runtime.interactions.For(string.Empty));
        Assert.Throws<ArgumentException>(() => m_runtime.interactions.TogglePanel(" "));
        Assert.Throws<ArgumentException>(() => m_runtime.interactions.OpenPanel(" "));
        Assert.Throws<ArgumentException>(() => m_runtime.interactions.ClosePanel(" "));
    }

    [Fact]
    public void PanelOpenAndCloseOperationsAreIdempotentAndRequestFocus()
    {
        EditorPanelExtension panel = Assert.Single(m_runtime.panels.Where(static candidate => candidate.id == "tests.panel"));
        Assert.False(panel.isOpen);
        Assert.True(m_runtime.interactions.OpenPanel("tests.panel"));
        Assert.True(m_runtime.interactions.OpenPanel("tests.panel"));
        Assert.True(panel.isOpen);
        Assert.True(panel.TakeFocusRequest());
        Assert.False(panel.TakeFocusRequest());
        Assert.True(m_runtime.interactions.ClosePanel("tests.panel"));
        Assert.True(m_runtime.interactions.ClosePanel("tests.panel"));
        Assert.False(panel.isOpen);
        Assert.False(panel.TakeFocusRequest());
        Assert.False(m_runtime.interactions.OpenPanel("tests.missing-panel"));
    }

    [Fact]
    public void DocumentHostOpensOneInstanceAndRequiresExplicitDirtyCloseDecision()
    {
        IEditorDocumentService documents = m_runtime.interactions.documents;
        var provider = new TestDocumentProvider();
        using IDisposable lease = documents.RegisterProvider(provider);
        Guid assetID = Guid.NewGuid();
        EditorDocumentContext opened = documents.Open("./Assets/Hero.ispriteatlas2d", assetID);
        EditorDocumentContext focused = documents.Open("Assets/OtherName.ispriteatlas2d", assetID);
        documents.SetDirty(opened.documentID);
        Assert.Same(opened, focused);
        Assert.Equal("Assets/OtherName.ispriteatlas2d", opened.assetPath);
        Assert.Equal("OtherName.ispriteatlas2d", opened.title);
        Assert.True(opened.isDirty);
        Assert.False(documents.Close(opened.documentID, EditorDocumentCloseMode.Cancel));
        Assert.Single(documents.documents);
        Assert.True(documents.Close(opened.documentID, EditorDocumentCloseMode.Save));
        Assert.Empty(documents.documents);
        Assert.Equal(1, provider.saveCount);
        Assert.Equal(1, provider.closeCount);
    }

    [Fact]
    public void DocumentPathReassignmentRetiresOnlyCleanStaleOwner()
    {
        IEditorDocumentService documents = m_runtime.interactions.documents;
        var provider = new TestDocumentProvider();
        using IDisposable lease = documents.RegisterProvider(provider);
        const string C_PATH = "Assets/Shared.ispriteatlas2d";
        EditorDocumentContext first = documents.Open(C_PATH, Guid.NewGuid());
        EditorDocumentContext replacement = documents.Open(C_PATH, Guid.NewGuid());
        Assert.NotSame(first, replacement);
        Assert.Single(documents.documents);
        Assert.Equal(1, provider.closeCount);
        documents.SetDirty(replacement.documentID);
        InvalidOperationException failure = Assert.Throws<InvalidOperationException>(() => documents.Open(C_PATH, Guid.NewGuid()));
        Assert.Contains("unsaved changes", failure.Message, StringComparison.Ordinal);
        Assert.Same(replacement, Assert.Single(documents.documents));
    }

    [Fact]
    public void DocumentStateSurvivesProviderGenerationReplacement()
    {
        IEditorDocumentService documents = m_runtime.interactions.documents;
        var firstProvider = new TestDocumentProvider();
        IDisposable firstLease = documents.RegisterProvider(firstProvider);
        EditorDocumentContext opened = documents.Open("Assets/World.itilemap2d", Guid.NewGuid());
        opened.SetViewParameter("zoom", "2.5");
        documents.SetDirty(opened.documentID);
        firstLease.Dispose();
        Assert.False(opened.isProviderAvailable);
        Assert.False(documents.Save(opened.documentID));
        var replacement = new TestDocumentProvider();
        using IDisposable replacementLease = documents.RegisterProvider(replacement);
        Assert.True(opened.isProviderAvailable);
        Assert.Equal(1, replacement.openCount);
        EditorDocumentContext restored = Assert.Single(documents.documents);
        Assert.True(restored.TryGetViewParameter("zoom", out string zoom));
        Assert.Equal("2.5", zoom);
        Assert.True(documents.Close(opened.documentID, EditorDocumentCloseMode.Discard));
    }

    [Fact]
    public void ViewportToolCapturesOnePointerAndCommitsOneGestureTransaction()
    {
        IEditorHistory history = m_runtime.interactions.history;
        var coordinates = new TestViewportCoordinates();
        var tool = new TestViewportTool(history);
        using var session = new EditorViewportToolSession(history, coordinates);
        session.SetTool(tool);
        var down = new EditorViewportPointerEvent(
            7,
            EditorViewportPointerPhase.Down,
            new Vector2(2f, 3f),
            coordinates.ScreenToWorld(new Vector2(2f, 3f)),
            0,
            KeyModifier.None
        );
        Assert.True(session.HandlePointer(down));
        Assert.False(
            session.HandlePointer(new EditorViewportPointerEvent(8, EditorViewportPointerPhase.Move, default, default, 0, KeyModifier.None))
        );
        Assert.True(
            session.HandlePointer(
                new EditorViewportPointerEvent(
                    7,
                    EditorViewportPointerPhase.Up,
                    down.screenPosition,
                    down.worldPosition,
                    0,
                    KeyModifier.None
                )
            )
        );
        Assert.Equal(1, tool.downCount);
        Assert.Equal(1, tool.upCount);
        Assert.True(history.canUndo);
        Assert.Equal("Paint Tile", history.undoName);
        Assert.True(history.Undo().succeeded);
        Assert.Equal(0, NeutralHistoryHandler.value);
    }

    [Fact]
    public void RuntimeDiscoversModulesAndPanelsWithoutRegistrationCalls()
    {
        Assert.True(TestModule.startCount > 0);
        Assert.True(TestPanel.attachCount > 0);
        Assert.False(TestPanel.firstAttachPrecededModuleStart);
        Assert.True(m_runtime.panelCount >= 1);
    }

    [Fact]
    public void StatisticsUseStableReplacementAndOneFrameOrderHandoff()
    {
        EditorStatistics statistics = m_runtime.context.statistics;
        var id = new EditorStatisticID("tests.statistics.frame-time");
        var groupID = new EditorStatisticGroupID("tests.statistics");
        statistics.Publish(new EditorStatistic(id, groupID, "Tests", "Frame Time", "10 ms"));
        statistics.Publish(new EditorStatistic(id, groupID, "Tests", "Frame Time", "8 ms"));
        Assert.Equal("8 ms", Assert.Single(statistics.GetSnapshot()).value);
        m_runtime.Update(new EditorFrame(0.016f, 0.016f, isFocused: true));
        Assert.Equal("8 ms", Assert.Single(statistics.GetSnapshot()).value);
        m_runtime.Update(new EditorFrame(0.016f, 0.032f, isFocused: true));
        Assert.Empty(statistics.GetSnapshot());
    }

    [Fact]
    public void BlockingModuleDefersOnlyModulesOrderedAfterIt()
    {
        UpdateBarrierModule.block = true;
        m_runtime.Update(new EditorFrame(0.016f, 0.016f, isFocused: true));
        Assert.Equal(0, FollowingUpdateModule.updateCount);
        UpdateBarrierModule.block = false;
        m_runtime.Update(new EditorFrame(0.016f, 0.032f, isFocused: true));
        Assert.Equal(1, FollowingUpdateModule.updateCount);
    }

    [Fact]
    public void ThrowingModuleBarrierIsQuarantinedWithoutStoppingFollowingModules()
    {
        UpdateBarrierModule.throwWhenRead = true;
        m_runtime.Update(new EditorFrame(0.016f, 0.016f, isFocused: true));
        m_runtime.Update(new EditorFrame(0.016f, 0.032f, isFocused: true));
        Assert.Equal(1, UpdateBarrierModule.barrierReadCount);
        Assert.Equal(2, FollowingUpdateModule.updateCount);
    }

    [Fact]
    public void ActionResolutionPrefersExactAreaAndTarget()
    {
        var target = new DerivedTarget();
        EditorActionState exact = m_runtime.interactions.For("tests/special", target).Query("tests.resolve");
        EditorActionState fallback = m_runtime.interactions.For("tests/other", target).Query("tests.resolve");
        Assert.Equal("area", exact.displayName);
        Assert.Equal("base", fallback.displayName);
    }

    [Fact]
    public void ActionTypeMetadataIsCapturedOnceAndNeverReadOnTheRoutingPath()
    {
        int readsAfterCatalogBuild = MetadataAction.targetTypeReadCount;
        MetadataAction.throwWhenRead = true;
        try
        {
            EditorInteraction interaction = m_runtime.interactions.For("tests/action-metadata", new BaseTarget());
            Assert.True(interaction.Query("tests.action-metadata").isEnabled);
            Assert.True(interaction.Execute("tests.action-metadata"));
            Assert.True(interaction.Query("tests.action-metadata").isEnabled);
        }
        finally
        {
            MetadataAction.throwWhenRead = false;
        }

        Assert.Equal(readsAfterCatalogBuild, MetadataAction.targetTypeReadCount);
    }

    [Fact]
    public void QueuedActionExecutesAtRuntimeSafePoint()
    {
        m_runtime.interactions.For("tests/other").Enqueue("tests.deferred");
        Assert.Equal(0, DeferredAction.executeCount);
        m_runtime.Update(new EditorFrame(0.016f, 1f, isFocused: true));
        Assert.Equal(1, DeferredAction.executeCount);
    }

    [Fact]
    public void BuiltInUndoAndRedoActionsExposeHistoryThroughMenusAndShortcuts()
    {
        NeutralHistoryHandler.value = 1;
        m_runtime.interactions.history.RecordApplied("Change Test Value", NeutralHistoryHandler.CreateChange(before: 0, after: 1));
        EditorInteraction global = m_runtime.interactions.For("editor/global");
        EditorActionState undo = global.Query("editor/undo");
        Assert.True(undo.isEnabled);
        Assert.Equal("Undo Change Test Value", undo.displayName);
        Assert.True(global.Execute("editor/undo"));
        Assert.Equal(0, NeutralHistoryHandler.value);
        EditorActionState redo = global.Query("editor/redo");
        Assert.True(redo.isEnabled);
        Assert.Equal("Redo Change Test Value", redo.displayName);
        Assert.True(global.Execute("editor/redo"));
        Assert.Equal(1, NeutralHistoryHandler.value);
    }

    [Fact]
    public void MissingHistoryHandlerCreatesAnExplicitBarrier()
    {
        m_runtime.interactions.history.RecordApplied(
            "Unavailable Change",
            new EditorHistoryChange("tests/missing-history-handler", EditorHistoryPayload.FromBytes([1, 2, 3]))
        );
        Assert.False(m_runtime.interactions.history.canUndo);
        Assert.Contains("tests/missing-history-handler", m_runtime.interactions.history.undoUnavailableReason);
        Assert.False(m_runtime.interactions.history.Undo().succeeded);
        Assert.Equal("Unavailable Change", m_runtime.interactions.history.undoName);
    }

    [Fact]
    public void LargeNeutralPayloadSpillsToTheBoundedSessionDiskStore()
    {
        NeutralHistoryHandler.value = 32;
        var bytes = new byte[128 * 1024];
        BitConverter.GetBytes(9).CopyTo(bytes, 0);
        BitConverter.GetBytes(32).CopyTo(bytes, sizeof(int));
        m_runtime.interactions.history.RecordApplied(
            "Large Neutral Change",
            new EditorHistoryChange(NeutralHistoryHandler.C_KIND, EditorHistoryPayload.FromBytes(bytes))
        );
        Assert.Equal(0, m_runtime.interactions.history.residentBytes);
        Assert.Equal(bytes.LongLength, m_runtime.interactions.history.diskBytes);
        Assert.True(m_runtime.interactions.history.Undo().succeeded);
        Assert.Equal(9, NeutralHistoryHandler.value);
    }

    [Fact]
    public void AttributeMenusSupportArbitraryDepthAndDynamicEntries()
    {
        EditorMenuModel menu = m_runtime.interactions.For("tests/menu").BuildMenu();
        EditorMenuItem tools = Assert.Single(menu.items);
        Assert.Equal("Tools", tools.label);
        EditorMenuItem create = Assert.Single(tools.children);
        Assert.Equal("Create", create.label);
        Assert.Equal(["Asset", "Libraries", "Generated"], create.children.Select(static item => item.label));
        EditorMenuItem libraries = Assert.Single(create.children.Where(static item => item.label == "Libraries"));
        Assert.True(libraries.separatorBefore);
        Assert.False(tools.separatorBefore);
        Assert.False(create.separatorBefore);
        Assert.Equal("Function", Assert.Single(libraries.children).label);
    }

    [Fact]
    public void ToolbarResolvesContextualIconTooltipStateAndDispatch()
    {
        EditorInteraction interaction = m_runtime.interactions.For("tests/toolbar");
        EditorToolbarItem initial = Assert.Single(interaction.BuildToolbar().items);
        Assert.Equal("tests.toolbar", initial.actionID);
        Assert.Equal(EditorToolbarIcon.Play, initial.icon);
        Assert.Equal("Start Runtime", initial.tooltip);
        Assert.False(initial.status.isChecked);
        ToolbarAction.isChecked = true;
        EditorToolbarItem active = Assert.Single(interaction.BuildToolbar().items);
        Assert.Equal(EditorToolbarIcon.Stop, active.icon);
        Assert.Equal("Stop Runtime", active.tooltip);
        Assert.True(active.status.isChecked);
        interaction.Enqueue(active.actionID);
        m_runtime.Update(new EditorFrame(0.016f, 1f, isFocused: true));
        Assert.Equal(1, ToolbarAction.executeCount);
    }

    [Fact]
    public void WorkbenchPlacesPanelTogglesUnderWindow()
    {
        EditorMenuModel menu = m_runtime.interactions.For("editor/main-menu").BuildMenu();
        EditorMenuItem panel = Assert.Single(menu.items.Where(static item => item.label == "Window"));
        EditorMenuItem testing = Assert.Single(panel.children.Where(static item => item.label == "Testing"));
        EditorMenuItem test = Assert.Single(testing.children.Where(static item => item.label == "Test"));
        Assert.False(test.status.isChecked);
        EditorMenuItem? view = menu.items.SingleOrDefault(static item => item.label == "View");
        Assert.True(view is null || view.children.All(static item => item.label != "Test"));
        Assert.True(m_runtime.interactions.TogglePanel("tests.panel"));
        EditorMenuModel openMenu = m_runtime.interactions.For("editor/main-menu").BuildMenu();
        EditorMenuItem openPanel = Assert.Single(openMenu.items.Where(static item => item.label == "Window"));
        EditorMenuItem openTesting = Assert.Single(openPanel.children.Where(static item => item.label == "Testing"));
        EditorMenuItem openTest = Assert.Single(openTesting.children.Where(static item => item.label == "Test"));
        Assert.True(openTest.status.isChecked);
    }

    [Fact]
    public void EveryShortcutDeclaredByTheSameActionCanDispatch()
    {
        m_runtime.interactions.For("tests/multiple-shortcuts").Focus();
        m_runtime.HandleKeyPressed(new KeyPressedEvent(0, KeyCode.F3, KeyModifier.None));
        m_runtime.HandleKeyPressed(new KeyPressedEvent(0, KeyCode.F4, KeyModifier.None));
        Assert.Equal(2, MultiShortcutAction.executeCount);
    }

    [Fact]
    public void ProductKeyboardPolicyControlsDiscoveryLabelsAndShortcutDispatch()
    {
        using EditorInteractionRuntime runtime = CreateRuntime(
            new EditorContext(m_projectRoot, new EditorKeyboardPolicy(KeyModifier.Super, "Cmd"))
        );
        runtime.Start();
        NeutralHistoryHandler.value = 1;
        runtime.interactions.history.RecordApplied("Change Test Value", NeutralHistoryHandler.CreateChange(before: 0, after: 1));
        EditorInteraction global = runtime.interactions.For("editor/global");
        global.Focus();
        Assert.True(global.TryGetShortcut("editor/undo", out HotKeyGesture gesture));
        Assert.Equal(KeyModifier.Super, gesture.modifiers);
        Assert.Equal("Cmd+Z", gesture.ToString());
        runtime.HandleKeyPressed(new KeyPressedEvent(0, KeyCode.Z, KeyModifier.Control));
        Assert.Equal(1, NeutralHistoryHandler.value);
        runtime.HandleKeyPressed(new KeyPressedEvent(0, KeyCode.Z, KeyModifier.Super));
        Assert.Equal(0, NeutralHistoryHandler.value);
        Assert.True(runtime.interactions.history.canRedo);
    }

    [Fact]
    public void ActionExecutionFailureCancelsStateActivatedBeforeTheException()
    {
        EditorInteraction interaction = m_runtime.interactions.For("tests/throw-after-activate");
        Assert.False(interaction.Execute("tests.throw-after-activate"));
        Assert.False(interaction.IsActive("tests.throw-after-activate"));
        Assert.Equal(1, ThrowAfterActivateAction.cancelCount);
    }

    [Fact]
    public void ActionOwnsValidatedMultiFrameState()
    {
        var target = new InteractionTarget();
        EditorInteraction interaction = m_runtime.interactions.For("tests/other", target);
        Assert.True(interaction.Execute("tests.interaction"));
        Assert.True(interaction.IsActive("tests.interaction"));
        Assert.True(interaction.Present("tests.interaction", new InteractionPresentation(string.Empty, submit: true)));
        Assert.True(interaction.IsActive("tests.interaction"));
        Assert.Equal("A name is required.", target.validationMessage);
        Assert.Null(target.committedValue);
        Assert.True(interaction.Present("tests.interaction", new InteractionPresentation("Renamed", submit: true)));
        Assert.Equal("Renamed", target.committedValue);
        Assert.False(interaction.IsActive("tests.interaction"));
        Assert.False(interaction.Present("tests.interaction"));
    }

    [Fact]
    public void ChangingSelectionFinishesThePreviousTargetsActivePresentation()
    {
        var target = new InteractionTarget();
        EditorInteraction interaction = m_runtime.interactions.For("tests/other", target);
        Assert.True(interaction.Select());
        Assert.True(interaction.Execute("tests.commit-on-presentation-lost"));
        Assert.True(m_runtime.interactions.For("tests/other", new DerivedTarget()).Select());
        Assert.Equal("Committed on focus loss", target.committedValue);
        Assert.False(interaction.IsActive("tests.commit-on-presentation-lost"));
    }

    [Fact]
    public void SelectionAndFocusUseTheLightweightAreaHandle()
    {
        var target = new DerivedTarget();
        EditorInteraction interaction = m_runtime.interactions.For("tests/other", target);
        interaction.Focus();
        Assert.Equal("tests/other", m_runtime.interactions.focusedArea);
        Assert.Same(target, m_runtime.interactions.focusedTarget);
        Assert.True(interaction.Select());
        Assert.Same(target, m_runtime.interactions.selection.selectedTarget);
        Assert.True(interaction.isSelected);
        Assert.True(m_runtime.interactions.For("tests/other").Select());
        Assert.Null(m_runtime.interactions.selection.selectedTarget);
        Assert.Null(typeof(EditorSelectionState).GetMethod("Select"));
        Assert.Null(typeof(EditorSelectionState).GetMethod("Clear"));
    }

    [Fact]
    public void IdentityResolutionUsesTheOwningDomain()
    {
        DragSource primary = Register(new DragSource());
        var secondary = new DragSource();
        Assert.True(m_secondaryIdentities.Register(secondary));
        RuntimeIdentity primaryIdentity = primary.identity.runtimeIdentity!.Value;
        RuntimeIdentity secondaryIdentity = secondary.identity.runtimeIdentity!.Value;
        Assert.True(m_runtime.interactions.TryResolveIdentity(primaryIdentity, out IdentityObject? primaryResult));
        Assert.Same(primary, primaryResult);
        Assert.True(m_runtime.interactions.TryResolveIdentity(secondaryIdentity, out IdentityObject? secondaryResult));
        Assert.Same(secondary, secondaryResult);
        Assert.True(
            m_runtime.interactions.TryResolveIdentity(
                secondaryIdentity.domainID,
                secondary.identity.persistentID,
                out IdentityObject? persistentResult
            )
        );
        Assert.Same(secondary, persistentResult);
        var crossDomainAlias = new RuntimeIdentity(secondaryIdentity.domainID, primaryIdentity.runtimeID);
        Assert.NotSame(primary, m_runtime.interactions.TryResolveIdentity(crossDomainAlias, out IdentityObject? alias) ? alias : null);
        Guid persistentID = secondary.identity.persistentID;
        Assert.True(m_secondaryIdentities.Unregister(secondary));
        var replacement = new DragSource();
        Assert.True(m_secondaryIdentities.Register(replacement, persistentID));
        Assert.True(
            m_runtime.interactions.TryResolveIdentity(secondaryIdentity.domainID, persistentID, out IdentityObject? replacementResult)
        );
        Assert.Same(replacement, replacementResult);
    }

    [Fact]
    public void TypedDropRoutesAndCancelsItsManagedSession()
    {
        DragSource supersededSource = Register(new DragSource());
        DragSource source = Register(new DragSource());
        var target = new DropTarget();
        var data = new EditorDragData(source, "source");
        RuntimeIdentity supersededIdentity = m_runtime.interactions
            .For("tests/other", supersededSource)
            .BeginDrag(new EditorDragData(supersededSource, "superseded"));
        RuntimeIdentity identity = m_runtime.interactions.For("tests/other", source).BeginDrag(data);
        EditorInteraction dropTarget = m_runtime.interactions.For("tests/drop", target);
        Assert.NotEqual(supersededIdentity, identity);
        Assert.False(m_runtime.interactions.TryGetDragSource(supersededIdentity, out _));
        Assert.True(dropTarget.QueryDrop(identity, EditorDropPlacement.Into).canDrop);
        Assert.True(m_runtime.interactions.TryGetDragSource(identity, out _));
        EditorDropResult result = dropTarget.Drop(identity, EditorDropPlacement.Into);
        Assert.True(result.accepted, $"Drop target invoked: {target.wasDropped}.");
        Assert.True(target.wasDropped);
        Assert.False(m_runtime.interactions.TryGetDragSource(identity, out _));
    }

    [Fact]
    public void UnregisteredDragSourceCancelsTheManagedSession()
    {
        DragSource source = Register(new DragSource());
        RuntimeIdentity identity = m_runtime.interactions.For("tests/drag", source).BeginDrag(new EditorDragData(source, "source"));
        Assert.True(m_identities.Unregister(source));
        Assert.False(m_runtime.interactions.TryGetDragSource(identity, out _));
        Assert.False(m_runtime.interactions.TryGetDragSource(identity, out _));
    }

    [Fact]
    public void ThrowingPanelPresentationPropertyQuarantinesOnlyThatPanel()
    {
        TestPanel.throwFromWindowPadding = true;
        EditorPanelExtension panel = Assert.Single(m_runtime.panels.Where(static value => value.id == "tests.panel"));
        Assert.False(panel.TryGetWindowPresentation(out _, out _, out _));
        Assert.False(panel.isOpen);
    }

    [Fact]
    public void ShutdownDetachesPanelsBeforeStoppingModulesThenDisposesInteractionsAndExtensions()
    {
        ZThrowingDisposeModule.throwOnDispose = true;
        AggregateException failure = Assert.Throws<AggregateException>(m_runtime.Dispose);
        Assert.Contains("Injected extension disposal failure", failure.ToString());
        Assert.Equal(GenerationState.Faulted, m_modules.generations.state);
        Assert.Throws<InvalidOperationException>(() => m_modules.generations.EnsureReady("reload after failed extension retirement"));
        Assert.True(ADisposeObserverModule.historyAvailableDuringStop);
        Assert.True(ADisposeObserverModule.historyDisposedDuringDispose);
        Assert.True(ShutdownOrder.events.IndexOf("panel.detach") < ShutdownOrder.events.IndexOf("module.stop"));
        Assert.Equal(1, ADisposeObserverModule.disposeCount);
        Assert.Equal(1, ZThrowingDisposeModule.disposeCount);
    }

    [Fact]
    public void FailedModuleStartIsStoppedBeforeCandidateDisposal()
    {
        m_runtime.Dispose();
        int stops = TestModule.stopCount;
        TestModule.throwOnStart = true;
        using EditorInteractionRuntime runtime = CreateRuntime(
            new EditorContext(m_projectRoot, new EditorKeyboardPolicy(KeyModifier.Control, "Super"))
        );
        try
        {
            Assert.Throws<InvalidOperationException>(runtime.Start);
            Assert.Equal(stops + 1, TestModule.stopCount);
            Assert.Equal(GenerationState.Ready, m_modules.generations.state);
        }
        finally
        {
            TestModule.throwOnStart = false;
        }
    }

    [Fact]
    public void FailedPanelAttachmentIsCompensatedBeforeItIsQuarantined()
    {
        m_runtime.Dispose();
        int detaches = TestPanel.detachCount;
        TestPanel.throwOnAttach = true;
        using EditorInteractionRuntime runtime = CreateRuntime(
            new EditorContext(m_projectRoot, new EditorKeyboardPolicy(KeyModifier.Control, "Super"))
        );
        try
        {
            runtime.Start();
            Assert.Equal(detaches + 1, TestPanel.detachCount);
            Assert.DoesNotContain(runtime.panels, static panel => panel.id == "tests.panel");
            Assert.Equal(GenerationState.Ready, m_modules.generations.state);
            runtime.Dispose();
            Assert.Equal(detaches + 1, TestPanel.detachCount);
        }
        finally
        {
            TestPanel.throwOnAttach = false;
        }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void LifecycleFailureIsReportedAfterAllCompletedStagesAndFaultsAdmission(bool panelFailure)
    {
        TestModule.throwOnStop = !panelFailure;
        TestPanel.throwOnDetach = panelFailure;
        try
        {
            AggregateException failure = Assert.Throws<AggregateException>(m_runtime.Dispose);
            Assert.Contains(panelFailure ? "Injected panel detach failure" : "Injected module stop failure", failure.ToString());
            Assert.Equal(1, TestModule.stopCount);
            Assert.Equal(1, TestPanel.detachCount);
            Assert.Equal(1, ADisposeObserverModule.disposeCount);
            Assert.Equal(GenerationState.Faulted, m_modules.generations.state);
            m_runtime.Dispose();
            Assert.Equal(1, TestModule.stopCount);
        }
        finally
        {
            TestModule.throwOnStop = false;
            TestPanel.throwOnDetach = false;
        }
    }

    [Fact]
    public void PendingPanelAndModuleDrainBeforeHistoryAndExtensionResourcesAreDisposed()
    {
        TestPanel.pendingDetaches = 2;
        TestModule.pendingStops = 2;
        m_runtime.Dispose();
        Assert.Equal(3, TestPanel.detachCount);
        Assert.Equal(3, TestModule.stopCount);
        Assert.True(ADisposeObserverModule.historyAvailableDuringStop);
        Assert.True(ADisposeObserverModule.historyDisposedDuringDispose);
        Assert.Equal(1, ADisposeObserverModule.disposeCount);
        Assert.Equal(GenerationState.Ready, m_modules.generations.state);
        Assert.Equal(
            new[] { "panel.detach", "panel.detach", "panel.detach", "module.stop", "module.stop", "module.stop" },
            ShutdownOrder.events.Where(static value => value is "panel.detach" or "module.stop")
        );
    }

    [Fact]
    public void RebuildRetainsHostExtensionsInsteadOfRestartingModules()
    {
        int starts = TestModule.startCount;
        int attaches = TestPanel.attachCount;
        m_types.Rebuild();
        _ = m_runtime.panelCount;
        Assert.Equal(starts, TestModule.startCount);
        Assert.Equal(attaches, TestPanel.attachCount);
    }

    [Fact]
    public void ModuleStateAndPanelVisibilityRestoreForTheSameProject()
    {
        TestModule.stateValue = 42;
        EditorPanelExtension panel = Assert.Single(m_runtime.panels.Where(static value => value.id == "tests.panel"));
        panel.isOpen = true;
        m_runtime.Update(new EditorFrame(0.016f, 3f, isFocused: true));
        m_runtime.Dispose();
        string settingsPath = Path.Combine(m_projectRoot, "editor.ini");
        Assert.True(File.Exists(settingsPath));
        string document = File.ReadAllText(settingsPath);
        Assert.Contains("[InnoEditor][Module.tests.state]", document);
        Assert.DoesNotContain("[InnoEditor][Module.tests.update-barrier]", document);
        using EditorInteractionRuntime restored = CreateRuntime(
            new EditorContext(m_projectRoot, new EditorKeyboardPolicy(KeyModifier.Control, "Super"))
        );
        restored.Start();
        Assert.Equal(42, TestModule.restoredStateValue);
        Assert.True(Assert.Single(restored.panels.Where(static value => value.id == "tests.panel")).isOpen);
    }

    [Fact]
    public void MalformedModuleStateValueUsesTheExtensionFallback()
    {
        m_runtime.Dispose();
        var context = new EditorContext(m_projectRoot, new EditorKeyboardPolicy(KeyModifier.Control, "Super"));
        context.SetLayoutSection("Module.tests.state", new Dictionary<string, string> { ["value"] = "not-json" });
        context.SaveLayout();
        TestModule.restoredStateValue = -1;
        using EditorInteractionRuntime restored = CreateRuntime(context);
        restored.Start();
        Assert.Equal(0, TestModule.restoredStateValue);
    }

    [Fact]
    public void ExtensionStateCaptureFailurePublishesCurrentDiagnosticUntilRetrySucceeds()
    {
        var sink = new TestDiagnosticSink();
        m_diagnostics.RegisterSink(sink);
        try
        {
            TestModule.captureFailure = true;
            m_runtime.Update(new EditorFrame(0.016f, 3f, isFocused: true));
            DiagnosticReport report = Assert.Single(
                sink.reports.Values.Where(static value => value.source.displayName == "Editor State Capture")
            );
            Assert.Equal("EDITOR-STATE-CAPTURE", Assert.Single(report.diagnostics).code);
            TestModule.captureFailure = false;
            m_runtime.Update(new EditorFrame(0.016f, 6f, isFocused: true));
            Assert.DoesNotContain(sink.reports.Values, static value => value.source.displayName == "Editor State Capture");
        }
        finally
        {
            TestModule.captureFailure = false;
            m_diagnostics.UnregisterSink(sink);
        }
    }

    [Fact]
    public void UnifiedEditorIniPreservesLayoutAndExtensionStateSectionsTogether()
    {
        const string C_LAYOUT = "[Window][Hierarchy]\nPos=10,20\nSize=300,400";
        var context = new EditorContext(m_projectRoot, new EditorKeyboardPolicy(KeyModifier.Control, "Super"));
        context.SetImGuiLayout(C_LAYOUT);
        context.SetLayoutSection("Module.tests", new Dictionary<string, string> { ["openScenes"] = "[\"Scenes/Test.iscene\"]" });
        Assert.True(context.SaveLayoutIfChanged());
        var restored = new EditorContext(m_projectRoot, new EditorKeyboardPolicy(KeyModifier.Control, "Super"));
        Assert.Equal(C_LAYOUT, restored.imguiLayout);
        Assert.True(restored.TryGetLayoutSection("Module.tests", out IReadOnlyDictionary<string, string> values));
        Assert.Equal("[\"Scenes/Test.iscene\"]", values["openScenes"]);
        string document = File.ReadAllText(restored.layoutPath);
        Assert.Contains("[Window][Hierarchy]", document);
        Assert.Contains("[InnoEditor][Module.tests]", document);
        Assert.Contains("openScenes=[\"Scenes/Test.iscene\"]", document);
        Assert.DoesNotContain("Payload=", document);
    }

    [Fact]
    public void StartupRegistryRefreshCannotOverwriteModuleStateBeforeRestore()
    {
        m_runtime.Dispose();
        var context = new EditorContext(m_projectRoot, new EditorKeyboardPolicy(KeyModifier.Control, "Super"));
        context.SetLayoutSection("Module.tests.state", new Dictionary<string, string> { ["value"] = "91" });
        context.SaveLayout();
        TestModule.startCount = 0;
        TestModule.restoredStateValue = 0;
        TestModule.rebuildDuringRestore = true;
        using EditorInteractionRuntime restored = CreateRuntime(
            new EditorContext(m_projectRoot, new EditorKeyboardPolicy(KeyModifier.Control, "Super"))
        );
        restored.Start();
        Assert.Equal(91, TestModule.restoredStateValue);
        TestModule.stateValue = TestModule.restoredStateValue;
        restored.Update(new EditorFrame(0.016f, 0.016f, isFocused: true));
        restored.SaveState();
        string document = File.ReadAllText(Path.Combine(m_projectRoot, "editor.ini"));
        Assert.Contains("[InnoEditor][Module.tests.state]", document);
        Assert.Contains("value=91", document);
        Assert.False(TestModule.rebuildDuringRestore);
    }

    private EditorInteractionRuntime CreateRuntime(
        EditorContext context,
        params object[] hostServices
    ) => new(context, m_types, m_logs, [m_types, m_identities, m_secondaryIdentities, ..hostServices]);

    private TIdentity Register<TIdentity>(TIdentity value)
        where TIdentity : IdentityObject
    {
        Assert.True(m_identities.Register(value));
        return value;
    }

    private sealed class TestDiagnosticSink : IDiagnosticSink
    {
        public void Replace(DiagnosticReport report) => reports[report.source.id] = report;

        public void Clear(DiagnosticSource source) => reports.Remove(source.id);

        internal Dictionary<string, DiagnosticReport> reports { get; } = new(StringComparer.Ordinal);
    }
}

public sealed record InteractionPresentation(
    string value,
    bool submit,
    bool cancel = false
);
