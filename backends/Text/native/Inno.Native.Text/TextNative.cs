using Inno.Native.LibraryLoading;

namespace Inno.Native.Text;

/// <summary>
/// Initializes the explicitly deployed module through the generated BGCS API.
/// </summary>
public static unsafe partial class TextNative
{
#if DEBUG
    internal const string C_LIB_NAME = "inno-text-debug";
#else
    internal const string C_LIB_NAME = "inno-text-release";
#endif

    static TextNative()
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
