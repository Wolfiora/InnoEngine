using System;

namespace Inno.Extensibility.Types;

/// <summary>
/// Identifies the exact declaration and generation whose extension factory failed.
/// </summary>
/// <remarks>
/// Identity fields contain only neutral values. The inner exception preserves the original failure
/// and can retain plugin code; diagnostic owners must publish its text instead of retaining this
/// exception beyond candidate retirement.
/// </remarks>
public sealed class ExtensionActivationException : InvalidOperationException
{
    /// <summary>
    /// Creates an activation failure without retaining a type or catalog in its identity fields.
    /// </summary>
    /// <param name="typeId">
    /// The persistent declaration identity in the rejected candidate.
    /// </param>
    /// <param name="typeName">
    /// The fully qualified implementation name.
    /// </param>
    /// <param name="assemblyName">
    /// The simple assembly name that supplied the implementation.
    /// </param>
    /// <param name="generation">
    /// The type catalog generation used by the failed factory.
    /// </param>
    /// <param name="innerException">
    /// The original constructor, dependency resolution, or factory failure.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The declaration identity or either name is empty.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The original failure is null.
    /// </exception>
    public ExtensionActivationException(
        Guid typeId,
        string typeName,
        string assemblyName,
        long generation,
        Exception innerException
    ) : base($"Extension '{typeName}' ({typeId:D}) from '{assemblyName}' failed to activate in generation {generation}: {innerException?.Message}", innerException)
    {
        if (typeId == Guid.Empty)
            throw new ArgumentException("An extension declaration identity is required.", nameof(typeId));
        ArgumentException.ThrowIfNullOrWhiteSpace(typeName);
        ArgumentException.ThrowIfNullOrWhiteSpace(assemblyName);
        ArgumentNullException.ThrowIfNull(innerException);
        this.typeId = typeId;
        this.typeName = typeName;
        this.assemblyName = assemblyName;
        this.generation = generation;
    }

    /// <summary>
    /// Gets the persistent declaration identity, independent of any live instance.
    /// </summary>
    public Guid typeId { get; }

    /// <summary>
    /// Gets the rejected implementation's fully qualified name.
    /// </summary>
    public string typeName { get; }

    /// <summary>
    /// Gets the supplying simple assembly name for source correlation by the owning compiler.
    /// </summary>
    public string assemblyName { get; }

    /// <summary>
    /// Gets the candidate type catalog generation; it does not grant a generation lease.
    /// </summary>
    public long generation { get; }
}
