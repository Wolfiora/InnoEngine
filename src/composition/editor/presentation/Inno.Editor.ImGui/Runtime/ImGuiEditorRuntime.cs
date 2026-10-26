using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Inno.Core.Events;
using Inno.Core.Execution;
using Inno.Core.Logging;
using Inno.Core.Input;
using Inno.Extensibility.Types;
using Inno.Editor.Core;
using Inno.Editor.ImGui.ImGuiWidget;
using Inno.Editor.Interactions;
using Inno.Native.ImGui;
using NativeImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.ImGui;
/// <summary>
/// Presents the backend-independent editor interaction runtime through ImGui.
/// </summary>
public sealed unsafe class ImGuiEditorRuntime : EditorRuntime
{
    private readonly Stopwatch m_timer = Stopwatch.StartNew();
    private readonly EditorInteractionRuntime m_runtime;
    private readonly EditorUiSession m_ui = new();
    private readonly EditorModalHost m_modals = new();
    private readonly HashSet<string> m_openOwners = new(StringComparer.Ordinal);
    private readonly HashSet<string> m_currentOwners = new(StringComparer.Ordinal);
    private readonly List<string> m_retiredOwners = [];
    private uint m_dockspaceId;
    private Vector2 m_dockspaceSize;
    private bool m_disposed;
    /// <summary>
    /// Creates an ImGui editor runtime with stable host-owned extension services.
    /// </summary>
    /// <param name="context">
    /// The shared editor context that owns project settings and frame state.
    /// </param>
    /// <param name="types">
    /// The host-owned type catalog that coordinates extension generations.
    /// </param>
    /// <param name="logs">
    /// The host-owned logging router used by editor infrastructure.
    /// </param>
    /// <param name="hostServices">
    /// Services available to discovered extension constructors.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name = "context"/>, <paramref name="types"/>, or
    /// <paramref name="hostServices"/> is <see langword="null"/>.
    /// </exception>
    public ImGuiEditorRuntime(
        EditorContext context,
        TypeCatalog types,
        LogRouter logs,
        IEnumerable<object> hostServices
    ) : base(context ?? throw new ArgumentNullException(nameof(context)))
    {
        ArgumentNullException.ThrowIfNull(types);
        ArgumentNullException.ThrowIfNull(logs);
        ArgumentNullException.ThrowIfNull(hostServices);
        var services = new List<object>(hostServices)
        {
            m_ui
        };
        m_runtime = new EditorInteractionRuntime(context, types, logs, services);
        m_ui.Connect(m_runtime.interactions);
        using IDisposable scope = m_ui.Enter();
        m_ui.widgets.SetupStyle();
    }

    /// <summary>
    /// Gets the active presentation-independent interaction entry point.
    /// </summary>
    public EditorInteractions interactions => m_runtime.interactions;
    /// <summary>
    /// Gets the number of active dockable panels.
    /// </summary>
    public int panelCount => m_runtime.panelCount;

    /// <summary>
    /// Starts value processing after validating the current state.
    /// </summary>
    public override void Start()
    {
        using IDisposable scope = m_ui.Enter();
        m_runtime.Start();
    }

    /// <summary>
    /// Recomputes owned state from the current validated inputs.
    /// </summary>
    /// <param name="frame">
    /// The frame consumed by update; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public override void Update(EditorFrame frame)
    {
        using IDisposable scope = m_ui.Enter();
        m_runtime.Update(frame);
    }

    /// <summary>
    /// Captures all stateful active modules and panels and flushes their project state to disk.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    /// Thrown after this runtime has been disposed.
    /// </exception>
    public void SaveState()
    {
        using IDisposable uiScope = m_ui.Enter();
        ObjectDisposedException.ThrowIf(m_disposed, this);
        m_runtime.SaveState();
    }

    /// <summary>
    /// Freezes automatic extension-state persistence and writes the final project state before editor
    /// modules begin shutting down.
    /// </summary>
    /// <remarks>
    /// This operation is idempotent and prevents module teardown from overwriting the saved
    /// module sections with transient empty state.
    /// </remarks>
    /// <exception cref="ObjectDisposedException">
    /// Thrown after this runtime has been disposed.
    /// </exception>
    public void PrepareShutdown()
    {
        using IDisposable uiScope = m_ui.Enter();
        ObjectDisposedException.ThrowIf(m_disposed, this);
        m_ui.widgets.CommitEdits();
        m_runtime.PrepareShutdown();
    }

