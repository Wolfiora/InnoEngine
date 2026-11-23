using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Inno.Core.Serialization.Converters;
using Inno.Extensibility.Types;

namespace Inno.Core.Serialization;

internal sealed class ConverterRegistry : IDisposable
{
    private readonly ConverterTypeRegistry m_registry;
    private bool m_disposed;

    internal ConverterRegistry(TypeCatalog types)
    {
        ArgumentNullException.ThrowIfNull(types);
        m_registry = new ConverterTypeRegistry(types);
        m_registry.Refresh();
    }

    /// <summary>
    /// Releases the resources owned by this instance.
    /// </summary>
    public void Dispose()
    {
        if (m_disposed)
        {
            return;
        }
        m_disposed = true;
        m_registry.Dispose();
    }

    internal void Refresh() => RequireRegistry().Refresh();

    internal ConverterInvoker? Resolve(Type valueType)
    {
        using ConverterRegistryLease lease = Capture();
        return lease.Resolve(valueType);
    }

    internal ConverterRegistryLease Capture() => RequireRegistry().Capture();

    internal static ConverterInvoker? ResolveSnapshot(
        ConverterRegistrySnapshot snapshot,
        Type valueType
    ) {
        List<ConverterCandidate> candidates = [];
        for (int i = 0; i < snapshot.registrations.Count; i++)
        {
            if (TryCreateCandidate(snapshot, snapshot.registrations[i], valueType, out ConverterCandidate? candidate))
            {
                candidates.Add(candidate!);
            }
        }

        if (candidates.Count == 0)
        {
            return null;
        }

        int bestDistance = candidates.Min(static candidate => candidate.distance);
        ConverterCandidate[] best = candidates
            .Where(candidate => candidate.distance == bestDistance)
            .OrderBy(static candidate => candidate.converterType.FullName, StringComparer.Ordinal)
            .ToArray();
        if (best.Length > 1)
        {
            throw new InvalidOperationException(
                $"Serialization type '{valueType.FullName}' has ambiguous converters "
                + $"'{best[0].converterType.FullName}' and '{best[1].converterType.FullName}' at distance {bestDistance}."
            );
        }

        return best[0].invoker;
    }

    private ConverterTypeRegistry RequireRegistry()
        => !m_disposed
           ? m_registry
           : throw new ObjectDisposedException(nameof(ConverterRegistry));

    private static bool TryCreateCandidate(
        ConverterRegistrySnapshot snapshot,
        Type registeredType,
        Type valueType,
        out ConverterCandidate? candidate
    ) {
        candidate = null;
        if (!TryGetConverterTargetPattern(registeredType, out Type targetPattern))
        {
            throw new InvalidOperationException(
                $"Serialization extension '{registeredType.FullName}' must inherit SerializationConverter<T>."
            );
        }

        Type closedConverterType = registeredType;
        if (registeredType.ContainsGenericParameters)
        {
            if (!registeredType.IsGenericTypeDefinition)
            {
                return false;
            }

            var bindings = new Dictionary<Type, Type>();
            if (!TryUnify(targetPattern, valueType, bindings))
            {
                return false;
            }

            Type[] parameters = registeredType.GetGenericArguments();
            var arguments = new Type[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                if (!bindings.TryGetValue(parameters[i], out Type? argument))
                {
                    return false;
                }
                arguments[i] = argument;
            }

            Type? construction = snapshot.ConstructGenericType(registeredType, arguments);
            if (construction is null)
            {
                return false;
            }
            closedConverterType = construction;
        }

        if (!TryGetConverterTargetPattern(closedConverterType, out Type targetType) || targetType.ContainsGenericParameters)
        {
            return false;
        }
        if (!targetType.IsAssignableFrom(valueType))
        {
            return false;
        }

        SerializationConverter converter = (SerializationConverter)GetOrCreateConverter(snapshot, closedConverterType);
        ConverterInvoker invoker = converter.CreateInvoker();
        candidate = new ConverterCandidate(closedConverterType, GetTypeDistance(valueType, targetType), invoker);
        return true;
    }

    private static object GetOrCreateConverter(
        ConverterRegistrySnapshot snapshot,
        Type converterType
    ) {
        if (snapshot.converterInstances.TryGetValue(converterType, out object? converter))
        {
            return converter;
        }

        try
        {
            converter = snapshot.CreateInstance(converterType);
        }
        catch (Exception exception)
        {
            throw new InvalidOperationException(
                $"Serialization converter '{converterType.FullName}' must have a parameterless constructor.",
                exception
            );
        }

        snapshot.converterInstances.Add(converterType, converter);
        return converter;
    }

    private static bool TryGetConverterTargetPattern(
        Type converterType,
        out Type targetPattern
    ) {
        for (Type? current = converterType; current is not null; current = current.BaseType)
        {
            if (!current.IsGenericType || current.GetGenericTypeDefinition() != typeof(SerializationConverter<>))
            {
                continue;
            }

            targetPattern = current.GetGenericArguments()[0];
            return true;
        }

        targetPattern = null!;
        return false;
    }

