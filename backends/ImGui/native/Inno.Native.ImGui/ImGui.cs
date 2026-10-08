using BGCS.Runtime;
using Inno.Native.LibraryLoading;

namespace Inno.Native.ImGui;

/// <summary>
/// Initializes generated imports against the component's dynamic native library.
/// </summary>
public static partial class ImGui
{
#if DEBUG
    private const string C_LIBRARY_NAME = "libcimgui-debug";
#else
    private const string C_LIBRARY_NAME = "libcimgui-release";
#endif

    static ImGui()
    {
        nint handle = NativeDllLoader.LoadNativeDll(C_LIBRARY_NAME, typeof(ImGui).Assembly);
        InitApi(new NativeLibraryContext(handle));
    }
}