    /// <summary>
    /// Draws the complete editor frame through ImGui.
    /// </summary>
    public void Draw()
    {
        using IDisposable uiScope = m_ui.Enter();
        ObjectDisposedException.ThrowIf(m_disposed, this);
        m_ui.widgets.BeginEditingFrame();
        EditorUI.current.widgets.ApplyPendingStyle();
        IReadOnlyList<EditorModalExtension> modals = m_runtime.modals;
        double now = m_timer.Elapsed.TotalSeconds;
        bool blocksInteraction = m_modals.Update(modals, now);
        if (blocksInteraction)
            NativeImGui.BeginDisabled(true);
        try
        {
            // The main menu must reserve the viewport work area before the dockspace snapshots it.
            // Reversing this order makes the dockspace overlap the bottom of the menu and clip the
            // first docked tab bar row.
            EditorUI.current.menus.MainMenu(interactions.For(ImGuiInteractionIds.C_MAIN_MENU_AREA));
            DrawDockSpace();
            DrawPanels(m_runtime.panels);
        }
        finally
        {
            if (blocksInteraction)
                NativeImGui.EndDisabled();
        }

        m_ui.widgets.CompleteEditingFrame();
        m_runtime.Flush();
        // Menu actions can change modal visibility after the pre-draw transition update.
        modals = m_runtime.modals;
        _ = m_modals.Update(modals, now);
        m_modals.Draw(context, modals, now);
    }

    private void DrawDockSpace()
    {
        ImGuiViewport* viewport = NativeImGui.Raw.GetMainViewport();
        Vector2 size = ImGui.FromNative(viewport->WorkSize);
        ImGuiStyle* style = NativeImGui.Raw.GetStyle();
        if (m_dockspaceId == 0)
        {
            uint hostId = ImGui.HashNativeId($"WindowOverViewport_{viewport->ID:X8}");
            m_dockspaceId = ImGui.HashNativeId("DockSpace", hostId);
            ImGuiDockNode* loaded = NativeImGui.Raw.DockBuilderGetNode(m_dockspaceId);
            if (loaded != null)
                ResizeDockSplits(loaded, size, style->DockingSeparatorSize, ImGui.FromNative(style->WindowMinSize));
        }
        else if (m_dockspaceSize != Vector2.Zero && (MathF.Abs(size.X - m_dockspaceSize.X) >= 0.5f || MathF.Abs(size.Y - m_dockspaceSize.Y) >= 0.5f))
        {
            ImGuiDockNode* root = NativeImGui.Raw.DockBuilderGetNode(m_dockspaceId);
            if (root != null)
                ResizeDockSplits(root, size, style->DockingSeparatorSize, ImGui.FromNative(style->WindowMinSize));
        }

        NativeImGui.PushStyleColor((int)(ImGuiCol.ResizeGripHovered), EditorPalette.accentHovered);
        NativeImGui.PushStyleColor((int)(ImGuiCol.ResizeGripActive), EditorPalette.accentActive);
        try
        {
            uint submittedId = NativeImGui.Raw.DockSpaceOverViewport(0, null, 0, null);
            if (submittedId != m_dockspaceId)
                throw new InvalidOperationException("The ImGui viewport dockspace identity changed before layout could be resized.");
        }
        finally
        {
            NativeImGui.PopStyleColor(2);
        }

        if (m_dockspaceSize == Vector2.Zero)
        {
            ImGuiDockNode* root = NativeImGui.Raw.DockBuilderGetNode(m_dockspaceId);
            if (root != null)
                ResizeDockSplits(root, size, style->DockingSeparatorSize, ImGui.FromNative(style->WindowMinSize));
        }

        m_dockspaceSize = size;
    }

    private static unsafe void ResizeDockSplits(
        ImGuiDockNode* node,
        Vector2 size,
        float separator,
        Vector2 minimumWindowSize
    ) {
        ImGuiDockNode* first = node->ChildNodes[0];
        ImGuiDockNode* second = node->ChildNodes[1];
        if (first == null || second == null || node->SplitAxis is not (ImGuiAxis.X or ImGuiAxis.Y))
            return;
        bool horizontal = node->SplitAxis == ImGuiAxis.X;
        float firstReference = horizontal ? first->Size.X : first->Size.Y;
        float secondReference = horizontal ? second->Size.X : second->Size.Y;
        float previousAvailable = (horizontal ? node->Size.X : node->Size.Y) - separator;
        if (firstReference <= 0f || secondReference <= 0f || MathF.Abs(firstReference + secondReference - previousAvailable) > MathF.Max(1f, separator))
        {
            firstReference = horizontal ? first->SizeRef.X : first->SizeRef.Y;
            secondReference = horizontal ? second->SizeRef.X : second->SizeRef.Y;
        }

        float available = (horizontal ? size.X : size.Y) - separator;
        if (!float.IsFinite(firstReference) || !float.IsFinite(secondReference) || firstReference <= 0f || secondReference <= 0f || available <= 1f)
            return;
        float minimum = MathF.Min(available * 0.5f, horizontal ? minimumWindowSize.X : minimumWindowSize.Y);
        float firstExtent = Math.Clamp(available * firstReference / (firstReference + secondReference), minimum, available - minimum);
        Vector2 firstSize = size;
        Vector2 secondSize = size;
        if (horizontal)
        {
            firstSize.X = firstExtent;
            secondSize.X = available - firstExtent;
        }
        else
        {
            firstSize.Y = firstExtent;
            secondSize.Y = available - firstExtent;
        }

        first->SizeRef = ImGui.ToNative(firstSize);
        second->SizeRef = ImGui.ToNative(secondSize);
        ResizeDockSplits(first, firstSize, separator, minimumWindowSize);
        ResizeDockSplits(second, secondSize, separator, minimumWindowSize);
    }

