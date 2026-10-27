using System;
using System.Numerics;

namespace Inno.Editor.ImGui;

/// <summary>
/// Defines reusable layout metrics and separates owned frame values from requested style changes.
/// </summary>
/// <remarks>
/// Standalone instances apply changes immediately. Session-owned instances defer changes requested
/// during native drawing until the next style preparation. They are mutable only on the owner thread
/// and must not be modified after their session retires.
/// </remarks>
public sealed class EditorStyleMetrics
{
    /// <summary>
    /// Defines the smallest supported editor UI zoom.
    /// </summary>
    public const float C_MIN_ZOOM = 0.75f;

    /// <summary>
    /// Defines the largest supported editor UI zoom.
    /// </summary>
    public const float C_MAX_ZOOM = 1.50f;

    /// <summary>
    /// Defines one keyboard or menu zoom increment.
    /// </summary>
    public const float C_ZOOM_STEP = 0.10f;

    private readonly WeakReference<EditorUiSession>? m_owner;
    private float m_requestedZoom = 1f;
    private bool m_requestedCompactMode;
    private float m_zoom = 1f;
    private bool m_compactMode;

    /// <summary>
    /// Creates independent metrics whose changes apply immediately without a native context.
    /// </summary>
    public EditorStyleMetrics() { }

    internal EditorStyleMetrics(EditorUiSession owner)
    {
        ArgumentNullException.ThrowIfNull(owner);
        m_owner = new(owner);
    }

    /// <summary>
    /// Gets the effective zoom used by the current presentation frame.
    /// </summary>
    public float zoom => m_zoom;

    /// <summary>
    /// Gets whether compact editor density is active.
    /// </summary>
    public bool isCompact => m_compactMode;

    /// <summary>
    /// Gets the scaled height of a compact sampled-value trend plot.
    /// </summary>
    public float trendHeight => Scale(64f);

    /// <summary>
    /// Requests comfortable or compact density without changing an active session frame.
    /// </summary>
    /// <param name="value">
    /// <see langword="true"/> for compact density.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the requested density changed; owned drawing applies it at the next frame preparation.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    /// The owning session has retired or is no longer available.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The caller is not the owning session's thread.
    /// </exception>
    public bool SetCompactMode(bool value)
    {
        bool defer = ShouldDeferChanges();
        bool changed = m_requestedCompactMode != value;
        m_requestedCompactMode = value;
        if (!defer)
            CommitPending();
        return changed;
    }

    /// <summary>
    /// Requests a bounded zoom without changing metrics partway through an owned native frame.
    /// </summary>
    /// <param name="value">
    /// The requested UI zoom multiplier.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the requested zoom changed; owned drawing applies it at the next frame preparation.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="value"/> is not finite.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// The owning session has retired or is no longer available.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The caller is not the owning session's thread.
    /// </exception>
    public bool SetZoom(float value)
    {
        if (!float.IsFinite(value))
            throw new ArgumentOutOfRangeException(nameof(value), value, "UI zoom must be finite.");
        bool defer = ShouldDeferChanges();
        float clamped = Math.Clamp(value, C_MIN_ZOOM, C_MAX_ZOOM);
        bool changed = MathF.Abs(m_requestedZoom - clamped) >= 0.0001f;
        m_requestedZoom = clamped;
        if (!defer)
            CommitPending();
        return changed;
    }

    /// <summary>
    /// Increases editor UI zoom by one bounded increment.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> when the requested zoom changed.
    /// </returns>
    public bool ZoomIn() => SetZoom(m_requestedZoom + C_ZOOM_STEP);

    /// <summary>
    /// Decreases editor UI zoom by one bounded increment.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> when the requested zoom changed.
    /// </returns>
    public bool ZoomOut() => SetZoom(m_requestedZoom - C_ZOOM_STEP);

    /// <summary>
    /// Restores the baseline editor UI zoom.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> when the requested zoom changed.
    /// </returns>
    public bool ResetZoom() => SetZoom(1f);

