using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Rendering;

/// <summary>
/// Stores the manifest-derived binding contract verified after backend program creation.
/// </summary>
public sealed class ShaderInterface
{
    /// <summary>
    /// Creates a shader interface contract.
    /// </summary>
    /// <param name="bindings">
    /// Stable expected bindings.
    /// </param>
    public ShaderInterface(IReadOnlyList<ShaderInterfaceBinding> bindings)
    {
        ArgumentNullException.ThrowIfNull(bindings);
        this.bindings = Array.AsReadOnly(bindings.ToArray());
    }

    /// <summary>
    /// Gets stable expected bindings.
    /// </summary>
    public IReadOnlyList<ShaderInterfaceBinding> bindings { get; }

}

