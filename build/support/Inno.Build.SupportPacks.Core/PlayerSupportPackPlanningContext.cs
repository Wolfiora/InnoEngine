using System;

namespace Inno.Build.SupportPacks;

/// <summary>
/// Supplies read-only discovery inputs before a publication directory exists.
/// </summary>
public sealed class PlayerSupportPackPlanningContext
{
    /// <summary>
    /// Captures the checkout and host from which project-scoped tools are resolved.
    /// </summary>
    /// <param name="engineRoot">
    /// The complete checkout to inspect without modifying publication output.
    /// </param>
    /// <param name="dotnetHost">
    /// The explicitly selected managed host executable.
    /// </param>
    /// <param name="nativeDependencies">
    /// An exact dependency selection; null permits local source preparation.
    /// </param>
    /// <exception cref="ArgumentException">
    /// A required input is blank.
    /// </exception>
    public PlayerSupportPackPlanningContext(
        string engineRoot,
        string dotnetHost,
        string? nativeDependencies = null
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(engineRoot);
        ArgumentException.ThrowIfNullOrWhiteSpace(dotnetHost);
        this.engineRoot = engineRoot;
        this.dotnetHost = dotnetHost;
        this.nativeDependencies = nativeDependencies;
    }

    /// <summary>
    /// Gets the borrowed source checkout used for preflight.
    /// </summary>
    public string engineRoot { get; }

    /// <summary>
    /// Gets the explicit host from which the source freezes its SDK selection.
    /// </summary>
    public string dotnetHost { get; }

    /// <summary>
    /// Gets the explicit dependency selection, or null for source production.
    /// </summary>
    public string? nativeDependencies { get; }
}
