using System;

namespace Inno.Core.Collections;

/// <summary>
/// Internal store validation surface for IndexedObjectKey.
/// </summary>
internal interface IIndexedObjectStore
{
    /// <summary>
    /// Determines whether the key belongs to a live slot in the current storage generation.
    /// </summary>
    /// <param name="id">
    /// The stable identity used to locate the requested value.
    /// </param>
    /// <param name="keyType">
    /// The key type consumed by is valid key; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the requested condition is satisfied; otherwise, <see langword="false"/>.
    /// </returns>
    bool IsValidKey(
        int id,
        Type keyType
    );
}
