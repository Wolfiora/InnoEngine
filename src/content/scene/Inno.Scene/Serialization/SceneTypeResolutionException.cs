using System;

namespace Inno.Scene;

/// <summary>
/// Reports that a serialized scene element cannot be created because its stable type is not present
/// in the active type catalog.
/// </summary>
public sealed class SceneTypeResolutionException : Exception
{
    /// <summary>
    /// Creates an exception for one unresolved serialized scene type.
    /// </summary>
    /// <param name="stableTypeID">
    /// The stable identity stored by the serialized scene element.
    /// </param>
    /// <param name="elementKind">
    /// The human-readable scene element kind, such as component or system.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="stableTypeID"/> is empty or <paramref name="elementKind"/> is empty.
    /// </exception>
    public SceneTypeResolutionException(
        Guid stableTypeID,
        string elementKind
    ) : base($"Scene {elementKind} stable type id '{stableTypeID}' is not loaded.")
    {
        if (stableTypeID == Guid.Empty)
        {
            throw new ArgumentException("A stable scene type identity is required.", nameof(stableTypeID));
        }
        ArgumentException.ThrowIfNullOrWhiteSpace(elementKind);
        this.stableTypeID = stableTypeID;
        this.elementKind = elementKind;
    }

    /// <summary>
    /// Gets the unresolved stable type identity.
    /// </summary>
    public Guid stableTypeID { get; }

    /// <summary>
    /// Gets the scene element kind that expected the missing type.
    /// </summary>
    public string elementKind { get; }
}
