using System;
using System.Collections.Generic;
using Inno.Core.Execution;

namespace Inno.Extensibility.Modules;

internal sealed class AssemblyCatalogRefreshSet(IReadOnlyList<IAssemblyCatalogTransaction> transactions)
{
    private int m_activatedCount;
    private bool m_finished;
    private Exception? m_failure;

    internal IReadOnlyList<object> contexts
    {
        get
        {
            var result = new List<object>(transactions.Count);
            foreach (IAssemblyCatalogTransaction transaction in transactions)
            {
                if (transaction.context is not null)
                {
                    result.Add(transaction.context);
                }
            }
            return result;
        }
    }

    internal void Activate()
    {
        if (m_failure is not null)
        {
            throw m_failure;
        }
        if (m_finished)
        {
            throw new InvalidOperationException("Assembly catalog refresh set is already finished.");
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
                    "Assembly catalog activation and rollback failed.",
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
            throw new AggregateException("Assembly catalog completion failed after all participants were attempted.", failures);
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
            throw new AggregateException("Assembly catalog rollback failed after all participants were attempted.", failures);
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
                "Catalog cleanup remains pending after earlier failures.",
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
