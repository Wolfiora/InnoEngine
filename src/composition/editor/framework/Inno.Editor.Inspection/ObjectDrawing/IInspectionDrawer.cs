using System;

namespace Inno.Editor.Inspection;

/// <summary>
/// Defines the runtime bridge used to render a discovered inspection drawer.
/// </summary>
public interface IInspectionDrawer
{
    /// <summary>
    /// Determines whether a type-compatible selected value can be inspected by this drawer.
    /// </summary>
    /// <param name="target">
    /// Selected value, never mutated by this query.
    /// </param>
    /// <returns>
    /// Whether this drawer accepts the value; ordinary type-only drawers accept by default.
    /// </returns>
    bool CanInspect(object target) => true;

    /// <summary>
    /// Gets the icon glyph displayed in the Inspector target header.
    /// </summary>
    /// <param name="context">
    /// The active Inspector drawing context.
    /// </param>
    /// <returns>
    /// The icon glyph for the current target.
    /// </returns>
    string GetIcon(InspectionDrawContext context);

    /// <summary>
    /// Binds the target name displayed by the Inspector target header.
    /// </summary>
    /// <param name="context">
    /// The active Inspector drawing context.
    /// </param>
    /// <returns>
    /// The current target name and a callback that accepts a requested replacement name, or
    /// <see langword="null"/> for the callback when the name is read-only.
    /// </returns>
    (string name, Action<string>? setter) BindName(InspectionDrawContext context);

    /// <summary>
    /// Draws the target-specific second header row.
    /// </summary>
    /// <param name="context">
    /// The active Inspector drawing context.
    /// </param>
    void DrawHeader(InspectionDrawContext context);

    /// <summary>
    /// Draws the target-specific Inspector body.
    /// </summary>
    /// <param name="context">
    /// The active Inspector drawing context.
    /// </param>
    void Draw(InspectionDrawContext context);
}
