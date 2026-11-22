using System;
using System.Numerics;
using Inno.Editor.Core;
using Inno.Native.ImGui;

using NativeGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.ImGui.Native.Tests;

[EditorModal("tests.dynamic-modal", "Dynamic Modal")]
public sealed unsafe class DynamicTestModal(DynamicModalState state) : EditorModal
{
    public override bool isVisible => state.visible;
    public override bool allowScrolling => false;
    public override Vector2 initialSize => state.initialSize;
    public override bool canResize => state.resizable;

    protected override Vector2? OnPrepare(
        EditorContext context,
        Vector2 availableContentSize
    ) {
        state.preparations++;
        if (state.rejectPreparation) throw new InvalidOperationException("Rejected modal measurement.");
        EditorUISession.current.widgets.style.SetZoom(state.zoom);
        state.content = new(
            state.message,
            state.showProgress ? 0.5f : null,
            state.indeterminate,
            actionLabel: state.showAction ? "Cancel" : null,
            actionEnabled: !state.canceled,
            details: state.details,
            cancelOnEscape: true
        );
        state.layout = EditorUISession.current.widgets.MeasureDialog(state.content, availableContentSize);
        return state.layout.size;
    }

    protected override void OnDraw(EditorContext context)
    {
        state.draws++;
        state.windowSize = NativeGui.GetWindowSize();
        state.windowMax = NativeGui.GetWindowPos() + state.windowSize;
        if (EditorUISession.current.widgets.DrawDialog(state.content, state.layout))
        {
            state.actions++;
            state.canceled = true;
        }
        state.actionMin = NativeGui.GetItemRectMin();
        state.scrollMaxX = NativeGui.GetScrollMaxX();
        ImGuiWindow* window = NativeGui.Raw.GetCurrentWindow();
        state.wrapWidth = NativeGui.GetContentRegionAvail().X;
        state.bodyHeight = state.message.Length == 0 ? 0f : NativeGui.CalcTextSize(state.message, false, state.wrapWidth).Y;
        state.bodyContentHeight = state.bodyHeight;
        if (window->DC.ChildWindows.Size > 0)
        {
            ImGuiWindow* child = window->DC.ChildWindows.Data[0];
            state.wrapWidth = child->WorkRect.Max.X - child->WorkRect.Min.X;
            state.bodyHeight = child->Size.Y;
            state.bodyContentHeight = child->ContentSize.Y;
        }
        if (state.rejectDraw) throw new InvalidOperationException("Rejected modal drawing.");
        state.actionMax = NativeGui.GetItemRectMax();
    }
}
