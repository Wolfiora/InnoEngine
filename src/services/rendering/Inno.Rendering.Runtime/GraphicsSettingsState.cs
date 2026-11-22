using System;
using Inno.Extensibility.Types;
using Inno.Rendering.Assets;

namespace Inno.Rendering.Runtime;

[StableTypeID("1af0f9ea-f6cf-544c-a2c5-0c3c306c6780")]
internal sealed class GraphicsSettingsState
{
    private readonly object m_sync = new();
    private RenderPipelineAsset? m_defaultPipeline;
    private RenderFrameStatistics? m_frameStatistics;

    internal GraphicsSettingsState(GraphicsCapabilities capabilities)
    {
        ArgumentNullException.ThrowIfNull(capabilities);
        this.capabilities = capabilities;
    }

    internal GraphicsCapabilities capabilities { get; }

    internal RenderPipelineAsset? defaultPipeline
    {
        get
        {
            lock (m_sync)
            {
                return m_defaultPipeline;
            }
        }
        set
        {
            lock (m_sync)
            {
                m_defaultPipeline = value;
            }
        }
    }

    internal RenderFrameStatistics? frameStatistics
    {
        get
        {
            lock (m_sync)
            {
                return m_frameStatistics;
            }
        }
        set
        {
            lock (m_sync)
            {
                m_frameStatistics = value;
            }
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
