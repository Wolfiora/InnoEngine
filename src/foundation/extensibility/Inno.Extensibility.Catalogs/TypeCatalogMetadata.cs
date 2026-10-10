using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Extensibility.Catalogs;

/// <summary>
/// Freezes a declaration's discovery facts independently of its runtime's reflection implementation.
/// </summary>
public sealed class TypeCatalogMetadata
{
    /// <summary>
    /// Captures type relationships and attributes for one candidate generation.
    /// </summary>
    /// <param name="type">
    /// The declared type belonging to the contributing assembly.
    /// </param>
    /// <param name="baseTypes">
    /// Ordered base types, starting with the immediate base and excluding System.Object.
    /// </param>
    /// <param name="interfaces">
    /// All implemented interfaces, including inherited interfaces.
    /// </param>
    /// <param name="declaredAttributes">
    /// Attributes declared directly on this type.
    /// </param>
    /// <param name="inheritedAttributes">
    /// The effective attributes after applying inheritance and multiplicity rules.
    /// </param>
    /// <param name="parameterlessOverrides">
    /// Effective parameterless instance overrides, paired with the declaration owning their virtual slot.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// A required declaration or collection is null.
    /// </exception>
    public TypeCatalogMetadata(
        Type type,
        IReadOnlyList<Type> baseTypes,
        IReadOnlyList<Type> interfaces,
        IReadOnlyList<Attribute> declaredAttributes,
        IReadOnlyList<Attribute> inheritedAttributes,
        IReadOnlyList<(string name, Type declaringBase)> parameterlessOverrides
    ) {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(baseTypes);
        ArgumentNullException.ThrowIfNull(interfaces);
        ArgumentNullException.ThrowIfNull(declaredAttributes);
        ArgumentNullException.ThrowIfNull(inheritedAttributes);
        ArgumentNullException.ThrowIfNull(parameterlessOverrides);
        this.type = type;
        this.baseTypes = Array.AsReadOnly(baseTypes.ToArray());
        this.interfaces = Array.AsReadOnly(interfaces.ToArray());
        this.declaredAttributes = Array.AsReadOnly(declaredAttributes.ToArray());
        this.inheritedAttributes = Array.AsReadOnly(inheritedAttributes.ToArray());
        this.parameterlessOverrides = Array.AsReadOnly(parameterlessOverrides.ToArray());
    }

    /// <summary>
    /// Gets the declaration whose metadata is owned by this generation.
    /// </summary>
    public Type type { get; }

    /// <summary>
    /// Gets the ordered inheritance chain, excluding System.Object.
    /// </summary>
    public IReadOnlyList<Type> baseTypes { get; }

    /// <summary>
    /// Gets the complete implemented interface set.
    /// </summary>
    public IReadOnlyList<Type> interfaces { get; }

    /// <summary>
    /// Gets directly declared attribute instances owned by this generation.
    /// </summary>
    public IReadOnlyList<Attribute> declaredAttributes { get; }

    /// <summary>
    /// Gets the effective attribute set used by inherited extension discovery.
    /// </summary>
    public IReadOnlyList<Attribute> inheritedAttributes { get; }

    /// <summary>
    /// Gets effective parameterless overrides without retaining runtime MethodInfo objects.
    /// </summary>
    public IReadOnlyList<(string name, Type declaringBase)> parameterlessOverrides { get; }
}
