using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Inno.Extensibility.Modules;

namespace Inno.Adapter.Modules.DotNet;

/// <summary>
/// Owns discovery of an explicit desktop authoring closure and its shareable framework contracts.
/// </summary>
public sealed class DotNetAssemblyCatalogSource : IAssemblyCatalogSource
{
    private readonly Assembly[] m_roots;
    private readonly AssemblyLoadContext m_hostLoadContext;
    private readonly HashSet<string> m_trustedPlatformAssemblies = GetTrustedPlatformAssemblyNames();
    private readonly HashSet<Assembly> m_ownedAssemblies = [];
    private bool m_disposed;
    private bool m_discovering;

    /// <summary>
    /// Resolves the referenced Inno closure in the roots' existing load context.
    /// </summary>
    /// <param name="roots">
    /// Explicit composition roots, all belonging to the same externally owned load context.
    /// </param>
    /// <exception cref="ArgumentException">
    /// No roots are supplied or roots belong to different load contexts.
    /// </exception>
    public DotNetAssemblyCatalogSource(params Assembly[] roots)
    {
        ArgumentNullException.ThrowIfNull(roots);
        if (roots.Length == 0)
            throw new ArgumentException("At least one composition root is required.", nameof(roots));
        m_roots = roots.Distinct().ToArray();
        m_hostLoadContext = AssemblyLoadContext.GetLoadContext(m_roots[0])!;
        if (m_roots.Any(assembly =>
                AssemblyLoadContext.GetLoadContext(assembly) != m_hostLoadContext))
            throw new ArgumentException("Composition roots must share one load context.", nameof(roots));
        PreloadInnoHostDependencies();
        AppDomain.CurrentDomain.AssemblyLoad += OnAssemblyLoaded;
    }

    /// <inheritdoc />
    public event Action? changed;

    /// <inheritdoc />
    public IReadOnlyList<Assembly> GetAssemblies()
    {
        ObjectDisposedException.ThrowIf(m_disposed, this);
        lock (m_ownedAssemblies)
            return m_ownedAssemblies.Where(static assembly =>
                assembly.TryGetInnoAssemblyClassification(out AssemblyDomain domain, out _) &&
                domain == AssemblyDomain.InnoInternal).ToArray();
    }

    /// <inheritdoc />
    public IReadOnlyList<Assembly> GetSharedAssemblies()
    {
        ObjectDisposedException.ThrowIf(m_disposed, this);
        lock (m_ownedAssemblies)
            return m_ownedAssemblies.Concat(AppDomain.CurrentDomain.GetAssemblies()
                .Where(IsTrustedFrameworkAssembly)).Distinct().ToArray();
    }

    /// <inheritdoc />
    public bool IsFrameworkReference(string assemblyName) => m_trustedPlatformAssemblies.Contains(assemblyName);

    /// <inheritdoc />
    public bool IsFrameworkAssembly(Assembly assembly) => IsTrustedFrameworkAssembly(assembly);

    /// <inheritdoc />
    public void Dispose()
    {
        if (m_disposed)
            return;
        AppDomain.CurrentDomain.AssemblyLoad -= OnAssemblyLoaded;
        changed = null;
        m_ownedAssemblies.Clear();
        m_disposed = true;
    }

    private void OnAssemblyLoaded(
        object? sender,
        AssemblyLoadEventArgs args
    ) {
        // Discovery remains the explicit composition closure. New assemblies become visible only
        // when referenced by a root; arbitrary assemblies in the process never enter the catalog.
        if (AssemblyLoadContext.GetLoadContext(args.LoadedAssembly) != m_hostLoadContext)
            return;
        int previousCount;
        lock (m_ownedAssemblies)
        {
            if (m_disposed || m_discovering)
                return;
            previousCount = m_ownedAssemblies.Count;
            m_discovering = true;
            try
            {
                PreloadInnoHostDependencies();
            }
            finally
            {
                m_discovering = false;
            }
            if (m_ownedAssemblies.Count == previousCount)
                return;
        }
        changed?.Invoke();
    }

    private void PreloadInnoHostDependencies()
    {
        Assembly[] roots = m_roots;
        if (roots.Length == 0)
            return;

        var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var pending = new Queue<Assembly>();
        foreach (Assembly root in roots)
        {
            string rootName = root.GetName().Name ?? string.Empty;
            if (!string.IsNullOrEmpty(rootName))
                visited.Add(rootName);
            m_ownedAssemblies.Add(root);
            pending.Enqueue(root);
        }
        foreach (AssemblyName dependency in HostDependencyManifest.GetInnoRuntimeAssemblies(roots))
            TryEnqueueHostAssembly(dependency, visited, pending);

        while (pending.Count > 0)
        {
            Assembly assembly = pending.Dequeue();
            foreach (AssemblyName reference in assembly.GetReferencedAssemblies())
                TryEnqueueHostAssembly(reference, visited, pending);
        }
    }
    private void TryEnqueueHostAssembly(
        AssemblyName assemblyName,
        ISet<string> visited,
        Queue<Assembly> pending
    ) {
        string name = assemblyName.Name ?? string.Empty;
        if (!name.StartsWith("Inno.", StringComparison.Ordinal) || !visited.Add(name))
            return;
        try
        {
            Assembly assembly = m_hostLoadContext.LoadFromAssemblyName(assemblyName);
            lock (m_ownedAssemblies)
                m_ownedAssemblies.Add(assembly);
            pending.Enqueue(assembly);
        }
        catch (FileNotFoundException)
        {
            // Optional engine modules can be absent from a host deployment.
        }
    }
    private static HashSet<string> GetTrustedPlatformAssemblyNames()
    {
        string paths = AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") as string ?? string.Empty;
        return paths.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries)
            .Select(Path.GetFileNameWithoutExtension).OfType<string>()
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    private static bool IsTrustedFrameworkAssembly(Assembly assembly)
    {
        if (AssemblyLoadContext.GetLoadContext(assembly) != AssemblyLoadContext.Default)
            return false;
        AssemblyName identity = assembly.GetName();
        string name = identity.Name ?? string.Empty;
        if (!name.StartsWith("System.", StringComparison.Ordinal)
            && !name.StartsWith("Microsoft.Win32.", StringComparison.Ordinal)
            && name is not ("System" or "netstandard" or "mscorlib"))
        {
            return false;
        }
        string token = Convert.ToHexString(identity.GetPublicKeyToken() ?? []);
        return token is "B03F5F7F11D50A3A" or "7CEC85D7BEA7798E" or "CC7B13FFCD2DDD51";
    }
}
