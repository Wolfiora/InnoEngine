using System;
using System.Collections.Generic;
using System.Reflection;

using Inno.Extensibility.Catalogs;

namespace Inno.Extensibility.Types;

/// <summary>
/// Provides type metadata for an explicit assembly in a candidate module generation.
/// </summary>
public interface ITypeCatalogSource
{
    /// <summary>
    /// Resolves the complete type set without publishing a partial generation.
    /// </summary>
    /// <param name="assembly">
    /// An assembly owned by the candidate module catalog.
    /// </param>
    /// <returns>
    /// The complete type metadata for this assembly; unsupported or invalid metadata throws.
    /// </returns>
    IReadOnlyList<Type> GetTypes(Assembly assembly);

    /// <summary>
    /// Resolves immutable discovery facts for a declaration in this source.
    /// </summary>
    /// <param name="type">
    /// The declaration whose relationships and attributes are required.
    /// </param>
    /// <returns>
    /// Complete metadata owned by the caller's candidate generation; unavailable declarations throw.
    /// </returns>
    TypeCatalogMetadata GetMetadata(Type type);

    /// <summary>
    /// Resolves a generic construction through the deployment's linked or dynamic type mechanism.
    /// </summary>
    /// <param name="definition">
    /// A generic declaration owned by the candidate generation.
    /// </param>
    /// <param name="arguments">
    /// The complete ordered set of closed type arguments.
    /// </param>
    /// <returns>
    /// The closed type, or null when its generic constraints reject these arguments.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// A static deployment did not link the requested construction.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The declaration is not generic, or the argument count or closedness is invalid.
    /// </exception>
    Type? ConstructGenericType(
        Type definition,
        IReadOnlyList<Type> arguments
    );

    /// <summary>
    /// Determines whether this deployment supplies a parameterless factory for a declared type.
    /// </summary>
    /// <param name="type">
    /// The declared runtime type.
    /// </param>
    /// <returns>
    /// Whether instance construction is supported by the selected deployment.
    /// </returns>
    bool CanCreateInstance(Type type);

    /// <summary>
    /// Constructs a declared type through the selected deployment's factory mechanism.
    /// </summary>
    /// <param name="type">
    /// The declared runtime type.
    /// </param>
    /// <returns>
    /// A new instance owned by the caller; missing factories or failed constructors throw.
    /// </returns>
    object CreateInstance(Type type);
}
