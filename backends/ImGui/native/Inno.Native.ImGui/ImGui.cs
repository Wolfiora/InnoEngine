using Inno.Native.LibraryLoading;

namespace Inno.Native.ImGui;

/// <summary>
/// Initializes the explicitly deployed module through the generated BGCS API.
/// </summary>
public static unsafe partial class ImGui
{
#if DEBUG
    internal const string C_LIB_NAME = "libcimgui-debug";
#else
    internal const string C_LIB_NAME = "libcimgui-release";
#endif

    static ImGui()
    {
#if INNO_RUNTIME_IMPORTS
        Create();
#elif INNO_STATIC_NATIVE
        FromStatic();
#else
        Load(NativeDllLoader.FindNativeLibrary(C_LIB_NAME));
#endif
    }
}
