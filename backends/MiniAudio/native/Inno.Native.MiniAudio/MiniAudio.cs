using Inno.Native.LibraryLoading;

namespace Inno.Native.MiniAudio;

/// <summary>
/// Initializes the explicitly deployed module through the generated BGCS API.
/// </summary>
public static unsafe partial class MiniAudio
{
#if DEBUG
    internal const string C_LIB_NAME = "miniaudio-debug";
#else
    internal const string C_LIB_NAME = "miniaudio-release";
#endif

    static MiniAudio()
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
