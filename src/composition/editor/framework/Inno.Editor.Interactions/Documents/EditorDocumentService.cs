using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Inno.Core.Events;

namespace Inno.Editor.Interactions;

internal sealed class EditorDocumentService : IEditorDocumentService
{
    private readonly List<EditorDocumentContext> m_documents = [];
    private readonly Dictionary<string, EditorDocumentProvider> m_providers = new(StringComparer.Ordinal);
    private readonly EventDispatcher m_events;

    internal EditorDocumentService(EventDispatcher events)
    {
        m_events = events;
    }

    /// <summary>
    /// Gets an immutable snapshot of currently owned source documents.
    /// </summary>
    public IReadOnlyList<EditorDocumentContext> documents => m_documents.ToArray();

    /// <summary>
    /// Registers one current-generation provider and reconnects its retained documents.
    /// </summary>
    /// <param name="provider">
    /// Provider instance owned by the returned registration lease.
    /// </param>
    /// <returns>
    /// A lease that unregisters the exact provider instance when disposed.
    /// </returns>
    public IDisposable RegisterProvider(EditorDocumentProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider);
        ArgumentException.ThrowIfNullOrWhiteSpace(provider.id);
        if (!m_providers.TryAdd(provider.id, provider))
        {
            throw new InvalidOperationException($"Editor document provider '{provider.id}' is already registered.");
        }
        try
        {
            foreach (EditorDocumentContext document in m_documents.Where(document => document.providerID == provider.id).ToArray())
            {
                if (!m_documents.Contains(document))
                {
                    continue;
                }
                document.isProviderAvailable = true;
                provider.Open(document);
            }
        }
        catch
        {
            Unregister(provider);
            throw;
        }