    /// <summary>
    /// Gets global content scale.
    /// </summary>
    public float fontScale => Scale(m_compactMode ? 1.10f : 1.20f);

    /// <summary>
    /// Gets disabled content opacity.
    /// </summary>
    public float disabledAlpha => EditorPalette.opacityMedium;

    /// <summary>
    /// Gets standard window padding.
    /// </summary>
    public Vector2 windowPadding => ScaleVector(m_compactMode ? new(5f, 4f) : new(8f, 7f));

    /// <summary>
    /// Gets standard window rounding.
    /// </summary>
    public float windowRounding => 0f;

    /// <summary>
    /// Gets standard border thickness.
    /// </summary>
    public float borderSize => Scale(1f);

    /// <summary>
    /// Gets minimum window size.
    /// </summary>
    public Vector2 windowMinimumSize => ScaleVector(new(30f, 30f));

    /// <summary>
    /// Gets standard frame padding.
    /// </summary>
    public Vector2 framePadding => ScaleVector(m_compactMode ? new(4f, 1f) : new(7f, 3f));

    /// <summary>
    /// Gets the frame padding used while docked panel tabs are laid out.
    /// </summary>
    public Vector2 panelTabFramePadding => ScaleVector(m_compactMode ? new(4f, 3f) : new(7f, 4f));

    /// <summary>
    /// Gets the uniform content padding of editor context menus.
    /// </summary>
    public Vector2 menuWindowPadding => ScaleVector(new(8f, 6f));

    /// <summary>
    /// Gets the uniform item padding of editor context menus.
    /// </summary>
    public Vector2 menuFramePadding => ScaleVector(new(8f, 3f));

    /// <summary>
    /// Gets the compact padding used by the search field at the top of editor context menus.
    /// Its vertical padding matches the half-spacing that menu entries add above and below a line.
    /// </summary>
    public Vector2 menuSearchFramePadding => ScaleVector(new(8f, 1f));

    /// <summary>
    /// Gets the uniform spacing between editor context-menu items.
    /// </summary>
    public Vector2 menuItemSpacing => ScaleVector(new(4f, 2f));

    /// <summary>
    /// Gets editor context-menu corner rounding.
    /// </summary>
    public float menuRounding => Scale(3f);

    /// <summary>
    /// Gets editor context-menu border thickness.
    /// </summary>
    public float menuBorderSize => Scale(1f);

    /// <summary>
    /// Gets compact frame padding.
    /// </summary>
    public Vector2 compactFramePadding => ScaleVector(new(4f, 1f));

    /// <summary>
    /// Gets the compact inner padding of inline rename fields.
    /// </summary>
    public Vector2 inlineRenameFramePadding => ScaleVector(new(4f, 0f));

    /// <summary>
    /// Gets toolbar frame padding.
    /// </summary>
    public Vector2 toolbarFramePadding => ScaleVector(new(5f, 1f));

    /// <summary>
    /// Gets breadcrumb frame padding.
    /// </summary>
    public Vector2 breadcrumbFramePadding => ScaleVector(new(6f, 1f));

    /// <summary>
    /// Gets standard frame rounding.
    /// </summary>
    public float frameRounding => Scale(2f);

    /// <summary>
    /// Gets standard item spacing.
    /// </summary>
    public Vector2 itemSpacing => ScaleVector(m_compactMode ? new(4f, 2f) : new(6f, 4f));

    /// <summary>
    /// Gets compact vertical item spacing.
    /// </summary>
    public Vector2 compactItemSpacing => ScaleVector(new(4f, 2f));

    /// <summary>
    /// Gets standard inner item spacing.
    /// </summary>
    public Vector2 itemInnerSpacing => ScaleVector(m_compactMode ? new(3f, 3f) : new(5f, 4f));

    /// <summary>
    /// Gets standard table cell padding.
    /// </summary>
    public Vector2 cellPadding => ScaleVector(m_compactMode ? new(3f, 1f) : new(5f, 3f));

