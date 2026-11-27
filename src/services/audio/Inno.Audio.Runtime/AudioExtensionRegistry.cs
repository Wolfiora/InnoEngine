using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Inno.Core.Execution;
using Inno.Extensibility.Types;

namespace Inno.Audio.Runtime;

internal sealed class AudioExtensionRegistry : TypeRegistry<AudioExtensionRegistry.Snapshot>
{
    private readonly TypeCatalog m_types;
    private Exception? m_retirementFailure;

    internal AudioExtensionRegistry(TypeCatalog types)
        : base(types)
    {
        m_types = types;
    }

    internal Snapshot extensions => current;

    /// <summary>
    /// Revokes audio admission when candidate or active registry cleanup faults the shared generation.
    /// </summary>
    /// <param name="phase">
    /// The lifecycle phase that could not retire safely.
    /// </param>
    /// <param name="exception">
    /// The terminal cleanup failure already reported to the shared gate.
    /// </param>
    protected override void OnCleanupFailed(
        string phase,
        Exception exception
    ) => m_retirementFailure ??= exception;

    /// <summary>
    /// Builds a complete validated audio extension snapshot from one type generation.
    /// </summary>
    /// <param name="types">
    /// Candidate type-cache generation.
    /// </param>
    /// <returns>
    /// A validated audio extension registry snapshot.
    /// </returns>
    protected override Snapshot Build(TypeCacheSnapshot types)
    {
        ArgumentNullException.ThrowIfNull(types);
        return new Snapshot(
            types,
            Discover<AudioMixerExtensionAttribute, AudioMixerExtension>(types, static value => value.id, "mixer"),
            Discover<AudioMixerFeatureExtensionAttribute, AudioMixerFeature>(types, static value => value.id, "mixer feature"),
            Discover<AudioContentProviderExtensionAttribute, AudioContentProvider>(types, static value => value.id, "content provider"),
            Discover<AudioBlockProcessorAttribute, AudioBlockProcessor>(types, static value => value.id, "block processor"),
            Discover<AudioStreamDecoderAttribute, AudioStreamDecoder>(types, static value => value.id, "stream decoder"),
            CreateExtension<AudioContentProvider>
        );
    }

    internal AudioBlockProcessor? BindProcessor(AudioProcessorID id)
        => extensions.HasProcessor(id.value) ? new BoundProcessor(this, id) : null;

    internal AudioStreamReader OpenDecoder(
        AudioClipDescriptor descriptor,
        IAudioClipSource source
    ) {
        using IDisposable operation = AcquireOperation("Open plugin decoder");
        Stream stream = source.OpenRead();
        try
        {
            AudioStreamReader reader = extensions.CreateDecoder(descriptor.codec).Open(stream, descriptor)
                                       ?? throw new InvalidDataException("The audio decoder did not supply a cursor.");
            return new DecoderCursor(this, extensions.Track(new OwnedCursor(reader, stream)));
        }
        catch
        {
            stream.Dispose();
            throw;
        }
    }

    private sealed class OwnedCursor(
        AudioStreamReader reader,
        Stream stream
    ) : IDisposable
    {
        internal AudioStreamReader decoder { get; } = reader;

        public void Dispose()
        {
            try
            {
                decoder.Dispose();
            }
            finally
            {
                stream.Dispose();
            }
        }
    }

    private sealed class DecoderCursor(
        AudioExtensionRegistry registry,
        Lease<OwnedCursor> lease
    ) : AudioStreamReader
    {
        private AudioStreamReader decoder => lease.resource?.decoder ?? throw new ObjectDisposedException("Audio decoder generation");

        public override bool canSeek
        {
            get
            {
                using IDisposable operation = registry.AcquireOperation("Inspect plugin decoder");
                return decoder.canSeek;
            }
        }

        public override int Read(Span<float> samples)
        {
            using IDisposable operation = registry.AcquireOperation("Read plugin audio frames");
            return decoder.Read(samples);
        }

        public override bool Seek(long frame)
        {
            using IDisposable operation = registry.AcquireOperation("Seek plugin audio frames");
            return decoder.Seek(frame);
        }

        public override void Dispose() => lease.Dispose();
    }

    internal IDisposable AcquireOperation(string name) => m_types.AcquireOperation(name);

