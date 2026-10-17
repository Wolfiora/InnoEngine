using System;
using System.Collections.Generic;

namespace Inno.Extensibility.Catalogs;

/// <summary>
/// Accepts explicit declarations and linked generic factories while a static catalog is being composed.
/// </summary>
/// <remarks>
/// Generated contributors run synchronously on the composition thread and must not retain the registrar.
/// Its registrations belong to the resulting generation. Constructors execute only when requested.
/// </remarks>
public interface ITypeCatalogRegistrar
{
    /// <summary>
    /// Registers immutable discovery metadata and an optional parameterless construction function.
    /// </summary>
    /// <param name="metadata">
    /// Complete discovery facts for one assembly-local declaration.
    /// </param>
    /// <param name="factory">
    /// Creates a new instance, or null when the declaration is not constructible.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The declaration has already been registered.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The caller runs on another thread or retained this registrar after catalog composition completed.
    /// </exception>
    void Register(
        TypeCatalogMetadata metadata,
        Func<object>? factory
    );

    /// <summary>
    /// Registers a closed generic construction linked by the contributing assembly.
    /// </summary>
    /// <param name="type">
    /// A fully closed generic type whose definition is contributed by the completed catalog.
    /// </param>
    /// <param name="factory">
    /// Creates a new instance of the linked construction without dynamic code generation.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The type is not a fully closed generic construction.
    /// </exception>
    /// <remarks>
    /// Repeated registrations of the same closed construction are idempotent; the first factory is retained.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// The caller runs on another thread or retained this registrar after catalog composition completed.
    /// </exception>
    void RegisterFactory(
        Type type,
        Func<object> factory
    );

    /// <summary>
    /// Records a construction whose argument set was rejected by the declaration's compile-time constraints.
    /// </summary>
    /// <param name="definition">
    /// The generic declaration contributed by the completed catalog.
    /// </param>
    /// <param name="arguments">
    /// The complete closed argument set. The registrar copies this collection during registration.
    /// </param>
    /// <remarks>
    /// This fact lets static resolution skip an inapplicable converter without attempting dynamic generic creation.
    /// Unknown constructions remain explicit deployment errors. Repeated rejections are idempotent.
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// The declaration or argument set is incomplete, open, or conflicts with a registered factory.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The declaration or argument collection is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The caller runs on another thread or retained this registrar after catalog composition completed.
    /// </exception>
    void RejectGenericConstruction(
        Type definition,
        IReadOnlyList<Type> arguments
    );
}
