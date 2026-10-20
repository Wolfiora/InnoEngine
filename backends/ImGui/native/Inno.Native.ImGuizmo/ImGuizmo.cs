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
        var dependency = new DynamicModuleProvider(NativeDllLoader.FindNativeLibrary(C_DEPENDENCY_NAME));
        DynamicModuleProvider api;
        try
        {
            api = new DynamicModuleProvider(NativeDllLoader.FindNativeLibrary(C_LIBRARY_NAME));
        }
        catch
        {
            dependency.Dispose();
            throw;
        }
        FromProvider(new ImGuizmoModuleProvider(api, dependency));
    }
}
