using System;
using System.Threading.Tasks;

namespace Inno.Adapter.Presentation.ImGui;

/// <summary>
/// Presents ImGui draw data through a replaceable renderer while platform input and windows stay shared.
/// </summary>
public interface IPlatformImGuiRenderer : IDisposable
{
    /// <summary>
    /// Gets whether every draw-data consumer applies per-command vertex offsets to 16-bit indices.
    /// </summary>
    bool supportsVertexOffsets { get; }

    /// <summary>
    /// Gets whether detached viewport windows are rendered by this backend.
    /// </summary>
    bool supportsViewports { get; }

    /// <summary>
    /// Consumes current main-window draw data before the next ImGui frame begins.
    /// </summary>
    /// <param name="drawData">
    /// Opaque native ImDrawData pointer valid for the current frame.
    /// </param>
    void RenderMain(nint drawData);

    /// <summary>
    /// Synchronizes the primary drawable after a platform resize.
    /// </summary>
    /// <param name="pixelWidth">
    /// Current drawable width in physical pixels.
    /// </param>
    /// <param name="pixelHeight">
    /// Current drawable height in physical pixels.
    /// </param>
    void SynchronizeMainOutput(
        int pixelWidth,
        int pixelHeight
    );

    /// <summary>
    /// Creates backend presentation state for one detached viewport.
    /// </summary>
    /// <param name="target">
    /// New viewport target.
    /// </param>
    void CreateViewport(PlatformImGuiViewportTarget target);

    /// <summary>
    /// Resizes backend presentation state for one detached viewport.
    /// </summary>
    /// <param name="target">
    /// Updated viewport target.
    /// </param>
    void ResizeViewport(PlatformImGuiViewportTarget target);

    /// <summary>
    /// Consumes draw data for one detached viewport.
    /// </summary>
    /// <param name="target">
    /// Viewport destination.
    /// </param>
    /// <param name="drawData">
    /// Opaque native ImDrawData pointer valid for the current frame.
    /// </param>
    void RenderViewport(
        PlatformImGuiViewportTarget target,
        nint drawData
    );

    /// <summary>
    /// Marks one detached viewport ready for presentation.
    /// </summary>
    /// <param name="target">
    /// Viewport destination.
    /// </param>
    void PresentViewport(PlatformImGuiViewportTarget target);

    /// <summary>
    /// Removes the viewport from future rendering and schedules release of its borrowed native window.
    /// </summary>
    /// <param name="target">
    /// The viewport whose original owner keeps its native window alive during retirement.
    /// </param>
    /// <returns>
    /// A task completed only when the renderer no longer uses the native window. This is not a GPU fence.
    /// </returns>
    /// <remarks>
    /// Native callbacks may request retirement but must not wait or advance rendering.
    /// </remarks>
    Task RetireViewportAsync(PlatformImGuiViewportTarget target);

    /// <summary>
    /// Drains requested retirements at a closed-frame owner-thread point outside native UI callbacks.
    /// </summary>
    /// <remarks>
    /// Failure or timeout retains all unfinished viewport and renderer owners.
    /// </remarks>
    void DrainViewportRetirements();
}
