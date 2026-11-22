using System;
using System.Collections.Generic;
using Inno.Core.Execution;

namespace Inno.Extensibility.Types;

internal sealed class TypeRegistryCoordinator
{
    private readonly object m_sync = new();
    private readonly List<RegistryReference> m_registries = [];
    private Exception? m_retirementFailure;
    private object? m_retainedPreparation;

    internal TypeRegistryRegistration Register(ITypeRegistry registry)
    {
        lock (m_sync)
        {
            RemoveCollectedRegistries();
            var registration = new TypeRegistryRegistration(this, Guid.NewGuid());
            m_registries.Add(new RegistryReference(registration.id, new WeakReference<ITypeRegistry>(registry)));
            return registration;
        }
    }

    internal TypeRegistryRefreshSet Prepare(TypeCacheSnapshot types)
    {
        if (m_retirementFailure is not null)
        {
            GC.KeepAlive(m_retainedPreparation);
            throw m_retirementFailure;
        }
        List<ITypeRegistry> registries = [];
        lock (m_sync)
        {
            RemoveCollectedRegistries();
            foreach (RegistryReference registration in m_registries)
            {
                if (registration.registry.TryGetTarget(out ITypeRegistry? registry))
                {
                    registries.Add(registry);
                }
            }
        }

        var transactions = new List<ITypeRegistryTransaction>(registries.Count);
        try
        {
            foreach (ITypeRegistry registry in registries)
            {
                transactions.Add(registry.Prepare(types));
            }
            return new TypeRegistryRefreshSet(transactions);
        }
        catch (Exception failure) when (RetirementPendingException.Find(failure) is not null)
        {
            m_retirementFailure = failure;
            m_retainedPreparation = (registries, transactions);
            throw;
        }
        catch (Exception preparationFailure)
        {
            List<Exception> failures = [preparationFailure];
            for (int i = transactions.Count - 1; i >= 0; i--)
            {
                try
                {
                    transactions[i].Rollback();
                }
                catch (Exception failure) when (RetirementPendingException.Find(failure) is not null)
                {
                    m_retirementFailure = new AggregateException(
                        "Registry preparation failed and rollback remains pending.",
                        [.. failures, failure]
                    );
                    m_retainedPreparation = (registries, transactions);
                    throw m_retirementFailure;
                }
                catch (Exception cleanupFailure)
                {
                    failures.Add(cleanupFailure);
                }
            }
            if (failures.Count > 1)
            {
                throw new AggregateException("Type registry preparation and rollback failed.", failures);
            }
            throw;
        }
    }

    internal void Unregister(Guid registrationID)
    {
        lock (m_sync)
        {
            m_registries.RemoveAll(registration => registration.id == registrationID);
        }
    }

    private void RemoveCollectedRegistries() => m_registries.RemoveAll(static registration => !registration.registry.TryGetTarget(out _));

    private readonly record struct RegistryReference(
        Guid id,
        WeakReference<ITypeRegistry> registry
    );
}
