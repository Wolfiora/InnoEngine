using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Inno.Extensibility.Modules;

internal sealed class AssemblyModuleEntry
{
    internal required AssemblyModuleHandle handle { get; init; }
    internal required string moduleName { get; init; }
    internal required int generation { get; init; }
    internal required bool externallyOwned { get; init; }
    internal required bool collectible { get; init; }
    internal required AssemblyDomain domain { get; init; }
    internal required AssemblyScope scope { get; init; }
    internal required Assembly[] assemblies { get; init; }
    internal required IReadOnlyDictionary<Assembly, AssemblyScope> assemblyScopes { get; init; }
    internal IReadOnlyList<string> upstreamModuleNames { get; init; } = [];
    internal required ModuleCatalogContribution contribution { get; init; }

    internal AssemblyModuleInfo CreateInfo()
        => new(
            handle,
            moduleName,
            generation,
            collectible,
            externallyOwned,
            domain,
            scope,
            AssemblyModuleStatus.Active,
            assemblies
                .Select(static assembly => assembly.GetName().Name ?? assembly.FullName ?? "Unknown")
                .OrderBy(static name => name, StringComparer.Ordinal)
                .ToArray())
        {
            upstreamModuleNames = upstreamModuleNames
        };
}
