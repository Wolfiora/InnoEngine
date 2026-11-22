using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Xunit;
using Inno.Adapter.Modules.DotNet;
using Inno.Assets;
using Inno.Core.Diagnostics;
using Inno.Core.Events;
using Inno.Core.Execution;
using Inno.Core.Identity;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;

namespace Inno.Audio.Runtime.Tests;

public abstract class AudioRuntimeFixture : IDisposable
{
    protected readonly FakeArtifactLookup m_artifacts = new();
    protected readonly EventDispatcher m_events = new();
    private readonly string m_root;
    protected readonly ModuleHost m_modules;
    protected readonly TypeCatalog m_types;

    protected AudioRuntimeFixture()
    {
        m_root = Path.Combine(Path.GetTempPath(), "InnoAudioRuntimeTests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(m_root);
        m_modules = new ModuleHost(new ModuleHostOptions
        {
            catalogSource = new DotNetAssemblyCatalogSource(typeof(AudioRuntimeFixture).Assembly)
        });
        _ = typeof(TestMixerExtension);
        m_types = new TypeCatalog(m_modules, new ReflectionTypeCatalogSource());
        m_types.Rebuild();
    }

    public void Dispose()
    {
        m_types.Dispose();
        m_modules.Dispose();
        if (Directory.Exists(m_root))
        {
            Directory.Delete(m_root, recursive: true);
        }
    }

    protected AudioRuntime CreateRuntime(
        int maxVoices = 128,
        long decodedCacheBudgetBytes = 128L * 1024 * 1024,
        long automaticStreamingThresholdBytes = 1024
    ) => new(
        m_types,
        new MutedAudioDevice(),
        m_artifacts,
        m_events,
        new DiagnosticHub().CreateReporter(new DiagnosticSource("tests.audio", "Audio")),
        options: new AudioRuntimeOptions
        {
            maxVoices = maxVoices,
            decodedCacheBudgetBytes = decodedCacheBudgetBytes,
            automaticStreamingThresholdBytes = automaticStreamingThresholdBytes
        }
    );

    protected AudioClipAsset CreateClip(long frameCount)
    {
        var clip = new AudioClipAsset();
        byte[] payload = AudioClipMetadataCodec.Encode(
            new AudioClipMetadata(AudioCodecID.wav, 2, 48_000, frameCount, 256)
        );
        new AssetRuntimeOwner().Initialize(
            clip,
            AssetPath.Project($"Audio/{clip.identity.persistentID:N}.wav"),
            "TEST",
            payload,
            isMissing: false,
            version: 1
        );
        m_artifacts.Add(clip.identity.persistentID, Path.Combine(m_root, $"{clip.identity.persistentID:N}.wav"));
        return clip;
    }

    protected sealed class TestAudioContent : IdentityObject
    {
        internal Action<AudioContentProviderContext>? first;
        internal Action<AudioContentProviderContext>? second;
        internal Action<AudioContentProviderContext>? third;
    }

    [AudioContentProviderExtension("tests.content.first", -30)]
    private sealed class FirstContentProvider : AudioContentProvider
    {
        public override void Submit(AudioContentProviderContext context)
        {
            foreach (TestAudioContent content in context.content.GetValues<TestAudioContent>())
            {
                content.first?.Invoke(context);
            }
        }
    }

    [AudioContentProviderExtension("tests.content.second", -20)]
    private sealed class SecondContentProvider : AudioContentProvider
    {
        public override void Submit(AudioContentProviderContext context)
        {
            foreach (TestAudioContent content in context.content.GetValues<TestAudioContent>())
            {
                content.second?.Invoke(context);
            }
        }
    }

    [AudioContentProviderExtension("tests.content.third", -10)]
    private sealed class ThirdContentProvider : AudioContentProvider
    {
        public override void Submit(AudioContentProviderContext context)
        {
            foreach (TestAudioContent content in context.content.GetValues<TestAudioContent>())
            {
                content.third?.Invoke(context);
            }
        }
    }

    [AudioContentProviderExtension("tests.retirement.first", -100)]
    protected sealed class RetirementProviderFirst : AudioContentProvider
    {
        internal static bool fail;
        internal static int disposals;
        internal static int pendingRetirements;

        public override void Submit(AudioContentProviderContext context) {}

        protected override void Dispose(bool disposing)
        {
            disposals++;
            if (pendingRetirements-- > 0)
            {
                throw new RetirementPendingException("Expected provider drain.");
            }
            if (fail)
            {
                throw new InvalidOperationException("Expected provider retirement failure.");
            }
        }
    }

    [AudioContentProviderExtension("tests.retirement.second", 100)]
    protected sealed class RetirementProviderSecond : AudioContentProvider
    {
        internal static bool failConstruction;
        internal static int disposals;

        public RetirementProviderSecond()
        {
            if (failConstruction)
            {
                throw new InvalidOperationException("Expected provider candidate failure.");
            }
        }

        public override void Submit(AudioContentProviderContext context) {}

        protected override void Dispose(bool disposing) => disposals++;
    }

    protected sealed class RecordingSink : IDiagnosticSink
    {
        public void Replace(DiagnosticReport report) => issues = report.diagnostics;

        public void Clear(DiagnosticSource source) => issues = [];

        internal IReadOnlyList<Diagnostic> issues { get; private set; } = [];
    }

    [AudioMixerExtension("tests.audio.mixer")]
    private sealed class TestMixerExtension : AudioMixerExtension
    {
        public override void Build(
            AudioMixerBuilder builder,
            SerializedAudioExtensionState state
        ) {
            builder.AddBus(new AudioBusID("tests.audio.bus.music"), AudioBusID.master);
            builder.AddProcessor(
                new AudioBusID("tests.audio.bus.music"),
                new AudioProcessorConfiguration(AudioProcessorID.lowPass)
            );
        }
    }

    protected sealed class FakeArtifactLookup : AssetResidencyProvider, IAssetArtifactLookup
    {
        private readonly ArtifactRetention m_retention = new();
        private readonly Dictionary<Guid, AssetArtifactInfo> m_artifacts = [];
        private readonly Dictionary<Guid, string> m_paths = [];
        internal int acquisitions;
        internal Exception? acquisitionFailure;
        internal Guid pendingReleaseID;
        internal int pendingReleases;
        internal int releaseAttempts;

        public ArtifactLease AcquireArtifact(
            Guid persistentID,
            string outputName
        ) {
            acquisitions++;
            if (acquisitionFailure is not null)
            {
                throw acquisitionFailure;
            }
            ArtifactLease retained = m_retention.Retain(
                m_artifacts[persistentID],
                () => File.OpenRead(m_paths[persistentID])
            );
            return CreateArtifactLease(retained.info, retained.OpenRead, () =>
            {
                releaseAttempts++;
                if (persistentID == pendingReleaseID && pendingReleases-- > 0)
                {
                    throw new RetirementPendingException("Expected admitted artifact drain.");
                }
                retained.Dispose();
            });
        }

        public bool TryGetArtifact(
            Guid persistentID,
            string outputName,
            out AssetArtifactInfo? artifact
        ) {
            if (outputName == "audio-data" && m_artifacts.TryGetValue(persistentID, out AssetArtifactInfo? found))
            {
                artifact = found;
                return true;
            }
            artifact = null;
            return false;
        }

        internal IReadOnlyList<AssetArtifactKey> retainedKeys => m_retention.GetRetainedKeys();

        internal void Add(
            Guid id,
            string path
        ) {
            var bytes = new byte[256];
            File.WriteAllBytes(path, bytes);
            m_paths[id] = path;
            m_artifacts[id] = new AssetArtifactInfo(
                new AssetArtifactKey(new string('A', 64)),
                "audio-data",
                Convert.ToHexString(SHA256.HashData(bytes)),
                256
            );
        }
    }

    protected sealed class ReadyAudioDevice : IAudioDevice
    {
        private readonly Dictionary<AudioBusHandle, AudioBusID> m_busIDs = [];
        private readonly Dictionary<AudioBusID, bool> m_busPaused = [];
        private readonly Dictionary<AudioBusID, float> m_busVolumes = [];
        private readonly IAudioDevice m_inner = new MutedAudioDevice();
        private readonly Dictionary<AudioBusHandle, bool> m_pausedHandles = [];
        internal int busPauseCalls;
        internal int? failBusPauseAt;
        internal bool failClipRetirement;
        internal bool failDisposal;
        internal int clipRetirements;
        internal int disposals;
        internal int pendingClipRetirements;
        internal int pendingBusRetirements;
        internal int busRetirements;
        internal int listenerCreations;
        internal int stops;
        internal Exception? clipQueryFailure;
        internal AudioClipState clipState = AudioClipState.Ready;

        public AudioCapabilities capabilities => m_inner.capabilities;

        public uint generation => m_inner.generation;

        public AudioDeviceState state => AudioDeviceState.Ready;

        public double dspTime => m_inner.dspTime;

        public AudioStatistics statistics => m_inner.statistics;

        public AudioClipHandle CreateClip(
            AudioClipDescriptor descriptor,
            IAudioClipSource source
        ) => m_inner.CreateClip(descriptor, source);

        public AudioClipState GetClipState(AudioClipHandle clip)
        {
            if (clipQueryFailure is not null)
            {
                throw clipQueryFailure;
            }
            return clipState;
        }

        public bool DestroyClip(AudioClipHandle clip)
        {
            clipRetirements++;
            if (pendingClipRetirements-- > 0)
            {
                throw new RetirementPendingException("Expected clip drain.");
            }
            if (failClipRetirement)
            {
                throw new InvalidOperationException("Expected clip retirement failure.");
            }
            return m_inner.DestroyClip(clip);
        }

        public AudioDeviceVoiceHandle Play(
            AudioClipHandle clip,
            AudioBusHandle bus,
            AudioPlayOptions options,
            double? scheduledDspTime = null
        ) => m_inner.Play(clip, bus, options, scheduledDspTime);

        public bool Stop(AudioDeviceVoiceHandle voice)
        {
            stops++;
            return m_inner.Stop(voice);
        }

        public bool Pause(AudioDeviceVoiceHandle voice) => m_inner.Pause(voice);

        public bool Resume(AudioDeviceVoiceHandle voice) => m_inner.Resume(voice);

        public bool Seek(
            AudioDeviceVoiceHandle voice,
            TimeSpan position
        ) => m_inner.Seek(voice, position);

        public bool SetVoiceParameters(
            AudioDeviceVoiceHandle voice,
            AudioVoiceParameters parameters
        ) => m_inner.SetVoiceParameters(voice, parameters);

        public bool TryGetVoiceState(
            AudioDeviceVoiceHandle voice,
            out AudioPlaybackState playbackState
        ) => m_inner.TryGetVoiceState(voice, out playbackState);

        public AudioBusHandle CreateBus(
            AudioBusID id,
            AudioBusHandle parent = default
        ) {
            AudioBusHandle handle = m_inner.CreateBus(id, parent);
            if (handle.isValid)
            {
                m_busIDs.Add(handle, id);
                m_pausedHandles.Add(handle, false);
            }
            return handle;
        }

        public bool DestroyBus(AudioBusHandle bus)
        {
            busRetirements++;
            if (pendingBusRetirements-- > 0)
            {
                throw new RetirementPendingException("Expected bus drain.");
            }
            bool destroyed = m_inner.DestroyBus(bus);
            if (destroyed)
            {
                m_pausedHandles.Remove(bus);
            }
            return destroyed;
        }

        public bool SetBusVolume(
            AudioBusHandle bus,
            float volume
        ) {
            if (!m_inner.SetBusVolume(bus, volume))
            {
                return false;
            }
            m_busVolumes[m_busIDs[bus]] = volume;
            return true;
        }

        public bool SetBusMuted(
            AudioBusHandle bus,
            bool muted
        ) => m_inner.SetBusMuted(bus, muted);

        public bool SetBusPaused(
            AudioBusHandle bus,
            bool paused
        ) {
            if (++busPauseCalls == failBusPauseAt)
            {
                return false;
            }
            if (!m_inner.SetBusPaused(bus, paused))
            {
                return false;
            }
            m_busPaused[m_busIDs[bus]] = paused;
            m_pausedHandles[bus] = paused;
            return true;
        }

        public bool AddBusProcessor(
            AudioBusHandle bus,
            AudioProcessorConfiguration processor
        ) => m_inner.AddBusProcessor(bus, processor);

        public AudioListenerHandle CreateListener(AudioListenerState state)
        {
            listenerCreations++;
            return m_inner.CreateListener(state);
        }

        public bool SetListener(
            AudioListenerHandle listener,
            AudioListenerState state
        ) => m_inner.SetListener(listener, state);

        public bool DestroyListener(AudioListenerHandle listener) => m_inner.DestroyListener(listener);

        public void Update(float deltaTime) => m_inner.Update(deltaTime);

        public bool TryDequeueCompletion(out AudioDeviceCompletion completion)
            => m_inner.TryDequeueCompletion(out completion);

        public void Dispose()
        {
            disposals++;
            m_inner.Dispose();
            if (failDisposal)
            {
                throw new InvalidOperationException("Expected device retirement failure.");
            }
        }

        internal float GetBusVolume(AudioBusID id) => m_busVolumes[id];

        internal bool GetBusPaused(AudioBusID id) => m_busPaused[id];

        internal IReadOnlyList<bool> GetMasterPauses()
            => m_pausedHandles.Where(pair => m_busIDs[pair.Key] == AudioBusID.master)
                .Select(pair => pair.Value)
                .ToArray();
    }
}
