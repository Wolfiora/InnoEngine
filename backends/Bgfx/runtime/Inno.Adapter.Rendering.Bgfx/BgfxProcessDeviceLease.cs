using System;

namespace Inno.Adapter.Rendering.Bgfx;

/// <summary>
/// Serializes ownership of BGFX's process-global native runtime without exposing process state through the device API.
/// </summary>
internal sealed class BgfxProcessDeviceLease : IDisposable
{
    private static readonly object S_SYNC = new();
    private static BgfxProcessDeviceLease? s_active;
    private static bool s_singleThreadedRuntimeConsumed;

    private readonly bool m_singleThreaded;
    private bool m_disposed;

    private BgfxProcessDeviceLease(bool singleThreaded)
    {
        m_singleThreaded = singleThreaded;
    }

    internal static BgfxProcessDeviceLease Acquire(bool singleThreaded)
    {
        lock (S_SYNC)
        {
            if (s_active is not null)
                throw new InvalidOperationException("Only one BGFX device may own the native process runtime.");
            if (s_singleThreadedRuntimeConsumed)
            {
                throw new InvalidOperationException(
                    "BGFX cannot be initialized again after this process entered single-threaded mode.");
            }

            var lease = new BgfxProcessDeviceLease(singleThreaded);
            s_active = lease;
            return lease;
        }
    }

    internal void MarkSingleThreadedEntered()
    {
        lock (S_SYNC)
        {
            if (!ReferenceEquals(s_active, this) || m_disposed)
                throw new InvalidOperationException("The BGFX process lease is not active.");
            if (m_singleThreaded)
                s_singleThreadedRuntimeConsumed = true;
        }
    }

    /// <summary>
    /// Releases active ownership after native shutdown; a consumed inline runtime remains unavailable.
    /// </summary>
    public void Dispose()
    {
        lock (S_SYNC)
        {
            if (m_disposed)
                return;
            if (!ReferenceEquals(s_active, this))
                throw new InvalidOperationException("BGFX process leases must be released by their owner.");
            m_disposed = true;
            s_active = null;
        }
    }
}
