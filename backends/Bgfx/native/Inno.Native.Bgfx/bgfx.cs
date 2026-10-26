using Inno.Native.LibraryLoading;

namespace Inno.Native.Bgfx;

/// <summary>
/// Initializes the explicitly deployed module through the generated BGCS API.
/// </summary>
public static unsafe partial class bgfx
{
#if DEBUG
    internal const string LibName = "bgfx-shared-lib-debug";
#else
    internal const string LibName = "bgfx-shared-lib-release";
#endif

    static bgfx()
    {
#if INNO_RUNTIME_IMPORTS
        Create();
#elif INNO_STATIC_NATIVE
        FromStatic();
#else
        Load(NativeDllLoader.FindNativeLibrary(LibName));
#endif
    }
}