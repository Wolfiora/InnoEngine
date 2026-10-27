using UiDrawing = Inno.Editor.ImGui.ImGui;
using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using EditorImGui = Inno.Editor.ImGui.ImGui;
using Inno.References;
using Inno.Core.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Inno.Editor.Core;
using Inno.Editor.ImGui;
using Inno.Editor.ImGui.ImGuiWidget;
using Inno.Editor.Interactions;
using Inno.Editor.PlayMode;
using Inno.Editor.Rendering;
using Inno.Editor.Scene;
using Inno.Editor.Settings;
using Inno.Scene;
using Inno.Scene.Components;
using ImGuizmoMode = Inno.Editor.ImGui.ImGuiWidget.TransformGizmoSpace;
using ImGuizmoOperation = Inno.Editor.ImGui.ImGuiWidget.TransformGizmoOperation;
using Inno.Rendering;
using Ui = Inno.Editor.ImGui.ImGui;
using EngineMatrix = Inno.Core.Mathematics.Matrix;
using EngineQuaternion = Inno.Core.Mathematics.Quaternion;
using EngineVector3 = Inno.Core.Mathematics.Vector3;

namespace Inno.Editor.Panel.SceneView;
/// <summary>
/// Presents the active Plugin provider and host-owned transform manipulation for the Scene viewport.
/// </summary>
[EditorPanel("rendering.scene-view", "Scene", order: 210, menuPath: "Viewports")]
internal sealed class SceneViewPanel : EditorPanel
{
    private IDisposable? m_settingsSubscription;
    private const string C_VIEWPORT_ID = "scene-view";
    private const string C_GIZMO_CLUSTER_POPUP = "##scene-gizmo-cluster";
    private const int C_MANIPULATION_TOOL_COUNT = 4;
    private const string C_TRANSFORM_EDIT = "scene-view.transform";
    private static readonly EditorViewportKindId S_KIND = new("inno.editor.viewport.scene");
    private static readonly Vector2 S_UNAVAILABLE_PADDING = new(48f, 32f);
    private readonly EditorRenderingModule m_rendering;
    private readonly EditorInteractions m_interactions;
    private readonly SceneEdits m_sceneEdits;
    private readonly IEditorGameScenePresentation m_scenePresentation;
    private readonly EditorPlayModeLoop m_playModeLoop;
    private readonly EditorSettings m_settings;
    private Vector4 m_backgroundColor;
    private NavigationDrag m_navigationDrag;
    private readonly EditorPlanarNavigation m_planarNavigation = new();
    private ImGuiMouseButton m_navigationDragButton;
    private ImGuizmoOperation m_operation = ImGuizmoOperation.Translate;
    private ImGuizmoMode m_mode = ImGuizmoMode.World;
    private RuntimeIdentity? m_gestureTarget;
    private RuntimeIdentity[] m_gestureTargets = [];
    private Identity[] m_popupGizmoOwners = [];
    internal SceneViewPanel(
        EditorRenderingModule rendering,
        EditorInteractions interactions,
        SceneEdits sceneEdits,
        IEditorGameScenePresentation scenePresentation,
        EditorPlayModeLoop playModeLoop,
        EditorSettings settings
    ) {
        m_rendering = rendering ?? throw new ArgumentNullException(nameof(rendering));
        m_interactions = interactions ?? throw new ArgumentNullException(nameof(interactions));
        m_sceneEdits = sceneEdits ?? throw new ArgumentNullException(nameof(sceneEdits));
        m_scenePresentation = scenePresentation ?? throw new ArgumentNullException(nameof(scenePresentation));
        m_playModeLoop = playModeLoop ?? throw new ArgumentNullException(nameof(playModeLoop));
        m_settings = settings ?? throw new ArgumentNullException(nameof(settings));
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
        if (m_gestureTarget is not null && !Ui.IsMouseButtonDown(ImGuiMouseButton.Left))
            CommitGesture();
        Vector2 available = Ui.GetContentRegionAvailable();
        if (available.X <= 0f || available.Y <= 0f)
            return;
        int width = Math.Max(1, (int)MathF.Floor(available.X));
        int height = Math.Max(1, (int)MathF.Floor(available.Y));
        m_rendering.SetPresentation(C_VIEWPORT_ID, new EditorViewportPresentation(ToEngineColor(m_backgroundColor)));
        m_rendering.SetContentScope(C_VIEWPORT_ID, CreateContentScope());
        Vector2 minimum = Ui.GetCursorScreenPosition();
        Vector2 maximum = minimum + new Vector2(width, height);
        ManipulationToolbarLayout toolbar = CreateManipulationToolbarLayout(minimum, maximum);
        bool hovered = Ui.IsWindowHovered(ImGuiHoveredFlags.RootAndChildWindows) && Ui.IsMouseHoveringRect(minimum, maximum);
        bool toolbarHovered = IsManipulationToolbarHovered(toolbar);
        _ = m_rendering.TryConfigureNavigation(S_KIND, C_VIEWPORT_ID, width, height, out EditorViewportNavigationProfile navigationProfile);
        bool navigationOwnsPointer = HandleNavigation(navigationProfile, hovered && !toolbarHovered, minimum, maximum);
        if (!m_rendering.TrySubmit(S_KIND, C_VIEWPORT_ID, width, height, out EditorViewportOutput output))
        {
            DrawUnavailable(available, m_rendering.GetCompositionError(C_VIEWPORT_ID) ?? "No rendering model contributes to Scene View.");
            _ = DrawManipulationToolbar(toolbar);
            return;
        }

        if (!output.isReady)
        {
            DrawUnavailable(available, "Preparing Scene View GPU target...");
            _ = DrawManipulationToolbar(toolbar);
            return;
        }

        m_rendering.Draw(output, new Vector2(width, height));
        minimum = Ui.GetItemMinimum();
        maximum = Ui.GetItemMaximum();
        bool viewportClicked = Ui.IsItemClicked(ImGuiMouseButton.Left);
        toolbar = CreateManipulationToolbarLayout(minimum, maximum);
        toolbarHovered = IsManipulationToolbarHovered(toolbar);
        EditorDrawList drawList = EditorImGui.GetDrawList();
        IReadOnlyList<(Vector2 center, EditorGizmoIcon icon)> icons;
        bool toolbarOwnsPointer;
        bool gizmoOwnsPointer;
        drawList.SplitChannels(3);
        try
        {
            drawList.SetChannel(0);
            icons = DrawGizmos(minimum, maximum);
            drawList.SetChannel(2);
            toolbarOwnsPointer = DrawManipulationToolbar(toolbar);
            drawList.SetChannel(1);
            gizmoOwnsPointer = !navigationOwnsPointer && DrawTransformGizmo(minimum, maximum, hovered && !toolbarHovered);
        }
        finally
        {
            drawList.MergeChannels();
        }

        bool iconOwnsPointer = SelectGizmoIcons(icons, viewportClicked && !toolbarOwnsPointer && !gizmoOwnsPointer && !navigationOwnsPointer);
        if (!viewportClicked || toolbarOwnsPointer || gizmoOwnsPointer || iconOwnsPointer || navigationOwnsPointer || UiDrawing.GetInput().alt)
            return;
        Vector2 mouse = Ui.GetMousePos();
        float x = (mouse.X - minimum.X) / Math.Max(1f, maximum.X - minimum.X);
        float y = (mouse.Y - minimum.Y) / Math.Max(1f, maximum.Y - minimum.Y);
        m_rendering.HandlePointer(S_KIND, C_VIEWPORT_ID, width, height, x, y, button: 0);
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
        ApplySettings(m_settings);
        m_settingsSubscription = m_settings.ListenChanges(ApplySettings);
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
        m_planarNavigation.Cancel();
        m_navigationDrag = NavigationDrag.None;
        CommitGesture();
        m_rendering.Release(C_VIEWPORT_ID);
    }

