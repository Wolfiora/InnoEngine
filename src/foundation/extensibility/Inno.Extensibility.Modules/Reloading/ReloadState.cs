namespace Inno.Extensibility.Modules;

internal sealed class ReloadState
{
    internal required AssemblyModuleEntry?[] previousModules { get; init; }
    internal required AssemblyModuleEntry[] removedModules { get; init; }
    internal required AssemblyModuleEntry[] candidateModules { get; init; }
    internal required AssemblyCatalogSnapshot previousCatalog { get; init; }
    internal required AssemblyCatalogSnapshot candidateCatalog { get; init; }
    internal required AssemblyCatalogRefreshSet refresh { get; init; }
    internal bool activated { get; set; }
    internal bool finished { get; set; }
}
