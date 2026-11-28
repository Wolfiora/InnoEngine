using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Inno.Core.Identity;

namespace Inno.Input;

/// <summary>
/// Freezes the current control values for one connected instance at a frame boundary.
/// </summary>
public sealed class InputDeviceState
{
    /// <summary>
    /// Copies the control values so future input cannot mutate a retained frame.
    /// </summary>
    /// <param name="instance">
    /// The domain-qualified connection identity.
    /// </param>
    /// <param name="typeID">
    /// The stable device protocol identifier.
    /// </param>
    /// <param name="controls">
    /// The latest values for controls observed on this instance.
    /// </param>
    public InputDeviceState(
        RuntimeIdentity instance,
        string typeID,
        IReadOnlyDictionary<string, double> controls
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(typeID);
        ArgumentNullException.ThrowIfNull(controls);
        this.instance = instance;
        this.typeID = typeID;
        this.controls = new ReadOnlyDictionary<string, double>(new Dictionary<string, double>(controls));
    }

    /// <summary>
    /// Gets the connection identity valid for this instance's lifetime.
    /// </summary>
    public RuntimeIdentity instance { get; }

    /// <summary>
    /// Gets the stable device protocol identifier.
    /// </summary>
    public string typeID { get; }

    /// <summary>
    /// Gets the latest values keyed by control identifier.
    /// </summary>
    public IReadOnlyDictionary<string, double> controls { get; }
}
