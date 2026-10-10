using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Inno.Extensibility.Modules;

namespace Inno.Runtime;

/// <summary>
/// Supplies an explicit linked host closure without runtime assembly probing or dynamic loading.
/// </summary>
public sealed class StaticAssemblyCatalogSource : IAssemblyCatalogSource
{
    private readonly IReadOnlyList<Assembly> m_assemblies;
    private readonly IReadOnlyList<Assembly> m_sharedAssemblies;
    private readonly HashSet<Assembly> m_framework;
    private readonly HashSet<string> m_frameworkNames;
    private bool m_disposed;

    /// <summary>
    /// Copies the host and framework identities provided by the generated platform composition.
    /// </summary>
    /// <param name="assemblies">
    /// The complete statically linked Inno host closure.
    /// </param>
    /// <param name="frameworkAssemblies">
    /// Framework contracts shared by every contributed module.
    /// </param>
    public StaticAssemblyCatalogSource(
        IReadOnlyList<Assembly> assemblies,
        IReadOnlyList<Assembly> frameworkAssemblies
    ) {
        ArgumentNullException.ThrowIfNull(assemblies);
        ArgumentNullException.ThrowIfNull(frameworkAssemblies);
        if (assemblies.Any(static assembly => assembly is null || assembly.IsCollectible)
            || frameworkAssemblies.Any(static assembly => assembly is null || assembly.IsCollectible))
            throw new ArgumentException("A static catalog requires noncollectible linked assemblies.", nameof(assemblies));
        m_assemblies = Array.AsReadOnly(assemblies.Distinct().ToArray());
        m_framework = frameworkAssemblies.ToHashSet();
        m_frameworkNames = frameworkAssemblies.Select(static assembly => assembly.GetName().Name!)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        m_sharedAssemblies = Array.AsReadOnly(m_assemblies.Concat(frameworkAssemblies).Distinct().ToArray());
    }

    /// <inheritdoc />
    public event Action? changed
    {
        add { ObjectDisposedException.ThrowIf(m_disposed, this); }
        remove { }
    }

    /// <inheritdoc />
    public IReadOnlyList<Assembly> GetAssemblies()
    {
        ObjectDisposedException.ThrowIf(m_disposed, this);
        return m_assemblies;
    }

    /// <inheritdoc />
    public IReadOnlyList<Assembly> GetSharedAssemblies()
    {
        ObjectDisposedException.ThrowIf(m_disposed, this);
        return m_sharedAssemblies;
    }

    /// <inheritdoc />
    public bool IsFrameworkReference(string assemblyName)
    {
        ObjectDisposedException.ThrowIf(m_disposed, this);
        ArgumentException.ThrowIfNullOrWhiteSpace(assemblyName);
        return m_frameworkNames.Contains(assemblyName);
    }
    /// <inheritdoc />
    public bool IsFrameworkAssembly(Assembly assembly)
    {
        ObjectDisposedException.ThrowIf(m_disposed, this);
        ArgumentNullException.ThrowIfNull(assembly);
        return m_framework.Contains(assembly);
    }
    /// <inheritdoc />
    public void Dispose() => m_disposed = true;
}
