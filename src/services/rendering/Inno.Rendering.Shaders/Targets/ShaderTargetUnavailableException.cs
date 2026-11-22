using System;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Identifies an authored target that is absent from the current extension generation.
/// </summary>
public sealed class ShaderTargetUnavailableException : InvalidOperationException
{
    /// <summary>
    /// Creates a missing-target diagnostic without retaining extension objects.
    /// </summary>
    /// <param name="targetID">
    /// The stable authored target identity.
    /// </param>
    public ShaderTargetUnavailableException(string targetID)
        : base($"Shader target '{targetID}' is unavailable. The authored graph is retained for recovery.")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(targetID);
        this.targetID = targetID;
    }

    /// <summary>
    /// Gets the stable target identity required by the authored graph.
    /// </summary>
    public string targetID { get; }
}
