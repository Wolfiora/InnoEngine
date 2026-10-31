using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using System;
using BGCS.Runtime;
using System.Numerics;
using System.Runtime.CompilerServices;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.Native.ImGui;
using NativeImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.ImGui;
/// <summary>
/// Renders centered editor modals consistently on the main viewport.
/// </summary>
internal static unsafe class EditorModalRenderer
{
    /// <summary>
    /// Draws a modal in the main viewport work area with a caller-provided opacity.
    /// </summary>
    /// <param name="id">
    /// The stable popup identity independent of the visible title.
    /// </param>
    /// <param name="title">
    /// The visible modal title.
    /// </param>
    /// <param name="alpha">
    /// The opacity applied to the complete modal window.
    /// </param>
    /// <param name="modal">
    /// The modal content and window presentation policy.
    /// </param>
    /// <param name="presentation">
    /// The generation-safe window presentation values.
    /// </param>
    /// <param name="context">
    /// The shared editor context supplied to the modal body.
    /// </param>
    // Current-frame sizing remains on the hot path in Debug hosts as well as optimized products.
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    internal static void Draw(
        string id,
        string title,
        float alpha,
        EditorModalExtension modal,
        EditorModalExtension.Presentation presentation,
        EditorContext context
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentNullException.ThrowIfNull(modal);
        ArgumentNullException.ThrowIfNull(context);
        EditorUiSession session = EditorUI.current;
        // The runtime established this session; revalidate below after invoking extension preparation.
        string popupId = $"{title}##{id}";
        ImGuiViewport* viewport = NativeImGui.Raw.GetMainViewport();
        var style = NativeImGui.Raw.GetStyle();
        Vector2 workSize = Vector2.Max(Vector2.One, ImGui.FromNative(viewport->WorkSize));
        Vector2 padding = ImGui.FromNative(style->WindowPadding);
        Vector2 bounds = Vector2.Max(Vector2.One, workSize - padding * 2f);
        Vector2 maximumSize = new(MathF.Floor(bounds.X), MathF.Floor(bounds.Y));
        Vector2 minimumSize = Vector2.Min(presentation.minimumSize * EditorUI.current.widgets.style.zoom, maximumSize);
        Vector2 initialSize = presentation.initialSize * EditorUI.current.widgets.style.zoom;
        if (presentation.canResize && initialSize.Y <= 0f)
        {
            using NativeStringLease nativeTitle = Utf8Codec.EncodeNullTerminated(popupId, allowsNull: false);
            ImGuiWindow* existing = NativeImGui.Raw.FindWindowByName((sbyte*)nativeTitle.pointer);
            if (existing != null && existing->WasActive != 0)
                initialSize = ImGui.FromNative(existing->SizeFull);
        }
        float width = MathF.Floor(Math.Clamp(initialSize.X > 0f ? initialSize.X : EditorUI.current.widgets.style.modalWidth,
            MathF.Max(1f, minimumSize.X), maximumSize.X));
        float decorations = NativeImGui.GetFrameHeight();
        Vector2 available = new(MathF.Max(1f, width - padding.X * 2f),
            MathF.Max(1f, (initialSize.Y > 0f ? MathF.Min(initialSize.Y, maximumSize.Y) : maximumSize.Y) - padding.Y * 2f - decorations));
        // Explicit resizable windows commit current input in Begin; measure their actual content area afterwards.
        bool prepareInside = presentation.canResize && initialSize.Y > 0f;
        Vector2? measured = null;
        if (!prepareInside && !modal.TryPrepare(context, presentation, available, out measured))
        {
            Close(id, title);
            return;
        }
        if (!ReferenceEquals(EditorUI.current, session))
            throw new InvalidOperationException("Modal preparation changed the active UI session.");
        session.EnsureDrawing();
        bool horizontalOverflow = false;
        if (presentation.allowScrolling && measured is Vector2 content)
        {
            horizontalOverflow = content.X > available.X;
            bool verticalOverflow = content.Y > available.Y - (horizontalOverflow ? style->ScrollbarSize : 0f);
            horizontalOverflow |= content.X > available.X - (verticalOverflow ? style->ScrollbarSize : 0f);
        }
        float height = initialSize.Y > 0f ? initialSize.Y : measured!.Value.Y + padding.Y * 2f + decorations;
        if (horizontalOverflow && initialSize.Y <= 0f)
            height += style->ScrollbarSize;
        Vector2 windowSize = new(width, MathF.Ceiling(Math.Clamp(height, MathF.Max(1f, minimumSize.Y), maximumSize.Y)));
        Vector2 center = ImGui.FromNative(viewport->WorkPos) + workSize * 0.5f;
        if (presentation.blocksInteraction)
            NativeImGui.OpenPopup(popupId, (int)ImGuiPopupFlags.NoReopen);
        NativeImGui.SetNextWindowViewport(viewport->ID);
        ImGuiCond placementCondition = presentation.canMove || presentation.canResize ? ImGuiCond.Appearing : ImGuiCond.Always;
        NativeImGui.SetNextWindowPos(center, (int)placementCondition, new Vector2(0.5f));
        NativeImGui.SetNextWindowSize(windowSize, (int)(presentation.canResize ? ImGuiCond.Appearing : ImGuiCond.Always));
        ImGui.ConstrainNativeWindowSize(minimumSize, maximumSize, null, null);
        if (measured is Vector2 extent)
            NativeImGui.SetNextWindowContentSize(extent);

        NativeImGui.PushStyleVar((int)(ImGuiStyleVar.Alpha), alpha);
        ImGuiWindowFlags flags = ImGuiWindowFlags.NoDocking | ImGuiWindowFlags.NoCollapse;
        if (horizontalOverflow)
            flags |= ImGuiWindowFlags.HorizontalScrollbar;
        if (!presentation.allowScrolling)
            flags |= ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse;
        if (!presentation.canMove)
            flags |= ImGuiWindowFlags.NoMove;
        if (!presentation.canResize)
            flags |= ImGuiWindowFlags.NoResize;

        bool beganWindow = false;
        try
        {
            bool visible = presentation.blocksInteraction ? ImGui.BeginNativeWindow(
                popupId,
                flags,
                modal: true
            ) : ImGui.BeginNativeWindow(
                popupId,
                flags | ImGuiWindowFlags.NoSavedSettings,
                modal: false
            );
            beganWindow = !presentation.blocksInteraction || visible;
            if (visible)
            {
                if (!prepareInside || modal.TryPrepare(context, presentation, Vector2.Max(Vector2.One, NativeImGui.GetContentRegionAvail()), out _))
                    _ = modal.Draw(context);
            }
        }
        finally
        {
            if (beganWindow)
            {
                if (presentation.blocksInteraction)
                    NativeImGui.EndPopup();
                else
                    NativeImGui.End();
            }

            NativeImGui.PopStyleVar();
        }
    }

    /// <summary>
    /// Closes a modal popup if it is currently open.
    /// </summary>
    /// <param name="id">
    /// The stable popup identity used when the modal was opened.
    /// </param>
    /// <param name="title">
    /// The visible title used when the modal was opened.
    /// </param>
    internal static void Close(
        string id,
        string title
    ) {
        string popupId = $"{title}##{id}";
        if (!NativeImGui.IsPopupOpen(popupId) || !ImGui.BeginNativeWindow(popupId, ImGuiWindowFlags.None, modal: true))
            return;
        try
        {
            NativeImGui.CloseCurrentPopup();
        }
        finally
        {
            NativeImGui.EndPopup();
        }
    }
}
