using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Core.Diagnostics;
using Inno.Core.Execution;
using Inno.References;

namespace Inno.Audio.Runtime;

internal sealed class AudioContentOwner : IDisposable
{
    private readonly Dictionary<Guid, EmitterRecord> m_emitters = [];
    private readonly HashSet<string> m_providerDiagnosticIDs = new(StringComparer.Ordinal);
    private readonly IAudioDevice m_device;
    private readonly AudioVoiceOwner m_voices;
    private readonly IDiagnosticReporter m_diagnostics;
    private readonly Func<ContentReadScope>? m_contentScopeProvider;
    private readonly int m_capacity;
    private AudioListenerHandle m_listener;
    private Guid m_listenerID;
    private Guid m_listenerDiagnosticID;

    internal AudioContentOwner(
        IAudioDevice device,
        AudioVoiceOwner voices,
        IDiagnosticReporter diagnostics,
        Func<ContentReadScope>? contentScopeProvider,
        int capacity
    ) {
        m_device = device;
        m_voices = voices;
        m_diagnostics = diagnostics;
        m_contentScopeProvider = contentScopeProvider;
        m_capacity = capacity;
        this.statistics = new AudioContentStatistics(capacity, 0, 0);
    }

    /// <summary>
    /// Releases the listener and clears neutral synchronization state before the device can retire.
    /// </summary>
    public void Dispose()
    {
        if (m_listener.isValid)
        {
            _ = m_device.DestroyListener(m_listener);
        }
        m_listener = default;
        m_listenerID = Guid.Empty;
        m_emitters.Clear();
        foreach (string id in m_providerDiagnosticIDs)
        {
            m_diagnostics.Resolve("AUDIO_CONTENT_PROVIDER_FAILED", id);
        }
        m_providerDiagnosticIDs.Clear();
        if (m_listenerDiagnosticID != Guid.Empty)
        {
            m_diagnostics.Resolve("AUDIO_LISTENER_PRIORITY_TIE", m_listenerDiagnosticID.ToString("D"));
        }
        m_listenerDiagnosticID = Guid.Empty;
    }

    internal AudioContentStatistics statistics { get; private set; }

    internal void ClearEmitters() => m_emitters.Clear();

    internal void Update(
        AudioExtensionRegistry.ProviderGeneration providers,
        float deltaTime
    ) {
        ContentReadScope content;
        try
        {
            content = m_contentScopeProvider?.Invoke() ?? ContentReadScope.empty;
            m_diagnostics.Resolve("AUDIO_CONTENT_SCOPE_FAILED", "AudioRuntime");
        }
        catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
        {
            throw;
        }
        catch (Exception exception)
        {
            Publish(
                "AUDIO_CONTENT_SCOPE_FAILED",
                $"The host audio content scope failed; an empty scope is used for this update: {exception.Message}",
                DiagnosticSeverity.Error,
                "AudioRuntime"
            );
            content = ContentReadScope.empty;
        }
        using ContentReadScope contentScope = content;
        List<AudioEmitterSnapshot> emitters = [];
        List<AudioListenerSnapshot> listeners = [];
        var ids = new HashSet<Guid>();
        int rejectedProviders = 0;
        foreach (string retiredID in m_providerDiagnosticIDs
            .Where(id => !providers.providers.Any(entry => entry.id == id))
            .ToArray()
        ) {
            m_diagnostics.Resolve("AUDIO_CONTENT_PROVIDER_FAILED", retiredID);
            m_providerDiagnosticIDs.Remove(retiredID);
        }
        foreach (AudioExtensionRegistry.ProviderEntry entry in providers.providers)
        {
            using var context = new AudioContentProviderContext(content, deltaTime, m_capacity - ids.Count);
            try
            {
                entry.provider.Submit(context);
                IReadOnlyList<AudioEmitterSnapshot> contributionEmitters = context.emitters;
                IReadOnlyList<AudioListenerSnapshot> contributionListeners = context.listeners;
                if (contributionEmitters.Any(emitter => ids.Contains(emitter.id))
                    || contributionListeners.Any(listener => ids.Contains(listener.id))
                ) {
                    throw new ArgumentException("The contribution repeats an identity already accepted from another provider.");
                }
                foreach (AudioEmitterSnapshot emitter in contributionEmitters)
                {
                    ids.Add(emitter.id);
                    emitters.Add(emitter);
                }
                foreach (AudioListenerSnapshot listener in contributionListeners)
                {
                    ids.Add(listener.id);
                    listeners.Add(listener);
                }
                m_diagnostics.Resolve("AUDIO_CONTENT_PROVIDER_FAILED", entry.id);
                m_providerDiagnosticIDs.Remove(entry.id);
            }
            catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
            {
                throw;
            }
            catch (Exception exception)
            {
                rejectedProviders++;
                m_providerDiagnosticIDs.Add(entry.id);
                Publish(
                    "AUDIO_CONTENT_PROVIDER_FAILED",
                    $"Audio content provider '{entry.id}' was isolated for this update: {exception.Message}",
                    DiagnosticSeverity.Error,
                    entry.id
                );
            }
        }
        statistics = new AudioContentStatistics(m_capacity, ids.Count, rejectedProviders);
        SynchronizeEmitters(emitters);
        SynchronizeListener(listeners);
    }

