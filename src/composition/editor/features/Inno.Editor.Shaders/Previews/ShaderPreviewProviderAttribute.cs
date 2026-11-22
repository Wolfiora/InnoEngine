using System;

namespace Inno.Editor.Shaders;

/// <summary>
/// Associates a reloadable preview implementation with one open Shader contract.
/// </summary>
/// <param name="contractID">
/// Non-empty domain-owned contract identity.
/// </param>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class ShaderPreviewProviderAttribute(string contractID) : Attribute
{
    /// <summary>
    /// Gets the non-empty Shader contract consumed by this preview.
    /// </summary>
    public string contractID { get; } = string.IsNullOrWhiteSpace(contractID)
                                        ? throw new ArgumentException("A preview contract is required.", nameof(contractID))
                                        : contractID;
}