    /// <summary>
    /// Captures an immutable snapshot of the current observable state.
    /// </summary>
    /// <param name="state">
    /// The lifecycle or domain state applied by this operation.
    /// </param>
    protected override void Capture(EditorState state)
    {
        EditorViewportNavigationState navigation = m_rendering.GetNavigationState(C_VIEWPORT_ID);
        state.Set("navigation.initialized", navigation.isInitialized);
        if (!navigation.isInitialized)
            return;
        state.Set("navigation.position.x", navigation.position.x);
        state.Set("navigation.position.y", navigation.position.y);
        state.Set("navigation.position.z", navigation.position.z);
        state.Set("navigation.rotation.x", navigation.rotation.x);
        state.Set("navigation.rotation.y", navigation.rotation.y);
        state.Set("navigation.rotation.z", navigation.rotation.z);
        state.Set("navigation.rotation.w", navigation.rotation.w);
        state.Set("navigation.pivot.x", navigation.pivot.x);
        state.Set("navigation.pivot.y", navigation.pivot.y);
        state.Set("navigation.pivot.z", navigation.pivot.z);
        state.Set("navigation.projection", (int)navigation.projection);
        state.Set("navigation.mode", (int)navigation.mode);
        state.Set("navigation.orthographicSize", navigation.orthographicSize);
        state.Set("navigation.fieldOfView", navigation.fieldOfView);
        state.Set("navigation.nearClip", navigation.nearClip);
        state.Set("navigation.farClip", navigation.farClip);
        state.Set("navigation.focusDistance", navigation.focusDistance);
        state.Set("navigation.movementSpeed", navigation.movementSpeed);
    }

    /// <summary>
    /// Restores the supplied snapshot while preserving current invariants.
    /// </summary>
    /// <param name="state">
    /// The lifecycle or domain state applied by this operation.
    /// </param>
    protected override void Restore(EditorState state)
    {
        if (!state.Get("navigation.initialized", false))
            return;
        var position = new EngineVector3(state.Get("navigation.position.x", 0f), state.Get("navigation.position.y", 0f), state.Get("navigation.position.z", 0f));
        var rotation = new EngineQuaternion(
            state.Get("navigation.rotation.x", 0f),
            state.Get("navigation.rotation.y", 0f),
            state.Get("navigation.rotation.z", 0f),
            state.Get("navigation.rotation.w", 1f)
        );
        var pivot = new EngineVector3(state.Get("navigation.pivot.x", 0f), state.Get("navigation.pivot.y", 0f), state.Get("navigation.pivot.z", 0f));
        EditorViewportProjection projection = (EditorViewportProjection)state.Get("navigation.projection", (int)EditorViewportProjection.Orthographic);
        try
        {
            EditorViewportNavigationState navigation = m_rendering.GetNavigationState(C_VIEWPORT_ID);
            if (projection == EditorViewportProjection.Perspective)
            {
                navigation.ConfigurePerspective(
                    position,
                    rotation,
                    state.Get("navigation.fieldOfView", 60f),
                    state.Get("navigation.nearClip", 0.01f),
                    state.Get("navigation.farClip", 1000f)
                );
            }
            else
            {
                navigation.ConfigureOrthographic(position, rotation, state.Get("navigation.orthographicSize", 5f));
            }

            navigation.pivot = pivot;
            navigation.focusDistance = state.Get("navigation.focusDistance", 10f);
            navigation.movementSpeed = state.Get("navigation.movementSpeed", 5f);
            navigation.mode = (EditorViewportNavigationMode)state.Get("navigation.mode", (int)EditorViewportNavigationMode.Planar);
        }
        catch (ArgumentException)
        {
        // Malformed workspace camera state is ignored and the active provider supplies a fresh default.
        }
    }

