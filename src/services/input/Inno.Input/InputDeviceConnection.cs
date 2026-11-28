using System;
using Inno.Core.Identity;

namespace Inno.Input;

/// <summary>
/// Owns one device connection. Its identity distinguishes simultaneous devices and changes on reconnect.
/// </summary>
public abstract class InputDeviceConnection : IdentityObject, IDisposable
{
    /// <summary>
    /// Submits a control value in acquisition order on the session owner thread.
    /// </summary>
    /// <param name="controlID">
    /// A stable control name within the device protocol.
    /// </param>
    /// <param name="value">
    /// A finite scalar. Digital controls use zero for released and one for pressed.
    /// </param>
    public abstract void SetValue(
        string controlID,
        double value
    );

    /// <summary>
    /// Disconnects this instance and rejects further submissions.
    /// </summary>
    public abstract void Dispose();
}