    /// <summary>
    /// Gets tree indentation.
    /// </summary>
    public float indentSpacing => Scale(m_compactMode ? 18f : 22f);

    /// <summary>
    /// Gets minimum column spacing.
    /// </summary>
    public float columnMinimumSpacing => Scale(4f);

    /// <summary>
    /// Gets scrollbar width.
    /// </summary>
    public float scrollbarSize => Scale(m_compactMode ? 10f : 12f);

    /// <summary>
    /// Gets the inset that keeps an overlay scrollbar grab visually lightweight.
    /// </summary>
    public float scrollbarPadding => Scale(3f);

    /// <summary>
    /// Gets minimum grab size.
    /// </summary>
    public float grabMinimumSize => Scale(m_compactMode ? 10f : 12f);

    /// <summary>
    /// Gets the minimum visible width retained for either resizable content pane while its splitter is dragged.
    /// </summary>
    public float paneMinimumVisibleWidth => Scale(16f);

    /// <summary>
    /// Gets the minimum draggable resizable content splitter width.
    /// </summary>
    public float splitterMinimumWidth => Scale(2f);

    /// <summary>
    /// Gets spacing between cards.
    /// </summary>
    public float cardSpacing => Scale(3f);

    /// <summary>
    /// Gets the inner padding of the persistent target header.
    /// </summary>
    public Vector2 targetHeaderPadding => ScaleVector(new(6f, 4f));

    /// <summary>
    /// Gets spacing between the two rows of the persistent target header.
    /// </summary>
    public float targetHeaderRowSpacing => Scale(1f);

    /// <summary>
    /// Gets the font-size multiplier used by the large target icon.
    /// </summary>
    public float targetIconScale => 1.65f;

    /// <summary>
    /// Gets card body padding.
    /// </summary>
    public Vector2 cardBodyPadding => ScaleVector(new(7f, 5f));

    /// <summary>
    /// Gets card header padding.
    /// </summary>
    public Vector2 cardHeaderPadding => ScaleVector(new(4f, 1f));

    /// <summary>
    /// Gets the shared padding around separator-style section titles.
    /// </summary>
    public Vector2 sectionHeaderPadding => ScaleVector(new(20f, 3f));

    /// <summary>
    /// Gets the inner padding of a framed Inspector section.
    /// </summary>
    public Vector2 sectionPadding => ScaleVector(new(7f, 5f));

    /// <summary>
    /// Gets the gap between an Inspector section legend and its interrupted top border.
    /// </summary>
    public float sectionLegendGap => itemSpacing.X;

    /// <summary>
    /// Gets the length of each centered end cap on a collapsed Inspector section.
    /// </summary>
    public float collapsedSectionCapLength => Scale(10f);

    /// <summary>
    /// Gets the spacing around the separator between inline metadata and its value.
    /// </summary>
    public float propertyMetadataSpacing => Scale(5f);

    /// <summary>
    /// Gets the vertical distance after a framed Inspector section.
    /// </summary>
    public float sectionSpacing => Scale(4f);

    /// <summary>
    /// Gets the subtle corner rounding used by framed content sections inside editor containers.
    /// </summary>
    public float sectionRounding => Scale(3f);

    /// <summary>
    /// Gets card disclosure inset.
    /// </summary>
    public float disclosureButtonInset => Scale(2f);

    /// <summary>
    /// Gets property label width ratio.
    /// </summary>
    public float propertyLabelRatio => 0.40f;

    /// <summary>
    /// Gets the default width of searchable editor popups.
    /// </summary>
    public float searchPopupWidth => Scale(280f);

    /// <summary>
    /// Gets spacing between a leading icon and its text.
    /// </summary>
    public float iconLabelSpacing => Scale(6f);

    /// <summary>
    /// Gets the compact checkbox size used by card headers.
    /// </summary>
    public float compactCheckboxSize => Scale(13f);

    /// <summary>
    /// Gets spacing between leading controls inside card headers.
    /// </summary>
    public float headerControlSpacing => Scale(4f);

