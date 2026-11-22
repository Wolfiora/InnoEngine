using System;

namespace Inno.Editor.Shaders.ImGui;

/// <summary>
/// Registers Editor-only node controls independently from a node's shader compiler.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class ShaderNodeDrawerAttribute : Attribute
{
    /// <summary>
    /// Associates the drawer with one stable node definition.
    /// </summary>
    /// <param name="definitionID">
    /// The compiler-independent graph node identity.
    /// </param>
    /// <param name="displayName">
    /// Optional artist-facing name used in node headers and creation menus.
    /// </param>
    /// <param name="createPath">
    /// Optional slash-delimited path below Create. Empty uses the host's built-in classification.
    /// </param>
    /// <param name="createOrder">
    /// Stable ordering value among sibling creation groups and nodes.
    /// </param>
    /// <param name="separatorBefore">
    /// Whether the contributed creation group is preceded by a separator at its own level.
    /// </param>
    public ShaderNodeDrawerAttribute(
        string definitionID,
        string displayName = "",
        string createPath = "",
        int createOrder = 0,
        bool separatorBefore = false
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(definitionID);
        ArgumentNullException.ThrowIfNull(displayName);
        ArgumentNullException.ThrowIfNull(createPath);
        this.definitionID = definitionID;
        this.displayName = displayName;
        this.createPath = createPath;
        this.createOrder = createOrder;
        this.separatorBefore = separatorBefore;
    }

    /// <summary>
    /// Gets the stable node identity handled by this drawer.
    /// </summary>
    public string definitionID { get; }

    /// <summary>
    /// Gets the optional presentation-only name; it does not participate in Shader compilation or identity.
    /// </summary>
    public string displayName { get; }

    /// <summary>
    /// Gets the optional creation-menu path below Create.
    /// </summary>
    public string createPath { get; }

    /// <summary>
    /// Gets the creation-menu order.
    /// </summary>
    public int createOrder { get; }

    /// <summary>
    /// Gets whether the contributed creation group starts a new visual section.
    /// </summary>
    public bool separatorBefore { get; }
}
