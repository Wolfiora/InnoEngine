using System;

namespace Inno.Editor.Core;
/// <summary>
/// Ranks assignable editor extension targets by class ancestry, with interface registrations one step away.
/// </summary>
public static class EditorTypeSpecificity
{
    /// <summary>
    /// Determines whether a registration accepts a concrete target and returns its specificity distance.
    /// </summary>
    /// <param name="concreteType">
    /// The target type supplied by the active extension generation.
    /// </param>
    /// <param name="candidateType">
    /// The registered class or interface accepted by the extension.
    /// </param>
    /// <param name="distance">
    /// Zero for an exact match, ancestry distance for classes, or one for an interface.
    /// </param>
    /// <returns>
    /// True when the registration accepts the target; otherwise false and the maximum distance.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Either type is null.
    /// </exception>
    public static bool TryGet(
        Type concreteType,
        Type candidateType,
        out int distance
    ) {
        ArgumentNullException.ThrowIfNull(concreteType);
        ArgumentNullException.ThrowIfNull(candidateType);
        if (!candidateType.IsAssignableFrom(concreteType))
        {
            distance = int.MaxValue;
            return false;
        }

        if (concreteType == candidateType)
        {
            distance = 0;
            return true;
        }

        distance = 1;
        Type? current = concreteType.BaseType;
        while (current is not null)
        {
            if (current == candidateType)
                return true;
            distance++;
            current = current.BaseType;
        }

        distance = candidateType.IsInterface ? 1 : distance;
        return true;
    }
}