    private void ApplySettings(EditorSettings settings) => m_backgroundColor = SceneViewBackgroundSetting.Read(settings);
    private ContentReadScope CreateContentScope() => m_scenePresentation.Capture();
    private IReadOnlyList<(Vector2 center, EditorGizmoIcon icon)> DrawGizmos(
        Vector2 minimum,
        Vector2 maximum
    ) {
        if (!m_rendering.TryGetManipulationSpace(C_VIEWPORT_ID, out EditorViewportManipulationSpace space))
            return[];
        int width = Math.Max(1, (int)(maximum.X - minimum.X));
        int height = Math.Max(1, (int)(maximum.Y - minimum.Y));
        EditorGizmoFrame frame = m_rendering.CollectGizmos(C_VIEWPORT_ID, width, height);
        EngineMatrix worldToClip = space.projectionMatrix * space.viewMatrix;
        EditorDrawList draw = EditorImGui.GetDrawList();
        uint white = Ui.ToPackedColor(Vector4.One);
        uint outline = Ui.ToPackedColor(new Vector4(0f, 0f, 0f, 0.75f));
        foreach (EditorGizmoLine line in frame.lines)
        {
            if (!TryProject(line.start, out Vector2 start) || !TryProject(line.end, out Vector2 end))
                continue;
            EditorImGui.DrawLine(start + Vector2.One, end + Vector2.One, outline, 3.5f);
            EditorImGui.DrawLine(start, end, white, 2.5f);
        }

        float iconSize = 24f * EditorUI.current.widgets.style.zoom;
        var displayIcons = new List<(Vector2 center, EditorGizmoIcon icon)>();
        foreach (EditorGizmoIcon icon in frame.icons)
        {
            if (!TryProject(icon.position, out Vector2 center))
                continue;
            displayIcons.Add((center, icon));
            string glyph = GizmoIconGlyph(icon.iconId);
            float fontScale = iconSize / Ui.GetFontSize();
            Vector2 size = EditorImGui.MeasureText(glyph, fontScale);
            Vector2 origin = center - size * 0.5f;
            draw.Text(origin + Vector2.One, outline, glyph, fontScale);
            draw.Text(origin, white, glyph, fontScale);
        }

        return displayIcons;
        bool TryProject(
            EngineVector3 world,
            out Vector2 screen
        ) {
            Inno.Core.Mathematics.Vector4 clip = Inno.Core.Mathematics.Vector4.Transform(new Inno.Core.Mathematics.Vector4(world.x, world.y, world.z, 1f), worldToClip);
            if (clip.w <= 0.000001f)
            {
                screen = default;
                return false;
            }

            screen = minimum + new Vector2((clip.x / clip.w + 1f) * width * 0.5f, (1f - clip.y / clip.w) * height * 0.5f);
            return float.IsFinite(screen.X) && float.IsFinite(screen.Y);
        }
    }

    private bool SelectGizmoIcons(
        IReadOnlyList<(Vector2 center, EditorGizmoIcon icon)> displayIcons,
        bool maySelect
    ) {
        bool iconClicked = false;
        if (maySelect)
        {
            Vector2 mouse = Ui.GetMousePos();
            float radius = 20f * EditorUI.current.widgets.style.zoom;
            Identity[] owners = displayIcons.Where(icon => Vector2.DistanceSquared(mouse, icon.center) <= radius * radius).Select(static icon => icon.icon.owner).DistinctBy(static owner => owner.persistentId).ToArray();
            if (owners.Length == 1 && owners[0].Resolve<GameObject>() is { isDestroyed: false } owner)
            {
                m_interactions.SetSelection(owner);
                iconClicked = true;
            }
            else if (owners.Length > 1)
            {
                m_popupGizmoOwners = owners;
                Ui.OpenPopup(C_GIZMO_CLUSTER_POPUP);
                iconClicked = true;
            }
        }

        DrawGizmoClusterPopup();
        return iconClicked;
    }

    private void DrawGizmoClusterPopup()
    {
        if (!Ui.BeginPopup(C_GIZMO_CLUSTER_POPUP))
            return;
        foreach (Identity identity in m_popupGizmoOwners)
        {
            GameObject? owner = identity.Resolve<GameObject>();
            if (owner is null || owner.isDestroyed)
                continue;
            string label = $"{owner.name}##{identity.runtimeIdentity}";
            if (!Ui.MenuItem(label, string.Empty, false, true))
                continue;
            m_interactions.SetSelection(owner);
            Ui.CloseCurrentPopup();
            break;
        }

        Ui.EndPopup();
    }

