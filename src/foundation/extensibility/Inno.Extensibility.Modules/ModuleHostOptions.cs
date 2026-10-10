namespace Inno.Extensibility.Modules;

/// <summary>
/// Selects the isolated host catalog without prescribing storage for contributed module sources.
/// </summary>
public sealed class ModuleHostOptions
{
    /// <summary>
    /// Gets the host source whose ownership transfers to the module host during construction.
    /// </summary>
    public required IAssemblyCatalogSource catalogSource { get; init; }
}
