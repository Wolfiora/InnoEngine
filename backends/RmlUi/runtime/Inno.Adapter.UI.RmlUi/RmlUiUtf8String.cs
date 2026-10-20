using System.Runtime.InteropServices;

namespace Inno.Adapter.UI.RmlUi;

internal readonly unsafe ref struct RmlUiUtf8String
{
    private readonly nint m_memory;

    internal RmlUiUtf8String(string value) => m_memory = Marshal.StringToCoTaskMemUTF8(value);

    internal byte* pointer => (byte*)m_memory;

    internal void Dispose() => Marshal.FreeCoTaskMem(m_memory);
}
