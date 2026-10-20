using Inno.Native.LibraryLoading;

namespace Inno.Native.MiniAudio;

/// <summary>
/// Initializes the explicitly deployed module through the generated BGCS API.
/// </summary>
public static unsafe partial class MiniAudio
{
#if DEBUG
    internal const string LibName = "miniaudio-debug";
#else
    internal const string LibName = "miniaudio-release";
#endif

    static MiniAudio()
    {
#if INNO_STATIC_NATIVE
        FromStatic();
#else
        Load(NativeDllLoader.FindNativeLibrary(LibName));
#endif
    }
}