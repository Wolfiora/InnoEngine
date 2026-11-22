using System;

namespace Inno.Editor.Rendering;

/// <summary>
/// Marks a reloadable rendering-model contributor for one Editor viewport purpose.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class EditorViewportContributorExtensionAttribute : Attribute
{
    /// <summary>
    /// Creates a viewport contributor declaration.
    /// </summary>
    /// <param name="id">
    /// Globally stable contributor identity.
    /// </param>
    /// <param name="kind">
    /// Open viewport purpose handled by the contributor.
    /// </param>
    /// <param name="order">
    /// Ascending model-composition order within the viewport.
    /// </param>
    /// <param name="controllerPriority">
    /// Priority used to select the contributor that owns navigation, tools, and pointer interaction.
    /// </param>
    public EditorViewportContributorExtensionAttribute(
        string id,
        string kind,
        int order = 0,
        int controllerPriority = 0
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(kind);
        this.id = id.Trim();
        this.kind = new EditorViewportKindID(kind);
        this.order = order;
        this.controllerPriority = controllerPriority;
    }

    /// <summary>
    /// Gets the globally stable contributor identity.
    /// </summary>
    public string id { get; }

    /// <summary>
    /// Gets the open viewport purpose handled by the contributor.
    /// </summary>
    public EditorViewportKindID kind { get; }

    /// <summary>
    /// Gets ascending model-composition order.
    /// </summary>
    public int order { get; }

    /// <summary>
    /// Gets the priority used to select the viewport interaction controller.
    /// </summary>
    public int controllerPriority { get; }
}
