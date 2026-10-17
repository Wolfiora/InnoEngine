using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Inno.Core.Collections;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Reload;

namespace Inno.Adapter.Modules.DotNet;

/// <summary>
/// Describes one independently reloadable managed assembly module.
/// </summary>
public sealed class DotNetModuleSource : IModuleSource
{
    /// <summary>
    /// Gets the absolute host-selected root owned by this dynamic source for shadow-copy generations.
    /// </summary>
    public required string artifactRootDirectory { get; init; }

    /// <summary>
    /// Gets or sets the stable logical module name.
    /// </summary>
    public required string moduleName { get; init; }

    /// <summary>
    /// Gets or sets the path of the module's primary managed assembly.
    /// </summary>
    public required string mainAssemblyPath { get; init; }

    /// <summary>
    /// Gets or sets additional managed assemblies loaded into the same context.
    /// </summary>
    public IReadOnlyList<string> preloadAssemblyPaths { get; init; } = Array.Empty<string>();

    /// <summary>
    /// Gets stable module names whose exported assemblies may satisfy this module's managed dependencies.
    /// </summary>
    /// <remarks>
    /// Dependencies are explicit for every module. An empty collection declares that the module has no
    /// upstream module dependencies; the host never infers dependencies from process-wide active state.
    /// </remarks>
    public IReadOnlyList<string> upstreamModuleNames { get; init; } = Array.Empty<string>();

    /// <summary>
    /// Gets or sets whether the module load context supports cooperative unloading.
    /// </summary>
    public bool collectible { get; init; } = true;

    /// <summary>
    /// Gets or sets the ownership domain for every assembly in this module.
    /// </summary>
    public AssemblyDomain domain { get; init; } = AssemblyDomain.InnoPlugin;

    /// <summary>
    /// Gets or sets the dependency scope for assemblies without a more specific internal descriptor.
    /// </summary>
    public AssemblyScope scope { get; init; } = AssemblyScope.Runtime;

    /// <summary>
    /// Gets per-assembly scope overrides keyed by managed assembly simple name.
    /// </summary>
    /// <remarks>
    /// This is used when one plugin generation contains both runtime and editor-only assemblies.
    /// Names not present in the map use <see cref="scope"/>.
    /// </remarks>
    public IReadOnlyDictionary<string, AssemblyScope> assemblyScopes { get; init; } =
        new Dictionary<string, AssemblyScope>(StringComparer.OrdinalIgnoreCase);

