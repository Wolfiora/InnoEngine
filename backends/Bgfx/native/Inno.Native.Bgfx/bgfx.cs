using Inno.Native.LibraryLoading;

namespace Inno.Native.Bgfx;

/// <summary>
/// bgfx native bindings loader.
/// </summary>
public static partial class bgfx
{
#if INNO_STATIC_NATIVE
    internal const string LibName = "bgfxRelease";
#elif DEBUG
    internal const string LibName = "bgfx-shared-lib-debug";
#else
    internal const string LibName = "bgfx-shared-lib-release";
#endif
#if !INNO_STATIC_NATIVE
    private const string DLL_NAME = LibName;
    
    static bgfx()
    {
        NativeDllLoader.LoadNativeDll(LibName, typeof(bgfx).Assembly);
    }
#endif
}
