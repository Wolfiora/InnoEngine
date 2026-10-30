using System;
using System.Collections.Generic;
using Inno.Core.Identity;
using Inno.Core.Input;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.Native.ImGui;
using NativeImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.ImGui.ImGuiWidget;
/// <summary>
/// Applies ordered selection gestures through the authoritative interaction owner.
/// </summary>
public sealed unsafe partial class ImGuiWidget
{
    /// <summary>
    /// Applies ordinary, additive or visible-range selection using the active keyboard modifiers.
    /// </summary>
    /// <param name="interactions">
    /// The session's authoritative interaction service.
    /// </param>
    /// <param name="target">
    /// The freshly resolved clicked target.
    /// </param>
    /// <param name="visibleTargets">
    /// Current runtime-identity view order; collapsed or filtered items must be omitted.
    /// </param>
    public void SelectTarget(
        EditorInteractions interactions,
        IdentityObject target,
        IReadOnlyList<RuntimeIdentity> visibleTargets
    ) {
        m_session.EnsureDrawing();
        ArgumentNullException.ThrowIfNull(interactions);
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(visibleTargets);
        ImGuiIO* io = NativeImGui.Raw.GetIO();
        bool additive = HasPrimaryModifier(interactions.keyboard, io);
        if (io->KeyShift != 0)
            interactions.SelectRange(visibleTargets, target, additive);
        else if (additive)
            interactions.ToggleSelection(target);
        else
            interactions.SetSelection(target);
    }

    /// <summary>
    /// Reads one focused list's keyboard navigation without retaining its entries or changing domain selection.
    /// </summary>
    /// <param name="interactions">
    /// The authoritative interaction owner supplying the product modifier policy.
    /// </param>
    /// <param name="itemCount">
    /// The number of currently visible, ordered entries.
    /// </param>
    /// <param name="focusedIndex">
    /// The current visible focus index, or minus one when absent.
    /// </param>
    /// <param name="pageSize">
    /// The positive number of visible rows advanced by Page Up or Page Down.
    /// </param>
    /// <returns>
    /// Neutral focus and selection intents; text editing, active controls and popups produce no intent.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The interaction owner is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The count, focus index or page size is invalid.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// No matching owner-thread drawing session is active.
    /// </exception>
    public ListNavigationResult NavigateList(
        EditorInteractions interactions,
        int itemCount,
        int focusedIndex,
        int pageSize = 10
    ) {
        m_session.EnsureDrawing();
        ArgumentNullException.ThrowIfNull(interactions);
        ArgumentOutOfRangeException.ThrowIfNegative(itemCount);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageSize);
        if (focusedIndex < -1 || focusedIndex >= itemCount && focusedIndex != -1)
            throw new ArgumentOutOfRangeException(nameof(focusedIndex));
        ImGuiIO* io = NativeImGui.Raw.GetIO();
        ImGuiNativeContext* context = NativeImGui.Raw.GetCurrentContext();
        bool ownsActiveRow = RouteTreeNavigationKeys(context);
        if (!NativeImGui.IsWindowFocused((int)Inno.Native.ImGui.ImGuiFocusedFlags.ChildWindows)
            || io->WantTextInput != 0 || (context->ActiveId != 0 && !ownsActiveRow) || ImGui.IsAnyPopupOpen())
            return new(focusedIndex, false, false, false, false, false, false, false);
        int next = focusedIndex;
        bool navigation = false;
        if (itemCount > 0)
        {
            navigation = true;
            if (NativeImGui.IsKeyPressed(Inno.Native.ImGui.ImGuiKey.DownArrow))
                next = focusedIndex < 0 ? 0 : Math.Min(itemCount - 1, focusedIndex + 1);
            else if (NativeImGui.IsKeyPressed(Inno.Native.ImGui.ImGuiKey.UpArrow))
                next = focusedIndex < 0 ? itemCount - 1 : Math.Max(0, focusedIndex - 1);
            else if (NativeImGui.IsKeyPressed(Inno.Native.ImGui.ImGuiKey.Home))
                next = 0;
            else if (NativeImGui.IsKeyPressed(Inno.Native.ImGui.ImGuiKey.End))
                next = itemCount - 1;
            else if (NativeImGui.IsKeyPressed(Inno.Native.ImGui.ImGuiKey.PageDown))
                next = focusedIndex < 0 ? 0 : (int)Math.Min(itemCount - 1L, (long)focusedIndex + pageSize);
            else if (NativeImGui.IsKeyPressed(Inno.Native.ImGui.ImGuiKey.PageUp))
                next = focusedIndex < 0 ? 0 : Math.Max(0, focusedIndex - pageSize);
            else
                navigation = false;
        }
        bool primary = HasPrimaryModifier(interactions.keyboard, io);
        bool extend = io->KeyShift != 0;
        bool space = itemCount > 0 && NativeImGui.IsKeyPressed(Inno.Native.ImGui.ImGuiKey.Space, false);
        bool activate = itemCount > 0 && (NativeImGui.IsKeyPressed(Inno.Native.ImGui.ImGuiKey.Enter, false)
            || NativeImGui.IsKeyPressed(Inno.Native.ImGui.ImGuiKey.KeypadEnter, false));
        bool cancel = NativeImGui.IsKeyPressed(Inno.Native.ImGui.ImGuiKey.Escape, false);
        if ((space || activate) && next < 0)
            next = 0;
        return new(next, next != focusedIndex, space || navigation && (!primary || extend),
            extend, primary, space && primary, activate, cancel);
    }

    private bool RouteTreeNavigationKeys(ImGuiNativeContext* context)
    {
        uint window = NativeImGui.Raw.GetCurrentWindow()->ID;
        if (!m_treeStatesByWindow.TryGetValue((m_session.owner, window), out TreeWidgetWindowState? state)
            || state.lastFrame != NativeImGui.GetFrameCount()
            || !NativeImGui.IsWindowFocused((int)Inno.Native.ImGui.ImGuiFocusedFlags.ChildWindows)
            || NativeImGui.Raw.GetIO()->WantTextInput != 0 || ImGui.IsAnyPopupOpen())
            return false;
        bool ownsActiveRow = context->ActiveId != 0
            && context->ActiveIdSource == ImGuiInputSource.Keyboard && state.navigationItemIds.Contains(context->ActiveId);
        if (state.navigationItemIds.Contains(context->NavId) && (context->ActiveId == 0 || ownsActiveRow))
        {
            // Native navigation must not activate the disclosure while the list owns selection/activation.
            // A distinct owner preserves text fields and overlay buttons, including their native navigation.
            uint owner = NativeImGui.GetID("##inno_list_navigation");
            NativeImGui.SetKeyOwner(Inno.Native.ImGui.ImGuiKey.Space, owner, 0);
            NativeImGui.SetKeyOwner(Inno.Native.ImGui.ImGuiKey.Enter, owner, 0);
            NativeImGui.SetKeyOwner(Inno.Native.ImGui.ImGuiKey.KeypadEnter, owner, 0);
        }
        return ownsActiveRow;
    }

    private static bool HasPrimaryModifier(
        EditorKeyboardPolicy keyboard,
        ImGuiIO* io
    ) {
        var input = new EditorUiInput(io);
        KeyModifier modifiers = KeyModifier.None;
        if (input.control)
            modifiers |= KeyModifier.Control;
        if (input.super)
            modifiers |= KeyModifier.Super;
        if (input.alt)
            modifiers |= KeyModifier.Alt;
        if (input.shift)
            modifiers |= KeyModifier.Shift;
        return (modifiers & keyboard.primaryModifier) == keyboard.primaryModifier;
    }
}