    private void SynchronizeEmitters(IReadOnlyList<AudioEmitterSnapshot> snapshots)
    {
        var seen = new HashSet<Guid>();
        foreach (AudioEmitterSnapshot snapshot in snapshots)
        {
            seen.Add(snapshot.id);
            if (!snapshot.shouldPlay)
            {
                if (m_emitters.Remove(snapshot.id, out EmitterRecord? inactive))
                {
                    _ = m_voices.Stop(inactive.voice);
                }
                continue;
            }
            if (m_emitters.TryGetValue(snapshot.id, out EmitterRecord? current))
            {
                if (current.clipID != snapshot.clip.identity.persistentID
                    || current.contentVersion != snapshot.clip.contentVersion
                    || current.playbackRevision != snapshot.playbackRevision
                ) {
                    _ = m_voices.Stop(current.voice);
                    current = new EmitterRecord(
                        snapshot.clip,
                        snapshot.playbackRevision,
                        m_voices.Play(snapshot.clip, snapshot.options)
                    );
                    m_emitters[snapshot.id] = current;
                }
                else
                {
                    _ = m_voices.SetVoiceParameters(
                        current.voice,
                        new AudioVoiceParameters(
                            snapshot.options.volume,
                            snapshot.options.pitch,
                            snapshot.options.pan,
                            snapshot.options.spatial
                        )
                    );
                }
                continue;
            }
            m_emitters.Add(
                snapshot.id,
                new EmitterRecord(snapshot.clip, snapshot.playbackRevision, m_voices.Play(snapshot.clip, snapshot.options))
            );
        }
        foreach (Guid id in m_emitters.Keys.Where(id => !seen.Contains(id)).ToArray())
        {
            _ = m_voices.Stop(m_emitters[id].voice);
            m_emitters.Remove(id);
        }
    }

    private void SynchronizeListener(IReadOnlyList<AudioListenerSnapshot> snapshots)
    {
        if (m_listenerDiagnosticID != Guid.Empty)
        {
            m_diagnostics.Resolve("AUDIO_LISTENER_PRIORITY_TIE", m_listenerDiagnosticID.ToString("D"));
            m_listenerDiagnosticID = Guid.Empty;
        }
        AudioListenerSnapshot[] active = snapshots
            .Where(static listener => listener.active)
            .OrderByDescending(static listener => listener.priority)
            .ThenBy(static listener => listener.id)
            .ToArray();
        if (active.Length == 0)
        {
            if (m_listener.isValid)
            {
                _ = m_device.DestroyListener(m_listener);
            }
            m_listener = default;
            m_listenerID = Guid.Empty;
            return;
        }
        AudioListenerSnapshot selected = active[0];
        if (active.Length > 1 && active[1].priority == selected.priority)
        {
            m_listenerDiagnosticID = selected.id;
            Publish(
                "AUDIO_LISTENER_PRIORITY_TIE",
                "Multiple active listeners share the highest priority; stable identity selected the winner.",
                DiagnosticSeverity.Warning,
                selected.id.ToString("D")
            );
        }
        if (!m_listener.isValid || m_listenerID != selected.id)
        {
            if (m_listener.isValid)
            {
                _ = m_device.DestroyListener(m_listener);
            }
            m_listener = m_device.CreateListener(selected.state);
            m_listenerID = selected.id;
            return;
        }
        _ = m_device.SetListener(m_listener, selected.state);
    }

    private void Publish(
        string code,
        string message,
        DiagnosticSeverity severity,
        string? source
    ) => m_diagnostics.Publish(new Diagnostic(code, message, severity, source));

    private sealed class EmitterRecord(
        AudioClipAsset clip,
        ulong playbackRevision,
        AudioVoiceHandle voice
    ) {
        internal Guid clipID { get; } = clip.identity.persistentID;
        internal long contentVersion { get; } = clip.contentVersion;
        internal ulong playbackRevision { get; } = playbackRevision;
        internal AudioVoiceHandle voice { get; } = voice;
    }
}
