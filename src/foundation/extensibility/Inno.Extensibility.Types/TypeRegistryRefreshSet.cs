using System;
using System.Collections.Generic;
using Inno.Core.Execution;

namespace Inno.Extensibility.Types;

internal sealed class TypeRegistryRefreshSet(IReadOnlyList<ITypeRegistryTransaction> transactions)
{
    private int m_activatedCount;
    private bool m_finished;
    private Exception? m_failure;

    internal void Activate()
    {
        if (m_failure is not null)
        {
            throw m_failure;
        }
        if (m_finished)
        {
            throw new InvalidOperationException("Type registry refresh set is already finished.");
        }
        try
        {
            for (; m_activatedCount < transactions.Count; m_activatedCount++)
            {
                transactions[m_activatedCount].Activate();
            }
        }
        catch (Exception failure) when (RetirementPendingException.Find(failure) is not null)
        {
            m_failure = failure;
            throw;
        }
        catch (Exception activationFailure)
        {
            try
            {
                Rollback();
            }
            catch (Exception rollbackFailure)
            {
                var combined = new AggregateException(
                    "Type registry activation and rollback failed.",
                    activationFailure,
                    rollbackFailure
                );
                if (RetirementPendingException.Find(combined) is not null)
                {
                    m_failure = combined;
                }
                throw combined;
            }
            throw;
        }
    }

    internal void Complete()
    {
        if (m_failure is not null)
        {
            throw m_failure;
        }
        if (m_finished)
        {
            return;
        }
        List<Exception> failures = [];
        for (int i = 0; i < transactions.Count; i++)
        {
            TryCleanup(transactions[i].Complete, failures);
        }
        m_finished = true;
        transactions = [];
        if (failures.Count > 0)
        {
            throw new AggregateException("Type registry completion failed after all participants were attempted.", failures);
        }
    }

    internal void Rollback()
    {
        if (m_failure is not null)
        {
            throw m_failure;
        }
        if (m_finished)
        {
            return;
        }
        List<Exception> failures = [];
        for (int i = transactions.Count - 1; i >= 0; i--)
        {
            TryCleanup(transactions[i].Rollback, failures);
        }
        m_finished = true;
        transactions = [];
        if (failures.Count > 0)
        {
            throw new AggregateException("Type registry rollback failed after all participants were attempted.", failures);
        }
    }

    private void TryCleanup(
        Action cleanup,
        List<Exception> failures
    ) {
        try
        {
            cleanup();
        }
        catch (Exception failure) when (RetirementPendingException.Find(failure) is not null)
        {
            m_failure = failures.Count == 0 ? failure : new AggregateException(
                "Registry cleanup remains pending after earlier failures.",
                [.. failures, failure]
            );
            if (ReferenceEquals(m_failure, failure))
            {
                throw;
            }
            throw m_failure;
        }
        catch (Exception exception)
        {
            failures.Add(exception);
        }
    }
}