    /// <inheritdoc />
    public IReadOnlyList<string> GetAssemblyNames()
    {
        var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (string path in new[] { mainAssemblyPath }.Concat(preloadAssemblyPaths))
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                throw new FileNotFoundException("A module assembly does not exist.", path);
            string name = AssemblyName.GetAssemblyName(path).Name
                ?? throw new InvalidDataException($"Assembly '{path}' has no simple name.");
            if (!names.Add(name))
                throw new InvalidDataException($"Module contains duplicate assembly name '{name}'.");
        }
        return names.OrderBy(static name => name, StringComparer.Ordinal).ToArray();
    }

    private bool HasRequestedClassification(
        Assembly assembly,
        AssemblyScope requestedScope
    ) => assembly.GetInnoAssemblyDomain() == domain && assembly.GetInnoAssemblyScope() == requestedScope;

    /// <inheritdoc />
    public ModuleCatalogContribution Prepare(ModuleSourceContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentException.ThrowIfNullOrWhiteSpace(artifactRootDirectory);
        if (!Path.IsPathFullyQualified(artifactRootDirectory))
            throw new ArgumentException("Dynamic module storage requires an absolute adapter-owned root.", nameof(artifactRootDirectory));

        DotNetModuleSource request = this;
        _ = GetAssemblyNames();
        string generationDirectory = Path.Combine(
            artifactRootDirectory,
            SanitizePathSegment(request.moduleName),
            context.generation.ToString(System.Globalization.CultureInfo.InvariantCulture) + "-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(generationDirectory);

        string[] sourcePaths = new[] { request.mainAssemblyPath }
            .Concat(request.preloadAssemblyPaths)
            .Select(Path.GetFullPath)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
        var shadowPathsByName = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var explicitShadowPaths = new List<string>(sourcePaths.Length);
        foreach (string sourcePath in sourcePaths)
        {
            string assemblyName = AssemblyName.GetAssemblyName(sourcePath).Name
                ?? throw new InvalidOperationException($"Assembly '{sourcePath}' has no simple name.");
            if (shadowPathsByName.ContainsKey(assemblyName))
                throw new InvalidOperationException($"Module contains duplicate assembly name '{assemblyName}'.");
            string shadowPath = CopyAssemblyArtifacts(sourcePath, generationDirectory, assemblyName);
            shadowPathsByName.Add(assemblyName, shadowPath);
            explicitShadowPaths.Add(shadowPath);
        }

        string mainSourcePath = Path.GetFullPath(request.mainAssemblyPath);
        string mainShadowPath = explicitShadowPaths[sourcePaths
            .Select((
                path,
                index
            ) => (path, index))
            .First(pair => string.Equals(pair.path, mainSourcePath, StringComparison.OrdinalIgnoreCase)).index];
        IReadOnlyDictionary<string, Assembly> sharedAssemblies = BuildSharedAssemblies(
            request,
            context,
            shadowPathsByName.Keys);
        var loadContext = new ModuleLoadContext(
            $"{request.moduleName}#{context.generation}",
            mainShadowPath,
            request.collectible,
            sharedAssemblies,
            shadowPathsByName.Values);

        try
        {
            var assemblies = new List<Assembly>(explicitShadowPaths.Count)
            {
                loadContext.LoadFromAssemblyPath(mainShadowPath)
            };
            foreach (string shadowPath in explicitShadowPaths)
            {
                string name = AssemblyName.GetAssemblyName(shadowPath).Name ?? string.Empty;
                if (assemblies.Any(assembly => string.Equals(
                        assembly.GetName().Name,
                        name,
                        StringComparison.OrdinalIgnoreCase)))
                    continue;
                assemblies.Add(loadContext.LoadFromAssemblyPath(shadowPath));
            }

            Assembly[] loadedAssemblies = assemblies.Distinct().ToArray();
            var loadedScopes = new Dictionary<Assembly, AssemblyScope>(ReferenceEqualityComparer.Instance);
            foreach (Assembly assembly in loadedAssemblies)
            {
                string simpleName = assembly.GetName().Name ?? string.Empty;
                AssemblyScope assemblyScope = request.assemblyScopes.GetValueOrDefault(simpleName, request.scope);
                if (request.domain == AssemblyDomain.InnoScripting &&
                    (!HasRequestedClassification(assembly, assemblyScope)))
                {
                    throw new InvalidDataException(
                        $"Script assembly '{simpleName}' does not declare its requested domain and scope metadata.");
                }
                loadedScopes.Add(assembly, assemblyScope);
            }
            ValidateLoadedModule(
                request,
                context,
                loadedAssemblies,
                loadedScopes,
                sharedAssemblies,
                context.upstreamModules,
                context.plannedAssemblies);

            return new ModuleCatalogContribution(
                moduleName, domain, scope, loadedAssemblies, loadedScopes,
                new DotNetModuleLifetime(loadContext, generationDirectory,
                    $"{moduleName} ({domain}/{scope}, generation {context.generation})"));
        }
        catch
        {
            if (request.collectible)
            {
                var reference = new WeakReference(loadContext, trackResurrection: false);
                loadContext.Unload();
                context.TrackRetirement(new AssemblyUnloadMonitor(reference, generationDirectory));
                    }
            throw;
        }
    
    }
    private static IReadOnlyDictionary<string, Assembly> BuildSharedAssemblies(
        DotNetModuleSource request,
        ModuleSourceContext context,
        IEnumerable<string> ownedNames
    ) {
        var result = context.host.GetSharedAssemblies()
            .GroupBy(static assembly => assembly.GetName().Name!, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(static group => group.Key, static group => group.First(), StringComparer.OrdinalIgnoreCase);
        var owned = new HashSet<string>(ownedNames, StringComparer.OrdinalIgnoreCase);
        foreach (string ownedName in owned)
        {
            if (result.ContainsKey(ownedName))
            {
                throw new InvalidDataException(
                    $"Module assembly '{ownedName}' duplicates an assembly already shared by the host context.");
            }
        }

        foreach (ModuleCatalogContribution upstream in context.upstreamModules)
        {
            foreach (Assembly assembly in upstream.assemblies)
            {
                if (request.scope == AssemblyScope.Runtime &&
                    upstream.assemblyScopes[assembly] == AssemblyScope.Editor)
                {
                    continue;
                }
                string name = assembly.GetName().Name ?? string.Empty;
                if (owned.Contains(name) || !result.TryAdd(name, assembly))
                    throw new InvalidDataException($"Reload graph contains duplicate managed assembly name '{name}'.");
            }
        }
        return result;
    }
    private static void ValidateLoadedModule(
        DotNetModuleSource request,
        ModuleSourceContext context,
        IReadOnlyList<Assembly> assemblies,
        IReadOnlyDictionary<Assembly, AssemblyScope> assemblyScopes,
        IReadOnlyDictionary<string, Assembly> sharedAssemblies,
        IReadOnlyList<ModuleCatalogContribution> upstreamModules,
        IReadOnlyDictionary<string, ModuleAssemblyDescriptor> plannedAssemblies
    ) {
        var ownByName = assemblies.ToDictionary(
            static assembly => assembly.GetName().Name ?? string.Empty,
            StringComparer.OrdinalIgnoreCase);
        ValidateOwnedDependencyGraph(ownByName);
        var forbiddenDownstreamNames = context.activeModules
            .Where(module => request.domain == AssemblyDomain.InnoPlugin
                ? module.domain == AssemblyDomain.InnoScripting
                : request.scope == AssemblyScope.Runtime && module.scope == AssemblyScope.Editor)
            .SelectMany(static module => module.assemblies)
            .Select(static assembly => assembly.GetName().Name ?? string.Empty)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var upstreamByName = upstreamModules
            .SelectMany(module => module.assemblies.Select(assembly => (module, assembly)))
            .ToDictionary(
                static pair => pair.assembly.GetName().Name ?? string.Empty,
                static pair => pair,
                StringComparer.OrdinalIgnoreCase);

        foreach (Assembly assembly in assemblies)
        {
            AssemblyScope sourceScope = assemblyScopes[assembly];
            foreach (AssemblyName reference in assembly.GetReferencedAssemblies())
            {
                string name = reference.Name ?? string.Empty;
                if (ownByName.TryGetValue(name, out Assembly? ownedDependency))
                {
                    if (sourceScope == AssemblyScope.Runtime &&
                        assemblyScopes[ownedDependency] == AssemblyScope.Editor)
                    {
                        throw new InvalidDataException(
                            $"Runtime assembly '{assembly.GetName().Name}' cannot reference editor assembly '{name}'.");
                    }
                    continue;
                }
                if (forbiddenDownstreamNames.Contains(name))
                {
                    throw new InvalidDataException(
                        $"Assembly '{assembly.GetName().Name}' has a forbidden downstream reference to '{name}'.");
                }
                if (upstreamByName.TryGetValue(name, out var upstream))
                {
                    if (sourceScope == AssemblyScope.Runtime &&
                        upstream.module.assemblyScopes[upstream.assembly] == AssemblyScope.Editor)
                    {
                        throw new InvalidDataException(
                            $"Runtime assembly '{assembly.GetName().Name}' cannot reference editor assembly '{name}'.");
                    }
                    continue;
                }
                if (plannedAssemblies.TryGetValue(name, out ModuleAssemblyDescriptor planned))
                {
                    if (request.domain == AssemblyDomain.InnoPlugin &&
                        planned.domain == AssemblyDomain.InnoScripting)
                    {
                        throw new InvalidDataException(
                            $"Plugin assembly '{assembly.GetName().Name}' cannot reference project script assembly '{name}'.");
                    }
                    if (sourceScope == AssemblyScope.Runtime && planned.scope == AssemblyScope.Editor)
                    {
                        throw new InvalidDataException(
                            $"Runtime assembly '{assembly.GetName().Name}' cannot reference editor assembly '{name}'.");
                    }
                    throw new InvalidDataException(
                        $"Assembly '{assembly.GetName().Name}' has an unavailable downstream reference to '{name}'.");
                }
                if (context.host.IsFrameworkReference(name)
                    || (sharedAssemblies.TryGetValue(name, out Assembly? platformAssembly)
                        && context.host.IsFrameworkAssembly(platformAssembly)))
                    continue;
                if (sharedAssemblies.TryGetValue(name, out Assembly? sharedAssembly))
                {
                    if (sharedAssembly.GetInnoAssemblyDomain() != AssemblyDomain.InnoInternal)
                    {
                        throw new InvalidDataException(
                            $"Assembly '{assembly.GetName().Name}' can only share BCL or InnoInternal contracts, not '{name}'.");
                    }
                    if (sourceScope == AssemblyScope.Runtime && sharedAssembly.GetInnoAssemblyScope() == AssemblyScope.Editor)
                    {
                        throw new InvalidDataException(
                            $"Runtime assembly '{assembly.GetName().Name}' cannot reference editor contract '{name}'.");
                    }
                    continue;
                }
                throw new InvalidDataException(
                    $"Assembly '{assembly.GetName().Name}' references unavailable assembly '{name}'. " +
                    "Include the dependency in its module or load an InnoInternal contract in the host.");
            }
        }
    }
    private static void ValidateOwnedDependencyGraph(IReadOnlyDictionary<string, Assembly> assemblies)
    {
        var graph = new DependencyGraph<string>(
            StringComparer.OrdinalIgnoreCase,
            StringComparer.Ordinal);
        foreach (string name in assemblies.Keys)
            graph.AddNode(name);
        foreach ((string name, Assembly assembly) in assemblies)
        {
            foreach (AssemblyName reference in assembly.GetReferencedAssemblies())
            {
                if (reference.Name is string dependency && assemblies.ContainsKey(dependency))
                    graph.AddDependency(name, dependency);
            }
        }
        if (graph.TryFindCycle(out IReadOnlyList<string> cycle))
        {
            throw new InvalidDataException(
                $"Module assembly reference cycle: {string.Join(" -> ", cycle)}.");
        }
    }
    private static string CopyAssemblyArtifacts(
        string sourcePath,
        string destinationDirectory,
        string assemblyName
    ) {
        string destinationPath = Path.Combine(destinationDirectory, assemblyName + ".dll");
        File.Copy(sourcePath, destinationPath, overwrite: true);
        string sourcePdb = Path.ChangeExtension(sourcePath, ".pdb");
        if (File.Exists(sourcePdb))
            File.Copy(sourcePdb, Path.Combine(destinationDirectory, Path.GetFileName(sourcePdb)), overwrite: true);
        string sourceDeps = Path.ChangeExtension(sourcePath, ".deps.json");
        if (File.Exists(sourceDeps))
            File.Copy(sourceDeps, Path.Combine(destinationDirectory, Path.GetFileName(sourceDeps)), overwrite: true);
        return destinationPath;
    }
    private static string SanitizePathSegment(string value)
    {
        char[] invalid = Path.GetInvalidFileNameChars();
        return new string(value.Select(character => invalid.Contains(character) ? '_' : character).ToArray());
    }
}
