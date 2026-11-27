using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Core.Execution;

namespace Inno.Audio.Runtime;

internal sealed class AudioMixerOwner : IDisposable
{
    private readonly IAudioDevice m_device;
    private readonly AudioExtensionRegistry m_extensions;
    private readonly Action<Exception> m_retirementFailed;
    private readonly Action<Action> m_retire;
    private readonly List<Exception> m_retirementFailures = [];
    private readonly Dictionary<AudioBusID, AudioBusHandle> m_buses = [];
    private readonly Dictionary<AudioBusID, BusControlState> m_busControls = [];
    private readonly List<Dictionary<AudioBusID, AudioBusHandle>> m_retiredBuses = [];
    private readonly Dictionary<AudioBusHandle, bool> m_retiredMasterPauses = [];
    private AudioMixer m_activeMixer;
    private bool m_suspended;

    internal AudioMixerOwner(
        IAudioDevice device,
        AudioExtensionRegistry extensions,
        Action<Exception> retirementFailed,
        Action<Action> retire
    ) : this(device, new AudioMixerBuilder().Build(), null, false, extensions, retirementFailed, retire) {}

    private AudioMixerOwner(
        IAudioDevice device,
        AudioMixer mixer,
        IReadOnlyDictionary<AudioBusID, BusControlState>? controls,
        bool suspended,
        AudioExtensionRegistry extensions,
        Action<Exception> retirementFailed,
        Action<Action> retire
    ) {
        m_device = device;
        m_extensions = extensions;
        m_retirementFailed = retirementFailed;
        m_retire = retire;
        m_activeMixer = mixer;
        m_suspended = suspended;
        foreach ((AudioBusID id, AudioBusHandle handle) in CreateBusSet(device, mixer, controls))
        {
            m_buses.Add(id, handle);
        }
        foreach (AudioBusDefinition bus in mixer.buses)
        {
            BusControlState state = controls is not null && controls.TryGetValue(bus.id, out BusControlState? current)
                                    ? current
                                    : new BusControlState(bus.volume, bus.muted, paused: false);
            m_busControls.Add(bus.id, new BusControlState(state.volume, state.muted, state.paused));
        }
    }

    /// <summary>
    /// Retires the active graph and all retained graph generations after their voices have stopped.
    /// </summary>
    public void Dispose()
    {
        try
        {
            DestroyBusSet(m_device, m_buses);
        }
        catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
        {
            throw;
        }
        catch (Exception error)
        {
            m_retirementFailures.Add(error);
        }
        while (m_retiredBuses.Count > 0)
        {
            try
            {
                DestroyBusSet(m_device, m_retiredBuses[^1]);
            }
            catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
            {
                throw;
            }
            catch (Exception error)
            {
                m_retirementFailures.Add(error);
            }
            m_retiredBuses.RemoveAt(m_retiredBuses.Count - 1);
        }
        m_busControls.Clear();
        m_retiredMasterPauses.Clear();
        if (m_retirementFailures.Count == 0)
        {
            return;
        }
        Exception[] failures = m_retirementFailures.ToArray();
        m_retirementFailures.Clear();
        throw new AggregateException("Audio mixer retirement failed after every graph was attempted.", failures);
    }

    internal AudioMixerOwner PrepareReplacement(IAudioDevice device)
        => new(device, m_activeMixer, m_busControls, m_suspended, m_extensions, m_retirementFailed, m_retire);

    internal bool TryGetBus(
        AudioBusID id,
        out AudioBusHandle handle
    ) => m_buses.TryGetValue(id, out handle);

    internal bool SetBusVolume(
        AudioBusID bus,
        float volume
    ) {
        if (volume < 0f || !float.IsFinite(volume)
            || !m_buses.TryGetValue(bus, out AudioBusHandle handle)
            || !m_device.SetBusVolume(handle, volume)
        ) {
            return false;
        }
        m_busControls[bus].volume = volume;
        return true;
    }

    internal bool SetBusMuted(
        AudioBusID bus,
        bool muted
    ) {
        if (!m_buses.TryGetValue(bus, out AudioBusHandle handle) || !m_device.SetBusMuted(handle, muted))
        {
            return false;
        }
        m_busControls[bus].muted = muted;
        return true;
    }

    internal bool SetBusPaused(
        AudioBusID bus,
        bool paused
    ) {
        if (!m_buses.TryGetValue(bus, out AudioBusHandle handle)
            || !m_device.SetBusPaused(handle, paused || bus == AudioBusID.master && m_suspended)
        ) {
            return false;
        }
        m_busControls[bus].paused = paused;
        return true;
    }

    internal void SetSuspended(bool suspended)
    {
        if (suspended == m_suspended)
        {
            return;
        }
        var masters = new List<KeyValuePair<AudioBusHandle, bool>>(m_retiredMasterPauses)
        {
            new KeyValuePair<AudioBusHandle, bool>(m_buses[AudioBusID.master], m_busControls[AudioBusID.master].paused)
        };
        int applied = 0;
        try
        {
            foreach ((AudioBusHandle handle, bool paused) in masters)
            {
                if (!m_device.SetBusPaused(handle, paused || suspended))
                {
                    throw new InvalidOperationException("The audio backend rejected application suspension.");
                }
                applied++;
            }
        }
        catch (Exception failure)
        {
            List<Exception> failures = [failure];
            for (int index = applied - 1; index >= 0; index--)
            {
                try
                {
                    KeyValuePair<AudioBusHandle, bool> master = masters[index];
                    if (!m_device.SetBusPaused(master.Key, master.Value || m_suspended))
                    {
                        throw new InvalidOperationException("The audio backend rejected suspension rollback.");
                    }
                }
                catch (Exception rollback)
                {
                    failures.Add(rollback);
                }
            }
            if (failures.Count > 1)
            {
                var rollbackFailure = new AggregateException("Audio suspension and rollback failed.", failures);
                m_retirementFailed(rollbackFailure);
                throw rollbackFailure;
            }
            throw;
        }
        m_suspended = suspended;
    }