    private static string GizmoIconGlyph(string iconId) => iconId switch
    {
        "camera" => ImGuiIcon.Camera,
        "light" => ImGuiIcon.Lightbulb,
        "canvas" => ImGuiIcon.VectorSquare,
        _ => ImGuiIcon.CircleQuestion
    };
    private bool HandleNavigation(
        EditorViewportNavigationProfile profile,
        bool hovered,
        Vector2 minimum,
        Vector2 maximum
    ) {
        EditorViewportNavigationState navigation = m_rendering.GetNavigationState(C_VIEWPORT_ID);
        if (!navigation.isInitialized || profile.capabilities == EditorViewportNavigationCapabilities.None)
        {
            m_navigationDrag = NavigationDrag.None;
            m_planarNavigation.Cancel();
            return false;
        }

        if (!SupportsMode(profile, navigation.mode))
            navigation.mode = profile.defaultMode;
        EditorUiInput io = UiDrawing.GetInput();
        if (m_navigationDrag != NavigationDrag.None && !Ui.IsMouseButtonDown(m_navigationDragButton))
        {
            m_navigationDrag = NavigationDrag.None;
        }

        bool panning = m_planarNavigation.Update(
            hovered && m_navigationDrag == NavigationDrag.None && profile.capabilities.HasFlag(EditorViewportNavigationCapabilities.Pan),
            Ui.IsMouseClicked(ImGuiMouseButton.Left),
            Ui.IsMouseClicked(ImGuiMouseButton.Middle),
            Ui.IsMouseButtonDown(ImGuiMouseButton.Left),
            Ui.IsMouseButtonDown(ImGuiMouseButton.Middle),
            io.alt,
            allowAltPrimary: !profile.capabilities.HasFlag(EditorViewportNavigationCapabilities.Orbit)
        );
        if (hovered && !panning && m_navigationDrag == NavigationDrag.None)
        {
            if (io.alt && Ui.IsMouseClicked(ImGuiMouseButton.Left))
            {
                if (profile.capabilities.HasFlag(EditorViewportNavigationCapabilities.Orbit))
                {
                    m_navigationDrag = NavigationDrag.Orbit;
                    m_navigationDragButton = ImGuiMouseButton.Left;
                }
            }
            else if (Ui.IsMouseClicked(ImGuiMouseButton.Right) && profile.capabilities.HasFlag(EditorViewportNavigationCapabilities.Fly))
            {
                m_navigationDrag = NavigationDrag.Fly;
                m_navigationDragButton = ImGuiMouseButton.Right;
            }
        }

        if (!io.wantsText && Ui.IsKeyPressed(ImGuiKey.Escape, repeat: false))
        {
            m_planarNavigation.Cancel();
            m_navigationDrag = NavigationDrag.None;
            panning = false;
        }

        bool ownsPointer = panning || m_navigationDrag != NavigationDrag.None;
        if (!hovered && !ownsPointer)
            return false;
        float width = MathF.Max(1f, maximum.X - minimum.X);
        float height = MathF.Max(1f, maximum.Y - minimum.Y);
        float aspect = width / height;
        if (panning)
        {
            Pan(navigation, new Vector2(io.mouseDelta.X, io.mouseDelta.Y), height);
            Ui.SetMouseCursor(ImGuiMouseCursor.ResizeAll);
        }

        switch (m_navigationDrag)
        {
            case NavigationDrag.Orbit:
                Orbit(navigation, profile, new Vector2(io.mouseDelta.X, io.mouseDelta.Y));
                break;
            case NavigationDrag.Fly:
                Fly(navigation, profile, io);
                break;
        }

        if (hovered && profile.capabilities.HasFlag(EditorViewportNavigationCapabilities.Zoom) && io.wheel != 0f)
            Zoom(navigation, profile, io.wheel, minimum, width, height, aspect);
        if (hovered && profile.capabilities.HasFlag(EditorViewportNavigationCapabilities.FrameSelection) && Ui.IsKeyPressed(ImGuiKey.F, repeat: false))
        {
            EditorViewportFocusBounds? bounds = profile.focusBounds;
            if (bounds is null && TryGetSelectedTransform(out Transform? selected))
                bounds = new EditorViewportFocusBounds(selected!.worldPosition, 0.5f);
            if (bounds is EditorViewportFocusBounds focus)
                Frame(navigation, profile, focus);
        }

        return ownsPointer;
    }

    private static void Pan(
        EditorViewportNavigationState navigation,
        Vector2 mouseDelta,
        float height
    ) {
        float verticalSpan = navigation.projection == EditorViewportProjection.Orthographic ? navigation.orthographicSize * 2f : 2f * navigation.focusDistance * MathF.Tan(navigation.fieldOfView * MathF.PI / 360f);
        float unitsPerPixel = verticalSpan / height;
        EngineVector3 right = EngineVector3.Transform(EngineVector3.RIGHT, navigation.rotation);
        EngineVector3 up = EngineVector3.Transform(EngineVector3.UP, navigation.rotation);
        EngineVector3 delta = right * (-mouseDelta.X * unitsPerPixel) + up * (mouseDelta.Y * unitsPerPixel);
        navigation.position += delta;
        navigation.pivot += delta;
    }

    private static void Orbit(
        EditorViewportNavigationState navigation,
        EditorViewportNavigationProfile profile,
        Vector2 mouseDelta
    ) {
        EngineVector3 worldUp = GetWorldUp(profile);
        float sensitivity = GetPositive(profile.rotationSensitivity, 0.005f);
        EngineQuaternion yaw = EngineQuaternion.CreateFromAxisAngle(worldUp, -mouseDelta.X * sensitivity);
        EngineQuaternion yawed = (yaw * navigation.rotation).normalized;
        EngineVector3 right = EngineVector3.Transform(EngineVector3.RIGHT, yawed).normalized;
        navigation.rotation = ApplyConstrainedPitch(yawed, right, worldUp, -mouseDelta.Y * sensitivity);
        navigation.mode = EditorViewportNavigationMode.Orbit;
        EngineVector3 forward = EngineVector3.Transform(EngineVector3.FORWARD, navigation.rotation);
        navigation.position = navigation.pivot - forward * navigation.focusDistance;
    }