    private sealed class BoundProcessor(
        AudioExtensionRegistry registry,
        AudioProcessorID id
    ) : AudioBlockProcessor
    {
        private Lease<AudioBlockProcessor>? m_lease;
        private bool m_disposed;

        public override void Process(
            Span<float> samples,
            int channels,
            int sampleRate,
            AudioProcessorConfiguration configuration
        ) {
            ObjectDisposedException.ThrowIf(m_disposed, this);
            using IDisposable operation = registry.AcquireOperation("Process plugin audio block");
            if (m_lease?.resource is null) m_lease = registry.extensions.OpenProcessor(id.value);
            m_lease.resource!.Process(samples, channels, sampleRate, configuration);
        }

        public override void Dispose()
        {
            m_disposed = true;
            m_lease?.Dispose();
            m_lease = null;
        }
    }

    internal sealed class Lease<TResource>(
        HashSet<IDisposable> active,
        TResource value
    ) : IDisposable
        where TResource : class, IDisposable
    {
        private HashSet<IDisposable>? m_active = active;
        internal TResource? resource { get; private set; } = value;

        public void Dispose()
        {
            if (resource is not { } current) return;
            try
            {
                current.Dispose();
            }
            catch (Exception exception) when (RetirementPendingException.Find(exception) is null)
            {
                Release();
                throw;
            }
            Release();
        }

        private void Release()
        {
            resource = null;
            m_active?.Remove(this);
            m_active = null;
        }
    }

    internal void RetireBackend(Action retire)
    {
        try
        {
            RetireResource("audio device generation", retire);
        }
        catch (Exception exception)
        {
            Fault(exception);
            throw;
        }
    }

    internal void Fault(Exception exception)
    {
        m_retirementFailure ??= exception;
        ReportRetirementFailure(exception);
    }

    internal void EnsureHealthy()
    {
        if (m_retirementFailure is not null)
        {
            throw new InvalidOperationException("The audio generation owner is Faulted and requires a host restart.", m_retirementFailure);
        }
    }

    private static Dictionary<string, Type> Discover<TAttribute, TContract>(
        TypeCacheSnapshot types,
        Func<TAttribute, string> getID,
        string kind
    ) where TAttribute : Attribute
    {
        var result = new Dictionary<string, Type>(StringComparer.Ordinal);
        foreach (TypeRef typeRef in types.GetTypesWithAttribute<TAttribute>())
        {
            Type type = typeRef.Resolve(types);
            if (type.IsAbstract || !typeof(TContract).IsAssignableFrom(type))
            {
                throw new InvalidOperationException(
                    $"Audio {kind} '{type.FullName}' must be a non-abstract {typeof(TContract).FullName}."
                );
            }
            if (!types.CanCreateInstance(typeRef))
            {
                throw new InvalidOperationException($"Audio {kind} '{type.FullName}' requires a parameterless constructor.");
            }
            TAttribute attribute = types.GetAttribute<TAttribute>(typeRef, inherit: false)!;
            string id = getID(attribute);
            if (!result.TryAdd(id, type))
            {
                throw new InvalidOperationException(
                    $"Audio {kind} ID '{id}' is declared by both '{result[id].FullName}' and '{type.FullName}'."
                );
            }
        }
        return result;
    }

    internal sealed class Snapshot : IDisposable
    {
        private readonly IReadOnlyDictionary<string, Type> m_features;
        private readonly IReadOnlyDictionary<string, Type> m_mixers;
        private readonly IReadOnlyDictionary<string, Type> m_providers;
        private readonly IReadOnlyDictionary<string, Type> m_processors;
        private readonly IReadOnlyDictionary<string, Type> m_decoders;
        private readonly HashSet<IDisposable> m_active = [];
        private TypeCacheSnapshot? m_types;

        internal Snapshot(
            TypeCacheSnapshot types,
            IReadOnlyDictionary<string, Type> mixers,
            IReadOnlyDictionary<string, Type> features,
            IReadOnlyDictionary<string, Type> providers,
            IReadOnlyDictionary<string, Type> processors,
            IReadOnlyDictionary<string, Type> decoders,
            Func<Type, AudioContentProvider> createProvider
        ) {
            m_types = types;
            m_processors = processors;
            m_decoders = decoders;
            this.typeCacheVersion = types.version;
            m_mixers = mixers;
            m_features = features;
            m_providers = providers;
            this.providers = CreateProviders(createProvider);
        }

        /// <summary>
        /// Retires the provider instances with their owning type-catalog snapshot.
        /// </summary>
        public void Dispose()
        {
            List<Exception>? failures = null;
            foreach (IDisposable instance in m_active.ToArray())
            {
                try
                {
                    instance.Dispose();
                }
                catch (Exception exception) when (RetirementPendingException.Find(exception) is null)
                {
                    (failures ??= []).Add(exception);
                }
            }
            providers.Dispose();
            m_types = null;
            if (failures is not null) throw new AggregateException("Audio extension retirement failed.", failures);
        }

