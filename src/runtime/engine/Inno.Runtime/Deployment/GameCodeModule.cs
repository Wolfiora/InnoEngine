using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Extensibility.Modules;

namespace Inno.Runtime;

/// <summary>
/// Freezes the code ownership, ordered assembly identities and dependencies of one logical module.
/// </summary>
public sealed class GameCodeModule
{
    /// <summary>
    /// Copies and validates a module contribution without retaining mutable manifest objects.
    /// </summary>
    /// <param name="name">
    /// The stable module identity.
    /// </param>
    /// <param name="domain">
    /// The ownership domain of this game's code.
    /// </param>
    /// <param name="assemblies">
    /// The exact ordered code identities to activate.
    /// </param>
    /// <param name="dependencies">
    /// The logical modules that precede this contribution.
    /// </param>
    /// <exception cref="System.IO.InvalidDataException">
    /// The module identity, domain, code or dependencies are invalid.
    /// </exception>
    public GameCodeModule(
        string name,
        AssemblyDomain domain,
        IReadOnlyList<GameCodeAssembly> assemblies,
        IReadOnlyList<string> dependencies
    ) {
        ArgumentNullException.ThrowIfNull(assemblies);
        ArgumentNullException.ThrowIfNull(dependencies);
        if (assemblies.Any(static assembly => assembly is null))
            throw new System.IO.InvalidDataException("A code module contains a null assembly identity.");
        var declaration = new GameRuntimeModule
        {
            name = name,
            domain = domain,
            assemblies = assemblies.Select(static assembly => new GameRuntimeAssembly
            {
                name = assembly.name,
                contentFingerprint = assembly.contentFingerprint
            }).ToArray(),
            dependencies = dependencies.ToArray()
        };
        declaration.Validate();
        this.name = name;
        this.domain = domain;
        this.assemblies = Array.AsReadOnly(assemblies.ToArray());
        this.dependencies = Array.AsReadOnly(dependencies.ToArray());
    }

    /// <summary>
    /// Gets the stable logical module identity.
    /// </summary>
    public string name { get; }

    /// <summary>
    /// Gets the ownership domain of this contribution.
    /// </summary>
    public AssemblyDomain domain { get; }

    /// <summary>
    /// Gets the exact ordered code identities.
    /// </summary>
    public IReadOnlyList<GameCodeAssembly> assemblies { get; }

    /// <summary>
    /// Gets the logical modules required before activation.
    /// </summary>
    public IReadOnlyList<string> dependencies { get; }
}
