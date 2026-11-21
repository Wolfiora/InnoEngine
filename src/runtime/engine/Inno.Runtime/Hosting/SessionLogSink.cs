using System;
using Inno.Core.Logging;

namespace Inno.Runtime;

internal sealed class SessionLogSink : ILogSink, IDisposable
{
    private readonly LogSessionId m_sessionID;
    private ILogSink? m_sink;

    internal SessionLogSink(
        LogSessionId sessionID,
        ILogSink sink
    ) {
        m_sessionID = sessionID;
        m_sink = sink;
    }

    /// <inheritdoc/>
    public void Receive(LogEntry entry)
    {
        if (entry.sessionId == m_sessionID)
        {
            m_sink?.Receive(entry);
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (m_sink is IDisposable owner)
        {
            owner.Dispose();
        }
        m_sink = null;
    }
}
