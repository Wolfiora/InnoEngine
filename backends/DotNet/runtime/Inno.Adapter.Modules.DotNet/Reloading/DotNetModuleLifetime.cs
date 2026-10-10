using System;
using System.Runtime.Loader;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Reload;

namespace Inno.Adapter.Modules.DotNet;

internal sealed class DotNetModuleLifetime : IModuleLifetime
{
    private AssemblyLoadContext? m_loadContext;
    private readonly string m_shadowDirectory;
    private readonly string m_description;
    private IAssemblyUnloadProbe? m_retirement;

    internal DotNetModuleLifetime(
        AssemblyLoadContext loadContext,
        string shadowDirectory,
        string description
    ) {
        m_loadContext = loadContext;
        m_shadowDirectory = shadowDirectory;
        m_description = description;
    }

    /// <inheritdoc />
    public IAssemblyUnloadProbe BeginRetirement()
    {
        if (m_retirement is not null)
            return m_retirement;
        var reference = new WeakReference(m_loadContext!, trackResurrection: false);
        m_loadContext!.Unload();
        m_loadContext = null;
        return m_retirement = new AssemblyUnloadMonitor(reference, m_shadowDirectory, m_description);
    }
}
