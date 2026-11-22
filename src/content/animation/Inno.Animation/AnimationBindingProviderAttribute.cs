using System;

namespace Inno.Animation;

/// <summary>
/// Declares a stable, automatically discovered animation destination protocol.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class AnimationBindingProviderAttribute : Attribute
{
    /// <summary>
    /// Declares one provider and the value protocol it implements.
    /// </summary>
    /// <param name="id">
    /// Globally unique stable extension identifier.
    /// </param>
    /// <param name="bindingID">
    /// Stable track binding protocol, independent of CLR type names.
    /// </param>
    /// <param name="kind">
    /// The accepted value representation.
    /// </param>
    /// <exception cref="ArgumentException">
    /// An identifier is blank or the value representation is undefined.
    /// </exception>
    public AnimationBindingProviderAttribute(
        string id,
        string bindingID,
        AnimationValueKind kind
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(bindingID);
        if (!Enum.IsDefined(kind))
        {
            throw new ArgumentException("A defined animation value kind is required.", nameof(kind));
        }
        this.id = id;
        this.bindingID = new AnimationBindingID(bindingID);
        this.kind = kind;
    }

    /// <summary>
    /// Gets the stable provider identifier.
    /// </summary>
    public string id { get; }

    /// <summary>
    /// Gets the supported track binding protocol.
    /// </summary>
    public AnimationBindingID bindingID { get; }

    /// <summary>
    /// Gets the accepted value representation.
    /// </summary>
    public AnimationValueKind kind { get; }
}
