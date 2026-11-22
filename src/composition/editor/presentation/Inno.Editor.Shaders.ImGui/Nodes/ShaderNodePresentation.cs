namespace Inno.Editor.Shaders.ImGui;

/// <summary>
/// Immutable authoring presentation contributed for one Shader node definition.
/// </summary>
public readonly record struct ShaderNodePresentation
{
    /// <summary>
    /// Creates presentation metadata for one Shader node definition.
    /// </summary>
    /// <param name="displayName">
    /// Optional artist-facing node name.
    /// </param>
    /// <param name="createPath">
    /// Optional slash-delimited creation-menu path.
    /// </param>
    /// <param name="createOrder">
    /// Stable ordering value within the creation menu.
    /// </param>
    /// <param name="separatorBefore">
    /// Whether the contributed group starts a visual section.
    /// </param>
    public ShaderNodePresentation(
        string displayName,
        string createPath,
        int createOrder,
        bool separatorBefore
    ) {
        this.displayName = displayName;
        this.createPath = createPath;
        this.createOrder = createOrder;
        this.separatorBefore = separatorBefore;
    }

    /// <summary>
    /// Gets the optional artist-facing node name.
    /// </summary>
    public string displayName { get; }

    /// <summary>
    /// Gets the optional slash-delimited creation-menu path.
    /// </summary>
    public string createPath { get; }

    /// <summary>
    /// Gets the stable ordering value within the creation menu.
    /// </summary>
    public int createOrder { get; }

    /// <summary>
    /// Gets whether the contributed group starts a visual section.
    /// </summary>
    public bool separatorBefore { get; }
}
