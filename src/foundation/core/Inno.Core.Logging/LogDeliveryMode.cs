namespace Inno.Core.Logging;

/// <summary>
/// Selects the delivery policy independently of the operating system or execution host.
/// </summary>
public enum LogDeliveryMode
{
    /// <summary>
    /// Delivers queued entries through a dedicated background worker.
    /// </summary>
    Background,

    /// <summary>
    /// Delivers entries synchronously on the producer's thread.
    /// </summary>
    Inline
}
