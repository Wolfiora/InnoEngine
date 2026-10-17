using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Build.Managed;

/// <summary>
/// Owns an immutable collection of deployment providers selected explicitly by the build composition.
/// </summary>
public sealed class ManagedDeploymentCatalog
{
    private readonly IReadOnlyDictionary<ManagedDeploymentId, IManagedDeploymentCompiler> m_compilers;

    /// <summary>
    /// Copies providers and rejects ambiguous identities before any publication starts.
    /// </summary>
    /// <param name="compilers">
    /// The explicit provider implementations owned by the composition root.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The collection is empty, contains null entries or has duplicate or invalid identities.
    /// </exception>
    public ManagedDeploymentCatalog(IReadOnlyList<IManagedDeploymentCompiler> compilers)
    {
        ArgumentNullException.ThrowIfNull(compilers);
        if (compilers.Count == 0 || compilers.Any(static compiler => compiler is null
            || string.IsNullOrWhiteSpace(compiler.id.value))
            || compilers.Select(static compiler => compiler.id).Distinct().Count() != compilers.Count)
            throw new ArgumentException("Managed providers require unique valid identities.", nameof(compilers));
        m_compilers = compilers.ToDictionary(static compiler => compiler.id);
        availableDeployments = Array.AsReadOnly(compilers.Select(static compiler => compiler.id)
            .OrderBy(static id => id.value, StringComparer.Ordinal).ToArray());
    }

    /// <summary>
    /// Gets the immutable provider identities in stable order.
    /// </summary>
    public IReadOnlyList<ManagedDeploymentId> availableDeployments { get; }

    /// <summary>
    /// Returns the registered deployment identities that support one managed target.
    /// </summary>
    /// <param name="runtimeIdentifier">
    /// The managed toolchain target selected by the platform composition.
    /// </param>
    /// <returns>
    /// A new immutable list in stable identity order; unsupported targets produce an empty list.
    /// </returns>
    public IReadOnlyList<ManagedDeploymentId> GetSupportedDeployments(string runtimeIdentifier)
        => Array.AsReadOnly(availableDeployments.Where(id =>
            m_compilers[id].capabilities.runtimeIdentifiers.Contains(runtimeIdentifier, StringComparer.Ordinal)).ToArray());

    /// <summary>
    /// Resolves a provider and verifies target support before staging begins.
    /// </summary>
    /// <param name="deployment">
    /// The stable provider identity selected by the build profile.
    /// </param>
    /// <param name="runtimeIdentifier">
    /// The exact managed target required by the platform composition.
    /// </param>
    /// <returns>
    /// The provider that supports the requested target; missing capabilities throw explicitly.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// No provider is registered, or the selected provider cannot publish the requested target.
    /// </exception>
    public IManagedDeploymentCompiler Resolve(
        ManagedDeploymentId deployment,
        string runtimeIdentifier
    ) {
        if (!m_compilers.TryGetValue(deployment, out IManagedDeploymentCompiler? compiler))
            throw new InvalidOperationException($"Managed deployment '{deployment}' is not registered.");
        if (!compiler.capabilities.runtimeIdentifiers.Contains(runtimeIdentifier, StringComparer.Ordinal))
            throw new InvalidOperationException($"Managed deployment '{deployment}' does not support '{runtimeIdentifier}'.");
        return compiler;
    }
}
