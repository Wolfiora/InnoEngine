using Inno.Core.Execution;
using Inno.Rendering;
using Inno.Rendering.Assets;
using System;
using System.Threading;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Exposes current rendering configuration and immutable device state.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("6310eecc-3492-572c-a2f7-e29ec16e65b9")]
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

[Inno.Extensibility.Types.StableTypeId("1af0f9ea-f6cf-544c-a2c5-0c3c306c6780")]
internal sealed class GraphicsSettingsState
{
    private readonly object m_sync = new();
    private RenderPipelineAsset? m_defaultPipeline;
    private RenderFrameStatistics? m_frameStatistics;

    internal GraphicsSettingsState(GraphicsCapabilities capabilities)
    {
        this.capabilities = capabilities ?? throw new ArgumentNullException(nameof(capabilities));
    }

    internal GraphicsCapabilities capabilities { get; }

    internal RenderPipelineAsset? defaultPipeline
    {
        get
        {
            lock (m_sync)
                return m_defaultPipeline;
        }
        set
        {
            lock (m_sync)
                m_defaultPipeline = value;
        }
    }

    internal RenderFrameStatistics? frameStatistics
    {
        get
        {
            lock (m_sync)
                return m_frameStatistics;
        }
        set
        {
            lock (m_sync)
                m_frameStatistics = value;
        }
    }

    internal void Clear()
    {
        lock (m_sync)
        {
            m_defaultPipeline = null;
            m_frameStatistics = null;
        }
    }
}

[Inno.Extensibility.Types.StableTypeId("0f145d08-b261-54bf-92aa-5ee6605d819e")]
internal static class GraphicsSettingsExecutionContext
{
    private static readonly ExecutionSlot<GraphicsSettingsState> S_CURRENT = new("state");

    internal static GraphicsSettingsState current
        => currentOrNull
            ?? throw new InvalidOperationException(
                "No rendering runtime is bound to the current execution context.");

    internal static GraphicsSettingsState? currentOrNull => S_CURRENT.TryGet(out GraphicsSettingsState? state) ? state : null;

    internal static IDisposable Enter(GraphicsSettingsState state)
    {
        ArgumentNullException.ThrowIfNull(state);
        return S_CURRENT.Enter(state);
    }

}
