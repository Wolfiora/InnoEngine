using System;

namespace Inno.Editor.Interactions;
/// <summary>
/// Owns one continuous edit transaction without retaining edited objects, setters or extension instances.
/// </summary>
public sealed class EditorEditGesture : IDisposable
{
    private EditorHistoryTransaction? m_transaction;
    /// <summary>
    /// Begins an edit whose updates use the existing domain mutation and History protocols.
    /// </summary>
    /// <param name="history">
    /// The authoritative History for the current editor session.
    /// </param>
    /// <param name="name">
    /// The operation name displayed by Undo and Redo after a successful commit.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The History owner is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// History cannot begin a transaction in its current state.
    /// </exception>
    public EditorEditGesture(
        IEditorHistory history,
        string name
    ) {
        ArgumentNullException.ThrowIfNull(history);
        m_transaction = history.BeginTransaction(name);
        mergeKey = "gesture:" + m_transaction.id.ToString("N");
    }

    /// <summary>
    /// Gets the transient merge boundary shared only by updates within this gesture.
    /// </summary>
    public string mergeKey { get; }
    /// <summary>
    /// Gets whether this gesture still owns an active transaction, including a rejected completion.
    /// </summary>
    public bool isActive => m_transaction is not null;

    /// <summary>
    /// Commits the edit exactly once; rejected completion leaves the transaction available for retry.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// The gesture is finished or a nested transaction remains active.
    /// </exception>
    public void Commit()
    {
        EditorHistoryTransaction transaction = RequireTransaction();
        transaction.Commit();
        m_transaction = null;
        transaction.Dispose();
    }

    /// <summary>
    /// Attempts to restore the state before the gesture without discarding a failed rollback.
    /// </summary>
    /// <returns>
    /// The rollback outcome; a state-preserving failure keeps this gesture active for retry.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The gesture is finished or a nested transaction remains active.
    /// </exception>
    public EditorHistoryResult Cancel()
    {
        EditorHistoryTransaction transaction = RequireTransaction();
        EditorHistoryResult result = transaction.Rollback();
        if (result.succeeded)
        {
            m_transaction = null;
            transaction.Dispose();
        }

        return result;
    }

    /// <summary>
    /// Cancels an unfinished gesture and releases its ownership only after rollback succeeds.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Rollback is blocked or a nested transaction remains active.
    /// </exception>
    public void Dispose()
    {
        if (m_transaction is null)
            return;
        EditorHistoryResult result = Cancel();
        if (!result.succeeded)
            throw new InvalidOperationException($"The edit gesture could not roll back: {result.message}");
        GC.SuppressFinalize(this);
    }

    private EditorHistoryTransaction RequireTransaction() => m_transaction ?? throw new InvalidOperationException("The edit gesture has already finished.");
}