    private static bool TryUnify(
        Type pattern,
        Type concrete,
        Dictionary<Type, Type> bindings
    ) {
        if (pattern.IsGenericParameter)
        {
            if (bindings.TryGetValue(pattern, out Type? existing))
            {
                return existing == concrete;
            }
            bindings.Add(pattern, concrete);
            return true;
        }

        if (pattern.IsArray)
        {
            return concrete.IsArray
                   && pattern.GetArrayRank() == concrete.GetArrayRank()
                   && TryUnify(pattern.GetElementType()!, concrete.GetElementType()!, bindings);
        }

        if (!pattern.IsGenericType)
        {
            return pattern == concrete;
        }
        if (!concrete.IsGenericType || pattern.GetGenericTypeDefinition() != concrete.GetGenericTypeDefinition())
        {
            return false;
        }

        Type[] patternArguments = pattern.GetGenericArguments();
        Type[] concreteArguments = concrete.GetGenericArguments();
        for (int i = 0; i < patternArguments.Length; i++)
        {
            if (!TryUnify(patternArguments[i], concreteArguments[i], bindings))
            {
                return false;
            }
        }

        return true;
    }

    private static int GetTypeDistance(
        Type derivedType,
        Type targetType
    ) {
        if (derivedType == targetType)
        {
            return 0;
        }
        // Assignability was validated by the caller. Implemented interfaces share one rank,
        // including variant constructions; class inheritance retains its exact depth.
        if (targetType.IsInterface)
        {
            return 1;
        }

        int distance = 1;
        for (Type? current = derivedType.BaseType; current is not null; current = current.BaseType)
        {
            if (current == targetType)
            {
                return distance;
            }
            distance++;
        }

        return int.MaxValue;
    }

    private sealed record ConverterCandidate(
        Type converterType,
        int distance,
        ConverterInvoker invoker
    );

    private sealed class ConverterTypeRegistry : TypeRegistry<ConverterRegistrySnapshot>
    {
        internal ConverterTypeRegistry(TypeCatalog types)
            : base(types) {}

        /// <summary>
        /// Builds a validated result from the current immutable input snapshot.
        /// </summary>
        /// <param name="types">
        /// The active type catalog generation used for extension resolution.
        /// </param>
        /// <returns>
        /// The validated converter registry snapshot that represents the completed operation.
        /// </returns>
        protected override ConverterRegistrySnapshot Build(TypeCacheSnapshot types)
        {
            IReadOnlyList<Type> registrations = types
                .GetSubTypesOf<SerializationConverter>()
                .Select(typeRef => typeRef.Resolve(types))
                .OrderBy(static type => type.FullName, StringComparer.Ordinal)
                .ToArray();
            for (int i = 0; i < registrations.Count; i++)
            {
                if (!TryGetConverterTargetPattern(registrations[i], out _))
                {
                    throw new InvalidOperationException(
                        $"Serialization extension '{registrations[i].FullName}' must inherit SerializationConverter<T>."
                    );
                }
            }

            return new ConverterRegistrySnapshot(registrations, types);
        }

        /// <summary>
        /// Releases the generation lease retained by an immutable registry snapshot.
        /// </summary>
        /// <param name="snapshot">
        /// The immutable state snapshot consumed by this operation.
        /// </param>
        protected override void DisposeSnapshot(ConverterRegistrySnapshot snapshot) => snapshot.Release();

        internal ConverterRegistryLease Capture()
        {
            while (true)
            {
                ConverterRegistrySnapshot snapshot = current;
                if (snapshot.TryAcquire())
                {
                    return new ConverterRegistryLease(snapshot);
                }
            }
        }
    }

    internal sealed class ConverterRegistrySnapshot(
        IReadOnlyList<Type> registrations,
        TypeCacheSnapshot types
    ) {
        internal readonly Guid captureIdentity = Guid.NewGuid();
        internal readonly Lock sync = new();
        internal readonly ConcurrentDictionary<Type, ConverterInvoker?> cache = new();
        internal readonly Dictionary<Type, object> converterInstances = [];
        private TypeCacheSnapshot? m_types = types;
        internal IReadOnlyList<Type> registrations = registrations;
        private int m_referenceCount = 1;

        internal TypeCacheSnapshot types => m_types ?? throw new ObjectDisposedException(nameof(ConverterRegistrySnapshot));

        internal object CreateInstance(Type type)
            => (m_types ?? throw new ObjectDisposedException(nameof(ConverterRegistrySnapshot))).CreateInstance(type);

        internal Type? ConstructGenericType(
            Type definition,
            Type[] arguments
        ) => (m_types ?? throw new ObjectDisposedException(nameof(ConverterRegistrySnapshot)))
            .ConstructGenericType(definition, arguments);

        internal bool TryAcquire()
        {
            lock (sync)
            {
                if (m_referenceCount == 0)
                {
                    return false;
                }
                m_referenceCount++;
                return true;
            }
        }

        internal void Release()
        {
            bool dispose;
            lock (sync)
            {
                if (m_referenceCount <= 0)
                {
                    throw new InvalidOperationException("The serialization converter generation was released twice.");
                }
                m_referenceCount--;
                dispose = m_referenceCount == 0;
            }
            if (!dispose)
            {
                return;
            }

            foreach (object converter in converterInstances.Values.Distinct(ReferenceEqualityComparer.Instance))
            {
                if (converter is IDisposable disposable)
                {
                    try
                    {
                        disposable.Dispose();
                    }
                    catch (Exception exception)
                    {
                        Trace.TraceError(
                            "Serialization converter '{0}' failed while being disposed: {1}",
                            converter.GetType().FullName,
                            exception
                        );
                    }
                }
            }
            lock (sync)
            {
                cache.Clear();
                converterInstances.Clear();
                registrations = [];
                m_types = null;
            }
        }
    }
}
