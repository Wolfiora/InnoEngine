#if !INNO_STATIC_NATIVE
using BGCS.Runtime;
using Inno.Native.LibraryLoading;

namespace Inno.Native.UI;

/// <summary>
/// Initializes generated imports against the component's dynamic native library.
/// </summary>
public static unsafe partial class UiNative
{
#if DEBUG
    private const string C_LIBRARY_NAME = "inno-ui-debug";
#else
    private const string C_LIBRARY_NAME = "inno-ui-release";
#endif

    static UiNative()
    {
        nint handle = NativeDllLoader.LoadNativeDll(C_LIBRARY_NAME, typeof(UiNative).Assembly);
        InitApi(new NativeLibraryContext(handle));
    }
}
#endif
