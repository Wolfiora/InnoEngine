using System;
using System.IO;
using Inno.Core.Logging;

namespace Inno.Runtime;

/// <summary>
/// Collects application-level runtime services before creating an <see cref="EngineHost"/>.
/// </summary>
public sealed class EngineHostBuilder
{
    private LogDeliveryMode m_logDeliveryMode = LogDeliveryMode.Background;
    private TimeSpan m_retirementTimeout = TimeSpan.FromSeconds(30);
    private string m_metadataCacheDirectory = Path.Combine(
        Path.GetTempPath(),
        "InnoEngine",
        "RuntimeMetadata");

    /// <summary>
    /// Selects the writable cache used for assembly shadow copies and immutable type metadata.
    /// </summary>
    /// <param name="directory">
    /// The absolute or current-directory-relative cache directory.
    /// </param>
    /// <returns>
    /// This builder for fluent configuration.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="directory"/> is empty.
    /// </exception>
    public EngineHostBuilder UseMetadataCache(string directory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(directory);
        m_metadataCacheDirectory = Path.GetFullPath(directory);
        return this;
    }

    /// <summary>
    /// Sets the maximum owner-thread drain duration before retirement faults the host.
    /// </summary>
    /// <param name="timeout">
    /// A positive deadline shared by failed startup, session stop and host shutdown.
    /// </param>
    /// <returns>
    /// This builder for fluent configuration.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The timeout is not positive.
    /// </exception>
    public EngineHostBuilder UseRetirementTimeout(TimeSpan timeout)
    {
        if (timeout <= TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(timeout));
        m_retirementTimeout = timeout;
        return this;
    }

    /// <summary>
    /// Creates an application host and acquires its immutable metadata services.
    /// </summary>
    /// <returns>
    /// A host owned by the caller.
    /// </returns>
    public EngineHost Build() => new(m_metadataCacheDirectory, m_retirementTimeout, m_logDeliveryMode);

    /// <summary>
    /// Selects the host router's delivery policy, which also schedules every session's file sink.
    /// </summary>
    /// <param name="deliveryMode">
    /// Background worker or inline producer-thread delivery.
    /// </param>
    /// <returns>
    /// This builder for fluent host composition.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The policy is undefined.
    /// </exception>
    public EngineHostBuilder UseLogDelivery(LogDeliveryMode deliveryMode)
    {
        if (!Enum.IsDefined(deliveryMode))
            throw new ArgumentOutOfRangeException(nameof(deliveryMode));
        m_logDeliveryMode = deliveryMode;
        return this;
    }
}
