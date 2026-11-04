using System;
using System.Collections.Generic;
using System.Collections.Frozen;
using System.Collections.ObjectModel;
using System.Linq;

namespace Inno.UI;

/// <summary>
/// Describes optional behavior implemented by one UI backend generation.
/// </summary>
public sealed class UiBackendCapabilities
{
    private readonly IReadOnlyList<UiDocumentLanguageId> m_documentLanguages;
    private readonly IReadOnlySet<UiBackendCapability> m_capabilities;

    /// <summary>
    /// Creates an immutable capability description.
    /// </summary>
    /// <param name="documentLanguages">
    /// Explicit source languages accepted by the backend.
    /// </param>
    /// <param name="capabilities">
    /// Open stable identities of optional behaviors supported by this backend.
    /// </param>
    public UiBackendCapabilities(
        IEnumerable<UiDocumentLanguageId> documentLanguages,
        IEnumerable<UiBackendCapability> capabilities
    ) {
        ArgumentNullException.ThrowIfNull(documentLanguages);
        UiDocumentLanguageId[] languages = documentLanguages.Distinct().ToArray();
        if (languages.Length == 0 || languages.Any(static language => !language.isValid))
            throw new ArgumentException("A UI backend must declare at least one assigned document language.", nameof(documentLanguages));
        m_documentLanguages = new ReadOnlyCollection<UiDocumentLanguageId>(languages);
        ArgumentNullException.ThrowIfNull(capabilities);
        m_capabilities = capabilities.ToFrozenSet();
        if (m_capabilities.Any(static capability => !capability.isValid))
            throw new ArgumentException("Capabilities require assigned stable identities.", nameof(capabilities));
    }

    /// <summary>
    /// Gets source languages accepted by the backend.
    /// </summary>
    public IReadOnlyList<UiDocumentLanguageId> documentLanguages => m_documentLanguages;

    /// <summary>
    /// Gets the immutable optional feature identities.
    /// </summary>
    public IReadOnlySet<UiBackendCapability> capabilities => m_capabilities;

    /// <summary>
    /// Checks an exact optional capability identity.
    /// </summary>
    /// <param name="capability">
    /// The requested capability.
    /// </param>
    /// <returns>
    /// True when this backend explicitly supports it.
    /// </returns>
    public bool Supports(UiBackendCapability capability) => m_capabilities.Contains(capability);

    /// <summary>
    /// Determines whether an exact source language is supported.
    /// </summary>
    /// <param name="language">
    /// Language to test.
    /// </param>
    /// <returns>
    /// True when the language is registered by this backend.
    /// </returns>
    public bool Supports(UiDocumentLanguageId language) => m_documentLanguages.Contains(language);
}

/// <summary>
/// Reports that the selected backend cannot perform an explicitly requested optional operation.
/// </summary>
public sealed class UiCapabilityUnavailableException : NotSupportedException
{
    /// <summary>
    /// Creates a capability failure without naming a concrete implementation.
    /// </summary>
    /// <param name="capability">
    /// Unavailable neutral capability.
    /// </param>
    public UiCapabilityUnavailableException(UiBackendCapability capability)
        : base($"The selected UI backend does not support capability '{capability}'.")
        => this.capability = capability;

    /// <summary>
    /// Gets the unavailable capability.
    /// </summary>
    public UiBackendCapability capability { get; }
}
