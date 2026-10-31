using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using Inno.Native.ImGui;
using NativeGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.ImGui.ImGuiWidget;

/// <summary>
/// Describes immutable content for a measured message dialog. Actions are returned to its owner, never retained as callbacks.
/// </summary>
/// <param name="message">
/// The current status or error text.
/// </param>
/// <param name="progress">
/// A completed fraction, or null when no progress bar is needed.
/// </param>
/// <param name="indeterminate">
/// Whether to show activity without inventing a completed fraction.
/// </param>
/// <param name="actionLabel">
/// The optional centered action label.
/// </param>
/// <param name="actionEnabled">
/// Whether the current operation admits the action.
/// </param>
/// <param name="details">
/// Optional expanded diagnostic text sharing the message scroll owner.
/// </param>
/// <param name="cancelOnEscape">
/// Whether Escape activates this action; opt in only for cancellation or dismissal.
/// </param>
public readonly record struct DialogPresentation(
    string message,
    float? progress = null,
    bool indeterminate = false,
    string? actionLabel = null,
    bool actionEnabled = true,
    string? details = null,
    bool cancelOnEscape = false
);

/// <summary>
/// Carries a measured dialog layout. Only its required extent is public; drawing mechanics remain owned by the widget.
/// </summary>
public readonly struct DialogLayout
{
    internal DialogLayout(
        Vector2 size,
        float textWidth,
        float bodyHeight,
        float bodyContentHeight,
        float spacing,
        float frameHeight,
        float actionWidth
    ) {
        this.size = size;
        this.textWidth = textWidth;
        this.bodyHeight = bodyHeight;
        this.bodyContentHeight = bodyContentHeight;
        this.spacing = spacing;
        this.frameHeight = frameHeight;
        this.actionWidth = actionWidth;
    }

    /// <summary>
    /// Gets the content extent, excluding the window title, border and padding.
    /// </summary>
    public Vector2 size { get; }
    internal float textWidth { get; }
    internal float bodyHeight { get; }
    internal float bodyContentHeight { get; }
    internal float spacing { get; }
    internal float frameHeight { get; }
    internal float actionWidth { get; }
}

/// <summary>
/// Presents measured message dialogs through the owning editor UI session.
/// </summary>
public sealed unsafe partial class ImGuiWidget
{
    private const int C_MAX_CACHED_DIALOG_CHARS = 16_384;
    private DialogMeasurementKey m_dialogKey;
    private DialogMeasurementKey m_previousDialogKey;
    private DialogLayout m_dialogLayout;
    private DialogLayout m_previousDialogLayout;