    private static void Fly(
        EditorViewportNavigationState navigation,
        EditorViewportNavigationProfile profile,
        EditorUiInput io
    ) {
        float sensitivity = GetPositive(profile.rotationSensitivity, 0.005f);
        EngineVector3 worldUp = GetWorldUp(profile);
        EngineQuaternion yaw = EngineQuaternion.CreateFromAxisAngle(worldUp, -io.mouseDelta.X * sensitivity);
        EngineQuaternion yawed = (yaw * navigation.rotation).normalized;
        EngineVector3 right = EngineVector3.Transform(EngineVector3.RIGHT, yawed).normalized;
        navigation.rotation = ApplyConstrainedPitch(yawed, right, worldUp, -io.mouseDelta.Y * sensitivity);
        navigation.mode = EditorViewportNavigationMode.Fly;
        EngineVector3 movement = EngineVector3.ZERO;
        EngineVector3 forward = EngineVector3.Transform(EngineVector3.FORWARD, navigation.rotation);
        right = EngineVector3.Transform(EngineVector3.RIGHT, navigation.rotation);
        if (Ui.IsKeyDown(ImGuiKey.W))
            movement += forward;
        if (Ui.IsKeyDown(ImGuiKey.S))
            movement -= forward;
        if (Ui.IsKeyDown(ImGuiKey.D))
            movement += right;
        if (Ui.IsKeyDown(ImGuiKey.A))
            movement -= right;
        if (Ui.IsKeyDown(ImGuiKey.E))
            movement += worldUp;
        if (Ui.IsKeyDown(ImGuiKey.Q))
            movement -= worldUp;
        if (movement.LengthSquared() > 0.000001f)
        {
            float multiplier = io.shift ? GetPositive(profile.fastMovementMultiplier, 4f) : 1f;
            navigation.position += movement.normalized * navigation.movementSpeed * multiplier * MathF.Max(0f, io.deltaTime);
            navigation.pivot = navigation.position + forward * navigation.focusDistance;
        }
    }

    private static void Zoom(
        EditorViewportNavigationState navigation,
        EditorViewportNavigationProfile profile,
        float wheel,
        Vector2 minimum,
        float width,
        float height,
        float aspect
    ) {
        float zoomSensitivity = GetPositive(profile.zoomSensitivity, 0.16f);
        if (navigation.projection == EditorViewportProjection.Orthographic)
        {
            Vector2 mouse = Ui.GetMousePos();
            float normalizedX = Math.Clamp((mouse.X - minimum.X) / width, 0f, 1f);
            float normalizedY = Math.Clamp((mouse.Y - minimum.Y) / height, 0f, 1f);
            float previousSize = navigation.orthographicSize;
            EngineVector3 previousOffset = GetViewportOffset(normalizedX, normalizedY, previousSize, aspect, navigation.rotation);
            float minimumSize = GetPositive(profile.minimumOrthographicSize, 0.001f);
            float maximumSize = MathF.Max(minimumSize, GetPositive(profile.maximumOrthographicSize, 100000f));
            float nextSize = Math.Clamp(previousSize / EditorPlanarNavigation.WheelFactor(wheel, zoomSensitivity), minimumSize, maximumSize);
            EngineVector3 nextOffset = GetViewportOffset(normalizedX, normalizedY, nextSize, aspect, navigation.rotation);
            EngineVector3 delta = previousOffset - nextOffset;
            navigation.position += delta;
            navigation.pivot += delta;
            navigation.orthographicSize = nextSize;
            return;
        }

        float minimumDistance = GetPositive(profile.minimumFocusDistance, 0.01f);
        float maximumDistance = MathF.Max(minimumDistance, GetPositive(profile.maximumFocusDistance, 1000000f));
        navigation.focusDistance = Math.Clamp(navigation.focusDistance / EditorPlanarNavigation.WheelFactor(wheel, zoomSensitivity), minimumDistance, maximumDistance);
        EngineVector3 direction = EngineVector3.Transform(EngineVector3.FORWARD, navigation.rotation);
        navigation.position = navigation.pivot - direction * navigation.focusDistance;
    }

    private static void Frame(
        EditorViewportNavigationState navigation,
        EditorViewportNavigationProfile profile,
        EditorViewportFocusBounds focus
    ) {
        float padding = GetPositive(profile.framePadding, 1.25f);
        float radius = MathF.Max(focus.radius, 0.01f);
        navigation.pivot = focus.center;
        EngineVector3 forward = EngineVector3.Transform(EngineVector3.FORWARD, navigation.rotation);
        if (navigation.projection == EditorViewportProjection.Orthographic)
        {
            float minimumSize = GetPositive(profile.minimumOrthographicSize, 0.001f);
            float maximumSize = MathF.Max(minimumSize, GetPositive(profile.maximumOrthographicSize, 100000f));
            navigation.orthographicSize = Math.Clamp(radius * padding, minimumSize, maximumSize);
            navigation.focusDistance = MathF.Max(navigation.focusDistance, radius * 2f);
        }
        else
        {
            float halfFov = navigation.fieldOfView * MathF.PI / 360f;
            float distance = radius * padding / MathF.Max(0.001f, MathF.Tan(halfFov));
            navigation.focusDistance = Math.Clamp(distance, GetPositive(profile.minimumFocusDistance, 0.01f), GetPositive(profile.maximumFocusDistance, 1000000f));
        }

        navigation.position = focus.center - forward * navigation.focusDistance;
    }

    private static bool SupportsMode(
        EditorViewportNavigationProfile profile,
        EditorViewportNavigationMode mode
    ) => mode switch
    {
        EditorViewportNavigationMode.Orbit => profile.capabilities.HasFlag(EditorViewportNavigationCapabilities.Orbit),
        EditorViewportNavigationMode.Fly => profile.capabilities.HasFlag(EditorViewportNavigationCapabilities.Fly),
        _ => (profile.capabilities & EditorViewportNavigationCapabilities.Planar) != 0
    };
    private static EngineQuaternion ApplyConstrainedPitch(
        EngineQuaternion yawed,
        EngineVector3 right,
        EngineVector3 worldUp,
        float angle
    ) {
        EngineQuaternion pitch = EngineQuaternion.CreateFromAxisAngle(right, angle);
        EngineQuaternion candidate = (pitch * yawed).normalized;
        EngineVector3 forward = EngineVector3.Transform(EngineVector3.FORWARD, candidate).normalized;
        return MathF.Abs(EngineVector3.Dot(forward, worldUp)) < 0.999f ? candidate : yawed;
    }

