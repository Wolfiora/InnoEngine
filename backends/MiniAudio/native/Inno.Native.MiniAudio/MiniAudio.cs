#if !INNO_STATIC_NATIVE
using BGCS.Runtime;
using Inno.Native.LibraryLoading;
#endif

#if !INNO_STATIC_NATIVE

namespace Inno.Native.MiniAudio;

/// <summary>
/// Initializes generated imports against the component's dynamic native library.
/// </summary>
public static unsafe partial class MiniAudio
{
#if DEBUG
    private const string C_LIBRARY_NAME = "miniaudio-debug";
#else
    private const string C_LIBRARY_NAME = "miniaudio-release";
#endif

    static MiniAudio()
    {
        nint handle = NativeDllLoader.LoadNativeDll(C_LIBRARY_NAME, typeof(MiniAudio).Assembly);
        InitApi(new NativeLibraryContext(handle));
    }
}
#endif