    /// <summary>
    /// Measures current dialog content before its window is opened. The returned geometry must be used with the same presentation.
    /// </summary>
    /// <param name="presentation">
    /// The immutable content captured for this frame.
    /// </param>
    /// <param name="availableSize">
    /// The bounded content area in already scaled presentation coordinates.
    /// </param>
    /// <returns>
    /// The bounded current-frame layout, including a single scrolling body when required.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The message is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The message, expanded details or action label contain an embedded zero code unit.
    /// </exception>
    /// <exception cref="System.Text.EncoderFallbackException">
    /// The message, expanded details or action label contain invalid Unicode.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The available dimensions are not finite and positive, or a determinate progress fraction is outside zero through one.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The owning frame, context or thread is not active.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// The owning UI session has retired.
    /// </exception>
    public DialogLayout MeasureDialog(
        DialogPresentation presentation,
        Vector2 availableSize
    ) {
        ImGuiNativeContext* native = m_session.RequireContext();
        if (native->WithinFrameScope == 0 || native->Font == null || native->FontBaked == null)
            throw new InvalidOperationException("Dialog measurement requires an active native frame and font.");
        ArgumentNullException.ThrowIfNull(presentation.message);
        if (presentation.progress is float fraction && (!float.IsFinite(fraction) || fraction < 0f || fraction > 1f))
            throw new ArgumentOutOfRangeException(nameof(presentation), "A determinate progress fraction must be between zero and one.");
        if (!float.IsFinite(availableSize.X) || !float.IsFinite(availableSize.Y) || availableSize.X <= 0f || availableSize.Y <= 0f)
            throw new ArgumentOutOfRangeException(nameof(availableSize));
        float width = MathF.Max(1f, MathF.Floor(availableSize.X));
        ImGuiStyle* nativeStyle = &native->Style;
        float spacing = nativeStyle->ItemSpacing.Y;
        float frame = MathF.Ceiling(native->FontSize + nativeStyle->FramePadding.Y * 2f);
        bool hasProgress = presentation.progress.HasValue || presentation.indeterminate;
        bool hasAction = !string.IsNullOrEmpty(presentation.actionLabel);
        ImFont* font = native->Font;
        ImFontBaked* baked = native->FontBaked;
        // Only value data is retained. Shared or remapped native fonts are measured on every request.
        bool memoize = font->OwnerAtlas->OwnerContext == native && font->RemapPairs.Data.Size == 0 &&
            (long)presentation.message.Length + (presentation.details?.Length ?? 0) + (presentation.actionLabel?.Length ?? 0) <= C_MAX_CACHED_DIALOG_CHARS;
        DialogMeasurementKey key = new(presentation.message, presentation.details, presentation.actionLabel,
            availableSize, hasProgress, font->FontId, baked->BakedId, baked->Glyphs.Size,
            baked->IndexAdvanceX.Size, font->Sources.Size, font->OwnerAtlas->FontNextUniqueId,
            native->FontSize, baked->RasterizerDensity, baked->FallbackAdvanceX,
            nativeStyle->ItemSpacing.Y, ImGui.FromNative(nativeStyle->FramePadding), nativeStyle->ScrollbarSize);
        if (memoize)
        {
            if (m_dialogLayout.size.X > 0f && m_dialogKey == key)
                return m_dialogLayout;
            if (m_previousDialogLayout.size.X > 0f && m_previousDialogKey == key)
                return m_previousDialogLayout;
        }
        float footer = (hasProgress ? frame : 0f) + (hasAction ? frame : 0f) + (hasProgress && hasAction ? spacing : 0f);
        float body = MeasureDialogBody(presentation, width, spacing);
        float availableBody = MathF.Max(0f, MathF.Floor(availableSize.Y - footer - (body > 0f && footer > 0f ? spacing : 0f)));
        float textWidth = width;
        if (body > availableBody)
        {
            textWidth = MathF.Max(1f, width - nativeStyle->ScrollbarSize);
            body = MeasureDialogBody(presentation, textWidth, spacing);
        }
        float visibleBody = MathF.Min(body, availableBody);
        float height = visibleBody + footer + (visibleBody > 0f && footer > 0f ? spacing : 0f);
        float actionWidth = hasAction ? ImGuiUtf8Buffer.MeasureText(presentation.actionLabel!, -1f).X + nativeStyle->FramePadding.X * 2f : 0f;
        DialogLayout result = new(new Vector2(width, MathF.Ceiling(height)), textWidth, visibleBody, body, spacing, frame, actionWidth);
        if (memoize)
        {
            m_previousDialogKey = m_dialogKey;
            m_previousDialogLayout = m_dialogLayout;
            m_dialogKey = key;
            m_dialogLayout = result;
        }
        return result;
    }

