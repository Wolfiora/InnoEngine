using Inno.Native.LibraryLoading;

namespace Inno.Native.ImGui;

/// <summary>
/// Initializes the explicitly deployed module through the generated BGCS API.
/// </summary>
public static unsafe partial class ImGui
{
#if DEBUG
    internal const string LibName = "libcimgui-debug";
#else
    internal const string LibName = "libcimgui-release";
#endif

    static ImGui()
    {
#if INNO_STATIC_NATIVE
        FromStatic();
#else
        Load(NativeDllLoader.FindNativeLibrary(LibName));
#endif
    }
}