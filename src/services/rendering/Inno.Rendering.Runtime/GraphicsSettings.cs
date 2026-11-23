using Inno.Extensibility.Types;
using Inno.Rendering.Assets;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Exposes current rendering configuration and immutable device state.
/// </summary>
[StableTypeID("6310eecc-3492-572c-a2f7-e29ec16e65b9")]
public static class GraphicsSettings
{
    /// <summary>
    /// Gets current device capabilities, or <see langword="null"/> before device initialization.
    /// </summary>
    public static GraphicsCapabilities? capabilities => GraphicsSettingsExecutionContext.currentOrNull?.capabilities;

    /// <summary>
    /// Gets or sets the project default pipeline used by requests without an override.
    /// </summary>
    public static RenderPipelineAsset? defaultPipeline
    {
        get => GraphicsSettingsExecutionContext.currentOrNull?.defaultPipeline;
        set => GraphicsSettingsExecutionContext.current.defaultPipeline = value;
    }

    /// <summary>
    /// Gets statistics for the last completed frame, or <see langword="null"/> before the first frame.
    /// </summary>
    public static RenderFrameStatistics? frameStatistics => GraphicsSettingsExecutionContext.currentOrNull?.frameStatistics;
}