    internal void Install(
        AudioMixer mixer,
        IEnumerable<AudioBusHandle> usedBuses
    ) {
        CollectRetiredBuses(usedBuses);
        IReadOnlyDictionary<AudioBusID, AudioBusHandle> candidate = CreateBusSet(m_device, mixer, null);
        if (m_buses.Count > 0)
        {
            m_retiredBuses.Add(new Dictionary<AudioBusID, AudioBusHandle>(m_buses));
            m_retiredMasterPauses.Add(m_buses[AudioBusID.master], m_busControls[AudioBusID.master].paused);
        }
        m_buses.Clear();
        foreach ((AudioBusID id, AudioBusHandle handle) in candidate)
        {
            m_buses.Add(id, handle);
        }
        m_busControls.Clear();
        foreach (AudioBusDefinition bus in mixer.buses)
        {
            m_busControls.Add(bus.id, new BusControlState(bus.volume, bus.muted, paused: false));
        }
        m_activeMixer = mixer;
    }

    internal void CollectRetiredBuses(IEnumerable<AudioBusHandle> usedBuses)
    {
        HashSet<AudioBusHandle> retained = usedBuses.ToHashSet();
        for (int index = m_retiredBuses.Count - 1; index >= 0; index--)
        {
            Dictionary<AudioBusID, AudioBusHandle> retired = m_retiredBuses[index];
            if (retired.Values.Any(retained.Contains))
            {
                continue;
            }
            try
            {
                AudioBusHandle master = retired[AudioBusID.master];
                DestroyBusSet(m_device, retired);
                m_retiredMasterPauses.Remove(master);
                m_retiredBuses.RemoveAt(index);
            }
            catch (Exception exception)
            {
                m_retirementFailed(exception);
                throw;
            }
        }
    }

    private IReadOnlyDictionary<AudioBusID, AudioBusHandle> CreateBusSet(
        IAudioDevice device,
        AudioMixer mixer,
        IReadOnlyDictionary<AudioBusID, BusControlState>? controls
    ) {
        var candidate = new Dictionary<AudioBusID, AudioBusHandle>();
        try
        {
            foreach (AudioBusDefinition bus in mixer.buses)
            {
                AudioBusHandle parent = bus.parent is AudioBusID parentID
                                        ? candidate[parentID]
                                        : default;
                AudioBusHandle handle = device.CreateBus(bus.id, parent);
                if (!handle.isValid)
                {
                    throw new InvalidOperationException($"The backend rejected audio bus '{bus.id}'.");
                }
                candidate.Add(bus.id, handle);
                BusControlState state = controls is not null && controls.TryGetValue(bus.id, out BusControlState? current)
                                        ? current
                                        : new BusControlState(bus.volume, bus.muted, paused: false);
                if (!device.SetBusVolume(handle, state.volume)
                    || !device.SetBusMuted(handle, state.muted)
                    || (state.paused || bus.id == AudioBusID.master && m_suspended)
                       && !device.SetBusPaused(handle, paused: true)
                ) {
                    throw new InvalidOperationException($"The backend rejected parameters for audio bus '{bus.id}'.");
                }
                foreach (AudioProcessorConfiguration processor in bus.processors)
                {
                    if (!AddProcessor(device, handle, processor))
                    {
                        throw new InvalidOperationException($"The backend rejected processor '{processor.id}'.");
                    }
                }
            }
            return candidate;
        }
        catch (Exception failure)
        {
            try
            {
                m_retire(() => DestroyBusSet(device, candidate));
            }
            catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
            {
                throw;
            }
            catch (Exception cleanup)
            {
                m_retirementFailed(cleanup);
                throw new AggregateException("Audio mixer preparation and rollback failed.", failure, cleanup);
            }
            throw;
        }
    }

    private bool AddProcessor(
        IAudioDevice device,
        AudioBusHandle bus,
        AudioProcessorConfiguration configuration
    ) {
        AudioBlockProcessor? processor = m_extensions.BindProcessor(configuration.id);
        if (processor is null) return device.AddBusProcessor(bus, configuration);
        try
        {
            if (device is IAudioBlockProcessorDevice buffered && buffered.AddBlockProcessor(bus, configuration, processor))
            {
                return true;
            }
            processor.Dispose();
            return false;
        }
        catch
        {
            processor.Dispose();
            throw;
        }
    }

    private static void DestroyBusSet(
        IAudioDevice device,
        Dictionary<AudioBusID, AudioBusHandle> buses
    ) {
        List<Exception> failures = [];
        foreach ((AudioBusID id, AudioBusHandle bus) in buses.Reverse().ToArray())
        {
            try
            {
                if (!device.DestroyBus(bus))
                {
                    throw new InvalidOperationException("The backend refused audio bus retirement.");
                }
            }
            catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
            {
                throw;
            }
            catch (Exception exception)
            {
                failures.Add(exception);
            }
            buses.Remove(id);
        }
        if (failures.Count > 0)
        {
            throw new AggregateException("Audio bus retirement failed after every bus was attempted.", failures);
        }
    }

    private sealed class BusControlState(
        float volume,
        bool muted,
        bool paused
    ) {
        internal float volume { get; set; } = volume;
        internal bool muted { get; set; } = muted;
        internal bool paused { get; set; } = paused;
    }
}
