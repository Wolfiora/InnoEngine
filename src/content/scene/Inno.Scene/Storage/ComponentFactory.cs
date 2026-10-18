using System;
using Inno.Extensibility.Types;

namespace Inno.Scene;

/// <summary>
/// Creates component instances without retaining runtime type or constructor caches.
/// </summary>
internal static class ComponentFactory
{
    internal static GameComponent Create(
        Type componentType,
        TypeCacheSnapshot types
    ) {
        ArgumentNullException.ThrowIfNull(componentType);
        ArgumentNullException.ThrowIfNull(types);
        if (!typeof(GameComponent).IsAssignableFrom(componentType) || componentType.IsAbstract || !componentType.IsClass)
            throw new ArgumentException($"Type '{componentType.FullName}' is not a concrete GameComponent.", nameof(componentType));
        return (GameComponent)types.CreateInstance(componentType);
    }
}