    private static EngineVector3 GetWorldUp(EditorViewportNavigationProfile profile) => profile.worldUp.LengthSquared() > 0.000001f ? profile.worldUp.normalized : EngineVector3.UP;
    private static float GetPositive(
        float value,
        float fallback
    ) => float.IsFinite(value) && value > 0f ? value : fallback;
    private static EngineVector3 GetViewportOffset(
        float normalizedX,
        float normalizedY,
        float halfHeight,
        float aspect,
        EngineQuaternion rotation
    ) {
        var local = new EngineVector3((normalizedX * 2f - 1f) * halfHeight * aspect, (1f - normalizedY * 2f) * halfHeight, 0f);
        return EngineVector3.Transform(local, rotation);
    }

    private void DrawUnavailable(
        Vector2 size,
        string message
    ) {
        Vector2 minimum = Ui.GetCursorScreenPosition();
        Vector2 maximum = minimum + size;
        EditorDrawList drawList = EditorImGui.GetDrawList();
        EditorImGui.DrawFilledRectangle(minimum, maximum, Ui.ToPackedColor(m_backgroundColor), 0);
        Ui.PushStyleColor(ImGuiCol.Text, EditorPalette.textDisabled);
        try
        {
            EditorUI.current.widgets.CenteredWrappedText(message, size, S_UNAVAILABLE_PADDING);
        }
        finally
        {
            Ui.PopStyleColor();
        }
    }

    private static Inno.Core.Mathematics.Color ToEngineColor(Vector4 value) => new(value.X, value.Y, value.Z, value.W);
    private bool DrawManipulationToolbar(ManipulationToolbarLayout layout)
    {
        EditorDrawList drawList = EditorImGui.GetDrawList();
        Vector4 background = EditorPalette.targetHeader;
        background.W = EditorPalette.opacityMedium;
        EditorImGui.DrawFilledRectangle(layout.minimum, layout.maximum, Ui.ToPackedColor(background), EditorUI.current.widgets.style.frameRounding);
        float itemX = layout.minimum.X + layout.padding;
        float itemY = layout.minimum.Y + layout.padding;
        try
        {
            if (DrawManipulationTool("move", ImGuiIcon.UpDownLeftRight, "Move", new Vector2(itemX, itemY), layout.itemSize, m_operation == ImGuizmoOperation.Translate))
            {
                m_operation = ImGuizmoOperation.Translate;
            }

            itemY += layout.itemSize.Y + layout.spacing;
            if (DrawManipulationTool("rotate", ImGuiIcon.Rotate, "Rotate", new Vector2(itemX, itemY), layout.itemSize, m_operation == ImGuizmoOperation.Rotate))
            {
                m_operation = ImGuizmoOperation.Rotate;
            }

            itemY += layout.itemSize.Y + layout.spacing;
            if (DrawManipulationTool("scale", ImGuiIcon.UpRightAndDownLeftFromCenter, "Scale", new Vector2(itemX, itemY), layout.itemSize, m_operation == ImGuizmoOperation.Scale))
            {
                m_operation = ImGuizmoOperation.Scale;
            }

            itemY += layout.itemSize.Y + layout.sectionSpacing;
            float separatorY = itemY - layout.sectionSpacing * 0.5f;
            EditorImGui.DrawLine(
                new Vector2(layout.minimum.X + layout.padding, separatorY),
                new Vector2(layout.maximum.X - layout.padding, separatorY),
                Ui.ToPackedColor(Vector4.One),
                EditorUI.current.widgets.style.borderSize
            );
            bool supportsCoordinateSpace = m_operation != ImGuizmoOperation.Scale;
            string modeIcon = supportsCoordinateSpace && m_mode == ImGuizmoMode.World ? ImGuiIcon.Globe : ImGuiIcon.Cube;
            string modeTooltip = !supportsCoordinateSpace ? "Coordinate Space: Local (Scale is always local)" : m_mode == ImGuizmoMode.Local ? "Coordinate Space: Local (axes follow the selected transform)" : "Coordinate Space: World (axes follow the world)";
            if (!supportsCoordinateSpace)
                Ui.BeginDisabled(true);
            bool toggleCoordinateSpace;
            try
            {
                toggleCoordinateSpace = DrawManipulationTool("coordinate_space", modeIcon, modeTooltip, new Vector2(itemX, itemY), layout.itemSize, selected: false);
            }
            finally
            {
                if (!supportsCoordinateSpace)
                    Ui.EndDisabled();
            }

            if (!supportsCoordinateSpace && Ui.IsItemHovered(ImGuiHoveredFlags.AllowWhenDisabled) && EditorUI.current.widgets.BeginMenuTooltip())
            {
                Ui.Text(modeTooltip);
                EditorUI.current.widgets.EndMenuTooltip();
            }

            if (toggleCoordinateSpace && supportsCoordinateSpace)
            {
                m_mode = m_mode == ImGuizmoMode.Local ? ImGuizmoMode.World : ImGuizmoMode.Local;
            }
        }
        finally
        {
            Ui.SetCursorScreenPosition(layout.viewportMaximum);
            Ui.Dummy(Vector2.Zero);
        }

        return IsManipulationToolbarHovered(layout);
    }

    private static bool DrawManipulationTool(
        string id,
        string icon,
        string tooltip,
        Vector2 position,
        Vector2 size,
        bool selected
    ) {
        Ui.SetCursorScreenPosition(position);
        if (selected)
            Ui.PushStyleColor(ImGuiCol.Text, Vector4.One);
        bool pressed;
        try
        {
            pressed = EditorUI.current.widgets.ClickableText($"scene_transform_{id}", icon, size, tooltip);
        }
        finally
        {
            if (selected)
                Ui.PopStyleColor();
        }

        if (selected)
        {
            Vector2 minimum = Ui.GetItemMinimum();
            Vector2 maximum = Ui.GetItemMaximum();
            float thickness = EditorUI.current.widgets.style.interactionOverlayThickness;
            EditorImGui.DrawFilledRectangle(minimum, new Vector2(minimum.X + thickness, maximum.Y), Ui.ToPackedColor(Vector4.One), 0);
        }

        return pressed;
    }

