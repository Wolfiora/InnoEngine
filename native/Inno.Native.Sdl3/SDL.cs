#if !INNO_STATIC_NATIVE
using BGCS.Runtime;
using Inno.Native.LibraryLoading;

namespace Inno.Native.Sdl3;

/// <summary>
/// Initializes generated imports against the component's dynamic native library.
/// </summary>
public static unsafe partial class SDL
{
#if DEBUG
    private const string C_LIBRARY_NAME = "SDL3-debug";
#else
    private const string C_LIBRARY_NAME = "SDL3-release";
#endif

    static SDL()
    {
        NativeDllLoader.EnsureNativeDll(C_LIBRARY_NAME);
        nint handle = NativeDllLoader.LoadNativeDll(C_LIBRARY_NAME);
        InitApi(new NativeLibraryContext(handle));
    }
}
#endif
