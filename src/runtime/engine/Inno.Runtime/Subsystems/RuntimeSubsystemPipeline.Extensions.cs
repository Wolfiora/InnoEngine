using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Core.Execution;
using Inno.Extensibility.Types;
using Inno.Runtime.Contracts;

namespace Inno.Runtime;

public sealed partial class RuntimeSubsystemPipeline
{
    private sealed class ExtensionRegistry(
        RuntimeSubsystemPipeline pipeline,
        IReadOnlyList<IRuntimeSubsystemFactory> configured
    ) : TypeRegistry<Generation>(pipeline.m_context.types, pipeline.m_retirementTimeout)
    {
        internal IRuntimeSubsystemFactory[] ordered => current.ordered;

        /// <inheritdoc/>
        protected override Generation Build(TypeCacheSnapshot types)
        {
            List<IRuntimeSubsystemFactory> factories = [.. configured];
            foreach (TypeRef reference in types.GetTypesWithAttribute<RuntimeSubsystemExtensionAttribute>())
            {
                RuntimeSubsystemExtensionAttribute declaration = types.GetAttribute<RuntimeSubsystemExtensionAttribute>(
                    reference,
                    inherit: false
                )!;
                if (!Enum.IsDefined(declaration.lifetime))
                {
                    throw new InvalidOperationException($"Unknown owner scope for subsystem '{declaration.id}'.");
                }
                if (declaration.lifetime != pipeline.m_context.lifetime) continue;

                Type type = reference.Resolve(types);
                if (!typeof(IRuntimeSubsystemActivator).IsAssignableFrom(type) || !types.CanCreateInstance(reference))
                {
                    throw new InvalidOperationException($"Subsystem '{declaration.id}' requires a constructible activator.");
                }
                factories.Add(new DiscoveredFactory(types, type, declaration));
            }
            return new Generation(Order(factories));
        }

        /// <inheritdoc/>
        protected override void OnActivating(
            Generation? previous,
            Generation candidate
        ) {
            if (previous is null) return;
            if (pipeline.m_frameOpen || pipeline.m_stopping)
            {
                throw new InvalidOperationException("Subsystem generations can change only between active frames.");
            }

            Dictionary<RuntimeSubsystemID, Entry> active = pipeline.m_entries.ToDictionary(static entry => entry.descriptor.id);
            Dictionary<RuntimeSubsystemID, IRuntimeSubsystemFactory> oldFactories = previous.ordered
                .ToDictionary(static factory => factory.descriptor.id);
            var changed = new HashSet<RuntimeSubsystemID>();
            foreach (IRuntimeSubsystemFactory factory in candidate.ordered)
            {
                RuntimeSubsystemID id = factory.descriptor.id;
                oldFactories.TryGetValue(id, out IRuntimeSubsystemFactory? prior);
                if (factory is DiscoveredFactory discovered && (prior as DiscoveredFactory)?.type != discovered.type
                    || factory.descriptor.dependencies.Any(changed.Contains)
                ) {
                    changed.Add(id);
                    if (active.ContainsKey(id) && (prior is not DiscoveredFactory { requiresRestart: false }
                                                   || factory is not DiscoveredFactory { requiresRestart: false })
                    ) {
                        throw new InvalidOperationException($"Replacing subsystem '{id}' requires a host restart.");
                    }
                }
            }
            HashSet<RuntimeSubsystemID> retainedIDs = candidate.ordered.Select(static factory => factory.descriptor.id).ToHashSet();
            foreach (DiscoveredFactory removed in previous.ordered.OfType<DiscoveredFactory>())
            {
                if (!retainedIDs.Contains(removed.descriptor.id) && active.ContainsKey(removed.descriptor.id) && removed.requiresRestart)
                {
                    throw new InvalidOperationException($"Removing subsystem '{removed.descriptor.id}' requires a host restart.");
                }
            }
            var entries = new List<Entry>(candidate.ordered.Length);
            var available = new HashSet<RuntimeSubsystemID>();
            foreach (IRuntimeSubsystemFactory factory in candidate.ordered)
            {
                RuntimeSubsystemDescriptor descriptor = factory.descriptor;
                if (!changed.Contains(descriptor.id))
                {
                    if (active.TryGetValue(descriptor.id, out Entry? retained))
                    {
                        entries.Add(retained);
                        available.Add(descriptor.id);
                    }
                    continue;
                }
                if (!descriptor.dependencies.All(available.Contains)
                    || !descriptor.requiredCapabilities.All(pipeline.m_context.capabilities.Contains)
                ) {
                    throw new InvalidOperationException($"Subsystem '{descriptor.id}' has unavailable dependencies or capabilities.");
                }
                var entry = new Entry(descriptor);
                candidate.release.Add(entry);
                try
                {
                    pipeline.Attach(entry, factory);
                }
                catch (Exception failure) when (RetirementPendingException.Find(failure) is null)
                {
                    // Exception stack frames can keep a collectible candidate alive beyond rollback.
                    throw new InvalidOperationException($"Subsystem '{descriptor.id}' rejected activation: {failure.Message}");
                }
                entries.Add(entry);
                available.Add(descriptor.id);
            }
            candidate.before = pipeline.m_entries;
            pipeline.m_entries = entries;
        }

        /// <inheritdoc/>
        protected override void OnActivationRolledBack(
            Generation? previous,
            Generation candidate
        ) {
            if (candidate.before is not null) pipeline.m_entries = candidate.before;
            candidate.before = null;
        }

        /// <inheritdoc/>
        protected override void OnActivationCompleted(
            Generation? previous,
            Generation candidate
        ) {
            if (previous is null || candidate.before is null) return;
            HashSet<Entry> retained = pipeline.m_entries.ToHashSet();
            previous.release.AddRange(candidate.before.Where(entry => !retained.Contains(entry)));
            candidate.before = null;
            candidate.release.Clear();
        }

        /// <inheritdoc/>
        protected override void DisposeSnapshot(Generation snapshot)
        {
            while (snapshot.release.Count != 0)
            {
                Entry entry = snapshot.release[^1];
                try
                {
                    RetireEntry(entry);
                }
                catch (Exception exception) when (RetirementPendingException.Find(exception) is null)
                {
                    snapshot.failures.Add(exception);
                }
                snapshot.release.RemoveAt(snapshot.release.Count - 1);
            }
            if (snapshot.failures.Count != 0)
            {
                throw new AggregateException("Plugin subsystem retirement failed.", snapshot.failures);
            }
        }
    }

