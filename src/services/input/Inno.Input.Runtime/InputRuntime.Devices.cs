using System;
using System.Collections.Generic;
using Inno.Core.Events;
using Inno.Core.Identity;

namespace Inno.Input.Runtime;

public sealed partial class InputRuntime
{
    private readonly List<Connection> m_connections = [];
    private readonly List<InputDeviceChange> m_changes = [];
    private readonly IdentityAllocator m_identities;
    private readonly EventDispatcher m_events;
    private readonly int m_deviceThread = Environment.CurrentManagedThreadId;
    private bool m_accepting = true;

    /// <inheritdoc/>
    public InputDeviceConnection ConnectDevice(string typeID)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(typeID);
        EnsureDeviceThread();
        ObjectDisposedException.ThrowIf(!m_accepting, this);
        var device = new Connection(this, typeID);
        m_identities.Register(device);
        m_connections.Add(device);
        try
        {
            device.Publish(InputDeviceChangeKind.Connected);
            return device;
        }
        catch
        {
            device.Dispose();
            throw;
        }
    }

    private void EnsureDeviceThread()
    {
        if (Environment.CurrentManagedThreadId != m_deviceThread)
        {
            throw new InvalidOperationException("Device transitions require the input session thread.");
        }
    }

    private sealed class Connection(
        InputRuntime source,
        string typeID
    ) : InputDeviceConnection
    {
        private readonly Dictionary<string, double> m_controls = new(StringComparer.Ordinal);
        private InputRuntime? m_source = source;
        private InputDeviceState? m_cached;

        public override void SetValue(
            string controlID,
            double value
        ) {
            ArgumentException.ThrowIfNullOrWhiteSpace(controlID);
            if (!double.IsFinite(value)) throw new ArgumentOutOfRangeException(nameof(value));
            InputRuntime owner = m_source ?? throw new ObjectDisposedException(nameof(InputDeviceConnection));
            owner.EnsureDeviceThread();
            m_controls[controlID] = value;
            m_cached = null;
            Publish(InputDeviceChangeKind.ValueChanged, controlID, value);
        }

        public override void Dispose()
        {
            if (m_source is not { } owner) return;
            owner.EnsureDeviceThread();
            InputDeviceChange change = MakeChange(InputDeviceChangeKind.Disconnected, null, 0);
            m_source = null;
            owner.m_connections.Remove(this);
            owner.m_identities.Unregister(this);
            owner.PublishDeviceChange(change);
        }

        internal InputDeviceState Capture()
            => m_cached ??= new InputDeviceState(identity.runtimeIdentity!.Value, typeID, m_controls);

        internal void Publish(
            InputDeviceChangeKind kind,
            string? controlID = null,
            double value = 0
        ) => m_source!.PublishDeviceChange(MakeChange(kind, controlID, value));

        private InputDeviceChange MakeChange(
            InputDeviceChangeKind kind,
            string? controlID,
            double value
        ) => new(identity.runtimeIdentity!.Value, typeID, kind, controlID, value);
    }

    private void PublishDeviceChange(InputDeviceChange change)
    {
        m_changes.Add(change);
        m_events.Emit(new InputDeviceEvent(change));
    }
}
