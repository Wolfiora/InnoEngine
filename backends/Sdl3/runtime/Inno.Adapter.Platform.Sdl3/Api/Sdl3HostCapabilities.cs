namespace Inno.Adapter.Platform.Sdl3;

/// <summary>
/// Declares immutable window and frame scheduling capabilities supplied by the selected host.
/// </summary>
public sealed class Sdl3HostCapabilities
{
    /// <summary>
    /// Captures host capabilities without initializing SDL or creating windows.
    /// </summary>
    /// <param name="multipleWindows">
    /// Whether the host supports more than one concurrently registered window.
    /// </param>
    /// <param name="liveResize">
    /// Whether native window move and resize loops require redraw callbacks.
    /// </param>
    public Sdl3HostCapabilities(
        bool multipleWindows,
        bool liveResize
    ) {
        this.multipleWindows = multipleWindows;
        this.liveResize = liveResize;
    }

    /// <summary>
    /// Gets whether the application can register additional native windows.
    /// </summary>
    public bool multipleWindows { get; }

    /// <summary>
    /// Gets whether the application installs the native live-resize scheduling integration.
    /// </summary>
    public bool liveResize { get; }
}
