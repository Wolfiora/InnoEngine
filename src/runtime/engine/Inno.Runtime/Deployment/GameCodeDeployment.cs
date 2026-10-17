using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Inno.Runtime;

/// <summary>
/// Owns a validated, immutable and dependency-ordered logical game code closure.
/// </summary>
public sealed class GameCodeDeployment
{
    /// <summary>
    /// Copies an explicit code closure and verifies unique ownership and dependency ordering.
    /// </summary>
    /// <param name="modules">
    /// Ordered immutable module contributions; an empty closure describes an engine-only composition.
    /// </param>
    /// <exception cref="InvalidDataException">
    /// A module is null or duplicated, an assembly has multiple owners, or an upstream module is absent.
    /// </exception>
    public GameCodeDeployment(IReadOnlyList<GameCodeModule> modules)
    {
        ArgumentNullException.ThrowIfNull(modules);
        var accepted = new HashSet<string>(StringComparer.Ordinal);
        var assemblies = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (GameCodeModule module in modules)
        {
            if (module is null || !accepted.Add(module.name))
                throw new InvalidDataException("A code deployment contains a null or duplicate module.");
            foreach (string dependency in module.dependencies)
                if (!accepted.Contains(dependency))
                    throw new InvalidDataException($"Code module '{module.name}' precedes dependency '{dependency}'.");
            foreach (GameCodeAssembly assembly in module.assemblies)
                if (!assemblies.Add(assembly.name))
                    throw new InvalidDataException($"Code assembly '{assembly.name}' has multiple module owners.");
        }
        this.modules = Array.AsReadOnly(modules.ToArray());
    }

    /// <summary>
    /// Gets the immutable dependency-ordered code closure.
    /// </summary>
    public IReadOnlyList<GameCodeModule> modules { get; }

    /// <summary>
    /// Freezes validated manifest declarations without retaining their mutable arrays or entries.
    /// </summary>
    /// <param name="modules">
    /// Current-format runtime manifest declarations.
    /// </param>
    /// <returns>
    /// An immutable logical deployment; invalid declarations fail before activation.
    /// </returns>
    /// <exception cref="InvalidDataException">
    /// A declaration or its ownership and dependencies are invalid.
    /// </exception>
    public static GameCodeDeployment FromManifest(IReadOnlyList<GameRuntimeModule> modules)
    {
        ArgumentNullException.ThrowIfNull(modules);
        return new GameCodeDeployment(modules.Select(static module =>
        {
            if (module is null)
                throw new InvalidDataException("A code deployment contains a null module declaration.");
            module.Validate();
            return new GameCodeModule(module.name, module.domain,
                module.assemblies.Select(static assembly => new GameCodeAssembly(
                    assembly.name, assembly.contentFingerprint)).ToArray(), module.dependencies);
        }).ToArray());
    }

    /// <summary>
    /// Rejects a manifest that differs from the code closure linked by the build.
    /// </summary>
    /// <param name="expected">
    /// The independently frozen deployment to compare with this closure.
    /// </param>
    /// <exception cref="InvalidDataException">
    /// Module ordering, ownership, dependencies, code names or fingerprints differ.
    /// </exception>
    public void ValidateMatches(GameCodeDeployment expected)
    {
        ArgumentNullException.ThrowIfNull(expected);
        if (modules.Count != expected.modules.Count)
            throw new InvalidDataException("The deployed code module count differs from the linked composition.");
        for (int index = 0; index < modules.Count; index++)
        {
            GameCodeModule actual = modules[index];
            GameCodeModule linked = expected.modules[index];
            if (actual.name != linked.name || actual.domain != linked.domain
                || !actual.dependencies.SequenceEqual(linked.dependencies, StringComparer.Ordinal)
                || !actual.assemblies.SequenceEqual(linked.assemblies))
                throw new InvalidDataException($"Code module '{actual.name}' differs from the linked composition.");
        }
    }
}