        return new ProviderLease(this, provider);
    }

    /// <summary>
    /// Opens the requested resource and establishes its owned lifetime.
    /// </summary>
    /// <param name="assetPath">
    /// The asset path text validated by the open operation.
    /// </param>
    /// <param name="assetID">
    /// Persistent asset identity used for single-instance matching, or an empty value to match by path.
    /// </param>
    /// <returns>
    /// The existing matching context or a newly opened document context.
    /// </returns>
    public EditorDocumentContext Open(
        string assetPath,
        Guid assetID = default
    ) {
        string normalizedPath = NormalizePath(assetPath);
        EditorDocumentContext? identityOwner = assetID == Guid.Empty
                                               ? null
                                               : m_documents.FirstOrDefault(document => document.assetID == assetID);
        EditorDocumentContext? pathOwner = FindPathOwner(normalizedPath);
        if (identityOwner is not null && pathOwner is not null && !ReferenceEquals(identityOwner, pathOwner))
        {
            RetireStalePathOwner(pathOwner, normalizedPath);
        }
        EditorDocumentContext? existing = identityOwner ?? pathOwner;
        if (existing is not null && assetID != Guid.Empty && existing.assetID != assetID)
        {
            RetireStalePathOwner(existing, normalizedPath);
            existing = null;
        }

        if (existing is not null)
        {
            _ = UpdateAssetPath(existing.documentID, normalizedPath);
            return existing;
        }

        EditorDocumentProvider provider = m_providers.Values.OrderBy(
            static candidate => candidate.id,
            StringComparer.Ordinal
        ).FirstOrDefault(candidate => candidate.CanOpen(normalizedPath))
            ?? throw new InvalidOperationException($"No editor document provider can open '{normalizedPath}'.");
        var context = new EditorDocumentContext(Guid.NewGuid(), assetID, normalizedPath, provider.id)
        {
            isProviderAvailable = true
        };
        m_documents.Add(context);
        try
        {
            provider.Open(context);
        }
        catch
        {
            m_documents.Remove(context);
            throw;
        }

        return context;
    }

    /// <summary>
    /// Updates asset path state from the current authoritative inputs.
    /// </summary>
    /// <param name="documentID">
    /// The document id consumed by update asset path; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="assetPath">
    /// The asset path text validated by the update asset path operation.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the operation succeeds or its condition is satisfied; otherwise, <see langword="false"/>.
    /// </returns>
    public bool UpdateAssetPath(
        Guid documentID,
        string assetPath
    ) {
        EditorDocumentContext? document = Find(documentID);
        if (document is null)
        {
            return false;
        }
        string path = NormalizePath(assetPath);
        if (m_providers.TryGetValue(document.providerID, out EditorDocumentProvider? provider) && !provider.CanOpen(path))
        {
            throw new ArgumentException("The new source path is not supported by this document provider.", nameof(assetPath));
        }
        EditorDocumentContext? pathOwner = FindPathOwner(path, documentID);
        if (pathOwner is not null)
        {
            RetireStalePathOwner(pathOwner, path);
        }
        bool defaultTitle = document.title == Path.GetFileName(document.assetPath);
        document.assetPath = path;
        if (defaultTitle)
        {
            document.title = Path.GetFileName(path);
        }
        return true;
    }

    /// <summary>
    /// Updates whether an open document differs from its saved authoring baseline.
    /// </summary>
    /// <param name="documentID">
    /// Stable identity of the open document to mark.
    /// </param>
    /// <param name="isDirty">
    /// True when the provider retains unsaved changes.
    /// </param>
    public void SetDirty(
        Guid documentID,
        bool isDirty = true
    ) => Get(documentID).isDirty = isDirty;

    /// <summary>
    /// Persists the supplied value through the configured storage contract.
    /// </summary>
    /// <param name="documentID">
    /// Stable identity of the open document to save.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the owning provider saved the document.
    /// </returns>
    public bool Save(Guid documentID)
        => Invoke(documentID, static (
            provider,
            context
        ) => provider.Save(context), clearDirty: true);

    /// <summary>
    /// Saves every dirty document through its owning provider.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> when every dirty document was saved.
    /// </returns>
    public bool SaveAll()
    {
        bool succeeded = true;
        foreach (EditorDocumentContext document in m_documents.ToArray())
        {
            if (document.isDirty && !Save(document.documentID))
            {
                succeeded = false;
            }
        }

        return succeeded;
    }

    /// <summary>
    /// Applies a validated change atomically at the caller-controlled commit point.
    /// </summary>
    /// <param name="documentID">
    /// Stable identity of the open document whose staged state should be applied.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the owning provider applied the staged state.
    /// </returns>
    public bool Apply(Guid documentID)
        => Invoke(documentID, static (
            provider,
            context
        ) => provider.Apply(context), clearDirty: true);

    /// <summary>
    /// Reverts one open document through its owning provider.
    /// </summary>
    /// <param name="documentID">
    /// Stable identity of the open document to revert.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the owning provider restored the saved state.
    /// </returns>
    public bool Revert(Guid documentID)
        => Invoke(documentID, static (
            provider,
            context
        ) => provider.Revert(context), clearDirty: true);

    /// <summary>
    /// Closes the requested resource and releases its operation-scoped state.
    /// </summary>
    /// <param name="documentID">
    /// Stable identity of the open document to close.
    /// </param>
    /// <param name="mode">
    /// Explicit policy for handling dirty state.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the document closed under the requested dirty-state policy.
    /// </returns>
    public bool Close(
        Guid documentID,
        EditorDocumentCloseMode mode
    ) {
        m_events.Emit(new EditorOperationPreparingEvent());
        EditorDocumentContext? document = Find(documentID);
        if (document is null || mode == EditorDocumentCloseMode.Cancel)
        {
            return false;
        }
        if (document.isDirty && mode == EditorDocumentCloseMode.Save && !Save(documentID))
        {
            return false;
        }
        if (document.isDirty && mode != EditorDocumentCloseMode.Discard && mode != EditorDocumentCloseMode.Save)
        {
            return false;
        }
        if (m_providers.TryGetValue(document.providerID, out EditorDocumentProvider? provider))
        {
            provider.Close(document);
        }
        m_documents.Remove(document);
        return true;
    }

    internal void Shutdown()
    {
        foreach (EditorDocumentContext document in m_documents.ToArray())
        {
            if (m_providers.TryGetValue(document.providerID, out EditorDocumentProvider? provider))
            {
                provider.Close(document);
            }
        }

        m_documents.Clear();
        m_providers.Clear();
    }

    private bool Invoke(
        Guid documentID,
        Func<EditorDocumentProvider, EditorDocumentContext, bool> operation,
        bool clearDirty
    ) {
        m_events.Emit(new EditorOperationPreparingEvent());
        EditorDocumentContext document = Get(documentID);
        if (!m_providers.TryGetValue(document.providerID, out EditorDocumentProvider? provider))
        {
            return false;
        }
        bool succeeded = operation(provider, document);
        if (succeeded && clearDirty)
        {
            document.isDirty = false;
        }
        return succeeded;
    }

    private EditorDocumentContext Get(Guid documentID)
        => Find(documentID) ?? throw new ArgumentException($"Editor document '{documentID}' is not open.", nameof(documentID));

    private EditorDocumentContext? Find(Guid documentID) => m_documents.FirstOrDefault(document => document.documentID == documentID);

    private EditorDocumentContext? FindPathOwner(
        string assetPath,
        Guid excludedDocumentID = default
    ) => m_documents.FirstOrDefault(
        document => document.documentID != excludedDocumentID && string.Equals(document.assetPath, assetPath, StringComparison.Ordinal)
    );

    private void RetireStalePathOwner(
        EditorDocumentContext document,
        string destinationPath
    ) {
        if (document.isDirty)
        {
            throw new InvalidOperationException(
                $"Open document '{document.title}' has unsaved changes for '{destinationPath}'. " + "Save, revert, or close it before assigning that source path to another asset identity."
            );
        }

        if (m_providers.TryGetValue(document.providerID, out EditorDocumentProvider? provider))
        {
            provider.Close(document);
        }
        if (!m_documents.Remove(document))
        {
            return;
        }
    }

    private void Unregister(EditorDocumentProvider provider)
    {
        if (!m_providers.TryGetValue(provider.id, out EditorDocumentProvider? registered) || !ReferenceEquals(provider, registered))
        {
            return;
        }

        m_providers.Remove(provider.id);
        foreach (EditorDocumentContext document in m_documents.Where(document => document.providerID == provider.id))
        {
            document.isProviderAvailable = false;
        }
    }

    private static string NormalizePath(string assetPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(assetPath);
        string normalized = assetPath.Replace('\\', '/').Trim();
        while (normalized.StartsWith("./", StringComparison.Ordinal))
        {
            normalized = normalized[2..];
        }
        return normalized;
    }

    private sealed class ProviderLease(
        EditorDocumentService owner,
        EditorDocumentProvider provider
    ) : IDisposable
    {
        private EditorDocumentService? m_owner = owner;

        /// <summary>
        /// Releases the resources owned by this instance.
        /// </summary>
        public void Dispose()
        {
            EditorDocumentService? current = m_owner;
            m_owner = null;
            current?.Unregister(provider);
        }
    }
}
