using System;

namespace Inno.UI;

/// <summary>
/// Reports that the selected backend cannot perform an explicitly requested optional operation.
/// </summary>
public sealed class UICapabilityUnavailableException : NotSupportedException
{
    /// <summary>
    /// Creates a capability failure without naming a concrete implementation.
    /// </summary>
    /// <param name="capability">
    /// Unavailable neutral capability.
    /// </param>
    public UICapabilityUnavailableException(UIBackendCapability capability)
        : base($"The selected UI backend does not support capability '{capability}'.")
        {
        this.capability = capability;
    }

    /// <summary>
    /// Gets the unavailable capability.
    /// </summary>
    public UIBackendCapability capability { get; }
}
