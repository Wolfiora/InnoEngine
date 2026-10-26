using System;
using System.Numerics;

using Inno.Native.ImGui;
using Inno.Adapter.Presentation.ImGui;
using NativeImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.ImGui.ImGuiWidget;

/// <summary>
/// Provides reusable editor controls and rendering helpers built on the native ImGui API.
/// </summary>
public static unsafe partial class ImGuiWidget
{
    /// <summary>
    /// Begins a combo whose popup opens below the control and stays within its containing window.
    /// </summary>
    /// <param name="id">
    /// Stable combo identifier in the current ImGui scope.
    /// </param>
    /// <param name="preview">
    /// Text displayed by the closed combo.
    /// </param>
    /// <param name="flags">
    /// Native combo presentation flags. Only <see cref="ImGuiComboFlags.WidthFitPreview"/> is supported.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the combo popup is open and its contents should be submitted;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="id"/> is empty.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="preview"/> is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="flags"/> contains a presentation mode unsupported by the editor selector.
    /// </exception>
    public static bool BeginBoundedCombo(
        string id,
        string preview,
        ImGuiComboFlags flags = ImGuiComboFlags.None
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentNullException.ThrowIfNull(preview);
        if ((flags & ~ImGuiComboFlags.WidthFitPreview) != 0)
            throw new ArgumentOutOfRangeException(nameof(flags), flags, "The combo flag is not supported by the editor selector.");

        ImGuiStyle* nativeStyle = NativeImGui.Raw.GetStyle();
        float width = (flags & ImGuiComboFlags.WidthFitPreview) != 0
            ? NativeImGui.CalcTextSize(preview).X + NativeImGui.GetFrameHeight() + nativeStyle->FramePadding.X * 2f
            : NativeImGui.CalcItemWidth();
        return BeginMenuSelector(id, preview, MathF.Max(1f, width), MathF.Max(1f, width));
    }

    /// <summary>
    /// Ends a combo opened by <see cref="BeginBoundedCombo"/>.
    /// </summary>
    public static void EndBoundedCombo() => EndMenuSelector();

    /// <summary>
    /// Draws a compact selector control and begins a work-area-bounded menu popup.
    /// </summary>
    /// <param name="id">
    /// Stable selector identifier in the current ImGui scope.
    /// </param>
    /// <param name="preview">
    /// Text displayed by the closed selector.
    /// </param>
    /// <param name="width">
    /// Width reserved for the selector control.
    /// </param>
    /// <param name="minimumPopupWidth">
    /// Minimum outer width of the popup, including its window padding.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the popup is open and its contents should be submitted;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="id"/> is empty.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="preview"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="width"/> or <paramref name="minimumPopupWidth"/> is not positive.
    /// </exception>
    public static bool BeginMenuSelector(
        string id,
        string preview,
        float width,
        float minimumPopupWidth
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentNullException.ThrowIfNull(preview);
        if (width <= 0f)
            throw new ArgumentOutOfRangeException(nameof(width), width, "Selector width must be positive.");
        if (minimumPopupWidth <= 0f)
        {
            throw new ArgumentOutOfRangeException(
                nameof(minimumPopupWidth),
                minimumPopupWidth,
                "Popup width must be positive.");
        }

        string popupId = $"##menu_selector_popup_{id}";
        Vector2 minimum = NativeImGui.GetCursorScreenPos();
        float height = NativeImGui.GetFrameHeight();
        Vector2 size = new(width, height);
        bool pressed = NativeImGui.InvisibleButton($"##menu_selector_{id}", size);
        bool hovered = NativeImGui.IsItemHovered();
        bool active = NativeImGui.IsItemActive();
        bool open = NativeImGui.IsPopupOpen(popupId);
        if (pressed)
            NativeImGui.OpenPopup(popupId);

        DrawMenuSelectorFrame(minimum, size, preview, hovered, active || open);
        Vector2 popupOrigin = new(minimum.X, minimum.Y + height);
        NativeImGui.SetNextWindowPos(popupOrigin, (int)ImGuiCond.Always, Vector2.Zero);
        ImGuiViewport* parentViewport = NativeImGui.Raw.GetWindowViewport();
        NativeImGui.SetNextWindowViewport(parentViewport->ID);
        SetWorkAreaPopupSize(
            MathF.Max(width, minimumPopupWidth),
            popupOrigin,
            parentViewport);
        return BeginMenuPopup(popupId);
    }