    /// <summary>
    /// Dispatches contextual shortcuts, leaving editing keys with active text widgets while permitting explicit save.
    /// </summary>
    /// <param name="keyEvent">
    /// The keyboard event received from the application event stream.
    /// </param>
    public void HandleKeyPressed(KeyPressedEvent keyEvent)
    {
        using IDisposable uiScope = m_ui.Enter();
        ArgumentNullException.ThrowIfNull(keyEvent);
        if (m_modals.Update(m_runtime.modals, m_timer.Elapsed.TotalSeconds))
            return;
        // Text widgets own editing keys, including Backspace and clipboard/history gestures.
        // Explicit document save remains available without first leaving an input field.
        HotKeyGesture save = HotKeyGesture.Primary(KeyCode.S, context.keyboard);
        if (NativeImGui.Raw.GetIO()->WantTextInput != 0 && (keyEvent.key != save.key || keyEvent.modifiers != save.modifiers))
            return;
        m_ui.widgets.CommitEdits();
        m_runtime.HandleKeyPressed(keyEvent);
    }

    /// <summary>
    /// Releases the resources owned by this implementation.
    /// </summary>
    /// <exception cref="RetirementPendingException">
    /// Editor extensions still own live work. The interaction runtime remains retained for safe shutdown.
    /// </exception>
    public override void Dispose()
    {
        if (m_disposed)
            return;
        m_modals.Clear();
        try
        {
            using (m_ui.Enter())
            {
                m_ui.widgets.CommitEdits();
                m_runtime.Dispose();
            }

            m_ui.Dispose();
        }
        catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
        {
            throw;
        }
        catch
        {
            m_disposed = true;
            throw;
        }

        m_disposed = true;
        GC.SuppressFinalize(this);
    }

    private void DrawPanels(IReadOnlyList<EditorPanelExtension> panels)
    {
        m_currentOwners.Clear();
        for (int i = 0; i < panels.Count; i++)
        {
            EditorPanelExtension extension = panels[i];
            m_currentOwners.Add(extension.id);
            if (!extension.isOpen)
            {
                if (m_openOwners.Remove(extension.id))
                    m_ui.RetireOwner(extension.id);
                continue;
            }

            m_openOwners.Add(extension.id);
            using IDisposable ownerScope = m_ui.EnterOwner(extension.id);
            if (!extension.TryGetWindowPresentation(out bool useWindowPadding, out bool allowScrolling, out Vector2 initialSize))
                continue;
            bool isOpen = extension.isOpen;
            if (initialSize.X > 0 && initialSize.Y > 0)
                NativeImGui.SetNextWindowSize(initialSize, (int)(ImGuiCond.FirstUseEver));
            ImGuiWindowFlags flags = ImGuiWindowFlags.NoCollapse;
            if (!allowScrolling)
                flags |= ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse;
            if (extension.TakeFocusRequest())
                NativeImGui.SetNextWindowFocus();
            EditorUI.current.widgets.PanelWindow(extension.title, ref isOpen, () =>
            {
                if (extension.Draw(context) && NativeImGui.IsWindowFocused((int)(Inno.Native.ImGui.ImGuiFocusedFlags.RootAndChildWindows)))
                {
                    interactions.For($"panel/{extension.id}", interactions.selection.selectedTarget).Focus();
                }
            }, flags, useWindowPadding);
            extension.isOpen = isOpen;
        }

        m_retiredOwners.Clear();
        foreach (string owner in m_openOwners)
            if (!m_currentOwners.Contains(owner))
                m_retiredOwners.Add(owner);
        foreach (string owner in m_retiredOwners)
        {
            m_ui.RetireOwner(owner);
            m_openOwners.Remove(owner);
        }

        m_retiredOwners.Clear();
    }
}
