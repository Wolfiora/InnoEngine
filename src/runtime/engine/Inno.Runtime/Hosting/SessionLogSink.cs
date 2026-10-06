using System;
using Inno.Core.Logging;

namespace Inno.Runtime;

internal sealed class SessionLogSink : ILogSink, IDisposable
{
    private readonly LogSessionId m_sessionId;
    private ILogSink? m_sink;

    internal SessionLogSink(
        LogSessionId sessionId,
        ILogSink sink
    ) {
        m_sessionId = sessionId;
        m_sink = sink;
    }

    /// <inheritdoc />
    public void Receive(LogEntry entry)
    {
        if (entry.sessionId == m_sessionId)
            m_sink?.Receive(entry);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (m_sink is IDisposable owner)
            owner.Dispose();
        m_sink = null;
    }
}