    /// <summary>
    /// Ends a selector popup opened by <see cref="BeginMenuSelector"/>.
    /// </summary>
    public static void EndMenuSelector() => EndMenuPopup();

    private static void SetWorkAreaPopupSize(
        float requestedWidth,
        Vector2 popupOrigin,
        ImGuiViewport* viewport
    ) {
        ImGuiWindow* containingWindow = NativeImGui.Raw.GetCurrentWindow()->RootWindow;
        Vector2 windowMaximum = ImGui.FromNative(containingWindow->Pos) + ImGui.FromNative(containingWindow->Size);
        Vector2 viewportMaximum = ImGui.FromNative(viewport->WorkPos) + ImGui.FromNative(viewport->WorkSize);
        float availableHeight = MathF.Max(1f, MathF.Min(windowMaximum.Y, viewportMaximum.Y) - popupOrigin.Y);
        float maximumHeight = MathF.Min(availableHeight, containingWindow->Size.Y * 0.45f);
        float availableWidth = MathF.Max(1f, MathF.Min(windowMaximum.X, viewportMaximum.X) - popupOrigin.X);
        float popupWidth = Math.Clamp(requestedWidth, 1f, availableWidth);
        SetMenuPopupSizeConstraints(
            new Vector2(popupWidth, 0f),
            new Vector2(popupWidth, MathF.Max(1f, maximumHeight)));
    }

    private static void DrawMenuSelectorFrame(
        Vector2 minimum,
        Vector2 size,
        string preview,
        bool hovered,
        bool active
    ) {
        ImGuiStyle* nativeStyle = NativeImGui.Raw.GetStyle();
        uint background = NativeImGui.GetColorU32(
            (int)(active ? ImGuiCol.FrameBgActive : hovered ? ImGuiCol.FrameBgHovered : ImGuiCol.FrameBg));
        ImDrawList* drawList = NativeImGui.Raw.GetWindowDrawList();
        Vector2 maximum = minimum + size;
        float arrowSlot = size.Y;
        float arrowMinimumX = MathF.Max(minimum.X, maximum.X - arrowSlot);
        ImDrawFlags previewRounding = size.X <= arrowSlot
            ? ImDrawFlags.RoundCornersAll
            : ImDrawFlags.RoundCornersLeft;
        ImGui.DrawNativeFilledRectangle(
            drawList,
            minimum,
            new Vector2(arrowMinimumX, maximum.Y),
            background,
            nativeStyle->FrameRounding,
            (int)(previewRounding)
        );
        uint arrowBackground = NativeImGui.GetColorU32(
            (int)(active || hovered ? ImGuiCol.ButtonHovered : ImGuiCol.Button));
        ImGui.DrawNativeFilledRectangle(
            drawList,
            new Vector2(arrowMinimumX, minimum.Y),
            maximum,
            arrowBackground,
            nativeStyle->FrameRounding,
            (int)(size.X <= arrowSlot ? ImDrawFlags.RoundCornersAll : ImDrawFlags.RoundCornersRight)
        );

        Vector2 textMinimum = minimum + ImGui.FromNative(nativeStyle->FramePadding);
        Vector2 textMaximum = new(arrowMinimumX, maximum.Y);
        ImGui.PushNativeClipRectangle(drawList, textMinimum, textMaximum, 1);
        ImGui.DrawNativeText(drawList, textMinimum, NativeImGui.GetColorU32((int)(ImGuiCol.Text)), preview);
        NativeImGui.Raw.PopClipRect(drawList);
        ImGui.DrawNativeArrow(
            drawList,
            new Vector2(arrowMinimumX + nativeStyle->FramePadding.Y, minimum.Y + nativeStyle->FramePadding.Y),
            NativeImGui.GetColorU32((int)(ImGuiCol.Text)),
            ImGuiDir.Down,
            1f);
        NativeImGui.RenderFrameBorder(minimum, maximum, nativeStyle->FrameRounding);
    }
}
