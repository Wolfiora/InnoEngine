using System;
using BGCS.Runtime;

namespace Inno.Native.ImGuizmo;

internal sealed class ImGuizmoNativeContext : INativeContext
{
    private readonly NativeLibraryContext m_api;
    private readonly NativeLibraryContext m_dependency;

    internal ImGuizmoNativeContext(
        NativeLibraryContext api,
        NativeLibraryContext dependency
    ) {
        m_api = api;
        m_dependency = dependency;
    }

    /// <inheritdoc />
    public nint GetProcAddress(string procName) => m_api.GetProcAddress(procName);

    /// <inheritdoc />
    public bool TryGetProcAddress(
        string procName,
        out nint procAddress
    ) => m_api.TryGetProcAddress(procName, out procAddress);

    /// <inheritdoc />
    public bool IsExtensionSupported(string extensionName) => m_api.IsExtensionSupported(extensionName);

    /// <summary>
    /// Releases the API before its explicitly loaded dependency; repeated release is safe.
    /// </summary>
    public void Dispose()
    {
        try
        {
            m_api.Dispose();
        }
        finally
        {
            m_dependency.Dispose();
        }
    }
}
