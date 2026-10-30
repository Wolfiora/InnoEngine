using System;

namespace Inno.Core.Serialization;

/// <summary>
/// Declares a sealed identity source's transient revision member, which covers all serialized state and pure getters.
/// </summary>
/// <remarks>
/// The member must return ulong or nullable ulong. Every relevant write must advance its value before mutation;
/// null declines reuse. The counter is not serialized. Metadata providers explicitly opt into this declaration.
/// </remarks>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class SerializationRevisionAttribute : Attribute
{
    /// <summary>
    /// Selects the source-owned nonserialized revision field or property.
    /// </summary>
    /// <param name="memberName">
    /// The exact nonstatic field or readable nonindexed property name.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The name is blank.
    /// </exception>
    public SerializationRevisionAttribute(string memberName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(memberName);
        this.memberName = memberName;
    }

    /// <summary>
    /// Gets the exact source-owned revision member name.
    /// </summary>
    public string memberName { get; }
}