    private static ManipulationToolbarLayout CreateManipulationToolbarLayout(
        Vector2 viewportMinimum,
        Vector2 viewportMaximum
    ) {
        float zoom = EditorUI.current.widgets.style.zoom;
        float inset = 10f * zoom;
        float padding = 4f * zoom;
        float spacing = 2f * zoom;
        float sectionSpacing = 8f * zoom;
        float itemExtent = MathF.Max(Ui.GetFrameHeight(), 26f * zoom);
        Vector2 itemSize = new(itemExtent);
        float width = itemExtent + padding * 2f;
        float height = padding * 2f + itemExtent * C_MANIPULATION_TOOL_COUNT + spacing * 2f + sectionSpacing;
        Vector2 minimum = viewportMinimum + new Vector2(inset);
        Vector2 maximum = minimum + new Vector2(width, height);
        if (maximum.X > viewportMaximum.X - inset)
        {
            float offset = maximum.X - (viewportMaximum.X - inset);
            minimum.X -= offset;
            maximum.X -= offset;
        }

        if (maximum.Y > viewportMaximum.Y - inset)
        {
            float offset = maximum.Y - (viewportMaximum.Y - inset);
            minimum.Y -= offset;
            maximum.Y -= offset;
        }

        return new ManipulationToolbarLayout(minimum, maximum, viewportMaximum, itemSize, padding, spacing, sectionSpacing);
    }

    private static bool IsManipulationToolbarHovered(ManipulationToolbarLayout layout) => Ui.IsWindowHovered(ImGuiHoveredFlags.RootAndChildWindows)
        && Ui.IsMouseHoveringRect(
        layout.minimum,
        layout.maximum
    );
    private bool DrawTransformGizmo(
        Vector2 minimum,
        Vector2 maximum,
        bool canInteract
    ) {
        if (!m_rendering.TryGetManipulationSpace(C_VIEWPORT_ID, out EditorViewportManipulationSpace manipulationSpace) || !TryGetSelectedTransform(out Transform? selected))
        {
            return false;
        }

        if (m_gestureTarget is not null && !EditorUI.current.widgets.IsEditing(C_TRANSFORM_EDIT))
        {
            m_gestureTarget = null;
            m_gestureTargets = [];
        }

        Transform target = m_gestureTarget is RuntimeIdentity identity
            && m_interactions.TryResolveIdentity(
            identity,
            out IdentityObject? resolved
        )
            && resolved is Transform active ? active : selected!;
        EngineMatrix world = target.localToWorldMatrix;
        ImGuizmoOperation operation = SelectManipulationOperation(manipulationSpace.plane, m_operation);
        bool planarTranslation = manipulationSpace.plane != EditorViewportManipulationPlane.Spatial && m_operation == ImGuizmoOperation.Translate;
        TransformGizmoResult result = EditorUI.current.widgets.TransformGizmo(
            world,
            new TransformGizmoOptions {
                minimum = minimum,
                maximum = maximum,
                view = manipulationSpace.viewMatrix,
                projection = manipulationSpace.projectionMatrix,
                orthographic = manipulationSpace.isOrthographic,
                enabled = canInteract || m_gestureTarget is not null,
                operation = operation,
                space = m_operation == ImGuizmoOperation.Scale ? ImGuizmoMode.Local : m_mode,
                maskX = planarTranslation && manipulationSpace.plane == EditorViewportManipulationPlane.YZ,
                maskY = planarTranslation && manipulationSpace.plane == EditorViewportManipulationPlane.XZ,
                maskZ = planarTranslation && manipulationSpace.plane == EditorViewportManipulationPlane.XY
            }
        );
        if (Ui.IsKeyPressed(ImGuiKey.Escape, false) && m_gestureTarget is not null)
        {
            EditorUI.current.widgets.CancelEdits();
            m_gestureTarget = null;
            m_gestureTargets = [];
            return true;
        }

        if (result.isUsing)
        {
            if (m_gestureTarget is null)
            {
                m_gestureTarget = target.identity.runtimeIdentity;
                m_gestureTargets = SelectedTransforms().Select(value => value.identity.runtimeIdentity!.Value).ToArray();
            }

            EditorUI.current.widgets.BeginEdit(m_interactions.history, C_TRANSFORM_EDIT, "Transform Selection");
        }

        if (result.changed && m_gestureTargets.Length != 0)
        {
            try
            {
                EngineMatrix delta = result.matrix * EngineMatrix.Invert(world);
                foreach (RuntimeIdentity current in m_gestureTargets)
                {
                    if (!m_interactions.TryResolveIdentity(current, out IdentityObject? value) || value is not Transform transform)
                        throw new InvalidOperationException("A transform gesture target is no longer available.");
                    EngineMatrix next = delta * transform.localToWorldMatrix;
                    if (!TryReadManipulatedTransform(next, transform, m_operation, out var position, out var rotation, out var scale) || !IsUsable(position, rotation, scale))
                        throw new InvalidOperationException("The manipulated transform is not invertible or finite.");
                    _ = m_sceneEdits.ChangeProperty(
                        transform,
                        nameof(Transform.localPosition),
                        () => transform.SetWorldTransform(position, rotation, scale),
                        "Transform Selection",
                        EditorUI.current.widgets.editMergeKey
                    );
                }
            }
            catch
            {
                EditorUI.current.widgets.CancelEdits();
                m_gestureTarget = null;
                m_gestureTargets = [];
                throw;
            }
        }

        if (!result.isUsing && m_gestureTarget is not null)
            CommitGesture();
        return result.isUsing || result.isOver;
    }

