using System;

namespace Inno.Editor.Panel.Inspector;

/// <summary>
/// Borrows an asset property's type and assignment callback for one synchronous drop request.
/// </summary>
/// <remarks>
/// Recreate this target for each presentation request. Its type and callback must not be retained
/// across frames or extension generations, or included in History or persistent state.
/// </remarks>
public sealed class AssetReferenceDropTarget
{
    private readonly Action<Guid> m_assign;
    private readonly Type m_expectedType;

    /// <summary>
    /// Creates a drop target that validates an asset type and assigns its persistent identity to a property.
    /// </summary>
    /// <param name="expectedType">
    /// The runtime asset type accepted by the property.
    /// </param>
    /// <param name="assign">
    /// The callback that writes an accepted persistent identity to the property.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when either argument is <see langword="null"/>.
    /// </exception>
    public AssetReferenceDropTarget(
        Type expectedType,
        Action<Guid> assign
    ) {
        ArgumentNullException.ThrowIfNull(expectedType);
        m_expectedType = expectedType;
        m_assign = assign ?? throw new ArgumentNullException(nameof(assign));
    }

    /// <summary>
    /// Gets the required asset type.
    /// </summary>
    public Type expectedType => m_expectedType;

    /// <summary>
    /// Assigns a persistent asset identity to the represented property.
    /// </summary>
    /// <param name="persistentId">
    /// The stable persistent identity used for lookup.
    /// </param>
    public void Assign(Guid persistentId) => m_assign(persistentId);
}
