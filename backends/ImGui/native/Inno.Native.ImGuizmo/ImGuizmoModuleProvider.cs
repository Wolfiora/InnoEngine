using System;
using BGCS.Runtime;

namespace Inno.Native.ImGuizmo;

internal sealed class ImGuizmoModuleProvider : INativeModuleProvider
{
    private readonly object m_gate = new();
    private readonly INativeModuleProvider m_api;
    private readonly INativeModuleProvider m_dependency;
    private bool m_closing;

    internal ImGuizmoModuleProvider(
        INativeModuleProvider api,
        INativeModuleProvider dependency
    ) {
        m_api = api;
        m_dependency = dependency;
    }

    /// <inheritdoc />
    public bool TryGetExport(
        string name,
        out nint address
    ) {
        lock (m_gate)
        {
            ObjectDisposedException.ThrowIf(m_closing, this);
            return m_api.TryGetExport(name, out address);
        }
    }

    /// <summary>
    /// Releases the API before its dependency, retaining the dependency if API unload fails.
    /// </summary>
    public void Dispose()
    {
        lock (m_gate)
        {
            if (m_closing)
                return;
            m_closing = true;
            // A failed API unload is terminal; its dependency must remain retained for process lifetime.
            m_api.Dispose();
            m_dependency.Dispose();
        }
    }
}
