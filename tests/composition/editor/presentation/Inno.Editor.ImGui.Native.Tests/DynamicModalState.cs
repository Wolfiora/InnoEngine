using System.Numerics;
using Inno.Editor.ImGui.ImGuiWidget;

namespace Inno.Editor.ImGui.Native.Tests;

public sealed class DynamicModalState
{
    public string message = "Short";
    public float zoom = 1f;
    public bool showAction = true;
    public bool showProgress = true;
    public bool indeterminate;
    public string? details;
    public Vector2 initialSize;
    public bool resizable;
    public bool visible = true;
    public bool rejectPreparation;
    public bool rejectDraw;
    public bool canceled;
    public int actions;
    public bool popupOpen;
    public Vector2 actionMin;
    public float scrollMaxX;
    public float wrapWidth;
    public float bodyHeight;
    public float bodyContentHeight;
    public int preparations;
    public int draws;
    public DialogPresentation content;
    public DialogLayout layout;
    public Vector2 windowSize;
    public Vector2 windowMax;
    public Vector2 actionMax;
}
