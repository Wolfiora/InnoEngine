using System;
using System.IO;

namespace Inno.Extensibility.Modules;

/// <summary>
/// Configures one isolated module host and its shadow-copy storage.
/// </summary>
public sealed class ModuleHostOptions
{
    /// <summary>
    /// Gets or sets the directory used for isolated assembly generations.
    /// </summary>
    public string cacheDirectory { get; set; } = Path.Combine(
        AppContext.BaseDirectory,
        "AssemblyCache");

    /// <summary>
    /// Gets the host source whose ownership transfers to the module host during construction.
    /// </summary>
    public required IAssemblyCatalogSource catalogSource { get; init; }
}
