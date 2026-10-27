using System;
using System.Numerics;
using Inno.Editor.ImGui.ImGuiWidget;
using Inno.Native.ImGui;
using Xunit;
using NativeImGui = Inno.Native.ImGui.ImGui;
using Ui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.PlayMode.Tests;

public sealed unsafe class TreeKeyboardOwnershipNativeTests
{
    [Fact]
    public void ListActivationDoesNotToggleDisclosureOrStealAnOverlayButton()
    {
        using var host = new NativePanelHost();
        bool rowFocus = true;
        bool overlayFocus = false;
        bool open = false;
        int toggles = 0;
        int overlayPresses = 0;
        bool initialize = true;
        uint overlayId = 0;
        string overlayState = string.Empty;
        ListNavigationResult navigation = default;
        Action draw = () =>
        {
            NativeImGui.SetNextWindowSize(new Vector2(600f, 400f), (int)ImGuiCond.Always);
            bool visible = true;
            _ = NativeImGui.Begin("Tree keyboard owner", ref visible, (int)ImGuiWindowFlags.NoSavedSettings);
            try
            {
                if (initialize)
                {
                    host.ui.widgets.SetNextTreeNodeOpen(true);
                    initialize = false;
                }
                TreeNodeResult row = host.ui.widgets.TreeNode("keyboard-row", _ => Ui.Text("A domain-independent row"), new()
                {
                    requestFocus = rowFocus,
                    drawViewportOverlay = () =>
                    {
                        NativeImGui.SameLine(0f, -1f);
                        if (overlayFocus)
                            Ui.FocusNextItem();
                        if (NativeImGui.SmallButton("Overlay"))
                            overlayPresses++;
                        overlayId = NativeImGui.Raw.GetCurrentContext()->LastItemData.ID;
                        ImGuiNativeContext* state = NativeImGui.Raw.GetCurrentContext();
                        overlayState = $"id={overlayId:X}, nav={state->NavId:X}, active={state->ActiveId:X}, activate={state->NavActivateId:X}, pressed={state->NavActivatePressedId:X}, keyOwner={NativeImGui.GetKeyOwner(ImGuiKey.Space):X}";
                    }
                });
                open = row.isOpen;
                if (open)
                    NativeImGui.TreePop();
                navigation = host.ui.widgets.NavigateList(host.runtime.interactions, 1, 0);
                if (navigation.toggle)
                    toggles++;
            }
            finally
            {
                NativeImGui.End();
            }
        };
        host.Frame(draw);
        host.Frame(draw);
        host.Frame(draw);
        rowFocus = false;
        ImGuiIO* io = NativeImGui.Raw.GetIO();
        NativeImGui.Raw.AddKeyEvent(io, ImGuiKey.ImGuiModCtrl, 1);
        NativeImGui.Raw.AddKeyEvent(io, ImGuiKey.Space, 1);
        host.Frame(draw);
        Assert.True(open);
        Assert.Equal(1, toggles);
        Assert.Equal(0, overlayPresses);
        NativeImGui.Raw.AddKeyEvent(io, ImGuiKey.Space, 0);
        NativeImGui.Raw.AddKeyEvent(io, ImGuiKey.ImGuiModCtrl, 0);
        host.Frame(draw);
        overlayFocus = true;
        host.Frame(draw);
        overlayFocus = false;
        host.Frame(draw);
        Assert.Equal(overlayId, NativeImGui.Raw.GetCurrentContext()->NavId);
        // Programmatic focus does not enter native keyboard navigation mode by itself.
        NativeImGui.SetNavCursorVisible(true);
        NativeImGui.Raw.AddKeyEvent(io, ImGuiKey.Space, 1);
        host.Frame(draw);
        NativeImGui.Raw.AddKeyEvent(io, ImGuiKey.Space, 0);
        host.Frame(draw);
        Assert.True(overlayPresses == 1, overlayState);
        Assert.False(navigation.selectionRequested);
        Assert.True(open);
    }
}
