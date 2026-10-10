#if !INNO_STATIC_NATIVE
using BGCS.Runtime;
using Inno.Native.LibraryLoading;

namespace Inno.Native.Text;

/// <summary>
/// Initializes generated imports against the component's dynamic native library.
/// </summary>
public static unsafe partial class TextNative
{
#if DEBUG
    private const string C_LIBRARY_NAME = "inno-text-debug";
#else
    private const string C_LIBRARY_NAME = "inno-text-release";
#endif

    static TextNative()
    {
        nint handle = NativeDllLoader.LoadNativeDll(C_LIBRARY_NAME, typeof(TextNative).Assembly);
        InitApi(new NativeLibraryContext(handle));
    }
}
#endif