    private sealed class Generation(IRuntimeSubsystemFactory[] ordered)
    {
        internal IRuntimeSubsystemFactory[] ordered { get; } = ordered;
        internal List<Entry>? before { get; set; }
        internal List<Entry> release { get; } = [];
        internal List<Exception> failures { get; } = [];
    }

    private sealed class DiscoveredFactory(
        TypeCacheSnapshot types,
        Type activatorType,
        RuntimeSubsystemExtensionAttribute declaration
    ) : IRuntimeSubsystemFactory
    {
        /// <inheritdoc/>
        public RuntimeSubsystemDescriptor descriptor { get; } = new(
            new RuntimeSubsystemID(declaration.id),
            dependencies: declaration.dependencies.Select(static id => new RuntimeSubsystemID(id)).ToArray(),
            lifetime: declaration.lifetime,
            requiredCapabilities: declaration.requiredCapabilities.Select(static id => new RuntimeCapabilityID(id)).ToArray()
        );

        /// <inheritdoc/>
        public IRuntimeSubsystem Create(RuntimeSubsystemContext context)
        {
            IRuntimeSubsystemActivator activator = (IRuntimeSubsystemActivator)types.CreateInstance(type);
            if (activator is IDisposable disposable) context.resources.Own(disposable);
            return activator.Create(context);
        }

        internal Type type { get; } = activatorType;
        internal bool requiresRestart { get; } = declaration.requiresRestart;
    }
}
