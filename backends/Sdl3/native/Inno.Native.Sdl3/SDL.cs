using Inno.Native.LibraryLoading;

namespace Inno.Native.Sdl3;

/// <summary>
/// Initializes the explicitly deployed module through the generated BGCS API.
/// </summary>
public static unsafe partial class SDL
{
#if DEBUG
    internal const string LibName = "SDL3-debug";
#else
    internal const string LibName = "SDL3-release";
#endif

    static SDL()
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