using System;
using System.Numerics;
using Inno.Editor.ImGui;

namespace Inno.Editor.Assets.ImGui;

/// <summary>
/// Freezes Browser-specific layout values from the shared UI scale without retaining a session.
/// The default value uses the standard 100 percent scale.
/// </summary>
public readonly struct AssetBrowserStyle
{
    private readonly float m_zoom;

    /// <summary>
    /// Captures the current shared scale for one Browser presentation pass.
    /// </summary>
    /// <param name="metrics">
    /// The existing session metrics; later mutations do not change this value.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The metrics are null.
    /// </exception>
    public AssetBrowserStyle(EditorStyleMetrics metrics)
    {
        ArgumentNullException.ThrowIfNull(metrics);
        m_zoom = metrics.zoom;
    }

    /// <summary>
    /// Gets asset browser window padding.
    /// </summary>
    public Vector2 windowPadding => ScaleVector(new(2f, 1f));

    /// <summary>
    /// Gets asset browser cell padding.
    /// </summary>
    public Vector2 cellPadding => ScaleVector(new(5f, 2f));

    /// <summary>
    /// Gets asset browser item spacing.
    /// </summary>
    public Vector2 itemSpacing => ScaleVector(new(2f, 2f));

    /// <summary>
    /// Gets asset browser frame rounding.
    /// </summary>
    public float frameRounding => Scale(1f);

    /// <summary>
    /// Gets asset breadcrumb bar height.
    /// </summary>
    public float breadcrumbHeight => Scale(25f);

    /// <summary>
    /// Gets minimum asset grid cell size.
    /// </summary>
    public float gridMinimumCellSize => Scale(32f);

    /// <summary>
    /// Gets asset grid cell padding.
    /// </summary>
    public float gridCellPadding => Scale(2f);

    /// <summary>
    /// Gets default asset grid scale.
    /// </summary>
    public float gridDefaultScale => 3f;

    /// <summary>
    /// Gets minimum asset grid scale.
    /// </summary>
    public float gridMinimumScale => 1f;

    /// <summary>
    /// Gets maximum asset grid scale.
    /// </summary>
    public float gridMaximumScale => 10f;

    /// <summary>
    /// Gets spacing between asset list rows.
    /// </summary>
    public float listRowSpacing => Scale(2f);

    /// <summary>
    /// Gets the default normalized position of the asset list name/type separator.
    /// </summary>
    public float listNameSeparatorPosition => 0.4f;

    /// <summary>
    /// Gets the default normalized position of the asset list type/source separator.
    /// </summary>
    public float listTypeSeparatorPosition => 0.7f;

    /// <summary>
    /// Gets the minimum normalized width reserved for each asset list column.
    /// </summary>
    public float listMinimumColumnRatio => 0.1f;

    /// <summary>
    /// Gets the horizontal hit width of an asset list column separator.
    /// </summary>
    public float listSeparatorHitWidth => Scale(8f);

    /// <summary>
    /// Gets the horizontal inset between an asset list separator and column content.
    /// </summary>
    public float listContentHorizontalPadding => Scale(6f);

    /// <summary>
    /// Gets horizontal padding for asset grid labels.
    /// </summary>
    public float gridLabelHorizontalPadding => Scale(6f);

    /// <summary>
    /// Gets bottom padding for asset grid labels.
    /// </summary>
    public float gridLabelBottomPadding => Scale(3f);

    /// <summary>
    /// Gets the additional vertical spacing between asset grid label lines.
    /// </summary>
    public float gridLabelLineSpacing => Scale(-2f);

    /// <summary>
    /// Gets fixed padding added to calculated asset grid cells.
    /// </summary>
    public float gridFixedCellPadding => Scale(8f);

    /// <summary>
    /// Gets the scale bias added to asset grid icons.
    /// </summary>
    public float gridScaleBias => 2f;

    /// <summary>
    /// Gets the top inset reserved above an asset grid icon.
    /// </summary>
    public float gridIconTopPadding => Scale(6f);

    /// <summary>
    /// Gets the horizontal inset that constrains an asset grid icon inside its card.
    /// </summary>
    public float gridIconHorizontalPadding => Scale(6f);

    /// <summary>
    /// Gets the vertical spacing between an asset grid icon and its label.
    /// </summary>
    public float gridIconLabelSpacing => Scale(4f);

    /// <summary>
    /// Gets spacing around breadcrumb separators.
    /// </summary>
    public float breadcrumbSpacing => Scale(4f);

    private float Scale(float value) => value * (m_zoom == 0f ? 1f : m_zoom);

    private Vector2 ScaleVector(Vector2 value) => value * (m_zoom == 0f ? 1f : m_zoom);
}
