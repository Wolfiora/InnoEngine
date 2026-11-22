using Inno.Adapter.Platform;

namespace Inno.Adapter.Presentation.ImGui;

/// <summary>
/// Describes one detached ImGui viewport without exposing SDL or graphics-backend handles.
/// </summary>
public sealed class PlatformImGuiViewportTarget
{
    internal PlatformImGuiViewportTarget(
        uint viewportID,
        uint windowID,
        PlatformNativeHandles nativeHandles,
        int width,
        int height
    ) {
        this.viewportID = viewportID;
        this.windowID = windowID;
        this.nativeHandles = nativeHandles;
        this.width = width;
        this.height = height;
    }

    /// <summary>
    /// Gets the stable Dear ImGui viewport identity.
    /// </summary>
    public uint viewportID { get; }

    /// <summary>
    /// Gets the platform window identity.
    /// </summary>
    public uint windowID { get; }

    /// <summary>
    /// Gets backend-neutral native-window integration handles.
    /// </summary>
    public PlatformNativeHandles nativeHandles { get; }

    /// <summary>
    /// Gets the current framebuffer width.
    /// </summary>
    public int width { get; internal set; }

    /// <summary>
    /// Gets the current framebuffer height.
    /// </summary>
    public int height { get; internal set; }
}