    private Transform[] SelectedTransforms()
    {
        var selected = new List<EngineObject>();
        m_interactions.selection.CopyTargets(selected);
        Transform[] transforms = selected.Select(value => value switch
        {
            GameObject gameObject => gameObject.transform,
            GameComponent component => component.transform,
            _ => null
        }).OfType<Transform>().Distinct().ToArray();
        var set = transforms.ToHashSet();
        return transforms.Where(value => !HasSelectedAncestor(value, set)).ToArray();
    }

    private static bool HasSelectedAncestor(
        Transform transform,
        ISet<Transform> selected
    ) {
        for (Transform? parent = transform.parent; parent is not null; parent = parent.parent)
            if (selected.Contains(parent))
                return true;
        return false;
    }

    private static ImGuizmoOperation SelectManipulationOperation(
        EditorViewportManipulationPlane plane,
        ImGuizmoOperation operation
    ) => operation switch
    {
        ImGuizmoOperation.Rotate => ImGuizmoOperation.Rotate,
        ImGuizmoOperation.Scale => plane switch
        {
            EditorViewportManipulationPlane.XY => ImGuizmoOperation.ScaleX | ImGuizmoOperation.ScaleY,
            EditorViewportManipulationPlane.XZ => ImGuizmoOperation.ScaleX | ImGuizmoOperation.ScaleZ,
            EditorViewportManipulationPlane.YZ => ImGuizmoOperation.ScaleY | ImGuizmoOperation.ScaleZ,
            _ => operation
        },
        _ => operation
    };
    private static bool TryReadManipulatedTransform(
        EngineMatrix matrix,
        Transform target,
        ImGuizmoOperation operation,
        out Inno.Core.Mathematics.Vector3 position,
        out EngineQuaternion rotation,
        out Inno.Core.Mathematics.Vector3 scale
    ) {
        position = new Inno.Core.Mathematics.Vector3(matrix.m14, matrix.m24, matrix.m34);
        rotation = target.worldRotation;
        scale = target.worldScale;
        if (operation == ImGuizmoOperation.Translate)
            return true;
        if (operation == ImGuizmoOperation.Scale)
        {
            EngineMatrix original = target.localToWorldMatrix;
            scale = new Inno.Core.Mathematics.Vector3(
                scale.x * SignedColumnRatio(original.m11, original.m21, original.m31, matrix.m11, matrix.m21, matrix.m31),
                scale.y * SignedColumnRatio(original.m12, original.m22, original.m32, matrix.m12, matrix.m22, matrix.m32),
                scale.z * SignedColumnRatio(original.m13, original.m23, original.m33, matrix.m13, matrix.m23, matrix.m33)
            );
            return true;
        }

        if (MathF.Abs(scale.x) <= 0.00001f || MathF.Abs(scale.y) <= 0.00001f || MathF.Abs(scale.z) <= 0.00001f)
            return false;
        EngineMatrix normalized = new(
            matrix.m11 / scale.x,
            matrix.m12 / scale.y,
            matrix.m13 / scale.z,
            0f,
            matrix.m21 / scale.x,
            matrix.m22 / scale.y,
            matrix.m23 / scale.z,
            0f,
            matrix.m31 / scale.x,
            matrix.m32 / scale.y,
            matrix.m33 / scale.z,
            0f,
            0f,
            0f,
            0f,
            1f
        );
        rotation = EngineQuaternion.FromRotationMatrix(normalized).normalized;
        return true;
    }

    private static float SignedColumnRatio(
        float originalX,
        float originalY,
        float originalZ,
        float changedX,
        float changedY,
        float changedZ
    ) {
        float squaredLength = originalX * originalX + originalY * originalY + originalZ * originalZ;
        return squaredLength <= 0.0000000001f ? 1f : (originalX * changedX + originalY * changedY + originalZ * changedZ) / squaredLength;
    }

    private bool TryGetSelectedTransform(out Transform? transform)
    {
        transform = m_interactions.selection.selectedTarget switch
        {
            GameObject gameObject when !gameObject.isDestroyed => gameObject.transform,
            Transform selectedTransform when !selectedTransform.isDestroyed => selectedTransform,
            GameComponent component when !component.isDestroyed => component.transform,
            _ => null
        };
        return transform is not null && m_sceneEdits.CanEdit(transform);
    }

    private void CommitGesture()
    {
        if (m_gestureTarget is null)
            return;
        if (EditorUI.current.widgets.IsEditing(C_TRANSFORM_EDIT))
            EditorUI.current.widgets.CommitEdits();
        m_gestureTarget = null;
        m_gestureTargets = [];
    }

    private static bool IsUsable(
        Inno.Core.Mathematics.Vector3 position,
        EngineQuaternion rotation,
        Inno.Core.Mathematics.Vector3 scale
    ) => float.IsFinite(position.x)
        && float.IsFinite(position.y)
        && float.IsFinite(position.z)
        && float.IsFinite(rotation.x)
        && float.IsFinite(rotation.y)
        && float.IsFinite(rotation.z)
        && float.IsFinite(rotation.w)
        && float.IsFinite(scale.x)
        && float.IsFinite(scale.y)
        && float.IsFinite(scale.z)
        && MathF.Abs(scale.x) > 0.00001f
        && MathF.Abs(scale.y) > 0.00001f
        && MathF.Abs(scale.z) > 0.00001f;
    private readonly record struct ManipulationToolbarLayout(
        Vector2 minimum,
        Vector2 maximum,
        Vector2 viewportMaximum,
        Vector2 itemSize,
        float padding,
        float spacing,
        float sectionSpacing
    );
    private enum NavigationDrag
    {
        None,
        Orbit,
        Fly
    }
}