        internal long typeCacheVersion { get; }

        internal ProviderGeneration providers { get; }

        internal bool TryBuildMixer(
            AudioMixerAsset asset,
            out AudioMixer? mixer
        ) {
            ArgumentNullException.ThrowIfNull(asset);
            var builder = new AudioMixerBuilder();
            if (!string.IsNullOrWhiteSpace(asset.mixerTypeId))
            {
                if (!m_mixers.TryGetValue(asset.mixerTypeId, out Type? mixerType))
                {
                    mixer = null;
                    return false;
                }
                Create<AudioMixerExtension>(mixerType).Build(builder, asset.mixerState);
            }

            var configured = new HashSet<string>(StringComparer.Ordinal);
            foreach (AudioMixerFeatureConfiguration configuration in asset.features)
            {
                if (!configuration.enabled)
                {
                    continue;
                }
                if (string.IsNullOrWhiteSpace(configuration.featureTypeId) || !configured.Add(configuration.featureTypeId))
                {
                    throw new InvalidOperationException("Enabled audio mixer features require unique stable identifiers.");
                }
                if (!m_features.TryGetValue(configuration.featureTypeId, out Type? featureType))
                {
                    mixer = null;
                    return false;
                }
                Create<AudioMixerFeature>(featureType).Build(builder, configuration.state);
            }
            mixer = builder.Build();
            return true;
        }

        internal bool HasDecoder(AudioCodecID codec) => m_decoders.ContainsKey(codec.value);

        internal AudioStreamDecoder CreateDecoder(AudioCodecID codec)
            => m_decoders.TryGetValue(codec.value, out Type? type)
               ? Create<AudioStreamDecoder>(type)
               : throw new InvalidOperationException($"Audio decoder '{codec.value}' has retired.");

        internal bool HasProcessor(string id) => m_processors.ContainsKey(id);

        internal Lease<AudioBlockProcessor> OpenProcessor(string id)
        {
            if (!m_processors.TryGetValue(id, out Type? type))
            {
                throw new InvalidOperationException($"Audio processor '{id}' is missing from the active generation.");
            }
            return Track(Create<AudioBlockProcessor>(type));
        }

        internal Lease<TResource> Track<TResource>(TResource resource)
            where TResource : class, IDisposable
        {
            var lease = new Lease<TResource>(m_active, resource);
            m_active.Add(lease);
            return lease;
        }

        private ProviderGeneration CreateProviders(Func<Type, AudioContentProvider> createProvider)
        {
            var entries = new List<ProviderEntry>(m_providers.Count);
            foreach ((string id, Type type) in m_providers)
            {
                AudioContentProviderExtensionAttribute attribute = m_types!.GetAttribute<AudioContentProviderExtensionAttribute>(
                    m_types.GetTypeRef(type),
                    inherit: false
                )!;
                entries.Add(new ProviderEntry(id, attribute.priority, createProvider(type)));
            }
            entries.Sort(static (
                left,
                right
            ) =>
            {
                int priority = left.priority.CompareTo(right.priority);
                return priority != 0 ? priority : string.CompareOrdinal(left.id, right.id);
            });
            return new ProviderGeneration(typeCacheVersion, entries);
        }

        private TContract Create<TContract>(Type type)
            where TContract : class
        {
            try
            {
                return (TContract)(m_types ?? throw new ObjectDisposedException(nameof(Snapshot))).CreateInstance(type);
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"Audio extension '{type.FullName}' could not be activated.", exception);
            }
        }
    }

    internal sealed class ProviderGeneration : IDisposable
    {
        private readonly LifetimeScope m_lifetime = new();
        private bool m_disposed;

        internal ProviderGeneration(
            long typeCacheVersion,
            IReadOnlyList<ProviderEntry> providers
        ) {
            this.typeCacheVersion = typeCacheVersion;
            this.providers = Array.AsReadOnly(providers.ToArray());
            foreach (ProviderEntry entry in providers)
            {
                m_lifetime.Own(entry.provider);
            }
        }

        /// <summary>
        /// Releases every provider instance retained by this candidate generation.
        /// </summary>
        public void Dispose()
        {
            if (m_disposed)
            {
                return;
            }
            try
            {
                m_lifetime.Dispose();
            }
            catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
            {
                throw;
            }
            catch
            {
                providers = [];
                m_disposed = true;
                throw;
            }
            providers = [];
            m_disposed = true;
        }

        internal long typeCacheVersion { get; }

        internal IReadOnlyList<ProviderEntry> providers { get; private set; }
    }

    internal sealed record ProviderEntry(
        string id,
        int priority,
        AudioContentProvider provider
    );
}
