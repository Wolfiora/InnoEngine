using Inno.Native.LibraryLoading;

namespace Inno.Native.UI;

/// <summary>
/// Initializes the explicitly deployed module through the generated BGCS API.
/// </summary>
public static unsafe partial class UiNative
{
#if DEBUG
    internal const string C_LIB_NAME = "inno-ui-debug";
#else
    internal const string C_LIB_NAME = "inno-ui-release";
#endif

    static UiNative()
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
