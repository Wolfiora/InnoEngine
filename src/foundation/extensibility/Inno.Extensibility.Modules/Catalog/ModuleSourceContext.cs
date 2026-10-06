using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Inno.Extensibility.Reload;

namespace Inno.Extensibility.Modules;

/// <summary>
/// Supplies a source with the frozen dependencies and retirement boundary of one candidate generation.
/// </summary>
public sealed class ModuleSourceContext
{
    private readonly Action<IAssemblyUnloadProbe> m_trackRetirement;

    internal ModuleSourceContext(
        int generation,
        IAssemblyCatalogSource host,
        IReadOnlyList<ModuleCatalogContribution> upstreamModules,
        IReadOnlyList<ModuleCatalogContribution> activeModules,
        IReadOnlyDictionary<string, ModuleAssemblyDescriptor> plannedAssemblies,
        Action<IAssemblyUnloadProbe> trackRetirement
    ) {
        this.generation = generation;
        this.host = host;
        this.upstreamModules = Array.AsReadOnly(upstreamModules.ToArray());
        this.activeModules = Array.AsReadOnly(activeModules.ToArray());
        this.plannedAssemblies = new ReadOnlyDictionary<string, ModuleAssemblyDescriptor>(
            plannedAssemblies.ToDictionary(static pair => pair.Key, static pair => pair.Value,
                StringComparer.OrdinalIgnoreCase));
        m_trackRetirement = trackRetirement;
    }
    /// <summary>
    /// Gets the candidate generation number.
    /// </summary>
    public int generation { get; }
    /// <summary>
    /// Gets the borrowed host contract source.
    /// </summary>
    public IAssemblyCatalogSource host { get; }
    /// <summary>
    /// Gets the validated direct upstream contributions for this candidate.
    /// </summary>
    public IReadOnlyList<ModuleCatalogContribution> upstreamModules { get; }
    /// <summary>
    /// Gets active contributions used to reject forbidden downstream references.
    /// </summary>
    public IReadOnlyList<ModuleCatalogContribution> activeModules { get; }
    /// <summary>
    /// Gets identities and classifications for the complete candidate closure.
    /// </summary>
    public IReadOnlyDictionary<string, ModuleAssemblyDescriptor> plannedAssemblies { get; }
    /// <summary>
    /// Transfers a failed acquisition's non-owning retirement probe to the shared generation gate.
    /// </summary>
    /// <param name="probe">
    /// The failed candidate's code and storage release probe.
    /// </param>
    public void TrackRetirement(IAssemblyUnloadProbe probe)
    {
        ArgumentNullException.ThrowIfNull(probe);
        m_trackRetirement(probe);
    }
}
