using System;
using System.IO;
using System.Linq;
using Inno.Core.Serialization;
using Inno.Extensibility.Modules;

namespace Inno.Runtime;

/// <summary>
/// Describes one dependency-ordered managed module in a frozen Player generation.
/// </summary>
[GenerateSerializationConverter]
public sealed class GameRuntimeModule : ISerializable
{
    /// <summary>
    /// Gets or sets the stable module name used by the managed module host.
    /// </summary>
    [SerializableProperty]
    public string name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the ownership domain declared by the deployed module.
    /// </summary>
    [SerializableProperty]
    public AssemblyDomain domain { get; set; }

    /// <summary>
    /// Gets or sets the exact ordered code identities contributed by this module.
    /// </summary>
    [SerializableProperty]
    public GameRuntimeAssembly[] assemblies { get; set; } = [];

    /// <summary>
    /// Gets or sets stable module names that must be active before this module.
    /// </summary>
    [SerializableProperty]
    public string[] dependencies { get; set; } = [];

    /// <summary>
    /// Validates module identity, ownership, file names, and dependency declarations.
    /// </summary>
    /// <exception cref="InvalidDataException">
    /// Thrown when the module cannot be activated as part of a frozen Player generation.
    /// </exception>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidDataException("A runtime module requires a stable name.");
        if (domain is not (AssemblyDomain.InnoPlugin or AssemblyDomain.InnoScripting))
            throw new InvalidDataException($"Runtime module '{name}' has an invalid ownership domain.");
        if (assemblies is null || dependencies is null || assemblies.Length == 0)
            throw new InvalidDataException($"Runtime module '{name}' requires code and dependency collections.");
        foreach (GameRuntimeAssembly assembly in assemblies)
        {
            if (assembly is null)
                throw new InvalidDataException($"Runtime module '{name}' contains a null code identity.");
            assembly.Validate();
        }
        if (assemblies.Select(static assembly => assembly.name).Distinct(StringComparer.OrdinalIgnoreCase).Count()
            != assemblies.Length)
            throw new InvalidDataException($"Runtime module '{name}' contains duplicate code identities.");
        if (dependencies.Any(string.IsNullOrWhiteSpace)
            || dependencies.Contains(name, StringComparer.Ordinal)
            || dependencies.Distinct(StringComparer.Ordinal).Count() != dependencies.Length)
        {
            throw new InvalidDataException($"Runtime module '{name}' contains invalid dependencies.");
        }
    }

}