    /// <summary>
    /// Gets the inner padding of compact colored label chips.
    /// </summary>
    public Vector2 labelChipPadding => ScaleVector(new(6f, 1f));

    /// <summary>
    /// Gets the corner rounding of compact colored label chips.
    /// </summary>
    public float labelChipRounding => frameRounding;

    /// <summary>
    /// Gets the compact inner padding of outlined type badges.
    /// </summary>
    public Vector2 typeBadgePadding => ScaleVector(new(5f, 1f));

    /// <summary>
    /// Gets the corner rounding of outlined type badges.
    /// </summary>
    public float typeBadgeRounding => Scale(3f);

    /// <summary>
    /// Gets the horizontal gap between a property name and its type badge.
    /// </summary>
    public float typeBadgeSpacing => Scale(6f);

    /// <summary>
    /// Gets spacing between distinct control groups in the target header row.
    /// </summary>
    public float headerSectionSpacing => Scale(10f);

    /// <summary>
    /// Gets top padding above trailing add buttons.
    /// </summary>
    public float addButtonTopPadding => Scale(7f);

    /// <summary>
    /// Gets the tight resizable content toolbar spacing.
    /// </summary>
    public float toolbarTightSpacing => Scale(2f);

    /// <summary>
    /// Gets the regular resizable content toolbar spacing.
    /// </summary>
    public float toolbarSpacing => Scale(4f);

    /// <summary>
    /// Gets spacing between resizable content toolbar sections.
    /// </summary>
    public float toolbarSectionSpacing => Scale(5f);

    /// <summary>
    /// Gets the minimum width of one axis value field.
    /// </summary>
    public float axisValueMinimumWidth => Scale(24f);

    /// <summary>
    /// Gets the minimum width of an axis prefix.
    /// </summary>
    public float axisPrefixMinimumWidth => Scale(18f);

    /// <summary>
    /// Gets the axis prefix share of the complete control.
    /// </summary>
    public float axisPrefixWidthRatio => 0.36f;

    /// <summary>
    /// Gets the standard foreground interaction-overlay thickness.
    /// </summary>
    public float interactionOverlayThickness => Scale(2f);

    /// <summary>
    /// Gets script compilation modal width.
    /// </summary>
    public float scriptCompilationWidth => Scale(460f);

    /// <summary>
    /// Gets the standard centered modal width.
    /// </summary>
    public float modalWidth => scriptCompilationWidth;

    /// <summary>
    /// Gets the modal fade-in duration in seconds.
    /// </summary>
    public double modalFadeInSeconds { get; } = 0.12;

    /// <summary>
    /// Gets the minimum modal visibility duration in seconds.
    /// </summary>
    public double modalMinimumVisibleSeconds { get; } = 0.35;

    /// <summary>
    /// Gets the modal fade-out duration in seconds.
    /// </summary>
    public double modalFadeOutSeconds { get; } = 0.14;

    /// <summary>
    /// Gets tree guide left offset.
    /// </summary>
    public float treeGuideLeftOffset => Scale(1f);

    /// <summary>
    /// Gets the overlap used to join adjacent tree guide segments.
    /// </summary>
    public float treeGuideLineOverlap => Scale(1f);

    /// <summary>
    /// Gets additional connector padding for expandable tree nodes.
    /// </summary>
    public float treeFolderConnectorPadding => Scale(2f);

    /// <summary>
    /// Gets the vertical offset of text decorations from the baseline.
    /// </summary>
    public float textDecorationOffset => Scale(2f);

    internal void CommitPending()
    {
        m_zoom = m_requestedZoom;
        m_compactMode = m_requestedCompactMode;
    }

    private bool ShouldDeferChanges()
    {
        if (m_owner is null)
            return false;
        if (!m_owner.TryGetTarget(out EditorUiSession? owner))
            throw new ObjectDisposedException(nameof(EditorUiSession));
        return owner.shouldDeferStyleChanges;
    }

    private float Scale(float value) => value * m_zoom;

    private Vector2 ScaleVector(Vector2 value) => value * m_zoom;
}