    /// <summary>
    /// Draws one prepared message dialog. Text owns one bounded scroll region and actions remain outside it.
    /// </summary>
    /// <param name="presentation">
    /// The same immutable content used for measurement.
    /// </param>
    /// <param name="layout">
    /// The measurement captured in this frame and context.
    /// </param>
    /// <returns>
    /// True when the enabled action was activated by native button input or its explicitly enabled Escape policy.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The message is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The layout is invalid.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The owning frame, context or thread is not active.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// The owning UI session has retired.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public bool DrawDialog(
        DialogPresentation presentation,
        DialogLayout layout
    ) {
        m_session.EnsureDrawing();
        ArgumentNullException.ThrowIfNull(presentation.message);
        if (!float.IsFinite(layout.size.X) || !float.IsFinite(layout.size.Y) || layout.size.X <= 0f || layout.size.Y < 0f)
            throw new ArgumentOutOfRangeException(nameof(layout));
        Vector2 origin = NativeGui.GetCursorScreenPos();
        bool scrolling = layout.bodyContentHeight > layout.bodyHeight;
        if (layout.bodyHeight > 0f)
        {
            if (scrolling)
            {
                // The modal owns the outer inset; its scrolling body must not apply a second padding.
                NativeGui.PushStyleVar((int)ImGuiStyleVar.WindowPadding, Vector2.Zero);
                try
                {
                    NativeGui.SetNextWindowContentSize(new Vector2(0f, layout.bodyContentHeight));
                    ImVec2C bodySize = ImGui.ToNative(new Vector2(layout.size.X, layout.bodyHeight));
                    ReadOnlySpan<byte> bodyId = "##dialog-body\0"u8;
                    bool visible;
                    fixed (byte* id = bodyId)
                        visible = NativeGui.Raw.BeginChild((sbyte*)id, &bodySize, 0, 0) != 0;
                    try
                    {
                        if (visible)
                            DrawDialogBody(presentation, layout.textWidth);
                    }
                    finally
                    {
                        NativeGui.EndChild();
                    }
                }
                finally
                {
                    NativeGui.PopStyleVar();
                }
            }
            else
                DrawDialogBody(presentation, layout.textWidth);
        }
        float y = layout.bodyHeight;
        if (y > 0f && (presentation.progress.HasValue || presentation.indeterminate || !string.IsNullOrEmpty(presentation.actionLabel)))
            y += layout.spacing;
        if (presentation.progress.HasValue || presentation.indeterminate)
        {
            NativeGui.SetCursorScreenPos(origin + new Vector2(0f, y));
            float fraction = presentation.progress ?? 0f;
            CenteredProgressBar(presentation.indeterminate ? -(float)NativeGui.GetTime() : fraction,
                new Vector2(layout.size.X, layout.frameHeight), presentation.indeterminate ? "Working…" : $"{fraction:P0}");
            y += layout.frameHeight + layout.spacing;
        }
        bool activated = false;
        if (!string.IsNullOrEmpty(presentation.actionLabel))
        {
            bool disabled = !presentation.actionEnabled;
            if (disabled)
                NativeGui.BeginDisabled(true);
            try
            {
                activated = DrawCenteredButton(presentation.actionLabel, origin + new Vector2(0f, y), layout.size.X, layout.actionWidth);
                if (presentation.cancelOnEscape && presentation.actionEnabled && NativeGui.IsWindowFocused((int)Inno.Native.ImGui.ImGuiFocusedFlags.RootAndChildWindows) &&
                    NativeGui.IsKeyPressed(Inno.Native.ImGui.ImGuiKey.Escape, false))
                    activated = true;
            }
            finally
            {
                if (disabled)
                    NativeGui.EndDisabled();
            }
        }
        return activated;
    }

    private void ClearDialogMeasurements()
    {
        m_dialogKey = default;
        m_previousDialogKey = default;
        m_dialogLayout = default;
        m_previousDialogLayout = default;
    }

    private readonly record struct DialogMeasurementKey(
        string message,
        string? details,
        string? action,
        Vector2 available,
        bool progress,
        uint font,
        uint baked,
        int glyphs,
        int advances,
        int sources,
        int nextFont,
        float fontSize,
        float density,
        float fallbackAdvance,
        float spacing,
        Vector2 framePadding,
        float scrollbar
    );

    private static float MeasureDialogBody(
        DialogPresentation presentation,
        float width,
        float spacing
    ) {
        float height = presentation.message.Length == 0 ? 0f : ImGuiUtf8Buffer.MeasureText(presentation.message, width).Y;
        if (!string.IsNullOrEmpty(presentation.details))
            height += (height > 0f ? spacing : 0f) + ImGuiUtf8Buffer.MeasureText(presentation.details, width).Y;
        return MathF.Ceiling(height);
    }

    private static void DrawDialogBody(
        DialogPresentation presentation,
        float width
    ) {
        NativeGui.PushTextWrapPos(NativeGui.GetCursorPosX() + width);
        try
        {
            if (presentation.message.Length != 0)
                ImGuiUtf8Buffer.DrawText(presentation.message);
            if (!string.IsNullOrEmpty(presentation.details))
            {
                // Native text already contributes the same inter-item spacing used by measurement.
                ImGuiUtf8Buffer.DrawText(presentation.details);
            }
        }
        finally
        {
            NativeGui.PopTextWrapPos();
        }
    }
}
