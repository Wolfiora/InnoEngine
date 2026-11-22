using System;
using System.Collections.Generic;
using Inno.Core.Execution;

namespace Inno.Extensibility.Modules;

internal sealed class AssemblyCatalogCoordinator
{
    private readonly object m_sync = new();
    private readonly List<ParticipantReference> m_participants = [];
    private Exception? m_retirementFailure;
    private object? m_retainedPreparation;

    internal CatalogParticipantRegistration Register(IAssemblyCatalogParticipant participant)
    {
        lock (m_sync)
        {
            RemoveCollectedParticipants();
            var registration = new CatalogParticipantRegistration(this, Guid.NewGuid());
            m_participants.Add(
                new ParticipantReference(registration.id, new WeakReference<IAssemblyCatalogParticipant>(participant))
            );
            return registration;
        }
    }

    internal AssemblyCatalogRefreshSet Prepare(AssemblyCatalogSnapshot catalog)
    {
        if (m_retirementFailure is not null)
        {
            GC.KeepAlive(m_retainedPreparation);
            throw m_retirementFailure;
        }
        List<IAssemblyCatalogParticipant> participants = [];
        lock (m_sync)
        {
            RemoveCollectedParticipants();
            foreach (ParticipantReference registration in m_participants)
            {
                if (registration.participant.TryGetTarget(out IAssemblyCatalogParticipant? participant))
                {
                    participants.Add(participant);
                }
            }
        }

        var transactions = new List<IAssemblyCatalogTransaction>(participants.Count);
        try
        {
            foreach (IAssemblyCatalogParticipant participant in participants)
            {
                transactions.Add(participant.Prepare(catalog));
            }
            return new AssemblyCatalogRefreshSet(transactions);
        }
        catch (Exception failure) when (RetirementPendingException.Find(failure) is not null)
        {
            m_retirementFailure = failure;
            m_retainedPreparation = (participants, transactions);
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
                        "Catalog preparation failed and rollback remains pending.",
                        [.. failures, failure]
                    );
                    m_retainedPreparation = (participants, transactions);
                    throw m_retirementFailure;
                }
                catch (Exception cleanupFailure)
                {
                    failures.Add(cleanupFailure);
                }
            }
            if (failures.Count > 1)
            {
                throw new AggregateException("Assembly catalog preparation and rollback failed.", failures);
            }
            throw;
        }
    }

    internal void Unregister(Guid registrationID)
    {
        lock (m_sync)
        {
            m_participants.RemoveAll(registration => registration.id == registrationID);
        }
    }

    private void RemoveCollectedParticipants()
        => m_participants.RemoveAll(static registration => !registration.participant.TryGetTarget(out _));

    private readonly record struct ParticipantReference(
        Guid id,
        WeakReference<IAssemblyCatalogParticipant> participant
    );
}
