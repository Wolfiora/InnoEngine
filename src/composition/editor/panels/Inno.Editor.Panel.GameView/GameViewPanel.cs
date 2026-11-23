using System;
using System.Collections.Generic;
using System.Numerics;
using Inno.Core.Input;
using Inno.Core.Settings;
using Inno.Editor.Core;
using Inno.Editor.ImGui;
using Inno.Editor.PlayMode;
using Inno.Editor.Rendering;
using Inno.Editor.Scene;
using Inno.Editor.Settings;
using Inno.References;
using Inno.Rendering;
using Inno.Rendering.Runtime;
using Inno.Runtime;

using EditorUI = Inno.Editor.ImGui.EditorUISession;
using EngineColor = Inno.Core.Mathematics.Color;
using EngineVector2 = Inno.Core.Mathematics.Vector2;
using Gui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.GameView;

/// <summary>
/// Presents the active Plugin provider for the open Game viewport purpose.
/// </summary>
[EditorPanel("rendering.game-view", "Game", order: 220, menuPath: "Viewports")]
internal sealed class GameViewPanel : EditorPanel
{
    private const string C_VIEWPORT_ID = "game-view";
    private static readonly EditorViewportKindID KIND = new("inno.editor.viewport.game");
    private static readonly Vector2 UNAVAILABLE_PADDING = new(48f, 32f);
    private readonly EditorRenderingModule m_rendering;
    private readonly IEditorGameScenePresentation m_scenePresentation;
    private readonly IEditorPlayMode m_playMode;
    private readonly EditorPlayModeLoop m_playModeLoop;
    private readonly EditorGameInputCapture m_inputCapture;
    private readonly EditorSettings m_editorSettings;
    private readonly ProjectSettingsStore m_projectSettings;
    private IDisposable? m_settingsSubscription;
    private Vector4 m_backgroundColor;
    private GamePresentationSettings m_presentation = new();
    private long m_presentationRevision = -1;
    private bool m_leftPointerActive;

    internal GameViewPanel(
        EditorRenderingModule rendering,
        IEditorGameScenePresentation scenePresentation,
        IEditorPlayMode playMode,
        EditorPlayModeLoop playModeLoop,
        EditorGameInputCapture inputCapture,
        EditorSettings editorSettings,
        ProjectSettingsStore projectSettings
    ) {
        ArgumentNullException.ThrowIfNull(rendering);
        ArgumentNullException.ThrowIfNull(scenePresentation);
        ArgumentNullException.ThrowIfNull(playMode);
        ArgumentNullException.ThrowIfNull(playModeLoop);
        ArgumentNullException.ThrowIfNull(inputCapture);
        ArgumentNullException.ThrowIfNull(editorSettings);
        ArgumentNullException.ThrowIfNull(projectSettings);
        m_rendering = rendering;
        m_scenePresentation = scenePresentation;
        m_playMode = playMode;
        m_playModeLoop = playModeLoop;
        m_inputCapture = inputCapture;
        m_editorSettings = editorSettings;
        m_projectSettings = projectSettings;
    }

    /// <summary>
    /// Gets whether use window padding is enabled for this implementation.
    /// </summary>
    public override bool useWindowPadding => false;

    /// <summary>
    /// Gets whether allow scrolling is enabled for this implementation.
    /// </summary>
    public override bool allowScrolling => false;

