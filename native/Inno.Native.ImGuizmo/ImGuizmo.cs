using BGCS.Runtime;
using Inno.Native.LibraryLoading;

namespace Inno.Native.ImGuizmo;

/// <summary>
/// Initializes generated imports against the component's dynamic native library.
/// </summary>
public static unsafe partial class ImGuizmo
{
#if DEBUG
    private const string C_LIBRARY_NAME = "libcimguizmo-debug";
    private const string C_DEPENDENCY_NAME = "libcimgui-debug";
#else
    private const string C_LIBRARY_NAME = "libcimguizmo-release";
    private const string C_DEPENDENCY_NAME = "libcimgui-release";
#endif

    static ImGuizmo()
    {
        NativeLibraryContext dependency = new(NativeDllLoader.LoadNativeDll(C_DEPENDENCY_NAME, typeof(ImGuizmo).Assembly));
        NativeLibraryContext? api = null;
        try
        {
            api = new NativeLibraryContext(NativeDllLoader.LoadNativeDll(C_LIBRARY_NAME, typeof(ImGuizmo).Assembly));
            InitApi(new ImGuizmoNativeContext(api, dependency));
        }
        catch
        {
            try
            {
                api?.Dispose();
            }
            finally
            {
                dependency.Dispose();
            }
            throw;
        }
    }
}