/// <summary>
/// Carries frame-local list navigation intents over visible indices without owning objects or selection state.
/// </summary>
public readonly struct ListNavigationResult
{
    internal ListNavigationResult(
        int focusedIndex,
        bool focusChanged,
        bool selectionRequested,
        bool extendRange,
        bool additive,
        bool toggle,
        bool activate,
        bool cancel
    ) {
        this.focusedIndex = focusedIndex;
        this.focusChanged = focusChanged;
        this.selectionRequested = selectionRequested;
        this.extendRange = extendRange;
        this.additive = additive;
        this.toggle = toggle;
        this.activate = activate;
        this.cancel = cancel;
    }

    /// <summary>
    /// Gets the resulting visible focus index, or minus one for no focus.
    /// </summary>
    public int focusedIndex { get; }
    /// <summary>
    /// Gets whether the visible focus index changed.
    /// </summary>
    public bool focusChanged { get; }
    /// <summary>
    /// Gets whether the caller should apply a domain selection operation.
    /// </summary>
    public bool selectionRequested { get; }
    /// <summary>
    /// Gets whether the selection should extend from its existing anchor.
    /// </summary>
    public bool extendRange { get; }
    /// <summary>
    /// Gets whether the product's primary modifier requests preserving other selected entries.
    /// </summary>
    public bool additive { get; }
    /// <summary>
    /// Gets whether Space with the primary modifier requests toggling the focused entry.
    /// </summary>
    public bool toggle { get; }
    /// <summary>
    /// Gets whether Enter requests activation of the focused entry.
    /// </summary>
    public bool activate { get; }
    /// <summary>
    /// Gets whether Escape requests canceling the current list workflow.
    /// </summary>
    public bool cancel { get; }
}