    /// <summary>
    /// Draws this feature using the current editor presentation context.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    protected override void OnDraw(EditorContext context)
    {
        using IDisposable presentationScope = m_playModeLoop.EnterPresentationScope();
        _ = context;
        Vector2 available = Gui.GetContentRegionAvailable();
        if (available.X <= 0f || available.Y <= 0f)
        {
            return;
        }
        RefreshPresentationSettings();
        float framebufferScale = Gui.GetInput().framebufferScale.X;
        if (!float.IsFinite(framebufferScale) || framebufferScale <= 0f)
        {
            framebufferScale = 1f;
        }
        GameViewportLayout layout = CalculateLayout(available, m_presentation, framebufferScale);
        m_rendering.SetPresentation(
            C_VIEWPORT_ID,
            new EditorViewportPresentation(
                new EngineColor(m_backgroundColor.X, m_backgroundColor.Y, m_backgroundColor.Z, m_backgroundColor.W),
                framebufferScale
            )
        );
        m_rendering.SetContentScope(C_VIEWPORT_ID, CreateContentScope());
        Vector2 outputOrigin = Gui.GetCursorScreenPosition() + layout.offset;
        Vector2 mouse = Gui.GetMousePos();
        Vector2 local = mouse - outputOrigin;
        bool inside = local.X >= 0f && local.Y >= 0f && local.X < layout.size.X && local.Y < layout.size.Y;
        (uint id, Vector2 position) viewport = Gui.GetViewport();
        bool gameViewFocused = m_playMode.isPlaying && Gui.IsWindowFocused(ImGuiFocusedFlags.RootAndChildWindows);
        bool imageHovered = inside && Gui.IsWindowHovered(ImGuiHoveredFlags.RootAndChildWindows);
        bool focused = m_inputCapture.Report(viewport.id, outputOrigin - viewport.position, layout.size, gameViewFocused, imageHovered);
        bool acceptsPointer = focused && imageHovered;
        if (!Gui.IsMouseButtonDown(ImGuiMouseButton.Left) && !Gui.IsMouseReleased(ImGuiMouseButton.Left))
        {
            m_leftPointerActive = false;
        }

        bool leftPressed = acceptsPointer && Gui.IsMouseClicked(ImGuiMouseButton.Left);
        if (leftPressed)
        {
            m_leftPointerActive = true;
        }
        bool leftReleased = m_leftPointerActive && Gui.IsMouseReleased(ImGuiMouseButton.Left);
        if (leftReleased || !focused)
        {
            m_leftPointerActive = false;
        }
        EditorUIInput io = Gui.GetInput();
        KeyModifier modifiers = KeyModifier.None;
        if (io.alt)
        {
            modifiers |= KeyModifier.Alt;
        }
        if (io.control)
        {
            modifiers |= KeyModifier.Control;
        }
        if (io.shift)
        {
            modifiers |= KeyModifier.Shift;
        }
        if (io.super)
        {
            modifiers |= KeyModifier.Super;
        }
        m_rendering.SetOutputInput(
            C_VIEWPORT_ID,
            focused
            ? new RenderOutputInput(
                new EngineVector2(
                    local.X * layout.pixelWidth / layout.size.X,
                    local.Y * layout.pixelHeight / layout.size.Y
                ),
                acceptsPointer,
                acceptsPointer ? new EngineVector2(0f, io.wheel) : default,
                modifiers,
                [],
                [],
                leftPressed ? [MouseButton.Left] : [],
                leftReleased ? [MouseButton.Left] : [],
                []
            )
            : RenderOutputInput.suspended
        );
        if (!m_rendering.TrySubmit(KIND, C_VIEWPORT_ID, layout.pixelWidth, layout.pixelHeight, out EditorViewportOutput output))
        {
            DrawUnavailable(available, m_rendering.GetCompositionError(C_VIEWPORT_ID) ?? "No rendering model contributes to Game View.");
            return;
        }

        if (!output.isReady)
        {
            DrawUnavailable(available, "Preparing Game View GPU target...");
            return;
        }

        Vector2 origin = Gui.GetCursorScreenPosition();
        if (m_presentation.preserveAspectRatio)
        {
            Gui.DrawFilledRectangle(
                origin,
                origin + available,
                Gui.ToPackedColor(new Vector4(0f, 0f, 0f, EditorPalette.C_OPACITY_OPAQUE)),
                0
            );
        }

        Gui.SetCursorScreenPosition(origin + layout.offset);
        m_rendering.Draw(output, layout.size);
        Gui.SetCursorScreenPosition(origin);
        Gui.Dummy(available);
    }

    /// <summary>
    /// Attaches this feature to its owning runtime generation.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    protected override void OnAttach(EditorContext context)
    {
        _ = context;
        ApplyEditorSettings(m_editorSettings);
        RefreshPresentationSettings();
        m_settingsSubscription = m_editorSettings.ListenChanges(ApplyEditorSettings);
    }

    /// <summary>
    /// Detaches this feature and releases generation-scoped state.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    protected override void OnDetach(EditorContext context)
    {
        _ = context;
        m_settingsSubscription?.Dispose();
        m_rendering.Release(C_VIEWPORT_ID);
        m_presentationRevision = -1;
        m_leftPointerActive = false;
    }

    private void ApplyEditorSettings(EditorSettings settings) => m_backgroundColor = GameViewBackgroundSetting.Read(settings);

    private void RefreshPresentationSettings()
    {
        if (m_presentationRevision == m_projectSettings.revision)
        {
            return;
        }
        m_presentation = m_projectSettings.Get<GamePresentationSettings>(GamePresentationSettings.settingID);
        m_presentationRevision = m_projectSettings.revision;
    }

    private ContentReadScope CreateContentScope() => m_scenePresentation.Capture();

    private void DrawUnavailable(
        Vector2 size,
        string message
    ) {
        Vector2 minimum = Gui.GetCursorScreenPosition();
        Gui.DrawFilledRectangle(minimum, minimum + size, Gui.ToPackedColor(m_backgroundColor), 0);
        Gui.PushStyleColor(ImGuiCol.Text, EditorPalette.textDisabled);
        try
        {
            EditorUI.current.widgets.CenteredWrappedText(message, size, UNAVAILABLE_PADDING);
        }
        finally
        {
            Gui.PopStyleColor();
        }
    }

    private static GameViewportLayout CalculateLayout(
        Vector2 available,
        GamePresentationSettings presentation,
        float framebufferScale
    ) {
        int availableWidth = Math.Max(1, (int)MathF.Floor(available.X));
        int availableHeight = Math.Max(1, (int)MathF.Floor(available.Y));
        GamePresentationViewport viewport = presentation.CalculateViewport(availableWidth, availableHeight);
        return new GameViewportLayout(
            new Vector2(viewport.x, viewport.y),
            new Vector2(viewport.width, viewport.height),
            Math.Max(1, (int)MathF.Ceiling(viewport.width * framebufferScale)),
            Math.Max(1, (int)MathF.Ceiling(viewport.height * framebufferScale))
        );
    }

    private readonly record struct GameViewportLayout(
        Vector2 offset,
        Vector2 size,
        int pixelWidth,
        int pixelHeight
    );
}
