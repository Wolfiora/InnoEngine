using System;
using System.Collections.Generic;

namespace Inno.Core.Collections;

internal interface IIndexedObjectIndex<T>
    where T : class
{
    /// <summary>
    /// Gets the human-readable name used for presentation and diagnostics.
    /// </summary>
    string name { get; }

    /// <summary>
    /// Gets the key type accepted by this index implementation.
    /// </summary>
    Type keyType { get; }

    /// <summary>
    /// Adds a new keyed value or atomically replaces the existing value for that key.
    /// </summary>
    /// <param name="item">
    /// The stored item associated with the validated handle.
    /// </param>
    /// <param name="key">
    /// The backend-neutral physical key associated with this event.
    /// </param>
    void AddOrUpdate(
        T item,
        object key
    );

    /// <summary>
    /// Removes the requested value while preserving the collection's invariants.
    /// </summary>
    /// <param name="item">
    /// The stored item associated with the validated handle.
    /// </param>
    void Remove(T item);

    /// <summary>
    /// Removes all retained entries and returns the instance to an empty reusable state.
    /// </summary>
    void Clear();

    /// <summary>
    /// Retrieves the requested count value from current authoritative state.
    /// </summary>
    /// <param name="key">
    /// The backend-neutral physical key associated with this event.
    /// </param>
    /// <returns>
    /// The scalar result calculated from the supplied inputs.
    /// </returns>
    int GetCount(object key);

    /// <summary>
    /// Attempts to get single without changing state when the operation cannot complete.
    /// </summary>
    /// <param name="key">
    /// The backend-neutral physical key associated with this event.
    /// </param>
    /// <param name="item">
    /// The stored item associated with the validated handle.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the requested condition is satisfied; otherwise, <see langword="false"/>.
    /// </returns>
    bool TryGetSingle(
        object key,
        out T? item
    );

    /// <summary>
    /// Determines whether current state contains the requested value value.
    /// </summary>
    /// <param name="key">
    /// The backend-neutral physical key associated with this event.
    /// </param>
    /// <param name="item">
    /// The stored item associated with the validated handle.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the requested condition is satisfied; otherwise, <see langword="false"/>.
    /// </returns>
    bool Contains(
        object key,
        T item
    );

    /// <summary>
    /// Retrieves the requested set value from current authoritative state.
    /// </summary>
    /// <param name="key">
    /// The backend-neutral physical key associated with this event.
    /// </param>
    /// <returns>
    /// The validated hash sett? that represents the completed operation.
    /// </returns>
    HashSet<T>? GetSet(object key);
}
